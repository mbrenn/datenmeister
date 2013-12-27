using BurnSystems.FlexBG.Modules.ServerInfoM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    public class AdminMailSender : IAdminMailSender
    {
        [Inject(IsMandatory = true)]
        public IMailSender MailSender
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IServerInfoProvider ServerInfoProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Sends mail with given subject and message to administrator
        /// </summary>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="message">Message of the mail</param>
        public void Send(string subject, string message)
        {
            this.MailSender.SendMail(
                this.ServerInfoProvider.ServerInfo.AdminEMail,
                subject,
                message);
        }
    }
}
