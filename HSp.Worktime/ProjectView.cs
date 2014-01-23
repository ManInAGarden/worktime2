using HSp.CsEpo;
using System;

namespace HSp.Worktime
{
internal class ProjectView : WorkBaseView
{
	public ProjectView(Epo viewedEpo, object parent) : base(viewedEpo, parent)
	{
	}

	protected override void InitSpecialControls()
	{
		base.InitSpecialControls();
		base.SetLabel("SapName", "SAP-PSP Element");
		base.SetGeometry("SapName", 1, 2, 11, 2, 20, 1);
		base.SetLabel("Active", "aktiv");
		base.SetGeometry("Active", 1, 3, 11, 3, 1, 1);
		base.SetLabel("Vorrat", "Vorrat/h");
		base.SetGeometry("Vorrat", 1, 4, 11, 4, 20, 1);
		base.SetLabel("EndTag", "Letzter Tag");
		base.SetGeometry("EndTag", 1, 5, 11, 5, 20, 1);
		base.SetLabel("Verfuegbar", "Verfuegbar/h");
		base.SetGeometry("Verfuegbar", 1, 6, 11, 6, 20, 1);
	}
}
}