using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using Monitor;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace monitor.fiscal.io;

public class frmDueExport : Form
{
	private clsComexProfile _clsComexProfile;

	private List<DueHeader> _DueHeaderList = new List<DueHeader>();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsDataDueLink _clsDataDueLink = new clsDataDueLink();

	private clsDataDocBinary _clsDataDocBin = new clsDataDocBinary();

	private clsZipService _clsZipService = new clsZipService();

	private clsDataDueItem _clsDataDueItem = new clsDataDueItem();

	private clsDataDueItemRem _clsDataDueItemRem = new clsDataDueItemRem();

	private clsDataConfig _clsDataConfig = new clsDataConfig();

	private IContainer components;

	private Button btStart;

	private ImageList imageList01;

	private Panel pnFolder;

	private Button btFolder;

	private TextBox txFolder;

	private Label lbFolder;

	private Panel pnForm;

	private Panel panel1;

	private Panel pnTitleBar;

	private PictureBox picTitleBar;

	private Button btHelp;

	private Button btClose;

	private Label lbTitleBar;

	private Label lbSep01;

	private ListView lsvStatus;

	private ColumnHeader colStatus;

	private CheckBox ckScanSiscomex;

	private Label lbAviso;

	private Label lbStatus;

	private CheckBox ckExportDFePdf;

	private CheckBox ckExportDFeXml;

	public frmDueExport(List<DueHeader> pDueHeaderList)
	{
		InitializeComponent();
		_DueHeaderList = pDueHeaderList;
		if (clsFunction.IsAdmin)
		{
			lbTitleBar.Text = base.Name + " | " + lbTitleBar.Text;
		}
	}

	private async void frmDueExport_Shown(object sender, EventArgs e)
	{
		await funcLoadUserDataAsync();
	}

