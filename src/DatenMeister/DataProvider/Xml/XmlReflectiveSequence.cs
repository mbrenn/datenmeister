using BurnSystems.ObjectActivation;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Defines the type of the sequence type
    /// and describes where the objects are stored. 
    /// </summary>
    public enum XmlReflectiveSequenceType
    {
        /// <summary>
        /// It is currently not known, whether we add attribute or nodes
        /// </summary>
        Unknown, 

        /// <summary>
        /// The xml reflective sequence stores its values into the attributes
        /// Used for references to other objects
        /// </summary>
        Attributes,

        /// <summary>
        /// The xml reflective sequence stores its values into the nodes. 
        /// Used for contained objects and native objects
        /// </summary>
        Nodes
    }

    /// <summary>
    /// Implements the Reflective Sequence Collection for Xml-Types
    /// </summary>
    public class XmlReflectiveSequence : BaseReflectiveSequence
    {
        /// <summary>
        /// Gets or sets the sequence type
        /// </summary>
        public XmlReflectiveSequenceType sequenceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unspecified object
        /// </summary>
        private XmlUnspecified Unspecified
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the XmlReflectiveSequence
        /// </summary>
        /// <param name="unspecified">Unspecified object</param>
        public XmlReflectiveSequence(XmlUnspecified unspecified)
        {
            this.Unspecified = unspecified;
            this.EstimateSequenceType();
        }

        #region Functions for estimation of sequence type

        /// <summary>
        /// Estimates the type of the sequence. Might be driven by
        /// Xml-Attributes or might be driven by Xml-Nodes.
        /// A reflective sequence can only be one of both
        /// </summary>
        public void EstimateSequenceType()
        {
            var owner = this.Unspecified.Owner as XmlObject;

            // Check, if we have an attribute
            if (owner.Node.Attribute(this.Unspecified.PropertyName + "-ref") != null)
            {
                this.sequenceType = XmlReflectiveSequenceType.Attributes;
                return;
            }

            // Check, if we have at least one node
            if (owner.Node.Element(this.Unspecified.PropertyName) != null)
            {
                this.sequenceType = XmlReflectiveSequenceType.Nodes;
                return;
            }

            // Ok, we have a node
            this.sequenceType = XmlReflectiveSequenceType.Unknown;
        }

        #endregion

        #region Get or set content from Xml-Attributes

        /// <summary>
        /// Gets the attribute for the given property or creates 
        /// </summary>
        /// <returns></returns>
        private XAttribute GetAttribute(bool create = true)
        {
            if (this.sequenceType == XmlReflectiveSequenceType.Nodes)
            {
                throw new NotImplementedException("Sequencetype is Nodes... Getting attributes does not make sense");
            }

            var owner = this.Unspecified.Owner as XmlObject;
            var propertyName = this.Unspecified.PropertyName + "-ref";
            var attribute = owner.Node.Attribute(propertyName);

            if (attribute == null)
            {
                attribute = new XAttribute(propertyName, string.Empty);

                if (create)
                {
                    // Only if we do like to create the Attribute, add it to the node
                    owner.Node.Add(attribute);
                }
            }

            return attribute;
        }

        private List<string> GetAttributeAsList()
        {
            var valueAsString = this.GetAttribute().Value;

            return valueAsString.Split(new[] { ' ' }).Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        private void SetAttributeAsList(List<string> values)
        {
            if (this.sequenceType == XmlReflectiveSequenceType.Nodes)
            {
                throw new NotImplementedException("Sequencetype is Nodes... Getting attributes does not make sense");
            }

            var builder = new StringBuilder();
            var first = true;
            foreach (var value in values)
            {
                if (!first)
                {
                    builder.Append(' ');
                }

                builder.Append(value);

                first = false;
            }

            this.GetAttribute().Value = builder.ToString();

            this.sequenceType = XmlReflectiveSequenceType.Attributes;
        }

        #endregion

        public override void add(int index, object value)
        {
            // Which type is the value 
            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                if (valueAsIObject.Extent != null)
                {
                    // Ok, we can add it. 
                    var poolResolver = PoolResolver.GetDefault(this.Unspecified.Owner.Extent.Pool);
                    var path = poolResolver.GetResolvePath(valueAsIObject, this.Unspecified.Owner);

                    // Ok, now we got it, now we need to inject our element
                    var list = this.GetAttributeAsList();
                    list.Insert(index, path);
                    this.SetAttributeAsList(list);
                }
                else
                {
                    throw new NotImplementedException("Given Object is not connected to an extent. ");
                }
            }
            else if (Extensions.IsNative(value))
            {
                // Add it as a new Xml Element, containing the property as a value
                var element = new XElement(this.Unspecified.PropertyName);
                element.Value = value.ToString();

                var xmlObject = this.Unspecified.Owner as XmlObject;
                xmlObject.Node.Add(element);

                this.sequenceType = XmlReflectiveSequenceType.Nodes;
            }
            else
            {
                throw new NotImplementedException("Only IObjects and native objects are supported to get added");
            }
        }

        public override object get(int index)
        {
            if (this.sequenceType == XmlReflectiveSequenceType.Attributes)
            {
                // At the moment, we just support IObjects
                var objectList = this.GetAttributeAsList();

                var path = objectList[index];
                var poolResolver = PoolResolver.GetDefault(this.Unspecified.Owner.Extent.Pool);
                var resolvedObject = poolResolver.Resolve(path, this.Unspecified.Owner);

                return resolvedObject;
            }
            else if (this.sequenceType == XmlReflectiveSequenceType.Nodes)
            {
                var xmlObject = this.Unspecified.Owner as XmlObject;
                var elements = xmlObject.Node.Elements(this.Unspecified.PropertyName);
                if (elements.Count() <= index)
                {
                    // The number of available subelements is too low
                    return DatenMeister.Logic.ObjectHelper.NotSet;
                }

                var element = elements.ElementAt(index);
                if (element.Value != null && !element.HasElements && !element.HasAttributes)
                {
                    // Is a string (or any other type)
                    return element.Value;
                }
                else
                {
                    throw new NotImplementedException("No return of elements containing subelements or attributes implemented. ");
                }
            }
            else
            {
                return DatenMeister.Logic.ObjectHelper.NotSet;
            }
        }

        public override object remove(int index)
        {
            if (this.sequenceType == XmlReflectiveSequenceType.Attributes)
            {
                var objectList = this.GetAttributeAsList();
                var result = objectList[index];
                objectList.RemoveAt(index);
                this.SetAttributeAsList(objectList);

                return result;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override object set(int index, object value)
        {
            // Which type is the value 
            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                if (valueAsIObject.Extent != null)
                {
                    var poolResolver = PoolResolver.GetDefault(this.Unspecified.Owner.Extent.Pool);
                    var path = poolResolver.GetResolvePath(valueAsIObject, this.Unspecified.Owner);

                    // Ok, now we got it, now we need to inject our element
                    var list = this.GetAttributeAsList();
                    var oldResult = list[index];
                    list[index] = path;
                    this.SetAttributeAsList(list);
                    return oldResult;
                }
                else
                {
                    throw new NotImplementedException("Given Object is not connected to an extent. ");
                }
            }
            else
            {
                throw new NotImplementedException("Only IObjects are supported to get added");
            }
        }

        public override bool add(object value)
        {
            this.add(this.size(), value);

            return true;
        }

        public override void clear()
        {
            // Direct setting, without using the intermediate list
            this.GetAttribute().Value = string.Empty;
        }

        public override bool remove(object value)
        {
            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                if (valueAsIObject.Extent != null)
                {
                    var poolResolver = PoolResolver.GetDefault(this.Unspecified.Owner.Extent.Pool);
                    var path = poolResolver.GetResolvePath(valueAsIObject, this.Unspecified.Owner);

                    var attributeList = this.GetAttributeAsList();
                    var result = attributeList.Remove(path);
                    this.SetAttributeAsList(attributeList);
                    return result;
                }
                else
                {
                    throw new NotImplementedException("Given Object is not connected to an extent. ");
                }
            }
            else
            {
                throw new NotImplementedException("Only IObjects are supported to get added");
            }
        }

        public override int size()
        {
            switch (this.sequenceType)
            {
                case XmlReflectiveSequenceType.Unknown:
                    return 0;
                case XmlReflectiveSequenceType.Attributes:
                    return this.GetAttributeAsList().Count;
                case XmlReflectiveSequenceType.Nodes:
                    var xmlObject = this.Unspecified.Owner as XmlObject;
                    return xmlObject.Node.Elements(this.Unspecified.PropertyName).Count();
                default:
                    throw new NotImplementedException("Sequence type is not known");
            }
        }

        public override IEnumerable<object> getAll()
        {
            var attributeList = this.GetAttributeAsList();
            if (attributeList.Count() == 0)
            {
                yield break;
            }

            var poolResolver = PoolResolver.GetDefault(this.Unspecified.Owner.Extent.Pool);
            foreach (var path in attributeList)
            {
                yield return poolResolver.Resolve(path, this.Unspecified.Owner);
            }
        }
    }
}
