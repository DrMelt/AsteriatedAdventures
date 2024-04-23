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

	RoundStart = 1,
	Maintain_Uncover,

	Maintain_Reset,
	Maintain_Composite,
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

public enum EffectTimeEnum
{
	None = -1,
	RoundStart = 1,

	Timeline1_WinnerDecision = 1 + 0x10,
	Timeline2_CalculatingAttackDamage = 2 + 0x10,
	Timeline3_Attack = 3 + 0x10,
	Timeline4_AttackAfterEffect = 4 + 0x10,
	Timeline5_Reverse = 5 + 0x10,
	Timeline6_ReverseAfter = 6 + 0x10,
	Timeline7_DuelAfter = 7 + 0x10,

	TimelineMax,
}

public enum ActionTimeline
{

}

public class DuelData
{
	public int Damage = 0;
	public PlayerId BeDamagedPlayerId = PlayerId.None;
}


public partial class GameManager : Node
{
	[Export]
	GameStateEnum gameState = GameStateEnum.None;

	public GameStateEnum GameState { get => gameState; }



	#region Effects Process
	public enum ProcessStateEnum
	{
		WaitPlayer,
		Process,
	}

	[Export]
	ProcessStateEnum processState = ProcessStateEnum.WaitPlayer;
	public void ProcessWaitPlayer(){
		processState = ProcessStateEnum.WaitPlayer;
	}

	List<List<EffectData>> effectsProcessList =
	 new List<List<EffectData>>((int)EffectTimeEnum.TimelineMax);

	DuelData currentDuelData = null;
	public DuelData CurrentDuelData { get => currentDuelData; }



	public void AddProcess(CardEffect cardEffect, CardBase card)
	{
		EffectData newEffectData = new EffectData
		{
			cardEffect = cardEffect,
			card = card,
		};
		effectsProcessList[(int)cardEffect.EffectTime].Add(newEffectData);
	}

	void ProcessEffects()
	{
		while (HasUnhandledProcess())
		{
			EffectData cardEffectData = effectsProcessList[(int)gameState][0];
			effectsProcessList[(int)gameState].RemoveAt(0);

			cardEffectData.Effect();
		}
	}

	bool HasUnhandledProcess()
	{
		return effectsProcessList[(int)gameState].Count > 0;
	}

	void ClearProcessList()
	{
		for (int i = 0; i < effectsProcessList.Count; i++)
		{
			effectsProcessList[i].Clear();
		}
	}

	public void AttackDamage(PlayerId toPlayerId, int damage)
	{
		currentDuelData = new DuelData
		{
			BeDamagedPlayerId = toPlayerId,
			Damage = damage
		};
	}

	public void DrawCard(PlayerId playerId)
	{
		int ind = 0;
		if (playerId == PlayerId.Player1)
		{
			ind = 0;
		}
		else if (playerId == PlayerId.Player2)
		{
			ind = 1;
		}

		CardBase card = decksRef[ind].DrawCard();
		card.CardState = CardBase.CardStateEnum.InHand;
		handsRef[ind].MoveCard(card);
	}

	void InitGame()
	{
		decksRef[0].SetCardFromDeck(PlayerId.Player1);
		decksRef[0].SetCardsOwner(PlayerId.Player1);
		decksRef[1].SetCardFromDeck(PlayerId.Player2);
		decksRef[1].SetCardsOwner(PlayerId.Player2);

		Random random = new Random();
		int randNum = random.Next(2);

		for (int i = 0; i < 3; i++)
		{
			DrawCard(PlayerId.Player1);
			DrawCard(PlayerId.Player2);
		}
	}

	public void ToNext()
	{
		if (gameState == GameStateEnum.Init) { }
		else if (gameState == GameStateEnum.RoundStart)
		{

		}
		else if (gameState == GameStateEnum.Maintain_Uncover)
		{
			centerPanelRef.Uncover();
		}
		else if (gameState == GameStateEnum.Maintain_Reset)
		{
			GetPlayer(TurnPlayerId).AsteriatedReset();
			gameState = GameStateEnum.Maintain_Composite;
		}
		else if (gameState == GameStateEnum.Maintain_Composite)
		{
			GetPlayer(TurnPlayerId).AsteriatedComposite();
			gameState = GameStateEnum.Draw;
		}
		else if (gameState == GameStateEnum.Draw)
		{
			DoDraw();
			gameState = GameStateEnum.Action;
		}
		else if (gameState == GameStateEnum.Action)
		{
			gameState = GameStateEnum.Action_End;
			processState = ProcessStateEnum.Process;
		}
		else if (gameState == GameStateEnum.Action_End)
		{
			gameState = GameStateEnum.Defend;
			processState = ProcessStateEnum.WaitPlayer;
		}
		else if (gameState == GameStateEnum.Defend)
		{
			gameState = GameStateEnum.Duel;
			processState = ProcessStateEnum.Process;
		}
		else if (gameState == GameStateEnum.Duel)
		{
			DoDuel();
		}
	}

