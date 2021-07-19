
using EnsekMeterReadingManager.Models;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

namespace EnsekMeterReadingManagerTests.DataAccess
{
    abstract class BaseSqliteInMemoryEntityFrameworkTest
    {
        protected static readonly DbContextOptions<EnsekDbContext> _options = 
            new DbContextOptionsBuilder<EnsekDbContext>()
                    .UseSqlite("Filename=Test.db")
                    .Options;

        
        public static void SetupForFixture()
        {
            using var ctx = new EnsekDbContext(_options);
            ctx.Database.EnsureCreated();
        }


        protected static EnsekDbContext GetContext()
        {
            return new EnsekDbContext(_options);
        }
    }
}
