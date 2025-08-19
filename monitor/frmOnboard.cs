using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmOnboard : Form
{
	public enum enDataType
	{
		Initial,
		InformChoise,
		InformCompany
	}

	private class clsObjType
	{
		public Form FormScreen { get; set; }

		public bool StatusDone { get; set; }

		public bool UserChoise { get; set; }
	}

	private List<clsObjType> _ObjectList = new List<clsObjType>();

	private int _Position;

	private Form _ActualForm;

	private frmOnboardStep00 _clsFormInitial = new frmOnboardStep00();

	private frmOnboardStep02 _clsFormUserChoise = new frmOnboardStep02();

	private frmOnboardStep01 _clsFormCompany = new frmOnboardStep01();

	private bool _isFormCloseDisabled;

	private IContainer components;

	private Panel pnlContent;

	private Button btnNext;

	private Button btnBack;

	private Label lbStatusSave;

	public frmOnboard(enDataType pDataType)
	{
		InitializeComponent();
		InitializeStartupSetup(pDataType);
	}

	private void _clsFormUserChoise_VisibleChanged(object sender, EventArgs e)
	{
		if (_clsFormUserChoise.Visible)
		{
			_ObjectList.RemoveAll((clsObjType r) => r.UserChoise);
			return;
		}
		string varUserChoise = _clsFormUserChoise.funcGetChoise();
		if (!clsFunction.IsEmpty(varUserChoise))
		{
			if (clsFunction.IsEqual(varUserChoise, "Buscar NFe/CTe emitidos contra CPF", pIgnoreCase: true))
			{
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch01Sc01(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch01Sc02(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep01Ch01Sc03(),
					StatusDone = false,
					UserChoise = true
				});
			}
			else if (clsFunction.IsEqual(varUserChoise, "Buscar NFe/CTe emitidos contra CNPJ", pIgnoreCase: true))
			{
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch02Sc01(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch02Sc02(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch02Sc03(),
					StatusDone = false,
					UserChoise = true
				});
			}
			else if (clsFunction.IsEqual(varUserChoise, "Buscar Documentos de Saída", pIgnoreCase: true))
			{
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch03Sc01(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch03Sc02(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch03Sc03(),
					StatusDone = false,
					UserChoise = true
				});
			}
			else if (clsFunction.IsEqual(varUserChoise, "Desacordo de Serviço (CTe)", pIgnoreCase: true))
			{
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch04Sc01(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch04Sc02(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch04Sc03(),
					StatusDone = false,
					UserChoise = true
				});
			}
			else if (clsFunction.IsEqual(varUserChoise, "Recuperação de documentos do passado", pIgnoreCase: true))
			{
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch05Sc01(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch05Sc02(),
					StatusDone = false,
					UserChoise = true
				});
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch05Sc03(),
					StatusDone = false,
					UserChoise = true
				});
			}
			else if (clsFunction.IsEqual(varUserChoise, "Outros", pIgnoreCase: true))
			{
				_ObjectList.Add(new clsObjType
				{
					FormScreen = new frmOnboardStep02Ch06Sc01(),
					StatusDone = false,
					UserChoise = true
				});
			}
			_ObjectList.Add(new clsObjType
			{
				FormScreen = _clsFormCompany,
				StatusDone = true,
				UserChoise = true
			});
			btnNext_Click(sender, e);
		}
	}

	public frmOnboard(enDataType pDataType, bool mustDisableClose)
	{
		InitializeComponent();
		InitializeStartupSetup(pDataType);
		_isFormCloseDisabled = mustDisableClose;
	}

	private async void InitializeStartupSetup(enDataType pDataType)
	{
		_ObjectList.Add(new clsObjType
		{
			FormScreen = _clsFormInitial,
			StatusDone = false
		});
		_ObjectList.Add(new clsObjType
		{
			FormScreen = _clsFormUserChoise,
			StatusDone = false
		});
		if (pDataType == enDataType.InformCompany)
		{
			_ObjectList.Add(new clsObjType
			{
				FormScreen = _clsFormCompany,
				StatusDone = true,
				UserChoise = true
			});
		}
		int varInitialPosition = Convert.ToInt32(pDataType);
		_Position = await funcLoadFormAsync(varInitialPosition);
	}

	private async Task<int> funcLoadFormAsync(int pPosition)
	{
		if (pPosition + 1 > _ObjectList.Count)
		{
			return pPosition;
		}
		if (pPosition < 0)
		{
			pPosition = 0;
		}
		int varPosition = pPosition;
		Form varForm = _ObjectList[pPosition].FormScreen;
		varForm.TopLevel = false;
		varForm.AutoScroll = true;
		varForm.Dock = DockStyle.Fill;
		pnlContent.Controls.Clear();
		pnlContent.Controls.Add(varForm);
		if (varForm.Name.Equals(_clsFormInitial.Name))
		{
			_clsFormInitial.funcLoadDataAsync();
		}
		else if (varForm.Name.Equals(_clsFormUserChoise.Name))
		{
			_clsFormInitial.funcLoadDataAsync();
		}
		varForm.Show();
		_ActualForm = varForm;
		_clsFormUserChoise.VisibleChanged -= _clsFormUserChoise_VisibleChanged;
		if (varForm.Name.Equals(_clsFormUserChoise.Name))
		{
			_ObjectList.RemoveAll((clsObjType r) => r.UserChoise);
			_clsFormUserChoise.VisibleChanged += _clsFormUserChoise_VisibleChanged;
			btnNext.Visible = false;
		}
		else if (varPosition + 1 == _ObjectList.Count)
		{
			btnNext.Text = "Vamos ao sistema!";
			btnNext.Visible = true;
		}
		else
		{
			btnNext.Text = "Próximo";
			btnNext.Visible = true;
		}
		if (varPosition == 0)
		{
			btnBack.Visible = false;
		}
		else
		{
			btnBack.Visible = true;
		}
		return varPosition;
	}

	private async void btnNext_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (_ActualForm.Name.Equals(_clsFormInitial.Name))
			{
				if (!_clsFormInitial.funcValidate())
				{
					return;
				}
				await funcSaveConfigDataAsync();
			}
			else if (_ActualForm.Name.Equals(_clsFormCompany.Name) && !(await _clsFormCompany.funcValidateAsync()))
			{
				return;
			}
			if (_Position + 1 >= _ObjectList.Count)
			{
				_isFormCloseDisabled = false;
				Close();
			}
			else
			{
				_ObjectList[_Position].StatusDone = true;
				_Position = await funcLoadFormAsync(_Position + 1);
			}
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
			Close();
		}
	}

	private async void btnBack_Click(object sender, EventArgs e)
	{
		_Position = await funcLoadFormAsync(_Position - 1);
	}

	private async Task<bool> funcSaveConfigDataAsync()
	{
		clsDataConfig varclsDataConfig = new clsDataConfig();
		bool varSaveData = false;
		string varUserName = _clsFormInitial.funcGetUserName();
		string varUserEmail = _clsFormInitial.funcGetUserEmail();
		string varUserPhone = _clsFormInitial.funcGetUserPhone();
		string varUserPosition = _clsFormInitial.funcGetUserPosition();
		string varUserCompany = _clsFormInitial.funcGetUserCompany();
		string varUserBusiness = _clsFormInitial.funcGetUserBusiness();
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		if (!clsFunction.IsEmpty(varUserName) && !varUserName.Equals(varclsConfig.UserName))
		{
			varclsConfig.UserName = varUserName;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserEmail) && !varUserEmail.Equals(varclsConfig.UserEmail))
		{
			varclsConfig.UserEmail = varUserEmail;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserPhone) && !varUserEmail.Equals(varclsConfig.UserPhone))
		{
			varclsConfig.UserPhone = varUserPhone;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserPosition) && !varUserPosition.Equals(varclsConfig.UserPosition))
		{
			varclsConfig.UserPosition = varUserPosition;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserCompany) && !varUserCompany.Equals(varclsConfig.UserCompany))
		{
			varclsConfig.UserCompany = varUserCompany;
			varSaveData = true;
		}
		if (!clsFunction.IsEmpty(varUserBusiness) && !varUserBusiness.Equals(varclsConfig.UserBusiness))
		{
			varclsConfig.UserBusiness = varUserBusiness;
			varSaveData = true;
		}
		if (!varSaveData)
		{
			return true;
		}
		varclsConfig.DateModified = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
		await varclsDataConfig.funcUpdateAsync(varclsConfig);
		clsDataParameter clsDataParameter = new clsDataParameter();
		string varLastUpdate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
		await clsDataParameter.funcSetAsync("LAST-USER-DATA-UPDATE", varLastUpdate);
		lbStatusSave.Text = "Aguarde, salvando os dados...";
		lbStatusSave.Visible = true;
		clsMetricService varclsMetricService = new clsMetricService();
		clsReturn varclsReturnFunc = new clsReturn();
		Task<clsReturn> varTaskSetContact = varclsMetricService.funcSyncAsync(pSyncMarket: false);
		if (!(await varTaskSetContact.WaitAsync(TimeSpan.FromSeconds(3.0))))
		{
			if (clsFunction.IsAdmin)
			{
				varclsReturnFunc.AddMessage(new clsMessage("W", "9999", "Tempo de sincronização superior a 3 segundos."));
			}
		}
		else
		{
			clsReturn varRetSetFilial = varTaskSetContact.Result;
			if (varRetSetFilial.HasError)
			{
				varclsReturnFunc.AddRange(varRetSetFilial);
			}
		}
		lbStatusSave.Text = string.Empty;
		lbStatusSave.Visible = false;
		if (varclsReturnFunc.HasError && clsFunction.IsAdmin)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc);
		}
		return true;
	}

	private void frmOnboard_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btnNext_Click(this, e);
		}
		else if (e.KeyCode == Keys.Escape && btnBack.Visible)
		{
			btnBack_Click(this, e);
		}
	}

	private void lkbSupportChat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallChatWebPageAsync("");
	}

	private void frmOnboard_FormClosing(object sender, FormClosingEventArgs e)
	{
		e.Cancel = _isFormCloseDisabled;
	}

	private void frmOnboard_SizeChanged(object sender, EventArgs e)
	{
		if (!base.WindowState.Equals(FormWindowState.Maximized))
		{
			base.WindowState = FormWindowState.Maximized;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboard));
		this.btnNext = new System.Windows.Forms.Button();
		this.btnBack = new System.Windows.Forms.Button();
		this.pnlContent = new System.Windows.Forms.Panel();
		this.lbStatusSave = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnNext.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnNext.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNext.ForeColor = System.Drawing.Color.White;
		this.btnNext.Location = new System.Drawing.Point(658, 449);
		this.btnNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(164, 31);
		this.btnNext.TabIndex = 0;
		this.btnNext.Text = "Próximo >";
		this.btnNext.UseVisualStyleBackColor = false;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.btnBack.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnBack.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnBack.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBack.ForeColor = System.Drawing.Color.White;
		this.btnBack.Location = new System.Drawing.Point(486, 449);
		this.btnBack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnBack.Name = "btnBack";
		this.btnBack.Size = new System.Drawing.Size(164, 31);
		this.btnBack.TabIndex = 1;
		this.btnBack.Text = "< Anterior";
		this.btnBack.UseVisualStyleBackColor = false;
		this.btnBack.Click += new System.EventHandler(btnBack_Click);
		this.pnlContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnlContent.BackColor = System.Drawing.Color.White;
		this.pnlContent.Location = new System.Drawing.Point(1, 2);
		this.pnlContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.pnlContent.Name = "pnlContent";
		this.pnlContent.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
		this.pnlContent.Size = new System.Drawing.Size(826, 438);
		this.pnlContent.TabIndex = 88;
		this.lbStatusSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lbStatusSave.Location = new System.Drawing.Point(213, 458);
		this.lbStatusSave.Name = "lbStatusSave";
		this.lbStatusSave.Size = new System.Drawing.Size(266, 13);
		this.lbStatusSave.TabIndex = 242;
		this.lbStatusSave.Tag = "";
		this.lbStatusSave.Text = ".";
		this.lbStatusSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lbStatusSave.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(829, 486);
		base.Controls.Add(this.lbStatusSave);
		base.Controls.Add(this.btnNext);
		base.Controls.Add(this.btnBack);
		base.Controls.Add(this.pnlContent);
		this.Font = new System.Drawing.Font("Verdana", 8.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		base.Name = "frmOnboard";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Instruções iniciais de uso.";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmOnboard_FormClosing);
		base.SizeChanged += new System.EventHandler(frmOnboard_SizeChanged);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmOnboard_KeyDown);
		base.ResumeLayout(false);
	}
}
