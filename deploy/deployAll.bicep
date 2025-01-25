@minLength(1)
@maxLength(8)
@description('Your username here, fellow developer.')
param developerUsername string

@secure()
@minLength(1)
@maxLength(8)
@description('Sql admin password.')
param sqlAdminPassword string

module names './_naming.bicep' = {
  name: 'names'
  params: {
    developerUsername: developerUsername
  }
}

var databaseName = 'products'
var sqlAdminName = 'sqladmin'

module sqlServer './sqlServer.bicep' = {
  name: 'sqlServer'
  params: {
    sqlServerName: names.outputs.sqlServerName
    sqlAdminUsername: sqlAdminName
    sqlAdminPassword: sqlAdminPassword
    sqlDatabaseName: databaseName
  }
}

module webApp './webapp.bicep' = {
  name: 'webApp'
  params: {
    webAppName: names.outputs.webappName
    appSettings: [
      {
        name: 'SQL_CONNECTION_STRING'
        value: 'Server=${sqlServer.outputs.sqlServerFqdn};Database=${databaseName};User Id=${sqlAdminName};Password=${sqlAdminPassword};'
      }
    ]
  }
}
