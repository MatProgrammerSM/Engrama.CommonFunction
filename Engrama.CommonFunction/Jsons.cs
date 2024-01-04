using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CommonFuncion
{
	public sealed class Jsons
	{
		public static string Stringify(
			object obj, bool bRemoveNull = false, Formatting formatting = Formatting.None
		)
		{
			JsonSerializerSettings jsonSettings = new JsonSerializerSettings();

			if (bRemoveNull)
			{
				jsonSettings.NullValueHandling = NullValueHandling.Ignore;
			}

			return JsonConvert.SerializeObject(obj, formatting, jsonSettings);
		}

		public static T Parse<T>(
			string json
		)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static T ReadJsonPath<T>(
			string path
		)
		{
			using StreamReader streamReader = new StreamReader(path);

			return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
		}

		public static List<T> ReadListJsonPath<T>(string path)
		{
			try
			{
				using StreamReader streamReader = new StreamReader(path);

				return JsonConvert.DeserializeObject<List<T>>(streamReader.ReadToEnd());
			}
			catch (Exception)
			{
				return new List<T>();
			}
		}
	}
}