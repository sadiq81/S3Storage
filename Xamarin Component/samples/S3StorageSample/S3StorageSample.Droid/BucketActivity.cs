using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Provider;
using System.Threading.Tasks;
using S3Storage.AWSException;
using System.Collections.Generic;
using S3Storage.Service;
using S3Storage.S3;
using S3Storage.Response;
using Java.IO;
using System.IO;
using Android.Graphics;
using S3Storage.Model;

namespace S3StorageSample.Droid
{
	[Activity (Label = "Bucket", Icon = "@drawable/icon", ParentActivity = typeof(BucketsActivity), LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
	public class BucketActivity : Activity
	{
		private BucketAdapter Adapter;

		private string BucketName { get; set; }

		protected override void OnCreate (Bundle bundle)
		{

			BucketName = Intent.GetStringExtra ("bucketName");

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Bucket);
			ListView listView = (ListView)FindViewById (Resource.Id.ListView_Bucket);
			listView.Adapter = Adapter = new BucketAdapter (this);

			SwipeRefreshLayout refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.Refresher_Bucket);
			refresher.SetColorScheme (Resource.Color.xam_dark_blue,
				Resource.Color.xam_purple,
				Resource.Color.xam_gray,
				Resource.Color.xam_green);
			refresher.Refresh += async delegate {
				await RefreshContent ();
				refresher.Refreshing = false;
			};

			Initialize ();
		
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.MainMenu, menu);
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.New:
				{
					AddNewItem ();
					return true;
				}
			default:
				{
					return base.OnOptionsItemSelected (item);
				}
			}
		}

		private async void AddNewItem ()
		{


			try {

				string fileName = new Random ().Next (2) + ".jpg";
				var bitmap = BitmapFactory.DecodeStream (Assets.Open (fileName));


				byte[] bitmapData;
				using (var stream = new MemoryStream ()) {
					bitmap.Compress (Bitmap.CompressFormat.Png, 0, stream);
					bitmapData = stream.ToArray ();
				}
				string saveAs = DateTime.UtcNow.Ticks + "-" + fileName;

				await ServiceContainer.Resolve<S3ClientCore> ().PutObject (BucketName, saveAs, bitmapData);

				Adapter.Objects.Insert (0, new Contents (){ Key = saveAs });
				Adapter.NotifyDataSetChanged ();
			} catch (AWSErrorException e) {
				ShowAlert (e);
			}
		}

		private async Task RefreshContent ()
		{
			try {
				ListBucketResult result = await ServiceContainer.Resolve<S3ClientCore> ().GetBucket (BucketName);
				if (result.Contents != null) {
					Adapter.Objects = new List<Contents> (result.Contents);
					Adapter.NotifyDataSetChanged ();
				} else {
					Adapter.Objects = new List<Contents> ();
					Adapter.NotifyDataSetChanged ();
				}
			} catch (AWSErrorException e) {
				ShowAlert (e);
			}
		}


		private void ShowAlert (AWSErrorException e)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			AlertDialog dialog = builder.Create ();
			dialog.SetTitle ("Error");
			dialog.SetIcon (Android.Resource.Drawable.IcDialogAlert);
			dialog.SetMessage (e.ToString ());
			dialog.SetButton ("Ok", (s, ev) => {
			});
			dialog.Show ();

		}

		private async void Initialize ()
		{
			await RefreshContent ();
		}


		public class BucketAdapter : BaseAdapter<string>
		{
			private bool btnLock = false;

			private List<Contents> objects = new List<Contents> ();

			public List<Contents> Objects {
				get {
					return objects;
				}
				set {
					objects = value;
				}
			}

			private BucketActivity Context;

			public BucketAdapter (BucketActivity context) : base ()
			{
				this.Context = context;
			}

			public override long GetItemId (int position)
			{
				return position;
			}

			public override string this [int position] {  
				get { return objects [position].Key; }
			}

			public override int Count {
				get { return objects.Count; }
			}



			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				View view = convertView; // re-use an existing view, if one is supplied
				if (view == null) // otherwise create a new one
					view = Context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem1, null);
				// set view properties to reflect data for the given row
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = objects [position].Key;
				view.Click += (object sender, EventArgs e) => {
					Intent intent = new Intent (Context, typeof(DetailActivity));
					intent.PutExtra ("bucketName", Context.BucketName);
					intent.PutExtra ("objectName", objects [position].Key);
					Context.StartActivity (intent);
				};

				view.LongClick += (sender, e) => {

					if (!btnLock) {
						btnLock = true;
					

						AlertDialog.Builder alert = new AlertDialog.Builder (Context);

						alert.SetTitle ("Delete");
						alert.SetMessage ("Do you want delete this object?");
						alert.SetPositiveButton ("YES", async (dialog, args) => {

							try {
								await ServiceContainer.Resolve<S3ClientCore> ().DeleteObject (Context.BucketName, objects [position].Key);
								objects.RemoveAt (position);
								NotifyDataSetChanged ();
								NotifyDataSetInvalidated ();
							} catch (AWSErrorException exception) {
								AlertDialog.Builder alert2 = new AlertDialog.Builder (Context);
								alert2.SetTitle (exception.ToString ());
								alert2.Show ();
							} finally {
								btnLock = false;
							}

						});
						alert.SetNegativeButton ("NO", (dialog, args) => {
							btnLock = false;
						});
						alert.Show ();

					}

				};
					

				// return the view, populated with data, for display
				return view;
			}
		}

	}
}


