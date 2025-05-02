using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using FitQuest.Api; 

namespace FitQuest.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FitQuestContext>
    {
        public FitQuestContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FitQuestContext>();
            var connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(
                connectionString,
                sqlOpts => sqlOpts.EnableRetryOnFailure());

            return new FitQuestContext(optionsBuilder.Options);
        }
    }
}