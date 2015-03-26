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

namespace FET.SpaceConstraintsUI.Activity
{
    /// <summary>
    /// Interaction logic for ActivitiesAreInTheSameRoomIfTheyAreConsecutive.xaml
    /// </summary>
    public partial class ActivitiesAreInTheSameRoomIfTheyAreConsecutive : Window
    {
        string constraint;
        public ActivitiesAreInTheSameRoomIfTheyAreConsecutive(List<ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive> list)
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
                    item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId;
                    listConstraints.Items.Add(item);
                }
            }
            constraint = "ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            List<int> activitiesIds = new List<int>();

            using (ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem window = new ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem())
            {
                window.ShowDialog();

                activitiesIds = window.GetActivitiesIds();
            }

            if (activitiesIds.Count() > 0)
            {
                if (constraint.Equals("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive"))
                {
                    Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList.Add(
                                new Data.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive()
                                {
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
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList.Last().
                            ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId;
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
            
            List<Data.Activity> selectedActs = new List<Data.Activity>();

            if (constraint.Equals("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive"))
            {
                foreach (var con in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList)
                {
                    if (con.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId == constrId)
                    {
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

            List<int> newIds = new List<int>();
            using (ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem window = new ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem(selectedActs))
            {
                window.ShowDialog();

                newIds = window.GetActivitiesIds();
            }
            if (newIds.Count() < 1)
            {
                return;
            }

            if (constraint.Equals("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive"))
            {
                var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList
                          where c.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

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
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId;
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

            if (constraint.Equals("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive"))
            {
                var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList
                          where c.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList.Remove(con.First());
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

            if (constraint.Equals("ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive"))
            {
                foreach (var con in Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList)
                {
                    if (con.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfActivities") + ": " + con.ActivityIds.Count + Environment.NewLine;
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
