# Prepare for deployment

Complete the following tasks when the application is ready for deployment.

* [ ] Create server-specific settings and config files and add copies to the "app-config" repository.
* [ ] Create Web Deploy Publish Profiles for each web server using the "Example-Server.pubxml" file as an example.
* [ ] Configure the following external services as needed:
    - [ ] [Azure App registration](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) to manage employee authentication. *(Add configuration settings in the "AzureAd" section in a server settings file.)*
      When configuring the app in the Azure Portal, add optional claims for "email", "family_name", and "given_name" under "Token configuration".
    - [ ] [Raygun](https://app.raygun.com/) for crash reporting and performance monitoring. *(Add the API key to the "RaygunSettings" section in a server settings file.)*
    - [ ] [Better Uptime](https://betterstack.com/better-uptime) for site uptime monitoring. *(No app configuration needed.)*
