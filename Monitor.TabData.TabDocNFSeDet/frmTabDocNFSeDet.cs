using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor.TabData.TabDocNFSeDet;

public class frmTabDocNFSeDet : Form
{
	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocNFSeDet _SqlTabData = new clsSrvTabDocNFSeDet();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private Color _ColumnColor;

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private string _InfoAddXml = string.Empty;

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private decimal _TotalValue;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private bool _HasFiscalioConnect;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private IContainer components;

	private Panel pnContent;

	private FastObjectListView lsvData;

	private ContextMenuStrip contextMenuDocs;

	private Panel pnMarket;

	private LinkLabel lknClose;

	private Panel pnMarketContent;

	private Button btSalesAction;

	private Label label3;

	private Label lbTitle03;

	private PictureBox picWarning;

	private LinkLabel lknAction;

	private Label lbText02;

	private Label lbText01;

	private Label lbTitle01;

	private Label lbTitle02;

	private Button btClose;

	private OLVColumn olvSelect;

	private OLVColumn olvHasXml;

	private OLVColumn olvTag;

	private OLVColumn olvTagDtHr;

	private OLVColumn olvTagUser;

	private OLVColumn olvDocNote;

	private OLVColumn olvDocNoteDtHr;

	private OLVColumn olvDocNoteUser;

	private OLVColumn olvTpAmb;

	private OLVColumn olvnDpsServ;

	private OLVColumn olvDtCompet;

	private OLVColumn olvTpEmit;

	private OLVColumn olvPrestIM;

	private OLVColumn olvRegApTribSN;

	private OLVColumn olvRegEspTrib;

	private OLVColumn olvTomaIM;

	private OLVColumn olvInterIM;

	private OLVColumn olvcLocEmi;

	private OLVColumn olvxLocEmi;

	private OLVColumn olvcLocPrest;

	private OLVColumn olvxLocPrest;

	private OLVColumn olvcMunNFSeMun;

	private OLVColumn olvnNFSeMun;

	private OLVColumn olvcVerifNFSeMun;

	private OLVColumn olvTpSusp;

	private OLVColumn olvnProcesso;

	private OLVColumn olvTpImunidade;

	private OLVColumn olvTotalRet;

	private OLVColumn olvEmitIM;

	private OLVColumn olvxTribMun;

	private OLVColumn olvcTribMun;

	private OLVColumn olvVersion;

	private OLVColumn olvChave;

	private OLVColumn olvNum;

	private OLVColumn olvNatOper;

	private OLVColumn olvProcEmi;

	private OLVColumn olvcStat;

	private OLVColumn olvDtAut;

	private OLVColumn olvAnoMes;

	private OLVColumn olvEstado;

	private OLVColumn olvTipo;

	private OLVColumn olvTpDoc;

	private OLVColumn olvTomaIE;

	private OLVColumn olvFilial;

	private OLVColumn olvEmitID;

	private OLVColumn olvEmitNome;

	private OLVColumn olvEmitcMun;

	private OLVColumn olvEmitUf;

	private OLVColumn olvValor;

	private OLVColumn olvDtEmi;

	private OLVColumn olvHrEmi;

	private OLVColumn olvSerie;

	private OLVColumn olvPrestID;

	private OLVColumn olvPrestNome;

	private OLVColumn olvIndSimplesNac;

	private OLVColumn olvTomaID;

	private OLVColumn olvTomaNome;

	private OLVColumn olvInterID;

	private OLVColumn olvInterNome;

	private OLVColumn olvNCM;

	private OLVColumn olvISSQNCdSrv;

	private OLVColumn olvISSQNTribut;

	private OLVColumn olvISSQNTPret;

	private OLVColumn olvISSQNTaxa;

	private OLVColumn olvISSQNValor;

	private OLVColumn olvPISBase;

	private OLVColumn olvPISTaxa;

	private OLVColumn olvPISValor;

	private OLVColumn olvPISCdCst;

	private OLVColumn olvCOFINSBase;

	private OLVColumn olvCOFINSTaxa;

	private OLVColumn olvCOFINSValor;

	private OLVColumn olvCOFINSCdCst;

	private OLVColumn olvPISCoftPret;

	private OLVColumn olvPISCofvRetCP;

	private OLVColumn olvPISCofvRetIRRF;

	private OLVColumn olvPISCofvRetCSLL;

	private OLVColumn olvStatus;

	private OLVColumn olvxProd;

	private ImageList ImageListDocs;

	private Button btSalesContact;

	private OLVColumn olvEmitida;

	private OLVColumn olvDtEntFisc;

	private OLVColumn olvDtRegFisc;

	private OLVColumn olvUsRegFisc;

	private OLVColumn olvDcNumFisc;

	private OLVColumn olvCFOP;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabDocNFSeDet(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		olvAnoMes.Name = "AnoMes";
		lsvData.HandleDestroyed += LsvData_HandleDestroyed;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private void LsvData_HandleDestroyed(object sender, EventArgs e)
	{
		funcSaveColumns();
	}

	public void funcSaveColumns()
	{
		if (lsvData != null && _ColumnsLoaded)
		{
			clsListViewUtils.funcSaveColumnsOrderAsDisplayed(lsvData, base.Name);
		}
	}

	private async void frmTabDocNFSeDet_Load(object sender, EventArgs e)
	{
		_InfoAddXml = (await new clsDataConfig().funcGetItemByKeyAsync()).InfoAddXmlNFe;
		_StateList = await new clsDataEstado().funcGetListAsync(pWthAN: false);
		_HasFiscalioConnect = await clsScreenGeral.funcMustShowRegFieldsAsync();
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		await funcLoadTagsAsync();
		_ColumnsLoaded = false;
		clsScreenGeral.funcSetColumnsObjectListView(this, lsvData);
		_ColumnsLoaded = true;
		foreach (OLVColumn allColumn in lsvData.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
		if (clsFunction.IsAdmin)
		{
			clsScreenGeral.funcCheckListViewDdic(typeof(DocDocItem), lsvData);
		}
		if (string.IsNullOrEmpty(_InfoAddXml))
		{
			List<string> varColList = new List<string>
			{
				clsFunction.funcClearSpecialCaracter("Informações complementares"),
				clsFunction.funcClearSpecialCaracter("Informações adicionais fisco")
			};
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList);
		}
		if (!_HasFiscalioConnect)
		{
			List<string> varColList2 = new List<string> { "DtEntFisc", "DtRegFisc", "UsRegFisc", "DcNumFisc" };
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList2);
		}
		lsvData.CellToolTipShowing += olv_CellToolTipShowing;
		funcDefineListViewFeatures();
		bool varMustShow = true;
		if (!clsFunction.IsEmpty(await _clsDataParam.funcGetAsync(_consMarketScreenUser)))
		{
			varMustShow = false;
		}
		clsFeatureService.strSalesData varclsSalesData = await _clsFeatService.funcGetSalesDataAsync(_FeatExtId);
		if (clsFunction.Contains(varclsSalesData.FeatType, "LOCK"))
		{
			_IsLocked = true;
		}
		lbTitle01.Text = "Relatório " + Text;
		Button button = btSalesContact;
		bool visible = (btSalesAction.Visible = false);
		button.Visible = visible;
		if (_IsLocked)
		{
			pnMarket.Visible = true;
			lsvData.Dock = DockStyle.None;
			Button button2 = btClose;
			visible = (lknClose.Visible = false);
			button2.Visible = visible;
			Label label = lbTitle01;
			label.Text = label.Text + Environment.NewLine + varclsSalesData.PlanDirection;
			picWarning.Image = Resources.image_locker;
			if (varclsSalesData.ShowPlanAction)
			{
				btSalesContact.Visible = true;
				btSalesContact.Left = 90;
				btSalesAction.Visible = true;
			}
			else
			{
				btSalesContact.Visible = true;
				btSalesContact.Left = 188;
				btSalesAction.Visible = false;
			}
		}
		else if (!varMustShow)
		{
			lsvData.Dock = DockStyle.Fill;
			pnMarket.Visible = false;
			base.Controls.Remove(pnMarket);
		}
		else
		{
			pnMarket.Visible = true;
		}
	}

