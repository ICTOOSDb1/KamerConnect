using System.ComponentModel;
using System.Reflection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KamerConnect.Utils;

public static class GetAttribute
{
    public static TAttribute GetAttributes<TAttribute>(this Enum enumValue) 
        where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
    }
    
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
        return attribute != null ? attribute.Name : value.ToString();
    }
}