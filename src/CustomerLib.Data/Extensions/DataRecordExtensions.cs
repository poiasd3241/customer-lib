using System;
using System.Data;

namespace CustomerLib.Data.Extensions
{
	public static class DataRecordExtensions
	{
		public static T GetValueOrDefault<T>(this IDataRecord dataRecord, string columnName)
		{
			var value = dataRecord[columnName];

			if (value == DBNull.Value)
			{
				return default;
			}

			return (T)value;
		}
	}
}
