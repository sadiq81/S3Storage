
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using S3StorageSample.Droid;
using S3Storage.Response;
using S3Storage.Service;
using S3Storage.S3;
using Android.Graphics;

namespace S3StorageSample.Droid
{

	[Activity (Label = "DetailActivity", ParentActivity = typeof(BucketActivity))]	
	public class DetailActivity : Activity
	{
		private string BucketName;
		private string ObjectName;

		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			BucketName = Intent.GetStringExtra ("bucketName");
			ObjectName = Intent.GetStringExtra ("objectName");

			GetObjectResult result = await ServiceContainer.Resolve<S3ClientCore> ().GetObject (BucketName, ObjectName);
			byte[] buffer = new byte[result.Stream.Length];
			result.Stream.Read (buffer, 0, buffer.Length);

			SetContentView (Resource.Layout.Detail);
			ImageView imageView = FindViewById<ImageView> (Resource.Id.imageView1);

			var imageBitmap = BitmapFactory.DecodeByteArray (buffer, 0, buffer.Length);
			RunOnUiThread (() => imageView.SetImageBitmap (imageBitmap));
		
			// Create your application here
		}

	}
}

