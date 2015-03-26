using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string BuildingName { get; set; }
        public string Name { get; set; }
        private int capacity = -1;
        public int Capacity
        {
            get { return this.capacity; }
            set { this.capacity = value; }
        }

        private ObservableCollection<TimeDayHour> notAvailableTimes = new ObservableCollection<TimeDayHour>();

        public ObservableCollection<TimeDayHour> NotAvailableTimes
        {
            get { return notAvailableTimes; }
            set { notAvailableTimes = value; }
        }

        public Room()
        {
            this.RoomId = App.RandomTool.Next();
        }

        public Room(string name, string buildingName)
        {
            this.Name = name;
            this.BuildingName = buildingName;
            this.RoomId = App.RandomTool.Next();
        }
    }
}
