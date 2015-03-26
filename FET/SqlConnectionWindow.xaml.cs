using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SqlConnectionWindow.xaml
    /// </summary>
    public partial class SqlConnectionWindow : MetroWindow
    {
        BackgroundWorker bgWorker;
        public SqlConnectionWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string conString = connectionString.Text;

            
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(Export);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExportCompleted);
 
            bgWorker.RunWorkerAsync(conString);
            
        }

        void Export(object sender, DoWorkEventArgs e)
        {
            string conString = (string)e.Argument;
            if (ManageDatabase.ExportToDatabase(conString, bgWorker))
            {
                MessageBox.Show("Successfully exported!");
            }
        }

        void ExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("An error has occurred, be sure that connection string is correct!"
                                + Environment.NewLine + e.Error.Message);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            bgWorker.CancelAsync();
            this.Close();
        }
    }
}
