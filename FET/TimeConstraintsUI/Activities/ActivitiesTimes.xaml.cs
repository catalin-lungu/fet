using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FET.Data;

namespace FET.TimeConstraintsUI.Activities
{
    /// <summary>
    /// Interaction logic for ActivitiesTimes.xaml
    /// </summary>
    public partial class ActivitiesTimes : Window
    {
        string constraint;
        public ActivitiesTimes(List<Data.ConstraintActivitiesPreferredStartingTimes> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ":" +
                    con.PreferredStartingTime.Count() + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                    con.ConstraintActivitiesPreferredStartingTimesId);
            }
            constraint = "ConstraintActivitiesPreferredStartingTimes";
            this.Title = (string)App.Current.TryFindResource("activitiesPreferredStartingTimes");
        }

        public ActivitiesTimes(List<Data.ConstraintActivitiesPreferredTimeSlots> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ":" +
                    con.PreferredTimeSlots.Count() + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                    con.ConstraintActivitiesPreferredTimeSlotsId);
            }
            constraint = "ConstraintActivitiesPreferredTimeSlots";
            this.Title = (string)App.Current.TryFindResource("activitiesPreferredTimeSlots");
        }

        public ActivitiesTimes(List<Data.ConstraintSubactivitiesPreferredStartingTimes> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ":" +
                    con.PreferredStartingTimes.Count() + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                    con.ConstraintSubactivitiesPreferredStartingTimesId);
            }
            constraint = "ConstraintSubactivitiesPreferredStartingTimes";
            this.Title = (string)App.Current.TryFindResource("subactivitiesPreferredStartingTimes");
        }

        public ActivitiesTimes(List<Data.ConstraintSubactivitiesPreferredTimeSlots> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ":" +
                    con.PreferredTimeSlots.Count() + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                    con.ConstraintSubactivitiesPreferredTimeSlotsId);
            }
            constraint = "ConstraintSubactivitiesPreferredTimeSlots";
            this.Title = (string)App.Current.TryFindResource("subactivitiesPreferredTimeSlots");
        }

        public ActivitiesTimes(List<Data.ConstraintActivitiesEndStudentsDay> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("subject") + ": " + con.Subject + " " +
                    (string)App.Current.TryFindResource("students") + ": " + con.Students + " " +
                    (string)App.Current.TryFindResource("teacher") + ": " + (con.Teacher != null ? con.Teacher.Name : "") + " - " + 
                    (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivitiesEndStudentsDayId);
            }
            constraint = "ConstraintActivitiesEndStudentsDay";
            this.Title = (string)App.Current.TryFindResource("activitiesEndStudentsDay");
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string activityTag;
            string teacherName;
            Data.Teacher teacher = null;
            string students;
            string subject;
            List<Data.TimeDayHour> slots = new List<Data.TimeDayHour>();
            
            string windowTitle =  DeterminChildWindowTitle(constraint);
            using (ActivitiesTimesSlotsItems window = new ActivitiesTimesSlotsItems(
                Timetable.GetInstance().DaysList, Timetable.GetInstance().HoursList, windowTitle))
            {
                window.ShowDialog();

                activityTag = window.GetActivityTag();
                teacherName = window.GetTecherName();
                students = window.GetStudents();
                subject = window.GetSubject();
                slots = window.GetSlots();
            }
            var teachers = from t in Timetable.GetInstance().TeacherList
                           where t.Name.Equals(teacherName)
                           select t;
            if (teachers.Count() > 0)
            {
                teacher = teachers.First();
            }

            if (slots.Count() > 0 || constraint.Equals("ConstraintActivitiesEndStudentsDay"))
            {
                if (constraint.Equals("ConstraintActivitiesPreferredStartingTimes"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList.Add(
                                new Data.ConstraintActivitiesPreferredStartingTimes()
                                {
                                    PreferredStartingTime = slots,
                                    ActivityTagName = activityTag,
                                    Teacher = teacher,
                                    StudentsName = students,
                                    SubjectName = subject
                                });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList.Last().PreferredStartingTime.Count() +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList.Last().ConstraintActivitiesPreferredStartingTimesId);
                }
                else if (constraint.Equals("ConstraintActivitiesPreferredTimeSlots"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList.Add(
                                new Data.ConstraintActivitiesPreferredTimeSlots()
                                {
                                    PreferredTimeSlots = slots,
                                    ActivityTagName = activityTag,
                                    Teacher = teacher,
                                    StudentsName = students,
                                    SubjectName = subject
                                });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList.Last().PreferredTimeSlots.Count() +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList.Last().ConstraintActivitiesPreferredTimeSlotsId);

                }
                else if (constraint.Equals("ConstraintSubactivitiesPreferredStartingTimes"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList.Add(
                                 new Data.ConstraintSubactivitiesPreferredStartingTimes()
                                 {
                                     PreferredStartingTimes = slots,
                                     ActivityTag = activityTag, 
                                     Teacher = teacher,
                                     Students = students,
                                     Subject = subject
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList.Last().PreferredStartingTimes.Count() +
                        " - " + (string)App.Current.TryFindResource("id") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList.Last().ConstraintSubactivitiesPreferredStartingTimesId);

                }
                else if (constraint.Equals("ConstraintSubactivitiesPreferredTimeSlots"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList.Add(
                                 new Data.ConstraintSubactivitiesPreferredTimeSlots()
                                 {
                                     PreferredTimeSlots = slots,
                                     ActivityTag = activityTag,
                                     Teacher = teacher,
                                     Students = students,
                                     Subject = subject
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList.Last().PreferredTimeSlots.Count() +
                        " - " + (string)App.Current.TryFindResource("id") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList.Last().ConstraintSubactivitiesPreferredTimeSlotsId);

                }
                else if (constraint.Equals("ConstraintActivitiesEndStudentsDay"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Add(
                                 new Data.ConstraintActivitiesEndStudentsDay()
                                 {
                                     ActivityTag = activityTag,
                                     Teacher = teacher,
                                     Students = students,
                                     Subject = subject
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("subject") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Last().Subject +
                        " " + (string)App.Current.TryFindResource("students") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Last().Students +
                        " " + (string)App.Current.TryFindResource("teacher") +": " + 
                        (Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Last().Teacher != null ? Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Last().Teacher.Name : "") +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Last().ConstraintActivitiesEndStudentsDayId);
                }

            }

        }      

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }
            string[] items = conString.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            int constrId = Convert.ToInt32(items[items.Count() - 1].Substring(items[items.Count() - 1].IndexOf(":") + 1));

            string activityTag="";
            string teacherName = "";
            string students = "";
            string subject = "";
            List<Data.TimeDayHour> selectedSlots = new List<Data.TimeDayHour>();

            if (constraint.Equals("ConstraintActivitiesPreferredStartingTimes"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList)
                {
                    if (con.ConstraintActivitiesPreferredStartingTimesId == constrId)
                    {
                        selectedSlots = con.PreferredStartingTime;
                        activityTag = con.ActivityTagName;
                        teacherName = con.Teacher != null ? con.Teacher.Name : "";
                        students = con.StudentsName;
                        subject = con.SubjectName;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesPreferredTimeSlots"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList)
                {
                    if (con.ConstraintActivitiesPreferredTimeSlotsId == constrId)
                    {
                        selectedSlots = con.PreferredTimeSlots;
                        activityTag = con.ActivityTagName;
                        teacherName = con.Teacher != null ? con.Teacher.Name : "";
                        students = con.StudentsName;
                        subject = con.SubjectName;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredStartingTimes"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList)
                {
                    if (con.ConstraintSubactivitiesPreferredStartingTimesId == constrId)
                    {
                        selectedSlots = con.PreferredStartingTimes;
                        activityTag = con.ActivityTag;
                        teacherName = con.Teacher != null ? con.Teacher.Name : "";
                        students = con.Students;
                        subject = con.Subject;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredTimeSlots"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList)
                {
                    if (con.ConstraintSubactivitiesPreferredTimeSlotsId == constrId)
                    {
                        selectedSlots = con.PreferredTimeSlots;
                        activityTag = con.ActivityTag;
                        teacherName = con.Teacher != null ? con.Teacher.Name : "";
                        students = con.Students;
                        subject = con.Subject;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesEndStudentsDay"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList)
                {
                    if (con.ConstraintActivitiesEndStudentsDayId == constrId)
                    {
                        activityTag = con.ActivityTag;
                        teacherName = con.Teacher != null ? con.Teacher.Name : "";
                        students = con.Students;
                        subject = con.Subject;
                        break;
                    }
                }
            }

            string windowTitle = DeterminChildWindowTitle(constraint);

            string newActivityTag;
            string newTeacherName;
            string newStudents;
            string newSubject;
            List<Data.TimeDayHour> newSlots = new List<Data.TimeDayHour>();
            using (ActivitiesTimesSlotsItems window = new ActivitiesTimesSlotsItems(
                Timetable.GetInstance().DaysList, Timetable.GetInstance().HoursList, windowTitle, selectedSlots, teacherName, students, subject, activityTag))
            {
                window.ShowDialog();

                newActivityTag = window.GetActivityTag();
                newTeacherName = window.GetTecherName();
                newStudents = window.GetStudents();
                newSubject = window.GetSubject();
                newSlots = window.GetSlots();
            }
            if (newSlots.Count() < 1 && !constraint.Equals("ConstraintActivitiesEndStudentsDay"))
            {
                return;
            }

            Data.Teacher newTeacher= null;
            var teachers = from t in Timetable.GetInstance().TeacherList
                           where t.Name.Equals(newTeacherName)
                           select t;
            if (teachers.Count() > 0)
            {
                newTeacher = teachers.First();
            }

            if (constraint.Equals("ConstraintActivitiesPreferredStartingTimes"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList
                          where c.ConstraintActivitiesPreferredStartingTimesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.PreferredStartingTime = newSlots;
                    constr.ActivityTagName = newActivityTag;
                    constr.Teacher = newTeacher;
                    constr.StudentsName = newStudents;
                    constr.SubjectName = newSubject;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ":" +
                    constr.PreferredStartingTime.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + 
                    constr.ConstraintActivitiesPreferredStartingTimesId);

                }
            }
            else if (constraint.Equals("ConstraintActivitiesPreferredTimeSlots"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList
                          where c.ConstraintActivitiesPreferredTimeSlotsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.PreferredTimeSlots = newSlots;
                    constr.ActivityTagName = newActivityTag;
                    constr.Teacher = newTeacher;
                    constr.StudentsName = newStudents;
                    constr.SubjectName = newSubject;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ":" +
                    constr.PreferredTimeSlots.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " +
                    constr.ConstraintActivitiesPreferredTimeSlotsId);

                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredStartingTimes"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList
                          where c.ConstraintSubactivitiesPreferredStartingTimesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.PreferredStartingTimes = newSlots;
                    constr.ActivityTag = newActivityTag;
                    constr.Teacher = newTeacher;
                    constr.Students = newStudents;
                    constr.Subject = newSubject;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ":" +
                    constr.PreferredStartingTimes.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " +
                    constr.ConstraintSubactivitiesPreferredStartingTimesId);

                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredTimeSlots"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList
                          where c.ConstraintSubactivitiesPreferredTimeSlotsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.PreferredTimeSlots = newSlots;
                    constr.ActivityTag = newActivityTag;
                    constr.Teacher = newTeacher;
                    constr.Students = newStudents;
                    constr.Subject = newSubject;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ":" +
                     constr.PreferredTimeSlots.Count() + 
                     " - "+ (string)App.Current.TryFindResource("id")+ ": " +
                     constr.ConstraintSubactivitiesPreferredTimeSlotsId);

                }
            }
            else if (constraint.Equals("ConstraintActivitiesEndStudentsDay"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList
                          where c.ConstraintActivitiesEndStudentsDayId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityTag = newActivityTag;
                    constr.Teacher = newTeacher;
                    constr.Students = newStudents;
                    constr.Subject = newSubject;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("subject")+ ": " + constr.Subject +
                        " " + (string)App.Current.TryFindResource("students") + ": " + constr.Students +
                        " " + (string)App.Current.TryFindResource("teacher") + ": " + (constr.Teacher != null ? constr.Teacher.Name : "") +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + constr.ConstraintActivitiesEndStudentsDayId);

                }
            }

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }
            string[] items = conString.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            int constrId = Convert.ToInt32(items[items.Count() - 1].Substring(items[items.Count() - 1].IndexOf(":") + 1));

            if (constraint.Equals("ConstraintActivitiesPreferredStartingTimes"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList
                          where c.ConstraintActivitiesPreferredStartingTimesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivitiesPreferredTimeSlots"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList
                          where c.ConstraintActivitiesPreferredTimeSlotsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredStartingTimes"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList
                          where c.ConstraintSubactivitiesPreferredStartingTimesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredTimeSlots"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList
                          where c.ConstraintSubactivitiesPreferredTimeSlotsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivitiesEndStudentsDay"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList
                          where c.ConstraintActivitiesEndStudentsDayId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listConstraints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            constraintInfo.Text = "";
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }
            string[] items = conString.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            int constrId = Convert.ToInt32(items[items.Count() - 1].Substring(items[items.Count() - 1].IndexOf(":") + 1));

            if (constraint.Equals("ConstraintActivitiesPreferredStartingTimes"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList)
                {
                    if (con.ConstraintActivitiesPreferredStartingTimesId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("activityTag")+ ": " + con.ActivityTagName + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (con.Teacher != null ? con.Teacher.Name : "") + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + con.StudentsName + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + con.SubjectName + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        foreach (var slot in con.PreferredStartingTime)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("day") + ": " + slot.Day + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("hour") + ": " + slot.Hour + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesPreferredTimeSlots"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList)
                {
                    if (con.ConstraintActivitiesPreferredTimeSlotsId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + con.ActivityTagName + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (con.Teacher != null ? con.Teacher.Name : "") + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + con.StudentsName + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + con.SubjectName + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        foreach (var slot in con.PreferredTimeSlots)
                        {

                            constraintInfo.Text += (string)App.Current.TryFindResource("day") + ": " + slot.Day + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("hour") + ": " + slot.Hour + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredStartingTimes"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList)
                {
                    if (con.ConstraintSubactivitiesPreferredStartingTimesId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + con.ActivityTag + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (con.Teacher != null ? con.Teacher.Name : "") + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + con.Students + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + con.Subject + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        foreach (var slot in con.PreferredStartingTimes)
                        {

                            constraintInfo.Text += (string)App.Current.TryFindResource("day") + ": " + slot.Day + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("hour") + ": " + slot.Hour + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintSubactivitiesPreferredTimeSlots"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList)
                {
                    if (con.ConstraintSubactivitiesPreferredTimeSlotsId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + con.ActivityTag + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (con.Teacher != null ? con.Teacher.Name : "") + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + con.Students + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + con.Subject + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        foreach (var slot in con.PreferredTimeSlots)
                        {

                            constraintInfo.Text += (string)App.Current.TryFindResource("day") + ": " + slot.Day + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("hour") + ": " + slot.Hour + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesEndStudentsDay"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList)
                {
                    if (con.ConstraintActivitiesEndStudentsDayId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + con.ActivityTag + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (con.Teacher != null ? con.Teacher.Name : "") + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + con.Students + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + con.Subject + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        
                        break;
                    }
                }
            }
        }

        private string DeterminChildWindowTitle(string constraint)
        {
            string title = "";
            switch (constraint)
            {
                case "ConstraintActivitiesPreferredStartingTimes":
                    title = (string)App.Current.TryFindResource("addActivitiesPreferredStartingTimes");
                    break;
                case "ConstraintActivitiesPreferredTimeSlots":
                    title = (string)App.Current.TryFindResource("addActivitiesPreferredTimeSlots");
                    break;
                case "ConstraintSubactivitiesPreferredStartingTimes":
                    title = (string)App.Current.TryFindResource("addSubactivitiesPreferredStartingTimes");
                    break;
                case "ConstraintSubactivitiesPreferredTimeSlots":
                    title = (string)App.Current.TryFindResource("addSubactivitiesPreferredTimeSlots");
                    break;
                case "ConstraintActivitiesEndStudentsDay":
                    title = (string)App.Current.TryFindResource("addActivitiesEndStudentsDay");
                    break;
            }

            return title;
        }
    }
}
