using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace Monitor;

public class frmOnboardStep02Ch03Sc02 : Form
{
	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label8;

	private Label label2;

	private Label label6;

	private Label label11;

	private LinkLabel linkLabel5;

	private Label label10;

	private LinkLabel linkLabel4;

	private Label label7;

	private LinkLabel linkLabel3;

	private Label label3;

	private LinkLabel linkLabel2;

	private Label label1;

	private LinkLabel linkLabel1;

	private Panel panel3;

	private Label label4;

	private Label label5;

	private Label label9;

	public frmOnboardStep02Ch03Sc02()
	{
		InitializeComponent();
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/extraindo-documentos-do-e-mail-automaticamente", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/importando-documentos-de-saida-de-uma-pasta-de-diretorio-automaticamente", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/robo-para-transmissao-de-documentos-fiscal-io-cliente", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/download-documentos-nfe-cte-saida-autxml", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/consulta-de-documentos-de-saida-diretamente-da-sefaz", "onboard");
		Cursor = Cursors.Default;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboardStep02Ch03Sc02));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.linkLabel5 = new System.Windows.Forms.LinkLabel();
		this.label10 = new System.Windows.Forms.Label();
		this.linkLabel4 = new System.Windows.Forms.LinkLabel();
		this.label7 = new System.Windows.Forms.Label();
		this.linkLabel3 = new System.Windows.Forms.LinkLabel();
		this.label3 = new System.Windows.Forms.Label();
		this.linkLabel2 = new System.Windows.Forms.LinkLabel();
		this.label1 = new System.Windows.Forms.Label();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.label6 = new System.Windows.Forms.Label();
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
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add(this.linkLabel5);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.linkLabel4);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.linkLabel3);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.linkLabel2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.linkLabel1);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Location = new System.Drawing.Point(12, 12);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(539, 351);
		this.panel2.TabIndex = 200;
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Location = new System.Drawing.Point(236, 325);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(67, 23);
		this.panel3.TabIndex = 218;
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
		this.label11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label11.Location = new System.Drawing.Point(31, 260);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(21, 16);
		this.label11.TabIndex = 217;
		this.label11.Text = "5.";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel5.Location = new System.Drawing.Point(58, 260);
		this.linkLabel5.Name = "linkLabel5";
		this.linkLabel5.Size = new System.Drawing.Size(478, 16);
		this.linkLabel5.TabIndex = 216;
		this.linkLabel5.TabStop = true;
		this.linkLabel5.Text = "Ativar a busca automática direto da SEFAZ da UF";
		this.linkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel5_LinkClicked);
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label10.Location = new System.Drawing.Point(31, 223);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(21, 16);
		this.label10.TabIndex = 215;
		this.label10.Text = "4.";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel4.Location = new System.Drawing.Point(58, 223);
		this.linkLabel4.Name = "linkLabel4";
		this.linkLabel4.Size = new System.Drawing.Size(478, 16);
		this.linkLabel4.TabIndex = 214;
		this.linkLabel4.TabStop = true;
		this.linkLabel4.Text = "Baixar automaticamente da SEFAZ utilizando a tag AutXML";
		this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel4_LinkClicked);
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label7.Location = new System.Drawing.Point(31, 188);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(21, 16);
		this.label7.TabIndex = 213;
		this.label7.Text = "3.";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel3.Location = new System.Drawing.Point(58, 188);
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.Size = new System.Drawing.Size(478, 16);
		this.linkLabel3.TabIndex = 212;
		this.linkLabel3.TabStop = true;
		this.linkLabel3.Text = "Utilizar nosso robô para transmitir os XMLs de um terceiro para a sua instalação";
		this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(31, 156);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(21, 16);
		this.label3.TabIndex = 211;
		this.label3.Text = "2.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel2.Location = new System.Drawing.Point(58, 156);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(478, 16);
		this.linkLabel2.TabIndex = 210;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "Importar automaticamente os XMLs a partir de uma pasta interna";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(31, 123);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(21, 16);
		this.label1.TabIndex = 209;
		this.label1.Text = "1.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel1.Location = new System.Drawing.Point(58, 123);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(478, 16);
		this.linkLabel1.TabIndex = 208;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Importar automaticamente os XMLs que chegam por e-mail";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.label6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label6.Location = new System.Drawing.Point(3, 84);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(533, 19);
		this.label6.TabIndex = 206;
		this.label6.Text = "(abra o link para acompanhar)";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(3, 60);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(533, 24);
		this.label2.TabIndex = 199;
		this.label2.Text = "Para obter 100% dos documentos, você pode usar as opções:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(3, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(533, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Buscar Documentos de Saída";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(563, 375);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnboardStep02Ch03Sc02";
		this.Text = "frmOnboardStep02Ch03Sc02";
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
