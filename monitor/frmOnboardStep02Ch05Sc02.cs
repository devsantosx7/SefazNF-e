using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace Monitor;

public class frmOnboardStep02Ch05Sc02 : Form
{
	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label8;

	private Label label2;

	private Label label1;

	private Panel panel3;

	private Label label4;

	private Label label5;

	private Label label9;

	private LinkLabel linkLabel1;

	private LinkLabel linkLabel2;

	private Label label6;

	private Label label3;

	public frmOnboardStep02Ch05Sc02()
	{
		InitializeComponent();
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/preparando-um-arquivo-txt-com-as-chaves-de-acesso", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallAuditorHelp();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboardStep02Ch05Sc02));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.linkLabel2 = new System.Windows.Forms.LinkLabel();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(563, 375);
		this.panel1.TabIndex = 83;
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel2.Controls.Add(this.linkLabel1);
		this.panel2.Controls.Add(this.linkLabel2);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Location = new System.Drawing.Point(12, 12);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(539, 351);
		this.panel2.TabIndex = 200;
		this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
		this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel1.Location = new System.Drawing.Point(94, 139);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(86, 16);
		this.linkLabel1.TabIndex = 224;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "[Saiba mais]";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.linkLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
		this.linkLabel2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel2.Location = new System.Drawing.Point(94, 209);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(86, 16);
		this.linkLabel2.TabIndex = 223;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "[Saiba mais]";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label6.Location = new System.Drawing.Point(94, 189);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(404, 20);
		this.label6.TabIndex = 222;
		this.label6.Text = "2. Importar arquivos SPED ICMS/IPI e selecione as chaves de acesso";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(94, 119);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(306, 20);
		this.label3.TabIndex = 221;
		this.label3.Text = "1. Importar arquivos TXT com as chaves de acesso";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Location = new System.Drawing.Point(236, 325);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(67, 23);
		this.panel3.TabIndex = 220;
		this.label4.BackColor = System.Drawing.Color.Transparent;
		this.label4.Font = new System.Drawing.Font("Verdana", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Image = Monitor.Resources.circle_empty;
		this.label4.Location = new System.Drawing.Point(47, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(16, 16);
		this.label4.TabIndex = 210;
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Font = new System.Drawing.Font("Verdana", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Image = Monitor.Resources.circle_filled;
		this.label5.Location = new System.Drawing.Point(25, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(16, 16);
		this.label5.TabIndex = 207;
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.BackColor = System.Drawing.Color.Transparent;
		this.label9.Font = new System.Drawing.Font("Verdana", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Image = Monitor.Resources.circle_empty;
		this.label9.Location = new System.Drawing.Point(3, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(16, 16);
		this.label9.TabIndex = 208;
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(2, 267);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(533, 27);
		this.label1.TabIndex = 206;
		this.label1.Text = "O Fiscal.io fará os downloads em massa.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(3, 71);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(533, 38);
		this.label2.TabIndex = 199;
		this.label2.Text = "Para grandes quantidades de documentos utilize uma das seguintes opções:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(3, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(533, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Recuperação de documentos do passado";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(563, 375);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnboardStep02Ch05Sc02";
		this.Text = "frmOnboardStep02Ch05Sc02";
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
