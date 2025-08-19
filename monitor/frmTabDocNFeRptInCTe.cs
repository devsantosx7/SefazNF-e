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

namespace Monitor;

public class frmTabDocNFeRptInCTe : Form
{
	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocNFeRptInCTe _SqlTabData = new clsSrvTabDocNFeRptInCTe();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private bool _SelectedAllItens;

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private bool _HasFiscalioConnect;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

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

	private OLVColumn olvEmitida;

	private OLVColumn olvTipo;

	private OLVColumn olvTag;

	private OLVColumn olvNum;

	private OLVColumn olvSerie;

	private OLVColumn olvDtAut;

	private OLVColumn olvStatus;

	private OLVColumn olvValor;

	private OLVColumn olvReason;

	private OLVColumn olvCNPJ;

	private OLVColumn olvNome;

	private OLVColumn olvChave;

	private OLVColumn olvFilial;

	private ImageList ImageListDocs;

	private OLVColumn olvCFOP;

	private OLVColumn olvError;

	private OLVColumn olvCancel;

	private OLVColumn olvManifest;

	private OLVColumn olvDocNote;

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

	private OLVColumn olvEstado;

	private OLVColumn olvAnoMes;

	private Button btSalesContact;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabDocNFeRptInCTe(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		olvAnoMes.Name = "AnoMes";
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private async void frmTabDocNFeRptInCTe_Load(object sender, EventArgs e)
	{
		_StateList = await new clsDataEstado().funcGetListAsync(pWthAN: false);
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
			clsScreenGeral.funcCheckListViewDdic(typeof(DocCTeDuplicado), lsvData);
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
		olvValor.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return clsFunction.IsEmpty(docCTeDuplicado?.Valor) ? null : ((object)clsFunction.funcConvStrToDec(docCTeDuplicado.Valor));
		};
		olvHasXml.ImageGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : ((object)clsScreenGeral.funcGetDocXmlIcon(docCTeDuplicado, _HasNFSeFeatEnabled, _HasCFeFeatEnabled));
		};
		olvHasXml.AspectGetter = (object x) => string.Empty;
		olvEmitida.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			if (docCTeDuplicado == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docCTeDuplicado.Emitida) ? "Terceiros" : "Empresa";
		};
		olvTag.ImageGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : ((object)clsScreenGeral.funcGetDocNotesIcon(docCTeDuplicado));
		};
		olvTag.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			if (docCTeDuplicado == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docCTeDuplicado.Tag) ? null : _TagHsList[docCTeDuplicado.Tag];
		};
		olvNum.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : clsFunction.funcGetValue(docCTeDuplicado.Num).PadLeft(9, '0');
		};
		olvTipo.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : clsFunction.funcGetDocType(docCTeDuplicado.Model);
		};
		olvStatus.Renderer = new ImageRenderer();
		olvStatus.AspectGetter = delegate(object x)
		{
			new List<int>();
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : clsScreenGeral.funcGetDocStatIcon(docCTeDuplicado);
		};
		olvManifest.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : _clsDFeCodes.GetEventDesc(docCTeDuplicado.Model, docCTeDuplicado.ManifCode);
		};
		olvCNPJ.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			if (docCTeDuplicado == null)
			{
				return (object)null;
			}
			if (string.IsNullOrEmpty(docCTeDuplicado.Emitida))
			{
				return docCTeDuplicado.EmitID;
			}
			if (!string.IsNullOrEmpty(docCTeDuplicado.TomaID))
			{
				return docCTeDuplicado.TomaID;
			}
			return (!string.IsNullOrEmpty(docCTeDuplicado.DestinID)) ? docCTeDuplicado.DestinID : docCTeDuplicado.RecebeID;
		};
		olvNome.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			if (docCTeDuplicado == null)
			{
				return (object)null;
			}
			if (string.IsNullOrEmpty(docCTeDuplicado.Emitida))
			{
				return docCTeDuplicado.EmitNome;
			}
			if (!string.IsNullOrEmpty(docCTeDuplicado.TomaNome))
			{
				return docCTeDuplicado.TomaNome;
			}
			return (!string.IsNullOrEmpty(docCTeDuplicado.DestinNome)) ? docCTeDuplicado.DestinNome : docCTeDuplicado.RecebeNome;
		};
		olvFilial.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : clsFunction.funcFormatDoc(docCTeDuplicado.Filial);
		};
		olvManifest.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : _clsDFeCodes.GetEventDesc(docCTeDuplicado.Model, docCTeDuplicado.ManifCode);
		};
		olvError.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			return (docCTeDuplicado == null) ? null : varclsValidator.funcGetDesc(docCTeDuplicado);
		};
		olvEstado.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado varDocument = (DocCTeDuplicado)x;
			return (varDocument == null) ? null : _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varDocument.cUF))?.Sigla;
		};
		olvAnoMes.AspectGetter = delegate(object x)
		{
			DocCTeDuplicado docCTeDuplicado = (DocCTeDuplicado)x;
			if (docCTeDuplicado == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = docCTeDuplicado.DtAut.Substring(0, 7);
			}
			catch
			{
			}
			return result;
		};
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturn = new clsReturn();
		string varSqlQuery = await _SqlTabData.funcGetSqlStrSelectAsync(pclsDataFilter);
		if (clsFunction.IsEmpty(varSqlQuery))
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
		List<DocCTeDuplicado> varclsDocList = await new clsDataCTeDuplicado().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
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
		OLVColumn varGroupColum = null;
		SortOrder varSortOrder = SortOrder.None;
		if (pclsDataFilter.GroupByDisable || _IsLocked)
		{
			varGroupColum = null;
			varSortOrder = SortOrder.None;
		}
		else if (pclsDataFilter.GroupByPartner)
		{
			varGroupColum = olvNome;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByYearMonth)
		{
			varGroupColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByDate)
		{
			varGroupColum = olvDtAut;
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
		plsvData = clsScreenGeral.funcSetLsvSortGroup(this, plsvData, varGroupColum, varSortOrder, olvDtAut, SortOrder.Descending);
		return plsvData;
	}

	private void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		DocCTeDuplicado varDocCTeDuplicado = (DocCTeDuplicado)e.Model;
		if (varDocCTeDuplicado != null)
		{
			_ = string.Empty;
			if (e.ColumnIndex.Equals(olvHasXml.Index))
			{
				e.Text = varclsValidator.funcGetDesc(varDocCTeDuplicado);
			}
			else if (e.ColumnIndex.Equals(olvStatus.Index))
			{
				e.Text = clsScreenGeral.funcGetDocStatDesc(varDocCTeDuplicado);
			}
			else if (e.ColumnIndex.Equals(olvTag.Index))
			{
				e.Text = varDocCTeDuplicado.DocNote;
			}
		}
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
		List<DocCTeDuplicado> varObjList = funcGetObjList(pFocused, pChecked);
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
		List<DocCTeDuplicado> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<DocCTeDuplicado> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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

	private async void tsbDocReset_Click(object sender, EventArgs e)
	{
		List<DocCTeDuplicado> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		return await clsScreenGeral.funcExportDocToExcelAsync<DocCTeDuplicado>(lsvData, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<DocCTeDuplicado> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<DocCTeDuplicado> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (DocCTeDuplicado varObject in pObjList)
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

	private List<DocCTeDuplicado> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<DocCTeDuplicado> varDocList = new List<DocCTeDuplicado>();
		DocCTeDuplicado varFocused = new DocCTeDuplicado();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (DocCTeDuplicado)lsvData.GetItem(lsvData.FocusedItem.Index).RowObject;
				varDocList.Add(varFocused);
			}
			catch
			{
				varFocused = new DocCTeDuplicado();
			}
		}
		if (pChecked)
		{
			foreach (DocCTeDuplicado varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varDocList.Add(varObject);
				}
			}
			if (varDocList.Count > 0)
			{
				varDocList = lsvData.Objects.Cast<DocCTeDuplicado>().Intersect(varDocList).ToList();
			}
		}
		if (!pFocused && !pChecked)
		{
			varDocList = lsvData.FilteredObjects.Cast<DocCTeDuplicado>().ToList();
			if (varDocList.Count <= 0)
			{
				varDocList = lsvData.Objects.Cast<DocCTeDuplicado>().ToList();
			}
		}
		return varDocList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<DocCTeDuplicado> varObjList = funcGetObjList(pFocused, pChecked);
		funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
		return true;
	}

	private async void funcRefreshListAsync(List<Document> pDocList, List<DocCTeDuplicado> pObjList)
	{
		ConcurrentBag<ObjFieldBuffer> varDocFieldList = clsObjectBuffer.funcGet<Document>().Fields;
		ConcurrentBag<ObjFieldBuffer> varObjFieldList = clsObjectBuffer.funcGet<DocCTeDuplicado>().Fields;
		await Task.WhenAll(((IEnumerable<DocCTeDuplicado>)pObjList).Select((Func<DocCTeDuplicado, Task>)async delegate(DocCTeDuplicado varclsObjItem)
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

	private void lstView_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false);
	}

	private async void funcShowDocViewerAsync(bool pShowPDF, bool pShowXML, DocCTeDuplicado pDocCTeDuplicado = null)
	{
		new List<Document>();
		List<Document> varDocList;
		if (pDocCTeDuplicado != null)
		{
			List<DocCTeDuplicado> varObjList = new List<DocCTeDuplicado> { pDocCTeDuplicado };
			varDocList = await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true);
		}
		else
		{
			varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		}
		await clsMonGeral.funcDocViewerAsync(this, varDocList, pShowPDF, pShowXML);
	}

	private void lsvData_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
	{
		if (e.ColumnIndex == olvNum.Index)
		{
			DocCTeDuplicado varDocCTeDuplicado = (DocCTeDuplicado)e.Model;
			if (varDocCTeDuplicado != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varDocCTeDuplicado);
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

	private async Task<bool> funcTransferFilialAsync()
	{
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		varArguments.TaskProgress = "Transferindo DocCTeDuplicadoo(s) para o destino ....";
		OnEventTabManager(varArguments);
		foreach (clsValue varclsValue in (await new clsManGeral().funcTransferFilialAsync(varDocList)).Values)
		{
			if (clsFunction.IsEqual(varclsValue.Name, "DocCTeDuplicado", pIgnoreCase: true))
			{
				lsvData.RemoveObject(varclsValue.ObjDoc);
			}
		}
		varArguments.TaskProgress = string.Empty;
		OnEventTabManager(varArguments);
		return true;
	}

	private void lsvData_FormatCell(object sender, FormatCellEventArgs e)
	{
		if (e.ColumnIndex == olvTag.Index)
		{
			DocCTeDuplicado varDocument = (DocCTeDuplicado)e.Model;
			if (varDocument != null && !clsFunction.IsEmpty(varDocument.Tag))
			{
				string varColorName = (string)_HasColors[varDocument.Tag];
				e.SubItem.BackColor = ColorTranslator.FromHtml(varColorName);
			}
		}
		else if (e.ColumnIndex == olvReason.Index)
		{
			e.SubItem.BackColor = _ColumnColor;
		}
	}

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		DocCTeDuplicado varDocument = (DocCTeDuplicado)e.Model;
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

	private void lsvData_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false);
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
			List<DocCTeDuplicado> varObjList = varEnumList.Cast<DocCTeDuplicado>().ToList();
			if (varObjList == null)
			{
				varObjList = new List<DocCTeDuplicado>();
			}
			_TotalValue = varObjList.Sum((DocCTeDuplicado r) => clsFunction.funcConvStrToDec(r.Valor));
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
		clsHelpService.funcCallTipReportCTeCobrancDuplicAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocNFeRptInCTe));
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
		this.olvHasXml = new BrightIdeasSoftware.OLVColumn();
		this.olvTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvDtAut = new BrightIdeasSoftware.OLVColumn();
		this.olvValor = new BrightIdeasSoftware.OLVColumn();
		this.olvManifest = new BrightIdeasSoftware.OLVColumn();
		this.olvCancel = new BrightIdeasSoftware.OLVColumn();
		this.olvCNPJ = new BrightIdeasSoftware.OLVColumn();
		this.olvNome = new BrightIdeasSoftware.OLVColumn();
		this.olvReason = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitida = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvError = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNote = new BrightIdeasSoftware.OLVColumn();
		this.olvEstado = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
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
		this.pnMarket.TabIndex = 15;
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
		this.btSalesContact.TabIndex = 218;
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
		this.lknAction.Text = "CTe cobrança duplicada";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(17, 134);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 37);
		this.lbText02.TabIndex = 196;
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 81);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 35);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = "Lista todos os CTe que citam uma NFe que já foi citada antes em outro CTe, enquadrando-se como \"emitido em duplicidade\".";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Cobranças duplicadas de CTe é ativado nos \r\nplanos Avançado e Enterprise.";
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
		this.lsvData.AllColumns.Add(this.olvHasXml);
		this.lsvData.AllColumns.Add(this.olvTipo);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvSerie);
		this.lsvData.AllColumns.Add(this.olvDtAut);
		this.lsvData.AllColumns.Add(this.olvValor);
		this.lsvData.AllColumns.Add(this.olvManifest);
		this.lsvData.AllColumns.Add(this.olvCancel);
		this.lsvData.AllColumns.Add(this.olvCNPJ);
		this.lsvData.AllColumns.Add(this.olvNome);
		this.lsvData.AllColumns.Add(this.olvReason);
		this.lsvData.AllColumns.Add(this.olvChave);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvEmitida);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvError);
		this.lsvData.AllColumns.Add(this.olvDocNote);
		this.lsvData.AllColumns.Add(this.olvEstado);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[22]
		{
			this.olvSelect, this.olvHasXml, this.olvTipo, this.olvTag, this.olvNum, this.olvSerie, this.olvDtAut, this.olvValor, this.olvManifest, this.olvCancel,
			this.olvCNPJ, this.olvNome, this.olvReason, this.olvChave, this.olvFilial, this.olvCFOP, this.olvEmitida, this.olvStatus, this.olvError, this.olvDocNote,
			this.olvEstado, this.olvAnoMes
		});
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.EmptyListMsg = "";
		this.lsvData.EmptyListMsgFont = new System.Drawing.Font("Verdana", 8.25f);
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
		this.olvHasXml.Width = 36;
		this.olvTipo.AspectName = "Model";
		this.olvTipo.Text = "Tipo";
		this.olvTipo.Width = 43;
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.Width = 39;
		this.olvNum.AspectName = "Num";
		this.olvNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.Hyperlink = true;
		this.olvNum.Text = "Num";
		this.olvNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.Width = 74;
		this.olvSerie.AspectName = "Serie";
		this.olvSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.Text = "Serie";
		this.olvSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSerie.Width = 44;
		this.olvDtAut.AspectName = "DtAut";
		this.olvDtAut.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.Text = "DtAut";
		this.olvDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtAut.Width = 71;
		this.olvValor.AspectName = "Valor";
		this.olvValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValor.Text = "Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.Width = 80;
		this.olvManifest.AspectName = "";
		this.olvManifest.Text = "Manifestação";
		this.olvCancel.AspectName = "Canceled";
		this.olvCancel.Text = "Canc";
		this.olvCancel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCancel.Width = 35;
		this.olvCNPJ.AspectName = "EmitID";
		this.olvCNPJ.Text = "CNPJ/CPF";
		this.olvCNPJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCNPJ.Width = 130;
		this.olvNome.AspectName = "EmitNome";
		this.olvNome.Text = "Nome";
		this.olvNome.Width = 230;
		this.olvReason.AspectName = "Reason";
		this.olvReason.Text = "Resultado da análise";
		this.olvReason.Width = 200;
		this.olvChave.AspectName = "Chave";
		this.olvChave.Text = "Chave";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "Filial";
		this.olvCFOP.AspectName = "CFOPList";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.Width = 90;
		this.olvEmitida.AspectName = "Emitida";
		this.olvEmitida.Text = "Emissor";
		this.olvStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStatus.Searchable = false;
		this.olvStatus.Sortable = false;
		this.olvStatus.Text = "Status";
		this.olvStatus.UseFiltering = false;
		this.olvStatus.Width = 77;
		this.olvError.AspectName = "XmlError";
		this.olvError.Text = "Validação";
		this.olvDocNote.AspectName = "DocNote";
		this.olvDocNote.Text = "Comentários";
		this.olvEstado.AspectName = "cUF";
		this.olvEstado.Text = "UF";
		this.olvEstado.ToolTipText = "Código da UF do emitente do Documento Fiscal";
		this.olvAnoMes.AspectName = "DtAut";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
		base.Name = "frmTabDocNFeRptInCTe";
		this.Text = "Cobranças duplicadas de CTe";
		base.Load += new System.EventHandler(frmTabDocNFeRptInCTe_Load);
		this.pnContent.ResumeLayout(false);
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		base.ResumeLayout(false);
	}
}
