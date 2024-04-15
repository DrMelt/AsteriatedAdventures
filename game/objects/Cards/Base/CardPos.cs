using Godot;
using System;
using System.Threading;

public partial class CardPos : CardArea
{
	[Export]
	Node3D cardHintRef = null;

	[Export]
	bool showHint = false;

	[Export]
	Vector3 posOffset = new Vector3(0, 0, 0.001f);


	public bool ShowHint { get => showHint; set { showHint = value; cardHintRef.Visible = showHint; } }

	public Transform3D GetTrans(int cardInd)
	{
		Transform3D trans = GlobalTransform;

		for (int i = 1; i < cardInd; i++)
		{
			trans = trans.Translated(GlobalTransform * posOffset);
		}

		return trans;
	}


	public void MouseEnter()
	{

	}

	public void MouseExit()
	{

	}
}
