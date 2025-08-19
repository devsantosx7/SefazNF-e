using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmBalanceConfirm04 : Form
{
	private ObjectMeasure varclsObjMeasure = new ObjectMeasure();

	private clsBalanceService.BalanceModel varclsBalance = new clsBalanceService.BalanceModel();

	private IContainer components;

	private Label lbTitle;

	private Label lbUserInfo01;

	private Label lbUserInfo02;

	private Panel panel1;

	private Panel panel2;

	private ListView lstData;

	private ColumnHeader colMeasure;

	private ColumnHeader colValue;

	private Label label3;

	private ColumnHeader colSignal;

	private LinkLabel lkbLicense;

	private Label lbMeasure;

	private Button btCancelar;

	private Label label2;

	private LinkLabel lkbSupportChat;

	private PictureBox picSupportChat;

	private Panel panel3;

	private LinkLabel lkbAddCreditToSign02;

	private LinkLabel lkbAddCreditToSign01;

	private PictureBox pickAddCreditToSign;

	private LinkLabel lkbAddCreditAdHoc02;

	private LinkLabel lkbAddCreditAdHoc01;

	private PictureBox picAddCreditAdHoc;

	private ColumnHeader colUnit;

	public frmBalanceConfirm04(ObjectMeasure clsObjMeasure, clsBalanceService.BalanceModel pclsBalance)
	{
		InitializeComponent();
		varclsObjMeasure = clsObjMeasure;
		varclsBalance = pclsBalance;
	}

	private async void frmBalanceConfirm04_Load(object sender, EventArgs e)
	{
		lbMeasure.Text = varclsObjMeasure.MeaDesc;
		lstData = new clsBalanceHandler().funcFillListView(lstData, varclsBalance);
		if (await new clsSoftwareService(null).funcHasPayedPlanAsync())
		{
			lkbAddCreditToSign01.Text = "Adicionar creditos a assinatura";
			LinkLabel linkLabel = lkbAddCreditToSign01;
			object tag = (lkbAddCreditToSign02.Tag = "PAYED");
			linkLabel.Tag = tag;
		}
		else
		{
			lkbAddCreditToSign01.Text = "Fazer assinatura e adquirir créditos";
			LinkLabel linkLabel2 = lkbAddCreditToSign01;
			object tag = (lkbAddCreditToSign02.Tag = "FREE");
			linkLabel2.Tag = tag;
		}
	}

	private void lkbLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		frmLicense frmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
		frmLicense.ShowDialog(this);
		frmLicense.Dispose();
	}

	private void btCancelar_Click(object sender, EventArgs e)
	{
		Close();
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

	private void lkbSupportChat_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallChatWebPageAsync("SPECIAL_CREDIT");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmBalanceConfirm04));
		this.lbTitle = new System.Windows.Forms.Label();
		this.lbUserInfo01 = new System.Windows.Forms.Label();
		this.lbUserInfo02 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lkbLicense = new System.Windows.Forms.LinkLabel();
		this.lstData = new System.Windows.Forms.ListView();
		this.colMeasure = new System.Windows.Forms.ColumnHeader();
		this.colUnit = new System.Windows.Forms.ColumnHeader();
		this.colValue = new System.Windows.Forms.ColumnHeader();
		this.colSignal = new System.Windows.Forms.ColumnHeader();
		this.label3 = new System.Windows.Forms.Label();
		this.lbMeasure = new System.Windows.Forms.Label();
		this.btCancelar = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.lkbSupportChat = new System.Windows.Forms.LinkLabel();
		this.picSupportChat = new System.Windows.Forms.PictureBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lkbAddCreditToSign02 = new System.Windows.Forms.LinkLabel();
		this.lkbAddCreditToSign01 = new System.Windows.Forms.LinkLabel();
		this.pickAddCreditToSign = new System.Windows.Forms.PictureBox();
		this.lkbAddCreditAdHoc02 = new System.Windows.Forms.LinkLabel();
		this.lkbAddCreditAdHoc01 = new System.Windows.Forms.LinkLabel();
		this.picAddCreditAdHoc = new System.Windows.Forms.PictureBox();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picSupportChat).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pickAddCreditToSign).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picAddCreditAdHoc).BeginInit();
		base.SuspendLayout();
		this.lbTitle.AutoSize = true;
		this.lbTitle.Location = new System.Drawing.Point(12, 8);
		this.lbTitle.Name = "lbTitle";
		this.lbTitle.Size = new System.Drawing.Size(0, 13);
		this.lbTitle.TabIndex = 0;
		this.lbUserInfo01.AutoSize = true;
		this.lbUserInfo01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbUserInfo01.ForeColor = System.Drawing.Color.Red;
		this.lbUserInfo01.Location = new System.Drawing.Point(4, 7);
		this.lbUserInfo01.Name = "lbUserInfo01";
		this.lbUserInfo01.Size = new System.Drawing.Size(433, 14);
		this.lbUserInfo01.TabIndex = 1;
		this.lbUserInfo01.Text = "Você não tem créditos especiais suficientes para realizar esta ação.";
		this.lbUserInfo02.AutoSize = true;
		this.lbUserInfo02.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbUserInfo02.Location = new System.Drawing.Point(4, 29);
		this.lbUserInfo02.Name = "lbUserInfo02";
		this.lbUserInfo02.Size = new System.Drawing.Size(470, 14);
		this.lbUserInfo02.TabIndex = 2;
		this.lbUserInfo02.Text = "Para continuar, você precisa realizar um dos seguintes passos abaixo:";
		this.panel1.BackColor = System.Drawing.SystemColors.Info;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.lbUserInfo02);
		this.panel1.Controls.Add(this.lbUserInfo01);
		this.panel1.Controls.Add(this.lbTitle);
		this.panel1.Location = new System.Drawing.Point(5, 5);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(565, 54);
		this.panel1.TabIndex = 173;
		this.panel2.BackColor = System.Drawing.SystemColors.Window;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.lkbLicense);
		this.panel2.Controls.Add(this.lstData);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.lbMeasure);
		this.panel2.Location = new System.Drawing.Point(5, 122);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(565, 144);
		this.panel2.TabIndex = 178;
		this.lkbLicense.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lkbLicense.Location = new System.Drawing.Point(414, 9);
		this.lkbLicense.Name = "lkbLicense";
		this.lkbLicense.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lkbLicense.Size = new System.Drawing.Size(138, 13);
		this.lkbLicense.TabIndex = 179;
		this.lkbLicense.TabStop = true;
		this.lkbLicense.Text = "Mais detalhes ...";
		this.lkbLicense.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lkbLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbLicense_LinkClicked);
		this.lstData.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lstData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.colMeasure, this.colUnit, this.colValue, this.colSignal });
		this.lstData.Font = new System.Drawing.Font("Verdana", 9f);
		this.lstData.FullRowSelect = true;
		this.lstData.HideSelection = false;
		this.lstData.Location = new System.Drawing.Point(5, 34);
		this.lstData.MultiSelect = false;
		this.lstData.Name = "lstData";
		this.lstData.Size = new System.Drawing.Size(546, 103);
		this.lstData.TabIndex = 178;
		this.lstData.UseCompatibleStateImageBehavior = false;
		this.lstData.View = System.Windows.Forms.View.Details;
		this.colMeasure.Text = "Item";
		this.colMeasure.Width = 310;
		this.colUnit.Text = "Un";
		this.colUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.colUnit.Width = 40;
		this.colValue.Text = "Quantidade";
		this.colValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.colValue.Width = 140;
		this.colSignal.Text = "";
		this.colSignal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.colSignal.Width = 30;
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(9, 29);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(543, 2);
		this.label3.TabIndex = 177;
		this.lbMeasure.AutoSize = true;
		this.lbMeasure.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbMeasure.ForeColor = System.Drawing.Color.Blue;
		this.lbMeasure.Location = new System.Drawing.Point(6, 8);
		this.lbMeasure.Name = "lbMeasure";
		this.lbMeasure.Size = new System.Drawing.Size(223, 14);
		this.lbMeasure.TabIndex = 3;
		this.lbMeasure.Text = "Downloads e consultas especiais";
		this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btCancelar.Location = new System.Drawing.Point(483, 280);
		this.btCancelar.Name = "btCancelar";
		this.btCancelar.Size = new System.Drawing.Size(87, 34);
		this.btCancelar.TabIndex = 182;
		this.btCancelar.Text = "&Cancelar";
		this.btCancelar.UseVisualStyleBackColor = true;
		this.btCancelar.Click += new System.EventHandler(btCancelar_Click);
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(8, 272);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(561, 2);
		this.label2.TabIndex = 180;
		this.lkbSupportChat.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lkbSupportChat.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lkbSupportChat.Location = new System.Drawing.Point(45, 284);
		this.lkbSupportChat.Name = "lkbSupportChat";
		this.lkbSupportChat.Size = new System.Drawing.Size(310, 29);
		this.lkbSupportChat.TabIndex = 185;
		this.lkbSupportChat.TabStop = true;
		this.lkbSupportChat.Text = "Iniciar bate papo com o time da Fiscal.io";
		this.lkbSupportChat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbSupportChat.Click += new System.EventHandler(lkbSupportChat_Click);
		this.picSupportChat.Image = Monitor.Resources.image_support;
		this.picSupportChat.Location = new System.Drawing.Point(8, 283);
		this.picSupportChat.Name = "picSupportChat";
		this.picSupportChat.Size = new System.Drawing.Size(30, 29);
		this.picSupportChat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picSupportChat.TabIndex = 184;
		this.picSupportChat.TabStop = false;
		this.panel3.BackColor = System.Drawing.SystemColors.Window;
		this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel3.Controls.Add(this.lkbAddCreditToSign02);
		this.panel3.Controls.Add(this.lkbAddCreditToSign01);
		this.panel3.Controls.Add(this.pickAddCreditToSign);
		this.panel3.Controls.Add(this.lkbAddCreditAdHoc02);
		this.panel3.Controls.Add(this.lkbAddCreditAdHoc01);
		this.panel3.Controls.Add(this.picAddCreditAdHoc);
		this.panel3.Location = new System.Drawing.Point(5, 62);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(565, 56);
		this.panel3.TabIndex = 186;
		this.lkbAddCreditToSign02.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lkbAddCreditToSign02.Font = new System.Drawing.Font("Verdana", 9f);
		this.lkbAddCreditToSign02.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
		this.lkbAddCreditToSign02.Location = new System.Drawing.Point(444, 6);
		this.lkbAddCreditToSign02.Name = "lkbAddCreditToSign02";
		this.lkbAddCreditToSign02.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lkbAddCreditToSign02.Size = new System.Drawing.Size(108, 16);
		this.lkbAddCreditToSign02.TabIndex = 187;
		this.lkbAddCreditToSign02.TabStop = true;
		this.lkbAddCreditToSign02.Text = "Clique aqui";
		this.lkbAddCreditToSign02.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lkbAddCreditToSign02.Click += new System.EventHandler(lkbAddCreditToSign);
		this.lkbAddCreditToSign01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lkbAddCreditToSign01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lkbAddCreditToSign01.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lkbAddCreditToSign01.Location = new System.Drawing.Point(29, 6);
		this.lkbAddCreditToSign01.Name = "lkbAddCreditToSign01";
		this.lkbAddCreditToSign01.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lkbAddCreditToSign01.Size = new System.Drawing.Size(409, 16);
		this.lkbAddCreditToSign01.TabIndex = 186;
		this.lkbAddCreditToSign01.TabStop = true;
		this.lkbAddCreditToSign01.Text = "Adicionar créditos no plano atual";
		this.lkbAddCreditToSign01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbAddCreditToSign01.Click += new System.EventHandler(lkbAddCreditToSign);
		this.pickAddCreditToSign.Image = Monitor.Resources.image_number_one;
		this.pickAddCreditToSign.Location = new System.Drawing.Point(7, 6);
		this.pickAddCreditToSign.Name = "pickAddCreditToSign";
		this.pickAddCreditToSign.Size = new System.Drawing.Size(16, 16);
		this.pickAddCreditToSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pickAddCreditToSign.TabIndex = 185;
		this.pickAddCreditToSign.TabStop = false;
		this.lkbAddCreditAdHoc02.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lkbAddCreditAdHoc02.Font = new System.Drawing.Font("Verdana", 9f);
		this.lkbAddCreditAdHoc02.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
		this.lkbAddCreditAdHoc02.Location = new System.Drawing.Point(444, 30);
		this.lkbAddCreditAdHoc02.Name = "lkbAddCreditAdHoc02";
		this.lkbAddCreditAdHoc02.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lkbAddCreditAdHoc02.Size = new System.Drawing.Size(108, 16);
		this.lkbAddCreditAdHoc02.TabIndex = 184;
		this.lkbAddCreditAdHoc02.TabStop = true;
		this.lkbAddCreditAdHoc02.Text = "Clique aqui";
		this.lkbAddCreditAdHoc02.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lkbAddCreditAdHoc02.Click += new System.EventHandler(lkbAddCreditAdHoc);
		this.lkbAddCreditAdHoc01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lkbAddCreditAdHoc01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lkbAddCreditAdHoc01.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lkbAddCreditAdHoc01.Location = new System.Drawing.Point(29, 30);
		this.lkbAddCreditAdHoc01.Name = "lkbAddCreditAdHoc01";
		this.lkbAddCreditAdHoc01.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lkbAddCreditAdHoc01.Size = new System.Drawing.Size(409, 16);
		this.lkbAddCreditAdHoc01.TabIndex = 182;
		this.lkbAddCreditAdHoc01.TabStop = true;
		this.lkbAddCreditAdHoc01.Text = "Adquirir créditos avulsos";
		this.lkbAddCreditAdHoc01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbAddCreditAdHoc01.Click += new System.EventHandler(lkbAddCreditAdHoc);
		this.picAddCreditAdHoc.Image = Monitor.Resources.image_number_two;
		this.picAddCreditAdHoc.Location = new System.Drawing.Point(7, 30);
		this.picAddCreditAdHoc.Name = "picAddCreditAdHoc";
		this.picAddCreditAdHoc.Size = new System.Drawing.Size(16, 16);
		this.picAddCreditAdHoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picAddCreditAdHoc.TabIndex = 181;
		this.picAddCreditAdHoc.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btCancelar;
		base.ClientSize = new System.Drawing.Size(575, 320);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.lkbSupportChat);
		base.Controls.Add(this.picSupportChat);
		base.Controls.Add(this.btCancelar);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmBalanceConfirm04";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Saldo indisponível para créditos especiais";
		base.Load += new System.EventHandler(frmBalanceConfirm04_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picSupportChat).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pickAddCreditToSign).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picAddCreditAdHoc).EndInit();
		base.ResumeLayout(false);
	}
}
