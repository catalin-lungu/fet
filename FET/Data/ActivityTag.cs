using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class ActivityTag
    {
        [Key]
        public int ActivityTagId { get; set; }

        public string Name { get; set; }

        private ObservableCollection<Room> preferredRooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> PreferredRooms
        {
            get { return preferredRooms; }
            set { preferredRooms = value; }
        }

        public ActivityTag() { }

        public ActivityTag(string name)
        {
            this.Name = name;
            this.ActivityTagId = App.RandomTool.Next();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            ActivityTag a = obj as ActivityTag;
            if ((System.Object)a == null)
            {
                return false;
            }

            // Return true if the fields match:

            return this.Name.Equals(a.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
        
    }
}
