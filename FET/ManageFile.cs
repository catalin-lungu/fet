using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using FET.Data;
using Microsoft.Win32;

namespace FET
{
    /// <summary>
    /// properties for interacting with generated timetable and
    /// methods for saving data to a file in xml format
    /// </summary>
    class ManageFile
    {

        /// <summary>
        /// save timetable data into a .fet file
        /// </summary>
        /// <param name="filePath"></param>
        public static void SaveChanges(string filePath="")
        {
            Timetable timetable = Timetable.GetInstance();

            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlNode fetNode = doc.CreateElement(string.Empty, "fet", string.Empty);
            doc.AppendChild(fetNode);

            XmlNode institutionNameNode = doc.CreateElement("Institution_Name");
            institutionNameNode.InnerText = timetable.InstitutionName;
            fetNode.AppendChild(institutionNameNode);

            XmlNode commentsNode = doc.CreateElement("Comments");
            commentsNode.InnerText = timetable.Comments;
            fetNode.AppendChild(commentsNode);

            XmlNode hoursListNode = doc.CreateElement("Hours_List");
            XmlNode nrHoursListNode = doc.CreateElement("Number");
            nrHoursListNode.InnerText = timetable.HoursList.Count().ToString();
            hoursListNode.AppendChild(nrHoursListNode);
            foreach (var h in timetable.HoursList)
            {
                XmlNode nameHourListNode = doc.CreateElement("Name");
                nameHourListNode.InnerText = h;
                hoursListNode.AppendChild(nameHourListNode);
            }
            fetNode.AppendChild(hoursListNode);

            XmlNode daysListNode = doc.CreateElement("Days_List");
            XmlNode nrDaysListNode = doc.CreateElement("Number");
            nrDaysListNode.InnerText = timetable.DaysList.Count().ToString();
            daysListNode.AppendChild(nrDaysListNode);
            foreach (var d in timetable.DaysList)
            {
                XmlNode nameDaysListNode = doc.CreateElement("Name");
                nameDaysListNode.InnerText = d;
                daysListNode.AppendChild(nameDaysListNode);
            }
            fetNode.AppendChild(daysListNode);

            XmlNode studentsListNode = doc.CreateElement("Students_List");
            foreach (var year in timetable.ClassList)
            {
                XmlNode yearNode = doc.CreateElement("Year");
                XmlNode nameYearNode = doc.CreateElement("Name");
                nameYearNode.InnerText = year.Name;
                yearNode.AppendChild(nameYearNode);

                XmlNode nrOfStudentsNode = doc.CreateElement("Number_of_Students");
                nrOfStudentsNode.InnerText = year.NumberOfStudents.ToString();
                yearNode.AppendChild(nrOfStudentsNode);

                foreach (var group in year.Groups)
                {
                    XmlNode groupNode = doc.CreateElement("Group");
                    XmlNode nameGroupNode = doc.CreateElement("Name");
                    nameGroupNode.InnerText = group.Name;
                    groupNode.AppendChild(nameGroupNode);

                    XmlNode nrOfStudentsGrNode = doc.CreateElement("Number_of_Students");
                    nrOfStudentsGrNode.InnerText = group.NumberOfStudents.ToString();
                    groupNode.AppendChild(nrOfStudentsGrNode);

                    foreach (var subGroup in group.Subgroups)
                    {
                        XmlNode subgroupNode = doc.CreateElement("Subgroup");

                        XmlNode nameSubgroupNode = doc.CreateElement("Name");
                        nameSubgroupNode.InnerText = subGroup.Name;
                        subgroupNode.AppendChild(nameSubgroupNode);

                        XmlNode nrOfStudentsSubgrNode = doc.CreateElement("Number_of_Students");
                        nrOfStudentsSubgrNode.InnerText = subGroup.NumberOfStudents.ToString();
                        subgroupNode.AppendChild(nrOfStudentsSubgrNode);

                        groupNode.AppendChild(subgroupNode);
                    }

                    yearNode.AppendChild(groupNode);
                }
                studentsListNode.AppendChild(yearNode);
            }
            fetNode.AppendChild(studentsListNode);

            XmlNode teacherListNode = doc.CreateElement("Teachers_List");
            foreach (var teacher in timetable.TeacherList)
            {
                XmlNode teacherNode = doc.CreateElement("Teacher");
                XmlNode teacherNameNode = doc.CreateElement("Name");
                teacherNameNode.InnerText = teacher.Name;
                teacherNode.AppendChild(teacherNameNode);

                teacherListNode.AppendChild(teacherNode);
            }
            fetNode.AppendChild(teacherListNode);

            XmlNode subjectsListNode = doc.CreateElement("Subjects_List");
            foreach (var subject in timetable.SubjectList)
            {
                XmlNode subjectNode = doc.CreateElement("Subject");
                XmlNode subjectNameNode = doc.CreateElement("Name");
                subjectNameNode.InnerText = subject.Name;
                subjectNode.AppendChild(subjectNameNode);

                subjectsListNode.AppendChild(subjectNode);
            }
            fetNode.AppendChild(subjectsListNode);

            XmlNode activityTagListNode = doc.CreateElement("Activity_Tags_List");
            foreach (var activityTag in timetable.ActivitiyTagsList)
            {
                XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
                XmlNode activityNameNode = doc.CreateElement("Name");
                activityNameNode.InnerText = activityTag.Name;
                activityTagNode.AppendChild(activityNameNode);

                activityTagListNode.AppendChild(activityTagNode);
            }
            fetNode.AppendChild(activityTagListNode);

            XmlNode activityListNode = doc.CreateElement("Activities_List");
            foreach (var activity in timetable.ActivityList)
            {
                XmlNode activityNode = doc.CreateElement("Activity");

                XmlNode teacherNode = doc.CreateElement("Teacher");
                teacherNode.InnerText = activity.Teacher.Name;
                activityNode.AppendChild(teacherNode);

                XmlNode subjectNode = doc.CreateElement("Subject");
                subjectNode.InnerText = activity.Subject.Name;
                activityNode.AppendChild(subjectNode);
                
                if (activity.ActivityTag != null)
                {
                    XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
                    activityTagNode.InnerText = activity.ActivityTag != null ? activity.ActivityTag.Name : string.Empty;
                    activityNode.AppendChild(activityTagNode);
                }

                XmlNode studentsNode = doc.CreateElement("Students");
                studentsNode.InnerText = activity.Students.Name;
                activityNode.AppendChild(studentsNode);

                XmlNode durationNode = doc.CreateElement("Duration");
                durationNode.InnerText = activity.Duration.ToString();
                activityNode.AppendChild(durationNode);

                XmlNode totalDurationNode = doc.CreateElement("Total_Duration");
                totalDurationNode.InnerText = activity.TotalDuration.ToString();
                activityNode.AppendChild(totalDurationNode);

                XmlNode idNode = doc.CreateElement("Id");
                idNode.InnerText = activity.ActivityId.ToString();
                activityNode.AppendChild(idNode);

                XmlNode activityGroupIdNode = doc.CreateElement("Activity_Group_Id");
                activityGroupIdNode.InnerText = activity.ActivityGroupId.ToString();
                activityNode.AppendChild(activityGroupIdNode);

                XmlNode activeNode = doc.CreateElement("Active");
                activeNode.InnerText = activity.Active.ToString();
                activityNode.AppendChild(activeNode);

                XmlNode commentsActivityNode = doc.CreateElement("Comments");
                commentsActivityNode.InnerText = activity.Comments;
                activityNode.AppendChild(commentsActivityNode);

                activityListNode.AppendChild(activityNode);
            }
            fetNode.AppendChild(activityListNode);

            XmlNode buildingsListNode = doc.CreateElement("Buildings_List");
            foreach (var building in timetable.BuildingsList)
            {
                XmlNode buildingNode = doc.CreateElement("Building");
                XmlNode buildingNameNode = doc.CreateElement("Name");
                buildingNameNode.InnerText = building.Name;
                buildingNode.AppendChild(buildingNameNode);

                buildingsListNode.AppendChild(buildingNode);
            }
            fetNode.AppendChild(buildingsListNode);

            XmlNode roomsListNode = doc.CreateElement("Rooms_List");
            foreach (var room in timetable.GetRoomsList())
            {
                XmlNode roomNode = doc.CreateElement("Room");

                XmlNode roomNameNode = doc.CreateElement("Name");
                roomNameNode.InnerText = room.Name;
                roomNode.AppendChild(roomNameNode);

                XmlNode roomBuildingNode = doc.CreateElement("Building");
                bool foundBuilding = false;
                foreach (var b in timetable.BuildingsList)
                {
                    if (foundBuilding)
                        break;
                    foreach (var r in b.Rooms)
                    {
                        if (r.Name.Equals(room.Name))
                        {
                            roomBuildingNode.InnerText = b.Name;
                            foundBuilding = true;
                            break;
                        }
                    }
                }
                roomNode.AppendChild(roomBuildingNode);

                XmlNode capacityNode = doc.CreateElement("Capacity");
                capacityNode.InnerText = room.Capacity.ToString();
                roomNode.AppendChild(capacityNode);

                roomsListNode.AppendChild(roomNode);
            }
            fetNode.AppendChild(roomsListNode);


            //AddTimeConstraints(fetNode, doc, timetable);
            //AddSpaceConstraints(fetNode, doc, timetable);

            if (filePath.Equals(""))
            {
                if (string.IsNullOrEmpty(App.DataPathFile))
                {
                    SaveFileDialog fileDialog = new SaveFileDialog();
                    fileDialog.DefaultExt = ".fet";
                    fileDialog.Filter = "FET documents (.fet)|*.fet";

                    if (fileDialog.ShowDialog().Value)
                    {      
                        App.DataPathFile = fileDialog.FileName;
                    }
                }
                doc.Save(App.DataPathFile);
            }
            else
            {
                doc.Save(filePath);
            }

            App.DataHasChanged = false;
        }
 
