using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    /// <summary>
    /// Defines some constants for user data
    /// </summary>
    public static class UserDataTokens
    {
        /// <summary>
        /// Token for the displayname
        /// </summary>
        public const string DisplayName = "DisplayName";

        /// <summary>
        /// Token for the name
        /// </summary>
        public const string Name = "Name";

        /// <summary>
        /// Token for the prename
        /// </summary>
        public const string Prename = "Prename";

        /// <summary>
        /// Token for the birthday
        /// </summary>
        public const string Birthday = "Birthday";


        /// <summary>
        /// Token for the date when the user received a mail since the last time
        /// </summary>
        public const string AcceptsUserMails = "AcceptsUserMails";

        /// <summary>
        /// Token for the date when the user received a mail since the last time
        /// </summary>
        public const string LastMailSending = "LastMailSending";

        /// <summary>
        /// Token for the date when the user had his last login
        /// </summary>
        public const string LastLogin = "LastLogin";

        /// <summary>
        /// Stores the id of the profile
        /// </summary>
        public const string ProfilePhoto = "ProfilePhoto";
    }
}
