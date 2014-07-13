using System;
using S3Storage.S3;
using System.IO;
using System.Collections.Generic;
using S3Storage.Model;

namespace S3Storage.Request
{
	public class PutBucketRequest : BaseRequest
	{
		public string ACL { get; set; }

		public List<string> XAmzGrantRead { get; set; }

		public List<string> XAmzGrantWrite { get; set; }

		public List<string> XAmzGrantReadAcp { get; set; }

		public List<string> XAmzGrantWriteAcp { get; set; }

		public List<string> XAmzGrantFullControl{ get; set; }

		public PutBucketRequest (string bucketName, byte[] buffer)
		{
			this.HttpMethod = "PUT";
			this.Region = Region.USEast_1;
			this.Uri = new Uri ("https://" + bucketName + "." + Region.USEast_1.LONG);
			this.Host = bucketName + "." + Region.USEast_1.LONG;
			this.Date = DateTime.UtcNow;
			this.ContentLength = buffer.GetLength (0).ToString ();
			this.XAmzDate = Date.ToString ("yyyyMMddTHHmmssZ");

		}

	}
}

