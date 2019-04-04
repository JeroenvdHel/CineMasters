# CineMasters
Theater app ASP.NET Core MVC 

This application makes use of 2 databases:
- MongoDB for domain objects like movies, shows, tickets, etc
- MSSQL database for Identity Framework

Steps to create the MSSQL database:
- Create a database in MSSQL server
- Start commandline and navigate to the projectfolder.
- Enter 'dotnet ef migrations add Initial'
- Enter 'dotnet ef database update'

Create a mongoDB database.
Enter connection parameters for both databases in the file appsettings.json


In the folder 'root'/Config are 4 json files, use these to import to Mongo collections.


