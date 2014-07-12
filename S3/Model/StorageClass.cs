using System;
using System.Xml.Serialization;

namespace S3Storage.Model
{
	[XmlTypeAttribute]
	public enum StorageClass
	{
		STANDARD,
		REDUCED_REDUNDANCY,
		GLACIER,
	}
}

