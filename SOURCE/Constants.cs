namespace SOURCE;

// ReSharper disable InconsistentNaming
public static class Constants
{
    public const string DataFileName = "data.json";

    #region PATHS

    public const string ControllerPath = "API\\Controllers\\";

    public const string IRepositoryPath = "DAL\\Abstract\\";

    public const string IServicePath = "BLL\\Abstract\\";

    public const string IUnitOfWorkPath = "DAL\\UnitOfWorks\\Abstract\\";

    public const string RepositoryPath = "DAL\\Concrete\\";

    public const string ServicePath = "BLL\\Concrete\\";

    public const string UnitOfWorkPath = "DAL\\UnitOfWorks\\Concrete\\";

    public const string AutomapperPath = "BLL\\Mappers\\";

    public const string DtoPath = "DTO\\{entityName}\\";

    public const string EntitiesPath = "ENTITIES\\Entities\\";

    #endregion
}