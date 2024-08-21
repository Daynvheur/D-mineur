using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

//using System.Drawing.Drawing2D;

//public class PixelBox : PictureBox
//{
//    protected override void OnPaint(PaintEventArgs pe)
//    {
//        pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
//        pe.Graphics.PixelOffsetMode = PixelOffsetMode.None;
//        base.OnPaint(pe);
//    }
//}

public class GetSetT<T>(T _valeur, Action? _action = null)
{
	public T Moi
	{ get => _valeur; set { _valeur = value; _action?.Invoke(); } }
}

public class Case
{
	private static int population = 0; //Déclaration d'existence dans la population
	public int populationId;

	private static readonly Action? MettreTaille = static () => Taille!.Moi = (Vector2I)((Vector2)TailleBase!.Moi * Zoom!.Moi);
	public static GetSetT<float> Zoom { get; set; } = new(1, MettreTaille);
	public static GetSetT<Vector2I> TailleBase { get; set; } = new(new(12, 12), MettreTaille);
	public static GetSetT<Vector2I> Taille { get; set; } = new(TailleBase.Moi);

	public bool estFermée; //La case n'est pas dévoilée
	public bool estMinée; //La case n'est pas minée (elle peut le devenir)
	public bool estMarquée; //La case est marquée comme minée
	public bool estQuestionnée; //La case est décorée, mais sans que cela n'entre en compte
	public bool estRatée; //La case obtient un visuel spécifique
	public int NbMinesVoisines => Voisines.Count(c => c.estMinée);
	public bool AMinesVoisines => Voisines.Any(c => c.estMinée);
	public List<Case> Voisines { get; set; } = [];

	public TextureButton? Image { get; set; }

	//public Image Image { get => (Image)pictBox.Image.Clone(); set { pictBox.Image = value; } }
	//private const string imgDir = @"Images\";

	//private static readonly Image imgHidden = Image.FromFile(imgDir + "_.png");

	////Affichage dans une PictureBox
	//public PixelBox pictBox = new()
	//{
	//    Size = new Size(Size_x, Size_y),  //dimensions
	//    Name = population.ToString(), //nom
	//    Image = imgHidden, //apparence
	//    SizeMode = PictureBoxSizeMode.Zoom
	//};

	private readonly Plateau Plateau;

	private Case? sauve;

	private Case(Plateau plateau)
	{
		Plateau = plateau;
	}

	public Case(Plateau _plateau, Vector2I xy, bool _estMinée = false, bool _estFermée = true, bool _estMarquée = false, bool _estQuestionnée = false, bool _estRatée = false)
	{
		populationId = population++;
		estFermée = _estFermée;
		estMarquée = _estMarquée;
		estMinée = _estMinée;
		estQuestionnée = _estQuestionnée;
		estRatée = _estRatée;
		Plateau = _plateau;
		Image = Plateau.AjouterCase?.Invoke(Taille.Moi * xy);
		if (Image is not null && Plateau.CliquerCase is not null)
			Image.GuiInput += Plateau.CliquerCase(this);
		Sauve();
	}

	public void Ouvre(bool _estRatée = false)
	{
		if (estFermée && estMarquée) Plateau.MinesMarquees--;
		estFermée = false;
		estMarquée = false;
		estQuestionnée = false;
		estRatée = _estRatée;
		Rafraîchit();

		if (AMinesVoisines) return; //S'il y a des mines dans le voisinage, s'arrêter là
		Voisines.Where(c => c.estFermée && !c.estMarquée).ToList().ForEach(c => c.Ouvre());
	}

	public void Marque()
	{
		if (estFermée && !estMarquée) Plateau.MinesMarquees++;
		estMarquée = true;
		estQuestionnée = false;
		Rafraîchit();
	}

	public void Questionne()
	{
		if (estFermée && estMarquée) Plateau.MinesMarquees--;
		estMarquée = false;
		estQuestionnée = true;
		Rafraîchit();
	}

	public void Démarque()
	{
		if (estFermée && estMarquée) Plateau.MinesMarquees--;
		estMarquée = false;
		estQuestionnée = false;
		Rafraîchit();
	}

	public void Restaure() => Restaure(sauve);

	public void Restaure(Case? cible)
	{
		if (cible is null) return;
		populationId = cible.populationId;
		estFermée = cible.estFermée;
		estMinée = cible.estMinée;
		estMarquée = cible.estMarquée;
		estQuestionnée = cible.estQuestionnée;
		estRatée = cible.estRatée;
		Image = cible.Image;
		Rafraîchit();
	}

	public void Sauve()
	{
		sauve = new(Plateau)
		{
			populationId = populationId,
			estFermée = estFermée,
			estMinée = estMinée,
			estMarquée = estMarquée,
			estQuestionnée = estQuestionnée,
			estRatée = estRatée,
			Image = Image
		};
	}

	public void Rafraîchit()
	{
		Plateau.MettreTexture?.Invoke(this);
	}

	internal void Démine()
	{
		if (estFermée && estMinée) Plateau.MinesMax--;
		estMinée = false;
		Ouvre();
		Voisines.Where(c => !c.estFermée).ToList().ForEach(c => { if (!c.AMinesVoisines) c.Ouvre(); else c.Rafraîchit(); });
	}
}
