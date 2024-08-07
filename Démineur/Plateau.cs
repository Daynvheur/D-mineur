﻿namespace Démineur;

public static class Plateau
{
	private static int x = 1;
	private static int y = 1;
	private static Case[] lPlateau = [];

	public static int X { get => x; private set => x = value; }
	public static int Y { get => y; private set => y = value; }
	public static int M { get; set; } = 1;
	public static Case[] LPlateau { get => lPlateau; private set => lPlateau = value; }

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Supprimer le paramètre inutilisé", Justification = "Oui.")]
	public static void InitialisePlateau(Action<object?, EventArgs> pbCase_Click, int largeur = 25, int hauteur = 25, int mines = 63, int seed = 1337) //50, 50, 250
	{
		int mining = 0;
		Random rand = new(/*seed*/);
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

	public static void Interaction1(Case @case)
	{
		if (@case.isHidden && !@case.isMarked)
		{
			RevealCase(@case);
			if (@case.isMined) //[WIP] Ajouter un fond rouge sur les cases marquées incorrectement, ainsi que sur la mine incorrectement dévoilées
			{
				LPlateau.Where(c => c.isHidden && c.isMined).ToList().ForEach(c => c.Reveal());
			}
		}
		else
		{
			//Récupération du nombre de cases alentours voilées et marquées
			var lookupHiddenMarked = @case.Voisines.Where(c => c.isHidden).ToLookup(c => c.isMarked);
			int voisinesVoilées = lookupHiddenMarked[false].Count();
			int voisinesMarquées = lookupHiddenMarked[true].Count();

			int minesVoisines = @case.HasMineVoisines;
			if (voisinesVoilées != 0 && minesVoisines > 0 && voisinesVoilées + voisinesMarquées == minesVoisines)//Si le nombre de voilées (augmenté de celles déjà marquées) correspond aux voisines, marquer les voilées voisines
			{
				lookupHiddenMarked[false].ToList().ForEach(c => c.Marque());
			}
			else if (voisinesMarquées == minesVoisines)
			{
				if (lookupHiddenMarked[false].Any())
				{
					if (lookupHiddenMarked[false].Any(c => c.isMined)) //[WIP] Ajouter un fond rouge sur les cases marquées incorrectement, ainsi que sur la/les mine/s incorrectement dévoilées
					{
						LPlateau.Where(c => c.isHidden && c.isMined).ToList().ForEach(c => c.Reveal());
						//tsslReste.Text = "0"; //GameOver (raccourci)
					}
					else lookupHiddenMarked[false].ToList().ForEach(RevealCase);
				}
				else
				{
					if (@case.isMined && @case.isMarked)
					{
						@case.Démine();
						M -= 1;
					}
					else
					{
						var mines = @case.Voisines.Where(c => c.isMined).ToList();
						mines.ForEach(c => c.Démine());
						mines.ForEach(RevealCase); //[WIP]Il reste des cases voisines-de-voisines passant à zéro qui ne sont pas ouvertes en prolongement, ce qui donne des cases sans numéro collées à une case ne pouvant pas contenir de mine.
						mines.ForEach(c => c.Voisines.Where(cv => !cv.isHidden && cv.HasMineVoisines == 0).ToList().ForEach(RevealCase));
						M -= mines.Count;
					}
				}
			}
		}
		List<Case> plateauVoilées = LPlateau.Where(c => c.isHidden).ToList();
		//Si toutes les cases restantes sont minées, les marquer
		if (plateauVoilées.Count == LPlateau.Count(c => c.isMined)) plateauVoilées.ForEach(c => c.Marque());
	}

	public static void Interaction2(Case @case)
	{
		if (!@case.isHidden) return;

		if (@case.isMarked) @case.Demarque();
		else @case.Marque();
	}

	public static void RevealCase(Case @case)
	{
		@case.Reveal(); //Dévoiler celle-ci
		if (@case.HasMineVoisines != 0) return; //S'il y a des mines dans le voisinage, s'arrêter là

		@case.Voisines.Where(c => c.isHidden && !c.isMarked).ToList().ForEach(RevealCase);
	}
}