using HSp.CsEpo;
using System;

namespace HSp.Worktime
{
public class WorkDay : WorkBase
{
	private EpoMemo comment;

	private DateTime dayDate;

	private DateTime inTime;

	private DateTime outTime;

	private TimeSpan pauseTime;

	private Oid workClassOid;

	public TimeSpan BruttoAnw
	{
		get
		{
			DateTime outTime = this.OutTime;
			return outTime.Subtract(this.InTime);
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
		this.InTime = DateTime.Now;
		this.OutTime = DateTime.Now;
		this.DayDate = DateTime.Now;
		this.PauseTime = new TimeSpan(0, 0, 0);
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

	public override string ToString()
	{
		string name;
		TimeSpan timeSpan;

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

		//return string.Format(object[] objArray[4] = name, objArray);
        return name;
	}
}
}