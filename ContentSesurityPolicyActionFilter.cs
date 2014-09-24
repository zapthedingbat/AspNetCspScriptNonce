namespace RadicalResearch.Web.Mvc
{
    using System.Globalization;
    using System.Web.Mvc;

    public class ContentSesurityPolicyActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var scriptSource = string.Format(CultureInfo.InvariantCulture, "script-src 'nonce-{0}' 'self'", ContentSesurityPolicy.GetScriptNonce());
            filterContext.HttpContext.Response.AddHeader("Content-Security-Policy", scriptSource);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}