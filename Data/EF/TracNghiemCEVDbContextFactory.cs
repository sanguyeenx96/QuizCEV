using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public class TracNghiemCEVDbContextFactory : IDesignTimeDbContextFactory<TracNghiemCEVDbContext>
    {
        public TracNghiemCEVDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json")
                           .Build();

            var connectionString = configuration.GetConnectionString("tracnghiemDb");

            var optionsBuilder = new DbContextOptionsBuilder<TracNghiemCEVDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TracNghiemCEVDbContext(optionsBuilder.Options);
        }
    }
}
