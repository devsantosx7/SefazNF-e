using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmDocNote : Form
{
	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private List<Document> varDocList = new List<Document>();

	private bool _SelectedAllItens;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private IContainer components;

	private SplitContainer splitList;

	private FolderBrowserDialog folderDialog;

	private ToolStrip toolStrip1;

	private ToolStripButton tsbAddNote;

	private ListView lstDocs;

	private ColumnHeader clDoc;

	private ColumnHeader clSerie;

	private ColumnHeader clDtAut;

	private ColumnHeader clEmissor;

	private ColumnHeader clTomador;

	private Panel pnEmailData;

	private TextBox txDocNote;

	private ToolStripSeparator toolStripSeparator3;

	private ColumnHeader clHasXml;

	private Label label1;

	private Label lbDocNote;

	private ColumnHeader clDocNote;

	private ColumnHeader DocNoteDtHr;

	private ColumnHeader DocNoteUser;

	private ImageList ImageListDocs;

	public frmDocNote(ref List<Document> pDocList)
	{
		InitializeComponent();
		varDocList = pDocList;
	}

	private async void frmDocNote_Load(object sender, EventArgs e)
	{
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		funcLoadDocsAsync();
		lstDocs.Columns[0].ImageIndex = 16;
	}

	public async void funcLoadDocsAsync()
	{
		clsValidatorService varclsValidator = new clsValidatorService();
		new List<Event>();
		lstDocs.Items.Clear();
		new ListViewItem();
		foreach (Document varclsDoc in varDocList)
		{
			ListViewItem listViewItem = lstDocs.Items.Add("");
			listViewItem.ImageIndex = clsScreenGeral.funcGetDocXmlIcon(varclsDoc, _HasNFSeFeatEnabled, _HasCFeFeatEnabled);
			listViewItem.Tag = varclsDoc;
			listViewItem.ToolTipText = varclsValidator.funcGetDesc(varclsDoc);
			listViewItem.SubItems.Add(varclsDoc.Num.PadLeft(9, '0'));
			listViewItem.SubItems.Add(varclsDoc.Serie);
			listViewItem.ForeColor = Color.Blue;
			listViewItem.SubItems.Add(varclsDoc.DtAut);
			listViewItem.SubItems.Add(varclsDoc.EmitNome);
			listViewItem.SubItems.Add(varclsDoc.TomaNome);
			listViewItem.SubItems.Add(varclsDoc.DocNote);
			listViewItem.SubItems.Add(varclsDoc.DocNoteDtHr);
			listViewItem.SubItems.Add(varclsDoc.DocNoteUser);
			listViewItem.Checked = true;
		}
	}

	private void lstDocs_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		foreach (ListViewItem varItem in ((ListView)sender).Items)
		{
			if (varItem.Checked)
			{
				varItem.BackColor = Color.LightBlue;
			}
			else
			{
				varItem.BackColor = Color.White;
			}
		}
	}

	private void lstDocs_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		if (e.Column == 0)
		{
			funcSelectAllItens();
		}
	}

	private void funcSelectAllItens()
	{
		foreach (ListViewItem varListItem in lstDocs.Items)
		{
			if (_SelectedAllItens)
			{
				varListItem.Checked = false;
			}
			else
			{
				varListItem.Checked = true;
			}
		}
		if (_SelectedAllItens)
		{
			_SelectedAllItens = false;
		}
		else
		{
			_SelectedAllItens = true;
		}
	}

	private async void tsbAddNote_Click(object sender, EventArgs e)
	{
		clsDataDoc varclsDataDoc = new clsDataDoc();
		bool varclsHasDocNote = false;
		foreach (ListViewItem checkedItem in lstDocs.CheckedItems)
		{
			Document varclsDoc = (Document)checkedItem.Tag;
			if (varclsDoc != null && !clsFunction.IsEmpty(varclsDoc.DocNote))
			{
				varclsHasDocNote = true;
				break;
			}
		}
		if (clsFunction.IsEmpty(txDocNote.Text) && !varclsHasDocNote)
		{
			MessageBox.Show("Nenhum comentário foi informado.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			if (clsFunction.IsEmpty(txDocNote.Text) && varclsHasDocNote && MessageBox.Show("Deseja remover o(s) comentário(s) dos documentos selecionados?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (lstDocs.CheckedItems.Count <= 0)
			{
				MessageBox.Show("Nenhum documento foi selecionado.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				if (varclsHasDocNote && MessageBox.Show("Um ou mais documentos já tem comentários atribuidos." + Environment.NewLine + Environment.NewLine + "Mesmo assim, deseja continuar?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				foreach (ListViewItem varclsItem in lstDocs.CheckedItems)
				{
					Document varclsDoc2 = (Document)varclsItem.Tag;
					if (varclsDoc2 != null)
					{
						varclsDataDoc.funcSetBuffer(varclsDoc2);
						varclsDoc2.DocNote = txDocNote.Text;
						varclsDoc2 = await varclsDataDoc.funcSetUserAndDateDocNoteAsync(varclsDoc2);
						await varclsDataDoc.funcUpdateAsync(varclsDoc2);
						varclsItem.SubItems[6].Text = varclsDoc2.DocNote;
						varclsItem.SubItems[7].Text = varclsDoc2.DocNoteDtHr;
						varclsItem.SubItems[8].Text = varclsDoc2.DocNoteUser;
						varclsItem.Focused = true;
						varclsItem.EnsureVisible();
					}
				}
				MessageBox.Show("Atribuição realizada com sucesso.", "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Close();
			}
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmDocNote));
		this.splitList = new System.Windows.Forms.SplitContainer();
		this.pnEmailData = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.lbDocNote = new System.Windows.Forms.Label();
		this.txDocNote = new System.Windows.Forms.TextBox();
		this.lstDocs = new System.Windows.Forms.ListView();
		this.clHasXml = new System.Windows.Forms.ColumnHeader();
		this.clDoc = new System.Windows.Forms.ColumnHeader();
		this.clSerie = new System.Windows.Forms.ColumnHeader();
		this.clDtAut = new System.Windows.Forms.ColumnHeader();
		this.clEmissor = new System.Windows.Forms.ColumnHeader();
		this.clTomador = new System.Windows.Forms.ColumnHeader();
		this.clDocNote = new System.Windows.Forms.ColumnHeader();
		this.DocNoteDtHr = new System.Windows.Forms.ColumnHeader();
		this.DocNoteUser = new System.Windows.Forms.ColumnHeader();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.tsbAddNote = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
		((System.ComponentModel.ISupportInitialize)this.splitList).BeginInit();
		this.splitList.Panel1.SuspendLayout();
		this.splitList.Panel2.SuspendLayout();
		this.splitList.SuspendLayout();
		this.pnEmailData.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.splitList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitList.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitList.Location = new System.Drawing.Point(0, 0);
		this.splitList.Name = "splitList";
		this.splitList.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitList.Panel1.Controls.Add(this.pnEmailData);
		this.splitList.Panel1MinSize = 85;
		this.splitList.Panel2.Controls.Add(this.lstDocs);
		this.splitList.Panel2.Controls.Add(this.toolStrip1);
		this.splitList.Size = new System.Drawing.Size(742, 445);
		this.splitList.SplitterDistance = 85;
		this.splitList.TabIndex = 2;
		this.pnEmailData.Controls.Add(this.label1);
		this.pnEmailData.Controls.Add(this.lbDocNote);
		this.pnEmailData.Controls.Add(this.txDocNote);
		this.pnEmailData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnEmailData.Location = new System.Drawing.Point(0, 0);
		this.pnEmailData.Name = "pnEmailData";
		this.pnEmailData.Size = new System.Drawing.Size(742, 85);
		this.pnEmailData.TabIndex = 101;
		this.label1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label1.Location = new System.Drawing.Point(9, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(721, 1);
		this.label1.TabIndex = 125;
		this.lbDocNote.AutoSize = true;
		this.lbDocNote.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbDocNote.Location = new System.Drawing.Point(5, 6);
		this.lbDocNote.Name = "lbDocNote";
		this.lbDocNote.Size = new System.Drawing.Size(405, 14);
		this.lbDocNote.TabIndex = 123;
		this.lbDocNote.Text = "Informe um comentário que deseja atribuir ao(s) documento(s)";
		this.lbDocNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txDocNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txDocNote.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txDocNote.Location = new System.Drawing.Point(7, 29);
		this.txDocNote.MaxLength = 1000;
		this.txDocNote.Multiline = true;
		this.txDocNote.Name = "txDocNote";
		this.txDocNote.Size = new System.Drawing.Size(727, 54);
		this.txDocNote.TabIndex = 1;
		this.lstDocs.CheckBoxes = true;
		this.lstDocs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[9] { this.clHasXml, this.clDoc, this.clSerie, this.clDtAut, this.clEmissor, this.clTomador, this.clDocNote, this.DocNoteDtHr, this.DocNoteUser });
		this.lstDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lstDocs.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lstDocs.FullRowSelect = true;
		this.lstDocs.HideSelection = false;
		this.lstDocs.Location = new System.Drawing.Point(0, 27);
		this.lstDocs.MultiSelect = false;
		this.lstDocs.Name = "lstDocs";
		this.lstDocs.ShowItemToolTips = true;
		this.lstDocs.Size = new System.Drawing.Size(742, 329);
		this.lstDocs.SmallImageList = this.ImageListDocs;
		this.lstDocs.TabIndex = 6;
		this.lstDocs.UseCompatibleStateImageBehavior = false;
		this.lstDocs.View = System.Windows.Forms.View.Details;
		this.lstDocs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lstDocs_ColumnClick);
		this.lstDocs.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lstDocs_ItemChecked);
		this.clHasXml.Text = "";
		this.clHasXml.Width = 39;
		this.clDoc.Text = "Doc";
		this.clDoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDoc.Width = 78;
		this.clSerie.Text = "Serie";
		this.clSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clSerie.Width = 42;
		this.clDtAut.Text = "Data";
		this.clDtAut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDtAut.Width = 80;
		this.clEmissor.Text = "Emissor";
		this.clEmissor.Width = 92;
		this.clTomador.Text = "Tomador";
		this.clTomador.Width = 113;
		this.clDocNote.Text = "Comentários";
		this.clDocNote.Width = 150;
		this.DocNoteDtHr.Text = "Data";
		this.DocNoteUser.Text = "Usuário";
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_xml_green.png");
		this.ImageListDocs.Images.SetKeyName(1, "image_xml_brown.png");
		this.ImageListDocs.Images.SetKeyName(2, "image_xml_red.png");
		this.ImageListDocs.Images.SetKeyName(3, "dfe_approved.png");
		this.ImageListDocs.Images.SetKeyName(4, "dfe_canceled.png");
		this.ImageListDocs.Images.SetKeyName(5, "dfe_transport.png");
		this.ImageListDocs.Images.SetKeyName(6, "dfe_mdfedoc.png");
		this.ImageListDocs.Images.SetKeyName(7, "dfe_acknow.png");
		this.ImageListDocs.Images.SetKeyName(8, "dfe_confirm.png");
		this.ImageListDocs.Images.SetKeyName(9, "dfe_disagree.png");
		this.ImageListDocs.Images.SetKeyName(10, "dfe_unknow.png");
		this.ImageListDocs.Images.SetKeyName(11, "image_zfmvist.png");
		this.ImageListDocs.Images.SetKeyName(12, "image_zfminte.png");
		this.ImageListDocs.Images.SetKeyName(13, "image_lancfiscal.png");
		this.ImageListDocs.Images.SetKeyName(14, "image_notes.png");
		this.ImageListDocs.Images.SetKeyName(15, "image_locker_16_16.png");
		this.ImageListDocs.Images.SetKeyName(16, "image_select_all.jpg");
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsbAddNote, this.toolStripSeparator3 });
		this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(742, 27);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.tsbAddNote.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsbAddNote.Image = Monitor.Resources.image_notes;
		this.tsbAddNote.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbAddNote.Name = "tsbAddNote";
		this.tsbAddNote.Size = new System.Drawing.Size(150, 24);
		this.tsbAddNote.Text = "Atribuir comentário";
		this.tsbAddNote.Click += new System.EventHandler(tsbAddNote_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.folderDialog.RootFolder = System.Environment.SpecialFolder.DesktopDirectory;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(742, 445);
		base.Controls.Add(this.splitList);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmDocNote";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Comentar Documentos";
		base.Load += new System.EventHandler(frmDocNote_Load);
		this.splitList.Panel1.ResumeLayout(false);
		this.splitList.Panel2.ResumeLayout(false);
		this.splitList.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitList).EndInit();
		this.splitList.ResumeLayout(false);
		this.pnEmailData.ResumeLayout(false);
		this.pnEmailData.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
