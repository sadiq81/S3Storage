using System;
using S3Storage.S3;

namespace S3Storage
{
	public  class USEast_1 : IRegion
	{
		public   string SHORT {
			get {
				return "us-east-1";
			}
		}

		public   string LONG {
			get {
				return "s3.amazonaws.com";
			}
		}
	}

	public  class USWest_1: IRegion
	{
		public   string SHORT {
			get {
				return "us-west-1";
			}
		}

		public   string LONG {
			get {
				return "s3-us-west-1.amazonaws.com";
			}
		}
	}

	public  class USWest_2: IRegion
	{
		public    string SHORT {
			get {
				return "us-west-2";
			}
		}

		public    string LONG {
			get {
				return "s3-us-west-2.amazonaws.com";
			}
		}
	}

	public  class EUWest_1: IRegion
	{
		public   string SHORT {
			get {
				return "eu-west-1";
			}
		}

		public   string LONG {
			get {
				return "s3-eu-west-1.amazonaws.com";
			}
		}
	}

	public  class APSouthEast_1: IRegion
	{
		public    string SHORT {
			get {
				return "ap-southeast-1";
			}
		}

		public    string LONG {
			get {
				return "s3-ap-southeast-1.amazonaws.com";
			}
		}
	}

	public  class APSouthEast_2: IRegion
	{
		public    string SHORT {
			get {
				return "ap-southeast-2";
			}
		}

		public    string LONG {
			get {
				return "s3-ap-southeast-2.amazonaws.com";
			}
		}
	}

	public  class APNorthEast_1: IRegion
	{
		public    string SHORT {
			get {
				return "ap-northeast-1";
			}
		}

		public    string LONG {
			get {
				return "s3-ap-northeast-1.amazonaws.com";
			}
		}
	}

	public  class SAEast_1: IRegion
	{
		public    string SHORT {
			get {
				return "sa-east-1";
			}
		}

		public    string LONG {
			get {
				return "s3-sa-east-1.amazonaws.com";
			}
		}
	}
}

