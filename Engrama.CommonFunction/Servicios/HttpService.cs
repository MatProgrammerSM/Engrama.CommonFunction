using CommonFuncion.Extensions;
using CommonFuncion.Logger;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommonFuncion.Servicios
{
	public class HttpService : IHttpService
	{
		private readonly HttpClient httpClient;
		private readonly ILoggerHelper loggerHelper;

		private JsonSerializerOptions defaultJsonSerializerOptions => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
		
		public HttpService(
			HttpClient httpClient, ILoggerHelper _LoggerHelper
		)
		{
			this.httpClient = httpClient;
			loggerHelper = _LoggerHelper;
		}

		public async Task<HttpResponseWrapper<T>> Get<T>(
			string url
		)
		{
			HttpResponseMessage responseHTTP = await httpClient.GetAsync(url);

			if (responseHTTP.IsSuccessStatusCode)
			{
				T response = await Deserialize<T>(responseHTTP, defaultJsonSerializerOptions);
				HttpResponseWrapper<T> resultado = new HttpResponseWrapper<T>(response, true, responseHTTP);
				
				return resultado;
			}
			else
			{
				HttpResponseWrapper<T> resultado = new HttpResponseWrapper<T>(default, false, responseHTTP);
				
				return resultado;
			}
		}

		public async Task<HttpResponseWrapper<object>> Post<T>(
			string url, T data
		)
		{
			string dataJson = JsonSerializer.Serialize(data);
			StringContent stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await httpClient.PostAsync(url, stringContent);

			HttpResponseWrapper<object> resultado = new HttpResponseWrapper<object>(
				null, response.IsSuccessStatusCode, response
			);
			
			return resultado;
		}

		public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(
			string url, T data
		)
		{
			loggerHelper.Info(url);

			string dataJson = JsonSerializer.Serialize(data);
			StringContent stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

			loggerHelper.Info(dataJson);

			HttpResponseMessage response = await httpClient.PostAsync(url, stringContent);
			
			if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
			{
				TResponse responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
				HttpResponseWrapper<TResponse> resultado = new HttpResponseWrapper<TResponse>(
					responseDeserialized, true, response
				);
				
				return resultado;
			}
			else
			{
				Log.Warning(url + response.StatusCode);

				if (response.IsNull())
				{
					response = new HttpResponseMessage();
				}

				return new HttpResponseWrapper<TResponse>(default, false, response);
			}
		}

		public async Task<HttpResponseWrapper<TResponse>> Post<TResponse>(
			string url
		)
		{
			StringContent stringContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

			loggerHelper.Info(stringContent.ToString());

			HttpResponseMessage response = await httpClient.PostAsync(url, stringContent);

			if (response.IsSuccessStatusCode)
			{
				TResponse responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
				HttpResponseWrapper<TResponse> resultado = new HttpResponseWrapper<TResponse>(
					responseDeserialized, true, response
				);

				return resultado;
			}
			else
			{
				if (response.IsNull())
				{
					response = new HttpResponseMessage();
				}
				
				return new HttpResponseWrapper<TResponse>(default, false, response);
			}
		}

		private async Task<T> Deserialize<T>(
			HttpResponseMessage httpResponse, JsonSerializerOptions options
		)
		{
			var responseString = await httpResponse.Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<T>(responseString, options);
		}
	}
}