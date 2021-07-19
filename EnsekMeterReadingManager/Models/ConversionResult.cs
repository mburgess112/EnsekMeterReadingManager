using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnsekMeterReadingManager.Models
{
    public class ConversionResult<T>
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public bool IsSuccess
        {
            get
            {
                return ErrorMessages?.Count == 0;
            }
        }

        public T Result { get; set; }
    }
}
