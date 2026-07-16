param location string = resourceGroup().location
param environment string = 'dev'
param vnetName string = 'vnet-riskmanagement-${environment}'
param addressPrefix string = '10.0.0.0/16'

resource virtualNetwork 'Microsoft.Network/virtualNetworks@2023-09-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [addressPrefix]
    }
    subnets: [
      {
        name: 'snet-api-${environment}'
        properties: {
          addressPrefix: '10.0.1.0/24'
          networkSecurityGroup: {
            id: nsgApi.id
          }
        }
      }
      {
        name: 'snet-database-${environment}'
        properties: {
          addressPrefix: '10.0.2.0/24'
          networkSecurityGroup: {
            id: nsgDatabase.id
          }
          serviceEndpoints: [
            {
              service: 'Microsoft.DBforPostgreSQL/flexibleServers'
            }
          ]
        }
      }
      {
        name: 'snet-ai-${environment}'
        properties: {
          addressPrefix: '10.0.3.0/24'
          networkSecurityGroup: {
            id: nsgAi.id
          }
        }
      }
    ]
  }
}

resource nsgApi 'Microsoft.Network/networkSecurityGroups@2023-09-01' = {
  name: 'nsg-api-${environment}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowHttps'
        properties: {
          priority: 100
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'AllowHttp'
        properties: {
          priority: 101
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '80'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}

resource nsgDatabase 'Microsoft.Network/networkSecurityGroups@2023-09-01' = {
  name: 'nsg-database-${environment}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowPostgreSQL'
        properties: {
          priority: 100
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '5432'
          sourceAddressPrefix: '10.0.1.0/24'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}

resource nsgAi 'Microsoft.Network/networkSecurityGroups@2023-09-01' = {
  name: 'nsg-ai-${environment}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowHttpsOutbound'
        properties: {
          priority: 100
          direction: 'Outbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'AzureOpenAI'
        }
      }
    ]
  }
}

output vnetId string = virtualNetwork.id
output apiSubnetId string = virtualNetwork.properties.subnets[0].id
output databaseSubnetId string = virtualNetwork.properties.subnets[1].id
output aiSubnetId string = virtualNetwork.properties.subnets[2].id
