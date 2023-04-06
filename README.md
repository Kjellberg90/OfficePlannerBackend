# Epiroc Office Planner

## About the Project

The Application Epiroc-Office-Planner is developed as a LIA-project by students from Yrkeshögskolan SKY.

The application itself is meant to make it easier for the users of the application where they are booked for a specific day or week.

Tha application also contains an admin page where an admin can add groups/rooms and set up a long term schedule for where the different teams are booked.

### Reqirements

This project is works in tandem with epiroc-office-planner which is the frontend for this project. For information about the frontend read the readme for the frontend projec. 

### Installation

- Clone the project to Visual Studio
- Run the SKY_Backend.sln file

### Deployment

- Make sure your role is activeted in Azure Portal
- Right click SKY_Backend in the solution explorer and select publish
- Add a publish profile
- Select Azure as a target
- Select Azure App Service as a specific target
- Select epirox-office-planner-backend as App Service
- Check the box skip this step when selecting API Management
- Select Publish (genereate pubxml file) when selecting deployment type
- When the setup is done you should be ready to deploy by clicking on the Publish button

Further information about deployment for deployment of this project can be read about here:
https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=netframework48&pivots=development-environment-vs

## Adding Users, Resetting Data and Swagger

### Swagger

Swagger can be used to add users and reset the database to its default state. Swaggar is by default set to not be accesible for production.

In order to activate Swagger for production follow these steps:

- Open program.cs
- Comment out line 83 `if (app.Environment.IsDevelopment())`
- Save the file
- Deploy the application

Your shold no be able to visit https://epiroc-office-planner-backend.azurewebsites.net/swagger/index.html where you can add users and reset the database.
Remember to make sure to uncomment line 83 `if (app.Environment.IsDevelopment())` in program.cs and deploy again to hide swagger.

### Adding users

Adding users can be done by activating swagger acoording to instructions above and creating users with the API endpoints.
Since tha application is only supposed to be have one user and one admin account, the endpoints for userregister and adminregister will be disabled.

In order to register a new user:

- Activate swagger according to instructions above
- Go to UserCOntroller and outcomment either userregister or adminregister
- Save and deploy the application according

You shoyld now be able to register a new user in  an endpoint at https://epiroc-office-planner-backend.azurewebsites.net/swagger/index.html.
Remember to deactivate swagger and the endpoints when finished.

### Resetting database

If there is a need to reset the database to its original state it can be done with admincredentials by doing the following steps:

- Activate Swagger according to instructions above
- Login with the admincredentials in the login endpoint
- Copy your token
- Scroll up the page and click the autourize button
- In the field write "bearer token", token being the string you copied earlier
- You should now be logged in as an admin and can reset the database by using the refresh endpoint

Important to note is that the reset database clears all users and you will have to register new users in order to log in again.

Remember to deactivate swagger when finsihed.
