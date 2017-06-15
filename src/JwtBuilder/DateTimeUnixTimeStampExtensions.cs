using System;

namespace JwtBuilder
{
    public static class DateTimeUnixTimeStampExtensions
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static double? ToUnixTimestamp(this DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;
            return (dateTime.Value - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
    }
}
