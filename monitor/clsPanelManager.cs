using System;
using System.Windows.Forms;
using util.fiscal.io;

namespace Monitor;

public class clsPanelManager
{
	private Panel _PanelBoard;

	private Form _CurrentForm;

	public event EventPanelManagerHandler EventPanelManager;

	protected virtual void OnEventPanelManager(EventPanelManagerEventArgs e)
	{
		this.EventPanelManager?.Invoke(this, e);
	}

	public clsPanelManager(Panel pPanel)
	{
		_PanelBoard = pPanel;
	}

	public bool IsVisible()
	{
		if (_PanelBoard == null)
		{
			return false;
		}
		if (_CurrentForm == null)
		{
			return false;
		}
		if (!_PanelBoard.Visible)
		{
			return false;
		}
		return true;
	}

	public void funcSet(clsTaskStatus pclsTaskStatus)
	{
		Type varFormType = typeof(frmPanelUserStatus);
		if (!pclsTaskStatus.Show)
		{
			_PanelBoard.Visible = false;
		}
		else
		{
			if (!SystemInformation.UserInteractive)
			{
				return;
			}
			try
			{
				_PanelBoard.SuspendLayout();
				if (pclsTaskStatus.FormName.Equals(enPanelName.Undefined))
				{
					varFormType = typeof(frmPanelUserStatus);
				}
				else if (pclsTaskStatus.FormName.Equals(enPanelName.PanelXmlValidator))
				{
					varFormType = typeof(frmPanelXmlValidator);
				}
				else if (pclsTaskStatus.FormName.Equals(enPanelName.PanelFiscalServer))
				{
					varFormType = typeof(frmPanelFiscalServer);
				}
				else if (pclsTaskStatus.FormName.Equals(enPanelName.PanelMoreReports))
				{
					varFormType = typeof(frmPanelMoreReports);
				}
				else if (pclsTaskStatus.FormName.Equals(enPanelName.PanelStorageAlmostFull))
				{
					varFormType = typeof(frmPanelStorageAlmostFull);
				}
				else if (pclsTaskStatus.FormName.Equals(enPanelName.PanelStorageFull))
				{
					varFormType = typeof(frmPanelStorageFull);
				}
				bool varLoadForm = false;
				if (_CurrentForm == null)
				{
					varLoadForm = true;
				}
				else if (!_CurrentForm.GetType().Equals(varFormType))
				{
					varLoadForm = true;
				}
				else if (pclsTaskStatus.Reload)
				{
					varLoadForm = true;
				}
				if (varLoadForm)
				{
					_CurrentForm = (Form)Activator.CreateInstance(varFormType);
					_CurrentForm.TopLevel = false;
					_CurrentForm.AutoScroll = true;
					_CurrentForm.Dock = DockStyle.Fill;
					_PanelBoard.Controls.Clear();
					_CurrentForm.FormClosed -= _CurrentForm_FormClosed;
					_CurrentForm.FormClosed += _CurrentForm_FormClosed;
					_PanelBoard.Height = _CurrentForm.Height;
					_PanelBoard.Controls.Add(_CurrentForm);
				}
				pclsTaskStatus.PanelHeight = 0;
				_CurrentForm.Tag = pclsTaskStatus;
				_CurrentForm.Text = $"UserPanel-{new Random().Next()}";
				_CurrentForm.Show();
				_PanelBoard.Visible = true;
				if (pclsTaskStatus.PanelHeight > 0)
				{
					_PanelBoard.Height = pclsTaskStatus.PanelHeight;
				}
				else
				{
					_PanelBoard.Height = _CurrentForm.Height + 2;
				}
			}
			catch (Exception)
			{
				if (clsFunction.IsAdmin)
				{
					throw;
				}
			}
			finally
			{
				_PanelBoard.ResumeLayout();
			}
		}
	}

	public void _CurrentForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (_CurrentForm != null)
		{
			clsTaskStatus varclsTaskStatus = (clsTaskStatus)_CurrentForm.Tag;
			if (varclsTaskStatus == null)
			{
				varclsTaskStatus = new clsTaskStatus();
			}
			EventPanelManagerEventArgs varArguments = new EventPanelManagerEventArgs();
			varArguments.UserAction = varclsTaskStatus.FormAction;
			varArguments.ObjectItem = varclsTaskStatus.ObjectItem;
			OnEventPanelManager(varArguments);
			_PanelBoard.Visible = false;
			_PanelBoard.Controls.Clear();
			if (_CurrentForm != null)
			{
				_CurrentForm.Dispose();
				_CurrentForm = null;
			}
		}
	}
}
