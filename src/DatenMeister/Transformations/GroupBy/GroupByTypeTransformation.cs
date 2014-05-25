﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations.GroupBy
{
    /// <summary>
    /// Performs the grouping by transformation
    /// </summary>
    public class GroupByTypeTransformation : BaseGroupByTransformation
    {
        /// <summary>
        /// Initializes a new instance of the GroupByTypeTransformation
        /// </summary>
        public GroupByTypeTransformation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the GroupByTypeTransformation
        /// </summary>
        /// <param name="source">Source extent</param>
        public GroupByTypeTransformation(IURIExtent source)
        {
            this.source = source;
        }

        public override void ExecuteMapping(GroupByDictionary storage)
        {
            // Nothing to do at the moment
            foreach (var item in this.source.Elements())
            {
                var itemAsElement = item as IElement;
                var type = itemAsElement.getMetaClass();
                storage.AddToGroup(type, item);
            }
        }
    }
}
