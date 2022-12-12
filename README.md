# Net6ApiStarter

### Code contributors

![Alt](https://repobeats.axiom.co/api/embed/8a34edc6094601ed624424f8b121ad4937d0f8dd.svg "Repobeats analytics image")

> .NET 6.0 Web Api project with Entity framework 6 code first approach. In this template n-tier architecture, repository and unit of work pattern implemented,
> database based logging implemented using action filters, request profiling added using MiniProfiler, unhandled exception handled by sentry,
> response security headers added, audit properties implemented by overriding SaveChangesAsync method of DbContext, docker-compose yaml written,
> authentication and authorization implemented using custom middleware, generic CRUD operations implemented, automapper configured,
> simple one to many role and permission logic implemented,
> password policy implemented
> antiforgery token implemented
> generic pagination, global exception handling, localization, whitelist, custom generic automapping & validations between entity and dtos implemented.
> Database and application dockerized. Docker compose implemented with build step.

## Structure

    │── ── ── ── ── ── ── ── ── ── ── ── ── ──
    │ .NET 6.0 WebApi Starter Project
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
    │   │   └── IUtilService
    │   ├── Concrete
    │   │   └── UtilService
    │   ├── Constants
    │   │   ├── Constants
    │   │   ├── LocalizationConstants
    │   │   └── Messages
    │   ├── Config
    │   │   ├── AuthSettings
    │   │   ├── ConfigSettings
    │   │   ├── ConnectionStrings
    │   │   ├── Controllable
    │   │   ├── HttpClientSettings
    │   │   ├── HttpHeader
    │   │   ├── RedisSettings
    │   │   ├── RequestSettings
    │   │   ├── SentrySettings
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
    └── ── ── ── ── ── ── ── ── ── ── ── ── ──
