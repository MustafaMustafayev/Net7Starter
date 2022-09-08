using AutoMapper;

namespace Project.BLL.Mappers;

public static class Automapper
{
    public static IEnumerable<Type> GetAutoMapperProfilesFromAllAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var aType in assembly.GetTypes())
            {
                if (aType.IsClass && !aType.IsAbstract && aType.IsSubclassOf(typeof(Profile)))
                    yield return aType;
            }
        }
    }
}