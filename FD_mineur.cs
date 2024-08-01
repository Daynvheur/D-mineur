using Godot;
using System;

public partial class FD_mineur : Control
{
	[Export]
	public bool isGameOver = false;

	[Export]
	public HBoxContainer? HBoxContainer { get; set; }
	[Export]
	public Label? TsslReste { get; set; }
	[Export]
	public Label? TsslTotal { get; set; }
	[Export]
	public ProgressBar? ProgressBar { get; set; }
	[Export]
	public Label? TsslGameOver { get; set; }
	[Export]
	public Label? TsslTemps { get; set; }
	[Export]
	public Timer? Timer { get; set; }

	[Export]
	public double elapsedTime = 0;

	public FD_mineur() : base()
	{
		Plateau.UpdateMines = (int min, int marques, int max) =>
		{
			TsslReste?.SetText((max - marques - min).ToString());
			TsslTotal?.SetText(max.ToString());
			ProgressBar?.SetMin(min);
			ProgressBar?.SetValue(marques);
			ProgressBar?.SetMax(max);
		};
		Plateau.AddCase = (int x, int y) =>
		{
			var bouton = new Button() { Position = new(x, y), Size = new(Case.Size_x, Case.Size_y) };
			AddChild(bouton);
			return bouton;
		};
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TsslGameOver?.SetVisible(isGameOver);
		TsslTemps?.SetText(FormatTime(elapsedTime));

		Plateau.CaseClick = (@case) => (@event) => Plateau.InteractionDispatcher(@event, @case); //Plateau.InteractionDispatcher(@event, @case);
		Plateau.InitialisePlateau();

		GetWindow().Size = new(Plateau.X * Case.Size_x, (Plateau.Y * Case.Size_y) + (int)(HBoxContainer?.Size.Y ?? 0));
		Timer?.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;

		TsslTemps?.SetText(isGameOver ? "(Temps écoulé)" : FormatTime(elapsedTime));
	}

	public void SetGameOver(bool gameOver = true)
	{
		isGameOver = gameOver;
		TsslGameOver?.SetVisible(gameOver);
	}

	private static string FormatTime(double time)
	{
		var (heures, reste) = GetDivMod(time, 3600);
		var (minutes, secondes) = GetDivMod(reste, 60);
		return $"{heures:00}:{minutes:00}:{secondes:00}";
	}

	private static (int entier, double reste) GetDivMod(double valeur, double baseValeur, bool forceSign = false)
	{
		double reste = valeur;
		int entier = 0;
		switch (baseValeur)
		{
			case > 0:
				while (reste >= baseValeur)
				{
					entier++;
					reste -= baseValeur;
				}
				while (forceSign && reste < 0)
				{
					entier--;
					reste += baseValeur;
				}
				break;

			case < 0:
				while (reste <= baseValeur)
				{
					entier++;
					reste += baseValeur;
				}
				while (forceSign && reste > 0)
				{
					entier--;
					reste -= baseValeur;
				}
				break;

			default:
				break;
		}

		return (entier, reste);
	}
}
