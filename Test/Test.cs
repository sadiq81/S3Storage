using NUnit.Framework;
using System;
using S3Storage.Service;
using S3Storage.S3;
using S3Storage;

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
			ServiceContainer.Register<S3ClientCore> (() => new S3ClientCore ("AKIAJCSIBUKJK42X5XLA", "de2Rssuss5rQrWLPbTvyEdQa6QaIrfmnnLMV8wY0", new EUWest_1 ()));
			Client = ServiceContainer.Resolve<S3ClientCore> ();
		}

		[Test ()]
		public void TestCase ()
		{
			Client.GetBuckets (null);
		}
	}
}

