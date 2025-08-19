using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsTabDataFactory
{
	private static clsTabDataSqlHandler _SqlTabHandler = new clsTabDataSqlHandler();

	private static IEnumerable<Type> _TypeList = null;

	public intDocTabData funcGetClass(string pTabName)
	{
		intDocTabData varclsObject = null;
		string varTabName = _SqlTabHandler.funcGetTabName(pTabName);
		if (clsFunction.IsEmpty(varTabName))
		{
			return varclsObject;
		}
		try
		{
			if (_TypeList == null)
			{
				_TypeList = from r in Assembly.GetExecutingAssembly().GetTypes()
					where r.Name.StartsWith("clsTab")
					select r;
			}
			return (intDocTabData)Activator.CreateInstance(_TypeList.FirstOrDefault((Type r) => r.Name.Equals("cls" + varTabName)));
		}
		catch
		{
			return null;
		}
	}
}
