using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PrgHome.DataLayer
{
    class PrgHomeContextFactory : IDesignTimeDbContextFactory<PrgHomeContext>
    {
        public PrgHomeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PrgHomeContext>();
            optionsBuilder.UseSqlServer(@"Server=.;Database=PrgHomeDB;trusted_Connection=True");
            return new PrgHomeContext(optionsBuilder.Options);
        }
    }
}
