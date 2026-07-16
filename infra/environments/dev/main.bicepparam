param location string = 'eastus'
param environment string = 'dev'

module vnet '../modules/vnet/main.bicep' = {
  name: 'deploy-vnet'
  params: {
    location: location
    environment: environment
  }
}

module postgresql '../modules/postgresql/main.bicep' = {
  name: 'deploy-postgresql'
  params: {
    location: location
    environment: environment
    subnetId: vnet.outputs.databaseSubnetId
    administratorLogin: 'riskadmin'
    administratorLoginPassword: 'placeholder-will-be-replaced-by-keyvault'
  }
}

output vnetId string = vnet.outputs.vnetId
output apiSubnetId string = vnet.outputs.apiSubnetId
output dbServerFqdn string = postgresql.outputs.serverFqdn
