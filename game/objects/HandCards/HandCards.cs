using Godot;
using System;

public partial class HandCards : CardArea
{
	[Export]
	float radius = 0.1f;
	[Export]
	float intervalRadian = 0.1f;
	[Export]
	float intervalZ = 0.001f;

	public override void _Ready()
	{
		base._Ready();
	}

	public override Transform3D GetTrans(int cardInd)
	{
		float startZ = -intervalZ * (cardsRef.Count - 1) * 0.5f;
		startZ += cardInd * intervalZ;
		float startRadian = -intervalRadian * (cardsRef.Count - 1) * 0.5f;
		startRadian += cardInd * intervalRadian;

		Transform3D trans = Transform3D.Identity;


		trans = trans.Rotated(Vector3.Forward, startRadian);
		Vector3 offset = radius * Vector3.Up.Rotated(Vector3.Forward, startRadian);

		offset += -Vector3.Forward * startZ;
		trans = trans.Translated(offset);

		trans = GlobalTransform * trans;
		return trans;
	}

	public override int CardNum()
	{
		return cardsRef.Count;
	}

	public override bool CanAddCard(CardBase card)
	{
		PlayerData playerData = gameManagerRef.GetPlayer(gameControlRef.ScreenPlayer);

		if (cardsRef.Count < playerData.DrawCardMax && gameManagerRef.GameState == GameStateEnum.Draw)
		{
			return true;
		}

		return false;
	}
}
