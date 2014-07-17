using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using S3Storage.S3;
using S3Storage.Service;
using S3Storage.Response;
using S3Storage.Model;

namespace S3StorageSample.iOS
{
	public partial class DetailViewController : UIViewController
	{

		public string BucketName { get; set; }

		public Contents Contents { get; set; }

		public DetailViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			GetObjectResult result = await ServiceContainer.Resolve<S3ClientCore> ().GetObject (BucketName, Contents.Key);
			byte[] buffer = new byte[result.Stream.Length];
			result.Stream.Read (buffer, 0, buffer.Length);
			UIImage image = new UIImage (NSData.FromArray (buffer));
			UIImageView view = new UIImageView (image);
			view.Frame = new RectangleF (10, 10, (this.View.Frame.Width - 20), (this.View.Frame.Height - 20));
			Add (view);
		}
	}
}

