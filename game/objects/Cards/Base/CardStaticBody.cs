using Godot;
using System;

public partial class CardStaticBody : StaticBody3D
{
	[Export]
	Node objRef = null;
	public Node ObjRef { get => objRef; }

}


