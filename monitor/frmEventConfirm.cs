using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using srv.fiscal.io.email;
using util.fiscal.io;

namespace Monitor;

public class frmEventConfirm : Form
{
	private Configuration varclsConfig;

	private bool _IsLoaded;

	private bool _ColumnsLoaded;

	private int _InitialHeight;

	private clsDFeCodes varclsDFeCodes = new clsDFeCodes();

	private clsDataDoc varclsDataDoc = new clsDataDoc();

	private clsDataBatchHead varclsDataBatchHead = new clsDataBatchHead();

	private clsBatchService varclsBatchService = new clsBatchService();

	private clsDataParameter varclsDataParam = new clsDataParameter();

	private clsEventService varclsService = new clsEventService();

	private clsDataPartner varclsDataPartner = new clsDataPartner();

	private clsMetricService varclsMetricService = new clsMetricService();

	private CancellationTokenSource varTokenToCancel = new CancellationTokenSource();

	private bool _MustBatchRePost;

	private IContainer components;

	private Label lbPartnerName;

	private Panel pnEmail;

	private LinkLabel lkbChangeMyEmail;

	private Label lbMyEmail;

	private Label lbWarning02;

	private Label lbOthersEmail;

	private TextBox txOthersEmail;

	private CheckBox ckbSendOthers;

	private Label lbWarning01;

	private Label lbPartnerEmail;

	private TextBox txPartnerEmail;

	private CheckBox ckbSendMySelf;

	private CheckBox ckbSendPartner;

	private Panel pnReason;

	private LinkLabel lbAplicarTodos;

	private Label lbSeparator01;

	private TextBox txReason;

	private Label lbDescription;

	private Panel pnButtonData;

	private LinkLabel lkbDFeAutomatic;

	private Label lbDFeAutomatic;

	private Button btSair;

	private Label lbSeparator02;

	private Button btConfirmar;

	private Label label2;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private Panel pnCompany;

	private TextBox txDocKey;

	private Button btEnter;

	private Label lbDocKey;

	private ToolTip toolTipScreen;

	private Label txCompany;

	private Label lbCompany;

	private Label label3;

	private LinkLabel lkbLoadTxtFile;

	private Label lbWarning;

	private ContextMenuStrip contextMenuAction;

	private ToolStripMenuItem tsmRunOnline;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem tsmRunBackground;

	private FastObjectListView lsvDataDoc;

	private OLVColumn olvSelect;

	private OLVColumn olvDcNum;

	private OLVColumn olvtpEvento;

	private OLVColumn olvxEvento;

	private OLVColumn olvxJust;

	private OLVColumn olvcStat;

	private OLVColumn olvxMotivo;

	private OLVColumn olvChave;

	private Label label4;

	private Panel pnConfig;

	private PictureBox picDFeAutomatic;

	private Label label9;

	private Label label10;

	private ImageList ImageListDocs;

	private Panel panel1;

	private PictureBox pictureBox1;

	private Button btImport;

	private Panel pnForm;

	private Panel pnContent;

	private Panel pnTitleBar;

	private PictureBox picTitleBar;

	private Button btHelp;

	private Button btClose;

	private Label lbTitleBar;

	private ComboBox cbJustCode;

	public frmEventConfirm(clsEventData pclsEvtData)
	{
		InitializeComponent();
		clsFunction.funcSetShowBatchStatus(pIsToShow: false);
		varclsService.EvtData = pclsEvtData;
		if (clsFunction.IsAdmin)
		{
			Text = base.Name + " | " + Text;
		}
		lbTitleBar.Text = Text;
		funcHideAllFrames();
		funcConfigureListView();
	}

