using System;
using System.Collections.Generic;
using System.Data;
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
using MahApps.Metro.Controls;
using Telerik.Windows.Controls;

namespace FET
{
    /// <summary>
    /// Interaction logic for StudentsTimetable.xaml
    /// </summary>
    public partial class StudentsTimetable : Window
    {
        public StudentsTimetable(List<StudentYear> list)
        {
            InitializeComponent();
            this.DataContext = new StudentsTimetableViewModel();
 
            

        }

        private void listStudents_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var selItem = (RadTreeViewItem)e.OriginalSource;


            if (selItem.Item is Data.StudentSubgroup)
            {
                var subgroup = (Data.StudentSubgroup)selItem.Item;
                DisplaySchedule(subgroup.Schedule);
            }
            else if (selItem.Item is Data.StudentGroup)
            {
                var group = (Data.StudentGroup)selItem.Item;
                DisplaySchedule(group.Schedule);
            }
            else if (selItem.Item is Data.StudentYear)
            {
                var year = (Data.StudentYear)selItem.Item;
                DisplaySchedule(year.Schedule);
            }

        }

        private void DisplaySchedule(string[,] schedule)
        {
            List<Dictionary<string, object>> source = new List<Dictionary<string, object>>();
            DataGrid grid = new DataGrid();
            DataTable dt = new DataTable();

            DataColumn colHours = new DataColumn("");
            dt.Columns.Add(colHours); 

            for (int i = 0; i < schedule.GetLength(0); i++)
            {

                DataColumn textColumn = new DataColumn(Timetable.GetDay(i));
                textColumn.Caption = Timetable.GetDay(i);                
                dt.Columns.Add(textColumn); 
            }
                      

            for (int i = 0; i < schedule.GetLength(1); i++)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                DataRow row = dt.NewRow();
                row[0] = Timetable.GetHour(i);
                for (int j = 0; j < schedule.GetLength(0); j++)
                {                   
                   row[j+1] = schedule[j,i];
                }
                dt.Rows.Add(row);                
            }                                 

            scheduleGrid.ItemsSource = dt.DefaultView; 
        }

        private bool ContainsData(string[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] != null && table[i, j] != string.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

   
}
