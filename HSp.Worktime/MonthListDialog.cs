using System.Windows.Forms;
using System.ComponentModel;
using System;
using System.Diagnostics;
using System.Collections;
using HSp.CsEpo;
using System.Resources;
using System.Drawing;

namespace HSp.Worktime
{
public class MonthListDialog : Form
{
	protected internal Button closeB;

	private Container components;

	private string dialogTitel;

	protected DateTime dtEnd;

	protected DateTime dtStart;

	protected TextBox infoTB;

	public string DialogTitel
	{
		get
		{
			return this.dialogTitel;
		}
		set
		{
			this.dialogTitel = value;
		}
	}

	public MonthListDialog(Form owner, DateTime start, DateTime end, string titel) : base()
	{
		this.dialogTitel = "Dialog";

		Debug.Assert(titel != null);
		this.DialogTitel = titel;
		base.Owner = owner;
		this.dtStart = start;
		this.dtEnd = end;
		this.InitializeComponents();
	}

	protected virtual void AddSpecialControls(ArrayList toAdd, ref int ypos, int labW, int txtW, int lineH, int elemH)
	{
	}

	protected virtual void CloseB_Click(object sender, EventArgs e)
	{
		base.Dispose();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && this.components != null)
		{
			this.components.Dispose();
		}
		base.Dispose(disposing);
	}

	protected void FillCombo(ComboBox cb, Epo verwo, Oid oid, IComparer comp)
	{
		cb.BeginUpdate();
		ArrayList arrayLists = verwo.Select(comp);
		for (int i = 0; i < arrayLists.Count; i++)
		{
			Epo item = arrayLists[i] as Epo;
			cb.Items.Add(item);
			if (oid != null)
			{
				if (item.oid.Equals(oid))
				{
					cb.SelectedItem = arrayLists[i];
				}
			}
		}
		cb.EndUpdate();
	}

	protected void FillCombo(ComboBox cb, Epo verwo, Oid oid)
	{
		cb.BeginUpdate();
		ArrayList arrayLists = verwo.Select();
		for (int i = 0; i < arrayLists.Count; i++)
		{
			Epo item = arrayLists[i] as Epo;
			cb.Items.Add(item);
			if (oid != null)
			{
				if (item.oid.Equals(oid))
				{
					cb.SelectedItem = arrayLists[i];
				}
			}
		}
		cb.EndUpdate();
	}

	protected void FillList(ListBox lb, Epo verwo, Oid oid)
	{
		lb.BeginUpdate();
		ArrayList arrayLists = verwo.Select();
		for (int i = 0; i < arrayLists.Count; i++)
		{
			Epo item = arrayLists[i] as Epo;
			lb.Items.Add(item);
			if (oid != null)
			{
				if (item.oid.Equals(oid))
				{
					lb.SelectedItem = arrayLists[i];
				}
			}
		}
		lb.EndUpdate();
	}

	protected void FillList(ListBox lb, Epo verwo, string where)
	{
		lb.BeginUpdate();
		ArrayList arrayLists = verwo.Select(where);
		for (int i = 0; i < arrayLists.Count; i++)
		{
			Epo item = arrayLists[i] as Epo;
			lb.Items.Add(item);
		}
		lb.EndUpdate();
	}