        /// <summary>
        /// add time constraints to a xml doc from timetable
        /// </summary>
        /// <param name="fetNode"></param>
        /// <param name="doc"></param>
        /// <param name="timetable"></param>
//        private static void AddTimeConstraints(XmlNode fetNode, XmlDocument doc , Timetable timetable)
//        {

//            //time
//            XmlNode timeNode = doc.CreateElement("Time_Constraints_List");

//#region ConstraintBasicCompulsoryTime
//            foreach (var constr in timetable.TimeConstraints.ConstraintBasicCompulsoryTimeList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintBasicCompulsoryTime");
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }
//#endregion
//#region ConstraintBreakTimes
//            foreach (var constr in timetable.TimeConstraints.ConstraintBreakTimesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintBreakTimes");
                
//                XmlNode NumberofBreakTimesConstraintBreakTimesNode = doc.CreateElement("Number_of_Break_Times");
//                NumberofBreakTimesConstraintBreakTimesNode.InnerText = constr.BreakTimes.Count().ToString();
//                constrNode.AppendChild(NumberofBreakTimesConstraintBreakTimesNode);

//                foreach (var constrBreakTime in constr.BreakTimes)
//                {
//                    XmlNode BreakTimeNode = doc.CreateElement("Break_Time");

//                    XmlNode DayConstraintBreakTimesNode = doc.CreateElement("Day");
//                    DayConstraintBreakTimesNode.InnerText = constrBreakTime.Day;
//                    BreakTimeNode.AppendChild(DayConstraintBreakTimesNode);

//                    XmlNode HourConstraintBreakTimesNode = doc.CreateElement("Hour");
//                    HourConstraintBreakTimesNode.InnerText = constrBreakTime.Hour;
//                    BreakTimeNode.AppendChild(HourConstraintBreakTimesNode);

//                    constrNode.AppendChild(BreakTimeNode);
//                }
//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);
//            }
//#endregion
//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherNotAvailableTimesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherNotAvailableTimes");

//                XmlNode TeacherConstraintTeacherNotAvailableTimesNode = doc.CreateElement("Teacher");
//                TeacherConstraintTeacherNotAvailableTimesNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherConstraintTeacherNotAvailableTimesNode);

//                XmlNode NumberofNotAvailableTimesConstraintTeacherNotAvailableTimesNode = doc.CreateElement("Number_of_Not_Available_Times");
//                NumberofNotAvailableTimesConstraintTeacherNotAvailableTimesNode.InnerText = constr.NotAvailableTimes.Count().ToString();
//                constrNode.AppendChild(NumberofNotAvailableTimesConstraintTeacherNotAvailableTimesNode);

//                foreach (var notAvailable in constr.NotAvailableTimes)
//                {
//                    XmlNode NotAvailableTimeNode = doc.CreateElement("Not_Available_Time");

//                    XmlNode DayConstraintNode = doc.CreateElement("Day");
//                    DayConstraintNode.InnerText = notAvailable.Day;
//                    NotAvailableTimeNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Hour");
//                    HourConstraintNode.InnerText = notAvailable.Hour;
//                    NotAvailableTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(NotAvailableTimeNode);

//                }
//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMaxDaysPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxDaysPerWeek");

//                XmlNode TeacherNameConstraintTeacherMaxDaysPerWeekNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintTeacherMaxDaysPerWeekNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintTeacherMaxDaysPerWeekNode);

//                XmlNode MaxDaysPerWeekConstraintTeacherMaxDaysPerWeekNode = doc.CreateElement("Max_Days_Per_Week");
//                MaxDaysPerWeekConstraintTeacherMaxDaysPerWeekNode.InnerText = constr.MaxDaysPerWeek.ToString();
//                constrNode.AppendChild(MaxDaysPerWeekConstraintTeacherMaxDaysPerWeekNode);

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMaxGapsPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxGapsPerWeek");
                               
//                XmlNode TeacherNameConstraintTeacherMaxGapsPerWeekNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintTeacherMaxGapsPerWeekNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintTeacherMaxGapsPerWeekNode);

//                XmlNode MaxGapsConstraintTeacherMaxGapsPerWeekNode = doc.CreateElement("Max_Gaps");
//                MaxGapsConstraintTeacherMaxGapsPerWeekNode.InnerText = constr.MaxGaps.ToString();
//                constrNode.AppendChild(MaxGapsConstraintTeacherMaxGapsPerWeekNode);
                
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

//            //foreach (var constr in timetable.TimeConstraints.ConstraintTeachersMaxGapsPerWeekList)
//            //{
//            //    XmlNode constrNode = doc.CreateElement("ConstraintTeachersMaxGapsPerWeek");
                                
//            //    XmlNode maxGapsNode = doc.CreateElement("Max_Gaps");
//            //    maxGapsNode.InnerText = constr.MaxGaps.ToString();
//            //    constrNode.AppendChild(maxGapsNode);

//            //    AddCommonTagsTime(constr, doc, constrNode);
//            //    timeNode.AppendChild(constrNode);
//            //}

//            XmlNode constrNodeTeachersMaxGapsPerWeek = doc.CreateElement("ConstraintTeachersMaxGapsPerWeek");

//            XmlNode maxGapsNode1 = doc.CreateElement("Max_Gaps");
//            maxGapsNode1.InnerText = Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerWeek.ToString();
//            constrNodeTeachersMaxGapsPerWeek.AppendChild(maxGapsNode1);

//            AddCommonTagsTime(null, doc, constrNodeTeachersMaxGapsPerWeek);
//            timeNode.AppendChild(constrNodeTeachersMaxGapsPerWeek);



//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMaxHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxHoursDaily");

//                XmlNode TeacherNameConstraintTeacherMaxHoursDailyNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintTeacherMaxHoursDailyNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintTeacherMaxHoursDailyNode);

//                XmlNode MaximumHoursDailyConstraintTeacherMaxHoursDailyNode = doc.CreateElement("Maximum_Hours_Daily");
//                MaximumHoursDailyConstraintTeacherMaxHoursDailyNode.InnerText = constr.MaximumHoursDaily.ToString();
//                constrNode.AppendChild(MaximumHoursDailyConstraintTeacherMaxHoursDailyNode);
                
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMinHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMinHoursDaily");
                                
//                XmlNode TeacherNameConstraintTeacherMinHoursDailyNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintTeacherMinHoursDailyNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintTeacherMinHoursDailyNode);

//                XmlNode MinimumHoursDailyConstraintTeacherMinHoursDailyNode = doc.CreateElement("Minimum_Hours_Daily");
//                MinimumHoursDailyConstraintTeacherMinHoursDailyNode.InnerText = constr.MinimumHoursDaily.ToString();
//                constrNode.AppendChild(MinimumHoursDailyConstraintTeacherMinHoursDailyNode);

//                XmlNode AllowEmptyDaysConstraintTeacherMinHoursDailyNode = doc.CreateElement("Allow_Empty_Days");
//                AllowEmptyDaysConstraintTeacherMinHoursDailyNode.InnerText = constr.AllowEmptyDays.ToString();
//                constrNode.AppendChild(AllowEmptyDaysConstraintTeacherMinHoursDailyNode);
                
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherIntervalMaxDaysPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherIntervalMaxDaysPerWeek");

//                XmlNode TeacherNameConstraintTeacherIntervalMaxDaysPerWeekNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintTeacherIntervalMaxDaysPerWeekNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintTeacherIntervalMaxDaysPerWeekNode);

//                XmlNode IntervalStartHourConstraintTeacherIntervalMaxDaysPerWeekNode = doc.CreateElement("Interval_Start_Hour");
//                IntervalStartHourConstraintTeacherIntervalMaxDaysPerWeekNode.InnerText = constr.IntervalStartHour.ToString();
//                constrNode.AppendChild(IntervalStartHourConstraintTeacherIntervalMaxDaysPerWeekNode);

//                XmlNode IntervalEndHourConstraintTeacherIntervalMaxDaysPerWeekNode = doc.CreateElement("Interval_End_Hour");
//                IntervalEndHourConstraintTeacherIntervalMaxDaysPerWeekNode.InnerText = constr.IntervalEndHour.ToString();
//                constrNode.AppendChild(IntervalEndHourConstraintTeacherIntervalMaxDaysPerWeekNode);

//                XmlNode MaxDaysPerWeekConstraintTeacherIntervalMaxDaysPerWeekNode = doc.CreateElement("Max_Days_Per_Week");
//                MaxDaysPerWeekConstraintTeacherIntervalMaxDaysPerWeekNode.InnerText = constr.MaxDaysPerWeek.ToString();
//                constrNode.AppendChild(MaxDaysPerWeekConstraintTeacherIntervalMaxDaysPerWeekNode);
                
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivityPreferredStartingTimesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivityPreferredStartingTimes");

//                XmlNode ActivityIdConstraintActivityPreferredStartingTimesNode = doc.CreateElement("Activity_Id");
//                ActivityIdConstraintActivityPreferredStartingTimesNode.InnerText = constr.ActivityId.ToString();
//                constrNode.AppendChild(ActivityIdConstraintActivityPreferredStartingTimesNode);

//                XmlNode NumberofPreferredStartingTimesConstraintActivityPreferredStartingTimesNode = doc.CreateElement("Number_of_Preferred_Starting_Times");
//                NumberofPreferredStartingTimesConstraintActivityPreferredStartingTimesNode.InnerText = constr.PreferredStartingTimes.Count().ToString();
//                constrNode.AppendChild(NumberofPreferredStartingTimesConstraintActivityPreferredStartingTimesNode);

//                foreach (var prefTime in constr.PreferredStartingTimes)
//                {
//                    XmlNode prefTimeNode = doc.CreateElement("Preferred_Starting_Time");

//                    XmlNode DayConstraintNode = doc.CreateElement("Preferred_Starting_Day");
//                    DayConstraintNode.InnerText = prefTime.Day;
//                    prefTimeNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Starting_Hour");
//                    HourConstraintNode.InnerText = prefTime.Hour;
//                    prefTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(prefTimeNode);

//                }
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesSameStartingDayList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesSameStartingDay");

//                XmlNode NumberofActivitiesConstraintActivitiesSameStartingDayNode = doc.CreateElement("Number_of_Activities");
//                NumberofActivitiesConstraintActivitiesSameStartingDayNode.InnerText = constr.ActivityIds.Count().ToString();
//                constrNode.AppendChild(NumberofActivitiesConstraintActivitiesSameStartingDayNode);

//                foreach (var act in constr.ActivityIds)
//                {
//                    XmlNode ActivityId1ConstraintActivitiesSameStartingDayNode = doc.CreateElement("Activity_Id");
//                    ActivityId1ConstraintActivitiesSameStartingDayNode.InnerText = act.ToString();
//                    constrNode.AppendChild(ActivityId1ConstraintActivitiesSameStartingDayNode);
//                }
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMaxGapsPerDayList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxGapsPerDay");

//                XmlNode TeacherNameConstraintTeacherMaxGapsPerDayNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintTeacherMaxGapsPerDayNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintTeacherMaxGapsPerDayNode);

//                XmlNode MaxGapsConstraintTeacherMaxGapsPerDayNode = doc.CreateElement("Max_Gaps");
//                MaxGapsConstraintTeacherMaxGapsPerDayNode.InnerText = constr.MaxGaps.ToString();
//                constrNode.AppendChild(MaxGapsConstraintTeacherMaxGapsPerDayNode);

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesPreferredTimeSlots");

//                XmlNode TeacherNameConstraintActivitiesPreferredTimeSlotsNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintActivitiesPreferredTimeSlotsNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintActivitiesPreferredTimeSlotsNode);

//                XmlNode StudentsNameConstraintActivitiesPreferredTimeSlotsNode = doc.CreateElement("Students_Name");
//                StudentsNameConstraintActivitiesPreferredTimeSlotsNode.InnerText = constr.StudentsName;
//                constrNode.AppendChild(StudentsNameConstraintActivitiesPreferredTimeSlotsNode);

//                XmlNode SubjectNameConstraintActivitiesPreferredTimeSlotsNode = doc.CreateElement("Subject_Name");
//                SubjectNameConstraintActivitiesPreferredTimeSlotsNode.InnerText = constr.SubjectName;
//                constrNode.AppendChild(SubjectNameConstraintActivitiesPreferredTimeSlotsNode);

//                XmlNode ActivityTagNameConstraintActivitiesPreferredTimeSlotsNode = doc.CreateElement("Activity_Tag_Name");
//                ActivityTagNameConstraintActivitiesPreferredTimeSlotsNode.InnerText = constr.ActivityTagName;
//                constrNode.AppendChild(ActivityTagNameConstraintActivitiesPreferredTimeSlotsNode);

//                XmlNode NumberofPreferredTimeSlotsConstraintActivitiesPreferredTimeSlotsNode = doc.CreateElement("Number_of_Preferred_Time_Slots");
//                NumberofPreferredTimeSlotsConstraintActivitiesPreferredTimeSlotsNode.InnerText = constr.PreferredTimeSlots.Count().ToString();
//                constrNode.AppendChild(NumberofPreferredTimeSlotsConstraintActivitiesPreferredTimeSlotsNode);

//                foreach (var prefTime in constr.PreferredTimeSlots)
//                {
//                    XmlNode prefTimeNode = doc.CreateElement("Preferred_Time_Slot");

//                    XmlNode DayConstraintNode = doc.CreateElement("Preferred_Day");
//                    DayConstraintNode.InnerText = prefTime.Day;
//                    prefTimeNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Hour");
//                    HourConstraintNode.InnerText = prefTime.Hour;
//                    prefTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(prefTimeNode);
//                }
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }


