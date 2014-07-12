using System;
using S3Storage.Response;
using System.Xml.Serialization;
using System.Net.Http;
using System.Xml.Linq;
using System.Net;
using S3Storage.AWSException;

namespace S3Storage.Marshalling
{
	public class BaseUnMarshaller<T> : IUnMarshaller<T> where T: BaseResult
	{
		private const string NameSpace = "http://s3.amazonaws.com/doc/2006-03-01/";

		protected XmlSerializer Serializer { get; set; }

		protected HttpResponseMessage Message { get; set; }

		protected T Result { get; set; }

		public void Configure (HttpResponseMessage message)
		{
			this.Message = message;
		}

		public T UnMarshal ()
		{
			XDocument doc = XDocument.Load (Message.Content.ReadAsStreamAsync ().Result);

			if (Message.StatusCode.Equals (HttpStatusCode.OK)) {
				Serializer = new XmlSerializer (typeof(T), NameSpace);
				Result = (T)Serializer.Deserialize (doc.CreateReader ());
				Result.HttpStatusCode = Message.StatusCode;
				Result.ContentLength = Message.Content.Headers.ContentLength;
				return Result;
			} else {
				Serializer = new XmlSerializer (typeof(BaseResult));
				BaseResult result = (BaseResult)Serializer.Deserialize (doc.CreateReader ());
				Result.HttpStatusCode = Message.StatusCode;
				Result.ContentLength = Message.Content.Headers.ContentLength;
				throw new AWSErrorException (Result);
			}
		}
	}
}

