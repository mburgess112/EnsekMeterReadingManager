using EnsekMeterReadingManager.Models;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekMeterReadingManagerTests.Models
{
    class MeterReadingDtoConverterTests
    {
        private MeterReadingDtoConverter _underTest;

        [SetUp]
        public void Setup()
        {
            _underTest = new MeterReadingDtoConverter();
        }

        [Test]
        public void Convert_ValidInputDto_ShouldReturnSuccess()
        {
            var input = GetValidDto();

            var result = _underTest.Convert(input);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.ErrorMessages, Has.Count.EqualTo(0));
        }

        [Test]
        public void Convert_ValidInputDate_ShouldIncludeInResult()
        {
            var input = GetValidDto();
            input.MeterReadingDateTime = "22/04/2019 12:25";
            var expectedResultDate = DateTime.Parse("2019-04-22T12:25");

            var result = _underTest.Convert(input);

            Assert.That(result.Result.MeterReadingDateTime, Is.EqualTo(expectedResultDate));
        }

        [Test, TestCaseSource(nameof(InvalidMeterReadingDateTimeSamples))]
        public void Convert_InvalidDate_ShouldNotReturnDateValue_ShouldIncludeErrorMessage(string invalidDateTime)
        {
            var input = GetValidDto();
            input.MeterReadingDateTime = invalidDateTime;

            var result = _underTest.Convert(input);

            Assert.That(result.Result.MeterReadingDateTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(result.ErrorMessages, Has.Count.EqualTo(1));
        }

        public static IEnumerable<TestCaseData> InvalidMeterReadingDateTimeSamples()
        {
            yield return new TestCaseData("2019-04-22T12:25");
            yield return new TestCaseData("22/04/2019 12:25:53");
            yield return new TestCaseData("22/04/2019 12:2");
            yield return new TestCaseData("22/04/19 12:25");
            yield return new TestCaseData("22/4/2019 12:25");
            yield return new TestCaseData("2/04/19 12:25");
        }

        [Test]
        public void Convert_ValidMeterReading_ShouldIncludeInResult()
        {
            var input = GetValidDto();
            input.MeterReadValue = 99999;

            var result = _underTest.Convert(input);

            Assert.That(result.Result.MeterReadValue, Is.EqualTo(input.MeterReadValue));
        }

        [Test, TestCaseSource(nameof(InvalidMeterReadingSamples))]
        public void Convert_InvalidMeterReading_ShouldNotReturnReadingValue_ShouldIncludeErrorMessage(int invalidMeterReading)
        {
            var input = GetValidDto();
            input.MeterReadValue = invalidMeterReading;

            var result = _underTest.Convert(input);

            Assert.That(result.Result.MeterReadValue, Is.EqualTo(0));
            Assert.That(result.ErrorMessages, Has.Count.EqualTo(1));
        }

        public static IEnumerable<TestCaseData> InvalidMeterReadingSamples()
        {
            yield return new TestCaseData(0);
            yield return new TestCaseData(-1);
            yield return new TestCaseData(1);
            yield return new TestCaseData(12);
            yield return new TestCaseData(123);
            yield return new TestCaseData(1234);
            yield return new TestCaseData(123456);
        }

        [Test]
        public void Convert_ValidAccountId_ShouldIncludeInResult()
        {
            var input = GetValidDto();
            input.AccountId = 1;

            var result = _underTest.Convert(input);

            Assert.That(result.Result.AccountId, Is.EqualTo(input.AccountId));
        }

        private static MeterReadingDto GetValidDto()
        {
            return new MeterReadingDto
            {
                MeterReadingDateTime = "22/04/2019 12:25",
                AccountId = 1,
                MeterReadValue = 99999
            };
        }
    }
}
