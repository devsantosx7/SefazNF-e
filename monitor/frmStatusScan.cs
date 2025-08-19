using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmStatusScan : Form
{
	private FilialView varclsFilial;

	private clsReturn varclsReturn;

	private object p;

	private IContainer components;

	private Panel panel1;

	private Label lbCTeEvent;

	private Label lbCTeDocOther;

	private Label lbCTeDocToma;

	private Label lbNFeEventOut;

	private Label lbNFeEventInb;

	private Label lbNFeDocInb;

	private Label label5;

	private Label label14;

	private Label label15;

	private Label label17;

	private Label label13;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label9;

	private Label label16;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private Label lbNotasFiscais;

	private Label label19;

	private Label lbDocScanTitle;

	private Label lbMDFeEvent;

	private Label lbMDFeDocInb;

	private Label label22;

	private Label label23;

	private Label label24;

	private Label label25;

	private Label label26;

	private Label label27;

	private Label lbLine01;

	private Button btSair;

	private Label label18;

	private Label label20;

	private Label label21;

	private Label label28;

	private Label label29;

	private Label label30;

	private Label lbNFSeEventInb;

	private Label lbNFSeDocInb;

	public frmStatusScan(clsReturn pclsReturn, FilialView pclsFilial)
	{
		InitializeComponent();
		varclsFilial = pclsFilial;
		funcShowResult(pclsReturn);
	}

	public frmStatusScan(clsReturn varclsReturn, object p)
	{
		this.varclsReturn = varclsReturn;
		this.p = p;
	}

	private void funcShowResult(clsReturn pclsReturn)
	{
		if (varclsFilial != null)
		{
			lbDocScanTitle.Text = "Resumo da busca na SEFAZ : " + varclsFilial.Nome;
		}
		else
		{
			lbDocScanTitle.Text = "Resumo da busca na SEFAZ : Todas as Empresas";
		}
		clsDFeObjects varDFeObjects = new clsDFeCodes().funcGetDFeObjects(pclsReturn);
		if (varDFeObjects == null)
		{
			varDFeObjects = new clsDFeObjects();
		}
		lbNFeDocInb.Text = varDFeObjects.NFeDocInb.ToString();
		lbNFeEventInb.Text = varDFeObjects.NFeEventInb.ToString();
		lbNFeEventOut.Text = varDFeObjects.NFeEventOut.ToString();
		lbNFSeDocInb.Text = varDFeObjects.NFSeDocInb.ToString();
		lbNFSeEventInb.Text = varDFeObjects.NFSeEventInb.ToString();
		lbCTeDocToma.Text = varDFeObjects.CTeDocToma.ToString();
		lbCTeDocOther.Text = varDFeObjects.CTeDocOther.ToString();
		long varTotalCTe = varDFeObjects.CTeEventInb + varDFeObjects.CTeEventOut;
		lbCTeEvent.Text = varTotalCTe.ToString();
		lbMDFeDocInb.Text = varDFeObjects.MDFeDocInb.ToString();
		long varTotalMDFe = varDFeObjects.MDFeEventInb + varDFeObjects.MDFeEventOut;
		lbMDFeEvent.Text = varTotalMDFe.ToString();
	}

	private void btSair_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmStatusScan_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			Close();
		}
		if (e.KeyCode == Keys.Escape)
		{
			Close();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmStatusScan));
		this.panel1 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.lbDocScanTitle = new System.Windows.Forms.Label();
		this.lbCTeEvent = new System.Windows.Forms.Label();
		this.lbCTeDocOther = new System.Windows.Forms.Label();
		this.lbCTeDocToma = new System.Windows.Forms.Label();
		this.lbNFeEventOut = new System.Windows.Forms.Label();
		this.lbNFeEventInb = new System.Windows.Forms.Label();
		this.lbNFeDocInb = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.lbNotasFiscais = new System.Windows.Forms.Label();
		this.btSair = new System.Windows.Forms.Button();
		this.lbLine01 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.lbMDFeDocInb = new System.Windows.Forms.Label();
		this.lbMDFeEvent = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.lbNFSeDocInb = new System.Windows.Forms.Label();
		this.lbNFSeEventInb = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.lbNFSeEventInb);
		this.panel1.Controls.Add(this.lbNFSeDocInb);
		this.panel1.Controls.Add(this.label18);
		this.panel1.Controls.Add(this.label20);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Controls.Add(this.label28);
		this.panel1.Controls.Add(this.label29);
		this.panel1.Controls.Add(this.label30);
		this.panel1.Controls.Add(this.lbMDFeEvent);
		this.panel1.Controls.Add(this.lbMDFeDocInb);
		this.panel1.Controls.Add(this.label22);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.label24);
		this.panel1.Controls.Add(this.label25);
		this.panel1.Controls.Add(this.label26);
		this.panel1.Controls.Add(this.label27);
		this.panel1.Controls.Add(this.label19);
		this.panel1.Controls.Add(this.lbDocScanTitle);
		this.panel1.Controls.Add(this.lbCTeEvent);
		this.panel1.Controls.Add(this.lbCTeDocOther);
		this.panel1.Controls.Add(this.lbCTeDocToma);
		this.panel1.Controls.Add(this.lbNFeEventOut);
		this.panel1.Controls.Add(this.lbNFeEventInb);
		this.panel1.Controls.Add(this.lbNFeDocInb);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label14);
		this.panel1.Controls.Add(this.label15);
		this.panel1.Controls.Add(this.label17);
		this.panel1.Controls.Add(this.label13);
		this.panel1.Controls.Add(this.label12);
		this.panel1.Controls.Add(this.label11);
		this.panel1.Controls.Add(this.label10);
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.label16);
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.lbNotasFiscais);
		this.panel1.Controls.Add(this.lbLine01);
		this.panel1.Controls.Add(this.btSair);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(801, 523);
		this.panel1.TabIndex = 59;
		this.label19.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label19.Location = new System.Drawing.Point(14, 8);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(773, 2);
		this.label19.TabIndex = 87;
		this.lbDocScanTitle.AutoSize = true;
		this.lbDocScanTitle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDocScanTitle.Location = new System.Drawing.Point(17, 19);
		this.lbDocScanTitle.Name = "lbDocScanTitle";
		this.lbDocScanTitle.Size = new System.Drawing.Size(200, 14);
		this.lbDocScanTitle.TabIndex = 86;
		this.lbDocScanTitle.Text = "Resumo da busca na SEFAZ : ";
		this.lbCTeEvent.AutoSize = true;
		this.lbCTeEvent.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbCTeEvent.ForeColor = System.Drawing.Color.Red;
		this.lbCTeEvent.Location = new System.Drawing.Point(189, 352);
		this.lbCTeEvent.Name = "lbCTeEvent";
		this.lbCTeEvent.Size = new System.Drawing.Size(25, 14);
		this.lbCTeEvent.TabIndex = 84;
		this.lbCTeEvent.Text = "35";
		this.lbCTeDocOther.AutoSize = true;
		this.lbCTeDocOther.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbCTeDocOther.ForeColor = System.Drawing.Color.Red;
		this.lbCTeDocOther.Location = new System.Drawing.Point(449, 310);
		this.lbCTeDocOther.Name = "lbCTeDocOther";
		this.lbCTeDocOther.Size = new System.Drawing.Size(25, 14);
		this.lbCTeDocOther.TabIndex = 83;
		this.lbCTeDocOther.Text = "35";
		this.lbCTeDocToma.AutoSize = true;
		this.lbCTeDocToma.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbCTeDocToma.ForeColor = System.Drawing.Color.Red;
		this.lbCTeDocToma.Location = new System.Drawing.Point(342, 267);
		this.lbCTeDocToma.Name = "lbCTeDocToma";
		this.lbCTeDocToma.Size = new System.Drawing.Size(25, 14);
		this.lbCTeDocToma.TabIndex = 82;
		this.lbCTeDocToma.Text = "35";
		this.lbNFeEventOut.AutoSize = true;
		this.lbNFeEventOut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNFeEventOut.ForeColor = System.Drawing.Color.Red;
		this.lbNFeEventOut.Location = new System.Drawing.Point(275, 136);
		this.lbNFeEventOut.Name = "lbNFeEventOut";
		this.lbNFeEventOut.Size = new System.Drawing.Size(25, 14);
		this.lbNFeEventOut.TabIndex = 81;
		this.lbNFeEventOut.Text = "35";
		this.lbNFeEventInb.AutoSize = true;
		this.lbNFeEventInb.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNFeEventInb.ForeColor = System.Drawing.Color.Red;
		this.lbNFeEventInb.Location = new System.Drawing.Point(288, 92);
		this.lbNFeEventInb.Name = "lbNFeEventInb";
		this.lbNFeEventInb.Size = new System.Drawing.Size(25, 14);
		this.lbNFeEventInb.TabIndex = 80;
		this.lbNFeEventInb.Text = "35";
		this.lbNFeDocInb.AutoSize = true;
		this.lbNFeDocInb.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNFeDocInb.ForeColor = System.Drawing.Color.Red;
		this.lbNFeDocInb.Location = new System.Drawing.Point(250, 50);
		this.lbNFeDocInb.Name = "lbNFeDocInb";
		this.lbNFeDocInb.Size = new System.Drawing.Size(25, 14);
		this.lbNFeDocInb.TabIndex = 79;
		this.lbNFeDocInb.Text = "35";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label5.Location = new System.Drawing.Point(17, 370);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(557, 13);
		this.label5.TabIndex = 78;
		this.label5.Text = "Total de Eventos como : Cancelamento, MDF-e vinculado a CTe, Passagem em Posto Fiscal, etc.";
		this.label14.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label14.Location = new System.Drawing.Point(14, 346);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(773, 2);
		this.label14.TabIndex = 77;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label15.Location = new System.Drawing.Point(17, 328);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(541, 13);
		this.label15.TabIndex = 76;
		this.label15.Text = "Total de Conhecimentos de Transporte que tem o seu CNPJ envolvido no processo comercial.";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(17, 310);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(427, 14);
		this.label17.TabIndex = 75;
		this.label17.Text = "CTE's emitidos contra o seu CNPJ com ação diferente de Tomador :";
		this.label13.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label13.Location = new System.Drawing.Point(14, 304);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(773, 2);
		this.label13.TabIndex = 74;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label12.Location = new System.Drawing.Point(17, 285);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(566, 13);
		this.label12.TabIndex = 73;
		this.label12.Text = "Total de Conhecimentos de Transporte que tem o seu CNPJ destacado como Tomador do Serviço.";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label11.Location = new System.Drawing.Point(17, 154);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(636, 13);
		this.label11.TabIndex = 72;
		this.label11.Text = "Total de Eventos como : Manifestação do Destinatário realizada pelos seus clientes, Registro na Suframa, etc.";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label10.Location = new System.Drawing.Point(17, 110);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(675, 13);
		this.label10.TabIndex = 71;
		this.label10.Text = "Total de Eventos de como : Carta de Correção, Cancelamento, CTe vinculado a NFe, Passagem em Posto Fiscal, etc.";
		this.label9.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label9.Location = new System.Drawing.Point(14, 86);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(773, 2);
		this.label9.TabIndex = 70;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label16.Location = new System.Drawing.Point(17, 68);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(391, 13);
		this.label16.TabIndex = 69;
		this.label16.Text = "Total de Notas Fiscais emitidas contra o seu CNPJ em todo o Brasil.";
		this.label8.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label8.Location = new System.Drawing.Point(14, 173);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(773, 2);
		this.label8.TabIndex = 68;
		this.label7.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label7.Location = new System.Drawing.Point(14, 130);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(773, 2);
		this.label7.TabIndex = 67;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(17, 352);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(168, 14);
		this.label6.TabIndex = 66;
		this.label6.Text = "Eventos sobre os CTE's : ";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(17, 267);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(320, 14);
		this.label4.TabIndex = 65;
		this.label4.Text = "CTE's emitidos contra o seu CNPJ como Tomador :";
		this.label3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label3.Location = new System.Drawing.Point(14, 44);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(773, 2);
		this.label3.TabIndex = 64;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(17, 136);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(252, 14);
		this.label2.TabIndex = 63;
		this.label2.Text = "Eventos sobre Notas Fiscais de Saída :";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(17, 92);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(267, 14);
		this.label1.TabIndex = 62;
		this.label1.Text = "Eventos sobre Notas Fiscais de Entrada :";
		this.lbNotasFiscais.AutoSize = true;
		this.lbNotasFiscais.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNotasFiscais.Location = new System.Drawing.Point(17, 50);
		this.lbNotasFiscais.Name = "lbNotasFiscais";
		this.lbNotasFiscais.Size = new System.Drawing.Size(226, 14);
		this.lbNotasFiscais.TabIndex = 61;
		this.lbNotasFiscais.Text = "NFes emitidas contra o seu CNPJ : ";
		this.btSair.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btSair.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btSair.Location = new System.Drawing.Point(703, 482);
		this.btSair.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btSair.Name = "btSair";
		this.btSair.Size = new System.Drawing.Size(84, 31);
		this.btSair.TabIndex = 59;
		this.btSair.Text = "&Sair";
		this.btSair.UseVisualStyleBackColor = true;
		this.btSair.Click += new System.EventHandler(btSair_Click);
		this.lbLine01.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbLine01.Location = new System.Drawing.Point(14, 389);
		this.lbLine01.Name = "lbLine01";
		this.lbLine01.Size = new System.Drawing.Size(773, 2);
		this.lbLine01.TabIndex = 60;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(16, 395);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(276, 14);
		this.label27.TabIndex = 89;
		this.label27.Text = "MDFe's emitidos com citação ao seu CNPJ :";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(16, 437);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(178, 14);
		this.label26.TabIndex = 90;
		this.label26.Text = "Eventos sobre os MDFe's : ";
		this.label25.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label25.Location = new System.Drawing.Point(13, 473);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(773, 2);
		this.label25.TabIndex = 91;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label24.Location = new System.Drawing.Point(16, 413);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(483, 13);
		this.label24.TabIndex = 92;
		this.label24.Text = "Total de Manifestos Eletrônicos emitidos com citação ao seu CNPJ em todo o Brasil.";
		this.label23.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label23.Location = new System.Drawing.Point(13, 431);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(773, 2);
		this.label23.TabIndex = 93;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label22.Location = new System.Drawing.Point(16, 455);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(582, 13);
		this.label22.TabIndex = 94;
		this.label22.Text = "Total de Eventos de como :  Inclusão de Condutor, Encerramento de Transporte, Cancelamento, etc.";
		this.lbMDFeDocInb.AutoSize = true;
		this.lbMDFeDocInb.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbMDFeDocInb.ForeColor = System.Drawing.Color.Red;
		this.lbMDFeDocInb.Location = new System.Drawing.Point(299, 395);
		this.lbMDFeDocInb.Name = "lbMDFeDocInb";
		this.lbMDFeDocInb.Size = new System.Drawing.Size(25, 14);
		this.lbMDFeDocInb.TabIndex = 95;
		this.lbMDFeDocInb.Text = "35";
		this.lbMDFeEvent.AutoSize = true;
		this.lbMDFeEvent.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbMDFeEvent.ForeColor = System.Drawing.Color.Red;
		this.lbMDFeEvent.Location = new System.Drawing.Point(200, 437);
		this.lbMDFeEvent.Name = "lbMDFeEvent";
		this.lbMDFeEvent.Size = new System.Drawing.Size(25, 14);
		this.lbMDFeEvent.TabIndex = 96;
		this.lbMDFeEvent.Text = "35";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label18.Location = new System.Drawing.Point(17, 239);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(318, 13);
		this.label18.TabIndex = 102;
		this.label18.Text = "Total de Eventos de como : Confirmação, Recusa, etc.";
		this.label20.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label20.Location = new System.Drawing.Point(14, 215);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(773, 2);
		this.label20.TabIndex = 101;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.CornflowerBlue;
		this.label21.Location = new System.Drawing.Point(17, 197);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(561, 13);
		this.label21.TabIndex = 100;
		this.label21.Text = "Total de Notas Fiscais de Serviço contra o seu CNPJ em todo o Brasil via Portal da NFSe Nacional";
		this.label28.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label28.Location = new System.Drawing.Point(13, 259);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(773, 2);
		this.label28.TabIndex = 99;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(17, 221);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(262, 14);
		this.label29.TabIndex = 98;
		this.label29.Text = "Eventos sobre Notas Fiscais de Serviço :";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(17, 179);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(234, 14);
		this.label30.TabIndex = 97;
		this.label30.Text = "NFSes emitidas contra o seu CNPJ : ";
		this.lbNFSeDocInb.AutoSize = true;
		this.lbNFSeDocInb.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNFSeDocInb.ForeColor = System.Drawing.Color.Red;
		this.lbNFSeDocInb.Location = new System.Drawing.Point(255, 179);
		this.lbNFSeDocInb.Name = "lbNFSeDocInb";
		this.lbNFSeDocInb.Size = new System.Drawing.Size(25, 14);
		this.lbNFSeDocInb.TabIndex = 103;
		this.lbNFSeDocInb.Text = "35";
		this.lbNFSeEventInb.AutoSize = true;
		this.lbNFSeEventInb.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbNFSeEventInb.ForeColor = System.Drawing.Color.Red;
		this.lbNFSeEventInb.Location = new System.Drawing.Point(285, 221);
		this.lbNFSeEventInb.Name = "lbNFSeEventInb";
		this.lbNFSeEventInb.Size = new System.Drawing.Size(25, 14);
		this.lbNFSeEventInb.TabIndex = 104;
		this.lbNFSeEventInb.Text = "35";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(801, 523);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmStatusScan";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Status de Busca dos Documentos na SEFAZ";
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStatusScan_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
