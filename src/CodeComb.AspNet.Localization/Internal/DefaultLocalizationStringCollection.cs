using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeComb.AspNet.Localization;

namespace CodeComb.AspNet.Localization.Internal
{
    public class DefaultLocalizationStringCollection : LocalizationStringCollection
    {
        private IList<CultureInfo> _collection;

        public DefaultLocalizationStringCollection(IList<CultureInfo> collection, IRequestCultureProvider cultureProvider) : base(cultureProvider)
        {
            _collection = collection;
        }

        public override IList<CultureInfo> Collection
        {
            get
            {
                return _collection;
            }
        }

        public override void Refresh()
        {
            return;
        }

        public override void RemoveString(string Identifier)
        {
            throw new NotImplementedException();
        }
    }
}
