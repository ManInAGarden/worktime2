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
using System.Reflection;

namespace Worktime2
{
    /// <summary>
    /// Interaktionslogik für InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string version = "V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            versionLabel.Content = version;
        }
    }
}
