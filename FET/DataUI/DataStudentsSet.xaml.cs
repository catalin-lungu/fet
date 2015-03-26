﻿using System;
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
using FET.DataUI;
using FET.ViewModel;
using Telerik.Windows.Controls;

namespace FET
{
    /// <summary>
    /// Interaction logic for DataStudentsYear.xaml
    /// </summary>
    public partial class DataStudentsSet : Window
    {
        private string nameSelectedItem = string.Empty;

        public DataStudentsSet()
        {
            InitializeComponent();
            this.DataContext = new StudentsSetViewModel();
        }
              
               
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        

    }
}
