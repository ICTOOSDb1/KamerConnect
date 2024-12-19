using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
        return attribute != null ? attribute.Name : value.ToString();
    }
}
