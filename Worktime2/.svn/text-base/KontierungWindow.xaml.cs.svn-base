using System;
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
using System.Windows.Shapes;
using System.Windows.Markup;

using HSp.CsEpo;
using System.Data;

namespace Worktime2
{
    /// <summary>
    /// Interaktionslogik für KontierungWindow.xaml
    /// </summary>
    public partial class KontierungWindow : Window
    {
        public DateTime DtStart { set; get; }
        public DateTime DtEnd { set; get; }
        public DataTable KontierungenTable { set; get; }

        public KontierungWindow()
        {
            this.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            titleLabel.Content = "Kontierungen von: " + DtStart.ToShortDateString() + " bis: " + DtEnd.ToShortDateString();

            DataTable currTab = GetKontierungData();
            CreateGridColumns(currTab);
            kontierungGrid.ItemsSource = currTab.DefaultView;
            
        }

        private void CreateGridColumns(DataTable currTab)
        {
            DataGridTextColumn currDgCol;
            foreach(DataColumn col in currTab.Columns)
            {
                currDgCol = new DataGridTextColumn();
                currDgCol.Header = col.Caption;
                currDgCol.Binding = new Binding(col.ColumnName);
                if (col.DataType == typeof(DateTime))
                {
                    currDgCol.Binding.StringFormat = "d";
                }
                else if (col.DataType == typeof(double))
                {
                    currDgCol.Binding.StringFormat = "0.00";
                    currDgCol.CellStyle = this.Resources["NumericCellStyle"] as Style;
                    currDgCol.HeaderStyle = this.Resources["NumericHeaderStyle"] as Style;
                }

                kontierungGrid.Columns.Add(currDgCol);
            }
        }

        protected void AddColumn(DataTable table, string columnName, string caption, System.Type dataType)
        {
            DataColumn col = new DataColumn(columnName, dataType);
            col.Caption = caption;
            table.Columns.Add(col);
        }

        protected DataTable GetKontierungData()
        {
            DataTable answ = new DataTable("kontierungen");

            WorkDay wdV = Epo.VerwoByClassName("WorkDay") as WorkDay;
            WorkDayProjectLink wdplV = Epo.VerwoByClassName("WorkDayProjectLink") as WorkDayProjectLink;
            string selFrom = DtStart.ToString("yyyy-MM-dd") + " 00:00:00";
            string selTo = DtEnd.ToString("yyyy-MM-dd") + " 23:59:00";

            ArrayList workDays = wdV.Select("[DayDate] >= #" + selFrom + "# AND [DayDate] <= #" + selTo + "#", "[DayDate]");

            Project proj;
            Dictionary<Oid, double> projectSums = new Dictionary<Oid, double>();
            Dictionary<Oid, int> projectPositions = new Dictionary<Oid, int>();
            List<WorkDayBo> wdbos = new List<WorkDayBo>(workDays.Count);
            List<Project> projects = new List<Project>();

            foreach (WorkDay wd in workDays)
            {
                wdbos.Add(new WorkDayBo(wd));
            }

            int posct = 0;

            //Datumsspalte hinzufügen
            AddColumn(answ, "DayDate", "Datum", typeof(DateTime));

            foreach (WorkDayBo wdbo in wdbos)
            {
                if (wdbo.NettoAnw > TimeSpan.Zero)
                {
                    foreach (ProjectOfTheDayBo podbo in wdbo.ProjectsOfTheDay)
                    {
                        if (podbo.ProjectOid != null)
                        {
                            if (!answ.Columns.Contains(podbo.ProjectOid.OidStr))
                            {
                                proj = wdV.ResolveOid(podbo.ProjectOid) as Project;
                                projectPositions.Add(proj.oid, posct++);

                                AddColumn(answ,
                                    proj.oid.OidStr,
                                    proj.Name,
                                    typeof(double));

                            }

                            if (projectSums.ContainsKey(podbo.ProjectOid))
                            {
                                projectSums[podbo.ProjectOid] += (podbo.Percentage / 100.0) * wdbo.NettoAnw.TotalHours;
                            }
                            else
                            {
                                projectSums.Add(podbo.ProjectOid, 0);
                            }
                        }
                    }
                }
            }

            int maxLines = workDays.Count;

            int curColIdx;
            object[] rowData = new object[answ.Columns.Count];

            //Nun die Tabelle aufbauen
            foreach (WorkDayBo wdbo in wdbos)
            {   
                rowData[0] = wdbo.DayDate;
                //Den Rest der Zeile mit 0.0 initialisieren
                for (int i = 1; i < rowData.Length; i++)
                    rowData[i] = 0.0;

                if (wdbo.NettoAnw > TimeSpan.Zero)
                {
                    foreach (ProjectOfTheDayBo podbo in wdbo.ProjectsOfTheDay)
                    {
                        if (podbo.ProjectOid != null)
                        {
                            curColIdx = answ.Columns.IndexOf(podbo.ProjectOid.OidStr);
                            if (curColIdx < 0)
                                throw new ApplicationException("SYSERR: Projektspalte nicht gefunden");

                            rowData[curColIdx] = (podbo.Percentage / 100.0) * wdbo.NettoAnw.TotalHours;
                        }
                    }
                }
                
                answ.Rows.Add(rowData);
                
            }

            //Nun noch die Summenzeile
            rowData[0] = null;
            //Den Rest der Zeile mit 0.0 initialisieren
            for (int i = 1; i < rowData.Length; i++)
                rowData[i] = 0.0;

            foreach (Oid projSumOid in projectSums.Keys)
            {
                
                curColIdx = answ.Columns.IndexOf(projSumOid.OidStr);
                if (curColIdx < 0)
                    throw new ApplicationException("SYSERR: Projektspalte nicht gefunden");

                rowData[curColIdx] = projectSums[projSumOid];
                
            }

            //answ.Rows.Add(rowData);

            return answ;
        }
    }
}
