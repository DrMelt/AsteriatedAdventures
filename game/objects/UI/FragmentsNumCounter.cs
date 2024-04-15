using Godot;
using System;

public partial class FragmentsNumCounter : LabelInGameBase
{


	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{
		Text = gameManagerRef.FragmentsNum().ToString();
	}
}
