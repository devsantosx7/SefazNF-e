using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace Monitor;

public class frmHelpEventInBatch : Form
{
	private clsFeatureService varclsFeatService = new clsFeatureService();

	private string varFeatExtId = string.Empty;

	private IContainer components;

	private Label label3;

	private Button btSair;

	private Button btComprar;

	private Label lbTitle01;

	private Label lbText01;

	private Label lbText02;

	private Label lbTitle02;

	private PictureBox lbPicture01;

	private PictureBox lbPicture02;

	private Label lbSeparator01;

	private Label lbSalesMessage;

	private PictureBox picRocket;

	private Panel panel1;

	private Label label1;

	private Label lbTitle;

	public frmHelpEventInBatch()
	{
		InitializeComponent();
		varFeatExtId = clsFeatureService.consEventInBatch;
	}

	private async void frmHelpEventInBatch_Load(object sender, EventArgs e)
	{
		Color varForeColor = Color.Gray;
		Bitmap varImagePict = Resources.image_locker;
		string varFeatType = "LOCKED";
		clsFeatStatus varFeatStatus = await varclsFeatService.funcGetFeatStatusAsync(varFeatExtId);
		if (varFeatStatus != null)
		{
			varForeColor = varFeatStatus.ftForeColor;
			varImagePict = varFeatStatus.ftBitmpag;
			varFeatType = varFeatStatus.ftFeatType;
		}
		lbPicture01.Image = Resources.image_free;
		lbTitle02.ForeColor = varForeColor;
		lbText02.ForeColor = varForeColor;
		lbPicture02.Image = varImagePict;
		if (!varFeatType.Contains("LOCK"))
		{
			Button button = btComprar;
			Label label = lbSeparator01;
			Label label2 = lbSalesMessage;
			bool flag = (picRocket.Visible = false);
			bool flag3 = (label2.Visible = flag);
			bool visible = (label.Visible = flag3);
			button.Visible = visible;
		}
		else
		{
			Button button2 = btComprar;
			Label label3 = lbSeparator01;
			Label label4 = lbSalesMessage;
			bool flag = (picRocket.Visible = true);
			bool flag3 = (label4.Visible = flag);
			bool visible = (label3.Visible = flag3);
			button2.Visible = visible;
		}
	}

