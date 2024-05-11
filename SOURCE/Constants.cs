namespace SOURCE;

// ReSharper disable InconsistentNaming
public static class Constants
{
    public const string DATA_FILE_NAME = "data.json";
    public const string OPEN_API_FILE_NAME = "openapi.json";

    #region PATHS

    public const string CONTROLLER_PATH = "API\\Controllers\\";

    public const string I_REPOSITORY_PATH = "DAL\\EntityFramework\\Abstract\\";

    public const string I_SERVICE_PATH = "BLL\\Abstract\\";

    public const string I_UNIT_OF_WORK_PATH = "DAL\\EntityFramework\\UnitOfWork\\";

    public const string REPOSITORY_PATH = "DAL\\EntityFramework\\Concrete\\";

    public const string SERVICE_PATH = "BLL\\Concrete\\";

    public const string UNIT_OF_WORK_PATH = "DAL\\EntityFramework\\UnitOfWork\\";

    public const string AUTOMAPPER_PATH = "BLL\\Mappers\\";

    public const string DTO_PATH = "DTO\\{entityName}\\";

    public const string ENTITIES_PATH = "ENTITIES\\Entities\\";

    public const string ENTITY_CONFIGURATION_PATH = "DAL\\EntityFramework\\Configurations";
    #endregion
}