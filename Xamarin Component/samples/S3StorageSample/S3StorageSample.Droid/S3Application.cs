using System;
using Android.App;
using Android.Runtime;
using S3Storage.Service;
using S3Storage.S3;
using S3Storage;
using S3StorageSample.Core.Services;

namespace S3StorageSample.Droid
{
	[Application]
	public class S3Application :Application
	{
		public S3Application (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}

		public override void OnCreate ()
		{
			base.OnCreate ();

			ServiceContainer.Register<ISHA256Service> (() => new SHA256Service ());
			ServiceContainer.Register<S3ClientCore> (() => new S3ClientCore ("KEY", "SECRET", Region.EUWest_1));
		}
	}
}

