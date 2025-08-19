using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using audit.fiscal.io;
using BrightIdeasSoftware;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabAuditor : Form
{
	public class clsOlvTreeNode : ICloneable
	{
		public string ObjName { get; set; }

		public string ObjDate { get; set; }

		public string ObjVersion { get; set; }

		public string ObjError { get; set; }

		public string ObjWarn { get; set; }

		public clsOlvTreeNode Child { get; set; }

		public clsOlvTreeNode GetClone()
		{
			return (clsOlvTreeNode)MemberwiseClone();
		}

		object ICloneable.Clone()
		{
			return (clsOlvTreeNode)MemberwiseClone();
		}
	}

	public List<clsOlvTreeNode> varFieldList = new List<clsOlvTreeNode>();

	private clsDataDocFiscal varclsDataDocFiscal = new clsDataDocFiscal();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private List<DocFiscal> _clsDocList = new List<DocFiscal>();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private Hashtable _TagHsList = new Hashtable();

	private Hashtable _HasColors = new Hashtable();

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private string _consMarketScreenUser = string.Empty;

	private bool _IsOnHeaderCheckStatus;

	private IContainer components;

	private Panel pnContent;

	private SplitContainer splitAuditor;

	private TreeListView TreeObjects;

	private OLVColumn olvDocType;

	private ToolStrip tspObjMenu;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton tsbAudit;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton tsbDelete;

	private OLVColumn olvLayout;

	private OLVColumn olvTarget;

	private OLVColumn olvDate;

	private OLVColumn olvTErrors;

	private SplitContainer splitContent;

	private OLVColumn olvStaType;

	private FastObjectListView lsvDataList;

	private OLVColumn olvSelect;

	private OLVColumn olvDocKey;

	private ToolStripDropDownButton tsbImport;

	private ToolStripMenuItem tsmEfdIcmsIpi;

	private ToolStripMenuItem tsmEfdPisCofins;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private OLVColumn olvFilial;

	private OLVColumn olvTWarnin;

	private ImageList ImageListDocs;

	private OLVColumn olvDocNum;

	private OLVColumn olvDocSerie;

	private OLVColumn olvDocModel;

	private OLVColumn olvDocLine;

	private ToolStrip tspTaskMenu;

	private ToolStripButton tsbError;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton tsbWarning;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton tsbSucess;

	private ToolStripSeparator toolStripSeparator5;

	private OLVColumn olvAction02;

	private ContextMenuStrip contextMenuDocs;

	private OLVColumn olvAudRole;

	private ToolStripSeparator toolStripSeparator6;

	private OLVColumn olvDocValue;

	private OLVColumn olvFisValue;

	private OLVColumn olvAudRoleHeader;

	private OLVColumn olvEmitida;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton toolStripButton1;

	private ObjectListView lsvTaskList;

	private OLVColumn olvSelectGroup;

	private OLVColumn olvAudTitle;

	private OLVColumn olvDocPart;

	private OLVColumn olvStTotal;

	private OLVColumn olvAction01;

	private OLVColumn olvErrorTitle;

	private Button btClose;

	private Panel pnMarketContent;

	private Button btSalesContact;

	private Label label4;

	private LinkLabel lknAction;

	private Label label2;

	private Label lbText02;

	private Label label1;

	private Button btSalesAction;

	private Label label3;

	private PictureBox picWarning;

	private Label lbTitle03;

	private Label lbText01;

	private Label lbTitle01;

	private Label lbTitle02;

	private LinkLabel lknClose;

	private Panel pnMarket;

	public event EventTabManagerHandler EventTabManager;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public frmTabAuditor(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private async void frmTabAuditor_Load(object sender, EventArgs e)
	{
		funcDefineListViewLayout();
		funcDefineListViewFeatures();
		await funcLoadTagsAsync();
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
		lbTitle01.Text = Text;
		Button button = btSalesContact;
		bool visible = (btSalesAction.Visible = false);
		button.Visible = visible;
		if (_IsLocked)
		{
			pnMarket.Visible = true;
			Button button2 = btClose;
			visible = (lknClose.Visible = false);
			button2.Visible = visible;
			ToolStrip toolStrip = tspObjMenu;
			visible = (tspTaskMenu.Enabled = false);
			toolStrip.Enabled = visible;
			TreeListView treeObjects = TreeObjects;
			ObjectListView objectListView = lsvTaskList;
			bool flag4 = (lsvDataList.Enabled = false);
			visible = (objectListView.Enabled = flag4);
			treeObjects.Enabled = visible;
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
			pnMarket.Visible = false;
			base.Controls.Remove(pnMarket);
		}
		else
		{
			pnMarket.Visible = true;
		}
	}

	private void funcDefineListViewLayout()
	{
		lsvTaskList.RowHeight = 40;
		TreeObjects.RowHeight = 22;
		olvAudTitle.Renderer = funcCreateDescribedTaskRenderer();
		olvAudTitle.CellPadding = new Rectangle(2, 4, 2, 2);
		olvAction01.IsButton = true;
		olvAction01.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
		olvAction01.ButtonSize = new Size(100, 26);
		olvAction01.EnableButtonWhenItemIsDisabled = true;
		olvAction01.TextAlign = HorizontalAlignment.Center;
		olvAction02.IsButton = true;
		olvAction02.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
		olvAction02.ButtonSize = new Size(100, 26);
		olvAction02.EnableButtonWhenItemIsDisabled = true;
		olvAction02.TextAlign = HorizontalAlignment.Center;
		lsvTaskList.ButtonClick += lsvTaskList_ButtonClick;
	}

	private async void lsvTaskList_ButtonClick(object sender, CellClickEventArgs e)
	{
		object varclsObject = lsvTaskList.SelectedObject;
		if (varclsObject != null)
		{
			DocAuditGroup varclsDocAudit = (DocAuditGroup)varclsObject;
			string varclsFieldValue = string.Empty;
			if (clsFunction.IsEqual(e.Column.AspectName, "Action01", pIgnoreCase: true))
			{
				varclsFieldValue = varclsDocAudit.Action01;
			}
			else if (clsFunction.IsEqual(e.Column.AspectName, "Action02", pIgnoreCase: true))
			{
				varclsFieldValue = varclsDocAudit.Action02;
			}
			FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsDocAudit.Filial);
			List<Document> varclsDocList = await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true);
			List<string> varDocTypeList = new List<string> { "NFe", "NFCe", "CTe", "CTeOs", "NFSe" };
			if (clsFunction.IsEqual(varclsFieldValue, "DOWNLOAD"))
			{
				await funcEventConfirmAsync(varDocTypeList, "DOWNLOAD", varclsFilial, varclsDocList);
			}
			else if (clsFunction.IsEqual(varclsFieldValue, "STATUSQUERY"))
			{
				await funcEventConfirmAsync(varDocTypeList, "747474", varclsFilial, varclsDocList);
			}
		}
	}

	private async Task<bool> funcEventConfirmAsync(List<string> pDocTypeList, string pEvtType, FilialView pclsFilial, List<Document> pDocList)
	{
		clsEvtService varclsEvtService = new clsEvtService();
		new clsDFeCodes();
		new clsMeasureService(null).funcSyncAsync();
		clsEventData varclsEvtData = new clsEventData();
		varclsEvtData.Agent = "Manual";
		varclsEvtData.FilialList = new List<FilialView> { pclsFilial };
		varclsEvtData.EvtUserType = pEvtType;
		foreach (Document varclsDoc in pDocList)
		{
			string varDocType = clsFunction.funcGetDocType(varclsDoc.Model, pCTeOs: false);
			if (pDocTypeList.Contains(varDocType) && (!clsFunction.IsEqual(pEvtType, "DOWNLOAD") || varclsEvtService.IsToDown(varclsDoc)) && !varclsEvtData.DocList.Any((Document r) => clsFunction.IsEqual(r.Filial, varclsDoc.Filial) && clsFunction.IsEqual(r.Chave, varclsDoc.Chave)))
			{
				varclsEvtData.DocList.Add(varclsDoc);
			}
		}
		foreach (string varDocType2 in pDocTypeList)
		{
			varclsEvtData.TypeList.Add(varDocType2, pEvtType);
		}
		await varclsEvtService.funcExecuteAsync(this, varclsEvtData);
		return true;
	}

	private DescribedTaskRenderer funcCreateDescribedTaskRenderer()
	{
		return new DescribedTaskRenderer
		{
			ImageList = ImageListDocs,
			DescriptionAspectName = "AudDesc",
			TitleFont = new Font("Tahoma", 9f, FontStyle.Bold),
			DescriptionFont = new Font("Tahoma", 9f),
			ImageTextSpace = 8,
			TitleDescriptionSpace = 1,
			UseGdiTextRendering = true
		};
	}

	private void funcDefineListViewFeatures()
	{
		olvDocType.ImageGetter = delegate(object x)
		{
			DocFiscal docFiscal = (DocFiscal)x;
			if (docFiscal == null)
			{
				return (object)null;
			}
			long num = clsFunction.funcConvStrToLong(docFiscal.TErrors);
			long num2 = clsFunction.funcConvStrToLong(docFiscal.TWarnin);
			if (clsFunction.IsEqual(docFiscal.DocStat, "AUDIT"))
			{
				if (num > 0)
				{
					return 1;
				}
				if (num2 > 0)
				{
					return 2;
				}
				return 3;
			}
			return 0;
		};
		olvTarget.AspectGetter = delegate(object x)
		{
			DocFiscal docFiscal = (DocFiscal)x;
			return (docFiscal == null) ? null : new clsDocFileFactory().funcGetClass(docFiscal.DocType).funcGetTarget(docFiscal.Target);
		};
		olvAudTitle.ImageGetter = delegate(object x)
		{
			DocAuditGroup docAuditGroup = (DocAuditGroup)x;
			if (docAuditGroup == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEqual(docAuditGroup.StaType, "E"))
			{
				return 1;
			}
			if (clsFunction.IsEqual(docAuditGroup.StaType, "W"))
			{
				return 2;
			}
			if (clsFunction.IsEqual(docAuditGroup.StaType, "S"))
			{
				return 3;
			}
			return clsFunction.IsEqual(docAuditGroup.StaType, "A") ? ((object)4) : null;
		};
		olvDocLine.AspectGetter = delegate(object x)
		{
			DocAuditItem docAuditItem = (DocAuditItem)x;
			return (docAuditItem == null) ? null : ((object)clsFunction.funcConvStrToLong(docAuditItem.LinePos));
		};
		olvAction01.AspectGetter = delegate(object x)
		{
			DocAuditGroup docAuditGroup = (DocAuditGroup)x;
			if (docAuditGroup == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEqual(docAuditGroup.Action01, "DOWNLOAD"))
			{
				return "Baixar XML Oficial";
			}
			return clsFunction.IsEqual(docAuditGroup.Action01, "STATUSQUERY") ? "Consultar Status" : string.Empty;
		};
		olvAction02.AspectGetter = delegate(object x)
		{
			DocAuditGroup docAuditGroup = (DocAuditGroup)x;
			if (docAuditGroup == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEqual(docAuditGroup.Action01, "DOWNLOAD"))
			{
				return "Baixar XML Oficial";
			}
			return clsFunction.IsEqual(docAuditGroup.Action01, "STATUSQUERY") ? "Consultar Status" : string.Empty;
		};
		olvEmitida.AspectGetter = delegate(object x)
		{
			DocAuditItem docAuditItem = (DocAuditItem)x;
			if (docAuditItem == null)
			{
				return (object)null;
			}
			return clsFunction.IsEmpty(docAuditItem.Emitida) ? "Terceiros" : "Empresa";
		};
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturn = new clsReturn();
		new ArrayList();
		string varSqlQuery = clsSqlFilter.funcGetDocFiscalItemSqlStr(pclsDataFilter);
		if (string.IsNullOrEmpty(varSqlQuery))
		{
			return varclsReturn;
		}
		long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
		if (_IsLocked)
		{
			varPageSize = 4L;
		}
		_clsDocList = await varclsDataDocFiscal.funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		TreeObjects.BeginUpdate();
		TreeObjects.SuspendLayout();
		TreeObjects.Roots = _clsDocList;
		TreeObjects.ExpandAll();
		olvSelect.HeaderCheckState = CheckState.Unchecked;
		lsvTaskList.SetObjects(null);
		lsvDataList.SetObjects(null);
		TreeObjects.SelectedItem = null;
		TreeObjects.FocusedItem = null;
		TreeObjects.EndUpdate();
		TreeObjects.ResumeLayout();
		return varclsReturn;
	}

	private async Task<bool> funcIsAvaliableAsync()
	{
		clsFeatureService varclsFeatureService = new clsFeatureService();
		await new clsDataParameter().funcAddCounterAsync(_FeatExtId + "-CLICKS");
		string pValue = await varclsFeatureService.funcGetFeatTypeAsync(_FeatExtId);
		bool varIsAvaliable = true;
		if (clsFunction.Contains(pValue, "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, _FeatExtId);
			varIsAvaliable = false;
		}
		return varIsAvaliable;
	}

	public async Task<bool> funcLoadTagsAsync()
	{
		return await clsMonGeral.funcLoadTagsAsync(_HasColors, _TagHsList, contextMenuDocs, tsmCopyDocKey_Click, null, null, null, null, null);
	}

	private async void tsmEfdIcmsIpi_Click(object sender, EventArgs e)
	{
		if (!(await funcIsAvaliableAsync()) || !(await clsScreenGeral.funcHasAccessAsync("AUDIT-MANAGER", "INSERT", "*")))
		{
			return;
		}
		clsDocFileFactory varclsFactory = new clsDocFileFactory();
		intDocFile varclsHandler = varclsFactory.funcGetClass(enDocType.EfdIcmsIpi);
		varclsHandler.EventDocFileStatusSrv += funcEventDocFileSrvStatus;
		varclsHandler.EventDocFileFilialSrv += funcEventDocFileSrvFilial;
		OpenFileDialog varFileDialog = new OpenFileDialog();
		OpenFileDialog openFileDialog = varFileDialog;
		openFileDialog.InitialDirectory = await clsScreenGeral.funcGetDefaultFolderAsync(this);
		varFileDialog.Multiselect = true;
		varFileDialog.Filter = "Arquivo SPED (*.*)|*.*";
		varFileDialog.Title = "Informe um ou mais arquivos do SPED ICMS/IPI";
		DialogResult varResult = varFileDialog.ShowDialog();
		await clsScreenGeral.funcSetDefaultFolderAsync(this, varFileDialog.FileName);
		if (varResult != DialogResult.OK || varFileDialog.FileNames.Length == 0)
		{
			return;
		}
		lbProgress.Text = "Aguarde, as informações estão sendo carregadas ...";
		plnMessage.Visible = true;
		Application.DoEvents();
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			string[] fileNames = varFileDialog.FileNames;
			foreach (string varFilePath in fileNames)
			{
				if (!File.Exists(varFilePath))
				{
					continue;
				}
				string varFileName = new FileInfo(varFilePath).Name;
				lbProgress.Text = "Arquivo " + varFileName + " : Lendo conteúdo ...";
				plnMessage.Visible = true;
				Application.DoEvents();
				clsReturn varclsReturnItem = await varclsHandler.funcImportAsync(varFilePath);
				varclsReturnFunc.AddRange(varclsReturnItem);
				DocFiscal varclsDocFiscal = (DocFiscal)varclsReturnItem.GetObject("DocFiscal");
				if (varclsDocFiscal != null && varclsDocFiscal.DocId != null)
				{
					_clsDocList.RemoveAll((DocFiscal r) => clsFunction.IsEqual(r.DocId, varclsDocFiscal.DocId));
					_clsDocList.Add(varclsDocFiscal);
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		lbProgress.Text = string.Empty;
		plnMessage.Visible = false;
		Application.DoEvents();
		TreeObjects.SetObjects(_clsDocList);
		TreeObjects.Refresh();
		olvSelect.HeaderCheckState = CheckState.Unchecked;
		lsvTaskList.SetObjects(null);
		lsvDataList.SetObjects(null);
		TreeObjects.SelectedItem = null;
		TreeObjects.FocusedItem = null;
		if ((varclsReturnFunc.HasError || varclsReturnFunc.HasWarning) && base.Visible)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
		}
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		return true;
	}

	private async void tsbAudit_Click(object sender, EventArgs e)
	{
		if (!(await funcIsAvaliableAsync()))
		{
			return;
		}
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			object varclsObject = TreeObjects.SelectedObject;
			if (varclsObject == null)
			{
				return;
			}
			DocFiscal varclsDocFiscal = (DocFiscal)varclsObject;
			if (!(await clsScreenGeral.funcHasAccessAsync("AUDIT-MANAGER", "AUDIT", varclsDocFiscal.Filial)))
			{
				return;
			}
			clsDocFileFactory varclsFactory = new clsDocFileFactory();
			intDocFile varclsHandler = varclsFactory.funcGetClass(enDocType.EfdIcmsIpi);
			varclsHandler.EventDocFileStatusSrv += funcEventDocFileSrvStatus;
			varclsHandler.EventDocFileFilialSrv += funcEventDocFileSrvFilial;
			varclsReturnFunc.AddRange(await varclsHandler.funcResetAsync(varclsDocFiscal));
			TreeObjects.SelectedObject = varclsDocFiscal;
			TreeObjects.RefreshObject(varclsDocFiscal);
			olvSelect.HeaderCheckState = CheckState.Unchecked;
			lsvTaskList.ClearObjects();
			lsvDataList.ClearObjects();
			varclsReturnFunc.AddRange(await varclsHandler.funcAuditAsync(varclsDocFiscal, pReset: false));
			DocFiscal varclsDocReturn = (DocFiscal)varclsReturnFunc.GetObject("DocFiscal");
			if (varclsDocReturn == null)
			{
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		lbProgress.Text = string.Empty;
		plnMessage.Visible = false;
		Application.DoEvents();
		int varIndex = TreeObjects.SelectedIndex;
		TreeObjects.SetObjects(_clsDocList);
		TreeObjects.Refresh();
		TreeObjects.SelectedIndex = varIndex;
		if ((varclsReturnFunc.HasError || varclsReturnFunc.HasWarning) && base.Visible)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
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

	private async Task<clsReturn> funcEventDocFileSrvFilial(object sender, EventDocFileSrvEventArgs e)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		if (e.ReturnFunc == null)
		{
			return varclsReturnFunc;
		}
		Partner varclsPartner = (Partner)e.ReturnFunc.GetObject("DocPartner");
		if (varclsPartner == null)
		{
			return varclsReturnFunc;
		}
		if (MessageBox.Show(string.Concat(string.Concat(string.Concat("Empresa não cadastrada no Fiscal.io Monitor !" + Environment.NewLine + Environment.NewLine, clsFunction.funcFormatDoc(varclsPartner.ID), " : ", clsFunction.funcGetValue(varclsPartner.Name)), Environment.NewLine, Environment.NewLine), "Deseja realizar o cadastro agora e importar o arquivo ?", Environment.NewLine, Environment.NewLine), "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
		{
			return varclsReturnFunc;
		}
		if (new frmFilial(null, varclsPartner).ShowDialog(this).Equals(DialogResult.Cancel))
		{
			return varclsReturnFunc;
		}
		varclsReturnFunc.ActionDone = true;
		return varclsReturnFunc;
	}

	private async void TreeObjects_SelectedIndexChanged(object sender, EventArgs e)
	{
		object varclsObject = TreeObjects.SelectedObject;
		if (varclsObject != null)
		{
			DocFiscal varclsDocFiscal = (DocFiscal)varclsObject;
			if (varclsDocFiscal != null)
			{
				List<DocAuditGroup> varAuditList = await new clsDataDocAudit().funcGetGroupListByDocAsync(varclsDocFiscal);
				olvSelect.HeaderCheckState = CheckState.Unchecked;
				lsvTaskList.SetObjects(varAuditList);
				lsvDataList.SetObjects(null);
			}
		}
	}

	private async Task funcUpdateDataListValuesAsync(IList<DocAuditGroup> pSelectedObjects)
	{
		lsvDataList.ClearObjects();
		List<DocAuditItem> varAllAuditItems = new List<DocAuditItem>();
		if (pSelectedObjects == null || pSelectedObjects.Count == 0)
		{
			return;
		}
		foreach (DocAuditGroup varClsObj in pSelectedObjects)
		{
			DocAuditGroup varclsDocAuditt = varClsObj;
			if (varclsDocAuditt == null)
			{
				return;
			}
			List<DocAuditItem> varAuditListt = await new clsDataDocAudit().funcGetListByAuditGroupAsync(varclsDocAuditt);
			foreach (DocAuditItem varObject in varAuditListt)
			{
				Document varclsDocument = _clsDataDoc.funcGetDocFromKey(varclsDocAuditt.Filial, varObject.DocKey);
				varObject.DocNum = varclsDocument.Num;
				varObject.DocSerie = varclsDocument.Serie;
				varObject.DocModel = varclsDocument.Model;
			}
			varAllAuditItems.AddRange(varAuditListt);
		}
		lsvDataList.ClearObjects();
		lsvDataList.SetObjects(varAllAuditItems);
		olvSelect.HeaderCheckState = CheckState.Unchecked;
	}

	private void lsvTaskList_Resize(object sender, EventArgs e)
	{
		int varTotalWith = 0;
		foreach (OLVColumn varColumn in lsvTaskList.Columns)
		{
			if (varColumn.IsVisible && !varColumn.Equals(olvAudTitle))
			{
				varTotalWith += varColumn.MinimumWidth;
			}
		}
		olvAudTitle.Width = lsvTaskList.Width - varTotalWith - 25;
	}

	private void tsbError_Click(object sender, EventArgs e)
	{
		tsbError.Checked = !tsbError.Checked;
		ToolStripButton toolStripButton = tsbWarning;
		bool flag = (tsbSucess.Checked = false);
		toolStripButton.Checked = flag;
		funcRebuildFilters();
	}

	private void tsbWarning_Click(object sender, EventArgs e)
	{
		tsbWarning.Checked = !tsbWarning.Checked;
		ToolStripButton toolStripButton = tsbError;
		bool flag = (tsbSucess.Checked = false);
		toolStripButton.Checked = flag;
		funcRebuildFilters();
	}

	private void tsbSucess_Click(object sender, EventArgs e)
	{
		tsbSucess.Checked = !tsbSucess.Checked;
		ToolStripButton toolStripButton = tsbError;
		bool flag = (tsbWarning.Checked = false);
		toolStripButton.Checked = flag;
		funcRebuildFilters();
	}

	private void funcRebuildFilters()
	{
		List<IModelFilter> filters = new List<IModelFilter>();
		if (tsbError.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((DocAuditGroup)model).StaType.Equals("E")));
		}
		if (tsbWarning.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((DocAuditGroup)model).StaType.Equals("W")));
		}
		if (tsbSucess.Checked)
		{
			filters.Add(new ModelFilter((object model) => ((DocAuditGroup)model).StaType.Equals("S")));
		}
		lsvTaskList.AdditionalFilter = ((filters.Count == 0) ? null : new CompositeAllFilter(filters));
	}

	private void lsvDataList_DoubleClick(object sender, EventArgs e)
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

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<DocAuditItem> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<DocAuditItem> pObjList, bool pSyncFromDbaFirst)
	{
		DocFiscal varDocFiscal = (DocFiscal)TreeObjects.FocusedObject;
		if (varDocFiscal == null)
		{
			return new List<Document>();
		}
		List<Document> varDocDbaList = new List<Document>();
		List<Document> varNotFoundList = new List<Document>();
		foreach (DocAuditItem varObject in pObjList)
		{
			if (clsFunction.IsEqual(varObject.AudRole, "DOC-NOT-FOUND", pIgnoreCase: true))
			{
				Document varclsDoc = _clsDataDoc.funcGetDocFromKey(varDocFiscal.Filial, varObject.DocKey);
				varNotFoundList.Add(varclsDoc);
				continue;
			}
			Document varclsDoc2 = new Document
			{
				Filial = varDocFiscal.Filial,
				Chave = varObject.DocKey
			};
			varDocDbaList.Add(varclsDoc2);
		}
		List<Document> varDocList = new List<Document>();
		if (pSyncFromDbaFirst)
		{
			varDocList = await _clsDataDoc.funcGetSyncListByListAsync(varDocDbaList);
		}
		varDocList.AddRange(varNotFoundList);
		return varDocList;
	}

	public List<DocAuditItem> funcGetObjList(bool pFocused, bool pChecked)
	{
		List<DocAuditItem> varAuditList = new List<DocAuditItem>();
		DocAuditItem varFocused = new DocAuditItem();
		new clsDataDoc();
		if ((DocFiscal)TreeObjects.FocusedObject == null)
		{
			return varAuditList;
		}
		if (pFocused && lsvDataList.FocusedItem != null)
		{
			try
			{
				varFocused = (DocAuditItem)lsvDataList.FocusedObject;
				varAuditList.Add(varFocused);
			}
			catch
			{
				varFocused = new DocAuditItem();
			}
		}
		if (pChecked)
		{
			foreach (DocAuditItem varObject in lsvDataList.CheckedObjects)
			{
				if (!pFocused || !varObject.Equals(varFocused))
				{
					varAuditList.Add(varObject);
				}
			}
			if (varAuditList.Count > 0)
			{
				varAuditList = lsvDataList.Objects.Cast<DocAuditItem>().Intersect(varAuditList).ToList();
			}
		}
		if ((!pFocused && !pChecked) || varAuditList.Count <= 0)
		{
			varAuditList = lsvDataList.FilteredObjects.Cast<DocAuditItem>().ToList();
			if (varAuditList.Count <= 0)
			{
				varAuditList = lsvDataList.Objects.Cast<DocAuditItem>().ToList();
			}
		}
		return varAuditList;
	}

	private async void lsvDataList_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
	{
		if (e.ColumnIndex != olvDocNum.Index)
		{
			return;
		}
		clsDataDoc varclsDataDoc = new clsDataDoc();
		DocFiscal varDocFiscal = (DocFiscal)TreeObjects.FocusedObject;
		if (varDocFiscal == null)
		{
			return;
		}
		DocAuditItem varDocAudit = (DocAuditItem)e.Model;
		if (varDocAudit != null)
		{
			Document varclsDoc = ((!clsFunction.IsEqual(varDocAudit.AudRole, "DOC-NOT-FOUND", pIgnoreCase: true)) ? (await varclsDataDoc.funcGetItemByKeyAsync(varDocFiscal.Filial, varDocAudit.DocKey)) : _clsDataDoc.funcGetDocFromKey(varDocFiscal.Filial, varDocAudit.DocKey));
			if (varclsDoc != null)
			{
				funcShowDocViewerAsync(pShowPDF: true, pShowXML: false, varclsDoc);
			}
		}
	}

	private async void lsvDataList_KeyUp(object sender, KeyEventArgs e)
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

	private async void tsbDelete_Click(object sender, EventArgs e)
	{
		DocFiscal varDocFiscal = (DocFiscal)TreeObjects.FocusedObject;
		if (varDocFiscal == null || !(await clsScreenGeral.funcHasAccessAsync("AUDIT-MANAGER", "DELETE", varDocFiscal.Filial)))
		{
			return;
		}
		string varMensagem = "Deseja excluir o arquivo selecionado?";
		varMensagem = varMensagem + Environment.NewLine + Environment.NewLine;
		varMensagem = varMensagem + varDocFiscal.DocType + "  / " + varDocFiscal.DocDate + " / " + varDocFiscal.Filial;
		varMensagem = varMensagem + Environment.NewLine + Environment.NewLine;
		if (!MessageBox.Show(varMensagem, "Atenção, confirme sua ação", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
		{
			lbProgress.Text = "Excluindo arquivo selecionado ...";
			plnMessage.Visible = true;
			Application.DoEvents();
			await new clsDataDocAudit().funcDeleteByDocIdAsync(varDocFiscal.DocId);
			await new clsDataDocFiscLink().funcDeleteAsyncByDocFisc(varDocFiscal.DocId);
			await new clsDataDocFiscal().funcDeleteAsync(varDocFiscal);
			_clsDocList.RemoveAll((DocFiscal r) => r.DocId.Equals(varDocFiscal.DocId));
			TreeObjects.SetObjects(_clsDocList);
			TreeObjects.Refresh();
			olvSelect.HeaderCheckState = CheckState.Unchecked;
			lsvTaskList.SetObjects(null);
			lsvDataList.SetObjects(null);
			TreeObjects.SelectedItem = null;
			TreeObjects.FocusedItem = null;
			plnMessage.Visible = false;
			lbProgress.Text = "";
			Application.DoEvents();
		}
	}

	public void funcSearchText(string pSearchTerm, bool pSearchInteligent)
	{
		if (clsFunction.IsEmpty(pSearchTerm))
		{
			lsvDataList.AdditionalFilter = null;
		}
		else
		{
			if (pSearchInteligent)
			{
				foreach (OLVColumn column in lsvDataList.Columns)
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
				foreach (OLVColumn column2 in lsvDataList.Columns)
				{
					column2.Searchable = true;
				}
			}
			TextMatchFilter varTextMatchFilter = TextMatchFilter.Contains(lsvDataList, pSearchTerm);
			if (lsvDataList.DefaultRenderer == null)
			{
				lsvDataList.DefaultRenderer = new HighlightTextRenderer(varTextMatchFilter);
			}
			lsvDataList.AdditionalFilter = varTextMatchFilter;
		}
		if (lsvDataList.ShowGroups)
		{
			lsvDataList.BuildGroups();
		}
	}

	private List<DocAuditGroup> funcGetAllTaskList()
	{
		if (lsvTaskList.Objects == null)
		{
			return new List<DocAuditGroup>();
		}
		return lsvTaskList.Objects.Cast<DocAuditGroup>().ToList();
	}

	private List<DocAuditGroup> funcGetCheckedTaskList()
	{
		if (lsvTaskList.CheckedObjects == null)
		{
			return new List<DocAuditGroup>();
		}
		return lsvTaskList.CheckedObjects.Cast<DocAuditGroup>().ToList();
	}

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		new clsReturn();
		return (lsvDataList.Items.Count <= 0) ? (await funcExportAuditGroupToExcellAsync(funcGetAllTaskList(), onProgressChange)) : (await funcExportAuditGroupToExcellAsync(funcGetCheckedTaskList(), onProgressChange));
	}

	public clsReturn funcGetAuditGroupToSend(Action<decimal> onProgressChange)
	{
		clsReturn clsReturn = new clsReturn();
		new List<DocAuditGroup>();
		List<DocAuditGroup> varAuditGroup = funcGetCheckedTaskList();
		if (varAuditGroup.Count == 0)
		{
			varAuditGroup = funcGetAllTaskList();
		}
		clsReturn.AddValue("varAuditGroup", varAuditGroup);
		return clsReturn;
	}

	public async Task<List<DocAuditItem>> funcPrepareAuditGroupToExport(List<DocAuditGroup> pDocAuditGroup)
	{
		clsDataDocAudit varclsDataDocAudit = new clsDataDocAudit();
		List<DocAuditItem> varDocAuditItemList = new List<DocAuditItem>();
		DocFiscal varSelectedDocFiscal = (DocFiscal)TreeObjects.SelectedObject;
		DocFiscal varDocFiscal = await new clsDataDocFiscal().funcGetItemByKeyAsync(varSelectedDocFiscal.DocId);
		foreach (DocAuditGroup varAuditGroup in pDocAuditGroup)
		{
			List<DocAuditItem> varCurrentDocAuditItemList = await varclsDataDocAudit.funcGetListByAuditGroupAsync(varAuditGroup);
			foreach (DocAuditItem varAuditItem in varCurrentDocAuditItemList)
			{
				Document varclsDoc = _clsDataDoc.funcGetDocFromKey(varAuditGroup.Filial, varAuditItem.DocKey);
				intDocFile varHandler = new clsDocFileFactory().funcGetClass(varDocFiscal.DocType);
				varAuditItem.DocType = varDocFiscal.DocType;
				varAuditItem.Filial = varDocFiscal.Filial;
				varAuditItem.DocDate = varDocFiscal.DocDate;
				varAuditItem.Target = varHandler.funcGetTarget(varDocFiscal.Target);
				varAuditItem.DocNum = varclsDoc.Num;
				varAuditItem.DocSerie = varclsDoc.Serie;
				varAuditItem.DocModel = varclsDoc.Model;
				varAuditItem.LinePos = varAuditItem.LinePos.Replace("0", "");
				varAuditItem.Emitida = (clsFunction.IsEmpty(varAuditItem.Emitida) ? "Terceiros" : "Empresa");
				AuditRole varAuditRole = new clsDataAuditRole().funcGetItemByKey(varAuditItem.AudRole);
				varAuditItem.AudRole = varAuditRole.AudDesc;
			}
			varDocAuditItemList.AddRange(varCurrentDocAuditItemList);
		}
		return varDocAuditItemList;
	}

	private async Task<clsReturn> funcExportAuditGroupToExcellAsync(List<DocAuditGroup> pDocAuditGroup, Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportDocToExcelAsync(await funcPrepareAuditGroupToExport(pDocAuditGroup), onProgressChange);
	}

	public int funcGetTotalDocs()
	{
		return lsvDataList.Items.Count;
	}

	public decimal funcGetTotalValue()
	{
		return 0m;
	}

	private void btHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallAuditorHelp();
	}

	private void btClose_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		clsHelpService.funcCallAuditorHelp();
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
	}

	private async void lknClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
		await _clsDataParam.funcSetAsync(_consMarketScreenUser, "X");
	}

	private void btSalesAction_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallProductPricePageAsync();
	}

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipFiscalAuditToolAsync();
	}

	private void lsvDataList_FormatRow(object sender, FormatRowEventArgs e)
	{
		e.Item.BackColor = (e.Item.Checked ? Color.LightBlue : Color.White);
	}

	private void lsvDataList_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (!e.Item.Checked)
		{
			olvSelect.HeaderCheckState = CheckState.Unchecked;
		}
		funcSetTabDataTotalValues();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcSalesContactAsync(this, btSalesContact, _FeatExtId);
	}

	private void lsvDataList_HeaderCheckBoxChanging(object sender, HeaderCheckBoxChangingEventArgs e)
	{
		_IsOnHeaderCheckStatus = true;
		if (e.NewCheckState == CheckState.Checked)
		{
			lsvDataList.CheckAll();
		}
		else
		{
			lsvDataList.UncheckAll();
		}
		_IsOnHeaderCheckStatus = false;
		funcSetTabDataTotalValues();
	}

	private void funcSetTabDataTotalValues()
	{
		if (!_IsOnHeaderCheckStatus)
		{
			IEnumerable varEnumList = null;
			FastObjectListView fastObjectListView = lsvDataList;
			varEnumList = ((fastObjectListView == null || !(fastObjectListView.CheckedObjects?.Count > 0)) ? lsvDataList.FilteredObjects : lsvDataList.CheckedObjectsEnumerable);
			List<DocAuditItem> varObjList = varEnumList.Cast<DocAuditItem>().ToList();
			if (varObjList == null)
			{
				varObjList = new List<DocAuditItem>();
			}
			EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
			varArguments.TotalValue = default(decimal);
			varArguments.TotalQuant = varObjList.Count;
			OnEventTabManager(varArguments);
		}
	}

	private void lsvDataList_ItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		funcSetTabDataTotalValues();
	}

	private async void lsvTaskList_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		BeginInvoke((Action)async delegate
		{
			List<DocAuditGroup> varclsObjects = lsvTaskList.CheckedObjects.Cast<DocAuditGroup>().ToList();
			await funcUpdateDataListValuesAsync(varclsObjects);
		});
	}

	private void HeaderCheckBoxChangingGroup(object sender, HeaderCheckBoxChangingEventArgs e)
	{
		if (e.NewCheckState == CheckState.Checked)
		{
			lsvTaskList.CheckAll();
		}
		else
		{
			lsvTaskList.UncheckAll();
		}
	}

	private void lsvTaskList_MouseClick(object sender, MouseEventArgs e)
	{
		ListViewHitTestInfo hit = lsvTaskList.HitTest(e.Location);
		if (hit.Item != null && hit.SubItem != null && hit.Item.SubItems.IndexOf(hit.SubItem) == 0)
		{
			e = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);
			hit.Item.Checked = !hit.Item.Checked;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabAuditor));
		this.pnContent = new System.Windows.Forms.Panel();
		this.splitAuditor = new System.Windows.Forms.SplitContainer();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.TreeObjects = new BrightIdeasSoftware.TreeListView();
		this.olvDocType = new BrightIdeasSoftware.OLVColumn();
		this.olvFilial = new BrightIdeasSoftware.OLVColumn();
		this.olvDate = new BrightIdeasSoftware.OLVColumn();
		this.olvTarget = new BrightIdeasSoftware.OLVColumn();
		this.olvTErrors = new BrightIdeasSoftware.OLVColumn();
		this.olvTWarnin = new BrightIdeasSoftware.OLVColumn();
		this.olvLayout = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.tspObjMenu = new System.Windows.Forms.ToolStrip();
		this.tsbImport = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsmEfdIcmsIpi = new System.Windows.Forms.ToolStripMenuItem();
		this.tsmEfdPisCofins = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbAudit = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.splitContent = new System.Windows.Forms.SplitContainer();
		this.lsvTaskList = new BrightIdeasSoftware.ObjectListView();
		this.olvSelectGroup = new BrightIdeasSoftware.OLVColumn();
		this.olvAudTitle = new BrightIdeasSoftware.OLVColumn();
		this.olvStaType = new BrightIdeasSoftware.OLVColumn();
		this.olvDocPart = new BrightIdeasSoftware.OLVColumn();
		this.olvStTotal = new BrightIdeasSoftware.OLVColumn();
		this.olvAction01 = new BrightIdeasSoftware.OLVColumn();
		this.olvAction02 = new BrightIdeasSoftware.OLVColumn();
		this.olvAudRoleHeader = new BrightIdeasSoftware.OLVColumn();
		this.tspTaskMenu = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSucess = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbWarning = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbError = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.lsvDataList = new BrightIdeasSoftware.FastObjectListView();
		this.olvSelect = new BrightIdeasSoftware.OLVColumn();
		this.olvErrorTitle = new BrightIdeasSoftware.OLVColumn();
		this.olvEmitida = new BrightIdeasSoftware.OLVColumn();
		this.olvDocLine = new BrightIdeasSoftware.OLVColumn();
		this.olvDocNum = new BrightIdeasSoftware.OLVColumn();
		this.olvDocSerie = new BrightIdeasSoftware.OLVColumn();
		this.olvDocModel = new BrightIdeasSoftware.OLVColumn();
		this.olvDocValue = new BrightIdeasSoftware.OLVColumn();
		this.olvFisValue = new BrightIdeasSoftware.OLVColumn();
		this.olvDocKey = new BrightIdeasSoftware.OLVColumn();
		this.olvAudRole = new BrightIdeasSoftware.OLVColumn();
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.btClose = new System.Windows.Forms.Button();
		this.pnMarketContent = new System.Windows.Forms.Panel();
		this.btSalesContact = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.lknAction = new System.Windows.Forms.LinkLabel();
		this.label2 = new System.Windows.Forms.Label();
		this.lbText02 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btSalesAction = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.picWarning = new System.Windows.Forms.PictureBox();
		this.lbTitle03 = new System.Windows.Forms.Label();
		this.lbText01 = new System.Windows.Forms.Label();
		this.lbTitle01 = new System.Windows.Forms.Label();
		this.lbTitle02 = new System.Windows.Forms.Label();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.pnMarket = new System.Windows.Forms.Panel();
		this.pnContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitAuditor).BeginInit();
		this.splitAuditor.Panel1.SuspendLayout();
		this.splitAuditor.Panel2.SuspendLayout();
		this.splitAuditor.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.TreeObjects).BeginInit();
		this.tspObjMenu.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContent).BeginInit();
		this.splitContent.Panel1.SuspendLayout();
		this.splitContent.Panel2.SuspendLayout();
		this.splitContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvTaskList).BeginInit();
		this.tspTaskMenu.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvDataList).BeginInit();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		this.pnMarket.SuspendLayout();
		base.SuspendLayout();
		this.pnContent.Controls.Add(this.splitAuditor);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(1025, 564);
		this.pnContent.TabIndex = 10;
		this.splitAuditor.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitAuditor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitAuditor.Location = new System.Drawing.Point(0, 0);
		this.splitAuditor.Name = "splitAuditor";
		this.splitAuditor.Panel1.Controls.Add(this.plnMessage);
		this.splitAuditor.Panel1.Controls.Add(this.TreeObjects);
		this.splitAuditor.Panel1.Controls.Add(this.tspObjMenu);
		this.splitAuditor.Panel2.Controls.Add(this.splitContent);
		this.splitAuditor.Panel2MinSize = 0;
		this.splitAuditor.Size = new System.Drawing.Size(1025, 564);
		this.splitAuditor.SplitterDistance = 456;
		this.splitAuditor.TabIndex = 0;
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 526);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(456, 38);
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
		this.TreeObjects.AllColumns.Add(this.olvDocType);
		this.TreeObjects.AllColumns.Add(this.olvFilial);
		this.TreeObjects.AllColumns.Add(this.olvDate);
		this.TreeObjects.AllColumns.Add(this.olvTarget);
		this.TreeObjects.AllColumns.Add(this.olvTErrors);
		this.TreeObjects.AllColumns.Add(this.olvTWarnin);
		this.TreeObjects.AllColumns.Add(this.olvLayout);
		this.TreeObjects.AllowDrop = true;
		this.TreeObjects.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.TreeObjects.CellEditUseWholeCell = false;
		this.TreeObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[6] { this.olvDocType, this.olvFilial, this.olvDate, this.olvTarget, this.olvTErrors, this.olvTWarnin });
		this.TreeObjects.Cursor = System.Windows.Forms.Cursors.Default;
		this.TreeObjects.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TreeObjects.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TreeObjects.FullRowSelect = true;
		this.TreeObjects.HideSelection = false;
		this.TreeObjects.Location = new System.Drawing.Point(0, 33);
		this.TreeObjects.MenuLabelColumns = "Colunas";
		this.TreeObjects.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.TreeObjects.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.TreeObjects.MenuLabelSelectColumns = "Selecionar colunas...";
		this.TreeObjects.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.TreeObjects.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.TreeObjects.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.TreeObjects.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.TreeObjects.MenuLabelUnsort = "Remover ordenação";
		this.TreeObjects.MultiSelect = false;
		this.TreeObjects.Name = "TreeObjects";
		this.TreeObjects.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
		this.TreeObjects.SelectedBackColor = System.Drawing.Color.CornflowerBlue;
		this.TreeObjects.ShowGroups = false;
		this.TreeObjects.Size = new System.Drawing.Size(456, 531);
		this.TreeObjects.SmallImageList = this.ImageListDocs;
		this.TreeObjects.TabIndex = 11;
		this.TreeObjects.UseAlternatingBackColors = true;
		this.TreeObjects.UseCompatibleStateImageBehavior = false;
		this.TreeObjects.UseTranslucentSelection = true;
		this.TreeObjects.View = System.Windows.Forms.View.Details;
		this.TreeObjects.VirtualMode = true;
		this.TreeObjects.SelectedIndexChanged += new System.EventHandler(TreeObjects_SelectedIndexChanged);
		this.olvDocType.AspectName = "DocType";
		this.olvDocType.Text = "Arquivo Fiscal";
		this.olvDocType.Width = 117;
		this.olvFilial.AspectName = "Filial";
		this.olvFilial.Text = "Empresa";
		this.olvFilial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFilial.Width = 95;
		this.olvDate.AspectName = "DocDate";
		this.olvDate.Text = "Data";
		this.olvDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDate.Width = 70;
		this.olvTarget.AspectName = "Target";
		this.olvTarget.Text = "Finalidade";
		this.olvTErrors.AspectName = "TErrors";
		this.olvTErrors.Text = "Erros";
		this.olvTErrors.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTErrors.Width = 45;
		this.olvTWarnin.AspectName = "TWarnin";
		this.olvTWarnin.Text = "Avisos";
		this.olvTWarnin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvTWarnin.Width = 45;
		this.olvLayout.AspectName = "Layout";
		this.olvLayout.DisplayIndex = 6;
		this.olvLayout.IsVisible = false;
		this.olvLayout.Text = "Layout";
		this.olvLayout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvLayout.Width = 0;
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_button_help.png");
		this.ImageListDocs.Images.SetKeyName(1, "image_error.png");
		this.ImageListDocs.Images.SetKeyName(2, "image_tool_disabled.png");
		this.ImageListDocs.Images.SetKeyName(3, "dfe_confirm.png");
		this.ImageListDocs.Images.SetKeyName(4, "image_updater.ico");
		this.tspObjMenu.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspObjMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.tsbImport, this.toolStripSeparator1, this.tsbAudit, this.toolStripSeparator2, this.tsbDelete, this.toolStripSeparator7, this.toolStripButton1 });
		this.tspObjMenu.Location = new System.Drawing.Point(0, 0);
		this.tspObjMenu.Name = "tspObjMenu";
		this.tspObjMenu.Padding = new System.Windows.Forms.Padding(2);
		this.tspObjMenu.Size = new System.Drawing.Size(456, 33);
		this.tspObjMenu.TabIndex = 0;
		this.tspObjMenu.Text = "toolStrip1";
		this.tsbImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsmEfdIcmsIpi, this.tsmEfdPisCofins });
		this.tsbImport.Image = Monitor.Resources.image_add_object;
		this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbImport.Name = "tsbImport";
		this.tsbImport.Size = new System.Drawing.Size(78, 26);
		this.tsbImport.Text = "Importar";
		this.tsbImport.ToolTipText = "Importar arquivo fiscal no Monitor";
		this.tsmEfdIcmsIpi.Name = "tsmEfdIcmsIpi";
		this.tsmEfdIcmsIpi.Size = new System.Drawing.Size(167, 22);
		this.tsmEfdIcmsIpi.Text = "SPED ICMS/IPI";
		this.tsmEfdIcmsIpi.Click += new System.EventHandler(tsmEfdIcmsIpi_Click);
		this.tsmEfdPisCofins.Enabled = false;
		this.tsmEfdPisCofins.Name = "tsmEfdPisCofins";
		this.tsmEfdPisCofins.Size = new System.Drawing.Size(167, 22);
		this.tsmEfdPisCofins.Text = "SPED Contribuições";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
		this.tsbAudit.Image = Monitor.Resources.image_working;
		this.tsbAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbAudit.Name = "tsbAudit";
		this.tsbAudit.Size = new System.Drawing.Size(62, 26);
		this.tsbAudit.Text = "Auditar";
		this.tsbAudit.ToolTipText = "Executar processo de auditoria";
		this.tsbAudit.Click += new System.EventHandler(tsbAudit_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
		this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.tsbDelete.Image = Monitor.Resources.image_delete_object;
		this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbDelete.Name = "tsbDelete";
		this.tsbDelete.Size = new System.Drawing.Size(23, 26);
		this.tsbDelete.Text = "Excluir";
		this.tsbDelete.ToolTipText = "Excluir arquivo fiscal";
		this.tsbDelete.Click += new System.EventHandler(tsbDelete_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 29);
		this.toolStripButton1.BackColor = System.Drawing.Color.Transparent;
		this.toolStripButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.toolStripButton1.ForeColor = System.Drawing.Color.White;
		this.toolStripButton1.Image = Monitor.Resources.image_form_help;
		this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
		this.toolStripButton1.Size = new System.Drawing.Size(80, 26);
		this.toolStripButton1.Click += new System.EventHandler(btHelp_Click);
		this.splitContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContent.Location = new System.Drawing.Point(0, 0);
		this.splitContent.Name = "splitContent";
		this.splitContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContent.Panel1.Controls.Add(this.lsvTaskList);
		this.splitContent.Panel1.Controls.Add(this.tspTaskMenu);
		this.splitContent.Panel2.Controls.Add(this.lsvDataList);
		this.splitContent.Size = new System.Drawing.Size(565, 564);
		this.splitContent.SplitterDistance = 250;
		this.splitContent.TabIndex = 0;
		this.lsvTaskList.AllColumns.Add(this.olvSelectGroup);
		this.lsvTaskList.AllColumns.Add(this.olvAudTitle);
		this.lsvTaskList.AllColumns.Add(this.olvStaType);
		this.lsvTaskList.AllColumns.Add(this.olvDocPart);
		this.lsvTaskList.AllColumns.Add(this.olvStTotal);
		this.lsvTaskList.AllColumns.Add(this.olvAction01);
		this.lsvTaskList.AllColumns.Add(this.olvAction02);
		this.lsvTaskList.AllColumns.Add(this.olvAudRoleHeader);
		this.lsvTaskList.AllowColumnReorder = true;
		this.lsvTaskList.AllowDrop = true;
		this.lsvTaskList.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.lsvTaskList.CellEditUseWholeCell = false;
		this.lsvTaskList.CheckBoxes = true;
		this.lsvTaskList.CheckedAspectName = "";
		this.lsvTaskList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5] { this.olvSelectGroup, this.olvAudTitle, this.olvDocPart, this.olvStTotal, this.olvAction01 });
		this.lsvTaskList.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvTaskList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lsvTaskList.EmptyListMsgFont = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvTaskList.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvTaskList.FullRowSelect = true;
		this.lsvTaskList.HeaderWordWrap = true;
		this.lsvTaskList.HideSelection = false;
		this.lsvTaskList.IncludeColumnHeadersInCopy = true;
		this.lsvTaskList.Location = new System.Drawing.Point(0, 27);
		this.lsvTaskList.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lsvTaskList.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lsvTaskList.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lsvTaskList.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lsvTaskList.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lsvTaskList.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lsvTaskList.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lsvTaskList.MenuLabelUnsort = "Remover ordenação";
		this.lsvTaskList.Name = "lsvTaskList";
		this.lsvTaskList.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
		this.lsvTaskList.ShowCommandMenuOnRightClick = true;
		this.lsvTaskList.ShowGroups = false;
		this.lsvTaskList.ShowHeaderInAllViews = false;
		this.lsvTaskList.ShowItemToolTips = true;
		this.lsvTaskList.Size = new System.Drawing.Size(565, 223);
		this.lsvTaskList.SmallImageList = this.ImageListDocs;
		this.lsvTaskList.SortGroupItemsByPrimaryColumn = false;
		this.lsvTaskList.TabIndex = 38;
		this.lsvTaskList.UseAlternatingBackColors = true;
		this.lsvTaskList.UseCellFormatEvents = true;
		this.lsvTaskList.UseCompatibleStateImageBehavior = false;
		this.lsvTaskList.UseFilterIndicator = true;
		this.lsvTaskList.UseFiltering = true;
		this.lsvTaskList.UseHotItem = true;
		this.lsvTaskList.View = System.Windows.Forms.View.Details;
		this.lsvTaskList.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(HeaderCheckBoxChangingGroup);
		this.lsvTaskList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvTaskList_ItemChecked);
		this.lsvTaskList.MouseClick += new System.Windows.Forms.MouseEventHandler(lsvTaskList_MouseClick);
		this.lsvTaskList.Resize += new System.EventHandler(lsvTaskList_Resize);
		this.olvSelectGroup.AspectName = "";
		this.olvSelectGroup.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelectGroup.Groupable = false;
		this.olvSelectGroup.HeaderCheckBox = true;
		this.olvSelectGroup.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelectGroup.Sortable = false;
		this.olvSelectGroup.Text = "";
		this.olvSelectGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelectGroup.Width = 33;
		this.olvAudTitle.AspectName = "AudTitle";
		this.olvAudTitle.Text = "Regra";
		this.olvAudTitle.Width = 260;
		this.olvStaType.AspectName = "StaType";
		this.olvStaType.DisplayIndex = 1;
		this.olvStaType.IsTileViewColumn = true;
		this.olvStaType.IsVisible = false;
		this.olvStaType.MinimumWidth = 0;
		this.olvStaType.Text = "Status";
		this.olvStaType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStaType.ToolTipText = "";
		this.olvStaType.Width = 50;
		this.olvDocPart.AspectName = "DocPart";
		this.olvDocPart.MinimumWidth = 60;
		this.olvDocPart.Text = "Local";
		this.olvDocPart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocPart.ToolTipText = "";
		this.olvStTotal.AspectName = "StTotal";
		this.olvStTotal.MinimumWidth = 50;
		this.olvStTotal.Text = "Total";
		this.olvStTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStTotal.Width = 50;
		this.olvAction01.AspectName = "Action01";
		this.olvAction01.MinimumWidth = 110;
		this.olvAction01.Text = "Ação";
		this.olvAction01.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAction01.Width = 110;
		this.olvAction02.AspectName = "Action02";
		this.olvAction02.DisplayIndex = 5;
		this.olvAction02.IsVisible = false;
		this.olvAction02.MinimumWidth = 110;
		this.olvAction02.Text = "Ação";
		this.olvAction02.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvAction02.Width = 110;
		this.olvAudRoleHeader.AspectName = "AudRole";
		this.olvAudRoleHeader.DisplayIndex = 6;
		this.olvAudRoleHeader.IsVisible = false;
		this.olvAudRoleHeader.Text = "ID";
		this.olvAudRoleHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tspTaskMenu.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tspTaskMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator3, this.tsbSucess, this.toolStripSeparator6, this.tsbWarning, this.toolStripSeparator4, this.tsbError, this.toolStripSeparator5 });
		this.tspTaskMenu.Location = new System.Drawing.Point(0, 0);
		this.tspTaskMenu.Name = "tspTaskMenu";
		this.tspTaskMenu.Padding = new System.Windows.Forms.Padding(2);
		this.tspTaskMenu.Size = new System.Drawing.Size(565, 27);
		this.tspTaskMenu.TabIndex = 39;
		this.tspTaskMenu.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
		this.tsbSucess.Image = Monitor.Resources.dfe_confirm;
		this.tsbSucess.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSucess.Name = "tsbSucess";
		this.tsbSucess.Size = new System.Drawing.Size(66, 20);
		this.tsbSucess.Text = "Sucesso";
		this.tsbSucess.Click += new System.EventHandler(tsbSucess_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
		this.tsbWarning.Image = Monitor.Resources.image_warning;
		this.tsbWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbWarning.Name = "tsbWarning";
		this.tsbWarning.Size = new System.Drawing.Size(58, 20);
		this.tsbWarning.Text = "Avisos";
		this.tsbWarning.Click += new System.EventHandler(tsbWarning_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
		this.tsbError.Image = Monitor.Resources.image_error;
		this.tsbError.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbError.Name = "tsbError";
		this.tsbError.Size = new System.Drawing.Size(52, 20);
		this.tsbError.Text = "Erros";
		this.tsbError.Click += new System.EventHandler(tsbError_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
		this.lsvDataList.AllColumns.Add(this.olvSelect);
		this.lsvDataList.AllColumns.Add(this.olvErrorTitle);
		this.lsvDataList.AllColumns.Add(this.olvEmitida);
		this.lsvDataList.AllColumns.Add(this.olvDocLine);
		this.lsvDataList.AllColumns.Add(this.olvDocNum);
		this.lsvDataList.AllColumns.Add(this.olvDocSerie);
		this.lsvDataList.AllColumns.Add(this.olvDocModel);
		this.lsvDataList.AllColumns.Add(this.olvDocValue);
		this.lsvDataList.AllColumns.Add(this.olvFisValue);
		this.lsvDataList.AllColumns.Add(this.olvDocKey);
		this.lsvDataList.AllColumns.Add(this.olvAudRole);
		this.lsvDataList.AllowColumnReorder = true;
		this.lsvDataList.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.lsvDataList.CellEditUseWholeCell = false;
		this.lsvDataList.CheckBoxes = true;
		this.lsvDataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[10] { this.olvSelect, this.olvErrorTitle, this.olvEmitida, this.olvDocLine, this.olvDocNum, this.olvDocSerie, this.olvDocModel, this.olvDocValue, this.olvFisValue, this.olvDocKey });
		this.lsvDataList.ContextMenuStrip = this.contextMenuDocs;
		this.lsvDataList.Cursor = System.Windows.Forms.Cursors.Default;
		this.lsvDataList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lsvDataList.EmptyListMsg = "";
		this.lsvDataList.EmptyListMsgFont = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvDataList.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.lsvDataList.FullRowSelect = true;
		this.lsvDataList.HideSelection = false;
		this.lsvDataList.IncludeColumnHeadersInCopy = true;
		this.lsvDataList.Location = new System.Drawing.Point(0, 0);
		this.lsvDataList.MenuLabelColumns = "Colunas";
		this.lsvDataList.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lsvDataList.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lsvDataList.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lsvDataList.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lsvDataList.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lsvDataList.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lsvDataList.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lsvDataList.MenuLabelUnsort = "Remover ordenação";
		this.lsvDataList.Name = "lsvDataList";
		this.lsvDataList.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.ModelDialog;
		this.lsvDataList.SelectedBackColor = System.Drawing.Color.LightBlue;
		this.lsvDataList.SelectedForeColor = System.Drawing.Color.Black;
		this.lsvDataList.ShowCommandMenuOnRightClick = true;
		this.lsvDataList.ShowGroups = false;
		this.lsvDataList.ShowImagesOnSubItems = true;
		this.lsvDataList.ShowItemCountOnGroups = true;
		this.lsvDataList.Size = new System.Drawing.Size(565, 310);
		this.lsvDataList.SmallImageList = this.ImageListDocs;
		this.lsvDataList.SortGroupItemsByPrimaryColumn = false;
		this.lsvDataList.SpaceBetweenGroups = 5;
		this.lsvDataList.TabIndex = 139;
		this.lsvDataList.TintSortColumn = true;
		this.lsvDataList.UseAlternatingBackColors = true;
		this.lsvDataList.UseCellFormatEvents = true;
		this.lsvDataList.UseCompatibleStateImageBehavior = false;
		this.lsvDataList.UseFilterIndicator = true;
		this.lsvDataList.UseFiltering = true;
		this.lsvDataList.UseHotControls = false;
		this.lsvDataList.UseHyperlinks = true;
		this.lsvDataList.UseTranslucentSelection = true;
		this.lsvDataList.View = System.Windows.Forms.View.Details;
		this.lsvDataList.VirtualMode = true;
		this.lsvDataList.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(lsvDataList_FormatRow);
		this.lsvDataList.HeaderCheckBoxChanging += new System.EventHandler<BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs>(lsvDataList_HeaderCheckBoxChanging);
		this.lsvDataList.HyperlinkClicked += new System.EventHandler<BrightIdeasSoftware.HyperlinkClickedEventArgs>(lsvDataList_HyperlinkClicked);
		this.lsvDataList.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(lsvDataList_ItemsChanged);
		this.lsvDataList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lsvDataList_ItemChecked);
		this.lsvDataList.DoubleClick += new System.EventHandler(lsvDataList_DoubleClick);
		this.lsvDataList.KeyUp += new System.Windows.Forms.KeyEventHandler(lsvDataList_KeyUp);
		this.olvSelect.AspectName = "";
		this.olvSelect.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelect.Groupable = false;
		this.olvSelect.HeaderCheckBox = true;
		this.olvSelect.HeaderCheckBoxUpdatesRowCheckBoxes = false;
		this.olvSelect.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Sortable = false;
		this.olvSelect.Text = "";
		this.olvSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelect.Width = 33;
		this.olvErrorTitle.AspectName = "AudTitle";
		this.olvErrorTitle.Text = "Erros";
		this.olvErrorTitle.Width = 200;
		this.olvEmitida.AspectName = "Emitida";
		this.olvEmitida.Text = "Emissor";
		this.olvEmitida.Width = 75;
		this.olvDocLine.AspectName = "LinePos";
		this.olvDocLine.Text = "Linha";
		this.olvDocLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocNum.AspectName = "DocNum";
		this.olvDocNum.Hyperlink = true;
		this.olvDocNum.Text = "Número";
		this.olvDocNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocNum.Width = 90;
		this.olvDocSerie.AspectName = "DocSerie";
		this.olvDocSerie.Text = "Série";
		this.olvDocSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocSerie.Width = 50;
		this.olvDocModel.AspectName = "DocModel";
		this.olvDocModel.Text = "Tipo";
		this.olvDocModel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocModel.Width = 50;
		this.olvDocValue.AspectName = "DocValue";
		this.olvDocValue.Text = "Xml";
		this.olvDocValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvFisValue.AspectName = "FisValue";
		this.olvFisValue.Text = "SPED";
		this.olvFisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocKey.AspectName = "DocKey";
		this.olvDocKey.Text = "Chave";
		this.olvDocKey.Width = 300;
		this.olvAudRole.AspectName = "AudRole";
		this.olvAudRole.IsVisible = false;
		this.olvAudRole.Text = "Regra";
		this.contextMenuDocs.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(994, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 3;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.pnMarketContent.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.pnMarketContent.Controls.Add(this.btSalesContact);
		this.pnMarketContent.Controls.Add(this.label4);
		this.pnMarketContent.Controls.Add(this.lknAction);
		this.pnMarketContent.Controls.Add(this.label2);
		this.pnMarketContent.Controls.Add(this.lbText02);
		this.pnMarketContent.Controls.Add(this.label1);
		this.pnMarketContent.Controls.Add(this.btSalesAction);
		this.pnMarketContent.Controls.Add(this.label3);
		this.pnMarketContent.Controls.Add(this.picWarning);
		this.pnMarketContent.Controls.Add(this.lbTitle03);
		this.pnMarketContent.Controls.Add(this.lbText01);
		this.pnMarketContent.Controls.Add(this.lbTitle01);
		this.pnMarketContent.Controls.Add(this.lbTitle02);
		this.pnMarketContent.Location = new System.Drawing.Point(235, 27);
		this.pnMarketContent.Name = "pnMarketContent";
		this.pnMarketContent.Size = new System.Drawing.Size(566, 368);
		this.pnMarketContent.TabIndex = 195;
		this.btSalesContact.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesContact.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesContact.ForeColor = System.Drawing.Color.White;
		this.btSalesContact.Location = new System.Drawing.Point(89, 321);
		this.btSalesContact.Name = "btSalesContact";
		this.btSalesContact.Size = new System.Drawing.Size(188, 28);
		this.btSalesContact.TabIndex = 214;
		this.btSalesContact.Text = "Solicitar Contato";
		this.btSalesContact.UseVisualStyleBackColor = false;
		this.btSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label4.Location = new System.Drawing.Point(18, 284);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(88, 17);
		this.label4.TabIndex = 213;
		this.label4.Text = "Saiba mais:";
		this.lknAction.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lknAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknAction.Location = new System.Drawing.Point(112, 284);
		this.lknAction.Name = "lknAction";
		this.lknAction.Size = new System.Drawing.Size(436, 16);
		this.lknAction.TabIndex = 212;
		this.lknAction.TabStop = true;
		this.lknAction.Text = "Funcionamento do Auditor de SPED Fiscal e suas regras de validação";
		this.lknAction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lknAction_LinkClicked);
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(18, 239);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(530, 33);
		this.label2.TabIndex = 211;
		this.label2.Text = "Veja neste artigo como esta funcionalidade pode facilitar o seu trabalho e todas as Regras de validação do Auditor.";
		this.lbText02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText02.Location = new System.Drawing.Point(18, 193);
		this.lbText02.Name = "lbText02";
		this.lbText02.Size = new System.Drawing.Size(530, 33);
		this.lbText02.TabIndex = 210;
		this.lbText02.Text = "Quando o usuário importa o arquivo EFD (SPED Fiscal), o sistema cruza todos os seus registros com a base de dados de XMLs.";
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(18, 172);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(530, 17);
		this.label1.TabIndex = 209;
		this.label1.Text = "Princípio de funcionamento:";
		this.btSalesAction.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btSalesAction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesAction.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesAction.ForeColor = System.Drawing.Color.White;
		this.btSalesAction.Location = new System.Drawing.Point(300, 321);
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
		this.picWarning.Image = Monitor.Resources.image_help;
		this.picWarning.Location = new System.Drawing.Point(20, 10);
		this.picWarning.Name = "picWarning";
		this.picWarning.Size = new System.Drawing.Size(20, 20);
		this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picWarning.TabIndex = 205;
		this.picWarning.TabStop = false;
		this.lbTitle03.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle03.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle03.Location = new System.Drawing.Point(17, 183);
		this.lbTitle03.Name = "lbTitle03";
		this.lbTitle03.Size = new System.Drawing.Size(530, 22);
		this.lbTitle03.TabIndex = 196;
		this.lbText01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbText01.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbText01.Location = new System.Drawing.Point(18, 76);
		this.lbText01.Name = "lbText01";
		this.lbText01.Size = new System.Drawing.Size(530, 86);
		this.lbText01.TabIndex = 195;
		this.lbText01.Text = resources.GetString("lbText01.Text");
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "Auditor de SPED Fiscal é ativado nos \r\nplanos Avançado e Enterprise.";
		this.lbTitle01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbTitle02.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle02.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lbTitle02.Location = new System.Drawing.Point(18, 55);
		this.lbTitle02.Name = "lbTitle02";
		this.lbTitle02.Size = new System.Drawing.Size(530, 17);
		this.lbTitle02.TabIndex = 194;
		this.lbTitle02.Text = "Problemas resolvidos:";
		this.lknClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lknClose.AutoSize = true;
		this.lknClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lknClose.Location = new System.Drawing.Point(841, 393);
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
		this.pnMarket.Location = new System.Drawing.Point(0, 147);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(1025, 417);
		this.pnMarket.TabIndex = 21;
		this.pnMarket.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1025, 564);
		base.Controls.Add(this.pnMarket);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabAuditor";
		this.Text = "Auditor de SPED Fiscal";
		base.Load += new System.EventHandler(frmTabAuditor_Load);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(btClose_HelpRequested);
		this.pnContent.ResumeLayout(false);
		this.splitAuditor.Panel1.ResumeLayout(false);
		this.splitAuditor.Panel1.PerformLayout();
		this.splitAuditor.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitAuditor).EndInit();
		this.splitAuditor.ResumeLayout(false);
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		((System.ComponentModel.ISupportInitialize)this.TreeObjects).EndInit();
		this.tspObjMenu.ResumeLayout(false);
		this.tspObjMenu.PerformLayout();
		this.splitContent.Panel1.ResumeLayout(false);
		this.splitContent.Panel1.PerformLayout();
		this.splitContent.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContent).EndInit();
		this.splitContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.lsvTaskList).EndInit();
		this.tspTaskMenu.ResumeLayout(false);
		this.tspTaskMenu.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.lsvDataList).EndInit();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		base.ResumeLayout(false);
	}
}
