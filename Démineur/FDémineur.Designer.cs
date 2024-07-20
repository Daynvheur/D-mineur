//#define GODOT
#if GODOT
#else
namespace Démineur;

partial class FDémineur : System.Windows.Forms.Form
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		components = new System.ComponentModel.Container();
		ssMines = new StatusStrip();
		tsslMines = new ToolStripStatusLabel();
		tsslReste = new ToolStripStatusLabel();
		tsslSep = new ToolStripStatusLabel();
		tsslTotal = new ToolStripStatusLabel();
		tsslGameOver = new ToolStripStatusLabel();
		tspbReste = new ToolStripProgressBar();
		tsslTemps = new ToolStripStatusLabel();
		tTemps = new System.Windows.Forms.Timer(components);
		ssMines.SuspendLayout();
		SuspendLayout();
		// 
		// ssMines
		// 
		ssMines.Items.AddRange(new ToolStripItem[] { tsslMines, tsslReste, tsslSep, tsslTotal, tspbReste, tsslGameOver, tsslTemps });
		ssMines.Location = new Point(0, 364);
		ssMines.Name = "ssMines";
		ssMines.Size = new Size(693, 22);
		ssMines.TabIndex = 0;
		ssMines.Text = "statusStrip1";
		// 
		// tsslMines
		// 
		tsslMines.Name = "tsslMines";
		tsslMines.Size = new Size(98, 17);
		tsslMines.Text = "Mines restantes : ";
		// 
		// tsslReste
		// 
		tsslReste.Name = "tsslReste";
		tsslReste.Size = new Size(19, 17);
		tsslReste.Text = "56";
		tsslReste.TextChanged += TsslReste_TextChanged;
		// 
		// tsslSep
		// 
		tsslSep.Name = "tsslSep";
		tsslSep.Size = new Size(12, 17);
		tsslSep.Text = "/";
		// 
		// tsslTotal
		// 
		tsslTotal.Name = "tsslTotal";
		tsslTotal.Size = new Size(25, 17);
		tsslTotal.Text = "200";
		// 
		// tsslGameOver
		// 
		tsslGameOver.BackColor = SystemColors.Control;
		tsslGameOver.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
		tsslGameOver.ForeColor = Color.DarkRed;
		tsslGameOver.Name = "tsslGameOver";
		tsslGameOver.Size = new Size(75, 17);
		tsslGameOver.Text = "GameOver !";
		tsslGameOver.Visible = false;
		tsslGameOver.Click += TsslGameOver_Click;
		// 
		// tspbReste
		// 
		tspbReste.Maximum = 200;
		tspbReste.Name = "tspbReste";
		tspbReste.Size = new Size(100, 16);
		tspbReste.Value = 144;
		// 
		// tsslTemps
		// 
		tsslTemps.Name = "tsslTemps";
		tsslTemps.Size = new Size(95, 17);
		tsslTemps.Text = "(Temps écoulé !)";
		// 
		// tTemps
		// 
		tTemps.Enabled = true;
		tTemps.Interval = 1000;
		tTemps.Tick += TTemps_Tick;
		// 
		// fDémineur
		// 
		AutoScaleDimensions = new SizeF(6F, 13F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(693, 386);
		Controls.Add(ssMines);
		Name = "fDémineur";
		Text = "Démineur";
		KeyPress += FDémineur_KeyPress;
		ssMines.ResumeLayout(false);
		ssMines.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private System.Windows.Forms.StatusStrip ssMines;
	private System.Windows.Forms.ToolStripStatusLabel tsslMines;
	private System.Windows.Forms.ToolStripStatusLabel tsslReste;
	private System.Windows.Forms.ToolStripStatusLabel tsslSep;
	private System.Windows.Forms.ToolStripStatusLabel tsslTotal;
	private System.Windows.Forms.ToolStripStatusLabel tsslGameOver;
	private System.Windows.Forms.ToolStripProgressBar tspbReste;
	private System.Windows.Forms.ToolStripStatusLabel tsslTemps;
	private System.Windows.Forms.Timer tTemps;



}
#endif
