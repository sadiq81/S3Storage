using System;
using System.Xml.Serialization;

namespace S3Storage.Model
{
	public  class LocationConstraint
	{
		public string Location { get; set; }

		public static LocationConstraint EUWest_1 = new LocationConstraint ("EU");

		public static LocationConstraint USEast_1 = new LocationConstraint ("");
		public static LocationConstraint USWest_1 = new LocationConstraint ("us-west-1");
		public static LocationConstraint USWest_2 = new LocationConstraint ("us-west-2");

		public static LocationConstraint APSouthEast_1 = new LocationConstraint ("ap-southeast-1");
		public static LocationConstraint APSouthEast_2 = new LocationConstraint ("ap-southeast-2");

		public static LocationConstraint APNorthEast_1 = new LocationConstraint ("ap-northeast-1");

		public static LocationConstraint SAEast_1 = new LocationConstraint ("sa-east-1");

		public LocationConstraint (string location)
		{
			this.Location = location;
		}

		public override string ToString ()
		{
			return Location;
		}
		

	}
}

