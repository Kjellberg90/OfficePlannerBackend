# Office Planner

## About the Project

The Application Office-Planner is developed as a LIA-project by students from Yrkeshögskolan SKY.

The application itself is meant to make it easier for the users of the application where they are booked for a specific day or week.

Tha application also contains an admin page where an admin can add groups/rooms and set up a long term schedule for where the different teams are booked.

### Reqirements

This project is works in tandem with epiroc-office-planner which is the frontend for this project. For information about the frontend read the readme for the frontend project see https://github.com/Kjellberg90/OfficePlannerFrontend. 

### Installation

- Clone the project to Visual Studio
- Run the SKY_Backend.sln file

## Adding Users, Resetting Data and Swagger

### Swagger

Swagger can be used to add users and reset the database to its default state.

In order to activate Swagger for production follow these steps:

- Open program.cs
- Comment out line 83 `if (app.Environment.IsDevelopment())`
- Save the file
- Deploy the application

### Adding users

- Go to UserController and outcomment either userregister or adminregister
- Save and deploy the application according

### Resetting database

If there is a need to reset the database to its original state it can be done with admincredentials by doing the following steps:

- Login with the admincredentials in the login endpoint
- Copy your token
- Scroll up the page and click the autourize button
- In the field write "bearer token", token being the string you copied earlier
- You should now be logged in as an admin and can reset the database by using the refresh endpoint

Important to note is that the reset database clears all users and you will have to register new users in order to log in again.
