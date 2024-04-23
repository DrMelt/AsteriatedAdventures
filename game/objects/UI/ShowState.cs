using Godot;
using System;

public partial class ShowState : LabelInGameBase
{
	public override void _Process(double delta)
	{
		Text = gameManagerRef.GameState.ToString();
	}
}
