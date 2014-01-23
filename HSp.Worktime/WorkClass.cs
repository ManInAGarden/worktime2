using System;

namespace HSp.Worktime
{
public class WorkClass : WorkBase
{
	private int orderSequence;

	public int OrderSequence
	{
		get
		{
			return this.orderSequence;
		}
		set
		{
			this.orderSequence = value;
		}
	}

	public WorkClass()
	{
		base.Name = "<neue Arbeitstagart>";
	}

	public WorkClass(string connStr) : base(connStr)
	{
	}

	public override void InitializeData()
	{
		WorkClass workClass = new WorkClass();
		workClass.Name = "GLAZ";
		workClass.OrderSequence = 20;
		workClass.Flush();
		workClass = new WorkClass();
		workClass.Name = "Urlaub";
		workClass.OrderSequence = 40;
		workClass.Flush();
		workClass = new WorkClass();
		workClass.Name = "Krankheit";
		workClass.OrderSequence = 30;
		workClass.Flush();
		workClass = new WorkClass();
		workClass.Name = "Wochenende";
		workClass.OrderSequence = 50;
		workClass.Flush();
		workClass = new WorkClass();
		workClass.Name = "Feiertag";
		workClass.OrderSequence = 10;
		workClass.Flush();
	}

	protected override void InitSpecialDbLen()
	{
		base.InitSpecialDbLen();
	}

	public override string PreferredOrdering()
	{
		return "OrderSequence";
	}

	public override string ToString()
	{
		return base.Name;
	}
}
}