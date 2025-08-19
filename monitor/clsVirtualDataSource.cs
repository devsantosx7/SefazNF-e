using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;

namespace Monitor;

public class clsVirtualDataSource : AbstractVirtualListDataSource
{
	private dynamic Objects;

	public clsVirtualDataSource(VirtualObjectListView pListView, dynamic pObjectList)
		: base(pListView)
	{
		Objects = pObjectList;
	}

	public override int GetObjectIndex(object model)
	{
		return Objects.IndexOf((Document)model);
	}

	public override object GetNthObject(int n)
	{
		return Objects[n % Objects.Count];
	}

	public override int GetObjectCount()
	{
		return Objects.Count;
	}

	public override void Sort(OLVColumn column, SortOrder order)
	{
		Objects.Sort(new clsMasterListSorter(column, order));
	}

	public override int SearchText(string value, int first, int last, OLVColumn column)
	{
		return AbstractVirtualListDataSource.DefaultSearchText(value, first, last, column, this);
	}
}
