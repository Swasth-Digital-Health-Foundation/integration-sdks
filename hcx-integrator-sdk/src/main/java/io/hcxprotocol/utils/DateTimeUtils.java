package io.hcxprotocol.utils;

import lombok.experimental.UtilityClass;
import org.joda.time.DateTime;

/**
 * The Date time Util to validate timestamp.
 */
@UtilityClass
public class DateTimeUtils {

    public static boolean validTimestamp(String timestamp) {
        try {
            DateTime requestTime = new DateTime(timestamp);
            DateTime currentTime = DateTime.now();
            return !requestTime.isAfter(currentTime);
        } catch (Exception e) {
            return false;
        }
    }

}
