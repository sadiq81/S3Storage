using System;

namespace S3Storage.Service
{
	public interface ISHA256Service
	{
		string CreateHash (string message, string secret);

		byte[] CreateHash (byte[] message, byte[] secret);

		byte[] CreateHash (byte[] buffer);

	}
}