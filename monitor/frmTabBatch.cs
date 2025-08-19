using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using srv.fiscal.io.DFeData;
using util.fiscal.io;

namespace Monitor;

public class frmTabBatch : Form
{
	private clsDataBatchHead varclsDataBatch = new clsDataBatchHead();

	private clsDataBatchItem varclsDataItem = new clsDataBatchItem();

	private clsDataTaskAction varclsDataTask = new clsDataTaskAction();

	private List<BatchHead> _clsBufferList = new List<BatchHead>();

	private clsDataParameter varclsDataParam = new clsDataParameter();

	private clsMetricService varclsMetricService = new clsMetricService();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsDataFilter _clsDataFilter = new clsDataFilter();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private bool _ColumnsLoaded;

	private bool _IsLoading;

	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private IContainer components;

	private Panel pnContent;

	private SplitContainer splitBatch;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private ToolStrip tspDocs;

	private ToolStripButton tsbError;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton tsbWarning;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton tsbSucess;

	private ToolStripSeparator toolStripSeparator5;

	private ContextMenuStrip contextMenuDocs;

	private OLVColumn olvAudRole;

	private FastObjectListView lsvDocs;

	private OLVColumn olvBtiDocKey;

	private OLVColumn olvBtiMessage;

	private ToolStrip tspBatchs;

	private ToolStripSeparator tsbSep01;

	private ImageList ImageListDocs;

	private OLVColumn olvBtiSelect;

	private FastObjectListView lsvBatchs;

	private OLVColumn olvBthSelect;

	private OLVColumn olvBthDescrt;

	private OLVColumn olvBthPerc;

	private OLVColumn olvBthItems;

	private OLVColumn olvBthTotEr;

	private OLVColumn olvBthPendt;

	private OLVColumn olvBtiBatch;

	private ToolStripButton tsbDelete;

	private OLVColumn olvBthFilial;

	private OLVColumn olvBthId;

	private ToolStripButton tsbRefresh;

	private ToolStripSeparator tsbSep04;

	private ToolStripButton tsbTimer;

	private System.Windows.Forms.Timer timerRefresh;

	private ToolStripSeparator tsbSep05;

	private ToolStripButton tsbRework01;

	private ToolStripSeparator tsbSep06;

	private ToolStripSeparator tsbSep07;

	private ToolStripButton tsbPendent;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton tsbPlanned;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripComboBox tsbSystemAct;

	private ToolStripSeparator toolStripSeparator7;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabBatch(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
	}

	public new void Dispose()
	{
		timerRefresh.Dispose();
		timerRefresh = null;
		Dispose(disposing: true);
	}

	private async void frmTabBatch_Load(object sender, EventArgs e)
	{
		_IsLoading = true;
		funcDefineListViewLayout();
		funcDefineListViewFeatures();
		await funcLoadTagsAsync();
		funcTimerScreen();
		List<clsObjectType> varclsObjList = new List<clsObjectType>();
		varclsObjList.Add(new clsObjectType
		{
			Name = "Lotes manuais",
			Value = "BatchMan"
		});
		varclsObjList.Add(new clsObjectType
		{
			Name = "Lotes automáticos",
			Value = "BatchAut"
		});
		varclsObjList.Add(new clsObjectType
		{
			Name = "Todos os Lotes",
			Value = "BatchAll"
		});
		tsbSystemAct.ComboBox.ValueMember = "Value";
		tsbSystemAct.ComboBox.DisplayMember = "Name";
		tsbSystemAct.ComboBox.DataSource = varclsObjList;
		string varValue = await varclsDataParam.funcGetAsync("TabBatch-BatchType");
		if (clsFunction.IsEmpty(varValue))
		{
			varValue = "BatchAll";
		}
		tsbSystemAct.ComboBox.SelectedValue = varValue;
		_IsLoading = false;
	}

	private void funcDefineListViewLayout()
	{
		_ColumnsLoaded = false;
		funcSetListViewConfig(lsvBatchs);
		funcSetListViewConfig(lsvDocs);
		lsvDocs.RowHeight = 22;
		lsvBatchs.RowHeight = 22;
		_ColumnsLoaded = true;
	}

	private void funcSetListViewConfig(ObjectListView pListView)
	{
		clsScreenGeral.funcSetColumnsObjectListView(this, pListView);
		pListView.ColumnReordered += ObjListView_ColumnReordered;
		pListView.ColumnWidthChanged += ObjListView_ColumnWidthChanged;
		foreach (OLVColumn allColumn in pListView.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
	}

	private void ObjListView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		if (!_ColumnsLoaded)
		{
			return;
		}
		ObjectListView varListView = (ObjectListView)sender;
		if (varListView != null)
		{
			string varColumName = clsScreenGeral.funcGetColumnName(varListView.AllColumns[e.Header.Index]);
			if (!string.IsNullOrEmpty(varColumName))
			{
				clsFunction.funcSetRegisterValue(base.Name + "-" + varListView.Name + "-" + varColumName + "-Order", e.NewDisplayIndex.ToString(), pGlobal: false);
			}
		}
	}

