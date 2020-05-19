using System;

namespace LobbyPad {
    public static class DateTimeJavaScript
    {
        private static readonly DateTime UnixEpoc =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToJavascriptTime(this DateTime dateTime)
        {
            return (long)Math.Floor((dateTime.ToUniversalTime() - UnixEpoc).TotalMilliseconds);
        }
    }
}