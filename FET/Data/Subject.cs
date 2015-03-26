using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class Subject
    {
        [Key]
        public int NameId { get; set; }
        public string Name { get; set; }

        private ObservableCollection<Room> homeRooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> HomeRooms
        {
            get { return homeRooms; }
            set { homeRooms = value; }
        }

        public Subject() { }

        public Subject(string name)
        {
            this.Name = name;
            this.NameId = App.RandomTool.Next();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Subject s = obj as Subject;
            if ((System.Object)s == null)
            {
                string subName = obj as string;
                if (subName == null)
                    return false;
                else
                    return this.Name.Equals(subName);
            }

            // Return true if the fields match:

            return this.Name.Equals(s.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
