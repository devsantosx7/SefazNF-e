using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using util.fiscal.io;

namespace Monitor;

public class frmPanelUserStatus : Form
{
	private IContainer components;

	private Button btClose;

	private Label lbProgress03;

	private Label lbProgress02;

	private Label lbProgress01;

	private LinkLabel lkbStatus;

	private Label lbResult;

	private PictureBox picProgress;

	public frmPanelUserStatus()
	{
		InitializeComponent();
	}

	private void frmPanelUserStatus_TextChanged(object sender, EventArgs e)
	{
		try
		{
			funcShowUserData();
		}
		catch (Exception)
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	public void funcShowUserData()
	{
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			return;
		}
		if (varclsTaskStatus.Progress)
		{
			picProgress.Image = ((varclsTaskStatus.Image != null) ? varclsTaskStatus.Image : Resources.gif_loading);
			if (!string.IsNullOrEmpty(varclsTaskStatus.Message01) && !string.IsNullOrEmpty(varclsTaskStatus.Message02))
			{
				lbProgress01.Visible = true;
				lbProgress01.Text = varclsTaskStatus.Message01;
				lbProgress02.Visible = true;
				lbProgress02.Text = varclsTaskStatus.Message02;
				lbProgress03.Visible = false;
				lbProgress03.Text = string.Empty;
			}
			else if (!string.IsNullOrEmpty(varclsTaskStatus.Message01))
			{
				lbProgress01.Visible = false;
				lbProgress01.Text = string.Empty;
				lbProgress02.Visible = false;
				lbProgress02.Text = string.Empty;
				lbProgress03.Visible = true;
				lbProgress03.Text = varclsTaskStatus.Message01;
			}
			else if (!string.IsNullOrEmpty(varclsTaskStatus.Message02))
			{
				lbProgress01.Visible = false;
				lbProgress01.Text = string.Empty;
				lbProgress02.Visible = false;
				lbProgress02.Text = string.Empty;
				lbProgress03.Visible = true;
				lbProgress03.Text = varclsTaskStatus.Message02;
			}
			lbResult.Visible = false;
			lkbStatus.Visible = false;
			lkbStatus.Tag = new clsReturn();
		}
		else
		{
			if (varclsTaskStatus.Error)
			{
				picProgress.Image = ((varclsTaskStatus.Image != null) ? varclsTaskStatus.Image : Resources.image_error);
			}
			else if (varclsTaskStatus.Warning)
			{
				picProgress.Image = ((varclsTaskStatus.Image != null) ? varclsTaskStatus.Image : Resources.image_warning);
			}
			else
			{
				picProgress.Image = ((varclsTaskStatus.Image != null) ? varclsTaskStatus.Image : Resources.image_ok);
			}
			lbProgress01.Visible = false;
			lbProgress02.Visible = false;
			lbProgress03.Visible = false;
			if (!clsFunction.IsEmpty(varclsTaskStatus.ButtonText))
			{
				lbResult.Visible = true;
				lbResult.Text = varclsTaskStatus.Message01;
			}
			else
			{
				lbProgress03.Visible = true;
				lbProgress03.Text = varclsTaskStatus.Message01;
			}
			lkbStatus = funcGetLinkLabel();
			if (!clsFunction.IsEmpty(varclsTaskStatus.ButtonText))
			{
				lkbStatus.Visible = true;
			}
			else
			{
				lkbStatus.Visible = false;
			}
			lkbStatus.Text = varclsTaskStatus.ButtonText;
			lkbStatus.LinkClicked -= varclsTaskStatus.Event;
			lkbStatus.LinkClicked += varclsTaskStatus.Event;
			lkbStatus.Tag = varclsTaskStatus.Return;
		}
		Application.DoEvents();
	}

