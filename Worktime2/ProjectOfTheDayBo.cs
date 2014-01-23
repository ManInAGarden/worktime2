using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Worktime2
{
    public class ProjectOfTheDayBo : IEditableObject, INotifyPropertyChanged
    {
        public WorkDayProjectLink PotdData;

        public HSp.CsEpo.Oid ProjectOid
        {
            get { return PotdData.ProjectOid; }
            set { PotdData.ProjectOid = value; }
        }

        public HSp.CsEpo.Oid WorkDayOid
        {
            get { return PotdData.WorkDayOid; }
            set { PotdData.WorkDayOid = value; }
        }

        public double Percentage
        {
            get { return PotdData.Percentage; }
            set { PotdData.Percentage = value; }
        }


        public ProjectOfTheDayBo()
        {
            PotdData = new WorkDayProjectLink();
        }

        public ProjectOfTheDayBo(WorkDayProjectLink wdprLink)
        {
            PotdData = wdprLink;
        }

        #region interface IEditableObject
        public void BeginEdit()
        {
            
        }

        public void CancelEdit()
        {
        }

        public void EndEdit()
        {
            PotdData.Flush();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
