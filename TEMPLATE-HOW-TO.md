# Template Application Setup and Use

The following steps describe how to use the template for a new application.

## Initialize the template files

* Run the "create-sln.ps1" file to create a new solution file.
* Rename or search and replace the following terms. *(Caution: not all of these will be visible in the Visual Studio solution view.)*
    - "MY_APP_NAME" - Replace with the readable display name of the app.
    - "MY_APP" - Replace with the short name or acronym of the app.
    - `MyApp`:
        - Rename the root namespace for the app.
        - Update the `<RootNamespace>` element in each "csproj" file.
        - Update the namespace in the "_ViewImports.cshtml" file.
        - Update the exclusions in the coverlet commands in the "sonarcloud-scan.yml" file.
        - Update the exclusions in the "finecodecoverage-settings.xml" file.
    - "template-app" - Search and replace with the repository name. This will affect the following:
        - The LocalDB database name in various connection strings.
        - The project key in the "sonarcloud-scan.yml" workflow file.
        - The comment URL path in the "lighthouse-analysis.yml" workflow file.
        - The URLs in the GitHub and SonarCloud badges in the "README.md" file.
        - The URL on the support page.

## Customize the application

* Update these files with information about the new application:
    * README.md
    * docs/Entity relationship diagram.md
    * docs/Role capabilities.md
    * docs/Site map.md
* Change the branding colors in "src\WebApp\wwwroot\css\site.css" by adjusting the `--base-hue` variable.

## Prepare for deployment

Complete the following tasks when the application is ready for deployment.

* Create server-specific settings and config files and add copies to the "app-config" repository.
* Create Web Deploy Publish Profiles for each web server using the "Example-Server.pubxml" file as an example.
* Configure the following external services as needed:
    - [Azure App registration](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) to manage employee authentication. *(Add configuration settings in the "AzureAd" section in a server settings file.)*
      When configuring the app in the Azure Portal, add optional claims for "email", "family_name", and "given_name" under "Token configuration".
    - [Raygun](https://app.raygun.com/) for crash reporting and performance monitoring. *(Add the API key to the "RaygunSettings" section in a server settings file.)*
    - [SonarCloud](https://sonarcloud.io/projects) for code quality and security scanning. *(Update the project key in the "sonarcloud-scan.yml" workflow file and in the badges above.)*
    - [Better Uptime](https://betterstack.com/better-uptime) for site uptime monitoring. *(No app configuration needed.)*
