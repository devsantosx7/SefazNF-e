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
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabDocCTeDet : Form
{
	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsSrvTabDocCTeDet _SqlTabData = new clsSrvTabDocCTeDet();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

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

	private string _InfoAddXml = string.Empty;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private FastObjectListView lsvData;

	private ImageList ImageListDocs;

	private OLVColumn olvSelect;

	private OLVColumn olvHasXml;

	private OLVColumn olvTipo;

	private OLVColumn olvTag;

	private OLVColumn olvNum;

	private OLVColumn olvSerie;

	private OLVColumn olvDtAut;

	private OLVColumn olvValor;

	private OLVColumn olvCTeModFrete;

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

	private OLVColumn olvBaseIcmsOutra;

	private OLVColumn olvValorIcmsOutra;

	private OLVColumn olvTaxaICMSOutraUF;

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

	private OLVColumn olvAnoMes;

	private OLVColumn olvStatus;

	private OLVColumn olvDFeTipo;

	private OLVColumn olvDFeChave;

	private OLVColumn olvDFeNum;

	private OLVColumn olvDFeSerie;

	private OLVColumn olvDFeValor;

	private OLVColumn olvDFecUF;

	private OLVColumn olvDFeDtAut;

	private OLVColumn olvDFeDtEmi;

	private OLVColumn olvDFeDVenc;

	private OLVColumn olvDFexMotivo;

	private OLVColumn olvDFeTpDoc;

	private OLVColumn olvDFeNatOper;

	private OLVColumn olvDFeCFOPList;

	private OLVColumn olvDFeEmitId;

	private OLVColumn olvDFeEmitNome;

	private OLVColumn olvDFeEmitIE;

	private OLVColumn olvDFeEmitCmun;

	private OLVColumn olvDFeEmitXmun;

	private OLVColumn olvDFeEmitUf;

	private OLVColumn olvDFeTomaId;

	private OLVColumn olvDFeTomaNome;

	private OLVColumn olvDFeTomaIE;

	private OLVColumn olvDFeTomaCmun;

	private OLVColumn olvDFeTomaXmun;

	private OLVColumn olvDFeTomaUf;

	private OLVColumn olvDFeDestinId;

	private OLVColumn olvDFeDestinNome;

	private OLVColumn olvDFeDestinIE;

	private OLVColumn olvDFeDestinCmun;

	private OLVColumn olvDFeDestinXmun;

	private OLVColumn olvDFeDestinUf;

	private OLVColumn olvDFeTranspId;

	private OLVColumn olvDFeTranspNome;

	private OLVColumn olvDFeTranspIE;

	private OLVColumn olvDFeTranspCmun;

	private OLVColumn olvDFeTranspXmun;

	private OLVColumn olvDFeTranspUf;

	private OLVColumn olvDFeBaseICMS;

	private OLVColumn olvDFeValorICMS;

	private OLVColumn olvDFeVlrICMSDeson;

	private OLVColumn olvDFeVlrBCST;

	private OLVColumn olvDFeVlrTotST;

	private OLVColumn olvDFeVlrTotProd;

	private OLVColumn olvDFeVlrTotFrete;

	private OLVColumn olvDFeVlrTotSeg;

	private OLVColumn olvDFeVlrTotDesc;

	private OLVColumn olvDFeVlrTotII;

	private OLVColumn olvDFeVlrTotIPI;

	private OLVColumn olvDFeVlrTotIPIDevol;

	private OLVColumn olvDFeVlrTotPIS;

	private OLVColumn olvDFeVlrTotCOFINS;

	private OLVColumn olvDFeVlrTotOutro;

	private OLVColumn olvDFeVlrTotTrib;

	private OLVColumn olvDFeDtEntFisc;

	private OLVColumn olvDtEntFisc;

	private OLVColumn olvDocNote;

	private OLVColumn olvDFeDocNote;

	private OLVColumn olvDFeModFrete;

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

	private OLVColumn olvDFeVersion;

	private Button btSalesContact;

	private OLVColumn olvCdCstICMSUf;

	private OLVColumn olvBaseIcmsOutraUf;

	private OLVColumn olvValorIcmsOutraUf;

	private OLVColumn olvTaxaICMSOutra;

	private OLVColumn olvManifest;

	private OLVColumn olvFretePesoTransp;

	private OLVColumn olvFreteUnidadePeso;

	private OLVColumn olvVolPesoB;

	private OLVColumn olvDFeDestinCEP;

	private OLVColumn olvObsCont;

	private OLVColumn olvFretePesoBruto;

	private OLVColumn olvFretePesoBrtUn;

	private OLVColumn olvFretePesoLiqUn;

	private OLVColumn olvFretePesoLiquido;

	private OLVColumn olvInfAdFisco;

	private OLVColumn olvInfCpl;

	private OLVColumn olvFreteQuantidade;

	private OLVColumn olvFreteQuantidadeUn;

	private OLVColumn olvFreteVolTransp;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabDocCTeDet(string pTitle, Color pColumnColor, string pFeatExtId)
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

	private async void frmTabDocCTeDet_Load(object sender, EventArgs e)
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
			clsScreenGeral.funcCheckListViewDdic(typeof(DocDocLink), lsvData);
		}
		if (!_HasFiscalioConnect)
		{
			List<string> varColList = new List<string> { "DtEntFisc", "DtRegFisc", "UsRegFisc", "DcNumFisc" };
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList);
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
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(docDocLink, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
		};
		olvHasXml.AspectGetter = (object x) => string.Empty;
		olvNum.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : clsFunction.funcGetValue(docDocLink.Num).PadLeft(9, '0');
		};
		olvDFeNum.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : clsFunction.funcGetValue(docDocLink.DFeNum).PadLeft(9, '0');
		};
		olvTipo.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : clsFunction.funcGetDocType(docDocLink.Model);
		};
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : clsScreenGeral.funcGetDocStatIcon(docDocLink);
		};
		olvTag.ImageGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : ((object)clsScreenGeral.funcGetDocNotesIcon(docDocLink));
		};
		olvTag.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			if (docDocLink == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docDocLink.Tag) ? null : _TagHsList[docDocLink.Tag];
		};
		olvEmitCNPJ.AspectGetter = (object x) => ((DocDocLink)x)?.EmitID;
		olvEmitNome.AspectGetter = (object x) => ((DocDocLink)x)?.EmitNome;
		olvEstado.AspectGetter = delegate(object x)
		{
			DocDocLink varDocument = (DocDocLink)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
		};
		olvTpDoc.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetTipoDoc(docDocLink.Model, docDocLink.TpDoc);
		};
		olvTpServ.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetTpServ(docDocLink.Model, docDocLink.tpServ);
		};
		olvModal.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetModal(docDocLink.Model, docDocLink.Modal);
		};
		olvTomaIE.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : clsSrvGeral.funcGetIEFormat(docDocLink.TomaIE);
		};
		olvDFeTpDoc.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetTipoDoc(docDocLink.DFeModel, docDocLink.DFeTpDoc);
		};
		olvDescCstICMS.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetCteIcmsStDesc(docDocLink);
		};
		olvIndSimplesNac.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetIndSimples(docDocLink.Model, docDocLink.IndSimplesNac);
		};
		olvAnoMes.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			if (docDocLink == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = docDocLink.DtAut.Substring(0, 7);
			}
			catch
			{
			}
			return result;
		};
		olvCTeModFrete.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetCTeModFrete(docDocLink.ModFrete);
		};
		olvManifest.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.GetEventDesc(docDocLink.Model, docDocLink.ManifCode);
		};
		olvDFeModFrete.AspectGetter = delegate(object x)
		{
			DocDocLink docDocLink = (DocDocLink)x;
			return (docDocLink == null) ? null : _clsDFeCodes.funcGetNFeModFrete(docDocLink.DFeModel, docDocLink.DFeModFrete);
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
	}

	private void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		DocDocLink varDocument = (DocDocLink)e.Model;
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
		List<DocDocLink> varclsDocList = await new clsDataDocDocLink().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		foreach (DocDocLink varclsDocItem in varclsDocList)
		{
			if (clsFunction.IsEmpty(varclsDocItem.LinkRefKey))
			{
				continue;
			}
			varclsDocItem.DFeType = clsFunction.funcGetDocTypeFromKey(varclsDocItem.LinkRefKey);
			if (clsFunction.IsEmpty(varclsDocItem.DFeModel))
			{
				Document varclsDoc = _clsDataDoc.funcGetDocFromKey(varclsDocItem.Filial, varclsDocItem.LinkRefKey);
				if (varclsDoc == null)
				{
					varclsDoc = new Document();
				}
				varclsDocItem.DFeNum = varclsDoc.Num;
				varclsDocItem.DFeChave = varclsDoc.Chave;
				varclsDocItem.DFeSerie = varclsDoc.Serie;
				varclsDocItem.DFecUF = varclsDoc.cUF;
				varclsDocItem.DFeModel = varclsDoc.Model;
			}
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
		return await clsMonGeral.funcLoadTagsAsync(_HasColors, _TagHsList, contextMenuDocs, tsmCopyDocKey_Click, null, tsbTagCode_Click, tsbDocNote_Click, varclsActFiscReset, tsbRemoveDocNote_Click);
	}

	private async void tsbDocReset_Click(object sender, EventArgs e)
	{
		List<DocDocLink> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<DocDocLink> varObjList = funcGetObjList(pFocused, pChecked);
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
		List<DocDocLink> varObjList = funcGetObjList(pFocused, pChecked);
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
		List<DocDocLink> varObjList = funcGetObjList(pFocused, pChecked);
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
					if (column.Text == "CTe:Num")
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
		return await clsScreenGeral.funcExportDocToExcelAsync<DocDocLink>(lsvData, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<DocDocLink> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	public async Task<List<Document>> funcGetDocListAsync(List<DocDocLink> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (DocDocLink varObject in pObjList)
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

	private List<DocDocLink> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<DocDocLink> varDocList = new List<DocDocLink>();
		DocDocLink varFocused = new DocDocLink();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (DocDocLink)lsvData.GetItem(lsvData.FocusedItem.Index).RowObject;
				varDocList.Add(varFocused);
			}
			catch
			{
				varFocused = new DocDocLink();
			}
		}
		if (pChecked)
		{
			foreach (DocDocLink varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varDocList.Add(varObject);
				}
			}
			if (varDocList.Count > 0)
			{
				varDocList = lsvData.Objects.Cast<DocDocLink>().Intersect(varDocList).ToList();
			}
		}
		if (!pFocused && !pChecked)
		{
			varDocList = lsvData.FilteredObjects.Cast<DocDocLink>().ToList();
			if (varDocList.Count <= 0)
			{
				varDocList = lsvData.Objects.Cast<DocDocLink>().ToList();
			}
		}
		return varDocList;
	}

	private async void funcRefreshListAsync(List<Document> pDocList, List<DocDocLink> pObjList)
	{
		ConcurrentBag<ObjFieldBuffer> varDocFieldList = clsObjectBuffer.funcGet<Document>().Fields;
		ConcurrentBag<ObjFieldBuffer> varObjFieldList = clsObjectBuffer.funcGet<DocDocLink>().Fields;
		await Task.WhenAll(((IEnumerable<DocDocLink>)pObjList).Select((Func<DocDocLink, Task>)async delegate(DocDocLink varclsObjItem)
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

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<DocDocLink> varObjList = funcGetObjList(pFocused, pChecked);
		funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
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
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, null);
	}

	private async void funcShowDocViewerAsync(bool pShowPDF, bool pShowXML, Document pDocument)
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
			DocDocLink varDocument = (DocDocLink)e.Model;
			if (varDocument != null && !clsFunction.IsEmpty(varDocument.Tag))
			{
				string varColorName = (string)_HasColors[varDocument.Tag];
				e.SubItem.BackColor = ColorTranslator.FromHtml(varColorName);
			}
		}
	}

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		DocDocLink varDocument = (DocDocLink)e.Model;
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
		DocDocLink varDocDocitem = (DocDocLink)e.Model;
		if (varDocDocitem == null)
		{
			return;
		}
		if (e.ColumnIndex.Equals(olvNum.Index))
		{
			Document varDocCTe = await varDataHandler.funcGetItemByKeyAsync(varDocDocitem.Filial, varDocDocitem.Chave);
			if (varDocCTe != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varDocCTe);
			}
		}
		else if (e.ColumnIndex.Equals(olvDFeNum.Index))
		{
			Document varDocNFe = await varDataHandler.funcGetItemByKeyAsync(varDocDocitem.Filial, varDocDocitem.DFeChave);
			if (varDocNFe != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varDocNFe);
			}
		}
	}

	private async void lsvData_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode.Equals(Keys.C))
		{
			await funcCopyDocKeyAsync();
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
			List<DocDocLink> varObjList = varEnumList.Cast<DocDocLink>().ToList();
			if (varObjList == null)
			{
				varObjList = new List<DocDocLink>();
			}
			_TotalValue = varObjList.Sum((DocDocLink r) => clsFunction.funcConvStrToDec(r.Valor));
			EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
			varArguments.TotalValue = _TotalValue;
			varArguments.TotalQuant = varObjList.Count;
			OnEventTabManager(varArguments);
		}
	}

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipReportCTeAnalyticalAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocCTeDet));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvValor = new BrightIdeasSoftware.OLVColumn();
		this.olvCTeModFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvVenc = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEntFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvManifest = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitCNPJ = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitNome = new BrightIdeasSoftware.OLVColumn();
		this.olvCancel = new BrightIdeasSoftware.OLVColumn();
		this.olvHasEvent = new BrightIdeasSoftware.OLVColumn();
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
		this.olvCdCstICMSUf = new BrightIdeasSoftware.OLVColumn();
		this.olvBaseIcmsOutraUf = new BrightIdeasSoftware.OLVColumn();
		this.olvValorIcmsOutraUf = new BrightIdeasSoftware.OLVColumn();
		this.olvTaxaICMSOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvFretePesoTransp = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteUnidadePeso = new BrightIdeasSoftware.OLVColumn();
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
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvTagDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvTagUser = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteUser = new BrightIdeasSoftware.OLVColumn();
		this.olvInfAdFisco = new BrightIdeasSoftware.OLVColumn();
		this.olvInfCpl = new BrightIdeasSoftware.OLVColumn();
		this.olvObsCont = new BrightIdeasSoftware.OLVColumn();
		this.olvVersion = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeChave = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeNum = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeValor = new BrightIdeasSoftware.OLVColumn();
		this.olvDFecUF = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDtEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDVenc = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDtEntFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDFexMotivo = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeCFOPList = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeEmitId = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeEmitNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeEmitIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeEmitCmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeEmitXmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeEmitUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTomaId = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTomaNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTomaIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTomaCmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTomaXmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTomaUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinId = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinCmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinXmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTranspId = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTranspNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTranspIE = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTranspCmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTranspXmun = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeTranspUf = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeBaseICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeValorICMS = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrICMSDeson = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrBCST = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotST = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotProd = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotSeg = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvVolPesoB = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotII = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotIPI = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotIPIDevol = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotPIS = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotCOFINS = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotOutro = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVlrTotTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeModFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeVersion = new BrightIdeasSoftware.OLVColumn();
		this.olvDFeDestinCEP = new BrightIdeasSoftware.OLVColumn();
		this.olvFretePesoBruto = new BrightIdeasSoftware.OLVColumn();
		this.olvFretePesoBrtUn = new BrightIdeasSoftware.OLVColumn();
		this.olvFretePesoLiquido = new BrightIdeasSoftware.OLVColumn();
		this.olvFretePesoLiqUn = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteQuantidade = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteQuantidadeUn = new BrightIdeasSoftware.OLVColumn();
		this.olvFreteVolTransp = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.olvBaseIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvValorIcmsOutra = new BrightIdeasSoftware.OLVColumn();
		this.olvTaxaICMSOutraUF = new BrightIdeasSoftware.OLVColumn();
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
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvDtAut);
		this.lsvData.AllColumns.Add(this.olvDtEmi);
		this.lsvData.AllColumns.Add(this.olvValor);
		this.lsvData.AllColumns.Add(this.olvCTeModFrete);
		this.lsvData.AllColumns.Add(this.olvVenc);
		this.lsvData.AllColumns.Add(this.olvDtEntFisc);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvManifest);
		this.lsvData.AllColumns.Add(this.olvEmitCNPJ);
		this.lsvData.AllColumns.Add(this.olvEmitNome);
		this.lsvData.AllColumns.Add(this.olvCancel);
		this.lsvData.AllColumns.Add(this.olvHasEvent);
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
		this.lsvData.AllColumns.Add(this.olvCdCstICMSUf);
		this.lsvData.AllColumns.Add(this.olvBaseIcmsOutraUf);
		this.lsvData.AllColumns.Add(this.olvValorIcmsOutraUf);
		this.lsvData.AllColumns.Add(this.olvTaxaICMSOutra);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvFretePesoTransp);
		this.lsvData.AllColumns.Add(this.olvFreteUnidadePeso);
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
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvTagDtHr);
		this.lsvData.AllColumns.Add(this.olvTagUser);
		this.lsvData.AllColumns.Add(this.olvDocNoteDtHr);
		this.lsvData.AllColumns.Add(this.olvDocNoteUser);
		this.lsvData.AllColumns.Add(this.olvInfAdFisco);
		this.lsvData.AllColumns.Add(this.olvInfCpl);
		this.lsvData.AllColumns.Add(this.olvObsCont);
		this.lsvData.AllColumns.Add(this.olvVersion);
		this.lsvData.AllColumns.Add(this.olvDFeTipo);
		this.lsvData.AllColumns.Add(this.olvDFeChave);
		this.lsvData.AllColumns.Add(this.olvDFeNum);
		this.lsvData.AllColumns.Add(this.olvDFeSerie);
		this.lsvData.AllColumns.Add(this.olvDFeValor);
		this.lsvData.AllColumns.Add(this.olvDFecUF);
		this.lsvData.AllColumns.Add(this.olvDFeDtAut);
		this.lsvData.AllColumns.Add(this.olvDFeDtEmi);
		this.lsvData.AllColumns.Add(this.olvDFeDVenc);
		this.lsvData.AllColumns.Add(this.olvDFeDtEntFisc);
		this.lsvData.AllColumns.Add(this.olvDFexMotivo);
		this.lsvData.AllColumns.Add(this.olvDFeTpDoc);
		this.lsvData.AllColumns.Add(this.olvDFeNatOper);
		this.lsvData.AllColumns.Add(this.olvDFeCFOPList);
		this.lsvData.AllColumns.Add(this.olvDFeEmitId);
		this.lsvData.AllColumns.Add(this.olvDFeEmitNome);
		this.lsvData.AllColumns.Add(this.olvDFeEmitIE);
		this.lsvData.AllColumns.Add(this.olvDFeEmitCmun);
		this.lsvData.AllColumns.Add(this.olvDFeEmitXmun);
		this.lsvData.AllColumns.Add(this.olvDFeEmitUf);
		this.lsvData.AllColumns.Add(this.olvDFeTomaId);
		this.lsvData.AllColumns.Add(this.olvDFeTomaNome);
		this.lsvData.AllColumns.Add(this.olvDFeTomaIE);
		this.lsvData.AllColumns.Add(this.olvDFeTomaCmun);
		this.lsvData.AllColumns.Add(this.olvDFeTomaXmun);
		this.lsvData.AllColumns.Add(this.olvDFeTomaUf);
		this.lsvData.AllColumns.Add(this.olvDFeDestinId);
		this.lsvData.AllColumns.Add(this.olvDFeDestinNome);
		this.lsvData.AllColumns.Add(this.olvDFeDestinIE);
		this.lsvData.AllColumns.Add(this.olvDFeDestinCmun);
		this.lsvData.AllColumns.Add(this.olvDFeDestinXmun);
		this.lsvData.AllColumns.Add(this.olvDFeDestinUf);
		this.lsvData.AllColumns.Add(this.olvDFeTranspId);
		this.lsvData.AllColumns.Add(this.olvDFeTranspNome);
		this.lsvData.AllColumns.Add(this.olvDFeTranspIE);
		this.lsvData.AllColumns.Add(this.olvDFeTranspCmun);
		this.lsvData.AllColumns.Add(this.olvDFeTranspXmun);
		this.lsvData.AllColumns.Add(this.olvDFeTranspUf);
		this.lsvData.AllColumns.Add(this.olvDFeBaseICMS);
		this.lsvData.AllColumns.Add(this.olvDFeValorICMS);
		this.lsvData.AllColumns.Add(this.olvDFeVlrICMSDeson);
		this.lsvData.AllColumns.Add(this.olvDFeVlrBCST);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotST);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotProd);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotFrete);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotSeg);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotDesc);
		this.lsvData.AllColumns.Add(this.olvVolPesoB);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotII);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotIPI);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotIPIDevol);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotPIS);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotCOFINS);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotOutro);
		this.lsvData.AllColumns.Add(this.olvDFeVlrTotTrib);
		this.lsvData.AllColumns.Add(this.olvDFeDocNote);
		this.lsvData.AllColumns.Add(this.olvDFeModFrete);
		this.lsvData.AllColumns.Add(this.olvDFeVersion);
		this.lsvData.AllColumns.Add(this.olvDFeDestinCEP);
		this.lsvData.AllColumns.Add(this.olvFretePesoBruto);
		this.lsvData.AllColumns.Add(this.olvFretePesoBrtUn);
		this.lsvData.AllColumns.Add(this.olvFretePesoLiquido);
		this.lsvData.AllColumns.Add(this.olvFretePesoLiqUn);
		this.lsvData.AllColumns.Add(this.olvFreteQuantidade);
		this.lsvData.AllColumns.Add(this.olvFreteQuantidadeUn);
		this.lsvData.AllColumns.Add(this.olvFreteVolTransp);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[158]
		{
			this.olvSelect, this.olvHasXml, this.olvTag, this.olvNum, this.olvSerie, this.olvDtAut, this.olvDtEmi, this.olvValor, this.olvCTeModFrete, this.olvVenc,
			this.olvDtEntFisc, this.olvStatus, this.olvManifest, this.olvEmitCNPJ, this.olvEmitNome, this.olvCancel, this.olvHasEvent, this.olvMotivo, this.olvEstado, this.olvChave,
			this.olvFilial, this.olvTpDoc, this.olvTpServ, this.olvModal, this.olvNatOper, this.olvProPred, this.olvBaseIcms, this.olvTaxaIcms, this.olvValorIcms, this.olvCdCstICMS,
			this.olvDescCstICMS, this.olvIndSimplesNac, this.olvCdCstICMSUf, this.olvBaseIcmsOutraUf, this.olvValorIcmsOutraUf, this.olvTaxaICMSOutra, this.olvCFOP, this.olvFretePesoTransp, this.olvFreteUnidadePeso, this.olvMunIni,
			this.olvxMunIni, this.olvUFIni, this.olvMunFim, this.olvxMunFim, this.olvUFFim, this.olvcMunEnv, this.olvxMunEnv, this.olvUFEnv, this.olvEmitIE, this.olvEmitcMun,
			this.olvEmitxMun, this.olvEmitUf, this.olvTomaId, this.olvTomaIE, this.olvTomaNome, this.olvTomacMun, this.olvTomaxMun, this.olvTomaUf, this.olvExpediId, this.olvExpediIE,
			this.olvExpediNome, this.olvExpedicMun, this.olvExpedixMun, this.olvExpediUf, this.olvRemeteId, this.olvRemeteIE, this.olvRemeteNome, this.olvRemetecMun, this.olvRemetexMun, this.olvRemeteUf,
			this.olvDestinId, this.olvDestinIE, this.olvDestinNome, this.olvDestincMun, this.olvDestinxMun, this.olvDestinUf, this.olvRecebeId, this.olvRecebeIE, this.olvRecebeNome, this.olvRecebecMun,
			this.olvRecebexMun, this.olvRecebeUf, this.olvAnoMes, this.olvDocNote, this.olvTagDtHr, this.olvTagUser, this.olvDocNoteDtHr, this.olvDocNoteUser, this.olvInfAdFisco, this.olvInfCpl,
			this.olvObsCont, this.olvVersion, this.olvDFeTipo, this.olvDFeChave, this.olvDFeNum, this.olvDFeSerie, this.olvDFeValor, this.olvDFecUF, this.olvDFeDtAut, this.olvDFeDtEmi,
			this.olvDFeDVenc, this.olvDFeDtEntFisc, this.olvDFexMotivo, this.olvDFeTpDoc, this.olvDFeNatOper, this.olvDFeCFOPList, this.olvDFeEmitId, this.olvDFeEmitNome, this.olvDFeEmitIE, this.olvDFeEmitCmun,
			this.olvDFeEmitXmun, this.olvDFeEmitUf, this.olvDFeTomaId, this.olvDFeTomaNome, this.olvDFeTomaIE, this.olvDFeTomaCmun, this.olvDFeTomaXmun, this.olvDFeTomaUf, this.olvDFeDestinId, this.olvDFeDestinNome,
			this.olvDFeDestinIE, this.olvDFeDestinCmun, this.olvDFeDestinXmun, this.olvDFeDestinUf, this.olvDFeTranspId, this.olvDFeTranspNome, this.olvDFeTranspIE, this.olvDFeTranspCmun, this.olvDFeTranspXmun, this.olvDFeTranspUf,
			this.olvDFeBaseICMS, this.olvDFeValorICMS, this.olvDFeVlrICMSDeson, this.olvDFeVlrBCST, this.olvDFeVlrTotST, this.olvDFeVlrTotProd, this.olvDFeVlrTotFrete, this.olvDFeVlrTotSeg, this.olvDFeVlrTotDesc, this.olvVolPesoB,
			this.olvDFeVlrTotII, this.olvDFeVlrTotIPI, this.olvDFeVlrTotIPIDevol, this.olvDFeVlrTotPIS, this.olvDFeVlrTotCOFINS, this.olvDFeVlrTotOutro, this.olvDFeVlrTotTrib, this.olvDFeDocNote, this.olvDFeModFrete, this.olvDFeVersion,
			this.olvDFeDestinCEP, this.olvFretePesoBruto, this.olvFretePesoBrtUn, this.olvFretePesoLiquido, this.olvFretePesoLiqUn, this.olvFreteQuantidade, this.olvFreteQuantidadeUn, this.olvFreteVolTransp
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
		this.olvHasXml.Width = 46;
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "CTe:Etiq";
		this.olvTag.ToolTipText = "Etiqueta atribuída";
		this.olvTag.Width = 39;
		this.olvNum.AspectName = "Num";
		this.olvNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.Hyperlink = true;
		this.olvNum.Text = "CTe:Num";
		this.olvNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.ToolTipText = "Número do documento fiscal";
		this.olvNum.Width = 84;
		this.olvTipo.AspectName = "Model";
		this.olvTipo.DisplayIndex = 4;
		this.olvTipo.IsVisible = false;
		this.olvTipo.Text = "Tipo";
		this.olvTipo.ToolTipText = "Modelo do documento fiscal";
		this.olvTipo.Width = 43;
		this.olvSerie.AspectName = "Serie";
		this.olvSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.Text = "CTe:Serie";
		this.olvSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.ToolTipText = "Série do CT-e";
		this.olvSerie.Width = 142;
		this.olvDtAut.AspectName = "DtAut";
		this.olvDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.Text = "CTe:DtAut";
		this.olvDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.ToolTipText = "Data de autorização";
		this.olvDtAut.Width = 155;
		this.olvDtEmi.AspectName = "DtEmi";
		this.olvDtEmi.Text = "CTe:DtEmi";
		this.olvDtEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmi.ToolTipText = "Data de emissão";
		this.olvDtEmi.Width = 81;
		this.olvValor.AspectName = "Valor";
		this.olvValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValor.Text = "CTe:Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.ToolTipText = "Valor do documento fiscal";
		this.olvValor.Width = 155;
		this.olvCTeModFrete.AspectName = "ModFrete";
		this.olvCTeModFrete.Text = "CTe: ModFrete";
		this.olvCTeModFrete.ToolTipText = "Modalidade de Frete CTe";
		this.olvCTeModFrete.Width = 148;
		this.olvVenc.AspectName = "DVenc";
		this.olvVenc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVenc.Text = "CTe:Venc.";
		this.olvVenc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvVenc.ToolTipText = "Data de vencimento";
		this.olvVenc.Width = 90;
		this.olvDtEntFisc.AspectName = "DtEntFisc";
		this.olvDtEntFisc.Text = "CTe:EntFiscal";
		this.olvDtEntFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEntFisc.ToolTipText = "Data entrada fiscal";
		this.olvDtEntFisc.Width = 71;
		this.olvStatus.Groupable = false;
		this.olvStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStatus.Searchable = false;
		this.olvStatus.Text = "CTe:Status";
		this.olvStatus.ToolTipText = "Status do documento";
		this.olvStatus.UseFiltering = false;
		this.olvManifest.AspectName = "ManifCode";
		this.olvManifest.Text = "CTe: Manifestação";
		this.olvManifest.ToolTipText = "Manifestação do destinatario";
		this.olvManifest.Width = 150;
		this.olvEmitCNPJ.AspectName = "EmitID";
		this.olvEmitCNPJ.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitCNPJ.Text = "CTe: Emissor CNPJ/CPF";
		this.olvEmitCNPJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitCNPJ.ToolTipText = "CNPJ/CPF do emitente";
		this.olvEmitCNPJ.Width = 50;
		this.olvEmitNome.AspectName = "EmitNome";
		this.olvEmitNome.Text = "CTe:Emissor Nome";
		this.olvEmitNome.ToolTipText = "Nome do emitente";
		this.olvCancel.AspectName = "Canceled";
		this.olvCancel.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.Text = "CTe:Can";
		this.olvCancel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.ToolTipText = "Cancelado";
		this.olvCancel.Width = 45;
		this.olvHasEvent.AspectName = "HasEvent";
		this.olvHasEvent.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasEvent.Text = "CTe:Evt";
		this.olvHasEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasEvent.ToolTipText = "Tem evento vinculado";
		this.olvHasEvent.Width = 45;
		this.olvMotivo.AspectName = "xMotivo";
		this.olvMotivo.Text = "CTe:Status";
		this.olvMotivo.ToolTipText = "Motivo";
		this.olvMotivo.Width = 210;
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "CTe:UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do CT-e";
		this.olvEstado.Width = 40;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "CTe:Chave";
		this.olvChave.ToolTipText = "Chave de acesso da NF-e";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "CTe:Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 130;
		this.olvTpDoc.AspectName = "TpDoc";
		this.olvTpDoc.Text = "CTe:TipoDoc";
		this.olvTpDoc.ToolTipText = "Tipo de documento";
		this.olvTpServ.AspectName = "tpServ";
		this.olvTpServ.Text = "CTe:TipoServ";
		this.olvTpServ.ToolTipText = "Tipo de serviço";
		this.olvModal.AspectName = "Modal";
		this.olvModal.Text = "CTe:Modal";
		this.olvModal.ToolTipText = "Modal";
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "CTe:Natureza";
		this.olvNatOper.ToolTipText = "Descrição da Natureza da operação";
		this.olvProPred.AspectName = "ProPred";
		this.olvProPred.Text = "CTe:Produto predominante";
		this.olvProPred.ToolTipText = "Produto predominante";
		this.olvBaseIcms.AspectName = "BaseICMS";
		this.olvBaseIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseIcms.Text = "CTe:ICMS Base";
		this.olvBaseIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseIcms.ToolTipText = "Base de Cálculo do ICMS";
		this.olvBaseIcms.Width = 80;
		this.olvTaxaIcms.AspectName = "TaxaICMS";
		this.olvTaxaIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTaxaIcms.Text = "CTe:ICMS Taxa";
		this.olvTaxaIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvTaxaIcms.ToolTipText = "Taxa de Cálculo do ICMS";
		this.olvValorIcms.AspectName = "ValorICMS";
		this.olvValorIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorIcms.Text = "CTe:ICMS Valor";
		this.olvValorIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorIcms.ToolTipText = "Valor do ICMS";
		this.olvValorIcms.Width = 90;
		this.olvCdCstICMS.AspectName = "CdCstICMS";
		this.olvCdCstICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMS.Text = "CTe:ICMS ST";
		this.olvCdCstICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMS.ToolTipText = "Classificação tributária do serviço";
		this.olvCdCstICMS.Width = 90;
		this.olvDescCstICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDescCstICMS.Text = "CTe:ICMS ST Desc";
		this.olvDescCstICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDescCstICMS.ToolTipText = "Código de situação tributária";
		this.olvDescCstICMS.Width = 200;
		this.olvIndSimplesNac.AspectName = "IndSimplesNac";
		this.olvIndSimplesNac.Text = "CTe:IndSimplesNac";
		this.olvIndSimplesNac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMSUf.AspectName = "CdCstICMS";
		this.olvCdCstICMSUf.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCdCstICMSUf.Text = "CTe:CST Icms Outra UF";
		this.olvCdCstICMSUf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvCdCstICMSUf.ToolTipText = "Valor do CST do ICMS";
		this.olvCdCstICMSUf.Width = 80;
		this.olvBaseIcmsOutraUf.AspectName = "BaseICMSOutraUF";
		this.olvBaseIcmsOutraUf.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseIcmsOutraUf.Text = "CTe:Base Icms Outra UF";
		this.olvBaseIcmsOutraUf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseIcmsOutraUf.ToolTipText = "Valor da BC do ICMS";
		this.olvBaseIcmsOutraUf.Width = 80;
		this.olvValorIcmsOutraUf.AspectName = "ValorICMSOutraUF";
		this.olvValorIcmsOutraUf.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorIcmsOutraUf.Text = "CTe:Valor ICMS Outra UF";
		this.olvValorIcmsOutraUf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorIcmsOutraUf.ToolTipText = "Valor do ICMS devido outra UF";
		this.olvValorIcmsOutraUf.Width = 80;
		this.olvTaxaICMSOutra.AspectName = "TaxaICMSOutraUF";
		this.olvTaxaICMSOutra.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTaxaICMSOutra.Text = "CTe:Taxa ICMS Outra UF";
		this.olvTaxaICMSOutra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvTaxaICMSOutra.ToolTipText = "Taxa do ICMS devido outra UF";
		this.olvTaxaICMSOutra.Width = 80;
		this.olvCFOP.AspectName = "CFOPList";
		this.olvCFOP.Text = "CTe:CFOP";
		this.olvCFOP.ToolTipText = "Código fiscal de operações";
		this.olvCFOP.Width = 100;
		this.olvFretePesoTransp.AspectName = "FretePesoTransp";
		this.olvFretePesoTransp.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoTransp.Text = "CTe: Peso Transportado";
		this.olvFretePesoTransp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFretePesoTransp.ToolTipText = "Peso total em Kg";
		this.olvFreteUnidadePeso.AspectName = "FreteUnidadePeso";
		this.olvFreteUnidadePeso.Text = "CTe: Frete unidade de peso";
		this.olvFreteUnidadePeso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFreteUnidadePeso.ToolTipText = "Frete unidade de peso";
		this.olvFreteUnidadePeso.Width = 100;
		this.olvMunIni.AspectName = "cMunIni";
		this.olvMunIni.Text = "CTe:MunIniCod";
		this.olvMunIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMunIni.ToolTipText = "Código do município de início";
		this.olvxMunIni.AspectName = "xMunIni";
		this.olvxMunIni.Text = "CTe:MunIniNome";
		this.olvxMunIni.ToolTipText = "Nome do município do início";
		this.olvUFIni.AspectName = "UFIni";
		this.olvUFIni.Text = "CTe:MunIniUf";
		this.olvUFIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUFIni.ToolTipText = "UF do início da prestação";
		this.olvMunFim.AspectName = "cMunFim";
		this.olvMunFim.Text = "CTe:MunFimCod";
		this.olvMunFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMunFim.ToolTipText = "Código do município de término da prestação";
		this.olvxMunFim.AspectName = "xMunFim";
		this.olvxMunFim.Text = "CTe:MunFimNome";
		this.olvxMunFim.ToolTipText = "Nome do município do término da prestação";
		this.olvUFFim.AspectName = "UFFim";
		this.olvUFFim.Text = "CTe:MunFimUf";
		this.olvUFFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUFFim.ToolTipText = "UF do término da prestação";
		this.olvcMunEnv.AspectName = "cMunEnv";
		this.olvcMunEnv.Text = "CTe:MunEnvCod";
		this.olvcMunEnv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvcMunEnv.ToolTipText = "Código do município de envio";
		this.olvxMunEnv.AspectName = "xMunEnv";
		this.olvxMunEnv.Text = "CTe:MunEnvNome";
		this.olvxMunEnv.ToolTipText = "Nome do município de envio";
		this.olvUFEnv.AspectName = "UFEnv";
		this.olvUFEnv.Text = "CTe:MunEnvUf";
		this.olvUFEnv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUFEnv.ToolTipText = "Sigla da UF de envio do CT-e";
		this.olvEmitIE.AspectName = "EmitIE";
		this.olvEmitIE.Text = "CTe:Emissor IE";
		this.olvEmitIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitIE.ToolTipText = "Inscrição estadual do emissor ";
		this.olvEmitcMun.AspectName = "EmitcMun";
		this.olvEmitcMun.Text = "CTe:Emissor Mun Cod";
		this.olvEmitcMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitcMun.ToolTipText = "Código do município do emissor";
		this.olvEmitxMun.AspectName = "EmitxMun";
		this.olvEmitxMun.Text = "CTe:Emissor Mun Desc";
		this.olvEmitxMun.ToolTipText = "Nome do municipío do emissor";
		this.olvEmitUf.AspectName = "EmitUf";
		this.olvEmitUf.Text = "CTe:Emissor Uf";
		this.olvEmitUf.ToolTipText = "Sigla da UF do emissor";
		this.olvTomaId.AspectName = "TomaID";
		this.olvTomaId.Text = "CTe:Tomador CNPJ/CPF";
		this.olvTomaId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomaId.ToolTipText = "CNPJ/CPF do tomador";
		this.olvTomaId.Width = 130;
		this.olvTomaIE.AspectName = "TomaIE";
		this.olvTomaIE.Text = "CTe:Tomador IE";
		this.olvTomaIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomaIE.ToolTipText = "Inscrição estadual do tomador";
		this.olvTomaNome.AspectName = "TomaNome";
		this.olvTomaNome.Text = "CTe:Tomador Nome";
		this.olvTomaNome.ToolTipText = "Nome do tomador";
		this.olvTomaNome.Width = 230;
		this.olvTomacMun.AspectName = "TomacMun";
		this.olvTomacMun.Text = "CTe:Tomador Mun Cod";
		this.olvTomacMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomacMun.ToolTipText = "Código do município do tomador";
		this.olvTomaxMun.AspectName = "TomaxMun";
		this.olvTomaxMun.Text = "CTe:Tomador Mun Desc";
		this.olvTomaxMun.ToolTipText = "Nome do municipío do tomador";
		this.olvTomaUf.AspectName = "TomaUf";
		this.olvTomaUf.Text = "CTe:Tomador Uf";
		this.olvTomaUf.ToolTipText = "UF do tomador";
		this.olvExpediId.AspectName = "ExpediID";
		this.olvExpediId.Text = "CTe:Expedidor CNPJ/CPF";
		this.olvExpediId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExpediId.ToolTipText = "CNPJ/CPF do expedidor";
		this.olvExpediId.Width = 130;
		this.olvExpediIE.AspectName = "ExpediIE";
		this.olvExpediIE.Text = "CTe:Expedidor IE";
		this.olvExpediIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExpediIE.ToolTipText = "Inscrição estadual do expedidor";
		this.olvExpediNome.AspectName = "ExpediNome";
		this.olvExpediNome.Text = "CTe:Expedidor Nome";
		this.olvExpediNome.ToolTipText = "Nome do expedidor";
		this.olvExpediNome.Width = 230;
		this.olvExpedicMun.AspectName = "ExpedicMun";
		this.olvExpedicMun.Text = "CTe:Expedidor Mun Cod";
		this.olvExpedicMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExpedicMun.ToolTipText = "Código do município do expedidor";
		this.olvExpedixMun.AspectName = "ExpedixMun";
		this.olvExpedixMun.Text = "CTe:Expedidor Mun Desc";
		this.olvExpedixMun.ToolTipText = "Nome do municipío do expedidor";
		this.olvExpediUf.AspectName = "ExpediUf";
		this.olvExpediUf.Text = "CTe:Expedidor Uf";
		this.olvExpediUf.ToolTipText = "Sigla da UF do expedidor";
		this.olvRemeteId.AspectName = "RemeteID";
		this.olvRemeteId.Text = "CTe:Remetente CNPJ/CPF";
		this.olvRemeteId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRemeteId.ToolTipText = "CNPJ/CPF do remetente";
		this.olvRemeteId.Width = 130;
		this.olvRemeteIE.AspectName = "RemeteIE";
		this.olvRemeteIE.Text = "CTe:Remetente IE";
		this.olvRemeteIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRemeteIE.ToolTipText = "Inscrição estadual do remetente";
		this.olvRemeteNome.AspectName = "RemeteNome";
		this.olvRemeteNome.Text = "CTe:Remetente Nome";
		this.olvRemeteNome.ToolTipText = "Nome do remetente";
		this.olvRemeteNome.Width = 230;
		this.olvRemetecMun.AspectName = "RemetecMun";
		this.olvRemetecMun.Text = "CTe:Remetente Mun Cod";
		this.olvRemetecMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRemetecMun.ToolTipText = "Código do municipío do remetente";
		this.olvRemetexMun.AspectName = "RemetexMun";
		this.olvRemetexMun.Text = "CTe:Remetente Mun Desc";
		this.olvRemetexMun.ToolTipText = "Nome do municipío do remetente";
		this.olvRemeteUf.AspectName = "RemeteUf";
		this.olvRemeteUf.Text = "CTe:Remetente Uf";
		this.olvRemeteUf.ToolTipText = "Sigla da UF do remetente";
		this.olvDestinId.AspectName = "DestinID";
		this.olvDestinId.Text = "CTe:Destinatário CNPJ/CPF";
		this.olvDestinId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinId.ToolTipText = "CNPJ/CPF do destinatário";
		this.olvDestinId.Width = 130;
		this.olvDestinIE.AspectName = "DestinIE";
		this.olvDestinIE.Text = "CTe:Destinatário IE";
		this.olvDestinIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinIE.ToolTipText = "Inscrição estadual do destinatário";
		this.olvDestinNome.AspectName = "DestinNome";
		this.olvDestinNome.Text = "CTe:Destinatário Nome";
		this.olvDestinNome.ToolTipText = "Nome do destinatário";
		this.olvDestinNome.Width = 230;
		this.olvDestincMun.AspectName = "DestincMun";
		this.olvDestincMun.Text = "CTe:Destinatário Mun Cod";
		this.olvDestincMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestincMun.ToolTipText = "Código do município do destinatário";
		this.olvDestinxMun.AspectName = "DestinxMun";
		this.olvDestinxMun.Text = "CTe:Destinatário Mun Desc";
		this.olvDestinxMun.ToolTipText = "Nome do município do destinatário";
		this.olvDestinUf.AspectName = "DestinUf";
		this.olvDestinUf.Text = "CTe:Destinatário Uf";
		this.olvDestinUf.ToolTipText = "Sigla da UF do destinatário";
		this.olvRecebeId.AspectName = "RecebeID";
		this.olvRecebeId.Text = "CTe:Recebedor CNPJ/CPF";
		this.olvRecebeId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecebeId.ToolTipText = "CNPJ/CPF do recebedor";
		this.olvRecebeId.Width = 130;
		this.olvRecebeIE.AspectName = "RecebeIE";
		this.olvRecebeIE.Text = "CTe:Recebedor IE";
		this.olvRecebeIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecebeIE.ToolTipText = "Inscrição estadual do recebedor";
		this.olvRecebeNome.AspectName = "RecebeNome";
		this.olvRecebeNome.Text = "CTe:Recebedor Nome";
		this.olvRecebeNome.ToolTipText = "Nome do recebedor";
		this.olvRecebeNome.Width = 230;
		this.olvRecebecMun.AspectName = "RecebecMun";
		this.olvRecebecMun.Text = "CTe:Recebedor Mun Cod";
		this.olvRecebecMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecebecMun.ToolTipText = "Código do município do recebedor";
		this.olvRecebexMun.AspectName = "RecebexMun";
		this.olvRecebexMun.Text = "CTe:Recebedor Mun Desc";
		this.olvRecebexMun.ToolTipText = "Nome do município do recebedor";
		this.olvRecebeUf.AspectName = "RecebeUf";
		this.olvRecebeUf.Text = "CTe:Recebedor Uf";
		this.olvRecebeUf.ToolTipText = "Sigla da UF do recebedor";
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "CTe:Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Data de autorização";
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "CTe:Comentários";
		this.olvDocNote.ToolTipText = "Comentários";
		this.olvTagDtHr.AspectName = "TagDtHr";
		this.olvTagDtHr.Text = "CTe:Etiq : Data Hora";
		this.olvTagUser.AspectName = "TagUser";
		this.olvTagUser.Text = "CTe:Etiq : Usuário";
		this.olvDocNoteDtHr.AspectName = "DocNoteDtHr";
		this.olvDocNoteDtHr.Text = "CTe:Comentário : Data Hora";
		this.olvDocNoteUser.AspectName = "DocNoteUser";
		this.olvDocNoteUser.Text = "CTe:Comentário : Usuário";
		this.olvInfAdFisco.Text = "CTe:Informações adicionais fisco";
		this.olvInfAdFisco.ToolTipText = "Informações adicionais do fiscal";
		this.olvInfAdFisco.Width = 150;
		this.olvInfCpl.Text = "CTe:Informações complementares ";
		this.olvInfCpl.Width = 150;
		this.olvObsCont.Text = "CTe:Observações contribuinte";
		this.olvObsCont.Width = 150;
		this.olvVersion.AspectName = "Version";
		this.olvVersion.Text = "CTe:XML: Versão";
		this.olvVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeTipo.AspectName = "DFeType";
		this.olvDFeTipo.DisplayIndex = 99;
		this.olvDFeTipo.Text = "DFe:Tipo";
		this.olvDFeTipo.ToolTipText = "Tipo de documento relacionado";
		this.olvDFeChave.AspectName = "DFeChave";
		this.olvDFeChave.DisplayIndex = 100;
		this.olvDFeChave.Text = "DFe:Chave";
		this.olvDFeChave.ToolTipText = "Chave de acesso da NF-e";
		this.olvDFeNum.AspectName = "DFeNum";
		this.olvDFeNum.DisplayIndex = 101;
		this.olvDFeNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeNum.Hyperlink = true;
		this.olvDFeNum.Text = "DFe:Num";
		this.olvDFeNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeNum.ToolTipText = "Número da NF-e";
		this.olvDFeNum.Width = 64;
		this.olvDFeSerie.AspectName = "DFeSerie";
		this.olvDFeSerie.DisplayIndex = 102;
		this.olvDFeSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeSerie.Text = "DFe:Serie";
		this.olvDFeSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeSerie.ToolTipText = "Série da NF-e";
		this.olvDFeSerie.Width = 54;
		this.olvDFeValor.AspectName = "DFeValor";
		this.olvDFeValor.DisplayIndex = 103;
		this.olvDFeValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeValor.Text = "DFe:Valor";
		this.olvDFeValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeValor.ToolTipText = "Valor da NF-e";
		this.olvDFeValor.Width = 90;
		this.olvDFecUF.AspectName = "DFecUF";
		this.olvDFecUF.DisplayIndex = 104;
		this.olvDFecUF.Text = "DFe:UF";
		this.olvDFecUF.ToolTipText = "Sigla da UF da NF-e";
		this.olvDFeDtAut.AspectName = "DFeDtAut";
		this.olvDFeDtAut.DisplayIndex = 105;
		this.olvDFeDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDtAut.Text = "DFe:DtAut";
		this.olvDFeDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDtAut.ToolTipText = "Data de autorização da NF-e";
		this.olvDFeDtAut.Width = 81;
		this.olvDFeDtEmi.AspectName = "DFeDtEmi";
		this.olvDFeDtEmi.DisplayIndex = 106;
		this.olvDFeDtEmi.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDtEmi.Text = "DFe:DtEmi";
		this.olvDFeDtEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDtEmi.ToolTipText = "Data de emissão da NF-e";
		this.olvDFeDtEmi.Width = 81;
		this.olvDFeDVenc.AspectName = "DFeDVenc";
		this.olvDFeDVenc.DisplayIndex = 107;
		this.olvDFeDVenc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDVenc.Text = "DFe:Venc.";
		this.olvDFeDVenc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDVenc.ToolTipText = "Data de vencimento da NF-e";
		this.olvDFeDVenc.Width = 81;
		this.olvDFeDtEntFisc.AspectName = "DFeDtEntFisc";
		this.olvDFeDtEntFisc.DisplayIndex = 108;
		this.olvDFeDtEntFisc.Text = "DFe:EntFiscal";
		this.olvDFeDtEntFisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDtEntFisc.ToolTipText = "Data da entrada fiscal";
		this.olvDFeDtEntFisc.Width = 71;
		this.olvDFexMotivo.AspectName = "DFexMotivo";
		this.olvDFexMotivo.DisplayIndex = 109;
		this.olvDFexMotivo.Text = "DFe:Status";
		this.olvDFexMotivo.ToolTipText = "Motivo";
		this.olvDFeTpDoc.AspectName = "DFeTpDoc";
		this.olvDFeTpDoc.DisplayIndex = 110;
		this.olvDFeTpDoc.Text = "DFe:TipoDoc";
		this.olvDFeTpDoc.ToolTipText = "Tipo de documento";
		this.olvDFeNatOper.AspectName = "DFeNatOper";
		this.olvDFeNatOper.DisplayIndex = 111;
		this.olvDFeNatOper.Text = "DFe:Natureza";
		this.olvDFeNatOper.ToolTipText = "Natureza da operação";
		this.olvDFeCFOPList.AspectName = "DFeCFOPList";
		this.olvDFeCFOPList.DisplayIndex = 112;
		this.olvDFeCFOPList.Text = "DFe:CFOP";
		this.olvDFeCFOPList.ToolTipText = "Código fiscal de operações e prestações";
		this.olvDFeCFOPList.Width = 90;
		this.olvDFeEmitId.AspectName = "DFeEmitId";
		this.olvDFeEmitId.DisplayIndex = 113;
		this.olvDFeEmitId.Text = "DFe:Emissor CNPJ/CPF";
		this.olvDFeEmitId.ToolTipText = "CNPJ/CPF do emissor";
		this.olvDFeEmitId.Width = 130;
		this.olvDFeEmitNome.AspectName = "DFeEmitNome";
		this.olvDFeEmitNome.DisplayIndex = 114;
		this.olvDFeEmitNome.Text = "DFe:Emissor Nome";
		this.olvDFeEmitNome.ToolTipText = "Nome do emissor";
		this.olvDFeEmitNome.Width = 230;
		this.olvDFeEmitIE.AspectName = "DFeEmitIE";
		this.olvDFeEmitIE.DisplayIndex = 115;
		this.olvDFeEmitIE.Text = "DFe:Emissor IE";
		this.olvDFeEmitIE.ToolTipText = "Inscrição Estadual do emissor";
		this.olvDFeEmitCmun.AspectName = "DFeEmitCmun";
		this.olvDFeEmitCmun.DisplayIndex = 116;
		this.olvDFeEmitCmun.Text = "DFe:Emissor Mun Cod";
		this.olvDFeEmitCmun.ToolTipText = "Código do município do emissor";
		this.olvDFeEmitXmun.AspectName = "DFeEmitXmun";
		this.olvDFeEmitXmun.DisplayIndex = 117;
		this.olvDFeEmitXmun.Text = "DFe:Emissor Mun Desc";
		this.olvDFeEmitXmun.ToolTipText = "Nome do município do emissor";
		this.olvDFeEmitUf.AspectName = "DFeEmitUf";
		this.olvDFeEmitUf.DisplayIndex = 118;
		this.olvDFeEmitUf.Text = "DFe:Emissor Uf";
		this.olvDFeEmitUf.ToolTipText = "Sigla da UF do emissor ";
		this.olvDFeTomaId.AspectName = "DFeTomaId";
		this.olvDFeTomaId.DisplayIndex = 119;
		this.olvDFeTomaId.Text = "DFe: Tomador CNPJ/CPF";
		this.olvDFeTomaId.ToolTipText = "CNPJ/CPF do tomador";
		this.olvDFeTomaId.Width = 130;
		this.olvDFeTomaNome.AspectName = "DFeTomaNome";
		this.olvDFeTomaNome.DisplayIndex = 120;
		this.olvDFeTomaNome.Text = "DFe:Tomador Nome";
		this.olvDFeTomaNome.ToolTipText = "Nome do tomador";
		this.olvDFeTomaNome.Width = 230;
		this.olvDFeTomaIE.AspectName = "DFeTomaIE";
		this.olvDFeTomaIE.DisplayIndex = 121;
		this.olvDFeTomaIE.Text = "DFe:Tomador IE";
		this.olvDFeTomaIE.ToolTipText = "Inscrição estadual do tomador";
		this.olvDFeTomaCmun.AspectName = "DFeTomaCmun";
		this.olvDFeTomaCmun.DisplayIndex = 122;
		this.olvDFeTomaCmun.Text = "DFe:Tomador Mun Cod";
		this.olvDFeTomaCmun.ToolTipText = "Código do município do tomador";
		this.olvDFeTomaXmun.AspectName = "DFeTomaXmun";
		this.olvDFeTomaXmun.DisplayIndex = 123;
		this.olvDFeTomaXmun.Text = "DFe:Tomador Mun Desc";
		this.olvDFeTomaXmun.ToolTipText = "Nome do município do tomador";
		this.olvDFeTomaUf.AspectName = "DFeTomaUf";
		this.olvDFeTomaUf.DisplayIndex = 124;
		this.olvDFeTomaUf.Text = "DFe:Tomador Uf";
		this.olvDFeTomaUf.ToolTipText = "Sigla da UF do tomador";
		this.olvDFeDestinId.AspectName = "DFeDestinId";
		this.olvDFeDestinId.DisplayIndex = 125;
		this.olvDFeDestinId.Text = "DFe:Destinatário CNPJ/CPF";
		this.olvDFeDestinId.ToolTipText = "CNPJ/CPF do destinatário";
		this.olvDFeDestinId.Width = 130;
		this.olvDFeDestinNome.AspectName = "DFeDestinNome";
		this.olvDFeDestinNome.DisplayIndex = 126;
		this.olvDFeDestinNome.Text = "DFe:Destinatário Nome";
		this.olvDFeDestinNome.ToolTipText = "Nome do destinatário";
		this.olvDFeDestinNome.Width = 230;
		this.olvDFeDestinIE.AspectName = "DFeDestinIE";
		this.olvDFeDestinIE.DisplayIndex = 127;
		this.olvDFeDestinIE.Text = "DFe:Destinatário IE";
		this.olvDFeDestinIE.ToolTipText = "Inscrição estadual do destinatário";
		this.olvDFeDestinCmun.AspectName = "DFeDestinCmun";
		this.olvDFeDestinCmun.DisplayIndex = 128;
		this.olvDFeDestinCmun.Text = "DFe:Destinatário Mun Cod";
		this.olvDFeDestinCmun.ToolTipText = "Código do município do destinatário";
		this.olvDFeDestinXmun.AspectName = "DFeDestinXmun";
		this.olvDFeDestinXmun.DisplayIndex = 129;
		this.olvDFeDestinXmun.Text = "DFe:Destinatário Mun Desc";
		this.olvDFeDestinXmun.ToolTipText = "Nome do município do destinatário";
		this.olvDFeDestinUf.AspectName = "DFeDestinUf";
		this.olvDFeDestinUf.DisplayIndex = 130;
		this.olvDFeDestinUf.Text = "DFe:Destinatário Uf";
		this.olvDFeDestinUf.ToolTipText = "Sigla da UF do destinatário";
		this.olvDFeTranspId.AspectName = "DFeTranspId";
		this.olvDFeTranspId.DisplayIndex = 131;
		this.olvDFeTranspId.Text = "DFe: Transportador CNPJ/CPF";
		this.olvDFeTranspId.ToolTipText = "CNPJ/CPF do transportador";
		this.olvDFeTranspId.Width = 130;
		this.olvDFeTranspNome.AspectName = "DFeTranspNome";
		this.olvDFeTranspNome.DisplayIndex = 132;
		this.olvDFeTranspNome.Text = "DFe:Transportador Nome";
		this.olvDFeTranspNome.ToolTipText = "Nome do transportador ";
		this.olvDFeTranspNome.Width = 230;
		this.olvDFeTranspIE.AspectName = "DFeTranspIE";
		this.olvDFeTranspIE.DisplayIndex = 133;
		this.olvDFeTranspIE.Text = "DFe:Transportador IE";
		this.olvDFeTranspIE.ToolTipText = "Inscrição estadual do transportador";
		this.olvDFeTranspCmun.AspectName = "DFeTranspCmun";
		this.olvDFeTranspCmun.DisplayIndex = 134;
		this.olvDFeTranspCmun.Text = "DFe:Transportador Mun Cod";
		this.olvDFeTranspCmun.ToolTipText = "Código do município do transportador ";
		this.olvDFeTranspXmun.AspectName = "DFeTranspXmun";
		this.olvDFeTranspXmun.DisplayIndex = 135;
		this.olvDFeTranspXmun.Text = "DFe:Transportador Mun Desc";
		this.olvDFeTranspXmun.ToolTipText = "Nome do município do transportador ";
		this.olvDFeTranspUf.AspectName = "DFeTranspUf";
		this.olvDFeTranspUf.DisplayIndex = 136;
		this.olvDFeTranspUf.Text = "DFe:Transportador Uf";
		this.olvDFeTranspUf.ToolTipText = "Sigla da UF do transportador";
		this.olvDFeBaseICMS.AspectName = "DFeBaseICMS";
		this.olvDFeBaseICMS.DisplayIndex = 137;
		this.olvDFeBaseICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeBaseICMS.Text = "DFe:ICMS Base";
		this.olvDFeBaseICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeBaseICMS.ToolTipText = "Valor da BC do ICMS";
		this.olvDFeBaseICMS.Width = 80;
		this.olvDFeValorICMS.AspectName = "DFeValorICMS";
		this.olvDFeValorICMS.DisplayIndex = 138;
		this.olvDFeValorICMS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeValorICMS.Text = "DFe:ICMS Valor";
		this.olvDFeValorICMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeValorICMS.ToolTipText = "Valor do ICMS";
		this.olvDFeValorICMS.Width = 80;
		this.olvDFeVlrICMSDeson.AspectName = "DFeVlrICMSDeson";
		this.olvDFeVlrICMSDeson.DisplayIndex = 139;
		this.olvDFeVlrICMSDeson.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrICMSDeson.Text = "DFe:ICMS Desonerado";
		this.olvDFeVlrICMSDeson.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrICMSDeson.ToolTipText = "DFe:Valor Total do ICMS desonerado";
		this.olvDFeVlrICMSDeson.Width = 80;
		this.olvDFeVlrBCST.AspectName = "DFeVlrBCST";
		this.olvDFeVlrBCST.DisplayIndex = 140;
		this.olvDFeVlrBCST.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrBCST.Text = "DFe:ICMS ST Base";
		this.olvDFeVlrBCST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrBCST.ToolTipText = "DFe:Base de Cálculo do ICMS ST";
		this.olvDFeVlrBCST.Width = 80;
		this.olvDFeVlrTotST.AspectName = "DFeVlrTotST";
		this.olvDFeVlrTotST.DisplayIndex = 141;
		this.olvDFeVlrTotST.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotST.Text = "DFe:ICMS ST Valor";
		this.olvDFeVlrTotST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotST.ToolTipText = "DFe:Valor Total do ICMS ST ";
		this.olvDFeVlrTotST.Width = 80;
		this.olvDFeVlrTotProd.AspectName = "DFeVlrTotProd";
		this.olvDFeVlrTotProd.DisplayIndex = 142;
		this.olvDFeVlrTotProd.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotProd.Text = "DFe:Valor Prod";
		this.olvDFeVlrTotProd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotProd.ToolTipText = "DFe:Valor Total dos produtos e serviços";
		this.olvDFeVlrTotProd.Width = 80;
		this.olvDFeVlrTotFrete.AspectName = "DFeVlrTotFrete";
		this.olvDFeVlrTotFrete.DisplayIndex = 143;
		this.olvDFeVlrTotFrete.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotFrete.Text = "DFe:Valor Frete";
		this.olvDFeVlrTotFrete.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotFrete.ToolTipText = "DFe:Valor Total do Frete";
		this.olvDFeVlrTotFrete.Width = 80;
		this.olvDFeVlrTotSeg.AspectName = "DFeVlrTotSeg";
		this.olvDFeVlrTotSeg.DisplayIndex = 144;
		this.olvDFeVlrTotSeg.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotSeg.Text = "DFe:Valor Seguro";
		this.olvDFeVlrTotSeg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotSeg.ToolTipText = "DFe:Valor Total do Seguro";
		this.olvDFeVlrTotSeg.Width = 80;
		this.olvDFeVlrTotDesc.AspectName = "DFeVlrTotDesc";
		this.olvDFeVlrTotDesc.DisplayIndex = 145;
		this.olvDFeVlrTotDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotDesc.Text = "DFe:Valor Desc";
		this.olvDFeVlrTotDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotDesc.ToolTipText = "DFe:Valor Total do Desconto";
		this.olvDFeVlrTotDesc.Width = 80;
		this.olvVolPesoB.AspectName = "VolPesoB";
		this.olvVolPesoB.DisplayIndex = 146;
		this.olvVolPesoB.Text = "DFe: Vol.PesoBruto";
		this.olvVolPesoB.ToolTipText = "Peso Bruto (em kg)";
		this.olvDFeVlrTotII.AspectName = "DFeVlrTotII";
		this.olvDFeVlrTotII.DisplayIndex = 147;
		this.olvDFeVlrTotII.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotII.Text = "DFe:II Valor";
		this.olvDFeVlrTotII.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotII.ToolTipText = "DFe:Valor Total do II";
		this.olvDFeVlrTotII.Width = 80;
		this.olvDFeVlrTotIPI.AspectName = "DFeVlrTotIPI";
		this.olvDFeVlrTotIPI.DisplayIndex = 148;
		this.olvDFeVlrTotIPI.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotIPI.Text = "DFe:IPI Valor";
		this.olvDFeVlrTotIPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotIPI.ToolTipText = "DFe:Valor Total do IPI";
		this.olvDFeVlrTotIPI.Width = 80;
		this.olvDFeVlrTotIPIDevol.AspectName = "DFeVlrTotIPIDevol";
		this.olvDFeVlrTotIPIDevol.DisplayIndex = 149;
		this.olvDFeVlrTotIPIDevol.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotIPIDevol.Text = "DFe:IPI Valor Dev.";
		this.olvDFeVlrTotIPIDevol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotIPIDevol.ToolTipText = "DFe:Valor Total do IPI devolvDFeido";
		this.olvDFeVlrTotIPIDevol.Width = 80;
		this.olvDFeVlrTotPIS.AspectName = "DFeVlrTotPIS";
		this.olvDFeVlrTotPIS.DisplayIndex = 150;
		this.olvDFeVlrTotPIS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotPIS.Text = "DFe:PIS Valor";
		this.olvDFeVlrTotPIS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotPIS.ToolTipText = "DFe:Valor Total do PIS";
		this.olvDFeVlrTotPIS.Width = 80;
		this.olvDFeVlrTotCOFINS.AspectName = "DFeVlrTotCOFINS";
		this.olvDFeVlrTotCOFINS.DisplayIndex = 151;
		this.olvDFeVlrTotCOFINS.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotCOFINS.Text = "DFe:COFINS Valor";
		this.olvDFeVlrTotCOFINS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotCOFINS.ToolTipText = "DFe:Valor Total do COFINS";
		this.olvDFeVlrTotCOFINS.Width = 80;
		this.olvDFeVlrTotOutro.AspectName = "DFeVlrTotOutro";
		this.olvDFeVlrTotOutro.DisplayIndex = 152;
		this.olvDFeVlrTotOutro.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeVlrTotOutro.Text = "DFe:Valor Outros";
		this.olvDFeVlrTotOutro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotOutro.ToolTipText = "DFe:Outras Despesas acessórias";
		this.olvDFeVlrTotOutro.Width = 80;
		this.olvDFeVlrTotTrib.AspectName = "DFeVlrTotTrib";
		this.olvDFeVlrTotTrib.DisplayIndex = 153;
		this.olvDFeVlrTotTrib.Text = "DFe:Valor Apr Tributos";
		this.olvDFeVlrTotTrib.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDFeVlrTotTrib.ToolTipText = "DFe:Valor aproximado total de tributos federais, estaduais e municipais.";
		this.olvDFeVlrTotTrib.Width = 80;
		this.olvDFeDocNote.AspectName = "DFeDocNote";
		this.olvDFeDocNote.DisplayIndex = 154;
		this.olvDFeDocNote.Text = "DFe:Comentários";
		this.olvDFeDocNote.ToolTipText = "Comentário vinculado";
		this.olvDFeModFrete.AspectName = "DFeModFrete";
		this.olvDFeModFrete.DisplayIndex = 155;
		this.olvDFeModFrete.Text = "DFe:ModFrete";
		this.olvDFeModFrete.ToolTipText = "Modalidade de frete";
		this.olvDFeModFrete.Width = 150;
		this.olvDFeVersion.AspectName = "DFeVersion";
		this.olvDFeVersion.DisplayIndex = 156;
		this.olvDFeVersion.Text = "DFe:XML: Versão";
		this.olvDFeVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDFeDestinCEP.AspectName = "DestinCEP";
		this.olvDFeDestinCEP.DisplayIndex = 157;
		this.olvDFeDestinCEP.Text = "DFe: Destinatário CEP";
		this.olvDFeDestinCEP.ToolTipText = "CEP destinatário";
		this.olvFretePesoBruto.AspectName = "FretePesoBruto";
		this.olvFretePesoBruto.DisplayIndex = 92;
		this.olvFretePesoBruto.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoBruto.Text = "CTe:Peso Bruto : Valor";
		this.olvFretePesoBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFretePesoBruto.ToolTipText = "Peso Bruto do Mercadoria Transportadas";
		this.olvFretePesoBruto.Width = 80;
		this.olvFretePesoBrtUn.AspectName = "FretePesoBrtUn";
		this.olvFretePesoBrtUn.DisplayIndex = 93;
		this.olvFretePesoBrtUn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoBrtUn.Text = "CTe:Peso Bruto : Unidade";
		this.olvFretePesoBrtUn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoBrtUn.ToolTipText = "Unidade de Medida do Peso Bruto da Mercadoria Transportada";
		this.olvFretePesoBrtUn.Width = 80;
		this.olvFretePesoLiquido.AspectName = "FretePesoLiquido";
		this.olvFretePesoLiquido.DisplayIndex = 94;
		this.olvFretePesoLiquido.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoLiquido.Text = "CTe:Peso Líquido : Valor";
		this.olvFretePesoLiquido.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFretePesoLiquido.ToolTipText = "Peso Líquido da Mercadoria Transportada";
		this.olvFretePesoLiquido.Width = 80;
		this.olvFretePesoLiqUn.AspectName = "FretePesoLiqUn";
		this.olvFretePesoLiqUn.DisplayIndex = 95;
		this.olvFretePesoLiqUn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoLiqUn.Text = "CTe:Peso Líquido : Unidade";
		this.olvFretePesoLiqUn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFretePesoLiqUn.ToolTipText = "Unidade de Medida do Peso Líquido da Mercadoria Transportada";
		this.olvFretePesoLiqUn.Width = 80;
		this.olvFreteQuantidade.AspectName = "FreteQuantidade";
		this.olvFreteQuantidade.DisplayIndex = 96;
		this.olvFreteQuantidade.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFreteQuantidade.Text = "CTe:Quantidade : Valor";
		this.olvFreteQuantidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteQuantidade.ToolTipText = "Quantidade da Carga da Mercadoria Transportada";
		this.olvFreteQuantidade.Width = 80;
		this.olvFreteQuantidadeUn.AspectName = "FreteQuantidadeUn";
		this.olvFreteQuantidadeUn.DisplayIndex = 97;
		this.olvFreteQuantidadeUn.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFreteQuantidadeUn.Text = "CTe:Quantidade : Unidade";
		this.olvFreteQuantidadeUn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFreteQuantidadeUn.ToolTipText = "Unidade de Medida da Carga da Mercadoria Transportada";
		this.olvFreteQuantidadeUn.Width = 80;
		this.olvFreteVolTransp.AspectName = "FreteVolTransp";
		this.olvFreteVolTransp.DisplayIndex = 98;
		this.olvFreteVolTransp.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFreteVolTransp.Text = "CTe:Volume : Valor";
		this.olvFreteVolTransp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvFreteVolTransp.ToolTipText = "Unidade de Medida do Volume da Mercadoria Transportada";
		this.olvFreteVolTransp.Width = 80;
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
		this.olvBaseIcmsOutra.AspectName = "BaseICMSOutraUF";
		this.olvBaseIcmsOutra.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseIcmsOutra.IsVisible = false;
		this.olvBaseIcmsOutra.Text = "CTe:Base Icms Outra UF";
		this.olvBaseIcmsOutra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseIcmsOutra.ToolTipText = "Valor da BC do ICMS";
		this.olvValorIcmsOutra.AspectName = "ValorICMSOutraUF";
		this.olvValorIcmsOutra.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorIcmsOutra.IsVisible = false;
		this.olvValorIcmsOutra.Text = "CTe:Valor ICMS Outra UF";
		this.olvValorIcmsOutra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorIcmsOutra.ToolTipText = "Valor do ICMS devido outra UF";
		this.olvTaxaICMSOutraUF.AspectName = "TaxaICMSOutraUF";
		this.olvTaxaICMSOutraUF.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTaxaICMSOutraUF.IsVisible = false;
		this.olvTaxaICMSOutraUF.Text = "CTe:Taxa ICMS Outra UF";
		this.olvTaxaICMSOutraUF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvTaxaICMSOutraUF.ToolTipText = "Taxa do ICMS devido outra UF";
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 117);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(784, 328);
		this.pnMarket.TabIndex = 13;
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
		this.btSalesContact.TabIndex = 219;
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
		this.lknAction.Text = "CTe Dados Analíticos";
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
		this.lbText01.Text = "Apresenta a visualização de todas as informações do CTe em nível de item, identificando todas as NFe associadas a cada documento.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "CTe: Dados Analíticos é ativado nos \r\nplanos Avançado e Enterprise.";
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
		base.Name = "frmTabDocCTeDet";
		base.Load += new System.EventHandler(frmTabDocCTeDet_Load);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