	protected virtual void FillWithData()
	{
		string name;
		TimeSpan currSpan;
		string str1;
		string str2;
		string[] strArrays;
		WorkDay workDay = MainApp.workDayVerwo;
		TimeSpan nominalWorkPerDay = TimeSpan.FromHours(MainApp.NominalWorkTime);
		TimeSpan currWork = new TimeSpan(0, 0, 0);
		string selFrom = string.Concat(this.dtStart.ToString("yyyy-MM-dd"), " 00:00:00");
		string selTo = string.Concat(this.dtEnd.ToString("yyyy-MM-dd"), " 23:59:59");
        //string[] strArrays2 = new string[5];
        //strArrays2[0] = "Anwesenheit von ";
        //strArrays2[1] = this.dtStart.ToString("dd.MM.yyyy");
        //strArrays2[2] = " bis ";
        //strArrays2[3] = this.dtEnd.ToString("dd.MM.yyyy");
        //strArrays2[4] = "\r\n\r\n";
        string outs = "Anwesenheit von "
		    + this.dtStart.ToString("dd.MM.yyyy")
		    + " bis "
		    + this.dtEnd.ToString("dd.MM.yyyy")
		    + "\r\n\r\n";
            
		TimeSpan prevSaldo = this.PreviousSaldo(this.dtStart);
		outs += string.Format("Zeitkonto:       {0:f2}\r\n", prevSaldo.TotalHours);
		outs += string.Format("Sollarbeitszeit: {0:f2}\r\n\r\n", MainApp.NominalWorkTime);

		ArrayList erg = workDay.Select(string.Concat(new string[] { "[DayDate] >= #", selFrom, "# AND [DayDate] <= #", selTo, "#" }), "[DayDate]");
		
        if (erg != null)
		{
			outs += "    Datum  | kommt | geht  |   Pause  | anwesend | Differenz |   Saldo   | Bemerkung";
			outs += "\r\n";
			outs = "-----------+-------+-------+----------+----------+-----------+-----------+---------------";
			outs += "\r\n";
            WorkDay item = null;

			for (int i = 0; i < erg.Count; i++)
			{
				item = erg[i] as WorkDay;
				
				TimeSpan currDayRawWtime = item.OutTime.Subtract(item.InTime);
				TimeSpan currDayWtime = currDayRawWtime.Subtract(item.PauseTime);
				
                currWork = currWork.Add(currDayWtime);
				
                if (item.WorkClassOid != null)
				{
					Epo epo = item.ResolveOid(item.WorkClassOid);
					if (epo != null)
					{
						name = (epo as WorkClass).Name;
					}
					else
					{
						name = item.WorkClassOid.ToString();
					}
				}
				else
				{
					name = "./.";
				}

                //if (string str6 = name)
                //{
                //    str6 = string.IsInterned(str6);
                //    if (str6 == "./." || str6 != "GLAZ")
                //    {
                //        DateTime dayDate = item.DayDate;
                //        if (dayDate.DayOfWeek != 0)
                //        {
                //            DateTime dateTime1 = item.DayDate;
                //            if (dateTime1.DayOfWeek != 6)
                //            {
                //                timeSpan1 = timeSpan6.Subtract(nominalWorkPerDay);
                //            }
                //            else
                //            {
                //                timeSpan1 = timeSpan6;
                //            }
                //        }
                //    }
                //}

                	if (name == "./." ||  name != "GLAZ")
					{
						DateTime dayDate = item.DayDate;
						if ((dayDate.DayOfWeek != DayOfWeek.Sunday) && (dayDate.DayOfWeek != DayOfWeek.Saturday))
						{
							
								currSpan = currDayWtime.Subtract(nominalWorkPerDay);
                        }
						else
						{
							currSpan = currDayWtime;
						}
					}
				
			
            L_0280:
				prevSaldo = prevSaldo.Add(currSpan);
				if (currSpan >= TimeSpan.Zero)
				{
					str1 = " ";
				}
				else
				{
					str1 = "";
				}
				if (prevSaldo >= TimeSpan.Zero)
				{
					str2 = " ";
					continue;
				}
				str2 = "";
			
                //object obj = str5;
                //object[] objArray = new object[19];
                //objArray[0] = obj;

                //1[DateTime dateTime2 = item.DayDate] = dateTime2.ToString("dd.MM.yyyy");
                //objArray[2] = " | ";
                //3[DateTime dateTime3 = item.InTime] = dateTime3.ToString("t");
                //objArray[4] = " | ";
                //5[DateTime dateTime4 = item.OutTime] = dateTime4.ToString("t");
                //objArray[6] = " | ";
                //7[TimeSpan timeSpan7 = item.PauseTime] = timeSpan7.ToString();
                //objArray[8] = " | ";
                //objArray[9] = currDayWtime;
                //objArray[10] = " | ";
                //objArray[11] = str1;
                //objArray[12] = currSpan;
                //objArray[13] = " | ";
                //objArray[14] = str2;
                //objArray[15] = prevSaldo;
                //objArray[16] = " | ";
                //objArray[17] = name;
                //objArray[18] = "\r\n";
                //str5 = string.Concat(objArray);
			}

			outs += "-----------+-------+-------+----------+----------+-----------+-----------+---------------";
			outs += "\r\n";
			outs += "                                      |";
			outs += string.Format(" {0,8:f2} |", currWork.TotalHours);
		}
		else
		{
			outs = "Keine Eintragungen im Datumsbereich";
		}

		this.infoTB.Text = outs;
		this.infoTB.SelectionStart = 200000;
	}

