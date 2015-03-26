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

namespace FET.TimeConstraintsUI.Students
{
    /// <summary>
    /// Interaction logic for MaxHoursContinuouslyForStudentsSetWithActivityTag.xaml
    /// </summary>
    public partial class MaxHoursDailyForStudentsSetWithActivityTag : Window
    {
        public MaxHoursDailyForStudentsSetWithActivityTag()
        {
            InitializeComponent();
            this.DataContext = new StudentsSetActivityTagViewModel();
        }

       
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
