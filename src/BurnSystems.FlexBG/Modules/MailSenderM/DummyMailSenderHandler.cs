//-----------------------------------------------------------------------
// <copyright file="DummyMailSenderHandler.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    using BurnSystems.Logging;
    using System.Collections.Generic;
    using System.Net.Mail;

    /// <summary>
    /// This class is used to ignore all requests to send a mail. 
    /// The mail is stored in the logicstate
    /// </summary>
    public class DummyMailSenderHandler : IMailSender
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(DummyMailSenderHandler));

        /// <summary>
        /// Stores the list of sent mails
        /// </summary>
        public static List<MailMessage> SentMails = new List<MailMessage>();

        /// <summary>
        /// Sends a mail 
        /// </summary>
        /// <param name="receiver">Recipient of the mail</param>
        /// <param name="subject">Subject of th email</param>
        /// <param name="message">Message of the email</param>
        public void SendMail(string receiver, string subject, string message)
        {
            var mail = new MailMessage("fbk@depon.net", receiver);
            mail.Body = message;
            mail.Subject = subject;
            this.SendMail(mail);

            logger.Message("Dummy-Mail has received request to send mail '" + subject + " to '" + receiver + "'");
            logger.Verbose("Subject: " + subject);
            logger.Verbose("Message: " + message);
        }

        /// <summary>
        /// Sends a mail
        /// </summary>
        /// <param name="mail">Mail to be sent</param>
        public void SendMail(System.Net.Mail.MailMessage mail)
        {
            SentMails.Add(mail);
        }
    }
}
