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
    /// Interaction logic for ActivityTimeSlotsItems.xaml
    /// </summary>
    public partial class ActivityTimeSlotsItems : Window , IDisposable
    {
        int activityId = -1;
        List<Data.TimeDayHour> slots = new List<Data.TimeDayHour>();
        bool acceptMultipleSlots;
        public ActivityTimeSlotsItems(List<string> days, List<string> hours,string windowTitle , bool acceptMultipleSlots = true)
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
            foreach (var act in Timetable.GetInstance().ActivityList)
            {
                activityComboBox.Items.Add(act.ActivityId);
            }

            this.acceptMultipleSlots = acceptMultipleSlots;
            this.Title = windowTitle;
        }

        public ActivityTimeSlotsItems(List<string> days, List<string> hours, string windowTitle, List<Data.TimeDayHour> selectedSlots, int selectedActId, bool acceptMultipleSlots = true)
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
            this.acceptMultipleSlots = acceptMultipleSlots;

            foreach(var selectedSlot in selectedSlots)
            {
                listSlots.Items.Add("Day: " + selectedSlot.Day + " - Hour: " + selectedSlot.Hour);
            }
            foreach (var act in Timetable.GetInstance().ActivityList)
            {
                activityComboBox.Items.Add(act.ActivityId);
            }
            activityComboBox.SelectedItem = selectedActId;
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
            if (!acceptMultipleSlots && listSlots.Items.Count > 0)
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

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (activityComboBox.SelectedItem != null)
            {
                activityId = Convert.ToInt32(activityComboBox.SelectedItem.ToString());
            }
            else
            {
                return;
            }

            foreach (var item in listSlots.Items)
            {
                string[] vals = item.ToString().Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                string day = vals[0].Substring(vals[0].IndexOf(":")+1).Trim();
                string hour = vals[1].Substring(vals[1].IndexOf(":")+1).Trim();
                slots.Add(new Data.TimeDayHour(day, hour));
            }
            this.Close();
        }

        public int GetActivityId()
        {
            return this.activityId;
        }

        public List<Data.TimeDayHour> GetSlots()
        {
            return this.slots;
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
