using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    public class AdminSetPasswordModel
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
        /// Gets or sets the password
        /// </summary>
        public string NewPassword
        {
            get;
            set;
        }
    }
}
