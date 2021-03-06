﻿using System;
using System.Net.Http;
using S3Storage.Response;

namespace S3Storage.Marshalling
{
	public interface IUnMarshaller<T> where T:BaseResult
	{
		void Configure (HttpResponseMessage message);

		T UnMarshal ();
	}
}

