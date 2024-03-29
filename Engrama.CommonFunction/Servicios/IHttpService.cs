﻿using System.Threading.Tasks;

namespace CommonFuncion.Servicios
{
	public interface IHttpService
	{
		Task<HttpResponseWrapper<T>> Get<T>(string url);
		
		Task<HttpResponseWrapper<object>> Post<T>(string url, T data);
		
		Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data);

		Task<HttpResponseWrapper<TResponse>> Post<TResponse>(string url);
	}
}