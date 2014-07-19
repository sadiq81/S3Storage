
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

namespace S3StorageSample.Droid
{

	[Activity (Label = "DetailActivity")]	
	public class Detail : Activity
	{
		private string BucketName;
		private string ObjectName;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


			BucketName = Intent.GetStringExtra ("bucketName");
			ObjectName = Intent.GetStringExtra ("objectName");

			SetContentView (Resource.Layout.Detail);
			TextView textView = (TextView)FindViewById (Resource.Id.TextView);
			textView.Text = ObjectName;

			ActionBar.SetHomeButtonEnabled (true);
			ActionBar.SetDisplayHomeAsUpEnabled (true);


			// Create your application here
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				Finish ();
				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}
	}
}

