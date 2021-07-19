using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnsekMeterReadingManager.Models
{
    public class BatchConversionResult<T>
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public int FailureCount { get; set; }
        public List<T> Results { get; set; } = new List<T>();
    }
}
