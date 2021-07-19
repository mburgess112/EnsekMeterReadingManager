using EnsekMeterReadingManager.Csv;
using EnsekMeterReadingManager.Models;

using NUnit.Framework;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnsekMeterReadingManagerTests.Csv
{
    class CsvMeterReadingExtractorTests
    {
        private CsvMeterReadingExtractor _underTest;

        [SetUp]
        public void Setup()
        {
            _underTest = new CsvMeterReadingExtractor();
        }

        [Test]
        public void ExtractReadings_EmptyStream_ShouldReturnEmptyList()
        {
            var returnedModels = _underTest.ExtractReadings(GetStringStream(""));

            Assert.That(returnedModels, Is.Empty);
        }

        [Test]
        public void ExtractReadings_SingleValidRow_ShouldReturnExpectedModel()
        {
            var singleRowCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                "2345,22/04/2019 12:25,45522\r\n";

            using var input = GetStringStream(singleRowCsv);
            var returnedModels = _underTest.ExtractReadings(input);

            var expectedModels = new List<MeterReadingDto>()
            {
                new MeterReadingDto()
                {
                    AccountId = 2345,
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = 45522
                }
            };
            Assert.That(returnedModels, Is.EqualTo(expectedModels));
        }

        [Test]
        public void ExtractReadings_MissingColumn_ShouldReturnEmptyList()
        {
            var missingColumnCsv = "AccountId,MeterReadingDateTime\r\n" +
                "2345,22/04/2019 12:25\r\n";

            using var input = GetStringStream(missingColumnCsv);
            var returnedModels = _underTest.ExtractReadings(input);

            Assert.That(returnedModels, Has.Count.EqualTo(0));
        }

        [Test]
        public void ExtractReadings_MissingValueInRow_ShouldSkipRow()
        {
            var missingValueInRowCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                "2345,22/04/2019 12:25,45522\r\n" +
                "2349,22/04/2019 12:25\r\n" +
                "2351,22/04/2019 12:25,57579\r\n";

            using var input = GetStringStream(missingValueInRowCsv);
            var returnedModels = _underTest.ExtractReadings(input);

            Assert.That(returnedModels, Has.Count.EqualTo(2));
        }

        [Test]
        public void ExtractReadings_InvalidDataInRow_ShouldSkipRow()
        {
            var invalidDataInRowCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                "2345,22/04/2019 12:25,45522\r\n" +
                "2349,22/04/2019 12:25,VOID\r\n" +
                "2351,22/04/2019 12:25,57579\r\n";

            using var input = GetStringStream(invalidDataInRowCsv);
            var returnedModels = _underTest.ExtractReadings(input);

            Assert.That(returnedModels, Has.Count.EqualTo(2));
        }

        private static Stream GetStringStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
    }
}
