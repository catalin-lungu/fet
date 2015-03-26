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

namespace FET
{
    /// <summary>
    /// Interaction logic for DataTeacher.xaml
    /// </summary>
    public partial class DataTeacher : Window
    {
        bool dataHasChanged;
        TeacherViewModel vm;
        public DataTeacher()
        {
            InitializeComponent();
            this.Closed += Close_Click;
            vm = new TeacherViewModel();
            this.DataContext = vm;
            
            dataHasChanged = false;
        }

        
        private void Close_Click(object sender, EventArgs e)
        {
            if (dataHasChanged)
            {
                App.DataHasChanged = true;
            }
            this.Close();
        }

        private void AddTime_Click(object sender, EventArgs e)
        {

            if (dayComboBox.SelectedItem == null || hourComboBox.SelectedItem == null)
            {
                return;
            }

            string day = dayComboBox.SelectedItem.ToString();
            string hour = hourComboBox.SelectedItem.ToString();
                        
            var teacher = (Data.Teacher)listTeachers.SelectedItem;
            if (teacher == null)
            {
                return;
            }
            teacher.NotAvailableTimes.Add(new Data.TimeDayHour(day, hour));
            
            listSlots.Items.Refresh();

            dayComboBox.SelectedItem = null;
            hourComboBox.SelectedItem = null;
        }

        private void RemoveTime_Click(object sender, EventArgs e)
        {

            var timeDayHour = (Data.TimeDayHour)listSlots.SelectedItem;

            var teacher = (Data.Teacher)listTeachers.SelectedItem;
            if (teacher == null)
            {
                return;
            }
            teacher.NotAvailableTimes.Remove(timeDayHour);
            
            listSlots.Items.Refresh();

        }

        private void AddActivityButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSubject.SelectedItem == null || comboBoxStudent.SelectedItem == null)
            {
                return;
            }

            var subject = (Data.Subject)comboBoxSubject.SelectedItem;
            var students = (Data.Students)comboBoxStudent.SelectedItem;
            var teacher = vm.SelectedTeacher;

            int nrOfLectures = Convert.ToInt32(txtNrOfLectures.Text.Trim());
            for (int i = 0; i < nrOfLectures; i++)
            {
                Activity act = new Activity()
                {
                    Teacher = teacher,
                    Students = students,
                    Subject = subject
                };
                Timetable.GetInstance().ActivityList.Add(act);
            }
            vm.UpdateTeachList();
        }

        private void RemoveActivityButton_Click(object sender, RoutedEventArgs e)
        {
            if (listActivities.SelectedItem == null)
            {
                return;
            }
            var act = (Activity)listActivities.SelectedItem;
            Timetable.GetInstance().ActivityList.Remove(act);
            vm.UpdateTeachList();
            listActivities.SelectedItem = null;
        }

                          
        
    }
}
