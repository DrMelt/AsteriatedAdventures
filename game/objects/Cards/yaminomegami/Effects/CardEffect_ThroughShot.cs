using Godot;
using System;

public partial class CardEffect_ThroughShot : CardEffect
{
    public override void Effect(CardBase cardFrom)
    {
        CardBase card = gameControlRef.RequestOneCard(ValidCheck);
        card.Reverse();
    }

    bool ValidCheck(CardBase cardCheck, CardBase cardFrom)
    {
        PlayerId adverseId = gameManagerRef.GetAdversePlayerId(cardFrom.OwnerId);

        if (adverseId == cardCheck.OwnerId && cardCheck.InCardAreaRef.AreaName == CardAreaNames.Table)
        {
            return true;
        }

        return false;
    }
}
