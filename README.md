# Mercury Access Admin Backend API Challenge

## Dependencies

- SQL Server 2016
- .NET Core 3.1 SDK
- Postman

## Configuration

all configuration values can be found in *src/CoreAccessControl/CoreAccessControl.API/appsettings.json*

| key                       | description                                                                   |
| --------------------------| ------------------------------------------------------------------------------|
| Database.ConnectionString | the connection string for database connection                                 |
| Email.Host                |SMTP  host address                                                             |
| Email.Port                |SMTP  post                                                                     |
| Email.EnableSsl           |SMTP  ssl enable property                                                      |
| Email.Username            |SMTP username                                                                  |
| Email.Password            |SMTP password                                                                  |
| Email.FromAddress         |the email address will be used in email for sender                             |
| Domain                    |Application url (example: www.google.com or localhost:80 (if deployed locally))|
| RemoteServer.BasePath     |the remote core api base url                                                   |
| RemoteServer.UserName     |the remote core api auth username                                              |
| RemoteServer.Password     |the remote core api auth password                                              |
| Secret                    |the secret key for salt generation                                             |
| Limit                     |pagin limit                                          |

## Local deployment

1. goto *src/CoreAccessControl/Database/scripts* then run the `schema.sql`, `logger.sql` and `data.sql` in your sqlserver instance sequentially.
2. Change `Domain` of *src/CoreAccessControl/CoreAccessControl.API/appconfig.json* by any of the *src\CoreAccessControl\CoreAccessControl.API\Properties\launchSettings.json*  `CoreAccessControl.API.applicationUrl`
2. goto *src/CoreAccessControl/CoreAccessControl.API* then run command `dotnet run`, this will run the project in kestrel.

## Azure Deployment

- Create a database in azure by following [database in azure](https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-dotnetcore).
- Create the connection string bt=y following the [guideline](https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-dotnetcore) and store in *src/CoreAccessControl/CoreAccessControl.API/appconfig.json* `Database.ConnectionString`
- goto *src/CoreAccessControl/Database/scripts* then run the `schema.sql`, `logger.sql` and `data.sql` in your sqlserver instance sequentially.
- Create a azure web app (windows) by following the document [Create an ASP.NET Core web app in Azure](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-get-started-dotnet) or
- Create a azure web app (linux) by following the document [Create an ASP.NET Core web app in Azure on Linux](https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-dotnetcore)
- Deploy the app by follwing the [link](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-dotnetcore-sqldb?toc=%2Faspnet%2Fcore%2Ftoc.json&bc=%2Faspnet%2Fcore%2Fbreadcrumb%2Ftoc.json&view=aspnetcore-3.1#deploy-app-to-azure)

- Note : Change the Domain and other configuration as necessary.

## Test & verification

- goto *src/CoreAccessControl/CoreAccessControl.API.Test* run command `dotnet test` this project include api unit testing.
- goto *src/CoreAccessControl/CoreAccessControl.Services.Test* run command `dotnet test` this project include business unit testing.
- - goto *src/CoreAccessControl/docs/* use the `postman_collection.json` file for manual testing.

## NOTE

- the admin user info : un: admin@admin.com pwd: 123456 (please note that the password may vary if you change the salt/secret in config file)
- Update all the configurations as necessary.
- For excel I have used [EPPlus](https://www.epplussoftware.com/) software (currently in non commerial mode.)

