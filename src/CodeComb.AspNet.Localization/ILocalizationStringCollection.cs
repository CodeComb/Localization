using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeComb.AspNet.Localization
{
    public interface ILocalizationStringCollection
    {
        IList<CultureInfo> Collection { get; }
        string this[string identifier, params object[] objects] { get; }
        string this[string culture, string identifier, params object[] objects] { get; set; }
        void Refresh();
        void RemoveString(string Identifier);
        string SingleCulture(string[] cultures);
    }
}
