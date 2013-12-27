using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Defines the mail sending engine
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Sends a mail
        /// </summary>
        /// <param name="receiver">The one, who shall receive the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="content">Content of the message</param>
        void SendMail(string receiver, string subject, string content);
    }
}
