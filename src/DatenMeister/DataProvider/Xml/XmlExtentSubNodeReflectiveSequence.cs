using BurnSystems.Logger;
using BurnSystems.Test;
using DatenMeister.DataProvider.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// This reflective sequence returns the elements of an XmlExtent, which are 
    /// subelements below the root nodes. 
    /// The root node itself is not returned
    /// </summary>
    internal class XmlExtentSubnodeReflectiveSequence : BaseReflectiveSequence
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(XmlExtentSubnodeReflectiveSequence));
        private XmlExtent extent;

        public XmlExtentSubnodeReflectiveSequence(XmlExtent extent)
            : base(extent)
        {
            Ensure.That(extent != null);
            this.extent = extent;
        }

        public override void add(int index, object value)
        {
            logger.LogEntry(new LogEntry("add(int, object) is not fully supported. Will be added to last position", LogLevel.Message));

            this.add(value);
        }

        public override object get(int index)
        {
            return this.getAll().ElementAt(index);
        }

        public override object remove(int index)
        {
            throw new NotImplementedException();
        }

        public override object set(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override bool add(object value)
        {
            if (value is XmlObject)
            {
                var valueAsXmlObject = value as XmlObject;
                valueAsXmlObject.ContainerExtent = this.extent;
                var parentElement = this.extent.XmlDocument.Root;

                // Checks, if we have a better element, where new node can be added
                var info = this.extent.Settings.Mapping.FindByType(valueAsXmlObject.getMetaClass());
                if (info != null)
                {
                    parentElement = info.RetrieveRootNode(this.extent.XmlDocument);
                }

                // Adds a simple object 
                parentElement.Add(valueAsXmlObject.Node);

                this.extent.IsDirty = true;

                return true;
            }

            if (value == null)
            {
                logger.Notify("Nothing (null) cannot be added");
                return false;
            }

            throw new InvalidOperationException("Only objects as IObject may be added");
        }

        public override void clear()
        {
            throw new NotImplementedException();
        }

        public override bool remove(object value)
        {
            Ensure.That(value != null);
            var valueAsObject = value as XmlObject;
            Ensure.That(valueAsObject != null, "Value is not XmlObject");

            valueAsObject.delete();

            return true;
        }

        public override int size()
        {
            return this.getAll().Count();
        }

        public override IEnumerable<object> getAll()
        {
            lock (this.extent.XmlDocument)
            {
                var foundItems = new List<XElement>();

                // Goes through the mapping table to find additional objects
                if (this.extent.Settings != null)
                {
                    foreach (var mapInfo in this.extent.Settings.Mapping.GetAll())
                    {
                        var rootNode = mapInfo.RetrieveRootNode(this.extent.XmlDocument);
                        if (rootNode != null)
                        {
                            foundItems.Add(rootNode);
                            foreach (var xmlSubnode in rootNode.Elements())
                            {
                                var result = new XmlObject(this.extent, xmlSubnode, null)
                                {
                                    ContainerExtent = this.extent
                                };

                                yield return result;
                            }
                        }
                    }
                }

                foreach (var subNode in this.extent.XmlDocument.Root.Elements())
                {
                    // Only for the items, that do not have a direct mapping via settings, the elements will be returned
                    if (!foundItems.Contains(subNode))
                    {
                        var typeAttributeName = DatenMeister.Entities.AsObject.Uml.Types.XmiNamespace + "type";
                        var hasTypeAttribute = subNode.Attribute(typeAttributeName) != null;

                        // Per default, show root nodes, or if user does not want to skip the nodes
                        // or when node has a type attribute
                        if (this.extent.Settings == null
                            || !this.extent.Settings.OnlyUseAssignedNodes
                            || hasTypeAttribute)
                        {
                            var subObject = new XmlObject(this.extent, subNode)
                            {
                                ContainerExtent = this.extent
                            };

                            yield return subObject;
                        }
                    }
                }
            }
        }
    }
}
