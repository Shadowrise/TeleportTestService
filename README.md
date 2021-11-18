# TeleportTestService
Test Assignment

The purpose of this test assignment is to demonstrate the skills of building scalable and resilient
services.
We will assess code structure, applied patterns, solution completeness, and correctness.
Description
Build a REST service to measure distance in miles between two airports. Airports are identified by 3-
letter IATA code.
Sample call to get airport details:
GET https://places-dev.cteleport.com/airports/AMS HTTP/1.1

Requirement:
- dotnet core 2 or later has to be used.
  And it’s allowed to use 3-rd party components.
- please, provide final solution in ZIP format
## Implementation
Solution consist of 5 projects built on net6.0.
### AirportsService.Api
Web Application which starts on http://localhost:5049 and https://localhost:7049 urls by default.
Provides one endpoint ```/airports/distance``` which expects two query parameters ```code1``` and ```code2```. 
The parameters are IATA codes of the airports, between which we need to calculate the distance.

Request URL example https://localhost:7049/airports/distance?code1=LED&code2=BKK

The service also provides swagger ui https://localhost:7049/swagger/index.html for testing purposes.

### AirportsService.Business
Stores business logic. The logic is divided into pieces: queries and commands (according to the CQRS pattern).

### AirportsService.Common
Stores common code which used all across the solution, for example exception related stuff.

### AirportsService.Tests
Unit tests.

### TeleportPlaces.Client
RestEase wrappers for external service which provides airports data (https://places-dev.cteleport.com)