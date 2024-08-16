using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1121:Assignments should not be made from within sub-expressions")]
public static class Plateau
{
	public static int Surface(this Vector2I vector) => vector.X * vector.Y;

	public static GetSetT<Vector2I> Taille { get; private set; } = new(new(1, 1));
	private static Case[] lPlateau = [];
	private static int minesMax = 0;
	private static int minesMarquees = 0;
	private static int minesMin = 0;

	public static Case[] LPlateau { get => lPlateau; private set => lPlateau = value; }
	public static Func<Vector2I, TextureButton>? AjouterCase { get; set; }
	public static Func<Case, Control.GuiInputEventHandler>? CliquerCase { get; set; }
	public static Action<Case>? MettreTexture { get; set; }
	public static Action<bool>? MettreGameOver { get; set; }
	public static Action<int, int, int>? RafraîchirMines { get; set; }

	public static int MinesMax
	{ get => minesMax; set { minesMax = value; RafraîchirMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	public static int MinesMarquees
	{ get => minesMarquees; set { minesMarquees = value; RafraîchirMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	public static int MinesMin
	{ get => minesMin; set { minesMin = value; RafraîchirMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Supprimer le paramètre inutilisé", Justification = "Oui.")]
	public static void InitialisePlateau(Vector2I size, int mines = 0, int? seed = 1337, bool boucle = false, bool gameOver = false) //50, 50, 250
	{
		int minées = 0;
		Random rand = seed is null ? new() : new(seed.Value);
		int iMax = size.Surface();

		//Initialisation de la liste des cases du plateau
		LPlateau = new Case[iMax];
		int xMax = size.X;
		int yMax = iMax - xMax;
		for (int i = 0; i < iMax; i++)
		{
			int i_x = i % xMax;
			int i_y = i / xMax;
			LPlateau[i] = new(new(i_x, i_y), (rand.Next(iMax - i) < mines - minées) && minées == minées++); //Référencement de la case
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

		Taille.Moi = size;
		MinesMax = mines;
		MettreGameOver?.Invoke(gameOver);
	}

	public static void RestaurePlateau()
	{
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

	public static void InteractionDispatcher(InputEvent @event, Case @case)
	{
		switch (@event)
		{
			//case InputEventMouseMotion:
			//case InputEventMagnifyGesture:
			//case InputEventPanGesture:
			//case InputEventScreenDrag:
			//case InputEventScreenTouch:
			//case InputEventJoypadButton:
			//case InputEventJoypadMotion:
			//case InputEventMidi:
			//case InputEventShortcut:
			//case InputEventAction:
			//case InputEventMouseMotion mouseMove:
			//	if (@case.Image?.GetRect().HasPoint(mouseMove.Position) == true)
			//	{
			//		Console.WriteLine($"I'm in {@case.populationId}.");
			//	}
			//	else
			//	{
			//		Console.WriteLine($"I'm out {@case.populationId}.");
			//	}
			//	break;

			case InputEventMouseButton mouseInput:
				Console.WriteLine($"Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
				//if (mouseInput.ButtonIndex != MouseButton.Left)
				//{
				//	Console.WriteLine($"Je suis le bouton {mouseInput.ButtonIndex}");
				//}
				//else
				if (mouseInput.ButtonIndex == MouseButton.Left)
				{
					if (!mouseInput.Pressed)
					{
						if (@case.Image?.IsHovered() == true)
						{
							Console.Write($"Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
							Interaction1(@case);
						}
						//else
						//	Console.Write($"Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
					}
					//else
					//	Console.WriteLine("Je suis pressé !");
				}
				else if (mouseInput.ButtonIndex == MouseButton.Right)
				{
					if (mouseInput.Pressed)
						Interaction2(@case);
					//else
					//	Console.WriteLine($"Je suis un clic droit. Appuyé : {mouseInput.Pressed}.");
				}
				break;
				//case InputEventKey keyEvent:
				//	Console.WriteLine($"{{{nameof(keyEvent.GetKeyLabelWithModifiers)}:{keyEvent.GetKeyLabelWithModifiers()},{nameof(keyEvent.GetKeycodeWithModifiers)}:{keyEvent.GetKeycodeWithModifiers()},{nameof(keyEvent.Pressed)}:{keyEvent.Pressed}}}");
				//	Console.Write($"Je suis la case {@case.populationId} ! ");
				//	break;

				//default:
				//	Console.WriteLine($"Je suis un événement {@event.GetType()}.");
				//	break;
		}
	}

	public static void Interaction1(Case @case)
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

	private static void FinTragique()
	{
		LPlateau.Where(c => c.estFermée && c.estMinée).ToList().ForEach(c => c.Ouvre());
		LPlateau.Where(c => c.estMarquée && !c.estMinée).ToList().ForEach(c => c.Ouvre(true));

		MettreGameOver?.Invoke(true);
	}

	public static void Interaction2(Case @case)
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