using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using FET.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FET.ViewModel
{
    class BreakTimesViewModel : ViewModelBase
    {
        public string SelectedHour { get; set; }
        public string SelectedDay { get; set; }
        public TimeDayHour SelectedTime { get; set; }

        public ObservableCollection<TimeDayHour> BreakTimes { get; set; }

        private ObservableCollection<string> hours;
        public ObservableCollection<string> Hours
        {
            get
            {
                return this.hours;
            }
            set
            {
                this.hours = value;
            }
        }

        private ObservableCollection<string> days;
        public ObservableCollection<string> Days
        {
            get { return days; }
            set { days = value; }
        }

        public BreakTimesViewModel()
        {
            BreakTimes = new ObservableCollection<TimeDayHour>(Timetable.GetInstance().TimeConstraints.ConstraintsBreakTimes.BreakTimes);
            Hours = new ObservableCollection<string>(Timetable.GetInstance().HoursList);
            Days = new ObservableCollection<string>(Timetable.GetInstance().DaysList);
        }

        private DelegateCommand addTimeCommand;
        private DelegateCommand delTimeCommand;

        public ICommand AddTimeCommand
        {
            get
            {
                if (addTimeCommand == null)
                {
                    addTimeCommand = new DelegateCommand(AddTime);
                }
                return addTimeCommand;
            }
        }
        public ICommand DelTimeCommand
        {
            get
            {
                if (delTimeCommand == null)
                {
                    delTimeCommand = new DelegateCommand(DelTime);
                }
                return delTimeCommand;
            }
        }

        void AddTime(object obj)
        {
            if (!string.IsNullOrEmpty(SelectedHour) && !string.IsNullOrEmpty(SelectedDay))
            {
                Timetable.GetInstance().TimeConstraints.ConstraintsBreakTimes.BreakTimes.Add(new TimeDayHour(SelectedDay, SelectedHour));
                BreakTimes = new ObservableCollection<TimeDayHour>(Timetable.GetInstance().TimeConstraints.ConstraintsBreakTimes.BreakTimes);
                OnPropertyChanged("BreakTimes");
            }
        }

        void DelTime(object obj)
        {
            if (SelectedTime != null)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintsBreakTimes.BreakTimes.Remove(SelectedTime);
                BreakTimes = new ObservableCollection<TimeDayHour>(Timetable.GetInstance().TimeConstraints.ConstraintsBreakTimes.BreakTimes);
                OnPropertyChanged("BreakTimes");
            }
        }
    }
}
