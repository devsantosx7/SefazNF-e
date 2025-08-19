using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using srv.fiscal.io;

namespace Monitor;

public class frmOnboardStep01 : Form
{
	private IContainer components;

	private Panel panel1;

	private ListView lstCompanies;

	private ColumnHeader clID;

	private ColumnHeader clName;

	private ToolStrip toolbarFiliais;

	private ToolStripButton tsbAddFilial;

	private ToolStripButton tsbEdtFilial;

	private ToolStripButton tsbExcFilial;

	private ToolStripSeparator toolStripSeparator24;

	private ColumnHeader clCertType;

	private ImageList imageList01;

	private Panel panel2;

	private Label label8;

	private Label label19;

	private Label label20;

	private Label label3;

	private Label label2;

	private ToolStripButton toolStripButton1;

	private FlowLayoutPanel flowLayoutPanel1;

	public frmOnboardStep01()
	{
		InitializeComponent();
	}

	private void frmOnboardStep01_Load(object sender, EventArgs e)
	{
		funcLoadCompaniesAsync();
	}

	private async void funcLoadCompaniesAsync()
	{
		lstCompanies.Items.Clear();
		foreach (FilialView varFilial in await new clsDataFilial().funcGetListAsync(pLoadDummy: false))
		{
			ListViewItem varItem = new ListViewItem(varFilial.CNPJ);
			varItem.Tag = varFilial;
			varItem.SubItems.Add(varFilial.Nome);
			varItem.SubItems.Add(varFilial.CertifType);
			varItem.ImageIndex = 0;
			lstCompanies.Items.Add(varItem);
		}
	}

	private void tsbAddFilial_Click(object sender, EventArgs e)
	{
		new frmFilial(null).ShowDialog(this);
		funcLoadCompaniesAsync();
	}

