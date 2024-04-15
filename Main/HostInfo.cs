using Godot;
using System;

public partial class HostInfo : Node
{
	static string nodePath = "";
	public static string NodePath { get => nodePath; }

	[Export]
	int hostId = 0;
	[Export]
	int hostInRoom = 0;

	public int HostInRoom { get => hostInRoom; }



	public override void _Ready()
	{
		nodePath = GetPath();

		hostId = Multiplayer.GetUniqueId();
	}

	public override void _Process(double delta)
	{
	}
}