	void DoRoundStart()
	{
		foreach (CardArea cardArea in cardAreasRef)
		{
			cardArea.NewRoundReset();
		}

	}

	void DoActionEnd()
	{
		// Center Panel Align
		centerPanelRef.Align();
	}

	void DoAlign()
	{
		// Center Panel Align
		centerPanelRef.Align();

		// Re Panel Align
		if (turnPlayerId == PlayerId.Player1)
		{
			rePanelRef[0].Align();
		}
		else if (turnPlayerId == PlayerId.Player2)
		{
			rePanelRef[1].Align();

		}
	}

	void DoDraw()
	{
		int playerInd = 0;
		if (turnPlayerId == PlayerId.Player1)
		{ playerInd = 0; }
		else if (turnPlayerId == PlayerId.Player2)
		{ playerInd = 1; }

		for (int i = 0; i < 3; i++)
		{
			if (handsRef[playerInd].CardNum() >= 8)
			{
				break;
			}
			DrawCard(turnPlayerId);
		}

	}

	void DoDuel()
	{
		centerPanelRef.Duel();
	}


	#endregion

	[ExportGroup("Data Ref")]
	Godot.Collections.Array<CardArea> cardAreasRef = new Godot.Collections.Array<CardArea>();
	public Array<CardArea> CardAreasRef { get => cardAreasRef; }

	Godot.Collections.Array<CardBase> cardsRef = new Godot.Collections.Array<CardBase>();

	[ExportSubgroup("Set before game")]
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
	public PlayerId GetAdversePlayerId(PlayerId playerId)
	{
		if (playerId == PlayerId.Player1)
		{
			return PlayerId.Player2;
		}
		else if (playerId == PlayerId.Player2)
		{
			return PlayerId.Player1;
		}
		else
		{
			return PlayerId.None;
		}
	}

	[Export]
	CenterPanel centerPanelRef = null;
	[Export]
	Godot.Collections.Array<RePanel> rePanelRef = new Array<RePanel>();
	public RePanel GetRePanelRef(PlayerId playerId)
	{
		if (playerId == PlayerId.Player1)
		{
			return rePanelRef[0];
		}
		else if (playerId == PlayerId.Player2)
		{
			return rePanelRef[1];
		}
		else
		{
			return null;
		}
	}


	[Export]
	Godot.Collections.Array<CardArea> handsRef = new Godot.Collections.Array<CardArea>();
	public CardArea GetHandRef(PlayerId playerId)
	{
		if (playerId == PlayerId.Player1)
		{
			return handsRef[0];
		}
		else if (playerId == PlayerId.Player2)
		{
			return handsRef[1];
		}
		else
		{
			return null;
		}
	}


	[Export]
	Godot.Collections.Array<CardArea> decksRef = new Godot.Collections.Array<CardArea>();
	public CardArea GetDecksRef(PlayerId playerId)
	{
		if (playerId == PlayerId.Player1)
		{
			return decksRef[0];
		}
		else if (playerId == PlayerId.Player2)
		{
			return decksRef[1];
		}
		else
		{
			return null;
		}
	}


	[Export]
	Godot.Collections.Array<CardArea> discardPileRef = new Godot.Collections.Array<CardArea>();
	public CardArea GetDiscardPileRef(PlayerId playerId)
	{
		if (playerId == PlayerId.Player1)
		{
			return discardPileRef[0];
		}
		else if (playerId == PlayerId.Player2)
		{
			return discardPileRef[1];
		}
		else
		{
			return null;
		}
	}


	[ExportGroup("Data")]
	[Export]
	Godot.Collections.Array<int> fragments = new Godot.Collections.Array<int>();

	[Export]
	PlayerId turnPlayerId = PlayerId.None;
	public PlayerId TurnPlayerId { get => turnPlayerId; }


	public int FragmentsNum()
	{
		return fragments.Count;
	}

	public int GetFragmentTop()
	{
		if (fragments.Count > 0)
		{
			int res = fragments[fragments.Count - 1];
			fragments.RemoveAt(fragments.Count - 1);
			return res;
		}
		else
		{
			return 0;
		}
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
	public override void _Ready()
	{
		GetNode<CallInit>("../" + nameof(CallInit)).InitGameEvent += InitGame;
	}

	public override void _Process(double delta)
	{
		// For Debug
		if (Input.IsActionJustPressed("debug"))
		{
			DoActionEnd();
		}

		while (processState == ProcessStateEnum.Process)
		{
			ToNext();
		}
	}


}
