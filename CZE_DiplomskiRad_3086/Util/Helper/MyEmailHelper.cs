using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CZE.Data.Models;
using Microsoft.Extensions.Configuration;

namespace CZE.Web.Util.Helper
{
    public class MyEmailHelper
    {
        private readonly string _fromAddress;
        private readonly string _password;
        private readonly string _fromDisplayName;
        private readonly string _host;
        private readonly int _port;

        public Osoba Recipient { get; set; }
        //public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public MyEmailHelper(IConfiguration conf)
        {
            _fromAddress = conf.GetValue<string>("MySettings:Email:FromAddress");
            _password = conf.GetValue<string>("MySettings:Email:Password");
            _fromDisplayName = conf.GetValue<string>("MySettings:Email:FromDisplayName");
            _host = conf.GetValue<string>("MySettings:Email:Host");
            _port = conf.GetValue<int>("MySettings:Email:Port");
        }

        public void SendEmailKorisnickiPodatci()
        {

            using (SmtpClient smtpClient = new SmtpClient(_host, _port))
            {
                smtpClient.Credentials = new NetworkCredential(_fromAddress, _password);
                //smtpClient.UseDefaultCredentials = false; // ovo ne radi kako god se stavi 
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                using (MailMessage mail = new MailMessage())
                {
                    //Setting From , To and CC
                    mail.From = new MailAddress(_fromAddress, _fromDisplayName);
                    mail.To.Add(new MailAddress(Recipient.Email));
                    //mail.CC.Add(new MailAddress("muris.cengic@edu.fit.ba"));
                    mail.Subject = this.Subject;
                    mail.Body = this.Body;

                    smtpClient.Send(mail);
                }
            }

        }

        public static string GetKorisnickiPodatciCreateEmailBody(Osoba recipient)
        {
            return $"{"Poštovan" + (recipient.Spol == Osoba.eSpol.M ? "i" : "a")},\n\n" +
                          "Vaš zahtjev za registracijom je odobren. Lozinka za login je {lozinkaPolje}.\n\n" +
                          "Ugodno korištenje želi vam CZE tim.";
        }
        public static string GetKorisnickiPodatciEditEmailBody()
        {
            return $"Poštovani,\n\n" +
                          "Došlo je do promjene vaše lozinke. Nova lozinka za login je {lozinkaPolje}.\n\n" +
                          "Ugodno korištenje želi vam CZE tim.";
        }

    }
}
