namespace CommonFuncion.Results
{
	public class Response<T>
	{
		public T Data { get; set; }

		public bool IsSuccess { get; set; }

		public string Message { get; set; } = string.Empty;

		public static Response<T> BadResult(string Message, T data)
		{
			Response<T> result = new Response<T>
			{
				IsSuccess = false,
				Message = Message,
				Data = data
			};

			return result;
		}
	}
}