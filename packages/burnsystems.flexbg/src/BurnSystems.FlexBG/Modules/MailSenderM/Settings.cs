using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    [XmlRoot("MailSender")]
    public class Settings
    {
        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string SmtpUsername
        {
            get;
            set;
        }

        public string SmtpPassword
        {
            get;
            set;
        }

        public string SubjectPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether mailsending is deactivated
        /// </summary>
        public bool IsDeactivated
        {
            get;
            set;
        }
    }
}
