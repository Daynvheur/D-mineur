using Godot;
using System;

public partial class Case : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	//public override void _Ready()
	//{
	//}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//}
	
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mbe && mbe.ButtonIndex == MouseButton.Left && mbe.Pressed)
		{
			GD.Print("Left mouse button was pressed!");
		}
	}
}
