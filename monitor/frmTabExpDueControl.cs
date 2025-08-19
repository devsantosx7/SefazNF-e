using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using audit.fiscal.io;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using monitor.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using srv.fiscal.io.Siscomex;
using util.fiscal.io;

namespace Monitor;

public class frmTabExpDueControl : Form
{
	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsSrvTabExpDueControl _SqlTabData;

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsDataDueHeader _clsDataDueHeader = new clsDataDueHeader();

	private clsDataDueItem _clsDataDueItem = new clsDataDueItem();

	private clsDataDueItemRem _clsDataDueItemRem = new clsDataDueItemRem();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsComexProfile _clsComexProfile;

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private clsComexEmailService _clsEmailService = new clsComexEmailService();

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private bool _IsLpcoControlEnabled;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private clsDataFilter _clsDataFilter = new clsDataFilter();

	private string _SqlFields = " * ";

	private bool _IsLoading;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private ImageList ImageListDocs;

	private Panel pnMarket;

	private LinkLabel lknClose;

	private Panel pnMarketContent;

	private Label label3;

	private PictureBox picWarning;

	private Label lbText01;

	private Label lbTitle01;

	private Label lbTitle02;

	private Button btClose;

	private Label lbTitle03;

	private LinkLabel lknAction;

	private Button btSalesContact;

	private Button btSalesAction;

	private FastObjectListView lsvData;

	private OLVColumn olvSelect;

	private OLVColumn olvFilial;

	private OLVColumn olvNum;

	private OLVColumn olvDataDue;

	private OLVColumn olvDataAverb;

	private OLVColumn olvCanal;

	private OLVColumn olvChave;

	private OLVColumn olvForma;

	private OLVColumn olvValor;

	private OLVColumn olvBloqueio;

	private OLVColumn olvStatus;

	private OLVColumn olvHasDossie;

	private OLVColumn olvTipoDoc;

	private OLVColumn olvStatusAdm;

	private OLVColumn olvStatusCarga;

	private OLVColumn olvPaisDest;

	private OLVColumn olvPaisDestName;

	private OLVColumn olvRucNum;

	private OLVColumn olvNatOper;

	private OLVColumn olvConEmbNum;

	private OLVColumn olvConEmbTipo;

	private OLVColumn olvConEmbData;

	private OLVColumn olvTipoDocTrans;

	private OLVColumn olvViaTransCode;

	private OLVColumn olvDeclaraId;

	private OLVColumn olvDeclaraName;

	private OLVColumn olvExportaId;

	private OLVColumn olvExportaName;

	private OLVColumn olvLocDespCode;

	private OLVColumn olvLocDespDesc;

	private OLVColumn olvLocEmbarCode;

	private OLVColumn olvLocEmbarDesc;

	private OLVColumn olvRecAduanDespCode;

	private OLVColumn olvRecAduanDespDesc;

	private OLVColumn olvRecAduanDespZona;

	private OLVColumn olvRecAduanDespDepId;

	private OLVColumn olvRecAduanDespDepName;

	private OLVColumn olvRecAduanEmbarCode;

	private OLVColumn olvRecAduanEmbarDesc;

	private OLVColumn olvRecAduanEmbarZona;

	private OLVColumn olvRecAduanEmbarDepId;

	private OLVColumn olvRecAduanEmbarDepName;

	private OLVColumn olvAnoMes;

	private OLVColumn olvDtLastSync;

	private OLVColumn olvDocNotes;

	private ToolStrip miniToolStrip;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton tsbDocs;

	private ToolStripSeparator tssSepDocs;

	private ToolStripButton tsbSpedCreate;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStrip tspTaskMenu;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private OLVColumn olvImportaName;

	private OLVColumn olvTipoFrete;

	private ToolStripMenuItem tsmCopyDue;

	private ToolStripMenuItem tsmCopyDFe;

	private ToolStripButton tsbSync;

