using System;
using System.Collections.ObjectModel;
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
using System.Windows.Markup;


namespace Worktime2
{
    /// <summary>
    /// Interaktionslogik für ProjekteWindow.xaml
    /// </summary>
    public partial class ProjekteWindow : Window
    {
        public ProjekteWindow()
        {
            this.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillGrid();
        }


        protected void FillGrid()
        {
            ObservableCollection<ProjektBo> projekte = ProjektBo.GetProjekte();

            projekteDátaGrid.ItemsSource = projekte;
        }
    }
}
