using System.Windows.Forms;
using System;
using HSp.CsEpo;
using System.Xml;
using System.Diagnostics;
using HSp.CsNman;
using System.Collections;
using System.Drawing;
using System.Resources;

namespace HSp.Worktime
{
internal class AllgList : Form
{
	protected Button closeBU;

	protected Button delBU;

	protected Button dupBU;

	protected Button expBU;

	protected Button goBU;

	protected Button impBU;

	protected ListBox myLI;

	protected Button newBU;

	private string title;

	protected Epo verwo;

	public AllgList(Form own, string tit, Epo verwo)
	{
		base.Owner = own;
		this.title = tit;
		this.verwo = verwo;
		this.Initialize();
	}

	private void Close_Clicked(object sender, EventArgs e)
	{
		base.Close();
	}

	private void Del_Clicked(object sender, EventArgs e)
	{
		object selectedItem = this.myLI.SelectedItem;
		if (selectedItem != null)
		{
			EpoCached epoCached = selectedItem as EpoCached;
			DialogResult dialogResult = MessageBox.Show(this, "Soll das Element wirklich gelöscht werden?", "Rückfrage", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
			if (dialogResult == DialogResult.Yes)
			{
				epoCached.Delete();
			}
		}
	}

	private void Dup_Clicked(object sender, EventArgs e)
	{
		object selectedItem = this.myLI.SelectedItem;
		if (selectedItem != null)
		{
			Epo epo1 = selectedItem as Epo;
			Epo epo2 = epo1.Clone() as Epo;
			if (epo2 != null)
			{
				epo2.Flush();
			}
		}
	}

	private void Exp_Clicked(object sender, EventArgs e)
	{
		object selectedItem = this.myLI.SelectedItem;
		if (selectedItem != null)
		{
			Epo epo = selectedItem as Epo;
			FileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = string.Concat(epo.ToString(), "_CfgExp.xml");
			saveFileDialog.InitialDirectory = "c:\\";
			saveFileDialog.Filter = "xml Dateien (*.xml)|*.xml|Alle Dateien(*.*)|*.*";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.RestoreDirectory = true;
			if (saveFileDialog.ShowDialog() == 1)
			{
				string fileName = saveFileDialog.FileName;
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode xmlNodes1 = xmlDocument.CreateElement("WorkTimeDataNode");
				XmlNode xmlNodes2 = xmlDocument.CreateAttribute("dataversion");
				xmlNodes2.Value = "1.0";
				XmlNode xmlNodes3 = xmlDocument.CreateAttribute("purpouse");
				xmlNodes3.Value = string.Concat("SpecificDataset:", epo.GetType().Name);
				xmlNodes1.Attributes.Append(xmlNodes2 as XmlAttribute);
				xmlNodes1.Attributes.Append(xmlNodes3 as XmlAttribute);
				XmlNode xmlNodes4 = epo.AppendXmlRepresentation(xmlNodes1);
				xmlDocument.AppendChild(xmlNodes1);
				xmlDocument.Save(fileName);
				Trace.WriteLine(string.Format("Konfigurationsdaten zu {0} nach {1} extrahiert", epo.ToString(), fileName));
			}
		}
	}

	private void FillList()
	{
		if (this.verwo != null)
		{
			NDelegate nDelegate1 = new NDelegate(this.UpdateList);
			NDelegate nDelegate2 = new NDelegate(this.UpdateListAfterDelete);
			this.myLI.BeginUpdate();
			this.myLI.Items.Clear();
			ArrayList arrayLists = this.verwo.Select();
			string str1 = string.Concat("DATAFLUSH:", this.verwo.GetType().Name);
			Nman.Instance.Register(str1, this, nDelegate1);
			if (arrayLists != null)
			{
				for (int i = 0; i < arrayLists.Count; i++)
				{
					this.myLI.Items.Add(arrayLists[i]);
					Epo item = arrayLists[i] as Epo;
					string str2 = string.Concat("DATADELETE:", item.oid.ToString());
					Nman.Instance.Register(str2, this, nDelegate2);
				}
			}
			this.myLI.EndUpdate();
		}
	}

	private void Imp_Clicked(object sender, EventArgs e)
	{
		XmlDocument xmlDocument = null;
		string str1 = "1.0";
		FileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.InitialDirectory = "c:\\";
		openFileDialog.Filter = "xml Dateien (*.xml)|*.xml|Alle Dateien(*.*)|*.*";
		openFileDialog.FilterIndex = 1;
		openFileDialog.RestoreDirectory = true;
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
            return;
		}

		string fileName = openFileDialog.FileName;
		try
		{
			xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
		}
		catch (Exception exception)
		{
			Trace.WriteLine(exception.Message);
			MessageBox.Show(exception.Message);
		}
		try
		{
			XmlNode firstChild = xmlDocument.FirstChild;
			if (firstChild == null)
			{
				MessageBox.Show("Die XML-Datei hat nicht das geforderte Format für einen SSynchXML-Import");
			}
			if (!firstChild.Name.Equals("SSynchXmlDataNode"))
			{
				MessageBox.Show("Die XML-Datei hat nicht das geforderte Format für einen SSynchXML-Import");
			}
			XmlAttribute itemOf = firstChild.Attributes["dataversion"];
			if (!itemOf.Value.Equals(str1))
			{
				MessageBox.Show(string.Format("Die Datenversion des XML-Exportfiles ist {0}, verlangt ist jedoch {1}. Stellen Sie sicher, dass die Exportdaten mit einer kompatiblen SynchXML-Verion ausgespielt wurden", itemOf.Value, str1));
			}
			else
			{
				XmlAttribute xmlAttribute = firstChild.Attributes["purpouse"];
				string str2 = string.Concat("SpecificDataset:", this.verwo.GetType().Name);
				if (!xmlAttribute.Value.Equals(str2))
				{
					MessageBox.Show("Das Exportfile wurde nicht für den Einzelimport dieser Objektart ausgespielt. Stellen Sie sicher dass es als Einzelexport der selben Datenart ausgespielt wurde, die sie gerade versuchen einzuspielen");
				}
				this.verwo.BeginTransaction();
				for (int i = 0; i < firstChild.ChildNodes.Count; i++)
				{
					this.verwo.CreateFromXmlNode(firstChild.ChildNodes[i]);
				}
				Trace.WriteLine(string.Format("Konfigurationsdaten von {0} importiert", fileName));
				this.verwo.CommitTransaction();
				Trace.WriteLine(exception2.Message);
				MessageBox.Show(exception2.Message);
				this.verwo.RollbackTransaction();
				Trace.WriteLine("TRANSACTION ROLLBACK");
			}
		}
		catch (Exception exception2)
		{
		}
	}

