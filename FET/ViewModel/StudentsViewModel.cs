using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FET.Data;
using Telerik.Windows.Controls;


namespace FET.ViewModel
{
    class StudentsViewModel : ViewModelBase
    {
        private ConstraintsStudents constraint;

        public ConstraintsStudents Constraint
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

        public StudentsViewModel()
        {
            if (Timetable.GetInstance().TimeConstraints.ConstraintsStudents == null)
            {
                Timetable.GetInstance().TimeConstraints.ConstraintsStudents = new ConstraintsStudents();
            }
            this.Hours = Timetable.GetInstance().HoursList;
            this.activityTags = Timetable.GetInstance().ActivitiyTagsList;
            this.Constraint = Timetable.GetInstance().TimeConstraints.ConstraintsStudents;
            
        }
    }
}
