using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsFilialManager
{
	private TreeView _FilialTreeView;

	private ToolStripComboBox _FilialComboBox;

	private ToolStripButton _FilterButton;

	private clsFeatureService varclsFeatService = new clsFeatureService();

	private clsDataFilial varclsDataFilial = new clsDataFilial();

	private clsDataCertificate varclsDataCert = new clsDataCertificate();

	private clsDataParameter varclsDataParam = new clsDataParameter();

	private clsDataConfig varclsDataConfig = new clsDataConfig();

	private DateTime _LastScreenUpdate = new DateTime(1, 1, 1);

	private bool _IsLoading;

	private static string _DummyIdnt = string.Empty;

	public clsFilialManager(TreeView pTreeView, ToolStripComboBox pComboBox, ToolStripButton pFilterButton)
	{
		_FilialTreeView = pTreeView;
		_FilialComboBox = pComboBox;
		_FilterButton = pFilterButton;
	}

	public bool IsLoading()
	{
		return _IsLoading;
	}

	public void IsLoading(bool pValue)
	{
		_IsLoading = pValue;
	}

	public async Task<bool> funcRefreshAsync(bool pForce)
	{
		bool varMustUpdate = false;
		if (pForce)
		{
			varMustUpdate = true;
		}
		else if (_LastScreenUpdate <= DateTime.Now.AddMinutes(-25.0))
		{
			varMustUpdate = true;
		}
		if (!varMustUpdate)
		{
			return varMustUpdate;
		}
		await funcLoadAsync(pUseBuffer: false, null);
		return true;
	}

	public clsReturn funcRemoveFilial(FilialView pclsFilial)
	{
		clsReturn varclsReturn = new clsReturn();
		try
		{
			string varNodeName = "ndFilial-" + pclsFilial.CNPJ;
			int varNodeIndex = _FilialTreeView.Nodes.IndexOfKey(varNodeName);
			if (varNodeIndex < 0)
			{
				return varclsReturn;
			}
			_FilialTreeView.Nodes.RemoveAt(varNodeIndex);
		}
		catch (Exception pException)
		{
			varclsReturn.AddException(pException);
		}
		finally
		{
			_IsLoading = false;
		}
		return varclsReturn;
	}

	public async Task<clsReturn> funcLoadAsync(bool pUseBuffer, FilialView pclsFilial)
	{
		clsReturn varclsReturn = new clsReturn();
		try
		{
			if (_IsLoading)
			{
				return varclsReturn;
			}
			_IsLoading = true;
			if (!pUseBuffer && pclsFilial == null)
			{
				await varclsDataFilial.funcResetBufferAsync();
				await varclsDataCert.funcResetBufferAsync();
			}
			if (pclsFilial != null)
			{
				pclsFilial = await varclsDataFilial.funcGetItemByKeyAsync(pclsFilial.CNPJ);
			}
			Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
			List<FilialView> varFilialList = new List<FilialView>();
			if (pclsFilial != null)
			{
				varFilialList.Add(pclsFilial);
			}
			else
			{
				varFilialList = await varclsDataFilial.funcGetListAsync(pLoadDummy: true);
			}
			string varFilialFilter = await varclsDataParam.funcGetAsync("FilialShowHideFilter");
			List<clsObjectType> varObjectList = new List<clsObjectType>();
			TreeNode varNodeFunc01 = null;
			if (varFilialList.Count > 1)
			{
				varNodeFunc01 = await varclsFeatService.funcGetNodeAsync(clsFeatureService.consShowAllDocsKey);
			}
			string varFuncFeatType = await varclsFeatService.funcGetFeatTypeAsync(clsFeatureService.consShowAllDocsKey);
			TreeView filialTreeView = _FilialTreeView;
			object tag = (_FilialComboBox.Tag = varFuncFeatType);
			filialTreeView.Tag = tag;
			if (clsFunction.Contains(varFuncFeatType, "LOCK"))
			{
				varFilialFilter = string.Empty;
			}
			_FilialTreeView.SuspendLayout();
			_FilialTreeView.BeginUpdate();
			if (pclsFilial == null)
			{
				_FilialTreeView.Nodes.Clear();
			}
			if (varNodeFunc01 != null)
			{
				funcAddTreeNode(varNodeFunc01, pReplace: false);
				clsObjectType varObjItem = new clsObjectType
				{
					Name = varNodeFunc01.Text,
					Value = varNodeFunc01.Name
				};
				varObjectList.Add(varObjItem);
			}
			if (clsFunction.IsEmpty(_DummyIdnt))
			{
				_DummyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
			}
			foreach (FilialView varclsItem in varFilialList)
			{
				TreeNode varNodeSLF = await funcLoadItemAsync(varclsConfig, varclsItem, varFilialFilter);
				if (varNodeSLF != null)
				{
					if (pclsFilial == null)
					{
						_FilialTreeView.Nodes.Add(varNodeSLF);
					}
					else
					{
						funcAddTreeNode(varNodeSLF, pReplace: true);
					}
					varObjectList.Add(new clsObjectType
					{
						Name = varNodeSLF.Text,
						Value = varclsItem.CNPJ
					});
				}
			}
			await funcSetSelectedNodeAsync(_FilialTreeView, varFilialList);
			_FilialComboBox.BeginUpdate();
			_FilialComboBox.ComboBox.ValueMember = "Value";
			_FilialComboBox.ComboBox.DisplayMember = "Name";
			_FilialComboBox.ComboBox.DataSource = varObjectList;
			_FilialComboBox.EndUpdate();
			_FilialTreeView.Scrollable = false;
			if (_FilialTreeView.SelectedNode != null)
			{
				_FilialTreeView.SelectedNode.EnsureVisible();
			}
			_FilialTreeView.Scrollable = true;
			funcSetHideCheckBox(_FilialTreeView);
			_FilialTreeView.EndUpdate();
			_FilialTreeView.ResumeLayout();
			if (clsFunction.Contains(varFuncFeatType, "LOCK"))
			{
				_FilterButton.Visible = false;
				_FilterButton.Image = Resources.image_filter_off;
				_FilterButton.Checked = false;
			}
			else if (clsFunction.IsEmpty(varFilialFilter))
			{
				_FilterButton.Visible = true;
				_FilterButton.Image = Resources.image_filter_off;
				_FilterButton.Checked = false;
			}
			else
			{
				_FilterButton.Visible = true;
				_FilterButton.Image = Resources.image_filter_on;
				_FilterButton.Checked = true;
			}
			_LastScreenUpdate = DateTime.Now;
		}
		catch (Exception pException)
		{
			varclsReturn.AddMessage(new clsMessage("E", "999", "Ocorreu um erro ao carregar as empresas no Monitor.", pException));
		}
		finally
		{
			_IsLoading = false;
		}
		return varclsReturn;
	}

	private bool funcAddTreeNode(TreeNode pclsNodItem, bool pReplace)
	{
		if (pclsNodItem == null)
		{
			return false;
		}
		int varNodeIndex = _FilialTreeView.Nodes.IndexOfKey(pclsNodItem.Name);
		if (varNodeIndex >= 0 && !pReplace)
		{
			return true;
		}
		if (varNodeIndex < 0)
		{
			_FilialTreeView.Nodes.Add(pclsNodItem);
		}
		else
		{
			_FilialTreeView.Nodes.RemoveAt(varNodeIndex);
			_FilialTreeView.Nodes.Insert(varNodeIndex, pclsNodItem);
		}
		return true;
	}

	private async Task<TreeNode> funcLoadItemAsync(Configuration pclsConfig, FilialView pclsFilial, string pFilialFilter)
	{
		TreeNode varNodeSLF = null;
		if (!clsFunction.IsEmpty(pFilialFilter) && clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("Filial-Filter-" + pclsFilial.CNPJ, pBuffer: true, pGlobal: false, pOnlyBuffer: true)))
		{
			return varNodeSLF;
		}
		string varNodeName = "ndFilial-" + pclsFilial.CNPJ;
		varNodeSLF = clsFunction.funcGetDefaultNode(varNodeName, pclsFilial.NomeView, pclsFilial.CNPJ);
		TreeNode varNodeCNPJ = clsFunction.funcGetDefaultNode("ndCNPJ", "", pclsFilial.CNPJ);
		if (clsFunction.funcIsCNPJ(pclsFilial.CNPJView))
		{
			varNodeCNPJ.Text = "CNPJ : " + pclsFilial.CNPJView;
		}
		else
		{
			varNodeCNPJ.Text = "CPF : " + pclsFilial.CNPJView;
		}
		varNodeSLF.Nodes.Add(varNodeCNPJ);
		if (clsFunction.IsEqual(pclsFilial.CNPJ, _DummyIdnt))
		{
			return varNodeSLF;
		}
		Certificate varCertificate = await varclsDataCert.funcGetItemByKeyAsync(pclsFilial.Certificado, pclsFilial.CNPJ);
		TreeNode varNodeCertific = clsFunction.funcGetDefaultNode("ndCertifc", "", pclsFilial.CNPJ);
		varNodeSLF.Nodes.Add(varNodeCertific);
		string varCertType = clsFunction.funcGetValue(pclsFilial.CertifType);
		if (varCertType.Equals("A3") && varCertificate == null)
		{
			varNodeCertific.Text = "Certificado A3 : Sob demanda";
			varNodeCertific.ForeColor = Color.Blue;
		}
		else if (varCertType.Equals("A1") && varCertificate == null)
		{
			varNodeCertific.Text = "Certificado A1 : Vencido !!!";
			TreeNode treeNode = varNodeSLF;
			Color foreColor = (varNodeCertific.ForeColor = Color.Red);
			treeNode.ForeColor = foreColor;
		}
		else if (varCertificate != null)
		{
			varNodeCertific.Text = "Certificado " + varCertType;
			TreeNode varNodeCertificBegin = clsFunction.funcGetDefaultNode("ndCertifcBegin", "", pclsFilial.CNPJ);
			varNodeCertificBegin.Text = "Inicio : " + varCertificate.CrtDtBegin;
			varNodeCertific.Nodes.Add(varNodeCertificBegin);
			TreeNode varNodeCertificEnd = clsFunction.funcGetDefaultNode("ndCertifcEnd", "", pclsFilial.CNPJ);
			varNodeCertificEnd.Text = "Fim : " + varCertificate.CrtDtEndOf;
			varNodeCertific.Nodes.Add(varNodeCertificEnd);
			varNodeCertific.Collapse();
		}
		bool varIsScaOnTime = true;
		if (clsSrvGeral.funcIsTimeToScan(pclsConfig, pclsFilial, "NFEIN"))
		{
			varIsScaOnTime = false;
		}
		if (clsSrvGeral.funcIsTimeToScan(pclsConfig, pclsFilial, "CTEIN"))
		{
			varIsScaOnTime = false;
		}
		if (clsSrvGeral.funcIsTimeToScan(pclsConfig, pclsFilial, "CFEOUT"))
		{
			varIsScaOnTime = false;
		}
		if (!varIsScaOnTime && varNodeCertific.ForeColor.Equals(Color.Empty))
		{
			varNodeSLF.ForeColor = Color.Sienna;
		}
		if (!clsFunction.IsEmpty(pclsFilial.GetNFeAndEvent))
		{
			TreeNode varNodeNFe = clsFunction.funcGetDefaultNode("ndDadosNFe", "Dados de NFe", pclsFilial.CNPJ);
			varNodeSLF.Nodes.Add(varNodeNFe);
			TreeNode varNodeLastScanNFe = clsFunction.funcGetDefaultNode("ndLastScanNFe", "", pclsFilial.CNPJ);
			if (!clsFunction.IsEmpty(pclsFilial.NFeInFirstScan))
			{
				pclsFilial.LastScanNFe = string.Empty;
			}
			string varPointerScanNFe = (clsFunction.Contains(pclsFilial.NFeInStCode, "656", pIgnoreCase: true) ? pclsFilial.ScanNFeFault : pclsFilial.LastScanNFe);
			varNodeLastScanNFe.Text = "Procura : " + varPointerScanNFe;
			varNodeNFe.Nodes.Add(varNodeLastScanNFe);
			TreeNode varNodeNSUNFe = clsFunction.funcGetDefaultNode("ndNSUNFe", "", pclsFilial.CNPJ);
			varNodeNSUNFe.Text = "Ponteiro : " + pclsFilial.NSUNFe;
			varNodeNFe.Nodes.Add(varNodeNSUNFe);
			if (clsFunction.Contains(pclsFilial.NFeInStCode, "656", pIgnoreCase: true))
			{
				varNodeSLF.ForeColor = Color.FromArgb(0, 0, 255);
				TreeNode varNodeStatus = clsFunction.funcGetDefaultNode("ndDadosStatusNFe", "", pclsFilial.CNPJ);
				varNodeStatus.Text = pclsFilial.NFeInStCode + " - " + pclsFilial.NFeInStDesc;
				varNodeNFe.Nodes.Add(varNodeStatus);
			}
			varNodeNFe.Expand();
		}
		if (!clsFunction.IsEmpty(pclsFilial.GetCTeAndEvent))
		{
			TreeNode varNodeCTe = clsFunction.funcGetDefaultNode("ndDadosCTe", "Dados de CTe", pclsFilial.CNPJ);
			varNodeSLF.Nodes.Add(varNodeCTe);
			TreeNode varNodeLastScanCTe = clsFunction.funcGetDefaultNode("ndLastScanCTe", "", pclsFilial.CNPJ);
			if (!clsFunction.IsEmpty(pclsFilial.CTeInFirstScan))
			{
				pclsFilial.LastScanCTe = string.Empty;
			}
			string varPointerScanCTe = (clsFunction.Contains(pclsFilial.CTeInStCode, "656", pIgnoreCase: true) ? pclsFilial.ScanCTeFault : pclsFilial.LastScanCTe);
			varNodeLastScanCTe.Text = "Procura : " + varPointerScanCTe;
			varNodeCTe.Nodes.Add(varNodeLastScanCTe);
			TreeNode varNodeNSUCTe = clsFunction.funcGetDefaultNode("ndNSUCTe", "", pclsFilial.CNPJ);
			varNodeNSUCTe.Text = "Ponteiro : " + pclsFilial.NSUCTe;
			varNodeCTe.Nodes.Add(varNodeNSUCTe);
			if (clsFunction.Contains(pclsFilial.CTeInStCode, "656", pIgnoreCase: true))
			{
				varNodeSLF.ForeColor = Color.FromArgb(0, 0, 255);
				TreeNode varNodeStatus2 = clsFunction.funcGetDefaultNode("ndDadosStatusCTe", "", pclsFilial.CNPJ);
				varNodeStatus2.Text = pclsFilial.CTeInStCode + " - " + pclsFilial.CTeInStDesc;
				varNodeCTe.Nodes.Add(varNodeStatus2);
			}
			varNodeCTe.Expand();
		}
		if (!clsFunction.IsEmpty(pclsFilial.GetNFSeAndEvent))
		{
			TreeNode varNodeNFSe = clsFunction.funcGetDefaultNode("ndDadosNFSe", "Dados de NFSe", pclsFilial.CNPJ);
			varNodeSLF.Nodes.Add(varNodeNFSe);
			TreeNode varNodeLastScanNFSe = clsFunction.funcGetDefaultNode("ndLastScanNFSe", "", pclsFilial.CNPJ);
			if (!clsFunction.IsEmpty(pclsFilial.NFSeInFirstScan))
			{
				pclsFilial.LastScanNFSe = string.Empty;
			}
			varNodeLastScanNFSe.Text = "Procura : " + pclsFilial.LastScanNFSe;
			varNodeNFSe.Nodes.Add(varNodeLastScanNFSe);
			TreeNode varNodeNSUNFSe = clsFunction.funcGetDefaultNode("ndNSUNFSe", "", pclsFilial.CNPJ);
			varNodeNSUNFSe.Text = "Ponteiro : " + pclsFilial.NSUNFSe;
			varNodeNFSe.Nodes.Add(varNodeNSUNFSe);
			varNodeNFSe.Expand();
		}
		if (!clsFunction.IsEmpty(pclsFilial.GetCFeOutAndEvent))
		{
			TreeNode varNodeCFeOut = clsFunction.funcGetDefaultNode("ndDadosCFe", "Dados de CFe", pclsFilial.CNPJ);
			varNodeSLF.Nodes.Add(varNodeCFeOut);
			TreeNode varNodeLastScanCFeOut = clsFunction.funcGetDefaultNode("ndLastScanCFeOut", "", pclsFilial.CNPJ);
			if (!clsFunction.IsEmpty(pclsFilial.CFeOutFirstScan))
			{
				pclsFilial.LastScanCFeOut = string.Empty;
			}
			varNodeLastScanCFeOut.Text = "Procura : " + pclsFilial.LastScanCFeOut;
			varNodeCFeOut.Nodes.Add(varNodeLastScanCFeOut);
			varNodeCFeOut.Expand();
		}
		if (!clsFunction.IsEmpty(pclsFilial.GetMDFeAndEvent))
		{
			TreeNode varNodeMDFe = clsFunction.funcGetDefaultNode("ndDadosMDFe", "Dados de MDFe", pclsFilial.CNPJ);
			varNodeSLF.Nodes.Add(varNodeMDFe);
			TreeNode varNodeLastScanMDFe = clsFunction.funcGetDefaultNode("ndLastScanMDFe", "", pclsFilial.CNPJ);
			if (!clsFunction.IsEmpty(pclsFilial.MDFeInFirstScan))
			{
				pclsFilial.LastScanMDFe = string.Empty;
			}
			string varPointerScanMDFe = (clsFunction.Contains(pclsFilial.MDFeInStCode, "656", pIgnoreCase: true) ? pclsFilial.ScanMDFeFault : pclsFilial.LastScanMDFe);
			varNodeLastScanMDFe.Text = "Procura : " + varPointerScanMDFe;
			varNodeMDFe.Nodes.Add(varNodeLastScanMDFe);
			TreeNode varNodeNSUMDFe = clsFunction.funcGetDefaultNode("ndNSUMDFe", "", pclsFilial.CNPJ);
			varNodeNSUMDFe.Text = "Ponteiro : " + pclsFilial.NSUMDFe;
			varNodeMDFe.Nodes.Add(varNodeNSUMDFe);
			if (clsFunction.Contains(pclsFilial.MDFeInStCode, "656", pIgnoreCase: true))
			{
				varNodeSLF.ForeColor = Color.FromArgb(0, 0, 255);
				TreeNode varNodeStatus3 = clsFunction.funcGetDefaultNode("ndDadosStatusMDFe", "", pclsFilial.CNPJ);
				varNodeStatus3.Text = pclsFilial.MDFeInStCode + " - " + pclsFilial.MDFeInStDesc;
				varNodeMDFe.Nodes.Add(varNodeStatus3);
			}
			varNodeMDFe.Expand();
		}
		return varNodeSLF;
	}

	public async Task<bool> funcSetNodeCheckBoxAsync(bool pChecked)
	{
		bool varclsReturn = false;
		try
		{
			new List<Parameter>();
			Dictionary<string, bool> varNodeList = new Dictionary<string, bool>();
			foreach (TreeNode node in _FilialTreeView.Nodes)
			{
				node.Checked = pChecked;
			}
			string varIsChecked = clsFunction.funcConvBoolToStr(pChecked);
			if (await varclsDataParam.funcSetByStartAsync("ActFilial-", varIsChecked) == _FilialTreeView.Nodes.Count)
			{
				return true;
			}
			Configuration obj = await varclsDataConfig.funcGetItemByKeyAsync();
			int varParallelThreads = clsFunction.funcConvStrToInt(obj.ParallelThreads);
			if (!obj.IsParallel())
			{
				varParallelThreads = 1;
			}
			SemaphoreSlim varSemaphore = new SemaphoreSlim(varParallelThreads);
			await Task.WhenAll(((IEnumerable<KeyValuePair<string, bool>>)varNodeList).Select((Func<KeyValuePair<string, bool>, Task>)async delegate(KeyValuePair<string, bool> varNodeName)
			{
				new clsReturn();
				try
				{
					await varSemaphore.WaitAsync();
					await varclsDataParam.funcSetAsync($"ActFilial-{varNodeName}", varIsChecked);
				}
				finally
				{
					varSemaphore.Release();
				}
			}));
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
			varclsReturn = false;
		}
		return varclsReturn;
	}

	private async Task<bool> funcSetSelectedNodeAsync(TreeView pObjTreeView, List<FilialView> pFilialList)
	{
		if (pObjTreeView == null)
		{
			return false;
		}
		if (pFilialList == null)
		{
			return false;
		}
		if (pFilialList.Count <= 0)
		{
			return false;
		}
		bool varHasSelected = false;
		bool varHasNoChecked = false;
		string varFuncFeatType = (string)pObjTreeView.Tag;
		if (clsFunction.IsEmpty(varFuncFeatType))
		{
			return false;
		}
		TreeNode varNodeSelectAll = funcGetNodeSelectAll();
		bool varAllNodesSelected = false;
		if (varNodeSelectAll != null && !clsFunction.Contains(varFuncFeatType, "LOCK") && !clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("ActFilial-" + varNodeSelectAll.Name)))
		{
			varAllNodesSelected = true;
		}
		string varFilialFocused = await varclsDataParam.funcGetAsync("LAST-FILIAL-FOCUSED");
		foreach (TreeNode varTreeNode in _FilialTreeView.Nodes)
		{
			if (varTreeNode.Tag == null)
			{
				continue;
			}
			string varNodeCNPJ = (string)varTreeNode.Tag;
			if (clsFunction.IsEmpty(varNodeCNPJ))
			{
				continue;
			}
			if (clsFunction.IsEqual(varFilialFocused, varNodeCNPJ))
			{
				_FilialTreeView.SelectedNode = varTreeNode;
			}
			if (clsFunction.Contains(varFuncFeatType, "LOCK"))
			{
				string varCNPJSelected = clsFunction.funcGetValue(await varclsDataParam.funcGetAsync("ActFilial", pBuffer: true, pGlobal: false, pOnlyBuffer: true));
				if (clsFunction.IsEqual(varNodeCNPJ, varCNPJSelected))
				{
					_FilialTreeView.SelectedNode = varTreeNode;
					_FilialTreeView.SelectedNode.BackColor = Color.CornflowerBlue;
					_FilialComboBox.ComboBox.SelectedValue = varNodeCNPJ;
					varHasSelected = true;
				}
			}
			else if (varAllNodesSelected)
			{
				varTreeNode.Checked = varAllNodesSelected;
			}
			else
			{
				varTreeNode.Checked = !clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("ActFilial-" + varNodeCNPJ, pBuffer: true, pGlobal: false, pOnlyBuffer: true));
				if (varTreeNode.Checked)
				{
					varHasSelected = true;
				}
			}
		}
		if (varNodeSelectAll != null)
		{
			foreach (TreeNode varTreeNode2 in _FilialTreeView.Nodes)
			{
				if (!clsFunction.Contains(varTreeNode2.Name, "-FUNC-") && !varTreeNode2.Checked)
				{
					varHasNoChecked = true;
					break;
				}
			}
			if (varNodeSelectAll.Checked && varHasNoChecked)
			{
				varNodeSelectAll.Checked = false;
			}
			else if (!varNodeSelectAll.Checked && !varHasNoChecked)
			{
				varNodeSelectAll.Checked = true;
			}
		}
		if (varHasSelected)
		{
			return true;
		}
		foreach (TreeNode varTreeNode3 in _FilialTreeView.Nodes)
		{
			if (!clsFunction.IsEmpty((string)varTreeNode3.Tag) && !clsFunction.Contains(varFuncFeatType, "LOCK"))
			{
				varTreeNode3.Checked = true;
			}
		}
		return true;
	}

	public async Task funcDefineFilterListAsync(string pFilialFilter)
	{
		if (_FilialTreeView == null)
		{
			return;
		}
		string varFuncFeatType = (string)_FilialTreeView.Tag;
		List<Parameter> varParamList = new List<Parameter>();
		await varclsDataParam.funcSetByStartAsync("Filial-Filter-", "");
		if (clsFunction.IsEmpty(pFilialFilter))
		{
			return;
		}
		foreach (TreeNode varTreeNode in _FilialTreeView.Nodes)
		{
			string varNodeCNPJ = (string)varTreeNode.Tag;
			if (!clsFunction.IsEmpty(varNodeCNPJ) && !clsFunction.Contains(varFuncFeatType, "LOCK") && !clsFunction.Contains(varTreeNode.Name, "-FUNC-") && varTreeNode.Checked)
			{
				varParamList.Add(new Parameter("Filial-Filter-" + varNodeCNPJ, "X"));
			}
		}
		if (varParamList.Count > 0)
		{
			await varclsDataParam.funcSetAsync(varParamList);
		}
	}

	private bool funcSetHideCheckBox(TreeView pObjTreeView)
	{
		string varFuncFeatType = (string)pObjTreeView.Tag;
		if (clsFunction.IsEmpty(varFuncFeatType))
		{
			return false;
		}
		if (clsFunction.Contains(varFuncFeatType, "LOCK"))
		{
			pObjTreeView.CheckBoxes = false;
			return true;
		}
		pObjTreeView.CheckBoxes = true;
		foreach (TreeNode node in pObjTreeView.Nodes)
		{
			foreach (TreeNode node2 in node.Nodes)
			{
				node2.HideCheckBox();
				foreach (TreeNode node3 in node2.Nodes)
				{
					node3.HideCheckBox();
					foreach (TreeNode node4 in node3.Nodes)
					{
						node4.HideCheckBox();
					}
				}
			}
		}
		return true;
	}

	public TreeNode funcGetNodeSelectAll()
	{
		TreeNode varNodeSelectAll = null;
		foreach (TreeNode varTreeNode in _FilialTreeView.Nodes)
		{
			if (clsFunction.Contains(varTreeNode.Name, "-FUNC-"))
			{
				varNodeSelectAll = varTreeNode;
				break;
			}
		}
		return varNodeSelectAll;
	}
}
