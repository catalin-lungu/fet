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

namespace FET
{
    /// <summary>
    /// Interaction logic for MaxHoursContinuouslyWithAnActivityTagForAllTeachers.xaml
    /// </summary>
    public partial class DataTimeConstraintTeachersActivityTag : Window
    {

        int number;
        string activityTag="";
        public DataTimeConstraintTeachersActivityTag(List<Data.ConstraintActivityTagMaxHoursContinuously> list)
        {
            InitializeComponent();
            foreach (var constr in list)
            {
                listConstraints.Items.Add(constr.ActivityTag +"-"+ constr.MaximumHoursContinuously);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (DataTimeConstraintTeachersActivityTagItem window = new DataTimeConstraintTeachersActivityTagItem(
                Timetable.GetInstance().ActivitiyTagsList, Timetable.GetInstance().HoursList.Count()))
            {
                window.ShowDialog();

                number = window.GetNr();
                activityTag = window.GetActivityTag();
            }

            if (number != -1 && !activityTag.Equals(""))
            {
                //Timetable.GetInstance().TimeContraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList.Add(
                //            new Data.ConstraintTeachersActivityTagMaxHoursContinuously()
                //            {
                //                MaximumHoursContinuously = number,
                //                ActivityTag = activityTag,
                //                Active = true,
                //                WeightPercentage = 100,
                //                Comments = ""
                //            });
                //listConstraints.Items.Add(activityTag + " - " + number);
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

            int newNumber = -1;
            string newActivityTag = null;
            using (DataTimeConstraintTeachersActivityTagItem window = new DataTimeConstraintTeachersActivityTagItem(Timetable.GetInstance().ActivitiyTagsList,
                Timetable.GetInstance().HoursList.Count(), activityTagName, nr))
            {
                window.ShowDialog();

                newNumber = window.GetNr();
                newActivityTag = window.GetActivityTag();
            }
            if (string.IsNullOrEmpty(newActivityTag) || newNumber == -1)
            {
                return;
            }

            //var con = from c in Timetable.GetInstance().TimeContraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList
            //          where c.ActivityTag.Equals(activityTagName) && c.MaximumHoursContinuously == nr
            //          select c;
            //if (con.Count() > 0)
            //{
            //    var constr = con.First();
            //    constr.ActivityTag = newActivityTag;
            //    constr.MaximumHoursContinuously = newNumber;
            //}

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

            //var con = from c in Timetable.GetInstance().TimeContraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList
            //          where c.ActivityTag.Equals(activityTagName) && c.MaximumHoursContinuously == nr
            //          select c;
            //if (con.Count() > 0)
            //{
            //    Timetable.GetInstance().TimeContraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList.Remove(con.First());
            //    listConstraints.Items.Remove(conString);
            //}              
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
