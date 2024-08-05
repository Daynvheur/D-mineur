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

public class GetSetT<T>(T _value, Action? _action = null)
{
	public T Me { get => _value; set { _value = value; _action?.Invoke(); } }
}

public class Case
{
	private static int population = 0; //Déclaration d'existence dans la population
	public int populationId;
	public static GetSetT<float> Zoom { get; set; } = new (1, static () => Size!.Me = (Vector2I)((Vector2)BaseSize!.Me * Zoom!.Me));
	public static GetSetT<Vector2I> BaseSize { get; set; } = new(new(12, 12), static () => Size!.Me = (Vector2I)((Vector2)BaseSize!.Me * Zoom!.Me));
	public static GetSetT<Vector2I> Size { get; set; } = new(BaseSize.Me);

	public bool isHidden; //La case n'est pas dévoilée
	public bool isMined; //La case n'est pas minée (elle peut le devenir)
	public bool isMarked; //La case n'est pas marquée comme minée
	public int HasMineVoisines => Voisines.Count(c => c.isMined);
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
	private Case? save;

	private Case()
	{ }
	public Case(Vector2I xy, bool _isMined = false, bool _isHidden = true, bool _isMarked = false)
	{
		populationId = population++;
		isHidden = _isHidden;
		isMarked = _isMarked;
		isMined = _isMined;
		Image = Plateau.AddCase?.Invoke(Size.Me * xy);
		if (Image is not null && Plateau.CaseClick is not null)
			Image.GuiInput += Plateau.CaseClick(this);
	}

	public void Reveal()
	{
		isHidden = false;
		Refresh();
	}

	public void Marque()
	{
		isMarked = true;
		Refresh();
	}

	public void Demarque()
	{
		isMarked = false;
		Refresh();
	}

	public void Restore()
	{
		if (save is null) return;
		isHidden = save.isHidden;
		isMined = save.isMined;
		isMarked = save.isMarked;
		Image = save.Image;
		Refresh();
	}

	public void Save()
	{
		save = new()
		{
			isHidden = isHidden,
			isMined = isMined,
			isMarked = isMarked,
			Image = Image
		};
	}

	public void Refresh()
	{
		Plateau.SetTexture?.Invoke(this);
	}

	internal void Démine()
	{
		isMined = false;
		isMarked = false;
		isHidden = false;
		Refresh();
		Voisines.ForEach(c => c.Refresh());
	}
}
