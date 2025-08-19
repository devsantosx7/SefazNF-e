using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmDocSearch : Form
{
	private List<FilialView> varclsFilialList = new List<FilialView>();

	private List<clsObjecSearch> varclsDocList = new List<clsObjecSearch>();

	private clsDataDoc varclsDataDoc = new clsDataDoc();

	private const int cnt_icon_initial = 0;

	private const int cnt_icon_sucess = 1;

	private const int cnt_icon_error = 2;

	private const int cnt_icon_xml_error = 3;

	public string UserAction = string.Empty;

	private bool _ColumnsLoaded;

	private clsFeatureService varclsFeatService = new clsFeatureService();

	private IContainer components;

	private TextBox txDocKey;

	private Label lbScreenTitle;

	private Label lbDocKey;

	private Button btEnter;

	private Label lbSeparator02;

	private Label lbWarning;

	private LinkLabel lkbLoadTxtFile;

	private Label label1;

	private Label label2;

	private Label label3;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private ToolTip toolTipInfAdd;

	private FastObjectListView lsvDataDoc;

	private OLVColumn olvSelect;

	private OLVColumn olvDcKey;

	private OLVColumn olvDcType;

	private OLVColumn olvStatus;

	private ImageList ImageListDoc;

	private ToolStrip toolStrip1;

	private ToolStripButton tsbFiltrar;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton tsbExportar;

	private Label label4;

	private ToolStripButton tsbExport;

	private ToolStripButton tsbSendDoc;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton tsbDownloadDFe;

	private ToolStripSeparator toolStripSeparator8;

	private ToolStripSeparator toolStripSeparator5;

	private Panel pnForm;

	private Panel pnContent;

	private Panel pnTitleBar;

	private Button btMaximize;

	private PictureBox picTitleBar;

	private Button btHelp;

	private Button btClose;

	private Label lbTitleBar;

	private OLVColumn olvFilial;

	public frmDocSearch()
	{
		InitializeComponent();
		if (clsFunction.IsAdmin)
		{
			lbTitleBar.Text = base.Name + " | " + lbTitleBar.Text;
		}
		lsvDataDoc.HandleDestroyed += LsvDataDoc_HandleDestroyed;
	}

	private void LsvDataDoc_HandleDestroyed(object sender, EventArgs e)
	{
		if (_ColumnsLoaded && lsvDataDoc != null)
		{
			clsListViewUtils.funcSaveColumnsOrderAsDisplayed(lsvDataDoc, base.Name);
		}
	}

	public frmDocSearch(List<FilialView> pclsFilialList)
	{
		InitializeComponent();
		varclsFilialList = pclsFilialList;
		if (varclsFilialList.Count != 1)
		{
			lbScreenTitle.Text = "Empresa : Conforme seleção de dados";
		}
		else
		{
			FilialView varclsFilial = pclsFilialList.FirstOrDefault();
			lbScreenTitle.Text = "Empresa : [ " + clsFunction.funcFormatDoc(varclsFilial.CNPJView) + " ] - " + varclsFilial.NomeView;
		}
		_ColumnsLoaded = false;
		clsScreenGeral.funcSetColumnsObjectListView(this, lsvDataDoc);
		_ColumnsLoaded = true;
		foreach (OLVColumn allColumn in lsvDataDoc.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
		if (clsFunction.IsAdmin)
		{
			clsScreenGeral.funcCheckListViewDdic(typeof(clsObjecSearch), lsvDataDoc);
		}
		funcDefineListViewFeatures();
	}

	private void funcDefineListViewFeatures()
	{
		olvSelect.ImageGetter = delegate(object x)
		{
			clsObjecSearch clsObjecSearch = (clsObjecSearch)x;
			return (clsObjecSearch == null) ? null : ((object)clsObjecSearch.DcIcon);
		};
	}

	private async void frmDocSearch_Load(object sender, EventArgs e)
	{
		clsFeatureService clsFeatureService = new clsFeatureService();
		string varFeatureIdCode = clsFeatureService.consFeatXmlJuridicValidation;
		await clsFeatureService.funcGetFeatTypeAsync(varFeatureIdCode);
		funcLoadListView();
	}

	private void btSair_Click(object sender, EventArgs e)
	{
		Close();
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

	private async void btEnter_Click(object sender, EventArgs e)
	{
		clsFunction.funcClearAllSpaces(Regex.Replace(txDocKey.Text, "[^0-9 ]", ""));
		clsReturn varclsReturnFunc = await funcAddItemAsync(txDocKey.Text);
		txDocKey.Text = string.Empty;
		funcLoadListView();
		funcHideProgress();
		funcSetFocusItem(new clsReturn());
		if (varclsReturnFunc.HasError)
		{
			funcShowErrorMessage(varclsReturnFunc);
		}
	}

	private void funcShowErrorMessage(clsReturn pclsReturn)
	{
		if (base.Visible && pclsReturn != null && (pclsReturn.HasError || pclsReturn.HasWarning))
		{
			clsScreenGeral.funcShowUserMessage(this, pclsReturn);
		}
	}

	private void funcSetFocusItem(clsReturn pclsReturn)
	{
		if (!pclsReturn.HasError && !pclsReturn.HasWarning && lsvDataDoc.Items.Count > 0)
		{
			lsvDataDoc.SelectedIndex = lsvDataDoc.Items.Count - 1;
			lsvDataDoc.SelectedItem.Focused = true;
			lsvDataDoc.SelectedItem.EnsureVisible();
		}
	}

	private void funcHideProgress()
	{
		lbProgress.Text = string.Empty;
		plnMessage.Visible = false;
		Application.DoEvents();
	}

	private void funcShowProgress(string pMessage)
	{
		lbProgress.Text = pMessage;
		plnMessage.Visible = true;
		Application.DoEvents();
	}

	private void funcShowProgress(double pTotal, double pCounter, string pMessage)
	{
		if (pCounter % 200.0 == 0.0)
		{
			int varPercent = Convert.ToInt32(pCounter / pTotal * 100.0);
			lbProgress.Text = $"{pMessage} [ {varPercent} % ] ";
			plnMessage.Visible = true;
			Application.DoEvents();
		}
	}

	private async Task<clsReturn> funcLoadKeyListAsync(string pKeyList)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			funcShowProgress("Aguarde, as informações estão sendo carregadas ...");
			funcShowProgress("Lendo o conteúdo copiado ...");
			funcShowProgress("Calculando o total de linhas ...");
			StringReader varReaderData01 = new StringReader(pKeyList);
			_ = string.Empty;
			double varTotalList = 0.0;
			double varCounter = 0.0;
			while (varReaderData01.ReadLine() != null)
			{
				varTotalList += 1.0;
			}
			funcShowProgress("Processando o conteúdo copiado ...");
			StringReader varReaderData2 = new StringReader(pKeyList);
			string varStrLine;
			while ((varStrLine = varReaderData2.ReadLine()) != null)
			{
				funcShowProgress(varTotalList, varCounter, "Processando o conteúdo copiado ...");
				varCounter += 1.0;
				varclsReturnFunc.AddRange((await funcAddItemAsync(varStrLine)).Messages);
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		funcHideProgress();
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcAddItemAsync(string pDocKey)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDFeNumService varclsDFeService = new clsDFeNumService();
		try
		{
			string varDocKey = Regex.Replace(pDocKey, "[^0-9 ]", "");
			varDocKey = clsFunction.funcClearAllSpaces(varDocKey);
			if (clsFunction.IsEmpty(varDocKey))
			{
				return varclsReturnFunc;
			}
			if (varclsDocList.Any((clsObjecSearch r) => r.DocKey.Equals(varDocKey)))
			{
				return varclsReturnFunc;
			}
			clsObjecSearch varDocItem = new clsObjecSearch();
			if (clsFunction.IsAdmin && varDocKey.Length == 43)
			{
				clsDFeNumService varclsDFe = new clsDFeNumService();
				varDocKey += varclsDFe.funcGetDigit(varDocKey);
			}
			varDocItem.DocKey = varDocKey;
			string varDocModel = clsFunction.funcGetDFeModel(varDocKey);
			_ = string.Empty;
			if (clsFunction.funcIsNFSe(varDocModel) && varDocKey.Length != 50)
			{
				varDocItem.Action = "NO-ACTION";
				varDocItem.Status = "Chave inválida. Tamanho diferente de 50 digitos.";
			}
			else if (!clsFunction.funcIsNFSe(varDocModel) && varDocKey.Length != 44)
			{
				varDocItem.Action = "NO-ACTION";
				varDocItem.Status = "Chave inválida. Tamanho diferente de 44 digitos.";
			}
			else if (!varclsDFeService.funcIsDigitOk(varDocKey))
			{
				varDocItem.Action = "NO-ACTION";
				varDocItem.Status = "Chave inválida. Digito verificador incorreto. {varDocKey}";
			}
			List<clsObjecSearch> varclslistDocItem = await funcFillDocListAsync(varDocItem);
			varclsDocList.AddRange(varclslistDocItem);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	public List<string> funcGetDocKeyList()
	{
		List<string> varDocKeyList = new List<string>();
		foreach (clsObjecSearch varDocItem in varclsDocList)
		{
			varDocKeyList.Add(varDocItem.DocKey);
		}
		return varDocKeyList;
	}

	private async Task<List<clsObjecSearch>> funcFillDocListAsync(clsObjecSearch pObject)
	{
		List<clsObjecSearch> varclsListObjects = new List<clsObjecSearch>();
		string varDocModel = clsFunction.funcGetDFeModel(pObject.DocKey);
		bool varIsDownload = clsFunction.funcIsNFe(varDocModel) || clsFunction.funcIsCTe(varDocModel) || clsFunction.funcIsNFCe(varDocModel) || clsFunction.funcIsNFSe(varDocModel);
		List<Document> varDocList = await varclsDataDoc.funcGetListKeyAsync(varclsFilialList, pObject.DocKey);
		if (varDocList == null)
		{
			varDocList = new List<Document>();
		}
		pObject.DcType = clsFunction.funcGetDocType(varDocModel);
		pObject.DocKey = pObject.DocKey;
		pObject.Action = clsFunction.funcGetValue(pObject.Action);
		pObject.Status = clsFunction.funcGetValue(pObject.Status);
		if (pObject.Action.Equals("NO-ACTION"))
		{
			pObject.DcIcon = 2;
			varclsListObjects.Add(pObject);
			return varclsListObjects;
		}
		if (varDocList.Count == 0)
		{
			if (varIsDownload)
			{
				pObject.DcIcon = 0;
				pObject.Action = "DOWN-FULL";
				pObject.Status = "Ação : Documento não encontrado. Baixe via opção [Download de XML]";
			}
			else
			{
				pObject.DcIcon = 0;
				pObject.Action = "UPLD-FULL";
				pObject.Status = "Ação : Doc. não encontrado. Carregue via opção [Importar]";
			}
			varclsListObjects.Add(pObject);
		}
		if (varclsListObjects.Count > 0)
		{
			return varclsListObjects;
		}
		foreach (Document varclsDocItem in varDocList)
		{
			clsObjecSearch varclsModel = pObject.GetClone();
			varclsModel.Filial = varclsDocItem.Filial;
			if (!clsFunction.IsEmpty(varclsDocItem.XmlError) && clsFunction.IsEmpty(varclsDocItem.FisIoApi))
			{
				varclsModel.DcIcon += 2;
				if (varIsDownload)
				{
					varclsModel.Action = "DOWN-XML";
					varclsModel.Status = "Ação : XML sem validade jurídica. Baixe via opção [Download de XML]";
				}
				else
				{
					varclsModel.Action = "UPLD-XML";
					varclsModel.Status = "Ação : XML sem validade jurídica. Carregue via opção [Importar]";
				}
				varclsListObjects.Add(varclsModel);
				continue;
			}
			if (!clsFunction.IsEmpty(varclsDocItem.HasXml))
			{
				varclsModel.DcIcon = 1;
				varclsModel.Action = string.Empty;
				varclsModel.Status = "Documento e XML já existe no Fiscal.io Monitor";
				varclsListObjects.Add(varclsModel);
				continue;
			}
			varclsModel.DcIcon = 0;
			string varDocCancel = string.Empty;
			if (!clsFunction.IsEmpty(varclsDocItem.Canceled) || !clsFunction.IsEmpty(varclsDocItem.HasCancelEvent))
			{
				varDocCancel = "  [Doc.Cancelado]";
			}
			if (varIsDownload)
			{
				varclsModel.Action = "DOWN-XML";
				varclsModel.Status = "Ação : XML não encontrado" + varDocCancel + ". Baixe via opção [Download de XML]";
			}
			else
			{
				varclsModel.Action = "UPLD-XML";
				varclsModel.Status = "Ação : XML não encontrado" + varDocCancel + ". Carregue via opção [Importar]";
			}
			varclsListObjects.Add(varclsModel);
		}
		return varclsListObjects;
	}

	private clsMessage funcGetErrorInvalidContent(string pFileName)
	{
		string varMesssage = "O conteúdo do arquivo é inválido." + Environment.NewLine + "Arquivo : " + pFileName + Environment.NewLine;
		return new clsMessage("E", "9999", varMesssage);
	}

	private async Task<Event> funcGetEventAsync(string pFilial, string pChave, string pEvent)
	{
		return await new clsDataEvent().funcGetItemByChaveTpEventoAsync(pChave, pEvent);
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

	private async Task<clsReturn> funcLoadFileListAsync(string[] pFileList)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			funcShowProgress("Aguarde, as informações estão sendo carregadas ...");
			foreach (string varFilePath in pFileList)
			{
				string varFileName = new FileInfo(varFilePath).Name;
				funcShowProgress("Arquivo " + varFileName + " : Lendo conteúdo ...");
				if (!File.Exists(varFilePath))
				{
					continue;
				}
				string varFileContent = File.ReadAllText(varFilePath);
				string[] varDocKeyList = (varFileContent.Contains(Environment.NewLine) ? varFileContent.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None) : ((!varFileContent.Contains(";")) ? varFileContent.Split(' ') : varFileContent.Split(';')));
				funcShowProgress("Arquivo " + varFileName + " : Processando conteúdo ...");
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
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		funcHideProgress();
		return varclsReturnFunc;
	}

	private void funcLoadListView()
	{
		lsvDataDoc.BeginUpdate();
		lsvDataDoc.SuspendLayout();
		new clsDataBatchHead();
		new List<BatchHead>();
		lsvDataDoc.SetObjects(varclsDocList);
		lsvDataDoc.ShowGroups = true;
		lsvDataDoc.BuildGroups(olvDcType, SortOrder.Ascending, olvDcType, SortOrder.Ascending, olvStatus, SortOrder.Ascending);
		lsvDataDoc.EndUpdate();
		lsvDataDoc.ResumeLayout();
	}

	private void lsvData_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
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

	private async Task<List<Document>> funcGetDocListAsync()
	{
		clsDataDoc varclsDataDoc = new clsDataDoc();
		List<Document> varDocList = new List<Document>();
		funcShowProgress("Preparando lista de documentos ...");
		double varCounter = 0.0;
		double varTotalList = varclsDocList.Count;
		foreach (clsObjecSearch varDocItem in varclsDocList)
		{
			funcShowProgress(varTotalList, varCounter, "Preparando lista de documentos ...");
			varCounter += 1.0;
			varDocItem.Action = clsFunction.funcGetValue(varDocItem.Action);
			if (!varDocItem.Action.Contains("XML"))
			{
				Document varclsDoc = await varclsDataDoc.funcGetItemByChaveHasXmlAsync(varDocItem.DocKey);
				if (varclsDoc != null)
				{
					varDocList.Add(varclsDoc);
				}
			}
		}
		string varFeatExtId = clsFeatureService.consFeatScanDocNFSeIn;
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId, pIfNotFoundRetLock: false), "LOCK"))
		{
			int varTotalNFSe = varDocList.Count((Document r) => clsFunction.funcIsNFSe(r.Model));
			if (varTotalNFSe == varDocList.Count && varTotalNFSe > 0)
			{
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
				varDocList.Clear();
			}
			else
			{
				varDocList = varDocList.Where((Document r) => !clsFunction.funcIsNFSe(r.Model)).ToList();
			}
		}
		varFeatExtId = clsFeatureService.consFeatScanDocCFeOut;
		if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId, pIfNotFoundRetLock: false), "LOCK"))
		{
			int varTotalNFSe2 = varDocList.Count((Document r) => clsFunction.funcIsCFeSat(r.Model));
			if (varTotalNFSe2 == varDocList.Count && varTotalNFSe2 > 0)
			{
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
				varDocList.Clear();
			}
			else
			{
				varDocList = varDocList.Where((Document r) => !clsFunction.funcIsCFeSat(r.Model)).ToList();
			}
		}
		funcHideProgress();
		return varDocList;
	}

	private async Task<clsReturn> funcPlanEventAsync(clsEventData pclsEvtData)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsEvtService varclsEvtService = new clsEvtService();
		clsDataDoc varclsDataDoc = new clsDataDoc();
		try
		{
			funcShowProgress("Gerando eventos de download ...");
			double varCounter = 0.0;
			double varTotalList = varclsDocList.Count;
			FilialView varclsFilialDefault = pclsEvtData.FilialList.FirstOrDefault();
			FilialView varclsFilialItem = new FilialView();
			foreach (clsObjecSearch varDocItem in varclsDocList)
			{
				funcShowProgress(varTotalList, varCounter, "Gerando eventos de download ...");
				varCounter += 1.0;
				if (clsFunction.IsEmpty(varDocItem.Action) || !clsFunction.Contains(varDocItem.Action, "DOWN-"))
				{
					continue;
				}
				varclsFilialItem = (clsFunction.IsEqual(varDocItem.Filial, varclsFilialItem.CNPJ, "") ? varclsFilialDefault.GetClone() : (await clsSrvGeral.funcGetFilialAsync(varDocItem.Filial)));
				Document varclsDoc = null;
				if (clsFunction.Contains(varDocItem.Action, "-FULL"))
				{
					varclsDoc = varclsDataDoc.funcGetDocFromKey(varclsFilialItem, varDocItem.DocKey);
				}
				else if (clsFunction.Contains(varDocItem.Action, "-XML"))
				{
					if (!clsFunction.IsEmpty(varDocItem.Filial))
					{
						varclsDoc = await varclsDataDoc.funcGetItemByKeyAsync(varDocItem.Filial, varDocItem.DocKey);
					}
					else
					{
						varclsDoc = await varclsDataDoc.funcGetItemByChaveAsync(varDocItem.DocKey);
					}
				}
				if (varclsDoc == null)
				{
					continue;
				}
				pclsEvtData.DocList.Add(varclsDoc);
				if (!pclsEvtData.FilialList.Any((FilialView r) => r.CNPJ.Equals(varclsDoc.Filial)))
				{
					FilialView varclsBuffer = await clsSrvGeral.funcGetFilialAsync(varclsDoc.Filial);
					if (varclsBuffer != null)
					{
						pclsEvtData.FilialList.Add(varclsBuffer);
					}
				}
				string varDocType = clsFunction.funcGetDocType(varclsDoc.Model);
				pclsEvtData.TypeList.TryGetValue(varDocType, out var varEventType);
				if (!clsFunction.IsEmpty(varEventType))
				{
					List<Event> varEvtList = await varclsEvtService.funcNewEventAsync(varEventType, varclsFilialItem, varclsDoc, "Manual");
					pclsEvtData.EvtList.AddRange(varEvtList);
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		funcHideProgress();
		if (varclsReturnFunc.HasError)
		{
			funcShowErrorMessage(varclsReturnFunc);
		}
		else
		{
			new frmEventConfirm(pclsEvtData).ShowDialog(this);
			Close();
		}
		return varclsReturnFunc;
	}

	private void tsbFiltrar_Click(object sender, EventArgs e)
	{
		UserAction = "FILTER_BY_KEY";
		Close();
	}

	private async void tsbDownloadDFe_Click(object sender, EventArgs e)
	{
		clsEvtService varclsEvtService = new clsEvtService();
		clsDataFilial varclsDataFilial = new clsDataFilial();
		clsEventData varclsEvtData = new clsEventData
		{
			EvtUserType = "DOWNLOAD",
			FilialList = varclsFilialList,
			ForceScreen = true
		};
		if (varclsDocList.Any((clsObjecSearch r) => clsFunction.IsEmpty(r.Filial)))
		{
			varclsEvtData = varclsEvtService.funcGetFilialForEvent(varclsEvtData, varclsDocList);
			if (varclsEvtData.Cancel)
			{
				return;
			}
		}
		List<string> varFilialListStr = varclsDocList.Select((clsObjecSearch r) => r.Filial).Distinct().ToList();
		clsEventData clsEventData = varclsEvtData;
		clsEventData.FilialList = await varclsDataFilial.funcGetListByListAsync(varFilialListStr);
		if (varclsDocList.Any((clsObjecSearch r) => clsFunction.IsEqual(r.DcType, "NFe")))
		{
			varclsEvtData = varclsEvtService.funcGetDownEvtType(this, varclsEvtData, "NFe");
		}
		if (varclsDocList.Any((clsObjecSearch r) => clsFunction.IsEqual(r.DcType, "NFCe")))
		{
			varclsEvtData = varclsEvtService.funcGetDownEvtType(this, varclsEvtData, "NFCe");
		}
		if (varclsDocList.Any((clsObjecSearch r) => clsFunction.IsEqual(r.DcType, "CTe")))
		{
			varclsEvtData = varclsEvtService.funcGetDownEvtType(this, varclsEvtData, "CTe");
		}
		if (varclsDocList.Any((clsObjecSearch r) => clsFunction.IsEqual(r.DcType, "NFSe")))
		{
			varclsEvtData = varclsEvtService.funcGetDownEvtType(this, varclsEvtData, "NFSe");
			string varFeatExtId = clsFeatureService.consFeatScanDocNFSeIn;
			if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId, pIfNotFoundRetLock: false), "LOCK"))
			{
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
				return;
			}
		}
		if (varclsDocList.Any((clsObjecSearch r) => clsFunction.IsEqual(r.DcType, "CFeSat")))
		{
			varclsEvtData = varclsEvtService.funcGetDownEvtType(this, varclsEvtData, "CFeSat");
			string varFeatExtId = clsFeatureService.consFeatScanDocCFeOut;
			if (clsFunction.Contains(await varclsFeatService.funcGetFeatTypeAsync(varFeatExtId, pIfNotFoundRetLock: false), "LOCK"))
			{
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
				return;
			}
		}
		if (!varclsEvtData.Cancel)
		{
			clsReturn varclsReturnFunc = await varclsEvtService.funcCheckAuthorizationAsync(varclsEvtData);
			if (!varclsReturnFunc.HasError && varclsReturnFunc.ActionDone)
			{
				await funcPlanEventAsync(varclsEvtData);
			}
		}
	}

	private async void tsbExportar_Click(object sender, EventArgs e)
	{
		_ = lsvDataDoc.Objects;
		await clsScreenGeral.funcExportDocToExcelAsync<clsObjecSearch>(lsvDataDoc);
	}

	private async void tsbExport_Click(object sender, EventArgs e)
	{
		await funcExportDocsAsync(enExportScreen.DefaultData);
	}

	private async Task<bool> funcExportDocsAsync(enExportScreen pScreen)
	{
		List<Document> varDocList = await funcGetDocListAsync();
		if (varDocList.Count <= 0)
		{
			MessageBox.Show(this, "Nenhum documento com arquivo XML para exportar.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		if (!(await clsScreenGeral.funcHasAccessAsync("DFE-EXPORT-MASS", "EXECUTE")))
		{
			return false;
		}
		frmDynFolder varfrmDynFolder = new frmDynFolder(pSetUserData: true, pScreen);
		if (varfrmDynFolder.ShowDialog().Equals(DialogResult.Cancel))
		{
			return false;
		}
		clsExportData varUserData = varfrmDynFolder.funcGetData();
		new List<clsObjectType>();
		clsExportService clsExportService = new clsExportService();
		clsExportService.EventLongRunner += funcEventLongRunner;
		clsReturn varclsReturnFunc = await clsExportService.funcSendDocsAsync(varDocList, varUserData);
		funcHideProgress();
		if (varclsReturnFunc.HasError)
		{
			funcShowErrorMessage(varclsReturnFunc);
			return false;
		}
		string varUserMessage = varclsReturnFunc.FirstMessage();
		if (clsFunction.IsEmpty(varUserMessage))
		{
			varUserMessage = "Exportação realizada com sucesso.";
		}
		MessageBox.Show(varUserMessage, "Exportação de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		return true;
	}

	private void funcEventLongRunner(object sender, EventLongRunnerEventArgs e)
	{
		if (e != null && !clsFunction.IsEmpty(e.RetMessage))
		{
			funcShowProgress(e.RetMessage);
		}
	}

	private async void funcOpenChannelManager(string pUserAction)
	{
		if (await clsScreenGeral.funcHasAccessAsync("CHANNEL-MANAGER"))
		{
			frmChannelManager frmChannelManager = new frmChannelManager(pUserAction);
			frmChannelManager.ShowDialog(this);
			frmChannelManager.Dispose();
		}
	}

	private async void tsbSendDoc_Click(object sender, EventArgs e)
	{
		List<Document> varDocList = await funcGetDocListAsync();
		if (varDocList.Count <= 0)
		{
			MessageBox.Show(this, "Nenhum documento com arquivo XML para enviar.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (await clsScreenGeral.funcHasAccessAsync("EMAIL-EXECUTE", "EXECUTE"))
		{
			frmDocSend obj = new frmDocSend(varDocList, null, pEventLoad: true);
			obj.ShowDialog(this);
			obj.Dispose();
			Close();
		}
	}

	private void btHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallChavesHelp();
	}

	private void frmDocSearch_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		clsHelpService.funcCallChavesHelp();
	}

	private void btMaximize_Click(object sender, EventArgs e)
	{
		if (base.WindowState.Equals(FormWindowState.Maximized))
		{
			base.WindowState = FormWindowState.Normal;
			btMaximize.Image = Resources.image_screen_maximize;
		}
		else if (base.WindowState.Equals(FormWindowState.Normal))
		{
			base.WindowState = FormWindowState.Maximized;
			btMaximize.Image = Resources.image_screen_restore;
		}
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
				btMaximize.Image = Resources.image_screen_maximize;
			}
			else if (base.MaximizeBox)
			{
				base.WindowState = FormWindowState.Maximized;
				btMaximize.Image = Resources.image_screen_restore;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmDocSearch));
		this.txDocKey = new System.Windows.Forms.TextBox();
		this.lbScreenTitle = new System.Windows.Forms.Label();
		this.lbDocKey = new System.Windows.Forms.Label();
		this.btEnter = new System.Windows.Forms.Button();
		this.lbSeparator02 = new System.Windows.Forms.Label();
		this.lbWarning = new System.Windows.Forms.Label();
		this.lkbLoadTxtFile = new System.Windows.Forms.LinkLabel();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.toolTipInfAdd = new System.Windows.Forms.ToolTip(this.components);
		this.lsvDataDoc = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvDcKey = new BrightIdeasSoftware.OLVColumn();
		this.olvDcType = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDoc = new System.Windows.Forms.ImageList(this.components);
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbFiltrar = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDownloadDFe = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbExport = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSendDoc = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbExportar = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.label4 = new System.Windows.Forms.Label();
		this.pnForm = new System.Windows.Forms.Panel();
		this.pnContent = new System.Windows.Forms.Panel();
		this.pnTitleBar = new System.Windows.Forms.Panel();
		this.picTitleBar = new System.Windows.Forms.PictureBox();
		this.btHelp = new System.Windows.Forms.Button();
		this.btClose = new System.Windows.Forms.Button();
		this.lbTitleBar = new System.Windows.Forms.Label();
		this.btMaximize = new System.Windows.Forms.Button();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lsvDataDoc).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.pnForm.SuspendLayout();
		this.pnContent.SuspendLayout();
		this.pnTitleBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).BeginInit();
		base.SuspendLayout();
		this.txDocKey.BackColor = System.Drawing.SystemColors.Info;
		this.txDocKey.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txDocKey.Location = new System.Drawing.Point(4, 48);
		this.txDocKey.MaxLength = 255;
		this.txDocKey.Name = "txDocKey";
		this.txDocKey.Size = new System.Drawing.Size(390, 22);
		this.txDocKey.TabIndex = 120;
		this.txDocKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txDocKey.KeyDown += new System.Windows.Forms.KeyEventHandler(txDocKey_KeyDown);
		this.lbScreenTitle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbScreenTitle.Location = new System.Drawing.Point(3, 4);
		this.lbScreenTitle.Name = "lbScreenTitle";
		this.lbScreenTitle.Size = new System.Drawing.Size(455, 14);
		this.lbScreenTitle.TabIndex = 119;
		this.lbScreenTitle.Text = "Empresa : ";
		this.lbDocKey.AutoSize = true;
		this.lbDocKey.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDocKey.Location = new System.Drawing.Point(3, 29);
		this.lbDocKey.Name = "lbDocKey";
		this.lbDocKey.Size = new System.Drawing.Size(241, 14);
		this.lbDocKey.TabIndex = 118;
		this.lbDocKey.Text = "Informe a chave e pressione [ENTER]";
		this.btEnter.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btEnter.Location = new System.Drawing.Point(398, 46);
		this.btEnter.Name = "btEnter";
		this.btEnter.Size = new System.Drawing.Size(60, 25);
		this.btEnter.TabIndex = 124;
		this.btEnter.Text = "&Enter";
		this.btEnter.UseVisualStyleBackColor = true;
		this.btEnter.Click += new System.EventHandler(btEnter_Click);
		this.lbSeparator02.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbSeparator02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator02.Location = new System.Drawing.Point(4, 417);
		this.lbSeparator02.Name = "lbSeparator02";
		this.lbSeparator02.Size = new System.Drawing.Size(992, 2);
		this.lbSeparator02.TabIndex = 227;
		this.lbWarning.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.lbWarning.Location = new System.Drawing.Point(593, 2);
		this.lbWarning.Name = "lbWarning";
		this.lbWarning.Size = new System.Drawing.Size(232, 39);
		this.lbWarning.TabIndex = 228;
		this.lbWarning.Text = "Use CTRL C e CTRL V para colar várias chaves no campo [Chave]";
		this.lbWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbLoadTxtFile.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.lkbLoadTxtFile.Location = new System.Drawing.Point(593, 55);
		this.lkbLoadTxtFile.Name = "lkbLoadTxtFile";
		this.lkbLoadTxtFile.Size = new System.Drawing.Size(232, 13);
		this.lkbLoadTxtFile.TabIndex = 229;
		this.lkbLoadTxtFile.TabStop = true;
		this.lkbLoadTxtFile.Text = "Importar lista de chaves (.txt)";
		this.lkbLoadTxtFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbLoadTxtFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbLoadTxtFile_LinkClicked);
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(586, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(2, 68);
		this.label1.TabIndex = 230;
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(830, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(2, 68);
		this.label2.TabIndex = 231;
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(593, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(232, 2);
		this.label3.TabIndex = 232;
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(4, 376);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(992, 38);
		this.plnMessage.TabIndex = 233;
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
		this.toolTipInfAdd.AutomaticDelay = 50000;
		this.toolTipInfAdd.IsBalloon = true;
		this.toolTipInfAdd.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTipInfAdd.ToolTipTitle = "Ajuda";
		this.lsvDataDoc.AllColumns.Add(this.olvSelect);
		this.lsvDataDoc.AllColumns.Add(this.olvDcKey);
		this.lsvDataDoc.AllColumns.Add(this.olvDcType);
		this.lsvDataDoc.AllColumns.Add(this.olvFilial);
		this.lsvDataDoc.AllColumns.Add(this.olvStatus);
		this.lsvDataDoc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvDataDoc.CellEditUseWholeCell = false;
		this.lsvDataDoc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5] { this.olvSelect, this.olvDcKey, this.olvDcType, this.olvFilial, this.olvStatus });
		this.lsvDataDoc.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvDataDoc.EmptyListMsg = "";
		this.lsvDataDoc.EmptyListMsgFont = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvDataDoc.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvDataDoc.FullRowSelect = true;
		this.lsvDataDoc.HideSelection = false;
		this.lsvDataDoc.Location = new System.Drawing.Point(4, 76);
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
		this.lsvDataDoc.Size = new System.Drawing.Size(992, 338);
		this.lsvDataDoc.SmallImageList = this.ImageListDoc;
		this.lsvDataDoc.SortGroupItemsByPrimaryColumn = false;
		this.lsvDataDoc.SpaceBetweenGroups = 5;
		this.lsvDataDoc.TabIndex = 234;
		this.lsvDataDoc.TintSortColumn = true;
		this.lsvDataDoc.UseCellFormatEvents = true;
		this.lsvDataDoc.UseCompatibleStateImageBehavior = false;
		this.lsvDataDoc.UseFilterIndicator = true;
		this.lsvDataDoc.UseFiltering = true;
		this.lsvDataDoc.UseHotControls = false;
		this.lsvDataDoc.UseHyperlinks = true;
		this.lsvDataDoc.UseOverlays = false;
		this.lsvDataDoc.View = System.Windows.Forms.View.Details;
		this.lsvDataDoc.VirtualMode = true;
		this.lsvDataDoc.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(lsvData_ColumnReordered);
		this.lsvDataDoc.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lsvData_ColumnWidthChanged);
		this.olvSelect.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelect.Groupable = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 30;
		this.olvDcKey.AspectName = "DocKey";
		this.olvDcKey.Text = "Chave";
		this.olvDcKey.Width = 320;
		this.olvDcType.AspectName = "DcType";
		this.olvDcType.Text = "Tipo";
		this.olvDcType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDcType.Width = 49;
		this.olvStatus.AspectName = "Status";
		this.olvStatus.Text = "Status e/ou Ação";
		this.olvStatus.Width = 450;
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.MaximumWidth = 100;
		this.olvFilial.MinimumWidth = 0;
		this.olvFilial.Text = "Filial";
		this.olvFilial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFilial.Width = 113;
		this.ImageListDoc.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDoc.ImageStream");
		this.ImageListDoc.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDoc.Images.SetKeyName(0, "image_schedule.jpg");
		this.ImageListDoc.Images.SetKeyName(1, "image_ok.jpg");
		this.ImageListDoc.Images.SetKeyName(2, "image_error.png");
		this.ImageListDoc.Images.SetKeyName(3, "image_xml_red.png");
		this.toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.toolStrip1.AutoSize = false;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(-1);
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripSeparator5, this.tsbFiltrar, this.toolStripSeparator3, this.tsbDownloadDFe, this.toolStripSeparator8, this.tsbExport, this.toolStripSeparator1, this.tsbSendDoc, this.toolStripSeparator4, this.tsbExportar,
			this.toolStripSeparator2
		});
		this.toolStrip1.Location = new System.Drawing.Point(4, 421);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(5);
		this.toolStrip1.Size = new System.Drawing.Size(992, 31);
		this.toolStrip1.Stretch = true;
		this.toolStrip1.TabIndex = 236;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator5.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 21);
		this.tsbFiltrar.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbFiltrar.Image = Monitor.Resources.image_filter;
		this.tsbFiltrar.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbFiltrar.Name = "tsbFiltrar";
		this.tsbFiltrar.Size = new System.Drawing.Size(139, 18);
		this.tsbFiltrar.Text = "Filtrar por Chaves";
		this.tsbFiltrar.Click += new System.EventHandler(tsbFiltrar_Click);
		this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 21);
		this.tsbDownloadDFe.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbDownloadDFe.Image = Monitor.Resources.image_download_cloud;
		this.tsbDownloadDFe.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDownloadDFe.Name = "tsbDownloadDFe";
		this.tsbDownloadDFe.Size = new System.Drawing.Size(139, 18);
		this.tsbDownloadDFe.Tag = "#NOT-TABPARTNER";
		this.tsbDownloadDFe.Text = "Download de XML";
		this.tsbDownloadDFe.Click += new System.EventHandler(tsbDownloadDFe_Click);
		this.toolStripSeparator8.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 21);
		this.tsbExport.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbExport.Image = Monitor.Resources.image_export;
		this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExport.Name = "tsbExport";
		this.tsbExport.Size = new System.Drawing.Size(115, 18);
		this.tsbExport.Tag = "";
		this.tsbExport.Text = "Exportar Docs";
		this.tsbExport.ToolTipText = "Exportar documentos";
		this.tsbExport.Click += new System.EventHandler(tsbExport_Click);
		this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 21);
		this.tsbSendDoc.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbSendDoc.Image = Monitor.Resources.image_email;
		this.tsbSendDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSendDoc.Name = "tsbSendDoc";
		this.tsbSendDoc.Size = new System.Drawing.Size(128, 18);
		this.tsbSendDoc.Tag = "";
		this.tsbSendDoc.Text = "Enviar por Email";
		this.tsbSendDoc.ToolTipText = "Enviar documentos por e-mail";
		this.tsbSendDoc.Click += new System.EventHandler(tsbSendDoc_Click);
		this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 21);
		this.tsbExportar.Image = (System.Drawing.Image)resources.GetObject("tsbExportar.Image");
		this.tsbExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExportar.Name = "tsbExportar";
		this.tsbExportar.Size = new System.Drawing.Size(23, 18);
		this.tsbExportar.ToolTipText = "Exportar lista de documentos para o Excel";
		this.tsbExportar.Click += new System.EventHandler(tsbExportar_Click);
		this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 21);
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label4.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label4.Location = new System.Drawing.Point(9, 498);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(1134, 2);
		this.label4.TabIndex = 237;
		this.pnForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnForm.Controls.Add(this.pnContent);
		this.pnForm.Controls.Add(this.pnTitleBar);
		this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnForm.Location = new System.Drawing.Point(0, 0);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(1016, 506);
		this.pnForm.TabIndex = 238;
		this.pnContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnContent.BackColor = System.Drawing.Color.White;
		this.pnContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnContent.Controls.Add(this.plnMessage);
		this.pnContent.Controls.Add(this.lbScreenTitle);
		this.pnContent.Controls.Add(this.lsvDataDoc);
		this.pnContent.Controls.Add(this.toolStrip1);
		this.pnContent.Controls.Add(this.btEnter);
		this.pnContent.Controls.Add(this.lbDocKey);
		this.pnContent.Controls.Add(this.label3);
		this.pnContent.Controls.Add(this.txDocKey);
		this.pnContent.Controls.Add(this.label2);
		this.pnContent.Controls.Add(this.lbSeparator02);
		this.pnContent.Controls.Add(this.label1);
		this.pnContent.Controls.Add(this.lbWarning);
		this.pnContent.Controls.Add(this.lkbLoadTxtFile);
		this.pnContent.Location = new System.Drawing.Point(6, 37);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(1001, 458);
		this.pnContent.TabIndex = 8;
		this.pnTitleBar.BackColor = System.Drawing.Color.White;
		this.pnTitleBar.Controls.Add(this.picTitleBar);
		this.pnTitleBar.Controls.Add(this.btHelp);
		this.pnTitleBar.Controls.Add(this.btClose);
		this.pnTitleBar.Controls.Add(this.lbTitleBar);
		this.pnTitleBar.Controls.Add(this.btMaximize);
		this.pnTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnTitleBar.Location = new System.Drawing.Point(0, 0);
		this.pnTitleBar.Name = "pnTitleBar";
		this.pnTitleBar.Size = new System.Drawing.Size(1014, 31);
		this.pnTitleBar.TabIndex = 2;
		this.picTitleBar.Image = Monitor.Resources.image_favicon;
		this.picTitleBar.Location = new System.Drawing.Point(7, 6);
		this.picTitleBar.Name = "picTitleBar";
		this.picTitleBar.Size = new System.Drawing.Size(18, 18);
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
		this.btHelp.Location = new System.Drawing.Point(877, 3);
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
		this.btClose.Location = new System.Drawing.Point(984, 2);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 1;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lbTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTitleBar.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitleBar.Location = new System.Drawing.Point(32, 5);
		this.lbTitleBar.Name = "lbTitleBar";
		this.lbTitleBar.Size = new System.Drawing.Size(839, 20);
		this.lbTitleBar.TabIndex = 0;
		this.lbTitleBar.Text = "Fiscal.io - Informe uma ou mais chaves de acesso dos documentos fiscais";
		this.lbTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(lbTitleBar_MouseDown);
		this.btMaximize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btMaximize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btMaximize.FlatAppearance.BorderSize = 0;
		this.btMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btMaximize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btMaximize.Image = Monitor.Resources.image_screen_maximize;
		this.btMaximize.Location = new System.Drawing.Point(955, 2);
		this.btMaximize.Name = "btMaximize";
		this.btMaximize.Size = new System.Drawing.Size(26, 24);
		this.btMaximize.TabIndex = 4;
		this.btMaximize.UseVisualStyleBackColor = true;
		this.btMaximize.Click += new System.EventHandler(btMaximize_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1016, 506);
		base.Controls.Add(this.pnForm);
		base.Controls.Add(this.label4);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MinimizeBox = false;
		base.Name = "frmDocSearch";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Informe uma ou mais chaves de acesso dos documentos fiscais";
		base.Load += new System.EventHandler(frmDocSearch_Load);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(frmDocSearch_HelpRequested);
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lsvDataDoc).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.pnForm.ResumeLayout(false);
		this.pnContent.ResumeLayout(false);
		this.pnContent.PerformLayout();
		this.pnTitleBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).EndInit();
		base.ResumeLayout(false);
	}
}
