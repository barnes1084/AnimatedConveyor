using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace CommonTools
{
    public class Send
    {
        
        public static void Email(string subject, string body, bool html = false)
        {
            try
            {
                List<string> list = listCreation();
                MailMessage message = new MailMessage();
                message.From = new MailAddress($"{Environment.MachineName}@goodyear.com");
                foreach (var emailAddr in list)
                {
                    message.To.Add(new MailAddress(emailAddr));
                }
                message.Subject = subject;
                if (html)
                {
                    message.IsBodyHtml = true;
                }
                else { message.IsBodyHtml = false; }

                message.Body = body;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 25;
                smtp.Host = "gyrelay";
                smtp.Credentials = CredentialCache.DefaultNetworkCredentials;

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Log.ToFile($"{ex.Message} - {ex.StackTrace}");
            }
        }



        private static List<string> listCreation()
        {
            string query = "SELECT count(*) FROM emailList";
            string connection = @"Data Source=AKRWEBRPT;Initial Catalog=General;Integrated Security=True;User ID=sa;Password=W3lc0m32G00dy3ar!";
            
            long rowNumber = DbAccess.SQLsingleInteger(query, connection);
            try
            {
                DataTable table = new DataTable();
                List<string> list = new List<string>();
                string queryEmails = "SELECT * FROM emailList";
                table = DbAccess.QuerySqlDB(queryEmails, connection);
                
                for (long i = 0; i < rowNumber; i++)
                {          
                    list.Add(table.Rows[(int)i].Field<string>(2));
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.ToFile($"{ex.Message} - {ex.StackTrace}");
                return null;
            }
        }


        public static void TestEmail(string ToAddress, string subject, string body, bool html = false)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress($"{Environment.MachineName}@goodyear.com");
                message.To.Add(new MailAddress(ToAddress));

                message.Subject = subject;
                if (html)
                {
                    message.IsBodyHtml = true;
                }
                else { message.IsBodyHtml = false; }

                message.Body = body;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 25;
                smtp.Host = "gyrelay";  // "gysmtpnotls.goodyear.com";
                smtp.Credentials = CredentialCache.DefaultNetworkCredentials;

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Log.ToFile($"{ex.Message} - {ex.StackTrace}");
            }
        }
    }
}
