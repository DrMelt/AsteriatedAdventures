using Godot;
using System;

public partial class Button_ToNext : ButtonInGameBase
{
  public override void _Process(double delta)
  {
    Visible = false;

    if (gameManagerRef.GameState == GameStateEnum.Action)
    {
      if (gameManagerRef.TurnPlayerId == gameControlRef.ScreenPlayer)
      {
        Visible = true;
        Text = "结束行动";
      }
    }
    else if (gameManagerRef.GameState == GameStateEnum.Defend)
    {
      if (gameManagerRef.GetAdversePlayerId(gameManagerRef.TurnPlayerId) == gameControlRef.ScreenPlayer)
      {
        Visible = true;
        Text = "结束防御";
      }
    }

  }

  public override void _Pressed()
  {
    gameManagerRef.ToNext();
  }
}

