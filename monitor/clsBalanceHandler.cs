using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace Monitor;

public class clsBalanceHandler
{
	private clsBalanceService varclsService = new clsBalanceService();

	public ListView funcFillListView(ListView pListView, clsBalanceService.BalanceModel pclsBalance, decimal pTotalCost)
	{
		pListView.Items.Clear();
		pListView.HeaderStyle = ColumnHeaderStyle.None;
		ListViewItem varPlanItem = new ListViewItem("Saldo inicial");
		varPlanItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		varPlanItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, pclsBalance.TotalPlan));
		varPlanItem.SubItems.Add("+");
		varPlanItem.ForeColor = Color.Blue;
		pListView.Items.Add(varPlanItem);
		ListViewItem varRealItem = new ListViewItem("Já utilizado");
		varRealItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		varRealItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, pclsBalance.TotalReal));
		varRealItem.SubItems.Add("-");
		varRealItem.ForeColor = Color.DarkGray;
		pListView.Items.Add(varRealItem);
		ListViewItem varToUseItem = new ListViewItem("A utilizar (Previsto)");
		varToUseItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		varToUseItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, pTotalCost));
		varToUseItem.SubItems.Add("-");
		varToUseItem.ForeColor = Color.Green;
		pListView.Items.Add(varToUseItem);
		ListViewItem varBalanceItem = new ListViewItem("Saldo final");
		varBalanceItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		decimal varBalance = pclsBalance.TotalBalc - pTotalCost;
		varBalanceItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, varBalance));
		varBalanceItem.SubItems.Add("=");
		if (varBalance < 0m)
		{
			varToUseItem.ForeColor = Color.Red;
		}
		else
		{
			varToUseItem.ForeColor = Color.Black;
		}
		pListView.Items.Add(varBalanceItem);
		return pListView;
	}

	public ListView funcFillListView(ListView pListView, clsBalanceService.BalanceModel pclsBalance)
	{
		pListView.Items.Clear();
		pListView.HeaderStyle = ColumnHeaderStyle.None;
		ListViewItem varPlanItem = new ListViewItem("Saldo inicial");
		varPlanItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		varPlanItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, pclsBalance.TotalPlan));
		varPlanItem.SubItems.Add("+");
		varPlanItem.ForeColor = Color.Blue;
		pListView.Items.Add(varPlanItem);
		ListViewItem varRealItem = new ListViewItem("Já utilizado");
		varRealItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		varRealItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, pclsBalance.TotalReal));
		varRealItem.SubItems.Add("-");
		varRealItem.ForeColor = Color.DarkGray;
		pListView.Items.Add(varRealItem);
		ListViewItem varBalanceItem = new ListViewItem("Saldo atual");
		varBalanceItem.SubItems.Add(pclsBalance.MeaUnit.ToString());
		decimal varBalance = pclsBalance.TotalBalc;
		varBalanceItem.SubItems.Add(varclsService.funcGetValStr(pclsBalance.MeaType, varBalance));
		varBalanceItem.SubItems.Add("=");
		if (pclsBalance.TotalBalc <= 0m)
		{
			varBalanceItem.ForeColor = Color.Red;
		}
		else
		{
			varBalanceItem.ForeColor = Color.Black;
		}
		pListView.Items.Add(varBalanceItem);
		return pListView;
	}
}
