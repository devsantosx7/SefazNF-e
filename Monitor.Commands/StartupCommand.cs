using System;
using System.Threading.Tasks;

namespace Monitor.Commands;

[CommandAttributes(CommandName = "startup")]
public class StartupCommand : Command
{
	public override Task RunAsync()
	{
		throw new NotImplementedException();
	}
}
