using System;

namespace S3Storage.Model
{
	public  class ACL
	{
		public static string Header { get; set; }

		public static ACL PRIVATE = new ACL ("private");
		public static ACL PUBLIC_READ = new ACL ("public-read");
		public static ACL PUBLIC_READ_WRITE = new ACL ("public-read-write");
		public static ACL AUTHENTICATED_READ = new ACL ("authenticated_read");
		public static ACL BUCKET_OWNER_READ = new ACL ("bucket-owner-read");
		public static ACL BUCKET_OWNER_FULL_CONTROL = new ACL ("bucket-owner-full-control");

		ACL (string header)
		{
			Header = header;
		}

	}
}

