using System;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The Date time Util to validate timestamp and provide UniversalTime method.
    /// </summary>
    public static class DateTimeUtils
    {
        public static bool IsValidTimestamp(this string timestamp)
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
