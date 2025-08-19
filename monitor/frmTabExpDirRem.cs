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

public class frmTabExpDirRem : Form
{
	public class clsOlvDirRem : ICloneable
	{
		public string Guid { get; set; }

		public string RegType { get; set; }

		public string DocNum { get; set; }

		public string Valor { get; set; }

		public string Descript { get; set; }

		public string Identity { get; set; }

		public DateTime? DtEmit { get; set; }

		public DateTime? DtEndOf { get; set; }

		public long? DtTotDays { get; set; }

		public string Unit { get; set; }

		public string ProdCode { get; set; }

		public string ProdNcm { get; set; }

		public string ProdCfop { get; set; }

		public decimal? QtRem { get; set; }

		public decimal? QtExport { get; set; }

		public decimal? QtAverb { get; set; }

		public decimal? QtDif01 { get; set; }

		public decimal? QtDif02 { get; set; }

		public decimal? QtDif03 { get; set; }

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

		public List<clsOlvDirRem> Children { get; set; } = new List<clsOlvDirRem>();

		public clsOlvDirRem GetClone()
		{
			return (clsOlvDirRem)MemberwiseClone();
		}

		object ICloneable.Clone()
		{
			return (clsOlvDirRem)MemberwiseClone();
		}
	}

	private class clsModelDoc
	{
		public Document DocRem = new Document();

		public ComexDeadLine ComexDeadLine = new ComexDeadLine();

		public List<clsModelDocItem> DocItemRemList = new List<clsModelDocItem>();

		public int DocItemRemCount;
	}

	private class clsModelDocItem
	{
		public DocItem DocItemRem = new DocItem();

		public List<clsModelDueItemRem> DueItemRemList = new List<clsModelDueItemRem>();

		public int DueItemRemCount;
	}

	public class clsModelDueItemRem
	{
		public DueItemRem DueItemRem = new DueItemRem();

		public DueHeader DueHeader = new DueHeader();

		public List<Event790700> AverbRemList = new List<Event790700>();

		public int AverbRemListCount;

		public DueItem DueItemExp = new DueItem();

		public Document DocExp = new Document();

		public DocItem DocItemExp = new DocItem();
	}

	private class ObjDoc
	{
		public string FiliaKey { get; set; }

		public string DocmtKey { get; set; }
	}

	private clsSrvTabExpDirRem _SqlTabData = new clsSrvTabExpDirRem();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsDataDueHeader _clsDataDueHeader = new clsDataDueHeader();

	private clsDataDueItem _clsDataDueItem = new clsDataDueItem();

	private clsDataDueItemRem _clsDataDueItemRem = new clsDataDueItemRem();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsDataDocItem _clsDataDocItem = new clsDataDocItem();

	private clsDataDocEvent790700 _clsDataEvtAver = new clsDataDocEvent790700();

	private clsDataComexDeadLine _clsDataComexDeadLine = new clsDataComexDeadLine();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private clsComexEmailService _clsEmailService = new clsComexEmailService();

	private clsComexProfile _clsComexProfile;

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

	private OLVColumn olvIdentity;

	private OLVColumn olvTag;

	private OLVColumn olvDtEmit;

	private OLVColumn olvDtEnd;

	private OLVColumn olvDays;

	private OLVColumn olvProduct;

	private OLVColumn olvUnit;

	private OLVColumn olvQtRem;

	private OLVColumn olvQtExport;

	private OLVColumn olvQtAverb;

	private OLVColumn olvQtDif01;

	private OLVColumn olvQtDif02;

	private OLVColumn olvQtDif03;

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

	private ToolStripButton tsbError;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton tsbSync;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private ToolStripComboBox tscUnitType;

	private ToolStripSeparator tssSepComex;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripLabel tslUnitType;

	private Panel pnMarket;

	private LinkLabel lknClose;

	private Panel pnMarketContent;

	private Button btSalesAction;

	private Label label3;

	private Label lbTitle03;

	private PictureBox picWarning;

	private LinkLabel lknAction;

	private Label lbText01;

	private Label lbTitle01;

	private Label lbTitle02;

	private Button btClose;

	private ImageList ImageListDocs;

	private Button btSalesContact;

	private ToolStripButton tsbDocs;

	private ToolStripSeparator tssSepDocs;

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

