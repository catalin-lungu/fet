
The project is organized as follows:

MainWindow.xaml => represents the starting point of the application

ManageDatabase => file that contains function for interracting with database
(export and import)
Also for this purpose exist two windows SQLConnectionImportWindow.xaml and
SqlConnectionWindow which get a sql connection string from user input.

Manage File contains functions to save timetable data from a .fet file.
In App.xaml.cs exists a boolean property DataHasChanged which is used 
in order to see if some data has been modified through data menu before exit.

OpenData contains functions to load timetable from a .fet file.

To view timetable exists:
TeachersTimetable, StudentsTimetable and RoomsTimetable

The Data folder contains classes that represents entities in timetable
and also a special class timetable that contains all sort of objects used
in timetable and also a class that represents slots for that timetable.
The algorithm (existing in ManageTimetable) use all information
contained by timetable and schedule all activities in that special subclass of
timetable that represents the slots of time. 

Data UI contains all the windows which could be opened from data menu and used for
interracting with data from timetable instance.
The windows for constraints are saved in SpaceConstraintsUI and TimeConstraintsUI 
folders.

In the ManageTimetable you will find the algorithm and if you want a possibility to schedule activities randomly.