using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Vector2IHelper
{
	public static int Surface(this Vector2I vector) => vector.X * vector.Y;
}

public class Plateau
{

	public GetSetT<Vector2I> Taille { get; private set; } = new(new(1, 1));
	private Case[] lPlateau = [];
	private int minesMax = 0;
	private int minesMarquees = 0;
	private int minesMin = 0;

	public Case[] LPlateau { get => lPlateau; private set => lPlateau = value; }
	public Func<Vector2I, TextureButton>? AjouterCase { get; set; }
	public Func<Case, Control.GuiInputEventHandler>? CliquerCase { get; set; }
	public Action<Case>? MettreTexture { get; set; }
	public Action<bool>? MettreGameOver { get; set; }
	public Action<int, int, int>? RafraîchirMines { get; set; }

	public int MinesMax
	{ get => minesMax; set { minesMax = value; RafraîchirMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	public int MinesMarquees
	{ get => minesMarquees; set { minesMarquees = value; RafraîchirMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	public int MinesMin
	{ get => minesMin; set { minesMin = value; RafraîchirMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	Plateau? sauve;

	public void InitialisePlateau(Vector2I taillePlateau, int mines = 0, int? seed = 1337, bool boucle = false, bool gameOver = false) //50, 50, 250
	{
		int minées = 0;
		Random rand = seed is null ? new() : new(seed.Value);
		int iMax = taillePlateau.Surface();

		//Initialisation de la liste des cases du plateau
		LPlateau = new Case[iMax];
		int xMax = taillePlateau.X;
		int yMax = iMax - xMax;
		for (int i = 0; i < iMax; i++)
		{
			int i_x = i % xMax;
			int i_y = i / xMax;
			LPlateau[i] = new(this, new(i_x, i_y), (rand.Next(iMax - i) < mines - minées) && minées == minées++); //Référencement de la case
			List<Case> voisines = [];
			if (i >= xMax) //étage 1+
			{
				if (i_x > 0) voisines.Add(LPlateau[i - 1 - xMax]); //haut gauche
				else if (boucle && i_x == 0) voisines.Add(LPlateau[i - 1 - xMax + xMax]); //(boucle haut droite) [OK]
				voisines.Add(LPlateau[i - xMax]); //haut centre
				if (i_x + 1 < xMax) voisines.Add(LPlateau[i + 1 - xMax]); //haut droite
				else if (boucle && i_x + 1 == xMax) voisines.Add(LPlateau[i + 1 - xMax - xMax]); //(boucle haut gauche) [OK]
			}

			if (i_x > 0) voisines.Add(LPlateau[i - 1]); //gauche
			if (boucle && i_x + 1 == xMax && i > 0) voisines.Add((LPlateau[i + 1 - xMax])); //boucle gauche

			if (boucle && i >= yMax && i > 0) //dernier étage
			{
				if (i_x > 0) voisines.Add(LPlateau[i - 1 - yMax]); //boucle bas gauche
				else if (i_x == 0) voisines.Add(LPlateau[i - 1 - yMax + xMax]); //boucle haut droite
				voisines.Add(LPlateau[i - yMax]); //boucle bas
				if (i_x + 1 < xMax) voisines.Add(LPlateau[i + 1 - yMax]); //boucle bas droite
				else if (i_x + 1 == xMax) voisines.Add(LPlateau[i + 1 - yMax - xMax]); //boucle haut gauche
			}
			voisines.ForEach(c => c.Voisines.Add(LPlateau[i]));
			LPlateau[i].Voisines.AddRange(voisines);
			LPlateau[i].Sauve();
		}

		Taille.Moi = taillePlateau;
		MinesMax = mines;
		MettreGameOver?.Invoke(gameOver);

		Sauve();
	}

	public void Restaure() => Restaure(sauve);

	private void Restaure(Plateau? cible)
	{
		if (cible is null) return;
		MinesMax = cible.minesMax;
		MinesMin = cible.minesMin;
		MinesMarquees = cible.minesMarquees;

		int mining = 0;
		Random rand = new(/*seed*/);
		int iMax = Taille.Moi.X * Taille.Moi.Y;

		for (int i = 0; i < iMax; i++)
		{
			LPlateau[i].Restaure();
			LPlateau[i].estMinée = (rand.Next(iMax - i) < MinesMax - mining) && mining == mining++;
			LPlateau[i].Sauve();
		}
		MettreGameOver?.Invoke(false);
	}

	public void Sauve()
	{
		sauve = new()
		{
			minesMax = MinesMax,
			minesMin = MinesMin,
			minesMarquees = MinesMarquees
		};
	}

	public void Interaction1(Case @case)
	{
		if (!@case.estFermée) //Si la case est OUVERTE
		{
			if (!@case.AMinesVoisines) //Si PAS de mines voisines, marquer le plateau
			{
				var ouvertesIncomplètes = LPlateau.Where(c => !c.estFermée && c.AMinesVoisines && c.Voisines.Count(_c => _c.estFermée) == c.NbMinesVoisines).ToLookup(c => c.Voisines.Any(_c => _c.estFermée && !_c.estMarquée)); //Toutes les cases ouvertes, incomplètes ou complètes
				if (ouvertesIncomplètes[true].Any())
					ouvertesIncomplètes[true].ToList().ForEach(c => c.Voisines.Where(_c => _c.estFermée && !_c.estMarquée).ToList().ForEach(_c => _c.Marque()));
				else
				{
					ouvertesIncomplètes[false].ToList().ForEach(c => c.Voisines.Where(_c => _c.estFermée && _c.estMarquée).ToList().ForEach(_c => _c.Démine()));
				}
			}
			else //S'il Y A des mines voisines
			{
				var voisinesFermées = @case.Voisines.Where(c => c.estFermée); //Récupération du nombre de fermées
				var lookupVoisinesFerméesMarquées = voisinesFermées.ToLookup(c => c.estMarquée);
				var voisinesFerméesNonMarquées = lookupVoisinesFerméesMarquées[false]; //et non marquées
				var voisinesFerméesMarquées = lookupVoisinesFerméesMarquées[true];
				int minesVoisines = @case.NbMinesVoisines;

				if (voisinesFermées.Count() == minesVoisines) //Si le nombre de fermées correspond aux voisines, les marquer ou déminer
				{
					if (voisinesFerméesNonMarquées.Any()) //S'il y a des non-marquées, les marquer;
						voisinesFerméesNonMarquées.ToList().ForEach(c => c.Marque());
					else //sinon, déminer les voisines
						voisinesFerméesMarquées.ToList().ForEach(c => c.Démine());
				}
				else if (voisinesFerméesMarquées.Count() == minesVoisines) //Sinon, si les marquages sont satisfaits
				{
					if (voisinesFerméesNonMarquées.Any(c => c.estMinée)) //mais qu'au-moins un est faux, fin de partie
					{
						voisinesFerméesNonMarquées.Where(c => c.estMinée).ToList().ForEach(c => c.Ouvre(true));
						FinTragique();
					}
					else //sinon, ouverture des cases supplémentaires
						voisinesFerméesNonMarquées.ToList().ForEach(c => c.Ouvre());
				}
			}
		}
		else if (@case.estMarquée) //Si la case est MARQUÉE
		{
			if (!@case.estMinée)
				FinTragique();
			else
				@case.Démine();
		}
		else //Si la case n'est NI ouverte, NI marquée
		{
			if (@case.estMinée)
			{
				@case.Ouvre(true);
				FinTragique();
			}
			else
				@case.Ouvre();
		}

		List<Case> plateauFermées = LPlateau.Where(c => c.estFermée).ToList();
		//Si toutes les cases restantes sont forcément minées, les marquer
		if (plateauFermées.Count == MinesMax)
			plateauFermées.ForEach(c => c.Marque());
		//Si toutes les cases marquées font le compte de mines, finaliser la partie
		if (MinesMarquees == MinesMax)
		{
			LPlateau.Where(c => c.estMarquée && !c.estMinée).ToList().ForEach(c => c.Ouvre(true));

			MettreGameOver?.Invoke(true);
		}
	}

	private void FinTragique()
	{
		LPlateau.Where(c => c.estFermée && c.estMinée).ToList().ForEach(c => c.Ouvre());
		LPlateau.Where(c => c.estMarquée && !c.estMinée).ToList().ForEach(c => c.Ouvre(true));

		MettreGameOver?.Invoke(true);
	}

	public void Interaction2(Case @case)
	{
		if (!@case.estFermée) //Si la case est OUVERTE
		{
			if (!@case.AMinesVoisines) //Si PAS de mines voisines, marquer le plateau
			{
				var ouvertesIncomplètes = LPlateau.Where(c => !c.estFermée && c.AMinesVoisines && c.Voisines.Count(_c => _c.estFermée) == c.NbMinesVoisines).ToLookup(c => c.Voisines.Any(_c => _c.estFermée && !_c.estMarquée)); //Toutes les cases ouvertes, incomplètes ou complètes
				if (ouvertesIncomplètes[true].Any())
					ouvertesIncomplètes[true].ToList().ForEach(c => c.Voisines.Where(_c => _c.estFermée && !_c.estMarquée).ToList().ForEach(_c => _c.Marque()));
				else
				{
					ouvertesIncomplètes[false].ToList().ForEach(c => c.Voisines.Where(_c => _c.estFermée && _c.estMarquée).ToList().ForEach(_c => _c.Démine()));

					List<Case> plateauVoilées = LPlateau.Where(c => c.estFermée).ToList();
					//Si toutes les cases restantes sont minées, les marquer
					if (plateauVoilées.Count == MinesMax)
					{
						plateauVoilées.ForEach(c => c.Marque());
						MettreGameOver?.Invoke(true);
					}
				}
			}
		}

		if (@case.estMarquée)
			@case.Questionne();
		else if (@case.estQuestionnée)
			@case.Démarque();
		else
		{
			@case.Marque();
			//Si toutes les cases marquées sont minées, finaliser la partie
			if (MinesMarquees == MinesMax)
			{
				LPlateau.Where(c => c.estMarquée && !c.estMinée).ToList().ForEach(c => c.Ouvre(true));

				MettreGameOver?.Invoke(true);
			}
		}
	}
}