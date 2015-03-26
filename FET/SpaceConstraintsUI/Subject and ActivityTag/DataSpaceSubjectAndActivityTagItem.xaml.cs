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
    /// Interaction logic for DataSpaceSubjectAndActivityTagItem.xaml
    /// </summary>
    public partial class DataSpaceSubjectAndActivityTagItem : Window, IDisposable
    {
        bool acceptOnlyOneRoom;
        string subject;
        string activityTag;
        List<Data.Room> rooms = new List<Data.Room>();
        
        public DataSpaceSubjectAndActivityTagItem(bool acceptOnlyOneRoom, string windowTitle)
        {
            InitializeComponent();
            foreach (var s in Timetable.GetInstance().SubjectList)
            {
                subjectComboBox.Items.Add(s.Name);
            }
            foreach (var actTag in Timetable.GetInstance().ActivitiyTagsList)
            {
                activityTagComboBox.Items.Add(actTag.Name);
            }

            foreach (var r in Timetable.GetInstance().GetRoomsList())
            {
                roomComboBox.Items.Add(r.RoomId + " - " + r.Name);
            }
            this.acceptOnlyOneRoom = acceptOnlyOneRoom;
            this.Title = windowTitle;
        }

        public DataSpaceSubjectAndActivityTagItem(bool acceptOnlyOneRoom, string windowTitle, string selectedSubject, string selectedActivityTag, List<Data.Room> selectedRooms)
        {
            InitializeComponent();
            foreach (var s in Timetable.GetInstance().SubjectList)
            {
                subjectComboBox.Items.Add(s.Name);
            }
            subjectComboBox.SelectedItem = selectedSubject;

            foreach (var actTag in Timetable.GetInstance().ActivitiyTagsList)
            {
                activityTagComboBox.Items.Add(actTag.Name);
            }
            activityTagComboBox.SelectedItem = selectedActivityTag;

            foreach (var r in Timetable.GetInstance().GetRoomsList())
            {
                roomComboBox.Items.Add(r.RoomId + " - " + r.Name);
            }
            foreach (var r in selectedRooms)
            {
                listRooms.Items.Add(r.RoomId + " - " + r.Name);
            }
            this.acceptOnlyOneRoom = acceptOnlyOneRoom;
            this.Title = windowTitle;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listRooms.SelectedItem != null)
            {
                listRooms.Items.Remove(listRooms.SelectedItem.ToString());
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (acceptOnlyOneRoom && (listRooms.Items.Count > 0))
            {
                string message = (string)App.Current.TryFindResource("thisConstraintAcceptOnlyOneRoom");
                MessageBox.Show(message);
                return;
            }

            if (roomComboBox.SelectedItem != null)
            {
                string item = roomComboBox.SelectedItem.ToString();
                listRooms.Items.Add(item);
            }
            else
            {
                string message = (string)App.Current.TryFindResource("pleaseSelectARoom");
                MessageBox.Show(message);
            }
        }

        public string GetSubject()
        {
            return this.subject;
        }
        public string GetActivityTag()
        {
            return this.activityTag;
        }
        public List<Data.Room> GetRooms()
        {
            return this.rooms;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.subject = subjectComboBox.SelectedItem != null ? subjectComboBox.SelectedItem.ToString() : "";
            this.activityTag = activityTagComboBox.SelectedItem != null ? activityTagComboBox.SelectedItem.ToString() : "";

            if (this.subject.Equals("") || this.activityTag.Equals(""))
            {
                string message = (string)App.Current.TryFindResource("pleaseSelectASubjectAndAnActivityTag");
                MessageBox.Show(message);
                return;
            }

            if (!(listRooms.Items.Count > 0))
            {
                string message = (string)App.Current.TryFindResource("pleaseAddARoom");
                MessageBox.Show(message);
            }

            foreach (var item in listRooms.Items)
            {
                string[] vals = item.ToString().Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                int roomId = Convert.ToInt32(vals[0].Trim());

                var roomsSel = from r in Timetable.GetInstance().GetRoomsList()
                               where r.RoomId == roomId && r.Name.Equals(vals[1].Trim())
                               select r;
                if (roomsSel.Count() > 0)
                {
                    rooms.Add(roomsSel.First());
                }

            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            this.Close();
        }
    }
}
