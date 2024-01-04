﻿using CommonFuncion.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonFuncion.Extensions
{
	public static class ListExt
	{
		public static bool AddIf<T>(
			this IList<T> lst, T data, Func<T, bool> check
		)
		{
			if (lst.All(check).False()) return false;

			lst.Add(data);

			return true;
		}

		/// <summary>
		/// Extencion para validar las listas de datos de interfaz DbResult si el bResltado es true
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="lst"></param>
		/// <returns></returns>
		public static (bool, string) ValidaResult<T>(
			this IEnumerable<T> lst
		) where T : class, DbResult, new()
		{
			if (lst.Any(e => e.bResultado.False()))
			{
				T itemError = lst.FirstOrDefault(e => e.bResultado.False());
				
				return (itemError.bResultado, itemError.vchMensaje);
			}

			return (true, string.Empty);
		}
	}
}