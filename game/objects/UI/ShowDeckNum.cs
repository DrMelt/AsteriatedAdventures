using Godot;
using System;

public partial class ShowDeckNum : LabelInGameBase
{
	[Export]
	int deckInd = 0;


	CardArea deck = null;

	public override void _Ready()
	{
		base._Ready();
		deck = gameManagerRef.DecksRef[deckInd];
	}

	public override void _Process(double delta)
	{
		Text = deck.CardNum().ToString();
	}
}
