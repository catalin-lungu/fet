using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FET.Data;

namespace FET.SpaceConstraintsUI.Subject_and_ActivityTag
{
    /// <summary>
    /// Interaction logic for DataSpaceSubjectAndActivityTag.xaml
    /// </summary>
    public partial class DataSpaceSubjectAndActivityTag : Window
    {
        string constraintName;
        public DataSpaceSubjectAndActivityTag(List<ConstraintSubjectActivityTagPreferredRoom> list)
        {
            InitializeComponent();
            foreach (var constr in list)
            {
                listConstraints.Items.Add((string)App.Current.TryFindResource("subject") + ": " + constr.Subject +
                    " - " + (string)App.Current.TryFindResource("activityTag") + ": " + constr.ActivityTag +
                    " - " + (string)App.Current.TryFindResource("room") + ": " + constr.Room.Name);
            }
            constraintName = "ConstraintSubjectActivityTagPreferredRoom";
            this.Title = (string)App.Current.TryFindResource("subjectAndActivityTagPreferredRoom");
        }

        public DataSpaceSubjectAndActivityTag(List<ConstraintSubjectActivityTagPreferredRooms> list)
        {
            InitializeComponent();
            foreach (var constr in list)
            {
                listConstraints.Items.Add("Subject: " + constr.Subject +
                    " - " + (string)App.Current.TryFindResource("activityTag") + ": " + constr.ActivityTag +
                    " - " + (string)App.Current.TryFindResource("numberOfRooms") + ": " + constr.Rooms.Count);
            }
            constraintName = "ConstraintSubjectActivityTagPreferredRooms";
            this.Title = (string)App.Current.TryFindResource("subjectAndActivityTagPreferredRooms");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string subject;
            string activityTag;
            List<Data.Room> rooms = new List<Data.Room>();

            bool acceptOnlyOneRoom = false;
            if (constraintName.Equals("ConstraintSubjectPreferredRoom"))
            {
                acceptOnlyOneRoom = true;
            }

            string windowTitle;
            if (acceptOnlyOneRoom)
            {
                windowTitle = (string)App.Current.TryFindResource("addSubjectAndActivityTagPreferredRoom");
            }
            else
            {
                windowTitle = (string)App.Current.TryFindResource("addSubjectAndActivityTagPreferredRooms");
            }
            using (DataSpaceSubjectAndActivityTagItem window = new DataSpaceSubjectAndActivityTagItem(acceptOnlyOneRoom, windowTitle))
            {
                window.ShowDialog();

                subject = window.GetSubject();
                activityTag = window.GetActivityTag();
                rooms = window.GetRooms();
            }


            if (rooms.Count > 0)
            {
                switch (constraintName)
                {
                    case "ConstraintSubjectActivityTagPreferredRoom":
                        Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Add(
                            new Data.ConstraintSubjectActivityTagPreferredRoom()
                            {
                                Room = rooms.First(),
                                Subject = subject,
                                ActivityTag = activityTag
                            });
                        listConstraints.Items.Add((string)App.Current.TryFindResource("subject") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Last().Subject +
                            " - " + (string)App.Current.TryFindResource("activityTag") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Last().ActivityTag +
                            " - " + (string)App.Current.TryFindResource("room") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Last().Room.Name);
                        break;
                    case "ConstraintSubjectActivityTagPreferredRooms":
                        Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Add(
                            new Data.ConstraintSubjectActivityTagPreferredRooms()
                            {
                                Rooms = rooms,
                                Subject = subject,
                                ActivityTag = activityTag
                                
                            });
                        listConstraints.Items.Add("Subject: " + Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Last().Subject +
                            " - " + (string)App.Current.TryFindResource("activityTag") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Last().ActivityTag +
                            " - " + (string)App.Current.TryFindResource("numberOfRooms") + ": " + Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Last().Rooms.Count);
                        break;
                }

            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }

            string subject = conString.Substring(0, conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":"));
            subject = subject.Substring(subject.IndexOf(":") + 1).Trim();
            string activityTag;
            if (conString.Contains("- " + (string)App.Current.TryFindResource("room") + ":"))
            {
                activityTag = conString.Substring(conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":") +
                    ((string)App.Current.TryFindResource("activityTag")).Length + 3);
                activityTag = activityTag.Substring(0, activityTag.IndexOf("- " + (string)App.Current.TryFindResource("room") + ":")).Trim();
            }
            else
            {
                activityTag = conString.Substring(conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":") +
                    ((string)App.Current.TryFindResource("activityTag")).Length + 3);
                activityTag = activityTag.Substring(0, activityTag.IndexOf("- " + (string)App.Current.TryFindResource("numberOfRooms") + ":")).Trim();
            }
                         

            List<Data.Room> rooms = new List<Data.Room>();
            if (constraintName.Equals("ConstraintSubjectActivityTagPreferredRoom"))
            {
                var roomsList = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList
                                where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                                select c.Room;
                if (roomsList.Count() > 0)
                {
                    rooms.Add(roomsList.First());
                }

            }
            else if (constraintName.Equals("ConstraintSubjectActivityTagPreferredRooms"))
            {
                var roomsLists = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList
                                 where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                                 select c.Rooms;
                foreach (var r in roomsLists.First())
                {
                    rooms.Add(r);
                }
            }

            bool acceptOnlyOneRoom = false;
            if (constraintName.Equals("ConstraintSubjectActivityTagPreferredRoom"))
            {
                acceptOnlyOneRoom = true;
            }

            string windowTitle;
            if (acceptOnlyOneRoom)
            {
                windowTitle = (string)App.Current.TryFindResource("addSubjectAndActivityTagPreferredRoom");
            }
            else
            {
                windowTitle = (string)App.Current.TryFindResource("addSubjectAndActivityTagPreferredRooms");
            }

            string newSubject;
            string newActivityTag;
            List<Data.Room> newRooms;
            using (DataSpaceSubjectAndActivityTagItem window = new DataSpaceSubjectAndActivityTagItem(acceptOnlyOneRoom, windowTitle, subject, activityTag ,rooms))
            {
                window.ShowDialog();
                newSubject = window.GetSubject();
                newActivityTag = window.GetActivityTag();
                newRooms = window.GetRooms();
            }

            if (!(newRooms.Count() > 0))
            {
                return;
            }

            switch (constraintName)
            {
                case "ConstraintSubjectActivityTagPreferredRoom":
                    var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList
                              where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                              select c;
                    if (con.Count() > 0)
                    {
                        var constr1 = con.First();
                        constr1.Subject = newSubject;
                        constr1.ActivityTag = newActivityTag;
                        constr1.Room = newRooms.First();


                        listConstraints.Items.Remove(conString);
                        listConstraints.Items.Add((string)App.Current.TryFindResource("subject") + ": " + constr1.Subject +
                            " - " + (string)App.Current.TryFindResource("activityTag") + ": " + constr1.ActivityTag +
                            " - " + (string)App.Current.TryFindResource("room") + ": " + constr1.Room.Name);
                    }

                    break;
                case "ConstraintSubjectActivityTagPreferredRooms":
                    var con2 = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList
                               where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                               select c;
                    if (con2.Count() > 0)
                    {
                        var constr2 = con2.First();
                        constr2.Subject = newSubject;
                        constr2.ActivityTag = newActivityTag;
                        constr2.Rooms = newRooms;

                        listConstraints.Items.Remove(conString);
                        listConstraints.Items.Add((string)App.Current.TryFindResource("subject") + ": " + constr2.Subject +
                            " - " + (string)App.Current.TryFindResource("activityTag") + ": " + constr2.ActivityTag +
                            " - " + (string)App.Current.TryFindResource("numberOfRooms") + ": " + constr2.Rooms.Count);
                    }


                    break;

            }
        }


        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }

            string subject = conString.Substring(0, conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":"));
            subject = subject.Substring(subject.IndexOf(":") + 1).Trim();
            string activityTag;
            if (conString.Contains("- Room:"))
            {
                activityTag = conString.Substring(conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":") +
                    ((string)App.Current.TryFindResource("activityTag")).Length+3);
                activityTag = activityTag.Substring(0, activityTag.IndexOf("- " + (string)App.Current.TryFindResource("room") + ":")).Trim();
            }
            else
            {
                activityTag = conString.Substring(conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":") +
                    ((string)App.Current.TryFindResource("activityTag")).Length+3);
                activityTag = activityTag.Substring(0, activityTag.IndexOf("- " + (string)App.Current.TryFindResource("numberOfRooms") + ":")).Trim();
            }

            switch (constraintName)
            {
                case "ConstraintSubjectActivityTagPreferredRoom":
                    var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList
                              where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                              select c;
                    if (con.Count() > 0)
                    {
                        Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList.Remove(con.First());
                        listConstraints.Items.Remove(conString);
                    }
                    break;
                case "ConstraintSubjectActivityTagPreferredRooms":
                    var con2 = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList
                               where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                               select c;
                    if (con2.Count() > 0)
                    {
                        Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList.Remove(con2.First());
                        listConstraints.Items.Remove(conString);
                    }
                    break;

            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void listConstraints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            constraintInfo.Text = "";
            string conString = listConstraints.SelectedItem != null ? listConstraints.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrEmpty(conString))
            {
                return;
            }

            string subject = conString.Substring(0, conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":"));
            subject = subject.Substring(subject.IndexOf(":") + 1).Trim();
            string activityTag;
            if (conString.Contains("- " + (string)App.Current.TryFindResource("room") + ":"))
            {
                activityTag = conString.Substring(conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":") + 15);
                activityTag = activityTag.Substring(0, activityTag.IndexOf("- " + (string)App.Current.TryFindResource("room") + ":")).Trim();
            }
            else
            {
                activityTag = conString.Substring(conString.IndexOf("- " + (string)App.Current.TryFindResource("activityTag") + ":") + 15);
                activityTag = activityTag.Substring(0, activityTag.IndexOf("- " + (string)App.Current.TryFindResource("numberOfRooms") + ":")).Trim();
            }

            switch (constraintName)
            {
                case "ConstraintSubjectActivityTagPreferredRoom":
                    var con = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomList
                              where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                              select c;
                    if (con.Count() > 0)
                    {
                        var constr1 = con.First();
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + constr1.Subject + Environment.NewLine +
                            (string)App.Current.TryFindResource("activityTag") + ": " + constr1.ActivityTag + Environment.NewLine +
                            (string)App.Current.TryFindResource("room") + ": " + constr1.Room.Name + Environment.NewLine;
                    }
                    break;
                case "ConstraintSubjectActivityTagPreferredRooms":
                    var con2 = from c in Timetable.GetInstance().SpaceConstraints.ConstraintSubjectActivityTagPreferredRoomsList
                               where c.Subject.Equals(subject) && c.ActivityTag.Equals(activityTag)
                               select c;
                    if (con2.Count() > 0)
                    {
                        var constr2 = con2.First();
                        constraintInfo.Text += (string)App.Current.TryFindResource("subject") + ": " + constr2.Subject + Environment.NewLine;
                        constraintInfo.Text += (string)App.Current.TryFindResource("activityTag") + ": " + constr2.ActivityTag + Environment.NewLine;
                        constraintInfo.Text += "--------------------" + Environment.NewLine;
                        foreach (var r in constr2.Rooms)
                        {
                            constraintInfo.Text += (string)App.Current.TryFindResource("building") + ": " + r.BuildingName + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("room") + ": " + r.Name + Environment.NewLine;
                            constraintInfo.Text += (string)App.Current.TryFindResource("capacity") + ": " + r.Capacity + Environment.NewLine;
                            constraintInfo.Text += "--------------------" + Environment.NewLine;
                        }


                    }
                    break;

            }
        }
  
    }
}
