using System;
using S3Storage.S3;
using System.IO;

namespace S3Storage.Request
{
	public class PutObjectRequest : BaseRequest
	{
		byte[] buffer { get; set; }

		public PutObjectRequest (Region region, string bucketName, string objectName, byte[] buffer)
		{
			this.HttpMethod = "PUT";
			this.Region = region;
			this.Uri = new Uri ("https://" + bucketName + "." + Region.LONG + "/" + objectName);
			this.Host = bucketName + "." + Region.LONG;
			this.Date = DateTime.UtcNow;
			this.Region = Region;
			this.XAmzDate = Date.ToString ("yyyyMMddTHHmmssZ");
			this.ContentLength = buffer.GetLength (0).ToString ();

		}

	}
}

