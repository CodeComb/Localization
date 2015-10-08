using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace CodeComb.AspNet.Localization.EntityFramework
{
    public interface ILocalizationDbContext<TKey>
        where TKey : IEquatable<TKey>
    {
        DbSet<CultureInfo<TKey>> LocalizationCultureInfo { get; set; }
        DbSet<Culture<TKey>> LocalizationCulture { get; set; }
        DbSet<LocalizedString<TKey>> LocalizationString { get; set; }
        int SaveChanges();
        DatabaseFacade Database { get; }
    }
}
