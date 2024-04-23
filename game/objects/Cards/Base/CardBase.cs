using Godot;
using System;



public enum CardUsedType
{


}

public partial class CardBase : Node3DInGameBase
{
	public enum CardStarEnum
	{
		None = -1,
		A = 0,
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
	}

	[Flags]
	public enum CardStateEnum
	{
		Normal = 0x01,
		Reversed = 0x02,
		Horizontal = 0x04,
		Back = 0x08,
		Back_H = Back + Horizontal,
		Back_Re = Back + Reversed,
		InHand = 0x16,
	}

	public enum CardElentEnum
	{
		None,
		Earth,
		Water,
		Fire,
		Wind,
		Thunder,
		Light,
		Dark,
	}

	public enum CardBackTypeEnum
	{
		Cannot,
		Normal,
		Red,
	}

	public enum CardCoverTypeEnum
	{
		Cannot,
		Normal,
	}


	[ExportGroup("Data")]
	[ExportSubgroup("Base Data")]
	[Export]
	string cardName = "Card";
	public string CardName { get => cardName; }

	[Export]
	CardStarEnum cardStar = CardStarEnum.A;
	public CardStarEnum CardStar { get => cardStar; }

	public CardStarEnum GetBaseCardStar()
	{
		if (cardState != CardBase.CardStateEnum.Reversed)
		{
			return cardStar;
		}
		else
		{
			return cardStar_Re;
		}
	}

	[Export]
	private CardStarEnum cardStar_Re = CardStarEnum.A;
	public CardStarEnum CardStar_Re { get => cardStar_Re; }
	[Export]
	CardBackTypeEnum cardBackType = CardBackTypeEnum.Normal;
	public CardBackTypeEnum CardBackType { get => cardBackType; }

	[Export]
	CardCoverTypeEnum cardCoverType = CardCoverTypeEnum.Normal;
	public CardCoverTypeEnum CardCoverType { get => cardCoverType; }



	[Export]
	CardElentEnum cardElent = CardElentEnum.None;
	public CardElentEnum CardElent { get => cardElent; }

	[ExportSubgroup("Scene Data")]
	[Export]
	PlayerId fromDeskId = PlayerId.None;
	public PlayerId FromDeskId { get => fromDeskId; set => fromDeskId = value; }

	[Export]
	PlayerId ownerId = PlayerId.None;
	public PlayerId OwnerId { get => ownerId; set => ownerId = value; }

	[Export]
	CardArea inCardAreaRef = null; // initial in CardArea
	public CardArea InCardAreaRef
	{
		get => inCardAreaRef;
		set
		{
			inCardAreaRef = value;

			TargetTrans = value.GetTrans(this);
		}
	}

	[Export]
	bool isCardPublicity = false;

	[Export]
	CardStateEnum cardState = CardStateEnum.Normal;
	public CardStateEnum CardState { get => cardState; set => cardState = value; }


	public virtual void RegisterCard()
	{
		gameManagerRef.RegisterCard(this);
	}



	public override void _Ready()
	{
		base._Ready();
		targetTrans = GlobalTransform;
	}

	public override void _Process(double delta)
	{
		cardHintInRange.Visible = gameControlRef.CardBasePick == this;

		UpdateTargetTrans();

		MoveToTarget(delta);
	}

	[ExportGroup("Interaction Data")]
	[Export]
	Node3D cardHintRef = null;
	[Export]
	Node3D cardHintInRange = null;

	[Export]
	bool showHint = false;

	public bool ShowHint { get => showHint; set { showHint = value; cardHintRef.Visible = showHint; } }



	#region Move to Target Pos
	[Export]
	Transform3D tempTargetTrans = Transform3D.Identity;
	public Transform3D TempTargetTrans
	{
		get => tempTargetTrans;
		set
		{
			tempTargetTrans = value;
		}
	}

	[Export]
	Transform3D targetTrans = Transform3D.Identity;
	public Transform3D GetTargetTrans
	{
		get => targetTrans;
	}
	protected Transform3D TargetTrans
	{
		get => targetTrans;
		set
		{
			preTrans = GlobalTransform;
			targetTrans = value; movedTime = 0.0;
		}
	}

	protected void UpdateTargetTrans()
	{
		// if (inCardAreaRef.AreaName == CardAreaNames.Hand)
		{
			TargetTrans = inCardAreaRef.GetTrans(this, cardState);
		}
	}


	Transform3D preTrans = Transform3D.Identity;

	double movedTime = 0.0;

	void ResetMoveToTarget()
	{
		movedTime = 0.0f;
		preTrans = GlobalTransform;
	}

	[Export]
	const double moveTime = 0.5;

	void MoveToTarget(double delta)
	{
		if (dragState != DragStateEnum.None)
		{
			GlobalTransform = tempTargetTrans;
			return;
		}
		else
		{
			if (movedTime > moveTime)
			{
				return;
			}

			movedTime += delta;
			double percent = Math.Clamp(movedTime / moveTime, 0.0, 1.0);
			GlobalTransform = preTrans.InterpolateWith(TargetTrans, (float)percent);
		}
	}
	#endregion


	#region DragCard
	DragStateEnum dragState = DragStateEnum.None;
	public DragStateEnum DragState { get => dragState; }