	private void tsbEdtFilial_Click(object sender, EventArgs e)
	{
		foreach (ListViewItem selectedItem in lstCompanies.SelectedItems)
		{
			FilialView varclsFilial = (FilialView)selectedItem.Tag;
			if (varclsFilial != null)
			{
				string varFilialDummnyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
				if (varclsFilial.CNPJ.Equals(varFilialDummnyIdnt))
				{
					MessageBox.Show(string.Concat("Empresa criada automaticamente Fiscal.io Monitor." + Environment.NewLine + Environment.NewLine, "Ela não pode ser alterada manualmente !", Environment.NewLine), "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				new frmFilial(varclsFilial).ShowDialog(this);
			}
		}
		funcLoadCompaniesAsync();
	}

	private async void tsbExcFilial_Click(object sender, EventArgs e)
	{
		foreach (ListViewItem varItem in lstCompanies.SelectedItems)
		{
			FilialView varclsFilial = (FilialView)varItem.Tag;
			if (varclsFilial != null)
			{
				string varFilialDummnyIdnt = clsDataGeral.funcGetFilialDummnyIdnt();
				if (varclsFilial.CNPJ.Equals(varFilialDummnyIdnt))
				{
					MessageBox.Show(string.Concat("Empresa criada automaticamente Fiscal.io Monitor." + Environment.NewLine + Environment.NewLine, "Ela não pode ser excluída manualmente !", Environment.NewLine), "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (MessageBox.Show("Deseja excluir a empresa [ " + varclsFilial.NomeView + " - " + varclsFilial.CNPJView + " ] ?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				await new clsDataFilial().funcDeleteAsync(varclsFilial);
				await new clsDataTaskAction().funcDeleteByFilialAsync(varclsFilial.CNPJ);
			}
		}
		funcLoadCompaniesAsync();
	}

	public async Task<bool> funcValidateAsync()
	{
		string varMensagem = string.Empty;
		if ((await new clsDataFilial().funcGetListAsync(pLoadDummy: false)).Count == 0 && lstCompanies.Items.Count == 0)
		{
			varMensagem = varMensagem + Environment.NewLine + "--> Nenhuma empresa foi informada !!!";
		}
		if (!string.IsNullOrEmpty(varMensagem))
		{
			MessageBox.Show("Confirme os dados antes de continuar" + varMensagem, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return false;
		}
		return true;
	}

	private void btHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallOnboardHelp();
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmOnboardStep01));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.toolbarFiliais = new System.Windows.Forms.ToolStrip();
		this.tsbAddFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbEdtFilial = new System.Windows.Forms.ToolStripButton();
		this.tsbExcFilial = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.lstCompanies = new System.Windows.Forms.ListView();
		this.clID = new System.Windows.Forms.ColumnHeader();
		this.clName = new System.Windows.Forms.ColumnHeader();
		this.clCertType = new System.Windows.Forms.ColumnHeader();
		this.imageList01 = new System.Windows.Forms.ImageList(this.components);
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolbarFiliais.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(785, 485);
		this.panel1.TabIndex = 83;
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.toolbarFiliais);
		this.panel2.Controls.Add(this.lstCompanies);
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.label20);
		this.panel2.Controls.Add(this.flowLayoutPanel1);
		this.panel2.Location = new System.Drawing.Point(62, 41);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(653, 387);
		this.panel2.TabIndex = 199;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(22, 354);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(613, 15);
		this.label3.TabIndex = 198;
		this.label3.Text = "Os certificados digitais estarão seguros em seu computador.";
		this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label2.Location = new System.Drawing.Point(3, 60);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(647, 15);
		this.label2.TabIndex = 197;
		this.label2.Text = "É necessário utilizar o certificado eCNPJ ou eCPF para que o Fiscal.io Monitor comunique com a SEFAZ.";
		this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label8.Location = new System.Drawing.Point(59, 17);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(541, 23);
		this.label8.TabIndex = 186;
		this.label8.Text = "Cadastre os CNPjs/CPFs que serão utilizados no Fiscal.io Monitor:";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.toolbarFiliais.Dock = System.Windows.Forms.DockStyle.None;
		this.toolbarFiliais.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolbarFiliais.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsbAddFilial, this.tsbEdtFilial, this.tsbExcFilial, this.toolStripSeparator24, this.toolStripButton1 });
		this.toolbarFiliais.Location = new System.Drawing.Point(31, 95);
		this.toolbarFiliais.Name = "toolbarFiliais";
		this.toolbarFiliais.Padding = new System.Windows.Forms.Padding(0, 4, 1, 4);
		this.toolbarFiliais.Size = new System.Drawing.Size(293, 37);
		this.toolbarFiliais.TabIndex = 106;
		this.tsbAddFilial.Image = Monitor.Resources.image_add_object;
		this.tsbAddFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbAddFilial.Name = "tsbAddFilial";
		this.tsbAddFilial.Size = new System.Drawing.Size(76, 26);
		this.tsbAddFilial.Text = "Adicionar";
		this.tsbAddFilial.Click += new System.EventHandler(tsbAddFilial_Click);
		this.tsbEdtFilial.Image = Monitor.Resources.image_edit_object;
		this.tsbEdtFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbEdtFilial.Name = "tsbEdtFilial";
		this.tsbEdtFilial.Size = new System.Drawing.Size(58, 26);
		this.tsbEdtFilial.Text = "Editar";
		this.tsbEdtFilial.Click += new System.EventHandler(tsbEdtFilial_Click);
		this.tsbExcFilial.Image = Monitor.Resources.image_delete_object;
		this.tsbExcFilial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExcFilial.Name = "tsbExcFilial";
		this.tsbExcFilial.Size = new System.Drawing.Size(61, 26);
		this.tsbExcFilial.Text = "Excluir";
		this.tsbExcFilial.Click += new System.EventHandler(tsbExcFilial_Click);
		this.toolStripSeparator24.Name = "toolStripSeparator24";
		this.toolStripSeparator24.Size = new System.Drawing.Size(6, 29);
		this.toolStripButton1.BackColor = System.Drawing.Color.Transparent;
		this.toolStripButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Font = new System.Drawing.Font("Tahoma", 9f);
		this.toolStripButton1.ForeColor = System.Drawing.Color.White;
		this.toolStripButton1.Image = Monitor.Resources.image_form_help;
		this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
		this.toolStripButton1.Size = new System.Drawing.Size(80, 26);
		this.toolStripButton1.Click += new System.EventHandler(btHelp_Click);
		this.lstCompanies.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.lstCompanies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.clID, this.clName, this.clCertType });
		this.lstCompanies.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lstCompanies.FullRowSelect = true;
		this.lstCompanies.HideSelection = false;
		this.lstCompanies.Location = new System.Drawing.Point(29, 132);
		this.lstCompanies.MultiSelect = false;
		this.lstCompanies.Name = "lstCompanies";
		this.lstCompanies.Size = new System.Drawing.Size(599, 206);
		this.lstCompanies.SmallImageList = this.imageList01;
		this.lstCompanies.TabIndex = 105;
		this.lstCompanies.UseCompatibleStateImageBehavior = false;
		this.lstCompanies.View = System.Windows.Forms.View.Details;
		this.clID.Text = "CNPJ/CPF";
		this.clID.Width = 140;
		this.clName.Text = "Nome";
		this.clName.Width = 368;
		this.clCertType.Text = "Certificado";
		this.clCertType.Width = 81;
		this.imageList01.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList01.ImageStream");
		this.imageList01.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList01.Images.SetKeyName(0, "company-icon-300.png");
		this.label19.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label19.BackColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label19.Location = new System.Drawing.Point(51, 17);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(2, 23);
		this.label19.TabIndex = 187;
		this.label20.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label20.Font = new System.Drawing.Font("Tahoma", 15.75f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.FromArgb(14, 74, 204);
		this.label20.Location = new System.Drawing.Point(24, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(21, 23);
		this.label20.TabIndex = 185;
		this.label20.Text = "3";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(29, 98);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(599, 34);
		this.flowLayoutPanel1.TabIndex = 199;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(785, 485);
		base.Controls.Add(this.panel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnboardStep01";
		this.Text = "frmOnboardStep01";
		base.Load += new System.EventHandler(frmOnboardStep01_Load);
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolbarFiliais.ResumeLayout(false);
		this.toolbarFiliais.PerformLayout();
		base.ResumeLayout(false);
	}
}