	protected virtual void Initialize()
	{
		Size size;
		Size size2;
		ResourceManager resourceManager = new ResourceManager(typeof(MainApp));
		Icon obj = (Icon)resourceManager.GetObject("$this.Icon");
		base.Icon = obj;
		base.ClientSize = new Size(210, 260);
		this.myLI = new ListBox();
		this.myLI.Location = new Point(5, 30);
		this.myLI.Size = new Size(200, 200);
		this.myLI.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		this.myLI.add_DoubleClick(new EventHandler(this.Open_Clicked));
		this.myLI.add_SelectedValueChanged(new EventHandler(this.SelectedValueChanged));
		this.closeBU = new Button();
		this.closeBU.Text = "Schließen";
		this.closeBU.Location = new Point(5, 235);
		this.closeBU.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		this.closeBU.add_Click(new EventHandler(this.Close_Clicked));
		this.goBU = new Button();
		this.goBU.Location = new Point(5, 5);
		this.goBU.Text = ">";
		this.goBU.Size = new Size(20, 20);
		this.goBU.add_Click(new EventHandler(this.Open_Clicked));
		this.goBU.Enabled = false;
		this.newBU = new Button();
		this.newBU.Location = new Point(25, 5);
		this.newBU.Text = "*";
		this.newBU.Size = new Size(20, 20);
		this.newBU.add_Click(new EventHandler(this.New_Clicked));
		this.dupBU = new Button();
		this.dupBU.Location = new Point(45, 5);
		this.dupBU.Text = "2";
		this.dupBU.Size = new Size(20, 20);
		this.dupBU.add_Click(new EventHandler(this.Dup_Clicked));
		this.dupBU.Enabled = false;
		this.delBU = new Button();
		this.delBU.Location = new Point(65, 5);
		this.delBU.Text = "X";
		this.delBU.Size = new Size(20, 20);
		this.delBU.add_Click(new EventHandler(this.Del_Clicked));
		this.delBU.Enabled = false;
		this.expBU = new Button();
		this.expBU.Location = new Point(85, 5);
		this.expBU.Text = "E";
		this.expBU.Size = new Size(20, 20);
		this.expBU.add_Click(new EventHandler(this.Exp_Clicked));
		this.expBU.Enabled = false;
		this.impBU = new Button();
		this.impBU.Location = new Point(105, 5);
		this.impBU.Text = "I";
		this.impBU.Size = new Size(20, 20);
		this.impBU.add_Click(new EventHandler(this.Imp_Clicked));
		this.impBU.Enabled = true;
		ToolTip toolTip = new ToolTip();
		toolTip.SetToolTip(this.goBU, "Öffnet das selektierte Objekt");
		toolTip.SetToolTip(this.delBU, "Löscht das selektierte Objekt und evtl. dessen sämtliche Kindobjekte");
		toolTip.SetToolTip(this.newBU, "Legt ein neues Objekt mit voreingestellten Werten an");
		toolTip.SetToolTip(this.dupBU, "Dupliziert das selektierte Objekt");
		toolTip.SetToolTip(this.expBU, "Exportiert das selektierte Objekt in eine XML-Datei");
		toolTip.SetToolTip(this.impBU, "Importiert Objekte aus einer XML-Datei");
		base.Controls.Add(this.goBU);
		base.Controls.Add(this.newBU);
		base.Controls.Add(this.delBU);
		base.Controls.Add(this.dupBU);
		base.Controls.Add(this.expBU);
		base.Controls.Add(this.impBU);
		base.Controls.Add(this.myLI);
		base.Controls.Add(this.closeBU);
		base.Text = this.title;
		base.AcceptButton = this.closeBU;
		base.StartPosition = FormStartPosition.Manual;
		Point desktopLocation = base.Owner.DesktopLocation;
		int width = size.Width + ((Size size3 = base.Size) - size3.Width) / 2;
		Point point = base.Owner.DesktopLocation;
		int height = size2.Height + ((Size size4 = base.Size) - size4.Height) / 2;
		base.Location = new Point(width, height);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.FillList();
	}

