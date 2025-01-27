# Sample app setup procedure

## Set the environment variables

The password must be 8 chars long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.

```powershell
$env:SUBSCRIPTION_ID = "<your subscription id here>"
$env:RESOURCE_GROUP_NAME = "<your resource group name here>"
$env:DEVELOPER_USERNAME = "<your username here>"
$env:SQL_ADMIN_PASSWORD = "<your password here>"
$env:LOCATION = "<your location here>"
```

## Create SQL Server and Web app

```powershell
.\deploy.ps1 -subscriptionId $env:SUBSCRIPTION_ID -resourceGroupName $env:RESOURCE_GROUP_NAME -developerUserName $env:DEVELOPER_USERNAME -location $env:LOCATION
```

## Setup the github deployment from that repository

Use Azure Portal Deployment Center to setup the deployment from your github repository.

