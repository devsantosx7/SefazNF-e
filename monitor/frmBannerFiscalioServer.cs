using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io.SalesManager;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmBannerFiscalioServer : Form
{
	private Timer _timer;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatureService = new clsFeatureService();

	private string _FeatExtId = clsFeatureService.consFeatFiscalServer;

	private IContainer components;

	private Panel pnForm;

	private Button btnSalesContact;

	private Button btnRememberOrClose;

	private CheckBox chkRememberAgain;

	public frmBannerFiscalioServer()
	{
		InitializeComponent();
		_timer = new Timer();
		_timer.Tick += async delegate
		{
			string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-" + _FeatExtId + "-LAST-SHOW", varDateTime);
			Close();
			_timer.Stop();
			_timer.Enabled = false;
		};
		_timer.Interval = (int)TimeSpan.FromMinutes(5.0).TotalMilliseconds;
		_timer.Start();
	}

	private async void frmPanelFiscalServer_Load(object sender, EventArgs e)
	{
		if (!clsFunction.IsEmpty(await _clsDataParam.funcGetAsync("NOT-SHOW-AGAIN-" + _FeatExtId)))
		{
			chkRememberAgain.Checked = true;
			chkRememberAgain.Enabled = true;
		}
		string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-" + _FeatExtId + "-LAST-SHOW", varDateTime);
		await _clsDataParam.funcSetAsync("BANNER-LAST-SHOW-DATE", varDateTime);
	}

	private async void btClose_Click(object sender, EventArgs e)
	{
		if (chkRememberAgain.Checked)
		{
			await _clsDataParam.funcSetAsync("NOT-SHOW-AGAIN-" + _FeatExtId, "X");
		}
		string varDateTimeStr = (clsFunction.IsEqual(btnRememberOrClose.Text, "Fechar", pIgnoreCase: true) ? DateTime.Now.AddDays(90.0).ToString("yyyy-MM-ddTHH:mm:sszzz") : DateTime.Now.AddDays(1.0).ToString("yyyy-MM-ddTHH:mm:sszzz"));
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-" + _FeatExtId + "-NEXT-SHOW", varDateTimeStr);
		(((clsTaskStatus)base.Tag) ?? new clsTaskStatus()).FormAction = "FORM_CLOSE";
		Close();
	}

	private void picFiscalServer_Click(object sender, EventArgs e)
	{
		funcOpenFiscalServer();
	}

	public void funcOpenFiscalServer()
	{
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			varclsTaskStatus = new clsTaskStatus();
		}
		varclsTaskStatus.FormAction = "CONFIG_SERVER";
		Close();
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		await _clsDataParam.funcAddCounterAsync(_FeatExtId + "-CLICKS");
		frmSalesPayed02 frmSalesPayed = new frmSalesPayed02(_FeatExtId);
		frmSalesPayed.ShowDialog(this);
		frmSalesPayed.Dispose();
	}

	private void chkRememberAgain_CheckedChanged(object sender, EventArgs e)
	{
		if (chkRememberAgain.Checked)
		{
			btnRememberOrClose.Text = "Fechar";
		}
		else
		{
			btnRememberOrClose.Text = "Lembrar mais tarde";
		}
	}

	private void frmBannerFiscalioServer_Leave(object sender, EventArgs e)
	{
		if (_timer != null)
		{
			_timer.Stop();
			_timer.Enabled = false;
			_timer.Dispose();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmBannerFiscalioServer));
		this.pnForm = new System.Windows.Forms.Panel();
		this.chkRememberAgain = new System.Windows.Forms.CheckBox();
		this.btnSalesContact = new System.Windows.Forms.Button();
		this.btnRememberOrClose = new System.Windows.Forms.Button();
		this.pnForm.SuspendLayout();
		base.SuspendLayout();
		this.pnForm.BackColor = System.Drawing.Color.Transparent;
		this.pnForm.BackgroundImage = Monitor.Resources.bd_superior;
		this.pnForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.pnForm.Controls.Add(this.chkRememberAgain);
		this.pnForm.Controls.Add(this.btnSalesContact);
		this.pnForm.Controls.Add(this.btnRememberOrClose);
		this.pnForm.Location = new System.Drawing.Point(4, 4);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(768, 476);
		this.pnForm.TabIndex = 292;
		this.chkRememberAgain.AutoSize = true;
		this.chkRememberAgain.BackColor = System.Drawing.Color.Transparent;
		this.chkRememberAgain.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Italic);
		this.chkRememberAgain.Location = new System.Drawing.Point(56, 452);
		this.chkRememberAgain.Name = "chkRememberAgain";
		this.chkRememberAgain.Size = new System.Drawing.Size(237, 20);
		this.chkRememberAgain.TabIndex = 242;
		this.chkRememberAgain.Text = "Não desejo ser lembrado novamente";
		this.chkRememberAgain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.chkRememberAgain.UseVisualStyleBackColor = false;
		this.chkRememberAgain.CheckedChanged += new System.EventHandler(chkRememberAgain_CheckedChanged);
		this.btnSalesContact.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnSalesContact.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.btnSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btnSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSalesContact.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnSalesContact.ForeColor = System.Drawing.Color.White;
		this.btnSalesContact.Location = new System.Drawing.Point(471, 404);
		this.btnSalesContact.Name = "btnSalesContact";
		this.btnSalesContact.Size = new System.Drawing.Size(213, 42);
		this.btnSalesContact.TabIndex = 241;
		this.btnSalesContact.Text = "Fale com um consultor";
		this.btnSalesContact.UseVisualStyleBackColor = false;
		this.btnSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		this.btnRememberOrClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnRememberOrClose.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.btnRememberOrClose.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
		this.btnRememberOrClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnRememberOrClose.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnRememberOrClose.ForeColor = System.Drawing.Color.Black;
		this.btnRememberOrClose.Location = new System.Drawing.Point(68, 404);
		this.btnRememberOrClose.Name = "btnRememberOrClose";
		this.btnRememberOrClose.Size = new System.Drawing.Size(213, 42);
		this.btnRememberOrClose.TabIndex = 241;
		this.btnRememberOrClose.Text = "Lembrar mais tarde";
		this.btnRememberOrClose.UseVisualStyleBackColor = false;
		this.btnRememberOrClose.Click += new System.EventHandler(btClose_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.DimGray;
		base.ClientSize = new System.Drawing.Size(775, 484);
		base.ControlBox = false;
		base.Controls.Add(this.pnForm);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmBannerFiscalioServer";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.Load += new System.EventHandler(frmPanelFiscalServer_Load);
		base.Leave += new System.EventHandler(frmBannerFiscalioServer_Leave);
		this.pnForm.ResumeLayout(false);
		this.pnForm.PerformLayout();
		base.ResumeLayout(false);
	}
}
