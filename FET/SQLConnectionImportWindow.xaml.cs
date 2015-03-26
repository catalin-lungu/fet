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
using MahApps.Metro.Controls;

namespace FET
{
    /// <summary>
    /// Interaction logic for SQLConnectionImportWindow.xaml
    /// </summary>
    public partial class SQLConnectionImportWindow : MetroWindow
    {
        public SQLConnectionImportWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string conString = connectionString.Text;
            if (ManageDatabase.ImportFromDatabase(conString))
            {
                MessageBox.Show("Successfully imported!");
                this.Close();
            }
            else
            {
                MessageBox.Show("An error has occurred, be sure that connection string is correct!");
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
