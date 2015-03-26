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

namespace FET.TimeConstraintsUI.Students
{
    /// <summary>
    /// Interaction logic for MaxHoursIntervalStudents.xaml
    /// </summary>
    public partial class MaxHoursActivityTagStudents : Window
    {
        int number;
        string activityTag = "";
        string constraint;
        public MaxHoursActivityTagStudents(List<Data.ConstraintActivityTagMaxHoursDaily> list)
        {
            InitializeComponent();
            foreach (var constr in list)
            {
                listConstraints.Items.Add(constr.ActivityTag + "-" + constr.MaximumHoursDaily);
            }
            constraint = "ConstraintStudentsActivityTagMaxHoursDaily";
        }

        public MaxHoursActivityTagStudents(List<Data.ConstraintActivityTagMaxHoursContinuously> list)
        {
            InitializeComponent();
            foreach (var constr in list)
            {
                listConstraints.Items.Add(constr.ActivityTag + "-" + constr.MaximumHoursContinuously);
            }
            constraint = "ConstraintStudentsActivityTagMaxHoursContinuously";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string nrLabel = "";
            switch (constraint)
            {
                case "ConstraintStudentsActivityTagMaxHoursDaily":
                    nrLabel = "Max hours daily:";
                    break;
                case "ConstraintStudentsActivityTagMaxHoursContinuously":
                    nrLabel = "Max hours continuously:";
                    break;
            }

            using (MaxHoursActivityTagStudentsItem window = new MaxHoursActivityTagStudentsItem(
                Timetable.GetInstance().ActivitiyTagsList, Timetable.GetInstance().HoursList.Count(),nrLabel))
            {
                window.ShowDialog();

                number = window.GetNr();
                activityTag = window.GetActivityTag();
            }

            if (number != -1 && !activityTag.Equals("ConstraintStudentsActivityTagMaxHoursDaily"))
            {
                if (constraint.Equals("ConstraintStudentsActivityTagMaxHoursDaily"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList.Add(
                                new Data.ConstraintActivityTagMaxHoursDaily()
                                {
                                    MaximumHoursDaily = number,
                                    ActivityTag = activityTag                                    
                                });
                }
                else if (constraint.Equals("ConstraintStudentsActivityTagMaxHoursContinuously"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList.Add(
                                                   new Data.ConstraintActivityTagMaxHoursContinuously()
                                                   {
                                                       MaximumHoursContinuously = number,
                                                       ActivityTag = activityTag
                                                   });
                }
                listConstraints.Items.Add(activityTag + " - " + number);
            }

        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }
            string activityTagName = conString.Substring(0, conString.IndexOf("-")).Trim();
            int nr = Convert.ToInt32(conString.Substring(conString.IndexOf("-") + 1).Trim());

            string nrLabel = "";
            switch (constraint)
            {
                case "ConstraintStudentsActivityTagMaxHoursDaily":
                    nrLabel = "Max hours daily:";
                    break;
                case "ConstraintStudentsActivityTagMaxHoursContinuously":
                    nrLabel = "Max hours continuously:";
                    break;
            }

            int newNumber = -1;
            string newActivityTag = null;
            using (MaxHoursActivityTagStudentsItem window = new MaxHoursActivityTagStudentsItem(Timetable.GetInstance().ActivitiyTagsList,
                Timetable.GetInstance().HoursList.Count(),nrLabel, activityTagName, nr))
            {
                window.ShowDialog();

                newNumber = window.GetNr();
                newActivityTag = window.GetActivityTag();
            }
            if (string.IsNullOrEmpty(newActivityTag) || newNumber == -1)
            {
                return;
            }

            if (constraint.Equals("ConstraintStudentsActivityTagMaxHoursDaily"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList
                          where c.ActivityTag.Equals(activityTagName) && c.MaximumHoursDaily == nr
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityTag = newActivityTag;
                    constr.MaximumHoursDaily = newNumber;
                }
            }
            else if (constraint.Equals("ConstraintStudentsActivityTagMaxHoursContinuously"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList
                          where c.ActivityTag.Equals(activityTagName) && c.MaximumHoursContinuously == nr
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityTag = newActivityTag;
                    constr.MaximumHoursContinuously = newNumber;
                }
            }
            listConstraints.Items.Remove(conString);
            listConstraints.Items.Add(newActivityTag + " - " + newNumber);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }

            string activityTagName = conString.Substring(0, conString.IndexOf("-")).Trim();
            int nr = Convert.ToInt32(conString.Substring(conString.IndexOf("-") + 1).Trim());

            if (constraint.Equals("ConstraintStudentsActivityTagMaxHoursDaily"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList
                          where c.ActivityTag.Equals(activityTagName) && c.MaximumHoursDaily == nr
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintStudentsActivityTagMaxHoursContinuously"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList
                          where c.ActivityTag.Equals(activityTagName) && c.MaximumHoursContinuously == nr
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
