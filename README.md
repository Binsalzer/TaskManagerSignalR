This web application is designed to keep track of tasks that need to be completed.
The user signs up for an account, logs in and can then add a task in the Home page.
Once added, it will  be added to the grid on the bottom part of the home page listing all of the added tasks.
Each tasks will have have a button next to it with the words "I'm doing this one".
When clicked, the text will change to "I'm done" for the user that selected it, and "{name of user} is doing this" for all other users.
When the user clicks "I'm done", the task will disappear from the grid.
The status of each task will be updated in real time for everyone without having to refresh, using SignalR.

Technologies used for this project:
Backend done using C# with Entity Framework and Sql Server as the database.
Frontend done using React.
SignalR used to update in real time.
