using System;
using System.Net.Http;
using System.Threading.Tasks;
using S3Storage.Response;
using S3Storage.Request;
using System.Globalization;
using System.Collections.Generic;
using S3Storage.Utils;
using S3Storage.Service;
using System.Text;
using S3Storage.Signer;

namespace S3Storage.S3
{
	public class S3ClientCore
	{
		public string AWSAccessKeyId { get; set; }

		public string AWSSecretKey { get; set; }

		public IRegion Region { get; set; }

		private HttpClient Client;

		public S3ClientCore (string aWSAccessKeyId, string aWSSecretKey, IRegion region)
		{
			this.AWSAccessKeyId = aWSAccessKeyId;
			this.AWSSecretKey = aWSSecretKey;
			this.Region = region;
		}

		public  Task<GetBucketsResponse> GetBuckets (GetBucketsRequest request)
		{
			request.Date = DateTime.UtcNow;
			request.Host = Region.SHORT;
			request.Region = Region;
			request.Uri = new Uri ("https://" + Region.LONG);

			BaseSigner signer = new BaseSigner (request);
			request.Authorization = signer.CreateAuthorization (AWSSecretKey)

			using (Client = new HttpClient ()) {

			}
			return null;
		}


	}
}

