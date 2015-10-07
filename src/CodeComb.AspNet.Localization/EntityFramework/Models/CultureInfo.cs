using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeComb.AspNet.Localization.EntityFramework
{
    [Table("AspNetLocalizationCultureInformations")]
    public class CultureInfo<TKey> : CultureInfo
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }

        public virtual ICollection<Cultures<TKey>> _Cultures { get; set; } = new List<Cultures<TKey>>();

        public virtual ICollection<LocalizedString<TKey>> _Strings { get; set; } = new List<LocalizedString<TKey>>();
    }
}
