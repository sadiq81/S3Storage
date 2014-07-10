using System;

namespace S3Storage.Service
{
	public interface ISHA256Service
	{
		byte[] CreateHash (byte[] key, byte[] buffer);

		byte[] CreateHash (byte[] buffer);

	}
}