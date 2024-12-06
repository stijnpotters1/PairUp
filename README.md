# PairUp
This is a application that allows users to search for activities based on their location. This is done by offering users the option in the frontend to filter by categories such as: playground, zoo, parks, art galleries and historical locations, whereby it is also possible to select a radius of kilometers in which to search. The backend to which api requests can be sent is connected to a database. This backend also has a web scraping functionality that can periodically populate the database when enabled, updating or enriching the activities that can be retrieved by the API.

## Development Setup
### Note:  
 - Make sure you have installed .NET 8.0 SDK for the backend.
 - Make sure you have installed npm and node for the frontend.
 - Make sure you have installed PostgreSQL and PgAdmin to make a database.

1. Clone the repository.
2. Make a Postgresql database in PgAdmin

### Backend
3. Open your IDE that is compatible with .NET
4. Open the backend project in here
5. Make a .env file locally int the root directory of the PairUpApi project
6. Paste your connection string in here, that looks like: ```DB_CONNECTION_STRING="Server=database_server;Port=database_port;Database=database_name;Username=database_user;Password=database_password"```
7. Open the terminal
8. Execute ```dotnet ef database update --project .\PairUpInfrastructure --startup-project .\PairUpApi```
9. Run the backend by clicking on the run configuration: ```PairUpApi:https```
 
### Frontend
10. Open your IDE that is compatible with React
11. Open the frontend project in here
12. Open the terminal
13. Execute ```npm install```
14. Execute ```npm run dev```

### Completed?
Once you are completed you can reach out to your local hosted application by browsing to ```http://localhost:5173``` 
