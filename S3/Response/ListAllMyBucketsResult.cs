using S3Storage.Model;
using System.Xml.Serialization;

namespace S3Storage.Response
{
	[XmlRoot]
	public class ListAllMyBucketsResult : BaseResult
	{
		[XmlElement]
		public Owner Owner { get; set; }

		[XmlArrayItem]
		public Bucket[] Buckets { get; set; }
	}

}

