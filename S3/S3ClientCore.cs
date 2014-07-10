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
using System.Net.Http.Headers;

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
			request.Uri = new Uri ("https://halalguide." + Region.LONG + "/MySampleFileChunked.txt");
			request.Host = "halalguide." + Region.LONG;
			request.Date = DateTime.UtcNow;
			request.Region = Region;
			request.XAmzContentSha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
			request.XAmzDate = request.Date.ToString ("yyyyMMddTHHmmssZ");


			BaseSigner signer = new BaseSigner (request);
			request.Authorization = signer.CreateAuthorization (AWSAccessKeyId, AWSSecretKey);

			using (Client = new HttpClient ()) {
				HttpRequestMessage message = new HttpRequestMessage (HttpMethod.Get, request.Uri);

				Client.DefaultRequestHeaders.Host = request.Host;
				Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("AWS4-HMAC-SHA256", request.Authorization);
				Client.DefaultRequestHeaders.Add ("x-amz-date", request.XAmzDate);
				Client.DefaultRequestHeaders.Add ("x-amz-content-sha256", request.XAmzContentSha256);
				HttpResponseMessage response = Client.GetAsync (request.Uri).Result;
				int i = 0;
			}
			return null;
		}


	}
}

