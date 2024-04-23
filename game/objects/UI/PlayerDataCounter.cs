using Godot;
using System;

public partial class PlayerDataCounter : ControlInGameBase
{
	[Export]
	PlayerId playerId = PlayerId.None;

	[ExportGroup("References")]
	[Export]
	Label HPLabelRef = null;
	[Export]
	Label EPLabelRef = null;
	[Export]
	Label Fragment0LabelRef = null;
	[Export]
	Label Fragment1LabelRef = null;
	[Export]
	Label Fragment2LabelRef = null;
	[Export]
	Label Fragment3LabelRef = null;
	[Export]
	Label Fragment4LabelRef = null;
	[Export]
	Label Fragment1_H_LabelRef = null;
	[Export]
	Label Fragment2_H_LabelRef = null;
	[Export]
	Label Fragment3_H_LabelRef = null;
	[Export]
	Label Fragment4_H_LabelRef = null;
	[Export]
	Label AsteriatedLabelRef = null;


	public override void _Process(double delta)
	{
		PlayerData playerData = gameManagerRef.GetPlayer(playerId);

		HPLabelRef.Text = playerData.HP.ToString();
		EPLabelRef.Text = playerData.EP.ToString();
		
		Fragment0LabelRef.Text = playerData.AsteriatedFragment0.ToString();
		Fragment1LabelRef.Text = playerData.AsteriatedFragment1.ToString();
		Fragment2LabelRef.Text = playerData.AsteriatedFragment2.ToString();
		Fragment3LabelRef.Text = playerData.AsteriatedFragment3.ToString();
		Fragment4LabelRef.Text = playerData.AsteriatedFragment4.ToString();

		Fragment1_H_LabelRef.Text = playerData.AsteriatedFragment1_H.ToString();
		Fragment2_H_LabelRef.Text = playerData.AsteriatedFragment2_H.ToString();
		Fragment3_H_LabelRef.Text = playerData.AsteriatedFragment3_H.ToString();
		Fragment4_H_LabelRef.Text = playerData.AsteriatedFragment4_H.ToString();

		AsteriatedLabelRef.Text = playerData.Asteriated.ToString();
	}
}
