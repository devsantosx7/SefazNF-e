using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabDocNFePendSND : Form
{
	private class ObjDoc
	{
		public string FilId { get; set; }

		public string DocId { get; set; }
	}

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private clsSrvTabDocNFePendSND _SqlTabData = new clsSrvTabDocNFePendSND();

	private Hashtable _HasColors = new Hashtable();

	private Hashtable _TagHsList = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsValidatorService varclsValidator = new clsValidatorService();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private bool _SelectedAllItens;

	private bool _ColumnsLoaded;

	private decimal _TotalValue;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private string _consMarketScreenUser = string.Empty;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private IContainer components;

	private ContextMenuStrip contextMenuDocs;

	private Panel pnContent;

	private ListView lsvTabDocNFePendSND;

	private ColumnHeader clInHasXml;

	private ColumnHeader clInTipo;

	private ColumnHeader clInTag;

	private ColumnHeader clInNum;

	private ColumnHeader clInSerie;

	private ColumnHeader clInDtAut;

	private ColumnHeader clInValor;

	private ColumnHeader clInReason;

	private ColumnHeader clInCNPJ;

	private ColumnHeader clInNome;

	private ColumnHeader clInChave;

	private ColumnHeader clInFilial;

	private ProgressBar tlpProgress;

	private ColumnHeader clInCFOP;

	private ColumnHeader clInNCM;

	private ColumnHeader clInError;

	private ColumnHeader clInCancel;

	private ColumnHeader clInManifest;

	private ColumnHeader clDocNote;

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

	private ImageList ImageListDocs;

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

	public frmTabDocNFePendSND(string pTitle, Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		Text = pTitle;
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
		_consMarketScreenUser = base.Name + "-market-close";
	}

	private async void frmTabDocNFePendSND_Load(object sender, EventArgs e)
	{
		_StateList = await new clsDataEstado().funcGetListAsync(pWthAN: false);
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		funcLoadTagsAsync();
		_ColumnsLoaded = false;
		clsFunction.funcSetColumnsConfiguration(this, lsvTabDocNFePendSND);
		_ColumnsLoaded = true;
		lsvTabDocNFePendSND.Columns[0].ImageIndex = 16;
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
			lsvTabDocNFePendSND.Dock = DockStyle.None;
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
			lsvTabDocNFePendSND.Dock = DockStyle.Fill;
			pnMarket.Visible = false;
			base.Controls.Remove(pnMarket);
		}
		else
		{
			pnMarket.Visible = true;
		}
	}

	public async Task<bool> funcLoadTagsAsync()
	{
		return await clsMonGeral.funcLoadTagsAsync(_HasColors, _TagHsList, contextMenuDocs, tsmCopyDocKey_Click, null, tsbTagCode_Click, tsbDocNote_Click, null, tsbRemoveDocNote_Click);
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
		List<ListViewItem> varObjList = funcGetObjList(pFocused, pChecked);
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
		await funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	private async void tsbRemoveDocNote_Click(object sender, EventArgs e)
	{
		funcRemoveDocNoteAsync(pFocused: true, pChecked: true);
	}

	public async Task<bool> funcRemoveDocNoteAsync(bool pFocused, bool pChecked)
	{
		List<ListViewItem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		await funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	private async void tsbDocNote_Click(object sender, EventArgs e)
	{
		await funcSetDocNoteAsync(pFocused: true, pChecked: true);
	}

	public async Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked)
	{
		List<ListViewItem> varObjList = funcGetObjList(pFocused: true, pChecked: true);
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
		await funcRefreshListAsync(varDocList, varObjList);
		return true;
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturn = new clsReturn();
		string varSqlQuery = await _SqlTabData.funcGetSqlStrSelectAsync(pclsDataFilter);
		if (string.IsNullOrEmpty(varSqlQuery))
		{
			return varclsReturn;
		}
		long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
		if (_IsLocked)
		{
			varPageSize = 4L;
		}
		List<Document> varclsDocList = await new clsDataDoc().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		setTabDataValuesBasedOnFilter();
		return await funcShowDataAsync(pclsDataFilter, varclsDocList);
	}

	private async Task<clsReturn> funcShowDataAsync(clsDataFilter pclsFilter, List<Document> pDocList)
	{
		_ = string.Empty;
		_ = string.Empty;
		clsReturn varclsReturn = new clsReturn();
		tlpProgress.Visible = true;
		tlpProgress.Minimum = 0;
		tlpProgress.Maximum = pDocList.Count;
		tlpProgress.Value = 0;
		Application.DoEvents();
		lsvTabDocNFePendSND.BeginUpdate();
		lsvTabDocNFePendSND.SuspendLayout();
		lsvTabDocNFePendSND.Items.Clear();
		int varCounter = 0;
		string varGroupByText = "FISCAL_IO_STARTER";
		ListViewGroup varListGroup = new ListViewGroup();
		if (pclsFilter.GroupByDisable || _IsLocked)
		{
			lsvTabDocNFePendSND.ShowGroups = false;
			lsvTabDocNFePendSND.Groups.Clear();
		}
		else
		{
			lsvTabDocNFePendSND.ShowGroups = true;
		}
		foreach (Document varclsDocument in pDocList)
		{
			try
			{
				varCounter++;
				if (varCounter % 200 == 0)
				{
					tlpProgress.Value = varCounter;
				}
			}
			catch
			{
			}
			if (pclsFilter.GroupByPartner)
			{
				string varPartnerName;
				string varPartnerIdnt;
				if (string.IsNullOrEmpty(varclsDocument.Emitida))
				{
					varPartnerIdnt = clsFunction.funcGetValue(varclsDocument.EmitID);
					varPartnerName = clsFunction.funcGetValue(varclsDocument.EmitNome);
				}
				else
				{
					varPartnerIdnt = clsFunction.funcGetValue(varclsDocument.TomaID);
					varPartnerName = clsFunction.funcGetValue(varclsDocument.TomaNome);
				}
				varPartnerIdnt = clsFunction.funcFormatDoc(varPartnerIdnt);
				if (!varGroupByText.Equals(varPartnerIdnt))
				{
					string varGroupHeaderText = varPartnerName + " [ " + varPartnerIdnt + " ] ";
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varPartnerIdnt, varGroupHeaderText);
				}
				varGroupByText = varPartnerIdnt;
			}
			else if (pclsFilter.GroupByDate)
			{
				if (!varGroupByText.Equals(varclsDocument.DtAut))
				{
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varclsDocument.DtAut, varclsDocument.DtAut);
				}
				varGroupByText = varclsDocument.DtAut;
			}
			else if (pclsFilter.GroupByYearMonth)
			{
				string varAnoMes = varclsDocument.DtAut.Substring(0, 7);
				if (!varGroupByText.Equals(varAnoMes))
				{
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varAnoMes, varAnoMes);
				}
				varGroupByText = varAnoMes;
			}
			else if (pclsFilter.GroupByState)
			{
				if (!varGroupByText.Equals(varclsDocument.cUF))
				{
					string varGroupHeaderText2 = varclsDocument.cUF;
					Estado varEstado = _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varclsDocument.cUF));
					if (varEstado != null)
					{
						varGroupHeaderText2 = varEstado.Nome + " [ " + varGroupHeaderText2 + " ] ";
					}
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varclsDocument.cUF, varGroupHeaderText2);
				}
				varGroupByText = varclsDocument.cUF;
			}
			else if (pclsFilter.GroupByModel)
			{
				string varModel = clsFunction.funcGetDocType(varclsDocument.Model);
				if (!varGroupByText.Equals(varModel))
				{
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varModel, varModel);
				}
				varGroupByText = varModel;
			}
			else if (pclsFilter.GroupByTpDoc)
			{
				string varTipoDoc = _clsDFeCodes.funcGetTipoDoc(varclsDocument.Model, varclsDocument.TpDoc);
				if (!varGroupByText.Equals(varTipoDoc))
				{
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varTipoDoc, varTipoDoc);
				}
				varGroupByText = varTipoDoc;
			}
			else if (pclsFilter.GroupByNatOper)
			{
				string varNatOper = clsFunction.funcGetValue(varclsDocument.NatOper).ToUpper();
				if (!varGroupByText.Equals(varNatOper))
				{
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varNatOper, varNatOper);
				}
				varGroupByText = varNatOper;
			}
			else if (pclsFilter.GroupByCFOP)
			{
				string varDocCFOP = clsFunction.funcGetValue(varclsDocument.CFOPList).ToUpper();
				if (!varGroupByText.Equals(varDocCFOP))
				{
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varDocCFOP, varDocCFOP);
				}
				varGroupByText = varDocCFOP;
			}
			else if (pclsFilter.GroupByFilial)
			{
				string varFilialId = varclsDocument.Filial;
				if (!varGroupByText.Equals(varFilialId))
				{
					FilialView obj2 = await new clsDataFilial().funcGetItemByKeyAsync(varFilialId);
					_ = string.Empty;
					string varGroupHeaderText3 = string.Concat(str2: clsFunction.funcFormatDoc(obj2.CNPJView), str0: obj2.NomeView, str1: " [ ", str3: " ] ");
					varListGroup = lsvTabDocNFePendSND.Groups.Add(varFilialId, varGroupHeaderText3);
				}
				varGroupByText = varFilialId;
			}
			ListViewItem varListItem = new ListViewItem();
			varListItem.Group = varListGroup;
			varListItem = await funcFillItemAsync(varListItem, varclsDocument);
			lsvTabDocNFePendSND.Items.Add(varListItem);
		}
		lsvTabDocNFePendSND.EndUpdate();
		lsvTabDocNFePendSND.ResumeLayout();
		tlpProgress.Visible = false;
		tlpProgress.Maximum = 0;
		tlpProgress.Value = 0;
		Application.DoEvents();
		return varclsReturn;
	}

	private async Task<ListViewItem> funcFillItemAsync(ListViewItem pListItem, Document pclsDocument)
	{
		_ = string.Empty;
		pListItem.SubItems.Clear();
		pListItem.ImageIndex = clsScreenGeral.funcGetDocXmlIcon(pclsDocument, _HasNFSeFeatEnabled, _HasCFeFeatEnabled);
		string varDocType = clsFunction.funcGetDocType(pclsDocument.Model);
		pListItem.SubItems.Add(varDocType);
		pListItem.Tag = new ObjDoc
		{
			FilId = pclsDocument.Filial,
			DocId = pclsDocument.Chave
		};
		ListViewItem.ListViewSubItem varSubItemTag = new ListViewItem.ListViewSubItem();
		if (!string.IsNullOrEmpty(pclsDocument.Tag))
		{
			pListItem.UseItemStyleForSubItems = false;
			varSubItemTag.Text = (string)_TagHsList[pclsDocument.Tag];
			string varColorName = (string)_HasColors[pclsDocument.Tag];
			varSubItemTag.BackColor = ColorTranslator.FromHtml(varColorName);
			pListItem.SubItems.Add(varSubItemTag);
		}
		else
		{
			pListItem.SubItems.Add("");
		}
		pListItem.SubItems.Add(pclsDocument.Num);
		pListItem.SubItems.Add(pclsDocument.Serie);
		pListItem.SubItems.Add(pclsDocument.DtAut);
		ListViewItem.ListViewSubItem varSubItemTotal = new ListViewItem.ListViewSubItem
		{
			Name = "Valor",
			Text = pclsDocument.Valor
		};
		pListItem.SubItems.Add(varSubItemTotal);
		string varEventDesc = _clsDFeCodes.GetEventDesc(pclsDocument.Model, pclsDocument.ManifCode);
		pListItem.SubItems.Add(varEventDesc);
		pListItem.SubItems.Add(pclsDocument.Canceled).Tag = "clCancel";
		if (string.IsNullOrEmpty(pclsDocument.Emitida))
		{
			pListItem.SubItems.Add(clsFunction.funcFormatDoc(pclsDocument.EmitID));
			pListItem.SubItems.Add(clsFunction.funcGetValue(pclsDocument.EmitNome));
		}
		else
		{
			pListItem.SubItems.Add(clsFunction.funcFormatDoc(pclsDocument.TomaID));
			pListItem.SubItems.Add(clsFunction.funcGetValue(pclsDocument.TomaNome));
		}
		string varAnalizerStatus = clsAbsAnalizer.consNFE_TRANSPORT_PENDEN;
		ListViewItem.ListViewSubItem varSubItemReason = new ListViewItem.ListViewSubItem();
		DocStatus varclsDocStatus = await new clsDataDocStatus().funcGetItemByKeyStatAsync(pclsDocument.Chave, varAnalizerStatus);
		if (varclsDocStatus != null)
		{
			varSubItemReason.Text = varclsDocStatus.Reason;
		}
		pListItem.UseItemStyleForSubItems = false;
		varSubItemReason.BackColor = Color.LightBlue;
		pListItem.SubItems.Add(varSubItemReason);
		pListItem.SubItems.Add(varDocType + "-" + pclsDocument.Chave);
		pListItem.SubItems.Add(clsFunction.funcFormatDoc(pclsDocument.Filial));
		pListItem.SubItems.Add(pclsDocument.CFOPList);
		pListItem.SubItems.Add(pclsDocument.NCMList);
		string varErrorDesc = varclsValidator.funcGetDesc(pclsDocument);
		pListItem.SubItems.Add(varErrorDesc);
		pListItem.ToolTipText = varErrorDesc;
		pListItem.SubItems.Add(pclsDocument.DocNote);
		if (!string.IsNullOrEmpty(pclsDocument.Canceled))
		{
			pListItem.BackColor = Color.PapayaWhip;
		}
		else if (!string.IsNullOrEmpty(pclsDocument.HasCancelEvent))
		{
			pListItem.BackColor = Color.PapayaWhip;
		}
		else
		{
			pListItem.BackColor = Color.White;
		}
		return pListItem;
	}

	private async void tsmCopyDocKey_Click(object sender, EventArgs e)
	{
		await funcCopyDocKeyAsync();
	}

	private async Task<bool> funcCopyDocKeyAsync()
	{
		if (lsvTabDocNFePendSND.FocusedItem == null)
		{
			return false;
		}
		ObjDoc varObjDoc = (ObjDoc)lsvTabDocNFePendSND.FocusedItem.Tag;
		if (varObjDoc == null)
		{
			return false;
		}
		return await clsScreenGeral.funcCopyDocKeyAsync(varObjDoc.DocId);
	}

	private async void lstView_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode.Equals(Keys.C))
		{
			await funcCopyDocKeyAsync();
		}
	}

	public void funcSelectAllItens()
	{
		foreach (ListViewItem varListItem in lsvTabDocNFePendSND.Items)
		{
			if (_SelectedAllItens)
			{
				varListItem.Checked = false;
			}
			else
			{
				varListItem.Checked = true;
			}
		}
		if (_SelectedAllItens)
		{
			_SelectedAllItens = false;
		}
		else
		{
			_SelectedAllItens = true;
		}
	}

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportToCsvAsync(lsvTabDocNFePendSND, onProgressChange);
	}

	private List<ListViewItem> funcGetObjList(bool pFocused, bool pChecked)
	{
		return clsFunction.funcGetListViewItens(lsvTabDocNFePendSND, pAllItens: false, pFocused, pChecked);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<ListViewItem> varObjList = funcGetObjList(pFocused, pChecked);
		return await funcGetDocListAsync(varObjList, pSyncFromDbaFirst);
	}

	private async Task<List<Document>> funcGetDocListAsync(List<ListViewItem> pObjList, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = new List<Document>();
		foreach (ListViewItem pObj in pObjList)
		{
			ObjDoc varObjDocView = (ObjDoc)pObj.Tag;
			if (varObjDocView != null)
			{
				Document varclsDoc = new Document
				{
					Filial = varObjDocView.FilId,
					Chave = varObjDocView.DocId
				};
				varDocList.Add(varclsDoc);
			}
		}
		if (pSyncFromDbaFirst)
		{
			varDocList = await _clsDataDoc.funcGetSyncListByListAsync(varDocList);
		}
		return varDocList;
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		List<ListViewItem> varObjList = funcGetObjList(pFocused, pChecked);
		await funcRefreshListAsync(await funcGetDocListAsync(varObjList, pSyncFromDbaFirst: true), varObjList);
		return true;
	}

	private async Task<bool> funcRefreshListAsync(List<Document> pDocList, List<ListViewItem> pViewList)
	{
		foreach (ListViewItem varListItem in pViewList)
		{
			ObjDoc varObjDocView = (ObjDoc)varListItem.Tag;
			if (varObjDocView != null)
			{
				Document varclsDocView = pDocList.FirstOrDefault((Document r) => clsFunction.IsEqual(r.Filial, varObjDocView.FilId) && clsFunction.IsEqual(r.Chave, varObjDocView.DocId));
				if (varclsDocView != null)
				{
					await funcFillItemAsync(varListItem, varclsDocView);
				}
			}
		}
		return true;
	}

	public int funcGetTotalDocs()
	{
		return lsvTabDocNFePendSND.Items.Count;
	}

	public decimal funcGetTotalValue()
	{
		return _TotalValue;
	}

	private void lstView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		ListView varListView = (ListView)sender;
		if (varListView != null)
		{
			int varColumnIndex = e.ColumnIndex;
			clsFunction.funcSetRegisterValue($"{base.Name}-{varListView.Name}-Index{varColumnIndex}", varListView.Columns[varColumnIndex].Width.ToString(), pGlobal: false);
		}
	}

	private void lstView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		ListView varListView = (ListView)sender;
		if (varListView != null)
		{
			int varColumnIndex = e.Header.Index;
			clsFunction.funcSetRegisterValue($"{base.Name}-{varListView.Name}-Order{varColumnIndex}", e.NewDisplayIndex.ToString(), pGlobal: false);
		}
	}

	private void lstView_ItemChecked(object sender, ItemCheckedEventArgs e)
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
		setTabDataValuesBasedOnFilter();
	}

	private void lstView_DoubleClick(object sender, EventArgs e)
	{
		funcShowDocViewer(pShowPDF: true, pShowXML: false);
	}

	private async void funcShowDocViewer(bool pShowPDF, bool pShowXML)
	{
		await clsMonGeral.funcDocViewerAsync(this, await funcGetDocListAsync(pFocused: true, pChecked: true, pSyncFromDbaFirst: true), pShowPDF, pShowXML);
	}

	private void lstView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		if (e.Column == 0)
		{
			funcSelectAllItens();
		}
	}

	private void setTabDataValuesBasedOnFilter()
	{
		if (lsvTabDocNFePendSND != null)
		{
			IList varObjectsOnView = lsvTabDocNFePendSND.CheckedItems;
			if (varObjectsOnView.Count <= 0)
			{
				varObjectsOnView = lsvTabDocNFePendSND.Items;
			}
			_TotalValue = varObjectsOnView.OfType<ListViewItem>().Sum((ListViewItem r) => clsFunction.funcConvStrToDec(r.SubItems["Valor"].Text));
			EventTabManagerEventArgs varArguments = new EventTabManagerEventArgs();
			varArguments.TotalValue = _TotalValue;
			varArguments.TotalQuant = varObjectsOnView.Count;
			OnEventTabManager(varArguments);
		}
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		lsvTabDocNFePendSND.Dock = DockStyle.Fill;
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
	}

	private async void lknClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		lsvTabDocNFePendSND.Dock = DockStyle.Fill;
		pnMarket.Visible = false;
		base.Controls.Remove(pnMarket);
		await _clsDataParam.funcSetAsync(_consMarketScreenUser, "X");
	}

	private void lknAction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallTipReportNFePendDeliveryAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabDocNFePendSND));
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.pnContent = new System.Windows.Forms.Panel();
		this.tlpProgress = new System.Windows.Forms.ProgressBar();
		this.lsvTabDocNFePendSND = new System.Windows.Forms.ListView();
		this.clInHasXml = new System.Windows.Forms.ColumnHeader();
		this.clInTipo = new System.Windows.Forms.ColumnHeader();
		this.clInTag = new System.Windows.Forms.ColumnHeader();
		this.clInNum = new System.Windows.Forms.ColumnHeader();
		this.clInSerie = new System.Windows.Forms.ColumnHeader();
		this.clInDtAut = new System.Windows.Forms.ColumnHeader();
		this.clInValor = new System.Windows.Forms.ColumnHeader();
		this.clInManifest = new System.Windows.Forms.ColumnHeader();
		this.clInCancel = new System.Windows.Forms.ColumnHeader();
		this.clInCNPJ = new System.Windows.Forms.ColumnHeader();
		this.clInNome = new System.Windows.Forms.ColumnHeader();
		this.clInReason = new System.Windows.Forms.ColumnHeader();
		this.clInChave = new System.Windows.Forms.ColumnHeader();
		this.clInFilial = new System.Windows.Forms.ColumnHeader();
		this.clInCFOP = new System.Windows.Forms.ColumnHeader();
		this.clInNCM = new System.Windows.Forms.ColumnHeader();
		this.clInError = new System.Windows.Forms.ColumnHeader();
		this.clDocNote = new System.Windows.Forms.ColumnHeader();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.pnMarket = new System.Windows.Forms.Panel();
		this.lknClose = new System.Windows.Forms.LinkLabel();
		this.pnMarketContent = new System.Windows.Forms.Panel();
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
		this.btSalesContact = new System.Windows.Forms.Button();
		this.pnContent.SuspendLayout();
		this.pnMarket.SuspendLayout();
		this.pnMarketContent.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picWarning).BeginInit();
		base.SuspendLayout();
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(61, 4);
		this.pnContent.Controls.Add(this.tlpProgress);
		this.pnContent.Controls.Add(this.lsvTabDocNFePendSND);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(784, 445);
		this.pnContent.TabIndex = 10;
		this.tlpProgress.Dock = System.Windows.Forms.DockStyle.Top;
		this.tlpProgress.Location = new System.Drawing.Point(0, 0);
		this.tlpProgress.Name = "tlpProgress";
		this.tlpProgress.Size = new System.Drawing.Size(784, 20);
		this.tlpProgress.TabIndex = 14;
		this.tlpProgress.Visible = false;
		this.lsvTabDocNFePendSND.AllowColumnReorder = true;
		this.lsvTabDocNFePendSND.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lsvTabDocNFePendSND.CheckBoxes = true;
		this.lsvTabDocNFePendSND.Columns.AddRange(new System.Windows.Forms.ColumnHeader[18]
		{
			this.clInHasXml, this.clInTipo, this.clInTag, this.clInNum, this.clInSerie, this.clInDtAut, this.clInValor, this.clInManifest, this.clInCancel, this.clInCNPJ,
			this.clInNome, this.clInReason, this.clInChave, this.clInFilial, this.clInCFOP, this.clInNCM, this.clInError, this.clDocNote
		});
		this.lsvTabDocNFePendSND.ContextMenuStrip = this.contextMenuDocs;
		this.lsvTabDocNFePendSND.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lsvTabDocNFePendSND.FullRowSelect = true;
		this.lsvTabDocNFePendSND.HideSelection = false;
		this.lsvTabDocNFePendSND.Location = new System.Drawing.Point(0, 0);
		this.lsvTabDocNFePendSND.Name = "lsvTabDocNFePendSND";
		this.lsvTabDocNFePendSND.ShowItemToolTips = true;
		this.lsvTabDocNFePendSND.Size = new System.Drawing.Size(784, 116);
		this.lsvTabDocNFePendSND.SmallImageList = this.ImageListDocs;
		this.lsvTabDocNFePendSND.TabIndex = 8;
		this.lsvTabDocNFePendSND.UseCompatibleStateImageBehavior = false;
		this.lsvTabDocNFePendSND.View = System.Windows.Forms.View.Details;
		this.lsvTabDocNFePendSND.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lstView_ColumnClick);
		this.lsvTabDocNFePendSND.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(lstView_ColumnReordered);
		this.lsvTabDocNFePendSND.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lstView_ColumnWidthChanged);
		this.lsvTabDocNFePendSND.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lstView_ItemChecked);
		this.lsvTabDocNFePendSND.DoubleClick += new System.EventHandler(lstView_DoubleClick);
		this.clInHasXml.Text = "";
		this.clInHasXml.Width = 34;
		this.clInTipo.Text = "Tipo";
		this.clInTipo.Width = 43;
		this.clInTag.Text = "Etiq";
		this.clInTag.Width = 20;
		this.clInNum.Text = "Num";
		this.clInNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clInNum.Width = 79;
		this.clInSerie.Text = "Serie";
		this.clInSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clInSerie.Width = 42;
		this.clInDtAut.Text = "Data";
		this.clInDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clInDtAut.Width = 80;
		this.clInValor.Text = "Valor";
		this.clInValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.clInValor.Width = 80;
		this.clInManifest.Text = "Manifestação";
		this.clInCancel.Text = "Canc";
		this.clInCancel.Width = 35;
		this.clInCNPJ.Text = "CNPJ/CPF";
		this.clInCNPJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clInCNPJ.Width = 130;
		this.clInNome.Text = "Nome";
		this.clInNome.Width = 230;
		this.clInReason.Text = "Resultado da análise";
		this.clInReason.Width = 200;
		this.clInChave.Text = "Chave";
		this.clInFilial.Text = "Filial";
		this.clInCFOP.Text = "CFOP";
		this.clInCFOP.Width = 90;
		this.clInNCM.Text = "NCM";
		this.clInNCM.Width = 90;
		this.clInError.Text = "Validação";
		this.clDocNote.Text = "Comentários";
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
		this.ImageListDocs.Images.SetKeyName(16, "image_select_all.jpg");
		this.pnMarket.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnMarket.BackColor = System.Drawing.Color.WhiteSmoke;
		this.pnMarket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnMarket.Controls.Add(this.lknClose);
		this.pnMarket.Controls.Add(this.pnMarketContent);
		this.pnMarket.Controls.Add(this.btClose);
		this.pnMarket.Location = new System.Drawing.Point(0, 117);
		this.pnMarket.Name = "pnMarket";
		this.pnMarket.Size = new System.Drawing.Size(784, 328);
		this.pnMarket.TabIndex = 21;
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
		this.lknAction.Text = "NFes pendente de entrega";
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
		this.lbText01.Text = "Lista as NFe emitidas pela sua empresa com o frete de entrega sob a sua responsabilidade (CIF) e que ainda não tiveram o devido CTe emitido.";
		this.lbTitle01.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lbTitle01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitle01.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.lbTitle01.Location = new System.Drawing.Point(3, 3);
		this.lbTitle01.Name = "lbTitle01";
		this.lbTitle01.Size = new System.Drawing.Size(560, 34);
		this.lbTitle01.TabIndex = 187;
		this.lbTitle01.Text = "NFes pendente de entrega é ativado nos \r\nplanos Avançado e Enterprise.";
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(784, 445);
		base.Controls.Add(this.pnMarket);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabDocNFePendSND";
		base.Load += new System.EventHandler(frmTabDocNFePendSND_Load);
		this.pnContent.ResumeLayout(false);
		this.pnMarket.ResumeLayout(false);
		this.pnMarket.PerformLayout();
		this.pnMarketContent.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picWarning).EndInit();
		base.ResumeLayout(false);
	}
}
