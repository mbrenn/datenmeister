using BurnSystems.ObjectActivation;
using BurnSystems.Serialization;
using BurnSystems.Test;
using DatenMeister;
using DatenMeister.DataProvider.Common;
using DatenMeister.Logic;
using DatenMeister.Pool;
using System;
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
    public class XmlObject : IElement
    {
        /// <summary>
        /// Gets or sets the extent, where this element belongs to
        /// </summary>
        public XmlExtent Extent
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
            Ensure.That(xmlExtent != null);
            Ensure.That(node != null);
            this.Node = node;
            this.Parent = parent;
            this.Extent = xmlExtent;

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
        private object Resolve(object value, string propertyName)
        {
            var result = this.Extent.Pool.Resolve(this, value);
            if (result != null)
            {
                result = new XmlUnspecified(this, propertyName, result);
            }

            return result;
        }

        /// <summary>
        /// Gets the property by name
        /// </summary>
        /// <param name="propertyName">Name of the property to be retrieved</param>
        /// <returns>Retrieval a property</returns>
        public object get(string propertyName)
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
                        result.Add(new ResolvableByPath(xmlRefProperty.Value));
                    }
                }

                // Checks, if we have elements nodes with the given property name
                foreach (var element in this.Node.Elements(propertyName))
                {
                    result.Add(new XmlObject(this.Extent, element, this));
                }
            }

            // Checks, if we have an item, if not, return a not set element
            if (result.Count == 0)
            {
                return this.Resolve(ObjectHelper.NotSet, propertyName);
            }

            return this.Resolve(result, propertyName);
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

                found.Add(new XmlObject(this.Extent, element, this));
            }

            foreach (var attribute in this.Node.Attributes())
            {
                // Checks, if the attribute has a -ref as ending. If yes, then it is a reference
                // to another object
                var attributeName = attribute.Name.ToString();
                object attributeValue = attribute.Value;

                if (attributeName.EndsWith("-ref"))
                {
                    // Ok, we got a reference
                    attributeName = attributeName.Substring(0, attributeName.Length - "-ref".Length);
                    attributeValue = new ResolvableByPath(attributeValue.ToString());
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
                    yield return new ObjectPropertyPair(valuePair.Key, this.Resolve(valuePair.Value.First(), valuePair.Key));
                }
                else
                {
                    yield return new ObjectPropertyPair(valuePair.Key, this.Resolve(valuePair.Value, valuePair.Key));
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
            var result = this.get(propertyName).AsSingle();
            if (result == null)
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
            if (propertyName.EndsWith("-ref"))
            {
                throw new InvalidOperationException("Property name may not end with '-ref'");
            }

            // Finds the container for the property
            if (string.IsNullOrEmpty(propertyName))
            {
                // Content of the node will be set
                if (Extensions.IsNative(value))
                {
                    if (value == null)
                    {
                        // Setting of empty content
                        this.Node.Value = string.Empty;
                        this.Extent.IsDirty = true;
                    }
                    else
                    {
                        // Setting of node content by value
                        this.Node.Value = value.ToString();
                        this.Extent.IsDirty = true;
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
                this.Node.Attribute(propertyName).Value = value.ToString();
                this.Extent.IsDirty = true;
            }
            else if (this.Node.Element(propertyName) != null)
            {
                // Element is an element
                if (Extensions.IsNative(value))
                {
                    this.Node.Element(propertyName).Value = value.ToString();
                    this.Extent.IsDirty = true;
                }
            }
            else if (Extensions.IsNative(value))
            {
                // Ok, we have no attribute and no element with the name.
                // If this is a simple type, we just assume that this is a property, otherwise no suppurt
                this.Node.Add(new XAttribute(propertyName, value.ToString()));
                this.Extent.IsDirty = true;
            }
            else if (value is IObject)
            {
                var valueAsIObject = value as IObject;
                // Set as an attribute and reference
                var poolResolver = PoolResolver.GetDefault(this.Extent.Pool);
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
            else
            {
                throw new NotImplementedException("We do not know how to set the value to the XmlObject");
            }

            // If property name is an id, we use this id for setting the id of the entity
            if (propertyName == "id")
            {
                this.Id = value.ToString();
            }
        }

        /// <summary>
        /// Removes the properties
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public bool unset(string propertyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the node from the extent
        /// </summary>
        public void delete()
        {
            this.Node.Remove();
            this.Extent.IsDirty = true;
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
        /// Returns the meta class, if known
        /// </summary>
        /// <returns>Found Metaclass or null, if not found</returns>
        public IObject getMetaClass()
        {
            var nodeName = this.Node.Name.ToString();
            var info = this.Extent.Settings.Mapping.FindByNodeName(nodeName);
            if (info == null)
            {
                return null;
            }

            return info.Type;
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
            get { return this.Extent; }
        }
    }
}
