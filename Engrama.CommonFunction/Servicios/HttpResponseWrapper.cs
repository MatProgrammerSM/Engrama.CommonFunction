﻿using System.Net.Http;
using System.Threading.Tasks;

namespace CommonFuncion.Servicios
{
	public class HttpResponseWrapper<T>
	{
		public bool Success { get; set; }

		public T Response { get; set; }

		public HttpResponseMessage HttpResponseMessage { get; set; }

		public HttpResponseWrapper(T response, bool success, HttpResponseMessage httpResponseMessage)
		{
			Success = success;
			Response = response;
			HttpResponseMessage = httpResponseMessage;
		}

		public async Task<string> GetBody()
		{
			var respuesta = await HttpResponseMessage.Content.ReadAsStringAsync();
			return respuesta;
		}
	}
}