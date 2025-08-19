using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

[ReportClass]
public class clsTabDocJump : clsAbsTabData, intDocTabData, IDisposable
{
	private frmTabDocJump _FormTabData;

	public clsTabDocJump()
	{
		base.TabName = "TabDocJump";
		base.TabText = "Saltos de numeração";
		base.FeatExtId = clsFeatureService.consReportJumpOut;
		base.TabColor = Color.LightSkyBlue;
		_FormTabData = new frmTabDocJump(base.TabText, base.TabColor, base.FeatExtId);
		_FormTabData.EventTabManager += funcEventTabManager;
	}

	public bool ShowSumary()
	{
		return false;
	}

	public bool IsLocked()
	{
		if (_FormTabData == null)
		{
			return false;
		}
		return _FormTabData.funcIsLocked();
	}

	private void funcEventTabManager(object sender, EventTabManagerEventArgs e)
	{
		OnEventTabManager(e);
	}

	public TabPage funcGetTabPage()
	{
		TabPage obj = new TabPage
		{
			Name = base.TabName,
			Text = base.TabText,
			ImageIndex = 0,
			BackColor = base.TabColor
		};
		Panel varPlnContent = new Panel
		{
			AutoScroll = true,
			Dock = DockStyle.Fill
		};
		_FormTabData.TopLevel = false;
		_FormTabData.AutoScroll = true;
		_FormTabData.Dock = DockStyle.Fill;
		varPlnContent.Controls.Clear();
		varPlnContent.Controls.Add(_FormTabData);
		_FormTabData.Show();
		obj.Controls.Add(varPlnContent);
		return obj;
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		if (_FormTabData == null)
		{
			return new clsReturn();
		}
		clsReturn varclsReturnFunc = new clsReturn();
		if (_FormTabData == null)
		{
			return varclsReturnFunc;
		}
		varclsReturnFunc = await _FormTabData.funcLoadDataAsync(pclsDataFilter);
		_IsLoaded = true;
		return varclsReturnFunc;
	}

	public async Task<bool> funcLoadTagsAsync()
	{
		_ = _FormTabData;
		return false;
	}

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		if (_FormTabData == null)
		{
			return new clsReturn();
		}
		return await _FormTabData.funcExportToExcelAsync(onProgressChange);
	}

	public int funcGetTotalDocs()
	{
		if (_FormTabData == null)
		{
			return 0;
		}
		return _FormTabData.funcGetTotalDocs();
	}

	public decimal funcGetTotalValue()
	{
		if (_FormTabData == null)
		{
			return 0m;
		}
		return _FormTabData.funcGetTotalValue();
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		_ = _FormTabData;
		return new List<Document>();
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		_ = _FormTabData;
		return false;
	}

	public async Task<clsReturn> funcResumeAsync(clsDataFilter pclsDataFilter, string pFeatType)
	{
		return new clsReturn();
	}

	public void Dispose()
	{
		if (_FormTabData != null)
		{
			_FormTabData.Close();
			_FormTabData.Dispose();
			_FormTabData = null;
		}
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_Disposed)
		{
			if (disposing && _FormTabData != null)
			{
				_FormTabData.Close();
				_FormTabData.Dispose();
				_FormTabData = null;
			}
			_Disposed = true;
		}
	}

	public void funcResetLoadStatus()
	{
		_IsLoaded = false;
	}

	public void funcSearchText(string pSearchTerm, bool pSearchInteligent)
	{
		if (_FormTabData != null)
		{
			_FormTabData.funcSearchText(pSearchTerm, pSearchInteligent);
		}
	}

	public void funcColapseExpand()
	{
		_ = _FormTabData;
	}

	public async Task<bool> funcSetTagAsync(string pTagCode, bool pFocused, bool pChecked)
	{
		if (_FormTabData == null)
		{
			return false;
		}
		return await _FormTabData.funcSetTagAsync(pTagCode, pFocused, pChecked);
	}

	public async Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked)
	{
		return false;
	}

	public async Task<bool> funcRemoveDocNoteAsync(bool pFocused, bool pChecked)
	{
		return false;
	}
}
