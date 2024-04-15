using Godot;
using System;


public partial class CardArea : Node3DInGameBase
{
    [Export]
    protected Godot.Collections.Array<CardBase> cardsRef;


    public override void _Ready()
    {
        base._Ready();
        RegisterArea();
    }

    protected virtual void RegisterArea()
    {
        HostInfo hostInfo = GetNode<HostInfo>(HostInfo.NodePath);
        GameManager gameManager = GetNode<GameManager>(GameControl.GetGamePath(hostInfo.HostInRoom) + nameof(GameManager));

        gameManager.RegisterArea(this);
    }

    public virtual int CardNum() { return cardsRef.Count; }
    public virtual bool CanAddCard(CardBase card) { return true; }
    public virtual void AddCard(CardBase card) { cardsRef.Add(card); }
    public virtual bool CanRemoveCard(CardBase card) { return true; }
    public virtual void RemoveCard(CardBase card) { cardsRef.Remove(card); }
}
