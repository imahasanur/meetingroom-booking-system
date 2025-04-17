# Meeting Room Booking System 

It is a Asp Net Core MVC (v 8.0) Project. Project is designed following Domain Driven Design(DDD) & Clean Architecture(CL).

## Features

Functional:

User

1. View available & booked meetings in a scheduler.
2. Create, Update meeting through drag and resize event in scheduler for any time slot.
3. Create Booking request.
4. Update , Delete own pending request.
5. View all of his meetings assigned as Host, Guest.
6. View all rooms with details.
7. Users first time after login redireted to change passwrod page.

Admin

8. View available & booked meetings in a scheduler.
9. View meeting Room details by scanning QR code.
10. Create, Update meeting through drag and resize event in scheduler for any time slot.
11. Create Booking request
12. Update , Delete all pending request
13. View all meetings assigned as Guest, pending & approved requests.
14. Create, Update, Delete room.
15. Modify user role.
16. Update meeting time limit

Before Login any user will see available rooms rooms and rooms details by scanning QR code 


Non Functional:
1. Added forms client side and server side validations.
2. Integrated serilog for logging and use exception handling for fault tolerance.
3. Integrated Adminlte v3, bootstrap and datatables to make the site responsible and more useable.
4. Used entity framework, repository and unit of work pattern to make the project robust.
5. Used bootstrap, fontawesome icon to make the application user friendly.
6. All migrations added including seed data to make the project maintainable.
7. Used Daypilot Scheduler to create, update by drag and resize into a day time portion booking scheduler and view booking.


## Used Technology
 FrontEnd: Html, Bootstrap, Fontawesome, Javascript, Daypilot Scheduler 
 
 Server side: C#, Asp Net Core MVC (v8.0), Entity FrameWork Core(v8), Daypilot Scheduler

 Design Method: Domain Driven Design, Clean Architecture
 
 Database : SQL SERVER
 
 Tool : Visual Studio

## Project Document 
project core workflow[https://drive.google.com/file/d/1CDQ3pbxDHrCRcSIO7Z5Hnre1AG62zVNu/view?usp=sharing)

Db Backup file[https://drive.google.com/file/d/17qD_rUtAIaGM82OWFyguGXY1-rNE213A/view?usp=sharing)

## Need to Run the project
 1. Download Packages from NUGET Package Manager
 2. Open project in Visual Studio Build & Run
 3. Dot Net Core MVC (v8)
 4. SQL server


## Connect with me
linkedin profile[linkedin.com/ahasanur-rahman](https://www.linkedin.com/in/ahasanur-rahman-a10925202/)


