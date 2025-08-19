using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmSqlEditor : Form
{
	private bool varIsLoading;

	private clsDataParameter varclsDataParameter = new clsDataParameter();

	private IContainer components;

	private Panel panel6;

	private Button btExecute;

	private TextBox txDbaSqlStr;

	private Label lbDbaSqlStr;

	private DataGridView dgData;

	private TextBox txDbaConSTr;

	private Label lbDbaConSTr;

	private ComboBox cbDbaType;

	private Label lbDbaType;

	private Button btExport;

	public frmSqlEditor()
	{
		InitializeComponent();
	}

	private async void frmSqlEditor_Load(object sender, EventArgs e)
	{
		Text = Text + " - Versão : " + Application.ProductVersion;
		funcLoadDbaType();
		clsDbaFactory varclsFactory = new clsDbaFactory();
		intDatabase varclsDatabase = await varclsFactory.funcGetClassAsync();
		cbDbaType.SelectedValue = varclsDatabase.funcGetDbaType();
		txDbaConSTr.Text = varclsFactory.funcGetDbaCStr();
		if (!clsFunction.IsAdmin)
		{
			txDbaConSTr.PasswordChar = '*';
		}
		varIsLoading = false;
	}

	private void funcLoadDbaType()
	{
		List<clsObjectType> varDbaTypeList = clsSrvGeral.funcGetExtSystDbaTypeList(pLocal: true);
		cbDbaType.DisplayMember = "Name";
		cbDbaType.ValueMember = "Value";
		cbDbaType.DataSource = varDbaTypeList;
		cbDbaType.SelectedIndex = -1;
	}

	private async void btExecute_Click(object sender, EventArgs e)
	{
		try
		{
			btExecute.Text = "Executando";
			btExecute.Enabled = false;
			clsReturn varclsReturnFunc = await funcExecuteAsync();
			if (varclsReturnFunc.HasMessage())
			{
				clsScreenGeral.funcShowUserMessage(this, varclsReturnFunc, pNotSendToSoftError: true);
			}
		}
		finally
		{
			btExecute.Text = "Executar";
			btExecute.Enabled = true;
		}
	}

	private async Task<clsReturn> funcExecuteAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			string varDbaType = clsFunction.funcGetValue(cbDbaType.SelectedValue);
			IEnumerable<object> varDocList = null;
			using (intDatabase varclsDbaClient = await new clsDbaFactory().funcGetClassAsync(varDbaType, txDbaConSTr.Text))
			{
				if (varclsDbaClient == null)
				{
					clsMessage varclsMessage = new clsMessage("W", "9999", "Nenhum tipo de banco foi selecionado.");
					varclsReturnFunc.AddMessage(varclsMessage);
					return varclsReturnFunc;
				}
				varclsReturnFunc = await varclsDbaClient.funcTestConAsync();
				if (varclsReturnFunc.HasError)
				{
					return varclsReturnFunc;
				}
				varclsReturnFunc = await varclsDbaClient.funcOpenAsync();
				if (varclsReturnFunc.HasError)
				{
					return varclsReturnFunc;
				}
				varDocList = await varclsDbaClient.funcQueryAsync<object>(txDbaSqlStr.Text);
			}
			dgData.DataSource = funcToDataTable(varDocList);
			dgData.AutoResizeColumns();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		return varclsReturnFunc;
	}

	private DataTable funcToDataTable(IEnumerable<object> varList)
	{
		object[] varArray = varList.ToArray();
		if (varArray.Count() == 0)
		{
			return null;
		}
		DataTable varDataTable = new DataTable();
		foreach (string varKey in ((IDictionary<string, object>)varArray[0]).Keys)
		{
			varDataTable.Columns.Add(varKey);
		}
		object[] array = varArray;
		foreach (object varRow in array)
		{
			varDataTable.Rows.Add(((IDictionary<string, object>)varRow).Values.ToArray());
		}
		return varDataTable;
	}

	private async void btExport_Click(object sender, EventArgs e)
	{
		clsReturn varclsReturn = await clsScreenGeral.funcExportObjToExcelAsync(dgData);
		if (varclsReturn.HasError)
		{
			clsScreenGeral.funcShowUserMessage(this, varclsReturn, pNotSendToSoftError: true);
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmSqlEditor));
		this.panel6 = new System.Windows.Forms.Panel();
		this.btExport = new System.Windows.Forms.Button();
		this.cbDbaType = new System.Windows.Forms.ComboBox();
		this.lbDbaType = new System.Windows.Forms.Label();
		this.btExecute = new System.Windows.Forms.Button();
		this.txDbaSqlStr = new System.Windows.Forms.TextBox();
		this.lbDbaSqlStr = new System.Windows.Forms.Label();
		this.dgData = new System.Windows.Forms.DataGridView();
		this.txDbaConSTr = new System.Windows.Forms.TextBox();
		this.lbDbaConSTr = new System.Windows.Forms.Label();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgData).BeginInit();
		base.SuspendLayout();
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.btExport);
		this.panel6.Controls.Add(this.cbDbaType);
		this.panel6.Controls.Add(this.lbDbaType);
		this.panel6.Controls.Add(this.btExecute);
		this.panel6.Controls.Add(this.txDbaSqlStr);
		this.panel6.Controls.Add(this.lbDbaSqlStr);
		this.panel6.Controls.Add(this.dgData);
		this.panel6.Controls.Add(this.txDbaConSTr);
		this.panel6.Controls.Add(this.lbDbaConSTr);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(719, 511);
		this.panel6.TabIndex = 165;
		this.btExport.BackColor = System.Drawing.SystemColors.Control;
		this.btExport.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.btExport.Location = new System.Drawing.Point(8, 131);
		this.btExport.Name = "btExport";
		this.btExport.Size = new System.Drawing.Size(118, 28);
		this.btExport.TabIndex = 290;
		this.btExport.Tag = "";
		this.btExport.Text = "E&xportar";
		this.btExport.UseVisualStyleBackColor = false;
		this.btExport.Click += new System.EventHandler(btExport_Click);
		this.cbDbaType.BackColor = System.Drawing.SystemColors.Info;
		this.cbDbaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbDbaType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbDbaType.FormattingEnabled = true;
		this.cbDbaType.Location = new System.Drawing.Point(135, 6);
		this.cbDbaType.Name = "cbDbaType";
		this.cbDbaType.Size = new System.Drawing.Size(279, 22);
		this.cbDbaType.TabIndex = 2;
		this.cbDbaType.TabStop = false;
		this.lbDbaType.AutoSize = true;
		this.lbDbaType.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbDbaType.Location = new System.Drawing.Point(32, 10);
		this.lbDbaType.Name = "lbDbaType";
		this.lbDbaType.Size = new System.Drawing.Size(96, 13);
		this.lbDbaType.TabIndex = 289;
		this.lbDbaType.Text = "Tipo de banco :";
		this.lbDbaType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btExecute.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btExecute.BackColor = System.Drawing.SystemColors.Control;
		this.btExecute.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.btExecute.Location = new System.Drawing.Point(590, 131);
		this.btExecute.Name = "btExecute";
		this.btExecute.Size = new System.Drawing.Size(118, 28);
		this.btExecute.TabIndex = 1;
		this.btExecute.Tag = "";
		this.btExecute.Text = "&Executar";
		this.btExecute.UseVisualStyleBackColor = false;
		this.btExecute.Click += new System.EventHandler(btExecute_Click);
		this.txDbaSqlStr.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txDbaSqlStr.BackColor = System.Drawing.SystemColors.Info;
		this.txDbaSqlStr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txDbaSqlStr.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.txDbaSqlStr.Location = new System.Drawing.Point(134, 74);
		this.txDbaSqlStr.Multiline = true;
		this.txDbaSqlStr.Name = "txDbaSqlStr";
		this.txDbaSqlStr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txDbaSqlStr.Size = new System.Drawing.Size(574, 52);
		this.txDbaSqlStr.TabIndex = 0;
		this.txDbaSqlStr.Tag = "";
		this.lbDbaSqlStr.AutoSize = true;
		this.lbDbaSqlStr.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbDbaSqlStr.Location = new System.Drawing.Point(32, 74);
		this.lbDbaSqlStr.Name = "lbDbaSqlStr";
		this.lbDbaSqlStr.Size = new System.Drawing.Size(98, 13);
		this.lbDbaSqlStr.TabIndex = 285;
		this.lbDbaSqlStr.Text = "Comando SQL :";
		this.dgData.AllowUserToAddRows = false;
		this.dgData.AllowUserToDeleteRows = false;
		this.dgData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgData.Location = new System.Drawing.Point(8, 165);
		this.dgData.Name = "dgData";
		this.dgData.ReadOnly = true;
		dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.dgData.RowsDefaultCellStyle = dataGridViewCellStyle1;
		this.dgData.Size = new System.Drawing.Size(700, 337);
		this.dgData.TabIndex = 284;
		this.txDbaConSTr.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txDbaConSTr.BackColor = System.Drawing.SystemColors.Info;
		this.txDbaConSTr.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txDbaConSTr.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.txDbaConSTr.Location = new System.Drawing.Point(134, 34);
		this.txDbaConSTr.Multiline = true;
		this.txDbaConSTr.Name = "txDbaConSTr";
		this.txDbaConSTr.Size = new System.Drawing.Size(574, 34);
		this.txDbaConSTr.TabIndex = 3;
		this.txDbaConSTr.TabStop = false;
		this.txDbaConSTr.Tag = "";
		this.lbDbaConSTr.AutoSize = true;
		this.lbDbaConSTr.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.lbDbaConSTr.Location = new System.Drawing.Point(8, 34);
		this.lbDbaConSTr.Name = "lbDbaConSTr";
		this.lbDbaConSTr.Size = new System.Drawing.Size(120, 13);
		this.lbDbaConSTr.TabIndex = 281;
		this.lbDbaConSTr.Text = "String de conexão :";
		this.lbDbaConSTr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(719, 511);
		base.Controls.Add(this.panel6);
		this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "frmSqlEditor";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - SQL Editor";
		base.Load += new System.EventHandler(frmSqlEditor_Load);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgData).EndInit();
		base.ResumeLayout(false);
	}
}
