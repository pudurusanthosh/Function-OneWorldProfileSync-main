# Introduction 
This project runs the comparision between the SOLAR data and Amadeus.

# Supported comparisions
This supports

1. Upgrade
2. Downgrade 

# Configuration

1. Create a file called config.json
1. Fille the config .json with the below data
    ```
    [
        {
            "name": "amadeusBaseUri",
            "value": "https://nodeA3.production.webservices.amadeus.com/1ASIWPASAS",
            "slotSetting": false
        },
        {
            "name": "amadeusPassword",
            "value": "",
            "slotSetting": false
        },
        {
            "name": "originator",
            "value": "",
            "slotSetting": false
        },
        {
            "name": "sourceOffice",
            "value": "",
            "slotSetting": false
        },
        {
            "name": "wsaTo",
            "value": "",
            "slotSetting": false
        },
        {
            "name": "dbUserName",
            "value": "soreport",
            "slotSetting": false
        },
        {
            "name": "dbUserPassword",
            "value": "",
            "slotSetting": false
        },
        {
            "name": "databaseSource",
            "value": "seavvsbl8db01.alaskaair.com:1521/solprod",
            "slotSetting": false
        },
        {
            "name": "OperationUnderTest",
            "value": "Upgrade"
        }
    ]
    ```
1. Fill the config file with production values/skip this process and set them as environment values.
1. Change current directory to that directory where config file is downloaded
1. Run the below commands from Package Manager Console
```
$fields=Get-Content .\config.json | ConvertFrom-Json
foreach($field in $fields){ [Environment]::SetEnvironmentVariable($field.Name,$field.Value) }
```

# Outputs:
Output files will be created in the below directory
```
Function-OneWorldProfileSync\AlaskaAir.MileagePlan.OneWorldProfileSync.Function\AmadeusDataTally
```