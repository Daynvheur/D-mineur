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
			isGameOver = gameOver;
			TsslGameOver?.SetVisible(gameOver);
		};
		Plateau.UpdateMines = (int min, int marques, int max) =>
		{
			TsslReste?.SetText((max - marques - min).ToString());
			TsslTotal?.SetText(max.ToString());
			ProgressBar?.SetMin(min);
			ProgressBar?.SetValue(marques);
			ProgressBar?.SetMax(max);
		};
		Plateau.AddCase = (int x, int y) =>
		{
			var bouton = new TextureButton
			{
				Position = new(x, y),
				Size = new(Case.Size_x, Case.Size_y),
				StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered,
			};
			SetTextures(bouton, ImagesArray[ETexture.Fermee]);
			AddChild(bouton);
			return bouton;
		};
		Plateau.SetTexture = (Case @case) => SetTextures(@case.Image, ImagesArray[@case.isHidden
			? @case.isMarked
				? ETexture.Marquee
				// : @case.isQuestion
				// ? ETexture.Question
				: ETexture.Fermee
			: @case.isMined
				? ETexture.Minee
				: (ETexture)@case.HasMineVoisines]);
		Plateau.CaseClick = (@case) => (@event) => Plateau.InteractionDispatcher(@event, @case);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (TsslGameOver is not null)
		{
			TsslGameOver.SetVisible(isGameOver);
			TsslGameOver.GuiInput += _ => Plateau.RestaurePlateau();
		}
		TsslTemps?.SetText(FormatTime(elapsedTime));
		Plateau.InitialisePlateau();

		GetWindow().Size = new(Plateau.X * Case.Size_x, (Plateau.Y * Case.Size_y) + (int)(HBoxContainer?.Size.Y ?? 0));
		Timer?.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		elapsedTime += delta;

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