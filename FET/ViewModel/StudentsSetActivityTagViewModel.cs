using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FET.Data;
using Telerik.Windows.Controls;

namespace FET.ViewModel
{
    class StudentsSetActivityTagViewModel : ViewModelBase
    {
        private Students selectedStudents;
        public Students SelectedStudents
        {
            get { return selectedStudents; }
            set
            {
                selectedStudents = value;
            }
        }

        public ObservableCollection<Students> studentsList;
        public ObservableCollection<Students> StudentsList
        {
            get { return studentsList; }
            set { studentsList = value; }
        }

        public int SelectedMaxHoursDaily { get; set; }
        public ActivityTag SelectedActivityTagDaily { get; set; }

        public int SelectedMaxHoursContinuously { get; set; }
        public ActivityTag SelectedActivityTagContinuously { get; set; }

        public ObservableCollection<ActivityTag> ActivityTags { get; set; }

        public ConstraintActivityTagMaxHoursDaily SelectedConstraintDaily { get; set; }
        public ConstraintActivityTagMaxHoursContinuously SelectedConstraintContinuously { get; set; }

        public StudentsSetActivityTagViewModel()
        {
            this.StudentsList = new ObservableCollection<Students>(Timetable.GetInstance().GetStudents());
            this.ActivityTags = new ObservableCollection<ActivityTag>(Timetable.GetInstance().ActivitiyTagsList);
        }

        private DelegateCommand addDailyCommand;
        private DelegateCommand delDailyCommand;
        private DelegateCommand addContinuouslyCommand;
        private DelegateCommand delContinuouslyCommand;

        public ICommand AddDailyCommand
        {
            get
            {
                if (addDailyCommand == null)
                {
                    addDailyCommand = new DelegateCommand(AddMaxHoursDaily);
                }
                return addDailyCommand;
            }
        }

        public ICommand DelDailyCommand
        {
            get
            {
                if (delDailyCommand == null)
                {
                    delDailyCommand = new DelegateCommand(DelMaxHoursDaily);
                }
                return delDailyCommand;
            }
        }

        public ICommand AddContinuouslyCommand
        {
            get
            {
                if (addContinuouslyCommand == null)
                {
                    addContinuouslyCommand = new DelegateCommand(AddMaxContinuouslyDaily);
                }
                return addContinuouslyCommand;
            }
        }

        public ICommand DelContinuouslyCommand
        {
            get
            {
                if (delContinuouslyCommand == null)
                {
                    delContinuouslyCommand = new DelegateCommand(DelMaxContinuouslyDaily);
                }
                return delContinuouslyCommand;
            }
        }

        void AddMaxHoursDaily(object obj)
        {
            if (SelectedStudents != null && SelectedActivityTagDaily != null)
            {
                SelectedStudents.StudentsSetActivityTagMaxHoursDailyList.Add(new ConstraintActivityTagMaxHoursDaily()
                {
                    ActivityTag = SelectedActivityTagDaily.Name,
                    MaximumHoursDaily = SelectedMaxHoursDaily
                });
            }
        }
        void DelMaxHoursDaily(object obj)
        {
            if (SelectedStudents != null && SelectedConstraintDaily != null)
            {
                SelectedStudents.StudentsSetActivityTagMaxHoursDailyList.Remove(SelectedConstraintDaily);
            }

        }
        void AddMaxContinuouslyDaily(object obj)
        {
            if (SelectedStudents != null && SelectedActivityTagContinuously != null)
            {
                SelectedStudents.StudentsSetActivityTagMaxHoursContinuouslyList.Add(new ConstraintActivityTagMaxHoursContinuously()
                {
                    ActivityTag = SelectedActivityTagContinuously.Name,
                    MaximumHoursContinuously = SelectedMaxHoursContinuously
                });
            }
        }
        void DelMaxContinuouslyDaily(object obj)
        {
            if (SelectedStudents != null && SelectedConstraintContinuously != null)
            {
                SelectedStudents.StudentsSetActivityTagMaxHoursContinuouslyList.Remove(SelectedConstraintContinuously);
            }
        }
    }
}
