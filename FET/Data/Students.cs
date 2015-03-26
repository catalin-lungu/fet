using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public abstract class Students
    {
        public virtual string Name { get; set; }
        public int NumberOfStudents { get; set; }
        public int MaxGapsPerWeek { get; set; }
        public int MaximumHoursDaily { get; set; }        
        public int MaxBeginningsAtSecondHour { get; set; }
        public int MaxGapsPerDay { get; set; }
        public int MaxDaysPerWeek { get; set; }
        public int MaximumHoursContinuously { get; set; }

        private MinimHoursDaily minHoursDaily;
        public MinimHoursDaily MinHoursDaily 
        {
            get { return this.minHoursDaily; }
            set { this.minHoursDaily = value; }
        }

        public int MaxBuildingChangesPerDay { get; set; }
        public int MaxBuildingChangesPerWeek { get; set; }
        public int MinGapsBetweenBuildingChanges { get; set; }

        private ConstraintIntervalMaxDaysPerWeek studentsSetIntervalMaxDaysPerWeek;

        public ConstraintIntervalMaxDaysPerWeek StudentsSetIntervalMaxDaysPerWeek
        {
            get { return studentsSetIntervalMaxDaysPerWeek; }
            set { studentsSetIntervalMaxDaysPerWeek = value; }
        }

        private ObservableCollection<Room> homeRooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> HomeRooms
        {
            get { return homeRooms; }
            set { homeRooms = value; }
        }

        private ObservableCollection<TimeDayHour> notAvailableTimes = new ObservableCollection<TimeDayHour>();
        public ObservableCollection<TimeDayHour> NotAvailableTimes
        {
            get { return notAvailableTimes; }
            set { notAvailableTimes = value; }
        }

        private ObservableCollection<ConstraintActivityTagMaxHoursDaily> studentsSetActivityTagMaxHoursDailyList
            = new ObservableCollection<ConstraintActivityTagMaxHoursDaily>();
        public ObservableCollection<ConstraintActivityTagMaxHoursDaily> StudentsSetActivityTagMaxHoursDailyList
        {
            get { return studentsSetActivityTagMaxHoursDailyList; }
            set { studentsSetActivityTagMaxHoursDailyList = value; }
        }


        private ObservableCollection<ConstraintActivityTagMaxHoursContinuously> studentsSetActivityTagMaxHoursContinuouslyList
            = new ObservableCollection<ConstraintActivityTagMaxHoursContinuously>();
        public ObservableCollection<ConstraintActivityTagMaxHoursContinuously> StudentsSetActivityTagMaxHoursContinuouslyList
        {
            get { return studentsSetActivityTagMaxHoursContinuouslyList; }
            set { studentsSetActivityTagMaxHoursContinuouslyList = value; }
        }

        public Students()
        {

            MaxGapsPerWeek = -1;
            MaximumHoursDaily = -1;
            MaxBeginningsAtSecondHour = -1;
            MaxGapsPerDay = -1;
            MaxDaysPerWeek = -1;
            MaximumHoursContinuously = -1;

            MaxBuildingChangesPerDay = -1;
            MaxBuildingChangesPerWeek = -1;
            MinGapsBetweenBuildingChanges = -1;

            if (this.studentsSetIntervalMaxDaysPerWeek == null)
            {
                this.studentsSetIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();
            }
            if (this.minHoursDaily == null)
            {
                this.minHoursDaily = new MinimHoursDaily();
            }
        }
    }
}
