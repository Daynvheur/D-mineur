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

public class Case
{
	public static int population = 0; //Déclaration d'existence dans la population
	public int populationId;
	public static int Size_x { get; set; } = 12 * 2;
	public static int Size_y { get; set; } = 12 * 2;

	//private static int nbVoisines = 0;
	public bool isHidden; //La case n'est pas dévoilée
	public bool isMined; //La case n'est pas minée (elle peut le devenir)
	public bool isMarked; //La case n'est pas marquée comme minée
	public int HasMineVoisines => Voisines.Count(c => c.isMined);
	public List<Case> Voisines { get; set; } = [];

	//public Image Image { get => (Image)pictBox.Image.Clone(); set { pictBox.Image = value; } }
	public TextureButton? Image { get; set; }

	private const string imgDir = @"Images\";

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
	public Case(int i_x, int i_y, bool _isMined = false, bool _isHidden = true, bool _isMarked = false)
	{
		populationId = population++;
		isHidden = _isHidden;
		isMarked = _isMarked;
		isMined = _isMined;
		Image = Plateau.AddCase?.Invoke(i_x * Size_x, i_y * Size_y);
		if (Image is not null && Plateau.CaseClick is not null)
			Image.GuiInput += Plateau.CaseClick(this);
	}

	public void Reveal()
	{
		//Image = !isMined ? Image.FromFile(imgDir + HasMineVoisines + ".png") : Image.FromFile(imgDir + "m.png");
		isHidden = false;
		Plateau.SetTexture?.Invoke(this);
	}

	public void Marque()
	{
		//Image = Image.FromFile(imgDir + "d.png");
		isMarked = true;
		Plateau.SetTexture?.Invoke(this);
	}

	public void Demarque()
	{
		//Image = imgHidden;
		isMarked = false;
		Plateau.SetTexture?.Invoke(this);
	}

	public void Restore()
	{
		if (save is null) return;
		isHidden = save.isHidden;
		isMined = save.isMined;
		isMarked = save.isMarked;
		Image = save.Image;
		Plateau.SetTexture?.Invoke(this);
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
		if (isHidden) return;
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
