TaskManager is an application that allows users to create, update and delete tasks.

## 1. How to run the app:
1. Start the backend
Load the TaskManagerApp.sln solution file in Visual Studio and start the app.
This will also do the build step. This way, the backend is started. 
A swagger interface will open at this address: https://localhost:7014/swagger/index.html
![Alt text](media/swagger.png)
From this page, the REST API is available. The user can perform CRUD operations.

2. Start the frontend
Install npm, if not already installed: npm install
Go to the frontend folder and start the server: npm start
This will start the frontend, that will connect to the previously started backend.
Add, Edit and Delete are available as commands; all existing tasks are shown.
![Alt text](media/frontend.png)


## 2. Code design