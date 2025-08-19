using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using screen.fiscal.io;
using screen.fiscal.io.SalesManager;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmBannerStorageAlmostFull : Form
{
	private Timer _timer;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatureService = new clsFeatureService();

	private clsDataProduct _clsDataProduct = new clsDataProduct(null);

	private clsReturn _clsReturn = new clsReturn();

	private double varTotalSizeInGB;

	private IContainer components;

	private Panel pnForm;

	private Button btnSalesContact;

	private PictureBox pictureBox1;

	private LinkLabel linkSaibaMais;

	private Label textBox2;

	private Label textBox1;

	private Label lbSize;

	private Label textBox4;

	private Label textBox7;

	private Label textBox6;

	private Label textBox5;

	private Label textBox12;

	private Label textBox11;

	private Label textBox10;

	private Label textBox9;

	private Label textBox8;

	private HighlightTextRenderer highlightTextRenderer1;

	private Label textBox13;

	private Button btClose;

	public frmBannerStorageAlmostFull()
	{
		InitializeComponent();
		_timer = new Timer();
		_timer.Tick += async delegate
		{
			string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-STORAGE-ALMOST-FULL-LAST-SHOW", varDateTime);
			Close();
			_timer.Stop();
			_timer.Enabled = false;
		};
		_timer.Interval = (int)TimeSpan.FromMinutes(5.0).TotalMilliseconds;
		_timer.Start();
	}

	private async void frmBannerStorageAlmostFull_Load(object sender, EventArgs e)
	{
		long varTotalSizeInKBytes = clsFunction.funcConvStrToLong((await new clsDataConfig().funcGetItemByKeyAsync()).DatabaseSize);
		varTotalSizeInGB = clsFunction.funcConvKBtoGB(varTotalSizeInKBytes);
		int varCount = 0;
		lbSize.Text = $"{varTotalSizeInGB} de 9GB";
		foreach (Product item in await _clsDataProduct.funcGetFullListAsync())
		{
			if (clsFunction.IsEqual(item.ProdType, "PAYED", pIgnoreCase: true))
			{
				varCount++;
			}
		}
		if (varCount > 0)
		{
			btnSalesContact.Text = "Fale com o Suporte";
		}
		string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-STORAGE-ALMOST-FULL-LAST-SHOW", varDateTime);
		await _clsDataParam.funcSetAsync("BANNER-LAST-SHOW-DATE", varDateTime);
	}

	private async void btClose_Click(object sender, EventArgs e)
	{
		string varDateTimeStr = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-STORAGE-ALMOST-FULL-NEXT-SHOW", varDateTimeStr);
		(((clsTaskStatus)base.Tag) ?? new clsTaskStatus()).FormAction = "FORM_CLOSE";
		Close();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		string varUserMessage = $"Banco de dados Local excede o tamanho de 8GB. O tamanho do banco atualmente é de {varTotalSizeInGB}GB";
		string varUserDetail = "Banco de dados Local excede o tamanho de 8GB";
		await _clsDataParam.funcAddCounterAsync("STORAGE-ALMOST-FULL-CLICKS");
		if (clsFunction.IsEqual(btnSalesContact.Text, "Fale com o Suporte", pIgnoreCase: true))
		{
			base.TopMost = false;
			if (clsFunction.IsEmpty(_clsReturn.ApiRefGuid))
			{
				_clsReturn.ApiRefGuid = Guid.NewGuid().ToString();
			}
			if (new frmSupportConfirm(varUserMessage, varUserDetail, _clsReturn.ApiRefGuid).ShowDialog(this).Equals(DialogResult.OK))
			{
				Close();
			}
		}
		else
		{
			frmSalesPayed02 frmSalesPayed = new frmSalesPayed02("STORAGE-ALMOST-FULL");
			frmSalesPayed.ShowDialog(this);
			frmSalesPayed.Dispose();
		}
	}

	private void frmBannerStorageAlmostFull_Leave(object sender, EventArgs e)
	{
		if (_timer != null)
		{
			_timer.Stop();
			_timer.Enabled = false;
			_timer.Dispose();
		}
	}

	private void linkSaibaMais_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallHelpDatabaseLimitAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmBannerStorageAlmostFull));
		this.pnForm = new System.Windows.Forms.Panel();
		this.btClose = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.textBox13 = new System.Windows.Forms.Label();
		this.lbSize = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.Label();
		this.textBox12 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.Label();
		this.textBox11 = new System.Windows.Forms.Label();
		this.textBox10 = new System.Windows.Forms.Label();
		this.textBox9 = new System.Windows.Forms.Label();
		this.textBox8 = new System.Windows.Forms.Label();
		this.textBox7 = new System.Windows.Forms.Label();
		this.textBox6 = new System.Windows.Forms.Label();
		this.textBox5 = new System.Windows.Forms.Label();
		this.textBox4 = new System.Windows.Forms.Label();
		this.linkSaibaMais = new System.Windows.Forms.LinkLabel();
		this.btnSalesContact = new System.Windows.Forms.Button();
		this.highlightTextRenderer1 = new BrightIdeasSoftware.HighlightTextRenderer();
		this.pnForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.pnForm.BackColor = System.Drawing.Color.White;
		this.pnForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.pnForm.Controls.Add(this.btClose);
		this.pnForm.Controls.Add(this.pictureBox1);
		this.pnForm.Controls.Add(this.textBox13);
		this.pnForm.Controls.Add(this.lbSize);
		this.pnForm.Controls.Add(this.textBox2);
		this.pnForm.Controls.Add(this.textBox12);
		this.pnForm.Controls.Add(this.textBox1);
		this.pnForm.Controls.Add(this.textBox11);
		this.pnForm.Controls.Add(this.textBox10);
		this.pnForm.Controls.Add(this.textBox9);
		this.pnForm.Controls.Add(this.textBox8);
		this.pnForm.Controls.Add(this.textBox7);
		this.pnForm.Controls.Add(this.textBox6);
		this.pnForm.Controls.Add(this.textBox5);
		this.pnForm.Controls.Add(this.textBox4);
		this.pnForm.Controls.Add(this.linkSaibaMais);
		this.pnForm.Controls.Add(this.btnSalesContact);
		this.pnForm.ForeColor = System.Drawing.SystemColors.Window;
		this.pnForm.Location = new System.Drawing.Point(4, 4);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(765, 476);
		this.pnForm.TabIndex = 292;
		this.btClose.BackColor = System.Drawing.SystemColors.Window;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.ForeColor = System.Drawing.SystemColors.WindowText;
		this.btClose.Location = new System.Drawing.Point(707, 8);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(43, 27);
		this.btClose.TabIndex = 260;
		this.btClose.Text = "X";
		this.btClose.UseVisualStyleBackColor = false;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
		this.pictureBox1.BackgroundImage = Monitor.Resources.fiscalio_monitor_limite_banco_local;
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.ErrorImage = null;
		this.pictureBox1.InitialImage = null;
		this.pictureBox1.Location = new System.Drawing.Point(45, 22);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(81, 56);
		this.pictureBox1.TabIndex = 243;
		this.pictureBox1.TabStop = false;
		this.textBox13.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox13.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox13.Location = new System.Drawing.Point(109, 131);
		this.textBox13.Name = "textBox13";
		this.textBox13.Size = new System.Drawing.Size(244, 19);
		this.textBox13.TabIndex = 258;
		this.textBox13.Text = "de armazenamento de dados.";
		this.lbSize.BackColor = System.Drawing.SystemColors.Window;
		this.lbSize.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbSize.ForeColor = System.Drawing.SystemColors.WindowText;
		this.lbSize.Location = new System.Drawing.Point(336, 54);
		this.lbSize.Name = "lbSize";
		this.lbSize.Size = new System.Drawing.Size(128, 24);
		this.lbSize.TabIndex = 246;
		this.lbSize.Text = "X de 9GB";
		this.textBox2.BackColor = System.Drawing.SystemColors.Window;
		this.textBox2.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox2.Location = new System.Drawing.Point(279, 54);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(54, 24);
		this.textBox2.TabIndex = 245;
		this.textBox2.Text = "Atual: ";
		this.textBox12.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox12.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox12.Location = new System.Drawing.Point(109, 357);
		this.textBox12.Name = "textBox12";
		this.textBox12.Size = new System.Drawing.Size(454, 19);
		this.textBox12.TabIndex = 256;
		this.textBox12.Text = "Por favor, acione nosso suporte técnico para te auxiliar:";
		this.textBox1.BackColor = System.Drawing.SystemColors.Window;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox1.Location = new System.Drawing.Point(135, 22);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(502, 30);
		this.textBox1.TabIndex = 244;
		this.textBox1.Text = "Armazenamento chegando ao limite!";
		this.textBox11.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox11.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox11.Location = new System.Drawing.Point(162, 319);
		this.textBox11.Name = "textBox11";
		this.textBox11.Size = new System.Drawing.Size(529, 19);
		this.textBox11.TabIndex = 255;
		this.textBox11.Text = "3. Migrar para um banco de dados no seu servidor empresarial;";
		this.textBox10.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox10.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox10.Location = new System.Drawing.Point(162, 294);
		this.textBox10.Name = "textBox10";
		this.textBox10.Size = new System.Drawing.Size(529, 19);
		this.textBox10.TabIndex = 254;
		this.textBox10.Text = "2. Migrar para o banco de dados na nuvem da Fiscal.io;";
		this.textBox9.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox9.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox9.Location = new System.Drawing.Point(162, 269);
		this.textBox9.Name = "textBox9";
		this.textBox9.Size = new System.Drawing.Size(529, 19);
		this.textBox9.TabIndex = 253;
		this.textBox9.Text = "1. Excluir dados da instalação (risco de perda de informações);";
		this.textBox8.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox8.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox8.Location = new System.Drawing.Point(105, 227);
		this.textBox8.Name = "textBox8";
		this.textBox8.Size = new System.Drawing.Size(529, 19);
		this.textBox8.TabIndex = 252;
		this.textBox8.Text = "Para solucionar essa limitação técnica, você pode:";
		this.textBox7.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox7.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox7.Location = new System.Drawing.Point(105, 194);
		this.textBox7.Name = "textBox7";
		this.textBox7.Size = new System.Drawing.Size(558, 19);
		this.textBox7.TabIndex = 251;
		this.textBox7.Text = "documentos e eventos da SEFAZ.";
		this.textBox6.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox6.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox6.Location = new System.Drawing.Point(0, 169);
		this.textBox6.Name = "textBox6";
		this.textBox6.Size = new System.Drawing.Size(765, 19);
		this.textBox6.TabIndex = 250;
		this.textBox6.Text = "Com o banco de dados cheio, o sistema não poderá importar novos ";
		this.textBox6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.textBox5.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox5.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox5.Location = new System.Drawing.Point(614, 107);
		this.textBox5.Name = "textBox5";
		this.textBox5.Size = new System.Drawing.Size(48, 19);
		this.textBox5.TabIndex = 249;
		this.textBox5.Text = "9GB";
		this.textBox4.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox4.ForeColor = System.Drawing.SystemColors.WindowText;
		this.textBox4.Location = new System.Drawing.Point(109, 107);
		this.textBox4.Name = "textBox4";
		this.textBox4.Size = new System.Drawing.Size(542, 19);
		this.textBox4.TabIndex = 248;
		this.textBox4.Text = "Sua instalação do Fiscal.io Monitor está prestes a atingir o limite de";
		this.linkSaibaMais.AutoSize = true;
		this.linkSaibaMais.Location = new System.Drawing.Point(247, 451);
		this.linkSaibaMais.Name = "linkSaibaMais";
		this.linkSaibaMais.Size = new System.Drawing.Size(279, 14);
		this.linkSaibaMais.TabIndex = 242;
		this.linkSaibaMais.TabStop = true;
		this.linkSaibaMais.Text = "Saiba mais sobre o limite máximo de dados";
		this.linkSaibaMais.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkSaibaMais_LinkClicked);
		this.btnSalesContact.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnSalesContact.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btnSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btnSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSalesContact.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnSalesContact.ForeColor = System.Drawing.Color.White;
		this.btnSalesContact.Location = new System.Drawing.Point(270, 394);
		this.btnSalesContact.Name = "btnSalesContact";
		this.btnSalesContact.Size = new System.Drawing.Size(233, 42);
		this.btnSalesContact.TabIndex = 241;
		this.btnSalesContact.Text = "Fale com um consultor";
		this.btnSalesContact.UseVisualStyleBackColor = false;
		this.btnSalesContact.Click += new System.EventHandler(btSalesContact_Click);
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
		base.Name = "frmBannerStorageAlmostFull";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.Load += new System.EventHandler(frmBannerStorageAlmostFull_Load);
		base.Leave += new System.EventHandler(frmBannerStorageAlmostFull_Leave);
		this.pnForm.ResumeLayout(false);
		this.pnForm.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}
