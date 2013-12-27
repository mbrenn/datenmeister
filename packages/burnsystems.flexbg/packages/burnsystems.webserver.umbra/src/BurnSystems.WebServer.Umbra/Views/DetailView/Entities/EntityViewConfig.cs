using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView.Entities
{
    /// <summary>
    /// Stores the configuration for the view
    /// </summary>
    public class EntityViewConfig
    {
        /// <summary>
        /// Gets or sets the rows
        /// </summary>
        public List<EntityViewTable> Tables
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all detail tables
        /// </summary>
        public IEnumerable<EntityViewDetailTable> DetailTables
        {
            get { return this.Tables.Select(x => x as EntityViewDetailTable).Where(x => x != null); }
        }

        /// <summary>
        /// Initializes a new instance of the entity view config
        /// </summary>
        public EntityViewConfig(params EntityViewTable[] tables)
        {
            this.Tables = new List<EntityViewTable>();
            foreach (var table in tables)
            {
                this.Tables.Add(table);
            }
        }

        /// <summary>
        /// Converts the object to json
        /// </summary>
        /// <returns></returns>
        public object ToJson(IActivates container, ITreeViewItem item)
        {
            return this.Tables.Select(x => x.ToJson(container, item));
        }
    }
}
