using System;
using System.Linq;

namespace CommonFuncion.Extensions
{
	public static class DatetimeExt
	{
		public static bool IsAny(
			this DateTime value, params DateTime[] values
		)
		{
			return values.Count(val => val == value) > 0;
		}

		public static string ToFormatMX(
			this DateTime value, bool bWithSlash = true
		)
		{
			if (bWithSlash)
			{
				return Convert.ToDateTime(value.ToString()).ToString("dd/MM/yyyy");
			}
			else
			{
				return Convert.ToDateTime(value.ToString()).ToString("dd-MM-yyyy");
			}
		}

		public static string ToFormatMXComplete(
			this DateTime value, bool bWithSlash = true
		)
		{
			if (bWithSlash)
			{
				return Convert.ToDateTime(value.ToString()).ToString("dd/MM/yyyy hh:mm:ss tt");
			}

			return Convert.ToDateTime(value.ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
		}

		public static bool EqualsToDefaultDate(
			this DateTime datetime
		)
		{
			DateTime dDate = DateTime.Parse(datetime.ToShortDateString());
			DateTime dDefault = Defaults.SqlMinDate();

			int value = DateTime.Compare(dDefault, dDate);

			return value == 0;
		}

		public static bool EqualsToDefaultDateTime(
			this DateTime datetime, bool defaultFirstValue = false
		)
		{
			int value;

			if (defaultFirstValue)
			{
				value = DateTime.Compare(datetime, Defaults.SqlMinDate());
			}
			else
			{
				value = DateTime.Compare(Defaults.SqlMinDate(), datetime);
			}

			return value == 0;
		}

		public static string EqualsDefaultToFormatMXOrEmpty(
			this DateTime datetime
		)
		{
			int value = DateTime.Compare(Defaults.SqlMinDate(), datetime);

			return value == 0 ? string.Empty : datetime.ToFormatMX();
		}

		public static string ToNumber(
			this DateTime dateTime, bool time = true
		)
		{
			string sTime = time ? "hmmss" : "";

			return dateTime.ToString("ddMMyyyy" + sTime);
		}

		public static string ToFormatNull(
			this DateTime self, bool condition = false, string value = "N/A"
		)
		{
			if (condition)
			{
				return value;
			}

			return self.ToFormatMX();
		}
	}
}