//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesPreferredStartingTimesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesPreferredStartingTimes");

//                XmlNode TeacherNameConstraintActivitiesPreferredStartingTimesNode = doc.CreateElement("Teacher_Name");
//                TeacherNameConstraintActivitiesPreferredStartingTimesNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(TeacherNameConstraintActivitiesPreferredStartingTimesNode);

//                XmlNode StudentsNameConstraintActivitiesPreferredStartingTimesNode = doc.CreateElement("Students_Name");
//                StudentsNameConstraintActivitiesPreferredStartingTimesNode.InnerText = constr.StudentsName;
//                constrNode.AppendChild(StudentsNameConstraintActivitiesPreferredStartingTimesNode);

//                XmlNode SubjectNameConstraintActivitiesPreferredStartingTimesNode = doc.CreateElement("Subject_Name");
//                SubjectNameConstraintActivitiesPreferredStartingTimesNode.InnerText = constr.SubjectName;
//                constrNode.AppendChild(SubjectNameConstraintActivitiesPreferredStartingTimesNode);

//                XmlNode ActivityTagNameConstraintActivitiesPreferredStartingTimesNode = doc.CreateElement("Activity_Tag_Name");
//                ActivityTagNameConstraintActivitiesPreferredStartingTimesNode.InnerText = constr.ActivityTagName;
//                constrNode.AppendChild(ActivityTagNameConstraintActivitiesPreferredStartingTimesNode);

//                XmlNode NumberofPreferredStartingTimesConstraintActivitiesPreferredStartingTimesNode = doc.CreateElement("Number_of_Preferred_Starting_Times");
//                NumberofPreferredStartingTimesConstraintActivitiesPreferredStartingTimesNode.InnerText = constr.PreferredStartingTime.Count().ToString();
//                constrNode.AppendChild(NumberofPreferredStartingTimesConstraintActivitiesPreferredStartingTimesNode);

//                foreach (var prefStartTime in constr.PreferredStartingTime)
//                {
//                    XmlNode prefStartTimeNode = doc.CreateElement("Preferred_Starting_Time");

//                    XmlNode DayConstraintNode = doc.CreateElement("Preferred_Starting_Day");
//                    DayConstraintNode.InnerText = prefStartTime.Day;
//                    prefStartTimeNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Starting_Hour");
//                    HourConstraintNode.InnerText = prefStartTime.Hour;
//                    prefStartTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(prefStartTimeNode);
//                }
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }


//            foreach (var constr in timetable.TimeConstraints.ConstraintTwoActivitiesConsecutiveList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTwoActivitiesConsecutive");

//                XmlNode FirstActivityIdConstraintTwoActivitiesConsecutiveNode = doc.CreateElement("First_Activity_Id");
//                FirstActivityIdConstraintTwoActivitiesConsecutiveNode.InnerText = constr.FirstActivityId.ToString();
//                constrNode.AppendChild(FirstActivityIdConstraintTwoActivitiesConsecutiveNode);

//                XmlNode SecondActivityIdConstraintTwoActivitiesConsecutiveNode = doc.CreateElement("Second_Activity_Id");
//                SecondActivityIdConstraintTwoActivitiesConsecutiveNode.InnerText = constr.FirstActivityId.ToString();
//                constrNode.AppendChild(SecondActivityIdConstraintTwoActivitiesConsecutiveNode);

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);

//            }

            
//                XmlNode constrNode15 = doc.CreateElement("ConstraintStudentsMaxHoursDaily");

//                XmlNode MaximumHoursDailyConstraintStudentsMaxHoursDailyNode = doc.CreateElement("Maximum_Hours_Daily");
//                MaximumHoursDailyConstraintStudentsMaxHoursDailyNode.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursDaily.ToString();
//                constrNode15.AppendChild(MaximumHoursDailyConstraintStudentsMaxHoursDailyNode);

//                AddCommonTagsTime(null, doc, constrNode15);
//                timeNode.AppendChild(constrNode15);
            

//            #region ConstraintStudentsSetMaxGapsPerWeek
//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetMaxGapsPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxGapsPerWeek");
                
//                XmlNode MaxGapsConstraintStudentsSetMaxGapsPerWeekNode = doc.CreateElement("Max_Gaps");
//                MaxGapsConstraintStudentsSetMaxGapsPerWeekNode.InnerText = constr.MaxGaps.ToString();
//                constrNode.AppendChild(MaxGapsConstraintStudentsSetMaxGapsPerWeekNode);

//                XmlNode StudentsConstraintStudentsSetMaxGapsPerWeekNode = doc.CreateElement("Students");
//                StudentsConstraintStudentsSetMaxGapsPerWeekNode.InnerText = constr.Students.ToString();
//                constrNode.AppendChild(StudentsConstraintStudentsSetMaxGapsPerWeekNode);

//                AddCommonTagsTime(constr, doc, constrNode);
                
//                timeNode.AppendChild(constrNode);
//            }
//            #endregion

//            foreach (var constr in timetable.TimeConstraints.ConstraintMinDaysBetweenActivitiesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintMinDaysBetweenActivities");

//                XmlNode ConsecutiveIfSameDayNode = doc.CreateElement("Consecutive_If_Same_Day");
//                ConsecutiveIfSameDayNode.InnerText = constr.ConsecutiveIfSameDay.ToString();
//                constrNode.AppendChild(ConsecutiveIfSameDayNode);

//                XmlNode NumberOfActivitiesNode = doc.CreateElement("Number_of_Activities");
//                NumberOfActivitiesNode.InnerText = constr.NumberOfActivities.ToString();
//                constrNode.AppendChild(NumberOfActivitiesNode);

//                XmlNode MinDaysNode = doc.CreateElement("MinDays");
//                MinDaysNode.InnerText = constr.MinDays.ToString();
//                constrNode.AppendChild(MinDaysNode);

//                foreach (var act in constr.ActivityIds)
//                {
//                    XmlNode ActivityIdNode = doc.CreateElement("Activity_Id");
//                    ActivityIdNode.InnerText = act.ToString();
//                    constrNode.AppendChild(ActivityIdNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);
//            }

            
//                XmlNode constrNode16 = doc.CreateElement("ConstraintStudentsEarlyMaxBeginningsAtSecondHour");

