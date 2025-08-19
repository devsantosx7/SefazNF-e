using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using srv.fiscal.io;

namespace Monitor;

public class clsBalanceManager
{
	private clsDataObjectMeasure varclsDataObjectMeasure = new clsDataObjectMeasure(null);

	private clsBalanceService varclsBalanceService = new clsBalanceService();

	private clsDataParameter varclsDataParameter = new clsDataParameter();

	public async Task<bool> funcConfirmActionAsync(string pMeaExtId, decimal pTotalCost)
	{
		bool varclsReturn = false;
		ObjectMeasure varclsObjMeasure = await varclsDataObjectMeasure.funcGetItemByMeaExtIdAsync(pMeaExtId);
		if (varclsObjMeasure == null)
		{
			return varclsReturn;
		}
		clsBalanceService.BalanceModel varclsBalance = await varclsBalanceService.funcGetBalanceAsync(pMeaExtId);
		if (varclsBalance == null)
		{
			return varclsReturn;
		}
		return (!(varclsBalance.TotalBalc >= pTotalCost)) ? (await funcGetConfirmation02Async(varclsObjMeasure, varclsBalance, pTotalCost)) : (await funcGetConfirmation01Async(varclsObjMeasure, varclsBalance, pTotalCost));
	}

	public async Task<bool> funcConfirmConfigAsync(string pMeaExtId)
	{
		bool varclsReturn = false;
		ObjectMeasure varclsObjMeasure = await varclsDataObjectMeasure.funcGetItemByMeaExtIdAsync(pMeaExtId);
		if (varclsObjMeasure == null)
		{
			return varclsReturn;
		}
		clsBalanceService.BalanceModel varclsBalance = await varclsBalanceService.funcGetBalanceAsync(pMeaExtId);
		if (varclsObjMeasure == null)
		{
			return varclsReturn;
		}
		return (!(varclsBalance.TotalBalc > 0m)) ? (await funcGetConfirmation04Async(varclsObjMeasure, varclsBalance)) : (await funcGetConfirmation03Async(varclsObjMeasure, varclsBalance));
	}

	private async Task<bool> funcGetConfirmation01Async(ObjectMeasure clsObjMeasure, clsBalanceService.BalanceModel pclsBalance, decimal pTotalCost)
	{
		if (!string.IsNullOrEmpty(await varclsDataParameter.funcGetAsync("BalanceNotShowUserDec")))
		{
			return true;
		}
		if (new frmBalanceConfirm01(clsObjMeasure, pclsBalance, pTotalCost).ShowDialog().Equals(DialogResult.OK))
		{
			return true;
		}
		return false;
	}

	private async Task<bool> funcGetConfirmation02Async(ObjectMeasure clsObjMeasure, clsBalanceService.BalanceModel pclsBalance, decimal pTotalCost)
	{
		new frmBalanceConfirm02(clsObjMeasure, pclsBalance, pTotalCost).ShowDialog();
		return false;
	}

	private async Task<bool> funcGetConfirmation03Async(ObjectMeasure clsObjMeasure, clsBalanceService.BalanceModel pclsBalance)
	{
		if (!string.IsNullOrEmpty(await varclsDataParameter.funcGetAsync("BalanceNotShowUserDec")))
		{
			return true;
		}
		if (new frmBalanceConfirm03(clsObjMeasure, pclsBalance).ShowDialog().Equals(DialogResult.OK))
		{
			return true;
		}
		return false;
	}

	private async Task<bool> funcGetConfirmation04Async(ObjectMeasure clsObjMeasure, clsBalanceService.BalanceModel pclsBalance)
	{
		new frmBalanceConfirm04(clsObjMeasure, pclsBalance).ShowDialog();
		return false;
	}
}
