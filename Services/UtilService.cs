using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using Newtonsoft.Json;

namespace IntegrationAPIs.Services
{
    public class UtilService
    {

        //public static bool SendMail(string email, string subject, string message)
        //{
        //    try
        //    {
        //        string[] Recipients = email.Split(',');
        //        EmailInfo info = new EmailInfo();

        //        info = InitializeMailServer();

        //        info.FileName = "";
        //        info.DisplayName = "HYGEIA HMO";
        //        info.MailSubject = subject;
        //        info.Recipients = Recipients;
        //        info.IsHTML = true;
        //        info.Message = message;

        //        string response = SendMailServer(info);

        //        //AuditLog($"Sending Email to {email} | Subject {subject}| Message: {message} | ", DateTime.Now, "Refunds", "Refunds reporting", $"{response}", "SendMail");

        //        return response == "success" ? true : false;
        //    }
        //    catch (Exception ex) { return false; }
        //}

        //public static EmailInfo InitializeMailServer()
        //{
        //    EmailInfo info = new EmailInfo();
        //    SQLDataManager sql = new SQLDataManager(false);

        //    string mserver = "select [value] from app_setting where [code]='Mail_Server'";
        //    string muser = "select [value] from app_setting where [code]='Mail_Username'";
        //    string mpass = "select [value] from app_setting where [code]='Mail_Password'";
        //    string mfrom = "select [value] from app_setting where [code]='Mail_From'";
        //    string mport = "select [value] from app_setting where [code]='Mail_Port'";
        //    string mssl = "select [value] from app_setting where [code]='Mail_SSL'";

        //    DataTable dserver = sql.GetDataset(mserver, CommandType.Text).Tables[0];
        //    DataTable duser = sql.GetDataset(muser, CommandType.Text).Tables[0];
        //    DataTable dpass = sql.GetDataset(mpass, CommandType.Text).Tables[0];
        //    DataTable dfrom = sql.GetDataset(mfrom, CommandType.Text).Tables[0];
        //    DataTable dport = sql.GetDataset(mport, CommandType.Text).Tables[0];
        //    DataTable dssl = sql.GetDataset(mssl, CommandType.Text).Tables[0];


        //    info.IsHTML = true;
        //    info.MailServer = dserver.Rows[0][0].ToString();
        //    info.MailUsername = duser.Rows[0][0].ToString();
        //    info.MailPassword = dpass.Rows[0][0].ToString();
        //    info.MailPort = Convert.ToInt32(dport.Rows[0][0].ToString());
        //    info.MailSSL = Convert.ToBoolean(dssl.Rows[0][0].ToString());
        //    info.MailFrom = dfrom.Rows[0][0].ToString();

        //    sql.ClosedbConnection();

        //    return info;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        //public static string SendMailServer(EmailInfo info)
        //{
        //    SmtpClient client = new SmtpClient(info.MailServer);
        //    client.Credentials = new NetworkCredential(info.MailUsername, info.MailPassword);
        //    client.Port = info.MailPort;
        //    client.EnableSsl = info.MailSSL;

        //    MailMessage mail = new MailMessage();
        //    mail.From = new MailAddress(info.MailFrom, info.DisplayName);
        //    mail.Subject = info.MailSubject;
        //    mail.Body = info.Message;
        //    mail.IsBodyHtml = info.IsHTML;

        //    if (!info.FileName.Trim().Equals(""))
        //        mail.Attachments.Add(new Attachment(info.FileName));

        //    string[] receivers = info.Recipients;

        //    try
        //    {
        //        for (int i = 0; i < receivers.Length; i++)
        //        {
        //            if (!receivers[i].Trim().Contains("@hygeiahmo.com"))
        //            {
        //                if (!ValidateEmailService(receivers[i].Trim()).Result)
        //                {
        //                    return $"email address {receivers[i].Trim()} is invalid";
        //                }
        //            }

        //            mail.To.Add(receivers[i].Trim());
        //        }

        //        client.Send(mail);

        //        mail.Attachments.Dispose();
        //        return "success";
        //    }
        //    catch (SmtpException ex)
        //    {
        //        string msg = ex.Message;
        //        return msg;
        //    }

        //}

        //private static async Task<bool> ValidateEmailService(string email)
        //{
        //    try
        //    {
        //        if (!email.Equals("ITmailrepo@hygeiahmo.com"))
        //        {
        //            SQLDataManager sql = new SQLDataManager(false);
        //            //Check if the email exist on the database
        //            string e = "select [email] from ArchivedEmail where email ='" + email + "'";
        //            string v = "select [Isvalid] from ArchivedEmail where email ='" + email + "'";

