using System.Windows.Forms;
using System.Collections.Specialized;
using System;
using System.Configuration;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using HSp.CsNman;
using System.Collections;
using System.Resources;
using System.Drawing;
using HSp.CsEpo;

namespace HSp.Worktime
{
public class MainApp : Form
{
	private MenuItem anwesenheitMI;

	private static NameValueCollection config;

	private DateTimePicker dateFromDTP;

	private DateTimePicker dateToDTP;

	private ListBox dayLI;

	public static string dbConnStr;

	private MenuItem deleteDayMI;

	private MenuItem editProjectMI;

	private MenuItem exitMI;

	public static MainApp instance;

	private MenuItem kontierungMI;

	private MenuItem menuItem1;

	private MenuItem menuItem2;

	private MenuItem menuItem3;

	private MenuItem menuItem4;

	private MainMenu menuMain;

	private MenuItem newTimeEntryMI;

	private static double nominalWorkTime;

	public static Project projectVerwo;

	public static WorkClass workClassVerwo;

	public static WorkDayProjectLink workDayProjectLinkVerwo;
    private System.ComponentModel.IContainer components;

	public static WorkDay workDayVerwo;

	public static NameValueCollection Config
	{
		get
		{
			if (MainApp.config == null)
			{
				MainApp.config = ConfigurationSettings.AppSettings;
			}
			return MainApp.config;
		}
	}

	public static string DbConnStr
	{
		get
		{
			return MainApp.dbConnStr;
		}
	}

	public static MainApp Instance
	{
		get
		{
			return MainApp.instance;
		}
	}

	public static double NominalWorkTime
	{
		get
		{
			return MainApp.nominalWorkTime;
		}
	}

	static MainApp()
	{
		MainApp.dbConnStr = null;
	}

	public MainApp(string culture)
	{
		if (culture != "")
		{
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
            catch (ArgumentException argumentException)
            {
                MessageBox.Show(this, argumentException.Message, "Bad command-line argument");
            }
		}

		

		this.InitializeComponent();
	}

	private void AccountingList_Clicked(object sender, EventArgs e)
	{
		AccountingListDialog accountingListDialog = new AccountingListDialog(this, this.dateFromDTP.Value, this.dateToDTP.Value, "Kontierung");
		if (accountingListDialog != null)
		{
			accountingListDialog.Show();
		}
	}

	private void AppendListsContextMenu(ListBox lb)
	{
		ContextMenu contextMenu = new ContextMenu();
		MenuItem menuItem = new MenuItem();
		menuItem.Text = "Neuer Zeiteintrag";
		menuItem.Click += new EventHandler(this.NewWd_Clicked);
		contextMenu.MenuItems.Add(menuItem);
		menuItem = new MenuItem();
		menuItem.Text = "Löschen";
		menuItem.Click += new EventHandler(this.DeleteEntry_Clicked);
		contextMenu.MenuItems.Add(menuItem);
		lb.ContextMenu = contextMenu;
	}

	private void DeleteEntry_Clicked(object sender, EventArgs e)
	{
		WorkDay selectedItem = this.dayLI.SelectedItem as WorkDay;
		if (selectedItem != null)
		{
			selectedItem.Delete();
		}
	}

	private void EditProjects_Clicked(object sender, EventArgs e)
	{
		AllgList allgList = new AllgList(this, "Projekte", MainApp.projectVerwo);
		allgList.Show();
	}

	private void Exit_Clicked(object sender, EventArgs e)
	{
		base.Close();
	}

	private void FilterCriteriaChanged(object sender, EventArgs e)
	{
		this.InitialFillList(this.dateFromDTP.Value, this.dateToDTP.Value);
	}

	public Stream GetStreamFromAssembly(string name)
	{
		string str1 = null;
		Debug.Assert(name != null);
		Debug.Assert(name.Length > 0);
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		
        //int num = executingAssembly.FullName.IndexOf(44);
        int num = executingAssembly.FullName.IndexOf(',');
		if (num > 0)
		{
			str1 = executingAssembly.FullName.Substring(0, num);
		}
		Debug.Assert(str1 != null);
		string str2 = string.Concat(str1, ".", name);
		Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(str2);
		return manifestResourceStream;
	}

