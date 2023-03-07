using System.Text.RegularExpressions;

namespace Io.HcxProtocol.Utils
{
    /// <summary>
    /// The UUID Util to validate its format.
    /// </summary>
    public class UUIDUtils
    {

        public static bool IsUUID(string s)
        {
            var pattern = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
            return Regex.IsMatch(s, pattern);
        }

    }
}
