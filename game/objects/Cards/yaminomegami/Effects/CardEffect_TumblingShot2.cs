using Godot;
using System;

public partial class CardEffect_TumblingShot2 : CardEffect
{
	public override void Effect(CardBase card)
	{
		gameManagerRef.CurrentDuelData.Damage = 0;
	}
}
