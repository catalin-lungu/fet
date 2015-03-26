using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FET.Data;

namespace FET
{
    /// <summary>
    /// contains methods to generate the timetable
    /// </summary>
    class ManageTimetable
    {
        static int tryNr = 0;

        /// <summary>
        /// method used for generating the timetable after the timetable was filled with data
        /// </summary>
        /// <param name="timetable"></param>
        /// <returns></returns>
        public static bool Generate(Timetable timetable)
        {
            // create time slots for this timetable
            //these slots are empty and they will contain the generated timetable
            // at the end each slot will contain a list of scheduled activities
            timetable.LastGeneratedTimetable = new Timetable.GeneratedTimetable();

           
            #region manage activities
            //for each activity determine concurrent activities
            //activities that have the same teacher or the same students
            foreach (var activity in timetable.ActivityList)
            {
                foreach (var otherActivity in timetable.ActivityList)
                {
                    if (!activity.Equals(otherActivity))
                    {
                        if (activity.Teacher.Equals(otherActivity.Teacher)
                            || activity.Students.Equals(otherActivity.Students))
                        {
                            activity.ConcurrentActivities.Add(otherActivity);
                        }
                        // check for conflicts with class                        
                        else if (activity.Students is Group && (activity.Students as Group).YearName.Equals(otherActivity.Students.Name))
                        {
                            activity.ConcurrentActivities.Add(otherActivity);
                        }
                        else if (otherActivity.Students is Group && (otherActivity.Students as Group).YearName.Equals(activity.Students.Name))
                        {
                            activity.ConcurrentActivities.Add(otherActivity);
                        }
                        // check for conflicts with class or group
                        else if (activity.Students is Subgroup)
                        {
                            if ((activity.Students as Subgroup).GroupName.Equals(otherActivity.Students.Name))
                            {
                                activity.ConcurrentActivities.Add(otherActivity);
                            }
                            else if (otherActivity.Students is Year)
                            {
                                var gr = timetable.GetStudents().Find(g => g.Name.Equals((activity.Students as Subgroup).GroupName));
                                if ((gr as Group).YearName.Equals((otherActivity.Students as Year).Name))
                                {
                                    activity.ConcurrentActivities.Add(otherActivity);
                                }
                            }
                        }
                        else if (otherActivity.Students is Subgroup)
                        {
                            if ((otherActivity.Students as Subgroup).GroupName.Equals(activity.Students.Name))
                            {
                                activity.ConcurrentActivities.Add(otherActivity);
                            }
                            else if (activity.Students is Year)
                            {
                                var gr = timetable.GetStudents().Find(g => g.Name.Equals((otherActivity.Students as Subgroup).GroupName));
                                if ((gr as Group).YearName.Equals((activity.Students as Year).Name))
                                {
                                    activity.ConcurrentActivities.Add(otherActivity);
                                }
                            }
                        }
                    }
                }
            }

            //assign acceptable for each activity 
            //each activity contains a two dimensional array of int that represents the timetable slots
            //and the value of a cell of this array represents if this activity can take place in that slot,
            //at first all the arrays are assigned with 1, which means acceptable
            foreach (var activity in timetable.ActivityList)
            {
                activity.AcceptableTimeSlots = new int[timetable.DaysList.Count,timetable.HoursList.Count];
                for (int i = 0; i < activity.AcceptableTimeSlots.GetLength(0); i++)
                {
                    for (int j = 0; j < activity.AcceptableTimeSlots.GetLength(1); j++)
                        activity.AcceptableTimeSlots[i, j] = 1;
                }
                //if a previous generation of this timetable has been taken place, then make schedule activities unscheduled
                //because they must be scheduled in this new generated timetable
                if (activity.Scheduled)
                {
                    activity.Scheduled = false;
                    activity.PozScheduled = -1;
                    activity.UsedSlotPositions.Clear();
                }
            }
            #endregion
            
            // apply some type of restrictions on all activities from timetable 
            // this will change some values from the two dimensional array of each activity
            ApplyTimeRestrictions(timetable);

            // sort after first round of constraints
            // depending on how many slots are acceptable for each activity,
            // an activity which has fewer acceptable  slots is considered more important 
            timetable.ActivityList.Sort();
            //in each slot from timetable are added all activities which are acceptable till this moment for that slot
            AssignActivitiesInSlots(timetable);
            // in each slot from timetable add all the rooms 
            AssignRoomsInSlots(timetable);

            tryNr = 0;
            // make activities existing in the slots programmed depending on constraints
            // at the end of this method activities from slots will be programmed according to time and space constraints
            ScheduleActivities(timetable , tryNr);

            return false;
        }

        /// <summary>
        /// get the students timetable in a custom structure
        /// </summary>
        /// <param name="timetable"></param>
        /// <returns></returns>
        public static List<StudentYear> GetStudentTimetable(Timetable timetable)
        {
            List<StudentYear> list = new List<StudentYear>();

            string[,] timetableStud = new string[timetable.LastGeneratedTimetable.Days.Count,
                timetable.LastGeneratedTimetable.Days.ElementAt(0).Slots.Count];

            int i = 0 ,j = 0;
            foreach (var student in timetable.ClassList)
            {
                foreach (var day in timetable.LastGeneratedTimetable.Days)
                {
                    foreach (var slot in day.Slots)
                    {
                        foreach (var act in slot.Activities)
                        {
                            if (student.Name.Equals(act.Students.Name) && act.Scheduled)
                            {
                                timetableStud[i, j] = "Subject " + act.Subject.Name + Environment.NewLine +
                                    "Teacher " + act.Teacher.Name;
                                break;
                            }
                        }
                        j++;
                        if (j >= timetableStud.GetLength(1))
                        {
                            j = 0;
                        }
                    }
                    i++;
                    if (i >= timetableStud.GetLength(0))
                    {
                        i = 0;
                    }
                }

                //if (ContainsData(timetableStud))
                {
                    list.Add(new StudentYear(student.Name, timetableStud));
                }
                for (int k = 0; k < timetableStud.GetLength(0); k++)
                {
                    for (int l = 0; l < timetableStud.GetLength(1); l++)
                    {
                        timetableStud[k, l] = string.Empty;
                    }
                }

                foreach (var studentGroup in student.Groups)
                {
                    foreach (var day in timetable.LastGeneratedTimetable.Days)
                    {
                        foreach (var slot in day.Slots)
                        {
                            foreach (var act in slot.Activities)
                            {
                                if (studentGroup.Name.Equals(act.Students.Name) && act.Scheduled)
                                {
                                    timetableStud[i, j] = "Subject " + act.Subject.Name + Environment.NewLine +
                                        "Teacher " + act.Teacher.Name;
                                    break;
                                }
                            }
                            j++;
                            if (j >= timetableStud.GetLength(1))
                            {
                                j = 0;
                            }
                        }
                        i++;
                        if (i >= timetableStud.GetLength(0))
                        {
                            i = 0;
                        }
                    }
                    //if (ContainsData(timetableStud))
                    {
                        list.Last().Groups.Add(new StudentGroup(studentGroup.Name, timetableStud));
                    }
                    for (int k = 0; k < timetableStud.GetLength(0); k++)
                    {
                        for (int l = 0; l < timetableStud.GetLength(1); l++)
                        {
                            timetableStud[k, l] = string.Empty;
                        }
                    }

                    foreach (var studentSubGroup in studentGroup.Subgroups)
                    {
                        foreach (var day in timetable.LastGeneratedTimetable.Days)
                        {
                            foreach (var slot in day.Slots)
                            {
                                foreach (var act in slot.Activities)
                                {
                                    if (studentSubGroup.Name.Equals(act.Students.Name) && act.Scheduled)
                                    {
                                        timetableStud[i, j] = "Subject " + act.Subject.Name + Environment.NewLine +
                                            "Teacher " + act.Teacher.Name;
                                        break;
                                    }
                                }
                                j++;
                                if (j >= timetableStud.GetLength(1))
                                {
                                    j = 0;
                                }
                            }
                            i++;
                            if (i >= timetableStud.GetLength(0))
                            {
                                i = 0;
                            }
                        }
                        //if (ContainsData(timetableStud))
                        {
                            list.Last().Groups.Last().Subgroups.Add(new StudentSubgroup(studentSubGroup.Name, timetableStud));
                            
                        }
                        for (int k = 0; k < timetableStud.GetLength(0); k++)
                        {
                            for (int l = 0; l < timetableStud.GetLength(1); l++)
                            {
                                timetableStud[k, l] = string.Empty;
                            }
                        }
                    }
                }

                
            }

            return list;
            
        }

        /// <summary>
        /// get the teachers timetable in a custom structure
        /// </summary>
        /// <param name="timetable"></param>
        /// <returns></returns>
        public static List<TeacherSchedule> GetTeacherTimetable(Timetable timetable)
        {
            List<TeacherSchedule> teachersSchedule = new List<TeacherSchedule>();

            foreach (var teacher in timetable.TeacherList)
            {
                int d = -1;
                int h = -1;
                TeacherSchedule teacherSchedule = new TeacherSchedule();
                teacherSchedule.Teacher = teacher;

                foreach (var day in timetable.LastGeneratedTimetable.Days)
                {
                    d++;
                    foreach (var slot in day.Slots)
                    {
                        h++;
                        foreach (var act in slot.Activities)
                        {
                            if (teacher.Equals(act.Teacher))
                            {
                                teacherSchedule.DayHourList.Add(new TeacherSchedule.DayHour(d, h, act));
                                break;
                            }
                        }
                        if (h >= timetable.HoursList.Count -1)
                        {
                            h = -1;
                        }
                    }
                    if (d >= timetable.DaysList.Count -1)
                    {
                        d = -1;
                    }
                }

                teachersSchedule.Add(teacherSchedule);
            }
            return teachersSchedule;
        }

        /// <summary>
        /// get the rooms timetable in a custom structure
        /// </summary>
        /// <param name="timetable"></param>
        /// <returns></returns>
        public static List<RoomSchedule> GetRoomTimetable(Timetable timetable)
        {
            List<RoomSchedule> teachersSchedule = new List<RoomSchedule>();

            foreach (var room in timetable.GetRoomsList())
            {
                int d = -1;
                int h = -1;
                RoomSchedule roomSchedule = new RoomSchedule();
                roomSchedule.Room = room;

                foreach (var day in timetable.LastGeneratedTimetable.Days)
                {
                    d++;
                    foreach (var slot in day.Slots)
                    {
                        h++;
                        foreach (var act in slot.Activities)
                        {
                            if (room.Equals(act.AssignedRoom))
                            {
                                roomSchedule.DayHourList.Add(new RoomSchedule.DayHour(d, h, act));
                                break;
                            }
                        }
                        if (h >= timetable.HoursList.Count - 1)
                        {
                            h = -1;
                        }
                    }
                    if (d >= timetable.DaysList.Count - 1)
                    {
                        d = -1;
                    }
                }

                teachersSchedule.Add(roomSchedule);
            }
            return teachersSchedule;
        }

        /// <summary>
        /// apply restrictions on acceptable timeslots
        /// </summary>
        /// <param name="timetable"></param>
        private static void ApplyTimeRestrictions(Timetable timetable)
        {
            foreach (var activity in timetable.ActivityList)
            {
                ApplyConstraintBreakTimes(timetable, activity);
                ApplyConstraintTeacherNotAvailableTimes(timetable, activity);
                ApplyConstraintActivityPreferredStartingTimes(timetable, activity);
                ApplyConstraintActivitiesPreferredTimeSlots(timetable, activity);
                ApplyConstraintActivitiesPreferredStartingTimes(timetable, activity);
                ApplyConstraintStudentsSetNotAvailableTimes(timetable, activity);
                ApplyConstraintActivityPreferredStartingTime(timetable, activity);
                ApplyConstraintActivityPreferredTimeSlots(timetable, activity);
                ApplyConstraintSubactivitiesPreferredStartingTimes(timetable, activity);
                ApplyConstraintSubactivitiesPreferredTimeSlots(timetable, activity);
                ApplyConstraintActivitiesNotOverlapping(timetable, activity);
            }
        }

        #region Apply Time Restrictions
        //ConstraintBreakTimes
        private static void ApplyConstraintBreakTimes(Timetable timetable, Activity activity)
        {
            foreach (var breakTime in timetable.TimeConstraints.ConstraintsBreakTimes.BreakTimes)
            {
                int i = timetable.DaysList.IndexOf(breakTime.Day);
                int j = timetable.HoursList.IndexOf(breakTime.Hour);
                activity.AcceptableTimeSlots[i, j] = 0;
            }
        }
        
        //ConstraintTeacherNotAvailableTimes
        private static void ApplyConstraintTeacherNotAvailableTimes(Timetable timetable, Activity activity)
        {

            foreach (var notAvailableTime in activity.Teacher.NotAvailableTimes)
            {
                int i = timetable.DaysList.IndexOf(notAvailableTime.Day);
                int j = timetable.HoursList.IndexOf(notAvailableTime.Hour);
                activity.AcceptableTimeSlots[i, j] = 0;
            }


        }
        
        //ConstraintActivityPreferredStartingTimes
        private static void ApplyConstraintActivityPreferredStartingTimes(Timetable timetable, Activity activity)
        {
            foreach (var condition in timetable.TimeConstraints.ConstraintActivityPreferredStartingTimesList)
            {
                if (activity.ActivityId == condition.ActivityId)
                {
                    foreach (var prefDay in condition.PreferredStartingTimes)
                    {
                        int i = timetable.DaysList.IndexOf(prefDay.Day);
                        int j = timetable.HoursList.IndexOf(prefDay.Hour);
                        if (activity.AcceptableTimeSlots[i, j] != 0)
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }
                    }
                }
            }
        }
        
