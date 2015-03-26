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

namespace FET.SpaceConstraintsUI.Subject_and_ActivityTag
{
    /// <summary>
    /// Interaction logic for ActivitiesOccupiesMaxDifferentRooms.xaml
    /// </summary>
    public partial class ActivitiesOccupiesMaxDifferentRooms : Window
    {
        string constraint;
        public ActivitiesOccupiesMaxDifferentRooms(List<ConstraintActivitiesOccupyMaxDifferentRooms> list)
        {
            InitializeComponent();

            foreach (var constr in list)
            {
                var activities = from a in Timetable.GetInstance().ActivityList
                                 where constr.ActivityIds.Contains(a.ActivityId)
                                 select a;
                if (activities.Count() > 0)
                {
                    var act = activities.First();
                    string item = act.ActivityId + " - " + act.Teacher.Name +
                    act.Students + " - " + act.Subject.Name;
                    item += act.ActivityTag != null ? act.ActivityTag.Name : "";
                    item += " - " + (string)App.Current.TryFindResource("activitiesCount") + ": " + constr.ActivityIds.Count();
                    item += " - " + (string)App.Current.TryFindResource("maxDifferentRooms") + ": " + constr.MaxNumberOfDifferentRooms;
                    item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintActivitiesOccupyMaxDifferentRoomsId;
                    listConstraints.Items.Add(item);
                }
            }
            constraint = "ConstraintActivitiesOccupyMaxDifferentRooms";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int number = -1;
            List<int> activitiesIds = new List<int>();

            using (ActivitiesOccupiesMaxDifferentRoomsItem window = new ActivitiesOccupiesMaxDifferentRoomsItem(
                Timetable.GetInstance().ActivityList, Timetable.GetInstance().DaysList.Count()))
            {
                window.ShowDialog();

                number = window.GetNr();
                activitiesIds = window.GetActivitiesIds();
            }

            if (number != -1 && activitiesIds.Count() > 0)
            {
                if (constraint.Equals("ConstraintActivitiesOccupyMaxDifferentRooms"))
                {
                    Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList.Add(
                                new Data.ConstraintActivitiesOccupyMaxDifferentRooms()
                                {
                                    MaxNumberOfDifferentRooms = number,
                                    ActivityIds = activitiesIds
                                });
                    var activities = from a in Timetable.GetInstance().ActivityList
                                     where a.ActivityId == activitiesIds.First()
                                     select a;
                    if (activities.Count() > 0)
                    {
                        var act = activities.First();
                        string item = act.ActivityId + " - " + act.Teacher.Name +
                        act.Students + " - " + act.Subject.Name;
                        item += act.ActivityTag != null ? act.ActivityTag.Name : "";
                        item += " - " + (string)App.Current.TryFindResource("activitiesCount") + ": " + activitiesIds.Count();
                        item += " - " + (string)App.Current.TryFindResource("maxDifferentRooms") + ": " + number;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList.Last().
                            ConstraintActivitiesOccupyMaxDifferentRoomsId;
                        listConstraints.Items.Add(item);
                    }
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
            int nr = 0;
            List<Data.Activity> selectedActs = new List<Data.Activity>();

            if (constraint.Equals("ConstraintActivitiesOccupyMaxDifferentRooms"))
            {
                foreach (var con in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
                {
                    if (con.ConstraintActivitiesOccupyMaxDifferentRoomsId == constrId)
                    {
                        nr = con.MaxNumberOfDifferentRooms;
                        foreach (var id in con.ActivityIds)
                        {
                            var act = from a in Timetable.GetInstance().ActivityList
                                      where a.ActivityId == id
                                      select a;
                            if (act.Count() > 0)
                            {
                                selectedActs.Add(act.First());
                            }
                        }

                        break;
                    }
                }
            }

            int newNumber = -1;
            List<int> newIds = new List<int>();
            using (ActivitiesOccupiesMaxDifferentRoomsItem window = new ActivitiesOccupiesMaxDifferentRoomsItem(
                Timetable.GetInstance().ActivityList, Timetable.GetInstance().DaysList.Count(), selectedActs, nr))
            {
                window.ShowDialog();

                newNumber = window.GetNr();
                newIds = window.GetActivitiesIds();
            }
            if (newIds.Count() < 1 || newNumber == -1)
            {
                return;
            }

            if (constraint.Equals("ConstraintActivitiesOccupyMaxDifferentRooms"))
            {
                var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList
                          where c.ConstraintActivitiesOccupyMaxDifferentRoomsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;
                    constr.MaxNumberOfDifferentRooms = newNumber;

                    listConstraints.Items.Remove(conString);

                    var activities = from a in Timetable.GetInstance().ActivityList
                                     where constr.ActivityIds.Contains(a.ActivityId)
                                     select a;
                    if (activities.Count() > 0)
                    {
                        var act = activities.First();
                        string item = act.ActivityId + " - " + act.Teacher.Name +
                        act.Students + " - " + act.Subject.Name;
                        item += act.ActivityTag != null ? act.ActivityTag.Name : "";
                        item += " - " + (string)App.Current.TryFindResource("activitiesCount") + ": " + constr.ActivityIds.Count();
                        item += " - " + (string)App.Current.TryFindResource("maxDifferentRooms") + ": " + constr.MaxNumberOfDifferentRooms;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintActivitiesOccupyMaxDifferentRoomsId;
                        listConstraints.Items.Add(item);
                    }
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

            if (constraint.Equals("ConstraintActivitiesOccupyMaxDifferentRooms"))
            {
                var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList
                          where c.ConstraintActivitiesOccupyMaxDifferentRoomsId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList.Remove(con.First());
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

            if (constraint.Equals("ConstraintActivitiesOccupyMaxDifferentRooms"))
            {
                foreach (var con in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList)
                {
                    if (con.ConstraintActivitiesOccupyMaxDifferentRoomsId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("maxNumberOfDifferentRooms") + ": " + con.MaxNumberOfDifferentRooms + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        foreach (var id in con.ActivityIds)
                        {
                            var acts = from a in Timetable.GetInstance().ActivityList
                                       where a.ActivityId == id
                                       select a;
                            foreach (var act in acts)
                            {
                                constraintInfo.Text += (string)App.Current.TryFindResource("id") + ": " + act.ActivityId + Environment.NewLine;
                                constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + act.Teacher.Name + Environment.NewLine;
                                constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                                constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + act.Subject + Environment.NewLine;
                                if (act.ActivityTag != null)
                                {
                                    constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + act.ActivityTag.Name + Environment.NewLine;
                                }
                                constraintInfo.Text += "------------------" + Environment.NewLine;
                            }
                        }

                        break;
                    }
                }
            }
        }
   
    }
}
