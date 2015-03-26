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
using FET.DataUI;
using FET.ViewModel;
using Telerik.Windows.Controls;

namespace FET
{
    /// <summary>
    /// Interaction logic for DataBuilding.xaml
    /// </summary>
    public partial class DataBuilding : Window
    {
        BuildingViewModel vm;
        public DataBuilding()
        {
            InitializeComponent();
            vm = new BuildingViewModel();
            this.DataContext = vm;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
    }
}
