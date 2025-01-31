# PairUp
This is a application that allows users to search for activities based on their location. This is done by offering users the option in the frontend to filter by categories such as: playground, zoo, parks, art galleries and historical locations, whereby it is also possible to select a radius of kilometers in which to search. In addition, users can also create accounts on the application after which they can like activities so that they can easily find them back later on. The backend to which api requests can be sent is connected to a database. This backend also has a web scraping functionality that can periodically populate the database when enabled, updating or enriching the activities that can be retrieved by the API.

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
6. Paste your enviroment variables in here, that looks like the following format: ```DB_CONNECTION_STRING="Server={db_host};Port={db_port};Database={db_name};Username={db_username};Password={db_password}"
JWT_SECRET_KEY="{jwt_secret}"
JWT_ISSUER="https://localhost:7247"
JWT_AUDIENCE="https://localhost:7247"
SEEDER_ADMIN_FIRSTNAME="admin_account_firstname"
SEEDER_ADMIN_LASTNAME="{admin_account_lastname}"
SEEDER_ADMIN_EMAIL="{admin_account_email}"
SEEDER_ADMIN_PASSWORD="{admin_account_password}"
SEEDER_ROLES="Admin,User"```
8. Open the terminal
9. Execute ```dotnet ef database update --project .\PairUpInfrastructure --startup-project .\PairUpApi```
10. Run the backend by clicking on the run configuration: ```PairUpApi:https```
 
### Frontend
10. Open your IDE that is compatible with React
11. Open the frontend project in here
12. Open the terminal
13. Execute ```npm install```
14. Execute ```npm run dev```

### Completed?
Once you are completed you can reach out to your local hosted application by browsing to ```http://localhost:5173``` 
Note: when you start the application, the database only starts to fill using the scraper, so not all activities are already in the database from the start.
