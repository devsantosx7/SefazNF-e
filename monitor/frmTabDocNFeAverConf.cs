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
using audit.fiscal.io;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using monitor.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabDocNFeAverConf : Form
{
	private clsSrvTabDocNFeAverConf _SqlTabData;

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsXmlHelper _clsXmlHelper = new clsXmlHelper();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private clsDataDueHeader _clsDataDueHeader = new clsDataDueHeader();

	private clsComexProfile _clsComexProfile;

	private bool _SelectedAllItens;

	private bool _ColumnsLoaded;

	private decimal _TotalValue;

	private string _InfoAddXml = string.Empty;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private bool _HasFiscalioConnect;

	private bool _GroupColapsed;

	private bool _IsOnHeaderCheckStatus;

	private string _SqlFields = " * ";

	private bool _IsLoading;

	private string _consMarketScreenUser = string.Empty;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private clsDataFilter _clsDataFilter = new clsDataFilter();

	private clsDataDocEvent790700 _clsDataDocEvent790700 = new clsDataDocEvent790700();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private FastObjectListView lsvData;

	private OLVColumn olvSelect;

	private OLVColumn olvInHasXml;

	private OLVColumn olvInTipo;

	private OLVColumn olvInTag;

	private OLVColumn olvInNum;

	private OLVColumn olvInSerie;

	private OLVColumn olvInDtAut;

	private OLVColumn olvInValor;

	private OLVColumn olvInCNPJ;

	private OLVColumn olvInNome;

	private OLVColumn olvInChave;

	private OLVColumn olvInFilial;

	private OLVColumn olvInCFOP;

	private OLVColumn olvnDue;

	private OLVColumn olvnItem;

	private OLVColumn olvnItemDue;

	private OLVColumn olvqItem;

	private OLVColumn olvDtEmb;

	private OLVColumn olvHrEmb;

	private OLVColumn olvDtAverb;

	private OLVColumn olvHrAverb;

	private OLVColumn olvMotAlt;

	private OLVColumn olvInNCM;

	private OLVColumn olvInError;

	private OLVColumn olvInCancel;

	private OLVColumn olvInManifest;

	private OLVColumn olvDocNote;

	private OLVColumn olvAnoMes;

	private OLVColumn olvStatus;

	private OLVColumn olvEstado;

	private OLVColumn olvTomaIE;

	private OLVColumn olvTpDoc;

	private OLVColumn olvNatOper;

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

	private ImageList ImageListDocs;

	private Button btSalesContact;

	private Button btClose;

	private ToolStrip tspTaskMenu;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton tsbSync;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton tsbDocs;

	private ToolStripSeparator tssSepDocs;

	private ToolStripButton tsbSpedCreate;

	private ToolStripSeparator toolStripSeparator7;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabDocNFeAverConf(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		olvAnoMes.Name = "AnoMes";
		lsvData.HandleDestroyed += lsvData_HandleDestroyed;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private void lsvData_HandleDestroyed(object sender, EventArgs e)
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

	private async void frmTabDocNFeAverConf_Load(object sender, EventArgs e)
	{
		_IsLoading = false;
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
			clsScreenGeral.funcCheckListViewDdic(typeof(DocEvent790700), lsvData);
		}
		if (!_HasFiscalioConnect)
		{
			List<string> varColList = new List<string> { "DtEntFisc", "DtRegFisc", "UsRegFisc", "DcNumFisc" };
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList);
		}
		if (string.IsNullOrEmpty(_InfoAddXml))
		{
			List<string> varColList2 = new List<string>();
			varColList2.Add(clsFunction.funcClearSpecialCaracter("Informações complementares"));
			varColList2.Add(clsFunction.funcClearSpecialCaracter("Informações adicionais fisco"));
			varColList2.Add(clsFunction.funcClearSpecialCaracter("Observações contribuinte"));
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList2);
		}
		lsvData.CellToolTipShowing += olv_CellToolTipShowing;
		_SqlFields = "document.TomaIE, document.HasXml, document.XmlError, ";
		_SqlFields += "document.Canceled, document.DocNote, document.cUF,  document.DtAut, ";
		_SqlFields += "document.tag, document.Model, document.Num, document.ComexAverbStat, ";
		_SqlFields += "document.ManifCode, document.Filial, document.EmitID, document.FisIoApi, document.Serie, ";
		_SqlFields += "document.Chave, document.CFOPList, document.NCMList, document.TpDoc, document.NatOper, ";
		_SqlFields += "document.Valor, document.DFeSource, document.XmlSource, document.EmitNome, Event790700.nDue, ";
		_SqlFields += "Event790700.nItem, Event790700.nItemDue, Event790700.qItem, Event790700.dtEmbarque, ";
		_SqlFields += "Event790700.hrEmbarque, Event790700.dtAverbacao, Event790700.hrAverbacao, ";
		_SqlFields += "Event790700.motAlteracao";
		_SqlTabData = new clsSrvTabDocNFeAverConf(_SqlFields);
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
			LinkLabel linkLabel = lknClose;
			bool flag2 = (tspTaskMenu.Enabled = false);
			visible = (linkLabel.Visible = flag2);
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
		_IsLoading = true;
	}

	private void funcDefineListViewFeatures()
	{
		olvInValor.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return clsFunction.IsEmpty(docEvent?.Valor) ? null : ((object)clsFunction.funcConvStrToDec(docEvent.Valor));
		};
		olvInHasXml.ImageGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(docEvent, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
		};
		olvInHasXml.AspectGetter = (object x) => string.Empty;
		olvInTag.ImageGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : ((object)clsScreenGeral.funcGetDocNotesIcon(docEvent));
		};
		olvInTag.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			if (docEvent == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docEvent.Tag) ? null : _TagHsList[docEvent.Tag];
		};
		olvInNum.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : clsFunction.funcGetValue(docEvent.Num).PadLeft(9, '0');
		};
		olvInTipo.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : clsFunction.funcGetDocType(docEvent.Model);
		};
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : clsScreenGeral.funcGetDocStatIcon(docEvent);
		};
		olvEstado.AspectGetter = delegate(object x)
		{
			DocEvent790700 varDocument = (DocEvent790700)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
		};
		olvInFilial.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : clsFunction.funcFormatDoc(docEvent.Filial);
		};
		olvInManifest.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : _clsDFeCodes.GetEventDesc(docEvent.Model, docEvent.ManifCode);
		};
		olvInCNPJ.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : clsFunction.funcFormatDoc(docEvent.EmitID);
		};
		olvAnoMes.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			if (docEvent == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = docEvent.DtAut.Substring(0, 7);
			}
			catch
			{
			}
			return result;
		};
		olvInError.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : varclsValidator.funcGetDesc(document);
		};
		olvnItem.AspectGetter = (object x) => ((DocEvent790700)x)?.nItem;
		olvnDue.AspectGetter = (object x) => ((DocEvent790700)x)?.nDue;
		olvnItemDue.AspectGetter = (object x) => ((DocEvent790700)x)?.nItemDue;
		olvqItem.AspectGetter = (object x) => ((DocEvent790700)x)?.qItem;
		olvDtEmb.AspectGetter = (object x) => ((DocEvent790700)x)?.dtEmbarque;
		olvHrEmb.AspectGetter = (object x) => ((DocEvent790700)x)?.hrEmbarque;
		olvDtAverb.AspectGetter = (object x) => ((DocEvent790700)x)?.dtAverbacao;
		olvHrAverb.AspectGetter = (object x) => ((DocEvent790700)x)?.hrAverbacao;
		olvMotAlt.AspectGetter = (object x) => ((DocEvent790700)x)?.motAlteracao;
		olvInSerie.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : (docEvent.Serie ?? string.Empty);
		};
		olvInDtAut.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : (docEvent.DtAut ?? string.Empty);
		};
		olvInCancel.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : (docEvent.Canceled ?? string.Empty);
		};
		olvInNome.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : (docEvent.EmitNome ?? string.Empty);
		};
		olvInChave.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : (docEvent.Chave ?? string.Empty);
		};
		olvInCFOP.AspectGetter = (object x) => ((DocEvent790700)x)?.CFOPList;
		olvInNCM.AspectGetter = (object x) => ((DocEvent790700)x)?.NCMList;
		olvDocNote.AspectGetter = (object x) => ((DocEvent790700)x)?.DocNote;
		olvTpDoc.AspectGetter = delegate(object x)
		{
			DocEvent790700 docEvent = (DocEvent790700)x;
			return (docEvent == null) ? null : _clsDFeCodes.funcGetTipoDoc(docEvent.Model, docEvent.TpDoc);
		};
		olvNatOper.AspectGetter = (object x) => ((DocEvent790700)x)?.NatOper;
		olvTomaIE.AspectGetter = (object x) => ((DocEvent790700)x)?.TomaIE;
	}

	private async Task<FastObjectListView> funcListGroupSortAsync(FastObjectListView plsvData, clsDataFilter pclsDataFilter)
	{
		OLVColumn varGroupColum;
		SortOrder varSortOrder;
		if (pclsDataFilter.GroupByDisable || _IsLocked)
		{
			varGroupColum = null;
			varSortOrder = SortOrder.None;
		}
		else if (pclsDataFilter.GroupByPartner)
		{
			varGroupColum = olvInNome;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByDate)
		{
			varGroupColum = olvInDtAut;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByYearMonth)
		{
			varGroupColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByState)
		{
			varGroupColum = olvEstado;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByModel)
		{
			varGroupColum = olvInTipo;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByTpDoc)
		{
			varGroupColum = olvTpDoc;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByNatOper)
		{
			varGroupColum = olvNatOper;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByCFOP)
		{
			varGroupColum = olvInCFOP;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByFilial)
		{
			varGroupColum = olvInFilial;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByTomaIE)
		{
			varGroupColum = olvTomaIE;
			varSortOrder = SortOrder.Ascending;
		}
		else
		{
			varGroupColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		plsvData = clsScreenGeral.funcSetLsvSortGroup(this, plsvData, varGroupColum, varSortOrder, olvInDtAut, SortOrder.Descending);
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

	private async void tsbDocReset_Click(object sender, EventArgs e)
	{
		List<DocEvent790700> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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

	private async void tsmTransferFilial_Click(object sender, EventArgs e)
	{
		await funcTransferFilialAsync();
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
		List<DocEvent790700> varObjList = funcGetObjList(pFocused, pChecked);
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
		List<DocEvent790700> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<DocEvent790700> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		frmDocNote obj = new frmDocNote(ref varDocList);
		obj.ShowDialog(this);
		obj.Dispose();
		funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		while (!_IsLoading)
		{
			await Task.Delay(TimeSpan.FromSeconds(1.0));
		}
		_clsDataFilter = pclsDataFilter;
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
		List<DocEvent790700> varclsDocList = await _clsDataDocEvent790700.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		olvSelect.HeaderCheckState = CheckState.Unchecked;
		lsvData.SetObjects(varclsDocList);
		_GroupColapsed = false;
		lsvData.ResumeLayout();
		lsvData.EndUpdate();
		lsvData = await funcListGroupSortAsync(lsvData, pclsDataFilter);
		return varclsReturn;
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

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportDocToExcelAsync<DocEvent790700>(lsvData, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<DocEvent790700> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<DocEvent790700> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (DocEvent790700 varObject in pObjList)
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

	public async Task<List<DueHeader>> funcGetDueListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst, bool pOnlyPend = false)
	{
		new clsDataDoc();
		List<DocEvent790700> list = funcGetObjList(pFocused, pChecked);
		List<DueHeader> varDueList = new List<DueHeader>();
		foreach (DocEvent790700 varObject in list)
		{
			DueHeader varclsDueHeader = new DueHeader
			{
				Filial = varObject.Filial,
				Num = varObject.nDue
			};
			varDueList.Add(varclsDueHeader);
		}
		if (pSyncFromDbaFirst)
		{
			varDueList = await _clsDataDueHeader.funcGetSyncListByListAsync(varDueList);
		}
		if (pOnlyPend && !clsFunction.IsAdmin)
		{
			varDueList.RemoveAll((DueHeader r) => clsFunction.Contains(r.Status, "SUCCESS", "AVERBADA", pIgnoreCase: true));
		}
		return varDueList;
	}

	public List<DocEvent790700> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<DocEvent790700> varDocList = new List<DocEvent790700>();
		DocEvent790700 varFocused = new DocEvent790700();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (DocEvent790700)lsvData.GetItem(lsvData.FocusedItem.Index).RowObject;
				varDocList.Add(varFocused);
			}
			catch
			{
				varFocused = new DocEvent790700();
			}
		}
		if (pChecked)
		{
			foreach (DocEvent790700 varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varDocList.Add(varObject);
				}
			}
			if (varDocList.Count > 0)
			{
				varDocList = lsvData.Objects.Cast<DocEvent790700>().Intersect(varDocList).ToList();
			}
		}
		if ((!pFocused && !pChecked) || varDocList.Count <= 0)
		{
			varDocList = lsvData.FilteredObjects.Cast<DocEvent790700>().ToList();
			if (varDocList.Count <= 0)
			{
				varDocList = lsvData.Objects.Cast<DocEvent790700>().ToList();
			}
		}
		return varDocList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<DocEvent790700> varObjList = funcGetObjList(pFocused, pChecked);
		funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
		return true;
	}

	private async void funcRefreshListAsync(List<Document> pDocList, List<DocEvent790700> pObjList)
	{
		ConcurrentBag<ObjFieldBuffer> varDocFieldList = clsObjectBuffer.funcGet<Document>().Fields;
		ConcurrentBag<ObjFieldBuffer> varObjFieldList = clsObjectBuffer.funcGet<DocEvent790700>().Fields;
		await Task.WhenAll(((IEnumerable<DocEvent790700>)pObjList).Select((Func<DocEvent790700, Task>)async delegate(DocEvent790700 varclsObjItem)
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
		clsHelpService.funcCallTipReportExportAverbationAsync();
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private void lsvData_FormatCell(object sender, FormatCellEventArgs e)
	{
		if (e.ColumnIndex == olvInTag.Index)
		{
			Document varDocument = (Document)e.Model;
			if (varDocument != null && !clsFunction.IsEmpty(varDocument.Tag))
			{
				string varColorName = (string)_HasColors[varDocument.Tag];
				e.SubItem.BackColor = ColorTranslator.FromHtml(varColorName);
			}
		}
	}

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		Document varDocument = (Document)e.Model;
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

	private void funcSetTabDataTotalValues()
	{
		if (!_IsOnHeaderCheckStatus)
		{
			IEnumerable varEnumList = null;
			FastObjectListView fastObjectListView = lsvData;
			varEnumList = ((fastObjectListView == null || !(fastObjectListView.CheckedObjects?.Count > 0)) ? lsvData.FilteredObjects : lsvData.CheckedObjectsEnumerable);
			List<Document> varObjList = varEnumList.Cast<Document>().ToList();
			if (varObjList == null)
			{
				varObjList = new List<Document>();
			}
			_TotalValue = varObjList.Sum((Document r) => clsFunction.funcConvStrToDec(r.Valor));
			EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
			varArguments.TotalValue = _TotalValue;
			varArguments.TotalQuant = varObjList.Count;
			OnEventTabManager(varArguments);
		}
	}

	private void lsvData_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
	{
		if (e.ColumnIndex == olvInNum.Index)
		{
			Document varDocument = (Document)e.Model;
			if (varDocument != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varDocument);
			}
		}
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

	private void lsvData_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		funcSetTabDataTotalValues();
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

	private void lsvData_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (!e.Item.Checked)
		{
			olvSelect.HeaderCheckState = CheckState.Unchecked;
		}
		funcSetTabDataTotalValues();
	}

	private void lsvData_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false);
	}

	private async void lsvData_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode.Equals(Keys.C))
		{
			await funcCopyDocKeyAsync();
		}
		if (e.Control && e.KeyCode.Equals(Keys.T))
		{
			await funcTransferFilialAsync();
		}
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
			if (clsFunction.IsEqual(varclsValue.Name, "Document"))
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

	private void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		Document varDocument = (Document)e.Model;
		if (varDocument != null)
		{
			_ = string.Empty;
			if (e.ColumnIndex.Equals(olvInHasXml.Index))
			{
				e.Text = varclsValidator.funcGetDesc(varDocument);
			}
			else if (e.ColumnIndex.Equals(olvStatus.Index))
			{
				e.Text = clsScreenGeral.funcGetDocStatDesc(varDocument);
			}
			else if (e.ColumnIndex.Equals(olvInTag.Index))
			{
				e.Text = varDocument.DocNote;
			}
		}
	}

	private async void tsbSync_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsComexApi varclsComexApi = new clsComexApi();
		try
		{
			tsbSync.Enabled = false;
			tsbSync.Text = "Sincronizando...";
			plnMessage.Visible = true;
			Application.DoEvents();
			List<DueHeader> varclsDbaDueList = await funcGetDueListAsync(pFocused: false, pChecked: true, pSyncFromDbaFirst: true, pOnlyPend: true);
			if (varclsDbaDueList.Count <= 0)
			{
				return;
			}
			List<DueHeader> varclsExcDueList = new List<DueHeader>();
			foreach (DueHeader varclsDbaDueItem in varclsDbaDueList)
			{
				if (!varclsExcDueList.Any((DueHeader r) => clsFunction.IsEqual(r.Num, varclsDbaDueItem.Num)))
				{
					varclsExcDueList.Add(varclsDbaDueItem);
				}
			}
			int varCounter = 0;
			int varTotal = varclsExcDueList.Count;
			FilialView varclsFilial = new FilialView();
			foreach (DueHeader varclsItem in varclsExcDueList)
			{
				varCounter++;
				if (!clsFunction.IsEqual(varclsFilial.CNPJ, varclsItem.Filial))
				{
					varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsItem.Filial);
				}
				if (varclsFilial == null)
				{
					continue;
				}
				string varLastCertSerial = _clsComexProfile?.CrtSerial;
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
					lbProgress.Text = "Conectando ao Portal Único : Siscomex ...";
					Application.DoEvents();
					clsReturn varclsRetCert = await varclsComexApi.funcConnectAsync(_clsComexProfile);
					varclsReturnFunc.AddRange(varclsRetCert);
					if (varclsRetCert.HasError || varclsRetCert.HasWarning)
					{
						_clsComexProfile = null;
						return;
					}
				}
				lbProgress.Text = $"[ {varCounter} de {varTotal} ] Sincronizando dados da DUE {varclsItem.Num}...";
				Application.DoEvents();
				clsReturn varclsRetItem = await varclsComexApi.funcGetDataByDueKeyLinkAsync(varclsFilial, varclsItem.Num, pByTaskAction: false, pGetDossie: false);
				if (varclsRetItem.HasError || varclsRetItem.UserCancel)
				{
					varclsReturnFunc.AddRange(varclsRetItem);
				}
				if (varclsRetItem.UserCancel)
				{
					break;
				}
			}
			lbProgress.Text = "Recarregando dados da tela ...";
			Application.DoEvents();
			clsReturn varclsRetLoad = await funcLoadDataAsync(_clsDataFilter);
			if (varclsRetLoad.HasError)
			{
				varclsReturnFunc.AddRange(varclsRetLoad);
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			tsbSync.Enabled = true;
			tsbSync.Text = "Siscomex";
			lbProgress.Text = string.Empty;
			plnMessage.Visible = false;
			Application.DoEvents();
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

	private async void tsbDocs_Click(object sender, EventArgs e)
	{
		string varFeatExtId = clsFeatureService.consReportExpDueControl;
		await _clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
		if (clsFunction.Contains(await _clsFeatService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
			return;
		}
		List<DueHeader> varclsDbaDueList = await funcGetDueListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		if (varclsDbaDueList.Count <= 0)
		{
			return;
		}
		List<DueHeader> varclsExcDueList = new List<DueHeader>();
		foreach (DueHeader varclsDbaDueItem in varclsDbaDueList)
		{
			if (!varclsExcDueList.Any((DueHeader r) => clsFunction.IsEqual(r.Num, varclsDbaDueItem.Num)))
			{
				varclsExcDueList.Add(varclsDbaDueItem);
			}
		}
		using frmDueExport varFrmDueExport = new frmDueExport(varclsExcDueList);
		varFrmDueExport.ShowDialog(this);
	}

	private async void tsbSpedCreate_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		string varSpedFilePath = string.Empty;
		try
		{
			tsbSpedCreate.Enabled = false;
			OpenFileDialog varFileDialog = new OpenFileDialog();
			OpenFileDialog openFileDialog = varFileDialog;
			openFileDialog.InitialDirectory = await clsScreenGeral.funcGetDefaultFolderAsync(this);
			varFileDialog.Multiselect = false;
			varFileDialog.Filter = "Arquivo SPED (*.*)|*.*";
			varFileDialog.Title = "Informe o arquivo do SPED ICMS/IPI";
			DialogResult varResult = varFileDialog.ShowDialog();
			await clsScreenGeral.funcSetDefaultFolderAsync(this, varFileDialog.FileName);
			if (varResult != DialogResult.OK || varFileDialog.FileNames.Length == 0)
			{
				return;
			}
			lbProgress.Text = "Aguarde, carregando arquivo selecionado ...";
			plnMessage.Visible = true;
			Application.DoEvents();
			clsSpedComex varclsHandler = new clsSpedComex();
			varclsHandler.EventDocFileStatusSrv += funcEventDocFileSrvStatus;
			varclsReturnFunc = await varclsHandler.funcGetDataAsync(varFileDialog.FileName);
			clsBlockLine0000 varObjSource = varclsReturnFunc.GetObject<clsBlockLine0000>("Object");
			if (!varclsReturnFunc.HasMessage() && varObjSource != null && new frmSpedComex(varFileDialog.FileName, varObjSource).ShowDialog(this).Equals(DialogResult.OK))
			{
				lbProgress.Text = string.Empty;
				plnMessage.Visible = false;
				Application.DoEvents();
				varclsReturnFunc = await varclsHandler.funcSetDataAsync(varFileDialog.FileName, varObjSource);
				varSpedFilePath = varclsReturnFunc.GetValue("DocFilePath");
				if (!varclsReturnFunc.HasMessage() && varSpedFilePath != null)
				{
					varclsReturnFunc.ActionDone = true;
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			tsbSpedCreate.Enabled = true;
			lbProgress.Text = string.Empty;
			plnMessage.Visible = false;
			Application.DoEvents();
			if (varclsReturnFunc.ActionDone)
			{
				string varUserMessage = "Dados de Exportação gerados com sucesso!! " + Environment.NewLine + Environment.NewLine;
				varUserMessage = varUserMessage + "Arquivo gerado : " + varSpedFilePath + Environment.NewLine;
				MessageBox.Show(this, varUserMessage, "Operação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (varclsReturnFunc.HasError || varclsReturnFunc.HasWarning)
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
			}
		}
	}

	private void funcEventDocFileSrvStatus(object sender, EventDocFileSrvEventArgs e)
	{
		if (string.IsNullOrEmpty(e.RetMessage))
		{
			lbProgress.Enabled = false;
			lbProgress.Text = string.Empty;
		}
		else
		{
			lbProgress.Enabled = true;
			lbProgress.Text = e.RetMessage;
		}
		plnMessage.Visible = true;
		Application.DoEvents();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocNFeAverConf));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.tspTaskMenu = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSync = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDocs = new System.Windows.Forms.ToolStripButton();
		this.tssSepDocs = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSpedCreate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvInHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvInTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvInTag = new BrightIdeasSoftware.OLVColumn();
		this.olvInNum = new BrightIdeasSoftware.OLVColumn();
		this.olvnItem = new BrightIdeasSoftware.OLVColumn();
		this.olvnDue = new BrightIdeasSoftware.OLVColumn();
		this.olvnItemDue = new BrightIdeasSoftware.OLVColumn();
		this.olvqItem = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmb = new BrightIdeasSoftware.OLVColumn();
		this.olvHrEmb = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvHrAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvMotAlt = new BrightIdeasSoftware.OLVColumn();
		this.olvInSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvInDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvInValor = new BrightIdeasSoftware.OLVColumn();
		this.olvInCancel = new BrightIdeasSoftware.OLVColumn();
		this.olvInCNPJ = new BrightIdeasSoftware.OLVColumn();
		this.olvInNome = new BrightIdeasSoftware.OLVColumn();
		this.olvInChave = new BrightIdeasSoftware.OLVColumn();
		this.olvInFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvInCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvInNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvInManifest = new BrightIdeasSoftware.OLVColumn();
		this.olvInError = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvEstado = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaIE = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.pnMarket = new System.Windows.Forms.Panel();
		this.btClose = new System.Windows.Forms.Button();
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
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		this.tspTaskMenu.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 443);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(898, 38);
		this.plnMessage.TabIndex = 108;
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
		this.tspTaskMenu.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspTaskMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator3, this.tsbSync, this.toolStripSeparator2, this.tsbDocs, this.tssSepDocs, this.tsbSpedCreate, this.toolStripSeparator7 });
		this.tspTaskMenu.Location = new System.Drawing.Point(0, 0);
		this.tspTaskMenu.Name = "tspTaskMenu";
		this.tspTaskMenu.Padding = new System.Windows.Forms.Padding(2);
		this.tspTaskMenu.Size = new System.Drawing.Size(900, 27);
		this.tspTaskMenu.TabIndex = 43;
		this.tspTaskMenu.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
		this.tsbSync.Image = Monitor.Resources.image_cloud;
		this.tsbSync.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSync.Name = "tsbSync";
		this.tsbSync.Size = new System.Drawing.Size(74, 20);
		this.tsbSync.Text = " Siscomex";
		this.tsbSync.ToolTipText = "Sincronizar dados com Siscomex";
		this.tsbSync.Click += new System.EventHandler(tsbSync_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
		this.tsbDocs.Image = Monitor.Resources.image_copy;
		this.tsbDocs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDocs.Name = "tsbDocs";
		this.tsbDocs.Size = new System.Drawing.Size(63, 20);
		this.tsbDocs.Text = "Anexos";
		this.tsbDocs.ToolTipText = "Documentação Siscomex : DUE, LPCO, Dôssie, Manifesto de Carga, etc";
		this.tsbDocs.Click += new System.EventHandler(tsbDocs_Click);
		this.tssSepDocs.Name = "tssSepDocs";
		this.tssSepDocs.Size = new System.Drawing.Size(6, 23);
		this.tsbSpedCreate.Image = Monitor.Resources.image_add_object;
		this.tsbSpedCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSpedCreate.Name = "tsbSpedCreate";
		this.tsbSpedCreate.Size = new System.Drawing.Size(260, 20);
		this.tsbSpedCreate.Text = "SPED ICMS/IPI : Anexar exportações averbadas";
		this.tsbSpedCreate.ToolTipText = "Gerar Bloco 1 : Dados de Exportação : 1100, 1105 e 1110";
		this.tsbSpedCreate.Click += new System.EventHandler(tsbSpedCreate_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 23);
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvInHasXml);
		this.lsvData.AllColumns.Add(this.olvInTipo);
		this.lsvData.AllColumns.Add(this.olvInTag);
		this.lsvData.AllColumns.Add(this.olvInNum);
		this.lsvData.AllColumns.Add(this.olvnItem);
		this.lsvData.AllColumns.Add(this.olvnDue);
		this.lsvData.AllColumns.Add(this.olvnItemDue);
		this.lsvData.AllColumns.Add(this.olvqItem);
		this.lsvData.AllColumns.Add(this.olvDtEmb);
		this.lsvData.AllColumns.Add(this.olvHrEmb);
		this.lsvData.AllColumns.Add(this.olvDtAverb);
		this.lsvData.AllColumns.Add(this.olvHrAverb);
		this.lsvData.AllColumns.Add(this.olvMotAlt);
		this.lsvData.AllColumns.Add(this.olvInSerie);
		this.lsvData.AllColumns.Add(this.olvInDtAut);
		this.lsvData.AllColumns.Add(this.olvInValor);
		this.lsvData.AllColumns.Add(this.olvInCancel);
		this.lsvData.AllColumns.Add(this.olvInCNPJ);
		this.lsvData.AllColumns.Add(this.olvInNome);
		this.lsvData.AllColumns.Add(this.olvInChave);
		this.lsvData.AllColumns.Add(this.olvInFilial);
		this.lsvData.AllColumns.Add(this.olvInCFOP);
		this.lsvData.AllColumns.Add(this.olvInNCM);
		this.lsvData.AllColumns.Add(this.olvInManifest);
		this.lsvData.AllColumns.Add(this.olvInError);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvEstado);
		this.lsvData.AllColumns.Add(this.olvTpDoc);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvTomaIE);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[33]
		{
			this.olvSelect, this.olvInHasXml, this.olvInTipo, this.olvInTag, this.olvInNum, this.olvnItem, this.olvnDue, this.olvnItemDue, this.olvqItem, this.olvDtEmb,
			this.olvHrEmb, this.olvDtAverb, this.olvHrAverb, this.olvMotAlt, this.olvInSerie, this.olvInDtAut, this.olvInValor, this.olvInCancel, this.olvInCNPJ, this.olvInNome,
			this.olvInChave, this.olvInFilial, this.olvInCFOP, this.olvInNCM, this.olvInManifest, this.olvInError, this.olvDocNote, this.olvAnoMes, this.olvStatus, this.olvEstado,
			this.olvTpDoc, this.olvNatOper, this.olvTomaIE
		});
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.EmptyListMsg = "";
		this.lsvData.EmptyListMsgFont = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvData.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvData.FullRowSelect = true;
		this.lsvData.HideSelection = false;
		this.lsvData.IncludeColumnHeadersInCopy = true;
		this.lsvData.Location = new System.Drawing.Point(0, 29);
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
		this.lsvData.Size = new System.Drawing.Size(900, 98);
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
		this.olvSelect.HeaderCheckBoxUpdatesRowCheckBoxes = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvInHasXml.AspectName = "HasXml";
		this.olvInHasXml.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInHasXml.IsEditable = false;
		this.olvInHasXml.Text = "XML";
		this.olvInHasXml.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInHasXml.ToolTipText = "Tem XML vinculado";
		this.olvInHasXml.Width = 40;
		this.olvInTipo.AspectName = "Model";
		this.olvInTipo.Text = "Tipo";
		this.olvInTipo.ToolTipText = "Modelo do Documento Fiscal";
		this.olvInTipo.Width = 43;
		this.olvInTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInTag.Text = "Etiq";
		this.olvInTag.ToolTipText = "Etiqueta atribuída";
		this.olvInNum.AspectName = "Num";
		this.olvInNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInNum.Hyperlink = true;
		this.olvInNum.Text = "Num";
		this.olvInNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInNum.ToolTipText = "Número do documento fiscal";
		this.olvInNum.Width = 79;
		this.olvnItem.AspectName = "nItem";
		this.olvnItem.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvnItem.IsEditable = false;
		this.olvnItem.Text = "Item";
		this.olvnItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvnItem.ToolTipText = "Item";
		this.olvnItem.Width = 50;
		this.olvnDue.AspectName = "nDue";
		this.olvnDue.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvnDue.IsEditable = false;
		this.olvnDue.Text = "Due";
		this.olvnDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvnDue.ToolTipText = "Due";
		this.olvnDue.Width = 110;
		this.olvnItemDue.AspectName = "nItemDue";
		this.olvnItemDue.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvnItemDue.IsEditable = false;
		this.olvnItemDue.Text = "ItemDue";
		this.olvnItemDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvnItemDue.ToolTipText = "Item Due";
		this.olvnItemDue.Width = 50;
		this.olvqItem.AspectName = "qItem";
		this.olvqItem.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvqItem.IsEditable = false;
		this.olvqItem.Text = "Quant.";
		this.olvqItem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvqItem.ToolTipText = "Quantidade Item";
		this.olvqItem.Width = 90;
		this.olvDtEmb.AspectName = "dtEmbarque";
		this.olvDtEmb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmb.IsEditable = false;
		this.olvDtEmb.Text = "Dt.Emb.";
		this.olvDtEmb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmb.ToolTipText = "Data Embarque";
		this.olvDtEmb.Width = 100;
		this.olvHrEmb.AspectName = "hrEmbarque";
		this.olvHrEmb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHrEmb.IsEditable = false;
		this.olvHrEmb.Text = "Hr.Emb.";
		this.olvHrEmb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHrEmb.ToolTipText = "Hora Embarque";
		this.olvHrEmb.Width = 80;
		this.olvDtAverb.AspectName = "dtAverbacao";
		this.olvDtAverb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAverb.IsEditable = false;
		this.olvDtAverb.Text = "Dt.Averb.";
		this.olvDtAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAverb.ToolTipText = "Data Averbação";
		this.olvDtAverb.Width = 100;
		this.olvHrAverb.AspectName = "hrAverbacao";
		this.olvHrAverb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHrAverb.IsEditable = false;
		this.olvHrAverb.Text = "Hr.Averb.";
		this.olvHrAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHrAverb.ToolTipText = "Hora Averbação";
		this.olvHrAverb.Width = 80;
		this.olvMotAlt.AspectName = "motAlteracao";
		this.olvMotAlt.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMotAlt.IsEditable = false;
		this.olvMotAlt.Text = "Motivo da Alteração";
		this.olvMotAlt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvMotAlt.ToolTipText = "Motivo da Alteração";
		this.olvMotAlt.Width = 120;
		this.olvInSerie.AspectName = "Serie";
		this.olvInSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInSerie.IsEditable = false;
		this.olvInSerie.Text = "Serie";
		this.olvInSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInSerie.ToolTipText = "Serie";
		this.olvInSerie.Width = 50;
		this.olvInDtAut.AspectName = "DtAut";
		this.olvInDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInDtAut.IsEditable = false;
		this.olvInDtAut.Text = "Data";
		this.olvInDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInDtAut.ToolTipText = "Data Autorização";
		this.olvInDtAut.Width = 100;
		this.olvInValor.AspectName = "Valor";
		this.olvInValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInValor.IsEditable = false;
		this.olvInValor.Text = "Valor";
		this.olvInValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvInValor.ToolTipText = "Valor";
		this.olvInValor.Width = 100;
		this.olvInCancel.AspectName = "Canceled";
		this.olvInCancel.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInCancel.IsEditable = false;
		this.olvInCancel.Text = "Canc";
		this.olvInCancel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInCancel.ToolTipText = "Cancelado";
		this.olvInCancel.Width = 35;
		this.olvInCNPJ.AspectName = "EmitID";
		this.olvInCNPJ.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInCNPJ.IsEditable = false;
		this.olvInCNPJ.Text = "CNPJ/CPF";
		this.olvInCNPJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInCNPJ.ToolTipText = "CNPJ/CPF";
		this.olvInCNPJ.Width = 130;
		this.olvInNome.AspectName = "EmitNome";
		this.olvInNome.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInNome.IsEditable = false;
		this.olvInNome.Text = "Nome";
		this.olvInNome.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInNome.ToolTipText = "Nome";
		this.olvInNome.Width = 230;
		this.olvInChave.AspectName = "Chave";
		this.olvInChave.Text = "Chave";
		this.olvInChave.ToolTipText = "Chave do documento fiscal";
		this.olvInFilial.AspectName = "Filial";
		this.olvInFilial.Text = "Filial";
		this.olvInFilial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvInFilial.ToolTipText = "Filial";
		this.olvInFilial.Width = 130;
		this.olvInCFOP.AspectName = "CFOPList";
		this.olvInCFOP.Text = "CFOP";
		this.olvInCFOP.ToolTipText = "Código Fiscal de Operações e Prestações";
		this.olvInCFOP.Width = 90;
		this.olvInNCM.AspectName = "NCMList";
		this.olvInNCM.Text = "NCM";
		this.olvInNCM.ToolTipText = "Nomenclatura Comum do Mercosul";
		this.olvInNCM.Width = 90;
		this.olvInManifest.Text = "Manifestação";
		this.olvInManifest.ToolTipText = "Manifestação";
		this.olvInManifest.Width = 90;
		this.olvInError.AspectName = "XmlError";
		this.olvInError.Text = "Validação";
		this.olvInError.ToolTipText = "Validação";
		this.olvInError.Width = 90;
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvDocNote.ToolTipText = "Comentarios";
		this.olvDocNote.Width = 90;
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Ano-Mes do documento fiscal";
		this.olvStatus.Groupable = false;
		this.olvStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStatus.Searchable = false;
		this.olvStatus.Text = "Status";
		this.olvStatus.ToolTipText = "Status do documento";
		this.olvStatus.UseFiltering = false;
		this.olvStatus.Width = 77;
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do Documento Fiscal";
		this.olvEstado.Width = 30;
		this.olvTpDoc.AspectName = "TpDoc";
		this.olvTpDoc.Text = "TipoDoc";
		this.olvTpDoc.ToolTipText = "Tipo de documento";
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "Natureza";
		this.olvNatOper.ToolTipText = "Descrição da Natureza da Operação";
		this.olvTomaIE.AspectName = "TomaIE";
		this.olvTomaIE.Text = "IE Tomador";
		this.olvTomaIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomaIE.ToolTipText = "IE do tomador";
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
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Location = new System.Drawing.Point(0, 128);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(900, 353);
		this.pnMarket.TabIndex = 11;
		this.pnMarket.Visible = false;
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(869, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 205;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lknClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lknClose.AutoSize = true;
		this.lknClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknClose.Location = new System.Drawing.Point(718, 330);
		this.lknClose.Name = "lknClose";
		this.lknClose.Size = new System.Drawing.Size(176, 16);
		this.lknClose.TabIndex = 204;
		this.lknClose.TabStop = true;
		this.lknClose.Text = "Não mostrar mais esse alerta";
		this.lknClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lknClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknClose_LinkClicked);
		this.pnMarketContent.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pnMarketContent.BackColor = System.Drawing.Color.WhiteSmoke;
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
		this.pnMarketContent.Location = new System.Drawing.Point(170, 50);
		this.pnMarketContent.Name = "pnMarketContent";
		this.pnMarketContent.Size = new System.Drawing.Size(566, 261);
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
		this.lknAction.Text = "Averbação de Embarque";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(17, 134);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 37);
		this.lbText02.TabIndex = 196;
		this.lbText02.Text = "Neste relatório é possivel visualizar todos os dados relevantes do embarque dos itens averbados no evento.";
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 81);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 35);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = "Apresenta todos os documentos da base de dados do sistema que possuem o evento de averbação de embarque, conhecido também como averbação de exportação.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(137, 6);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(308, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Relatório Averbação de embarque Contratação de funcionalidade sob demanda. ";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(17, 55);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Definição do relatório";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(900, 481);
		base.Controls.Add(this.pnMarket);
		base.Controls.Add(this.plnMessage);
		base.Controls.Add(this.lsvData);
		base.Controls.Add(this.tspTaskMenu);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocNFeAverConf";
		this.Text = "Averbações de Exportação";
		base.Load += new System.EventHandler(frmTabDocNFeAverConf_Load);
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		this.tspTaskMenu.ResumeLayout(false);
		this.tspTaskMenu.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
