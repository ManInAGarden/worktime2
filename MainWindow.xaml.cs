﻿using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


using HSp.CsEpo;
using HSp.CsNman;

namespace Worktime2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool dontRefresh = true;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            fromDatePicker.SelectedDate = new DateTime(now.Year, now.Month, 1);
            toDatePicker.SelectedDate = fromDatePicker.SelectedDate.Value.AddMonths(1);
            string conns = Properties.Settings.Default.DataBaseConn;

            if (string.IsNullOrEmpty(conns))
            {
                MessageBox.Show("Die Datenbankverbindung wurde nicht definiert. Bitte in den Einstellungen mit dem Schlüssel DataBaseConn eintragen.");
            }

            InitializeDb(conns);
            InitWorkClassCombo();
            InitProjectCombo();
            
            dontRefresh = false;
            
            FillDayList();
        }

        /// <summary>
        /// Die Datenbankverbindunge initialsieren
        /// </summary>
        /// <param name="conns"></param>
        private void InitializeDb(string conns)
        {
            WorkDay wd = new WorkDay(conns);
            Project proj = new Project(conns);
            WorkClass wcl = new WorkClass(conns);
            WorkDayProjectLink wpl = new WorkDayProjectLink(conns);
        }


        private void InitWorkClassCombo()
        {
            WorkClass wdv = Epo.VerwoByClassName("WorkClass") as WorkClass;

            ArrayList erg = wdv.Select();
            List<WorkClass> workClasses = new List<WorkClass>(erg.Count);
            foreach (WorkClass wd in erg)
            {
                workClasses.Add(wd);
            }

            DataGridComboBoxColumn cbc = null;
            foreach (DataGridColumn col in workDaysGrid.Columns)
            {
                if (col.Header.ToString() == "Typ")
                    cbc = col as DataGridComboBoxColumn;
            }
            if (cbc == null)
                throw new ApplicationException("Die Spalte mit dem Titel \"Typ\" konnte nicht gefunden werden.");

            cbc.ItemsSource = workClasses;
        }


        private void InitProjectCombo()
        {
            Project projv = Epo.VerwoByClassName("Project") as Project;

            ArrayList erg = projv.Select();
            List<Project> projects = new List<Project>(erg.Count);
            foreach (Project proj in erg)
            {
                projects.Add(proj);
            }

            DataGridComboBoxColumn cbc = null;
            foreach (DataGridColumn col in projectLinksGrid.Columns)
            {
                if (col.Header.ToString() == "Projekt")
                    cbc = col as DataGridComboBoxColumn;
            }
            if (cbc == null)
                throw new ApplicationException("Die Spalte mit dem Titel \"Projekt\" konnte nicht gefunden werden.");

            cbc.ItemsSource = projects;
        }

        private void FillDayList()
        {
            if (dontRefresh)
                return;

            if ((fromDatePicker.SelectedDate.HasValue != true) || (toDatePicker.SelectedDate.HasValue != true))
                return;

            ArrayList erg = WorkDay.GetDays(fromDatePicker.SelectedDate.Value, toDatePicker.SelectedDate.Value);
            ObservableCollection<WorkDayBo> workDaysInList = new ObservableCollection<WorkDayBo>();
            
            foreach (WorkDay wd in erg)
            {
                workDaysInList.Add(new WorkDayBo(wd));
            }
            //Event handler für Löschoperationen hinzufügen
            workDaysInList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(workDaysInList_CollectionChanged);

            //Die Datenquelle für das DataGrid ...
            workDaysGrid.ItemsSource = workDaysInList;
            
        }

        void workDaysInList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(WorkDayBo wdbo in e.OldItems)
                {
                    wdbo.WorkDayData.Delete();
                }
            }
        }

        private void fromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDayList();
        }

        private void toDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDayList();
        }

        private void workDaysGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dontRefresh)
                return;

            WorkDayBo wdbo = null;
            foreach (object o in e.AddedItems)
            {
                wdbo = o as WorkDayBo;
                if(wdbo!=null)
                    FillDetailGrid(wdbo);
            }
        }

        private void FillDetailGrid(WorkDayBo wdbo)
        {
            projectLinksGrid.ItemsSource = wdbo.ProjectsOfTheDay;
        }
       
    }
}
