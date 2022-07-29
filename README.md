
[![CrispyCollab Build and Test](https://github.com/DavidEggenberger/CrispyCollab/actions/workflows/build.yml/badge.svg)](https://github.com/DavidEggenberger/CrispyCollab/actions/workflows/build.yml)

# CrispyCollab

This repository is a **reference application** for building monolithic SaaS solutions with ASP.NET Core, Blazor and EF Core. CrispyCollab features:

- Subscription-based billing (<a href="https://stripe.com/docs/payments/checkout">Stripe Checkout</a>)
- Multi-tenancy
- Tenant wide operations (sending chat messages)
- Admin Section (e.g. inviting users)
- Domain Driven Design
- CQRS (no library, own implementation through using Scrutor)

## Architecture

CrsipyCollab is a web application. The backend, ASP.NET Core, is consumed by the Blazor WebAssembly client. DTO's, constants and shared authorization logic resides in the WebShared class library. To simplifiy deployment/hosting the ASP.NET Core backend references the WebAssembly client. The whole application (excluding SQL Server) subsequently runs in one process. CrsipyCollab follows the paradigms of clean architecture. The application layer loads the specified data by the controllers (Queries), facilitates applying the desired changes (Commands) and reacts to subsequently invoked events (EventHandlers). To do so services defined in the infrastucture layer are used.              

<img src="https://raw.githubusercontent.com/DavidEggenberger/CrispyCollab/main/Img/ProjectDependencies.png" height=350/>

### Projects

**WebWasmClient**: Blazor WebAssembly Application \
**WebCommon**: DTOs, Constants, shared authorization logic \
**WebServer**: API, SignalR Hubs, Razor Pages (Identity, Landing Pages) \
**Application**: Application Logic (Queries, Commands, Events) \
**Infrastructure**: Infrastructure Services (e.g. Database access) \
**Domain**: Entities (Rich domain model, DDD)

### Architectural Concepts
#### Clean Architecture
CrsipyCollab follows the paradigms of Clean Architecture. Layering, dependency inversion, framework and database independence are the main principles of clean architecture. Structuring an application thereafter means spliting-up its code into three distinct layers, Application, Infrastructure and Domain. These layers are implemented as class libraries. Layering in the context of Clean Architecture now means that the dependencies of the respective class library can only point inwards. Referencing an outer circle is not possible. The innermost layer, the Domain layer, has subsequently no external dependencies. The infrastructure layer references the Domain layer. Through referencing the infrastructure layer the application layer has access to the types defined in both the infrastucture and domain class libraries. When an inner layer relies on functionality from an outer layer the inner layer defines an interface for which an outer layer will provide an implementation. This dependency inversion is of particular importance for the domain layer as it can't take on external dependencies. This allows to swap out infrastructure components, e.g. database access, at will without the need to change the domain layer.  

##### Application Layer
CQRS is used to organize the application logic. For each aggregate the Application class library defines the supported Commands and Queries. The respective Command and QueryHandlers reside in the same file. They get executed when the controllers dispatch the according Query or Command.

##### Infrastructure Layer

##### Domain Layer

#### Domain Driven Design
CrispyCollab models its entities after the paradigms of Domain Driven Design. Entities denote objects that are persisted in a database. The fields and properties of their respective classes, structs or records they were instantiated from then determines the database scheme.

#### Backend for Frontend Pattern

## Showcased SaaS Concepts
Software as a Service, abbreviated as SaaS, is a licensing and delivery model for software. SaaS solutions are mostly billed on a subscription basis and distributed through the web. This frees the users from long-term commitments and the need to install updates. Because the server infrastructure operated by the SaaS vendor stores and computes all the application’s data, the subscription fee can be priced upon usage parameters such as made transactions or the user count in a tenant. 

### Multitenancy
Multitenancy denotes the application simultaniously serving multiple tenants. A tenant is a user group of a SaaS solution. In the context of B2B (Business to Business) a user group is mostly congruent with the employees of the company that subscribes to the SaaS application. The respective admins can then manage the tenant (e.g. changing its name and appearance, modyfiny the functionality, inviting users, giving them admin rights etc.). To enforce that members of a tenant can only operate on data that belongs to their respective tenant every entity is marked with a TenantIdentifier. By using Global Query Filters they are then accordingly filtered. The TenantIdentifier is retrieved from the AuthenticationCookie. It also persists the user's role in the tenant. This implies that whenever the user changes his/her current tenant the AuthenticationCookie is deleted and replaced with a new one.  

### Subscription-based billing
CrispyCollab uses <a href="https://stripe.com/docs/payments/checkout">Stripe Checkout</a>. Through Stripe's dashboard the subscription plans can be created. Each one is identifiable through an Id. The<a href="https://github.com/DavidEggenberger/CrispyCollab/blob/main/Source/Infrastructure/Identity/IdentityDbSeeder.cs"> IdentityDbSeeder</a> then stores the according subscriptions in the database. 

## Running CrispyCollab
### Docker
When running through Docker the appsettings are read from the appsettings.docker.json file. You can run CrispyCollab by running these commands from the root folder (where CrispyCollab.sln file is located):
```
docker-compose build
docker-compose up
```

### Local
When running CrispyCollab locally SQL Server must be installed. The connection string must be added into appsettings.json. From the package manager console you can run the following commands:
```
add-migration initialApplication -context ApplicationDbContext
add-migration initialIdentity -context IdentityDbContext
update-database -context ApplicationDbContext
update-database -context IdentityDbContext
```
