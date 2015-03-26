using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FET.Data;
using FET.SpaceConstraintsUI.Subject_and_ActivityTag;
using FET.SpaceConstraintsUI.Activity;
using FET.TimeConstraintsUI;
using FET.TimeConstraintsUI.Activities;
using FET.TimeConstraintsUI.Students;
using FET.TimeConstraintsUI.Teacher;
using Microsoft.Win32;
using System.ComponentModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Navigation;
using FET.TimeConstraintsUI.Teachers;
using FET.DataUI;

namespace FET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> recentFiles = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();       
        }

        private void ConstraintsTeachers_Click(object sender, RoutedEventArgs e)
        {
            new AllTeachers().Show();
        }

        private void ConstraintsStudents_Click(object sender, RoutedEventArgs e)
        {
            new AllStudents().Show();
        }


        #region file
        private void New_Click(object sender, RoutedEventArgs e)
        {
            Timetable.GetInstance().Clear();
            MessageBox.Show("New timetable has created!" + Environment.NewLine + "Enter data using data meniu!");
               
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".fet"; 
            fileDialog.Filter = "FET documents (.fet)|*.fet";
            
            if (fileDialog.ShowDialog().Value)
            {

                MenuItem item = new MenuItem();
                item.Header = fileDialog.SafeFileName;
                item.Click += OpenRecent_Click;

                MenuItem items = new MenuItem();
                for (int i = openRecentList.Items.Count-1; i >= 0; i--)
                {
                    var it = openRecentList.Items.GetItemAt(i);
                    openRecentList.Items.RemoveAt(i);
                    items.Items.Add(it);
                }

                //add new opened
                openRecentList.Items.Add(item);
                
                //add older
                for (int i = items.Items.Count - 1; i >= 0; i-- )
                {
                    var it = items.Items.GetItemAt(i);
                    items.Items.RemoveAt(i);
                    openRecentList.Items.Add(it);
                }

                if (recentFiles.ContainsValue(fileDialog.FileName))
                {
                    recentFiles.Remove(item.Header.ToString());
                }
                recentFiles.Add(item.Header.ToString(), fileDialog.FileName);
                

                OpenData.Load(fileDialog.FileName);
                App.DataPathFile = fileDialog.FileName;

                MessageBox.Show("Data loaded!");
            }
        }

        private void OpenRecent_Click(object sender, RoutedEventArgs e)
        {
            foreach(var f in recentFiles)
            {
                if (f.Key.Equals(((MenuItem)sender).Header.ToString()))
                {
                    OpenData.Load(f.Value);
                    App.DataPathFile = f.Value;

                    MessageBox.Show("Data loaded!");
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            
            for (int i = openRecentList.Items.Count - 1; i >= 0; i-- )
            {
                if(recentFiles.Keys.Contains(((MenuItem)openRecentList.Items.GetItemAt(i)).Header.ToString()))
                {
                    openRecentList.Items.RemoveAt(i);
                }
            }
            recentFiles.Clear(); 

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ManageFile.SaveChanges();
            MessageBox.Show("Data has been saved!");     
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".fet";
            saveDialog.Filter = "FET documents (.fet)|*.fet|All files (.*)|*.*";
            
            if (saveDialog.ShowDialog().Value)
            {
                ManageFile.SaveChanges(saveDialog.FileName);
                MessageBox.Show("Data has been saved!");
            }
          
        }

        private void ImportFromDb_Click(object sender, RoutedEventArgs e)
        {
            new SQLConnectionImportWindow().Show();
        }

        private void ExportToDB_Click(object sender, RoutedEventArgs e)
        {
            new SqlConnectionWindow().Show();
        }



        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            if (App.DataHasChanged)
            {
                if (MessageBox.Show("Save data changes before close?", "Save data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ManageFile.SaveChanges();
                }
            }
            Application.Current.Shutdown();
        }

        #endregion
              
        
        #region data
        #region general
        private void GeneralInfo_Click(object sender, RoutedEventArgs e)
        {
            new DataGeneralInfo().Show();
        }

        private void Buildings_Click(object sender, RoutedEventArgs e)
        {
            new DataBuilding().Show();
        }

        private void Subjects_Click(object sender, RoutedEventArgs e)
        {
            new DataSubjects().Show();
        }

        private void ActivityTags_Click(object sender, RoutedEventArgs e)
        {
            new DataActivityTags().Show();
        }

        private void Teachers_Click(object sender, RoutedEventArgs e)
        {
            new DataTeacher().Show();
        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            new DataStudentsSet().Show();
        }

        
        private void Activities_Click(object sender, RoutedEventArgs e)
        {
            new DataActivities().Show();
        }

        
        
        #endregion

        #region time constraints


        #region Miscellaneous

        private void NotAvailableTimes_Click(object sender, RoutedEventArgs e)
        {
            new BreakTimes().Show();
        }

        #endregion

        #region teacher constraint

        private void MaxHoursWithAnActivityTagForATeacher_Click(object sender, RoutedEventArgs e)
        {
            new DataTimeConstraintTeacherActivityTag().Show();
        }               
       

        #endregion

        #region teachers
       
        private void MaxHoursDailyWithAnActivityTagForAllTeachers_Click(object sender, RoutedEventArgs e)
        {
           // new DataTimeConstraintTeachers(ManageFile.TimeContraints.ConstraintTeachersActivityTagMaxHoursDailyList).Show();
        }       
        private void MaxHoursContinuouslyWithAnActivityTagForAllTeachers_Click(object sender, RoutedEventArgs e)
        {
            //new DataTimeConstraintTeachersActivityTag(ManageFile.TimeContraints.ConstraintTeachersActivityTagMaxHoursContinuouslyList).Show();
        }
        
        #endregion

        #region students set


        private void MaxHoursForStudentsSetWithActivityTag_Click(object sender, RoutedEventArgs e)
        {
            new MaxHoursDailyForStudentsSetWithActivityTag().Show();
        }
        
       
        #endregion

        #region students 

        
        private void MaxHoursDailyWithActivityTagAllStudents_Click(object sender, RoutedEventArgs e)
        {
            new MaxHoursActivityTagStudents(Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursDailyList).Show();
        }
        
        private void MaxHoursContinuouslyActivityTagAllStudents_Click(object sender, RoutedEventArgs e)
        {
            new MaxHoursActivityTagStudents(Timetable.GetInstance().TimeConstraints.ConstraintsStudents.StudentsActivityTagMaxHoursContinuouslyList).Show();
        }
        
        #endregion

        #region activities

        private void ActivityHasPreferredStartingTime_Click(object sender, RoutedEventArgs e)
        {
            new ActivityTimes(Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimeList).Show();
        }
        private void ActivityHasSetOfPreferredStartingTimes_Click(object sender, RoutedEventArgs e)
        {
            new ActivityTimes(Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredStartingTimesList).Show();

        }
        private void ActivityHasSetOfPreferredTimeSlots_Click(object sender, RoutedEventArgs e)
        {
            new ActivityTimes(Timetable.GetInstance().TimeConstraints.ConstraintActivityPreferredTimeSlotsList).Show();
        }

        private void SetOfActivityHasSetOfPreferredStartingTimes_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesTimes(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredStartingTimesList).Show();
        }
        private void SetOfActivityHasSetOfPreferredTimeSlots_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesTimes(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesPreferredTimeSlotsList).Show();
        }

        private void SetOfSubactivityHasSetOfPreferredStartingTimes_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesTimes(Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredStartingTimesList).Show();

        }
        private void SetOfSubactivityHasSetOfPreferredTimeSlots_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesTimes(Timetable.GetInstance().TimeConstraints.ConstraintSubactivitiesPreferredTimeSlotsList).Show();
        }

        private void MinDaysBetweenSetOfActivities_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesNumberConstraint(Timetable.GetInstance().TimeConstraints.ConstraintMinDaysBetweenActivitiesList).Show();
        }
        private void MaxDaysBetweenSetOfActivities_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesNumberConstraint(Timetable.GetInstance().TimeConstraints.ConstraintMaxDaysBetweenActivitiesList).Show();
        }

        private void AnActivityEndsStudentsDay_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintActivityEndsStudentsDayList).Show();
        }
        private void SetOfActivitiesEndStudentsDay_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesTimes(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesEndStudentsDayList).Show();
        }

        private void SetOfActivitiesHasSameStartingTime_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingTimeList).Show();
        }
        private void SetOfActivitiesHasSameStartingDay_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingDayList).Show();
        }
        private void SetOfActivitiesHasSameStartingHour_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesSameStartingHourList).Show();
        }

        

        private void TwoActivitiesAreOrdered_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesOrderedList).Show();
        }

        private void TwoActivitiesAreConsecutive_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesConsecutiveList).Show();
        }
        private void TwoActivitiesAreGrouped_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintTwoActivitiesGroupedList).Show();
        }
        private void ThreeActivitiesAreGrouped_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintThreeActivitiesGroupedList).Show();
        }

        private void ASetOfActivitiesAreNotOverlapping_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesIdsTime(Timetable.GetInstance().TimeConstraints.ConstraintActivitiesNotOverlappingList).Show();
        }
       
        private void MinGapsBetweenSetOfActivities_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesNumberConstraint(Timetable.GetInstance().TimeConstraints.ConstraintMinGapsBetweenActivitiesList).Show();
        }

        #endregion


        #endregion

        #region space constraints

                
        private void ASubjectAnActivityTagHaveAPreferredRoom_Click(object sender, RoutedEventArgs e)
        {
            new DataSpaceSubjectAndActivityTag(Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList).Show();
        }
		private void ASubjectAnActivityTagHaveASetOfPreferredRooms_Click(object sender, RoutedEventArgs e)
        {
            new DataSpaceSubjectAndActivityTag(Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList).Show();
        }        		

        private void ASetOfActivitiesAreInTheSameRoomIfTheyAreConsecutive_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesAreInTheSameRoomIfTheyAreConsecutive(Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList).Show();
        }
		private void ASetOfActivitiesOccupiesMaxDifferentRooms_Click(object sender, RoutedEventArgs e)
        {
            new ActivitiesOccupiesMaxDifferentRooms(Timetable.GetInstance().SpaceConstraints.ConstraintActivitiesOccupyMaxDifferentRoomsList).Show();
        }

        #endregion


        #endregion


        #region timetable
        bool isGenerating = false;
        BackgroundWorker bgWorker;
        private void GenerateNew_Click(object sender, RoutedEventArgs e)
        {
            if (isGenerating) 
            {
                MessageBox.Show("Is Generating...Please Wait!");
                return;
            }
            if (!Timetable.IsLoaded())
            {
                MessageBox.Show("Please load a timetable!");

            }
            else
            {
                bgWorker = new BackgroundWorker();
                bgWorker.WorkerSupportsCancellation = true;
                bgWorker.DoWork += new DoWorkEventHandler(GenerateTimetable);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GenerateTimetableCompleted);

                bgWorker.RunWorkerAsync();
                
                
            }

        }
        void GenerateTimetable(object sender, DoWorkEventArgs e)
        {
            isGenerating = true;
            Timetable.GetInstance().Generate();
        }

        void GenerateTimetableCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isGenerating = false;
            if (e.Error != null)
            {
                MessageBox.Show("An error has occurred!"
                                + Environment.NewLine + e.Error.Message);
            }
            MessageBox.Show("Timetable created!");
        }

        private void StudentsTimetable_Click(object sender, RoutedEventArgs e)
        {
            if (isGenerating)
            {
                MessageBox.Show("Is Generating...Please Wait!");
                return;
            }
            if (!Timetable.IsLoaded() || !Timetable.GetInstance().IsGenerated())
            {
                MessageBox.Show("Please load and generate!");
                return;
            }
            List<StudentYear> timetable = ManageTimetable.GetStudentTimetable(Timetable.GetInstance());
           
            StudentsTimetable studentTimetable = new StudentsTimetable(timetable);
            studentTimetable.Show();

        }
               
        private void TeachersTimetable_Click(object sender, RoutedEventArgs e)
        {
            if (isGenerating)
            {
                MessageBox.Show("Is Generating...Please Wait!");
                return;
            }
            if (!Timetable.IsLoaded() || !Timetable.GetInstance().IsGenerated())
            {
                MessageBox.Show("Please load and generate!");
                return;
            }
            List<TeacherSchedule> timetable = ManageTimetable.GetTeacherTimetable(Timetable.GetInstance());

            TeachersTimetable teachersTimetable = new TeachersTimetable(timetable);
            teachersTimetable.Show();

        }

        private void RoomsTimetable_Click(object sender, RoutedEventArgs e)
        {
            if (isGenerating)
            {
                MessageBox.Show("Is Generating...Please Wait!");
                return;
            }
            if (!Timetable.IsLoaded() || !Timetable.GetInstance().IsGenerated())
            {
                MessageBox.Show("Please load and generate!");
                return;
            }
            List<RoomSchedule> timetable = ManageTimetable.GetRoomTimetable(Timetable.GetInstance());

            RoomsTimetable roomsTimetable = new RoomsTimetable(timetable);
            roomsTimetable.Show();

        }

        #endregion

        #region settings

        private void LanguageChange_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as MenuItem).Tag.ToString();

            App.SetLanguageDictionary(tag);
        }
                
        #endregion

        private void menuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var item = (MenuItem)e.OriginalSource;
            

        }
 
    }

}
