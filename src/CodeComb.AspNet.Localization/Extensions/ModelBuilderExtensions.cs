using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeComb.AspNet.Localization.EntityFramework;

namespace Microsoft.Data.Entity
{
    public static class ModelBuilderExtensions
    {
        public static void BuildLocalization<TKey>(this ModelBuilder self)
            where TKey : IEquatable<TKey>
        {
            self.Entity<CultureInfo<TKey>>(e =>
            {
                e.HasIndex(x => x.IsDefault);
                e.HasIndex(x => x.Set);
            });
        }
    }
}
