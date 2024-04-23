using Godot;
using System;

public partial class CardPos : CardArea
{
	[Export]
	Node3D cardHintRef = null;

	[Export]
	bool showHint = false;

	[Export]
	Vector3 posOffset_mm = new Vector3(0, 0, 0.5f);


	public bool ShowHint { get => showHint; set { showHint = value; cardHintRef.Visible = showHint; } }

	public override Transform3D GetTrans(int cardInd)
	{
		Transform3D trans = GlobalTransform;

		trans = trans.Translated(GlobalBasis * (cardInd * posOffset_mm * 0.001f));

		return trans;
	}

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		cardHintRef.Visible = gameControlRef.IsValidCardArea(this);
	}

}
