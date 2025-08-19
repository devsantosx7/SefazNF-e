using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor;

public class frmTabTools : Form
{
	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private IContainer components;

	private Panel pnContent;

	private Label lbXmlValidatorText;

	private Label lbXmlValidatorTitle;

	private Label lbChannelInText;

	private Label lbChannelInTitle;

	private Label lbDatabaseText02;

	private Label lbDatabaseText01;

	private Label lbDatabaseTitle;

	private Button btChannelIn;

	private PictureBox picOracle;

	private PictureBox picSqlServer;

	private PictureBox picMySql;

	private PictureBox picPostgreSql;

	private Label lbFiscalServerText02;

	private Label lbFiscalServerText01;

	private Label lbFiscalServerTitle;

	private Button btDbaWizard;

	private Label lbFiscalServerText04;

	private Label lbFiscalServerText03;

	private Label lbChannelOutText;

	private Label lbChannelOutTitle;

	private Button btFiscalServer;

	private Button btChannelOut;

	private PictureBox picDestinFolder;

	private PictureBox picDestinEmail;

	private PictureBox picDestinFtp;

	private PictureBox picRighDirection;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel1;

	private PictureBox picBackupHelp;

	private LinkLabel lkbBackupHelp;

	private PictureBox picBackupHardDisk;

	private Button btBackupWizard;

	private PictureBox picBackupNetwork;

	private PictureBox picBackupCloud;

	private Label lbBackupTitle;

	private Label lbBackupText;

	private Panel panel2;

	private PictureBox picChannelInHelp;

	private LinkLabel lkbChannelInHelp;

	private Panel panel3;

	private PictureBox picDbaWizard;

	private LinkLabel lkbDbaWizard;

	private Panel panel4;

	private PictureBox picChannelOutHelp;

	private LinkLabel lkbChannelOutHelp;

	private Panel panel5;

	private PictureBox picFiscalServer;

	private LinkLabel lkbFiscalServer;

	private Panel panel6;

	private Panel panel7;

	private PictureBox picExtSystem;

	private LinkLabel lkbExtSystem;

	private Label lbExtSystemTitle;

	private Label label2;

	private Button btExtSystem;

	private PictureBox picExtSystem04;

	private PictureBox picExtSystem02;

	private PictureBox picExtSystem01;

	private PictureBox picExtSystem03;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabTools(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
	}

	private void btBackupWizard_Click(object sender, EventArgs e)
	{
		funcOpenBackupWizard();
	}

	private void picBackupWizard_Click(object sender, EventArgs e)
	{
		funcOpenBackupWizard();
	}

