namespace RadicalResearch.Web
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Optimization;

    public class ContentSesurityPolicy
    {
        private const string defaultScriptTagFormatPattern = "<script type=\"text/javascript\" src=\"{{0}}\" nonce=\"{0}\"></script>";

        private const string scriptNonceHttpContextItemsKey = "__ContentSesurityPolicy_ScriptNonce";

        private static readonly RNGCryptoServiceProvider _RngCryptoServiceProvider = new RNGCryptoServiceProvider();
		
        public static IHtmlString Render(params string[] paths)
        {
            return Scripts.RenderFormat(GetScriptFormatTagWithNonce(), paths);
        }

        private static string GetScriptFormatTagWithNonce()
        {
            return string.Format(CultureInfo.InvariantCulture, defaultScriptTagFormatPattern, GetScriptNonce());
        }

        public static string GetScriptNonce()
        {
            return GetScriptNonce(new HttpContextWrapper(HttpContext.Current));
        }

        public static string GetScriptNonce(HttpContextBase httpContext)
        {
            var scriptNonce = httpContext.Items[scriptNonceHttpContextItemsKey] as string;
            if (!string.IsNullOrEmpty(scriptNonce))
            {
                return scriptNonce;
            }

            scriptNonce = GenerateScriptNonce();
            httpContext.Items[scriptNonceHttpContextItemsKey] = scriptNonce;
            return scriptNonce;
        }

        public static string GenerateScriptNonce()
        {
            // Use 18 bytes because it is divisible by 3 so will have no padding characters
            var bytes = new byte[18];
            _RngCryptoServiceProvider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}