//                XmlNode MaxBeginningsAtSecondHourNode = doc.CreateElement("Max_Beginnings_At_Second_Hour");
//                MaxBeginningsAtSecondHourNode.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsEarlyMaxBeginningsAtSecondHour.ToString();
//                constrNode16.AppendChild(MaxBeginningsAtSecondHourNode);

//                AddCommonTagsTime(null, doc, constrNode16);

//                timeNode.AppendChild(constrNode16);
            

            
//                XmlNode constrNode17 = doc.CreateElement("ConstraintStudentsMaxGapsPerWeek");

//                XmlNode maxGapsNode4 = doc.CreateElement("Max_Gaps");
//                maxGapsNode4.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerWeek.ToString();
//                constrNode17.AppendChild(maxGapsNode4);

//                AddCommonTagsTime(null, doc, constrNode17);

//                timeNode.AppendChild(constrNode17);
            

            
//                XmlNode constrNode18 = doc.CreateElement("ConstraintStudentsMinHoursDaily");

//                XmlNode minHourDailyNode = doc.CreateElement("Minimum_Hours_Daily");
//                minHourDailyNode.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsMinHoursDaily.ToString();
//                constrNode18.AppendChild(minHourDailyNode);

//                //not suported in new version
//                XmlNode allowEmptyDaysNode = doc.CreateElement("Allow_Empty_Days");
//                allowEmptyDaysNode.InnerText = true.ToString();
//                constrNode18.AppendChild(allowEmptyDaysNode);

//                AddCommonTagsTime(null, doc, constrNode18);

//                timeNode.AppendChild(constrNode18);

            

//            //foreach (var constr in timetable.TimeConstraints.ConstraintTeachersMaxGapsPerDayList)
//            //{
//            //    XmlNode constrNode = doc.CreateElement("ConstraintTeachersMaxGapsPerDay");

//            //    XmlNode maxGapsNode = doc.CreateElement("Max_Gaps");
//            //    maxGapsNode.InnerText = constr.MaxGaps.ToString();
//            //    constrNode.AppendChild(maxGapsNode);

//            //    AddCommonTagsTime(constr, doc, constrNode);

//            //    timeNode.AppendChild(constrNode);
//            //}

//            XmlNode constrNode1 = doc.CreateElement("ConstraintTeachersMaxGapsPerDay");

//            XmlNode maxGapsNode2 = doc.CreateElement("Max_Gaps");
//            maxGapsNode2.InnerText = Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMaxGapsPerDay.ToString();
//            constrNode1.AppendChild(maxGapsNode2);

//            AddCommonTagsTime(null, doc, constrNode1);

//            timeNode.AppendChild(constrNode1);

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesNotOverlappingList)
//            {
//                XmlNode ConstrNode = doc.CreateElement("ConstraintActivitiesNotOverlapping");

//                XmlNode NumberOfActivitiesNode = doc.CreateElement("Number_of_Activities");
//                NumberOfActivitiesNode.InnerText = constr.NumberOfActivities.ToString();
//                ConstrNode.AppendChild(NumberOfActivitiesNode);


//                foreach (var act in constr.ActivityIds)
//                {
//                    XmlNode ActivityIdNode = doc.CreateElement("Activity_Id");
//                    ActivityIdNode.InnerText = act.ToString();
//                    ConstrNode.AppendChild(ActivityIdNode);
//                }

//                AddCommonTagsTime(constr, doc, ConstrNode);

//                timeNode.AppendChild(ConstrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetMaxHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxHoursDaily");

//                XmlNode maxHourDailyNode = doc.CreateElement("Maximum_Hours_Daily");
//                maxHourDailyNode.InnerText = constr.MaximumHoursDaily.ToString();
//                constrNode.AppendChild(maxHourDailyNode);

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetMinHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMinHoursDaily");

//                XmlNode minHourDailyNodex = doc.CreateElement("Minimum_Hours_Daily");
//                minHourDailyNodex.InnerText = constr.MinimumHoursDaily.ToString();
//                constrNode.AppendChild(minHourDailyNodex);

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                XmlNode allowNode = doc.CreateElement("Allow_Empty_Days");
//                allowNode.InnerText = constr.AllowEmptyDays.ToString();
//                constrNode.AppendChild(allowNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetNotAvailableTimesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetNotAvailableTimes");

//                XmlNode StudentsNameNode = doc.CreateElement("Students");
//                StudentsNameNode.InnerText = constr.Students;
//                constrNode.AppendChild(StudentsNameNode);

//                XmlNode NumberofPreferredNode = doc.CreateElement("Number_of_Not_Available_Times");
//                NumberofPreferredNode.InnerText = constr.NotAvailableTimes.Count().ToString();
//                constrNode.AppendChild(NumberofPreferredNode);

//                foreach (var prefStartTime in constr.NotAvailableTimes)
//                {
//                    XmlNode notAvailableNode = doc.CreateElement("Not_Available_Time");

//                    XmlNode DayConstraintNode = doc.CreateElement("Day");
//                    DayConstraintNode.InnerText = prefStartTime.Day;
//                    notAvailableNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Hour");
//                    HourConstraintNode.InnerText = prefStartTime.Hour;
//                    notAvailableNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(notAvailableNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetEarlyMaxBeginningsAtSecondHourList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetEarlyMaxBeginningsAtSecondHour");

//                XmlNode minHourDailyNode5 = doc.CreateElement("Max_Beginnings_At_Second_Hour");
//                minHourDailyNode5.InnerText = constr.MaxBeginningsAtSecondHour.ToString();
//                constrNode.AppendChild(minHourDailyNode5);

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);
            
//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }


//            foreach (var constr in timetable.TimeConstraints.ConstraintActivityEndsStudentsDayList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivityEndsStudentsDay");

//                XmlNode minHourDailyNode6 = doc.CreateElement("Activity_Id");
//                minHourDailyNode6.InnerText = constr.ActivityId.ToString();
//                constrNode.AppendChild(minHourDailyNode6);
                
//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivityPreferredStartingTimeList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivityPreferredStartingTime");

//                XmlNode activityIdNode = doc.CreateElement("Activity_Id");
//                activityIdNode.InnerText = constr.ActivityId.ToString();
//                constrNode.AppendChild(activityIdNode);

//                XmlNode prefDayNode = doc.CreateElement("Preferred_Day");
//                prefDayNode.InnerText = constr.PreferredDay;
//                constrNode.AppendChild(prefDayNode);

//                XmlNode prefHourNode = doc.CreateElement("Preferred_Hour");
//                prefHourNode.InnerText = constr.PreferredDay;
//                constrNode.AppendChild(prefHourNode);

//                XmlNode permanentLokedNode = doc.CreateElement("Permanently_Locked");
//                permanentLokedNode.InnerText = constr.PermanentlyLocked.ToString();
//                constrNode.AppendChild(permanentLokedNode);


//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            //foreach (var constr in timetable.TimeConstraints.ConstraintTeachersMaxHoursDailyList)
//            //{
//            //    XmlNode constrNode = doc.CreateElement("ConstraintTeachersMaxHoursDaily");

//            //    XmlNode maxHourDailyNode = doc.CreateElement("Maximum_Hours_Daily");
//            //    maxHourDailyNode.InnerText = constr.MaximumHoursDaily.ToString();
//            //    constrNode.AppendChild(maxHourDailyNode);

//            //    AddCommonTagsTime(constr, doc, constrNode);

//            //    timeNode.AppendChild(constrNode);

//            //}

//            XmlNode constrNode3 = doc.CreateElement("ConstraintTeachersMaxHoursDaily");

//            XmlNode maxHourDailyNode1 = doc.CreateElement("Maximum_Hours_Daily");
//            maxHourDailyNode1.InnerText = Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMaxHoursDaily.ToString();
//            constrNode3.AppendChild(maxHourDailyNode1);

//            AddCommonTagsTime(null, doc, constrNode3);

//            timeNode.AppendChild(constrNode3);


//            //foreach (var constr in timetable.TimeConstraints.ConstraintTeachersMinHoursDailyList)
//            //{
//            //    XmlNode constrNode = doc.CreateElement("ConstraintTeachersMinHoursDaily");

//            //    XmlNode minHourDailyNode = doc.CreateElement("Minimum_Hours_Daily");
//            //    minHourDailyNode.InnerText = constr.MinimumHoursDaily.ToString();
//            //    constrNode.AppendChild(minHourDailyNode);

//            //    XmlNode allowNode = doc.CreateElement("Allow_Empty_Days");
//            //    allowNode.InnerText = constr.AllowEmptyDays.ToString();
//            //    constrNode.AppendChild(allowNode);

//            //    AddCommonTagsTime(constr, doc, constrNode);

//            //    timeNode.AppendChild(constrNode);

//            //}

//            XmlNode constrNode4 = doc.CreateElement("ConstraintTeachersMinHoursDaily");

//            XmlNode minHourDailyNode1 = doc.CreateElement("Minimum_Hours_Daily");
//            minHourDailyNode1.InnerText = Timetable.GetInstance().TimeConstraints.ConstraintsTeachers.TeachersMinHoursDaily.ToString();
//            constrNode4.AppendChild(minHourDailyNode1);

//            XmlNode allowNode1 = doc.CreateElement("Allow_Empty_Days");
//            allowNode1.InnerText = true.ToString();
//            constrNode4.AppendChild(allowNode1);

//            AddCommonTagsTime(null, doc, constrNode4);

//            timeNode.AppendChild(constrNode4);

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivityPreferredTimeSlotsList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivityPreferredTimeSlots");

//                XmlNode activityIdNode = doc.CreateElement("Activity_Id");
//                activityIdNode.InnerText = constr.ActivityId.ToString();
//                constrNode.AppendChild(activityIdNode);

//                XmlNode nrPreferredNode = doc.CreateElement("Number_of_Preferred_Time_Slots");
//                nrPreferredNode.InnerText = constr.PreferredTimeSlots.Count().ToString();
//                constrNode.AppendChild(nrPreferredNode);

//                foreach (var prefStartTime in constr.PreferredTimeSlots)
//                {
//                    XmlNode prefTimeNode = doc.CreateElement("Preferred_Time_Slot");

