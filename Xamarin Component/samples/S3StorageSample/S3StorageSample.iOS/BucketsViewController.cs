// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using S3Storage.Model;
using S3Storage.Service;
using S3Storage.AWSException;
using System.Threading.Tasks;
using S3Storage.S3;
using S3Storage.Response;

namespace S3StorageSample.iOS
{
	public partial class BucketsViewController : UITableViewController
	{
		DataSource dataSource;

		public BucketsViewController (IntPtr handle) : base (handle)
		{
			Title = "Buckets";
		}

		async void AddNewItem (object sender, EventArgs args)
		{
			string bucketName = (UIDevice.CurrentDevice.IdentifierForVendor.AsString () + "-" + DateTime.Now.Ticks).ToLower ();

			try {
				await ServiceContainer.Resolve<S3ClientCore> ().PutBucket (bucketName, new CreateBucketConfiguration (LocationConstraint.EUWest_1));
				dataSource.Objects.Insert (0, new Bucket{ Name = bucketName });
				using (var indexPath = NSIndexPath.FromRowSection (0, 0))
					TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
			} catch (AWSErrorException e) {
				UIAlertView alert = new UIAlertView ("Error", e.ToString (), null, "OK", null);
				alert.Show ();
			}

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, AddNewItem);
			NavigationItem.RightBarButtonItem = addButton;

			TableView.Source = dataSource = new DataSource (this, TableView);

			RefreshControl = new UIRefreshControl ();
			RefreshControl.ValueChanged += async (object sender, EventArgs e) => {
				RefreshControl.BeginRefreshing ();
				await dataSource.RefreshData ();
				RefreshControl.EndRefreshing ();
				TableView.ReloadData ();
			};

		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "bucketSeque") {
				var indexPath = TableView.IndexPathForSelectedRow;
				var item = dataSource.Objects [indexPath.Row];

				((BucketViewController)segue.DestinationViewController).Title = item.Name;
			}
		}


		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("Cell");
			List<Bucket> objects = new List<Bucket> ();

			readonly BucketsViewController Controller;
			readonly UITableView TableView;

			public DataSource (BucketsViewController controller, UITableView uiTableView)
			{
				this.Controller = controller;
				this.TableView = uiTableView;
				Initialize ();
			}

			private async void Initialize ()
			{
				await RefreshData ();
				TableView.ReloadData ();
			}

			public async Task RefreshData ()
			{
				try {
					ListAllMyBucketsResult result = await ServiceContainer.Resolve<S3ClientCore> ().GetBuckets ();
					if (result.Buckets != null) {
						this.Objects = new List<Bucket> (result.Buckets);
					} else {
						this.Objects = new List<Bucket> ();
					}
				} catch (AWSErrorException e) {
					UIAlertView alert = new UIAlertView ("Error", e.ToString (), null, "OK", null);
					alert.Show ();
				}
			}

			public IList<Bucket> Objects {
				get { return objects; }
				set { objects = (List<Bucket>)value; }
			}

			// Customize the number of sections in the table view.
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return objects.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (UITableViewCell)tableView.DequeueReusableCell (CellIdentifier, indexPath);

				cell.TextLabel.Text = objects [indexPath.Row].Name;

				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}

			public async override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					// Delete the row from the data source.
					try {
						await ServiceContainer.Resolve<S3ClientCore> ().DeleteBucket (objects [indexPath.Row].Name);
						objects.RemoveAt (indexPath.Row);
						Controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					} catch (AWSErrorException e) {
						UIAlertView alert = new UIAlertView ("Error", e.ToString (), null, "OK", null);
						alert.Show ();
					}
				} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}

			/*
			// Override to support rearranging the table view.
			public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
			}
			*/

			/*
			// Override to support conditional rearranging of the table view.
			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the item to be re-orderable.
				return true;
			}
			*/
		}
	}
}
