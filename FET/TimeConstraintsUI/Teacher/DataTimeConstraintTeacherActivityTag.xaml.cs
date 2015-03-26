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

namespace FET.TimeConstraintsUI.Teacher
{
    /// <summary>
    /// Interaction logic for DataTimeConstraintTeacherActivityTag.xaml
    /// </summary>
    public partial class DataTimeConstraintTeacherActivityTag : Window
    {
        public DataTimeConstraintTeacherActivityTag()
        {
            InitializeComponent();
            this.DataContext = new TeacherActivityTagViewModel();
        }

      
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
