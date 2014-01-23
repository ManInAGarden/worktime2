using System;
using System.Collections;
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

using HSp.CsEpo;

namespace Worktime2
{
    /// <summary>
    /// Interaktionslogik für AnwesenheitWindow.xaml
    /// </summary>
    public partial class AnwesenheitWindow : Window
    {
        public DateTime DtStart { set; get; }
        public DateTime DtEnd { set; get; }

        public AnwesenheitWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            anwesenheitTextBlock.Text = FormattedAnwenheitString();
        }

        private string FormattedAnwenheitString()
        {
            string name;
            TimeSpan daySaldo;
            TimeSpan nominalWorkPerDay = WorkDay.NominalWorkTime;
            TimeSpan currWork = new TimeSpan(0, 0, 0);
            string selFrom = DtStart.ToString("yyyy-MM-dd") + " 00:00:00";
            string selTo = DtEnd.ToString("yyyy-MM-dd") + " 23:59:59";
            string diffPositiveBuff, saldoPositiveBuff, saldoNonZeroDaysBuff;

            string outs = "Anwesenheit von "
                + DtStart.ToString("dd.MM.yyyy")
                + " bis "
                + DtEnd.ToString("dd.MM.yyyy")
                + "\r\n\r\n";

            TimeSpan prevSaldo = PreviousSaldo();

            outs += string.Format("Zeitkonto:       {0:f2}\r\n", prevSaldo.TotalHours);
            outs += string.Format("Sollarbeitszeit: {0:f2}\r\n\r\n", WorkDay.NominalWorkTime.TotalHours);

            ArrayList erg = WorkDay.GetDays(DtStart, DtEnd);

            if (erg != null)
            {
                outs += "    Datum  | kommt | geht  |   Pause  | anwesend | Differenz |     Saldo     | Bemerkung";
                outs += "\r\n";
                outs += "-----------+-------+-------+----------+----------+-----------+---------------+---------------";
                outs += "\r\n";

                foreach (WorkDay item in erg)
                {
                    TimeSpan currDayWtime = item.NettoAnw;

                    currWork = currWork.Add(currDayWtime);

                    daySaldo = item.GetAccountableWorkTime();
                    
                    prevSaldo += daySaldo;

                    if (daySaldo >= TimeSpan.Zero)
                        diffPositiveBuff = " ";
                    else
                        diffPositiveBuff = "";

                    if (prevSaldo >= TimeSpan.Zero)
                        saldoPositiveBuff = " ";
                    else
                        saldoPositiveBuff = "";


                    if (Math.Abs(prevSaldo.TotalDays) >= 1)
                    {
                        int numSpaces = 3 - ((int)Math.Log10(Math.Abs(prevSaldo.TotalDays)) + 1);
                        saldoNonZeroDaysBuff = new String(' ', numSpaces);
                    }
                    else
                        saldoNonZeroDaysBuff = "    "; // 4 Leerzeichen

                    outs += item.DayDate.ToString("dd.MM.yyyy")
                        + " | "
                        + item.InTime.ToString("t")
                        + " | "
                        + item.OutTime.ToString("t")
                        + " | "
                        + item.PauseTime.ToString()
                        + " | "
                        + currDayWtime.ToString()
                        + " | "
                        + diffPositiveBuff + daySaldo
                        + " | "
                        + saldoPositiveBuff + saldoNonZeroDaysBuff + prevSaldo
                        + " | "
                        + item.WorkClassName
                        + "\r\n";

                }

                outs += "-----------+-------+-------+----------+----------+-----------+---------------+---------------";
                outs += "\r\n";
                outs += "                                      |";
                outs += string.Format(" {0,8:f2} |", currWork.TotalHours);
            }
            else
            {
                outs = "Keine Eintragungen im Datumsbereich";
            }


            return outs;
        }



        /// <summary>
        /// Rechnet den seit Beginn der Aufzeichnung bis zum DtStart angefallenen Saldo aus
        /// </summary>
        /// <returns>Der bis zum DtStart aufgelaufenen Saldo</returns>
        private TimeSpan PreviousSaldo()
        {
            TimeSpan saldoOfCurrDay = TimeSpan.Zero;
            TimeSpan fullSaldo = new TimeSpan(0, 0, 0);
            TimeSpan nominalWt = WorkDay.NominalWorkTime;

            WorkDay workDayV = Epo.VerwoByClassName("WorkDay") as WorkDay;
            string selFrom = DtStart.ToString("yyyy-MM-dd") + " 00:00:00";
            ArrayList workDays = workDayV.Select("[DayDate] < #" + selFrom + "#", "[DayDate]");

            if (workDays == null)
                return fullSaldo;


            foreach (WorkDay item in workDays)
            {
                fullSaldo += item.GetAccountableWorkTime();
            }

            return fullSaldo;
        }
    }
}
