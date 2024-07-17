namespace Démineur;

public static class Plateau
{
	private static int x = 1;
	private static int y = 1;
	private static int m = 1;
	private static Case[] lPlateau = [];

	public static int X { get => x; private set => x = value; }
	public static int Y { get => y; private set => y = value; }
	public static int M { get => m; private set => m = value; }
	public static Case[] LPlateau { get => lPlateau; private set => lPlateau = value; }

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Supprimer le paramètre inutilisé", Justification = "Oui.")]
	public static void InitialisePlateau(Action<object?, EventArgs> pbCase_Click, int largeur = 4, int hauteur = 2, int mines = 1, int seed = 1337) //50, 50, 250
	{
		int mining = 0;
		Random rand = new(seed);
		int iMax = largeur * hauteur;

		//Initialisation de la liste des cases du plateau
		LPlateau = new Case[iMax];
		for (int i = 0; i < iMax; i++)
		{
			int i_x = i % largeur;
			int i_y = i / largeur;
			LPlateau[i] = new(i_x, i_y, pbCase_Click)
			{
				isMined = (rand.Next(iMax - i) < mines - mining) && mining == mining++
			}; //Référencement de la case

			if (i >= largeur) //étage 1+
			{
				if (i_x > 0) LPlateau[i].Voisines.Add(LPlateau[i - 1 - largeur]); //haut gauche
				LPlateau[i].Voisines.Add(LPlateau[i - largeur]); //haut centre
				if (i_x < largeur - 1) LPlateau[i].Voisines.Add(LPlateau[i + 1 - largeur]); //droite
			}

			if (i_x > 0) LPlateau[i].Voisines.Add(LPlateau[i - 1]); //gauche

			LPlateau[i].Voisines.ForEach(c => c.Voisines.Add(LPlateau[i]));
			LPlateau[i].Save();
		}

		X = largeur;
		Y = hauteur;
		M = mines;
	}
}