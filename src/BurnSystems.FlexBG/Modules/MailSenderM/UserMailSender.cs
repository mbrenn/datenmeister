using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    public class UserMailSender : IUserMailSender
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(UserMailSender));

        [Inject(IsMandatory = true)]
        public UserMailSenderSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mailsender, who will really send out the mail
        /// </summary>
        [Inject(IsMandatory = true)]
        public IMailSender MailSender
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the usermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Sends a mail to user
        /// </summary>
        /// <param name="userId">Id of the user, who shal receive te mail</param>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="content">Content to be sent</param>
        /// <returns>Defines the information whether a mail had been sent to user</returns>
        public bool SendMailToUser(long userId, string subject, string content)
        {
            var user = this.UserManagement.GetUser(userId);
            if (user == null)
            {
                logger.Fail("User is not known " + userId.ToString());
                return false;
            }

            // Check the information about the last mail sending and login date
            var lastLogin = this.UserManagement.GetUserData<DateTime>(user, UserDataTokens.LastLogin);
            var lastSending = this.UserManagement.GetUserData<DateTime>(user, UserDataTokens.LastMailSending);
            var acceptsMails = this.UserManagement.GetUserData<bool>(user, UserDataTokens.AcceptsUserMails);
            var email = user.EMail;
            var now = DateTime.Now;

            if (!acceptsMails)
            {
                //logger.Message("Mail " + subject + " to user " + email + " not sent due to settings");
                return false;
            }

            if (now - lastLogin > this.Settings.TimeSinceLastLogin)
            {
                //logger.Message("Mail " + subject + " to user " + email + " not sent due to last login");
                return false;
            }

            if (now - lastSending < this.Settings.MaxMailInterval)
            {
                //logger.Message("Mail " + subject + " to user " + email + " not sent due to last mail sending");
                return false;
            }

            this.UserManagement.SetUserData(user, UserDataTokens.LastMailSending, DateTime.Now);

            this.MailSender.SendMail(email, subject, content);

            return true;
        }
    }
}
