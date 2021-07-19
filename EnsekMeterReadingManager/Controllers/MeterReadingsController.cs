using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnsekMeterReadingManager.Models;
using EnsekMeterReadingManager.Csv;

namespace EnsekMeterReadingManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingsController : ControllerBase
    {
        private readonly EnsekDbContext _context;
        private readonly CsvMeterReadingExtractor _csvMeterReadingExtractor;
        private readonly MeterReadingDtoConverter _meterReadingDtoConverter;
        private readonly MeterReadingRepository _meterReadingRepository;

        public MeterReadingsController(
            EnsekDbContext context,
            CsvMeterReadingExtractor csvMeterReadingExtractor,
            MeterReadingDtoConverter meterReadingDtoConverter,
            MeterReadingRepository meterReadingRepository)
        {
            _context = context;
            _csvMeterReadingExtractor = csvMeterReadingExtractor;
            _meterReadingDtoConverter = meterReadingDtoConverter;
            _meterReadingRepository = meterReadingRepository;
        }

        // GET: api/MeterReadings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReading>>> GetMeterReadings()
        {
            return await _context.MeterReadings.ToListAsync();
        }

        // GET: api/MeterReadings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterReading>> GetMeterReading(int id)
        {
            var meterReading = await _context.MeterReadings.FindAsync(id);

            if (meterReading == null)
            {
                return NotFound();
            }

            return meterReading;
        }

        // PUT: api/MeterReadings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeterReading(int id, MeterReading meterReading)
        {
            if (id != meterReading.MeterReadingId)
            {
                return BadRequest();
            }

            _context.Entry(meterReading).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeterReadingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MeterReadings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MeterReading>> PostMeterReading(MeterReading meterReading)
        {
            _context.MeterReadings.Add(meterReading);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeterReading", new { id = meterReading.MeterReadingId }, meterReading);
        }

        // DELETE: api/MeterReadings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeterReading(int id)
        {
            var meterReading = await _context.MeterReadings.FindAsync(id);
            if (meterReading == null)
            {
                return NotFound();
            }

            _context.MeterReadings.Remove(meterReading);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("meter-reading-uploads")]
        public ActionResult<BatchResult> UploadMeterReadingCsv(IFormFile meterReadingCsv)
        {
            // TODO: consider if more detailed file validation is useful before attempting to read
            if (meterReadingCsv == null)
            {
                return BadRequest("A valid CSV file must be provided");
            }

            var result = new BatchResult();
            var extractedReadings = _csvMeterReadingExtractor.ExtractReadings(meterReadingCsv.OpenReadStream());
            result.FailureCount += extractedReadings.FailureCount;

            // TODO investigate possible performance issues around saving each row individually
            // Moderate files: some sort of bulk upload to save on database connections
            // Very large files: consider saving the CSV somewhere then processing asynchronously
            foreach (var readingDto in extractedReadings.Results)
            {
                var conversionResult = _meterReadingDtoConverter.Convert(readingDto);
                if (conversionResult.IsSuccess)
                {
                    var isSaved = _meterReadingRepository.SaveMeterReading(conversionResult.Result);
                    if (isSaved)
                    {
                        result.SuccessCount++;
                        continue;
                    }
                }
                result.FailureCount++;
            }

            return result;
        }

        private bool MeterReadingExists(int id)
        {
            return _context.MeterReadings.Any(e => e.MeterReadingId == id);
        }
    }
}
