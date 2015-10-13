using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace CodeComb.AspNet.Localization
{
    public class CookieRequestCultureProvider : IRequestCultureProvider
    {
        public HttpContext HttpContext { get; set; }

        public string CookieField { get; set; }

        public CookieRequestCultureProvider(IHttpContextAccessor httpContextAccessor, string CookieField = "ASPNET_LANG")
        {
            this.CookieField = CookieField;
            HttpContext = httpContextAccessor.HttpContext;
        }

        public string[] DetermineRequestCulture()
        {
            if (HttpContext.Request.Cookies[CookieField] == null)
            {
                var ret = new List<string>();
                var tmp = HttpContext.Request.Headers["Accept-Language"].FirstOrDefault();
                if (tmp == null)
                    return new string[] { };
                var split = tmp.Split(',');
                foreach(var x in split)
                {
                    ret.Add(x.Split(';')[0]);
                }
                return ret.ToArray();
            }
            else
            {
                return new string[] { HttpContext.Request.Cookies[CookieField].ToString() };
            }
        }
    }
}
