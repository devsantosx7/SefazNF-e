using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Monitor.PanelManager;
using srv.fiscal.io;
using TheArtOfDev.HtmlRenderer.WinForms;
using util.fiscal.io;

namespace Monitor;

public class frmPanelXmlValidator : Form
{
	private IContainer components;

	private HtmlPanel htpnMessage01;

	private Button btChannelManager;

	private Label label1;

	private Button btClose;

	private PictureBox picSourceEmail;

	private PictureBox picSourceFolder;

	private PictureBox picXmlError;

	private PictureBox picXmlCorrect;

	private PictureBox picMySql;

	private PictureBox pictureBox1;

	private PictureBox picFiscalServer;

	private LinkLabel lkbArticleSearch;

	public frmPanelXmlValidator()
	{
		InitializeComponent();
	}

	private void frmPanelXmlValidator_Load(object sender, EventArgs e)
	{
		htpnMessage01.Text = ResourcePanel.strPanelXmlValidation;
	}

	private void btChannelManager_Click(object sender, EventArgs e)
	{
		funcOpenChanelManager();
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			varclsTaskStatus = new clsTaskStatus();
		}
		varclsTaskStatus.FormAction = "FORM_CLOSE";
		Close();
	}

	private void picChannelManager_Click(object sender, EventArgs e)
	{
		funcOpenChanelManager();
	}

	public void funcOpenChanelManager()
	{
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			varclsTaskStatus = new clsTaskStatus();
		}
		varclsTaskStatus.FormAction = "CONFIG_CHANNEL_IN";
		Close();
	}

	private void lkbArticleSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallIntegHelp();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmPanelXmlValidator));
		this.htpnMessage01 = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
		this.btChannelManager = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.btClose = new System.Windows.Forms.Button();
		this.picSourceEmail = new System.Windows.Forms.PictureBox();
		this.picSourceFolder = new System.Windows.Forms.PictureBox();
		this.picXmlError = new System.Windows.Forms.PictureBox();
		this.picXmlCorrect = new System.Windows.Forms.PictureBox();
		this.picMySql = new System.Windows.Forms.PictureBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.picFiscalServer = new System.Windows.Forms.PictureBox();
		this.lkbArticleSearch = new System.Windows.Forms.LinkLabel();
		((System.ComponentModel.ISupportInitialize)this.picSourceEmail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picSourceFolder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picXmlError).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picXmlCorrect).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picMySql).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picFiscalServer).BeginInit();
		base.SuspendLayout();
		this.htpnMessage01.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.htpnMessage01.AutoScroll = true;
		this.htpnMessage01.BackColor = System.Drawing.SystemColors.Window;
		this.htpnMessage01.BaseStylesheet = null;
		this.htpnMessage01.Location = new System.Drawing.Point(5, 2);
		this.htpnMessage01.Name = "htpnMessage01";
		this.htpnMessage01.Size = new System.Drawing.Size(747, 76);
		this.htpnMessage01.TabIndex = 3;
		this.htpnMessage01.Text = null;
		this.btChannelManager.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btChannelManager.Image = Monitor.Resources.image_integration;
		this.btChannelManager.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btChannelManager.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.btChannelManager.Location = new System.Drawing.Point(472, 90);
		this.btChannelManager.Margin = new System.Windows.Forms.Padding(2);
		this.btChannelManager.Name = "btChannelManager";
		this.btChannelManager.Size = new System.Drawing.Size(280, 36);
		this.btChannelManager.TabIndex = 182;
		this.btChannelManager.Text = "Clique aqui para configurar uma integração";
		this.btChannelManager.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btChannelManager.UseVisualStyleBackColor = true;
		this.btChannelManager.Click += new System.EventHandler(btChannelManager_Click);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(5, 81);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(747, 2);
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
		this.picSourceEmail.Image = Monitor.Resources.image_source_email;
		this.picSourceEmail.Location = new System.Drawing.Point(5, 88);
		this.picSourceEmail.Name = "picSourceEmail";
		this.picSourceEmail.Size = new System.Drawing.Size(30, 30);
		this.picSourceEmail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picSourceEmail.TabIndex = 190;
		this.picSourceEmail.TabStop = false;
		this.picSourceEmail.Click += new System.EventHandler(picChannelManager_Click);
		this.picSourceFolder.Image = Monitor.Resources.image_source_folder;
		this.picSourceFolder.Location = new System.Drawing.Point(5, 122);
		this.picSourceFolder.Name = "picSourceFolder";
		this.picSourceFolder.Size = new System.Drawing.Size(30, 30);
		this.picSourceFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picSourceFolder.TabIndex = 191;
		this.picSourceFolder.TabStop = false;
		this.picSourceFolder.Click += new System.EventHandler(picChannelManager_Click);
		this.picXmlError.Image = Monitor.Resources.image_xml_red;
		this.picXmlError.Location = new System.Drawing.Point(191, 123);
		this.picXmlError.Name = "picXmlError";
		this.picXmlError.Size = new System.Drawing.Size(30, 30);
		this.picXmlError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picXmlError.TabIndex = 193;
		this.picXmlError.TabStop = false;
		this.picXmlError.Click += new System.EventHandler(picChannelManager_Click);
		this.picXmlCorrect.Image = Monitor.Resources.image_xml_blue;
		this.picXmlCorrect.Location = new System.Drawing.Point(191, 88);
		this.picXmlCorrect.Name = "picXmlCorrect";
		this.picXmlCorrect.Size = new System.Drawing.Size(30, 30);
		this.picXmlCorrect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picXmlCorrect.TabIndex = 192;
		this.picXmlCorrect.TabStop = false;
		this.picXmlCorrect.Click += new System.EventHandler(picChannelManager_Click);
		this.picMySql.Image = Monitor.Resources.image_process_done;
		this.picMySql.Location = new System.Drawing.Point(86, 97);
		this.picMySql.Name = "picMySql";
		this.picMySql.Size = new System.Drawing.Size(40, 41);
		this.picMySql.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picMySql.TabIndex = 194;
		this.picMySql.TabStop = false;
		this.picMySql.Click += new System.EventHandler(picChannelManager_Click);
		this.pictureBox1.Image = Monitor.Resources.image_source_destin;
		this.pictureBox1.Location = new System.Drawing.Point(35, 95);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(150, 51);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 195;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(picChannelManager_Click);
		this.picFiscalServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.picFiscalServer.Image = Monitor.Resources.image_help;
		this.picFiscalServer.Location = new System.Drawing.Point(513, 132);
		this.picFiscalServer.Name = "picFiscalServer";
		this.picFiscalServer.Size = new System.Drawing.Size(20, 20);
		this.picFiscalServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFiscalServer.TabIndex = 239;
		this.picFiscalServer.TabStop = false;
		this.lkbArticleSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lkbArticleSearch.AutoSize = true;
		this.lkbArticleSearch.Location = new System.Drawing.Point(535, 135);
		this.lkbArticleSearch.Name = "lkbArticleSearch";
		this.lkbArticleSearch.Size = new System.Drawing.Size(172, 14);
		this.lkbArticleSearch.TabIndex = 238;
		this.lkbArticleSearch.TabStop = true;
		this.lkbArticleSearch.Text = "Saiba mais. Veja passo a passo";
		this.lkbArticleSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbArticleSearch_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(776, 157);
		base.Controls.Add(this.picFiscalServer);
		base.Controls.Add(this.lkbArticleSearch);
		base.Controls.Add(this.picMySql);
		base.Controls.Add(this.picXmlError);
		base.Controls.Add(this.picXmlCorrect);
		base.Controls.Add(this.picSourceFolder);
		base.Controls.Add(this.picSourceEmail);
		base.Controls.Add(this.btClose);
		base.Controls.Add(this.htpnMessage01);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btChannelManager);
		base.Controls.Add(this.pictureBox1);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPanelXmlValidator";
		this.Text = "frmPanelXmlValidator";
		base.Load += new System.EventHandler(frmPanelXmlValidator_Load);
		((System.ComponentModel.ISupportInitialize)this.picSourceEmail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picSourceFolder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picXmlError).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picXmlCorrect).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picMySql).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picFiscalServer).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
