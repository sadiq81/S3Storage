using System;
using S3Storage.Response;
using System.Net;
using S3Storage.AWSException;

namespace S3Storage.Marshalling
{
	public class DeleteObjectUnMarshaller : BaseUnMarshaller<DeleteObjectResult>
	{

		public  DeleteObjectResult UnMarshal ()
		{
			Result = new DeleteObjectResult ();

			if (Message.StatusCode.Equals (HttpStatusCode.NoContent)) {
				Result.HttpStatusCode = Message.StatusCode;
				Result.ContentLength = Message.Content.Headers.ContentLength;
				return Result;
			} else {
				Result.HttpStatusCode = Message.StatusCode;
				Result.ContentLength = Message.Content.Headers.ContentLength;
				throw new AWSErrorException (Result);
			}


		}
	}
}

