using Godot;
using System;

public partial class ScaledControl : Control
{
	private Vector2 customScale = new Vector2(1, 1);

	public Vector2 CustomScale { get => customScale; set { customScale = value; Console.Write($"CustomScale.set ! {value} appliquée."); } }
	public override void _Ready()
	{
		// Restaurer la propriété Scale
		Scale = CustomScale;
		Console.Write($"ScaledControl._Read() ! {CustomScale} appliqué.");

	}

	public override void _ExitTree()
	{
		// Conserver la propriété Scale
		CustomScale = Scale;
		Console.Write($"ScaledControl._ExitTree() ! {CustomScale} sauvé.");
	}
}