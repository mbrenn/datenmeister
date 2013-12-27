using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using System.Net.Mail;
using System.Xml.Serialization;
using System.Net;
using BurnSystems.Logging;
using BurnSystems.FlexBG.Modules.ServerInfoM;
using BurnSystems.ObjectActivation;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Implements the mailsender
    /// </summary>
    public class MailSender : IMailSender
    {
        private ILog logger = new ClassLogger(typeof(MailSender));

        private bool sendAsync = true;

        [Inject(IsMandatory = true)]
        public IServerInfoProvider GameInfo
        {
            get;
            set;
        }

        private Settings settings;

        [Inject]
        public MailSender(IConfigurationStorage storage)
        {
            var xmlSettings = storage.Documents
                .Elements("FlexBG")
                .Elements("MailSender")
                .LastOrDefault();

            if (xmlSettings == null)
            {
                logger.LogEntry(LogLevel.Critical, "No Settings found within configuration '/FlexBG/MailSender'");
                throw new InvalidOperationException("No Settings found within configuration '/FlexBG/MailSender'");
            }
            else
            {
                this.settings = (Settings)
                    new XmlSerializer(typeof(Settings)).Deserialize(xmlSettings.CreateReader());
            }
        }

        /// <summary>
        /// Sends a mail
        /// </summary>
        /// <param name="mailMessage">Mail message to be sent</param>
        public void SendMail(string receiver, string subject, string content)
        {
            var smtpClient = new System.Net.Mail.SmtpClient(this.settings.Host, this.settings.Port);
            smtpClient.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);

            var mailMessage = new MailMessage(
                this.GameInfo.ServerInfo.AdminEMail,
                receiver,
                subject,
                content);

            mailMessage.Subject = this.settings.SubjectPrefix + mailMessage.Subject;

            // Sends mail

            if (this.settings.IsDeactivated)
            {
                logger.LogEntry(LogLevel.Message, "Mail " + mailMessage.Subject + " to " + mailMessage.To + " has not been sent due to deactivation of mailsender.");
            }
            else
            {
                if (sendAsync)
                {
                    smtpClient.SendAsync(mailMessage, null);
                    smtpClient.SendCompleted += (x, y) =>
                    {
                        logger.LogEntry(LogLevel.Message, "Mail " + mailMessage.Subject + " to " + mailMessage.To + " has been sent.");
                    };
                }
                else
                {
                    smtpClient.Send(mailMessage);
                    logger.LogEntry(LogLevel.Message, "Mail " + mailMessage.Subject + " to " + mailMessage.To + " has been sent.");
                }
            }
        }
    }
}
