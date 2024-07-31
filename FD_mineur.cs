using Godot;
using System;

public partial class FD_mineur : Control
{
	private class Re
	{
		protected Re()
		{ }

		public static Func<NodePath, Node> MaybeGetNode { protected get; set; } = _ => throw new NotImplementedException(nameof(MaybeGetNode));
	}

	private sealed class ReNode<TNode> : Re where TNode : Node
	{
		public ReNode(string path) : base()
		{ _path = path; }

		private readonly string _path;
		private TNode? _me;
		public TNode Me => _me ??= (TNode)MaybeGetNode(_path);
	}

	[Export]
	public bool isGameOver = false;

	[Export]
	public double elapsedTime = 0;

	private readonly ReNode<Timer> timer = new("timer");
	private readonly ReNode<Label> tsslGameOver = new("VBoxContainer/HBoxContainer/tsslGameOver");
	private readonly ReNode<Label> tsslTemps = new("VBoxContainer/HBoxContainer/tsslTemps");

	//private Label? _tsslReste; private Label? oldtsslReste => _tsslReste ??= (Label)GetNodeOrNull("VBoxContainer/HBoxContainer/tsslReste")
	private readonly ReNode<Label> tsslReste = new("/root/FD_mineur/VBoxContainer/HBoxContainer/tsslReste");

	private readonly ReNode<Label> tsslTotal = new("/root/FD_mineur/VBoxContainer/HBoxContainer/tsslTotal");
	private readonly ReNode<ProgressBar> ProgressBar = new("/root/FD_mineur/VBoxContainer/HBoxContainer/ProgressBar");

	public FD_mineur() : base()
	{
		Re.MaybeGetNode = GetNode;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tsslGameOver.Me.SetVisible(isGameOver);
		tsslTemps.Me.SetText(FormatTime(elapsedTime));

		Plateau.UpdateMines = (int min, int marques, int max) =>
		{
			tsslReste.Me.SetText((max - marques - min).ToString());
			tsslTotal.Me.SetText(max.ToString());
			ProgressBar.Me.SetMin(min);
			ProgressBar.Me.SetValue(marques);
			ProgressBar.Me.SetMax(max);
		};
		Plateau.InitialisePlateau();
		timer.Me.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Mettre à jour le temps écoulé
		elapsedTime += delta;

		tsslTemps.Me.SetText(isGameOver ? "(Temps écoulé)" : FormatTime(elapsedTime));
	}

	public void SetGameOver(bool gameOver = true)
	{
		isGameOver = gameOver;
		tsslGameOver.Me.SetVisible(gameOver);
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
