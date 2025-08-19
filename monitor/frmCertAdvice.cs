using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor;

public class frmCertAdvice : Form
{
	private IContainer components;

	private Label label3;

	private Label lbNotasFiscais;

	private Button btClose;

	private PictureBox pictureBox1;

	private Label label1;

	private Label label2;

	private Label label4;

	private Label label5;

	private Label label7;

	private Label label8;

	private Label label9;

	private Label label10;

	private Label label11;

	private Label label12;

	private LinkLabel lkbCertA1Help;

	public frmCertAdvice()
	{
		InitializeComponent();
	}

	private void lkbCertA1Help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process varSysProc = new Process();
		varSysProc.StartInfo.FileName = "https://fiscal.io/blog/como-instalar-certificado-digital-a1-no-chrome-internet-exporer-ie-firefox/?utm_source=monitor&utm_medium=instalacao";
		try
		{
			varSysProc.Start();
		}
		catch
		{
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmCertAdvice));
		this.label3 = new System.Windows.Forms.Label();
		this.lbNotasFiscais = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.lkbCertA1Help = new System.Windows.Forms.LinkLabel();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(14, 243);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(474, 2);
		this.label3.TabIndex = 114;
		this.lbNotasFiscais.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNotasFiscais.Location = new System.Drawing.Point(49, 44);
		this.lbNotasFiscais.Name = "lbNotasFiscais";
		this.lbNotasFiscais.Size = new System.Drawing.Size(430, 25);
		this.lbNotasFiscais.TabIndex = 112;
		this.lbNotasFiscais.Text = "Se o certificado for do tipo A3, faça o seguinte:";
		this.lbNotasFiscais.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btClose.BackColor = System.Drawing.SystemColors.Control;
		this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Location = new System.Drawing.Point(396, 258);
		this.btClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(84, 31);
		this.btClose.TabIndex = 111;
		this.btClose.Text = "&OK";
		this.btClose.UseVisualStyleBackColor = false;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.pictureBox1.Image = Monitor.Resources.image_tool_disabled;
		this.pictureBox1.Location = new System.Drawing.Point(14, 4);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(25, 25);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 115;
		this.pictureBox1.TabStop = false;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(246, 64);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(19, 1);
		this.label1.TabIndex = 116;
		this.label2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(49, 71);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(430, 19);
		this.label2.TabIndex = 117;
		this.label2.Text = "  Retire o certificado da leitora e conecte novamente";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label4.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(68, 93);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(411, 15);
		this.label4.TabIndex = 118;
		this.label4.Text = "O certificado pode ter ficado ocioso por muito tempo.";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label5.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(68, 112);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(411, 15);
		this.label5.TabIndex = 119;
		this.label5.Text = "Uma nova conexão reconhecerá o certificado novamente.";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label7.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(68, 193);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(411, 15);
		this.label7.TabIndex = 123;
		this.label7.Text = "O certificado pode ter sido instalado sem o acesso à chave privada.";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(49, 171);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(430, 19);
		this.label8.TabIndex = 122;
		this.label8.Text = "  Reinstale o certificado digital";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label9.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label9.Location = new System.Drawing.Point(245, 164);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(19, 1);
		this.label9.TabIndex = 121;
		this.label10.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(49, 144);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(430, 25);
		this.label10.TabIndex = 120;
		this.label10.Text = "Se o certificado for do tipo A1, faça o seguinte:";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label11.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label11.Location = new System.Drawing.Point(14, 34);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(474, 2);
		this.label11.TabIndex = 125;
		this.label12.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(48, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(440, 25);
		this.label12.TabIndex = 126;
		this.label12.Text = "Chave Privada não encontrada no Certificado Digital.";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbCertA1Help.AutoSize = true;
		this.lkbCertA1Help.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Italic);
		this.lkbCertA1Help.Location = new System.Drawing.Point(68, 213);
		this.lkbCertA1Help.Name = "lkbCertA1Help";
		this.lkbCertA1Help.Size = new System.Drawing.Size(364, 13);
		this.lkbCertA1Help.TabIndex = 127;
		this.lkbCertA1Help.TabStop = true;
		this.lkbCertA1Help.Text = "Clique aqui e veja como instalar o certificado A1 no Windows. ";
		this.lkbCertA1Help.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbCertA1Help_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(499, 303);
		base.Controls.Add(this.lkbCertA1Help);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.btClose);
		base.Controls.Add(this.lbNotasFiscais);
		base.Controls.Add(this.label10);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmCertAdvice";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Chave Privada não encontrada no Certificado Digital.";
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
