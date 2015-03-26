using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace FET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //new MainWindow().Show();
            base.OnStartup(e);
        }
            
        #region Member variables
        public static App Instance;
        public static String Directory;
        //public event EventHandler LanguageChangedEvent;
        public static bool DataHasChanged { get; set; }
        public static string DataPathFile { get; set; }
        public static Random RandomTool = new Random();
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();
            App.DataHasChanged = false;
            SetLanguageDictionary();
        }

        #endregion


        public static void SetLanguageDictionary(string cult = "")
        {
            if (string.IsNullOrEmpty(cult))
            {
                cult = Thread.CurrentThread.CurrentCulture.ToString();
            }

            ResourceDictionary dict = new ResourceDictionary();
            switch (cult)
            {
                case "ar":
                case "ar-DZ":
                case "ar-BH":
                case "ar-EG":
                case "ar-IQ":
                case "ar-JO":
                case "ar-KW":
                case "ar-LB":
                case "ar-LY":
                case "ar-MA":
                case "ar-OM":
                case "ar-QA":
                case "ar-SA":
                case "ar-SY":
                case "ar-TN":
                case "ar-AE":
                case "ar-YE":
                    dict.Source = new Uri("pack://application:,,,/FET;component/languages/Dictionary.ar.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "en-US":
                    dict.Source = new Uri("pack://application:,,,/FET;component/languages/Dictionary.en-US.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "ro-RO":
                    dict.Source = new Uri("pack://application:,,,/FET;component/languages/Dictionary.ro-RO.xaml", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    dict.Source = new Uri("pack://application:,,,/FET;component/languages/Dictionary.en-US.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            App.Current.Resources.MergedDictionaries.Add(dict);
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (DataHasChanged)
            {
                if (MessageBox.Show("Save data changes before close?", "Save data", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ManageFile.SaveChanges();
                }
            }
        }
    }
}
