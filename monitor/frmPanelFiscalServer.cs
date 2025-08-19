using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using Monitor.PanelManager;
using screen.fiscal.io.SalesManager;
using srv.fiscal.io;
using TheArtOfDev.HtmlRenderer.WinForms;
using util.fiscal.io;

namespace Monitor;

public class frmPanelFiscalServer : Form
{
	private Timer _timer;

	private string _FeatType = string.Empty;

	private string _FeatExtId = clsFeatureService.consFeatFiscalServer;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatureService = new clsFeatureService();

	private IContainer components;

	private HtmlPanel htpnMessage01;

	private Label label1;

	private Button btClose;

	private PictureBox picFiscalServer;

	private PictureBox picChannelInHelp;

	private LinkLabel lkbArticleSearch;

	private Button btSalesContact;

	public frmPanelFiscalServer()
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
		htpnMessage01.Text = ResourcePanel.strPanelFiscalServer;
		_FeatType = await _clsFeatureService.funcGetFeatTypeAsync(_FeatExtId);
		string varDateTime = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-" + _FeatExtId + "-LAST-SHOW", varDateTime);
		await _clsDataParam.funcSetAsync("BANNER-LAST-SHOW-DATE", varDateTime);
	}

	private async void btClose_Click(object sender, EventArgs e)
	{
		string varDateTimeStr = (clsFunction.Contains(_FeatType, "LOCK") ? DateTime.Now.AddDays(15.0) : DateTime.Now.AddDays(90.0)).ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("BANNER-SHOW-DATE-" + _FeatExtId + "-NEXT-SHOW", varDateTimeStr);
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			varclsTaskStatus = new clsTaskStatus();
		}
		varclsTaskStatus.FormAction = "FORM_CLOSE";
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

	private void lkbArticleSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallFiscalServer();
	}

	private void frmPanelFiscalServer_Leave(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmPanelFiscalServer));
		this.htpnMessage01 = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
		this.label1 = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.picFiscalServer = new System.Windows.Forms.PictureBox();
		this.picChannelInHelp = new System.Windows.Forms.PictureBox();
		this.lkbArticleSearch = new System.Windows.Forms.LinkLabel();
		this.btSalesContact = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.picFiscalServer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picChannelInHelp).BeginInit();
		base.SuspendLayout();
		this.htpnMessage01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.htpnMessage01.AutoScroll = true;
		this.htpnMessage01.BackColor = System.Drawing.SystemColors.Window;
		this.htpnMessage01.BaseStylesheet = null;
		this.htpnMessage01.Location = new System.Drawing.Point(116, 2);
		this.htpnMessage01.Name = "htpnMessage01";
		this.htpnMessage01.Size = new System.Drawing.Size(635, 82);
		this.htpnMessage01.TabIndex = 3;
		this.htpnMessage01.Text = null;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(116, 92);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(635, 2);
		this.label1.TabIndex = 188;
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Tahoma", 6f);
		this.btClose.Location = new System.Drawing.Point(757, 2);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(17, 19);
		this.btClose.TabIndex = 189;
		this.btClose.Text = "X";
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.picFiscalServer.Image = Monitor.Resources.image_fiscal_server;
		this.picFiscalServer.Location = new System.Drawing.Point(5, 4);
		this.picFiscalServer.Name = "picFiscalServer";
		this.picFiscalServer.Size = new System.Drawing.Size(98, 132);
		this.picFiscalServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFiscalServer.TabIndex = 190;
		this.picFiscalServer.TabStop = false;
		this.picFiscalServer.Click += new System.EventHandler(picFiscalServer_Click);
		this.picChannelInHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.picChannelInHelp.Image = Monitor.Resources.image_help;
		this.picChannelInHelp.Location = new System.Drawing.Point(247, 106);
		this.picChannelInHelp.Name = "picChannelInHelp";
		this.picChannelInHelp.Size = new System.Drawing.Size(20, 20);
		this.picChannelInHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picChannelInHelp.TabIndex = 235;
		this.picChannelInHelp.TabStop = false;
		this.lkbArticleSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lkbArticleSearch.AutoSize = true;
		this.lkbArticleSearch.Location = new System.Drawing.Point(269, 109);
		this.lkbArticleSearch.Name = "lkbArticleSearch";
		this.lkbArticleSearch.Size = new System.Drawing.Size(172, 14);
		this.lkbArticleSearch.TabIndex = 234;
		this.lkbArticleSearch.TabStop = true;
		this.lkbArticleSearch.Text = "Saiba mais. Veja passo a passo";
		this.lkbArticleSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbArticleSearch_LinkClicked);
		this.btSalesContact.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btSalesContact.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btSalesContact.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.btSalesContact.ForeColor = System.Drawing.Color.White;
		this.btSalesContact.Location = new System.Drawing.Point(499, 101);
		this.btSalesContact.Name = "btSalesContact";
		this.btSalesContact.Size = new System.Drawing.Size(234, 34);
		this.btSalesContact.TabIndex = 241;
		this.btSalesContact.Text = "Solicitar Contato";
		this.btSalesContact.UseVisualStyleBackColor = false;
		this.btSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(776, 140);
		base.Controls.Add(this.btSalesContact);
		base.Controls.Add(this.picChannelInHelp);
		base.Controls.Add(this.lkbArticleSearch);
		base.Controls.Add(this.picFiscalServer);
		base.Controls.Add(this.btClose);
		base.Controls.Add(this.htpnMessage01);
		base.Controls.Add(this.label1);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPanelFiscalServer";
		this.Text = "frmPanelFiscalServer";
		base.Load += new System.EventHandler(frmPanelFiscalServer_Load);
		base.Leave += new System.EventHandler(frmPanelFiscalServer_Leave);
		((System.ComponentModel.ISupportInitialize)this.picFiscalServer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picChannelInHelp).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
