using System.Threading.Tasks;
using System.Windows.Forms;
using screen.fiscal.io;
using util.fiscal.io;

namespace Monitor.Commands;

[CommandAttributes(CommandName = "fixbracellorder")]
public class FixBracellCommand : Command
{
	public override async Task RunAsync()
	{
		funcSetTaskStatus(new clsTaskStatus("Corrigindo ordem das colunas..."));
		clsListViewUtils.funcSaveColumnsOrderBracell();
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		MessageBox.Show(base.WindowOwner, "Ordem corrigida com sucesso. Reabra as colunas !!!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}
}
