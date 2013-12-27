using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Defines the settings
    /// </summary>
    public class UserMailSenderSettings
    {
        public UserMailSenderSettings()
        {
            this.TimeSinceLastLogin = TimeSpan.FromDays(14);
            this.MaxMailInterval = TimeSpan.FromDays(1);
        }

        /// <summary>
        /// Gets or sets the time until users shall receive mails since the last login. 
        /// If last login has occured before, the user will not receive any mail
        /// </summary>
        public TimeSpan TimeSinceLastLogin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mail interval in which the users shall receive the mails.
        /// Users will not receive two mails within the time interval
        /// </summary>
        public TimeSpan MaxMailInterval
        {
            get;
            set;
        }
    }
}
