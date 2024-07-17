using System.Drawing.Drawing2D;

namespace Démineur;
public class PixelBox : PictureBox
{
	protected override void OnPaint(PaintEventArgs pe)
	{
		pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
		pe.Graphics.PixelOffsetMode = PixelOffsetMode.None;
		base.OnPaint(pe);
	}
}

public class Case
{
	private static int population = 0; //Déclaration d'existence dans la population

	public static int Size_x { get; set; } = 12 * 4;
	public static int Size_y { get; set; } = 12 * 4;

	//private static int nbVoisines = 0;
	public bool isHidden = true; //La case n'est pas dévoilée
	public bool isMined = false; //La case n'est pas minée (elle peut le devenir)
	public bool isMarked = false; //La case n'est pas marquée comme minée
	public int HasMineVoisines => Voisines.Where(c => c.isMined).Count();
	public List<Case> Voisines { get; set; } = [];
	public Image Image { get => (Image)pictBox.Image.Clone(); set { pictBox.Image = value; } }

	private const string imgDir = @"Images\";
	private static readonly Image imgHidden = Image.FromFile(imgDir + "_.png");

	//Affichage dans une PictureBox
	public PixelBox pictBox = new()
	{
		Size = new Size(Size_x, Size_y),  //dimensions
		Name = population.ToString(), //nom
		Image = imgHidden, //apparence
		SizeMode = PictureBoxSizeMode.Zoom
	};

	private Case? saved;

	private Case() { }
	public Case(int i_x, int i_y, Action<object?, EventArgs> pbCase_Click)
	{
		population++;
		isHidden = true;
		isMarked = false;
		Image = imgHidden;
		pictBox.Location = new Point(i_x * Size_x, i_y * Size_y); //positionnement du point haut-gauche
		pictBox.Click += new EventHandler(pbCase_Click);
	}

	public void Reveal()
	{
		Image = !isMined ? Image.FromFile(imgDir + HasMineVoisines + ".png") : Image.FromFile(imgDir + "m.png");
		isHidden = false;
	}

	public void Marque()
	{
		Image = Image.FromFile(imgDir + "d.png");
		isMarked = true;
	}

	public void Demarque()
	{
		Image = imgHidden;
		isMarked = false;
	}

	public void Restore() => Restore(saved);
	private void Restore(Case? @case)
	{
		if (@case is null) return;
		isHidden = @case.isHidden;
		isMined = @case.isMined;
		isMarked = @case.isMarked;
		Image = @case.Image;
	}

	public void Save()
	{
		saved = new()
		{
			isHidden = isHidden,
			isMined = isMined,
			isMarked = isMarked,
			Image = (Image)pictBox.Image.Clone()
		};
	}
}