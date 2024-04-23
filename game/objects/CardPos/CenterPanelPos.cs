using Godot;
using System;
using System.Collections.Generic;

public partial class CenterPanelPos : CardPos
{
	[Export]
	CenterPanel centerPanelRef = null;

	[Export]
	int AP = 1;

	[Export]
	int canCoverTime = 1;

	// [Export]
	// int areaId = 0;


	public static void Duel(CenterPanelPos cardArea1, CenterPanelPos cardArea2)
	{
		if (cardArea1.AreaStar() > cardArea2.AreaStar())
		{
			cardArea1.OnCardWin();
			cardArea2.OnCardLose();
		}
		else if (cardArea1.AreaStar() < cardArea2.AreaStar())
		{
			cardArea2.OnCardWin();
			cardArea1.OnCardLose();
		}
		else
		{
			cardArea1.OnCardLeave();
			cardArea2.OnCardLeave();
		}
	}

	void OnCardLose()
	{
		foreach (CardBase card in cardsRef)
		{
			card.OnCardLose();
		}
	}

	void OnCardWin()
	{
		foreach (CardBase card in cardsRef)
		{
			card.OnCardWin();
		}
	}

	void OnCardLeave()
	{
		foreach (CardBase card in cardsRef)
		{
			card.OnCardLeave();
		}
	}


	public override void NewRoundReset()
	{
		base.NewRoundReset();

		AP = 1;
		canCoverTime = 1;
	}


	public override bool CanAddCard(CardBase card)
	{
		// In Action
		if (gameManagerRef.GameState == GameStateEnum.Action && AP > 0)
		{
			if (card.DragState == DragStateEnum.Drag)
			{
				if (CardNum() == 0)
				{
					if (card.CardStar != CardBase.CardStarEnum.A)
					{
						List<CardBase.CardStarEnum> stars = centerPanelRef.ExistedStars();
						if (stars.Contains(card.CardStar))
						{
							return false;
						}
						else
						{
							return true;
						}
					}
				}
				else
				{
					if (card.CardCoverType == CardBase.CardCoverTypeEnum.Normal
						&& card.CardStar == AreaStar())
					{
						return true;
					}
				}
			}
			else if (card.DragState == DragStateEnum.Back)
			{
				if (CardNum() == 0)
				{
					return true;
				}
			}
		}

		// In Defend
		else if (gameManagerRef.GameState == GameStateEnum.Defend && canCoverTime > 0)
		{
			if (CardNum() > 0 && AreaStar() != CardBase.CardStarEnum.None)
			{
				if (card.CardStar_Re == AreaStar())
				{
					return true;
				}
				else if (card.CardStar_Re == CardBase.CardStarEnum.A)
				{
					return true;
				}
			}
		}


		return false;
	}

	public override void MoveCard(CardBase card)
	{
		if (CanAddCard(card))
		{
			base.MoveCard(card);
			if (gameManagerRef.GameState == GameStateEnum.Action)
			{
				AP -= 1;
			}
			if (gameManagerRef.GameState == GameStateEnum.Defend)
			{
				canCoverTime -= 1;
			}
		}
		else
		{
			GD.PrintErr("Can not add card");
		}
	}


}
