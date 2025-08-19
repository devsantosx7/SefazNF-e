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

public class frmTabDocNFeDet : Form
{
	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocNFeDet _SqlTabData;

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private Color _ColumnColor;

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private string _InfoAddXmlNFe = string.Empty;

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private decimal _TotalValue;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private string _SqlFields = " * ";

	private bool _IsFormLoaded;

	private bool _HasFiscalioConnect;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private Panel pnContent;

	private FastObjectListView lsvData;

	private OLVColumn olvSelect;

	private OLVColumn olvHasXml;

	private OLVColumn olvNum;

	private OLVColumn olvSerie;

	private OLVColumn olvTipo;

	private OLVColumn olvTag;

	private OLVColumn olvDtAut;

	private OLVColumn olvVenc;

	private OLVColumn olvStatus;

	private OLVColumn olvnItem;

	private OLVColumn olvcProd;

	private OLVColumn olvcEAN;

	private OLVColumn olvxProd;

	private OLVColumn olvNCM;

	private OLVColumn olvNVE;

	private OLVColumn olvEXTIPI;

	private OLVColumn olvCEST;

	private OLVColumn olvIndSimplesNac;

	private OLVColumn olvCFOP;

	private OLVColumn olvuCom;

	private OLVColumn olvqCom;

	private OLVColumn olvvUnCom;

	private OLVColumn olvvProd;

	private OLVColumn olvcEANTrib;

	private OLVColumn olvuTrib;

	private OLVColumn olvqTrib;

	private OLVColumn olvvUnTrib;

	private OLVColumn olvvFrete;

	private OLVColumn olvvSeg;

	private OLVColumn olvvDesc;

	private OLVColumn olvvOutro;

	private OLVColumn olvindTot;

	private OLVColumn olvICMSBase;

	private OLVColumn olvICMSTaxa;

	private OLVColumn olvICMSValor;

	private OLVColumn olvICMSCdCst;

	private OLVColumn olvICMSModBC;

	private OLVColumn olvICMSOrig;

	private OLVColumn olvICMSCSOSN;

	private OLVColumn olvICMSDeson;

	private OLVColumn olvICMSMotDes;

	private OLVColumn olvICMSFCPBase;

	private OLVColumn olvICMSFCPValor;

	private OLVColumn olvICMSSTMod;

	private OLVColumn olvICMSSTMVA;

	private OLVColumn olvICMSSTRedBase;

	private OLVColumn olvICMSSTBase;

	private OLVColumn olvICMSSTTaxa;

	private OLVColumn olvICMSSTValor;

	private OLVColumn olvICMSSTRetBase;

	private OLVColumn olvICMSSTRetValor;

	private OLVColumn olvICMSSTFCPBase;

	private OLVColumn olvICMSSTFCPTaxa;

	private OLVColumn olvICMSSTFCPValor;

	private OLVColumn olvICMSSTRetTaxa;

	private OLVColumn olvICMSSubst;

	private OLVColumn olvICMSCredSnTaxa;

	private OLVColumn olvICMSCredSnValor;

	private OLVColumn olvICMSUfDestBase;

	private OLVColumn olvICMSUfDestTaxa;

	private OLVColumn olvICMSUfIntrTaxa;

	private OLVColumn olvICMSUfDestValor;

	private OLVColumn olvICMSUfRemeValor;

	private OLVColumn olvICMSUfDestFCPBase;

	private OLVColumn olvICMSUfDestFCPTaxa;

	private OLVColumn olvICMSUfDestFCPValor;

	private OLVColumn olvIPIcEnq;

	private OLVColumn olvIPICdCst;

	private OLVColumn olvIPIBase;

	private OLVColumn olvIPITaxa;

	private OLVColumn olvIPIValor;

	private OLVColumn olvIPIDevol;

	private OLVColumn olvIIBase;

	private OLVColumn olvIIValor;

	private OLVColumn olvIIDespAdu;

	private OLVColumn olvIIIOF;

	private OLVColumn olvPISBase;

	private OLVColumn olvPISTaxa;

	private OLVColumn olvPISValor;

	private OLVColumn olvPISCdCst;

	private OLVColumn olvCOFINSBase;

	private OLVColumn olvCOFINSTaxa;

	private OLVColumn olvCOFINSValor;

	private OLVColumn olvCOFINSCdCst;

	private OLVColumn olvISSQNBase;

	private OLVColumn olvISSQNTaxa;

	private OLVColumn olvISSQNValor;

	private OLVColumn olvISSQNCdSrv;

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

	private OLVColumn olvEmitCNPJ;

	private OLVColumn olvEmitNome;

	private OLVColumn olvManifest;

	private OLVColumn olvCancel;

	private OLVColumn olvHasEvent;

	private OLVColumn olvHasCTeEvent;

	private OLVColumn olvMDFeEvtData;

	private OLVColumn olvSufVistEvtData;

	private OLVColumn olvSufVistInteData;

	private OLVColumn olvMotivo;

	private OLVColumn olvEstado;

	private OLVColumn olvChave;

	private OLVColumn olvFilial;

	private OLVColumn olvTpDoc;

	private OLVColumn olvNatOper;

	private OLVColumn olvnFCI;

	private OLVColumn olvxPed;

	private OLVColumn olvnItemPed;

	private OLVColumn olvinfAdProd;

	private OLVColumn olvvTotTrib;

	private OLVColumn olvAnoMes;

	private ImageList ImageListDocs;

	private OLVColumn olvBatchNum;

	private OLVColumn olvTpOp;

	private OLVColumn olvChassi;

	private OLVColumn olvCCor;

	private OLVColumn olvXCor;

	private OLVColumn olvPot;

	private OLVColumn olvCilin;

	private OLVColumn olvPesoL;

	private OLVColumn olvPesoB;

	private OLVColumn olvNSerie;

	private OLVColumn olvTpComb;

	private OLVColumn olvNMotor;

	private OLVColumn olvCmt;

	private OLVColumn olvDist;

	private OLVColumn olvAnoMod;

	private OLVColumn olvAnoFab;

	private OLVColumn olvTpPint;

	private OLVColumn olvTpVeic;

	private OLVColumn olvEspVeic;

	private OLVColumn olvVin;

	private OLVColumn olvCondVeic;

	private OLVColumn olvcMod;

	private OLVColumn olvCCorDENATRAN;

	private OLVColumn olvLota;

	private OLVColumn olvTpRest;

	private OLVColumn olvDtEntFisc;

	private OLVColumn olvDtRegFisc;

	private OLVColumn olvUsRegFisc;

	private OLVColumn olvDcNumFisc;

	private OLVColumn olvEmitida;

	private OLVColumn olvDocNote;

	private OLVColumn olvInfAdFisco;

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

	private OLVColumn olvICMSRedBC;

	private Button btSalesContact;

	private OLVColumn olvICMSValorDif;

	private OLVColumn olvICMSValorOper;

	private OLVColumn olvICMSTaxaDif;

	private OLVColumn olvICMSMonoRetBaseQtd;

	private OLVColumn olvICMSAdRemRetTaxa;

	private OLVColumn olvICMSAliqAdRem;

	private OLVColumn olvICMSMonoRetValor;

	private OLVColumn olvCNPJCPFTransp;

	private OLVColumn olvNomeTransp;

	private OLVColumn olvValor;

	private OLVColumn olvcProdANP;

	private OLVColumn olvComexQtdAverb;

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

	public frmTabDocNFeDet(string pTitle, Color pColumnColor, string pFeatExtId)
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

