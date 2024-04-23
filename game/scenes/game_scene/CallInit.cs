using Godot;
using System;

public partial class CallInit : NodeInGameBase
{
	public delegate void InitGameDelegate();
	public event InitGameDelegate InitGameEvent;


	public override void _Ready()
	{
		base._Ready();
		InitGameEvent.Invoke();
	}

	public override void _Process(double delta)
	{
	}
}
