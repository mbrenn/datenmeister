using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Mail sender that is responsible to send emails to administrator
    /// </summary>
    public interface IAdminMailSender
    {
        /// <summary>
        /// Sends mail with given subject and message to administrator
        /// </summary>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="message">Message of the mail</param>
        void Send(string subject, string message);
    }
}
