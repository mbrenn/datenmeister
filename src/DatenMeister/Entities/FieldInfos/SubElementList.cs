using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Shows a list of subelements
    /// </summary>
    public class SubElementList : General
    {
        /// <summary>
        /// Gets or sets the type, that shall be created when user selects a new object
        /// </summary>
        public IObject typeForNew
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the layout for the table view of the subitems. 
        /// The tableview shall be of type <c>TableView</c>
        /// </summary>
        public TableView listTableView       
        {
            get;
            set;
        }

        public SubElementList(string name, string binding)
            : base(name, binding)
        {
        }
    }
}
