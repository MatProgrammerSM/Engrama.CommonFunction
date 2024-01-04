using AutoMapper;

using CommonFuncion.Dapper.Interfaces;
using CommonFuncion.Extensions;
using System.Collections.Generic;

namespace CommonFuncion.Results
{
	public class ResponseHelper : IResponseHelper
	{
		private readonly IMapper _mapper;

		public ResponseHelper(IMapper mapper)
		{
			_mapper = mapper;
		}

		public Response<IEnumerable<TResultado>> Validacion<TProducto, TResultado>(
			IEnumerable<TProducto> ModeloValidar
		) where TProducto : DbResult
		{
			Response<IEnumerable<TResultado>> response = new Response<IEnumerable<TResultado>>
			{
				Data = new List<TResultado>()
			};

			if (ModeloValidar.IsEmpty().False())
			{
				Result validacion = ValidateErrors(ModeloValidar);

				if (validacion.Ok)
				{
					response.IsSuccess = true;
					response.Message = string.Empty;
					response.Data = _mapper.Map<IEnumerable<TResultado>>(ModeloValidar);
				}
				else
				{
					response.IsSuccess = false;
					response.Message = validacion.Msg;
				}
			}

			return response;
		}

		public Response<TResultado> Validacion<TProducto, TResultado>(
			TProducto ModeloValidar
		) where TProducto : DbResult where TResultado : new()
		{
			Response<TResultado> response = new Response<TResultado>
			{
				Data = new TResultado()
			};

			if (ModeloValidar.IsNull().False())
			{
				Result validacion = ValidateError(ModeloValidar);

				if (validacion.Ok)
				{
					response.IsSuccess = true;
					response.Message = string.Empty;
					response.Data = _mapper.Map<TResultado>(ModeloValidar);
				}
				else
				{
					response.IsSuccess = false;
					response.Message = validacion.Msg;
				}
			}

			return response;
		}

		public Response<TResultado> ValidacionSinMapper<TProducto, TResultado>(
			TProducto ModeloValidar
		) where TProducto : DbResult where TResultado : new()
		{
			Response<TResultado> response = new Response<TResultado>
			{
				Data = new TResultado()
			};

			if (ModeloValidar.IsNull().False())
			{
				Result validacion = ValidateError(ModeloValidar);

				if (validacion.Ok)
				{
					response.IsSuccess = true;
					response.Message = string.Empty;
				}
				else
				{
					response.IsSuccess = false;
					response.Message = validacion.Msg;
				}
			}

			return response;
		}

		public Response<IEnumerable<TResultado>> ValidacionSinMapper<TProducto, TResultado>(
			IEnumerable<TProducto> ModeloValidar
		) where TProducto : DbResult where TResultado : new()
		{
			Response<IEnumerable<TResultado>> response = new Response<IEnumerable<TResultado>>
			{
				Data = new List<TResultado>()
			};

			if (ModeloValidar.IsNull().False())
			{
				Result validacion = ValidateErrors(ModeloValidar);

				if (validacion.Ok)
				{
					response.IsSuccess = true;
					response.Message = string.Empty;
				}
				else
				{
					response.IsSuccess = false;
					response.Message = validacion.Msg;
				}
			}

			return response;
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