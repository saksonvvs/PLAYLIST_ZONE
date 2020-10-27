using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Code
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            if (!filterContext.HttpContext.Response.Headers.ContainsKey("Cache-Control"))
                filterContext.HttpContext.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate");


            if (!filterContext.HttpContext.Response.Headers.ContainsKey("Expires"))
                filterContext.HttpContext.Response.Headers.Add("Expires", "Thu, 01 Jan 1970 00:00:00 GMT");


            base.OnResultExecuting(filterContext);
        }
    }



}
