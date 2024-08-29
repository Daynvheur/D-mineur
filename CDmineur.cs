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
	public Control? ControlMine { get; set; }
	private ScaledControl? ScaledControlMine => _scaledControlMine ??= (ScaledControl?)ControlMine;
	private ScaledControl? _scaledControlMine;

	[Export]
	public bool isGameOver = false;

	[Export]
	public HBoxContainer? HBoxContainer { get; set; }

	[Export]
	public Label? TsslReste { get; set; }

	[Export]
	public Label? TsslTotal { get; set; }

	[Export]
	public ProgressBar? ProgressBar { get; set; }

	[Export]
	public Label? TsslGameOver { get; set; }

	[Export]
	public Label? TsslTemps { get; set; }

	[Export]
	public Timer? Timer { get; set; }

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
			TsslGameOver?.SetVisible(isGameOver);
		};
		Plateau.RafraîchirMines = (int min, int marques, int max) =>
		{
			TsslReste?.SetText((max - marques - min).ToString());
			TsslTotal?.SetText(max.ToString());
			ProgressBar?.SetMin(min);
			ProgressBar?.SetValue(marques);
			ProgressBar?.SetMax(max);
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
			ControlMine?.AddChild(bouton);
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
		//if (TsslGameOver != null) TsslGameOver.GuiInput += @event => { Console.WriteLine("bla."); Plateau.RestaurePlateau(); };
		if (HBoxContainer != null) HBoxContainer.GuiInput += @event => { if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) { Plateau.Restaure(); elapsedTime = 0; } };
		//DisplayServer.ScreenGetSize;//
		//DisplayServer.ScreenGetScale;//Linux+Mac seulement. :(
		//DisplayServer.WindowGetSize;//
		//DisplayServer.WindowSetSize;//
		//GetWindow().CurrentScreen.;//
		//GetWindow().;//
		Window window = GetWindow();
		//Case.Zoom.Moi = zoom;
		//Case.TailleBase.Moi = (Vector2I)(DisplayServer.ScreenGetSize(window.CurrentScreen) * new Vector2(12.0f / 1920, 12.0f / 1080));
		if (ScaledControlMine != null) ScaledControlMine.CustomScale *= zoom;

		//window.GuiSnapControlsToPixels = true;

		Plateau.InitialisePlateau(taillePlateau, mines, seed: isSeeded ? seed : null, boucle: boucle, gameOver: isGameOver);
		Vector2I caseSize = Case.Taille.Moi;
		Vector2I plateauSize = Plateau.Taille.Moi;
		if (ControlMine is not null) ControlMine.CustomMinimumSize = new(plateauSize.X * caseSize.X, (plateauSize.Y * caseSize.Y));// + (int)(HBoxContainer?.Size.Y ?? 0));
		//Panel p = new();
		//p.

		//window.Size = new(plateauSize.X * caseSize.X, (plateauSize.Y * caseSize.Y) + (int)(HBoxContainer?.Size.Y ?? 0));
		//window.MoveToCenter();
		Timer?.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;
		elapsedTotalTime += delta;

		TsslTemps?.SetText(isGameOver ? "(Temps écoulé)" : FormatTime(elapsedTime));
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

	private async void InteractionDispatcher(InputEvent @event, Case @case)
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
				Console.WriteLine($"1Je suis la case {@case.populationId} ! Et mon statut hover est : {@case.Image?.IsHovered()}");
				//if (mouseInput.ButtonIndex != MouseButton.Left)
				//{
				Console.WriteLine($"Je suis le bouton {mouseInput.ButtonIndex}, et mon statut ctrlPressed est {mouseInput.CtrlPressed}");
				//}
				//else
				if (mouseInput.ButtonIndex == MouseButton.WheelDown && mouseInput.CtrlPressed)
				{
					if (ScaledControlMine is not null)
					{
						ScaledControlMine.CustomScale *= (zoom *= 0.9f);
						ScaledControlMine.CustomMinimumSize *= zoom;
					}
					//Case.Zoom.Moi -= 0.1f;
					Console.WriteLine($"Les cases ont un zoom de {Case.Zoom.Moi}");
				}
				else if (mouseInput.ButtonIndex == MouseButton.WheelUp && mouseInput.CtrlPressed)
				{
					if (ScaledControlMine is not null)
					{
						ScaledControlMine.CustomScale *= (zoom *= 1.1f);
						ScaledControlMine.CustomMinimumSize *= zoom;
					}
					//Case.Zoom.Moi += 0.1f;
					Console.WriteLine($"Les cases ont un zoom de {Case.Zoom.Moi}");
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