using NUnit.Framework;
using System;
using S3Storage.Service;
using S3Storage.S3;
using S3Storage;
using S3Storage.Request;

namespace Test
{
	[TestFixture ()]
	public class Test
	{
		S3ClientCore Client;

		[TestFixtureSetUp ()]
		public void BeforeAllTests ()
		{
			ServiceContainer.Register<ISHA256Service> (() => new SHA256Service ());
			ServiceContainer.Register<S3ClientCore> (() => new S3ClientCore (Key.AWSAccessKeyId, Key.AWSSecretKey, new EUWest_1 ()));
			Client = ServiceContainer.Resolve<S3ClientCore> ();
		}

		[Test ()]
		public void TestCase ()
		{
			//Client.GetBuckets ();

			Client.GetBucket ("halalguide");
		}
	}
}

