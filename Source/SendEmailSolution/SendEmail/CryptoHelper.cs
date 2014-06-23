using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace SendEmail
{
	/// <summary>
	/// http://support.microsoft.com/kb/307010
	/// </summary>
	public class CryptoHelper
	{
		//  Call this function to remove the key from memory after use for security.
		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
		public static extern bool ZeroMemory(ref string Destination, int Length);

		// Function to Generate a 64 bits Key.
		public static string GenerateKey()
		{
			// Create an instance of Symetric Algorithm. Key and IV is generated automatically.
			DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

			// Use the Automatically generated key for Encryption.
			return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
		}

		public static void EncryptFile(string inputFilename, string outputFilename, string key)
		{
			FileStream fsInput = new FileStream(inputFilename,
				FileMode.Open,
				FileAccess.Read);

			FileStream fsEncrypted = new FileStream(outputFilename,
							FileMode.Create,
							FileAccess.Write);

			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
			DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

			ICryptoTransform desencrypt = DES.CreateEncryptor();
			CryptoStream cryptostream = new CryptoStream(fsEncrypted,
								desencrypt,
								CryptoStreamMode.Write);

			byte[] bytearrayinput = new byte[fsInput.Length];
			fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
			cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);

			cryptostream.Close();
			fsEncrypted.Close();
			fsInput.Close();
		}

		public static void DecryptFile(string inputFilename, string outputFilename, string key)
		{
			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			//A 64 bit key and IV is required for this provider.
			//Set secret key For DES algorithm.
			DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
			//Set initialization vector.
			DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

			//Create a file stream to read the encrypted file back.
			FileStream fsread = new FileStream(inputFilename,
										   FileMode.Open,
										   FileAccess.Read);
			//Create a DES decryptor from the DES instance.
			ICryptoTransform desdecrypt = DES.CreateDecryptor();
			//Create crypto stream set to read and do a
			//DES decryption transform on incoming bytes.
			CryptoStream cryptostreamDecr = new CryptoStream(fsread,
														 desdecrypt,
														 CryptoStreamMode.Read);
			//Print the contents of the decrypted file.
			StreamWriter fsDecrypted = new StreamWriter(outputFilename);
			fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
			fsDecrypted.Flush();

			fsDecrypted.Close();
			fsread.Close();
			cryptostreamDecr.Close();
		}
	}
}