using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabDocJump : Form
{
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

	private clsSrvTabDocJump _SqlTabData;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private bool _ColumnsLoaded;

	private bool _GroupColapsed;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private string _SqlFields = " * ";

	private bool _IsFormLoaded;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private ImageList ImageListDocs;

	private OLVColumn olvDest;

	private Label lbTitle01;

	private Label lbTitle02;

	private Label lbText01;

	private Label lbText02;

	private Label label3;

	private Label lbTitle03;

	private FastObjectListView lsvData;

	private OLVColumn olvSelect;

	private OLVColumn olvSkipFilial;

	private OLVColumn olvSkipTipo;

	private OLVColumn olvSkipSerie;

	private OLVColumn olvFirstNum;

	private OLVColumn olvLastNum;

	private LinkLabel lknClose;

	private Button btClose;

	private Panel pnMarket;

	private Panel pnMarketContent;

	private Label label1;

	private Label label2;

	private PictureBox picWarning;

	private LinkLabel linkAction;

	private Label label5;

	private Label label6;

	private Label label7;

	private Button btSalesContact;

	private Button btSalesAction;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabDocJump(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
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

	private async void frmTabDocJump_Load(object sender, EventArgs e)
	{
		_IsFormLoaded = false;
		await new clsDataConfig().funcGetItemByKeyAsync();
		_ColumnsLoaded = false;
		clsScreenGeral.funcSetColumnsObjectListView(this, lsvData);
		_ColumnsLoaded = true;
		foreach (OLVColumn allColumn in lsvData.AllColumns)
		{
			allColumn.VisibilityChanged += OlvColumn_VisibilityChanged;
		}
		if (clsFunction.IsAdmin)
		{
			clsScreenGeral.funcCheckListViewDdic(typeof(DocJumpOut), lsvData);
		}
		_SqlFields = "document.filial, document.model, document.serie, document.num";
		_SqlFields = clsFunction.funcClearEnd(_SqlFields, ",");
		_SqlTabData = new clsSrvTabDocJump(_SqlFields);
		funcDefineListViewFeatures();
		lbTitle01.Text = "Relatório " + Text;
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
		Button button = btSalesContact;
		bool visible = (btSalesAction.Visible = false);
		button.Visible = visible;
		if (_IsLocked)
		{
			pnMarket.Visible = true;
			Button button2 = btClose;
			visible = (lknClose.Visible = false);
			button2.Visible = visible;
			picWarning.Image = Resources.image_locker;
			if (varclsSalesData.ShowPlanAction)
			{
				Button button3 = btSalesContact;
				visible = (btSalesAction.Visible = true);
				button3.Visible = visible;
			}
			else
			{
				Button button4 = btSalesContact;
				visible = (btSalesAction.Visible = false);
				button4.Visible = visible;
			}
		}
		else if (!varMustShow)
		{
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
		olvFirstNum.AspectGetter = delegate(object x)
		{
			DocJumpOut docJumpOut = (DocJumpOut)x;
			return (docJumpOut == null) ? null : clsFunction.funcGetValue(docJumpOut.FirstNum).PadLeft(9, '0');
		};
		olvLastNum.AspectGetter = delegate(object x)
		{
			DocJumpOut docJumpOut = (DocJumpOut)x;
			return (docJumpOut == null) ? null : clsFunction.funcGetValue(docJumpOut.LastNum).PadLeft(9, '0');
		};
		olvSkipTipo.AspectGetter = delegate(object x)
		{
			DocJumpOut docJumpOut = (DocJumpOut)x;
			return (docJumpOut == null) ? null : clsFunction.funcGetDocType(docJumpOut.Model);
		};
		olvSkipFilial.AspectGetter = delegate(object x)
		{
			DocJumpOut docJumpOut = (DocJumpOut)x;
			return (docJumpOut == null) ? null : clsFunction.funcFormatDoc(docJumpOut.Filial);
		};
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
		List<Document> varclsDocList = await new clsDataDoc().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		olvSelect.HeaderCheckState = CheckState.Unchecked;
		List<DocJumpOut> varJumpList = await Task.Run(() => funcLoadSkipNum(varclsDocList));
		if (_IsLocked)
		{
			varJumpList = varJumpList.Take(4).ToList();
		}
		lsvData.SetObjects(varJumpList);
		_GroupColapsed = false;
		lsvData.ResumeLayout();
		lsvData.EndUpdate();
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

	public async Task<bool> funcSetTagAsync(string pTagCode, bool pFocused, bool pChecked)
	{
		return false;
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
					if (column.Text == "Empresa")
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
		return await clsScreenGeral.funcExportDocToExcelAsync<DocJumpOut>(lsvData, onProgressChange);
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
	}

	private void lsvData_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (!e.Item.Checked)
		{
			olvSelect.HeaderCheckState = CheckState.Unchecked;
		}
	}

	private async void btExcel_Click(object sender, EventArgs e)
	{
		await funcExportJumpsToExcelAsync();
	}

	private async Task<clsReturn> funcExportJumpsToExcelAsync()
	{
		return await clsScreenGeral.funcExportDocToExcelAsync<DocJumpOut>(lsvData);
	}

	private void lsvData_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
	}

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipsHowToScanDFeOutAsync();
	}

	private async void lknClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
		await _clsDataParam.funcSetAsync(_consMarketScreenUser, "X");
	}

	private void linkAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallJumpOutAsync();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocJump));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lsvData = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvSkipFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvSkipTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvSkipSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvFirstNum = new BrightIdeasSoftware.OLVColumn();
		this.olvLastNum = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.lbText02 = new System.Windows.Forms.Label();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.btClose = new System.Windows.Forms.Button();
		this.pnMarket = new System.Windows.Forms.Panel();
		this.pnMarketContent = new System.Windows.Forms.Panel();
		this.btSalesContact = new System.Windows.Forms.Button();
		this.btSalesAction = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.picWarning = new System.Windows.Forms.PictureBox();
		this.linkAction = new System.Windows.Forms.LinkLabel();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.lsvData.AllColumns.Add(this.olvSelect);
		this.lsvData.AllColumns.Add(this.olvSkipFilial);
		this.lsvData.AllColumns.Add(this.olvSkipTipo);
		this.lsvData.AllColumns.Add(this.olvSkipSerie);
		this.lsvData.AllColumns.Add(this.olvFirstNum);
		this.lsvData.AllColumns.Add(this.olvLastNum);
		this.lsvData.AllowColumnReorder = true;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.CheckBoxes = true;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[6] { this.olvSelect, this.olvSkipFilial, this.olvSkipTipo, this.olvSkipSerie, this.olvFirstNum, this.olvLastNum });
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.Dock = System.Windows.Forms.DockStyle.Fill;
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
		this.lsvData.Size = new System.Drawing.Size(880, 529);
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
		this.lsvData.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(lsvData_HeaderCheckBoxChanging);
		this.lsvData.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(lsvData_ItemsChanged);
		this.lsvData.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lsvData_ColumnWidthChanged);
		this.lsvData.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvData_ItemChecked);
		this.olvSelect.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelect.Groupable = false;
		this.olvSelect.HeaderCheckBox = true;
		this.olvSelect.HeaderCheckBoxUpdatesRowCheckBoxes = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvSkipFilial.AspectName = "Filial";
		this.olvSkipFilial.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSkipFilial.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSkipFilial.Text = "Empresa";
		this.olvSkipFilial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSkipFilial.Width = 130;
		this.olvSkipTipo.AspectName = "Model";
		this.olvSkipTipo.Groupable = false;
		this.olvSkipTipo.Text = "Tipo";
		this.olvSkipTipo.Width = 40;
		this.olvSkipSerie.AspectName = "Serie";
		this.olvSkipSerie.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSkipSerie.Text = "Série";
		this.olvSkipSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSkipSerie.Width = 44;
		this.olvFirstNum.AspectName = "FirstNum";
		this.olvFirstNum.Groupable = false;
		this.olvFirstNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFirstNum.Text = "Início";
		this.olvFirstNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFirstNum.Width = 100;
		this.olvLastNum.AspectName = "LastNum";
		this.olvLastNum.Text = "Término";
		this.olvLastNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvLastNum.Width = 100;
		this.ImageListDocs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
		this.ImageListDocs.ImageSize = new System.Drawing.Size(16, 16);
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.lbTitle03.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle03.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle03.Location = new System.Drawing.Point(17, 195);
		this.lbTitle03.Name = "lbTitle03";
		this.lbTitle03.Size = new System.Drawing.Size(88, 17);
		this.lbTitle03.TabIndex = 212;
		this.lbTitle03.Text = "Saiba mais:";
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.BackColor = System.Drawing.Color.Gainsboro;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Gainsboro;
		this.label3.Location = new System.Drawing.Point(369, 34);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(560, 1);
		this.label3.TabIndex = 208;
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(17, 124);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 54);
		this.lbText02.TabIndex = 196;
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 71);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 35);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = "Aponta os saltos de numeração dos documentos emitidos pela empresa";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(369, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 23);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Relatório : Saltos de numeração";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(17, 45);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Importante saber";
		this.lknClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lknClose.AutoSize = true;
		this.lknClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknClose.Location = new System.Drawing.Point(698, 388);
		this.lknClose.Name = "lknClose";
		this.lknClose.Size = new System.Drawing.Size(176, 16);
		this.lknClose.TabIndex = 204;
		this.lknClose.TabStop = true;
		this.lknClose.Text = "Não mostrar mais esse alerta";
		this.lknClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lknClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknClose_LinkClicked);
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(849, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 3;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 118);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(880, 411);
		this.pnMarket.TabIndex = 3;
		this.pnMarket.Visible = false;
		this.pnMarketContent.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pnMarketContent.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarketContent.Controls.Add(this.btSalesContact);
		this.pnMarketContent.Controls.Add(this.btSalesAction);
		this.pnMarketContent.Controls.Add(this.label1);
		this.pnMarketContent.Controls.Add(this.label2);
		this.pnMarketContent.Controls.Add(this.picWarning);
		this.pnMarketContent.Controls.Add(this.linkAction);
		this.pnMarketContent.Controls.Add(this.label5);
		this.pnMarketContent.Controls.Add(this.label6);
		this.pnMarketContent.Controls.Add(this.label7);
		this.pnMarketContent.Location = new System.Drawing.Point(160, 64);
		this.pnMarketContent.Name = "pnMarketContent";
		this.pnMarketContent.Size = new System.Drawing.Size(566, 307);
		this.pnMarketContent.TabIndex = 195;
		this.btSalesContact.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesContact.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesContact.ForeColor = System.Drawing.Color.White;
		this.btSalesContact.Location = new System.Drawing.Point(71, 261);
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
		this.btSalesAction.Location = new System.Drawing.Point(290, 261);
		this.btSalesAction.Name = "btSalesAction";
		this.btSalesAction.Size = new System.Drawing.Size(188, 28);
		this.btSalesAction.TabIndex = 218;
		this.btSalesAction.Text = "Conheça os planos";
		this.btSalesAction.UseVisualStyleBackColor = false;
		this.btSalesAction.Visible = false;
		this.btSalesAction.Click += new System.EventHandler(btSalesAction_Click);
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(17, 223);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 17);
		this.label1.TabIndex = 212;
		this.label1.Text = "Saiba mais:";
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label2.BackColor = System.Drawing.Color.Gainsboro;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Gainsboro;
		this.label2.Location = new System.Drawing.Point(3, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(560, 1);
		this.label2.TabIndex = 208;
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.picWarning.Image = Monitor.Resources.image_help;
		this.picWarning.Location = new System.Drawing.Point(20, 4);
		this.picWarning.Name = "picWarning";
		this.picWarning.Size = new System.Drawing.Size(20, 20);
		this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picWarning.TabIndex = 206;
		this.picWarning.TabStop = false;
		this.linkAction.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.linkAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkAction.Location = new System.Drawing.Point(111, 223);
		this.linkAction.Name = "linkAction";
		this.linkAction.Size = new System.Drawing.Size(440, 16);
		this.linkAction.TabIndex = 203;
		this.linkAction.TabStop = true;
		this.linkAction.Text = "Saltos de Numeração";
		this.linkAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkAction_LinkClicked);
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label5.Location = new System.Drawing.Point(17, 71);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(530, 136);
		this.label5.TabIndex = 195;
		this.label5.Text = resources.GetString("label5.Text");
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(560, 23);
		this.label6.TabIndex = 187;
		this.label6.Text = "Relatório: Saltos de Numeração";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label7.Location = new System.Drawing.Point(17, 45);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(530, 17);
		this.label7.TabIndex = 194;
		this.label7.Text = "Definição do relatório";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(880, 529);
		base.Controls.Add(this.pnMarket);
		base.Controls.Add(this.lsvData);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocJump";
		this.Text = "Saltos de numeracao";
		base.Load += new System.EventHandler(frmTabDocJump_Load);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
