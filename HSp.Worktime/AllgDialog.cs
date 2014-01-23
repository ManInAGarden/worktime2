using System.Windows.Forms;
using System.ComponentModel;
using System;
using HSp.CsEpo;
using System.Drawing;
using System.Collections;
using HSp.CsNman;
using System.Diagnostics;

namespace HSp.Worktime
{
public class AllgDialog : Form
{
	protected internal Button closeBU;

	private Container components;

	protected internal Button delBU;

	protected internal Button dupBU;

	protected internal ListBox entriesLB;

	protected internal Button goBU;

	protected internal Label labelListTitle;

	private string listTitle;

	protected internal Button newBU;

	protected string orderClause;

	private string title;

	private Epo verwO;

	protected string whereClause;

	public string OrderClause
	{
		get
		{
			return this.orderClause;
		}
		set
		{
			this.orderClause = value;
		}
	}

	public string WhereClause
	{
		get
		{
			return this.whereClause;
		}
		set
		{
			this.whereClause = value;
		}
	}

	public AllgDialog(Form owner, string title, string listTitle, Epo verwO)
	{
		base.Owner = owner;
		this.listTitle = listTitle;
		this.title = title;
		this.verwO = verwO;
		this.InitializeComponents();
	}

	private void CloseBu_Click(object sender, EventArgs e)
	{
		base.Close();
	}

	private void DelBU_Click(object sender, EventArgs e)
	{
		Epo selectedItem = this.entriesLB.SelectedItem as Epo;
		DialogResult dialogResult = MessageBox.Show(this, "Soll das Element wirklich gelöscht werden?", "Rückfrage", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
		if (dialogResult != DialogResult.Yes || selectedItem.Delete())
		{
			this.entriesLB.Items.Remove(selectedItem);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && this.components != null)
		{
			this.components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void DupBU_Click(object sender, EventArgs e)
	{
		Epo selectedItem = this.entriesLB.SelectedItem as Epo;
		Epo epo = selectedItem.Clone() as Epo;
		if (epo != null)
		{
			epo.Flush();
		}
	}

	private void EntriesLB_DoubleClick(object sender, EventArgs e)
	{
	}

	private void EntriesLB_SelectedValueChanged(object sender, EventArgs e)
	{
		if ((sender as ListBox).SelectedItem != null)
		{
			this.goBU.Enabled = true;
			this.delBU.Enabled = true;
			this.dupBU.Enabled = true;
		}
		else
		{
			this.goBU.Enabled = false;
			this.delBU.Enabled = false;
			this.dupBU.Enabled = false;
		}
	}

	private void GoBU_Click(object sender, EventArgs e)
	{
		Form form = null;
		Epo selectedItem = this.entriesLB.SelectedItem as Epo;
		if (form != null)
		{
			form.Show();
		}
	}

	private void InitializeComponents()
	{
		this.components = new Container();
		this.labelListTitle = new Label();
		this.labelListTitle.Location = new Point(8, 13);
		this.labelListTitle.TabIndex = 1;
		this.labelListTitle.Size = new Size(64, 16);
		this.labelListTitle.Text = this.listTitle;
		this.entriesLB = new ListBox();
		this.entriesLB.Location = new Point(8, 30);
		this.entriesLB.TabIndex = 10;
		this.entriesLB.Size = new Size(275, 200);
		this.entriesLB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		this.entriesLB.SelectedValueChanged += new EventHandler(this.EntriesLB_SelectedValueChanged);
		this.entriesLB.add_DoubleClick(new EventHandler(this.EntriesLB_DoubleClick));
		this.closeBU = new Button();
		this.closeBU.Text = "Schließen";
		this.closeBU.Location = new Point(205, 235);
		this.closeBU.TabIndex = 2;
		AnchorStyles anchorStyle = AnchorStyles.Bottom | AnchorStyles.Right;
		this.closeBU.Anchor = anchorStyle;
		this.closeBU.add_Click(new EventHandler(this.CloseBu_Click));
		this.goBU = new Button();
		this.goBU.Enabled = false;
		this.goBU.Text = ">";
		this.goBU.Location = new Point(90, 8);
		this.goBU.Size = new Size(20, 20);
		this.goBU.Anchor = anchorStyle;
		this.goBU.add_Click(new EventHandler(this.GoBU_Click));
		this.delBU = new Button();
		this.delBU.Enabled = false;
		this.delBU.Text = "X";
		this.delBU.Location = new Point(110, 8);
		this.delBU.Size = new Size(20, 20);
		this.delBU.Anchor = anchorStyle;
		this.delBU.add_Click(new EventHandler(this.DelBU_Click));
		this.newBU = new Button();
		this.newBU.Text = "*";
		this.newBU.Location = new Point(130, 8);
		this.newBU.Size = new Size(20, 20);
		this.newBU.Anchor = anchorStyle;
		this.newBU.add_Click(new EventHandler(this.NewBU_Click));
		this.dupBU = new Button();
		this.dupBU.Enabled = false;
		this.dupBU.Text = "2";
		this.dupBU.Location = new Point(150, 8);
		this.dupBU.Size = new Size(20, 20);
		this.dupBU.Anchor = anchorStyle;
		this.dupBU.add_Click(new EventHandler(this.DupBU_Click));
		base.Text = this.listTitle;
		base.Size = new Size(300, 300);
		base.Controls.Add(this.labelListTitle);
		base.Controls.Add(this.entriesLB);
		base.Controls.Add(this.closeBU);
		base.Controls.Add(this.goBU);
		base.Controls.Add(this.newBU);
		base.Controls.Add(this.delBU);
		base.Controls.Add(this.dupBU);
	}

	private void InitializeData()
	{
		ArrayList arrayLists;
		NDelegate nDelegate1 = new NDelegate(this.UpdateList);
		NDelegate nDelegate2 = new NDelegate(this.UpdateListAfterDelete);
		if (this.whereClause != null)
		{
			arrayLists = this.verwO.Select(this.whereClause, this.orderClause);
			Debug.Assert(arrayLists != null);
		}
		else
		{
			arrayLists = this.verwO.Select();
		}
		this.entriesLB.BeginUpdate();
		this.entriesLB.Items.Clear();
		for (int i = 0; i < arrayLists.Count; i++)
		{
			this.entriesLB.Items.Add(arrayLists[i]);
			Epo item = arrayLists[i] as Epo;
			if (i == 0)
			{
				string str1 = string.Concat("DATAFLUSH:", item.oid.ClassName);
				Nman.Instance.Register(str1, this, nDelegate1);
			}
			string str2 = string.Concat("DATADELETE:", item.oid.ToString());
			Nman.Instance.Register(str2, this, nDelegate2);
		}
		this.entriesLB.EndUpdate();
	}

	private void NewBU_Click(object sender, EventArgs e)
	{
		Epo epo = this.verwO.New();
		epo.Flush();
	}

	public void ShowAllg(string where, string order)
	{
		Debug.Assert(where != null);
		this.whereClause = where;
		this.orderClause = order;
		this.ShowAllg();
	}

	public void ShowAllg()
	{
		this.InitializeData();
		base.Show();
	}

	public void UpdateData()
	{
		this.InitializeData();
	}

	public void UpdateList(NMessage msg)
	{
		Nman.Instance.UnRegister(this);
		this.InitializeData();
	}

	public void UpdateListAfterDelete(NMessage msg)
	{
		this.entriesLB.Items.Remove(msg.Sender);
		Nman.Instance.UnRegister(msg.Title);
	}
}
}