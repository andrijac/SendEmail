using System;
using System.Diagnostics;
using System.IO;

namespace SendEmail
{
	internal class Program
	{
		private static string CredentialsFile = "sendemail.creds";
		private static string CredentialsFileTemp = "temp.sendemail.creds";

		/// <summary>
		/// Used code from http://stackoverflow.com/a/32336
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Help();
			}
			else
			{
				if (args[0] == "-w")
				{
					WriteCredentials(args);
				}
				else
				{
					ConfigInfo configInfo = ReadCredentials(args);
					SendEmail(args, configInfo);
				}
			}

			if (Debugger.IsAttached)
			{
				Console.ReadLine();
			}
		}

		private static void SendEmail(string[] args, ConfigInfo configInfo)
		{
			string toAddress = args[1];
			string subject = args[2];
			string body = args[3];

			EmailHelper.Send(configInfo.FromAddress, configInfo.FromPassword, toAddress, subject, body);
		}

		private static ConfigInfo ReadCredentials(string[] args)
		{
			string securityKey = args[0];

			CryptoHelper.DecryptFile(CredentialsFile, CredentialsFileTemp, securityKey);

			ConfigInfo configInfo = (ConfigInfo)SerializerHelper.Deserialize(typeof(ConfigInfo), CredentialsFileTemp);

			File.Delete(CredentialsFileTemp);

			return configInfo;
		}

		private static void WriteCredentials(string[] args)
		{
			string fromAddress = args[1];
			string fromPassword = args[2];
			string securityKey = args[3];

			if (securityKey.Length != 8)
			{
				throw new Exception("Key has be 8 characters in length");
			}

			ConfigInfo configInfo = new ConfigInfo()
			{
				FromAddress = fromAddress,
				FromPassword = fromPassword
			};

			SerializerHelper.Serialize(typeof(ConfigInfo), configInfo, CredentialsFileTemp);

			CryptoHelper.EncryptFile(CredentialsFileTemp, CredentialsFile, securityKey);

			File.Delete(CredentialsFileTemp);
		}

		private static void Help()
		{
			WL(string.Empty);
			WL("SendEmail - send email from command line");
			WL("-----------------------------------------");
			WL("Usage:");
			WL(string.Empty);
			WL("\tSend email:");
			WL("\tSendEmail.exe \"security key\" \"to address\" \"email subject\" \"email body\"");
			WL(string.Empty);
			WL("\tWrite credentials:");
			WL("\tSendEmail.exe -w \"from adress\" \"from password\" \"security key\"");

			WL(string.Empty);
			WL(string.Empty);

			WL("Examples:");
			WL(string.Empty);
			WL("\tSend email: ");
			WL("\tSendEmail.exe securitykey to@domain.com \"subject\" \"body\"");
			WL(string.Empty);
			WL("\tWrite credentials:");
			WL("\tSendEmail.exe -w from@domain.com fromPassword securitykey");
		}

		private static void WL(string value)
		{
			Console.WriteLine(value);
		}
	}
}