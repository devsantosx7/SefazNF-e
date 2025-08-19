using System;
using System.Collections.Generic;
using System.Windows.Forms;
using util.fiscal.io;

namespace Monitor.Errors;

public static class ErrorsHandler
{
	public const string INSTALATIONS_ERROR_CODE = "555";

	public static bool HasInstallationsError(clsReturn returnedData)
	{
		return returnedData.Messages.Exists((clsMessage message) => message.Message.Contains("555") && message.Message.StartsWith("Unauthorized", StringComparison.OrdinalIgnoreCase));
	}

	private static bool HasOnboardOppened()
	{
		bool isOppened = false;
		foreach (object openForm in Application.OpenForms)
		{
			_ = openForm.GetType().Name;
			if (openForm.GetType().Equals(typeof(frmOnboard)))
			{
				isOppened = true;
				break;
			}
		}
		return isOppened;
	}

	private static void CloseFormsButNotMonitor()
	{
		List<Form> openForms = new List<Form>();
		foreach (Form f in Application.OpenForms)
		{
			openForms.Add(f);
		}
		foreach (Form varclsForm in openForms)
		{
			if (!CantCloseForm(varclsForm))
			{
				varclsForm.Dispose();
			}
		}
	}

	private static bool CantCloseForm(Form form)
	{
		if (!form.Name.Contains("Tab") && !form.Name.Contains("Panel") && !form.Name.Contains("frmOnboard") && !form.Name.Contains("frmMonitor") && !form.GetType().Equals(typeof(frmOnboard)))
		{
			return form.GetType().Equals(typeof(frmMonitor));
		}
		return true;
	}
}
