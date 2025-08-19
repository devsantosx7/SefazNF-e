using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

internal static class Program
{
	private static Mutex varMutex = new Mutex(initiallyOwned: true, "71bfef4e-c51e-498f-9391-e2bc11f541b8");

	public static bool LaunchedViaStartup { get; set; }

	public static bool MustRunDbaOptimize { get; set; }

	public static bool NotLoadDataInStart { get; set; }

	[STAThread]
	private static void Main(string[] args)
	{
		clsFunction.funcSetThreadCulture();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.ThreadException += clsScreenGeral.Application_ThreadException;
		AppDomain.CurrentDomain.UnhandledException += clsScreenGeral.CurrentDomain_UnhandledException;
		AppDomain.CurrentDomain.AssemblyResolve += clsScreenGeral.CurrentDomain_LoadDependency;
		if (new clsDbaConStr().funcLoadDbaConfigBuffer().Count > 1)
		{
			using frmDbaManager varfrmDbaConsStr = new frmDbaManager(pIsStartup: true);
			if (!varfrmDbaConsStr.ShowDialog().Equals(DialogResult.OK))
			{
				Application.Exit();
				return;
			}
		}
		try
		{
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
		}
		catch
		{
		}
		new clsSqlSrvLocUtil().funcTryToStartAsync();
		LaunchedViaStartup = false;
		try
		{
			if (args == null)
			{
				LaunchedViaStartup = (MustRunDbaOptimize = false);
			}
			else
			{
				if (args.FirstOrDefault((string r) => r.Contains("startup") || r.Contains("STARTUP")) != null)
				{
					LaunchedViaStartup = true;
				}
				if (args.FirstOrDefault((string r) => r.Contains("dbaoptimize") || r.Contains("DBAOPTIMIZE")) != null)
				{
					MustRunDbaOptimize = true;
				}
			}
		}
		catch
		{
			LaunchedViaStartup = (MustRunDbaOptimize = false);
		}
		try
		{
			clsWindowsHelper clsWindowsHelper = new clsWindowsHelper();
			Process varCurrent = Process.GetCurrentProcess();
			if (clsWindowsHelper.funcCheckInstances(varCurrent, varMutex, LaunchedViaStartup).Equals(clsWindowsHelper.enReturn.enCloseApplication))
			{
				Environment.Exit(0);
				return;
			}
		}
		catch
		{
		}
		try
		{
			string text = Control.ModifierKeys.ToString().ToUpper();
			if (text.Equals("SHIFT"))
			{
				NotLoadDataInStart = true;
			}
			if (text.Equals("CTRL"))
			{
				MustRunDbaOptimize = true;
			}
		}
		catch
		{
		}
		try
		{
			new clsUpdaterService().funcCheckTask();
		}
		catch
		{
		}
		frmMonitor varfrmMonitor = new frmMonitor();
		if (LaunchedViaStartup)
		{
			varfrmMonitor.Hide();
			varfrmMonitor.Visible = false;
			varfrmMonitor.ShowInTaskbar = false;
		}
		Application.Run(varfrmMonitor);
	}
}
