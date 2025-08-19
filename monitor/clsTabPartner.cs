using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor;

[ReportClass]
public class clsTabPartner : clsAbsTabData, intDocTabData, IDisposable
{
	private frmTabPartner _FormTabData;

	public clsTabPartner()
	{
		base.TabName = "TabPartner";
		base.TabText = "Parceiros Comerciais";
		base.TabColor = Color.DarkKhaki;
		_FormTabData = new frmTabPartner(base.TabColor, base.FeatExtId);
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
		if (_FormTabData == null)
		{
			return false;
		}
		return await _FormTabData.funcLoadTagsAsync();
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
		if (_FormTabData == null)
		{
			return new List<Document>();
		}
		return await _FormTabData.funcGetDocListAsync(pFocused, pChecked, pSyncFromDbaFirst);
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		if (_FormTabData == null)
		{
			return false;
		}
		return await _FormTabData.funcRefreshItensAsync(pFocused, pChecked);
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
	}

	public void funcColapseExpand()
	{
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
		if (_FormTabData == null)
		{
			return false;
		}
		return await _FormTabData.funcSetDocNoteAsync(pFocused, pChecked);
	}

	public async Task<bool> funcRemoveDocNoteAsync(bool pFocused, bool pChecked)
	{
		return false;
	}
}