	private void InitialFillList(DateTime ff, DateTime ft)
	{
		string[] strArrays;
		NDelegate nDelegate1 = new NDelegate(this.RefreshAfterFlush);
		NDelegate nDelegate2 = new NDelegate(this.RefreshAfterDelete);
		Nman.Instance.Register("DATAFLUSH:WorkDay", this, nDelegate1);
		this.dayLI.Items.Clear();
		this.dayLI.BeginUpdate();
		string str1 = string.Concat(ff.ToString("yyyy-MM-dd"), " 00:00:00");
		string str2 = string.Concat(ft.ToString("yyyy-MM-dd"), " 23:59:59");
		ArrayList arrayLists = MainApp.workDayVerwo.Select(string.Concat(new string[] { "[DayDate] >= #", str1, "# AND [DayDate] <= #", str2, "#" }), "[DayDate]");
		if (arrayLists != null)
		{
			for (int i = 0; i < arrayLists.Count; i++)
			{
				WorkDay item = arrayLists[i] as WorkDay;
				string str3 = string.Concat("DATADELETE:", item.oid.ToString());
				Nman.Instance.Register(str3, this, nDelegate2);
				this.dayLI.Items.Add(item);
			}
		}
		this.dayLI.EndUpdate();
	}

	private void InitializeComponent()
	{
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
        this.menuMain = new System.Windows.Forms.MainMenu(this.components);
        this.menuItem1 = new System.Windows.Forms.MenuItem();
        this.newTimeEntryMI = new System.Windows.Forms.MenuItem();
        this.deleteDayMI = new System.Windows.Forms.MenuItem();
        this.menuItem4 = new System.Windows.Forms.MenuItem();
        this.exitMI = new System.Windows.Forms.MenuItem();
        this.menuItem3 = new System.Windows.Forms.MenuItem();
        this.editProjectMI = new System.Windows.Forms.MenuItem();
        this.menuItem2 = new System.Windows.Forms.MenuItem();
        this.anwesenheitMI = new System.Windows.Forms.MenuItem();
        this.kontierungMI = new System.Windows.Forms.MenuItem();
        this.dateFromDTP = new System.Windows.Forms.DateTimePicker();
        this.dateToDTP = new System.Windows.Forms.DateTimePicker();
        this.dayLI = new System.Windows.Forms.ListBox();
        this.SuspendLayout();
        // 
        // menuMain
        // 
        this.menuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem2});
        // 
        // menuItem1
        // 
        this.menuItem1.Index = 0;
        this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newTimeEntryMI,
            this.deleteDayMI,
            this.menuItem4,
            this.exitMI});
        this.menuItem1.Text = "Datei";
        // 
        // newTimeEntryMI
        // 
        this.newTimeEntryMI.Index = 0;
        this.newTimeEntryMI.Text = "Neuer Zeiteintrag";
        this.newTimeEntryMI.Click += new System.EventHandler(this.NewWd_Clicked);
        // 
        // deleteDayMI
        // 
        this.deleteDayMI.Index = 1;
        this.deleteDayMI.Text = "Zeiteintrag löschen";
        this.deleteDayMI.Click += new System.EventHandler(this.DeleteEntry_Clicked);
        // 
        // menuItem4
        // 
        this.menuItem4.Index = 2;
        this.menuItem4.Text = "-";
        // 
        // exitMI
        // 
        this.exitMI.Index = 3;
        this.exitMI.Text = "Beenden";
        this.exitMI.Click += new System.EventHandler(this.Exit_Clicked);
        // 
        // menuItem3
        // 
        this.menuItem3.Index = 1;
        this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.editProjectMI});
        this.menuItem3.Text = "Projekte";
        // 
        // editProjectMI
        // 
        this.editProjectMI.Index = 0;
        this.editProjectMI.Text = "Bearbeiten";
        this.editProjectMI.Click += new System.EventHandler(this.EditProjects_Clicked);
        // 
        // menuItem2
        // 
        this.menuItem2.Index = 2;
        this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.anwesenheitMI,
            this.kontierungMI});
        this.menuItem2.Text = "Ansicht";
        // 
        // anwesenheitMI
        // 
        this.anwesenheitMI.Index = 0;
        this.anwesenheitMI.Text = "Anwesenheit";
        this.anwesenheitMI.Click += new System.EventHandler(this.MonthList_Clicked);
        // 
        // kontierungMI
        // 
        this.kontierungMI.Index = 1;
        this.kontierungMI.Text = "Kontierung";
        this.kontierungMI.Click += new System.EventHandler(this.AccountingList_Clicked);
        // 
        // dateFromDTP
        // 
        this.dateFromDTP.Location = new System.Drawing.Point(5, 5);
        this.dateFromDTP.Name = "dateFromDTP";
        this.dateFromDTP.Size = new System.Drawing.Size(195, 20);
        this.dateFromDTP.TabIndex = 0;
        this.dateFromDTP.ValueChanged += new System.EventHandler(this.FilterCriteriaChanged);
        // 
        // dateToDTP
        // 
        this.dateToDTP.Location = new System.Drawing.Point(200, 5);
        this.dateToDTP.Name = "dateToDTP";
        this.dateToDTP.Size = new System.Drawing.Size(195, 20);
        this.dateToDTP.TabIndex = 1;
        this.dateToDTP.ValueChanged += new System.EventHandler(this.FilterCriteriaChanged);
        // 
        // dayLI
        // 
        this.dayLI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.dayLI.BackColor = System.Drawing.Color.LemonChiffon;
        this.dayLI.Location = new System.Drawing.Point(5, 25);
        this.dayLI.Name = "dayLI";
        this.dayLI.Size = new System.Drawing.Size(390, 355);
        this.dayLI.TabIndex = 2;
        // 
        // MainApp
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(402, 386);
        this.Controls.Add(this.dateFromDTP);
        this.Controls.Add(this.dateToDTP);
        this.Controls.Add(this.dayLI);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Menu = this.menuMain;
        this.Name = "MainApp";
        this.Text = "Worktime 3.0";
        this.Closed += new System.EventHandler(this.MainApp_Closed);
        this.Load += new System.EventHandler(this.MainApp_Load);
        this.ResumeLayout(false);

	}

	private void List_DblClick(object sender, EventArgs e)
	{
		WorkDay selectedItem = this.dayLI.SelectedItem as WorkDay;
		if (selectedItem != null)
		{
			WorkDayView workDayView = new WorkDayView(selectedItem, this);
			if (workDayView != null)
			{
				workDayView.ShowDialog();
			}
		}
	}

	[STAThread]
	public static void Main(string[] args)
	{
		string str = "";
		Trace.WriteLine(string.Format("dd.MM.yyyy hh:mm:ss", DateTime.Now.ToString()));

		if ((int)args.Length == 1)
		{
			MainApp.dbConnStr = args[0];
			Trace.WriteLine(string.Format("Datenbank Connectstring: {0}", MainApp.dbConnStr));
		}
		else
		{
			MessageBox.Show("Bitte auf der Kommandozeile den Pfad für die Datenbankverbindung angeben");
			Trace.WriteLine("Initialisiere Datenbankverbindungen...");
			MainApp.workDayVerwo = new WorkDay(MainApp.dbConnStr);
			MainApp.workClassVerwo = new WorkClass(MainApp.dbConnStr);
			MainApp.projectVerwo = new Project(MainApp.dbConnStr);
			MainApp.workDayProjectLinkVerwo = new WorkDayProjectLink(MainApp.dbConnStr);
			EpoView.AddEpoViewRelation(MainApp.workDayProjectLinkVerwo, new WorkDayProjectLinkView());

			Trace.WriteLine("...fertig");

			string item = MainApp.Config["nominalWorkTime"];

			if (item != null)
			{
                try
                {
                    MainApp.nominalWorkTime = double.Parse(item);
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(exc.Message);
                    MainApp.nominalWorkTime = 8;
                }
			}
			else
			{
				MainApp.nominalWorkTime = 8;
			}
			try
			{
			}
			catch (Exception exception)
			{
			}
			Trace.WriteLine(string.Format("Die soll Arbeitszeit berträgt {0}h/Tag", MainApp.nominalWorkTime));
			MainApp.instance = new MainApp(str);
			Application.Run(MainApp.Instance);
		}
	}

	private void MainApp_Closed(object sender, EventArgs e)
	{
		Trace.WriteLine("Exiting Worktime");
		Trace.Close();
	}

	private void MainApp_Load(object sender, EventArgs e)
	{
		DateTime now = DateTime.Now;
		DateTime dateTime1 = new DateTime(now.Year, now.Month, 1, 0, 0, 0, 0);
		DateTime dateTime2 = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, 99);
		this.dateFromDTP.Value = dateTime1;
		this.dateToDTP.Value = dateTime2;
	}

	private void MonthList_Clicked(object sender, EventArgs e)
	{
		MonthListDialog monthListDialog = new MonthListDialog(this, this.dateFromDTP.Value, this.dateToDTP.Value, "Arbeitszeit");
		if (monthListDialog != null)
		{
			monthListDialog.Show();
		}
	}

	private void NewWd_Clicked(object sender, EventArgs e)
	{
		NDelegate nDelegate = new NDelegate(this.RefreshAfterDelete);
		WorkDay workDay = new WorkDay();
		workDay.Flush();
		string str = string.Concat("DATADELETE:", workDay.oid.ToString());
		Nman.Instance.Register(str, this, nDelegate);
	}

	public void RefreshAfterDelete(NMessage msg)
	{
		this.dayLI.Items.Remove(msg.Sender);
		Nman.Instance.UnRegister(msg.Title);
	}

	public void RefreshAfterFlush(NMessage msg)
	{
		WorkDay sender = msg.Sender as WorkDay;
		this.dayLI.BeginUpdate();
		int num = this.dayLI.Items.IndexOf(sender);
		if (num >= 0)
		{
			this.dayLI.Items.RemoveAt(num);
			this.dayLI.Items.Insert(num, sender);
		}
		else
		{
			this.dayLI.Items.Add(sender);
		}
		this.dayLI.EndUpdate();
	}
}
}