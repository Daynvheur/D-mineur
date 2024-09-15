using Godot;
using Godot.Collections;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

public enum ETexture
{
	Minee = -5,
	Question = -4,
	Marquee = -3,
	Fermee = -2,
	None = -1,
	Zero,
	Un,
	Deux,
	Trois,
	Quatre,
	Cinq,
	Six,
	Sept,
	Huit,
}

public enum Textures
{
	Normal,
	Pressed,
	Hover,
	Disabled,
	Focused,
	ClickMask,
}


public partial class CDmineur : VBoxContainer
{
	[Export]
	public Node2D ControlMine { get; set; } = new();

	[Export]
	public bool isGameOver = false;

	[Export]
	public HBoxContainer HBoxContainer { get; set; } = new();

	[Export]
	public Label TsslReste { get; set; } = new();

	[Export]
	public Label TsslTotal { get; set; } = new();

	[Export]
	public ProgressBar ProgressBar { get; set; } = new();

	[Export]
	public Label TsslGameOver { get; set; } = new();

	[Export]
	public Label TsslTemps { get; set; } = new();

	[Export]
	public Timer Timer { get; set; } = new();

	[Export]
	public double elapsedTime = 0;

	[Export]
	public double elapsedTotalTime = 0;

	[Export]
	public Vector2I taillePlateau = Vector2I.One;

	[Export]
	public int mines = 0;

	[Export]
	public bool isSeeded = false;

	[Export]
	public int seed = 1337;

	[Export]
	public bool boucle = false;

	[Export]
	public float zoom = 1.0f;

	private Vector2 tailleInitialeGrille = Vector2.One;

	[Export]
	public Dictionary<ETexture, Dictionary<Textures, Resource?>> ImagesArray { get; set; } = new()
	{
		{ ETexture.Minee, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Question, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Marquee, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Fermee, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.None, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Zero, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Un, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Deux, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Trois, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Quatre, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Cinq, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Six, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Sept, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
		{ ETexture.Huit, new()
			{
				{ Textures.Normal, null },
				{ Textures.Pressed, null },
				{ Textures.Hover, null },
				{ Textures.Disabled, null },
				{ Textures.Focused, null },
				{ Textures.ClickMask, null },
			}
		},
	};

	[Export]
	public ETexture TestEtexture;

	public CDmineur() : base()
	{
		Plateau.MettreGameOver = (gameOver) =>
		{
			elapsedTime = 0;
			isGameOver = gameOver;
			TsslGameOver.SetVisible(isGameOver);
		};
		Plateau.RafraîchirMines = (int min, int marques, int max) =>
		{
			TsslReste.SetText((max - marques - min).ToString());
			TsslTotal.SetText(max.ToString());
			ProgressBar.SetMin(min);
			ProgressBar.SetValue(marques);
			ProgressBar.SetMax(max);
		};
		Plateau.AjouterCase = (Vector2I xy) =>
		{
			var bouton = new TextureButton
			{
				Position = xy,
				Size = Case.Taille.Moi,
				StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered,
			};
			SetTextures(bouton, ImagesArray[ETexture.Fermee]);
			ControlMine.AddChild(bouton);
			return bouton;
		};
		Plateau.MettreTexture = (Case @case) => SetTextures(@case.Image, ImagesArray[@case.estRatée
			? ETexture.None
			: @case.estFermée
				? @case.estMarquée
					? ETexture.Marquee
					: @case.estQuestionnée
						? ETexture.Question
						: ETexture.Fermee
				: @case.estMinée
					? ETexture.Minee
					: (ETexture)@case.NbMinesVoisines]);
		Plateau.CliquerCase = (@case) => (@event) => InteractionDispatcher(@event, @case);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//TsslGameOver.GuiInput += @event => { Console.WriteLine("bla."); Plateau.RestaurePlateau(); };
		HBoxContainer.GuiInput += @event => { if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) { Plateau.Restaure(); elapsedTime = 0; } };
		Window window = GetWindow();

		Plateau.InitialisePlateau(taillePlateau, mines, seed: isSeeded ? seed : null, boucle: boucle, gameOver: isGameOver);
		Vector2I caseSize = Case.Taille.Moi;
		Vector2I plateauSize = Plateau.Taille.Moi;
		tailleInitialeGrille = plateauSize * caseSize;
		ControlMine.Scale = Vector2.One * zoom;
		((Control)ControlMine.GetParent()).CustomMinimumSize = tailleInitialeGrille * zoom; // + (int)(HBoxContainer.Size.Y ?? 0));

		Timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;
		elapsedTotalTime += delta;

		TsslTemps.SetText(isGameOver ? "(Temps écoulé)" : FormatTime(elapsedTime));
	}

	private readonly Plateau Plateau = new();

