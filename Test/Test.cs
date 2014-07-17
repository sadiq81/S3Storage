using NUnit.Framework;
using System;
using S3Storage.Service;
using S3Storage.S3;
using S3Storage;
using S3Storage.Request;
using System.Text;
using S3Storage.Model;
using S3Storage.Response;
using System.Net;

namespace Test
{
	[TestFixture ()]
	public class Test
	{
		S3ClientCore Client;

		public static string BucketName = "testhalalguide";
		public static string ObjectText = "hello dolly";
		public static string FileName = "PutObject.txt";

		[TestFixtureSetUp ()]
		public void BeforeAllTests ()
		{
			ServiceContainer.Register<ISHA256Service> (() => new SHA256Service ());
			ServiceContainer.Register<S3ClientCore> (() => new S3ClientCore (Key.AWSAccessKeyId, Key.AWSSecretKey, Region.EUWest_1));
			Client = ServiceContainer.Resolve<S3ClientCore> ();

			PutBucketResult response = Client.PutBucket (BucketName, new CreateBucketConfiguration (LocationConstraint.EUWest_1)).Result;
			Assert.AreEqual (HttpStatusCode.OK, response.HttpStatusCode);
		}

		[TestFixtureTearDown ()]
		public void AfterAllTests ()
		{
			DeleteBucketResult response = Client.DeleteBucket (BucketName).Result;
			Assert.AreEqual (HttpStatusCode.OK, response.HttpStatusCode);
		}

		[SetUp ()]
		public  void BeforeEachTest ()
		{

		}

		[TearDown ()]
		public  void AfterEachTest ()
		{

		}

		[Test ()]
		public void TestGetBuckets ()
		{
			ListAllMyBucketsResult response = Client.GetBuckets ().Result;
			Assert.AreEqual (response.Buckets.Length, 2);
			Assert.AreEqual (BucketName, response.Buckets [1].Name);
			Assert.AreEqual (HttpStatusCode.OK, response.HttpStatusCode);
		}

		[Test ()]
		public void TestGetBucket ()
		{
			ListBucketResult response = Client.GetBucket (BucketName).Result;
			Assert.AreEqual (BucketName, response.Name);
			Assert.AreEqual (HttpStatusCode.OK, response.HttpStatusCode);
		}

		[Test ()]
		public void TestGetObject ()
		{
			byte[] message = Encoding.UTF8.GetBytes (ObjectText);

			PutObjectResult response = Client.PutObject (BucketName, FileName, message).Result;
			Assert.AreEqual (HttpStatusCode.OK, response.HttpStatusCode);

			GetObjectResult response2 = Client.GetObject (BucketName, FileName).Result;
			Assert.AreEqual (HttpStatusCode.OK, response2.HttpStatusCode);

			byte[] buffer = new byte[response2.Stream.Length];
			int i = response2.Stream.ReadAsync (buffer, 0, buffer.Length).Result;
			string messageReturned = Encoding.UTF8.GetString (buffer);
			Assert.AreEqual (ObjectText, messageReturned);

			DeleteObjectResult response3 = Client.DeleteObject (BucketName, FileName).Result;
			Assert.AreEqual (HttpStatusCode.NoContent, response3.HttpStatusCode);
		}
	}
}

