using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmTabPartner : Form
{
	private class ObjDoc
	{
		public string FilId { get; set; }

		public string DocId { get; set; }
	}

	private clsSrvTabPartner _SqlTabData = new clsSrvTabPartner();

	private Hashtable _HasColors = new Hashtable();

	private List<Estado> _StateList = new List<Estado>();

	private clsDFeCodes _clsDFeCodes = new clsDFeCodes();

	private clsCadCodes varclsCadCodes = new clsCadCodes();

	private bool _SelectedAllItens;

	private bool _ColumnsLoaded;

	private Color _ColumnColor;

	private string _FeatExtId = string.Empty;

	private bool _IsLocked;

	private IContainer components;

	private Panel pnContent;

	private ListView lsvTabPartner;

	private ColumnHeader clID;

	private ColumnHeader clName;

	private ColumnHeader clInscEst;

	private ColumnHeader clCodCnae;

	private ColumnHeader clRegApur;

	private ColumnHeader clEmail;

	private ColumnHeader clNmFant;

	private ColumnHeader clLograd;

	private ColumnHeader clNumero;

	private ColumnHeader clComplt;

	private ColumnHeader clBairro;

	private ColumnHeader clCodMun;

	private ColumnHeader clDesMun;

	private ColumnHeader clCodCep;

	private ColumnHeader clCodUf;

	private ColumnHeader clFoneNum;

	private ColumnHeader clCodISuf;

	private ColumnHeader clCodIMun;

	private ColumnHeader cldIniAtiv;

	private ColumnHeader cldUltSit;

	private ColumnHeader cldBaixa;

	private ColumnHeader clIEUnica;

	private ColumnHeader clIEAtual;

	private ColumnHeader clindCredNFe;

	private ColumnHeader clindCredCTe;

	private ColumnHeader clCodSit;

	private ColumnHeader clDatCons;

	private ColumnHeader clHorCons;

	private ColumnHeader clSrcCert;

	private ColumnHeader clSrcDFe;

	private ColumnHeader clSrcCons;

	private ColumnHeader clEmailCert;

	private ColumnHeader clIndex;

	private ProgressBar tlpProgress;

	private ImageList ImageListDocs;

	public bool funcIsLocked()
	{
		return _IsLocked;
	}

	public frmTabPartner(Color pColumnColor, string pFeatExtId)
	{
		InitializeComponent();
		_ColumnColor = pColumnColor;
		_FeatExtId = pFeatExtId;
	}

	private async void frmTabPartner_Load(object sender, EventArgs e)
	{
		_StateList = await new clsDataEstado().funcGetListAsync(pWthAN: false);
		_ColumnsLoaded = false;
		clsFunction.funcSetColumnsConfiguration(this, lsvTabPartner);
		_ColumnsLoaded = true;
		lsvTabPartner.Columns[0].ImageIndex = 16;
	}

	public async Task<bool> funcLoadTagsAsync()
	{
		return true;
	}

	public async Task<clsReturn> funcLoadDataAsync(clsDataFilter pclsDataFilter)
	{
		clsReturn varclsReturn = new clsReturn();
		string varSqlQuery = await _SqlTabData.funcGetSqlStrSelectAsync(pclsDataFilter);
		if (string.IsNullOrEmpty(varSqlQuery))
		{
			return varclsReturn;
		}
		long varPageSize = clsFunction.funcConvStrToLong(pclsDataFilter.PageSize);
		List<Partner> varclsPartnerList = await new clsDataPartner().funcGetListByFullSqlAsync(varSqlQuery, varPageSize);
		_ = varclsPartnerList.Count;
		return await funcShowDataAsync(pclsDataFilter, varclsPartnerList);
	}

	private async Task<clsReturn> funcShowDataAsync(clsDataFilter pclsFilter, List<Partner> pPartnerList)
	{
		_ = string.Empty;
		_ = string.Empty;
		clsReturn varclsReturn = new clsReturn();
		tlpProgress.Visible = true;
		tlpProgress.Minimum = 0;
		tlpProgress.Maximum = pPartnerList.Count;
		tlpProgress.Value = 0;
		Application.DoEvents();
		lsvTabPartner.BeginUpdate();
		lsvTabPartner.SuspendLayout();
		lsvTabPartner.Items.Clear();
		int varCounter = 0;
		string varGroupByText = "FISCAL_IO_STARTER";
		ListViewGroup varListGroup = new ListViewGroup();
		if (pclsFilter.GroupByDisable || _IsLocked)
		{
			lsvTabPartner.ShowGroups = false;
			lsvTabPartner.Groups.Clear();
		}
		else
		{
			lsvTabPartner.ShowGroups = true;
		}
		foreach (Partner varclsPartner in pPartnerList)
		{
			try
			{
				varCounter++;
				if (varCounter % 200 == 0)
				{
					tlpProgress.Value = varCounter;
				}
			}
			catch
			{
			}
			if (pclsFilter.GroupByDate)
			{
				string varPartnerDatUpdt = clsFunction.funcGetValue(varclsPartner.DatUpdt);
				if (!varGroupByText.Equals(varPartnerDatUpdt))
				{
					varListGroup = lsvTabPartner.Groups.Add(varPartnerDatUpdt, varPartnerDatUpdt);
				}
				varGroupByText = varPartnerDatUpdt;
			}
			else if (pclsFilter.GroupByYearMonth)
			{
				string varPartnerAnoMes = clsFunction.funcGetValue(varclsPartner.DatUpdt).Substring(0, 7);
				if (!varGroupByText.Equals(varPartnerAnoMes))
				{
					varListGroup = lsvTabPartner.Groups.Add(varPartnerAnoMes, varPartnerAnoMes);
				}
				varGroupByText = varPartnerAnoMes;
			}
			else if (pclsFilter.GroupByState)
			{
				string varPartnerCodUf = clsFunction.funcGetValue(varclsPartner.CodUf);
				if (!varGroupByText.Equals(varPartnerCodUf))
				{
					string varGroupHeaderText = varPartnerCodUf;
					Estado varEstado = _StateList.FirstOrDefault((Estado r) => r.Code.Equals(varPartnerCodUf));
					if (varEstado != null)
					{
						varGroupHeaderText = varEstado.Nome + " [ " + varGroupHeaderText + " ] ";
					}
					varListGroup = lsvTabPartner.Groups.Add(varPartnerCodUf, varGroupHeaderText);
				}
				varGroupByText = varPartnerCodUf;
			}
			else if (pclsFilter.GroupByTomaIE)
			{
				string varDocTomaIE = clsSrvGeral.funcGetIEFormat(varclsPartner.IEAtual);
				if (!varGroupByText.Equals(varDocTomaIE))
				{
					varListGroup = lsvTabPartner.Groups.Add(varDocTomaIE, varDocTomaIE);
				}
				varGroupByText = varDocTomaIE;
			}
			ListViewItem varListItem = new ListViewItem();
			varListItem.Group = varListGroup;
			varListItem = funcFillItem(varListItem, varclsPartner);
			lsvTabPartner.Items.Add(varListItem);
		}
		lsvTabPartner.EndUpdate();
		lsvTabPartner.ResumeLayout();
		tlpProgress.Visible = false;
		tlpProgress.Maximum = 0;
		tlpProgress.Value = 0;
		Application.DoEvents();
		return varclsReturn;
	}

	private ListViewItem funcFillItem(ListViewItem pListItem, Partner pclsPartner)
	{
		pListItem.SubItems.Clear();
		pListItem.Text = string.Empty;
		pListItem.Tag = pclsPartner.ID;
		pListItem.SubItems.Add(clsFunction.funcFormatDoc(pclsPartner.ID));
		if (!string.IsNullOrEmpty(pclsPartner.Name))
		{
			pListItem.SubItems.Add(pclsPartner.Name);
		}
		else
		{
			pListItem.SubItems.Add(pclsPartner.NmFant);
		}
		pListItem.SubItems.Add(pclsPartner.Email);
		pListItem.SubItems.Add(pclsPartner.FoneNum);
		pListItem.SubItems.Add(pclsPartner.InscEst);
		pListItem.SubItems.Add(varclsCadCodes.funcGetRegApur(pclsPartner.RegApur));
		pListItem.SubItems.Add(pclsPartner.CodCnae);
		pListItem.SubItems.Add(pclsPartner.Lograd);
		pListItem.SubItems.Add(pclsPartner.Numero);
		pListItem.SubItems.Add(pclsPartner.Complt);
		pListItem.SubItems.Add(pclsPartner.Bairro);
		pListItem.SubItems.Add(pclsPartner.CodMun);
		pListItem.SubItems.Add(pclsPartner.DesMun);
		pListItem.SubItems.Add(pclsPartner.CodCep);
		Estado varEstado = _StateList.FirstOrDefault((Estado r) => r.Code.Equals(pclsPartner.CodUf));
		if (varEstado != null)
		{
			pListItem.SubItems.Add(varEstado.Sigla);
		}
		else
		{
			pListItem.SubItems.Add(string.Empty);
		}
		pListItem.SubItems.Add(pclsPartner.CodISuf);
		pListItem.SubItems.Add(pclsPartner.CodIMun);
		pListItem.SubItems.Add(pclsPartner.dIniAtiv);
		pListItem.SubItems.Add(pclsPartner.dUltSit);
		pListItem.SubItems.Add(pclsPartner.dBaixa);
		pListItem.SubItems.Add(pclsPartner.IEUnica);
		pListItem.SubItems.Add(pclsPartner.IEAtual);
		if (!string.IsNullOrEmpty(pclsPartner.NmFant))
		{
			pListItem.SubItems.Add(pclsPartner.NmFant);
		}
		else
		{
			pListItem.SubItems.Add(pclsPartner.Name);
		}
		pListItem.SubItems.Add(varclsCadCodes.funcGetIndCredNFe(pclsPartner.indCredNFe));
		pListItem.SubItems.Add(varclsCadCodes.funcGetIndCredCTe(pclsPartner.indCredCTe));
		pListItem.SubItems.Add(varclsCadCodes.funcGetSituacao(pclsPartner.CodSit));
		pListItem.SubItems.Add(pclsPartner.DatCons);
		pListItem.SubItems.Add(pclsPartner.HorCons);
		pListItem.SubItems.Add(pclsPartner.SrcCert);
		pListItem.SubItems.Add(pclsPartner.SrcDFe);
		pListItem.SubItems.Add(pclsPartner.SrcCons);
		pListItem.SubItems.Add(pclsPartner.EmailCert);
		return pListItem;
	}

	private void tsmCopyDocKey_Click(object sender, EventArgs e)
	{
	}

	public void funcSelectAllItens()
	{
		foreach (ListViewItem varListItem in lsvTabPartner.Items)
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

	public async Task<clsReturn> funcExportToExcelAsync(Action<decimal> onProgressChange)
	{
		return await clsScreenGeral.funcExportToCsvAsync(lsvTabPartner, onProgressChange);
	}

	public async Task<List<Document>> funcGetDocListAsync(bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		return new List<Document>();
	}

	public int funcGetTotalDocs()
	{
		return lsvTabPartner.Items.Count;
	}

	public decimal funcGetTotalValue()
	{
		return 0m;
	}

	private void lstView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		ListView varListView = (ListView)sender;
		if (varListView != null)
		{
			int varColumnIndex = e.ColumnIndex;
			clsFunction.funcSetRegisterValue($"{base.Name}-{varListView.Name}-Index{varColumnIndex}", varListView.Columns[varColumnIndex].Width.ToString(), pGlobal: false);
		}
	}

	private void lstView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		ListView varListView = (ListView)sender;
		if (varListView != null)
		{
			int varColumnIndex = e.Header.Index;
			clsFunction.funcSetRegisterValue($"{base.Name}-{varListView.Name}-Order{varColumnIndex}", e.NewDisplayIndex.ToString(), pGlobal: false);
		}
	}

	private void lstView_ItemChecked(object sender, ItemCheckedEventArgs e)
	{
		if (e.Item.Checked)
		{
			if (e.Item.BackColor == Color.White)
			{
				e.Item.BackColor = Color.LightBlue;
			}
			else if (e.Item.BackColor == Color.PapayaWhip)
			{
				e.Item.BackColor = Color.PeachPuff;
			}
		}
		else if (e.Item.BackColor == Color.LightBlue)
		{
			e.Item.BackColor = Color.White;
		}
		else if (e.Item.BackColor == Color.PeachPuff)
		{
			e.Item.BackColor = Color.PapayaWhip;
		}
	}

	public async Task<bool> funcRefreshItensAsync(bool pFocused, bool pChecked)
	{
		return true;
	}

	private void lstView_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		if (e.Column == 0)
		{
			funcSelectAllItens();
		}
	}

	public async Task<bool> funcSetTagAsync(string pTagCode, bool pFocused, bool pChecked)
	{
		return false;
	}

	public async Task<bool> funcSetDocNoteAsync(bool pFocused, bool pChecked)
	{
		return false;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmTabPartner));
		this.pnContent = new System.Windows.Forms.Panel();
		this.tlpProgress = new System.Windows.Forms.ProgressBar();
		this.lsvTabPartner = new System.Windows.Forms.ListView();
		this.clIndex = new System.Windows.Forms.ColumnHeader();
		this.clID = new System.Windows.Forms.ColumnHeader();
		this.clName = new System.Windows.Forms.ColumnHeader();
		this.clEmail = new System.Windows.Forms.ColumnHeader();
		this.clFoneNum = new System.Windows.Forms.ColumnHeader();
		this.clInscEst = new System.Windows.Forms.ColumnHeader();
		this.clRegApur = new System.Windows.Forms.ColumnHeader();
		this.clCodCnae = new System.Windows.Forms.ColumnHeader();
		this.clLograd = new System.Windows.Forms.ColumnHeader();
		this.clNumero = new System.Windows.Forms.ColumnHeader();
		this.clComplt = new System.Windows.Forms.ColumnHeader();
		this.clBairro = new System.Windows.Forms.ColumnHeader();
		this.clCodMun = new System.Windows.Forms.ColumnHeader();
		this.clDesMun = new System.Windows.Forms.ColumnHeader();
		this.clCodCep = new System.Windows.Forms.ColumnHeader();
		this.clCodUf = new System.Windows.Forms.ColumnHeader();
		this.clCodISuf = new System.Windows.Forms.ColumnHeader();
		this.clCodIMun = new System.Windows.Forms.ColumnHeader();
		this.cldIniAtiv = new System.Windows.Forms.ColumnHeader();
		this.cldUltSit = new System.Windows.Forms.ColumnHeader();
		this.cldBaixa = new System.Windows.Forms.ColumnHeader();
		this.clIEUnica = new System.Windows.Forms.ColumnHeader();
		this.clIEAtual = new System.Windows.Forms.ColumnHeader();
		this.clNmFant = new System.Windows.Forms.ColumnHeader();
		this.clindCredNFe = new System.Windows.Forms.ColumnHeader();
		this.clindCredCTe = new System.Windows.Forms.ColumnHeader();
		this.clCodSit = new System.Windows.Forms.ColumnHeader();
		this.clDatCons = new System.Windows.Forms.ColumnHeader();
		this.clHorCons = new System.Windows.Forms.ColumnHeader();
		this.clSrcCert = new System.Windows.Forms.ColumnHeader();
		this.clSrcDFe = new System.Windows.Forms.ColumnHeader();
		this.clSrcCons = new System.Windows.Forms.ColumnHeader();
		this.clEmailCert = new System.Windows.Forms.ColumnHeader();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.pnContent.SuspendLayout();
		base.SuspendLayout();
		this.pnContent.Controls.Add(this.tlpProgress);
		this.pnContent.Controls.Add(this.lsvTabPartner);
		this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnContent.Location = new System.Drawing.Point(0, 0);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(946, 361);
		this.pnContent.TabIndex = 10;
		this.tlpProgress.Dock = System.Windows.Forms.DockStyle.Top;
		this.tlpProgress.Location = new System.Drawing.Point(0, 0);
		this.tlpProgress.Name = "tlpProgress";
		this.tlpProgress.Size = new System.Drawing.Size(946, 20);
		this.tlpProgress.TabIndex = 15;
		this.tlpProgress.Visible = false;
		this.lsvTabPartner.Activation = System.Windows.Forms.ItemActivation.OneClick;
		this.lsvTabPartner.Alignment = System.Windows.Forms.ListViewAlignment.Left;
		this.lsvTabPartner.AllowColumnReorder = true;
		this.lsvTabPartner.CheckBoxes = true;
		this.lsvTabPartner.Columns.AddRange(new System.Windows.Forms.ColumnHeader[33]
		{
			this.clIndex, this.clID, this.clName, this.clEmail, this.clFoneNum, this.clInscEst, this.clRegApur, this.clCodCnae, this.clLograd, this.clNumero,
			this.clComplt, this.clBairro, this.clCodMun, this.clDesMun, this.clCodCep, this.clCodUf, this.clCodISuf, this.clCodIMun, this.cldIniAtiv, this.cldUltSit,
			this.cldBaixa, this.clIEUnica, this.clIEAtual, this.clNmFant, this.clindCredNFe, this.clindCredCTe, this.clCodSit, this.clDatCons, this.clHorCons, this.clSrcCert,
			this.clSrcDFe, this.clSrcCons, this.clEmailCert
		});
		this.lsvTabPartner.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lsvTabPartner.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lsvTabPartner.FullRowSelect = true;
		this.lsvTabPartner.HideSelection = false;
		this.lsvTabPartner.Location = new System.Drawing.Point(0, 0);
		this.lsvTabPartner.Name = "lsvTabPartner";
		this.lsvTabPartner.Size = new System.Drawing.Size(946, 361);
		this.lsvTabPartner.SmallImageList = this.ImageListDocs;
		this.lsvTabPartner.TabIndex = 13;
		this.lsvTabPartner.UseCompatibleStateImageBehavior = false;
		this.lsvTabPartner.View = System.Windows.Forms.View.Details;
		this.lsvTabPartner.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lstView_ColumnClick);
		this.lsvTabPartner.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(lstView_ColumnReordered);
		this.lsvTabPartner.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lstView_ColumnWidthChanged);
		this.lsvTabPartner.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lstView_ItemChecked);
		this.clIndex.Text = "";
		this.clIndex.Width = 25;
		this.clID.Text = "CNPJ/CPF";
		this.clID.Width = 128;
		this.clName.DisplayIndex = 23;
		this.clName.Text = "Nome";
		this.clName.Width = 229;
		this.clEmail.Text = "Email";
		this.clEmail.Width = 230;
		this.clFoneNum.Text = "Fone";
		this.clFoneNum.Width = 100;
		this.clInscEst.Text = "IE";
		this.clInscEst.Width = 103;
		this.clRegApur.Text = "Regime";
		this.clRegApur.Width = 118;
		this.clCodCnae.Text = "CNAE";
		this.clCodCnae.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clCodCnae.Width = 61;
		this.clLograd.Text = "Endereço";
		this.clLograd.Width = 250;
		this.clNumero.Text = "Num.";
		this.clNumero.Width = 47;
		this.clComplt.Text = "Complemento";
		this.clComplt.Width = 120;
		this.clBairro.Text = "Bairro";
		this.clBairro.Width = 150;
		this.clCodMun.Text = "CodMun";
		this.clCodMun.Width = 61;
		this.clDesMun.Text = "Município";
		this.clDesMun.Width = 196;
		this.clCodCep.Text = "CEP";
		this.clCodCep.Width = 68;
		this.clCodUf.Text = "UF";
		this.clCodUf.Width = 40;
		this.clCodISuf.Text = "Suframa";
		this.clCodISuf.Width = 92;
		this.clCodIMun.Text = "Insc.Mun";
		this.clCodIMun.Width = 89;
		this.cldIniAtiv.Text = "InicAtiv";
		this.cldIniAtiv.Width = 103;
		this.cldUltSit.Text = "UltSit";
		this.cldUltSit.Width = 90;
		this.cldBaixa.Text = "DtBaixa";
		this.cldBaixa.Width = 76;
		this.clIEUnica.Text = "IEUnica";
		this.clIEUnica.Width = 81;
		this.clIEAtual.Text = "IEAtual";
		this.clIEAtual.Width = 80;
		this.clNmFant.DisplayIndex = 2;
		this.clNmFant.Text = "Nome Fantasia";
		this.clNmFant.Width = 229;
		this.clindCredNFe.Text = "CredNFe";
		this.clindCredNFe.Width = 77;
		this.clindCredCTe.Text = "CredCTe";
		this.clindCredCTe.Width = 93;
		this.clCodSit.Text = "Situação";
		this.clCodSit.Width = 86;
		this.clDatCons.Text = "DataCons";
		this.clDatCons.Width = 83;
		this.clHorCons.Text = "HoraCons";
		this.clHorCons.Width = 81;
		this.clSrcCert.Text = "CertFont";
		this.clSrcCert.Width = 67;
		this.clSrcDFe.Text = "DFeFont";
		this.clSrcDFe.Width = 64;
		this.clSrcCons.Text = "ConsFont";
		this.clSrcCons.Width = 64;
		this.clEmailCert.Text = "EmailCert";
		this.clEmailCert.Width = 284;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(946, 361);
		base.Controls.Add(this.pnContent);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmTabPartner";
		this.Text = "Parceiros Comerciais";
		base.Load += new System.EventHandler(frmTabPartner_Load);
		this.pnContent.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
