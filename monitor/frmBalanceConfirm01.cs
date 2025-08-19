using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmBalanceConfirm01 : Form
{
	private ObjectMeasure varclsObjMeasure = new ObjectMeasure();

	private clsBalanceService.BalanceModel varclsBalance = new clsBalanceService.BalanceModel();

	private clsDataParameter varclsDataParameter = new clsDataParameter();

	private decimal varTotalCost;

	private IContainer components;

	private Button btConfirm;

	private Label lbTitle;

	private Label lbUserInfo01;

	private Label lbUserInfo02;

	private Panel panel1;

	private Panel panel2;

	private ListView lstData;

	private ColumnHeader colMeasure;

	private ColumnHeader colValue;

	private Label label3;

	private Label label2;

	private CheckBox ckbBalcNotShowUserDec;

	private Button btCancelar;

	private ColumnHeader colSignal;

	private LinkLabel lkbLicense;

	private Label lbMeasure;

	private ColumnHeader colUnit;

	public frmBalanceConfirm01(ObjectMeasure clsObjMeasure, clsBalanceService.BalanceModel pclsBalance, decimal pTotalCost)
	{
		InitializeComponent();
		varclsObjMeasure = clsObjMeasure;
		varclsBalance = pclsBalance;
		varTotalCost = pTotalCost;
	}

	private async void frmBalanceConfirm01_Load(object sender, EventArgs e)
	{
		string varUserDecision = await varclsDataParameter.funcGetAsync("BalanceNotShowUserDec");
		ckbBalcNotShowUserDec.Checked = clsFunction.funcConvStrToBool(varUserDecision);
		lbMeasure.Text = varclsObjMeasure.MeaDesc;
		lstData = new clsBalanceHandler().funcFillListView(lstData, varclsBalance, varTotalCost);
	}

	private void lkbLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		frmLicense frmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
		frmLicense.ShowDialog(this);
		frmLicense.Dispose();
	}

	private void btConfirm_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btCancelar_Click(object sender, EventArgs e)
	{
		Close();
	}

	private async void ckbBalcNotShowUserDec_CheckedChanged(object sender, EventArgs e)
	{
		if (ckbBalcNotShowUserDec.Checked)
		{
			await varclsDataParameter.funcSetAsync("BalanceNotShowUserDec", "X");
		}
		else
		{
			await varclsDataParameter.funcSetAsync("BalanceNotShowUserDec", string.Empty);
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmBalanceConfirm01));
		this.btConfirm = new System.Windows.Forms.Button();
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
		this.label2 = new System.Windows.Forms.Label();
		this.ckbBalcNotShowUserDec = new System.Windows.Forms.CheckBox();
		this.btCancelar = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.btConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btConfirm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btConfirm.Location = new System.Drawing.Point(473, 210);
		this.btConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btConfirm.Name = "btConfirm";
		this.btConfirm.Size = new System.Drawing.Size(97, 34);
		this.btConfirm.TabIndex = 174;
		this.btConfirm.Text = "&Confirmar";
		this.btConfirm.UseVisualStyleBackColor = true;
		this.btConfirm.Click += new System.EventHandler(btConfirm_Click);
		this.lbTitle.AutoSize = true;
		this.lbTitle.Location = new System.Drawing.Point(12, 8);
		this.lbTitle.Name = "lbTitle";
		this.lbTitle.Size = new System.Drawing.Size(0, 13);
		this.lbTitle.TabIndex = 0;
		this.lbUserInfo01.AutoSize = true;
		this.lbUserInfo01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbUserInfo01.Location = new System.Drawing.Point(4, 7);
		this.lbUserInfo01.Name = "lbUserInfo01";
		this.lbUserInfo01.Size = new System.Drawing.Size(408, 14);
		this.lbUserInfo01.TabIndex = 1;
		this.lbUserInfo01.Text = "Você selecionou uma ação que utilizará seus créditos especiais.";
		this.lbUserInfo02.AutoSize = true;
		this.lbUserInfo02.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbUserInfo02.Location = new System.Drawing.Point(4, 29);
		this.lbUserInfo02.Name = "lbUserInfo02";
		this.lbUserInfo02.Size = new System.Drawing.Size(268, 14);
		this.lbUserInfo02.TabIndex = 2;
		this.lbUserInfo02.Text = "Por favor, confirme antes de continuar?";
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
		this.panel2.Location = new System.Drawing.Point(5, 58);
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
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(6, 251);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(561, 2);
		this.label2.TabIndex = 180;
		this.ckbBalcNotShowUserDec.AutoSize = true;
		this.ckbBalcNotShowUserDec.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ckbBalcNotShowUserDec.Location = new System.Drawing.Point(6, 258);
		this.ckbBalcNotShowUserDec.Name = "ckbBalcNotShowUserDec";
		this.ckbBalcNotShowUserDec.Size = new System.Drawing.Size(378, 17);
		this.ckbBalcNotShowUserDec.TabIndex = 181;
		this.ckbBalcNotShowUserDec.Text = "Memorizar minha decisão e não mostrar esta tela novamente";
		this.ckbBalcNotShowUserDec.UseVisualStyleBackColor = true;
		this.ckbBalcNotShowUserDec.CheckedChanged += new System.EventHandler(ckbBalcNotShowUserDec_CheckedChanged);
		this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btCancelar.Location = new System.Drawing.Point(5, 209);
		this.btCancelar.Name = "btCancelar";
		this.btCancelar.Size = new System.Drawing.Size(87, 34);
		this.btCancelar.TabIndex = 182;
		this.btCancelar.Text = "&Cancelar";
		this.btCancelar.UseVisualStyleBackColor = true;
		this.btCancelar.Click += new System.EventHandler(btCancelar_Click);
		base.AcceptButton = this.btConfirm;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btCancelar;
		base.ClientSize = new System.Drawing.Size(577, 279);
		base.Controls.Add(this.btCancelar);
		base.Controls.Add(this.ckbBalcNotShowUserDec);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.btConfirm);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmBalanceConfirm01";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Confirme a utilização de créditos especiais";
		base.Load += new System.EventHandler(frmBalanceConfirm01_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
