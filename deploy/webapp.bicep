param location string = resourceGroup().location
param webAppName string = 'myWebApp'
param appSettings object[]

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: 'sample-app-service-plan'
  location: location
  sku: {
    name: 'S1'
    tier: 'Standard'
    capacity: 1
  }
}

resource webApp 'Microsoft.Web/sites@2021-02-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: appSettings
    }
  }
}

output webAppEndpoint string = webApp.properties.defaultHostName