	private static void SetTextures(TextureButton? bouton, Dictionary<Textures, Resource?> images)
	{
		if (bouton is null) return;
		foreach (var texture in images)
			switch (texture.Key)
			{
				case Textures.Normal:
					bouton.TextureNormal = (Texture2D?)texture.Value;
					break;

				case Textures.Pressed:
					bouton.TexturePressed = (Texture2D?)texture.Value;
					break;

				case Textures.Hover:
					bouton.TextureHover = (Texture2D?)texture.Value;
					break;

				case Textures.Disabled:
					bouton.TextureDisabled = (Texture2D?)texture.Value;
					break;

				case Textures.Focused:
					bouton.TextureFocused = (Texture2D?)texture.Value;
					break;

				case Textures.ClickMask:
					bouton.TextureClickMask = (Bitmap?)texture.Value;
					break;

				default:
					break;
			}
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

	private void InteractionDispatcher(InputEvent @event, Case @case)
	{
		switch (@event)
		{
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
			case InputEventMouseMotion mouseMotion:
				if ((mouseMotion.ButtonMask & MouseButtonMask.Middle) == MouseButtonMask.Middle)
				{
					ScrollContainer scrollContainer = ((ScrollContainer)ControlMine.GetParent().GetParent());
					var move = mouseMotion.ScreenRelative;// * zoom;
					scrollContainer.ScrollHorizontal -= (int)move.X;
					scrollContainer.ScrollVertical -= (int)move.Y;
				}
				break;

			case InputEventMouseButton mouseInput:
				Console.WriteLine($"1Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
				//if (mouseInput.ButtonIndex != MouseButton.Left)
				//{
				Console.WriteLine($"Je suis le bouton {mouseInput.ButtonIndex}, et mon statut ctrlPressed est {mouseInput.CtrlPressed}");
				//}
				//else
				if (mouseInput.ButtonIndex == MouseButton.WheelDown && mouseInput.CtrlPressed)
				{
					zoom -= 0.1f;
					//var currentScale = ControlMine.Scale;
					//var mouseCanvasPosition = ControlMine.GetGlobalMousePosition();
					//var mouseOffset = mouseCanvasPosition - ControlMine.GlobalPosition;
					//ControlMine.GlobalPosition = mouseCanvasPosition - mouseOffset * zoom;
					//Console.WriteLine($"mouseCanvasPosition:{mouseCanvasPosition};mouseOffset:{mouseOffset}");

					ControlMine.Scale = Vector2.One * zoom;
					((Control)ControlMine.GetParent()).CustomMinimumSize = tailleInitialeGrille * zoom;
					ScrollContainer scrollContainer = ((ScrollContainer)ControlMine.GetParent().GetParent());
					scrollContainer.ScrollHorizontal -= (int)ControlMine.Scale.X;
					scrollContainer.ScrollVertical -= (int)ControlMine.Scale.Y;

					mouseInput.Canceled = true;
				}
				else if (mouseInput.ButtonIndex == MouseButton.WheelUp && mouseInput.CtrlPressed)
				{
					zoom += 0.1f;
					//var currentScale = ControlMine.Scale;
					var mouseCanvasPosition = ControlMine.GetGlobalMousePosition();
					var mouseOffset = mouseCanvasPosition - ControlMine.GlobalPosition;
					//ControlMine.GlobalPosition = mouseCanvasPosition - mouseOffset * zoom;
					//Console.WriteLine($"mouseCanvasPosition:{mouseCanvasPosition};mouseOffset:{mouseOffset}");

					ControlMine.Scale = Vector2.One * zoom;
					((Control)ControlMine.GetParent()).CustomMinimumSize = tailleInitialeGrille * zoom;
					ScrollContainer scrollContainer = ((ScrollContainer)ControlMine.GetParent().GetParent());
					scrollContainer.ScrollHorizontal += (int)ControlMine.Scale.X;
					scrollContainer.ScrollVertical += (int)ControlMine.Scale.Y;

					mouseInput.Canceled = true;

					/*
			{
				Vector2 mousePos = GetGlobalMousePosition();
				Vector2 offset = mousePos - GlobalPosition;

				// Calculer le facteur de mise à l'échelle
				float scaleFactor = mouseEvent.ButtonIndex == (int)ButtonList.WheelUp ? 1.1f : 0.9f;

				// Ajuster la position pour simuler le pivot
				GlobalPosition = mousePos - offset * scaleFactor;

				// Modifier l'échelle
				Scale *= new Vector2(scaleFactor, scaleFactor);

				// Ajuster les barres de défilement pour conserver la position relative
				scrollContainer.ScrollHorizontal += (int)(offset.x * (scaleFactor - 1));
				scrollContainer.ScrollVertical += (int)(offset.y * (scaleFactor - 1));
			}*/
				}
				else  if (mouseInput.ButtonIndex == MouseButton.Left)
				{
					if (!mouseInput.Pressed)
					{
						if (@case.Image?.IsHovered() == true)
						{
							Console.Write($"2Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
							Plateau.Interaction1(@case);
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
						Plateau.Interaction2(@case);
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
}