	private async void frmTabDocNFeDet_Load(object sender, EventArgs e)
	{
		_IsFormLoaded = false;
		_InfoAddXmlNFe = (await new clsDataConfig().funcGetItemByKeyAsync()).InfoAddXmlNFe;
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
		if (!_HasFiscalioConnect)
		{
			List<string> varColList = new List<string> { "DtEntFisc", "DtRegFisc", "UsRegFisc", "DcNumFisc" };
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList);
		}
		if (string.IsNullOrEmpty(_InfoAddXmlNFe))
		{
			List<string> varColList2 = new List<string>();
			varColList2.Add(clsFunction.funcClearSpecialCaracter("Informações complementares"));
			varColList2.Add(clsFunction.funcClearSpecialCaracter("Informações adicionais fisco"));
			varColList2.Add(clsFunction.funcClearSpecialCaracter("Observações contribuinte"));
			lsvData = clsScreenGeral.funcLsvDelColumn(lsvData, varColList2);
		}
		lsvData.CellToolTipShowing += olv_CellToolTipShowing;
		_SqlFields = "document.ManifCode, document.HasXml, document.Emitida, document.EmitId,";
		_SqlFields += "document.EmitNome, document.Num, document.DtAut, document.Serie, document.Model,";
		_SqlFields += "document.TpDoc, document.Tag, document.DVenc, document.DtEntFisc,";
		_SqlFields += "document.DtRegFisc, document.UsRegFisc, document.DcNumFisc,";
		_SqlFields += "document.IndSimplesNac, document.EmitIE, document.EmitcMun,";
		_SqlFields += "document.EmitxMun, document.EmitUf, document.DestinID, document.DestinIE,";
		_SqlFields += "document.DestinNome, document.DestincMun, document.DestinxMun,";
		_SqlFields += "document.DestinUf, document.Canceled, document.HasEvent,";
		_SqlFields += "document.HasCTeEvent, document.MDFeEvtData, document.SufVistEvtData,";
		_SqlFields += "document.SufInteEvtData, document.xMotivo, document.cUF,";
		_SqlFields += "document.Filial, document.NatOper, document.BatchNum, document.DocNote,";
		_SqlFields += "document.TagDtHr, document.TagUser, document.DocNoteDtHr, document.DFeSource, document.XmlSource,";
		_SqlFields += "document.DocNoteUser, document.Version, document.TranspID, document.TranspNome,document.Chave,";
		_SqlFields += "document.ComexAverbStat, docitem.veicprodtprest, docitem.veicprodlota, docitem.veicprodccordenatran,";
		_SqlFields += "docitem.veicprodcmod, docitem.veicprodcondveic, docitem.veicprodvin, docitem.veicprodespveic,";
		_SqlFields += "docitem.veicprodtpveic, docitem.veicprodtppint, docitem.veicprodanofab, docitem.veicprodanomod,";
		_SqlFields += "docitem.veicproddist, docitem.veicprodcmt, docitem.veicprodnmotor, docitem.veicprodtpcomb,";
		_SqlFields += "docitem.veicprodnserie, docitem.veicprodpesob, docitem.veicprodpesol, docitem.veicprodcilin,";
		_SqlFields += "docitem.veicprodpot, docitem.veicprodxcor, docitem.veicprodccor, docitem.issqnvalor,";
		_SqlFields += "docitem.issqntaxa, docitem.issqnbase, docitem.cofinscdcst, docitem.cofinsvalor,";
		_SqlFields += "docitem.cofinstaxa, docitem.cofinsbase, docitem.piscdcst, docitem.pisvalor,";
		_SqlFields += "docitem.pistaxa, docitem.pisbase, docitem.iiiof, docitem.iidespadu, docitem.iivalor,";
		_SqlFields += "docitem.iibase, docitem.ipidevol, docitem.ipivalor, docitem.ipitaxa, docitem.ipibase,";
		_SqlFields += "docitem.ipicdcst, docitem.ipicenq, docitem.icmsufdestfcpvalor, docitem.icmsufdestfcptaxa,";
		_SqlFields += "docitem.icmsufdestfcpbase, docitem.icmsufremevalor, docitem.icmsufdestvalor,";
		_SqlFields += "docitem.icmsufintrtaxa, docitem.icmsufdesttaxa, docitem.icmsufdestbase,";
		_SqlFields += "docitem.icmscredsnvalor, docitem.icmscredsntaxa, docitem.icmsstfcpvalor,";
		_SqlFields += "docitem.icmsstfcptaxa, docitem.icmsstfcpbase, docitem.icmsstsubvalor,";
		_SqlFields += "docitem.icmsstrettaxa, docitem.icmsstretvalor, docitem.icmsstretbase,";
		_SqlFields += "docitem.icmsstvalor, docitem.icmssttaxa, docitem.icmsstbase, docitem.icmsredbc,";
		_SqlFields += "docitem.icmsstredbase,docitem.icmsstmva, docitem.icmsstmod, docitem.icmsfcpvalor,";
		_SqlFields += "docitem.icmsfcpbase, docitem.icmsmonoretvalor,docitem.icmsadremrettaxa,";
		_SqlFields += "docitem.icmsaliqadrem, docitem.icmsmonoretbaseqtd, docitem.icmstaxadif,";
		_SqlFields += "docitem.icmsvaloroper, docitem.icmsvalordif,";
		_SqlFields += "docitem.icmsmotdes, docitem.icmsdeson, docitem.icmscsosn, docitem.icmsorig,";
		_SqlFields += "docitem.icmsmodbc, docitem.icmscdcst, docitem.icmsvalor, docitem.icmstaxa,";
		_SqlFields += "docitem.icmsbase, docitem.vtottrib, docitem.infadprod, docitem.nitemped, docitem.xped,";
		_SqlFields += "docitem.nfci, docitem.indtot, docitem.voutro, docitem.vdesc, docitem.vseg,";
		_SqlFields += "docitem.vfrete, docitem.vuntrib, docitem.qtrib, docitem.utrib, docitem.ceantrib,";
		_SqlFields += "docitem.vprod, docitem.vuncom, docitem.qcom, docitem.ucom, docitem.cfop,";
		_SqlFields += "docitem.cest, docitem.extipi, docitem.nve, docitem.ncm, docitem.xprod,";
		_SqlFields += "docitem.cean, docitem.cprod, docitem.nitem, docitem.cProdANP, docitem.ComexQtdAverb";
		_SqlTabData = new clsSrvTabDocNFeDet(_SqlFields);
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
		_IsFormLoaded = true;
	}

	private void funcDefineListViewFeatures()
	{
		olvHasXml.ImageGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(docDocItem, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
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
		olvNum.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcGetValue(docDocItem.Num).PadLeft(9, '0');
		};
		olvTipo.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcGetDocType(docDocItem.Model);
		};
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsScreenGeral.funcGetDocStatIcon(docDocItem);
		};
		olvInfCpl.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXmlNFe) ? null : clsScreenGeral.funcGetDocText(docDocItem.Chave)?.infCpl;
		};
		olvObsCont.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXmlNFe) ? null : clsScreenGeral.funcGetDocText(docDocItem.Chave)?.XObsCont;
		};
		olvInfAdFisco.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			return string.IsNullOrEmpty(_InfoAddXmlNFe) ? null : clsScreenGeral.funcGetDocText(docDocItem.Chave)?.infAdFisco;
		};
		olvManifest.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.GetEventDesc(docDocItem.Model, docDocItem.ManifCode);
		};
		olvEmitCNPJ.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcFormatDoc(docDocItem.EmitID);
		};
		olvEmitNome.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsFunction.funcFormatDoc(docDocItem.EmitNome);
		};
		olvEstado.AspectGetter = delegate(object x)
		{
			DocDocItem varDocument = (DocDocItem)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
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
		olvTpDoc.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : _clsDFeCodes.funcGetTipoDoc(docDocItem.Model, docDocItem.TpDoc);
		};
		olvDestinIE.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			return (docDocItem == null) ? null : clsSrvGeral.funcGetIEFormat(docDocItem.DestinIE);
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
		olvValor.AspectGetter = delegate(object x)
		{
			DocDocItem docDocItem = (DocDocItem)x;
			if (docDocItem == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = docDocItem.funcGetValor().ToString();
			}
			catch
			{
			}
			return result;
		};
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
		while (!_IsFormLoaded)
		{
			await Task.Delay(TimeSpan.FromSeconds(1.0));
		}
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
		OLVColumn varGroupColum;
		SortOrder varSortOrder;
		if (pclsDataFilter.GroupByDisable || _IsLocked)
		{
			varGroupColum = null;
			varSortOrder = SortOrder.None;
		}
		else if (pclsDataFilter.GroupByPartner)
		{
			varGroupColum = olvEmitNome;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByDate)
		{
			varGroupColum = olvDtAut;
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
			varGroupColum = olvTipo;
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
			varGroupColum = olvCFOP;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByFilial)
		{
			varGroupColum = olvFilial;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByTomaIE)
		{
			varGroupColum = olvDestinIE;
			varSortOrder = SortOrder.Ascending;
		}
		else
		{
			varGroupColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		plsvData = clsScreenGeral.funcSetLsvSortGroup(this, plsvData, varGroupColum, varSortOrder, olvDtAut, SortOrder.Descending);
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
			_TotalValue = varObjList.Sum((DocDocItem r) => clsFunction.funcConvStrToDec(r.funcGetValor().ToString()));
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
		clsHelpService.funcCallTipReportNFeAnalyticalAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocNFeDet));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.pnContent = new System.Windows.Forms.Panel();
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitida = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitCNPJ = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitNome = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvVenc = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEntFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDtRegFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvUsRegFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvDcNumFisc = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvnItem = new BrightIdeasSoftware.OLVColumn();
		this.olvcProd = new BrightIdeasSoftware.OLVColumn();
		this.olvcEAN = new BrightIdeasSoftware.OLVColumn();
		this.olvxProd = new BrightIdeasSoftware.OLVColumn();
		this.olvNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvNVE = new BrightIdeasSoftware.OLVColumn();
		this.olvEXTIPI = new BrightIdeasSoftware.OLVColumn();
		this.olvCEST = new BrightIdeasSoftware.OLVColumn();
		this.olvIndSimplesNac = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvuCom = new BrightIdeasSoftware.OLVColumn();
		this.olvqCom = new BrightIdeasSoftware.OLVColumn();
		this.olvvUnCom = new BrightIdeasSoftware.OLVColumn();
		this.olvvProd = new BrightIdeasSoftware.OLVColumn();
		this.olvcEANTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvuTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvqTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvvUnTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvComexQtdAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvvFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvvSeg = new BrightIdeasSoftware.OLVColumn();
		this.olvvDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvvOutro = new BrightIdeasSoftware.OLVColumn();
		this.olvvTotTrib = new BrightIdeasSoftware.OLVColumn();
		this.olvindTot = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSRedBC = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSCdCst = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSModBC = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSOrig = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSCSOSN = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSDeson = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSMotDes = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSFCPBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSFCPValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTMod = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTMVA = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTRedBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTFCPBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTFCPTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTFCPValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTRetBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTRetValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSTRetTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSSubst = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSValorDif = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSValorOper = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSTaxaDif = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSMonoRetBaseQtd = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSMonoRetValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSAdRemRetTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSAliqAdRem = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSCredSnTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSCredSnValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfDestBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfDestTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfIntrTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfDestValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfRemeValor = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfDestFCPBase = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfDestFCPTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvICMSUfDestFCPValor = new BrightIdeasSoftware.OLVColumn();
		this.olvIPIcEnq = new BrightIdeasSoftware.OLVColumn();
		this.olvIPICdCst = new BrightIdeasSoftware.OLVColumn();
		this.olvIPIBase = new BrightIdeasSoftware.OLVColumn();
		this.olvIPITaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvIPIValor = new BrightIdeasSoftware.OLVColumn();
		this.olvIPIDevol = new BrightIdeasSoftware.OLVColumn();
		this.olvIIBase = new BrightIdeasSoftware.OLVColumn();
		this.olvIIValor = new BrightIdeasSoftware.OLVColumn();
		this.olvIIDespAdu = new BrightIdeasSoftware.OLVColumn();
		this.olvIIIOF = new BrightIdeasSoftware.OLVColumn();
		this.olvPISBase = new BrightIdeasSoftware.OLVColumn();
		this.olvPISTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvPISValor = new BrightIdeasSoftware.OLVColumn();
		this.olvPISCdCst = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSBase = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSValor = new BrightIdeasSoftware.OLVColumn();
		this.olvCOFINSCdCst = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNBase = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNTaxa = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNValor = new BrightIdeasSoftware.OLVColumn();
		this.olvISSQNCdSrv = new BrightIdeasSoftware.OLVColumn();
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
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvnFCI = new BrightIdeasSoftware.OLVColumn();
		this.olvxPed = new BrightIdeasSoftware.OLVColumn();
		this.olvnItemPed = new BrightIdeasSoftware.OLVColumn();
		this.olvcProdANP = new BrightIdeasSoftware.OLVColumn();
		this.olvinfAdProd = new BrightIdeasSoftware.OLVColumn();
		this.olvBatchNum = new BrightIdeasSoftware.OLVColumn();
		this.olvTpOp = new BrightIdeasSoftware.OLVColumn();
		this.olvChassi = new BrightIdeasSoftware.OLVColumn();
		this.olvCCor = new BrightIdeasSoftware.OLVColumn();
		this.olvXCor = new BrightIdeasSoftware.OLVColumn();
		this.olvPot = new BrightIdeasSoftware.OLVColumn();
		this.olvCilin = new BrightIdeasSoftware.OLVColumn();
		this.olvPesoL = new BrightIdeasSoftware.OLVColumn();
		this.olvPesoB = new BrightIdeasSoftware.OLVColumn();
		this.olvNSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvTpComb = new BrightIdeasSoftware.OLVColumn();
		this.olvNMotor = new BrightIdeasSoftware.OLVColumn();
		this.olvCmt = new BrightIdeasSoftware.OLVColumn();
		this.olvDist = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMod = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoFab = new BrightIdeasSoftware.OLVColumn();
		this.olvTpPint = new BrightIdeasSoftware.OLVColumn();
		this.olvTpVeic = new BrightIdeasSoftware.OLVColumn();
		this.olvEspVeic = new BrightIdeasSoftware.OLVColumn();
		this.olvVin = new BrightIdeasSoftware.OLVColumn();
		this.olvCondVeic = new BrightIdeasSoftware.OLVColumn();
		this.olvcMod = new BrightIdeasSoftware.OLVColumn();
		this.olvCCorDENATRAN = new BrightIdeasSoftware.OLVColumn();
		this.olvLota = new BrightIdeasSoftware.OLVColumn();
		this.olvTpRest = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvInfAdFisco = new BrightIdeasSoftware.OLVColumn();
		this.olvInfCpl = new BrightIdeasSoftware.OLVColumn();
		this.olvTagDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvTagUser = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteUser = new BrightIdeasSoftware.OLVColumn();
		this.olvVersion = new BrightIdeasSoftware.OLVColumn();
		this.olvCNPJCPFTransp = new BrightIdeasSoftware.OLVColumn();
		this.olvNomeTransp = new BrightIdeasSoftware.OLVColumn();
		this.olvValor = new BrightIdeasSoftware.OLVColumn();
		this.olvObsCont = new BrightIdeasSoftware.OLVColumn();
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
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvHasXml);
		this.lsvData.AllColumns.Add(this.olvEmitida);
		this.lsvData.AllColumns.Add(this.olvEmitCNPJ);
		this.lsvData.AllColumns.Add(this.olvEmitNome);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvTpDoc);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvDtAut);
		this.lsvData.AllColumns.Add(this.olvVenc);
		this.lsvData.AllColumns.Add(this.olvDtEntFisc);
		this.lsvData.AllColumns.Add(this.olvDtRegFisc);
		this.lsvData.AllColumns.Add(this.olvUsRegFisc);
		this.lsvData.AllColumns.Add(this.olvDcNumFisc);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvnItem);
		this.lsvData.AllColumns.Add(this.olvcProd);
		this.lsvData.AllColumns.Add(this.olvcEAN);
		this.lsvData.AllColumns.Add(this.olvxProd);
		this.lsvData.AllColumns.Add(this.olvNCM);
		this.lsvData.AllColumns.Add(this.olvNVE);
		this.lsvData.AllColumns.Add(this.olvEXTIPI);
		this.lsvData.AllColumns.Add(this.olvCEST);
		this.lsvData.AllColumns.Add(this.olvIndSimplesNac);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvuCom);
		this.lsvData.AllColumns.Add(this.olvqCom);
		this.lsvData.AllColumns.Add(this.olvvUnCom);
		this.lsvData.AllColumns.Add(this.olvvProd);
		this.lsvData.AllColumns.Add(this.olvcEANTrib);
		this.lsvData.AllColumns.Add(this.olvuTrib);
		this.lsvData.AllColumns.Add(this.olvqTrib);
		this.lsvData.AllColumns.Add(this.olvvUnTrib);
		this.lsvData.AllColumns.Add(this.olvComexQtdAverb);
		this.lsvData.AllColumns.Add(this.olvvFrete);
		this.lsvData.AllColumns.Add(this.olvvSeg);
		this.lsvData.AllColumns.Add(this.olvvDesc);
		this.lsvData.AllColumns.Add(this.olvvOutro);
		this.lsvData.AllColumns.Add(this.olvvTotTrib);
		this.lsvData.AllColumns.Add(this.olvindTot);
		this.lsvData.AllColumns.Add(this.olvICMSBase);
		this.lsvData.AllColumns.Add(this.olvICMSTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSRedBC);
		this.lsvData.AllColumns.Add(this.olvICMSValor);
		this.lsvData.AllColumns.Add(this.olvICMSCdCst);
		this.lsvData.AllColumns.Add(this.olvICMSModBC);
		this.lsvData.AllColumns.Add(this.olvICMSOrig);
		this.lsvData.AllColumns.Add(this.olvICMSCSOSN);
		this.lsvData.AllColumns.Add(this.olvICMSDeson);
		this.lsvData.AllColumns.Add(this.olvICMSMotDes);
		this.lsvData.AllColumns.Add(this.olvICMSFCPBase);
		this.lsvData.AllColumns.Add(this.olvICMSFCPValor);
		this.lsvData.AllColumns.Add(this.olvICMSSTMod);
		this.lsvData.AllColumns.Add(this.olvICMSSTMVA);
		this.lsvData.AllColumns.Add(this.olvICMSSTRedBase);
		this.lsvData.AllColumns.Add(this.olvICMSSTBase);
		this.lsvData.AllColumns.Add(this.olvICMSSTTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSSTValor);
		this.lsvData.AllColumns.Add(this.olvICMSSTFCPBase);
		this.lsvData.AllColumns.Add(this.olvICMSSTFCPTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSSTFCPValor);
		this.lsvData.AllColumns.Add(this.olvICMSSTRetBase);
		this.lsvData.AllColumns.Add(this.olvICMSSTRetValor);
		this.lsvData.AllColumns.Add(this.olvICMSSTRetTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSSubst);
		this.lsvData.AllColumns.Add(this.olvICMSValorDif);
		this.lsvData.AllColumns.Add(this.olvICMSValorOper);
		this.lsvData.AllColumns.Add(this.olvICMSTaxaDif);
		this.lsvData.AllColumns.Add(this.olvICMSMonoRetBaseQtd);
		this.lsvData.AllColumns.Add(this.olvICMSMonoRetValor);
		this.lsvData.AllColumns.Add(this.olvICMSAdRemRetTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSAliqAdRem);
		this.lsvData.AllColumns.Add(this.olvICMSCredSnTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSCredSnValor);
		this.lsvData.AllColumns.Add(this.olvICMSUfDestBase);
		this.lsvData.AllColumns.Add(this.olvICMSUfDestTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSUfIntrTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSUfDestValor);
		this.lsvData.AllColumns.Add(this.olvICMSUfRemeValor);
		this.lsvData.AllColumns.Add(this.olvICMSUfDestFCPBase);
		this.lsvData.AllColumns.Add(this.olvICMSUfDestFCPTaxa);
		this.lsvData.AllColumns.Add(this.olvICMSUfDestFCPValor);
		this.lsvData.AllColumns.Add(this.olvIPIcEnq);
		this.lsvData.AllColumns.Add(this.olvIPICdCst);
		this.lsvData.AllColumns.Add(this.olvIPIBase);
		this.lsvData.AllColumns.Add(this.olvIPITaxa);
		this.lsvData.AllColumns.Add(this.olvIPIValor);
		this.lsvData.AllColumns.Add(this.olvIPIDevol);
		this.lsvData.AllColumns.Add(this.olvIIBase);
		this.lsvData.AllColumns.Add(this.olvIIValor);
		this.lsvData.AllColumns.Add(this.olvIIDespAdu);
		this.lsvData.AllColumns.Add(this.olvIIIOF);
		this.lsvData.AllColumns.Add(this.olvPISBase);
		this.lsvData.AllColumns.Add(this.olvPISTaxa);
		this.lsvData.AllColumns.Add(this.olvPISValor);
		this.lsvData.AllColumns.Add(this.olvPISCdCst);
		this.lsvData.AllColumns.Add(this.olvCOFINSBase);
		this.lsvData.AllColumns.Add(this.olvCOFINSTaxa);
		this.lsvData.AllColumns.Add(this.olvCOFINSValor);
		this.lsvData.AllColumns.Add(this.olvCOFINSCdCst);
		this.lsvData.AllColumns.Add(this.olvISSQNBase);
		this.lsvData.AllColumns.Add(this.olvISSQNTaxa);
		this.lsvData.AllColumns.Add(this.olvISSQNValor);
		this.lsvData.AllColumns.Add(this.olvISSQNCdSrv);
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
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvnFCI);
		this.lsvData.AllColumns.Add(this.olvxPed);
		this.lsvData.AllColumns.Add(this.olvnItemPed);
		this.lsvData.AllColumns.Add(this.olvcProdANP);
		this.lsvData.AllColumns.Add(this.olvinfAdProd);
		this.lsvData.AllColumns.Add(this.olvBatchNum);
		this.lsvData.AllColumns.Add(this.olvTpOp);
		this.lsvData.AllColumns.Add(this.olvChassi);
		this.lsvData.AllColumns.Add(this.olvCCor);
		this.lsvData.AllColumns.Add(this.olvXCor);
		this.lsvData.AllColumns.Add(this.olvPot);
		this.lsvData.AllColumns.Add(this.olvCilin);
		this.lsvData.AllColumns.Add(this.olvPesoL);
		this.lsvData.AllColumns.Add(this.olvPesoB);
		this.lsvData.AllColumns.Add(this.olvNSerie);
		this.lsvData.AllColumns.Add(this.olvTpComb);
		this.lsvData.AllColumns.Add(this.olvNMotor);
		this.lsvData.AllColumns.Add(this.olvCmt);
		this.lsvData.AllColumns.Add(this.olvDist);
		this.lsvData.AllColumns.Add(this.olvAnoMod);
		this.lsvData.AllColumns.Add(this.olvAnoFab);
		this.lsvData.AllColumns.Add(this.olvTpPint);
		this.lsvData.AllColumns.Add(this.olvTpVeic);
		this.lsvData.AllColumns.Add(this.olvEspVeic);
		this.lsvData.AllColumns.Add(this.olvVin);
		this.lsvData.AllColumns.Add(this.olvCondVeic);
		this.lsvData.AllColumns.Add(this.olvcMod);
		this.lsvData.AllColumns.Add(this.olvCCorDENATRAN);
		this.lsvData.AllColumns.Add(this.olvLota);
		this.lsvData.AllColumns.Add(this.olvTpRest);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvInfAdFisco);
		this.lsvData.AllColumns.Add(this.olvInfCpl);
		this.lsvData.AllColumns.Add(this.olvTagDtHr);
		this.lsvData.AllColumns.Add(this.olvTagUser);
		this.lsvData.AllColumns.Add(this.olvDocNoteDtHr);
		this.lsvData.AllColumns.Add(this.olvDocNoteUser);
		this.lsvData.AllColumns.Add(this.olvVersion);
		this.lsvData.AllColumns.Add(this.olvCNPJCPFTransp);
		this.lsvData.AllColumns.Add(this.olvNomeTransp);
		this.lsvData.AllColumns.Add(this.olvValor);
		this.lsvData.AllColumns.Add(this.olvObsCont);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[169]
		{
			this.olvSelect, this.olvAnoMes, this.olvHasXml, this.olvEmitida, this.olvEmitCNPJ, this.olvEmitNome, this.olvNum, this.olvSerie, this.olvTipo, this.olvTpDoc,
			this.olvTag, this.olvDtAut, this.olvVenc, this.olvDtEntFisc, this.olvDtRegFisc, this.olvUsRegFisc, this.olvDcNumFisc, this.olvStatus, this.olvnItem, this.olvcProd,
			this.olvcEAN, this.olvxProd, this.olvNCM, this.olvNVE, this.olvEXTIPI, this.olvCEST, this.olvIndSimplesNac, this.olvCFOP, this.olvuCom, this.olvqCom,
			this.olvvUnCom, this.olvvProd, this.olvcEANTrib, this.olvuTrib, this.olvqTrib, this.olvvUnTrib, this.olvComexQtdAverb, this.olvvFrete, this.olvvSeg, this.olvvDesc,
			this.olvvOutro, this.olvvTotTrib, this.olvindTot, this.olvICMSBase, this.olvICMSTaxa, this.olvICMSRedBC, this.olvICMSValor, this.olvICMSCdCst, this.olvICMSModBC, this.olvICMSOrig,
			this.olvICMSCSOSN, this.olvICMSDeson, this.olvICMSMotDes, this.olvICMSFCPBase, this.olvICMSFCPValor, this.olvICMSSTMod, this.olvICMSSTMVA, this.olvICMSSTRedBase, this.olvICMSSTBase, this.olvICMSSTTaxa,
			this.olvICMSSTValor, this.olvICMSSTFCPBase, this.olvICMSSTFCPTaxa, this.olvICMSSTFCPValor, this.olvICMSSTRetBase, this.olvICMSSTRetValor, this.olvICMSSTRetTaxa, this.olvICMSSubst, this.olvICMSValorDif, this.olvICMSValorOper,
			this.olvICMSTaxaDif, this.olvICMSMonoRetBaseQtd, this.olvICMSMonoRetValor, this.olvICMSAdRemRetTaxa, this.olvICMSAliqAdRem, this.olvICMSCredSnTaxa, this.olvICMSCredSnValor, this.olvICMSUfDestBase, this.olvICMSUfDestTaxa, this.olvICMSUfIntrTaxa,
			this.olvICMSUfDestValor, this.olvICMSUfRemeValor, this.olvICMSUfDestFCPBase, this.olvICMSUfDestFCPTaxa, this.olvICMSUfDestFCPValor, this.olvIPIcEnq, this.olvIPICdCst, this.olvIPIBase, this.olvIPITaxa, this.olvIPIValor,
			this.olvIPIDevol, this.olvIIBase, this.olvIIValor, this.olvIIDespAdu, this.olvIIIOF, this.olvPISBase, this.olvPISTaxa, this.olvPISValor, this.olvPISCdCst, this.olvCOFINSBase,
			this.olvCOFINSTaxa, this.olvCOFINSValor, this.olvCOFINSCdCst, this.olvISSQNBase, this.olvISSQNTaxa, this.olvISSQNValor, this.olvISSQNCdSrv, this.olvEmitIE, this.olvEmitcMun, this.olvEmitxMun,
			this.olvEmitUf, this.olvDestinId, this.olvDestinIE, this.olvDestinNome, this.olvDestincMun, this.olvDestinxMun, this.olvDestinUf, this.olvManifest, this.olvCancel, this.olvHasEvent,
			this.olvHasCTeEvent, this.olvMDFeEvtData, this.olvSufVistEvtData, this.olvSufVistInteData, this.olvMotivo, this.olvEstado, this.olvChave, this.olvFilial, this.olvNatOper, this.olvnFCI,
			this.olvxPed, this.olvnItemPed, this.olvcProdANP, this.olvinfAdProd, this.olvBatchNum, this.olvTpOp, this.olvChassi, this.olvCCor, this.olvXCor, this.olvPot,
			this.olvCilin, this.olvPesoL, this.olvPesoB, this.olvNSerie, this.olvTpComb, this.olvNMotor, this.olvCmt, this.olvDist, this.olvAnoMod, this.olvAnoFab,
			this.olvTpPint, this.olvTpVeic, this.olvEspVeic, this.olvVin, this.olvCondVeic, this.olvcMod, this.olvCCorDENATRAN, this.olvLota, this.olvTpRest, this.olvDocNote,
			this.olvInfAdFisco, this.olvInfCpl, this.olvTagDtHr, this.olvTagUser, this.olvDocNoteDtHr, this.olvDocNoteUser, this.olvVersion, this.olvValor, this.olvObsCont
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
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Data de autorização";
		this.olvHasXml.AspectName = "HasXml";
		this.olvHasXml.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.IsEditable = false;
		this.olvHasXml.Text = "XML";
		this.olvHasXml.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasXml.ToolTipText = "Tem evento XML";
		this.olvHasXml.Width = 36;
		this.olvEmitida.AspectName = "Emitida";
		this.olvEmitida.Text = "Emissor";
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
		this.olvSerie.ToolTipText = "Série do documento fiscal";
		this.olvSerie.Width = 44;
		this.olvTipo.AspectName = "Model";
		this.olvTipo.Text = "Tipo";
		this.olvTipo.ToolTipText = "Modelo do documento fiscal";
		this.olvTipo.Width = 43;
		this.olvTpDoc.AspectName = "TpDoc";
		this.olvTpDoc.Text = "TipoDoc";
		this.olvTpDoc.ToolTipText = "Tipo de documento ";
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.Width = 29;
		this.olvDtAut.AspectName = "DtAut";
		this.olvDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.Text = "Data";
		this.olvDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.ToolTipText = "Data de autorização";
		this.olvDtAut.Width = 71;
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
		this.olvStatus.UseFiltering = false;
		this.olvStatus.Width = 77;
		this.olvnItem.AspectName = "nItem";
		this.olvnItem.Text = "Item";
		this.olvnItem.ToolTipText = "Número do item";
		this.olvcProd.AspectName = "cProd";
		this.olvcProd.Text = "Produto";
		this.olvcProd.ToolTipText = "Descrição do produto ou serviço";
		this.olvcEAN.AspectName = "cEAN";
		this.olvcEAN.Text = "EAN/GTIN";
		this.olvcEAN.ToolTipText = "GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras";
		this.olvxProd.AspectName = "xProd";
		this.olvxProd.Text = "Descrição de produto ou serviço";
		this.olvxProd.ToolTipText = "Descrição do produto ou serviço ";
		this.olvNCM.AspectName = "NCM";
		this.olvNCM.Text = "NCM";
		this.olvNCM.ToolTipText = "Nomenclatura comum do mercosul";
		this.olvNVE.AspectName = "NVE";
		this.olvNVE.Text = "NVE";
		this.olvNVE.ToolTipText = "Codificação NVE - Nomenclatura de Valor Aduaneiro e Estatística.";
		this.olvEXTIPI.AspectName = "EXTIPI";
		this.olvEXTIPI.Text = "EXTIPI";
		this.olvEXTIPI.ToolTipText = "EX_TIPI";
		this.olvCEST.AspectName = "CEST";
		this.olvCEST.Text = "CEST";
		this.olvCEST.ToolTipText = "Código Especificador da Substituição Tributário";
		this.olvIndSimplesNac.AspectName = "IndSimplesNac";
		this.olvIndSimplesNac.Text = "Simples";
		this.olvIndSimplesNac.ToolTipText = "Indica se o contribuinte é simples nacional";
		this.olvCFOP.AspectName = "CFOP";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.ToolTipText = "Código Fiscal de Operações e Prestações";
		this.olvuCom.AspectName = "uCom";
		this.olvuCom.Text = "UnCom";
		this.olvuCom.ToolTipText = "Unidade Comercial";
		this.olvqCom.AspectName = "qCom";
		this.olvqCom.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvqCom.Text = "QtdCom";
		this.olvqCom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvqCom.ToolTipText = "Quantidade Comercial";
		this.olvqCom.Width = 100;
		this.olvvUnCom.AspectName = "vUnCom";
		this.olvvUnCom.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvUnCom.Text = "VlrUnit";
		this.olvvUnCom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvUnCom.ToolTipText = "Valor Unitário de Comercialização";
		this.olvvProd.AspectName = "vProd";
		this.olvvProd.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvProd.Text = "VlrPrd";
		this.olvvProd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvProd.ToolTipText = "Valor Total Bruto dos Produtos ou Serviços";
		this.olvvProd.Width = 100;
		this.olvcEANTrib.AspectName = "cEANTrib";
		this.olvcEANTrib.Text = "EANTrib";
		this.olvcEANTrib.ToolTipText = "GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras";
		this.olvuTrib.AspectName = "uTrib";
		this.olvuTrib.Text = "UnTrib";
		this.olvuTrib.ToolTipText = "Unidade Tributável";
		this.olvqTrib.AspectName = "qTrib";
		this.olvqTrib.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvqTrib.Text = "QtdTrib";
		this.olvqTrib.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvqTrib.ToolTipText = "Unidade Tributável";
		this.olvqTrib.Width = 100;
		this.olvvUnTrib.AspectName = "vUnTrib";
		this.olvvUnTrib.Text = "UnTrib";
		this.olvvUnTrib.ToolTipText = "Valor Unitário de tributação";
		this.olvComexQtdAverb.AspectName = "ComexQtdAverb";
		this.olvComexQtdAverb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvComexQtdAverb.Text = "QtdAverb";
		this.olvComexQtdAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvComexQtdAverb.ToolTipText = "Siscomex : Quantidade Averbada";
		this.olvvFrete.AspectName = "vFrete";
		this.olvvFrete.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvFrete.Text = "VlrFrete";
		this.olvvFrete.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvFrete.ToolTipText = "Valor Total do Frete";
		this.olvvFrete.Width = 100;
		this.olvvSeg.AspectName = "vSeg";
		this.olvvSeg.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvSeg.Text = "VlrSeg";
		this.olvvSeg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvSeg.ToolTipText = "Valor Total do Seguro";
		this.olvvSeg.Width = 100;
		this.olvvDesc.AspectName = "vDesc";
		this.olvvDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvDesc.Text = "VlrDesc";
		this.olvvDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvDesc.ToolTipText = "Valor do Desconto";
		this.olvvDesc.Width = 100;
		this.olvvOutro.AspectName = "vOutro";
		this.olvvOutro.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvOutro.Text = "VlrOutros";
		this.olvvOutro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvOutro.ToolTipText = "Indica se valor do Item (vProd) entra no valor total da NF-e (vProd)";
		this.olvvOutro.Width = 100;
		this.olvvTotTrib.AspectName = "vTotTrib";
		this.olvvTotTrib.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvvTotTrib.Text = "Valor Apr Tributos";
		this.olvvTotTrib.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvvTotTrib.ToolTipText = "Valor aproximado total de tributos federais, estaduais e municipais.";
		this.olvvTotTrib.Width = 100;
		this.olvindTot.AspectName = "indTot";
		this.olvindTot.Text = "IndTotal";
		this.olvindTot.ToolTipText = "Indica se valor do Item (vProd) entra no valor total da NF-e (vProd)";
		this.olvICMSBase.AspectName = "ICMSBase";
		this.olvICMSBase.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSBase.Text = "ICMS Base";
		this.olvICMSBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSBase.ToolTipText = "Base do ICMS";
		this.olvICMSBase.Width = 100;
		this.olvICMSTaxa.AspectName = "ICMSTaxa";
		this.olvICMSTaxa.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSTaxa.Text = "ICMS Taxa";
		this.olvICMSTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSTaxa.ToolTipText = "Base da taxa do ICMS";
		this.olvICMSTaxa.Width = 100;
		this.olvICMSRedBC.AspectName = "ICMSRedBC";
		this.olvICMSRedBC.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSRedBC.Text = "ICMS Red BC";
		this.olvICMSRedBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSRedBC.ToolTipText = "Percentual da Redução de Base";
		this.olvICMSRedBC.Width = 100;
		this.olvICMSValor.AspectName = "ICMSValor";
		this.olvICMSValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSValor.Text = "ICMS Valor";
		this.olvICMSValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSValor.ToolTipText = "Valor do ICMS";
		this.olvICMSValor.Width = 100;
		this.olvICMSCdCst.AspectName = "ICMSCdCst";
		this.olvICMSCdCst.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSCdCst.Text = "ICMS CST";
		this.olvICMSCdCst.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSCdCst.ToolTipText = "Classificação Tributária do serviço";
		this.olvICMSCdCst.Width = 100;
		this.olvICMSModBC.AspectName = "ICMSModBC";
		this.olvICMSModBC.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSModBC.Text = "ICMS ModBc";
		this.olvICMSModBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSModBC.ToolTipText = "Modalidade de determinação da BC do ICMS";
		this.olvICMSModBC.Width = 100;
		this.olvICMSOrig.AspectName = "ICMSOrig";
		this.olvICMSOrig.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSOrig.Text = "ICMS Origem";
		this.olvICMSOrig.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSOrig.ToolTipText = "Origem do mercadoria";
		this.olvICMSOrig.Width = 100;
		this.olvICMSCSOSN.AspectName = "ICMSCSOSN";
		this.olvICMSCSOSN.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSCSOSN.Text = "ICMS CSOSN";
		this.olvICMSCSOSN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSCSOSN.ToolTipText = "Código de Situação da operação no simples nacional";
		this.olvICMSCSOSN.Width = 100;
		this.olvICMSDeson.AspectName = "ICMSDeson";
		this.olvICMSDeson.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSDeson.Text = "ICMS Desonerado";
		this.olvICMSDeson.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSDeson.ToolTipText = "Valor do ICMS desonerado";
		this.olvICMSDeson.Width = 100;
		this.olvICMSMotDes.AspectName = "ICMSMotDes";
		this.olvICMSMotDes.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSMotDes.Text = "ICMS Motivo da desoneração";
		this.olvICMSMotDes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSMotDes.ToolTipText = "Motivo da desoneração do ICMS";
		this.olvICMSMotDes.Width = 100;
		this.olvICMSFCPBase.AspectName = "ICMSFCPBase";
		this.olvICMSFCPBase.Text = "ICMS FCP Base";
		this.olvICMSFCPBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSFCPBase.ToolTipText = "Base fundo de combate à pobreza";
		this.olvICMSFCPBase.Width = 100;
		this.olvICMSFCPValor.AspectName = "ICMSFCPValor";
		this.olvICMSFCPValor.Text = "ICMS FCP Valor";
		this.olvICMSFCPValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSFCPValor.ToolTipText = "Valor do fundo de combate à pobreza";
		this.olvICMSFCPValor.Width = 100;
		this.olvICMSSTMod.AspectName = "ICMSSTMod";
		this.olvICMSSTMod.Text = "ICMS ST Modalidade";
		this.olvICMSSTMod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvICMSSTMod.ToolTipText = "Valor do ICMS ST";
		this.olvICMSSTMod.Width = 100;
		this.olvICMSSTMVA.AspectName = "ICMSSTMVA";
		this.olvICMSSTMVA.Text = "ICMS ST MVA";
		this.olvICMSSTMVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTMVA.ToolTipText = "Margem de Valor Agregada";
		this.olvICMSSTMVA.Width = 100;
		this.olvICMSSTRedBase.AspectName = "ICMSSTRedBase";
		this.olvICMSSTRedBase.Text = "ICMS ST Red. Base";
		this.olvICMSSTRedBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTRedBase.ToolTipText = "Percentual da Redução de BC";
		this.olvICMSSTRedBase.Width = 100;
		this.olvICMSSTBase.AspectName = "ICMSSTBase";
		this.olvICMSSTBase.Text = "ICMS ST Base";
		this.olvICMSSTBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTBase.ToolTipText = "Base substituição tributária";
		this.olvICMSSTBase.Width = 100;
		this.olvICMSSTTaxa.AspectName = "ICMSSTTaxa";
		this.olvICMSSTTaxa.Text = "ICMS ST Taxa";
		this.olvICMSSTTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTTaxa.ToolTipText = "Taxa substituição tributária";
		this.olvICMSSTTaxa.Width = 100;
		this.olvICMSSTValor.AspectName = "ICMSSTValor";
		this.olvICMSSTValor.Text = "ICMS ST Valor";
		this.olvICMSSTValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTValor.ToolTipText = "Valor da substituição tributária";
		this.olvICMSSTValor.Width = 100;
		this.olvICMSSTFCPBase.AspectName = "ICMSSTFCPBase";
		this.olvICMSSTFCPBase.Text = "ICMS ST FCP Base";
		this.olvICMSSTFCPBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTFCPBase.ToolTipText = "Base percentual do fundo de combate à pobreza (FCP)";
		this.olvICMSSTFCPBase.Width = 100;
		this.olvICMSSTFCPTaxa.AspectName = "ICMSSTFCPTaxa";
		this.olvICMSSTFCPTaxa.Text = "ICMS ST FCP Taxa";
		this.olvICMSSTFCPTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTFCPTaxa.ToolTipText = "Taxa percentual do fundo de combate à pobreza (FCP)";
		this.olvICMSSTFCPTaxa.Width = 100;
		this.olvICMSSTFCPValor.AspectName = "ICMSSTFCPValor";
		this.olvICMSSTFCPValor.Text = "ICMS ST FCP Valor";
		this.olvICMSSTFCPValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTFCPValor.ToolTipText = "Valor percentual do fundo de combate à pobreza (FCP)";
		this.olvICMSSTFCPValor.Width = 100;
		this.olvICMSSTRetBase.AspectName = "ICMSSTRetBase";
		this.olvICMSSTRetBase.Text = "ICMS Valor BC ST Ret";
		this.olvICMSSTRetBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTRetBase.ToolTipText = "Valor da BC do ICMS ST retido";
		this.olvICMSSTRetBase.Width = 100;
		this.olvICMSSTRetValor.AspectName = "ICMSSTRetValor";
		this.olvICMSSTRetValor.Text = "ICMS Valor ST Ret";
		this.olvICMSSTRetValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSSTRetValor.ToolTipText = "Valor do ICMS ST retido";
		this.olvICMSSTRetValor.Width = 100;
		this.olvICMSSTRetTaxa.AspectName = "ICMSSTRetTaxa";
		this.olvICMSSTRetTaxa.Text = "ICMS Taxa Consumidor Final";
		this.olvICMSSTRetTaxa.ToolTipText = "Alíquota suportada pelo Consumidor Final";
		this.olvICMSSTRetTaxa.Width = 100;
		this.olvICMSSubst.AspectName = "ICMSSTSubValor";
		this.olvICMSSubst.Text = "ICMS Valor Substituto";
		this.olvICMSSubst.ToolTipText = "Valor do ICMS próprio do Substituto";
		this.olvICMSSubst.Width = 100;
		this.olvICMSValorDif.AspectName = "ICMSValorDif";
		this.olvICMSValorDif.Text = "ICMS Valor Diferido";
		this.olvICMSValorDif.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSValorDif.ToolTipText = "Valor do ICMS diferido";
		this.olvICMSValorDif.Width = 100;
		this.olvICMSValorOper.AspectName = "ICMSValorOper";
		this.olvICMSValorOper.Text = "ICMS Valor da Operação";
		this.olvICMSValorOper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSValorOper.ToolTipText = "Valor do ICMS da Operação";
		this.olvICMSValorOper.Width = 100;
		this.olvICMSTaxaDif.AspectName = "ICMSTaxaDif";
		this.olvICMSTaxaDif.Text = "ICMS Percentual do Diferimento";
		this.olvICMSTaxaDif.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSTaxaDif.ToolTipText = "Percentual do Diferimento";
		this.olvICMSTaxaDif.Width = 100;
		this.olvICMSMonoRetBaseQtd.AspectName = "ICMSMonoRetBaseQtd";
		this.olvICMSMonoRetBaseQtd.Text = "ICMS Mono Ret Base";
		this.olvICMSMonoRetBaseQtd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSMonoRetBaseQtd.ToolTipText = "Quantidade tributada retida anteriormente";
		this.olvICMSMonoRetBaseQtd.Width = 100;
		this.olvICMSMonoRetValor.AspectName = "ICMSMonoRetValor";
		this.olvICMSMonoRetValor.Text = "ICMS Monofásico valor retido anteriormente";
		this.olvICMSMonoRetValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSMonoRetValor.ToolTipText = "Valor do ICMS retido anteriormente";
		this.olvICMSMonoRetValor.Width = 100;
		this.olvICMSAdRemRetTaxa.AspectName = "ICMSAdRemRetTaxa";
		this.olvICMSAdRemRetTaxa.Text = "ICMS Aliquota Ad Rem";
		this.olvICMSAdRemRetTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSAdRemRetTaxa.ToolTipText = "Alíquota ad rem do imposto retido anteriormente";
		this.olvICMSAdRemRetTaxa.Width = 100;
		this.olvICMSAliqAdRem.AspectName = "ICMSAliqAdRem";
		this.olvICMSAliqAdRem.Text = "ICMS Aliq. Ad Rem estab. na leg. para o prod.";
		this.olvICMSAliqAdRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSAliqAdRem.ToolTipText = "Alíquota ad rem do ICMS estabelecida na legislação para o produto.";
		this.olvICMSAliqAdRem.Width = 100;
		this.olvICMSCredSnTaxa.AspectName = "ICMSCredSnTaxa";
		this.olvICMSCredSnTaxa.Text = "ICMS Simples Nac. Taxa Crédito";
		this.olvICMSCredSnTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSCredSnTaxa.ToolTipText = "Taxa alíquota aplicável de cálculo do crédito (Simples Nacional)";
		this.olvICMSCredSnTaxa.Width = 100;
		this.olvICMSCredSnValor.AspectName = "ICMSCredSnValor";
		this.olvICMSCredSnValor.Text = "ICMS Simples Nac. Valor Crédito";
		this.olvICMSCredSnValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSCredSnValor.ToolTipText = "Valor alíquota aplicável de cálculo do crédito (Simples Nacional)";
		this.olvICMSCredSnValor.Width = 100;
		this.olvICMSUfDestBase.AspectName = "ICMSUfDestBase";
		this.olvICMSUfDestBase.Text = "ICMS UF Destino Base";
		this.olvICMSUfDestBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfDestBase.ToolTipText = "Base alíquota interna da UF de destino ";
		this.olvICMSUfDestBase.Width = 100;
		this.olvICMSUfDestTaxa.AspectName = "ICMSUfDestTaxa";
		this.olvICMSUfDestTaxa.Text = "ICMS UF Destino Taxa";
		this.olvICMSUfDestTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfDestTaxa.ToolTipText = "Taxa alíquota interna da UF de destino ";
		this.olvICMSUfDestTaxa.Width = 100;
		this.olvICMSUfIntrTaxa.AspectName = "ICMSUfIntrTaxa";
		this.olvICMSUfIntrTaxa.Text = "ICMS UF Taxa Interestadual";
		this.olvICMSUfIntrTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfIntrTaxa.ToolTipText = "Taxa ICMS interestadual";
		this.olvICMSUfIntrTaxa.Width = 100;
		this.olvICMSUfDestValor.AspectName = "ICMSUfDestValor";
		this.olvICMSUfDestValor.Text = "ICMS UF Destino Valor";
		this.olvICMSUfDestValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfDestValor.ToolTipText = "Valor UF de destino";
		this.olvICMSUfDestValor.Width = 100;
		this.olvICMSUfRemeValor.AspectName = "ICMSUfRemeValor";
		this.olvICMSUfRemeValor.Text = "ICMS UF Remetente Valor";
		this.olvICMSUfRemeValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfRemeValor.ToolTipText = "Valor total do ICMS interestadual para a UF do remetente";
		this.olvICMSUfRemeValor.Width = 100;
		this.olvICMSUfDestFCPBase.AspectName = "ICMSUfDestFCPBase";
		this.olvICMSUfDestFCPBase.Text = "ICMS UF Destino FCP Base";
		this.olvICMSUfDestFCPBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfDestFCPBase.ToolTipText = "Base de fundo de combate à pobreza (FCP) na UF de destino";
		this.olvICMSUfDestFCPBase.Width = 100;
		this.olvICMSUfDestFCPTaxa.AspectName = "ICMSUfDestFCPTaxa";
		this.olvICMSUfDestFCPTaxa.Text = "ICMS UF Destino FCP Taxa";
		this.olvICMSUfDestFCPTaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfDestFCPTaxa.ToolTipText = "Taxa fundo de combate à pobreza (FCP) na UF de destino";
		this.olvICMSUfDestFCPTaxa.Width = 100;
		this.olvICMSUfDestFCPValor.AspectName = "ICMSUfDestFCPValor";
		this.olvICMSUfDestFCPValor.Text = "ICMS UF Destino FCP Valor";
		this.olvICMSUfDestFCPValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvICMSUfDestFCPValor.ToolTipText = "Valor fundo de combate à pobreza (FCP) na UF de destino";
		this.olvICMSUfDestFCPValor.Width = 100;
		this.olvIPIcEnq.AspectName = "IPIcEnq";
		this.olvIPIcEnq.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPIcEnq.Text = "IPI Enquadramento";
		this.olvIPIcEnq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPIcEnq.ToolTipText = "Código de enquadramento legal do IPI ";
		this.olvIPIcEnq.Width = 100;
		this.olvIPICdCst.AspectName = "IPICdCst";
		this.olvIPICdCst.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPICdCst.Text = "IPI CST";
		this.olvIPICdCst.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPICdCst.ToolTipText = " Código de situação tributária";
		this.olvIPICdCst.Width = 100;
		this.olvIPIBase.AspectName = "IPIBase";
		this.olvIPIBase.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPIBase.Text = "IPI Base";
		this.olvIPIBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIPIBase.ToolTipText = "Base de impostos sobre produtos industrializados";
		this.olvIPIBase.Width = 100;
		this.olvIPITaxa.AspectName = "IPITaxa";
		this.olvIPITaxa.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPITaxa.Text = "IPI Taxa";
		this.olvIPITaxa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIPITaxa.ToolTipText = "Taxa de imposto sobre produtos industrializados";
		this.olvIPITaxa.Width = 100;
		this.olvIPIValor.AspectName = "IPIValor";
		this.olvIPIValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPIValor.Text = "IPI Valor";
		this.olvIPIValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIPIValor.ToolTipText = "Valor de imposto sobre produtos industrializados";
		this.olvIPIValor.Width = 100;
		this.olvIPIDevol.AspectName = "IPIDevol";
		this.olvIPIDevol.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIPIDevol.Text = "IPI Devolvido";
		this.olvIPIDevol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIPIDevol.ToolTipText = "Valor do IPI devolvido ";
		this.olvIPIDevol.Width = 100;
		this.olvIIBase.AspectName = "IIBase";
		this.olvIIBase.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIIBase.Text = "II Base";
		this.olvIIBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIIBase.ToolTipText = "Base II";
		this.olvIIBase.Width = 100;
		this.olvIIValor.AspectName = "IIValor";
		this.olvIIValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIIValor.Text = "II Valor";
		this.olvIIValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIIValor.ToolTipText = "Valor Total do II ";
		this.olvIIValor.Width = 100;
		this.olvIIDespAdu.AspectName = "IIDespAdu";
		this.olvIIDespAdu.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIIDespAdu.Text = "II Desp.Adu";
		this.olvIIDespAdu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIIDespAdu.ToolTipText = "Valor despesas aduaneiras ";
		this.olvIIDespAdu.Width = 100;
		this.olvIIIOF.AspectName = "IIIOF";
		this.olvIIIOF.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvIIIOF.Text = "II IOF";
		this.olvIIIOF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvIIIOF.ToolTipText = "Valor imposto sobre operações financeiras";
		this.olvIIIOF.Width = 100;
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
		this.olvPISCdCst.Text = "PIS CST";
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
		this.olvISSQNBase.AspectName = "ISSQNBase";
		this.olvISSQNBase.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvISSQNBase.Text = "ISSQN Base";
		this.olvISSQNBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvISSQNBase.ToolTipText = "Valor da base de cálculo do ISSQN";
		this.olvISSQNBase.Width = 100;
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
		this.olvISSQNCdSrv.AspectName = "ISSQNCdSrv";
		this.olvISSQNCdSrv.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvISSQNCdSrv.Text = "ISSQN Srv";
		this.olvISSQNCdSrv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvISSQNCdSrv.ToolTipText = "Item da lista de serviços";
		this.olvISSQNCdSrv.Width = 100;
		this.olvEmitIE.AspectName = "EmitIE";
		this.olvEmitIE.Text = "Emissor IE";
		this.olvEmitIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitIE.ToolTipText = "IE do emitente ";
		this.olvEmitcMun.AspectName = "EmitcMun";
		this.olvEmitcMun.Text = "Emissor Mun Cod";
		this.olvEmitcMun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvEmitcMun.ToolTipText = "Código do municipío do emitente";
		this.olvEmitxMun.AspectName = "EmitxMun";
		this.olvEmitxMun.Text = "Emissor Mun Desc";
		this.olvEmitxMun.ToolTipText = "Nome do municipío do emitente";
		this.olvEmitUf.AspectName = "EmitUf";
		this.olvEmitUf.Text = "Emissor Uf";
		this.olvEmitUf.ToolTipText = "Sigla da UF do emitente";
		this.olvDestinId.AspectName = "DestinID";
		this.olvDestinId.Text = "Destinatário CNPJ/CPF";
		this.olvDestinId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinId.ToolTipText = "CNPJ/CPF do destinatário";
		this.olvDestinId.Width = 130;
		this.olvDestinIE.AspectName = "DestinIE";
		this.olvDestinIE.Text = "Destinatário IE";
		this.olvDestinIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDestinIE.ToolTipText = "IE do destinatário";
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
		this.olvDestinxMun.ToolTipText = "Nome do municipío do destinatário";
		this.olvDestinUf.AspectName = "DestinUf";
		this.olvDestinUf.Text = "Destinatário Uf";
		this.olvDestinUf.ToolTipText = "Sigla da UF do destinatário";
		this.olvManifest.Text = "Manifestação";
		this.olvManifest.ToolTipText = "Código da manifestação";
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
		this.olvHasEvent.ToolTipText = "Tem evento";
		this.olvHasEvent.Width = 35;
		this.olvHasCTeEvent.AspectName = "HasCTeEvent";
		this.olvHasCTeEvent.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasCTeEvent.Text = "CTe";
		this.olvHasCTeEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasCTeEvent.ToolTipText = "Tem evento no CTe";
		this.olvHasCTeEvent.Width = 35;
		this.olvMDFeEvtData.AspectName = "MDFeEvtData";
		this.olvMDFeEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.Text = "MDFe";
		this.olvMDFeEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.ToolTipText = "Data manifesto eletrônico de documentos fiscais";
		this.olvMDFeEvtData.Width = 76;
		this.olvSufVistEvtData.AspectName = "SufVistEvtData";
		this.olvSufVistEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistEvtData.Text = "Suf Vist";
		this.olvSufVistEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistEvtData.ToolTipText = "Data da vistoria na suframa";
		this.olvSufVistEvtData.Width = 76;
		this.olvSufVistInteData.AspectName = "SufInteEvtData";
		this.olvSufVistInteData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistInteData.Text = "Suf Inter";
		this.olvSufVistInteData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistInteData.ToolTipText = "Data da internalização na suframa";
		this.olvSufVistInteData.Width = 76;
		this.olvMotivo.AspectName = "xMotivo";
		this.olvMotivo.Text = "Status";
		this.olvMotivo.ToolTipText = "Motivo";
		this.olvMotivo.Width = 200;
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do documento fiscal ";
		this.olvEstado.Width = 30;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "Chave";
		this.olvChave.ToolTipText = "Chave de acesso";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 130;
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "Natureza";
		this.olvNatOper.ToolTipText = "Descrição da natureza da operação";
		this.olvnFCI.AspectName = "nFCI";
		this.olvnFCI.Text = "FCI";
		this.olvnFCI.ToolTipText = "Número de controle da FCI - Ficha de Conteúdo de Importação";
		this.olvxPed.AspectName = "xPed";
		this.olvxPed.Text = "Pedido";
		this.olvxPed.ToolTipText = "Número do Pedido de Compra";
		this.olvnItemPed.AspectName = "nItemPed";
		this.olvnItemPed.Text = "PedItem";
		this.olvnItemPed.ToolTipText = "Item do Pedido de Compra";
		this.olvcProdANP.AspectName = "cProdANP";
		this.olvcProdANP.Text = "Código de produto da ANP";
		this.olvcProdANP.ToolTipText = "Codificação de produtos do sistema de informações de movimentação de produtos - SIMP";
		this.olvinfAdProd.AspectName = "infAdProd";
		this.olvinfAdProd.Text = "InfAdProd";
		this.olvinfAdProd.ToolTipText = "Informações Adicionais do Produto";
		this.olvBatchNum.AspectName = "BatchNum";
		this.olvBatchNum.Text = "Lote Proc.";
		this.olvBatchNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBatchNum.ToolTipText = "Número de lote de processamento";
		this.olvTpOp.AspectName = "VeicProdTpOp";
		this.olvTpOp.Text = "VeicProd:TpOper";
		this.olvTpOp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTpOp.ToolTipText = "Veículos Novos : Tipo de Operação";
		this.olvTpOp.Width = 100;
		this.olvChassi.AspectName = "VeicProdChassi";
		this.olvChassi.Text = "VeicProd:Chassi";
		this.olvChassi.ToolTipText = "Veículos Novos : Chassi do veículo - VIN (código-identificação-veículo)";
		this.olvChassi.Width = 100;
		this.olvCCor.AspectName = "VeicProdCCor";
		this.olvCCor.Text = "VeicProd:CodCor";
		this.olvCCor.ToolTipText = "Veículos Novos : Cor - Código de cada montadora";
		this.olvCCor.Width = 100;
		this.olvXCor.AspectName = "VeicProdXCor";
		this.olvXCor.Text = "VeicProd:DescCor";
		this.olvXCor.ToolTipText = "Descrição da Cor";
		this.olvXCor.Width = 100;
		this.olvPot.AspectName = "VeicProdPot";
		this.olvPot.Text = "VeicProd:Potência";
		this.olvPot.ToolTipText = "Potência Motor (CV) - Potência máxima do motor do veículo em cavalo vapor (CV). (potência-veículo)";
		this.olvPot.Width = 100;
		this.olvCilin.AspectName = "VeicProdCilin";
		this.olvCilin.Text = "VeicProd:Cilin";
		this.olvCilin.ToolTipText = "Cilindradas -Capacidade voluntária do motor expressa em centímetros cúbicos (CC). (cilindradas)";
		this.olvCilin.Width = 100;
		this.olvPesoL.AspectName = "VeicProdPesoL";
		this.olvPesoL.Text = "VeicProd:PesoL";
		this.olvPesoL.ToolTipText = "Peso Líquido - em toneladas - 4 casas decimais";
		this.olvPesoL.Width = 100;
		this.olvPesoB.AspectName = "VeicProdPesoB";
		this.olvPesoB.Text = "VeicProd:PesoB";
		this.olvPesoB.ToolTipText = "Peso Bruto - Peso Bruto Total - em tonelada - 4 casas decimais";
		this.olvPesoB.Width = 100;
		this.olvNSerie.AspectName = "VeicProdNSerie";
		this.olvNSerie.Text = "VeicProd:NSerie";
		this.olvNSerie.ToolTipText = "Serial (série)";
		this.olvNSerie.Width = 100;
		this.olvTpComb.AspectName = "VeicProdTpComb";
		this.olvTpComb.Text = "VeicProd:TpCom";
		this.olvTpComb.ToolTipText = "Tipo de combustível - Tabela RENAVAM";
		this.olvTpComb.Width = 100;
		this.olvNMotor.AspectName = "VeicProdNMotor";
		this.olvNMotor.Text = "VeicProd:NMotor";
		this.olvNMotor.ToolTipText = "Número de Motor";
		this.olvNMotor.Width = 100;
		this.olvCmt.AspectName = "VeicProdCmt";
		this.olvCmt.Text = "VeicProd:CMT";
		this.olvCmt.ToolTipText = "Capacidade Máxima de Tração - CMT-Capacidade Máxima de Tração - em Toneladas 4 casas decimais";
		this.olvCmt.Width = 100;
		this.olvDist.AspectName = "VeicProdDist";
		this.olvDist.Text = "VeicProd:Dist";
		this.olvDist.ToolTipText = "Distância entre eixos";
		this.olvDist.Width = 100;
		this.olvAnoMod.AspectName = "VeicProdAnoMod";
		this.olvAnoMod.Text = "VeicProd:AnoMod";
		this.olvAnoMod.ToolTipText = "Ano Modelo de Fabricação";
		this.olvAnoMod.Width = 100;
		this.olvAnoFab.AspectName = "VeicProdAnoFab";
		this.olvAnoFab.Text = "VeicProd:AnoFab";
		this.olvAnoFab.ToolTipText = "Ano de Fabricação";
		this.olvAnoFab.Width = 100;
		this.olvTpPint.AspectName = "VeicProdTpPint";
		this.olvTpPint.Text = "VeicProd:TpPint";
		this.olvTpPint.ToolTipText = "Tipo de Pintura";
		this.olvTpPint.Width = 100;
		this.olvTpVeic.AspectName = "VeicProdTpVeic";
		this.olvTpVeic.Text = "VeicProd:TpVeic";
		this.olvTpVeic.ToolTipText = "Tipo de Veículo : Tabela RENAVAM";
		this.olvTpVeic.Width = 100;
		this.olvEspVeic.AspectName = "VeicProdEspVeic";
		this.olvEspVeic.Text = "VeicProd:EspVeic";
		this.olvEspVeic.ToolTipText = "Espécie de Veículo : Tabela RENAVAM";
		this.olvEspVeic.Width = 100;
		this.olvVin.AspectName = "VeicProdVin";
		this.olvVin.Text = "VeicProd:VIN";
		this.olvVin.ToolTipText = "Condição do VIN - Informa-se o veículo tem VIN (chassi) remarcado";
		this.olvVin.Width = 100;
		this.olvCondVeic.AspectName = "VeicProdCondVeic";
		this.olvCondVeic.Text = "VeicProd:CondVeic";
		this.olvCondVeic.ToolTipText = "Condição do Veículo";
		this.olvCondVeic.Width = 100;
		this.olvcMod.AspectName = "VeicProdcMod";
		this.olvcMod.Text = "VeicProd:Modelo";
		this.olvcMod.ToolTipText = "Código Marca Modelo : Tabela RENAVAM";
		this.olvcMod.Width = 100;
		this.olvCCorDENATRAN.AspectName = "VeicProdCCorDENATRAN";
		this.olvCCorDENATRAN.Text = "VeicProd:CCorDENATRAN";
		this.olvCCorDENATRAN.ToolTipText = "CCódigo da Cor - Segundo as regras de pré-cadastro do DENATRAN";
		this.olvCCorDENATRAN.Width = 100;
		this.olvLota.AspectName = "VeicProdLota";
		this.olvLota.Text = "VeicProd:Lotação";
		this.olvLota.ToolTipText = "Capacidade máxima de lotação - Quantidade máxima permitida de passageiros sentados, inclusive motorista.";
		this.olvLota.Width = 100;
		this.olvTpRest.AspectName = "VeicProdTpRest";
		this.olvTpRest.Text = "VeicProd:TpRest";
		this.olvTpRest.ToolTipText = "Restrição";
		this.olvTpRest.Width = 100;
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvDocNote.ToolTipText = "Comentários";
		this.olvInfAdFisco.Text = "Informações adicionais fisco";
		this.olvInfAdFisco.ToolTipText = "Informações adicionais fisco";
		this.olvInfCpl.Text = "Informações complementares";
		this.olvInfCpl.ToolTipText = "Complemento";
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
		this.olvCNPJCPFTransp.AspectName = "TranspID";
		this.olvCNPJCPFTransp.DisplayIndex = 164;
		this.olvCNPJCPFTransp.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCNPJCPFTransp.IsVisible = false;
		this.olvCNPJCPFTransp.Text = "Transportadora\n: CNPJ/CPF";
		this.olvCNPJCPFTransp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvCNPJCPFTransp.Width = 70;
		this.olvNomeTransp.AspectName = "TranspNome";
		this.olvNomeTransp.DisplayIndex = 165;
		this.olvNomeTransp.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNomeTransp.IsVisible = false;
		this.olvNomeTransp.Text = "Transportadora: Nome";
		this.olvNomeTransp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvNomeTransp.Width = 70;
		this.olvValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValor.Text = "Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.ToolTipText = resources.GetString("olvValor.ToolTipText");
		this.olvValor.Width = 100;
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
		this.lknAction.Text = "NFe Dados Analíticos";
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
		this.lbText01.Text = "Apresenta a visualização de todas as informações de NFe, NFCe e CFeSAT em nível de item.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "NFe: Dados Analíticos é ativado nos \r\nplanos Avançado e Enterprise.";
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
		base.Name = "frmTabDocNFeDet";
		this.Text = "NFe: Dados Analíticos";
		base.Load += new System.EventHandler(frmTabDocNFeDet_Load);
		this.pnContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
