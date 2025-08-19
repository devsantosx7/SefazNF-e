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

public class frmTabDocOut : Form
{
	private delegate void funcShowSkipNumDelegate(List<DocJumpOut> pDocList);

	private class DocComparer : IEqualityComparer<Document>
	{
		public bool Equals(Document x, Document y)
		{
			string text = x.Filial + x.Model + x.Serie;
			string varKeyY = y.Filial + y.Model + y.Serie;
			return text.Equals(varKeyY);
		}

		public int GetHashCode(Document obj)
		{
			return (obj.Filial + obj.Model + obj.Serie).GetHashCode();
		}
	}

	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocOut _SqlTabData = new clsSrvTabDocOut();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private string _InfoAddXml = string.Empty;

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private bool _HasFiscalioConnect;

	private string _SqlFields = " * ";

	private bool _IsFormLoaded;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private funcShowSkipNumDelegate funcShowSkipNumAction;

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

	private OLVColumn olvTomaIE;

	private OLVColumn olvTpDoc;

	private OLVColumn olvNatOper;

	private OLVColumn olvBaseIcms;

	private OLVColumn olvValorIcms;

	private OLVColumn olvCFOP;

	private OLVColumn olvNCM;

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

	private ImageList ImageListDocs;

	private OLVColumn olvDtEmi;

	private OLVColumn olvBatchNum;

	private OLVColumn olvDtEntFisc;

	private OLVColumn olvDtRegFisc;

	private OLVColumn olvUsRegFisc;

	private OLVColumn olvDcNumFisc;

	private OLVColumn olvDocNote;

	private SplitContainer splitData;

	private OLVColumn olvSkipFilial;

	private OLVColumn olvSkipTipo;

	private OLVColumn olvSkipSerie;

	private OLVColumn olvFirstNum;

	private SplitContainer splitContainer1;

	private Panel panel1;

	private Label label2;

	private Label lbSeeMore;

	private LinkLabel lkbHelpScanOut;

	private Label label1;

	private OLVColumn olvDestNome;

	private OLVColumn olvDestID;

	private OLVColumn olvLastNum;

	private Button btClose;

	private Label lbTitle01;

	private Label lbTitle02;

	private Panel pnMarketContent;

	private Label lbText01;

	private Label lbText02;

	private LinkLabel lknAction;

	private LinkLabel lknClose;

	private Panel pnMarket;

	private PictureBox picWarning;

	private Label label3;

	private Label lbTitle03;

	private OLVColumn olvTagDtHr;

	private OLVColumn olvTagUser;

	private OLVColumn olvDocNoteDtHr;

	private OLVColumn olvDocNoteUser;

	private LinkLabel lnkJumpOut;

	private Label label5;

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

	public frmTabDocOut(string pTitle, Color pColumnColor, string pFeatExtId)
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

	private async void frmTabDocOut_Load(object sender, EventArgs e)
	{
		_IsFormLoaded = false;
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
		_SqlFields = "document.ManifCode, document.TomaIE, document.HasXml, document.XmlError, ";
		_SqlFields += "document.FisIoApi, document.DocNote, document.cUF, document.DtAut, ";
		_SqlFields += "document.Tag, document.HasCancelEvent, document.Canceled, document.Model, ";
		_SqlFields += "document.HasCTeEvent, document.HasMDFeEvent, document.HasSufVistEvent, ";
		_SqlFields += "document.HasSufInteEvent, document.TomaID, document.TomaNome, ";
		_SqlFields += "document.EmitID, document.EmitNome, document.DestinID, document.DestinNome, ";
		_SqlFields += "document.BaseICMSOutraUF, document.TaxaICMSOutraUF, document.ValorICMSOutraUF, ";
		_SqlFields += "document.ComexAverbStat, document.DFeSource, document.XmlSource, ";
		foreach (OLVColumn varColumn in lsvData.AllColumns)
		{
			if (!clsFunction.IsEmpty(varColumn.AspectName) && !_SqlFields.Contains(varColumn.AspectName + ","))
			{
				_SqlFields = _SqlFields + "document." + varColumn.AspectName + ",";
			}
		}
		_SqlFields = clsFunction.funcClearEnd(_SqlFields, ",");
		_SqlTabData = new clsSrvTabDocOut(_SqlFields);
		funcDefineListViewFeatures();
		lbTitle01.Text = "Relatório " + Text;
		if (!clsFunction.IsEmpty(await _clsDataParam.funcGetAsync(_consMarketScreenUser)))
		{
			lsvData.Dock = DockStyle.Fill;
			pnMarket.Visible = false;
			base.Controls.Remove(pnMarket);
		}
		else
		{
			pnMarket.Visible = true;
			lsvData.Dock = DockStyle.None;
		}
		_IsFormLoaded = true;
	}