	private LinkLabel funcGetLinkLabel()
	{
		LinkLabel varlkbStatus = null;
		foreach (Control varclsItem in base.Controls)
		{
			if (clsFunction.IsEqual(varclsItem.Name, "lkbStatus", pIgnoreCase: true))
			{
				varlkbStatus = (LinkLabel)varclsItem;
				break;
			}
		}
		if (varlkbStatus == null)
		{
			varlkbStatus = new LinkLabel();
		}
		varlkbStatus.AutoSize = true;
		varlkbStatus.Location = new Point(39, 19);
		varlkbStatus.Name = "lkbStatus";
		varlkbStatus.Size = new Size(120, 13);
		varlkbStatus.TabIndex = 100;
		varlkbStatus.TabStop = true;
		base.Controls.Add(varlkbStatus);
		return varlkbStatus;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmPanelUserStatus));
		this.btClose = new System.Windows.Forms.Button();
		this.lbProgress03 = new System.Windows.Forms.Label();
		this.lbProgress02 = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.lkbStatus = new System.Windows.Forms.LinkLabel();
		this.lbResult = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		base.SuspendLayout();
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Tahoma", 6f);
		this.btClose.Location = new System.Drawing.Point(757, 2);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(17, 19);
		this.btClose.TabIndex = 2;
		this.btClose.Text = "X";
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lbProgress03.AutoSize = true;
		this.lbProgress03.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.lbProgress03.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress03.Location = new System.Drawing.Point(40, 14);
		this.lbProgress03.Name = "lbProgress03";
		this.lbProgress03.Size = new System.Drawing.Size(15, 13);
		this.lbProgress03.TabIndex = 109;
		this.lbProgress03.Text = "..";
		this.lbProgress03.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbProgress02.AutoSize = true;
		this.lbProgress02.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.lbProgress02.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress02.Location = new System.Drawing.Point(40, 22);
		this.lbProgress02.Name = "lbProgress02";
		this.lbProgress02.Size = new System.Drawing.Size(15, 13);
		this.lbProgress02.TabIndex = 108;
		this.lbProgress02.Text = "..";
		this.lbProgress02.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbProgress01.AutoSize = true;
		this.lbProgress01.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.lbProgress01.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress01.Location = new System.Drawing.Point(40, 6);
		this.lbProgress01.Name = "lbProgress01";
		this.lbProgress01.Size = new System.Drawing.Size(15, 13);
		this.lbProgress01.TabIndex = 107;
		this.lbProgress01.Text = "..";
		this.lbProgress01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbStatus.AutoSize = true;
		this.lkbStatus.Location = new System.Drawing.Point(40, 22);
		this.lkbStatus.Name = "lkbStatus";
		this.lkbStatus.Size = new System.Drawing.Size(115, 14);
		this.lkbStatus.TabIndex = 106;
		this.lkbStatus.TabStop = true;
		this.lkbStatus.Text = "Ver status da busca";
		this.lbResult.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbResult.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.lbResult.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbResult.Location = new System.Drawing.Point(40, 6);
		this.lbResult.Name = "lbResult";
		this.lbResult.Size = new System.Drawing.Size(711, 13);
		this.lbResult.TabIndex = 105;
		this.lbResult.Text = ".............";
		this.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picProgress.Image = Monitor.Resources.gif_loading;
		this.picProgress.Location = new System.Drawing.Point(5, 6);
		this.picProgress.Name = "picProgress";
		this.picProgress.Size = new System.Drawing.Size(30, 30);
		this.picProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picProgress.TabIndex = 104;
		this.picProgress.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		base.ClientSize = new System.Drawing.Size(776, 43);
		base.Controls.Add(this.lbProgress03);
		base.Controls.Add(this.lbProgress02);
		base.Controls.Add(this.lbProgress01);
		base.Controls.Add(this.lkbStatus);
		base.Controls.Add(this.lbResult);
		base.Controls.Add(this.picProgress);
		base.Controls.Add(this.btClose);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPanelUserStatus";
		this.Text = "frmPanelUserStatus";
		base.TextChanged += new System.EventHandler(frmPanelUserStatus_TextChanged);
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
