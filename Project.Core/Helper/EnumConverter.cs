namespace Project.Core.Helper;

public class EnumConverter<T>
    where T : Enum
{
    public static IEnumerable<string> GetAllValuesAsIEnumerable()
    {
        return Enum.GetNames(typeof(T));
    }
}