using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;

namespace Monitor;

public class frmOnboardStep02 : Form
{
	private string _UserChoise = string.Empty;

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label2;

	private Label label8;

	private Label label19;

	private Label label20;

	private Button button5;

	private Label label6;

	private Button button4;

	private Label label3;

	private Button button3;

	private Label label5;

	private Button button2;

	private Label label4;

	private Button button1;

	private Label label1;

	private Button btnBack;

	public frmOnboardStep02()
	{
		InitializeComponent();
	}

	public string funcGetChoise()
	{
		return _UserChoise;
	}

	private async void btnButton_Click(object sender, EventArgs e)
	{
		Button varButton = (Button)sender;
		_UserChoise = varButton.Text;
		base.Visible = false;
		await _clsDataParam.funcSetAsync("FIRST_INTENTION", varButton.Text);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboardStep02));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.button5 = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.button4 = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.button3 = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.btnBack = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(770, 457);
		this.panel1.TabIndex = 83;
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel2.Controls.Add(this.button5);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.button4);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.button3);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.button2);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.button1);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.btnBack);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.label20);
		this.panel2.Location = new System.Drawing.Point(23, 24);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(722, 403);
		this.panel2.TabIndex = 200;
		this.button5.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.button5.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button5.ForeColor = System.Drawing.Color.White;
		this.button5.Location = new System.Drawing.Point(29, 344);
		this.button5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.button5.Name = "button5";
		this.button5.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.button5.Size = new System.Drawing.Size(293, 37);
		this.button5.TabIndex = 209;
		this.button5.Text = "Outros";
		this.button5.UseVisualStyleBackColor = false;
		this.button5.Click += new System.EventHandler(btnButton_Click);
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label6.Location = new System.Drawing.Point(329, 344);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(378, 37);
		this.label6.TabIndex = 208;
		this.label6.Text = "Manifestação do destinatário em massa, Averbação de Exportação, Classificar XMLs, auditar arquivos EFD, entre outros.";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button4.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.button4.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button4.ForeColor = System.Drawing.Color.White;
		this.button4.Location = new System.Drawing.Point(29, 291);
		this.button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.button4.Name = "button4";
		this.button4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.button4.Size = new System.Drawing.Size(293, 37);
		this.button4.TabIndex = 207;
		this.button4.Text = "Recuperação de documentos do passado";
		this.button4.UseVisualStyleBackColor = false;
		this.button4.Click += new System.EventHandler(btnButton_Click);
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(329, 291);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(378, 37);
		this.label3.TabIndex = 206;
		this.label3.Text = "Recupere XMLs de NFe ou CTe dos últimos 12 anos, importando no sistema suas chaves de acesso. Automático e em massa.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.button3.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.ForeColor = System.Drawing.Color.White;
		this.button3.Location = new System.Drawing.Point(29, 238);
		this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.button3.Name = "button3";
		this.button3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.button3.Size = new System.Drawing.Size(293, 37);
		this.button3.TabIndex = 205;
		this.button3.Text = "Desacordo de Serviço (CTe)";
		this.button3.UseVisualStyleBackColor = false;
		this.button3.Click += new System.EventHandler(btnButton_Click);
		this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label5.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label5.Location = new System.Drawing.Point(329, 238);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(378, 37);
		this.label5.TabIndex = 204;
		this.label5.Text = "Registrar o evento Prestação de Serviço em Desacordo que cancela um CTe com erro (reversão de frete).";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.button2.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button2.ForeColor = System.Drawing.Color.White;
		this.button2.Location = new System.Drawing.Point(29, 185);
		this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.button2.Name = "button2";
		this.button2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.button2.Size = new System.Drawing.Size(293, 37);
		this.button2.TabIndex = 203;
		this.button2.Text = "Buscar Documentos de Saída";
		this.button2.UseVisualStyleBackColor = false;
		this.button2.Click += new System.EventHandler(btnButton_Click);
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label4.Location = new System.Drawing.Point(329, 185);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(378, 37);
		this.label4.TabIndex = 202;
		this.label4.Text = "Consultar, baixar e gerenciar NFe, CTe, NFCe, CFe SAT e eventos emitidos pelas empresas cadastradas.";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.button1.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(29, 132);
		this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.button1.Name = "button1";
		this.button1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.button1.Size = new System.Drawing.Size(293, 37);
		this.button1.TabIndex = 201;
		this.button1.Text = "Buscar NFe/CTe emitidos contra CNPJ";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(btnButton_Click);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label1.Location = new System.Drawing.Point(329, 132);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(378, 37);
		this.label1.TabIndex = 200;
		this.label1.Text = "Consultar, baixar e gerenciar NFe, CTe, MDFe e eventos emitidos contra as empresas cadastradas.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btnBack.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnBack.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBack.ForeColor = System.Drawing.Color.White;
		this.btnBack.Location = new System.Drawing.Point(29, 79);
		this.btnBack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnBack.Name = "btnBack";
		this.btnBack.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
		this.btnBack.Size = new System.Drawing.Size(293, 37);
		this.btnBack.TabIndex = 199;
		this.btnBack.Text = "Buscar NFe/CTe emitidos contra CPF";
		this.btnBack.UseVisualStyleBackColor = false;
		this.btnBack.Click += new System.EventHandler(btnButton_Click);
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(329, 79);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(378, 37);
		this.label2.TabIndex = 197;
		this.label2.Text = "Consultar, baixar e gerenciar NFe ou CTe emitidos contra CPFs cadastrados no Fiscal.io Monitor.";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label8.Location = new System.Drawing.Point(59, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(541, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Qual é o seu principal objetivo ao utilizar o Fiscal.io Monitor?";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label19.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label19.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label19.Location = new System.Drawing.Point(51, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(2, 23);
		this.label19.TabIndex = 187;
		this.label20.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label20.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label20.Location = new System.Drawing.Point(24, 16);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(21, 23);
		this.label20.TabIndex = 185;
		this.label20.Text = "2";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(770, 457);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnboardStep02";
		this.Text = "frmOnboardStep02";
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