	private void New_Clicked(object sender, EventArgs e)
	{
		Epo epo = this.verwo.NewInstance();
		epo.Flush();
	}

	protected void Open_Clicked(object sender, EventArgs e)
	{
		EpoView projectView = null;
		object selectedItem = this.myLI.SelectedItem;
		Epo epo = selectedItem as Epo;
		if (epo.GetType() == MainApp.projectVerwo.GetType())
		{
			projectView = new ProjectView(epo, this);
		}
		if (projectView != null)
		{
			projectView.Show();
		}
	}

	private void SelectedValueChanged(object sender, EventArgs e)
	{
		if (sender as ListBox.SelectedItem == null)
		{
			this.goBU.Enabled = false;
			this.delBU.Enabled = false;
			this.dupBU.Enabled = false;
			this.expBU.Enabled = false;
		}
		else
		{
			this.goBU.Enabled = true;
			this.delBU.Enabled = true;
			this.dupBU.Enabled = true;
			this.expBU.Enabled = true;
		}
	}

	public void UpdateList(NMessage msg)
	{
		Nman.Instance.UnRegister(this);
		this.FillList();
	}

	public void UpdateListAfterDelete(NMessage msg)
	{
		this.myLI.Items.Remove(msg.Sender);
		Nman.Instance.UnRegister(msg.Title);
	}
}
}