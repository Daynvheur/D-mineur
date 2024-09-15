using Godot;
using System;

public partial class ScaledControl : Control
{
	private float zoom = 1;

	public Vector2 CustomScale => Zoom * Scale;

	public float Zoom
	{
		get => zoom;
		set
		{
			zoom = value;
			CustomMinimumSize = CustomScale;
			Scale = CustomScale; QueueRedraw();
		}
	}

	public override void _Ready()
	{
		Console.Write($"ScaledControl._Read() ! {CustomScale} appliqué.");

	}

	public override void _Draw()
	{
		Console.Write($"ScaledControl._Draw() ! {CustomScale} appliqué.");
	}
}
