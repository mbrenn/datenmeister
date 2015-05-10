using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.DM.UserManagement
{
    public class User
    {
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password of the user. 
        /// The password may also be encrypted. Depends on the project 
        /// </summary>
        public string password
        {
            get;
            set;
        }
    }
}
