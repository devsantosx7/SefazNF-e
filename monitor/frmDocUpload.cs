using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmDocUpload : Form
{
	private clsFeatureService varclsFeatureService = new clsFeatureService();

	private clsSoftwareService varclsSoftwareService = new clsSoftwareService(null);

	private clsDataParameter varclsDataParam = new clsDataParameter();

	private bool varLoaded;

	private string varFeatSubDirect = string.Empty;

	private string varFeatXmlValJur = string.Empty;

	private IContainer components;

	private ImageList ImageListDocs;

	private Button btSair;

	private Button btImportXml;

	private Label lbSeparator05;

	private ToolTip toolTipInfAdd;

	private Label lbSeparator01;

	private Label lbFolder_FolderIn06;

	private Label lbFolder_FolderIn04;

	private Label lbFolder_FolderIn05;

	private Label lbFolder_FolderIn03;

	private CheckBox ckFolder_SubDirIn;

	private ComboBox cbFolder_ActionIn;

	private Label lbFolder_FolderIn02;

	private Button btFolder_FolderIn;

	private Label lbFolder_FolderIn01;

	private TextBox txFolder_FolderIn;

	private Label lbSeparator03;

	private PictureBox picDocInXmlJurValLocker;

	private CheckBox ckDocInXmlJurVal;

	private PictureBox picFolder_SubDirInLocker;

	private Label lbSeparator02;

	private Panel panel1;

	private Label label6;

	private Label label7;

	private Label label9;

	private Label label10;

	private Panel panel2;

	private ComboBox cbDocInTagValue;

	private Label lbDocInTagValue01;

	private Label lbDocInTagValue02;

	private Label label13;

	private Button btDocInTagValue;

	private CheckBox ckDocInTagReplc;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	public frmDocUpload()
	{
		InitializeComponent();
	}

	private async void frmDocUpload_Load(object sender, EventArgs e)
	{
		varLoaded = false;
		await funcLoadTagListAsync(cbDocInTagValue);
		if (await varclsSoftwareService.funcHasPayedPlanAsync())
		{
			varFeatSubDirect = "PAYED";
		}
		else
		{
			varFeatSubDirect = "LOCKED";
		}
		funcLoadFolderActionIn();
		varFeatXmlValJur = await varclsFeatureService.funcGetFeatTypeAsync(clsFeatureService.consFeatXmlJuridicValidation);
		ckFolder_SubDirIn.Checked = true;
		ckDocInXmlJurVal.Checked = true;
		if (clsFunction.Contains(varFeatSubDirect, "LOCK"))
		{
			picFolder_SubDirInLocker.Visible = true;
		}
		else
		{
			picFolder_SubDirInLocker.Visible = false;
		}
		if (clsFunction.Contains(varFeatSubDirect, "LOCK"))
		{
			ckFolder_SubDirIn.Checked = false;
		}
		if (clsFunction.Contains(varFeatXmlValJur, "LOCK"))
		{
			picDocInXmlJurValLocker.Visible = true;
		}
		else
		{
			picDocInXmlJurValLocker.Visible = false;
		}
		if (varFeatXmlValJur.Contains("LOCK"))
		{
			ckDocInXmlJurVal.Checked = false;
		}
		await clsScreenGeral.funcGetUserDataAsync(this, base.Controls);
		varLoaded = true;
	}

	private async Task<bool> funcLoadTagListAsync(ComboBox pComboBox)
	{
		pComboBox.DisplayMember = "Nome";
		pComboBox.ValueMember = "Code";
		List<Tag> varTagList = await new clsDataTag().funcGetListAsync(pJustWithAccess: true);
		varTagList.Add(new Tag
		{
			Code = "",
			Nome = ""
		});
		pComboBox.DataSource = varTagList;
		pComboBox.SelectedIndex = -1;
		return true;
	}

	private void funcLoadFolderActionIn()
	{
		List<clsObjectType> varList = new List<clsObjectType>();
		varList.Add(new clsObjectType
		{
			Value = "DELE",
			Name = "Excluir arquivo do Diretório"
		});
		varList.Add(new clsObjectType
		{
			Value = "MOVE",
			Name = "Mover arquivo para [Processados]"
		});
		if (clsFunction.Contains(varFeatSubDirect, "LOCK"))
		{
			varList.Add(new clsObjectType
			{
				Value = "KEEP",
				Name = "Manter arquivo no Diretório [*Incluso nos Planos*]"
			});
		}
		else
		{
			varList.Add(new clsObjectType
			{
				Value = "KEEP",
				Name = "Manter arquivo no Diretório"
			});
		}
		cbFolder_ActionIn.DisplayMember = "Name";
		cbFolder_ActionIn.ValueMember = "Value";
		cbFolder_ActionIn.DataSource = varList;
		cbFolder_ActionIn.SelectedValue = string.Empty;
	}

	private async void btFolder_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog varFolderDialog = new FolderBrowserDialog();
		FolderBrowserDialog folderBrowserDialog = varFolderDialog;
		folderBrowserDialog.SelectedPath = await clsScreenGeral.funcGetDefaultFolderAsync(this);
		varFolderDialog.ShowDialog(this);
		await clsScreenGeral.funcSetDefaultFolderAsync(this, varFolderDialog.SelectedPath);
		if (!string.IsNullOrEmpty(varFolderDialog.SelectedPath))
		{
			txFolder_FolderIn.Text = varFolderDialog.SelectedPath;
		}
		varFolderDialog.Dispose();
	}

	private void btSair_Click(object sender, EventArgs e)
	{
		Close();
	}

	private async void ckDocInXmlJurVal_CheckedChanged(object sender, EventArgs e)
	{
		if (varLoaded && ckDocInXmlJurVal.Checked)
		{
			await funCheckBoxFeatureAsync(ckDocInXmlJurVal, clsFeatureService.consFeatXmlJuridicValidation);
		}
	}

	private async void picDocInXmlJurValLocker_Click(object sender, EventArgs e)
	{
		await funcPicFeatureCheckAsync(clsFeatureService.consFeatXmlJuridicValidation);
	}

	private async void ckFolder_SubDirIn_CheckedChanged(object sender, EventArgs e)
	{
		if (varLoaded && ckFolder_SubDirIn.Checked)
		{
			await funCheckBoxFeatureAsync(ckFolder_SubDirIn, string.Empty);
		}
	}

	private async void picFolder_SubDirInLocker_Click(object sender, EventArgs e)
	{
		await funcPicFeatureCheckAsync(string.Empty);
	}

	private async void picDocInValdQueryLocker_Click(object sender, EventArgs e)
	{
		await funcPicFeatureCheckAsync(clsFeatureService.consStatusDFeWhSec);
	}

	private async Task<bool> funCheckBoxFeatureAsync(CheckBox pCheckBox, string pFeatExtId)
	{
		if (!pCheckBox.Checked)
		{
			return true;
		}
		string varFeatType = string.Empty;
		if (string.IsNullOrEmpty(pFeatExtId))
		{
			if (!(await varclsSoftwareService.funcHasPayedPlanAsync()))
			{
				varFeatType = "LOCK";
			}
		}
		else
		{
			await new clsDataParameter().funcAddCounterAsync(pFeatExtId + "-CLICKS");
			varFeatType = await varclsFeatureService.funcGetFeatTypeAsync(pFeatExtId);
		}
		if (!varFeatType.Contains("LOCK"))
		{
			return true;
		}
		pCheckBox.Checked = false;
		await new clsManGeral().funcGetSalesActionAsync(this, pFeatExtId);
		return true;
	}

	private async Task<bool> funcPicFeatureCheckAsync(string pFeatExtId)
	{
		string varFeatType = string.Empty;
		if (string.IsNullOrEmpty(pFeatExtId))
		{
			if (!(await varclsSoftwareService.funcHasPayedPlanAsync()))
			{
				varFeatType = "LOCK";
			}
		}
		else
		{
			await new clsDataParameter().funcAddCounterAsync(pFeatExtId + "-CLICKS");
			varFeatType = await varclsFeatureService.funcGetFeatTypeAsync(pFeatExtId);
		}
		if (!varFeatType.Contains("LOCK"))
		{
			return true;
		}
		await new clsManGeral().funcGetSalesActionAsync(this, pFeatExtId);
		return true;
	}

	private bool funcFormValidation()
	{
		string varMensagem = string.Empty;
		if (string.IsNullOrEmpty(txFolder_FolderIn.Text))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> O Diretório de origem não informado.";
		}
		if (clsFunction.IsEmpty(clsFunction.funcGetValue(cbFolder_ActionIn.SelectedValue)))
		{
			varMensagem = varMensagem + Environment.NewLine + "--> [Ação após processamento do XML] não informada.";
		}
		if (string.IsNullOrEmpty(varMensagem))
		{
			return true;
		}
		MessageBox.Show(this, "Confirme os dados para continuar" + varMensagem, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		return false;
	}

	private async void btImportXml_Click(object sender, EventArgs e)
	{
		btImportXml.Enabled = false;
		lbProgress.Text = "Em Processamento...";
		plnMessage.Visible = true;
		clsDataChannel varclsDataChannel = new clsDataChannel();
		clsChannelManager varclsChannelManager = new clsChannelManager();
		if (!funcFormValidation())
		{
			plnMessage.Visible = false;
			btImportXml.Enabled = true;
			return;
		}
		Channel varclsChannel = new Channel();
		varclsChannel = funcGetFormData(varclsChannel);
		if (await varclsDataChannel.funcGetItemByPropTypeFolderIn(varclsChannel.Proposal, varclsChannel.TypeInt, varclsChannel.Folder_FolderIn, varclsChannel.Machine) != null)
		{
			MessageBox.Show(this, "Já existe um canal de integração com mesmo diretório de origem.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			plnMessage.Visible = false;
			btImportXml.Enabled = true;
			return;
		}
		clsDataBatchHead clsDataBatchHead = new clsDataBatchHead();
		BatchHead varclsBatchHead = new BatchHead
		{
			BthAction = "INTEGRACAO",
			BthStatus = "PLANNED",
			BthItems = "0",
			BthPerc = "0",
			BthDescrt = "Importação inteligente de arquivos XML",
			BthTasker = "X"
		};
		varclsBatchHead = await clsDataBatchHead.funcInsertAsync(varclsBatchHead);
		varclsChannel.BatchNum = varclsBatchHead.BthId;
		varclsChannel.ID = Guid.NewGuid().ToString().ToUpper();
		await new clsDataChannel().funcInsertAsync(varclsChannel);
		await varclsChannelManager.funcSyncInboundAsync();
		await clsScreenGeral.funcSetUserDataAsync(this, base.Controls);
		plnMessage.Visible = false;
		MessageBox.Show(string.Concat(string.Concat("Atividade adicionada a fila de processamento!" + Environment.NewLine + Environment.NewLine, "Consulte o andamento através do [Gerenciador de Tarefas] -> Lotes", Environment.NewLine, Environment.NewLine), "Lote de processamento : ", varclsBatchHead.BthId, Environment.NewLine), "Tarefa programada com sucesso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		Close();
	}

	private Channel funcGetFormData(Channel pclsChannel)
	{
		pclsChannel.Proposal = "RECEIV";
		pclsChannel.TypeInt = "FOLDER";
		pclsChannel.Frequency = "5";
		pclsChannel.RunOnServer = string.Empty;
		pclsChannel.Machine = Environment.MachineName;
		pclsChannel.Folder_FolderOut = string.Empty;
		pclsChannel.Folder_FolderIn = txFolder_FolderIn.Text;
		pclsChannel.Folder_SubDirIn = clsFunction.funcConvBoolToStr(ckFolder_SubDirIn.Checked);
		pclsChannel.Folder_ActionIn = (string)cbFolder_ActionIn.SelectedValue;
		pclsChannel.Folder_RCopyFull = "X";
		pclsChannel.NFeDoc = string.Empty;
		string text = (pclsChannel.NFeDist = "X");
		string nFeDocTom = (pclsChannel.NFeDocTer = text);
		pclsChannel.NFeDocTom = nFeDocTom;
		text = (pclsChannel.NFCeEvt = "X");
		nFeDocTom = (pclsChannel.NFCeDoc = text);
		pclsChannel.NFeEvt = nFeDocTom;
		pclsChannel.CTeDoc = string.Empty;
		text = (pclsChannel.CTeEvt = "X");
		nFeDocTom = (pclsChannel.CTeDocTer = text);
		pclsChannel.CTeDocTom = nFeDocTom;
		text = (pclsChannel.DocIn = "X");
		nFeDocTom = (pclsChannel.MDFeEvt = text);
		pclsChannel.MDFeDoc = nFeDocTom;
		nFeDocTom = (pclsChannel.CFeSatEvt = "X");
		pclsChannel.CFeSatDoc = nFeDocTom;
		nFeDocTom = (pclsChannel.NFSeEvt = "X");
		pclsChannel.NFSeDoc = nFeDocTom;
		pclsChannel.DocOut = "X";
		pclsChannel.ForAllCompanies = "X";
		pclsChannel.EventFilter = "Todos";
		pclsChannel.DocInXmlJurVal = clsFunction.funcConvBoolToStr(ckDocInXmlJurVal.Checked);
		pclsChannel.DocInTagValue = clsFunction.funcGetValue(cbDocInTagValue.SelectedValue);
		pclsChannel.DocInTagReplc = clsFunction.funcConvBoolToStr(ckDocInTagReplc.Checked);
		pclsChannel.Description = "Receber XML de " + pclsChannel.Folder_FolderIn;
		pclsChannel.ChannelAdHoc = "X";
		return pclsChannel;
	}

	private async void cbFolder_ActionIn_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (varLoaded)
		{
			string varSelected = (string)cbFolder_ActionIn.SelectedValue;
			varSelected = clsFunction.funcGetValue(varSelected);
			if (varSelected.Equals("KEEP") && varFeatSubDirect.Contains("LOCK"))
			{
				await funcPicFeatureCheckAsync(string.Empty);
				cbFolder_ActionIn.SelectedValue = string.Empty;
			}
			else if (varSelected.Equals("DELE") && MessageBox.Show(this, "ATENÇÃO !!!! " + Environment.NewLine + "Após o processamento dos arquivos, os mesmos serão excluídos da" + Environment.NewLine + "pasta : " + txFolder_FolderIn.Text + Environment.NewLine + Environment.NewLine + "TEM CERTEZA que deseja esta ação ?", "Fiscal.io - Confirmação de Dados", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				cbFolder_ActionIn.SelectedValue = string.Empty;
			}
		}
	}

	private void frmDocUpload_Shown(object sender, EventArgs e)
	{
		txFolder_FolderIn.Focus();
	}

	private async void btDocInTagValue_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("TAG-MANAGER"))
		{
			frmTags frmTags = new frmTags();
			frmTags.ShowDialog();
			frmTags.Dispose();
			object varTagCode = cbDocInTagValue.SelectedValue;
			await funcLoadTagListAsync(cbDocInTagValue);
			cbDocInTagValue.SelectedValue = clsFunction.funcGetValue(varTagCode);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmDocUpload));
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.btSair = new System.Windows.Forms.Button();
		this.btImportXml = new System.Windows.Forms.Button();
		this.lbSeparator05 = new System.Windows.Forms.Label();
		this.toolTipInfAdd = new System.Windows.Forms.ToolTip(this.components);
		this.lbFolder_FolderIn05 = new System.Windows.Forms.Label();
		this.lbFolder_FolderIn03 = new System.Windows.Forms.Label();
		this.ckDocInTagReplc = new System.Windows.Forms.CheckBox();
		this.lbSeparator01 = new System.Windows.Forms.Label();
		this.lbFolder_FolderIn06 = new System.Windows.Forms.Label();
		this.lbFolder_FolderIn04 = new System.Windows.Forms.Label();
		this.ckFolder_SubDirIn = new System.Windows.Forms.CheckBox();
		this.cbFolder_ActionIn = new System.Windows.Forms.ComboBox();
		this.lbFolder_FolderIn02 = new System.Windows.Forms.Label();
		this.btFolder_FolderIn = new System.Windows.Forms.Button();
		this.lbFolder_FolderIn01 = new System.Windows.Forms.Label();
		this.txFolder_FolderIn = new System.Windows.Forms.TextBox();
		this.lbSeparator03 = new System.Windows.Forms.Label();
		this.picDocInXmlJurValLocker = new System.Windows.Forms.PictureBox();
		this.ckDocInXmlJurVal = new System.Windows.Forms.CheckBox();
		this.picFolder_SubDirInLocker = new System.Windows.Forms.PictureBox();
		this.lbSeparator02 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btDocInTagValue = new System.Windows.Forms.Button();
		this.cbDocInTagValue = new System.Windows.Forms.ComboBox();
		this.lbDocInTagValue01 = new System.Windows.Forms.Label();
		this.lbDocInTagValue02 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.picDocInXmlJurValLocker).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picFolder_SubDirInLocker).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		base.SuspendLayout();
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "tobesend.png");
		this.ImageListDocs.Images.SetKeyName(1, "ok.png");
		this.ImageListDocs.Images.SetKeyName(2, "error.png");
		this.btSair.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btSair.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btSair.Location = new System.Drawing.Point(5, 359);
		this.btSair.Name = "btSair";
		this.btSair.Size = new System.Drawing.Size(117, 37);
		this.btSair.TabIndex = 8;
		this.btSair.Text = "&Cancelar";
		this.btSair.UseVisualStyleBackColor = true;
		this.btSair.Click += new System.EventHandler(btSair_Click);
		this.btImportXml.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btImportXml.ForeColor = System.Drawing.Color.Black;
		this.btImportXml.Location = new System.Drawing.Point(484, 358);
		this.btImportXml.Name = "btImportXml";
		this.btImportXml.Size = new System.Drawing.Size(117, 37);
		this.btImportXml.TabIndex = 7;
		this.btImportXml.Text = "&Confirmar";
		this.btImportXml.UseVisualStyleBackColor = true;
		this.btImportXml.Click += new System.EventHandler(btImportXml_Click);
		this.lbSeparator05.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator05.Location = new System.Drawing.Point(5, 349);
		this.lbSeparator05.Name = "lbSeparator05";
		this.lbSeparator05.Size = new System.Drawing.Size(596, 5);
		this.lbSeparator05.TabIndex = 227;
		this.toolTipInfAdd.AutomaticDelay = 50000;
		this.toolTipInfAdd.IsBalloon = true;
		this.toolTipInfAdd.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTipInfAdd.ToolTipTitle = "Ajuda";
		this.lbFolder_FolderIn05.AutoSize = true;
		this.lbFolder_FolderIn05.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbFolder_FolderIn05.Location = new System.Drawing.Point(11, 164);
		this.lbFolder_FolderIn05.Name = "lbFolder_FolderIn05";
		this.lbFolder_FolderIn05.Size = new System.Drawing.Size(496, 13);
		this.lbFolder_FolderIn05.TabIndex = 245;
		this.lbFolder_FolderIn05.Tag = "#FOLDER#SENDER";
		this.lbFolder_FolderIn05.Text = "*Arquivos XML sem vinculo a empresas serão movidos para o diretório [SemVinculo]";
		this.toolTipInfAdd.SetToolTip(this.lbFolder_FolderIn05, "Arquivos XML que não tem o CNPJ de algumas das empresas cadastradas no Fiscal.io Monitor");
		this.lbFolder_FolderIn03.AutoSize = true;
		this.lbFolder_FolderIn03.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbFolder_FolderIn03.Location = new System.Drawing.Point(11, 137);
		this.lbFolder_FolderIn03.Name = "lbFolder_FolderIn03";
		this.lbFolder_FolderIn03.Size = new System.Drawing.Size(496, 13);
		this.lbFolder_FolderIn03.TabIndex = 244;
		this.lbFolder_FolderIn03.Tag = "#FOLDER#SENDER";
		this.lbFolder_FolderIn03.Text = "*Arquivos XML com formato desconhecido serão movidos para o diretório [Invalidos]";
		this.toolTipInfAdd.SetToolTip(this.lbFolder_FolderIn03, "Arquivos XML que não estão no layout da NFe, CTe, MDFe, NFCe e Eventos relacionados conforme padrão estabelecido pelo Governo Federal.");
		this.ckDocInTagReplc.AutoSize = true;
		this.ckDocInTagReplc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ckDocInTagReplc.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ckDocInTagReplc.Location = new System.Drawing.Point(478, 257);
		this.ckDocInTagReplc.Name = "ckDocInTagReplc";
		this.ckDocInTagReplc.Size = new System.Drawing.Size(102, 17);
		this.ckDocInTagReplc.TabIndex = 288;
		this.ckDocInTagReplc.Text = "sobregravar?";
		this.ckDocInTagReplc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.toolTipInfAdd.SetToolTip(this.ckDocInTagReplc, "Se o documento já existir no Monitor com uma etiqueta, \r\nela será substituida pela nova etiqueta informada nesta tela.");
		this.ckDocInTagReplc.UseVisualStyleBackColor = true;
		this.lbSeparator01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator01.Location = new System.Drawing.Point(14, 6);
		this.lbSeparator01.Name = "lbSeparator01";
		this.lbSeparator01.Size = new System.Drawing.Size(567, 2);
		this.lbSeparator01.TabIndex = 234;
		this.lbFolder_FolderIn06.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbFolder_FolderIn06.Location = new System.Drawing.Point(101, 180);
		this.lbFolder_FolderIn06.Name = "lbFolder_FolderIn06";
		this.lbFolder_FolderIn06.Size = new System.Drawing.Size(143, 2);
		this.lbFolder_FolderIn06.TabIndex = 246;
		this.lbFolder_FolderIn04.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbFolder_FolderIn04.Location = new System.Drawing.Point(101, 152);
		this.lbFolder_FolderIn04.Name = "lbFolder_FolderIn04";
		this.lbFolder_FolderIn04.Size = new System.Drawing.Size(156, 2);
		this.lbFolder_FolderIn04.TabIndex = 241;
		this.ckFolder_SubDirIn.AutoSize = true;
		this.ckFolder_SubDirIn.Location = new System.Drawing.Point(14, 58);
		this.ckFolder_SubDirIn.Name = "ckFolder_SubDirIn";
		this.ckFolder_SubDirIn.Size = new System.Drawing.Size(321, 17);
		this.ckFolder_SubDirIn.TabIndex = 2;
		this.ckFolder_SubDirIn.Text = "Realizar a leitura de arquivos XML em subdiretórios";
		this.ckFolder_SubDirIn.UseVisualStyleBackColor = true;
		this.ckFolder_SubDirIn.CheckedChanged += new System.EventHandler(ckFolder_SubDirIn_CheckedChanged);
		this.cbFolder_ActionIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbFolder_ActionIn.FormattingEnabled = true;
		this.cbFolder_ActionIn.Location = new System.Drawing.Point(14, 109);
		this.cbFolder_ActionIn.Name = "cbFolder_ActionIn";
		this.cbFolder_ActionIn.Size = new System.Drawing.Size(493, 21);
		this.cbFolder_ActionIn.TabIndex = 2;
		this.cbFolder_ActionIn.Tag = "#FOLDER#SENDER";
		this.cbFolder_ActionIn.SelectedIndexChanged += new System.EventHandler(cbFolder_ActionIn_SelectedIndexChanged);
		this.lbFolder_FolderIn02.AutoSize = true;
		this.lbFolder_FolderIn02.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbFolder_FolderIn02.Location = new System.Drawing.Point(13, 91);
		this.lbFolder_FolderIn02.Name = "lbFolder_FolderIn02";
		this.lbFolder_FolderIn02.Size = new System.Drawing.Size(276, 13);
		this.lbFolder_FolderIn02.TabIndex = 243;
		this.lbFolder_FolderIn02.Tag = "";
		this.lbFolder_FolderIn02.Text = "Ação após processamento do XML pelo Monitor";
		this.btFolder_FolderIn.Location = new System.Drawing.Point(558, 29);
		this.btFolder_FolderIn.Name = "btFolder_FolderIn";
		this.btFolder_FolderIn.Size = new System.Drawing.Size(27, 22);
		this.btFolder_FolderIn.TabIndex = 1;
		this.btFolder_FolderIn.Tag = "#FOLDER#SENDER";
		this.btFolder_FolderIn.Text = "...";
		this.btFolder_FolderIn.UseVisualStyleBackColor = true;
		this.btFolder_FolderIn.Click += new System.EventHandler(btFolder_Click);
		this.lbFolder_FolderIn01.AutoSize = true;
		this.lbFolder_FolderIn01.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFolder_FolderIn01.Location = new System.Drawing.Point(13, 14);
		this.lbFolder_FolderIn01.Name = "lbFolder_FolderIn01";
		this.lbFolder_FolderIn01.Size = new System.Drawing.Size(404, 13);
		this.lbFolder_FolderIn01.TabIndex = 240;
		this.lbFolder_FolderIn01.Tag = "";
		this.lbFolder_FolderIn01.Text = "Informe o diretório onde o Fiscal.io buscará os arquivos XML";
		this.txFolder_FolderIn.BackColor = System.Drawing.SystemColors.Info;
		this.txFolder_FolderIn.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.txFolder_FolderIn.Location = new System.Drawing.Point(14, 30);
		this.txFolder_FolderIn.Name = "txFolder_FolderIn";
		this.txFolder_FolderIn.Size = new System.Drawing.Size(540, 21);
		this.txFolder_FolderIn.TabIndex = 0;
		this.txFolder_FolderIn.Tag = "";
		this.lbSeparator03.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator03.Location = new System.Drawing.Point(14, 191);
		this.lbSeparator03.Name = "lbSeparator03";
		this.lbSeparator03.Size = new System.Drawing.Size(567, 2);
		this.lbSeparator03.TabIndex = 249;
		this.picDocInXmlJurValLocker.Image = (System.Drawing.Image)resources.GetObject("picDocInXmlJurValLocker.Image");
		this.picDocInXmlJurValLocker.Location = new System.Drawing.Point(563, 203);
		this.picDocInXmlJurValLocker.Name = "picDocInXmlJurValLocker";
		this.picDocInXmlJurValLocker.Size = new System.Drawing.Size(17, 16);
		this.picDocInXmlJurValLocker.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picDocInXmlJurValLocker.TabIndex = 251;
		this.picDocInXmlJurValLocker.TabStop = false;
		this.picDocInXmlJurValLocker.Click += new System.EventHandler(picDocInXmlJurValLocker_Click);
		this.ckDocInXmlJurVal.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ckDocInXmlJurVal.Location = new System.Drawing.Point(14, 202);
		this.ckDocInXmlJurVal.Name = "ckDocInXmlJurVal";
		this.ckDocInXmlJurVal.Size = new System.Drawing.Size(540, 17);
		this.ckDocInXmlJurVal.TabIndex = 3;
		this.ckDocInXmlJurVal.Text = "Verificar a validade jurídica dos XMLs durante a importação";
		this.ckDocInXmlJurVal.UseVisualStyleBackColor = true;
		this.ckDocInXmlJurVal.CheckedChanged += new System.EventHandler(ckDocInXmlJurVal_CheckedChanged);
		this.picFolder_SubDirInLocker.Image = (System.Drawing.Image)resources.GetObject("picFolder_SubDirInLocker.Image");
		this.picFolder_SubDirInLocker.Location = new System.Drawing.Point(563, 59);
		this.picFolder_SubDirInLocker.Name = "picFolder_SubDirInLocker";
		this.picFolder_SubDirInLocker.Size = new System.Drawing.Size(17, 16);
		this.picFolder_SubDirInLocker.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFolder_SubDirInLocker.TabIndex = 255;
		this.picFolder_SubDirInLocker.TabStop = false;
		this.picFolder_SubDirInLocker.Click += new System.EventHandler(picFolder_SubDirInLocker_Click);
		this.lbSeparator02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator02.Location = new System.Drawing.Point(14, 83);
		this.lbSeparator02.Name = "lbSeparator02";
		this.lbSeparator02.Size = new System.Drawing.Size(567, 2);
		this.lbSeparator02.TabIndex = 263;
		this.panel1.BackColor = System.Drawing.SystemColors.Info;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.label10);
		this.panel1.Location = new System.Drawing.Point(5, 7);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(596, 48);
		this.panel1.TabIndex = 270;
		this.label6.AutoSize = true;
		this.label6.ForeColor = System.Drawing.Color.Blue;
		this.label6.Location = new System.Drawing.Point(84, 46);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(0, 13);
		this.label6.TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.ForeColor = System.Drawing.Color.Blue;
		this.label7.Location = new System.Drawing.Point(84, 25);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(473, 13);
		this.label7.TabIndex = 2;
		this.label7.Text = "Garanta uma base de dados integra e em conformidade com a legislação vigente";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(84, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(492, 13);
		this.label9.TabIndex = 1;
		this.label9.Text = "Automatizar a importação, validação juridíca e consulta de status e eventos de XMLs";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(70, 13);
		this.label10.TabIndex = 0;
		this.label10.Text = "Objetivo: ";
		this.panel2.BackColor = System.Drawing.SystemColors.Window;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.ckDocInTagReplc);
		this.panel2.Controls.Add(this.btDocInTagValue);
		this.panel2.Controls.Add(this.cbDocInTagValue);
		this.panel2.Controls.Add(this.lbDocInTagValue01);
		this.panel2.Controls.Add(this.lbDocInTagValue02);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.lbFolder_FolderIn01);
		this.panel2.Controls.Add(this.lbSeparator01);
		this.panel2.Controls.Add(this.txFolder_FolderIn);
		this.panel2.Controls.Add(this.btFolder_FolderIn);
		this.panel2.Controls.Add(this.lbFolder_FolderIn02);
		this.panel2.Controls.Add(this.lbSeparator02);
		this.panel2.Controls.Add(this.cbFolder_ActionIn);
		this.panel2.Controls.Add(this.ckFolder_SubDirIn);
		this.panel2.Controls.Add(this.lbFolder_FolderIn03);
		this.panel2.Controls.Add(this.picFolder_SubDirInLocker);
		this.panel2.Controls.Add(this.lbFolder_FolderIn05);
		this.panel2.Controls.Add(this.lbFolder_FolderIn04);
		this.panel2.Controls.Add(this.picDocInXmlJurValLocker);
		this.panel2.Controls.Add(this.lbFolder_FolderIn06);
		this.panel2.Controls.Add(this.ckDocInXmlJurVal);
		this.panel2.Controls.Add(this.lbSeparator03);
		this.panel2.Location = new System.Drawing.Point(5, 54);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(596, 290);
		this.panel2.TabIndex = 271;
		this.btDocInTagValue.Location = new System.Drawing.Point(390, 254);
		this.btDocInTagValue.Name = "btDocInTagValue";
		this.btDocInTagValue.Size = new System.Drawing.Size(27, 22);
		this.btDocInTagValue.TabIndex = 284;
		this.btDocInTagValue.Tag = "#FOLDER#SENDER";
		this.btDocInTagValue.Text = "...";
		this.btDocInTagValue.UseVisualStyleBackColor = true;
		this.btDocInTagValue.Click += new System.EventHandler(btDocInTagValue_Click);
		this.cbDocInTagValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbDocInTagValue.FormattingEnabled = true;
		this.cbDocInTagValue.Location = new System.Drawing.Point(83, 255);
		this.cbDocInTagValue.Name = "cbDocInTagValue";
		this.cbDocInTagValue.Size = new System.Drawing.Size(305, 21);
		this.cbDocInTagValue.TabIndex = 281;
		this.lbDocInTagValue01.AutoSize = true;
		this.lbDocInTagValue01.Location = new System.Drawing.Point(14, 236);
		this.lbDocInTagValue01.Name = "lbDocInTagValue01";
		this.lbDocInTagValue01.Size = new System.Drawing.Size(262, 13);
		this.lbDocInTagValue01.TabIndex = 283;
		this.lbDocInTagValue01.Text = "Atribuir etiqueta ao receber os arquivos XML";
		this.lbDocInTagValue02.AutoSize = true;
		this.lbDocInTagValue02.Location = new System.Drawing.Point(14, 259);
		this.lbDocInTagValue02.Name = "lbDocInTagValue02";
		this.lbDocInTagValue02.Size = new System.Drawing.Size(66, 13);
		this.lbDocInTagValue02.TabIndex = 282;
		this.lbDocInTagValue02.Text = "Etiqueta : ";
		this.label13.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label13.Location = new System.Drawing.Point(14, 227);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(567, 2);
		this.label13.TabIndex = 280;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(5, 306);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(596, 38);
		this.plnMessage.TabIndex = 272;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(606, 401);
		base.Controls.Add(this.plnMessage);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.btSair);
		base.Controls.Add(this.btImportXml);
		base.Controls.Add(this.lbSeparator05);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmDocUpload";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Importação inteligente de arquivos XML";
		base.Load += new System.EventHandler(frmDocUpload_Load);
		base.Shown += new System.EventHandler(frmDocUpload_Shown);
		((System.ComponentModel.ISupportInitialize)this.picDocInXmlJurValLocker).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picFolder_SubDirInLocker).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		base.ResumeLayout(false);
	}
}
