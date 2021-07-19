using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnsekMeterReadingManager.Models
{
    public class MeterReadingDtoConverter
    {
        public ConversionResult<MeterReading> Convert(MeterReadingDto meterReadingDto)
        {
            var conversionResult = new ConversionResult<MeterReading>();
            conversionResult.Result = new MeterReading();

            if (DateTime.TryParseExact(
                meterReadingDto.MeterReadingDateTime,
                "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime result))
            {
                conversionResult.Result.MeterReadingDateTime = result;
            }
            else
            {
                conversionResult.ErrorMessages.Add(
                    $"The value {meterReadingDto.MeterReadingDateTime} was not a recognised date-time");
            }

            if (Regex.IsMatch(meterReadingDto.MeterReadValue.ToString(), @"\A\d{5}\z", RegexOptions.None, TimeSpan.FromSeconds(10)))
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
    }
}
