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
using MahApps.Metro.Controls;

namespace FET
{
    /// <summary>
    /// Interaction logic for RoomsTimetable.xaml
    /// </summary>
    public partial class RoomsTimetable : Window
    {
        List<RoomSchedule> time;
        public RoomsTimetable(List<RoomSchedule> schedule)
        {
            InitializeComponent();
            this.time = schedule;

            foreach (var teacher in schedule)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = teacher.Room.Name;

                list.Items.Add(item);
            }

            list.SelectedIndex = 0;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem boxItem = (ComboBoxItem)list.SelectedItem;
            string selected = boxItem.Content.ToString();

            DataTable dataTable = null;
            foreach (var t in time)
            {
                if (t.Room.Name.Equals(selected))
                {
                    dataTable = new DataTable();

                    dataTable.Columns.Add("Hours");

                    for (int i = 0; i < Timetable.GetInstance().DaysList.Count; i++)
                    {
                        dataTable.Columns.Add(Timetable.GetDay(i));
                    }

                    for (int i = 0; i < Timetable.GetInstance().HoursList.Count; i++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow[0] = Timetable.GetHour(i);
                        foreach (var dayHour in t.DayHourList)
                        {
                            if (i == dayHour.Hour)
                            {
                                dataRow[dayHour.Day + 1] = dayHour.Activity.Subject.Name +
                                    Environment.NewLine + dayHour.Activity.Students;
                            }
                        }

                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            if (dataTable != null)
            {
                timetable.ItemsSource = dataTable.DefaultView;
            }
        }
  
    }
}
