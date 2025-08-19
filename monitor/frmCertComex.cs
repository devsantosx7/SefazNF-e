using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmCertComex : Form
{
	private string _Certificate;

	private string _UserProfile;

	private CancellationTokenSource varTokenToCancel = new CancellationTokenSource();

	private clsSingleCertificates varclsCertificates;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private IContainer components;

	private ImageList imgCertific;

	private ListView lvCertific;

	private ColumnHeader colStart;

	private ColumnHeader colDesc;

	private ColumnHeader colDtBegin;

	private ColumnHeader clDtEndOf;

	private Button btConfirmar;

	private Button btRefresh;

	private Label label4;

	private Label lbsep01;

	private PictureBox pictureBox1;

	private Label lbsep02;

	private Button btImport;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private Label label1;

	private Label lbSiscomexProfile;

	private ComboBox cbSiscomexProfile;

	private Label label3;

	private Label label5;

	private ColumnHeader colIdent;

	public frmCertComex()
	{
		InitializeComponent();
		_Certificate = (_UserProfile = string.Empty);
		varclsCertificates = clsSingleCertificates.Instance;
		base.Height = lbsep01.Top + 40;
	}

	private async void frmCertComex_Shown(object sender, EventArgs e)
	{
		await funcProfileDataAsync();
		await funcLoadCertListAsync();
	}

	public string funcGetCertificate()
	{
		return _Certificate;
	}

	public string funcGetUserProfile()
	{
		return _UserProfile;
	}

	private async Task<bool> funcProfileDataAsync()
	{
		string varSiscomexProfile = await _clsDataParam.funcGetAsync("Export-Siscomex-Profile", pBuffer: true, pGlobal: true);
		if (clsFunction.IsEmpty(varSiscomexProfile))
		{
			varSiscomexProfile = "IMPEXP";
		}
		List<clsComexApi.clsProfile> varProfileList = new clsComexApi().funcGetProfileList();
		cbSiscomexProfile.DisplayMember = "Name";
		cbSiscomexProfile.ValueMember = "Type";
		cbSiscomexProfile.DataSource = varProfileList;
		cbSiscomexProfile.SelectedValue = varSiscomexProfile;
		return true;
	}

	private async Task<bool> funcLoadCertListAsync()
	{
		try
		{
			lbProgress.Text = "Aguarde, procurando Certificados Digitais disponíveis...";
			plnMessage.Visible = true;
			Application.DoEvents();
			List<clsSrvCertFull> varList = await varclsCertificates.funcGetListAsync(varTokenToCancel.Token, null, pReload: true);
			if (varTokenToCancel.IsCancellationRequested)
			{
				return false;
			}
			lvCertific.Items.Clear();
			foreach (clsSrvCertFull varclsItem in varList)
			{
				funcAddCertItem(varclsItem);
			}
		}
		finally
		{
			plnMessage.Visible = false;
			Application.DoEvents();
		}
		return true;
	}

	private void funcAddCertItem(clsSrvCertFull pclsObject, bool pCheckRoot = true)
	{
		ListViewItem varListItem = new ListViewItem();
		varListItem.Text = "";
		varListItem.Tag = pclsObject.SerialNumber;
		varListItem.ImageIndex = 0;
		string varIdentCode = pclsObject.CNPJView;
		if (clsFunction.IsEmpty(varIdentCode))
		{
			varIdentCode = pclsObject.CPFView;
		}
		ListViewItem.ListViewSubItem varListViewSubItem01 = new ListViewItem.ListViewSubItem(varListItem, varIdentCode);
		varListItem.SubItems.Add(varListViewSubItem01);
		ListViewItem.ListViewSubItem varListViewSubItem2 = new ListViewItem.ListViewSubItem(varListItem, pclsObject.TitularView);
		varListItem.SubItems.Add(varListViewSubItem2);
		ListViewItem.ListViewSubItem varListViewSubItem3 = new ListViewItem.ListViewSubItem(varListItem, pclsObject.Certificate.NotBefore.ToString("dd/MM/yyyy"));
		varListItem.SubItems.Add(varListViewSubItem3);
		ListViewItem.ListViewSubItem varListViewSubItem4 = new ListViewItem.ListViewSubItem(varListItem, pclsObject.Certificate.NotAfter.ToString("dd/MM/yyyy"));
		varListItem.SubItems.Add(varListViewSubItem4);
		lvCertific.Items.Add(varListItem);
	}

	private void btCancelar_Click(object sender, EventArgs e)
	{
		_Certificate = (_UserProfile = string.Empty);
		Close();
	}

	private async void lvCertific_DoubleClick(object sender, EventArgs e)
	{
		_Certificate = await funcDefineCertificateAsync();
		_UserProfile = clsFunction.funcGetValue(cbSiscomexProfile.SelectedValue);
		if (!clsFunction.IsEmpty(_Certificate))
		{
			Close();
		}
	}

	private async void btConfirmar_Click(object sender, EventArgs e)
	{
		_Certificate = await funcDefineCertificateAsync();
		_UserProfile = clsFunction.funcGetValue(cbSiscomexProfile.SelectedValue);
		if (clsFunction.IsEmpty(_Certificate))
		{
			base.DialogResult = DialogResult.None;
		}
		else
		{
			Close();
		}
	}

	private async Task<string> funcDefineCertificateAsync()
	{
		_ = string.Empty;
		string varUserSelect;
		try
		{
			varUserSelect = (string)lvCertific.SelectedItems[0].Tag;
		}
		catch
		{
			varUserSelect = string.Empty;
		}
		if (string.IsNullOrEmpty(varUserSelect))
		{
			try
			{
				varUserSelect = (string)lvCertific.FocusedItem.Tag;
			}
			catch
			{
				varUserSelect = string.Empty;
			}
		}
		if (string.IsNullOrEmpty(varUserSelect))
		{
			MessageBox.Show("Nenhum certificado digital (A1, A2 , A3) válido foi selecionado.", "Fiscal.io Monitor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return string.Empty;
		}
		clsSrvCertFull varCertContent = await varclsCertificates.funcGetCertDataAsync(varUserSelect);
		if (varCertContent == null)
		{
			MessageBox.Show("O certificado digital selecionado não é valido.", "Fiscal.io Monitor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return string.Empty;
		}
		if (varCertContent.Certificate == null)
		{
			MessageBox.Show("O certificado digital selecionado não é valido.", "Fiscal.io Monitor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return string.Empty;
		}
		if (!varCertContent.Certificate.HasPrivateKey)
		{
			MessageBox.Show("Certificado Digital não foi instalado com a Chave Privada." + Environment.NewLine + "Favor instalar o mesmo novamente no Windows." + Environment.NewLine + "Mas lembre-se de permitir acesso a sua chave privada.", "Fiscal.io Monitor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return string.Empty;
		}
		clsReturn varclsReturnCheck = await varCertContent.funcCheckSignAsync();
		string varErrorMessage = varclsReturnCheck.funcGetAllMessageResult();
		if (clsFunction.funcIsCertError(varErrorMessage))
		{
			frmCertAdvice obj3 = new frmCertAdvice();
			obj3.ShowDialog(this);
			obj3.Dispose();
			return string.Empty;
		}
		if (clsFunction.funcIsCertCancel(varErrorMessage))
		{
			return string.Empty;
		}
		if (varclsReturnCheck.HasError)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnCheck);
			return string.Empty;
		}
		return varUserSelect;
	}

	private async void btRefresh_Click(object sender, EventArgs e)
	{
		int varTotalItens = lvCertific.Items.Count;
		await funcLoadCertListAsync();
		if (lvCertific.Items.Count <= varTotalItens)
		{
			base.Height = lbsep02.Top + 50;
		}
	}

	private void lvCertific_SelectedIndexChanged(object sender, EventArgs e)
	{
		foreach (ListViewItem varItem in ((ListView)sender).Items)
		{
			if (varItem.Selected)
			{
				varItem.BackColor = Color.LightBlue;
			}
			else
			{
				varItem.BackColor = Color.White;
			}
		}
	}

	private void frmCertComex_Leave(object sender, EventArgs e)
	{
		_Certificate = (_UserProfile = string.Empty);
	}

	private async void btImport_Click(object sender, EventArgs e)
	{
		frmCertItem varfrmCertItem = new frmCertItem();
		if (varfrmCertItem.ShowDialog(this).Equals(DialogResult.Cancel))
		{
			return;
		}
		X509Certificate2 varCertificate = varfrmCertItem.funcGetObject();
		varfrmCertItem.Dispose();
		if (varCertificate == null)
		{
			return;
		}
		clsSrvCertFull varclsObject = await varclsCertificates.funcGetCertDataAsync(varCertificate.SerialNumber);
		if (varclsObject == null)
		{
			return;
		}
		try
		{
			funcAddCertItem(varclsObject, pCheckRoot: false);
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void frmCertComex_FormClosed(object sender, FormClosedEventArgs e)
	{
		varTokenToCancel.Cancel();
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmCertComex));
		this.imgCertific = new System.Windows.Forms.ImageList(this.components);
		this.lvCertific = new System.Windows.Forms.ListView();
		this.colStart = new System.Windows.Forms.ColumnHeader();
		this.colDesc = new System.Windows.Forms.ColumnHeader();
		this.colDtBegin = new System.Windows.Forms.ColumnHeader();
		this.clDtEndOf = new System.Windows.Forms.ColumnHeader();
		this.btConfirmar = new System.Windows.Forms.Button();
		this.btRefresh = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.lbsep01 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.lbsep02 = new System.Windows.Forms.Label();
		this.btImport = new System.Windows.Forms.Button();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.lbSiscomexProfile = new System.Windows.Forms.Label();
		this.cbSiscomexProfile = new System.Windows.Forms.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.colIdent = new System.Windows.Forms.ColumnHeader();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		base.SuspendLayout();
		this.imgCertific.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgCertific.ImageStream");
		this.imgCertific.TransparentColor = System.Drawing.Color.Transparent;
		this.imgCertific.Images.SetKeyName(0, "image_certificate.jpg");
		this.lvCertific.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5] { this.colStart, this.colIdent, this.colDesc, this.colDtBegin, this.clDtEndOf });
		this.lvCertific.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lvCertific.FullRowSelect = true;
		this.lvCertific.HideSelection = false;
		this.lvCertific.LargeImageList = this.imgCertific;
		this.lvCertific.Location = new System.Drawing.Point(12, 77);
		this.lvCertific.MultiSelect = false;
		this.lvCertific.Name = "lvCertific";
		this.lvCertific.Size = new System.Drawing.Size(743, 229);
		this.lvCertific.SmallImageList = this.imgCertific;
		this.lvCertific.TabIndex = 25;
		this.lvCertific.UseCompatibleStateImageBehavior = false;
		this.lvCertific.View = System.Windows.Forms.View.Details;
		this.lvCertific.SelectedIndexChanged += new System.EventHandler(lvCertific_SelectedIndexChanged);
		this.lvCertific.DoubleClick += new System.EventHandler(lvCertific_DoubleClick);
		this.colStart.Text = "";
		this.colStart.Width = 26;
		this.colDesc.Text = "Descrição";
		this.colDesc.Width = 350;
		this.colDtBegin.Text = "Inicio Validade";
		this.colDtBegin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.colDtBegin.Width = 95;
		this.clDtEndOf.Text = "Fim Validade";
		this.clDtEndOf.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDtEndOf.Width = 95;
		this.btConfirmar.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btConfirmar.Location = new System.Drawing.Point(634, 312);
		this.btConfirmar.Name = "btConfirmar";
		this.btConfirmar.Size = new System.Drawing.Size(121, 34);
		this.btConfirmar.TabIndex = 23;
		this.btConfirmar.Text = "&Confirmar";
		this.btConfirmar.UseVisualStyleBackColor = true;
		this.btConfirmar.Click += new System.EventHandler(btConfirmar_Click);
		this.btRefresh.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btRefresh.ForeColor = System.Drawing.Color.Blue;
		this.btRefresh.Location = new System.Drawing.Point(11, 312);
		this.btRefresh.Name = "btRefresh";
		this.btRefresh.Size = new System.Drawing.Size(121, 34);
		this.btRefresh.TabIndex = 26;
		this.btRefresh.Text = "&Atualizar lista";
		this.btRefresh.UseVisualStyleBackColor = true;
		this.btRefresh.Click += new System.EventHandler(btRefresh_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(46, 364);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(654, 26);
		this.label4.TabIndex = 120;
		this.label4.Text = resources.GetString("label4.Text");
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbsep01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbsep01.Location = new System.Drawing.Point(12, 353);
		this.lbsep01.Name = "lbsep01";
		this.lbsep01.Size = new System.Drawing.Size(743, 2);
		this.lbsep01.TabIndex = 126;
		this.pictureBox1.Image = Monitor.Resources.image_tool_disabled;
		this.pictureBox1.Location = new System.Drawing.Point(15, 365);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(24, 24);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 127;
		this.pictureBox1.TabStop = false;
		this.lbsep02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbsep02.Location = new System.Drawing.Point(14, 404);
		this.lbsep02.Name = "lbsep02";
		this.lbsep02.Size = new System.Drawing.Size(743, 2);
		this.lbsep02.TabIndex = 128;
		this.btImport.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btImport.ForeColor = System.Drawing.SystemColors.ControlText;
		this.btImport.Location = new System.Drawing.Point(138, 312);
		this.btImport.Name = "btImport";
		this.btImport.Size = new System.Drawing.Size(121, 34);
		this.btImport.TabIndex = 130;
		this.btImport.Text = "&Importar";
		this.btImport.UseVisualStyleBackColor = true;
		this.btImport.Click += new System.EventHandler(btImport_Click);
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(12, 77);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(743, 38);
		this.plnMessage.TabIndex = 131;
		this.plnMessage.Visible = false;
		this.lbProgress.AutoSize = true;
		this.lbProgress.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbProgress.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress.Location = new System.Drawing.Point(39, 11);
		this.lbProgress.Name = "lbProgress";
		this.lbProgress.Size = new System.Drawing.Size(15, 13);
		this.lbProgress.TabIndex = 103;
		this.lbProgress.Text = "..";
		this.lbProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbProgress01.AutoSize = true;
		this.lbProgress01.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbProgress01.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress01.Location = new System.Drawing.Point(39, 3);
		this.lbProgress01.Name = "lbProgress01";
		this.lbProgress01.Size = new System.Drawing.Size(15, 13);
		this.lbProgress01.TabIndex = 101;
		this.lbProgress01.Text = "..";
		this.lbProgress01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picProgress.Image = Monitor.Resources.gif_loading;
		this.picProgress.Location = new System.Drawing.Point(4, 3);
		this.picProgress.Name = "picProgress";
		this.picProgress.Size = new System.Drawing.Size(30, 30);
		this.picProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picProgress.TabIndex = 1;
		this.picProgress.TabStop = false;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(743, 2);
		this.label1.TabIndex = 132;
		this.lbSiscomexProfile.AutoSize = true;
		this.lbSiscomexProfile.Location = new System.Drawing.Point(10, 22);
		this.lbSiscomexProfile.Name = "lbSiscomexProfile";
		this.lbSiscomexProfile.Size = new System.Drawing.Size(183, 13);
		this.lbSiscomexProfile.TabIndex = 362;
		this.lbSiscomexProfile.Text = "Perfil de acesso ao Siscomex :";
		this.cbSiscomexProfile.BackColor = System.Drawing.SystemColors.Info;
		this.cbSiscomexProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbSiscomexProfile.FormattingEnabled = true;
		this.cbSiscomexProfile.Location = new System.Drawing.Point(204, 18);
		this.cbSiscomexProfile.Name = "cbSiscomexProfile";
		this.cbSiscomexProfile.Size = new System.Drawing.Size(321, 21);
		this.cbSiscomexProfile.TabIndex = 361;
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(14, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(743, 2);
		this.label3.TabIndex = 363;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(11, 57);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(179, 13);
		this.label5.TabIndex = 364;
		this.label5.Text = "Selecione o Certificado Digital";
		this.colIdent.Text = "CNPJ/CPF";
		this.colIdent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.colIdent.Width = 140;
		base.AcceptButton = this.btConfirmar;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(769, 413);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.lbSiscomexProfile);
		base.Controls.Add(this.cbSiscomexProfile);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.plnMessage);
		base.Controls.Add(this.btImport);
		base.Controls.Add(this.lbsep02);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.lbsep01);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.btRefresh);
		base.Controls.Add(this.lvCertific);
		base.Controls.Add(this.btConfirmar);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmCertComex";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Selecione o Certificado Digital";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCertComex_FormClosed);
		base.Shown += new System.EventHandler(frmCertComex_Shown);
		base.Leave += new System.EventHandler(frmCertComex_Leave);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
