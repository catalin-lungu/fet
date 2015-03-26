using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FET.Data;
using Telerik.Windows.Controls;
using FET.Convertor;

namespace FET.ViewModel
{
    class ActivitiesViewModel : ViewModelBase
    {
        public Activity SelectedActivity { get; set; }

        public ObservableCollection<Activity> Activities { get; set; }

        public Room SelectedAddRoom { get; set; }
        public Room SelectedDelRoom { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        private Teacher selectedTeacher;
        public Teacher SelectedTeacher
        {
            get { return this.selectedTeacher; }
            set
            {
                this.selectedTeacher = value;
                ApplyFilter();
            }
        }
        public ObservableCollection<Teacher> Teachers { get; set; }

        private Students selectedStudents;
        public Students SelectedStudents
        {
            get { return this.selectedStudents; }
            set
            {
                this.selectedStudents = value;
                ApplyFilter();
            }
        }
        public ObservableCollection<Students> Students { get; set; }

        private Subject selectedSubject;
        public Subject SelectedSubject
        {
            get { return this.selectedSubject; }
            set
            {
                this.selectedSubject = value;
                ApplyFilter();
            }
        }
        public ObservableCollection<Subject> Subjects { get; set; }

        private ActivityTag selectedActivityTag;
        public ActivityTag SelectedActivityTag
        {
            get { return this.selectedActivityTag; }
            set
            {
                this.selectedActivityTag = value;
                ApplyFilter();
            }
        }
        public ObservableCollection<ActivityTag> ActivityTags { get; set; }

        public ActivitiesViewModel()
        {
            Activities = new ObservableCollection<Activity>(Timetable.GetInstance().ActivityList);
            Rooms = new ObservableCollection<Room>(Timetable.GetInstance().GetRoomsList());
            Teachers = new ObservableCollection<Teacher>(Timetable.GetInstance().TeacherList);
            Students = new ObservableCollection<Students>(Timetable.GetInstance().GetStudents());
            Subjects = new ObservableCollection<Subject>(Timetable.GetInstance().SubjectList);
            ActivityTags = new ObservableCollection<ActivityTag>(Timetable.GetInstance().ActivitiyTagsList);
        }

        private DelegateCommand addRoomCommand;
        private DelegateCommand delRoomCommand;

        private DelegateCommand clearFilterCommand;

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


        public ICommand ClearFilterCommand
        {
            get
            {
                if (clearFilterCommand == null)
                {
                    clearFilterCommand = new DelegateCommand(ClearFilter);
                }
                return clearFilterCommand;
            }
        }

        void AddRoom(object obj)
        {
            if (SelectedActivity != null && SelectedAddRoom != null)
            {
                SelectedActivity.PreferredRooms.Add(SelectedAddRoom);                
            }
        }
        void RemoveRoom(object obj)
        {
            if (SelectedActivity != null && SelectedDelRoom != null)
            {
                SelectedActivity.PreferredRooms.Remove(SelectedDelRoom);
            }
        }

        void ClearFilter(object obj)
        {
            SelectedTeacher = null;
            SelectedSubject = null;
            SelectedStudents = null;
            SelectedActivityTag = null;
        }

        private void ApplyFilter()
        {
            
            var activities = from act in Timetable.GetInstance().ActivityList
                             where (act.Teacher.Equals(SelectedTeacher) || SelectedTeacher == null)
                             && (act.Students.Equals(SelectedStudents) || SelectedStudents == null)
                             && (act.Subject.Equals(SelectedSubject) || SelectedSubject == null)
                             && (act.ActivityTag != null && (act.ActivityTag.Equals(SelectedActivityTag)) || SelectedActivityTag == null)
                             select act;

            Activities = new ObservableCollection<Activity>(activities);
            OnPropertyChanged("Activities");
        }
    }
}
