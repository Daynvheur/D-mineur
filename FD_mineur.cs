using Godot;
using Godot.Collections;
using System;
using System.Linq;

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

public partial class FD_mineur : Control
{
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

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "<En attente>")]
	public FD_mineur() : base()
	{
		Plateau.SetGameOver = (gameOver) =>
		{
			elapsedTime = 0;
			isGameOver = gameOver;
			TsslGameOver?.SetVisible(isGameOver);
		};
		Plateau.UpdateMines = (int min, int marques, int max) =>
		{
			TsslReste?.SetText((max - marques - min).ToString());
			TsslTotal?.SetText(max.ToString());
			ProgressBar?.SetMin(min);
			ProgressBar?.SetValue(marques);
			ProgressBar?.SetMax(max);
		};
		Plateau.AddCase = (Vector2I xy) =>
		{
			var bouton = new TextureButton
			{
				Position = xy,
				Size = Case.Size.Me,
				StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered,
			};
			SetTextures(bouton, ImagesArray[ETexture.Fermee]);
			AddChild(bouton);
			return bouton;
		};
		Plateau.SetTexture = (Case @case) => SetTextures(@case.Image, ImagesArray[@case.isHidden
			? @case.isMarked
				? ETexture.Marquee
				: @case.isQuestioned
					? ETexture.Question
					: ETexture.Fermee
			: @case.isMined
				? ETexture.Minee
				: (ETexture)@case.HasMineVoisines]);
		Plateau.CaseClick = (@case) => (@event) => Plateau.InteractionDispatcher(@event, @case);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//if (TsslGameOver != null) TsslGameOver.GuiInput += @event => { Console.WriteLine("bla."); Plateau.RestaurePlateau(); };
		if (HBoxContainer != null) HBoxContainer!.GuiInput += @event => { if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) { Plateau.RestaurePlateau(); elapsedTime = 0; } };
		//DisplayServer.ScreenGetSize;//
		//DisplayServer.ScreenGetScale;//Linux+Mac seulement. :(
		//DisplayServer.WindowGetSize;//
		//DisplayServer.WindowSetSize;//
		//GetWindow().CurrentScreen.;//
		//GetWindow().;//
		Case.Zoom.Me = 2.5f;
		Case.BaseSize.Me = (Vector2I)(DisplayServer.ScreenGetSize(GetWindow().CurrentScreen) * new Vector2(12.0f / 1920, 12.0f / 1080));

		GetWindow().GuiSnapControlsToPixels = true;

		Plateau.InitialisePlateau(taillePlateau, mines, isSeeded ? seed : null, gameOver: isGameOver);
		Vector2I caseSize = Case.Size.Me;
		Vector2I plateauSize = Plateau.Size.Me;
		GetWindow().Size = new(plateauSize.X * caseSize.X, (plateauSize.Y * caseSize.Y) + (int)(HBoxContainer?.Size.Y ?? 0));
		GetWindow().MoveToCenter();
		Timer?.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;
		elapsedTotalTime += delta;

		TsslTemps?.SetText(isGameOver ? "(Temps écoulé)" : FormatTime(elapsedTime));
	}

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
}
