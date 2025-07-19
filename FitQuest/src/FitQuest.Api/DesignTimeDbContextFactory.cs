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
            var optionsBuilder = new DbContextOptionsBuilder<FitQuestContext>();
            optionsBuilder.UseSqlite("Data Source=FitQuestDb.sqlite;");
            return new FitQuestContext(optionsBuilder.Options);
        }
    }
}