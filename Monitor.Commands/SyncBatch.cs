using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using srv.fiscal.io;

namespace Monitor.Commands;

[CommandAttributes(CommandName = "syncbatch")]
public class SyncBatch : Command
{
	public override async Task RunAsync()
	{
		foreach (BatchHead varBatchHead in await new clsDataBatchHead().funcGetListAsync())
		{
			await new clsBatchService().funcDefinePercAsync(varBatchHead.BthId);
		}
		MessageBox.Show(base.WindowOwner, "Lotes sincronizados com sucesso !!!", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}
}
