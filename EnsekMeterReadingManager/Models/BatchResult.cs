using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnsekMeterReadingManager.Models
{
    public class BatchResult
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }
}
