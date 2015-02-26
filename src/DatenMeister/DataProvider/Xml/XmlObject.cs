using BurnSystems.Test;
using Ninject;
using DatenMeister;
using DatenMeister.DataProvider.Common;
using DatenMeister.Entities.AsObject.Uml;
using DatenMeister.Logic;
using DatenMeister.Logic.TypeResolver;
using DatenMeister.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Defines one xml object being used by Datenmeister
    /// </summary>
    public class XmlObject : IElement, IHasFactoryExtent, IKnowsExtentType
    {
        /// <summary>
        /// Gets or sets the extent, whose extent was used to create the item
        /// </summary>
        public XmlExtent FactoryExtent
        {
            get;
            set;
        }

        IURIExtent IHasFactoryExtent.FactoryExtent
        {
            get { return this.FactoryExtent; }
        }

        /// <summary>
        /// Gets or sets the extent, in which the element is contained to
        /// </summary>
        public XmlExtent ContainerExtent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the node
        /// </summary>
        public XElement Node
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parents node
        /// </summary>
        public XmlObject Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Xml Object
        /// </summary>
        /// <param name="node">Node being used</param>
        /// <param name="parent">The parent node, which is used for id derivation</param>
        public XmlObject(XmlExtent xmlExtent,  XElement node, XmlObject parent = null)
        {
            Ensure.That(node != null);
            this.Node = node;
            this.Parent = parent;
            this.FactoryExtent = xmlExtent;

            // Check, if we have an id attribute
            // TODO: Check with RFC about xml.
            var idAttribute = this.Node.Attribute("id");
            if (idAttribute != null)
            {
                this.Id = idAttribute.Value;
            }
        }

        /// <summary>
        /// Resolves the object to be returned
        /// </summary>
        /// <param name="value">Value to be resolved</param>
        /// <returns>Resolved object</returns>
        private object Resolve(object value, string propertyName, RequestType requestType)
        {
            switch (requestType)
            {
                case RequestType.AsDefault:
                    var valueAsList = value as IList;
                    var multiple = valueAsList != null ? true : valueAsList.Count > 1;
                    return this.Resolve(
                        value, 
                        propertyName, 
                        multiple ? RequestType.AsReflectiveCollection : RequestType.AsSingle);
                case RequestType.AsSingle:
                    return value;
                case RequestType.AsReflectiveCollection:
                    return new XmlReflectiveSequence(this, propertyName);
                default:
                    throw new InvalidOperationException("Unknown result type: " + requestType.ToString());
            }
        }

        /// <summary>
        /// Gets the property by name
        /// </summary>
        /// <param name="propertyName">Name of the property to be retrieved</param>
        /// <returns>Retrieval a property</returns>
        public object get(string propertyName, RequestType requestType = RequestType.AsDefault)
        {
            var result = new List<object>();
            if (string.IsNullOrEmpty(propertyName))
            {
                // If empty, return just the content of the Xml-Node
                result.Add(this.Node.Value);
            }
            else
            {
                // Checks, if we have an attribute with the given name
                var xmlProperty = this.Node.Attribute(propertyName);
                if (xmlProperty != null)
                {
                    result.Add(xmlProperty.Value);
                }

                // Checks, if we have a reference attribute with the given name
                else
                {
                    var xmlRefProperty = this.Node.Attribute(propertyName + "-ref");
                    if (xmlRefProperty != null)
                    {
                        foreach (var partProperty in xmlRefProperty.Value.Split(new char[] { ' ' }))
                        {
                            result.Add(new ResolvableByPath(this.FactoryExtent.Pool, this, partProperty));
                        }
                    }
                }

                // Checks, if we have elements nodes with the given property name
                foreach (var element in this.Node.Elements(propertyName))
                {
                    result.Add(new XmlObject(this.FactoryExtent, element, this)
                    {
                        ContainerExtent = this.ContainerExtent
                    });
                }
            }

            // Checks, if we have an item, if not, return a not set element
            if (result.Count == 0)
            {
                return this.Resolve(ObjectHelper.NotSet, propertyName, requestType);
            }

            return this.Resolve(
                result,
                propertyName,
                requestType);
        }

        /// <summary>
        /// Checks, whether the given name is internal and shall not 
        /// be exported to the outside world. 
        /// These are XMI-Namespaces and XmlNs
        /// </summary>
        /// <param name="name">Name to be checked</param>
        /// <returns>true,if this is external</returns>
        private static bool IsInternalNamespace(XName name)
        {
            if (name.Namespace == DatenMeister.Entities.AsObject.Uml.Types.XmiNamespace
                || name.Namespace == DatenMeister.Entities.AsObject.Uml.Types.XmlNamespace)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all properties by name and pair
        /// </summary>
        /// <returns>Enumeration of property pairs</returns>
        public IEnumerable<ObjectPropertyPair> getAll()
        {
            if (!string.IsNullOrEmpty(this.Node.Value))
            {
                yield return new ObjectPropertyPair(string.Empty, this.Node.Value);
            }

            var returns = new Dictionary<string, List<object>>();
            List<object> found;

            // Go through the elements
            foreach (var element in this.Node.Elements())
            {
                if (!returns.TryGetValue(element.Name.ToString(), out found))
                {
                    found = new List<object>();
                    returns[element.Name.ToString()] = found;
                }

                found.Add(new XmlObject(this.FactoryExtent, element, this)
                    {
                        ContainerExtent = this.ContainerExtent
                    });
            }

            foreach (var attribute in this.Node.Attributes())
            {
                // Checks, if the attribute has a -ref as ending. If yes, then it is a reference
                // to another object
                var attributeName = attribute.Name.ToString();
                if (IsInternalNamespace(attribute.Name))
                {
                    continue;
                }

                object attributeValue = attribute.Value;

                if (attributeName.EndsWith("-ref"))
                {
                    // Ok, we got a reference
                    attributeName = attributeName.Substring(0, attributeName.Length - "-ref".Length);
                    attributeValue = new ResolvableByPath(this.FactoryExtent.Pool, this, attributeValue.ToString());
                }

                if (!returns.TryGetValue(attributeName, out found))
                {
                    found = new List<object>();
                    returns[attributeName] = found;
                }

                found.Add(attributeValue);
            }

            // Ok, got all the elements and attributes
            foreach (var valuePair in returns)
            {
                if (valuePair.Value.Count == 1)
                {
                    yield return new ObjectPropertyPair(
                        valuePair.Key, 
                        this.Resolve(valuePair.Value.First(), valuePair.Key, RequestType.AsDefault));
                }
                else
                {
                    yield return new ObjectPropertyPair(
                        valuePair.Key,
                        this.Resolve(valuePair.Value, valuePair.Key, RequestType.AsDefault));
                }
            }
        }

        /// <summary>
        /// Checks, if the property has been set
        /// </summary>
        /// <param name="propertyName">Name of the property to be checked</param>
        /// <returns>true, if this property exists</returns>
        public bool isSet(string propertyName)
        {
            if (propertyName.EndsWith("-ref"))
            {
                throw new InvalidOperationException("Property name may not end with '-ref'");
            }

            // Checks, if we have a value
            var result = this.getAsSingle(propertyName);
            if (ObjectConversion.IsNull(result))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Sets the property
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void set(string propertyName, object value)
        {
            // If property name is an id, we use this id for setting the id of the entity
            if (propertyName == "id")
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    throw new ArgumentException("Id cannot be set to null or empty");
                }

                if (value.ToString().Contains(" "))
                {
                    throw new ArgumentException("Id may not contain an empty space");
                }

                this.Id = value.ToString();
            }

            if (propertyName.EndsWith("-ref"))
            {
                throw new InvalidOperationException("Property name may not end with '-ref'");
            }

            // Finds the container for the property
            if (string.IsNullOrEmpty(propertyName))
            {
                // Content of the node will be set
                if (ObjectConversion.IsNative(value))
                {
                    if (value == null)
                    {
                        // Setting of empty content
                        this.Node.Value = string.Empty;
                    }
                    else
                    {
                        // Setting of node content by value
                        this.Node.Value = ObjectConversion.ToString(value);
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        "Assigning elements to empty property is not supported");
                }
            }            
            else if (this.Node.Attribute(propertyName) != null)
            {
                // Checks, if we have multiple objects, if yes, throw exception. 
                // Check, if we have an attribute with the given name, if yes, set the property     
                if (value == ObjectHelper.NotSet)
                {
                    this.Node.Attribute(propertyName).Remove();
                }
                else
                {
                    this.Node.Attribute(propertyName).Value = ObjectConversion.ToString(value);
                }
            }
            else if (this.Node.Element(propertyName) != null)
            {
                // Element is an element
                if (value == ObjectHelper.NotSet)
                {
                    this.Node.Element(propertyName).Remove();
                }
                else
                {
                    if (ObjectConversion.IsNative(value))
                    {
                        this.Node.Element(propertyName).Value = ObjectConversion.ToString(value);
                    }
                }
            }
            else if (ObjectConversion.IsNative(value) || ObjectConversion.IsEnum(value))
            {
                // Ok, we have no attribute and no element with the name.
                // If this is a simple type, we just assume that this is a property, otherwise no suppurt
                this.Node.Add(new XAttribute(propertyName, ObjectConversion.ToString(value)));
            }
            else if (value is IObject)
            {
                var valueAsIObject = value as IObject;
                var extent = this.FactoryExtent;
                if (valueAsIObject.Extent == null)
                {
                    XmlObject.CopyObjectIntoXmlNode(this, valueAsIObject, propertyName, extent);
                }
                else
                {
                    // Set as an attribute and reference
                    var poolResolver = PoolResolver.GetDefault(extent.Pool);
                    var path = poolResolver.GetResolvePath(valueAsIObject, this);

                    // Checks, if attribute is existing
                    var attribute = this.Node.Attribute(propertyName + "-ref");
                    if (attribute == null)
                    {
                        attribute = new XAttribute(propertyName + "-ref", path);
                        this.Node.Add(attribute);
                    }
                    else
                    {
                        attribute.Value = path;
                    }
                }
            }
            else if (value is IEnumerable || value is IReflectiveCollection)
            {
                var propertyAsReflectiveCollection = this.getAsReflectiveSequence(propertyName);
                foreach (var item in (value as IEnumerable))
                {
                    propertyAsReflectiveCollection.add(item);
                }
            }
            else if (value == ObjectHelper.NotSet)
            {
                // do Nothing.. Nothing necessary, will not create a new element or attribute
            }
            else
            {
                throw new NotImplementedException("We do not know how to set the value to the XmlObject");
            }

            this.MakeExtentDirty();
        }

        /// <summary>
        /// Removes the properties
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public bool unset(string propertyName)
        {
            throw new NotImplementedException("unset is not defined until now");
        }

        /// <summary>
        /// Deletes the node from the extent
        /// </summary>
        public void delete()
        {
            this.Node.Remove();
            this.ContainerExtent = null;
            this.MakeExtentDirty();
        }

        /// <summary>
        /// Gets or sets the id of the given xml object
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Makes the factory dirty
        /// </summary>
        public void MakeExtentDirty()
        {
            if (this.ContainerExtent != null)
            {
                this.ContainerExtent.IsDirty = true;
            }
        }

        /// <summary>
        /// Returns the meta class, if known
        /// </summary>
        /// <returns>Found Metaclass or null, if not found</returns>
        public IObject getMetaClass()
        {
            // Checks the type by xmi:type attribute
            var xmiTypeAttribute = this.Node.Attribute(DatenMeister.Entities.AsObject.Uml.Types.XmiNamespace + "type");
            if (xmiTypeAttribute != null)
            {
                var typeName = xmiTypeAttribute.Value;

                // First, try it via the local TypeResolver,
                if (this.FactoryExtent != null)
                {
                    var localTypeResolver = this.FactoryExtent.Injection.TryGet<ITypeResolver>();
                    if (localTypeResolver != null)
                    {
                        var localType = localTypeResolver.GetType(typeName);
                        if (localType != null)
                        {
                            return localType;
                        }
                    }
                }

                // If it does not work, try it via global TypeResolver

                var typeResolver = Injection.Application.Get<ITypeResolver>();
                var type = typeResolver.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            var nodeName = this.Node.Name.ToString();

            // Checks the type by mapping
            if (this.FactoryExtent != null)
            {
                var info = this.FactoryExtent.Settings.Mapping.FindByNodeName(nodeName);
                if (info != null)
                {
                    return info.Type;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the object, which contains this element
        /// </summary>
        /// <returns>Object, which is returned</returns>
        public IObject container()
        {
            return this.Parent;
        }

        IURIExtent IObject.Extent
        {
            get { return this.ContainerExtent; }
        }

        /// <summary>
        /// Copies the object into the given Xml node. 
        /// The Xml node already has to exist and a created node, according to propertyname, will be attached to this. 
        /// </summary>
        /// <param name="xmlObject">Xmlobject, where new node will be attached</param>
        /// <param name="valueAsIObject">The object being copied to the xml node</param>
        /// <param name="propertyName">Name of the property being </param>
        /// <param name="extent"></param>
        public static void CopyObjectIntoXmlNode(XmlObject xmlObject, IObject valueAsIObject, string propertyName, IURIExtent extent)
        {
            var copier = new ObjectCopier(extent);
            var copiedXmlObject = copier.CopyElement(valueAsIObject) as XmlObject;
            copiedXmlObject.ContainerExtent = xmlObject.ContainerExtent;
            Ensure.That(copiedXmlObject != null);

            // Renames node to the propertyname
            copiedXmlObject.Node.Name = propertyName;

            xmlObject.Node.Add(copiedXmlObject.Node);
        }

        /// <summary>
        /// Returns the Extent Type
        /// </summary>
        System.Type IKnowsExtentType.ExtentType
        {
            get { return typeof(XmlExtent); }
        }

        /// <summary>
        /// Checks, if the nodes are equest
        /// </summary>
        /// <param name="obj">Object to be done</param>
        /// <returns>true, if the same</returns>
        public override bool Equals(object obj)
        {
            var xmlObj = obj as XmlObject;
            if (xmlObj != null)
            {
                return xmlObj.Node == this.Node;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.Node == null)
            {
                return 0;
            }

            return this.Node.GetHashCode();
        }

        public override string ToString()
        {
            var nameElement = this.getAsSingle("name");
            string nameText = string.Empty;
            if (nameElement != null )
            {
                nameText =
                    string.Format(
                        " ({0})",
                        ObjectConversion.ToString(nameElement));
            }

            if (this.Id == null)
            {
                return base.ToString() + nameText;
            }
            else
            {
                return "XmlObject: " + this.Id.ToString() + nameText;
            }
        }
    }
}
