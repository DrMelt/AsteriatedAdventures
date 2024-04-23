using Godot;
using System;


public class EffectData
{
    public CardEffect cardEffect = null;
    public CardBase card = null;

    public void Effect()
    {
        cardEffect.Effect(card);
    }
}

public partial class CardEffect : NodeInGameBase
{
    [Flags]
    public enum EffectLimitsEnum
    {
        None = 0x1,
        TurnOne = 0x2,
        TrickOne = 0x4,
    }

    [Export]
    EffectLimitsEnum effectLimit = EffectLimitsEnum.None;
    public EffectLimitsEnum EffectLimit { get => effectLimit; }

    [Flags]
    public enum EffectTrunEnum
    {
        Adverse = 0x1,
        Own = 0x2,
        ALL = Adverse | Own,
    }
    [Export]
    EffectTrunEnum effectTrun = EffectTrunEnum.ALL;
    public EffectTrunEnum EffectTrun { get => effectTrun; }

    [Export]
    EffectTimeEnum effectTime = EffectTimeEnum.None;
    public EffectTimeEnum EffectTime { get => effectTime; }

    [Export]
    CardBase.CardStateEnum validCardState = CardBase.CardStateEnum.Normal | CardBase.CardStateEnum.Reversed;
    public CardBase.CardStateEnum ValidCardState { get => validCardState; }


    [Flags]
    public enum EffectTypeEnum
    {
        Passive = 0x01,
        Response = 0x02,
    }
    [Export]
    EffectTypeEnum effectType = EffectTypeEnum.Passive;
    public EffectTypeEnum EffectType { get => effectType; }

    public static void SortRuleInSameTime() { }

    public virtual void Effect(CardBase cardFrom)
    {

    }
}