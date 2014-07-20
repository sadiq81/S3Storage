
using MonoTouch.UIKit;
using S3Storage;
using S3Storage.S3;
using S3Storage.Service;
using S3StorageSample.Core.Services;

namespace S3StorageSample.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			ServiceContainer.Register<ISHA256Service> (() => new SHA256Service ());
			ServiceContainer.Register<S3ClientCore> (() => new S3ClientCore ("KEY", "SECRET", Region.EUWest_1));
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.

			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
