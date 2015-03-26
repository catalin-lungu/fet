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
using FET.ViewModel;

namespace FET
{
    /// <summary>
    /// Interaction logic for DataActivities.xaml
    /// </summary>
    public partial class DataActivities : Window
    {
        public DataActivities()
        {
            InitializeComponent();
            this.DataContext = new ActivitiesViewModel();
            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            teacherComboBox.SelectedValue = "";
            studentsComboBox.SelectedValue = "";
            subjectsComboBox.SelectedValue = "";
            activityTagComboBox.SelectedValue = "";
        }
                
    }
}
