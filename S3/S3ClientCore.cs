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
using S3Storage.Marshalling;

namespace S3Storage.S3
{
	public class S3ClientCore
	{
		public string AWSAccessKeyId { get; set; }

		public string AWSSecretKey { get; set; }

		public IRegion Region { get; set; }

		BaseSigner BaseSigner = new BaseSigner ();

		public S3ClientCore (string aWSAccessKeyId, string aWSSecretKey, IRegion region)
		{
			this.AWSAccessKeyId = aWSAccessKeyId;
			this.AWSSecretKey = aWSSecretKey;
			this.Region = region;
		}

		public async Task<ListAllMyBucketsResult> GetBuckets ()
		{
			GetBucketsRequest request = new GetBucketsRequest (Region);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClientGet (client, request);
				HttpResponseMessage response = client.GetAsync (request.Uri).Result;

				ListAllMyBucketsResultUnMarshaller unmarshaller = new ListAllMyBucketsResultUnMarshaller ();
				unmarshaller.Configure (response);

				ListAllMyBucketsResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		public async Task<ListBucketResult> GetBucket (string bucketName)
		{
			GetBucketRequest request = new GetBucketRequest (Region, bucketName);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClientGet (client, request);
				HttpResponseMessage response = client.GetAsync (request.Uri).Result;

				ListBucketResultUnMarshaller unmarshaller = new ListBucketResultUnMarshaller ();
				unmarshaller.Configure (response);

				ListBucketResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		private void ConfigureClientGet (HttpClient client, BaseRequest request)
		{
			foreach (KeyValuePair<string,string> kvp in request.GetHeaders()) {

				if (kvp.Key.Equals ("host", StringComparison.OrdinalIgnoreCase)) {
					client.DefaultRequestHeaders.Host = kvp.Value;
				} else if (kvp.Key.Equals ("authorization", StringComparison.OrdinalIgnoreCase)) {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("AWS4-HMAC-SHA256", kvp.Value);
				} else {
					client.DefaultRequestHeaders.Add (kvp.Key, kvp.Value);
				}
			}
		}
	}
}

