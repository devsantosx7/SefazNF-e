using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsTabManager
{
	private intDocTabData _TabActual;

	private List<intDocTabData> _TabList = new List<intDocTabData>();

	private clsDataParameter _clsParamHandler = new clsDataParameter();

	private clsTabDataSqlHandler _clsTabDataSqlHandler = new clsTabDataSqlHandler();

	private clsTreeViewHandler _clsTreeHandler = new clsTreeViewHandler();

	public event EventTabManagerHandler EventTabManager;

	protected virtual void OnEventTabManager(EventTabManagerEventArgs e)
	{
		this.EventTabManager?.Invoke(this, e);
	}

	public async Task<List<intDocTabData>> ListAsync()
	{
		if (_TabList.Count == 0)
		{
			await funcLoadTabListAsync();
		}
		return _TabList;
	}

	public void funcResetLoadStatus()
	{
		foreach (intDocTabData tab in _TabList)
		{
			tab.funcResetLoadStatus();
		}
	}

	public int funcCount()
	{
		return _TabList.Count;
	}

	public intDocTabData Get(string pTabName)
	{
		string varTabName = _clsTabDataSqlHandler.funcGetTabName(pTabName);
		return _TabList.FirstOrDefault((intDocTabData r) => r.TabName.Equals(varTabName));
	}

	public intDocTabData GetTab(string pTabName, string pExtId)
	{
		_clsTabDataSqlHandler.funcGetTabName(pTabName);
		return _TabList.FirstOrDefault((intDocTabData r) => r.FeatExtId.Equals(pExtId));
	}

	public async void funcSetToolsAsync(ToolStrip pToolStrip, TreeView pclsTreeFeature)
	{
		funcSetToolStrip(pToolStrip);
		funcSetDataFilterAsync(pclsTreeFeature);
	}

	private async void funcSetDataFilterAsync(TreeView pclsTreeFeature)
	{
		if (pclsTreeFeature != null)
		{
			_ = string.Empty;
			string varTagDisable = ((_TabActual == null) ? "#NOT-WITHOUT-TAB" : ("#NOT-" + _TabActual.TabName.ToUpper()));
			pclsTreeFeature = await _clsTreeHandler.funcSyncBufferAsync(pclsTreeFeature, varTagDisable);
		}
	}

	private void funcSetToolStrip(ToolStrip pToolStrip)
	{
		if (pToolStrip == null)
		{
			return;
		}
		string varTagDisable = string.Empty;
		_ = string.Empty;
		if (_TabActual != null)
		{
			varTagDisable = "#NOT-" + _TabActual.TabName.ToUpper();
			_ = "#PLUG-BUTTON-ONLY-IN-" + _TabActual.TabText.ToUpper();
		}
		else
		{
			varTagDisable = "#NOT-WITHOUT-TAB";
		}
		pToolStrip.SuspendLayout();
		foreach (ToolStripItem varControl in pToolStrip.Items)
		{
			if (varControl.Tag != null)
			{
				string varTag = (string)varControl.Tag;
				if (string.IsNullOrEmpty(varTag))
				{
					varControl.Visible = true;
				}
				else if (string.IsNullOrEmpty(varTagDisable))
				{
					varControl.Visible = true;
				}
				else if (varTag.Contains(varTagDisable))
				{
					varControl.Visible = false;
				}
				else
				{
					varControl.Visible = true;
				}
			}
		}
		pToolStrip.ResumeLayout();
	}

	public async Task<bool> AddAsync(string pTabName, bool pSelect)
	{
		string varTabName = _clsTabDataSqlHandler.funcGetTabName(pTabName);
		intDocTabData varTabData = funcGetObjByName(varTabName);
		if (varTabData == null)
		{
			return false;
		}
		_TabList.Add(varTabData);
		await _clsParamHandler.funcSetAsync(varTabData.TabName, "X");
		if (pSelect)
		{
			await SelectAsync(varTabName);
		}
		return true;
	}

	public async Task<bool> RemoveAsync(string pTabName)
	{
		string varTabName = _clsTabDataSqlHandler.funcGetTabName(pTabName);
		intDocTabData varTabData = _TabList.FirstOrDefault((intDocTabData r) => r.TabName.Equals(varTabName));
		if (varTabData != null)
		{
			_TabList.Remove(varTabData);
			varTabData.Dispose();
		}
		await SelectAsync(string.Empty);
		await _clsParamHandler.funcSetAsync(varTabName, "");
		return true;
	}

	public async Task<intDocTabData> SelectAsync(string pTabName)
	{
		string varTabName = _clsTabDataSqlHandler.funcGetTabName(pTabName);
		if (_TabList.Count == 0)
		{
			_TabActual = null;
		}
		else if (string.IsNullOrEmpty(varTabName))
		{
			_TabActual = _TabList.First();
		}
		else
		{
			_TabActual = Get(varTabName);
		}
		await _clsParamHandler.funcSetAsync("TabDataSelected", varTabName);
		return _TabActual;
	}

	public intDocTabData Selected()
	{
		return _TabActual;
	}

	private async Task<clsReturn> funcLoadTabListAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		new clsFeatureService();
		try
		{
			if (await funcIsOpenAsync("TabDocInb"))
			{
				await AddAsync("TabDocInb", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocOut"))
			{
				await AddAsync("TabDocOut", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocJump"))
			{
				await AddAsync("TabDocJump", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFe"))
			{
				await AddAsync("TabDocNFe", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFeDet"))
			{
				await AddAsync("TabDocNFeDet", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocCTe"))
			{
				await AddAsync("TabDocCTe", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocCTeDet"))
			{
				await AddAsync("TabDocCTeDet", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFSe"))
			{
				await AddAsync("TabDocNFSe", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFSeDet"))
			{
				await AddAsync("TabDocNFSeDet", pSelect: false);
			}
			if (await funcIsOpenAsync("TabPartner"))
			{
				await AddAsync("TabPartner", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocCanc"))
			{
				await AddAsync("TabDocCanc", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocCCe"))
			{
				await AddAsync("TabDocCCe", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocRetTercr"))
			{
				await AddAsync("TabDocRetTercr", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocRetPropr"))
			{
				await AddAsync("TabDocRetPropr", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocReject"))
			{
				await AddAsync("TabDocReject", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocConf"))
			{
				await AddAsync("TabDocConf", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFePendCTe"))
			{
				await AddAsync("TabDocNFePendCTe", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFePendCLC"))
			{
				await AddAsync("TabDocNFePendCLC", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFePendSND"))
			{
				await AddAsync("TabDocNFePendSND", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocCTeError"))
			{
				await AddAsync("TabDocCTeError", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFeRptInCTe"))
			{
				await AddAsync("TabDocNFeRptInCTe", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocPrtSrv"))
			{
				await AddAsync("TabDocPrtSrv", pSelect: false);
			}
			if (await funcIsOpenAsync("TabBatch"))
			{
				await AddAsync("TabBatch", pSelect: false);
			}
			if (await funcIsOpenAsync("TabAuditor"))
			{
				await AddAsync("TabAuditor", pSelect: false);
			}
			if (await funcIsOpenAsync("TabTools"))
			{
				await AddAsync("TabTools", pSelect: false);
			}
			if (await funcIsOpenAsync("TabExpDueControl"))
			{
				await AddAsync("TabExpDueControl", pSelect: false);
			}
			if (await funcIsOpenAsync("TabDocNFeAverConf"))
			{
				await AddAsync("TabDocNFeAverConf", pSelect: false);
			}
			if (await funcIsOpenAsync("TabExpIndRem"))
			{
				await AddAsync("TabExpIndRem", pSelect: false);
			}
			if (await funcIsOpenAsync("TabExpDirRem"))
			{
				await AddAsync("TabExpDirRem", pSelect: false);
			}
			if (await funcIsOpenAsync("TabExpBalance"))
			{
				await AddAsync("TabExpBalance", pSelect: false);
			}
			if (_TabList.Count == 0)
			{
				await AddAsync("TabDocInb", pSelect: false);
				await AddAsync("TabDocOut", pSelect: false);
			}
			if (!(await funcIsOpenAsync("TabTools")))
			{
				await AddAsync("TabTools", pSelect: false);
			}
			List<intDocTabData> varTabTempList = _TabList.ToList();
			foreach (intDocTabData varTabItem in varTabTempList)
			{
				if (varTabItem.GetType().GetCustomAttribute<ReportClassAttribute>() != null && !(await clsScreenGeral.funcHasAccessAsync("ACCESS-REPORT", "VIEW", varTabItem.TabName, pShow: false)))
				{
					await RemoveAsync(varTabItem.TabName);
				}
			}
			string varTabName = clsFunction.funcGetValue(await _clsParamHandler.funcGetAsync("TabDataSelected"));
			varTabName = _clsTabDataSqlHandler.funcGetTabName(varTabName);
			await SelectAsync(varTabName);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private async Task<bool> funcIsOpenAsync(string pTabName)
	{
		string varTabName = _clsTabDataSqlHandler.funcGetTabName(pTabName);
		return clsFunction.funcConvStrToBool(await _clsParamHandler.funcGetAsync(varTabName));
	}

	private intDocTabData funcGetObjByName(string pTabName)
	{
		intDocTabData varTabData = new clsTabDataFactory().funcGetClass(pTabName);
		if (varTabData == null)
		{
			return varTabData;
		}
		varTabData.EventTabManager -= funcEventTabManager;
		varTabData.EventTabManager += funcEventTabManager;
		return varTabData;
	}

	private void funcEventTabManager(object sender, EventTabManagerEventArgs e)
	{
		OnEventTabManager(e);
	}
}
