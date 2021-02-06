# BeerNetApi
API allows to create site associating beer enthusiasts, register user accounts, read informations about breweries, its beers and share thoughts about them.

## Main technologies
- ASP.NET Core 3.1
- Entity Framework Core
- MS SQL
***
## Authorization
Some actions require authorization. To access restricted actions (like rating beers) you have to register your account using register endpoint:
~~~
[POST] /api/Authenticate/register
~~~
Get JWT(JSON web token) by using login endpoint:
~~~
[POST] /api/Authenticate/login
~~~
And finally place it in the header of your HTTP request:

|Key |Value
-|-
|authorization    |bearer <token>

***
## Sample accounts:
|Login |Password |Role
|-|-|-|
|user|User1.|User
|admin|Admin1.|Admin
***
## Hosting
Application is currently hosted in Microsoft Azure and is available here:
http://beernetapi.azurewebsites.net

API documentation was created with swagger and can be read at:
https://beernetapi.azurewebsites.net/swagger

