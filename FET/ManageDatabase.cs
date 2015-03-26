using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Entity;
using FET.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Entity.Infrastructure; 

namespace FET
{   
    /// <summary>
    /// class used for import and export timetable to a database
    /// </summary>
    public class ManageDatabase : DbContext
    {
        public DbSet<GeneralInfoEntity> GeneralInfoEntity { get; set; }
        public DbSet<HoursEntity> Hour { get; set; }
        public DbSet<DayEntity> Day { get; set; }

        public DbSet<Year> Year { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Subgroup> Subgroup { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Subject> Subject { get; set; }

        public DbSet<ActivityTag> ActivityTag { get; set; }
        public DbSet<Activity> Activity { get; set; }

        public DbSet<Building> Building { get; set; }
        public DbSet<Room> Room { get; set; }

        public DbSet<TimeConstraint> TimeContraint { get; set; }
        public DbSet<SpaceConstraint> SpaceConstraint { get; set; }
        public DbSet<ActivityIdConstraintEntity> ActivityIdContraintEntity { get; set; }
        public DbSet<RoomNameConstraintEntity> RoomNameConstraintEntity { get; set; }
        
        public DbSet<ConstraintsTeachers> ConstraintsTeachers { get; set; }
        public DbSet<ConstraintsStudents> ConstraintsStudents { get; set; }
        #region time
        
        public DbSet<TimeDayHour> TimeDayHour { get; set; }
        //public DbSet<ConstraintBasicCompulsoryTime> ConstraintBasicCompulsoryTime { get; set; }
        public DbSet<ConstraintBreakTimes> ConstraintBreakTimes { get; set; }
        //public DbSet<ConstraintTeacherNotAvailableTimes> ConstraintTeacherNotAvailableTimes { get; set; }
        //public DbSet<ConstraintTeacherMaxDaysPerWeek> ConstraintTeacherMaxDaysPerWeek { get; set; }
        //public DbSet<ConstraintTeacherMaxGapsPerWeek> ConstraintTeacherMaxGapsPerWeek { get; set; }
        //public DbSet<ConstraintTeachersMaxGapsPerWeek> ConstraintTeachersMaxGapsPerWeek { get; set; }
       // public DbSet<ConstraintTeacherMaxHoursDaily> ConstraintTeacherMaxHoursDaily { get; set; }
        //public DbSet<ConstraintTeacherMinHoursDaily> ConstraintTeacherMinHoursDaily { get; set; }
        public DbSet<ConstraintIntervalMaxDaysPerWeek> ConstraintTeacherIntervalMaxDaysPerWeek { get; set; }
        public DbSet<ConstraintActivityPreferredStartingTimes> ConstraintActivityPreferredStartingTimes { get; set; }
        public DbSet<ConstraintActivitiesSameStartingDay> ConstraintActivitiesSameStartingDay { get; set; }
       // public DbSet<ConstraintTeacherMaxGapsPerDay> ConstraintTeacherMaxGapsPerDay { get; set; }
        public DbSet<ConstraintActivitiesPreferredTimeSlots> ConstraintActivitiesPreferredTimeSlots { get; set; }
        public DbSet<ConstraintActivitiesPreferredStartingTimes> ConstraintActivitiesPreferredStartingTimes { get; set; }
        public DbSet<ConstraintTwoActivitiesConsecutive> ConstraintTwoActivitiesConsecutive { get; set; }
        //public DbSet<ConstraintStudentsMaxHoursDaily> ConstraintStudentsMaxHoursDaily { get; set; }
        //public DbSet<ConstraintStudentsSetMaxGapsPerWeek> ConstraintStudentsSetMaxGapsPerWeek { get; set; }
        public DbSet<ConstraintMinDaysBetweenActivities> ConstraintMinDaysBetweenActivities { get; set; }
        //public DbSet<ConstraintStudentsEarlyMaxBeginningsAtSecondHour> ConstraintStudentsEarlyMaxBeginningsAtSecondHour { get; set; }
        //public DbSet<ConstraintStudentsMaxGapsPerWeek> ConstraintStudentsMaxGapsPerWeek { get; set; }
       // public DbSet<ConstraintStudentsMinHoursDaily> ConstraintStudentsMinHoursDaily { get; set; }
       // public DbSet<ConstraintTeachersMaxGapsPerDay> ConstraintTeachersMaxGapsPerDay { get; set; }
        public DbSet<ConstraintActivitiesNotOverlapping> ConstraintActivitiesNotOverlapping { get; set; }
        //public DbSet<ConstraintStudentsSetMaxHoursDaily> ConstraintStudentsSetMaxHoursDaily { get; set; }
        //public DbSet<ConstraintStudentsSetMinHoursDaily> ConstraintStudentsSetMinHoursDaily { get; set; }
        //public DbSet<ConstraintStudentsSetNotAvailableTimes> ConstraintStudentsSetNotAvailableTimes { get; set; }
       // public DbSet<ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour> ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour { get; set; }
        public DbSet<ConstraintActivityEndsStudentsDay> ConstraintActivityEndsStudentsDay { get; set; }
        public DbSet<ConstraintActivityPreferredStartingTime> ConstraintActivityPreferredStartingTime { get; set; }
        //public DbSet<ConstraintTeachersMaxHoursDaily> ConstraintTeachersMaxHoursDaily { get; set; }
        //public DbSet<ConstraintTeachersMinHoursDaily> ConstraintTeachersMinHoursDaily { get; set; }
        public DbSet<ConstraintActivityPreferredTimeSlots> ConstraintActivityPreferredTimeSlots { get; set; }
       // public DbSet<ConstraintStudentsMaxGapsPerDay> ConstraintStudentsMaxGapsPerDay { get; set; }
        public DbSet<ConstraintActivitiesSameStartingHour> ConstraintActivitiesSameStartingHour { get; set; }
        public DbSet<ConstraintActivitiesSameStartingTime> ConstraintActivitiesSameStartingTime { get; set; }
        //public DbSet<ConstraintStudentsIntervalMaxDaysPerWeek> ConstraintStudentsIntervalMaxDaysPerWeek { get; set; }
        //public DbSet<ConstraintTeachersIntervalMaxDaysPerWeek> ConstraintTeachersIntervalMaxDaysPerWeek { get; set; }
        //public DbSet<ConstraintStudentsActivityTagMaxHoursContinuously> ConstraintStudentsActivityTagMaxHoursContinuously { get; set; }
        //public DbSet<ConstraintTeacherMaxHoursContinuously> ConstraintTeacherMaxHoursContinuously { get; set; }
        public DbSet<ConstraintThreeActivitiesGrouped> ConstraintThreeActivitiesGrouped { get; set; }
        public DbSet<ConstraintTwoActivitiesGrouped> ConstraintTwoActivitiesGrouped { get; set; }
        //public DbSet<ConstraintStudentsSetMaxGapsPerDay> ConstraintStudentsSetMaxGapsPerDay { get; set; }
        public DbSet<ConstraintActivityTagMaxHoursDaily> ConstraintTeacherActivityTagMaxHoursDaily { get; set; }
        //public DbSet<ConstraintTeachersMaxHoursContinuously> ConstraintTeachersMaxHoursContinuously { get; set; }
        //public DbSet<ConstraintTeachersActivityTagMaxHoursContinuously> ConstraintTeachersActivityTagMaxHoursContinuously { get; set; }
        //public DbSet<ConstraintTeachersActivityTagMaxHoursDaily> ConstraintTeachersActivityTagMaxHoursDaily { get; set; }
        //public DbSet<ConstraintTeacherMinDaysPerWeek> ConstraintTeacherMinDaysPerWeek { get; set; }
       // public DbSet<ConstraintTeachersMaxDaysPerWeek> ConstraintTeachersMaxDaysPerWeek { get; set; }
        //public DbSet<ConstraintStudentsActivityTagMaxHoursDaily> ConstraintStudentsActivityTagMaxHoursDaily { get; set; }
       // public DbSet<ConstraintStudentsMaxHoursContinuously> ConstraintStudentsMaxHoursContinuously { get; set; }
        public DbSet<ConstraintTwoActivitiesOrdered> ConstraintTwoActivitiesOrdered { get; set; }
        public DbSet<ConstraintActivitiesEndStudentsDay> ConstraintActivitiesEndStudentsDay { get; set; }
        public DbSet<ConstraintSubactivitiesPreferredStartingTimes> ConstraintSubactivitiesPreferredStartingTimes { get; set; }
        //public DbSet<ConstraintStudentsSetActivityTagMaxHoursDaily> ConstraintStudentsSetActivityTagMaxHoursDaily { get; set; }
        public DbSet<ConstraintSubactivitiesPreferredTimeSlots> ConstraintSubactivitiesPreferredTimeSlots { get; set; }
        public DbSet<ConstraintMinGapsBetweenActivities> ConstraintMinGapsBetweenActivities { get; set; }
       // public DbSet<ConstraintStudentsSetIntervalMaxDaysPerWeek> ConstraintStudentsSetIntervalMaxDaysPerWeek { get; set; }
       // public DbSet<ConstraintTeachersMinDaysPerWeek> ConstraintTeachersMinDaysPerWeek { get; set; }
        public DbSet<ConstraintActivitiesOccupyMaxTimeSlotsFromSelection> ConstraintActivitiesOccupyMaxTimeSlotsFromSelection { get; set; }

        public DbSet<ConstraintActivityTagMaxHoursContinuously> ConstraintTeacherActivityTagMaxHoursContinuously { get; set; }
        //public DbSet<ConstraintStudentsSetMaxDaysPerWeek> ConstraintStudentsSetMaxDaysPerWeek { get; set; }
        //public DbSet<ConstraintStudentsSetMaxHoursContinuously> ConstraintStudentsSetMaxHoursContinuously { get; set; }
       // public DbSet<ConstraintStudentsSetActivityTagMaxHoursContinuously> ConstraintStudentsSetActivityTagMaxHoursContinuously { get; set; }
      //  public DbSet<ConstraintStudentsMaxDaysPerWeek> ConstraintStudentsMaxDaysPerWeek { get; set; }
        public DbSet<ConstraintMaxDaysBetweenActivities> ConstraintMaxDaysBetweenActivities { get; set; }
        #endregion

        #region space
        //public DbSet<ConstraintBasicCompulsorySpace> ConstraintBasicCompulsorySpace { get; set; }
        //public DbSet<ConstraintTeacherMinGapsBetweenBuildingChanges> ConstraintTeacherMinGapsBetweenBuildingChanges { get; set; }
        //public DbSet<ConstraintTeacherHomeRoom> ConstraintTeacherHomeRoom { get; set; }
        public DbSet<ConstraintSubjectActivityTagPreferredRoom> ConstraintSubjectActivityTagPreferredRoom { get; set; }
        //public DbSet<ConstraintRoomNotAvailableTimes> ConstraintRoomNotAvailableTimes { get; set; }
        //public DbSet<ConstraintTeacherHomeRooms> ConstraintTeacherHomeRooms { get; set; }
        //public DbSet<ConstraintActivityPreferredRoom> ConstraintActivityPreferredRoom { get; set; }
        //public DbSet<ConstraintActivityPreferredRooms> ConstraintActivityPreferredRooms { get; set; }
        //public DbSet<ConstraintSubjectPreferredRoom> ConstraintSubjectPreferredRoom { get; set; }
        public DbSet<ConstraintSubjectActivityTagPreferredRooms> ConstraintSubjectActivityTagPreferredRooms { get; set; }
        //public DbSet<ConstraintTeacherMaxBuildingChangesPerDay> ConstraintTeacherMaxBuildingChangesPerDay { get; set; }
        //public DbSet<ConstraintTeachersMaxBuildingChangesPerDay> ConstraintTeachersMaxBuildingChangesPerDay { get; set; }
       // public DbSet<ConstraintStudentsMaxBuildingChangesPerDay> ConstraintStudentsMaxBuildingChangesPerDay { get; set; }
       // public DbSet<ConstraintStudentsMinGapsBetweenBuildingChanges> ConstraintStudentsMinGapsBetweenBuildingChanges { get; set; }
      //  public DbSet<ConstraintStudentsMaxBuildingChangesPerWeek> ConstraintStudentsMaxBuildingChangesPerWeek { get; set; }
        //public DbSet<ConstraintActivityTagPreferredRoom> ConstraintActivityTagPreferredRoom { get; set; }
        //public DbSet<ConstraintActivityTagPreferredRooms> ConstraintActivityTagPreferredRooms { get; set; }
        //public DbSet<ConstraintTeacherMaxBuildingChangesPerWeek> ConstraintTeacherMaxBuildingChangesPerWeek { get; set; }
       // public DbSet<ConstraintTeachersMaxBuildingChangesPerWeek> ConstraintTeachersMaxBuildingChangesPerWeek { get; set; }
        //public DbSet<ConstraintStudentsSetHomeRoom> ConstraintStudentsSetHomeRoom { get; set; }
        //public DbSet<ConstraintSubjectPreferredRooms> ConstraintSubjectPreferredRooms { get; set; }
        public DbSet<ConstraintActivitiesOccupyMaxDifferentRooms> ConstraintActivitiesOccupyMaxDifferentRooms { get; set; }
        //public DbSet<ConstraintStudentsSetHomeRooms> ConstraintStudentsSetHomeRooms { get; set; }

      //  public DbSet<ConstraintTeachersMinGapsBetweenBuildingChanges> ConstraintTeachersMinGapsBetweenBuildingChanges { get; set; }
        //public DbSet<ConstraintStudentsSetMaxBuildingChangesPerDay> ConstraintStudentsSetMaxBuildingChangesPerDay { get; set; }
        //public DbSet<ConstraintStudentsSetMaxBuildingChangesPerWeek> ConstraintStudentsSetMaxBuildingChangesPerWeek { get; set; }
       // public DbSet<ConstraintStudentsSetMinGapsBetweenBuildingChanges> ConstraintStudentsSetMinGapsBetweenBuildingChanges { get; set; }
        public DbSet<ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive> ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive { get; set; }
        #endregion

        public ManageDatabase()
            : base("name=ManageDatabase")
        { 
        }

        public ManageDatabase(string connectionString)
            : base(connectionString)
        {
        }
        
        #region export
        /// <summary>
        /// export timetable to a database 
        /// </summary>
        /// <param name="conString"></param>
        /// <param name="bgworker"></param>
        /// <returns></returns>
        public static bool ExportToDatabase(string conString, BackgroundWorker bgworker)
        {
            if (!string.IsNullOrEmpty(conString))
            {
                using (var db = new ManageDatabase(conString))
                {
                    //Database.SetInitializer<ManageDatabase>(null);
                    if (bgworker.CancellationPending)
                    {
                        return false;
                    }
                    ClearEntities(db);

                    if (bgworker.CancellationPending)
                    {
                        return false;
                    }
                    ExportEntities(db);

                    if (bgworker.CancellationPending)
                    {
                        return false;
                    }
                    db.SaveChanges();
                }
            }

            else
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// add general data from timetable to export
        /// </summary>
        /// <param name="db"></param>
        private static void ExportEntities(ManageDatabase db)
        {
            #region general info
            GeneralInfoEntity generalInfoEntity = new GeneralInfoEntity();
            generalInfoEntity.InstitutionName = Timetable.GetInstance().InstitutionName;
            generalInfoEntity.Comments = Timetable.GetInstance().Comments;

            db.GeneralInfoEntity.Add(generalInfoEntity);

            foreach (var h in Timetable.GetInstance().HoursList)
            {
                db.Hour.Add(new HoursEntity(h, Timetable.GetInstance().HoursList.IndexOf(h)));
            }

            foreach (var d in Timetable.GetInstance().DaysList)
            {
                db.Day.Add(new DayEntity(d, Timetable.GetInstance().DaysList.IndexOf(d)));
            }


            foreach (var year in Timetable.GetInstance().ClassList)
            {
                db.Year.Add(year);
                foreach (var group in year.Groups)
                {
                    db.Group.Add(group);
                    foreach (var subgroup in group.Subgroups)
                    {
                        db.Subgroup.Add(subgroup);
                    }
                }
            }
            foreach (var teacher in Timetable.GetInstance().TeacherList)
            {
                db.Teacher.Add(teacher);
            }

            foreach (var subject in Timetable.GetInstance().SubjectList)
            {
                db.Subject.Add(subject);
            }

            foreach (var activityTag in Timetable.GetInstance().ActivitiyTagsList)
            {
                db.ActivityTag.Add(activityTag);
            }

            foreach (var activity in Timetable.GetInstance().ActivityList)
            {
                db.Activity.Add(activity);
            }

            foreach (var building in Timetable.GetInstance().BuildingsList)
            {
                db.Building.Add(building);
            }

            foreach (var room in Timetable.GetInstance().GetRoomsList())
            {
                db.Room.Add(room);
            }
            #endregion

            //time
            db.TimeContraint.Add(Timetable.GetInstance().TimeConstraints);
            ExportTimeContraintEntities(db);

            //space
            db.SpaceConstraint.Add(Timetable.GetInstance().SpaceConstraints);
            ExportSpaceConstraintEntities(db);
        }
        
        /// <summary>
        /// add time constraints to export
        /// </summary>
        /// <param name="db"></param>
        private static void ExportTimeContraintEntities(ManageDatabase db)
        {         
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintBasicCompulsoryTimeList)
            //{
            //    db.ConstraintBasicCompulsoryTime.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintBreakTimesList)
            //{
            //    db.ConstraintBreakTimes.Add(constr);
            //    foreach (var time in constr.BreakTimes)
            //    {
            //        db.TimeDayHour.Add(time);
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherNotAvailableTimesList)
            //{
            //    db.ConstraintTeacherNotAvailableTimes.Add(constr);
            //    foreach (var time in constr.NotAvailableTimes)
            //    {
            //        db.TimeDayHour.Add(time);
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxDaysPerWeekList)
            //{
            //    db.ConstraintTeacherMaxDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxGapsPerWeekList)
            //{
            //    db.ConstraintTeacherMaxGapsPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxGapsPerWeekList)
            //{
            //    //db.ConstraintTeachersMaxGapsPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxHoursDailyList)
            //{
            //    db.ConstraintTeacherMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMinHoursDailyList)
            //{
            //    db.ConstraintTeacherMinHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherIntervalMaxDaysPerWeekList)
            //{
            //    db.ConstraintTeacherIntervalMaxDaysPerWeek.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList)
            {
                db.ConstraintActivityPreferredStartingTimes.Add(constr);
                foreach (var time in constr.PreferredStartingTimes)
                {
                    db.TimeDayHour.Add(time);
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList)
            {
                db.ConstraintActivitiesSameStartingDay.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesSameStartingDayId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxGapsPerDayList)
            //{
            //    db.ConstraintTeacherMaxGapsPerDay.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList)
            {
                db.ConstraintActivitiesPreferredTimeSlots.Add(constr);
                foreach (var time in constr.PreferredTimeSlots)
                {
                    db.TimeDayHour.Add(time);
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList)
            {
                db.ConstraintActivitiesPreferredStartingTimes.Add(constr);
                foreach (var time in constr.PreferredStartingTime)
                {
                    db.TimeDayHour.Add(time);
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList)
            {
                db.ConstraintTwoActivitiesConsecutive.Add(constr);
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxHoursDailyList)
            //{
            //    db.ConstraintStudentsMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxGapsPerWeekList)
            //{
            //    db.ConstraintStudentsSetMaxGapsPerWeek.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList)
            {
                db.ConstraintMinDaysBetweenActivities.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintMinDaysBetweenActivitiesId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsEarlyMaxBeginningsAtSecondHourList)
            //{
            //    db.ConstraintStudentsEarlyMaxBeginningsAtSecondHour.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxGapsPerWeekList)
            //{
            //    db.ConstraintStudentsMaxGapsPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsMinHoursDailyList)
            //{
            //    db.ConstraintStudentsMinHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxGapsPerDayList)
            //{
            //    db.ConstraintTeachersMaxGapsPerDay.Add(constr);
            //}

            db.ConstraintsTeachers.Add(Timetable.GetInstance().TimeConstraints.ConstraintsTeachers);

            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList)
            {
                db.ConstraintActivitiesNotOverlapping.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesNotOverlappingId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxHoursDailyList)
            //{
            //    db.ConstraintStudentsSetMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMinHoursDailyList)
            //{
            //    db.ConstraintStudentsSetMinHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetNotAvailableTimesList)
            //{
            //    db.ConstraintStudentsSetNotAvailableTimes.Add(constr);
            //    foreach (var time in constr.NotAvailableTimes)
            //    {
            //        db.TimeDayHour.Add(time);
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHourList)
            //{
            //    db.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList)
            {
                db.ConstraintActivityEndsStudentsDay.Add(constr);
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList)
            {
                db.ConstraintActivityPreferredStartingTime.Add(constr);
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxHoursDailyList)
            //{
            //    db.ConstraintTeachersMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMinHoursDailyList)
            //{
            //    db.ConstraintTeachersMinHoursDaily.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList)
            {
                db.ConstraintActivityPreferredTimeSlots.Add(constr);
                foreach (var time in constr.PreferredTimeSlots)
                {
                    db.TimeDayHour.Add(time);
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxGapsPerDayList)
            //{
            //    db.ConstraintStudentsMaxGapsPerDay.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList)
            {
                db.ConstraintActivitiesSameStartingHour.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesSameStartingHourId));
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList)
            {
                db.ConstraintActivitiesSameStartingTime.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesSameStartingTimeId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsIntervalMaxDaysPerWeekList)
            //{
            //    db.ConstraintStudentsIntervalMaxDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersIntervalMaxDaysPerWeekList)
            //{
            //    db.ConstraintTeachersIntervalMaxDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsActivityTagMaxHoursContinuouslyList)
            //{
            //    db.ConstraintStudentsActivityTagMaxHoursContinuously.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxHoursContinuouslyList)
            //{
            //    db.ConstraintTeacherMaxHoursContinuously.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList)
            {
                db.ConstraintThreeActivitiesGrouped.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintThreeActivitiesGroupedId));
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList)
            {
                db.ConstraintTwoActivitiesGrouped.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintTwoActivitiesGroupedId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxGapsPerDayList)
            //{
            //    db.ConstraintStudentsSetMaxGapsPerDay.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherActivityTagMaxHoursDailyList)
            //{
            //    db.ConstraintTeacherActivityTagMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxHoursContinuouslyList)
            //{
            //    db.ConstraintTeachersMaxHoursContinuously.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList)
            //{
            //    db.ConstraintTeachersActivityTagMaxHoursContinuously.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersActivityTagMaxHoursDailyList)
            //{
            //    db.ConstraintTeachersActivityTagMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherMinDaysPerWeekList)
            //{
            //    db.ConstraintTeacherMinDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxDaysPerWeekList)
            //{
            //    db.ConstraintTeachersMaxDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsActivityTagMaxHoursDailyList)
            //{
            //    db.ConstraintStudentsActivityTagMaxHoursDaily.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxHoursContinuouslyList)
            //{
            //    db.ConstraintStudentsMaxHoursContinuously.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList)
            {
                db.ConstraintTwoActivitiesOrdered.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintTwoActivitiesOrderedId));
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList)
            {
                db.ConstraintActivitiesEndStudentsDay.Add(constr);
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList)
            {
                db.ConstraintSubactivitiesPreferredStartingTimes.Add(constr);
                foreach (var time in constr.PreferredStartingTimes)
                {
                    db.TimeDayHour.Add(time);
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursDailyList)
            //{
            //    db.ConstraintStudentsSetActivityTagMaxHoursDaily.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList)
            {
                db.ConstraintSubactivitiesPreferredTimeSlots.Add(constr);
                foreach (var time in constr.PreferredTimeSlots)
                {
                    db.TimeDayHour.Add(time);
                }
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList)
            {
                db.ConstraintMinGapsBetweenActivities.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintMinGapsBetweenActivitiesId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetIntervalMaxDaysPerWeekList)
            //{
            //    db.ConstraintStudentsSetIntervalMaxDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeachersMinDaysPerWeekList)
            //{
            //    db.ConstraintTeachersMinDaysPerWeek.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList)
            {
                db.ConstraintActivitiesOccupyMaxTimeSlotsFromSelection.Add(constr);
                foreach (var time in constr.SelectedTimeSlots)
                {
                    db.TimeDayHour.Add(time);
                }
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionId));
                }
            }

            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherActivityTagMaxHoursContinuouslyList)
            //{
            //    db.ConstraintTeacherActivityTagMaxHoursContinuously.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxDaysPerWeekList)
            //{
            //    db.ConstraintStudentsSetMaxDaysPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxHoursContinuouslyList)
            //{
            //    db.ConstraintStudentsSetMaxHoursContinuously.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursContinuouslyList)
            //{
            //    db.ConstraintStudentsSetActivityTagMaxHoursContinuously.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxDaysPerWeekList)
            //{
            //    db.ConstraintStudentsMaxDaysPerWeek.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList)
            {
                db.ConstraintMaxDaysBetweenActivities.Add(constr);                
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintMaxDaysBetweenActivitiesId));
                }
            }
        }

        /// <summary>
        /// add space constraints to export
        /// </summary>
        /// <param name="db"></param>
        private static void ExportSpaceConstraintEntities(ManageDatabase db)
        {
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintBasicCompulsorySpaceList)
            //{
            //    db.ConstraintBasicCompulsorySpace.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeacherMinGapsBetweenBuildingChangesList)
            //{
            //    db.ConstraintTeacherMinGapsBetweenBuildingChanges.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeacherHomeRoomList)
            //{
            //    db.ConstraintTeacherHomeRoom.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList)
            {
                db.ConstraintSubjectActivityTagPreferredRoom.Add(constr);
            }
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintRoomNotAvailableTimesList)
            //{
            //    db.ConstraintRoomNotAvailableTimes.Add(constr);
            //    foreach (var time in constr.NotAvailableTimes)
            //    {
            //        db.TimeDayHour.Add(time);
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeacherHomeRoomsList)
            //{
            //    db.ConstraintTeacherHomeRooms.Add(constr);
            //    foreach (var room in constr.Rooms)
            //    {
            //        db.RoomNameConstraintEntity.Add(new RoomNameConstraintEntity(room.Name, constr.ConstraintTeacherHomeRoomsId));
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivityPreferredRoomList)
            //{
            //    db.ConstraintActivityPreferredRoom.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivityPreferredRoomsList)
            //{
            //    db.ConstraintActivityPreferredRooms.Add(constr);
            //    foreach (var room in constr.Rooms)
            //    {
            //        db.RoomNameConstraintEntity.Add(new RoomNameConstraintEntity(room.Name, constr.ConstraintActivityPreferredRoomsId));
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectPreferredRoomList)
            //{
            //    db.ConstraintSubjectPreferredRoom.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList)
            {
                db.ConstraintSubjectActivityTagPreferredRooms.Add(constr);
                foreach (var room in constr.Rooms)
                {
                    db.RoomNameConstraintEntity.Add(new RoomNameConstraintEntity(room.Name, constr.ConstraintSubjectActivityTagPreferredRoomsId));
                }
            }
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeacherMaxBuildingChangesPerDayList)
            //{
            //    db.ConstraintTeacherMaxBuildingChangesPerDay.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeachersMaxBuildingChangesPerDayList)
            //{
            //    db.ConstraintTeachersMaxBuildingChangesPerDay.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsMaxBuildingChangesPerDayList)
            //{
            //    db.ConstraintStudentsMaxBuildingChangesPerDay.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsMinGapsBetweenBuildingChangesList)
            //{
            //    db.ConstraintStudentsMinGapsBetweenBuildingChanges.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsMaxBuildingChangesPerWeekList)
            //{
            //    db.ConstraintStudentsMaxBuildingChangesPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivityTagPreferredRoomList)
            //{
            //    db.ConstraintActivityTagPreferredRoom.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivityTagPreferredRoomsList)
            //{
            //    db.ConstraintActivityTagPreferredRooms.Add(constr);
            //    foreach (var room in constr.Rooms)
            //    {
            //        db.RoomNameConstraintEntity.Add(new RoomNameConstraintEntity(room.Name, constr.ConstraintActivityTagPreferredRoomsId));
            //    }
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeacherMaxBuildingChangesPerWeekList)
            //{
            //    db.ConstraintTeacherMaxBuildingChangesPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeachersMaxBuildingChangesPerWeekList)
            //{
            //    db.ConstraintTeachersMaxBuildingChangesPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetHomeRoomList)
            //{
            //    db.ConstraintStudentsSetHomeRoom.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectPreferredRoomsList)
            //{
            //    db.ConstraintSubjectPreferredRooms.Add(constr);
            //    foreach (var room in constr.Rooms)
            //    {
            //        db.RoomNameConstraintEntity.Add(new RoomNameConstraintEntity(room.Name, constr.ConstraintSubjectPreferredRoomsId));
            //    }
            //}
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
            {
                db.ConstraintActivitiesOccupyMaxDifferentRooms.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesOccupyMaxDifferentRoomsId));
                } 
            }
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetHomeRoomsList)
            //{
            //    db.ConstraintStudentsSetHomeRooms.Add(constr);
            //    foreach (var room in constr.Rooms)
            //    {
            //        db.RoomNameConstraintEntity.Add(new RoomNameConstraintEntity(room.Name, constr.ConstraintStudentsSetHomeRoomsId));
            //    }
            //}

            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeachersMinGapsBetweenBuildingChangesList)
            //{
            //    db.ConstraintTeachersMinGapsBetweenBuildingChanges.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetMaxBuildingChangesPerDayList)
            //{
            //    db.ConstraintStudentsSetMaxBuildingChangesPerDay.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetMaxBuildingChangesPerWeekList)
            //{
            //    db.ConstraintStudentsSetMaxBuildingChangesPerWeek.Add(constr);
            //}
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetMinGapsBetweenBuildingChangesList)
            //{
            //    db.ConstraintStudentsSetMinGapsBetweenBuildingChanges.Add(constr);
            //}
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList)
            {
                db.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive.Add(constr);
                foreach (var a in constr.ActivityIds)
                {
                    db.ActivityIdContraintEntity.Add(new ActivityIdConstraintEntity(a, constr.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId));
                }
            }
        }
        #endregion

        /// <summary>
        /// clear database before exporting 
        /// </summary>
        /// <param name="db"></param>
        private static void ClearEntities(ManageDatabase db)
        {
            db.GeneralInfoEntity.RemoveRange(db.GeneralInfoEntity);
            db.Hour.RemoveRange(db.Hour);
            db.Day.RemoveRange(db.Day);
            
            db.Subgroup.RemoveRange(db.Subgroup);
            db.Group.RemoveRange(db.Group);
            db.Year.RemoveRange(db.Year);            

            db.Teacher.RemoveRange(db.Teacher);
           
            db.Room.RemoveRange(db.Room);
            db.Building.RemoveRange(db.Building);

            db.ActivityTag.RemoveRange(db.ActivityTag);
            db.Activity.RemoveRange(db.Activity);

            db.TimeDayHour.RemoveRange(db.TimeDayHour);
            db.ActivityIdContraintEntity.RemoveRange(db.ActivityIdContraintEntity);
            db.RoomNameConstraintEntity.RemoveRange(db.RoomNameConstraintEntity);

            db.ConstraintBreakTimes.RemoveRange(db.ConstraintBreakTimes);
            //db.ConstraintBasicCompulsoryTime.RemoveRange(db.ConstraintBasicCompulsoryTime);
            //db.ConstraintTeacherNotAvailableTimes.RemoveRange(db.ConstraintTeacherNotAvailableTimes);
            //db.ConstraintTeacherMaxDaysPerWeek.RemoveRange(db.ConstraintTeacherMaxDaysPerWeek);
            //db.ConstraintTeacherMaxGapsPerWeek.RemoveRange(db.ConstraintTeacherMaxGapsPerWeek);
            //db.ConstraintTeachersMaxGapsPerWeek.RemoveRange(db.ConstraintTeachersMaxGapsPerWeek);
            //db.ConstraintTeacherMaxHoursDaily.RemoveRange(db.ConstraintTeacherMaxHoursDaily);
            //db.ConstraintTeacherMinHoursDaily.RemoveRange(db.ConstraintTeacherMinHoursDaily);
            db.ConstraintTeacherIntervalMaxDaysPerWeek.RemoveRange(db.ConstraintTeacherIntervalMaxDaysPerWeek);
            db.ConstraintActivityPreferredStartingTimes.RemoveRange(db.ConstraintActivityPreferredStartingTimes);
            db.ConstraintActivitiesSameStartingDay.RemoveRange(db.ConstraintActivitiesSameStartingDay);
            //db.ConstraintTeacherMaxGapsPerDay.RemoveRange(db.ConstraintTeacherMaxGapsPerDay);
            db.ConstraintActivitiesPreferredTimeSlots.RemoveRange(db.ConstraintActivitiesPreferredTimeSlots);
            db.ConstraintActivitiesPreferredStartingTimes.RemoveRange(db.ConstraintActivitiesPreferredStartingTimes);
            db.ConstraintTwoActivitiesConsecutive.RemoveRange(db.ConstraintTwoActivitiesConsecutive);
            //db.ConstraintStudentsMaxHoursDaily.RemoveRange(db.ConstraintStudentsMaxHoursDaily);
            //db.ConstraintStudentsSetMaxGapsPerWeek.RemoveRange(db.ConstraintStudentsSetMaxGapsPerWeek);
            db.ConstraintMinDaysBetweenActivities.RemoveRange(db.ConstraintMinDaysBetweenActivities);
            //db.ConstraintStudentsEarlyMaxBeginningsAtSecondHour.RemoveRange(db.ConstraintStudentsEarlyMaxBeginningsAtSecondHour);
            //db.ConstraintStudentsMaxGapsPerWeek.RemoveRange(db.ConstraintStudentsMaxGapsPerWeek);
            //db.ConstraintStudentsMinHoursDaily.RemoveRange(db.ConstraintStudentsMinHoursDaily);
            //db.ConstraintTeachersMaxGapsPerDay.RemoveRange(db.ConstraintTeachersMaxGapsPerDay);
            db.ConstraintActivitiesNotOverlapping.RemoveRange(db.ConstraintActivitiesNotOverlapping);
            //db.ConstraintStudentsSetMaxHoursDaily.RemoveRange(db.ConstraintStudentsSetMaxHoursDaily);
            //db.ConstraintStudentsSetMinHoursDaily.RemoveRange(db.ConstraintStudentsSetMinHoursDaily);
            //db.ConstraintStudentsSetNotAvailableTimes.RemoveRange(db.ConstraintStudentsSetNotAvailableTimes);
            //db.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour.RemoveRange(db.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour);
            db.ConstraintActivityEndsStudentsDay.RemoveRange(db.ConstraintActivityEndsStudentsDay);
            db.ConstraintActivityPreferredStartingTime.RemoveRange(db.ConstraintActivityPreferredStartingTime);
            //db.ConstraintTeachersMaxHoursDaily.RemoveRange(db.ConstraintTeachersMaxHoursDaily);
            //db.ConstraintTeachersMinHoursDaily.RemoveRange(db.ConstraintTeachersMinHoursDaily);
            db.ConstraintActivityPreferredTimeSlots.RemoveRange(db.ConstraintActivityPreferredTimeSlots);
            //db.ConstraintStudentsMaxGapsPerDay.RemoveRange(db.ConstraintStudentsMaxGapsPerDay);
            db.ConstraintActivitiesSameStartingHour.RemoveRange(db.ConstraintActivitiesSameStartingHour);
            db.ConstraintActivitiesSameStartingTime.RemoveRange(db.ConstraintActivitiesSameStartingTime);
            //db.ConstraintStudentsIntervalMaxDaysPerWeek.RemoveRange(db.ConstraintStudentsIntervalMaxDaysPerWeek);
            //db.ConstraintTeachersIntervalMaxDaysPerWeek.RemoveRange(db.ConstraintTeachersIntervalMaxDaysPerWeek);
            //db.ConstraintStudentsActivityTagMaxHoursContinuously.RemoveRange(db.ConstraintStudentsActivityTagMaxHoursContinuously);
            //db.ConstraintTeacherMaxHoursContinuously.RemoveRange(db.ConstraintTeacherMaxHoursContinuously);
            db.ConstraintThreeActivitiesGrouped.RemoveRange(db.ConstraintThreeActivitiesGrouped);
            db.ConstraintTwoActivitiesGrouped.RemoveRange(db.ConstraintTwoActivitiesGrouped);
            //db.ConstraintStudentsSetMaxGapsPerDay.RemoveRange(db.ConstraintStudentsSetMaxGapsPerDay);
            db.ConstraintTeacherActivityTagMaxHoursDaily.RemoveRange(db.ConstraintTeacherActivityTagMaxHoursDaily);
           // db.ConstraintTeachersMaxHoursContinuously.RemoveRange(db.ConstraintTeachersMaxHoursContinuously);
            //db.ConstraintTeachersActivityTagMaxHoursContinuously.RemoveRange(db.ConstraintTeachersActivityTagMaxHoursContinuously);
            //db.ConstraintTeachersActivityTagMaxHoursDaily.RemoveRange(db.ConstraintTeachersActivityTagMaxHoursDaily);
            //db.ConstraintTeacherMinDaysPerWeek.RemoveRange(db.ConstraintTeacherMinDaysPerWeek);
            //db.ConstraintTeachersMaxDaysPerWeek.RemoveRange(db.ConstraintTeachersMaxDaysPerWeek);
            //db.ConstraintStudentsActivityTagMaxHoursDaily.RemoveRange(db.ConstraintStudentsActivityTagMaxHoursDaily);
            //db.ConstraintStudentsMaxHoursContinuously.RemoveRange(db.ConstraintStudentsMaxHoursContinuously);
            db.ConstraintTwoActivitiesOrdered.RemoveRange(db.ConstraintTwoActivitiesOrdered);
            db.ConstraintActivitiesEndStudentsDay.RemoveRange(db.ConstraintActivitiesEndStudentsDay);
            db.ConstraintSubactivitiesPreferredStartingTimes.RemoveRange(db.ConstraintSubactivitiesPreferredStartingTimes);
            //db.ConstraintStudentsSetActivityTagMaxHoursDaily.RemoveRange(db.ConstraintStudentsSetActivityTagMaxHoursDaily);
            db.ConstraintSubactivitiesPreferredTimeSlots.RemoveRange(db.ConstraintSubactivitiesPreferredTimeSlots);
            db.ConstraintMinGapsBetweenActivities.RemoveRange(db.ConstraintMinGapsBetweenActivities);
           // db.ConstraintStudentsSetIntervalMaxDaysPerWeek.RemoveRange(db.ConstraintStudentsSetIntervalMaxDaysPerWeek);
            //db.ConstraintTeachersMinDaysPerWeek.RemoveRange(db.ConstraintTeachersMinDaysPerWeek);
            db.ConstraintActivitiesOccupyMaxTimeSlotsFromSelection.RemoveRange(db.ConstraintActivitiesOccupyMaxTimeSlotsFromSelection);

            db.ConstraintTeacherActivityTagMaxHoursContinuously.RemoveRange(db.ConstraintTeacherActivityTagMaxHoursContinuously);
            //db.ConstraintStudentsSetMaxDaysPerWeek.RemoveRange(db.ConstraintStudentsSetMaxDaysPerWeek);
            //db.ConstraintStudentsSetMaxHoursContinuously.RemoveRange(db.ConstraintStudentsSetMaxHoursContinuously);
           // db.ConstraintStudentsSetActivityTagMaxHoursContinuously.RemoveRange(db.ConstraintStudentsSetActivityTagMaxHoursContinuously);
           // db.ConstraintStudentsMaxDaysPerWeek.RemoveRange(db.ConstraintStudentsMaxDaysPerWeek);
            db.ConstraintMaxDaysBetweenActivities.RemoveRange(db.ConstraintMaxDaysBetweenActivities);


            //space
            //db.ConstraintBasicCompulsorySpace.RemoveRange(db.ConstraintBasicCompulsorySpace);
            //db.ConstraintTeacherMinGapsBetweenBuildingChanges.RemoveRange(db.ConstraintTeacherMinGapsBetweenBuildingChanges);
            //db.ConstraintTeacherHomeRoom.RemoveRange(db.ConstraintTeacherHomeRoom);
            db.ConstraintSubjectActivityTagPreferredRoom.RemoveRange(db.ConstraintSubjectActivityTagPreferredRoom);
            //db.ConstraintRoomNotAvailableTimes.RemoveRange(db.ConstraintRoomNotAvailableTimes);
            //db.ConstraintTeacherHomeRooms.RemoveRange(db.ConstraintTeacherHomeRooms);
            //db.ConstraintActivityPreferredRoom.RemoveRange(db.ConstraintActivityPreferredRoom);
            //db.ConstraintActivityPreferredRooms.RemoveRange(db.ConstraintActivityPreferredRooms);
            //db.ConstraintSubjectPreferredRoom.RemoveRange(db.ConstraintSubjectPreferredRoom);
            db.ConstraintSubjectActivityTagPreferredRooms.RemoveRange(db.ConstraintSubjectActivityTagPreferredRooms);
            //db.ConstraintTeacherMaxBuildingChangesPerDay.RemoveRange(db.ConstraintTeacherMaxBuildingChangesPerDay);
            //db.ConstraintTeachersMaxBuildingChangesPerDay.RemoveRange(db.ConstraintTeachersMaxBuildingChangesPerDay);
            //db.ConstraintStudentsMaxBuildingChangesPerDay.RemoveRange(db.ConstraintStudentsMaxBuildingChangesPerDay);
            //db.ConstraintStudentsMinGapsBetweenBuildingChanges.RemoveRange(db.ConstraintStudentsMinGapsBetweenBuildingChanges);
            //db.ConstraintStudentsMaxBuildingChangesPerWeek.RemoveRange(db.ConstraintStudentsMaxBuildingChangesPerWeek);
            //db.ConstraintActivityTagPreferredRoom.RemoveRange(db.ConstraintActivityTagPreferredRoom);
            //db.ConstraintActivityTagPreferredRooms.RemoveRange(db.ConstraintActivityTagPreferredRooms);
            //db.ConstraintTeacherMaxBuildingChangesPerWeek.RemoveRange(db.ConstraintTeacherMaxBuildingChangesPerWeek);
            //db.ConstraintTeachersMaxBuildingChangesPerWeek.RemoveRange(db.ConstraintTeachersMaxBuildingChangesPerWeek);
            //db.ConstraintStudentsSetHomeRoom.RemoveRange(db.ConstraintStudentsSetHomeRoom);
            //db.ConstraintSubjectPreferredRooms.RemoveRange(db.ConstraintSubjectPreferredRooms);
            db.ConstraintActivitiesOccupyMaxDifferentRooms.RemoveRange(db.ConstraintActivitiesOccupyMaxDifferentRooms);
            //db.ConstraintStudentsSetHomeRooms.RemoveRange(db.ConstraintStudentsSetHomeRooms);

            //db.ConstraintTeachersMinGapsBetweenBuildingChanges.RemoveRange(db.ConstraintTeachersMinGapsBetweenBuildingChanges);
            //db.ConstraintStudentsSetMaxBuildingChangesPerDay.RemoveRange(db.ConstraintStudentsSetMaxBuildingChangesPerDay);
            //db.ConstraintStudentsSetMaxBuildingChangesPerWeek.RemoveRange(db.ConstraintStudentsSetMaxBuildingChangesPerWeek);
            //db.ConstraintStudentsSetMinGapsBetweenBuildingChanges.RemoveRange(db.ConstraintStudentsSetMinGapsBetweenBuildingChanges);
            db.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive.RemoveRange(db.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive);
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            { }
        }
        
        #region import
        
        /// <summary>
        /// load data from database in timetable
        /// </summary>
        /// <param name="conString"></param>
        /// <returns></returns>
        public static bool ImportFromDatabase(string conString)
        {

            if (!string.IsNullOrEmpty(conString))
            {
                using (var db = new ManageDatabase(conString))
                {
                    //Database.SetInitializer<ManageDatabase>(null);
                    Timetable.GetInstance().Clear();
                    LoadDataFromDB(db);

                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// load entities in timetable
        /// </summary>
        /// <param name="db"></param>
        private static void LoadDataFromDB(ManageDatabase db)
        {
            #region general
            foreach (var general in db.GeneralInfoEntity)
            {
                Timetable.GetInstance().InstitutionName = general.InstitutionName;
                Timetable.GetInstance().Comments = general.Comments;
            }

            foreach (var h in db.Hour.OrderBy(i => i.OrderNumber))
            {
                Timetable.GetInstance().HoursList.Add(h.Hour);
            }
            foreach (var d in db.Day.OrderBy(i => i.OrderNumber))
            {
                Timetable.GetInstance().DaysList.Add(d.Day);
            }

            foreach (var y in db.Year)
            {
                Timetable.GetInstance().ClassList.Add(y);
            }

            foreach (var g in db.Group)
            {
                var year = (from y in Timetable.GetInstance().ClassList
                            where y.Name.Equals(g.YearName)
                            select y).First();

                if (year != null)
                {
                    year.Groups.Add(g);
                }

            }

            foreach (var subg in db.Subgroup)
            {

                Group group = null;
                bool grFound = false;
                foreach (var y in Timetable.GetInstance().ClassList)
                {
                    if (!grFound)
                    {
                        foreach (var g in y.Groups)
                        {
                            if (subg.GroupName.Equals(g.Name))
                            {
                                group = g;
                                grFound = true;
                                break;
                            }
                        }
                    }
                }

                if (group != null)
                {
                    group.Subgroups.Add(subg);
                }
            }

            foreach (var t in db.Teacher)
            {
                Timetable.GetInstance().TeacherList.Add(t);
            }
            foreach (var t in db.Subject)
            {
                Timetable.GetInstance().SubjectList.Add(t);
            }
            foreach (var t in db.ActivityTag)
            {
                Timetable.GetInstance().ActivitiyTagsList.Add(t);
            }
            foreach (var t in db.Activity)
            {
                Timetable.GetInstance().ActivityList.Add(t);
            }
            foreach (var t in db.Building)
            {
                Timetable.GetInstance().BuildingsList.Add(t);
            }
            foreach (var r in db.Room)
            {
                var building = (from b in Timetable.GetInstance().BuildingsList
                                where b.Name == r.BuildingName
                                select b).First();

                if (building != null)
                {
                    building.Rooms.Add(r);
                }
                Timetable.GetInstance().GetRoomsList().Add(r);
            }
            #endregion

            #region time constraints
            //foreach (var constr in db.ConstraintBasicCompulsoryTime)
            //{
            //    if (!constr.GetType().IsSubclassOf(Type.GetType("FET.Data.ConstraintBasicCompulsoryTime")))
            //    {
            //        Timetable.GetInstance().TimeConstraints.ConstraintBasicCompulsoryTimeList.Add(constr);
            //    }
            //}
            //foreach (var constr in db.ConstraintBreakTimes)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintBreakTimesList.Add(constr);
            //}
            //add time to constraints
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintBreakTimesList)
            //{
            //    var timeBreaks = from tb in db.TimeDayHour
            //                     where tb.ParentId == constr.ConstraintBreakTimesId
            //                     select tb;
            //    foreach (var timeDayHour in timeBreaks)
            //    {
            //        constr.BreakTimes.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintBreakTimesId));

            //    }
            //}
            //foreach (var constr in db.ConstraintTeacherNotAvailableTimes)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherNotAvailableTimesList.Add(constr);
            //}
            //add time to constraints
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTeacherNotAvailableTimesList)
            //{
            //    var timeBreaks = from tb in db.TimeDayHour
            //                     where tb.ConstraintId == constr.ConstraintTeacherNotAvailableTimesId
            //                     select tb;
            //    foreach (var timeDayHour in timeBreaks)
            //    {
            //        constr.NotAvailableTimes.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintTeacherNotAvailableTimesId));

            //    }
            //}
            //foreach (var constr in db.ConstraintTeacherMaxDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeacherMaxGapsPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxGapsPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMaxGapsPerWeek)
            //{
            //    //Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxGapsPerWeekList.Add(constr);
            //    Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerWeek = constr.MaxGaps;
            //}
            //foreach (var constr in db.ConstraintTeacherMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeacherMinHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMinHoursDailyList.Add(constr);
            //}
            foreach (var constr in db.ConstraintTeacherIntervalMaxDaysPerWeek)
            {
                //Timetable.GetInstance().TimeConstraints.ConstraintTeacherIntervalMaxDaysPerWeekList.Add(constr);
            }
            foreach (var constr in db.ConstraintActivityPreferredStartingTimes)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList.Add(constr);
            }

            foreach (var constr in db.ConstraintActivitiesSameStartingDay)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList.Add(constr);

            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesSameStartingDayId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }


            //foreach (var constr in db.ConstraintTeacherMaxGapsPerDay)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxGapsPerDayList.Add(constr);
            //}
            foreach (var constr in db.ConstraintActivitiesPreferredTimeSlots)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList.Add(constr);
            }
            //add time slots
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList)
            {
                var times = from tb in db.TimeDayHour
                            where tb.ParentId == constr.ConstraintActivitiesPreferredTimeSlotsId
                            select tb;
                foreach (var timeDayHour in times)
                {
                    constr.PreferredTimeSlots.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintActivitiesPreferredTimeSlotsId));

                }
            }


            foreach (var constr in db.ConstraintActivitiesPreferredStartingTimes)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList.Add(constr);
            }
            //add time
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList)
            {
                var times = from tb in db.TimeDayHour
                            where tb.ParentId == constr.ConstraintActivitiesPreferredStartingTimesId
                            select tb;
                foreach (var timeDayHour in times)
                {
                    constr.PreferredStartingTime.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintActivitiesPreferredStartingTimesId));

                }
            }


            foreach (var constr in db.ConstraintTwoActivitiesConsecutive)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Add(constr);
            }
            //foreach (var constr in db.ConstraintStudentsMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetMaxGapsPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxGapsPerWeekList.Add(constr);
            //}
            foreach (var constr in db.ConstraintMinDaysBetweenActivities)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList.Add(constr);
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintMinDaysBetweenActivitiesId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            //foreach (var constr in db.ConstraintStudentsEarlyMaxBeginningsAtSecondHour)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsEarlyMaxBeginningsAtSecondHourList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMaxGapsPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxGapsPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMinHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsMinHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMaxGapsPerDay)
            //{
            //    //Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxGapsPerDayList.Add(constr);
            //}
            foreach (var constr in db.ConstraintActivitiesNotOverlapping)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList.Add(constr);
            }
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesNotOverlappingId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            //foreach (var constr in db.ConstraintStudentsSetMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetMinHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMinHoursDailyList.Add(constr);
            //}

            //foreach (var constr in db.ConstraintStudentsSetNotAvailableTimes)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetNotAvailableTimesList.Add(constr);
            //}
            ////add time
            //foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetNotAvailableTimesList)
            //{
            //    var times = from tb in db.TimeDayHour
            //                where tb.ParentId == constr.ConstraintStudentsSetNotAvailableTimesId
            //                select tb;
            //    foreach (var timeDayHour in times)
            //    {
            //        constr.NotAvailableTimes.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintStudentsSetNotAvailableTimesId));

            //    }
            //}

            //foreach (var constr in db.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHourList.Add(constr);
            //}
            foreach (var constr in db.ConstraintActivityEndsStudentsDay)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList.Add(constr);
            }
            foreach (var constr in db.ConstraintActivityPreferredStartingTime)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList.Add(constr);
            }

            //foreach (var constr in db.ConstraintTeachersMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxHoursDailyList.Add(constr);
            //}
            

            //foreach (var constr in db.ConstraintTeachersMinHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeachersMinHoursDailyList.Add(constr);
            //}

            foreach (var constr in db.ConstraintActivityPreferredTimeSlots)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList.Add(constr);
            }
            //add time
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList)
            {
                var times = from tb in db.TimeDayHour
                            where tb.ParentId == constr.ConstraintActivityPreferredTimeSlotsId
                            select tb;
                foreach (var timeDayHour in times)
                {
                    constr.PreferredTimeSlots.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintActivityPreferredTimeSlotsId));

                }
            }

            //foreach (var constr in db.ConstraintStudentsMaxGapsPerDay)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxGapsPerDayList.Add(constr);
            //}

            foreach (var constr in db.ConstraintActivitiesSameStartingHour)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesSameStartingHourId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            foreach (var constr in db.ConstraintActivitiesSameStartingTime)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesSameStartingTimeId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }


            //foreach (var constr in db.ConstraintStudentsIntervalMaxDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsIntervalMaxDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersIntervalMaxDaysPerWeek)
            //{
            //   // Timetable.GetInstance().TimeConstraints.ConstraintTeachersIntervalMaxDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsActivityTagMaxHoursContinuously)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsActivityTagMaxHoursContinuouslyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeacherMaxHoursContinuously)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMaxHoursContinuouslyList.Add(constr);
            //}

            foreach (var constr in db.ConstraintThreeActivitiesGrouped)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintThreeActivitiesGroupedId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }
            foreach (var constr in db.ConstraintTwoActivitiesGrouped)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintTwoActivitiesGroupedId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            //foreach (var constr in db.ConstraintStudentsSetMaxGapsPerDay)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxGapsPerDayList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeacherActivityTagMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherActivityTagMaxHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMaxHoursContinuously)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxHoursContinuouslyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersActivityTagMaxHoursContinuously)
            //{
            //    //Timetable.GetInstance().TimeConstraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersActivityTagMaxHoursDaily)
            //{
            //    //Timetable.GetInstance().TimeConstraints.ConstraintTeachersActivityTagMaxHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeacherMinDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeacherMinDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMaxDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeachersMaxDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsActivityTagMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsActivityTagMaxHoursDailyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMaxHoursContinuously)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxHoursContinuouslyList.Add(constr);
            //}

            foreach (var constr in db.ConstraintTwoActivitiesOrdered)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintTwoActivitiesOrderedId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            foreach (var constr in db.ConstraintActivitiesEndStudentsDay)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Add(constr);
            }

            foreach (var constr in db.ConstraintSubactivitiesPreferredStartingTimes)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList.Add(constr);
            }
            //add time
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList)
            {
                var times = from tb in db.TimeDayHour
                            where tb.ParentId == constr.ConstraintSubactivitiesPreferredStartingTimesId
                            select tb;
                foreach (var timeDayHour in times)
                {
                    constr.PreferredStartingTimes.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintSubactivitiesPreferredStartingTimesId));

                }
            }

            //foreach (var constr in db.ConstraintStudentsSetActivityTagMaxHoursDaily)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursDailyList.Add(constr);
            //}

            foreach (var constr in db.ConstraintSubactivitiesPreferredTimeSlots)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList.Add(constr);
            }
            //add time
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList)
            {
                var times = from tb in db.TimeDayHour
                            where tb.ParentId == constr.ConstraintSubactivitiesPreferredTimeSlotsId
                            select tb;
                foreach (var timeDayHour in times)
                {
                    constr.PreferredTimeSlots.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintSubactivitiesPreferredTimeSlotsId));

                }
            }

            foreach (var constr in db.ConstraintMinGapsBetweenActivities)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintMinGapsBetweenActivitiesId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            //foreach (var constr in db.ConstraintStudentsSetIntervalMaxDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetIntervalMaxDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMinDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintTeachersMinDaysPerWeekList.Add(constr);
            //}

            foreach (var constr in db.ConstraintActivitiesOccupyMaxTimeSlotsFromSelection)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }
            //add time
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList)
            {
                var times = from tb in db.TimeDayHour
                            where tb.ParentId == constr.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionId
                            select tb;
                foreach (var timeDayHour in times)
                {
                    constr.SelectedTimeSlots.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionId));

                }
            }

            foreach (var constr in db.ConstraintTeacherActivityTagMaxHoursContinuously)
            {
                //Timetable.GetInstance().TimeConstraints.ConstraintTeacherActivityTagMaxHoursContinuouslyList.Add(constr);
            }
            //foreach (var constr in db.ConstraintStudentsSetMaxDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxDaysPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetMaxHoursContinuously)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetMaxHoursContinuouslyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetActivityTagMaxHoursContinuously)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursContinuouslyList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMaxDaysPerWeek)
            //{
            //    Timetable.GetInstance().TimeConstraints.ConstraintStudentsMaxDaysPerWeekList.Add(constr);
            //}

            foreach (var constr in db.ConstraintMaxDaysBetweenActivities)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintMaxDaysBetweenActivitiesId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }
            #endregion

            #region space constraints

            //foreach (var constr in db.ConstraintBasicCompulsorySpace)
            //{
            //    if (!constr.GetType().IsSubclassOf(Type.GetType("FET.Data.ConstraintBasicCompulsorySpace")))
            //    {
            //        Timetable.GetInstance().SpaceConstraints.ConstraintBasicCompulsorySpaceList.Add(constr);
            //    }

            //}

            //foreach (var constr in db.ConstraintTeacherMinGapsBetweenBuildingChanges)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeacherMinGapsBetweenBuildingChangesList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeacherHomeRoom)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeacherHomeRoomList.Add(constr);
            //}
            foreach (var constr in db.ConstraintSubjectActivityTagPreferredRoom)
            {
                Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Add(constr);
            }

            //foreach (var constr in db.ConstraintRoomNotAvailableTimes)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintRoomNotAvailableTimesList.Add(constr);
            //}
            ////add time
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintRoomNotAvailableTimesList)
            //{
            //    var times = from tb in db.TimeDayHour
            //                where tb.ParentId == constr.ConstraintRoomNotAvailableTimesId
            //                select tb;
            //    foreach (var timeDayHour in times)
            //    {
            //        constr.NotAvailableTimes.Add(new TimeDayHour(timeDayHour.Day, timeDayHour.Hour, constr.ConstraintRoomNotAvailableTimesId));

            //    }
            //}

            //foreach (var constr in db.ConstraintTeacherHomeRooms)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeacherHomeRoomsList.Add(constr);
            //}
            //add rooms
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintTeacherHomeRoomsList)
            //{
            //    var rooms = from tb in db.RoomNameConstraintEntity
            //                where tb.ConstraintId == constr.ConstraintTeacherHomeRoomsId
            //                select tb;
            //    foreach (var roomName in rooms)
            //    {
            //        var rs = from r in Timetable.GetInstance().GetRoomsList()
            //                 where r.Name.Equals(roomName.RoomName)
            //                 select r;
            //        if (rs.Count() > 0)
            //        {
            //            constr.Rooms.Add(rs.First());
            //        }
            //    }
            //}

            //foreach (var constr in db.ConstraintActivityPreferredRoom)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintActivityPreferredRoomList.Add(constr);
            //}

            //foreach (var constr in db.ConstraintActivityPreferredRooms)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintActivityPreferredRoomsList.Add(constr);
            //}
            ////add rooms
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivityPreferredRoomsList)
            //{
            //    var rooms = from tb in db.RoomNameConstraintEntity
            //                where tb.ConstraintId == constr.ConstraintActivityPreferredRoomsId
            //                select tb;
            //    foreach (var roomName in rooms)
            //    {
            //        var rs = from r in Timetable.GetInstance().GetRoomsList()
            //                 where r.Name.Equals(roomName.RoomName)
            //                 select r;
            //        if (rs.Count() > 0)
            //        {
            //            constr.Rooms.Add(rs.First());
            //        }
            //    }
            //}

            //foreach (var constr in db.ConstraintSubjectPreferredRoom)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintSubjectPreferredRoomList.Add(constr);
            //}

            foreach (var constr in db.ConstraintSubjectActivityTagPreferredRooms)
            {
                Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Add(constr);
            }
            //add rooms
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList)
            {
                var rooms = from tb in db.RoomNameConstraintEntity
                            where tb.ConstraintId == constr.ConstraintSubjectActivityTagPreferredRoomsId
                            select tb;
                foreach (var roomName in rooms)
                {
                    var rs = from r in Timetable.GetInstance().GetRoomsList()
                             where r.Name.Equals(roomName.RoomName)
                             select r;
                    if (rs.Count() > 0)
                    {
                        constr.Rooms.Add(rs.First());
                    }
                }
            }

            //foreach (var constr in db.ConstraintTeacherMaxBuildingChangesPerDay)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeacherMaxBuildingChangesPerDayList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMaxBuildingChangesPerDay)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeachersMaxBuildingChangesPerDayList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMaxBuildingChangesPerDay)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsMaxBuildingChangesPerDayList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMinGapsBetweenBuildingChanges)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsMinGapsBetweenBuildingChangesList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsMaxBuildingChangesPerWeek)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsMaxBuildingChangesPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintActivityTagPreferredRoom)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintActivityTagPreferredRoomList.Add(constr);
            //}

            //foreach (var constr in db.ConstraintActivityTagPreferredRooms)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintActivityTagPreferredRoomsList.Add(constr);
            //}
            //add rooms
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivityTagPreferredRoomsList)
            //{
            //    var rooms = from tb in db.RoomNameConstraintEntity
            //                where tb.ConstraintId == constr.ConstraintActivityTagPreferredRoomsId
            //                select tb;
            //    foreach (var roomName in rooms)
            //    {
            //        var rs = from r in Timetable.GetInstance().GetRoomsList()
            //                 where r.Name.Equals(roomName.RoomName)
            //                 select r;
            //        if (rs.Count() > 0)
            //        {
            //            constr.Rooms.Add(rs.First());
            //        }
            //    }
            //}

            //foreach (var constr in db.ConstraintTeacherMaxBuildingChangesPerWeek)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeacherMaxBuildingChangesPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintTeachersMaxBuildingChangesPerWeek)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeachersMaxBuildingChangesPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetHomeRoom)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetHomeRoomList.Add(constr);
            //}


            //foreach (var constr in db.ConstraintSubjectPreferredRooms)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintSubjectPreferredRoomsList.Add(constr);
            //}
            //add rooms
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectPreferredRoomsList)
            //{
            //    var rooms = from tb in db.RoomNameConstraintEntity
            //                where tb.ConstraintId == constr.ConstraintSubjectPreferredRoomsId
            //                select tb;
            //    foreach (var roomName in rooms)
            //    {
            //        var rs = from r in Timetable.GetInstance().GetRoomsList()
            //                 where r.Name.Equals(roomName.RoomName)
            //                 select r;
            //        if (rs.Count() > 0)
            //        {
            //            constr.Rooms.Add(rs.First());
            //        }
            //    }
            //}

            foreach (var constr in db.ConstraintActivitiesOccupyMaxDifferentRooms)
            {
                Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesOccupyMaxDifferentRoomsId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }

            //foreach (var constr in db.ConstraintStudentsSetHomeRooms)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetHomeRoomsList.Add(constr);
            //}
            //add rooms
            //foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetHomeRoomsList)
            //{
            //    var rooms = from tb in db.RoomNameConstraintEntity
            //                where tb.ConstraintId == constr.ConstraintStudentsSetHomeRoomsId
            //                select tb;
            //    foreach (var roomName in rooms)
            //    {
            //        var rs = from r in Timetable.GetInstance().GetRoomsList()
            //                 where r.Name.Equals(roomName.RoomName)
            //                 select r;
            //        if (rs.Count() > 0)
            //        {
            //            constr.Rooms.Add(rs.First());
            //        }
            //    }
            //}

            //foreach (var constr in db.ConstraintTeachersMinGapsBetweenBuildingChanges)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintTeachersMinGapsBetweenBuildingChangesList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetMaxBuildingChangesPerDay)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetMaxBuildingChangesPerDayList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetMaxBuildingChangesPerWeek)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetMaxBuildingChangesPerWeekList.Add(constr);
            //}
            //foreach (var constr in db.ConstraintStudentsSetMinGapsBetweenBuildingChanges)
            //{
            //    Timetable.GetInstance().SpaceConstraints.ConstraintStudentsSetMinGapsBetweenBuildingChangesList.Add(constr);
            //}

            foreach (var constr in db.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive)
            {
                Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList.Add(constr);
            }
            //add ids
            foreach (var constr in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList)
            {
                var actIds = from v in db.ActivityIdContraintEntity
                             where v.ConstraintId == constr.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId
                             select v;
                foreach (var act in actIds)
                {
                    constr.ActivityIds.Add(act.ActivityId);
                }
            }
            #endregion

        }

        #endregion
    }

    #region types used for database storage
    public class HoursEntity
    {
        [Key]
        public int HourId { get; set; }
        public string Hour { get; set; }
        public int OrderNumber { get; set; }
        public HoursEntity()
        {
        }

        public HoursEntity(string hour, int order)
        {
            this.Hour = hour;
            this.OrderNumber = order;
            this.HourId = App.RandomTool.Next();
        }
    }

    public class DayEntity
    {
        [Key]
        public int DayId { get; set; }
        public string Day { get; set; }
        public int OrderNumber { get; set; }

        public DayEntity()
        {
        }
        public DayEntity(string day, int order)
        {
            this.Day = day;
            this.OrderNumber = order;
            this.DayId = App.RandomTool.Next();
        }
    }

    public class ActivityIdConstraintEntity
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int ConstraintId { get; set; }

        public ActivityIdConstraintEntity()
        {}
        public ActivityIdConstraintEntity(int activityId, int constraintId)
        {
            this.ActivityId = activityId;
            this.ConstraintId = constraintId;
        }
    }

    public class RoomNameConstraintEntity
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int ConstraintId { get; set; }

        public RoomNameConstraintEntity()
        { }
        public RoomNameConstraintEntity(string roomName, int constraintId)
        {
            this.RoomName = roomName;
            this.ConstraintId = constraintId;
        }
    }

    public class GeneralInfoEntity
    {
        [Key]
        public int GeneralInfoEntityId { get; set; }
        public string InstitutionName { get; set; }
        public string Comments { get; set; }
    }
    #endregion 
}