	private void funcDefineListViewFeatures()
	{
		olvHasXml.ImageGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(document, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
		};
		olvValor.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return clsFunction.IsEmpty(document?.Valor) ? null : ((object)clsFunction.funcConvStrToDec(document.Valor));
		};
		olvValorIcms.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			decimal num = clsFunction.funcConvStrToDec(document.ValorICMS);
			if (num <= 0m)
			{
				num = clsFunction.funcConvStrToDec(document.ValorICMSOutraUF);
			}
			return num;
		};
		olvBaseIcms.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			if (document == null)
			{
				return (object)null;
			}
			decimal num = clsFunction.funcConvStrToDec(document.BaseICMS);
			if (num <= 0m)
			{
				num = clsFunction.funcConvStrToDec(document.BaseICMSOutraUF);
			}
			return num;
		};
		olvHasXml.AspectGetter = (object x) => string.Empty;
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
		olvManifest.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.GetEventDesc(document.Model, document.ManifCode);
		};
		olvEmitCNPJ.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsFunction.funcFormatDoc(document.EmitID);
		};
		olvEmitNome.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsFunction.funcFormatDoc(document.EmitNome);
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
		olvManifest.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.GetEventDesc(document.Model, document.ManifCode);
		};
		olvTpDoc.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : _clsDFeCodes.funcGetTipoDoc(document.Model, document.TpDoc);
		};
		olvTomaIE.AspectGetter = delegate(object x)
		{
			Document document = (Document)x;
			return (document == null) ? null : clsSrvGeral.funcGetIEFormat(document.TomaIE);
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
		List<Document> varclsDocList = await _clsDataDoc.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		olvSelect.HeaderCheckState = CheckState.Unchecked;
		lsvData.SetObjects(varclsDocList);
		lsvData.ResumeLayout();
		lsvData.EndUpdate();
		lsvData = funcListGroupSort(lsvData, pclsDataFilter);
		_GroupColapsed = false;
		return varclsReturn;
	}

	private List<DocJumpOut> funcLoadSkipNum(List<Document> pDocList)
	{
		IEnumerable<Document> enumerable = pDocList.Distinct(new DocComparer());
		List<DocJumpOut> varFinalList = new List<DocJumpOut>();
		foreach (Document varclsItem in enumerable)
		{
			List<Document> varDocList = (from r in pDocList
				where clsFunction.IsEqual(r.Filial, varclsItem.Filial) && clsFunction.IsEqual(r.Model, varclsItem.Model) && clsFunction.IsEqual(r.Serie, varclsItem.Serie)
				orderby clsFunction.funcConvStrToLong(r.Num)
				select r).ToList();
			long varDocInit = clsFunction.funcConvStrToLong(varDocList.FirstOrDefault().Num);
			long varDocLast = clsFunction.funcConvStrToLong(varDocList.LastOrDefault().Num);
			long? first = null;
			long last = 0L;
			for (long varCounter = varDocInit; varCounter <= varDocLast; varCounter++)
			{
				Document found = varDocList.FirstOrDefault();
				if (found != null && clsFunction.funcConvStrToLong(found.Num).Equals(varCounter))
				{
					varDocList.RemoveAt(0);
					if (first.HasValue)
					{
						DocJumpOut varclsDoc = varclsItem.GetDocOutJump($"{first}", $"{last}");
						varFinalList.Add(varclsDoc);
						first = null;
					}
				}
				else
				{
					if (!first.HasValue)
					{
						first = varCounter;
					}
					last = varCounter;
				}
			}
		}
		return varFinalList;
	}

	private FastObjectListView funcListGroupSort(FastObjectListView plsvData, clsDataFilter pclsDataFilter)
	{
		OLVColumn varGroupColum = null;
		SortOrder varSortOrder = SortOrder.None;
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
			varGroupColum = olvTomaIE;
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
		clsDataDoc varDataHandler = new clsDataDoc();
		List<Document> varDocList = funcGetObjList(pFocused, pChecked);
		if (pSyncFromDbaFirst)
		{
			varDocList = await varDataHandler.funcGetSyncListByListAsync(varDocList);
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
		varArguments.TaskProgress = "Transferindo documento(s) para o destino ....";
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

	private void lkbHelpScanOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallScanDFeKeyOutAsync();
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

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipsHowToScanDFeOutAsync();
	}

	private async void lknClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		lsvData.Dock = DockStyle.Fill;
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
		await _clsDataParam.funcSetAsync(_consMarketScreenUser, "X");
	}

	private void lbTitle03_Click(object sender, EventArgs e)
	{
	}

	private void lsvData_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void lnkJumpOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.UserAction = "OPEN_TABDOCJUMP";
		OnEventTabManager(varArguments);
		lnkJumpOut.LinkVisited = true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocOut));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.pnContent = new System.Windows.Forms.Panel();
		this.splitData = new System.Windows.Forms.SplitContainer();
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAut = new BrightIdeasSoftware.OLVColumn();
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
		this.olvSufVistEvtData = new BrightIdeasSoftware.OLVColumn();
		this.olvSufVistInteData = new BrightIdeasSoftware.OLVColumn();
		this.olvMotivo = new BrightIdeasSoftware.OLVColumn();
		this.olvEstado = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmi = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvTomaIE = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvBaseIcms = new BrightIdeasSoftware.OLVColumn();
		this.olvValorIcms = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvInfCpl = new BrightIdeasSoftware.OLVColumn();
		this.olvInfAdFisco = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvError = new BrightIdeasSoftware.OLVColumn();
		this.olvBatchNum = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvDestNome = new BrightIdeasSoftware.OLVColumn();
		this.olvDestID = new BrightIdeasSoftware.OLVColumn();
		this.olvTagDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvTagUser = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteDtHr = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNoteUser = new BrightIdeasSoftware.OLVColumn();
		this.olvObsCont = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.lnkJumpOut = new System.Windows.Forms.LinkLabel();
		this.label1 = new System.Windows.Forms.Label();
		this.lkbHelpScanOut = new System.Windows.Forms.LinkLabel();
		this.label2 = new System.Windows.Forms.Label();
		this.lbSeeMore = new System.Windows.Forms.Label();
		this.pnMarket = new System.Windows.Forms.Panel();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.pnMarketContent = new System.Windows.Forms.Panel();
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.picWarning = new System.Windows.Forms.PictureBox();
		this.lknAction = new System.Windows.Forms.LinkLabel();
		this.lbText02 = new System.Windows.Forms.Label();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.pnContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitData).BeginInit();
		this.splitData.Panel1.SuspendLayout();
		this.splitData.Panel2.SuspendLayout();
		this.splitData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.pnContent.Controls.Add(this.splitData);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(741, 466);
		this.pnContent.TabIndex = 10;
		this.splitData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitData.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitData.IsSplitterFixed = true;
		this.splitData.Location = new System.Drawing.Point(0, 0);
		this.splitData.Name = "splitData";
		this.splitData.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitData.Panel1.Controls.Add(this.lsvData);
		this.splitData.Panel1MinSize = 324;
		this.splitData.Panel2.Controls.Add(this.splitContainer1);
		this.splitData.Panel2MinSize = 90;
		this.splitData.Size = new System.Drawing.Size(741, 466);
		this.splitData.SplitterDistance = 372;
		this.splitData.TabIndex = 1;
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvHasXml);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvDtAut);
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
		this.lsvData.AllColumns.Add(this.olvSufVistEvtData);
		this.lsvData.AllColumns.Add(this.olvSufVistInteData);
		this.lsvData.AllColumns.Add(this.olvMotivo);
		this.lsvData.AllColumns.Add(this.olvEstado);
		this.lsvData.AllColumns.Add(this.olvDtEmi);
		this.lsvData.AllColumns.Add(this.olvChave);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvTomaIE);
		this.lsvData.AllColumns.Add(this.olvTpDoc);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvBaseIcms);
		this.lsvData.AllColumns.Add(this.olvValorIcms);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvNCM);
		this.lsvData.AllColumns.Add(this.olvInfCpl);
		this.lsvData.AllColumns.Add(this.olvInfAdFisco);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvError);
		this.lsvData.AllColumns.Add(this.olvBatchNum);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvDestNome);
		this.lsvData.AllColumns.Add(this.olvDestID);
		this.lsvData.AllColumns.Add(this.olvTagDtHr);
		this.lsvData.AllColumns.Add(this.olvTagUser);
		this.lsvData.AllColumns.Add(this.olvDocNoteDtHr);
		this.lsvData.AllColumns.Add(this.olvDocNoteUser);
		this.lsvData.AllColumns.Add(this.olvObsCont);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[46]
		{
			this.olvSelect, this.olvHasXml, this.olvNum, this.olvSerie, this.olvTipo, this.olvTag, this.olvDtAut, this.olvValor, this.olvVenc, this.olvDtEntFisc,
			this.olvDtRegFisc, this.olvUsRegFisc, this.olvDcNumFisc, this.olvStatus, this.olvEmitCNPJ, this.olvEmitNome, this.olvManifest, this.olvCancel, this.olvHasEvent, this.olvHasCTeEvent,
			this.olvMDFeEvtData, this.olvSufVistEvtData, this.olvSufVistInteData, this.olvMotivo, this.olvEstado, this.olvDtEmi, this.olvChave, this.olvFilial, this.olvTomaIE, this.olvTpDoc,
			this.olvNatOper, this.olvBaseIcms, this.olvValorIcms, this.olvCFOP, this.olvNCM, this.olvInfCpl, this.olvInfAdFisco, this.olvAnoMes, this.olvError, this.olvBatchNum,
			this.olvDocNote, this.olvTagDtHr, this.olvTagUser, this.olvDocNoteDtHr, this.olvDocNoteUser, this.olvObsCont
		});
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.EmptyListMsg = "";
		this.lsvData.EmptyListMsgFont = new System.Drawing.Font("Verdana", 8.25f);
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
		this.lsvData.Size = new System.Drawing.Size(741, 126);
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
		this.lsvData.SelectedIndexChanged += new System.EventHandler(lsvData_SelectedIndexChanged);
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
		this.olvHasXml.ToolTipText = "Tem evento do XML";
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
		this.olvVenc.ToolTipText = "Valor do documento fiscal";
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
		this.olvStatus.ToolTipText = "Status do documento";
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
		this.olvManifest.ToolTipText = "Manifestação do destinatário";
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
		this.olvHasCTeEvent.ToolTipText = "Tem evento de Cte vinculado";
		this.olvHasCTeEvent.Width = 35;
		this.olvMDFeEvtData.AspectName = "MDFeEvtData";
		this.olvMDFeEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.Text = "MDFe";
		this.olvMDFeEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvMDFeEvtData.ToolTipText = "Data do evento do manifesto eletrônico de documentos";
		this.olvMDFeEvtData.Width = 35;
		this.olvSufVistEvtData.AspectName = "SufVistEvtData";
		this.olvSufVistEvtData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistEvtData.Text = "Suf Vist";
		this.olvSufVistEvtData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistEvtData.ToolTipText = "Data do evento de vistoria na Suframa";
		this.olvSufVistEvtData.Width = 76;
		this.olvSufVistInteData.AspectName = "SufInteEvtData";
		this.olvSufVistInteData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistInteData.Text = "Suf Inter";
		this.olvSufVistInteData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSufVistInteData.ToolTipText = "Data do evento de internalização na Suframa";
		this.olvSufVistInteData.Width = 76;
		this.olvMotivo.AspectName = "xMotivo";
		this.olvMotivo.Text = "Status";
		this.olvMotivo.ToolTipText = "Motivo";
		this.olvMotivo.Width = 200;
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do Documento Fiscal";
		this.olvEstado.Width = 30;
		this.olvDtEmi.AspectName = "DtEmi";
		this.olvDtEmi.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmi.Text = "DtEmi";
		this.olvDtEmi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmi.ToolTipText = "Data de emissão";
		this.olvDtEmi.Width = 71;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "Chave";
		this.olvChave.ToolTipText = "Chave do documento fiscal";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 130;
		this.olvTomaIE.AspectName = "TomaIE";
		this.olvTomaIE.Text = "Tomador IE";
		this.olvTomaIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTomaIE.ToolTipText = "IE do Tomador";
		this.olvTpDoc.AspectName = "TpDoc";
		this.olvTpDoc.Text = "TipoDoc";
		this.olvTpDoc.ToolTipText = "Tipo de documento";
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.Text = "Natureza";
		this.olvNatOper.ToolTipText = "Natureza da operação";
		this.olvBaseIcms.AspectName = "BaseICMS";
		this.olvBaseIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBaseIcms.Text = "Base ICMS";
		this.olvBaseIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvBaseIcms.ToolTipText = "Base ICMS";
		this.olvBaseIcms.Width = 80;
		this.olvValorIcms.AspectName = "ValorICMS";
		this.olvValorIcms.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValorIcms.Text = "Valor ICMS";
		this.olvValorIcms.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValorIcms.ToolTipText = "Valor ICMS";
		this.olvValorIcms.Width = 80;
		this.olvCFOP.AspectName = "CFOPList";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.ToolTipText = "Código Fiscal de Operações e Prestações";
		this.olvCFOP.Width = 90;
		this.olvNCM.AspectName = "NCMList";
		this.olvNCM.Text = "NCM";
		this.olvNCM.ToolTipText = "Nomenclatura Comum do Mercosul";
		this.olvNCM.Width = 90;
		this.olvInfCpl.Text = "Informações complementares";
		this.olvInfCpl.ToolTipText = "Informações complementares";
		this.olvInfCpl.Width = 150;
		this.olvInfAdFisco.Text = "Informações adicionais fisco";
		this.olvInfAdFisco.ToolTipText = "Informações adicionais";
		this.olvInfAdFisco.Width = 150;
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Ano-Mês do documento fiscal";
		this.olvError.AspectName = "XmlError";
		this.olvError.Text = "Validação";
		this.olvError.ToolTipText = "Status de validação do XML";
		this.olvBatchNum.AspectName = "BatchNum";
		this.olvBatchNum.Text = "Lote Proc.";
		this.olvBatchNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBatchNum.ToolTipText = "Número do lote de processamento";
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvDocNote.ToolTipText = "Comentários atribuídos";
		this.olvDestNome.AspectName = "DestinNome";
		this.olvDestNome.IsVisible = false;
		this.olvDestNome.Text = "Destinatário Nome";
		this.olvDestNome.ToolTipText = "Nome do destinatário";
		this.olvDestID.AspectName = "DestinID";
		this.olvDestID.IsVisible = false;
		this.olvDestID.Text = "Destinatário CNPJ/CPF";
		this.olvDestID.ToolTipText = "CNPJ/CPF do destinatário";
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
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add(this.panel1);
		this.splitContainer1.Panel1MinSize = 415;
		this.splitContainer1.Panel2Collapsed = true;
		this.splitContainer1.Panel2MinSize = 0;
		this.splitContainer1.Size = new System.Drawing.Size(741, 90);
		this.splitContainer1.SplitterDistance = 415;
		this.splitContainer1.TabIndex = 2;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.lnkJumpOut);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.lkbHelpScanOut);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.lbSeeMore);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(741, 90);
		this.panel1.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(635, 35);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(38, 14);
		this.label5.TabIndex = 240;
		this.label5.Text = "aqui.";
		this.lnkJumpOut.AutoSize = true;
		this.lnkJumpOut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lnkJumpOut.Location = new System.Drawing.Point(421, 35);
		this.lnkJumpOut.Name = "lnkJumpOut";
		this.lnkJumpOut.Size = new System.Drawing.Size(216, 14);
		this.lnkJumpOut.TabIndex = 239;
		this.lnkJumpOut.TabStop = true;
		this.lnkJumpOut.Text = "relatório de saltos de numeração";
		this.lnkJumpOut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkJumpOut_LinkClicked);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(12, 63);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(712, 1);
		this.label1.TabIndex = 238;
		this.lkbHelpScanOut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lkbHelpScanOut.Location = new System.Drawing.Point(12, 68);
		this.lkbHelpScanOut.Name = "lkbHelpScanOut";
		this.lkbHelpScanOut.Size = new System.Drawing.Size(326, 14);
		this.lkbHelpScanOut.TabIndex = 237;
		this.lkbHelpScanOut.TabStop = true;
		this.lkbHelpScanOut.Text = "Saiba mais. Veja como ativar a busca das saídas";
		this.lkbHelpScanOut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbHelpScanOut_LinkClicked);
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(12, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(421, 17);
		this.label2.TabIndex = 104;
		this.label2.Text = "Para verificar detalhes sobre os saltos de numeração, acesse o";
		this.lbSeeMore.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbSeeMore.Image = Monitor.Resources.image_see_more;
		this.lbSeeMore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbSeeMore.Location = new System.Drawing.Point(7, 4);
		this.lbSeeMore.Margin = new System.Windows.Forms.Padding(0);
		this.lbSeeMore.Name = "lbSeeMore";
		this.lbSeeMore.Size = new System.Drawing.Size(169, 24);
		this.lbSeeMore.TabIndex = 1;
		this.lbSeeMore.Text = "       Saltos de numeração";
		this.lbSeeMore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 128);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(741, 338);
		this.pnMarket.TabIndex = 2;
		this.pnMarket.Visible = false;
		this.lknClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lknClose.AutoSize = true;
		this.lknClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknClose.Location = new System.Drawing.Point(559, 315);
		this.lknClose.Name = "lknClose";
		this.lknClose.Size = new System.Drawing.Size(176, 16);
		this.lknClose.TabIndex = 204;
		this.lknClose.TabStop = true;
		this.lknClose.Text = "Não mostrar mais esse alerta";
		this.lknClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lknClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknClose_LinkClicked);
		this.pnMarketContent.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pnMarketContent.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarketContent.Controls.Add(this.lbTitle03);
		this.pnMarketContent.Controls.Add(this.label3);
		this.pnMarketContent.Controls.Add(this.picWarning);
		this.pnMarketContent.Controls.Add(this.lknAction);
		this.pnMarketContent.Controls.Add(this.lbText02);
		this.pnMarketContent.Controls.Add(this.lbText01);
		this.pnMarketContent.Controls.Add(this.lbTitle01);
		this.pnMarketContent.Controls.Add(this.lbTitle02);
		this.pnMarketContent.Location = new System.Drawing.Point(90, 43);
		this.pnMarketContent.Name = "pnMarketContent";
		this.pnMarketContent.Size = new System.Drawing.Size(566, 233);
		this.pnMarketContent.TabIndex = 195;
		this.lbTitle03.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle03.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle03.Location = new System.Drawing.Point(17, 195);
		this.lbTitle03.Name = "lbTitle03";
		this.lbTitle03.Size = new System.Drawing.Size(88, 17);
		this.lbTitle03.TabIndex = 212;
		this.lbTitle03.Text = "Saiba mais:";
		this.lbTitle03.Click += new System.EventHandler(lbTitle03_Click);
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.BackColor = System.Drawing.Color.Gainsboro;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Gainsboro;
		this.label3.Location = new System.Drawing.Point(3, 27);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(560, 1);
		this.label3.TabIndex = 208;
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.picWarning.Image = Monitor.Resources.image_help;
		this.picWarning.Location = new System.Drawing.Point(20, 4);
		this.picWarning.Name = "picWarning";
		this.picWarning.Size = new System.Drawing.Size(20, 20);
		this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picWarning.TabIndex = 206;
		this.picWarning.TabStop = false;
		this.lknAction.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lknAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknAction.Location = new System.Drawing.Point(102, 195);
		this.lknAction.Name = "lknAction";
		this.lknAction.Size = new System.Drawing.Size(440, 16);
		this.lknAction.TabIndex = 203;
		this.lknAction.TabStop = true;
		this.lknAction.Text = "Dicas para obter 100% dos XMLs de documentos emitidos pela empresa";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(17, 124);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 54);
		this.lbText02.TabIndex = 196;
		this.lbText02.Text = resources.GetString("lbText02.Text");
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 71);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 35);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = "O relatório Emitidos pela Empresa, em sua utilização padrão, apresenta os resumos de NFe, CTe, NFCe, CFeSAT e MDFe que possuem eventos associados a eles.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(517, 23);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Relatório : Emitidos pela Empresa";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(17, 45);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Importante saber";
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(710, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 3;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(741, 466);
		base.Controls.Add(this.pnMarket);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocOut";
		this.Text = "Emitidos pela empresa";
		base.Load += new System.EventHandler(frmTabDocOut_Load);
		this.pnContent.ResumeLayout(false);
		this.splitData.Panel1.ResumeLayout(false);
		this.splitData.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitData).EndInit();
		this.splitData.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.splitContainer1.Panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
