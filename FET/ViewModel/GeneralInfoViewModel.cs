using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using FET.Data;
using Telerik.Windows.Controls;

namespace FET.ViewModel
{
    class GeneralInfoViewModel : ViewModelBase
    {
        private Timetable timetable;

        public string InstitutionName
        {
            get { return timetable.InstitutionName; }
            set { timetable.InstitutionName = value; }
        }

        public string Comments
        {
            get { return timetable.Comments; }
            set { this.Comments = value; }
        }

        public string SelectedHour { get; set; }
        public string SelectedDay { get; set; }
        public string NewHour { get; set; }
        public string NewDay { get; set; }

        private ObservableCollection<string> days;
        public ObservableCollection<string> Days
        {
            get { return this.days; }
            set 
            { 
                this.days = value;               
            }
        }

        private ObservableCollection<string> hours;
        public ObservableCollection<string> Hours
        {
            get { return this.hours; }
            set 
            {  
                this.hours = value;                
            }
        }

        private static object _syncLock = new object();

        public GeneralInfoViewModel()
        {
            timetable = Timetable.GetInstance();
            Days = new ObservableCollection<string>(timetable.DaysList);
            Hours = new ObservableCollection<string>(timetable.HoursList);
        }

        private DelegateCommand addHourCommand;
        private DelegateCommand delHourCommand;
        private DelegateCommand addDayCommand;
        private DelegateCommand delDayCommand;

        public ICommand AddHourCommand
        {
            get
            {
                if (addHourCommand == null)
                {
                    addHourCommand = new DelegateCommand(AddHour);
                }
                return addHourCommand;
            }
        }

        public ICommand DelHourCommand
        {
            get
            {
                if (delHourCommand == null)
                {
                    delHourCommand = new DelegateCommand(DelHour);
                }
                return delHourCommand;
            }
        }

        public ICommand AddDayCommand
        {
            get
            {
                if (addDayCommand == null)
                {
                    addDayCommand = new DelegateCommand(AddDay);
                }
                return addDayCommand;
            }
        }

        public ICommand DelDayCommand
        {
            get
            {
                if (delDayCommand == null)
                {
                    delDayCommand = new DelegateCommand(DelDay);
                }
                return delDayCommand;
            }
        }

        void AddHour(object obj)
        {
            if (!string.IsNullOrEmpty(NewHour))
            {
                Timetable.GetInstance().HoursList.Add(NewHour);
                Hours = new ObservableCollection<string>(Timetable.GetInstance().HoursList);
                OnPropertyChanged("Hours");
                NewHour = "";
                OnPropertyChanged("NewHour");
            }
        }
        void DelHour(object obj)
        {
            if (!string.IsNullOrEmpty(SelectedHour))
            {
                Timetable.GetInstance().HoursList.Remove(SelectedHour);
                Hours = new ObservableCollection<string>(Timetable.GetInstance().HoursList);
                OnPropertyChanged("Hours");
            }
        }
        void AddDay(object obj)
        {
            if (!string.IsNullOrEmpty(NewDay))
            {
                Timetable.GetInstance().DaysList.Add(NewDay);
                Days = new ObservableCollection<string>(Timetable.GetInstance().DaysList);
                OnPropertyChanged("Days");
                NewDay = "";
                OnPropertyChanged("NewDay");
            }
        }
        void DelDay(object obj)
        {
            if (!string.IsNullOrEmpty(SelectedDay))
            {
                Timetable.GetInstance().DaysList.Remove(SelectedDay);
                Days = new ObservableCollection<string>(Timetable.GetInstance().DaysList);
                OnPropertyChanged("Days");
            }
        }
    }
}
