using System;
using HSp.CsEpo;
using System.Collections;

namespace HSp.Worktime
{
public class WorkDayProjectLink : WorkBase
{
	private double percentage;

	private Oid projectOid;

	private Oid workDayOid;

	public double Percentage
	{
		get
		{
			return this.percentage;
		}
		set
		{
			this.percentage = value;
		}
	}

	public Oid ProjectOid
	{
		get
		{
			return this.projectOid;
		}
		set
		{
			this.projectOid = value;
		}
	}

	public Oid WorkDayOid
	{
		get
		{
			return this.workDayOid;
		}
		set
		{
			this.workDayOid = value;
		}
	}

	public WorkDayProjectLink()
	{
		base.Name = "n.n.";
	}

	public WorkDayProjectLink(string connStr) : base(connStr)
	{
	}

	public override void Flush()
	{
		double percentage = 0;
		if (this.Percentage == 0 && this.WorkDayOid != null)
		{
			WorkDayProjectLink workDayProjectLink1 = Epo.VerwoByClassName("WorkDayProjectLink") as WorkDayProjectLink;
            if (workDayProjectLink1 == null)
                return;

			ArrayList arrayLists = workDayProjectLink1.Select(string.Concat("WorkDayOid='", this.WorkDayOid.OidStr, "'"));
			foreach (WorkDayProjectLink workDayProjectLink2 in arrayLists)
			{
				percentage = percentage + workDayProjectLink2.Percentage;
			}
			if (percentage <= 100 && percentage >= 0)
			{
				this.Percentage = 100 - percentage;
			}
		}
		base.Flush();
	}

	public override void InitializeData()
	{
	}

	protected override void InitSpecialDbLen()
	{
		base.InitSpecialDbLen();
		base.SetDbLen("WorkDayOid", Oid.DbSize);
		base.AddJoin("WorkDayOid", "WorkDay");
		base.SetDbLen("ProjectOid", Oid.DbSize);
		base.AddJoin("ProjectOid", "Project");
	}

	public override string PreferredOrdering()
	{
		return "Percentage";
	}

	public override string ToString()
	{
		string str1 = "?";
		string str2 = "?";
		string str3 = string.Format("{0:000.0}%", this.Percentage);
		if (this.ProjectOid != null)
		{
			Project project = base.ResolveOid(this.ProjectOid) as Project;
			if (project != null)
			{
				if (project.Name != null)
				{
					str1 = project.ToString();
				}
				else
				{
					str1 = "<leerer Projektname>";
				}
			}
		}
		if (this.WorkDayOid != null)
		{
			WorkDay workDay = base.ResolveOid(this.WorkDayOid) as WorkDay;
			if (workDay != null)
			{
				TimeSpan nettoAnw = workDay.NettoAnw;
				double totalHours = nettoAnw.TotalHours * this.Percentage / 100;
				str2 = string.Format("[{0,-3:G3}h]", totalHours);
			}
		}
		string[] strArrays = new string[5];
		strArrays[0] = str3;
		strArrays[1] = " ";
		strArrays[2] = str2;
		strArrays[3] = " ";
		strArrays[4] = str1;
		str3 = string.Concat(strArrays);
		return str3;
	}
}
}