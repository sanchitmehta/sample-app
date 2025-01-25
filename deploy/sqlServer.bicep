param sqlServerName string
param sqlAdminUsername string
@secure()
param sqlAdminPassword string
param sqlDatabaseName string
param location string = resourceGroup().location

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
    name: sqlServerName
    location: location
    properties: {
        administratorLogin: sqlAdminUsername
        administratorLoginPassword: sqlAdminPassword
    }  
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-02-01-preview' = {
    parent: sqlServer
    name: sqlDatabaseName
    location: location
    properties: {
        collation: 'SQL_Latin1_General_CP1_CI_AS'
        maxSizeBytes: 2147483648
        sampleName: 'AdventureWorksLT'
    }
    sku: {
        name: 'S0'
        tier: 'Standard'
        capacity: 10
    }
}

output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
