using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using audit.fiscal.io;
using data.fiscal.io;
using Monitor.CustomControls;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsDataFilter : clsAbsDataFilter
{
	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatService = new clsFeatureService();

	private clsDataDocFiscal _clsDataDocFiscal = new clsDataDocFiscal();

	private clsDataAuthAccess _clsDataAuthAccess = new clsDataAuthAccess();

	private clsDataTag _clsDataTag = new clsDataTag();

	public string DataNodeFocus { get; set; }

	public async Task<clsReturn> funcResetFilterAsync(CustomTreeview pTreeView)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			foreach (TreeNode varNodeHeader in pTreeView.Nodes)
			{
				foreach (TreeNode varNodeItem in varNodeHeader.Nodes)
				{
					if (clsFunction.Contains(clsFunction.funcGetValue(varNodeItem.Tag), "CKB"))
					{
						await _clsDataParam.funcSetAsync(varNodeItem.Name, string.Empty);
					}
				}
			}
			await _clsDataParam.funcClearFiltersAsync();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	public async Task<clsReturn> funcLoadDataAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			base.SearchTerm = await _clsDataParam.funcGetAsync("SearchTerm");
			base.DateType = await _clsDataParam.funcGetAsync("DateType");
			if (clsFunction.IsEmpty(base.DateType))
			{
				base.DateType = "DtAut";
			}
			base.BeginDate = await _clsDataParam.funcGetAsync("BeginDate");
			base.EndDate = await _clsDataParam.funcGetAsync("EndDate");
			base.PageSize = await _clsDataParam.funcGetAsync("PageSize");
			if (clsFunction.funcConvStrToInt(base.PageSize) <= 0)
			{
				base.PageSize = "1000";
			}
			if (!clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("DbaQueryLimit", pBuffer: true, pGlobal: true)))
			{
				base.PageSize = "0";
			}
			base.FilterByTomaOwner = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByTomaOwner"));
			base.FilterByTercOwner = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByTercOwner"));
			base.FilterByTomaEmitter = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByTomaEmitter"));
			base.FilterByTercEmitter = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByTercEmitter"));
			base.FilterByNFe = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFe"));
			base.FilterByCTe = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByCTe"));
			base.FilterByCTeOs = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByCTeOs"));
			base.FilterByMDFe = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByMDFe"));
			base.FilterByNFCe = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFCe"));
			base.FilterByCFeSat = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByCFeSat"));
			base.FilterByNFSe = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFSe"));
			base.FilterByDocApr = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByDocApr"));
			base.FilterByComXml = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComXml"));
			base.FilterBySemXml = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemXml"));
			base.FilterByXmlVld = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByXmlVld"));
			base.FilterByXmlErr = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByXmlErr"));
			base.FilterByDocCan = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(clsFeatureService.consFilterDocCanc));
			base.FilterByDocInu = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByDocInu"));
			base.FilterByDocDen = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByDocDen"));
			base.FilterByDocRej = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByDocRej"));
			base.FilterByComLancFisc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComLancFisc"));
			base.FilterBySemLancFisc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemLancFisc"));
			base.FilterBySemSpedIcmsIpi = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemSpedIcmsIpi"));
			base.FilterByWhtManifest = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByWhtManifest"));
			base.FilterByNFeConfirm = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFeConfirm"));
			base.FilterByNFeUnknow = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFeUnknow"));
			base.FilterByNFeDisagree = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFeDisagree"));
			base.FilterByNFeAcknow = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFeAcknow"));
			base.FilterByCTeDisagree = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByCTeDisagree"));
			base.FilterByNFSeConfirm = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFSeConfirm"));
			base.FilterByNFSeDisagree = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByNFSeDisagree"));
			base.FilterByOperIntMun = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByOperIntMun"));
			base.FilterByOperIntEst = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByOperIntEst"));
			base.FilterByOperExpDir = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByOperExpDir"));
			base.FilterByOperExpInd = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByOperExpInd"));
			base.FilterByOperImport = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByOperImport"));
			base.FilterByOperSuframa = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByOperSuframa"));
			base.FilterByComCTeDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComCTeDoc"));
			base.FilterBySemCTeDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemCTeDoc"));
			base.FilterByComMDFeDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComMDFeDoc"));
			base.FilterBySemMDFeDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemMDFeDoc"));
			base.FilterByComSufVistDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComSufVistDoc"));
			base.FilterBySemSufVistDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemSufVistDoc"));
			base.FilterByComSufInteDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComSufInteDoc"));
			base.FilterBySemSufInteDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterBySemSufInteDoc"));
			base.FilterByComexAverbSemR = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComexAverbSemR"));
			base.FilterByComexAverbParc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComexAverbParc"));
			base.FilterByComexAverbDone = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComexAverbDone"));
			base.FilterByComexAverbExcs = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("FilterByComexAverbExcs"));
			base.GroupByDisable = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("GroupByDisable"));
			base.GroupByDate = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("GroupByDate"));
			base.GroupByYearMonth = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("GroupByYearMonth"));
			base.GroupByState = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("GroupByState"));
			base.GroupByPartner = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("GroupByPartner"));
			base.GroupByFilial = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(clsFeatureService.consGroupByFilialKey));
			base.GroupByTomaIE = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync("GroupByTomaIE"));
			base.GroupByModel = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(clsFeatureService.consGroupByModelKey));
			base.GroupByNatOper = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(clsFeatureService.consGroupByNatOperKey));
			base.GroupByCFOP = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(clsFeatureService.consGroupByCFOP));
			base.GroupByTpDoc = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(clsFeatureService.consGroupByTpDocKey));
			_ = string.Empty;
			string varParametCode;
			foreach (Tag varTagItem in await _clsDataTag.funcGetListAsync())
			{
				varParametCode = "FilterBy" + varTagItem.Code;
				if (clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(varParametCode)))
				{
					TaggerList.Add(varTagItem);
				}
			}
			varParametCode = "FilterByWithoutTag";
			if (clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(varParametCode)))
			{
				TaggerList.Add(new Tag
				{
					Code = "",
					Nome = "FilterByWithoutTag"
				});
				TaggerList.Add(new Tag
				{
					Code = "NULL",
					Nome = "FilterByWithoutTag"
				});
			}
			string varSqlQuery = clsSqlFilter.funcGetDocFiscalItemSqlStr(this);
			List<DocFiscal> varDbaDocList = await _clsDataDocFiscal.funcGetListByFullSqlAsync(varSqlQuery, 0L);
			string varLastDocType = string.Empty;
			bool varFilterByDocRoot = false;
			_ = string.Empty;
			string varLoopLastKey = string.Empty;
			foreach (DocFiscal varDocFiscal in varDbaDocList)
			{
				string varLoopItemKey = varDocFiscal.DocType + "|" + varDocFiscal.DocDate + "|" + varDocFiscal.Target;
				if (varLoopItemKey.Equals(varLoopLastKey))
				{
					continue;
				}
				if (!varLastDocType.Equals(varDocFiscal.DocType))
				{
					string varNodeRootKey = "FilterByDocFisc" + varDocFiscal.DocType;
					varFilterByDocRoot = clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(varNodeRootKey));
					if (varFilterByDocRoot)
					{
						AuditSumList.Add(varNodeRootKey);
					}
				}
				string varNodeRootItem = "FilterByDocFisc" + varDocFiscal.DocType + "|" + varDocFiscal.DocDate + "|" + varDocFiscal.Target;
				if (clsFunction.funcConvStrToBool(await _clsDataParam.funcGetAsync(varNodeRootItem)) || varFilterByDocRoot)
				{
					AuditSumList.Add(varNodeRootItem);
				}
				varLastDocType = varDocFiscal.DocType;
				varLoopLastKey = varLoopItemKey;
			}
			foreach (DocFiscal varDocFiscal2 in varDbaDocList)
			{
				string varNodeRootItem2 = "FilterByDocFisc" + varDocFiscal2.DocType + "|" + varDocFiscal2.DocDate + "|" + varDocFiscal2.Target;
				if (AuditSumList.Contains(varNodeRootItem2))
				{
					AuditDocList.Add(varDocFiscal2);
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	public async Task<CustomTreeview> funcCreateAsync(CustomTreeview pTreeView)
	{
		pTreeView.BeginUpdate();
		pTreeView.SuspendLayout();
		pTreeView.Nodes.Clear();
		pTreeView.Tag = "NEWONE";
		pTreeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
		pTreeView.DrawNode += Treeview_DrawNode;
		pTreeView.Nodes.Add(funcAddFilterByOwner());
		pTreeView.Nodes.Add(funcAddFilterByEmitter());
		pTreeView.Nodes.Add(funcAddFilterByDoc());
		TreeNodeCollection nodes = pTreeView.Nodes;
		nodes.Add(await funcAddFilterByTagsAsync());
		nodes = pTreeView.Nodes;
		nodes.Add(await funcAddFilterByStatusAsync());
		TreeNode varAuditNode = await funcAddFilterByAuditAsync();
		if (varAuditNode != null)
		{
			pTreeView.Nodes.Add(varAuditNode);
		}
		TreeNode varFilterByManifest = await funcAddFilterByManifestAsync();
		if (varFilterByManifest != null)
		{
			pTreeView.Nodes.Add(varFilterByManifest);
		}
		TreeNode varFilterByLogistic = await funcAddFilterByLogisticAsync();
		if (varFilterByLogistic != null)
		{
			pTreeView.Nodes.Add(varFilterByLogistic);
		}
		nodes = pTreeView.Nodes;
		nodes.Add(await funcAddGroupByObjectAsync());
		TreeNode varReportNodes = await funcAddReportsAsync();
		pTreeView.Nodes.Add(varReportNodes);
		pTreeView.EndUpdate();
		pTreeView.ResumeLayout();
		return pTreeView;
	}

	private TreeNode funcAddFilterByOwner()
	{
		bool varMustExpand = false;
		string varTagException = "#NOT-TABDOCOUT#NOT-TABPARTNER#NOT-TABDOCJUMP#NOT-TABEXPINDREM#NOT-TABEXPDIRREM#NOT-TABEXPBALANCE#NOT-TABEXPDUECONTROL";
		string varNodeTag = "ndFilterByOwner" + varTagException;
		TreeNode varFilterByOwner = clsFunction.funcGetDefaultNode("RootFilterByOwner", "Filtrar por Tomador", varNodeTag, clsFunction.icon_filter);
		TreeNode varFilterByTomaOwner = clsFunction.funcGetDefaultNode("FilterByTomaOwner", "Empresa", "ndFilterByTomaOwner-CKB");
		if (base.FilterByTomaOwner)
		{
			varFilterByTomaOwner.Checked = base.FilterByTomaOwner;
			varMustExpand = true;
		}
		varFilterByOwner.Nodes.Add(varFilterByTomaOwner);
		TreeNode varFilterByTercOwner = clsFunction.funcGetDefaultNode("FilterByTercOwner", "Terceiros", "ndFilterByTercOwner-CKB");
		if (base.FilterByTercOwner)
		{
			varFilterByTercOwner.Checked = base.FilterByTercOwner;
			varMustExpand = true;
		}
		varFilterByOwner.Nodes.Add(varFilterByTercOwner);
		if (varMustExpand)
		{
			varFilterByOwner.Expand();
		}
		return varFilterByOwner;
	}

	private TreeNode funcAddFilterByEmitter()
	{
		bool varMustExpand = false;
		string varTagException = "#NOT-TABPARTNER#NOT-TABDOCINB#NOT-TABDOCOUT#NOT-TABDOCJUMP#NOT-TABEXPINDREM#NOT-TABEXPDIRREM#NOT-TABEXPBALANCE#NOT-TABEXPDUECONTROL";
		string varNodeTag = "ndFilterByEmitter" + varTagException;
		TreeNode varFilterByEmitter = clsFunction.funcGetDefaultNode("RootFilterByEmitter", "Filtrar por Emissor", varNodeTag, clsFunction.icon_filter);
		TreeNode varFilterByTomaEmitter = clsFunction.funcGetDefaultNode("FilterByTomaEmitter", "Empresa", "ndFilterByTomaEmitter-CKB");
		if (base.FilterByTomaEmitter)
		{
			varFilterByTomaEmitter.Checked = base.FilterByTomaEmitter;
			varMustExpand = true;
		}
		varFilterByEmitter.Nodes.Add(varFilterByTomaEmitter);
		TreeNode varFilterByTercEmitter = clsFunction.funcGetDefaultNode("FilterByTercEmitter", "Terceiros", "ndFilterByTercEmitter-CKB");
		if (base.FilterByTercEmitter)
		{
			varFilterByTercEmitter.Checked = base.FilterByTercEmitter;
			varMustExpand = true;
		}
		varFilterByEmitter.Nodes.Add(varFilterByTercEmitter);
		if (varMustExpand)
		{
			varFilterByEmitter.Expand();
		}
		return varFilterByEmitter;
	}

	private TreeNode funcAddFilterByDoc()
	{
		bool varMustExpand = false;
		string varTagException = "#NOT-TABPARTNER#NOT-TABDOCCTE#NOT-TABEXPBALANCE#NOT-TABEXPINDREM#NOT-TABEXPDIRREM#NOT-TABDOCNFEPENDCLC#NOT-TABDOCNFEPENDCTE#NOT-TABDOCRETTERCR#NOT-TABDOCRETPROPR#NOT-TABDOCREJECT#NOT-TABDOCCONF#NOT-TABDOCNFEPENDCLC#NOT-TABDOCNFEPENDSND#NOT-TABDOCNFEPENDSND#NOT-TABDOCNFERPTINCTE#NOT-TABEXPDUECONTROL";
		TreeNode varFilterByDoc = clsFunction.funcGetDefaultNode("RootFilterByDocType", "Filtrar por Tipo Doc", "ndFilterByDoc" + varTagException, clsFunction.icon_filter);
		TreeNode varFilterByNFe = clsFunction.funcGetDefaultNode("FilterByNFe", "NFe", "ndFilterByNFe-CKB");
		if (base.FilterByNFe)
		{
			varFilterByNFe.Checked = base.FilterByNFe;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByNFe);
		TreeNode varFilterByCTe = clsFunction.funcGetDefaultNode("FilterByCTe", "CTe", "ndFilterByCTe-CKB#NOT-TABDOCNFE");
		if (base.FilterByCTe)
		{
			varFilterByCTe.Checked = base.FilterByCTe;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByCTe);
		TreeNode varFilterByCTeOs = clsFunction.funcGetDefaultNode("FilterByCTeOs", "CTeOs", "ndFilterByCTeOs-CKB");
		if (base.FilterByCTeOs)
		{
			varFilterByCTeOs.Checked = base.FilterByCTeOs;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByCTeOs);
		TreeNode varFilterByMDFe = clsFunction.funcGetDefaultNode("FilterByMDFe", "MDFe", "ndFilterByMDFe-CKB");
		if (base.FilterByMDFe)
		{
			varFilterByMDFe.Checked = base.FilterByMDFe;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByMDFe);
		TreeNode varFilterByNFCe = clsFunction.funcGetDefaultNode("FilterByNFCe", "NFCe", "ndFilterByNFCe-CKB");
		if (base.FilterByNFCe)
		{
			varFilterByNFCe.Checked = base.FilterByNFCe;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByNFCe);
		TreeNode varFilterByCFeSat = clsFunction.funcGetDefaultNode("FilterByCFeSat", "CFe Sat", "ndFilterByCFeSat-CKB");
		if (base.FilterByCFeSat)
		{
			varFilterByCFeSat.Checked = base.FilterByCFeSat;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByCFeSat);
		TreeNode varFilterByNFSe = clsFunction.funcGetDefaultNode("FilterByNFSe", "NFSe", "ndFilterByNFSe-CKB");
		if (base.FilterByNFSe)
		{
			varFilterByNFSe.Checked = base.FilterByNFSe;
			varMustExpand = true;
		}
		varFilterByDoc.Nodes.Add(varFilterByNFSe);
		if (varMustExpand)
		{
			varFilterByDoc.Expand();
		}
		return varFilterByDoc;
	}

	private async Task<TreeNode> funcAddGroupByObjectAsync()
	{
		bool varMustExpand = false;
		string varTagException = "#NOT-TABDOCJUMP";
		TreeNode varGroupByObj = clsFunction.funcGetDefaultNode("RootGroupBy", "Agrupamento", "ndRootGroupBy" + varTagException, clsFunction.icon_groupby);
		TreeNode varGroupByDisable = clsFunction.funcGetDefaultNode("GroupByDisable", "Inativo", "ndGroupByDisable");
		varGroupByDisable.Checked = base.GroupByDisable;
		if (base.GroupByDisable)
		{
			int imageIndex = (varGroupByDisable.SelectedImageIndex = clsFunction.icon_selected);
			varGroupByDisable.ImageIndex = imageIndex;
		}
		varGroupByObj.Nodes.Add(varGroupByDisable);
		TreeNode varGroupByDate = clsFunction.funcGetDefaultNode("GroupByDate", "Data", "ndGroupByDate");
		varGroupByDate.Checked = base.GroupByDate;
		if (base.GroupByDate)
		{
			int imageIndex = (varGroupByDate.SelectedImageIndex = clsFunction.icon_selected);
			varGroupByDate.ImageIndex = imageIndex;
			varMustExpand = true;
		}
		varGroupByObj.Nodes.Add(varGroupByDate);
		TreeNode varGroupByYearMonth = clsFunction.funcGetDefaultNode("GroupByYearMonth", "Ano-Mês", "ndGroupByYearMonth");
		varGroupByYearMonth.Checked = base.GroupByYearMonth;
		if (base.GroupByYearMonth)
		{
			int imageIndex = (varGroupByYearMonth.SelectedImageIndex = clsFunction.icon_selected);
			varGroupByYearMonth.ImageIndex = imageIndex;
			varMustExpand = true;
		}
		varGroupByObj.Nodes.Add(varGroupByYearMonth);
		TreeNode varGroupByState = clsFunction.funcGetDefaultNode("GroupByState", "Estado", "ndGroupByState");
		varGroupByState.Checked = base.GroupByState;
		if (base.GroupByState)
		{
			int imageIndex = (varGroupByState.SelectedImageIndex = clsFunction.icon_selected);
			varGroupByState.ImageIndex = imageIndex;
			varMustExpand = true;
		}
		varGroupByObj.Nodes.Add(varGroupByState);
		TreeNode varGroupByPartner = clsFunction.funcGetDefaultNode("GroupByPartner", "Parceiro", "ndGroupByPartner#NOT-TABPARTNER");
		varGroupByPartner.Checked = base.GroupByPartner;
		if (base.GroupByPartner)
		{
			int imageIndex = (varGroupByPartner.SelectedImageIndex = clsFunction.icon_selected);
			varGroupByPartner.ImageIndex = imageIndex;
			varMustExpand = true;
		}
		varGroupByObj.Nodes.Add(varGroupByPartner);
		TreeNode varGroupByFilial = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consGroupByFilialKey);
		if (varGroupByFilial != null)
		{
			varGroupByFilial.Tag = varGroupByFilial.Tag?.ToString() + "#NOT-TABPARTNER";
			varGroupByFilial.Checked = base.GroupByFilial;
			if (base.GroupByFilial && !clsFunction.Contains((string)varGroupByFilial.Tag, "LOCK"))
			{
				int imageIndex = (varGroupByFilial.SelectedImageIndex = clsFunction.icon_selected);
				varGroupByFilial.ImageIndex = imageIndex;
				varMustExpand = true;
			}
			varGroupByObj.Nodes.Add(varGroupByFilial);
		}
		TreeNode varGroupByTomaIE = clsFunction.funcGetDefaultNode("GroupByTomaIE", "IE do Tomador", "ndGroupByTomaIE#NOT-TABPARTNER");
		varGroupByTomaIE.Checked = base.GroupByTomaIE;
		if (base.GroupByTomaIE)
		{
			int imageIndex = (varGroupByTomaIE.SelectedImageIndex = clsFunction.icon_selected);
			varGroupByTomaIE.ImageIndex = imageIndex;
			varMustExpand = true;
		}
		varGroupByObj.Nodes.Add(varGroupByTomaIE);
		TreeNode varGroupByModel = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consGroupByModelKey);
		if (varGroupByModel != null)
		{
			varGroupByModel.Tag = varGroupByModel.Tag?.ToString() + "#NOT-TABPARTNER";
			varGroupByModel.Checked = base.GroupByModel;
			if (base.GroupByModel && !clsFunction.Contains((string)varGroupByModel.Tag, "LOCK"))
			{
				int imageIndex = (varGroupByModel.SelectedImageIndex = clsFunction.icon_selected);
				varGroupByModel.ImageIndex = imageIndex;
				varMustExpand = true;
			}
			varGroupByObj.Nodes.Add(varGroupByModel);
		}
		TreeNode varGroupByNatOper = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consGroupByNatOperKey);
		if (varGroupByNatOper != null)
		{
			varGroupByNatOper.Tag = varGroupByNatOper.Tag?.ToString() + "#NOT-TABPARTNER";
			varGroupByNatOper.Checked = base.GroupByNatOper;
			if (base.GroupByNatOper && !clsFunction.Contains((string)varGroupByNatOper.Tag, "LOCK"))
			{
				int imageIndex = (varGroupByNatOper.SelectedImageIndex = clsFunction.icon_selected);
				varGroupByNatOper.ImageIndex = imageIndex;
				varMustExpand = true;
			}
			varGroupByObj.Nodes.Add(varGroupByNatOper);
		}
		TreeNode varGroupByCFOP = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consGroupByCFOP);
		if (varGroupByCFOP != null)
		{
			varGroupByCFOP.Tag = varGroupByCFOP.Tag?.ToString() + "#NOT-TABPARTNER";
			varGroupByCFOP.Checked = base.GroupByCFOP;
			if (base.GroupByCFOP && !clsFunction.Contains((string)varGroupByCFOP.Tag, "LOCK"))
			{
				int imageIndex = (varGroupByCFOP.SelectedImageIndex = clsFunction.icon_selected);
				varGroupByCFOP.ImageIndex = imageIndex;
				varMustExpand = true;
			}
			varGroupByObj.Nodes.Add(varGroupByCFOP);
		}
		TreeNode varGroupByTpDoc = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consGroupByTpDocKey);
		if (varGroupByTpDoc != null)
		{
			varGroupByTpDoc.Tag = varGroupByTpDoc.Tag?.ToString() + "#NOT-TABPARTNER";
			varGroupByTpDoc.Checked = base.GroupByTpDoc;
			if (base.GroupByTpDoc && !clsFunction.Contains((string)varGroupByTpDoc.Tag, "LOCK"))
			{
				int imageIndex = (varGroupByTpDoc.SelectedImageIndex = clsFunction.icon_selected);
				varGroupByTpDoc.ImageIndex = imageIndex;
				varMustExpand = true;
			}
			varGroupByObj.Nodes.Add(varGroupByTpDoc);
		}
		if (varMustExpand)
		{
			varGroupByObj.Expand();
		}
		return varGroupByObj;
	}

	private async Task<TreeNode> funcAddFilterByTagsAsync()
	{
		bool varMustExpand = false;
		new Tag();
		string varTagException = "#NOT-TABPARTNER#NOT-TABDOCJUMP#NOT-TABEXPDUECONTROL";
		TreeNode varFilterByTag = clsFunction.funcGetDefaultNode("RootFilterByTag", "Filtrar por Etiquetas", "ndFilterByTag" + varTagException, clsFunction.icon_filter);
		foreach (Tag varTag in await new clsDataTag().funcGetListAsync())
		{
			TreeNode varTagNode = clsFunction.funcGetDefaultNode("FilterBy" + varTag.Code, varTag.Nome, varTag.Code + " - CKB");
			if (TaggerList.FirstOrDefault((Tag r) => r.Code.Equals(varTag.Code)) != null)
			{
				varTagNode.Checked = true;
				varMustExpand = true;
			}
			varFilterByTag.Nodes.Add(varTagNode);
		}
		TreeNode varNodeWithoutTag = clsFunction.funcGetDefaultNode("FilterByWithoutTag", "Sem Etiquetas", "WithoutTag-CKB");
		if (TaggerList.FirstOrDefault((Tag r) => r.Nome.Equals(varNodeWithoutTag.Name)) != null)
		{
			varNodeWithoutTag.Checked = true;
			varMustExpand = true;
		}
		varFilterByTag.Nodes.Add(varNodeWithoutTag);
		if (varMustExpand)
		{
			varFilterByTag.Expand();
		}
		return varFilterByTag;
	}

	private async Task<TreeNode> funcAddFilterByStatusAsync()
	{
		bool varMustExpand = false;
		string varTagException = "#NOT-TABPARTNER#NOT-TABDOCJUMP#NOT-TABEXPDUECONTROL";
		TreeNode varFilterByStat = clsFunction.funcGetDefaultNode("RootFilterByStatus", "Filtrar por Status", "ndFilterByStatus" + varTagException, clsFunction.icon_filter);
		TreeNode varRepDocComXml = clsFunction.funcGetDefaultNode("FilterByComXml", "Docs com XML", "ndFilterByComXml-CKB");
		if (base.FilterByComXml)
		{
			varRepDocComXml.Checked = base.FilterByComXml;
			varMustExpand = true;
		}
		varFilterByStat.Nodes.Add(varRepDocComXml);
		TreeNode varRepDocSemXml = clsFunction.funcGetDefaultNode("FilterBySemXml", "Docs sem XML", "ndFilterBySemXml-CKB");
		if (base.FilterBySemXml)
		{
			varRepDocSemXml.Checked = base.FilterBySemXml;
			varMustExpand = true;
		}
		varFilterByStat.Nodes.Add(varRepDocSemXml);
		TreeNode varRepDocApprov = clsFunction.funcGetDefaultNode("FilterByDocApr", "Docs aprovados", "ndFilterByDocApr-CKB");
		if (base.FilterByDocApr)
		{
			varRepDocApprov.Checked = base.FilterByDocApr;
			varMustExpand = true;
		}
		varFilterByStat.Nodes.Add(varRepDocApprov);
		TreeNode varFilterDocCancel = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consFilterDocCanc);
		if (varFilterDocCancel != null)
		{
			varFilterDocCancel.Text = "Docs cancelados";
			varFilterDocCancel.Tag = $"{clsFeatureService.consFilterDocCanc}-{varFilterDocCancel.Tag}-CKB";
			if (base.FilterByDocCan && !clsFunction.Contains((string)varFilterDocCancel.Tag, "LOCK"))
			{
				varFilterDocCancel.Checked = base.FilterByDocCan;
				varMustExpand = true;
			}
			varFilterByStat.Nodes.Add(varFilterDocCancel);
			TreeNode varRepDocInu = (TreeNode)varFilterDocCancel.Clone();
			varRepDocInu.Text = "Docs inutilizados";
			varRepDocInu.Name = "FilterByDocInu";
			varRepDocInu.Checked = false;
			if (base.FilterByDocInu && !clsFunction.Contains((string)varRepDocInu.Tag, "LOCK"))
			{
				varRepDocInu.Checked = base.FilterByDocInu;
				varMustExpand = true;
			}
			varFilterByStat.Nodes.Add(varRepDocInu);
			TreeNode varRepDocDen = (TreeNode)varFilterDocCancel.Clone();
			varRepDocDen.Text = "Docs denegados";
			varRepDocDen.Name = "FilterByDocDen";
			varRepDocDen.Checked = false;
			if (base.FilterByDocDen && !clsFunction.Contains((string)varRepDocDen.Tag, "LOCK"))
			{
				varRepDocDen.Checked = base.FilterByDocDen;
				varMustExpand = true;
			}
			varFilterByStat.Nodes.Add(varRepDocDen);
			TreeNode varRepDocRej = (TreeNode)varFilterDocCancel.Clone();
			varRepDocRej.Text = "Docs rejeitados";
			varRepDocRej.Name = "FilterByDocRej";
			varRepDocRej.Checked = false;
			if (base.FilterByDocRej && !clsFunction.Contains((string)varRepDocRej.Tag, "LOCK"))
			{
				varRepDocRej.Checked = base.FilterByDocRej;
				varMustExpand = true;
			}
			varFilterByStat.Nodes.Add(varRepDocRej);
		}
		TreeNode varNodeFilterByXmlJurVal = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consFeatXmlJuridicValidation);
		if (varNodeFilterByXmlJurVal != null)
		{
			varNodeFilterByXmlJurVal.Tag = $"{clsFeatureService.consFeatXmlJuridicValidation}-{varNodeFilterByXmlJurVal.Tag}-CKB";
			TreeNode varRepDocXmlVld = (TreeNode)varNodeFilterByXmlJurVal.Clone();
			varRepDocXmlVld.Text = "XMLs válidos";
			varRepDocXmlVld.Name = "FilterByXmlVld";
			varRepDocXmlVld.Checked = false;
			if (base.FilterByXmlVld && !clsFunction.Contains((string)varRepDocXmlVld.Tag, "LOCK"))
			{
				varRepDocXmlVld.Checked = base.FilterByXmlVld;
				varMustExpand = true;
			}
			varFilterByStat.Nodes.Add(varRepDocXmlVld);
			TreeNode varRepDocXmlErr = (TreeNode)varNodeFilterByXmlJurVal.Clone();
			varRepDocXmlErr.Text = "XMLs inválidos";
			varRepDocXmlErr.Name = "FilterByXmlErr";
			varRepDocXmlErr.Checked = false;
			if (base.FilterByXmlErr && !clsFunction.Contains((string)varRepDocXmlErr.Tag, "LOCK"))
			{
				varRepDocXmlErr.Checked = base.FilterByXmlErr;
				varMustExpand = true;
			}
			varFilterByStat.Nodes.Add(varRepDocXmlErr);
		}
		TreeNode varNodeConnect = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consFeatFiscalConnect);
		if (varNodeConnect != null)
		{
			bool num = await clsScreenGeral.funcMustShowRegFieldsAsync();
			varNodeConnect.Tag = $"{clsFeatureService.consFeatFiscalConnect}-{varNodeConnect.Tag}-CKB";
			if (num || clsFunction.Contains((string)varNodeConnect.Tag, "LOCK"))
			{
				TreeNode varRepComLancFisc = (TreeNode)varNodeConnect.Clone();
				varRepComLancFisc.Text = "Fiscal : Lançados";
				varRepComLancFisc.Name = "FilterByComLancFisc";
				varRepComLancFisc.Checked = false;
				if (base.FilterByComLancFisc && !clsFunction.Contains((string)varRepComLancFisc.Tag, "LOCK"))
				{
					varRepComLancFisc.Checked = base.FilterByComLancFisc;
					varMustExpand = true;
				}
				varFilterByStat.Nodes.Add(varRepComLancFisc);
				TreeNode varRepSemLancFisc = (TreeNode)varNodeConnect.Clone();
				varRepSemLancFisc.Text = "Fiscal : Pendentes";
				varRepSemLancFisc.Name = "FilterBySemLancFisc";
				varRepSemLancFisc.Checked = false;
				if (base.FilterBySemLancFisc && !clsFunction.Contains((string)varRepSemLancFisc.Tag, "LOCK"))
				{
					varRepSemLancFisc.Checked = base.FilterBySemLancFisc;
					varMustExpand = true;
				}
				varFilterByStat.Nodes.Add(varRepSemLancFisc);
			}
		}
		if (varMustExpand)
		{
			varFilterByStat.Expand();
		}
		return varFilterByStat;
	}

	public async Task<TreeNode> funcAddFilterByAuditAsync(TreeNode pTreeNode = null)
	{
		bool varMustExpand = false;
		bool varNodeSelect = false;
		clsDocFileFactory varclsFactory = new clsDocFileFactory();
		intDocFile varHandler = null;
		if (clsFunction.IsEqual((await new clsDbaFactory().funcGetClassAsync()).funcGetDbaType(), "SQLLITE"))
		{
			return null;
		}
		string varTagException = "#NOT-TABPARTNER-#NOT-TABDOCJUMP#NOT-TABEXPDUECONTROL#NOT-" + clsFeatureService.consFeatFiscalAudit;
		TreeNode varNodeTemplate = pTreeNode;
		if (varNodeTemplate == null)
		{
			varNodeTemplate = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consFeatFiscalAudit);
		}
		if (varNodeTemplate == null)
		{
			return null;
		}
		TreeNode varFilterByAudit = new TreeNode();
		varFilterByAudit.Name = "RootFilterByAudit";
		varFilterByAudit.Text = "Filtrar por Auditoria";
		varFilterByAudit.Tag = $"{clsFeatureService.consFeatFiscalAudit}-{varFilterByAudit.Tag}{varTagException}";
		int imageIndex = (varFilterByAudit.SelectedImageIndex = clsFunction.icon_audit);
		varFilterByAudit.ImageIndex = imageIndex;
		bool varIsExpanded = varFilterByAudit.IsExpanded;
		varFilterByAudit.Nodes.Clear();
		TreeNode varNodeSemSpedIcmsIpi = new TreeNode();
		varNodeSemSpedIcmsIpi.Name = "FilterBySemSpedIcmsIpi";
		varNodeSemSpedIcmsIpi.Text = "Sem SPED-ICMS-IPI";
		varNodeSemSpedIcmsIpi.Tag = $"{clsFeatureService.consFeatFiscalAudit}-{varNodeSemSpedIcmsIpi.Tag}-CKB";
		if (base.FilterBySemSpedIcmsIpi)
		{
			varMustExpand = true;
		}
		varNodeSemSpedIcmsIpi.Checked = base.FilterBySemSpedIcmsIpi;
		varFilterByAudit.Nodes.Add(varNodeSemSpedIcmsIpi);
		string varSqlQuery = clsSqlFilter.funcGetDocFiscalItemSqlStr(this);
		List<DocFiscal> obj = await new clsDataDocFiscal().funcGetListByFullSqlAsync(varSqlQuery, 0L);
		string varLastDocType = string.Empty;
		TreeNode varDocNodeRoot = new TreeNode();
		string varLoopLastKey = string.Empty;
		foreach (DocFiscal varDocFiscal in obj)
		{
			string varLoopItemKey = varDocFiscal.DocType + "|" + varDocFiscal.DocDate + "|" + varDocFiscal.Target;
			if (clsFunction.IsEqual(varLoopItemKey, varLoopLastKey))
			{
				continue;
			}
			if (!clsFunction.IsEqual(varLastDocType, varDocFiscal.DocType))
			{
				string varNodeKeyRoot = (varDocNodeRoot.Name = "FilterByDocFisc" + varDocFiscal.DocType);
				varDocNodeRoot.Text = "Com " + varDocFiscal.DocType;
				varDocNodeRoot.Tag = clsFeatureService.consFeatFiscalAudit + "-nd" + varDocFiscal.DocType + "-CKB";
				varHandler = varclsFactory.funcGetClass(varDocFiscal.DocType);
				if (AuditSumList.Contains(varNodeKeyRoot) && !clsFunction.Contains((string)varDocNodeRoot.Tag, "LOCK"))
				{
					varDocNodeRoot.Checked = true;
				}
				if (varDocNodeRoot.Checked)
				{
					varMustExpand = true;
				}
				varFilterByAudit.Nodes.Add(varDocNodeRoot);
			}
			string varNodeKey = "FilterByDocFisc" + varDocFiscal.DocType + "|" + varDocFiscal.DocDate + "|" + varDocFiscal.Target;
			string varDocFiscalIdnt = varDocFiscal.DocDate + "-" + varHandler.funcGetTarget(varDocFiscal.Target);
			TreeNode varDocNodeItem = new TreeNode();
			varDocNodeItem.Name = varNodeKey;
			varDocNodeItem.Text = varDocFiscalIdnt;
			varDocNodeItem.Tag = clsFeatureService.consFeatFiscalAudit + "-nd" + varNodeKey + "-CKB";
			if (AuditSumList.Contains(varNodeKey) && !clsFunction.Contains((string)varDocNodeItem.Tag, "LOCK"))
			{
				varNodeSelect = (varDocNodeItem.Checked = true);
			}
			if (varNodeSelect)
			{
				varMustExpand = true;
			}
			varDocNodeRoot.Nodes.Add(varDocNodeItem);
			varDocNodeRoot.Expand();
			varLastDocType = varDocFiscal.DocType;
			varLoopLastKey = varLoopItemKey;
		}
		if (varIsExpanded)
		{
			varMustExpand = true;
		}
		if (varMustExpand)
		{
			varFilterByAudit.Expand();
		}
		else
		{
			varFilterByAudit.Collapse();
		}
		return varFilterByAudit;
	}

	private async Task<TreeNode> funcAddFilterByManifestAsync()
	{
		bool varMustExpand = false;
		TreeNode varNodeFilterByManifest = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consFilterByManifest);
		if (varNodeFilterByManifest == null)
		{
			return null;
		}
		varNodeFilterByManifest.Tag = $"{clsFeatureService.consFilterByManifest}-{varNodeFilterByManifest.Tag}-CKB";
		string varTagException = "#NOT-TABPARTNER#NOT-TABDOCJUMP#NOT-TABDOCPRTSRV#NOT-TABDOCCONF#NOT-TABDOCREJECT#NOT-TABEXPDUECONTROL";
		TreeNode varFilterByManif = clsFunction.funcGetDefaultNode("RootFilterByManifest", "Filtrar por Manifestação", "ndFilterByManifest" + varTagException, clsFunction.icon_filter);
		TreeNode varRepDocWhtManifest = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocWhtManifest.Text = "Sem manifestação";
		varRepDocWhtManifest.Name = "FilterByWhtManifest";
		if (base.FilterByWhtManifest && !clsFunction.Contains((string)varRepDocWhtManifest.Tag, "LOCK"))
		{
			varRepDocWhtManifest.Checked = base.FilterByWhtManifest;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocWhtManifest);
		TreeNode varRepDocNFeAcknow = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocNFeAcknow.Text = "Ciência da operação";
		varRepDocNFeAcknow.Name = "FilterByNFeAcknow";
		if (base.FilterByNFeAcknow && !clsFunction.Contains((string)varRepDocNFeAcknow.Tag, "LOCK"))
		{
			varRepDocNFeAcknow.Checked = base.FilterByNFeAcknow;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocNFeAcknow);
		TreeNode varRepDocNFeConfirm = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocNFeConfirm.Text = "Operação confirmada";
		varRepDocNFeConfirm.Name = "FilterByNFeConfirm";
		if (base.FilterByNFeConfirm && !clsFunction.Contains((string)varRepDocNFeConfirm.Tag, "LOCK"))
		{
			varRepDocNFeConfirm.Checked = base.FilterByNFeConfirm;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocNFeConfirm);
		TreeNode varRepDocNFeUnknow = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocNFeUnknow.Text = "Operação desconhecida";
		varRepDocNFeUnknow.Name = "FilterByNFeUnknow";
		if (base.FilterByNFeUnknow && !clsFunction.Contains((string)varRepDocNFeUnknow.Tag, "LOCK"))
		{
			varRepDocNFeUnknow.Checked = base.FilterByNFeUnknow;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocNFeUnknow);
		TreeNode varRepDocNFeDisagree = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocNFeDisagree.Text = "Operação não realizada";
		varRepDocNFeDisagree.Name = "FilterByNFeDisagree";
		if (base.FilterByNFeDisagree && !clsFunction.Contains((string)varRepDocNFeDisagree.Tag, "LOCK"))
		{
			varRepDocNFeDisagree.Checked = base.FilterByNFeDisagree;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocNFeDisagree);
		TreeNode varRepDocCTeDisagree = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocCTeDisagree.Text = "Desacordo do serviço";
		varRepDocCTeDisagree.Name = "FilterByCTeDisagree";
		if (base.FilterByCTeDisagree && !clsFunction.Contains((string)varRepDocCTeDisagree.Tag, "LOCK"))
		{
			varRepDocCTeDisagree.Checked = base.FilterByCTeDisagree;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocCTeDisagree);
		TreeNode varRepDocNFSeConfirm = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocNFSeConfirm.Text = "Confirmação do Tomador";
		varRepDocNFSeConfirm.Name = "FilterByNFSeConfirm";
		if (base.FilterByNFSeConfirm && !clsFunction.Contains((string)varRepDocNFSeConfirm.Tag, "LOCK"))
		{
			varRepDocNFSeConfirm.Checked = base.FilterByNFSeConfirm;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocNFSeConfirm);
		TreeNode varRepDocNFSeDisagree = (TreeNode)varNodeFilterByManifest.Clone();
		varRepDocNFSeDisagree.Text = "Rejeição do Tomador";
		varRepDocNFSeDisagree.Name = "FilterByNFSeDisagree";
		if (base.FilterByNFSeDisagree && !clsFunction.Contains((string)varRepDocNFSeDisagree.Tag, "LOCK"))
		{
			varRepDocNFSeDisagree.Checked = base.FilterByNFSeDisagree;
			varMustExpand = true;
		}
		varFilterByManif.Nodes.Add(varRepDocNFSeDisagree);
		if (varMustExpand)
		{
			varFilterByManif.Expand();
		}
		return varFilterByManif;
	}

	private async Task<TreeNode> funcAddFilterByLogisticAsync()
	{
		bool varMustExpand = false;
		TreeNode varNodeFilterByLogistic = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consFilterByLogistic);
		if (varNodeFilterByLogistic == null)
		{
			return null;
		}
		varNodeFilterByLogistic.Tag = $"{clsFeatureService.consFilterByLogistic}-{varNodeFilterByLogistic.Tag}-CKB";
		string varTagException = "#NOT-TABPARTNER#NOT-TABDOCJUMP#NOT-TABDOCPRTSRV#NOT-TABDOCCONF#NOT-TABDOCREJECT#NOT-TABEXPDUECONTROL";
		TreeNode varFilterByLogist = clsFunction.funcGetDefaultNode("RootFilterByLogistic", "Filtrar por Operações", "ndFilterByLogistic" + varTagException, clsFunction.icon_filter);
		TreeNode varRepDocOperIntMun = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocOperIntMun.Text = "Internas";
		varRepDocOperIntMun.Name = "FilterByOperIntMun";
		if (base.FilterByOperIntMun)
		{
			varRepDocOperIntMun.Checked = base.FilterByOperIntMun;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocOperIntMun);
		TreeNode varRepDocOperIntEst = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocOperIntEst.Text = "Interestaduais";
		varRepDocOperIntEst.Name = "FilterByOperIntEst";
		if (base.FilterByOperIntEst)
		{
			varRepDocOperIntEst.Checked = base.FilterByOperIntEst;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocOperIntEst);
		TreeNode varRepDocOperExpDir = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocOperExpDir.Text = "Exportaçao Direta";
		varRepDocOperExpDir.Name = "FilterByOperExpDir";
		if (base.FilterByOperExpDir)
		{
			varRepDocOperExpDir.Checked = base.FilterByOperExpDir;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocOperExpDir);
		TreeNode varRepDocOperExpInd = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocOperExpInd.Text = "Exportaçao Indireta";
		varRepDocOperExpInd.Name = "FilterByOperExpInd";
		if (base.FilterByOperExpInd)
		{
			varRepDocOperExpInd.Checked = base.FilterByOperExpInd;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocOperExpInd);
		TreeNode varRepDocOperImport = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocOperImport.Text = "Importação";
		varRepDocOperImport.Name = "FilterByOperImport";
		if (base.FilterByOperImport)
		{
			varRepDocOperImport.Checked = base.FilterByOperImport;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocOperImport);
		TreeNode varRepDocOperSuframa = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocOperSuframa.Text = "Suframa";
		varRepDocOperSuframa.Name = "FilterByOperSuframa";
		if (base.FilterByOperSuframa)
		{
			varRepDocOperSuframa.Checked = base.FilterByOperSuframa;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocOperSuframa);
		TreeNode varRepDocComCTeDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocComCTeDoc.Text = "Com CTe vinculado";
		varRepDocComCTeDoc.Name = "FilterByComCTeDoc";
		if (base.FilterByComCTeDoc)
		{
			varRepDocComCTeDoc.Checked = base.FilterByComCTeDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocComCTeDoc);
		TreeNode varRepDoSemCTeDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDoSemCTeDoc.Text = "Sem CTe vinculado";
		varRepDoSemCTeDoc.Name = "FilterBySemCTeDoc";
		if (base.FilterBySemCTeDoc)
		{
			varRepDoSemCTeDoc.Checked = base.FilterBySemCTeDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDoSemCTeDoc);
		TreeNode varRepDocComMDFeDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocComMDFeDoc.Text = "Com MDFe vinculado";
		varRepDocComMDFeDoc.Name = "FilterByComMDFeDoc";
		if (base.FilterByComMDFeDoc)
		{
			varRepDocComMDFeDoc.Checked = base.FilterByComMDFeDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocComMDFeDoc);
		TreeNode varRepDoSemMDFeDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDoSemMDFeDoc.Text = "Sem MDFe vinculado";
		varRepDoSemMDFeDoc.Name = "FilterBySemMDFeDoc";
		if (base.FilterBySemMDFeDoc)
		{
			varRepDoSemMDFeDoc.Checked = base.FilterBySemMDFeDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDoSemMDFeDoc);
		TreeNode varRepDocComSufVistDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocComSufVistDoc.Text = "Com Suframa Vistoria";
		varRepDocComSufVistDoc.Name = "FilterByComSufVistDoc";
		if (base.FilterByComSufVistDoc)
		{
			varRepDocComSufVistDoc.Checked = base.FilterByComSufVistDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocComSufVistDoc);
		TreeNode varRepDoSemSufVistDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDoSemSufVistDoc.Text = "Sem Suframa Vistoria";
		varRepDoSemSufVistDoc.Name = "FilterBySemSufVistDoc";
		if (base.FilterBySemSufVistDoc)
		{
			varRepDoSemSufVistDoc.Checked = base.FilterBySemSufVistDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDoSemSufVistDoc);
		TreeNode varRepDocComSufInteDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDocComSufInteDoc.Text = "Com Suframa Internalização";
		varRepDocComSufInteDoc.Name = "FilterByComSufInteDoc";
		if (base.FilterByComSufInteDoc)
		{
			varRepDocComSufInteDoc.Checked = base.FilterByComSufInteDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDocComSufInteDoc);
		TreeNode varRepDoSemSufInteDoc = (TreeNode)varNodeFilterByLogistic.Clone();
		varRepDoSemSufInteDoc.Text = "Sem Suframa Internalização";
		varRepDoSemSufInteDoc.Name = "FilterBySemSufInteDoc";
		if (base.FilterBySemSufInteDoc)
		{
			varRepDoSemSufInteDoc.Checked = base.FilterBySemSufInteDoc;
			varMustExpand = true;
		}
		varFilterByLogist.Nodes.Add(varRepDoSemSufInteDoc);
		if (!clsFunction.Contains(await _clsFeatService.funcGetFeatTypeAsync(clsFeatureService.consReportDocNFeAverConf), "LOCK"))
		{
			TreeNode varRepComexAverbSemR = (TreeNode)varNodeFilterByLogistic.Clone();
			varRepComexAverbSemR.Text = "Averbação Sem registro";
			varRepComexAverbSemR.Name = "FilterByComexAverbSemR";
			if (base.FilterByComexAverbSemR)
			{
				varRepComexAverbSemR.Checked = base.FilterByComexAverbSemR;
				varMustExpand = true;
			}
			varFilterByLogist.Nodes.Add(varRepComexAverbSemR);
			TreeNode varRepComexAverbParc = (TreeNode)varNodeFilterByLogistic.Clone();
			varRepComexAverbParc.Text = "Averbação Parcial";
			varRepComexAverbParc.Name = "FilterByComexAverbParc";
			if (base.FilterByComexAverbParc)
			{
				varRepComexAverbParc.Checked = base.FilterByComexAverbParc;
				varMustExpand = true;
			}
			varFilterByLogist.Nodes.Add(varRepComexAverbParc);
			TreeNode varRepComexAverbDone = (TreeNode)varNodeFilterByLogistic.Clone();
			varRepComexAverbDone.Text = "Averbação Concluída";
			varRepComexAverbDone.Name = "FilterByComexAverbDone";
			if (base.FilterByComexAverbParc)
			{
				varRepComexAverbDone.Checked = base.FilterByComexAverbDone;
				varMustExpand = true;
			}
			varFilterByLogist.Nodes.Add(varRepComexAverbDone);
			TreeNode varRepComexAverbExcs = (TreeNode)varNodeFilterByLogistic.Clone();
			varRepComexAverbExcs.Text = "Averbação Excedida";
			varRepComexAverbExcs.Name = "FilterByComexAverbExcs";
			if (base.FilterByComexAverbExcs)
			{
				varRepComexAverbExcs.Checked = base.FilterByComexAverbExcs;
				varMustExpand = true;
			}
			varFilterByLogist.Nodes.Add(varRepComexAverbExcs);
		}
		if (varMustExpand)
		{
			varFilterByLogist.Expand();
		}
		return varFilterByLogist;
	}

	private async Task<TreeNode> funcAddReportsAsync()
	{
		TreeNode varReports = clsFunction.funcGetDefaultNode("RootReports", "Relatórios", "ndRootReports", clsFunction.icon_report);
		TreeNode varRepDocInb = clsFunction.funcGetDefaultNode("TabDocInb", "Emitidos por terceiros", "ndTabDocInb");
		varReports.Nodes.Add(varRepDocInb);
		TreeNode varRepDocOut = clsFunction.funcGetDefaultNode("TabDocOut", "Emitidos pela empresa", "ndTabDocOut");
		varReports.Nodes.Add(varRepDocOut);
		varReports = await funcGetReportAsync(varReports, clsFeatureService.consReportJumpOut);
		TreeNode varReportAnaliticSintetic = await _clsFeatService.funcGetNodeAsync(clsFeatureService.consReportAnaliticSintetic);
		if (varReportAnaliticSintetic != null)
		{
			varReportAnaliticSintetic.Tag = $"{clsFeatureService.consReportAnaliticSintetic}-{varReportAnaliticSintetic.Tag}";
			TreeNode varRepDocNFe = (TreeNode)varReportAnaliticSintetic.Clone();
			varRepDocNFe.Text = "NFe : Dados sintéticos";
			varRepDocNFe.Name = "TabDocNFe";
			varReports.Nodes.Add(varRepDocNFe);
			TreeNode varRepDocNFeDet = (TreeNode)varReportAnaliticSintetic.Clone();
			varRepDocNFeDet.Text = "NFe : Dados analíticos";
			varRepDocNFeDet.Name = "TabDocNFeDet";
			varReports.Nodes.Add(varRepDocNFeDet);
			TreeNode varRepDocCTe = (TreeNode)varReportAnaliticSintetic.Clone();
			varRepDocCTe.Text = "CTe : Dados sintéticos";
			varRepDocCTe.Name = "TabDocCTe";
			varReports.Nodes.Add(varRepDocCTe);
			TreeNode varRepDocCTeDet = (TreeNode)varReportAnaliticSintetic.Clone();
			varRepDocCTeDet.Text = "CTe : Dados analíticos";
			varRepDocCTeDet.Name = "TabDocCTeDet";
			varReports.Nodes.Add(varRepDocCTeDet);
			TreeNode varRepDocNFSe = (TreeNode)varReportAnaliticSintetic.Clone();
			varRepDocNFSe.Text = "NFSe : Dados sintéticos";
			varRepDocNFSe.Name = "TabDocNFSe";
			varReports.Nodes.Add(varRepDocNFSe);
			TreeNode varRepDocNFSeDet = (TreeNode)varReportAnaliticSintetic.Clone();
			varRepDocNFSeDet.Text = "NFSe : Dados analíticos";
			varRepDocNFSeDet.Name = "TabDocNFSeDet";
			varReports.Nodes.Add(varRepDocNFSeDet);
		}
		varReports = await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(await funcGetReportAsync(varReports, clsFeatureService.consReportExpDueControl), clsFeatureService.consReportDocNFeAverConf), clsFeatureService.consReportDocExpIndRemBal), clsFeatureService.consReportDocExpDirRemBal), clsFeatureService.consReportDocExportBalance), clsFeatureService.consReportDocNFePendCTe), clsFeatureService.consReportDocRetTercr), clsFeatureService.consReportDocRetPropr), clsFeatureService.consReportDocReject), clsFeatureService.consReportDocConf), clsFeatureService.consReportDocNFePendCLC), clsFeatureService.consReportDocNFePendSND), clsFeatureService.consReportDocCTeError), clsFeatureService.consReportDocNFeRptInCTe), clsFeatureService.consReportDocPrtSrv), clsFeatureService.constReportDocNFSeDet), clsFeatureService.constReportDocNFSe), clsFeatureService.consReportDocCanc), clsFeatureService.consReportDocCCe);
		TreeNode varRepPartner = clsFunction.funcGetDefaultNode("TabPartner", "Parceiros Comerciais", "ndTabPartner");
		varReports.Nodes.Add(varRepPartner);
		varReports.Expand();
		return varReports;
	}

	public async Task<TreeNode> funcGetReportAsync(TreeNode pRootNode, string pReportKey)
	{
		TreeNode varReportNode = await _clsFeatService.funcGetNodeAsync(pReportKey);
		if (varReportNode == null)
		{
			return pRootNode;
		}
		pRootNode.Nodes.Add(varReportNode);
		return pRootNode;
	}

	public void Treeview_DrawNode(object sender, DrawTreeNodeEventArgs e)
	{
		string varNodeTag = (string)e.Node.Tag;
		if (varNodeTag != null && e.Node.Name != null)
		{
			if (!varNodeTag.Contains("CKB"))
			{
				e.Node.HideCheckBox();
			}
			e.DrawDefault = true;
		}
	}

	public async Task<clsObjData> funcGetDataAsync(string pTabName, string pFeatType)
	{
		clsTabDataFactory varFactory = new clsTabDataFactory();
		clsObjData varObjData = new clsObjData();
		intDocTabData varclsTabData = varFactory.funcGetClass(pTabName);
		if (varclsTabData == null)
		{
			return varObjData;
		}
		DataResume varObjQtdVal = (DataResume)(await varclsTabData.funcResumeAsync(this, pFeatType)).GetObject("ObjQtdVlr");
		if (varObjQtdVal == null)
		{
			return varObjData;
		}
		varObjData.TabName = varclsTabData.TabName;
		varObjData.ObjDesct = varclsTabData.TabText;
		varObjData.ObjectId = pTabName;
		varObjData.QuantNum = varObjQtdVal.Quant;
		varObjData.QuantStr = $"Total: {varObjQtdVal.Quant} docs";
		varObjData.ValueNum = varObjQtdVal.Value;
		varObjData.ValueStr = "Valor: R$ " + clsFunction.funcFormatValue(varObjQtdVal.Value);
		return varObjData;
	}
}
