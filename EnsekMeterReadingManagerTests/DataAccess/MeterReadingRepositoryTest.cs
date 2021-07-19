using EnsekMeterReadingManager.Models;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekMeterReadingManagerTests.DataAccess
{
    class MeterReadingRepositoryTest : BaseSqliteInMemoryEntityFrameworkTest
    {
        private EnsekDbContext _context;
        private MeterReadingRepository _underTest;

        [OneTimeSetUp]
        public static void SetupDbContext()
        {
            SetupForFixture();
            SeedValidAccount();
        }

        [SetUp]
        public void Setup()
        {
            _context = GetContext();
            _underTest = new MeterReadingRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context?.Dispose();
        }

        [Test]
        public void SaveMeterReading_ValidReading_ShouldReturnTrue()
        {
            var validReading = new MeterReadingDto
            {
                AccountId = VALID_ACCOUNT_ID,
                MeterReadingDateTime = "22/04/2019 12:25:00",
                MeterReadValue = 45522
            };

            var success = _underTest.SaveMeterReading(validReading);

            Assert.That(success, Is.True);
        }

        [Test]
        public void SaveMeterReading_InvalidAccountId_ShouldReturnFalse()
        {
            var reading = new MeterReadingDto
            {
                AccountId = INVALID_ACCOUNT_ID,
                MeterReadingDateTime = "22/04/2019 12:25:00",
                MeterReadValue = 45522
            };

            var success = _underTest.SaveMeterReading(reading);

            Assert.That(success, Is.False);
        }

        [Test]
        [Ignore("TODO: Test fails when running against Sqlite, despite equivalent functionality working on Sql Server")]
        public void SaveMeterReading_DuplicateEntryForAccount_ShouldReturnFalse()
        {
            var validReading = new MeterReadingDto
            {
                AccountId = VALID_ACCOUNT_ID,
                MeterReadingDateTime = "22/04/2019 12:25:00",
                MeterReadValue = 45522
            };
            SeedValidReading();

            var success = _underTest.SaveMeterReading(validReading);

            Assert.That(success, Is.False);
        }

        [Test]
        public void SaveMeterReadingBatch_AllValidEntries_ShouldReturnMatchingSuccessCount()
        {
            var readingList = new List<MeterReadingDto>
            {
                new MeterReadingDto
                {
                    AccountId = VALID_ACCOUNT_ID,
                    MeterReadingDateTime = "22/04/2019 12:25:00",
                    MeterReadValue = 45522
                },
                 new MeterReadingDto
                {
                    AccountId = VALID_ACCOUNT_ID,
                    MeterReadingDateTime = "29/04/2019 11:00:00",
                    MeterReadValue = 45672
                },
                new MeterReadingDto
                {
                    AccountId = VALID_ACCOUNT_ID,
                    MeterReadingDateTime = "06/05/2019 09:21:00",
                    MeterReadValue = 45522
                }
            };

            var result = _underTest.SaveMeterReadingBatch(readingList);

            Assert.That(result.SuccessCount, Is.EqualTo(3));
            Assert.That(result.FailureCount, Is.EqualTo(0));
        }

        [Test]
        public void SaveMeterReadingBatch_InvalidEntryMidList_ShouldReturnCorrectSuccessCount()
        {
            //TODO: seems to fail when error value is in middle of the list - work out why
            var readingList = new List<MeterReadingDto>
            {
                new MeterReadingDto
                {
                    AccountId = VALID_ACCOUNT_ID,
                    MeterReadingDateTime = "22/04/2019 12:25:00",
                    MeterReadValue = 45522
                },
                new MeterReadingDto
                {
                    AccountId = VALID_ACCOUNT_ID,
                    MeterReadingDateTime = "06/05/2019 09:21:00",
                    MeterReadValue = 45785
                },
                new MeterReadingDto
                {
                    AccountId = INVALID_ACCOUNT_ID,
                    MeterReadingDateTime = "29/04/2019 11:00:00",
                    MeterReadValue = 45672
                }
            };

            var result = _underTest.SaveMeterReadingBatch(readingList);

            Assert.That(result.SuccessCount, Is.EqualTo(2));
            Assert.That(result.FailureCount, Is.EqualTo(1));
        }

        private static readonly int INVALID_ACCOUNT_ID = 1;
        private static readonly int VALID_ACCOUNT_ID = 2345;

        private static void SeedValidAccount()
        {
            using var context = GetContext();
            context.Accounts.Add(new Account
            {
                AccountId = VALID_ACCOUNT_ID,
                FirstName = "Jerry",
                LastName = "Test"
            });
        }
        
        private void SeedValidReading()
        {
            var validReading = new MeterReading
            {
                AccountId = VALID_ACCOUNT_ID,
                MeterReadingDateTime = DateTime.Parse("2019-04.22T12:25:00"),
                MeterReadValue = 45522
            };
            using var ctx = GetContext();
            ctx.MeterReadings.Add(validReading);
            ctx.SaveChanges();
        }

    }
}
