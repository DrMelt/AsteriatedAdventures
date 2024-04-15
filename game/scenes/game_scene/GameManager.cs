/// <summary>
/// 逻辑处理，不会涉及任何交互、UI
/// </summary>


using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;




public enum GameStateEnum
{
	None = -1,
	Init = 0,

	RoundStart,
	Maintain_Turn,

	Maintain_Reset,
	Maintain_Combine,
	Draw,
	Action,
	Action_End,
	Defend,
	Duel,
	Align,
	RoundEnd,

	End,

	StateMax,
}

public enum ActionTimeline
{
	Start = 0,
	Timeline1_WinnerDecision = 1,
	Timeline2_CalculatingAttackDamage = 2,
	Timeline3_Attack = 3,
	Timeline4_AttackAfterEffect = 4,
	Timeline5_Reverse = 5,
	Timeline6_ReverseAfter = 6,
	Timeline7_DuelAfter = 7,
}

public class DuelData
{
	public int Damage = 0;
	public PlayerId BeDamagedPlayerId = PlayerId.None;
}


public partial class GameManager : Node
{
	[Export]
	Godot.Collections.Array<PlayerData> playersRef = new Godot.Collections.Array<PlayerData>();

	public PlayerData GetPlayer(PlayerId playerId)
	{
		foreach (PlayerData player in playersRef)
		{
			if (player.playerId == playerId)
			{
				return player;
			}
		}
		return null;
	}


	[Export]
	GameStateEnum gameState = GameStateEnum.None;

	public GameStateEnum GameState { get => gameState; }

	List<List<CardEffect>> processList =
	 new List<List<CardEffect>>((int)GameStateEnum.StateMax);

	[ExportGroup("Data Ref")]
	[Export]
	Godot.Collections.Array<CardArea> cardAreasRef = new Godot.Collections.Array<CardArea>();

	[Export]
	Godot.Collections.Array<CardBase> cardsRef = new Godot.Collections.Array<CardBase>();

	[Export]
	Godot.Collections.Array<CardArea> decksRef = new Godot.Collections.Array<CardArea>();
	public Array<CardArea> DecksRef { get => decksRef; }
	
	[ExportGroup("Data")]
	[Export]
	Godot.Collections.Array<int> fragments = new Godot.Collections.Array<int>();

	public int FragmentsNum()
	{
		return fragments.Count;
	}

	public int GetFragmentTop()
	{
		int res = fragments[fragments.Count - 1];
		fragments.RemoveAt(fragments.Count - 1);
		return res;
	}


	public void RegisterArea(CardArea cardArea)
	{
		cardAreasRef.Add(cardArea);
	}
	public void UnRegisterArea(CardArea cardArea)
	{
		cardAreasRef.Remove(cardArea);
	}
	public void RegisterCard(CardBase cardBase)
	{
		cardsRef.Add(cardBase);
	}

	public void UnRegisterCard(CardBase cardBase)
	{
		cardsRef.Remove(cardBase);
	}

	public void RegisterDeck(CardArea cardArea)
	{
		decksRef.Add(cardArea);
	}

	public void UnRegisterDeck(CardArea cardArea)
	{
		decksRef.Remove(cardArea);
	}

	public Godot.Collections.Array<CardArea> GetAvailableAreas(CardBase cardBase)
	{
		Godot.Collections.Array<CardArea> areas = new Godot.Collections.Array<CardArea>();
		foreach (CardArea cardArea in cardAreasRef)
		{

		}
		return areas;
	}


	void InitFragments()
	{
		fragments.Clear();
		for (int i = 0; i < 5; i++)
		{
			fragments.Add(0);
		}
		for (int i = 0; i < 7; i++)
		{
			fragments.Add(1);
		}
		for (int i = 0; i < 8; i++)
		{
			fragments.Add(2);
		}
		for (int i = 0; i < 6; i++)
		{
			fragments.Add(3);
		}
		for (int i = 0; i < 4; i++)
		{
			fragments.Add(4);
		}
		fragments.Shuffle();
	}
	public void InitGame()
	{

	}




}
