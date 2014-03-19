﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Stores the mapping between the types and 
    /// </summary>
    public class XmlTypeMapping
    {
        /// <summary>
        /// Stores the information
        /// </summary>
        private List<XmlTypeInformation> information = new List<XmlTypeInformation>();

        /// <summary>
        /// Adds a mapping between the node name and the type
        /// </summary>
        /// <param name="nodeName">Name of the Xml-Element to be used</param>
        /// <param name="type">Type to be used</param>
        /// <param name="rootNode">Root node being used to insert new elements</param>
        public void Add(string nodeName, IObject type, XElement rootNode)
        {
            var info = new XmlTypeInformation()
            {
                NodeName = nodeName,
                Type = type,
                RootNode = rootNode
            };

            this.information.Add(info);
        }

        /// <summary>
        /// Finds by the nodename
        /// </summary>
        /// <param name="nodeName">Name of the node</param>
        public XmlTypeInformation FindByNodeName(string nodeName)
        {
            return this.information.Where(x => x.NodeName == nodeName).FirstOrDefault();
        }

        /// <summary>
        /// Finds the node by type
        /// </summary>
        /// <param name="type"></param>
        public XmlTypeInformation FindByType(IObject type)
        {
            return this.information.Where(x => x.Type == type).FirstOrDefault();
        }
    }
}