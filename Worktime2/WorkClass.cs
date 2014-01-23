using System;

namespace Worktime2
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
		base.Name = "<neue Arbeitstagsart>";
	}

	public WorkClass(string connStr) : base(connStr)
	{
	}

	public override void InitializeData()
	{
		WorkClass workClass = new WorkClass();
        workClass.oid = new HSp.CsEpo.Oid("WorkClass_20040806_083218_23416");
		workClass.Name = "GLAZ";
		workClass.OrderSequence = 20;
		workClass.Flush();
		
        workClass = new WorkClass();
        workClass.oid = new HSp.CsEpo.Oid("WorkClass_20040806_083218_17180");
		workClass.Name = "Urlaub";
		workClass.OrderSequence = 40;
		workClass.Flush();

		workClass = new WorkClass();
        workClass.oid = new HSp.CsEpo.Oid("WorkClass_20040806_083218_25747");
		workClass.Name = "Krankheit";
		workClass.OrderSequence = 30;
		workClass.Flush();
		
        workClass = new WorkClass();
        workClass.oid = new HSp.CsEpo.Oid("WorkClass_20040806_083218_2050");
		workClass.Name = "Wochenende";
		workClass.OrderSequence = 50;
		workClass.Flush();

		workClass = new WorkClass();
        workClass.oid = new HSp.CsEpo.Oid("WorkClass_20040806_083218_4073");
		workClass.Name = "Feiertag";
		workClass.OrderSequence = 10;
		workClass.Flush();

        //the empty entry
        workClass = new WorkClass();
        workClass.oid = new HSp.CsEpo.Oid("WorkClass_20120128_000000_0000");
        workClass.Name = "";
        workClass.OrderSequence = 0;
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