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

	public override void _Process(double delta)
	{
		ShowCards();
	}

	void ShowCards()
	{
		if (cardsRef.Count < 1)
		{
			return;
		}
		float startZ = -intervalZ * (cardsRef.Count - 1) * 0.5f;
		float startRadian = -intervalRadian * (cardsRef.Count - 1) * 0.5f;

		for (int i = 0; i < cardsRef.Count; i++)
		{
			Transform3D trans = Transform3D.Identity;
			trans = trans.Rotated(Vector3.Forward, startRadian);
			Vector3 offset = radius * Vector3.Up.Rotated(Vector3.Forward, startRadian);

			offset += -Vector3.Forward * startZ;
			trans = trans.Translated(offset);

			trans = GlobalTransform * trans;
			cardsRef[i].TargetTrans = trans;

			startZ += intervalZ;
			startRadian += intervalRadian;
		}
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
