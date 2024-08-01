using Godot;
using System;

public partial class Button_Test : Button
{
	// Le signal "pressed" sera automatiquement connecté

	// Vous pouvez ajouter votre propre logique ici
	private void OnButtonPressed()
	{
		Console.WriteLine("Bouton cliqué !");
		// Autres actions à effectuer lors du clic
	}
	
	public override void _Pressed()
	{
		// Gérer le clic gauche
		if (Input.IsActionJustPressed("ui_accept"))
		{
			Console.WriteLine("Clic gauche !");
		}
	}

	public void OnButtonInput(InputEvent @event)
	{
		// Gérer le clic droit
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Right)
		{
			Console.WriteLine($"{{{nameof(mouseEvent.ButtonIndex)}:{mouseEvent.ButtonIndex},{nameof(mouseEvent.ButtonMask)}:{mouseEvent.ButtonMask},{nameof(mouseEvent.Pressed)}:{mouseEvent.Pressed}}}");
		}
	}
}
