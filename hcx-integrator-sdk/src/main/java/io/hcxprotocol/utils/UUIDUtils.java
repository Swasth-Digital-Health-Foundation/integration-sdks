package io.hcxprotocol.utils;

import lombok.experimental.UtilityClass;

/**
 * The UUID Util to validate its format.
 */
@UtilityClass
public class UUIDUtils {
    public static boolean isUUID(String s) {
        return s.matches("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
    }
}
