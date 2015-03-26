using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using FET.Data;

namespace FET
{   
    /// <summary>
    /// class used for loading a .fet file in programm and convert each xml element in a c# object equivalent
    /// </summary>
    class OpenData 
    {
        /// <summary>
        /// method which reads a .fet file and creates c# objects 
        /// if doesn't exist any instance of timetable in the programm this method will create one
        /// </summary>
        /// <param name="path"> path is the way to the .fet file </param>
 
        public static void Load(String path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            Timetable timetable = Timetable.GetInstance();
            timetable.Clear();

            timetable.InstitutionName = doc.SelectSingleNode("/fet/Institution_Name").InnerText;
            timetable.Comments = doc.SelectSingleNode("/fet/Comments").InnerText;
 
            LoadHoursList(timetable, doc);

            // Days List
            LoadDaysList(timetable, doc);
                
            // Students List
            LoadStudentsList(timetable, doc);

            // Teachers List
            LoadTeachersList(timetable, doc);
 
            // Subject List
            LoadSubjectList(timetable, doc);

            // Activity_Tags_List
            LoadActivityTagsList(timetable, doc);

           
            // Buildings List
            LoadBuildingsList(timetable, doc);

            // Rooms List
            LoadRoomsList(timetable, doc);

            // Activities List
            LoadActivitiesList(timetable, doc);

            LoadTimeConstraints(timetable, doc);

            LoadSpaceConstraints(timetable, doc);

           

            
        }
        /// <summary>
        /// loads hours from xml in a string list
        /// </summary>
        /// <param name="timetable"> instance of timetable</param>
        /// <param name="doc"> .fet document</param>
        private static void LoadHoursList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode hour in doc.SelectSingleNode("/fet/Hours_List").ChildNodes)
            {
                if (!hour.Name.Equals("Name"))
                {
                    continue;
                }
                timetable.HoursList.Add(hour.InnerText);
            }
        }

        /// <summary>
        /// loads days from xml in a string list
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadDaysList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode day in doc.SelectSingleNode("/fet/Days_List").ChildNodes)
            {
                if (!day.Name.Equals("Name"))
                {
                    continue;
                }
                timetable.DaysList.Add(day.InnerText);
            }
        }

        /// <summary>
        /// loads students in a tree format( every year contains groups, every group contains subgroup)
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadStudentsList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode year in doc.SelectSingleNode("/fet/Students_List").ChildNodes)
            {
                Year y = new Year();
                y.Name = year["Name"].InnerText;
                y.NumberOfStudents = Convert.ToInt32(year["Number_of_Students"].InnerText);

                foreach (XmlNode group in year.ChildNodes)
                {
                    if (!group.Name.Equals("Group"))
                    {
                        continue;
                    }

                    Group g = new Group(y.Name);
                    g.Name = group["Name"].InnerText;
                    g.YearName = y.Name;
                    g.NumberOfStudents = Convert.ToInt32(group["Number_of_Students"].InnerText);

                    foreach (XmlNode subgroup in group.ChildNodes)
                    {
                        if (!subgroup.Name.Equals("Subgroup"))
                        {
                            continue;
                        }

                        Subgroup subg = new Subgroup(g.Name);
                        subg.Name = subgroup["Name"].InnerText;
                        subg.GroupName = g.Name;
                        subg.NumberOfStudents = Convert.ToInt32(subgroup["Number_of_Students"].InnerText);

                        g.Subgroups.Add(subg);
                    }

                    y.Groups.Add(g);
                }
                timetable.ClassList.Add(y);
            }
        }
        
        /// <summary>
        /// loads teachers and create objects for each of them
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>

        private static void LoadTeachersList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode teacher in doc.SelectSingleNode("/fet/Teachers_List").ChildNodes)
            {
                timetable.TeacherList.Add(new Teacher(teacher.InnerText));
            }
        }

        /// <summary>
        /// loads subjects
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadSubjectList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode subject in doc.SelectSingleNode("/fet/Subjects_List").ChildNodes)
            {
                timetable.SubjectList.Add(new Subject(subject.InnerText));
            }
        }

        /// <summary>
        /// loads activity tags
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadActivityTagsList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode activityTag in doc.SelectSingleNode("/fet/Activity_Tags_List").ChildNodes)
            {
                timetable.ActivitiyTagsList.Add(new ActivityTag(activityTag.InnerText));
            }
        }

        /// <summary>
        /// loads activities 
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadActivitiesList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode activity in doc.SelectSingleNode("/fet/Activities_List").ChildNodes)
            {
                Activity activityObject = new Activity();
                if (activity["Teacher"] == null)
                {
                    activityObject.Teacher = new Teacher("Unknown");
                }
                else
                {
                    activityObject.Teacher = timetable.TeacherList.Find(t => t.Name == activity["Teacher"].InnerText);
                }
                activityObject.Subject = new Subject(activity["Subject"].InnerText);

                if (activity["Activity_Tag"] != null)
                {
                    activityObject.ActivityTag = new ActivityTag(activity["Activity_Tag"].InnerText);
                }

                if (activity["Students"] == null)
                {/*
                    MessageBox.Show("Warning!! An activity does not contains Students element" + Environment.NewLine
                        + "Activity Teacher: " + activityObject.Teacher.Name + "Activity subject:" + activityObject.Subject.Name
                        + "This activity cannot be added!");
                   
                  */
                    activityObject.Students = new Year();
                    continue;
                }
                else
                {
                    activityObject.Students = timetable.GetStudents( activity["Students"].InnerText );

                }
                activityObject.Duration = Convert.ToInt32(activity["Duration"].InnerText);
                activityObject.TotalDuration = Convert.ToInt32(activity["Total_Duration"].InnerText);
                activityObject.ActivityId = Convert.ToInt32(activity["Id"].InnerText);
                activityObject.ActivityGroupId = Convert.ToInt32(activity["Activity_Group_Id"].InnerText);
                activityObject.Active = Convert.ToBoolean(activity["Active"].InnerText);
                activityObject.Comments = activity["Active"].InnerText;

                timetable.ActivityList.Add(activityObject);
            }
        }

        /// <summary>
        /// loads buildings 
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadBuildingsList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode building in doc.SelectSingleNode("/fet/Buildings_List").ChildNodes)
            {
                timetable.BuildingsList.Add(new Building(building.InnerText));
            }
        }
        
        /// <summary>
        /// loads rooms
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadRoomsList(Timetable timetable, XmlDocument doc)
        {
            foreach (XmlNode room in doc.SelectSingleNode("/fet/Rooms_List").ChildNodes)
            {
                Room r = new Room();
                r.Name = room["Name"].InnerText;

                Building build = timetable.BuildingsList.Find(b => b.Name == room["Building"].InnerText);
                if (build == null)
                {
                    build = new Building(room["Building"].InnerText);
                    timetable.BuildingsList.Add(build);
                }
                r.Capacity = Convert.ToInt32(room["Capacity"].InnerText);
                r.BuildingName = build.Name;

                build.Rooms.Add(r);
                timetable.GetRoomsList().Add(r);
            }
        }

        /// <summary>
        /// loads time constraints
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadTimeConstraints(Timetable timetable, XmlDocument doc)
        {
            #region time constraints
            // Time Constraints List
            foreach (XmlNode timeContraint in doc.SelectSingleNode("/fet/Time_Constraints_List").ChildNodes)
            {
                if(timeContraint.Name.Equals("ConstraintBasicCompulsoryTime"))
                {
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintBreakTimes"))
                {
                    ConstraintBreakTimes constraint = new ConstraintBreakTimes();
                    
                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Break_Time"))
                        {
                            continue;
                        }
                        constraint.BreakTimes.Add(new TimeDayHour(subnode["Day"].InnerText, subnode["Hour"].InnerText, constraint.ConstraintBreakTimesId));
                    }

                    timetable.TimeConstraints.ConstraintsBreakTimes = constraint;
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherNotAvailableTimes"))
                {
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Not_Available_Time"))
                        {
                            continue;
                        }
                        teacher.NotAvailableTimes.Add(new TimeDayHour(subnode["Day"].InnerText, subnode["Hour"].InnerText));
                    }
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMaxDaysPerWeek"))
                {
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    teacher.MaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMaxGapsPerWeek"))
                {
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher  == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    teacher.MaxGapsPerWeek = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMaxGapsPerWeek"))
                {
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerWeek = 
                        Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMaxHoursDaily"))
                {                   
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    teacher.MaximumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMinHoursDaily"))
                {
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }
                    teacher.MinHoursDaily = new MinimHoursDaily();
                    teacher.MinHoursDaily.MinimumHoursDaily = Convert.ToInt32(timeContraint["Minimum_Hours_Daily"].InnerText);
                    teacher.MinHoursDaily.AllowEmptyDays = Convert.ToBoolean(timeContraint["Allow_Empty_Days"].InnerText);

                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintTeacherIntervalMaxDaysPerWeek"))
                {
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    teacher.TeacherIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();

                    teacher.TeacherIntervalMaxDaysPerWeek.MaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);
                    teacher.TeacherIntervalMaxDaysPerWeek.IntervalStartHour = timeContraint["Interval_Start_Hour"].InnerText;
                    teacher.TeacherIntervalMaxDaysPerWeek.IntervalEndHour = timeContraint["Interval_End_Hour"].InnerText;

                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintActivityPreferredStartingTimes"))
                {
                    ConstraintActivityPreferredStartingTimes constraint = new ConstraintActivityPreferredStartingTimes();
                   
                    constraint.ActivityId = Convert.ToInt32(timeContraint["Activity_Id"].InnerText);
                    constraint.NumberOfPreferredStartingTimes = Convert.ToInt32(timeContraint["Number_of_Preferred_Starting_Times"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Starting_Time"))
                        {
                            continue;
                        }
                        constraint.PreferredStartingTimes.Add(new TimeDayHour(subnode["Preferred_Starting_Day"].InnerText, subnode["Preferred_Starting_Hour"].InnerText));
                    }

                    timetable.TimeConstraints.ConstraintActivityPreferredStartingTimesList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesSameStartingDay"))
                {
                    ConstraintActivitiesSameStartingDay constraint = new ConstraintActivitiesSameStartingDay();
                    
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.TimeConstraints.ConstraintActivitiesSameStartingDayList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMaxGapsPerDay"))
                {
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    teacher.MaxGapsPerDay = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesPreferredTimeSlots"))
                {
                    ConstraintActivitiesPreferredTimeSlots constraint = new ConstraintActivitiesPreferredTimeSlots();
                   
                    constraint.Teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (constraint.Teacher == null)
                    {
                        constraint.Teacher = new Teacher(string.Empty);
                    }
                    constraint.StudentsName = timeContraint["Students_Name"].InnerText;
                    constraint.SubjectName = timeContraint["Subject_Name"].InnerText;
                    constraint.ActivityTagName = timeContraint["Activity_Tag_Name"].InnerText;

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Time_Slot"))
                        {
                            continue;
                        }
                        constraint.PreferredTimeSlots.Add(new TimeDayHour(subnode["Preferred_Day"].InnerText, subnode["Preferred_Hour"].InnerText));
                    }

                    timetable.TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesPreferredStartingTimes"))
                {
                    ConstraintActivitiesPreferredStartingTimes constraint = new ConstraintActivitiesPreferredStartingTimes();
                   
                    constraint.Teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (constraint.Teacher == null)
                    {
                        constraint.Teacher = new Teacher(string.Empty);
                    }

                    constraint.StudentsName = timeContraint["Students_Name"].InnerText;
                    constraint.SubjectName = timeContraint["Subject_Name"].InnerText;
                    constraint.ActivityTagName = timeContraint["Activity_Tag_Name"].InnerText;

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Starting_Time"))
                        {
                            continue;
                        }
                        constraint.PreferredStartingTime.Add(new TimeDayHour(subnode["Preferred_Starting_Day"].InnerText, subnode["Preferred_Starting_Hour"].InnerText));
                    }

                    timetable.TimeConstraints.ConstraintActivitiesPreferredStartingTimesList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTwoActivitiesConsecutive"))
                {
                    ConstraintTwoActivitiesConsecutive constraint = new ConstraintTwoActivitiesConsecutive();
                   
                    constraint.FirstActivityId = Convert.ToInt32(timeContraint["First_Activity_Id"].InnerText);
                    constraint.SecondActivityId = Convert.ToInt32(timeContraint["Second_Activity_Id"].InnerText);

                    timetable.TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsMaxHoursDaily"))
                {
                   
                    timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetMaxGapsPerWeek"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText.Trim());
                    students.MaxGapsPerWeek = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintMinDaysBetweenActivities"))
                {
                    ConstraintMinDaysBetweenActivities constraint = new ConstraintMinDaysBetweenActivities();
                   
                    constraint.ConsecutiveIfSameDay = Convert.ToBoolean(timeContraint["Consecutive_If_Same_Day"].InnerText);
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);
                    constraint.MinDays = Convert.ToInt32(timeContraint["MinDays"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.TimeConstraints.ConstraintMinDaysBetweenActivitiesList.Add(constraint);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsEarlyMaxBeginningsAtSecondHour"))
                {
                    
                    timetable.TimeConstraints.ConstraintsStudents.StudentsEarlyMaxBeginningsAtSecondHour = Convert.ToInt32(timeContraint["Max_Beginnings_At_Second_Hour"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsMaxGapsPerWeek"))
                {
                    
                    timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerWeek = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsMinHoursDaily"))
                {
                
                    timetable.TimeConstraints.ConstraintsStudents.StudentsMinHoursDaily = Convert.ToInt32(timeContraint["Minimum_Hours_Daily"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMaxGapsPerDay"))
                {
                    
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerDay = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesNotOverlapping"))
                {
                    ConstraintActivitiesNotOverlapping constraint = new ConstraintActivitiesNotOverlapping();
                   
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.TimeConstraints.ConstraintActivitiesNotOverlappingList.Add(constraint);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetMaxHoursDaily"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.MaximumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetMinHoursDaily"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.MinHoursDaily.MinimumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);
                    students.MinHoursDaily.AllowEmptyDays = Convert.ToBoolean(timeContraint["Allow_Empty_Days"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetNotAvailableTimes"))
                {
                   
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText.Trim());

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Not_Available_Time"))
                        {
                            continue;
                        }
                        students.NotAvailableTimes.Add(new TimeDayHour(subnode["Day"].InnerText, subnode["Hour"].InnerText));

                    }
                   
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.MaxBeginningsAtSecondHour = Convert.ToInt32(timeContraint["Max_Beginnings_At_Second_Hour"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivityEndsStudentsDay"))
                {
                    ConstraintActivityEndsStudentsDay constraint = new ConstraintActivityEndsStudentsDay();
                    
                    constraint.ActivityId = Convert.ToInt32(timeContraint["Activity_Id"].InnerText);

                    timetable.TimeConstraints.ConstraintActivityEndsStudentsDayList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivityPreferredStartingTime"))
                {
                    ConstraintActivityPreferredStartingTime constraint = new ConstraintActivityPreferredStartingTime();
                    
                    constraint.ActivityId = Convert.ToInt32(timeContraint["Activity_Id"].InnerText);
                    constraint.PreferredDay = timeContraint["Preferred_Day"].InnerText;
                    constraint.PreferredHour = timeContraint["Preferred_Hour"].InnerText;
                    constraint.PermanentlyLocked = Convert.ToBoolean(timeContraint["Permanently_Locked"].InnerText);

                    timetable.TimeConstraints.ConstraintActivityPreferredStartingTimeList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMaxHoursDaily"))
                {
                   
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMinHoursDaily"))
                {
                   
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMinHoursDaily = Convert.ToInt32(timeContraint["Minimum_Hours_Daily"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivityPreferredTimeSlots"))
                {
                    ConstraintActivityPreferredTimeSlots constraint = new ConstraintActivityPreferredTimeSlots();
                   
                    constraint.ActivityId = Convert.ToInt32(timeContraint["Activity_Id"].InnerText);
                    constraint.NumberOfPreferredTimeSlots = Convert.ToInt32(timeContraint["Number_of_Preferred_Time_Slots"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Time_Slot"))
                        {
                            continue;
                        }
                        constraint.PreferredTimeSlots.Add(new TimeDayHour(subnode["Preferred_Day"].InnerText, subnode["Preferred_Hour"].InnerText));
                    }
                    timetable.TimeConstraints.ConstraintActivityPreferredTimeSlotsList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsMaxGapsPerDay"))
                {
                    
                    timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerDay = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesSameStartingHour"))
                {
                    ConstraintActivitiesSameStartingHour constraint = new ConstraintActivitiesSameStartingHour();
                   
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.TimeConstraints.ConstraintActivitiesSameStartingHourList.Add(constraint);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesSameStartingTime"))
                {
                    ConstraintActivitiesSameStartingTime constraint = new ConstraintActivitiesSameStartingTime();
                    
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.TimeConstraints.ConstraintActivitiesSameStartingTimeList.Add(constraint);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsIntervalMaxDaysPerWeek"))
                {
                    ConstraintIntervalMaxDaysPerWeek constraint = new ConstraintIntervalMaxDaysPerWeek();
                    
                    constraint.IntervalStartHour = timeContraint["Interval_Start_Hour"].InnerText;
                    constraint.IntervalEndHour = timeContraint["Interval_End_Hour"].InnerText;
                    constraint.MaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);

                    timetable.TimeConstraints.ConstraintsStudents.StudentsIntervalMaxDaysPerWeek = constraint;
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersIntervalMaxDaysPerWeek"))
                {
                    
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersIntervalMaxDaysPerWeek = new
                        ConstraintIntervalMaxDaysPerWeek()
                        {
                            IntervalStartHour = timeContraint["Interval_Start_Hour"].InnerText,
                            IntervalEndHour = timeContraint["Interval_End_Hour"].InnerText,
                            MaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText)
                        };
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsActivityTagMaxHoursContinuously"))
                {
                    ConstraintActivityTagMaxHoursContinuously constraint = new ConstraintActivityTagMaxHoursContinuously();
                    
                    constraint.ActivityTag = timeContraint["Activity_Tag"].InnerText;
                    constraint.MaximumHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);

                    timetable.TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMaxHoursContinuously"))
                {
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                    }
                    teacher.MaximumHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);

                    //timetable.TimeConstraints.ConstraintTeacherMaxHoursContinuouslyList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintThreeActivitiesGrouped"))
                {
                    ConstraintThreeActivitiesGrouped constraint = new ConstraintThreeActivitiesGrouped();
                   
                    int actId = Convert.ToInt32(timeContraint["First_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);
                    actId = Convert.ToInt32(timeContraint["Second_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);
                    actId = Convert.ToInt32(timeContraint["Third_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);

                    timetable.TimeConstraints.ConstraintThreeActivitiesGroupedList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTwoActivitiesGrouped"))
                {
                    ConstraintTwoActivitiesGrouped constraint = new ConstraintTwoActivitiesGrouped();
                    
                    int actId = Convert.ToInt32(timeContraint["First_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);
                    actId = Convert.ToInt32(timeContraint["Second_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);

                    timetable.TimeConstraints.ConstraintTwoActivitiesGroupedList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetMaxGapsPerDay"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.MaxGapsPerDay = Convert.ToInt32(timeContraint["Max_Gaps"].InnerText);

                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherActivityTagMaxHoursDaily"))
                {
                   
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }
                    teacher.TeacherActivityTagMaxHoursDailyList.Add(new ConstraintActivityTagMaxHoursDaily()
                    {
                        ActivityTag = timeContraint["Activity_Tag_Name"].InnerText,
                        MaximumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText)
                    });
                    
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMaxHoursContinuously"))
                {
                    
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintTeachersActivityTagMaxHoursContinuously"))
                {
                    ConstraintActivityTagMaxHoursContinuously constraint = new ConstraintActivityTagMaxHoursContinuously();
                    
                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;
                    constraint.MaximumHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);

                    timetable.TimeConstraints.ConstraintsTeachers.TeachersActivityTagMaxHoursContinuouslyList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersActivityTagMaxHoursDaily"))
                {
                    ConstraintActivityTagMaxHoursDaily constraint = new ConstraintActivityTagMaxHoursDaily();
                    
                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;
                    constraint.MaximumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);

                    timetable.TimeConstraints.ConstraintsTeachers.TeachersActivityTagMaxHoursDailyList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeacherMinDaysPerWeek"))
                {
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    teacher.MinimumDaysPerWeek = Convert.ToInt32(timeContraint["Minimum_Days_Per_Week"].InnerText);
                                        
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMaxDaysPerWeek"))
                {
                    
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsActivityTagMaxHoursDaily"))
                {
                    ConstraintActivityTagMaxHoursDaily constraint = new ConstraintActivityTagMaxHoursDaily();
                   
                    constraint.ActivityTag = timeContraint["Activity_Tag"].InnerText;
                    constraint.MaximumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);

                    timetable.TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintStudentsMaxHoursContinuously"))
                {
                    
                    timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintTwoActivitiesOrdered"))
                {
                    ConstraintTwoActivitiesOrdered constraint = new ConstraintTwoActivitiesOrdered();
                    
                    int actId = Convert.ToInt32(timeContraint["First_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);
                    actId = Convert.ToInt32(timeContraint["Second_Activity_Id"].InnerText);
                    constraint.ActivityIds.Add(actId);

                    timetable.TimeConstraints.ConstraintTwoActivitiesOrderedList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesEndStudentsDay"))
                {
                    ConstraintActivitiesEndStudentsDay constraint = new ConstraintActivitiesEndStudentsDay();
                    
                    constraint.Teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (constraint.Teacher == null)
                    {
                        constraint.Teacher = new Teacher(string.Empty);
                    }

                    constraint.Students = timeContraint["Students_Name"].InnerText;
                    constraint.Subject = timeContraint["Subject_Name"].InnerText;
                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;

                    timetable.TimeConstraints.ConstraintActivitiesEndStudentsDayList.Add(constraint);
                    continue;
                }
                else if (timeContraint.Name.Equals("ConstraintSubactivitiesPreferredStartingTimes"))
                {
                    ConstraintSubactivitiesPreferredStartingTimes constraint = new ConstraintSubactivitiesPreferredStartingTimes();
                   
                    constraint.ComponentNumber = Convert.ToInt32(timeContraint["Component_Number"].InnerText);
                    constraint.Teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (constraint.Teacher == null)
                    {
                        constraint.Teacher = new Teacher(string.Empty);
                    }

                    constraint.Students = timeContraint["Students_Name"].InnerText;
                    constraint.Subject = timeContraint["Subject_Name"].InnerText;
                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;
                    constraint.NumberOfPreferredStartingTimes = Convert.ToInt32(timeContraint["Number_of_Preferred_Starting_Times"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Starting_Time"))
                        {
                            continue;
                        }
                        constraint.PreferredStartingTimes.Add(new TimeDayHour(subnode["Preferred_Starting_Day"].InnerText, subnode["Preferred_Starting_Hour"].InnerText));
                    }

                    timetable.TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetActivityTagMaxHoursDaily"))
                {
                    ConstraintActivityTagMaxHoursDaily constraint = new ConstraintActivityTagMaxHoursDaily();
                                        
                    constraint.ActivityTag = timeContraint["Activity_Tag"].InnerText;
                    constraint.MaximumHoursDaily = Convert.ToInt32(timeContraint["Maximum_Hours_Daily"].InnerText);

                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.StudentsSetActivityTagMaxHoursDailyList.Add(constraint);
                    //timetable.TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursDailyList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintSubactivitiesPreferredTimeSlots"))
                {
                    ConstraintSubactivitiesPreferredTimeSlots constraint = new ConstraintSubactivitiesPreferredTimeSlots();
                   
                    constraint.ComponentNumber = Convert.ToInt32(timeContraint["Component_Number"].InnerText);
                    constraint.Teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (constraint.Teacher == null)
                    {
                        constraint.Teacher = new Teacher(string.Empty);
                    }

                    constraint.Students = timeContraint["Students_Name"].InnerText;
                    constraint.Subject = timeContraint["Subject_Name"].InnerText;
                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;
                    constraint.NumberOfPreferredTimeSlots = Convert.ToInt32(timeContraint["Number_of_Preferred_Time_Slots"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Time_Slot"))
                        {
                            continue;
                        }
                        constraint.PreferredTimeSlots.Add(new TimeDayHour(subnode["Preferred_Day"].InnerText, subnode["Preferred_Hour"].InnerText));
                    }

                    timetable.TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintMinGapsBetweenActivities"))
                {
                    ConstraintMinGapsBetweenActivities constraint = new ConstraintMinGapsBetweenActivities();
                    
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);
                    constraint.MinGaps = Convert.ToInt32(timeContraint["MinGaps"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.TimeConstraints.ConstraintMinGapsBetweenActivitiesList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetIntervalMaxDaysPerWeek"))
                {
                    ConstraintIntervalMaxDaysPerWeek constraint = new ConstraintIntervalMaxDaysPerWeek();
                                                            
                    constraint.IntervalStartHour = timeContraint["Interval_Start_Hour"].InnerText;
                    constraint.IntervalEndHour = timeContraint["Interval_End_Hour"].InnerText;
                    constraint.MaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.StudentsSetIntervalMaxDaysPerWeek = constraint;
                                        
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintTeachersMinDaysPerWeek"))
                {
                    
                    timetable.TimeConstraints.ConstraintsTeachers.TeachersMinDaysPerWeek = Convert.ToInt32(timeContraint["Minimum_Days_Per_Week"].InnerText);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintActivitiesOccupyMaxTimeSlotsFromSelection"))
                {
                    ConstraintActivitiesOccupyMaxTimeSlotsFromSelection constraint = new ConstraintActivitiesOccupyMaxTimeSlotsFromSelection();
                    
                    constraint.NumberOfActivities = Convert.ToInt32(timeContraint["Number_of_Activities"].InnerText);
                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    constraint.NumberOfSelectedTimeSlots = Convert.ToInt32(timeContraint["Number_of_Selected_Time_Slots"].InnerText);

                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Selected_Time_Slot"))
                        {
                            continue;
                        }
                        constraint.SelectedTimeSlots.Add(new TimeDayHour(subnode["Selected_Day"].InnerText, subnode["Selected_Hour"].InnerText));
                    }
                    constraint.MaxNumberOfOccupiedTimeSlots = Convert.ToInt32(timeContraint["Max_Number_of_Occupied_Time_Slots"].InnerText);

                    timetable.TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintTeacherActivityTagMaxHoursContinuously"))
                {
                    ConstraintActivityTagMaxHoursContinuously constraint = new ConstraintActivityTagMaxHoursContinuously();
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == timeContraint["Teacher_Name"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }

                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;
                    constraint.MaximumHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);
                    
                    teacher.TeacherActivityTagMaxHoursContinuouslyList.Add(constraint);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetMaxDaysPerWeek"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.MaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);

                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetMaxHoursContinuously"))
                {
                    
                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.MaximumHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsSetActivityTagMaxHoursContinuously"))
                {
                    ConstraintActivityTagMaxHoursContinuously constraint = new ConstraintActivityTagMaxHoursContinuously();
                    
                    constraint.ActivityTag = timeContraint["Activity_Tag_Name"].InnerText;
                    constraint.MaximumHoursContinuously = Convert.ToInt32(timeContraint["Maximum_Hours_Continuously"].InnerText);

                    var students = timetable.GetStudents(timeContraint["Students"].InnerText);
                    students.StudentsSetActivityTagMaxHoursContinuouslyList.Add(constraint);
                                       
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintStudentsMaxDaysPerWeek"))
                {                    
                    timetable.TimeConstraints.ConstraintsStudents.StudentsMaxDaysPerWeek = Convert.ToInt32(timeContraint["Max_Days_Per_Week"].InnerText);
                    continue;

                }
                else if (timeContraint.Name.Equals("ConstraintMaxDaysBetweenActivities"))
                {
                    ConstraintMaxDaysBetweenActivities constraint = new ConstraintMaxDaysBetweenActivities();
                  
                    foreach (XmlNode subnode in timeContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    constraint.MaxDays = Convert.ToInt32(timeContraint["Max_Days"].InnerText);

                    timetable.TimeConstraints.ConstraintMaxDaysBetweenActivitiesList.Add(constraint);
                    continue;

                }
                else
                {
                    MessageBox.Show("Not found: " + timeContraint.Name);
                    
                }

            }

            #endregion
        }
        
        /// <summary>
        /// loads space constraints
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="doc"></param>
        private static void LoadSpaceConstraints(Timetable timetable, XmlDocument doc)
        {
            #region space constraint
            foreach (XmlNode spaceContraint in doc.SelectSingleNode("/fet/Space_Constraints_List").ChildNodes)
            {

                if (spaceContraint.Name.Equals("ConstraintTeacherMinGapsBetweenBuildingChanges"))
                {
                    var teacher = timetable.TeacherList.Find(t => t.Name == spaceContraint["Teacher"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }
                    teacher.MinGapsBetweenBuildingChanges = Convert.ToInt32(spaceContraint["Min_Gaps_Between_Building_Changes"].InnerText);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeacherHomeRoom"))
                {
                    var teacher = timetable.TeacherList.Find(t => t.Name == spaceContraint["Teacher"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                    }

                    var room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);
                    teacher.HomeRooms.Add(room);
                   
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintSubjectActivityTagPreferredRoom"))
                {
                    ConstraintSubjectActivityTagPreferredRoom constraint = new ConstraintSubjectActivityTagPreferredRoom();
                    
                    constraint.Subject = spaceContraint["Subject"].InnerText;
                    constraint.ActivityTag = spaceContraint["Activity_Tag"].InnerText;
                    constraint.Room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);

                    timetable.SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Add(constraint);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintRoomNotAvailableTimes"))
                {
                   
                    var room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);
                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Not_Available_Time"))
                        {
                            continue;
                        }
                        room.NotAvailableTimes.Add(new TimeDayHour(subnode["Day"].InnerText, subnode["Hour"].InnerText));
                    }

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeacherHomeRooms"))
                {
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == spaceContraint["Teacher"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                    }                    
                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Room"))
                        {
                            continue;
                        }
                        Room room = timetable.GetRoomsList().Find(r => r.Name == subnode.InnerText);
                        teacher.HomeRooms.Add(room);

                    }

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintActivityPreferredRoom"))
                {
                    
                    var activity = timetable.ActivityList.Find(a => a.ActivityId == Convert.ToInt32(spaceContraint["Activity_Id"].InnerText));

                    var room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);
                    activity.PreferredRooms.Add(room);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintActivityPreferredRooms"))
                {
                    
                    var activity = timetable.ActivityList.Find(a => a.ActivityId == Convert.ToInt32(spaceContraint["Activity_Id"].InnerText));

                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Room"))
                        {
                            continue;
                        }
                        Room room = timetable.GetRoomsList().Find(r => r.Name == subnode.InnerText);
                        activity.PreferredRooms.Add(room);

                    }

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintSubjectPreferredRoom"))
                {
                   
                    var room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);
                    var subject = timetable.SubjectList.Find(s => s.Name == spaceContraint["Subject"].InnerText);
                    subject.HomeRooms.Add(room);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintSubjectActivityTagPreferredRooms"))
                {
                    ConstraintSubjectActivityTagPreferredRooms constraint = new ConstraintSubjectActivityTagPreferredRooms();
                    
                    constraint.Subject = spaceContraint["Subject"].InnerText;
                    constraint.ActivityTag = spaceContraint["Activity_Tag"].InnerText;
                    constraint.NumberOfPreferredRooms = Convert.ToInt32(spaceContraint["Number_of_Preferred_Rooms"].InnerText);

                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Room"))
                        {
                            continue;
                        }
                        Room room = timetable.GetRoomsList().Find(r => r.Name == subnode.InnerText);
                        constraint.Rooms.Add(room);

                    }

                    timetable.SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Add(constraint);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeacherMaxBuildingChangesPerDay"))
                {
                   
                    var teacher = timetable.TeacherList.Find(t => t.Name == spaceContraint["Teacher"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                        timetable.TeacherList.Add(teacher);
                    }
                    teacher.MaxBuildingChangesPerDay = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Day"].InnerText);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeachersMaxBuildingChangesPerDay"))
                {
                   
                    timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerDay = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Day"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsMaxBuildingChangesPerDay"))
                {
                    
                    timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerDay = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Day"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsMinGapsBetweenBuildingChanges"))
                {
                   
                    timetable.TimeConstraints.ConstraintsStudents.MinGapsBetweenBuildingChanges = Convert.ToInt32(spaceContraint["Min_Gaps_Between_Building_Changes"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsMaxBuildingChangesPerWeek"))
                {
                    
                    timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerWeek = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Week"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintActivityTagPreferredRoom"))
                {
                    
                    var activityTag = timetable.ActivitiyTagsList.Find(at => at.Name.Equals(spaceContraint["Activity_Tag"].InnerText));

                    var room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);

                    activityTag.PreferredRooms.Add(room);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintActivityTagPreferredRooms"))
                {
                   
                    var activityTag = timetable.ActivitiyTagsList.Find(at => at.Name.Equals(spaceContraint["Activity_Tag"].InnerText));

                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Room"))
                        {
                            continue;
                        }
                        Room room = timetable.GetRoomsList().Find(r => r.Name == subnode.InnerText);
                        activityTag.PreferredRooms.Add(room);

                    }

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeacherMaxBuildingChangesPerWeek"))
                {
                    
                    var teacher = timetable.TeacherList.Find(t => t.Name == spaceContraint["Teacher"].InnerText);
                    if (teacher == null)
                    {
                        teacher = new Teacher(string.Empty);
                    }
                    teacher.MaxBuildingChangesPerWeek = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Week"].InnerText);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeachersMaxBuildingChangesPerWeek"))
                {
                    
                    timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerWeek = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Week"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsSetHomeRoom"))
                {
                   
                    var room = timetable.GetRoomsList().Find(r => r.Name == spaceContraint["Room"].InnerText);

                    var students = timetable.GetStudents(spaceContraint["Students"].InnerText);
                    students.HomeRooms.Add(room);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintSubjectPreferredRooms"))
                {
                   
                    var subject = timetable.SubjectList.Find(s => s.Name == spaceContraint["Subject"].InnerText);

                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Room"))
                        {
                            continue;
                        }
                        Room room = timetable.GetRoomsList().Find(r => r.Name == subnode.InnerText);
                        subject.HomeRooms.Add(room);

                    }

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintActivitiesOccupyMaxDifferentRooms"))
                {
                    ConstraintActivitiesOccupyMaxDifferentRooms constraint = new ConstraintActivitiesOccupyMaxDifferentRooms();
                   
                    constraint.NumberOfActivities = Convert.ToInt32(spaceContraint["Number_of_Activities"].InnerText);
                    constraint.MaxNumberOfDifferentRooms = Convert.ToInt32(spaceContraint["Max_Number_of_Different_Rooms"].InnerText);

                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList.Add(constraint);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsSetHomeRooms"))
                {
                   
                    var students = timetable.GetStudents(spaceContraint["Students"].InnerText);
                                        
                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Preferred_Room"))
                        {
                            continue;
                        }
                        Room room = timetable.GetRoomsList().Find(r => r.Name == subnode.InnerText);
                        students.HomeRooms.Add(room);

                    }

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintTeachersMinGapsBetweenBuildingChanges"))
                {                    
                    timetable.TimeConstraints.ConstraintsTeachers.MinGapsBetweenBuildingChanges = Convert.ToInt32(spaceContraint["Min_Gaps_Between_Building_Changes"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsSetMaxBuildingChangesPerDay"))
                {
                    
                    var student = timetable.GetStudents(spaceContraint["Students"].InnerText);
                    student.MaxBuildingChangesPerDay = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Day"].InnerText);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsSetMaxBuildingChangesPerWeek"))
                {
                   
                    var student = timetable.GetStudents(spaceContraint["Students"].InnerText);
                    student.MaxBuildingChangesPerWeek = Convert.ToInt32(spaceContraint["Max_Building_Changes_Per_Week"].InnerText);
                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintStudentsSetMinGapsBetweenBuildingChanges"))
                {
                   
                    var student = timetable.GetStudents(spaceContraint["Students"].InnerText);
                    student.MinGapsBetweenBuildingChanges = Convert.ToInt32(spaceContraint["Min_Gaps_Between_Building_Changes"].InnerText);

                    continue;
                }
                else if (spaceContraint.Name.Equals("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive"))
                {
                    ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive constraint = new ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive();
                    
                    constraint.NumberOfActivities = Convert.ToInt32(spaceContraint["Number_of_Activities"].InnerText);
                    foreach (XmlNode subnode in spaceContraint.ChildNodes)
                    {
                        if (!subnode.Name.Equals("Activity_Id"))
                        {
                            continue;
                        }
                        int id = Convert.ToInt32(subnode.InnerText);
                        constraint.ActivityIds.Add(id);
                    }

                    timetable.SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList.Add(constraint);

                    continue;
                }
                else
                {
                     //MessageBox.Show("Not found: " + spaceContraint.Name);
                    
                }
            }

            #endregion
        }
    }
}
