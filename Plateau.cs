using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1121:Assignments should not be made from within sub-expressions")]
public static class Plateau
{
	public static int Surface(this Vector2I vector) => vector.X * vector.Y;

	public static GetSetT<Vector2I> Size { get; private set; } = new(new(1, 1));
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

		Size.Me = size;
		MinesMax = mines;
		MettreGameOver?.Invoke(gameOver);
	}

	public static void RestaurePlateau()
	{
		int mining = 0;
		Random rand = new(/*seed*/);
		int iMax = Size.Me.X * Size.Me.Y;

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
		if (@case.estFermée && !@case.estMarquée)
		{
			RevealCase(@case);
			if (@case.estMinée) //[WIP] Ajouter un fond rouge sur les cases marquées incorrectement, ainsi que sur la mine incorrectement dévoilées
			{
				LPlateau.Where(c => c.estFermée && c.estMinée).ToList().ForEach(c => c.Révèle());
				MettreGameOver?.Invoke(true);
			}
		}
		else
		{
			//Récupération du nombre de cases alentours voilées et marquées
			var lookupHiddenMarked = @case.Voisines.Where(c => c.estFermée).ToLookup(c => c.estMarquée);
			int voisinesVoilées = lookupHiddenMarked[false].Count();
			int voisinesMarquées = lookupHiddenMarked[true].Count();

			int minesVoisines = @case.AMinesVoisines;
			if (voisinesVoilées != 0 && minesVoisines > 0 && voisinesVoilées + voisinesMarquées == minesVoisines)//Si le nombre de voilées (augmenté de celles déjà marquées) correspond aux voisines, marquer les voilées voisines
			{
				lookupHiddenMarked[false].ToList().ForEach(c => c.Marque());
			}
			else if (voisinesMarquées == minesVoisines)
			{
				if (lookupHiddenMarked[false].Any())
				{
					if (lookupHiddenMarked[false].Any(c => c.estMinée)) //[WIP] Ajouter un fond rouge sur les cases marquées incorrectement, ainsi que sur la/les mine/s incorrectement dévoilées
					{
						LPlateau.Where(c => c.estFermée && c.estMinée).ToList().ForEach(c => c.Révèle());
						//tsslReste.Text = "0"; //GameOver (raccourci)
					}
					else lookupHiddenMarked[false].ToList().ForEach(RevealCase);
				}
				else
				{
					if (@case.estMinée && @case.estMarquée)
					{
						@case.Démine();
						MinesMax -= 1;
					}
					else
					{
						var mines = @case.Voisines.Where(c => c.estMinée).ToList();
						mines.ForEach(c => c.Démine());
						mines.ForEach(RevealCase); //[WIP]Il reste des cases voisines-de-voisines passant à zéro qui ne sont pas ouvertes en prolongement, ce qui donne des cases sans numéro collées à une case ne pouvant pas contenir de mine.
						mines.ForEach(c => c.Voisines.Where(cv => !cv.estFermée && cv.AMinesVoisines == 0).ToList().ForEach(RevealCase));
						MinesMax -= mines.Count;
					}
				}
			}
		}
		List<Case> plateauVoilées = LPlateau.Where(c => c.estFermée).ToList();
		//Si toutes les cases restantes sont minées, les marquer
		if (plateauVoilées.Count == LPlateau.Count(c => c.estMinée))
		{
			plateauVoilées.ForEach(c => c.Marque());
			MettreGameOver?.Invoke(true);
		}
	}

	public static void Interaction2(Case @case)
	{
		if (!@case.estFermée) return;

		if (@case.estMarquée)
			@case.Questionne();
		else if (@case.estQuestionnée)
			@case.Démarque();
		else @case.Marque();
	}

	public static void RevealCase(Case @case)
	{
		@case.Révèle(); //Dévoiler celle-ci
		if (@case.AMinesVoisines != 0) return; //S'il y a des mines dans le voisinage, s'arrêter là

		@case.Voisines.Where(c => c.estFermée && !c.estMarquée).ToList().ForEach(RevealCase);
	}
}
