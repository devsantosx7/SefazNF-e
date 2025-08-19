using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor;

public class frmSpecialCmd : Form
{
	private IContainer components;

	private ImageList imgCertific;

	private Button btConfirmar;

	private Button btCancelar;

	private TextBox txSpecialCmd;

	private Label label2;

	public frmSpecialCmd()
	{
		InitializeComponent();
	}

	public string funcGetCommand()
	{
		return txSpecialCmd.Text.Replace("/", "");
	}

	private void btCancelar_Click(object sender, EventArgs e)
	{
		txSpecialCmd.Text = string.Empty;
		Close();
	}

	private void btConfirmar_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmSpecialCmd_Leave(object sender, EventArgs e)
	{
		txSpecialCmd.Text = string.Empty;
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmSpecialCmd));
		this.imgCertific = new System.Windows.Forms.ImageList(this.components);
		this.btConfirmar = new System.Windows.Forms.Button();
		this.btCancelar = new System.Windows.Forms.Button();
		this.txSpecialCmd = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.imgCertific.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgCertific.ImageStream");
		this.imgCertific.TransparentColor = System.Drawing.Color.Transparent;
		this.imgCertific.Images.SetKeyName(0, "image_certificate.jpg");
		this.btConfirmar.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btConfirmar.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.btConfirmar.Location = new System.Drawing.Point(359, 58);
		this.btConfirmar.Name = "btConfirmar";
		this.btConfirmar.Size = new System.Drawing.Size(87, 34);
		this.btConfirmar.TabIndex = 1;
		this.btConfirmar.Text = "&Confirmar";
		this.btConfirmar.UseVisualStyleBackColor = true;
		this.btConfirmar.Click += new System.EventHandler(btConfirmar_Click);
		this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btCancelar.Location = new System.Drawing.Point(16, 58);
		this.btCancelar.Name = "btCancelar";
		this.btCancelar.Size = new System.Drawing.Size(87, 34);
		this.btCancelar.TabIndex = 2;
		this.btCancelar.Text = "Cancela&r";
		this.btCancelar.UseVisualStyleBackColor = true;
		this.btCancelar.Click += new System.EventHandler(btCancelar_Click);
		this.txSpecialCmd.Font = new System.Drawing.Font("Verdana", 12f);
		this.txSpecialCmd.Location = new System.Drawing.Point(13, 13);
		this.txSpecialCmd.Name = "txSpecialCmd";
		this.txSpecialCmd.PasswordChar = '*';
		this.txSpecialCmd.Size = new System.Drawing.Size(433, 27);
		this.txSpecialCmd.TabIndex = 0;
		this.txSpecialCmd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(13, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(433, 2);
		this.label2.TabIndex = 109;
		base.AcceptButton = this.btConfirmar;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.CancelButton = this.btCancelar;
		base.ClientSize = new System.Drawing.Size(461, 101);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.txSpecialCmd);
		base.Controls.Add(this.btCancelar);
		base.Controls.Add(this.btConfirmar);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmSpecialCmd";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Comando especial";
		base.Leave += new System.EventHandler(frmSpecialCmd_Leave);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
