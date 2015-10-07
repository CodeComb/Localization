﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace CodeComb.AspNet.Localization
{
    public class QueryStringRequestCultureProvider : IRequestCultureProvider
    {
        public string QueryField { get; set; }

        public QueryStringRequestCultureProvider(IHttpContextAccessor httpContextAccessor, string QueryField = "lang")
        {
            this.QueryField = QueryField;
            HttpContext = httpContextAccessor.HttpContext;
        }

        public HttpContext HttpContext { get; set; }

        public string[] DetermineRequestCulture()
        {
            return HttpContext.Request.Query[QueryField];
        }
    }
}