	public void CheckBeDrag()
	{
		Action unDrag = () =>
		{
			if (dragState != DragStateEnum.None)
			{
				ResetMoveToTarget();
			}

			dragState = DragStateEnum.None;
			return;
		};

		if (inCardAreaRef.AreaName == CardAreaNames.Deck || inCardAreaRef.AreaName == CardAreaNames.Discard)
		{
			unDrag.Invoke();
			return;
		}

		if (gameControlRef.ScreenPlayer != ownerId && !isCardPublicity)
		{
			unDrag.Invoke();
			return;
		}

		if (Input.IsActionPressed("press_r") && Input.IsActionPressed("press"))
		{
			unDrag.Invoke();
			return;
		}


		if (gameControlRef.CardBasePick == this && Input.IsActionPressed("press"))
		{
			dragState = DragStateEnum.Drag;
			return;
		}
		else if (gameControlRef.CardBasePick == this && Input.IsActionPressed("press_r") && inCardAreaRef.AreaName == CardAreaNames.Hand)
		{
			dragState = DragStateEnum.Back;
			return;
		}
		else
		{
			unDrag.Invoke();
			return;
		}
	}
	#endregion

	#region Valid check

	/// <summary>
	/// 启动！
	/// </summary>
	/// <param name="cardArea"></param>
	/// <returns></returns>
	bool ValidCheckInAction(CardArea cardArea)
	{
		// From
		if (inCardAreaRef.AreaName == CardAreaNames.Hand || inCardAreaRef.AreaName == CardAreaNames.ReTable)
		{
			// To
			if (cardArea.AreaName == CardAreaNames.Table && cardArea.OwnerId == gameControlRef.ScreenPlayer)
			{
				if (cardArea.CanAddCard(this))
				{
					return true;
				}
			}
		}

		return false;
	}
	/// <summary>
	/// 盖卡
	/// </summary>
	/// <param name="cardArea"></param>
	/// <returns></returns>
	bool ValidCheckInBack(CardArea cardArea)
	{


		return false;
	}

	bool ValidCheckInDefend(CardArea cardArea)
	{


		return false;
	}

	public bool IsValidCardArea(CardArea cardArea)
	{
		if (gameManagerRef.TurnPlayerId == gameControlRef.ScreenPlayer)
		{
			if (ownerId == gameControlRef.ScreenPlayer)
			{
				if (gameManagerRef.GameState == GameStateEnum.Action)
				{
					return ValidCheckInAction(cardArea);
				}
			}
		}
		else
		{
			if (gameManagerRef.GameState == GameStateEnum.Defend)
			{
				return ValidCheckInDefend(cardArea);
			}
		}

		return false;
	}



	#endregion

	#region Card Actions
	/// <summary>
	/// 揭示
	/// </summary>
	public virtual void Uncover()
	{
		if ((int)(cardState & CardStateEnum.Back) > 0)
		{
			PlayerData player = gameManagerRef.GetPlayer(ownerId);
			if (cardBackType == CardBackTypeEnum.Normal)
			{
				player.HP -= (int)cardStar;
			}
			else if (cardBackType == CardBackTypeEnum.Red)
			{
				player.HP -= 5;
			}
		}
	}

	public virtual void Reverse()
	{

	}

	#endregion

	#region Cards effects

	void AddEffects(Godot.Collections.Array<CardEffect> cardEffects)
	{
		foreach (CardEffect effect in cardEffects)
		{
			bool isOwnTurn = gameControlRef.ScreenPlayer == gameManagerRef.TurnPlayerId;
			if ((effect.ValidCardState & cardState) > 0)
			{
				if (((effect.EffectTrun & CardEffect.EffectTrunEnum.Own) > 0 && isOwnTurn) ||
					((effect.EffectTrun & CardEffect.EffectTrunEnum.Adverse) > 0 && !isOwnTurn))
				{
					gameManagerRef.AddProcess(effect, this);
				}
			}
		}
	}

	[ExportGroup("Card Effects")]
	[Export]
	Godot.Collections.Array<CardEffect> effectsOnRoundStart = new Godot.Collections.Array<CardEffect>();
	[Export]
	Godot.Collections.Array<CardEffect> effectsOnWin = new Godot.Collections.Array<CardEffect>();
	[Export]
	Godot.Collections.Array<CardEffect> effectsOnLose = new Godot.Collections.Array<CardEffect>();
	[Export]
	Godot.Collections.Array<CardEffect> effectsOnLeave = new Godot.Collections.Array<CardEffect>();
	[Export]
	Godot.Collections.Array<CardEffect> effectsOnUsed = new Godot.Collections.Array<CardEffect>();

	#endregion

	public virtual void OnRoundStart()
	{
		if (ValidCardOnRoundStart())
		{ AddEffects(effectsOnRoundStart); }
	}
	public virtual bool ValidCardOnRoundStart()
	{
		return false;
	}

	public virtual void OnCardWin()
	{
		AddEffects(effectsOnWin);
	}
	public virtual void OnCardLose()
	{
		AddEffects(effectsOnLose);
	}

	public virtual void OnCardLeave()
	{
		AddEffects(effectsOnLeave);
	}

	public virtual void OnCardUsed()
	{
		AddEffects(effectsOnUsed);
	}


}