using DAL.Storage;
using System.Globalization;
using System.Threading;

namespace DAL.Services
{
    public static class LocalizationService
    {
        public static void ApplyLanguage(AppLanguage lang)
        {
            var culture = (lang == AppLanguage.Hr)
                ? new CultureInfo("hr-HR")
                : new CultureInfo("en-US");

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}
