using HSp.CsEpo;
using System;
using HSp.CsNman;


namespace Worktime2
{
public class WorkBase : EpoCached
{
	private DateTime created;
	private string createdBy;
	private DateTime modified;
	private string modifiedBy;
	private string name;

	public DateTime Created
	{
		get
		{
			return this.created;
		}
		set
		{
			this.created = value;
		}
	}

	public string CreatedBy
	{
		get
		{
			return this.createdBy;
		}
		set
		{
			this.createdBy = value;
		}
	}

	public DateTime Modified
	{
		get
		{
			return this.modified;
		}
		set
		{
			this.modified = value;
		}
	}

	public string ModifiedBy
	{
		get
		{
			return this.modifiedBy;
		}
		set
		{
			this.modifiedBy = value;
		}
	}

	public string Name
	{
		get
		{
			return this.name;
		}
		set
		{
			this.name = value;
		}
	}

	public WorkBase()
	{
		this.Name = "<neu>";
		this.Created = DateTime.Now;
		this.Modified = DateTime.Now;
		this.CreatedBy = Environment.UserName;
		this.ModifiedBy = Environment.UserName;
	}

	public WorkBase(string connStr) : base(connStr)
	{
		this.Created = DateTime.Now;
		this.Modified = DateTime.Now;
		this.CreatedBy = Environment.UserName;
		this.ModifiedBy = Environment.UserName;
	}

	public override bool Delete()
	{
		bool flag = base.Delete();
		string str = string.Concat("DATADELETE:", base.oid.ToString());
		Nman.Instance.Send(new NMessage(this, str, null));
		return flag;
	}

	public override void Flush()
	{
		this.ModifiedBy = Environment.UserName;
		this.Modified = DateTime.Now;
		base.Flush();
		Nman.Instance.Send(new NMessage(this, string.Concat("DATAFLUSH:", base.oid.ClassName), null));
		Nman.Instance.Send(new NMessage(this, string.Concat("DATAFLUSH:", base.oid.ToString()), null));
	}

	public virtual string GetNodeLabel()
	{
		return this.Name;
	}

	protected override void InitSpecialDbLen()
	{
		base.InitSpecialDbLen();
		base.SetDbLen("Name", 50);
		base.SetDbLen("ModifiedBy", 30);
		base.SetDbLen("CreatedBy", 30);
	}

	

	public override string ToString()
	{
		return this.Name;
	}
}
}