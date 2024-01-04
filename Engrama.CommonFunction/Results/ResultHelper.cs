using CommonFuncion.Dapper.Interfaces;
using CommonFuncion.Extensions;

using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommonFuncion.Results
{
	public class ResultHelper : IResultHelper
	{
		private IMessageHandler MessageHandler { get; }

		public ResultHelper(
			IMessageHandler iMessageHandler
		)
		{
			MessageHandler = iMessageHandler;
		}

		public async Task<Result> ValidateAsync<T>(
			DataResult<T> dataResult, bool notifyEmptyResult = false, string emptyResultMessage = "",
			bool notifyResult = true, bool notifyError = true, [CallerFilePath] string filePath = null,
			[CallerMemberName] string method = null
		) where T : DbResult
		{
			string validateMessage = string.Empty;

			if (dataResult.Ok)
			{
				if (dataResult.Data.NotNull())
				{
					Result validateErrorResult = ValidateError(dataResult.Data);
					
					validateMessage = validateErrorResult.Msg;

					if (validateErrorResult.Ok)
					{
						return Result.Success();
					}

					if (notifyResult && validateMessage.NotEmpty()) await MessageHandler.WarningAsync("ALERTA", validateMessage);
				}

				if (notifyEmptyResult)
				{
					validateMessage = emptyResultMessage;

					await MessageHandler.WarningAsync("ALERTA", validateMessage);
				}
			}
			else if (notifyError)
			{
				await MessageHandler.ErrorAsync("ERROR", dataResult.Error);
			}

			if (validateMessage.NotEmpty())
			{
				string callerType = Path.GetFileNameWithoutExtension(filePath);

				Log.Information(string.Empty);
				Log.Information($"{callerType}.{method}() => {validateMessage}");
				Log.Information(string.Empty);
			}

			return Result.Fail(validateMessage);
		}

		public async Task<Result> ValidateAsync<T>(
			DataResult<IList<T>> dataResult, bool notifyEmptyResult = false, string emptyResultMessage = "",
			bool notifyResult = true, bool notifyError = true, [CallerFilePath] string filePath = null,
			[CallerMemberName] string method = null
		) where T : DbResult
		{
			string validateMessage = string.Empty;

			if (dataResult.Ok)
			{
				if (dataResult.Data.NotEmpty())
				{
					Result validateErrorsResult = ValidateErrors(dataResult.Data);

					validateMessage = validateErrorsResult.Msg;

					if (validateErrorsResult.Ok)
					{
						return Result.Success();
					}

					if (notifyResult && validateMessage.NotEmpty())
					{
						await MessageHandler.WarningAsync("ALERTA", validateMessage);
					}
				}

				if (notifyEmptyResult)
				{
					validateMessage = emptyResultMessage;

					await MessageHandler.WarningAsync("ALERTA", validateMessage);
				}
			}
			else if (notifyError)
			{
				await MessageHandler.ErrorAsync("ERROR", dataResult.Error);
			}

			if (validateMessage.NotEmpty())
			{
				string callerType = Path.GetFileNameWithoutExtension(filePath);

				Log.Information(string.Empty);
				Log.Information($"{callerType}.{method}() => {validateMessage}");
				Log.Information(string.Empty);
			}

			return Result.Fail(validateMessage);
		}

		private static Result ValidateErrors<T>(IEnumerable<T> data) where T : DbResult
		{
			foreach (T item in data)
			{
				Result validateErrorResult = ValidateError(item);

				if (validateErrorResult.Ok.False())
				{
					return Result.Fail(validateErrorResult.Msg);
				}
			}

			return Result.Success();
		}

		private static Result ValidateError<T>(T obj) where T : DbResult
		{
			if (obj.bResultado.False())
			{
				return Result.Fail(obj.vchMensaje);
			}

			return Result.Success();
		}
	}
}