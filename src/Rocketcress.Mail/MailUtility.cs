using Microsoft.Exchange.WebServices.Data;
using Rocketcress.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Rocketcress.Mail
{
    /// <summary>
    /// Utility class for mail actions.
    /// </summary>
    public class MailUtility
    {
        private ExchangeService service;
        private Folder inbox;

        private static AssertEx Assert => AssertEx.Instance;

        /// <summary>
        /// Gets or set the domain for the user (only used for Exchange connections).
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Gets or set the user name.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the mail address of the account to login to.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the SMTP server name.
        /// </summary>
        public string SmtpServerName { get; set; }
        /// <summary>
        /// Gets or sets the port fo the SMTP server.
        /// </summary>
        public string SmtpServerPort { get; set; }
        /// <summary>
        /// Gets or sets the Uri to the Exchange server.
        /// </summary>
        public Uri ExchangeServerUrl { get; set; }

        /// <summary>
        /// Sends an email using SMTP.
        /// </summary>
        public void SendSmtp(string[] receivers, string subject, string body)
        {
            using var mail = new MailMessage
            {
                From = new MailAddress(Address),
                Subject = subject,
                Body = body,
            };

            foreach (string rec in receivers)
                mail.To.Add(rec);

            using var client = new SmtpClient(SmtpServerName, int.Parse(SmtpServerPort))
            {
                Credentials = new NetworkCredential(Username, Password)
            };

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed sending e-mail\n" + ex);
            }
        }

        /// <summary>
        /// Retrieves the number of mails in the inbox of the exchange account.
        /// </summary>
        /// <returns>Returns the number of mails that are in the inbox of the account (<see cref="Address"/>).</returns>
        public int? GetInboxItemCountExchange()
        {
            InitializeExchange();
            if (service != null)
            {
                try
                {
                    inbox.Load();
                    return inbox.TotalCount;
                }
                catch (Exception ex)
                {
                    Assert.Fail("Failed receiving Email Count\n" + ex);
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves all mails that are located in the inbox of the exchange account.
        /// </summary>
        /// <returns>Returns a collection of mails that are in the inbox of the account (<see cref="Address"/>).</returns>
        public FindItemsResults<Item> GetAllInboxItemsExchange()
        {
            InitializeExchange();
            if (service != null)
            {
                try
                {
                    return inbox.FindItems(new ItemView(50)).Result;
                }
                catch (Exception ex)
                {
                    Assert.Fail("Failed receiving Emails\n" + ex);
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the latest email in the inbox of the exchange account.
        /// </summary>
        public void RemoveLatestEmailExchange()
        {
            InitializeExchange();
            if (service != null)
            {
                try
                {
                    IEnumerable<ItemId> ids = inbox.FindItems(new ItemView(1)).Result.Select(x => x.Id);
                    service.DeleteItems(ids, DeleteMode.HardDelete, null, null);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void InitializeExchange()
        {
            if (service == null)
            {
                service = new ExchangeService();
                service.TraceEnabled = true;
                ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
                service.UseDefaultCredentials = false;
                service.Credentials = new WebCredentials(Username, Password, Domain);
                service.EnableScpLookup = false;
                if (ExchangeServerUrl == null)
                    service.AutodiscoverUrl(Address, SuppressRedirectionUrlValidation);
                else
                    service.Url = ExchangeServerUrl;
            }
            if (inbox == null)
                inbox = Folder.Bind(service, WellKnownFolderName.Inbox).Result;
        }

        private static bool SuppressRedirectionUrlValidation(string url)
        {
            return true;
        }

        private static bool CertificateValidationCallBack(
           object sender,
           System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
           System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
