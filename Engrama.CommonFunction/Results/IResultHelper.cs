﻿using CommonFuncion.Dapper.Interfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommonFuncion.Results
{
	public interface IResultHelper
	{
		Task<Result> ValidateAsync<T>(
			DataResult<T> dataResult, bool notifyEmptyResult = false, string emptyResultMessage = "",
			bool notifyResult = true, bool notifyError = true, [CallerFilePath] string filePath = null,
			[CallerMemberName] string method = null
		) where T : DbResult;
		
		Task<Result> ValidateAsync<T>(
			DataResult<IList<T>> dataResult, bool notifyEmptyResult = false, string emptyResultMessage = "",
			bool notifyResult = true, bool notifyError = true, [CallerFilePath] string filePath = null,
			[CallerMemberName] string method = null
		) where T : DbResult;
	}
}