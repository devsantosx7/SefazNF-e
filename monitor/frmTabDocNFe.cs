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

public class frmTabDocNFe : Form
{
	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocNFe _SqlTabData = new clsSrvTabDocNFe();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private string _InfoAddXml = string.Empty;

	private string _varShowColumnsPartner = string.Empty;

	private clsDataPartner varclsDataPartner = new clsDataPartner();

	private List<Partner> _partnerCache = new List<Partner>();

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private bool _HasFiscalioConnect;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private Panel pnContent;

	private ImageList ImageListDocs;

	private FastObjectListView lsvData;

	private OLVColumn olvSelect;

	private OLVColumn olvHasXml;

	private OLVColumn olvNum;

	private OLVColumn olvSerie;

	private OLVColumn olvTipo;

	private OLVColumn olvTag;

	private OLVColumn olvDtAut;

	private OLVColumn olvValor;

	private OLVColumn olvVenc;

	private OLVColumn olvCancel;

	private OLVColumn olvHasEvent;

	private OLVColumn olvEmitID;

	private OLVColumn olvEmitNome;

	private OLVColumn olvMotivo;

	private OLVColumn olvEstado;

	private OLVColumn olvChave;

	private OLVColumn olvFilial;

	private OLVColumn olvTpDoc;

	private OLVColumn olvNatOper;

	private OLVColumn olvIndSimplesNac;

	private OLVColumn olvCFOP;

	private OLVColumn olvNCM;

	private OLVColumn olvBaseICMS;

	private OLVColumn olvTaxaICMS;

	private OLVColumn olvValorICMS;

	private OLVColumn olvCdCstICMS;

	private OLVColumn olvVlrICMSDeson;

	private OLVColumn olvVlrBCST;

	private OLVColumn olvVlrTotST;

	private OLVColumn olvVlrTotProd;

	private OLVColumn olvVlrTotFrete;

	private OLVColumn olvVlrTotSeg;

	private OLVColumn olvVlrTotDesc;

	private OLVColumn olvVlrTotII;

	private OLVColumn olvVlrTotIPI;

	private OLVColumn olvVlrTotIPIDevol;

	private OLVColumn olvVlrTotPIS;

	private OLVColumn olvVlrTotCOFINS;

	private OLVColumn olvVlrTotOutro;

	private OLVColumn olvVlrTotTrib;

	private OLVColumn olvDtEmi;

	private OLVColumn olvHrEmi;

	private OLVColumn olvEmitIE;

	private OLVColumn olvEmitcMun;

	private OLVColumn olvEmitxMun;

	private OLVColumn olvEmitUf;

	private OLVColumn olvDestinId;

	private OLVColumn olvDestinIE;

	private OLVColumn olvDestinNome;

	private OLVColumn olvDestincMun;

	private OLVColumn olvDestinxMun;

	private OLVColumn olvDestinUf;

	private OLVColumn olvDestinIndIE;

	private OLVColumn olvDestinCEP;

	private OLVColumn olvDestinBairro;

	private OLVColumn olvTranspId;

	private OLVColumn olvTranspIE;

	private OLVColumn olvTranspNome;

	private OLVColumn olvTranspcMun;

	private OLVColumn olvTranspxMun;

	private OLVColumn olvTranspUf;

	private OLVColumn olvEntregID;

	private OLVColumn olvEntregNome;

	private OLVColumn olvEntregIE;

	private OLVColumn olvEntregcMun;

	private OLVColumn olvEntregxMun;

	private OLVColumn olvEntregUF;

	private OLVColumn olvEntregxLgr;

	private OLVColumn olvEntregNro;

	private OLVColumn olvEntregxBairro;

	private OLVColumn olvEntregCEP;

	private OLVColumn olvRetiraID;

	private OLVColumn olvRetiraNome;

	private OLVColumn olvRetiraIE;

	private OLVColumn olvRetiracMun;

	private OLVColumn olvRetiraxMun;

	private OLVColumn olvRetiraUF;

	private OLVColumn olvRetiraxLgr;

	private OLVColumn olvRetiraNro;

	private OLVColumn olvRetiraxBairro;

	private OLVColumn olvRetiraCEP;

	private OLVColumn olvTrnPlaca;

	private OLVColumn olvTrnUf;

	private OLVColumn olvTrnRNTC;

	private OLVColumn olvVolQuant;

	private OLVColumn olvVolEspec;

	private OLVColumn olvVolMarca;

	private OLVColumn olvVolNumer;

	private OLVColumn olvVolPesoL;

	private OLVColumn olvVolPesoB;

	private OLVColumn olvInfCpl;

	private OLVColumn olvInfAdFisco;

	private OLVColumn olvError;

	private OLVColumn olvAnoMes;

	private OLVColumn olvStatus;

	private OLVColumn olvManifest;

	private OLVColumn olvHasCTeEvent;

	private OLVColumn olvMDFeEvtData;

	private OLVColumn olvSufVistEvtData;

	private OLVColumn olvSufVistInteData;

	private OLVColumn olvBatchNum;

	private OLVColumn olvDtEntFisc;

	private OLVColumn olvDtRegFisc;

	private OLVColumn olvUsRegFisc;

	private OLVColumn olvDcNumFisc;

	private OLVColumn olvEmitida;

	private OLVColumn olvCompranEmp;

	private OLVColumn olvCompraxPed;

	private OLVColumn olvCompraxCont;

	private OLVColumn olvVlrTotFCP;

	private OLVColumn olvVlrTotFCPUFDest;

	private OLVColumn olvVlrTotFCPST;

	private OLVColumn olvVlrTotFCPSTRet;

	private OLVColumn olvVlrTotICMSUFRemet;

	private OLVColumn olvVlrTotICMSUFDest;

	private OLVColumn olvDocNote;

	private OLVColumn olvBaseIcmsOutra;

	private OLVColumn olvValorIcmsOutra;

	private OLVColumn olvTaxaIcmsOutra;

	private OLVColumn olvIndPres;

	private OLVColumn olvIndPag;

	private OLVColumn olvTPag;

	private OLVColumn olvVPag;

	private OLVColumn olvIndIntermed;

	private Panel pnMarket;

	private LinkLabel lknClose;

	private Panel pnMarketContent;

	private Label lbTitle03;

	private LinkLabel lknAction;

	private Label lbText02;

	private Label lbText01;

	private Label lbTitle02;

	private Button btClose;

	private PictureBox picWarning;

	private Label label3;

	private Label lbTitle01;

	private Button btSalesAction;

	private OLVColumn olvTagDtHr;

	private OLVColumn olvTagUser;

	private OLVColumn olvDocNoteDtHr;

	private OLVColumn olvDocNoteUser;

	private OLVColumn olvVersion;

	private OLVColumn olvModFrete;

	private Button btSalesContact;

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

	public frmTabDocNFe(string pTitle, Color pColumnColor, string pFeatExtId)
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

