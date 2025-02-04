@minLength(0)
@maxLength(15)
@description('Developer username or custom identifier used to tell private deployments apart')
param developerUsername string

@maxLength(63)
@metadata({globallyScoped: true, validCharacters: 'alphanumericsOnly'})
@description('Unique name for the SQL Server')
param sqlServerName string = 'oa-demo-sql-${developerUsername}'

@maxLength(63)
@metadata({globallyScoped: true, validCharacters: 'alphanumericsOnly'})
@description('Unique name for the SQL Server')
param webappName string = 'oa-demo-web-${developerUsername}'

output sqlServerName string = sqlServerName
output webappName string = webappName
