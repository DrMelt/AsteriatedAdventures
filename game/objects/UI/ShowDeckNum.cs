using Godot;
using System;

public partial class ShowDeckNum : LabelInGameBase
{
	[Export]
	PlayerId deckInd = PlayerId.None;

	CardArea deck = null;

	public override void _Ready()
	{
		base._Ready();
		deck = gameManagerRef.GetDecksRef(deckInd);
	}

	public override void _Process(double delta)
	{
		Text = deck.CardNum().ToString();
	}
}
