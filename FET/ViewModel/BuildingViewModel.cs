using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FET.Data;
using FET.Convertor;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FET.ViewModel
{
    class BuildingViewModel : ViewModelBase
    {
        private object currentSelected;
        public object CurrentSelected
        {
            get { return this.currentSelected; }
            set 
            {
                if (value is Building)
                {
                    IsBuilding = true;
                    IsRoom = false;
                }
                else
                {
                    IsBuilding = false;
                    IsRoom = true;
                }                
                this.currentSelected = value;
                OnPropertyChanged("IsBuilding");
                OnPropertyChanged("IsRoom");
            }
        }

        private ObservableCollection<Data.Building> buildingsList;
        public ObservableCollection<Data.Building> BuildingsList
        {
            get { return buildingsList; }
            set { buildingsList = value; }
        }

        private bool isBuilding;
        public bool IsBuilding
        {
            get { return isBuilding; }
            set 
            { 
                isBuilding = value;
                
            }
        }

        private bool isRoom;
        public bool IsRoom
        {
            get { return isRoom; }
            set 
            { 
                isRoom = value;
                
            }
        }
        
        private string newRoomName ;
        public string NewRoomName
        {
            get { return newRoomName; }
            set
            {
                newRoomName = value;
                OnPropertyChanged("NewRoomName");
            }
        }

        private int newRoomCapacity = -1;
        public int NewRoomCapacity
        {
            get { return newRoomCapacity; }
            set 
            {
                newRoomCapacity = value; 
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

        private BoolToVisibilityConverter converter;
        public BoolToVisibilityConverter Converter
        {
            get { return converter; }
            set { converter = value; }
        }

        public string SelectedHour { get; set; }
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

        public string SelectedDay { get; set; }
        private ObservableCollection<string> days;
        public ObservableCollection<string> Days
        {
            get { return days; }
            set { days = value; }
        }

        public TimeDayHour SelectedTime { get; set; }

        public BuildingViewModel()
        {
            this.BuildingsList = new ObservableCollection<Building>(Timetable.GetInstance().BuildingsList);
            this.IsBuilding = true;
            this.IsRoom = false;
            this.Converter = new BoolToVisibilityConverter();
            this.Hours = new ObservableCollection<string>( Timetable.GetInstance().HoursList);
            this.Days = new ObservableCollection<string>( Timetable.GetInstance().DaysList);
        }

        private DelegateCommand addBuildingCommand;
        private DelegateCommand delBuildingCommand;
        private DelegateCommand addRoomCommand;
        private DelegateCommand delRoomCommand;
        private DelegateCommand addTimeCommand;
        private DelegateCommand delTimeCommand;

        public ICommand AddBuildingCommand
        {
            get
            {
                if (addBuildingCommand == null)
                {
                    addBuildingCommand = new DelegateCommand(AddBuilding);
                }
                return addBuildingCommand;
            }
        }
        public ICommand RemoveBuildingCommand
        {
            get
            {
                if (delBuildingCommand == null)
                {
                    delBuildingCommand = new DelegateCommand(RemoveBuilding);
                }
                return delBuildingCommand;
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

        void AddBuilding(object obj)
        {
            Building building = new Building((string)App.Current.TryFindResource("newBuildingName"));
            Timetable.GetInstance().BuildingsList.Add(building);
            BuildingsList = new ObservableCollection<Building>(Timetable.GetInstance().BuildingsList);
            CurrentSelected = building;
            OnPropertyChanged("BuildingsList");
            OnPropertyChanged("CurrentSelected");
        }
        void RemoveBuilding(object obj)
        {
            if (CurrentSelected != null && CurrentSelected is Building)
            {
                var building = CurrentSelected as Building;
                Timetable.GetInstance().BuildingsList.Remove(building);

            }
            BuildingsList = new ObservableCollection<Building>(Timetable.GetInstance().BuildingsList);
        }

        void AddRoom(object obj)
        {
            if (CurrentSelected != null && CurrentSelected is Building)
            {
                Room room = new Room(NewRoomName, (CurrentSelected as Building).Name);
                if (NewRoomCapacity != -1)
                {
                    room.Capacity = NewRoomCapacity ;
                    NewRoomCapacity = -1;
                }
                (CurrentSelected as Building).Rooms.Add(room);
                NewRoomName = "";
            }
        }
        void RemoveRoom(object obj)
        {
            if (SelectedRoomRemove != null && CurrentSelected is Building)
            {
                (CurrentSelected as Building).Rooms.Remove(SelectedRoomRemove);
            }
        }

        void AddTime(object obj)
        {
            if (CurrentSelected is Room && !string.IsNullOrEmpty(SelectedDay) && !string.IsNullOrEmpty(SelectedHour))
            {
                (CurrentSelected as Room).NotAvailableTimes.Add(new TimeDayHour(SelectedDay, SelectedHour));
            }
        }
        void RemoveTime(object obj)
        {
            if (CurrentSelected is Room && SelectedTime != null)
            {
                (CurrentSelected as Room).NotAvailableTimes.Remove(SelectedTime);
            }
        }
    }
}
