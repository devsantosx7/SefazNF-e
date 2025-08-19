using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmOnboardStep00 : Form
{
	private IContainer components;

	private Label label38;

	private Label label37;

	private Label label36;

	private ComboBox cbBusiness;

	private Label lbBusiness;

	private ComboBox cbPosition;

	private Label lbCompany;

	private TextBox txCompany;

	private Label lbPosition;

	private Label label18;

	private Label label17;

	private Label lbEmail;

	private TextBox txEmail;

	private Label lbName;

	private TextBox txName;

	private Label lbPhone;

	private TextBox txPhone;

	private LinkLabel linkLabel1;

	private Label label3;

	private Label label2;

	private Label label15;

	private Label label14;

	private Label label13;

	private Label label11;

	private Label label10;

	private Label label7;

	private Label label8;

	private Label label9;

	private Panel panel2;

	private Panel panel1;

	public frmOnboardStep00()
	{
		InitializeComponent();
	}

	public async void funcLoadDataAsync()
	{
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		txName.Text = varclsConfig.UserName;
		txName.Tag = varclsConfig.MauticId;
		txEmail.Text = varclsConfig.UserEmail;
		txEmail.Tag = varclsConfig.MauticId;
		txPhone.Text = varclsConfig.UserPhone;
		txPhone.Tag = varclsConfig.MauticId;
		txCompany.Text = varclsConfig.UserCompany;
		funcSetPosition(varclsConfig.UserPosition);
		funcSetBusiness(varclsConfig.UserBusiness);
	}

	private void funcSetPosition(string pPosition)
	{
		if (!clsFunction.IsEmpty(pPosition))
		{
			if (cbPosition.Items.Contains(pPosition))
			{
				cbPosition.Text = pPosition;
			}
			else
			{
				cbPosition.SelectedIndex = -1;
			}
		}
	}

	private void funcSetBusiness(string pBusiness)
	{
		if (!clsFunction.IsEmpty(pBusiness))
		{
			if (cbBusiness.Items.Contains(pBusiness))
			{
				cbBusiness.Text = pBusiness;
			}
			else
			{
				cbBusiness.SelectedIndex = -1;
			}
		}
	}

	public string funcGetUserName()
	{
		return txName.Text;
	}

	public string funcGetUserEmail()
	{
		return txEmail.Text;
	}

	public string funcGetUserPhone()
	{
		return txPhone.Text;
	}

	public string funcGetUserPosition()
	{
		return cbPosition.Text;
	}

	public string funcGetUserCompany()
	{
		return txCompany.Text;
	}

	public string funcGetUserBusiness()
	{
		return cbBusiness.Text;
	}

	public bool funcValidate()
	{
		string varMensagem = string.Empty;
		if (clsFunction.IsEmpty(txName.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Nome não foi preenchido !!!";
		}
		if (clsFunction.IsEmpty(txEmail.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O E-mail não foi preenchido !!!";
		}
		else if (!clsFunction.funcIsValidEmail(txEmail.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O E-mail informando é inválido!!!";
		}
		if (clsFunction.IsEmpty(txCompany.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> A empresa não foi preenchida !!!";
		}
		if (cbPosition.SelectedIndex == -1)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Cargo não foi informado !!!";
		}
		if (cbBusiness.SelectedIndex == -1)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> A área de atuação não foi informada !!!";
		}
		if (!clsFunction.IsEmpty(varMensagem))
		{
			MessageBox.Show("Confirme os dados antes de continuar" + varMensagem, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		return true;
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/versao-gratuita", "onboard");
		Cursor = Cursors.Default;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboardStep00));
		this.panel2 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.txName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.lbName = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txEmail = new System.Windows.Forms.TextBox();
		this.lbEmail = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.lbPosition = new System.Windows.Forms.Label();
		this.txCompany = new System.Windows.Forms.TextBox();
		this.lbCompany = new System.Windows.Forms.Label();
		this.cbPosition = new System.Windows.Forms.ComboBox();
		this.label7 = new System.Windows.Forms.Label();
		this.lbBusiness = new System.Windows.Forms.Label();
		this.cbBusiness = new System.Windows.Forms.ComboBox();
		this.label9 = new System.Windows.Forms.Label();
		this.lbPhone = new System.Windows.Forms.Label();
		this.txPhone = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.linkLabel1);
		this.panel2.Controls.Add(this.txName);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label15);
		this.panel2.Controls.Add(this.lbName);
		this.panel2.Controls.Add(this.label14);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.txEmail);
		this.panel2.Controls.Add(this.lbEmail);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.label17);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.lbPosition);
		this.panel2.Controls.Add(this.txCompany);
		this.panel2.Controls.Add(this.lbCompany);
		this.panel2.Controls.Add(this.cbPosition);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.lbBusiness);
		this.panel2.Controls.Add(this.cbBusiness);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.lbPhone);
		this.panel2.Controls.Add(this.txPhone);
		this.panel2.Controls.Add(this.label36);
		this.panel2.Controls.Add(this.label37);
		this.panel2.Controls.Add(this.label38);
		this.panel2.Location = new System.Drawing.Point(15, 15);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(653, 502);
		this.panel2.TabIndex = 198;
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label8.Location = new System.Drawing.Point(51, 185);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(453, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Por favor, confirme seus dados:";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.linkLabel1.AutoSize = true;
		this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel1.Location = new System.Drawing.Point(475, 123);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(70, 16);
		this.linkLabel1.TabIndex = 197;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "saber mais";
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.txName.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txName.Location = new System.Drawing.Point(134, 232);
		this.txName.MaxLength = 150;
		this.txName.Name = "txName";
		this.txName.Size = new System.Drawing.Size(485, 22);
		this.txName.TabIndex = 0;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(8, 468);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(632, 17);
		this.label3.TabIndex = 196;
		this.label3.Text = "Garantimos sigilo total das informações.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label15.Location = new System.Drawing.Point(7, 149);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(632, 17);
		this.label15.TabIndex = 193;
		this.label15.Text = "Em 3 passos o sistema ficará pronto para você usar.";
		this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lbName.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbName.Location = new System.Drawing.Point(10, 236);
		this.lbName.Name = "lbName";
		this.lbName.Size = new System.Drawing.Size(118, 14);
		this.lbName.TabIndex = 91;
		this.lbName.Text = "Nome :";
		this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label14.Location = new System.Drawing.Point(7, 104);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(632, 17);
		this.label14.TabIndex = 192;
		this.label14.Text = "Fique à vontade: esta versão é eternamente gratuita, nada lhe será cobrado.";
		this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(8, 444);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(632, 17);
		this.label2.TabIndex = 195;
		this.label2.Text = "A Fiscal.io está 100% de acordo com a LGPD.";
		this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label13.Location = new System.Drawing.Point(7, 66);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(632, 17);
		this.label13.TabIndex = 191;
		this.label13.Text = "resolver várias tarefas de gerenciamento de documentos fiscais.";
		this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txEmail.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txEmail.Location = new System.Drawing.Point(134, 261);
		this.txEmail.MaxLength = 150;
		this.txEmail.Name = "txEmail";
		this.txEmail.Size = new System.Drawing.Size(485, 22);
		this.txEmail.TabIndex = 1;
		this.lbEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbEmail.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbEmail.Location = new System.Drawing.Point(10, 265);
		this.lbEmail.Name = "lbEmail";
		this.lbEmail.Size = new System.Drawing.Size(118, 14);
		this.lbEmail.TabIndex = 92;
		this.lbEmail.Text = "E-mail :";
		this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label11.Location = new System.Drawing.Point(7, 44);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(632, 17);
		this.label11.TabIndex = 189;
		this.label11.Text = "Esta é a versão gratuita de um sistema premium que ajudará você a ";
		this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label10.Location = new System.Drawing.Point(7, 14);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(632, 14);
		this.label10.TabIndex = 188;
		this.label10.Text = "Olá,";
		this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label17.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(625, 236);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 14);
		this.label17.TabIndex = 94;
		this.label17.Text = "*";
		this.label18.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(625, 265);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 14);
		this.label18.TabIndex = 95;
		this.label18.Text = "*";
		this.lbPosition.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbPosition.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbPosition.Location = new System.Drawing.Point(10, 350);
		this.lbPosition.Name = "lbPosition";
		this.lbPosition.Size = new System.Drawing.Size(118, 14);
		this.lbPosition.TabIndex = 96;
		this.lbPosition.Text = "Cargo :";
		this.lbPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txCompany.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txCompany.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txCompany.Location = new System.Drawing.Point(134, 317);
		this.txCompany.MaxLength = 150;
		this.txCompany.Name = "txCompany";
		this.txCompany.Size = new System.Drawing.Size(485, 22);
		this.txCompany.TabIndex = 3;
		this.lbCompany.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbCompany.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbCompany.Location = new System.Drawing.Point(10, 321);
		this.lbCompany.Name = "lbCompany";
		this.lbCompany.Size = new System.Drawing.Size(118, 14);
		this.lbCompany.TabIndex = 97;
		this.lbCompany.Text = "Empresa :";
		this.lbCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.cbPosition.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.cbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbPosition.Font = new System.Drawing.Font("Tahoma", 9f);
		this.cbPosition.FormattingEnabled = true;
		this.cbPosition.Items.AddRange(new object[4] { "Sócio/Proprietário/Diretor", "Gerente/Coordenador", "Analista/Técnico", "Assistente" });
		this.cbPosition.Location = new System.Drawing.Point(134, 346);
		this.cbPosition.Name = "cbPosition";
		this.cbPosition.Size = new System.Drawing.Size(370, 22);
		this.cbPosition.TabIndex = 4;
		this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label7.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label7.Location = new System.Drawing.Point(43, 185);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(2, 23);
		this.label7.TabIndex = 187;
		this.lbBusiness.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbBusiness.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbBusiness.Location = new System.Drawing.Point(10, 380);
		this.lbBusiness.Name = "lbBusiness";
		this.lbBusiness.Size = new System.Drawing.Size(118, 14);
		this.lbBusiness.TabIndex = 98;
		this.lbBusiness.Text = "Ramo de atividade :";
		this.lbBusiness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.cbBusiness.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.cbBusiness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbBusiness.Font = new System.Drawing.Font("Tahoma", 9f);
		this.cbBusiness.FormattingEnabled = true;
		this.cbBusiness.Items.AddRange(new object[5] { "Serviços Contábeis/Tributários", "Indústria ou Distribuição", "Varejo", "Transporte e Logística", "TI ou Outros Serviços" });
		this.cbBusiness.Location = new System.Drawing.Point(134, 376);
		this.cbBusiness.Name = "cbBusiness";
		this.cbBusiness.Size = new System.Drawing.Size(370, 22);
		this.cbBusiness.TabIndex = 5;
		this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label9.Location = new System.Drawing.Point(16, 185);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(21, 23);
		this.label9.TabIndex = 185;
		this.label9.Text = "1";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbPhone.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbPhone.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbPhone.Location = new System.Drawing.Point(10, 292);
		this.lbPhone.Name = "lbPhone";
		this.lbPhone.Size = new System.Drawing.Size(118, 14);
		this.lbPhone.TabIndex = 112;
		this.lbPhone.Text = "Telefone :";
		this.lbPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txPhone.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txPhone.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txPhone.Location = new System.Drawing.Point(134, 289);
		this.txPhone.Name = "txPhone";
		this.txPhone.Size = new System.Drawing.Size(230, 22);
		this.txPhone.TabIndex = 2;
		this.label36.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(626, 321);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 14);
		this.label36.TabIndex = 99;
		this.label36.Text = "*";
		this.label37.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(626, 350);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 14);
		this.label37.TabIndex = 100;
		this.label37.Text = "*";
		this.label38.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(626, 380);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 14);
		this.label38.TabIndex = 101;
		this.label38.Text = "*";
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(684, 534);
		this.panel1.TabIndex = 83;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(684, 534);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnboardStep00";
		this.Text = "frmOnboardStep00";
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
