using System;
using S3Storage.Service;
using System.Security.Cryptography;

namespace S3StorageSample
{
	public class SHA256Service : ISHA256Service
	{
		public static HashAlgorithm CanonicalRequestHashAlgorithm = HashAlgorithm.Create ("SHA-256");

		public byte[] CreateHash (byte[] key, byte[] buffer)
		{
			var kha = KeyedHashAlgorithm.Create ("HMACSHA256");
			kha.Key = key;
			var hash_1 = kha.ComputeHash (buffer);
			return hash_1;

		}

		public byte[] CreateHash (byte[] buffer)
		{
			var hash_1 = CanonicalRequestHashAlgorithm.ComputeHash (buffer);
			return hash_1;
		}
	}
}

