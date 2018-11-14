using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RiverLinkReporter.service.Data;

namespace RiverLinkReporter.service
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=RiverLinkReporterDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            var returnValue = new ApplicationDbContext(builder.Options);

            return returnValue;
        }
    }
}