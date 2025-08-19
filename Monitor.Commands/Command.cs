using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using util.fiscal.io;

namespace Monitor.Commands;

public abstract class Command
{
	public clsPanelManager varclsPanelManager { get; set; }

	public clsTraceService varclsTracer { get; set; }

	public clsTabManager varclsTabManager { get; set; }

	public clsFilialManager varclsFilialManager { get; set; }

	public IWin32Window WindowOwner { get; set; }

	public abstract Task RunAsync();

	public Command()
	{
		if (!GetType().IsDefined(typeof(CommandAttributes), inherit: false))
		{
			Console.WriteLine("You must decorate Command Attributes class");
			throw new InvalidOperationException();
		}
	}

	protected void funcSetTaskStatus(clsTaskStatus pTaskStatus)
	{
		Application.DoEvents();
		varclsPanelManager.funcSet(pTaskStatus);
		Application.DoEvents();
	}

	public static IEnumerable<Type> GetAllCommands(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (type.GetCustomAttributes(typeof(CommandAttributes), inherit: true).Length != 0)
			{
				yield return type;
			}
		}
	}
}
