using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class TeacherSchedule
    {
        public Teacher Teacher { get; set; }
        public List<DayHour> DayHourList = new List<DayHour>();

        public class DayHour
        {
            public int Day { get; set; }
            public int Hour { get; set; }
            public Activity Activity { get; set; }
            public DayHour(int day, int hour, Activity activity)
            {
                this.Day = day;
                this.Hour = hour;
                this.Activity = activity;
            }
        }
    }
}