	private async void btFolder_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog varFolderDialog = new FolderBrowserDialog();
		varFolderDialog.SelectedPath = clsFileManager.funcGetDefaultFolder(txFolder.Text);
		varFolderDialog.ShowDialog(this);
		if (!string.IsNullOrEmpty(varFolderDialog.SelectedPath))
		{
			txFolder.Text = varFolderDialog.SelectedPath;
		}
		varFolderDialog.Dispose();
		if (!clsFunction.IsEmpty(txFolder.Text))
		{
			await _clsDataParam.funcSetAsync("DueExport-InitialFolder", txFolder.Text);
		}
	}

	private async Task<bool> funcLoadUserDataAsync()
	{
		TextBox textBox = txFolder;
		textBox.Text = await _clsDataParam.funcGetAsync("DueExport-InitialFolder");
		txFolder.Text = clsFunction.funcGetFolderPath(txFolder.Text);
		return true;
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmDueExport_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		clsHelpService.funcCallExportarHelp();
	}

	private void btHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallExportarHelp();
	}

	private void lbTitleBar_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			clsScreenGeral.funcFormMouseMove(base.Handle);
		}
	}

	private ListViewItem funcGetListViewItem(string pStatus, string pDesc)
	{
		ListViewItem varListViewItem = new ListViewItem(pDesc);
		varListViewItem.Text = pDesc;
		varListViewItem = funcSetListViewItem(varListViewItem, pStatus);
		lsvStatus.Items.Add(varListViewItem);
		lsvStatus.FocusedItem = varListViewItem;
		lsvStatus.EnsureVisible(varListViewItem.Index);
		lsvStatus.Focus();
		return varListViewItem;
	}

	private ListViewItem funcSetListViewItem(ListViewItem pListViewItem, string pStatus, string pDesc)
	{
		if (!clsFunction.IsEmpty(pDesc))
		{
			pListViewItem.Text = pDesc;
		}
		pListViewItem = funcSetListViewItem(pListViewItem, pStatus);
		return pListViewItem;
	}

	private ListViewItem funcSetListViewItem(ListViewItem pListViewItem, string pStatus)
	{
		if (clsFunction.IsEqual(pStatus, "R", pIgnoreCase: true))
		{
			pListViewItem.ImageIndex = 0;
		}
		else if (clsFunction.IsEqual(pStatus, "S", pIgnoreCase: true))
		{
			pListViewItem.ImageIndex = 1;
		}
		else if (clsFunction.IsEqual(pStatus, "W", pIgnoreCase: true))
		{
			pListViewItem.ImageIndex = 2;
		}
		else if (clsFunction.IsEqual(pStatus, "E", pIgnoreCase: true))
		{
			pListViewItem.ImageIndex = 3;
		}
		pListViewItem.Focused = true;
		pListViewItem.Selected = true;
		Application.DoEvents();
		return pListViewItem;
	}

	private bool funcFormValidation()
	{
		string varMensagem = string.Empty;
		string varFolderPath = clsFunction.funcGetFolderPath(txFolder.Text);
		if (string.IsNullOrEmpty(varFolderPath))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Diretório não informado.";
		}
		else if (!Directory.Exists(varFolderPath))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Diretório informado não existe.";
		}
		if (string.IsNullOrEmpty(varMensagem))
		{
			return true;
		}
		MessageBox.Show(this, "Confirme os dados para continuar" + varMensagem, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		return false;
	}

	private async void btStart_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsComexApi varclsComexApi = new clsComexApi();
		clsXmlFactory varclsXmlFactory = new clsXmlFactory();
		clsPdfFactory varclsPdfFactory = new clsPdfFactory();
		new clsEdiFactory();
		try
		{
			lsvStatus.Items.Clear();
			lsvStatus.Focus();
			lbStatus.Text = string.Empty;
			if (!funcFormValidation())
			{
				return;
			}
			string varFolderRoot = txFolder.Text;
			Configuration varclsConfig = await _clsDataConfig.funcGetItemByKeyAsync();
			btStart.Enabled = false;
			btStart.Text = "Executando";
			int varCounter = 0;
			int varTotal = _DueHeaderList.Count;
			FilialView varclsFilial = new FilialView();
			foreach (DueHeader varclsDueHeader in _DueHeaderList)
			{
				varCounter++;
				lbStatus.Text = $"{varCounter} de {varTotal}";
				if (!clsFunction.IsEqual(varclsFilial.CNPJ, varclsDueHeader.Filial))
				{
					varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsDueHeader.Filial);
				}
				if (varclsFilial == null)
				{
					continue;
				}
				ListViewItem varListViewItem;
				string varLastCertSerial;
				if (ckScanSiscomex.Checked)
				{
					varLastCertSerial = _clsComexProfile?.CrtSerial;
					_clsComexProfile = await clsMonGeral.funcGetCertComexAsync(this, _clsComexProfile, varclsFilial);
					if (_clsComexProfile == null)
					{
						break;
					}
					if (!clsFunction.IsEqual(varLastCertSerial, _clsComexProfile?.CrtSerial))
					{
						varclsComexApi.IsConnected = false;
					}
					if (!varclsComexApi.IsConnected)
					{
						varListViewItem = funcGetListViewItem("R", "Conectando ao Portal Único : Siscomex ...");
						clsReturn varclsRetCert = await varclsComexApi.funcConnectAsync(_clsComexProfile);
						varclsReturnFunc.AddRange(varclsRetCert);
						if (varclsRetCert.HasError || varclsRetCert.HasWarning)
						{
							_clsComexProfile = null;
							return;
						}
						funcSetListViewItem(varListViewItem, "S", "Conexão no Portal Único realizada com sucesso ...");
					}
					varListViewItem = funcGetListViewItem("R", varclsDueHeader.Num + " : Consultando Siscomex...");
					clsReturn varclsRetItem = await varclsComexApi.funcGetDataByDueKeyLinkAsync(varclsFilial, varclsDueHeader.Num, pByTaskAction: false, pGetDossie: true);
					if (varclsRetItem.HasError)
					{
						funcSetListViewItem(varListViewItem, "E", varclsDueHeader.Num + " : " + varclsRetItem.funcGetMessageResult());
						break;
					}
					funcSetListViewItem(varListViewItem, "S", varclsDueHeader.Num + " : Consulta realizada com sucesso.");
				}
				string varDueFolderName = clsFunction.funcFixFilePath(varFolderRoot + "\\" + varclsDueHeader.Num);
				if (!new DirectoryInfo(varDueFolderName).Exists)
				{
					Directory.CreateDirectory(varDueFolderName);
				}
				varListViewItem = funcGetListViewItem("R", varclsDueHeader.Num + " : Obtendo documentos para exportação...");
				List<DueLink> varclsDocList = await _clsDataDueLink.funcGetListByNumAsync(varclsDueHeader.Num);
				funcSetListViewItem(varListViewItem, "S", varclsDueHeader.Num + " : Documentos obtidos para exportação...");
				foreach (DueLink varclsLink in varclsDocList)
				{
					funcGetListViewItem("S", varclsDueHeader.Num + " : Local " + varDueFolderName);
					using MemoryStream varMemoryStream = await _clsDataDocBin.funcGetAsync(varclsLink.DocLink, pUseRawName: true);
					if (varMemoryStream == null || varMemoryStream.Length == 0L)
					{
						continue;
					}
					varMemoryStream.Position = 0L;
					varListViewItem = funcGetListViewItem("R", varclsDueHeader.Num + " : Exportando " + varclsLink.DocType + ":" + varclsLink.DocKey);
					string varFileName = varDueFolderName + "\\" + varclsLink.DocType + "_" + varclsLink.DocKey;
					if (clsFunction.Contains(varclsLink.DocType, "DOSSIE", pIgnoreCase: true))
					{
						string varFolderDestin = clsFunction.funcFixFilePath(varFileName).ToLower();
						_clsZipService.funcUnzipFile(varMemoryStream, varFolderDestin);
					}
					else
					{
						varFileName = clsFunction.funcFixFilePath((varFileName + ".json").ToLower());
						using FileStream varFileStream = new FileStream(varFileName, FileMode.Create, FileAccess.Write);
						varMemoryStream.CopyTo(varFileStream);
					}
					funcSetListViewItem(varListViewItem, "S", varclsDueHeader.Num + " : Exportado " + varclsLink.DocType + ":" + varclsLink.DocKey);
				}
				string varDocType = string.Empty;
				if (ckExportDFeXml.Checked && ckExportDFePdf.Checked)
				{
					varDocType = "XMLs e PDFs";
				}
				else if (ckExportDFeXml.Checked)
				{
					varDocType = "XMLs";
				}
				else if (ckExportDFePdf.Checked)
				{
					varDocType = "PDFs";
				}
				if (clsFunction.IsEmpty(varDocType))
				{
					continue;
				}
				List<DueItem> varclsDueItemList = await _clsDataDueItem.funcGetListByKeyAsync(varclsDueHeader.Filial, varclsDueHeader.Num);
				if (varclsDueItemList.Count >= 0)
				{
					varListViewItem = funcGetListViewItem("R", varclsDueHeader.Num + " : NFes de Exportação : Exportando " + varDocType + "...");
					varLastCertSerial = clsFunction.funcFixFilePath(varDueFolderName + "\\NFesExportacao");
					if (!new DirectoryInfo(varLastCertSerial).Exists)
					{
						Directory.CreateDirectory(varLastCertSerial);
					}
					foreach (DueItem varclsDueItem in varclsDueItemList)
					{
						string varDocModel = clsFunction.funcGetDFeModel(varclsDueItem.ExpNFe);
						intXmlObject varXmlObject = varclsXmlFactory.funcGetDocClass(varclsConfig, varDocModel, pReload: false);
						if (ckExportDFeXml.Checked)
						{
							await varXmlObject.funcDownloadAsync(varclsFilial, varclsDueItem.ExpNFe, varLastCertSerial, null);
						}
						if (ckExportDFePdf.Checked)
						{
							await varclsPdfFactory.funcGetDocClass(varXmlObject, varDocModel).funcDownloadAsync(varclsFilial, varclsDueItem.ExpNFe, varLastCertSerial, null);
						}
					}
					funcSetListViewItem(varListViewItem, "S", varclsDueHeader.Num + " : NFes de Exportação : Exportadas com sucesso");
				}
				List<DueItemRem> varclsDueItemRemList = await _clsDataDueItemRem.funcGetListByKeyAsync(varclsDueHeader.Filial, varclsDueHeader.Num);
				if (varclsDueItemRemList.Count < 0)
				{
					continue;
				}
				varListViewItem = funcGetListViewItem("R", varclsDueHeader.Num + " : NFes de Remessa : Exportando " + varDocType + "...");
				varLastCertSerial = clsFunction.funcFixFilePath(varDueFolderName + "\\NFesRemessa");
				if (!new DirectoryInfo(varLastCertSerial).Exists)
				{
					Directory.CreateDirectory(varLastCertSerial);
				}
				foreach (DueItemRem varclsDueItemRem in varclsDueItemRemList)
				{
					string varDocModel = clsFunction.funcGetDFeModel(varclsDueItemRem.RemNFe);
					intXmlObject varXmlObject = varclsXmlFactory.funcGetDocClass(varclsConfig, varDocModel, pReload: false);
					if (ckExportDFeXml.Checked)
					{
						await varXmlObject.funcDownloadAsync(varclsFilial, varclsDueItemRem.RemNFe, varLastCertSerial, null);
					}
					if (ckExportDFePdf.Checked)
					{
						await varclsPdfFactory.funcGetDocClass(varXmlObject, varDocModel).funcDownloadAsync(varclsFilial, varclsDueItemRem.RemNFe, varLastCertSerial, null);
					}
				}
				funcSetListViewItem(varListViewItem, "S", varclsDueHeader.Num + " : NFes de Remessa : Exportadas com sucesso");
			}
			Process varSysProc = new Process();
			varSysProc.StartInfo.FileName = varFolderRoot;
			try
			{
				varSysProc.Start();
			}
			catch
			{
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			btStart.Enabled = true;
			btStart.Text = "Iniciar";
			lbStatus.Text = string.Empty;
			bool notSendToSoftError = true;
			if (varclsReturnFunc.Messages.Any((clsMessage r) => clsFunction.Contains(r.Message, "Autenticação", pIgnoreCase: true)))
			{
				_clsComexProfile = null;
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc, notSendToSoftError);
			}
			else
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
			}
		}
	}

	private void txFolder_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Directory.Exists(txFolder.Text))
			{
				Process process = new Process();
				process.StartInfo.FileName = txFolder.Text;
				process.Start();
			}
		}
		catch
		{
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(monitor.fiscal.io.frmDueExport));
		this.btFolder = new System.Windows.Forms.Button();
		this.txFolder = new System.Windows.Forms.TextBox();
		this.btStart = new System.Windows.Forms.Button();
		this.imageList01 = new System.Windows.Forms.ImageList(this.components);
		this.pnFolder = new System.Windows.Forms.Panel();
		this.lbFolder = new System.Windows.Forms.Label();
		this.pnForm = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ckExportDFePdf = new System.Windows.Forms.CheckBox();
		this.ckExportDFeXml = new System.Windows.Forms.CheckBox();
		this.lbStatus = new System.Windows.Forms.Label();
		this.lbAviso = new System.Windows.Forms.Label();
		this.ckScanSiscomex = new System.Windows.Forms.CheckBox();
		this.lbSep01 = new System.Windows.Forms.Label();
		this.lsvStatus = new System.Windows.Forms.ListView();
		this.colStatus = new System.Windows.Forms.ColumnHeader();
		this.pnTitleBar = new System.Windows.Forms.Panel();
		this.picTitleBar = new System.Windows.Forms.PictureBox();
		this.btHelp = new System.Windows.Forms.Button();
		this.btClose = new System.Windows.Forms.Button();
		this.lbTitleBar = new System.Windows.Forms.Label();
		this.pnFolder.SuspendLayout();
		this.pnForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.pnTitleBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).BeginInit();
		base.SuspendLayout();
		this.btFolder.Location = new System.Drawing.Point(582, 25);
		this.btFolder.Name = "btFolder";
		this.btFolder.Size = new System.Drawing.Size(27, 22);
		this.btFolder.TabIndex = 173;
		this.btFolder.Tag = "#FOLDER#SENDER";
		this.btFolder.Text = "...";
		this.btFolder.UseVisualStyleBackColor = true;
		this.btFolder.Click += new System.EventHandler(btFolder_Click);
		this.txFolder.BackColor = System.Drawing.SystemColors.Info;
		this.txFolder.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.txFolder.Location = new System.Drawing.Point(9, 26);
		this.txFolder.Name = "txFolder";
		this.txFolder.Size = new System.Drawing.Size(566, 21);
		this.txFolder.TabIndex = 0;
		this.txFolder.Tag = "";
		this.txFolder.DoubleClick += new System.EventHandler(txFolder_DoubleClick);
		this.btStart.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btStart.Location = new System.Drawing.Point(521, 66);
		this.btStart.Name = "btStart";
		this.btStart.Size = new System.Drawing.Size(102, 29);
		this.btStart.TabIndex = 12;
		this.btStart.Text = "&Iniciar";
		this.btStart.UseVisualStyleBackColor = true;
		this.btStart.Click += new System.EventHandler(btStart_Click);
		this.imageList01.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList01.ImageStream");
		this.imageList01.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList01.Images.SetKeyName(0, "image_working.png");
		this.imageList01.Images.SetKeyName(1, "dfe_confirm.png");
		this.imageList01.Images.SetKeyName(2, "image_warning.png");
		this.imageList01.Images.SetKeyName(3, "image_error.png");
		this.pnFolder.BackColor = System.Drawing.Color.White;
		this.pnFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnFolder.Controls.Add(this.lbFolder);
		this.pnFolder.Controls.Add(this.btFolder);
		this.pnFolder.Controls.Add(this.txFolder);
		this.pnFolder.Location = new System.Drawing.Point(3, 3);
		this.pnFolder.Name = "pnFolder";
		this.pnFolder.Size = new System.Drawing.Size(620, 57);
		this.pnFolder.TabIndex = 170;
		this.lbFolder.AutoSize = true;
		this.lbFolder.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFolder.Location = new System.Drawing.Point(9, 6);
		this.lbFolder.Name = "lbFolder";
		this.lbFolder.Size = new System.Drawing.Size(302, 13);
		this.lbFolder.TabIndex = 241;
		this.lbFolder.Tag = "";
		this.lbFolder.Text = "Informe o diretório onde deseja gravar os arquivos";
		this.pnForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnForm.Controls.Add(this.panel1);
		this.pnForm.Controls.Add(this.pnTitleBar);
		this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnForm.Location = new System.Drawing.Point(0, 0);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(642, 513);
		this.pnForm.TabIndex = 175;
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.Controls.Add(this.ckExportDFePdf);
		this.panel1.Controls.Add(this.ckExportDFeXml);
		this.panel1.Controls.Add(this.lbStatus);
		this.panel1.Controls.Add(this.lbAviso);
		this.panel1.Controls.Add(this.ckScanSiscomex);
		this.panel1.Controls.Add(this.lbSep01);
		this.panel1.Controls.Add(this.lsvStatus);
		this.panel1.Controls.Add(this.pnFolder);
		this.panel1.Controls.Add(this.btStart);
		this.panel1.Location = new System.Drawing.Point(6, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(628, 474);
		this.panel1.TabIndex = 8;
		this.ckExportDFePdf.AutoSize = true;
		this.ckExportDFePdf.Checked = true;
		this.ckExportDFePdf.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckExportDFePdf.Location = new System.Drawing.Point(10, 126);
		this.ckExportDFePdf.Name = "ckExportDFePdf";
		this.ckExportDFePdf.Size = new System.Drawing.Size(291, 17);
		this.ckExportDFePdf.TabIndex = 177;
		this.ckExportDFePdf.Text = "Exportar arquivos PDFs das NFes relacionados";
		this.ckExportDFePdf.UseVisualStyleBackColor = true;
		this.ckExportDFeXml.AutoSize = true;
		this.ckExportDFeXml.Checked = true;
		this.ckExportDFeXml.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckExportDFeXml.Location = new System.Drawing.Point(10, 102);
		this.ckExportDFeXml.Name = "ckExportDFeXml";
		this.ckExportDFeXml.Size = new System.Drawing.Size(292, 17);
		this.ckExportDFeXml.TabIndex = 176;
		this.ckExportDFeXml.Text = "Exportar arquivos XMLs das NFes relacionados";
		this.ckExportDFeXml.UseVisualStyleBackColor = true;
		this.lbStatus.AutoSize = true;
		this.lbStatus.Location = new System.Drawing.Point(438, 74);
		this.lbStatus.Name = "lbStatus";
		this.lbStatus.Size = new System.Drawing.Size(11, 13);
		this.lbStatus.TabIndex = 175;
		this.lbStatus.Text = ".";
		this.lbAviso.AutoSize = true;
		this.lbAviso.Font = new System.Drawing.Font("Verdana", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbAviso.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbAviso.Location = new System.Drawing.Point(8, 84);
		this.lbAviso.Name = "lbAviso";
		this.lbAviso.Size = new System.Drawing.Size(321, 12);
		this.lbAviso.TabIndex = 174;
		this.lbAviso.Text = "*O download dos anexos é registrado no histórico do siscomex";
		this.ckScanSiscomex.AutoSize = true;
		this.ckScanSiscomex.Checked = true;
		this.ckScanSiscomex.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckScanSiscomex.Location = new System.Drawing.Point(10, 67);
		this.ckScanSiscomex.Name = "ckScanSiscomex";
		this.ckScanSiscomex.Size = new System.Drawing.Size(288, 17);
		this.ckScanSiscomex.TabIndex = 173;
		this.ckScanSiscomex.Text = "Buscar informação mais recente do Siscomex";
		this.ckScanSiscomex.UseVisualStyleBackColor = true;
		this.lbSep01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSep01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbSep01.Location = new System.Drawing.Point(3, 147);
		this.lbSep01.Name = "lbSep01";
		this.lbSep01.Size = new System.Drawing.Size(620, 2);
		this.lbSep01.TabIndex = 172;
		this.lsvStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.colStatus });
		this.lsvStatus.FullRowSelect = true;
		this.lsvStatus.GridLines = true;
		this.lsvStatus.HideSelection = false;
		this.lsvStatus.LargeImageList = this.imageList01;
		this.lsvStatus.Location = new System.Drawing.Point(3, 155);
		this.lsvStatus.MultiSelect = false;
		this.lsvStatus.Name = "lsvStatus";
		this.lsvStatus.Size = new System.Drawing.Size(620, 314);
		this.lsvStatus.SmallImageList = this.imageList01;
		this.lsvStatus.TabIndex = 171;
		this.lsvStatus.UseCompatibleStateImageBehavior = false;
		this.lsvStatus.View = System.Windows.Forms.View.Details;
		this.colStatus.Text = "Status do processamento";
		this.colStatus.Width = 590;
		this.pnTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnTitleBar.BackColor = System.Drawing.Color.White;
		this.pnTitleBar.Controls.Add(this.picTitleBar);
		this.pnTitleBar.Controls.Add(this.btHelp);
		this.pnTitleBar.Controls.Add(this.btClose);
		this.pnTitleBar.Controls.Add(this.lbTitleBar);
		this.pnTitleBar.Location = new System.Drawing.Point(0, 0);
		this.pnTitleBar.Name = "pnTitleBar";
		this.pnTitleBar.Size = new System.Drawing.Size(641, 31);
		this.pnTitleBar.TabIndex = 2;
		this.picTitleBar.Image = Monitor.Resources.image_favicon;
		this.picTitleBar.Location = new System.Drawing.Point(5, 4);
		this.picTitleBar.Name = "picTitleBar";
		this.picTitleBar.Size = new System.Drawing.Size(23, 23);
		this.picTitleBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picTitleBar.TabIndex = 3;
		this.picTitleBar.TabStop = false;
		this.btHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btHelp.BackColor = System.Drawing.Color.Transparent;
		this.btHelp.BackgroundImage = Monitor.Resources.image_form_help;
		this.btHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btHelp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.btHelp.FlatAppearance.BorderSize = 0;
		this.btHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btHelp.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btHelp.ForeColor = System.Drawing.Color.White;
		this.btHelp.Location = new System.Drawing.Point(530, 3);
		this.btHelp.Name = "btHelp";
		this.btHelp.Size = new System.Drawing.Size(75, 24);
		this.btHelp.TabIndex = 2;
		this.btHelp.UseVisualStyleBackColor = false;
		this.btHelp.Click += new System.EventHandler(btHelp_Click);
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(611, 2);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 1;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lbTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTitleBar.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitleBar.Location = new System.Drawing.Point(32, 5);
		this.lbTitleBar.Name = "lbTitleBar";
		this.lbTitleBar.Size = new System.Drawing.Size(492, 20);
		this.lbTitleBar.TabIndex = 0;
		this.lbTitleBar.Text = "Fiscal.io : Consulta e exportação de DUEs, Dossie, LPCO, XML, Danfe, etc";
		this.lbTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(lbTitleBar_MouseDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(642, 513);
		base.Controls.Add(this.pnForm);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmDueExport";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Fiscal.io : Consulta e exportação de DUEs, Dossie, LPCO, XML, Danfe, etc";
		base.Shown += new System.EventHandler(frmDueExport_Shown);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(frmDueExport_HelpRequested);
		this.pnFolder.ResumeLayout(false);
		this.pnFolder.PerformLayout();
		this.pnForm.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.pnTitleBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).EndInit();
		base.ResumeLayout(false);
	}
}
