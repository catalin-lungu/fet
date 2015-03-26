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
using FET.ViewModel;

namespace FET
{
    /// <summary>
    /// Interaction logic for DataActivityTags.xaml
    /// </summary>
    public partial class DataActivityTags : Window
    {
        public DataActivityTags()
        {
            InitializeComponent();
            this.DataContext = new ActivityTagsViewModel();
        }
               
        private void Close_Click(object sender, EventArgs e)
        {           
            this.Close();
        }
               
    }
}
