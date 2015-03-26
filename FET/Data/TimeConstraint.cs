using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FET.Data
{
    public class TimeConstraint
    {
        [Key]
        public int TimeContraintId { get; set; }

        public ConstraintsTeachers ConstraintsTeachers { get; set; }
        public ConstraintsStudents ConstraintsStudents { get; set; }
        public ConstraintBreakTimes ConstraintsBreakTimes { get; set; }

        #region lists of constraints

        public List<ConstraintActivityPreferredStartingTimes> ConstraintActivityPreferredStartingTimesList =
            new List<ConstraintActivityPreferredStartingTimes>();
        public List<ConstraintActivitiesPreferredStartingTimes> ConstraintActivitiesPreferredStartingTimesList =
            new List<ConstraintActivitiesPreferredStartingTimes>();
        public List<ConstraintActivitiesSameStartingDay> ConstraintActivitiesSameStartingDayList =
            new List<ConstraintActivitiesSameStartingDay>();
        public List<ConstraintActivitiesPreferredTimeSlots> ConstraintActivitiesPreferredTimeSlotsList =
            new List<ConstraintActivitiesPreferredTimeSlots>();
        public List<ConstraintTwoActivitiesConsecutive> ConstraintTwoActivitiesConsecutiveList =
            new List<ConstraintTwoActivitiesConsecutive>();        
        public List<ConstraintMinDaysBetweenActivities> ConstraintMinDaysBetweenActivitiesList =
            new List<ConstraintMinDaysBetweenActivities>();
        public List<ConstraintActivitiesNotOverlapping> ConstraintActivitiesNotOverlappingList =
            new List<ConstraintActivitiesNotOverlapping>();
        public List<ConstraintActivityEndsStudentsDay> ConstraintActivityEndsStudentsDayList =
            new List<ConstraintActivityEndsStudentsDay>();
        public List<ConstraintActivityPreferredStartingTime> ConstraintActivityPreferredStartingTimeList =
            new List<ConstraintActivityPreferredStartingTime>();
        public List<ConstraintActivityPreferredTimeSlots> ConstraintActivityPreferredTimeSlotsList =
            new List<ConstraintActivityPreferredTimeSlots>();
        public List<ConstraintActivitiesSameStartingHour> ConstraintActivitiesSameStartingHourList =
            new List<ConstraintActivitiesSameStartingHour>();
        public List<ConstraintActivitiesSameStartingTime> ConstraintActivitiesSameStartingTimeList =
            new List<ConstraintActivitiesSameStartingTime>();
        public List<ConstraintThreeActivitiesGrouped> ConstraintThreeActivitiesGroupedList =
            new List<ConstraintThreeActivitiesGrouped>();
        public List<ConstraintTwoActivitiesGrouped> ConstraintTwoActivitiesGroupedList =
            new List<ConstraintTwoActivitiesGrouped>();
        public List<ConstraintTwoActivitiesOrdered> ConstraintTwoActivitiesOrderedList =
            new List<ConstraintTwoActivitiesOrdered>();
        public List<ConstraintActivitiesEndStudentsDay> ConstraintActivitiesEndStudentsDayList =
            new List<ConstraintActivitiesEndStudentsDay>();
        public List<ConstraintSubactivitiesPreferredStartingTimes> ConstraintSubactivitiesPreferredStartingTimesList =
            new List<ConstraintSubactivitiesPreferredStartingTimes>();
        public List<ConstraintSubactivitiesPreferredTimeSlots> ConstraintSubactivitiesPreferredTimeSlotsList =
            new List<ConstraintSubactivitiesPreferredTimeSlots>();
        public List<ConstraintMinGapsBetweenActivities> ConstraintMinGapsBetweenActivitiesList =
            new List<ConstraintMinGapsBetweenActivities>();
        public List<ConstraintActivitiesOccupyMaxTimeSlotsFromSelection> ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList =
            new List<ConstraintActivitiesOccupyMaxTimeSlotsFromSelection>();
        public List<ConstraintMaxDaysBetweenActivities> ConstraintMaxDaysBetweenActivitiesList =
            new List<ConstraintMaxDaysBetweenActivities>();
        #endregion
       
        public TimeConstraint()
        {
            this.TimeContraintId = App.RandomTool.Next();
            this.ConstraintsTeachers = new ConstraintsTeachers();
            this.ConstraintsStudents = new ConstraintsStudents();
            this.ConstraintsBreakTimes = new ConstraintBreakTimes();
        }

        public void Clear()
        {

            if (this.ConstraintsTeachers != null)
            {
                this.ConstraintsTeachers.Clear();
            }
            if (this.ConstraintsStudents != null)
            {
                this.ConstraintsStudents.Clear();
            }
            if (this.ConstraintsBreakTimes != null)
            {
                this.ConstraintsBreakTimes.Clear();
            }

            this.ConstraintActivityPreferredStartingTimesList.Clear();
            this.ConstraintActivitiesPreferredStartingTimesList.Clear();
            this.ConstraintActivitiesSameStartingDayList.Clear();
            this.ConstraintActivitiesPreferredTimeSlotsList.Clear();
            this.ConstraintTwoActivitiesConsecutiveList.Clear();
            this.ConstraintMinDaysBetweenActivitiesList.Clear();
            this.ConstraintActivitiesNotOverlappingList.Clear();
            this.ConstraintActivityEndsStudentsDayList.Clear();
            this.ConstraintActivityPreferredStartingTimeList.Clear();
            this.ConstraintActivityPreferredTimeSlotsList.Clear();
            this.ConstraintActivitiesSameStartingHourList.Clear();
            this.ConstraintActivitiesSameStartingTimeList.Clear();
            this.ConstraintThreeActivitiesGroupedList.Clear();
            this.ConstraintTwoActivitiesGroupedList.Clear();
            this.ConstraintTwoActivitiesOrderedList.Clear();
            this.ConstraintActivitiesEndStudentsDayList.Clear();
            this.ConstraintSubactivitiesPreferredStartingTimesList.Clear();
            this.ConstraintSubactivitiesPreferredTimeSlotsList.Clear();
            this.ConstraintMinGapsBetweenActivitiesList.Clear();
            this.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList.Clear();
            this.ConstraintMaxDaysBetweenActivitiesList.Clear();
        }

    }

    #region general usage classes
    /// <summary>
    /// class that represents a slot time, day and hour
    /// </summary>
    public class TimeDayHour
    {
        [Key]
        public int TimeDayHourId { get; set; }
        public int ParentId { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }

        public TimeDayHour() { }
        public TimeDayHour(string day, string hour)
        {
            this.Day = day;
            this.Hour = hour;
        }
        public TimeDayHour(string day, string hour, int parentId)
        {
            this.Day = day;
            this.Hour = hour;
            this.ParentId = parentId;
        }

        public override string ToString()
        {
            return this.Day + "\t"+ this.Hour;
        }
    }

    /// <summary>
    /// type of constraint
    /// </summary>
    public class MinimHoursDaily
    {
        private int minimumHoursDaily = -1;
        public int MinimumHoursDaily
        {
            get { return this.minimumHoursDaily; }
            set { this.minimumHoursDaily = value; }
        }
        private bool allowEmptyDays = true;
        public bool AllowEmptyDays
        {
            get { return this.allowEmptyDays; }
            set { this.allowEmptyDays = value; }
        }
    }

    /// <summary>
    /// class that contains constraints for all teachers 
    /// </summary>
    public class ConstraintsTeachers 
    {
        private int teachersMaxGapsPerWeek;
        public int TeachersMaxGapsPerWeek
        {
            get
            {
                return this.teachersMaxGapsPerWeek;
            }

            set 
            {
                if (value != teachersMaxGapsPerWeek)
                {
                    this.teachersMaxGapsPerWeek = value;
                    
                }
            }
        }

        public int TeachersMaxGapsPerDay { get; set; } 
        public int TeachersMaxHoursDaily { get; set; }
        public int TeachersMinHoursDaily { get; set; }
        public int TeachersMaxHoursContinuously { get; set; }
        public int TeachersMaxDaysPerWeek { get; set; }
        public int TeachersMinDaysPerWeek { get; set; }

        private ConstraintIntervalMaxDaysPerWeek teachersIntervalMaxDaysPerWeek;
        public ConstraintIntervalMaxDaysPerWeek TeachersIntervalMaxDaysPerWeek
        {
            get
            {
                return this.teachersIntervalMaxDaysPerWeek;
            }
            set
            {
                if (value != teachersIntervalMaxDaysPerWeek)
                {
                    this.teachersIntervalMaxDaysPerWeek = value;

                }
            }
        }

        public List<ConstraintActivityTagMaxHoursContinuously> TeachersActivityTagMaxHoursContinuouslyList
            = new List<ConstraintActivityTagMaxHoursContinuously>();
        public List<ConstraintActivityTagMaxHoursDaily> TeachersActivityTagMaxHoursDailyList
            = new List<ConstraintActivityTagMaxHoursDaily>();

        //space
        public int MaxBuildingChangesPerDay { get; set; }
        public int MaxBuildingChangesPerWeek { get; set; }
        public int MinGapsBetweenBuildingChanges { get; set; }

        public ConstraintsTeachers()
        {

            TeachersMaxGapsPerWeek = -1;
            TeachersMaxGapsPerDay = -1;
            TeachersMaxHoursDaily = -1;
            TeachersMinHoursDaily = -1;
            TeachersMaxHoursContinuously = -1;
            TeachersMaxDaysPerWeek = -1;
            TeachersMinDaysPerWeek = -1;

            if (this.teachersIntervalMaxDaysPerWeek == null)
            {
                this.teachersIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();
            }

            MaxBuildingChangesPerDay = -1;
            MaxBuildingChangesPerWeek = -1;
            MinGapsBetweenBuildingChanges = -1;
        }

        public void Clear()
        {
            TeachersMaxGapsPerWeek = -1;
            TeachersMaxGapsPerDay = -1;
            TeachersMaxHoursDaily = -1;
            TeachersMinHoursDaily = -1;
            TeachersMaxHoursContinuously = -1;
            TeachersMaxDaysPerWeek = -1;
            TeachersMinDaysPerWeek = -1;

            if (this.teachersIntervalMaxDaysPerWeek == null)
            {
                this.teachersIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();
            }

            MaxBuildingChangesPerDay = -1;
            MaxBuildingChangesPerWeek = -1;
            MinGapsBetweenBuildingChanges = -1;

            TeachersActivityTagMaxHoursContinuouslyList.Clear();
            TeachersActivityTagMaxHoursDailyList.Clear();
        }
    }
    
    /// <summary>
    /// class that contains constraints for all students
    /// </summary>
    public class ConstraintsStudents
    {
        public int StudentsMaxGapsPerWeek { get; set; }
        public int StudentsMaxGapsPerDay { get; set; }
        public int StudentsMaxHoursDaily { get; set; }
        public int StudentsMinHoursDaily { get; set; }
        public int StudentsMaxHoursContinuously { get; set; }
        public int StudentsMaxDaysPerWeek { get; set; }
        public int StudentsEarlyMaxBeginningsAtSecondHour { get; set; }

        private ConstraintIntervalMaxDaysPerWeek studentsIntervalMaxDaysPerWeek;
        public ConstraintIntervalMaxDaysPerWeek StudentsIntervalMaxDaysPerWeek
        { 
            get { return this.studentsIntervalMaxDaysPerWeek; }
            set { this.studentsIntervalMaxDaysPerWeek = value; }
        }

        public List<ConstraintActivityTagMaxHoursContinuously> StudentsActivityTagMaxHoursContinuouslyList
            = new List<ConstraintActivityTagMaxHoursContinuously>();
        public List<ConstraintActivityTagMaxHoursDaily> StudentsActivityTagMaxHoursDailyList
            = new List<ConstraintActivityTagMaxHoursDaily>();

        //space
        public int MaxBuildingChangesPerDay { get; set; }
        public int MaxBuildingChangesPerWeek { get; set; }
        public int MinGapsBetweenBuildingChanges { get; set; }

        public ConstraintsStudents()
        {
            StudentsMaxGapsPerWeek = -1;
            StudentsMaxGapsPerDay = -1;
            StudentsMaxHoursDaily = -1;
            StudentsMinHoursDaily = -1;
            StudentsMaxHoursContinuously = -1;
            StudentsMaxDaysPerWeek = -1;
            StudentsEarlyMaxBeginningsAtSecondHour = -1;

            if (this.studentsIntervalMaxDaysPerWeek == null)
            {
                this.studentsIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();
            }

            MaxBuildingChangesPerDay = -1;
            MaxBuildingChangesPerWeek = -1;
            MinGapsBetweenBuildingChanges = -1;
        }

        public void Clear()
        {
            StudentsMaxGapsPerWeek = -1;
            StudentsMaxGapsPerDay = -1;
            StudentsMaxHoursDaily = -1;
            StudentsMinHoursDaily = -1;
            StudentsMaxHoursContinuously = -1;
            StudentsMaxDaysPerWeek = -1;
            StudentsEarlyMaxBeginningsAtSecondHour = -1;

            if (this.studentsIntervalMaxDaysPerWeek == null)
            {
                this.studentsIntervalMaxDaysPerWeek = new ConstraintIntervalMaxDaysPerWeek();
            }

            MaxBuildingChangesPerDay = -1;
            MaxBuildingChangesPerWeek = -1;
            MinGapsBetweenBuildingChanges = -1;

            StudentsActivityTagMaxHoursContinuouslyList.Clear();
            StudentsActivityTagMaxHoursDailyList.Clear();
        }
    }

    /// <summary>
    /// class that contains a list of slots in which no activity can take place
    /// </summary>
    public class ConstraintBreakTimes
    {
        [Key]
        public int ConstraintBreakTimesId { get; set; }
        public List<TimeDayHour> BreakTimes = new List<TimeDayHour>();

        public ConstraintBreakTimes()
        {
            this.ConstraintBreakTimesId = App.RandomTool.Next();
        }

        public void Clear()
        {
            this.BreakTimes.Clear();
        }
    }

    /// <summary>
    /// general type of constraint
    /// </summary>
    public class ConstraintIntervalMaxDaysPerWeek 
    {
        [Key]
        public int ConstraintTeacherIntervalMaxDaysPerWeekId { get; set; }
        public int MaxDaysPerWeek { get; set; }
        public string IntervalStartHour { get; set; }
        public string IntervalEndHour { get; set; }
        public ConstraintIntervalMaxDaysPerWeek()
        {
            this.MaxDaysPerWeek = -1;
            this.ConstraintTeacherIntervalMaxDaysPerWeekId = App.RandomTool.Next();
        }
    }

    /// <summary>
    /// general type of constraint
    /// </summary>
    public class ConstraintActivityTagMaxHoursDaily 
    {
        [Key]
        public int ConstraintActivityTagMaxHoursDailyId { get; set; }        
        public string ActivityTag { get; set; }
        public int MaximumHoursDaily { get; set; }
        public ConstraintActivityTagMaxHoursDaily()
        {
            this.ConstraintActivityTagMaxHoursDailyId = App.RandomTool.Next();
        }
    }

    /// <summary>
    /// general type of constraint
    /// </summary>
    public class ConstraintActivityTagMaxHoursContinuously 
    {
        [Key]
        public int ConstraintActivityTagMaxHoursContinuouslyId { get; set; }
        public string ActivityTag { get; set; }
        public int MaximumHoursContinuously { get; set; }
        public ConstraintActivityTagMaxHoursContinuously()
        {
            this.ConstraintActivityTagMaxHoursContinuouslyId = App.RandomTool.Next();
        }

    }
    
    
    #endregion

    public class ConstraintActivityPreferredStartingTimes 
    {
        [Key]
        public int ConstraintActivityPreferredStartingTimesId { get; set; }
        public int ActivityId { get; set; }
        public int NumberOfPreferredStartingTimes { get; set; }
        public List<TimeDayHour> PreferredStartingTimes = new List<TimeDayHour>();     
        public ConstraintActivityPreferredStartingTimes()
        {
            this.ConstraintActivityPreferredStartingTimesId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivitiesSameStartingDay 
    {
        [Key]
        public int ConstraintActivitiesSameStartingDayId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintActivitiesSameStartingDay()
        {
            this.ConstraintActivitiesSameStartingDayId = App.RandomTool.Next();
        }
    }
        
    public class ConstraintActivitiesPreferredTimeSlots 
    {
        [Key]
        public int ConstraintActivitiesPreferredTimeSlotsId { get; set; }
        public Teacher Teacher { get; set; }
        public string StudentsName { get; set; }
        public string SubjectName { get; set; }
        public string ActivityTagName { get; set; }
        public List<TimeDayHour> PreferredTimeSlots = new List<TimeDayHour>();  
        public ConstraintActivitiesPreferredTimeSlots()
        {
            this.ConstraintActivitiesPreferredTimeSlotsId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivitiesPreferredStartingTimes 
    {
        [Key]
        public int ConstraintActivitiesPreferredStartingTimesId { get; set; }
        public Teacher Teacher { get; set; }
        public string StudentsName { get; set; }
        public string SubjectName { get; set; }
        public string ActivityTagName { get; set; }
        public List<TimeDayHour> PreferredStartingTime = new List<TimeDayHour>();  
        public ConstraintActivitiesPreferredStartingTimes()
        {
            this.ConstraintActivitiesPreferredStartingTimesId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintTwoActivitiesConsecutive 
    {
        [Key]
        public int ConstraintTwoActivitiesConsecutiveId { get; set; }
        public int FirstActivityId { get; set; }
        public int SecondActivityId { get; set; }
        public ConstraintTwoActivitiesConsecutive()
        {
            this.ConstraintTwoActivitiesConsecutiveId = App.RandomTool.Next();
        }
    }
        
    public class ConstraintMinDaysBetweenActivities 
    {
        [Key]
        public int ConstraintMinDaysBetweenActivitiesId { get; set; }
        public bool ConsecutiveIfSameDay { get; set; }
        public int NumberOfActivities { get; set; }
        public int MinDays { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintMinDaysBetweenActivities()
        {
            this.ConstraintMinDaysBetweenActivitiesId = App.RandomTool.Next();
        }
               
    }

    public class ConstraintActivitiesNotOverlapping 
    {
        [Key]
        public int ConstraintActivitiesNotOverlappingId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintActivitiesNotOverlapping()
        {
            this.ConstraintActivitiesNotOverlappingId = App.RandomTool.Next();
        }
    }
      
    public class ConstraintActivityEndsStudentsDay 
    {
        [Key]
        public int ConstraintActivityEndsStudentsDayId { get; set; }
        public int ActivityId { get; set; }
        public ConstraintActivityEndsStudentsDay()
        {
            this.ConstraintActivityEndsStudentsDayId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivityPreferredStartingTime 
    {
        [Key]
        public int ConstraintActivityPreferredStartingTimeId { get; set; }
        public int ActivityId { get; set; }
        public string PreferredDay { get; set; }
        public string PreferredHour { get; set; }
        public bool PermanentlyLocked { get; set; }
        public ConstraintActivityPreferredStartingTime()
        {
            this.ConstraintActivityPreferredStartingTimeId = App.RandomTool.Next();
        }
    }
  
    public class ConstraintActivityPreferredTimeSlots 
    {
        [Key]
        public int ConstraintActivityPreferredTimeSlotsId { get; set; }
        public int ActivityId { get; set; }
        public int NumberOfPreferredTimeSlots { get; set; }
        public List<TimeDayHour> PreferredTimeSlots = new List<TimeDayHour>();   
        public ConstraintActivityPreferredTimeSlots()
        {
            this.ConstraintActivityPreferredTimeSlotsId = App.RandomTool.Next();
        }

    }
  
    public class ConstraintActivitiesSameStartingHour 
    {
        [Key]
        public int ConstraintActivitiesSameStartingHourId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintActivitiesSameStartingHour()
        {
            this.ConstraintActivitiesSameStartingHourId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivitiesSameStartingTime 
    {
        [Key]
        public int ConstraintActivitiesSameStartingTimeId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintActivitiesSameStartingTime()
        {
            this.ConstraintActivitiesSameStartingTimeId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintThreeActivitiesGrouped 
    {
        [Key]
        public int ConstraintThreeActivitiesGroupedId { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintThreeActivitiesGrouped()
        {
            this.ConstraintThreeActivitiesGroupedId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintTwoActivitiesGrouped 
    {
        [Key]
        public int ConstraintTwoActivitiesGroupedId { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintTwoActivitiesGrouped()
        {
            this.ConstraintTwoActivitiesGroupedId = App.RandomTool.Next();
        }
    }

    public class ConstraintTwoActivitiesOrdered 
    {
        [Key]
        public int ConstraintTwoActivitiesOrderedId { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintTwoActivitiesOrdered()
        {
            this.ConstraintTwoActivitiesOrderedId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivitiesEndStudentsDay 
    {
        [Key]
        public int ConstraintActivitiesEndStudentsDayId { get; set; }
        public Teacher Teacher { get; set; }
        public string Students { get; set; }
        public string Subject { get; set; }
        public string ActivityTag { get; set; }
        public ConstraintActivitiesEndStudentsDay()
        {
            this.ConstraintActivitiesEndStudentsDayId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintSubactivitiesPreferredStartingTimes 
    {
        [Key]
        public int ConstraintSubactivitiesPreferredStartingTimesId { get; set; }
        public int ComponentNumber { get; set; }
        public Teacher Teacher { get; set; }
        public string Students { get; set; }
        public string Subject { get; set; }
        public string ActivityTag { get; set; }
        public int NumberOfPreferredStartingTimes { get; set; }
        public List<TimeDayHour> PreferredStartingTimes = new List<TimeDayHour>();
        public ConstraintSubactivitiesPreferredStartingTimes()
        {
            this.ConstraintSubactivitiesPreferredStartingTimesId = App.RandomTool.Next();
        }
        
    }
    
    public class ConstraintSubactivitiesPreferredTimeSlots 
    {
        [Key]
        public int ConstraintSubactivitiesPreferredTimeSlotsId { get; set; }
        public int ComponentNumber { get; set; }
        public Teacher Teacher { get; set; }
        public string Students { get; set; }
        public string Subject { get; set; }
        public string ActivityTag { get; set; }
        public int NumberOfPreferredTimeSlots { get; set; }
        public List<TimeDayHour> PreferredTimeSlots = new List<TimeDayHour>(); 
        public ConstraintSubactivitiesPreferredTimeSlots()
        {
            this.ConstraintSubactivitiesPreferredTimeSlotsId = App.RandomTool.Next();
        }
        
    }
    
    public class ConstraintMinGapsBetweenActivities 
    {
        [Key]
        public int ConstraintMinGapsBetweenActivitiesId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public int MinGaps { get; set; }
        public ConstraintMinGapsBetweenActivities()
        {
            this.ConstraintMinGapsBetweenActivitiesId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivitiesOccupyMaxTimeSlotsFromSelection 
    {
        [Key]
        public int ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public int NumberOfSelectedTimeSlots { get; set; }
        public List<TimeDayHour> SelectedTimeSlots = new List<TimeDayHour>();
        public int MaxNumberOfOccupiedTimeSlots { get; set; }
        public ConstraintActivitiesOccupyMaxTimeSlotsFromSelection()
        {
            this.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionId = App.RandomTool.Next();
        }
    }

    public class ConstraintMaxDaysBetweenActivities 
    {
        [Key]
        public int ConstraintMaxDaysBetweenActivitiesId { get; set; }
        public int NumberOfActivities { get; set; }
        public int MaxDays { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintMaxDaysBetweenActivities()
        {
            this.ConstraintMaxDaysBetweenActivitiesId = App.RandomTool.Next();
        }

    }
    
    

}
