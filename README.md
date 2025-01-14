# Air Web Application

The Air Web app is operated to collect and organize the data required to operate an efficient air quality regulatory
program for the State of Georgia Environmental Protection Division (EPD) Air Protection Branch (APB).

[![Georgia EPD-IT](https://raw.githubusercontent.com/gaepdit/gaepd-brand/main/blinkies/blinkies.cafe-gaepdit.gif)](https://github.com/gaepdit)
[![.NET Test](https://github.com/gaepdit/air-web/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/gaepdit/air-web/actions/workflows/dotnet-test.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_air-web&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_air-web)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_air-web&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_air-web)

## Background and project requirements

The application is under active development to replace similar functionality currently housed in
the [Integrated Air Information Platform](https://github.com/gaepdit/iaip) (IAIP). As each new module is developed, it
will be removed from the IAIP until all functionality has been migrated and the IAIP can be retired.

This long-term project began with
the [Small Business Environmental Assistance Program](https://github.com/gaepdit/sbeap) which was migrated into a
standalone application.

The current effort focuses on the Stationary Source Compliance Program, specifically the compliance monitoring and
enforcement modules (which are also used by the EPD District Offices). This effort will also require updates to
our [ICIS-Air data flows](https://github.com/gaepdit/icis-air-data-exchange).

The remaining IAIP modules are described in [this discussion topic](https://github.com/gaepdit/air-web/discussions/50).

---

## Info for developers

This is an ASP.NET web application.

### Project ownership

The overall project is owned by the Air Protection Branch. Various modules are owned by the appropriate Programs within
the Branch.

### Prerequisites for development

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET SDK](https://dotnet.microsoft.com/download)

### Preparing for deployment

Complete the following tasks when the application is ready for deployment.

* Create server-specific settings and config files and add copies to the "app-config" repository.
* Create Web Deploy Publish Profiles for each web server using the "Example-Server.pubxml" file as an example.
* Configure the following external services as needed:
    - [Azure App registration](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) to
      manage employee authentication. *(Add configuration settings in the "AzureAd" section in a server settings file.)*
      When configuring the app in the Azure Portal, add optional claims for "email", "family_name", and "given_name"
      under "Token configuration".
    - [Raygun](https://app.raygun.com/) for crash reporting and performance monitoring. *(Add the API key to the "
      RaygunSettings" section in a server settings file.)*
    - [SonarCloud](https://sonarcloud.io/projects) for code quality and security scanning. *(Update the project key in
      the "sonarcloud-scan.yml" workflow file and in the badges above.)*
    - [Better Uptime](https://betterstack.com/better-uptime) for site uptime monitoring. *(No app configuration
      needed.)*

### Project organization

The solution contains the following projects:

* **Domain** — A class library containing the data models, business logic, and repository interfaces.
* **AppServices** — A class library containing the services used by an application to interact with the domain.
* **LocalRepository** — A class library implementing the repositories and data stores using static in-memory test data
  (for local development).
* **EfRepository** — A class library implementing the repositories and data stores using Entity Framework and a
  database (as specified by the configured connection string).
* **WebApp** — The front end web application and/or API.
* **TestData** — A class library containing test data for development and testing.

There are also corresponding unit test projects for each (not counting the `TestData` project).

### Development settings

The following settings section configures the data stores, authentication, and other settings for development purposes.
To work with these settings, add an `appsettings.Development.json` file in the root of the `WebApp` folder with a
`DevSettings` section, and make your changes there. Here's a sample `appsettings.Development.json` file to start out:

```json
{
  "DevSettings": {
    "UseDevSettings": true,
    "UseInMemoryData": true,
    "UseEfMigrations": false,
    "DeleteAndRebuildDatabase": true,
    "UseAzureAd": false,
    "LocalUserIsAuthenticated": true,
      "LocalUserRoles": [],
      "UseSecurityHeadersInDev": false,
      "EnableWebOptimizer": false
  }
}
```

- *UseDevSettings* — Indicates whether the following Dev settings should be applied.
- *UseInMemoryData*
    - When `true`, the `LocalRepository` project is used for repositories and data stores. Data is initially seeded from
      the `TestData` project.
    - When `false`, the `EfRepository` project is used, and a SQL Server database (as specified by the connection
      string) is created. (If the connection string is missing, then a temporary EF Core in-memory database provider is
      used. This option is included for convenience and is not recommended.)
- *UseInMemoryIaipData* — Works the same as `UseInMemoryData`, but for the IAIP data services.
- *UseEfMigrations* — Uses Entity Framework database migrations when `true`. When `false`, the`DeleteAndRebuildDatabase`
  setting controls how the database is handled. (Only applies if `UseInMemoryData` is`false`.)
- *DeleteAndRebuildDatabase* — When set to `true`, the database is deleted and recreated on each run. When set to
  `false`, the database is not modified on each run. (Only applies if `UseInMemoryData` and `UseEfMigrations` are both
  `false`.) If the database does not exist yet, it will not be created if this is set to `false`. The database is seeded
  with data from the `TestData` project only when `UseEfMigrations` is `false` and `DeleteAndRebuildDatabase` is `true`.
  Otherwise, the data in the database is not changed.
- *UseAzureAd* — If `true`, connects to Azure AD for user authentication. (The app must be registered in the Azure
  portal, and configuration added to the settings file.) If `false`, authentication is simulated using test user data.
- *LocalUserIsAuthenticated* — Simulates a successful login with a test account when `true`. Simulates a failed login
  when `false`. (Only applies if `UseAzureAd` is `false`.)
- *LocalUserRoles* — Adds the listed App Roles to the logged in account. (Only applies if `LocalUserIsAuthenticated` is
  `true`.)
- *UseSecurityHeadersLocally* — Sets whether to include HTTP security headers when running locally in the Development
  environment.
- *EnableWebOptimizer* — Enables the WebOptimizer middleware for bundling and minification of CSS and JavaScript
  files. (This is disabled by default to simplify debugging.)

#### Seeding user roles

There are two ways to seed user roles, depending on the authentication method chosen.

When authenticating as a test user (`UseAzureAd` set to `false`), the `LocalUserRoles` setting determines the roles
assigned to the user. Add the desired roles to the array using their short name. For example:

```json
{
    "LocalUserRoles": [
        "Staff",
        "SiteMaintenance"
    ]
}
```

When authenticating using AzureAD (`UseAzureAd` set to `true`), roles can be seeded using the `SeedUserRoles` setting.
This setting is also used in production if needed to help seed initial admin roles. The roles are added to the user's
account the first time they log in. For example:

```json
{
    "SeedUserRoles": [
        {
            "User": "user1@example.com",
            "Roles": [
                "UserAdmin",
                "Staff"
            ]
        }
    ]
}
```

#### Production defaults

In a production or staging environment (or if `UseDevSettings` is set to `false` or the `DevSettings` section is
missing), the settings are automatically set to production defaults as follows:

```csharp
UseDevSettings = false,
UseInMemoryData = false,
UseEfMigrations = true,
DeleteAndRebuildDatabase = false,
UseAzureAd = true,
EnableWebOptimizer = true,
```

#### Data persistence

Here's a visualization of how the settings configure data storage at runtime.

```mermaid
flowchart LR
    subgraph SPL["'UseInMemoryData' = true"]
        direction LR
        D[Domain]
        T["Test Data (in memory)"]
        R[Local Repositories]
        A[App Services]
        W([Web App])
        W --> A
        A --> D
        A --> R
        R --> T
        T --> D
    end
```

```mermaid
flowchart LR
    subgraph SPB["'UseInMemoryData' = false"]
        direction LR
        D[Domain]
        T[Test Data]
        R[EF Repositories]
        A[App Services]
        W([Web App])
        B[(Database)]
        W --> A
        A --> D
        R --> B
        A --> R
        T -->|Seed| B
        B --> D
    end
```

```mermaid
flowchart LR
    subgraph SPD["Production or staging environment"]
        direction LR
        D[Domain]
        R[EF Repositories]
        A[App Services]
        W([Web App])
        B[(Database)]
        W --> A
        A --> D
        A --> R
        R --> B
        B --> D
    end
```
