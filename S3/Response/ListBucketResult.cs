using System;
using System.Xml.Serialization;
using S3Storage.Model;

namespace S3Storage.Response
{
	[XmlRoot]
	public class ListBucketResult : BaseResponse
	{
		[XmlElementAttribute]
		public Contents[] Contents { get; set; }

		[XmlElement]
		public string CommonPrefixes { get; set; }

		[XmlElement]
		public string Delimiter { get; set; }

		[XmlElement ("Encoding-Type")]
		public string EncodingType { get; set; }

		[XmlElement]
		public bool IsTruncated { get; set; }

		[XmlElement]
		public string Marker { get; set; }

		[XmlElement]
		public string MaxKeys{ get; set; }

		[XmlElement]
		public string Name{ get; set; }

		[XmlElement]
		public string NextMarker{ get; set; }

		[XmlElement]
		public string Prefix{ get; set; }


	}
}

