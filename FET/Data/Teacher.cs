using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace FET.Data
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public int MaxDaysPerWeek { get; set; }
        public int MaxGapsPerWeek { get; set; }
        public int MaxGapsPerDay { get; set; }
        public int MaximumHoursDaily { get; set; }
        
        public int MaximumHoursContinuously { get; set; }
        public int MinimumDaysPerWeek { get; set; }

        public int MaxBuildingChangesPerDay { get; set; }
        public int MaxBuildingChangesPerWeek { get; set; }
        public int MinGapsBetweenBuildingChanges { get; set; }

        private MinimHoursDaily minHoursDaily;
        public MinimHoursDaily MinHoursDaily
        {
            get { return this.minHoursDaily; }
            set { this.minHoursDaily = value; }
        }

        private ConstraintIntervalMaxDaysPerWeek teacherIntervalMaxDaysPerWeek;
        public ConstraintIntervalMaxDaysPerWeek TeacherIntervalMaxDaysPerWeek
        {
            get { return this.teacherIntervalMaxDaysPerWeek; }
            set { this.teacherIntervalMaxDaysPerWeek = value; }
        }

        private ObservableCollection<TimeDayHour> teacherNotAvailableTimes = new ObservableCollection<TimeDayHour>();
        public ObservableCollection<TimeDayHour> NotAvailableTimes
        {
            get { return teacherNotAvailableTimes; }
            set { teacherNotAvailableTimes = value; }
        }

        private ObservableCollection<Room> homeRooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> HomeRooms
        {
            get { return homeRooms; }
            set { homeRooms = value; }
        }

        private ObservableCollection<ConstraintActivityTagMaxHoursDaily> teacherActivityTagMaxHoursDailyList
            = new ObservableCollection<ConstraintActivityTagMaxHoursDaily>();
        public ObservableCollection<ConstraintActivityTagMaxHoursDaily> TeacherActivityTagMaxHoursDailyList
        {
            get { return this.teacherActivityTagMaxHoursDailyList; }
            set { this.teacherActivityTagMaxHoursDailyList = value; }
        }

        private ObservableCollection<ConstraintActivityTagMaxHoursContinuously> teacherActivityTagMaxHoursContinuously
            = new ObservableCollection<ConstraintActivityTagMaxHoursContinuously>();
        public ObservableCollection<ConstraintActivityTagMaxHoursContinuously> TeacherActivityTagMaxHoursContinuouslyList
        {
            get { return this.teacherActivityTagMaxHoursContinuously; }
            set { this.teacherActivityTagMaxHoursContinuously = value; }
        }


        public Teacher()
        {
            this.TeacherId = App.RandomTool.Next();

            MaxDaysPerWeek = -1;
            MaxGapsPerWeek = -1;
            MaxGapsPerDay = -1;
            MaximumHoursDaily = -1;           
            MaximumHoursContinuously = -1;
            MinimumDaysPerWeek = -1;

            MaxBuildingChangesPerDay = -1;
            MaxBuildingChangesPerWeek = -1;
            MinGapsBetweenBuildingChanges = -1;

            if (this.teacherIntervalMaxDaysPerWeek == null)
            {
                this.teacherIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();
            }
            if (this.minHoursDaily == null)
            {
                this.minHoursDaily = new MinimHoursDaily();
            }
        }

        public Teacher(string name) :this()
        {
            this.Name = name;
            
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Teacher t = obj as Teacher;
            if ((System.Object)t == null)
            {
                string teacherName = obj as string;
                if (teacherName == null)
                    return false;
                else
                    return this.Name.Equals(teacherName);
            }

            // Return true if the fields match:
            
            return this.Name.Equals(t.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }       
      
    }
}
