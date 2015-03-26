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
    /// Interaction logic for ActivitiesTimeSlotsItems.xaml
    /// </summary>
    public partial class ActivitiesIdsTimeSlotsItems : Window, IDisposable
    {
        List<int> activitiesIds = new List<int>();
        int maxNumberOfActivities;
        int minNumberOfActivities;

        public ActivitiesIdsTimeSlotsItems(int min, int max, string windowTitle)
        {
            InitializeComponent();
            
            foreach (var act in Timetable.GetInstance().ActivityList)
            {
                activitiesComboBox.Items.Add(act.ActivityId);
            }
            this.maxNumberOfActivities = max;
            this.minNumberOfActivities = min;
            this.Title = windowTitle;
        }

        public ActivitiesIdsTimeSlotsItems(List<int> selectedActivities, int min, int max, string windowTitle)
        {
            InitializeComponent();
            foreach (var act in Timetable.GetInstance().ActivityList)
            {
                activitiesComboBox.Items.Add(act.ActivityId);
            }

            foreach (var act in selectedActivities)
            {
                activitiesList.Items.Add(act);
                activitiesIds.Add(act);
            }

            this.maxNumberOfActivities = max;
            this.minNumberOfActivities = min;
            this.Title = windowTitle;
        }


        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (activitiesIds.Count +1 > maxNumberOfActivities && (maxNumberOfActivities > 0))
            {
                string message = (string)App.Current.TryFindResource("thisTypeOfConstraintCanContainOnlyXActivitiesIds");
                MessageBox.Show(string.Format(message, maxNumberOfActivities));
                return;
            }

            if (activitiesComboBox.SelectedItem != null)
            {
                activitiesList.Items.Add(activitiesComboBox.SelectedItem.ToString());
                var str = activitiesComboBox.SelectedItem.ToString();
                activitiesIds.Add(Convert.ToInt32(str.Trim()));
            }
        }

        private void RemoveFromList_Click(object sender, RoutedEventArgs e)
        {
            if (activitiesList.SelectedItem != null)
            {
                string str = activitiesList.SelectedItem.ToString();
                int id = Convert.ToInt32(str.Trim());
                activitiesIds.Remove(id);
                activitiesList.Items.Remove(id);
               
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (activitiesIds.Count < minNumberOfActivities )
            {
                string message = (string)App.Current.TryFindResource("thisTypeOfConstraintRequiresAMinimumOfActivities");
                MessageBox.Show(string.Format(message, minNumberOfActivities));
                return;
            }
            this.Close();
        }

        public List<int> GetActivityIds()
        {
            return this.activitiesIds;
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
