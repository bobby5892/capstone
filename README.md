# PeerIt
LCC Programming Capstone - Team 42

## Frameworks and Libraries being used:
  ASP .net is used for the server-side code.
  React.js is used for the client-side code, with a Webix User Interface library.
  The database is generated using Entity Framework Core.
  
  webix information: https://webix.com/
  
This project is called PeerIt: A Single-Page Peer Review Application. The objective of the application is to provide an easy-to-use Peer Review framework that can be used by universities. This application is built (for now) to be deployed on a server-by-server basis, meaning that this is not a stand-alone application for public use on it's own domain.

There are 4 roles that a user can have: "Admin", "Instructor", "Student" and "Invited User". These roles function as follows:

  ### An Admin is the "Super User" for the group.
  
  They can:
    -Manage user accounts
    -Set up an email server for automated messaging
    -Perform all of the tasks that an instructor or student can perform.
    
  They have access to all of the information for the server they administrate.
  
  ### An Instructor is a teacher for a course, or multiple courses.
  
  They can:
    -Create courses
    -Create course assignments
    -Create rubrics
    -Send email invitations to join a course
    -Write and view peer reviews
    
  They have access to all of the data regarding the courses they teach.
  
  ### A Student is a person participating in a course.
  
  They can:
    -View course materials
    -Upload Assignments
    -View another student's assignment
    -Upload reviews
    -View reviews
    -Comment on an assignment submission
    
  They have access to all of the data regarding their work, and work from other students that they are allowed to view.
  
  ### An Invited User is a person who has been invited, via email from an instructor, to a course and still needs to create an account.

It serves as a sort of placeholder for an account that has yet to be fully created. They do not have access to any data, and can only access the means to complete their account creation.

## Setting up the development environment

In order to set up your development environment, there are a few things that need to be done.
First, there is a entity framework development database that needs to be created in order to create the Admin account that is included in the migration files. In order to do that, you will need to have the entity framework command line installed, navigate to the subfolder "capstone/Peerit/peerit" in your command line, and run the command "dotnet ef database update".

Within the migrations files, 