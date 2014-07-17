using S3Storage.Response;
using System.Net;
using S3Storage.AWSException;
using System.Xml.Serialization;
using System.IO;

namespace S3Storage.Marshalling
{
	public class GetObjectUnMarshaller : BaseUnMarshaller<GetObjectResult>
	{



		new public  GetObjectResult UnMarshal ()
		{
			Result = new GetObjectResult ();

			if (Message.StatusCode.Equals (HttpStatusCode.OK)) {
			
				Result.Stream = (MemoryStream)Message.Content.ReadAsStreamAsync ().Result;

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

