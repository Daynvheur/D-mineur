namespace Démineur;

public partial class FDémineur : Form
{
	private int Seconds = 0;

	public FDémineur()
	{
		InitializeComponent();

		FirstGame();
	}

	public void FirstGame()
	{
		Plateau.InitialisePlateau(PbCase_Click);
		Controls.AddRange(Plateau.LPlateau.Select(c => c.pictBox).ToArray());
		ResetGame();
	}

	public void ResetGame()
	{
		Plateau.LPlateau.ToList().ForEach(c => c.Restore());
		ClientSize = new Size(Plateau.X * Case.Size_x, Plateau.Y * Case.Size_y + ssMines.Height); //redimension de la zone visible

		tsslGameOver.Visible = false;
		tsslMines.Visible = true;
		tsslReste.Visible = true;
		tsslReste.Text = Plateau.M.ToString();
		tsslSep.Visible = true;
		tsslTotal.Visible = true;
		tsslTotal.Text = Plateau.M.ToString();
		tspbReste.Visible = true;
		tspbReste.Maximum = Plateau.M;
		tspbReste.Value = 0;
		Seconds = 0; //Initialisation moche :p
		tTemps.Start();
	}

	private void PbCase_Click(object? sender, EventArgs e)
	{
		//Récupération de l'id de la case par le champ Name de la PictureBox
		if (sender is null || !int.TryParse(((PixelBox)sender).Name, out int id)) return;

		Case @case = Plateau.LPlateau[id];

		if (e is not MouseEventArgs mouse) return; //Possiblement d'autres modes d'interaction ?

		switch (mouse.Button)
		{
			case MouseButtons.Right:
				if (@case.isHidden)
					if (@case.isMarked) @case.Demarque();
					else @case.Marque();

				break;
			case MouseButtons.Left:
				if (@case.isHidden && !@case.isMarked)
				{
					RevealCase(@case);
					if (@case.isMined) tsslReste.Text = "0"; //GameOver (raccourci)
				}
				else
				{
					//Récupération du nombre de cases alentours voilées et marquées
					int id_y = id % Plateau.X;

					var lookupHiddenMarked = @case.Voisines.Where(c => c.isHidden).ToLookup(c => c.isMarked);
					int voisinesVoilées = lookupHiddenMarked[false].Count();
					int voisinesMarquées = lookupHiddenMarked[true].Count();

					//Si le nombre de voilées (augmenté de celles déjà marquées) correspond aux voisines, marquer les voilées voisines
					if (voisinesVoilées + voisinesMarquées == @case.HasMineVoisines) lookupHiddenMarked[false].ToList().ForEach(c => c.Marque());
					//Sinon, si le nombre de marquées correspond déjà aux voisines, dévoiler les manquantes
					else if (voisinesMarquées == @case.HasMineVoisines) lookupHiddenMarked[false].ToList().ForEach(RevealCase);
				}
				List<Case> plateauVoilées = Plateau.LPlateau.Where(c => c.isHidden).ToList();
				//Si toutes les cases restantes sont minées, les marquer
				if (plateauVoilées.Count == Plateau.M) plateauVoilées.ForEach(c => c.Marque());

				break;
			default:
				throw new NotImplementedException(mouse.Button.ToString());
		}

		int plateauMarquées = Plateau.LPlateau.Where(c => c.isMarked).Count();
		if (plateauMarquées == Plateau.M) Plateau.LPlateau.Where(c => c.isHidden && !c.isMarked).ToList().ForEach(c => c.Reveal());
		tspbReste.Value = plateauMarquées;
		tsslReste.Text = (Plateau.M - plateauMarquées).ToString();
	}

	public static void RevealCase(Case @case)
	{
		@case.Reveal(); //Dévoiler celle-ci
		if (@case.HasMineVoisines != 0) return; //S'il y a des mines dans le voisinage, s'arrêter là

		@case.Voisines.Where(c => c.isHidden && !c.isMarked).ToList().ForEach(RevealCase);
	}

	private void FDémineur_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == (char)Keys.Escape)
			Close(); //Sortie sur Échap
	}

	private void TsslReste_TextChanged(object sender, EventArgs e)
	{
		if (((ToolStripStatusLabel)sender).Text == "0")
		{
			tsslMines.Visible = false;
			tsslReste.Visible = false;
			tsslSep.Visible = false;
			tsslTotal.Visible = false;
			tspbReste.Visible = false;
			tsslGameOver.Visible = true;
			tTemps.Stop();
		}
	}

	private void TsslGameOver_Click(object sender, EventArgs e) => ResetGame();

	private void TTemps_Tick(object sender, EventArgs e) => tsslTemps.Text = new TimeSpan(0, 0, Seconds++).ToString();
}