//                    XmlNode DayConstraintNode = doc.CreateElement("Preferred_Day");
//                    DayConstraintNode.InnerText = prefStartTime.Day;
//                    prefTimeNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Hour");
//                    HourConstraintNode.InnerText = prefStartTime.Hour;
//                    prefTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(prefTimeNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

            
//                XmlNode constrNode21 = doc.CreateElement("ConstraintStudentsMaxGapsPerDay");

//                XmlNode maxGapsNode = doc.CreateElement("Max_Gaps");
//                maxGapsNode.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsMaxGapsPerDay.ToString();
//                constrNode21.AppendChild(maxGapsNode);

//                AddCommonTagsTime(null, doc, constrNode21);

//                timeNode.AppendChild(constrNode21);

            

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesSameStartingHourList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesSameStartingHour");

//                XmlNode nrOfActivitiesNode = doc.CreateElement("Number_of_Activities");
//                nrOfActivitiesNode.InnerText = constr.ActivityIds.Count().ToString();
//                constrNode.AppendChild(nrOfActivitiesNode);
                
//                foreach (var id in constr.ActivityIds)
//                {
//                    XmlNode idConstraintNode = doc.CreateElement("Activity_Id");
//                    idConstraintNode.InnerText = id.ToString();
//                    constrNode.AppendChild(idConstraintNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesSameStartingTimeList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesSameStartingTime");

//                XmlNode nrOfActivitiesNode = doc.CreateElement("Number_of_Activities");
//                nrOfActivitiesNode.InnerText = constr.ActivityIds.Count().ToString();
//                constrNode.AppendChild(nrOfActivitiesNode);

//                foreach (var id in constr.ActivityIds)
//                {
//                    XmlNode idConstraintNode = doc.CreateElement("Activity_Id");
//                    idConstraintNode.InnerText = id.ToString();
//                    constrNode.AppendChild(idConstraintNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsIntervalMaxDaysPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsIntervalMaxDaysPerWeek");

//                XmlNode intervalStartNode = doc.CreateElement("Interval_Start_Hour");
//                intervalStartNode.InnerText = constr.IntervalStartHour;
//                constrNode.AppendChild(intervalStartNode);

//                XmlNode intervalEndNode = doc.CreateElement("Interval_End_Hour");
//                intervalEndNode.InnerText = constr.IntervalEndHour;
//                constrNode.AppendChild(intervalEndNode);

//                XmlNode maxDaysNode = doc.CreateElement("Max_Days_Per_Week");
//                maxDaysNode.InnerText = constr.MaxDaysPerWeek.ToString();
//                constrNode.AppendChild(maxDaysNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

            
//                XmlNode constrNode22 = doc.CreateElement("ConstraintTeachersIntervalMaxDaysPerWeek");

//                XmlNode intervalStartNode1 = doc.CreateElement("Interval_Start_Hour");
//                intervalStartNode1.InnerText = timetable.TimeConstraints.ConstraintsTeachers.TeachersIntervalMaxDaysPerWeek.IntervalStartHour;
//                constrNode22.AppendChild(intervalStartNode1);

//                XmlNode intervalEndNode1 = doc.CreateElement("Interval_End_Hour");
//                intervalEndNode1.InnerText = timetable.TimeConstraints.ConstraintsTeachers.TeachersIntervalMaxDaysPerWeek.IntervalEndHour;
//                constrNode22.AppendChild(intervalEndNode1);

//                XmlNode maxDaysNode1 = doc.CreateElement("Max_Days_Per_Week");
//                maxDaysNode1.InnerText = timetable.TimeConstraints.ConstraintsTeachers.TeachersIntervalMaxDaysPerWeek.MaxDaysPerWeek.ToString();
//                constrNode22.AppendChild(maxDaysNode1);

//                AddCommonTagsTime(null, doc, constrNode22);

//                timeNode.AppendChild(constrNode22);

            

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsActivityTagMaxHoursContinuouslyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsActivityTagMaxHoursContinuously");

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Continuously");
//                maxHoursNode.InnerText = constr.MaximumHoursContinuously.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMaxHoursContinuouslyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxHoursContinuously");

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Continuously");
//                maxHoursNode.InnerText = constr.MaximumHoursContinuously.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintThreeActivitiesGroupedList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintThreeActivitiesGrouped");

//                XmlNode firstActivityNode = doc.CreateElement("First_Activity_Id");
//                firstActivityNode.InnerText = constr.ActivityIds.ElementAt(0).ToString();
//                constrNode.AppendChild(firstActivityNode);

//                XmlNode secondActivityNode = doc.CreateElement("Second_Activity_Id");
//                secondActivityNode.InnerText = constr.ActivityIds.ElementAt(1).ToString();
//                constrNode.AppendChild(secondActivityNode);

//                XmlNode thirdActivityNode = doc.CreateElement("Third_Activity_Id");
//                thirdActivityNode.InnerText = constr.ActivityIds.ElementAt(2).ToString();
//                constrNode.AppendChild(thirdActivityNode);

                
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTwoActivitiesGroupedList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTwoActivitiesGrouped");

//                XmlNode firstActivityNode = doc.CreateElement("First_Activity_Id");
//                firstActivityNode.InnerText = constr.ActivityIds.ElementAt(0).ToString();
//                constrNode.AppendChild(firstActivityNode);

//                XmlNode secondActivityNode = doc.CreateElement("Second_Activity_Id");
//                secondActivityNode.InnerText = constr.ActivityIds.ElementAt(1).ToString();
//                constrNode.AppendChild(secondActivityNode);
                
//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetMaxGapsPerDayList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxGapsPerDay");

//                XmlNode maxGapsNode7 = doc.CreateElement("Max_Gaps");
//                maxGapsNode7.InnerText = constr.MaxGaps.ToString();
//                constrNode.AppendChild(maxGapsNode7);

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherActivityTagMaxHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherActivityTagMaxHoursDaily");

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Daily");
//                maxHoursNode.InnerText = constr.MaximumHoursDaily.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

            
//                XmlNode constrNode23 = doc.CreateElement("ConstraintTeachersMaxHoursContinuously");

//                XmlNode maxHoursNode23 = doc.CreateElement("Maximum_Hours_Continuously");
//                maxHoursNode23.InnerText = timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxHoursContinuously.ToString();
//                constrNode23.AppendChild(maxHoursNode23);

//                AddCommonTagsTime(null, doc, constrNode23);

//                timeNode.AppendChild(constrNode23);

            

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeachersActivityTagMaxHoursContinuously");

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Continuously");
//                maxHoursNode.InnerText = constr.MaximumHoursContinuously.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeachersActivityTagMaxHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeachersActivityTagMaxHoursDaily");

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Daily");
//                maxHoursNode.InnerText = constr.MaximumHoursDaily.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherMinDaysPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherMinDaysPerWeek");

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode minHoursNode = doc.CreateElement("Minimum_Days_Per_Week");
//                minHoursNode.InnerText = constr.MinimumDaysPerWeek.ToString();
//                constrNode.AppendChild(minHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

           
//                XmlNode constrNode14 = doc.CreateElement("ConstraintTeachersMaxDaysPerWeek");

//                XmlNode maxDayNode = doc.CreateElement("Max_Days_Per_Week");
//                maxDayNode.InnerText = timetable.TimeConstraints.ConstraintsTeachers.TeachersMaxDaysPerWeek.ToString();
//                constrNode14.AppendChild(maxDayNode);

//                AddCommonTagsTime(null, doc, constrNode14);

//                timeNode.AppendChild(constrNode14);

            


//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsActivityTagMaxHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsActivityTagMaxHoursDaily");

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Daily");
//                maxHoursNode.InnerText = constr.MaximumHoursDaily.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

            
//                XmlNode constrNode13 = doc.CreateElement("ConstraintStudentsMaxHoursContinuously");

//                XmlNode maxHoursNode3 = doc.CreateElement("Maximum_Hours_Continuously");
//                maxHoursNode3.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsMaxHoursContinuously.ToString();
//                constrNode13.AppendChild(maxHoursNode3);

//                AddCommonTagsTime(null, doc, constrNode13);

//                timeNode.AppendChild(constrNode13);

            

//            foreach (var constr in timetable.TimeConstraints.ConstraintTwoActivitiesOrderedList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTwoActivitiesOrdered");

//                XmlNode firstIdNode = doc.CreateElement("First_Activity_Id");
//                firstIdNode.InnerText = constr.ActivityIds.ElementAt(0).ToString();
//                constrNode.AppendChild(firstIdNode);

//                XmlNode secondIdNode = doc.CreateElement("Second_Activity_Id");
//                secondIdNode.InnerText = constr.ActivityIds.ElementAt(1).ToString();
//                constrNode.AppendChild(secondIdNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesEndStudentsDayList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesEndStudentsDay");

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode studentsNode = doc.CreateElement("Students_Name");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                XmlNode subjectNode = doc.CreateElement("Subject_Name");
//                subjectNode.InnerText = constr.Subject;
//                constrNode.AppendChild(subjectNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintSubactivitiesPreferredStartingTimes");

//                XmlNode componentNrNode = doc.CreateElement("Component_Number");
//                componentNrNode.InnerText = constr.ComponentNumber.ToString();
//                constrNode.AppendChild(componentNrNode);

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode studentsNode = doc.CreateElement("Students_Name");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                XmlNode subjectNode = doc.CreateElement("Subject_Name");
//                subjectNode.InnerText = constr.Subject;
//                constrNode.AppendChild(subjectNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode nrOfPrefNode = doc.CreateElement("Number_of_Preferred_Starting_Times");
//                nrOfPrefNode.InnerText = constr.PreferredStartingTimes.Count().ToString();
//                constrNode.AppendChild(nrOfPrefNode);           
                
//                foreach (var prefStartTime in constr.PreferredStartingTimes)
//                {
//                    XmlNode prefTimeNode = doc.CreateElement("Preferred_Starting_Time");

