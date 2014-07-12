using System;
using System.IO;

namespace S3Storage.Response
{
	public class GetObjectResult : BaseResult
	{
		public MemoryStream Stream { get; set; }

	}
}

