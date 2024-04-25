# NetAPIStarter

### Code contributors

![Alt](https://repobeats.axiom.co/api/embed/f8c50b5c55ce520d8198a81cb6f63150cec32209.svg "Repobeats analytics image")

> .NET Web Api project with Entity framework 6 code first approach. In this template n-tier architecture, repository and unit of work pattern implemented,
> database based logging implemented using action filters, request profiling added using MiniProfiler, unhandled exception handled by sentry,
> response security headers added, audit properties implemented by overriding SaveChangesAsync method of DbContext, docker-compose yaml written,
> authentication and authorization implemented using custom middleware, generic CRUD operations implemented, automapper configured,
> simple one to many role and permission logic implemented,
> password policy implemented,
> anti forgery token implemented,
> generic pagination, global exception handling, localization, whitelist, custom generic automapping & validations between entity and dtos implemented,
> mail sender implemented,
> rate limiting implemented,
> encoding and decoding implemened,
> sftp functions implemented,
> source code generator for DAL, BLL, API layer implemented,
> Database and application dockerized. Docker compose implemented with build step,
> automated service registration implemented using Scrutor,
> unit test implemented,
> load test implemented,
> Mediatr seperated as a independent layer,
> Elasticsearch implemented,
> MongoDb implemented.

## Structure

    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │ .NET 7.0 WebApi Starter Project
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── API 
    │   ├── ActionFilters
    │   │   ├── LogActionFilter
    │   │   └── ModelValidatorActionFilter
    │   ├── Attributes
    │   │   ├── ValidateForgeryTokenAttribute
    │   │   └── ValidateTokenAttribute
    │   ├── Containers
    │   │   └── DependencyContainer
    │   ├── Controllers
    │   │   ├── UserController
    │   │   └── ...
    │   ├── Graphql
    │   │   ├── Role
    │   │   │   ├── Mutation
    │   │   │   └── Query
    │   │   └── ...
    │   ├── Hubs
    │   │   ├── UserHub
    │   │   └── ...
    │   ├── Middlewares
    │   │   ├── AntiForgery
    │   │   │   ├── AntiForgeryTokenValidator
    │   │   │   └── ValidateAntiForgeryTokenMiddleware
    │   │   ├── ExceptionMiddleware
    │   │   └── LocalizationMiddleware
    │   └── Services
    │       └── RedisIndexCreatorService
    │
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── BLL     
    │   ├── Abstract
    │   │   ├── IUserService
    │   │   └── ...
    │   ├── Concrete
    │   │   ├── UserService
    │   │   └── ...
    │   ├── Mappers
    │   │   ├── Automapper
    │   │   ├── UserMapper
    │   │   └── ...
    │   └── MediatR
    │       ├── OrganizationCQRS
    │       │   ├── Commands
    │       │   │   ├── AddOrganizationCommand
    │       │   │   └── ...
    │       │   ├── Handlers
    │       │   │   ├── AddOrganizationHandler
    │       │   │   ├── GetOrganizationListHandler
    │       │   │   └── ...
    │       │   └── Queries
    │       │       ├── GetOrganizationListQuery
    │       │       └── ...
    │       └── ...
    │       //TODO ADD RABBITMQ HERE
    │ 
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── CORE
    │   ├── Abstract
    │   │   └── ISftpService
    │   │   └── IUtilService
    │   ├── Concrete
    │   │   └── SftpService
    │   │   └── UtilService
    │   ├── Constants
    │   │   ├── Constants
    │   │   └── LocalizationConstants
    │   ├── Config
    │   │   ├── AuthSettings
    │   │   ├── ConfigSettings
    │   │   ├── ConnectionStrings
    │   │   ├── Controllable
    │   │   ├── CryptographySettings
    │   │   ├── HttpClientSettings
    │   │   ├── HttpHeader
    │   │   ├── MailSettings
    │   │   ├── RedisSettings
    │   │   ├── RequestSettings
    │   │   ├── SentrySettings
    │   │   ├── SftpSettings
    │   │   └── SwaggerSettings
    │   ├── Helper
    │   │   ├── ExpressionHelper
    │   │   ├── FileHelper
    │   │   ├── FilterHelper
    │   │   ├── ObjectSerializer
    │   │   └── SecurityHelper
    │   ├── Logging
    │   │   ├── ILoggerManager
    │   │   └── LoggerManager
    │   └── Localization
    │       ├── TranslatorExtension
    │       ├── Messages
    │       ├── MsgResource.az
    │       ├── MsgResource.en
    │       └── MsgResource.ru
    │
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── DAL
    │   ├── Abstract
    │   │   ├── IUserRepository
    │   │   └── ...
    │   ├── Concrete
    │   │   ├── UserRepository
    │   │   └── ...
    │   ├── CustomMigrations
    │   │   └── DataSeed
    │   ├── DatabaseContext
    │   │   └── DataContext
    │   ├── GenericRepositories
    │   │   ├── Abstract
    │   │   │   └── IGenericRepository
    │   │   └── Concrete
    │   │       └── GenericRepository
    │   ├── Migrations
    │   │   └── ...
    │   ├── UnitOfWorks
    │   │   ├── Abstract
    │   │   │   └── IUnitOfWork
    │   │   └── Concrete
    │   │       └── UnitOfWork
    │   └── Utility
    │       ├── PaginatedList 
    │       └── PaginationInfo
    │
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── DTO  
    │   ├── User
    │   │   ├── UserValidators
    │   │   │   ├── AddDtoValidator
    │   │   │   └── ...
    │   │   ├── UserToAddDto
    │   │   └── ...
    │   └── ...
    │
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── ENTITIES
    │   ├── Entities
    │   │   ├── Redis
    │   │   │   └── ..
    │   │   ├── Logging
    │   │   │   └── ..
    │   │   ├── User
    │   │   └── ..
    │   ├── Enums
    │   │   ├── UserType
    │   │   └── ..
    │   └── IEntity
    │ 
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── SOURCE
    │   ├── Builders
    │   ├── Helpers
    │   ├── Models
    │   └── Workers
    │
    └── ── ── ── ── ── ── ── ── ── ── ── ── ──
