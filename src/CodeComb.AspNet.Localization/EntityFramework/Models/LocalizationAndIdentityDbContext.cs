using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CodeComb.AspNet.Localization.EntityFramework
{
    public class LocalizationAndIdentityDbContext<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>, ILocalizationDbContext<TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
    {
        public DbSet<CultureInfo<TKey>> LocalizationCultureInfo { get; set; }
        public DbSet<Culture<TKey>> LocalizationCulture { get; set; }
        public DbSet<LocalizedString<TKey>> LocalizationString { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CultureInfo<TKey>>(e =>
            {
                e.HasIndex(x => x.IsDefault);
                e.HasIndex(x => x.Set);
            });
        }
    }
}
