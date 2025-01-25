param (
    [Parameter(Mandatory=$true)]
    [string]$subscriptionId,

    [Parameter(Mandatory=$true)]
    [string]$developerUserName,

    [Parameter(Mandatory=$true)]
    [string]$location
)

# Define variables
$resourceGroupName = "sample-app-rg"
$templateFile = "./deploy/deployAll.bicep"
$deploymentName = "sample-app-deployment"

# Read SQL admin password from environment variable
$sqlAdminPassword = $env:SQL_ADMIN_PASSWORD

# Check if SQL admin password exists
if (-not $sqlAdminPassword) {
    Write-Error "SQL admin password is not set in the environment variable SQL_ADMIN_PASSWORD."
    exit 1
}

# Login to Azure
az account set --subscription $subscriptionId

# Create resource group if it doesn't exist
az group create --name $resourceGroupName --location $location

# Deploy the Bicep template
az deployment group create `
    --name $deploymentName `
    --resource-group $resourceGroupName `
    --template-file $templateFile `
    --parameters developerUsername=$developerUserName sqlAdminPassword=$sqlAdminPassword