using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FET.Data;
using Telerik.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace FET.ViewModel
{
    class TeachersViewModel : ViewModelBase
    {
        private ConstraintsTeachers constraint;

        public ConstraintsTeachers Constraint
        {
            get 
            {
                return constraint;
            }
            set 
            {
                if (this.constraint != value)
                {
                    this.constraint = value;
                    OnPropertyChanged("Constraint");
                }
            }
        }

        private List<string> hours;
        public List<string> Hours
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

        private ActivityTag activityTagClass;
    

        public ActivityTag ActivityTagClass
        {
            get { return activityTagClass; }
            set { activityTagClass = value; }
        }
        

        private List<ActivityTag> activityTags;

        public List<ActivityTag> ActivityTags
        {
            get { return activityTags; }
            set { activityTags = value; }
        }
        

        public ICommand OKButtonClick { get; set; }
        public ICommand CloseButtonClick { get; set; }

        public TeachersViewModel()
        {
            if (Timetable.GetInstance().TimeConstraints.ConstraintsTeachers == null)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintsTeachers = new ConstraintsTeachers();
            }
            this.Hours = Timetable.GetInstance().HoursList;
            this.activityTags = Timetable.GetInstance().ActivitiyTagsList;
            this.Constraint = Timetable.GetInstance().TimeConstraints.ConstraintsTeachers;

            this.OKButtonClick = new DelegateCommand(this.OnOKButtonClick, this.CanClickExecute);

        }


        private void OnOKButtonClick(object obj)
        {
            //Constraint.TeachersMaxDaysPerWeek = ((TimeConstraintsTeachers)obj).TeachersMaxDaysPerWeek;
        }
        private bool CanClickExecute(object obj)
        {
            return obj != null;
        }
 
    }
}
