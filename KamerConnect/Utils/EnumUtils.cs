using System.ComponentModel;

namespace KamerConnect.Utils;

public static class EnumUtils
{
    public static T Validate<T>(string value) where T : struct
    {
        if (Enum.TryParse(value, out T result) && Enum.IsDefined(typeof(T), result))
        {
            return result;
        }

        throw new ArgumentException($"Invalid value '{value}' for enum {typeof(T).Name}");
    }
}