	private void ObjListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		if (!_ColumnsLoaded)
		{
			return;
		}
		ObjectListView varListView = (ObjectListView)sender;
		if (varListView != null)
		{
			OLVColumn varColumnObj = varListView.AllColumns[e.ColumnIndex];
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
		if (varOlvColumn == null)
		{
			return;
		}
		string varColumName = clsScreenGeral.funcGetColumnName(varOlvColumn);
		ListView varListView = varOlvColumn.ListView;
		if (varListView == null)
		{
			varListView = lsvBatchs;
		}
		if (varListView != null && !string.IsNullOrEmpty(varColumName))
		{
			string varObjectKey = base.Name + "-" + varListView.Name + "-" + varColumName + "-Hidden";
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

	private void funcDefineListViewFeatures()
	{
		new clsDFeMessage();
		olvBthSelect.ImageGetter = delegate(object x)
		{
			BatchHead batchHead = (BatchHead)x;
			return (batchHead == null) ? null : ((object)funcGetStatIcon(batchHead.BthStatus));
		};
		olvBthPendt.AspectGetter = delegate(object x)
		{
			BatchHead batchHead = (BatchHead)x;
			if (batchHead == null)
			{
				return (object)null;
			}
			int num = clsFunction.funcConvStrToInt(batchHead.BthPendt);
			int num2 = clsFunction.funcConvStrToInt(batchHead.BthTotEr);
			int num3 = num - num2;
			if (num3 < 0)
			{
				num3 = 0;
			}
			return num3;
		};
		olvBtiSelect.ImageGetter = delegate(object x)
		{
			BatchItem batchItem = (BatchItem)x;
			return (batchItem == null) ? null : ((object)funcGetStatIcon(batchItem.BtiStatus));
		};
	}

	private int funcGetStatIcon(string pStatus)
	{
		int varIcone = 0;
		string varStatus = clsFunction.funcGetValue(pStatus);
		if (varStatus.Equals("PLANNED"))
		{
			varIcone = 0;
		}
		else if (varStatus.Equals("PLANERR"))
		{
			varIcone = 2;
		}
		else if (varStatus.Equals("RUNNING"))
		{
			varIcone = 1;
		}
		else if (varStatus.Equals("RUNNERR"))
		{
			varIcone = 2;
		}
		else if (varStatus.Equals("FINISHED"))
		{
			varIcone = 4;
		}
		else if (varStatus.Equals("FINRESTR"))
		{
			varIcone = 5;
		}
		else if (varStatus.Equals("FINERROR"))
		{
			varIcone = 2;
		}
		return varIcone;
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturn = new clsReturn();
		_clsDataFilter = pclsDataFilter;
		List<BatchHead> varclsBatchList = await funcGetLoadBatchAsync(pclsDataFilter);
		_clsBufferList.Clear();
		lsvBatchs.BeginUpdate();
		lsvBatchs.SuspendLayout();
		lsvBatchs.UncheckHeaderCheckBox(olvBthSelect);
		lsvBatchs.SetObjects(varclsBatchList);
		olvBtiSelect.HeaderCheckState = CheckState.Unchecked;
		lsvDocs.SetObjects(null);
		lsvBatchs.SelectedItem = null;
		lsvBatchs.FocusedItem = null;
		lsvBatchs.EndUpdate();
		lsvBatchs.ResumeLayout();
		funcTimerScreen();
		return varclsReturn;
	}

	private async Task<List<BatchHead>> funcGetLoadBatchAsync(clsDataFilter pclsDataFilter)
	{
		List<BatchHead> varclsBatchList = new List<BatchHead>();
		clsObjectType varclsObjType = (clsObjectType)tsbSystemAct.SelectedItem;
		if (varclsObjType == null)
		{
			varclsObjType = new clsObjectType();
		}
		string varBatchType = clsFunction.funcGetValue(varclsObjType.Value);
		string varSqlQuery = clsSqlFilter.funcGetBatchHeadSqlStr(pclsDataFilter, varBatchType);
		if (string.IsNullOrEmpty(varSqlQuery))
		{
			return varclsBatchList;
		}
		long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
		return await varclsDataBatch.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
	}

	private void funcTimerScreen()
	{
		if (lsvBatchs.Objects.Cast<BatchHead>().ToList().Any((BatchHead r) => r.IsOpen() || r.IsRunning()))
		{
			timerRefresh.Enabled = true;
			timerRefresh.Start();
			tsbTimer.Checked = true;
			tsbTimer.Visible = true;
			tsbSep06.Visible = true;
		}
		else
		{
			timerRefresh.Enabled = false;
			timerRefresh.Stop();
			tsbTimer.Checked = false;
			tsbTimer.Visible = false;
			tsbSep06.Visible = false;
		}
	}

	public async Task<bool> funcLoadTagsAsync()
	{
		return await clsMonGeral.funcLoadTagsAsync(_HasColors, _TagHsList, contextMenuDocs, tsmCopyDocKey_Click, null, null, null, null, null);
	}

	private async void timerRefresh_Tick(object sender, EventArgs e)
	{
		if (!tsbTimer.Checked)
		{
			return;
		}
		clsReturn varclsReturnFunc = new clsReturn();
		int varSeconds = 0;
		try
		{
			string varclsTag = clsFunction.funcGetValue(timerRefresh.Tag, "10");
			varSeconds = Convert.ToInt32(varclsTag);
			varSeconds--;
			if (varSeconds < 0)
			{
				varSeconds = 10;
			}
			timerRefresh.Tag = varSeconds.ToString();
			tsbTimer.Text = $"[ {varSeconds} s ]";
			if (varSeconds > 0)
			{
				return;
			}
			tsbTimer.Image = Resources.gif_loading;
			tsbTimer.Text = " Atualizando ...";
			Cursor.Current = Cursors.WaitCursor;
			timerRefresh.Enabled = false;
			timerRefresh.Stop();
			List<BatchHead> varclsBatchList = lsvBatchs.Objects.Cast<BatchHead>().ToList();
			clsDFeKeyService varclsService = new clsDFeKeyService();
			if (varclsBatchList.Any((BatchHead r) => r.IsOpen()))
			{
				await funcLoadDataAsync(_clsDataFilter);
			}
			clsBatchService varclsBatchService = new clsBatchService();
			for (int varIndex = 0; varIndex < varclsBatchList.Count; varIndex++)
			{
				if (varclsBatchList[varIndex].IsOpen())
				{
					clsReturn varclsItem = await varclsService.funcGetFromQueueAsync(varclsBatchList[varIndex].BtdQueSrv);
					if (varclsItem.HasError)
					{
						varclsReturnFunc.AddRange(varclsItem);
					}
					BatchHead varclsBatchHead = await varclsDataBatch.funcGetItemByKeyAsync(varclsBatchList[varIndex].BthId);
					List<BatchHead> list = varclsBatchList;
					int index = varIndex;
					list[index] = await varclsBatchService.funcDefinePercAsync(varclsBatchHead);
				}
			}
			varSeconds = 10;
			timerRefresh.Tag = "10";
			if (!varclsBatchList.Any((BatchHead r) => r.IsFinished()))
			{
				await varclsMetricService.funcTrySetAhaMomentAsync("DOC_RECOVERY");
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			tsbTimer.Image = Resources.image_countdown;
			tsbTimer.Text = $"[ {varSeconds} s ]";
			Cursor.Current = Cursors.Default;
			timerRefresh.Enabled = true;
			timerRefresh.Start();
		}
		if (varclsReturnFunc.HasError && clsFunction.IsAdmin)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
		}
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		return true;
	}

	private void tsbError_Click(object sender, EventArgs e)
	{
		tsbError.Checked = !tsbError.Checked;
		ToolStripButton toolStripButton = tsbPlanned;
		ToolStripButton toolStripButton2 = tsbPendent;
		ToolStripButton toolStripButton3 = tsbSucess;
		bool flag = (tsbWarning.Checked = false);
		bool flag3 = (toolStripButton3.Checked = flag);
		bool flag5 = (toolStripButton2.Checked = flag3);
		toolStripButton.Checked = flag5;
		funcRebuildFilters();
	}

	private void tsbWarning_Click(object sender, EventArgs e)
	{
		tsbWarning.Checked = !tsbWarning.Checked;
		ToolStripButton toolStripButton = tsbPlanned;
		ToolStripButton toolStripButton2 = tsbPendent;
		ToolStripButton toolStripButton3 = tsbSucess;
		bool flag = (tsbError.Checked = false);
		bool flag3 = (toolStripButton3.Checked = flag);
		bool flag5 = (toolStripButton2.Checked = flag3);
		toolStripButton.Checked = flag5;
		funcRebuildFilters();
	}

	private void tsbSucess_Click(object sender, EventArgs e)
	{
		tsbSucess.Checked = !tsbSucess.Checked;
		ToolStripButton toolStripButton = tsbPlanned;
		ToolStripButton toolStripButton2 = tsbPendent;
		ToolStripButton toolStripButton3 = tsbWarning;
		bool flag = (tsbError.Checked = false);
		bool flag3 = (toolStripButton3.Checked = flag);
		bool flag5 = (toolStripButton2.Checked = flag3);
		toolStripButton.Checked = flag5;
		funcRebuildFilters();
	}

	private void tsbPendent_Click(object sender, EventArgs e)
	{
		tsbPendent.Checked = !tsbPendent.Checked;
		ToolStripButton toolStripButton = tsbPlanned;
		ToolStripButton toolStripButton2 = tsbSucess;
		ToolStripButton toolStripButton3 = tsbWarning;
		bool flag = (tsbError.Checked = false);
		bool flag3 = (toolStripButton3.Checked = flag);
		bool flag5 = (toolStripButton2.Checked = flag3);
		toolStripButton.Checked = flag5;
		funcRebuildFilters();
	}

	private void tsbPlanned_Click(object sender, EventArgs e)
	{
		tsbPlanned.Checked = !tsbPlanned.Checked;
		ToolStripButton toolStripButton = tsbPendent;
		ToolStripButton toolStripButton2 = tsbSucess;
		ToolStripButton toolStripButton3 = tsbWarning;
		bool flag = (tsbError.Checked = false);
		bool flag3 = (toolStripButton3.Checked = flag);
		bool flag5 = (toolStripButton2.Checked = flag3);
		toolStripButton.Checked = flag5;
		funcRebuildFilters();
	}

	private void funcRebuildFilters()
	{
		List<IModelFilter> filters = new List<IModelFilter>();
		if (tsbPlanned.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((BatchItem)model).BtiStatus.StartsWith("PLAN")));
		}
		else if (tsbPendent.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((BatchItem)model).BtiStatus.StartsWith("RUN")));
		}
		else if (tsbSucess.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((BatchItem)model).BtiStatus.Equals("FINISHED")));
		}
		else if (tsbWarning.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((BatchItem)model).BtiStatus.Equals("FINRESTR")));
		}
		else if (tsbError.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((BatchItem)model).BtiStatus.Contains("ERR")));
		}
		lsvDocs.AdditionalFilter = ((filters.Count == 0) ? null : new CompositeAllFilter(filters));
	}

	private void lsvDataList_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false);
	}

	private async void funcShowDocViewerAsync(bool pShowPDF, bool pShowXML)
	{
		List<BatchItem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
		await clsMonGeral.funcDocViewerAsync(this, await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true, pCheckHasXml: true), pShowPDF, pShowXML);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<BatchItem> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst, pCheckHasXml: false);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<BatchItem> pObjList, bool pSyncFromDbaFirst, bool pCheckHasXml)
	{
		List<BatchHead> varBatchList = lsvBatchs.Objects.Cast<BatchHead>().ToList();
		List<Document> varScrDocList = new List<Document>();
		BatchHead varclsBatch = new BatchHead();
		foreach (BatchItem varObject in pObjList)
		{
			if (!clsFunction.IsEqual(varclsBatch.BthId, varObject.BtiBatch))
			{
				varclsBatch = varBatchList.FirstOrDefault((BatchHead r) => clsFunction.IsEqual(r.BthId, varObject.BtiBatch));
			}
			Document varclsDoc = new Document
			{
				Filial = varclsBatch.BthFilial,
				Chave = varObject.BtiDocKey
			};
			varScrDocList.Add(varclsDoc);
		}
		if (!pSyncFromDbaFirst)
		{
			return varScrDocList;
		}
		List<Document> varDbaDocList = await _clsDataDoc.funcGetSyncListByListAsync(varScrDocList);
		List<Document> varFinalDocList = new List<Document>();
		foreach (Document varScrDocItem in varScrDocList)
		{
			Document varclsDbaItem = varDbaDocList.FirstOrDefault((Document r) => clsFunction.IsEqual(r.Filial, varScrDocItem.Filial) && clsFunction.IsEqual(r.Chave, varScrDocItem.Chave));
			if (varclsDbaItem != null && !pCheckHasXml)
			{
				varFinalDocList.Add(varclsDbaItem);
				continue;
			}
			if (varclsDbaItem == null && !pCheckHasXml)
			{
				if (_clsDataDoc.funcGetDocFromKey(varScrDocItem.Filial, varScrDocItem.Chave) != null)
				{
					varFinalDocList.Add(varclsDbaItem);
				}
				continue;
			}
			if (varclsDbaItem != null && !clsFunction.IsEmpty(varclsDbaItem.HasXml))
			{
				varFinalDocList.Add(varclsDbaItem);
				continue;
			}
			if (varclsDbaItem != null && clsFunction.IsEmpty(varclsDbaItem.HasXml))
			{
				Document varclsDocHasXml = await _clsDataDoc.funcGetItemByChaveHasXmlAsync(varclsDbaItem.Chave);
				if (varclsDocHasXml != null)
				{
					varclsDbaItem = varclsDocHasXml.GetClone();
				}
				varFinalDocList.Add(varclsDbaItem);
				continue;
			}
			Document varclsDocHasXml2 = await _clsDataDoc.funcGetItemByChaveHasXmlAsync(varScrDocItem.Chave);
			if (varclsDocHasXml2 != null)
			{
				varFinalDocList.Add(varclsDocHasXml2.GetClone());
				continue;
			}
			Document varclsDocItem = _clsDataDoc.funcGetDocFromKey(varScrDocItem.Filial, varScrDocItem.Chave);
			if (varclsDocItem != null)
			{
				varFinalDocList.Add(varclsDocItem);
			}
		}
		return varFinalDocList;
	}

	public List<BatchItem> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<BatchItem> varItemList = new List<BatchItem>();
		BatchItem varFocused = new BatchItem();
		if (pFocused && lsvDocs.FocusedItem != null)
		{
			try
			{
				varFocused = (BatchItem)lsvDocs.FocusedObject;
				varItemList.Add(varFocused);
			}
			catch
			{
				varFocused = new BatchItem();
			}
		}
		if (pChecked)
		{
			foreach (BatchItem varObject in lsvDocs.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varItemList.Add(varObject);
				}
			}
			if (varItemList.Count > 0)
			{
				varItemList = lsvDocs.Objects.Cast<BatchItem>().Intersect(varItemList).ToList();
			}
		}
		if ((!pFocused && !pChecked) || varItemList.Count <= 0)
		{
			varItemList = lsvDocs.FilteredObjects.Cast<BatchItem>().ToList();
			if (varItemList.Count <= 0)
			{
				varItemList = lsvDocs.Objects.Cast<BatchItem>().ToList();
			}
		}
		return varItemList;
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

	public void funcSearchText(string pSearchTerm, bool pSearchInteligent)
	{
		if (clsFunction.IsEmpty(pSearchTerm))
		{
			lsvDocs.AdditionalFilter = null;
		}
		else
		{
			if (pSearchInteligent)
			{
				foreach (OLVColumn column in lsvDocs.Columns)
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
				foreach (OLVColumn column2 in lsvDocs.Columns)
				{
					column2.Searchable = true;
				}
			}
			TextMatchFilter varTextMatchFilter = TextMatchFilter.Contains(lsvDocs, pSearchTerm);
			if (lsvDocs.DefaultRenderer == null)
			{
				lsvDocs.DefaultRenderer = new HighlightTextRenderer(varTextMatchFilter);
			}
			lsvDocs.AdditionalFilter = varTextMatchFilter;
		}
		if (lsvDocs.ShowGroups)
		{
			lsvDocs.BuildGroups();
		}
	}

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportDocToExcelAsync<BatchItem>(lsvDocs, onProgressChange);
	}

	public int funcGetTotalDocs()
	{
		return lsvDocs.Items.Count;
	}

	public decimal funcGetTotalValue()
	{
		return 0m;
	}

	private async void tsbRework01_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsBatchService varclsService = new clsBatchService();
		try
		{
			bool varMustWebSync = false;
			tsbRework01.Image = Resources.gif_loading;
			tsbRework01.ToolTipText = "Planejando ...";
			List<BatchHead> varBatchList = lsvBatchs.CheckedObjects.Cast<BatchHead>().ToList();
			if (varBatchList.Count <= 0)
			{
				varBatchList = lsvBatchs.Objects.Cast<BatchHead>().ToList();
			}
			foreach (BatchHead varBatchHead in varBatchList)
			{
				if (!(await clsScreenGeral.funcHasAccessAsync("DOWN-MANAGER", "REWORK", varBatchHead.BthFilial)))
				{
					return;
				}
			}
			List<BatchHead> varBatchListToPost = new List<BatchHead>();
			for (int varIndex = 0; varIndex < varBatchList.Count; varIndex++)
			{
				BatchHead varclsBatch = varBatchList[varIndex];
				List<BatchItem> obj = await varclsDataItem.funcGetListByBatchAsync(varclsBatch.BthId);
				List<BatchItem> varclsProcList = new List<BatchItem>();
				clsFunction.funcConvStrToDec(varclsBatch.BthPerc);
				foreach (BatchItem varclsItem in obj)
				{
					bool varMustRun = false;
					if (varclsItem.IsPlan())
					{
						varMustRun = true;
					}
					else if (varclsItem.HasError())
					{
						varMustRun = true;
					}
					else if (varclsItem.IsFinished())
					{
						varMustRun = false;
					}
					if (varMustRun)
					{
						if (varclsItem.HasError())
						{
							varclsItem.BtiReScan = "X";
						}
						varclsItem.BtiStatus = "PLANNED";
						varclsItem.BtiMessage = clsFunction.funcGetValue(varclsItem.BtiMessage);
						if (clsFunction.Contains(varclsItem.BtiMessage, "sincronizados", pIgnoreCase: true))
						{
							varMustWebSync = true;
							BatchHead batchHead = varclsBatch;
							string bthContact = (varclsBatch.BthInstall = string.Empty);
							batchHead.BthContact = bthContact;
						}
						varclsProcList.Add(varclsItem);
					}
				}
				if (varclsProcList.Count > 0)
				{
					await varclsDataItem.funcUpdateAsync(varclsProcList);
					varclsBatch = await varclsService.funcDefinePercAsync(varclsBatch);
					varBatchListToPost.Add(varclsBatch);
					if (clsFunction.IsEmpty(varclsBatch.ApiServer))
					{
						varclsReturnFunc.AddRange(await varclsService.funcPlanTaskRequestAsync(varclsBatch, varclsProcList));
						continue;
					}
					varclsReturnFunc.AddRange(await varclsService.funcPlanTaskRequestAsync(varclsBatch, pSetPercent: false));
					varclsReturnFunc.AddRange(await varclsService.funcPlanTaskResponseAsync(varclsBatch));
				}
			}
			if (varMustWebSync)
			{
				clsSrvGeral.funcSyncWebDataAsync((FilialView)null, pHardSync: true, "").ContinueWith(delegate
				{
					varclsService.funcPostBatchAsync(varBatchListToPost, null, pIsOnline: false);
				});
			}
			else
			{
				varclsService.funcPostBatchAsync(varBatchListToPost, null, pIsOnline: false);
			}
			funcTimerScreen();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			tsbRework01.Image = Resources.image_rework;
			tsbRework01.ToolTipText = "Realizar o reprocessamento dos itens com erro";
			Cursor.Current = Cursors.Default;
		}
	}

	private async void tsbDelete_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsBatchService varclsBatchService = new clsBatchService();
		try
		{
			tsbDelete.Image = Resources.gif_loading;
			tsbDelete.ToolTipText = "Excluindo os objetos ...";
			List<BatchHead> varBatchList = lsvBatchs.CheckedObjects.Cast<BatchHead>().ToList();
			List<BatchItem> varclsObjList = lsvDocs.Objects.Cast<BatchItem>().ToList();
			if (varclsObjList == null)
			{
				varclsObjList = new List<BatchItem>();
			}
			foreach (BatchHead varBatchHead in varBatchList)
			{
				if (!(await clsScreenGeral.funcHasAccessAsync("DOWN-MANAGER", "DELETE", varBatchHead.BthFilial)))
				{
					return;
				}
			}
			foreach (BatchHead varBatchHead2 in varBatchList)
			{
				if (MessageBox.Show("Confirma a exclusão do lote [ " + varBatchHead2.BthId + " ] e de seus itens relacionadas?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					await varclsDataTask.funcDeleteByBatchNumAsync(varBatchHead2.BthId);
					await varclsDataItem.funcDeleteByBatchAsync(varBatchHead2.BthId);
					await varclsDataTask.funcDeleteByBatchNumAsync(varBatchHead2.BthId);
					await varclsDataBatch.funcDeleteAsync(varBatchHead2);
					string varParamKey = await varclsBatchService.funcGetUserBatchKeyAsync(varBatchHead2);
					await varclsDataParam.funcGetAsync(varParamKey);
					await varclsDataParam.funcSetAsync(varParamKey, string.Empty);
					varclsObjList.RemoveAll((BatchItem r) => r.BtiBatch.Equals(varBatchHead2.BthId));
					_clsBufferList.Remove(varBatchHead2);
					lsvBatchs.RemoveObject(varBatchHead2);
				}
			}
			funcTimerScreen();
			lsvDocs.BeginUpdate();
			lsvDocs.SuspendLayout();
			lsvDocs.SetObjects(varclsObjList);
			lsvDocs.ShowGroups = true;
			lsvDocs.BuildGroups(olvBtiBatch, SortOrder.Ascending);
			lsvDocs.EndUpdate();
			lsvDocs.ResumeLayout();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			tsbDelete.Image = Resources.image_delete_object;
			tsbDelete.ToolTipText = "Excluir objetos";
			Cursor.Current = Cursors.Default;
		}
	}

	private async void tsbRefresh_Click(object sender, EventArgs e)
	{
		await funcLoadDataAsync(_clsDataFilter);
	}

	private void tsbTimer_Click(object sender, EventArgs e)
	{
		tsbTimer.Checked = !tsbTimer.Checked;
	}

	private void lsvDocs_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewerAsync(pShowPDF: true, pShowXML: false);
	}

	private async void lsvDocs_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode.Equals(Keys.C))
		{
			await funcCopyDocKeyAsync();
		}
	}

	private async void tsbSystemAct_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!_IsLoading)
		{
			string varSelected = clsFunction.funcGetValue(((clsObjectType)tsbSystemAct.SelectedItem).Value);
			await varclsDataParam.funcSetAsync("TabBatch-BatchType", varSelected);
			Cursor.Current = Cursors.WaitCursor;
			await funcLoadDataAsync(_clsDataFilter);
			Cursor.Current = Cursors.Default;
		}
	}

	private async void lsvBatchs_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		OLVListItem varclsItem = (OLVListItem)e.Item;
		if (varclsItem == null)
		{
			return;
		}
		BatchHead varclsObject = (BatchHead)varclsItem.RowObject;
		if (varclsObject == null)
		{
			return;
		}
		try
		{
			await _semaphore.WaitAsync();
			Cursor.Current = Cursors.WaitCursor;
			List<BatchItem> varclsObjList = lsvDocs.Objects.Cast<BatchItem>().ToList();
			if (varclsObjList == null)
			{
				varclsObjList = new List<BatchItem>();
			}
			if (!varclsItem.Checked)
			{
				varclsObjList.RemoveAll((BatchItem r) => r.BtiBatch.Equals(varclsObject.BthId));
				_clsBufferList.Remove(varclsObject);
			}
			else
			{
				varclsObjList.AddRange(await varclsDataItem.funcGetListByBatchAsync(varclsObject.BthId));
				_clsBufferList.Add(varclsObject);
			}
			lsvDocs.BeginUpdate();
			lsvDocs.SuspendLayout();
			olvBtiSelect.HeaderCheckState = CheckState.Unchecked;
			lsvDocs.SetObjects(varclsObjList);
			lsvDocs.ShowGroups = true;
			lsvDocs.BuildGroups(olvBtiBatch, SortOrder.Ascending);
			lsvDocs.EndUpdate();
			lsvDocs.ResumeLayout();
		}
		finally
		{
			Cursor.Current = Cursors.Default;
			_semaphore.Release();
		}
	}

	private void lsvDocs_FormatRow(object sender, FormatRowEventArgs e)
	{
		e.Item.BackColor = (e.Item.Checked ? Color.LightBlue : Color.White);
	}

	private void lsvDocs_HeaderCheckBoxChanging(object sender, HeaderCheckBoxChangingEventArgs e)
	{
		if (e.NewCheckState == CheckState.Checked)
		{
			lsvDocs.CheckAll();
		}
		else
		{
			lsvDocs.UncheckAll();
		}
	}

	private void lsvDocs_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (!e.Item.Checked)
		{
			olvBtiSelect.HeaderCheckState = CheckState.Unchecked;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabBatch));
		this.pnContent = new System.Windows.Forms.Panel();
		this.splitBatch = new System.Windows.Forms.SplitContainer();
		this.lsvBatchs = new BrightIdeasSoftware.FastObjectListView();
		this.olvBthSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvBthId = new BrightIdeasSoftware.OLVColumn();
		this.olvBthFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvBthDescrt = new BrightIdeasSoftware.OLVColumn();
		this.olvBthPerc = new BrightIdeasSoftware.OLVColumn();
		this.olvBthItems = new BrightIdeasSoftware.OLVColumn();
		this.olvBthTotEr = new BrightIdeasSoftware.OLVColumn();
		this.olvBthPendt = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.tspBatchs = new System.Windows.Forms.ToolStrip();
		this.tsbSep01 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSystemAct = new System.Windows.Forms.ToolStripComboBox();
		this.tsbSep07 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
		this.tsbSep04 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbRework01 = new System.Windows.Forms.ToolStripButton();
		this.tsbSep05 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbTimer = new System.Windows.Forms.ToolStripButton();
		this.tsbSep06 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.lsvDocs = new BrightIdeasSoftware.FastObjectListView();
		this.olvBtiSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvBtiBatch = new BrightIdeasSoftware.OLVColumn();
		this.olvBtiDocKey = new BrightIdeasSoftware.OLVColumn();
		this.olvBtiMessage = new BrightIdeasSoftware.OLVColumn();
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tspDocs = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbPlanned = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbPendent = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSucess = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbWarning = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbError = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.olvAudRole = new BrightIdeasSoftware.OLVColumn();
		this.timerRefresh = new System.Windows.Forms.Timer(this.components);
		this.pnContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitBatch).BeginInit();
		this.splitBatch.Panel1.SuspendLayout();
		this.splitBatch.Panel2.SuspendLayout();
		this.splitBatch.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvBatchs).BeginInit();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		this.tspBatchs.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvDocs).BeginInit();
		this.tspDocs.SuspendLayout();
		base.SuspendLayout();
		this.pnContent.Controls.Add(this.splitBatch);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(1025, 564);
		this.pnContent.TabIndex = 10;
		this.splitBatch.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitBatch.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitBatch.Location = new System.Drawing.Point(0, 0);
		this.splitBatch.Name = "splitBatch";
		this.splitBatch.Panel1.Controls.Add(this.lsvBatchs);
		this.splitBatch.Panel1.Controls.Add(this.plnMessage);
		this.splitBatch.Panel1.Controls.Add(this.tspBatchs);
		this.splitBatch.Panel2.Controls.Add(this.lsvDocs);
		this.splitBatch.Panel2.Controls.Add(this.tspDocs);
		this.splitBatch.Panel2MinSize = 0;
		this.splitBatch.Size = new System.Drawing.Size(1025, 564);
		this.splitBatch.SplitterDistance = 400;
		this.splitBatch.TabIndex = 0;
		this.lsvBatchs.AllColumns.Add(this.olvBthSelect);
		this.lsvBatchs.AllColumns.Add(this.olvBthId);
		this.lsvBatchs.AllColumns.Add(this.olvBthFilial);
		this.lsvBatchs.AllColumns.Add(this.olvBthDescrt);
		this.lsvBatchs.AllColumns.Add(this.olvBthPerc);
		this.lsvBatchs.AllColumns.Add(this.olvBthItems);
		this.lsvBatchs.AllColumns.Add(this.olvBthTotEr);
		this.lsvBatchs.AllColumns.Add(this.olvBthPendt);
		this.lsvBatchs.AllowColumnReorder = true;
		this.lsvBatchs.AllowDrop = true;
		this.lsvBatchs.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.lsvBatchs.CellEditUseWholeCell = false;
		this.lsvBatchs.CheckBoxes = true;
		this.lsvBatchs.CheckedAspectName = "";
		this.lsvBatchs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[7] { this.olvBthSelect, this.olvBthId, this.olvBthDescrt, this.olvBthPerc, this.olvBthItems, this.olvBthTotEr, this.olvBthPendt });
		this.lsvBatchs.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvBatchs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lsvBatchs.EmptyListMsgFont = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvBatchs.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvBatchs.FullRowSelect = true;
		this.lsvBatchs.HeaderWordWrap = true;
		this.lsvBatchs.HideSelection = false;
		this.lsvBatchs.IncludeColumnHeadersInCopy = true;
		this.lsvBatchs.Location = new System.Drawing.Point(0, 25);
		this.lsvBatchs.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lsvBatchs.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lsvBatchs.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lsvBatchs.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lsvBatchs.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lsvBatchs.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lsvBatchs.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lsvBatchs.MenuLabelUnsort = "Remover ordenação";
		this.lsvBatchs.Name = "lsvBatchs";
		this.lsvBatchs.OverlayText.Alignment = System.Drawing.ContentAlignment.BottomLeft;
		this.lsvBatchs.OverlayText.BorderColor = System.Drawing.Color.FromArgb(192, 192, 0);
		this.lsvBatchs.OverlayText.BorderWidth = 2f;
		this.lsvBatchs.OverlayText.Rotation = -20;
		this.lsvBatchs.OverlayText.Text = "";
		this.lsvBatchs.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
		this.lsvBatchs.ShowCommandMenuOnRightClick = true;
		this.lsvBatchs.ShowGroups = false;
		this.lsvBatchs.ShowHeaderInAllViews = false;
		this.lsvBatchs.ShowImagesOnSubItems = true;
		this.lsvBatchs.ShowItemToolTips = true;
		this.lsvBatchs.Size = new System.Drawing.Size(400, 539);
		this.lsvBatchs.SmallImageList = this.ImageListDocs;
		this.lsvBatchs.SortGroupItemsByPrimaryColumn = false;
		this.lsvBatchs.TabIndex = 237;
		this.lsvBatchs.UseAlternatingBackColors = true;
		this.lsvBatchs.UseCellFormatEvents = true;
		this.lsvBatchs.UseCompatibleStateImageBehavior = false;
		this.lsvBatchs.UseFilterIndicator = true;
		this.lsvBatchs.UseFiltering = true;
		this.lsvBatchs.UseHotItem = true;
		this.lsvBatchs.View = System.Windows.Forms.View.Details;
		this.lsvBatchs.VirtualMode = true;
		this.lsvBatchs.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvBatchs_ItemChecked);
		this.olvBthSelect.Groupable = false;
		this.olvBthSelect.HeaderCheckBox = true;
		this.olvBthSelect.MinimumWidth = 50;
		this.olvBthSelect.Sortable = false;
		this.olvBthSelect.Text = "";
		this.olvBthSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBthSelect.UseFiltering = false;
		this.olvBthSelect.Width = 50;
		this.olvBthId.AspectName = "BthId";
		this.olvBthId.Text = "Lote";
		this.olvBthId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBthFilial.AspectName = "BthFilial";
		this.olvBthFilial.DisplayIndex = 2;
		this.olvBthFilial.IsVisible = false;
		this.olvBthFilial.Text = "Empresa";
		this.olvBthFilial.Width = 20;
		this.olvBthDescrt.AspectName = "BthDescrt";
		this.olvBthDescrt.FillsFreeSpace = true;
		this.olvBthDescrt.Text = "Descrição";
		this.olvBthDescrt.Width = 100;
		this.olvBthPerc.AspectName = "BthPerc";
		this.olvBthPerc.MinimumWidth = 35;
		this.olvBthPerc.Text = "%";
		this.olvBthPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBthPerc.ToolTipText = "";
		this.olvBthPerc.Width = 42;
		this.olvBthItems.AspectName = "BthItems";
		this.olvBthItems.MinimumWidth = 40;
		this.olvBthItems.Text = "Itens";
		this.olvBthItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBthItems.Width = 40;
		this.olvBthTotEr.AspectName = "BthTotEr";
		this.olvBthTotEr.MinimumWidth = 40;
		this.olvBthTotEr.Tag = "";
		this.olvBthTotEr.Text = "Erros";
		this.olvBthTotEr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBthTotEr.Width = 40;
		this.olvBthPendt.AspectName = "BthPendt";
		this.olvBthPendt.MinimumWidth = 42;
		this.olvBthPendt.Text = "Pend.";
		this.olvBthPendt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBthPendt.Width = 42;
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_schedule.png");
		this.ImageListDocs.Images.SetKeyName(1, "image_working.png");
		this.ImageListDocs.Images.SetKeyName(2, "image_error.png");
		this.ImageListDocs.Images.SetKeyName(3, "image_clock_go_32.png");
		this.ImageListDocs.Images.SetKeyName(4, "dfe_confirm.png");
		this.ImageListDocs.Images.SetKeyName(5, "image_warning.png");
		this.ImageListDocs.Images.SetKeyName(6, "image_logger.png");
		this.ImageListDocs.Images.SetKeyName(7, "image_pause.png");
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 704);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(400, 38);
		this.plnMessage.TabIndex = 235;
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
		this.tspBatchs.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspBatchs.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.tsbSep01, this.tsbSystemAct, this.tsbSep07, this.tsbRefresh, this.tsbSep04, this.tsbRework01, this.tsbSep05, this.tsbTimer, this.tsbSep06, this.tsbDelete,
			this.toolStripSeparator7
		});
		this.tspBatchs.Location = new System.Drawing.Point(0, 0);
		this.tspBatchs.Name = "tspBatchs";
		this.tspBatchs.Size = new System.Drawing.Size(400, 25);
		this.tspBatchs.TabIndex = 236;
		this.tspBatchs.Text = "toolStrip1";
		this.tsbSep01.Name = "tsbSep01";
		this.tsbSep01.Size = new System.Drawing.Size(6, 25);
		this.tsbSystemAct.BackColor = System.Drawing.SystemColors.Info;
		this.tsbSystemAct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.tsbSystemAct.Items.AddRange(new object[3] { "Lotes automáticos", "Lotes manuais", "Todos os Lotes" });
		this.tsbSystemAct.Name = "tsbSystemAct";
		this.tsbSystemAct.Size = new System.Drawing.Size(120, 25);
		this.tsbSystemAct.SelectedIndexChanged += new System.EventHandler(tsbSystemAct_SelectedIndexChanged);
		this.tsbSep07.Name = "tsbSep07";
		this.tsbSep07.Size = new System.Drawing.Size(6, 25);
		this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbRefresh.Image = Monitor.Resources.image_refresh;
		this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbRefresh.Name = "tsbRefresh";
		this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
		this.tsbRefresh.Text = "Atualizar";
		this.tsbRefresh.ToolTipText = "Atualizar lista de lotes na tela";
		this.tsbRefresh.Click += new System.EventHandler(tsbRefresh_Click);
		this.tsbSep04.Name = "tsbSep04";
		this.tsbSep04.Size = new System.Drawing.Size(6, 25);
		this.tsbRework01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbRework01.Image = Monitor.Resources.image_rework;
		this.tsbRework01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbRework01.Name = "tsbRework01";
		this.tsbRework01.Size = new System.Drawing.Size(23, 22);
		this.tsbRework01.Text = "Reprocessar";
		this.tsbRework01.ToolTipText = "Realizar o reprocessamento dos itens com erro";
		this.tsbRework01.Click += new System.EventHandler(tsbRework01_Click);
		this.tsbSep05.Name = "tsbSep05";
		this.tsbSep05.Size = new System.Drawing.Size(6, 25);
		this.tsbTimer.Image = Monitor.Resources.image_countdown;
		this.tsbTimer.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbTimer.Name = "tsbTimer";
		this.tsbTimer.Size = new System.Drawing.Size(55, 22);
		this.tsbTimer.Text = "[10 s]";
		this.tsbTimer.Click += new System.EventHandler(tsbTimer_Click);
		this.tsbSep06.Name = "tsbSep06";
		this.tsbSep06.Size = new System.Drawing.Size(6, 25);
		this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbDelete.Image = Monitor.Resources.image_delete_object;
		this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDelete.Name = "tsbDelete";
		this.tsbDelete.Size = new System.Drawing.Size(23, 22);
		this.tsbDelete.Tag = "";
		this.tsbDelete.Text = "Excluir Tarefa";
		this.tsbDelete.Click += new System.EventHandler(tsbDelete_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
		this.lsvDocs.AllColumns.Add(this.olvBtiSelect);
		this.lsvDocs.AllColumns.Add(this.olvBtiBatch);
		this.lsvDocs.AllColumns.Add(this.olvBtiDocKey);
		this.lsvDocs.AllColumns.Add(this.olvBtiMessage);
		this.lsvDocs.AllowColumnReorder = true;
		this.lsvDocs.AllowDrop = true;
		this.lsvDocs.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.lsvDocs.CellEditUseWholeCell = false;
		this.lsvDocs.CheckBoxes = true;
		this.lsvDocs.CheckedAspectName = "";
		this.lsvDocs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.olvBtiSelect, this.olvBtiBatch, this.olvBtiDocKey, this.olvBtiMessage });
		this.lsvDocs.ContextMenuStrip = this.contextMenuDocs;
		this.lsvDocs.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lsvDocs.EmptyListMsg = "Ops. Nenhum lote foi selecionado.";
		this.lsvDocs.EmptyListMsgFont = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvDocs.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvDocs.FullRowSelect = true;
		this.lsvDocs.HeaderWordWrap = true;
		this.lsvDocs.HideSelection = false;
		this.lsvDocs.IncludeColumnHeadersInCopy = true;
		this.lsvDocs.Location = new System.Drawing.Point(0, 25);
		this.lsvDocs.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lsvDocs.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lsvDocs.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lsvDocs.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lsvDocs.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lsvDocs.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lsvDocs.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lsvDocs.MenuLabelUnsort = "Remover ordenação";
		this.lsvDocs.Name = "lsvDocs";
		this.lsvDocs.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
		this.lsvDocs.ShowCommandMenuOnRightClick = true;
		this.lsvDocs.ShowGroups = false;
		this.lsvDocs.ShowHeaderInAllViews = false;
		this.lsvDocs.ShowImagesOnSubItems = true;
		this.lsvDocs.ShowItemCountOnGroups = true;
		this.lsvDocs.ShowItemToolTips = true;
		this.lsvDocs.Size = new System.Drawing.Size(621, 539);
		this.lsvDocs.SmallImageList = this.ImageListDocs;
		this.lsvDocs.SortGroupItemsByPrimaryColumn = false;
		this.lsvDocs.TabIndex = 38;
		this.lsvDocs.UseAlternatingBackColors = true;
		this.lsvDocs.UseCellFormatEvents = true;
		this.lsvDocs.UseCompatibleStateImageBehavior = false;
		this.lsvDocs.UseFilterIndicator = true;
		this.lsvDocs.UseFiltering = true;
		this.lsvDocs.UseHotItem = true;
		this.lsvDocs.View = System.Windows.Forms.View.Details;
		this.lsvDocs.VirtualMode = true;
		this.lsvDocs.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(lsvDocs_FormatRow);
		this.lsvDocs.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(lsvDocs_HeaderCheckBoxChanging);
		this.lsvDocs.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvDocs_ItemChecked);
		this.lsvDocs.DoubleClick += new System.EventHandler(lsvDocs_DoubleClick);
		this.lsvDocs.KeyUp += new System.Windows.Forms.KeyEventHandler(lsvDocs_KeyUp);
		this.olvBtiSelect.Groupable = false;
		this.olvBtiSelect.HeaderCheckBox = true;
		this.olvBtiSelect.HeaderCheckBoxUpdatesRowCheckBoxes = false;
		this.olvBtiSelect.Sortable = false;
		this.olvBtiSelect.Text = "";
		this.olvBtiSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBtiSelect.UseFiltering = false;
		this.olvBtiSelect.Width = 50;
		this.olvBtiBatch.AspectName = "BtiBatch";
		this.olvBtiBatch.Text = "Lote";
		this.olvBtiBatch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvBtiDocKey.AspectName = "BtiDocKey";
		this.olvBtiDocKey.Hyperlink = true;
		this.olvBtiDocKey.Text = "Documento";
		this.olvBtiDocKey.Width = 280;
		this.olvBtiMessage.AspectName = "BtiMessage";
		this.olvBtiMessage.FillsFreeSpace = true;
		this.olvBtiMessage.MinimumWidth = 60;
		this.olvBtiMessage.Text = "Mensagem";
		this.olvBtiMessage.ToolTipText = "";
		this.olvBtiMessage.Width = 172;
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(28, 28);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.tspDocs.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspDocs.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripSeparator2, this.tsbPlanned, this.toolStripSeparator1, this.tsbPendent, this.toolStripSeparator6, this.tsbSucess, this.toolStripSeparator3, this.tsbWarning, this.toolStripSeparator4, this.tsbError,
			this.toolStripSeparator5
		});
		this.tspDocs.Location = new System.Drawing.Point(0, 0);
		this.tspDocs.Name = "tspDocs";
		this.tspDocs.Size = new System.Drawing.Size(621, 25);
		this.tspDocs.TabIndex = 39;
		this.tspDocs.Text = "toolStrip1";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.tsbPlanned.Image = Monitor.Resources.image_schedule;
		this.tsbPlanned.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbPlanned.Name = "tsbPlanned";
		this.tsbPlanned.Size = new System.Drawing.Size(74, 22);
		this.tsbPlanned.Text = "Planejado";
		this.tsbPlanned.Click += new System.EventHandler(tsbPlanned_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tsbPendent.Image = Monitor.Resources.image_working;
		this.tsbPendent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbPendent.Name = "tsbPendent";
		this.tsbPendent.Size = new System.Drawing.Size(84, 22);
		this.tsbPendent.Text = "Executando";
		this.tsbPendent.Click += new System.EventHandler(tsbPendent_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.tsbSucess.Image = Monitor.Resources.dfe_confirm;
		this.tsbSucess.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSucess.Name = "tsbSucess";
		this.tsbSucess.Size = new System.Drawing.Size(66, 22);
		this.tsbSucess.Text = "Sucesso";
		this.tsbSucess.Click += new System.EventHandler(tsbSucess_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.tsbWarning.Image = Monitor.Resources.image_warning;
		this.tsbWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbWarning.Name = "tsbWarning";
		this.tsbWarning.Size = new System.Drawing.Size(58, 22);
		this.tsbWarning.Text = "Avisos";
		this.tsbWarning.Click += new System.EventHandler(tsbWarning_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.tsbError.Image = Monitor.Resources.image_error;
		this.tsbError.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbError.Name = "tsbError";
		this.tsbError.Size = new System.Drawing.Size(52, 22);
		this.tsbError.Text = "Erros";
		this.tsbError.Click += new System.EventHandler(tsbError_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.olvAudRole.AspectName = "AudRole";
		this.olvAudRole.IsVisible = false;
		this.olvAudRole.Text = "Regra";
		this.timerRefresh.Enabled = true;
		this.timerRefresh.Interval = 1000;
		this.timerRefresh.Tick += new System.EventHandler(timerRefresh_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1025, 564);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabBatch";
		this.Text = "Lotes de Processamento";
		base.Load += new System.EventHandler(frmTabBatch_Load);
		this.pnContent.ResumeLayout(false);
		this.splitBatch.Panel1.ResumeLayout(false);
		this.splitBatch.Panel1.PerformLayout();
		this.splitBatch.Panel2.ResumeLayout(false);
		this.splitBatch.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitBatch).EndInit();
		this.splitBatch.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lsvBatchs).EndInit();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		this.tspBatchs.ResumeLayout(false);
		this.tspBatchs.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvDocs).EndInit();
		this.tspDocs.ResumeLayout(false);
		this.tspDocs.PerformLayout();
		base.ResumeLayout(false);
	}
}
