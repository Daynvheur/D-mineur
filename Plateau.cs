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

	//public static int X { get => x; private set => x = value; }
	//public static int Y { get => y; private set => y = value; }
	public static Func<Vector2I, TextureButton>? AddCase { get; set; }

	public static Func<Case, Control.GuiInputEventHandler>? CaseClick { get; set; }
	public static Action<Case>? SetTexture { get; set; }
	public static Action<bool>? SetGameOver { get; set; }
	public static Action<int, int, int>? UpdateMines { get; set; }
	public static int MinesMax
	{ get => minesMax; set { minesMax = value; UpdateMines?.Invoke(minesMin, minesMarquees, minesMax); } }
	public static int MinesMarquees
	{ get => minesMarquees; set { minesMarquees = value; UpdateMines?.Invoke(minesMin, minesMarquees, minesMax); } }
	public static int MinesMin
	{ get => minesMin; set { minesMin = value; UpdateMines?.Invoke(minesMin, minesMarquees, minesMax); } }

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Supprimer le paramètre inutilisé", Justification = "Oui.")]
	public static void InitialisePlateau(Vector2I size, int mines = 0, int? seed = 1337, bool gameOver = false) //50, 50, 250
	{
		int mining = 0;
		Random rand = seed is null ? new() : new(seed.Value);
		int iMax = size.Surface();

		//Initialisation de la liste des cases du plateau
		LPlateau = new Case[iMax];
		for (int i = 0; i < iMax; i++)
		{
			int i_x = i % size.X;
			int i_y = i / size.X;
			LPlateau[i] = new(new(i_x, i_y), (rand.Next(iMax - i) < mines - mining) && mining == mining++); //Référencement de la case

			if (i >= size.X) //étage 1+
			{
				if (i_x > 0) LPlateau[i].Voisines.Add(LPlateau[i - 1 - size.X]); //haut gauche
				LPlateau[i].Voisines.Add(LPlateau[i - size.X]); //haut centre
				if (i_x < size.X - 1) LPlateau[i].Voisines.Add(LPlateau[i + 1 - size.X]); //droite
			}

			if (i_x > 0) LPlateau[i].Voisines.Add(LPlateau[i - 1]); //gauche

			LPlateau[i].Voisines.ForEach(c => c.Voisines.Add(LPlateau[i]));
			LPlateau[i].Save();
		}

		Size.Me = size;
		MinesMax = mines;
		SetGameOver?.Invoke(gameOver);
	}

	public static void RestaurePlateau()
	{
		Console.Write($"Je suis la fonction {nameof(RestaurePlateau)}.");
		int mining = 0;
		Random rand = new(/*seed*/);
		int iMax = Size.Me.X * Size.Me.Y;

		for (int i = 0; i < iMax; i++)
		{
			Console.Write($"{LPlateau[i] is null}.");
			LPlateau[i].Restore();
			LPlateau[i].isMined = (rand.Next(iMax - i) < MinesMax - mining) && mining == mining++;
			LPlateau[i].Save();
		}
		SetGameOver?.Invoke(false);
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
			case InputEventMouseMotion mouseMove:
				if ((@case.Image?.IsHovered()) == true)
				{
					Console.WriteLine("I'm in.");
				}
				else
				{
					Console.WriteLine("I'm out.");
				}
				break;

			case InputEventMouseButton mouseInput:
				Console.Write($"Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
				if (mouseInput.ButtonIndex == MouseButton.Right)
				{
					Console.WriteLine($"Je suis un clic droit. Appuyé : {mouseInput.Pressed}.");
				}
				else if (mouseInput.ButtonIndex != MouseButton.Left)
				{
					Console.WriteLine($"Je suis le bouton {mouseInput.ButtonIndex}");
				}
				else
				{
					if (mouseInput.Pressed)
						Console.WriteLine("Je suis pressé !");
					else
					{
						if ((@case.Image?.IsHovered()) != true)
							Console.Write($"Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
						else
						{
							Console.Write($"Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
				Interaction1(@case);
						}
					}
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
		if (@case.isHidden && !@case.isMarked)
		{
			RevealCase(@case);
			if (@case.isMined) //[WIP] Ajouter un fond rouge sur les cases marquées incorrectement, ainsi que sur la mine incorrectement dévoilées
			{
				LPlateau.Where(c => c.isHidden && c.isMined).ToList().ForEach(c => c.Reveal());
				SetGameOver?.Invoke(true);
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
						MinesMax -= 1;
					}
					else
					{
						var mines = @case.Voisines.Where(c => c.isMined).ToList();
						mines.ForEach(c => c.Démine());
						mines.ForEach(RevealCase); //[WIP]Il reste des cases voisines-de-voisines passant à zéro qui ne sont pas ouvertes en prolongement, ce qui donne des cases sans numéro collées à une case ne pouvant pas contenir de mine.
						mines.ForEach(c => c.Voisines.Where(cv => !cv.isHidden && cv.HasMineVoisines == 0).ToList().ForEach(RevealCase));
						MinesMax -= mines.Count;
					}
				}
			}
		}
		List<Case> plateauVoilées = LPlateau.Where(c => c.isHidden).ToList();
		//Si toutes les cases restantes sont minées, les marquer
		if (plateauVoilées.Count == LPlateau.Count(c => c.isMined))
		{
			plateauVoilées.ForEach(c => c.Marque());
			SetGameOver?.Invoke(true);
		}
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
