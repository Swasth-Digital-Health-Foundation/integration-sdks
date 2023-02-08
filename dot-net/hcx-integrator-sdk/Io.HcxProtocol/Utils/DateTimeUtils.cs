using System;

namespace Io.HcxProtocol.Utils
{
    /// <summary>
    /// The Date time Util to validate timestamp.
    /// </summary>
    public class DateTimeUtils
    {
        public static bool ValidTimestamp(string timestamp)
        {
            try
            {
                DateTime requestTime = DateTime.Parse(timestamp);
                DateTime currentTime = DateTime.Now;
                return requestTime <= currentTime;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string TotalMillisecondsUTC()
        {
            return ((long)DateTime.Now.ToUniversalTime().Subtract(
                                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                ).TotalMilliseconds).ToString();
        }

    }
}
