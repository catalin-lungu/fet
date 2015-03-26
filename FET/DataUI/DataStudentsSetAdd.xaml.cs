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

namespace FET.DataUI
{
    /// <summary>
    /// Interaction logic for DataStudentsAdd.xaml
    /// </summary>
    public partial class DataStudentsSetAdd : Window, IDisposable
    {
        string parentNameYear;
        string parentNameGroup;
        string newStudentsName;

        

        public DataStudentsSetAdd()
        {
            InitializeComponent();
            this.DataContext = new StudentsSetAddViewModel();
        }

        public string GetParentNameYear()
        {
            return this.parentNameYear;
        }

        public string GetParentNameGroup()
        {
            return this.parentNameGroup;
        }

        public string GetStudentsName()
        {
            return this.newStudentsName;
        }


        public void Dispose()
        {
            this.Close();
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentNameYear = yearComboBox.SelectedItem != null ? ((Data.Year)yearComboBox.SelectedItem).Name : string.Empty;
            this.parentNameGroup = groupComboBox.SelectedItem != null ? ((Data.Group)groupComboBox.SelectedItem).Name : string.Empty;
            this.newStudentsName = txtName.Text;

            this.Close();
        }
    }

    
}
