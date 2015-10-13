using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace CodeComb.AspNet.Localization
{
    public interface IRequestCultureProvider
    {
        HttpContext HttpContext { get; }
        string[] DetermineRequestCulture();
    }
}
