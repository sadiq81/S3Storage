using System;
using S3Storage.Request;
using S3Storage.Utils;
using S3Storage.Service;
using System.Text;

namespace S3Storage.Signer
{
	public class BaseSigner
	{
		public BaseRequest BaseRequest { get; set; }

		private const  string NEW_LINE = "\n";
		private const  string DIVIDER = "\\";
		private const string HASH_OF_EMPTY_PAYLOAD = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
		private const string SIGNATURE_VERSION = "AWS4-HMAC-SHA256";
		private const string TIMESTAMP_DATE_FORMAT = "yyyyMMddTHHmmssZ";
		private const string SCOPE_DATE_FORMAT = "yyyyMMdd";
		private const string SERVICE = "s3";
		private const string REQUEST = "aws4_request";

		private ISHA256Service HashService = ServiceContainer.Resolve<ISHA256Service> ();

		public BaseSigner (BaseRequest baseRequest)
		{
			this.BaseRequest = baseRequest;
		}

		public string CreateAuthorization (string aWSSecretKey)
		{
			string canonicalRequest = BaseRequest.HttpMethod + NEW_LINE +
			                          DIVIDER + BaseRequest.Uri.AbsolutePath.ToRfc3986 () + NEW_LINE +
			                          NEW_LINE +
			                          BaseRequest.GetCanonicalHeaders () + NEW_LINE +
			                          HASH_OF_EMPTY_PAYLOAD;

			string stringToSign = SIGNATURE_VERSION + NEW_LINE +
			                      BaseRequest.Date.ToString (TIMESTAMP_DATE_FORMAT) + NEW_LINE +
			                      BaseRequest.Date.ToString (SCOPE_DATE_FORMAT) + DIVIDER + BaseRequest.Region.SHORT + DIVIDER + SERVICE + DIVIDER + REQUEST + NEW_LINE +
			                      HashService.CreateHash (Encoding.UTF8.GetBytes (canonicalRequest));

			byte[] dateKey = HashService.CreateHash (Encoding.UTF8.GetBytes (BaseRequest.Date.ToString (SCOPE_DATE_FORMAT)),
				                 Encoding.UTF8.GetBytes ("AWS4" + aWSSecretKey));

			byte[] dateRegionKey = HashService.CreateHash (Encoding.UTF8.GetBytes (BaseRequest.Region.SHORT), dateKey);

			byte[] dateRegionServiceKey = HashService.CreateHash (Encoding.UTF8.GetBytes (SERVICE), dateRegionKey);

			byte[] signingKey = HashService.CreateHash (Encoding.UTF8.GetBytes (REQUEST), dateRegionServiceKey);

			byte[] signatureBytes = HashService.CreateHash (Encoding.UTF8.GetBytes (stringToSign), signingKey);

			string signature = Convert.ToBase64String (signatureBytes);

			return signature;
		}

	}
}

