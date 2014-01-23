using HSp.CsEpo;
using System;

namespace HSp.Worktime
{
internal class WorkDayProjectLinkView : WorkBaseView
{
	public WorkDayProjectLinkView(Epo viewedEpo, object parent) : base(viewedEpo, parent)
	{
	}

	public WorkDayProjectLinkView()
	{
	}

	protected override void InitSpecialControls()
	{
		base.InitSpecialControls();
		base.SetVisibility("Name", false);
		base.SetLabel("WorkDayOid", "Tag");
		base.SetGeometry("WorkDayOid", 1, 1, 11, 1, 20, 1);
		base.SetLabel("ProjectOid", "Projekt");
		base.SetGeometry("ProjectOid", 1, 2, 11, 2, 20, 1);
		base.SetLabel("Percentage", "Zeitanteil/%");
		base.SetGeometry("Percentage", 1, 3, 11, 3, 20, 1);
	}
}
}