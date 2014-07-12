using System;
using System.Xml.Serialization;

namespace S3Storage.Model
{
	[XmlTypeAttribute]
	public class Contents
	{
		[XmlElement]
		public string Key;

		[XmlElement]
		public DateTime LastModified;

		[XmlElement]
		public string ETag;

		[XmlElementAttribute]
		public Owner Owner;

		[XmlElement]
		public long Size;

		[XmlElementAttribute]
		public StorageClass StorageClass;

	}
}

