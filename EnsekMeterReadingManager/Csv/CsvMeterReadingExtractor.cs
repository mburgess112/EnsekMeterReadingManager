using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using EnsekMeterReadingManager.Models;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EnsekMeterReadingManager.Csv
{
    public class CsvMeterReadingExtractor
    {
        public BatchConversionResult<MeterReadingDto> ExtractReadings(Stream csvFileStream)
        {
            using var streamReader = new StreamReader(csvFileStream);
            using var csv = new CsvReader(streamReader, CsvConfiguration);

            var list = new List<MeterReadingDto>();
            var batchResult = new BatchConversionResult<MeterReadingDto>();
            // TODO: investigate possible performance issue when looping over large CSV files
            while (csv.Read())
            {
                var result = GetMeterReadingDto(csv);
                if (result.IsSuccess)
                {
                    batchResult.Results.Add(result.Result);
                }
                else
                {
                    batchResult.FailureCount++;
                    batchResult.ErrorMessages.AddRange(result.ErrorMessages);
                }
            }
            return batchResult;
        }

        private ConversionResult<MeterReadingDto> GetMeterReadingDto(CsvReader csv)
        {
            var result = new ConversionResult<MeterReadingDto>();
            try
            {
                var record = csv.GetRecord<MeterReadingDto>();
                result.Result = record;
            }
            catch (CsvHelperException ex)
            {
                result.ErrorMessages.Add(ex.Message);
            }
            return result;
        }

        private readonly CsvConfiguration CsvConfiguration =
            new(CultureInfo.InvariantCulture)
            {
            };
    }
}