	public void funcOpenBackupWizard()
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.UserAction = "CONFIG_BACKUP";
		OnEventTabManager(varArguments);
	}

	private void btChannelManager_Click(object sender, EventArgs e)
	{
		string varUserAction = (string)((Button)sender).Tag;
		funcOpenChanelManager(varUserAction);
	}

	private void picChannelManager_Click(object sender, EventArgs e)
	{
		funcOpenChanelManager("CONFIG_CHANNEL_OUT");
	}

	public void funcOpenChanelManager(string pUserAction)
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.UserAction = pUserAction;
		OnEventTabManager(varArguments);
	}

	private async void btDbaWizard_Click(object sender, EventArgs e)
	{
		funcOpenDbaWizard();
	}

	private void picDbaWizard_Click(object sender, EventArgs e)
	{
		funcOpenDbaWizard();
	}

	public void funcOpenDbaWizard()
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.UserAction = "CONFIG_DATABASE";
		OnEventTabManager(varArguments);
	}

	private void btFiscalServer_Click(object sender, EventArgs e)
	{
		funcOpenFiscalServer();
	}

	public void funcOpenFiscalServer()
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.UserAction = "CONFIG_SERVER";
		OnEventTabManager(varArguments);
	}

	private void btExtSystem_Click(object sender, EventArgs e)
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.UserAction = "CONFIG_EXTSYST";
		OnEventTabManager(varArguments);
	}

	public async Task<bool> funcSetTagAsync(string pTagCode, bool pFocused, bool pChecked)
	{
		return false;
	}

	public async Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked)
	{
		return false;
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
		this.pnContent = new System.Windows.Forms.Panel();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.picBackupHelp = new System.Windows.Forms.PictureBox();
		this.lkbBackupHelp = new System.Windows.Forms.LinkLabel();
		this.picBackupHardDisk = new System.Windows.Forms.PictureBox();
		this.btBackupWizard = new System.Windows.Forms.Button();
		this.picBackupNetwork = new System.Windows.Forms.PictureBox();
		this.picBackupCloud = new System.Windows.Forms.PictureBox();
		this.lbBackupTitle = new System.Windows.Forms.Label();
		this.lbBackupText = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.picChannelInHelp = new System.Windows.Forms.PictureBox();
		this.lkbChannelInHelp = new System.Windows.Forms.LinkLabel();
		this.lbChannelInTitle = new System.Windows.Forms.Label();
		this.lbChannelInText = new System.Windows.Forms.Label();
		this.lbXmlValidatorTitle = new System.Windows.Forms.Label();
		this.lbXmlValidatorText = new System.Windows.Forms.Label();
		this.btChannelIn = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.picDbaWizard = new System.Windows.Forms.PictureBox();
		this.lkbDbaWizard = new System.Windows.Forms.LinkLabel();
		this.lbDatabaseTitle = new System.Windows.Forms.Label();
		this.lbDatabaseText01 = new System.Windows.Forms.Label();
		this.lbDatabaseText02 = new System.Windows.Forms.Label();
		this.picPostgreSql = new System.Windows.Forms.PictureBox();
		this.picMySql = new System.Windows.Forms.PictureBox();
		this.picSqlServer = new System.Windows.Forms.PictureBox();
		this.picOracle = new System.Windows.Forms.PictureBox();
		this.btDbaWizard = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.picChannelOutHelp = new System.Windows.Forms.PictureBox();
		this.lkbChannelOutHelp = new System.Windows.Forms.LinkLabel();
		this.lbChannelOutTitle = new System.Windows.Forms.Label();
		this.picRighDirection = new System.Windows.Forms.PictureBox();
		this.lbChannelOutText = new System.Windows.Forms.Label();
		this.picDestinFolder = new System.Windows.Forms.PictureBox();
		this.btChannelOut = new System.Windows.Forms.Button();
		this.picDestinEmail = new System.Windows.Forms.PictureBox();
		this.picDestinFtp = new System.Windows.Forms.PictureBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.picFiscalServer = new System.Windows.Forms.PictureBox();
		this.lbFiscalServerTitle = new System.Windows.Forms.Label();
		this.lkbFiscalServer = new System.Windows.Forms.LinkLabel();
		this.btFiscalServer = new System.Windows.Forms.Button();
		this.lbFiscalServerText01 = new System.Windows.Forms.Label();
		this.lbFiscalServerText04 = new System.Windows.Forms.Label();
		this.lbFiscalServerText02 = new System.Windows.Forms.Label();
		this.lbFiscalServerText03 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.picExtSystem04 = new System.Windows.Forms.PictureBox();
		this.picExtSystem02 = new System.Windows.Forms.PictureBox();
		this.picExtSystem01 = new System.Windows.Forms.PictureBox();
		this.picExtSystem03 = new System.Windows.Forms.PictureBox();
		this.picExtSystem = new System.Windows.Forms.PictureBox();
		this.lkbExtSystem = new System.Windows.Forms.LinkLabel();
		this.lbExtSystemTitle = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btExtSystem = new System.Windows.Forms.Button();
		this.pnContent.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picBackupHelp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picBackupHardDisk).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picBackupNetwork).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picBackupCloud).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picChannelInHelp).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picDbaWizard).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picPostgreSql).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picMySql).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picSqlServer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picOracle).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picChannelOutHelp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picRighDirection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picDestinFolder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picDestinEmail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picDestinFtp).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picFiscalServer).BeginInit();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem04).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem02).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem01).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem03).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem).BeginInit();
		base.SuspendLayout();
		this.pnContent.Controls.Add(this.tableLayoutPanel1);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(1025, 564);
		this.pnContent.TabIndex = 10;
		this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tableLayoutPanel1.ColumnCount = 3;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334f));
		this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel4, 2, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel5, 1, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel6, 2, 1);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 2;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(1025, 564);
		this.tableLayoutPanel1.TabIndex = 232;
		this.panel1.Controls.Add(this.picBackupHelp);
		this.panel1.Controls.Add(this.lkbBackupHelp);
		this.panel1.Controls.Add(this.picBackupHardDisk);
		this.panel1.Controls.Add(this.btBackupWizard);
		this.panel1.Controls.Add(this.picBackupNetwork);
		this.panel1.Controls.Add(this.picBackupCloud);
		this.panel1.Controls.Add(this.lbBackupTitle);
		this.panel1.Controls.Add(this.lbBackupText);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(4, 4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(334, 274);
		this.panel1.TabIndex = 0;
		this.picBackupHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picBackupHelp.Image = Monitor.Resources.image_help;
		this.picBackupHelp.Location = new System.Drawing.Point(68, 232);
		this.picBackupHelp.Name = "picBackupHelp";
		this.picBackupHelp.Size = new System.Drawing.Size(20, 20);
		this.picBackupHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBackupHelp.TabIndex = 231;
		this.picBackupHelp.TabStop = false;
		this.picBackupHelp.Visible = false;
		this.lkbBackupHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lkbBackupHelp.AutoSize = true;
		this.lkbBackupHelp.Location = new System.Drawing.Point(90, 235);
		this.lkbBackupHelp.Name = "lkbBackupHelp";
		this.lkbBackupHelp.Size = new System.Drawing.Size(172, 14);
		this.lkbBackupHelp.TabIndex = 198;
		this.lkbBackupHelp.TabStop = true;
		this.lkbBackupHelp.Text = "Saiba mais. Veja passo a passo";
		this.lkbBackupHelp.Visible = false;
		this.picBackupHardDisk.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picBackupHardDisk.Image = Monitor.Resources.image_backup_harddisk;
		this.picBackupHardDisk.Location = new System.Drawing.Point(145, 102);
		this.picBackupHardDisk.Name = "picBackupHardDisk";
		this.picBackupHardDisk.Size = new System.Drawing.Size(64, 57);
		this.picBackupHardDisk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBackupHardDisk.TabIndex = 193;
		this.picBackupHardDisk.TabStop = false;
		this.picBackupHardDisk.Click += new System.EventHandler(picBackupWizard_Click);
		this.btBackupWizard.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btBackupWizard.Image = Monitor.Resources.image_backup_data;
		this.btBackupWizard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btBackupWizard.Location = new System.Drawing.Point(59, 186);
		this.btBackupWizard.Margin = new System.Windows.Forms.Padding(2);
		this.btBackupWizard.Name = "btBackupWizard";
		this.btBackupWizard.Size = new System.Drawing.Size(213, 36);
		this.btBackupWizard.TabIndex = 183;
		this.btBackupWizard.Text = "Clique aqui para configurar\r\n";
		this.btBackupWizard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btBackupWizard.UseVisualStyleBackColor = true;
		this.btBackupWizard.Click += new System.EventHandler(btBackupWizard_Click);
		this.picBackupNetwork.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picBackupNetwork.Image = Monitor.Resources.image_backup_network;
		this.picBackupNetwork.Location = new System.Drawing.Point(215, 102);
		this.picBackupNetwork.Name = "picBackupNetwork";
		this.picBackupNetwork.Size = new System.Drawing.Size(64, 57);
		this.picBackupNetwork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBackupNetwork.TabIndex = 194;
		this.picBackupNetwork.TabStop = false;
		this.picBackupNetwork.Click += new System.EventHandler(picBackupWizard_Click);
		this.picBackupCloud.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picBackupCloud.Image = Monitor.Resources.image_backup_cloud;
		this.picBackupCloud.Location = new System.Drawing.Point(54, 98);
		this.picBackupCloud.Name = "picBackupCloud";
		this.picBackupCloud.Size = new System.Drawing.Size(85, 64);
		this.picBackupCloud.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBackupCloud.TabIndex = 195;
		this.picBackupCloud.TabStop = false;
		this.picBackupCloud.Click += new System.EventHandler(picBackupWizard_Click);
		this.lbBackupTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbBackupTitle.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbBackupTitle.Location = new System.Drawing.Point(10, 12);
		this.lbBackupTitle.Name = "lbBackupTitle";
		this.lbBackupTitle.Size = new System.Drawing.Size(315, 24);
		this.lbBackupTitle.TabIndex = 196;
		this.lbBackupTitle.Text = "Backup dos XMLs";
		this.lbBackupTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbBackupText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbBackupText.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbBackupText.Location = new System.Drawing.Point(10, 38);
		this.lbBackupText.Name = "lbBackupText";
		this.lbBackupText.Size = new System.Drawing.Size(315, 54);
		this.lbBackupText.TabIndex = 197;
		this.lbBackupText.Text = "Cópias 100% seguras dos arquivos digitais e conforme legislação.";
		this.lbBackupText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel2.Controls.Add(this.picChannelInHelp);
		this.panel2.Controls.Add(this.lkbChannelInHelp);
		this.panel2.Controls.Add(this.lbChannelInTitle);
		this.panel2.Controls.Add(this.lbChannelInText);
		this.panel2.Controls.Add(this.lbXmlValidatorTitle);
		this.panel2.Controls.Add(this.lbXmlValidatorText);
		this.panel2.Controls.Add(this.btChannelIn);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(345, 4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(334, 274);
		this.panel2.TabIndex = 1;
		this.picChannelInHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picChannelInHelp.Image = Monitor.Resources.image_help;
		this.picChannelInHelp.Location = new System.Drawing.Point(72, 232);
		this.picChannelInHelp.Name = "picChannelInHelp";
		this.picChannelInHelp.Size = new System.Drawing.Size(20, 20);
		this.picChannelInHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picChannelInHelp.TabIndex = 233;
		this.picChannelInHelp.TabStop = false;
		this.picChannelInHelp.Visible = false;
		this.lkbChannelInHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lkbChannelInHelp.AutoSize = true;
		this.lkbChannelInHelp.Location = new System.Drawing.Point(94, 235);
		this.lkbChannelInHelp.Name = "lkbChannelInHelp";
		this.lkbChannelInHelp.Size = new System.Drawing.Size(172, 14);
		this.lkbChannelInHelp.TabIndex = 232;
		this.lkbChannelInHelp.TabStop = true;
		this.lkbChannelInHelp.Text = "Saiba mais. Veja passo a passo";
		this.lkbChannelInHelp.Visible = false;
		this.lbChannelInTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbChannelInTitle.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbChannelInTitle.Location = new System.Drawing.Point(10, 12);
		this.lbChannelInTitle.Name = "lbChannelInTitle";
		this.lbChannelInTitle.Size = new System.Drawing.Size(315, 24);
		this.lbChannelInTitle.TabIndex = 198;
		this.lbChannelInTitle.Text = "Extrair XMLs de emails";
		this.lbChannelInTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbChannelInText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbChannelInText.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbChannelInText.Location = new System.Drawing.Point(10, 38);
		this.lbChannelInText.Name = "lbChannelInText";
		this.lbChannelInText.Size = new System.Drawing.Size(315, 54);
		this.lbChannelInText.TabIndex = 199;
		this.lbChannelInText.Text = "Automatize o processo de captura dos arquivos XML de uma ou mais caixas de e-mail";
		this.lbChannelInText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbXmlValidatorTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbXmlValidatorTitle.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbXmlValidatorTitle.Location = new System.Drawing.Point(10, 95);
		this.lbXmlValidatorTitle.Name = "lbXmlValidatorTitle";
		this.lbXmlValidatorTitle.Size = new System.Drawing.Size(315, 24);
		this.lbXmlValidatorTitle.TabIndex = 200;
		this.lbXmlValidatorTitle.Text = "Validação jurídica dos XMLs";
		this.lbXmlValidatorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbXmlValidatorText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbXmlValidatorText.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbXmlValidatorText.Location = new System.Drawing.Point(10, 124);
		this.lbXmlValidatorText.Name = "lbXmlValidatorText";
		this.lbXmlValidatorText.Size = new System.Drawing.Size(315, 35);
		this.lbXmlValidatorText.TabIndex = 201;
		this.lbXmlValidatorText.Text = "Identificação automática dos XMLs sem validade jurídica ou cancelados na SEFAZ";
		this.lbXmlValidatorText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btChannelIn.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btChannelIn.Image = Monitor.Resources.image_integration;
		this.btChannelIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btChannelIn.Location = new System.Drawing.Point(70, 186);
		this.btChannelIn.Margin = new System.Windows.Forms.Padding(2);
		this.btChannelIn.Name = "btChannelIn";
		this.btChannelIn.Size = new System.Drawing.Size(213, 36);
		this.btChannelIn.TabIndex = 202;
		this.btChannelIn.Tag = "CONFIG_CHANNEL_IN";
		this.btChannelIn.Text = "Clique aqui para configurar";
		this.btChannelIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btChannelIn.UseVisualStyleBackColor = true;
		this.btChannelIn.Click += new System.EventHandler(btChannelManager_Click);
		this.panel3.Controls.Add(this.picDbaWizard);
		this.panel3.Controls.Add(this.lkbDbaWizard);
		this.panel3.Controls.Add(this.lbDatabaseTitle);
		this.panel3.Controls.Add(this.lbDatabaseText01);
		this.panel3.Controls.Add(this.lbDatabaseText02);
		this.panel3.Controls.Add(this.picPostgreSql);
		this.panel3.Controls.Add(this.picMySql);
		this.panel3.Controls.Add(this.picSqlServer);
		this.panel3.Controls.Add(this.picOracle);
		this.panel3.Controls.Add(this.btDbaWizard);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(4, 285);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(334, 275);
		this.panel3.TabIndex = 2;
		this.picDbaWizard.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picDbaWizard.Image = Monitor.Resources.image_help;
		this.picDbaWizard.Location = new System.Drawing.Point(68, 239);
		this.picDbaWizard.Name = "picDbaWizard";
		this.picDbaWizard.Size = new System.Drawing.Size(20, 20);
		this.picDbaWizard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picDbaWizard.TabIndex = 233;
		this.picDbaWizard.TabStop = false;
		this.picDbaWizard.Visible = false;
		this.lkbDbaWizard.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lkbDbaWizard.AutoSize = true;
		this.lkbDbaWizard.Location = new System.Drawing.Point(90, 242);
		this.lkbDbaWizard.Name = "lkbDbaWizard";
		this.lkbDbaWizard.Size = new System.Drawing.Size(172, 14);
		this.lkbDbaWizard.TabIndex = 232;
		this.lkbDbaWizard.TabStop = true;
		this.lkbDbaWizard.Text = "Saiba mais. Veja passo a passo";
		this.lkbDbaWizard.Visible = false;
		this.lbDatabaseTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbDatabaseTitle.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDatabaseTitle.Location = new System.Drawing.Point(10, 11);
		this.lbDatabaseTitle.Name = "lbDatabaseTitle";
		this.lbDatabaseTitle.Size = new System.Drawing.Size(315, 24);
		this.lbDatabaseTitle.TabIndex = 207;
		this.lbDatabaseTitle.Text = "Banco de dados profissional";
		this.lbDatabaseTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbDatabaseText01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbDatabaseText01.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDatabaseText01.ForeColor = System.Drawing.Color.Blue;
		this.lbDatabaseText01.Location = new System.Drawing.Point(10, 35);
		this.lbDatabaseText01.Name = "lbDatabaseText01";
		this.lbDatabaseText01.Size = new System.Drawing.Size(315, 35);
		this.lbDatabaseText01.TabIndex = 208;
		this.lbDatabaseText01.Text = "Aumente 20 vezes a velocidade do Monitor";
		this.lbDatabaseText01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbDatabaseText02.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbDatabaseText02.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDatabaseText02.Location = new System.Drawing.Point(10, 74);
		this.lbDatabaseText02.Name = "lbDatabaseText02";
		this.lbDatabaseText02.Size = new System.Drawing.Size(315, 42);
		this.lbDatabaseText02.TabIndex = 209;
		this.lbDatabaseText02.Text = "Instalações multiusuários com gestão unificada dos dados";
		this.lbDatabaseText02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.picPostgreSql.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picPostgreSql.Image = Monitor.Resources.image_postgresql;
		this.picPostgreSql.Location = new System.Drawing.Point(32, 122);
		this.picPostgreSql.Name = "picPostgreSql";
		this.picPostgreSql.Size = new System.Drawing.Size(62, 56);
		this.picPostgreSql.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picPostgreSql.TabIndex = 210;
		this.picPostgreSql.TabStop = false;
		this.picPostgreSql.Click += new System.EventHandler(picDbaWizard_Click);
		this.picMySql.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picMySql.Image = Monitor.Resources.image_mysql;
		this.picMySql.Location = new System.Drawing.Point(100, 122);
		this.picMySql.Name = "picMySql";
		this.picMySql.Size = new System.Drawing.Size(62, 56);
		this.picMySql.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picMySql.TabIndex = 211;
		this.picMySql.TabStop = false;
		this.picMySql.Click += new System.EventHandler(picDbaWizard_Click);
		this.picSqlServer.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picSqlServer.Image = Monitor.Resources.image_sqlserver;
		this.picSqlServer.Location = new System.Drawing.Point(168, 122);
		this.picSqlServer.Name = "picSqlServer";
		this.picSqlServer.Size = new System.Drawing.Size(62, 56);
		this.picSqlServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picSqlServer.TabIndex = 212;
		this.picSqlServer.TabStop = false;
		this.picSqlServer.Click += new System.EventHandler(picDbaWizard_Click);
		this.picOracle.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picOracle.Image = Monitor.Resources.image_oracle;
		this.picOracle.Location = new System.Drawing.Point(236, 122);
		this.picOracle.Name = "picOracle";
		this.picOracle.Size = new System.Drawing.Size(62, 56);
		this.picOracle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picOracle.TabIndex = 213;
		this.picOracle.TabStop = false;
		this.picOracle.Click += new System.EventHandler(picDbaWizard_Click);
		this.btDbaWizard.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btDbaWizard.Image = Monitor.Resources.image_magic_wand;
		this.btDbaWizard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btDbaWizard.Location = new System.Drawing.Point(59, 195);
		this.btDbaWizard.Margin = new System.Windows.Forms.Padding(2);
		this.btDbaWizard.Name = "btDbaWizard";
		this.btDbaWizard.Size = new System.Drawing.Size(213, 36);
		this.btDbaWizard.TabIndex = 214;
		this.btDbaWizard.Text = "Clique aqui para configurar";
		this.btDbaWizard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btDbaWizard.UseVisualStyleBackColor = true;
		this.btDbaWizard.Click += new System.EventHandler(btDbaWizard_Click);
		this.panel4.Controls.Add(this.picChannelOutHelp);
		this.panel4.Controls.Add(this.lkbChannelOutHelp);
		this.panel4.Controls.Add(this.lbChannelOutTitle);
		this.panel4.Controls.Add(this.picRighDirection);
		this.panel4.Controls.Add(this.lbChannelOutText);
		this.panel4.Controls.Add(this.picDestinFolder);
		this.panel4.Controls.Add(this.btChannelOut);
		this.panel4.Controls.Add(this.picDestinEmail);
		this.panel4.Controls.Add(this.picDestinFtp);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(686, 4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(335, 274);
		this.panel4.TabIndex = 3;
		this.picChannelOutHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picChannelOutHelp.Image = Monitor.Resources.image_help;
		this.picChannelOutHelp.Location = new System.Drawing.Point(69, 232);
		this.picChannelOutHelp.Name = "picChannelOutHelp";
		this.picChannelOutHelp.Size = new System.Drawing.Size(20, 20);
		this.picChannelOutHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picChannelOutHelp.TabIndex = 233;
		this.picChannelOutHelp.TabStop = false;
		this.picChannelOutHelp.Visible = false;
		this.lkbChannelOutHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lkbChannelOutHelp.AutoSize = true;
		this.lkbChannelOutHelp.Location = new System.Drawing.Point(91, 235);
		this.lkbChannelOutHelp.Name = "lkbChannelOutHelp";
		this.lkbChannelOutHelp.Size = new System.Drawing.Size(172, 14);
		this.lkbChannelOutHelp.TabIndex = 232;
		this.lkbChannelOutHelp.TabStop = true;
		this.lkbChannelOutHelp.Text = "Saiba mais. Veja passo a passo";
		this.lkbChannelOutHelp.Visible = false;
		this.lbChannelOutTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbChannelOutTitle.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbChannelOutTitle.Location = new System.Drawing.Point(10, 12);
		this.lbChannelOutTitle.Name = "lbChannelOutTitle";
		this.lbChannelOutTitle.Size = new System.Drawing.Size(315, 24);
		this.lbChannelOutTitle.TabIndex = 224;
		this.lbChannelOutTitle.Text = "Exportação de XMLs";
		this.lbChannelOutTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.picRighDirection.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picRighDirection.Image = Monitor.Resources.image_right_direction;
		this.picRighDirection.Location = new System.Drawing.Point(59, 115);
		this.picRighDirection.Name = "picRighDirection";
		this.picRighDirection.Size = new System.Drawing.Size(30, 30);
		this.picRighDirection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picRighDirection.TabIndex = 230;
		this.picRighDirection.TabStop = false;
		this.picRighDirection.Click += new System.EventHandler(picChannelManager_Click);
		this.lbChannelOutText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbChannelOutText.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbChannelOutText.Location = new System.Drawing.Point(10, 38);
		this.lbChannelOutText.Name = "lbChannelOutText";
		this.lbChannelOutText.Size = new System.Drawing.Size(315, 54);
		this.lbChannelOutText.TabIndex = 225;
		this.lbChannelOutText.Text = "Automatize a exportação dos arquivos XML recebidos da SEFAZ e de outras fontes como e-mail, pasta e ftp.";
		this.lbChannelOutText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.picDestinFolder.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picDestinFolder.Image = Monitor.Resources.image_source_folder;
		this.picDestinFolder.Location = new System.Drawing.Point(164, 107);
		this.picDestinFolder.Name = "picDestinFolder";
		this.picDestinFolder.Size = new System.Drawing.Size(53, 47);
		this.picDestinFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picDestinFolder.TabIndex = 229;
		this.picDestinFolder.TabStop = false;
		this.picDestinFolder.Click += new System.EventHandler(picChannelManager_Click);
		this.btChannelOut.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btChannelOut.Image = Monitor.Resources.image_integration;
		this.btChannelOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btChannelOut.Location = new System.Drawing.Point(59, 186);
		this.btChannelOut.Margin = new System.Windows.Forms.Padding(2);
		this.btChannelOut.Name = "btChannelOut";
		this.btChannelOut.Size = new System.Drawing.Size(213, 36);
		this.btChannelOut.TabIndex = 226;
		this.btChannelOut.Tag = "CONFIG_CHANNEL_OUT";
		this.btChannelOut.Text = "Clique aqui para configurar";
		this.btChannelOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btChannelOut.UseVisualStyleBackColor = true;
		this.btChannelOut.Click += new System.EventHandler(btChannelManager_Click);
		this.picDestinEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picDestinEmail.Image = Monitor.Resources.image_source_email;
		this.picDestinEmail.Location = new System.Drawing.Point(105, 107);
		this.picDestinEmail.Name = "picDestinEmail";
		this.picDestinEmail.Size = new System.Drawing.Size(53, 47);
		this.picDestinEmail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picDestinEmail.TabIndex = 228;
		this.picDestinEmail.TabStop = false;
		this.picDestinEmail.Click += new System.EventHandler(picChannelManager_Click);
		this.picDestinFtp.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picDestinFtp.Image = Monitor.Resources.image_source_ftp;
		this.picDestinFtp.Location = new System.Drawing.Point(223, 107);
		this.picDestinFtp.Name = "picDestinFtp";
		this.picDestinFtp.Size = new System.Drawing.Size(53, 47);
		this.picDestinFtp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picDestinFtp.TabIndex = 227;
		this.picDestinFtp.TabStop = false;
		this.picDestinFtp.Click += new System.EventHandler(picChannelManager_Click);
		this.panel5.Controls.Add(this.picFiscalServer);
		this.panel5.Controls.Add(this.lbFiscalServerTitle);
		this.panel5.Controls.Add(this.lkbFiscalServer);
		this.panel5.Controls.Add(this.btFiscalServer);
		this.panel5.Controls.Add(this.lbFiscalServerText01);
		this.panel5.Controls.Add(this.lbFiscalServerText04);
		this.panel5.Controls.Add(this.lbFiscalServerText02);
		this.panel5.Controls.Add(this.lbFiscalServerText03);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(345, 285);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(334, 275);
		this.panel5.TabIndex = 4;
		this.picFiscalServer.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picFiscalServer.Image = Monitor.Resources.image_help;
		this.picFiscalServer.Location = new System.Drawing.Point(78, 239);
		this.picFiscalServer.Name = "picFiscalServer";
		this.picFiscalServer.Size = new System.Drawing.Size(20, 20);
		this.picFiscalServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFiscalServer.TabIndex = 235;
		this.picFiscalServer.TabStop = false;
		this.picFiscalServer.Visible = false;
		this.lbFiscalServerTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbFiscalServerTitle.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFiscalServerTitle.Location = new System.Drawing.Point(10, 11);
		this.lbFiscalServerTitle.Name = "lbFiscalServerTitle";
		this.lbFiscalServerTitle.Size = new System.Drawing.Size(315, 24);
		this.lbFiscalServerTitle.TabIndex = 215;
		this.lbFiscalServerTitle.Text = "Fiscal.io Server ";
		this.lbFiscalServerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbFiscalServer.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lkbFiscalServer.AutoSize = true;
		this.lkbFiscalServer.Location = new System.Drawing.Point(100, 242);
		this.lkbFiscalServer.Name = "lkbFiscalServer";
		this.lkbFiscalServer.Size = new System.Drawing.Size(172, 14);
		this.lkbFiscalServer.TabIndex = 234;
		this.lkbFiscalServer.TabStop = true;
		this.lkbFiscalServer.Text = "Saiba mais. Veja passo a passo";
		this.lkbFiscalServer.Visible = false;
		this.btFiscalServer.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btFiscalServer.Image = Monitor.Resources.image_fiscal_robot;
		this.btFiscalServer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btFiscalServer.Location = new System.Drawing.Point(70, 195);
		this.btFiscalServer.Margin = new System.Windows.Forms.Padding(2);
		this.btFiscalServer.Name = "btFiscalServer";
		this.btFiscalServer.Size = new System.Drawing.Size(213, 36);
		this.btFiscalServer.TabIndex = 223;
		this.btFiscalServer.Text = "Clique aqui para configurar";
		this.btFiscalServer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btFiscalServer.UseVisualStyleBackColor = true;
		this.btFiscalServer.Click += new System.EventHandler(btFiscalServer_Click);
		this.lbFiscalServerText01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbFiscalServerText01.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFiscalServerText01.ForeColor = System.Drawing.Color.Blue;
		this.lbFiscalServerText01.Location = new System.Drawing.Point(10, 35);
		this.lbFiscalServerText01.Name = "lbFiscalServerText01";
		this.lbFiscalServerText01.Size = new System.Drawing.Size(315, 35);
		this.lbFiscalServerText01.TabIndex = 216;
		this.lbFiscalServerText01.Text = "Aumente 100 vezes a velocidade do Monitor";
		this.lbFiscalServerText01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbFiscalServerText04.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbFiscalServerText04.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFiscalServerText04.ForeColor = System.Drawing.Color.Blue;
		this.lbFiscalServerText04.Location = new System.Drawing.Point(10, 153);
		this.lbFiscalServerText04.Name = "lbFiscalServerText04";
		this.lbFiscalServerText04.Size = new System.Drawing.Size(315, 29);
		this.lbFiscalServerText04.TabIndex = 222;
		this.lbFiscalServerText04.Text = "Processamento simultâneo de múltiplas empresas";
		this.lbFiscalServerText04.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lbFiscalServerText02.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbFiscalServerText02.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFiscalServerText02.Location = new System.Drawing.Point(10, 74);
		this.lbFiscalServerText02.Name = "lbFiscalServerText02";
		this.lbFiscalServerText02.Size = new System.Drawing.Size(315, 42);
		this.lbFiscalServerText02.TabIndex = 217;
		this.lbFiscalServerText02.Text = "Processamento de grandes volumes de dados com alta performance e escalabilidade";
		this.lbFiscalServerText02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbFiscalServerText03.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbFiscalServerText03.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFiscalServerText03.ForeColor = System.Drawing.Color.Blue;
		this.lbFiscalServerText03.Location = new System.Drawing.Point(10, 124);
		this.lbFiscalServerText03.Name = "lbFiscalServerText03";
		this.lbFiscalServerText03.Size = new System.Drawing.Size(315, 29);
		this.lbFiscalServerText03.TabIndex = 221;
		this.lbFiscalServerText03.Text = "Gestão centralizada dos certificados digitais";
		this.lbFiscalServerText03.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(686, 285);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(335, 275);
		this.panel6.TabIndex = 5;
		this.panel7.Controls.Add(this.picExtSystem04);
		this.panel7.Controls.Add(this.picExtSystem02);
		this.panel7.Controls.Add(this.picExtSystem01);
		this.panel7.Controls.Add(this.picExtSystem03);
		this.panel7.Controls.Add(this.picExtSystem);
		this.panel7.Controls.Add(this.lkbExtSystem);
		this.panel7.Controls.Add(this.lbExtSystemTitle);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.btExtSystem);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(335, 275);
		this.panel7.TabIndex = 4;
		this.picExtSystem04.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picExtSystem04.Image = Monitor.Resources.image_alterdata;
		this.picExtSystem04.Location = new System.Drawing.Point(231, 114);
		this.picExtSystem04.Name = "picExtSystem04";
		this.picExtSystem04.Size = new System.Drawing.Size(53, 47);
		this.picExtSystem04.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picExtSystem04.TabIndex = 237;
		this.picExtSystem04.TabStop = false;
		this.picExtSystem02.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picExtSystem02.Image = Monitor.Resources.image_dominio_erp;
		this.picExtSystem02.Location = new System.Drawing.Point(113, 114);
		this.picExtSystem02.Name = "picExtSystem02";
		this.picExtSystem02.Size = new System.Drawing.Size(53, 47);
		this.picExtSystem02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picExtSystem02.TabIndex = 236;
		this.picExtSystem02.TabStop = false;
		this.picExtSystem01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picExtSystem01.Image = Monitor.Resources.image_totvs_protheus;
		this.picExtSystem01.Location = new System.Drawing.Point(54, 114);
		this.picExtSystem01.Name = "picExtSystem01";
		this.picExtSystem01.Size = new System.Drawing.Size(53, 47);
		this.picExtSystem01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picExtSystem01.TabIndex = 235;
		this.picExtSystem01.TabStop = false;
		this.picExtSystem03.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picExtSystem03.Image = Monitor.Resources.image_sap_erp;
		this.picExtSystem03.Location = new System.Drawing.Point(172, 114);
		this.picExtSystem03.Name = "picExtSystem03";
		this.picExtSystem03.Size = new System.Drawing.Size(53, 47);
		this.picExtSystem03.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picExtSystem03.TabIndex = 234;
		this.picExtSystem03.TabStop = false;
		this.picExtSystem.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.picExtSystem.Image = Monitor.Resources.image_help;
		this.picExtSystem.Location = new System.Drawing.Point(78, 239);
		this.picExtSystem.Name = "picExtSystem";
		this.picExtSystem.Size = new System.Drawing.Size(20, 20);
		this.picExtSystem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picExtSystem.TabIndex = 233;
		this.picExtSystem.TabStop = false;
		this.picExtSystem.Visible = false;
		this.lkbExtSystem.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lkbExtSystem.AutoSize = true;
		this.lkbExtSystem.Location = new System.Drawing.Point(100, 242);
		this.lkbExtSystem.Name = "lkbExtSystem";
		this.lkbExtSystem.Size = new System.Drawing.Size(172, 14);
		this.lkbExtSystem.TabIndex = 232;
		this.lkbExtSystem.TabStop = true;
		this.lkbExtSystem.Text = "Saiba mais. Veja passo a passo";
		this.lkbExtSystem.Visible = false;
		this.lbExtSystemTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbExtSystemTitle.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbExtSystemTitle.Location = new System.Drawing.Point(10, 12);
		this.lbExtSystemTitle.Name = "lbExtSystemTitle";
		this.lbExtSystemTitle.Size = new System.Drawing.Size(315, 24);
		this.lbExtSystemTitle.TabIndex = 224;
		this.lbExtSystemTitle.Text = "Fiscal.io Connect";
		this.lbExtSystemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 38);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(315, 54);
		this.label2.TabIndex = 225;
		this.label2.Text = "Acompanhamento online de lançamento dos documentos fiscais em sistemas como Dominio, AlterData, SAGE, MasterMaq, Prothers, LINX, SAP, etc.";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btExtSystem.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btExtSystem.Image = Monitor.Resources.image_integration;
		this.btExtSystem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btExtSystem.Location = new System.Drawing.Point(70, 195);
		this.btExtSystem.Margin = new System.Windows.Forms.Padding(2);
		this.btExtSystem.Name = "btExtSystem";
		this.btExtSystem.Size = new System.Drawing.Size(213, 36);
		this.btExtSystem.TabIndex = 226;
		this.btExtSystem.Tag = "CONFIG_CHANNEL_OUT";
		this.btExtSystem.Text = "Clique aqui para configurar";
		this.btExtSystem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btExtSystem.UseVisualStyleBackColor = true;
		this.btExtSystem.Click += new System.EventHandler(btExtSystem_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1025, 564);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabTools";
		this.Text = "Automações";
		this.pnContent.ResumeLayout(false);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picBackupHelp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picBackupHardDisk).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picBackupNetwork).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picBackupCloud).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picChannelInHelp).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picDbaWizard).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picPostgreSql).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picMySql).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picSqlServer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picOracle).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picChannelOutHelp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picRighDirection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picDestinFolder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picDestinEmail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picDestinFtp).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picFiscalServer).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem04).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem02).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem01).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem03).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picExtSystem).EndInit();
		base.ResumeLayout(false);
	}
}
