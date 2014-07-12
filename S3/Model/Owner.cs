using System;
using System.Xml.Serialization;

namespace S3Storage.Model
{
	[XmlTypeAttribute]
	public class Owner
	{
		[XmlElement]
		public string ID { get; set; }

		[XmlElement]
		public string DisplayName { get; set; }

	}
}

