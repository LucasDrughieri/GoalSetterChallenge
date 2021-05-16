# GoalSetterChallenge

## How to build

* Install the latest .NET 5 SDK
* Install the lastest ASP.NET Runtime 5
* Install Git
* Clone this repo
* Run dotnet restore
* Run dotnet build

## How to run tests

* Run dotnet test Tests

## How to run

* cd Web/
* Go to appsettings.json and change "LUCAS2\\SQLEXPRESS" for your connection

"ConnectionStrings": {
    "Default": "Server=LUCAS2\\SQLEXPRESS;Database=GoalSetterChallenge;Trusted_Connection=True"
},

* Run dotnet run

## Documentation

* Swagger documentation on route -> /swagger/index.html
* There is a postman collection 'GoalSetter.postman_collection' on root's solution 