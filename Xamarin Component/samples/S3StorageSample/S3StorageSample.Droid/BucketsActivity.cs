using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using S3Storage.AWSException;
using S3Storage.Service;
using S3Storage.S3;
using S3Storage.Model;
using S3Storage.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Provider;

namespace S3StorageSample.Droid
{
	[Activity (Label = "Buckets", MainLauncher = true, Icon = "@drawable/icon")]
	public class BucketsActivity : Activity
	{
		private BucketsAdapter Adapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Buckets);
			ListView listView = (ListView)FindViewById (Resource.Id.ListView_Buckets);
			listView.Adapter = Adapter = new BucketsAdapter (this);

			SwipeRefreshLayout refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.Refresher_Buckets);
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


			string bucketName = (Settings.Secure.AndroidId + "-" + DateTime.Now.Ticks.ToString ()).ToLower ().Replace ("_", "-");

			try {
				await ServiceContainer.Resolve<S3ClientCore> ().PutBucket (bucketName, new CreateBucketConfiguration (LocationConstraint.EUWest_1));
				Adapter.Objects.Insert (0, new Bucket (){ Name = bucketName });
				Adapter.NotifyDataSetChanged ();
			} catch (AWSErrorException e) {
				ShowAlert (e);
			}
		}

		private async Task RefreshContent ()
		{
			try {
				ListAllMyBucketsResult result = await ServiceContainer.Resolve<S3ClientCore> ().GetBuckets ();
				if (result.Buckets != null) {
					Adapter.Objects = new List<Bucket> (result.Buckets);
					Adapter.NotifyDataSetChanged ();
				} else {
					Adapter.Objects = new List<Bucket> ();
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

		public class BucketsAdapter : BaseAdapter<string>
		{
			private Activity Context;

			private bool btnLock = false;

			private List<Bucket> objects = new List<Bucket> ();

			public BucketsAdapter (Activity context) : base ()
			{
				this.Context = context;
			}

			public List<Bucket> Objects {
				get {
					return objects;
				}
				set {
					objects = value;
				}
			}

			public override long GetItemId (int position)
			{
				return position;
			}

			public override string this [int position] {  
				get { return objects [position].Name; }
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
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = objects [position].Name;
				view.Click += (object sender, EventArgs e) => {
					Intent intent = new Intent (Context, typeof(BucketActivity));
					intent.PutExtra ("bucketName", objects [position].Name);
					Context.StartActivity (intent);
				};

				view.LongClick += (sender, e) => {

					if (!btnLock) {
						btnLock = true;


						AlertDialog.Builder alert = new AlertDialog.Builder (Context);

						alert.SetTitle ("Delete");
						alert.SetMessage ("Do you want delete this bucket?");
						alert.SetPositiveButton ("YES", async (dialog, args) => {

							try {
								await ServiceContainer.Resolve<S3ClientCore> ().DeleteBucket (objects [position].Name);
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


