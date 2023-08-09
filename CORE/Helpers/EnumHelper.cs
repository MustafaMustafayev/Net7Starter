using System.ComponentModel;

namespace CORE.Helpers;

public abstract class EnumHelper
{
    public static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
        return attribute?.Description ?? value.ToString();
    }
}