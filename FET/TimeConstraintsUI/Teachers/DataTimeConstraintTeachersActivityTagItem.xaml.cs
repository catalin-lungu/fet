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

namespace FET
{
    /// <summary>
    /// Interaction logic for DataTimeConstraintTeachersActivityTagItem.xaml
    /// </summary>
    public partial class DataTimeConstraintTeachersActivityTagItem : Window, IDisposable
    {
        int nr = -1;
        string tag ;
        public DataTimeConstraintTeachersActivityTagItem(List<Data.ActivityTag> activityTags, int maxNr)
        {
            InitializeComponent();
            for (int i = 0; i <= maxNr; i++)
            {
                numberComboBox.Items.Add(i);
            }
            foreach (var actTag in activityTags)
            {
                activityTagComboBox.Items.Add(actTag.Name);
            }
        }

        public DataTimeConstraintTeachersActivityTagItem(List<Data.ActivityTag> activityTags, int maxNr, string selectedTag, int selectedNr)
        {
            InitializeComponent();
            for (int i = 0; i <= maxNr; i++)
            {
                numberComboBox.Items.Add(i);
            }
            numberComboBox.SelectedItem = selectedNr;
            foreach (var actTag in activityTags)
            {
                activityTagComboBox.Items.Add(actTag.Name);
            }
            activityTagComboBox.SelectedItem = selectedTag;
        }

        public int GetNr()
        {
            return this.nr;
        }
        public string GetActivityTag()
        {
            return this.tag;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.nr = numberComboBox.SelectedItem != null ? (int)numberComboBox.SelectedItem : -1;
            this.tag = activityTagComboBox.SelectedItem != null ? activityTagComboBox.SelectedItem.ToString() : string.Empty;

            if (string.IsNullOrEmpty(tag) || nr == -1)
            {
                MessageBox.Show("Please select a number!");
                return;
            }
            this.Close();
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
