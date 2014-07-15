using System;
using S3Storage.S3;

namespace S3Storage
{

	public  class Region
	{
		public  string SHORT { get; set; }


		public  string LONG { get; set; }

		public static Region EUWest_1 = new Region ("eu-west-1", "s3-eu-west-1.amazonaws.com");

		public static Region USEast_1 = new Region ("us-east-1", "s3.amazonaws.com");

		public static Region USWest_1 = new Region ("us-west-1", "s3-us-west-1.amazonaws.com");
		public static Region USWest_2 = new Region ("us-west-2", "s3-us-west-2.amazonaws.com");

		public static Region APSouthEast_1 = new Region ("ap-southeast-1", "s3-ap-southeast-1.amazonaws.com");
		public static Region APSouthEast_2 = new Region ("ap-southeast-2", "s3-ap-southeast-2.amazonaws.com");

		public static Region APNorthEast_1 = new Region ("ap-northeast-1", "s3-ap-northeast-1.amazonaws.com");

		public static Region SAEast_1 = new Region ("sa-east-1", "s3-sa-east-1.amazonaws.com");

		Region (string sHORT, string lONG)
		{
			this.SHORT = sHORT;
			this.LONG = lONG;
		}
		
		
	}

}