	private OLVColumn olvLpcoStat;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabExpDueControl(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		olvAnoMes.Name = "AnoMes";
		lsvData.HandleDestroyed += lsvData_HandleDestroyed;
		_SqlTabData = new clsSrvTabExpDueControl(_SqlFields);
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

	private async void frmTabExpDueControl_Load(object sender, EventArgs e)
	{
		_IsLoading = false;
		_ColumnsLoaded = false;
		clsScreenGeral.funcSetColumnsObjectListView(this, lsvData);
		_ColumnsLoaded = true;
		_IsLpcoControlEnabled = await clsScreenGeral.funcHasLpcoControlFeatureAsync();
		foreach (OLVColumn allColumn in lsvData.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
		if (clsFunction.IsAdmin)
		{
			clsScreenGeral.funcCheckListViewDdic(typeof(DueHeader), lsvData);
		}
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

	private void lsvData_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		DueHeader varDueHeader = (DueHeader)e.Model;
		if (varDueHeader == null)
		{
			return;
		}
		_ = string.Empty;
		if (e.ColumnIndex.Equals(olvHasDossie.Index))
		{
			if (!clsFunction.IsEmpty(varDueHeader.HasDossie))
			{
				e.Text = "Tem anexos vinculados a DUE";
			}
			else
			{
				e.Text = string.Empty;
			}
		}
	}

	private void funcDefineListViewFeatures()
	{
		olvHasDossie.ImageGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			return (!clsFunction.IsEmpty(dueHeader.HasDossie)) ? ((object)9) : null;
		};
		olvValor.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(dueHeader?.Valor) ? null : ((object)clsFunction.funcConvStrToDec(dueHeader.Valor));
		};
		olvTipoDoc.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			return (dueHeader == null) ? null : clsComexCodes.funcGetTipoDocDesc(dueHeader.TipoDoc);
		};
		olvNatOper.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			return (dueHeader == null) ? null : clsComexCodes.funcGetNatOperDesc(dueHeader.NatOper);
		};
		olvTipoDocTrans.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			return (dueHeader == null) ? null : clsComexCodes.funcGetTipoDocTransDesc(dueHeader.TipoDocTrans);
		};
		olvViaTransCode.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			return (dueHeader == null) ? null : clsComexCodes.funcGetViaTransDesc(dueHeader.ViaTransCode);
		};
		olvStatus.ImageGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(dueHeader.Status))
			{
				return (object)null;
			}
			if (clsFunction.Contains(dueHeader.Status, "REGIST", pIgnoreCase: true))
			{
				return 4;
			}
			if (clsFunction.Contains(dueHeader.Status, "PENDEN", pIgnoreCase: true))
			{
				return 5;
			}
			if (clsFunction.Contains(dueHeader.Status, "AGUARD", pIgnoreCase: true))
			{
				return 5;
			}
			if (clsFunction.Contains(dueHeader.Status, "PARCIAL", pIgnoreCase: true))
			{
				return 6;
			}
			if (clsFunction.Contains(dueHeader.Status, "AVERBADA", pIgnoreCase: true))
			{
				return 7;
			}
			return clsFunction.Contains(dueHeader.Status, "CANCEL", pIgnoreCase: true) ? ((object)8) : ((object)6);
		};
		olvCanal.ImageGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			if (clsFunction.Contains(dueHeader.Canal, "Verde", pIgnoreCase: true))
			{
				return 0;
			}
			if (clsFunction.Contains(dueHeader.Canal, "Amarelo", pIgnoreCase: true))
			{
				return 1;
			}
			if (clsFunction.Contains(dueHeader.Canal, "Laranja", pIgnoreCase: true))
			{
				return 2;
			}
			return clsFunction.Contains(dueHeader.Canal, "Vermelho", pIgnoreCase: true) ? ((object)3) : null;
		};
		olvLpcoStat.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(dueHeader.LpcoStat))
			{
				return string.Empty;
			}
			return _IsLpcoControlEnabled ? dueHeader.LpcoStat : "Detalhes";
		};
		olvLpcoStat.ImageGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(dueHeader.LpcoStat))
			{
				return string.Empty;
			}
			return _IsLpcoControlEnabled ? null : ((object)10);
		};
		olvAnoMes.AspectGetter = delegate(object x)
		{
			DueHeader dueHeader = (DueHeader)x;
			if (dueHeader == null)
			{
				return (object)null;
			}
			string result = string.Empty;
			try
			{
				result = clsFunction.funcSubString(dueHeader.DataDue, 0, 7);
			}
			catch
			{
			}
			return result;
		};
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
		List<DueHeader> varclsDocList = await _clsDataDueHeader.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
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
			varGroupColum = olvExportaName;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByDate)
		{
			varGroupColum = olvDataDue;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByYearMonth)
		{
			varGroupColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		else if (pclsDataFilter.GroupByState)
		{
			varGroupColum = olvPaisDestName;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByModel)
		{
			varGroupColum = olvForma;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByTpDoc)
		{
			varGroupColum = olvTipoDoc;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByNatOper)
		{
			varGroupColum = olvNatOper;
			varSortOrder = SortOrder.Ascending;
		}
		else if (pclsDataFilter.GroupByFilial)
		{
			varGroupColum = olvFilial;
			varSortOrder = SortOrder.Ascending;
		}
		else
		{
			varGroupColum = olvAnoMes;
			varSortOrder = SortOrder.Descending;
		}
		plsvData = clsScreenGeral.funcSetLsvSortGroup(this, plsvData, varGroupColum, varSortOrder, olvDataDue, SortOrder.Descending);
		return plsvData;
	}

	public void funcSearchText(string pSearchTerm, bool pSearchInteligent)
	{
		if (clsFunction.IsEmpty(pSearchTerm))
		{
			lsvData.AdditionalFilter = null;
		}
		else
		{
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
		return await clsScreenGeral.funcExportDocToExcelAsync<DueHeader>(lsvData, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		new clsDataDoc();
		List<DueHeader> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<DueHeader> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (DueHeader varclsDueHeader in pObjList)
		{
			foreach (DueItem varclsDueItem in await _clsDataDueItem.funcGetListByKeyAsync(varclsDueHeader.Filial, varclsDueHeader.Num))
			{
				Document varclsDoc = new Document
				{
					Filial = varclsDueItem.Filial,
					Chave = varclsDueItem.ExpNFe
				};
				varDocList.Add(varclsDoc);
			}
			foreach (DueItemRem varclsDueItemRem in await _clsDataDueItemRem.funcGetListByKeyAsync(varclsDueHeader.Filial, varclsDueHeader.Num))
			{
				Document varclsDoc2 = new Document
				{
					Filial = varclsDueItemRem.Filial,
					Chave = varclsDueItemRem.RemNFe
				};
				varDocList.Add(varclsDoc2);
			}
		}
		if (pSyncFromDbaFirst)
		{
			varDocList = await _clsDataDoc.funcGetSyncListByListAsync(varDocList);
		}
		return varDocList;
	}

	public async Task<List<DueHeader>> funcGetDueListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst, bool pOnlyPend = false)
	{
		List<DueHeader> varDocList = funcGetObjList(pFocused, pChecked);
		if (pSyncFromDbaFirst)
		{
			varDocList = await _clsDataDueHeader.funcGetSyncListByListAsync(varDocList);
		}
		if (pOnlyPend && !clsFunction.IsAdmin)
		{
			varDocList.RemoveAll((DueHeader r) => clsFunction.Contains(r.Status, "SUCCESS", "AVERBADA", pIgnoreCase: true));
		}
		return varDocList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<DueHeader> varDocList = funcGetObjList(pFocused, pChecked);
		await _clsDataDueHeader.funcGetSyncListByListAsync(varDocList);
		return true;
	}

	private List<DueHeader> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<DueHeader> varDocList = new List<DueHeader>();
		DueHeader varFocused = new DueHeader();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (DueHeader)lsvData.GetItem(lsvData.FocusedItem.Index).RowObject;
				varDocList.Add(varFocused);
			}
			catch
			{
				varFocused = new DueHeader();
			}
		}
		if (pChecked)
		{
			foreach (DueHeader varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varDocList.Add(varObject);
				}
			}
			if (varDocList.Count > 0)
			{
				varDocList = lsvData.Objects.Cast<DueHeader>().Intersect(varDocList).ToList();
			}
		}
		if ((!pFocused && !pChecked) || varDocList.Count <= 0)
		{
			varDocList = lsvData.FilteredObjects.Cast<DueHeader>().ToList();
			if (varDocList.Count <= 0)
			{
				varDocList = lsvData.Objects.Cast<DueHeader>().ToList();
			}
		}
		return varDocList;
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

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		DueHeader varDueHeader = (DueHeader)e.Model;
		if (varDueHeader != null)
		{
			if (clsFunction.Contains(varDueHeader.Status, "CANCEL"))
			{
				e.Item.BackColor = (e.Item.Checked ? Color.PeachPuff : Color.PapayaWhip);
			}
			else
			{
				e.Item.BackColor = (e.Item.Checked ? Color.LightBlue : Color.White);
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
			List<DueHeader> varObjList = varEnumList.Cast<DueHeader>().ToList();
			if (varObjList == null)
			{
				varObjList = new List<DueHeader>();
			}
			_TotalValue = varObjList.Sum((DueHeader r) => clsFunction.funcConvStrToDec(r.Valor));
			EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
			varArguments.TotalValue = _TotalValue;
			varArguments.TotalQuant = varObjList.Count;
			OnEventTabManager(varArguments);
		}
	}

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipReportDueControlAsync();
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

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
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

	private async void tsbSync_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsComexApi varclsComexApi = new clsComexApi();
		try
		{
			tsbSync.Enabled = false;
			tsbSync.Text = "Sincronizando...";
			lbProgress.Text = "Definindo DUEs para Sincronização ...";
			Application.DoEvents();
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
			varclsExcDueList = varclsExcDueList.OrderBy((DueHeader r) => r.Filial).ToList();
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

	private async void lsvData_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode.Equals(Keys.C))
		{
			await funcCopyDueKeyAsync();
		}
		else if (e.Control && e.KeyCode.Equals(Keys.N))
		{
			await funcCopyNFeKeyAsync();
		}
	}

	private async void tsmCopyDue_Click(object sender, EventArgs e)
	{
		await funcCopyDueKeyAsync();
	}

	private async Task<bool> funcCopyDueKeyAsync()
	{
		List<DueHeader> varDocList = await funcGetDueListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: false);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		string varDocKeyString = string.Empty;
		foreach (DueHeader varItem in varDocList)
		{
			varDocKeyString = varDocKeyString + varItem.Num + Environment.NewLine;
		}
		return await clsScreenGeral.funcCopyDocKeyAsync(varDocKeyString, "DUE");
	}

	private async void tsmCopyDFe_Click(object sender, EventArgs e)
	{
		await funcCopyNFeKeyAsync();
	}

	private async Task<bool> funcCopyNFeKeyAsync()
	{
		List<Document> varDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: false);
		if (varDocList.Count <= 0)
		{
			return false;
		}
		string varDocKeyString = string.Empty;
		foreach (Document varItem in varDocList)
		{
			varDocKeyString = varDocKeyString + varItem.Chave + Environment.NewLine;
		}
		return await clsScreenGeral.funcCopyDocKeyAsync(varDocKeyString);
	}

	private async void lsvData_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
	{
		DueHeader varclsDueHeader = (DueHeader)e.Model;
		if (varclsDueHeader == null)
		{
			return;
		}
		if (e.ColumnIndex == olvNum.Index)
		{
			funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varclsDueHeader);
		}
		else if (e.ColumnIndex == olvStatus.Index)
		{
			_clsEmailService.funcCallDueLink(varclsDueHeader);
		}
		else if (e.ColumnIndex == olvLpcoStat.Index)
		{
			string varFeatExtId = clsFeatureService.consReportExpLpcoControl;
			await _clsDataParam.funcAddCounterAsync(varFeatExtId + "-CLICKS");
			if (clsFunction.Contains(await _clsFeatService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
			{
				await new clsManGeral().funcGetSalesActionAsync(this, varFeatExtId);
			}
			else
			{
				_clsEmailService.funcCallLpcoLinkAsync(varclsDueHeader);
			}
		}
	}

	private async void funcShowDocViewerAsync(bool pShowPDF, bool pShowXML, DueHeader pclsDueHeader = null)
	{
		List<DueHeader> varDueList = new List<DueHeader>();
		if (pclsDueHeader != null)
		{
			varDueList.Add(pclsDueHeader);
		}
		else
		{
			varDueList = funcGetObjList(pFocused: true, pChecked: true);
		}
		await clsMonGeral.funcDocViewerAsync(this, await funcGetDocListAsync(varDueList, pSyncFromDbaFirst: true), pShowPDF, pShowXML);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabExpDueControl));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmCopyDue = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmCopyDFe = new System.Windows.Forms.ToolStripMenuItem();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.pnMarket = new System.Windows.Forms.Panel();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.pnMarketContent = new System.Windows.Forms.Panel();
		this.btSalesContact = new System.Windows.Forms.Button();
		this.btSalesAction = new System.Windows.Forms.Button();
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.lknAction = new System.Windows.Forms.LinkLabel();
		this.label3 = new System.Windows.Forms.Label();
		this.picWarning = new System.Windows.Forms.PictureBox();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvNum = new BrightIdeasSoftware.OLVColumn();
		this.olvStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvLpcoStat = new BrightIdeasSoftware.OLVColumn();
		this.olvHasDossie = new BrightIdeasSoftware.OLVColumn();
		this.olvDataDue = new BrightIdeasSoftware.OLVColumn();
		this.olvDataAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvCanal = new BrightIdeasSoftware.OLVColumn();
		this.olvChave = new BrightIdeasSoftware.OLVColumn();
		this.olvForma = new BrightIdeasSoftware.OLVColumn();
		this.olvValor = new BrightIdeasSoftware.OLVColumn();
		this.olvBloqueio = new BrightIdeasSoftware.OLVColumn();
		this.olvTipoDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvStatusAdm = new BrightIdeasSoftware.OLVColumn();
		this.olvStatusCarga = new BrightIdeasSoftware.OLVColumn();
		this.olvPaisDest = new BrightIdeasSoftware.OLVColumn();
		this.olvPaisDestName = new BrightIdeasSoftware.OLVColumn();
		this.olvImportaName = new BrightIdeasSoftware.OLVColumn();
		this.olvTipoFrete = new BrightIdeasSoftware.OLVColumn();
		this.olvRucNum = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvConEmbNum = new BrightIdeasSoftware.OLVColumn();
		this.olvConEmbTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvConEmbData = new BrightIdeasSoftware.OLVColumn();
		this.olvTipoDocTrans = new BrightIdeasSoftware.OLVColumn();
		this.olvViaTransCode = new BrightIdeasSoftware.OLVColumn();
		this.olvDeclaraId = new BrightIdeasSoftware.OLVColumn();
		this.olvDeclaraName = new BrightIdeasSoftware.OLVColumn();
		this.olvExportaId = new BrightIdeasSoftware.OLVColumn();
		this.olvExportaName = new BrightIdeasSoftware.OLVColumn();
		this.olvLocDespCode = new BrightIdeasSoftware.OLVColumn();
		this.olvLocDespDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvLocEmbarCode = new BrightIdeasSoftware.OLVColumn();
		this.olvLocEmbarDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanDespCode = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanDespDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanDespZona = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanDespDepId = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanDespDepName = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanEmbarCode = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanEmbarDesc = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanEmbarZona = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanEmbarDepId = new BrightIdeasSoftware.OLVColumn();
		this.olvRecAduanEmbarDepName = new BrightIdeasSoftware.OLVColumn();
		this.olvAnoMes = new BrightIdeasSoftware.OLVColumn();
		this.olvDtLastSync = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNotes = new BrightIdeasSoftware.OLVColumn();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDocs = new System.Windows.Forms.ToolStripButton();
		this.tssSepDocs = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSpedCreate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.tspTaskMenu = new System.Windows.Forms.ToolStrip();
		this.tsbSync = new System.Windows.Forms.ToolStripButton();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.contextMenuDocs.SuspendLayout();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.tspTaskMenu.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmCopyDue, this.tsmCopyDFe });
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(188, 48);
		this.tsmCopyDue.Name = "tsmCopyDue";
		this.tsmCopyDue.Size = new System.Drawing.Size(187, 22);
		this.tsmCopyDue.Text = "CTRL+C : Copiar DUE";
		this.tsmCopyDue.Click += new System.EventHandler(tsmCopyDue_Click);
		this.tsmCopyDFe.Name = "tsmCopyDFe";
		this.tsmCopyDFe.Size = new System.Drawing.Size(187, 22);
		this.tsmCopyDFe.Text = "CTRL+N : Copiar NFe";
		this.tsmCopyDFe.Click += new System.EventHandler(tsmCopyDFe_Click);
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_channel_green.png");
		this.ImageListDocs.Images.SetKeyName(1, "image_channel_yellow.png");
		this.ImageListDocs.Images.SetKeyName(2, "image_channel_orange.png");
		this.ImageListDocs.Images.SetKeyName(3, "image_channel_red.png");
		this.ImageListDocs.Images.SetKeyName(4, "image_status_initial.png");
		this.ImageListDocs.Images.SetKeyName(5, "image_status_pendent.png");
		this.ImageListDocs.Images.SetKeyName(6, "image_status_running.png");
		this.ImageListDocs.Images.SetKeyName(7, "image_status_finished.png");
		this.ImageListDocs.Images.SetKeyName(8, "dfe_canceled.png");
		this.ImageListDocs.Images.SetKeyName(9, "image_source_folder.png");
		this.ImageListDocs.Images.SetKeyName(10, "image_locker_16_16.png");
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 128);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(900, 353);
		this.pnMarket.TabIndex = 11;
		this.pnMarket.Visible = false;
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
		this.pnMarketContent.Controls.Add(this.lbTitle03);
		this.pnMarketContent.Controls.Add(this.lknAction);
		this.pnMarketContent.Controls.Add(this.label3);
		this.pnMarketContent.Controls.Add(this.picWarning);
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
		this.btSalesContact.Location = new System.Drawing.Point(80, 220);
		this.btSalesContact.Name = "btSalesContact";
		this.btSalesContact.Size = new System.Drawing.Size(188, 28);
		this.btSalesContact.TabIndex = 220;
		this.btSalesContact.Text = "Solicitar Contato";
		this.btSalesContact.UseVisualStyleBackColor = false;
		this.btSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		this.btSalesAction.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btSalesAction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesAction.ForeColor = System.Drawing.Color.White;
		this.btSalesAction.Location = new System.Drawing.Point(299, 220);
		this.btSalesAction.Name = "btSalesAction";
		this.btSalesAction.Size = new System.Drawing.Size(188, 28);
		this.btSalesAction.TabIndex = 219;
		this.btSalesAction.Text = "Conheça os planos";
		this.btSalesAction.UseVisualStyleBackColor = false;
		this.btSalesAction.Visible = false;
		this.btSalesAction.Click += new System.EventHandler(btSalesAction_Click);
		this.lbTitle03.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle03.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle03.Location = new System.Drawing.Point(18, 184);
		this.lbTitle03.Name = "lbTitle03";
		this.lbTitle03.Size = new System.Drawing.Size(88, 17);
		this.lbTitle03.TabIndex = 211;
		this.lbTitle03.Text = "Saiba mais:";
		this.lknAction.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lknAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknAction.Location = new System.Drawing.Point(112, 184);
		this.lknAction.Name = "lknAction";
		this.lknAction.Size = new System.Drawing.Size(436, 16);
		this.lknAction.TabIndex = 210;
		this.lknAction.TabStop = true;
		this.lknAction.Text = "Controle de Exportação : DUEs";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.BackColor = System.Drawing.Color.Gainsboro;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Gainsboro;
		this.label3.Location = new System.Drawing.Point(3, 38);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(560, 1);
		this.label3.TabIndex = 208;
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.picWarning.Image = Monitor.Resources.image_help;
		this.picWarning.Location = new System.Drawing.Point(20, 9);
		this.picWarning.Name = "picWarning";
		this.picWarning.Size = new System.Drawing.Size(20, 20);
		this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picWarning.TabIndex = 206;
		this.picWarning.TabStop = false;
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 71);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 98);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = resources.GetString("lbText01.Text");
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Relatório : Controle de Exportação : DUEs";
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
		this.btClose.Location = new System.Drawing.Point(869, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 3;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvNum);
		this.lsvData.AllColumns.Add(this.olvStatus);
		this.lsvData.AllColumns.Add(this.olvLpcoStat);
		this.lsvData.AllColumns.Add(this.olvHasDossie);
		this.lsvData.AllColumns.Add(this.olvDataDue);
		this.lsvData.AllColumns.Add(this.olvDataAverb);
		this.lsvData.AllColumns.Add(this.olvCanal);
		this.lsvData.AllColumns.Add(this.olvChave);
		this.lsvData.AllColumns.Add(this.olvForma);
		this.lsvData.AllColumns.Add(this.olvValor);
		this.lsvData.AllColumns.Add(this.olvBloqueio);
		this.lsvData.AllColumns.Add(this.olvTipoDoc);
		this.lsvData.AllColumns.Add(this.olvStatusAdm);
		this.lsvData.AllColumns.Add(this.olvStatusCarga);
		this.lsvData.AllColumns.Add(this.olvPaisDest);
		this.lsvData.AllColumns.Add(this.olvPaisDestName);
		this.lsvData.AllColumns.Add(this.olvImportaName);
		this.lsvData.AllColumns.Add(this.olvTipoFrete);
		this.lsvData.AllColumns.Add(this.olvRucNum);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvConEmbNum);
		this.lsvData.AllColumns.Add(this.olvConEmbTipo);
		this.lsvData.AllColumns.Add(this.olvConEmbData);
		this.lsvData.AllColumns.Add(this.olvTipoDocTrans);
		this.lsvData.AllColumns.Add(this.olvViaTransCode);
		this.lsvData.AllColumns.Add(this.olvDeclaraId);
		this.lsvData.AllColumns.Add(this.olvDeclaraName);
		this.lsvData.AllColumns.Add(this.olvExportaId);
		this.lsvData.AllColumns.Add(this.olvExportaName);
		this.lsvData.AllColumns.Add(this.olvLocDespCode);
		this.lsvData.AllColumns.Add(this.olvLocDespDesc);
		this.lsvData.AllColumns.Add(this.olvLocEmbarCode);
		this.lsvData.AllColumns.Add(this.olvLocEmbarDesc);
		this.lsvData.AllColumns.Add(this.olvRecAduanDespCode);
		this.lsvData.AllColumns.Add(this.olvRecAduanDespDesc);
		this.lsvData.AllColumns.Add(this.olvRecAduanDespZona);
		this.lsvData.AllColumns.Add(this.olvRecAduanDespDepId);
		this.lsvData.AllColumns.Add(this.olvRecAduanDespDepName);
		this.lsvData.AllColumns.Add(this.olvRecAduanEmbarCode);
		this.lsvData.AllColumns.Add(this.olvRecAduanEmbarDesc);
		this.lsvData.AllColumns.Add(this.olvRecAduanEmbarZona);
		this.lsvData.AllColumns.Add(this.olvRecAduanEmbarDepId);
		this.lsvData.AllColumns.Add(this.olvRecAduanEmbarDepName);
		this.lsvData.AllColumns.Add(this.olvAnoMes);
		this.lsvData.AllColumns.Add(this.olvDtLastSync);
		this.lsvData.AllColumns.Add(this.olvDocNotes);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[48]
		{
			this.olvSelect, this.olvFilial, this.olvNum, this.olvStatus, this.olvLpcoStat, this.olvHasDossie, this.olvDataDue, this.olvDataAverb, this.olvCanal, this.olvChave,
			this.olvForma, this.olvValor, this.olvBloqueio, this.olvTipoDoc, this.olvStatusAdm, this.olvStatusCarga, this.olvPaisDest, this.olvPaisDestName, this.olvImportaName, this.olvTipoFrete,
			this.olvRucNum, this.olvNatOper, this.olvConEmbNum, this.olvConEmbTipo, this.olvConEmbData, this.olvTipoDocTrans, this.olvViaTransCode, this.olvDeclaraId, this.olvDeclaraName, this.olvExportaId,
			this.olvExportaName, this.olvLocDespCode, this.olvLocDespDesc, this.olvLocEmbarCode, this.olvLocEmbarDesc, this.olvRecAduanDespCode, this.olvRecAduanDespDesc, this.olvRecAduanDespZona, this.olvRecAduanDespDepId, this.olvRecAduanDespDepName,
			this.olvRecAduanEmbarCode, this.olvRecAduanEmbarDesc, this.olvRecAduanEmbarZona, this.olvRecAduanEmbarDepId, this.olvRecAduanEmbarDepName, this.olvAnoMes, this.olvDtLastSync, this.olvDocNotes
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
		this.lsvData.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(lsvData_CellToolTipShowing);
		this.lsvData.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(lsvData_FormatRow);
		this.lsvData.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(lsvData_HeaderCheckBoxChanging);
		this.lsvData.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(lsvData_HyperlinkClicked);
		this.lsvData.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(lsvData_ItemsChanged);
		this.lsvData.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lsvData_ColumnWidthChanged);
		this.lsvData.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvData_ItemChecked);
		this.lsvData.KeyUp += new System.Windows.Forms.KeyEventHandler(lsvData_KeyUp);
		this.olvSelect.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelect.Groupable = false;
		this.olvSelect.HeaderCheckBox = true;
		this.olvSelect.HeaderCheckBoxUpdatesRowCheckBoxes = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvFilial.Text = "Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 115;
		this.olvNum.AspectName = "Num";
		this.olvNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.Hyperlink = true;
		this.olvNum.Text = "DUE";
		this.olvNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvNum.ToolTipText = "Número da DUE";
		this.olvNum.Width = 110;
		this.olvStatus.AspectName = "Status";
		this.olvStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvStatus.Hyperlink = true;
		this.olvStatus.Text = "Status";
		this.olvStatus.ToolTipText = "Status da Exportação";
		this.olvStatus.Width = 74;
		this.olvLpcoStat.AspectName = "LpcoStat";
		this.olvLpcoStat.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvLpcoStat.Hyperlink = true;
		this.olvLpcoStat.Text = "LPCO";
		this.olvLpcoStat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvLpcoStat.ToolTipText = "Status da LPCO";
		this.olvHasDossie.AspectName = "";
		this.olvHasDossie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasDossie.Text = "Anexos";
		this.olvHasDossie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvHasDossie.Width = 55;
		this.olvDataDue.AspectName = "DataDue";
		this.olvDataDue.AspectToStringFormat = "";
		this.olvDataDue.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataDue.Text = "DtDue";
		this.olvDataDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataDue.ToolTipText = "Data da Declaração de Exportação";
		this.olvDataDue.Width = 78;
		this.olvDataAverb.AspectName = "DataAverb";
		this.olvDataAverb.AspectToStringFormat = "";
		this.olvDataAverb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataAverb.Text = "DtAverb";
		this.olvDataAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataAverb.ToolTipText = "Data da Averbação";
		this.olvDataAverb.Width = 78;
		this.olvCanal.AspectName = "Canal";
		this.olvCanal.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvCanal.Text = "Canal";
		this.olvCanal.ToolTipText = "Canal de Exportação";
		this.olvCanal.Width = 74;
		this.olvChave.AspectName = "Chave";
		this.olvChave.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvChave.Text = "Chave";
		this.olvChave.ToolTipText = "Chave da DUE";
		this.olvChave.Width = 74;
		this.olvForma.AspectName = "Forma";
		this.olvForma.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvForma.Text = "Forma";
		this.olvForma.ToolTipText = "Forma de Exportação";
		this.olvForma.Width = 100;
		this.olvValor.AspectName = "Valor";
		this.olvValor.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvValor.Text = "Valor";
		this.olvValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvValor.ToolTipText = "Valor da Exportação";
		this.olvValor.Width = 100;
		this.olvBloqueio.AspectName = "Bloqueio";
		this.olvBloqueio.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBloqueio.Text = "Bloqueio";
		this.olvBloqueio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBloqueio.ToolTipText = "Bloqueio da Exportação";
		this.olvBloqueio.Width = 74;
		this.olvTipoDoc.AspectName = "TipoDoc";
		this.olvTipoDoc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvTipoDoc.Text = "TipoDoc";
		this.olvTipoDoc.ToolTipText = "Tipo do Documento";
		this.olvTipoDoc.Width = 90;
		this.olvStatusAdm.AspectName = "StatusAdm";
		this.olvStatusAdm.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvStatusAdm.Text = "StatusAdm";
		this.olvStatusAdm.ToolTipText = "Situação do controle administrativo";
		this.olvStatusAdm.Width = 100;
		this.olvStatusCarga.AspectName = "StatusCarga";
		this.olvStatusCarga.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvStatusCarga.Text = "StatusCarga";
		this.olvStatusCarga.ToolTipText = "Situação da Carga";
		this.olvStatusCarga.Width = 100;
		this.olvPaisDest.AspectName = "PaisDest";
		this.olvPaisDest.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPaisDest.Text = "Pais Destino : Código";
		this.olvPaisDest.ToolTipText = "Código do País de Destino";
		this.olvPaisDest.Width = 100;
		this.olvPaisDestName.AspectName = "PaisDestName";
		this.olvPaisDestName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvPaisDestName.Text = "Pais Destino : Nome";
		this.olvPaisDestName.ToolTipText = "Nome do País de Destino";
		this.olvPaisDestName.Width = 120;
		this.olvImportaName.AspectName = "ImportaName";
		this.olvImportaName.Text = "ImportaName";
		this.olvImportaName.ToolTipText = "Nome do Importador";
		this.olvImportaName.Width = 150;
		this.olvTipoFrete.AspectName = "TipoFrete";
		this.olvTipoFrete.Text = "TipoFrete";
		this.olvTipoFrete.ToolTipText = "Tipo de Frete : Incoterm";
		this.olvRucNum.AspectName = "RucNum";
		this.olvRucNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRucNum.Text = "RUC";
		this.olvRucNum.ToolTipText = "Número da RUC";
		this.olvRucNum.Width = 100;
		this.olvNatOper.AspectName = "NatOper";
		this.olvNatOper.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvNatOper.Text = "NatOper";
		this.olvNatOper.ToolTipText = "Natureza da Operação";
		this.olvNatOper.Width = 120;
		this.olvConEmbNum.AspectName = "ConEmbNum";
		this.olvConEmbNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvConEmbNum.Text = "ConhecEmbarque";
		this.olvConEmbNum.ToolTipText = "Nº do conhecimento de embarque";
		this.olvConEmbNum.Width = 74;
		this.olvConEmbTipo.AspectName = "ConEmbTipo";
		this.olvConEmbTipo.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvConEmbTipo.Text = "ConhecTipo";
		this.olvConEmbTipo.ToolTipText = "Tipo de Conhecimento de Embarque";
		this.olvConEmbTipo.Width = 74;
		this.olvConEmbData.AspectName = "ConEmbData";
		this.olvConEmbData.AspectToStringFormat = "";
		this.olvConEmbData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvConEmbData.Text = "ConhecData";
		this.olvConEmbData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvConEmbData.ToolTipText = "Data do Conhecimento de Embarque";
		this.olvConEmbData.Width = 74;
		this.olvTipoDocTrans.AspectName = "TipoDocTrans";
		this.olvTipoDocTrans.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTipoDocTrans.Text = "Tipo Doc Transporte";
		this.olvTipoDocTrans.ToolTipText = "Tipo de Documento de Transporte";
		this.olvTipoDocTrans.Width = 70;
		this.olvViaTransCode.AspectName = "ViaTransCode";
		this.olvViaTransCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvViaTransCode.Text = "Via Transporte";
		this.olvViaTransCode.ToolTipText = "Via de Transporte";
		this.olvViaTransCode.Width = 70;
		this.olvDeclaraId.AspectName = "DeclaraId";
		this.olvDeclaraId.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDeclaraId.Text = "Declarante : Id";
		this.olvDeclaraId.ToolTipText = "CNPJ/CPF do Declarante da Exportação";
		this.olvDeclaraId.Width = 115;
		this.olvDeclaraName.AspectName = "DeclaraName";
		this.olvDeclaraName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvDeclaraName.Text = "Declarante : Nome";
		this.olvDeclaraName.ToolTipText = "Nome do Declarante da Exportação";
		this.olvDeclaraName.Width = 150;
		this.olvExportaId.AspectName = "ExportaId";
		this.olvExportaId.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvExportaId.Text = "Exportador : ID";
		this.olvExportaId.ToolTipText = "CNPJ/CPF do Exportador";
		this.olvExportaId.Width = 115;
		this.olvExportaName.AspectName = "ExportaName";
		this.olvExportaName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvExportaName.Text = "Exportador : Nome";
		this.olvExportaName.ToolTipText = "Nome do Exportador";
		this.olvExportaName.Width = 150;
		this.olvLocDespCode.AspectName = "LocDespCode";
		this.olvLocDespCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvLocDespCode.Text = "LocDespCode";
		this.olvLocDespCode.ToolTipText = "Código do Local de Despacho";
		this.olvLocDespCode.Width = 74;
		this.olvLocDespDesc.AspectName = "LocDespDesc";
		this.olvLocDespDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvLocDespDesc.Text = "LocDespDesc";
		this.olvLocDespDesc.ToolTipText = "Descrição do Local de Despacho";
		this.olvLocDespDesc.Width = 150;
		this.olvLocEmbarCode.AspectName = "LocEmbarCode";
		this.olvLocEmbarCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvLocEmbarCode.Text = "LocEmbarCode";
		this.olvLocEmbarCode.ToolTipText = "Código do Local de Embarque";
		this.olvLocEmbarCode.Width = 80;
		this.olvLocEmbarDesc.AspectName = "LocEmbarDesc";
		this.olvLocEmbarDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvLocEmbarDesc.Text = "LocEmbarDesc";
		this.olvLocEmbarDesc.ToolTipText = "Descrição do Local de Embarque";
		this.olvLocEmbarDesc.Width = 150;
		this.olvRecAduanDespCode.AspectName = "RecAduanDespCode";
		this.olvRecAduanDespCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecAduanDespCode.Text = "RecAduanDespCode";
		this.olvRecAduanDespCode.ToolTipText = "Código do Recinto Aduaneiro de Despacho";
		this.olvRecAduanDespCode.Width = 80;
		this.olvRecAduanDespDesc.AspectName = "RecAduanDespDesc";
		this.olvRecAduanDespDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRecAduanDespDesc.Text = "RecAduanDespDesc";
		this.olvRecAduanDespDesc.ToolTipText = "Descrição do Recinto Aduaneiro de Despacho";
		this.olvRecAduanDespDesc.Width = 150;
		this.olvRecAduanDespZona.AspectName = "RecAduanDespZona";
		this.olvRecAduanDespZona.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRecAduanDespZona.Text = "RecAduanDespZona";
		this.olvRecAduanDespZona.ToolTipText = "Zona do Recinto Aduaneiro de Despacho";
		this.olvRecAduanDespZona.Width = 100;
		this.olvRecAduanDespDepId.AspectName = "RecAduanDespDepId";
		this.olvRecAduanDespDepId.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecAduanDespDepId.Text = "RecAduanDespDepId";
		this.olvRecAduanDespDepId.ToolTipText = "Código do Depositário do Recinto Aduaneiro de Despacho";
		this.olvRecAduanDespDepId.Width = 115;
		this.olvRecAduanDespDepName.AspectName = "RecAduanDespDepName";
		this.olvRecAduanDespDepName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRecAduanDespDepName.Text = "RecAduanDespDepName";
		this.olvRecAduanDespDepName.ToolTipText = "Descrição do Depositário do Recinto Aduaneiro de Despacho";
		this.olvRecAduanDespDepName.Width = 150;
		this.olvRecAduanEmbarCode.AspectName = "RecAduanEmbarCode";
		this.olvRecAduanEmbarCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecAduanEmbarCode.Text = "RecAduanEmbarCode";
		this.olvRecAduanEmbarCode.ToolTipText = "Código do Recinto Aduaneiro de Embarque";
		this.olvRecAduanEmbarCode.Width = 80;
		this.olvRecAduanEmbarDesc.AspectName = "RecAduanEmbarDesc";
		this.olvRecAduanEmbarDesc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRecAduanEmbarDesc.Text = "RecAduanEmbarDesc";
		this.olvRecAduanEmbarDesc.ToolTipText = "Descrição do Recinto Aduaneiro de Embarque";
		this.olvRecAduanEmbarDesc.Width = 150;
		this.olvRecAduanEmbarZona.AspectName = "RecAduanEmbarZona";
		this.olvRecAduanEmbarZona.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRecAduanEmbarZona.Text = "RecAduanEmbarZona";
		this.olvRecAduanEmbarZona.ToolTipText = "Zona do Recinto Aduaneiro de Embarque";
		this.olvRecAduanEmbarZona.Width = 100;
		this.olvRecAduanEmbarDepId.AspectName = "RecAduanEmbarDepId";
		this.olvRecAduanEmbarDepId.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvRecAduanEmbarDepId.Text = "RecAduanEmbarDepId";
		this.olvRecAduanEmbarDepId.ToolTipText = "Código do Depositário do Recinto Aduaneiro de Embarque";
		this.olvRecAduanEmbarDepId.Width = 115;
		this.olvRecAduanEmbarDepName.AspectName = "RecAduanEmbarDepName";
		this.olvRecAduanEmbarDepName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvRecAduanEmbarDepName.Text = "RecAduanEmbarDepName";
		this.olvRecAduanEmbarDepName.ToolTipText = "Descrição do Depositário do Recinto Aduaneiro de Embarque";
		this.olvRecAduanEmbarDepName.Width = 150;
		this.olvAnoMes.AspectName = "DataDue";
		this.olvAnoMes.Text = "Ano-Mês";
		this.olvAnoMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAnoMes.ToolTipText = "Ano-Mes de Registro da DUE";
		this.olvAnoMes.Width = 77;
		this.olvDtLastSync.AspectName = "DtLastSync";
		this.olvDtLastSync.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtLastSync.Text = "DtSyncSiscomex";
		this.olvDtLastSync.ToolTipText = "Data da Ultima Sincronização com o Siscomex";
		this.olvDtLastSync.Width = 150;
		this.olvDocNotes.AspectName = "DocNotes";
		this.olvDocNotes.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvDocNotes.Text = "Info Complementar";
		this.olvDocNotes.ToolTipText = "Informações Complementares";
		this.olvDocNotes.Width = 200;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.Location = new System.Drawing.Point(1, 25);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Padding = new System.Windows.Forms.Padding(2);
		this.miniToolStrip.Size = new System.Drawing.Size(741, 27);
		this.miniToolStrip.TabIndex = 42;
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
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
		this.tspTaskMenu.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspTaskMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator3, this.tsbSync, this.toolStripSeparator2, this.tsbDocs, this.tssSepDocs, this.tsbSpedCreate, this.toolStripSeparator7 });
		this.tspTaskMenu.Location = new System.Drawing.Point(0, 0);
		this.tspTaskMenu.Name = "tspTaskMenu";
		this.tspTaskMenu.Padding = new System.Windows.Forms.Padding(2);
		this.tspTaskMenu.Size = new System.Drawing.Size(900, 27);
		this.tspTaskMenu.TabIndex = 42;
		this.tspTaskMenu.Text = "toolStrip1";
		this.tsbSync.Image = Monitor.Resources.image_cloud;
		this.tsbSync.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSync.Name = "tsbSync";
		this.tsbSync.Size = new System.Drawing.Size(74, 20);
		this.tsbSync.Text = " Siscomex";
		this.tsbSync.ToolTipText = "Sincronizar dados com Siscomex";
		this.tsbSync.Click += new System.EventHandler(tsbSync_Click);
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 443);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(898, 38);
		this.plnMessage.TabIndex = 107;
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
		base.Name = "frmTabExpDueControl";
		this.Text = "Controle de Exportação";
		base.Load += new System.EventHandler(frmTabExpDueControl_Load);
		this.contextMenuDocs.ResumeLayout(false);
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.tspTaskMenu.ResumeLayout(false);
		this.tspTaskMenu.PerformLayout();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
