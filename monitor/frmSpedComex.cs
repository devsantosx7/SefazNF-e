using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using audit.fiscal.io;
using data.fiscal.io;
using screen.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmSpedComex : Form
{
	public class DueModel
	{
		public string Filial { get; set; }

		public string Num { get; set; }

		public string DataDue { get; set; }

		public string DataAverb { get; set; }

		public string ConEmbNum { get; set; }

		public string ConEmbTipo { get; set; }

		public string ConEmbData { get; set; }
	}

	private clsBlockLine0000 _clsObject;

	private FileInfo _clsFileInfo;

	private clsSpedComex _SpedComex = new clsSpedComex();

	private clsDataParameter _clsDataParam = new clsDataParameter();

	private List<DueHeader> _DueList = new List<DueHeader>();

	private IContainer components;

	private Panel panel1;

	private Label lbUserInfo02;

	private Label lbUserInfo01;

	private Label lbTitle;

	private Button btCancelar;

	private Button btConfirm;

	private Label label2;

	private DataGridView dtGridData;

	private Panel panel2;

	private Label label5;

	private Label txFilial;

	private Label lbFilial;

	private Label txFile;

	private Label lbFile;

	private Label lbTo;

	private Label label3;

	private Label lbFrom;

	private Label lbPeriod;

	private Label lbConEmbTipo;

	private PictureBox picConEmbTipo;

	private ComboBox cbConEmbTipo;

	private Button btApply;

	private DataGridViewTextBoxColumn ColFilial;

	private DataGridViewTextBoxColumn colNum;

	private DataGridViewTextBoxColumn colDataDue;

	private DataGridViewTextBoxColumn colDataAverb;

	private DataGridViewTextBoxColumn ColConEmbNum;

	private DataGridViewTextBoxColumn colConEmbData;

	private DataGridViewComboBoxColumn colConEmbTipo;

	public frmSpedComex(string pFileName, clsBlockLine0000 pclsObject)
	{
		InitializeComponent();
		_clsObject = pclsObject;
		_clsFileInfo = new FileInfo(pFileName);
	}

	private async void frmSpedComex_Shown(object sender, EventArgs e)
	{
		clsReturn varclsReturn = await _SpedComex.funcGetDueListAsync(_clsObject);
		if (varclsReturn.HasError || varclsReturn.HasWarning)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturn);
		}
		await funcLoadTipoConhecAsync();
		List<DueHeader> varDueList = varclsReturn.GetObject<List<DueHeader>>("Object");
		if (varDueList == null)
		{
			return;
		}
		List<clsObjectType> varTipoList = funcConEmbTipoList();
		DataGridViewComboBoxColumn obj = (DataGridViewComboBoxColumn)dtGridData.Columns[6];
		obj.DisplayMember = "Name";
		obj.ValueMember = "Value";
		obj.DataSource = varTipoList;
		List<DueModel> varDueModelList = new List<DueModel>();
		foreach (DueHeader varclsItem in varDueList)
		{
			DueModel varclsModel = new DueModel();
			varclsModel.Filial = varclsItem.Filial;
			varclsModel.Num = varclsItem.Num;
			varclsModel.DataDue = varclsItem.DataDue;
			varclsModel.DataAverb = varclsItem.DataAverb;
			varclsModel.ConEmbNum = varclsItem.ConEmbNum;
			varclsModel.ConEmbData = varclsItem.ConEmbData;
			varclsModel.ConEmbTipo = varclsItem.ConEmbTipo;
			varDueModelList.Add(varclsModel);
		}
		varDueModelList = varDueModelList.OrderBy((DueModel varDueModel) => varDueModel.DataAverb).ToList();
		BindingSource varBinding = new BindingSource();
		varBinding.DataSource = varDueModelList;
		dtGridData.DataSource = varBinding;
	}

	private void frmSpedComex_Load(object sender, EventArgs e)
	{
		txFile.Text = _clsFileInfo.Name;
		lbFrom.Text = clsAuditGeral.funcGetDate(_clsObject.DT_INI).ToString("dd.MM.yyyy");
		lbTo.Text = clsAuditGeral.funcGetDate(_clsObject.DT_FIN).ToString("dd.MM.yyyy");
		txFilial.Text = _clsObject.CNPJ + " : " + _clsObject.NOME;
	}

	private async Task<bool> funcLoadTipoConhecAsync()
	{
		List<clsObjectType> varTipoList = funcConEmbTipoList();
		cbConEmbTipo.DisplayMember = "Name";
		cbConEmbTipo.ValueMember = "Value";
		cbConEmbTipo.DataSource = varTipoList;
		string varTipConEmbTipo = await _clsDataParam.funcGetAsync("COMEX-TIPO-CONHEC-EMB");
		if (clsFunction.IsEmpty(varTipConEmbTipo))
		{
			cbConEmbTipo.SelectedIndex = -1;
		}
		else
		{
			try
			{
				cbConEmbTipo.SelectedValue = varTipConEmbTipo;
			}
			catch
			{
			}
		}
		return true;
	}

	private async void btConfirm_Click(object sender, EventArgs e)
	{
		clsDataDueHeader varclsDataDueHeader = new clsDataDueHeader();
		bool varError = false;
		_DueList.Clear();
		foreach (DataGridViewRow varclsRow in (IEnumerable)dtGridData.Rows)
		{
			DueModel varclsModel = new DueModel
			{
				Filial = funcGetValue("colFilial", varclsRow.Index),
				Num = funcGetValue("colNum", varclsRow.Index),
				DataDue = funcGetValue("colDataDue", varclsRow.Index),
				DataAverb = funcGetValue("colDataAverb", varclsRow.Index),
				ConEmbNum = funcGetValue("colConEmbNum", varclsRow.Index),
				ConEmbData = funcGetValue("colConEmbData", varclsRow.Index),
				ConEmbTipo = funcGetValue("colConEmbTipo", varclsRow.Index)
			};
			if (clsFunction.IsEmpty(varclsModel.ConEmbNum))
			{
				varError = true;
			}
			else if (clsFunction.IsEmpty(varclsModel.ConEmbTipo))
			{
				varError = true;
			}
			else if (clsFunction.IsEmpty(varclsModel.ConEmbData))
			{
				varError = true;
			}
			DueHeader varclsDueHeader = await varclsDataDueHeader.funcGetItemByKeyAsync(varclsModel.Filial, varclsModel.Num);
			if (varclsDueHeader != null)
			{
				varclsDueHeader.ConEmbNum = varclsModel.ConEmbNum;
				varclsDueHeader.ConEmbData = varclsModel.ConEmbData;
				varclsDueHeader.ConEmbTipo = varclsModel.ConEmbTipo;
				await varclsDataDueHeader.funcUpdateAsync(varclsDueHeader);
				_DueList.Add(varclsDueHeader);
			}
		}
		if (varError)
		{
			string varMessage = "Um ou mais campos relacionados ao Conhecimento de Transporte";
			varMessage += " não foram preenchidos. Deseja continuar mesmo assim?";
			if (!MessageBox.Show(this, varMessage, "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private string funcGetValue(string pColum, int pRow)
	{
		string varReturn = string.Empty;
		foreach (DataGridViewColumn varCol in dtGridData.Columns)
		{
			if (clsFunction.IsEqual(varCol.Name, pColum, pIgnoreCase: true))
			{
				varReturn = (string)dtGridData.Rows[pRow].Cells[varCol.Index].Value;
				break;
			}
		}
		return varReturn;
	}

	private bool funcSetValue(string pColum, int pRow, string pValue)
	{
		_ = string.Empty;
		foreach (DataGridViewColumn varCol in dtGridData.Columns)
		{
			if (clsFunction.IsEqual(varCol.Name, pColum, pIgnoreCase: true))
			{
				dtGridData.Rows[pRow].Cells[varCol.Index].Value = pValue;
			}
		}
		return true;
	}

	private void dtGridData_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (!dtGridData.CurrentCell.ReadOnly)
		{
			dtGridData.BeginEdit(selectAll: true);
		}
	}

	private void dtGridData_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (!dtGridData.CurrentCell.ReadOnly)
		{
			dtGridData.BeginEdit(selectAll: true);
		}
	}

	private void dtGridData_KeyDown(object sender, KeyEventArgs e)
	{
		if (!dtGridData.CurrentCell.ReadOnly && (e.KeyData.Equals(Keys.Down) || e.KeyData.Equals(Keys.Up)))
		{
			dtGridData.BeginEdit(selectAll: true);
		}
	}

	private void btCancelar_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	public List<clsObjectType> funcConEmbTipoList()
	{
		return new List<clsObjectType>
		{
			new clsObjectType
			{
				Value = "",
				Name = ""
			},
			new clsObjectType
			{
				Value = "01",
				Name = "01 - AWB"
			},
			new clsObjectType
			{
				Value = "02",
				Name = "02 - MAWB"
			},
			new clsObjectType
			{
				Value = "03",
				Name = "03 - HAWB"
			},
			new clsObjectType
			{
				Value = "04",
				Name = "04 - COMAT"
			},
			new clsObjectType
			{
				Value = "06",
				Name = "06 - R.EXPRESSAS"
			},
			new clsObjectType
			{
				Value = "07",
				Name = "07 - ETIQ.REXPRESSAS"
			},
			new clsObjectType
			{
				Value = "08",
				Name = "08 - HR.EXPRESSAS"
			},
			new clsObjectType
			{
				Value = "09",
				Name = "09 - AV7"
			},
			new clsObjectType
			{
				Value = "10",
				Name = "10 - BL"
			},
			new clsObjectType
			{
				Value = "11",
				Name = "11 - MBL"
			},
			new clsObjectType
			{
				Value = "12",
				Name = "12 - HBL"
			},
			new clsObjectType
			{
				Value = "13",
				Name = "13 - CRT"
			},
			new clsObjectType
			{
				Value = "14",
				Name = "14 - DSIC"
			},
			new clsObjectType
			{
				Value = "16",
				Name = "16 - COMAT BL"
			},
			new clsObjectType
			{
				Value = "17",
				Name = "17 - RWB"
			},
			new clsObjectType
			{
				Value = "18",
				Name = "18 - HRWB"
			},
			new clsObjectType
			{
				Value = "19",
				Name = "19 - TIF / DTA"
			},
			new clsObjectType
			{
				Value = "20",
				Name = "20 - CP2"
			},
			new clsObjectType
			{
				Value = "91",
				Name = "91 - NÂO IATA"
			},
			new clsObjectType
			{
				Value = "92",
				Name = "92 - MNAO IATA"
			},
			new clsObjectType
			{
				Value = "93",
				Name = "93 - HNAO IATA"
			},
			new clsObjectType
			{
				Value = "99",
				Name = "99 – OUTROS"
			}
		};
	}

	private async void btApply_Click(object sender, EventArgs e)
	{
		string varTipConEmbTipo = clsFunction.funcGetValue(cbConEmbTipo.SelectedValue);
		foreach (DataGridViewRow varclsRow in (IEnumerable)dtGridData.Rows)
		{
			funcSetValue("colConEmbTipo", varclsRow.Index, varTipConEmbTipo);
		}
		await _clsDataParam.funcSetAsync("COMEX-TIPO-CONHEC-EMB", varTipConEmbTipo);
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmSpedComex));
		this.panel1 = new System.Windows.Forms.Panel();
		this.lbUserInfo02 = new System.Windows.Forms.Label();
		this.lbUserInfo01 = new System.Windows.Forms.Label();
		this.lbTitle = new System.Windows.Forms.Label();
		this.btCancelar = new System.Windows.Forms.Button();
		this.btConfirm = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.dtGridData = new System.Windows.Forms.DataGridView();
		this.panel2 = new System.Windows.Forms.Panel();
		this.lbTo = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.lbFrom = new System.Windows.Forms.Label();
		this.lbPeriod = new System.Windows.Forms.Label();
		this.txFilial = new System.Windows.Forms.Label();
		this.lbFilial = new System.Windows.Forms.Label();
		this.txFile = new System.Windows.Forms.Label();
		this.lbFile = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lbConEmbTipo = new System.Windows.Forms.Label();
		this.picConEmbTipo = new System.Windows.Forms.PictureBox();
		this.cbConEmbTipo = new System.Windows.Forms.ComboBox();
		this.btApply = new System.Windows.Forms.Button();
		this.ColFilial = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.colDataDue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.colDataAverb = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ColConEmbNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.colConEmbData = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.colConEmbTipo = new System.Windows.Forms.DataGridViewComboBoxColumn();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dtGridData).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picConEmbTipo).BeginInit();
		base.SuspendLayout();
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.BackColor = System.Drawing.SystemColors.Info;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.lbUserInfo02);
		this.panel1.Controls.Add(this.lbUserInfo01);
		this.panel1.Controls.Add(this.lbTitle);
		this.panel1.Location = new System.Drawing.Point(5, 5);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(831, 54);
		this.panel1.TabIndex = 174;
		this.lbUserInfo02.AutoSize = true;
		this.lbUserInfo02.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbUserInfo02.Location = new System.Drawing.Point(4, 29);
		this.lbUserInfo02.Name = "lbUserInfo02";
		this.lbUserInfo02.Size = new System.Drawing.Size(268, 14);
		this.lbUserInfo02.TabIndex = 2;
		this.lbUserInfo02.Text = "Por favor, confirme antes de continuar?";
		this.lbUserInfo01.AutoSize = true;
		this.lbUserInfo01.Font = new System.Drawing.Font("Verdana", 9f);
		this.lbUserInfo01.Location = new System.Drawing.Point(4, 7);
		this.lbUserInfo01.Name = "lbUserInfo01";
		this.lbUserInfo01.Size = new System.Drawing.Size(373, 14);
		this.lbUserInfo01.TabIndex = 1;
		this.lbUserInfo01.Text = "Informações selecionadas para registro no SPED ICMS/IPI";
		this.lbTitle.AutoSize = true;
		this.lbTitle.Location = new System.Drawing.Point(12, 8);
		this.lbTitle.Name = "lbTitle";
		this.lbTitle.Size = new System.Drawing.Size(0, 13);
		this.lbTitle.TabIndex = 0;
		this.btCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btCancelar.Location = new System.Drawing.Point(8, 538);
		this.btCancelar.Name = "btCancelar";
		this.btCancelar.Size = new System.Drawing.Size(87, 34);
		this.btCancelar.TabIndex = 184;
		this.btCancelar.Text = "&Cancelar";
		this.btCancelar.UseVisualStyleBackColor = true;
		this.btCancelar.Click += new System.EventHandler(btCancelar_Click);
		this.btConfirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btConfirm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btConfirm.Location = new System.Drawing.Point(711, 538);
		this.btConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.btConfirm.Name = "btConfirm";
		this.btConfirm.Size = new System.Drawing.Size(123, 34);
		this.btConfirm.TabIndex = 183;
		this.btConfirm.Text = "&Confirmar";
		this.btConfirm.UseVisualStyleBackColor = true;
		this.btConfirm.Click += new System.EventHandler(btConfirm_Click);
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(5, 530);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(831, 2);
		this.label2.TabIndex = 185;
		this.dtGridData.AllowUserToAddRows = false;
		this.dtGridData.AllowUserToDeleteRows = false;
		this.dtGridData.AllowUserToResizeRows = false;
		this.dtGridData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 8.25f);
		dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dtGridData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
		this.dtGridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dtGridData.Columns.AddRange(this.ColFilial, this.colNum, this.colDataDue, this.colDataAverb, this.ColConEmbNum, this.colConEmbData, this.colConEmbTipo);
		this.dtGridData.Location = new System.Drawing.Point(5, 108);
		this.dtGridData.Name = "dtGridData";
		this.dtGridData.RowHeadersVisible = false;
		this.dtGridData.RowHeadersWidth = 35;
		this.dtGridData.Size = new System.Drawing.Size(831, 419);
		this.dtGridData.TabIndex = 186;
		this.dtGridData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dtGridData_CellClick);
		this.dtGridData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dtGridData_CellContentClick);
		this.dtGridData.KeyDown += new System.Windows.Forms.KeyEventHandler(dtGridData_KeyDown);
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel2.BackColor = System.Drawing.SystemColors.Info;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.lbTo);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.lbFrom);
		this.panel2.Controls.Add(this.lbPeriod);
		this.panel2.Controls.Add(this.txFilial);
		this.panel2.Controls.Add(this.lbFilial);
		this.panel2.Controls.Add(this.txFile);
		this.panel2.Controls.Add(this.lbFile);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Location = new System.Drawing.Point(5, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(831, 51);
		this.panel2.TabIndex = 192;
		this.lbTo.AutoSize = true;
		this.lbTo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTo.Location = new System.Drawing.Point(711, 26);
		this.lbTo.Name = "lbTo";
		this.lbTo.Size = new System.Drawing.Size(95, 14);
		this.lbTo.TabIndex = 10;
		this.lbTo.Text = "01/01/2022";
		this.lbTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(676, 26);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(28, 14);
		this.label3.TabIndex = 9;
		this.label3.Text = "até";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbFrom.AutoSize = true;
		this.lbFrom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFrom.Location = new System.Drawing.Point(574, 26);
		this.lbFrom.Name = "lbFrom";
		this.lbFrom.Size = new System.Drawing.Size(95, 14);
		this.lbFrom.TabIndex = 8;
		this.lbFrom.Text = "01/01/2022";
		this.lbFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbPeriod.AutoSize = true;
		this.lbPeriod.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbPeriod.Location = new System.Drawing.Point(504, 26);
		this.lbPeriod.Name = "lbPeriod";
		this.lbPeriod.Size = new System.Drawing.Size(64, 14);
		this.lbPeriod.TabIndex = 7;
		this.lbPeriod.Text = "Período :";
		this.txFilial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txFilial.Location = new System.Drawing.Point(76, 26);
		this.txFilial.Name = "txFilial";
		this.txFilial.Size = new System.Drawing.Size(422, 14);
		this.txFilial.TabIndex = 6;
		this.txFilial.Text = "...";
		this.lbFilial.AutoSize = true;
		this.lbFilial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFilial.Location = new System.Drawing.Point(26, 26);
		this.lbFilial.Name = "lbFilial";
		this.lbFilial.Size = new System.Drawing.Size(43, 14);
		this.lbFilial.TabIndex = 5;
		this.lbFilial.Text = "Filial :";
		this.txFile.AutoSize = true;
		this.txFile.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txFile.Location = new System.Drawing.Point(76, 6);
		this.txFile.Name = "txFile";
		this.txFile.Size = new System.Drawing.Size(19, 14);
		this.txFile.TabIndex = 4;
		this.txFile.Text = "...";
		this.lbFile.AutoSize = true;
		this.lbFile.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbFile.Location = new System.Drawing.Point(6, 6);
		this.lbFile.Name = "lbFile";
		this.lbFile.Size = new System.Drawing.Size(63, 14);
		this.lbFile.TabIndex = 3;
		this.lbFile.Text = "Arquivo :";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(12, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(0, 13);
		this.label5.TabIndex = 0;
		this.lbConEmbTipo.AutoSize = true;
		this.lbConEmbTipo.Location = new System.Drawing.Point(206, 549);
		this.lbConEmbTipo.Name = "lbConEmbTipo";
		this.lbConEmbTipo.Size = new System.Drawing.Size(148, 13);
		this.lbConEmbTipo.TabIndex = 193;
		this.lbConEmbTipo.Text = "Tipo de Conhecimento : ";
		this.picConEmbTipo.Image = Monitor.Resources.image_tip;
		this.picConEmbTipo.Location = new System.Drawing.Point(182, 545);
		this.picConEmbTipo.Name = "picConEmbTipo";
		this.picConEmbTipo.Size = new System.Drawing.Size(20, 20);
		this.picConEmbTipo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picConEmbTipo.TabIndex = 194;
		this.picConEmbTipo.TabStop = false;
		this.cbConEmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbConEmbTipo.FormattingEnabled = true;
		this.cbConEmbTipo.Location = new System.Drawing.Point(354, 545);
		this.cbConEmbTipo.Name = "cbConEmbTipo";
		this.cbConEmbTipo.Size = new System.Drawing.Size(179, 21);
		this.cbConEmbTipo.TabIndex = 196;
		this.btApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btApply.Location = new System.Drawing.Point(539, 543);
		this.btApply.Name = "btApply";
		this.btApply.Size = new System.Drawing.Size(61, 24);
		this.btApply.TabIndex = 197;
		this.btApply.Text = "&Aplicar";
		this.btApply.UseVisualStyleBackColor = true;
		this.btApply.Click += new System.EventHandler(btApply_Click);
		this.ColFilial.DataPropertyName = "Filial";
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.ColFilial.DefaultCellStyle = dataGridViewCellStyle2;
		this.ColFilial.FillWeight = 5f;
		this.ColFilial.HeaderText = "Filial";
		this.ColFilial.Name = "ColFilial";
		this.ColFilial.ReadOnly = true;
		this.ColFilial.Width = 5;
		this.colNum.DataPropertyName = "Num";
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.colNum.DefaultCellStyle = dataGridViewCellStyle3;
		this.colNum.FillWeight = 120f;
		this.colNum.HeaderText = "Due";
		this.colNum.Name = "colNum";
		this.colNum.ReadOnly = true;
		this.colNum.ToolTipText = "Declaração Única de Exportação";
		this.colNum.Width = 120;
		this.colDataDue.DataPropertyName = "DataDue";
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.colDataDue.DefaultCellStyle = dataGridViewCellStyle4;
		this.colDataDue.FillWeight = 120f;
		this.colDataDue.HeaderText = "Data Due";
		this.colDataDue.Name = "colDataDue";
		this.colDataDue.ReadOnly = true;
		this.colDataDue.ToolTipText = "Data da Declaração de exportação";
		this.colDataDue.Width = 120;
		this.colDataAverb.DataPropertyName = "DataAverb";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.colDataAverb.DefaultCellStyle = dataGridViewCellStyle5;
		this.colDataAverb.FillWeight = 125f;
		this.colDataAverb.HeaderText = "Data Averbação";
		this.colDataAverb.Name = "colDataAverb";
		this.colDataAverb.ReadOnly = true;
		this.colDataAverb.ToolTipText = "Data da averbação da Declaração de exportação";
		this.colDataAverb.Width = 125;
		this.ColConEmbNum.DataPropertyName = "ConEmbNum";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.ColConEmbNum.DefaultCellStyle = dataGridViewCellStyle6;
		this.ColConEmbNum.FillWeight = 120f;
		this.ColConEmbNum.HeaderText = "Número do Conhecimento";
		this.ColConEmbNum.Name = "ColConEmbNum";
		this.ColConEmbNum.ToolTipText = "Conhecimento de Embarque";
		this.ColConEmbNum.Width = 120;
		this.colConEmbData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
		this.colConEmbData.DataPropertyName = "ConEmbData";
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle7.Format = "dd.MM.yyyy";
		dataGridViewCellStyle7.NullValue = null;
		this.colConEmbData.DefaultCellStyle = dataGridViewCellStyle7;
		this.colConEmbData.FillWeight = 120f;
		this.colConEmbData.HeaderText = "Data.Conhec. [AAAA.MM.DD]";
		this.colConEmbData.Name = "colConEmbData";
		this.colConEmbData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.colConEmbData.ToolTipText = "Data do Conhecimento de Embarque";
		this.colConEmbData.Width = 120;
		this.colConEmbTipo.DataPropertyName = "ConEmbTipo";
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle8.Format = "yyyy.MM.dd";
		this.colConEmbTipo.DefaultCellStyle = dataGridViewCellStyle8;
		this.colConEmbTipo.FillWeight = 200f;
		this.colConEmbTipo.HeaderText = "Tipo de Conhecimento";
		this.colConEmbTipo.Name = "colConEmbTipo";
		this.colConEmbTipo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.colConEmbTipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.colConEmbTipo.ToolTipText = "Tipo de Conhecimento de Embarque";
		this.colConEmbTipo.Width = 200;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(841, 578);
		base.Controls.Add(this.btApply);
		base.Controls.Add(this.cbConEmbTipo);
		base.Controls.Add(this.picConEmbTipo);
		base.Controls.Add(this.lbConEmbTipo);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.dtGridData);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.btCancelar);
		base.Controls.Add(this.btConfirm);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 8.25f);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmSpedComex";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - SPED ICMS/IPI : Geração dos registros de exportações concluídas";
		base.Load += new System.EventHandler(frmSpedComex_Load);
		base.Shown += new System.EventHandler(frmSpedComex_Shown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dtGridData).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picConEmbTipo).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
