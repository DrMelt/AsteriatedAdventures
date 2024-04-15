using Godot;
using System;


public enum CollectionLayers
{
	CardAreaLayer = 512,
	CardLayer = 1024,
}

public partial class GameControl : Node
{
	[Export]
	RayCast3D rayCastRef = null;

	[Export]
	public PlayerId ScreenPlayer = PlayerId.None;

	[Export]
	int GameRoomId = 0;
	public int GameRoomId1 { get => GameRoomId; }

	[Export]
	CardArea cardAreaPick = null;

	public CardArea CardAreaPick { get => cardAreaPick; }

	[Export]
	CardBase cardBasePick = null;
	public CardBase CardBasePick { get => cardBasePick; }

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		PickBody();
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
			// GD.Print(cardCollider + " " + cardBasePick);
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


	public static string GetGamePath(int GameRoomId)
	{
		return "/root/Main/Games/" + "GameRoom_" + GameRoomId.ToString() + "/";
	}

}
