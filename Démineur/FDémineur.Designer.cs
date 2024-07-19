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
		this.components = new System.ComponentModel.Container();
		this.ssMines = new System.Windows.Forms.StatusStrip();
		this.tsslMines = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslReste = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslSep = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslTotal = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslGameOver = new System.Windows.Forms.ToolStripStatusLabel();
		this.tspbReste = new System.Windows.Forms.ToolStripProgressBar();
		this.tsslTemps = new System.Windows.Forms.ToolStripStatusLabel();
		this.tTemps = new System.Windows.Forms.Timer(this.components);
		this.ssMines.SuspendLayout();
		this.SuspendLayout();
		// 
		// ssMines
		// 
		this.ssMines.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tsslMines,
			this.tsslReste,
			this.tsslSep,
			this.tsslTotal,
			this.tspbReste,
			this.tsslGameOver,
			this.tsslTemps});
		this.ssMines.Location = new System.Drawing.Point(0, 364);
		this.ssMines.Name = "ssMines";
		this.ssMines.Size = new System.Drawing.Size(693, 22);
		this.ssMines.TabIndex = 0;
		this.ssMines.Text = "statusStrip1";
		// 
		// tsslMines
		// 
		this.tsslMines.Name = "tsslMines";
		this.tsslMines.Size = new System.Drawing.Size(98, 17);
		this.tsslMines.Text = "Mines restantes : ";
		// 
		// tsslReste
		// 
		this.tsslReste.Name = "tsslReste";
		this.tsslReste.Size = new System.Drawing.Size(19, 17);
		this.tsslReste.Text = "56";
		this.tsslReste.TextChanged += new System.EventHandler(this.TsslReste_TextChanged);
		// 
		// tsslSep
		// 
		this.tsslSep.Name = "tsslSep";
		this.tsslSep.Size = new System.Drawing.Size(12, 17);
		this.tsslSep.Text = "/";
		// 
		// tsslTotal
		// 
		this.tsslTotal.Name = "tsslTotal";
		this.tsslTotal.Size = new System.Drawing.Size(25, 17);
		this.tsslTotal.Text = "200";
		// 
		// tsslGameOver
		// 
		this.tsslGameOver.BackColor = System.Drawing.SystemColors.Control;
		this.tsslGameOver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
		this.tsslGameOver.ForeColor = System.Drawing.Color.DarkRed;
		this.tsslGameOver.Name = "tsslGameOver";
		this.tsslGameOver.Size = new System.Drawing.Size(75, 17);
		this.tsslGameOver.Text = "GameOver !";
		this.tsslGameOver.Visible = false;
		this.tsslGameOver.Click += new System.EventHandler(this.TsslGameOver_Click);
		// 
		// tspbReste
		// 
		this.tspbReste.Maximum = 200;
		this.tspbReste.Name = "tspbReste";
		this.tspbReste.Size = new System.Drawing.Size(100, 16);
		this.tspbReste.Value = 144;
		// 
		// tsslTemps
		// 
		this.tsslTemps.Name = "tsslTemps";
		this.tsslTemps.Size = new System.Drawing.Size(95, 17);
		this.tsslTemps.Text = "(Temps écoulé !)";
		// 
		// tTemps
		// 
		this.tTemps.Enabled = true;
		this.tTemps.Interval = 1000;
		this.tTemps.Tick += new System.EventHandler(this.TTemps_Tick);
		// 
		// fDémineur
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(693, 386);
		this.Controls.Add(this.ssMines);
		this.Name = "fDémineur";
		this.Text = "Démineur";
		this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FDémineur_KeyPress);
		this.ssMines.ResumeLayout(false);
		this.ssMines.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();

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
