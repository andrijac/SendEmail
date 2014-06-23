using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace SendEmail
{
	internal class EmailHelper
	{
		public static void Send(string fromAddress, string fromPassword, string destinationAddress, string subject, string body)
		{
			MailAddress fromMailAddress = new MailAddress(fromAddress, "Source");
			MailAddress toMailAddress = new MailAddress(destinationAddress, "Destination");

			SmtpClient smtp = new SmtpClient()
			{
				Host = GetConfig("Host"),
				Port = int.Parse(GetConfig("Port")),
				EnableSsl = bool.Parse(GetConfig("EnableSsl")),
				DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), GetConfig("DeliveryMethod"), false),
				UseDefaultCredentials = bool.Parse(GetConfig("UseDefaultCredentials")),
				Credentials = new NetworkCredential(fromMailAddress.Address, fromPassword)
			};

			using (MailMessage message = new MailMessage(fromMailAddress, toMailAddress) { Subject = subject, Body = body })
			{
				smtp.Send(message);
			}
		}

		private static string GetConfig(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}