using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    public class AdminUpdateProfileModel
    {
        /// <summary>
        /// Gets or sets the path of the entity
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the displaynem
        /// </summary>
        public string Displayname
        {
            get;
            set;
        }
    }
}
