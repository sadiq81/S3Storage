using System;
using S3Storage.Response;
using System.Net;
using S3Storage.AWSException;

namespace S3Storage.Marshalling
{
	public class PutObjectUnMarshaller: BaseUnMarshaller<PutObjectResult>
	{
		new public  PutObjectResult UnMarshal ()
		{
			Result = new PutObjectResult ();

			if (Message.StatusCode.Equals (HttpStatusCode.OK)) {
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