//                    XmlNode DayConstraintNode = doc.CreateElement("Preferred_Starting_Day");
//                    DayConstraintNode.InnerText = prefStartTime.Day;
//                    prefTimeNode.AppendChild(DayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Starting_Hour");
//                    HourConstraintNode.InnerText = prefStartTime.Hour;
//                    prefTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(prefTimeNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursDailyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetActivityTagMaxHoursDaily");

//                XmlNode maxHourNode = doc.CreateElement("Maximum_Hours_Daily");
//                maxHourNode.InnerText = constr.MaximumHoursDaily.ToString();
//                constrNode.AppendChild(maxHourNode);

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintSubactivitiesPreferredTimeSlots");

//                XmlNode componentNrNode = doc.CreateElement("Component_Number");
//                componentNrNode.InnerText = constr.ComponentNumber.ToString();
//                constrNode.AppendChild(componentNrNode);

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode studentsNode = doc.CreateElement("Students_Name");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                XmlNode subjectNode = doc.CreateElement("Subject_Name");
//                subjectNode.InnerText = constr.Subject;
//                constrNode.AppendChild(subjectNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode nrOfPrefNode = doc.CreateElement("Number_of_Preferred_Time_Slots");
//                nrOfPrefNode.InnerText = constr.PreferredTimeSlots.Count().ToString();
//                constrNode.AppendChild(nrOfPrefNode);

//                foreach (var prefTime in constr.PreferredTimeSlots)
//                {
//                    XmlNode prefTimeNode = doc.CreateElement("Preferred_Time_Slot");

//                    XmlNode dayConstraintNode = doc.CreateElement("Preferred_Day");
//                    dayConstraintNode.InnerText = prefTime.Day;
//                    prefTimeNode.AppendChild(dayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Hour");
//                    HourConstraintNode.InnerText = prefTime.Hour;
//                    prefTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(prefTimeNode);
//                }

//                AddCommonTagsTime(constr, doc, constrNode);
//                timeNode.AppendChild(constrNode);
//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintMinGapsBetweenActivitiesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintMinGapsBetweenActivities");

//                XmlNode nrOfIdsNode = doc.CreateElement("Number_of_Activities");
//                nrOfIdsNode.InnerText = constr.ActivityIds.Count().ToString();
//                constrNode.AppendChild(nrOfIdsNode);

//                foreach (var id in constr.ActivityIds)
//                {
//                    XmlNode idConstraintNode = doc.CreateElement("Activity_Id");
//                    idConstraintNode.InnerText = id.ToString();
//                    constrNode.AppendChild(idConstraintNode);
//                }

//                XmlNode minGapsNode = doc.CreateElement("MinGaps");
//                minGapsNode.InnerText = constr.MinGaps.ToString();
//                constrNode.AppendChild(minGapsNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetIntervalMaxDaysPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetIntervalMaxDaysPerWeek");

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.Students;
//                constrNode.AppendChild(studentsNode);

//                XmlNode intervalStartNode = doc.CreateElement("Interval_Start_Hour");
//                intervalStartNode.InnerText = constr.IntervalStartHour;
//                constrNode.AppendChild(intervalStartNode);

//                XmlNode intervalEndNode = doc.CreateElement("Interval_End_Hour");
//                intervalEndNode.InnerText = constr.IntervalEndHour;
//                constrNode.AppendChild(intervalEndNode);

//                XmlNode maxDaysNode = doc.CreateElement("Max_Days_Per_Week");
//                maxDaysNode.InnerText = constr.MaxDaysPerWeek.ToString();
//                constrNode.AppendChild(maxDaysNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

            
//                XmlNode constrNode12 = doc.CreateElement("ConstraintTeachersMinDaysPerWeek");


//                XmlNode minDaysNode = doc.CreateElement("Minimum_Days_Per_Week");
//                minDaysNode.InnerText = timetable.TimeConstraints.ConstraintsTeachers.TeachersMinDaysPerWeek.ToString();
//                constrNode12.AppendChild(minDaysNode);

//                AddCommonTagsTime(null, doc, constrNode12);

//                timeNode.AppendChild(constrNode12);

            

//            foreach (var constr in timetable.TimeConstraints.ConstraintActivitiesOccupyMaxTimeSlotsFromSelectionList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintActivitiesOccupyMaxTimeSlotsFromSelection");

//                XmlNode nrOfIdsNode = doc.CreateElement("Number_of_Activities");
//                nrOfIdsNode.InnerText = constr.ActivityIds.Count().ToString();
//                constrNode.AppendChild(nrOfIdsNode);

//                foreach (var id in constr.ActivityIds)
//                {
//                    XmlNode idConstraintNode = doc.CreateElement("Activity_Id");
//                    idConstraintNode.InnerText = id.ToString();
//                    constrNode.AppendChild(idConstraintNode);
//                }

//                XmlNode nrOfSelectedTimeSlotsNode = doc.CreateElement("Number_of_Selected_Time_Slots");
//                nrOfSelectedTimeSlotsNode.InnerText = constr.SelectedTimeSlots.Count().ToString();
//                constrNode.AppendChild(nrOfSelectedTimeSlotsNode);

//                foreach (var selectedTime in constr.SelectedTimeSlots)
//                {
//                    XmlNode selectedTimeNode = doc.CreateElement("Selected_Time_Slot");

//                    XmlNode dayConstraintNode = doc.CreateElement("Preferred_Day");
//                    dayConstraintNode.InnerText = selectedTime.Day;
//                    selectedTimeNode.AppendChild(dayConstraintNode);

//                    XmlNode HourConstraintNode = doc.CreateElement("Preferred_Hour");
//                    HourConstraintNode.InnerText = selectedTime.Hour;
//                    selectedTimeNode.AppendChild(HourConstraintNode);

//                    constrNode.AppendChild(selectedTimeNode);
//                }


//                XmlNode maxOccupiedSlotsNode = doc.CreateElement("Max_Number_of_Occupied_Time_Slots");
//                maxOccupiedSlotsNode.InnerText = constr.MaxNumberOfOccupiedTimeSlots.ToString();
//                constrNode.AppendChild(maxOccupiedSlotsNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            foreach (var constr in timetable.TimeConstraints.ConstraintTeacherActivityTagMaxHoursContinuouslyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintTeacherActivityTagMaxHoursContinuously");

//                XmlNode teacherNode = doc.CreateElement("Teacher_Name");
//                teacherNode.InnerText = constr.Teacher.Name;
//                constrNode.AppendChild(teacherNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxHoursNode = doc.CreateElement("Maximum_Hours_Continuously");
//                maxHoursNode.InnerText = constr.MaximumHoursContinuously.ToString();
//                constrNode.AppendChild(maxHoursNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }
//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetMaxDaysPerWeekList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxDaysPerWeek");

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.StudentsSet;
//                constrNode.AppendChild(studentsNode);

//                XmlNode maxDaysNode = doc.CreateElement("Max_Days_Per_Week");
//                maxDaysNode.InnerText = constr.MaxDaysPerWeek.ToString();
//                constrNode.AppendChild(maxDaysNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }
//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetMaxHoursContinuouslyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxHoursContinuously");

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.StudentsSet;
//                constrNode.AppendChild(studentsNode);

//                XmlNode maxDaysNode = doc.CreateElement("Maximum_Hours_Continuously");
//                maxDaysNode.InnerText = constr.MaximumHoursContinuously.ToString();
//                constrNode.AppendChild(maxDaysNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }
//            foreach (var constr in timetable.TimeConstraints.ConstraintStudentsSetActivityTagMaxHoursContinuouslyList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetActivityTagMaxHoursContinuously");

//                XmlNode studentsNode = doc.CreateElement("Students");
//                studentsNode.InnerText = constr.StudentsSet;
//                constrNode.AppendChild(studentsNode);

//                XmlNode activityTagNode = doc.CreateElement("Activity_Tag_Name");
//                activityTagNode.InnerText = constr.ActivityTag;
//                constrNode.AppendChild(activityTagNode);

//                XmlNode maxDaysNode = doc.CreateElement("Maximum_Hours_Continuously");
//                maxDaysNode.InnerText = constr.MaximumHoursContinuously.ToString();
//                constrNode.AppendChild(maxDaysNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }
            
//                XmlNode constrNode11 = doc.CreateElement("ConstraintStudentsMaxDaysPerWeek");
                
//                XmlNode maxDaysNode12 = doc.CreateElement("Max_Days_Per_Week");
//                maxDaysNode12.InnerText = timetable.TimeConstraints.ConstraintsStudents.StudentsMaxDaysPerWeek.ToString();
//                constrNode11.AppendChild(maxDaysNode12);

//                AddCommonTagsTime(null, doc, constrNode11);

//                timeNode.AppendChild(constrNode11);

            

//            foreach (var constr in timetable.TimeConstraints.ConstraintMaxDaysBetweenActivitiesList)
//            {
//                XmlNode constrNode = doc.CreateElement("ConstraintMaxDaysBetweenActivities");

//                XmlNode nrOfIdsNode = doc.CreateElement("Number_of_Activities");
//                nrOfIdsNode.InnerText = constr.ActivityIds.Count().ToString();
//                constrNode.AppendChild(nrOfIdsNode);

//                foreach (var id in constr.ActivityIds)
//                {
//                    XmlNode idConstraintNode = doc.CreateElement("Activity_Id");
//                    idConstraintNode.InnerText = id.ToString();
//                    constrNode.AppendChild(idConstraintNode);
//                }

//                XmlNode maxOccupiedSlotsNode = doc.CreateElement("Max_Days");
//                maxOccupiedSlotsNode.InnerText = constr.MaxDays.ToString();
//                constrNode.AppendChild(maxOccupiedSlotsNode);

//                AddCommonTagsTime(constr, doc, constrNode);

//                timeNode.AppendChild(constrNode);

//            }

