using System;
using System.Xml.Serialization;

namespace S3Storage.Model
{
	[XmlTypeAttribute]
	public class Bucket
	{
		[XmlElement]
		public string Name { get; set; }

		[XmlElement]
		public DateTime CreationDate { get; set; }

	}
}

