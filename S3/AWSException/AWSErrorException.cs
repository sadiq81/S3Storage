using System;
using S3Storage.Response;
using System.Text;

namespace S3Storage.AWSException
{
	public class AWSErrorException : Exception
	{
		public BaseResult Response { get; set; }

		public AWSErrorException (BaseResult response)
		{
			this.Response = response;
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder ();
			foreach (BaseResult.Error error in Response.Errors) {
				sb.Append (error.ToString () + " ");
			}
			return string.Format ("[AWSErrorException: HttpStatusCode={0}, Errors={1}]", Response.HttpStatusCode, sb.ToString ());
		}
	}
}

