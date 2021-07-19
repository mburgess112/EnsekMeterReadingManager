using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EnsekMeterReadingManager.Models
{
    public class MeterReadingDtoConverter
    {
        private static readonly string MeterReadingDateFormat = "dd/MM/yyyy HH:mm";
        private static readonly string MeterReadingNumericRegex = @"\A\d{5}\z";

        public ConversionResult<MeterReading> Convert(MeterReadingDto meterReadingDto)
        {
            var conversionResult = new ConversionResult<MeterReading>();
            conversionResult.Result = new MeterReading();

            if (TryParseMeterReadingDate(meterReadingDto.MeterReadingDateTime, out DateTime result))
            {
                conversionResult.Result.MeterReadingDateTime = result;
            }
            else
            {
                conversionResult.ErrorMessages.Add(
                    $"The value {meterReadingDto.MeterReadingDateTime} was not a recognised date-time");
            }

            if (MatchMeterReadingNumericRegex(meterReadingDto.MeterReadValue))
            {
                conversionResult.Result.MeterReadValue = meterReadingDto.MeterReadValue;
            }
            else
            {
                conversionResult.ErrorMessages.Add(
                    $"The meter reading value {meterReadingDto.MeterReadValue} has an incorrect number of digits");
            }

            conversionResult.Result.AccountId = meterReadingDto.AccountId;
            return conversionResult;
        }

        private static bool TryParseMeterReadingDate(string meterReadingDateTime, out DateTime result)
        {
            return DateTime.TryParseExact(
                meterReadingDateTime,
                MeterReadingDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result);
        }

        private static bool MatchMeterReadingNumericRegex(int meterReadValue)
        {
            return Regex.IsMatch(
                meterReadValue.ToString(),
                MeterReadingNumericRegex,
                RegexOptions.None,
                TimeSpan.FromSeconds(10));
        }
    }
}
