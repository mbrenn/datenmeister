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
    /// Implements the Reflective Sequence Collection for Xml-Types
    /// </summary>
    public class XmlReflectiveSequence: BaseReflectiveSequence
    {
        private XmlUnspecified Unspecified
        {
            get;
            set;
        }

        public XmlReflectiveSequence(XmlUnspecified unspecified)
        {
            this.Unspecified = unspecified;
        }

        /// <summary>
        /// Gets the attribute for the given property or creates 
        /// </summary>
        /// <returns></returns>
        private XAttribute GetAttribute()
        {
            var owner = this.Unspecified.Owner as XmlObject;
            var propertyName = this.Unspecified.PropertyName + "-ref";
            var attribute = owner.Node.Attribute(propertyName);

            if (attribute == null)
            {
                attribute = new XAttribute(propertyName, string.Empty);
                owner.Node.Add(attribute);
            }

            return attribute;
        }

        private List<string> GetAttributeAsList()
        {
            var valueAsString = this.GetAttribute().Value;

            return valueAsString.Split(new[] { ' ' }).ToList();
        }

        private void SetAttributeAsList(List<string> values)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var value in values)
            {
                if (!first)
                {
                    builder.Append(' ');
                }

                builder.Append(value);
            }

            this.GetAttribute().Value = builder.ToString();
        }

        public override void add(int index, object value)
        {
            // Which type is the value 
            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                if (valueAsIObject.Extent != null)
                {
                    // Ok, we can add it. 
                    var poolResolver = Global.Application.Get<IPoolResolver>();
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
            else
            {
                throw new NotImplementedException("Only IObjects are supported to get added");
            }
        }

        public override object get(int index)
        {
            // At the moment, we just support IObjects
            var objectList = this.GetAttributeAsList();

            var path = objectList[index];
            var poolResolver = Global.Application.Get<IPoolResolver>();
            var resolvedObject = poolResolver.Resolve(path, this.Unspecified.Owner);

            return resolvedObject;
        }

        public override object remove(int index)
        {
            var objectList = this.GetAttributeAsList();
            objectList.RemoveAt(index);
            this.SetAttributeAsList(objectList);

            return true;
        }

        public override object set(int index, object value)
        {
            // Which type is the value 
            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                if (valueAsIObject.Extent != null)
                {
                    var poolResolver = Global.Application.Get<IPoolResolver>();
                    var path = poolResolver.GetResolvePath(valueAsIObject, this.Unspecified.Owner);

                    // Ok, now we got it, now we need to inject our element
                    var list = this.GetAttributeAsList();
                    list[index] = path;
                    this.SetAttributeAsList(list);
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
         
            return value;
        }

        public override bool add(object value)
        {
            this.add(this.size(), value);

            return true;
        }

        public override void clear()
        {
            this.GetAttribute().Value = string.Empty;
        }

        public override object remove(object value)
        {
            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                if (valueAsIObject.Extent != null)
                {
                    var poolResolver = Global.Application.Get<IPoolResolver>();
                    var path = poolResolver.GetResolvePath(valueAsIObject, this.Unspecified.Owner);

                    var attributeList = this.GetAttributeAsList();
                    return attributeList.Remove(path);
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
            return this.GetAttributeAsList().Count;
        }

        public override IEnumerable<object> getAll()
        {
            var poolResolver = Global.Application.Get<IPoolResolver>();
            
            var attributeList = this.GetAttributeAsList();
            foreach (var path in attributeList)
            {
                yield return poolResolver.Resolve(path, this.Unspecified.Owner);
            }
        }
    }
}
