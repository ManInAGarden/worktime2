using System;
using System.Collections;
using HSp.CsEpo;

namespace Worktime2
{
public class Project : WorkBase
{
	private bool active;
	private DateTime endTag;
	private string sapName;
	private int vorrat;

	public bool Active
	{
		get
		{
			return this.active;
		}
		set
		{
			this.active = value;
		}
	}

	public DateTime EndTag
	{
		get
		{
			return this.endTag;
		}
		set
		{
			this.endTag = value;
		}
	}

	public string SapName
	{
		get
		{
			return this.sapName;
		}
		set
		{
			this.sapName = value;
		}
	}

	public double Verfuegbar
	{
		get
		{
			return this.CalcVerfuegbareStunden();
		}
	}

	public int Vorrat
	{
		get
		{
			return this.vorrat;
		}
		set
		{
			this.vorrat = value;
		}
	}

	public Project()
	{
		base.Name = "<neues Projekt>";
		this.Active = true;
	}

	public Project(string connStr) : base(connStr)
	{
	}

	private double CalcVerfuegbareStunden()
	{
		double percentage = 0;
		if (this.vorrat == 0)
		{
			return 0;
		}

        Epo plVerw = Epo.VerwoByClassName("WorkDayProjectLink");
        ArrayList workDayProjects = plVerw.Select("[projectOid]='" + base.oid + "'");
		foreach (WorkDayProjectLink workDayProjectLink in workDayProjects)
		{
			if (workDayProjectLink.Percentage != 0 && workDayProjectLink.WorkDayOid != null)
			{
				WorkDay workDay = base.ResolveOid(workDayProjectLink.WorkDayOid) as WorkDay;
				if (workDay != null)
				{
					TimeSpan nettoAnw = workDay.NettoAnw;
					double totalHours = nettoAnw.TotalHours;
					percentage = percentage + workDayProjectLink.Percentage / 100 * totalHours;
				}
			}
		}
		return (double)this.vorrat - percentage;
	}

	public override void InitializeData()
	{
	}

	protected override void InitSpecialDbLen()
	{
		base.InitSpecialDbLen();
		base.SetDbLen("SapName", 50);
		base.AddLink("WorkDayProjectLink", "ProjectOid", EnumDelRule.cascade, EnumCopyRule.setnull);
	}

	public override string PreferredOrdering()
	{
		return "Active DESC, Name ASC";
	}

	public override string ToString()
	{
		string str;
		if (!this.Active)
		{
			str = "_";
		}
		else
		{
			str = "";
		}
		str = string.Concat(str, base.Name);
		return str;
	}
}
}