using System;
using S3Storage.Service;
using System.Security.Cryptography;
using System.Text;

namespace Test
{
	public class SHA256Service : ISHA256Service
	{
		public byte[] CreateHash (byte[] message, byte[] secret)
		{
			using (HMACSHA256 hmacsha256 = new HMACSHA256 (secret)) {
				byte[] hashmessage = hmacsha256.ComputeHash (message);
				return hashmessage;
			}
		}

		public string CreateHash (string message, string secret)
		{
			using (HMACSHA256 hmacsha256 = new HMACSHA256 ()) {
				hmacsha256.Key = Encoding.UTF8.GetBytes (secret);
				byte[] hashmessage = hmacsha256.ComputeHash (Encoding.UTF8.GetBytes (message));
				return Convert.ToBase64String (hashmessage);
			}
		}

		public byte[] CreateHash (byte[] buffer)
		{
			using (HMACSHA256 hmacsha256 = new HMACSHA256 ()) {
				byte[] hashmessage = hmacsha256.ComputeHash (buffer);
				return hashmessage;
			}
		}
	}
}

