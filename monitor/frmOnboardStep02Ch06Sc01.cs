using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using srv.fiscal.io;

namespace Monitor;

public class frmOnboardStep02Ch06Sc01 : Form
{
	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label8;

	private Label label2;

	private LinkLabel linkLabel5;

	private LinkLabel linkLabel4;

	private LinkLabel linkLabel3;

	private LinkLabel linkLabel2;

	private LinkLabel linkLabel1;

	private LinkLabel linkLabel6;

	private LinkLabel linkLabel7;

	private LinkLabel linkLabel8;

	private LinkLabel linkLabel9;

	private LinkLabel linkLabel10;

	private LinkLabel linkLabel12;

	private LinkLabel linkLabel11;

	private Label label3;

	public frmOnboardStep02Ch06Sc01()
	{
		InitializeComponent();
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/registrando-a-manifestacao-do-destinatario", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/averbacao-de-exportacao", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-saber-quando-um-cliente-recusa-uma-nfe-emitida-pela-sua-empresa", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-monitorar-cancelamentos-sobre-documentos-de-entrada", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-converter-xml-em-pdf", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallChatWebPageAsync("onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-converter-xml-em-edi-proceda-notfis-conemb", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-integrar-o-fiscal-io-monitor-com-seu-sistema-de-gestao", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-exportar-um-relatorio-do-fiscal-io-monitor-em-excel", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-consultar-o-status-dos-documentos-na-sefaz-atraves-de-seus-eventos", "onboard");
		Cursor = Cursors.Default;
	}

	private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		clsHelpService.funcCallWebPage("https://suporte.fiscal.io/portal/pt/kb/articles/como-auditar-arquivos-efd-cruzando-com-a-base-de-xml-do-fiscal-io-monitor", "onboard");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboardStep02Ch06Sc01));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.linkLabel12 = new System.Windows.Forms.LinkLabel();
		this.linkLabel11 = new System.Windows.Forms.LinkLabel();
		this.label3 = new System.Windows.Forms.Label();
		this.linkLabel6 = new System.Windows.Forms.LinkLabel();
		this.linkLabel7 = new System.Windows.Forms.LinkLabel();
		this.linkLabel8 = new System.Windows.Forms.LinkLabel();
		this.linkLabel9 = new System.Windows.Forms.LinkLabel();
		this.linkLabel10 = new System.Windows.Forms.LinkLabel();
		this.linkLabel5 = new System.Windows.Forms.LinkLabel();
		this.linkLabel4 = new System.Windows.Forms.LinkLabel();
		this.linkLabel3 = new System.Windows.Forms.LinkLabel();
		this.linkLabel2 = new System.Windows.Forms.LinkLabel();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(790, 400);
		this.panel1.TabIndex = 83;
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel2.Controls.Add(this.linkLabel12);
		this.panel2.Controls.Add(this.linkLabel11);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.linkLabel6);
		this.panel2.Controls.Add(this.linkLabel7);
		this.panel2.Controls.Add(this.linkLabel8);
		this.panel2.Controls.Add(this.linkLabel9);
		this.panel2.Controls.Add(this.linkLabel10);
		this.panel2.Controls.Add(this.linkLabel5);
		this.panel2.Controls.Add(this.linkLabel4);
		this.panel2.Controls.Add(this.linkLabel3);
		this.panel2.Controls.Add(this.linkLabel2);
		this.panel2.Controls.Add(this.linkLabel1);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Location = new System.Drawing.Point(14, 12);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(764, 374);
		this.panel2.TabIndex = 200;
		this.linkLabel12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel12.Location = new System.Drawing.Point(397, 342);
		this.linkLabel12.Name = "linkLabel12";
		this.linkLabel12.Size = new System.Drawing.Size(103, 16);
		this.linkLabel12.TabIndex = 224;
		this.linkLabel12.TabStop = true;
		this.linkLabel12.Text = "Central de Ajuda";
		this.linkLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel12.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel12_LinkClicked);
		this.linkLabel11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel11.Location = new System.Drawing.Point(271, 342);
		this.linkLabel11.Name = "linkLabel11";
		this.linkLabel11.Size = new System.Drawing.Size(103, 16);
		this.linkLabel11.TabIndex = 223;
		this.linkLabel11.TabStop = true;
		this.linkLabel11.Text = "Acesse o chat";
		this.linkLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel11.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel11_LinkClicked);
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(15, 310);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(732, 32);
		this.label3.TabIndex = 222;
		this.label3.Text = "Se precisar nos consulte pelo chat ou acesse a Central de Ajuda.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel6.Location = new System.Drawing.Point(400, 262);
		this.linkLabel6.Name = "linkLabel6";
		this.linkLabel6.Size = new System.Drawing.Size(347, 16);
		this.linkLabel6.TabIndex = 221;
		this.linkLabel6.TabStop = true;
		this.linkLabel6.Text = "Auditar arquivos EFD cruzando com uma base de XMLs";
		this.linkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel6_LinkClicked);
		this.linkLabel7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel7.Location = new System.Drawing.Point(400, 225);
		this.linkLabel7.Name = "linkLabel7";
		this.linkLabel7.Size = new System.Drawing.Size(317, 16);
		this.linkLabel7.TabIndex = 220;
		this.linkLabel7.TabStop = true;
		this.linkLabel7.Text = "Consultar status de XMLs na SEFAZ";
		this.linkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel7_LinkClicked);
		this.linkLabel8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel8.Location = new System.Drawing.Point(400, 190);
		this.linkLabel8.Name = "linkLabel8";
		this.linkLabel8.Size = new System.Drawing.Size(317, 16);
		this.linkLabel8.TabIndex = 219;
		this.linkLabel8.TabStop = true;
		this.linkLabel8.Text = "Exportar relatórios de XMLs para o Excel";
		this.linkLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel8_LinkClicked);
		this.linkLabel9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel9.Location = new System.Drawing.Point(400, 158);
		this.linkLabel9.Name = "linkLabel9";
		this.linkLabel9.Size = new System.Drawing.Size(317, 16);
		this.linkLabel9.TabIndex = 218;
		this.linkLabel9.TabStop = true;
		this.linkLabel9.Text = "Integrar o Fiscal.io Monitor ao seu sistema de gestão";
		this.linkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel9.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel9_LinkClicked);
		this.linkLabel10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel10.Location = new System.Drawing.Point(400, 125);
		this.linkLabel10.Name = "linkLabel10";
		this.linkLabel10.Size = new System.Drawing.Size(317, 16);
		this.linkLabel10.TabIndex = 217;
		this.linkLabel10.TabStop = true;
		this.linkLabel10.Text = "Converter XMLs em EDI (PROCEDA)";
		this.linkLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel10.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel10_LinkClicked);
		this.linkLabel5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel5.Location = new System.Drawing.Point(58, 262);
		this.linkLabel5.Name = "linkLabel5";
		this.linkLabel5.Size = new System.Drawing.Size(317, 16);
		this.linkLabel5.TabIndex = 216;
		this.linkLabel5.TabStop = true;
		this.linkLabel5.Text = "Converter XMLs em PDF";
		this.linkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel5_LinkClicked);
		this.linkLabel4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel4.Location = new System.Drawing.Point(58, 225);
		this.linkLabel4.Name = "linkLabel4";
		this.linkLabel4.Size = new System.Drawing.Size(317, 16);
		this.linkLabel4.TabIndex = 214;
		this.linkLabel4.TabStop = true;
		this.linkLabel4.Text = "Monitorar cancelamentos";
		this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel4_LinkClicked);
		this.linkLabel3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel3.Location = new System.Drawing.Point(58, 190);
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.Size = new System.Drawing.Size(317, 16);
		this.linkLabel3.TabIndex = 212;
		this.linkLabel3.TabStop = true;
		this.linkLabel3.Text = "Acompanhar recusa de cliente sobre NFe de saída";
		this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		this.linkLabel2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel2.Location = new System.Drawing.Point(58, 158);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(317, 16);
		this.linkLabel2.TabIndex = 210;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "Obter a Averbação de Exportação";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel1.Location = new System.Drawing.Point(58, 125);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(317, 16);
		this.linkLabel1.TabIndex = 208;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Manifestação do destinatário";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(15, 50);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(732, 50);
		this.label2.TabIndex = 199;
		this.label2.Text = "Abra o link correspondente ao ponto que precisa resolver e mantenha \r\naberto no seu navegador para consultar durante o uso do sistema:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(15, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(732, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Outros";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(790, 400);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnboardStep02Ch06Sc01";
		this.Text = "frmOnboardStep02Ch06Sc01";
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
