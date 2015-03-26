using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FET.Data;
using Telerik.Windows.Controls;
using System.Windows.Input;

namespace FET.ViewModel
{
    class StudentsSetViewModel : ViewModelBase
    {
        public string SelectedDay { get; set; }
        public string SelectedHour { get; set; }
        public TimeDayHour SelectedTime { get; set; }
        public Students SelectedStudents { get; set; }

        private ObservableCollection<Data.Year> classList;

        public ObservableCollection<Data.Year> ClassList
        {
            get { return classList; }
            set { classList = value; }
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

        public StudentsSetViewModel()
        {
            if (Timetable.GetInstance().ClassList == null)
            {
                Timetable.GetInstance().ClassList = new List<Year>();
            }

            this.ClassList = new ObservableCollection<Year>(Timetable.GetInstance().ClassList);
            this.Hours = new ObservableCollection<string>(Timetable.GetInstance().HoursList);
            this.Days = new ObservableCollection<string>(Timetable.GetInstance().DaysList);
            this.Rooms = new ObservableCollection<Room>(Timetable.GetInstance().GetRoomsList());
        }

        private DelegateCommand addStudentsCommand;
        private DelegateCommand delStudentsCommand;
        private DelegateCommand addRoomCommand;
        private DelegateCommand delRoomCommand;
        private DelegateCommand addTimeCommand;
        private DelegateCommand delTimeCommand;

        public ICommand AddStudentsCommand
        {
            get
            {
                if (addStudentsCommand == null)
                {
                    addStudentsCommand = new DelegateCommand(AddStudents);
                }
                return addStudentsCommand;
            }
        }
        public ICommand RemoveStudentsCommand
        {
            get
            {
                if (delStudentsCommand == null)
                {
                    delStudentsCommand = new DelegateCommand(RemoveStudents);
                }
                return delStudentsCommand;
            }
        }

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
        public ICommand RemoveTimeCommand
        {
            get
            {
                if (delTimeCommand == null)
                {
                    delTimeCommand = new DelegateCommand(RemoveTime);
                }
                return delTimeCommand;
            }
        }


        void AddStudents(object obj)
        {
            string name = string.Empty;
            string parentNameYear = string.Empty;
            string parentNameGroup = string.Empty;

            using (FET.DataUI.DataStudentsSetAdd addStudents = new FET.DataUI.DataStudentsSetAdd())
            {
                addStudents.ShowDialog();

                name = addStudents.GetStudentsName();
                parentNameYear = addStudents.GetParentNameYear();
                parentNameGroup = addStudents.GetParentNameGroup();
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(parentNameYear))
                {
                    if (!string.IsNullOrEmpty(parentNameGroup))
                    {
                        foreach (var y in Timetable.GetInstance().ClassList)
                        {
                            if (y.Name.Equals(parentNameYear))
                            {
                                foreach (var g in y.Groups)
                                {
                                    if (g.Name.Equals(parentNameGroup))
                                    {
                                        var subG = new Subgroup(g.Name);
                                        subG.Name = name;
                                        g.Subgroups.Add(subG);
                                    }
                                }
                            }
                        }
                    }
                    else // this is a new group
                    {
                        var g = new Group(parentNameYear) { Name = name };
                        foreach (var y in Timetable.GetInstance().ClassList)
                        {
                            if (y.Name.Equals(parentNameYear))
                            {
                                y.Groups.Add(g);
                            }
                        }
                    }
                }
                else //this is a new year
                {
                    var y = new Year() { Name = name };
                    Timetable.GetInstance().ClassList.Add(y);
                }
            }

            this.ClassList = new ObservableCollection<Year>(Timetable.GetInstance().ClassList);
            OnPropertyChanged("ClassList");
        }
        void RemoveStudents(object obj)
        {
            if (SelectedStudents != null)
            {
                if (SelectedStudents is Year)
                {
                    Timetable.GetInstance().ClassList.Remove(SelectedStudents as Year);
                }
                else if (SelectedStudents is Group)
                {
                    var group = SelectedStudents as Group;
                    var year = Timetable.GetInstance().ClassList.Find(y => y.Name.Equals(group.YearName));
                    year.Groups.Remove(group);
                    
                }
                else if (SelectedStudents is Subgroup)
                {
                    var subgroup = SelectedStudents as Subgroup;
                    foreach (var y in Timetable.GetInstance().ClassList)
                    {
                        var group = y.Groups.Find(g => g.Name.Equals(subgroup.GroupName));
                        if (group != null)
                        {
                            group.Subgroups.Remove(subgroup);
                            break;
                        }
                    }                                     
                    
                }

                this.ClassList = new ObservableCollection<Year>(Timetable.GetInstance().ClassList);
                OnPropertyChanged("ClassList");
            }
        }

        void AddRoom(object obj)
        {
            if (SelectedStudents != null &&(SelectedRoomAdd != null))
            {
                SelectedStudents.HomeRooms.Add(SelectedRoomAdd);
            }
        }
        void RemoveRoom(object obj)
        {
            if (SelectedStudents != null && SelectedRoomRemove != null)
            {
                SelectedStudents.HomeRooms.Remove(SelectedRoomRemove);
            }
        }

        void AddTime(object obj)
        {
            if (SelectedStudents != null && !string.IsNullOrEmpty(SelectedHour) && !string.IsNullOrEmpty(SelectedDay))
            {
                SelectedStudents.NotAvailableTimes.Add(new TimeDayHour(SelectedDay, SelectedHour));
            } 
        }
        void RemoveTime(object obj)
        {
            if (SelectedStudents != null && SelectedTime != null)
            {
                SelectedStudents.NotAvailableTimes.Remove(SelectedTime);
            } 
        }
    }

    
}
