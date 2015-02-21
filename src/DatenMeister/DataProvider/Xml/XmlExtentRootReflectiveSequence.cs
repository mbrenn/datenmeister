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
    internal class XmlExtentRootReflectiveSequence : BaseReflectiveSequence
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(XmlExtentRootReflectiveSequence));
        private XmlExtent extent;

        public XmlExtentRootReflectiveSequence(XmlExtent extent)
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
            throw new NotImplementedException("Adding elements is not supported, since there can only be one element as root in an xml document");
        }

        public override void clear()
        {
            throw new NotImplementedException();
        }

        public override bool remove(object value)
        {
            throw new NotImplementedException("The root element of an xml document cannot be removed");
        }

        public override int size()
        {
            return 1; // There is only one element
        }

        public override IEnumerable<object> getAll()
        {
            lock (this.extent.XmlDocument)
            {
                var subObject = new XmlObject(this.extent, this.extent.XmlDocument.Root)
                {
                    ContainerExtent = this.extent
                };

                yield return subObject;
            }
        }
    }
}
