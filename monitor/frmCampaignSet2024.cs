using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace monitor;

public class frmCampaignSet2024 : Form
{
	private IContainer components;

	private Panel pnForm;

	private PictureBox picCampaignSet;

	private Button btCampaignAction;

	private Button btClose;

	public frmCampaignSet2024()
	{
		InitializeComponent();
	}

	private void frmCampaignSet2024_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void btCampaignAction_Click(object sender, EventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://mkt.fiscal.io/mes-do-cliente-fiscal-io-2024?utm_source=monitor&utm_medium=banner&utm_campaign=mes_do_cliente", "");
		Cursor = Cursors.Default;
		Close();
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(monitor.frmCampaignSet2024));
		this.pnForm = new System.Windows.Forms.Panel();
		this.btCampaignAction = new System.Windows.Forms.Button();
		this.btClose = new System.Windows.Forms.Button();
		this.picCampaignSet = new System.Windows.Forms.PictureBox();
		this.pnForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picCampaignSet).BeginInit();
		base.SuspendLayout();
		this.pnForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnForm.Controls.Add(this.btCampaignAction);
		this.pnForm.Controls.Add(this.btClose);
		this.pnForm.Controls.Add(this.picCampaignSet);
		this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnForm.Location = new System.Drawing.Point(0, 0);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(775, 484);
		this.pnForm.TabIndex = 292;
		this.btCampaignAction.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btCampaignAction.FlatAppearance.BorderSize = 0;
		this.btCampaignAction.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btCampaignAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btCampaignAction.ForeColor = System.Drawing.Color.White;
		this.btCampaignAction.Location = new System.Drawing.Point(278, 398);
		this.btCampaignAction.Name = "btCampaignAction";
		this.btCampaignAction.Size = new System.Drawing.Size(213, 42);
		this.btCampaignAction.TabIndex = 116;
		this.btCampaignAction.Text = "Quero aproveitar";
		this.btCampaignAction.UseVisualStyleBackColor = false;
		this.btCampaignAction.Click += new System.EventHandler(btCampaignAction_Click);
		this.btClose.BackColor = System.Drawing.Color.White;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.ForeColor = System.Drawing.Color.FromArgb(19, 37, 99);
		this.btClose.Location = new System.Drawing.Point(720, 11);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(38, 31);
		this.btClose.TabIndex = 115;
		this.btClose.Text = "X";
		this.btClose.UseVisualStyleBackColor = false;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.picCampaignSet.BackgroundImage = (System.Drawing.Image)resources.GetObject("picCampaignSet.BackgroundImage");
		this.picCampaignSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.picCampaignSet.ErrorImage = null;
		this.picCampaignSet.InitialImage = null;
		this.picCampaignSet.Location = new System.Drawing.Point(1, -1);
		this.picCampaignSet.Name = "picCampaignSet";
		this.picCampaignSet.Size = new System.Drawing.Size(770, 480);
		this.picCampaignSet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
		this.picCampaignSet.TabIndex = 114;
		this.picCampaignSet.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.DimGray;
		base.ClientSize = new System.Drawing.Size(775, 484);
		base.ControlBox = false;
		base.Controls.Add(this.pnForm);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmCampaignSet2024";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCampaignSet2024_KeyDown);
		this.pnForm.ResumeLayout(false);
		this.pnForm.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picCampaignSet).EndInit();
		base.ResumeLayout(false);
	}
}