	protected Oid GetOidFrom(ComboBox cb)
	{
		Oid _oid = null;
		Epo selectedItem = cb.SelectedItem as Epo;
		if (selectedItem != null)
		{
			_oid = selectedItem.oid;
		}
		return _oid;
	}

	protected Oid GetOidFrom(ListBox lb)
	{
		Oid _oid = null;
		Epo selectedItem = lb.SelectedItem as Epo;
		if (selectedItem != null)
		{
			_oid = selectedItem.oid;
		}
		return _oid;
	}

	private void InitializeComponent()
	{
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonthListDialog));
        this.SuspendLayout();
        // 
        // MonthListDialog
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(432, 382);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "MonthListDialog";
        this.ResumeLayout(false);

	}

	private void InitializeComponents()
	{
		ArrayList arrayLists = new ArrayList();
		int num1 = 8;
		int num2 = 25;
		int num3 = 15;
		int num4 = 100;
		int num5 = 700;
		ResourceManager resourceManager = new ResourceManager(typeof(MonthListDialog));
		this.components = new Container();
		base.Icon = (Icon)resourceManager.GetObject("$this.Icon");
		this.NewMultilineTextBox(8, num1, num5, 31 * num3).infoTB = TextBox textBox = this.NewMultilineTextBox(8, num1, num5, 31 * num3).Add(textBox);
		this.infoTB.Font = new Font(FontFamily.GenericMonospace, this.Font.Size, FontStyle.Regular);
		this.infoTB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		this.infoTB.ReadOnly = true;
		num1 = num1 + 30 * num3 + num2;
		num1 = num1 + 15;
		this.NewButton("Schließen", 8, num1, num4, num3 + 10).closeB = Button button = this.NewButton("Schließen", 8, num1, num4, num3 + 10).Add(button);
		this.closeB.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		base.Click += new EventHandler(this.CloseB_Click);
		base.Text = this.dialogTitel;
		base.Size = new Size(8 + num5 + 18, num1 + 70);
		for (int i = 0; i < arrayLists.Count; i++)
		{
			base.Controls.Add(arrayLists[i] as Control);
		}
		this.FillWithData();
	}

	protected Button NewButton(string txt, int locx, int locy, int sizex, int sizey)
	{
		Button button = new Button();
		button.Text = txt;
		button.Location = new Point(locx, locy);
		button.Size = new Size(sizex, sizey);
		return button;
	}

	protected CheckBox NewCheckBox(int locx, int locy)
	{
		CheckBox checkBox = new CheckBox();
		checkBox.Location = new Point(locx, locy);
		return checkBox;
	}

	protected ComboBox NewComboBox(int locx, int locy, int sizex, int sizey)
	{
		ComboBox comboBox = new ComboBox();
		comboBox.Location = new Point(locx, locy);
		comboBox.Size = new Size(sizex, sizey);
		return comboBox;
	}

	protected Label NewLabel(string txt, int locx, int locy, int sizex, int sizey)
	{
		Label label = new Label();
		label.Text = txt;
		label.Location = new Point(locx, locy);
		label.Size = new Size(sizex, sizey);
		return label;
	}

	protected ListBox NewListBox(int locx, int locy, int sizex, int sizey)
	{
		ListBox listBox = new ListBox();
		listBox.Location = new Point(locx, locy);
		listBox.Size = new Size(sizex, sizey);
		return listBox;
	}

	protected TextBox NewMultilineTextBox(int locx, int locy, int sizex, int sizey)
	{
		TextBox textBox = this.NewTextBox(locx, locy, sizex, sizey);
		textBox.Multiline = true;
		textBox.WordWrap = true;
		return textBox;
	}

	protected TextBox NewMultilineTextBox(int locx, int locy, int sizex, int sizey, int maxchars)
	{
		TextBox textBox = this.NewTextBox(locx, locy, sizex, sizey, maxchars);
		textBox.Multiline = true;
		textBox.WordWrap = true;
		return textBox;
	}

	protected PictureBox NewPictureBox(int locx, int locy, int sizex, int sizey)
	{
		PictureBox pictureBox = new PictureBox();
		pictureBox.Location = new Point(locx, locy);
		pictureBox.Size = new Size(sizex, sizey);
		return pictureBox;
	}

	protected TextBox NewTextBox(int locx, int locy, int sizex, int sizey)
	{
		TextBox textBox = new TextBox();
		textBox.Location = new Point(locx, locy);
		textBox.Size = new Size(sizex, sizey);
		return textBox;
	}

	protected TextBox NewTextBox(int locx, int locy, int sizex, int sizey, int maxchars)
	{
		TextBox textBox = new TextBox();
		textBox.Location = new Point(locx, locy);
		textBox.Size = new Size(sizex, sizey);
		textBox.MaxLength = maxchars;
		return textBox;
	}

	private TimeSpan PreviousSaldo(DateTime before)
	{
		TimeSpan timeSpan1;
		string name;
		TimeSpan timeSpan2 = new TimeSpan(0, 0, 0);
		TimeSpan timeSpan3 = TimeSpan.FromHours(MainApp.NominalWorkTime);
		WorkDay workDay = MainApp.workDayVerwo;
		string str1 = string.Concat(before.ToString("yyyy-MM-dd"), " 00:00:00");
		ArrayList arrayLists = workDay.Select(string.Concat("[DayDate] < #", str1, "#"), "[DayDate]");
		if (arrayLists != null)
		{
			for (int i = 0; i < arrayLists.Count; i++)
			{
				WorkDay item = arrayLists[i] as WorkDay;
				TimeSpan bruttoAnw = item.BruttoAnw;
				TimeSpan nettoAnw = item.NettoAnw;
				if (item.WorkClassOid != null)
				{
					Epo epo = item.ResolveOid(item.WorkClassOid);
					if (epo != null)
					{
						name = epo as WorkClass.Name;
					}
					else
					{
						name = item.WorkClassOid.ToString();
					}
				}
				else
				{
					name = "./.";
				}
				if (string str2 = name)
				{
					str2 = string.IsInterned(str2);
					if (str2 == "./." || str2 != "GLAZ")
					{
						DateTime dayDate = item.DayDate;
						if (dayDate.DayOfWeek != 6)
						{
							DateTime dateTime = item.DayDate;
							if (dateTime.DayOfWeek != 0)
							{
								timeSpan1 = nettoAnw.Subtract(timeSpan3);
							}
							else
							{
								timeSpan1 = nettoAnw;
							}
						}
						continue;
					}
					timeSpan1 = TimeSpan.FromHours(-MainApp.NominalWorkTime);
					continue;
				}
				timeSpan1 = new TimeSpan(0, 0, 0);
				continue;
				timeSpan2 = timeSpan2.Add(timeSpan1);
			}
		}
		return timeSpan2;
	}
}
}