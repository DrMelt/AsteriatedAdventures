using Godot;
using System;
using System.Collections.Generic;


public enum CollectionLayers
{
	CardAreaLayer = 512,
	CardLayer = 1024,
}
public enum DragStateEnum
{
	None = 0,
	Drag,
	Back,
}

public partial class GameControl : Node
{
	[Export]
	float dragDistance = 0.2f;


	[Export]
	RayCast3D rayCastRef = null;

	HostInfo hostInfoRef;
	GameManager gameManagerRef;

	[Export]
	PlayerId screenPlayer = PlayerId.Player1; // Be Player1
	public PlayerId ScreenPlayer { get => screenPlayer; }

	int gameRoomId = 0;
	public int GameRoomId { get => gameRoomId; }

	CardBase cardBasePick = null;
	public CardBase CardBasePick { get => cardBasePick; }

	CardArea cardAreaPick = null;

	public CardArea CardAreaPick { get => cardAreaPick; }


	List<CardArea> validCardAreas = new List<CardArea>();
	public bool IsValidCardArea(CardArea cardArea)
	{
		return validCardAreas.Contains(cardArea);
	}

	public override void _Ready()
	{
		hostInfoRef = GetNode<HostInfo>(HostInfo.NodePath);
		gameManagerRef = GetNode<GameManager>(GetGamePath(hostInfoRef.HostInRoom) + nameof(GameManager));
	}

	public override void _Process(double delta)
	{
		ActivateCard();

		PickBody();
		UpdateCardsDragState();
		DragCard(); // Drag Card Transform

		UpdateValidCardAreas();
	}

	// Use cards
	void ActivateCard()
	{
		if (Input.IsActionJustReleased("press"))
		{
			if (cardBasePick != null && cardAreaPick != null)
			{
				if (cardBasePick.DragState == DragStateEnum.Drag)
				{
					if (validCardAreas.Contains(cardAreaPick))
					{
						cardAreaPick.MoveCard(cardBasePick);
						cardBasePick.CardState = CardBase.CardStateEnum.Normal;
					}
				}
			}
		}

		if (Input.IsActionJustReleased("press_r"))
		{
			if (cardBasePick != null && cardAreaPick != null)
			{
				if (cardBasePick.DragState == DragStateEnum.Back)
				{
					if (validCardAreas.Contains(cardAreaPick))
					{
						cardAreaPick.MoveCard(cardBasePick);
						cardBasePick.CardState = CardBase.CardStateEnum.Back;
					}
				}
			}
		}
	}

	void UpdateCardsDragState()
	{
		if (cardBasePick != null)
		{
			cardBasePick.CheckBeDrag();
		}
	}


	void DragCard()
	{
		if (cardBasePick == null)
		{
			return;
		}

		cardBasePick.CheckBeDrag();

		if (cardBasePick.DragState == DragStateEnum.None)
		{
			return;
		}

		Transform3D res = Transform3D.Identity;

		if (cardBasePick.DragState == DragStateEnum.Back)
		{
			res.Basis = res.Basis.Rotated(Vector3.Up, (float)Math.PI);
		}

		Vector2 mouseScreenPos = GetViewport().GetMousePosition();
		Camera3D camera = GetViewport().GetCamera3D();
		Vector3 localDir = camera.ProjectLocalRayNormal(mouseScreenPos);

		float dist = dragDistance;
		if (validCardAreas.Contains(cardAreaPick))
		{
			dist = 0.29f;
		}
		localDir *= -dist / Math.Min(localDir.Z, -0.2f);

		Vector3 offset = camera.GlobalTransform * localDir + camera.GlobalPosition;
		res.Basis = cardBasePick.GetTargetTrans.Basis * res.Basis;
		res.Origin = offset;

		cardBasePick.TempTargetTrans = res;
	}

	void PickBody()
	{
		Vector2 mouseScreenPos = GetViewport().GetMousePosition();
		Camera3D camera = GetViewport().GetCamera3D();
		Vector3 localDir = camera.ProjectLocalRayNormal(mouseScreenPos);

		rayCastRef.TargetPosition = localDir * 10;

		rayCastRef.CollisionMask = (int)CollectionLayers.CardLayer;
		rayCastRef.ForceRaycastUpdate();
		CardStaticBody cardCollider = rayCastRef.GetCollider() as CardStaticBody;
		if (cardCollider != null)
		{
			cardBasePick = cardCollider.ObjRef as CardBase;
			// GD.Print(cardBasePick.CardName + " " + cardBasePick.Name);
		}
		else
		{
			cardBasePick = null;
		}

		rayCastRef.CollisionMask = (int)CollectionLayers.CardAreaLayer;
		rayCastRef.ForceRaycastUpdate();
		CardStaticBody areaCollider = rayCastRef.GetCollider() as CardStaticBody;
		if (areaCollider != null)
		{
			cardAreaPick = areaCollider.ObjRef as CardArea;
			// GD.Print(areaCollider + " " + cardAreaPick);
		}
		else
		{
			cardAreaPick = null;
		}

	}

	void UpdateValidCardAreas()
	{
		validCardAreas.Clear();
		if (cardBasePick == null)
		{
			return;
		}

		foreach (CardArea cardArea in gameManagerRef.CardAreasRef)
		{
			if (cardBasePick.IsValidCardArea(cardArea))
			{
				validCardAreas.Add(cardArea);
			}
		}
	}

	public static string GetGamePath(int GameRoomId)
	{
		return "/root/Main/Games/" + "GameRoom_" + GameRoomId.ToString() + "/";
	}

	#region Player Input
	/// <summary>
	/// CardBase cardCheck, CardBase cardFrom
	/// </summary>
	public CardBase RequestOneCard(Func<CardBase, CardBase, bool> validCheck)
	{
		GD.PrintErr("Unfinished RequestOneCard");
		gameManagerRef.ProcessWaitPlayer();
		return null;
	}
	#endregion
}