	private async void frmEventConfirm_Shown(object sender, EventArgs e)
	{
		try
		{
			await funcLoadScreenData01Async();
			CenterToParent();
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void funcConfigureListView()
	{
		_ColumnsLoaded = false;
		clsScreenGeral.funcSetColumnsObjectListView(this, lsvDataDoc);
		_ColumnsLoaded = true;
		foreach (OLVColumn allColumn in lsvDataDoc.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
		if (clsFunction.IsAdmin)
		{
			clsScreenGeral.funcCheckListViewDdic(typeof(Event), lsvDataDoc);
		}
		funcDefineListViewFeatures();
	}

	private void funcDefineListViewFeatures()
	{
		olvSelect.ImageGetter = delegate(object x)
		{
			Event obj = (Event)x;
			return (obj == null) ? null : ((object)funcGetStatIcon(obj));
		};
		olvDcNum.AspectGetter = delegate(object x)
		{
			Event obj = (Event)x;
			return (obj == null) ? null : clsFunction.funcGetDFeNum(obj.Chave);
		};
	}

	private int funcGetStatIcon(Event pclsEvent)
	{
		int varIcone = 0;
		string varStatus = clsFunction.funcGetValue(pclsEvent.cStat);
		string varReason = clsFunction.funcGetValue(pclsEvent.xMotivo).ToUpper();
		if (varStatus.Equals("PLANNED"))
		{
			return 0;
		}
		if (varStatus.Equals("PLANERR"))
		{
			return 2;
		}
		if (varStatus.Equals("RUNNING"))
		{
			return 1;
		}
		if (varStatus.Equals("RUNNERR"))
		{
			return 2;
		}
		if (varStatus.Equals("FINRESTR"))
		{
			return 5;
		}
		if (varStatus.Equals("FINISHED"))
		{
			return 4;
		}
		if (varStatus.Equals("FINERROR"))
		{
			return 2;
		}
		if (varReason.Contains("DUPLICIDADE"))
		{
			return 5;
		}
		if (varReason.Contains("SEGUNDO PLANO"))
		{
			return 5;
		}
		if (varReason.Contains("REJEI"))
		{
			return 5;
		}
		if (varReason.Contains("INUT"))
		{
			return 5;
		}
		if (varReason.Contains("CANCELA"))
		{
			return 5;
		}
		if (varReason.Contains("DENEG"))
		{
			return 5;
		}
		if (varReason.Contains("NÃO LIBERADO"))
		{
			return 5;
		}
		if (!clsFunction.IsEmpty(pclsEvent.Protc))
		{
			return 4;
		}
		if (!clsFunction.IsEmpty(varReason))
		{
			return 2;
		}
		return 0;
	}

	private async Task<bool> funcLoadScreenData01Async()
	{
		varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		if (varclsService.funcGetFilialCount() != 1)
		{
			txCompany.Text = "Empresa : Conforme seleção de dados";
		}
		else
		{
			FilialView varclsFilial = varclsService.funcGetFirstFilial();
			txCompany.Text = "Empresa : [ " + clsFunction.funcFormatDoc(varclsFilial.CNPJView) + " ] - " + varclsFilial.NomeView;
		}
		if (varclsService.funcGetEvtCount() <= 0)
		{
			pnCompany.Visible = true;
			pnCompany.Top = 8;
			txDocKey.Focus();
		}
		else
		{
			pnCompany.Visible = false;
			lsvDataDoc.Top = 8;
			lsvDataDoc.Focus();
		}
		plnMessage.Top = lsvDataDoc.Top + lsvDataDoc.Height - plnMessage.Height;
		funcLoadListView();
		await funcLoadScreenData02Async();
		_IsLoaded = false;
		funcLoadJustCodeList();
		_IsLoaded = true;
		return true;
	}

	private async Task<bool> funcLoadScreenData02Async()
	{
		varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		FilialView varclsFilial = varclsService.funcGetFirstFilial();
		_IsLoaded = false;
		Partner varclsPartner = varclsService.funcGetFirstPartner();
		Event varclsEvent = varclsService.funcGetFirstEvent();
		funcCheckIfShowReason(varclsEvent.tpEvento);
		if (lsvDataDoc.SelectedItem == null && lsvDataDoc.Items.Count > 0)
		{
			lsvDataDoc.Items[0].Selected = true;
		}
		if (lsvDataDoc.SelectedItem != null)
		{
			funcEnableReasonFrame(varclsEvent.tpEvento);
		}
		else
		{
			funcDisableReasonFrame();
		}
		Text = "Fiscal.io : " + varclsEvent.xEvento + " - Confirme o envio a SEFAZ ...";
		if (clsFunction.IsAdmin)
		{
			Text = base.Name + " | " + Text;
		}
		lbTitleBar.Text = Text;
		funcCheckShowWarningDownAuto();
		ckbSendMySelf.Checked = clsFunction.funcConvStrToBool(varclsConfig.EmailMySelfSetted);
		lbMyEmail.Text = clsFunction.funcFixEmail(varclsConfig.UserEmail);
		ckbSendPartner.Text = "Enviar e-mail ao parceiro [ ##PARNAME## ] informando o evento [ ##EVENT## ]";
		ckbSendPartner.Text = ckbSendPartner.Text.Replace("##PARNAME##", varclsPartner.Name);
		ckbSendPartner.Text = ckbSendPartner.Text.Replace(" [  ] ", " ");
		ckbSendPartner.Text = ckbSendPartner.Text.Replace("##EVENT##", varclsEvent.xEvento);
		ckbSendPartner.Tag = varclsPartner.ID;
		Partner varPartner = await varclsDataPartner.funcGetItemWithEmailAsync(varclsPartner.ID);
		if (varPartner != null)
		{
			ckbSendPartner.Checked = clsFunction.funcConvStrToBool(varPartner.EmailSetted);
			if (!clsFunction.IsEmpty(varPartner.Email))
			{
				txPartnerEmail.Text = varPartner.Email;
			}
			else if (!clsFunction.IsEmpty(varPartner.EmailCert))
			{
				txPartnerEmail.Text = varPartner.EmailCert;
			}
		}
		else
		{
			ckbSendPartner.Checked = true;
			txPartnerEmail.Text = string.Empty;
		}
		txPartnerEmail.Tag = varclsPartner.Name;
		if (varclsFilial != null)
		{
			ckbSendOthers.Checked = clsFunction.funcConvStrToBool(varclsFilial.OtherEmailSetted);
			txOthersEmail.Text = varclsFilial.OtherEmailValue;
		}
		funcSetFieldsEnabled();
		funcCheckIfShowEmail(varclsEvent.tpEvento);
		_IsLoaded = true;
		return true;
	}

	private async Task<bool> funcShowEventReasonAsync(Event pclsEvent)
	{
		Document varDocument = await varclsService.funcGetDocAsync(pclsEvent.Chave);
		string varDescription = "Justificativa do evento para o documento [ " + varDocument.Num + "-" + varDocument.Serie + " ]";
		lbDescription.Text = varDescription;
		if (!clsFunction.IsEmpty(pclsEvent.xJust))
		{
			txReason.Text = pclsEvent.xJust;
		}
		else
		{
			txReason.Text = pclsEvent.xObs;
		}
		txReason.Tag = pclsEvent;
		if (lsvDataDoc.SelectedObjects.Count == 0)
		{
			funcDisableReasonFrame();
		}
		else if (!clsFunction.IsEmpty(pclsEvent.Protc))
		{
			funcDisableReasonFrame(pShowInfo: true);
		}
		else
		{
			funcEnableReasonFrame(pclsEvent.tpEvento);
		}
		lsvDataDoc.RefreshObject(pclsEvent);
		Application.DoEvents();
		return true;
	}

	private bool funcLoadJustCodeList()
	{
		List<clsObjectType> varNFSeJustList = varclsDFeCodes.funcNFSeJustCodeList(pClearCharacter: false);
		cbJustCode.DisplayMember = "Name";
		cbJustCode.ValueMember = "Value";
		cbJustCode.DataSource = varNFSeJustList;
		cbJustCode.SelectedIndex = -1;
		return true;
	}

	private void funcLoadListView()
	{
		lsvDataDoc.BeginUpdate();
		lsvDataDoc.SuspendLayout();
		List<Event> varEvtList = varclsService.funcGetEvtList();
		lsvDataDoc.SetObjects(varEvtList);
		lsvDataDoc.EndUpdate();
		lsvDataDoc.ResumeLayout();
	}

	private async void lsvDataDoc_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lsvDataDoc.SelectedItem != null)
		{
			Event varclsEvent = (Event)lsvDataDoc.SelectedItem.RowObject;
			if (varclsEvent != null)
			{
				await funcShowEventReasonAsync(varclsEvent);
			}
		}
	}

	private bool funcValidateEmailList(string pEmailText)
	{
		List<string> varEmailList = clsFunction.funcGetMailList(pEmailText);
		if (!clsFunction.IsEmpty(pEmailText) && varEmailList.Count == 0)
		{
			MessageBox.Show("O e-mail [ " + pEmailText + " ] não é válido. Favor corrigir", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		foreach (string varEmailValue in varEmailList)
		{
			if (!clsFunction.IsEmpty(varEmailValue) && !clsFunction.funcIsValidEmail(varEmailValue))
			{
				MessageBox.Show("O e-mail [ " + varEmailValue + " ] não é válido. Favor corrigir", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return false;
			}
		}
		return true;
	}

	public bool funcHasCertificate(FilialView pFilial)
	{
		if (clsFunction.IsEmpty(pFilial.Certificado))
		{
			if (!base.Visible)
			{
				return false;
			}
			MessageBox.Show("Nenhum Certificado Digital foi informado para a empresa.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		return true;
	}

	private async Task<List<string>> funcCheckDFeMasterCertAsync()
	{
		List<string> varWarnList = new List<string>();
		bool varMustCheckMaster = false;
		if (varclsService.IsEvtStatusQueryFull)
		{
			varMustCheckMaster = true;
		}
		else if (varclsService.IsEvtDownloadFull)
		{
			varMustCheckMaster = true;
		}
		if (!varMustCheckMaster)
		{
			return varWarnList;
		}
		clsDataDFeMaster varclsDataHandler = new clsDataDFeMaster();
		clsDataCertContent varclsDataCert = new clsDataCertContent();
		List<FilialView> varFilialList = varclsService.funcGetFilialList();
		string varLastKey = string.Empty;
		foreach (FilialView varclsFilial in varFilialList)
		{
			List<string> varModelList = varclsService.funcGetDocModelList(varclsFilial.CNPJ);
			string.Join(", ", varModelList.ToArray());
			List<string> varTypeList = varclsService.funcGetDocTypeList(varclsFilial.CNPJ);
			string varTypeListStr = string.Join("-", varTypeList.ToArray());
			List<string> varUfList = varclsService.funcGetUfCodeList(varclsFilial.CNPJ);
			string varvarUfListStr = string.Join("-", varUfList.ToArray());
			string varSiglaListStr = string.Join("-", (await varclsService.funcGetStateSiglaListAsync(varUfList)).ToArray());
			string varLoopKey = varTypeListStr + "-" + varvarUfListStr;
			if (clsFunction.IsEqual(varLoopKey, varLastKey))
			{
				continue;
			}
			varLastKey = varLoopKey;
			List<DFeMaster> varDFeMasterList = await varclsDataHandler.funcGetListAsync(varUfList, varTypeList, varclsFilial.TipoDoc);
			varDFeMasterList.RemoveAll((DFeMaster r) => clsFunction.IsEmpty(r.dmXmlJurd));
			if (varDFeMasterList.Count <= 0)
			{
				varWarnList.Add("Funcionalidade não disponível para " + varTypeListStr + " em " + varSiglaListStr + ".");
			}
			else if (!varDFeMasterList.Any((DFeMaster r) => clsFunction.IsEqual(r.dmCert, "A3")))
			{
				_ = varclsFilial.CNPJView + " : " + varclsFilial.NomeView;
				X509Certificate2 varCertificate = await clsSrvGeral.funcGetCertificateAsync(varclsFilial, pCheckKey: false, pCheckPin: false);
				if (varCertificate == null)
				{
					varCertificate = new X509Certificate2();
				}
				if (await varclsDataCert.funcGetItemByKeyAsync(varCertificate.SerialNumber) == null)
				{
					varWarnList.Add("Funcionalidade disponível somente com certificado A1 para " + varTypeListStr + " em " + varSiglaListStr + ".");
					btImport.Visible = true;
				}
			}
		}
		return varWarnList;
	}

	private async Task<bool> funcsHasErrorInFormAsync()
	{
		new clsDataCertContent();
		clsEmailService varclsEmailService = new clsEmailService();
		varclsService.funcGetFilialList();
		string varMessage = string.Empty;
		if (lsvDataDoc.Items.Count == 0)
		{
			varMessage = "Nenhum evento foi informado para envio a SEFAZ...";
			MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return true;
		}
		if (!varclsService.funcHasPendent())
		{
			varMessage = "Todos os registros já foram processsados com sucesso.";
			MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return true;
		}
		foreach (string varclsItem in await funcCheckDFeMasterCertAsync())
		{
			varMessage = varMessage + varMessage + varclsItem + Environment.NewLine;
		}
		if (!clsFunction.IsEmpty(varMessage))
		{
			MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return true;
		}
		if (varclsService.IsEvtDownload || varclsService.IsEvtStatusQuery)
		{
			return false;
		}
		string varEvtMessage = string.Empty;
		foreach (Event varclsEvent in lsvDataDoc.Objects)
		{
			if (varclsService.IsReasonMandatory(varclsEvent.tpEvento))
			{
				if (clsFunction.IsEmpty(varclsEvent.xJust))
				{
					varEvtMessage = "Justificativa não informada!!!";
					varclsEvent.cStat = "999";
					varclsEvent.xMotivo = varEvtMessage;
				}
				else if (varclsEvent.xJust.Length < 15)
				{
					varEvtMessage = "Justificativa com menos de 15 caracteres!!!";
					varclsEvent.cStat = "999";
					varclsEvent.xMotivo = varEvtMessage;
				}
				lsvDataDoc.RefreshObject(varclsEvent);
			}
		}
		if (!clsFunction.IsEmpty(varEvtMessage))
		{
			varMessage = "Justificativa não informada ou com menos de 15 caracteres!!!";
			MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return true;
		}
		if (pnEmail.Visible && ckbSendPartner.Checked)
		{
			if (clsFunction.IsEmpty(txPartnerEmail.Text))
			{
				varMessage = "Informe o e-mail do Parceiro Comercial para envio do Evento após o registro na SEFAZ...";
				MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}
			if (!funcValidateEmailList(txPartnerEmail.Text))
			{
				return true;
			}
		}
		if (pnEmail.Visible && ckbSendOthers.Checked)
		{
			if (clsFunction.IsEmpty(txOthersEmail.Text))
			{
				varMessage = "Informe o e-mail dos outros interessados ou Desmarque a opção [Enviar E-mail a outros interessados ...]";
				MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}
			if (!funcValidateEmailList(txOthersEmail.Text))
			{
				return true;
			}
		}
		List<string> varEmailList = new List<string>();
		if (pnEmail.Visible && ckbSendPartner.Checked)
		{
			varEmailList.AddRange(clsFunction.funcGetMailList(txPartnerEmail.Text));
		}
		if (pnEmail.Visible && ckbSendOthers.Checked)
		{
			varEmailList.AddRange(clsFunction.funcGetMailList(txOthersEmail.Text));
		}
		if (pnEmail.Visible && ckbSendMySelf.Checked)
		{
			varEmailList.Add(varclsConfig.UserEmail);
		}
		if (varEmailList.Count <= 0)
		{
			return false;
		}
		if (varEmailList.Count == 1)
		{
			funcShowActionProgress("Verificando se email de destino é valido...");
		}
		else
		{
			funcShowActionProgress("Verificando se emails de destino são validos...");
		}
		clsReturn varclsReturnFunc = new clsReturn();
		Task<clsReturn> varclsTaskCheckEmail = varclsEmailService.funcCheckEmailAsync(varEmailList);
		if (await varclsTaskCheckEmail.WaitAsync(TimeSpan.FromSeconds(5.0)))
		{
			varclsReturnFunc = varclsTaskCheckEmail.Result;
		}
		funcHideActionProgress();
		if (varclsReturnFunc.HasError || varclsReturnFunc.HasWarning)
		{
			funcShowErrorMessage(varclsReturnFunc);
			return true;
		}
		varEmailList = varclsReturnFunc.GetObject<List<string>>("EmailList");
		if (varEmailList == null)
		{
			varEmailList = new List<string>();
		}
		if (varEmailList.Count <= 0)
		{
			return false;
		}
		string varUserMessage = "Email inválido. Favor corrigir !!!" + Environment.NewLine + Environment.NewLine;
		foreach (string varclsItem2 in varEmailList)
		{
			varUserMessage = varUserMessage + varclsItem2 + Environment.NewLine + Environment.NewLine;
		}
		MessageBox.Show(varUserMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		return true;
	}

	private async void btConfirmar_Click(object sender, EventArgs e)
	{
		IEnumerable varEvents = lsvDataDoc.Objects;
		if (await funcsHasErrorInFormAsync())
		{
			return;
		}
		int varCounterNotExec = 0;
		int varCounterEvtList = 0;
		foreach (Event varItem in varEvents)
		{
			bool varMustConfirm = false;
			varCounterEvtList++;
			if (clsFunction.Contains(varItem.xMotivo, "cancelado", pIgnoreCase: true))
			{
				varMustConfirm = true;
			}
			else if (clsFunction.Contains(varItem.xMotivo, "denegado", pIgnoreCase: true))
			{
				varMustConfirm = true;
			}
			else if (clsFunction.Contains(varItem.xMotivo, "inutilizado", pIgnoreCase: true))
			{
				varMustConfirm = true;
			}
			else if (clsFunction.Contains(varItem.xMotivo, "rejeitado", pIgnoreCase: true))
			{
				varMustConfirm = true;
			}
			if (varMustConfirm)
			{
				Document varclsDoc = varclsDataDoc.funcGetDocFromKey("", varItem.Chave);
				if (MessageBox.Show(varclsDoc.Num + "-" + varclsDoc.Serie + " : " + varItem.xMotivo + ". Deseja continuar?", "Confirmação de dados", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					varItem.Protc = clsFunction.funcGetRandom(15);
					varCounterNotExec++;
				}
			}
		}
		if (!varCounterEvtList.Equals(varCounterNotExec))
		{
			new clsReturn();
			clsReturn varclsReturnFunc = (varclsService.IsEvtStatusQueryFull ? (await funcExecuteBackgroundAsync()) : ((!varclsService.IsEvtDownloadFull) ? (await funcExecuteOnLineAsync()) : (await funcExecuteBackgroundAsync())));
			if (varclsReturnFunc.ActionDone)
			{
				Close();
			}
		}
	}

	private async void tsmRunOnline_Click(object sender, EventArgs e)
	{
		if ((await funcExecuteOnLineAsync()).ActionDone)
		{
			Close();
		}
	}

	private async Task<bool> funcHasBalanceAsync()
	{
		clsBalanceManager varclsManager = new clsBalanceManager();
		int varTotDownXmlFull = varclsService.funcGetDownXmlFullCount();
		int varTotConsStaFull = varclsService.funcGetConsStaFullCount();
		if (varTotDownXmlFull <= 0 && varTotConsStaFull <= 0)
		{
			return true;
		}
		funcShowActionProgress("Calculando o custo dos downloads  ....");
		decimal varTotalCost = await varclsService.funcGetTotalCostAsync();
		plnMessage.Visible = false;
		Application.DoEvents();
		bool result = await varclsManager.funcConfirmActionAsync(clsBalanceService.consMetDowQuerySpec, varTotalCost);
		funcHideActionProgress();
		return result;
	}

	private async void tsmRunBackground_Click(object sender, EventArgs e)
	{
		await funcExecuteBackgroundAsync();
	}

	private async Task<clsReturn> funcExecuteBackgroundAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsMeasureService varclsMeasureService = new clsMeasureService(null);
		List<FilialView> varFilialList = varclsService.funcGetFilialList();
		foreach (FilialView varclsItem in varFilialList)
		{
			await clsMonGeral.funcSetCertificateAsync(this, varclsItem);
			if (!funcHasCertificate(varclsItem))
			{
				return varclsReturnFunc;
			}
		}
		if (!(await funcHasBalanceAsync()))
		{
			return varclsReturnFunc;
		}
		if (varclsService.funcGetFirstEvent() == null)
		{
			return varclsReturnFunc;
		}
		clsObjectType varclsObjType = varclsService.funcGetObjectType();
		varclsReturnFunc = await funcPlanTasksAsync(varclsObjType);
		varclsMeasureService.funcSyncAsync();
		if (varclsReturnFunc.HasError)
		{
			funcShowErrorMessage(varclsReturnFunc);
			varclsReturnFunc.ActionDone = false;
			clsFunction.funcSetShowBatchStatus(pIsToShow: false);
		}
		else
		{
			varclsReturnFunc.ActionDone = true;
			clsFunction.funcSetShowBatchStatus(pIsToShow: true);
		}
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcExecuteOnLineAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		int varTotDownXmlAut = varclsService.funcGetDownXmlFullCount();
		int varTotConsStFull = varclsService.funcGetConsStaFullCount();
		List<FilialView> varFilialList = varclsService.funcGetFilialList();
		foreach (FilialView varclsItem in varFilialList)
		{
			await clsMonGeral.funcSetCertificateAsync(this, varclsItem);
			if (!funcHasCertificate(varclsItem))
			{
				return varclsReturnFunc;
			}
		}
		if (!(await funcHasBalanceAsync()))
		{
			return varclsReturnFunc;
		}
		return (varTotDownXmlAut > 0 || varTotConsStFull > 0) ? (await funcOnServerOnLineAsync()) : (await funcOnLocalOnLineAsync());
	}

	private async Task<clsReturn> funcOnLocalOnLineAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		bool varHasError = false;
		lsvDataDoc.Focus();
		_ = string.Empty;
		_ = string.Empty;
		btConfirmar.Enabled = false;
		btConfirmar.Text = "Processando";
		DateTime varStartTime = DateTime.Now;
		DateTime varEndTime = varStartTime;
		int varTotItens = lsvDataDoc.Items.Count + 1;
		int varProcItens = 0;
		foreach (Event varclsEvent in lsvDataDoc.Objects)
		{
			varTotItens--;
			varProcItens++;
			if (!clsFunction.IsAdmin && !clsFunction.IsEmpty(varclsEvent.Protc))
			{
				continue;
			}
			FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsEvent.Cnpj);
			if (varclsFilial != null)
			{
				Document varclsDoc = varclsDataDoc.funcGetDocFromKey(varclsFilial.CNPJ, varclsEvent.Chave);
				funcShowActionProgress(varclsDoc, varclsEvent, varStartTime, varEndTime, varProcItens, varTotItens);
				varStartTime = DateTime.Now;
				clsReturn varclsRetItem = await varclsService.funcExecuteAsync(varclsDoc, varclsEvent);
				Event varclsRetEvent = (Event)varclsRetItem.GetObject("clsEvent");
				if (varclsRetEvent == null)
				{
					varclsRetEvent = varclsService.funcGetStatus(varclsEvent, varclsRetItem);
				}
				varclsReturnFunc.AddRange(varclsRetItem);
				varclsEvent.Protc = varclsRetEvent.Protc;
				varclsEvent.cStat = varclsRetEvent.cStat;
				varclsEvent.xMotivo = varclsRetEvent.xMotivo;
				Event varclsEvtEmail = varclsRetEvent;
				if (varclsEvtEmail == null)
				{
					varclsEvtEmail = varclsEvent;
				}
				clsReturn varclsRetEmail = await funcSendEmailAsync(varclsDoc, varclsEvtEmail);
				if (varclsRetEmail.HasError)
				{
					varclsReturnFunc.AddRange(varclsRetEmail);
				}
				varEndTime = DateTime.Now;
				await funcShowEventReasonAsync(varclsEvent);
				lsvDataDoc.RefreshObject(varclsEvent);
			}
		}
		if (varclsReturnFunc.HasError && !varclsReturnFunc.HasWarning)
		{
			varclsReturnFunc.AddMessage(0, new clsMessage("E", "999", "Ocorreram erros no registro do(s) evento(s) na SEFAZ"));
			funcShowErrorMessage(varclsReturnFunc);
			varHasError = true;
		}
		funcHideActionProgress();
		if (varHasError)
		{
			varclsReturnFunc.ActionDone = false;
			return varclsReturnFunc;
		}
		if (!varclsService.funcHasPendent())
		{
			MessageBox.Show(varclsService.funcGetMessageSucess(), "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			varclsReturnFunc.ActionDone = true;
		}
		else
		{
			if (lsvDataDoc.Items.Count == 1)
			{
				Event varclsEvent2 = varclsService.funcGetFirstEvent();
				if (!clsFunction.IsEmpty(varclsEvent2.xMotivo))
				{
					MessageBox.Show(varclsEvent2.xMotivo, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (!clsFunction.IsEmpty(varclsEvent2.xJust))
				{
					MessageBox.Show(varclsEvent2.xJust, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			varclsReturnFunc.ActionDone = false;
		}
		if (varclsReturnFunc.HasError)
		{
			varclsReturnFunc.ActionDone = false;
		}
		else if (varclsReturnFunc.HasWarning)
		{
			varclsReturnFunc.ActionDone = false;
		}
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcOnServerOnLineAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDataBatchItem varclsDataBatchItem = new clsDataBatchItem();
		varTokenToCancel = new CancellationTokenSource();
		lsvDataDoc.Focus();
		bool varHasError = false;
		_MustBatchRePost = false;
		new clsDataConfig().funcGetItemByKeyAsync();
		btConfirmar.Enabled = false;
		btConfirmar.Text = "Processando";
		Event varclsEvent = varclsService.funcGetFirstEvent();
		List<FilialView> varclsFilialList = varclsService.funcGetFilialList();
		int varTotItens = lsvDataDoc.Items.Count;
		int varProcItens = 0;
		FilialView varclsFilial = new FilialView();
		varclsService.DoneList.Clear();
		foreach (Event varclsItem in lsvDataDoc.Objects)
		{
			varProcItens++;
			if (!clsFunction.IsEqual(varclsFilial.CNPJ, varclsItem.Cnpj))
			{
				varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsItem.Cnpj);
			}
			if (varclsFilial == null)
			{
				varclsFilial = new FilialView();
				continue;
			}
			Document varclsDoc = varclsDataDoc.funcGetDocFromKey(varclsFilial.CNPJ, varclsItem.Chave);
			funcShowActionProgress($" [ {varProcItens} de {varTotItens} ] Preparando eventos...");
			clsReturn varclsRetItem = await varclsService.funcExecuteAsync(varclsDoc, varclsItem);
			if (varclsRetItem.HasError)
			{
				varclsReturnFunc.AddRange(varclsRetItem);
			}
		}
		if (varclsReturnFunc.HasError)
		{
			funcHideActionProgress();
			funcShowErrorMessage(varclsReturnFunc);
			return varclsReturnFunc;
		}
		funcShowActionProgress("Planejando processamento dos documentos...");
		List<BatchHead> varclsBatchList = await varclsBatchService.funcGetBatchListAsync(varclsFilialList, varclsEvent.tpEvento);
		varclsReturnFunc = await varclsBatchService.funcPlanTaskRequestAsync(varclsBatchList, pSetPercent: true);
		if (varclsReturnFunc.HasError)
		{
			funcHideActionProgress();
			funcShowErrorMessage(varclsReturnFunc);
			return varclsReturnFunc;
		}
		funcShowActionProgress("Obtendo status de documentos que já estavam em processamento...");
		foreach (Event varclsItem in lsvDataDoc.Objects)
		{
			if (!clsFunction.IsEqual(varclsFilial.CNPJ, varclsItem.Cnpj))
			{
				varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsItem.Cnpj);
			}
			if (varclsFilial == null)
			{
				varclsFilial = new FilialView();
				continue;
			}
			BatchHead varclsDFeHead = varclsBatchList.FirstOrDefault((BatchHead r) => clsFunction.IsEqual(r.BthFilial, varclsFilial.CNPJ));
			if (varclsDFeHead == null)
			{
				continue;
			}
			BatchItem varclsDFeItem = await varclsDataBatchItem.funcGetItemByKeyAsync(varclsDFeHead.BthId, varclsItem.Chave);
			if (varclsDFeItem != null && !clsFunction.IsEqual(varclsDFeItem.BtiStatus, "PLANNED"))
			{
				clsReturn varclsRetItem2 = await funcSetEventStatusAsync(varclsDFeItem);
				if (varclsRetItem2.HasError)
				{
					varclsRetItem2.AddRange(varclsRetItem2);
				}
			}
		}
		if (varclsReturnFunc.HasError)
		{
			funcHideActionProgress();
			funcShowErrorMessage(varclsReturnFunc);
			return varclsReturnFunc;
		}
		clsDFeKeyService varclsDFeService = new clsDFeKeyService();
		varclsDFeService.EventDFeKeyService += funcEventDFeKeyService;
		funcShowActionProgress("Enviando documentos para processamento...");
		varclsReturnFunc = await varclsBatchService.funcPostBatchAsync(varclsBatchList, varclsDFeService, pIsOnline: true);
		if (varclsReturnFunc.HasError)
		{
			funcHideActionProgress();
			funcShowErrorMessage(varclsReturnFunc);
			return varclsReturnFunc;
		}
		bool varHasPendent = varclsService.funcHasPendent();
		if (varHasPendent)
		{
			funcShowActionProgress("Aguarde, os documentos estão sendo processados...");
		}
		bool varExitLoop = false;
		while (!varExitLoop && varHasPendent && !varTokenToCancel.IsCancellationRequested)
		{
			varclsBatchList = varclsBatchList.Where((BatchHead r) => !r.IsFinished()).ToList();
			int varBatchCount = varclsBatchList.Count;
			string varLastRQueue = string.Empty;
			if (varBatchCount <= 0)
			{
				break;
			}
			if (_MustBatchRePost)
			{
				await varclsBatchService.funcPostBatchAsync(varclsBatchList, varclsDFeService, pIsOnline: true);
				_MustBatchRePost = false;
			}
			for (int varItem = 0; varItem < varBatchCount; varItem++)
			{
				string varBatchQueue = varclsBatchList[varItem].BtdQueUsr;
				if (!clsFunction.IsEqual(varLastRQueue, varBatchQueue))
				{
					varLastRQueue = varBatchQueue;
					clsReturn varclsRetItem3 = await varclsDFeService.funcGetFromQueueAsync(varBatchQueue);
					if (varclsRetItem3.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem3);
						varExitLoop = true;
						break;
					}
					List<BatchHead> list = varclsBatchList;
					int index = varItem;
					list[index] = await varclsBatchService.funcDefinePercAsync(varclsBatchList[varItem]);
				}
			}
			try
			{
				await Task.Delay(TimeSpan.FromSeconds(5.0), varTokenToCancel.Token);
			}
			catch
			{
			}
		}
		if (varclsReturnFunc.HasError && !varclsReturnFunc.HasWarning)
		{
			varclsReturnFunc.AddMessage(0, new clsMessage("E", "999", "Ocorreram erros no registro do(s) evento(s) na SEFAZ"));
			funcShowErrorMessage(varclsReturnFunc);
			varHasError = true;
		}
		funcHideActionProgress();
		if (varHasError)
		{
			varclsReturnFunc.ActionDone = false;
			return varclsReturnFunc;
		}
		if (!varclsService.funcHasPendent())
		{
			if (varclsService.IsEvtCTeDisagree)
			{
				await varclsMetricService.funcTrySetAhaMomentAsync("CTE_DISAGREE");
			}
			MessageBox.Show(varclsService.funcGetMessageSucess(), "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			varclsReturnFunc.ActionDone = true;
		}
		else
		{
			varclsReturnFunc.ActionDone = false;
		}
		if (varclsReturnFunc.HasError)
		{
			varclsReturnFunc.ActionDone = false;
		}
		else if (varclsReturnFunc.HasWarning)
		{
			varclsReturnFunc.ActionDone = false;
		}
		return varclsReturnFunc;
	}

	private async void funcEventDFeKeyService(object sender, EventDFeKeyEventArgs e)
	{
		if (e.ObjBatch == null || e.ObjItem == null || varTokenToCancel == null || await clsSrvGeral.funcGetFilialAsync(e.ObjBatch.BthFilial) == null)
		{
			return;
		}
		BatchItem varclsDFeItem = e.ObjItem;
		clsDataEvent varclsDataEvent = new clsDataEvent();
		Event varclsEvent = varclsService.funcGetEvent(varclsDFeItem.BtiDocKey);
		if (varclsEvent != null)
		{
			varclsDataEvent.funcSetBuffer(varclsEvent);
			if (clsFunction.IsEmpty(varclsEvent.cStat))
			{
				varclsEvent.cStat = varclsDFeItem.BtiStatus;
			}
			if (clsFunction.IsEmpty(varclsEvent.xMotivo))
			{
				varclsEvent.xMotivo = varclsDFeItem.BtiMessage;
			}
			if (clsFunction.IsEqual(varclsDFeItem.BtiStatus, "PLANNED"))
			{
				_MustBatchRePost = true;
			}
			else if (clsFunction.IsEqual(varclsDFeItem.BtiStatus, "FINISHED") && clsFunction.IsEmpty(varclsEvent.Protc))
			{
				varclsEvent.Protc = clsFunction.funcGetRandom(15);
			}
			await new clsDataEvent().funcUpdateAsync(varclsEvent);
			varclsService.RunnList.RemoveAll((Event r) => clsFunction.IsEqual(r.Chave, varclsEvent.Chave));
			varclsService.DoneList.RemoveAll((Event r) => clsFunction.IsEqual(r.Chave, varclsEvent.Chave));
			if (varclsDFeItem.IsRunning())
			{
				varclsService.RunnList.Add(varclsEvent);
			}
			else if (varclsDFeItem.IsFinished())
			{
				varclsService.DoneList.Add(varclsEvent);
			}
			await funcShowEventReasonAsync(varclsEvent);
			try
			{
				lsvDataDoc.RefreshObject(varclsEvent);
			}
			catch
			{
			}
			int varTotalList = lsvDataDoc.Items.Count;
			int varTotalDone = varclsService.DoneList.Count;
			int varTotalRunn = varclsService.RunnList.Count;
			int varTotalPend = varTotalList - varTotalDone;
			if (varTotalPend < 0)
			{
				varTotalPend = 0;
			}
			funcShowActionProgress($"Em andamento... Planejados {varTotalList} / Executando {varTotalRunn} / Concluídos {varTotalDone}");
			if (varTotalPend <= 0)
			{
				funcHideActionProgress();
				varTokenToCancel.Cancel();
			}
		}
	}

	private async Task<clsReturn> funcSetEventStatusAsync(BatchItem pclsDFeItem)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			if (pclsDFeItem == null)
			{
				return varclsReturnFunc;
			}
			if (varTokenToCancel == null)
			{
				return varclsReturnFunc;
			}
			clsDataEvent varclsDataEvent = new clsDataEvent();
			Event varclsEvent = varclsService.funcGetEvent(pclsDFeItem.BtiDocKey);
			if (varclsEvent == null)
			{
				return varclsReturnFunc;
			}
			varclsDataEvent.funcSetBuffer(varclsEvent);
			varclsEvent.cStat = pclsDFeItem.BtiStatus;
			varclsEvent.xMotivo = pclsDFeItem.BtiMessage;
			if (clsFunction.IsEqual(pclsDFeItem.BtiStatus, "PLANNED"))
			{
				_MustBatchRePost = true;
			}
			else if (clsFunction.IsEqual(pclsDFeItem.BtiStatus, "FINISHED"))
			{
				varclsEvent.Protc = clsFunction.funcGetRandom(15);
			}
			await new clsDataEvent().funcUpdateAsync(varclsEvent);
			varclsService.RunnList.RemoveAll((Event r) => clsFunction.IsEqual(r.Chave, varclsEvent.Chave));
			varclsService.DoneList.RemoveAll((Event r) => clsFunction.IsEqual(r.Chave, varclsEvent.Chave));
			if (pclsDFeItem.IsRunning())
			{
				varclsService.RunnList.Add(varclsEvent);
			}
			else if (pclsDFeItem.IsFinished())
			{
				varclsService.DoneList.Add(varclsEvent);
			}
			await funcShowEventReasonAsync(varclsEvent);
			try
			{
				lsvDataDoc.RefreshObject(varclsEvent);
			}
			catch
			{
			}
			int varTotalList = lsvDataDoc.Items.Count;
			int varTotalDone = varclsService.DoneList.Count;
			int varTotalRunn = varclsService.RunnList.Count;
			int varTotalPend = varTotalList - varTotalDone;
			if (varTotalPend < 0)
			{
				varTotalPend = 0;
			}
			funcShowActionProgress($"Em andamento... Planejados {varTotalList} / Executando {varTotalRunn} / Concluídos {varTotalDone}");
			if (varTotalPend > 0)
			{
				return varclsReturnFunc;
			}
			funcHideActionProgress();
			varTokenToCancel.Cancel();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcSendEmailAsync(Document pclsDoc, Event pclsEvent)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDataDoc varclsDataDoc = new clsDataDoc();
		try
		{
			if (!pnEmail.Visible)
			{
				return varclsReturnFunc;
			}
			if (clsFunction.IsEmpty(pclsEvent.Protc))
			{
				return varclsReturnFunc;
			}
			if (!varclsService.IsEmailMandatory(pclsEvent.tpEvento))
			{
				return varclsReturnFunc;
			}
			List<string> varMailListTo = new List<string>();
			List<string> varMailListCc = new List<string>();
			if (ckbSendPartner.Checked)
			{
				varMailListTo.AddRange(clsFunction.funcGetMailList(txPartnerEmail.Text));
			}
			if (ckbSendOthers.Checked)
			{
				varMailListTo.AddRange(clsFunction.funcGetMailList(txOthersEmail.Text));
			}
			if (ckbSendMySelf.Checked && varMailListTo.Count <= 0)
			{
				varMailListTo.Add(varclsConfig.UserEmail);
			}
			else if (ckbSendMySelf.Checked)
			{
				varMailListCc.Add(varclsConfig.UserEmail);
			}
			if (varMailListTo.Count <= 0)
			{
				return varclsReturnFunc;
			}
			funcShowActionProgress(pclsDoc.Num + " -> Enviando e-mail ....");
			plnMessage.Visible = true;
			Application.DoEvents();
			Document varclsDoc = await varclsDataDoc.funcGetItemByKeyAsync(pclsDoc.Filial, pclsDoc.Chave);
			if (varclsDoc == null)
			{
				varclsDoc = pclsDoc;
			}
			varclsReturnFunc = await varclsService.funcSendEmailAsync(varclsDoc, pclsEvent, varMailListTo, varMailListCc);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddError("999", "Doc. [ " + pclsDoc.Num + " ]. Não foi possivel enviar o e-mail.", pException);
		}
		return varclsReturnFunc;
	}

	private void funcShowErrorMessage(clsReturn pclsReturn)
	{
		if (base.Visible && pclsReturn != null && (pclsReturn.HasError || pclsReturn.HasWarning))
		{
			clsScreenGeral.funcShowUserMessage(this, pclsReturn);
		}
	}

	private void lbAplicarTodos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		foreach (Event varItem in lsvDataDoc.Objects)
		{
			if (clsFunction.IsEmpty(varItem.Protc))
			{
				varItem.xJust = txReason.Text;
				varItem.xMotivo = clsFunction.funcGetValue(varItem.xMotivo);
				if (varItem.xMotivo.StartsWith("Justificativa"))
				{
					string cStat = (varItem.xMotivo = string.Empty);
					varItem.cStat = cStat;
				}
			}
		}
	}

	private void txReason_TextChanged(object sender, EventArgs e)
	{
		Event varclsEvent = null;
		varclsEvent = ((lsvDataDoc.SelectedItem == null) ? ((Event)txReason.Tag) : ((Event)lsvDataDoc.SelectedItem.RowObject));
		if (varclsEvent == null)
		{
			return;
		}
		varclsEvent.xJust = clsFunction.funcClearSpecialCaracter(txReason.Text);
		varclsEvent.xMotivo = clsFunction.funcGetValue(varclsEvent.xMotivo);
		if (varclsEvent.xMotivo.StartsWith("Justificativa"))
		{
			Event obj = varclsEvent;
			string cStat = (varclsEvent.xMotivo = string.Empty);
			obj.cStat = cStat;
		}
		try
		{
			lsvDataDoc.RefreshObject(varclsEvent);
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void funcHideActionProgress()
	{
		plnMessage.Visible = false;
		lsvDataDoc.Height = _InitialHeight;
		btConfirmar.Enabled = true;
		btConfirmar.Text = "Confirmar";
	}

	private void funcShowActionProgress(string pMessage)
	{
		btConfirmar.Text = "Processando";
		btConfirmar.Enabled = false;
		lbProgress.Text = pMessage;
		lsvDataDoc.Height = _InitialHeight - (plnMessage.Height + 5);
		plnMessage.Visible = true;
		lbProgress.Visible = true;
		Application.DoEvents();
	}

	private void funcShowActionProgress(Document pclsDoc, Event pclsEvt, DateTime pStartTime, DateTime pEndTime, int pProcItens, int pTotItens)
	{
		btConfirmar.Text = "Processando";
		btConfirmar.Enabled = false;
		lbProgress.Text = $"[ {pProcItens} de {lsvDataDoc.Items.Count} ]   ";
		Label label = lbProgress;
		label.Text = label.Text + pclsDoc.Num + " -> " + pclsEvt.xEvento;
		double varTotalSeconds = pEndTime.Subtract(pStartTime).TotalSeconds;
		if (varTotalSeconds > 0.0)
		{
			lbProgress.Text += "   |   Tempo médio: ";
			lbProgress.Text += $"Item {Math.Round(varTotalSeconds, 0)} seg / ";
			double varTotalToFinish = varTotalSeconds * (double)pTotItens;
			if (varTotalToFinish < 60.0)
			{
				lbProgress.Text += $"Total {Math.Round(varTotalToFinish, 2)} seg";
			}
			else
			{
				lbProgress.Text += $"Total {Math.Round(varTotalToFinish / 60.0, 2)} min";
			}
		}
		lsvDataDoc.Height = _InitialHeight - (plnMessage.Height + 5);
		plnMessage.Visible = true;
		lbProgress.Visible = true;
		Application.DoEvents();
	}

	private void btSair_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btCancelar_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void lbDFeAutomatic_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		FilialView varclsFilial = varclsService.funcGetFirstFilial();
		if (varclsFilial != null)
		{
			frmFilial frmFilial = new frmFilial(varclsFilial, null, pShowDocIn: true);
			frmFilial.ShowDialog(this);
			frmFilial.Dispose();
			pnConfig.Visible = false;
			funcResizeScreen();
		}
	}

	private async void lkbChangeMyEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		frmLicense frmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
		frmLicense.ShowDialog(this);
		frmLicense.Dispose();
		varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		lbMyEmail.Text = varclsConfig.UserEmail;
	}

	private async void funcSaveUserEmailDataPreferencesAsync()
	{
		if (!_IsLoaded)
		{
			return;
		}
		FilialView varclsFilial = varclsService.funcGetFirstFilial();
		if (varclsFilial != null)
		{
			bool varNewPartner = false;
			varclsConfig.EmailMySelfSetted = (ckbSendMySelf.Checked ? "X" : "");
			await new clsDataConfig().funcUpdateAsync(varclsConfig);
			string varPartnerID = (string)ckbSendPartner.Tag;
			varPartnerID = (clsFunction.IsEmpty(varPartnerID) ? "" : varPartnerID);
			string varPartnerName = (string)txPartnerEmail.Tag;
			varPartnerName = (clsFunction.IsEmpty(varPartnerName) ? "" : varPartnerName);
			Partner varPartner = await new clsDataPartner().funcGetItemByKeyAsync(varPartnerID);
			if (varPartner == null)
			{
				varPartner = new Partner();
				varNewPartner = true;
			}
			varPartner.ID = varPartnerID;
			varPartner.Name = varPartnerName;
			varPartner.Email = txPartnerEmail.Text;
			varPartner.EmailSetted = (ckbSendPartner.Checked ? "X" : "");
			if (!varNewPartner)
			{
				await new clsDataPartner().funcUpdateAsync(varPartner);
			}
			else
			{
				await new clsDataPartner().funcInsertAsync(varPartner);
			}
			varclsFilial.OtherEmailSetted = (ckbSendOthers.Checked ? "X" : "");
			varclsFilial.OtherEmailValue = txOthersEmail.Text;
			await new clsDataFilial().funcUpdateAsync(varclsFilial, pLogUserFields: true);
		}
	}

	private void ckbSendPartner_CheckedChanged(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private void ckbSendOthers_CheckedChanged(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private void ckbSendMySelf_CheckedChanged(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private void txOthersEmail_Validated(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private void txPartnerEmail_Validated(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private void funcSetFieldsEnabled()
	{
		lbMyEmail.Enabled = ckbSendMySelf.Checked;
		lkbChangeMyEmail.Enabled = ckbSendMySelf.Checked;
		txPartnerEmail.Enabled = ckbSendPartner.Checked;
		lbPartnerEmail.Enabled = ckbSendPartner.Checked;
		lbWarning01.Enabled = ckbSendPartner.Checked;
		txOthersEmail.Enabled = ckbSendOthers.Checked;
		lbOthersEmail.Enabled = ckbSendOthers.Checked;
		lbWarning02.Enabled = ckbSendOthers.Checked;
	}

	private async void btEnter_Click(object sender, EventArgs e)
	{
		clsFunction.funcClearAllSpaces(Regex.Replace(txDocKey.Text, "[^0-9 ]", ""));
		clsReturn varclsReturnFunc = await funcAddItemAsync(txDocKey.Text);
		txDocKey.Text = string.Empty;
		funcLoadListView();
		await funcLoadScreenData02Async();
		funcHideKeyLoadProgress();
		funcSetFocusItem(new clsReturn());
		if (varclsReturnFunc.HasError || varclsReturnFunc.HasWarning)
		{
			funcShowErrorMessage(varclsReturnFunc);
		}
	}

	private async Task<clsReturn> funcAddItemAsync(string pDocKey)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsEvtService varclsEvtService = new clsEvtService();
		clsDFeNumService varclsDFeService = new clsDFeNumService();
		clsDFeNumService varclsDFe = new clsDFeNumService();
		try
		{
			FilialView varclsFilial = varclsService.funcGetFirstFilial();
			if (varclsFilial == null)
			{
				return varclsReturnFunc;
			}
			string varDocKey = Regex.Replace(pDocKey, "[^0-9 ]", "");
			varDocKey = clsFunction.funcClearAllSpaces(varDocKey);
			if (clsFunction.IsEmpty(varDocKey))
			{
				return varclsReturnFunc;
			}
			if (varclsService.funcHasDoc(varDocKey))
			{
				return varclsReturnFunc;
			}
			if (clsFunction.IsAdmin && varDocKey.Length == 43)
			{
				varDocKey += varclsDFe.funcGetDigit(varDocKey);
			}
			string varDocModel = clsFunction.funcGetDFeModel(varDocKey);
			string varUserMessage = string.Empty;
			if (clsFunction.funcIsNFSe(varDocModel) && varDocKey.Length != 50)
			{
				varUserMessage = "Chave inválida. Tamanho diferente de 50 digitos. " + varDocKey;
			}
			else if (!clsFunction.funcIsNFSe(varDocModel) && varDocKey.Length != 44)
			{
				varUserMessage = "Chave inválida. Tamanho diferente de 44 digitos. " + varDocKey;
			}
			else if (!varclsDFeService.funcIsDigitOk(varDocKey))
			{
				varUserMessage = "Chave inválida. Digito verificador incorreto. " + varDocKey;
			}
			if (!clsFunction.IsEmpty(varUserMessage))
			{
				varclsReturnFunc.AddMessage(new clsMessage("W", "9999", varUserMessage));
				return varclsReturnFunc;
			}
			Event varEventDefault = varclsService.funcGetFirstEvent();
			string varEventType = varclsService.funcGetEvtType(varDocKey);
			string varDocType = clsFunction.funcGetDocType(varDocModel, pCTeOs: false);
			if (clsFunction.IsEmpty(varEventType))
			{
				string varMessageTxt = varEventDefault.xEvento + " não disponível para " + varDocType + ".";
				varclsReturnFunc.AddMessage(new clsMessage("W", "9999", varMessageTxt));
				return varclsReturnFunc;
			}
			Document varclsDoc = await varclsDataDoc.funcGetItemByKeyAsync(varclsFilial.CNPJ, varDocKey);
			if (varclsDoc == null)
			{
				varclsDoc = varclsDataDoc.funcGetDocFromKey(varclsFilial, varDocKey);
			}
			varclsService.funcAddDoc(varclsDoc);
			if (varclsService.IsEvtDownload && varEventType.Equals("DOWNLOAD"))
			{
				varclsService.EvtData = varclsEvtService.funcGetDownEvtType(this, varclsService.EvtData, varDocType);
				if (varclsService.IsCancel)
				{
					varclsService.funcDelDoc(varclsDoc);
					return varclsReturnFunc;
				}
			}
			else if (varclsService.IsEvtDownload)
			{
				varclsService.EvtData.EvtUserType = varEventType;
			}
			varclsReturnFunc = await varclsEvtService.funcCheckAuthorizationAsync(varclsService.EvtData);
			if (varclsReturnFunc.HasError || !varclsReturnFunc.ActionDone)
			{
				varclsService.funcDelDoc(varclsDoc);
				return varclsReturnFunc;
			}
			varEventType = varclsService.funcGetEvtType(varDocKey);
			List<Event> varEvtList = await varclsEvtService.funcNewEventAsync(varEventType, varclsFilial, varclsDoc, "Manual");
			varclsService.funcAddEvt(varEvtList);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private async Task<Event> funcCreateEventItemAsync(Document pclsDoc, string pTpEvento, enEvent pEventType)
	{
		clsEventTemplate varclsTemplate = new clsEventTemplate();
		FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(pclsDoc.Filial);
		if (varclsFilial == null)
		{
			return null;
		}
		new Event();
		Event varclsEvent = ((!varclsDFeCodes.IsEvtStatusQueryFull(pTpEvento)) ? (await new clsDataEvent().funcGetItemByChaveTpEventoAsync(pclsDoc.Chave, pTpEvento)) : null);
		if (varclsEvent == null)
		{
			varclsEvent = await varclsTemplate.GetAsync(varclsFilial, pclsDoc.Chave, pEventType, "Manual");
		}
		if ((varclsDFeCodes.IsEvtDownloadFull(varclsEvent.tpEvento) || !clsFunction.IsEmpty(pclsDoc.HasXml)) && (clsFunction.IsEmpty(pclsDoc.XmlError) || !clsFunction.IsEmpty(pclsDoc.FisIoApi)))
		{
			varclsEvent.Protc = clsFunction.funcGetRandom(15);
			varclsEvent.xMotivo = "XML já se encontra no Fiscal.io Monitor";
		}
		return varclsEvent;
	}

	private async void txDocKey_KeyDown(object sender, KeyEventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			string varUserKeyData = string.Empty;
			if (e.Control && e.KeyCode == Keys.V)
			{
				if (Clipboard.ContainsText(TextDataFormat.Text))
				{
					varUserKeyData = Clipboard.GetText(TextDataFormat.Text);
				}
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Return)
			{
				varUserKeyData = txDocKey.Text;
			}
			if (clsFunction.IsEmpty(varUserKeyData))
			{
				return;
			}
			varclsReturnFunc = await funcLoadKeyListAsync(varUserKeyData);
			txDocKey.Text = string.Empty;
			funcLoadListView();
			await funcLoadScreenData02Async();
			funcSetFocusItem(varclsReturnFunc);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		if (varclsReturnFunc.HasError || varclsReturnFunc.HasWarning)
		{
			funcShowErrorMessage(varclsReturnFunc);
		}
	}

	private void funcSetFocusItem(clsReturn pclsReturn)
	{
		try
		{
			if (!pclsReturn.HasError && !pclsReturn.HasWarning && lsvDataDoc.Items.Count > 0)
			{
				lsvDataDoc.SelectedIndex = lsvDataDoc.Items.Count - 1;
				lsvDataDoc.SelectedItem.Focused = true;
				lsvDataDoc.SelectedItem.EnsureVisible();
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

	private async void lkbLoadTxtFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			OpenFileDialog varFileDialog = new OpenFileDialog();
			OpenFileDialog openFileDialog = varFileDialog;
			openFileDialog.InitialDirectory = await clsScreenGeral.funcGetDefaultFolderAsync(this);
			varFileDialog.Multiselect = true;
			varFileDialog.Filter = "Arquivo TXT (*.TXT;)|*.TXT|Arquivo CSV (*.CSV;)|*.CSV";
			varFileDialog.Title = "Informe um TXT ou CSV com as chaves de acesso";
			DialogResult varResult = varFileDialog.ShowDialog();
			await clsScreenGeral.funcSetDefaultFolderAsync(this, varFileDialog.FileName);
			if (varResult != DialogResult.OK || varFileDialog.FileNames.Length == 0)
			{
				return;
			}
			varclsReturnFunc = await funcLoadFileListAsync(varFileDialog.FileNames);
			funcLoadListView();
			await funcLoadScreenData02Async();
			funcSetFocusItem(varclsReturnFunc);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		if (varclsReturnFunc.HasError)
		{
			funcShowErrorMessage(varclsReturnFunc);
		}
	}

	private async void btBatchTask_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("TASK-MANAGER", "VIEW"))
		{
			frmTaskManager frmTaskManager = new frmTaskManager(pShowBatch: true);
			frmTaskManager.ShowDialog();
			frmTaskManager.Dispose();
		}
	}

	private void lsvData_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		if (!_ColumnsLoaded)
		{
			return;
		}
		ListView varListView = (ListView)sender;
		if (varListView != null)
		{
			string varColumName = clsScreenGeral.funcGetColumnName(lsvDataDoc.AllColumns[e.Header.Index]);
			if (!clsFunction.IsEmpty(varColumName))
			{
				clsFunction.funcSetRegisterValue(base.Name + "-" + varListView.Name + "-" + varColumName + "-Order", e.NewDisplayIndex.ToString(), pGlobal: false);
			}
		}
	}

	private void lsvData_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		if (!_ColumnsLoaded)
		{
			return;
		}
		ListView varListView = (ListView)sender;
		if (varListView != null)
		{
			OLVColumn varColumnObj = lsvDataDoc.AllColumns[e.ColumnIndex];
			string varColumName = clsScreenGeral.funcGetColumnName(varColumnObj);
			if (!clsFunction.IsEmpty(varColumName))
			{
				clsFunction.funcSetRegisterValue(base.Name + "-" + varListView.Name + "-" + varColumName + "-Width", varColumnObj.Width.ToString(), pGlobal: false);
			}
		}
	}

	private void OlvColumn_VisibilityChanged(object sender, EventArgs e)
	{
		if (!_ColumnsLoaded)
		{
			return;
		}
		OLVColumn varOlvColumn = (OLVColumn)sender;
		string varColumName = clsScreenGeral.funcGetColumnName(varOlvColumn);
		if (!clsFunction.IsEmpty(varColumName))
		{
			string varObjectKey = base.Name + "-" + lsvDataDoc.Name + "-" + varColumName + "-Hidden";
			if (!varOlvColumn.IsVisible)
			{
				clsFunction.funcSetRegisterValue(varObjectKey, "X", pGlobal: false);
			}
			else
			{
				clsFunction.funcSetRegisterValue(varObjectKey, string.Empty, pGlobal: false);
			}
		}
	}

	private async Task<clsReturn> funcLoadFileListAsync(string[] pFileList)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			funcShowKeyLoadProgress("Aguarde, as informações estão sendo carregadas ...");
			foreach (string varFilePath in pFileList)
			{
				string varFileName = new FileInfo(varFilePath).Name;
				funcShowKeyLoadProgress("Arquivo " + varFileName + " : Lendo conteúdo ...");
				if (!File.Exists(varFilePath))
				{
					continue;
				}
				string varFileContent = File.ReadAllText(varFilePath);
				string[] varDocKeyList = (varFileContent.Contains(Environment.NewLine) ? varFileContent.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None) : ((!varFileContent.Contains(";")) ? varFileContent.Split(' ') : varFileContent.Split(';')));
				funcShowKeyLoadProgress("Arquivo " + varFileName + " : Processando conteúdo ...");
				double varCounter = 0.0;
				double varTotalList = varDocKeyList.Length;
				string[] array = varDocKeyList;
				foreach (string varDocItem in array)
				{
					funcShowProgress(varTotalList, varCounter, "Arquivo " + varFileName + " : Processando conteúdo ...");
					varCounter += 1.0;
					clsReturn varclsReturnItem = await funcAddItemAsync(varDocItem);
					if (varclsReturnItem.HasError)
					{
						varclsReturnFunc.AddRange(varclsReturnItem);
					}
					if (varclsService.IsCancel)
					{
						break;
					}
				}
				if (varclsService.IsCancel)
				{
					break;
				}
			}
			varclsService.EvtData.Cancel = false;
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		funcHideKeyLoadProgress();
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcLoadKeyListAsync(string pKeyList)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			funcShowKeyLoadProgress("Aguarde, as informações estão sendo carregadas ...");
			funcShowKeyLoadProgress("Lendo o conteúdo copiado ...");
			funcShowKeyLoadProgress("Calculando o total de linhas ...");
			StringReader varReaderData01 = new StringReader(pKeyList);
			_ = string.Empty;
			double varTotalList = 0.0;
			double varCounter = 0.0;
			while (varReaderData01.ReadLine() != null)
			{
				varTotalList += 1.0;
			}
			funcShowKeyLoadProgress("Processando o conteúdo copiado ...");
			StringReader varReaderData2 = new StringReader(pKeyList);
			string varStrLine;
			while ((varStrLine = varReaderData2.ReadLine()) != null)
			{
				funcShowProgress(varTotalList, varCounter, "Processando o conteúdo copiado ...");
				varCounter += 1.0;
				varclsReturnFunc.AddRange((await funcAddItemAsync(varStrLine)).Messages);
				if (varclsService.IsCancel)
				{
					break;
				}
			}
			varclsService.EvtData.Cancel = false;
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		funcHideKeyLoadProgress();
		return varclsReturnFunc;
	}

	private void funcHideKeyLoadProgress()
	{
		lbProgress.Text = string.Empty;
		plnMessage.Visible = false;
		Application.DoEvents();
	}

	private void funcShowKeyLoadProgress(string pMessage)
	{
		lbProgress.Text = pMessage;
		plnMessage.Visible = true;
		Application.DoEvents();
	}

	private void funcShowProgress(double pTotal, double pCounter, string pMessage)
	{
		int varPercent = Convert.ToInt32(pCounter / pTotal * 100.0);
		lbProgress.Text = $"{pMessage} [ {varPercent} % ] ";
		plnMessage.Visible = true;
		Application.DoEvents();
	}

	private async Task<clsReturn> funcPlanTasksAsync(clsObjectType pclsObjType)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		BatchHead varclsBatchHead = new BatchHead();
		List<clsObjectType> varclsBatchList = new List<clsObjectType>();
		new clsTaskScheduler();
		string varLastFilial = string.Empty;
		try
		{
			funcShowActionProgress("Planejando tarefas ...");
			double varTotalList = lsvDataDoc.Items.Count;
			double varCounter = 0.0;
			FilialView varclsFilial = new FilialView();
			foreach (Event varclsEvent in lsvDataDoc.Objects)
			{
				if (varclsEvent == null)
				{
					continue;
				}
				funcShowProgress(varTotalList, varCounter, "Planejando tarefas ...");
				varCounter += 1.0;
				if (!clsFunction.IsEqual(varclsFilial.CNPJ, varclsEvent.Cnpj))
				{
					varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsEvent.Cnpj);
				}
				if (varclsFilial == null)
				{
					varclsFilial = new FilialView();
					continue;
				}
				if (!varLastFilial.Equals(varclsFilial.CNPJ))
				{
					varclsBatchHead = await funcGetBatchHeadAsync(varclsBatchList, varclsFilial, varclsEvent);
					varclsBatchList.Add(new clsObjectType
					{
						Name = varclsFilial.CNPJ,
						Object = varclsBatchHead
					});
				}
				varLastFilial = varclsFilial.CNPJ;
				clsReturn varclsRetItem = await varclsBatchService.funcAddItemAsync(varclsBatchHead, varclsEvent.Chave);
				if (varclsRetItem.HasError)
				{
					varclsReturnFunc.AddRange(varclsRetItem);
				}
				if (!varclsRetItem.ActionDone)
				{
				}
			}
			int varTotalTasks = await funcSyncBatchTotalAsync(varclsBatchList);
			clsReturn varclsRetSync = await funcSyncBatchTaskerAsync(varclsBatchList);
			if (varclsRetSync.HasError)
			{
				varclsReturnFunc.AddRange(varclsRetSync);
			}
			funcHideActionProgress();
			if (varTotalTasks > 0)
			{
				string varMessage = "Eventos adicionados a fila de processamento!" + Environment.NewLine + Environment.NewLine;
				varMessage = varMessage + "Consulte o andamento através do menu " + Environment.NewLine + Environment.NewLine;
				varMessage = varMessage + "[Download de XML] -> [Lotes de Processamento]" + Environment.NewLine + Environment.NewLine;
				foreach (clsObjectType varObjItem in varclsBatchList)
				{
					if (varObjItem != null)
					{
						varclsBatchHead = (BatchHead)varObjItem.Object;
						if (clsFunction.funcConvStrToInt(varclsBatchHead.BthItems) <= 0)
						{
							await varclsDataBatchHead.funcDeleteAsync((BatchHead)varObjItem.Object);
						}
						else
						{
							varMessage = varMessage + "Lote : " + varclsBatchHead.BthId + Environment.NewLine;
						}
					}
				}
				varMessage += Environment.NewLine;
				MessageBox.Show(varMessage, "Tarefas programadas com sucesso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (!varclsReturnFunc.HasError)
			{
				foreach (clsObjectType varObjItem2 in varclsBatchList)
				{
					if (varObjItem2 != null)
					{
						await varclsDataBatchHead.funcDeleteAsync((BatchHead)varObjItem2.Object);
					}
				}
				MessageBox.Show(string.Concat("Eventos já estavam planejados previamente!" + Environment.NewLine + Environment.NewLine, "Consulte o andamento através do [Gerenciador de Tarefas] -> Lotes", Environment.NewLine, Environment.NewLine), "Tarefas programadas com sucesso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private async Task<int> funcSyncBatchTotalAsync(List<clsObjectType> pBatchList)
	{
		int varTotalTasks = 0;
		foreach (clsObjectType pBatch in pBatchList)
		{
			BatchHead varBatchHead = (BatchHead)pBatch.Object;
			varTotalTasks += clsFunction.funcConvStrToInt((await varclsBatchService.funcDefinePercAsync(varBatchHead)).BthItems);
		}
		return varTotalTasks;
	}

	private async Task<clsReturn> funcSyncBatchTaskerAsync(List<clsObjectType> pBatchList)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		foreach (clsObjectType varObject in pBatchList)
		{
			BatchHead varBatchHead = (BatchHead)varObject.Object;
			varclsReturnFunc.AddRange(await varclsBatchService.funcPlanTaskRequestAsync(varBatchHead, pSetPercent: false));
			varclsReturnFunc.AddRange(await varclsBatchService.funcPlanTaskResponseAsync(varBatchHead));
		}
		return varclsReturnFunc;
	}

	private async Task<BatchHead> funcGetBatchHeadAsync(List<clsObjectType> pBatchList, FilialView pclsFilial, Event pclsEvent)
	{
		clsObjectType varObjItem = pBatchList.FirstOrDefault((clsObjectType r) => r.Name.Equals(pclsFilial.CNPJ));
		if (varObjItem != null)
		{
			return (BatchHead)varObjItem.Object;
		}
		BatchHead varclsBatchHead = await varclsBatchService.funcGetBatchNormalAsync(pclsFilial, pclsEvent);
		if (varclsBatchHead == null)
		{
			return null;
		}
		return varclsBatchHead;
	}

	private void backtaskBatch_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		if (e.ProgressPercentage <= 0)
		{
			lbProgress.Text = (string)e.UserState;
		}
		else
		{
			lbProgress.Text = $"{(string)e.UserState}  [ {e.ProgressPercentage} % ] ";
		}
		plnMessage.Visible = true;
		Application.DoEvents();
	}

	private void funcHideAllFrames()
	{
		Panel panel = pnReason;
		bool visible = (pnConfig.Visible = false);
		panel.Visible = visible;
		Panel panel2 = pnConfig;
		visible = (pnEmail.Visible = false);
		panel2.Visible = visible;
		funcResizeScreen();
	}

	private void funcResizeScreen()
	{
		try
		{
			if (pnReason.Visible)
			{
				pnReason.Top = lsvDataDoc.Top + lsvDataDoc.Height + 5;
				pnButtonData.Top = pnReason.Top + pnReason.Height + 5;
			}
			else
			{
				pnButtonData.Top = lsvDataDoc.Top + lsvDataDoc.Height + 5;
			}
			if (pnConfig.Visible)
			{
				pnConfig.Top = pnButtonData.Top + pnButtonData.Height;
				pnEmail.Top = pnConfig.Top + pnConfig.Height + 5;
			}
			else
			{
				pnEmail.Top = pnButtonData.Top + pnButtonData.Height;
			}
			if (pnEmail.Visible)
			{
				base.Height = pnEmail.Top + pnEmail.Height + 35;
			}
			else if (pnConfig.Visible)
			{
				base.Height = pnConfig.Top + pnConfig.Height + 35;
			}
			else
			{
				base.Height = pnButtonData.Top + pnButtonData.Height + 35;
			}
			lsvDataDoc.RebuildColumns();
			_InitialHeight = lsvDataDoc.Height;
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private void funcCheckIfShowReason(string pTpEvento)
	{
		if (varclsService.IsReasonMandatory(pTpEvento))
		{
			funcShowReasonFrame(pTpEvento);
		}
		else
		{
			funcHideReasonFrame();
		}
		funcResizeScreen();
	}

	private void funcCheckIfShowEmail(string pTpEvento)
	{
		if (varclsService.IsEmailMandatory(pTpEvento))
		{
			pnEmail.Visible = true;
		}
		else
		{
			pnEmail.Visible = false;
		}
		funcResizeScreen();
	}

	private void funcShowReasonFrame(string pTpEvento)
	{
		pnReason.Visible = true;
		olvxJust.IsVisible = true;
		if (lsvDataDoc.SelectedObjects.Count == 0)
		{
			funcDisableReasonFrame();
		}
		else
		{
			funcEnableReasonFrame(pTpEvento);
		}
		funcResizeScreen();
	}

	private void funcHideReasonFrame()
	{
		pnReason.Visible = false;
		olvxJust.IsVisible = false;
		funcDisableReasonFrame();
		funcResizeScreen();
	}

	private void funcDisableReasonFrame(bool pShowInfo = false)
	{
		if (!pShowInfo)
		{
			lbDescription.Text = "Clique sobre o documento fiscal para informar a justificativa ...";
			txReason.Tag = null;
			txReason.Text = string.Empty;
		}
		lbDescription.Enabled = false;
		lbAplicarTodos.Visible = false;
		txReason.Enabled = false;
	}

	private void funcEnableReasonFrame(string pTpEvento)
	{
		if (lbDescription.Visible)
		{
			lbDescription.Enabled = true;
			lbAplicarTodos.Visible = true;
			txReason.Enabled = true;
			if (varclsDFeCodes.IsNFSeDisagree(pTpEvento))
			{
				txReason.Enabled = false;
				ComboBox comboBox = cbJustCode;
				bool enabled = (cbJustCode.Visible = true);
				comboBox.Enabled = enabled;
				txReason.Height = cbJustCode.Height;
				txReason.Top = cbJustCode.Top + cbJustCode.Height + 1;
				txReason.Height = cbJustCode.Height;
			}
			else
			{
				txReason.Enabled = true;
				ComboBox comboBox2 = cbJustCode;
				bool enabled = (cbJustCode.Visible = false);
				comboBox2.Enabled = enabled;
				txReason.Top = cbJustCode.Top;
				txReason.Height = cbJustCode.Height * 2;
			}
		}
	}

	private void funcCheckShowWarningDownAuto()
	{
		if (!varclsService.funcHasEvt("210210"))
		{
			return;
		}
		foreach (FilialView item in varclsService.funcGetFilialList())
		{
			if (clsFunction.IsEmpty(item.NFeDownAuto))
			{
				pnConfig.Visible = true;
				funcResizeScreen();
				break;
			}
		}
	}

	private void frmEventConfirm_Leave(object sender, EventArgs e)
	{
		varTokenToCancel.Cancel();
	}

	private void lkbDownPrice_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallCreditPricePageAsync();
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
		clsDataFilial varclsDataFilial = new clsDataFilial();
		List<FilialView> varOldList = varclsService.funcGetFilialList();
		List<FilialView> varNewList = new List<FilialView>();
		foreach (FilialView varclsItem in varOldList)
		{
			varNewList.Add(await varclsDataFilial.funcGetItemByKeyAsync(varclsItem.CNPJ));
		}
		varclsService.EvtData.FilialList = varNewList;
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void lbTitleBar_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			clsScreenGeral.funcFormMouseMove(base.Handle);
		}
		if (e.Clicks > 1)
		{
			if (base.MaximizeBox && base.WindowState.Equals(FormWindowState.Maximized))
			{
				base.WindowState = FormWindowState.Normal;
			}
			else if (base.MaximizeBox)
			{
				base.WindowState = FormWindowState.Maximized;
			}
		}
	}

	private void btHelp_Click(object sender, EventArgs e)
	{
		Event varclsEvent = varclsService.funcGetFirstEvent();
		if (varclsEvent != null)
		{
			clsHelpService.funcCallDFeEventHelp(varclsEvent.tpEvento);
		}
	}

	private void frmEventConfirm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		Event varclsEvent = varclsService.funcGetFirstEvent();
		if (varclsEvent != null)
		{
			clsHelpService.funcCallDFeEventHelp(varclsEvent.tpEvento);
		}
	}

	private void cbJustCode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (_IsLoaded)
		{
			txReason.Enabled = false;
			txReason.Text = cbJustCode.Text;
		}
	}

	private void cbJustCode_TextChanged(object sender, EventArgs e)
	{
		if (_IsLoaded)
		{
			txReason.Enabled = false;
			txReason.Text = cbJustCode.Text;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmEventConfirm));
		this.lbPartnerName = new System.Windows.Forms.Label();
		this.pnEmail = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lkbChangeMyEmail = new System.Windows.Forms.LinkLabel();
		this.lbMyEmail = new System.Windows.Forms.Label();
		this.lbWarning02 = new System.Windows.Forms.Label();
		this.lbOthersEmail = new System.Windows.Forms.Label();
		this.txOthersEmail = new System.Windows.Forms.TextBox();
		this.ckbSendOthers = new System.Windows.Forms.CheckBox();
		this.lbWarning01 = new System.Windows.Forms.Label();
		this.lbPartnerEmail = new System.Windows.Forms.Label();
		this.txPartnerEmail = new System.Windows.Forms.TextBox();
		this.ckbSendMySelf = new System.Windows.Forms.CheckBox();
		this.ckbSendPartner = new System.Windows.Forms.CheckBox();
		this.pnReason = new System.Windows.Forms.Panel();
		this.cbJustCode = new System.Windows.Forms.ComboBox();
		this.lbAplicarTodos = new System.Windows.Forms.LinkLabel();
		this.lbSeparator01 = new System.Windows.Forms.Label();
		this.txReason = new System.Windows.Forms.TextBox();
		this.lbDescription = new System.Windows.Forms.Label();
		this.pnButtonData = new System.Windows.Forms.Panel();
		this.btImport = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.btSair = new System.Windows.Forms.Button();
		this.lbSeparator02 = new System.Windows.Forms.Label();
		this.btConfirmar = new System.Windows.Forms.Button();
		this.lkbDFeAutomatic = new System.Windows.Forms.LinkLabel();
		this.lbDFeAutomatic = new System.Windows.Forms.Label();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.pnCompany = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.lkbLoadTxtFile = new System.Windows.Forms.LinkLabel();
		this.lbWarning = new System.Windows.Forms.Label();
		this.txCompany = new System.Windows.Forms.Label();
		this.lbCompany = new System.Windows.Forms.Label();
		this.lbDocKey = new System.Windows.Forms.Label();
		this.txDocKey = new System.Windows.Forms.TextBox();
		this.btEnter = new System.Windows.Forms.Button();
		this.toolTipScreen = new System.Windows.Forms.ToolTip(this.components);
		this.contextMenuAction = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmRunOnline = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmRunBackground = new System.Windows.Forms.ToolStripMenuItem();
		this.lsvDataDoc = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvDcNum = new BrightIdeasSoftware.OLVColumn();
		this.olvtpEvento = new BrightIdeasSoftware.OLVColumn();
		this.olvxEvento = new BrightIdeasSoftware.OLVColumn();
		this.olvxJust = new BrightIdeasSoftware.OLVColumn();
		this.olvcStat = new BrightIdeasSoftware.OLVColumn();
		this.olvxMotivo = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.pnConfig = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.picDFeAutomatic = new System.Windows.Forms.PictureBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.pnForm = new System.Windows.Forms.Panel();
		this.pnContent = new System.Windows.Forms.Panel();
		this.pnTitleBar = new System.Windows.Forms.Panel();
		this.picTitleBar = new System.Windows.Forms.PictureBox();
		this.btHelp = new System.Windows.Forms.Button();
		this.btClose = new System.Windows.Forms.Button();
		this.lbTitleBar = new System.Windows.Forms.Label();
		this.pnEmail.SuspendLayout();
		this.pnReason.SuspendLayout();
		this.pnButtonData.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		this.pnCompany.SuspendLayout();
		this.contextMenuAction.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvDataDoc).BeginInit();
		this.pnConfig.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picDFeAutomatic).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.pnForm.SuspendLayout();
		this.pnContent.SuspendLayout();
		this.pnTitleBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).BeginInit();
		base.SuspendLayout();
		this.lbPartnerName.AutoSize = true;
		this.lbPartnerName.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbPartnerName.Location = new System.Drawing.Point(451, 524);
		this.lbPartnerName.Name = "lbPartnerName";
		this.lbPartnerName.Size = new System.Drawing.Size(0, 13);
		this.lbPartnerName.TabIndex = 99;
		this.pnEmail.Controls.Add(this.label4);
		this.pnEmail.Controls.Add(this.label2);
		this.pnEmail.Controls.Add(this.lkbChangeMyEmail);
		this.pnEmail.Controls.Add(this.lbMyEmail);
		this.pnEmail.Controls.Add(this.lbWarning02);
		this.pnEmail.Controls.Add(this.lbOthersEmail);
		this.pnEmail.Controls.Add(this.txOthersEmail);
		this.pnEmail.Controls.Add(this.ckbSendOthers);
		this.pnEmail.Controls.Add(this.lbWarning01);
		this.pnEmail.Controls.Add(this.lbPartnerEmail);
		this.pnEmail.Controls.Add(this.txPartnerEmail);
		this.pnEmail.Controls.Add(this.ckbSendMySelf);
		this.pnEmail.Controls.Add(this.ckbSendPartner);
		this.pnEmail.Location = new System.Drawing.Point(-2, 462);
		this.pnEmail.Name = "pnEmail";
		this.pnEmail.Size = new System.Drawing.Size(924, 151);
		this.pnEmail.TabIndex = 100;
		this.label4.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label4.Location = new System.Drawing.Point(7, 147);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(913, 2);
		this.label4.TabIndex = 124;
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 3);
		this.label2.Name = "label2";
		this.label2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
		this.label2.Size = new System.Drawing.Size(912, 21);
		this.label2.TabIndex = 123;
		this.label2.Text = "Ações automáticas após confirmar o(s) evento(s) na SEFAZ ...";
		this.lkbChangeMyEmail.Location = new System.Drawing.Point(599, 87);
		this.lkbChangeMyEmail.Name = "lkbChangeMyEmail";
		this.lkbChangeMyEmail.Size = new System.Drawing.Size(307, 13);
		this.lkbChangeMyEmail.TabIndex = 8;
		this.lkbChangeMyEmail.TabStop = true;
		this.lkbChangeMyEmail.Text = "Corrigir meu e-mail";
		this.lkbChangeMyEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbChangeMyEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbChangeMyEmail_LinkClicked);
		this.lbMyEmail.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbMyEmail.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
		this.lbMyEmail.Location = new System.Drawing.Point(599, 69);
		this.lbMyEmail.Name = "lbMyEmail";
		this.lbMyEmail.Size = new System.Drawing.Size(307, 14);
		this.lbMyEmail.TabIndex = 120;
		this.lbMyEmail.Text = "lucas@fiscal.io";
		this.lbMyEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbWarning02.AutoSize = true;
		this.lbWarning02.Font = new System.Drawing.Font("Verdana", 7f);
		this.lbWarning02.Location = new System.Drawing.Point(90, 130);
		this.lbWarning02.Name = "lbWarning02";
		this.lbWarning02.Size = new System.Drawing.Size(463, 12);
		this.lbWarning02.TabIndex = 119;
		this.lbWarning02.Text = "Para indicar mais que um endereço de e-mail, separe-os com ponto e vírgula (;).\r\n";
		this.lbOthersEmail.AutoSize = true;
		this.lbOthersEmail.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbOthersEmail.Location = new System.Drawing.Point(19, 110);
		this.lbOthersEmail.Name = "lbOthersEmail";
		this.lbOthersEmail.Size = new System.Drawing.Size(68, 13);
		this.lbOthersEmail.TabIndex = 118;
		this.lbOthersEmail.Text = "E-mail(s) :";
		this.txOthersEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txOthersEmail.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.txOthersEmail.Location = new System.Drawing.Point(92, 107);
		this.txOthersEmail.MaxLength = 250;
		this.txOthersEmail.Name = "txOthersEmail";
		this.txOthersEmail.Size = new System.Drawing.Size(492, 21);
		this.txOthersEmail.TabIndex = 6;
		this.txOthersEmail.Validated += new System.EventHandler(txOthersEmail_Validated);
		this.ckbSendOthers.AutoSize = true;
		this.ckbSendOthers.Checked = true;
		this.ckbSendOthers.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckbSendOthers.ForeColor = System.Drawing.Color.Blue;
		this.ckbSendOthers.Location = new System.Drawing.Point(7, 89);
		this.ckbSendOthers.Name = "ckbSendOthers";
		this.ckbSendOthers.Size = new System.Drawing.Size(229, 17);
		this.ckbSendOthers.TabIndex = 5;
		this.ckbSendOthers.Text = "Enviar e-mail a outros interessados";
		this.ckbSendOthers.UseVisualStyleBackColor = true;
		this.ckbSendOthers.CheckedChanged += new System.EventHandler(ckbSendOthers_CheckedChanged);
		this.lbWarning01.AutoSize = true;
		this.lbWarning01.Font = new System.Drawing.Font("Verdana", 7f);
		this.lbWarning01.Location = new System.Drawing.Point(90, 72);
		this.lbWarning01.Name = "lbWarning01";
		this.lbWarning01.Size = new System.Drawing.Size(463, 12);
		this.lbWarning01.TabIndex = 115;
		this.lbWarning01.Text = "Para indicar mais que um endereço de e-mail, separe-os com ponto e vírgula (;).\r\n";
		this.lbPartnerEmail.AutoSize = true;
		this.lbPartnerEmail.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbPartnerEmail.Location = new System.Drawing.Point(19, 52);
		this.lbPartnerEmail.Name = "lbPartnerEmail";
		this.lbPartnerEmail.Size = new System.Drawing.Size(68, 13);
		this.lbPartnerEmail.TabIndex = 114;
		this.lbPartnerEmail.Text = "E-mail(s) :";
		this.txPartnerEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txPartnerEmail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.txPartnerEmail.Location = new System.Drawing.Point(92, 49);
		this.txPartnerEmail.Name = "txPartnerEmail";
		this.txPartnerEmail.Size = new System.Drawing.Size(492, 21);
		this.txPartnerEmail.TabIndex = 4;
		this.txPartnerEmail.Validated += new System.EventHandler(txPartnerEmail_Validated);
		this.ckbSendMySelf.AutoSize = true;
		this.ckbSendMySelf.Checked = true;
		this.ckbSendMySelf.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckbSendMySelf.Location = new System.Drawing.Point(618, 51);
		this.ckbSendMySelf.Name = "ckbSendMySelf";
		this.ckbSendMySelf.Size = new System.Drawing.Size(281, 17);
		this.ckbSendMySelf.TabIndex = 7;
		this.ckbSendMySelf.Text = "Também quero receber uma cópia do e-mail";
		this.ckbSendMySelf.UseVisualStyleBackColor = true;
		this.ckbSendMySelf.CheckedChanged += new System.EventHandler(ckbSendMySelf_CheckedChanged);
		this.ckbSendPartner.AutoSize = true;
		this.ckbSendPartner.Checked = true;
		this.ckbSendPartner.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckbSendPartner.ForeColor = System.Drawing.Color.Blue;
		this.ckbSendPartner.Location = new System.Drawing.Point(7, 30);
		this.ckbSendPartner.Name = "ckbSendPartner";
		this.ckbSendPartner.Size = new System.Drawing.Size(502, 17);
		this.ckbSendPartner.TabIndex = 3;
		this.ckbSendPartner.Text = "Enviar e-mail ao parceiro [ ##PARNAME## ] informando o evento [ ##EVENT## ]";
		this.ckbSendPartner.UseVisualStyleBackColor = true;
		this.ckbSendPartner.CheckedChanged += new System.EventHandler(ckbSendPartner_CheckedChanged);
		this.pnReason.Controls.Add(this.cbJustCode);
		this.pnReason.Controls.Add(this.lbAplicarTodos);
		this.pnReason.Controls.Add(this.lbSeparator01);
		this.pnReason.Controls.Add(this.txReason);
		this.pnReason.Controls.Add(this.lbDescription);
		this.pnReason.Location = new System.Drawing.Point(-2, 298);
		this.pnReason.Name = "pnReason";
		this.pnReason.Size = new System.Drawing.Size(923, 71);
		this.pnReason.TabIndex = 101;
		this.cbJustCode.BackColor = System.Drawing.SystemColors.Info;
		this.cbJustCode.FormattingEnabled = true;
		this.cbJustCode.Location = new System.Drawing.Point(7, 27);
		this.cbJustCode.Name = "cbJustCode";
		this.cbJustCode.Size = new System.Drawing.Size(910, 21);
		this.cbJustCode.TabIndex = 35;
		this.cbJustCode.SelectedIndexChanged += new System.EventHandler(cbJustCode_SelectedIndexChanged);
		this.cbJustCode.TextChanged += new System.EventHandler(cbJustCode_TextChanged);
		this.lbAplicarTodos.AutoSize = true;
		this.lbAplicarTodos.Location = new System.Drawing.Point(614, 10);
		this.lbAplicarTodos.Name = "lbAplicarTodos";
		this.lbAplicarTodos.Size = new System.Drawing.Size(288, 13);
		this.lbAplicarTodos.TabIndex = 34;
		this.lbAplicarTodos.TabStop = true;
		this.lbAplicarTodos.Text = "Aplicar justificativa aos registros da lista acima...";
		this.lbAplicarTodos.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbAplicarTodos_LinkClicked);
		this.lbSeparator01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator01.Location = new System.Drawing.Point(7, 2);
		this.lbSeparator01.Name = "lbSeparator01";
		this.lbSeparator01.Size = new System.Drawing.Size(912, 2);
		this.lbSeparator01.TabIndex = 33;
		this.txReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txReason.Location = new System.Drawing.Point(7, 27);
		this.txReason.MaxLength = 255;
		this.txReason.Multiline = true;
		this.txReason.Name = "txReason";
		this.txReason.Size = new System.Drawing.Size(910, 40);
		this.txReason.TabIndex = 4;
		this.txReason.TextChanged += new System.EventHandler(txReason_TextChanged);
		this.lbDescription.AutoSize = true;
		this.lbDescription.Location = new System.Drawing.Point(7, 10);
		this.lbDescription.Name = "lbDescription";
		this.lbDescription.Size = new System.Drawing.Size(228, 13);
		this.lbDescription.TabIndex = 31;
		this.lbDescription.Text = "Justificativa - Documento [ {0} - {1} ]";
		this.pnButtonData.Controls.Add(this.btImport);
		this.pnButtonData.Controls.Add(this.label10);
		this.pnButtonData.Controls.Add(this.btSair);
		this.pnButtonData.Controls.Add(this.lbSeparator02);
		this.pnButtonData.Controls.Add(this.btConfirmar);
		this.pnButtonData.Location = new System.Drawing.Point(-2, 370);
		this.pnButtonData.Name = "pnButtonData";
		this.pnButtonData.Size = new System.Drawing.Size(923, 58);
		this.pnButtonData.TabIndex = 102;
		this.btImport.Font = new System.Drawing.Font("Verdana", 8f);
		this.btImport.Image = Monitor.Resources.image_premium;
		this.btImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btImport.Location = new System.Drawing.Point(7, 14);
		this.btImport.Name = "btImport";
		this.btImport.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
		this.btImport.Size = new System.Drawing.Size(108, 31);
		this.btImport.TabIndex = 238;
		this.btImport.Text = "  &Importar A1";
		this.btImport.UseVisualStyleBackColor = true;
		this.btImport.Visible = false;
		this.btImport.Click += new System.EventHandler(btImport_Click);
		this.label10.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label10.Location = new System.Drawing.Point(6, 54);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(912, 2);
		this.label10.TabIndex = 237;
		this.btSair.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btSair.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btSair.Location = new System.Drawing.Point(713, 12);
		this.btSair.Name = "btSair";
		this.btSair.Size = new System.Drawing.Size(70, 33);
		this.btSair.TabIndex = 96;
		this.btSair.Text = "&Sair";
		this.btSair.UseVisualStyleBackColor = true;
		this.btSair.Click += new System.EventHandler(btSair_Click);
		this.lbSeparator02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator02.Location = new System.Drawing.Point(7, 2);
		this.lbSeparator02.Name = "lbSeparator02";
		this.lbSeparator02.Size = new System.Drawing.Size(912, 2);
		this.lbSeparator02.TabIndex = 95;
		this.btConfirmar.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btConfirmar.Location = new System.Drawing.Point(789, 12);
		this.btConfirmar.Name = "btConfirmar";
		this.btConfirmar.Size = new System.Drawing.Size(128, 33);
		this.btConfirmar.TabIndex = 5;
		this.btConfirmar.Text = "&Confirmar";
		this.btConfirmar.UseVisualStyleBackColor = true;
		this.btConfirmar.Click += new System.EventHandler(btConfirmar_Click);
		this.lkbDFeAutomatic.AutoSize = true;
		this.lkbDFeAutomatic.Location = new System.Drawing.Point(525, 8);
		this.lkbDFeAutomatic.Name = "lkbDFeAutomatic";
		this.lkbDFeAutomatic.Size = new System.Drawing.Size(163, 13);
		this.lkbDFeAutomatic.TabIndex = 98;
		this.lkbDFeAutomatic.TabStop = true;
		this.lkbDFeAutomatic.Text = "Clique aqui para configurar";
		this.lkbDFeAutomatic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbDFeAutomatic_LinkClicked);
		this.lbDFeAutomatic.AutoSize = true;
		this.lbDFeAutomatic.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDFeAutomatic.ForeColor = System.Drawing.Color.Blue;
		this.lbDFeAutomatic.Location = new System.Drawing.Point(38, 8);
		this.lbDFeAutomatic.Name = "lbDFeAutomatic";
		this.lbDFeAutomatic.Size = new System.Drawing.Size(472, 13);
		this.lbDFeAutomatic.TabIndex = 97;
		this.lbDFeAutomatic.Text = "Evite trabalho manual. Configure o download automático dos documentos fiscais.";
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(3, 257);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(913, 38);
		this.plnMessage.TabIndex = 103;
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
		this.pnCompany.BackColor = System.Drawing.SystemColors.Window;
		this.pnCompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnCompany.Controls.Add(this.label3);
		this.pnCompany.Controls.Add(this.lkbLoadTxtFile);
		this.pnCompany.Controls.Add(this.lbWarning);
		this.pnCompany.Controls.Add(this.txCompany);
		this.pnCompany.Controls.Add(this.lbCompany);
		this.pnCompany.Controls.Add(this.lbDocKey);
		this.pnCompany.Controls.Add(this.txDocKey);
		this.pnCompany.Controls.Add(this.btEnter);
		this.pnCompany.Location = new System.Drawing.Point(3, 4);
		this.pnCompany.Name = "pnCompany";
		this.pnCompany.Size = new System.Drawing.Size(913, 71);
		this.pnCompany.TabIndex = 104;
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(663, 46);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(239, 1);
		this.label3.TabIndex = 237;
		this.lkbLoadTxtFile.Font = new System.Drawing.Font("Verdana", 7.8f, System.Drawing.FontStyle.Italic);
		this.lkbLoadTxtFile.Location = new System.Drawing.Point(661, 50);
		this.lkbLoadTxtFile.Name = "lkbLoadTxtFile";
		this.lkbLoadTxtFile.Size = new System.Drawing.Size(239, 13);
		this.lkbLoadTxtFile.TabIndex = 234;
		this.lkbLoadTxtFile.TabStop = true;
		this.lkbLoadTxtFile.Text = "Importar lista de chaves (.txt)";
		this.lkbLoadTxtFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbLoadTxtFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbLoadTxtFile_LinkClicked);
		this.lbWarning.Font = new System.Drawing.Font("Verdana", 7.8f, System.Drawing.FontStyle.Italic);
		this.lbWarning.Location = new System.Drawing.Point(661, 3);
		this.lbWarning.Name = "lbWarning";
		this.lbWarning.Size = new System.Drawing.Size(239, 39);
		this.lbWarning.TabIndex = 233;
		this.lbWarning.Text = "Use CTRL C e CTRL V para colar várias chaves no campo [Chave].";
		this.lbWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txCompany.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txCompany.Location = new System.Drawing.Point(90, 8);
		this.txCompany.Name = "txCompany";
		this.txCompany.Size = new System.Drawing.Size(410, 14);
		this.txCompany.TabIndex = 154;
		this.txCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbCompany.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbCompany.Location = new System.Drawing.Point(7, 8);
		this.lbCompany.Name = "lbCompany";
		this.lbCompany.Size = new System.Drawing.Size(77, 14);
		this.lbCompany.TabIndex = 153;
		this.lbCompany.Text = "Empresa :";
		this.lbCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbDocKey.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDocKey.Location = new System.Drawing.Point(7, 37);
		this.lbDocKey.Name = "lbDocKey";
		this.lbDocKey.Size = new System.Drawing.Size(77, 14);
		this.lbDocKey.TabIndex = 150;
		this.lbDocKey.Text = "Chave :";
		this.lbDocKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txDocKey.BackColor = System.Drawing.SystemColors.Info;
		this.txDocKey.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txDocKey.Location = new System.Drawing.Point(89, 34);
		this.txDocKey.MaxLength = 255;
		this.txDocKey.Name = "txDocKey";
		this.txDocKey.Size = new System.Drawing.Size(390, 22);
		this.txDocKey.TabIndex = 1;
		this.txDocKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txDocKey.KeyDown += new System.Windows.Forms.KeyEventHandler(txDocKey_KeyDown);
		this.btEnter.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btEnter.Location = new System.Drawing.Point(485, 32);
		this.btEnter.Name = "btEnter";
		this.btEnter.Size = new System.Drawing.Size(60, 25);
		this.btEnter.TabIndex = 2;
		this.btEnter.Text = "&Enter";
		this.btEnter.UseVisualStyleBackColor = true;
		this.btEnter.Click += new System.EventHandler(btEnter_Click);
		this.toolTipScreen.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTipScreen.ToolTipTitle = "Informações adicionais";
		this.contextMenuAction.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.contextMenuAction.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.tsmRunOnline, this.toolStripSeparator1, this.tsmRunBackground });
		this.contextMenuAction.Name = "contextMenuFilial";
		this.contextMenuAction.Size = new System.Drawing.Size(234, 54);
		this.tsmRunOnline.Image = Monitor.Resources.image_process_done;
		this.tsmRunOnline.Name = "tsmRunOnline";
		this.tsmRunOnline.Size = new System.Drawing.Size(233, 22);
		this.tsmRunOnline.Text = "Executar agora (online)";
		this.tsmRunOnline.Click += new System.EventHandler(tsmRunOnline_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(230, 6);
		this.tsmRunBackground.Image = Monitor.Resources.image_schedule;
		this.tsmRunBackground.Name = "tsmRunBackground";
		this.tsmRunBackground.Size = new System.Drawing.Size(233, 22);
		this.tsmRunBackground.Text = "Executar em segundo plano";
		this.tsmRunBackground.Click += new System.EventHandler(tsmRunBackground_Click);
		this.lsvDataDoc.AllColumns.Add(this.olvSelect);
		this.lsvDataDoc.AllColumns.Add(this.olvChave);
		this.lsvDataDoc.AllColumns.Add(this.olvDcNum);
		this.lsvDataDoc.AllColumns.Add(this.olvtpEvento);
		this.lsvDataDoc.AllColumns.Add(this.olvxEvento);
		this.lsvDataDoc.AllColumns.Add(this.olvxJust);
		this.lsvDataDoc.AllColumns.Add(this.olvcStat);
		this.lsvDataDoc.AllColumns.Add(this.olvxMotivo);
		this.lsvDataDoc.CellEditUseWholeCell = false;
		this.lsvDataDoc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[8] { this.olvSelect, this.olvChave, this.olvDcNum, this.olvtpEvento, this.olvxEvento, this.olvxJust, this.olvcStat, this.olvxMotivo });
		this.lsvDataDoc.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvDataDoc.EmptyListMsg = "";
		this.lsvDataDoc.EmptyListMsgFont = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvDataDoc.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvDataDoc.FullRowSelect = true;
		this.lsvDataDoc.HideSelection = false;
		this.lsvDataDoc.Location = new System.Drawing.Point(3, 78);
		this.lsvDataDoc.MenuLabelColumns = "Colunas";
		this.lsvDataDoc.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lsvDataDoc.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lsvDataDoc.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lsvDataDoc.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lsvDataDoc.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lsvDataDoc.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lsvDataDoc.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lsvDataDoc.MenuLabelUnsort = "Remover ordenação";
		this.lsvDataDoc.Name = "lsvDataDoc";
		this.lsvDataDoc.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.ModelDialog;
		this.lsvDataDoc.ShowCommandMenuOnRightClick = true;
		this.lsvDataDoc.ShowGroups = false;
		this.lsvDataDoc.ShowImagesOnSubItems = true;
		this.lsvDataDoc.ShowItemCountOnGroups = true;
		this.lsvDataDoc.Size = new System.Drawing.Size(913, 217);
		this.lsvDataDoc.SmallImageList = this.ImageListDocs;
		this.lsvDataDoc.SortGroupItemsByPrimaryColumn = false;
		this.lsvDataDoc.SpaceBetweenGroups = 5;
		this.lsvDataDoc.TabIndex = 235;
		this.lsvDataDoc.TintSortColumn = true;
		this.lsvDataDoc.UseCellFormatEvents = true;
		this.lsvDataDoc.UseCompatibleStateImageBehavior = false;
		this.lsvDataDoc.UseFilterIndicator = true;
		this.lsvDataDoc.UseFiltering = true;
		this.lsvDataDoc.UseHotControls = false;
		this.lsvDataDoc.UseHyperlinks = true;
		this.lsvDataDoc.View = System.Windows.Forms.View.Details;
		this.lsvDataDoc.VirtualMode = true;
		this.lsvDataDoc.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(lsvData_ColumnReordered);
		this.lsvDataDoc.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lsvData_ColumnWidthChanged);
		this.lsvDataDoc.SelectedIndexChanged += new System.EventHandler(lsvDataDoc_SelectedIndexChanged);
		this.olvSelect.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelect.Groupable = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 30;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "Chave";
		this.olvChave.Width = 2;
		this.olvDcNum.AspectName = "";
		this.olvDcNum.Text = "Documento";
		this.olvDcNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDcNum.Width = 78;
		this.olvtpEvento.AspectName = "tpEvento";
		this.olvtpEvento.Text = "Evento";
		this.olvtpEvento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvxEvento.AspectName = "xEvento";
		this.olvxEvento.Text = "Descrição";
		this.olvxEvento.Width = 210;
		this.olvxJust.AspectName = "xJust";
		this.olvxJust.Text = "Justificativa";
		this.olvxJust.Width = 200;
		this.olvcStat.AspectName = "cStat";
		this.olvcStat.Text = "Status";
		this.olvcStat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvcStat.Width = 48;
		this.olvxMotivo.AspectName = "xMotivo";
		this.olvxMotivo.FillsFreeSpace = true;
		this.olvxMotivo.Text = "Motivo";
		this.olvxMotivo.Width = 250;
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_schedule.jpg");
		this.ImageListDocs.Images.SetKeyName(1, "image_working.png");
		this.ImageListDocs.Images.SetKeyName(2, "image_error.png");
		this.ImageListDocs.Images.SetKeyName(3, "image_clock_go_32.png");
		this.ImageListDocs.Images.SetKeyName(4, "dfe_confirm.png");
		this.ImageListDocs.Images.SetKeyName(5, "image_warning.png");
		this.ImageListDocs.Images.SetKeyName(6, "image_logger.png");
		this.ImageListDocs.Images.SetKeyName(7, "image_pause.png");
		this.pnConfig.Controls.Add(this.label9);
		this.pnConfig.Controls.Add(this.picDFeAutomatic);
		this.pnConfig.Controls.Add(this.lkbDFeAutomatic);
		this.pnConfig.Controls.Add(this.lbDFeAutomatic);
		this.pnConfig.Location = new System.Drawing.Point(-2, 429);
		this.pnConfig.Name = "pnConfig";
		this.pnConfig.Size = new System.Drawing.Size(924, 32);
		this.pnConfig.TabIndex = 238;
		this.label9.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label9.Location = new System.Drawing.Point(6, 28);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(912, 2);
		this.label9.TabIndex = 123;
		this.picDFeAutomatic.Image = Monitor.Resources.image_tip;
		this.picDFeAutomatic.Location = new System.Drawing.Point(9, 4);
		this.picDFeAutomatic.Name = "picDFeAutomatic";
		this.picDFeAutomatic.Size = new System.Drawing.Size(20, 20);
		this.picDFeAutomatic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picDFeAutomatic.TabIndex = 100;
		this.picDFeAutomatic.TabStop = false;
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Location = new System.Drawing.Point(97, 141);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(593, 46);
		this.panel1.TabIndex = 239;
		this.panel1.Visible = false;
		this.pictureBox1.Image = Monitor.Resources.image_tip;
		this.pictureBox1.Location = new System.Drawing.Point(9, 13);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(20, 20);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 238;
		this.pictureBox1.TabStop = false;
		this.pnForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnForm.Controls.Add(this.pnContent);
		this.pnForm.Controls.Add(this.pnTitleBar);
		this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnForm.Location = new System.Drawing.Point(0, 0);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(936, 654);
		this.pnForm.TabIndex = 240;
		this.pnContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnContent.Controls.Add(this.panel1);
		this.pnContent.Controls.Add(this.pnCompany);
		this.pnContent.Controls.Add(this.pnEmail);
		this.pnContent.Controls.Add(this.pnConfig);
		this.pnContent.Controls.Add(this.pnReason);
		this.pnContent.Controls.Add(this.pnButtonData);
		this.pnContent.Controls.Add(this.plnMessage);
		this.pnContent.Controls.Add(this.lsvDataDoc);
		this.pnContent.Location = new System.Drawing.Point(6, 30);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(922, 618);
		this.pnContent.TabIndex = 8;
		this.pnTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnTitleBar.BackColor = System.Drawing.Color.White;
		this.pnTitleBar.Controls.Add(this.picTitleBar);
		this.pnTitleBar.Controls.Add(this.btHelp);
		this.pnTitleBar.Controls.Add(this.btClose);
		this.pnTitleBar.Controls.Add(this.lbTitleBar);
		this.pnTitleBar.Location = new System.Drawing.Point(0, 0);
		this.pnTitleBar.Name = "pnTitleBar";
		this.pnTitleBar.Size = new System.Drawing.Size(935, 31);
		this.pnTitleBar.TabIndex = 2;
		this.picTitleBar.ErrorImage = null;
		this.picTitleBar.Image = Monitor.Resources.image_favicon;
		this.picTitleBar.InitialImage = null;
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
		this.btHelp.Location = new System.Drawing.Point(824, 3);
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
		this.btClose.Location = new System.Drawing.Point(905, 2);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 1;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lbTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTitleBar.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitleBar.Location = new System.Drawing.Point(32, 5);
		this.lbTitleBar.Name = "lbTitleBar";
		this.lbTitleBar.Size = new System.Drawing.Size(786, 20);
		this.lbTitleBar.TabIndex = 0;
		this.lbTitleBar.Text = "Fiscal.io : ";
		this.lbTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(lbTitleBar_MouseDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(936, 654);
		base.Controls.Add(this.pnForm);
		base.Controls.Add(this.lbPartnerName);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmEventConfirm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io : ";
		base.Shown += new System.EventHandler(frmEventConfirm_Shown);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(frmEventConfirm_HelpRequested);
		base.Leave += new System.EventHandler(frmEventConfirm_Leave);
		this.pnEmail.ResumeLayout(false);
		this.pnEmail.PerformLayout();
		this.pnReason.ResumeLayout(false);
		this.pnReason.PerformLayout();
		this.pnButtonData.ResumeLayout(false);
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		this.pnCompany.ResumeLayout(false);
		this.pnCompany.PerformLayout();
		this.contextMenuAction.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lsvDataDoc).EndInit();
		this.pnConfig.ResumeLayout(false);
		this.pnConfig.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picDFeAutomatic).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.pnForm.ResumeLayout(false);
		this.pnContent.ResumeLayout(false);
		this.pnTitleBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
