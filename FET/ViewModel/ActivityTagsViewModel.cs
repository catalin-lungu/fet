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
    class ActivityTagsViewModel : ViewModelBase
    {
        public ActivityTag SelectedActivityTag { get; set; }

        public ObservableCollection<ActivityTag> ActivityTags { get; set; }

        public Room SelectedAddRoom { get; set; }
        public Room SelectedDelRoom { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        public ActivityTagsViewModel()
        {
            ActivityTags = new ObservableCollection<ActivityTag>(Timetable.GetInstance().ActivitiyTagsList);
            Rooms = new ObservableCollection<Room>(Timetable.GetInstance().GetRoomsList());
        }


        private DelegateCommand addActivityTagCommand;
        private DelegateCommand delActivityTagCommand;
        private DelegateCommand addRoomCommand;
        private DelegateCommand delRoomCommand;

        public ICommand AddActivityTagCommand
        {
            get
            {
                if (addActivityTagCommand == null)
                {
                    addActivityTagCommand = new DelegateCommand(AddActivityTag);
                }
                return addActivityTagCommand;
            }
        }
        public ICommand RemoveActivityTagCommand
        {
            get
            {
                if (delActivityTagCommand == null)
                {
                    delActivityTagCommand = new DelegateCommand(RemoveActivityTag);
                }
                return delActivityTagCommand;
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

        void AddActivityTag(object obj)
        {
            ActivityTag at = new ActivityTag((string)App.Current.TryFindResource("name"));
            Timetable.GetInstance().ActivitiyTagsList.Add(at);
            ActivityTags = new ObservableCollection<ActivityTag>(Timetable.GetInstance().ActivitiyTagsList);
            OnPropertyChanged("ActivityTags");
        }
        void RemoveActivityTag(object obj)
        {
            if (SelectedActivityTag != null)
            {
                Timetable.GetInstance().ActivitiyTagsList.Remove(SelectedActivityTag);
                ActivityTags.Remove(SelectedActivityTag);
                OnPropertyChanged("ActivityTags");
            }
        }

        void AddRoom(object obj)
        {
            if (SelectedActivityTag != null && SelectedAddRoom != null)
            {
                SelectedActivityTag.PreferredRooms.Add(SelectedAddRoom);
            }
        }
        void RemoveRoom(object obj)
        {
            if (SelectedActivityTag != null && SelectedDelRoom != null)
            {
                SelectedActivityTag.PreferredRooms.Remove(SelectedDelRoom);
            }
        }
    
    }
}
