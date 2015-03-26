using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using FET.Data;
using System.Windows.Input;
using System.Windows.Data;

namespace FET.ViewModel
{
    class SubjectsViewModel : ViewModelBase
    {

        private Subject selectedSubject;

        public Subject SelectedSubject
        {
            get { return selectedSubject; }
            set { selectedSubject = value; }
        }

        private ObservableCollection<Subject> subjects;

        public ObservableCollection<Subject> Subjects
        {
            get { return subjects; }
            set { subjects = value; }
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

        public SubjectsViewModel()
        {
            this.Subjects = new ObservableCollection<Subject>(Timetable.GetInstance().SubjectList);
            this.Rooms = new ObservableCollection<Room>(Timetable.GetInstance().GetRoomsList());
        }

        private DelegateCommand addRoomCommand;
        private DelegateCommand delRoomCommand;
        private DelegateCommand addSubjectCommand;
        private DelegateCommand delSubjectCommand;

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

        public ICommand AddSubjectCommand
        {
            get
            {
                if (addSubjectCommand == null)
                {
                    addSubjectCommand = new DelegateCommand(AddSubject);
                }
                return addSubjectCommand;
            }
        }

        public ICommand DelSubjectCommand
        {
            get
            {
                if (delSubjectCommand == null)
                {
                    delSubjectCommand = new DelegateCommand(DelSubject);
                }
                return delSubjectCommand;
            }
        }

        void AddRoom(object obj)
        {
            if (SelectedRoomAdd != null)
            {                
                SelectedSubject.HomeRooms.Add(SelectedRoomAdd);  
            }
        }

        void RemoveRoom(object obj)
        {
            if (SelectedRoomRemove != null)
            {
                SelectedSubject.HomeRooms.Remove(SelectedRoomRemove);               
            }
        }

        void AddSubject(object obj)
        {
            var subject = new Subject((string)App.Current.TryFindResource("new"));
            Subjects.Add(subject);
            Timetable.GetInstance().SubjectList.Add(subject);
            OnPropertyChanged("Subjects");
            
        }

        void DelSubject(object obj)
        {
            if (SelectedSubject != null)
            {
                Timetable.GetInstance().SubjectList.Remove(SelectedSubject);
                Subjects.Remove(SelectedSubject);
                OnPropertyChanged("Subjects");
            }
        }
    }
}
