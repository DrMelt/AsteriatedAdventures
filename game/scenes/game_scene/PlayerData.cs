using Godot;
using System;
using System.Collections.Generic;


public enum PlayerId
{
	None = -1,
	Player1 = 0,
	Player2 = 1,
}

public partial class PlayerData : Node
{
	public PlayerId playerId = PlayerId.None;

	int hp = 18;
	public int HP
	{
		get => hp; set
		{
			hp = value;
		}
	}

	int ep = 0;
	public int EP
	{
		get => ep; set
		{
			ep = Math.Clamp(value, 0, 10);
		}
	}

	int asteriated = 0;
	public int Asteriated
	{
		get => asteriated; set
		{
			asteriated = value;
		}
	}

	int asteriatedFragment0 = 0;
	public int AsteriatedFragment0
	{
		get => asteriatedFragment0;
		set
		{
			asteriatedFragment0 = value;
		}
	}
	int asteriatedFragment1 = 0;
	public int AsteriatedFragment1
	{
		get => asteriatedFragment1;
		set
		{
			asteriatedFragment1 = value;
		}
	}
	int asteriatedFragment1_H = 0;
	public int AsteriatedFragment1_H
	{
		get => asteriatedFragment1_H;
		set
		{
			asteriatedFragment1_H = value;
		}
	}
	int asteriatedFragment2 = 0;
	public int AsteriatedFragment2
	{
		get => asteriatedFragment2;
		set
		{
			asteriatedFragment2 = value;
		}
	}
	int asteriatedFragment2_H = 0;
	public int AsteriatedFragment2_H
	{
		get => asteriatedFragment2_H;
		set
		{
			asteriatedFragment2_H = value;
		}
	}
	int asteriatedFragment3 = 0;
	public int AsteriatedFragment3
	{
		get => asteriatedFragment3;
		set
		{
			asteriatedFragment3 = value;
		}
	}
	int asteriatedFragment3_H = 0;
	public int AsteriatedFragment3_H
	{
		get => asteriatedFragment3_H;
		set
		{
			asteriatedFragment3_H = value;
		}
	}
	int asteriatedFragment4 = 0;
	public int AsteriatedFragment4
	{
		get => asteriatedFragment4;
		set
		{
			asteriatedFragment4 = value;
		}
	}
	int asteriatedFragment4_H = 0;
	public int AsteriatedFragment4_H
	{
		get => asteriatedFragment4_H;
		set
		{
			asteriatedFragment4_H = value;
		}
	}



	[Export]
	int drawCardMax = 8;
	public int DrawCardMax { get => drawCardMax; set => drawCardMax = value; }

	[ExportGroup("Ref data")]
	[Export]
	HandCards handCards;


	public void AsteriatedReset()
	{
		while (AsteriatedFragment4_H > 0 && AsteriatedFragment0 > 0)
		{
			AsteriatedFragment0--;
			AsteriatedFragment4_H++;
		}
		while (AsteriatedFragment3_H > 0 && AsteriatedFragment0 > 0)
		{
			AsteriatedFragment0--;
			AsteriatedFragment3_H++;
		}
		while (AsteriatedFragment2_H > 0 && AsteriatedFragment0 > 0)
		{
			AsteriatedFragment0--;
			AsteriatedFragment2_H++;
		}
		while (AsteriatedFragment1_H > 0 && AsteriatedFragment0 > 0)
		{
			AsteriatedFragment0--;
			AsteriatedFragment1_H++;
		}
	}

	public void AsteriatedComposite()
	{
		while (AsteriatedFragment0 >= 4)
		{
			AsteriatedFragment0 -= 4;
			asteriated++;
		}

		while (AsteriatedFragment1 + AsteriatedFragment1_H >= 4)
		{
			int num = 0;

			while (num < 4)
			{
				if (AsteriatedFragment1_H > 0)
				{
					AsteriatedFragment1_H--;
					num++;
				}
				else
				{
					AsteriatedFragment1--;
					num++;
				}
			}

			asteriated++;
		}

		while (AsteriatedFragment2 + AsteriatedFragment2_H >= 4)
		{
			int num = 0;
			while (num < 4)
			{
				if (AsteriatedFragment2_H > 0)
				{
					AsteriatedFragment2_H--;
					num++;
				}
				else
				{
					AsteriatedFragment2--;
					num++;
				}
			}

			asteriated++;
		}


	}

}
