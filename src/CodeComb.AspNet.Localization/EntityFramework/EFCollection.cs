using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using CodeComb.AspNet.Localization.EntityFramework;

namespace CodeComb.AspNet.Localization.EntityFramework
{
    public class EFCollection<TKey> : LocalizationStringCollection
        where TKey : IEquatable<TKey>
    {
        private List<CultureInfo> _Collection { get; set; }

        private ILocalizationDbContext<TKey> _DbContext { get; set; }

        public EFCollection(ILocalizationDbContext<TKey> DbContext, IRequestCultureProvider cultureProvider) : base(cultureProvider)
        {
            _Collection = new List<CultureInfo>();
            _DbContext = DbContext;
            Refresh();
        }

        public override IList<CultureInfo> Collection
        {
            get
            {
                return _Collection;
            }
        }

        private Dictionary<string,string> ConvertToDictionary(ICollection<LocalizedString<TKey>> src)
        {
            var ret = new Dictionary<string, string>();
            foreach (var x in src)
                ret.Add(x.Key, x.Value);
            return ret;
        }

        public override void Refresh()
        {
            _Collection.Clear();

            var info = _DbContext.LocalizationCultureInfo
                .Include(x => x._Cultures)
                .Include(x => x._Strings)
                .ToList();

            foreach(var x in info)
            {
                _Collection.Add(new CultureInfo
                {
                    Cultures = x._Cultures.Select(y => y.Culture).ToList(),
                    IsDefault = x.IsDefault,
                    Set = x.Set,
                    LocalizedStrings = ConvertToDictionary(x._Strings)
                });
            }
        }

        public override void RemoveString(string Identifier)
        {
            var src = _DbContext.LocalizationString
                .Where(x => x.Key == Identifier)
                .ToList();

            foreach(var x in src)
            {
                _DbContext.LocalizationString.Remove(x);
            }
            _DbContext.SaveChanges();
        }

        public override void SetString(string culture, string identifier, string Content)
        {
            var obj = _Collection.Where(x => x.Cultures.Contains(culture)).FirstOrDefault();
            if (obj == null)
                throw new KeyNotFoundException();
            obj.LocalizedStrings[identifier] = Content;
            var id = obj.Identifier;
            var str = _DbContext.LocalizationString
                .Where(x => x.CultureInfoId.Equals(id) && x.Key == identifier)
                .ToList();
            foreach (var x in str)
            {
                x.Value = Content;
            }
            _DbContext.SaveChanges();
        }
    }
}
