using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FET.Data
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Room> rooms = new ObservableCollection<Room>();

        public ObservableCollection<Room> Rooms
        {
            get { return this.rooms; }
            set { this.rooms = value; }
        }

        public Building() 
        {
            this.BuildingId = App.RandomTool.Next();
        }

        public Building(string name)
        {
            this.Name = name;
            this.BuildingId = App.RandomTool.Next();
        }
    }
}
