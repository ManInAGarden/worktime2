using HSp.CsEpo;
using System;
using System.Windows.Forms;

namespace HSp.Worktime
{
internal class WorkDayView : WorkBaseView
{
	public WorkDayView(Epo viewedEpo, object parent) : base(viewedEpo, parent)
	{
	}

	protected override void InitSpecialControls()
	{
		base.InitSpecialControls();
		base.SetVisibility("Name", false);
		base.SetLabel("DayDate", "Datum");
		base.SetGeometry("DayDate", 1, 1, 11, 1, 30, 1);
		DateTimePicker control1 = base.GetControl("DayDate") as DateTimePicker;
		control1.Format = DateTimePickerFormat.Custom;
		control1.CustomFormat = "dd.MM.yyyy";
		base.SetLabel("InTime", "kommt");
		base.SetGeometry("InTime", 1, 2, 11, 2, 30, 1);
		DateTimePicker dateTimePicker1 = base.GetControl("InTime") as DateTimePicker;
		dateTimePicker1.Format = DateTimePickerFormat.Custom;
		dateTimePicker1.ShowUpDown = true;
		dateTimePicker1.CustomFormat = "HH:mm";
		base.SetLabel("OutTime", "geht");
		base.SetGeometry("OutTime", 1, 3, 11, 3, 30, 1);
		DateTimePicker control2 = base.GetControl("OutTime") as DateTimePicker;
		control2.Format = DateTimePickerFormat.Custom;
		control2.ShowUpDown = true;
		control2.CustomFormat = "HH:mm";
		base.SetLabel("PauseTime", "Pause");
		base.SetGeometry("PauseTime", 1, 4, 11, 4, 30, 1);
		DateTimePicker dateTimePicker2 = base.GetControl("PauseTime") as DateTimePicker;
		dateTimePicker2.Format = DateTimePickerFormat.Custom;
		dateTimePicker2.ShowUpDown = true;
		dateTimePicker2.CustomFormat = "HH:mm";
		base.SetLabel("WorkClassOid", "Ausnahmeart");
		base.SetGeometry("WorkClassOid", 1, 5, 11, 5, 30, 1);
		base.SetLabel("Comment", "Bemerkung");
		base.SetGeometry("Comment", 1, 6, 11, 6, 30, 4);
		base.AddViewedLink("Projects", "WorkDayProjectLink", "WorkDayOid", null);
		base.SetLabel("Projects", "Projektanteile");
		base.SetGeometry("Projects", 1, 12, 11, 12, 30, 5);
		base.SetLinkEditable("Projects", 11, 11);
	}
}
}