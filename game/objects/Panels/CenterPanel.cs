using Godot;
using System;

public enum CenterPanelCardPos
{
	Red_1 = 0,
	Red_2 = 1,
	Red_3 = 2,
	Blue_1 = 3,
	Blue_2 = 4,
	Blue_3 = 5
}


public partial class CenterPanel : Node3D
{
	[Export]
	Godot.Collections.Array<CardPos> cardPosRef = new Godot.Collections.Array<CardPos>(new CardPos[6]);

}