        //ConstraintActivitiesPreferredTimeSlots
        private static void ApplyConstraintActivitiesPreferredTimeSlots(Timetable timetable, Activity activity)
        {
            foreach (var condition in timetable.TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList)
            {
                if ((activity.ActivityTag != null && activity.ActivityTag.Name.Equals(condition.ActivityTagName) || string.IsNullOrEmpty(condition.ActivityTagName))
                    && (activity.Teacher.Equals(condition.Teacher) || string.IsNullOrEmpty(condition.Teacher.Name))
                    && (activity.Students.Equals(condition.StudentsName) || string.IsNullOrEmpty(condition.StudentsName))
                    && (activity.Subject.Name.Equals(condition.SubjectName) || string.IsNullOrEmpty(condition.SubjectName)))
                {
                    foreach (var prefTimeSlots in condition.PreferredTimeSlots)
                    {
                        int i = timetable.DaysList.IndexOf(prefTimeSlots.Day);
                        int j = timetable.HoursList.IndexOf(prefTimeSlots.Hour);
                        if (activity.AcceptableTimeSlots[i, j] != 0)
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }
                    }
                }
            }
        }
        
        //ConstraintActivitiesPreferredStartingTimes
        private static void ApplyConstraintActivitiesPreferredStartingTimes(Timetable timetable, Activity activity)
        {
            foreach (var condition in timetable.TimeConstraints.ConstraintActivitiesPreferredStartingTimesList)
            {
                if ((activity.ActivityTag != null && activity.ActivityTag.Name.Equals(condition.ActivityTagName) || string.IsNullOrEmpty(condition.ActivityTagName))
                    && (activity.Teacher.Equals(condition.Teacher) || string.IsNullOrEmpty(condition.Teacher.Name))
                    && (activity.Students.Equals(condition.StudentsName) || string.IsNullOrEmpty(condition.StudentsName))
                    && (activity.Subject.Name.Equals(condition.SubjectName) || string.IsNullOrEmpty(condition.SubjectName)))
                {
                    foreach (var prefTimeSlots in condition.PreferredStartingTime)
                    {
                        int i = timetable.DaysList.IndexOf(prefTimeSlots.Day);
                        int j = timetable.HoursList.IndexOf(prefTimeSlots.Hour);
                        if (activity.AcceptableTimeSlots[i, j] != 0)
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }
                    }
                }
            }
        }

        //ConstraintStudentsSetNotAvailableTimes
        private static void ApplyConstraintStudentsSetNotAvailableTimes(Timetable timetable, Activity activity)
        {
            foreach (var notAvailable in activity.Students.NotAvailableTimes)
            {
                int i = timetable.DaysList.IndexOf(notAvailable.Day);
                int j = timetable.HoursList.IndexOf(notAvailable.Hour);
                activity.AcceptableTimeSlots[i, j] = 0;
            }
        }

        //ConstraintActivityPreferredStartingTime
        private static void ApplyConstraintActivityPreferredStartingTime(Timetable timetable, Activity activity)
        {
            foreach (var constraint in timetable.TimeConstraints.ConstraintActivityPreferredStartingTimeList)
            {
                if (activity.ActivityId == constraint.ActivityId)
                {
                    int i = timetable.DaysList.IndexOf(constraint.PreferredDay);
                    int j = timetable.HoursList.IndexOf(constraint.PreferredHour);
                    if (activity.AcceptableTimeSlots[i, j] != 0)
                    {
                        if (constraint.PermanentlyLocked)
                        {
                            activity.AcceptableTimeSlots[i, j] = 3;
                        }
                        else
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }
                    }
                    
                }
            }
        }

        //ConstraintActivityPreferredTimeSlots
        private static void ApplyConstraintActivityPreferredTimeSlots(Timetable timetable, Activity activity)
        {
            foreach (var constraint in timetable.TimeConstraints.ConstraintActivityPreferredTimeSlotsList)
            {
                if (activity.ActivityId == constraint.ActivityId)
                {
                    foreach (var prefTime in constraint.PreferredTimeSlots)
                    {
                        int i = timetable.DaysList.IndexOf(prefTime.Day);
                        int j = timetable.HoursList.IndexOf(prefTime.Hour);
                        if (activity.AcceptableTimeSlots[i, j] != 0)
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }

                    }
                }
            }
        }

        //ConstraintSubactivitiesPreferredStartingTimes
        private static void ApplyConstraintSubactivitiesPreferredStartingTimes(Timetable timetable, Activity activity)
        {
            foreach (var constraint in timetable.TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList)
            {
                bool sameActivity = activity.Teacher.Equals(constraint.Teacher) || constraint.Teacher.Name.Equals(string.Empty);
                sameActivity = (activity.Students.Equals(constraint.Students) || constraint.Students.Equals(string.Empty)) && sameActivity;
                sameActivity = (activity.Subject.Equals(constraint.Subject) || constraint.Subject.Equals(string.Empty)) && sameActivity;
                //sameActivity = ((activity.ActivityTag == null && constraint.ActivityTag.Equals(string.Empty))
                 //   || activity.ActivityTag.Equals(constraint.ActivityTag) || constraint.ActivityTag.Equals(string.Empty)) && sameActivity;
                
                if (sameActivity)
                {
                    foreach (var prefTime in constraint.PreferredStartingTimes)
                    {
                        int i = timetable.DaysList.IndexOf(prefTime.Day);
                        int j = timetable.HoursList.IndexOf(prefTime.Hour);
                        if (activity.AcceptableTimeSlots[i, j] != 0)
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }

                    }
                }
            }
        }

        //ConstraintSubactivitiesPreferredTimeSlots
        private static void ApplyConstraintSubactivitiesPreferredTimeSlots(Timetable timetable, Activity activity)
        {
            foreach (var constraint in timetable.TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList)
            {
                bool sameActivity = activity.Teacher.Equals(constraint.Teacher) || constraint.Teacher.Name.Equals(string.Empty);
                sameActivity = (activity.Students.Equals(constraint.Students) || constraint.Students.Equals(string.Empty)) && sameActivity;
                sameActivity = (activity.Subject.Equals(constraint.Subject) || constraint.Subject.Equals(string.Empty)) && sameActivity;
               // sameActivity = ((activity.ActivityTag == null && constraint.ActivityTag.Equals(string.Empty))
                //    || activity.ActivityTag.Equals(constraint.ActivityTag) || constraint.ActivityTag.Equals(string.Empty)) && sameActivity;

                if (sameActivity)
                {
                    foreach (var prefTime in constraint.PreferredTimeSlots)
                    {
                        int i = timetable.DaysList.IndexOf(prefTime.Day);
                        int j = timetable.HoursList.IndexOf(prefTime.Hour);
                        if (activity.AcceptableTimeSlots[i, j] != 0)
                        {
                            activity.AcceptableTimeSlots[i, j] = 2;
                        }

                    }
                }
            }
        }

        //ConstraintActivitiesNotOverlapping
        private static void ApplyConstraintActivitiesNotOverlapping(Timetable timetable, Activity activity)
        {
            foreach (var con in timetable.TimeConstraints.ConstraintActivitiesNotOverlappingList)
            {
                bool applies = false;
                foreach (var actId in con.ActivityIds)
                {
                    if (actId == activity.ActivityId) 
                    {
                        applies = true;
                    }
                }
                if (applies)
                {
                    foreach (var actId in con.ActivityIds)
                    {
                        foreach(var a in timetable.ActivityList)
                        {
                            if(a.ActivityId == actId && a.ActivityId != activity.ActivityId)
                            {
                                activity.ConcurrentActivities.Add(a);
                            }
                        }

                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// create days and hours based on days list and hours list and
        /// for each timeslot assign acceptable activities so far
        /// </summary>
        /// <param name="timetable"></param>
        private static void AssignActivitiesInSlots(Timetable timetable)
        {
            timetable.LastGeneratedTimetable.Days.Clear();

            int nrDays = timetable.DaysList.Count;
            int nrHours = timetable.HoursList.Count;

            for (int dayIndex = 0; dayIndex < nrDays; dayIndex++)
            {
                Timetable.Day day = new Timetable.Day();

                for (int hIndex = 0; hIndex < nrHours; hIndex++)
                {
                    Timetable.TimeSlot timeSlot = new Timetable.TimeSlot();

                    foreach (var activity in timetable.ActivityList)
                    {

                        if (activity.AcceptableTimeSlots[dayIndex, hIndex] > 0)
                        {
                            timeSlot.Activities.Add(activity);
                        }
                    }

                    day.Slots.Add(timeSlot);

                }
                timetable.LastGeneratedTimetable.Days.Add(day);

            }
        }

        /// <summary>
        /// for each time slot add available rooms for that slot
        /// </summary>
        /// <param name="timetable"></param>
        private static void AssignRoomsInSlots(Timetable timetable)
        {
            int d = -1;
            int h = -1;
            foreach (var day in timetable.LastGeneratedTimetable.Days)
            {
                d++;
                foreach (var slot in day.Slots)
                {
                    h++;
                    foreach (var room in timetable.GetRoomsList())
                    {
                        //check if exists a restriction for time on this room
                        bool canBeAdded = true;

                        foreach (var dayHour in room.NotAvailableTimes)
                        {
                            if (d == timetable.DaysList.IndexOf(dayHour.Day) &&
                                h == timetable.HoursList.IndexOf(dayHour.Hour))
                            {
                                canBeAdded = false;
                                break;
                            }
                        }
                        
                        if (canBeAdded)
                        {
                            slot.Rooms.Add(room);
                        }
                    }
                    if (h >= day.Slots.Count)
                    {
                        h = -1;
                    }
                }
                if (d >= timetable.LastGeneratedTimetable.Days.Count)
                {
                    d = -1;
                }
            }
        }

        // from here begins the process of scheduling for the activities existing in the slots

        /// <summary>
        /// create the timetable
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="tryNumber"></param>
        private static void ScheduleActivities(Timetable timetable, int tryNumber)
        {
            //schedule activities
            // at this point the list of activities is sorted depending on how many acceptable slots
            // the first activity from the list has the minimum number of acceptable slots
            for (int actIndex = 0; actIndex < timetable.ActivityList.Count() - 1; actIndex++)
            {
                var activity = timetable.ActivityList.ElementAt(actIndex);
                if (!activity.Active)
                {
                    continue;
                }

                //int nrOfGen = 0;
                int dayIndex = -1, hourIndex = -1;

                var teacher = activity.Teacher;
                

                // if you want to choose an acceptable time slot for an activity random
                // you can use while region instead of those two fors which get always the first 
                //good time slot; if you choose to run the algorithm with while
                //it's possible that every time you will end up with a different
                //schedule for activities, that's why I prefer these two fors

                #region  choose random time slot 
                //while (!activity.Scheduled && !(activity.CountAcceptableSlots() < nrOfGen))
                //{
                //    nrOfGen++;
                //    int j;
                //    int i = activity.GetRandomSlotFromAcceptableSlots(out j);
                #endregion 
                #region iterrate thru time slots and choose first that is acceptable
                for (int i = 0; i < activity.AcceptableTimeSlots.GetLength(0) && !activity.Scheduled; i++)
                {
                    for (int j = 0; j < activity.AcceptableTimeSlots.GetLength(1) && !activity.Scheduled; j++)
                    {
                #endregion
                        if (i > -1 && activity.AcceptableTimeSlots[i, j] > 0 && !activity.Scheduled)
                        {
                            Timetable.Day day = timetable.LastGeneratedTimetable.Days.ElementAt(i);
                            Timetable.TimeSlot slot = day.Slots.ElementAt(j);

                            if (activity.UsedSlotPositions.Contains(i * 10 + j))
                            {
                                if (activity.CountAcceptableSlots() > activity.UsedSlotPositions.Count)
                                {
                                    continue;
                                }
                            }
                            
                            #region apply time restrictions

                            bool accept = true;
                            accept = accept && AcceptableForConstraintTeacherMaxDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTeachersMaxDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTeacherMaxGapsPerDay(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeacherIntervalMaxDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsMaxHoursDaily(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeacherMaxGapsPerWeek(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeachersMaxGapsPerWeek(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeacherMaxHoursDaily(timetable, activity, i);
                            accept = accept && AcceptableForConstraintActivitiesSameStartingDay(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsSetMaxGapsPerWeek(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsMaxGapsPerWeek(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeachersMaxGapsPerDay(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsSetMaxHoursDaily(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsSetActivityTagMaxHoursDaily(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintMinDaysBetweenActivities(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintMinGapsBetweenActivities(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintActivityEndsStudentsDay(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintActivitiesEndStudentsDay(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeachersMaxHoursDaily(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsMaxGapsPerDay(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintActivitiesSameStartingHour(timetable, activity, j);
                            accept = accept && AcceptableForConstraintActivitiesSameStartingTime(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeachersIntervalMaxDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsIntervalMaxDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsSetIntervalMaxDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsActivityTagMaxHoursContinuously(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeachersActivityTagMaxHoursContinuously(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeacherMaxHoursContinuously(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeachersMaxHoursContinuously(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsMaxHoursContinuously(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsSetMaxGapsPerDay(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTeacherActivityTagMaxHoursDaily(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTeachersActivityTagMaxHoursDaily(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsActivityTagMaxHoursDaily(timetable, activity, i, j);

                            accept = accept && AcceptableForConstraintTeacherMinDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTeachersMinDaysPerWeek(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTeacherMinHoursDaily(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTeachersMinHoursDaily(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsSetMinHoursDaily(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsMinHoursDaily(timetable, activity, i);
                            accept = accept && AcceptableForConstraintStudentsEarlyMaxBeginningsAtSecondHour(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintStudentsSetEarlyMaxBeginningsAtSecondHour(timetable, activity, i, j);

                            accept = accept && AcceptableForConstraintActivitiesOccupyMaxTimeSlotsFromSelection(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintTwoActivitiesOrdered(timetable, activity);
                            accept = accept && AcceptableForConstraintTwoActivitiesConsecutive(timetable, activity, i, j);
                            accept = accept && AcceptableForConstraintThreeActivitiesGrouped(timetable, activity, i);
                            accept = accept && AcceptableForConstraintTwoActivitiesGrouped(timetable, activity, i);

                            #endregion

                            #region find an acceptable room for this time slot that is ok for this activity
                            Room room = null;
                            if (timetable.GetRoomsList().Count > 0)
                            {
                                room = FindARoom(timetable, activity,
                                    timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(j).Rooms, i, j);

                                accept = accept && room != null;
                            }
                            #endregion

                            // check if all the time restrictions are verified 
                            // and a room has been found
                            if (!accept)
                            {
                                continue; // can not be in this slot
                            }

                            int myPoz = timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(j).
                                        Activities.IndexOf(activity);

                            // check if this activity hasn't been removed from the list of this slot by other previous scheduled activities
                            if (myPoz > -1)
                            {   // schedule this activity in this slot 
                                dayIndex = i;
                                hourIndex = j;
                                activity.Scheduled = true;
                                activity.PozScheduled = i * 10 + j;
                                activity.UsedSlotPositions.Add(i * 10 + j);

                                if (room != null)
                                {
                                    activity.AssignedRoom = room;
                                    timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(j).Rooms.Remove(room);
                                }
                            }
                            else
                            {
                                continue;
                            }

                            //remove concurrent activities from this slot because this activity has been scheduled here
                            foreach (var concurentActivity in activity.ConcurrentActivities)
                            {
                                if (slot.Activities.Contains(concurentActivity))
                                {

                                    int concurPoz = timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(j).
                                        Activities.IndexOf(concurentActivity);

                                    // if this activity has a high priority
                                    if (myPoz < concurPoz && myPoz > -1)
                                    {

                                        timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(j).
                                            Activities.Remove(concurentActivity);
                                    }

                                }
                            }


                        }
                    }
                }
               // }// while 
                    //remove this scheduled activity from other time slots
                    if (activity.Scheduled)
                    {
                        for (int i = 0; i < timetable.LastGeneratedTimetable.Days.Count; i++)
                        {
                            for (int j = 0; j < timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.Count; j++)
                            {
                                if (!(i == dayIndex && j == hourIndex))
                                {
                                    timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(j).Activities.Remove(activity);
                                }
                            }
                        }
                    }
                    //has not been able to schedule this activity
                    // go back (max 14) and try to schedule this activity by making other previous activities unscheduled 
                    else
                    {
                        #region hardwork for PC
                        /*
                    //try alternatives recursively (it may take awhile and may return a worse timetable)
                    List<Activity> unschedule = new List<Activity>();
                    for (int i = actIndex-1; i >= 0 && i >=actIndex-14 && !activity.Scheduled; i--)
                    {
                        unschedule.Add(timetable.ActivityList.ElementAt(i));
                        RemoveScheduledDate(timetable, timetable.ActivityList.ElementAt(i));
                        TryToSchedule(timetable, activity);
                    }                    
                    for (int i = unschedule.Count() - 1; i >= 0; i--)
                    {
                        TryToSchedule(timetable, unschedule.ElementAt(i));
                        if (!unschedule.ElementAt(i).Scheduled)
                        {
                            RemoveScheduledDate(timetable, activity);
                            TryToSchedule(timetable, unschedule.ElementAt(i));
                        }
                    }*/
                        #endregion
                        if (!activity.Scheduled)
                        {
                            timetable.LastGeneratedTimetable.UnableToSchedule.Add(activity);
                        }
                    }
                
            }

            if (timetable.AuxTimetable == null)
            {
                timetable.AuxTimetable = timetable.LastGeneratedTimetable.Clone();

            }
            // if some activities haven't been scheduled due to time constraints
            // try to schedule these activities depending on programming the concurrent activities
            // so these activities will fulfill a part of time constraints
            TryToPlaceUnscheduled(timetable);

            // check if timetable ( slots) needs to be regenerated
           
            if (timetable.LastGeneratedTimetable.UnableToSchedule.Count > 0)
            {
                //even so maybe is better than existing timetable
                if (timetable.LastGeneratedTimetable.UnableToSchedule.Count < timetable.AuxTimetable.UnableToSchedule.Count)
                {
                    timetable.AuxTimetable = timetable.LastGeneratedTimetable.Clone();

                }
                timetable.LastGeneratedTimetable.Clear();
                AssignActivitiesInSlots(timetable);

                //try recursively for 4 times to regenerate the whole timetable
                if (tryNr >= 4)
                {
                    timetable.LastGeneratedTimetable = timetable.AuxTimetable.Clone();
                    return;
                }
                //prepare for a new scheduling
                foreach (var act in timetable.ActivityList)
                {
                    act.Scheduled = false;
                }
                AssignRoomsInSlots(timetable);
                ScheduleActivities(timetable, tryNr++);
            }

            
        }

        /// <summary>
        /// place activities in slot taking into account only that 
        /// the activities in a slot must not have the same teacher or the same students
        /// </summary>
        /// <param name="timetable"></param>
        private static void TryToPlaceUnscheduled(Timetable timetable)
        {
            bool foundSlot = false;
            for (int i = timetable.LastGeneratedTimetable.UnableToSchedule.Count()-1; i >= 0; i-- )
            {
                foundSlot = false;
                Activity act = timetable.LastGeneratedTimetable.UnableToSchedule.ElementAt(i);
                int d=-1;
                int lastH = -1;
                foreach (var day in timetable.LastGeneratedTimetable.Days)
                {
                    d++;
                    int h = -1;
                    foreach (var slot in day.Slots)
                    {
                        h++;
                        bool possibleHere = true;
                        foreach (var possibleConcurent in slot.Activities)
                        {
                            if (possibleConcurent.ConcurrentActivities.Contains(act) && possibleConcurent.Scheduled
                                && act.AcceptableTimeSlots[d, h] > 0)
                            {
                                possibleHere = false;
                                break;
                            }
                        }
                        if (possibleHere)
                        {

                            if (lastH == -1 || h < lastH) // is new or better
                            {
                                lastH = h;
                                act.Scheduled = true;
                                act.ForcedScheduled = true;
                                for (int k = 0; k < timetable.LastGeneratedTimetable.Days.Count; k++)
                                {
                                    for (int j = 0; j < timetable.LastGeneratedTimetable.Days.ElementAt(k).Slots.Count; j++)
                                    {
                                        timetable.LastGeneratedTimetable.Days.ElementAt(k).Slots.ElementAt(j).Activities.Remove(act);

                                    }
                                }
                                slot.Activities.Add(act);

                                foundSlot = true;
                                break;
                            }
                        }
                    }
                }
                if (foundSlot)
                {
                    timetable.LastGeneratedTimetable.UnableToSchedule.RemoveAt(i);
                }
            }
        }
       
        
        #region time constraints
        /// <summary>
        /// check if it is acceptable for ConstraintTeacherMaxDaysPerWeek
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="activity"></param>
        /// <param name="desiredDay"></param>
        /// <returns></returns>
        private static bool AcceptableForConstraintTeacherMaxDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Teacher.MaxDaysPerWeek == -1)
            {
                return true;
            }
            else
            {
                HashSet<int> setOfDays = new HashSet<int>();



                //check activities that have the same teacher
                foreach (var act in activity.ConcurrentActivities)
                {
                    if (act.Teacher.Equals(activity.Teacher))
                    {
                        //count days to see if constraints is feasible
                        if (act.Scheduled)
                        {
                            int h;
                            int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                            setOfDays.Add(d);
                        }
                        //this activity is not scheduled in that case determine an appropriate day possible for this activity to be scheduled in
                        else
                        {
                            bool dayFound = false;
                            for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                            {
                                for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                                {
                                    if (act.AcceptableTimeSlots[i, j] > 0)
                                    {
                                        if (setOfDays.Contains(i))
                                        {
                                            dayFound = true;
                                        }

                                    }
                                }
                            }
                            if (!dayFound)
                            {
                                for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1) && !dayFound; i++)
                                {
                                    if (act.AcceptableTimeSlots[desiredDay, i] > 0)
                                    {
                                        dayFound = true;
                                        setOfDays.Add(desiredDay);
                                    }
                                }
                                if (!dayFound)
                                {
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1); j++)
                                        {
                                            if (act.AcceptableTimeSlots[i, j] > 0)
                                            {
                                                setOfDays.Add(i);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (setOfDays.Count <= activity.Teacher.MaxDaysPerWeek)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool AcceptableForConstraintTeachersMaxDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            bool acceptable = false;

            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxDaysPerWeek == -1)
            {
                return true;
            }
            else
            {
                HashSet<int> setOfDays = new HashSet<int>();

                
                    
                        //check activities that have the same teacher
                    foreach (var act in activity.ConcurrentActivities)
                    {
                        if (act.Teacher.Equals(activity.Teacher))
                        {
                            //count days to see if constraints is feasible
                            if (act.Scheduled)
                            {
                                int h;
                                int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                                setOfDays.Add(d);
                            }
                            //this activity is not scheduled in that case determine an appropriate day possible for this activity to be scheduled in
                            else
                            {
                                bool dayFound = false;
                                for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                                {
                                    for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                                    {
                                        if (act.AcceptableTimeSlots[i, j] > 0)
                                        {
                                            if (setOfDays.Contains(i))
                                            {
                                                dayFound = true;
                                            }

                                        }
                                    }
                                }
                                if (!dayFound)
                                {
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1) && !dayFound; i++)
                                    {
                                        if (act.AcceptableTimeSlots[desiredDay, i] > 0)
                                        {
                                            dayFound = true;
                                            setOfDays.Add(desiredDay);
                                        }
                                    }
                                    if (!dayFound)
                                    {
                                        for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                        {
                                            for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1); j++)
                                            {
                                                if (act.AcceptableTimeSlots[i, j] > 0)
                                                {
                                                    setOfDays.Add(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    if (setOfDays.Count <= timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxDaysPerWeek)
                    {
                        acceptable = true;
                    }
                    else
                    {
                        return false;
                    }
                
            }

            return acceptable;
        }
        
        private static bool AcceptableForConstraintTeacherMaxGapsPerWeek(Timetable timetable, Activity activity, int desiredDay , int desiredHour)
        {
            if (activity.Teacher.MaxGapsPerWeek == -1)
            {
                return true;
            }
            else
            {
                int nrOfGaps = 0;
                int d = -1;
                foreach (var day in timetable.LastGeneratedTimetable.Days)
                {
                    bool counting = false;
                    int lastHourOfTheDay = -1;
                    int h = -1;
                    d++;
                    foreach (var hour in day.Slots)
                    {
                        h++;
                        var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                        if (interserct.Count() > 0)
                        {
                            bool aConcurrentActivityHasFound = false;
                            foreach (var inter in interserct)
                            {
                                if (inter.Scheduled)
                                {
                                    counting = true;
                                    aConcurrentActivityHasFound = true;
                                    lastHourOfTheDay = h;
                                    break;
                                }
                            }
                            if (aConcurrentActivityHasFound)
                            {
                                continue;
                            }
                        }
                        //we want to schedule here this activity
                        else if (h == desiredHour && desiredDay == d)
                        {
                            counting = true;
                            lastHourOfTheDay = h;
                            continue;
                        }
                        if (counting)
                        {
                            nrOfGaps++;
                        }
                    }
                    //remove final hours
                    if (counting)
                    {
                        int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                        nrOfGaps -= dif;
                    }
                }
                if (nrOfGaps > activity.Teacher.MaxDaysPerWeek)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintTeachersMaxGapsPerWeek(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerWeek == -1)
            {
                return true;
            }
            else
            {                
                    int nrOfGaps = 0;
                    int d = -1;
                    foreach (var day in timetable.LastGeneratedTimetable.Days)
                    {
                        bool counting = false;
                        int lastHourOfTheDay = -1;
                        int h = -1;
                        d++;
                        foreach (var hour in day.Slots)
                        {
                            h++;
                            var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                            if (interserct.Count() > 0)
                            {
                                bool aConcurrentActivityHasFound = false;
                                foreach (var inter in interserct)
                                {
                                    if (inter.Scheduled && inter.Teacher.Equals(activity.Teacher))
                                    {
                                        counting = true;
                                        aConcurrentActivityHasFound = true;
                                        lastHourOfTheDay = h;
                                        break;
                                    }
                                }
                                if (aConcurrentActivityHasFound)
                                {
                                    continue;
                                }
                            }
                            //we want to schedule here this activity
                            if (h == desiredHour && desiredDay == d)
                            {
                                counting = true;
                                lastHourOfTheDay = h;
                                continue;
                            }
                            if (counting)
                            {
                                nrOfGaps++;
                            }
                        }
                        //remove final hours
                        if (counting)
                        {
                            int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                            nrOfGaps -= dif;
                        }
                    }
                    if (nrOfGaps > timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerWeek)
                    {
                        return false;
                    }

                
            }
            return true;
        }
        
        private static bool AcceptableForConstraintTeacherMaxGapsPerDay(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (activity.Teacher.MaxGapsPerDay == -1)
            {
                return true;
            }
            else
            {
                int nrOfGaps = 0;
                int d = -1;
                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                {
                    bool counting = false;
                    int lastHourOfTheDay = -1;
                    int h = -1;
                    d++;
                    foreach (var hour in day.Slots)
                    {
                        h++;
                        var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                        if (interserct.Count() > 0)
                        {
                            bool aConcurrentActivityHasFound = false;
                            foreach (var inter in interserct)
                            {
                                if (inter.Scheduled && inter.Teacher.Equals(activity.Teacher))
                                {
                                    counting = true;
                                    aConcurrentActivityHasFound = true;
                                    lastHourOfTheDay = h;
                                    break;
                                }
                            }
                            if (aConcurrentActivityHasFound)
                            {
                                continue;
                            }
                        }
                        //we want to schedule here this activity
                        else if (h == desiredHour && desiredDay == d)
                        {
                            counting = true;
                            lastHourOfTheDay = h;
                            continue;
                        }
                        if (counting)
                        {
                            nrOfGaps++;
                        }
                    }
                    //remove final hours
                    if (counting)
                    {
                        int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                        nrOfGaps -= dif;
                    }
                }
                if (nrOfGaps > activity.Teacher.MaxGapsPerDay)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintTeachersMaxGapsPerDay(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerDay == -1)
            {
                return true;
            }
            else
            {


                int nrOfGaps = 0;
                int d = -1;
                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                {
                    bool counting = false;
                    int lastHourOfTheDay = -1;
                    int h = -1;
                    d++;
                    foreach (var hour in day.Slots)
                    {
                        h++;
                        var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                        if (interserct.Count() > 0)
                        {
                            bool aConcurrentActivityHasFound = false;
                            foreach (var inter in interserct)
                            {
                                if (inter.Scheduled && inter.Teacher.Equals(activity.Teacher))
                                {
                                    counting = true;
                                    aConcurrentActivityHasFound = true;
                                    lastHourOfTheDay = h;
                                    break;
                                }
                            }
                            if (aConcurrentActivityHasFound)
                            {
                                continue;
                            }
                        }
                        //we want to schedule here this activity
                        else if (h == desiredHour && desiredDay == d)
                        {
                            counting = true;
                            lastHourOfTheDay = h;
                            continue;
                        }
                        if (counting)
                        {
                            nrOfGaps++;
                        }
                    }

                    //remove final hours
                    if (counting)
                    {
                        int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                        nrOfGaps -= dif;
                    }
                }
                if (nrOfGaps > timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerDay)
                {
                    return false;
                }


            }
            return true;
        }
               
        private static bool AcceptableForConstraintTeacherMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Teacher.MaximumHoursDaily == -1)
            {
                return true;
            }
            else
            {
                int nrOfHours = 0;
                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                foreach (var slot in day.Slots)
                {
                    foreach (var act in slot.Activities)
                    {
                        if (act.Teacher.Equals(activity.Teacher) && act.Scheduled)
                        {
                            nrOfHours++;
                            break;
                        }
                    }
                }
                nrOfHours++; //for this activity
                if (nrOfHours > activity.Teacher.MaximumHoursDaily)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintTeachersMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxHoursDaily == -1)
            {
                return true;
            }

            int maxHours = timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxHoursDaily;

            //count 
            int count = 0; int h = -1;
            var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
            foreach (var slot in day.Slots)
            {
                h++;
                if (h == desiredHour)
                {
                    count++;
                    continue;
                }
                foreach (var act in slot.Activities)
                {
                    if (act.Scheduled && act.Teacher.Equals(activity.Teacher))
                    {
                        count++;
                        break;
                    }
                }
            }

            if (maxHours >= count)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
        private static bool AcceptableForConstraintTeacherActivityTagMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Teacher.TeacherActivityTagMaxHoursDailyList.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (var constr in activity.Teacher.TeacherActivityTagMaxHoursDailyList)
                {
                    if (constr.ActivityTag.Equals(activity.ActivityTag))
                    {
                        int nrOfHours = 0;
                        var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                        foreach (var slot in day.Slots)
                        {
                            foreach (var act in slot.Activities)
                            {
                                if (act.Teacher.Equals(activity.Teacher) && act.Scheduled &&
                                    act.ActivityTag != null && act.ActivityTag.Equals(activity.ActivityTag))
                                {
                                    nrOfHours++;
                                    break;
                                }
                            }
                        }
                        nrOfHours++; //for this activity
                        if (nrOfHours > constr.MaximumHoursDaily)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool AcceptableForConstraintTeachersActivityTagMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintsTeachers.TeachersActivityTagMaxHoursDailyList)
            {
                if (activity.ActivityTag != null && activity.ActivityTag.Equals(constr.ActivityTag))
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    int contHours = 0;
                    for (int i = 0; i < day.Slots.Count() - 1; i++)
                    {
                        var slot = day.Slots.ElementAt(i);
                        if (i == desiredHour)
                        {
                            contHours++;
                            continue;
                        }

                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && activity.ActivityTag.Equals(act.ActivityTag) &&
                                act.Teacher.Equals(activity.Teacher))
                            {
                                contHours++;
                                break;
                            }
                        }
                    }
                    if (contHours > constr.MaximumHoursDaily)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private static bool AcceptableForConstraintStudentsSetActivityTagMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {           
            
            foreach (var constr in activity.Students.StudentsSetActivityTagMaxHoursDailyList)
            {
                if (constr.ActivityTag.Equals(activity.ActivityTag))
                {
                    int maxHours = constr.MaximumHoursDaily;

                    //count 
                    int count = 0; int h = -1;
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    foreach (var slot in day.Slots)
                    {
                        h++;
                        if (h == desiredHour)
                        {
                            count++;
                            continue;
                        }
                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && act.Students.Equals(activity.Students) &&
                                act.ActivityTag != null && act.ActivityTag.Equals(activity.ActivityTag))
                            {
                                count++;
                                break;
                            }
                        }
                    }

                    if (maxHours <= count)
                    {
                        return false;
                    }
                }
            }


            return true;
        }


        private static bool AcceptableForConstraintStudentsSetMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (activity.Students.MaximumHoursDaily == -1)
            {
                return true;
            }


            int maxHours = activity.Students.MaximumHoursDaily;

            //count 
            int count = 0; int h = -1;
            var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
            foreach (var slot in day.Slots)
            {
                h++;
                if (h == desiredHour)
                {
                    count++;
                    continue;
                }
                foreach (var act in slot.Activities)
                {
                    if (act.Scheduled && act.Students.Equals(activity.Students))
                    {
                        count++;
                        break;
                    }
                }
            }

            if (maxHours <= count)
            {
                return false;
            }



            return true;
        }

        private static bool AcceptableForConstraintStudentsMaxHoursDaily(Timetable timetable,Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursDaily == -1)
            {
                return true;
            }

            int maxHours = timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursDaily;

            //count 
            int count = 0; int h = -1;
            var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
            foreach (var slot in day.Slots)
            {
                h++;
                if(h == desiredHour)
                {
                    count++;
                    continue;
                }
                foreach (var act in slot.Activities)
                {
                    if (act.Scheduled && act.Students.Equals(activity.Students))
                    {
                        count++;
                        break;
                    }
                }
            }

            if (maxHours >= count )
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        private static bool AcceptableForConstraintStudentsSetMaxGapsPerWeek(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (activity.Students.MaxGapsPerWeek == -1)
            {
                return true;
            }
            else
            {

                int nrOfGaps = 0;
                int d = -1;
                foreach (var day in timetable.LastGeneratedTimetable.Days)
                {
                    bool counting = false;
                    int lastHourOfTheDay = -1;
                    int h = -1;
                    d++;
                    foreach (var hour in day.Slots)
                    {
                        h++;
                        var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                        if (interserct.Count() > 0)
                        {
                            bool aConcurrentActivityHasFound = false;
                            foreach (var inter in interserct)
                            {
                                if (inter.Scheduled)
                                {
                                    counting = true;
                                    aConcurrentActivityHasFound = true;
                                    lastHourOfTheDay = h;
                                    break;
                                }
                            }
                            if (aConcurrentActivityHasFound)
                            {
                                continue;
                            }
                        }
                        //we want to schedule here this activity
                        else if (h == desiredHour && desiredDay == d)
                        {
                            counting = true;
                            lastHourOfTheDay = h;
                            continue;
                        }
                        if (counting)
                        {
                            nrOfGaps++;
                        }
                    }

                    //remove final hours
                    if (counting)
                    {
                        int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                        nrOfGaps -= dif;
                    }
                }
                if (nrOfGaps > activity.Students.MaxGapsPerWeek)
                {
                    return false;
                }

            }
            return true;
        }

        private static bool AcceptableForConstraintStudentsMaxGapsPerWeek(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerWeek == -1)
            {
                return true;
            }
            else
            {
                

                    int nrOfGaps = 0;
                    int d = -1;
                    foreach (var day in timetable.LastGeneratedTimetable.Days)
                    {
                        bool counting = false;
                        int lastHourOfTheDay = -1;
                        int h = -1;
                        d++;
                        foreach (var hour in day.Slots)
                        {
                            h++;
                            var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                            if (interserct.Count() > 0)
                            {
                                bool aConcurrentActivityHasFound = false;
                                foreach (var inter in interserct)
                                {
                                    if (inter.Scheduled && inter.Students.Equals(activity.Students))
                                    {
                                        counting = true;
                                        aConcurrentActivityHasFound = true;
                                        lastHourOfTheDay = h;
                                        break;
                                    }
                                }
                                if (aConcurrentActivityHasFound)
                                {
                                    continue;
                                }
                            }
                            //we want to schedule here this activity
                            if (h == desiredHour && desiredDay == d)
                            {
                                counting = true;
                                lastHourOfTheDay = h;
                                continue;
                            }
                            if (counting)
                            {
                                nrOfGaps++;
                            }
                        }

                        //remove final hours
                        if (counting)
                        {
                            int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                            nrOfGaps -= dif;
                        }
                    }
                    if (nrOfGaps > timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerWeek)
                    {
                        return false;
                    }

                
            }
            return true;
        }


        private static bool AcceptableForConstraintStudentsSetMaxGapsPerDay(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (activity.Students.MaxGapsPerDay == -1)
            {
                return true;
            }
            else
            {
                
                   
                        int nrOfGaps = 0;
                        int d = -1;
                        var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                        {
                            bool counting = false;
                            int lastHourOfTheDay = -1;
                            int h = -1;
                            d++;
                            foreach (var hour in day.Slots)
                            {
                                h++;
                                var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                                if (interserct.Count() > 0)
                                {
                                    bool aConcurrentActivityHasFound = false;
                                    foreach (var inter in interserct)
                                    {
                                        if (inter.Scheduled && inter.Students.Equals(activity.Students))
                                        {
                                            counting = true;
                                            aConcurrentActivityHasFound = true;
                                            lastHourOfTheDay = h;
                                            break;
                                        }
                                    }
                                    if (aConcurrentActivityHasFound)
                                    {
                                        continue;
                                    }
                                }
                                //we want to schedule here this activity
                                else if (h == desiredHour && desiredDay == d)
                                {
                                    counting = true;
                                    lastHourOfTheDay = h;
                                    continue;
                                }
                                if (counting)
                                {
                                    nrOfGaps++;
                                }
                            }

                            //remove final hours
                            if (counting)
                            {
                                int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                                nrOfGaps -= dif;
                            }
                        }
                        if (nrOfGaps > activity.Students.MaxGapsPerDay)
                        {
                            return false;
                        }
                   
            }
            return true;
        }


        private static bool AcceptableForConstraintStudentsMaxGapsPerDay(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerDay == -1)
            {
                return true;
            }
            else
            {
                
                    // exists this type of restriction for this activity
                    //if (con.Teacher.Equals(activity.Teacher))
                    //{
                    int nrOfGaps = 0;
                    int d = -1;
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    {
                        bool counting = false;
                        int lastHourOfTheDay = -1;
                        int h = -1;
                        d++;
                        foreach (var hour in day.Slots)
                        {
                            h++;
                            var interserct = hour.Activities.Intersect(activity.ConcurrentActivities);
                            if (interserct.Count() > 0)
                            {
                                bool aConcurrentActivityHasFound = false;
                                foreach (var inter in interserct)
                                {
                                    if (inter.Scheduled && inter.Students.Equals(activity.Students))
                                    {
                                        counting = true;
                                        aConcurrentActivityHasFound = true;
                                        lastHourOfTheDay = h;
                                        break;
                                    }
                                }
                                if (aConcurrentActivityHasFound)
                                {
                                    continue;
                                }
                            }
                            //we want to schedule here this activity
                            else if (h == desiredHour && desiredDay == d)
                            {
                                counting = true;
                                lastHourOfTheDay = h;
                                continue;
                            }
                            if (counting)
                            {
                                nrOfGaps++;
                            }
                        }

                        //remove final hours
                        if (counting)
                        {
                            int dif = timetable.HoursList.Count() - 1 - lastHourOfTheDay;
                            nrOfGaps -= dif;
                        }
                    }
                    if (nrOfGaps > timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerDay)
                    {
                        return false;
                    }
                    //}
                
            }
            return true;
        }



        private static bool AcceptableForConstraintActivityEndsStudentsDay(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintActivityEndsStudentsDayList)
            {
                if (constr.ActivityId == activity.ActivityId)
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    for (int i = day.Slots.Count()-1; i > desiredHour ; i--)
                    {
                        var slot = day.Slots.ElementAt(i);
                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && act.Students.Equals(activity.Students))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintActivitiesEndStudentsDay(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesEndStudentsDayList)
            {
                if (constr.Teacher.Equals(activity.Teacher) &&
                    constr.Students.Equals(activity.Students) &&
                    constr.Subject.Equals(activity.Subject) &&
                    constr.ActivityTag != null && activity.ActivityTag.Equals(activity.ActivityTag))
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    for (int i = day.Slots.Count() - 1; i > desiredHour; i--)
                    {
                        var slot = day.Slots.ElementAt(i);
                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && act.Students.Equals(activity.Students) &&
                                act.Teacher.Equals(activity.Teacher) && act.Subject.Equals(activity.Subject))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        private static bool AcceptableForConstraintActivitiesSameStartingDay(Timetable timetable, Activity activity, int desiredDay)
        {
            if (timetable.TimeConstraints.ConstraintActivitiesSameStartingDayList.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesSameStartingDayList)
                {
                    if (constr.ActivityIds.Contains(activity.ActivityId))
                    {
                        foreach (var act in timetable.ActivityList)
                        {   int h;
                            if(constr.ActivityIds.Contains(act.ActivityId))
                            {
                                if (act.Scheduled && !(timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h) == desiredDay))
                                {
                                    return false;
                                }
                                else if (!act.Scheduled)
                                {
                                    bool spotFound = false;
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1); i++)
                                    {
                                        if (act.AcceptableTimeSlots[desiredDay, i] > 0)
                                        {
                                            spotFound = true;
                                            break;
                                        }
                                    }
                                    if (!spotFound)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintActivitiesSameStartingHour(Timetable timetable, Activity activity, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintActivitiesSameStartingHourList.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesSameStartingHourList)
                {
                    if (constr.ActivityIds.Contains(activity.ActivityId))
                    {
                        foreach (var act in timetable.ActivityList)
                        {
                            int h;
                            if (constr.ActivityIds.Contains(act.ActivityId))
                            {
                                timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                                if (act.Scheduled && !( h == desiredHour))
                                {
                                    return false;
                                }
                                else if (!act.Scheduled)
                                {
                                    bool spotFound = false;
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                    {
                                        if (act.AcceptableTimeSlots[i, desiredHour] > 0)
                                        {
                                            spotFound = true;
                                            break;
                                        }
                                    }
                                    if (!spotFound)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintActivitiesSameStartingTime(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintActivitiesSameStartingHourList.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesSameStartingHourList)
                {
                    if (constr.ActivityIds.Contains(activity.ActivityId))
                    {
                        foreach (var act in timetable.ActivityList)
                        {
                            int h;
                            if (constr.ActivityIds.Contains(act.ActivityId))
                            {
                                int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                                if (act.Scheduled && (!(d==desiredDay) || !(h == desiredHour)))
                                {
                                    return false;
                                }
                                else if (!act.Scheduled)
                                {
                                    bool spotFound = false;

                                    if (act.AcceptableTimeSlots[desiredDay, desiredHour] > 0)
                                    {
                                        spotFound = true;
                                        break;
                                    }
                                    
                                    if (!spotFound)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }


        private static bool AcceptableForConstraintStudentsActivityTagMaxHoursDaily(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList)
            {
                if (activity.ActivityTag != null && activity.ActivityTag.Equals(constr.ActivityTag))
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    int contHours = 0;
                    for (int i = 0; i < day.Slots.Count() - 1; i++)
                    {
                        var slot = day.Slots.ElementAt(i);
                        if (i == desiredHour)
                        {
                            contHours++;
                            continue;
                        }

                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && activity.ActivityTag.Equals(act.ActivityTag) &&
                                act.Students.Equals(activity.Students))
                            {
                                contHours++;
                                break;
                            }
                        }                        
                    }
                    if (contHours > constr.MaximumHoursDaily)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private static bool AcceptableForConstraintStudentsActivityTagMaxHoursContinuously(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach(var constr in timetable.TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList)
            {
                if (activity.ActivityTag != null && activity.ActivityTag.Equals(constr.ActivityTag))
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    int contHours = 0;
                    for (int i = 0; i < day.Slots.Count()-1; i++)
                    {
                        var slot = day.Slots.ElementAt(i);
                        bool cont = false;
                        if (i == desiredHour)
                        {
                            contHours++;
                            continue;
                        }

                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && activity.ActivityTag.Equals(act.ActivityTag) &&
                                act.Students.Equals(activity.Students))
                            {
                                contHours++;
                                cont = true;
                                break;
                            }      
                        }

                        if (contHours > constr.MaximumHoursContinuously)
                        {
                            return false;
                        }
                        if (!cont)
                        {
                            contHours = 0;
                        }
                    }
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintTeachersActivityTagMaxHoursContinuously(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintsTeachers.TeachersActivityTagMaxHoursContinuouslyList)
            {
                if (activity.ActivityTag != null && activity.ActivityTag.Equals(constr.ActivityTag))
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    int contHours = 0;
                    for (int i = 0; i < day.Slots.Count() - 1; i++)
                    {
                        var slot = day.Slots.ElementAt(i);
                        bool cont = false;
                        if (i == desiredHour)
                        {
                            contHours++;
                            continue;
                        }

                        foreach (var act in slot.Activities)
                        {
                            if (act.Scheduled && activity.ActivityTag.Equals(act.ActivityTag) &&
                                act.Teacher.Equals(activity.Teacher))
                            {
                                contHours++;
                                cont = true;
                                break;
                            }
                        }

                        if (contHours > constr.MaximumHoursContinuously)
                        {
                            return false;
                        }
                        if (!cont)
                        {
                            contHours = 0;
                        }
                    }
                }
            }
            return true;
        }
               

        private static bool AcceptableForConstraintTeacherMaxHoursContinuously(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in activity.Teacher.TeacherActivityTagMaxHoursContinuouslyList)
            {

                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                int contHours = 0;
                for (int i = 0; i < day.Slots.Count() - 1; i++)
                {
                    var slot = day.Slots.ElementAt(i);
                    bool cont = false;
                    if (i == desiredHour)
                    {
                        contHours++;
                        continue;
                    }

                    foreach (var act in slot.Activities)
                    {
                        if (activity.Teacher.Equals(act.Teacher))
                        {
                            contHours++;
                            cont = true;
                            break;
                        }
                    }

                    if (contHours > constr.MaximumHoursContinuously)
                    {
                        return false;
                    }
                    if (!cont)
                    {
                        contHours = 0;
                    }
                }

            }
            return true;
        }

        private static bool AcceptableForConstraintTeachersMaxHoursContinuously(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {

            if (Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMaxHoursContinuously == -1)
            {
                return true;
            }

                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                int contHours = 0;
                for (int i = 0; i < day.Slots.Count() - 1; i++)
                {
                    var slot = day.Slots.ElementAt(i);
                    bool cont = false;
                    if (i == desiredHour)
                    {
                        contHours++;
                        continue;
                    }

                    foreach (var act in slot.Activities)
                    {
                        if (activity.Teacher.Equals(act.Teacher))
                        {
                            contHours++;
                            cont = true;
                            break;
                        }
                    }

                    if (contHours > Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMaxHoursContinuously)
                    {
                        return false;
                    }
                    if (!cont)
                    {
                        contHours = 0;
                    }
                }

            
            return true;
        }

        private static bool AcceptableForConstraintStudentsMaxHoursContinuously(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursContinuously == -1)
            {
                return true;
            }

                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                int contHours = 0;
                for (int i = 0; i < day.Slots.Count() - 1; i++)
                {
                    var slot = day.Slots.ElementAt(i);
                    bool cont = false;
                    if (i == desiredHour)
                    {
                        contHours++;
                        continue;
                    }

                    foreach (var act in slot.Activities)
                    {
                        if (activity.Students.Equals(act.Students))
                        {
                            contHours++;
                            cont = true;
                            break;
                        }
                    }

                    if (contHours > timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursContinuously)
                    {
                        return false;
                    }
                    if (!cont)
                    {
                        contHours = 0;
                    }
                }

            
            return true;
        }


        private static bool AcceptableForConstraintMinDaysBetweenActivities(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintMinDaysBetweenActivitiesList)
            {
                if (constr.ActivityIds.Contains(activity.ActivityId))
                {
                    int minDays = constr.MinDays;
                    for (int i = 0; i < timetable.LastGeneratedTimetable.Days.Count() - 1; i++)
                    {
                        
                        if (i == desiredDay)
                        {
                            continue;
                        }
                        foreach (int id in constr.ActivityIds)
                        {
                            var acts = from a in timetable.ActivityList
                                      where a.ActivityId == id && a.Scheduled == true
                                      select a;
                            if (acts.Count() > 0)
                            {
                                int h = 0;
                                int progDay = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(acts.First(), out h);

                                if (Math.Abs(desiredDay - progDay) < minDays &&
                                    !(constr.ConsecutiveIfSameDay && desiredDay == progDay && Math.Abs(desiredHour - h) <2))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                }
            }
            return true;
        }

        private static bool AcceptableForConstraintMinGapsBetweenActivities(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintMinGapsBetweenActivitiesList)
            {
                if (constr.ActivityIds.Contains(activity.ActivityId))
                {
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    int concurrentActivityPoz = -1;
                    for (int i=0;i<day.Slots.Count()-1;i++)
                    {
                        var slot = day.Slots.ElementAt(i);
                        for(int j=0; j< slot.Activities.Count()-1;j++)
                        {
                            var act = slot.Activities.ElementAt(j);
                            if (act.Scheduled && constr.ActivityIds.Contains(act.ActivityId))
                            {
                                concurrentActivityPoz = j;
                                break;
                            }
                        }
                        if (Math.Abs(desiredHour - concurrentActivityPoz) < constr.MinGaps)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }



        private static bool AcceptableForConstraintTeacherIntervalMaxDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Teacher.TeacherIntervalMaxDaysPerWeek.MaxDaysPerWeek == -1)
            {
                return true;
            }

            bool acceptable = false;

            HashSet<int> setOfDays = new HashSet<int>();

            var con = activity.Teacher.TeacherIntervalMaxDaysPerWeek;
            int startHour = timetable.HoursList.IndexOf(con.IntervalStartHour);
            int endHour;
            if (string.IsNullOrEmpty(con.IntervalEndHour))
            {
                endHour = timetable.HoursList.Count - 1;
            }
            else
            {
                endHour = timetable.HoursList.IndexOf(con.IntervalEndHour);
            }


            //check activities that have the same teacher
            foreach (var act in activity.ConcurrentActivities)
            {
                if (act.Teacher.Equals(activity.Teacher))
                {
                    //count days to see if constraints are feasible
                    if (act.Scheduled)
                    {
                        int h;
                        int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                        setOfDays.Add(d);
                    }
                    //this activity is not scheduled in that case determine an appropriate day possible for this activity to be scheduled in
                    else
                    {
                        bool dayFound = false;
                        for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                        {
                            for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                            {
                                if (act.AcceptableTimeSlots[i, j] > 0)
                                {
                                    if (setOfDays.Contains(i) && i >= startHour && i <= endHour)
                                    {
                                        dayFound = true;
                                    }

                                }
                            }
                        }
                        if (!dayFound)
                        {
                            for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1) && !dayFound; i++)
                            {
                                if (act.AcceptableTimeSlots[desiredDay, i] > 0 && i >= startHour && i <= endHour)
                                {
                                    dayFound = true;
                                    setOfDays.Add(desiredDay);
                                }
                            }
                            if (!dayFound)
                            {
                                for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                {
                                    for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1); j++)
                                    {
                                        if (act.AcceptableTimeSlots[i, j] > 0 && i >= startHour && i <= endHour)
                                        {
                                            setOfDays.Add(i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (setOfDays.Count <= con.MaxDaysPerWeek)
            {
                acceptable = true;
            }
            else
            {
                return false;
            }



            return acceptable;
        }

        private static bool AcceptableForConstraintTeachersIntervalMaxDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            bool acceptable = false;


            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersIntervalMaxDaysPerWeek.MaxDaysPerWeek == -1)
            {
                return true;
            }
            else
            {
                HashSet<int> setOfDays = new HashSet<int>();

                var con = timetable.TimeConstraints.ConstraintsTeachers.TeachersIntervalMaxDaysPerWeek;
                    int startHour = timetable.HoursList.IndexOf(con.IntervalStartHour);
                    int endHour;
                    if (string.IsNullOrEmpty(con.IntervalEndHour))
                    {
                        endHour = timetable.HoursList.Count - 1;
                    }
                    else
                    {
                        endHour = timetable.HoursList.IndexOf(con.IntervalEndHour);
                    }

                    foreach (var act in activity.ConcurrentActivities)
                    {
                        if (act.Teacher.Equals(activity.Teacher))
                        {
                            //count days to see if constraints are feasible
                            if (act.Scheduled)
                            {
                                int h;
                                int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                                setOfDays.Add(d);
                            }
                            //this activity is not scheduled in that case determine an appropriate day possible for this activity to be scheduled in
                            else
                            {
                                bool dayFound = false;
                                for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                                {
                                    for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                                    {
                                        if (act.AcceptableTimeSlots[i, j] > 0)
                                        {
                                            if (setOfDays.Contains(i) && i >= startHour && i <= endHour)
                                            {
                                                dayFound = true;
                                            }

                                        }
                                    }
                                }
                                if (!dayFound)
                                {
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1) && !dayFound; i++)
                                    {
                                        if (act.AcceptableTimeSlots[desiredDay, i] > 0 && i >= startHour && i <= endHour)
                                        {
                                            dayFound = true;
                                            setOfDays.Add(desiredDay);
                                        }
                                    }
                                    if (!dayFound)
                                    {
                                        for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                        {
                                            for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1); j++)
                                            {
                                                if (act.AcceptableTimeSlots[i, j] > 0 && i >= startHour && i <= endHour)
                                                {
                                                    setOfDays.Add(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    if (setOfDays.Count <= con.MaxDaysPerWeek)
                    {
                        acceptable = true;
                    }
                    else
                    {
                        return false;
                    }
                
            }

            return acceptable;
        }


        private static bool AcceptableForConstraintStudentsSetIntervalMaxDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Students.StudentsSetIntervalMaxDaysPerWeek.MaxDaysPerWeek == -1)
            {
                return true;
            }

            var con = activity.Students.StudentsSetIntervalMaxDaysPerWeek;
            int startHour = timetable.HoursList.IndexOf(con.IntervalStartHour);
            int endHour;
            HashSet<int> setOfDays = new HashSet<int>();

            if (string.IsNullOrEmpty(con.IntervalEndHour))
            {
                endHour = timetable.HoursList.Count - 1;
            }
            else
            {
                endHour = timetable.HoursList.IndexOf(con.IntervalEndHour);
            }

            // exists this type of restriction for this activity
            if (con.MaxDaysPerWeek > -1)
            {

                //check activities that have the same teacher
                foreach (var act in activity.ConcurrentActivities)
                {
                    if (act.Students.Equals(activity.Students))
                    {
                        //count days to see if constraints are feasible
                        if (act.Scheduled)
                        {
                            int h;
                            int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                            setOfDays.Add(d);
                        }
                        //this activity is not scheduled in that case determine an appropriate day possible for this activity to be scheduled in
                        else
                        {
                            bool dayFound = false;
                            for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                            {
                                for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                                {
                                    if (act.AcceptableTimeSlots[i, j] > 0)
                                    {
                                        if (setOfDays.Contains(i) && i >= startHour && i <= endHour)
                                        {
                                            dayFound = true;
                                        }

                                    }
                                }
                            }
                            if (!dayFound)
                            {
                                for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1) && !dayFound; i++)
                                {
                                    if (act.AcceptableTimeSlots[desiredDay, i] > 0 && i >= startHour && i <= endHour)
                                    {
                                        dayFound = true;
                                        setOfDays.Add(desiredDay);
                                    }
                                }
                                if (!dayFound)
                                {
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1); j++)
                                        {
                                            if (act.AcceptableTimeSlots[i, j] > 0 && i >= startHour && i <= endHour)
                                            {
                                                setOfDays.Add(i);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (setOfDays.Count <= con.MaxDaysPerWeek)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static bool AcceptableForConstraintStudentsIntervalMaxDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {

            if (timetable.TimeConstraints.ConstraintsStudents.StudentsIntervalMaxDaysPerWeek.MaxDaysPerWeek == -1)
            {
                return true;
            }
            else
            {
                HashSet<int> setOfDays = new HashSet<int>();


                var con = timetable.TimeConstraints.ConstraintsStudents.StudentsIntervalMaxDaysPerWeek;
                int startHour = timetable.HoursList.IndexOf(con.IntervalStartHour);
                int endHour;
                if (string.IsNullOrEmpty(con.IntervalEndHour))
                {
                    endHour = timetable.HoursList.Count - 1;
                }
                else
                {
                    endHour = timetable.HoursList.IndexOf(con.IntervalEndHour);
                }

                foreach (var act in activity.ConcurrentActivities)
                {
                    if (act.Students.Equals(activity.Students))
                    {
                        //count days to see if constraints are feasible
                        if (act.Scheduled)
                        {
                            int h;
                            int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                            setOfDays.Add(d);
                        }
                        //this activity is not scheduled in that case determine an appropriate day possible for this activity to be scheduled in
                        else
                        {
                            bool dayFound = false;
                            for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                            {
                                for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                                {
                                    if (act.AcceptableTimeSlots[i, j] > 0)
                                    {
                                        if (setOfDays.Contains(i) && i >= startHour && i <= endHour)
                                        {
                                            dayFound = true;
                                        }

                                    }
                                }
                            }
                            if (!dayFound)
                            {
                                for (int i = 0; i < act.AcceptableTimeSlots.GetLength(1) && !dayFound; i++)
                                {
                                    if (act.AcceptableTimeSlots[desiredDay, i] > 0 && i >= startHour && i <= endHour)
                                    {
                                        dayFound = true;
                                        setOfDays.Add(desiredDay);
                                    }
                                }
                                if (!dayFound)
                                {
                                    for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1); j++)
                                        {
                                            if (act.AcceptableTimeSlots[i, j] > 0 && i >= startHour && i <= endHour)
                                            {
                                                setOfDays.Add(i);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                if (setOfDays.Count <= con.MaxDaysPerWeek)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }



        private static bool AcceptableForConstraintActivitiesOccupyMaxTimeSlotsFromSelection(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList.Count == 0)
            {
                return true;
            }
            else
            {
                HashSet<int> setOfTimeSlots = new HashSet<int>();

                foreach (var con in timetable.TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList)
                {

                    if (con.ActivityIds.Contains(activity.ActivityId))
                    {
                        bool validTimeSlot = false;
                        foreach (var timeslot in con.SelectedTimeSlots)
                        {
                            int d = timetable.DaysList.IndexOf(timeslot.Day);
                            int h = timetable.HoursList.IndexOf(timeslot.Hour);
                            if (d == desiredDay && h == desiredHour)
                            {
                                validTimeSlot = true;
                                break;
                            }
                        }

                        if (!validTimeSlot)
                        {
                            return false;
                        }

                        //count
                        foreach (var act in timetable.ActivityList)
                        {
                            if (act.Scheduled)
                            {
                                setOfTimeSlots.Add(act.PozScheduled);
                            }
                        }
                        //add this poz
                        setOfTimeSlots.Add(desiredDay * 10 + desiredHour);

                    }

                    if (con.MaxNumberOfOccupiedTimeSlots < setOfTimeSlots.Count())
                    {
                        return false;
                    }

                }
            }

            return true;
        }

        #region min

        private static bool AcceptableForConstraintTeacherMinDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Teacher.MinimumDaysPerWeek == -1)
            {
                return true;
            }
            HashSet<int> setOfDays = new HashSet<int>();



            setOfDays.Add(desiredDay);
            //same teacher
            foreach (var act in activity.ConcurrentActivities)
            {
                if (act.Teacher.Equals(activity.Teacher))
                {
                    //count days to see if constraints is feasible
                    if (act.Scheduled)
                    {
                        int h;
                        int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                        setOfDays.Add(d);
                        if (setOfDays.Count() >= activity.Teacher.MinimumDaysPerWeek)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        bool dayFound = false;
                        for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                        {
                            for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                            {
                                if (act.AcceptableTimeSlots[i, j] > 0 && !setOfDays.Contains(i))
                                {
                                    dayFound = true;
                                    setOfDays.Add(i);

                                }
                            }
                        }
                        if (setOfDays.Count() >= activity.Teacher.MinimumDaysPerWeek)
                        {
                            return true;
                        }
                    }

                }
            }

            if (setOfDays.Count >= activity.Teacher.MinimumDaysPerWeek)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool AcceptableForConstraintTeachersMinDaysPerWeek(Timetable timetable, Activity activity, int desiredDay)
        {
            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersMinDaysPerWeek == -1)
            {
                return true;
            }
            else
            {
                HashSet<int> setOfDays = new HashSet<int>();


                setOfDays.Add(desiredDay);
                //same teacher
                foreach (var act in activity.ConcurrentActivities)
                {
                    if (act.Teacher.Equals(activity.Teacher))
                    {
                        //count days to see if constraints is feasible
                        if (act.Scheduled)
                        {
                            int h;
                            int d = timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act, out h);
                            setOfDays.Add(d);
                            if (setOfDays.Count() >= timetable.TimeConstraints.ConstraintsTeachers.TeachersMinDaysPerWeek)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            bool dayFound = false;
                            for (int i = 0; i < act.AcceptableTimeSlots.GetLength(0) && !dayFound; i++)
                            {
                                for (int j = 0; j < act.AcceptableTimeSlots.GetLength(1) && !dayFound; j++)
                                {
                                    if (act.AcceptableTimeSlots[i, j] > 0 && !setOfDays.Contains(i))
                                    {
                                        dayFound = true;
                                        setOfDays.Add(i);

                                    }
                                }
                            }
                            if (setOfDays.Count() >= timetable.TimeConstraints.ConstraintsTeachers.TeachersMinDaysPerWeek)
                            {
                                return true;
                            }
                        }

                    }
                }

                if (setOfDays.Count >= timetable.TimeConstraints.ConstraintsTeachers.TeachersMinDaysPerWeek)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        private static bool AcceptableForConstraintTeacherMinHoursDaily(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Teacher.MinHoursDaily == null || activity.Teacher.MinHoursDaily.MinimumHoursDaily == -1)
            {
                return true;
            }

            var constr = activity.Teacher.MinHoursDaily;
            HashSet<int> countHours = new HashSet<int>();
           
                int h = -1;
                int alreadyScheduledInThisDay = 0;
                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                foreach (var slot in day.Slots)
                {
                    h++;
                    foreach (var act in slot.Activities)
                    {
                        if (act.Teacher.Equals(activity.Teacher))
                        {
                            if (act.Scheduled)
                            {
                                countHours.Add(h);
                                alreadyScheduledInThisDay++;
                                break;
                            }
                            else if (act.AcceptableTimeSlots[desiredDay, h] > 0)
                            {
                                countHours.Add(h);
                            }
                        }

                    }
                }

                if (countHours.Count() < constr.MinimumHoursDaily)
                {
                    return false;
                }
                if (!constr.AllowEmptyDays)
                {
                    int countActivities = (from a in timetable.ActivityList
                                           where a.Students.Equals(activity.Students)
                                           select a).Count();
                    int maxAllowed = countActivities / timetable.DaysList.Count();
                    while (maxAllowed * timetable.DaysList.Count() < countActivities)
                    {
                        maxAllowed++;
                    }

                    if (alreadyScheduledInThisDay >= maxAllowed)
                    {
                        return false;
                    }
                }
           



            return true;
        }

        private static bool AcceptableForConstraintTeachersMinHoursDaily(Timetable timetable, Activity activity, int desiredDay)
        {
            if (timetable.TimeConstraints.ConstraintsTeachers.TeachersMinHoursDaily == -1)
            {
                return true;
            }
            else
            {
                
                    HashSet<int> countHours = new HashSet<int>();

                    int h = -1;
                    int alreadyScheduledInThisDay = 0;
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    foreach (var slot in day.Slots)
                    {
                        h++;
                        foreach (var act in slot.Activities)
                        {
                            if (act.Teacher.Equals(activity.Teacher))
                            {
                                if (act.Scheduled)
                                {
                                    countHours.Add(h);
                                    alreadyScheduledInThisDay++;
                                    break;
                                }
                                else if (act.AcceptableTimeSlots[desiredDay, h] > 0)
                                {
                                    countHours.Add(h);
                                }
                            }

                        }
                    }

                    if (countHours.Count() < timetable.TimeConstraints.ConstraintsTeachers.TeachersMinHoursDaily)
                    {
                        return false;
                    }

                //not yet implemented in new aproach
                    //if (!constr.AllowEmptyDays)
                    //{
                    //    int countActivities = (from a in timetable.ActivityList
                    //                           where a.Students.Equals(activity.Students)
                    //                           select a).Count();
                    //    int maxAllowed = countActivities / timetable.DaysList.Count();
                    //    while (maxAllowed * timetable.DaysList.Count() < countActivities)
                    //    {
                    //        maxAllowed++;
                    //    }

                    //    if (alreadyScheduledInThisDay >= maxAllowed)
                    //    {
                    //        return false;
                    //    }
                    //}
                

            }
            return true;
        }



        private static bool AcceptableForConstraintStudentsSetMinHoursDaily(Timetable timetable, Activity activity, int desiredDay)
        {
            if (activity.Students.MinHoursDaily.MinimumHoursDaily == -1)
            {
                return true;
            }
            else
            {
                HashSet<int> countHours = new HashSet<int>();
                int alreadyScheduledInThisDay = 0;

                int h = -1;

                var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                foreach (var slot in day.Slots)
                {
                    h++;
                    foreach (var act in slot.Activities)
                    {
                        if (act.Students.Equals(activity.Students))
                        {
                            if (act.Scheduled)
                            {
                                countHours.Add(h);
                                alreadyScheduledInThisDay++;
                                break;
                            }
                            else if (act.AcceptableTimeSlots[desiredDay, h] > 0)
                            {
                                countHours.Add(h);
                            }
                        }

                    }
                }

                if (countHours.Count() < activity.Students.MinHoursDaily.MinimumHoursDaily)
                {
                    return false;
                }
                if (!activity.Students.MinHoursDaily.AllowEmptyDays)
                {
                    int countActivities = (from a in timetable.ActivityList
                                           where a.Students.Equals(activity.Students)
                                           select a).Count();
                    int maxAllowed = countActivities / timetable.DaysList.Count();
                    while (maxAllowed * timetable.DaysList.Count() < countActivities)
                    {
                        maxAllowed++;
                    }

                    if (alreadyScheduledInThisDay >= maxAllowed)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        private static bool AcceptableForConstraintStudentsMinHoursDaily(Timetable timetable, Activity activity, int desiredDay)
        {
            if (timetable.TimeConstraints.ConstraintsStudents.StudentsMinHoursDaily == -1)
            {
                return true;
            }
            else
            {
                
                    HashSet<int> countHours = new HashSet<int>();

                    int h = -1;
                    int alreadyScheduledInThisDay = 0;
                    var day = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay);
                    foreach (var slot in day.Slots)
                    {
                        h++;
                        foreach (var act in slot.Activities)
                        {
                            if (act.Students.Equals(activity.Students))
                            {
                                if (act.Scheduled)
                                {
                                    countHours.Add(h);
                                    alreadyScheduledInThisDay++;
                                    break;
                                }
                                else if (act.AcceptableTimeSlots[desiredDay, h] > 0)
                                {
                                    countHours.Add(h);
                                }
                            }

                        }
                    }

                    if (countHours.Count() < timetable.TimeConstraints.ConstraintsStudents.StudentsMinHoursDaily)
                    {
                        return false;
                    }
                    
                //not yet implemented in new aproach
                    //if(!constr.AllowEmptyDays)
                    //{
                    //    int countActivities = (from a in timetable.ActivityList
                    //                      where a.Students.Equals(activity.Students)
                    //                      select a).Count();
                    //    int maxAllowed = countActivities / timetable.DaysList.Count();
                    //    while (maxAllowed * timetable.DaysList.Count() < countActivities)
                    //    {
                    //        maxAllowed++;
                    //    }

                    //    if (alreadyScheduledInThisDay >= maxAllowed)
                    //    {
                    //        return false;
                    //    }
                    //}
                    

                

            }
            return true;
        }

        private static bool AcceptableForConstraintStudentsEarlyMaxBeginningsAtSecondHour(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintsStudents.StudentsEarlyMaxBeginningsAtSecondHour == -1)
            {
                return true;
            }
            else
            {
                if (desiredHour != 1)
                {
                    return true;
                }
                var sl = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay).Slots.ElementAt(0);
                foreach (var act in sl.Activities)
                {
                    if (act.Students.Equals(activity.Students) && (act.Scheduled || act.AcceptableTimeSlots[desiredDay, 0] > 0))
                    {
                        return true;
                    }
                }




                int count = 0;
                for (int i = 0; i < timetable.LastGeneratedTimetable.Days.Count() - 1; i++)
                {
                    if (desiredDay == i)
                    {
                        continue;
                    }
                    var slot = timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(0);
                    bool startAtSecond = true;
                    foreach (var act in slot.Activities)
                    {
                        if (act.Students.Equals(activity.Students) && act.Scheduled)
                        {
                            startAtSecond = false;
                            break;
                        }
                        else if (act.Students.Equals(activity.Students) && act.AcceptableTimeSlots[i, 0] > 0)
                        {
                            startAtSecond = false;
                            break;
                        }
                    }
                    if (startAtSecond)
                    {
                        count++;
                    }
                }
                if (count > timetable.TimeConstraints.ConstraintsStudents.StudentsEarlyMaxBeginningsAtSecondHour)
                {
                    return false;
                }

            }
            return true;
        }

        private static bool AcceptableForConstraintStudentsSetEarlyMaxBeginningsAtSecondHour(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (activity.Students.MaxBeginningsAtSecondHour == -1)
            {
                return true;
            }
            else
            {
                if (desiredHour != 1)
                {
                    return true;
                }
                var sl = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay).Slots.ElementAt(0);
                foreach (var act in sl.Activities)
                {
                    if (act.Students.Equals(activity.Students) && (act.Scheduled || act.AcceptableTimeSlots[desiredDay, 0] > 0))
                    {
                        return true;
                    }
                }


               
                        int count = 0;
                        for (int i = 0; i < timetable.LastGeneratedTimetable.Days.Count() - 1; i++)
                        {
                            if (desiredDay == i)
                            {
                                continue;
                            }
                            var slot = timetable.LastGeneratedTimetable.Days.ElementAt(i).Slots.ElementAt(0);
                            bool startAtSecond = true;
                            foreach (var act in slot.Activities)
                            {
                                if (act.Students.Equals(activity.Students) && act.Scheduled)
                                {
                                    startAtSecond = false;
                                    break;
                                }
                                else if (act.Students.Equals(activity.Students) && act.AcceptableTimeSlots[i, 0] > 0)
                                {
                                    startAtSecond = false;
                                    break;
                                }
                            }
                            if (startAtSecond)
                            {
                                count++;
                            }
                        }

                        if (count > activity.Students.MaxBeginningsAtSecondHour)
                        {
                            return false;
                        }
                    }
               
            return true;
        }


        #endregion


        private static bool AcceptableForConstraintTwoActivitiesOrdered(Timetable timetable, Activity activity)
        {
            if (timetable.TimeConstraints.ConstraintTwoActivitiesOrderedList.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (var constr in timetable.TimeConstraints.ConstraintTwoActivitiesOrderedList)
                {
                    if (constr.ActivityIds.ElementAt(0) == activity.ActivityId)
                    {
                        var act = (from a in timetable.ActivityList
                                  where a.ActivityId == constr.ActivityIds.ElementAt(1)
                                  select a).First();
                        if (act.Scheduled)
                        {
                            return false;
                        }
                    }
                    else if (constr.ActivityIds.ElementAt(1) == activity.ActivityId)
                    {
                        var act = (from a in timetable.ActivityList
                                   where a.ActivityId == constr.ActivityIds.ElementAt(0)
                                   select a).First();
                        if (!act.Scheduled)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintTwoActivitiesConsecutive(Timetable timetable, Activity activity, int desiredDay, int desiredHour)
        {
            if (timetable.TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (var constr in timetable.TimeConstraints.ConstraintTwoActivitiesConsecutiveList)
                {
                    if (constr.FirstActivityId == activity.ActivityId)
                    {
                        Timetable.TimeSlot slot;
                        if (desiredHour < timetable.HoursList.Count() - 2)
                        {
                            slot = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay).Slots.ElementAt(desiredHour + 1);

                        }
                        else
                        {
                            //this is the last day
                            if (desiredDay + 1 > timetable.DaysList.Count() - 1)
                            {
                                return false;
                            }
                            slot = timetable.LastGeneratedTimetable.Days.ElementAt(desiredDay + 1).Slots.ElementAt(0);
                        }
                        foreach (var act in slot.Activities)
                        {
                            if (act.ActivityId == constr.SecondActivityId)
                            {
                                if (act.Scheduled && act.PozScheduled != (desiredDay * 10 + desiredHour))
                                {
                                    return false;
                                }
                                else if (!act.Scheduled)
                                {
                                    if (act.AcceptableTimeSlots[desiredDay, desiredHour] < 1)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return true;
        }

        private static bool AcceptableForConstraintThreeActivitiesGrouped(Timetable timetable, Activity activity , int desiredDay)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintThreeActivitiesGroupedList)
            {
                if (constr.ActivityIds.Contains(activity.ActivityId))
                {
                    foreach (int id in constr.ActivityIds)
                    {
                        if (id != activity.ActivityId)
                        {
                            var act = (from a in timetable.ActivityList
                                       where a.ActivityId == id
                                       select a).First();
                            if (act.Scheduled && timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act) != desiredDay)
                            {
                                return false;
                            }
                            else if (!act.Scheduled)
                            {
                                bool found = false;
                                for (int h = 0; h < act.AcceptableTimeSlots.GetLength(1); h++)
                                {
                                    if (act.AcceptableTimeSlots[desiredDay, h] > 0)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static bool AcceptableForConstraintTwoActivitiesGrouped(Timetable timetable, Activity activity, int desiredDay)
        {
            foreach (var constr in timetable.TimeConstraints.ConstraintTwoActivitiesGroupedList)
            {
                if (constr.ActivityIds.Contains(activity.ActivityId))
                {
                    foreach (int id in constr.ActivityIds)
                    {
                        if (id != activity.ActivityId)
                        {
                            var act = (from a in timetable.ActivityList
                                       where a.ActivityId == id
                                       select a).First();
                            if (act.Scheduled && timetable.LastGeneratedTimetable.GetDayAndHourForActivity(act) != desiredDay)
                            {
                                return false;
                            }
                            else if (!act.Scheduled)
                            {
                                bool found = false;
                                for (int h = 0; h < act.AcceptableTimeSlots.GetLength(1); h++)
                                {
                                    if (act.AcceptableTimeSlots[desiredDay, h] > 0)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;        
        }

#endregion

        #region space contraints
        static Random random = new Random();
        private static Room FindARoom(Timetable timetable, Activity activity, List<Room> roomsAvailable , int day, int hour)
        {
            //first check for a preferated room

            #region activity
            
            //ConstraintActivityPreferredRooms
            foreach (var r in activity.PreferredRooms)
            {
                if (roomsAvailable.Contains(r))
                {
                    if (CheckBuildingChanges(timetable, activity, r, day, hour))
                    {
                        return r;
                    }
                }
            }
            
            //ConstraintActivityTagPreferredRooms
            if (activity.ActivityTag != null)
            {
                foreach (var r in activity.ActivityTag.PreferredRooms)
                {
                    if (roomsAvailable.Contains(r))
                    {
                        if (CheckBuildingChanges(timetable, activity, r, day, hour))
                        {
                            return r;
                        }
                    }
                }
            }

            //ConstraintSubjectActivityTagPreferredRoom
            foreach (var con in timetable.SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList)
            {
                if (activity.Subject.Equals(con.Subject) && activity.ActivityTag.Equals(con.ActivityTag))
                {
                    if (roomsAvailable.Contains(con.Room))
                    {
                        if (CheckBuildingChanges(timetable, activity, con.Room, day, hour))
                        {
                            return con.Room;
                        }
                    }
                }
            }
            //ConstraintSubjectActivityTagPreferredRooms
            foreach (var con in timetable.SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList)
            {
                if (activity.Subject.Equals(con.Subject) && activity.ActivityTag.Equals(con.ActivityTag))
                {
                    var interserct = con.Rooms.Intersect(roomsAvailable);
                    if (interserct.Count() > 0)
                    {
                        foreach (var inter in interserct)
                        {
                            if (CheckBuildingChanges(timetable, activity, inter, day, hour))
                            {
                                return inter;
                            }
                        }
                    }
                }
            }
            #endregion

            #region students
            //ConstraintStudentsSetHomeRoom
            //foreach (var con in timetable.SpaceConstraints.ConstraintStudentsSetHomeRoomList)
            //{
            //    if (activity.Teacher.Equals(con.Students))
            //    {
            //        if (roomsAvailable.Contains(con.Room))
            //        {
            //            if (CheckBuildingChanges(timetable, activity, con.Room, day, hour))
            //            {
            //                return con.Room;
            //            }
            //        }
            //    }
            //}
            //ConstraintStudentsSetHomeRooms
            foreach (var r in activity.Students.HomeRooms)
            {
                if (roomsAvailable.Contains(r))
                {

                    if (CheckBuildingChanges(timetable, activity, r, day, hour))
                    {
                        return r;
                    }
                }
            }

            #endregion

            #region teacher
            //ConstraintTeacherHomeRoom
            //foreach (var con in timetable.SpaceConstraints.ConstraintTeacherHomeRoomList)
            //{
            //    if(activity.Teacher.Equals(con.Teacher))
            //    {
            //        if (roomsAvailable.Contains(con.Room))
            //        {
            //            if (CheckBuildingChanges(timetable, activity, con.Room, day, hour))
            //            {
            //                return con.Room;
            //            }
            //        }
            //    }
            //}
            //ConstraintTeacherHomeRooms
            foreach (var r in activity.Teacher.HomeRooms)
            {
                if (roomsAvailable.Contains(r))
                {
                    if (CheckBuildingChanges(timetable, activity, r, day, hour))
                    {
                        return r;
                    }
                }
            }


            #endregion

            #region subject
            //ConstraintSubjectPreferredRoom
            //foreach (var con in timetable.SpaceConstraints.ConstraintSubjectPreferredRoomList)
            //{
            //    if (activity.Subject.Equals(con.Subject))
            //    {
            //        if (roomsAvailable.Contains(con.Room))
            //        {
            //            if (CheckBuildingChanges(timetable, activity, con.Room, day, hour))
            //            {
            //                return con.Room;
            //            }
            //        }
            //    }
            //}
            //ConstraintSubjectPreferredRooms
            foreach (var r in activity.Subject.HomeRooms)
            {
                if (roomsAvailable.Contains(r))
                {
                    if (CheckBuildingChanges(timetable, activity, r, day, hour))
                    {
                        return r;
                    }
                }
            }

            #endregion

            //no preferred room has found
            if (roomsAvailable.Count > 0)
            {
                int nrTries = 1;
                Room possibleRoom = roomsAvailable.ElementAt(App.RandomTool.Next(roomsAvailable.Count));
                while (!CheckBuildingChanges(timetable, activity, possibleRoom, day, hour)
                    && nrTries <= roomsAvailable.Count())
                {
                    nrTries++;
                    possibleRoom = roomsAvailable.ElementAt(App.RandomTool.Next(roomsAvailable.Count));
                }
                if (nrTries >= roomsAvailable.Count())
                {
                    return null;
                }
                else
                {
                    return possibleRoom;
                }
            }
            else
                return null;

        }

        private static bool CheckBuildingChanges(Timetable timetable, Activity activity, Room room, int day, int hour)
        {
            //ConstraintTeacherMaxBuildingChangesPerDay
            bool hasConstraintTeacherMaxBuildingPerDay = false;

            if (activity.Teacher.MaxBuildingChangesPerDay > -1)
            {
                hasConstraintTeacherMaxBuildingPerDay = true;
                int maxChanges = activity.Teacher.MaxBuildingChangesPerDay;
                if (BuildingChangesTeacherDay(timetable, activity.Teacher, room, day) > maxChanges)
                {
                    return false;
                }
            }
            

            //ConstraintTeachersMaxBuildingChangesPerDay
            if (!hasConstraintTeacherMaxBuildingPerDay && timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerDay != -1)
            {
                
                    int maxChanges = timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerDay;
                    if (BuildingChangesTeacherDay(timetable, activity.Teacher, room, day) > maxChanges)
                    {
                        return false;
                    }
                
            }

            //ConstraintTeacherMaxBuildingChangesPerWeek
            bool hasConstraintTeacherMaxBuildingPerWeek = false;

            if (activity.Teacher.MaxBuildingChangesPerWeek > -1)
            {
                hasConstraintTeacherMaxBuildingPerWeek = true;
                int maxChanges = activity.Teacher.MaxBuildingChangesPerWeek;
                if (BuildingChangesTeacherWeek(timetable, activity.Teacher, room) > maxChanges)
                {
                    return false;
                }
            }
            


            //ConstraintTeachersMaxBuildingChangesPerWeek
            if (!hasConstraintTeacherMaxBuildingPerWeek && timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerWeek != -1)
            {
                
                    int maxChanges = timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerWeek;
                    if (BuildingChangesTeacherWeek(timetable, activity.Teacher, room) > maxChanges)
                    {
                        return false;
                    }
                
            }

            //ConstraintStudentsSetMaxBuildingChangesPerDay
            bool hasConstraintStudentsSetMaxBuildingPerDay = false;
            if (activity.Students.MaxBuildingChangesPerDay > -1)
            {
                hasConstraintStudentsSetMaxBuildingPerDay = true;
                int maxChanges = activity.Students.MaxBuildingChangesPerDay;
                if (BuildingChangesStudentsDay(timetable, activity.Students, room, day) > maxChanges)
                {
                    return false;
                }

            }

            //ConstraintStudentsMaxBuildingChangesPerDay
            if (!hasConstraintStudentsSetMaxBuildingPerDay && timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerDay != -1)
            {
                
                    int maxChanges = timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerDay;
                    if (BuildingChangesStudentsDay(timetable, activity.Students, room, day) > maxChanges)
                    {
                        return false;
                    }
                
            }

            //ConstraintStudentsSetMaxBuildingChangesPerWeek
            bool hasConstraintStudentsSetMaxBuildingChangesPerWeek = false;
            if (activity.Students.MaxBuildingChangesPerWeek > -1)
            {

                hasConstraintStudentsSetMaxBuildingChangesPerWeek = true;
                int maxChanges = activity.Students.MaxBuildingChangesPerWeek;
                if (BuildingChangesStudentsWeek(timetable, activity.Students, room) > maxChanges)
                {
                    return false;
                }

            }

            //ConstraintStudentsMaxBuildingChangesPerWeek
            if (!hasConstraintStudentsSetMaxBuildingChangesPerWeek && timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerWeek != -1)
            {
                
                    int maxChanges = timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerWeek;
                    if (BuildingChangesStudentsWeek(timetable, activity.Students, room) > maxChanges)
                    {
                        return false;
                    }
                
            }

            //ConstraintActivitiesOccupyMaxDifferentRooms
            foreach (var constr in timetable.SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
            {
                foreach (var activityId in constr.ActivityIds)
                {
                    if (activity.ActivityId == activityId)
                    {
                        int maxDifRooms = constr.MaxNumberOfDifferentRooms;
                        if(RoomsChangesActivity(timetable, activity, room)> maxDifRooms)
                        {
                            return false;
                        }

                    }
                }
            }

            //ConstraintTeacherMinGapsBetweenBuildingChanges

            if (activity.Teacher.MinGapsBetweenBuildingChanges > -1)
            {
                int minGaps = activity.Teacher.MinGapsBetweenBuildingChanges;
                var dayWeek = timetable.LastGeneratedTimetable.Days.ElementAt(day);
                for (int i = dayWeek.Slots.Count() - 1; i >= 0; i--)
                {
                    if (hour < i)
                    {
                        continue;
                    }
                    foreach (var act in dayWeek.Slots.ElementAt(i).Activities)
                    {
                        if (act.Teacher.Equals(activity.Teacher) && act.Scheduled)
                        {
                            //check if are in different buildings
                            var prevBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(act.AssignedRoom)
                                               select b.Name;
                            var nextBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(room)
                                               select b.Name;
                            // has to change building
                            if (!prevBuilding.First().Equals(nextBuilding.First()))
                            {
                                //compute gaps
                                int gaps = hour - i;
                                if (gaps > minGaps)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            

            //ConstraintStudentsSetMinGapsBetweenBuildingChanges
            if (activity.Students.MinGapsBetweenBuildingChanges > -1)
            {
                int minGaps = activity.Students.MinGapsBetweenBuildingChanges;
                var dayWeek = timetable.LastGeneratedTimetable.Days.ElementAt(day);
                for (int i = dayWeek.Slots.Count() - 1; i >= 0; i--)
                {
                    if (hour < i)
                    {
                        continue;
                    }
                    foreach (var act in dayWeek.Slots.ElementAt(i).Activities)
                    {
                        if (act.Teacher.Equals(activity.Teacher) && act.Scheduled)
                        {
                            //check if are in different buildings
                            var prevBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(act.AssignedRoom)
                                               select b.Name;
                            var nextBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(room)
                                               select b.Name;
                            // has to change building
                            if (!prevBuilding.First().Equals(nextBuilding.First()))
                            {
                                //compute gaps
                                int gaps = hour - i;
                                if (gaps > minGaps)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }


            }

            //ConstraintTeachersMinGapsBetweenBuildingChanges
            if(timetable.TimeConstraints.ConstraintsTeachers.MinGapsBetweenBuildingChanges > -1)
            {

                int minGaps = timetable.TimeConstraints.ConstraintsTeachers.MinGapsBetweenBuildingChanges;
                var dayWeek = timetable.LastGeneratedTimetable.Days.ElementAt(day);
                for (int i = dayWeek.Slots.Count() - 1; i >= 0; i--)
                {
                    if (hour < i)
                    {
                        continue;
                    }
                    foreach (var act in dayWeek.Slots.ElementAt(i).Activities)
                    {
                        if (act.Teacher.Equals(activity.Teacher) && act.Scheduled)
                        {
                            //check if are in different buildings
                            var prevBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(act.AssignedRoom)
                                               select b.Name;
                            var nextBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(room)
                                               select b.Name;
                            // has to change building
                            if (!prevBuilding.First().Equals(nextBuilding.First()))
                            {
                                //compute gaps
                                int gaps = hour - i;
                                if (gaps > minGaps)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

            }
            

            //ConstraintStudentsMinGapsBetweenBuildingChanges
            if(timetable.TimeConstraints.ConstraintsStudents.MinGapsBetweenBuildingChanges > -1)
            {
                int minGaps = timetable.TimeConstraints.ConstraintsStudents.MinGapsBetweenBuildingChanges;
                var dayWeek = timetable.LastGeneratedTimetable.Days.ElementAt(day);
                for (int i = dayWeek.Slots.Count() - 1; i >= 0; i--)
                {
                    if (hour < i)
                    {
                        continue;
                    }
                    foreach (var act in dayWeek.Slots.ElementAt(i).Activities)
                    {
                        if (act.Students.Equals(activity.Students) && act.Scheduled)
                        {
                            //check if are in different buildings
                            var prevBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(act.AssignedRoom)
                                               select b.Name;
                            var nextBuilding = from b in timetable.BuildingsList
                                               where b.Rooms.Contains(room)
                                               select b.Name;
                            // has to change building
                            if (!prevBuilding.First().Equals(nextBuilding.First()))
                            {
                                //compute gaps
                                int gaps = hour - i;
                                if (gaps < minGaps)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var constr in timetable.SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList)
            {
                if (constr.ActivityIds.Contains(activity.ActivityId))
                {
                    foreach (var actId in constr.ActivityIds)
                    {
                        if (actId == activity.ActivityId)
                        {
                            continue;
                        }
                        var acts = from a in timetable.ActivityList
                                  where a.ActivityId == actId
                                  select a;
                        if (acts.Count() > 0)
                        {
                            var act = acts.First();
                            
                            if (act.Scheduled && ((act.PozScheduled == (day * 10 + hour) + 1) || (act.PozScheduled == (day * 10 + hour) - 1)))
                            { // they are consecutive
                                if (act.AssignedRoom != null && act.AssignedRoom != room)
                                {
                                    return false; // this is not the same room
                                }
                            }
                        }
                    }
                }
            }
                        
            return true;
                        
        }

        public static int BuildingChangesTeacherDay(Timetable timetable, Teacher teacher, Room room , int d)
        {
            HashSet<string> buildingName = new HashSet<string>();
            var day = timetable.LastGeneratedTimetable.Days.ElementAt(d);

            foreach (var slot in day.Slots)
            {
                foreach (var activity in slot.Activities)
                {
                    if (activity.Teacher.Equals(teacher) && activity.Scheduled)
                    {
                        var building = from b in timetable.BuildingsList
                                       where b.Rooms.Contains(activity.AssignedRoom)
                                       select b.Name;

                        buildingName.Add(building.First());

                        break;
                    }
                }
            }
            
            var bu = from b in timetable.BuildingsList
                     where b.Rooms.Contains(room)
                     select b.Name;

            buildingName.Add(bu.First());

            return buildingName.Count() -1;
        }

        public static int BuildingChangesTeacherWeek(Timetable timetable, Teacher teacher, Room room)
        {
            HashSet<string> buildingName = new HashSet<string>();
            foreach (var day in timetable.LastGeneratedTimetable.Days)
            {
                foreach (var slot in day.Slots)
                {
                    foreach (var activity in slot.Activities)
                    {
                        if (activity.Teacher.Equals(teacher) && activity.Scheduled)
                        {
                            var building = from b in timetable.BuildingsList
                                           where b.Rooms.Contains(activity.AssignedRoom)
                                           select b.Name;

                            buildingName.Add(building.First());

                            break;
                        }
                    }
                }
            }

            var bu = from b in timetable.BuildingsList
                           where b.Rooms.Contains(room)
                           select b.Name;

            buildingName.Add(bu.First());

            return buildingName.Count() - 1;
        }

        public static int BuildingChangesStudentsDay(Timetable timetable, Students students, Room room, int d)
        {
            HashSet<string> buildingName = new HashSet<string>();
            var day = timetable.LastGeneratedTimetable.Days.ElementAt(d);

            foreach (var slot in day.Slots)
            {
                foreach (var activity in slot.Activities)
                {
                    if (activity.Students.Equals(students) && activity.Scheduled)
                    {
                        var building = from b in timetable.BuildingsList
                                       where b.Rooms.Contains(activity.AssignedRoom)
                                       select b.Name;

                        buildingName.Add(building.First());

                        break;
                    }
                }
            }

            var bu = from b in timetable.BuildingsList
                     where b.Rooms.Contains(room)
                     select b.Name;

            buildingName.Add(bu.First());

            return buildingName.Count() - 1;
        }

        public static int BuildingChangesStudentsWeek(Timetable timetable, Students students, Room room)
        {
            HashSet<string> buildingName = new HashSet<string>();
            foreach (var day in timetable.LastGeneratedTimetable.Days)
            {
                foreach (var slot in day.Slots)
                {
                    foreach (var activity in slot.Activities)
                    {
                        if (activity.Students.Equals(students) && activity.Scheduled)
                        {
                            var building = from b in timetable.BuildingsList
                                           where b.Rooms.Contains(activity.AssignedRoom)
                                           select b.Name;

                            buildingName.Add(building.First());

                            break;
                        }
                    }
                }
            }

            var bu = from b in timetable.BuildingsList
                     where b.Rooms.Contains(room)
                     select b.Name;

            buildingName.Add(bu.First());

            return buildingName.Count() - 1;
        }

        public static int RoomsChangesActivity(Timetable timetable, Activity activity, Room room)
        {
            HashSet<string> roomNames = new HashSet<string>();
            foreach (var day in timetable.LastGeneratedTimetable.Days)
            {
                foreach (var slot in day.Slots)
                {
                    foreach (var act in slot.Activities)
                    {
                        if (act.Scheduled)
                        {
                            foreach(var constr in timetable.SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
                            {
                                if (constr.ActivityIds.Contains(act.ActivityId) && constr.ActivityIds.Contains(activity.ActivityId))
                                {
                                    roomNames.Add(act.AssignedRoom.Name);
                                }
                            }
                        }
                    }
                }
            }
            roomNames.Add(room.Name);
            return roomNames.Count() - 1;
        }
               
        #endregion

        /// <summary>
        /// check if a table of strings contains data
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static bool ContainsData(string[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] != null && table[i, j] != string.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
    }
}
