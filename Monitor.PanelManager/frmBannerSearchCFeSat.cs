using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io.SalesManager;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor.PanelManager;

public class frmBannerSearchCFeSat : Form
{
	private Timer _timer;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatureService = new clsFeatureService();

	private string _FeatExtId = clsFeatureService.consFeatScanDocCFeOut;

	private IContainer components;

	private Button btnRememberOrClose;

	private CheckBox chkRememberAgain;

	private Button button1;

	public frmBannerSearchCFeSat()
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

	private async void frmPanelBackup_Load(object sender, EventArgs e)
	{
		if (!clsFunction.IsEmpty(await _clsDataParam.funcGetAsync("NOT-SHOW-AGAIN-" + _FeatExtId)))
		{
			chkRememberAgain.Checked = false;
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
		string varDateTimeStr = (clsFunction.IsEqual(btnRememberOrClose.Text, "Fechar") ? DateTime.Now.AddDays(90.0).ToString("yyyy-MM-ddTHH:mm:sszzz") : DateTime.Now.AddDays(1.0).ToString("yyyy-MM-ddTHH:mm:sszzz"));
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-" + _FeatExtId + "-NEXT-SHOW", varDateTimeStr);
		(((clsTaskStatus)base.Tag) ?? new clsTaskStatus()).FormAction = "FORM_CLOSE";
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

	private void frmBannerBackup_Leave(object sender, EventArgs e)
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
		this.button1 = new System.Windows.Forms.Button();
		this.btnRememberOrClose = new System.Windows.Forms.Button();
		this.chkRememberAgain = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.button1.BackColor = System.Drawing.Color.FromArgb(36, 199, 118);
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(471, 404);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(213, 42);
		this.button1.TabIndex = 0;
		this.button1.Text = "Fale com um consultor";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(btSalesContact_Click);
		this.btnRememberOrClose.BackColor = System.Drawing.Color.Silver;
		this.btnRememberOrClose.FlatAppearance.BorderSize = 0;
		this.btnRememberOrClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnRememberOrClose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRememberOrClose.Location = new System.Drawing.Point(68, 404);
		this.btnRememberOrClose.Name = "btnRememberOrClose";
		this.btnRememberOrClose.Size = new System.Drawing.Size(213, 42);
		this.btnRememberOrClose.TabIndex = 1;
		this.btnRememberOrClose.Text = "Lembrar mais tarde";
		this.btnRememberOrClose.UseVisualStyleBackColor = false;
		this.btnRememberOrClose.Click += new System.EventHandler(btClose_Click);
		this.chkRememberAgain.AutoSize = true;
		this.chkRememberAgain.BackColor = System.Drawing.Color.Transparent;
		this.chkRememberAgain.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.chkRememberAgain.ForeColor = System.Drawing.Color.Transparent;
		this.chkRememberAgain.Location = new System.Drawing.Point(56, 452);
		this.chkRememberAgain.Name = "chkRememberAgain";
		this.chkRememberAgain.Size = new System.Drawing.Size(237, 20);
		this.chkRememberAgain.TabIndex = 2;
		this.chkRememberAgain.Text = "Não desejo ser lembrado novamente";
		this.chkRememberAgain.UseVisualStyleBackColor = false;
		this.chkRememberAgain.Click += new System.EventHandler(chkRememberAgain_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.DimGray;
		this.BackgroundImage = Monitor.Resources.cfesat;
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		base.ClientSize = new System.Drawing.Size(775, 484);
		base.ControlBox = false;
		base.Controls.Add(this.chkRememberAgain);
		base.Controls.Add(this.btnRememberOrClose);
		base.Controls.Add(this.button1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmBannerSearchCFeSat";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.Load += new System.EventHandler(frmPanelBackup_Load);
		base.Leave += new System.EventHandler(frmBannerBackup_Leave);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
