using System;
using System.Data;
using CustomerLib.Data.Extensions;
using Moq;
using Xunit;

namespace CustomerLib.Data.Tests.Extensions
{
	public class DataRecordExtensionsTest
	{
		private class GetValueOrDefaultWhenDBNullData : TheoryData<object, string>
		{
			public GetValueOrDefaultWhenDBNullData()
			{
				Add("text", "text");
				Add(DBNull.Value, null);
			}
		}
		[Theory]
		[ClassData(typeof(GetValueOrDefaultWhenDBNullData))]
		public void ShouldGetValueOrDefaultWhenDBNull(object value, string expected)
		{
			var columnName = "whatever";
			var record = new Mock<IDataRecord>();
			record.Setup(dataRecord => dataRecord[columnName]).Returns(value);

			var actual = record.Object.GetValueOrDefault<string>(columnName);

			Assert.Equal(expected, actual);
		}
	}
}
