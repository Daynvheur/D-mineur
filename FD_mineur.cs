#nullable enable

using Godot;
using System;

public partial class FD_mineur : Control
{
	[Export]
	public bool isGameOver = false;
	private double elapsedTime = 0;

	private Timer? timer;
	private Label? tsslGameOver;
	private Label? tsslTemps;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//PrintTree();
		timer = (Timer)GetNodeOrNull("timer");
		timer.Start();
		
		tsslGameOver = (Label)GetNodeOrNull("VBoxContainer/HBoxContainer/tsslGameOver");
		tsslGameOver.Visible = isGameOver;
		
		tsslTemps = (Label)GetNodeOrNull("VBoxContainer/HBoxContainer/tsslTemps");
		tsslTemps.Text = FormatTime(elapsedTime);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Mettre à jour le temps écoulé
		elapsedTime += delta;

		tsslTemps.Text = FormatTime(elapsedTime);
	}
	
	public void SetGameOver(bool gameOver = true)
	{
		isGameOver = gameOver;
		if (!(tsslGameOver is null)) tsslGameOver.Visible = gameOver;
	}
	
	private string FormatTime(double time)
	{
		int heures = (int)time / 3600;
		int minutes = (int)(time % 3600) / 60;
		double secondes = (double)time % 60;
		return string.Format("{0:00}:{1:00}", minutes, secondes);
	}
	
	private static (int entier, double reste) GetDivMod(double time, int baseValue, bool forceSign = false)
	{
		double reste = time;
		int entier = 0;
		if (baseValue > 0)
		{
			while (reste >= baseValue)
			{
				entier++;
				reste -= baseValue;
			}
			while (forceSign && reste < 0)
			{
				entier--;
				reste += baseValue;
			}
		}
		else if (baseValue < 0)
		{
			while (reste <= baseValue)
			{
				entier++;
				reste += baseValue;
			}
			while (forceSign && reste > 0)
			{
				entier--;
				reste -= baseValue;
			}
		}
		//else noop;
		
		return (entier, reste);
	}
}
