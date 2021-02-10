using System.Globalization;

namespace CotacolApp.Services.Extensions
{
    public static class PrintExtensions
    {
        public static string Decimal(this double value)
        {
            return value.ToString("#.##");
        }

        public static string Number(this double value)
        {
            return value.ToString("#,###.00", new NumberFormatInfo {NumberGroupSeparator = "."});
        }
        public static string Number(this int value)
        {
            return value.ToString("#,###", new NumberFormatInfo {NumberGroupSeparator = "."});
        }
        public static string Number(this long value)
        {
            return value.ToString("#,###", new NumberFormatInfo {NumberGroupSeparator = "."});
        }
        
        public static string Percentage(this double value)
        {
            return $"{value:#.0}%";
        }
    }
}