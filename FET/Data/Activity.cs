using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class Activity :IComparable<Activity>
    {
        /// <summary>
        /// exclusively used for operations on database
        /// </summary>
        [Key]
        public int ActivityId { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public ActivityTag ActivityTag { get; set; }
       
        public Students Students { get; set; }
        public int Duration { get; set; }
        public int TotalDuration { get; set; }

        private ObservableCollection<Room> preferredRooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> PreferredRooms
        {
            get { return preferredRooms; }
            set { preferredRooms = value; }
        }      
        
        public int ActivityGroupId { get; set; }
        private bool active = true;
        public bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }
        public string Comments { get; set; }

        #region fields or properties used for generating the timetable
        public bool Scheduled { get; set; }
        private int posScheduled = -1;
        public int PozScheduled
        {
            get { return this.posScheduled; }
            set { this.posScheduled = value; }
        }
        public Room AssignedRoom { get; set; }

        private bool forcedScheduled = false;
        public bool ForcedScheduled 
        { 
            get { return forcedScheduled; }
            set { forcedScheduled = value; }
        }
        #endregion

        
        public Activity() 
        {
            this.ActivityId = App.RandomTool.Next();
        }

        /// <summary>
        /// list of activities that can not take place at the same 
        /// time with this activity
        /// </summary>
        public List<Activity> ConcurrentActivities = new List<Activity>();

        
        /// <summary>
        /// acceptable time slots  for this activity based on restrictions
        /// 0- not acceptable 1- acceptable 2-preferable
        /// </summary>
        public int[,] AcceptableTimeSlots;

        /// <summary>
        /// used for recursion
        /// </summary>
        public HashSet<int> UsedSlotPositions = new HashSet<int>();

        public int CountAcceptableSlots()
        {
            int nr = 0;
            for (int i = 0; i < AcceptableTimeSlots.GetLength(0); i++)
            {
                for (int j = 0; j < AcceptableTimeSlots.GetLength(1); j++)
                {
                    if (AcceptableTimeSlots[i, j] > 0)
                    {
                        nr++;
                    }
                }
            }
            return nr;
        }
        public int GetRandomSlotFromAcceptableSlots(out int h)
        {
            int gen = App.RandomTool.Next(this.CountAcceptableSlots());
            int nrOrd = 0;
            for (int i = 0; i < AcceptableTimeSlots.GetLength(0); i++)
            {
                for (int j = 0; j < AcceptableTimeSlots.GetLength(1); j++)
                {
                    if (AcceptableTimeSlots[i, j] > 0)
                    { nrOrd++; }
                    if (nrOrd == gen+1)
                    {
                        h = j;
                        return i;
                    }
                }
            }
            h= -1;
            return -1;
        }
        public int CompareTo(Activity other)
        {
            if (this.CountAcceptableSlots() > other.CountAcceptableSlots()) return 1;
            else if (this.CountAcceptableSlots() < other.CountAcceptableSlots()) return -1;
            else return 0;
        }
    }
}
