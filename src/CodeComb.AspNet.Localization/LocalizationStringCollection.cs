using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace CodeComb.AspNet.Localization
{
    public abstract class LocalizationStringCollection : ILocalizationStringCollection
    {
        public virtual IRequestCultureProvider CultureProvider { get; set; }

        public HttpContext HttpContext
        {
            get { return CultureProvider.HttpContext; }
        }

        public LocalizationStringCollection(IRequestCultureProvider cultureProvider)
        {
            CultureProvider = cultureProvider;
        }

        public string this[string identifier, params object[] objects]
        {
            get
            {
                return this.GetString(SingleCulture(CultureProvider.DetermineRequestCulture()), identifier, objects);
            }
        }

        public abstract IList<CultureInfo> Collection { get; }

        public abstract void Refresh();

        public abstract void RemoveString(string Identifier);

        public virtual string SingleCulture(string[] cultures)
        {
            foreach(var x in cultures)
            {
                if (Collection.Any(y => y.Cultures.Contains(x)))
                {
                    var result = Collection.Where(y => y.Cultures.Contains(x)).First().Cultures.First();
                    return result;
                }
            }
            return Collection.Where(y => y.IsDefault).First().Cultures.First();
        }

        public virtual string GetString(string culture, string identifier, params object[] objects)
        {
            var cultureInfo = Collection.Where(x => x.Cultures.Contains(culture)).FirstOrDefault();
            if (cultureInfo == null)
                cultureInfo = Collection.Where(x => x.IsDefault).FirstOrDefault();
            if (cultureInfo == null)
                throw new FileNotFoundException();
            return string.Format(cultureInfo.LocalizedStrings[identifier], objects);
        }

        public virtual void SetString(string culture, string identifier, string Content)
        {
            var obj = Collection.Where(x => x.Cultures.Contains(culture)).FirstOrDefault();
            if (obj == null)
                throw new FileNotFoundException();
            obj.LocalizedStrings[identifier] = Content;
        }
    }
}
