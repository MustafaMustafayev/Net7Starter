namespace SOURCE;

// ReSharper disable InconsistentNaming
public static class Constants
{
    public const string DataFileName = "data.json";
    public const string OpenApiFileName = "openapi.json";

    #region PATHS

    public const string ControllerPath = "API\\Controllers\\";

    public const string IRepositoryPath = "DAL\\EntityFramework\\Abstract\\";

    public const string IServicePath = "BLL\\Abstract\\";

    public const string IUnitOfWorkPath = "DAL\\EntityFramework\\UnitOfWork\\";

    public const string RepositoryPath = "DAL\\EntityFramework\\Concrete\\";

    public const string ServicePath = "BLL\\Concrete\\";

    public const string UnitOfWorkPath = "DAL\\EntityFramework\\UnitOfWork\\";

    public const string AutomapperPath = "BLL\\Mappers\\";

    public const string DtoPath = "DTO\\{entityName}\\";

    public const string EntitiesPath = "ENTITIES\\Entities\\";

    #endregion
}