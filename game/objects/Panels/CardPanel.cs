using Godot;
using System;

public partial class CardPanel : Node3DInGameBase
{
	[Export]
	protected Godot.Collections.Array<CardArea> cardAreaRef = new Godot.Collections.Array<CardArea>(new CardPos[6]);
	// public Godot.Collections.Array<CardArea> CardPosRef { get => cardPosRef; }

	// public CardBase.CardStarEnum AreaStar(int ind)
	// {
	// 	return cardAreaRef[ind].AreaStar();
	// }

	public virtual void Align()
	{
		GD.Print("No Align action in CardPanel");
	}
}
