﻿using System;
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
using S3Storage.Model;

namespace S3Storage.S3
{
	public class S3ClientCore
	{
		public string AWSAccessKeyId { get; set; }

		public string AWSSecretKey { get; set; }

		public Region Region { get; set; }

		BaseSigner BaseSigner = new BaseSigner ();

		public S3ClientCore (string aWSAccessKeyId, string aWSSecretKey, Region region)
		{
			this.AWSAccessKeyId = aWSAccessKeyId;
			this.AWSSecretKey = aWSSecretKey;
			this.Region = region;
		}

		public async Task<ListAllMyBucketsResult> GetBuckets ()
		{
			GetBucketsRequest request = new GetBucketsRequest (Region);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, null);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClient (client, request);
				HttpResponseMessage response = await client.GetAsync (request.Uri);

				ListAllMyBucketsResultUnMarshaller unmarshaller = new ListAllMyBucketsResultUnMarshaller ();
				unmarshaller.Configure (response);

				ListAllMyBucketsResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		public async Task<ListBucketResult> GetBucket (string bucketName)
		{
			GetBucketRequest request = new GetBucketRequest (Region, bucketName);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, null);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClient (client, request);
				HttpResponseMessage response = await client.GetAsync (request.Uri);

				ListBucketResultUnMarshaller unmarshaller = new ListBucketResultUnMarshaller ();
				unmarshaller.Configure (response);

				ListBucketResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		public async Task<GetObjectResult> GetObject (string bucketName, string objectName)
		{
			GetObjectRequest request = new GetObjectRequest (Region, bucketName, objectName);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, null);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClient (client, request);
				HttpResponseMessage response = await client.GetAsync (request.Uri);

				GetObjectUnMarshaller unmarshaller = new GetObjectUnMarshaller ();
				unmarshaller.Configure (response);

				GetObjectResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		public async Task<PutObjectResult> PutObject (string bucketName, string objectName, byte[] buffer)
		{
			PutObjectRequest request = new PutObjectRequest (Region, bucketName, objectName, buffer);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, buffer);

			using (HttpClient client = new HttpClient ()) {
				
				ConfigureClient (client, request);
				HttpResponseMessage response = await client.PutAsync (request.Uri, new ByteArrayContent (buffer));

				PutObjectUnMarshaller unmarshaller = new PutObjectUnMarshaller ();
				unmarshaller.Configure (response);

				PutObjectResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		public async Task<DeleteBucketResult> DeleteBucket (string bucketName)
		{
			DeleteBucketRequest request = new DeleteBucketRequest (Region, bucketName);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, null);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClient (client, request);
				HttpResponseMessage response = await client.DeleteAsync (request.Uri);

				DeleteBucketUnMarshaller unmarshaller = new DeleteBucketUnMarshaller ();
				unmarshaller.Configure (response);

				DeleteBucketResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		public async Task<DeleteObjectResult> DeleteObject (string bucketName, string objectName)
		{
			DeleteObjectRequest request = new DeleteObjectRequest (Region, bucketName, objectName);

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, null);

			using (HttpClient client = new HttpClient ()) {
				ConfigureClient (client, request);
				HttpResponseMessage response = await client.DeleteAsync (request.Uri);

				DeleteObjectUnMarshaller unmarshaller = new DeleteObjectUnMarshaller ();
				unmarshaller.Configure (response);

				DeleteObjectResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		//Authorization
		//Content-Length
		//Date
		//Host
		//SHA256

		public async Task<PutBucketResult> PutBucket (string bucketName, CreateBucketConfiguration configuration)
		{
			string content = configuration != null ? configuration.SerializeObject<CreateBucketConfiguration> () : "";

			PutBucketRequest request = new PutBucketRequest (Region, bucketName, Encoding.UTF8.GetBytes (content));

			BaseSigner.CreateAuthorization (request, AWSAccessKeyId, AWSSecretKey, Encoding.UTF8.GetBytes (content));

			using (HttpClient client = new HttpClient ()) {

				ConfigureClient (client, request);
				HttpResponseMessage response = client.PutAsync (request.Uri, new ByteArrayContent (Encoding.UTF8.GetBytes (content))).Result;

				PutBucketUnMarshaller unmarshaller = new PutBucketUnMarshaller ();
				unmarshaller.Configure (response);

				PutBucketResult result = unmarshaller.UnMarshal ();
				return result;
			}
		}

		private void ConfigureClient (HttpClient client, BaseRequest request)
		{
			foreach (KeyValuePair<string,string> kvp in request.GetHeaders()) {

				if (kvp.Key.Equals ("host", StringComparison.OrdinalIgnoreCase)) {
					client.DefaultRequestHeaders.Host = kvp.Value;
				} else if (kvp.Key.Equals ("authorization", StringComparison.OrdinalIgnoreCase)) {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("AWS4-HMAC-SHA256", kvp.Value);
				} else if (kvp.Key.Equals ("content-length", StringComparison.OrdinalIgnoreCase)) {
					//Do nothing
				} else if (kvp.Key.Equals ("content-type", StringComparison.OrdinalIgnoreCase)) {
					//Do nothing
				} else if (kvp.Key.Equals ("date", StringComparison.OrdinalIgnoreCase)) {
					client.DefaultRequestHeaders.Date = request.Date;
				} else {
					client.DefaultRequestHeaders.Add (kvp.Key, kvp.Value);
				}
			}

		}
	}
}

