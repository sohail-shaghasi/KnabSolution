## KnabSolution
![currencyExchangeDiagram](https://user-images.githubusercontent.com/10161791/163439779-7ebb355c-09fd-4952-a62a-8803d9d3b22b.jpg)

In This project, Clean architecture is followed to be able to easily Unit/Integration test all the involved layers. Clean architecture lets infrastructure and implementation details depend on the Application Core.

Elaboration of the application layers are as follows:

### Knab.Exchange.CoinMarketCap.ApiClient (Infrastructure layer)
### Knab.Exchange.Exchangerates.ApiClient (Infrastructure layer)

This layer is mapped to the infrastructure layer of the clean architecture. It is primarily concerned about the concrete implementations of the ApiClients. 
spcifically the http interactions with 3rd party APIs such as CoinMarketCap and Exchangerates api.

### Knab.Exchange.Api (Application layer)
This layer is receiving requests in the api controller class through its rest api endpoint. 
Swagger ui is the application/client.

#### The localhost Swagger URL is: http://localhost:5044/swagger 
