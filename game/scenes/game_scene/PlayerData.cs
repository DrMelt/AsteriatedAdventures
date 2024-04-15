using Godot;
using System;


public enum PlayerId
{
	None = 0,
	Player1 = 1,
	Player2 = 2,
}

public partial class PlayerData : Node
{
	[Export]
	public PlayerId playerId = PlayerId.None;


	[Export]
	int HP = 18;
	[Export]
	int EP = 0;
	[Export]
	int Asteriated = 0;


	[Export]
	int drawCardMax = 8;
    public int DrawCardMax { get => DrawCardMax; set => DrawCardMax = value; }

	[ExportGroup("Ref data")]
	[Export]
	HandCards handCards;

}
