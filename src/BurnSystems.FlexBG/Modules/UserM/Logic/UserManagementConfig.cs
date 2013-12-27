using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Stores the configuration for the usermanagement
    /// </summary>
    public class UserManagementConfig
    {
        /// <summary>
        /// Gets or sets the subject for the register done
        /// </summary>
        public string RegisterDoneMailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the template for the register done
        /// </summary>
        public string RegisterDoneMailTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject for the register done
        /// </summary>
        public string ForgotPwdMailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the template for the register done
        /// </summary>
        public string ForgotPwdMailTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user shall be activated automatically
        /// </summary>
        public bool AutomaticActivation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value whether the authentication via auth shall be inhibited
        /// </summary>
        public bool NoActivationViaAuthPossible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a mail shall also be send to administrator. 
        /// The same mail as "RegisterDoneMailTemplate" will be sent.
        /// </summary>
        public bool RegisterMailToAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the displayname is required to be set
        /// for displaynem
        /// </summary>
        public bool UseDisplayNameForRegister
        {
            get;
            set;
        }
    }
}
