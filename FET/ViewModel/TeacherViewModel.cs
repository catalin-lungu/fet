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
    class TeacherViewModel : ViewModelBase
    {
        private Teacher selectedTeacher;
        public Teacher SelectedTeacher
        {
            get { return selectedTeacher; }
            set
            {
                selectedTeacher = value;
                this.OnPropertyChanged("Activities");
            }
        }

        public string SelectedHour { get; set; }
        public string SelectedDay { get; set; }
        public TimeDayHour SelectedTime { get; set; }

        public ObservableCollection<Teacher> teacherList;
        public ObservableCollection<Teacher> TeacherList
        {
            get { return teacherList; }
            set { teacherList = value; }
        }

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

        private ObservableCollection<Activity> activities = new ObservableCollection<Activity>();
        public ObservableCollection<Activity> Activities
        { 
            get 
            {
                activities = new ObservableCollection<Activity>((from a in Timetable.GetInstance().ActivityList
                              where a.Teacher.Equals(SelectedTeacher)
                              select a).ToList());
                return activities;
            }
            set 
            {
                this.activities = value;
                foreach (var a in activities)
                {
                    if (!Timetable.GetInstance().ActivityList.Any(item => item.ActivityId == a.ActivityId))
                    {
                        Timetable.GetInstance().ActivityList.Add(a);
                    }
                }
            }
        }

        private ObservableCollection<Subject> subjects;
        public ObservableCollection<Subject> Subjects
        {
            get { return subjects; }
            set { subjects = value; }
        }

        private ObservableCollection<Students> students;
        public ObservableCollection<Students> Students
        {
            get { return students; }
            set { students = value; }
        }

        private ObservableCollection<Room> rooms;
        public ObservableCollection<Room> Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }

        private Room selectedRoomAdd;
        public Room SelectedRoomAdd
        {
            get { return selectedRoomAdd; }
            set
            {
                selectedRoomAdd = value;
                OnPropertyChanged("SelectedRoomAdd");
            }
        }

        private Room selectedRoomRemove;
        public Room SelectedRoomRemove
        {
            get { return selectedRoomRemove; }
            set
            {
                selectedRoomRemove = value;
                OnPropertyChanged("SelectedRoomRemove");
            }
        }

        public TeacherViewModel()
        {
            if (Timetable.GetInstance().TeacherList == null)
            {
                Timetable.GetInstance().TeacherList = new List<Teacher>();
            }

            this.Hours = new ObservableCollection<string>( Timetable.GetInstance().HoursList);
            this.Days = new ObservableCollection<string>(Timetable.GetInstance().DaysList);
            this.TeacherList = new ObservableCollection<Teacher>(Timetable.GetInstance().TeacherList);
            this.SelectedTeacher = new Teacher();
            this.Subjects = new ObservableCollection<Subject>(Timetable.GetInstance().SubjectList);
            this.Students = new ObservableCollection<Students>(Timetable.GetInstance().GetStudents());
            this.Rooms = new ObservableCollection<Room>( Timetable.GetInstance().GetRoomsList());
        }

        public void UpdateTeachList()
        {
            this.OnPropertyChanged("Activities");
        }

        private DelegateCommand addRoomCommand;
        private DelegateCommand delRoomCommand;
        private DelegateCommand addTeacherCommand;
        private DelegateCommand delTeacherCommand;
        private DelegateCommand addTimeCommand;
        private DelegateCommand delTimeCommand;

        public ICommand AddRoomCommand
        {
            get
            {
                if (addRoomCommand == null)
                {
                    addRoomCommand = new DelegateCommand(AddRoom);
                }
                return addRoomCommand;
            }
        }
        public ICommand RemoveRoomCommand
        {
            get
            {
                if (delRoomCommand == null)
                {
                    delRoomCommand = new DelegateCommand(RemoveRoom);
                }
                return delRoomCommand;
            }
        }

        public ICommand AddTeacherCommand
        {
            get
            {
                if (addTeacherCommand == null)
                {
                    addTeacherCommand = new DelegateCommand(AddTeacher);
                }
                return addTeacherCommand;
            }
        }
        public ICommand DelTeacherCommand
        {
            get
            {
                if (delTeacherCommand == null)
                {
                    delTeacherCommand = new DelegateCommand(DelTeacher);
                }
                return delTeacherCommand;
            }
        }

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

        void AddRoom(object obj)
        {
            if (SelectedRoomAdd != null)
            {
                SelectedTeacher.HomeRooms.Add(SelectedRoomAdd);                
            }
        }

        void RemoveRoom(object obj)
        {
            if (SelectedRoomRemove != null)
            {
                SelectedTeacher.HomeRooms.Remove(SelectedRoomRemove);
            }
        }

        void AddTeacher(object obj)
        {
            var teacher = new Teacher((string)App.Current.TryFindResource("new"));
            TeacherList.Add(teacher);
            Timetable.GetInstance().TeacherList.Add(teacher);
            OnPropertyChanged("TeacherList");

        }

        void DelTeacher(object obj)
        {
            if (SelectedTeacher != null)
            {
                Timetable.GetInstance().TeacherList.Remove(SelectedTeacher);
                TeacherList.Remove(SelectedTeacher);
                OnPropertyChanged("TeacherList");
            }
        }

        void AddTime(object obj)
        {
            if (!string.IsNullOrEmpty(SelectedHour) && !string.IsNullOrEmpty(SelectedDay))
            {
                SelectedTeacher.NotAvailableTimes.Add(new TimeDayHour(SelectedDay, SelectedHour));
            }
        }

        void DelTime(object obj)
        {
            if (SelectedTime != null)
            {
                SelectedTeacher.NotAvailableTimes.Remove(SelectedTime);
            }
        }

       
    }
}
