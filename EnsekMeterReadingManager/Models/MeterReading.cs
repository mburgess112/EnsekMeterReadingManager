using Microsoft.EntityFrameworkCore;

using System;

namespace EnsekMeterReadingManager.Models
{
    [Index(nameof(AccountId), nameof(MeterReadingDateTime), IsUnique = true)]
    public class MeterReading
    {
        public int MeterReadingId { get; set; }
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }

        public Account Account { get; set; }
    }
}
