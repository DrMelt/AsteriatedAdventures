using Godot;
using System;

public partial class ControlInGameBase : Control
{
    protected HostInfo hostInfoRef = null;
    protected GameManager gameManagerRef = null;
    protected GameControl gameControlRef = null;

    public override void _Ready()
    {
        hostInfoRef = GetNode<HostInfo>(HostInfo.NodePath);
        gameManagerRef = GetNode<GameManager>(GameControl.GetGamePath(hostInfoRef.HostInRoom) + nameof(GameManager));
        gameControlRef = GetNode<GameControl>(GameControl.GetGamePath(hostInfoRef.HostInRoom) + nameof(GameControl));
    }
}
