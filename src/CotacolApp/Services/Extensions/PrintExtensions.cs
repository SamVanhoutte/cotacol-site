using System;
using System.Globalization;

namespace CotacolApp.Services.Extensions
{
    public static class PrintExtensions
    {
        public static string Decimal(this double value)
        {
            return value.ToString("#.##");
        }

        public static string Number(this double value, string suffix = null, bool belowZeroIsNull = false)
        {
            if (belowZeroIsNull && value <= 0) return "-";
            var numberValue = value.ToString("#,###.00", new NumberFormatInfo {NumberGroupSeparator = "."});
            if (!string.IsNullOrEmpty(suffix))
            {
                numberValue += suffix;
            }

            return numberValue;
        }

        public static string Number(this int value, string suffix = null, bool belowZeroIsNull = false)
        {
            long v = value;
            return v.Number(suffix, belowZeroIsNull);
        }

        public static string Number(this int? value, string suffix = null, bool belowZeroIsNull = false)
        {
            return value == null ? "-" : value.Value.Number(suffix, belowZeroIsNull);
        }

        public static string Number(this long value, string suffix = null, bool belowZeroIsNull = false)
        {
            if (belowZeroIsNull && value <= 0) return "-";
            var numberValue = value.ToString("#,###", new NumberFormatInfo {NumberGroupSeparator = "."});
            if (!string.IsNullOrEmpty(suffix))
            {
                numberValue += suffix;
            }

            return numberValue;
        }

        public static string Percentage(this double value)
        {
            return $"{value:#.0}%";
        }

        public static string WholeNumber(this double value, string suffix = null, bool belowZeroIsNull = false)
        {
            if (belowZeroIsNull && value <= 0) return "-";
            var numberValue = Math.Round(value, 0).ToString("#,###", new NumberFormatInfo {NumberGroupSeparator = "."});
            if (!string.IsNullOrEmpty(suffix))
            {
                numberValue += suffix;
            }

            return numberValue;
        }

        public static string Date(this DateTime? dateTime)
        {
            return dateTime == null ? "-" : dateTime.Value.ToString("MMM dd, yyyy");
        }

        public static string DateTime(this DateTime? dateTime)
        {
            return dateTime == null ? "-" : dateTime.Value.ToString("g");
        }

        public static string DateTime(this DateTime dateTime)
        {
            return dateTime == default ? "-" : dateTime.ToString("g");
        }
    }
}