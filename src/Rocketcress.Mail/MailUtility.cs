using Microsoft.Exchange.WebServices.Data;
using Rocketcress.Core;
using System.Net;
using System.Net.Mail;

namespace Rocketcress.Mail;

/// <summary>
/// Utility class for mail actions.
/// </summary>
public class MailUtility
{
    private ExchangeService _service;
    private Folder _inbox;

    private static Assert Assert => Assert.Instance;

    /// <summary>
    /// Gets or sets the domain for the user (only used for Exchange connections).
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Gets or sets the user name.
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
    /// <param name="receivers">The receivers of the mail.</param>
    /// <param name="subject">The subject of the mail.</param>
    /// <param name="body">The body of the mail.</param>
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
            Credentials = new NetworkCredential(Username, Password),
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
        if (_service != null)
        {
            try
            {
                _inbox.Load();
                return _inbox.TotalCount;
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
        if (_service != null)
        {
            try
            {
                return _inbox.FindItems(new ItemView(50)).Result;
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
        if (_service != null)
        {
            try
            {
                IEnumerable<ItemId> ids = _inbox.FindItems(new ItemView(1)).Result.Select(x => x.Id);
                _service.DeleteItems(ids, DeleteMode.HardDelete, null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private void InitializeExchange()
    {
        if (_service == null)
        {
            _service = new ExchangeService();
            _service.TraceEnabled = true;
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
            _service.UseDefaultCredentials = false;
            _service.Credentials = new WebCredentials(Username, Password, Domain);
            _service.EnableScpLookup = false;
            if (ExchangeServerUrl == null)
                _service.AutodiscoverUrl(Address, SuppressRedirectionUrlValidation);
            else
                _service.Url = ExchangeServerUrl;
        }

        if (_inbox == null)
            _inbox = Folder.Bind(_service, WellKnownFolderName.Inbox).Result;
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
