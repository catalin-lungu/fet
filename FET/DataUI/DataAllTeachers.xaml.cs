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
using Telerik.Windows.Controls;

namespace FET.TimeConstraintsUI.Teachers
{
    /// <summary>
    /// Interaction logic for TimeConstraintTeachers.xaml
    /// </summary>
    public partial class AllTeachers : Window
    {
        TeachersViewModel vm;
              

        public AllTeachers()
        {
            InitializeComponent();
            vm = new TeachersViewModel();
            this.DataContext = vm;
        }
               
        private void CloseRadButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

                
    }
}
