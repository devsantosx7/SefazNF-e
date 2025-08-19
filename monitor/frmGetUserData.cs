using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmGetUserData : Form
{
	private clsDataParameter _clsDataParam = new clsDataParameter();

	private IContainer components;

	private Panel pnlContent;

	private Button btnConfirm;

	private Panel panel2;

	private Label label8;

	private Label label3;

	private Label label2;

	private Label label11;

	private ComboBox cbUserIntent;

	private Label label5;

	private Label label6;

	private Label label10;

	private Label label7;

	private Label label9;

	private CheckBox checkBox6;

	private CheckBox checkBox5;

	private CheckBox checkBox4;

	private CheckBox checkBox3;

	private CheckBox checkBox2;

	private CheckBox checkBox1;

	private Label label4;

	private Label label12;

	private Label label13;

	private Label label1;

	private TextBox txName;

	private Label lbName;

	private TextBox txEmail;

	private Label lbEmail;

	private Label label17;

	private Label label18;

	private Label lbPosition;

	private TextBox txCompany;

	private Label lbCompany;

	private ComboBox cbPosition;

	private Label lbBusiness;

	private ComboBox cbBusiness;

	private Label lbPhone;

	private TextBox txPhone;

	private Label label36;

	private Label label37;

	private Label label38;

	public frmGetUserData()
	{
		InitializeComponent();
	}

	private async void frmGetUserData_Load(object sender, EventArgs e)
	{
		await funcLoadDataAsync();
	}

	public async Task<bool> funcLoadDataAsync()
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
		funcSetIntent(await _clsDataParam.funcGetAsync("FIRST_INTENTION"));
		return true;
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

	private void funcSetIntent(string pUserIntent)
	{
		if (!clsFunction.IsEmpty(pUserIntent))
		{
			if (cbUserIntent.Items.Contains(pUserIntent))
			{
				cbUserIntent.Text = pUserIntent;
			}
			else
			{
				cbUserIntent.SelectedIndex = -1;
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

	public string funcGetUserIntent()
	{
		return cbUserIntent.Text;
	}

	public bool funcValidate()
	{
		string varMensagem = string.Empty;
		if (clsFunction.IsEmpty(txName.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Nome não foi preenchido !!!";
		}
		if (txName.Text.Length > 150)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Nome não pode exceder 150 caracteres !!!";
		}
		if (clsFunction.IsEmpty(txEmail.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O E-mail não foi preenchido !!!";
		}
		if (txEmail.Text.Length > 150)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O E-mail não pode exceder 150 caracteres !!!";
		}
		if (!clsFunction.funcIsValidEmail(txEmail.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O E-mail informando é inválido!!!";
		}
		if (clsFunction.IsEmpty(txPhone.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Telefone não foi preenchido !!!";
		}
		if (clsFunction.IsEmpty(txCompany.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> A empresa não foi preenchida !!!";
		}
		if (txCompany.Text.Length > 150)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> A empresa não pode exceder 150 caracteres !!!";
		}
		if (cbPosition.SelectedIndex == -1)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Cargo não foi informado !!!";
		}
		if (cbBusiness.SelectedIndex == -1)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> A área de atuação não foi informada !!!";
		}
		if (cbUserIntent.SelectedIndex == -1)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O objetivo de uso não foi informado !!!";
		}
		if (!clsFunction.IsEmpty(varMensagem))
		{
			MessageBox.Show("Confirme os dados antes de continuar" + varMensagem, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		return true;
	}

	private async Task<bool> funcSaveConfigDataAsync()
	{
		bool varSaveData = false;
		string varUserName = funcGetUserName();
		string varUserEmail = funcGetUserEmail();
		string varUserPhone = funcGetUserPhone();
		string varUserPosition = funcGetUserPosition();
		string varUserCompany = funcGetUserCompany();
		string varUserBusiness = funcGetUserBusiness();
		string varUserIntent = funcGetUserIntent();
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		if (!clsFunction.IsEmpty(varUserName) && !varUserName.Equals(varclsConfig.UserName))
		{
			varclsConfig.UserName = varUserName;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserEmail) && !varUserEmail.Equals(varclsConfig.UserEmail))
		{
			varclsConfig.UserEmail = varUserEmail;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserPhone) && !varUserEmail.Equals(varclsConfig.UserPhone))
		{
			varclsConfig.UserPhone = varUserPhone;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserCompany) && !varUserCompany.Equals(varclsConfig.UserCompany))
		{
			varclsConfig.UserCompany = varUserCompany;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserPosition) && !varUserPosition.Equals(varclsConfig.UserPosition))
		{
			varclsConfig.UserPosition = varUserPosition;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserBusiness) && !varUserBusiness.Equals(varclsConfig.UserBusiness))
		{
			varclsConfig.UserBusiness = varUserBusiness;
			varSaveData = true;
		}
		string varDbaUserIntent = await _clsDataParam.funcGetAsync("FIRST_INTENTION");
		if (!clsFunction.IsEmpty(varUserIntent) && !varUserIntent.Equals(varDbaUserIntent))
		{
			varSaveData = true;
		}
		if (!varSaveData)
		{
			return true;
		}
		varclsConfig.DateModified = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
		await new clsDataConfig().funcUpdateAsync(varclsConfig);
		await _clsDataParam.funcSetAsync("FIRST_INTENTION", varUserIntent);
		clsDataParameter clsDataParameter = new clsDataParameter();
		string varLastUpdate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
		await clsDataParameter.funcSetAsync("LAST-USER-DATA-UPDATE", varLastUpdate);
		return true;
	}

	private void frmGetUserData_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btnConfirm_Click(this, e);
		}
	}

	private void frmGetUserData_SizeChanged(object sender, EventArgs e)
	{
		if (!base.WindowState.Equals(FormWindowState.Maximized))
		{
			base.WindowState = FormWindowState.Maximized;
		}
	}

	private async void btnConfirm_Click(object sender, EventArgs e)
	{
		if (funcValidate())
		{
			await funcSaveConfigDataAsync();
			Close();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmGetUserData));
		this.btnConfirm = new System.Windows.Forms.Button();
		this.pnlContent = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.txName = new System.Windows.Forms.TextBox();
		this.lbName = new System.Windows.Forms.Label();
		this.txEmail = new System.Windows.Forms.TextBox();
		this.lbEmail = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.lbPosition = new System.Windows.Forms.Label();
		this.txCompany = new System.Windows.Forms.TextBox();
		this.lbCompany = new System.Windows.Forms.Label();
		this.cbPosition = new System.Windows.Forms.ComboBox();
		this.lbBusiness = new System.Windows.Forms.Label();
		this.cbBusiness = new System.Windows.Forms.ComboBox();
		this.lbPhone = new System.Windows.Forms.Label();
		this.txPhone = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.checkBox6 = new System.Windows.Forms.CheckBox();
		this.checkBox5 = new System.Windows.Forms.CheckBox();
		this.checkBox4 = new System.Windows.Forms.CheckBox();
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.checkBox2 = new System.Windows.Forms.CheckBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cbUserIntent = new System.Windows.Forms.ComboBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.pnlContent.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnConfirm.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.ForeColor = System.Drawing.Color.White;
		this.btnConfirm.Location = new System.Drawing.Point(605, 544);
		this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(164, 31);
		this.btnConfirm.TabIndex = 13;
		this.btnConfirm.Text = "Confirmar";
		this.btnConfirm.UseVisualStyleBackColor = false;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.pnlContent.BackColor = System.Drawing.Color.White;
		this.pnlContent.Controls.Add(this.panel2);
		this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlContent.Location = new System.Drawing.Point(0, 0);
		this.pnlContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.pnlContent.Name = "pnlContent";
		this.pnlContent.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
		this.pnlContent.Size = new System.Drawing.Size(776, 581);
		this.pnlContent.TabIndex = 88;
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.txName);
		this.panel2.Controls.Add(this.lbName);
		this.panel2.Controls.Add(this.txEmail);
		this.panel2.Controls.Add(this.lbEmail);
		this.panel2.Controls.Add(this.label17);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.lbPosition);
		this.panel2.Controls.Add(this.txCompany);
		this.panel2.Controls.Add(this.lbCompany);
		this.panel2.Controls.Add(this.cbPosition);
		this.panel2.Controls.Add(this.lbBusiness);
		this.panel2.Controls.Add(this.cbBusiness);
		this.panel2.Controls.Add(this.lbPhone);
		this.panel2.Controls.Add(this.txPhone);
		this.panel2.Controls.Add(this.label36);
		this.panel2.Controls.Add(this.label37);
		this.panel2.Controls.Add(this.label38);
		this.panel2.Controls.Add(this.checkBox6);
		this.panel2.Controls.Add(this.checkBox5);
		this.panel2.Controls.Add(this.checkBox4);
		this.panel2.Controls.Add(this.checkBox3);
		this.panel2.Controls.Add(this.checkBox2);
		this.panel2.Controls.Add(this.checkBox1);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.cbUserIntent);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Location = new System.Drawing.Point(12, 1);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(752, 537);
		this.panel2.TabIndex = 199;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(675, 329);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 14);
		this.label1.TabIndex = 245;
		this.label1.Text = "*";
		this.txName.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txName.Location = new System.Drawing.Point(184, 110);
		this.txName.MaxLength = 150;
		this.txName.Name = "txName";
		this.txName.Size = new System.Drawing.Size(485, 22);
		this.txName.TabIndex = 0;
		this.lbName.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbName.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbName.Location = new System.Drawing.Point(60, 114);
		this.lbName.Name = "lbName";
		this.lbName.Size = new System.Drawing.Size(118, 14);
		this.lbName.TabIndex = 234;
		this.lbName.Text = "Nome :";
		this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txEmail.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txEmail.Location = new System.Drawing.Point(184, 139);
		this.txEmail.MaxLength = 150;
		this.txEmail.Name = "txEmail";
		this.txEmail.Size = new System.Drawing.Size(485, 22);
		this.txEmail.TabIndex = 1;
		this.lbEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbEmail.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbEmail.Location = new System.Drawing.Point(60, 143);
		this.lbEmail.Name = "lbEmail";
		this.lbEmail.Size = new System.Drawing.Size(118, 14);
		this.lbEmail.TabIndex = 235;
		this.lbEmail.Text = "E-mail :";
		this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label17.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(675, 114);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 14);
		this.label17.TabIndex = 236;
		this.label17.Text = "*";
		this.label18.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(675, 143);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 14);
		this.label18.TabIndex = 237;
		this.label18.Text = "*";
		this.lbPosition.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbPosition.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbPosition.Location = new System.Drawing.Point(60, 228);
		this.lbPosition.Name = "lbPosition";
		this.lbPosition.Size = new System.Drawing.Size(118, 14);
		this.lbPosition.TabIndex = 238;
		this.lbPosition.Text = "Cargo :";
		this.lbPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txCompany.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txCompany.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txCompany.Location = new System.Drawing.Point(184, 195);
		this.txCompany.MaxLength = 150;
		this.txCompany.Name = "txCompany";
		this.txCompany.Size = new System.Drawing.Size(485, 22);
		this.txCompany.TabIndex = 3;
		this.lbCompany.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbCompany.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbCompany.Location = new System.Drawing.Point(60, 199);
		this.lbCompany.Name = "lbCompany";
		this.lbCompany.Size = new System.Drawing.Size(118, 14);
		this.lbCompany.TabIndex = 239;
		this.lbCompany.Text = "Empresa :";
		this.lbCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.cbPosition.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.cbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbPosition.Font = new System.Drawing.Font("Tahoma", 9f);
		this.cbPosition.FormattingEnabled = true;
		this.cbPosition.Items.AddRange(new object[4] { "Sócio/Proprietário/Diretor", "Gerente/Coordenador", "Analista/Técnico", "Assistente" });
		this.cbPosition.Location = new System.Drawing.Point(184, 224);
		this.cbPosition.Name = "cbPosition";
		this.cbPosition.Size = new System.Drawing.Size(370, 22);
		this.cbPosition.TabIndex = 4;
		this.lbBusiness.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbBusiness.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbBusiness.Location = new System.Drawing.Point(60, 258);
		this.lbBusiness.Name = "lbBusiness";
		this.lbBusiness.Size = new System.Drawing.Size(118, 14);
		this.lbBusiness.TabIndex = 240;
		this.lbBusiness.Text = "Ramo de atividade :";
		this.lbBusiness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.cbBusiness.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.cbBusiness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbBusiness.Font = new System.Drawing.Font("Tahoma", 9f);
		this.cbBusiness.FormattingEnabled = true;
		this.cbBusiness.Items.AddRange(new object[5] { "Serviços Contábeis/Tributários", "Indústria ou Distribuição", "Varejo", "Transporte e Logística", "TI ou Outros Serviços" });
		this.cbBusiness.Location = new System.Drawing.Point(184, 254);
		this.cbBusiness.Name = "cbBusiness";
		this.cbBusiness.Size = new System.Drawing.Size(370, 22);
		this.cbBusiness.TabIndex = 5;
		this.lbPhone.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbPhone.Font = new System.Drawing.Font("Tahoma", 9f);
		this.lbPhone.Location = new System.Drawing.Point(60, 170);
		this.lbPhone.Name = "lbPhone";
		this.lbPhone.Size = new System.Drawing.Size(118, 14);
		this.lbPhone.TabIndex = 244;
		this.lbPhone.Text = "Telefone :";
		this.lbPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txPhone.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.txPhone.Font = new System.Drawing.Font("Tahoma", 9f);
		this.txPhone.Location = new System.Drawing.Point(184, 167);
		this.txPhone.Name = "txPhone";
		this.txPhone.Size = new System.Drawing.Size(230, 22);
		this.txPhone.TabIndex = 2;
		this.label36.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(676, 199);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 14);
		this.label36.TabIndex = 241;
		this.label36.Text = "*";
		this.label37.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(676, 228);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 14);
		this.label37.TabIndex = 242;
		this.label37.Text = "*";
		this.label38.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Tahoma", 9f);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(676, 258);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 14);
		this.label38.TabIndex = 243;
		this.label38.Text = "*";
		this.checkBox6.AutoSize = true;
		this.checkBox6.Location = new System.Drawing.Point(406, 445);
		this.checkBox6.Name = "checkBox6";
		this.checkBox6.Size = new System.Drawing.Size(281, 18);
		this.checkBox6.TabIndex = 12;
		this.checkBox6.Text = "Classificação automática de documentos";
		this.checkBox6.UseVisualStyleBackColor = true;
		this.checkBox5.AutoSize = true;
		this.checkBox5.Location = new System.Drawing.Point(130, 395);
		this.checkBox5.Name = "checkBox5";
		this.checkBox5.Size = new System.Drawing.Size(258, 18);
		this.checkBox5.TabIndex = 7;
		this.checkBox5.Text = "Baixa e conversão de XMLs para PDF";
		this.checkBox5.UseVisualStyleBackColor = true;
		this.checkBox4.AutoSize = true;
		this.checkBox4.Location = new System.Drawing.Point(406, 420);
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.Size = new System.Drawing.Size(186, 18);
		this.checkBox4.TabIndex = 11;
		this.checkBox4.Text = "Integração com Siscomex";
		this.checkBox4.UseVisualStyleBackColor = true;
		this.checkBox3.AutoSize = true;
		this.checkBox3.Location = new System.Drawing.Point(406, 395);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(152, 18);
		this.checkBox3.TabIndex = 10;
		this.checkBox3.Text = "Integração com ERP";
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox2.AutoSize = true;
		this.checkBox2.Location = new System.Drawing.Point(130, 420);
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.Size = new System.Drawing.Size(166, 18);
		this.checkBox2.TabIndex = 8;
		this.checkBox2.Text = "Auditor Fiscal do SPED";
		this.checkBox2.UseVisualStyleBackColor = true;
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(130, 445);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(241, 18);
		this.checkBox1.TabIndex = 9;
		this.checkBox1.Text = "Controle de Saldos de Exportação";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label4.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label4.Location = new System.Drawing.Point(153, 361);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(2, 23);
		this.label4.TabIndex = 209;
		this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label12.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label12.Location = new System.Drawing.Point(126, 361);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(21, 23);
		this.label12.TabIndex = 208;
		this.label12.Text = "3";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label13.Location = new System.Drawing.Point(161, 363);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(528, 23);
		this.label13.TabIndex = 207;
		this.label13.Text = "Gostaria de receber dicas de funcionalidades por email?";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label5.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label5.Location = new System.Drawing.Point(153, 289);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(2, 23);
		this.label5.TabIndex = 205;
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label6.Location = new System.Drawing.Point(126, 289);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(21, 23);
		this.label6.TabIndex = 204;
		this.label6.Text = "2";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label10.Location = new System.Drawing.Point(161, 291);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(528, 23);
		this.label10.TabIndex = 203;
		this.label10.Text = "Qual a principal problema que o Fiscal.io Monitor resolve no seu dia a dia?";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label7.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label7.Location = new System.Drawing.Point(153, 71);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(2, 23);
		this.label7.TabIndex = 202;
		this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label9.Location = new System.Drawing.Point(126, 71);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(21, 23);
		this.label9.TabIndex = 201;
		this.label9.Text = "1";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cbUserIntent.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.cbUserIntent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbUserIntent.Font = new System.Drawing.Font("Tahoma", 9f);
		this.cbUserIntent.FormattingEnabled = true;
		this.cbUserIntent.Items.AddRange(new object[6] { "Buscar NFe/CTe emitidos contra CPF", "Buscar NFe/CTe emitidos contra CNPJ", "Buscar Documentos de Saída", "Desacordo de Serviço (CTe)", "Recuperação de documentos do passado", "Outros" });
		this.cbUserIntent.Location = new System.Drawing.Point(131, 325);
		this.cbUserIntent.Name = "cbUserIntent";
		this.cbUserIntent.Size = new System.Drawing.Size(538, 22);
		this.cbUserIntent.TabIndex = 6;
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label8.Location = new System.Drawing.Point(161, 73);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(528, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Por favor, confirme seus dados para prosseguir no sistema.";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(6, 513);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(736, 17);
		this.label3.TabIndex = 196;
		this.label3.Text = "Garantimos sigilo total das informações.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(6, 489);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(736, 17);
		this.label2.TabIndex = 195;
		this.label2.Text = "A Fiscal.io está 100% de acordo com a LGPD.";
		this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label11.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Gray;
		this.label11.Location = new System.Drawing.Point(6, 12);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(736, 52);
		this.label11.TabIndex = 189;
		this.label11.Text = "Confirmação de dados cadastrais";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(776, 581);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.pnlContent);
		this.Font = new System.Drawing.Font("Verdana", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		base.Name = "frmGetUserData";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Confirmação de dados cadastrais";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmGetUserData_Load);
		base.SizeChanged += new System.EventHandler(frmGetUserData_SizeChanged);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGetUserData_KeyDown);
		this.pnlContent.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
