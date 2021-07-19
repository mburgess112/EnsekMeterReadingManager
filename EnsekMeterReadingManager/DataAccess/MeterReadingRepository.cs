using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EnsekMeterReadingManager.Models
{
    public class MeterReadingRepository
    {
        private readonly EnsekDbContext _ensekDbContext;

        public MeterReadingRepository(EnsekDbContext ensekDbContext)
        {
            this._ensekDbContext = ensekDbContext;
        }

        public bool SaveMeterReading(MeterReadingDto meterReadingDto)
        {
            var meterReading = new MeterReading()
            {
                AccountId = meterReadingDto.AccountId,
                MeterReadingDateTime = DateTime.ParseExact(meterReadingDto.MeterReadingDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                MeterReadValue = meterReadingDto.MeterReadValue
            };

            _ensekDbContext.MeterReadings.Add(meterReading);

            try
            {
                _ensekDbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public BatchResult SaveMeterReadingBatch(IEnumerable<MeterReadingDto> meterReadings)
        {
            var batchResult = new BatchResult();
            foreach (MeterReadingDto reading in meterReadings)
            {
                var success = SaveMeterReading(reading);
                if (success)
                {
                    batchResult.SuccessCount++;
                }
                else
                {
                    batchResult.FailureCount++;
                }
            }
            return batchResult;
        }
    }
}
