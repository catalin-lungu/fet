using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FET.Data
{   
    public class Year : Students ,INotifyPropertyChanged
    {
        [Key]
        public override string Name { get; set; }
        
        public List<Group> groups = new List<Group>();

        public List<Group> Groups
        { 
            get { return this.groups; }
            set { this.groups = value; }
        }
                
        public Year()
        {            
        }

        #region methods

        /// <summary>
        /// Check if are the same students or a subgroup
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EqualsOrContains(string obj)
        {
            if (Equals(obj))
            {
                return true;
            }
            else
            {
                foreach (var group in this.Groups)
                {
                    if (group.EqualsOrContains(obj))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// input name as a string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            Year year = obj as Year;
            if (year != null && year.Name.Equals(this.Name))
            {
                return true;
            }

            // If parameter cannot be cast to Point return false.
            string y = obj as string;
            if ((System.Object)y == null)
            {
                return false;
            }

            // Return true if the fields match:
            if (this.Name.Equals(y))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class Group : Students
    {
        [Key]
        public override string Name { get; set; }
        public string YearName { get; set; }
        public List<Subgroup> subgroups = new List<Subgroup>();

        public List<Subgroup> Subgroups 
        {
            get { return this.subgroups; }
            set { this.subgroups = value; }
        }

        public Group(string yearName)
        {
            this.YearName = yearName;
        }

        #region methods
        /// <summary>
        /// Check if are the same students or a subgroup
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool EqualsOrContains(string obj)
        {
            if (Equals(obj))
            {
                return true;
            }
            else
            {
                foreach (var group in this.Subgroups)
                {
                    if (group.Equals(obj))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            Group group = obj as Group;
            if (group != null && group.Name.Equals(this.Name))
            {
                return true;
            }

            // If parameter cannot be cast to Point return false.
            string g = obj as string;
            if ((System.Object)g == null)
            {
                return false;
            }

            // Return true if the fields match:
            if (this.Name.Equals(g))
            {
                return true;
            }
            else
            {
                foreach (var sub in this.Subgroups)
                {
                    if (sub.Equals(g))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    public class Subgroup: Students
    {
        [Key]
        public override string Name { get; set; }
        public string GroupName { get; set; }

        public Subgroup(string groupName)
        {
            this.GroupName = groupName;
        }

        #region methods
        /// <summary>
        /// enter string name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            Subgroup subgroup = obj as Subgroup;
            if (subgroup != null && subgroup.Name.Equals(this.Name))
            {
                return true;
            }

            // If parameter cannot be cast to Point return false.
            string s = obj as string;
            if ((System.Object)s == null)
            {
                return false;
            }

            return this.Name.Equals(s);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
