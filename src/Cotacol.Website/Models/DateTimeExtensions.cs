namespace Cotacol.Website.Models;

public static class DateTimeExtensions
{
    public static DateTime MinValue => new DateTime(2001, 01, 1);
    public static DateTime MaxValue => new DateTime(DateTime.UtcNow.Year + 30, 01, 1);

    public static DateTime GetNullableMin(this DateTime? dateTime)
    {
        if (dateTime == null)
        {
            return MinValue;
        }

        return dateTime.Value;
    }

    public static DateTime GetNullableMax(this DateTime? dateTime)
    {
        if (dateTime == null)
        {
            return MaxValue;
        }

        return dateTime.Value;
    }
    
    public static bool HasMaxedValue(this DateTime? dateTime)
    {
        if (dateTime == null)
        {
            return true;
        }

        return DateTime.UtcNow.AddYears(20) >  dateTime.Value;
    }
}