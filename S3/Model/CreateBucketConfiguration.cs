using System;
using System.Xml.Serialization;

namespace S3Storage.Model
{
	[XmlRoot (Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
	public class CreateBucketConfiguration
	{
		[XmlElement]
		public string LocationConstraint { get; set; }

		public CreateBucketConfiguration ()
		{
			LocationConstraint = "";
		}

		public CreateBucketConfiguration (LocationConstraint constraint)
		{
			this.LocationConstraint = constraint.Location;
		}
	}
}

