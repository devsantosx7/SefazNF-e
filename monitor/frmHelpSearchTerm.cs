using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace Monitor;

public class frmHelpSearchTerm : Form
{
	private clsFeatureService varclsFeatService = new clsFeatureService();

	private string varFeatExtId = string.Empty;

	private IContainer components;

	private Label label3;

	private Label lbTitle05;

	private Button btSair;

	private Button btComprar;

	private Label lbText05;

	private Label lbTitle01;

	private Label lbText01;

	private Label lbText03;

	private Label lbTitle03;

	private Label lbText04;

	private Label lbTitle04;

	private Label lbText02;

	private Label lbTitle02;

	private PictureBox lbPicture01;

	private PictureBox lbPicture02;

	private PictureBox lbPicture03;

	private PictureBox lbPicture04;

	private PictureBox lbPicture05;

	private Label lbSeparator01;

	private Label lbSalesMessage;

	private PictureBox picRocket;

	private PictureBox lbPicture06;

	private Label lbText06;

	private Label lbTitle06;

	public frmHelpSearchTerm()
	{
		InitializeComponent();
		varFeatExtId = clsFeatureService.consSearchInteligent;
	}

	private async void frmHelpSearchTerm_Load(object sender, EventArgs e)
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
		lbTitle03.ForeColor = varForeColor;
		lbText03.ForeColor = varForeColor;
		lbPicture03.Image = varImagePict;
		lbTitle04.ForeColor = varForeColor;
		lbText04.ForeColor = varForeColor;
		lbPicture04.Image = varImagePict;
		lbTitle05.ForeColor = varForeColor;
		lbText05.ForeColor = varForeColor;
		lbPicture05.Image = varImagePict;
		lbTitle06.ForeColor = varForeColor;
		lbText06.ForeColor = varForeColor;
		lbPicture06.Image = varImagePict;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmHelpSearchTerm));
		this.label3 = new System.Windows.Forms.Label();
		this.lbTitle05 = new System.Windows.Forms.Label();
		this.btSair = new System.Windows.Forms.Button();
		this.btComprar = new System.Windows.Forms.Button();
		this.lbText05 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbText03 = new System.Windows.Forms.Label();
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.lbText04 = new System.Windows.Forms.Label();
		this.lbTitle04 = new System.Windows.Forms.Label();
		this.lbText02 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.lbPicture01 = new System.Windows.Forms.PictureBox();
		this.lbPicture02 = new System.Windows.Forms.PictureBox();
		this.lbPicture03 = new System.Windows.Forms.PictureBox();
		this.lbPicture04 = new System.Windows.Forms.PictureBox();
		this.lbPicture05 = new System.Windows.Forms.PictureBox();
		this.lbSeparator01 = new System.Windows.Forms.Label();
		this.lbSalesMessage = new System.Windows.Forms.Label();
		this.picRocket = new System.Windows.Forms.PictureBox();
		this.lbPicture06 = new System.Windows.Forms.PictureBox();
		this.lbText06 = new System.Windows.Forms.Label();
		this.lbTitle06 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.lbPicture01).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture02).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture03).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture04).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture05).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picRocket).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture06).BeginInit();
		base.SuspendLayout();
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(11, 417);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(520, 2);
		this.label3.TabIndex = 114;
		this.lbTitle05.AutoSize = true;
		this.lbTitle05.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle05.Location = new System.Drawing.Point(42, 249);
		this.lbTitle05.Name = "lbTitle05";
		this.lbTitle05.Size = new System.Drawing.Size(207, 16);
		this.lbTitle05.TabIndex = 112;
		this.lbTitle05.Text = "Busca por Chave de Acesso";
		this.btSair.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btSair.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btSair.Location = new System.Drawing.Point(435, 426);
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
		this.btComprar.Location = new System.Drawing.Point(12, 426);
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
		this.lbText05.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText05.Location = new System.Drawing.Point(42, 269);
		this.lbText05.Name = "lbText05";
		this.lbText05.Size = new System.Drawing.Size(485, 32);
		this.lbText05.TabIndex = 129;
		this.lbText05.Text = "Digite a chave e pressione [ENTER]. O monitor buscará o documento desta chave e todos os outros  relacionados a ela (CT-e, CCe, etc).";
		this.lbTitle01.AutoSize = true;
		this.lbTitle01.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.Location = new System.Drawing.Point(42, 9);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(247, 16);
		this.lbTitle01.TabIndex = 130;
		this.lbTitle01.Text = "Busca por número do documento";
		this.lbText01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText01.Location = new System.Drawing.Point(42, 29);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(485, 18);
		this.lbText01.TabIndex = 131;
		this.lbText01.Text = "Digite o número do documento e pressione [ENTER].";
		this.lbText03.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText03.Location = new System.Drawing.Point(42, 141);
		this.lbText03.Name = "lbText03";
		this.lbText03.Size = new System.Drawing.Size(485, 32);
		this.lbText03.TabIndex = 133;
		this.lbText03.Text = "Digite o CNPJ ou CPF e pressione [ENTER]. O monitor buscará os documentos dos parceiros relacionados à informação inserida.";
		this.lbTitle03.AutoSize = true;
		this.lbTitle03.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle03.Location = new System.Drawing.Point(42, 121);
		this.lbTitle03.Name = "lbTitle03";
		this.lbTitle03.Size = new System.Drawing.Size(354, 16);
		this.lbTitle03.TabIndex = 132;
		this.lbTitle03.Text = "Busca por CNPJ e/ou CPF do Parceiro Comercial";
		this.lbText04.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText04.Location = new System.Drawing.Point(42, 205);
		this.lbText04.Name = "lbText04";
		this.lbText04.Size = new System.Drawing.Size(485, 32);
		this.lbText04.TabIndex = 135;
		this.lbText04.Text = "Digite o nome ou sua parte e pressione [ENTER]. O monitor buscará os documentos dos parceiros relacionados à informação inserida. ";
		this.lbTitle04.AutoSize = true;
		this.lbTitle04.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle04.Location = new System.Drawing.Point(42, 185);
		this.lbTitle04.Name = "lbTitle04";
		this.lbTitle04.Size = new System.Drawing.Size(288, 16);
		this.lbTitle04.TabIndex = 134;
		this.lbTitle04.Text = "Busca por Nome do Parceiro Comercial";
		this.lbText02.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText02.Location = new System.Drawing.Point(42, 78);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(485, 32);
		this.lbText02.TabIndex = 137;
		this.lbText02.Text = "Digite o CFOP e pressione [ENTER]. O monitor buscará todos os documentos fiscais que tem o CFOP digitado.";
		this.lbTitle02.AutoSize = true;
		this.lbTitle02.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.Location = new System.Drawing.Point(42, 58);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(122, 16);
		this.lbTitle02.TabIndex = 136;
		this.lbTitle02.Text = "Busca por CFOP";
		this.lbPicture01.Image = Monitor.Resources.image_free;
		this.lbPicture01.Location = new System.Drawing.Point(11, 12);
		this.lbPicture01.Name = "lbPicture01";
		this.lbPicture01.Size = new System.Drawing.Size(25, 25);
		this.lbPicture01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture01.TabIndex = 138;
		this.lbPicture01.TabStop = false;
		this.lbPicture02.Image = (System.Drawing.Image)resources.GetObject("lbPicture02.Image");
		this.lbPicture02.Location = new System.Drawing.Point(11, 58);
		this.lbPicture02.Name = "lbPicture02";
		this.lbPicture02.Size = new System.Drawing.Size(25, 25);
		this.lbPicture02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture02.TabIndex = 139;
		this.lbPicture02.TabStop = false;
		this.lbPicture03.Image = (System.Drawing.Image)resources.GetObject("lbPicture03.Image");
		this.lbPicture03.Location = new System.Drawing.Point(11, 121);
		this.lbPicture03.Name = "lbPicture03";
		this.lbPicture03.Size = new System.Drawing.Size(25, 25);
		this.lbPicture03.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture03.TabIndex = 140;
		this.lbPicture03.TabStop = false;
		this.lbPicture04.Image = (System.Drawing.Image)resources.GetObject("lbPicture04.Image");
		this.lbPicture04.Location = new System.Drawing.Point(11, 185);
		this.lbPicture04.Name = "lbPicture04";
		this.lbPicture04.Size = new System.Drawing.Size(25, 25);
		this.lbPicture04.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture04.TabIndex = 141;
		this.lbPicture04.TabStop = false;
		this.lbPicture05.Image = (System.Drawing.Image)resources.GetObject("lbPicture05.Image");
		this.lbPicture05.Location = new System.Drawing.Point(11, 249);
		this.lbPicture05.Name = "lbPicture05";
		this.lbPicture05.Size = new System.Drawing.Size(25, 25);
		this.lbPicture05.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture05.TabIndex = 142;
		this.lbPicture05.TabStop = false;
		this.lbSeparator01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator01.Location = new System.Drawing.Point(11, 375);
		this.lbSeparator01.Name = "lbSeparator01";
		this.lbSeparator01.Size = new System.Drawing.Size(520, 2);
		this.lbSeparator01.TabIndex = 143;
		this.lbSalesMessage.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbSalesMessage.ForeColor = System.Drawing.Color.Blue;
		this.lbSalesMessage.Location = new System.Drawing.Point(42, 388);
		this.lbSalesMessage.Name = "lbSalesMessage";
		this.lbSalesMessage.Size = new System.Drawing.Size(489, 16);
		this.lbSalesMessage.TabIndex = 144;
		this.lbSalesMessage.Text = "Habilite todas as modalidades de busca assinando qualquer um dos planos.";
		this.lbSalesMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picRocket.Image = Monitor.Resources.image_rocket_original;
		this.picRocket.Location = new System.Drawing.Point(11, 383);
		this.picRocket.Name = "picRocket";
		this.picRocket.Size = new System.Drawing.Size(27, 27);
		this.picRocket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picRocket.TabIndex = 145;
		this.picRocket.TabStop = false;
		this.lbPicture06.Image = (System.Drawing.Image)resources.GetObject("lbPicture06.Image");
		this.lbPicture06.Location = new System.Drawing.Point(11, 313);
		this.lbPicture06.Name = "lbPicture06";
		this.lbPicture06.Size = new System.Drawing.Size(25, 25);
		this.lbPicture06.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.lbPicture06.TabIndex = 148;
		this.lbPicture06.TabStop = false;
		this.lbText06.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbText06.Location = new System.Drawing.Point(42, 333);
		this.lbText06.Name = "lbText06";
		this.lbText06.Size = new System.Drawing.Size(485, 32);
		this.lbText06.TabIndex = 147;
		this.lbText06.Text = "Digite \"LOTE:\" + o número de lote relacionado a tarefas planejadas.\r\nO monitor buscará todos os documentos relacionados ao lote\r\n";
		this.lbTitle06.AutoSize = true;
		this.lbTitle06.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle06.Location = new System.Drawing.Point(42, 313);
		this.lbTitle06.Name = "lbTitle06";
		this.lbTitle06.Size = new System.Drawing.Size(254, 16);
		this.lbTitle06.TabIndex = 146;
		this.lbTitle06.Text = "Busca por Lote de Processamento";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(548, 468);
		base.Controls.Add(this.lbPicture06);
		base.Controls.Add(this.lbText06);
		base.Controls.Add(this.lbTitle06);
		base.Controls.Add(this.picRocket);
		base.Controls.Add(this.lbSalesMessage);
		base.Controls.Add(this.lbSeparator01);
		base.Controls.Add(this.lbPicture05);
		base.Controls.Add(this.lbPicture04);
		base.Controls.Add(this.lbPicture03);
		base.Controls.Add(this.lbPicture02);
		base.Controls.Add(this.lbPicture01);
		base.Controls.Add(this.lbText02);
		base.Controls.Add(this.lbTitle02);
		base.Controls.Add(this.lbText04);
		base.Controls.Add(this.lbTitle04);
		base.Controls.Add(this.lbText03);
		base.Controls.Add(this.lbTitle03);
		base.Controls.Add(this.lbText01);
		base.Controls.Add(this.lbTitle01);
		base.Controls.Add(this.lbText05);
		base.Controls.Add(this.btComprar);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.lbTitle05);
		base.Controls.Add(this.btSair);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmHelpSearchTerm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Ajuda sobre a Busca Inteligente";
		base.Load += new System.EventHandler(frmHelpSearchTerm_Load);
		((System.ComponentModel.ISupportInitialize)this.lbPicture01).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture02).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture03).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture04).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture05).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picRocket).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lbPicture06).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
