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
    /// Interaction logic for ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem.xaml
    /// </summary>
    public partial class ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem : Window, IDisposable
    {
        List<int> activitiesId = new List<int>();
        public ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem()
        {
            InitializeComponent();
            foreach (var act in Timetable.GetInstance().ActivityList)
            {
                string item = act.ActivityId + " - " + act.Teacher.Name + " - " +
                    act.Students + " - " + act.Subject.Name;
                item += act.ActivityTag != null ? " - " + act.ActivityTag.Name : "";
                activitiesComboBox.Items.Add(item);

            }
        }

        public ActivitiesAreInTheSameRoomIfTheyAreConsecutiveItem(List<Data.Activity> selectedActivities)
        {
            InitializeComponent();
            foreach (var act in Timetable.GetInstance().ActivityList)
            {
                string item = act.ActivityId + " - " + act.Teacher.Name + " - " +
                    act.Students + " - " + act.Subject.Name;
                item += act.ActivityTag != null ? " - " + act.ActivityTag.Name : "";
                activitiesComboBox.Items.Add(item);

            }
            foreach (var selAct in selectedActivities)
            {
                string item = selAct.ActivityId + " - " + selAct.Teacher.Name + " - " +
                    selAct.Students + " - " + selAct.Subject.Name;
                item += selAct.ActivityTag != null ? " - " + selAct.ActivityTag.Name : "";
                activitiesList.Items.Add(item);
                activitiesId.Add(Convert.ToInt32(selAct.ActivityId));
            }
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (activitiesComboBox.SelectedItem != null)
            {
                activitiesList.Items.Add(activitiesComboBox.SelectedItem.ToString());
                var str = activitiesComboBox.SelectedItem.ToString();
                activitiesId.Add(Convert.ToInt32(str.Substring(0, str.IndexOf("-")).Trim()));
            }
        }

        private void RemoveFromList_Click(object sender, RoutedEventArgs e)
        {
            if (activitiesList.SelectedItem != null)
            {
                string str = activitiesList.SelectedItem.ToString();
                int id = Convert.ToInt32(str.Substring(0, str.IndexOf("-")).Trim());
                activitiesId.Remove(id);
                activitiesList.Items.Remove(str);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (activitiesId.Count() < 1)
            {
                string message = (string)App.Current.TryFindResource("pleaseSelectAllDataNeeded");
                MessageBox.Show(message);
                return;
            }
            this.Close();
        }

        public List<int> GetActivitiesIds()
        {
            return activitiesId;
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
