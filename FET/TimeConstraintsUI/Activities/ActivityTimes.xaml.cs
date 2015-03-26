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
    /// Interaction logic for ActivityTime.xaml
    /// </summary>
    public partial class ActivityTimes : Window
    {
        string constraint;

        public ActivityTimes(List<Data.ConstraintActivityPreferredStartingTime> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("preferredDay") + ": " + con.PreferredDay +
                    " - " + (string)App.Current.TryFindResource("preferredHour") + ": " + con.PreferredHour + 
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivityPreferredStartingTimeId);
            }
            constraint = "ConstraintActivityPreferredStartingTime";
            this.Title = (string)App.Current.TryFindResource("activityPreferredStartingTime");
        }

        public ActivityTimes(List<Data.ConstraintActivityPreferredStartingTimes> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ": " +
                    con.PreferredStartingTimes.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivityPreferredStartingTimesId);
            }
            constraint = "ConstraintActivityPreferredStartingTimes";
            this.Title = (string)App.Current.TryFindResource("activityPreferredStartingTimes");
        }

        public ActivityTimes(List<Data.ConstraintActivityPreferredTimeSlots> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ": " +
                    con.PreferredTimeSlots.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivityPreferredTimeSlotsId);
            }
            constraint = "ConstraintActivityPreferredTimeSlots";
            this.Title = (string)App.Current.TryFindResource("activityPreferredTimeSlots");
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int activityId;
            List<Data.TimeDayHour> slots = new List<Data.TimeDayHour>();

            bool acceptMultipleSlots = true;
            if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
            {
                acceptMultipleSlots = false;
            }

            string windowTitle = DeterminChildWindowTitle(constraint);
            using (ActivityTimeSlotsItems window = new ActivityTimeSlotsItems(
                Timetable.GetInstance().DaysList, Timetable.GetInstance().HoursList, windowTitle, acceptMultipleSlots))
            {
                window.ShowDialog();

                activityId = window.GetActivityId();
                slots = window.GetSlots();
            }

            if (slots.Count() > 0)
            {
                if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
                {
                    var slot = slots.First();
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList.Add(
                                new Data.ConstraintActivityPreferredStartingTime()
                                {
                                    ActivityId = activityId,
                                    PreferredDay = slot.Day,
                                    PreferredHour = slot.Hour
                                });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("preferredDay") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList.Last().PreferredDay +
                        " - " + (string)App.Current.TryFindResource("preferredHour") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList.Last().PreferredHour +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList.Last().ConstraintActivityPreferredStartingTimeId);
                }
                else if (constraint.Equals("ConstraintActivityPreferredStartingTimes"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList.Add(
                                new Data.ConstraintActivityPreferredStartingTimes()
                                {
                                    ActivityId = activityId,
                                    PreferredStartingTimes = slots,
                                    NumberOfPreferredStartingTimes = slots.Count()
                                });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList.Last().PreferredStartingTimes.Count()
                        + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList.Last().ConstraintActivityPreferredStartingTimesId);

                }
                else if (constraint.Equals("ConstraintActivityPreferredTimeSlots"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList.Add(
                                 new Data.ConstraintActivityPreferredTimeSlots()
                                 {
                                     ActivityId = activityId,
                                     PreferredTimeSlots = slots,
                                     NumberOfPreferredTimeSlots = slots.Count()
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList.Last().PreferredTimeSlots.Count()
                        + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList.Last().ConstraintActivityPreferredTimeSlotsId);

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

            int activityId = 0 ;
            List<Data.TimeDayHour> selectedSlots = new List<Data.TimeDayHour>();

            if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList)
                {                    
                    if (con.ConstraintActivityPreferredStartingTimeId == constrId)
                    {
                        selectedSlots.Add(new Data.TimeDayHour(con.PreferredDay, con.PreferredHour));
                        activityId = con.ActivityId;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredStartingTimes"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList)
                {
                    if (con.ConstraintActivityPreferredStartingTimesId == constrId)
                    {
                        selectedSlots = con.PreferredStartingTimes;
                        activityId = con.ActivityId;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredTimeSlots"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList)
                {
                    if (con.ConstraintActivityPreferredTimeSlotsId == constrId)
                    {
                        selectedSlots = con.PreferredTimeSlots;
                        activityId = con.ActivityId;
                        break;
                    }
                }
            }

            bool acceptMultipleSlots = true;
            if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
            {
                acceptMultipleSlots = false;
            }

            string windowTitle = DeterminChildWindowTitle(constraint);

            int newActivityId;
            List<Data.TimeDayHour> newSlots = new List<Data.TimeDayHour>();
            using (ActivityTimeSlotsItems window = new ActivityTimeSlotsItems(
                Timetable.GetInstance().DaysList, Timetable.GetInstance().HoursList, windowTitle, selectedSlots, activityId, acceptMultipleSlots))
            {
                window.ShowDialog();

                newActivityId = window.GetActivityId();
                newSlots = window.GetSlots();
            }
            if (newSlots.Count() < 1)
            {
                return;
            }

            if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList
                          where c.ConstraintActivityPreferredStartingTimeId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    var newSlot = newSlots.First();
                    constr.ActivityId = newActivityId;
                    constr.PreferredDay = newSlot.Day;
                    constr.PreferredHour = newSlot.Hour;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("preferredDay") + ": " + 
                        constr.PreferredDay +
                        " - " + (string)App.Current.TryFindResource("preferredHour") + ": " + 
                        constr.PreferredHour +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        constr.ConstraintActivityPreferredStartingTimeId);

                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredStartingTimes"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList
                          where c.ConstraintActivityPreferredStartingTimesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityId = newActivityId;
                    constr.PreferredStartingTimes = newSlots;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ": " +
                        constr.PreferredStartingTimes.Count() + 
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        constr.ConstraintActivityPreferredStartingTimesId);

                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredTimeSlots"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList
                          where c.ConstraintActivityPreferredTimeSlotsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityId = newActivityId;
                    constr.PreferredTimeSlots = newSlots;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ": " +
                        constr.PreferredTimeSlots.Count() +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        constr.ConstraintActivityPreferredTimeSlotsId);

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

            if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList
                          where c.ConstraintActivityPreferredStartingTimeId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredStartingTimes"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList
                          where c.ConstraintActivityPreferredStartingTimesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredTimeSlots"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList
                          where c.ConstraintActivityPreferredTimeSlotsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList.Remove(con.First());
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

            if (constraint.Equals("ConstraintActivityPreferredStartingTime"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList)
                {
                    if (con.ConstraintActivityPreferredStartingTimeId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("activity") + ": "+ con.ActivityId + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("day") + ": " + con.PreferredDay + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("hour") + ": " + con.PreferredHour + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                       

                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivityPreferredStartingTimes"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList)
                {
                    if (con.ConstraintActivityPreferredStartingTimesId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfPreferredStartingTimes") + ": " + con.PreferredStartingTimes.Count() + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("activity") + ": " + con.ActivityId + Environment.NewLine;
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
            else if (constraint.Equals("ConstraintActivityPreferredTimeSlots"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList)
                {
                    if (con.ConstraintActivityPreferredTimeSlotsId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfPreferredTimeSlots") + ": " + con.PreferredTimeSlots.Count() + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("activity") + ": " + con.ActivityId + Environment.NewLine;
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
        }

        private string DeterminChildWindowTitle(string constraint)
        {
            string title = "";
            switch (constraint)
            {
                case "ConstraintActivityPreferredStartingTime":
                    title = (string)App.Current.TryFindResource("addActivityPreferredStartingTime");
                    break;
                case "ConstraintActivityPreferredStartingTimes":
                    title = (string)App.Current.TryFindResource("addAactivityPreferredStartingTimes");
                    break;
                case "ConstraintActivityPreferredTimeSlots":
                    title = (string)App.Current.TryFindResource("addAactivityPreferredTimeSlots");
                    break;
            }

            return title;
        }
    }
}
