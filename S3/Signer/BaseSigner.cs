﻿using System;
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
		private const  string AUTHORIZATION_SPLIT = ",";
		private const  string DIVIDER = "/";
		private const string HASH_OF_EMPTY_PAYLOAD = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
		private const string SIGNATURE = "AWS4";
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

		public string CreateAuthorization (string aWSAccessKeyId, string aWSSecretKey)
		{
			string canonicalRequest = BaseRequest.HttpMethod + NEW_LINE +
			                          BaseRequest.Uri.AbsolutePath.objectKeyNameToRfc3986 () + NEW_LINE +
			                          NEW_LINE +
			                          BaseRequest.GetSignedHeaders () +
			                          NEW_LINE +
			                          BaseRequest.GetCanonicalHeaders () + NEW_LINE +
			                          HASH_OF_EMPTY_PAYLOAD;
			string scope = BaseRequest.Date.ToString (SCOPE_DATE_FORMAT) + DIVIDER + BaseRequest.Region.SHORT + DIVIDER + SERVICE + DIVIDER + REQUEST;


			string stringToSign = SIGNATURE_VERSION + NEW_LINE +
			                      BaseRequest.Date.ToString (TIMESTAMP_DATE_FORMAT) + NEW_LINE +
			                      scope + NEW_LINE +
			                      HashService.CreateHash (Encoding.UTF8.GetBytes (canonicalRequest)).ToHexString (true);

			byte[] dateKey = HashService.CreateHash (
				                 Encoding.UTF8.GetBytes (SIGNATURE + aWSSecretKey),
				                 Encoding.UTF8.GetBytes (BaseRequest.Date.ToString (SCOPE_DATE_FORMAT)));

			byte[] dateRegionKey = HashService.CreateHash (
				                       dateKey,
				                       Encoding.UTF8.GetBytes (BaseRequest.Region.SHORT));

			byte[] dateRegionServiceKey = HashService.CreateHash (
				                              dateRegionKey,
				                              Encoding.UTF8.GetBytes (SERVICE));

			byte[] signingKey = HashService.CreateHash (
				                    dateRegionServiceKey,
				                    Encoding.UTF8.GetBytes (REQUEST));

			byte[] signatureBytes = HashService.CreateHash (
				                        signingKey,
				                        Encoding.UTF8.GetBytes (stringToSign));

			string signatureHex = signatureBytes.ToHexString (true);

			string authorization = "Credential=" + aWSAccessKeyId + DIVIDER + scope + AUTHORIZATION_SPLIT + "SignedHeaders=" + BaseRequest.GetCanonicalHeaders () + AUTHORIZATION_SPLIT + "Signature=" + signatureHex;

			return authorization;
		}

	}
}
