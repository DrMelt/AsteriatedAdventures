using Godot;
using System;
using System.Collections.Generic;

public partial class CenterPanel : CardPanel
{
	public List<CardBase.CardStarEnum> ExistedStars()
	{
		int panelIndStart = 0;
		if (gameManagerRef.TurnPlayerId == PlayerId.Player1)
		{
			panelIndStart = 0;
		}
		else if (gameManagerRef.TurnPlayerId == PlayerId.Player2)
		{
			panelIndStart = 3;
		}
		List<CardBase.CardStarEnum> stars = new List<CardBase.CardStarEnum>(3);
		for (int i = 0; i < 3; i++)
		{
			CardBase.CardStarEnum star = cardAreaRef[i + panelIndStart].AreaStar();
			if (star != CardBase.CardStarEnum.None)
			{
				stars.Add(star);
			}
		}

		return stars;
	}


	public void Uncover()
	{
		foreach (CardArea area in cardAreaRef)
		{
			area.Uncover();
		}
	}


	public override void Align()
	{
		int panelIndStart = 0;
		if (gameManagerRef.TurnPlayerId == PlayerId.Player1)
		{
			panelIndStart = 0;
		}
		else if (gameManagerRef.TurnPlayerId == PlayerId.Player2)
		{
			panelIndStart = 3;
		}

		for (int i = 0; i < 2; i++)
		{
			CardArea checkArea = cardAreaRef[i + panelIndStart];
			if (checkArea.CardNum() == 0)
			{
				for (int j = i + 1; j < 3; j++)
				{
					if (cardAreaRef[j + panelIndStart].CardNum() > 0)
					{
						CardArea cardArea = cardAreaRef[j + panelIndStart];
						checkArea.MoveCards(cardArea);
						break;
					}
				}
			}
		}
	}

	public void Duel()
	{
		for (int i = 0; i < 3; i++)
		{
			CenterPanelPos cardArea1 = cardAreaRef[i] as CenterPanelPos;
			CenterPanelPos cardArea2 = cardAreaRef[i + 3] as CenterPanelPos;

			CenterPanelPos.Duel(cardArea1, cardArea2);
		}
	}


}
