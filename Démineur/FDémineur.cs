#nullable enable
namespace Démineur;

public partial class FDémineur
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
		Plateau.M = Plateau.LPlateau.Count(c => c.isMined);
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
				Plateau.Interaction2(@case);

				break;
			case MouseButtons.Left:
				Plateau.Interaction1(@case);

				break;
			default:
				throw new NotImplementedException(mouse.Button.ToString());
		}

		int plateauMarquées = Plateau.LPlateau.Count(c => c.isMarked);
		if (plateauMarquées == Plateau.LPlateau.Count(c => c.isMined)) Plateau.LPlateau.Where(c => c.isHidden && !c.isMarked).ToList().ForEach(c => c.Reveal());
		tspbReste.Value = plateauMarquées;
		tsslReste.Text = (Plateau.LPlateau.Count(c => c.isMined && c.isHidden) - plateauMarquées).ToString();
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
