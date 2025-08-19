using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using audit.fiscal.io;
using BrightIdeasSoftware;
using data.fiscal.io;
using data.fiscal.io.Models.Comex;
using manager.fiscal.io;
using monitor.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using srv.fiscal.io.Siscomex;
using util.fiscal.io;

namespace Monitor;

public class frmTabExpBalance : Form
{
	public class clsOlvBalance : ICloneable
	{
		public string Guid { get; set; }

		public string RegType { get; set; }

		public string DocNum { get; set; }

		public string Valor { get; set; }

		public string Descript { get; set; }

		public DateTime? DtEmit { get; set; }

		public DateTime? DtEndOf { get; set; }

		public long? DtTotDays { get; set; }

		public string Unit { get; set; }

		public string ProdCode { get; set; }

		public string ProdNcm { get; set; }

		public string ProdCfop { get; set; }

		public decimal? QtExport { get; set; }

		public decimal? QtDue { get; set; }

		public decimal? QtAverb { get; set; }

		public decimal? QtDif01 { get; set; }

		public decimal? QtDif02 { get; set; }

		public string Chave { get; set; }

		public string Tag { get; set; }

		public string Canceled { get; set; }

		public string HasCancelEvent { get; set; }

		public string DocNote { get; set; }

		public string Status { get; set; }

		public string DueItem { get; set; }

		public string Filial { get; set; }

		public DateTime? DataDue { get; set; }

		public DateTime? DtEmbar { get; set; }

		public DateTime? DataAverb { get; set; }

		public string DueNum { get; set; }

		public string DueChave { get; set; }

		public string DueStatus { get; set; }

		public string LpcoStatus { get; set; }

		public string HasDossie { get; set; }

		public string DueCanal { get; set; }

		public string PaisDestName { get; set; }

		public string TipoDoc { get; set; }

		public string NatOper { get; set; }

		public string ConEmbNum { get; set; }

		public string ConEmbTipo { get; set; }

		public DateTime? ConEmbData { get; set; }

		public string TpDocTrans { get; set; }

		public string ViaTrans { get; set; }

		public List<clsOlvBalance> Children { get; set; } = new List<clsOlvBalance>();

		public clsOlvBalance GetClone()
		{
			return (clsOlvBalance)MemberwiseClone();
		}

		object ICloneable.Clone()
		{
			return (clsOlvBalance)MemberwiseClone();
		}
	}

	private class clsModelDoc
	{
		public Document Doc = new Document();

		public DueHeader DueHeader = new DueHeader();

		public ComexDeadLine ComexDeadLine = new ComexDeadLine();

		public List<clsModelDocItem> DocItemList = new List<clsModelDocItem>();

		public int DocItemCount;
	}

	private class clsModelDocItem
	{
		public DocItem DocItem = new DocItem();

		public DueItem DueItem = new DueItem();

		public List<Event790700> AverbList = new List<Event790700>();

		public int AverbListCount;
	}

	private clsSrvTabExpBalance _SqlTabData = new clsSrvTabExpBalance();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsDataDocItem _clsDataDocItem = new clsDataDocItem();

	private clsDataDueHeader _clsDataDueHeader = new clsDataDueHeader();

	private clsDataDueItem _clsDataDueItem = new clsDataDueItem();

	private clsDataDueItemRem _clsDataDueItemRem = new clsDataDueItemRem();

	private clsDataDocEvent790700 _clsDataEvtAver = new clsDataDocEvent790700();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsComexProfile _clsComexProfile;

	private clsDataComexDeadLine _clsDataComexDeadLine = new clsDataComexDeadLine();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private clsComexEmailService _clsEmailService = new clsComexEmailService();

	private bool _ColumnsLoaded;

	private bool _IsLoading;

	private bool _GroupColapsed;

	private bool _IsLpcoControlEnabled;

	private bool _HasFiscalioConnect;

	private string _consMarketScreenUser = string.Empty;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private clsDataFilter _clsDataFilter = new clsDataFilter();

	private Configuration _clsConfig = new Configuration();

	private clsDataEstado _clsDataEstado = new clsDataEstado();

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private TreeListView lsvData;

	private OLVColumn olvDocNum;

	private OLVColumn olvDescript;

	private OLVColumn olvTag;

	private OLVColumn olvDtEmit;

	private OLVColumn olvDtEnd;

	private OLVColumn olvDays;

	private OLVColumn olvProduct;

	private OLVColumn olvUnit;

	private OLVColumn olvQtExport;

	private OLVColumn olvQtDue;

	private OLVColumn olvQtAverb;

	private OLVColumn olvQtDif01;

	private OLVColumn olvQtDif02;

	private OLVColumn olvDocKey;

	private OLVColumn olvNCM;

	private OLVColumn olvCFOP;

	private OLVColumn olvDueItem;

	private OLVColumn olvFilial;

	private OLVColumn olvDataDue;

	private OLVColumn olvDataAverb;

	private OLVColumn olvDtEmbar;

	private OLVColumn olvDueNum;

	private OLVColumn olvDueChave;

	private OLVColumn olvDueStatus;

	private OLVColumn olvHasDossie;

	private OLVColumn olvDueCanal;

	private OLVColumn olvPaisDestName;

	private OLVColumn olvTipoDoc;

	private OLVColumn olvNatOper;

	private OLVColumn olvConEmbNum;

	private OLVColumn olvConEmbTipo;

	private OLVColumn olvConEmbData;

	private OLVColumn olvTpDocTrans;

	private OLVColumn olvViaTrans;

	private ToolStrip tspTaskMenu;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton tsbSuccess;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton tsbWarning;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton tsbError;

	private ToolStripButton tsbSync;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private ToolStripLabel tslUnitType;

	private ToolStripComboBox tscUnitType;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton tsbSpedCreate;

	private ToolStripSeparator toolStripSeparator7;

	private ImageList ImageListDocs;

	private ToolStripButton tsbDocs;

	private ToolStripSeparator tssSepDocs;

	private Button btClose;

	private Panel pnMarketContent;

	private Button btSalesContact;

	private Button btSalesAction;

	private Label label3;

	private Label lbTitle03;

	private PictureBox picWarning;

	private LinkLabel lknAction;

	private Label lbText01;

	private Label lbTitle01;

	private Label lbTitle02;

	private LinkLabel lknClose;

	private Panel pnMarket;

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

	public frmTabExpBalance(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private async void frmTabExpBalance_Load(object sender, EventArgs e)
	{
		_IsLoading = true;
		await _clsDataComexDeadLine.funcResetBufferAsync();
		await _clsDataComexDeadLine.funcLoadBufferListAsync();
		await new clsDataConfig().funcGetItemByKeyAsync();
		_HasFiscalioConnect = await clsScreenGeral.funcMustShowRegFieldsAsync();
		_IsLpcoControlEnabled = await clsScreenGeral.funcHasLpcoControlFeatureAsync();
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
			clsScreenGeral.funcCheckListViewDdic(typeof(clsOlvBalance), lsvData);
		}
		funcDefineListViewFeatures();
		List<clsObjectType> varclsObjList02 = new List<clsObjectType>();
		varclsObjList02.Add(new clsObjectType
		{
			Name = "Quantidade",
			Value = "Quantity"
		});
		varclsObjList02.Add(new clsObjectType
		{
			Name = "Valor R$",
			Value = "Value"
		});
		tscUnitType.ComboBox.ValueMember = "Value";
		tscUnitType.ComboBox.DisplayMember = "Name";
		tscUnitType.ComboBox.DataSource = varclsObjList02;
		string varUnitType = await _clsDataParam.funcGetAsync("TabExpBalance-UnitType");
		if (clsFunction.IsEmpty(varUnitType))
		{
			varUnitType = "Quantity";
		}
		tscUnitType.ComboBox.SelectedValue = varUnitType;
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
		_clsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		_IsLoading = false;
	}

