using HSp.CsEpo;
using System;

namespace HSp.Worktime
{
internal class WorkBaseView : EpoView
{
	public WorkBaseView(Epo viewedEpo, object parent) : base(viewedEpo, parent)
	{
	}

	public WorkBaseView()
	{
	}

	protected override void InitSpecialControls()
	{
		base.InitSpecialControls();
		base.SetLabel("Name", "Bezeichnung");
		base.SetGeometry("Name", 1, 1, 11, 1, 20, 1);
		base.SetLabel("Created", "Anlegedatum");
		base.SetVisibility("Created", false);
		base.SetGeometry("Created", 1, 2, 11, 2, 10, 1);
		base.SetLabel("CreatedBy", "Angelegt von");
		base.SetVisibility("CreatedBy", false);
		base.SetGeometry("CreatedBy", 1, 3, 11, 3, 10, 1);
		base.SetLabel("Modified", "Änderungsdatum");
		base.SetVisibility("Modified", false);
		base.SetGeometry("Modified", 1, 4, 11, 4, 10, 1);
		base.SetLabel("ModifiedBy", "Geändert von");
		base.SetVisibility("ModifiedBy", false);
		base.SetGeometry("ModifiedBy", 1, 5, 11, 5, 10, 1);
	}
}
}