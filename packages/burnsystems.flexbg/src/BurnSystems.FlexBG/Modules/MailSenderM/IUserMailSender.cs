using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Defines the interface for users that will be used when a user shall receive a mail.
    /// It respects the user settings
    /// </summary>
    public interface IUserMailSender
    {
        /// <summary>
        /// Sends a mail
        /// </summary>
        /// <param name="userId">Id of the user, who shall receive the message</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="content">Content of the message</param>
        /// <returns>Defines the information whether a mail had been sent to user</returns>
        bool SendMailToUser(long userId, string subject, string content);
    }
}