	private void funcDefineListViewFeatures()
	{
		olvHasXml.ImageGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(docDocItem, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
		};
		olvHasXml.AspectGetter = (object x) => string.Empty;
		olvEmitida.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docDocItem.Emitida) ? "Terceiros" : "Empresa";
		};
		olvTag.ImageGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : ((object)clsScreenGeral.funcGetDocNotesIcon(docDocItem));
		};
		olvTag.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docDocItem.Tag) ? null : _TagHsList[docDocItem.Tag];
		};
		olvNum.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcGetValue(docDocItem.Num).PadLeft(9, '0');
		};
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsScreenGeral.funcGetDocStatIcon(docDocItem);
		};
		olvAnoMes.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = docDocItem.DtAut.Substring(0, 7);
			}
			catch
			{
			}
			return result;
		};
		olvEstado.AspectGetter = delegate(object x)
		{
			DocDocItem varDocument = (DocDocItem)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
		};
		olvTipo.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcGetDocType(docDocItem.Model);
		};
		olvTpDoc.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetTipoDoc(docDocItem.Model, docDocItem.TpDoc);
		};
		olvFilial.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcFormatDoc(docDocItem.Filial);
		};
		olvIndSimplesNac.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetIndSimples(docDocItem.Model, docDocItem.IndSimplesNac);
		};
		olvcStat.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetCStat(docDocItem.cStat);
		};
		olvProcEmi.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetProcEmi(docDocItem.ProcEmi);
		};
		olvTpAmb.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.GetTpAmbDesc(docDocItem.TpAmb);
		};
		olvRegApTribSN.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetRegApTribSN(docDocItem.RegApTribSN);
		};
		olvRegEspTrib.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetRegEspTrib(docDocItem.RegEspTrib);
		};
		olvTpSusp.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetTpSusp(docDocItem.tpSusp);
		};
		olvTpImunidade.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetTpUminidade(docDocItem.tpImunidade);
		};
		olvISSQNTribut.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetISSQNTribut(docDocItem.ISSQNTribut);
		};
		olvISSQNTPret.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetISSQNTPret(docDocItem.ISSQNTPret);
		};
		olvCFOP.AspectGetter = (object x) => ((DocDocItem)x)?.CFOP;
	}

	private void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		DocDocItem varDocument = (DocDocItem)e.Model;
		if (varDocument != null)
		{
			_ = string.Empty;
			if (e.ColumnIndex.Equals(olvHasXml.Index))
			{
				e.Text = varclsValidator.funcGetDesc(varDocument);
			}
			else if (e.ColumnIndex.Equals(olvStatus.Index))
			{
				e.Text = clsScreenGeral.funcGetDocStatDesc(varDocument);
			}
			else if (e.ColumnIndex.Equals(olvTag.Index))
			{
				e.Text = varDocument.DocNote;
			}
		}
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturn = new clsReturn();
		string varSqlQuery = await _SqlTabData.funcGetSqlStrSelectAsync(pclsDataFilter);
		if (string.IsNullOrEmpty(varSqlQuery))
		{
			return varclsReturn;
		}
		lsvData.BeginUpdate();
		lsvData.SuspendLayout();
		long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
		if (_IsLocked)
		{
			varPageSize = 4L;
		}
		List<DocDocItem> varclsDocList = await new clsDataDocDocItem().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		olvSelect.HeaderCheckState = CheckState.Unchecked;
		lsvData.SetObjects(varclsDocList);
		_GroupColapsed = false;
		lsvData.ResumeLayout();
		lsvData.EndUpdate();
		lsvData = await funcListGroupSortAsync(lsvData, pclsDataFilter);
		return varclsReturn;
	}

	private async Task<FastObjectListView> funcListGroupSortAsync(FastObjectListView plsvData, clsDataFilter pclsDataFilter)
	{
		OLVColumn varOlvColum;
		SortOrder varSortOrder;
		if (pclsDataFilter.GroupByDisable || _IsLocked)
		{
			varOlvColum = null;
			varSortOrder = SortOrder.None;
		}
		else if (pclsDataFilter.GroupByPartner)
		{
			varOlvColum = olvEmitNome;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByDate)
		{
			varOlvColum = olvDtAut;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByYearMonth)
		{
			varOlvColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByState)
		{
			varOlvColum = olvEstado;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByModel)
		{
			varOlvColum = olvTipo;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByTpDoc)
		{
			varOlvColum = olvTpDoc;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByNatOper)
		{
			varOlvColum = olvNatOper;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByFilial)
		{
			varOlvColum = olvFilial;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByTomaIE)
		{
			varOlvColum = olvTomaIE;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByCFOP)
		{
			varOlvColum = olvCFOP;
			varSortOrder = SortOrder.Ascending;
		}
		else
		{
			varOlvColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		plsvData = clsScreenGeral.funcSetLsvSortGroup(this, plsvData, varOlvColum, varSortOrder, olvDtAut, SortOrder.Descending);
		return plsvData;
	}

	public async Task<bool> funcLoadTagsAsync()
	{
		EventHandler varclsActFiscReset = null;
		if (_HasFiscalioConnect)
		{
			varclsActFiscReset = tsbDocReset_Click;
		}
		return await clsMonGeral.funcLoadTagsAsync(_HasColors, _TagHsList, contextMenuDocs, tsmCopyDocKey_Click, tsmTransferFilial_Click, tsbTagCode_Click, tsbDocNote_Click, varclsActFiscReset, tsbRemoveDocNote_Click);
	}

	private async void tsmTransferFilial_Click(object sender, EventArgs e)
	{
		await funcTransferFilialAsync();
	}

	private async Task<bool> funcTransferFilialAsync()
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		varArguments.TaskProgress = "Transferindo documento(s) para o destino...";
		OnEventTabManager(varArguments);
		foreach (clsValue varclsValue in (await new clsManGeral().funcTransferFilialAsync(varDocList)).Values)
		{
			if (clsFunction.IsEqual(varclsValue.Name, "DocDocItem", pIgnoreCase: true))
			{
				lsvData.RemoveObject(varclsValue.ObjDoc);
			}
		}
		varArguments.TaskProgress = string.Empty;
		OnEventTabManager(varArguments);
		return true;
	}

	private async void lsvData_MouseDown(object sender, MouseEventArgs e)
	{
		if (!e.Button.Equals(MouseButtons.Right))
		{
			return;
		}
		string varFilialDummnyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
		bool varHasDummy = (await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true)).Any((Document r) => r.Filial.Equals(varFilialDummnyIdnt));
		foreach (ToolStripItem varclsItem in contextMenuDocs.Items)
		{
			if (clsFunction.IsEqual(clsFunction.funcGetValue(varclsItem.Tag), "TRANSFER", pIgnoreCase: true))
			{
				varclsItem.Visible = varHasDummy;
			}
		}
	}

	private async void tsbDocReset_Click(object sender, EventArgs e)
	{
		List<DocDocItem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
		List<Document> varDocList = await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true);
		if (varDocList == null || varDocList.Count <= 0)
		{
			return;
		}
		List<string> varFilialList = clsDataDoc.funcGetFilialList(varDocList);
		foreach (string varclsItem in varFilialList)
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("STATFISC-RESET", "RESET", varclsItem)))
			{
				return;
			}
		}
		string varUserMessage = "Tem certeza que deseja reiniciar o status de lançamento fiscal dos documentos selecionados?";
		if (!MessageBox.Show(this, varUserMessage, "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
		{
			await _clsDataDoc.funcResetFiscalAsync(varDocList, pSyncFromDbaFirst: false);
			funcRefreshListAsync(varDocList, varObjList);
		}
	}

	private async void tsbTagCode_Click(object sender, EventArgs e)
	{
		ToolStripDropDownItem varTagMenu = (ToolStripDropDownItem)sender;
		if (varTagMenu != null)
		{
			string varTagCode = (string)varTagMenu.Tag;
			if (varTagCode != null)
			{
				await funcSetTagAsync(varTagCode, pFocused: true, pChecked: true);
			}
		}
	}

	public async Task<bool> funcSetTagAsync(string pTagCode, bool pFocused, bool pChecked)
	{
		clsWorkFlowService varclsService = new clsWorkFlowService();
		List<DocDocItem> varObjList = funcGetObjList(pFocused, pChecked);
		List<Document> varDocList = await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true);
		if (varDocList == null)
		{
			return false;
		}
		if (varDocList.Count <= 0)
		{
			return false;
		}
		List<string> varFilialList = clsDataDoc.funcGetFilialList(varDocList);
		if (!(await clsScreenGeral.funcHasAccessAsync("TAG-ASSIGN", "ASSIGN", varFilialList)))
		{
			return false;
		}
		List<string> varTagList = clsDataDoc.funcGetTagList(varDocList);
		if (clsFunction.IsEmpty(pTagCode) && !(await clsScreenGeral.funcHasAccessAsync("TAG-SET-ITEM", "ASSIGN", varTagList)))
		{
			return false;
		}
		await varclsService.funcSetTagAsync(varDocList, pTagCode, pSyncFromDbaFirst: false);
		funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	private async void tsbRemoveDocNote_Click(object sender, EventArgs e)
	{
		funcRemoveDocNoteAsync(pFocused: true, pChecked: true);
	}

	public async Task<bool> funcRemoveDocNoteAsync(bool pFocused, bool pChecked)
	{
		List<DocDocItem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
		List<Document> varDocList = await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true);
		if (varDocList == null)
		{
			return false;
		}
		if (varDocList.Count <= 0)
		{
			return false;
		}
		List<string> varFilialList = clsDataDoc.funcGetFilialList(varDocList);
		foreach (string varclsItem in varFilialList)
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("DOCNOTE-ASSIGN", "ASSIGN", varclsItem)))
			{
				return false;
			}
		}
		string varUserMessage = "Tem certeza que deseja remover os comentários dos documentos selecionados?";
		if (MessageBox.Show(this, varUserMessage, "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
		{
			return false;
		}
		await _clsDataDoc.funcRemoveDocNoteAsync(varDocList, pSyncFromDbaFirst: false);
		funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	private async void tsbDocNote_Click(object sender, EventArgs e)
	{
		await funcSetDocNoteAsync(pFocused: true, pChecked: true);
	}

	public async Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked)
	{
		List<DocDocItem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
		List<Document> varDocList = await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true);
		if (varDocList == null)
		{
			return false;
		}
		if (varDocList.Count <= 0)
		{
			return false;
		}
		List<string> varFilialList = clsDataDoc.funcGetFilialList(varDocList);
		foreach (string varclsItem in varFilialList)
		{
			if (!(await clsScreenGeral.funcHasAccessAsync("DOCNOTE-ASSIGN", "ASSIGN", varclsItem)))
			{
				return false;
			}
		}
		frmDocNote frmDocNote = new frmDocNote(ref varDocList);
		frmDocNote.ShowDialog(this);
		frmDocNote.Dispose();
		funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	public void funcSearchText(string pSearchTerm, bool pSearchInteligent)
	{
		if (clsFunction.IsEmpty(pSearchTerm))
		{
			lsvData.AdditionalFilter = null;
		}
		else
		{
			if (pSearchInteligent)
			{
				foreach (OLVColumn column in lsvData.Columns)
				{
					if (column.Text == "Num")
					{
						column.Searchable = true;
					}
					else
					{
						column.Searchable = false;
					}
				}
			}
			else
			{
				foreach (OLVColumn column2 in lsvData.Columns)
				{
					column2.Searchable = true;
				}
			}
			TextMatchFilter varTextMatchFilter = TextMatchFilter.Contains(lsvData, pSearchTerm);
			if (lsvData.DefaultRenderer == null)
			{
				lsvData.DefaultRenderer = new HighlightTextRenderer(varTextMatchFilter);
			}
			lsvData.AdditionalFilter = varTextMatchFilter;
		}
		if (lsvData.ShowGroups)
		{
			lsvData.BuildGroups();
		}
	}

	private async void tsmCopyDocKey_Click(object sender, EventArgs e)
	{
		await funcCopyDocKeyAsync();
	}

	private async Task<bool> funcCopyDocKeyAsync()
	{
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: false);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		return await clsScreenGeral.funcCopyDocKeyAsync(varDocList);
	}

	private async void lsvData_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode.Equals(Keys.C))
		{
			await funcCopyDocKeyAsync();
		}
	}

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportDocToExcelAsync<DocDocItem>(lsvData, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<DocDocItem> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<DocDocItem> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (DocDocItem varObject in pObjList)
		{
			Document varclsDoc = new Document
			{
				Filial = varObject.Filial,
				Chave = varObject.Chave
			};
			varDocList.Add(varclsDoc);
		}
		if (pSyncFromDbaFirst)
		{
			varDocList = await _clsDataDoc.funcGetSyncListByListAsync(varDocList);
		}
		return varDocList;
	}

	private List<DocDocItem> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<DocDocItem> varDocList = new List<DocDocItem>();
		DocDocItem varFocused = new DocDocItem();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (DocDocItem)lsvData.GetItem(lsvData.FocusedItem.Index).RowObject;
				varDocList.Add(varFocused);
			}
			catch
			{
				varFocused = new DocDocItem();
			}
		}
		if (pChecked)
		{
			foreach (DocDocItem varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varDocList.Add(varObject);
				}
			}
			if (varDocList.Count > 0)
			{
				varDocList = lsvData.Objects.Cast<DocDocItem>().Intersect(varDocList).ToList();
			}
		}
		if (!pFocused && !pChecked)
		{
			varDocList = lsvData.FilteredObjects.Cast<DocDocItem>().ToList();
			if (varDocList.Count <= 0)
			{
				varDocList = lsvData.Objects.Cast<DocDocItem>().ToList();
			}
		}
		return varDocList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<DocDocItem> varObjList = funcGetObjList(pFocused, pChecked);
		funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
		return true;
	}

	private async void funcRefreshListAsync(List<Document> pDocList, List<DocDocItem> pObjList)
	{
		ConcurrentBag<ObjFieldBuffer> varDocFieldList = clsObjectBuffer.funcGet<Document>().Fields;
		ConcurrentBag<ObjFieldBuffer> varObjFieldList = clsObjectBuffer.funcGet<DocDocItem>().Fields;
		await Task.WhenAll(((IEnumerable<DocDocItem>)pObjList).Select((Func<DocDocItem, Task>)async delegate(DocDocItem varclsObjItem)
		{
			Document varclsDocItem = pDocList.FirstOrDefault((Document r) => clsFunction.IsEqual(r.Filial, varclsObjItem.Filial) && clsFunction.IsEqual(r.Chave, varclsObjItem.Chave));
			if (varclsDocItem != null)
			{
				await Task.WhenAll(varDocFieldList.Select((ObjFieldBuffer varclsField) => Task.Run(delegate
				{
					PropertyInfo varDocProperty = varclsField.Property;
					ObjFieldBuffer objFieldBuffer = varObjFieldList.FirstOrDefault((ObjFieldBuffer r) => clsFunction.IsEqual(r.Property.Name, varDocProperty.Name, pIgnoreCase: true));
					if (objFieldBuffer != null)
					{
						object value = varDocProperty.GetValue(varclsDocItem);
						objFieldBuffer.Property.SetValue(varclsObjItem, value);
					}
				})));
			}
		}));
		lsvData.RefreshSelectedObjects();
	}

	public int funcGetTotalDocs()
	{
		return lsvData.Items.Count;
	}

	public decimal funcGetTotalValue()
	{
		return _TotalValue;
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
			OLVColumn varColumnObj = lsvData.AllColumns[e.ColumnIndex];
			string varColumName = clsScreenGeral.funcGetColumnName(varColumnObj);
			if (!string.IsNullOrEmpty(varColumName))
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
		if (!string.IsNullOrEmpty(varColumName))
		{
			string varObjectKey = base.Name + "-" + lsvData.Name + "-" + varColumName + "-Hidden";
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

	private void lsvData_HeaderCheckBoxChanging(object sender, HeaderCheckBoxChangingEventArgs e)
	{
		_IsOnHeaderCheckStatus = true;
		if (e.NewCheckState == CheckState.Checked)
		{
			lsvData.CheckAll();
		}
		else
		{
			lsvData.UncheckAll();
		}
		_IsOnHeaderCheckStatus = false;
		funcSetTabDataTotalValues();
	}

	private void lsvData_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (!e.Item.Checked)
		{
			olvSelect.HeaderCheckState = CheckState.Unchecked;
		}
		funcSetTabDataTotalValues();
	}

	private async void lsvData_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false);
	}

	private async void funcShowDocViewerAsync(bool pShowPDF, bool pShowXML, Document pDocument = null)
	{
		List<Document> varDocList = new List<Document>();
		if (pDocument != null)
		{
			varDocList.Add(pDocument);
			varDocList = await _clsDataDoc.funcGetSyncListByListAsync(varDocList);
		}
		else
		{
			varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		}
		await clsMonGeral.funcDocViewerAsync(this, varDocList, pShowPDF, pShowXML);
	}

	private void lsvData_FormatCell(object sender, FormatCellEventArgs e)
	{
		if (e.ColumnIndex == olvTag.Index)
		{
			DocDocItem varDocument = (DocDocItem)e.Model;
			if (varDocument != null && !clsFunction.IsEmpty(varDocument.Tag))
			{
				string varColorName = (string)_HasColors[varDocument.Tag];
				e.SubItem.BackColor = ColorTranslator.FromHtml(varColorName);
			}
		}
	}

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		DocDocItem varDocument = (DocDocItem)e.Model;
		if (varDocument != null)
		{
			if (!clsFunction.IsEmpty(varDocument.Canceled))
			{
				e.Item.BackColor = (e.Item.Checked ? Color.PeachPuff : Color.PapayaWhip);
			}
			else if (!clsFunction.IsEmpty(varDocument.HasCancelEvent))
			{
				e.Item.BackColor = (e.Item.Checked ? Color.PeachPuff : Color.PapayaWhip);
			}
			else
			{
				e.Item.BackColor = (e.Item.Checked ? Color.LightBlue : Color.White);
			}
		}
	}

	private async void lsvData_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
	{
		clsDataDoc varDataHandler = new clsDataDoc();
		if (e.ColumnIndex != olvNum.Index)
		{
			return;
		}
		DocDocItem varDocDocitem = (DocDocItem)e.Model;
		if (varDocDocitem != null)
		{
			Document varDatabase = await varDataHandler.funcGetItemByKeyAsync(varDocDocitem.Filial, varDocDocitem.Chave);
			if (varDatabase != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varDatabase);
			}
		}
	}

	public void funcColapseExpand()
	{
		if (lsvData.OLVGroups == null)
		{
			return;
		}
		if (_GroupColapsed)
		{
			_GroupColapsed = false;
		}
		else
		{
			_GroupColapsed = true;
		}
		foreach (OLVGroup oLVGroup in lsvData.OLVGroups)
		{
			oLVGroup.Collapsed = _GroupColapsed;
		}
	}

	private void lsvData_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		funcSetTabDataTotalValues();
	}

	private void funcSetTabDataTotalValues()
	{
		if (!_IsOnHeaderCheckStatus)
		{
			IEnumerable varEnumList = null;
			FastObjectListView fastObjectListView = lsvData;
			varEnumList = ((fastObjectListView == null || !(fastObjectListView.CheckedObjects?.Count > 0)) ? lsvData.FilteredObjects : lsvData.CheckedObjectsEnumerable);
			List<DocDocItem> varObjList = varEnumList.Cast<DocDocItem>().ToList();
			if (varObjList == null)
			{
				varObjList = new List<DocDocItem>();
			}
			_TotalValue = varObjList.Sum((DocDocItem r) => clsFunction.funcConvStrToDec(r.Valor));
			EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
			varArguments.TotalValue = _TotalValue;
			varArguments.TotalQuant = varObjList.Count;
			OnEventTabManager(varArguments);
		}
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		lsvData.Dock = DockStyle.Fill;
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
	}

	private async void lknClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		lsvData.Dock = DockStyle.Fill;
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
		await _clsDataParam.funcSetAsync(_consMarketScreenUser, "X");
	}

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipReportNFSeAnalyticalAsync();
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.TabData.TabDocNFSeDet.frmTabDocNFSeDet));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.pnContent = new System.Windows.Forms.Panel();
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitida = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitID = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitNome = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvHrEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEntFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDtRegFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvUsRegFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDcNumFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvxProd = new BrightIdeasSoftware.OLVColumn();
		this.olvNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvIndSimplesNac = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvValor = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNCdSrv = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNTribut = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNTPret = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNValor = new BrightIdeasSoftware.OLVColumn();
		this.olvTpImunidade = new BrightIdeasSoftware.OLVColumn();
		this.olvTotalRet = new BrightIdeasSoftware.OLVColumn();
		this.olvPISBase = new BrightIdeasSoftware.OLVColumn();
		this.olvPISTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvPISValor = new BrightIdeasSoftware.OLVColumn();
		this.olvPISCdCst = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSBase = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSValor = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSCdCst = new BrightIdeasSoftware.OLVColumn();
		this.olvPISCoftPret = new BrightIdeasSoftware.OLVColumn();
		this.olvPISCofvRetCP = new BrightIdeasSoftware.OLVColumn();
		this.olvPISCofvRetIRRF = new BrightIdeasSoftware.OLVColumn();
		this.olvPISCofvRetCSLL = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitUf = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitcMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitIM = new BrightIdeasSoftware.OLVColumn();
		this.olvcMunNFSeMun = new BrightIdeasSoftware.OLVColumn();
		this.olvcLocEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvxLocEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaID = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaNome = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaIM = new BrightIdeasSoftware.OLVColumn();
		this.olvPrestID = new BrightIdeasSoftware.OLVColumn();
		this.olvPrestNome = new BrightIdeasSoftware.OLVColumn();
		this.olvPrestIM = new BrightIdeasSoftware.OLVColumn();
		this.olvcLocPrest = new BrightIdeasSoftware.OLVColumn();
		this.olvxLocPrest = new BrightIdeasSoftware.OLVColumn();
		this.olvInterID = new BrightIdeasSoftware.OLVColumn();
		this.olvInterNome = new BrightIdeasSoftware.OLVColumn();
		this.olvInterIM = new BrightIdeasSoftware.OLVColumn();
		this.olvEstado = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvcStat = new BrightIdeasSoftware.OLVColumn();
		this.olvnDpsServ = new BrightIdeasSoftware.OLVColumn();
		this.olvTpEmit = new BrightIdeasSoftware.OLVColumn();
		this.olvProcEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvDtCompet = new BrightIdeasSoftware.OLVColumn();
		this.olvcTribMun = new BrightIdeasSoftware.OLVColumn();
		this.olvxTribMun = new BrightIdeasSoftware.OLVColumn();
		this.olvnProcesso = new BrightIdeasSoftware.OLVColumn();
		this.olvnNFSeMun = new BrightIdeasSoftware.OLVColumn();
		this.olvcVerifNFSeMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRegEspTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvRegApTribSN = new BrightIdeasSoftware.OLVColumn();
		this.olvTpSusp = new BrightIdeasSoftware.OLVColumn();
		this.olvTpAmb = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvTagUser = new BrightIdeasSoftware.OLVColumn();
		this.olvTagDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteUser = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvVersion = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.olvTomaIE = new BrightIdeasSoftware.OLVColumn();
		this.pnMarket = new System.Windows.Forms.Panel();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.pnMarketContent = new System.Windows.Forms.Panel();
		this.btSalesContact = new System.Windows.Forms.Button();
		this.btSalesAction = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.picWarning = new System.Windows.Forms.PictureBox();
		this.lknAction = new System.Windows.Forms.LinkLabel();
		this.lbText02 = new System.Windows.Forms.Label();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.pnContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.pnContent.Controls.Add(this.lsvData);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(784, 445);
		this.pnContent.TabIndex = 10;
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvHasXml);
		this.lsvData.AllColumns.Add(this.olvEmitida);
		this.lsvData.AllColumns.Add(this.olvEmitID);
		this.lsvData.AllColumns.Add(this.olvEmitNome);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvTpDoc);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvDtAut);
		this.lsvData.AllColumns.Add(this.olvDtEmi);
		this.lsvData.AllColumns.Add(this.olvHrEmi);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvDtEntFisc);
		this.lsvData.AllColumns.Add(this.olvDtRegFisc);
		this.lsvData.AllColumns.Add(this.olvUsRegFisc);
		this.lsvData.AllColumns.Add(this.olvDcNumFisc);
		this.lsvData.AllColumns.Add(this.olvxProd);
		this.lsvData.AllColumns.Add(this.olvNCM);
		this.lsvData.AllColumns.Add(this.olvIndSimplesNac);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvValor);
		this.lsvData.AllColumns.Add(this.olvISSQNCdSrv);
		this.lsvData.AllColumns.Add(this.olvISSQNTribut);
		this.lsvData.AllColumns.Add(this.olvISSQNTPret);
		this.lsvData.AllColumns.Add(this.olvISSQNTaxa);
		this.lsvData.AllColumns.Add(this.olvISSQNValor);
		this.lsvData.AllColumns.Add(this.olvTpImunidade);
		this.lsvData.AllColumns.Add(this.olvTotalRet);
		this.lsvData.AllColumns.Add(this.olvPISBase);
		this.lsvData.AllColumns.Add(this.olvPISTaxa);
		this.lsvData.AllColumns.Add(this.olvPISValor);
		this.lsvData.AllColumns.Add(this.olvPISCdCst);
		this.lsvData.AllColumns.Add(this.olvCOFINSBase);
		this.lsvData.AllColumns.Add(this.olvCOFINSTaxa);
		this.lsvData.AllColumns.Add(this.olvCOFINSValor);
		this.lsvData.AllColumns.Add(this.olvCOFINSCdCst);
		this.lsvData.AllColumns.Add(this.olvPISCoftPret);
		this.lsvData.AllColumns.Add(this.olvPISCofvRetCP);
		this.lsvData.AllColumns.Add(this.olvPISCofvRetIRRF);
		this.lsvData.AllColumns.Add(this.olvPISCofvRetCSLL);
		this.lsvData.AllColumns.Add(this.olvEmitUf);
		this.lsvData.AllColumns.Add(this.olvEmitcMun);
		this.lsvData.AllColumns.Add(this.olvEmitIM);
		this.lsvData.AllColumns.Add(this.olvcMunNFSeMun);
		this.lsvData.AllColumns.Add(this.olvcLocEmi);
		this.lsvData.AllColumns.Add(this.olvxLocEmi);
		this.lsvData.AllColumns.Add(this.olvTomaID);
		this.lsvData.AllColumns.Add(this.olvTomaNome);
		this.lsvData.AllColumns.Add(this.olvTomaIM);
		this.lsvData.AllColumns.Add(this.olvPrestID);
		this.lsvData.AllColumns.Add(this.olvPrestNome);
		this.lsvData.AllColumns.Add(this.olvPrestIM);
		this.lsvData.AllColumns.Add(this.olvcLocPrest);
		this.lsvData.AllColumns.Add(this.olvxLocPrest);
		this.lsvData.AllColumns.Add(this.olvInterID);
		this.lsvData.AllColumns.Add(this.olvInterNome);
		this.lsvData.AllColumns.Add(this.olvInterIM);
		this.lsvData.AllColumns.Add(this.olvEstado);
		this.lsvData.AllColumns.Add(this.olvChave);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvcStat);
		this.lsvData.AllColumns.Add(this.olvnDpsServ);
		this.lsvData.AllColumns.Add(this.olvTpEmit);
		this.lsvData.AllColumns.Add(this.olvProcEmi);
		this.lsvData.AllColumns.Add(this.olvDtCompet);
		this.lsvData.AllColumns.Add(this.olvcTribMun);
		this.lsvData.AllColumns.Add(this.olvxTribMun);
		this.lsvData.AllColumns.Add(this.olvnProcesso);
		this.lsvData.AllColumns.Add(this.olvnNFSeMun);
		this.lsvData.AllColumns.Add(this.olvcVerifNFSeMun);
		this.lsvData.AllColumns.Add(this.olvRegEspTrib);
		this.lsvData.AllColumns.Add(this.olvRegApTribSN);
		this.lsvData.AllColumns.Add(this.olvTpSusp);
		this.lsvData.AllColumns.Add(this.olvTpAmb);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvTagUser);
		this.lsvData.AllColumns.Add(this.olvTagDtHr);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvDocNoteUser);
		this.lsvData.AllColumns.Add(this.olvDocNoteDtHr);
		this.lsvData.AllColumns.Add(this.olvVersion);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[84]
		{
			this.olvSelect, this.olvHasXml, this.olvEmitida, this.olvEmitID, this.olvEmitNome, this.olvNum, this.olvSerie, this.olvTipo, this.olvTpDoc, this.olvTag,
			this.olvDtAut, this.olvDtEmi, this.olvHrEmi, this.olvStatus, this.olvDtEntFisc, this.olvDtRegFisc, this.olvUsRegFisc, this.olvDcNumFisc, this.olvxProd, this.olvNCM,
			this.olvIndSimplesNac, this.olvCFOP, this.olvValor, this.olvISSQNCdSrv, this.olvISSQNTribut, this.olvISSQNTPret, this.olvISSQNTaxa, this.olvISSQNValor, this.olvTpImunidade, this.olvTotalRet,
			this.olvPISBase, this.olvPISTaxa, this.olvPISValor, this.olvPISCdCst, this.olvCOFINSBase, this.olvCOFINSTaxa, this.olvCOFINSValor, this.olvCOFINSCdCst, this.olvPISCoftPret, this.olvPISCofvRetCP,
			this.olvPISCofvRetIRRF, this.olvPISCofvRetCSLL, this.olvEmitUf, this.olvEmitcMun, this.olvEmitIM, this.olvcMunNFSeMun, this.olvcLocEmi, this.olvxLocEmi, this.olvTomaID, this.olvTomaNome,
			this.olvTomaIM, this.olvPrestID, this.olvPrestNome, this.olvPrestIM, this.olvcLocPrest, this.olvxLocPrest, this.olvInterID, this.olvInterNome, this.olvInterIM, this.olvEstado,
			this.olvChave, this.olvFilial, this.olvNatOper, this.olvcStat, this.olvnDpsServ, this.olvTpEmit, this.olvProcEmi, this.olvDtCompet, this.olvcTribMun, this.olvxTribMun,
			this.olvnProcesso, this.olvnNFSeMun, this.olvcVerifNFSeMun, this.olvRegEspTrib, this.olvRegApTribSN, this.olvTpSusp, this.olvTpAmb, this.olvAnoMes, this.olvTagUser, this.olvTagDtHr,
			this.olvDocNote, this.olvDocNoteUser, this.olvDocNoteDtHr, this.olvVersion
		});
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvData.FullRowSelect = true;
		this.lsvData.HideSelection = false;
		this.lsvData.IncludeColumnHeadersInCopy = true;
		this.lsvData.Location = new System.Drawing.Point(0, 0);
		this.lsvData.MenuLabelColumns = "Colunas";
		this.lsvData.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lsvData.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lsvData.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lsvData.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lsvData.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lsvData.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lsvData.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lsvData.MenuLabelUnsort = "Remover ordenação";
		this.lsvData.Name = "lsvData";
		this.lsvData.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.ModelDialog;
		this.lsvData.ShowCommandMenuOnRightClick = true;
		this.lsvData.ShowGroups = false;
		this.lsvData.ShowImagesOnSubItems = true;
		this.lsvData.ShowItemCountOnGroups = true;
		this.lsvData.Size = new System.Drawing.Size(784, 116);
		this.lsvData.SmallImageList = this.ImageListDocs;
		this.lsvData.SortGroupItemsByPrimaryColumn = false;
		this.lsvData.SpaceBetweenGroups = 5;
		this.lsvData.TabIndex = 0;
		this.lsvData.TintSortColumn = true;
		this.lsvData.UseCellFormatEvents = true;
		this.lsvData.UseCompatibleStateImageBehavior = false;
		this.lsvData.UseFilterIndicator = true;
		this.lsvData.UseFiltering = true;
		this.lsvData.UseHotControls = false;
		this.lsvData.UseHyperlinks = true;
		this.lsvData.View = System.Windows.Forms.View.Details;
		this.lsvData.VirtualMode = true;
		this.lsvData.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(lsvData_FormatCell);
		this.lsvData.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(lsvData_FormatRow);
		this.lsvData.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(lsvData_HeaderCheckBoxChanging);
		this.lsvData.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(lsvData_HyperlinkClicked);
		this.lsvData.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(lsvData_ItemsChanged);
		this.lsvData.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lsvData_ColumnWidthChanged);
		this.lsvData.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvData_ItemChecked);
		this.lsvData.DoubleClick += new System.EventHandler(lsvData_DoubleClick);
		this.lsvData.KeyUp += new System.Windows.Forms.KeyEventHandler(lsvData_KeyUp);
		this.lsvData.MouseDown += new System.Windows.Forms.MouseEventHandler(lsvData_MouseDown);
		this.olvSelect.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelect.Groupable = false;
		this.olvSelect.HeaderCheckBox = true;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvHasXml.AspectName = "HasXml";
		this.olvHasXml.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.IsEditable = false;
		this.olvHasXml.Searchable = false;
		this.olvHasXml.Text = "XML";
		this.olvHasXml.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.ToolTipText = "Tem XML vinculado";
		this.olvHasXml.UseFiltering = false;
		this.olvHasXml.Width = 36;
		this.olvEmitida.AspectName = "Emitida";
		this.olvEmitida.Text = "Emissor";
		this.olvEmitida.Width = 80;
		this.olvEmitID.AspectName = "EmitID";
		this.olvEmitID.Text = "Emitente CNPJ/CPF";
		this.olvEmitID.ToolTipText = " Número da inscrição federal (CNPJ) do emitente da NFS-e";
		this.olvEmitID.Width = 125;
		this.olvEmitNome.AspectName = "EmitNome";
		this.olvEmitNome.Text = "Emitente Nome";
		this.olvEmitNome.ToolTipText = "Nome do emitente da NFS-e";
		this.olvEmitNome.Width = 120;
		this.olvNum.AspectName = "Num";
		this.olvNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.Hyperlink = true;
		this.olvNum.Text = "Num";
		this.olvNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.ToolTipText = "Número do documento fiscal";
		this.olvNum.Width = 100;
		this.olvSerie.AspectName = "Serie";
		this.olvSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.Text = "Serie";
		this.olvSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.ToolTipText = "Série do documento fiscal";
		this.olvSerie.Width = 44;
		this.olvTipo.AspectName = "Model";
		this.olvTipo.Text = "Tipo";
		this.olvTipo.ToolTipText = "Modelo do Documento Fiscal";
		this.olvTpDoc.AspectName = "TpDoc";
		this.olvTpDoc.Text = "TipoDoc";
		this.olvTpDoc.ToolTipText = "Tipo de documento";
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.ToolTipText = "Etiqueta atribuída ao documento";
		this.olvTag.Width = 40;
		this.olvDtAut.AspectName = "DtAut";
		this.olvDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.Text = "DtAut";
		this.olvDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.ToolTipText = "Data de autorização";
		this.olvDtAut.Width = 71;
		this.olvDtEmi.AspectName = "DtEmi";
		this.olvDtEmi.Text = "DtEmi";
		this.olvDtEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmi.ToolTipText = "Data de emissão";
		this.olvDtEmi.Width = 71;
		this.olvHrEmi.AspectName = "HrEmi";
		this.olvHrEmi.Text = "HrEmi";
		this.olvHrEmi.ToolTipText = "Hora de emissão";
		this.olvStatus.Text = "Status";
		this.olvStatus.Width = 50;
		this.olvDtEntFisc.AspectName = "DtEntFisc";
		this.olvDtEntFisc.Text = "DtLancFisc";
		this.olvDtEntFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEntFisc.ToolTipText = "Data de contabilização no sistema de gestão fiscal";
		this.olvDtEntFisc.Width = 71;
		this.olvDtRegFisc.AspectName = "DtRegFisc";
		this.olvDtRegFisc.Text = "DtDigFisc";
		this.olvDtRegFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtRegFisc.ToolTipText = "Data de digitação no sistema de gestão fiscal";
		this.olvDtRegFisc.Width = 71;
		this.olvUsRegFisc.AspectName = "UsRegFisc";
		this.olvUsRegFisc.Text = "UsrDigFisc";
		this.olvUsRegFisc.ToolTipText = "Usuário de digitação no sistema de gestão fiscal";
		this.olvUsRegFisc.Width = 71;
		this.olvDcNumFisc.AspectName = "DcNumFisc";
		this.olvDcNumFisc.Text = "DocFiscal";
		this.olvDcNumFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDcNumFisc.ToolTipText = "Documento no sistema de gestão fiscal";
		this.olvDcNumFisc.Width = 71;
		this.olvxProd.AspectName = "xProd";
		this.olvxProd.Text = "Descrição Serviço";
		this.olvxProd.Width = 142;
		this.olvNCM.AspectName = "NCM";
		this.olvNCM.Text = "NBS";
		this.olvNCM.ToolTipText = "Nomenclatura comum do serviço ";
		this.olvIndSimplesNac.AspectName = "IndSimplesNac";
		this.olvIndSimplesNac.Text = "Simples Nacional";
		this.olvIndSimplesNac.ToolTipText = "Indica se o contribuinte é simples nacional";
		this.olvIndSimplesNac.Width = 120;
		this.olvCFOP.AspectName = "CFOP";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.ToolTipText = "Código Fiscal de Operações e Prestações";
		this.olvValor.AspectName = "Valor";
		this.olvValor.Text = "Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.ToolTipText = "Valor da NFS-e";
		this.olvISSQNCdSrv.AspectName = "ISSQNCdSrv";
		this.olvISSQNCdSrv.Text = "ISSQN CodTribNac";
		this.olvISSQNCdSrv.ToolTipText = "Código de tributação nacional do ISSQN";
		this.olvISSQNCdSrv.Width = 142;
		this.olvISSQNTribut.AspectName = "ISSQNTribut";
		this.olvISSQNTribut.Text = "ISSQN Tributação";
		this.olvISSQNTribut.ToolTipText = "Tributação do ISSQN sobre o serviço prestado";
		this.olvISSQNTribut.Width = 134;
		this.olvISSQNTPret.AspectName = "ISSQNTPret";
		this.olvISSQNTPret.Text = "ISSQN Tipo Retenção";
		this.olvISSQNTPret.ToolTipText = "Tipo de retençãoo do ISSQN";
		this.olvISSQNTPret.Width = 159;
		this.olvISSQNTaxa.AspectName = "ISSQNTaxa";
		this.olvISSQNTaxa.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvISSQNTaxa.Text = "ISSQN Taxa";
		this.olvISSQNTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvISSQNTaxa.ToolTipText = "Taxa de cálculo do ISSQN";
		this.olvISSQNTaxa.Width = 100;
		this.olvISSQNValor.AspectName = "ISSQNValor";
		this.olvISSQNValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvISSQNValor.Text = "ISSQN Valor";
		this.olvISSQNValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvISSQNValor.ToolTipText = "Valor do ISSQN ";
		this.olvISSQNValor.Width = 100;
		this.olvTpImunidade.AspectName = "tpImunidade";
		this.olvTpImunidade.Text = "ISSQN Imunidade";
		this.olvTpImunidade.ToolTipText = "Identificação da Imunidade do ISSQN";
		this.olvTpImunidade.Width = 130;
		this.olvTotalRet.AspectName = "TotalRet";
		this.olvTotalRet.Text = "Valor Total";
		this.olvTotalRet.ToolTipText = "Valor total de retenções";
		this.olvTotalRet.Width = 90;
		this.olvPISBase.AspectName = "PISBase";
		this.olvPISBase.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPISBase.Text = "PIS Base";
		this.olvPISBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvPISBase.ToolTipText = "Valor da base de cálculo do PIS";
		this.olvPISBase.Width = 100;
		this.olvPISTaxa.AspectName = "PISTaxa";
		this.olvPISTaxa.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPISTaxa.Text = "PIS Taxa";
		this.olvPISTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvPISTaxa.ToolTipText = "Taxa de cálculo do PIS";
		this.olvPISTaxa.Width = 100;
		this.olvPISValor.AspectName = "PISValor";
		this.olvPISValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPISValor.Text = "PIS Valor";
		this.olvPISValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvPISValor.ToolTipText = "Valor do PIS";
		this.olvPISValor.Width = 100;
		this.olvPISCdCst.AspectName = "PISCdCst";
		this.olvPISCdCst.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPISCdCst.Text = "PIS COF CST";
		this.olvPISCdCst.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPISCdCst.ToolTipText = "Código de situação tributária do PIS ";
		this.olvPISCdCst.Width = 100;
		this.olvCOFINSBase.AspectName = "COFINSBase";
		this.olvCOFINSBase.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCOFINSBase.Text = "COFINS Base";
		this.olvCOFINSBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvCOFINSBase.ToolTipText = "Valor da base de cálculo da COFINS";
		this.olvCOFINSBase.Width = 100;
		this.olvCOFINSTaxa.AspectName = "COFINSTaxa";
		this.olvCOFINSTaxa.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCOFINSTaxa.Text = "COFINS Taxa";
		this.olvCOFINSTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvCOFINSTaxa.ToolTipText = "Taxa de cálculo da CONFIS";
		this.olvCOFINSTaxa.Width = 100;
		this.olvCOFINSValor.AspectName = "COFINSValor";
		this.olvCOFINSValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCOFINSValor.Text = "COFINS Valor";
		this.olvCOFINSValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvCOFINSValor.ToolTipText = "Valor da COFINS ";
		this.olvCOFINSValor.Width = 100;
		this.olvCOFINSCdCst.AspectName = "COFINSCdCst";
		this.olvCOFINSCdCst.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCOFINSCdCst.Text = "COFINS CST";
		this.olvCOFINSCdCst.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCOFINSCdCst.ToolTipText = "Código de situação tributária da COFINS ";
		this.olvCOFINSCdCst.Width = 100;
		this.olvPISCoftPret.AspectName = "PISCoftPret";
		this.olvPISCoftPret.Text = "PIS/COFINS Tipo Retenção";
		this.olvPISCoftPret.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPISCoftPret.ToolTipText = "Tipo de retencao do Pis/Cofins";
		this.olvPISCoftPret.Width = 200;
		this.olvPISCofvRetCP.AspectName = "PISCofvRetCP";
		this.olvPISCofvRetCP.Text = "CP Valor Mon";
		this.olvPISCofvRetCP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvPISCofvRetCP.ToolTipText = "Valor monetário do CP(R$)";
		this.olvPISCofvRetCP.Width = 100;
		this.olvPISCofvRetIRRF.AspectName = "PISCofvRetIRRF";
		this.olvPISCofvRetIRRF.Text = "IRRF Valor Mon";
		this.olvPISCofvRetIRRF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvPISCofvRetIRRF.ToolTipText = "Valor monetário do IRRF (R$)";
		this.olvPISCofvRetIRRF.Width = 115;
		this.olvPISCofvRetCSLL.AspectName = "PISCofvRetCSLL";
		this.olvPISCofvRetCSLL.Text = "CSLL Valor Mon";
		this.olvPISCofvRetCSLL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvPISCofvRetCSLL.ToolTipText = "Valor monetário do CSLL (R$)";
		this.olvPISCofvRetCSLL.Width = 110;
		this.olvEmitUf.AspectName = "EmitUf";
		this.olvEmitUf.Text = "Emitente UF";
		this.olvEmitUf.ToolTipText = "UF Emitente";
		this.olvEmitUf.Width = 100;
		this.olvEmitcMun.AspectName = "EmitcMun";
		this.olvEmitcMun.Text = "Emitente Município";
		this.olvEmitcMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitcMun.ToolTipText = "Cód. Municipío Emitente";
		this.olvEmitcMun.Width = 150;
		this.olvEmitIM.AspectName = "EmitIM";
		this.olvEmitIM.Text = "Emitente Insc. Mun.";
		this.olvEmitIM.ToolTipText = "Número de inscrição municipal do emitente da NFS-e";
		this.olvEmitIM.Width = 140;
		this.olvcMunNFSeMun.AspectName = "cMunNFSeMun";
		this.olvcMunNFSeMun.Text = "Emissor Cód. Mun.";
		this.olvcMunNFSeMun.ToolTipText = "Código Município emissor da nota eletrônica municipal";
		this.olvcMunNFSeMun.Width = 135;
		this.olvcLocEmi.AspectName = "cLocEmi";
		this.olvcLocEmi.Text = "Local Emitente : Código";
		this.olvcLocEmi.ToolTipText = "Código da localidade da emissão do serviço";
		this.olvcLocEmi.Width = 100;
		this.olvxLocEmi.AspectName = "xLocEmi";
		this.olvxLocEmi.Text = "Local Emitente : Descrição";
		this.olvxLocEmi.ToolTipText = "Munícipio referente ao local da emissão do serviço";
		this.olvxLocEmi.Width = 100;
		this.olvTomaID.AspectName = "TomaID";
		this.olvTomaID.Text = "Tomador CNPJ/CPF";
		this.olvTomaID.ToolTipText = "Número de Inscrição Federal do Tomador";
		this.olvTomaID.Width = 140;
		this.olvTomaNome.AspectName = "TomaNome";
		this.olvTomaNome.Text = "Tomador Nome";
		this.olvTomaNome.ToolTipText = "Nome do Tomador";
		this.olvTomaNome.Width = 135;
		this.olvTomaIM.AspectName = "TomaIM";
		this.olvTomaIM.Text = "Tomador Insc. Mun.";
		this.olvTomaIM.ToolTipText = "Inscrição Municipal do Tomador";
		this.olvTomaIM.Width = 130;
		this.olvPrestID.AspectName = "PrestID";
		this.olvPrestID.Text = "Prestador CNPJ/CPF";
		this.olvPrestID.ToolTipText = "Número de Inscrição Federal do Prestador da NFS-e";
		this.olvPrestID.Width = 130;
		this.olvPrestNome.AspectName = "PrestNome";
		this.olvPrestNome.Text = "Prestador Nome";
		this.olvPrestNome.ToolTipText = "Nome do Prestador";
		this.olvPrestNome.Width = 120;
		this.olvPrestIM.AspectName = "PrestIM";
		this.olvPrestIM.Text = "Prestador Insc. Mun.";
		this.olvPrestIM.ToolTipText = "Inscrição Municipal Prestador";
		this.olvPrestIM.Width = 140;
		this.olvcLocPrest.AspectName = "cLocPrest";
		this.olvcLocPrest.Text = "Local Prestação : Código";
		this.olvcLocPrest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvcLocPrest.ToolTipText = "Código da localidade da prestação do serviço";
		this.olvcLocPrest.Width = 120;
		this.olvxLocPrest.AspectName = "xLocPrest";
		this.olvxLocPrest.Text = "Local Prestação : Descrição";
		this.olvxLocPrest.ToolTipText = "Munícipio referente ao local da prestação do serviço";
		this.olvxLocPrest.Width = 100;
		this.olvInterID.AspectName = "InterID";
		this.olvInterID.Text = "Intermediário CNPJ/CPF";
		this.olvInterID.ToolTipText = "Número de Inscrição ";
		this.olvInterID.Width = 160;
		this.olvInterNome.AspectName = "InterNome";
		this.olvInterNome.Text = "Intermediário Nome";
		this.olvInterNome.ToolTipText = "Nome do Intermediário";
		this.olvInterNome.Width = 130;
		this.olvInterIM.AspectName = "InterIM";
		this.olvInterIM.Text = "Intermediário Insc. Mun.";
		this.olvInterIM.ToolTipText = "Inscrição Municipal do Intermediário";
		this.olvInterIM.Width = 160;
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do Documento Fiscal";
		this.olvEstado.Width = 30;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "Chave";
		this.olvChave.ToolTipText = "Chave de acesso";
		this.olvChave.Width = 53;
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 130;
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "Natureza";
		this.olvNatOper.ToolTipText = "Descrição da natureza da operação";
		this.olvNatOper.Width = 600;
		this.olvcStat.AspectName = "cStat";
		this.olvcStat.Text = "Situação";
		this.olvcStat.ToolTipText = "Situação da NFS-e";
		this.olvcStat.Width = 65;
		this.olvnDpsServ.AspectName = "nDpsServ";
		this.olvnDpsServ.Text = "Número da DPS";
		this.olvnDpsServ.ToolTipText = "Número da DPS";
		this.olvnDpsServ.Width = 120;
		this.olvTpEmit.AspectName = "TpEmit";
		this.olvTpEmit.Text = "Tipo de Emitente DPS";
		this.olvTpEmit.ToolTipText = "Tipo de Emitente da DPS";
		this.olvTpEmit.Width = 135;
		this.olvProcEmi.AspectName = "ProcEmi";
		this.olvProcEmi.Text = "Emissão DPS";
		this.olvProcEmi.ToolTipText = "Processo de Emissão da DPS";
		this.olvProcEmi.Width = 140;
		this.olvDtCompet.AspectName = "DtCompet";
		this.olvDtCompet.Text = "DtCompetência";
		this.olvDtCompet.ToolTipText = "Data de competência da prestação do serviço";
		this.olvDtCompet.Width = 110;
		this.olvcTribMun.AspectName = "cTribMun";
		this.olvcTribMun.Text = "ISSQN Tributação : Código";
		this.olvcTribMun.ToolTipText = "Descrição do código de tributação municipal do ISSQN";
		this.olvcTribMun.Width = 30;
		this.olvxTribMun.AspectName = "xTribMun";
		this.olvxTribMun.Text = "ISSQN Tributação : Descrição";
		this.olvxTribMun.ToolTipText = "Descrição da tributação municipal do ISSQN";
		this.olvxTribMun.Width = 175;
		this.olvnProcesso.AspectName = "nProcesso";
		this.olvnProcesso.Text = "Num. Processo";
		this.olvnProcesso.ToolTipText = "Número do processo judicial ou administrativo de suspensão da exigibilidade";
		this.olvnProcesso.Width = 100;
		this.olvnNFSeMun.AspectName = "nNFSeMun";
		this.olvnNFSeMun.Text = "Número Nota";
		this.olvnNFSeMun.ToolTipText = "Número da nota eletrônica municipal";
		this.olvnNFSeMun.Width = 90;
		this.olvcVerifNFSeMun.AspectName = "cVerifNFSeMun";
		this.olvcVerifNFSeMun.Text = "Cód. Verificação";
		this.olvcVerifNFSeMun.ToolTipText = "Código de Verificação da nota eletrônica municipal";
		this.olvcVerifNFSeMun.Width = 110;
		this.olvRegEspTrib.AspectName = "RegEspTrib";
		this.olvRegEspTrib.Text = "Tipo Reg. Tributação";
		this.olvRegEspTrib.ToolTipText = "Tipos de Regimes Especiais de Tributação Municipal";
		this.olvRegEspTrib.Width = 135;
		this.olvRegApTribSN.AspectName = "RegApTribSN";
		this.olvRegApTribSN.Text = "Reg. Apuração Trib";
		this.olvRegApTribSN.ToolTipText = "Regime de Apuração Tributária pelo Simples Nacional";
		this.olvRegApTribSN.Width = 135;
		this.olvTpSusp.AspectName = "tpSusp";
		this.olvTpSusp.Text = "Exigibilidade Suspensa";
		this.olvTpSusp.ToolTipText = "Opção para Exigibilidade Suspensa";
		this.olvTpSusp.Width = 135;
		this.olvTpAmb.AspectName = "TpAmb";
		this.olvTpAmb.Text = "Tipo de Ambiente";
		this.olvTpAmb.ToolTipText = "Identificação do tipo de ambiente no Sistema Nacional NFS-e";
		this.olvTpAmb.Width = 120;
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Data de autorização";
		this.olvTagUser.AspectName = "TagUser";
		this.olvTagUser.Text = "Etiq : Usuário";
		this.olvTagUser.ToolTipText = "Usuário que atribuiu a etiqueta";
		this.olvTagDtHr.AspectName = "TagDtHr";
		this.olvTagDtHr.Text = "Etiq : Data Hora";
		this.olvTagDtHr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTagDtHr.ToolTipText = "Data/hora de atribuição da etiqueta";
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvDocNote.ToolTipText = "Comentários";
		this.olvDocNote.Width = 90;
		this.olvDocNoteUser.AspectName = "DocNoteUser";
		this.olvDocNoteUser.Text = "Comentário : Usuário";
		this.olvDocNoteUser.ToolTipText = "Usuário que atribuiu o comentário";
		this.olvDocNoteDtHr.AspectName = "DocNoteDtHr";
		this.olvDocNoteDtHr.Text = "Comentário : Data Hora";
		this.olvDocNoteDtHr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocNoteDtHr.ToolTipText = "Data/hora de atribuição do comentário";
		this.olvVersion.AspectName = "Version";
		this.olvVersion.Text = "XML: Versão";
		this.olvVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVersion.ToolTipText = "Versão do XML";
		this.olvVersion.Width = 100;
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_xml_green.png");
		this.ImageListDocs.Images.SetKeyName(1, "image_xml_brown.png");
		this.ImageListDocs.Images.SetKeyName(2, "image_xml_red.png");
		this.ImageListDocs.Images.SetKeyName(3, "dfe_approved.png");
		this.ImageListDocs.Images.SetKeyName(4, "dfe_canceled.png");
		this.ImageListDocs.Images.SetKeyName(5, "dfe_transport.png");
		this.ImageListDocs.Images.SetKeyName(6, "dfe_mdfedoc.png");
		this.ImageListDocs.Images.SetKeyName(7, "dfe_acknow.png");
		this.ImageListDocs.Images.SetKeyName(8, "dfe_confirm.png");
		this.ImageListDocs.Images.SetKeyName(9, "dfe_disagree.png");
		this.ImageListDocs.Images.SetKeyName(10, "dfe_unknow.png");
		this.ImageListDocs.Images.SetKeyName(11, "image_zfmvist.png");
		this.ImageListDocs.Images.SetKeyName(12, "image_zfminte.png");
		this.ImageListDocs.Images.SetKeyName(13, "image_lancfiscal.png");
		this.ImageListDocs.Images.SetKeyName(14, "image_notes.png");
		this.ImageListDocs.Images.SetKeyName(15, "image_locker_16_16.png");
		this.ImageListDocs.Images.SetKeyName(16, "image_status_initial.png");
		this.ImageListDocs.Images.SetKeyName(17, "image_status_running.png");
		this.ImageListDocs.Images.SetKeyName(18, "image_status_finished.png");
		this.olvTomaIE.AspectName = "TomaIE";
		this.olvTomaIE.DisplayIndex = 21;
		this.olvTomaIE.Text = "Tomador IE";
		this.olvTomaIE.ToolTipText = "Número de Inscrição Estadual do Tomador";
		this.olvTomaIE.Width = 135;
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 117);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(784, 328);
		this.pnMarket.TabIndex = 12;
		this.pnMarket.Visible = false;
		this.lknClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lknClose.AutoSize = true;
		this.lknClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknClose.Location = new System.Drawing.Point(600, 304);
		this.lknClose.Name = "lknClose";
		this.lknClose.Size = new System.Drawing.Size(176, 16);
		this.lknClose.TabIndex = 204;
		this.lknClose.TabStop = true;
		this.lknClose.Text = "Não mostrar mais esse alerta";
		this.lknClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lknClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknClose_LinkClicked);
		this.pnMarketContent.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pnMarketContent.Controls.Add(this.btSalesContact);
		this.pnMarketContent.Controls.Add(this.btSalesAction);
		this.pnMarketContent.Controls.Add(this.label3);
		this.pnMarketContent.Controls.Add(this.lbTitle03);
		this.pnMarketContent.Controls.Add(this.picWarning);
		this.pnMarketContent.Controls.Add(this.lknAction);
		this.pnMarketContent.Controls.Add(this.lbText02);
		this.pnMarketContent.Controls.Add(this.lbText01);
		this.pnMarketContent.Controls.Add(this.lbTitle01);
		this.pnMarketContent.Controls.Add(this.lbTitle02);
		this.pnMarketContent.Location = new System.Drawing.Point(115, 28);
		this.pnMarketContent.Name = "pnMarketContent";
		this.pnMarketContent.Size = new System.Drawing.Size(566, 264);
		this.pnMarketContent.TabIndex = 195;
		this.btSalesContact.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesContact.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesContact.ForeColor = System.Drawing.Color.White;
		this.btSalesContact.Location = new System.Drawing.Point(90, 224);
		this.btSalesContact.Name = "btSalesContact";
		this.btSalesContact.Size = new System.Drawing.Size(188, 28);
		this.btSalesContact.TabIndex = 222;
		this.btSalesContact.Text = "Solicitar Contato";
		this.btSalesContact.UseVisualStyleBackColor = false;
		this.btSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		this.btSalesAction.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btSalesAction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesAction.ForeColor = System.Drawing.Color.White;
		this.btSalesAction.Location = new System.Drawing.Point(309, 224);
		this.btSalesAction.Name = "btSalesAction";
		this.btSalesAction.Size = new System.Drawing.Size(188, 28);
		this.btSalesAction.TabIndex = 208;
		this.btSalesAction.Text = "Conheça os planos";
		this.btSalesAction.UseVisualStyleBackColor = false;
		this.btSalesAction.Visible = false;
		this.btSalesAction.Click += new System.EventHandler(btSalesAction_Click);
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.BackColor = System.Drawing.Color.Gainsboro;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Gainsboro;
		this.label3.Location = new System.Drawing.Point(3, 44);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(560, 1);
		this.label3.TabIndex = 207;
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle03.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle03.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle03.Location = new System.Drawing.Point(17, 188);
		this.lbTitle03.Name = "lbTitle03";
		this.lbTitle03.Size = new System.Drawing.Size(88, 17);
		this.lbTitle03.TabIndex = 204;
		this.lbTitle03.Text = "Saiba mais:";
		this.picWarning.Image = Monitor.Resources.image_help;
		this.picWarning.Location = new System.Drawing.Point(20, 10);
		this.picWarning.Name = "picWarning";
		this.picWarning.Size = new System.Drawing.Size(20, 20);
		this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picWarning.TabIndex = 205;
		this.picWarning.TabStop = false;
		this.lknAction.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lknAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknAction.Location = new System.Drawing.Point(111, 188);
		this.lknAction.Name = "lknAction";
		this.lknAction.Size = new System.Drawing.Size(436, 16);
		this.lknAction.TabIndex = 203;
		this.lknAction.TabStop = true;
		this.lknAction.Text = "NFSe Dados Analíticos";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(17, 126);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 48);
		this.lbText02.TabIndex = 196;
		this.lbText02.Text = "Para visualizar todos os campos do XML disponíveis neste relatório, clique com o botão direito do mouse sobre o título de qualquer coluna do relatório e escolha a opção [Selecionar colunas...].";
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 81);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 32);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = "Apresenta a visualização de todas as informações de NFSe em nível de item.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "NFSe: Dados Analíticos é ativado nos \r\nplanos Avançado e Enterprise.";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(17, 55);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Definição do relatório";
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(753, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 3;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(784, 445);
		base.Controls.Add(this.pnMarket);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocNFSeDet";
		this.Text = "NFSe: Dados Analíticos";
		base.Load += new System.EventHandler(frmTabDocNFSeDet_Load);
		this.pnContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
