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
    /// Interaction logic for ActivitiesNumberConstraint.xaml
    /// </summary>
    public partial class ActivitiesNumberConstraint : Window
    {
        string constraint;
        public ActivitiesNumberConstraint(List<Data.ConstraintMinDaysBetweenActivities> list)
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
                    item += " - " + (string)App.Current.TryFindResource("minDays") + ": " + constr.MinDays;
                    item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintMinDaysBetweenActivitiesId;
                    listConstraints.Items.Add(item);
                }
            }
            constraint = "ConstraintMinDaysBetweenActivities";
            this.Title = (string)App.Current.TryFindResource("minDaysBetweenActivities");
        }
        public ActivitiesNumberConstraint(List<Data.ConstraintMinGapsBetweenActivities> list)
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
                    item += " - " + (string)App.Current.TryFindResource("minGaps") + ": " + constr.MinGaps;
                    item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintMinGapsBetweenActivitiesId;
                    listConstraints.Items.Add(item);
                }
            }
            constraint = "ConstraintMinGapsBetweenActivities";
            this.Title = (string)App.Current.TryFindResource("minGapsBetweenActivities");
        }
        public ActivitiesNumberConstraint(List<Data.ConstraintMaxDaysBetweenActivities> list)
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
                    item += " - " + (string)App.Current.TryFindResource("maxDays") + ": " + constr.MaxDays;
                    item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintMaxDaysBetweenActivitiesId;
                    listConstraints.Items.Add(item);
                }
            }
            constraint = "ConstraintMaxDaysBetweenActivities";
            this.Title = (string)App.Current.TryFindResource("maxDaysBetweenActivities");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int number=-1;
            List<int> activitiesIds = new List<int>();

            string nrLabel = "";
            string windowTitle = "";
            switch (constraint)
            {
                case "ConstraintMinDaysBetweenActivities":
                    nrLabel = (string)App.Current.TryFindResource("minDaysBetweenActivities") + ":";
                    windowTitle = (string)App.Current.TryFindResource("addMinDaysBetweenActivities");
                    break;
                case "ConstraintMinGapsBetweenActivities":
                    nrLabel = (string)App.Current.TryFindResource("minGapsBetweenActivities") + ":";
                    windowTitle = (string)App.Current.TryFindResource("addMinGapsBetweenActivities");
                    break;
                case "ConstraintMaxDaysBetweenActivities":
                    nrLabel = (string)App.Current.TryFindResource("maxDaysBetweenActivities") + ":";
                    windowTitle = (string)App.Current.TryFindResource("addMaxDaysBetweenActivities");
                    break;
            }

            using (ActivitiesNumberConstraintItem window = new ActivitiesNumberConstraintItem(
                Timetable.GetInstance().ActivityList, Timetable.GetInstance().DaysList.Count(),nrLabel, windowTitle))
            {
                window.ShowDialog();

                number = window.GetNr();
                activitiesIds = window.GetActivitiesIds();
            }                      

            if (number != -1 && activitiesIds.Count()>0)
            {
                if (constraint.Equals("ConstraintMinDaysBetweenActivities"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList.Add(
                                new Data.ConstraintMinDaysBetweenActivities()
                                {
                                    MinDays = number,
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
                        item += " - " + (string)App.Current.TryFindResource("minDays") + ": " + number;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList.Last().
                            ConstraintMinDaysBetweenActivitiesId;
                        listConstraints.Items.Add(item);
                    }
                }
                else if (constraint.Equals("ConstraintMinGapsBetweenActivities"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList.Add(
                                   new Data.ConstraintMinGapsBetweenActivities()
                                   {
                                       MinGaps = number,
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
                        item += " - " + (string)App.Current.TryFindResource("minGaps") + ": " + number;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList.Last().
                            ConstraintMinGapsBetweenActivitiesId;
                        listConstraints.Items.Add(item);
                    }
                }
                else if (constraint.Equals("ConstraintMaxDaysBetweenActivities"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList.Add(
                                new Data.ConstraintMaxDaysBetweenActivities()
                                {
                                    MaxDays = number,
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
                        item += " - " + (string)App.Current.TryFindResource("maxDays") + ": " + number;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList.Last().
                            ConstraintMaxDaysBetweenActivitiesId;
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
            int nr=0;
            List<Data.Activity> selectedActs = new List<Data.Activity>();

            if (constraint.Equals("ConstraintMinDaysBetweenActivities"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList)
                {
                    if (con.ConstraintMinDaysBetweenActivitiesId == constrId)
                    {
                        nr = con.MinDays;
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
            else if (constraint.Equals("ConstraintMinGapsBetweenActivities"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList)
                {
                    if (con.ConstraintMinGapsBetweenActivitiesId == constrId)
                    {
                        nr = con.MinGaps;
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
            else if (constraint.Equals("ConstraintMaxDaysBetweenActivities"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList)
                {
                    if (con.ConstraintMaxDaysBetweenActivitiesId == constrId)
                    {
                        nr = con.MaxDays;
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


            string nrLabel = "";
            string windowTitle = "";
            switch (constraint)
            {
                case "ConstraintMinDaysBetweenActivities":
                    nrLabel = (string)App.Current.TryFindResource("minDaysBetweenActivities") + ":";
                    windowTitle = (string)App.Current.TryFindResource("addMinDaysBetweenActivities");
                    break;
                case "ConstraintMinGapsBetweenActivities":
                    nrLabel = (string)App.Current.TryFindResource("minGapsBetweenActivities") + ":";
                    windowTitle = (string)App.Current.TryFindResource("addMinGapsBetweenActivities");
                    break;
                case "ConstraintMaxDaysBetweenActivities":
                    nrLabel = (string)App.Current.TryFindResource("maxDaysBetweenActivities") + ":";
                    windowTitle = (string)App.Current.TryFindResource("addMaxDaysBetweenActivities");
                    break;
            }

            int newNumber = -1;
            List<int> newIds = new List<int>();
            using (ActivitiesNumberConstraintItem window = new ActivitiesNumberConstraintItem(
                Timetable.GetInstance().ActivityList, Timetable.GetInstance().DaysList.Count(), selectedActs, nr, nrLabel, windowTitle))
            {
                window.ShowDialog();

                newNumber = window.GetNr();
                newIds = window.GetActivitiesIds();
            }
            if ( newIds.Count()<1 || newNumber == -1)
            {
                return;
            }

            if (constraint.Equals("ConstraintMinDaysBetweenActivities"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList
                          where c.ConstraintMinDaysBetweenActivitiesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;
                    constr.MinDays = newNumber;

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
                        item += " - " + (string)App.Current.TryFindResource("minDays") + ": " + constr.MinDays;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintMinDaysBetweenActivitiesId;
                        listConstraints.Items.Add(item);
                    }
                }
            }
            else if (constraint.Equals("ConstraintMinGapsBetweenActivities"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList
                          where c.ConstraintMinGapsBetweenActivitiesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;
                    constr.MinGaps = newNumber;

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
                        item += " - " + (string)App.Current.TryFindResource("minGaps") + ": " + constr.MinGaps;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintMinGapsBetweenActivitiesId;
                        listConstraints.Items.Add(item);
                    }
                }
            }
            else if (constraint.Equals("ConstraintMaxDaysBetweenActivities"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList
                          where c.ConstraintMaxDaysBetweenActivitiesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;
                    constr.MaxDays = newNumber;

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
                        item += " - " + (string)App.Current.TryFindResource("maxDays") + ": " + constr.MaxDays;
                        item += " - " + (string)App.Current.TryFindResource("constraintId") + ": " + constr.ConstraintMaxDaysBetweenActivitiesId;
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

            if (constraint.Equals("ConstraintMinDaysBetweenActivities"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList
                          where c.ConstraintMinDaysBetweenActivitiesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintMinGapsBetweenActivities"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList
                          where c.ConstraintMinGapsBetweenActivitiesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintMaxDaysBetweenActivities"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList
                          where c.ConstraintMaxDaysBetweenActivitiesId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList.Remove(con.First());
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

            if (constraint.Equals("ConstraintMinDaysBetweenActivities"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList)
                {
                    if (con.ConstraintMinDaysBetweenActivitiesId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfMinDays") + ": " + con.MinDays + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        foreach (var id in con.ActivityIds)
                        {
                            var acts = from a in Timetable.GetInstance().ActivityList
                                      where a.ActivityId == id
                                      select a;
                            foreach(var act in acts)
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
            else if (constraint.Equals("ConstraintMinGapsBetweenActivities"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList)
                {
                    if (con.ConstraintMinGapsBetweenActivitiesId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfMinGaps") + ": " + con.MinGaps + Environment.NewLine;
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
            else if (constraint.Equals("ConstraintMaxDaysBetweenActivities"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList)
                {
                    if (con.ConstraintMaxDaysBetweenActivitiesId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfMaxDays") + ": " + con.MaxDays + Environment.NewLine;
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
