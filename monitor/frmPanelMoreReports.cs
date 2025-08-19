using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmPanelMoreReports : Form
{
	private List<Control> varControlList = new List<Control>();

	private static List<clsFeatStatus> varFeatStatList = new List<clsFeatStatus>();

	private clsFeatureService varclsFeatureService = new clsFeatureService();

	private List<clsObjectType> varLockedList = new List<clsObjectType>();

	private IContainer components;

	private Button btClose;

	private FlowLayoutPanel flpSeeMore;

	private Label lbSeeMore;

	private LinkLabel lkbReport01;

	private LinkLabel lkbReport02;

	private LinkLabel lkbReport03;

	private LinkLabel lkbReport04;

	private LinkLabel lkbReport05;

	private LinkLabel lkbReport06;

	private LinkLabel lkbReport07;

	private LinkLabel lkbReport08;

	public frmPanelMoreReports()
	{
		InitializeComponent();
	}

	private bool funcIsFeatLockedAsync(string pFeatExtId)
	{
		clsFeatStatus varclsFeatStatus = varFeatStatList.FirstOrDefault((clsFeatStatus r) => r.ftNodeKey.Equals(pFeatExtId));
		if (varclsFeatStatus == null)
		{
			varclsFeatStatus = Task.Run(async () => await varclsFeatureService.funcGetFeatStatusAsync(pFeatExtId)).Result;
		}
		if (varclsFeatStatus == null)
		{
			varclsFeatStatus = new clsFeatStatus
			{
				ftFeatType = "LOCKED"
			};
		}
		if (clsFunction.funcGetValue(varclsFeatStatus.ftFeatType).Contains("LOCK"))
		{
			return true;
		}
		return false;
	}

	private void frmPanelMoreReports_Load(object sender, EventArgs e)
	{
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			varclsTaskStatus = new clsTaskStatus();
		}
		LinkLabel linkLabel = lkbReport01;
		bool visible = (lkbReport02.Visible = false);
		linkLabel.Visible = visible;
		LinkLabel linkLabel2 = lkbReport03;
		visible = (lkbReport04.Visible = false);
		linkLabel2.Visible = visible;
		LinkLabel linkLabel3 = lkbReport05;
		visible = (lkbReport06.Visible = false);
		linkLabel3.Visible = visible;
		LinkLabel linkLabel4 = lkbReport07;
		visible = (lkbReport08.Visible = false);
		linkLabel4.Visible = visible;
		varControlList.Clear();
		varLockedList.Clear();
		foreach (clsObjData varTotItem in varclsTaskStatus.TotalList)
		{
			if (varTotItem.QuantNum <= 1)
			{
				continue;
			}
			Label varControl = null;
			if (!varControlList.Contains(lkbReport01))
			{
				varControl = lkbReport01;
			}
			else if (!varControlList.Contains(lkbReport02))
			{
				varControl = lkbReport02;
			}
			else if (!varControlList.Contains(lkbReport03))
			{
				varControl = lkbReport03;
			}
			else if (!varControlList.Contains(lkbReport04))
			{
				varControl = lkbReport04;
			}
			else if (!varControlList.Contains(lkbReport05))
			{
				varControl = lkbReport05;
			}
			else if (!varControlList.Contains(lkbReport06))
			{
				varControl = lkbReport06;
			}
			else if (!varControlList.Contains(lkbReport07))
			{
				varControl = lkbReport07;
			}
			else if (!varControlList.Contains(lkbReport08))
			{
				varControl = lkbReport08;
			}
			if (varControl == null)
			{
				continue;
			}
			if (varTotItem.ObjectId.Contains("-FUNC-"))
			{
				if (funcIsFeatLockedAsync(varTotItem.ObjectId))
				{
					varControl.Image = Resources.image_locker_16_16;
					varControl.Padding = new Padding(16, 3, 3, 3);
					clsObjectType varObjType = new clsObjectType
					{
						Name = varTotItem.TabName,
						Value = varTotItem.ObjectId
					};
					varLockedList.Add(varObjType);
				}
				else
				{
					varControl.Image = null;
					varControl.Padding = new Padding(3, 3, 3, 3);
				}
			}
			varControl.Visible = true;
			string varQuantNum = varTotItem.QuantNum.ToString();
			string varObjDesct = clsFunction.funcGetValue(varTotItem.ObjDesct).ToLower();
			varControl.Text = varQuantNum + " " + varObjDesct;
			varControl.Tag = varTotItem.TabName;
			varControlList.Add(varControl);
		}
		double varTotalWidth = (double)lbSeeMore.Width * 1.1;
		foreach (Control varControl2 in varControlList)
		{
			varTotalWidth += (double)varControl2.Width * 1.1;
		}
		if (varTotalWidth > (double)base.Width)
		{
			varclsTaskStatus.PanelHeight = 60;
		}
		else
		{
			varclsTaskStatus.PanelHeight = 32;
		}
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

	private async void lkbReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		LinkLabel varObject = (LinkLabel)sender;
		if (varObject == null)
		{
			return;
		}
		string varFormAction = (string)varObject.Tag;
		if (varFormAction == null)
		{
			return;
		}
		clsObjectType varLockedItem = varLockedList.FirstOrDefault((clsObjectType r) => r.Name.Equals(varFormAction));
		if (varLockedItem != null)
		{
			await new clsDataParameter().funcAddCounterAsync(varLockedItem.Value + "-CLICKS");
			await new clsManGeral().funcGetSalesActionAsync(this, varLockedItem.Value);
			return;
		}
		clsTaskStatus varclsTaskStatus = (clsTaskStatus)base.Tag;
		if (varclsTaskStatus == null)
		{
			varclsTaskStatus = new clsTaskStatus();
		}
		varclsTaskStatus.FormAction = varFormAction;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmPanelMoreReports));
		this.btClose = new System.Windows.Forms.Button();
		this.flpSeeMore = new System.Windows.Forms.FlowLayoutPanel();
		this.lkbReport01 = new System.Windows.Forms.LinkLabel();
		this.lkbReport02 = new System.Windows.Forms.LinkLabel();
		this.lkbReport03 = new System.Windows.Forms.LinkLabel();
		this.lkbReport04 = new System.Windows.Forms.LinkLabel();
		this.lkbReport05 = new System.Windows.Forms.LinkLabel();
		this.lkbReport06 = new System.Windows.Forms.LinkLabel();
		this.lkbReport07 = new System.Windows.Forms.LinkLabel();
		this.lkbReport08 = new System.Windows.Forms.LinkLabel();
		this.lbSeeMore = new System.Windows.Forms.Label();
		this.flpSeeMore.SuspendLayout();
		base.SuspendLayout();
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
		this.flpSeeMore.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.flpSeeMore.Controls.Add(this.lkbReport01);
		this.flpSeeMore.Controls.Add(this.lkbReport02);
		this.flpSeeMore.Controls.Add(this.lkbReport03);
		this.flpSeeMore.Controls.Add(this.lkbReport04);
		this.flpSeeMore.Controls.Add(this.lkbReport05);
		this.flpSeeMore.Controls.Add(this.lkbReport06);
		this.flpSeeMore.Controls.Add(this.lkbReport07);
		this.flpSeeMore.Controls.Add(this.lkbReport08);
		this.flpSeeMore.Location = new System.Drawing.Point(122, 0);
		this.flpSeeMore.Name = "flpSeeMore";
		this.flpSeeMore.Size = new System.Drawing.Size(633, 52);
		this.flpSeeMore.TabIndex = 194;
		this.lkbReport01.AutoSize = true;
		this.lkbReport01.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport01.Location = new System.Drawing.Point(4, 4);
		this.lkbReport01.Margin = new System.Windows.Forms.Padding(4);
		this.lkbReport01.Name = "lkbReport01";
		this.lkbReport01.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport01.Size = new System.Drawing.Size(132, 20);
		this.lkbReport01.TabIndex = 9;
		this.lkbReport01.TabStop = true;
		this.lkbReport01.Text = "26 cartas de correção";
		this.lkbReport01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport01.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport02.AutoSize = true;
		this.lkbReport02.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport02.Location = new System.Drawing.Point(143, 3);
		this.lkbReport02.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport02.Name = "lkbReport02";
		this.lkbReport02.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport02.Size = new System.Drawing.Size(162, 20);
		this.lkbReport02.TabIndex = 10;
		this.lkbReport02.TabStop = true;
		this.lkbReport02.Text = "26 documentos cancelados";
		this.lkbReport02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport02.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport03.AutoSize = true;
		this.lkbReport03.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport03.Location = new System.Drawing.Point(311, 3);
		this.lkbReport03.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport03.Name = "lkbReport03";
		this.lkbReport03.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport03.Size = new System.Drawing.Size(123, 20);
		this.lkbReport03.TabIndex = 11;
		this.lkbReport03.TabStop = true;
		this.lkbReport03.Text = "26 fretes duplicados";
		this.lkbReport03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport03.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport04.AutoSize = true;
		this.lkbReport04.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport04.Location = new System.Drawing.Point(440, 3);
		this.lkbReport04.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport04.Name = "lkbReport04";
		this.lkbReport04.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport04.Size = new System.Drawing.Size(175, 20);
		this.lkbReport04.TabIndex = 12;
		this.lkbReport04.TabStop = true;
		this.lkbReport04.Text = "26 averbações de exportação";
		this.lkbReport04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport04.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport05.AutoSize = true;
		this.lkbReport05.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport05.Location = new System.Drawing.Point(3, 31);
		this.lkbReport05.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport05.Name = "lkbReport05";
		this.lkbReport05.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport05.Size = new System.Drawing.Size(162, 20);
		this.lkbReport05.TabIndex = 13;
		this.lkbReport05.TabStop = true;
		this.lkbReport05.Text = "26 documentos cancelados";
		this.lkbReport05.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport05.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport06.AutoSize = true;
		this.lkbReport06.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport06.Location = new System.Drawing.Point(171, 31);
		this.lkbReport06.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport06.Name = "lkbReport06";
		this.lkbReport06.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport06.Size = new System.Drawing.Size(162, 20);
		this.lkbReport06.TabIndex = 14;
		this.lkbReport06.TabStop = true;
		this.lkbReport06.Text = "26 documentos cancelados";
		this.lkbReport06.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport06.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport07.AutoSize = true;
		this.lkbReport07.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport07.Location = new System.Drawing.Point(339, 31);
		this.lkbReport07.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport07.Name = "lkbReport07";
		this.lkbReport07.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport07.Size = new System.Drawing.Size(85, 20);
		this.lkbReport07.TabIndex = 15;
		this.lkbReport07.TabStop = true;
		this.lkbReport07.Text = "26 incorretos";
		this.lkbReport07.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport07.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lkbReport08.AutoSize = true;
		this.lkbReport08.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lkbReport08.Location = new System.Drawing.Point(430, 31);
		this.lkbReport08.Margin = new System.Windows.Forms.Padding(3);
		this.lkbReport08.Name = "lkbReport08";
		this.lkbReport08.Padding = new System.Windows.Forms.Padding(3);
		this.lkbReport08.Size = new System.Drawing.Size(85, 20);
		this.lkbReport08.TabIndex = 16;
		this.lkbReport08.TabStop = true;
		this.lkbReport08.Text = "26 incorretos";
		this.lkbReport08.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbReport08.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbReport_LinkClicked);
		this.lbSeeMore.Image = Monitor.Resources.image_see_more;
		this.lbSeeMore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbSeeMore.Location = new System.Drawing.Point(3, 3);
		this.lbSeeMore.Margin = new System.Windows.Forms.Padding(0);
		this.lbSeeMore.Name = "lbSeeMore";
		this.lbSeeMore.Size = new System.Drawing.Size(116, 23);
		this.lbSeeMore.TabIndex = 0;
		this.lbSeeMore.Text = "Veja tambem !";
		this.lbSeeMore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(776, 54);
		base.Controls.Add(this.lbSeeMore);
		base.Controls.Add(this.btClose);
		base.Controls.Add(this.flpSeeMore);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmPanelMoreReports";
		this.Text = "frmPanelMoreReports";
		base.Load += new System.EventHandler(frmPanelMoreReports_Load);
		this.flpSeeMore.ResumeLayout(false);
		this.flpSeeMore.PerformLayout();
		base.ResumeLayout(false);
	}
}
