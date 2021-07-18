using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using EnsekMeterReadingManager.Models;

using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EnsekMeterReadingManager.Csv
{
    public class CsvMeterReadingExtractor
    {
        public IEnumerable<MeterReadingDto> ExtractReadings(Stream csvFileStream)
        {
            using var streamReader = new StreamReader(csvFileStream);
            using var csv = new CsvReader(streamReader, CsvConfiguration);

            var list = new List<MeterReadingDto>();
            // TODO: investigate possible performance issue when looping over large CSV files
            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<MeterReadingDto>();
                    list.Add(record);
                }
                catch (HeaderValidationException)
                {

                }
                catch (MissingFieldException)
                {

                }
                catch (TypeConverterException)
                {

                }
            }
            return list;
        }

        private readonly CsvConfiguration CsvConfiguration =
            new(CultureInfo.InvariantCulture)
            {
            };
    }
}
