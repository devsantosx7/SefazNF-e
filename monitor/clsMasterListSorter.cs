using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Monitor;

public class clsMasterListSorter : Comparer<object>
{
	private readonly OLVColumn column;

	private readonly SortOrder sortOrder;

	public clsMasterListSorter(OLVColumn col, SortOrder order)
	{
		column = col;
		sortOrder = order;
	}

	public override int Compare(object x, object y)
	{
		IComparable xValue = column.GetValue(x) as IComparable;
		IComparable yValue = column.GetValue(y) as IComparable;
		int result = ((xValue != null && yValue != null) ? xValue.CompareTo(yValue) : ((xValue != null || yValue != null) ? ((xValue != null) ? 1 : (-1)) : 0));
		if (sortOrder == SortOrder.Ascending)
		{
			return result;
		}
		return -result;
	}
}
