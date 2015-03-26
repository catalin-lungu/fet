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
    class TeacherActivityTagViewModel : ViewModelBase
    {
        private Teacher selectedTeacher;
        public Teacher SelectedTeacher
        {
            get { return selectedTeacher; }
            set
            {
                selectedTeacher = value;
            }
        }

        public ObservableCollection<Teacher> teacherList;
        public ObservableCollection<Teacher> TeacherList
        {
            get { return teacherList; }
            set { teacherList = value; }
        }

        public int SelectedMaxHoursDaily { get; set; }
        public ActivityTag SelectedActivityTagDaily { get; set; }

        public int SelectedMaxHoursContinuously { get; set; }
        public ActivityTag SelectedActivityTagContinuously { get; set; }

        public ObservableCollection<ActivityTag> ActivityTags { get; set; }

        public ConstraintActivityTagMaxHoursDaily SelectedConstraintDaily { get; set; }
        public ConstraintActivityTagMaxHoursContinuously SelectedConstraintContinuously { get; set; }

        public TeacherActivityTagViewModel()
        {
            this.TeacherList = new ObservableCollection<Teacher>(Timetable.GetInstance().TeacherList);
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
            if (SelectedTeacher != null && SelectedActivityTagDaily != null)
            {
                SelectedTeacher.TeacherActivityTagMaxHoursDailyList.Add(new ConstraintActivityTagMaxHoursDaily() 
                {
                    ActivityTag = SelectedActivityTagDaily.Name,
                    MaximumHoursDaily = SelectedMaxHoursDaily
                });
            }
        }
        void DelMaxHoursDaily(object obj)
        {
            if (SelectedTeacher != null && SelectedConstraintDaily != null)
            {
                SelectedTeacher.TeacherActivityTagMaxHoursDailyList.Remove(SelectedConstraintDaily);
            }

        }
        void AddMaxContinuouslyDaily(object obj)
        {
            if (SelectedTeacher != null && SelectedActivityTagContinuously != null)
            {
                SelectedTeacher.TeacherActivityTagMaxHoursContinuouslyList.Add(new ConstraintActivityTagMaxHoursContinuously()
                {
                    ActivityTag = SelectedActivityTagContinuously.Name,
                    MaximumHoursContinuously = SelectedMaxHoursContinuously
                });
            }
        }
        void DelMaxContinuouslyDaily(object obj)
        {
            if (SelectedTeacher != null && SelectedConstraintContinuously != null)
            {
                SelectedTeacher.TeacherActivityTagMaxHoursContinuouslyList.Remove(SelectedConstraintContinuously);
            }
        }
    }
}
