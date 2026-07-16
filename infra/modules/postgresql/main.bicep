param location string = resourceGroup().location
param environment string = 'dev'
param serverName string = 'psql-riskmanagement-${environment}'
param dbName string = 'riskmanagement'
param subnetId string
param administratorLogin string
@secure()
param administratorLoginPassword string

resource flexibleServer 'Microsoft.DBforPostgreSQL/flexibleServers@2023-12-01-preview' = {
  name: serverName
  location: location
  sku: {
    name: 'Standard_D2s_v3'
    tier: 'GeneralPurpose'
  }
  properties: {
    version: '16'
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Enabled'
    }
    highAvailability: {
      mode: 'ZoneRedundant'
    }
    auth: {
      passwordAuth: 'Enabled'
      activeDirectoryAuth: 'Enabled'
    }
    network: {
      delegatedSubnetId: subnetId
      privateDnsZoneArmResourceId: privateDnsZone.id
    }
  }
}

resource database 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2023-12-01-preview' = {
  parent: flexibleServer
  name: dbName
}

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
  properties: {}
}

output serverFqdn string = flexibleServer.properties.fullyQualifiedDomainName
output serverId string = flexibleServer.id
