using Godot;
using System;

public partial class CardEffect_TumblingShot1 : CardEffect
{
    public override void Effect(CardBase card)
    {
        gameManagerRef.AttackDamage(gameManagerRef.GetAdversePlayerId(card.OwnerId), 2);
    }
}
