using HSp.CsEpo;
using System;
using System.Collections;


namespace Worktime2
{
public class WorkDay : WorkBase
{
	private EpoMemo comment;
	private DateTime dayDate;
	private DateTime inTime;
	private DateTime outTime;
	private TimeSpan pauseTime;
	private Oid workClassOid;


    internal static TimeSpan NominalWorkTime
    {
        get { return Properties.Settings.Default.NominalWorkTime; }
    }

    public TimeSpan DaySaldo
    {
        get
        {
            TimeSpan answ = new TimeSpan(0);

            WorkClass wc = this.ResolveOid(WorkClassOid) as WorkClass;

            if (wc == null)
                answ = NettoAnw - NominalWorkTime;
            else
            {
                if (wc.Name == "GLAZ")
                    answ = -NominalWorkTime;
                
            }

            return answ;
        }
    }

    public string WorkClassName
    {
        get
        {
            string name;

            if (this.WorkClassOid != null)
            {
                Epo epo = base.ResolveOid(this.WorkClassOid);
                if (epo != null)
                {
                    name = (epo as WorkClass).Name;
                }
                else
                {
                    name = this.WorkClassOid.ToString();
                }
            }
            else
            {
                name = "./.";
            }

            return name;
        }
    }

	public TimeSpan BruttoAnw
	{
		get
		{
			DateTime outTime = this.OutTime;
			return outTime.TimeOfDay.Subtract(this.InTime.TimeOfDay);
		}
	}

	public EpoMemo Comment
	{
		get
		{
			return this.comment;
		}
		set
		{
			this.comment = value;
		}
	}

	public DateTime DayDate
	{
		get
		{
			return this.dayDate;
		}
		set
		{
			this.dayDate = value;
		}
	}

	public DateTime InTime
	{
		get
		{
			return this.inTime;
		}
		set
		{
			this.inTime = value;
		}
	}

	public TimeSpan NettoAnw
	{
		get
		{
			TimeSpan bruttoAnw = this.BruttoAnw;
			return bruttoAnw.Subtract(this.PauseTime);
		}
	}

	public DateTime OutTime
	{
		get
		{
			return this.outTime;
		}
		set
		{
			this.outTime = value;
		}
	}

	public TimeSpan PauseTime
	{
		get
		{
			return this.pauseTime;
		}
		set
		{
			this.pauseTime = value;
		}
	}

	public Oid WorkClassOid
	{
		get
		{
			return this.workClassOid;
		}
		set
		{
			this.workClassOid = value;
		}
	}

	public WorkDay()
	{
        DateTime now = DateTime.Now;
        this.InTime = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
        this.OutTime = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0);
		this.DayDate = now;
		this.PauseTime = new TimeSpan(0, 30, 0);
	}

	public WorkDay(string connStr) : base(connStr)
	{
	}

	protected TimeSpan Anwesend()
	{
		DateTime outTime = this.OutTime;
		TimeSpan timeSpan1 = outTime.Subtract(this.InTime);
		TimeSpan timeSpan2 = timeSpan1.Subtract(this.PauseTime);
		return timeSpan2;
	}

	protected override void InitSpecialDbLen()
	{
		base.InitSpecialDbLen();
		base.SetDbLen("WorkClassOid", Oid.DbSize);
		base.AddJoin("WorkClassOid", "WorkClass");
		base.AddLink("WorkDayProjectLink", "WorkDayOid", EnumDelRule.cascade, EnumCopyRule.copy);
	}

    public override string PreferredOrdering()
    {
        return "DayDate";
    }

    public override void Flush()
    {
        //Den leeren Klassennamen berücksichtigen, dann auch die Verknüpfungzu diesem Spezialeintrag
        //löschen der nur dazu dient Tage ohne Klasse zu erfassen nachdem man vershentlich eine
        //ausgeählt hatte
        if (this.WorkClassName == "")
            workClassOid = null;

        base.Flush();
    }

    /// <summary>
    /// Liefert die für diesen Tag auf das Arbeitszeitkonto zu addierende Arbeitszeit
    /// </summary>
    /// <returns>Anzurechnende Arbeitszeit</returns>
    public TimeSpan GetAccountableWorkTime()
    {
        string name = WorkClassName;
        TimeSpan answ;

        //Die anrechenbare Arbeitszeit ergibt sich an normalen Werktagen, also
        //dann wenn es nicht Samstag oder Sonntag oder Feiertag ist als Differenz
        //zwischen der Soll-Arbeitszeit und den geleisteten Stunden.
        //

        if (string.IsNullOrEmpty(name) || (name == "./."))
        {
            //Wenn es aber ein WE ist dann ziehe nicht die Soll-AZ ab
            if ((DayDate.DayOfWeek == DayOfWeek.Saturday) || (DayDate.DayOfWeek == DayOfWeek.Sunday))
                answ = NettoAnw;
            else
                answ = NettoAnw - NominalWorkTime;
        }
        else if (name == "GLAZ")
        {
            answ = -NominalWorkTime;
        }
        else if ((name == "Feiertag") || (name=="Wochenende"))
        {
            answ = NettoAnw;
        }
        else
            answ = TimeSpan.Zero;


        return answ;
    }

    
	public override string ToString()
	{
		string name;

		if (this.WorkClassOid != null)
		{
			Epo epo = base.ResolveOid(this.WorkClassOid);
			if (epo != null)
			{
				name = (epo as WorkClass).Name;
			}
			else
			{
				name = this.WorkClassOid.ToString();
			}
		}
		else
		{
			name = "./.";
		}

        return name;
	}

    internal static ArrayList GetDays(DateTime fromDate, DateTime toDate)
    {
        string expr = "([DayDate]>=#" + fromDate.ToString("yyyy-MM-dd") + " 00:00:00" + "#)"
            + " AND ([DayDate] <= #" + toDate.ToString("yyyy-MM-dd") + " 23:59:59" + "#)";

        return Epo.VerwoByClassName("WorkDay").Select(expr);
    }

    
}
}