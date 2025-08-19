using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using screen.fiscal.io.SalesManager;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmPanelStorageAlmostFull : Form
{
	private Timer _timer;

	private string _FeatType = string.Empty;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatureService = new clsFeatureService();

	private clsDataProduct _clsDataProduct = new clsDataProduct(null);

	private clsReturn _clsReturn = new clsReturn();

	private IContainer components;

	private Button btSalesContact;

	private PictureBox pictureBox1;

	private Label textBox1;

	private Label textBox4;

	public frmPanelStorageAlmostFull()
	{
		InitializeComponent();
		_timer = new Timer();
		_timer.Tick += async delegate
		{
			string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			await _clsDataParam.funcSetAsync("PANEL-SHOW-DATE-STORAGE-ALMOST-FULL-LAST-SHOW", varDateTime);
			Close();
			_timer.Stop();
			_timer.Enabled = false;
		};
		_timer.Interval = (int)TimeSpan.FromMinutes(5.0).TotalMilliseconds;
		_timer.Start();
	}

	private async void frmPanelStorageAlmostFull_Load(object sender, EventArgs e)
	{
		int varCount = 0;
		foreach (Product item in await _clsDataProduct.funcGetFullListAsync())
		{
			if (clsFunction.IsEqual(item.ProdType, "PAYED"))
			{
				varCount++;
			}
		}
		if (varCount > 0)
		{
			btSalesContact.Text = "Fale com o Suporte";
		}
		string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("PANEL-SHOW-DATE-STORAGE-ALMOST-FULL-LAST-SHOW", varDateTime);
		await _clsDataParam.funcSetAsync("BANNER-LAST-SHOW-DATE", varDateTime);
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		double varTotalSizeInGB = clsFunction.funcConvKBtoGB(clsFunction.funcConvStrToLong((await new clsDataConfig().funcGetItemByKeyAsync()).DatabaseSize));
		string varUserMessage = $"Banco de dados Local excede o tamanho de 8GB. O tamanho do banco atualmente é de {varTotalSizeInGB}GB";
		string varUserDetail = "Banco de dados Local excede o tamanho de 8GB";
		await _clsDataParam.funcAddCounterAsync("STORAGE-ALMOST-FULL-CLICKS");
		if (clsFunction.IsEqual(btSalesContact.Text, "Fale com o Suporte", pIgnoreCase: true))
		{
			base.TopMost = false;
			if (clsFunction.IsEmpty(_clsReturn.ApiRefGuid))
			{
				_clsReturn.ApiRefGuid = Guid.NewGuid().ToString();
			}
			if (new frmSupportConfirm(varUserMessage, varUserDetail, _clsReturn.ApiRefGuid).ShowDialog(this).Equals(DialogResult.OK))
			{
				Close();
			}
		}
		else
		{
			frmSalesPayed02 frmSalesPayed = new frmSalesPayed02("STORAGE-ALMOST-FULL");
			frmSalesPayed.ShowDialog(this);
			frmSalesPayed.Dispose();
		}
	}

	private void lkbArticleSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallFiscalServer();
	}

	private void frmPanelStorageAlmostFull_Leave(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmPanelStorageAlmostFull));
		this.btSalesContact = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.textBox1 = new System.Windows.Forms.Label();
		this.textBox4 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.btSalesContact.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btSalesContact.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btSalesContact.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btSalesContact.ForeColor = System.Drawing.Color.White;
		this.btSalesContact.Location = new System.Drawing.Point(596, 6);
		this.btSalesContact.Name = "btSalesContact";
		this.btSalesContact.Size = new System.Drawing.Size(155, 38);
		this.btSalesContact.TabIndex = 241;
		this.btSalesContact.Text = "Fale com um consultor";
		this.btSalesContact.UseVisualStyleBackColor = false;
		this.btSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		this.pictureBox1.BackgroundImage = Monitor.Resources.fiscalio_monitor_limite_banco_local;
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.Location = new System.Drawing.Point(27, 7);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(44, 34);
		this.pictureBox1.TabIndex = 242;
		this.pictureBox1.TabStop = false;
		this.textBox1.BackColor = System.Drawing.Color.FromArgb(243, 213, 91);
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(83, 7);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(398, 20);
		this.textBox1.TabIndex = 243;
		this.textBox1.Text = "Armazenamento chegando ao limite de 9GB!";
		this.textBox4.BackColor = System.Drawing.Color.FromArgb(243, 213, 91);
		this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox4.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox4.Location = new System.Drawing.Point(83, 33);
		this.textBox4.Name = "textBox4";
		this.textBox4.Size = new System.Drawing.Size(398, 15);
		this.textBox4.TabIndex = 246;
		this.textBox4.Text = "Buscas na SEFAZ serão paralizadas caso o limite seja atingido.";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(243, 213, 91);
		base.ClientSize = new System.Drawing.Size(776, 52);
		base.Controls.Add(this.textBox4);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.btSalesContact);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPanelStorageAlmostFull";
		this.Text = "frmPanelStorageAlmostFull";
		base.Load += new System.EventHandler(frmPanelStorageAlmostFull_Load);
		base.Leave += new System.EventHandler(frmPanelStorageAlmostFull_Leave);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