//            fetNode.AppendChild(timeNode);
//        }
        /// <summary>
        /// add space constraints to a xml doc from timetable
        /// </summary>
        /// <param name="fetNode"></param>
        /// <param name="doc"></param>
        /// <param name="timetable"></param>
        //private static void AddSpaceConstraints(XmlNode fetNode, XmlDocument doc, Timetable timetable)
        //{
        //    XmlNode spaceNode = doc.CreateElement("Space_Constraints_List");

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintTeacherMinGapsBetweenBuildingChangesList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintTeacherMinGapsBetweenBuildingChanges");

        //        XmlNode teacherNode = doc.CreateElement("Teacher");
        //        teacherNode.InnerText = constr.Teacher.Name;
        //        constrNode.AppendChild(teacherNode);

        //        XmlNode minGapsNode = doc.CreateElement("Min_Gaps_Between_Building_Changes");
        //        minGapsNode.InnerText = constr.MinGapsBetweenBuildingChanges.ToString();
        //        constrNode.AppendChild(minGapsNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);

        //        spaceNode.AppendChild(constrNode);

        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintTeacherHomeRoomList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintTeacherHomeRoom");

        //        XmlNode TeacherNode = doc.CreateElement("Teacher");
        //        TeacherNode.InnerText = constr.Teacher.Name;
        //        constrNode.AppendChild(TeacherNode);

        //        XmlNode RoomNode = doc.CreateElement("Room");
        //        RoomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(RoomNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);


        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintSubjectActivityTagPreferredRoom");

        //        XmlNode SubjectNode = doc.CreateElement("Subject");
        //        SubjectNode.InnerText = constr.Subject;
        //        constrNode.AppendChild(SubjectNode);

        //        XmlNode ActivityTagNode = doc.CreateElement("Activity_Tag");
        //        ActivityTagNode.InnerText = constr.ActivityTag;
        //        constrNode.AppendChild(ActivityTagNode);

        //        XmlNode RoomNode = doc.CreateElement("Room");
        //        RoomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(RoomNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);


        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintRoomNotAvailableTimesList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintRoomNotAvailableTimes");

        //        XmlNode RoomNode = doc.CreateElement("Room");
        //        RoomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(RoomNode);

        //        XmlNode nrOfNotAvailableTimesNode = doc.CreateElement("Number_of_Not_Available_Times");
        //        nrOfNotAvailableTimesNode.InnerText = constr.NotAvailableTimes.Count().ToString();
        //        constrNode.AppendChild(nrOfNotAvailableTimesNode);

        //        foreach (var timeNode in constr.NotAvailableTimes)
        //        {
        //            XmlNode notAvailableNode = doc.CreateElement("Not_Available_Time");

        //            XmlNode DayConstraintNode = doc.CreateElement("Day");
        //            DayConstraintNode.InnerText = timeNode.Day;
        //            notAvailableNode.AppendChild(DayConstraintNode);

        //            XmlNode HourConstraintNode = doc.CreateElement("Hour");
        //            HourConstraintNode.InnerText = timeNode.Hour;
        //            notAvailableNode.AppendChild(HourConstraintNode);

        //            constrNode.AppendChild(notAvailableNode);


        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }


        //    foreach (var constr in timetable.SpaceConstraints.ConstraintTeacherHomeRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintTeacherHomeRooms");

        //        XmlNode TeacherNode = doc.CreateElement("Teacher");
        //        TeacherNode.InnerText = constr.Teacher.Name;
        //        constrNode.AppendChild(TeacherNode);

        //        XmlNode NumberofPreferredRoomsNode = doc.CreateElement("Number_of_Preferred_Rooms");
        //        NumberofPreferredRoomsNode.InnerText = constr.Rooms.Count().ToString();
        //        constrNode.AppendChild(NumberofPreferredRoomsNode);

        //        foreach (var room in constr.Rooms)
        //        {
        //            XmlNode prefRoomNode = doc.CreateElement("Preferred_Room");
        //            prefRoomNode.InnerText = room.Name;
        //            constrNode.AppendChild(prefRoomNode);

        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);
        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintActivityPreferredRoomList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintActivityPreferredRoom");

        //        XmlNode ActivityIdNode = doc.CreateElement("Activity_Id");
        //        ActivityIdNode.InnerText = constr.ActivityId.ToString();
        //        constrNode.AppendChild(ActivityIdNode);

        //        XmlNode RoomNode = doc.CreateElement("Room");
        //        RoomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(RoomNode);

        //        XmlNode PermanentlyLockedNode = doc.CreateElement("Permanently_Locked");
        //        PermanentlyLockedNode.InnerText = constr.PermanentlyLocked.ToString();
        //        constrNode.AppendChild(PermanentlyLockedNode);
                
        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }


        //    foreach (var constr in timetable.SpaceConstraints.ConstraintActivityPreferredRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintActivityPreferredRooms");

        //        XmlNode ActivityIdNode = doc.CreateElement("Activity_Id");
        //        ActivityIdNode.InnerText = constr.ActivityId.ToString();
        //        constrNode.AppendChild(ActivityIdNode);

        //        foreach (var roomNode in constr.Rooms)
        //        {
        //            XmlNode prefroomNode = doc.CreateElement("Preferred_Room");
        //            prefroomNode.InnerText = roomNode.Name;
        //            constrNode.AppendChild(prefroomNode);

        //        }

        //        XmlNode NumberofPreferredRoomsNode = doc.CreateElement("Number_of_Preferred_Rooms");
        //        NumberofPreferredRoomsNode.InnerText = constr.Rooms.Count().ToString();
        //        constrNode.AppendChild(NumberofPreferredRoomsNode);
                
        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }


        //    foreach (var constr in timetable.SpaceConstraints.ConstraintSubjectPreferredRoomList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintSubjectPreferredRoom");

        //        XmlNode SubjectNode = doc.CreateElement("Subject");
        //        SubjectNode.InnerText = constr.Subject;
        //        constrNode.AppendChild(SubjectNode);

        //        XmlNode RoomNode = doc.CreateElement("Room");
        //        RoomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(RoomNode);
                
        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintSubjectActivityTagPreferredRooms");

        //        XmlNode SubjectNode = doc.CreateElement("Subject");
        //        SubjectNode.InnerText = constr.Subject;
        //        constrNode.AppendChild(SubjectNode);

        //        XmlNode ActivityTagNode = doc.CreateElement("Activity_Tag");
        //        ActivityTagNode.InnerText = constr.ActivityTag;
        //        constrNode.AppendChild(ActivityTagNode);

        //        XmlNode NumberofPreferredRoomsNode = doc.CreateElement("Number_of_Preferred_Rooms");
        //        NumberofPreferredRoomsNode.InnerText = constr.Rooms.Count().ToString();
        //        constrNode.AppendChild(NumberofPreferredRoomsNode);

        //        foreach (var room in constr.Rooms)
        //        {
        //            XmlNode prefRoomNode = doc.CreateElement("Preferred_Room");
        //            prefRoomNode.InnerText = room.Name;
        //            constrNode.AppendChild(prefRoomNode);
        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);
        //    }


        //    foreach (var constr in timetable.SpaceConstraints.ConstraintTeacherMaxBuildingChangesPerDayList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxBuildingChangesPerDay");

        //        XmlNode TeacherNode = doc.CreateElement("Teacher");
        //        TeacherNode.InnerText = constr.Teacher.Name;
        //        constrNode.AppendChild(TeacherNode);

        //        XmlNode MaxBuildingChangesPerDayNode = doc.CreateElement("Max_Building_Changes_Per_Day");
        //        MaxBuildingChangesPerDayNode.InnerText = constr.MaxBuildingChangesPerDay.ToString();
        //        constrNode.AppendChild(MaxBuildingChangesPerDayNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }

            
        //        XmlNode constrNode9 = doc.CreateElement("ConstraintTeachersMaxBuildingChangesPerDay");

        //        XmlNode MaxBuildingChangesPerDayNode1 = doc.CreateElement("Max_Building_Changes_Per_Day");
        //        MaxBuildingChangesPerDayNode1.InnerText = timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerDay.ToString();
        //        constrNode9.AppendChild(MaxBuildingChangesPerDayNode1);

        //        AddCommonTagsSpace(null, doc, constrNode9);
        //        spaceNode.AppendChild(constrNode9);

            

            
        //        XmlNode constrNode8 = doc.CreateElement("ConstraintStudentsMaxBuildingChangesPerDay");
                
        //        XmlNode MaxBuildingChangesPerDayNode2 = doc.CreateElement("Max_Building_Changes_Per_Day");
        //        MaxBuildingChangesPerDayNode2.InnerText = timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerDay.ToString();
        //        constrNode8.AppendChild(MaxBuildingChangesPerDayNode2);

        //        AddCommonTagsSpace(null, doc, constrNode8);
        //        spaceNode.AppendChild(constrNode8);
                                
            

           
        //        XmlNode constrNode7 = doc.CreateElement("ConstraintStudentsMinGapsBetweenBuildingChanges");
                
        //        XmlNode MinGapsBetweenBuildingChangesNode = doc.CreateElement("Min_Gaps_Between_Building_Changes");
        //        MinGapsBetweenBuildingChangesNode.InnerText = timetable.TimeConstraints.ConstraintsStudents.MinGapsBetweenBuildingChanges.ToString();
        //        constrNode7.AppendChild(MinGapsBetweenBuildingChangesNode);

        //        AddCommonTagsSpace(null, doc, constrNode7);
        //        spaceNode.AppendChild(constrNode7);

            

           
        //        XmlNode constrNode6 = doc.CreateElement("ConstraintStudentsMaxBuildingChangesPerWeek");

        //        XmlNode maxBuildingChangesNode = doc.CreateElement("Max_Building_Changes_Per_Week");
        //        maxBuildingChangesNode.InnerText = timetable.TimeConstraints.ConstraintsStudents.MaxBuildingChangesPerWeek.ToString();
        //        constrNode6.AppendChild(maxBuildingChangesNode);

        //        AddCommonTagsSpace(null, doc, constrNode6);
        //        spaceNode.AppendChild(constrNode6);

            

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintActivityTagPreferredRoomList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintActivityTagPreferredRoom");

        //        XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
        //        activityTagNode.InnerText = constr.ActivityTag;
        //        constrNode.AppendChild(activityTagNode);

        //        XmlNode roomNode = doc.CreateElement("Room");
        //        roomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(roomNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintActivityTagPreferredRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintActivityTagPreferredRooms");

        //        XmlNode activityTagNode = doc.CreateElement("Activity_Tag");
        //        activityTagNode.InnerText = constr.ActivityTag;
        //        constrNode.AppendChild(activityTagNode);

        //        XmlNode nrOfPreferredRoomsNode = doc.CreateElement("Number_of_Preferred_Rooms");
        //        nrOfPreferredRoomsNode.InnerText = constr.Rooms.Count().ToString();
        //        constrNode.AppendChild(nrOfPreferredRoomsNode);

        //        foreach (var room in constr.Rooms)
        //        {
        //            XmlNode prefRoomNode = doc.CreateElement("Preferred_Room");
        //            prefRoomNode.InnerText = room.Name;
        //            constrNode.AppendChild(prefRoomNode);
        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);
        //    }
        //    foreach (var constr in timetable.SpaceConstraints.ConstraintTeacherMaxBuildingChangesPerWeekList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintTeacherMaxBuildingChangesPerWeek");

        //        XmlNode activityTagNode = doc.CreateElement("Teacher");
        //        activityTagNode.InnerText = constr.Teacher.Name;
        //        constrNode.AppendChild(activityTagNode);

        //        XmlNode maxChangesNode = doc.CreateElement("Max_Building_Changes_Per_Week");
        //        maxChangesNode.InnerText = constr.MaxBuildingChangesPerWeek.ToString();
        //        constrNode.AppendChild(maxChangesNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }
           
            
        //        XmlNode constrNode5 = doc.CreateElement("ConstraintTeachersMaxBuildingChangesPerWeek");

        //        XmlNode maxNode5 = doc.CreateElement("Max_Building_Changes_Per_Week");
        //        maxNode5.InnerText = timetable.TimeConstraints.ConstraintsTeachers.MaxBuildingChangesPerWeek.ToString();
        //        constrNode5.AppendChild(maxNode5);

        //        AddCommonTagsSpace(null, doc, constrNode5);
        //        spaceNode.AppendChild(constrNode5);

            

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintStudentsSetHomeRoomList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetHomeRoom");

        //        XmlNode studentNode = doc.CreateElement("Students");
        //        studentNode.InnerText = constr.Students;
        //        constrNode.AppendChild(studentNode);

        //        XmlNode roomNode = doc.CreateElement("Room");
        //        roomNode.InnerText = constr.Room.Name;
        //        constrNode.AppendChild(roomNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintSubjectPreferredRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintSubjectPreferredRooms");

        //        XmlNode subjectNode = doc.CreateElement("Students");
        //        subjectNode.InnerText = constr.Subject;
        //        constrNode.AppendChild(subjectNode);

        //        XmlNode nrPrefRoomsNode = doc.CreateElement("Number_of_Preferred_Rooms");
        //        nrPrefRoomsNode.InnerText = constr.Rooms.Count().ToString();
        //        constrNode.AppendChild(nrPrefRoomsNode);

        //        foreach (var room in constr.Rooms)
        //        {
        //            XmlNode roomNode = doc.CreateElement("Preferred_Room");
        //            roomNode.InnerText = room.Name;
        //            constrNode.AppendChild(roomNode);

        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintActivitiesOccupyMaxDifferentRooms");

        //        XmlNode nrOfActivitiesNode = doc.CreateElement("Number_of_Activities");
        //        nrOfActivitiesNode.InnerText = constr.ActivityIds.Count().ToString();
        //        constrNode.AppendChild(nrOfActivitiesNode);
                
        //        foreach (var id in constr.ActivityIds)
        //        {
        //            XmlNode idNode = doc.CreateElement("Activity_Id");
        //            idNode.InnerText = id.ToString();
        //            constrNode.AppendChild(idNode);

        //        }

        //        XmlNode maxNrOfDifRoomsNode = doc.CreateElement("Max_Number_of_Different_Rooms");
        //        maxNrOfDifRoomsNode.InnerText = constr.MaxNumberOfDifferentRooms.ToString();
        //        constrNode.AppendChild(maxNrOfDifRoomsNode);
                
        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }


        //    foreach (var constr in timetable.SpaceConstraints.ConstraintStudentsSetHomeRoomsList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetHomeRooms");

        //        XmlNode studentsNode = doc.CreateElement("Students");
        //        studentsNode.InnerText = constr.Students;
        //        constrNode.AppendChild(studentsNode);

        //        XmlNode nrOfPrefRoomsNode = doc.CreateElement("Number_of_Preferred_Rooms");
        //        nrOfPrefRoomsNode.InnerText = constr.Rooms.Count().ToString();
        //        constrNode.AppendChild(nrOfPrefRoomsNode);

        //        foreach (var room in constr.Rooms)
        //        {
        //            XmlNode roomNode = doc.CreateElement("Preferred_Room");
        //            roomNode.InnerText = room.Name;
        //            constrNode.AppendChild(roomNode);

        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }

            
        //        XmlNode constrNode4 = doc.CreateElement("ConstraintTeachersMinGapsBetweenBuildingChanges");
                                
        //        XmlNode maxNode4 = doc.CreateElement("Min_Gaps_Between_Building_Changes");
        //        maxNode4.InnerText = timetable.TimeConstraints.ConstraintsTeachers.MinGapsBetweenBuildingChanges.ToString();
        //        constrNode4.AppendChild(maxNode4);

        //        AddCommonTagsSpace(null, doc, constrNode4);
        //        spaceNode.AppendChild(constrNode4);

            

        //    foreach (var constr in timetable.SpaceConstraints.ConstraintStudentsSetMaxBuildingChangesPerDayList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxBuildingChangesPerDay");

        //        XmlNode studentsNode = doc.CreateElement("Students");
        //        studentsNode.InnerText = constr.Students;
        //        constrNode.AppendChild(studentsNode);

        //        XmlNode maxNode = doc.CreateElement("Max_Building_Changes_Per_Day");
        //        maxNode.InnerText = constr.MaxBuildingChangesPerDay.ToString();
        //        constrNode.AppendChild(maxNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }
        //    foreach (var constr in timetable.SpaceConstraints.ConstraintStudentsSetMaxBuildingChangesPerWeekList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMaxBuildingChangesPerWeek");

        //        XmlNode studentsNode = doc.CreateElement("Students");
        //        studentsNode.InnerText = constr.Students;
        //        constrNode.AppendChild(studentsNode);

        //        XmlNode maxNode = doc.CreateElement("Max_Building_Changes_Per_Week");
        //        maxNode.InnerText = constr.MaxBuildingChangesPerWeek.ToString();
        //        constrNode.AppendChild(maxNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }
        //    foreach (var constr in timetable.SpaceConstraints.ConstraintStudentsSetMinGapsBetweenBuildingChangesList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintStudentsSetMinGapsBetweenBuildingChanges");

        //        XmlNode studentsNode = doc.CreateElement("Students");
        //        studentsNode.InnerText = constr.Students;
        //        constrNode.AppendChild(studentsNode);

        //        XmlNode maxNode = doc.CreateElement("Min_Gaps_Between_Building_Changes");
        //        maxNode.InnerText = constr.MinGapsBetweenBuildingChanges.ToString();
        //        constrNode.AppendChild(maxNode);

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }
        //    foreach (var constr in timetable.SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList)
        //    {
        //        XmlNode constrNode = doc.CreateElement("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive");

        //        XmlNode nrOfActivitiesNode = doc.CreateElement("Number_of_Activities");
        //        nrOfActivitiesNode.InnerText = constr.ActivityIds.Count().ToString();
        //        constrNode.AppendChild(nrOfActivitiesNode);

        //        foreach (var id in constr.ActivityIds)
        //        {
        //            XmlNode idNode = doc.CreateElement("Activity_Id");
        //            idNode.InnerText = id.ToString();
        //            constrNode.AppendChild(idNode);

        //        }

        //        AddCommonTagsSpace(constr, doc, constrNode);
        //        spaceNode.AppendChild(constrNode);

        //    }
        //    fetNode.AppendChild(spaceNode);
        //}
        /// <summary>
        /// add common tags time for a xml time constraint node 
        /// </summary>
        /// <param name="constr"></param>
        /// <param name="doc"></param>
        /// <param name="parentNode"></param>
        private static void AddCommonTagsTime(XmlDocument doc, XmlNode parentNode)
        {

            XmlNode weightPercentageNode = doc.CreateElement("Weight_Percentage");
            weightPercentageNode.InnerText = "100";
            parentNode.AppendChild(weightPercentageNode);

            XmlNode activeNode = doc.CreateElement("Active");
            activeNode.InnerText = "Active";
            parentNode.AppendChild(activeNode);

            XmlNode commentsNode = doc.CreateElement("Comments");
            commentsNode.InnerText = "";
            parentNode.AppendChild(commentsNode);

            return;
            
        }
        /// <summary>
        /// add common tags space for a xml time constraint node 
        /// </summary>
        /// <param name="constr"></param>
        /// <param name="doc"></param>
        /// <param name="parentNode"></param>
        private static void AddCommonTagsSpace( XmlDocument doc, XmlNode parentNode)
        {
            XmlNode weightPercentageNode = doc.CreateElement("Weight_Percentage");
            weightPercentageNode.InnerText = "100"; // constr.WeightPercentage.ToString();
            parentNode.AppendChild(weightPercentageNode);

            XmlNode activeNode = doc.CreateElement("Active");
            activeNode.InnerText = true.ToString(); // constr.Active.ToString();
            parentNode.AppendChild(activeNode);

            XmlNode commentsNode = doc.CreateElement("Comments");
            commentsNode.InnerText = ""; // constr.Comments.ToString();
            parentNode.AppendChild(commentsNode);

        }
    }


   

    
}
