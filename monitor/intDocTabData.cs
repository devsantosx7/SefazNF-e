using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public interface intDocTabData : IDisposable
{
	string TabName { get; }

	string TabText { get; }

	Color TabColor { get; }

	string FeatExtId { get; }

	event EventTabManagerHandler EventTabManager;

	bool IsLoaded();

	bool IsLocked();

	bool ShowSumary();

	TabPage funcGetTabPage();

	Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter);

	Task<clsReturn> funcResumeAsync(clsDataFilter pclsDataFilter, string pFeatType);

	Task<bool> funcSetTagAsync(string pTagCode, bool pFocused, bool pChecked);

	Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked);

	Task<bool> funcRemoveDocNoteAsync(bool pFocused, bool pChecked);

	void funcSearchText(string pSearchTerm, bool pSearchInteligent);

	Task<bool> funcLoadTagsAsync();

	Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange);

	void funcResetLoadStatus();

	Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst);

	Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked);

	void funcColapseExpand();

	int funcGetTotalDocs();

	decimal funcGetTotalValue();
}