	public frmTabExpDirRem(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private async void frmTabExpDirRem_Load(object sender, EventArgs e)
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
			clsScreenGeral.funcCheckListViewDdic(typeof(clsOlvDirRem), lsvData);
		}
		funcDefineListViewFeatures();
		lsvData.CellToolTipShowing += olv_CellToolTipShowing;
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
		string varUnitType = await _clsDataParam.funcGetAsync("TabExpDirRem-UnitType");
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
		clsOlvDirRem varModel = (clsOlvDirRem)e.Model;
		if (varModel != null && e.ColumnIndex.Equals(olvHasDossie.Index))
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
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			return (!clsFunction.IsEmpty(clsOlvDirRem.HasDossie)) ? ((object)28) : null;
		};
		olvDocNum.ImageGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEqual(clsOlvDirRem.RegType, "REM-DOC-HEAD", "EXP-DOC-HEAD"))
			{
				if (clsFunction.IsEqual(clsOlvDirRem.Status, "SUCCESS"))
				{
					return 8;
				}
				if (clsFunction.IsEqual(clsOlvDirRem.Status, "WARNING"))
				{
					return 16;
				}
				if (clsFunction.IsEqual(clsOlvDirRem.Status, "ERROR"))
				{
					return 17;
				}
				return (object)null;
			}
			if (clsFunction.IsEqual(clsOlvDirRem.RegType, "REM-DOC-ITEM", "EXP-DOC-ITEM"))
			{
				if (clsFunction.IsEqual(clsOlvDirRem.Status, "SUCCESS"))
				{
					return 8;
				}
				if (clsFunction.IsEqual(clsOlvDirRem.Status, "WARNING"))
				{
					return 16;
				}
				if (clsFunction.IsEqual(clsOlvDirRem.Status, "ERROR"))
				{
					return 17;
				}
				return (object)null;
			}
			return 18;
		};
		lsvData.CanExpandGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return false;
			}
			if (clsOlvDirRem.Children == null)
			{
				return false;
			}
			return clsOlvDirRem.Children.Count > 0;
		};
		lsvData.ChildrenGetter = (object x) => ((clsOlvDirRem)x)?.Children;
		olvTag.ImageGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			return clsFunction.IsEqual(clsOlvDirRem.RegType, "REM-DOC-HEAD", "EXP-DOC-HEAD") ? ((!clsFunction.IsEmpty(clsOlvDirRem.DocNote)) ? ((object)14) : ((object)(-1))) : ((object)(-1));
		};
		olvTag.AspectGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			return clsFunction.IsEqual(clsOlvDirRem.RegType, "REM-DOC-HEAD", "EXP-DOC-HEAD") ? (clsFunction.IsEmpty(clsOlvDirRem.Tag) ? null : _TagHsList[clsOlvDirRem.Tag]) : null;
		};
		olvTipoDoc.AspectGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			return (clsOlvDirRem == null) ? null : clsComexCodes.funcGetTipoDocDesc(clsOlvDirRem.TipoDoc);
		};
		olvNatOper.AspectGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			return (clsOlvDirRem == null) ? null : clsComexCodes.funcGetNatOperDesc(clsOlvDirRem.NatOper);
		};
		olvTpDocTrans.AspectGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			return (clsOlvDirRem == null) ? null : clsComexCodes.funcGetTipoDocTransDesc(clsOlvDirRem.TpDocTrans);
		};
		olvViaTrans.AspectGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			return (clsOlvDirRem == null) ? null : clsComexCodes.funcGetViaTransDesc(clsOlvDirRem.ViaTrans);
		};
		olvDueStatus.ImageGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(clsOlvDirRem.DueStatus))
			{
				return (object)null;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueStatus, "REGIST", pIgnoreCase: true))
			{
				return 23;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueStatus, "PENDEN", pIgnoreCase: true))
			{
				return 24;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueStatus, "AGUARD", pIgnoreCase: true))
			{
				return 24;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueStatus, "PARCIAL", pIgnoreCase: true))
			{
				return 25;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueStatus, "AVERBADA", pIgnoreCase: true))
			{
				return 26;
			}
			return clsFunction.Contains(clsOlvDirRem.DueStatus, "CANCEL", pIgnoreCase: true) ? ((object)27) : ((object)25);
		};
		olvDueCanal.ImageGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueCanal, "Verde", pIgnoreCase: true))
			{
				return 19;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueCanal, "Amarelo", pIgnoreCase: true))
			{
				return 20;
			}
			if (clsFunction.Contains(clsOlvDirRem.DueCanal, "Laranja", pIgnoreCase: true))
			{
				return 21;
			}
			return clsFunction.Contains(clsOlvDirRem.DueCanal, "Vermelho", pIgnoreCase: true) ? ((object)22) : null;
		};
		olvLpcoStat.AspectGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(clsOlvDirRem.LpcoStatus))
			{
				return string.Empty;
			}
			return _IsLpcoControlEnabled ? clsOlvDirRem.LpcoStatus : "Detalhes";
		};
		olvLpcoStat.ImageGetter = delegate(object x)
		{
			clsOlvDirRem clsOlvDirRem = (clsOlvDirRem)x;
			if (clsOlvDirRem == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEmpty(clsOlvDirRem.LpcoStatus))
			{
				return string.Empty;
			}
			return _IsLpcoControlEnabled ? null : ((object)15);
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
			clsObjectType varUnitType = (clsObjectType)tscUnitType.SelectedItem;
			bool varByQuant = clsFunction.IsEqual(varUnitType.Value, "Quantity");
			lsvData.SuspendLayout();
			long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
			if (_IsLocked)
			{
				varPageSize = 4L;
			}
			List<Document> varclsDocList = await _clsDataDoc.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
			List<string> varDocKeyList = _clsDataDoc.funcGetDocKeyList(varclsDocList);
			List<DocItem> varDocItemList = await _clsDataDocItem.funcGetListByChaveListAsync(varDocKeyList);
			List<Event790700> varAverRemList = await _clsDataEvtAver.funcGetListByChaveListAsync(varDocKeyList);
			List<DueItemRem> varDueItemRemList = await _clsDataDueItemRem.funcGetListByRemNFeListAsync(varDocKeyList);
			await _clsDataDueHeader.funcGetItemByDueNumListAsync(varDueItemRemList);
			List<Estado> varStateList = await _clsDataEstado.funcGetByFilterAsync("", "", 0L, 0L);
			List<string> varDueHeaderNumList = _clsDataDueItemRem.funcGetNumList(varDueItemRemList);
			List<DueItem> varDueItemList = await _clsDataDueItem.funcGetListByNumListAsync(varDueHeaderNumList);
			List<string> varExpNFeKeyList = _clsDataDueItem.funcGetExpNFeKeyList(varDueItemList);
			List<Document> varDocExpListInDueItem = await _clsDataDoc.funcGetListByKeyListAsync(varExpNFeKeyList);
			List<DocItem> varDocExpItemListInDueItem = await _clsDataDocItem.funcGetListByChaveListAsync(varExpNFeKeyList);
			List<DueHeader> varDueHeaderList = await _clsDataDueHeader.funcGetItemByDueNumListAsync(varDueHeaderNumList, pNotCancel: true);
			if (pShow)
			{
				lbProgress.Text = "Aguarde, preparando relacionamento das informações...";
				plnMessage.Visible = true;
				Application.DoEvents();
			}
			ConcurrentQueue<clsModelDoc> varclsModelList = new ConcurrentQueue<clsModelDoc>();
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
					clsModelDoc varclsModel = new clsModelDoc();
					varclsModel.DocRem = varclsDoc;
					clsModelDoc clsModelDoc = varclsModel;
					clsModelDoc.ComexDeadLine = await _clsDataComexDeadLine.funcGetAsync(varclsDoc, "DirRem", varStateList);
					varclsModel.DocItemRemList = new List<clsModelDocItem>();
					int varDocItemPosition = varDocItemList.FindIndex((DocItem x) => clsFunction.IsEqual(x.Chave, varclsDoc.Chave));
					bool varExitLoop = varDocItemPosition < 0;
					while (!varExitLoop)
					{
						clsModelDocItem varclsModelItem2 = new clsModelDocItem();
						try
						{
							varclsModelItem2.DocItemRem = varDocItemList[varDocItemPosition];
							if (clsFunction.IsEqual(varclsModelItem2.DocItemRem.Chave, varclsDoc.Chave))
							{
								varclsModel.DocItemRemList.Add(varclsModelItem2);
								varclsModel.DocItemRemCount++;
								varDocItemPosition++;
								goto IL_025a;
							}
						}
						catch
						{
						}
						break;
						IL_025a:
						List<DueItemRem> varDueItemRemObtainList = varDueItemRemList.FindAll((DueItemRem r) => clsFunction.IsEqual(r.Filial, varclsModel.DocRem.Filial) && clsFunction.IsEqual(r.RemNFe, varclsModelItem2.DocItemRem.Chave) && clsFunction.IsEqualInt(r.RemNFeItem, varclsModelItem2.DocItemRem.nItem));
						if (varDueItemRemObtainList.Count <= 0)
						{
							varDueItemRemObtainList = varDueItemRemList.FindAll((DueItemRem r) => clsFunction.IsEqual(r.RemNFe, varclsModelItem2.DocItemRem.Chave) && clsFunction.IsEqualInt(r.RemNFeItem, varclsModelItem2.DocItemRem.nItem));
						}
						foreach (DueItemRem varDueItemRem2 in varDueItemRemObtainList)
						{
							clsModelDueItemRem varModelDueItemRem = new clsModelDueItemRem();
							try
							{
								varModelDueItemRem.DueItemRem = varDueItemRem2;
								varModelDueItemRem.DueItemRem = varDueItemRem2;
								int varPosDueItem = varDueItemList.FindIndex((DueItem x) => clsFunction.IsEqual(x.Num, varModelDueItemRem.DueItemRem.Num) && clsFunction.IsEqualInt(x.Item, varModelDueItemRem.DueItemRem.Item));
								if (varPosDueItem >= 0)
								{
									varModelDueItemRem.DueItemExp = varDueItemList[varPosDueItem];
								}
								int varPosDueHeader = varDueHeaderList.FindIndex((DueHeader x) => clsFunction.IsEqual(x.Num, varModelDueItemRem.DueItemRem.Num));
								if (varPosDueHeader >= 0)
								{
									varModelDueItemRem.DueHeader = varDueHeaderList[varPosDueHeader];
									int varPosDocExpInDueItem = varDocExpListInDueItem.FindIndex((Document x) => clsFunction.IsEqual(x.Chave, varModelDueItemRem.DueItemExp.ExpNFe));
									if (varPosDocExpInDueItem >= 0)
									{
										varModelDueItemRem.DocExp = varDocExpListInDueItem[varPosDocExpInDueItem];
									}
									else
									{
										varModelDueItemRem.DocExp = _clsDataDoc.funcGetDocFromKey(varModelDueItemRem.DueItemExp.Filial, varModelDueItemRem.DueItemExp.ExpNFe);
									}
									int varPosDocItemExp = varDocExpItemListInDueItem.FindIndex((DocItem x) => clsFunction.IsEqual(x.Chave, varModelDueItemRem.DueItemExp.ExpNFe) && clsFunction.IsEqualInt(x.nItem, varModelDueItemRem.DueItemExp.ExpNFeItem));
									if (varPosDocItemExp >= 0)
									{
										varModelDueItemRem.DocItemExp = varDocExpItemListInDueItem[varPosDocItemExp];
									}
									else
									{
										varModelDueItemRem.DocItemExp = new DocItem
										{
											Chave = varModelDueItemRem.DueItemExp.ExpNFe,
											nItem = varModelDueItemRem.DueItemExp.ExpNFeItem
										};
									}
									int varPosAverbRem = varAverRemList.FindIndex((Event790700 r) => clsFunction.IsEqual(r.Chave, varDueItemRem2.RemNFe) && clsFunction.IsEqualInt(r.nItem, varDueItemRem2.RemNFeItem) && clsFunction.IsEqual(r.nDue, varDueItemRem2.Num) && clsFunction.IsEqualInt(r.nItemDue, varDueItemRem2.Item));
									bool varExitLoopAverbRem = varPosAverbRem < 0;
									while (!varExitLoopAverbRem)
									{
										try
										{
											Event790700 varAverbRemItem = varAverRemList[varPosAverbRem];
											if (clsFunction.IsEqual(varAverbRemItem.Chave, varDueItemRem2.RemNFe) && clsFunction.IsEqualInt(varAverbRemItem.nItem, varDueItemRem2.RemNFeItem) && clsFunction.IsEqual(varAverbRemItem.nDue, varDueItemRem2.Num) && clsFunction.IsEqualInt(varAverbRemItem.nItemDue, varDueItemRem2.Item))
											{
												varModelDueItemRem.AverbRemList.Add(varAverbRemItem);
												varModelDueItemRem.AverbRemListCount++;
												varPosAverbRem++;
												continue;
											}
										}
										catch
										{
										}
										break;
									}
									varclsModelItem2.DueItemRemList.Add(varModelDueItemRem);
									varclsModelItem2.DueItemRemCount++;
								}
							}
							catch
							{
								break;
							}
						}
					}
					varclsModelList.Enqueue(varclsModel);
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
			lsvData.ClearObjects();
			ArrayList varDocList = new ArrayList();
			funcSetCollumnFormat(varUnitType.Value);
			foreach (clsModelDoc varclsModelItem in varclsModelList)
			{
				Document varclsDocRem = varclsModelItem.DocRem;
				ComexDeadLine varDeadLine = varclsModelItem.ComexDeadLine;
				decimal? varTotReme = default(decimal);
				decimal? varTotExpt = default(decimal);
				decimal? varTotAver = default(decimal);
				clsOlvDirRem varOlvRemHeader = new clsOlvDirRem();
				string varDocNum = clsFunction.funcGetValue(varclsDocRem.Num).PadLeft(9, '0');
				string varDocSerie = clsFunction.funcGetValue(varclsDocRem.Serie).PadLeft(3, '0');
				varOlvRemHeader.RegType = "REM-DOC-HEAD";
				varOlvRemHeader.Filial = varclsDocRem.Filial;
				varOlvRemHeader.Chave = varclsDocRem.Chave;
				varOlvRemHeader.DocNum = "NFe " + varDocNum + "-" + varDocSerie;
				varOlvRemHeader.Valor = varclsDocRem.Valor;
				varOlvRemHeader.Descript = varclsDocRem.EmitNome;
				varOlvRemHeader.Identity = clsFunction.funcFormatDoc(varclsDocRem.EmitID);
				varOlvRemHeader.Tag = varclsDocRem.Tag;
				varOlvRemHeader.Canceled = varclsDocRem.Canceled;
				varOlvRemHeader.HasCancelEvent = varclsDocRem.HasCancelEvent;
				varOlvRemHeader.DocNote = varclsDocRem.DocNote;
				DateTime varDtEmit = clsFunction.funcGetDate(varclsDocRem.DtEmi);
				DateTime varDtEndOf = clsFunction.funcGetDate(varclsDocRem.DtEmi).AddDays(clsFunction.funcConvStrToInt(varDeadLine.DeadLine));
				varOlvRemHeader.DtEmit = varDtEmit;
				varOlvRemHeader.DtEndOf = varDtEndOf;
				double varDtTotDays = DateTime.Now.Subtract(varDtEmit).TotalDays;
				varOlvRemHeader.DtTotDays = clsFunction.funcConvStrToInt(varDeadLine.DeadLine) - Convert.ToInt64(varDtTotDays);
				if (clsFunction.IsEmpty(varclsDocRem.HasXml))
				{
					varDocList.Add(varOlvRemHeader);
					continue;
				}
				if (!varByQuant)
				{
					varOlvRemHeader.QtRem = default(decimal);
				}
				if (!varByQuant)
				{
					varOlvRemHeader.QtExport = default(decimal);
				}
				if (!varByQuant)
				{
					varOlvRemHeader.QtAverb = default(decimal);
				}
				for (int varItemCount = 0; varItemCount < varclsModelItem.DocItemRemCount; varItemCount++)
				{
					clsModelDocItem varDocItem = varclsModelItem.DocItemRemList[varItemCount];
					DocItem varDocRemItem = varDocItem.DocItemRem;
					clsOlvDirRem varOlvRemItem = new clsOlvDirRem();
					varOlvRemItem.RegType = "REM-DOC-ITEM";
					varOlvRemItem.Filial = varclsDocRem.Filial;
					varOlvRemItem.Chave = varclsDocRem.Chave;
					varOlvRemItem.DocNum = "Item " + clsFunction.funcGetValue(varDocRemItem.nItem).PadLeft(3, '0');
					varOlvRemItem.Descript = varDocRemItem.xProd;
					varOlvRemItem.ProdCode = varDocRemItem.cProd;
					varOlvRemItem.ProdNcm = varDocRemItem.NCM;
					varOlvRemItem.ProdCfop = varDocRemItem.CFOP;
					varOlvRemItem.QtRem = clsFunction.funcConvStrToDec(varDocRemItem.qTrib);
					varTotReme += varOlvRemItem.QtRem * (decimal?)clsFunction.funcConvStrToDec(varDocRemItem.vUnTrib);
					if (!varByQuant)
					{
						varOlvRemItem.QtRem *= (decimal?)clsFunction.funcConvStrToDec(varDocRemItem.vUnTrib);
					}
					if (!varByQuant)
					{
						varOlvRemHeader.QtRem += varOlvRemItem.QtRem;
					}
					varOlvRemItem.QtAverb = default(decimal);
					varOlvRemItem.QtExport = default(decimal);
					varOlvRemItem.Unit = varDocRemItem.uTrib;
					if (!varByQuant)
					{
						varOlvRemItem.Unit = "R$";
					}
					decimal? qtDif;
					for (int varDueItemRemCount = 0; varDueItemRemCount < varDocItem.DueItemRemCount; varDueItemRemCount++)
					{
						clsModelDueItemRem varDueItemModelRem = varDocItem.DueItemRemList[varDueItemRemCount];
						DueItemRem varDueItemRem = varDueItemModelRem.DueItemRem;
						DueHeader varDueHeader = varDueItemModelRem.DueHeader;
						if (varDueHeader == null)
						{
							continue;
						}
						Document varDocExport = varDueItemModelRem.DocExp;
						if (varDocExport == null)
						{
							continue;
						}
						clsOlvDirRem varOlvExptHeader = new clsOlvDirRem();
						string varExptDocNum = clsFunction.funcGetValue(varDocExport.Num).PadLeft(9, '0');
						string varExptDocSerie = clsFunction.funcGetValue(varDocExport.Serie).PadLeft(3, '0');
						varOlvExptHeader.RegType = "EXP-DOC-HEAD";
						varOlvExptHeader.Filial = varDocExport.Filial;
						varOlvExptHeader.Chave = varDocExport.Chave;
						varOlvExptHeader.DocNum = "NFe " + varExptDocNum + "-" + varExptDocSerie;
						varOlvExptHeader.Descript = varDocExport.DestinNome;
						varOlvExptHeader.Identity = clsFunction.funcFormatDoc(varDocExport.DestinID);
						varOlvExptHeader.Tag = varDocExport.Tag;
						varOlvExptHeader.Canceled = varDocExport.Canceled;
						varOlvExptHeader.HasCancelEvent = varDocExport.HasCancelEvent;
						varOlvExptHeader.DocNote = varDocExport.DocNote;
						varOlvExptHeader.DtEmit = clsFunction.funcGetDate(varDocExport.DtEmi);
						varOlvExptHeader.PaisDestName = varDueHeader.PaisDestName;
						varOlvExptHeader.TipoDoc = varDueHeader.TipoDoc;
						varOlvExptHeader.DataDue = clsFunction.funcGetDate(varDueHeader.DataDue, pInCaseOfEmptyRetNull: true);
						varOlvExptHeader.DataAverb = clsFunction.funcGetDate(varDueHeader.DataAverb, pInCaseOfEmptyRetNull: true);
						varOlvExptHeader.NatOper = varDueHeader.NatOper;
						varOlvExptHeader.ConEmbNum = varDueHeader.ConEmbNum;
						varOlvExptHeader.ConEmbTipo = varDueHeader.ConEmbTipo;
						varOlvExptHeader.ConEmbData = clsFunction.funcGetDate(varDueHeader.ConEmbData, pInCaseOfEmptyRetNull: true);
						varOlvExptHeader.DueNum = varDueHeader.Num;
						varOlvExptHeader.DueChave = varDueHeader.Chave;
						varOlvExptHeader.DueCanal = varDueHeader.Canal;
						varOlvExptHeader.DueStatus = varDueHeader.Status;
						varOlvExptHeader.LpcoStatus = varDueHeader.LpcoStat;
						varOlvExptHeader.HasDossie = varDueHeader.HasDossie;
						varOlvExptHeader.TpDocTrans = varDueHeader.TipoDocTrans;
						varOlvExptHeader.ViaTrans = varDueHeader.ViaTransCode;
						if (!varByQuant)
						{
							varOlvExptHeader.QtExport = default(decimal);
						}
						if (!varByQuant)
						{
							varOlvExptHeader.QtAverb = default(decimal);
						}
						DocItem varExpItem = varDueItemModelRem.DocItemExp;
						if (varExpItem == null)
						{
							continue;
						}
						clsOlvDirRem varOlvExptItem = new clsOlvDirRem();
						varOlvExptItem.RegType = "EXP-DOC-ITEM";
						varOlvExptItem.Filial = varDocExport.Filial;
						varOlvExptItem.Chave = varDocExport.Chave;
						varOlvExptItem.DocNum = "Item " + clsFunction.funcGetValue(varExpItem.nItem).PadLeft(3, '0');
						varOlvExptItem.Descript = varExpItem.xProd;
						varOlvExptItem.ProdCode = varExpItem.cProd;
						varOlvExptItem.ProdNcm = varExpItem.NCM;
						varOlvExptItem.ProdCfop = varExpItem.CFOP;
						varOlvExptItem.QtExport = clsFunction.funcConvStrToDec(varDueItemRem.RemNFeItemQuant);
						varTotExpt += varOlvExptItem.QtExport * (decimal?)clsFunction.funcConvStrToDec(varDocRemItem.vUnTrib);
						if (!varByQuant)
						{
							varOlvExptItem.QtExport *= (decimal?)clsFunction.funcConvStrToDec(varDocRemItem.vUnTrib);
						}
						if (!varByQuant)
						{
							varOlvExptHeader.QtExport += varOlvExptItem.QtExport;
						}
						varOlvRemItem.QtExport += varOlvExptItem.QtExport;
						if (!varByQuant)
						{
							varOlvRemHeader.QtExport += varOlvExptItem.QtExport;
						}
						varOlvExptItem.QtAverb = default(decimal);
						varOlvExptItem.Unit = varExpItem.uTrib;
						if (!varByQuant)
						{
							varOlvExptItem.Unit = "R$";
						}
						for (int varAverbListCount = 0; varAverbListCount < varDueItemModelRem.AverbRemList.Count; varAverbListCount++)
						{
							Event790700 varclsAverb01 = varDueItemModelRem.AverbRemList[varAverbListCount];
							clsOlvDirRem varOlvAverbItem = new clsOlvDirRem();
							varOlvAverbItem.RegType = "AVERB-ITEM";
							varOlvAverbItem.Filial = varDocExport.Filial;
							varOlvAverbItem.Chave = varDocExport.Chave;
							varOlvAverbItem.DocNum = "DUE " + varclsAverb01.nDue + " Item " + varclsAverb01.nItemDue;
							varOlvAverbItem.Descript = varExpItem.xProd;
							varOlvAverbItem.ProdCode = varExpItem.cProd;
							varOlvAverbItem.ProdNcm = varExpItem.NCM;
							varOlvAverbItem.ProdCfop = varExpItem.CFOP;
							varOlvAverbItem.QtAverb = clsFunction.funcConvStrToDec(varclsAverb01.qItem);
							varTotAver += varOlvAverbItem.QtAverb * (decimal?)clsFunction.funcConvStrToDec(varDocRemItem.vUnTrib);
							if (!varByQuant)
							{
								varOlvAverbItem.QtAverb *= (decimal?)clsFunction.funcConvStrToDec(varDocRemItem.vUnTrib);
							}
							varOlvExptItem.QtAverb += varOlvAverbItem.QtAverb;
							if (!varByQuant)
							{
								varOlvExptHeader.QtAverb += varOlvAverbItem.QtAverb;
							}
							varOlvRemItem.QtAverb += varOlvAverbItem.QtAverb;
							if (!varByQuant)
							{
								varOlvRemHeader.QtAverb += varOlvAverbItem.QtAverb;
							}
							varOlvAverbItem.Unit = varDocRemItem.uTrib;
							if (!varByQuant)
							{
								varOlvAverbItem.Unit = "R$";
							}
							varOlvAverbItem.DtEmbar = clsFunction.funcGetDate(varclsAverb01.dtEmbarque, pInCaseOfEmptyRetNull: true);
							varOlvAverbItem.DataAverb = clsFunction.funcGetDate(varclsAverb01.dtAverbacao, pInCaseOfEmptyRetNull: true);
							varOlvAverbItem.DueItem = varclsAverb01.nItemDue;
							varOlvExptItem.Children.Add(varOlvAverbItem);
						}
						varOlvExptItem.QtDif03 = varOlvExptItem.QtExport - varOlvExptItem.QtAverb;
						qtDif = varOlvExptItem.QtDif03;
						if ((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue)
						{
							varOlvExptItem.Status = "ERROR";
						}
						else
						{
							qtDif = varOlvExptItem.QtDif03;
							if ((qtDif.GetValueOrDefault() > default(decimal)) & qtDif.HasValue)
							{
								varOlvExptItem.Status = "WARNING";
							}
							else
							{
								qtDif = varOlvExptItem.QtDif03;
								if ((qtDif.GetValueOrDefault() == default(decimal)) & qtDif.HasValue)
								{
									varOlvExptItem.Status = "SUCCESS";
								}
							}
						}
						varOlvExptHeader.Children.Add(varOlvExptItem);
						if (!varByQuant)
						{
							varOlvExptHeader.QtDif03 = varOlvExptHeader.QtExport - varOlvExptHeader.QtAverb;
						}
						if (!varByQuant)
						{
							varOlvExptHeader.Unit = "R$";
						}
						varOlvExptHeader.Status = varOlvExptItem.Status;
						varOlvRemItem.Children.Add(varOlvExptHeader);
					}
					varOlvRemItem.QtDif01 = varOlvRemItem.QtRem - varOlvRemItem.QtExport;
					varOlvRemItem.QtDif02 = varOlvRemItem.QtRem - varOlvRemItem.QtAverb;
					varOlvRemItem.QtDif03 = varOlvRemItem.QtExport - varOlvRemItem.QtAverb;
					qtDif = varOlvRemItem.QtDif01;
					if (!((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue))
					{
						qtDif = varOlvRemItem.QtDif02;
						if (!((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue))
						{
							qtDif = varOlvRemItem.QtDif03;
							if (!((qtDif.GetValueOrDefault() < default(decimal)) & qtDif.HasValue))
							{
								qtDif = varOlvRemItem.QtDif02;
								decimal num = 2;
								if ((qtDif.GetValueOrDefault() > num) & qtDif.HasValue)
								{
									varOlvRemItem.Status = "WARNING";
								}
								else
								{
									qtDif = varOlvRemItem.QtDif02;
									if ((qtDif.GetValueOrDefault() == default(decimal)) & qtDif.HasValue)
									{
										varOlvRemItem.Status = "SUCCESS";
									}
								}
								goto IL_1802;
							}
						}
					}
					varOlvRemItem.Status = "ERROR";
					goto IL_1802;
					IL_1802:
					if (!varByQuant)
					{
						varOlvRemHeader.Unit = "R$";
					}
					varOlvRemHeader.Children.Add(varOlvRemItem);
				}
				if (!varByQuant)
				{
					varOlvRemHeader.QtDif01 = varOlvRemHeader.QtRem - varOlvRemHeader.QtExport;
				}
				if (!varByQuant)
				{
					varOlvRemHeader.QtDif02 = varOlvRemHeader.QtRem - varOlvRemHeader.QtAverb;
				}
				if (!varByQuant)
				{
					varOlvRemHeader.QtDif03 = varOlvRemHeader.QtExport - varOlvRemHeader.QtAverb;
				}
				decimal varDifRemeAver = Math.Round((varTotReme - varTotAver).Value, 2);
				decimal varDifRemeExpt = Math.Round((varTotReme - varTotExpt).Value, 2);
				decimal varDifExptAver = Math.Round((varTotExpt - varTotAver).Value, 2);
				if (varDifRemeAver <= 0m && varOlvRemHeader.Children.Count > 0 && varOlvRemHeader.DtTotDays < 0)
				{
					varOlvRemHeader.DtTotDays = 0L;
				}
				if (varDifRemeAver < 0m || varDifRemeExpt < 0m || varDifExptAver < 0m)
				{
					varOlvRemHeader.Status = "ERROR";
				}
				else if (varOlvRemHeader.DtTotDays <= 0 && varDifRemeAver > 0m)
				{
					varOlvRemHeader.Status = "ERROR";
				}
				else if (varOlvRemHeader.DtTotDays <= clsFunction.funcConvStrToInt(varDeadLine.Warning) && varDifRemeAver > 0m)
				{
					varOlvRemHeader.Status = "WARNING";
				}
				else if (varOlvRemHeader.Children.Count <= 0)
				{
					varOlvRemHeader.Status = "WARNING";
				}
				else if (varDifRemeAver == 0m)
				{
					varOlvRemHeader.Status = "SUCCESS";
				}
				else if (varDifRemeAver > 0m)
				{
					varOlvRemHeader.Status = "WARNING";
				}
				varDocList.Add(varOlvRemHeader);
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
		_ = _HasFiscalioConnect;
		return await clsMonGeral.funcLoadTagsAsync(_HasColors, _TagHsList, contextMenuDocs, tsmCopyDocKey_Click, null, tsbTagCode_Click, tsbDocNote_Click, null, tsbRemoveDocNote_Click);
	}

	private async void tsbDocReset_Click(object sender, EventArgs e)
	{
		List<clsOlvDirRem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<clsOlvDirRem> varObjList = funcGetObjList(pFocused, pChecked);
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
		List<clsOlvDirRem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		List<clsOlvDirRem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
			varclsReturnFunc = await clsScreenGeral.funcExportDocTreeToExcelAsync<clsOlvDirRem>(lsvData, 5);
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
		List<clsOlvDirRem> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	public async Task<List<Document>> funcGetDocListAsync(List<clsOlvDirRem> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (clsOlvDirRem varObject in pObjList)
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

	public List<clsOlvDirRem> funcGetObjList(bool pFocused, bool pChecked, bool pOnlyPend = false)
	{
		List<clsOlvDirRem> varObjList = new List<clsOlvDirRem>();
		clsOlvDirRem varFocused = new clsOlvDirRem();
		if (pFocused && lsvData.FocusedItem != null)
		{
			try
			{
				varFocused = (clsOlvDirRem)lsvData.FocusedObject;
				varObjList.Add(varFocused);
			}
			catch
			{
				varFocused = new clsOlvDirRem();
			}
		}
		if (pChecked)
		{
			foreach (clsOlvDirRem varObject in lsvData.SelectedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varObjList.Add(varObject);
				}
			}
			if (varObjList.Count > 0)
			{
				varObjList = lsvData.Objects.Cast<clsOlvDirRem>().Intersect(varObjList).ToList();
			}
		}
		if ((!pFocused && !pChecked) || varObjList.Count <= 0)
		{
			varObjList = lsvData.FilteredObjects.Cast<clsOlvDirRem>().ToList();
			if (varObjList.Count <= 0)
			{
				varObjList = lsvData.Objects.Cast<clsOlvDirRem>().ToList();
			}
		}
		if (pOnlyPend && !clsFunction.IsAdmin)
		{
			varObjList.RemoveAll((clsOlvDirRem r) => clsFunction.Contains(r.Status, "SUCCESS", "AVERBADA", pIgnoreCase: true));
		}
		return varObjList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<clsOlvDirRem> varObjList = funcGetObjList(pFocused, pChecked);
		funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
		return true;
	}

	private async void funcRefreshListAsync(List<Document> pDocList, List<clsOlvDirRem> pObjList)
	{
		ConcurrentBag<ObjFieldBuffer> varDocFieldList = clsObjectBuffer.funcGet<Document>().Fields;
		ConcurrentBag<ObjFieldBuffer> varObjFieldList = clsObjectBuffer.funcGet<clsOlvDirRem>().Fields;
		await Task.WhenAll(((IEnumerable<clsOlvDirRem>)pObjList).Select((Func<clsOlvDirRem, Task>)async delegate(clsOlvDirRem varclsObjItem)
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

	private void lsvData_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (e.Item.Checked)
		{
			if (e.Item.BackColor == Color.White)
			{
				e.Item.BackColor = Color.LightBlue;
			}
			else if (e.Item.BackColor == Color.PapayaWhip)
			{
				e.Item.BackColor = Color.PeachPuff;
			}
		}
		else if (e.Item.BackColor == Color.LightBlue)
		{
			e.Item.BackColor = Color.White;
		}
		else if (e.Item.BackColor == Color.PeachPuff)
		{
			e.Item.BackColor = Color.PapayaWhip;
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
			clsOlvDirRem varModel = (clsOlvDirRem)e.Model;
			if (varModel != null && clsFunction.IsEqual(varModel.RegType, "REM-DOC-HEAD"))
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
			clsOlvDirRem varDocument = (clsOlvDirRem)e.Model;
			if (varDocument != null && !clsFunction.IsEmpty(varDocument.Tag))
			{
				string varColorName = (string)_HasColors[varDocument.Tag];
				e.SubItem.BackColor = ColorTranslator.FromHtml(varColorName);
			}
		}
	}

	private void lsvData_FormatRow(object sender, FormatRowEventArgs e)
	{
		clsOlvDirRem varDocument = (clsOlvDirRem)e.Model;
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
		clsOlvDirRem varclsObject = (clsOlvDirRem)e.Model;
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

	private async void funcShowDocViewerAsync(clsOlvDirRem pclsObject, bool pShowPDF, bool pShowXML)
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
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "SUCCESS", "WARNING", "ERROR", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		else if (tsbSuccess.Checked && tsbWarning.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "SUCCESS", "WARNING", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		else if (tsbWarning.Checked && tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "WARNING", "ERROR", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		else if (tsbSuccess.Checked && tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "SUCCESS", "ERROR", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		else if (tsbError.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "ERROR", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		else if (tsbWarning.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "WARNING", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		else if (tsbSuccess.Checked)
		{
			varFilters.Add(new ModelFilter((object model) => clsFunction.IsEqual(((clsOlvDirRem)model).Status, "SUCCESS", "") || !clsFunction.IsEqual(((clsOlvDirRem)model).RegType, "REM-DOC-HEAD")));
		}
		lsvData.AdditionalFilter = ((varFilters.Count == 0) ? null : new CompositeAllFilter(varFilters));
	}

	private async void tsbSync_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsComexApi varclsComexApi = new clsComexApi();
		try
		{
			tsbSync.Enabled = false;
			tsbSync.Text = "Sincronizando...";
			lbProgress.Text = "Conectando ao Portal Único: Siscomex ...";
			plnMessage.Visible = true;
			Application.DoEvents();
			lbProgress.Text = "Definindo NFes para Sincronização de DUE...";
			Application.DoEvents();
			List<clsOlvDirRem> varObjectList = funcGetObjList(pFocused: false, pChecked: true, pOnlyPend: true);
			List<clsComexApi.clsDueObject> varclsDbaDueList = new List<clsComexApi.clsDueObject>();
			int varCounter01 = 0;
			int varTotal01 = varObjectList.Count;
			FilialView varclsFilial = new FilialView();
			foreach (clsOlvDirRem varclsItem in varObjectList)
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
			clsReturn varclsRetLoad = await funcLoadDataAsync(_clsDataFilter, pShow: false);
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
		await _clsDataParam.funcSetAsync("TabExpDirRem-UnitType", varSelected);
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
			OLVColumn oLVColumn = olvQtRem;
			OLVColumn oLVColumn2 = olvQtExport;
			string text = (olvQtAverb.AspectToStringFormat = string.Empty);
			string aspectToStringFormat = (oLVColumn2.AspectToStringFormat = text);
			oLVColumn.AspectToStringFormat = aspectToStringFormat;
			OLVColumn oLVColumn3 = olvQtDif01;
			OLVColumn oLVColumn4 = olvQtDif02;
			text = (olvQtDif03.AspectToStringFormat = string.Empty);
			aspectToStringFormat = (oLVColumn4.AspectToStringFormat = text);
			oLVColumn3.AspectToStringFormat = aspectToStringFormat;
			olvQtRem.Text = "QtdRemessa";
			olvQtExport.Text = "QtdExportação";
			olvQtAverb.Text = "QtdAverbação";
		}
		else
		{
			OLVColumn oLVColumn5 = olvQtRem;
			OLVColumn oLVColumn6 = olvQtExport;
			string text = (olvQtAverb.AspectToStringFormat = "{0:0,0.00}");
			string aspectToStringFormat = (oLVColumn6.AspectToStringFormat = text);
			oLVColumn5.AspectToStringFormat = aspectToStringFormat;
			OLVColumn oLVColumn7 = olvQtDif01;
			OLVColumn oLVColumn8 = olvQtDif02;
			text = (olvQtDif03.AspectToStringFormat = "{0:0,0.00}");
			aspectToStringFormat = (oLVColumn8.AspectToStringFormat = text);
			oLVColumn7.AspectToStringFormat = aspectToStringFormat;
			olvQtRem.Text = "ValorRemessa";
			olvQtExport.Text = "ValorExportação";
			olvQtAverb.Text = "ValorAverbação";
		}
	}

	private void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
	{
		_ = string.Empty;
		if (e.ColumnIndex.Equals(olvTag.Index))
		{
			clsOlvDirRem varModel = (clsOlvDirRem)e.Model;
			if (varModel != null)
			{
				e.Text = varModel.DocNote;
			}
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
		clsHelpService.funcCallTipReportExportDirectAsync();
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
		List<clsOlvDirRem> varObjList = varEnumList.Cast<clsOlvDirRem>().ToList();
		if (varObjList == null)
		{
			varObjList = new List<clsOlvDirRem>();
		}
		_TotalValue = varObjList.Sum((clsOlvDirRem r) => clsFunction.funcConvStrToDec(r.Valor));
		EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
		varArguments.TotalValue = _TotalValue;
		varArguments.TotalQuant = varObjList.Count;
		OnEventTabManager(varArguments);
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private clsComexApi.clsDueObject funcGetDueObjItem(clsOlvDirRem pclsObject)
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
		foreach (clsOlvDirRem varclsItem in pclsObject.Children)
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
		List<clsOlvDirRem> list = funcGetObjList(pFocused: true, pChecked: true);
		List<clsComexApi.clsDueObject> varDueObjList = new List<clsComexApi.clsDueObject>();
		foreach (clsOlvDirRem varclsItem in list)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabExpDirRem));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lsvData = new BrightIdeasSoftware.TreeListView();
		this.olvDocNum = new BrightIdeasSoftware.OLVColumn();
		this.olvDescript = new BrightIdeasSoftware.OLVColumn();
		this.olvIdentity = new BrightIdeasSoftware.OLVColumn();
		this.olvTag = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEmit = new BrightIdeasSoftware.OLVColumn();
		this.olvDtEnd = new BrightIdeasSoftware.OLVColumn();
		this.olvDays = new BrightIdeasSoftware.OLVColumn();
		this.olvProduct = new BrightIdeasSoftware.OLVColumn();
		this.olvNCM = new BrightIdeasSoftware.OLVColumn();
		this.olvCFOP = new BrightIdeasSoftware.OLVColumn();
		this.olvUnit = new BrightIdeasSoftware.OLVColumn();
		this.olvQtRem = new BrightIdeasSoftware.OLVColumn();
		this.olvQtExport = new BrightIdeasSoftware.OLVColumn();
		this.olvQtAverb = new BrightIdeasSoftware.OLVColumn();
		this.olvQtDif01 = new BrightIdeasSoftware.OLVColumn();
		this.olvQtDif02 = new BrightIdeasSoftware.OLVColumn();
		this.olvQtDif03 = new BrightIdeasSoftware.OLVColumn();
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
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSync = new System.Windows.Forms.ToolStripButton();
		this.tssSepComex = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDocs = new System.Windows.Forms.ToolStripButton();
		this.tssSepDocs = new System.Windows.Forms.ToolStripSeparator();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.pnMarket = new System.Windows.Forms.Panel();
		this.lknClose = new System.Windows.Forms.LinkLabel();
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
		this.btClose = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.lsvData).BeginInit();
		this.tspTaskMenu.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.lsvData.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
		this.lsvData.AllColumns.Add(this.olvDocNum);
		this.lsvData.AllColumns.Add(this.olvDescript);
		this.lsvData.AllColumns.Add(this.olvIdentity);
		this.lsvData.AllColumns.Add(this.olvTag);
		this.lsvData.AllColumns.Add(this.olvDtEmit);
		this.lsvData.AllColumns.Add(this.olvDtEnd);
		this.lsvData.AllColumns.Add(this.olvDays);
		this.lsvData.AllColumns.Add(this.olvProduct);
		this.lsvData.AllColumns.Add(this.olvNCM);
		this.lsvData.AllColumns.Add(this.olvCFOP);
		this.lsvData.AllColumns.Add(this.olvUnit);
		this.lsvData.AllColumns.Add(this.olvQtRem);
		this.lsvData.AllColumns.Add(this.olvQtExport);
		this.lsvData.AllColumns.Add(this.olvQtAverb);
		this.lsvData.AllColumns.Add(this.olvQtDif01);
		this.lsvData.AllColumns.Add(this.olvQtDif02);
		this.lsvData.AllColumns.Add(this.olvQtDif03);
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
		this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[36]
		{
			this.olvDocNum, this.olvDescript, this.olvIdentity, this.olvTag, this.olvDtEmit, this.olvDtEnd, this.olvDays, this.olvProduct, this.olvNCM, this.olvCFOP,
			this.olvUnit, this.olvQtRem, this.olvQtExport, this.olvQtAverb, this.olvQtDif01, this.olvQtDif02, this.olvQtDif03, this.olvDueItem, this.olvFilial, this.olvDataDue,
			this.olvDataAverb, this.olvDtEmbar, this.olvDueNum, this.olvDueChave, this.olvDueStatus, this.olvLpcoStat, this.olvHasDossie, this.olvDueCanal, this.olvPaisDestName, this.olvTipoDoc,
			this.olvNatOper, this.olvConEmbNum, this.olvConEmbTipo, this.olvConEmbData, this.olvTpDocTrans, this.olvViaTrans
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
		this.olvDescript.Width = 110;
		this.olvIdentity.AspectName = "Identity";
		this.olvIdentity.Text = "CNPJ";
		this.olvTag.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTag.Text = "Etiq";
		this.olvTag.Width = 40;
		this.olvDtEmit.AspectName = "DtEmit";
		this.olvDtEmit.AspectToStringFormat = "{0:yyyy.MM.dd}";
		this.olvDtEmit.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmit.Text = "Emissão";
		this.olvDtEmit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDtEmit.Width = 96;
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
		this.olvQtRem.AspectName = "QtRem";
		this.olvQtRem.AspectToStringFormat = "{0:0,0.00}";
		this.olvQtRem.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvQtRem.Text = "Qt.Rem.";
		this.olvQtRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtRem.Width = 85;
		this.olvQtExport.AspectName = "QtExport";
		this.olvQtExport.AspectToStringFormat = "{0:0,0.00}";
		this.olvQtExport.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvQtExport.Text = "Qt.Export.";
		this.olvQtExport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtExport.Width = 85;
		this.olvQtAverb.AspectName = "QtAverb";
		this.olvQtAverb.AspectToStringFormat = "{0:0,0.00}";
		this.olvQtAverb.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvQtAverb.Text = "Qt.Averb.";
		this.olvQtAverb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtAverb.Width = 85;
		this.olvQtDif01.AspectName = "QtDif01";
		this.olvQtDif01.AspectToStringFormat = "{0:0,0.00}";
		this.olvQtDif01.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvQtDif01.Text = "[Rem-Export]";
		this.olvQtDif01.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtDif01.Width = 100;
		this.olvQtDif02.AspectName = "QtDif02";
		this.olvQtDif02.AspectToStringFormat = "{0:0,0.00}";
		this.olvQtDif02.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvQtDif02.Text = "[Rem-Averb]";
		this.olvQtDif02.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtDif02.Width = 100;
		this.olvQtDif03.AspectName = "QtDif03";
		this.olvQtDif03.AspectToStringFormat = "{0:0,0.00}";
		this.olvQtDif03.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvQtDif03.Text = "[Export-Averb]";
		this.olvQtDif03.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.olvQtDif03.Width = 100;
		this.olvDueItem.AspectName = "DueItem";
		this.olvDueItem.Text = "DueItem";
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
		this.tspTaskMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.toolStripSeparator3, this.tsbSuccess, this.toolStripSeparator6, this.tsbWarning, this.toolStripSeparator4, this.tsbError, this.toolStripSeparator5, this.tslUnitType, this.tscUnitType, this.toolStripSeparator2,
			this.tsbSync, this.tssSepComex, this.tsbDocs, this.tssSepDocs
		});
		this.tspTaskMenu.Location = new System.Drawing.Point(0, 0);
		this.tspTaskMenu.Name = "tspTaskMenu";
		this.tspTaskMenu.Padding = new System.Windows.Forms.Padding(2);
		this.tspTaskMenu.Size = new System.Drawing.Size(900, 27);
		this.tspTaskMenu.TabIndex = 42;
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
		this.tsbSync.Image = Monitor.Resources.image_cloud;
		this.tsbSync.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSync.Name = "tsbSync";
		this.tsbSync.Size = new System.Drawing.Size(74, 20);
		this.tsbSync.Text = " Siscomex";
		this.tsbSync.ToolTipText = "Sincronizar dados com Siscomex";
		this.tsbSync.Click += new System.EventHandler(tsbSync_Click);
		this.tssSepComex.Name = "tssSepComex";
		this.tssSepComex.Size = new System.Drawing.Size(6, 23);
		this.tsbDocs.Image = Monitor.Resources.image_copy;
		this.tsbDocs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDocs.Name = "tsbDocs";
		this.tsbDocs.Size = new System.Drawing.Size(63, 20);
		this.tsbDocs.Text = "Anexos";
		this.tsbDocs.ToolTipText = "Documentação Siscomex : DUE, LPCO, Dôssie, Manifesto de Carga, etc";
		this.tsbDocs.Click += new System.EventHandler(tsbDocs_Click);
		this.tssSepDocs.Name = "tssSepDocs";
		this.tssSepDocs.Size = new System.Drawing.Size(6, 23);
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 443);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(900, 38);
		this.plnMessage.TabIndex = 105;
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
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 144);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(900, 337);
		this.pnMarket.TabIndex = 107;
		this.pnMarket.Visible = false;
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
		this.lbTitle03.Text = "Saiba mais :";
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
		this.lknAction.Text = "Exportação Direta: Formação de Lote";
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
		this.lbTitle01.Text = "Exportação Direta: Saldos e Prazos é ativado nos \r\nplanos Avançado e Enterprise.";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(17, 55);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Definição do relatório:";
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
		base.Name = "frmTabExpDirRem";
		this.Text = "Exportação Direta: Saldos e Prazos";
		base.Load += new System.EventHandler(frmTabExpDirRem_Load);
		((System.ComponentModel.ISupportInitialize)this.lsvData).EndInit();
		this.tspTaskMenu.ResumeLayout(false);
		this.tspTaskMenu.PerformLayout();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