	private async void frmTabDocNFe_Load(object sender, EventArgs e)
	{
		_InfoAddXml = (await new clsDataConfig().funcGetItemByKeyAsync()).InfoAddXmlNFe;
		_StateList = await new clsDataEstado().funcGetListAsync(pWthAN: false);
		_HasFiscalioConnect = await clsScreenGeral.funcMustShowRegFieldsAsync();
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		_varShowColumnsPartner = await _clsDataParam.funcGetAsync("SHOW-COLUMNS-PARTNER", pBuffer: true, pGlobal: true);
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
		if (string.IsNullOrEmpty(_InfoAddXml))
		{
			List<string> varColList = new List<string>();
			varColList.Add(clsFunction.funcClearSpecialCaracter("Informações complementares"));
			varColList.Add(clsFunction.funcClearSpecialCaracter("Informações adicionais fisco"));
			varColList.Add(clsFunction.funcClearSpecialCaracter("Observações contribuinte"));
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList);
		}
		if (!_HasFiscalioConnect)
		{
			List<string> varColList2 = new List<string> { "DtEntFisc", "DtRegFisc", "UsRegFisc", "DcNumFisc" };
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList2);
		}
		if (clsFunction.IsEmpty(_varShowColumnsPartner))
		{
			List<string> varColList3 = new List<string> { "Retirada Logradouro", "Retirada Bairro", "Retirada CEP", "Entrega Logradouro", "Entrega Bairro", "Entrega CEP" };
			varColList3.Add(clsFunction.funcClearSpecialCaracter("Retirada Número"));
			varColList3.Add(clsFunction.funcClearSpecialCaracter("Entrega Número"));
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList3);
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
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			Document document = (Document)x;
			return (document == null) ? null : clsScreenGeral.funcGetDocStatIcon(document);
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
		olvIndSimplesNac.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetIndSimples(document.Model, document.IndSimplesNac);
		};
		olvManifest.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.GetEventDesc(document.Model, document.ManifCode);
		};
		olvDestinIndIE.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetindIEDest(document.Model, document.DestinIndIE);
		};
		olvEstado.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
		};
		olvFilial.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsFunction.funcFormatDoc(document.Filial);
		};
		olvTpDoc.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetTipoDoc(document.Model, document.TpDoc);
		};
		olvDestinIE.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsSrvGeral.funcGetIEFormat(document.DestinIE);
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
		olvInfAdFisco.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXml) ? null : clsScreenGeral.funcGetDocText(document.Chave)?.infAdFisco;
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
		olvEntregxLgr.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.EntregID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.EntregID))?.Lograd;
		};
		olvEntregNro.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.EntregID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.EntregID))?.Numero;
		};
		olvEntregxBairro.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.EntregID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.EntregID))?.Bairro;
		};
		olvEntregCEP.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.EntregID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.EntregID))?.CodCep;
		};
		olvRetiraxLgr.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.RetiraID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.RetiraID))?.Lograd;
		};
		olvRetiraNro.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.RetiraID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.RetiraID))?.Numero;
		};
		olvRetiraxBairro.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.RetiraID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.RetiraID))?.Bairro;
		};
		olvRetiraCEP.AspectGetter = delegate(object x)
		{
			Document varDocument = (Document)x;
			if (varDocument == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(_varShowColumnsPartner))
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(varDocument.RetiraID) ? null : _partnerCache.FirstOrDefault((Partner r) => r.ID.Equals(varDocument.RetiraID))?.CodCep;
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
		olvModFrete.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetNFeModFrete(document.Modal, document.ModFrete);
		};
		olvIndPag.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetindPag(document.Model, document.indPag);
		};
		olvTPag.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetTPag(document.Model, document.tPag);
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
		List<Document> varclsDocList = await _clsDataDoc.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		if (clsFunction.IsEqual(_varShowColumnsPartner, "ENABLE"))
		{
			await LoadPartnersCacheAsync(varclsDocList);
		}
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
			varOlvColum = olvDestinIE;
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

	public List<Document> funcGetObjList(bool pFocused, bool pChecked)
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
		funcShowDocViewer(pShowPDF: true, pShowXML: false);
	}

	private async void funcShowDocViewer(bool pShowPDF, bool pShowXML, Document pDocument = null)
	{
		if (!_IsLocked)
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
				funcShowDocViewer(pShowPDF: true, pShowXML: false, varDocument);
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
			if (clsFunction.IsEqual(varclsValue.Name, "Document", pIgnoreCase: true))
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
		clsHelpService.funcCallTipReportNFeSyntheticAsync();
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private async Task<clsReturn> LoadPartnersCacheAsync(List<Document> varDocuments)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		List<string> varPartnerIds = new List<string>();
		try
		{
			varPartnerIds = (from id in varDocuments.SelectMany((Document d) => new string[2] { d.RetiraID, d.EntregID })
				where !clsFunction.IsEmpty(id)
				select id).Distinct().ToList();
			if (varPartnerIds.Any())
			{
				_partnerCache = await varclsDataPartner.funcGetPartnerByIdAsync(varPartnerIds);
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
			_partnerCache.Clear();
		}
		finally
		{
			varPartnerIds.Clear();
		}
		return varclsReturnFunc;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocNFe));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.pnContent = new System.Windows.Forms.Panel();
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
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitida = new BrightIdeasSoftware.OLVColumn();
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
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
		this.olvEmitID = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitNome = new BrightIdeasSoftware.OLVColumn();
		this.olvManifest = new BrightIdeasSoftware.OLVColumn();
		this.olvCancel = new BrightIdeasSoftware.OLVColumn();
		this.olvHasEvent = new BrightIdeasSoftware.OLVColumn();
		this.olvHasCTeEvent = new BrightIdeasSoftware.OLVColumn();
		this.olvMDFeEvtData = new BrightIdeasSoftware.OLVColumn();
		this.olvSufVistEvtData = new BrightIdeasSoftware.OLVColumn();
		this.olvSufVistInteData = new BrightIdeasSoftware.OLVColumn();
		this.olvMotivo = new BrightIdeasSoftware.OLVColumn();
		this.olvEstado = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvIndSimplesNac = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvBaseICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvTaxaICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvValorICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvCdCstICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrICMSDeson = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrBCST = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotST = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotProd = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotSeg = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotOutro = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotII = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotIPI = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotIPIDevol = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotPIS = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotCOFINS = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotFCP = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotFCPUFDest = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotFCPST = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotFCPSTRet = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotICMSUFRemet = new BrightIdeasSoftware.OLVColumn();
		this.olvVlrTotICMSUFDest = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitIE = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitcMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinId = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDestincMun = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinIndIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinCEP = new BrightIdeasSoftware.OLVColumn();
		this.olvDestinBairro = new BrightIdeasSoftware.OLVColumn();
		this.olvTranspId = new BrightIdeasSoftware.OLVColumn();
		this.olvTranspIE = new BrightIdeasSoftware.OLVColumn();
		this.olvTranspNome = new BrightIdeasSoftware.OLVColumn();
		this.olvTranspcMun = new BrightIdeasSoftware.OLVColumn();
		this.olvTranspxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvTranspUf = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregID = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregNome = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregIE = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregcMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregUF = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregxLgr = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregNro = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregxBairro = new BrightIdeasSoftware.OLVColumn();
		this.olvEntregCEP = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraID = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraNome = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraIE = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiracMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraxMun = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraUF = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraxLgr = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraNro = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraxBairro = new BrightIdeasSoftware.OLVColumn();
		this.olvRetiraCEP = new BrightIdeasSoftware.OLVColumn();
		this.olvTrnPlaca = new BrightIdeasSoftware.OLVColumn();
		this.olvTrnUf = new BrightIdeasSoftware.OLVColumn();
		this.olvTrnRNTC = new BrightIdeasSoftware.OLVColumn();
		this.olvVolQuant = new BrightIdeasSoftware.OLVColumn();
		this.olvVolEspec = new BrightIdeasSoftware.OLVColumn();
		this.olvVolMarca = new BrightIdeasSoftware.OLVColumn();
		this.olvVolNumer = new BrightIdeasSoftware.OLVColumn();
		this.olvVolPesoL = new BrightIdeasSoftware.OLVColumn();
		this.olvVolPesoB = new BrightIdeasSoftware.OLVColumn();
		this.olvInfCpl = new BrightIdeasSoftware.OLVColumn();
		this.olvInfAdFisco = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvError = new BrightIdeasSoftware.OLVColumn();
		this.olvBatchNum = new BrightIdeasSoftware.OLVColumn();
		this.olvCompranEmp = new BrightIdeasSoftware.OLVColumn();
		this.olvCompraxPed = new BrightIdeasSoftware.OLVColumn();
		this.olvCompraxCont = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvBaseIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvValorIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvTaxaIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvIndPres = new BrightIdeasSoftware.OLVColumn();
		this.olvIndPag = new BrightIdeasSoftware.OLVColumn();
		this.olvTPag = new BrightIdeasSoftware.OLVColumn();
		this.olvVPag = new BrightIdeasSoftware.OLVColumn();
		this.olvIndIntermed = new BrightIdeasSoftware.OLVColumn();
		this.olvTagDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvTagUser = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteUser = new BrightIdeasSoftware.OLVColumn();
		this.olvVersion = new BrightIdeasSoftware.OLVColumn();
		this.olvModFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvObsCont = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.pnContent.SuspendLayout();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.pnContent.Controls.Add(this.pnMarket);
		this.pnContent.Controls.Add(this.lsvData);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(784, 445);
		this.pnContent.TabIndex = 10;
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 117);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(784, 328);
		this.pnMarket.TabIndex = 11;
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
		this.btSalesContact.TabIndex = 221;
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
		this.lknAction.Text = "NFe Dados Sintéticos";
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
		this.lbText01.Text = "Apresenta a visualização de todas as informações presentes no cabeçalho de NFe, NFCe e CFeSAT: informações de impostos, origem e destino da mercadoria, transportador, etc.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "NFe: Dados Sintéticos é ativado nos \r\nplanos Avançado e Enterprise.";
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
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvEmitida);
		this.lsvData.AllColumns.Add(this.olvHasXml);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvTag);
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
		this.lsvData.AllColumns.Add(this.olvEmitID);
		this.lsvData.AllColumns.Add(this.olvEmitNome);
		this.lsvData.AllColumns.Add(this.olvManifest);
		this.lsvData.AllColumns.Add(this.olvCancel);
		this.lsvData.AllColumns.Add(this.olvHasEvent);
		this.lsvData.AllColumns.Add(this.olvHasCTeEvent);
		this.lsvData.AllColumns.Add(this.olvMDFeEvtData);
		this.lsvData.AllColumns.Add(this.olvSufVistEvtData);
		this.lsvData.AllColumns.Add(this.olvSufVistInteData);
		this.lsvData.AllColumns.Add(this.olvMotivo);
		this.lsvData.AllColumns.Add(this.olvEstado);
		this.lsvData.AllColumns.Add(this.olvChave);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvTpDoc);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvIndSimplesNac);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvNCM);
		this.lsvData.AllColumns.Add(this.olvBaseICMS);
		this.lsvData.AllColumns.Add(this.olvTaxaICMS);
		this.lsvData.AllColumns.Add(this.olvValorICMS);
		this.lsvData.AllColumns.Add(this.olvCdCstICMS);
		this.lsvData.AllColumns.Add(this.olvVlrICMSDeson);
		this.lsvData.AllColumns.Add(this.olvVlrBCST);
		this.lsvData.AllColumns.Add(this.olvVlrTotST);
		this.lsvData.AllColumns.Add(this.olvVlrTotProd);
		this.lsvData.AllColumns.Add(this.olvVlrTotFrete);
		this.lsvData.AllColumns.Add(this.olvVlrTotSeg);
		this.lsvData.AllColumns.Add(this.olvVlrTotDesc);
		this.lsvData.AllColumns.Add(this.olvVlrTotOutro);
		this.lsvData.AllColumns.Add(this.olvVlrTotII);
		this.lsvData.AllColumns.Add(this.olvVlrTotIPI);
		this.lsvData.AllColumns.Add(this.olvVlrTotIPIDevol);
		this.lsvData.AllColumns.Add(this.olvVlrTotPIS);
		this.lsvData.AllColumns.Add(this.olvVlrTotCOFINS);
		this.lsvData.AllColumns.Add(this.olvVlrTotTrib);
		this.lsvData.AllColumns.Add(this.olvVlrTotFCP);
		this.lsvData.AllColumns.Add(this.olvVlrTotFCPUFDest);
		this.lsvData.AllColumns.Add(this.olvVlrTotFCPST);
		this.lsvData.AllColumns.Add(this.olvVlrTotFCPSTRet);
		this.lsvData.AllColumns.Add(this.olvVlrTotICMSUFRemet);
		this.lsvData.AllColumns.Add(this.olvVlrTotICMSUFDest);
		this.lsvData.AllColumns.Add(this.olvEmitIE);
		this.lsvData.AllColumns.Add(this.olvEmitcMun);
		this.lsvData.AllColumns.Add(this.olvEmitxMun);
		this.lsvData.AllColumns.Add(this.olvEmitUf);
		this.lsvData.AllColumns.Add(this.olvDestinId);
		this.lsvData.AllColumns.Add(this.olvDestinIE);
		this.lsvData.AllColumns.Add(this.olvDestinNome);
		this.lsvData.AllColumns.Add(this.olvDestincMun);
		this.lsvData.AllColumns.Add(this.olvDestinxMun);
		this.lsvData.AllColumns.Add(this.olvDestinUf);
		this.lsvData.AllColumns.Add(this.olvDestinIndIE);
		this.lsvData.AllColumns.Add(this.olvDestinCEP);
		this.lsvData.AllColumns.Add(this.olvDestinBairro);
		this.lsvData.AllColumns.Add(this.olvTranspId);
		this.lsvData.AllColumns.Add(this.olvTranspIE);
		this.lsvData.AllColumns.Add(this.olvTranspNome);
		this.lsvData.AllColumns.Add(this.olvTranspcMun);
		this.lsvData.AllColumns.Add(this.olvTranspxMun);
		this.lsvData.AllColumns.Add(this.olvTranspUf);
		this.lsvData.AllColumns.Add(this.olvEntregID);
		this.lsvData.AllColumns.Add(this.olvEntregNome);
		this.lsvData.AllColumns.Add(this.olvEntregIE);
		this.lsvData.AllColumns.Add(this.olvEntregcMun);
		this.lsvData.AllColumns.Add(this.olvEntregxMun);
		this.lsvData.AllColumns.Add(this.olvEntregUF);
		this.lsvData.AllColumns.Add(this.olvEntregxLgr);
		this.lsvData.AllColumns.Add(this.olvEntregNro);
		this.lsvData.AllColumns.Add(this.olvEntregxBairro);
		this.lsvData.AllColumns.Add(this.olvEntregCEP);
		this.lsvData.AllColumns.Add(this.olvRetiraID);
		this.lsvData.AllColumns.Add(this.olvRetiraNome);
		this.lsvData.AllColumns.Add(this.olvRetiraIE);
		this.lsvData.AllColumns.Add(this.olvRetiracMun);
		this.lsvData.AllColumns.Add(this.olvRetiraxMun);
		this.lsvData.AllColumns.Add(this.olvRetiraUF);
		this.lsvData.AllColumns.Add(this.olvRetiraxLgr);
		this.lsvData.AllColumns.Add(this.olvRetiraNro);
		this.lsvData.AllColumns.Add(this.olvRetiraxBairro);
		this.lsvData.AllColumns.Add(this.olvRetiraCEP);
		this.lsvData.AllColumns.Add(this.olvTrnPlaca);
		this.lsvData.AllColumns.Add(this.olvTrnUf);
		this.lsvData.AllColumns.Add(this.olvTrnRNTC);
		this.lsvData.AllColumns.Add(this.olvVolQuant);
		this.lsvData.AllColumns.Add(this.olvVolEspec);
		this.lsvData.AllColumns.Add(this.olvVolMarca);
		this.lsvData.AllColumns.Add(this.olvVolNumer);
		this.lsvData.AllColumns.Add(this.olvVolPesoL);
		this.lsvData.AllColumns.Add(this.olvVolPesoB);
		this.lsvData.AllColumns.Add(this.olvInfCpl);
		this.lsvData.AllColumns.Add(this.olvInfAdFisco);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvError);
		this.lsvData.AllColumns.Add(this.olvBatchNum);
		this.lsvData.AllColumns.Add(this.olvCompranEmp);
		this.lsvData.AllColumns.Add(this.olvCompraxPed);
		this.lsvData.AllColumns.Add(this.olvCompraxCont);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvBaseIcmsOutra);
		this.lsvData.AllColumns.Add(this.olvValorIcmsOutra);
		this.lsvData.AllColumns.Add(this.olvTaxaIcmsOutra);
		this.lsvData.AllColumns.Add(this.olvIndPres);
		this.lsvData.AllColumns.Add(this.olvIndPag);
		this.lsvData.AllColumns.Add(this.olvTPag);
		this.lsvData.AllColumns.Add(this.olvVPag);
		this.lsvData.AllColumns.Add(this.olvIndIntermed);
		this.lsvData.AllColumns.Add(this.olvTagDtHr);
		this.lsvData.AllColumns.Add(this.olvTagUser);
		this.lsvData.AllColumns.Add(this.olvDocNoteDtHr);
		this.lsvData.AllColumns.Add(this.olvDocNoteUser);
		this.lsvData.AllColumns.Add(this.olvVersion);
		this.lsvData.AllColumns.Add(this.olvModFrete);
		this.lsvData.AllColumns.Add(this.olvObsCont);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[122]
		{
			this.olvSelect, this.olvEmitida, this.olvHasXml, this.olvNum, this.olvSerie, this.olvTipo, this.olvTag, this.olvDtAut, this.olvDtEmi, this.olvHrEmi,
			this.olvValor, this.olvVenc, this.olvDtEntFisc, this.olvDtRegFisc, this.olvUsRegFisc, this.olvDcNumFisc, this.olvStatus, this.olvEmitID, this.olvEmitNome, this.olvManifest,
			this.olvCancel, this.olvHasEvent, this.olvHasCTeEvent, this.olvMDFeEvtData, this.olvSufVistEvtData, this.olvSufVistInteData, this.olvMotivo, this.olvEstado, this.olvChave, this.olvFilial,
			this.olvTpDoc, this.olvNatOper, this.olvIndSimplesNac, this.olvCFOP, this.olvNCM, this.olvBaseICMS, this.olvTaxaICMS, this.olvValorICMS, this.olvCdCstICMS, this.olvVlrICMSDeson,
			this.olvVlrBCST, this.olvVlrTotST, this.olvVlrTotProd, this.olvVlrTotFrete, this.olvVlrTotSeg, this.olvVlrTotDesc, this.olvVlrTotOutro, this.olvVlrTotII, this.olvVlrTotIPI, this.olvVlrTotIPIDevol,
			this.olvVlrTotPIS, this.olvVlrTotCOFINS, this.olvVlrTotTrib, this.olvVlrTotFCP, this.olvVlrTotFCPUFDest, this.olvVlrTotFCPST, this.olvVlrTotFCPSTRet, this.olvVlrTotICMSUFRemet, this.olvVlrTotICMSUFDest, this.olvEmitIE,
			this.olvEmitcMun, this.olvEmitxMun, this.olvEmitUf, this.olvDestinId, this.olvDestinIE, this.olvDestinNome, this.olvDestincMun, this.olvDestinxMun, this.olvDestinUf, this.olvDestinIndIE,
			this.olvTranspId, this.olvTranspIE, this.olvTranspNome, this.olvTranspcMun, this.olvTranspxMun, this.olvTranspUf, this.olvEntregID, this.olvEntregNome, this.olvEntregIE, this.olvEntregcMun,
			this.olvEntregxMun, this.olvEntregUF, this.olvEntregxLgr, this.olvEntregNro, this.olvEntregxBairro, this.olvEntregCEP, this.olvRetiraID, this.olvRetiraNome, this.olvRetiraIE, this.olvRetiracMun,
			this.olvRetiraxMun, this.olvRetiraUF, this.olvRetiraxLgr, this.olvRetiraNro, this.olvRetiraxBairro, this.olvRetiraCEP, this.olvTrnPlaca, this.olvTrnUf, this.olvTrnRNTC, this.olvVolQuant,
			this.olvVolEspec, this.olvVolMarca, this.olvVolNumer, this.olvVolPesoL, this.olvVolPesoB, this.olvInfCpl, this.olvInfAdFisco, this.olvAnoMes, this.olvError, this.olvBatchNum,
			this.olvCompranEmp, this.olvCompraxPed, this.olvCompraxCont, this.olvDocNote, this.olvTaxaIcmsOutra, this.olvTagDtHr, this.olvTagUser, this.olvDocNoteDtHr, this.olvDocNoteUser, this.olvVersion,
			this.olvModFrete, this.olvObsCont
		});
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lsvData.FullRowSelect = true;
		this.lsvData.HideSelection = false;
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
		this.olvEmitida.AspectName = "Emitida";
		this.olvEmitida.Text = "Emissor";
		this.olvEmitida.ToolTipText = "Emitida";
		this.olvHasXml.AspectName = "HasXml";
		this.olvHasXml.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.IsEditable = false;
		this.olvHasXml.Searchable = false;
		this.olvHasXml.Text = "XML";
		this.olvHasXml.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.ToolTipText = "Tem XML vinculado";
		this.olvHasXml.UseFiltering = false;
		this.olvHasXml.Width = 36;
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
		this.olvSerie.ToolTipText = "Série do Documento Fiscal";
		this.olvSerie.Width = 44;
		this.olvTipo.AspectName = "Model";
		this.olvTipo.Text = "Tipo";
		this.olvTipo.ToolTipText = "Modelo do Documento Fiscal";
		this.olvTipo.Width = 43;
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.ToolTipText = "Etiqueta atribuída";
		this.olvTag.Width = 29;
		this.olvDtAut.AspectName = "DtAut";
		this.olvDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.Text = "DtAut";
		this.olvDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.ToolTipText = "Data de autorização";
		this.olvDtAut.Width = 71;
		this.olvDtEmi.AspectName = "DtEmi";
		this.olvDtEmi.Text = "DtEmi";
		this.olvDtEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmi.ToolTipText = "Data de emissão do documento fiscal";
		this.olvDtEmi.Width = 71;
		this.olvHrEmi.AspectName = "HrEmi";
		this.olvHrEmi.Text = "HrEmi";
		this.olvHrEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHrEmi.ToolTipText = "Hora de emissão do documento Fiscal";
		this.olvHrEmi.Width = 71;
		this.olvValor.AspectName = "Valor";
		this.olvValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValor.Text = "Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.ToolTipText = "Valor do documento fiscal";
		this.olvValor.Width = 80;
		this.olvVenc.AspectName = "DVenc";
		this.olvVenc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVenc.Text = "Venc.";
		this.olvVenc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVenc.ToolTipText = "Data de vencimento";
		this.olvVenc.Width = 71;
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
		this.olvEmitID.AspectName = "EmitID";
		this.olvEmitID.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitID.Text = "Emissor CNPJ/CPF";
		this.olvEmitID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitID.ToolTipText = "CNPJ/CPF do emitente";
		this.olvEmitID.Width = 120;
		this.olvEmitNome.AspectName = "EmitNome";
		this.olvEmitNome.Text = "Emissor Nome";
		this.olvEmitNome.ToolTipText = "Nome do emitente";
		this.olvEmitNome.Width = 200;
		this.olvManifest.Text = "Manifestação";
		this.olvManifest.ToolTipText = "Manifestação do destinatário";
		this.olvCancel.AspectName = "Canceled";
		this.olvCancel.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.Text = "Cancelados";
		this.olvCancel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.ToolTipText = "Cancelado";
		this.olvCancel.Width = 80;
		this.olvHasEvent.AspectName = "HasEvent";
		this.olvHasEvent.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasEvent.Text = "Eventos";
		this.olvHasEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasEvent.ToolTipText = "Tem evento vinculado";
		this.olvHasEvent.Width = 80;
		this.olvHasCTeEvent.AspectName = "HasCTeEvent";
		this.olvHasCTeEvent.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasCTeEvent.Text = "CTe";
		this.olvHasCTeEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasCTeEvent.ToolTipText = "Tem evento de Cte vinculado";
		this.olvHasCTeEvent.Width = 35;
		this.olvMDFeEvtData.AspectName = "MDFeEvtData";
		this.olvMDFeEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.Text = "MDFe";
		this.olvMDFeEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.ToolTipText = "Data do evento do manifesto eletrônico de documentos";
		this.olvMDFeEvtData.Width = 76;
		this.olvSufVistEvtData.AspectName = "SufVistEvtData";
		this.olvSufVistEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistEvtData.Text = "Data da Vistoria Suframa";
		this.olvSufVistEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistEvtData.ToolTipText = "Data do evento de vistoria na Suframa";
		this.olvSufVistEvtData.Width = 100;
		this.olvSufVistInteData.AspectName = "SufInteEvtData";
		this.olvSufVistInteData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistInteData.Text = "Data da Inter. na Suframa";
		this.olvSufVistInteData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistInteData.ToolTipText = "Data do evento de internalização na Sufram";
		this.olvSufVistInteData.Width = 100;
		this.olvMotivo.AspectName = "xMotivo";
		this.olvMotivo.Text = "Status";
		this.olvMotivo.ToolTipText = "Motivo";
		this.olvMotivo.Width = 200;
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do Documento Fiscal";
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
		this.olvTpDoc.ToolTipText = "Tipo de documento";
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "Natureza";
		this.olvNatOper.ToolTipText = "Natureza da operação";
		this.olvIndSimplesNac.AspectName = "IndSimplesNac";
		this.olvIndSimplesNac.Text = "Simples";
		this.olvIndSimplesNac.ToolTipText = "Indica se o contribuinte é simples nacional";
		this.olvCFOP.AspectName = "CFOPList";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.ToolTipText = "Código Fiscal de Operações e Prestações";
		this.olvCFOP.Width = 90;
		this.olvNCM.AspectName = "NCMList";
		this.olvNCM.Text = "NCM";
		this.olvNCM.ToolTipText = "Nomenclatura Comum do Mercosul";
		this.olvNCM.Width = 90;
		this.olvBaseICMS.AspectName = "BaseICMS";
		this.olvBaseICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseICMS.Text = "ICMS Base";
		this.olvBaseICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseICMS.ToolTipText = "Base ICMS";
		this.olvBaseICMS.Width = 100;
		this.olvTaxaICMS.AspectName = "TaxaICMS";
		this.olvTaxaICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTaxaICMS.Text = "ICMS Taxa";
		this.olvTaxaICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvTaxaICMS.ToolTipText = "Taxa ICMS";
		this.olvTaxaICMS.Width = 100;
		this.olvValorICMS.AspectName = "ValorICMS";
		this.olvValorICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorICMS.Text = "ICMS Valor";
		this.olvValorICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorICMS.ToolTipText = "Valor do ICMS";
		this.olvValorICMS.Width = 100;
		this.olvCdCstICMS.AspectName = "CdCstICMS";
		this.olvCdCstICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMS.Text = "ICMS CST";
		this.olvCdCstICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMS.ToolTipText = "Código de situação tributária do ICMS";
		this.olvCdCstICMS.Width = 100;
		this.olvVlrICMSDeson.AspectName = "VlrICMSDeson";
		this.olvVlrICMSDeson.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrICMSDeson.Text = "ICMS Desonerado";
		this.olvVlrICMSDeson.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrICMSDeson.ToolTipText = "Valor Total do ICMS desonerado";
		this.olvVlrICMSDeson.Width = 100;
		this.olvVlrBCST.AspectName = "VlrBCST";
		this.olvVlrBCST.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrBCST.Text = "ICMS ST Base";
		this.olvVlrBCST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrBCST.ToolTipText = "Base de Cálculo do ICMS ST";
		this.olvVlrBCST.Width = 100;
		this.olvVlrTotST.AspectName = "VlrTotST";
		this.olvVlrTotST.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotST.Text = "ICMS ST Valor";
		this.olvVlrTotST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotST.ToolTipText = "Valor Total do ICMS ST";
		this.olvVlrTotST.Width = 100;
		this.olvVlrTotProd.AspectName = "VlrTotProd";
		this.olvVlrTotProd.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotProd.Text = "Valor Prod";
		this.olvVlrTotProd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotProd.ToolTipText = "Valor Total dos produtos e serviços";
		this.olvVlrTotProd.Width = 100;
		this.olvVlrTotFrete.AspectName = "VlrTotFrete";
		this.olvVlrTotFrete.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotFrete.Text = "Valor Frete";
		this.olvVlrTotFrete.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotFrete.ToolTipText = "Valor Total do Frete";
		this.olvVlrTotFrete.Width = 100;
		this.olvVlrTotSeg.AspectName = "VlrTotSeg";
		this.olvVlrTotSeg.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotSeg.Text = "Valor Seguro";
		this.olvVlrTotSeg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotSeg.ToolTipText = "Valor Total do Seguro";
		this.olvVlrTotSeg.Width = 100;
		this.olvVlrTotDesc.AspectName = "VlrTotDesc";
		this.olvVlrTotDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotDesc.Text = "Valor Desc";
		this.olvVlrTotDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotDesc.ToolTipText = "Valor Total do Desconto";
		this.olvVlrTotDesc.Width = 100;
		this.olvVlrTotOutro.AspectName = "VlrTotOutro";
		this.olvVlrTotOutro.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotOutro.Text = "Valor Outros";
		this.olvVlrTotOutro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotOutro.ToolTipText = "Outras Despesas acessórias";
		this.olvVlrTotOutro.Width = 100;
		this.olvVlrTotII.AspectName = "VlrTotII";
		this.olvVlrTotII.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotII.Text = "II Valor";
		this.olvVlrTotII.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotII.ToolTipText = "Valor Total do II";
		this.olvVlrTotII.Width = 100;
		this.olvVlrTotIPI.AspectName = "VlrTotIPI";
		this.olvVlrTotIPI.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotIPI.Text = "IPI Valor";
		this.olvVlrTotIPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotIPI.ToolTipText = "Valor Total do IPI";
		this.olvVlrTotIPI.Width = 100;
		this.olvVlrTotIPIDevol.AspectName = "VlrTotIPIDevol";
		this.olvVlrTotIPIDevol.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotIPIDevol.Text = "IPI Valor Dev.";
		this.olvVlrTotIPIDevol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotIPIDevol.ToolTipText = "Valor Total do IPI devolvido";
		this.olvVlrTotIPIDevol.Width = 100;
		this.olvVlrTotPIS.AspectName = "VlrTotPIS";
		this.olvVlrTotPIS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotPIS.Text = "PIS Valor";
		this.olvVlrTotPIS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotPIS.ToolTipText = "Valor Total do PIS";
		this.olvVlrTotPIS.Width = 100;
		this.olvVlrTotCOFINS.AspectName = "VlrTotCOFINS";
		this.olvVlrTotCOFINS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVlrTotCOFINS.Text = "COFINS Valor";
		this.olvVlrTotCOFINS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotCOFINS.ToolTipText = "Valor Total do COFINS";
		this.olvVlrTotCOFINS.Width = 100;
		this.olvVlrTotTrib.AspectName = "VlrTotTrib";
		this.olvVlrTotTrib.Text = "Valor Apr Tributos";
		this.olvVlrTotTrib.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotTrib.ToolTipText = "Valor aproximado total de tributos federais, estaduais e municipais";
		this.olvVlrTotTrib.Width = 100;
		this.olvVlrTotFCP.AspectName = "VlrTotFCP";
		this.olvVlrTotFCP.Text = "FCP Valor";
		this.olvVlrTotFCP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotFCP.ToolTipText = "Valor total do fundo de combate à pobreza";
		this.olvVlrTotFCP.Width = 100;
		this.olvVlrTotFCPUFDest.AspectName = "VlrTotFCPUFDest";
		this.olvVlrTotFCPUFDest.Text = "FCP Dest Valor";
		this.olvVlrTotFCPUFDest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotFCPUFDest.ToolTipText = "Valor total do ICMS relativo ao fundo de combate à pobreza";
		this.olvVlrTotFCPUFDest.Width = 100;
		this.olvVlrTotFCPST.AspectName = "VlrTotFCPST";
		this.olvVlrTotFCPST.Text = "FCP ST Valor";
		this.olvVlrTotFCPST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotFCPST.ToolTipText = "Valor total do FCP retido por substituição tributária";
		this.olvVlrTotFCPST.Width = 100;
		this.olvVlrTotFCPSTRet.AspectName = "VlrTotFCPSTRet";
		this.olvVlrTotFCPSTRet.Text = "FCP ST Retido Valor";
		this.olvVlrTotFCPSTRet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotFCPSTRet.Width = 100;
		this.olvVlrTotICMSUFRemet.AspectName = "VlrTotICMSUFRemet";
		this.olvVlrTotICMSUFRemet.Text = "ICMS UF Remetente";
		this.olvVlrTotICMSUFRemet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotICMSUFRemet.ToolTipText = "Valor do ICMS interestadual para a UF do remetente";
		this.olvVlrTotICMSUFRemet.Width = 100;
		this.olvVlrTotICMSUFDest.AspectName = "VlrTotICMSUFDest";
		this.olvVlrTotICMSUFDest.Text = "ICMS UF Destino";
		this.olvVlrTotICMSUFDest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVlrTotICMSUFDest.ToolTipText = "Valor do ICMS interestadual para a UF de destino";
		this.olvVlrTotICMSUFDest.Width = 100;
		this.olvEmitIE.AspectName = "EmitIE";
		this.olvEmitIE.Text = "Emissor IE";
		this.olvEmitIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitIE.ToolTipText = "IE do emitente";
		this.olvEmitcMun.AspectName = "EmitcMun";
		this.olvEmitcMun.Text = "Emissor Mun Cod";
		this.olvEmitcMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitcMun.ToolTipText = "Código do municipío";
		this.olvEmitxMun.AspectName = "EmitxMun";
		this.olvEmitxMun.Text = "Emissor Mun Desc";
		this.olvEmitxMun.ToolTipText = "Nome do municipío";
		this.olvEmitUf.AspectName = "EmitUf";
		this.olvEmitUf.Text = "Emissor Uf";
		this.olvEmitUf.ToolTipText = "Sigla da UF";
		this.olvDestinId.AspectName = "DestinID";
		this.olvDestinId.Text = "Destinatário CNPJ/CPF";
		this.olvDestinId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinId.ToolTipText = "CNPJ/CPF do destinatário";
		this.olvDestinId.Width = 130;
		this.olvDestinIE.AspectName = "DestinIE";
		this.olvDestinIE.Text = "Destinatário IE";
		this.olvDestinIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinIE.ToolTipText = "Inscrição estadual do destinatário";
		this.olvDestinNome.AspectName = "DestinNome";
		this.olvDestinNome.Text = "Destinatário Nome";
		this.olvDestinNome.ToolTipText = "Nome do destinatário";
		this.olvDestinNome.Width = 230;
		this.olvDestincMun.AspectName = "DestincMun";
		this.olvDestincMun.Text = "Destinatário Mun Cod";
		this.olvDestincMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestincMun.ToolTipText = "Código do municipío do destino";
		this.olvDestinxMun.AspectName = "DestinxMun";
		this.olvDestinxMun.Text = "Destinatário Mun Desc";
		this.olvDestinxMun.ToolTipText = "Nome do municipío de destino";
		this.olvDestinUf.AspectName = "DestinUf";
		this.olvDestinUf.Text = "Destinatário Uf";
		this.olvDestinUf.ToolTipText = "Sigla da UF do destino";
		this.olvDestinIndIE.AspectName = "DestinIndIE";
		this.olvDestinIndIE.Text = "Destinatário Indicador da IE";
		this.olvDestinIndIE.ToolTipText = "Indicador da IE do Destinatário";
		this.olvDestinCEP.AspectName = "DestinCEP";
		this.olvDestinCEP.IsVisible = false;
		this.olvDestinCEP.Text = "Destinatário CEP";
		this.olvDestinCEP.ToolTipText = "Código do CEP ";
		this.olvDestinCEP.Width = 100;
		this.olvDestinBairro.AspectName = "DestinBairro";
		this.olvDestinBairro.DisplayIndex = 93;
		this.olvDestinBairro.IsVisible = false;
		this.olvDestinBairro.Text = "Destinatário Bairro";
		this.olvDestinBairro.ToolTipText = "Bairro";
		this.olvDestinBairro.Width = 100;
		this.olvTranspId.AspectName = "TranspID";
		this.olvTranspId.Text = "Transportador CNPJ/CPF";
		this.olvTranspId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTranspId.ToolTipText = "CNPJ/CPF do transportador";
		this.olvTranspId.Width = 130;
		this.olvTranspIE.AspectName = "TranspIE";
		this.olvTranspIE.Text = "Transportador IE";
		this.olvTranspIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTranspIE.ToolTipText = "Inscrição estadual do transportador ";
		this.olvTranspNome.AspectName = "TranspNome";
		this.olvTranspNome.Text = "Transportador Nome";
		this.olvTranspNome.ToolTipText = "Nome do transportador";
		this.olvTranspNome.Width = 230;
		this.olvTranspcMun.AspectName = "TranspcMun";
		this.olvTranspcMun.Text = "Transportador Mun Cod";
		this.olvTranspcMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTranspcMun.ToolTipText = "Codigo do municipío do transportador";
		this.olvTranspxMun.AspectName = "TranspxMun";
		this.olvTranspxMun.Text = "Transportador Mun Desc";
		this.olvTranspxMun.ToolTipText = "Nome do municipío do transportador";
		this.olvTranspUf.AspectName = "TranspUf";
		this.olvTranspUf.Text = "Transportador Uf";
		this.olvTranspUf.ToolTipText = "Sigla da UF do transportador";
		this.olvEntregID.AspectName = "EntregID";
		this.olvEntregID.Text = "Entrega CNPJ/CPF";
		this.olvEntregID.ToolTipText = "Entrega CNPJ/CPF";
		this.olvEntregNome.AspectName = "EntregNome";
		this.olvEntregNome.Text = "Entrega Nome";
		this.olvEntregNome.ToolTipText = "Razão social ou nome do entregador";
		this.olvEntregIE.AspectName = "EntregIE";
		this.olvEntregIE.Text = "Entrega IE";
		this.olvEntregIE.ToolTipText = "Inscrição Estadual do Estabelecimento Recebedor";
		this.olvEntregcMun.AspectName = "EntregcMun";
		this.olvEntregcMun.Text = "Entrega Cod Municipio";
		this.olvEntregcMun.ToolTipText = "Entrega Código Municipio";
		this.olvEntregxMun.AspectName = "EntregxMun";
		this.olvEntregxMun.Text = "Entrega Municipio";
		this.olvEntregxMun.ToolTipText = "Entrega Municipio";
		this.olvEntregUF.AspectName = "EntregUF";
		this.olvEntregUF.Text = "Entrega UF";
		this.olvEntregUF.ToolTipText = "Entrega UF";
		this.olvEntregxLgr.Text = "Entrega Logradouro";
		this.olvEntregxLgr.ToolTipText = "Entrega Logradouro";
		this.olvEntregNro.Text = "Entrega Número";
		this.olvEntregNro.ToolTipText = "Entrega Número";
		this.olvEntregxBairro.Text = "Entrega Bairro";
		this.olvEntregxBairro.ToolTipText = "Entrega Bairro";
		this.olvEntregCEP.Text = "Entrega CEP";
		this.olvEntregCEP.ToolTipText = "Entrega CEP";
		this.olvRetiraID.AspectName = "RetiraID";
		this.olvRetiraID.Text = "Retirada CNPJ/CPF";
		this.olvRetiraID.ToolTipText = "Retirada CNPJ/CPF";
		this.olvRetiraNome.AspectName = "RetiraNome";
		this.olvRetiraNome.Text = "Retirada Nome";
		this.olvRetiraNome.ToolTipText = "Razão social ou nome do Retiradador";
		this.olvRetiraIE.AspectName = "RetiraIE";
		this.olvRetiraIE.DisplayIndex = 87;
		this.olvRetiraIE.Text = "Retirada IE";
		this.olvRetiraIE.ToolTipText = "Inscrição Estadual do Estabelecimento Recebedor";
		this.olvRetiracMun.AspectName = "RetiracMun";
		this.olvRetiracMun.DisplayIndex = 84;
		this.olvRetiracMun.Text = "Retirada Cod Municipio";
		this.olvRetiracMun.ToolTipText = "Retirada Código Municipio";
		this.olvRetiraxMun.AspectName = "RetiraxMun";
		this.olvRetiraxMun.DisplayIndex = 85;
		this.olvRetiraxMun.Text = "Retirada Municipio";
		this.olvRetiraxMun.ToolTipText = "Retirada Municipio";
		this.olvRetiraUF.AspectName = "RetiraUF";
		this.olvRetiraUF.DisplayIndex = 86;
		this.olvRetiraUF.Text = "Retirada UF";
		this.olvRetiraUF.ToolTipText = "Retirada UF";
		this.olvRetiraxLgr.Text = "Retirada Logradouro";
		this.olvRetiraxLgr.ToolTipText = "Retirada Logradouro";
		this.olvRetiraNro.Text = "Retirada Número";
		this.olvRetiraNro.ToolTipText = "Retirada Número";
		this.olvRetiraxBairro.Text = "Retirada Bairro";
		this.olvRetiraxBairro.ToolTipText = "Retirada Bairro";
		this.olvRetiraCEP.Text = "Retirada CEP";
		this.olvRetiraCEP.ToolTipText = "Retirada CEP";
		this.olvTrnPlaca.AspectName = "TrnPlaca";
		this.olvTrnPlaca.Text = "Veic.Placa";
		this.olvTrnPlaca.ToolTipText = "Placa do Veículo";
		this.olvTrnPlaca.Width = 100;
		this.olvTrnUf.AspectName = "TrnUf";
		this.olvTrnUf.Text = "Veic.Uf";
		this.olvTrnUf.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTrnUf.ToolTipText = "Sigla da UF do Veículo";
		this.olvTrnUf.Width = 100;
		this.olvTrnRNTC.AspectName = "TrnRNTC";
		this.olvTrnRNTC.Text = "Veic.RNTC";
		this.olvTrnRNTC.ToolTipText = "Registro Nacional de Transportador de Carga(ANTT)";
		this.olvTrnRNTC.Width = 100;
		this.olvVolQuant.AspectName = "VolQuant";
		this.olvVolQuant.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVolQuant.Text = "Vol.Quant";
		this.olvVolQuant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVolQuant.ToolTipText = "Quantidade de volumes transportados";
		this.olvVolQuant.Width = 100;
		this.olvVolEspec.AspectName = "VolEspec";
		this.olvVolEspec.Text = "Vol.Especie";
		this.olvVolEspec.ToolTipText = "Espécie dos volumes transportados";
		this.olvVolEspec.Width = 100;
		this.olvVolMarca.AspectName = "VolMarca";
		this.olvVolMarca.Text = "Vol.Marca";
		this.olvVolMarca.ToolTipText = "Marca dos volumes transportados ";
		this.olvVolMarca.Width = 100;
		this.olvVolNumer.AspectName = "VolNumer";
		this.olvVolNumer.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVolNumer.Text = "Vol.Num";
		this.olvVolNumer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVolNumer.ToolTipText = "Numeração dos volumes transportados";
		this.olvVolNumer.Width = 100;
		this.olvVolPesoL.AspectName = "VolPesoL";
		this.olvVolPesoL.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVolPesoL.Text = "Vol.PesoLiquido";
		this.olvVolPesoL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVolPesoL.ToolTipText = "Peso Líquido (em kg)";
		this.olvVolPesoL.Width = 100;
		this.olvVolPesoB.AspectName = "VolPesoB";
		this.olvVolPesoB.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVolPesoB.Text = "Vol.PesoBruto";
		this.olvVolPesoB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvVolPesoB.ToolTipText = "Peso Bruto (em kg) ";
		this.olvVolPesoB.Width = 100;
		this.olvInfCpl.Text = "Informações complementares";
		this.olvInfCpl.ToolTipText = "Informações complementares";
		this.olvInfCpl.Width = 150;
		this.olvInfAdFisco.Text = "Informações adicionais fisco";
		this.olvInfAdFisco.ToolTipText = "Informações adicionais fisco";
		this.olvInfAdFisco.Width = 150;
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Data de autorização";
		this.olvError.AspectName = "XmlError";
		this.olvError.Text = "Validação";
		this.olvError.ToolTipText = "Status de validação do XML";
		this.olvBatchNum.AspectName = "BatchNum";
		this.olvBatchNum.Text = "Lote Proc.";
		this.olvBatchNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBatchNum.ToolTipText = "Número do lote de processamento";
		this.olvCompranEmp.AspectName = "CompranEmp";
		this.olvCompranEmp.Text = "Nota de Empenho";
		this.olvCompranEmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCompranEmp.ToolTipText = "Nota de empenho de compras";
		this.olvCompranEmp.Width = 100;
		this.olvCompraxPed.AspectName = "CompraxPed";
		this.olvCompraxPed.Text = "Pedido de Compra";
		this.olvCompraxPed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCompraxPed.ToolTipText = "Pedido de compras";
		this.olvCompraxPed.Width = 100;
		this.olvCompraxCont.AspectName = "CompraxCont";
		this.olvCompraxCont.Text = "Contrato de Compra";
		this.olvCompraxCont.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCompraxCont.ToolTipText = "Contrato de compras";
		this.olvCompraxCont.Width = 100;
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvDocNote.ToolTipText = "Comentários";
		this.olvBaseIcmsOutra.AspectName = "BaseICMSOutraUF";
		this.olvBaseIcmsOutra.IsVisible = false;
		this.olvBaseIcmsOutra.Text = "Base ICMS Outra UF";
		this.olvBaseIcmsOutra.ToolTipText = "Base ICMS outra";
		this.olvValorIcmsOutra.AspectName = "ValorICMSOutraUF";
		this.olvValorIcmsOutra.IsVisible = false;
		this.olvValorIcmsOutra.Text = "Valor ICMS Outra UF";
		this.olvValorIcmsOutra.ToolTipText = "Valor ICMS outra";
		this.olvTaxaIcmsOutra.AspectName = "TaxaICMSOutraUF";
		this.olvTaxaIcmsOutra.Text = "Taxa ICMS Outra UF";
		this.olvTaxaIcmsOutra.ToolTipText = "Taxa ICMS outra";
		this.olvIndPres.AspectName = "indPres";
		this.olvIndPres.IsVisible = false;
		this.olvIndPres.Text = "Indicador de Presença";
		this.olvIndPres.ToolTipText = "Indicador de presença do comprador no estabelecimento";
		this.olvIndPres.Width = 100;
		this.olvIndPag.AspectName = "indPag";
		this.olvIndPag.IsVisible = false;
		this.olvIndPag.Text = "Indicador de Pagamento";
		this.olvIndPag.ToolTipText = "Indicador da forma de pagamento";
		this.olvIndPag.Width = 100;
		this.olvTPag.AspectName = "tPag";
		this.olvTPag.IsVisible = false;
		this.olvTPag.Text = "Meio Pagamento";
		this.olvTPag.ToolTipText = "Meio de pagamento";
		this.olvTPag.Width = 100;
		this.olvVPag.AspectName = "vPag";
		this.olvVPag.IsVisible = false;
		this.olvVPag.Text = "Valor Pagamento";
		this.olvVPag.ToolTipText = "Valor do Pagamento";
		this.olvVPag.Width = 90;
		this.olvIndIntermed.AspectName = "indIntermed";
		this.olvIndIntermed.DisplayIndex = 93;
		this.olvIndIntermed.IsVisible = false;
		this.olvIndIntermed.Text = "Indicador de Intermediador";
		this.olvIndIntermed.ToolTipText = "Indicador de intermediador/marketplace";
		this.olvIndIntermed.Width = 100;
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
		this.olvVersion.ToolTipText = "Versão do arquivo XML";
		this.olvModFrete.AspectName = "ModFrete";
		this.olvModFrete.Text = "Modalidade de Frete";
		this.olvModFrete.ToolTipText = "Modalidade do Frete";
		this.olvObsCont.Text = "Observações contribuinte";
		this.olvObsCont.Width = 150;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(784, 445);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocNFe";
		this.Text = "NFe: Dados Sintéticos";
		base.Load += new System.EventHandler(frmTabDocNFe_Load);
		this.pnContent.ResumeLayout(false);
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		base.ResumeLayout(false);
	}
}
