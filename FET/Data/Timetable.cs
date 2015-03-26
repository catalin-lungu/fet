using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FET.Data
{
    /// <summary>
    /// contains all the timetable information
    /// </summary>
    class Timetable
    {
        
        private static Timetable timetable = null;

        public string InstitutionName { get; set; }
        public string Comments { get; set; }

        public List<string> HoursList = new List<string>();
        public List<string> DaysList = new List<string>();

        private List<Year> classList = new List<Year>();
        public List<Year> ClassList
        {
            get { return this.classList; }
            set { this.classList = value; }
        }
        public List<Teacher> TeacherList = new List<Teacher>();
        public List<Subject> SubjectList = new List<Subject>();

        public List<ActivityTag> ActivitiyTagsList = new List<ActivityTag>();
        public List<Activity> ActivityList = new List<Activity>();

        public List<Building> BuildingsList = new List<Building>();
        
        public TimeConstraint TimeConstraints = new TimeConstraint();
        public SpaceConstraint SpaceConstraints = new SpaceConstraint();

        /// <summary>
        /// Generated schedule
        /// </summary>
        public GeneratedTimetable LastGeneratedTimetable = null;
        public GeneratedTimetable AuxTimetable = null;

        private Timetable() { }

        public bool IsGenerated()
        {
            if (LastGeneratedTimetable == null)
            {
                return false;
            }
            return true;
        }

        public void Clear()
        {
            timetable.InstitutionName = string.Empty;
            timetable.Comments = string.Empty;
            timetable.HoursList.Clear();
            timetable.DaysList.Clear();
            timetable.ClassList.Clear();
            timetable.TeacherList.Clear();
            timetable.SubjectList.Clear();
            timetable.ActivitiyTagsList.Clear();
            timetable.ActivityList.Clear();
            timetable.GetRoomsList().Clear();
            timetable.BuildingsList.Clear();            
            timetable.TimeConstraints.Clear();
            timetable.SpaceConstraints.Clear();
            AuxTimetable = null;
        }


        /// <summary>
        /// generate timetable
        /// </summary>
        /// <returns></returns>
        public bool Generate()
        {
            return ManageTimetable.Generate(this);
        }

        public static Timetable GetInstance()
        {
            if (timetable == null)
            {
                timetable = new Timetable();
            }
            return timetable;
        }

        public static bool IsLoaded()
        {
            if (timetable == null)
            {
                return false;
            }

            if (timetable.HoursList.Count == 0 || timetable.DaysList.Count == 0)
            {
                return false;
            }
            return true;
        }
 
        public static string GetDay(int i)
        {
            return timetable.DaysList.ElementAt(i);
        }
        public static string GetHour(int i)
        {
            return timetable.HoursList.ElementAt(i);
        }
      
        /// <summary>
        /// get a list of all students name (years, groups and subgroups)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetStudentsNames()
        {
            List<string> studentsName = new List<string>();
            foreach (var s in timetable.ClassList)
            {
                studentsName.Add(s.Name);
                foreach (var g in s.Groups)
                {
                    studentsName.Add(g.Name);
                    foreach (var sg in g.Subgroups)
                    {
                        studentsName.Add(sg.Name);
                    }
                }
            }
            return studentsName;
        }

        public Students GetStudents(string name)
        {
            var year = ClassList.FirstOrDefault(item => item.Name.Equals(name));

            if (year != null)
            {
                return year;
            }
            else
            {
                foreach (var y in ClassList)
                {
                    var g = y.Groups.FirstOrDefault(item => item.Name.Equals(name));
                    if (g != null)
                    {
                        return g;
                    }                    
                }
            }

            foreach (var y in ClassList)
            {
                foreach (var g in y.Groups)
                {
                    var subg = g.Subgroups.FirstOrDefault(item => item.Name.Equals(name));
                    if (subg != null)
                    {
                        return subg;
                    }
                }
            }
            return null;
        }

        public List<Students> GetStudents()
        {
            List<Students> students = new List<Students>();
            foreach (var y in ClassList)
            {
                students.Add(y);
                foreach (var g in y.Groups)
                {
                    students.Add(g);
                    students.AddRange(g.Subgroups);
                }
            }
            return students;
        }

        public List<Room> GetRoomsList()
        {
            List<Room> roomsList = new List<Room>();

            foreach (var b in BuildingsList)
            {
                roomsList.AddRange(b.Rooms);
            }
            return roomsList;
        }

        public class GeneratedTimetable 
        {
            public List<Day> Days = new List<Day>();

            public List<Activity> UnableToSchedule = new List<Activity>();

            /// <summary>
            /// return day of a scheduled activity
            /// </summary>
            /// <param name="activity"></param>
            /// <returns></returns>
            public int GetDayAndHourForActivity(Activity activity)
            {
                int h;
                return GetDayAndHourForActivity(activity, out h);
            }
            /// <summary>
            /// return day of a scheduled activity
            /// </summary>
            /// <param name="activity"></param>
            /// <param name="hour"></param>
            /// <returns></returns>
            public int GetDayAndHourForActivity(Activity activity, out int hour)
            {
                int d = -1;
                hour = -1;
                foreach (var day in Days)
                {
                    d++;
                    foreach (var slot in day.Slots)
                    {
                        hour++;
                        if (slot.Activities.Contains(activity) && activity.Scheduled)
                        {
                            return d;
                        }
                    }
                    hour = -1;
                }
                return -1;
            }

            public GeneratedTimetable Clone()
            {
                GeneratedTimetable gt = new GeneratedTimetable();
                foreach (var d in this.Days)
                {
                    gt.Days.Add(d);
                }
                foreach (var u in this.UnableToSchedule)
                {
                    gt.UnableToSchedule.Add(u);
                }
                return gt;
            }

            public void Clear()
            {
                this.Days.Clear();
                this.UnableToSchedule.Clear();
            }
        }

        public class Day 
        {
            public List<TimeSlot> Slots = new List<TimeSlot>();
        }

        /// <summary>
        /// Represent one slot of time for a day
        /// ex: 1 hour
        /// </summary>
        public class TimeSlot
        {
            /// <summary>
            /// list of activities that take place simultaneously
            /// </summary>
            public List<Activity> Activities = new List<Activity>();

            /// <summary>
            /// list of rooms available at this time
            /// </summary>
            public List<Room> Rooms = new List<Room>();
        }
    }
}
