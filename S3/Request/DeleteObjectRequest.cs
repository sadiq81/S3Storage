using System;
using S3Storage.S3;

namespace S3Storage.Request
{
	public class DeleteObjectRequest : BaseRequest
	{
		public DeleteObjectRequest (IRegion region, string bucketName, string objectName)
		{
			this.HttpMethod = "DELETE";
			this.Region = region;
			this.Uri = new Uri ("https://" + bucketName + "." + Region.LONG + "/" + objectName);
			this.Host = bucketName + "." + Region.LONG;
			this.Date = DateTime.UtcNow;
			this.Region = Region;
			this.XAmzContentSha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
			this.XAmzDate = Date.ToString ("yyyyMMddTHHmmssZ");
		}
	}
}

