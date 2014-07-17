using S3Storage.Response;
using System.Net;
using S3Storage.AWSException;

namespace S3Storage.Marshalling
{
	public class PutBucketUnMarshaller: BaseUnMarshaller<PutBucketResult>
	{
		new public  PutBucketResult UnMarshal ()
		{
			Result = new PutBucketResult ();

			if (Message.StatusCode.Equals (HttpStatusCode.OK)) {
				Result.HttpStatusCode = Message.StatusCode;
				Result.ContentLength = Message.Content.Headers.ContentLength;
				return Result;
			} else {
				Result.HttpStatusCode = Message.StatusCode;
				Result.ContentLength = Message.Content.Headers.ContentLength;
				Result.Errors = new System.Collections.Generic.List<BaseResult.Error> ();
				Result.Errors.Add (new BaseResult.Error () {
					Code = Result.HttpStatusCode.ToString (),
					Message = Message.Content.ReadAsStringAsync ().Result
				});
				throw new AWSErrorException (Result);
			}
		}
	}
}

