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
    /// Interaction logic for ActivitiesTimesSlotsItems.xaml
    /// </summary>
    public partial class ActivitiesTimesSlotsItems : Window, IDisposable
    {
        string activityTag;
        string teacherName;
        string students;
        string subject;
        List<Data.TimeDayHour> slots = new List<Data.TimeDayHour>();
        //bool acceptEmptyIds = false;

        public ActivitiesTimesSlotsItems(List<string> days, List<string> hours, string windowTitle)
        {
            InitializeComponent();
            foreach (var d in days)
            {
                dayComboBox.Items.Add(d);
            }
            foreach (var h in hours)
            {
                hourComboBox.Items.Add(h);
            }

            teacherComboBox.Items.Add("");
            foreach (var t in Timetable.GetInstance().TeacherList)
            {
                teacherComboBox.Items.Add(t.Name);
            }
            studentsComboBox.Items.Add("");
            foreach (var s in Timetable.GetStudentsNames())
            {
                studentsComboBox.Items.Add(s);
            }
            subjectComboBox.Items.Add("");
            foreach (var s in Timetable.GetInstance().SubjectList)
            {
                subjectComboBox.Items.Add(s.Name);
            }
            activityTagComboBox.Items.Add("");
            foreach (var a in Timetable.GetInstance().ActivitiyTagsList)
            {
                activityTagComboBox.Items.Add(a.Name);
            }
            this.Title = windowTitle;
        }

        public ActivitiesTimesSlotsItems(List<string> days, List<string> hours, string windowTitle, List<Data.TimeDayHour> selectedSlots,
            string selectedTeacherName, string selectedStudents, string selectedSubject, string selectedActivityTag)
        {
            InitializeComponent();
            foreach (var d in days)
            {
                dayComboBox.Items.Add(d);
            }
            foreach (var h in hours)
            {
                hourComboBox.Items.Add(h);
            }

            foreach (var selectedSlot in selectedSlots)
            {
                listSlots.Items.Add((string)App.Current.TryFindResource("day")+ ": " + selectedSlot.Day +
                    " - " + (string)App.Current.TryFindResource("hour") + ": " + selectedSlot.Hour);
            }

            teacherComboBox.Items.Add("");
            foreach (var t in Timetable.GetInstance().TeacherList)
            {
                teacherComboBox.Items.Add(t.Name);
            }
            teacherComboBox.SelectedItem = selectedTeacherName;

            studentsComboBox.Items.Add("");
            foreach (var s in Timetable.GetStudentsNames())
            {
                studentsComboBox.Items.Add(s);
            }
            studentsComboBox.SelectedItem = selectedStudents;

            subjectComboBox.Items.Add("");
            foreach (var s in Timetable.GetInstance().SubjectList)
            {
                subjectComboBox.Items.Add(s.Name);
            }
            subjectComboBox.SelectedItem = selectedSubject;

            activityTagComboBox.Items.Add("");
            foreach (var a in Timetable.GetInstance().ActivitiyTagsList)
            {
                activityTagComboBox.Items.Add(a.Name);
            }
            activityTagComboBox.SelectedItem = selectedActivityTag;
            this.Title = windowTitle;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listSlots.SelectedItem != null)
            {
                listSlots.Items.Remove(listSlots.SelectedItem.ToString());
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (listSlots.Items.Count > 0)// !acceptEmptyIds &&
            {
                string message = (string)App.Current.TryFindResource("thisConstraintsAcceptOnlyOneTimeSlot");
                MessageBox.Show(message);
                return;
            }

            if (dayComboBox.SelectedItem != null && hourComboBox.SelectedItem != null)
            {
                string item = (string)App.Current.TryFindResource("day") + ": " + dayComboBox.SelectedItem.ToString() +
                    " - " + (string)App.Current.TryFindResource("hour") + ": " + hourComboBox.SelectedItem.ToString();
                listSlots.Items.Add(item);
            }
            else
            {
                string message = (string)App.Current.TryFindResource("pleaseSelectDayAndHour");
                MessageBox.Show(message);
            }
        }

        public string GetTecherName()
        {
            return this.teacherName;
        }
        public string GetStudents()
        {
            return this.students;
        }
        public string GetSubject()
        {
            return this.subject;
        }
        public string GetActivityTag()
        {
            return this.activityTag;
        }
        public List<Data.TimeDayHour> GetSlots()
        {
            return this.slots;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {

            this.activityTag = activityTagComboBox.SelectedItem != null ? activityTagComboBox.SelectedItem.ToString() : "";
            this.teacherName = teacherComboBox.SelectedItem != null ? teacherComboBox.SelectedItem.ToString() : "";
            this.students = studentsComboBox.SelectedItem != null ? studentsComboBox.SelectedItem.ToString() : "";
            this.subject = subjectComboBox.SelectedItem != null ? subjectComboBox.SelectedItem.ToString() : "";

            foreach (var item in listSlots.Items)
            {
                string[] vals = item.ToString().Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                string day = vals[0].Substring(vals[0].IndexOf(":") + 1).Trim();
                string hour = vals[1].Substring(vals[1].IndexOf(":") + 1).Trim() + "-" + vals[2];
                slots.Add(new Data.TimeDayHour(day, hour));
            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            this.Close();
        }
    }
}
