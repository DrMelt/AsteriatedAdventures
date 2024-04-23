using Godot;
using System;

public partial class RePanel : CardPanel
{

	public override void Align()
	{
		for (int i = 0; i < 2; i++)
		{
			CardArea checkArea = cardAreaRef[i];
			if (checkArea.CardNum() == 0)
			{
				CardArea cardArea = null;
				if (cardAreaRef[3].CardNum() > 0)
				{
					cardArea = cardAreaRef[3];
				}

				if (cardArea != null)
				{
					checkArea.MoveCards(cardArea);
				}
			}
		}
	}


}
