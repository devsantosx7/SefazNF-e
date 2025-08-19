using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmDownOptionCTe : Form
{
	private string _DownOption = string.Empty;

	private IContainer components;

	private Panel panel1;

	private Panel pnOption01;

	private Label lbTitle;

	private Label lbUserInfo02;

	private Label lbUserInfo01;

	private Panel pnOption02;

	private Label label14;

	private Label label7;

	private Label label3;

	private Button btDowOption01;

	private Label label13;

	private Label label9;

	private Label label5;

	private Button btDowOption02;

	private Label label4;

	private Label label2;

	private Label lbBalance;

	private LinkLabel lkbAddCreditToSign01;

	private LinkLabel lkbAddCreditAdHoc01;

	private Label label6;

	private Label label24;

	private Label label18;

	private Label label12;

	private Label label11;

	private Label label25;

	private Label label19;

	private Label label22;

	private Label label23;

	private Label label1;

	private Label label8;

	private Label label26;

	private Label label21;

	public frmDownOptionCTe(bool pHasDocSelected)
	{
		InitializeComponent();
		if (pHasDocSelected)
		{
			lbUserInfo01.Text = "Você selecionou um ou mais documentos sem arquivo XML disponível";
			lbUserInfo02.Text = "Deseja baixar estes arquivos agora?  (escolha uma das opções)";
			return;
		}
		lbUserInfo01.Text = "Escolha uma das opções para baixar os arquivos XML";
		lbUserInfo02.Text = "";
		lbUserInfo02.Visible = false;
		lbUserInfo01.Top = 18;
	}

	private async void frmDownOptionCTe_Load(object sender, EventArgs e)
	{
		clsBalanceService varclsService = new clsBalanceService();
		clsBalanceService.BalanceModel varclsBalance = await varclsService.funcGetBalanceAsync(clsBalanceService.consMetDowQuerySpec);
		lbBalance.Text = "Créditos disponíveis :";
		if (!clsFunction.IsEmpty(varclsBalance.MeaUnit))
		{
			Label label = lbBalance;
			label.Text = label.Text + " " + varclsBalance.MeaUnit;
		}
		Label label2 = lbBalance;
		label2.Text = label2.Text + " " + varclsService.funcGetValStr(varclsBalance.MeaType, varclsBalance.TotalBalc);
		if (await new clsSoftwareService(null).funcHasPayedPlanAsync())
		{
			lkbAddCreditToSign01.Text = "Adicionar creditos a assinatura";
			lkbAddCreditToSign01.Tag = "PAYED";
		}
		else
		{
			lkbAddCreditToSign01.Text = "Fazer assinatura e adquirir créditos";
			lkbAddCreditToSign01.Tag = "FREE";
		}
		btDowOption01.Enabled = true;
		btDowOption02.Visible = true;
		if (!varclsBalance.HasBalance)
		{
			btDowOption02.Enabled = false;
			lbBalance.ForeColor = Color.Red;
		}
		else
		{
			btDowOption02.Enabled = true;
			lbBalance.ForeColor = Color.MidnightBlue;
		}
	}

	public string funcGetDownOption()
	{
		return _DownOption;
	}

	private void btDowOption01_Click(object sender, EventArgs e)
	{
		_DownOption = new clsDFeCodes().GetEvtDownloadXmlWebs();
		Close();
	}

	private void btDowOption02_Click(object sender, EventArgs e)
	{
		_DownOption = new clsDFeCodes().GetEvtDownloadXmlFull();
		Close();
	}

	private void frmDownOptionCTe_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode.Equals(Keys.D1))
		{
			btDowOption01_Click(null, null);
		}
		else if (e.KeyCode.Equals(Keys.D2))
		{
			btDowOption02_Click(null, null);
		}
	}

	private void lkbAddCreditToSign(object sender, EventArgs e)
	{
		LinkLabel varLinkButton = (LinkLabel)sender;
		if (varLinkButton != null)
		{
			if (clsFunction.funcGetValue(varLinkButton.Tag).Equals("PAYED"))
			{
				clsHelpService.funcCallAddCreditToSignAsync();
			}
			else
			{
				clsHelpService.funcCallProductPricePageAsync();
			}
		}
	}

	private void lkbAddCreditAdHoc(object sender, EventArgs e)
	{
		clsHelpService.funcCallBuyCreditAdHocAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmDownOptionCTe));
		this.panel1 = new System.Windows.Forms.Panel();
		this.lbUserInfo02 = new System.Windows.Forms.Label();
		this.lbUserInfo01 = new System.Windows.Forms.Label();
		this.lbTitle = new System.Windows.Forms.Label();
		this.pnOption01 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btDowOption01 = new System.Windows.Forms.Button();
		this.pnOption02 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lkbAddCreditToSign01 = new System.Windows.Forms.LinkLabel();
		this.lkbAddCreditAdHoc01 = new System.Windows.Forms.LinkLabel();
		this.lbBalance = new System.Windows.Forms.Label();
		this.btDowOption02 = new System.Windows.Forms.Button();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.pnOption01.SuspendLayout();
		this.pnOption02.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.SystemColors.Info;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.lbUserInfo02);
		this.panel1.Controls.Add(this.lbUserInfo01);
		this.panel1.Controls.Add(this.lbTitle);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(613, 54);
		this.panel1.TabIndex = 172;
		this.lbUserInfo02.AutoSize = true;
		this.lbUserInfo02.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbUserInfo02.Location = new System.Drawing.Point(4, 28);
		this.lbUserInfo02.Name = "lbUserInfo02";
		this.lbUserInfo02.Size = new System.Drawing.Size(429, 14);
		this.lbUserInfo02.TabIndex = 2;
		this.lbUserInfo02.Text = "Deseja baixar estes arquivos agora?  (escolha uma das opções)";
		this.lbUserInfo01.AutoSize = true;
		this.lbUserInfo01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbUserInfo01.Location = new System.Drawing.Point(4, 6);
		this.lbUserInfo01.Name = "lbUserInfo01";
		this.lbUserInfo01.Size = new System.Drawing.Size(441, 14);
		this.lbUserInfo01.TabIndex = 1;
		this.lbUserInfo01.Text = "Você selecionou um ou mais documentos sem arquivo XML disponível";
		this.lbTitle.AutoSize = true;
		this.lbTitle.Location = new System.Drawing.Point(12, 7);
		this.lbTitle.Name = "lbTitle";
		this.lbTitle.Size = new System.Drawing.Size(0, 13);
		this.lbTitle.TabIndex = 0;
		this.pnOption01.BackColor = System.Drawing.SystemColors.Window;
		this.pnOption01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnOption01.Controls.Add(this.label26);
		this.pnOption01.Controls.Add(this.label24);
		this.pnOption01.Controls.Add(this.label18);
		this.pnOption01.Controls.Add(this.label12);
		this.pnOption01.Controls.Add(this.label11);
		this.pnOption01.Controls.Add(this.label6);
		this.pnOption01.Controls.Add(this.label4);
		this.pnOption01.Controls.Add(this.label2);
		this.pnOption01.Controls.Add(this.label14);
		this.pnOption01.Controls.Add(this.label7);
		this.pnOption01.Controls.Add(this.label3);
		this.pnOption01.Controls.Add(this.btDowOption01);
		this.pnOption01.Location = new System.Drawing.Point(0, 53);
		this.pnOption01.Name = "pnOption01";
		this.pnOption01.Size = new System.Drawing.Size(307, 304);
		this.pnOption01.TabIndex = 173;
		this.label26.BackColor = System.Drawing.SystemColors.Highlight;
		this.label26.Location = new System.Drawing.Point(179, 50);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(47, 1);
		this.label26.TabIndex = 243;
		this.label24.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label24.Location = new System.Drawing.Point(13, 235);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(279, 1);
		this.label24.TabIndex = 235;
		this.label18.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label18.Location = new System.Drawing.Point(13, 76);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(279, 1);
		this.label18.TabIndex = 234;
		this.label12.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label12.Location = new System.Drawing.Point(13, 29);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(279, 1);
		this.label12.TabIndex = 233;
		this.label11.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label11.Location = new System.Drawing.Point(13, 162);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(279, 1);
		this.label11.TabIndex = 232;
		this.label6.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(13, 103);
		this.label6.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(279, 53);
		this.label6.TabIndex = 231;
		this.label6.Text = "Tomadores,  Remetentes, \r\nDestinatários, Recebedores, \r\nExpedidores e Terceiros \r\ncitados no XML\r\n";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
		this.label4.Location = new System.Drawing.Point(13, 189);
		this.label4.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(279, 21);
		this.label4.TabIndex = 230;
		this.label4.Text = "Ilimitado";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 82);
		this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(279, 17);
		this.label2.TabIndex = 229;
		this.label2.Text = "Deve ser utilizado por :";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label14.Font = new System.Drawing.Font("Verdana", 8f);
		this.label14.Location = new System.Drawing.Point(13, 55);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(279, 14);
		this.label14.TabIndex = 227;
		this.label14.Text = "XMLs de até 10 dias atrás";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.Font = new System.Drawing.Font("Verdana", 8f);
		this.label7.Location = new System.Drawing.Point(13, 35);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(279, 14);
		this.label7.TabIndex = 3;
		this.label7.Text = "Somente CTe de Entrada";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label3.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(13, 5);
		this.label3.Margin = new System.Windows.Forms.Padding(0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(279, 19);
		this.label3.TabIndex = 3;
		this.label3.Text = "Download Comum";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btDowOption01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btDowOption01.Image = Monitor.Resources.image_down_button;
		this.btDowOption01.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btDowOption01.Location = new System.Drawing.Point(46, 249);
		this.btDowOption01.Margin = new System.Windows.Forms.Padding(2);
		this.btDowOption01.Name = "btDowOption01";
		this.btDowOption01.Size = new System.Drawing.Size(213, 36);
		this.btDowOption01.TabIndex = 225;
		this.btDowOption01.Text = "1: Baixar XMLs da SEFAZ";
		this.btDowOption01.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btDowOption01.UseVisualStyleBackColor = true;
		this.btDowOption01.Click += new System.EventHandler(btDowOption01_Click);
		this.pnOption02.BackColor = System.Drawing.SystemColors.Window;
		this.pnOption02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnOption02.Controls.Add(this.label21);
		this.pnOption02.Controls.Add(this.label25);
		this.pnOption02.Controls.Add(this.label19);
		this.pnOption02.Controls.Add(this.label22);
		this.pnOption02.Controls.Add(this.label23);
		this.pnOption02.Controls.Add(this.label1);
		this.pnOption02.Controls.Add(this.label8);
		this.pnOption02.Controls.Add(this.lkbAddCreditToSign01);
		this.pnOption02.Controls.Add(this.lkbAddCreditAdHoc01);
		this.pnOption02.Controls.Add(this.lbBalance);
		this.pnOption02.Controls.Add(this.btDowOption02);
		this.pnOption02.Controls.Add(this.label13);
		this.pnOption02.Controls.Add(this.label9);
		this.pnOption02.Controls.Add(this.label5);
		this.pnOption02.Location = new System.Drawing.Point(306, 53);
		this.pnOption02.Name = "pnOption02";
		this.pnOption02.Size = new System.Drawing.Size(307, 304);
		this.pnOption02.TabIndex = 178;
		this.label21.BackColor = System.Drawing.SystemColors.Highlight;
		this.label21.Location = new System.Drawing.Point(149, 50);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 1);
		this.label21.TabIndex = 242;
		this.label25.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label25.Location = new System.Drawing.Point(16, 235);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(279, 1);
		this.label25.TabIndex = 236;
		this.label19.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label19.Location = new System.Drawing.Point(16, 76);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(279, 1);
		this.label19.TabIndex = 240;
		this.label22.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label22.Location = new System.Drawing.Point(16, 29);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(279, 1);
		this.label22.TabIndex = 239;
		this.label23.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label23.Location = new System.Drawing.Point(16, 162);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(279, 1);
		this.label23.TabIndex = 238;
		this.label1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(16, 103);
		this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(279, 53);
		this.label1.TabIndex = 233;
		this.label1.Text = "Emissores, Tomadores,  \r\nRemetentes, Destinatários, \r\nRecebedores, Expedidores e \r\nTerceiros citados no XML\r\n";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(16, 82);
		this.label8.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(279, 17);
		this.label8.TabIndex = 232;
		this.label8.Text = "Deve ser utilizado por :";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbAddCreditToSign01.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lkbAddCreditToSign01.Location = new System.Drawing.Point(13, 194);
		this.lkbAddCreditToSign01.Name = "lkbAddCreditToSign01";
		this.lkbAddCreditToSign01.Size = new System.Drawing.Size(284, 13);
		this.lkbAddCreditToSign01.TabIndex = 236;
		this.lkbAddCreditToSign01.TabStop = true;
		this.lkbAddCreditToSign01.Text = "Assinar um plano com créditos?";
		this.lkbAddCreditToSign01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbAddCreditToSign01.Click += new System.EventHandler(lkbAddCreditToSign);
		this.lkbAddCreditAdHoc01.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lkbAddCreditAdHoc01.Location = new System.Drawing.Point(13, 214);
		this.lkbAddCreditAdHoc01.Name = "lkbAddCreditAdHoc01";
		this.lkbAddCreditAdHoc01.Size = new System.Drawing.Size(284, 13);
		this.lkbAddCreditAdHoc01.TabIndex = 237;
		this.lkbAddCreditAdHoc01.TabStop = true;
		this.lkbAddCreditAdHoc01.Text = "Adquirir créditos avulsos?";
		this.lkbAddCreditAdHoc01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbAddCreditAdHoc01.Click += new System.EventHandler(lkbAddCreditAdHoc);
		this.lbBalance.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbBalance.ForeColor = System.Drawing.Color.MidnightBlue;
		this.lbBalance.Location = new System.Drawing.Point(13, 167);
		this.lbBalance.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.lbBalance.Name = "lbBalance";
		this.lbBalance.Size = new System.Drawing.Size(284, 21);
		this.lbBalance.TabIndex = 233;
		this.lbBalance.Text = "Créditos disponíveis :";
		this.lbBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btDowOption02.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btDowOption02.Image = Monitor.Resources.image_down_button;
		this.btDowOption02.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btDowOption02.Location = new System.Drawing.Point(49, 249);
		this.btDowOption02.Margin = new System.Windows.Forms.Padding(2);
		this.btDowOption02.Name = "btDowOption02";
		this.btDowOption02.Size = new System.Drawing.Size(213, 36);
		this.btDowOption02.TabIndex = 225;
		this.btDowOption02.Text = "2: Baixar XMLs da SEFAZ";
		this.btDowOption02.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btDowOption02.UseVisualStyleBackColor = true;
		this.btDowOption02.Click += new System.EventHandler(btDowOption02_Click);
		this.label13.Font = new System.Drawing.Font("Verdana", 8f);
		this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label13.Location = new System.Drawing.Point(16, 55);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(279, 14);
		this.label13.TabIndex = 230;
		this.label13.Text = "XMLs de até 12 anos atrás";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.Font = new System.Drawing.Font("Verdana", 8f);
		this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label9.Location = new System.Drawing.Point(16, 35);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(279, 14);
		this.label9.TabIndex = 228;
		this.label9.Text = "NFe e CTe de Entrada e Saída";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(13, 5);
		this.label5.Margin = new System.Windows.Forms.Padding(0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(284, 19);
		this.label5.TabIndex = 227;
		this.label5.Text = "Download Especial";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(613, 353);
		base.Controls.Add(this.pnOption02);
		base.Controls.Add(this.pnOption01);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmDownOptionCTe";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - CTe : Opções de download";
		base.Load += new System.EventHandler(frmDownOptionCTe_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDownOptionCTe_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.pnOption01.ResumeLayout(false);
		this.pnOption02.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
