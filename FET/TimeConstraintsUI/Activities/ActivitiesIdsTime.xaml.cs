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
    /// Interaction logic for ActivitiesIdsTime.xaml
    /// </summary>
    public partial class ActivitiesIdsTime : Window
    {
        string constraint;
        public ActivitiesIdsTime(List<Data.ConstraintActivitiesSameStartingTime> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {

                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " + con.ActivityIds.Count() + 
                    " - "+(string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivitiesSameStartingTimeId);
            }
            constraint = "ConstraintActivitiesSameStartingTime";
            this.Title = (string)App.Current.TryFindResource("activitiesSameStartingTime");
        }

        public ActivitiesIdsTime(List<Data.ConstraintActivitiesSameStartingDay> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " + con.ActivityIds.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivitiesSameStartingDayId);
            }
            constraint = "ConstraintActivitiesSameStartingDay";
            this.Title = (string)App.Current.TryFindResource("activitiesSameStartingDay");
        }

        public ActivitiesIdsTime(List<Data.ConstraintActivitiesSameStartingHour> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ":" + con.ActivityIds.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivitiesSameStartingHourId);
            }
            constraint = "ConstraintActivitiesSameStartingHour";
            this.Title = (string)App.Current.TryFindResource("activitiesSameStartingHour");
        }


        public ActivitiesIdsTime(List<Data.ConstraintActivityEndsStudentsDay> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("activityId") + ": " + con.ActivityId + 
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivityEndsStudentsDayId);
            }
            constraint = "ConstraintActivityEndsStudentsDay";
            this.Title = (string)App.Current.TryFindResource("activityEndsStudentsDay");
        }

        public ActivitiesIdsTime(List<Data.ConstraintTwoActivitiesOrdered> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + con.ActivityIds.ElementAt(0) +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ActivityIds.ElementAt(1) +
                    " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + con.ConstraintTwoActivitiesOrderedId);
            }
            constraint = "ConstraintTwoActivitiesOrdered";
            this.Title = (string)App.Current.TryFindResource("twoActivitiesOrdered");
        }

        public ActivitiesIdsTime(List<Data.ConstraintTwoActivitiesConsecutive> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + con.FirstActivityId +
                    " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + con.SecondActivityId +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintTwoActivitiesConsecutiveId);
            }
            constraint = "ConstraintTwoActivitiesConsecutive";
            this.Title = (string)App.Current.TryFindResource("twoActivitiesConsecutive");
        }

        public ActivitiesIdsTime(List<Data.ConstraintTwoActivitiesGrouped> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + con.ActivityIds.ElementAt(0) +
                    " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + con.ActivityIds.ElementAt(1) +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintTwoActivitiesGroupedId);
            }
            constraint = "ConstraintTwoActivitiesGrouped";
            this.Title = (string)App.Current.TryFindResource("twoActivitiesGrouped");
        }

        public ActivitiesIdsTime(List<Data.ConstraintThreeActivitiesGrouped> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + con.ActivityIds.ElementAt(0) +
                    " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + con.ActivityIds.ElementAt(1) +
                    " - " + (string)App.Current.TryFindResource("thirdActivity") + ": " + con.ActivityIds.ElementAt(2) +
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintThreeActivitiesGroupedId);
            }
            constraint = "ConstraintThreeActivitiesGrouped";
            this.Title = (string)App.Current.TryFindResource("threeActivitiesGrouped");
        }

        public ActivitiesIdsTime(List<Data.ConstraintActivitiesNotOverlapping> list)
        {
            InitializeComponent();
            foreach (var con in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " + con.ActivityIds.Count + 
                    " - " + (string)App.Current.TryFindResource("id") + ": " + con.ConstraintActivitiesNotOverlappingId);
            }
            constraint = "ConstraintActivitiesNotOverlapping";
            this.Title = (string)App.Current.TryFindResource("activitiesNotOverlapping");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            int minActivities = 0;
            int maxActivities = -1;

            if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
            {
                minActivities = 1;
                maxActivities = 1;
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered") ||
                constraint.Equals("ConstraintTwoActivitiesConsecutive") ||
                constraint.Equals("ConstraintTwoActivitiesGrouped"))
            {
                minActivities = 2;
                maxActivities = 2;
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
            {
                minActivities = 3;
                maxActivities = 3;
            }

            string windowTitle = DeterminChildWindowTitle(constraint);
            using (ActivitiesIdsTimeSlotsItems window = new ActivitiesIdsTimeSlotsItems(minActivities,maxActivities, windowTitle))
            {
                window.ShowDialog();

                ids = window.GetActivityIds();
            }

            if (ids.Count() > 0)
            {
                if (constraint.Equals("ConstraintActivitiesSameStartingTime"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList.Add(
                                new Data.ConstraintActivitiesSameStartingTime()
                                {
                                    ActivityIds = ids,
                                    NumberOfActivities = ids.Count
                                });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList.Last().ActivityIds.Count()
                        + " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList.Last().ConstraintActivitiesSameStartingTimeId);
                }
                else if (constraint.Equals("ConstraintActivitiesSameStartingDay"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList.Add(
                                new Data.ConstraintActivitiesSameStartingDay()
                                {
                                    ActivityIds = ids,
                                    NumberOfActivities = ids.Count
                                });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " +
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList.Last().ActivityIds.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " +
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList.Last().ConstraintActivitiesSameStartingDayId);

                }
                else if (constraint.Equals("ConstraintActivitiesSameStartingHour"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList.Add(
                                 new Data.ConstraintActivitiesSameStartingHour()
                                 {
                                     ActivityIds = ids,
                                     NumberOfActivities = ids.Count
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ":" +
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList.Last().ActivityIds.Count() +
                    " - " + (string)App.Current.TryFindResource("id") + ": " +
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList.Last().ConstraintActivitiesSameStartingHourId);

                }
                else if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList.Add(
                                 new Data.ConstraintActivityEndsStudentsDay()
                                 {
                                     ActivityId = ids.First()
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("activityId") + " : " +
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList.Last().ActivityId +
                    " - " + (string)App.Current.TryFindResource("id") + ": " +
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList.Last().ConstraintActivityEndsStudentsDayId);

                }
                else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList.Add(
                                 new Data.ConstraintTwoActivitiesOrdered()
                                 {
                                     ActivityIds = ids
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList.Last().ActivityIds.ElementAt(0) +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " +  
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList.Last().ActivityIds.ElementAt(1) +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList.Last().ConstraintTwoActivitiesOrderedId);

                }
                else if (constraint.Equals("ConstraintTwoActivitiesConsecutive"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Add(
                                 new Data.ConstraintTwoActivitiesConsecutive()
                                 {
                                     FirstActivityId = ids.ElementAt(0),
                                     SecondActivityId = ids.ElementAt(1)
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Last().FirstActivityId +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Last().SecondActivityId +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Last().ConstraintTwoActivitiesConsecutiveId);

                }
                else if (constraint.Equals("ConstraintTwoActivitiesGrouped"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList.Add(
                                 new Data.ConstraintTwoActivitiesGrouped()
                                 {
                                     ActivityIds = ids
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList.Last().ActivityIds.ElementAt(0) +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList.Last().ActivityIds.ElementAt(1) +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList.Last().ConstraintTwoActivitiesGroupedId);

                }
                else if (constraint.Equals("ConstraintThreeActivitiesGrouped"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Add(
                                 new Data.ConstraintThreeActivitiesGrouped()
                                 {
                                     ActivityIds = ids
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Last().ActivityIds.ElementAt(0) +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Last().ActivityIds.ElementAt(1) +
                        " - " + (string)App.Current.TryFindResource("thirdActivity") + ": " +
                        Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Last().ActivityIds.ElementAt(2) +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Last().ConstraintThreeActivitiesGroupedId);

                }
                else if (constraint.Equals("ConstraintActivitiesNotOverlapping"))
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList.Add(
                                 new Data.ConstraintActivitiesNotOverlapping()
                                 {
                                     ActivityIds = ids
                                 });
                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList.Last().ActivityIds.Count +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList.Last().ConstraintActivitiesNotOverlappingId);

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

            
            List<int> selectedIds = new List<int>();

            if (constraint.Equals("ConstraintActivitiesSameStartingTime"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList)
                {
                    if (con.ConstraintActivitiesSameStartingTimeId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingDay"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList)
                {
                    if (con.ConstraintActivitiesSameStartingDayId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingHour"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList)
                {
                    if (con.ConstraintActivitiesSameStartingHourId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList)
                {
                    if (con.ConstraintActivityEndsStudentsDayId == constrId)
                    {
                        selectedIds.Add(con.ActivityId);
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList)
                {
                    if (con.ConstraintTwoActivitiesOrderedId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesConsecutive"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList)
                {
                    if (con.ConstraintTwoActivitiesConsecutiveId == constrId)
                    {
                        selectedIds.Add(con.FirstActivityId);
                        selectedIds.Add(con.SecondActivityId);
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesGrouped"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList)
                {
                    if (con.ConstraintTwoActivitiesGroupedId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintThreeActivitiesGrouped"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList)
                {
                    if (con.ConstraintThreeActivitiesGroupedId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesNotOverlapping"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList)
                {
                    if (con.ConstraintActivitiesNotOverlappingId == constrId)
                    {
                        selectedIds = con.ActivityIds;
                        break;
                    }
                }
            }

            int minActivities = 0;
            int maxActivities = -1;

            if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
            {
                minActivities = 1;
                maxActivities = 1;
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered") ||
                constraint.Equals("ConstraintTwoActivitiesConsecutive") ||
                constraint.Equals("ConstraintTwoActivitiesGrouped"))
            {
                minActivities = 2;
                maxActivities = 2;
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
            {
                minActivities = 3;
                maxActivities = 3;
            }

            string windowTitle = DeterminChildWindowTitle(constraint);
            List<int> newIds = new List<int>();
            using (ActivitiesIdsTimeSlotsItems window = new ActivitiesIdsTimeSlotsItems(selectedIds, minActivities, maxActivities, windowTitle))
            {
                window.ShowDialog();

                newIds = window.GetActivityIds();
            }
            if (newIds.Count() < 1)
            {
                return;
            }

            if (constraint.Equals("ConstraintActivitiesSameStartingTime"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList
                          where c.ConstraintActivitiesSameStartingTimeId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + 
                        ": " + constr.ActivityIds.Count() +
                        " - " + (string)App.Current.TryFindResource("id") + 
                        ": " + constr.ConstraintActivitiesSameStartingTimeId);

                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingDay"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList
                          where c.ConstraintActivitiesSameStartingDayId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " +
                        constr.ActivityIds.Count() + 
                        " - " + (string)App.Current.TryFindResource("id") + ": " +
                        constr.ConstraintActivitiesSameStartingDayId);

                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingHour"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList
                          where c.ConstraintActivitiesSameStartingHourId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " +
                        constr.ActivityIds.Count() + 
                        " - " + (string)App.Current.TryFindResource("id") + ": " +
                        constr.ConstraintActivitiesSameStartingHourId);

                }
            }
            else if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList
                          where c.ConstraintActivityEndsStudentsDayId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityId = newIds.First();

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("activityId") + " : " +
                        constr.ActivityId + " - " + 
                        (string)App.Current.TryFindResource("id") + ": " +
                        constr.ConstraintActivityEndsStudentsDayId);

                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList
                          where c.ConstraintTwoActivitiesOrderedId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + 
                        constr.ActivityIds.ElementAt(0) +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + 
                        constr.ActivityIds.ElementAt(1) +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + 
                        constr.ConstraintTwoActivitiesOrderedId);

                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesConsecutive"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList
                          where c.ConstraintTwoActivitiesConsecutiveId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.FirstActivityId = newIds.ElementAt(0);
                    constr.SecondActivityId = newIds.ElementAt(1);

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + constr.FirstActivityId +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + constr.SecondActivityId +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + constr.ConstraintTwoActivitiesConsecutiveId);

                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesGrouped"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList
                          where c.ConstraintTwoActivitiesGroupedId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + constr.ActivityIds.ElementAt(0) +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + constr.ActivityIds.ElementAt(1) +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + constr.ConstraintTwoActivitiesGroupedId);

                }
            }
            else if (constraint.Equals("ConstraintThreeActivitiesGrouped"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList
                          where c.ConstraintThreeActivitiesGroupedId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("firstActivity") + ": " + constr.ActivityIds.ElementAt(0) +
                        " - " + (string)App.Current.TryFindResource("secondActivity") + ": " + constr.ActivityIds.ElementAt(1) +
                        " - " + (string)App.Current.TryFindResource("thirdActivity") + ": " + constr.ActivityIds.ElementAt(2) +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + constr.ConstraintThreeActivitiesGroupedId);

                }
            }
            else if (constraint.Equals("ConstraintActivitiesNotOverlapping"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList
                          where c.ConstraintActivitiesNotOverlappingId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    var constr = con.First();
                    constr.ActivityIds = newIds;

                    listConstraints.Items.Remove(conString);

                    listConstraints.Items.Add((string)App.Current.TryFindResource("numberOfActivities") + ": " + constr.ActivityIds.Count +
                        " - " + (string)App.Current.TryFindResource("id") + ": " + constr.ConstraintActivitiesNotOverlappingId);

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

            if (constraint.Equals("ConstraintActivitiesSameStartingTime"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList
                          where c.ConstraintActivitiesSameStartingTimeId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingDay"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList
                          where c.ConstraintActivitiesSameStartingDayId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingHour"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList
                          where c.ConstraintActivitiesSameStartingHourId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList
                          where c.ConstraintActivityEndsStudentsDayId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList
                          where c.ConstraintTwoActivitiesOrderedId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesConsecutive"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList
                          where c.ConstraintTwoActivitiesConsecutiveId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesGrouped"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList
                          where c.ConstraintTwoActivitiesGroupedId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintThreeActivitiesGrouped"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList
                          where c.ConstraintThreeActivitiesGroupedId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList.Remove(con.First());
                    listConstraints.Items.Remove(conString);
                }
            }
            else if (constraint.Equals("ConstraintActivitiesNotOverlapping"))
            {
                var con = from c in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList
                          where c.ConstraintActivitiesNotOverlappingId == constrId
                          select c;
                if (con.Count() > 0)
                {
                    Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList.Remove(con.First());
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

            if (constraint.Equals("ConstraintActivitiesSameStartingTime"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList)
                {
                    if (con.ConstraintActivitiesSameStartingTimeId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfActivities") + " " + con.ActivityIds.Count + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.Contains(a.ActivityId)
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingDay"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList)
                {
                    if (con.ConstraintActivitiesSameStartingDayId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfActivities") + " " + con.ActivityIds.Count + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.Contains(a.ActivityId)
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesSameStartingHour"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList)
                {
                    if (con.ConstraintActivitiesSameStartingHourId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfActivities") + " " + con.ActivityIds.Count + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.Contains(a.ActivityId)
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivityEndsStudentsDay"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList)
                {
                    if (con.ConstraintActivityEndsStudentsDayId == constrId)
                    {
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where a.ActivityId == con.ActivityId
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityId") + ": " + act.ActivityId + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesOrdered"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList)
                {
                    if (con.ConstraintTwoActivitiesOrderedId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("firstActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(0) == a.ActivityId
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        constraintInfo.Text += (string)App.Current.TryFindResource("secondActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var actsSec = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(1) == a.ActivityId
                                   select a;
                        foreach (var act in actsSec)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesConsecutive"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList)
                {
                    if (con.ConstraintTwoActivitiesConsecutiveId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("firstActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where a.ActivityId == con.FirstActivityId
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        constraintInfo.Text += (string)App.Current.TryFindResource("secondActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var actsSec = from a in Timetable.GetInstance().ActivityList
                                   where a.ActivityId == con.SecondActivityId
                                   select a;
                        foreach (var act in actsSec)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintTwoActivitiesGrouped"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList)
                {
                    if (con.ConstraintTwoActivitiesGroupedId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("firstActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(0)== a.ActivityId
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        constraintInfo.Text += (string)App.Current.TryFindResource("secondActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var actsSec = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(1) == a.ActivityId
                                   select a;
                        foreach (var act in actsSec)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintThreeActivitiesGrouped"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList)
                {
                    if (con.ConstraintThreeActivitiesGroupedId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("firstActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(0) == a.ActivityId
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        constraintInfo.Text += (string)App.Current.TryFindResource("secondActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var actsSec = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(1) == a.ActivityId
                                   select a;
                        foreach (var act in actsSec)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }

                        constraintInfo.Text += (string)App.Current.TryFindResource("thirdActivity") + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var actsTh = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.ElementAt(2) == a.ActivityId
                                   select a;
                        foreach (var act in actsTh)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
            else if (constraint.Equals("ConstraintActivitiesNotOverlapping"))
            {
                foreach (var con in Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList)
                {
                    if (con.ConstraintActivitiesNotOverlappingId == constrId)
                    {
                        constraintInfo.Text += (string)App.Current.TryFindResource("numberOfActivities") + " " + con.ActivityIds.Count + Environment.NewLine;
                        constraintInfo.Text += "------------------" + Environment.NewLine;
                        var acts = from a in Timetable.GetInstance().ActivityList
                                   where con.ActivityIds.Contains(a.ActivityId)
                                   select a;
                        foreach (var act in acts)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + (act.Subject != null ? act.Subject.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("teacher") + ": " + (act.Teacher != null ? act.Teacher.Name : "") + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("students") + ": " + act.Students + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + (act.ActivityTag != null ? act.ActivityTag.Name : "") + Environment.NewLine;
                            constraintInfo.Text += "------------------" + Environment.NewLine;
                        }
                        break;
                    }
                }
            }
        }

        private string DeterminChildWindowTitle(String parentWindowConstraintType)
        {
            string title = "";
            switch (parentWindowConstraintType) 
            {
                case "ConstraintActivitiesSameStartingTime":
                    title = (string)App.Current.TryFindResource("addActivitiesSameStartingTime");
                    break;
                case "ConstraintActivitiesSameStartingDay":
                    title = (string)App.Current.TryFindResource("addActivitiesSameStartingDay");
                    break;
                case "ConstraintActivitiesSameStartingHour":
                    title = (string)App.Current.TryFindResource("addActivitiesSameStartingHour");
                    break;
                case "ConstraintActivityEndsStudentsDay":
                    title = (string)App.Current.TryFindResource("addActivityEndsStudentsDay");
                    break;
                case "ConstraintTwoActivitiesOrdered":
                    title = (string)App.Current.TryFindResource("addTwoActivitiesOrdered");
                    break;
                case "ConstraintTwoActivitiesConsecutive":
                    title = (string)App.Current.TryFindResource("addTwoActivitiesConsecutive");
                    break;
                case "ConstraintTwoActivitiesGrouped":
                    title = (string)App.Current.TryFindResource("addTwoActivitiesGrouped");
                    break;
                case "ConstraintThreeActivitiesGrouped":
                    title = (string)App.Current.TryFindResource("addThreeActivitiesGrouped");
                    break;
                case "ConstraintActivitiesNotOverlapping":
                    title = (string)App.Current.TryFindResource("addActivitiesNotOverlapping");
                    break;
            }

            return title;
        }
    }
}
