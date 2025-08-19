using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmPanelStorageFull : Form
{
	private Timer _timer;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private clsFeatureService _clsFeatureService = new clsFeatureService();

	private clsReturn _clsReturn = new clsReturn();

	private IContainer components;

	private Button btSalesContact;

	private PictureBox pictureBox1;

	private Label textBox1;

	private Label textBox2;

	private Label textBox3;

	public frmPanelStorageFull()
	{
		InitializeComponent();
		_timer = new Timer();
		_timer.Tick += async delegate
		{
			string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			await _clsDataParam.funcSetAsync("PANEL-SHOW-DATE-FULL-STORAGE-LAST-SHOW", varDateTime);
			Close();
			_timer.Stop();
			_timer.Enabled = false;
		};
		_timer.Interval = (int)TimeSpan.FromMinutes(5.0).TotalMilliseconds;
		_timer.Start();
	}

	private async void frmPanelStorageFull_Load(object sender, EventArgs e)
	{
		string varDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
		await _clsDataParam.funcSetAsync("PANEL-SHOW-DATE-FULL-STORAGE-LAST-SHOW", varDateTime);
		await _clsDataParam.funcSetAsync("BANNER-LAST-SHOW-DATE", varDateTime);
	}

	private async void btSalesContact_Click(object sender, EventArgs e)
	{
		double varTotalSizeInGB = clsFunction.funcConvKBtoGB(clsFunction.funcConvStrToLong((await new clsDataConfig().funcGetItemByKeyAsync()).DatabaseSize));
		string varUserMessage = $"Banco de dados Local excede o tamanho de 9GB. O tamanho do banco atualmente é de {varTotalSizeInGB}GB";
		string varUserDetail = "Banco de dados Local excede o tamanho de 9GB";
		await _clsDataParam.funcAddCounterAsync("FULL-STORAGE-CLICKS");
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

	private void lkbArticleSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallFiscalServer();
	}

	private void frmPanelStorageFull_Leave(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmPanelStorageFull));
		this.btSalesContact = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.textBox1 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.Label();
		this.textBox3 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.btSalesContact.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btSalesContact.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btSalesContact.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(40, 177, 189);
		this.btSalesContact.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btSalesContact.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btSalesContact.ForeColor = System.Drawing.Color.White;
		this.btSalesContact.Location = new System.Drawing.Point(606, 6);
		this.btSalesContact.Name = "btSalesContact";
		this.btSalesContact.Size = new System.Drawing.Size(156, 38);
		this.btSalesContact.TabIndex = 241;
		this.btSalesContact.Text = "Fale com o suporte";
		this.btSalesContact.UseVisualStyleBackColor = false;
		this.btSalesContact.Click += new System.EventHandler(btSalesContact_Click);
		this.pictureBox1.BackgroundImage = Monitor.Resources.fiscalio_monitor_limite_banco_local;
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.Location = new System.Drawing.Point(28, 8);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(44, 34);
		this.pictureBox1.TabIndex = 242;
		this.pictureBox1.TabStop = false;
		this.textBox1.BackColor = System.Drawing.Color.FromArgb(230, 76, 60);
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(78, 8);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(496, 20);
		this.textBox1.TabIndex = 243;
		this.textBox1.Text = "Buscas na SEFAZ paralisadas! Limite de dados atingido.";
		this.textBox2.BackColor = System.Drawing.Color.FromArgb(230, 76, 60);
		this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox2.Location = new System.Drawing.Point(78, 31);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(496, 15);
		this.textBox2.TabIndex = 244;
		this.textBox2.Text = "Sua instalação do Fiscal.io Monitor atingiu o limite máximo de";
		this.textBox3.BackColor = System.Drawing.Color.FromArgb(230, 76, 60);
		this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox3.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox3.Location = new System.Drawing.Point(466, 31);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(119, 15);
		this.textBox3.TabIndex = 245;
		this.textBox3.Text = "9GB de dados.";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(230, 76, 60);
		base.ClientSize = new System.Drawing.Size(776, 52);
		base.Controls.Add(this.textBox3);
		base.Controls.Add(this.textBox2);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.btSalesContact);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPanelStorageFull";
		this.Text = "frmPanelStorageAlmostFull";
		base.Load += new System.EventHandler(frmPanelStorageFull_Load);
		base.Leave += new System.EventHandler(frmPanelStorageFull_Leave);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
