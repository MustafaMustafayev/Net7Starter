# Net6ApiStarter

### Code contributors

![Alt](https://repobeats.axiom.co/api/embed/8a34edc6094601ed624424f8b121ad4937d0f8dd.svg "Repobeats analytics image")

> .NET 6.0 Web Api project with Entity framework 6 code first approach. In this template n-tier architecture, repository and unit of work pattern implemented,
> database based logging implemented using action filters, request profiling added using MiniProfiler, unhandled exception handled by sentry,
> response security headers added, audit properties implemented by overriding SaveChangesAsync method of DbContext, docker-compose yaml written,
> authentication and authorization implemented using custom middleware, generic CRUD operations implemented, automapper configured,
> generic pagination, global exception handling, localization, whitelist, custom generic automapping & validations between entity and dtos implemented.
> Simple book library model implemented for test issues.
> Database and application dockerized. Docker compose implemented with build step.

## Structure

    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │ .NET 6.0 WebApi Starter Project
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── API 
    │   ├── ActionFilters
    │   │   ├── LogActionFilter
    │   │   └── ValidatorActionFilter
    │   ├── BackgroundServices
    │   │   └── TokenKeeperService
    │   ├── Controllers
    │   │   ├── UserController
    │   │   └── ...
    │   ├── CustomAttributes
    │   │   └── CacheTokenValidateAttribute
    │   ├── DependencyContainers
    │   │   └── StockDependencyContainer
    │   └── Hubs
    │       ├── UserHub
    │       └── ...
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
    │ 
    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │
    ├── CORE
    │   ├── Abstract
    │   │   └── IUtilService
    │   ├── Concrete
    │   │   └── UtilService
    │   ├── Constants
    │   │   ├── Constants
    │   │   ├── LocalizationConstants
    │   │   └── Messages
    │   ├── Enums
    │   │   └── URole
    │   ├── Helper
    │   │   ├── AuthSettings
    │   │   ├── ConfigSettings
    │   │   ├── ConnectionStrings
    │   │   ├── EnumConverter
    │   │   ├── ExpressionHelper
    │   │   ├── FileHelper
    │   │   ├── FilterHelper
    │   │   ├── ObjectSerializer
    │   │   ├── RequestSettings
    │   │   ├── SecurityHelper
    │   │   └── SwaggerSettings
    │   ├── Logging
    │   │   ├── ILoggerManager
    │   │   └── LoggerManager
    │   └── Middlewares
    │       ├── ExceptionHandler
    │       │   └── ExceptionMiddleware
    │       └── Translation
    │           ├── Localization
    │           ├── LocalizationMiddleware
    │           ├── LocalizationMiddleware
    │           ├── Messages
    │           ├── MsgResource.az
    │           ├── MsgResource.en
    │           └── MsgResource.ru
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
    └── ENTITY
        ├── Entities
        │   ├── User
        │   └── ..
        ├── Enums
        │   ├── UserType
        │   └── ..
        └── IEntities
            └── IEntity
