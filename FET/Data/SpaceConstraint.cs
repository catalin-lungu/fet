using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class SpaceConstraint
    {
        [Key]
        public int SpaceConstraintId { get; set; }

        #region declarations
        public List<ConstraintSubjectActivityTagPreferredRoom> ConstraintSubjectActivityTagPreferredRoomList =
            new List<ConstraintSubjectActivityTagPreferredRoom>();
        public List<ConstraintSubjectActivityTagPreferredRooms> ConstraintSubjectActivityTagPreferredRoomsList =
            new List<ConstraintSubjectActivityTagPreferredRooms>();
        public List<ConstraintActivitiesOccupyMaxDifferentRooms> ConstraintActivitiesOccupyMaxDifferentRoomsList =
            new List<ConstraintActivitiesOccupyMaxDifferentRooms>();       
        public List<ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive> ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList =
            new List<ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive>();
        #endregion

        public SpaceConstraint()
        {
            this.SpaceConstraintId = App.RandomTool.Next();
        }

        public void Clear()
        {
            this.ConstraintSubjectActivityTagPreferredRoomList.Clear();
            this.ConstraintSubjectActivityTagPreferredRoomsList.Clear();
            this.ConstraintActivitiesOccupyMaxDifferentRoomsList.Clear();
            this.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveList.Clear();
        }
    }
    
    public class ConstraintSubjectActivityTagPreferredRoom 
    {
        [Key]
        public int ConstraintSubjectActivityTagPreferredRoomId { get; set; }
        public string Subject { get; set; }
        public string ActivityTag { get; set; }
        public Room Room { get; set; }
        public ConstraintSubjectActivityTagPreferredRoom()
        {
            this.ConstraintSubjectActivityTagPreferredRoomId = App.RandomTool.Next();
        }
    }
           
    public class ConstraintSubjectActivityTagPreferredRooms 
    {
        [Key]
        public int ConstraintSubjectActivityTagPreferredRoomsId { get; set; }
        public string Subject { get; set; }
        public string ActivityTag { get; set; }
        public int NumberOfPreferredRooms { get; set; }
        public List<Room> Rooms = new List<Room>();
        public ConstraintSubjectActivityTagPreferredRooms()
        {
            this.ConstraintSubjectActivityTagPreferredRoomsId = App.RandomTool.Next();
        }
    }
       
    public class ConstraintActivitiesOccupyMaxDifferentRooms 
    {
        [Key]
        public int ConstraintActivitiesOccupyMaxDifferentRoomsId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public int MaxNumberOfDifferentRooms { get; set; }
        public ConstraintActivitiesOccupyMaxDifferentRooms()
        {
            this.ConstraintActivitiesOccupyMaxDifferentRoomsId = App.RandomTool.Next();
        }
    }
    
    public class ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive 
    {
        [Key]
        public int ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId { get; set; }
        public int NumberOfActivities { get; set; }
        public List<int> ActivityIds = new List<int>();
        public ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutive()
        {
            this.ConstraintActivitiesAreInTheSameRoomIfTheyAreConsecutiveId = App.RandomTool.Next();
        }
    }

}