        //            DataTable mailAddTable = sql.GetDataset(e, CommandType.Text).Tables[0];
        //            DataTable isValidTable = sql.GetDataset(v, CommandType.Text).Tables[0];

        //            sql.ClosedbConnection();

        //            //if it exist, then return the value of isValid

        //            if (mailAddTable.Rows.Count > 0)
        //            {
        //                var emailAdd = mailAddTable.Rows[0][0].ToString();
        //                var isValid = isValidTable.Rows[0][0].ToString();

        //                return bool.Parse(isValid);
        //            }

        //            //else validate using emailable
        //            string url = $"https://api.emailable.com/v1/verify?email={email}&api_key=live_980a5dfab643986c8537";
        //            var client = new HttpClient();
        //            var request = new HttpRequestMessage(HttpMethod.Get, url);
        //            var response = await client.SendAsync(request);
        //            response.EnsureSuccessStatusCode();
        //            Console.WriteLine(await response.Content.ReadAsStringAsync());
        //            var output = JsonConvert.DeserializeObject<MailParameter>(await response.Content.ReadAsStringAsync());

        //            bool resp;

        //            if (output.state != null)
        //            {
        //                if (!output.state.Trim().Equals("undeliverable"))
        //                    resp = true;
        //                else
        //                    resp = false;
        //            }
        //            else
        //                resp = false;

        //            //save the record to the database, email and isValid.
        //            //SaveEmail(email, resp);

        //            return resp;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }

        //    return false;
        //}

        //private static void SaveEmail(string email, bool isvalid)
        //{
        //    try
        //    {
        //        SQLDataManager sql = new SQLDataManager(false);
        //        string query = $"insert into ArchivedEmail (email, Isvalid, DateAdded) values (@email,@Isvalid, @DateAdded)";

        //        sql.AddParamAndValue("@email", email);
        //        sql.AddParamAndValue("@Isvalid", isvalid);
        //        sql.AddParamAndValue("@DateAdded", DateTime.Now);

        //        int resp = sql.ExecuteNonQuery(query, CommandType.Text);

        //        sql.ClosedbConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }

        //}

        //public void AuditLog(string keyvalue, DateTime logdate, string moduleName, string pagename, string typeofchange, string adjuster)

        //{

        //    using (var cdb = new HMO_CBA_ALLEntities())
        //    {

        //        audit log = new audit

        //        {

        //            keyvalue = keyvalue,

        //            logdate = logdate,

        //            modulename = moduleName,

        //            pagename = pagename,

        //            typeofchange = typeofchange,

        //            adjusterid = adjuster

        //        };

        //        cdb.audits.Add(log);

        //        cdb.SaveChanges();

        //    }

        //}

        //method to send a mail
        public void SendFormattedHTMLEmail(string recepientEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(recepientEmail));
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["Host"];
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                    smtp.Send(mailMessage);
                }
            }
            catch (Exception exc)
            {
            }
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }


    public class EmailInfo
    {
        private string _MailServer = "";
        private string _MailUsername = "";
        private string _MailPassword = "";
        private string _MailFrom = "";
        private string _MailSubject = "";
        private string[] _Recipients;
        private string _Message;
        private string _DisplayName = "";
        private bool _IsHTML;
        private bool _MailSSL;
        private string _FileName;
        private Stream _ContentStream;
        private string _ContentType;
        private int _MailPort;


        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        public Stream ContentStream
        {
            get { return _ContentStream; }
            set { _ContentStream = value; }
        }


        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public string MailServer
        {
            get { return _MailServer; }
            set { _MailServer = value; }
        }

        public string MailUsername
        {
            get { return _MailUsername; }
            set { _MailUsername = value; }
        }

        public string MailPassword
        {
            get { return _MailPassword; }
            set { _MailPassword = value; }
        }

        public string MailFrom
        {
            get { return _MailFrom; }
            set { _MailFrom = value; }
        }

        public string MailSubject
        {
            get { return _MailSubject; }
            set { _MailSubject = value; }
        }

        public string[] Recipients
        {
            get { return _Recipients; }
            set { _Recipients = value; }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        public bool IsHTML
        {
            get { return _IsHTML; }
            set { _IsHTML = value; }
        }

        public int MailPort
        {
            get { return _MailPort; }
            set { _MailPort = value; }
        }
        public bool MailSSL
        {
            get { return _MailSSL; }
            set { _MailSSL = value; }
        }
    }

    public class MailParameter
    {
        public string accept_all { get; set; }
        public string did_you_mean { get; set; }
        public string disposable { get; set; }
        public string domain { get; set; }
        public string duration { get; set; }
        public string state { get; set; }
    }


}
