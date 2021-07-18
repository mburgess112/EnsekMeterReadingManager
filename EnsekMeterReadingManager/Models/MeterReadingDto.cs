using System;

namespace EnsekMeterReadingManager.Models
{
    public class MeterReadingDto
    {
        public int AccountId { get; set; }
        //TODO: look at setting up proper converters for ASP.Net and CSV libraries
        public string MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MeterReadingDto dto &&
                   AccountId == dto.AccountId &&
                   MeterReadingDateTime == dto.MeterReadingDateTime &&
                   MeterReadValue == dto.MeterReadValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountId, MeterReadingDateTime, MeterReadValue);
        }

        public override string ToString()
        {
            return $"{{{nameof(AccountId)}={AccountId.ToString()}, " +
                $"{nameof(MeterReadingDateTime)}={MeterReadingDateTime}, " +
                $"{nameof(MeterReadValue)}={MeterReadValue.ToString()}}}";
        }
    }
}
