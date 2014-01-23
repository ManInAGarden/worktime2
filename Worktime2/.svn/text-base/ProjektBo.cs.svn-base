using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using HSp.CsEpo;

namespace Worktime2
{
    public class ProjektBo : IEditableObject, INotifyPropertyChanged
    {
        public Project ProjectData;

        #region Properties
        public bool Active
        {
            get
            {
                return ProjectData.Active;
            }
            set
            {
                ProjectData.Active = value;
            }
        }

        public string Name
        {
            get
            {
                return ProjectData.Name;
            }
            set
            {
                ProjectData.Name = value;
            }
        }

        public DateTime EndTag
        {
            get
            {
                return ProjectData.EndTag;
            }
            set
            {
                ProjectData.EndTag = value;
            }
        }

        public string SapName
        {
            get
            {
                return ProjectData.SapName;
            }
            set
            {
                ProjectData.SapName = value;
            }
        }

        public double Verfuegbar
        {
            get
            {
                return ProjectData.Verfuegbar;
            }
        }

        public int Vorrat
        {
            get
            {
                return ProjectData.Vorrat;
            }
            set
            {
                ProjectData.Vorrat = value;
            }
        }
        #endregion

        #region Construktoren
        public ProjektBo()
        {
            ProjectData = new Project();
        }


        public ProjektBo(Project proj)
        {
            ProjectData = proj;
        }
        #endregion

        #region Interface IEditableObject
        public void BeginEdit()
        {
            
        }

        public void CancelEdit()
        {
            
        }

        public void EndEdit()
        {
            ProjectData.Flush();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;


        public static ObservableCollection<ProjektBo> GetProjekte()
        {
            Project projV = Epo.VerwoByClassName("Project") as Project;
            ArrayList projects = projV.Select();
            ObservableCollection<ProjektBo> answ = new ObservableCollection<ProjektBo>();

            foreach (Project proj in projects)
            {
                answ.Add(new ProjektBo(proj));
            }

            answ.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(answ_CollectionChanged);

            return answ;
        }

        static void answ_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (ProjektBo pbo in e.OldItems)
                {
                    pbo.ProjectData.Delete();
                }
            }
        }
    }
}
