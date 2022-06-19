# INTRODUCTION
This is a solution by ***Yuvraj Sengar*** to a coding challenge presented by **Everest Engineering** to build a miniature courier service system.
<br/>
<br/>

# REQUIREMENTS
Kiki has opened a courier service to deliver packages. He has invested in N no. of vehicles and has driver partners to drive each vehicle & deliver packages. The requirement is to build a command line application which can solve the following challanges for Kiki:
<br/>

- Challenge 1: **Estimate the total delivery cost** of each package with
an offer code (if applicable).
- Challenge 1: **Calculate the estimated delivery time** for every package by maximizing no. of packages in every shipment.
<br/>
<br/>

# INSTALLATION
Please follow the following steps:
- One will need to install .Net Core 3.1 from [Microsoft's Web Site](https://dotnet.microsoft.com/en-us/download/dotnet/3.1).
 - Clone the repository.
 - Using any C# Editor - Debug or Run the Project "***Everest.Engineering.ConsoleApp***".
 - Or using sc.exe - Install the App as a windows service in your PC. [Visit ths for help](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create)
 - Or You can publsh this app in your System. For help [Visit this article](https://www.c-sharpcorner.com/article/how-to-create-exe-for-net-c) or [this one from Microsft](https://docs.microsoft.com/en-us/dotnet/core/deploying).
<br/>
<br/>

# CONFIGURATION

The app can be run in three different Environments.
- **Development** (File -> *appsettings.Development.json*)
- **PreProduction** (File -> *appsettings.PreProduction.json*)
- **Production** (File -> *appsettings.Production.json*)
<br/>

Please set an Environment variable "DOTNET_ENVIRONMENT" to either of the 3 values mentioned above. The default is Development.
For further help in understanding, Please visit [Microsoft's Explaination](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-3.1).
<br/>

Each configuration file consists of 3 sections, for ***App Settings***, ***Data to Seed*** and ***Test Scenarios***.
```json
{
  "AppSettings": {..},
  "DataToSeed": {..},
  "TestScenarios": {..}
}
```
<br/>

## AppSettings
- EstimationMode: If `Cost`, will estimate only cost of delivery (Problem 01). else if `Time`", will estimate both, cost and time (Problem 02). (Default is `Cost`).
- RunSampleTestCase: If `true`, will not wait for user to input test case details, instead, will run the sample test case (the data ill be picked up from `TestScenarios` Section). Else, `false`. (Default is `false`).
- IsLoggingEnabled: If `true`, will log details which could prove fruitful while development. Else, `false`. (Default is `false`).
```json
"AppSettings": {
    "EstimationMode": "Time", // Cost, Time
    "RunSampleTestCase": false,
    "IsLoggingEnabled": false
  },
"DataToSeed": {..},
"TestScenarios": {..}
```
<br/>

## DataToSeed
This section holds data that needs to be seeded once for the application to work. Contains an `Offers` object which helps define all the offers in the system. An example is shown below:
```json
"AppSettings": {..},
"DataToSeed": {
  "Offers": [
    {
      "Name": "OfferCode",
      "DiscountPercentage": 10,
      "Criteria": {
        "Distance": {
          "Minimum": 0,
          "Maximum": 199
        },
        "Weight": {
          "Minimum": 70,
          "Maximum": 200
        }
      },
      "Id": "8a270643-7a43-442a-ac6c-2c07350c54a5", // Type -> GUID
      "CreatedAt": "2022-06-19T22:55:28.6661882+05:30", // Date-format: YYYY-MM-DDTHH:mm:ss+TZ
      "LastModifiedAt": "2022-06-19T22:55:28.6661882+05:30" // Date-format: YYYY-MM-DDTHH:mm:ss+TZ
    }
    ...
  ]
},
"TestScenarios": {..}
```
<br/>

## TestScenarios
This section holds data that will be used as sample case data. If `AppSettings.RunSampleTestCase` is `true`, then teh data in `TestScenarios` scection will come into play. The user will not have to give the inputs manually, intead, the data will be taken from this section of the configuration file. 

### For Cost Estimation
If `AppSettings.RunSampleTestCase` is `true` and `AppSettings.EstimationMode` is `Cost`, then the value help by section `CostEstimateInput` will be usd to provide the sample input data. An example of this is shown below:
```json
"AppSettings": {
  "EstimationMode": "Cost",
  "RunSampleTestCase": true,
  ...
},
"DataToSeed": {..},
"TestScenarios": {
  "CostEstimateInput": {
    "BaseDeliveryCost": 100,
    "NumberOfPackages": 3,
    "Packages": [
      {
        "Id": "PKG1",
        "Weight": 5,
        "Distance": 5,
        "OfferCode": "OFR001",
        "IsDelivered": false,
        "IsAllreadySelected": false
      },
      {
        "Id": "PKG2",
        "Weight": 15,
        "Distance": 5,
        "OfferCode": "OFR002",
        "IsDelivered": false,
        "IsAllreadySelected": false
      },
      {
        "Id": "PKG3",
        "Weight": 10,
        "Distance": 100,
        "OfferCode": "OFR003",
        "IsDelivered": false,
        "IsAllreadySelected": false
      }
    ]
  },
  "TimeEstimateInput": {..}
}
```
<br/>

### For Time Estimation
If `AppSettings.RunSampleTestCase` is `true` and `AppSettings.EstimationMode` is `Time`, then the value help by section `TimeEstimateInput` will be usd to provide the sample input data. An example of this is shown below:
```json
"AppSettings": {
    "EstimationMode": "Time",
    "RunSampleTestCase": true,
    ...
},
"DataToSeed": {..},
"TestScenarios": {
  "CostEstimateInput": {..},
  "TimeEstimateInput":     "TimeEstimateInput": {
    "Vehicles": {
      "TotalCount": 2,
      "MaxSpeed": 70,
      "MaxWeightCapacity": 200
    },
    "BaseDeliveryCost": 100,
    "NumberOfPackages": 5,
    "Packages": [
      {
        "Id": "PKG1",
        "Weight": 50,
        "Distance": 30,
        "OfferCode": "OFR001",
        "IsDelivered": false,
        "IsAllreadySelected": false
      },
      {
        "Id": "PKG2",
        "Weight": 75,
        "Distance": 125,
        "OfferCode": "OFR008",
        "IsDelivered": false,
        "IsAllreadySelected": false
      },
      {
        "Id": "PKG3",
        "Weight": 175,
        "Distance": 100,
        "OfferCode": "OFR003",
        "IsDelivered": false,
        "IsAllreadySelected": false
      },
      {
        "Id": "PKG4",
        "Weight": 110,
        "Distance": 60,
        "OfferCode": "OFR002",
        "IsDelivered": false,
        "IsAllreadySelected": false
      },
      {
        "Id": "PKG5",
        "Weight": 155,
        "Distance": 95,
        "OfferCode": "NA",
        "IsDelivered": false,
        "IsAllreadySelected": false
      }
    ]
  }
}
```
<br/>
<br/>

# MAINTAINERS
 * Yuvraj Sengar - https://github.com/yuvisengar
 <br/>