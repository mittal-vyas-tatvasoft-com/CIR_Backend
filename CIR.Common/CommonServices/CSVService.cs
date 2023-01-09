using CsvHelper;
using System.Globalization;

namespace CIR.Common.CommonServices
{
	public class CSVService
	{
		public IEnumerable<T> ReadCSV<T>(Stream file)
		{
			var reader = new StreamReader(file);
			var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

			var records = csv.GetRecords<T>();
			return records;
		}
	}
}
