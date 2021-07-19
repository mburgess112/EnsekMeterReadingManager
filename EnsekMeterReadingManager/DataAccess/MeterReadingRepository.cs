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

        public bool SaveMeterReading(MeterReading meterReading)
        {
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
    }
}
