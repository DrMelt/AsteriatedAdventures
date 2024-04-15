using Godot;
using System;

public class CardEffect
{
	public enum BaseTypeEnum
	{
		Passive,
		Response,
	}

	public void SortRule() { }
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
	CardStateEnum cardState = CardStateEnum.Normal;
	public CardStateEnum CardState { get => cardState; set => cardState = value; }

	[Export]
	string cardName = "Card";
	public string CardName { get => cardName; }

	[Export]
	CardStarEnum cardStar = CardStarEnum.A;
	public CardStarEnum CardStar { get => cardStar; }
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

	[Export]
	PlayerId ownerId = PlayerId.None;

	[Export]
	bool isCardPublicity = false;


	CardArea cardArea = null;

	public virtual void RegisterCard()
	{
		HostInfo hostInfo = GetNode<HostInfo>(HostInfo.NodePath);
		GameManager gameManager = GetNode<GameManager>(GameControl.GetGamePath(hostInfo.HostInRoom) + nameof(GameManager));

		gameManager.RegisterCard(this);
	}



	public override void _Ready()
	{
		base._Ready();
		targetTrans = GlobalTransform;
	}

	public override void _Process(double delta)
	{
		cardHintInRange.Visible = gameControlRef.CardBasePick == this;
		CheckBeDrag();
		DragCard();
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
	Transform3D targetTrans = Transform3D.Identity;
	public Transform3D TargetTrans { get => targetTrans; set { preTrans = GlobalTransform; targetTrans = value; movedTime = 0.0; } }

	Transform3D preTrans = Transform3D.Identity;


	double movedTime = 0.0;

	[Export]
	const double moveTime = 0.5;

	void MoveToTarget(double delta)
	{
		if (isBeDrag)
		{
			return;
		}

		if (movedTime > moveTime)
		{
			return;
		}

		movedTime += delta;
		double percent = Math.Clamp(movedTime / moveTime, 0.0, 1.0);
		GlobalTransform = preTrans.InterpolateWith(TargetTrans, (float)percent);

	}
	#endregion

	#region DragCard	
	[Export]
	float dragDistance = 0.2f;

	bool isBeDrag = false;
	void CheckBeDrag()
	{
		if (gameControlRef.ScreenPlayer != ownerId && isCardPublicity)
		{
			isBeDrag = false;
		}

		if (gameControlRef.CardBasePick == this && Input.IsActionPressed("press"))
		{
			isBeDrag = true;
		}
		else
		{
			if (isBeDrag)
			{
				movedTime = 0.0f;
				preTrans = GlobalTransform;
			}

			isBeDrag = false;
		}
	}

	void DragCard()
	{
		if (!isBeDrag)
		{
			return;
		}

		Vector2 mouseScreenPos = GetViewport().GetMousePosition();
		Camera3D camera = GetViewport().GetCamera3D();
		Vector3 localDir = camera.ProjectLocalRayNormal(mouseScreenPos);

		localDir *= -dragDistance / Math.Min(localDir.Z, -0.2f);

		GlobalPosition = camera.GlobalTransform * localDir + camera.GlobalPosition;

	}
	#endregion


	public virtual bool CanBeCover()
	{
		return cardState == CardStateEnum.Normal && cardArea.CardNum() == 1;
	}


	public virtual void OnCardWin() { }
	public virtual void OnCardLose() { }

	public virtual void OnCardLeave() { }

	public virtual void OnCardUsed() { }
	public virtual void OnCardTurnOn()
	{

	}

}