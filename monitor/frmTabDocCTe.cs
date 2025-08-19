using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabDocCTe : Form
{
	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocCTe _SqlTabData = new clsSrvTabDocCTe();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private string _InfoAddXml = string.Empty;

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private bool _HasFiscalioConnect;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private FastObjectListView lsvData;

	private OLVColumn olvSelect;

	private OLVColumn olvHasXml;

	private OLVColumn olvTipo;

	private OLVColumn olvTag;

	private OLVColumn olvNum;

	private OLVColumn olvSerie;

	private OLVColumn olvDtAut;

	private OLVColumn olvValor;

	private OLVColumn olvVenc;

	private OLVColumn olvCancel;

	private OLVColumn olvHasEvent;

	private OLVColumn olvEmitCNPJ;

	private OLVColumn olvEmitNome;

	private OLVColumn olvMotivo;

	private OLVColumn olvEstado;

	private OLVColumn olvChave;

	private OLVColumn olvFilial;

	private OLVColumn olvTpDoc;

	private OLVColumn olvTpServ;

	private OLVColumn olvModal;

	private OLVColumn olvNatOper;

	private OLVColumn olvProPred;

	private OLVColumn olvBaseIcms;

	private OLVColumn olvValorIcms;

	private OLVColumn olvTaxaIcms;

	private OLVColumn olvCdCstICMS;

	private OLVColumn olvDescCstICMS;

	private OLVColumn olvIndSimplesNac;

	private OLVColumn olvCFOP;

	private OLVColumn olvMunIni;

	private OLVColumn olvxMunIni;

	private OLVColumn olvUFIni;

	private OLVColumn olvMunFim;

	private OLVColumn olvxMunFim;

	private OLVColumn olvUFFim;

	private OLVColumn olvcMunEnv;

	private OLVColumn olvxMunEnv;

	private OLVColumn olvUFEnv;

	private OLVColumn olvDtEmi;

	private OLVColumn olvHrEmi;

	private OLVColumn olvEmitIE;

	private OLVColumn olvEmitcMun;

	private OLVColumn olvEmitxMun;

	private OLVColumn olvEmitUf;

	private OLVColumn olvTomaId;

	private OLVColumn olvTomaIE;

	private OLVColumn olvTomaNome;

	private OLVColumn olvTomacMun;

	private OLVColumn olvTomaxMun;

	private OLVColumn olvTomaUf;

	private OLVColumn olvExpediId;

	private OLVColumn olvExpediNome;

	private OLVColumn olvExpediIE;

	private OLVColumn olvExpedicMun;

	private OLVColumn olvExpedixMun;

	private OLVColumn olvExpediUf;

	private OLVColumn olvRemeteId;

	private OLVColumn olvRemeteNome;

	private OLVColumn olvRemeteIE;

	private OLVColumn olvRemetecMun;

	private OLVColumn olvRemetexMun;

	private OLVColumn olvRemeteUf;

	private OLVColumn olvDestinId;

	private OLVColumn olvDestinIE;

	private OLVColumn olvDestinNome;

	private OLVColumn olvDestincMun;

	private OLVColumn olvDestinxMun;

	private OLVColumn olvDestinUf;

	private OLVColumn olvRecebeId;

	private OLVColumn olvRecebeNome;

	private OLVColumn olvRecebeIE;

	private OLVColumn olvRecebecMun;

	private OLVColumn olvRecebexMun;

	private OLVColumn olvRecebeUf;

	private OLVColumn olvInfAdFisco;

	private OLVColumn olvAnoMes;

	private OLVColumn olvError;

	private OLVColumn olvStatus;

	private OLVColumn olvManifest;

	private OLVColumn olvHasCTeEvent;

	private OLVColumn olvMDFeEvtData;

	private ImageList ImageListDocs;

	private OLVColumn olvBatchNum;

	private OLVColumn olvDtEntFisc;

	private OLVColumn olvDtRegFisc;

	private OLVColumn olvUsRegFisc;

	private OLVColumn olvDcNumFisc;

	private OLVColumn olvFretePesoTransp;

	private OLVColumn olvFreteUnidadePeso;

	private OLVColumn olvFreteValor;

	private OLVColumn olvFreteVlrPesoVol;

	private OLVColumn olvFreteVlrPedagio;

	private OLVColumn olvFreteVlrDespacho;

	private OLVColumn olvFreteVlrSecCat;

	private OLVColumn olvFreteVlrItr;

	private OLVColumn olvFreteVlrAdeme;

	private OLVColumn olvFreteVlrOutras;

	private OLVColumn olvEmitida;

	private OLVColumn olvDocNote;

	private OLVColumn olvValorCarga;

	private OLVColumn olvBaseIcmsOutra;

	private OLVColumn olvValorIcmsOutra;

	private OLVColumn olvTaxaICMSOutraUF;

	private OLVColumn olvInfCpl;

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

	private OLVColumn olvTagDtHr;

	private OLVColumn olvTagUser;

	private OLVColumn olvDocNoteDtHr;

	private OLVColumn olvDocNoteUser;

	private OLVColumn olvVersion;

	private Button btSalesContact;

	private OLVColumn olvFreteVolTransp;

	private OLVColumn olvObsCont;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabDocCTe(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		olvAnoMes.Name = "AnoMes";
		lsvData.HandleDestroyed += lsvData_HandleDestroyed;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private void lsvData_HandleDestroyed(object o, EventArgs e)
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

	private async void frmTabDocCTe_Load(object sender, EventArgs e)
	{
		_InfoAddXml = (await new clsDataConfig().funcGetItemByKeyAsync()).InfoAddXmlNFe;
		_StateList = await new clsDataEstado().funcGetListAsync(pWthAN: false);
		_HasFiscalioConnect = await clsScreenGeral.funcMustShowRegFieldsAsync();
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		funcLoadTagsAsync();
		_ColumnsLoaded = false;
		clsScreenGeral.funcSetColumnsObjectListView(this, lsvData);
		_ColumnsLoaded = true;
		foreach (OLVColumn allColumn in lsvData.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
		if (clsFunction.IsAdmin)
		{
			clsScreenGeral.funcCheckListViewDdic(typeof(Document), lsvData);
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
			Document document = (Document)x;
			return (document == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(document, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
		};
		olvHasXml.AspectGetter = (object x) => string.Empty;
		olvEmitida.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(document.Emitida) ? "Terceiros" : "Empresa";
		};
		olvNum.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsFunction.funcGetValue(document.Num).PadLeft(9, '0');
		};
		olvTipo.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsFunction.funcGetDocType(document.Model);
		};
		olvTag.ImageGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : ((object)clsScreenGeral.funcGetDocNotesIcon(document));
		};
		olvTag.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(document.Tag) ? null : _TagHsList[document.Tag];
		};
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			Document document = (Document)x;
			return (document == null) ? null : clsScreenGeral.funcGetDocStatIcon(document);
		};
		olvManifest.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.GetEventDesc(document.Model, document.ManifCode);
		};
		olvEmitCNPJ.AspectGetter = (object x) => ((Document)x)?.EmitID;
		olvEmitNome.AspectGetter = (object x) => ((Document)x)?.EmitNome;
		olvEstado.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
		};
		olvTpDoc.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetTipoDoc(document.Model, document.TpDoc);
		};
		olvTpServ.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetTpServ(document.Model, document.tpServ);
		};
		olvModal.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetModal(document.Model, document.Modal);
		};
		olvIndSimplesNac.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetIndSimples(document.Model, document.IndSimplesNac);
		};
		olvTomaIE.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsSrvGeral.funcGetIEFormat(document.TomaIE);
		};
		olvDescCstICMS.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetCteIcmsStDesc(document);
		};
		olvInfAdFisco.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXml) ? null : clsScreenGeral.funcGetDocText(document.Chave)?.infAdFisco;
		};
		olvInfCpl.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXml) ? null : clsScreenGeral.funcGetDocText(document.Chave)?.infCpl;
		};
		olvObsCont.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXml) ? null : clsScreenGeral.funcGetDocText(document.Chave)?.XObsCont;
		};
		olvAnoMes.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = document.DtAut.Substring(0, 7);
			}
			catch
			{
			}
			return result;
		};
		olvError.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : varclsValidator.funcGetDesc(document);
		};
	}

	private void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		Document varDocument = (Document)e.Model;
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
		List<Document> varclsDocList = await new clsDataDoc().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
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
		else if (pclsDataFilter.GroupByCFOP)
		{
			varOlvColum = olvCFOP;
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

	private async void tsbDocReset_Click(object sender, EventArgs e)
	{
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
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
		List<Document> varDocList = await funcGetDocListAsync(pFocused, pChecked, pSyncFromDbaFirst: true);
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
		return true;
	}

	private async void tsbRemoveDocNote_Click(object sender, EventArgs e)
	{
		funcRemoveDocNoteAsync(pFocused: true, pChecked: true);
	}

	public async Task<bool> funcRemoveDocNoteAsync(bool pFocused, bool pChecked)
	{
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
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
		return true;
	}

	private async void tsbDocNote_Click(object sender, EventArgs e)
	{
		await funcSetDocNoteAsync(pFocused: true, pChecked: true);
	}

	public async Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked)
	{
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
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

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportDocToExcelAsync<Document>(lsvData, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = funcGetObjList(pFocused, pChecked);
		if (pSyncFromDbaFirst)
		{
			varDocList = await _clsDataDoc.funcGetSyncListByListAsync(varDocList);
		}
		return varDocList;
	}

	private List<Document> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<Document> varDocList = new List<Document>();
		Document varFocused = new Document();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (Document)lsvData.GetItem(lsvData.FocusedItem.Index).RowObject;
				varDocList.Add(varFocused);
			}
			catch
			{
				varFocused = new Document();
			}
		}
		if (pChecked)
		{
			foreach (Document varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varDocList.Add(varObject);
				}
			}
			if (varDocList.Count > 0)
			{
				varDocList = lsvData.Objects.Cast<Document>().Intersect(varDocList).ToList();
			}
		}
		if (!pFocused && !pChecked)
		{
			varDocList = lsvData.FilteredObjects.Cast<Document>().ToList();
			if (varDocList.Count <= 0)
			{
				varDocList = lsvData.Objects.Cast<Document>().ToList();
			}
		}
		return varDocList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		await funcGetDocListAsync(pFocused, pChecked, pSyncFromDbaFirst: true);
		lsvData.RefreshSelectedObjects();
		return true;
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

	private void lsvData_DoubleClick(object sender, EventArgs e)
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

	private void lsvData_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
	{
		if (e.ColumnIndex == olvNum.Index)
		{
			Document varDocument = (Document)e.Model;
			if (varDocument != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varDocument);
			}
		}
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

	private async void tsmTransferFilial_Click(object sender, EventArgs e)
	{
		await funcTransferFilialAsync();
	}

	private async void tsmCopyDocKey_Click(object sender, EventArgs e)
	{
		await funcCopyDocKeyAsync();
	}

	private async Task<bool> funcTransferFilialAsync()
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		varArguments.TaskProgress = "Transferindo documento(s) para o destino....";
		OnEventTabManager(varArguments);
		foreach (clsValue varclsValue in (await new clsManGeral().funcTransferFilialAsync(varDocList)).Values)
		{
			if (varclsValue.Name.Equals("Document"))
			{
				lsvData.RemoveObject(varclsValue.ObjDoc);
			}
		}
		varArguments.TaskProgress = string.Empty;
		OnEventTabManager(varArguments);
		return true;
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
		clsHelpService.funcCallTipReportCTeSyntheticAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocCTe));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitida = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvHrEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvValor = new BrightIdeasSoftware.OLVColumn();
		this.olvVenc = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEntFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDtRegFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvUsRegFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDcNumFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitCNPJ = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitNome = new BrightIdeasSoftware.OLVColumn();
		this.olvManifest = new BrightIdeasSoftware.OLVColumn();
		this.olvCancel = new BrightIdeasSoftware.OLVColumn();
		this.olvHasEvent = new BrightIdeasSoftware.OLVColumn();
		this.olvHasCTeEvent = new BrightIdeasSoftware.OLVColumn();
		this.olvMDFeEvtData = new BrightIdeasSoftware.OLVColumn();
		this.olvMotivo = new BrightIdeasSoftware.OLVColumn();
		this.olvEstado = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvTpServ = new BrightIdeasSoftware.OLVColumn();
		this.olvModal = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvProPred = new BrightIdeasSoftware.OLVColumn();
		this.olvBaseIcms = new BrightIdeasSoftware.OLVColumn();
		this.olvTaxaIcms = new BrightIdeasSoftware.OLVColumn();
		this.olvValorIcms = new BrightIdeasSoftware.OLVColumn();
		this.olvCdCstICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvDescCstICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvIndSimplesNac = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvMunIni = new BrightIdeasSoftware.OLVColumn();
		this.olvxMunIni = new BrightIdeasSoftware.OLVColumn();
		this.olvUFIni = new BrightIdeasSoftware.OLVColumn();
		this.olvMunFim = new BrightIdeasSoftware.OLVColumn();
		this.olvxMunFim = new BrightIdeasSoftware.OLVColumn();
		this.olvUFFim = new BrightIdeasSoftware.OLVColumn();
		this.olvcMunEnv = new BrightIdeasSoftware.OLVColumn();
		this.olvxMunEnv = new BrightIdeasSoftware.OLVColumn();
		this.olvUFEnv = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitIE = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitcMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitUf = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaId = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaIE = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaNome = new BrightIdeasSoftware.OLVColumn();
		this.olvTomacMun = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaUf = new BrightIdeasSoftware.OLVColumn();
		this.olvExpediId = new BrightIdeasSoftware.OLVColumn();
		this.olvExpediIE = new BrightIdeasSoftware.OLVColumn();
		this.olvExpediNome = new BrightIdeasSoftware.OLVColumn();
		this.olvExpedicMun = new BrightIdeasSoftware.OLVColumn();
		this.olvExpedixMun = new BrightIdeasSoftware.OLVColumn();
		this.olvExpediUf = new BrightIdeasSoftware.OLVColumn();
		this.olvRemeteId = new BrightIdeasSoftware.OLVColumn();
		this.olvRemeteIE = new BrightIdeasSoftware.OLVColumn();
		this.olvRemeteNome = new BrightIdeasSoftware.OLVColumn();
		this.olvRemetecMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRemetexMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRemeteUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinId = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDestincMun = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinUf = new BrightIdeasSoftware.OLVColumn();
		this.olvRecebeId = new BrightIdeasSoftware.OLVColumn();
		this.olvRecebeIE = new BrightIdeasSoftware.OLVColumn();
		this.olvRecebeNome = new BrightIdeasSoftware.OLVColumn();
		this.olvRecebecMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRecebexMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRecebeUf = new BrightIdeasSoftware.OLVColumn();
		this.olvInfAdFisco = new BrightIdeasSoftware.OLVColumn();
		this.olvInfCpl = new BrightIdeasSoftware.OLVColumn();
		this.olvObsCont = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvError = new BrightIdeasSoftware.OLVColumn();
		this.olvBatchNum = new BrightIdeasSoftware.OLVColumn();
		this.olvFretePesoTransp = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteUnidadePeso = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteValor = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrPesoVol = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrPedagio = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrDespacho = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrSecCat = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrItr = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrAdeme = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVlrOutras = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVolTransp = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvValorCarga = new BrightIdeasSoftware.OLVColumn();
		this.olvBaseIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvValorIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvTaxaICMSOutraUF = new BrightIdeasSoftware.OLVColumn();
		this.olvTagDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvTagUser = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteUser = new BrightIdeasSoftware.OLVColumn();
		this.olvVersion = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
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
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvHasXml);
		this.lsvData.AllColumns.Add(this.olvEmitida);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvDtAut);
		this.lsvData.AllColumns.Add(this.olvDtEmi);
		this.lsvData.AllColumns.Add(this.olvHrEmi);
		this.lsvData.AllColumns.Add(this.olvValor);
		this.lsvData.AllColumns.Add(this.olvVenc);
		this.lsvData.AllColumns.Add(this.olvDtEntFisc);
		this.lsvData.AllColumns.Add(this.olvDtRegFisc);
		this.lsvData.AllColumns.Add(this.olvUsRegFisc);
		this.lsvData.AllColumns.Add(this.olvDcNumFisc);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvEmitCNPJ);
		this.lsvData.AllColumns.Add(this.olvEmitNome);
		this.lsvData.AllColumns.Add(this.olvManifest);
		this.lsvData.AllColumns.Add(this.olvCancel);
		this.lsvData.AllColumns.Add(this.olvHasEvent);
		this.lsvData.AllColumns.Add(this.olvHasCTeEvent);
		this.lsvData.AllColumns.Add(this.olvMDFeEvtData);
		this.lsvData.AllColumns.Add(this.olvMotivo);
		this.lsvData.AllColumns.Add(this.olvEstado);
		this.lsvData.AllColumns.Add(this.olvChave);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvTpDoc);
		this.lsvData.AllColumns.Add(this.olvTpServ);
		this.lsvData.AllColumns.Add(this.olvModal);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvProPred);
		this.lsvData.AllColumns.Add(this.olvBaseIcms);
		this.lsvData.AllColumns.Add(this.olvTaxaIcms);
		this.lsvData.AllColumns.Add(this.olvValorIcms);
		this.lsvData.AllColumns.Add(this.olvCdCstICMS);
		this.lsvData.AllColumns.Add(this.olvDescCstICMS);
		this.lsvData.AllColumns.Add(this.olvIndSimplesNac);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvMunIni);
		this.lsvData.AllColumns.Add(this.olvxMunIni);
		this.lsvData.AllColumns.Add(this.olvUFIni);
		this.lsvData.AllColumns.Add(this.olvMunFim);
		this.lsvData.AllColumns.Add(this.olvxMunFim);
		this.lsvData.AllColumns.Add(this.olvUFFim);
		this.lsvData.AllColumns.Add(this.olvcMunEnv);
		this.lsvData.AllColumns.Add(this.olvxMunEnv);
		this.lsvData.AllColumns.Add(this.olvUFEnv);
		this.lsvData.AllColumns.Add(this.olvEmitIE);
		this.lsvData.AllColumns.Add(this.olvEmitcMun);
		this.lsvData.AllColumns.Add(this.olvEmitxMun);
		this.lsvData.AllColumns.Add(this.olvEmitUf);
		this.lsvData.AllColumns.Add(this.olvTomaId);
		this.lsvData.AllColumns.Add(this.olvTomaIE);
		this.lsvData.AllColumns.Add(this.olvTomaNome);
		this.lsvData.AllColumns.Add(this.olvTomacMun);
		this.lsvData.AllColumns.Add(this.olvTomaxMun);
		this.lsvData.AllColumns.Add(this.olvTomaUf);
		this.lsvData.AllColumns.Add(this.olvExpediId);
		this.lsvData.AllColumns.Add(this.olvExpediIE);
		this.lsvData.AllColumns.Add(this.olvExpediNome);
		this.lsvData.AllColumns.Add(this.olvExpedicMun);
		this.lsvData.AllColumns.Add(this.olvExpedixMun);
		this.lsvData.AllColumns.Add(this.olvExpediUf);
		this.lsvData.AllColumns.Add(this.olvRemeteId);
		this.lsvData.AllColumns.Add(this.olvRemeteIE);
		this.lsvData.AllColumns.Add(this.olvRemeteNome);
		this.lsvData.AllColumns.Add(this.olvRemetecMun);
		this.lsvData.AllColumns.Add(this.olvRemetexMun);
		this.lsvData.AllColumns.Add(this.olvRemeteUf);
		this.lsvData.AllColumns.Add(this.olvDestinId);
		this.lsvData.AllColumns.Add(this.olvDestinIE);
		this.lsvData.AllColumns.Add(this.olvDestinNome);
		this.lsvData.AllColumns.Add(this.olvDestincMun);
		this.lsvData.AllColumns.Add(this.olvDestinxMun);
		this.lsvData.AllColumns.Add(this.olvDestinUf);
		this.lsvData.AllColumns.Add(this.olvRecebeId);
		this.lsvData.AllColumns.Add(this.olvRecebeIE);
		this.lsvData.AllColumns.Add(this.olvRecebeNome);
		this.lsvData.AllColumns.Add(this.olvRecebecMun);
		this.lsvData.AllColumns.Add(this.olvRecebexMun);
		this.lsvData.AllColumns.Add(this.olvRecebeUf);
		this.lsvData.AllColumns.Add(this.olvInfAdFisco);
		this.lsvData.AllColumns.Add(this.olvInfCpl);
		this.lsvData.AllColumns.Add(this.olvObsCont);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvError);
		this.lsvData.AllColumns.Add(this.olvBatchNum);
		this.lsvData.AllColumns.Add(this.olvFretePesoTransp);
		this.lsvData.AllColumns.Add(this.olvFreteUnidadePeso);
		this.lsvData.AllColumns.Add(this.olvFreteValor);
		this.lsvData.AllColumns.Add(this.olvFreteVlrPesoVol);
		this.lsvData.AllColumns.Add(this.olvFreteVlrPedagio);
		this.lsvData.AllColumns.Add(this.olvFreteVlrDespacho);
		this.lsvData.AllColumns.Add(this.olvFreteVlrSecCat);
		this.lsvData.AllColumns.Add(this.olvFreteVlrItr);
		this.lsvData.AllColumns.Add(this.olvFreteVlrAdeme);
		this.lsvData.AllColumns.Add(this.olvFreteVlrOutras);
		this.lsvData.AllColumns.Add(this.olvFreteVolTransp);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvValorCarga);
		this.lsvData.AllColumns.Add(this.olvBaseIcmsOutra);
		this.lsvData.AllColumns.Add(this.olvValorIcmsOutra);
		this.lsvData.AllColumns.Add(this.olvTaxaICMSOutraUF);
		this.lsvData.AllColumns.Add(this.olvTagDtHr);
		this.lsvData.AllColumns.Add(this.olvTagUser);
		this.lsvData.AllColumns.Add(this.olvDocNoteDtHr);
		this.lsvData.AllColumns.Add(this.olvDocNoteUser);
		this.lsvData.AllColumns.Add(this.olvVersion);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[107]
		{
			this.olvSelect, this.olvHasXml, this.olvEmitida, this.olvTipo, this.olvTag, this.olvNum, this.olvSerie, this.olvDtAut, this.olvDtEmi, this.olvHrEmi,
			this.olvValor, this.olvVenc, this.olvDtEntFisc, this.olvDtRegFisc, this.olvUsRegFisc, this.olvDcNumFisc, this.olvStatus, this.olvEmitCNPJ, this.olvEmitNome, this.olvManifest,
			this.olvCancel, this.olvHasEvent, this.olvHasCTeEvent, this.olvMDFeEvtData, this.olvMotivo, this.olvEstado, this.olvChave, this.olvFilial, this.olvTpDoc, this.olvTpServ,
			this.olvModal, this.olvNatOper, this.olvProPred, this.olvBaseIcms, this.olvTaxaIcms, this.olvValorIcms, this.olvCdCstICMS, this.olvDescCstICMS, this.olvIndSimplesNac, this.olvCFOP,
			this.olvMunIni, this.olvxMunIni, this.olvUFIni, this.olvMunFim, this.olvxMunFim, this.olvUFFim, this.olvcMunEnv, this.olvxMunEnv, this.olvUFEnv, this.olvEmitIE,
			this.olvEmitcMun, this.olvEmitxMun, this.olvEmitUf, this.olvTomaId, this.olvTomaIE, this.olvTomaNome, this.olvTomacMun, this.olvTomaxMun, this.olvTomaUf, this.olvExpediId,
			this.olvExpediIE, this.olvExpediNome, this.olvExpedicMun, this.olvExpedixMun, this.olvExpediUf, this.olvRemeteId, this.olvRemeteIE, this.olvRemeteNome, this.olvRemetecMun, this.olvRemetexMun,
			this.olvRemeteUf, this.olvDestinId, this.olvDestinIE, this.olvDestinNome, this.olvDestincMun, this.olvDestinxMun, this.olvDestinUf, this.olvRecebeId, this.olvRecebeIE, this.olvRecebeNome,
			this.olvRecebecMun, this.olvRecebexMun, this.olvRecebeUf, this.olvInfAdFisco, this.olvInfCpl, this.olvObsCont, this.olvAnoMes, this.olvError, this.olvBatchNum, this.olvFretePesoTransp,
			this.olvFreteUnidadePeso, this.olvFreteValor, this.olvFreteVlrPesoVol, this.olvFreteVlrPedagio, this.olvFreteVlrDespacho, this.olvFreteVlrSecCat, this.olvFreteVlrItr, this.olvFreteVlrAdeme, this.olvFreteVlrOutras, this.olvFreteVolTransp,
			this.olvDocNote, this.olvValorCarga, this.olvTagDtHr, this.olvTagUser, this.olvDocNoteDtHr, this.olvDocNoteUser, this.olvVersion
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
		this.olvSelect.HeaderCheckBoxUpdatesRowCheckBoxes = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvHasXml.AspectName = "HasXml";
		this.olvHasXml.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.IsEditable = false;
		this.olvHasXml.Text = "XML";
		this.olvHasXml.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.ToolTipText = "Tem XML vinculado";
		this.olvHasXml.Width = 36;
		this.olvEmitida.AspectName = "Emitida";
		this.olvEmitida.Text = "Emissor";
		this.olvEmitida.ToolTipText = "Emissor";
		this.olvTipo.AspectName = "Model";
		this.olvTipo.Text = "Tipo";
		this.olvTipo.ToolTipText = "Modelo do documento fiscal ";
		this.olvTipo.Width = 43;
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.ToolTipText = "Etiqueta atribuída";
		this.olvTag.Width = 29;
		this.olvNum.AspectName = "Num";
		this.olvNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.Hyperlink = true;
		this.olvNum.Text = "Num";
		this.olvNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.ToolTipText = "Número do documento fiscal";
		this.olvNum.Width = 74;
		this.olvSerie.AspectName = "Serie";
		this.olvSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.Text = "Serie";
		this.olvSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.ToolTipText = "Série do documento fiscal ";
		this.olvSerie.Width = 44;
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
		this.olvHrEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHrEmi.ToolTipText = "Hora de emissão";
		this.olvHrEmi.Width = 71;
		this.olvValor.AspectName = "Valor";
		this.olvValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValor.Text = "Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.ToolTipText = "Valor atribuído";
		this.olvValor.Width = 80;
		this.olvVenc.AspectName = "DVenc";
		this.olvVenc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVenc.Text = "Venc.";
		this.olvVenc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVenc.ToolTipText = "Data de vencimento";
		this.olvVenc.Width = 80;
		this.olvDtEntFisc.AspectName = "DtEntFisc";
		this.olvDtEntFisc.Text = "DtLancFisc";
		this.olvDtEntFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEntFisc.ToolTipText = "Data de contabilização no sistema de gestão fiscal ";
		this.olvDtEntFisc.Width = 71;
		this.olvDtRegFisc.AspectName = "DtRegFisc";
		this.olvDtRegFisc.Text = "DtDigFisc";
		this.olvDtRegFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtRegFisc.ToolTipText = "Data de digitação no sistema de gestão fiscal ";
		this.olvDtRegFisc.Width = 71;
		this.olvUsRegFisc.AspectName = "UsRegFisc";
		this.olvUsRegFisc.Text = "UsrDigFisc";
		this.olvUsRegFisc.ToolTipText = "Usuário de digitação no sistema de gestão fiscal ";
		this.olvUsRegFisc.Width = 71;
		this.olvDcNumFisc.AspectName = "DcNumFisc";
		this.olvDcNumFisc.Text = "DocFiscal";
		this.olvDcNumFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDcNumFisc.ToolTipText = "Documento no sistema de gestão fiscal ";
		this.olvDcNumFisc.Width = 71;
		this.olvStatus.Groupable = false;
		this.olvStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStatus.Searchable = false;
		this.olvStatus.Text = "Status";
		this.olvStatus.ToolTipText = "Status do documento fiscal";
		this.olvStatus.UseFiltering = false;
		this.olvStatus.Width = 77;
		this.olvEmitCNPJ.AspectName = "EmitID";
		this.olvEmitCNPJ.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitCNPJ.Text = "Emissor CNPJ/CPF";
		this.olvEmitCNPJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitCNPJ.ToolTipText = "CNPJ/CPF do emitente";
		this.olvEmitCNPJ.Width = 130;
		this.olvEmitNome.AspectName = "EmitNome";
		this.olvEmitNome.Text = "Emissor Nome";
		this.olvEmitNome.ToolTipText = "Nome do emitente";
		this.olvEmitNome.Width = 230;
		this.olvManifest.Text = "Manifestação";
		this.olvManifest.ToolTipText = "Manifestação";
		this.olvCancel.AspectName = "Canceled";
		this.olvCancel.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.Text = "Can";
		this.olvCancel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.ToolTipText = "Cancelado";
		this.olvCancel.Width = 35;
		this.olvHasEvent.AspectName = "HasEvent";
		this.olvHasEvent.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasEvent.Text = "Evt";
		this.olvHasEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasEvent.ToolTipText = "Tem evento vinculado";
		this.olvHasEvent.Width = 35;
		this.olvHasCTeEvent.AspectName = "HasCTeEvent";
		this.olvHasCTeEvent.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasCTeEvent.Text = "CTe";
		this.olvHasCTeEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasCTeEvent.ToolTipText = "Tem CTe vinculado";
		this.olvHasCTeEvent.Width = 35;
		this.olvMDFeEvtData.AspectName = "MDFeEvtData";
		this.olvMDFeEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.Text = "MDFe";
		this.olvMDFeEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.ToolTipText = "Data manifesto eletrônico de documentos fiscais";
		this.olvMDFeEvtData.Width = 35;
		this.olvMotivo.AspectName = "xMotivo";
		this.olvMotivo.Text = "Status";
		this.olvMotivo.ToolTipText = "Motivo";
		this.olvMotivo.Width = 200;
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Sigla da UF";
		this.olvEstado.Width = 30;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "Chave";
		this.olvChave.ToolTipText = "Chave do documento fiscal";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 130;
		this.olvTpDoc.AspectName = "TpDoc";
		this.olvTpDoc.Text = "TipoDoc";
		this.olvTpDoc.ToolTipText = "Tipo do documento";
		this.olvTpServ.AspectName = "tpServ";
		this.olvTpServ.Text = "TipoServ";
		this.olvTpServ.ToolTipText = "Tipo de serviço";
		this.olvModal.AspectName = "Modal";
		this.olvModal.Text = "Modal";
		this.olvModal.ToolTipText = "Código do modelo do documento fiscal";
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "Natureza";
		this.olvNatOper.ToolTipText = "Descrição da Natureza da operação ";
		this.olvProPred.AspectName = "ProPred";
		this.olvProPred.Text = "Produto predominante";
		this.olvProPred.ToolTipText = "Produto predominante";
		this.olvBaseIcms.AspectName = "BaseICMS";
		this.olvBaseIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseIcms.Text = "ICMS Base";
		this.olvBaseIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseIcms.ToolTipText = "Valor da BC do ICMS";
		this.olvBaseIcms.Width = 100;
		this.olvTaxaIcms.AspectName = "TaxaICMS";
		this.olvTaxaIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTaxaIcms.Text = "ICMS Taxa";
		this.olvTaxaIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvTaxaIcms.ToolTipText = "Taxa da BC do ICMS";
		this.olvTaxaIcms.Width = 100;
		this.olvValorIcms.AspectName = "ValorICMS";
		this.olvValorIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorIcms.Text = "ICMS Valor";
		this.olvValorIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorIcms.ToolTipText = "Valor da BC do ICMS";
		this.olvValorIcms.Width = 100;
		this.olvCdCstICMS.AspectName = "CdCstICMS";
		this.olvCdCstICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMS.Text = "ICMS ST";
		this.olvCdCstICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMS.ToolTipText = "Valor da BC do ICMS ST ";
		this.olvCdCstICMS.Width = 100;
		this.olvDescCstICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDescCstICMS.Text = "ICMS ST Desc";
		this.olvDescCstICMS.ToolTipText = "Código de situação tributária";
		this.olvDescCstICMS.Width = 200;
		this.olvIndSimplesNac.AspectName = "IndSimplesNac";
		this.olvIndSimplesNac.Text = "Simples";
		this.olvIndSimplesNac.ToolTipText = "Indica se o contribuinte é simples nacional";
		this.olvCFOP.AspectName = "CFOPList";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.ToolTipText = "Código fiscal de operações e prestações ";
		this.olvCFOP.Width = 90;
		this.olvMunIni.AspectName = "cMunIni";
		this.olvMunIni.Text = "MunIniCod";
		this.olvMunIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMunIni.ToolTipText = "Código do município de início";
		this.olvMunIni.Width = 100;
		this.olvxMunIni.AspectName = "xMunIni";
		this.olvxMunIni.Text = "MunIniNome";
		this.olvxMunIni.ToolTipText = "Nome do município do início";
		this.olvxMunIni.Width = 100;
		this.olvUFIni.AspectName = "UFIni";
		this.olvUFIni.Text = "MunIniUf";
		this.olvUFIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUFIni.ToolTipText = "UF do início da prestação ";
		this.olvUFIni.Width = 100;
		this.olvMunFim.AspectName = "cMunFim";
		this.olvMunFim.Text = "MunFimCod";
		this.olvMunFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMunFim.ToolTipText = "Código do município de termino da prestação";
		this.olvMunFim.Width = 100;
		this.olvxMunFim.AspectName = "xMunFim";
		this.olvxMunFim.Text = "MunFimNome";
		this.olvxMunFim.ToolTipText = "Nome do Município da prestação";
		this.olvxMunFim.Width = 100;
		this.olvUFFim.AspectName = "UFFim";
		this.olvUFFim.Text = "MunFimUf";
		this.olvUFFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUFFim.ToolTipText = "UF do término da prestação";
		this.olvcMunEnv.AspectName = "cMunEnv";
		this.olvcMunEnv.Text = "MunEnvCod";
		this.olvcMunEnv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvcMunEnv.ToolTipText = "Código do município de envio do CT-e (de onde o documento foi transmitido)";
		this.olvxMunEnv.AspectName = "xMunEnv";
		this.olvxMunEnv.Text = "MunEnvNome";
		this.olvxMunEnv.ToolTipText = "Nome do município de envio do CT-e (de onde o documento foi transmitido)";
		this.olvUFEnv.AspectName = "UFEnv";
		this.olvUFEnv.Text = "MunEnvUf";
		this.olvUFEnv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUFEnv.ToolTipText = "Sigla da UF de envio do CT-e";
		this.olvEmitIE.AspectName = "EmitIE";
		this.olvEmitIE.Text = "Emissor IE";
		this.olvEmitIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitIE.ToolTipText = "Inscrição Estadual do emitente";
		this.olvEmitcMun.AspectName = "EmitcMun";
		this.olvEmitcMun.Text = "Emissor Mun Cod";
		this.olvEmitcMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitcMun.ToolTipText = "Código do emitente";
		this.olvEmitxMun.AspectName = "EmitxMun";
		this.olvEmitxMun.Text = "Emissor Mun Desc";
		this.olvEmitxMun.ToolTipText = "Nome do município do emitente";
		this.olvEmitUf.AspectName = "EmitUf";
		this.olvEmitUf.Text = "Emissor Uf";
		this.olvEmitUf.ToolTipText = "Sigla da UF do emitente";
		this.olvTomaId.AspectName = "TomaID";
		this.olvTomaId.Text = "Tomador CNPJ/CPF";
		this.olvTomaId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomaId.ToolTipText = "CNPJ/CPF do tomador";
		this.olvTomaId.Width = 130;
		this.olvTomaIE.AspectName = "TomaIE";
		this.olvTomaIE.Text = "Tomador IE";
		this.olvTomaIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomaIE.ToolTipText = "IE do tomador";
		this.olvTomaNome.AspectName = "TomaNome";
		this.olvTomaNome.Text = "Tomador Nome";
		this.olvTomaNome.ToolTipText = "Nome do tomador";
		this.olvTomaNome.Width = 230;
		this.olvTomacMun.AspectName = "TomacMun";
		this.olvTomacMun.Text = "Tomador Mun Cod";
		this.olvTomacMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomacMun.ToolTipText = "Código do municipío do tomador";
		this.olvTomaxMun.AspectName = "TomaxMun";
		this.olvTomaxMun.Text = "Tomador Mun Desc";
		this.olvTomaxMun.ToolTipText = "Nome do municipío do tomador";
		this.olvTomaUf.AspectName = "TomaUf";
		this.olvTomaUf.Text = "Tomador Uf";
		this.olvTomaUf.ToolTipText = "Sigla da UF do tomador";
		this.olvExpediId.AspectName = "ExpediID";
		this.olvExpediId.Text = "Expedidor CNPJ/CPF";
		this.olvExpediId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExpediId.ToolTipText = "CNPJ/CPF do expedidor";
		this.olvExpediId.Width = 130;
		this.olvExpediIE.AspectName = "ExpediIE";
		this.olvExpediIE.Text = "Expedidor IE";
		this.olvExpediIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExpediIE.ToolTipText = "IE do Expedidor";
		this.olvExpediNome.AspectName = "ExpediNome";
		this.olvExpediNome.Text = "Expedidor Nome";
		this.olvExpediNome.ToolTipText = "Nome do expedidor";
		this.olvExpediNome.Width = 230;
		this.olvExpedicMun.AspectName = "ExpedicMun";
		this.olvExpedicMun.Text = "Expedidor Mun Cod";
		this.olvExpedicMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExpedicMun.ToolTipText = "Código do município do expedidor";
		this.olvExpedixMun.AspectName = "ExpedixMun";
		this.olvExpedixMun.Text = "Expedidor Mun Desc";
		this.olvExpedixMun.ToolTipText = "Nome do município do expedidor";
		this.olvExpediUf.AspectName = "ExpediUf";
		this.olvExpediUf.Text = "Expedidor Uf";
		this.olvExpediUf.ToolTipText = "Sigla da UF do expedidor";
		this.olvRemeteId.AspectName = "RemeteID";
		this.olvRemeteId.Text = "Remetente CNPJ/CPF";
		this.olvRemeteId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRemeteId.ToolTipText = "CNPJ/CPF do emitente";
		this.olvRemeteId.Width = 130;
		this.olvRemeteIE.AspectName = "RemeteIE";
		this.olvRemeteIE.Text = "Remetente IE";
		this.olvRemeteIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRemeteIE.ToolTipText = "Inscrição estadual do remetente";
		this.olvRemeteNome.AspectName = "RemeteNome";
		this.olvRemeteNome.Text = "Remetente Nome";
		this.olvRemeteNome.ToolTipText = "Nome do remetente";
		this.olvRemeteNome.Width = 230;
		this.olvRemetecMun.AspectName = "RemetecMun";
		this.olvRemetecMun.Text = "Remetente Mun Cod";
		this.olvRemetecMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRemetecMun.ToolTipText = "Código do município do remetente";
		this.olvRemetexMun.AspectName = "RemetexMun";
		this.olvRemetexMun.Text = "Remetente Mun Desc";
		this.olvRemetexMun.ToolTipText = "Nome do município do remetente";
		this.olvRemeteUf.AspectName = "RemeteUf";
		this.olvRemeteUf.Text = "Remetente Uf";
		this.olvRemeteUf.ToolTipText = "Sigla da UF do remente";
		this.olvDestinId.AspectName = "DestinID";
		this.olvDestinId.Text = "Destinatário CNPJ/CPF";
		this.olvDestinId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinId.ToolTipText = "CNPJ/CPF do destinatário";
		this.olvDestinId.Width = 130;
		this.olvDestinIE.AspectName = "DestinIE";
		this.olvDestinIE.Text = "Destinatário IE";
		this.olvDestinIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinIE.ToolTipText = "Inscrição Estadual do destinatário";
		this.olvDestinNome.AspectName = "DestinNome";
		this.olvDestinNome.Text = "Destinatário Nome";
		this.olvDestinNome.ToolTipText = "Nome do destinatário";
		this.olvDestinNome.Width = 230;
		this.olvDestincMun.AspectName = "DestincMun";
		this.olvDestincMun.Text = "Destinatário Mun Cod";
		this.olvDestincMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestincMun.ToolTipText = "Código do municipío do destinatário";
		this.olvDestinxMun.AspectName = "DestinxMun";
		this.olvDestinxMun.Text = "Destinatário Mun Desc";
		this.olvDestinxMun.ToolTipText = "Nome do destinatário";
		this.olvDestinUf.AspectName = "DestinUf";
		this.olvDestinUf.Text = "Destinatário Uf";
		this.olvDestinUf.ToolTipText = "Sigla da UF do destinatário";
		this.olvRecebeId.AspectName = "RecebeID";
		this.olvRecebeId.Text = "Recebedor CNPJ/CPF";
		this.olvRecebeId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecebeId.ToolTipText = "CNPJ/CPF do recebedor";
		this.olvRecebeId.Width = 130;
		this.olvRecebeIE.AspectName = "RecebeIE";
		this.olvRecebeIE.Text = "Recebedor IE";
		this.olvRecebeIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecebeIE.ToolTipText = "Inscrição estadual do recebedor";
		this.olvRecebeNome.AspectName = "RecebeNome";
		this.olvRecebeNome.Text = "Recebedor Nome";
		this.olvRecebeNome.ToolTipText = "Nome do recebedor";
		this.olvRecebeNome.Width = 230;
		this.olvRecebecMun.AspectName = "RecebecMun";
		this.olvRecebecMun.Text = "Recebedor Mun Cod";
		this.olvRecebecMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecebecMun.ToolTipText = "Código do municipío do recebedor";
		this.olvRecebexMun.AspectName = "RecebexMun";
		this.olvRecebexMun.Text = "Recebedor Mun Desc";
		this.olvRecebexMun.ToolTipText = "Nome do município do recebedor";
		this.olvRecebeUf.AspectName = "RecebeUf";
		this.olvRecebeUf.Text = "Recebedor Uf";
		this.olvRecebeUf.ToolTipText = "Sigla do destino do recebedor";
		this.olvInfAdFisco.Text = "Informações adicionais fisco";
		this.olvInfAdFisco.ToolTipText = "Informações adicionais do fiscal";
		this.olvInfAdFisco.Width = 150;
		this.olvInfCpl.Text = "Informações complementares ";
		this.olvInfCpl.Width = 150;
		this.olvObsCont.Text = "Observações contribuinte";
		this.olvObsCont.Width = 150;
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Data de autorização";
		this.olvError.AspectName = "XmlError";
		this.olvError.Text = "Validação";
		this.olvError.ToolTipText = "Status de validação  do XML ";
		this.olvBatchNum.AspectName = "BatchNum";
		this.olvBatchNum.Text = "Lote Proc.";
		this.olvBatchNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBatchNum.ToolTipText = "Número do lote de processamento";
		this.olvFretePesoTransp.AspectName = "FretePesoTransp";
		this.olvFretePesoTransp.Text = "Peso Transportado";
		this.olvFretePesoTransp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFretePesoTransp.ToolTipText = "Peso total em Kg ";
		this.olvFretePesoTransp.Width = 100;
		this.olvFreteUnidadePeso.AspectName = "FreteUnidadePeso";
		this.olvFreteUnidadePeso.Text = "Unidade";
		this.olvFreteUnidadePeso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFreteUnidadePeso.ToolTipText = "Frete unidade";
		this.olvFreteUnidadePeso.Width = 100;
		this.olvFreteValor.AspectName = "FreteValor";
		this.olvFreteValor.Text = "Frete Valor";
		this.olvFreteValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteValor.ToolTipText = "Valor do frete";
		this.olvFreteValor.Width = 100;
		this.olvFreteVlrPesoVol.AspectName = "FreteVlrPesoVol";
		this.olvFreteVlrPesoVol.Text = "Peso/Volume Valor";
		this.olvFreteVlrPesoVol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrPesoVol.ToolTipText = "Frete peso/volume/valor";
		this.olvFreteVlrPesoVol.Width = 100;
		this.olvFreteVlrPedagio.AspectName = "FreteVlrPedagio";
		this.olvFreteVlrPedagio.Text = "Pedágio Valor";
		this.olvFreteVlrPedagio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrPedagio.ToolTipText = "Frete valor pedágio";
		this.olvFreteVlrPedagio.Width = 100;
		this.olvFreteVlrDespacho.AspectName = "FreteVlrDespacho";
		this.olvFreteVlrDespacho.Text = "Despacho Valor";
		this.olvFreteVlrDespacho.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrDespacho.ToolTipText = "Frete valor despacho";
		this.olvFreteVlrDespacho.Width = 100;
		this.olvFreteVlrSecCat.AspectName = "FreteVlrSecCat";
		this.olvFreteVlrSecCat.Text = "SEC-CAT Valor";
		this.olvFreteVlrSecCat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrSecCat.ToolTipText = "Frete do valor do SEC/CAT";
		this.olvFreteVlrSecCat.Width = 100;
		this.olvFreteVlrItr.AspectName = "FreteVlrItr";
		this.olvFreteVlrItr.Text = "ITR Valor";
		this.olvFreteVlrItr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrItr.ToolTipText = "Frete do valor do litro";
		this.olvFreteVlrItr.Width = 100;
		this.olvFreteVlrAdeme.AspectName = "FreteVlrAdeme";
		this.olvFreteVlrAdeme.Text = "ADEME Valor";
		this.olvFreteVlrAdeme.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrAdeme.ToolTipText = "Valor do frete ademe";
		this.olvFreteVlrAdeme.Width = 100;
		this.olvFreteVlrOutras.AspectName = "FreteVlrOutras";
		this.olvFreteVlrOutras.Text = "Outras Despesas";
		this.olvFreteVlrOutras.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVlrOutras.ToolTipText = "Frete valor outras despesas";
		this.olvFreteVlrOutras.Width = 100;
		this.olvFreteVolTransp.AspectName = "FreteVolTransp";
		this.olvFreteVolTransp.Text = "Volume Transportado";
		this.olvFreteVolTransp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVolTransp.ToolTipText = "Frete volume transportado";
		this.olvFreteVolTransp.Width = 160;
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvDocNote.ToolTipText = "Comentários";
		this.olvValorCarga.AspectName = "ValorCarga";
		this.olvValorCarga.Text = "Valor da Carga";
		this.olvValorCarga.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorCarga.ToolTipText = "Valor da carga";
		this.olvBaseIcmsOutra.AspectName = "BaseICMSOutraUF";
		this.olvBaseIcmsOutra.DisplayIndex = 100;
		this.olvBaseIcmsOutra.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseIcmsOutra.IsVisible = false;
		this.olvBaseIcmsOutra.Text = "CTe:Base Icms Outra UF";
		this.olvBaseIcmsOutra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseIcmsOutra.ToolTipText = "Valor da BC do ICMS";
		this.olvValorIcmsOutra.AspectName = "ValorICMSOutraUF";
		this.olvValorIcmsOutra.DisplayIndex = 101;
		this.olvValorIcmsOutra.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorIcmsOutra.IsVisible = false;
		this.olvValorIcmsOutra.Text = "CTe:Valor ICMS Outra UF";
		this.olvValorIcmsOutra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorIcmsOutra.ToolTipText = "Valor do ICMS devido outra UF";
		this.olvTaxaICMSOutraUF.AspectName = "TaxaICMSOutraUF";
		this.olvTaxaICMSOutraUF.DisplayIndex = 102;
		this.olvTaxaICMSOutraUF.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTaxaICMSOutraUF.IsVisible = false;
		this.olvTaxaICMSOutraUF.Text = "CTe:Taxa ICMS Outra UF";
		this.olvTaxaICMSOutraUF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvTaxaICMSOutraUF.ToolTipText = "Taxa do ICMS devido outra UF";
		this.olvTagDtHr.AspectName = "TagDtHr";
		this.olvTagDtHr.Text = "Etiq : Data Hora";
		this.olvTagDtHr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTagDtHr.ToolTipText = "Data/hora de atribuição da etiqueta";
		this.olvTagUser.AspectName = "TagUser";
		this.olvTagUser.Text = "Etiq : Usuário";
		this.olvTagUser.ToolTipText = "Usuário que atribuiu a etiqueta";
		this.olvDocNoteDtHr.AspectName = "DocNoteDtHr";
		this.olvDocNoteDtHr.Text = "Comentário : Data Hora";
		this.olvDocNoteDtHr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocNoteDtHr.ToolTipText = "Data/hora de atribuição do comentário";
		this.olvDocNoteUser.AspectName = "DocNoteUser";
		this.olvDocNoteUser.Text = "Comentário : Usuário";
		this.olvDocNoteUser.ToolTipText = "Usuário que atribuiu o comentário";
		this.olvVersion.AspectName = "Version";
		this.olvVersion.Text = "XML: Versão";
		this.olvVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
		this.btSalesContact.TabIndex = 217;
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
		this.lknAction.Text = "CTe Dados Sintéticos";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(17, 129);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 54);
		this.lbText02.TabIndex = 196;
		this.lbText02.Text = "Para visualizar todos os campos do XML disponíveis neste relatório, clique com o botão direito do mouse sobre o título de qualquer coluna do relatório e escolha a opção [Selecionar colunas...].";
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 81);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 35);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = "Apresenta a visualização de todas as informações presentes no cabeçalho dos CTe: informações de impostos, origem e destino da mercadoria, transportador, etc.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "CTe: Dados Sintéticos é ativado nos \r\nplanos Avançado e Enterprise.";
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
		base.Controls.Add(this.lsvData);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocCTe";
		this.Text = "CTe: Dados Sintéticos";
		base.Load += new System.EventHandler(frmTabDocCTe_Load);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
