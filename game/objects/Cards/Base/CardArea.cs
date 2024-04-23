using Godot;
using System;

public enum CardAreaNames
{
    Deck,
    Hand,
    Table,
    ReTable,
    Discard,
}


public partial class CardArea : Node3DInGameBase
{
    [Export]
    PlayerId ownerId = PlayerId.None;

    public PlayerId OwnerId { get => ownerId; }
    [Export]
    CardAreaNames areaName = CardAreaNames.Deck;
    public CardAreaNames AreaName { get => areaName; }

    [Export]
    bool isShuffleAtStart = false;

    [Export]
    protected Godot.Collections.Array<CardBase> cardsRef;


    public void SetCardsOwner(PlayerId playerId)
    {
        foreach (var card in cardsRef)
        {
            card.OwnerId = playerId;
        }
    }

    public void SetCardFromDeck(PlayerId playerId)
    {
        foreach (var card in cardsRef)
        {
            card.FromDeskId = playerId;
        }
    }

    public void Shuffle()
    {
        cardsRef.Shuffle();
    }


    public override void _Ready()
    {
        base._Ready();
        RegisterArea();

        if (isShuffleAtStart)
        {
            Shuffle();
        }

        for (int i = 0; i < cardsRef.Count; i++)
        {
            cardsRef[i].OwnerId = ownerId;
            cardsRef[i].InCardAreaRef = this;
        }
    }

    protected virtual void RegisterArea()
    {
        HostInfo hostInfo = GetNode<HostInfo>(HostInfo.NodePath);
        GameManager gameManager = GetNode<GameManager>(GameControl.GetGamePath(hostInfo.HostInRoom) + nameof(GameManager));

        gameManager.RegisterArea(this);

    }

    public virtual void Uncover()
    {
        foreach (CardBase card in cardsRef)
        {
            card.Uncover();
        }
    }

    public virtual CardBase.CardStarEnum AreaStar()
    {
        CardBase.CardStarEnum res = CardBase.CardStarEnum.None;
        foreach (CardBase card in cardsRef)
        {
            if (card.CardState == CardBase.CardStateEnum.Back)
            {
                res = CardBase.CardStarEnum.None;
            }

            CardBase.CardStarEnum star = card.GetBaseCardStar();
            if (star != CardBase.CardStarEnum.A)
            {
                res = star;
            }
        }

        return res;
    }

    public virtual void NewRoundReset()
    {
    }

    public virtual Transform3D GetTrans(int cardInd)
    {
        return GlobalTransform;
    }

    public Transform3D GetTrans(CardBase card)
    {
        return GetTrans(card, card.CardState);
    }

    public Transform3D GetTrans(CardBase card, CardBase.CardStateEnum cardState)
    {
        Transform3D res = GetTrans(cardsRef.IndexOf(card));
        switch (cardState)
        {
            case CardBase.CardStateEnum.Normal:
                break;
            case CardBase.CardStateEnum.Reversed:
                res.Basis = res.Basis.Rotated((res.Basis * Vector3.Forward).Normalized(), (float)Math.PI);
                break;
            case CardBase.CardStateEnum.Horizontal:
                res.Basis = res.Basis.Rotated((res.Basis * Vector3.Forward).Normalized(), (float)Math.PI * 0.5f);
                break;
            case CardBase.CardStateEnum.Back:
                res.Basis = res.Basis.Rotated((res.Basis * Vector3.Up).Normalized(), (float)Math.PI);
                break;
            case CardBase.CardStateEnum.Back_Re:
                res.Basis = res.Basis.Rotated((res.Basis * Vector3.Up).Normalized(), (float)Math.PI).
                    Rotated((res.Basis * Vector3.Forward).Normalized(), (float)Math.PI);
                break;
            case CardBase.CardStateEnum.Back_H:
                res.Basis = res.Basis.Rotated((res.Basis * Vector3.Up).Normalized(), (float)Math.PI).
                    Rotated((res.Basis * Vector3.Forward).Normalized(), (float)Math.PI * 0.5f);
                break;
            case CardBase.CardStateEnum.InHand:
                if (card.OwnerId == gameControlRef.ScreenPlayer) { }
                else
                {
                    res.Basis = res.Basis.Rotated((res.Basis * Vector3.Up).Normalized(), (float)Math.PI);
                }
                break;
            default:
                break;
        }

        return res;
    }

    public virtual int CardNum() { return cardsRef.Count; }

    // Be called in CardBase IsValidCardArea
    public virtual bool CanAddCard(CardBase card) { return true; }
    public virtual void MoveCard(CardBase card)
    {
        card.InCardAreaRef.RemoveCard(card);
        card.InCardAreaRef = this;
        cardsRef.Add(card);
    }
    public virtual void MoveCards(CardArea cardArea)
    {
        foreach (var card in cardArea.cardsRef)
        {
            card.InCardAreaRef.RemoveCard(card);
            card.InCardAreaRef = this;
            cardsRef.Add(card);
        }
    }

    public virtual bool CanRemoveCard(CardBase card) { return true; }
    public virtual void RemoveCard(CardBase card) { cardsRef.Remove(card); }
    public virtual CardBase DrawCard()
    {
        if (cardsRef.Count == 0)
        {
            return null;
        }

        CardBase res = cardsRef[cardsRef.Count - 1];
        cardsRef.RemoveAt(cardsRef.Count - 1);
        return res;
    }
}
