using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using CodeComb.AspNet.Localization.EntityFramework;

namespace CodeComb.AspNet.Localization.Tests
{
    public class LocalizationContext : LocalizationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase();
        }
    }
}