	private void lsvData_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		clsOlvBalance varModel = (clsOlvBalance)e.Model;
		if (varModel == null)
		{
			return;
		}
		_ = string.Empty;
		if (e.ColumnIndex.Equals(olvHasDossie.Index))
		{
			if (!clsFunction.IsEmpty(varModel.HasDossie))
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
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			return (!clsFunction.IsEmpty(clsOlvBalance.HasDossie)) ? ((object)28) : null;
		};
		olvDocNum.ImageGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEqual(clsOlvBalance.RegType, "EXP-DOC-HEAD"))
			{
				if (clsFunction.IsEqual(clsOlvBalance.Status, "SUCCESS"))
				{
					return 8;
				}
				if (clsFunction.IsEqual(clsOlvBalance.Status, "WARNING"))
				{
					return 16;
				}
				if (clsFunction.IsEqual(clsOlvBalance.Status, "ERROR"))
				{
					return 17;
				}
				return (object)null;
			}
			if (clsFunction.IsEqual(clsOlvBalance.RegType, "EXP-DOC-ITEM"))
			{
				if (clsFunction.IsEqual(clsOlvBalance.Status, "SUCCESS"))
				{
					return 8;
				}
				if (clsFunction.IsEqual(clsOlvBalance.Status, "WARNING"))
				{
					return 16;
				}
				if (clsFunction.IsEqual(clsOlvBalance.Status, "ERROR"))
				{
					return 17;
				}
				return (object)null;
			}
			return 18;
		};
		olvTipoDoc.AspectGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			return (clsOlvBalance == null) ? null : clsComexCodes.funcGetTipoDocDesc(clsOlvBalance.TipoDoc);
		};
		olvNatOper.AspectGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			return (clsOlvBalance == null) ? null : clsComexCodes.funcGetNatOperDesc(clsOlvBalance.NatOper);
		};
		olvTpDocTrans.AspectGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			return (clsOlvBalance == null) ? null : clsComexCodes.funcGetTipoDocTransDesc(clsOlvBalance.TpDocTrans);
		};
		olvViaTrans.AspectGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			return (clsOlvBalance == null) ? null : clsComexCodes.funcGetViaTransDesc(clsOlvBalance.ViaTrans);
		};
		olvDueStatus.ImageGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(clsOlvBalance.DueStatus))
			{
				return (object)null;
			}
			if (clsFunction.Contains(clsOlvBalance.DueStatus, "REGIST", pIgnoreCase: true))
			{
				return 23;
			}
			if (clsFunction.Contains(clsOlvBalance.DueStatus, "PENDEN", pIgnoreCase: true))
			{
				return 24;
			}
			if (clsFunction.Contains(clsOlvBalance.DueStatus, "AGUARD", pIgnoreCase: true))
			{
				return 24;
			}
			if (clsFunction.Contains(clsOlvBalance.DueStatus, "PARCIAL", pIgnoreCase: true))
			{
				return 25;
			}
			if (clsFunction.Contains(clsOlvBalance.DueStatus, "AVERBADA", pIgnoreCase: true))
			{
				return 26;
			}
			return clsFunction.Contains(clsOlvBalance.DueStatus, "CANCEL", pIgnoreCase: true) ? ((object)27) : ((object)25);
		};
		olvDueCanal.ImageGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			if (clsFunction.Contains(clsOlvBalance.DueCanal, "Verde", pIgnoreCase: true))
			{
				return 19;
			}
			if (clsFunction.Contains(clsOlvBalance.DueCanal, "Amarelo", pIgnoreCase: true))
			{
				return 20;
			}
			if (clsFunction.Contains(clsOlvBalance.DueCanal, "Laranja", pIgnoreCase: true))
			{
				return 21;
			}
			return clsFunction.Contains(clsOlvBalance.DueCanal, "Vermelho", pIgnoreCase: true) ? ((object)22) : null;
		};
		olvLpcoStat.AspectGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(clsOlvBalance.LpcoStatus))
			{
				return string.Empty;
			}
			return _IsLpcoControlEnabled ? clsOlvBalance.LpcoStatus : "Detalhes";
		};
		olvLpcoStat.ImageGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(clsOlvBalance.LpcoStatus))
			{
				return string.Empty;
			}
			return _IsLpcoControlEnabled ? null : ((object)15);
		};
		lsvData.CanExpandGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return false;
			}
			if (clsOlvBalance.Children == null)
			{
				return false;
			}
			return clsOlvBalance.Children.Count > 0;
		};
		lsvData.ChildrenGetter = (object x) => ((clsOlvBalance)x)?.Children;
		olvTag.ImageGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			return clsFunction.IsEqual(clsOlvBalance.RegType, "EXP-DOC-HEAD") ? ((!clsFunction.IsEmpty(clsOlvBalance.DocNote)) ? ((object)14) : ((object)(-1))) : ((object)(-1));
		};
		olvTag.AspectGetter = delegate(object x)
		{
			clsOlvBalance clsOlvBalance = (clsOlvBalance)x;
			if (clsOlvBalance == null)
			{
				return (object)null;
			}
			return clsFunction.IsEqual(clsOlvBalance.RegType, "EXP-DOC-HEAD") ? (clsFunction.IsEmpty(clsOlvBalance.Tag) ? null : _TagHsList[clsOlvBalance.Tag]) : null;
		};
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter, bool pShow = true)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			if (pShow)
			{
				lbProgress.Text = "Aguarde, obtendo informações do banco de dados...";
				plnMessage.Visible = true;
				Application.DoEvents();
			}
			_clsDataFilter = pclsDataFilter;
			string varSqlQuery = await _SqlTabData.funcGetSqlStrSelectAsync(pclsDataFilter);
			if (string.IsNullOrEmpty(varSqlQuery))
			{
				return varclsReturnFunc;
			}
			lsvData.SuspendLayout();
			long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
			if (_IsLocked)
			{
				varPageSize = 4L;
			}
			List<Document> varclsDocList = await _clsDataDoc.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
			List<string> varDocKeyList = _clsDataDoc.funcGetDocKeyList(varclsDocList);
			List<DocItem> varDocItemList = await _clsDataDocItem.funcGetListByChaveListAsync(varDocKeyList);
			List<DueItem> varDueItemList = await _clsDataDueItem.funcGetListByExpNFeListAsync(varDocKeyList);
			List<Event790700> varAverList = await _clsDataEvtAver.funcGetListByChaveListAsync(varDocKeyList);
			List<DueHeader> varDataDueHeaderList = await _clsDataDueHeader.funcGetItemByDueNumListAsync(varDueItemList, pNotCancel: true);
			List<Estado> varStateList = await _clsDataEstado.funcGetByFilterAsync("", "", 0L, 0L);
			if (pShow)
			{
				lbProgress.Text = "Aguarde, preparando relacionamento das informações...";
				plnMessage.Visible = true;
				Application.DoEvents();
			}
			ConcurrentQueue<clsModelDoc> varclsModeList = new ConcurrentQueue<clsModelDoc>();
			int varParallelThreads = clsFunction.funcConvStrToInt(_clsConfig.ParallelThreads);
			if (!_clsConfig.IsParallel())
			{
				varParallelThreads = 1;
			}
			SemaphoreSlim varSemaphore = new SemaphoreSlim(varParallelThreads);
			await Task.WhenAll(((IEnumerable<Document>)varclsDocList).Select((Func<Document, Task>)async delegate(Document varclsDoc)
			{
				try
				{
					await varSemaphore.WaitAsync();
					clsModelDoc varclsModel = new clsModelDoc
					{
						Doc = varclsDoc,
						DueHeader = new DueHeader()
					};
					clsModelDoc clsModelDoc = varclsModel;
					clsModelDoc.ComexDeadLine = await _clsDataComexDeadLine.funcGetAsync(varclsDoc, "ExpBalance", varStateList);
					varclsModel.DocItemList = new List<clsModelDocItem>();
					int varDocItemPosition = varDocItemList.FindIndex((DocItem x) => clsFunction.IsEqual(x.Chave, varclsDoc.Chave));
					bool varExitLoop = varDocItemPosition < 0;
					bool varDueTriedToSearch = false;
					while (!varExitLoop)
					{
						clsModelDocItem varclsModelItem2 = new clsModelDocItem();
						try
						{
							varclsModelItem2.DocItem = varDocItemList[varDocItemPosition];
							if (!(varclsModelItem2.DocItem.Chave != varclsDoc.Chave))
							{
								varclsModel.DocItemList.Add(varclsModelItem2);
								varclsModel.DocItemCount++;
								varDocItemPosition++;
								goto IL_0235;
							}
						}
						catch
						{
						}
						break;
						IL_0235:
						int varPositionDueItem = varDueItemList.FindIndex((DueItem r) => clsFunction.IsEqual(r.ExpNFe, varclsModelItem2.DocItem.Chave) && clsFunction.IsEqualInt(r.ExpNFeItem, varclsModelItem2.DocItem.nItem));
						if (varPositionDueItem >= 0)
						{
							varclsModelItem2.DueItem = varDueItemList[varPositionDueItem];
						}
						if (!varDueTriedToSearch && varclsModelItem2.DueItem != null)
						{
							int varPositionDueHeader = varDataDueHeaderList.FindIndex((DueHeader r) => clsFunction.IsEqual(r.Num, varclsModelItem2.DueItem.Num));
							if (varPositionDueHeader >= 0)
							{
								varclsModel.DueHeader = varDataDueHeaderList[varPositionDueHeader];
							}
							varDueTriedToSearch = true;
						}
						int varAverbPosition = varAverList.FindIndex((Event790700 r) => clsFunction.IsEqual(r.Chave, varclsModelItem2.DocItem.Chave) && clsFunction.IsEqualInt(r.nItem, varclsModelItem2.DocItem.nItem));
						bool varExitLoopAverb = varAverbPosition < 0;
						while (!varExitLoopAverb)
						{
							try
							{
								Event790700 varAverbItem = varAverList[varAverbPosition];
								if (clsFunction.IsEqual(varAverbItem.Chave, varclsModelItem2.DocItem.Chave) && clsFunction.IsEqualInt(varAverbItem.nItem, varclsModelItem2.DocItem.nItem))
								{
									varclsModelItem2.AverbList.Add(varAverbItem);
									varclsModelItem2.AverbListCount++;
									varAverbPosition++;
									continue;
								}
							}
							catch
							{
							}
							break;
						}
					}
					varclsModeList.Enqueue(varclsModel);
				}
				finally
				{
					varSemaphore.Release();
				}
			}));
			if (pShow)
			{
				lbProgress.Text = "Aguarde, preparando exibição das informações...";
				plnMessage.Visible = true;
				Application.DoEvents();
			}
			_TotalValue = varclsDocList.Sum((Document r) => clsFunction.funcConvStrToDec(r.Valor));
			clsObjectType varUnitType = (clsObjectType)tscUnitType.SelectedItem;
			bool varByQuant = clsFunction.IsEqual(varUnitType.Value, "Quantity");
			funcSetCollumnFormat(varUnitType.Value);
			lsvData.ClearObjects();
			ArrayList varDocList = new ArrayList();
			foreach (clsModelDoc varclsModelItem in varclsModeList)
			{
				Document varclsDocExp = varclsModelItem.Doc;
				ComexDeadLine varDeadLine = varclsModelItem.ComexDeadLine;
				decimal? varDifExpDue = default(decimal);
				decimal? varDifDueAverb = default(decimal);
				clsOlvBalance varOlvExpHeader = new clsOlvBalance();
				string varDocNum = clsFunction.funcGetValue(varclsDocExp.Num).PadLeft(9, '0');
				string varDocSerie = clsFunction.funcGetValue(varclsDocExp.Serie).PadLeft(3, '0');
				varOlvExpHeader.RegType = "EXP-DOC-HEAD";
				varOlvExpHeader.Filial = varclsDocExp.Filial;
				varOlvExpHeader.Chave = varclsDocExp.Chave;
				varOlvExpHeader.DocNum = "NFe " + varDocNum + "-" + varDocSerie;
				varOlvExpHeader.Valor = varclsDocExp.Valor;
				varOlvExpHeader.Descript = varclsDocExp.DestinNome;
				varOlvExpHeader.Tag = varclsDocExp.Tag;
				varOlvExpHeader.Canceled = varclsDocExp.Canceled;
				varOlvExpHeader.HasCancelEvent = varclsDocExp.HasCancelEvent;
				varOlvExpHeader.DocNote = varclsDocExp.DocNote;
				DateTime varDtEmit = clsFunction.funcGetDate(varclsDocExp.DtEmi);
				DateTime varDtEndOf = clsFunction.funcGetDate(varclsDocExp.DtEmi).AddDays(clsFunction.funcConvStrToInt(varDeadLine.DeadLine));
				varOlvExpHeader.DtEmit = varDtEmit;
				varOlvExpHeader.DtEndOf = varDtEndOf;
				double varDtTotDays = DateTime.Now.Subtract(varDtEmit).TotalDays;
				varOlvExpHeader.DtTotDays = clsFunction.funcConvStrToInt(varDeadLine.DeadLine) - Convert.ToInt64(varDtTotDays);
				if (clsFunction.IsEmpty(varclsDocExp.HasXml) && !clsFunction.IsEmpty(varOlvExpHeader))
				{
					varDocList.Add(varOlvExpHeader);
					continue;
				}
				if (!varByQuant)
				{
					varOlvExpHeader.QtExport = default(decimal);
				}
				if (!varByQuant)
				{
					varOlvExpHeader.QtDue = default(decimal);
				}
				if (!varByQuant)
				{
					varOlvExpHeader.QtAverb = default(decimal);
				}
				if (!varByQuant)
				{
					varOlvExpHeader.Unit = "R$";
				}
				varOlvExpHeader.PaisDestName = varclsModelItem.DueHeader?.PaisDestName;
				varOlvExpHeader.TipoDoc = varclsModelItem.DueHeader?.TipoDoc;
				varOlvExpHeader.DataDue = clsFunction.funcGetDate(varclsModelItem.DueHeader?.DataDue, pInCaseOfEmptyRetNull: true);
				varOlvExpHeader.DataAverb = clsFunction.funcGetDate(varclsModelItem.DueHeader?.DataAverb, pInCaseOfEmptyRetNull: true);
				varOlvExpHeader.NatOper = varclsModelItem.DueHeader?.NatOper;
				varOlvExpHeader.ConEmbNum = varclsModelItem.DueHeader?.ConEmbNum;
				varOlvExpHeader.ConEmbTipo = varclsModelItem.DueHeader?.ConEmbTipo;
				varOlvExpHeader.ConEmbData = clsFunction.funcGetDate(varclsModelItem.DueHeader?.ConEmbData, pInCaseOfEmptyRetNull: true);
				varOlvExpHeader.DueNum = varclsModelItem.DueHeader?.Num;
				varOlvExpHeader.DueChave = varclsModelItem.DueHeader?.Chave;
				varOlvExpHeader.DueCanal = varclsModelItem.DueHeader?.Canal;
				varOlvExpHeader.DueStatus = varclsModelItem.DueHeader?.Status;
				varOlvExpHeader.LpcoStatus = varclsModelItem.DueHeader?.LpcoStat;
				varOlvExpHeader.HasDossie = varclsModelItem.DueHeader?.HasDossie;
				varOlvExpHeader.TpDocTrans = varclsModelItem.DueHeader?.TipoDocTrans;
				varOlvExpHeader.ViaTrans = varclsModelItem.DueHeader?.ViaTransCode;
				decimal? qtDif;
				for (int varItemCount = 0; varItemCount < varclsModelItem.DocItemCount; varItemCount++)
				{
					clsModelDocItem varDocItem = varclsModelItem.DocItemList[varItemCount];
					DocItem varDocExpItem = varDocItem.DocItem;
					clsOlvBalance varOlvExpItem = new clsOlvBalance();
					varOlvExpItem.RegType = "EXP-DOC-ITEM";
					varOlvExpItem.Filial = varclsDocExp.Filial;
					varOlvExpItem.Chave = varclsDocExp.Chave;
					varOlvExpItem.DocNum = "Item " + clsFunction.funcGetValue(varDocExpItem.nItem).PadLeft(3, '0');
					varOlvExpItem.Descript = varDocExpItem.xProd;
					varOlvExpItem.ProdCode = varDocExpItem.cProd;
					varOlvExpItem.ProdNcm = varDocExpItem.NCM;
					varOlvExpItem.ProdCfop = varDocExpItem.CFOP;
					varOlvExpItem.QtExport = clsFunction.funcConvStrToDec(varDocExpItem.qTrib);
					if (!varByQuant)
					{
						varOlvExpItem.QtExport *= (decimal?)clsFunction.funcConvStrToDec(varDocExpItem.vUnTrib);
					}
					if (!varByQuant)
					{
						varOlvExpHeader.QtExport += varOlvExpItem.QtExport;
					}
					if (varDocItem.DueItem != null)
					{
						varOlvExpItem.QtDue = clsFunction.funcConvStrToDec(varDocItem.DueItem.ExpNFeItemQuant);
					}
					else
					{
						varOlvExpItem.QtDue = default(decimal);
					}
					if (!varByQuant)
					{
						varOlvExpItem.QtDue *= (decimal?)clsFunction.funcConvStrToDec(varDocExpItem.vUnTrib);
					}
					if (!varByQuant)
					{
						varOlvExpHeader.QtDue += varOlvExpItem.QtDue;
					}
					varOlvExpItem.DueItem = varDocItem.DueItem?.Item;
					varOlvExpItem.ConEmbNum = varOlvExpHeader.ConEmbNum;
					varOlvExpItem.ConEmbTipo = varOlvExpHeader.ConEmbTipo;
					varOlvExpItem.ConEmbData = varOlvExpHeader.ConEmbData;
					varOlvExpItem.DueNum = varOlvExpHeader.DueNum;
					varOlvExpItem.DueChave = varOlvExpHeader.DueChave;
					varOlvExpItem.DueCanal = varOlvExpHeader.DueCanal;
					varOlvExpItem.DueStatus = varOlvExpHeader.DueStatus;
					varOlvExpItem.LpcoStatus = varOlvExpHeader.LpcoStatus;
					varOlvExpItem.HasDossie = varOlvExpHeader.HasDossie;
					varOlvExpItem.TpDocTrans = varOlvExpHeader.TpDocTrans;
					varOlvExpItem.ViaTrans = varOlvExpHeader.ViaTrans;
					varOlvExpItem.QtAverb = default(decimal);
					varOlvExpItem.Unit = varDocExpItem.uTrib;
					if (!varByQuant)
					{
						varOlvExpItem.Unit = "R$";
					}
					for (int varAverbCount = 0; varAverbCount < varDocItem.AverbListCount; varAverbCount++)
					{
						Event790700 varclsAverbItem = varDocItem.AverbList[varAverbCount];
						clsOlvBalance varOlvAverbItem = new clsOlvBalance();
						varOlvAverbItem.RegType = "AVERB-ITEM";
						varOlvAverbItem.Filial = varclsDocExp.Filial;
						varOlvAverbItem.Chave = varclsDocExp.Chave;
						varOlvAverbItem.DocNum = "DUE " + varclsAverbItem.nDue + " Item " + varclsAverbItem.nItemDue;
						varOlvAverbItem.Descript = varDocExpItem.xProd;
						varOlvAverbItem.ProdCode = varDocExpItem.cProd;
						varOlvAverbItem.ProdNcm = varDocExpItem.NCM;
						varOlvAverbItem.ProdCfop = varDocExpItem.CFOP;
						varOlvAverbItem.QtAverb = clsFunction.funcConvStrToDec(varclsAverbItem.qItem);
						if (!varByQuant)
						{
							varOlvAverbItem.QtAverb *= (decimal?)clsFunction.funcConvStrToDec(varDocExpItem.vUnTrib);
						}
						varOlvExpItem.QtAverb += varOlvAverbItem.QtAverb;
						if (!varByQuant)
						{
							varOlvExpHeader.QtAverb += varOlvAverbItem.QtAverb;
						}
						varOlvAverbItem.Unit = varDocExpItem.uTrib;
						if (!varByQuant)
						{
							varOlvAverbItem.Unit = "R$";
						}
						varOlvAverbItem.DtEmbar = clsFunction.funcGetDate(varclsAverbItem.dtEmbarque, pInCaseOfEmptyRetNull: true);
						varOlvAverbItem.DataAverb = clsFunction.funcGetDate(varclsAverbItem.dtAverbacao, pInCaseOfEmptyRetNull: true);
						varOlvAverbItem.DueItem = varclsAverbItem.nItemDue;
						varOlvExpItem.Children.Add(varOlvAverbItem);
					}
					varOlvExpItem.QtDif01 = varOlvExpItem.QtExport - varOlvExpItem.QtDue;
					varDifExpDue += varOlvExpItem.QtDif01;
					varOlvExpItem.QtDif02 = varOlvExpItem.QtDue - varOlvExpItem.QtAverb;
					varDifDueAverb += varOlvExpItem.QtDif02;
					if (varOlvExpItem.Children.Count <= 0)
					{
						varOlvExpItem.Status = "WARNING";
					}
					else
					{
						qtDif = varOlvExpItem.QtDif01;
						if ((qtDif.GetValueOrDefault() == default(decimal)) & qtDif.HasValue)
						{
							varOlvExpItem.Status = "SUCCESS";
						}
						else
						{
							qtDif = varOlvExpItem.QtDif01;
							if ((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue)
							{
								varOlvExpItem.Status = "ERROR";
							}
							else
							{
								qtDif = varOlvExpItem.QtDif01;
								if ((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue)
								{
									varOlvExpItem.Status = "WARNING";
								}
							}
						}
					}
					varOlvExpHeader.Children.Add(varOlvExpItem);
				}
				qtDif = varDifExpDue;
				if ((qtDif.GetValueOrDefault() <= default(decimal)) & qtDif.HasValue)
				{
					qtDif = varDifDueAverb;
					if (((qtDif.GetValueOrDefault() <= default(decimal)) & qtDif.HasValue) && varOlvExpHeader.Children.Count > 0 && varOlvExpHeader.DtTotDays < 0)
					{
						varOlvExpHeader.DtTotDays = 0L;
					}
				}
				if (varOlvExpHeader.Children.Count <= 0)
				{
					varOlvExpHeader.Status = "WARNING";
				}
				else
				{
					if (varOlvExpHeader.DtTotDays <= 0)
					{
						qtDif = varDifDueAverb;
						if ((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue)
						{
							varOlvExpHeader.Status = "ERROR";
							goto IL_1405;
						}
					}
					if (varOlvExpHeader.DtTotDays <= 0)
					{
						qtDif = varDifExpDue;
						if ((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue)
						{
							varOlvExpHeader.Status = "ERROR";
							goto IL_1405;
						}
					}
					if (varOlvExpHeader.DtTotDays <= clsFunction.funcConvStrToInt(varDeadLine.Warning))
					{
						qtDif = varDifDueAverb;
						if ((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue)
						{
							varOlvExpHeader.Status = "WARNING";
							goto IL_1405;
						}
					}
					if (varOlvExpHeader.DtTotDays <= clsFunction.funcConvStrToInt(varDeadLine.Warning))
					{
						qtDif = varDifExpDue;
						if ((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue)
						{
							varOlvExpHeader.Status = "WARNING";
							goto IL_1405;
						}
					}
					qtDif = varDifExpDue;
					if (!((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue))
					{
						qtDif = varDifDueAverb;
						if (!((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue))
						{
							qtDif = varDifExpDue;
							if (!((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue))
							{
								qtDif = varDifDueAverb;
								if (!((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue))
								{
									qtDif = varDifDueAverb;
									if ((qtDif.GetValueOrDefault() == default(decimal)) & qtDif.HasValue)
									{
										qtDif = varDifExpDue;
										if ((qtDif.GetValueOrDefault() == default(decimal)) & qtDif.HasValue)
										{
											varOlvExpHeader.Status = "SUCCESS";
										}
									}
									goto IL_1405;
								}
							}
							varOlvExpHeader.Status = "WARNING";
							goto IL_1405;
						}
					}
					varOlvExpHeader.Status = "ERROR";
				}
				goto IL_1405;
				IL_1405:
				varDocList.Add(varOlvExpHeader);
			}
			lsvData.Roots = varDocList;
			funcRebuildFilters();
			_GroupColapsed = false;
			lsvData.ResumeLayout();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			if (pShow)
			{
				lbProgress.Text = string.Empty;
				plnMessage.Visible = false;
				Application.DoEvents();
			}
		}
		return varclsReturnFunc;
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
		List<clsOlvBalance> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<clsOlvBalance> varObjList = funcGetObjList(pFocused, pChecked);
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
		List<clsOlvBalance> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<clsOlvBalance> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
					if (column.Text == "Número")
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

	public async Task<clsReturn> funcExportToExcelAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			lbProgress.Text = "Exportando dados para o Excel...";
			plnMessage.Visible = true;
			Application.DoEvents();
			varclsReturnFunc = await clsScreenGeral.funcExportDocTreeToExcelAsync<clsOlvBalance>(lsvData, 2);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		finally
		{
			lbProgress.Text = string.Empty;
			plnMessage.Visible = false;
			Application.DoEvents();
		}
		return varclsReturnFunc;
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		new clsDataDoc();
		List<clsOlvBalance> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<clsOlvBalance> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (clsOlvBalance varObject in pObjList)
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

	private List<clsOlvBalance> funcGetObjList(bool pFocused, bool pChecked, bool pOnlyPend = false)
	{
		List<clsOlvBalance> varObjList = new List<clsOlvBalance>();
		clsOlvBalance varFocused = new clsOlvBalance();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (clsOlvBalance)lsvData.FocusedObject;
				varObjList.Add(varFocused);
			}
			catch
			{
				varFocused = new clsOlvBalance();
			}
		}
		if (pChecked)
		{
			foreach (clsOlvBalance varObject in lsvData.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varObjList.Add(varObject);
				}
			}
			if (varObjList.Count > 0)
			{
				varObjList = lsvData.Objects.Cast<clsOlvBalance>().Intersect(varObjList).ToList();
			}
		}
		if ((!pFocused && !pChecked) || varObjList.Count <= 0)
		{
			varObjList = lsvData.FilteredObjects.Cast<clsOlvBalance>().ToList();
			if (varObjList.Count <= 0)
			{
				varObjList = lsvData.Objects.Cast<clsOlvBalance>().ToList();
			}
		}
		if (pOnlyPend && !clsFunction.IsAdmin)
		{
			varObjList.RemoveAll((clsOlvBalance r) => clsFunction.Contains(r.Status, "SUCCESS", "AVERBADA", pIgnoreCase: true));
		}
		return varObjList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<clsOlvBalance> varObjList = funcGetObjList(pFocused, pChecked);
		funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
		return true;
	}

	private async void funcRefreshListAsync(List<Document> pDocList, List<clsOlvBalance> pObjList)
	{
		ConcurrentBag<ObjFieldBuffer> varDocFieldList = clsObjectBuffer.funcGet<Document>().Fields;
		ConcurrentBag<ObjFieldBuffer> varObjFieldList = clsObjectBuffer.funcGet<clsOlvBalance>().Fields;
		await Task.WhenAll(((IEnumerable<clsOlvBalance>)pObjList).Select((Func<clsOlvBalance, Task>)async delegate(clsOlvBalance varclsObjItem)
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
		if (e.ColumnIndex == olvDays.Index)
		{
			clsOlvBalance varModel = (clsOlvBalance)e.Model;
			if (varModel != null && clsFunction.IsEqual(varModel.RegType, "EXP-DOC-HEAD"))
			{
				if (varModel.DtTotDays < 0 && clsFunction.IsEqual(varModel.Status, "ERROR"))
				{
					e.SubItem.BackColor = Color.OrangeRed;
				}
				else if (varModel.DtTotDays <= 20 && clsFunction.IsEqual(varModel.Status, "WARNING"))
				{
					e.SubItem.BackColor = Color.Yellow;
				}
			}
		}
		else if (e.ColumnIndex == olvTag.Index)
		{
			clsOlvBalance varDocument = (clsOlvBalance)e.Model;
			if (varDocument != null && !clsFunction.IsEmpty(varDocument.Tag))
			{
				string varColorName = (string)_HasColors[varDocument.Tag];
				e.SubItem.BackColor = ColorTranslator.FromHtml(varColorName);
			}
		}
	}

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		clsOlvBalance varDocument = (clsOlvBalance)e.Model;
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
		clsOlvBalance varclsObject = (clsOlvBalance)e.Model;
		if (varclsObject == null)
		{
			return;
		}
		if (e.ColumnIndex == olvDocNum.Index)
		{
			Document varclsDoc = await _clsDataDoc.funcGetItemByKeyAsync(varclsObject.Filial, varclsObject.Chave);
			if (varclsDoc != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varclsDoc);
			}
		}
		else if (e.ColumnIndex == olvDueNum.Index)
		{
			funcShowDocViewerAsync(varclsObject, pShowPDF: true, pShowXML: true);
		}
		else if (e.ColumnIndex == olvDueStatus.Index)
		{
			_clsEmailService.funcCallDueLink(varclsObject.DueNum);
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
				_clsEmailService.funcCallLpcoLinkAsync(varclsObject.DueNum);
			}
		}
	}

	private async void funcShowDocViewerAsync(clsOlvBalance pclsObject, bool pShowPDF, bool pShowXML)
	{
		DueHeader varclsDueHeader = await _clsDataDueHeader.funcGetItemByKeyAsync(pclsObject.Filial, pclsObject.DueNum);
		if (varclsDueHeader != null)
		{
			await clsMonGeral.funcDocViewerAsync(this, await funcGetDocListByDueHeaderAsync(varclsDueHeader), pShowPDF, pShowXML);
		}
	}

	private async Task<List<Document>> funcGetDocListByDueHeaderAsync(DueHeader pclsDueHeader)
	{
		List<Document> varDocList = new List<Document>();
		foreach (DueItem varclsDueItem in await _clsDataDueItem.funcGetListByKeyAsync(pclsDueHeader.Filial, pclsDueHeader.Num))
		{
			Document varclsDoc = new Document
			{
				Filial = varclsDueItem.Filial,
				Chave = varclsDueItem.ExpNFe
			};
			varDocList.Add(varclsDoc);
		}
		foreach (DueItemRem varclsDueItemRem in await _clsDataDueItemRem.funcGetListByKeyAsync(pclsDueHeader.Filial, pclsDueHeader.Num))
		{
			Document varclsDoc2 = new Document
			{
				Filial = varclsDueItemRem.Filial,
				Chave = varclsDueItemRem.RemNFe
			};
			varDocList.Add(varclsDoc2);
		}
		return varDocList;
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
			lsvData.CollapseAll();
		}
		else
		{
			_GroupColapsed = true;
			lsvData.ExpandAll();
		}
	}

	private void tsbSuccess_Click(object sender, EventArgs e)
	{
		tsbSuccess.Checked = !tsbSuccess.Checked;
		funcRebuildFilters();
	}

	private void tsbWarning_Click(object sender, EventArgs e)
	{
		tsbWarning.Checked = !tsbWarning.Checked;
		funcRebuildFilters();
	}

	private void tsbError_Click(object sender, EventArgs e)
	{
		tsbError.Checked = !tsbError.Checked;
		funcRebuildFilters();
	}

	private void funcRebuildFilters()
	{
		List<IModelFilter> varFilters = new List<IModelFilter>();
		if (tsbSuccess.Checked && tsbWarning.Checked && tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "SUCCESS", "WARNING", "ERROR", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		else if (tsbSuccess.Checked && tsbWarning.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "SUCCESS", "WARNING", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		else if (tsbWarning.Checked && tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "WARNING", "ERROR", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		else if (tsbSuccess.Checked && tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "SUCCESS", "ERROR", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		else if (tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "ERROR", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		else if (tsbWarning.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "WARNING", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		else if (tsbSuccess.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvBalance)model).Status, "SUCCESS", "") || !clsFunction.IsEqual(((clsOlvBalance)model).RegType, "EXP-DOC-HEAD")));
		}
		lsvData.AdditionalFilter = ((varFilters.Count == 0) ? null : new CompositeAllFilter(varFilters));
	}

	private async void tsbSync_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsComexApi varclsComexApi = new clsComexApi();
		try
		{
			plnMessage.Visible = true;
			Application.DoEvents();
			tsbSync.Enabled = false;
			tsbSync.Text = "Sincronizando...";
			lbProgress.Text = "Definindo NFes para Sincronização de DUE ...";
			Application.DoEvents();
			List<clsOlvBalance> varObjectList = funcGetObjList(pFocused: false, pChecked: true, pOnlyPend: true);
			List<clsComexApi.clsDueObject> varclsDbaDueList = new List<clsComexApi.clsDueObject>();
			int varCounter01 = 0;
			int varTotal01 = varObjectList.Count;
			FilialView varclsFilial = new FilialView();
			foreach (clsOlvBalance varclsItem in varObjectList)
			{
				varCounter01++;
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
				lbProgress.Text = $"[ {varCounter01} de {varTotal01} ] Procurando DUEs relacionadas a {varclsItem.DocNum}...";
				Application.DoEvents();
				clsReturn varclsRetItem = await varclsComexApi.funcGetDataByDocKeyLinkAsync(varclsFilial, varclsItem.Chave);
				if (varclsRetItem.UserCancel)
				{
					varclsReturnFunc.AddRange(varclsRetItem);
					break;
				}
				if (varclsRetItem.HasError)
				{
					varclsReturnFunc.AddRange(varclsRetItem);
					continue;
				}
				List<clsComexApi.clsDueObject> varclsObject = varclsRetItem.GetObject<List<clsComexApi.clsDueObject>>("Object");
				if (varclsObject != null)
				{
					varclsDbaDueList.AddRange(varclsObject);
				}
			}
			if (varclsDbaDueList.Count <= 0)
			{
				return;
			}
			List<clsComexApi.clsDueObject> varclsExcDueList = new List<clsComexApi.clsDueObject>();
			foreach (clsComexApi.clsDueObject varclsDbaDueItem in varclsDbaDueList)
			{
				if (!varclsExcDueList.Any((clsComexApi.clsDueObject r) => clsFunction.IsEqual(r.DueNum, varclsDbaDueItem.DueNum)))
				{
					varclsExcDueList.Add(varclsDbaDueItem);
				}
			}
			int varCounter2 = 0;
			int varTotal2 = varclsExcDueList.Count;
			foreach (clsComexApi.clsDueObject varclsItem2 in varclsExcDueList)
			{
				varCounter2++;
				lbProgress.Text = $"[ {varCounter2} de {varTotal2} ] Sincronizando dados da DUE {varclsItem2.DueNum}...";
				Application.DoEvents();
				clsReturn varclsRetItem2 = await varclsComexApi.funcGetDataByDueKeyLinkAsync(varclsItem2.Filial, varclsItem2.DueNum, pByTaskAction: false, pGetDossie: false);
				if (varclsRetItem2.HasError)
				{
					varclsReturnFunc.AddRange(varclsRetItem2);
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

	private async void tscUnitType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (_IsLoading)
		{
			return;
		}
		string varSelected = clsFunction.funcGetValue(((clsObjectType)tscUnitType.SelectedItem).Value);
		await _clsDataParam.funcSetAsync("TabExpBalance-UnitType", varSelected);
		try
		{
			lbProgress.Text = "Recarregando os dados...";
			plnMessage.Visible = true;
			Application.DoEvents();
			await funcLoadDataAsync(_clsDataFilter, pShow: false);
		}
		finally
		{
			lbProgress.Text = string.Empty;
			plnMessage.Visible = false;
			Application.DoEvents();
		}
	}

	public void funcSetCollumnFormat(string pUnitType)
	{
		if (clsFunction.IsEqual(pUnitType, "Quantity"))
		{
			OLVColumn oLVColumn = olvQtExport;
			OLVColumn oLVColumn2 = olvQtDue;
			string text = (olvQtAverb.AspectToStringFormat = string.Empty);
			string aspectToStringFormat = (oLVColumn2.AspectToStringFormat = text);
			oLVColumn.AspectToStringFormat = aspectToStringFormat;
			OLVColumn oLVColumn3 = olvQtDif01;
			aspectToStringFormat = (olvQtDif02.AspectToStringFormat = string.Empty);
			oLVColumn3.AspectToStringFormat = aspectToStringFormat;
			olvQtExport.Text = "QtdExport";
			olvQtDue.Text = "QtdDue";
			olvQtAverb.Text = "QtdAverbação";
		}
		else
		{
			OLVColumn oLVColumn4 = olvQtExport;
			OLVColumn oLVColumn5 = olvQtDue;
			string text = (olvQtAverb.AspectToStringFormat = "{0:0,0.00}");
			string aspectToStringFormat = (oLVColumn5.AspectToStringFormat = text);
			oLVColumn4.AspectToStringFormat = aspectToStringFormat;
			OLVColumn oLVColumn6 = olvQtDif01;
			aspectToStringFormat = (olvQtDif02.AspectToStringFormat = "{0:0,0.00}");
			oLVColumn6.AspectToStringFormat = aspectToStringFormat;
			olvQtExport.Text = "ValorExportação";
			olvQtDue.Text = "ValorDue";
			olvQtAverb.Text = "ValorAverbação";
		}
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
		clsHelpService.funcCallTipReportExportGeneralAsync();
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private void lsvData_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		funcSetTabDataTotalValues();
	}

	private void funcSetTabDataTotalValues()
	{
		IEnumerable varEnumList = null;
		TreeListView treeListView = lsvData;
		varEnumList = ((treeListView == null || !(treeListView.CheckedObjects?.Count > 0)) ? lsvData.FilteredObjects : lsvData.CheckedObjectsEnumerable);
		List<clsOlvBalance> varObjList = varEnumList.Cast<clsOlvBalance>().ToList();
		if (varObjList == null)
		{
			varObjList = new List<clsOlvBalance>();
		}
		_TotalValue = varObjList.Sum((clsOlvBalance r) => clsFunction.funcConvStrToDec(r.Valor));
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.TotalValue = _TotalValue;
		varArguments.TotalQuant = varObjList.Count;
		OnEventTabManager(varArguments);
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private clsComexApi.clsDueObject funcGetDueObjItem(clsOlvBalance pclsObject)
	{
		clsComexApi.clsDueObject varDueObjItem = null;
		if (!clsFunction.IsEmpty(pclsObject.DueNum))
		{
			return new clsComexApi.clsDueObject
			{
				Filial = new FilialView
				{
					CNPJ = pclsObject.Filial
				},
				DueNum = pclsObject.DueNum
			};
		}
		if (pclsObject.Children == null)
		{
			return varDueObjItem;
		}
		foreach (clsOlvBalance varclsItem in pclsObject.Children)
		{
			varDueObjItem = funcGetDueObjItem(varclsItem);
			if (varDueObjItem != null)
			{
				return varDueObjItem;
			}
		}
		return varDueObjItem;
	}

	private List<clsComexApi.clsDueObject> funcGetDueObjList()
	{
		List<clsOlvBalance> list = funcGetObjList(pFocused: true, pChecked: true);
		List<clsComexApi.clsDueObject> varDueObjList = new List<clsComexApi.clsDueObject>();
		foreach (clsOlvBalance varclsItem in list)
		{
			clsComexApi.clsDueObject varDueObjItem = funcGetDueObjItem(varclsItem);
			if (varDueObjItem != null)
			{
				varDueObjList.Add(varDueObjItem);
			}
		}
		return varDueObjList;
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
		List<clsComexApi.clsDueObject> varObjList = funcGetDueObjList();
		List<DueHeader> varclsExcDueList = new List<DueHeader>();
		foreach (clsComexApi.clsDueObject varclsObjItem in varObjList)
		{
			if (!varclsExcDueList.Any((DueHeader r) => clsFunction.IsEqual(r.Num, varclsObjItem.DueNum)))
			{
				string varFilialCnpj = varclsObjItem.Filial.CNPJ;
				DueHeader varclsDueHeader = await _clsDataDueHeader.funcGetItemByKeyAsync(varFilialCnpj, varclsObjItem.DueNum);
				if (varclsDueHeader != null)
				{
					varclsExcDueList.Add(varclsDueHeader);
				}
			}
		}
		using frmDueExport varFrmDueExport = new frmDueExport(varclsExcDueList);
		varFrmDueExport.ShowDialog(this);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabExpBalance));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lsvData = new BrightIdeasSoftware.TreeListView();
		this.olvDocNum = new BrightIdeasSoftware.OLVColumn();
		this.olvDescript = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmit = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEnd = new BrightIdeasSoftware.OLVColumn();
		this.olvDays = new BrightIdeasSoftware.OLVColumn();
		this.olvProduct = new BrightIdeasSoftware.OLVColumn();
		this.olvNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvUnit = new BrightIdeasSoftware.OLVColumn();
		this.olvQtExport = new BrightIdeasSoftware.OLVColumn();
		this.olvQtDue = new BrightIdeasSoftware.OLVColumn();
		this.olvQtAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvQtDif01 = new BrightIdeasSoftware.OLVColumn();
		this.olvQtDif02 = new BrightIdeasSoftware.OLVColumn();
		this.olvDueItem = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvDataDue = new BrightIdeasSoftware.OLVColumn();
		this.olvDataAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmbar = new BrightIdeasSoftware.OLVColumn();
		this.olvDueNum = new BrightIdeasSoftware.OLVColumn();
		this.olvDueChave = new BrightIdeasSoftware.OLVColumn();
		this.olvDueStatus = new BrightIdeasSoftware.OLVColumn();
		this.olvLpcoStat = new BrightIdeasSoftware.OLVColumn();
		this.olvHasDossie = new BrightIdeasSoftware.OLVColumn();
		this.olvDueCanal = new BrightIdeasSoftware.OLVColumn();
		this.olvPaisDestName = new BrightIdeasSoftware.OLVColumn();
		this.olvTipoDoc = new BrightIdeasSoftware.OLVColumn();
		this.olvNatOper = new BrightIdeasSoftware.OLVColumn();
		this.olvConEmbNum = new BrightIdeasSoftware.OLVColumn();
		this.olvConEmbTipo = new BrightIdeasSoftware.OLVColumn();
		this.olvConEmbData = new BrightIdeasSoftware.OLVColumn();
		this.olvTpDocTrans = new BrightIdeasSoftware.OLVColumn();
		this.olvViaTrans = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.tspTaskMenu = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSuccess = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbWarning = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbError = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.tslUnitType = new System.Windows.Forms.ToolStripLabel();
		this.tscUnitType = new System.Windows.Forms.ToolStripComboBox();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSync = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDocs = new System.Windows.Forms.ToolStripButton();
		this.tssSepDocs = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSpedCreate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.btClose = new System.Windows.Forms.Button();
		this.pnMarketContent = new System.Windows.Forms.Panel();
		this.btSalesContact = new System.Windows.Forms.Button();
		this.btSalesAction = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.picWarning = new System.Windows.Forms.PictureBox();
		this.lknAction = new System.Windows.Forms.LinkLabel();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.pnMarket = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.tspTaskMenu.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		this.pnMarket.SuspendLayout();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.lsvData.AllColumns.Add(this.olvDocNum);
		this.lsvData.AllColumns.Add(this.olvDescript);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvDtEmit);
		this.lsvData.AllColumns.Add(this.olvDtEnd);
		this.lsvData.AllColumns.Add(this.olvDays);
		this.lsvData.AllColumns.Add(this.olvProduct);
		this.lsvData.AllColumns.Add(this.olvNCM);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvUnit);
		this.lsvData.AllColumns.Add(this.olvQtExport);
		this.lsvData.AllColumns.Add(this.olvQtDue);
		this.lsvData.AllColumns.Add(this.olvQtAverb);
		this.lsvData.AllColumns.Add(this.olvQtDif01);
		this.lsvData.AllColumns.Add(this.olvQtDif02);
		this.lsvData.AllColumns.Add(this.olvDueItem);
		this.lsvData.AllColumns.Add(this.olvFilial);
		this.lsvData.AllColumns.Add(this.olvDataDue);
		this.lsvData.AllColumns.Add(this.olvDataAverb);
		this.lsvData.AllColumns.Add(this.olvDtEmbar);
		this.lsvData.AllColumns.Add(this.olvDueNum);
		this.lsvData.AllColumns.Add(this.olvDueChave);
		this.lsvData.AllColumns.Add(this.olvDueStatus);
		this.lsvData.AllColumns.Add(this.olvLpcoStat);
		this.lsvData.AllColumns.Add(this.olvHasDossie);
		this.lsvData.AllColumns.Add(this.olvDueCanal);
		this.lsvData.AllColumns.Add(this.olvPaisDestName);
		this.lsvData.AllColumns.Add(this.olvTipoDoc);
		this.lsvData.AllColumns.Add(this.olvNatOper);
		this.lsvData.AllColumns.Add(this.olvConEmbNum);
		this.lsvData.AllColumns.Add(this.olvConEmbTipo);
		this.lsvData.AllColumns.Add(this.olvConEmbData);
		this.lsvData.AllColumns.Add(this.olvTpDocTrans);
		this.lsvData.AllColumns.Add(this.olvViaTrans);
		this.lsvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvData.CellEditUseWholeCell = false;
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[34]
		{
			this.olvDocNum, this.olvDescript, this.olvTag, this.olvDtEmit, this.olvDtEnd, this.olvDays, this.olvProduct, this.olvNCM, this.olvCFOP, this.olvUnit,
			this.olvQtExport, this.olvQtDue, this.olvQtAverb, this.olvQtDif01, this.olvQtDif02, this.olvDueItem, this.olvFilial, this.olvDataDue, this.olvDataAverb, this.olvDtEmbar,
			this.olvDueNum, this.olvDueChave, this.olvDueStatus, this.olvLpcoStat, this.olvHasDossie, this.olvDueCanal, this.olvPaisDestName, this.olvTipoDoc, this.olvNatOper, this.olvConEmbNum,
			this.olvConEmbTipo, this.olvConEmbData, this.olvTpDocTrans, this.olvViaTrans
		});
		this.lsvData.ContextMenuStrip = this.contextMenuDocs;
		this.lsvData.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvData.EmptyListMsg = "";
		this.lsvData.FullRowSelect = true;
		this.lsvData.GridLines = true;
		this.lsvData.HideSelection = false;
		this.lsvData.Location = new System.Drawing.Point(0, 27);
		this.lsvData.Margin = new System.Windows.Forms.Padding(6);
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
		this.lsvData.ShowGroups = false;
		this.lsvData.ShowImagesOnSubItems = true;
		this.lsvData.Size = new System.Drawing.Size(900, 116);
		this.lsvData.SmallImageList = this.ImageListDocs;
		this.lsvData.TabIndex = 30;
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
		this.lsvData.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(lsvData_HyperlinkClicked);
		this.lsvData.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(lsvData_ItemsChanged);
		this.lsvData.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lsvData_ColumnWidthChanged);
		this.lsvData.DoubleClick += new System.EventHandler(lsvData_DoubleClick);
		this.lsvData.KeyUp += new System.Windows.Forms.KeyEventHandler(lsvData_KeyUp);
		this.olvDocNum.AspectName = "DocNum";
		this.olvDocNum.Hyperlink = true;
		this.olvDocNum.Text = "Número";
		this.olvDocNum.Width = 231;
		this.olvDescript.AspectName = "Descript";
		this.olvDescript.Text = "Descrição";
		this.olvDescript.Width = 164;
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.Width = 40;
		this.olvDtEmit.AspectName = "DtEmit";
		this.olvDtEmit.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvDtEmit.Text = "Emissão";
		this.olvDtEmit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmit.Width = 80;
		this.olvDtEnd.AspectName = "DtEndOf";
		this.olvDtEnd.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvDtEnd.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEnd.Text = "Prazo";
		this.olvDtEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEnd.Width = 80;
		this.olvDays.AspectName = "DtTotDays";
		this.olvDays.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDays.Text = "Dias";
		this.olvDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvDays.Width = 54;
		this.olvProduct.AspectName = "ProdCode";
		this.olvProduct.Text = "Produto";
		this.olvProduct.Width = 140;
		this.olvNCM.AspectName = "ProdNcm";
		this.olvNCM.Text = "NCM";
		this.olvCFOP.AspectName = "ProdCfop";
		this.olvCFOP.Text = "CFOP";
		this.olvCFOP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUnit.AspectName = "Unit";
		this.olvUnit.Text = "Un";
		this.olvUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvUnit.Width = 40;
		this.olvQtExport.AspectName = "QtExport";
		this.olvQtExport.Text = "Qt.Export.";
		this.olvQtExport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtExport.Width = 85;
		this.olvQtDue.AspectName = "QtDue";
		this.olvQtDue.Text = "Qt.Due";
		this.olvQtDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtDue.Width = 85;
		this.olvQtAverb.AspectName = "QtAverb";
		this.olvQtAverb.Text = "Qt.Averb.";
		this.olvQtAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtAverb.Width = 85;
		this.olvQtDif01.AspectName = "QtDif01";
		this.olvQtDif01.Text = "[Export-Due]";
		this.olvQtDif01.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtDif01.Width = 100;
		this.olvQtDif02.AspectName = "QtDif02";
		this.olvQtDif02.Text = "[Due-Averb]";
		this.olvQtDif02.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtDif02.Width = 101;
		this.olvDueItem.AspectName = "DueItem";
		this.olvDueItem.Text = "DueItem";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvFilial.Text = "Filial";
		this.olvFilial.ToolTipText = "Filial";
		this.olvFilial.Width = 115;
		this.olvDataDue.AspectName = "DataDue";
		this.olvDataDue.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvDataDue.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataDue.Text = "DtDue";
		this.olvDataDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataDue.ToolTipText = "Data da Declaração de Exportação";
		this.olvDataDue.Width = 78;
		this.olvDataAverb.AspectName = "DataAverb";
		this.olvDataAverb.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvDataAverb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataAverb.Text = "Dt.Averbação";
		this.olvDataAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDataAverb.Width = 80;
		this.olvDtEmbar.AspectName = "DtEmbar";
		this.olvDtEmbar.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvDtEmbar.Text = "Dt.Embarque";
		this.olvDtEmbar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmbar.Width = 80;
		this.olvDueNum.AspectName = "DueNum";
		this.olvDueNum.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDueNum.Hyperlink = true;
		this.olvDueNum.Text = "DUE";
		this.olvDueNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDueNum.ToolTipText = "Número da DUE";
		this.olvDueNum.Width = 110;
		this.olvDueChave.AspectName = "DueChave";
		this.olvDueChave.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDueChave.Text = "DueChave";
		this.olvDueChave.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDueChave.ToolTipText = "Chave da DUE";
		this.olvDueChave.Width = 110;
		this.olvDueStatus.AspectName = "DueStatus";
		this.olvDueStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvDueStatus.Hyperlink = true;
		this.olvDueStatus.Text = "Status";
		this.olvDueStatus.ToolTipText = "Status da Exportação";
		this.olvDueStatus.Width = 74;
		this.olvLpcoStat.AspectName = "LpcoStatus";
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
		this.olvDueCanal.AspectName = "DueCanal";
		this.olvDueCanal.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDueCanal.Text = "Canal";
		this.olvDueCanal.ToolTipText = "Canal de Exportação";
		this.olvDueCanal.Width = 74;
		this.olvPaisDestName.AspectName = "PaisDestName";
		this.olvPaisDestName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvPaisDestName.Text = "Pais Destino";
		this.olvPaisDestName.ToolTipText = "Nome do País de Destino";
		this.olvPaisDestName.Width = 100;
		this.olvTipoDoc.AspectName = "TipoDoc";
		this.olvTipoDoc.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.olvTipoDoc.Text = "TipoDoc";
		this.olvTipoDoc.ToolTipText = "Tipo do Documento";
		this.olvTipoDoc.Width = 90;
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
		this.olvConEmbData.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvConEmbData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvConEmbData.Text = "ConhecData";
		this.olvConEmbData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvConEmbData.ToolTipText = "Data do Conhecimento de Embarque";
		this.olvConEmbData.Width = 74;
		this.olvTpDocTrans.AspectName = "TpDocTrans";
		this.olvTpDocTrans.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTpDocTrans.Text = "Tipo Doc Transporte";
		this.olvTpDocTrans.ToolTipText = "Tipo de Documento de Transporte";
		this.olvTpDocTrans.Width = 70;
		this.olvViaTrans.AspectName = "ViaTrans";
		this.olvViaTrans.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvViaTrans.Text = "Via Transporte";
		this.olvViaTrans.ToolTipText = "Via de Transporte";
		this.olvViaTrans.Width = 70;
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
		this.ImageListDocs.Images.SetKeyName(16, "image_tool_disabled.png");
		this.ImageListDocs.Images.SetKeyName(17, "image_error.png");
		this.ImageListDocs.Images.SetKeyName(18, "image_premium.png");
		this.ImageListDocs.Images.SetKeyName(19, "image_channel_green.png");
		this.ImageListDocs.Images.SetKeyName(20, "image_channel_yellow.png");
		this.ImageListDocs.Images.SetKeyName(21, "image_channel_orange.png");
		this.ImageListDocs.Images.SetKeyName(22, "image_channel_red.png");
		this.ImageListDocs.Images.SetKeyName(23, "image_status_initial.png");
		this.ImageListDocs.Images.SetKeyName(24, "image_status_pendent.png");
		this.ImageListDocs.Images.SetKeyName(25, "image_status_running.png");
		this.ImageListDocs.Images.SetKeyName(26, "image_status_finished.png");
		this.ImageListDocs.Images.SetKeyName(27, "dfe_canceled.png");
		this.ImageListDocs.Images.SetKeyName(28, "image_source_folder.png");
		this.tspTaskMenu.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspTaskMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.toolStripSeparator3, this.tsbSuccess, this.toolStripSeparator6, this.tsbWarning, this.toolStripSeparator4, this.tsbError, this.toolStripSeparator5, this.tslUnitType, this.tscUnitType, this.toolStripSeparator1,
			this.tsbSync, this.toolStripSeparator2, this.tsbDocs, this.tssSepDocs, this.tsbSpedCreate, this.toolStripSeparator7
		});
		this.tspTaskMenu.Location = new System.Drawing.Point(0, 0);
		this.tspTaskMenu.Name = "tspTaskMenu";
		this.tspTaskMenu.Padding = new System.Windows.Forms.Padding(2);
		this.tspTaskMenu.Size = new System.Drawing.Size(900, 27);
		this.tspTaskMenu.TabIndex = 41;
		this.tspTaskMenu.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
		this.tsbSuccess.Image = Monitor.Resources.dfe_confirm;
		this.tsbSuccess.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSuccess.Name = "tsbSuccess";
		this.tsbSuccess.Size = new System.Drawing.Size(79, 20);
		this.tsbSuccess.Text = "Averbados";
		this.tsbSuccess.Click += new System.EventHandler(tsbSuccess_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
		this.tsbWarning.Image = Monitor.Resources.image_warning;
		this.tsbWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbWarning.Name = "tsbWarning";
		this.tsbWarning.Size = new System.Drawing.Size(73, 20);
		this.tsbWarning.Text = "Pendente";
		this.tsbWarning.Click += new System.EventHandler(tsbWarning_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
		this.tsbError.Image = Monitor.Resources.image_error;
		this.tsbError.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbError.Name = "tsbError";
		this.tsbError.Size = new System.Drawing.Size(76, 20);
		this.tsbError.Text = "Problemas";
		this.tsbError.Click += new System.EventHandler(tsbError_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
		this.tslUnitType.Name = "tslUnitType";
		this.tslUnitType.Size = new System.Drawing.Size(53, 20);
		this.tslUnitType.Text = "Unidade :";
		this.tscUnitType.BackColor = System.Drawing.SystemColors.Info;
		this.tscUnitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.tscUnitType.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.tscUnitType.Items.AddRange(new object[2] { "Quantidade", "Valor (R$)" });
		this.tscUnitType.Name = "tscUnitType";
		this.tscUnitType.Size = new System.Drawing.Size(120, 23);
		this.tscUnitType.SelectedIndexChanged += new System.EventHandler(tscUnitType_SelectedIndexChanged);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
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
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 443);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(966, 38);
		this.plnMessage.TabIndex = 106;
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
		this.pnMarketContent.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pnMarketContent.Controls.Add(this.btSalesContact);
		this.pnMarketContent.Controls.Add(this.btSalesAction);
		this.pnMarketContent.Controls.Add(this.label3);
		this.pnMarketContent.Controls.Add(this.lbTitle03);
		this.pnMarketContent.Controls.Add(this.picWarning);
		this.pnMarketContent.Controls.Add(this.lknAction);
		this.pnMarketContent.Controls.Add(this.lbText01);
		this.pnMarketContent.Controls.Add(this.lbTitle01);
		this.pnMarketContent.Controls.Add(this.lbTitle02);
		this.pnMarketContent.Location = new System.Drawing.Point(173, 32);
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
		this.lbTitle03.Location = new System.Drawing.Point(17, 174);
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
		this.lknAction.Location = new System.Drawing.Point(111, 174);
		this.lknAction.Name = "lknAction";
		this.lknAction.Size = new System.Drawing.Size(436, 16);
		this.lknAction.TabIndex = 203;
		this.lknAction.TabStop = true;
		this.lknAction.Text = "Exportação Geral: Saldos e Prazos";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(17, 81);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 71);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = resources.GetString("lbText01.Text");
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Exportação Geral: Saldos e Prazos é ativado nos \r\nplanos Avançado e Enterprise.";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(17, 55);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Definição do relatório";
		this.lknClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lknClose.AutoSize = true;
		this.lknClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknClose.Location = new System.Drawing.Point(716, 313);
		this.lknClose.Name = "lknClose";
		this.lknClose.Size = new System.Drawing.Size(176, 16);
		this.lknClose.TabIndex = 204;
		this.lknClose.TabStop = true;
		this.lknClose.Text = "Não mostrar mais esse alerta";
		this.lknClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lknClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknClose_LinkClicked);
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 144);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(900, 337);
		this.pnMarket.TabIndex = 108;
		this.pnMarket.Visible = false;
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
		base.Name = "frmTabExpBalance";
		this.Text = "Export. Geral: Saldos e prazos";
		base.Load += new System.EventHandler(frmTabExpBalance_Load);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.tspTaskMenu.ResumeLayout(false);
		this.tspTaskMenu.PerformLayout();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
