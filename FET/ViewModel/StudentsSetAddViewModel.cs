using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FET.Data;
using Telerik.Windows.Controls;
namespace FET.ViewModel
{
    class StudentsSetAddViewModel : ViewModelBase
    {
        private List<Data.Year> classList;

        public List<Data.Year> ClassList
        {
            get { return classList; }
            set { classList = value; }
        }

        public StudentsSetAddViewModel()
        {
            if (Timetable.GetInstance().ClassList == null)
            {
                Timetable.GetInstance().ClassList = new List<Year>();
            }

            this.ClassList = Timetable.GetInstance().ClassList;
        }
    }
}
