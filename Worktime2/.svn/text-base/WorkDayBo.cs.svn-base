using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

using HSp.CsEpo;

namespace Worktime2
{
    public class WorkDayBo : IEditableObject, INotifyPropertyChanged
    {
        public WorkDay WorkDayData { set; get; }
        #region Properties
        public string Comment 
        {
            set { WorkDayData.Comment = new HSp.CsEpo.EpoMemo(value); } 
            get {
                
                if (WorkDayData.Comment == null)
                    WorkDayData.Comment = new HSp.CsEpo.EpoMemo("");

                return WorkDayData.Comment.Text; 
            }
        }

        public DateTime DayDate { 
            set 
            { 
                WorkDayData.DayDate = value;
            } 
            get { return WorkDayData.DayDate; } 
        }

        public DateTime InTime { 
            set 
            { 
                WorkDayData.InTime = value;
                OnPropertyChanged("NettoAnw");
            } 
            get { return WorkDayData.InTime; } 
        }

        public DateTime OutTime { 
            set 
            { 
                WorkDayData.OutTime = value;
                OnPropertyChanged("NettoAnw");
            } 
            get { return WorkDayData.OutTime; } 
        }

        public TimeSpan NettoAnw { 
            get { 
                return WorkDayData.NettoAnw; 
            } 
        }

        public TimeSpan PauseTime { 
            set 
            { 
                WorkDayData.PauseTime = value;
                OnPropertyChanged("NettoAnw");
            } 
            get { return WorkDayData.PauseTime; } 
        }

        public HSp.CsEpo.Oid WorkClassOid { 
            set { WorkDayData.WorkClassOid = value; } 
            get { return WorkDayData.WorkClassOid; } 
        }

        public string WorkClassName
        {
            get 
            { 
                return WorkDayData.WorkClassName;
            }
        }

        public ObservableCollection<ProjectOfTheDayBo> ProjectsOfTheDay {set; get; }

        #endregion


        #region Contructors
        public WorkDayBo()
        {
            WorkDayData = new WorkDay();
            ProjectsOfTheDay = new ObservableCollection<ProjectOfTheDayBo>();
            ProjectsOfTheDay.CollectionChanged +=new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ProjectsOfTheDay_CollectionChanged);
        }

        public WorkDayBo(WorkDay WdData)
        {
            WorkDayData = WdData;
            ProjectsOfTheDay = GetProjetcsOfTheDay(WorkDayData);
            ProjectsOfTheDay.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ProjectsOfTheDay_CollectionChanged);
        }

        #endregion

        #region Methods

        //Ein Projektverbindungseintrag wurde aus der Liste entfernt - loesche hier auch den Datensatz aus der DB
        void ProjectsOfTheDay_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (ProjectOfTheDayBo potd in e.OldItems)
                {
                    potd.PotdData.Delete();
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (ProjectOfTheDayBo potd in e.NewItems)
                {
                    potd.PotdData.WorkDayOid = this.WorkDayData.oid;
                }
            }
        }

        private ObservableCollection<ProjectOfTheDayBo> GetProjetcsOfTheDay(WorkDay WorkDayData)
        {
            ObservableCollection<ProjectOfTheDayBo> answ = new ObservableCollection<ProjectOfTheDayBo>();

            Epo linkv = Epo.VerwoByClassName("WorkDayProjectLink");

            string expr = "WorkDayOid='" + WorkDayData.oid + "'";
            ArrayList erg = linkv.Select(expr);

            foreach (WorkDayProjectLink link in erg)
            {
                answ.Add(new ProjectOfTheDayBo(link));
            }

            return answ;
        }


        #endregion

        #region interface implementation

        public void BeginEdit()
        {
            
        }

        public void CancelEdit()
        {
            
        }

        public void EndEdit()
        {
            WorkDayData.Flush();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion



        internal TimeSpan GetAccountableWorktime()
        {
            return WorkDayData.GetAccountableWorkTime();
        }
    }
}
