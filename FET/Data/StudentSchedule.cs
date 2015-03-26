using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class StudentYear
    {
        public string Name { get; set; }
        public string[,] Schedule { get; set; }

        public List<StudentGroup> groups = new List<StudentGroup>();
        public List<StudentGroup> Groups 
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public StudentYear(string name, string[,] schedule)
        {
            this.Name = name;
            this.Schedule = (string[,])schedule.Clone();
        }
                
    }
    public class StudentGroup
    {
        public string Name { get; set; }
        public string[,] Schedule { get; set; }

        public List<StudentSubgroup> subgroups = new List<StudentSubgroup>();
        public List<StudentSubgroup> Subgroups 
        {
            get { return this.subgroups; }
            set { this.subgroups = value; }
        }

        public StudentGroup(string name, string[,] schedule)
        {
            this.Name = name;
            this.Schedule = (string[,])schedule.Clone();
        }
    }

    public class StudentSubgroup
    {
        public string Name { get; set; }
        public string[,] Schedule { get; set; }
        public StudentSubgroup(string name, string[,] schedule)
        {
            this.Name = name;
            this.Schedule = (string[,])schedule.Clone();
        }
    }
}
