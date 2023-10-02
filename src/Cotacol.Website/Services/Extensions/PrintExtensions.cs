using System.Globalization;

namespace Cotacol.Website.Services.Extensions
{
    public static class PrintExtensions
    {
        public static string Decimal(this double value)
        {
            return value.ToString("#.##");
        }

        public static string Number(this double value, string suffix = null, bool belowZeroIsNull = false,bool addSign = false)
        {
            if (belowZeroIsNull && value <= 0) return "-";
            var numberValue = value.ToString("#,###.00", new NumberFormatInfo {NumberGroupSeparator = "."});
            if (addSign && value != 0)
            {
                var sign = value < 0 ? "" : "+";
                numberValue = sign + numberValue;
            }
            if (!string.IsNullOrEmpty(suffix))
            {
                numberValue += suffix;
            }

            return numberValue;
        }

        public static string Number(this int value, string suffix = null, bool belowZeroIsNull = false,
            bool addSign = false)
        {
            long v = value;
            return v.Number(suffix, belowZeroIsNull, addSign);
        }

        public static string Number(this int? value, string suffix = null, bool belowZeroIsNull = false,bool addSign = false)
        {
            return value == null ? "-" : value.Value.Number(suffix, belowZeroIsNull, addSign);
        }

        public static string Number(this long value, string suffix = null, bool belowZeroIsNull = false,bool addSign = false)
        {
            if (belowZeroIsNull && value <= 0) return "-";
            var numberValue = value.ToString("#,###", new NumberFormatInfo {NumberGroupSeparator = "."});
            if (addSign && value != 0)
            {
                var sign = value < 0 ? "" : "+";
                numberValue = sign + numberValue;
            }
            if (!string.IsNullOrEmpty(suffix))
            {
                numberValue += suffix;
            }

            return numberValue;
        }

        
        public static string Percentage(this double value, bool fractional = false)
        {
            if (fractional) value = value * 100D;
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