	private void btComprar_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private void btSair_Click(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmHelpEventInBatch));
		this.label3 = new System.Windows.Forms.Label();
		this.btSair = new System.Windows.Forms.Button();
		this.btComprar = new System.Windows.Forms.Button();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbText02 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.lbPicture01 = new System.Windows.Forms.PictureBox();
		this.lbPicture02 = new System.Windows.Forms.PictureBox();
		this.lbSeparator01 = new System.Windows.Forms.Label();
		this.lbSalesMessage = new System.Windows.Forms.Label();
		this.picRocket = new System.Windows.Forms.PictureBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.lbTitle = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.lbPicture01).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture02).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picRocket).BeginInit();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(11, 274);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(520, 2);
		this.label3.TabIndex = 114;
		this.btSair.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btSair.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btSair.Location = new System.Drawing.Point(435, 283);
		this.btSair.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btSair.Name = "btSair";
		this.btSair.Size = new System.Drawing.Size(99, 31);
		this.btSair.TabIndex = 111;
		this.btSair.Text = "&Sair";
		this.btSair.UseVisualStyleBackColor = true;
		this.btSair.Click += new System.EventHandler(btSair_Click);
		this.btComprar.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btComprar.Image = (System.Drawing.Image)resources.GetObject("btComprar.Image");
		this.btComprar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btComprar.Location = new System.Drawing.Point(12, 283);
		this.btComprar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btComprar.Name = "btComprar";
		this.btComprar.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.btComprar.Size = new System.Drawing.Size(173, 31);
		this.btComprar.TabIndex = 124;
		this.btComprar.Text = "&Conheça os planos";
		this.btComprar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btComprar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btComprar.UseVisualStyleBackColor = true;
		this.btComprar.Click += new System.EventHandler(btComprar_Click);
		this.lbTitle01.AutoSize = true;
		this.lbTitle01.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.Location = new System.Drawing.Point(42, 110);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(410, 16);
		this.lbTitle01.TabIndex = 130;
		this.lbTitle01.Text = "Registro dos Eventos por parceiro comercial [ Gratuito ]";
		this.lbText01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText01.Location = new System.Drawing.Point(42, 130);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(485, 32);
		this.lbText01.TabIndex = 131;
		this.lbText01.Text = "Selecione os documentos do parceiro comercial, logo após execute os passos para registro do evento.";
		this.lbText02.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText02.Location = new System.Drawing.Point(42, 192);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(485, 32);
		this.lbText02.TabIndex = 137;
		this.lbText02.Text = "Selecione todos os documentos \"independente\" do parceiro comercial, logo após execute os passos para registro do evento.";
		this.lbTitle02.AutoSize = true;
		this.lbTitle02.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.Location = new System.Drawing.Point(42, 172);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(466, 16);
		this.lbTitle02.TabIndex = 136;
		this.lbTitle02.Text = "Registro dos Eventos para diversos parceiros ao mesmo tempo";
		this.lbPicture01.Image = (System.Drawing.Image)resources.GetObject("lbPicture01.Image");
		this.lbPicture01.Location = new System.Drawing.Point(11, 113);
		this.lbPicture01.Name = "lbPicture01";
		this.lbPicture01.Size = new System.Drawing.Size(25, 25);
		this.lbPicture01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture01.TabIndex = 138;
		this.lbPicture01.TabStop = false;
		this.lbPicture02.Image = (System.Drawing.Image)resources.GetObject("lbPicture02.Image");
		this.lbPicture02.Location = new System.Drawing.Point(11, 172);
		this.lbPicture02.Name = "lbPicture02";
		this.lbPicture02.Size = new System.Drawing.Size(25, 25);
		this.lbPicture02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture02.TabIndex = 139;
		this.lbPicture02.TabStop = false;
		this.lbSeparator01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator01.Location = new System.Drawing.Point(11, 232);
		this.lbSeparator01.Name = "lbSeparator01";
		this.lbSeparator01.Size = new System.Drawing.Size(520, 2);
		this.lbSeparator01.TabIndex = 143;
		this.lbSalesMessage.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbSalesMessage.ForeColor = System.Drawing.Color.Blue;
		this.lbSalesMessage.Location = new System.Drawing.Point(42, 245);
		this.lbSalesMessage.Name = "lbSalesMessage";
		this.lbSalesMessage.Size = new System.Drawing.Size(489, 16);
		this.lbSalesMessage.TabIndex = 144;
		this.lbSalesMessage.Text = "Habilite o registro em massa assinando qualquer um dos planos.";
		this.lbSalesMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picRocket.Image = Monitor.Resources.image_rocket_original;
		this.picRocket.Location = new System.Drawing.Point(11, 240);
		this.picRocket.Name = "picRocket";
		this.picRocket.Size = new System.Drawing.Size(27, 27);
		this.picRocket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picRocket.TabIndex = 145;
		this.picRocket.TabStop = false;
		this.panel1.BackColor = System.Drawing.Color.Cornsilk;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.lbTitle);
		this.panel1.Location = new System.Drawing.Point(14, 12);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(520, 87);
		this.panel1.TabIndex = 146;
		this.label1.Font = new System.Drawing.Font("Verdana", 8f);
		this.label1.Location = new System.Drawing.Point(6, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(509, 32);
		this.label1.TabIndex = 148;
		this.label1.Text = "Se você utiliza este recurso com frequência e precisa de mais agilidade, contrate um dos nossos planos e tenha o registro do evento para vários parceiros.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.lbTitle.Location = new System.Drawing.Point(6, 6);
		this.lbTitle.Name = "lbTitle";
		this.lbTitle.Size = new System.Drawing.Size(509, 37);
		this.lbTitle.TabIndex = 147;
		this.lbTitle.Text = "Para este evento você deve selecionar os documentos \r\nde um único Parceiro Comercial.";
		this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(548, 322);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.picRocket);
		base.Controls.Add(this.lbSalesMessage);
		base.Controls.Add(this.lbSeparator01);
		base.Controls.Add(this.lbPicture02);
		base.Controls.Add(this.lbPicture01);
		base.Controls.Add(this.lbText02);
		base.Controls.Add(this.lbTitle02);
		base.Controls.Add(this.lbText01);
		base.Controls.Add(this.lbTitle01);
		base.Controls.Add(this.btComprar);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.btSair);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmHelpEventInBatch";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Ajuda sobre Registro de Eventos em Massa";
		base.Load += new System.EventHandler(frmHelpEventInBatch_Load);
		((System.ComponentModel.ISupportInitialize)this.lbPicture01).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture02).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picRocket).EndInit();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
