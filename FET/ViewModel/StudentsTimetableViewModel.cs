using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FET.Data;
using Telerik.Windows.Controls;

namespace FET.ViewModel
{
    class StudentsTimetableViewModel : ViewModelBase
    {
        private List<Data.StudentYear> list;

        public List<Data.StudentYear> List
        {
            get { return list; }
            set { list = value; }
        }

        public StudentsTimetableViewModel()
        {
            this.List = ManageTimetable.GetStudentTimetable(Timetable.GetInstance());
        }
    }
}
