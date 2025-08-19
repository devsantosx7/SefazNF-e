using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using data.fiscal.io;
using FlatTabControl;
using manager.fiscal.io;
using monitor;
using Monitor.Commands;
using Monitor.CustomControls;
using Monitor.PanelManager;
using plugin.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using TheArtOfDev.HtmlRenderer.WinForms;
using util.fiscal.io;

namespace Monitor;

public class frmMonitor : Form
{
	private delegate void ExcelPanelDelegate(decimal porcentagem);

	private bool varIsScanning;

	private bool varFormIsIdle;

	private bool varLoadingTree;

	private bool varLoadingTabs;

	private bool varLoadingData;

	private bool _IsFiscalServer;

	private bool varObjectsLoaded;

	private bool varLoadingConfig;

	private bool _HasFiscalServer;

	private bool varStartInHideMode;

	private bool varStartupError;

	private clsTraceService varclsTracer;

	private clsTabManager varclsTabManager;

	private TreeNode _LastFilialNode;

	private clsDbaFactory varDbaFactory = new clsDbaFactory();

	private clsPanelManager varclsPanelManager;

	private clsFilialManager varclsFilialManager;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService varclsFeatService = new clsFeatureService();

	private ExcelPanelDelegate ExcelPanelAction;

	private Point _imgHitArea = new Point(13, 2);

	private Point _imageLocation = new Point(13, 5);

	private DateTime _LastDataSync = DateTime.Now.AddDays(-1.0);

	private FormWindowState varWindowState = FormWindowState.Maximized;

	private clsMetricService _clsMetricService = new clsMetricService();

	private clsUserUsageService varclsUserUsageService = new clsUserUsageService();

	private readonly string MANAGER_URL = "https://app.fiscal.io";

	private IContainer components;

	private SplitContainer splitMonData;

	private SplitContainer splitMonitorRight;

	private ToolStrip toolbarCompanies;

	private ToolStripButton tsbAddFilial;

	private ToolStripButton tsbEdtFilial;

	private ToolStripButton tsbExcFilial;

	private ToolStrip toolbarFeatures;

	private ToolStripButton tsbSupport;

	private ToolStripSeparator toolStripSeparator2;

	private LinkLabel lkbTaskManager;

	private LinkLabel lkbIntegration03;

	private PictureBox picTaskManager;

	private PictureBox picIntegration;

	private Panel plnMessage;

	private ToolStrip toolbarDocs02;

	private ToolStripButton tsbChave;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripLabel toolStripLabel5;

	private ToolStripTextBox tsbSearchTerm;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripTextBox tsbDataIni;

	private ToolStripLabel toolStripLabel2;

	private ToolStripTextBox tsbDataFim;

	private ToolStripSeparator toolStripSeparator12;

	private ToolStripButton tsbAtualizar01;

	private ToolStripButton tsbSearchHelp;

	private Panel pnContent;

	private NotifyIcon notifyScan;

	private System.Timers.Timer timerTaskRunUser;

	private System.Timers.Timer timerCheckScan;

	private ContextMenuStrip contextMenuScan;

	private ToolStripMenuItem tsmShowMonitor;

	private ToolStripSeparator toolStripSeparator9;

	private ToolStripSeparator toolStripSeparator19;

	private ToolStripMenuItem tsmShowConfig;

	private ToolStripSeparator toolStripSeparator26;

	private ToolStripMenuItem tsmShowChannelManager;

	private ToolStripMenuItem tsmShowTaskManager;

	private ToolStripSeparator toolStripSeparator27;

	private ToolStripMenuItem tsmSair;

	private global::FlatTabControl.FlatTabControl tabContent;

	private ImageList imgTabContent;

	private ToolStripComboBox tscFiliais;

	private ToolStripSeparator toolStripSeparator6;

	private ImageList ImgLibrary;

	private ToolStripButton tsbSyncronize;

	private System.Timers.Timer timerTaskRunSyst;

	private ContextMenuStrip contextSortFilial;

	private ToolStripMenuItem tsmOrderByDesct;

	private ToolStripMenuItem tsmOrderByIdent;

	private ToolStripButton tsbSortFilial;

	private Label lbBackupSize;

	private LinkLabel lkbBackupSet;

	private PictureBox picBackupSet;

	private System.Timers.Timer timerBackupAdv;

	private Label lbkBckDate;

	private ToolTip toolTipDbaSize;

	private Panel pnMonTools;

	private PictureBox picTimerScan;

	private ComboBox cbTimerScan;

	private Label lbTimerScan;

	private SplitContainer splitMonDataLeft;

	private StatusStrip stsFilial;

	private ToolStripStatusLabel tslLicenseAgreement;

	private Label lbMonToolsSep01;

	private ToolStripMenuItem tsmTimerScan;

	private ToolStripMenuItem tsmScanEach1Hour;

	private ToolStripMenuItem tsmScanEach2Hour;

	private ToolStripMenuItem tsmScanEach6Hour;

	private ToolStripMenuItem tsmScanEach3Hour;

	private Panel pnAdmTools;

	private Panel pnBackupSet;

	private Panel pnTimerScan;

	private Label lbMonToolsSep02;

	private ToolStripMenuItem tsmScanManually;

	private SplitContainer splitMonFeatures;

	private Button btnSignature;

	private LinkLabel lkbSupportChat;

	private PictureBox picSupportChat;

	private ToolStripButton tsbRefreshFilial;

	private ToolStripButton tsbSearch;

	private ToolStripButton tsbFilterFilial;

	private ContextMenuStrip contextMenuFilial;

	private ToolStripMenuItem tsmEditFilial;

	private ToolStripMenuItem tsmDelFilial;

	private ToolStripSeparator toolStripSeparator35;

	private ToolStripMenuItem tsmAddFilial;

	private ToolStripSeparator toolStripSeparator36;

	private ToolStripSeparator toolStripSeparator37;

	private ToolStripButton tsbDataColapse;

	private ToolStripSeparator toolStripSeparator40;

	private ToolStripButton tsbFullScreen;

	private LinkLabel lbLicenseManager;

	private Label lbSalesText01;

	private Panel pnUserData;

	private Label label3;

	private PictureBox picUserData;

	private LinkLabel lbUserData;

	private Label label2;

	private ContextMenuStrip contextManifest;

	private ToolStripSeparator toolStripSeparator13;

	private ToolStripMenuItem tsmEventosNFeConfirmacao;

	private ToolStripSeparator toolStripSeparator14;

	private ToolStripMenuItem tsmEventosNFeNaoRealizada;

	private ToolStripMenuItem tsmEventosNFeDesconhecida;

	private ToolStripSeparator toolStripSeparator15;

	private ToolStripMenuItem tsmEventosNFeCiencia;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem tsmCopyFilial;

	private StatusStrip stsSumary;

	private ToolStripStatusLabel stsTotalValue;

	private ToolStripStatusLabel stsTotalQuant;

	private ToolStripStatusLabel stsSumSep01;

	private BackgroundWorker backTaskRunUser;

	private BackgroundWorker backTaskRunSyst;

	private ToolStripComboBox tscDateType;

	private Panel pnStartup;

	private Button btDFeDownload;

	private Label lbLine02;

	private Label lbOnboardText10;

	private Button btNFeManifest;

	private Label lbOnboardText08;

	private Label lbOnboardText07;

	private Label lbOnboardText06;

	private Label label1;

	private Label lbOnboardText05;

	private Label lbLine01;

	private Label lbOnboardText09;

	private Button btSrvDisagree;

	private Label lbOnboardText03;

	private Label lbOnboardText04;

	private Label lbOnboardText02;

	private Label lbOnboardText01;

	private Button btOnboardAction;

	private Label label4;

	private LinkLabel lkbBalance;

	private CustomTreeview trvFeatures;

	private CustomTreeview trvFilial;

	private ToolStripStatusLabel stsSumSep02;

	private ToolStripStatusLabel stsHelpCenter;

	private ToolStrip toolbarDocs01;

	private ToolStripButton tsbConfiguration;

	private ToolStripSeparator toolStripSeparator41;

	private ToolStripButton tsbBuscarDocs;

	private ToolStripButton tsbBuscarDocsAll;

	private ToolStripSeparator toolStripSeparator29;

	private ToolStripDropDownButton tsmDownDFeMenu;

	private ToolStripMenuItem tsbDowDFeStart;

	private ToolStripSeparator toolStripSeparator42;

	private ToolStripMenuItem tsbDowDFeBatch_Click;

	private ToolStripSeparator toolStripSeparator24;

	private ToolStripDropDownButton tsbEventosNFe;

	private ToolStripMenuItem tsbEventoCTeDesacordo;

	private ToolStripSeparator toolStripSeparator01;

	private ToolStripMenuItem tsbEventosNFeConfirmacao;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripMenuItem tsbEventosNFeNaoRealizada;

	private ToolStripMenuItem tsbEventosNFeDesconhecida;

	private ToolStripSeparator toolStripSeparator33;

	private ToolStripMenuItem tsbEventosNFeCiencia;

	private ToolStripSeparator toolStripSeparator02;

	private ToolStripMenuItem tsbEventsDFeHelp;

	private ToolStripSeparator toolStripSeparator16;

	private ToolStripDropDownButton tsbDFeConsStatus;

	private ToolStripMenuItem tsbConsStatusDFeSimple;

	private ToolStripMenuItem tsbConsStatusDFeFull;

	private ToolStripSeparator tssSepCons01;

	private ToolStripMenuItem tsbConsStatusDFeHelp;

	private ToolStripSeparator toolStripSeparator22;

	private ToolStripDropDownButton tsbTags;

	private ToolStripSeparator SepAuditor;

	private ToolStripButton tsbAuditor;

	private ToolStripSeparator toolStripSeparator18;

	private ToolStripDropDownButton tsbHandlerEdiFile;

	private ToolStripMenuItem tsbViewEdiAll;

	private ToolStripSeparator toolStripSeparator31;

	private ToolStripMenuItem tsbBaixarNotFis;

	private ToolStripMenuItem tsbBaixarConemb;

	private ToolStripSeparator toolStripSeparator30;

	private ToolStripMenuItem tsbEdiProcedaHelp;

	private ToolStripSeparator toolStripSeparator11;

	private ToolStripButton tsbUploadXml;

	private ToolStripSeparator toolStripSeparator23;

	private ToolStripButton tsbExport;

	private ToolStripButton tsbExportar;

	private ToolStripSeparator toolStripSeparator28;

	private ToolStripButton tsbImprimir;

	private ToolStripSeparator toolStripSeparator34;

	private ToolStripDropDownButton tsbIntegrations;

	private ToolStripSeparator toolStripSeparator32;

	private ToolStripButton tsbSendDoc;

	private ToolStripSeparator toolStripSeparator38;

	private ToolStripButton tsbMonTools;

	private ToolStripSeparator toolStripSeparator21;

	private ToolStripDropDownButton tsbPlugins;

	private ToolStripSeparator toolStripSeparator39;

	private ToolStripMenuItem tsbConsExtSyst;

	private ToolStripSeparator tssSepCons02;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton toolStripBtnFilterClear;

	private ToolStripSeparator toolStripSeparator10;

	private ToolStripLabel lbPageSize;

	private ToolStripTextBox tsbPageSize;

	private ToolStripSeparator lbPageSizeSep;

	private HtmlToolTip htmlToolTip1;

	private ToolStripStatusLabel stsSumSep03;

	private ToolStripStatusLabel stsPageWarn;

	private ToolStripMenuItem tsbEventosNFSeConfirmacao;

	private ToolStripSeparator toolStripSeparator20;

	private ToolStripMenuItem tsbEventosNFSeRecusa;

	private ToolStripSeparator toolStripSeparator17;

	private ToolStripSeparator toolStripSeparator8;

	private System.Timers.Timer timerBannerCheck;

	private ToolStripStatusLabel stsWhatNew;

	private ToolStripStatusLabel stsSumSep04;

	private ToolStripMenuItem tsbEventoCTeDesacordoCanc;

	public frmMonitor()
	{
		InitializeComponent();
		base.AutoScaleMode = AutoScaleMode.Font;
		base.WindowState = FormWindowState.Maximized;
		Font = new Font(Font.Name, 792f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
		Text = Text + " - Versão : " + Application.ProductVersion;
		splitMonFeatures.Panel2Collapsed = true;
		splitMonDataLeft.Panel2Collapsed = true;
		varclsPanelManager = new clsPanelManager(plnMessage);
		varclsPanelManager.EventPanelManager += funcEventPanelManager;
		ExcelPanelAction = ChangeExcelPanelPercent;
		try
		{
			if (Program.LaunchedViaStartup)
			{
				_ = Task.Run(async () => await funcStartBackGroundObjectsAsync(pShowStatus: false)).Result;
				_ = Task.Run(async () => await funcStartTimerTaskRunAsync()).Result;
				notifyScan.Visible = true;
				varStartInHideMode = true;
			}
			else
			{
				notifyScan.Visible = false;
				varStartInHideMode = false;
			}
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private async void frmMonitor_Shown(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			splitMonDataLeft.SplitterDistance = base.Height;
			splitMonFeatures.SplitterDistance = base.Height;
			splitMonFeatures.Panel2MinSize = pnUserData.Height;
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		try
		{
			ToolStrip toolStrip = toolbarDocs01;
			bool enabled = (toolbarDocs02.Enabled = false);
			toolStrip.Enabled = enabled;
			ToolStrip toolStrip2 = toolbarCompanies;
			enabled = (toolbarFeatures.Enabled = false);
			toolStrip2.Enabled = enabled;
			contextMenuScan.Enabled = false;
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		if (!Program.LaunchedViaStartup)
		{
			varclsReturnFunc.AddRange((await funcStartBackGroundObjectsAsync(pShowStatus: true)).Messages);
		}
		if (varclsReturnFunc.HasError)
		{
			toolbarDocs02.Enabled = true;
			varStartupError = true;
			funcShowErrorInStartup(varclsReturnFunc, null);
			return;
		}
		try
		{
			ToolStrip toolStrip3 = toolbarDocs01;
			bool enabled = (toolbarDocs02.Enabled = true);
			toolStrip3.Enabled = enabled;
			ToolStrip toolStrip4 = toolbarCompanies;
			enabled = (toolbarFeatures.Enabled = true);
			toolStrip4.Enabled = enabled;
			contextMenuScan.Enabled = true;
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		varclsReturnFunc.AddRange((await funcStartTimerTaskRunAsync()).Messages);
		varclsReturnFunc.AddRange(await funcStartForeGroundObjectsAsync());
		if (varclsReturnFunc.HasError)
		{
			varStartupError = true;
			funcShowErrorInStartup(varclsReturnFunc, null);
			return;
		}
		funcAdjustMenuTools();
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		string varLastUtilization = DateTime.Now.ToString("yyyy-MM-dd");
		await _clsDataParam.funcSetAsync("USER_LAST_USE", varLastUtilization);
		await varclsUserUsageService.funcRegisterAsync(pFinish: false);
		await funcCheckInitialStartupAsync();
		await funcCheckNpsSurveys();
	}

	protected override void SetVisibleCore(bool value)
	{
		if (varStartInHideMode)
		{
			base.SetVisibleCore(value: false);
		}
		else
		{
			base.SetVisibleCore(value);
		}
	}

	public void funcSetStartInHideMode()
	{
		varStartInHideMode = true;
	}

	private void funcShowErrorInStartup(clsReturn pclsReturn, Exception pException)
	{
		if (pclsReturn != null)
		{
			string varStartMessage = "Não é possivel iniciar alguns objetos do Fiscal.io Monitor";
			if (pclsReturn.HasUserHelp("DBA-ALREADY-IN-USE"))
			{
				varStartMessage = clsFunction.funcAlertFileInUseResume();
			}
			pclsReturn.AddMessage(new clsMessage("E", "999", varStartMessage, pException));
			clsTaskStatus varTaskStatus = new clsTaskStatus();
			varTaskStatus.Show = true;
			varTaskStatus.Progress = false;
			varTaskStatus.Error = true;
			varTaskStatus.Message01 = varStartMessage;
			varTaskStatus.ButtonText = "Ver detalhe do erro";
			varTaskStatus.Event = funcShowErrorTask;
			varTaskStatus.Return = pclsReturn;
			funcSetTaskStatus(varTaskStatus);
		}
	}

	private async Task<bool> funcShowSearchInteligentAsync()
	{
		string varFeatExtId = clsFeatureService.consSearchInteligent;
		string varFeatType = await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId);
		bool varShowMessage = varFeatType == null || (clsFunction.Contains(varFeatType, "LOCK") ? true : false);
		if (varShowMessage)
		{
			tsbSearchTerm.ToolTipText = "Gratuito -- > Busca por Número de Documento" + Environment.NewLine + "Plano Básico --> Busca por CFOP e NCM" + Environment.NewLine + "Plano Básico --> Busca por CNPJ e/ou CPF do Parceiro Comercial" + Environment.NewLine + "Plano Básico -- > Busca por Nome do Parceiro Comercial" + Environment.NewLine + "Plano Básico -- > Busca por Chave de Acesso";
			tsbSearchTerm.AutoToolTip = true;
		}
		else
		{
			tsbSearchTerm.ToolTipText = "A Busca inteligente aceita termos como :" + Environment.NewLine + "CFOP:  + Número do CFOP nos documentos que deseja buscar" + Environment.NewLine + "NCM:   + Número do NCM nos documentos que deseja buscar" + Environment.NewLine + "CHAVE: + Chave do documento que deseja buscar" + Environment.NewLine + "CNPJ:  + Número do CNPJ do Parceiro Comercial" + Environment.NewLine + "CPF:   + Número do CPF do Parceiro Comercial" + Environment.NewLine + "COMENTARIO:   + Texto do comentário que deseja buscar" + Environment.NewLine;
			tsbSearchTerm.AutoToolTip = false;
		}
		return varShowMessage;
	}

	private void funcAdjustMenuTools()
	{
		try
		{
			splitMonDataLeft.SplitterDistance = base.Height;
			if (pnTimerScan.Visible && pnBackupSet.Visible)
			{
				pnTimerScan.Top = 0;
				pnBackupSet.Top = pnTimerScan.Height - 1;
				pnAdmTools.Top = pnBackupSet.Top + pnBackupSet.Height - 1;
				splitMonDataLeft.Panel2MinSize = 170;
			}
			else if (pnTimerScan.Visible)
			{
				pnTimerScan.Top = 0;
				pnAdmTools.Top = pnTimerScan.Height - 1;
				splitMonDataLeft.Panel2MinSize = 132;
			}
			else if (pnBackupSet.Visible)
			{
				pnBackupSet.Top = 0;
				pnAdmTools.Top = pnBackupSet.Height - 1;
				splitMonDataLeft.Panel2MinSize = 120;
			}
			else if (pnAdmTools.Visible)
			{
				pnAdmTools.Top = 0;
				splitMonDataLeft.Panel2MinSize = 80;
			}
			splitMonFeatures.SplitterDistance = base.Height;
			splitMonFeatures.Panel2MinSize = pnUserData.Height;
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private async Task<bool> funcShowBackupDataAsync()
	{
		clsDataConfig varclsDataConfig = new clsDataConfig();
		clsBackupService varclsBackupService = new clsBackupService();
		try
		{
			if (clsFunction.IsEqual(clsFunction.IsToUpdateScreen, "BACKUP"))
			{
				clsFunction.funcSetToUpdateScreen(string.Empty);
			}
			timerBackupAdv.Enabled = false;
			Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
			intDatabase obj = await new clsDbaFactory().funcGetClassAsync();
			bool varShowBackupData = true;
			if (!obj.funcIsLocalDba())
			{
				varShowBackupData = false;
			}
			splitMonDataLeft.Panel2Collapsed = false;
			pnBackupSet.Visible = varShowBackupData;
			if (!varShowBackupData)
			{
				return true;
			}
			timerBackupAdv.Enabled = true;
			string varFeatureIdCode = clsFeatureService.consFeatBackupOfData;
			string varFeatType = await varclsFeatService.funcGetFeatTypeAsync(varFeatureIdCode);
			TaskAction varclsTaskAction = await new clsTaskScheduler().funcGetTaskToBackupSlfProfileUserDataAsync(pNewIfNotFound: true);
			DateTime varNextRun = clsFunction.funcGetDateTime(varclsTaskAction.NextRun);
			double varDiffRun = DateTime.Now.Subtract(varNextRun).TotalMinutes;
			clsFunction.funcGetValue(varclsConfig.BackupActive);
			if (clsFunction.Contains(varFeatType, "LOCK"))
			{
				_ = string.Empty;
			}
			if (clsFunction.Contains(varFeatType, "LOCK"))
			{
				picBackupSet.Image = Resources.image_warning;
				lbBackupSize.BackColor = Color.OrangeRed;
				lbBackupSize.ForeColor = Color.White;
				lbkBckDate.ForeColor = Color.Salmon;
				lkbBackupSet.Text = "Backup inativo";
				timerBackupAdv.Enabled = true;
			}
			else if (clsFunction.IsEmpty(varclsConfig.BackupActive))
			{
				picBackupSet.Image = Resources.image_warning;
				lbBackupSize.BackColor = Color.OrangeRed;
				lbBackupSize.ForeColor = Color.White;
				lbkBckDate.ForeColor = Color.Salmon;
				lkbBackupSet.Text = "Backup inativo";
				timerBackupAdv.Enabled = true;
			}
			else if (varDiffRun > 120.0)
			{
				picBackupSet.Image = Resources.image_ok;
				lbBackupSize.BackColor = Color.Yellow;
				lbBackupSize.ForeColor = Color.Black;
				lbkBckDate.ForeColor = Color.Teal;
				lkbBackupSet.Text = "Backup pendente";
				timerBackupAdv.Enabled = false;
			}
			else if (!clsFunction.IsEmpty(varclsTaskAction.LastRun))
			{
				picBackupSet.Image = Resources.image_ok;
				lbBackupSize.BackColor = Color.LightGreen;
				lbBackupSize.ForeColor = Color.Black;
				lbkBckDate.ForeColor = Color.Teal;
				lkbBackupSet.Text = "Backup realizado";
				timerBackupAdv.Enabled = false;
			}
			else
			{
				picBackupSet.Image = Resources.image_ok;
				lbBackupSize.BackColor = Color.Yellow;
				lbBackupSize.ForeColor = Color.Black;
				lkbBackupSet.Text = "Backup planejado";
				lbkBckDate.ForeColor = Color.Teal;
				timerBackupAdv.Enabled = false;
			}
			if (!clsFunction.IsEmpty(varclsTaskAction.LastRun))
			{
				DateTime varLastDate = clsFunction.funcGetDateTime(varclsTaskAction.LastRun);
				lbkBckDate.Text = varLastDate.ToString("dd/MM  HH:mm:ss");
			}
			else if (!clsFunction.IsEmpty(varclsTaskAction.NextRun))
			{
				DateTime varNextDate = clsFunction.funcGetDateTime(varclsTaskAction.NextRun);
				lbkBckDate.Text = varNextDate.ToString("dd/MM  HH:mm:ss");
			}
			else
			{
				lbkBckDate.Text = "Não realizado";
			}
			lbBackupSize.Text = varclsBackupService.funcGetDbaSizeInUserView(varclsConfig);
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return true;
	}

	private async Task<bool> funcShowSalesDataAsync()
	{
		_ = 3;
		try
		{
			clsSoftwareService varclsSoftwareService = new clsSoftwareService(null);
			clsBalanceService varclsBalanceService = new clsBalanceService();
			if (clsFunction.IsEqual(clsFunction.IsToUpdateScreen, "BALANCE"))
			{
				clsFunction.funcSetToUpdateScreen(string.Empty);
			}
			string varSoftExtId = clsFunction.funcGetSoftExtId();
			Software varclsSoftware = await new clsDataSoftware().funcGetItemByExtIdAsync(varSoftExtId);
			if (varclsSoftware == null)
			{
				splitMonFeatures.Panel2Collapsed = true;
				return false;
			}
			splitMonFeatures.SuspendLayout();
			splitMonFeatures.SplitterDistance = base.Height;
			pnUserData.Visible = true;
			splitMonFeatures.Panel2MinSize = pnUserData.Height;
			Button button = btnSignature;
			Label label = lbSalesText01;
			bool flag = (lbLicenseManager.Visible = false);
			bool visible = (label.Visible = flag);
			button.Visible = visible;
			if (await varclsSoftwareService.funcHasPayedPlanAsync(varclsSoftware, pWhtFree: false))
			{
				Label label2 = lbSalesText01;
				label2.Text = await varclsSoftwareService.funcGetPayedMessageAsync(varclsSoftware);
				Label label3 = lbSalesText01;
				visible = (lbLicenseManager.Visible = true);
				label3.Visible = visible;
			}
			else
			{
				btnSignature.Visible = true;
			}
			clsBalanceService.BalanceModel varclsBalance = await varclsBalanceService.funcGetBalanceAsync(clsBalanceService.consMetDowQuerySpec);
			lkbBalance.Text = "Créditos especiais : ";
			if (!clsFunction.IsEmpty(varclsBalance.MeaUnit))
			{
				lkbBalance.Text += varclsBalance.MeaUnit;
			}
			lkbBalance.Text += varclsBalanceService.funcGetValStr(varclsBalance.MeaType, varclsBalance.TotalBalc);
			lkbBalance.Tag = varclsBalance.TotalBalc;
			if (!varclsBalance.HasBalance)
			{
				lkbBalance.ForeColor = Color.Red;
			}
			else
			{
				lkbBalance.ForeColor = Color.MidnightBlue;
			}
			splitMonFeatures.Panel2Collapsed = false;
			splitMonFeatures.SplitterDistance = base.Height;
			splitMonFeatures.Panel2MinSize = pnUserData.Height;
			splitMonFeatures.ResumeLayout();
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return true;
	}

	private async void funcShowStatusScan(object sender, LinkLabelLinkClickedEventArgs e)
	{
		LinkLabel obj = (LinkLabel)sender;
		if (obj == null)
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		clsReturn obj2 = (clsReturn)obj.Tag;
		if (obj2 == null)
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		frmStatusScan obj3 = new frmStatusScan(obj2, null);
		obj3.ShowDialog();
		obj3.Dispose();
		await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
	}

	private void funcShowErrorTask(object sender, LinkLabelLinkClickedEventArgs e)
	{
		LinkLabel obj = (LinkLabel)sender;
		if (obj == null)
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		clsReturn varclsReturn = (clsReturn)obj.Tag;
		if (varclsReturn == null)
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		if (varclsReturn.Messages.Count > 0)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturn);
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
	}

	private async Task<clsReturn> funcStartTimerTaskRunAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			string varTaskRunIntervalStr = await new clsDataParameter().funcGetAsync("TaskRunInterval", pBuffer: true, pGlobal: true);
			if (clsFunction.IsEqual(varTaskRunIntervalStr, "", "0"))
			{
				varTaskRunIntervalStr = "5";
			}
			double varTaskRunIntervalDec = clsFunction.funcConvStrToDouble(varTaskRunIntervalStr) * 60000.0;
			timerCheckScan.Interval = varTaskRunIntervalDec;
			timerCheckScan.Enabled = true;
			timerCheckScan.Start();
			timerTaskRunUser.Interval = varTaskRunIntervalDec;
			timerTaskRunUser.Enabled = true;
			timerTaskRunUser.Start();
			timerTaskRunSyst.Interval = varTaskRunIntervalDec;
			timerTaskRunSyst.Enabled = true;
			timerTaskRunSyst.Start();
			timerBannerCheck.Enabled = true;
			timerBannerCheck.Start();
			timerBackupAdv.Interval = varTaskRunIntervalDec;
			if (_HasFiscalServer)
			{
				timerBackupAdv.Enabled = false;
				timerBackupAdv.Stop();
			}
			else
			{
				timerBackupAdv.Enabled = true;
				timerBackupAdv.Start();
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return varclsReturnFunc;
	}

	private clsReturn funcStopTimerTaskRun()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			timerCheckScan.Enabled = false;
			timerCheckScan.Stop();
			timerTaskRunUser.Enabled = false;
			timerTaskRunUser.Stop();
			timerTaskRunSyst.Enabled = false;
			timerTaskRunSyst.Stop();
			timerBackupAdv.Enabled = false;
			timerBackupAdv.Stop();
			timerBannerCheck.Enabled = false;
			timerBannerCheck.Stop();
			if (!varclsReturnFunc.HasError)
			{
				funcSetTaskStatus(new clsTaskStatus(pShow: false));
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return varclsReturnFunc;
	}

	private async void tscDateType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!varLoadingTabs)
		{
			string varDateType = clsFunction.funcGetValue(tscDateType.ComboBox.SelectedValue);
			await _clsDataParam.funcSetAsync("DateType", varDateType);
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true, pShowMore: true);
		}
	}

	private async Task<clsReturn> funcStartForeGroundObjectsAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			while (!varObjectsLoaded)
			{
				await Task.Delay(TimeSpan.FromSeconds(3.0));
			}
			varFormIsIdle = !clsFunction.funcSetProcessPriority(await _clsDataParam.funcGetAsync("ClientThreadPrior"), ProcessPriorityClass.High);
			varclsFilialManager = new clsFilialManager(trvFilial, tscFiliais, tsbFilterFilial);
			varclsTabManager = new clsTabManager();
			varclsTabManager.EventTabManager += funcEventTabManager;
			funcSetTaskStatus(new clsTaskStatus("Preparando as etiquetas..."));
			clsReturn varResultTags = await funcLoadTagsAsync(pReloadTabs: false);
			if (varResultTags.HasError)
			{
				varclsReturnFunc.AddRange(varResultTags);
			}
			funcSetTaskStatus(new clsTaskStatus("Preparando lista de canais de integração..."));
			clsReturn varResultIntegrations = await funcLoadIntegrationButtonsAsync();
			if (varResultIntegrations.HasError)
			{
				varclsReturnFunc.AddRange(varResultIntegrations);
			}
			funcSetTaskStatus(new clsTaskStatus("Preparando as extensões..."));
			clsReturn varResultPlugin = funcLoadPluginButtons();
			if (varResultPlugin.HasError)
			{
				varclsReturnFunc.AddRange(varResultPlugin);
			}
			funcSetTaskStatus(new clsTaskStatus("Preparando Consulta Fiscal..."));
			await funcLoadConsFiscalAsync();
			if (clsFunction.IsEmpty(await _clsDataParam.funcGetAsync("DEMO-MODE", pBuffer: true, pGlobal: true)))
			{
				clsFunction.funcRemoveAsDemoMode();
			}
			else
			{
				clsFunction.funcDefineAsDemoMode();
			}
			List<clsObjectType> varclsObjList = new List<clsObjectType>();
			varclsObjList.Add(new clsObjectType
			{
				Name = "Emissão",
				Value = "DtEmi"
			});
			varclsObjList.Add(new clsObjectType
			{
				Name = "Autorização",
				Value = "DtAut"
			});
			tscDateType.SelectedIndexChanged -= tscDateType_SelectedIndexChanged;
			tscDateType.ComboBox.ValueMember = "Value";
			tscDateType.ComboBox.DisplayMember = "Name";
			tscDateType.ComboBox.DataSource = varclsObjList;
			string varDateType = await _clsDataParam.funcGetAsync("DateType");
			if (clsFunction.IsEmpty(varDateType))
			{
				varDateType = "DtAut";
			}
			tscDateType.ComboBox.SelectedValue = varDateType;
			tscDateType.SelectedIndexChanged += tscDateType_SelectedIndexChanged;
			clsDbaUserQueryService.strScope varDbaStartScope = await new clsDbaUserQueryService().funcGetAsync();
			tsbDataFim.Text = varDbaStartScope.EndDate;
			tsbDataIni.Text = varDbaStartScope.BeginDate;
			tsbPageSize.Text = varDbaStartScope.PageSize.ToString();
			bool visible;
			if (varDbaStartScope.HasPageSize)
			{
				ToolStripSeparator toolStripSeparator = lbPageSizeSep;
				ToolStripLabel toolStripLabel = lbPageSize;
				bool flag = (tsbPageSize.Visible = true);
				visible = (toolStripLabel.Visible = flag);
				toolStripSeparator.Visible = visible;
			}
			else
			{
				ToolStripSeparator toolStripSeparator2 = lbPageSizeSep;
				ToolStripLabel toolStripLabel2 = lbPageSize;
				bool flag = (tsbPageSize.Visible = false);
				visible = (toolStripLabel2.Visible = flag);
				toolStripSeparator2.Visible = visible;
			}
			await _clsDataParam.funcSetAsync("BeginDate", tsbDataIni.Text);
			await _clsDataParam.funcSetAsync("EndDate", tsbDataFim.Text);
			await _clsDataParam.funcSetAsync("PageSize", tsbPageSize.Text);
			string varOrderByDesct = await _clsDataParam.funcGetAsync("FILIAL_ORDERBY_DESCT");
			tsmOrderByDesct.Checked = clsFunction.funcConvStrToBool(varOrderByDesct);
			string varOrderByIdent = await _clsDataParam.funcGetAsync("FILIAL_ORDERBY_IDENT");
			tsmOrderByIdent.Checked = clsFunction.funcConvStrToBool(varOrderByIdent);
			funcSetTaskStatus(new clsTaskStatus("Carregando a lista de empresas..."));
			clsReturn varResultFilial = await funcLoadFiliaisAsync();
			if (varResultFilial.HasError)
			{
				varclsReturnFunc.AddRange(varResultFilial);
			}
			funcSetTaskStatus(new clsTaskStatus("Carregando funcionalidades da tela..."));
			await funcShowSearchInteligentAsync();
			await funcShowBackupDataAsync();
			await funcShowSalesDataAsync();
			await funcCheckFiscalServerFunctionsAsync();
			funcAdjustMenuTools();
			string varIsCollapsed = await _clsDataParam.funcGetAsync("MonitorIsCollapsed");
			SplitContainer splitContainer = splitMonData;
			visible = (splitMonitorRight.Panel2Collapsed = !clsFunction.IsEmpty(varIsCollapsed));
			splitContainer.Panel1Collapsed = visible;
			tscFiliais.Visible = !clsFunction.IsEmpty(varIsCollapsed);
			tsbFullScreen.Checked = !clsFunction.IsEmpty(varIsCollapsed);
			funcSetTaskStatus(new clsTaskStatus("Carregando os filtros dinâmicos ..."));
			await (await funcGetDataFilterAsync()).funcCreateAsync(trvFeatures);
			funcSetTaskStatus(new clsTaskStatus("Carregando últimas guias abertas ..."));
			List<intDocTabData> obj = await varclsTabManager.ListAsync();
			varLoadingTabs = true;
			tabContent.SuspendLayout();
			foreach (intDocTabData varTabData in obj)
			{
				tabContent.TabPages.Add(varTabData.funcGetTabPage());
			}
			if (tabContent.TabPages.Count > 0)
			{
				intDocTabData varTabPageSect = varclsTabManager.Selected();
				_ = string.Empty;
				if (varTabPageSect != null)
				{
					string varTabPageName = varclsTabManager.Selected().TabName;
					tabContent.SelectTab(varTabPageName);
				}
				foreach (TabPage varTabPage in tabContent.TabPages)
				{
					if (varTabPageSect != null && !varTabPage.Equals(varTabPageSect))
					{
						varTabPage.SuspendLayout();
						varTabPage.BackColor = Color.WhiteSmoke;
						varTabPage.ResumeLayout();
					}
				}
				varLoadingTabs = false;
				tabContent.ResumeLayout();
				funcSetTaskStatus(new clsTaskStatus("Carregando dados para visualização ..."));
				await funcClearSearchTermAsync();
				await funcLoadTabViewDataAsync(pSetTool: true, pSetTotal: true, pResetTabs: true);
				new clsUpdaterService().funcRunNow();
				if (!varclsReturnFunc.HasError)
				{
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
		}
		catch (Exception pException)
		{
			clsMessage varclsMessage = new clsMessage("E", "9999", "Erro ao executar a função funcStartForeGroundObjects", pException);
			varclsReturnFunc.AddMessage(varclsMessage);
		}
		finally
		{
			varLoadingTabs = false;
			tabContent.ResumeLayout();
			if (!tabContent.Visible)
			{
				tabContent.Visible = true;
			}
			new clsSoftErrorService().funcCreateAsync("Screen", varclsReturnFunc);
		}
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcStartBackGroundObjectsAsync(bool pShowStatus)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		if (varObjectsLoaded)
		{
			return varclsReturnFunc;
		}
		try
		{
			funcStopTimerTaskRun();
			new clsLogService().funcGetLogger("Fiscal.io Monitor");
			clsStartupService varclsStartupService = new clsStartupService(pShowStatus);
			varclsStartupService.EventStartupSrv += funcEventStartupSrv;
			varclsReturnFunc = await varclsStartupService.funcOpenDbaAsync();
			if (varclsReturnFunc.HasError)
			{
				return varclsReturnFunc;
			}
			string varclsDbaType = (await new clsDbaFactory().funcGetClassAsync()).funcGetDbaType();
			if (Program.MustRunDbaOptimize && clsFunction.IsEqual(varclsDbaType, "SQLLITE"))
			{
				new frmLocalMigration().ShowDialog(this);
			}
			varclsTracer = new clsTraceService();
			string varClientThreadPrior = await _clsDataParam.funcGetAsync("ClientThreadPrior");
			if (Program.LaunchedViaStartup)
			{
				varFormIsIdle = clsFunction.funcSetProcessPriority(varClientThreadPrior, ProcessPriorityClass.Idle);
			}
			else
			{
				varFormIsIdle = !clsFunction.funcSetProcessPriority(varClientThreadPrior, ProcessPriorityClass.High);
			}
			new clsDataParameter().funcBufferDataAsync();
			new clsDataFilial().funcBufferDataAsync();
			varclsReturnFunc = await funcStartTimerTaskRunAsync();
			if (varclsReturnFunc.HasError)
			{
				return varclsReturnFunc;
			}
			varclsReturnFunc = await varclsStartupService.funcLoadCertificatesAsync(pBuffer: true, pShowStatus);
			if (varclsReturnFunc.HasError)
			{
				return varclsReturnFunc;
			}
			varclsReturnFunc = funcDefineMainScreenTitle();
			if (varclsReturnFunc.HasError)
			{
				return varclsReturnFunc;
			}
			varclsReturnFunc = await varclsStartupService.funcExecuteBackgroundTasks();
			if (varclsReturnFunc.HasError)
			{
				return varclsReturnFunc;
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			new clsSoftErrorService().funcCreateAsync("Screen", varclsReturnFunc);
			varObjectsLoaded = true;
		}
		if (!varclsReturnFunc.HasError && pShowStatus)
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		return varclsReturnFunc;
	}

	private clsReturn funcDefineMainScreenTitle()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDbaFactory varclsDbaFactory = new clsDbaFactory();
		try
		{
			string varDbaType = varclsDbaFactory.funcGetDbaType();
			string varDbaDesc = varclsDbaFactory.funcGetDbaDesc();
			Text = "Fiscal.io Monitor";
			Text = Text + " | Versão : " + Application.ProductVersion;
			Text = Text + " | Banco : " + clsDataGeral.funcGetDbaDescrpt(varDbaType);
			if (!clsFunction.IsEmpty(varDbaDesc))
			{
				Text = Text + " | " + varDbaDesc;
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private void funcSetTaskStatus(clsReturn pclsReturn)
	{
		Application.DoEvents();
		clsTaskStatus varTaskStatus = new clsTaskStatus();
		varTaskStatus.Show = true;
		varTaskStatus.Progress = false;
		varTaskStatus.Error = true;
		varTaskStatus.Message01 = "Houve um ou mais erros durante o processamento.";
		varTaskStatus.ButtonText = "Ver detalhe do erro";
		varTaskStatus.Event = funcShowErrorTask;
		varTaskStatus.Return = pclsReturn;
		funcSetTaskStatus(varTaskStatus);
		varclsPanelManager.funcSet(varTaskStatus);
		Application.DoEvents();
	}

	private void funcSetTaskStatus(clsTaskStatus pTaskStatus)
	{
		Application.DoEvents();
		varclsPanelManager.funcSet(pTaskStatus);
		Application.DoEvents();
	}

	private long funcGetTotalNsuValues(List<FilialView> pFilialList)
	{
		int varTotalNSUValues = 0;
		foreach (FilialView varclsItem in pFilialList)
		{
			varTotalNSUValues += clsFunction.funcConvStrToInt(varclsItem.NSUCTe);
			varTotalNSUValues += clsFunction.funcConvStrToInt(varclsItem.NSUNFe);
			varTotalNSUValues += clsFunction.funcConvStrToInt(varclsItem.NSUMDFe);
			varTotalNSUValues += clsFunction.funcConvStrToInt(varclsItem.NSUNFSe);
		}
		return varTotalNSUValues;
	}

	private async void funcDefineOnBoardScreen(List<FilialView> pFilialList)
	{
		if (!pnContent.Visible)
		{
			List<FilialView> varFilialList = pFilialList;
			if (varFilialList == null)
			{
				varFilialList = await new clsDataFilial().funcGetListAsync(pLoadDummy: true);
			}
			Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
			double varTotalDocs = 0.0;
			_ = string.Empty;
			long varTotalNSUValues = funcGetTotalNsuValues(varFilialList);
			if (clsFunction.IsEmpty(varclsConfig.NotShowFirstAct) && varTotalNSUValues <= 0)
			{
				varTotalDocs = await new clsDataDoc().funcGetTotalAsync();
			}
			if (varTotalDocs > 0.0 || varTotalNSUValues > 0)
			{
				varclsConfig.NotShowFirstAct = "X";
				await new clsDataConfig().funcUpdateAsync(varclsConfig);
			}
			string varOnboardType;
			if (varFilialList.Count == 0)
			{
				pnStartup.Visible = false;
				pnContent.Visible = false;
				varOnboardType = "STARTUP_SCREEN";
				varclsConfig.NotShowFirstAct = string.Empty;
				new clsDataConfig().funcUpdateAsync(varclsConfig);
			}
			else if (!clsFunction.IsEmpty(varclsConfig.NotShowFirstAct))
			{
				varOnboardType = string.Empty;
			}
			else if (varTotalNSUValues == 0L && varTotalDocs == 0.0)
			{
				varOnboardType = "BUTTON_SCAN";
				pnStartup.Visible = true;
				pnContent.Visible = false;
			}
			else
			{
				varOnboardType = string.Empty;
			}
			if (clsFunction.IsEmpty(varclsConfig.UserName))
			{
				lbOnboardText01.Text = "Olá,";
			}
			else
			{
				lbOnboardText01.Text = "Olá " + varclsConfig.UserName + ",";
			}
			if (clsFunction.IsEmpty(varOnboardType))
			{
				pnStartup.Visible = false;
				pnContent.Visible = true;
			}
			else if (clsFunction.IsEqual(varOnboardType, "STARTUP_SCREEN"))
			{
				lbOnboardText02.Text = "Configure o Fiscal.io Monitor";
				lbOnboardText03.Text = "e utilize todas as suas funcionalidades.";
				lbOnboardText04.Text = "VAMOS COMEÇAR?";
				lbOnboardText05.Text = "A configuração é muito simples. Você só precisa informar alguns parâmetros básicos de negócio.";
				btOnboardAction.Text = "INICIAR CONFIGURAÇÃO";
				btOnboardAction.Tag = "STARTUP_SCREEN";
				pnStartup.Visible = true;
				pnContent.Visible = false;
				Label label = lbOnboardText06;
				Label label2 = lbOnboardText07;
				bool flag = (lbOnboardText08.Visible = false);
				bool visible = (label2.Visible = flag);
				label.Visible = visible;
				Label label3 = lbLine01;
				Label label4 = lbOnboardText09;
				flag = (btSrvDisagree.Visible = false);
				visible = (label4.Visible = flag);
				label3.Visible = visible;
				Label label5 = lbLine02;
				Label label6 = lbOnboardText10;
				Button button = btDFeDownload;
				bool flag6 = (btNFeManifest.Visible = false);
				flag = (button.Visible = flag6);
				visible = (label6.Visible = flag);
				label5.Visible = visible;
			}
			else if (clsFunction.IsEqual(varOnboardType, "BUTTON_SCAN"))
			{
				lbOnboardText02.Text = "Parabéns. Você fez todas as configurações";
				lbOnboardText03.Text = "para a utilização do Fiscal.io Monitor !";
				lbOnboardText04.Text = "VAMOS INICIAR?";
				lbOnboardText05.Text = "Se o seu objetivo é fazer a gestão dos documentos fiscais, o primeiro passo é fazer uma busca na SEFAZ.";
				btOnboardAction.Text = "BUSCAR DOCUMENTOS NA SEFAZ";
				btOnboardAction.Tag = "BUTTON_SCAN";
				pnStartup.Visible = true;
				pnContent.Visible = false;
				Label label7 = lbOnboardText06;
				Label label8 = lbOnboardText07;
				bool flag = (lbOnboardText08.Visible = true);
				bool visible = (label8.Visible = flag);
				label7.Visible = visible;
				Label label9 = lbLine01;
				Label label10 = lbOnboardText09;
				flag = (btSrvDisagree.Visible = true);
				visible = (label10.Visible = flag);
				label9.Visible = visible;
				Label label11 = lbLine02;
				Label label12 = lbOnboardText10;
				Button button2 = btDFeDownload;
				bool flag6 = (btNFeManifest.Visible = true);
				flag = (button2.Visible = flag6);
				visible = (label12.Visible = flag);
				label11.Visible = visible;
			}
		}
	}

	private async Task<clsDataFilter> funcSyncUpdateScreenAsync(bool pReloadFilter, bool pReloadData)
	{
		clsFunction.funcSetToUpdateScreen(string.Empty);
		bool varSetLoadTotal = true;
		if (clsFunction.IsEmpty(await _clsDataParam.funcGetAsync("ShowReportSum")))
		{
			varSetLoadTotal = false;
		}
		funcSetTaskStatus(new clsTaskStatus("Atualizando dados da tela..."));
		clsDataFilter varclsDataFilter = await funcGetDataFilterAsync();
		if (clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("DbaQueryLimit", pBuffer: true, pGlobal: true)))
		{
			ToolStripSeparator toolStripSeparator = lbPageSizeSep;
			ToolStripLabel toolStripLabel = lbPageSize;
			bool flag = (tsbPageSize.Visible = true);
			bool visible = (toolStripLabel.Visible = flag);
			toolStripSeparator.Visible = visible;
		}
		else
		{
			ToolStripSeparator toolStripSeparator2 = lbPageSizeSep;
			ToolStripLabel toolStripLabel2 = lbPageSize;
			bool flag = (tsbPageSize.Visible = false);
			bool visible = (toolStripLabel2.Visible = flag);
			toolStripSeparator2.Visible = visible;
		}
		await funcLoadFiliaisAsync();
		await funcShowSearchInteligentAsync();
		await funcShowBackupDataAsync();
		await funcShowSalesDataAsync();
		await funcLoadIntegrationButtonsAsync();
		await funcCheckFiscalServerFunctionsAsync();
		funcAdjustMenuTools();
		bool varIsDone = false;
		int varCounter = 0;
		if (!pReloadFilter)
		{
			varIsDone = true;
		}
		while (!varIsDone)
		{
			try
			{
				varCounter = 1;
				trvFeatures = await varclsDataFilter.funcCreateAsync(trvFeatures);
				varIsDone = true;
			}
			catch
			{
				await Task.Delay(TimeSpan.FromSeconds(1.0));
				if (clsFunction.IsAdmin)
				{
					throw;
				}
			}
			if (varCounter > 5)
			{
				break;
			}
		}
		if (pReloadData)
		{
			await funcLoadTabViewDataAsync(pSetTool: false, varSetLoadTotal, pResetTabs: false);
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		funcDefineMainScreenTitle();
		return varclsDataFilter;
	}

	private async Task<bool> funcClearSearchTermAsync()
	{
		if (varclsTabManager == null)
		{
			return false;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return false;
		}
		tsbSearchTerm.Text = string.Empty;
		await _clsDataParam.funcSetAsync("SearchTerm", tsbSearchTerm.Text);
		bool varSearchInteligent = await funcShowSearchInteligentAsync();
		varTabData.funcSearchText(string.Empty, varSearchInteligent);
		return true;
	}

	private async void tsbConfiguration_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("CONFIG-MANAGER"))
		{
			using (frmConfig varfrmConfig = new frmConfig())
			{
				varfrmConfig.ShowDialog(this);
			}
			await funcSyncUpdateScreenAsync(pReloadFilter: false, pReloadData: false);
			await funcStartTimerTaskRunAsync();
		}
	}

	private clsReturn funcLoadPluginButtons()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			List<IPlugin> list = new clsPluginService().funcGetPluginList();
			tsbPlugins.DropDownItems.Clear();
			foreach (IPlugin varclsPlugin in list)
			{
				ToolStripMenuItem varPluginMenu = new ToolStripMenuItem();
				varPluginMenu.Name = varclsPlugin.Guid;
				varPluginMenu.Text = varclsPlugin.Name;
				varPluginMenu.Image = varclsPlugin.Icon;
				tsbPlugins.DropDownItems.Add(varPluginMenu);
				foreach (IButton varclsButton in varclsPlugin.ButtonList)
				{
					ToolStripMenuItem varPluginSubMenu = new ToolStripMenuItem();
					varPluginSubMenu.Name = varclsButton.ibGuid;
					varPluginSubMenu.Tag = varclsButton.ibGuid;
					varPluginSubMenu.Text = varclsButton.ibName;
					varPluginSubMenu.Image = varclsButton.ibIcon;
					varPluginSubMenu.Click += tsbPluginButton_Click;
					varPluginMenu.DropDownItems.Add(varPluginSubMenu);
				}
			}
			if (tsbPlugins.DropDownItems.Count > 0)
			{
				tsbPlugins.DropDownItems.Add(new ToolStripSeparator());
			}
			ToolStripMenuItem varNewPluginManager = new ToolStripMenuItem();
			varNewPluginManager.Text = "Gerenciar Extensões";
			varNewPluginManager.Image = Resources.image_configuration;
			varNewPluginManager.Click += tsbPluginManager_Click;
			tsbPlugins.DropDownItems.Add(varNewPluginManager);
			tsbPlugins.DropDownItems.Add(new ToolStripSeparator());
			ToolStripMenuItem varNewPluginHelp = new ToolStripMenuItem();
			varNewPluginHelp.Text = "Ajuda";
			varNewPluginHelp.Image = Resources.image_help;
			varNewPluginHelp.Click += tsbPluginHelp_Click;
			tsbPlugins.DropDownItems.Add(varNewPluginHelp);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcLoadIntegrationButtonsAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			tsbIntegrations.DropDownItems.Clear();
			List<Channel> varSenderList = await new clsDataChannel().funcGetSenderListAsync();
			if (varSenderList.Count > 1)
			{
				ToolStripMenuItem varChannelMenuAll = new ToolStripMenuItem
				{
					Name = "tsbiALL_INT",
					Text = "Executar todas as integrações"
				};
				varChannelMenuAll.Click += tsbIntegrate_Click;
				tsbIntegrations.DropDownItems.Add(varChannelMenuAll);
				tsbIntegrations.DropDownItems.Add(new ToolStripSeparator());
			}
			foreach (Channel varclsItem in varSenderList)
			{
				ToolStripMenuItem varPluginMenu = new ToolStripMenuItem
				{
					Name = varclsItem.ID,
					Text = varclsItem.Name
				};
				varPluginMenu.Click += tsbIntegrate_Click;
				tsbIntegrations.DropDownItems.Add(varPluginMenu);
			}
			if (varSenderList.Count > 0)
			{
				tsbIntegrations.DropDownItems.Add(new ToolStripSeparator());
			}
			ToolStripMenuItem varChannelCreate = new ToolStripMenuItem
			{
				Name = "tsbChannelManager",
				Text = "Gerenciar Canais de Integração",
				Image = Resources.image_process_done
			};
			varChannelCreate.Click += tsbChannelCreate_Click;
			tsbIntegrations.DropDownItems.Add(varChannelCreate);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private void tsbChannelCreate_Click(object sender, EventArgs e)
	{
		funcOpenChannelManager("");
	}

	private async Task<bool> funcLoadConsFiscalAsync()
	{
		if (await clsScreenGeral.funcMustShowRegFieldsAsync())
		{
			ToolStripMenuItem toolStripMenuItem = tsbConsExtSyst;
			bool visible = (tssSepCons02.Visible = true);
			toolStripMenuItem.Visible = visible;
		}
		else
		{
			ToolStripMenuItem toolStripMenuItem2 = tsbConsExtSyst;
			bool visible = (tssSepCons02.Visible = false);
			toolStripMenuItem2.Visible = visible;
		}
		return true;
	}

	private async Task<clsReturn> funcLoadTagsAsync(bool pReloadTabs)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDataTag varclsDataTag = new clsDataTag();
		clsDataWorkFlow varclsDataWork = new clsDataWorkFlow();
		string varFeatTagManager = clsFeatureService.consFeatTagManager;
		string varFeatAutoTagDet = clsFeatureService.consFeatAutTagDeter;
		try
		{
			string varDbaType = (await new clsDbaFactory().funcGetClassAsync()).funcGetDbaType();
			tsbTags.DropDownItems.Clear();
			foreach (Tag varTag in await varclsDataTag.funcGetListAsync(pJustWithAccess: true))
			{
				ToolStripMenuItem varNewTagMenu = new ToolStripMenuItem();
				varNewTagMenu.Tag = varTag.Code;
				varNewTagMenu.Text = varTag.Nome;
				varNewTagMenu.BackColor = ColorTranslator.FromHtml(varTag.Color);
				varNewTagMenu.Click += tsbTagCode_Click;
				tsbTags.DropDownItems.Add(varNewTagMenu);
			}
			tsbTags.DropDownItems.Add(new ToolStripSeparator());
			ToolStripMenuItem varEmptyTagMenu = new ToolStripMenuItem();
			varEmptyTagMenu.Tag = "";
			varEmptyTagMenu.Text = "Sem Etiqueta";
			varEmptyTagMenu.BackColor = Color.White;
			varEmptyTagMenu.Click += tsbTagCode_Click;
			tsbTags.DropDownItems.Add(varEmptyTagMenu);
			string varFeatType01 = await varclsFeatService.funcGetFeatTypeAsync(varFeatAutoTagDet);
			List<WorkFlow> varWorkList = await varclsDataWork.funcGetListAsync("PROC-SETTING-TAG");
			ToolStripMenuItem varAutoTagMenu = new ToolStripMenuItem();
			varAutoTagMenu.Tag = "";
			varAutoTagMenu.Text = "Determinação automática";
			varAutoTagMenu.Image = Resources.image_workflow;
			if (clsFunction.Contains(varFeatType01, "LOCK"))
			{
				varWorkList.Clear();
			}
			if (varWorkList.Count <= 0)
			{
				varAutoTagMenu.Click += tsbWorkFlow_Click;
			}
			if (!clsFunction.IsEqual(varDbaType, "SQLLITE"))
			{
				tsbTags.DropDownItems.Add(new ToolStripSeparator());
				tsbTags.DropDownItems.Add(varAutoTagMenu);
			}
			else
			{
				varWorkList.Clear();
			}
			foreach (WorkFlow varclsItem in varWorkList)
			{
				ToolStripMenuItem varAutoTagItem = new ToolStripMenuItem();
				varAutoTagItem.Tag = varclsItem.ID;
				varAutoTagItem.Text = varclsItem.Description;
				varAutoTagItem.Click += tsbTagCode_Click;
				varAutoTagMenu.DropDownItems.Add(varAutoTagItem);
			}
			if (varWorkList.Count > 0)
			{
				varAutoTagMenu.DropDownItems.Add(new ToolStripSeparator());
				ToolStripMenuItem varAutoTagManager = new ToolStripMenuItem();
				varAutoTagManager.Tag = "";
				varAutoTagManager.Text = "Gerenciar Fluxos de Decisão";
				varAutoTagManager.Image = Resources.image_workflow;
				varAutoTagManager.Click += tsbWorkFlow_Click;
				varAutoTagMenu.DropDownItems.Add(varAutoTagManager);
			}
			tsbTags.DropDownItems.Add(new ToolStripSeparator());
			await varclsFeatService.funcGetFeatTypeAsync(varFeatTagManager);
			ToolStripMenuItem varNewTagManager = new ToolStripMenuItem();
			varNewTagManager.Image = Resources.image_tag_manager;
			varNewTagManager.Text = "Gerenciar Etiquetas";
			varNewTagManager.Click += tsbTagManager_Click;
			tsbTags.DropDownItems.Add(varNewTagManager);
			tsbTags.DropDownItems.Add(new ToolStripSeparator());
			ToolStripMenuItem varNewDocNote = new ToolStripMenuItem();
			varNewDocNote.Image = Resources.image_notes;
			varNewDocNote.Text = "Atribuir comentário";
			varNewDocNote.Click += tsbDocNote_Click;
			tsbTags.DropDownItems.Add(varNewDocNote);
			tsbTags.DropDownItems.Add(new ToolStripSeparator());
			ToolStripMenuItem varRemoveDocNote = new ToolStripMenuItem();
			varRemoveDocNote.Image = Resources.image_delete_object;
			varRemoveDocNote.Text = "Remover comentário";
			varRemoveDocNote.Click += tsbRemoveDocNote_Click;
			tsbTags.DropDownItems.Add(varRemoveDocNote);
			if (pReloadTabs && varclsTabManager != null)
			{
				foreach (intDocTabData item in await varclsTabManager.ListAsync())
				{
					await item.funcLoadTagsAsync();
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private async void tsbWorkFlow_Click(object sender, EventArgs e)
	{
		string varFeatExtId = clsFeatureService.consFeatAutTagDeter;
		_clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
		}
		else if (await clsScreenGeral.funcHasAccessAsync("WORKFLOW-MANAGER"))
		{
			frmWorkFlowManager frmWorkFlowManager = new frmWorkFlowManager("PROC-SETTING-TAG");
			frmWorkFlowManager.ShowDialog(this);
			frmWorkFlowManager.Dispose();
			await funcLoadTagsAsync(pReloadTabs: true);
		}
	}

	private async void tsbTagCode_Click(object sender, EventArgs e)
	{
		ToolStripDropDownItem varTagMenu = (ToolStripDropDownItem)sender;
		if (varTagMenu == null)
		{
			return;
		}
		string varTagCode = (string)varTagMenu.Tag;
		if (varTagCode == null || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return;
		}
		foreach (FilialView varclsItem in await funcGetFilialListAsync())
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("TAG-ASSIGN", "ASSIGN", varclsItem.CNPJ)))
			{
				return;
			}
		}
		await varTabData.funcSetTagAsync(varTagCode, pFocused: false, pChecked: true);
	}

	private async void tsbPluginButton_Click(object sender, EventArgs e)
	{
		ToolStripMenuItem varPlugButton = (ToolStripMenuItem)sender;
		if (varPlugButton == null)
		{
			return;
		}
		string varButtonGuid = varPlugButton.Name;
		if (clsFunction.IsEmpty(varButtonGuid) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return;
		}
		clsDataDoc varclsDataDoc = new clsDataDoc();
		clsDataDocLink varclsDataDocLink = new clsDataDocLink();
		funcSetTaskStatus(new clsTaskStatus("Obtendo lista de documentos ..."));
		List<Document> varTempList = await varTabData.funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		funcSetTaskStatus(new clsTaskStatus("Preparando lista de documentos ..."));
		List<DocToPlugin> varDocList = await varclsDataDoc.funcGetListToPluginAsync(varTempList);
		List<DocLink> varRefList = new List<DocLink>();
		funcSetTaskStatus(new clsTaskStatus("Preparando documentos relacionados..."));
		foreach (DocToPlugin varclsItem01 in varDocList)
		{
			List<DocLink> varDocLinkList01 = await varclsDataDocLink.funcGetListByDocKeyAsync(varclsItem01.Chave);
			varRefList.AddRange(varDocLinkList01);
			foreach (DocLink varclsItem2 in varDocLinkList01)
			{
				varRefList.AddRange(await varclsDataDocLink.funcGetListByDocKeyAsync(varclsItem2.RefKey));
			}
		}
		funcSetTaskStatus(new clsTaskStatus("Processando lista de documentos ..."));
		foreach (IPlugin item in new clsPluginService().funcGetPluginList())
		{
			foreach (IButton varclsButton in item.ButtonList)
			{
				if (clsFunction.IsEqual(varButtonGuid, varclsButton.ibGuid))
				{
					varclsButton.funcExecute(varDocList);
					varclsButton.funcExecute(varDocList, varRefList);
				}
			}
		}
		await varTabData.funcRefreshItensAsync(pFocused: false, pChecked: false);
		funcSetTaskStatus(new clsTaskStatus());
	}

	private async void tsbTagManager_Click(object sender, EventArgs e)
	{
		string varFeatExtId = clsFeatureService.consFeatTagManager;
		_clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
		}
		else if (await clsScreenGeral.funcHasAccessAsync("TAG-MANAGER"))
		{
			frmTags frmTags = new frmTags();
			frmTags.ShowDialog(this);
			frmTags.Dispose();
			trvFeatures = await (await funcGetDataFilterAsync()).funcCreateAsync(trvFeatures);
			await funcLoadTagsAsync(pReloadTabs: true);
		}
	}

	private async void tsbDocNote_Click(object sender, EventArgs e)
	{
		if (!(await funcCheckInitialStartupAsync()) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return;
		}
		foreach (FilialView varclsItem in await funcGetFilialListAsync())
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("DOCNOTE-ASSIGN", "ASSIGN", varclsItem.CNPJ)))
			{
				return;
			}
		}
		await varTabData.funcSetDocNoteAsync(pFocused: true, pChecked: true);
	}

	private async void tsbRemoveDocNote_Click(object sender, EventArgs e)
	{
		if (!(await funcCheckInitialStartupAsync()) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return;
		}
		foreach (FilialView varclsItem in await funcGetFilialListAsync())
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("DOCNOTE-ASSIGN", "ASSIGN", varclsItem.CNPJ)))
			{
				return;
			}
		}
		await varTabData.funcRemoveDocNoteAsync(pFocused: true, pChecked: true);
	}

	private async void tsbPluginManager_Click(object sender, EventArgs e)
	{
		string varFeatId = clsFeatureService.consFeatExtension;
		_clsDataParam.funcAddCounterAsync(varFeatId + "-CLICKS");
		if (await clsScreenGeral.funcHasAccessAsync("CONFIG-MANAGER"))
		{
			frmConfig frmConfig = new frmConfig(frmConfig.enTabConfig.Plugin);
			frmConfig.ShowDialog(this);
			frmConfig.Dispose();
			funcLoadPluginButtons();
			await funcSyncUpdateScreenAsync(pReloadFilter: false, pReloadData: false);
		}
	}

	private void tslLicenseAgreement_Click(object sender, EventArgs e)
	{
		Process varSysProc = new Process();
		varSysProc.StartInfo.FileName = "https://app.fiscal.io/contract/terms_and_conditions";
		try
		{
			varSysProc.Start();
		}
		catch
		{
		}
	}

	private async void lkbTaskManager_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("TASK-MANAGER", "VIEW"))
		{
			frmTaskManager frmTaskManager = new frmTaskManager();
			frmTaskManager.ShowDialog(this);
			frmTaskManager.Dispose();
			await funcLoadConsFiscalAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
		}
	}

	private void funcCallBackgroundWorkers(bool pTasks, bool pScan)
	{
		if (timerTaskRunUser.Enabled)
		{
			timerTaskRunUser_Elapsed(null, null);
		}
		if (timerTaskRunSyst.Enabled)
		{
			timerTaskRunSyst_Elapsed(null, null);
		}
	}

	private void lkbIntegration03_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		funcOpenChannelManager(string.Empty);
	}

	private async void funcOpenChannelManager(string pUserAction)
	{
		if (await clsScreenGeral.funcHasAccessAsync("CHANNEL-MANAGER"))
		{
			frmChannelManager frmChannelManager = new frmChannelManager(pUserAction);
			frmChannelManager.ShowDialog(this);
			frmChannelManager.Dispose();
			funcLoadConsFiscalAsync();
			funcLoadIntegrationButtonsAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
		}
	}

	private async void funcOpenExtSystManagerAsync()
	{
		string varFeatExtId = clsFeatureService.consFeatFiscalConnect;
		_clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
		}
		else if (await clsScreenGeral.funcHasAccessAsync("EXTSYS-MANAGER"))
		{
			frmExtSystManager frmExtSystManager = new frmExtSystManager();
			frmExtSystManager.ShowDialog(this);
			frmExtSystManager.Dispose();
			await funcLoadConsFiscalAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: false);
		}
	}

	private async Task<bool> funcCheckInitialStartupAsync()
	{
		if (!base.Visible)
		{
			return true;
		}
		if (!base.ContainsFocus)
		{
			return true;
		}
		if (!SystemInformation.UserInteractive)
		{
			return true;
		}
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		string varIsOnboardDone = await _clsDataParam.funcGetAsync("INITIAL_ONBOARD");
		if (pnStartup.Visible)
		{
			varIsOnboardDone = string.Empty;
		}
		else if (clsFunction.IsEmpty(varclsConfig.UserEmail))
		{
			varIsOnboardDone = string.Empty;
		}
		if (!clsFunction.IsEmpty(varIsOnboardDone))
		{
			return true;
		}
		clsDataFilial varclsDataFilial = new clsDataFilial();
		clsSoftwareService varclsService = new clsSoftwareService(varclsConfig);
		long varTotalFilial = await varclsDataFilial.funcGetTotalAsync();
		string varFirstIntent = await _clsDataParam.funcGetAsync("FIRST_INTENTION");
		frmOnboard.enDataType varDataType = frmOnboard.enDataType.Initial;
		if (clsFunction.IsEmpty(varclsConfig.UserEmail))
		{
			varDataType = frmOnboard.enDataType.Initial;
		}
		else if (varTotalFilial == 0L)
		{
			varDataType = frmOnboard.enDataType.InformCompany;
		}
		else if (clsFunction.IsEmpty(varFirstIntent))
		{
			if (await varclsService.funcHasPayedPlanAsync())
			{
				varIsOnboardDone = "DONE";
			}
			else
			{
				varDataType = frmOnboard.enDataType.InformChoise;
			}
		}
		else
		{
			varIsOnboardDone = "DONE";
		}
		if (!clsFunction.IsEmpty(varIsOnboardDone))
		{
			await _clsDataParam.funcSetAsync("INITIAL_ONBOARD", "DONE");
			return true;
		}
		funcStopTimerTaskRun();
		using (frmOnboard varfrmOnboard = new frmOnboard(varDataType))
		{
			varfrmOnboard.ShowDialog(this);
		}
		varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		await funcStartTimerTaskRunAsync();
		await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
		bool varIsOnboardFinished = true;
		varTotalFilial = await varclsDataFilial.funcGetTotalAsync();
		varFirstIntent = await _clsDataParam.funcGetAsync("FIRST_INTENTION");
		if (clsFunction.IsEmpty(varclsConfig.UserEmail))
		{
			varIsOnboardFinished = false;
		}
		else if (clsFunction.IsEmpty(varFirstIntent))
		{
			varIsOnboardFinished = false;
		}
		else if (varTotalFilial <= 0)
		{
			varIsOnboardFinished = false;
		}
		if (varIsOnboardFinished)
		{
			await _clsMetricService.funcTrySetOnboardAsDoneAsync();
		}
		_clsMetricService.funcSyncAsync(pSyncMarket: true).ContinueWith(async (Task<clsReturn> t) => await clsSrvGeral.funcSyncWebDataAsync(string.Empty, pHardSync: false));
		return varIsOnboardFinished;
	}

	private async Task<FilialView> funcGetFilialSelectedAsync()
	{
		TreeNode varSelectedNode = trvFilial.SelectedNode;
		if (varSelectedNode == null)
		{
			return null;
		}
		if (varSelectedNode.Tag == null)
		{
			return null;
		}
		string varNodeName = clsFunction.funcGetValue(varSelectedNode.Name);
		if (clsFunction.IsEmpty(varNodeName))
		{
			return null;
		}
		string varNodeTag = clsFunction.funcGetValue(varSelectedNode.Tag);
		if (clsFunction.IsEmpty(varNodeTag))
		{
			return null;
		}
		if (clsFunction.Contains(varNodeName, "-FUNC-"))
		{
			return null;
		}
		return await clsSrvGeral.funcGetFilialAsync(varNodeTag);
	}

	private async Task<List<FilialView>> funcGetFilialListAsync(bool pSelected = true)
	{
		List<FilialView> varFilialList = new List<FilialView>();
		clsDataFilial varclsDataFilial = new clsDataFilial();
		try
		{
			if (!clsFunction.Contains(clsFunction.funcGetValue(trvFilial.Tag), "LOCK"))
			{
				foreach (TreeNode varTreeNode in trvFilial.Nodes)
				{
					if (pSelected && !varTreeNode.Checked)
					{
						continue;
					}
					string varNodeCNPJ = clsFunction.funcGetValue(varTreeNode.Tag);
					if (!clsFunction.IsEmpty(varNodeCNPJ) && varNodeCNPJ.Length >= 11)
					{
						FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(varNodeCNPJ);
						if (varclsFilial != null)
						{
							varFilialList.Add(varclsFilial);
						}
					}
				}
			}
			if (varFilialList.Count == 0)
			{
				FilialView varclsFilial2 = await funcGetFilialSelectedAsync();
				if (pSelected && varclsFilial2 != null)
				{
					varFilialList.Add(varclsFilial2);
				}
			}
			if (!pSelected && varFilialList.Count == 0)
			{
				varFilialList = await varclsDataFilial.funcGetListAsync(pLoadDummy: false);
			}
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return varFilialList;
	}

	private async void tsbChave_Click(object sender, EventArgs e)
	{
		new List<FilialView>();
		if (!(await funcCheckInitialStartupAsync()))
		{
			return;
		}
		string varUserMessage = string.Empty;
		List<FilialView> varFilialList = await funcGetFilialListAsync();
		if (varFilialList.Count == 0)
		{
			varUserMessage = "Selecione pelo menos uma empresa para continuar !!!";
		}
		if (!clsFunction.IsEmpty(varUserMessage))
		{
			MessageBox.Show(this, varUserMessage, "Operação cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		frmDocSearch varfrmDocSearch = new frmDocSearch(varFilialList);
		varfrmDocSearch.ShowDialog(this);
		if (varfrmDocSearch.UserAction.Equals("FILTER_BY_KEY"))
		{
			clsDataFilter varclsDataFilter = await funcGetDataFilterAsync();
			varclsDataFilter.DocKeyList = varfrmDocSearch.funcGetDocKeyList();
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: false, pShowMore: false, varclsDataFilter);
			varfrmDocSearch.Dispose();
			if (clsFunction.funcIsShowBatchStatus())
			{
				funcOpenTabPage("TabBatch");
			}
			funcDefineOnBoardScreen(null);
			funcCallBackgroundWorkers(pTasks: true, pScan: false);
		}
	}

	private async void trvFeatures_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (varLoadingTree || e.Node == null || varclsTabManager == null)
		{
			return;
		}
		string varNodeName = clsFunction.funcGetValue(e.Node.Name);
		if (clsFunction.IsEmpty(varNodeName))
		{
			return;
		}
		string varNodeTag = clsFunction.funcGetValue(e.Node.Tag);
		if (clsFunction.IsEmpty(varNodeTag))
		{
			return;
		}
		TreeNode varNodeRoot = e.Node.Parent;
		if (varNodeRoot == null)
		{
			return;
		}
		string varFeatExtId = funcGetFeatExId(varNodeName);
		if (clsFunction.Contains(varNodeTag, "-FUNC-"))
		{
			varFeatExtId = funcGetFeatExId(varNodeTag);
		}
		bool varIsCheckedBox = clsFunction.Contains(varNodeTag, "-CKB");
		bool varIsReport = varNodeRoot.Name.StartsWith("RootReport");
		if (clsFunction.Contains(varFeatExtId, "-FUNC-"))
		{
			await _clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
		}
		bool varIsLocked = false;
		if (!varIsReport)
		{
			varIsLocked = await funcFeatureSelectedIsLockedAsync(e.Node);
		}
		if (varIsLocked)
		{
			await _clsDataParam.funcSetAsync(e.Node.Name, string.Empty);
		}
		else
		{
			if (varIsCheckedBox)
			{
				return;
			}
			await _clsDataParam.funcSetAsync("DataFilterNodeFocus", varNodeName);
			if (varNodeRoot.Name.StartsWith("RootGroupBy"))
			{
				foreach (TreeNode varItem in varNodeRoot.Nodes)
				{
					string varNodeItemTag = (string)varItem.Tag;
					if (varNodeItemTag == null)
					{
						continue;
					}
					string varNodeItemName = varItem.Name;
					if (varNodeItemName != null)
					{
						int varImageIndex = clsFunction.icon_default;
						bool varIsSelected = false;
						if (clsFunction.IsEqual(varNodeItemName, varNodeName))
						{
							varImageIndex = clsFunction.icon_selected;
							varIsSelected = true;
						}
						else if (clsFunction.Contains(varNodeItemTag, "PAYED", "LOCK"))
						{
							varImageIndex = clsFunction.icon_payed;
						}
						int imageIndex = (varItem.SelectedImageIndex = varImageIndex);
						varItem.ImageIndex = imageIndex;
						await _clsDataParam.funcSetAsync(varItem.Name, clsFunction.funcConvBoolToStr(varIsSelected));
					}
				}
				await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: false, pResetTabs: false);
			}
			else if (varIsReport)
			{
				intDocTabData varTabData = varclsTabManager.Get(varNodeName);
				bool varLoadDataHere = false;
				if (varTabData != null && !tabContent.TabPages.ContainsKey(varTabData.TabName))
				{
					varTabData = null;
				}
				if (varTabData == null)
				{
					await varclsTabManager.AddAsync(varNodeName, pSelect: true);
					varTabData = varclsTabManager.Get(varNodeName);
					if (varTabData == null)
					{
						return;
					}
					if (tabContent.TabPages.Count <= 0)
					{
						varLoadDataHere = true;
					}
					if (!(await clsScreenGeral.funcHasAccessAsync("ACCESS-REPORT", "VIEW", varTabData.TabName)))
					{
						await varclsTabManager.RemoveAsync(varTabData.TabName);
					}
					else
					{
						tabContent.TabPages.Add(varTabData.funcGetTabPage());
					}
				}
				if (varTabData == null)
				{
					return;
				}
				tabContent.SelectTab(varTabData.TabName);
				if (varLoadDataHere)
				{
					await funcLoadTabViewDataAsync(pSetTool: true, pSetTotal: true, pResetTabs: false);
				}
				if (clsFunction.IsEmpty(tsbSearchTerm.Text))
				{
					await funcClearSearchTermAsync();
				}
			}
			if (trvFeatures.SelectedNode != null)
			{
				trvFeatures.SelectedNode.EnsureVisible();
				trvFeatures.Focus();
			}
		}
	}

	private async void trvFeatures_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		if (varLoadingTree)
		{
			e.Cancel = true;
			return;
		}
		if (e.Node == null)
		{
			e.Cancel = true;
			return;
		}
		if (varclsTabManager == null)
		{
			e.Cancel = true;
			return;
		}
		if (await funcFeatureSelectedIsLockedAsync(e.Node))
		{
			e.Cancel = true;
			await _clsDataParam.funcSetAsync(e.Node.Name, string.Empty);
		}
		string varNodeName = clsFunction.funcGetValue(e.Node.Name);
		if (clsFunction.IsEmpty(varNodeName))
		{
			return;
		}
		string varNodeTag = clsFunction.funcGetValue(e.Node.Tag);
		if (!clsFunction.IsEmpty(varNodeTag) && e.Node.Parent != null)
		{
			string varFeatExtId = funcGetFeatExId(varNodeName);
			if (clsFunction.Contains(varNodeTag, "-FUNC-"))
			{
				varFeatExtId = funcGetFeatExId(varNodeTag);
			}
			if (clsFunction.Contains(varFeatExtId, "-FUNC-"))
			{
				await _clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
			}
		}
	}

	private async void trvFeatures_AfterCheck(object sender, TreeViewEventArgs e)
	{
		if (varLoadingTree)
		{
			return;
		}
		TreeNode varNodeRoot = e.Node.Parent;
		if (varNodeRoot == null)
		{
			return;
		}
		varLoadingTree = true;
		bool varMustLoad = false;
		string varNodeName = clsFunction.funcGetValue(varNodeRoot.Name);
		string varNodeChecked = clsFunction.funcConvBoolToStr(e.Node.Checked);
		if (varNodeName.StartsWith("RootFilterBy"))
		{
			await _clsDataParam.funcSetAsync(e.Node.Name, varNodeChecked);
			varMustLoad = true;
			if (clsFunction.Contains(e.Node.Name, "FilterByDocFisc"))
			{
				foreach (TreeNode varChildNode in e.Node.Nodes)
				{
					varChildNode.Checked = e.Node.Checked;
					await _clsDataParam.funcSetAsync(varChildNode.Name, varNodeChecked);
				}
			}
		}
		else if (varNodeName.StartsWith("FilterByDocFisc"))
		{
			await _clsDataParam.funcSetAsync(e.Node.Name, varNodeChecked);
			varMustLoad = true;
			int varTotalChecked = 0;
			foreach (TreeNode node in varNodeRoot.Nodes)
			{
				if (node.Checked)
				{
					varTotalChecked++;
				}
			}
			if (varTotalChecked.Equals(varNodeRoot.Nodes.Count))
			{
				varNodeRoot.Checked = true;
				varNodeChecked = "X";
			}
			else
			{
				varNodeRoot.Checked = false;
				varNodeChecked = string.Empty;
			}
			await _clsDataParam.funcSetAsync(varNodeRoot.Name, varNodeChecked);
		}
		if (!clsFunction.IsEmpty(await _clsDataParam.funcGetAsync("NotFilterByClick")))
		{
			varMustLoad = false;
		}
		if (varMustLoad)
		{
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true, pShowMore: false, null, pSyncAudit: false);
		}
		varLoadingTree = false;
	}

	private async Task<clsReturn> funcLoadTabViewDataAsync(bool pSetTool, bool pSetTotal, bool pResetTabs, bool pShowMore = false, clsDataFilter pclsDataFilter = null, bool pSyncAudit = true)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		if (varLoadingTabs)
		{
			return varclsReturnFunc;
		}
		if (Program.NotLoadDataInStart)
		{
			return varclsReturnFunc;
		}
		try
		{
			if (varclsTabManager == null)
			{
				return varclsReturnFunc;
			}
			intDocTabData varTabData = varclsTabManager.Selected();
			if (varTabData == null)
			{
				return varclsReturnFunc;
			}
			Application.UseWaitCursor = true;
			tsbAtualizar01.Image = Resources.gif_loading;
			tsbAtualizar01.ToolTipText = "Carregando documentos ...";
			funcSyncLockedFeatures(varTabData);
			ToolStripStatusLabel toolStripStatusLabel = stsSumSep03;
			bool visible = (stsPageWarn.Visible = true);
			toolStripStatusLabel.Visible = visible;
			stsPageWarn.Text = " Carregando documentos ";
			stsPageWarn.BackColor = SystemColors.Info;
			stsPageWarn.Image = Resources.gif_loading;
			Application.DoEvents();
			if (pResetTabs)
			{
				varclsTabManager.funcResetLoadStatus();
			}
			if (pclsDataFilter == null)
			{
				pclsDataFilter = await funcGetDataFilterAsync();
			}
			await varTabData.funcLoadDataAsync(pclsDataFilter);
			int varTotalDocs;
			decimal vrTotalValue;
			try
			{
				varTotalDocs = varTabData.funcGetTotalDocs();
				vrTotalValue = varTabData.funcGetTotalValue();
			}
			catch
			{
				varTotalDocs = 0;
				vrTotalValue = default(decimal);
			}
			long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
			stsTotalQuant.Text = $"Documentos : {varTotalDocs}";
			stsTotalValue.Text = "Valor Total : " + $"{vrTotalValue:C}";
			if (!clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("DbaQueryLimit", pBuffer: true, pGlobal: true)))
			{
				ToolStripStatusLabel toolStripStatusLabel2 = stsSumSep03;
				visible = (stsPageWarn.Visible = false);
				toolStripStatusLabel2.Visible = visible;
			}
			else if (varTotalDocs >= varPageSize)
			{
				stsPageWarn.Image = Resources.image_warning;
				stsPageWarn.Text = $" Primeiros {varPageSize} documentos ";
				ToolStripStatusLabel toolStripStatusLabel3 = stsSumSep03;
				visible = (stsPageWarn.Visible = true);
				toolStripStatusLabel3.Visible = visible;
			}
			else
			{
				ToolStripStatusLabel toolStripStatusLabel4 = stsSumSep03;
				visible = (stsPageWarn.Visible = false);
				toolStripStatusLabel4.Visible = visible;
			}
			stsSumary.Visible = varTabData.ShowSumary();
			Application.DoEvents();
			if (pSyncAudit || pResetTabs)
			{
				await funcSyncAuditNodeDataAsync(pclsDataFilter);
			}
			if (pSetTool)
			{
				varclsTabManager.funcSetToolsAsync(toolbarDocs01, trvFeatures);
			}
			if (pSetTotal && varTabData != null)
			{
				funcCalcRepTotalAsync(pclsDataFilter, pShowMore);
			}
			if (!clsFunction.IsEmpty(tsbSearchTerm.Text))
			{
				await funcClearSearchTermAsync(varTabData);
			}
			funcSyncLockedFeatures(varTabData);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			varLoadingData = false;
			tsbAtualizar01.Image = Resources.image_refresh;
			tsbAtualizar01.ToolTipText = "Atualizar lista de documentos na tela";
			Application.UseWaitCursor = false;
		}
		if (varclsReturnFunc.HasError)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
		}
		return varclsReturnFunc;
	}

	private async Task<bool> funcClearSearchTermAsync(intDocTabData pclsTabData)
	{
		if (pclsTabData == null)
		{
			return false;
		}
		try
		{
			varLoadingData = true;
			bool varSearchInteligent = await funcShowSearchInteligentAsync();
			pclsTabData.funcSearchText(string.Empty, varSearchInteligent);
		}
		finally
		{
			varLoadingData = false;
		}
		return true;
	}

	private async Task<clsDataFilter> funcGetDataFilterAsync()
	{
		clsDataFilter varclsDataFilter = new clsDataFilter();
		List<FilialView> varFilialList = await funcGetFilialListAsync();
		varclsDataFilter.FilialList.AddRange(varFilialList);
		await varclsDataFilter.funcLoadDataAsync();
		return varclsDataFilter;
	}

	private async void tabContent_ControlRemoved(object sender, ControlEventArgs e)
	{
		if ((TabControl)sender != null)
		{
			Control varControl = e.Control;
			if (varControl != null && varControl.Name.StartsWith("Tab"))
			{
				await varclsTabManager.RemoveAsync(varControl.Name);
				varControl.Dispose();
				trvFeatures.SelectedNode = null;
				GC.Collect();
			}
		}
	}

	private void funcSyncLockedFeatures(intDocTabData pclsTabData)
	{
		if (pclsTabData != null)
		{
			bool varIsEnabled = !pclsTabData.IsLocked();
			tsbEventosNFe.Enabled = varIsEnabled;
			tsbConsStatusDFeSimple.Enabled = varIsEnabled;
			tsbConsExtSyst.Enabled = varIsEnabled;
			tsbTags.Enabled = varIsEnabled;
			tsbViewEdiAll.Enabled = varIsEnabled;
			tsbBaixarNotFis.Enabled = varIsEnabled;
			tsbBaixarConemb.Enabled = varIsEnabled;
			tsbExport.Enabled = varIsEnabled;
			tsbExportar.Enabled = varIsEnabled;
			tsbImprimir.Enabled = varIsEnabled;
			tsbSendDoc.Enabled = varIsEnabled;
			tsbIntegrations.Enabled = varIsEnabled;
			tsbPlugins.Enabled = varIsEnabled;
		}
	}

	private void tabContent_SelectedIndexChanged(object sender, EventArgs e)
	{
		TabPage varTabPage = tabContent.SelectedTab;
		if (varTabPage != null)
		{
			intDocTabData varTabData = varclsTabManager.Get(varTabPage.Name);
			if (varTabData != null)
			{
				funcSyncLockedFeatures(varTabData);
			}
		}
	}

	private async void tabContent_Selected(object sender, TabControlEventArgs e)
	{
		_ = 3;
		try
		{
			if (varLoadingTabs || e.TabPage == null)
			{
				return;
			}
			await varclsTabManager.SelectAsync(e.TabPage.Name);
			intDocTabData varTabData = varclsTabManager.Get(e.TabPage.Name);
			if (varTabData == null)
			{
				return;
			}
			if (!varTabData.IsLoaded())
			{
				await funcLoadTabViewDataAsync(pSetTool: true, pSetTotal: true, pResetTabs: false, pShowMore: true);
			}
			else
			{
				varclsTabManager.funcSetToolsAsync(toolbarDocs01, trvFeatures);
			}
			e.TabPage.SuspendLayout();
			e.TabPage.BackColor = varTabData.TabColor;
			foreach (TabPage varTabPage in tabContent.TabPages)
			{
				if (!varTabPage.Equals(e.TabPage))
				{
					varTabPage.BackColor = Color.WhiteSmoke;
				}
			}
			string obj = clsFunction.funcGetValue(e.TabPage.Name);
			bool varHideFilter = false;
			if (obj.Equals("TabAuditor"))
			{
				varHideFilter = true;
			}
			if (obj.Equals("TabBatch"))
			{
				varHideFilter = true;
			}
			if (obj.Equals("TabTools"))
			{
				varHideFilter = true;
			}
			if (obj.Equals("TabPartner"))
			{
				varHideFilter = true;
			}
			if (varHideFilter)
			{
				splitMonitorRight.Panel2Collapsed = true;
			}
			else
			{
				string varIsCollapsed = await _clsDataParam.funcGetAsync("MonitorIsCollapsed");
				SplitContainer splitContainer = splitMonData;
				bool panel1Collapsed = (splitMonitorRight.Panel2Collapsed = !clsFunction.IsEmpty(varIsCollapsed));
				splitContainer.Panel1Collapsed = panel1Collapsed;
			}
			bool varSearchInteligent = await funcShowSearchInteligentAsync();
			funcGetSearchTerm(tsbSearchTerm.Text);
			varTabData.funcSearchText(tsbSearchTerm.Text, varSearchInteligent);
			stsTotalQuant.Text = "Documentos : " + varTabData.funcGetTotalDocs();
			decimal vrTotalValue = varTabData.funcGetTotalValue();
			stsTotalValue.Text = "Valor Total : " + $"{vrTotalValue:C}";
			e.TabPage.ResumeLayout();
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private string funcGetSearchTerm(string pSearchTerm)
	{
		int varIndexOf = pSearchTerm.IndexOf(":");
		pSearchTerm = ((varIndexOf > 0) ? clsFunction.funcSubString(pSearchTerm, varIndexOf + 1, 99999) : ((pSearchTerm.IndexOf(",") > 0) ? string.Empty : ((!pSearchTerm.StartsWith("sql=")) ? tsbSearchTerm.Text : string.Empty)));
		return pSearchTerm;
	}

	private async void tsbAtualizar01_Click(object sender, EventArgs e)
	{
		Program.NotLoadDataInStart = false;
		await _clsDataParam.funcSetAsync("SearchTerm", tsbSearchTerm.Text);
		await _clsDataParam.funcSetAsync("BeginDate", tsbDataIni.Text);
		await _clsDataParam.funcSetAsync("EndDate", tsbDataFim.Text);
		await _clsDataParam.funcSetAsync("PageSize", tsbPageSize.Text);
		await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
	}

	private async void tsbSearchTerm_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode.Equals(Keys.Return))
		{
			await _clsDataParam.funcSetAsync("SearchTerm", tsbSearchTerm.Text);
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
		}
	}

	private async void tsbDataIni_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			await _clsDataParam.funcSetAsync("BeginDate", tsbDataIni.Text);
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
		}
	}

	private async void tsbDataIni_Leave(object sender, EventArgs e)
	{
		await _clsDataParam.funcSetAsync("BeginDate", tsbDataIni.Text);
		tsbDataIni.Text = clsFunction.funcFormatData(tsbDataIni.Text, pCorrect: true);
	}

	private async void tsbDataFim_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			await _clsDataParam.funcSetAsync("EndDate", tsbDataFim.Text);
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
		}
	}

	private async void tsbDataFim_Leave(object sender, EventArgs e)
	{
		await _clsDataParam.funcSetAsync("EndDate", tsbDataFim.Text);
		tsbDataFim.Text = clsFunction.funcFormatData(tsbDataFim.Text, pCorrect: true);
	}

	private async void tsbPageSize_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			await _clsDataParam.funcSetAsync("PageSize", tsbPageSize.Text);
			await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
		}
	}

	private async void tsbPageSize_Leave(object sender, EventArgs e)
	{
		await _clsDataParam.funcSetAsync("PageSize", tsbPageSize.Text);
	}

	private void tabContent_MouseClick(object sender, MouseEventArgs e)
	{
		TabControl varTabControl = (TabControl)sender;
		Point varPoint = e.Location;
		int varTabWidth = 0;
		varTabWidth = tabContent.GetTabRect(varTabControl.SelectedIndex).Width - _imgHitArea.X;
		Rectangle varRectangle = tabContent.GetTabRect(varTabControl.SelectedIndex);
		varRectangle.Offset(varTabWidth, _imgHitArea.Y);
		varRectangle.Width = 13;
		varRectangle.Height = 13;
		if (varRectangle.Contains(varPoint))
		{
			TabPage varTabPage = varTabControl.TabPages[varTabControl.SelectedIndex];
			varTabControl.TabPages.Remove(varTabPage);
		}
	}

	private async Task<bool> funcSetSelectedTscFilialAsync()
	{
		try
		{
			if (!splitMonData.Panel1Collapsed)
			{
				return false;
			}
			if (clsFunction.Contains((string)trvFilial.Tag, "LOCK"))
			{
				return false;
			}
			int varTotalSelected = 0;
			TreeNode varSelectedNode = null;
			foreach (TreeNode varTreeNode in trvFilial.Nodes)
			{
				if (varTreeNode.Checked)
				{
					varSelectedNode = varTreeNode;
					varTotalSelected++;
				}
			}
			bool varMustLoadData = false;
			if (varTotalSelected > 1 || varSelectedNode == null)
			{
				await varclsFilialManager.funcSetNodeCheckBoxAsync(pChecked: true);
				tscFiliais.SelectedIndex = 0;
				varMustLoadData = true;
			}
			else
			{
				tscFiliais.SelectedIndex = varSelectedNode.Index;
			}
			if (varMustLoadData)
			{
				TreeViewEventArgs varArguments = new TreeViewEventArgs(trvFilial.TopNode, TreeViewAction.ByMouse);
				trvFilial_AfterCheck(trvFilial, varArguments);
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return true;
	}

	private async Task<bool> funcLicenseSyncFiscalAsync()
	{
		if (!(await funcCheckInitialStartupAsync()))
		{
			return false;
		}
		funcSetTaskStatus(new clsTaskStatus("Obtendo informações sobre atualizações ..."));
		new clsUpdaterService().funcUserCheckNow();
		bool varExitLoop = false;
		while (!varExitLoop && Process.GetProcessesByName("updater").Length != 0)
		{
			await Task.Delay(TimeSpan.FromSeconds(2.0));
		}
		funcSetTaskStatus(new clsTaskStatus("Obtendo informações sobre licenças de uso ..."));
		clsReturn varclsReturnFunc = await _clsMetricService.funcSyncAsync(pSyncMarket: false);
		if (varclsReturnFunc.HasError)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
		}
		await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		MessageBox.Show("Sincronização de licenças executada com sucesso!", "Operação realizada com sucesso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		return true;
	}

	private async void tsbSearchTerm_KeyPress(object sender, KeyPressEventArgs e)
	{
		clsCmdService varclsCmdService = new clsCmdService();
		new clsDataDoc();
		if (!e.KeyChar.ToString().Equals("/"))
		{
			return;
		}
		e.Handled = true;
		tsbSearchTerm.Text = string.Empty;
		frmSpecialCmd varfrmSpecialCmd = new frmSpecialCmd();
		DialogResult varResult = varfrmSpecialCmd.ShowDialog(this);
		tsbSearchTerm.Text = string.Empty;
		if (!varResult.Equals(DialogResult.OK))
		{
			return;
		}
		string varCommand = varfrmSpecialCmd.funcGetCommand();
		varCommand = clsFunction.funcGetValue(varCommand).ToLower();
		varfrmSpecialCmd.Dispose();
		if (clsFunction.IsEmpty(varCommand))
		{
			return;
		}
		if (varCommand.Equals("message02"))
		{
			using (frmBannerBackup varFrmBanner = new frmBannerBackup())
			{
				varFrmBanner.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("message03"))
		{
			clsTaskStatus varclsTaskStatus = new clsTaskStatus();
			varclsTaskStatus.Show = true;
			varclsTaskStatus.FormName = enPanelName.PanelXmlValidator;
			funcSetTaskStatus(varclsTaskStatus);
			return;
		}
		if (varCommand.Equals("message04"))
		{
			using (frmBannerFiscalioServer varFrmBanner2 = new frmBannerFiscalioServer())
			{
				varFrmBanner2.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("fullstorage"))
		{
			using (frmBannerStorageFull varFrmBanner3 = new frmBannerStorageFull())
			{
				varFrmBanner3.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("panelfull"))
		{
			clsTaskStatus varclsTaskStatus2 = new clsTaskStatus();
			varclsTaskStatus2.Show = true;
			varclsTaskStatus2.FormName = enPanelName.PanelStorageFull;
			funcSetTaskStatus(varclsTaskStatus2);
			return;
		}
		if (varCommand.Equals("panelalmost"))
		{
			clsTaskStatus varclsTaskStatus3 = new clsTaskStatus();
			varclsTaskStatus3.Show = true;
			varclsTaskStatus3.FormName = enPanelName.PanelStorageAlmostFull;
			funcSetTaskStatus(varclsTaskStatus3);
			return;
		}
		if (varCommand.Equals("almostfull"))
		{
			using (frmBannerStorageAlmostFull varFrmBanner4 = new frmBannerStorageAlmostFull())
			{
				varFrmBanner4.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("message05"))
		{
			clsTaskStatus varclsTaskStatus4 = new clsTaskStatus();
			varclsTaskStatus4.Show = true;
			varclsTaskStatus4.FormName = enPanelName.PanelMoreReports;
			funcSetTaskStatus(varclsTaskStatus4);
			return;
		}
		if (varCommand.Equals("message06"))
		{
			using (frmBannerSearchCFeSat varFrmBanner5 = new frmBannerSearchCFeSat())
			{
				varFrmBanner5.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("message07"))
		{
			using (frmBannerSearchAverb varFrmBanner6 = new frmBannerSearchAverb())
			{
				varFrmBanner6.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("message08"))
		{
			using (frmBannerSearchNFeNFCeCTeGO varFrmBanner7 = new frmBannerSearchNFeNFCeCTeGO())
			{
				varFrmBanner7.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("message09"))
		{
			using (frmBannerSearchNFeMT varFrmBanner8 = new frmBannerSearchNFeMT())
			{
				varFrmBanner8.ShowDialog();
				return;
			}
		}
		if (varCommand.Equals("message10"))
		{
			using (frmBannerSearchNFSe varFrmBanner9 = new frmBannerSearchNFSe())
			{
				varFrmBanner9.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("message11"))
		{
			using (frmBannerSpecialCredits varFrmBanner10 = new frmBannerSpecialCredits())
			{
				varFrmBanner10.ShowDialog();
				return;
			}
		}
		if (varCommand.Equals("onboard"))
		{
			funcStopTimerTaskRun();
			using (frmOnboard varFrmOnboard = new frmOnboard(frmOnboard.enDataType.Initial))
			{
				varFrmOnboard.ShowDialog();
			}
			_clsMetricService.funcSyncAsync(pSyncMarket: true);
			await new clsDataConfig().funcGetItemByKeyAsync();
			await funcStartTimerTaskRunAsync();
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			return;
		}
		if (varCommand.Equals("getuserdata"))
		{
			using (frmGetUserData varFrmGetUserData = new frmGetUserData())
			{
				varFrmGetUserData.ShowDialog();
				return;
			}
		}
		if (varCommand.Equals("license"))
		{
			frmLicense frmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
			frmLicense.ShowDialog(this);
			frmLicense.Dispose();
			return;
		}
		if (varCommand.Equals("rundbafixer"))
		{
			funcSetTaskStatus(new clsTaskStatus("Processsando script de banco de dados..."));
			clsReturn varclsReturnFunc = new clsReturn();
			using (intDatabase varclsDataBase = await new clsDbaFactory().funcGetClassAsync())
			{
				varclsReturnFunc = await varclsDataBase.funcInitializeAsync(pHardInit: true, pRunFixer: true);
			}
			if (varclsReturnFunc.HasError)
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
			}
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.StartsWith("image:"))
		{
			funcSetTaskStatus(new clsTaskStatus("Enviado imagens ao Api.Fiscal.io..."));
			string varImageFolder = varCommand.Replace("image:", "");
			if (!Directory.Exists(varImageFolder))
			{
				MessageBox.Show("Diretório " + varImageFolder + " não existe.", "Operação cancelada");
				return;
			}
			string varFolderDone = clsFunction.funcFixFilePath(varImageFolder + "\\Done");
			if (!Directory.Exists(varFolderDone))
			{
				Directory.CreateDirectory(varFolderDone);
			}
			string[] varFileList = Directory.GetFiles(varImageFolder);
			clsImageService varclsImageService = new clsImageService();
			clsReturn varclsReturnFunc2 = new clsReturn();
			string[] array = varFileList;
			foreach (string varFilePath in array)
			{
				FileInfo varFileInfo = new FileInfo(varFilePath);
				string varImage = Convert.ToBase64String(File.ReadAllBytes(varFileInfo.FullName));
				string varText = varFileInfo.Name.Replace(varFileInfo.Extension, "");
				clsReturn varResultPost = await varclsImageService.funcPostAsync(new ImagePostModel
				{
					imaType = "RECTV",
					imaText = varText,
					imaImage = varImage
				});
				if (varResultPost.HasError)
				{
					varclsReturnFunc2.AddRange(varResultPost.Messages);
				}
				File.Move(varFilePath, varFolderDone + "\\" + varFileInfo.Name);
			}
			if (varclsReturnFunc2.HasError)
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc2);
			}
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.Equals("docanalizer"))
		{
			if (varclsTabManager == null)
			{
				return;
			}
			intDocTabData varTabData = varclsTabManager.Selected();
			if (varTabData != null)
			{
				funcSetTaskStatus(new clsTaskStatus("Analisando os documentos..."));
				List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true);
				clsReturn varclsReturnFunc3 = await new clsAnalizer().funcExecuteAsync(varDocList);
				if (varclsReturnFunc3.HasError)
				{
					clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc3);
				}
				funcSetTaskStatus(new clsTaskStatus(pShow: false));
			}
			return;
		}
		if (varCommand.Equals("xmlvalidation"))
		{
			if (varclsTabManager == null)
			{
				return;
			}
			intDocTabData varTabData2 = varclsTabManager.Selected();
			if (varTabData2 != null)
			{
				funcSetTaskStatus(new clsTaskStatus("Executando validação dos XMLs ..."));
				List<Document> varDocList2 = await funcGetDocListAsync(varTabData2, pFocused: false, pChecked: true);
				clsReturn varclsReturnFunc4 = await new clsValidatorService().funcExecuteAsync(varDocList2);
				if (varclsReturnFunc4.HasError)
				{
					clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc4);
				}
				funcSetTaskStatus(new clsTaskStatus(pShow: false));
			}
			return;
		}
		if (varCommand.Contains("consdist"))
		{
			await funcConsDistDocsAsync(varCommand);
			return;
		}
		if (varCommand.Contains("scan"))
		{
			await funcScanDistDocsAsync(varCommand);
			return;
		}
		if (varCommand.Contains("fixnsudoclist"))
		{
			await funcFixNsuDocListAsync();
			return;
		}
		if (varCommand.Contains("hideprice"))
		{
			await _clsDataParam.funcSetAsync("HIDE_PRICE", "X", pGlobal: true);
			MessageBox.Show(this, "Status de PastScan Price definida.", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Contains("showprice"))
		{
			await _clsDataParam.funcSetAsync("HIDE_PRICE", "", pGlobal: true);
			MessageBox.Show(this, "Status de PastScan Price removida.", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.StartsWith("docreload"))
		{
			if (varclsTabManager == null)
			{
				return;
			}
			intDocTabData varTabData3 = varclsTabManager.Selected();
			if (varTabData3 != null)
			{
				List<Document> varDocList3 = await funcGetDocListAsync(varTabData3, pFocused: false, pChecked: true);
				bool varHasEvent = varCommand.EndsWith("evt");
				clsRebuildService clsRebuildService = new clsRebuildService();
				clsRebuildService.EventLongRunner += funcEventLongRunner;
				clsReturn varclsReturnFunc5 = await clsRebuildService.funcReloadDocsAsync(varDocList3, varHasEvent);
				if (varclsReturnFunc5.HasError)
				{
					funcSetTaskStatus(varclsReturnFunc5);
				}
				else
				{
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.Equals("server"))
		{
			new frmServerManager().ShowDialog(this);
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			return;
		}
		if (varCommand.Equals("workflow"))
		{
			new frmWorkFlowManager("").ShowDialog(this);
			return;
		}
		if (varCommand.Equals("connect"))
		{
			funcOpenExtSystManagerAsync();
			return;
		}
		if (varCommand.Equals("dbawizard"))
		{
			funcOpenDbaWizardAsync();
			return;
		}
		if (varCommand.Equals("fixdummny"))
		{
			if (varclsTabManager == null)
			{
				return;
			}
			intDocTabData varTabData4 = varclsTabManager.Selected();
			if (varTabData4 == null)
			{
				return;
			}
			funcSetTaskStatus(new clsTaskStatus("Corrigindo documentos in Dummny Filial"));
			foreach (Document item in await funcGetDocListAsync(varTabData4, pFocused: false, pChecked: true))
			{
				await clsDataGeral.funcClearDummyDocAsync(item);
			}
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.Equals("getsatlead"))
		{
			funcSetTaskStatus(new clsTaskStatus("Obtendo lista de software houses ..."));
			clsReturn varclsReturnFunc6 = await clsScreenGeral.funcGetSatLeadListAsync();
			if (varclsReturnFunc6.HasError)
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc6);
			}
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.Equals("fixtomador"))
		{
			clsReturn varclsReturnFunc2 = new clsReturn();
			if (varclsTabManager == null)
			{
				return;
			}
			intDocTabData varTabData5 = varclsTabManager.Selected();
			if (varTabData5 != null)
			{
				List<Document> varDocList4 = await funcGetDocListAsync(varTabData5, pFocused: false, pChecked: true);
				funcSetTaskStatus(new clsTaskStatus("Corrigindo tomador incorreto para NFe ..."));
				long varPageSize = clsFunction.funcConvStrToLong((await funcGetDataFilterAsync()).PageSize);
				if (!clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("DbaQueryLimit", pBuffer: true, pGlobal: true)))
				{
					varPageSize = 0L;
				}
				if (varDocList4.Count > 0)
				{
					varclsReturnFunc2.AddRange((await varclsCmdService.funcFixTomadorAsync(varDocList4)).Messages);
				}
				else
				{
					varclsReturnFunc2.AddRange((await varclsCmdService.funcFixTomadorAsync("NFe", varPageSize)).Messages);
				}
				funcSetTaskStatus(new clsTaskStatus("Corrigindo tomador incorreto para CTe ..."));
				if (varDocList4.Count > 0)
				{
					varclsReturnFunc2.AddRange((await varclsCmdService.funcFixTomadorAsync(varDocList4)).Messages);
				}
				else
				{
					varclsReturnFunc2.AddRange((await varclsCmdService.funcFixTomadorAsync("CTe", varPageSize)).Messages);
				}
				if (varclsReturnFunc2.HasError)
				{
					clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc2);
				}
				funcSetTaskStatus(new clsTaskStatus(pShow: false));
			}
			return;
		}
		if (varCommand.Equals("fixcancel"))
		{
			if (varclsTabManager != null)
			{
				intDocTabData varTabData6 = varclsTabManager.Selected();
				if (varTabData6 != null)
				{
					funcSetTaskStatus(new clsTaskStatus("Corrigindo status dos documentos..."));
					await varclsCmdService.funcFixDocCancelAsync(await funcGetDocListAsync(varTabData6, pFocused: false, pChecked: true));
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.StartsWith("fixhasxml"))
		{
			if (varclsTabManager != null)
			{
				intDocTabData varTabData7 = varclsTabManager.Selected();
				if (varTabData7 != null)
				{
					funcSetTaskStatus(new clsTaskStatus("Corrigindo status do campo [HasXML] dos documentos..."));
					List<Document> varDocList5 = await funcGetDocListAsync(varTabData7, pFocused: false, pChecked: true, pSelectAll: true, pSyncFromDbaFirst: false);
					bool varWithLog = varCommand.EndsWith("log");
					await varclsCmdService.funcFixHasXmlAsync(varDocList5, varWithLog);
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.StartsWith("fixduplevent"))
		{
			if (varclsTabManager != null)
			{
				intDocTabData varTabData8 = varclsTabManager.Selected();
				if (varTabData8 != null)
				{
					funcSetTaskStatus(new clsTaskStatus("Removendo eventos duplicados ..."));
					List<Document> varDocList6 = await funcGetDocListAsync(varTabData8, pFocused: false, pChecked: true);
					bool varWithLog2 = varCommand.EndsWith("log");
					await varclsCmdService.funcFixDuplEventAsync(varDocList6, varWithLog2);
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.Equals("fixdocaudit"))
		{
			funcSetTaskStatus(new clsTaskStatus("Recriando tabelas do Fiscal.io Auditor..."));
			clsReturn varclsReturnFunc7 = await varclsCmdService.funcFixAuditTablesAsync();
			if (varclsReturnFunc7.HasError)
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc7);
			}
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.Equals("cienc-down"))
		{
			if (varclsTabManager != null)
			{
				intDocTabData varTabData9 = varclsTabManager.Selected();
				if (varTabData9 != null)
				{
					funcSetTaskStatus(new clsTaskStatus("Criando tarefa(s) para Ciência da Operação + Download..."));
					await varclsCmdService.funcAddEventCienciaDownAsync(await funcGetDocListAsync(varTabData9, pFocused: false, pChecked: true));
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.Equals("download-wbs"))
		{
			if (varclsTabManager != null)
			{
				intDocTabData varTabData10 = varclsTabManager.Selected();
				if (varTabData10 != null)
				{
					funcSetTaskStatus(new clsTaskStatus("Criando tarefa(s) para Download via Manifestação..."));
					await varclsCmdService.funcAddEventDownloadWbsAsync(await funcGetDocListAsync(varTabData10, pFocused: false, pChecked: true));
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.Equals("ngen"))
		{
			funcSetTaskStatus(new clsTaskStatus("Realizando otimização dos programas"));
			clsNgenService.funcExecuteNgenProcess();
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.Equals("fixemissor"))
		{
			if (varclsTabManager != null)
			{
				intDocTabData varTabData11 = varclsTabManager.Selected();
				if (varTabData11 != null)
				{
					funcSetTaskStatus(new clsTaskStatus("Corrigindo emissor incorreto..."));
					await varclsCmdService.funcFixEmissorAsync(await funcGetDocListAsync(varTabData11, pFocused: false, pChecked: true));
					funcSetTaskStatus(new clsTaskStatus(pShow: false));
				}
			}
			return;
		}
		if (varCommand.Equals("copycert"))
		{
			FilialView varclsFilial = await funcGetFilialSelectedAsync();
			if (varclsFilial != null)
			{
				CertContent varclsContent = await new clsDataCertContent().funcGetItemByKeyAsync(varclsFilial.Certificado);
				if (varclsContent != null)
				{
					string varZipContent = new clsSecurity().funcEncryptBytesSymetric(varclsContent.Content.ToArray(), pInit16: true);
					Clipboard.SetText(varclsContent.Password + Environment.NewLine + Environment.NewLine + varZipContent);
					MessageBox.Show(this, "Comando executado com sucesso", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			return;
		}
		if (varCommand.Equals("pastcert"))
		{
			clsSecurity varclsSecurity = new clsSecurity();
			clsDataCertContent varclsDataContent = new clsDataCertContent();
			CertContent varclsCertContent = new CertContent();
			string varMessage01 = "Copie o conteúdo na area de transferência e pressione ENTER";
			if (MessageBox.Show(this, varMessage01, "Pergunta", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk).Equals(DialogResult.Cancel))
			{
				return;
			}
			string varContent = Clipboard.GetText();
			if (clsFunction.IsEmpty(varContent))
			{
				return;
			}
			int varSeparator = varContent.IndexOf(Environment.NewLine);
			string varContent2 = varContent.Substring(0, varSeparator);
			if (!clsFunction.IsEmpty(varContent2))
			{
				varclsCertContent.Password = varContent2;
				string varContent3 = clsFunction.funcSubString(varContent, varSeparator + 4, 99999999);
				if (!clsFunction.IsEmpty(varContent3))
				{
					byte[] varContBytes = varclsSecurity.funcDecryptBytesSymetric(varContent3);
					varclsCertContent.Content = new MemoryStream(varContBytes);
					string varPassword = varclsSecurity.funcDecryptSymetric(varclsCertContent.Password);
					X509Certificate2 varX509Cert = new X509Certificate2(varContBytes, varPassword);
					varclsCertContent.Serial = varX509Cert.SerialNumber;
					await varclsDataContent.funcInsertAsync(varclsCertContent, await clsSrvGeral.funcGetCertificateAsync(varclsCertContent));
					await new clsSingleCertificates().funcGetListAsync(CancellationToken.None, null, pReload: true);
					MessageBox.Show(this, "Comando executado com sucesso", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			return;
		}
		if (varCommand.Equals("savecert"))
		{
			MessageBox.Show(this, "Opção desabilitada pela Fiscal.io.", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("savecertsecret"))
		{
			clsSecurity varclsSecurity2 = new clsSecurity();
			FilialView varclsFilial2 = await funcGetFilialSelectedAsync();
			if (varclsFilial2 == null)
			{
				return;
			}
			CertContent varclsContent2 = await new clsDataCertContent().funcGetItemByKeyAsync(varclsFilial2.Certificado);
			if (varclsContent2 != null)
			{
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
				folderBrowserDialog.ShowDialog(this);
				string varFolderPath = folderBrowserDialog.SelectedPath;
				folderBrowserDialog.Dispose();
				string varPassword2 = varclsSecurity2.funcDecryptSymetric(varclsContent2.Password);
				string varFilePath2 = clsFunction.funcFixFilePath(varFolderPath + "\\Certificate - Senha " + varPassword2 + ".pfx");
				using (FileStream varFileStream = new FileStream(varFilePath2, FileMode.Create, FileAccess.Write))
				{
					varclsContent2.Content.WriteTo(varFileStream);
				}
				string varMessage2 = "Comando executado com sucesso !!!" + Environment.NewLine + Environment.NewLine;
				varMessage2 = varMessage2 + "Arquivo : " + varFilePath2 + Environment.NewLine;
				varMessage2 = varMessage2 + "Senha : " + varPassword2 + Environment.NewLine;
				MessageBox.Show(this, varMessage2, "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			return;
		}
		if (varCommand.Equals("resetnsu"))
		{
			List<FilialView> varFilialList = await funcGetFilialListAsync();
			if (varFilialList.Count == 0)
			{
				return;
			}
			foreach (FilialView varItem in varFilialList)
			{
				string text = (varItem.NSUNFSe = string.Empty);
				string text2 = (varItem.NSUMDFe = text);
				string nSUCTe = (varItem.NSUNFe = text2);
				varItem.NSUCTe = nSUCTe;
				varItem.NFeInFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.NFeInLastScan))
				{
					varItem.LastScanNFe = string.Empty;
				}
				varItem.CTeInFirstScan = string.Empty;
				if (clsFunction.IsEmpty(varItem.CTeInLastScan))
				{
					varItem.LastScanCTe = string.Empty;
				}
				varItem.MDFeInFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.MDFeInLastScan))
				{
					varItem.LastScanMDFe = string.Empty;
				}
				varItem.NFSeInFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.NFSeInLastScan))
				{
					varItem.LastScanNFSe = string.Empty;
				}
				varItem.NFeOutFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.NFeOutLastScan))
				{
					varItem.LastScanNFeOut = string.Empty;
				}
				varItem.NFCeOutFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.NFCeOutLastScan))
				{
					varItem.LastScanNFCeOut = string.Empty;
				}
				varItem.CTeOutFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.CTeOutLastScan))
				{
					varItem.LastScanCTeOut = string.Empty;
				}
				varItem.CFeOutFirstScan = "X";
				if (clsFunction.IsEmpty(varItem.CFeOutLastScan))
				{
					varItem.LastScanCFeOut = string.Empty;
				}
				await new clsDataFilial().funcUpdateAsync(varItem, pLogUserFields: true);
				clsDataSatSerial varclsDataSatSerial = new clsDataSatSerial();
				foreach (SatSerial varclsSatSerial in await varclsDataSatSerial.funcGetListByFilialAsync(varItem.CNPJ))
				{
					nSUCTe = (varclsSatSerial.LastScan = string.Empty);
					varclsSatSerial.LastConc = nSUCTe;
					await varclsDataSatSerial.funcUpdateAsync(varclsSatSerial, pLogUserFields: true);
				}
			}
			await funcLoadFiliaisAsync(pUseBuffer: false);
			string varMessage3 = "NSU reiniciado para a(s) empresa(s) :";
			foreach (FilialView varclsFilial3 in varFilialList)
			{
				varMessage3 = varMessage3 + Environment.NewLine + varclsFilial3.CNPJView + " - " + varclsFilial3.NomeView;
			}
			varMessage3 += Environment.NewLine;
			MessageBox.Show(this, varMessage3, "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("enable-hide-dest-data"))
		{
			await _clsDataParam.funcSetAsync("HIDE-DESTIN-DATA", "ACTIVE", pGlobal: true);
			MessageBox.Show(this, "Parâmetro TRIUMPH ativado com sucesso!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("disable-hide-dest-data"))
		{
			await _clsDataParam.funcSetAsync("HIDE-DESTIN-DATA", string.Empty, pGlobal: true);
			MessageBox.Show(this, "Parâmetro TRIUMPH desativado com sucesso!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("enable-hide-alfagres-data"))
		{
			await _clsDataParam.funcSetAsync("HIDE-ALFAGRES-DATA", "ACTIVE", pGlobal: true);
			MessageBox.Show(this, "Parâmetro ALFAGRES ativado com sucesso!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("disable-hide-alfagres-data"))
		{
			await _clsDataParam.funcSetAsync("HIDE-ALFAGRES-DATA", string.Empty, pGlobal: true);
			MessageBox.Show(this, "Parâmetro ALFAGRES desativado com sucesso!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("enable-hide-iestgroup-data"))
		{
			await _clsDataParam.funcSetAsync("HIDE-IESTGROUP-DATA", "ACTIVE", pGlobal: true);
			MessageBox.Show(this, "Parâmetro IEST GROUP ativado com sucesso!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("disable-hide-iestgroup-data"))
		{
			await _clsDataParam.funcSetAsync("HIDE-IESTGROUP-DATA", string.Empty, pGlobal: true);
			MessageBox.Show(this, "Parâmetro IEST GROUP desativado com sucesso!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("enabledfewbsapi"))
		{
			await _clsDataParam.funcSetAsync("NOT-USE-DFE-WBS-API", string.Empty, pGlobal: true);
			MessageBox.Show(this, "Desabilitado o uso dos WebServices da SEFAZ via API.FISCAL.IO!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("disablefewbsapi"))
		{
			await _clsDataParam.funcSetAsync("NOT-USE-DFE-WBS-API", "ACTIVE", pGlobal: true);
			MessageBox.Show(this, "Habilitado o uso dos WebServices da SEFAZ via API.FISCAL.IO!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("defineasadmin"))
		{
			clsFunction.funcDefineAsAdmin();
			MessageBox.Show(this, "Perfil de Administrador ativado!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("removeasadmin"))
		{
			clsFunction.funcRemoveAsAdmin();
			MessageBox.Show(this, "Perfil de Administrador desativado!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("enablesqltrace"))
		{
			clsFunction.funcEnableSqlTracer();
			await _clsDataParam.funcSetAsync("SQL-TRACE-ACTIVE", "X");
			MessageBox.Show(this, "Trace de SQL ativado! Lembre-se de desativar, informação persistente no banco de dados.", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("disablesqltrace"))
		{
			clsFunction.funcDisableSqlTracer();
			await _clsDataParam.funcSetAsync("SQL-TRACE-ACTIVE", "");
			MessageBox.Show(this, "Trace de SQL desativado!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("defineasdemomode"))
		{
			clsFunction.funcDefineAsDemoMode();
			await _clsDataParam.funcSetAsync("DEMO-MODE", "ACTIVE", pGlobal: true);
			await clsSingleCertificates.Instance.funcGetListAsync(CancellationToken.None, null, pReload: true);
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			MessageBox.Show(this, "Modo de demonstração ativado!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("removeasdemomode"))
		{
			clsFunction.funcRemoveAsDemoMode();
			await _clsDataParam.funcSetAsync("DEMO-MODE", string.Empty, pGlobal: true);
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			MessageBox.Show(this, "Modo de demonstração desativado!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("setlocaldba"))
		{
			new clsSecurity();
			funcSetTaskStatus(new clsTaskStatus("Definindo banco de dados como local..."));
			funcStopTimerTaskRun();
			funcSetTaskStatus(new clsTaskStatus("Definindo banco de dados como local..."));
			await new clsDbaWizard(null).funcSetLocalDbaAsync();
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			await funcStartTimerTaskRunAsync();
			MessageBox.Show(this, "Fiscal.io Monitor conectado no banco de dados local.", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
			return;
		}
		if (varCommand.Equals("dbaconstr"))
		{
			string varDbaConStr = new clsDbaFactory().funcGetDbaCStr();
			_ = string.Empty;
			string varUserMessage;
			if (!clsFunction.IsEmpty(varDbaConStr))
			{
				varUserMessage = "String de conexão copiada na área de transferência!";
				varUserMessage = varUserMessage + Environment.NewLine + Environment.NewLine + varDbaConStr;
				Clipboard.SetText(varDbaConStr);
			}
			else
			{
				varUserMessage = "Nenhuma string de conexão encontrada.";
			}
			MessageBox.Show(this, varUserMessage, "DBA : String de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		if (varCommand.Equals("sqleditor"))
		{
			new frmSqlEditor().Show(this);
			return;
		}
		if (varCommand.Equals("change"))
		{
			using (frmChangeManager varfrmChangeManager = new frmChangeManager())
			{
				varfrmChangeManager.ShowDialog(this);
				return;
			}
		}
		if (varCommand.Equals("dbamanager"))
		{
			using (frmDbaManager varfrmDbaManager = new frmDbaManager(pIsStartup: false))
			{
				varfrmDbaManager.TopMost = false;
				varfrmDbaManager.ShowDialog(this);
			}
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
		}
		else if (varCommand.Equals("access"))
		{
			using (frmAuthManager varfrmAuthManager = new frmAuthManager())
			{
				varfrmAuthManager.TopMost = false;
				varfrmAuthManager.ShowDialog(this);
			}
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
		}
		else if (varCommand.Equals("fixbracellorder"))
		{
			await new FixBracellCommand
			{
				varclsPanelManager = varclsPanelManager
			}.RunAsync();
		}
		else if (varCommand.StartsWith("filldocbinary"))
		{
			await new FillDocBinaryCommand(varCommand.EndsWith("log"))
			{
				varclsPanelManager = varclsPanelManager,
				varclsTabManager = varclsTabManager
			}.RunAsync();
		}
		else if (varCommand.Equals("dbaoptimize"))
		{
			funcSetTaskStatus(new clsTaskStatus("Realizando migração para o SqlServer LocalDba..,"));
			funcStopTimerTaskRun();
			frmLocalMigration varFrmMigration = new frmLocalMigration();
			varFrmMigration.ShowDialog(this);
			bool varIsDbaHasError = false;
			bool varExitLoop = false;
			while (!varExitLoop)
			{
				varIsDbaHasError = varFrmMigration.IsDbaHasError();
				if (varFrmMigration.IsDbaMigrated() || varIsDbaHasError)
				{
					break;
				}
				await Task.Delay(TimeSpan.FromSeconds(5.0));
			}
			varFrmMigration.Dispose();
			await funcStartTimerTaskRunAsync();
			if (varIsDbaHasError)
			{
				funcSetTaskStatus(new clsTaskStatus(pShow: false));
				return;
			}
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			MessageBox.Show(this, "Fiscal.io Monitor conectado no SqlServer LocalDba.", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		else if (varCommand.Equals("dbamigration"))
		{
			funcSetTaskStatus(new clsTaskStatus("Iniciando etapas de migração de dados..."));
			funcStopTimerTaskRun();
			using (frmDatabase varfrmDataBase = new frmDatabase(frmDatabase.enDataType.DbaSelection))
			{
				varfrmDataBase.ShowDialog(this);
			}
			await funcStartTimerTaskRunAsync();
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
		else if (varCommand.Equals("resettask"))
		{
			await new clsDataTaskAction().funcDeletePendentAsync();
			MessageBox.Show(this, "Lista de Tarefas Pendentes reinicializada !!!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (varCommand.Equals("syncbatch"))
		{
			clsDataBatchHead varclsDataBatchHead = new clsDataBatchHead();
			foreach (BatchHead varBatchHead in await new clsDataBatchHead().funcGetListAsync())
			{
				await new clsBatchService().funcDefinePercAsync(varBatchHead.BthId);
				if (clsFunction.funcConvStrToLong(varBatchHead.BthItems) <= 0)
				{
					await varclsDataBatchHead.funcDeleteAsync(varBatchHead);
				}
			}
			MessageBox.Show(this, "Lotes sincronizados com sucesso !!!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (varCommand.Equals("enable-columns-partner"))
		{
			await _clsDataParam.funcSetAsync("SHOW-COLUMNS-PARTNER", "ENABLE", pGlobal: true);
			MessageBox.Show(this, "Parâmetro ativado com sucesso! \nHabilitado no relatório 'NF-e: Dados Sintéticos' colunas adicionais de participantes!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (varCommand.Equals("disable-columns-partner"))
		{
			await _clsDataParam.funcSetAsync("SHOW-COLUMNS-PARTNER", "", pGlobal: true);
			MessageBox.Show(this, "Parâmetro desativado com sucesso! \nDesabilitado no relatório 'NF-e: Dados Sintéticos' colunas adicionais de participantes!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			MessageBox.Show(this, "Comando especial não identificado !!!", "Operação cancelada", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private async Task<clsReturn> funcConsDistDocsAsync(string pSearchTerm)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDFeCodes varclsDFeCodes = new clsDFeCodes();
		new clsTaskStatus();
		string varDocType = string.Empty;
		if (clsFunction.Contains(pSearchTerm, "nfe"))
		{
			varDocType = "nfe";
		}
		else if (clsFunction.Contains(pSearchTerm, "cte"))
		{
			varDocType = "cte";
		}
		else if (clsFunction.Contains(pSearchTerm, "mdfe"))
		{
			varDocType = "mdfe";
		}
		else if (clsFunction.Contains(pSearchTerm, "nfse"))
		{
			varDocType = "nfse";
		}
		if (clsFunction.IsEmpty(varDocType))
		{
			return varclsReturnFunc;
		}
		string[] varRangeList = pSearchTerm.Replace("consdist", "").Replace("nfe", "").Replace("cte", "")
			.Replace("mdfe", "")
			.Replace("nfse", "")
			.Replace("[", "")
			.Replace("]", "")
			.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		string varStarRange = string.Empty;
		string varEndRange = string.Empty;
		for (int varCounter = 0; varCounter < varRangeList.Length; varCounter++)
		{
			if (varCounter == 0)
			{
				varStarRange = varRangeList[varCounter];
			}
			if (varCounter == 1)
			{
				varEndRange = varRangeList[varCounter];
			}
		}
		int IntStartRange = clsFunction.funcConvStrToInt(varStarRange);
		int IntEndRange = clsFunction.funcConvStrToInt(varEndRange);
		FilialView varclsFilial = await funcGetFilialSelectedAsync();
		clsTaskStatus varTaskStatus;
		if (varclsFilial == null)
		{
			varTaskStatus = new clsTaskStatus
			{
				Show = true,
				Progress = false,
				Warning = true,
				Message01 = "Nenhuma empresa selecionada para executar o comando !!!"
			};
			funcSetTaskStatus(varTaskStatus);
			return varclsReturnFunc;
		}
		funcSetTaskStatus(new clsTaskStatus("Consulta " + varDocType.ToUpper() + "(s) e Evento(s) na SEFAZ ..."));
		clsNsuScanFactory varclsFactory = new clsNsuScanFactory();
		intNSUScan varclsHandler = varclsFactory.funcGetClass(varDocType, varclsFilial);
		if (IntStartRange != 0)
		{
			for (int varNSUNumber = IntStartRange; varNSUNumber <= IntEndRange; varNSUNumber++)
			{
				if (varNSUNumber % 10 == 0)
				{
					funcSetTaskStatus(new clsTaskStatus($"Consultando NSU {varNSUNumber} ..."));
				}
				clsReturn varclsRetItem = await varclsHandler.funcExecuteAsync(string.Empty, varNSUNumber.ToString());
				varclsReturnFunc.AddRange(varclsRetItem);
				if (varclsRetItem.HasError || varclsRetItem.HasWarning)
				{
					break;
				}
			}
		}
		else
		{
			varclsReturnFunc = await varclsHandler.funcExecuteAsync(string.Empty);
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		clsDFeObjects varDFeObjects = varclsDFeCodes.funcGetDFeObjects(varclsReturnFunc);
		if (varDFeObjects == null)
		{
			varDFeObjects = new clsDFeObjects();
		}
		varTaskStatus = new clsTaskStatus
		{
			Show = true,
			Progress = false
		};
		varTaskStatus.Return = varclsReturnFunc;
		varTaskStatus.Action01 = funcGetRunTimeMethod("funcLoadFiliaisAsync");
		if (varDFeObjects.TotalObjects > 0)
		{
			varTaskStatus.Message01 = $"{varDFeObjects.TotalObjects} novos documentos identificados na SEFAZ...";
			varTaskStatus.ButtonText = "Ver status da busca";
			varTaskStatus.Event = funcShowStatusScan;
		}
		else if (varclsReturnFunc.HasError)
		{
			varTaskStatus.Error = true;
			varTaskStatus.Message01 = "Ocorreram erros na busca dos documentos na SEFAZ.";
			varTaskStatus.ButtonText = "Ver mais detalhes";
			varTaskStatus.Event = funcShowErrorTask;
		}
		else if (varclsReturnFunc.HasWarning)
		{
			varTaskStatus.Warning = true;
			varTaskStatus.Message01 = "Ocorreram avisos na busca dos documentos na SEFAZ.";
			varTaskStatus.ButtonText = "Ver mais detalhes";
			varTaskStatus.Event = funcShowErrorTask;
		}
		else
		{
			varTaskStatus.Message01 = $"{varDFeObjects.TotalObjects} novos documentos identificados na SEFAZ...";
			varTaskStatus.ButtonText = "Ver status da busca";
			varTaskStatus.Event = funcShowStatusScan;
		}
		if (varDFeObjects.TotalObjects > 0)
		{
			varTaskStatus.Action03 = funcGetRunTimeMethod("funcSoftSyncDataAsync");
		}
		funcSetTaskStatus(varTaskStatus);
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcScanDistDocsAsync(string pSearchTerm)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDFeCodes varclsDFeCodes = new clsDFeCodes();
		new clsTaskStatus();
		string varDocType = string.Empty;
		string varDocModel = string.Empty;
		if (pSearchTerm.Contains("nfe"))
		{
			varDocType = "NFe";
			varDocModel = "55";
		}
		else if (pSearchTerm.Contains("cte"))
		{
			varDocType = "CTe";
			varDocModel = "57";
		}
		else if (pSearchTerm.Contains("nfce"))
		{
			varDocType = "NFCe";
			varDocModel = "65";
		}
		else if (pSearchTerm.Contains("mdfe"))
		{
			varDocType = "MDFe";
			varDocModel = "58";
		}
		else if (pSearchTerm.Contains("cfe"))
		{
			varDocType = "CFe Sat";
			varDocModel = "59";
		}
		else if (pSearchTerm.Contains("nfse"))
		{
			varDocType = "NFSe";
			varDocModel = "95";
		}
		if (clsFunction.IsEmpty(varDocType))
		{
			return varclsReturnFunc;
		}
		FilialView varclsFilial = await funcGetFilialSelectedAsync();
		clsTaskStatus varTaskStatus;
		if (varclsFilial == null)
		{
			varTaskStatus = new clsTaskStatus
			{
				Show = true,
				Progress = false,
				Warning = true,
				Message01 = "Nenhuma empresa selecionada para executar o comando !!!"
			};
			funcSetTaskStatus(varTaskStatus);
			return varclsReturnFunc;
		}
		funcSetTaskStatus(new clsTaskStatus("Buscando " + varDocType.ToUpper() + "(s) e Evento(s) na SEFAZ ..."));
		varclsFilial = await clsMonGeral.funcSetCertificateAsync(this, varclsFilial);
		if (clsFunction.IsEmpty(varclsFilial.Certificado))
		{
			varTaskStatus = new clsTaskStatus
			{
				Show = true,
				Progress = false,
				Warning = true,
				Message01 = "Certificado digital não encontrado !!!"
			};
			funcSetTaskStatus(varTaskStatus);
			return varclsReturnFunc;
		}
		varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varclsFilial, varDocModel, pScanOut: false, pForceScan: true));
		varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varclsFilial, varDocModel, pScanOut: true, pForceScan: true));
		try
		{
			varclsReturnFunc.Values.RemoveAll((clsValue r) => r.ObjDoc != null);
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		clsDFeObjects varDFeObjects = varclsDFeCodes.funcGetDFeObjects(varclsReturnFunc);
		if (varDFeObjects == null)
		{
			varDFeObjects = new clsDFeObjects();
		}
		varTaskStatus = new clsTaskStatus
		{
			Show = true,
			Progress = false
		};
		varTaskStatus.Return = varclsReturnFunc;
		varTaskStatus.Action01 = funcGetRunTimeMethod("funcLoadFiliaisAsync");
		if (varDFeObjects.TotalObjects > 0)
		{
			varTaskStatus.Message01 = $"{varDFeObjects.TotalObjects} novos documentos identificados na SEFAZ...";
			varTaskStatus.ButtonText = "Ver status da busca";
			varTaskStatus.Event = funcShowStatusScan;
		}
		else if (varclsReturnFunc.HasError)
		{
			varTaskStatus.Error = true;
			varTaskStatus.Message01 = "Ocorreram erros na busca dos documentos na SEFAZ.";
			varTaskStatus.ButtonText = "Ver mais detalhes";
			varTaskStatus.Event = funcShowErrorTask;
		}
		else if (varclsReturnFunc.HasWarning)
		{
			varTaskStatus.Warning = true;
			varTaskStatus.Message01 = "Ocorreram avisos na busca dos documentos na SEFAZ.";
			varTaskStatus.ButtonText = "Ver mais detalhes";
			varTaskStatus.Event = funcShowErrorTask;
		}
		else
		{
			varTaskStatus.Message01 = $"{varDFeObjects.TotalObjects} novos documentos identificados na SEFAZ...";
			varTaskStatus.ButtonText = "Ver status da busca";
			varTaskStatus.Event = funcShowStatusScan;
		}
		if (varDFeObjects.TotalObjects > 0)
		{
			varTaskStatus.Action03 = funcGetRunTimeMethod("funcSoftSyncDataAsync");
		}
		funcSetTaskStatus(varTaskStatus);
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcFixNsuDocListAsync()
	{
		clsDataFilial varclsDataFilial = new clsDataFilial();
		clsDataDoc varclsDataDoc = new clsDataDoc();
		clsReturn varclsReturnFunc = new clsReturn();
		new clsTaskStatus();
		if (varclsTabManager == null)
		{
			return varclsReturnFunc;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return varclsReturnFunc;
		}
		List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true);
		clsNsuScanFactory varclsFactory = new clsNsuScanFactory();
		int varTotal = varDocList.Count;
		int varCounter = 0;
		clsTaskStatus varTaskStatus;
		foreach (Document varlsItem in varDocList)
		{
			bool varMustDelete = false;
			varCounter++;
			FilialView varclsFilial = await varclsDataFilial.funcGetItemByKeyAsync(varlsItem.Filial);
			if (varclsFilial == null)
			{
				continue;
			}
			Document varclsDocItem = await varclsDataDoc.funcGetItemByKeyAsync(varlsItem.Filial, varlsItem.Chave);
			if (varclsDocItem == null || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.TomaID) || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.EmitID) || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.DestinID) || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.TranspID) || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.RecebeID) || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.RemeteID) || clsFunction.IsEqual(varclsDocItem.Filial, varclsDocItem.ExpediID) || varclsDocItem.Chave.Contains(varclsDocItem.Filial))
			{
				continue;
			}
			string varStrNSUNumber = clsFunction.funcGetValue(varclsDocItem.NSUGov);
			if (clsFunction.IsEmpty(varStrNSUNumber))
			{
				continue;
			}
			varTaskStatus = new clsTaskStatus($"[ {varCounter} de {varTotal} ] - Consulta DFe {varlsItem.Num}-{varlsItem.Serie} na SEFAZ ...");
			funcSetTaskStatus(varTaskStatus);
			string varDocType = clsFunction.funcGetDocType(varclsDocItem.Model);
			clsReturn varclsRetItem = await varclsFactory.funcGetClass(varDocType, varclsFilial).funcExecuteAsync(string.Empty, varStrNSUNumber);
			if (varclsRetItem.funcHasNumber("138"))
			{
				object varclsObject = varclsRetItem.GetObject("Object");
				if (varclsObject == null)
				{
					varclsObject = new Document();
				}
				if (varclsObject.GetType().Equals(varclsDocItem.GetType()))
				{
					Document varclsObjDoc = (Document)varclsObject;
					if (clsFunction.IsEqual(varclsDocItem.Chave, varclsObjDoc.Chave))
					{
						continue;
					}
					varMustDelete = true;
				}
				else
				{
					varMustDelete = true;
				}
			}
			else if (varclsRetItem.funcHasNumber("137"))
			{
				varMustDelete = true;
			}
			else if (varclsRetItem.funcHasNumber("589"))
			{
				varMustDelete = true;
			}
			else if (varclsRetItem.HasError)
			{
				if (clsSrvGeral.funcIsDummnyFilial(varclsDocItem.Filial))
				{
					varMustDelete = true;
				}
				else
				{
					varclsReturnFunc.AddRange(varclsRetItem.Messages);
				}
			}
			if (varMustDelete && await varclsDataDoc.funcGetTotalByDocKeyAsync(varclsDocItem.Chave) > 1)
			{
				await varclsDataDoc.funcDeleteAsync(varclsDocItem);
			}
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		if (!varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		varTaskStatus = new clsTaskStatus();
		varTaskStatus.Return = varclsReturnFunc;
		varTaskStatus.Show = true;
		varTaskStatus.Progress = false;
		varTaskStatus.Error = true;
		varTaskStatus.Message01 = "Ocorreram erros na busca dos documentos na SEFAZ.";
		varTaskStatus.ButtonText = "Ver detalhe do erro";
		varTaskStatus.Event = funcShowErrorTask;
		varTaskStatus.Action01 = funcGetRunTimeMethod("funcLoadFiliaisAsync");
		funcSetTaskStatus(varTaskStatus);
		return varclsReturnFunc;
	}

	private async void tsbImprimir_Click(object sender, EventArgs e)
	{
		if (!(await funcCheckInitialStartupAsync()) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null || !(await clsScreenGeral.funcHasAccessAsync("DFE-PRINTER-MASS", "EXECUTE")))
		{
			return;
		}
		List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true);
		if (varDocList.Count <= 0)
		{
			return;
		}
		clsPrinterService clsPrinterService = new clsPrinterService();
		clsPrinterService.EventLongRunner += funcEventLongRunner;
		clsReturn varclsReturnFunc = await clsPrinterService.funcSendDocsAsync(varDocList);
		if (varclsReturnFunc.HasError)
		{
			funcSetTaskStatus(varclsReturnFunc);
			return;
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		string varUserMessage = varclsReturnFunc.FirstMessage();
		if (clsFunction.IsEmpty(varUserMessage))
		{
			varUserMessage = "Impressão realizada com sucesso.";
		}
		MessageBox.Show(varUserMessage, "Impressão de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	private async void tsbExportar_Click(object sender, EventArgs e)
	{
		if (!(await funcCheckInitialStartupAsync()) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData != null && await clsScreenGeral.funcHasAccessAsync("DFE-EXPORT-EXCEL", "EXECUTE"))
		{
			await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true, pSelectAll: true, pSyncFromDbaFirst: false);
			clsReturn varclsReturn = await varTabData.funcExportToExcelAsync(delegate(decimal progress)
			{
				Invoke(ExcelPanelAction, progress);
			});
			varclsPanelManager.funcSet(new clsTaskStatus(pShow: false));
			if (varclsReturn.HasError)
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturn);
			}
		}
	}

	private void ChangeExcelPanelPercent(decimal progress)
	{
		clsTaskStatus varTaskStatus = new clsTaskStatus();
		varTaskStatus.Show = true;
		varTaskStatus.Progress = true;
		varTaskStatus.Message01 = "Processando Excel, processamento em " + progress.ToString("F") + "%";
		varclsPanelManager.funcSet(varTaskStatus);
		Application.DoEvents();
	}

	private async Task<bool> funcCheckFiscalServerFunctionsAsync()
	{
		clsWorkProcService varclsWorkProcSrv = new clsWorkProcService();
		new clsDataFilial();
		clsDataConfig varclsDataConfig = new clsDataConfig();
		try
		{
			varLoadingConfig = true;
			Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
			_HasFiscalServer = (_IsFiscalServer = false);
			_HasFiscalServer = await varclsWorkProcSrv.funcHasFiscalServerAsync();
			if (_HasFiscalServer)
			{
				_IsFiscalServer = await varclsWorkProcSrv.funcIsFiscalServerAsync();
			}
			await clsManGeral.funcSetAutoStartupAsync();
			if (cbTimerScan.DataSource == null)
			{
				cbTimerScan.DisplayMember = "Name";
				cbTimerScan.ValueMember = "Value";
				cbTimerScan.DataSource = clsSrvGeral.funcGetTimeList(_HasFiscalServer);
			}
			string varDocScanType = clsFunction.funcGetValue(varclsConfig.DocScanType);
			if (clsFunction.IsEmpty(varDocScanType))
			{
				varDocScanType = "EACH_1HOUR";
			}
			cbTimerScan.SelectedValue = varDocScanType;
			if (_IsFiscalServer)
			{
				pnTimerScan.Visible = true;
			}
			else if (_HasFiscalServer)
			{
				pnTimerScan.Visible = false;
			}
			else
			{
				pnTimerScan.Visible = true;
			}
			if (!pnTimerScan.Visible)
			{
				base.Controls.Remove(contextMenuScan);
			}
			FilialView varclsFilial = await funcGetFilialSelectedAsync();
			if (varclsFilial == null)
			{
				varclsFilial = (await funcGetFilialListAsync()).FirstOrDefault();
			}
			if (varclsFilial == null)
			{
				return true;
			}
			bool varMustScanLocal = await clsManGeral.funcMustScanLocalAsync(_HasFiscalServer, varclsFilial);
			if (_HasFiscalServer && varMustScanLocal)
			{
				ToolStripButton toolStripButton = tsbBuscarDocs;
				bool visible = (tsbBuscarDocsAll.Visible = true);
				toolStripButton.Visible = visible;
				toolStripSeparator16.Visible = true;
			}
			else if (_HasFiscalServer)
			{
				ToolStripButton toolStripButton2 = tsbBuscarDocs;
				bool visible = (tsbBuscarDocsAll.Visible = false);
				toolStripButton2.Visible = visible;
				toolStripSeparator16.Visible = false;
			}
			else
			{
				ToolStripButton toolStripButton3 = tsbBuscarDocs;
				bool visible = (tsbBuscarDocsAll.Visible = true);
				toolStripButton3.Visible = visible;
				toolStripSeparator16.Visible = true;
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		finally
		{
			varLoadingConfig = false;
		}
		return true;
	}

	private async Task<clsReturn> funcScanDocsByUserAction(List<FilialView> pclsFilialList, bool pForceNotify, bool pCheckAutoScan, bool pForceScan)
	{
		new clsDataFilial();
		clsReturn varclsReturnFunc = new clsReturn();
		new clsTaskStatus();
		clsDFeCodes varclsDFeCodes = new clsDFeCodes();
		clsDataConfig varclsDataConfig = new clsDataConfig();
		if (!varObjectsLoaded)
		{
			return varclsReturnFunc;
		}
		try
		{
			if (varIsScanning && clsFunction.IsAdmin)
			{
				if (MessageBox.Show(string.Concat("Já existe uma busca em andamento." + Environment.NewLine + Environment.NewLine, "Deseja continuar mesmo assim?"), "Pergunta", MessageBoxButtons.YesNo).Equals(DialogResult.No))
				{
					return varclsReturnFunc;
				}
			}
			else if (varIsScanning)
			{
				return varclsReturnFunc;
			}
			bool varMustScanLocal = await clsManGeral.funcMustScanLocalAsync(_HasFiscalServer);
			if (_HasFiscalServer && !varMustScanLocal)
			{
				return varclsReturnFunc;
			}
			varclsReturnFunc = await clsSrvGeral.funcIsOnIntervalToScanAsync("");
			if (!varclsReturnFunc.ActionDone)
			{
				funcSetTaskStatus(new clsTaskStatus(varclsReturnFunc)
				{
					Event = funcShowErrorTask
				});
				return varclsReturnFunc;
			}
			if (!(await funcCheckInitialStartupAsync()))
			{
				return varclsReturnFunc;
			}
			List<FilialView> varFilialList = pclsFilialList;
			if (varFilialList == null)
			{
				await new clsDataFilial().funcResetBufferAsync();
				varFilialList = await new clsDataFilial().funcGetListAsync(pLoadDummy: false);
			}
			if (varFilialList == null)
			{
				return varclsReturnFunc;
			}
			if (pCheckAutoScan)
			{
				bool varScanIsEnabled = false;
				foreach (FilialView varItem in varFilialList)
				{
					if (varItem != null && clsFunction.IsEmpty(varItem.DisableAutoScan))
					{
						varScanIsEnabled = true;
						break;
					}
				}
				if (!varScanIsEnabled)
				{
					return varclsReturnFunc;
				}
			}
			long varTotalNsuValues = funcGetTotalNsuValues(varFilialList);
			clsTaskStatus varTaskStatus = new clsTaskStatus
			{
				Show = true,
				Progress = true
			};
			Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
			if (clsFunction.IsEmpty(varclsConfig.ContactId) || clsFunction.IsEmpty(varclsConfig.InstallId))
			{
				varTaskStatus.Message01 = "Preparando ambiente para busca de documentos...";
				funcSetTaskStatus(varTaskStatus);
				clsReturn varRetLincSync = await _clsMetricService.funcSyncAsync(pSyncMarket: false);
				if (varRetLincSync.HasError)
				{
					varclsReturnFunc.AddRange(varRetLincSync);
				}
			}
			varTaskStatus.Show = true;
			varTaskStatus.Progress = true;
			if (varTotalNsuValues == 0L)
			{
				varTaskStatus.Message01 = "Buscando na SEFAZ os documentos emitidos nos últimos 90 dias. [ A primeira BUSCA é mais DEMORADA! ]";
			}
			else
			{
				varTaskStatus.Message01 = "Buscando na SEFAZ os documentos posteriores a última consulta...";
			}
			varTaskStatus.Message02 = "Atenção : Os filtros de data da tela não influênciam esta busca";
			funcSetTaskStatus(varTaskStatus);
			varIsScanning = true;
			foreach (FilialView varItem2 in varFilialList)
			{
				if (varItem2 == null)
				{
					continue;
				}
				FilialView varFilialItem = varItem2;
				if (clsSrvGeral.funcIsDummnyFilial(varFilialItem) || (pCheckAutoScan && !clsFunction.IsEmpty(varFilialItem.DisableAutoScan)))
				{
					continue;
				}
				varMustScanLocal = await clsManGeral.funcMustScanLocalAsync(_HasFiscalServer, varItem2);
				if (_HasFiscalServer && !varMustScanLocal)
				{
					continue;
				}
				varFilialItem = await clsMonGeral.funcSetCertificateAsync(this, varFilialItem, pCheckAutoScan);
				if (clsFunction.IsEmpty(varFilialItem.Certificado))
				{
					continue;
				}
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "55", pScanOut: false, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "55", pScanOut: true, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "65", pScanOut: true, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "57", pScanOut: false, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "57", pScanOut: true, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "58", pScanOut: false, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "95", pScanOut: false, pForceScan));
				varclsReturnFunc.AddRange(await funcScanDocTypeAsync(varFilialItem, "59", pScanOut: true, pForceScan));
				try
				{
					varclsReturnFunc.Values.RemoveAll((clsValue r) => r.ObjDoc != null);
				}
				catch
				{
					if (clsFunction.IsAdmin)
					{
						throw;
					}
				}
			}
			clsDFeObjects varDFeObjects = varclsDFeCodes.funcGetDFeObjects(varclsReturnFunc);
			if (varDFeObjects == null)
			{
				varDFeObjects = new clsDFeObjects();
			}
			await new clsDataFilial().funcResetBufferAsync();
			varTaskStatus = new clsTaskStatus
			{
				Return = varclsReturnFunc
			};
			if (varDFeObjects.TotalObjects > 0)
			{
				await _clsMetricService.funcTrySetAhaMomentAsync("SCAN_DOCS");
			}
			if (varclsReturnFunc.funcHasNumber("656"))
			{
				varTaskStatus.Show = true;
				varTaskStatus.Progress = false;
				varTaskStatus.Warning = true;
				clsMessage varclsMessage = varclsReturnFunc.Messages.FirstOrDefault((clsMessage r) => clsFunction.IsEqual(r.Number, "656"));
				varTaskStatus.Message01 = varclsMessage.Message;
				varTaskStatus.ButtonText = "Ver detalhe dos avisos";
				varTaskStatus.Event = funcShowErrorTask;
			}
			else if (varclsReturnFunc.HasError)
			{
				varTaskStatus.Show = true;
				varTaskStatus.Progress = false;
				varTaskStatus.Error = true;
				varTaskStatus.Message01 = varclsReturnFunc.FirstErrorMessage();
				varTaskStatus.ButtonText = "Ver detalhe dos erros";
				varTaskStatus.Event = funcShowErrorTask;
			}
			else if (varclsReturnFunc.HasWarning && varDFeObjects.TotalObjects <= 0)
			{
				varTaskStatus.Show = true;
				varTaskStatus.Progress = false;
				varTaskStatus.Warning = true;
				if (varclsReturnFunc.Messages.Count == 1)
				{
					varTaskStatus.Message01 = varclsReturnFunc.FirstWarningMessage();
				}
				else
				{
					varTaskStatus.Message01 = "Ocorreram avisos na busca dos documentos na SEFAZ.";
				}
				varTaskStatus.ButtonText = "Ver detalhe dos avisos";
				varTaskStatus.Event = funcShowErrorTask;
			}
			else
			{
				varTaskStatus.Show = true;
				varTaskStatus.Progress = false;
				varTaskStatus.Message01 = $"{varDFeObjects.TotalObjects} novos documentos identificados na SEFAZ...";
				varTaskStatus.ButtonText = "Ver status da busca";
				varTaskStatus.Event = funcShowStatusScan;
				varTaskStatus.Action01 = funcGetRunTimeMethod("funcLoadFiliaisAsync");
			}
			if (pForceNotify || varDFeObjects.TotalObjects > 0)
			{
				funcShowNotifyDocScanStatus(varclsReturnFunc);
			}
			varclsConfig.LastDocScan = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			await new clsDataConfig().funcUpdateAsync(varclsConfig);
			clsDataFilter varclsDataFilter = await funcGetDataFilterAsync();
			trvFeatures = await varclsDataFilter.funcCreateAsync(trvFeatures);
			funcCalcRepTotalAsync(varclsDataFilter, pShowMore: false);
			funcSetTaskStatus(varTaskStatus);
			funcDefineOnBoardScreen(null);
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
			varclsFilialManager.funcRefreshAsync(pForce: true);
		}
		catch (Exception pException)
		{
			clsMessage varclsMessage2 = new clsMessage("E", "999", "Um ou mais erros na busca dos documentos na SEFAZ.", pException);
			varclsReturnFunc.AddMessage(varclsMessage2);
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		finally
		{
			varIsScanning = false;
		}
		return varclsReturnFunc;
	}

	public async Task<clsReturn> funcLoadFiliaisAsync(bool pUseBuffer = true, FilialView pclsFilial = null)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			if (varclsFilialManager == null)
			{
				return varclsReturnFunc;
			}
			Application.UseWaitCursor = true;
			varclsReturnFunc = await varclsFilialManager.funcLoadAsync(pUseBuffer, pclsFilial);
			_LastFilialNode = trvFilial.SelectedNode;
			funcDefineOnBoardScreen(null);
		}
		finally
		{
			Application.UseWaitCursor = false;
		}
		return varclsReturnFunc;
	}

	public async Task<clsReturn> funcScanDocTypeAsync(FilialView pclsFilial, string pModel, bool pScanOut, bool pForceScan)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			Configuration pclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
			string varDocType = clsFunction.funcGetDocType(pModel, pCTeOs: false);
			string varDocInOut = (pScanOut ? "OUT" : "IN");
			if (!clsSrvGeral.funcIsTimeToScan(pclsConfig, pclsFilial, varDocType + varDocInOut) && !pForceScan)
			{
				return varclsReturnFunc;
			}
			intDocScan varclsDocScan = await new clsDocScanFactory().funcGetClassAsync(pModel, pclsFilial, pScanOut);
			if (varclsDocScan == null)
			{
				return varclsReturnFunc;
			}
			varclsDocScan.EventStatusProcess += funcEventStatusProcess;
			varclsReturnFunc = await varclsDocScan.funcExecuteAsync();
		}
		catch (Exception pException)
		{
			string varDocName = clsFunction.funcGetDocName(pModel);
			varclsReturnFunc.AddMessage(new clsMessage("E", "999", "Não foi possivel buscar " + varDocName + " na SEFAZ.", pException));
		}
		return varclsReturnFunc;
	}

	private void funcShowNotifyDocScanStatus(clsReturn pclsReturn)
	{
		clsDFeCodes varclsDFeCodes = new clsDFeCodes();
		if (!base.Visible || !clsFunction.funcIsMainAScreen(base.Handle))
		{
			clsDFeObjects varDFeObjects = varclsDFeCodes.funcGetDFeObjects(pclsReturn);
			if (varDFeObjects == null)
			{
				varDFeObjects = new clsDFeObjects();
			}
			if (!notifyScan.Visible)
			{
				notifyScan.Visible = true;
			}
			notifyScan.BalloonTipTitle = "";
			long varTotalNFeDocs = varDFeObjects.NFeDocInb + varDFeObjects.NFeDocOut;
			long varTotalNFeEvents = varDFeObjects.NFeEventInb + varDFeObjects.NFeEventOut;
			long varTotalCTeDocs = varDFeObjects.CTeDocToma + varDFeObjects.CTeDocOther;
			long varTotalCTeEvents = varDFeObjects.CTeEventInb + varDFeObjects.CTeEventOut;
			long varTotalCFeDocs = varDFeObjects.CFeSatDocInb + varDFeObjects.CFeSatDocOut;
			long varTotalMDFeDocs = varDFeObjects.MDFeDocInb + varDFeObjects.MDFeDocOut;
			long varTotalMDFeEvents = varDFeObjects.MDFeEventInb + varDFeObjects.MDFeEventOut;
			notifyScan.Tag = "DOCSCAN";
			notifyScan.BalloonTipText = "[Fiscal.io]. Resultado da Busca na SEFAZ :" + Environment.NewLine + " " + $"-> {varTotalNFeDocs} novas NFe's;{Environment.NewLine} " + $"-> {varTotalNFeEvents} novos eventos de NFe;{Environment.NewLine} " + $"-> {varTotalCTeDocs} novos CTe's;{Environment.NewLine} " + $"-> {varTotalCTeEvents} novos eventos de CTe;{Environment.NewLine} " + $"-> {varTotalCFeDocs} novos CFe's ;{Environment.NewLine} " + $"-> {varTotalMDFeDocs} novos MDFe's;{Environment.NewLine} " + $"-> {varTotalMDFeEvents} novos eventos de MDFe";
			notifyScan.ShowBalloonTip(150);
		}
	}

	private async void funcEventStatusProcess(object sender, EventStatusProcessEventArgs e)
	{
		try
		{
			clsDFeCodes varclsDFeCodes = new clsDFeCodes();
			clsTaskStatus varTaskStatus = new clsTaskStatus
			{
				Show = true,
				Progress = true,
				Message01 = "Empresa : " + e.ScanFilial.Nome
			};
			if (e.Starting)
			{
				varTaskStatus.Message02 = "Procurando " + e.ScanType + " no ambiente da SEFAZ ...";
				funcSetTaskStatus(varTaskStatus);
				return;
			}
			string varLastCNPJ = string.Empty;
			FilialView varclsFilial = await funcGetFilialSelectedAsync();
			if (varclsFilial != null)
			{
				varLastCNPJ = varclsFilial.CNPJ;
			}
			clsDFeObjects varDFeObjects = varclsDFeCodes.funcGetDFeObjects(e.ScanReturn);
			if (varDFeObjects == null)
			{
				varDFeObjects = new clsDFeObjects();
			}
			clsFunction.funcGetValue(e.ScanType);
			if (!e.DatePercent)
			{
				long varTotalLoaded = e.NSU_LastLoad - e.NSU_FirstOne;
				long varTotalToLoad = e.NSU_MaxInGov - e.NSU_FirstOne;
				if (varTotalLoaded > 0 || varTotalToLoad > 0)
				{
					varTaskStatus.Message02 = $"{e.ScanType}/Eventos carregados da SEFAZ : {varTotalLoaded} de {varTotalToLoad} e o trabalho continua ...";
				}
				else
				{
					if (clsFunction.IsEmpty(e.Message))
					{
						e.Message = "Busca em andamento";
					}
					varTaskStatus.Message02 = e.ScanType + " : " + e.Message + " ...";
				}
			}
			else if (!clsFunction.IsEmpty(e.Message))
			{
				varTaskStatus.Message02 = e.ScanType + " : " + e.Message + " ...";
			}
			else
			{
				double varTotalDays = e.DFe_MaxInGov.Subtract(e.DFe_FirstOne).TotalDays;
				double varPendtDays = e.DFe_MaxInGov.Subtract(e.DFe_LastLoad).TotalDays;
				double varTotalPerc = Math.Round((1.0 - varPendtDays / varTotalDays) * 100.0, 2);
				if (varTotalPerc > 100.0 || varTotalDays == 0.0)
				{
					varTotalPerc = 100.0;
				}
				varTaskStatus.Message02 = e.ScanType;
				if (!clsFunction.IsEmpty(e.CFe_SatSerial))
				{
					varTaskStatus.Message02 = varTaskStatus.Message02 + " : SAT " + e.CFe_SatSerial;
				}
				varTaskStatus.Message02 += $" : Busca em andamento ... [ {varTotalPerc} % ] ";
			}
			varTaskStatus.Event = funcShowStatusScan;
			bool varUpdateScreenNeeded = funcIsNeededUpdateScreen(varDFeObjects.TotalObjects);
			if (varUpdateScreenNeeded && varLastCNPJ.Equals(e.ScanFilial.CNPJ))
			{
				funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
			}
			else if (varUpdateScreenNeeded && varLastCNPJ.Length < 11)
			{
				funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
			}
			funcSetTaskStatus(varTaskStatus);
			funcDefineOnBoardScreen(null);
		}
		catch (Exception ex)
		{
			if (clsFunction.IsAdmin)
			{
				throw ex;
			}
		}
	}

	private MethodInfo funcGetRunTimeMethod(string pMethodName)
	{
		return GetType().GetRuntimeMethods().FirstOrDefault((MethodInfo methodInfo) => string.Equals(methodInfo.Name, pMethodName));
	}

	private bool funcIsNeededUpdateScreen(long pTotalDocs)
	{
		if (pTotalDocs == 0L)
		{
			return false;
		}
		if (pTotalDocs > 500)
		{
			return false;
		}
		if (varclsTabManager == null)
		{
			return false;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return false;
		}
		int varTotalDocs = varTabData.funcGetTotalDocs();
		if (varTotalDocs == 0)
		{
			return true;
		}
		if (varTotalDocs < 500)
		{
			return true;
		}
		return false;
	}

	private async void tsbSendDoc_Click(object sender, EventArgs e)
	{
		if (!(await funcCheckInitialStartupAsync()) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null || !(await clsScreenGeral.funcHasAccessAsync("EMAIL-EXECUTE", "EXECUTE")))
		{
			return;
		}
		if (varTabData.TabName == "TabAuditor")
		{
			clsReturn clsReturn = (varTabData as clsTabAuditor).funcSendDocs(null);
			List<DocAuditGroup> varSelectedItems = clsReturn.GetObject("varAuditGroup") as List<DocAuditGroup>;
			if (clsReturn != null && varSelectedItems.Count > 0)
			{
				frmExcelErrorSend obj = new frmExcelErrorSend(varSelectedItems, null, pEventLoad: true);
				obj.ShowDialog(this);
				obj.Dispose();
			}
		}
		else
		{
			List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true);
			if (varDocList.Count > 0)
			{
				frmDocSend obj2 = new frmDocSend(varDocList, null, pEventLoad: true);
				obj2.ShowDialog(this);
				obj2.Dispose();
			}
		}
	}

	public async Task<List<Document>> funcGetDocListAsync(intDocTabData pTabData, bool pFocused, bool pChecked, bool pSelectAll = true, bool pSyncFromDbaFirst = true)
	{
		List<Document> varDocList = new List<Document>();
		if (pTabData == null)
		{
			return varDocList;
		}
		varDocList = await pTabData.funcGetDocListAsync(pFocused, pChecked, pSyncFromDbaFirst);
		if (varDocList.Count == 0 && pSelectAll)
		{
			varDocList = await pTabData.funcGetDocListAsync(pFocused: false, pChecked: false, pSyncFromDbaFirst);
		}
		string varFeatExtId = clsFeatureService.consFeatScanDocNFSeIn;
		string pValue = await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId, pIfNotFoundRetLock: false);
		bool varShowNFSeMsg = false;
		bool varShowCFeSATMsg = false;
		if (clsFunction.Contains(pValue, "LOCK"))
		{
			varDocList.Count((Document r) => clsFunction.funcIsNFSe(r.Model));
			if (varDocList.FirstOrDefault((Document r) => clsFunction.funcIsNFSe(r.Model) && clsFunction.IsEqual(r.DFeSource, "SEFAZ", pIgnoreCase: true)) != null)
			{
				varShowNFSeMsg = true;
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
				varDocList = varDocList.Where((Document r) => !clsFunction.funcIsNFSe(r.Model) || !clsFunction.IsEqual(r.DFeSource, "SEFAZ", pIgnoreCase: true)).ToList();
			}
		}
		varFeatExtId = clsFeatureService.consFeatScanDocCFeOut;
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId, pIfNotFoundRetLock: false), "LOCK"))
		{
			varDocList.Count((Document r) => clsFunction.funcIsCFeSat(r.Model));
			if (varDocList.FirstOrDefault((Document r) => clsFunction.funcIsCFeSat(r.Model) && clsFunction.IsEqual(r.DFeSource, "SEFAZ", pIgnoreCase: true)) != null)
			{
				varShowCFeSATMsg = true;
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
				varDocList = varDocList.Where((Document r) => !clsFunction.funcIsCFeSat(r.Model) || !clsFunction.IsEqual(r.DFeSource, "SEFAZ", pIgnoreCase: true)).ToList();
			}
		}
		if (varDocList.Count == 0)
		{
			return varDocList;
		}
		if (varShowNFSeMsg && varShowCFeSATMsg)
		{
			MessageBox.Show("NFSe e CFeSAT estão bloqueados para a ação selecionada. Contrate as funcionalidades de NFSe e CFeSAT para liberar todos os recursos.", "Funcionalidade bloqueada!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (varShowNFSeMsg)
		{
			MessageBox.Show("Os documentos de NFSe estão bloqueados para a ação selecionada. Contrate a funcionalidade de NFSe para liberar todos os recursos.", "Funcionalidade bloqueada!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (varShowCFeSATMsg)
		{
			MessageBox.Show("Os documentos de CFeSAT estão bloqueados para a ação selecionada. Contrate a funcionalidade de CFeSAT para liberar todos os recursos.", "Funcionalidade bloqueada!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		return varDocList;
	}

	private async void tsbBuscarDocs_Click(object sender, EventArgs e)
	{
		if (await funcCheckInitialStartupAsync())
		{
			funcScanDocsByUserAction(await funcGetFilialListAsync(), pForceNotify: true, pCheckAutoScan: false, pForceScan: true);
		}
	}

	private void tsbBuscarDocsAll_Click(object sender, EventArgs e)
	{
		funcScanDocsByUserAction(null, pForceNotify: true, pCheckAutoScan: false, pForceScan: true);
	}

	private async void timerTaskRunUser_Elapsed(object sender, ElapsedEventArgs e)
	{
		_ = 3;
		try
		{
			if (varObjectsLoaded && !backTaskRunUser.IsBusy)
			{
				await varclsFilialManager.funcRefreshAsync(pForce: false);
				if (clsFunction.IsEqual(clsFunction.IsToUpdateScreen, "ALL"))
				{
					await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: false);
				}
				else if (clsFunction.IsEqual(clsFunction.IsToUpdateScreen, "BALANCE"))
				{
					await funcShowSalesDataAsync();
				}
				else if (clsFunction.IsEqual(clsFunction.IsToUpdateScreen, "BACKUP"))
				{
					await funcShowBackupDataAsync();
				}
				timerTaskRunUser.Enabled = false;
				timerTaskRunUser.Stop();
				backTaskRunUser.RunWorkerAsync();
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private async void backTaskRunUser_DoWork(object sender, DoWorkEventArgs e)
	{
		_ = 1;
		try
		{
			if (!(await new clsWorkProcService().funcHasFiscalLocalAsync()))
			{
				await new clsTaskManager(varclsTracer).funcExecuteByLocalAsync(clsDataTaskAction.enProcType.ProcUser);
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		finally
		{
			timerTaskRunUser.Enabled = true;
			timerTaskRunUser.Start();
		}
	}

	private void backTaskRunUser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		timerTaskRunUser.Enabled = true;
		timerTaskRunUser.Start();
	}

	private void timerTaskRunSyst_Elapsed(object sender, ElapsedEventArgs e)
	{
		try
		{
			if (varObjectsLoaded && !backTaskRunSyst.IsBusy)
			{
				timerTaskRunSyst.Enabled = false;
				timerTaskRunSyst.Stop();
				backTaskRunSyst.RunWorkerAsync();
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private async void backTaskRunSyst_DoWork(object sender, DoWorkEventArgs e)
	{
		_ = 1;
		try
		{
			if (!(await new clsWorkProcService().funcHasFiscalLocalAsync()))
			{
				await new clsTaskManager(varclsTracer).funcExecuteByLocalAsync(clsDataTaskAction.enProcType.ProcSyst);
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		finally
		{
			timerTaskRunSyst.Enabled = true;
			timerTaskRunSyst.Start();
		}
	}

	private void backTaskRunSyst_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		timerTaskRunSyst.Enabled = true;
		timerTaskRunSyst.Start();
	}

	private void frmMonitor_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			clsReturn result = Task.Run(async () => await clsManGeral.funcSetAutoStartupAsync()).Result;
			Task.Run(async delegate
			{
				await varclsUserUsageService.funcRegisterAsync(pFinish: true);
			});
			string varNotAutoStartup = result.GetValue("NotAutoStartup");
			varWindowState = base.WindowState;
			if (e.CloseReason != CloseReason.UserClosing)
			{
				notifyScan.Visible = false;
				notifyScan.Dispose();
				timerCheckScan.Enabled = false;
				return;
			}
			funcSaveTabsColumnsData();
			if (clsFunction.IsEqual(varNotAutoStartup, "", "N") && !varStartupError && varDbaFactory.funcIsLocalDba())
			{
				notifyScan.Visible = true;
				base.Visible = false;
				timerCheckScan.Enabled = true;
				e.Cancel = true;
				varFormIsIdle = clsFunction.funcSetProcessPriority(ProcessPriorityClass.Idle);
			}
			else
			{
				notifyScan.Visible = false;
				notifyScan.Dispose();
				timerCheckScan.Enabled = false;
			}
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void funcSaveTabsColumnsData()
	{
		foreach (TabPage varPageItem in tabContent.TabPages)
		{
			try
			{
				if (varPageItem.Controls.Count <= 0)
				{
					continue;
				}
				Control varFormItem = varPageItem.Controls[0];
				if (varFormItem.Controls.Count > 0)
				{
					MethodInfo varMethod = varFormItem.Controls[0].GetType().GetMethod("funcSaveColumns");
					if (varMethod != null)
					{
						varMethod.Invoke(varFormItem.Controls[0], new object[0]);
					}
				}
			}
			catch
			{
				if (clsFunction.IsAdmin)
				{
					throw;
				}
			}
		}
	}

	private void notifyScan_BalloonTipClicked(object sender, EventArgs e)
	{
		bool varShowCompany = false;
		if (((notifyScan.Tag == null) ? string.Empty : notifyScan.Tag).Equals("CADEMP"))
		{
			varShowCompany = true;
		}
		funcShowScreen(varShowCompany);
	}

	private async void funcShowScreen(bool pShowCompany = false)
	{
		varStartInHideMode = false;
		bool varOldTopMost = base.TopMost;
		notifyScan.Visible = false;
		if (base.WindowState.Equals(FormWindowState.Minimized))
		{
			base.WindowState = varWindowState;
		}
		base.ShowInTaskbar = true;
		base.TopMost = true;
		base.Visible = true;
		Show();
		Activate();
		base.TopMost = varOldTopMost;
		await varclsUserUsageService.funcRegisterAsync(pFinish: false);
		if (pShowCompany)
		{
			frmFilial frmFilial = new frmFilial(await funcGetFilialSelectedAsync());
			frmFilial.TopMost = true;
			frmFilial.ShowDialog(this);
			frmFilial.Dispose();
		}
		await funcLoadFiliaisAsync();
		varFormIsIdle = !clsFunction.funcSetProcessPriority(await _clsDataParam.funcGetAsync("ClientThreadPrior"), ProcessPriorityClass.High);
	}

	private void notifyScan_Click(object sender, EventArgs e)
	{
		if (((MouseEventArgs)e).Button.Equals(MouseButtons.Left))
		{
			funcShowScreen();
		}
	}

	private void notifyScan_DoubleClick(object sender, EventArgs e)
	{
		funcShowScreen();
	}

	private void tsmShowMonitor_Click(object sender, EventArgs e)
	{
		funcShowScreen();
	}

	private async void tsmScanTimer_Click(object sender, EventArgs e)
	{
		ToolStripMenuItem varScanTimer = (ToolStripMenuItem)sender;
		if (varScanTimer != null)
		{
			string varDocScanType = (string)varScanTimer.Tag;
			await funcSetTimeScanMenuAsync("tsmScanTimer", varDocScanType);
		}
	}

	private async Task<bool> funcSetTimeScanMenuAsync(string pSource, string pDocScanType)
	{
		string varDocScanType = clsFunction.funcGetValue(pDocScanType);
		if (clsFunction.IsEmpty(varDocScanType))
		{
			varDocScanType = "EACH_1HOUR";
		}
		else if (_HasFiscalServer && clsFunction.IsEqual(varDocScanType, "SCAN_DISAB"))
		{
			varDocScanType = "EACH_1HOUR";
		}
		if (!clsFunction.IsEqual(pSource, "cbTimerScan"))
		{
			cbTimerScan.SelectedValue = varDocScanType;
		}
		ToolStripMenuItem toolStripMenuItem = tsmScanEach1Hour;
		Image image = (tsmScanEach2Hour.Image = null);
		toolStripMenuItem.Image = image;
		ToolStripMenuItem toolStripMenuItem2 = tsmScanEach3Hour;
		ToolStripMenuItem toolStripMenuItem3 = tsmScanEach6Hour;
		Image image3 = (tsmScanManually.Image = null);
		image = (toolStripMenuItem3.Image = image3);
		toolStripMenuItem2.Image = image;
		if (clsFunction.IsEqual(varDocScanType, "EACH_1HOUR"))
		{
			tsmScanEach1Hour.Image = Resources.image_ok;
		}
		else if (clsFunction.IsEqual(varDocScanType, "EACH_2HOUR"))
		{
			tsmScanEach2Hour.Image = Resources.image_ok;
		}
		else if (clsFunction.IsEqual(varDocScanType, "EACH_3HOUR"))
		{
			tsmScanEach3Hour.Image = Resources.image_ok;
		}
		else if (clsFunction.IsEqual(varDocScanType, "EACH_6HOUR"))
		{
			tsmScanEach6Hour.Image = Resources.image_ok;
		}
		else if (clsFunction.IsEqual(varDocScanType, "SCAN_DISAB"))
		{
			tsmScanManually.Image = Resources.image_ok;
		}
		else
		{
			tsmScanEach1Hour.Image = Resources.image_ok;
		}
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		varclsConfig.DocScanType = varDocScanType;
		await new clsDataConfig().funcUpdateAsync(varclsConfig);
		return true;
	}

	private async void tsmShowConfig_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("CONFIG-MANAGER"))
		{
			frmConfig frmConfig = new frmConfig();
			frmConfig.ShowDialog(this);
			frmConfig.Dispose();
			await funcSyncUpdateScreenAsync(pReloadFilter: false, pReloadData: false);
			await funcStartTimerTaskRunAsync();
		}
	}

	private async void tsmShowChannelManager_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("CHANNEL-MANAGER"))
		{
			frmChannelManager frmChannelManager = new frmChannelManager(string.Empty);
			frmChannelManager.ShowDialog(this);
			frmChannelManager.Dispose();
			await funcLoadConsFiscalAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
		}
	}

	private async void tsmShowTaskManager_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("TASK-MANAGER", "VIEW"))
		{
			frmTaskManager frmTaskManager = new frmTaskManager();
			frmTaskManager.ShowDialog(this);
			frmTaskManager.Dispose();
			await funcLoadConsFiscalAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
		}
	}

	private void tsmSair_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private async void timerCheckScan_Elapsed(object sender, ElapsedEventArgs e)
	{
		clsReturn varclsRetFunc = new clsReturn();
		try
		{
			if (!varObjectsLoaded)
			{
				return;
			}
			Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
			if (clsFunction.IsEqual(varclsConfig.DocScanType, "SCAN_DISAB") || !clsSrvGeral.funcIsTimeToScan(varclsConfig))
			{
				return;
			}
			timerCheckScan.Enabled = false;
			timerCheckScan.Stop();
			varclsRetFunc = await funcScanDocsByNotificationAsync(pCheckAutoScan: true);
		}
		catch (Exception pException)
		{
			varclsRetFunc.AddException(pException);
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		finally
		{
			timerCheckScan.Enabled = true;
			timerCheckScan.Start();
		}
		if (clsFunction.IsAdmin && varclsRetFunc.HasError)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsRetFunc);
		}
	}

	private async Task<clsReturn> funcScanDocsByNotificationAsync(bool pCheckAutoScan)
	{
		clsReturn varclsReturn = new clsReturn();
		try
		{
			if (!(await new clsDataFilial().funcGetListAsync(pLoadDummy: false)).Any())
			{
				notifyScan.BalloonTipText = "[Fiscal.io]. Cadastre alguma EMPRESA para" + Environment.NewLine + "iniciarmos a buscar dos documentos na SEFAZ" + Environment.NewLine + "Você pode utilizar um Certificado Digital A1 ou A3";
				notifyScan.ShowBalloonTip(150);
				notifyScan.Tag = "CADEMP";
			}
			else
			{
				varclsReturn = await funcScanDocsByUserAction(null, pForceNotify: false, pCheckAutoScan, pForceScan: false);
			}
			Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
			varclsConfig.LastDocScan = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			await new clsDataConfig().funcUpdateAsync(varclsConfig);
			notifyScan.Text = "Fiscal.io Monitor";
		}
		catch (Exception pException)
		{
			varclsReturn.AddException(pException);
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		return varclsReturn;
	}

	private async void funcShowDocViewerAsync(bool pShowPDF, bool pShowXML, bool pShowEDI)
	{
		if (await funcCheckInitialStartupAsync() && varclsTabManager != null)
		{
			intDocTabData varTabData = varclsTabManager.Selected();
			if (varTabData != null)
			{
				await clsMonGeral.funcDocViewerAsync(this, await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true), pShowPDF, pShowXML, pShowEDI);
			}
		}
	}

	private async void tsbEventoCTeDesacordo_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "CTe", "CTeOs" };
		string varEventType = new clsDFeCodes().GetCTeDisagree();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventoCTeDesacordoCanc_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "CTe", "CTeOs" };
		string varEventType = new clsDFeCodes().GetCTeDisagreeCanc();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventosNFeCiencia_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe" };
		string varEventType = new clsDFeCodes().GetNFeAcknow();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventosNFeConfirmacao_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe" };
		string varEventType = new clsDFeCodes().GetNFeConfirm();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventosNFeNaoRealizada_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe" };
		string varEventType = new clsDFeCodes().GetNFeDisagree();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventosNFeDesconhecida_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe" };
		string varEventType = new clsDFeCodes().GetNFeUnknow();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventosNFSeConfirmacao_Click(object sender, EventArgs e)
	{
		string varFeatureIdCode = clsFeatureService.consFeatScanDocNFSeIn;
		await _clsDataParam.funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatureIdCode), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatureIdCode);
			return;
		}
		List<string> varDocTypeList = new List<string> { "NFSe" };
		string varEventType = new clsDFeCodes().GetNFSeConfirm();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private async void tsbEventosNFSeRecusa_Click(object sender, EventArgs e)
	{
		string varFeatureIdCode = clsFeatureService.consFeatScanDocNFSeIn;
		await _clsDataParam.funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatureIdCode), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatureIdCode);
			return;
		}
		List<string> varDocTypeList = new List<string> { "NFSe" };
		string varEventType = new clsDFeCodes().GetNFSeDisagree();
		await funcEventConfirmAsync(varDocTypeList, varEventType, pClick: true);
	}

	private string funcGetFeatExId(string pFeatExtId)
	{
		string varFeatExtId = clsFunction.funcGetValue(pFeatExtId);
		if (clsFunction.IsEmpty(pFeatExtId))
		{
			return string.Empty;
		}
		varFeatExtId = varFeatExtId.Replace("-CKB", "");
		varFeatExtId = varFeatExtId.Replace("-LOCKED", "");
		varFeatExtId = varFeatExtId.Replace("-PAYED", "");
		varFeatExtId = varFeatExtId.Replace("-FREE", "");
		return varFeatExtId.Replace("-TRIAL", "");
	}

	private async Task<bool> funcFeatureSelectedIsLockedAsync(TreeNode pNode)
	{
		if (pNode == null)
		{
			return false;
		}
		string varNodeName = clsFunction.funcGetValue(pNode.Name);
		if (clsFunction.IsEmpty(varNodeName))
		{
			return false;
		}
		string varNodeTag = clsFunction.funcGetValue(pNode.Tag);
		if (clsFunction.IsEmpty(varNodeTag))
		{
			return false;
		}
		string varObjFeature = varNodeName;
		if (clsFunction.Contains(varNodeTag, "-FUNC-"))
		{
			varObjFeature = varNodeTag;
		}
		string varFeatExtId = funcGetFeatExId(varObjFeature);
		if (clsFunction.IsEmpty(varFeatExtId))
		{
			return false;
		}
		bool varIsLocked = false;
		if (clsFunction.Contains(varNodeName, "LOCK"))
		{
			varIsLocked = true;
		}
		else if (clsFunction.Contains(varNodeTag, "LOCK"))
		{
			varIsLocked = true;
		}
		if (!varIsLocked)
		{
			return false;
		}
		Color varForeColor = pNode.ForeColor;
		int varImageIcon = pNode.ImageIndex;
		pNode.ForeColor = SystemColors.Highlight;
		int imageIndex = (pNode.SelectedImageIndex = clsFunction.icon_locker);
		pNode.ImageIndex = imageIndex;
		pNode.NodeFont = new Font(pNode.TreeView.Font.FontFamily, pNode.TreeView.Font.Size, FontStyle.Italic);
		await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
		pNode.ForeColor = varForeColor;
		pNode.NodeFont = new Font(pNode.TreeView.Font.FontFamily, pNode.TreeView.Font.Size, FontStyle.Regular);
		imageIndex = (pNode.SelectedImageIndex = varImageIcon);
		pNode.ImageIndex = imageIndex;
		return true;
	}

	private void tsbSearchHelp_Click(object sender, EventArgs e)
	{
		frmHelpSearchTerm obj = new frmHelpSearchTerm();
		obj.ShowDialog(this);
		obj.Dispose();
	}

	private async void tsbSupport_Click(object sender, EventArgs e)
	{
		if (await funcCheckInitialStartupAsync())
		{
			string varFeatExtId = clsFeatureService.consSupportPriority;
			string obj = await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId);
			new Form();
			Form varfrmHelpSupport = ((!obj.Contains("LOCK")) ? ((Form)new frmHelpSupportPayed()) : ((Form)new frmHelpSupportFree()));
			await _clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
			varfrmHelpSupport.ShowDialog(this);
			varfrmHelpSupport.Dispose();
		}
	}

	private async void funcCalcRepTotalAsync(clsDataFilter pclsDataFilter, bool pShowMore)
	{
		_ = 2;
		try
		{
			if (pclsDataFilter == null)
			{
				return;
			}
			List<clsObjData> varResultList = new List<clsObjData>();
			if (clsFunction.IsEmpty(await _clsDataParam.funcGetAsync("ShowReportSum")))
			{
				return;
			}
			new List<string>();
			int varNodeIndex = trvFeatures.Nodes.IndexOfKey("RootReports");
			if (varNodeIndex < 0)
			{
				return;
			}
			TreeNode varRootNode = trvFeatures.Nodes[varNodeIndex];
			if (varRootNode == null)
			{
				return;
			}
			foreach (TreeNode varTreeNode in varRootNode.Nodes)
			{
				string varFeatType = await varclsFeatService.funcGetFeatTypeAsync(varTreeNode.Name);
				clsObjData varObjData = await pclsDataFilter.funcGetDataAsync(varTreeNode.Name, varFeatType);
				if (varObjData != null && !clsFunction.IsEmpty(varObjData.ObjectId))
				{
					varResultList.Add(varObjData);
				}
			}
			trvFeatures.SuspendLayout();
			trvFeatures.BeginUpdate();
			bool varHasTotalToShow = false;
			foreach (TreeNode varTreeNode2 in varRootNode.Nodes)
			{
				clsObjData varObjData2 = varResultList.FirstOrDefault((clsObjData r) => r.ObjectId.Equals(varTreeNode2.Name));
				if (varObjData2 != null)
				{
					varTreeNode2.Nodes.Clear();
					Color varObjColor = varclsFeatService.funcGetTotalColor(varTreeNode2.Name);
					TreeNode varReportNodeSub01 = clsFunction.funcGetDefaultNode(varTreeNode2.Name, "", varTreeNode2.Tag);
					varReportNodeSub01.Text = varObjData2.ValueStr;
					varReportNodeSub01.ForeColor = varObjColor;
					varTreeNode2.Nodes.Add(varReportNodeSub01);
					TreeNode varReportNodeSub2 = clsFunction.funcGetDefaultNode(varTreeNode2.Name, "", varTreeNode2.Tag);
					varReportNodeSub2.Text = varObjData2.QuantStr;
					varReportNodeSub2.ForeColor = varObjColor;
					varTreeNode2.Nodes.Add(varReportNodeSub2);
					if (varObjData2.QuantNum > 0)
					{
						varTreeNode2.Expand();
					}
					else
					{
						varTreeNode2.Collapse();
					}
					if (varObjData2.QuantNum > 1)
					{
						varHasTotalToShow = true;
					}
				}
			}
			trvFeatures.EndUpdate();
			trvFeatures.ResumeLayout();
			if (trvFeatures.SelectedNode == null && trvFeatures.Nodes.Count > 0)
			{
				trvFeatures.SelectedNode = trvFeatures.Nodes[0];
			}
			if (trvFeatures.SelectedNode != null)
			{
				trvFeatures.SelectedNode.EnsureVisible();
			}
			if (pShowMore && varHasTotalToShow)
			{
				clsTaskStatus varclsTaskStatus = new clsTaskStatus();
				varclsTaskStatus.Show = true;
				varclsTaskStatus.Reload = true;
				varclsTaskStatus.FormName = enPanelName.PanelMoreReports;
				varclsTaskStatus.TotalList = varResultList;
				funcSetTaskStatus(varclsTaskStatus);
			}
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		finally
		{
			trvFeatures.EndUpdate();
			trvFeatures.ResumeLayout();
		}
	}

	private async Task<clsReturn> funcSyncAuditNodeDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			if (pclsDataFilter == null)
			{
				return varclsReturnFunc;
			}
			if ((await new clsDbaFactory().funcGetClassAsync()).funcGetDbaType().Equals("SQLLITE"))
			{
				return null;
			}
			new List<string>();
			int varNodeIndex = trvFeatures.Nodes.IndexOfKey("RootFilterByAudit");
			if (varNodeIndex < 0)
			{
				return varclsReturnFunc;
			}
			TreeNode varRootNode = trvFeatures.Nodes[varNodeIndex];
			if (varRootNode == null)
			{
				return varclsReturnFunc;
			}
			trvFeatures.SuspendLayout();
			trvFeatures.BeginUpdate();
			TreeNode varNewNode = await pclsDataFilter.funcAddFilterByAuditAsync(varRootNode);
			trvFeatures.Nodes.Remove(varRootNode);
			trvFeatures.Nodes.Insert(varNodeIndex, varNewNode);
			trvFeatures.EndUpdate();
			trvFeatures.ResumeLayout();
		}
		catch (Exception pException)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			trvFeatures.EndUpdate();
			trvFeatures.ResumeLayout();
		}
		return varclsReturnFunc;
	}

	private async void tsbSyncronize_Click(object sender, EventArgs e)
	{
		await funcLicenseSyncFiscalAsync();
	}

	private async void tsmOrderByDesct_Click(object sender, EventArgs e)
	{
		if (tsmOrderByDesct.Checked)
		{
			await _clsDataParam.funcSetAsync("FILIAL_ORDERBY_DESCT", "");
			tsmOrderByDesct.Checked = false;
		}
		else
		{
			await _clsDataParam.funcSetAsync("FILIAL_ORDERBY_DESCT", "X");
			tsmOrderByDesct.Checked = true;
		}
		await funcLoadFiliaisAsync(pUseBuffer: false);
	}

	private async void tsmOrderByIdent_Click(object sender, EventArgs e)
	{
		if (tsmOrderByIdent.Checked)
		{
			await _clsDataParam.funcSetAsync("FILIAL_ORDERBY_IDENT", "");
			tsmOrderByIdent.Checked = false;
		}
		else
		{
			await _clsDataParam.funcSetAsync("FILIAL_ORDERBY_IDENT", "X");
			tsmOrderByIdent.Checked = true;
		}
		await funcLoadFiliaisAsync(pUseBuffer: false);
	}

	private void tsbSortFilial_Click(object sender, EventArgs e)
	{
		contextSortFilial.Show(Cursor.Position);
	}

	private async void timerBackupAdv_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (!base.Visible || !varObjectsLoaded || _HasFiscalServer)
		{
			return;
		}
		try
		{
			if ((await new clsDbaFactory().funcGetClassAsync()).funcIsLocalDba())
			{
				PictureBox pictureBox = picBackupSet;
				bool visible = (lkbBackupSet.Visible = false);
				pictureBox.Visible = visible;
				lbBackupSize.Visible = false;
				lbkBckDate.Visible = false;
				await Task.Delay(1000);
				PictureBox pictureBox2 = picBackupSet;
				visible = (lkbBackupSet.Visible = true);
				pictureBox2.Visible = visible;
				lbBackupSize.Visible = true;
				lbkBckDate.Visible = true;
				await funcShowBackupDataAsync();
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void lkbBackup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		funcLoadBackupScreenAsync();
	}

	private void lbBackup_Click(object sender, EventArgs e)
	{
		funcLoadBackupScreenAsync();
	}

	private void lbkBckDate_Click(object sender, EventArgs e)
	{
		funcLoadBackupScreenAsync();
	}

	private async void funcLoadBackupScreenAsync()
	{
		string varFeatureIdCode = clsFeatureService.consFeatBackupOfData;
		await _clsDataParam.funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatureIdCode), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatureIdCode);
		}
		else if (await clsScreenGeral.funcHasAccessAsync("BACKUP-MANAGER"))
		{
			frmBackup frmBackup = new frmBackup();
			frmBackup.ShowDialog(this);
			frmBackup.Dispose();
			await funcStartTimerTaskRunAsync();
			await funcShowBackupDataAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: false);
		}
	}

	private void tsbViewEdiAll_Click(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: false, pShowXML: false, pShowEDI: true);
	}

	private async void cbTimerScan_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!varLoadingConfig)
		{
			string varSelectedValue = string.Empty;
			try
			{
				varSelectedValue = (string)cbTimerScan.SelectedValue;
			}
			catch
			{
			}
			if (!clsFunction.IsEmpty(varSelectedValue))
			{
				await funcSetTimeScanMenuAsync("cbTimerScan", varSelectedValue);
			}
		}
	}

	private async void btBuscarDocsAll_Click(object sender, EventArgs e)
	{
		if (await funcCheckInitialStartupAsync())
		{
			cbTimerScan.SelectedValue = "EACH_1HOUR";
			funcScanDocsByUserAction(null, pForceNotify: true, pCheckAutoScan: false, pForceScan: true);
			funcDefineOnBoardScreen(null);
		}
	}

	private async void btSrvDisagree_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "CTe", "CTeOs" };
		string varEventType = new clsDFeCodes().GetCTeDisagree();
		await funcEventConfirmAsync(varDocTypeList, varEventType);
		funcDefineOnBoardScreen(null);
		await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: false, pResetTabs: true);
	}

	private void funcEventPanelManager(object sender, EventPanelManagerEventArgs e)
	{
		funcOpenEventManager(e.UserAction, e.ObjectItem);
	}

	private void funcEventStartupSrv(object sender, EventStartupSrvEventArgs e)
	{
		Application.DoEvents();
		if (e.ShowStatus)
		{
			if (e.ReturnFunc != null)
			{
				funcSetTaskStatus(e.ReturnFunc);
			}
			else if (!clsFunction.IsEmpty(e.RetMessage))
			{
				funcSetTaskStatus(new clsTaskStatus(e.RetMessage));
			}
			else
			{
				funcSetTaskStatus(new clsTaskStatus(pShow: false));
			}
			Application.DoEvents();
		}
	}

	private void funcEventTabManager(object sender, EventTabManagerEventArgs e)
	{
		if (!clsFunction.IsEmpty(e.UserAction))
		{
			funcOpenEventManager(e.UserAction);
		}
		else if (!clsFunction.IsEmpty(e.TaskProgress))
		{
			funcSetTaskStatus(new clsTaskStatus(e.TaskProgress));
		}
		else if (e.TotalValue.HasValue || e.TotalQuant.HasValue)
		{
			stsTotalQuant.Text = $"Documentos : {e.TotalQuant}";
			stsTotalValue.Text = "Valor Total : " + $"{e.TotalValue:C}";
		}
		else
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
	}

	private void funcOpenEventManager(string pUserAction, object pObjectItem = null)
	{
		if (!clsFunction.IsEmpty(pUserAction))
		{
			if (pUserAction.Equals("CONFIG_DATABASE"))
			{
				funcOpenDbaWizardAsync();
			}
			else if (pUserAction.Equals("CONFIG_BACKUP"))
			{
				funcLoadBackupScreenAsync();
			}
			else if (pUserAction.Contains("CONFIG_CHANNEL"))
			{
				funcOpenChannelManager(pUserAction);
			}
			else if (pUserAction.Equals("CONFIG_SERVER"))
			{
				fncOpenFiscalServerAsync();
			}
			else if (pUserAction.Contains("Tab"))
			{
				fncOpenTabReport(pUserAction);
			}
			else if (pUserAction.Contains("OPEN_BATCHSTATUS"))
			{
				fncOpenBatchViewStatus();
			}
			else if (pUserAction.Contains("CONFIG_EXTSYST"))
			{
				funcOpenExtSystManagerAsync();
			}
			else if (pUserAction.Contains("OPEN_TABDOCJUMP"))
			{
				funcOpenTabPage("TabDocJump");
			}
		}
	}

	private void fncOpenBatchViewStatus()
	{
		funcOpenTabPage("TabBatch");
	}

	private async void fncOpenTabReport(string pTabName)
	{
		if (!clsFunction.IsEmpty(pTabName))
		{
			intDocTabData varTabData = varclsTabManager.Get(pTabName);
			if (varTabData == null)
			{
				await varclsTabManager.AddAsync(pTabName, pSelect: true);
				varTabData = varclsTabManager.Get(pTabName);
				tabContent.TabPages.Add(varTabData.funcGetTabPage());
			}
			else
			{
				varTabData.funcResetLoadStatus();
			}
			tabContent.SelectTab(pTabName);
		}
	}

	private async void funcOpenDbaWizardAsync()
	{
		string varFeatureIdCode = clsFeatureService.consFeatMultiUserDatabase;
		await _clsDataParam.funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatureIdCode), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatureIdCode);
			return;
		}
		funcStopTimerTaskRun();
		intDatabase obj = await new clsDbaFactory().funcGetClassAsync();
		obj.funcGetDbaType();
		new Form();
		Form varFomData;
		if (obj.funcIsLocalDba() || clsFunction.IsAdmin)
		{
			varFomData = new frmDatabase(frmDatabase.enDataType.DbaSelection);
		}
		else
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("CONFIG-MANAGER")))
			{
				return;
			}
			varFomData = new frmConfig(frmConfig.enTabConfig.ProxyData);
		}
		varFomData.ShowDialog(this);
		varFomData.Dispose();
		await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
		await funcLoadFiliaisAsync();
		await funcStartTimerTaskRunAsync();
	}

	private async void fncOpenFiscalServerAsync()
	{
		string varFeatureIdCode = clsFeatureService.consFeatFiscalServer;
		await _clsDataParam.funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatureIdCode), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatureIdCode);
		}
		else if (await clsScreenGeral.funcHasAccessAsync("SERVER-MANAGER"))
		{
			frmServerManager frmServerManager = new frmServerManager();
			frmServerManager.ShowDialog(this);
			frmServerManager.Dispose();
			await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: true);
			await funcStartTimerTaskRunAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: false);
		}
	}

	private void tsbMonTools_Click(object sender, EventArgs e)
	{
		funcOpenTabPage("TabTools");
	}

	private async void funcOpenTabPage(string pTabName, bool pShowWarn = true)
	{
		try
		{
			intDocTabData varTabData = varclsTabManager.Get(pTabName);
			if (varTabData == null)
			{
				await varclsTabManager.AddAsync(pTabName, pSelect: true);
				varTabData = varclsTabManager.Get(pTabName);
				if (varTabData == null)
				{
					return;
				}
				tabContent.TabPages.Add(varTabData.funcGetTabPage());
			}
			else
			{
				intDocTabData varSelected = varclsTabManager.Selected();
				if (varSelected != null && varSelected.Equals(varTabData) && pShowWarn)
				{
					MessageBox.Show(this, "A guia já está aberta.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			tabContent.SelectTab(pTabName);
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void btnSignature_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private void lbSalesText03_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallExtendTrial();
	}

	private async void tsbSearchTerm_TextChanged(object sender, EventArgs e)
	{
		if (varLoadingTabs || varLoadingData || varclsTabManager == null)
		{
			return;
		}
		bool varSearchInteligent = await funcShowSearchInteligentAsync();
		string varSearchTerm = funcGetSearchTerm(tsbSearchTerm.Text);
		string varTextLimit = await _clsDataParam.funcGetAsync("SearchTermCounter");
		if (clsFunction.IsEmpty(varTextLimit))
		{
			varTextLimit = "1000";
		}
		int varCounterLimit = clsFunction.funcConvStrToInt(varTextLimit);
		if (varCounterLimit > 0)
		{
			intDocTabData varTabData = varclsTabManager.Selected();
			if (varTabData != null && varTabData.funcGetTotalDocs() <= varCounterLimit)
			{
				Application.UseWaitCursor = true;
				varTabData.funcSearchText(varSearchTerm, varSearchInteligent);
				stsTotalQuant.Text = "Documentos : " + varTabData.funcGetTotalDocs();
				decimal vrTotalValue = varTabData.funcGetTotalValue();
				stsTotalValue.Text = "Valor Total : " + $"{vrTotalValue:C}";
				stsSumary.Visible = varTabData.ShowSumary();
				Application.UseWaitCursor = false;
			}
		}
	}

	private async void frmMonitor_Resize(object sender, EventArgs e)
	{
		if (!varObjectsLoaded || base.WindowState.Equals(FormWindowState.Minimized))
		{
			return;
		}
		try
		{
			splitMonDataLeft.SplitterDistance = base.Height;
			splitMonFeatures.SplitterDistance = base.Height;
			if (pnUserData.Height > 166)
			{
				pnUserData.Height = 166;
			}
			splitMonFeatures.Panel2MinSize = pnUserData.Height;
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
		if (base.WindowState == FormWindowState.Minimized)
		{
			varFormIsIdle = clsFunction.funcSetProcessPriority(ProcessPriorityClass.Idle);
		}
		else if (varFormIsIdle)
		{
			varFormIsIdle = !clsFunction.funcSetProcessPriority(await _clsDataParam.funcGetAsync("ClientThreadPrior"), ProcessPriorityClass.High);
		}
	}

	private void lkbSupportChat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallChatWebPageAsync("");
	}

	private void picSupportChat_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallChatWebPageAsync("");
	}

	private void pnSupportChat_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallChatWebPageAsync("");
	}

	private async void tsbConsStatusDFeSimple_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe", "CTe", "CTeOs", "MDFe", "NFCe", "NFSe" };
		await funcEventConfirmAsync(varDocTypeList, "949494");
	}

	private async void tsbConsStatusDFeFull_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe", "CTe", "CTeOs" };
		await funcEventConfirmAsync(varDocTypeList, "747474");
	}

	private void tsbConsStatusDFeHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallConsStatusDFeHelp();
	}

	private async void tsbRefreshFilial_Click(object sender, EventArgs e)
	{
		await funcLoadFiliaisAsync(pUseBuffer: false);
	}

	private async void tsbUploadXml_Click(object sender, EventArgs e)
	{
		if (await funcCheckInitialStartupAsync() && await clsScreenGeral.funcHasAccessAsync("DFE-IMPORT-MASS", "EXECUTE"))
		{
			frmDocUpload obj = new frmDocUpload();
			obj.ShowDialog(this);
			obj.Dispose();
			funcDefineOnBoardScreen(null);
			funcCallBackgroundWorkers(pTasks: true, pScan: false);
		}
	}

	private void tsbEdiProcedaHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallEdiProcedaHelp();
	}

	private async void trvFilial_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (varclsFilialManager.IsLoading() || e.Node == null)
		{
			return;
		}
		string varNodeName = clsFunction.funcGetValue(e.Node.Name);
		if (clsFunction.IsEmpty(varNodeName))
		{
			return;
		}
		string varNodeTag = clsFunction.funcGetValue(e.Node.Tag);
		if (clsFunction.IsEmpty(varNodeTag))
		{
			return;
		}
		if (clsFunction.Contains(varNodeName, "-FUNC-"))
		{
			await _clsDataParam.funcAddCounterAsync(varNodeName + "-CLICKS");
		}
		bool varIsLocked = false;
		if (clsFunction.Contains(varNodeName, "-FUNC-"))
		{
			varIsLocked = await funcFeatureSelectedIsLockedAsync(e.Node);
		}
		if (varIsLocked)
		{
			return;
		}
		if (!clsFunction.Contains(varNodeName, "-FUNC-"))
		{
			await _clsDataParam.funcSetAsync("LAST-FILIAL-FOCUSED", varNodeTag);
		}
		else
		{
			await _clsDataParam.funcSetAsync("LAST-FILIAL-FOCUSED", varNodeName);
		}
		FilialView varclsFilial = await funcGetFilialSelectedAsync();
		if (varclsFilial != null && _HasFiscalServer)
		{
			ToolStripButton toolStripButton = tsbBuscarDocs;
			toolStripButton.Visible = await clsManGeral.funcMustScanLocalAsync(_HasFiscalServer, varclsFilial);
		}
		if (sender.GetType().Equals(trvFilial.GetType()) && trvFilial.CheckBoxes)
		{
			return;
		}
		try
		{
			Application.UseWaitCursor = true;
			await funcFilialAfterUserSelect(e.Node, pSetLastNode: true);
		}
		finally
		{
			Application.UseWaitCursor = false;
		}
	}

	private async void trvFilial_AfterCheck(object sender, TreeViewEventArgs e)
	{
		if (varclsFilialManager.IsLoading() || e.Action.Equals(TreeViewAction.Unknown))
		{
			return;
		}
		try
		{
			Application.UseWaitCursor = true;
			await funcFilialAfterUserSelect(e.Node, pSetLastNode: false);
		}
		finally
		{
			Application.UseWaitCursor = false;
		}
	}

	private async Task<bool> funcFilialAfterUserSelect(TreeNode pNode, bool pSetLastNode)
	{
		if (pNode == null)
		{
			return false;
		}
		string varNodeName = clsFunction.funcGetValue(pNode.Name);
		if (clsFunction.IsEmpty(varNodeName))
		{
			return false;
		}
		string varNodeTag = clsFunction.funcGetValue(pNode.Tag);
		if (clsFunction.IsEmpty(varNodeTag))
		{
			return false;
		}
		varclsFilialManager.IsLoading(pValue: true);
		_ = string.Empty;
		string varSelectValue = ((!clsFunction.Contains(varNodeName, "-FUNC-")) ? varNodeTag : varNodeName);
		if (clsFunction.Contains(varNodeName, "-FUNC-"))
		{
			await varclsFilialManager.funcSetNodeCheckBoxAsync(pNode.Checked);
		}
		else
		{
			TreeNode varNodeSelectAll = varclsFilialManager.funcGetNodeSelectAll();
			if (!pNode.Checked && varNodeSelectAll != null)
			{
				varNodeSelectAll.Checked = false;
			}
		}
		if (trvFilial.CheckBoxes && pNode.Checked)
		{
			await _clsDataParam.funcSetAsync("ActFilial-" + varSelectValue, "X");
		}
		else if (!trvFilial.CheckBoxes)
		{
			await _clsDataParam.funcSetAsync("ActFilial", varSelectValue);
		}
		else
		{
			await _clsDataParam.funcSetAsync("ActFilial-" + varSelectValue, string.Empty);
		}
		tscFiliais.ComboBox.SelectedValue = varSelectValue;
		varclsFilialManager.IsLoading(pValue: false);
		if (pSetLastNode)
		{
			if (_LastFilialNode != null)
			{
				_LastFilialNode.BackColor = Color.White;
			}
			TreeNode varSelectedNode = pNode;
			if (varSelectedNode.Parent != null)
			{
				varSelectedNode = varSelectedNode.Parent;
			}
			if (varSelectedNode.Parent != null)
			{
				varSelectedNode = varSelectedNode.Parent;
			}
			trvFilial.SelectedNode = varSelectedNode;
			if (trvFilial.SelectedNode != null)
			{
				trvFilial.SelectedNode.BackColor = Color.CornflowerBlue;
			}
			_LastFilialNode = trvFilial.SelectedNode;
		}
		await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
		return true;
	}

	private void tsmEditFilial_Click(object sender, EventArgs e)
	{
		tsbEdtFilial_Click(sender, e);
	}

	private async void tsbEdtFilial_Click(object sender, EventArgs e)
	{
		clsMeasureService varclsMeasureService = new clsMeasureService(null);
		if (!(await funcCheckInitialStartupAsync()))
		{
			return;
		}
		FilialView varclsFilial = await funcGetFilialSelectedAsync();
		if (varclsFilial != null && await clsScreenGeral.funcHasAccessAsync("FILIAL-MANAGER", "EDIT", varclsFilial.CNPJ))
		{
			varclsMeasureService.funcSyncAsync();
			string varFilialDummnyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
			if (varclsFilial.CNPJ.Equals(varFilialDummnyIdnt))
			{
				MessageBox.Show(string.Concat("Empresa criada automaticamente Fiscal.io Monitor." + Environment.NewLine + Environment.NewLine, "Ela não pode ser alterada manualmente !", Environment.NewLine), "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			frmFilial frmFilial = new frmFilial(varclsFilial);
			frmFilial.ShowDialog(this);
			frmFilial.Dispose();
			await funcLoadFiliaisAsync(pUseBuffer: true, varclsFilial);
			funcCheckFiscalServerFunctionsAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
		}
	}

	private void tsmAddFilial_Click(object sender, EventArgs e)
	{
		tsbAddFilial_Click(sender, e);
	}

	private async void tsbAddFilial_Click(object sender, EventArgs e)
	{
		if (await funcCheckInitialStartupAsync() && await clsScreenGeral.funcHasAccessAsync("FILIAL-MANAGER", "INSERT"))
		{
			frmFilial frmFilial = new frmFilial(null);
			frmFilial.ShowDialog(this);
			frmFilial.Dispose();
			await funcLoadFiliaisAsync();
			funcCheckFiscalServerFunctionsAsync();
			funcCallBackgroundWorkers(pTasks: true, pScan: true);
		}
	}

	private void tsmDelFilial_Click(object sender, EventArgs e)
	{
		tsbExcFilial_Click(sender, e);
	}

	private async void tsbExcFilial_Click(object sender, EventArgs e)
	{
		clsDataFilial varclsDataFilial = new clsDataFilial();
		if (!(await funcCheckInitialStartupAsync()))
		{
			return;
		}
		FilialView varclsFilial = await funcGetFilialSelectedAsync();
		if (varclsFilial != null && await clsScreenGeral.funcHasAccessAsync("FILIAL-MANAGER", "DELETE", varclsFilial.CNPJ))
		{
			string varFilialDummnyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
			if (varclsFilial.CNPJ.Equals(varFilialDummnyIdnt) && !clsFunction.IsAdmin)
			{
				MessageBox.Show(string.Concat("Empresa criada automaticamente Fiscal.io Monitor." + Environment.NewLine + Environment.NewLine, "Ela não pode ser excluída manualmente !", Environment.NewLine), "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (MessageBox.Show("Deseja excluir a empresa [ " + varclsFilial.NomeView + " - " + varclsFilial.CNPJView + " ] ?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				await varclsDataFilial.funcDeleteAsync(varclsFilial);
				await new clsDataTaskAction().funcDeleteByFilialAsync(varclsFilial.CNPJ);
				await funcLoadFiliaisAsync();
				funcCheckFiscalServerFunctionsAsync();
			}
		}
	}

	private async void tsbFilterFilial_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			Application.UseWaitCursor = true;
			tsbFilterFilial.Enabled = false;
			if (tsbFilterFilial.Checked)
			{
				tsbFilterFilial.Checked = false;
			}
			else
			{
				tsbFilterFilial.Checked = true;
			}
			string varParValue = clsFunction.funcConvBoolToStr(tsbFilterFilial.Checked);
			await _clsDataParam.funcSetAsync("FilialShowHideFilter", varParValue);
			await varclsFilialManager.funcDefineFilterListAsync(varParValue);
			await funcLoadFiliaisAsync();
			tsbFilterFilial.Enabled = true;
		}
		finally
		{
			Application.UseWaitCursor = false;
		}
	}

	private void tsbEventsDFeHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallEventManifestDFeHelp();
	}

	private void tsbPluginHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallPluginHelp();
	}

	private async void tsbFullScreen_Click(object sender, EventArgs e)
	{
		bool varMustCollapsed = true;
		if (splitMonData.Panel1Collapsed)
		{
			varMustCollapsed = false;
		}
		SplitContainer splitContainer = splitMonData;
		bool panel1Collapsed = (splitMonitorRight.Panel2Collapsed = varMustCollapsed);
		splitContainer.Panel1Collapsed = panel1Collapsed;
		tscFiliais.Visible = varMustCollapsed;
		await _clsDataParam.funcSetAsync("MonitorIsCollapsed", clsFunction.funcConvBoolToStr(varMustCollapsed));
		tsbFullScreen.Checked = varMustCollapsed;
		await funcSetSelectedTscFilialAsync();
	}

	private void tsbDataColapse_Click(object sender, EventArgs e)
	{
		(varclsTabManager?.Selected())?.funcColapseExpand();
	}

	private void lbLicenseManager_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process varSysProc = new Process();
		varSysProc.StartInfo.FileName = MANAGER_URL;
		try
		{
			varSysProc.Start();
		}
		catch
		{
		}
	}

	private async void lbUserData_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		using (frmLicense varFrmLicense = new frmLicense(frmLicense.enTabPage.LicenseData))
		{
			varFrmLicense.ShowDialog(this);
		}
		await funcSyncUpdateScreenAsync(pReloadFilter: true, pReloadData: false);
	}

	private void lkbBalance_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if ((decimal)lkbBalance.Tag > 0m)
		{
			clsHelpService.funcCallLicenseManagerAsync();
		}
		else
		{
			clsHelpService.funcCallCreditPricePageAsync();
		}
	}

	private void picUserData_Click(object sender, EventArgs e)
	{
		using frmLicense varFrmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
		varFrmLicense.ShowDialog(this);
	}

	private async Task<bool> funcEventConfirmAsync(List<string> pDocTypeList, string pEvtType, bool pClick = false)
	{
		clsEvtService varclsEvtService = new clsEvtService();
		new clsDFeCodes();
		clsMeasureService varclsMeasureService = new clsMeasureService(null);
		if (!(await funcCheckInitialStartupAsync()))
		{
			return false;
		}
		if (varclsTabManager == null)
		{
			return false;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return false;
		}
		varclsMeasureService.funcSyncAsync();
		List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true, pSelectAll: false, pSyncFromDbaFirst: false);
		clsEventData varclsEvtData = new clsEventData
		{
			Agent = "Manual"
		};
		clsEventData clsEventData = varclsEvtData;
		clsEventData.FilialList = await funcGetFilialListAsync();
		varclsEvtData.EvtUserType = pEvtType;
		foreach (Document varclsDoc in varDocList)
		{
			string varDocType = clsFunction.funcGetDocType(varclsDoc.Model, pCTeOs: false);
			if (pDocTypeList.Contains(varDocType) && (!clsFunction.IsEqual(pEvtType, "DOWNLOAD") || varclsEvtService.IsToDown(varclsDoc)) && !varclsEvtData.DocList.Any((Document r) => clsFunction.IsEqual(r.Filial, varclsDoc.Filial) && clsFunction.IsEqual(r.Chave, varclsDoc.Chave)))
			{
				varclsEvtData.DocList.Add(varclsDoc);
			}
		}
		foreach (string varDocType2 in pDocTypeList)
		{
			varclsEvtData.TypeList.Add(varDocType2, pEvtType);
		}
		await varclsEvtService.funcExecuteAsync(this, varclsEvtData);
		if (clsFunction.funcIsShowBatchStatus())
		{
			funcOpenTabPage("TabBatch");
		}
		funcCallBackgroundWorkers(pTasks: true, pScan: true);
		await funcShowSalesDataAsync();
		await varTabData.funcRefreshItensAsync(pFocused: false, pChecked: true);
		return true;
	}

	private async void btNFeManifest_Click(object sender, EventArgs e)
	{
		int varPosX = btNFeManifest.Width / 2;
		int varPosY = btNFeManifest.Height / 2;
		Point varPosition = new Point(varPosX, varPosY);
		contextManifest.Show(btNFeManifest, varPosition);
	}

	private async void tsbDowDFeStart_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe", "NFCe", "CTe", "CTeOs", "NFSe" };
		await funcEventConfirmAsync(varDocTypeList, "DOWNLOAD");
	}

	private async void tsbExport_Click(object sender, EventArgs e)
	{
		await funcExportDocsAsync(enExportScreen.DefaultData);
	}

	private async void tsbBaixarNotFis_Click(object sender, EventArgs e)
	{
		await funcExportDocsAsync(enExportScreen.EdiNotFis);
	}

	private async void tsbBaixarConemb_Click(object sender, EventArgs e)
	{
		await funcExportDocsAsync(enExportScreen.EdiConemb);
	}

	private async Task<bool> funcExportDocsAsync(enExportScreen pScreen)
	{
		if (!(await funcCheckInitialStartupAsync()))
		{
			return false;
		}
		if (varclsTabManager == null)
		{
			return false;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return false;
		}
		if (!(await clsScreenGeral.funcHasAccessAsync("DFE-EXPORT-MASS", "EXECUTE")))
		{
			return false;
		}
		List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		string varUserMessage = string.Empty;
		if (pScreen.Equals(enExportScreen.EdiConemb))
		{
			if (!varDocList.Any((Document r) => clsFunction.funcIsCTe(r.Model)))
			{
				varUserMessage = "Nenhum CTe selecionado para geração do arquivo EDI CONEMB.";
			}
		}
		else if (pScreen.Equals(enExportScreen.EdiNotFis) && !varDocList.Any((Document r) => clsFunction.funcIsNFe(r.Model)))
		{
			varUserMessage = "Nenhuma NFe selecionada para geração do arquivo EDI NOTIFS.";
		}
		if (!clsFunction.IsEmpty(varUserMessage))
		{
			MessageBox.Show(varUserMessage, "Operação cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		frmDynFolder varfrmDynFolder = new frmDynFolder(pSetUserData: true, pScreen);
		if (varfrmDynFolder.ShowDialog().Equals(DialogResult.Cancel))
		{
			return false;
		}
		clsExportData varUserData = varfrmDynFolder.funcGetData();
		varfrmDynFolder.Dispose();
		new List<clsObjectType>();
		clsExportService clsExportService = new clsExportService();
		clsExportService.EventLongRunner += funcEventLongRunner;
		clsReturn varclsReturnFunc = await clsExportService.funcSendDocsAsync(varDocList, varUserData);
		if (varclsReturnFunc.HasError)
		{
			funcSetTaskStatus(varclsReturnFunc);
			return false;
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		varUserMessage = varclsReturnFunc.FirstMessage();
		if (clsFunction.IsEmpty(varUserMessage))
		{
			varUserMessage = "Exportação realizada com sucesso.";
		}
		MessageBox.Show(varUserMessage, "Exportação de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		return true;
	}

	private async void tsbIntegrate_Click(object sender, EventArgs e)
	{
		new clsXmlFactory();
		clsDataChannel varclsDataChannel = new clsDataChannel();
		if (!(await funcCheckInitialStartupAsync()) || varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return;
		}
		List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true, pSelectAll: true, pSyncFromDbaFirst: false);
		if (varDocList.Count <= 0)
		{
			return;
		}
		new List<clsObjectType>();
		if ((await varclsDataChannel.funcGetSenderListAsync()).Count == 0)
		{
			string varMessage = "Nenhum integração para saída de arquivos configurado.";
			varMessage = varMessage + Environment.NewLine + Environment.NewLine;
			varMessage += "Deseja iniciar a configuração de um canal de integração?";
			if (MessageBox.Show(this, varMessage, "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				funcOpenChannelManager("CONFIG_CHANNEL_OUT");
				if ((await varclsDataChannel.funcGetSenderListAsync()).Count == 0)
				{
					return;
				}
			}
		}
		clsIntegrationService varclsService = new clsIntegrationService();
		varclsService.EventLongRunner += funcEventLongRunner;
		IEnumerable<Channel> varChannelsToSend = null;
		ToolStripItem varToolStrip = (ToolStripItem)sender;
		if (varToolStrip != null)
		{
			string varToolStripName = clsFunction.funcGetValue(varToolStrip.Name);
			if (!varToolStrip.Name.Contains("ALL_INT"))
			{
				varChannelsToSend = await varclsDataChannel.funcGetSenderListByKeyAsync(varToolStripName);
			}
		}
		clsReturn varclsReturnFunc = await varclsService.funcSendDocsAsync(varDocList, varChannelsToSend);
		if (varclsReturnFunc.HasError)
		{
			funcSetTaskStatus(varclsReturnFunc);
			return;
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		int varTotalDocs = (int)varclsReturnFunc.GetObject("TotalDocs");
		int varTotalEvts = (int)varclsReturnFunc.GetObject("TotalEvts");
		if (varTotalDocs + varTotalEvts > 0)
		{
			MessageBox.Show(string.Concat(string.Concat(string.Concat("Documentos adicionados a fila de integração" + Environment.NewLine, "conforme filtros de conteúdo configurados.", Environment.NewLine, Environment.NewLine), $"Total de Documentos Fiscais : {varTotalDocs}{Environment.NewLine}"), $"Total de Eventos Fiscais : {varTotalEvts}{Environment.NewLine}"), "Integração de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			MessageBox.Show(string.Concat("Nenhum documento adicionado a fila de integração" + Environment.NewLine + Environment.NewLine, "Verifique os filtros de conteúdo configurados.", Environment.NewLine), "Integração de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		funcCallBackgroundWorkers(pTasks: true, pScan: false);
	}

	private void funcEventLongRunner(object sender, EventLongRunnerEventArgs e)
	{
		if (e.ReturnFunc != null)
		{
			funcSetTaskStatus(e.ReturnFunc);
		}
		else if (!clsFunction.IsEmpty(e.RetMessage))
		{
			funcSetTaskStatus(new clsTaskStatus(e.RetMessage));
		}
		else
		{
			funcSetTaskStatus(new clsTaskStatus(pShow: false));
		}
	}

	private async void btDFeDownload_Click(object sender, EventArgs e)
	{
		List<string> varDocTypeList = new List<string> { "NFe", "NFCe", "CTe", "CTeOs" };
		await funcEventConfirmAsync(varDocTypeList, "DOWNLOAD");
		funcDefineOnBoardScreen(null);
		await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
	}

	private async void tsbAuditor_Click(object sender, EventArgs e)
	{
		string varFeatureIdCode = clsFeatureService.consFeatFiscalAudit;
		await _clsDataParam.funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		foreach (FilialView varclsItem in await funcGetFilialListAsync())
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("AUDIT-MANAGER", "VIEW", varclsItem.CNPJ)))
			{
				return;
			}
		}
		funcOpenTabPage("TabAuditor");
	}

	private async void tsmCopyFilial_Click(object sender, EventArgs e)
	{
		clsDataFilial varclsDataFilial = new clsDataFilial();
		if (!(await funcCheckInitialStartupAsync()))
		{
			return;
		}
		FilialView varclsFilial = await funcGetFilialSelectedAsync();
		if (varclsFilial != null && await clsScreenGeral.funcHasAccessAsync("FILIAL-MANAGER", "INSERT"))
		{
			string varFilialDummnyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
			if (!varclsFilial.CNPJ.Equals(varFilialDummnyIdnt))
			{
				varclsFilial = varclsDataFilial.funcCopyFilial(varclsFilial);
				frmFilial frmFilial = new frmFilial(varclsFilial);
				frmFilial.ShowDialog(this);
				frmFilial.Dispose();
				await funcLoadFiliaisAsync();
				await funcCheckFiscalServerFunctionsAsync();
			}
		}
	}

	private async void tsbDowDFeBatch_Click_Click(object sender, EventArgs e)
	{
		foreach (FilialView varclsItem in await funcGetFilialListAsync())
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("DOWN-MANAGER", "VIEW", varclsItem.CNPJ)))
			{
				return;
			}
		}
		funcOpenTabPage("TabBatch");
	}

	private bool funcHasCert(string varUserText, TreeNode varNode)
	{
		string certType = varUserText.Substring(varUserText.IndexOf("CERT:") + "CERT:".Length)?.Trim();
		if (clsFunction.IsEmpty(certType))
		{
			return false;
		}
		string certText = null;
		foreach (TreeNode node in varNode.Nodes)
		{
			string text = node.Text?.ToUpper();
			if (text != null && text.Contains("CERTIFICADO"))
			{
				certText = text;
				break;
			}
		}
		if (clsFunction.IsEmpty(certText) || !certText.Contains(certType) || certType.Length < 2)
		{
			return false;
		}
		return true;
	}

	private void stsSumary_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		ToolStripItem varclsObject = e.ClickedItem;
		if (varclsObject != null && (clsFunction.IsEqual(varclsObject.Name, "stsHelpCenter") || clsFunction.IsEqual(varclsObject.Name, "stsWhatNew")))
		{
			if (clsFunction.IsEqual(varclsObject.Name, "stsHelpCenter"))
			{
				clsHelpService.funcCallHelpCenterAsync();
			}
			if (clsFunction.IsEqual(varclsObject.Name, "stsWhatNew"))
			{
				clsHelpService.funcCallVersionControlAsync(Application.ProductVersion.Replace(".", ""));
			}
		}
	}

	private void frmMonitor_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		clsHelpService.funcCallHelpCenterAsync();
	}

	private void frmMonitor_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		clsHelpService.funcCallHelpCenterAsync();
	}

	private async void tsbConsExtSyst_Click(object sender, EventArgs e)
	{
		new clsDataDoc();
		if (varclsTabManager == null)
		{
			return;
		}
		intDocTabData varTabData = varclsTabManager.Selected();
		if (varTabData == null)
		{
			return;
		}
		funcSetTaskStatus(new clsTaskStatus("Obtendo lista de documentos ..."));
		List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: true, pChecked: true);
		if (varDocList.Count <= 0)
		{
			return;
		}
		clsDataExtSyst varclsDataExtSyst = new clsDataExtSyst();
		clsDataExtSystFilial varclsDataExtSystFilial = new clsDataExtSystFilial();
		clsConnectFactory varclsFactory = new clsConnectFactory();
		funcSetTaskStatus(new clsTaskStatus("Obtendo a lista de sistemas externos ..."));
		List<ExtSyst> varFinalExList = (await varclsDataExtSyst.funcGetListAsync()).Where((ExtSyst r) => !clsFunction.IsEmpty(r.ForAllCompanies)).ToList();
		List<string> varCnpjStrList = new List<string>();
		foreach (Document varclsItem in varDocList)
		{
			varCnpjStrList.Add(varclsItem.Filial);
		}
		varCnpjStrList = varCnpjStrList.Distinct().ToList();
		new List<FilialView>();
		foreach (string item in varCnpjStrList)
		{
			FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(item);
			if (varclsFilial == null)
			{
				continue;
			}
			foreach (ExtSystFilial varclsExtItem in await varclsDataExtSystFilial.funcGetListByFilialAsync(varclsFilial.CNPJ))
			{
				if (!varFinalExList.Any((ExtSyst r) => clsFunction.IsEqual(r.ID, varclsExtItem.ExtSystId)))
				{
					ExtSyst varExtSystItem = await varclsDataExtSyst.funcGetItemByKeyAsync(varclsExtItem.ExtSystId);
					if (varExtSystItem != null)
					{
						varFinalExList.Add(varExtSystItem);
					}
				}
			}
		}
		clsReturn varclsReturnFunc = new clsReturn();
		foreach (ExtSyst varclsItem2 in varFinalExList)
		{
			funcSetTaskStatus(new clsTaskStatus(varclsItem2.Name + " : Consultando escrituração ..."));
			clsTraceService varclsTracer = new clsTraceService();
			intConnectType varclsService = varclsFactory.funcGetClass(varclsItem2, varclsTracer);
			if (varclsService != null)
			{
				clsReturn clsReturn = varclsReturnFunc;
				clsReturn.AddRange(await varclsService.funcExecuteAsync(varDocList));
			}
		}
		if (varclsReturnFunc.HasError)
		{
			funcSetTaskStatus(new clsTaskStatus(varclsReturnFunc));
		}
		else
		{
			funcSetTaskStatus(new clsTaskStatus());
		}
	}

	private async void toolStripBtnFilterClear_Click(object sender, EventArgs e)
	{
		toolStripBtnFilterClear.Enabled = false;
		toolStripBtnFilterClear.Image = Resources.gif_loading;
		tsbAtualizar01.ToolTipText = "Limpando os filtros ...";
		funcSetTaskStatus(new clsTaskStatus("Atualizando filtro de dados da tela..."));
		clsDataFilter varclsDataFilter = new clsDataFilter();
		await varclsDataFilter.funcResetFilterAsync(trvFeatures);
		varclsDataFilter = await funcGetDataFilterAsync();
		trvFeatures = await varclsDataFilter.funcCreateAsync(trvFeatures);
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		funcCalcRepTotalAsync(varclsDataFilter, pShowMore: false);
		await funcLoadTabViewDataAsync(pSetTool: true, pSetTotal: true, pResetTabs: true);
		toolStripBtnFilterClear.Image = Resources.image_filter_clear;
		toolStripBtnFilterClear.ToolTipText = "Limpar Filtros";
		toolStripBtnFilterClear.Enabled = true;
		MessageBox.Show(this, "Filtros reiniciados com sucesso", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	private void tscFiliais_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (varclsFilialManager.IsLoading())
		{
			return;
		}
		string varCNPJSelect = clsFunction.funcGetValue(tscFiliais.ComboBox.SelectedValue);
		if (varCNPJSelect == null)
		{
			return;
		}
		string varFuncFeatType = clsFunction.funcGetValue(tscFiliais.Tag);
		if (varFuncFeatType == null)
		{
			return;
		}
		TreeNode varSelectedNode = null;
		varclsFilialManager.IsLoading(pValue: true);
		foreach (TreeNode varTreeNode in trvFilial.Nodes)
		{
			string varNodeKey = string.Empty;
			varNodeKey = ((!clsFunction.Contains(varCNPJSelect, "-FUNC-")) ? ((string)varTreeNode.Tag) : varTreeNode.Name);
			if (varNodeKey == null)
			{
				continue;
			}
			if (!clsFunction.Contains(varNodeKey, varCNPJSelect))
			{
				varTreeNode.Checked = false;
				continue;
			}
			varSelectedNode = varTreeNode;
			if (clsFunction.Contains(varFuncFeatType, "LOCK"))
			{
				trvFilial.SelectedNode = varTreeNode;
			}
			else
			{
				varTreeNode.Checked = true;
			}
		}
		varclsFilialManager.IsLoading(pValue: false);
		if (varSelectedNode != null)
		{
			TreeNodeMouseClickEventArgs varArguments = new TreeNodeMouseClickEventArgs(varSelectedNode, MouseButtons.Right, 0, 0, 0);
			trvFilial_NodeMouseClick(tscFiliais, varArguments);
		}
	}

	private async void tsbSearch_Click(object sender, EventArgs e)
	{
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, string.Empty);
			return;
		}
		frmFilialMultiSelect varFrmSelect = new frmFilialMultiSelect(await funcGetFilialListAsync(pSelected: false), pIsSearch: true);
		if (varFrmSelect.ShowDialog().Equals(DialogResult.Cancel))
		{
			return;
		}
		List<FilialView> varclsFilialList = varFrmSelect.funcGetFilialList();
		if (varclsFilialList.Count <= 0)
		{
			return;
		}
		await varclsFilialManager.funcSetNodeCheckBoxAsync(pChecked: false);
		List<Parameter> varParamList = new List<Parameter>();
		foreach (FilialView varclsItem in varclsFilialList)
		{
			Parameter varParam = new Parameter("ActFilial-" + varclsItem.CNPJ, "X");
			varParamList.Add(varParam);
		}
		await _clsDataParam.funcSetAsync(varParamList);
		await funcLoadFiliaisAsync(pUseBuffer: false);
		await funcLoadTabViewDataAsync(pSetTool: false, pSetTotal: true, pResetTabs: true);
	}

	private async void timerBannerCheck_Elapsed(object sender, ElapsedEventArgs e)
	{
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		try
		{
			timerBannerCheck.Enabled = false;
			timerBannerCheck.Stop();
			if (!base.Visible)
			{
				return;
			}
			await varclsUserUsageService.funcRegisterAsync(pFinish: false);
			if (varclsPanelManager == null || varclsPanelManager.IsVisible() || varclsTabManager == null || varclsTabManager.funcCount() <= 0 || clsScreenGeral.funcHasFormOpenAsModal() || !base.ContainsFocus || !SystemInformation.UserInteractive)
			{
				return;
			}
			clsTaskStatus varclsTaskStatus = new clsTaskStatus(pShow: true);
			intDocTabData varSelectedTab = varclsTabManager.Selected();
			if (varSelectedTab == null)
			{
				return;
			}
			int varTotalDocs = varSelectedTab.funcGetTotalDocs();
			if (await varclsFeatService.funcIsToShowBannerAverbExportAsync(clsFeatureService.consReportDocNFeAverConf))
			{
				using frmBannerSearchAverb varFormBanner = new frmBannerSearchAverb();
				varFormBanner.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerNFeNFCeCTeGo(clsFeatureService.consFeatScanDocNFeOut, clsFeatureService.consFeatScanDocNFCeOut, clsFeatureService.consFeatScanDocCTeOut))
			{
				using frmBannerSearchNFeNFCeCTeGO varFormBanner2 = new frmBannerSearchNFeNFCeCTeGO();
				varFormBanner2.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerCFeSatOutAsync(clsFeatureService.consFeatScanDocCFeOut))
			{
				using frmBannerSearchCFeSat varFormBanner3 = new frmBannerSearchCFeSat();
				varFormBanner3.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerNFeMTAsync(clsFeatureService.consFeatScanDocNFeOut))
			{
				using frmBannerSearchNFeMT varFormBanner4 = new frmBannerSearchNFeMT();
				varFormBanner4.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerNFSeAsync(clsFeatureService.consFeatScanDocNFSeIn))
			{
				using frmBannerSearchNFSe varFormBanner5 = new frmBannerSearchNFSe();
				varFormBanner5.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerSpecialCreditsAsync())
			{
				using frmBannerSpecialCredits varFormBanner6 = new frmBannerSpecialCredits();
				varFormBanner6.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerBackupAsync(clsFeatureService.consFeatBackupOfData, varTotalDocs))
			{
				using frmBannerBackup varFormBanner7 = new frmBannerBackup();
				varFormBanner7.ShowDialog();
			}
			else if (await varclsFeatService.funcIsToShowBannerFiscalServerAsync(clsFeatureService.consFeatFiscalServer, varTotalDocs))
			{
				using frmBannerFiscalioServer varFormBanner8 = new frmBannerFiscalioServer();
				varFormBanner8.ShowDialog(this);
			}
			else if (await varclsFeatService.funcIsToShowCampaignMktAsync(clsFeatureService.consFeatFiscalServer))
			{
				using frmCampaignSet2024 varFormBanner9 = new frmCampaignSet2024();
				varFormBanner9.ShowDialog(this);
			}
			else if (await varclsFeatService.funcIsToShowBannerStorageFullAsync())
			{
				using frmBannerStorageFull varFormBanner10 = new frmBannerStorageFull();
				varFormBanner10.ShowDialog(this);
			}
			else if (await varclsFeatService.funcIsToShowBannerStorageAlmostFullAsync())
			{
				using frmBannerStorageAlmostFull varFormBanner11 = new frmBannerStorageAlmostFull();
				varFormBanner11.ShowDialog(this);
			}
			intDatabase varclsDataBase = await new clsDbaFactory().funcGetClassAsync();
			double varTotalSizeInGB = clsFunction.funcConvKBtoGB(clsFunction.funcConvStrToLong(varclsConfig.DatabaseSize));
			if (varclsDataBase.funcIsLocalDba() && varTotalSizeInGB >= 8.0 && varTotalSizeInGB <= 9.0)
			{
				varclsTaskStatus.Show = true;
				varclsTaskStatus.FormName = enPanelName.PanelStorageAlmostFull;
				funcSetTaskStatus(varclsTaskStatus);
			}
			else if (varclsDataBase.funcIsLocalDba() && varTotalSizeInGB > 9.0)
			{
				varclsTaskStatus.Show = true;
				varclsTaskStatus.FormName = enPanelName.PanelStorageFull;
				funcSetTaskStatus(varclsTaskStatus);
			}
		}
		finally
		{
			timerBannerCheck.Enabled = true;
			timerBannerCheck.Start();
		}
	}

	public async Task funcCheckNpsSurveys()
	{
		clsDataConfig varclsDataConfig = new clsDataConfig();
		try
		{
			if (varclsPanelManager == null || varclsPanelManager.IsVisible() || varclsTabManager == null || varclsTabManager.funcCount() <= 0 || clsScreenGeral.funcHasFormOpenAsModal() || !base.ContainsFocus || !SystemInformation.UserInteractive)
			{
				return;
			}
			Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
			List<NpsSurvey> varNpsSurveyList = await new clsDataNpsSurvey().funcGetNotAwnserListAsync(varclsConfig.InstallId);
			if (varNpsSurveyList.Count <= 0)
			{
				return;
			}
			using frmNpsSurvey varNpsSurveyForm = new frmNpsSurvey(varNpsSurveyList.FirstOrDefault());
			varNpsSurveyForm.ShowDialog(this);
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmMonitor));
		this.splitMonData = new System.Windows.Forms.SplitContainer();
		this.splitMonDataLeft = new System.Windows.Forms.SplitContainer();
		this.trvFilial = new Monitor.CustomControls.CustomTreeview();
		this.contextMenuFilial = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmEditFilial = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator35 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmAddFilial = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator36 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmCopyFilial = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmDelFilial = new System.Windows.Forms.ToolStripMenuItem();
		this.ImgLibrary = new System.Windows.Forms.ImageList(this.components);
		this.pnMonTools = new System.Windows.Forms.Panel();
		this.pnAdmTools = new System.Windows.Forms.Panel();
		this.picTaskManager = new System.Windows.Forms.PictureBox();
		this.lkbIntegration03 = new System.Windows.Forms.LinkLabel();
		this.picIntegration = new System.Windows.Forms.PictureBox();
		this.lkbTaskManager = new System.Windows.Forms.LinkLabel();
		this.pnTimerScan = new System.Windows.Forms.Panel();
		this.lbMonToolsSep02 = new System.Windows.Forms.Label();
		this.lbTimerScan = new System.Windows.Forms.Label();
		this.cbTimerScan = new System.Windows.Forms.ComboBox();
		this.picTimerScan = new System.Windows.Forms.PictureBox();
		this.pnBackupSet = new System.Windows.Forms.Panel();
		this.lbBackupSize = new System.Windows.Forms.Label();
		this.lbkBckDate = new System.Windows.Forms.Label();
		this.lbMonToolsSep01 = new System.Windows.Forms.Label();
		this.lkbBackupSet = new System.Windows.Forms.LinkLabel();
		this.picBackupSet = new System.Windows.Forms.PictureBox();
		this.stsFilial = new System.Windows.Forms.StatusStrip();
		this.tslLicenseAgreement = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolbarCompanies = new System.Windows.Forms.ToolStrip();
		this.tsbRefreshFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbAddFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbEdtFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbExcFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbSortFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbFilterFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbSearch = new System.Windows.Forms.ToolStripButton();
		this.splitMonitorRight = new System.Windows.Forms.SplitContainer();
		this.contextMenuScan = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmShowMonitor = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmScanEach1Hour = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmScanEach2Hour = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmScanEach3Hour = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmScanEach6Hour = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmScanManually = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmShowConfig = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator26 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmShowChannelManager = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmShowTaskManager = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator27 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmSair = new System.Windows.Forms.ToolStripMenuItem();
		this.pnStartup = new System.Windows.Forms.Panel();
		this.btDFeDownload = new System.Windows.Forms.Button();
		this.lbLine02 = new System.Windows.Forms.Label();
		this.lbOnboardText10 = new System.Windows.Forms.Label();
		this.btNFeManifest = new System.Windows.Forms.Button();
		this.lbOnboardText08 = new System.Windows.Forms.Label();
		this.lbOnboardText07 = new System.Windows.Forms.Label();
		this.lbOnboardText06 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lbOnboardText05 = new System.Windows.Forms.Label();
		this.lbLine01 = new System.Windows.Forms.Label();
		this.lbOnboardText09 = new System.Windows.Forms.Label();
		this.btSrvDisagree = new System.Windows.Forms.Button();
		this.lbOnboardText03 = new System.Windows.Forms.Label();
		this.lbOnboardText04 = new System.Windows.Forms.Label();
		this.lbOnboardText02 = new System.Windows.Forms.Label();
		this.lbOnboardText01 = new System.Windows.Forms.Label();
		this.btOnboardAction = new System.Windows.Forms.Button();
		this.pnContent = new System.Windows.Forms.Panel();
		this.tabContent = new global::FlatTabControl.FlatTabControl();
		this.imgTabContent = new System.Windows.Forms.ImageList(this.components);
		this.plnMessage = new System.Windows.Forms.Panel();
		this.toolbarDocs02 = new System.Windows.Forms.ToolStrip();
		this.tscFiliais = new System.Windows.Forms.ToolStripComboBox();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbChave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
		this.tsbSearchTerm = new System.Windows.Forms.ToolStripTextBox();
		this.tsbSearchHelp = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tscDateType = new System.Windows.Forms.ToolStripComboBox();
		this.tsbDataIni = new System.Windows.Forms.ToolStripTextBox();
		this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
		this.tsbDataFim = new System.Windows.Forms.ToolStripTextBox();
		this.lbPageSizeSep = new System.Windows.Forms.ToolStripSeparator();
		this.lbPageSize = new System.Windows.Forms.ToolStripLabel();
		this.tsbPageSize = new System.Windows.Forms.ToolStripTextBox();
		this.toolStripSeparator37 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbAtualizar01 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDataColapse = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator40 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbFullScreen = new System.Windows.Forms.ToolStripButton();
		this.stsSumary = new System.Windows.Forms.StatusStrip();
		this.stsTotalValue = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsSumSep01 = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsTotalQuant = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsSumSep02 = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsPageWarn = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsSumSep03 = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsWhatNew = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsSumSep04 = new System.Windows.Forms.ToolStripStatusLabel();
		this.stsHelpCenter = new System.Windows.Forms.ToolStripStatusLabel();
		this.splitMonFeatures = new System.Windows.Forms.SplitContainer();
		this.trvFeatures = new Monitor.CustomControls.CustomTreeview();
		this.pnUserData = new System.Windows.Forms.Panel();
		this.lkbBalance = new System.Windows.Forms.LinkLabel();
		this.label4 = new System.Windows.Forms.Label();
		this.picUserData = new System.Windows.Forms.PictureBox();
		this.lbUserData = new System.Windows.Forms.LinkLabel();
		this.label2 = new System.Windows.Forms.Label();
		this.lkbSupportChat = new System.Windows.Forms.LinkLabel();
		this.picSupportChat = new System.Windows.Forms.PictureBox();
		this.label3 = new System.Windows.Forms.Label();
		this.lbSalesText01 = new System.Windows.Forms.Label();
		this.lbLicenseManager = new System.Windows.Forms.LinkLabel();
		this.btnSignature = new System.Windows.Forms.Button();
		this.toolbarFeatures = new System.Windows.Forms.ToolStrip();
		this.tsbSupport = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripBtnFilterClear = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSyncronize = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
		this.contextSortFilial = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmOrderByDesct = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmOrderByIdent = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarDocs01 = new System.Windows.Forms.ToolStrip();
		this.tsbConfiguration = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator41 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBuscarDocs = new System.Windows.Forms.ToolStripButton();
		this.tsbBuscarDocsAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator29 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmDownDFeMenu = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbDowDFeStart = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator42 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDowDFeBatch_Click = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbEventosNFe = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbEventoCTeDesacordo = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbEventoCTeDesacordoCanc = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator01 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbEventosNFeConfirmacao = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbEventosNFeNaoRealizada = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbEventosNFeDesconhecida = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbEventosNFeCiencia = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator02 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbEventosNFSeConfirmacao = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbEventosNFSeRecusa = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbEventsDFeHelp = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDFeConsStatus = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbConsStatusDFeSimple = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbConsStatusDFeFull = new System.Windows.Forms.ToolStripMenuItem();
		this.tssSepCons01 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbConsExtSyst = new System.Windows.Forms.ToolStripMenuItem();
		this.tssSepCons02 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbConsStatusDFeHelp = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbTags = new System.Windows.Forms.ToolStripDropDownButton();
		this.SepAuditor = new System.Windows.Forms.ToolStripSeparator();
		this.tsbAuditor = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbHandlerEdiFile = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbViewEdiAll = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator31 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarNotFis = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarConemb = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator30 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbEdiProcedaHelp = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbUploadXml = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbExport = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator39 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbExportar = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator28 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbImprimir = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator34 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSendDoc = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator32 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbIntegrations = new System.Windows.Forms.ToolStripDropDownButton();
		this.toolStripSeparator38 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbMonTools = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbPlugins = new System.Windows.Forms.ToolStripDropDownButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator33 = new System.Windows.Forms.ToolStripSeparator();
		this.notifyScan = new System.Windows.Forms.NotifyIcon(this.components);
		this.timerTaskRunUser = new System.Timers.Timer();
		this.timerCheckScan = new System.Timers.Timer();
		this.timerTaskRunSyst = new System.Timers.Timer();
		this.timerBackupAdv = new System.Timers.Timer();
		this.toolTipDbaSize = new System.Windows.Forms.ToolTip(this.components);
		this.tsmTimerScan = new System.Windows.Forms.ToolStripMenuItem();
		this.contextManifest = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmEventosNFeConfirmacao = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmEventosNFeNaoRealizada = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmEventosNFeDesconhecida = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmEventosNFeCiencia = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.backTaskRunUser = new System.ComponentModel.BackgroundWorker();
		this.backTaskRunSyst = new System.ComponentModel.BackgroundWorker();
		this.htmlToolTip1 = new TheArtOfDev.HtmlRenderer.WinForms.HtmlToolTip();
		this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
		this.timerBannerCheck = new System.Timers.Timer();
		((System.ComponentModel.ISupportInitialize)this.splitMonData).BeginInit();
		this.splitMonData.Panel1.SuspendLayout();
		this.splitMonData.Panel2.SuspendLayout();
		this.splitMonData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitMonDataLeft).BeginInit();
		this.splitMonDataLeft.Panel1.SuspendLayout();
		this.splitMonDataLeft.Panel2.SuspendLayout();
		this.splitMonDataLeft.SuspendLayout();
		this.contextMenuFilial.SuspendLayout();
		this.pnMonTools.SuspendLayout();
		this.pnAdmTools.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picTaskManager).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picIntegration).BeginInit();
		this.pnTimerScan.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picTimerScan).BeginInit();
		this.pnBackupSet.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picBackupSet).BeginInit();
		this.stsFilial.SuspendLayout();
		this.toolbarCompanies.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitMonitorRight).BeginInit();
		this.splitMonitorRight.Panel1.SuspendLayout();
		this.splitMonitorRight.Panel2.SuspendLayout();
		this.splitMonitorRight.SuspendLayout();
		this.contextMenuScan.SuspendLayout();
		this.pnStartup.SuspendLayout();
		this.pnContent.SuspendLayout();
		this.toolbarDocs02.SuspendLayout();
		this.stsSumary.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitMonFeatures).BeginInit();
		this.splitMonFeatures.Panel1.SuspendLayout();
		this.splitMonFeatures.Panel2.SuspendLayout();
		this.splitMonFeatures.SuspendLayout();
		this.pnUserData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picUserData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picSupportChat).BeginInit();
		this.toolbarFeatures.SuspendLayout();
		this.contextSortFilial.SuspendLayout();
		this.toolbarDocs01.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.timerTaskRunUser).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.timerCheckScan).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.timerTaskRunSyst).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.timerBackupAdv).BeginInit();
		this.contextManifest.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.timerBannerCheck).BeginInit();
		base.SuspendLayout();
		this.splitMonData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitMonData.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitMonData.Location = new System.Drawing.Point(0, 58);
		this.splitMonData.Name = "splitMonData";
		this.splitMonData.Panel1.Controls.Add(this.splitMonDataLeft);
		this.splitMonData.Panel1.Controls.Add(this.toolbarCompanies);
		this.splitMonData.Panel1MinSize = 241;
		this.splitMonData.Panel2.Controls.Add(this.splitMonitorRight);
		this.splitMonData.Size = new System.Drawing.Size(1225, 653);
		this.splitMonData.SplitterDistance = 241;
		this.splitMonData.SplitterWidth = 3;
		this.splitMonData.TabIndex = 2;
		this.splitMonDataLeft.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitMonDataLeft.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitMonDataLeft.Location = new System.Drawing.Point(0, 31);
		this.splitMonDataLeft.Name = "splitMonDataLeft";
		this.splitMonDataLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitMonDataLeft.Panel1.Controls.Add(this.trvFilial);
		this.splitMonDataLeft.Panel2.Controls.Add(this.pnMonTools);
		this.splitMonDataLeft.Panel2.Controls.Add(this.stsFilial);
		this.splitMonDataLeft.Panel2MinSize = 171;
		this.splitMonDataLeft.Size = new System.Drawing.Size(241, 622);
		this.splitMonDataLeft.SplitterDistance = 449;
		this.splitMonDataLeft.SplitterWidth = 2;
		this.splitMonDataLeft.TabIndex = 3;
		this.trvFilial.CheckBoxes = true;
		this.trvFilial.ContextMenuStrip = this.contextMenuFilial;
		this.trvFilial.Dock = System.Windows.Forms.DockStyle.Fill;
		this.trvFilial.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.trvFilial.FullRowSelect = true;
		this.trvFilial.HotTracking = true;
		this.trvFilial.ImageIndex = 0;
		this.trvFilial.ImageList = this.ImgLibrary;
		this.trvFilial.Location = new System.Drawing.Point(0, 0);
		this.trvFilial.Name = "trvFilial";
		this.trvFilial.SelectedImageIndex = 0;
		this.trvFilial.Size = new System.Drawing.Size(241, 449);
		this.trvFilial.TabIndex = 8;
		this.trvFilial.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(trvFilial_AfterCheck);
		this.trvFilial.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(trvFilial_NodeMouseClick);
		this.contextMenuFilial.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.contextMenuFilial.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuFilial.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.tsmEditFilial, this.toolStripSeparator35, this.tsmAddFilial, this.toolStripSeparator36, this.tsmCopyFilial, this.toolStripSeparator3, this.tsmDelFilial });
		this.contextMenuFilial.Name = "contextMenuFilial";
		this.contextMenuFilial.Size = new System.Drawing.Size(148, 126);
		this.tsmEditFilial.Image = Monitor.Resources.image_edit_object;
		this.tsmEditFilial.Name = "tsmEditFilial";
		this.tsmEditFilial.Size = new System.Drawing.Size(147, 26);
		this.tsmEditFilial.Text = "Alterar item";
		this.tsmEditFilial.Click += new System.EventHandler(tsmEditFilial_Click);
		this.toolStripSeparator35.Name = "toolStripSeparator35";
		this.toolStripSeparator35.Size = new System.Drawing.Size(144, 6);
		this.tsmAddFilial.Image = Monitor.Resources.image_add_object;
		this.tsmAddFilial.Name = "tsmAddFilial";
		this.tsmAddFilial.Size = new System.Drawing.Size(147, 26);
		this.tsmAddFilial.Text = "Incluir item";
		this.tsmAddFilial.Click += new System.EventHandler(tsmAddFilial_Click);
		this.toolStripSeparator36.Name = "toolStripSeparator36";
		this.toolStripSeparator36.Size = new System.Drawing.Size(144, 6);
		this.tsmCopyFilial.Image = Monitor.Resources.image_copy;
		this.tsmCopyFilial.Name = "tsmCopyFilial";
		this.tsmCopyFilial.Size = new System.Drawing.Size(147, 26);
		this.tsmCopyFilial.Text = "Copiar Item";
		this.tsmCopyFilial.Click += new System.EventHandler(tsmCopyFilial_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(144, 6);
		this.tsmDelFilial.Image = Monitor.Resources.image_delete_object;
		this.tsmDelFilial.Name = "tsmDelFilial";
		this.tsmDelFilial.Size = new System.Drawing.Size(147, 26);
		this.tsmDelFilial.Text = "Excluir item";
		this.tsmDelFilial.Click += new System.EventHandler(tsmDelFilial_Click);
		this.ImgLibrary.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImgLibrary.ImageStream");
		this.ImgLibrary.TransparentColor = System.Drawing.Color.Transparent;
		this.ImgLibrary.Images.SetKeyName(0, "image_default.jpg");
		this.ImgLibrary.Images.SetKeyName(1, "image_free.jpg");
		this.ImgLibrary.Images.SetKeyName(2, "image_payed.jpg");
		this.ImgLibrary.Images.SetKeyName(3, "image_locker_16_16.png");
		this.ImgLibrary.Images.SetKeyName(4, "image_selected.jpg");
		this.ImgLibrary.Images.SetKeyName(5, "image_rocket.jpg");
		this.ImgLibrary.Images.SetKeyName(6, "image_groupby.jpg");
		this.ImgLibrary.Images.SetKeyName(7, "image_filter.jpg");
		this.ImgLibrary.Images.SetKeyName(8, "image_report.jpg");
		this.ImgLibrary.Images.SetKeyName(9, "image_audit.png");
		this.pnMonTools.BackColor = System.Drawing.SystemColors.Window;
		this.pnMonTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMonTools.Controls.Add(this.pnAdmTools);
		this.pnMonTools.Controls.Add(this.pnTimerScan);
		this.pnMonTools.Controls.Add(this.pnBackupSet);
		this.pnMonTools.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnMonTools.Location = new System.Drawing.Point(0, 0);
		this.pnMonTools.Name = "pnMonTools";
		this.pnMonTools.Size = new System.Drawing.Size(241, 146);
		this.pnMonTools.TabIndex = 9;
		this.pnAdmTools.Controls.Add(this.picTaskManager);
		this.pnAdmTools.Controls.Add(this.lkbIntegration03);
		this.pnAdmTools.Controls.Add(this.picIntegration);
		this.pnAdmTools.Controls.Add(this.lkbTaskManager);
		this.pnAdmTools.Location = new System.Drawing.Point(0, 92);
		this.pnAdmTools.Name = "pnAdmTools";
		this.pnAdmTools.Size = new System.Drawing.Size(239, 55);
		this.pnAdmTools.TabIndex = 42;
		this.picTaskManager.Image = Monitor.Resources.image_schedule;
		this.picTaskManager.Location = new System.Drawing.Point(5, 29);
		this.picTaskManager.Name = "picTaskManager";
		this.picTaskManager.Size = new System.Drawing.Size(20, 20);
		this.picTaskManager.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picTaskManager.TabIndex = 147;
		this.picTaskManager.TabStop = false;
		this.lkbIntegration03.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lkbIntegration03.Location = new System.Drawing.Point(29, 9);
		this.lkbIntegration03.Name = "lkbIntegration03";
		this.lkbIntegration03.Size = new System.Drawing.Size(203, 13);
		this.lkbIntegration03.TabIndex = 97;
		this.lkbIntegration03.TabStop = true;
		this.lkbIntegration03.Text = "Integração: enviar e receber XML";
		this.lkbIntegration03.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbIntegration03.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbIntegration03_LinkClicked);
		this.picIntegration.Image = Monitor.Resources.image_process_done;
		this.picIntegration.Location = new System.Drawing.Point(5, 5);
		this.picIntegration.Name = "picIntegration";
		this.picIntegration.Size = new System.Drawing.Size(20, 20);
		this.picIntegration.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picIntegration.TabIndex = 146;
		this.picIntegration.TabStop = false;
		this.lkbTaskManager.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lkbTaskManager.Location = new System.Drawing.Point(29, 33);
		this.lkbTaskManager.Name = "lkbTaskManager";
		this.lkbTaskManager.Size = new System.Drawing.Size(203, 13);
		this.lkbTaskManager.TabIndex = 144;
		this.lkbTaskManager.TabStop = true;
		this.lkbTaskManager.Text = "Gerenciar Tarefas";
		this.lkbTaskManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbTaskManager.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbTaskManager_LinkClicked);
		this.pnTimerScan.Controls.Add(this.lbMonToolsSep02);
		this.pnTimerScan.Controls.Add(this.lbTimerScan);
		this.pnTimerScan.Controls.Add(this.cbTimerScan);
		this.pnTimerScan.Controls.Add(this.picTimerScan);
		this.pnTimerScan.Location = new System.Drawing.Point(0, 0);
		this.pnTimerScan.Name = "pnTimerScan";
		this.pnTimerScan.Size = new System.Drawing.Size(239, 52);
		this.pnTimerScan.TabIndex = 40;
		this.lbMonToolsSep02.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbMonToolsSep02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbMonToolsSep02.Location = new System.Drawing.Point(5, 49);
		this.lbMonToolsSep02.Name = "lbMonToolsSep02";
		this.lbMonToolsSep02.Size = new System.Drawing.Size(228, 2);
		this.lbMonToolsSep02.TabIndex = 161;
		this.lbTimerScan.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTimerScan.Location = new System.Drawing.Point(30, 2);
		this.lbTimerScan.Name = "lbTimerScan";
		this.lbTimerScan.Size = new System.Drawing.Size(202, 18);
		this.lbTimerScan.TabIndex = 147;
		this.lbTimerScan.Text = "Busca automática na SEFAZ";
		this.lbTimerScan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cbTimerScan.BackColor = System.Drawing.SystemColors.Info;
		this.cbTimerScan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbTimerScan.FormattingEnabled = true;
		this.cbTimerScan.Location = new System.Drawing.Point(30, 22);
		this.cbTimerScan.Name = "cbTimerScan";
		this.cbTimerScan.Size = new System.Drawing.Size(202, 21);
		this.cbTimerScan.TabIndex = 148;
		this.cbTimerScan.SelectedIndexChanged += new System.EventHandler(cbTimerScan_SelectedIndexChanged);
		this.picTimerScan.Image = Monitor.Resources.image_past;
		this.picTimerScan.Location = new System.Drawing.Point(6, 11);
		this.picTimerScan.Name = "picTimerScan";
		this.picTimerScan.Size = new System.Drawing.Size(20, 20);
		this.picTimerScan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picTimerScan.TabIndex = 158;
		this.picTimerScan.TabStop = false;
		this.pnBackupSet.Controls.Add(this.lbBackupSize);
		this.pnBackupSet.Controls.Add(this.lbkBckDate);
		this.pnBackupSet.Controls.Add(this.lbMonToolsSep01);
		this.pnBackupSet.Controls.Add(this.lkbBackupSet);
		this.pnBackupSet.Controls.Add(this.picBackupSet);
		this.pnBackupSet.Location = new System.Drawing.Point(0, 52);
		this.pnBackupSet.Name = "pnBackupSet";
		this.pnBackupSet.Size = new System.Drawing.Size(239, 40);
		this.pnBackupSet.TabIndex = 41;
		this.lbBackupSize.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.lbBackupSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lbBackupSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lbBackupSize.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.lbBackupSize.ForeColor = System.Drawing.SystemColors.WindowText;
		this.lbBackupSize.Location = new System.Drawing.Point(172, 7);
		this.lbBackupSize.Name = "lbBackupSize";
		this.lbBackupSize.Size = new System.Drawing.Size(60, 23);
		this.lbBackupSize.TabIndex = 152;
		this.lbBackupSize.Text = "100 MB";
		this.lbBackupSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.toolTipDbaSize.SetToolTip(this.lbBackupSize, "Informações essências que precisam de BACKUP.\r\nSão arquivos XML's para NFe, CTe e MDFe e \r\ndados de utilização do Monitor que precisam \r\nde criação de cópias de segurança com frequência.");
		this.lbBackupSize.Click += new System.EventHandler(lbBackup_Click);
		this.lbkBckDate.Font = new System.Drawing.Font("Verdana", 6.25f);
		this.lbkBckDate.ForeColor = System.Drawing.Color.Teal;
		this.lbkBckDate.Location = new System.Drawing.Point(29, 19);
		this.lbkBckDate.Name = "lbkBckDate";
		this.lbkBckDate.Size = new System.Drawing.Size(140, 12);
		this.lbkBckDate.TabIndex = 155;
		this.lbkBckDate.Text = "Em 23/10 13:00";
		this.lbkBckDate.Click += new System.EventHandler(lbkBckDate_Click);
		this.lbMonToolsSep01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbMonToolsSep01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbMonToolsSep01.Location = new System.Drawing.Point(6, 37);
		this.lbMonToolsSep01.Name = "lbMonToolsSep01";
		this.lbMonToolsSep01.Size = new System.Drawing.Size(228, 2);
		this.lbMonToolsSep01.TabIndex = 159;
		this.lkbBackupSet.Location = new System.Drawing.Point(29, 5);
		this.lkbBackupSet.Name = "lkbBackupSet";
		this.lkbBackupSet.Size = new System.Drawing.Size(140, 13);
		this.lkbBackupSet.TabIndex = 154;
		this.lkbBackupSet.TabStop = true;
		this.lkbBackupSet.Text = "Backup realizado";
		this.lkbBackupSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbBackupSet.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbBackup_LinkClicked);
		this.picBackupSet.Image = Monitor.Resources.image_ok;
		this.picBackupSet.Location = new System.Drawing.Point(5, 7);
		this.picBackupSet.Name = "picBackupSet";
		this.picBackupSet.Size = new System.Drawing.Size(20, 20);
		this.picBackupSet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBackupSet.TabIndex = 153;
		this.picBackupSet.TabStop = false;
		this.stsFilial.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.stsFilial.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.tslLicenseAgreement });
		this.stsFilial.Location = new System.Drawing.Point(0, 146);
		this.stsFilial.Name = "stsFilial";
		this.stsFilial.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.stsFilial.Size = new System.Drawing.Size(241, 25);
		this.stsFilial.TabIndex = 160;
		this.stsFilial.Text = "statusStrip2";
		this.tslLicenseAgreement.BackColor = System.Drawing.SystemColors.Control;
		this.tslLicenseAgreement.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.tslLicenseAgreement.Image = Monitor.Resources.image_agreement;
		this.tslLicenseAgreement.IsLink = true;
		this.tslLicenseAgreement.Name = "tslLicenseAgreement";
		this.tslLicenseAgreement.Size = new System.Drawing.Size(226, 20);
		this.tslLicenseAgreement.Spring = true;
		this.tslLicenseAgreement.Text = "Termos e condições de uso";
		this.tslLicenseAgreement.Click += new System.EventHandler(tslLicenseAgreement_Click);
		this.toolbarCompanies.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.toolbarCompanies.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolbarCompanies.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolbarCompanies.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.tsbRefreshFilial, this.tsbAddFilial, this.tsbEdtFilial, this.tsbExcFilial, this.tsbSortFilial, this.tsbFilterFilial, this.tsbSearch });
		this.toolbarCompanies.Location = new System.Drawing.Point(0, 0);
		this.toolbarCompanies.Name = "toolbarCompanies";
		this.toolbarCompanies.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
		this.toolbarCompanies.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.toolbarCompanies.Size = new System.Drawing.Size(241, 31);
		this.toolbarCompanies.Stretch = true;
		this.toolbarCompanies.TabIndex = 2;
		this.tsbRefreshFilial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbRefreshFilial.Image = Monitor.Resources.image_refresh;
		this.tsbRefreshFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbRefreshFilial.Name = "tsbRefreshFilial";
		this.tsbRefreshFilial.Size = new System.Drawing.Size(24, 24);
		this.tsbRefreshFilial.Text = "Atualizar lista";
		this.tsbRefreshFilial.Click += new System.EventHandler(tsbRefreshFilial_Click);
		this.tsbAddFilial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbAddFilial.Image = Monitor.Resources.image_add_object;
		this.tsbAddFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbAddFilial.Name = "tsbAddFilial";
		this.tsbAddFilial.Size = new System.Drawing.Size(24, 24);
		this.tsbAddFilial.Text = "Adicionar CNPJ ou CPF";
		this.tsbAddFilial.Click += new System.EventHandler(tsbAddFilial_Click);
		this.tsbEdtFilial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbEdtFilial.Image = Monitor.Resources.image_edit_object;
		this.tsbEdtFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbEdtFilial.Name = "tsbEdtFilial";
		this.tsbEdtFilial.Size = new System.Drawing.Size(24, 24);
		this.tsbEdtFilial.Text = "Editar item";
		this.tsbEdtFilial.Click += new System.EventHandler(tsbEdtFilial_Click);
		this.tsbExcFilial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbExcFilial.Image = Monitor.Resources.image_delete_object;
		this.tsbExcFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExcFilial.Name = "tsbExcFilial";
		this.tsbExcFilial.Size = new System.Drawing.Size(24, 24);
		this.tsbExcFilial.Text = "Excluir item";
		this.tsbExcFilial.Click += new System.EventHandler(tsbExcFilial_Click);
		this.tsbSortFilial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbSortFilial.Image = Monitor.Resources.image_sorter;
		this.tsbSortFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSortFilial.Name = "tsbSortFilial";
		this.tsbSortFilial.Size = new System.Drawing.Size(24, 24);
		this.tsbSortFilial.Text = "Ordenar lista";
		this.tsbSortFilial.Click += new System.EventHandler(tsbSortFilial_Click);
		this.tsbFilterFilial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbFilterFilial.Image = Monitor.Resources.image_filter_on;
		this.tsbFilterFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbFilterFilial.Name = "tsbFilterFilial";
		this.tsbFilterFilial.Size = new System.Drawing.Size(24, 24);
		this.tsbFilterFilial.Text = "Exibir/Ocultar itens não selecionados";
		this.tsbFilterFilial.Click += new System.EventHandler(tsbFilterFilial_Click);
		this.tsbSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbSearch.Image = Monitor.Resources.image_search;
		this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSearch.Name = "tsbSearch";
		this.tsbSearch.Size = new System.Drawing.Size(24, 24);
		this.tsbSearch.Text = "Procurar uma ou mais empresas";
		this.tsbSearch.Click += new System.EventHandler(tsbSearch_Click);
		this.splitMonitorRight.BackColor = System.Drawing.SystemColors.Control;
		this.splitMonitorRight.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitMonitorRight.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitMonitorRight.Location = new System.Drawing.Point(0, 0);
		this.splitMonitorRight.Name = "splitMonitorRight";
		this.splitMonitorRight.Panel1.ContextMenuStrip = this.contextMenuScan;
		this.splitMonitorRight.Panel1.Controls.Add(this.pnStartup);
		this.splitMonitorRight.Panel1.Controls.Add(this.pnContent);
		this.splitMonitorRight.Panel1.Controls.Add(this.plnMessage);
		this.splitMonitorRight.Panel1.Controls.Add(this.toolbarDocs02);
		this.splitMonitorRight.Panel1.Controls.Add(this.stsSumary);
		this.splitMonitorRight.Panel2.Controls.Add(this.splitMonFeatures);
		this.splitMonitorRight.Panel2.Controls.Add(this.toolbarFeatures);
		this.splitMonitorRight.Size = new System.Drawing.Size(981, 653);
		this.splitMonitorRight.SplitterDistance = 742;
		this.splitMonitorRight.TabIndex = 0;
		this.contextMenuScan.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuScan.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.tsmShowMonitor, this.toolStripSeparator9, this.tsmScanEach1Hour, this.tsmScanEach2Hour, this.tsmScanEach3Hour, this.tsmScanEach6Hour, this.tsmScanManually, this.toolStripSeparator19, this.tsmShowConfig, this.toolStripSeparator26,
			this.tsmShowChannelManager, this.tsmShowTaskManager, this.toolStripSeparator27, this.tsmSair
		});
		this.contextMenuScan.Name = "contextMenuScan";
		this.contextMenuScan.Size = new System.Drawing.Size(207, 288);
		this.tsmShowMonitor.Image = Monitor.Resources.image_download;
		this.tsmShowMonitor.Name = "tsmShowMonitor";
		this.tsmShowMonitor.Size = new System.Drawing.Size(206, 26);
		this.tsmShowMonitor.Text = "Abrir Monitor";
		this.tsmShowMonitor.Click += new System.EventHandler(tsmShowMonitor_Click);
		this.toolStripSeparator9.Name = "toolStripSeparator9";
		this.toolStripSeparator9.Size = new System.Drawing.Size(203, 6);
		this.tsmScanEach1Hour.Name = "tsmScanEach1Hour";
		this.tsmScanEach1Hour.Size = new System.Drawing.Size(206, 26);
		this.tsmScanEach1Hour.Tag = "EACH_1HOUR";
		this.tsmScanEach1Hour.Text = "Buscar a cada 1 hora";
		this.tsmScanEach1Hour.Click += new System.EventHandler(tsmScanTimer_Click);
		this.tsmScanEach2Hour.Name = "tsmScanEach2Hour";
		this.tsmScanEach2Hour.Size = new System.Drawing.Size(206, 26);
		this.tsmScanEach2Hour.Tag = "EACH_2HOUR";
		this.tsmScanEach2Hour.Text = "Buscar a cada 2 horas";
		this.tsmScanEach2Hour.Click += new System.EventHandler(tsmScanTimer_Click);
		this.tsmScanEach3Hour.Name = "tsmScanEach3Hour";
		this.tsmScanEach3Hour.Size = new System.Drawing.Size(206, 26);
		this.tsmScanEach3Hour.Tag = "EACH_3HOUR";
		this.tsmScanEach3Hour.Text = "Buscar a cada 3 horas";
		this.tsmScanEach3Hour.Click += new System.EventHandler(tsmScanTimer_Click);
		this.tsmScanEach6Hour.Name = "tsmScanEach6Hour";
		this.tsmScanEach6Hour.Size = new System.Drawing.Size(206, 26);
		this.tsmScanEach6Hour.Tag = "EACH_6HOUR";
		this.tsmScanEach6Hour.Text = "Buscar a cada 6 horas";
		this.tsmScanEach6Hour.Click += new System.EventHandler(tsmScanTimer_Click);
		this.tsmScanManually.Name = "tsmScanManually";
		this.tsmScanManually.Size = new System.Drawing.Size(206, 26);
		this.tsmScanManually.Tag = "SCAN_DISAB";
		this.tsmScanManually.Text = "Buscar manualmente";
		this.tsmScanManually.Click += new System.EventHandler(tsmScanTimer_Click);
		this.toolStripSeparator19.Name = "toolStripSeparator19";
		this.toolStripSeparator19.Size = new System.Drawing.Size(203, 6);
		this.tsmShowConfig.Image = (System.Drawing.Image)resources.GetObject("tsmShowConfig.Image");
		this.tsmShowConfig.Name = "tsmShowConfig";
		this.tsmShowConfig.Size = new System.Drawing.Size(206, 26);
		this.tsmShowConfig.Text = "Configurações";
		this.tsmShowConfig.Click += new System.EventHandler(tsmShowConfig_Click);
		this.toolStripSeparator26.Name = "toolStripSeparator26";
		this.toolStripSeparator26.Size = new System.Drawing.Size(203, 6);
		this.tsmShowChannelManager.Image = (System.Drawing.Image)resources.GetObject("tsmShowChannelManager.Image");
		this.tsmShowChannelManager.Name = "tsmShowChannelManager";
		this.tsmShowChannelManager.Size = new System.Drawing.Size(206, 26);
		this.tsmShowChannelManager.Text = "Canais de Comunicação";
		this.tsmShowChannelManager.Click += new System.EventHandler(tsmShowChannelManager_Click);
		this.tsmShowTaskManager.Image = (System.Drawing.Image)resources.GetObject("tsmShowTaskManager.Image");
		this.tsmShowTaskManager.Name = "tsmShowTaskManager";
		this.tsmShowTaskManager.Size = new System.Drawing.Size(206, 26);
		this.tsmShowTaskManager.Text = "Gerenciador de Tarefas";
		this.tsmShowTaskManager.Click += new System.EventHandler(tsmShowTaskManager_Click);
		this.toolStripSeparator27.Name = "toolStripSeparator27";
		this.toolStripSeparator27.Size = new System.Drawing.Size(203, 6);
		this.tsmSair.Name = "tsmSair";
		this.tsmSair.Size = new System.Drawing.Size(206, 26);
		this.tsmSair.Text = "&Sair";
		this.tsmSair.Click += new System.EventHandler(tsmSair_Click);
		this.pnStartup.BackColor = System.Drawing.Color.White;
		this.pnStartup.Controls.Add(this.btDFeDownload);
		this.pnStartup.Controls.Add(this.lbLine02);
		this.pnStartup.Controls.Add(this.lbOnboardText10);
		this.pnStartup.Controls.Add(this.btNFeManifest);
		this.pnStartup.Controls.Add(this.lbOnboardText08);
		this.pnStartup.Controls.Add(this.lbOnboardText07);
		this.pnStartup.Controls.Add(this.lbOnboardText06);
		this.pnStartup.Controls.Add(this.label1);
		this.pnStartup.Controls.Add(this.lbOnboardText05);
		this.pnStartup.Controls.Add(this.lbLine01);
		this.pnStartup.Controls.Add(this.lbOnboardText09);
		this.pnStartup.Controls.Add(this.btSrvDisagree);
		this.pnStartup.Controls.Add(this.lbOnboardText03);
		this.pnStartup.Controls.Add(this.lbOnboardText04);
		this.pnStartup.Controls.Add(this.lbOnboardText02);
		this.pnStartup.Controls.Add(this.lbOnboardText01);
		this.pnStartup.Controls.Add(this.btOnboardAction);
		this.pnStartup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnStartup.Location = new System.Drawing.Point(0, 69);
		this.pnStartup.Name = "pnStartup";
		this.pnStartup.Size = new System.Drawing.Size(742, 559);
		this.pnStartup.TabIndex = 12;
		this.pnStartup.Visible = false;
		this.btDFeDownload.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btDFeDownload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btDFeDownload.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btDFeDownload.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btDFeDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btDFeDownload.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btDFeDownload.ForeColor = System.Drawing.Color.White;
		this.btDFeDownload.Image = Monitor.Resources.dfe_approved;
		this.btDFeDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btDFeDownload.Location = new System.Drawing.Point(402, 512);
		this.btDFeDownload.Name = "btDFeDownload";
		this.btDFeDownload.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
		this.btDFeDownload.Size = new System.Drawing.Size(289, 40);
		this.btDFeDownload.TabIndex = 47;
		this.btDFeDownload.Text = "Baixar XML e PDF direto da SEFAZ";
		this.btDFeDownload.UseVisualStyleBackColor = false;
		this.btDFeDownload.Click += new System.EventHandler(btDFeDownload_Click);
		this.lbLine02.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbLine02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbLine02.Location = new System.Drawing.Point(44, 453);
		this.lbLine02.Name = "lbLine02";
		this.lbLine02.Size = new System.Drawing.Size(635, 2);
		this.lbLine02.TabIndex = 46;
		this.lbOnboardText10.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText10.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbOnboardText10.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbOnboardText10.Location = new System.Drawing.Point(105, 468);
		this.lbOnboardText10.Name = "lbOnboardText10";
		this.lbOnboardText10.Size = new System.Drawing.Size(535, 25);
		this.lbOnboardText10.TabIndex = 45;
		this.lbOnboardText10.Text = "Outras ações rápidas que podem ser realizadas pelo Fiscal.io Monitor";
		this.lbOnboardText10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btNFeManifest.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btNFeManifest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btNFeManifest.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btNFeManifest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btNFeManifest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btNFeManifest.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btNFeManifest.ForeColor = System.Drawing.Color.White;
		this.btNFeManifest.Image = (System.Drawing.Image)resources.GetObject("btNFeManifest.Image");
		this.btNFeManifest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btNFeManifest.Location = new System.Drawing.Point(47, 512);
		this.btNFeManifest.Name = "btNFeManifest";
		this.btNFeManifest.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
		this.btNFeManifest.Size = new System.Drawing.Size(289, 40);
		this.btNFeManifest.TabIndex = 44;
		this.btNFeManifest.Text = "    NFe :\u00a0 Manifestação do Destinatário";
		this.btNFeManifest.UseVisualStyleBackColor = false;
		this.btNFeManifest.Click += new System.EventHandler(btNFeManifest_Click);
		this.lbOnboardText08.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText08.Location = new System.Drawing.Point(472, 261);
		this.lbOnboardText08.Name = "lbOnboardText08";
		this.lbOnboardText08.Size = new System.Drawing.Size(217, 54);
		this.lbOnboardText08.TabIndex = 43;
		this.lbOnboardText08.Text = "Fazer registro em massa de Manifestação do Destinatário";
		this.lbOnboardText08.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbOnboardText07.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText07.Location = new System.Drawing.Point(260, 261);
		this.lbOnboardText07.Name = "lbOnboardText07";
		this.lbOnboardText07.Size = new System.Drawing.Size(217, 54);
		this.lbOnboardText07.TabIndex = 42;
		this.lbOnboardText07.Text = "Gerenciar os documentos de entrada de meus clientes";
		this.lbOnboardText07.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbOnboardText06.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText06.Location = new System.Drawing.Point(37, 261);
		this.lbOnboardText06.Name = "lbOnboardText06";
		this.lbOnboardText06.Size = new System.Drawing.Size(217, 54);
		this.lbOnboardText06.TabIndex = 41;
		this.lbOnboardText06.Text = "Download, armazenamento e gerenciamento das NFe, CTe, MDFe e eventos de entrada";
		this.lbOnboardText06.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(44, 150);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(635, 2);
		this.label1.TabIndex = 40;
		this.lbOnboardText05.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText05.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbOnboardText05.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbOnboardText05.Location = new System.Drawing.Point(105, 165);
		this.lbOnboardText05.Name = "lbOnboardText05";
		this.lbOnboardText05.Size = new System.Drawing.Size(535, 40);
		this.lbOnboardText05.TabIndex = 39;
		this.lbOnboardText05.Text = "Se o seu objetivo é fazer a gestão dos documentos fiscais, o primeiro passo é realizar uma busca na SEFAZ.";
		this.lbOnboardText05.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbLine01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbLine01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbLine01.Location = new System.Drawing.Point(55, 329);
		this.lbLine01.Name = "lbLine01";
		this.lbLine01.Size = new System.Drawing.Size(635, 2);
		this.lbLine01.TabIndex = 38;
		this.lbOnboardText09.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText09.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbOnboardText09.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbOnboardText09.Location = new System.Drawing.Point(105, 341);
		this.lbOnboardText09.Name = "lbOnboardText09";
		this.lbOnboardText09.Size = new System.Drawing.Size(535, 40);
		this.lbOnboardText09.TabIndex = 21;
		this.lbOnboardText09.Text = "Se deseja apenas registrar a Prestação de Serviço em Desacordo para CTe, \r\nclique no botão abaixo.";
		this.lbOnboardText09.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btSrvDisagree.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btSrvDisagree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.btSrvDisagree.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btSrvDisagree.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btSrvDisagree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSrvDisagree.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btSrvDisagree.ForeColor = System.Drawing.Color.White;
		this.btSrvDisagree.Image = Monitor.Resources.dfe_disagree;
		this.btSrvDisagree.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btSrvDisagree.Location = new System.Drawing.Point(214, 392);
		this.btSrvDisagree.Name = "btSrvDisagree";
		this.btSrvDisagree.Size = new System.Drawing.Size(294, 40);
		this.btSrvDisagree.TabIndex = 20;
		this.btSrvDisagree.Text = "CTe :\u00a0Desacordo de Serviço";
		this.btSrvDisagree.UseVisualStyleBackColor = false;
		this.btSrvDisagree.Click += new System.EventHandler(btSrvDisagree_Click);
		this.lbOnboardText03.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText03.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.lbOnboardText03.Location = new System.Drawing.Point(189, 79);
		this.lbOnboardText03.Name = "lbOnboardText03";
		this.lbOnboardText03.Size = new System.Drawing.Size(367, 24);
		this.lbOnboardText03.TabIndex = 12;
		this.lbOnboardText03.Text = "para a utilização do Fiscal.io Monitor !";
		this.lbOnboardText03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbOnboardText04.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText04.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.lbOnboardText04.Location = new System.Drawing.Point(189, 111);
		this.lbOnboardText04.Name = "lbOnboardText04";
		this.lbOnboardText04.Size = new System.Drawing.Size(367, 24);
		this.lbOnboardText04.TabIndex = 11;
		this.lbOnboardText04.Text = "VAMOS INICIAR?";
		this.lbOnboardText04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbOnboardText02.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText02.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.lbOnboardText02.Location = new System.Drawing.Point(189, 49);
		this.lbOnboardText02.Name = "lbOnboardText02";
		this.lbOnboardText02.Size = new System.Drawing.Size(367, 24);
		this.lbOnboardText02.TabIndex = 10;
		this.lbOnboardText02.Text = "Parabéns. Você fez todas as configurações";
		this.lbOnboardText02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbOnboardText01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbOnboardText01.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.lbOnboardText01.Location = new System.Drawing.Point(178, 16);
		this.lbOnboardText01.Name = "lbOnboardText01";
		this.lbOnboardText01.Size = new System.Drawing.Size(367, 27);
		this.lbOnboardText01.TabIndex = 9;
		this.lbOnboardText01.Text = "Olá Lucas Farley,";
		this.lbOnboardText01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btOnboardAction.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btOnboardAction.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btOnboardAction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btOnboardAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btOnboardAction.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btOnboardAction.ForeColor = System.Drawing.Color.White;
		this.btOnboardAction.Image = (System.Drawing.Image)resources.GetObject("btOnboardAction.Image");
		this.btOnboardAction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btOnboardAction.Location = new System.Drawing.Point(224, 214);
		this.btOnboardAction.Name = "btOnboardAction";
		this.btOnboardAction.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
		this.btOnboardAction.Size = new System.Drawing.Size(274, 40);
		this.btOnboardAction.TabIndex = 8;
		this.btOnboardAction.Text = "  BUSCAR DOCUMENTOS NA SEFAZ";
		this.btOnboardAction.UseVisualStyleBackColor = false;
		this.btOnboardAction.Click += new System.EventHandler(btBuscarDocsAll_Click);
		this.pnContent.Controls.Add(this.tabContent);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 69);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(742, 559);
		this.pnContent.TabIndex = 11;
		this.pnContent.Visible = false;
		this.tabContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabContent.ImageList = this.imgTabContent;
		this.tabContent.Location = new System.Drawing.Point(0, 0);
		this.tabContent.myBackColor = System.Drawing.SystemColors.Control;
		this.tabContent.Name = "tabContent";
		this.tabContent.SelectedIndex = 0;
		this.tabContent.Size = new System.Drawing.Size(742, 559);
		this.tabContent.TabIndex = 0;
		this.tabContent.SelectedIndexChanged += new System.EventHandler(tabContent_SelectedIndexChanged);
		this.tabContent.Selected += new System.Windows.Forms.TabControlEventHandler(tabContent_Selected);
		this.tabContent.ControlRemoved += new System.Windows.Forms.ControlEventHandler(tabContent_ControlRemoved);
		this.tabContent.MouseClick += new System.Windows.Forms.MouseEventHandler(tabContent_MouseClick);
		this.imgTabContent.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgTabContent.ImageStream");
		this.imgTabContent.TransparentColor = System.Drawing.Color.Transparent;
		this.imgTabContent.Images.SetKeyName(0, "image_report.jpg");
		this.imgTabContent.Images.SetKeyName(1, "image_mon_tools");
		this.imgTabContent.Images.SetKeyName(2, "image_carrier.png");
		this.imgTabContent.Images.SetKeyName(3, "image_audit.png");
		this.imgTabContent.Images.SetKeyName(4, "image_download_cloud.png");
		this.plnMessage.BackColor = System.Drawing.SystemColors.Window;
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Dock = System.Windows.Forms.DockStyle.Top;
		this.plnMessage.Location = new System.Drawing.Point(0, 30);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(742, 39);
		this.plnMessage.TabIndex = 10;
		this.toolbarDocs02.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.toolbarDocs02.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolbarDocs02.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolbarDocs02.Items.AddRange(new System.Windows.Forms.ToolStripItem[21]
		{
			this.tscFiliais, this.toolStripSeparator6, this.tsbChave, this.toolStripSeparator5, this.toolStripLabel5, this.tsbSearchTerm, this.tsbSearchHelp, this.toolStripSeparator1, this.tscDateType, this.tsbDataIni,
			this.toolStripLabel2, this.tsbDataFim, this.lbPageSizeSep, this.lbPageSize, this.tsbPageSize, this.toolStripSeparator37, this.tsbAtualizar01, this.toolStripSeparator12, this.tsbDataColapse, this.toolStripSeparator40,
			this.tsbFullScreen
		});
		this.toolbarDocs02.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
		this.toolbarDocs02.Location = new System.Drawing.Point(0, 0);
		this.toolbarDocs02.Name = "toolbarDocs02";
		this.toolbarDocs02.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
		this.toolbarDocs02.Size = new System.Drawing.Size(742, 30);
		this.toolbarDocs02.TabIndex = 9;
		this.tscFiliais.AutoSize = false;
		this.tscFiliais.BackColor = System.Drawing.SystemColors.Info;
		this.tscFiliais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.tscFiliais.DropDownWidth = 250;
		this.tscFiliais.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.tscFiliais.Name = "tscFiliais";
		this.tscFiliais.Size = new System.Drawing.Size(140, 21);
		this.tscFiliais.Visible = false;
		this.tscFiliais.SelectedIndexChanged += new System.EventHandler(tscFiliais_SelectedIndexChanged);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
		this.tsbChave.AutoSize = false;
		this.tsbChave.Image = Monitor.Resources.image_barcode;
		this.tsbChave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbChave.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
		this.tsbChave.Name = "tsbChave";
		this.tsbChave.Size = new System.Drawing.Size(74, 23);
		this.tsbChave.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbChave.Text = "Chaves";
		this.tsbChave.ToolTipText = "Filtro por múltiplas chaves de acesso";
		this.tsbChave.Click += new System.EventHandler(tsbChave_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator5.Tag = "";
		this.toolStripLabel5.AutoSize = false;
		this.toolStripLabel5.Name = "toolStripLabel5";
		this.toolStripLabel5.Size = new System.Drawing.Size(50, 23);
		this.toolStripLabel5.Text = "Busca :";
		this.tsbSearchTerm.AutoSize = false;
		this.tsbSearchTerm.AutoToolTip = true;
		this.tsbSearchTerm.BackColor = System.Drawing.SystemColors.Info;
		this.tsbSearchTerm.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.tsbSearchTerm.Name = "tsbSearchTerm";
		this.tsbSearchTerm.Size = new System.Drawing.Size(150, 23);
		this.tsbSearchTerm.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tsbSearchTerm.KeyDown += new System.Windows.Forms.KeyEventHandler(tsbSearchTerm_KeyDown);
		this.tsbSearchTerm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tsbSearchTerm_KeyPress);
		this.tsbSearchTerm.TextChanged += new System.EventHandler(tsbSearchTerm_TextChanged);
		this.tsbSearchHelp.AutoSize = false;
		this.tsbSearchHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbSearchHelp.Image = Monitor.Resources.image_help;
		this.tsbSearchHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSearchHelp.Name = "tsbSearchHelp";
		this.tsbSearchHelp.Size = new System.Drawing.Size(22, 22);
		this.tsbSearchHelp.Text = "Ajuda";
		this.tsbSearchHelp.Click += new System.EventHandler(tsbSearchHelp_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator1.Tag = "#NOT-TABPARTNER";
		this.tscDateType.AutoSize = false;
		this.tscDateType.BackColor = System.Drawing.SystemColors.Info;
		this.tscDateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.tscDateType.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.tscDateType.Name = "tscDateType";
		this.tscDateType.Size = new System.Drawing.Size(92, 21);
		this.tscDateType.SelectedIndexChanged += new System.EventHandler(tscDateType_SelectedIndexChanged);
		this.tsbDataIni.AutoSize = false;
		this.tsbDataIni.BackColor = System.Drawing.SystemColors.Info;
		this.tsbDataIni.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.tsbDataIni.Name = "tsbDataIni";
		this.tsbDataIni.Size = new System.Drawing.Size(78, 23);
		this.tsbDataIni.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tsbDataIni.Leave += new System.EventHandler(tsbDataIni_Leave);
		this.tsbDataIni.KeyDown += new System.Windows.Forms.KeyEventHandler(tsbDataIni_KeyDown);
		this.toolStripLabel2.AutoSize = false;
		this.toolStripLabel2.Name = "toolStripLabel2";
		this.toolStripLabel2.Size = new System.Drawing.Size(25, 23);
		this.toolStripLabel2.Text = "até";
		this.tsbDataFim.AutoSize = false;
		this.tsbDataFim.BackColor = System.Drawing.SystemColors.Info;
		this.tsbDataFim.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.tsbDataFim.Name = "tsbDataFim";
		this.tsbDataFim.Size = new System.Drawing.Size(78, 23);
		this.tsbDataFim.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tsbDataFim.Leave += new System.EventHandler(tsbDataFim_Leave);
		this.tsbDataFim.KeyDown += new System.Windows.Forms.KeyEventHandler(tsbDataFim_KeyDown);
		this.lbPageSizeSep.Name = "lbPageSizeSep";
		this.lbPageSizeSep.Size = new System.Drawing.Size(6, 23);
		this.lbPageSizeSep.Visible = false;
		this.lbPageSize.AutoSize = false;
		this.lbPageSize.Name = "lbPageSize";
		this.lbPageSize.Size = new System.Drawing.Size(48, 23);
		this.lbPageSize.Text = "Docs:";
		this.lbPageSize.ToolTipText = "Define o total de registros que serão apresentados na tela";
		this.lbPageSize.Visible = false;
		this.tsbPageSize.AutoSize = false;
		this.tsbPageSize.BackColor = System.Drawing.SystemColors.Info;
		this.tsbPageSize.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.tsbPageSize.MaxLength = 999999999;
		this.tsbPageSize.Name = "tsbPageSize";
		this.tsbPageSize.Size = new System.Drawing.Size(40, 23);
		this.tsbPageSize.Tag = "";
		this.tsbPageSize.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tsbPageSize.ToolTipText = "Define o total de registros que serão apresentados na tela";
		this.tsbPageSize.Visible = false;
		this.tsbPageSize.Leave += new System.EventHandler(tsbPageSize_Leave);
		this.tsbPageSize.KeyDown += new System.Windows.Forms.KeyEventHandler(tsbPageSize_KeyDown);
		this.toolStripSeparator37.Name = "toolStripSeparator37";
		this.toolStripSeparator37.Size = new System.Drawing.Size(6, 23);
		this.tsbAtualizar01.AutoSize = false;
		this.tsbAtualizar01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbAtualizar01.Image = Monitor.Resources.image_refresh;
		this.tsbAtualizar01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbAtualizar01.Name = "tsbAtualizar01";
		this.tsbAtualizar01.Size = new System.Drawing.Size(22, 22);
		this.tsbAtualizar01.Text = "Atualizar";
		this.tsbAtualizar01.ToolTipText = "Atualizar lista de documentos na tela";
		this.tsbAtualizar01.Click += new System.EventHandler(tsbAtualizar01_Click);
		this.toolStripSeparator12.Name = "toolStripSeparator12";
		this.toolStripSeparator12.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator12.Tag = "#NOT-TABPARTNER";
		this.tsbDataColapse.AutoSize = false;
		this.tsbDataColapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbDataColapse.Image = Monitor.Resources.image_colapse;
		this.tsbDataColapse.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDataColapse.Name = "tsbDataColapse";
		this.tsbDataColapse.Size = new System.Drawing.Size(22, 22);
		this.tsbDataColapse.Text = "Compactar / Expandir agrupamentos";
		this.tsbDataColapse.Click += new System.EventHandler(tsbDataColapse_Click);
		this.toolStripSeparator40.Name = "toolStripSeparator40";
		this.toolStripSeparator40.Size = new System.Drawing.Size(6, 23);
		this.tsbFullScreen.AutoSize = false;
		this.tsbFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbFullScreen.Image = Monitor.Resources.image_fullscreen;
		this.tsbFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbFullScreen.Name = "tsbFullScreen";
		this.tsbFullScreen.Size = new System.Drawing.Size(22, 22);
		this.tsbFullScreen.Text = "Mostra tela cheia";
		this.tsbFullScreen.Click += new System.EventHandler(tsbFullScreen_Click);
		this.stsSumary.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.stsSumary.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.stsTotalValue, this.stsSumSep01, this.stsTotalQuant, this.stsSumSep02, this.stsPageWarn, this.stsSumSep03, this.stsWhatNew, this.stsSumSep04, this.stsHelpCenter });
		this.stsSumary.Location = new System.Drawing.Point(0, 628);
		this.stsSumary.Name = "stsSumary";
		this.stsSumary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		this.stsSumary.Size = new System.Drawing.Size(742, 25);
		this.stsSumary.TabIndex = 13;
		this.stsSumary.Text = "statusStrip1";
		this.stsSumary.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(stsSumary_ItemClicked);
		this.stsTotalValue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.stsTotalValue.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
		this.stsTotalValue.Name = "stsTotalValue";
		this.stsTotalValue.Size = new System.Drawing.Size(108, 20);
		this.stsTotalValue.Text = "Valor Total : R$ 0,00";
		this.stsSumSep01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.stsSumSep01.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
		this.stsSumSep01.Name = "stsSumSep01";
		this.stsSumSep01.Size = new System.Drawing.Size(10, 20);
		this.stsSumSep01.Text = "|";
		this.stsTotalQuant.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.stsTotalQuant.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
		this.stsTotalQuant.Name = "stsTotalQuant";
		this.stsTotalQuant.Size = new System.Drawing.Size(90, 20);
		this.stsTotalQuant.Text = "Documentos : 0";
		this.stsSumSep02.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.stsSumSep02.Name = "stsSumSep02";
		this.stsSumSep02.Size = new System.Drawing.Size(10, 20);
		this.stsSumSep02.Text = "|";
		this.stsPageWarn.BackColor = System.Drawing.SystemColors.Info;
		this.stsPageWarn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
		this.stsPageWarn.Image = Monitor.Resources.image_warning;
		this.stsPageWarn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.stsPageWarn.Name = "stsPageWarn";
		this.stsPageWarn.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
		this.stsPageWarn.Size = new System.Drawing.Size(236, 24);
		this.stsPageWarn.Text = " Primeiros {varPageSize} documentos ";
		this.stsPageWarn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.stsPageWarn.Visible = false;
		this.stsSumSep03.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.stsSumSep03.Name = "stsSumSep03";
		this.stsSumSep03.Size = new System.Drawing.Size(10, 20);
		this.stsSumSep03.Text = "|";
		this.stsSumSep03.Visible = false;
		this.stsWhatNew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.stsWhatNew.Image = Monitor.Resources.image_megafone;
		this.stsWhatNew.IsLink = true;
		this.stsWhatNew.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
		this.stsWhatNew.LinkVisited = true;
		this.stsWhatNew.Name = "stsWhatNew";
		this.stsWhatNew.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.stsWhatNew.Size = new System.Drawing.Size(129, 20);
		this.stsWhatNew.Text = "O que há de novo?";
		this.stsWhatNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.stsSumSep04.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.stsSumSep04.Name = "stsSumSep04";
		this.stsSumSep04.Size = new System.Drawing.Size(10, 20);
		this.stsSumSep04.Text = "|";
		this.stsHelpCenter.Image = Monitor.Resources.image_premium;
		this.stsHelpCenter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.stsHelpCenter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.stsHelpCenter.IsLink = true;
		this.stsHelpCenter.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
		this.stsHelpCenter.LinkVisited = true;
		this.stsHelpCenter.Name = "stsHelpCenter";
		this.stsHelpCenter.Size = new System.Drawing.Size(352, 20);
		this.stsHelpCenter.Spring = true;
		this.stsHelpCenter.Text = "Consulte a Central de Ajuda";
		this.stsHelpCenter.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.splitMonFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitMonFeatures.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitMonFeatures.IsSplitterFixed = true;
		this.splitMonFeatures.Location = new System.Drawing.Point(0, 31);
		this.splitMonFeatures.Name = "splitMonFeatures";
		this.splitMonFeatures.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitMonFeatures.Panel1.Controls.Add(this.trvFeatures);
		this.splitMonFeatures.Panel1MinSize = 150;
		this.splitMonFeatures.Panel2.Controls.Add(this.pnUserData);
		this.splitMonFeatures.Panel2MinSize = 168;
		this.splitMonFeatures.Size = new System.Drawing.Size(235, 622);
		this.splitMonFeatures.SplitterDistance = 452;
		this.splitMonFeatures.SplitterWidth = 2;
		this.splitMonFeatures.TabIndex = 8;
		this.trvFeatures.CheckBoxes = true;
		this.trvFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
		this.trvFeatures.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.trvFeatures.FullRowSelect = true;
		this.trvFeatures.HotTracking = true;
		this.trvFeatures.ImageIndex = 0;
		this.trvFeatures.ImageList = this.ImgLibrary;
		this.trvFeatures.Location = new System.Drawing.Point(0, 0);
		this.trvFeatures.Name = "trvFeatures";
		this.trvFeatures.SelectedImageIndex = 0;
		this.trvFeatures.Size = new System.Drawing.Size(235, 452);
		this.trvFeatures.TabIndex = 7;
		this.trvFeatures.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(trvFeatures_BeforeCheck);
		this.trvFeatures.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(trvFeatures_AfterCheck);
		this.trvFeatures.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(trvFeatures_NodeMouseClick);
		this.pnUserData.BackColor = System.Drawing.Color.White;
		this.pnUserData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnUserData.Controls.Add(this.lkbBalance);
		this.pnUserData.Controls.Add(this.label4);
		this.pnUserData.Controls.Add(this.picUserData);
		this.pnUserData.Controls.Add(this.lbUserData);
		this.pnUserData.Controls.Add(this.label2);
		this.pnUserData.Controls.Add(this.lkbSupportChat);
		this.pnUserData.Controls.Add(this.picSupportChat);
		this.pnUserData.Controls.Add(this.label3);
		this.pnUserData.Controls.Add(this.lbSalesText01);
		this.pnUserData.Controls.Add(this.lbLicenseManager);
		this.pnUserData.Controls.Add(this.btnSignature);
		this.pnUserData.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnUserData.Location = new System.Drawing.Point(0, 0);
		this.pnUserData.Name = "pnUserData";
		this.pnUserData.Size = new System.Drawing.Size(235, 168);
		this.pnUserData.TabIndex = 7;
		this.lkbBalance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lkbBalance.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lkbBalance.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lkbBalance.Location = new System.Drawing.Point(2, 94);
		this.lkbBalance.Name = "lkbBalance";
		this.lkbBalance.Size = new System.Drawing.Size(238, 17);
		this.lkbBalance.TabIndex = 166;
		this.lkbBalance.TabStop = true;
		this.lkbBalance.Text = "Créditos Especiais : R$ 100000";
		this.lkbBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbBalance.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbBalance_LinkClicked);
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label4.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label4.Location = new System.Drawing.Point(-1, 89);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(244, 1);
		this.label4.TabIndex = 165;
		this.picUserData.Image = Monitor.Resources.image_userdata;
		this.picUserData.Location = new System.Drawing.Point(5, 4);
		this.picUserData.Name = "picUserData";
		this.picUserData.Size = new System.Drawing.Size(19, 19);
		this.picUserData.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picUserData.TabIndex = 164;
		this.picUserData.TabStop = false;
		this.picUserData.Click += new System.EventHandler(picUserData_Click);
		this.lbUserData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbUserData.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lbUserData.Location = new System.Drawing.Point(28, 3);
		this.lbUserData.Name = "lbUserData";
		this.lbUserData.Size = new System.Drawing.Size(209, 18);
		this.lbUserData.TabIndex = 163;
		this.lbUserData.TabStop = true;
		this.lbUserData.Text = "[ Dados de usuário e créditos ]";
		this.lbUserData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbUserData.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbUserData_LinkClicked);
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(-1, 28);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(244, 2);
		this.label2.TabIndex = 162;
		this.lkbSupportChat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lkbSupportChat.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lkbSupportChat.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lkbSupportChat.Location = new System.Drawing.Point(54, 122);
		this.lkbSupportChat.Name = "lkbSupportChat";
		this.lkbSupportChat.Size = new System.Drawing.Size(183, 40);
		this.lkbSupportChat.TabIndex = 1;
		this.lkbSupportChat.TabStop = true;
		this.lkbSupportChat.Text = "Fale com o suporte \r\ntécnico no chat";
		this.lkbSupportChat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbSupportChat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbSupportChat_LinkClicked);
		this.picSupportChat.Image = Monitor.Resources.image_support;
		this.picSupportChat.Location = new System.Drawing.Point(5, 122);
		this.picSupportChat.Name = "picSupportChat";
		this.picSupportChat.Size = new System.Drawing.Size(41, 40);
		this.picSupportChat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picSupportChat.TabIndex = 0;
		this.picSupportChat.TabStop = false;
		this.picSupportChat.Click += new System.EventHandler(picSupportChat_Click);
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(-2, 116);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(244, 2);
		this.label3.TabIndex = 161;
		this.lbSalesText01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbSalesText01.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbSalesText01.ForeColor = System.Drawing.Color.Blue;
		this.lbSalesText01.Location = new System.Drawing.Point(3, 37);
		this.lbSalesText01.Name = "lbSalesText01";
		this.lbSalesText01.Size = new System.Drawing.Size(235, 18);
		this.lbSalesText01.TabIndex = 5;
		this.lbSalesText01.Text = "Plano assinado : Ouro";
		this.lbSalesText01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbLicenseManager.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbLicenseManager.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.lbLicenseManager.Location = new System.Drawing.Point(3, 60);
		this.lbLicenseManager.Name = "lbLicenseManager";
		this.lbLicenseManager.Size = new System.Drawing.Size(236, 17);
		this.lbLicenseManager.TabIndex = 7;
		this.lbLicenseManager.TabStop = true;
		this.lbLicenseManager.Text = "[ Gerenciar assinatura ]";
		this.lbLicenseManager.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbLicenseManager.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbLicenseManager_LinkClicked);
		this.btnSignature.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.btnSignature.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btnSignature.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btnSignature.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSignature.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSignature.ForeColor = System.Drawing.SystemColors.Window;
		this.btnSignature.Location = new System.Drawing.Point(62, 47);
		this.btnSignature.Name = "btnSignature";
		this.btnSignature.Size = new System.Drawing.Size(126, 24);
		this.btnSignature.TabIndex = 1;
		this.btnSignature.Text = "Assinar plano";
		this.btnSignature.UseVisualStyleBackColor = false;
		this.btnSignature.Click += new System.EventHandler(btnSignature_Click);
		this.toolbarFeatures.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.toolbarFeatures.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolbarFeatures.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolbarFeatures.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsbSupport, this.toolStripSeparator2, this.toolStripBtnFilterClear, this.toolStripSeparator7, this.tsbSyncronize, this.toolStripSeparator10 });
		this.toolbarFeatures.Location = new System.Drawing.Point(0, 0);
		this.toolbarFeatures.Name = "toolbarFeatures";
		this.toolbarFeatures.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
		this.toolbarFeatures.Size = new System.Drawing.Size(235, 31);
		this.toolbarFeatures.Stretch = true;
		this.toolbarFeatures.TabIndex = 3;
		this.tsbSupport.Image = Monitor.Resources.image_support;
		this.tsbSupport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSupport.Name = "tsbSupport";
		this.tsbSupport.Size = new System.Drawing.Size(76, 24);
		this.tsbSupport.Text = "Suporte";
		this.tsbSupport.Click += new System.EventHandler(tsbSupport_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.toolStripBtnFilterClear.BackColor = System.Drawing.Color.Transparent;
		this.toolStripBtnFilterClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripBtnFilterClear.Image = Monitor.Resources.image_filter_clear;
		this.toolStripBtnFilterClear.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripBtnFilterClear.Name = "toolStripBtnFilterClear";
		this.toolStripBtnFilterClear.Size = new System.Drawing.Size(24, 24);
		this.toolStripBtnFilterClear.ToolTipText = "Limpar Filtros";
		this.toolStripBtnFilterClear.Click += new System.EventHandler(toolStripBtnFilterClear_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
		this.tsbSyncronize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbSyncronize.Image = Monitor.Resources.image_sync_license;
		this.tsbSyncronize.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSyncronize.Name = "tsbSyncronize";
		this.tsbSyncronize.Size = new System.Drawing.Size(24, 24);
		this.tsbSyncronize.Text = "Sincronizar licença de uso";
		this.tsbSyncronize.Click += new System.EventHandler(tsbSyncronize_Click);
		this.toolStripSeparator10.Name = "toolStripSeparator10";
		this.toolStripSeparator10.Size = new System.Drawing.Size(6, 27);
		this.contextSortFilial.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.contextSortFilial.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextSortFilial.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmOrderByDesct, this.tsmOrderByIdent });
		this.contextSortFilial.Name = "contextMenuFilial";
		this.contextSortFilial.Size = new System.Drawing.Size(209, 56);
		this.tsmOrderByDesct.Image = Monitor.Resources.image_sorter;
		this.tsmOrderByDesct.Name = "tsmOrderByDesct";
		this.tsmOrderByDesct.Size = new System.Drawing.Size(208, 26);
		this.tsmOrderByDesct.Text = "Ordenar por Nome";
		this.tsmOrderByDesct.Click += new System.EventHandler(tsmOrderByDesct_Click);
		this.tsmOrderByIdent.Image = Monitor.Resources.image_sorter;
		this.tsmOrderByIdent.Name = "tsmOrderByIdent";
		this.tsmOrderByIdent.Size = new System.Drawing.Size(208, 26);
		this.tsmOrderByIdent.Text = "Ordenar por CNPJ/CPF";
		this.tsmOrderByIdent.Click += new System.EventHandler(tsmOrderByIdent_Click);
		this.toolbarDocs01.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.toolbarDocs01.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolbarDocs01.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolbarDocs01.Items.AddRange(new System.Windows.Forms.ToolStripItem[32]
		{
			this.tsbConfiguration, this.toolStripSeparator41, this.tsbBuscarDocs, this.tsbBuscarDocsAll, this.toolStripSeparator29, this.tsmDownDFeMenu, this.toolStripSeparator24, this.tsbEventosNFe, this.toolStripSeparator16, this.tsbDFeConsStatus,
			this.toolStripSeparator22, this.tsbTags, this.SepAuditor, this.tsbAuditor, this.toolStripSeparator18, this.tsbHandlerEdiFile, this.toolStripSeparator11, this.tsbUploadXml, this.toolStripSeparator23, this.tsbExport,
			this.toolStripSeparator39, this.tsbExportar, this.toolStripSeparator28, this.tsbImprimir, this.toolStripSeparator34, this.tsbSendDoc, this.toolStripSeparator32, this.tsbIntegrations, this.toolStripSeparator38, this.tsbMonTools,
			this.toolStripSeparator21, this.tsbPlugins
		});
		this.toolbarDocs01.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
		this.toolbarDocs01.Location = new System.Drawing.Point(0, 0);
		this.toolbarDocs01.Name = "toolbarDocs01";
		this.toolbarDocs01.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
		this.toolbarDocs01.Size = new System.Drawing.Size(1225, 58);
		this.toolbarDocs01.Stretch = true;
		this.toolbarDocs01.TabIndex = 3;
		this.tsbConfiguration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbConfiguration.Image = (System.Drawing.Image)resources.GetObject("tsbConfiguration.Image");
		this.tsbConfiguration.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbConfiguration.Name = "tsbConfiguration";
		this.tsbConfiguration.Size = new System.Drawing.Size(24, 24);
		this.tsbConfiguration.Text = "Configurações";
		this.tsbConfiguration.ToolTipText = "Configurações  e Proxy de Internet";
		this.tsbConfiguration.Click += new System.EventHandler(tsbConfiguration_Click);
		this.toolStripSeparator41.Name = "toolStripSeparator41";
		this.toolStripSeparator41.Size = new System.Drawing.Size(6, 23);
		this.tsbBuscarDocs.Image = (System.Drawing.Image)resources.GetObject("tsbBuscarDocs.Image");
		this.tsbBuscarDocs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbBuscarDocs.Name = "tsbBuscarDocs";
		this.tsbBuscarDocs.Size = new System.Drawing.Size(128, 24);
		this.tsbBuscarDocs.Text = "Buscar na SEFAZ";
		this.tsbBuscarDocs.ToolTipText = "Buscar Documentos na SEFAZ para empresa(s) selecionada(s)";
		this.tsbBuscarDocs.Click += new System.EventHandler(tsbBuscarDocs_Click);
		this.tsbBuscarDocsAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbBuscarDocsAll.Image = Monitor.Resources.image_scan_all;
		this.tsbBuscarDocsAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbBuscarDocsAll.Name = "tsbBuscarDocsAll";
		this.tsbBuscarDocsAll.Size = new System.Drawing.Size(24, 24);
		this.tsbBuscarDocsAll.Text = "Buscar na SEFAZ para todas as empresas";
		this.tsbBuscarDocsAll.Click += new System.EventHandler(tsbBuscarDocsAll_Click);
		this.toolStripSeparator29.Name = "toolStripSeparator29";
		this.toolStripSeparator29.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator29.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP#NOT-TABEXPDUECONTROL";
		this.tsmDownDFeMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsbDowDFeStart, this.toolStripSeparator42, this.tsbDowDFeBatch_Click });
		this.tsmDownDFeMenu.Image = Monitor.Resources.image_xml_blue;
		this.tsmDownDFeMenu.Name = "tsmDownDFeMenu";
		this.tsmDownDFeMenu.Size = new System.Drawing.Size(141, 24);
		this.tsmDownDFeMenu.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsmDownDFeMenu.Text = "Download de XML";
		this.tsbDowDFeStart.Image = Monitor.Resources.image_download_cloud;
		this.tsbDowDFeStart.Name = "tsbDowDFeStart";
		this.tsbDowDFeStart.Size = new System.Drawing.Size(219, 26);
		this.tsbDowDFeStart.Text = "Iniciar Download de XML";
		this.tsbDowDFeStart.Click += new System.EventHandler(tsbDowDFeStart_Click);
		this.toolStripSeparator42.Name = "toolStripSeparator42";
		this.toolStripSeparator42.Size = new System.Drawing.Size(216, 6);
		this.tsbDowDFeBatch_Click.Image = Monitor.Resources.image_schedule;
		this.tsbDowDFeBatch_Click.Name = "tsbDowDFeBatch_Click";
		this.tsbDowDFeBatch_Click.Size = new System.Drawing.Size(219, 26);
		this.tsbDowDFeBatch_Click.Text = "Lotes de Processamento";
		this.tsbDowDFeBatch_Click.Click += new System.EventHandler(tsbDowDFeBatch_Click_Click);
		this.toolStripSeparator24.Name = "toolStripSeparator24";
		this.toolStripSeparator24.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator24.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbEventosNFe.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.tsbEventoCTeDesacordo, this.tsbEventoCTeDesacordoCanc, this.toolStripSeparator01, this.tsbEventosNFeConfirmacao, this.tsbEventosNFeNaoRealizada, this.tsbEventosNFeDesconhecida, this.tsbEventosNFeCiencia, this.toolStripSeparator02, this.tsbEventosNFSeConfirmacao, this.tsbEventosNFSeRecusa,
			this.toolStripSeparator20, this.tsbEventsDFeHelp
		});
		this.tsbEventosNFe.ForeColor = System.Drawing.SystemColors.ControlText;
		this.tsbEventosNFe.Image = (System.Drawing.Image)resources.GetObject("tsbEventosNFe.Image");
		this.tsbEventosNFe.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbEventosNFe.Name = "tsbEventosNFe";
		this.tsbEventosNFe.Size = new System.Drawing.Size(99, 24);
		this.tsbEventosNFe.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbEventosNFe.Text = "Manifestar";
		this.tsbEventoCTeDesacordo.Image = Monitor.Resources.dfe_disagree;
		this.tsbEventoCTeDesacordo.Name = "tsbEventoCTeDesacordo";
		this.tsbEventoCTeDesacordo.Size = new System.Drawing.Size(292, 26);
		this.tsbEventoCTeDesacordo.Text = "CTe : Desacordo do serviço";
		this.tsbEventoCTeDesacordo.Click += new System.EventHandler(tsbEventoCTeDesacordo_Click);
		this.tsbEventoCTeDesacordoCanc.Image = Monitor.Resources.dfe_canceled;
		this.tsbEventoCTeDesacordoCanc.Name = "tsbEventoCTeDesacordoCanc";
		this.tsbEventoCTeDesacordoCanc.Size = new System.Drawing.Size(292, 26);
		this.tsbEventoCTeDesacordoCanc.Text = "CTe : Cancelar Desacordo do serviço";
		this.tsbEventoCTeDesacordoCanc.Click += new System.EventHandler(tsbEventoCTeDesacordoCanc_Click);
		this.toolStripSeparator01.Name = "toolStripSeparator01";
		this.toolStripSeparator01.Size = new System.Drawing.Size(289, 6);
		this.tsbEventosNFeConfirmacao.Image = Monitor.Resources.dfe_confirm;
		this.tsbEventosNFeConfirmacao.Name = "tsbEventosNFeConfirmacao";
		this.tsbEventosNFeConfirmacao.Size = new System.Drawing.Size(292, 26);
		this.tsbEventosNFeConfirmacao.Text = "NFe : Confirmação da operação";
		this.tsbEventosNFeConfirmacao.Click += new System.EventHandler(tsbEventosNFeConfirmacao_Click);
		this.tsbEventosNFeNaoRealizada.Image = Monitor.Resources.dfe_disagree;
		this.tsbEventosNFeNaoRealizada.Name = "tsbEventosNFeNaoRealizada";
		this.tsbEventosNFeNaoRealizada.Size = new System.Drawing.Size(292, 26);
		this.tsbEventosNFeNaoRealizada.Text = "NFe : Operação não realizada";
		this.tsbEventosNFeNaoRealizada.Click += new System.EventHandler(tsbEventosNFeNaoRealizada_Click);
		this.tsbEventosNFeDesconhecida.Image = Monitor.Resources.dfe_unknow;
		this.tsbEventosNFeDesconhecida.Name = "tsbEventosNFeDesconhecida";
		this.tsbEventosNFeDesconhecida.Size = new System.Drawing.Size(292, 26);
		this.tsbEventosNFeDesconhecida.Text = "NFe : Operação desconhecida";
		this.tsbEventosNFeDesconhecida.Click += new System.EventHandler(tsbEventosNFeDesconhecida_Click);
		this.tsbEventosNFeCiencia.Image = Monitor.Resources.dfe_acknow;
		this.tsbEventosNFeCiencia.Name = "tsbEventosNFeCiencia";
		this.tsbEventosNFeCiencia.Size = new System.Drawing.Size(292, 26);
		this.tsbEventosNFeCiencia.Text = "NFe : Ciência da operação";
		this.tsbEventosNFeCiencia.Click += new System.EventHandler(tsbEventosNFeCiencia_Click);
		this.toolStripSeparator02.Name = "toolStripSeparator02";
		this.toolStripSeparator02.Size = new System.Drawing.Size(289, 6);
		this.tsbEventosNFSeConfirmacao.Image = Monitor.Resources.dfe_confirm;
		this.tsbEventosNFSeConfirmacao.Name = "tsbEventosNFSeConfirmacao";
		this.tsbEventosNFSeConfirmacao.Size = new System.Drawing.Size(292, 26);
		this.tsbEventosNFSeConfirmacao.Text = "NFSe : Confirmação do Tomador";
		this.tsbEventosNFSeConfirmacao.Click += new System.EventHandler(tsbEventosNFSeConfirmacao_Click);
		this.tsbEventosNFSeRecusa.Image = Monitor.Resources.dfe_disagree;
		this.tsbEventosNFSeRecusa.Name = "tsbEventosNFSeRecusa";
		this.tsbEventosNFSeRecusa.Size = new System.Drawing.Size(292, 26);
		this.tsbEventosNFSeRecusa.Text = "NFSe : Recusa do Tomador";
		this.tsbEventosNFSeRecusa.Click += new System.EventHandler(tsbEventosNFSeRecusa_Click);
		this.toolStripSeparator20.Name = "toolStripSeparator20";
		this.toolStripSeparator20.Size = new System.Drawing.Size(289, 6);
		this.tsbEventsDFeHelp.Image = Monitor.Resources.image_help;
		this.tsbEventsDFeHelp.Name = "tsbEventsDFeHelp";
		this.tsbEventsDFeHelp.Size = new System.Drawing.Size(292, 26);
		this.tsbEventsDFeHelp.Text = "Ajuda";
		this.tsbEventsDFeHelp.Click += new System.EventHandler(tsbEventsDFeHelp_Click);
		this.toolStripSeparator16.Name = "toolStripSeparator16";
		this.toolStripSeparator16.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator16.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbDFeConsStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsbConsStatusDFeSimple, this.tsbConsStatusDFeFull, this.tssSepCons01, this.tsbConsExtSyst, this.tssSepCons02, this.tsbConsStatusDFeHelp });
		this.tsbDFeConsStatus.Image = Monitor.Resources.image_confirmation;
		this.tsbDFeConsStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDFeConsStatus.Name = "tsbDFeConsStatus";
		this.tsbDFeConsStatus.Size = new System.Drawing.Size(130, 24);
		this.tsbDFeConsStatus.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbDFeConsStatus.Text = "Consulta Status";
		this.tsbDFeConsStatus.ToolTipText = "Consulta status dos documentos na SEAZ";
		this.tsbConsStatusDFeSimple.Image = Monitor.Resources.dfe_approved;
		this.tsbConsStatusDFeSimple.Name = "tsbConsStatusDFeSimple";
		this.tsbConsStatusDFeSimple.Size = new System.Drawing.Size(247, 26);
		this.tsbConsStatusDFeSimple.Text = "[Sefaz] Consulta Simples";
		this.tsbConsStatusDFeSimple.ToolTipText = "Consulta o status de documentos emitidos nos últimos 6 meses";
		this.tsbConsStatusDFeSimple.Click += new System.EventHandler(tsbConsStatusDFeSimple_Click);
		this.tsbConsStatusDFeFull.Image = (System.Drawing.Image)resources.GetObject("tsbConsStatusDFeFull.Image");
		this.tsbConsStatusDFeFull.Name = "tsbConsStatusDFeFull";
		this.tsbConsStatusDFeFull.Size = new System.Drawing.Size(247, 26);
		this.tsbConsStatusDFeFull.Text = "[Sefaz] Consulta Completa";
		this.tsbConsStatusDFeFull.ToolTipText = "Consulta o status de documentos emitidos no passado [Anterior a 6 meses]";
		this.tsbConsStatusDFeFull.Click += new System.EventHandler(tsbConsStatusDFeFull_Click);
		this.tssSepCons01.Name = "tssSepCons01";
		this.tssSepCons01.Size = new System.Drawing.Size(244, 6);
		this.tsbConsExtSyst.Image = Monitor.Resources.image_lancfiscal;
		this.tsbConsExtSyst.Name = "tsbConsExtSyst";
		this.tsbConsExtSyst.Size = new System.Drawing.Size(247, 26);
		this.tsbConsExtSyst.Text = "[Fiscal] Consulta Escrituração";
		this.tsbConsExtSyst.ToolTipText = "Consulta se o documento já foi escriturado no Sistema Fiscal";
		this.tsbConsExtSyst.Click += new System.EventHandler(tsbConsExtSyst_Click);
		this.tssSepCons02.Name = "tssSepCons02";
		this.tssSepCons02.Size = new System.Drawing.Size(244, 6);
		this.tsbConsStatusDFeHelp.Image = Monitor.Resources.image_help;
		this.tsbConsStatusDFeHelp.Name = "tsbConsStatusDFeHelp";
		this.tsbConsStatusDFeHelp.Size = new System.Drawing.Size(247, 26);
		this.tsbConsStatusDFeHelp.Text = "Ajuda";
		this.tsbConsStatusDFeHelp.Click += new System.EventHandler(tsbConsStatusDFeHelp_Click);
		this.toolStripSeparator22.Name = "toolStripSeparator22";
		this.toolStripSeparator22.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator22.Tag = "#NOT-TABPARTNER#NOT-TABAUDITOR-#NOT-TABDOCJUMP#NOT-TABEXPDUECONTROL";
		this.tsbTags.Image = Monitor.Resources.image_tag;
		this.tsbTags.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbTags.Name = "tsbTags";
		this.tsbTags.Size = new System.Drawing.Size(86, 24);
		this.tsbTags.Tag = "#NOT-TABPARTNER#NOT-TABAUDITOR-#NOT-TABDOCJUMP#NOT-TABEXPDUECONTROL";
		this.tsbTags.Text = "Etiqueta";
		this.tsbTags.ToolTipText = "Classifique seus documentos";
		this.SepAuditor.Name = "SepAuditor";
		this.SepAuditor.Size = new System.Drawing.Size(6, 23);
		this.SepAuditor.Tag = "";
		this.tsbAuditor.Image = Monitor.Resources.image_audit;
		this.tsbAuditor.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbAuditor.Name = "tsbAuditor";
		this.tsbAuditor.Size = new System.Drawing.Size(72, 24);
		this.tsbAuditor.Text = "Auditor";
		this.tsbAuditor.Click += new System.EventHandler(tsbAuditor_Click);
		this.toolStripSeparator18.Name = "toolStripSeparator18";
		this.toolStripSeparator18.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator18.Tag = "#NOT-TABPARTNER#NOT-TABAUDITOR-#NOT-TABDOCJUMP";
		this.tsbHandlerEdiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsbViewEdiAll, this.toolStripSeparator31, this.tsbBaixarNotFis, this.tsbBaixarConemb, this.toolStripSeparator30, this.tsbEdiProcedaHelp });
		this.tsbHandlerEdiFile.Image = Monitor.Resources.image_ediproceda;
		this.tsbHandlerEdiFile.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbHandlerEdiFile.Name = "tsbHandlerEdiFile";
		this.tsbHandlerEdiFile.Size = new System.Drawing.Size(61, 24);
		this.tsbHandlerEdiFile.Tag = "#NOT-TABPARTNER#NOT-TABAUDITOR-#NOT-TABDOCJUMP";
		this.tsbHandlerEdiFile.Text = "EDI";
		this.tsbViewEdiAll.Name = "tsbViewEdiAll";
		this.tsbViewEdiAll.Size = new System.Drawing.Size(282, 26);
		this.tsbViewEdiAll.Text = "Visualizar";
		this.tsbViewEdiAll.Click += new System.EventHandler(tsbViewEdiAll_Click);
		this.toolStripSeparator31.Name = "toolStripSeparator31";
		this.toolStripSeparator31.Size = new System.Drawing.Size(279, 6);
		this.tsbBaixarNotFis.Name = "tsbBaixarNotFis";
		this.tsbBaixarNotFis.Size = new System.Drawing.Size(282, 26);
		this.tsbBaixarNotFis.Text = "Exportar NOTFIS : Notas Fiscais";
		this.tsbBaixarNotFis.Click += new System.EventHandler(tsbBaixarNotFis_Click);
		this.tsbBaixarConemb.Name = "tsbBaixarConemb";
		this.tsbBaixarConemb.Size = new System.Drawing.Size(282, 26);
		this.tsbBaixarConemb.Text = "Exportar CONEMB : Conhecimentos";
		this.tsbBaixarConemb.Click += new System.EventHandler(tsbBaixarConemb_Click);
		this.toolStripSeparator30.Name = "toolStripSeparator30";
		this.toolStripSeparator30.Size = new System.Drawing.Size(279, 6);
		this.tsbEdiProcedaHelp.Image = Monitor.Resources.image_help;
		this.tsbEdiProcedaHelp.Name = "tsbEdiProcedaHelp";
		this.tsbEdiProcedaHelp.Size = new System.Drawing.Size(282, 26);
		this.tsbEdiProcedaHelp.Text = "Ajuda";
		this.tsbEdiProcedaHelp.Click += new System.EventHandler(tsbEdiProcedaHelp_Click);
		this.toolStripSeparator11.Name = "toolStripSeparator11";
		this.toolStripSeparator11.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator11.Tag = "";
		this.tsbUploadXml.Image = Monitor.Resources.image_import;
		this.tsbUploadXml.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbUploadXml.Name = "tsbUploadXml";
		this.tsbUploadXml.Size = new System.Drawing.Size(82, 24);
		this.tsbUploadXml.Tag = "";
		this.tsbUploadXml.Text = "Importar";
		this.tsbUploadXml.ToolTipText = "Importação de documentos em massa.";
		this.tsbUploadXml.Click += new System.EventHandler(tsbUploadXml_Click);
		this.toolStripSeparator23.Name = "toolStripSeparator23";
		this.toolStripSeparator23.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator23.Tag = "#NOT-TABPARTNER#NOT-TABAUDITOR-#NOT-TABDOCJUMP";
		this.tsbExport.Image = Monitor.Resources.image_export;
		this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExport.Name = "tsbExport";
		this.tsbExport.Size = new System.Drawing.Size(80, 24);
		this.tsbExport.Tag = "#NOT-TABPARTNER#NOT-TABAUDITOR-#NOT-TABDOCJUMP";
		this.tsbExport.Text = "Exportar";
		this.tsbExport.ToolTipText = "Exportar em massa de XMLs e/ou PDFs";
		this.tsbExport.Click += new System.EventHandler(tsbExport_Click);
		this.toolStripSeparator39.Name = "toolStripSeparator39";
		this.toolStripSeparator39.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator39.Tag = "";
		this.tsbExportar.Image = (System.Drawing.Image)resources.GetObject("tsbExportar.Image");
		this.tsbExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExportar.Name = "tsbExportar";
		this.tsbExportar.Size = new System.Drawing.Size(24, 24);
		this.tsbExportar.ToolTipText = "Exportar lista de documentos para o Excel";
		this.tsbExportar.Click += new System.EventHandler(tsbExportar_Click);
		this.toolStripSeparator28.Name = "toolStripSeparator28";
		this.toolStripSeparator28.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator28.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbImprimir.Image = Monitor.Resources.image_printer;
		this.tsbImprimir.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbImprimir.Name = "tsbImprimir";
		this.tsbImprimir.Size = new System.Drawing.Size(24, 24);
		this.tsbImprimir.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbImprimir.ToolTipText = "Impressão em massa de PDFs";
		this.tsbImprimir.Click += new System.EventHandler(tsbImprimir_Click);
		this.toolStripSeparator34.Name = "toolStripSeparator34";
		this.toolStripSeparator34.Size = new System.Drawing.Size(6, 23);
		this.toolStripSeparator34.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbSendDoc.Image = Monitor.Resources.image_email;
		this.tsbSendDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSendDoc.Name = "tsbSendDoc";
		this.tsbSendDoc.Size = new System.Drawing.Size(67, 24);
		this.tsbSendDoc.Tag = "#NOT-TABPARTNER-#NOT-TABDOCJUMP";
		this.tsbSendDoc.Text = "Enviar";
		this.tsbSendDoc.ToolTipText = "Enviar documentos por e-mail";
		this.tsbSendDoc.Click += new System.EventHandler(tsbSendDoc_Click);
		this.toolStripSeparator32.Name = "toolStripSeparator32";
		this.toolStripSeparator32.Size = new System.Drawing.Size(6, 23);
		this.tsbIntegrations.Image = (System.Drawing.Image)resources.GetObject("tsbIntegrations.Image");
		this.tsbIntegrations.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbIntegrations.Name = "tsbIntegrations";
		this.tsbIntegrations.Size = new System.Drawing.Size(87, 24);
		this.tsbIntegrations.Text = "Integrar";
		this.toolStripSeparator38.Name = "toolStripSeparator38";
		this.toolStripSeparator38.Size = new System.Drawing.Size(6, 23);
		this.tsbMonTools.Image = Monitor.Resources.image_mon_tools;
		this.tsbMonTools.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbMonTools.Name = "tsbMonTools";
		this.tsbMonTools.Size = new System.Drawing.Size(101, 24);
		this.tsbMonTools.Text = "Automações";
		this.tsbMonTools.Click += new System.EventHandler(tsbMonTools_Click);
		this.toolStripSeparator21.Name = "toolStripSeparator21";
		this.toolStripSeparator21.Size = new System.Drawing.Size(6, 23);
		this.tsbPlugins.Image = Monitor.Resources.image_plugin;
		this.tsbPlugins.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbPlugins.Name = "tsbPlugins";
		this.tsbPlugins.Size = new System.Drawing.Size(98, 24);
		this.tsbPlugins.Text = "Extensões";
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(257, 6);
		this.toolStripSeparator33.Name = "toolStripSeparator33";
		this.toolStripSeparator33.Size = new System.Drawing.Size(257, 6);
		this.notifyScan.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.notifyScan.ContextMenuStrip = this.contextMenuScan;
		this.notifyScan.Icon = (System.Drawing.Icon)resources.GetObject("notifyScan.Icon");
		this.notifyScan.Text = "Fiscal.io Monitor";
		this.notifyScan.Visible = true;
		this.notifyScan.BalloonTipClicked += new System.EventHandler(notifyScan_BalloonTipClicked);
		this.notifyScan.Click += new System.EventHandler(notifyScan_Click);
		this.notifyScan.DoubleClick += new System.EventHandler(notifyScan_DoubleClick);
		this.timerTaskRunUser.Enabled = true;
		this.timerTaskRunUser.Interval = 60000.0;
		this.timerTaskRunUser.SynchronizingObject = this;
		this.timerTaskRunUser.Elapsed += new System.Timers.ElapsedEventHandler(timerTaskRunUser_Elapsed);
		this.timerCheckScan.Enabled = true;
		this.timerCheckScan.Interval = 60000.0;
		this.timerCheckScan.SynchronizingObject = this;
		this.timerCheckScan.Elapsed += new System.Timers.ElapsedEventHandler(timerCheckScan_Elapsed);
		this.timerTaskRunSyst.Enabled = true;
		this.timerTaskRunSyst.Interval = 60000.0;
		this.timerTaskRunSyst.SynchronizingObject = this;
		this.timerTaskRunSyst.Elapsed += new System.Timers.ElapsedEventHandler(timerTaskRunSyst_Elapsed);
		this.timerBackupAdv.Enabled = true;
		this.timerBackupAdv.Interval = 300000.0;
		this.timerBackupAdv.SynchronizingObject = this;
		this.timerBackupAdv.Elapsed += new System.Timers.ElapsedEventHandler(timerBackupAdv_Elapsed);
		this.toolTipDbaSize.AutomaticDelay = 50000;
		this.toolTipDbaSize.IsBalloon = true;
		this.toolTipDbaSize.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTipDbaSize.ToolTipTitle = "Volume dados";
		this.tsmTimerScan.Name = "tsmTimerScan";
		this.tsmTimerScan.Size = new System.Drawing.Size(202, 22);
		this.tsmTimerScan.Text = "Busca automática";
		this.contextManifest.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.contextManifest.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextManifest.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripSeparator13, this.tsmEventosNFeConfirmacao, this.toolStripSeparator14, this.tsmEventosNFeNaoRealizada, this.tsmEventosNFeDesconhecida, this.toolStripSeparator15, this.tsmEventosNFeCiencia, this.toolStripSeparator8 });
		this.contextManifest.Name = "contextMenuFilial";
		this.contextManifest.Size = new System.Drawing.Size(261, 132);
		this.toolStripSeparator13.Name = "toolStripSeparator13";
		this.toolStripSeparator13.Size = new System.Drawing.Size(257, 6);
		this.tsmEventosNFeConfirmacao.Image = (System.Drawing.Image)resources.GetObject("tsmEventosNFeConfirmacao.Image");
		this.tsmEventosNFeConfirmacao.Name = "tsmEventosNFeConfirmacao";
		this.tsmEventosNFeConfirmacao.Size = new System.Drawing.Size(260, 26);
		this.tsmEventosNFeConfirmacao.Text = "NFe : Confirmação da operação";
		this.tsmEventosNFeConfirmacao.Click += new System.EventHandler(tsbEventosNFeConfirmacao_Click);
		this.toolStripSeparator14.Name = "toolStripSeparator14";
		this.toolStripSeparator14.Size = new System.Drawing.Size(257, 6);
		this.tsmEventosNFeNaoRealizada.Image = (System.Drawing.Image)resources.GetObject("tsmEventosNFeNaoRealizada.Image");
		this.tsmEventosNFeNaoRealizada.Name = "tsmEventosNFeNaoRealizada";
		this.tsmEventosNFeNaoRealizada.Size = new System.Drawing.Size(260, 26);
		this.tsmEventosNFeNaoRealizada.Text = "NFe : Operação não realizada";
		this.tsmEventosNFeNaoRealizada.Click += new System.EventHandler(tsbEventosNFeNaoRealizada_Click);
		this.tsmEventosNFeDesconhecida.Image = (System.Drawing.Image)resources.GetObject("tsmEventosNFeDesconhecida.Image");
		this.tsmEventosNFeDesconhecida.Name = "tsmEventosNFeDesconhecida";
		this.tsmEventosNFeDesconhecida.Size = new System.Drawing.Size(260, 26);
		this.tsmEventosNFeDesconhecida.Text = "NFe : Operação desconhecida";
		this.tsmEventosNFeDesconhecida.Click += new System.EventHandler(tsbEventosNFeDesconhecida_Click);
		this.toolStripSeparator15.Name = "toolStripSeparator15";
		this.toolStripSeparator15.Size = new System.Drawing.Size(257, 6);
		this.tsmEventosNFeCiencia.Name = "tsmEventosNFeCiencia";
		this.tsmEventosNFeCiencia.Size = new System.Drawing.Size(260, 26);
		this.tsmEventosNFeCiencia.Text = "NFe : Ciência da operação";
		this.tsmEventosNFeCiencia.Click += new System.EventHandler(tsbEventosNFeCiencia_Click);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(257, 6);
		this.backTaskRunUser.DoWork += new System.ComponentModel.DoWorkEventHandler(backTaskRunUser_DoWork);
		this.backTaskRunUser.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backTaskRunUser_RunWorkerCompleted);
		this.backTaskRunSyst.DoWork += new System.ComponentModel.DoWorkEventHandler(backTaskRunSyst_DoWork);
		this.backTaskRunSyst.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backTaskRunSyst_RunWorkerCompleted);
		this.htmlToolTip1.AllowLinksHandling = true;
		this.htmlToolTip1.BaseStylesheet = null;
		this.htmlToolTip1.MaximumSize = new System.Drawing.Size(0, 0);
		this.htmlToolTip1.OwnerDraw = true;
		this.htmlToolTip1.TooltipCssClass = "htmltooltip";
		this.toolStripSeparator17.Name = "toolStripSeparator17";
		this.toolStripSeparator17.Size = new System.Drawing.Size(257, 6);
		this.timerBannerCheck.Enabled = true;
		this.timerBannerCheck.Interval = 30000.0;
		this.timerBannerCheck.SynchronizingObject = this;
		this.timerBannerCheck.Elapsed += new System.Timers.ElapsedEventHandler(timerBannerCheck_Elapsed);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1225, 711);
		base.Controls.Add(this.splitMonData);
		base.Controls.Add(this.toolbarDocs01);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmMonitor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io Monitor";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(frmMonitor_HelpButtonClicked);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMonitor_FormClosing);
		base.Shown += new System.EventHandler(frmMonitor_Shown);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(frmMonitor_HelpRequested);
		base.Resize += new System.EventHandler(frmMonitor_Resize);
		this.splitMonData.Panel1.ResumeLayout(false);
		this.splitMonData.Panel1.PerformLayout();
		this.splitMonData.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitMonData).EndInit();
		this.splitMonData.ResumeLayout(false);
		this.splitMonDataLeft.Panel1.ResumeLayout(false);
		this.splitMonDataLeft.Panel2.ResumeLayout(false);
		this.splitMonDataLeft.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitMonDataLeft).EndInit();
		this.splitMonDataLeft.ResumeLayout(false);
		this.contextMenuFilial.ResumeLayout(false);
		this.pnMonTools.ResumeLayout(false);
		this.pnAdmTools.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTaskManager).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picIntegration).EndInit();
		this.pnTimerScan.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTimerScan).EndInit();
		this.pnBackupSet.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picBackupSet).EndInit();
		this.stsFilial.ResumeLayout(false);
		this.stsFilial.PerformLayout();
		this.toolbarCompanies.ResumeLayout(false);
		this.toolbarCompanies.PerformLayout();
		this.splitMonitorRight.Panel1.ResumeLayout(false);
		this.splitMonitorRight.Panel1.PerformLayout();
		this.splitMonitorRight.Panel2.ResumeLayout(false);
		this.splitMonitorRight.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitMonitorRight).EndInit();
		this.splitMonitorRight.ResumeLayout(false);
		this.contextMenuScan.ResumeLayout(false);
		this.pnStartup.ResumeLayout(false);
		this.pnContent.ResumeLayout(false);
		this.toolbarDocs02.ResumeLayout(false);
		this.toolbarDocs02.PerformLayout();
		this.stsSumary.ResumeLayout(false);
		this.stsSumary.PerformLayout();
		this.splitMonFeatures.Panel1.ResumeLayout(false);
		this.splitMonFeatures.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitMonFeatures).EndInit();
		this.splitMonFeatures.ResumeLayout(false);
		this.pnUserData.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picUserData).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picSupportChat).EndInit();
		this.toolbarFeatures.ResumeLayout(false);
		this.toolbarFeatures.PerformLayout();
		this.contextSortFilial.ResumeLayout(false);
		this.toolbarDocs01.ResumeLayout(false);
		this.toolbarDocs01.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.timerTaskRunUser).EndInit();
		((System.ComponentModel.ISupportInitialize)this.timerCheckScan).EndInit();
		((System.ComponentModel.ISupportInitialize)this.timerTaskRunSyst).EndInit();
		((System.ComponentModel.ISupportInitialize)this.timerBackupAdv).EndInit();
		this.contextManifest.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.timerBannerCheck).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
