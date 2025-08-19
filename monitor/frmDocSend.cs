using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using srv.fiscal.io.email;
using util.fiscal.io;

namespace Monitor;

public class frmDocSend : Form
{
	public class clsChave
	{
		public string value;

		public clsChave(string pValue)
		{
			value = pValue;
		}
	}

	private List<Document> varDocumentList = new List<Document>();

	private List<Event> varEventList = new List<Event>();

	private clsZipService varclsZipService = new clsZipService();

	private clsDataConfig varclsDataConfig = new clsDataConfig();

	private clsEmailService varclsEmailService = new clsEmailService();

	private bool varEventLoad;

	private bool _SelectedAllItens;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeFeatEnabled;

	private IContainer components;

	private SplitContainer splitList;

	private FolderBrowserDialog folderDialog;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton tsbSendAll;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripDropDownButton tslXMLTitle;

	private ToolStripMenuItem tsbSendXML;

	private ToolStripMenuItem tsbBaixarXML;

	private ToolStripDropDownButton tslPDFTitle;

	private ToolStripMenuItem tsbSendPDF;

	private ToolStripMenuItem tsbBaixarPDF;

	private ListView lstDocs;

	private ColumnHeader clDoc;

	private ColumnHeader clSerie;

	private ColumnHeader clTipo;

	private ColumnHeader clEvent;

	private ColumnHeader clSeq;

	private ColumnHeader clData;

	private ColumnHeader clHora;

	private ColumnHeader clProtoc;

	private ColumnHeader clMotivo;

	private ToolStripButton tsbBaixarAll;

	private Panel pnEmailData;

	private LinkLabel lkbChangeMyEmail;

	private Label lbMyEmail;

	private Label lbWarning02;

	private Label lbOthersEmail;

	private TextBox txPartnerEmail;

	private CheckBox ckbSendMySelf;

	private Label lbSeparator03;

	private Label label1;

	private TextBox txtSubject;

	private Label lbSeparator02;

	private ToolStripSeparator toolStripSeparator3;

	private CheckBox ckbSendZip;

	private Label label2;

	private ColumnHeader clHasXml;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private ToolStripDropDownButton tslEDITitle;

	private ToolStripMenuItem tsbSendEDI;

	private ToolStripMenuItem tsbBaixarEDI;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripMenuItem tsbSendEDIAll;

	private ToolStripMenuItem tsbBaixarEDIAll;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem tsbSendEDIOthers;

	private ToolStripMenuItem tsbSendEDIXML;

	private ToolStripMenuItem tsbSendEDIPDF;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripMenuItem tsbBaixarEDIXML;

	private ToolStripMenuItem tsbBaixarEDIPDF;

	private Panel pnForm;

	private Panel pnContent;

	private Panel pnTitleBar;

	private PictureBox picTitleBar;

	private Button btHelp;

	private Label lbTitleBar;

	private Button btClose;

	private ImageList ImageListDocs;

	public frmDocSend(List<Document> pDocumentList, List<Event> pEventList, bool pEventLoad = false)
	{
		InitializeComponent();
		if (clsFunction.IsAdmin)
		{
			lbTitleBar.Text = base.Name + " | " + lbTitleBar.Text;
		}
		varDocumentList = pDocumentList;
		varEventList = pEventList;
		varEventLoad = pEventLoad;
	}

	private async void frmDocSend_Load(object sender, EventArgs e)
	{
		lbProgress.Text = "Carregando documentos ...";
		plnMessage.Visible = true;
		Application.DoEvents();
		toolStrip1.Enabled = false;
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		ckbSendMySelf.Checked = clsFunction.funcConvStrToBool(varclsConfig.EmailMySelfSetted);
		lbMyEmail.Text = clsFunction.funcFixEmail(varclsConfig.UserEmail);
		txtSubject.Text = varclsConfig.EmailTitle;
		txPartnerEmail.Text = varclsConfig.EmailAddress;
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		await funcLoadDocsAsync();
		plnMessage.Visible = false;
		Application.DoEvents();
		toolStrip1.Enabled = true;
		ckbSendZip.Checked = !string.IsNullOrEmpty(varclsConfig.SendZipped);
		funcSetFieldsEnabled();
		lstDocs.Columns[0].ImageIndex = 16;
	}

	public async Task funcLoadDocsAsync()
	{
		clsValidatorService varclsValidator = new clsValidatorService();
		new List<Event>();
		lstDocs.Items.Clear();
		new ListViewItem();
		foreach (Document varclsDoc in varDocumentList)
		{
			ListViewItem listViewItem = lstDocs.Items.Add("");
			listViewItem.ImageIndex = clsScreenGeral.funcGetDocXmlIcon(varclsDoc, _HasNFSeFeatEnabled, _HasCFeFeatEnabled);
			listViewItem.Tag = varclsDoc;
			listViewItem.ToolTipText = varclsValidator.funcGetDesc(varclsDoc);
			listViewItem.SubItems.Add(varclsDoc.Num.PadLeft(9, '0'));
			listViewItem.SubItems.Add(varclsDoc.Serie);
			string varDocName = clsFunction.funcGetDocName(varclsDoc.Model);
			listViewItem.SubItems.Add(varDocName);
			listViewItem.ForeColor = Color.Blue;
			listViewItem.SubItems.Add("");
			listViewItem.SubItems.Add("");
			listViewItem.SubItems.Add(varclsDoc.DtAut);
			listViewItem.SubItems.Add(varclsDoc.HrAut);
			listViewItem.SubItems.Add(varclsDoc.Protc);
			listViewItem.SubItems.Add(varclsDoc.xMotivo);
			listViewItem.Checked = true;
			List<Event> varListItens;
			if (varEventLoad)
			{
				varEventList = await new clsDataEvent().funcGetListByDocAsync(varclsDoc);
				varListItens = varEventList;
			}
			else
			{
				varListItens = varEventList.Where((Event r) => r.Chave == varclsDoc.Chave).ToList();
			}
			if (varListItens == null)
			{
				continue;
			}
			foreach (Event varclsEvent in varListItens)
			{
				if (!clsFunction.IsEqual(varclsEvent.tpEvento, "959595") && !clsFunction.IsEqual(varclsEvent.tpEvento, "949494") && !clsFunction.IsEqual(varclsEvent.tpEvento, "858585") && !clsFunction.IsEqual(varclsEvent.tpEvento, "848484") && !clsFunction.IsEqual(varclsEvent.tpEvento, "757575") && !clsFunction.IsEqual(varclsEvent.tpEvento, "848484"))
				{
					ListViewItem listViewItem2 = lstDocs.Items.Add("");
					listViewItem2.Tag = varclsEvent;
					listViewItem2.ImageIndex = 0;
					listViewItem2.SubItems.Add(varclsDoc.Num.PadLeft(9, '0'));
					listViewItem2.SubItems.Add(varclsDoc.Serie);
					listViewItem2.SubItems.Add(varclsEvent.xEvento);
					listViewItem2.SubItems.Add(varclsEvent.tpEvento);
					listViewItem2.SubItems.Add(varclsEvent.nSeqEvento);
					listViewItem2.SubItems.Add(varclsEvent.DtAut);
					listViewItem2.SubItems.Add(varclsEvent.HrAut);
					listViewItem2.SubItems.Add(varclsEvent.Protc);
					listViewItem2.SubItems.Add(varclsEvent.xMotivo);
				}
			}
		}
	}

	private async void lkbChangeMyEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		frmLicense frmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
		frmLicense.ShowDialog(this);
		frmLicense.Dispose();
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		lbMyEmail.Text = varclsConfig.UserEmail;
	}

	private void tsbBaixarAll_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: true, pPdf: true, pEdi: false);
	}

	private async void funcBaixarDocs(bool pXml, bool pPdf, bool pEdi)
	{
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		bool varHasNoXml = false;
		foreach (ListViewItem varCheckedItem in lstDocs.CheckedItems)
		{
			bool varIsDocumentType = false;
			if (typeof(Document).Equals(varCheckedItem.Tag.GetType()))
			{
				varIsDocumentType = true;
			}
			if (varIsDocumentType && string.IsNullOrEmpty(((Document)varCheckedItem.Tag).HasXml))
			{
				varHasNoXml = true;
				break;
			}
		}
		if (varHasNoXml)
		{
			MessageBox.Show("Falta o arquivo digital(XML) para um dos itens selecionados.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		folderDialog = new FolderBrowserDialog();
		if (!string.IsNullOrEmpty(varclsConfig.FolderPath))
		{
			folderDialog.SelectedPath = varclsConfig.FolderPath;
		}
		if (folderDialog.ShowDialog(this) != DialogResult.OK)
		{
			return;
		}
		varclsConfig.FolderPath = folderDialog.SelectedPath;
		await varclsDataConfig.funcUpdateAsync(varclsConfig);
		if (string.IsNullOrEmpty(varclsConfig.FolderPath))
		{
			return;
		}
		folderDialog.Dispose();
		folderDialog = null;
		frmProgress varfrmProgress = new frmProgress("Baixando documento(s) ...");
		varfrmProgress.Show(this);
		varfrmProgress.Activate();
		Application.DoEvents();
		foreach (ListViewItem varCheckedItem2 in lstDocs.CheckedItems)
		{
			bool varIsDocumentType2 = false;
			bool varIsEventType = false;
			if (typeof(Document).Equals(varCheckedItem2.Tag.GetType()))
			{
				varIsDocumentType2 = true;
			}
			if (typeof(Event).Equals(varCheckedItem2.Tag.GetType()))
			{
				varIsEventType = true;
			}
			if (varIsDocumentType2)
			{
				Document varclsDoc = (Document)varCheckedItem2.Tag;
				FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varclsDoc.Filial);
				intXmlObject varXmlObject = new clsXmlFactory().funcGetDocClass(varclsConfig, varclsDoc.Model, pReload: false);
				intPdfObject varPdfObject = new clsPdfFactory().funcGetDocClass(varXmlObject, varclsDoc.Model);
				intEdiObject varEdiObject = new clsEdiFactory().funcGetDocClass(varclsConfig, varXmlObject, varclsDoc.Model);
				string varObjectKey = varclsDoc.Chave;
				if (pXml)
				{
					await varXmlObject.funcDownloadAsync(varclsFilial, varObjectKey, varclsConfig.FolderPath, varclsDoc);
				}
				if (pPdf)
				{
					await varPdfObject.funcDownloadAsync(varclsFilial, varObjectKey, varclsConfig.FolderPath, varclsDoc);
				}
				if (pEdi)
				{
					await varEdiObject.funcDownloadAsync(varclsFilial, varObjectKey, varclsConfig.FolderPath, varclsDoc);
				}
			}
			else if (varIsEventType)
			{
				Event varclsEvent = (Event)varCheckedItem2.Tag;
				Document varclsDoc = await funcGetDocumentAsync(varclsEvent.Chave);
				FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varclsDoc.Filial);
				intXmlObject varXmlObject2 = new clsXmlFactory().funcGetEventClass(varclsConfig, varclsDoc.Model, pReload: false);
				intPdfObject varPdfObject = new clsPdfFactory().funcGetEventClass(varXmlObject2, varclsDoc.Model);
				string varObjectKey = varclsEvent.Chave + "-" + varclsEvent.tpEvento + "-" + varclsEvent.nSeqEvento;
				if (pXml)
				{
					await varXmlObject2.funcDownloadAsync(varclsFilial, varObjectKey, varclsConfig.FolderPath, varclsDoc);
				}
				if (pPdf)
				{
					await varPdfObject.funcDownloadAsync(varclsFilial, varObjectKey, varclsConfig.FolderPath, varclsDoc);
				}
			}
		}
		varfrmProgress.Close();
		varfrmProgress.Dispose();
		MessageBox.Show("Arquivos baixados com sucesso !!!", "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	private async Task<Document> funcGetDocumentAsync(string pChave)
	{
		return await new clsDataDoc().funcGetItemByChaveAsync(pChave);
	}

	private void tsbBaixarXML_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: true, pPdf: false, pEdi: false);
	}

	private void tsbBaixarPDF_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: false, pPdf: true, pEdi: false);
	}

	private void tsbBaixarEDI_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: false, pPdf: false, pEdi: true);
	}

	private void tsbBaixarEDIAll_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: true, pPdf: true, pEdi: true);
	}

	private void ckbSendZip_CheckedChanged(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private async Task<bool> funcValidateEmailListAsync()
	{
		bool varEmailIsValid = true;
		List<string> varEmailList = clsFunction.funcGetMailList(txPartnerEmail.Text);
		if (clsFunction.IsEmpty(txPartnerEmail.Text) || varEmailList.Count == 0)
		{
			MessageBox.Show("Nenhum e-mail de destino foi informado. Favor corrigir", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			varEmailIsValid = false;
		}
		if (!varEmailIsValid)
		{
			return varEmailIsValid;
		}
		foreach (string varEmailValue in varEmailList)
		{
			if (!clsFunction.IsEmpty(varEmailValue) && !clsFunction.funcIsValidEmail(varEmailValue))
			{
				MessageBox.Show("O e-mail [ " + varEmailValue + " ] não é válido. Favor corrigir", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				varEmailIsValid = false;
			}
		}
		if (!varEmailIsValid)
		{
			return varEmailIsValid;
		}
		varEmailList = new List<string>();
		if (!clsFunction.IsEmpty(txPartnerEmail.Text))
		{
			varEmailList.AddRange(clsFunction.funcGetMailList(txPartnerEmail.Text));
		}
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		if (ckbSendMySelf.Checked)
		{
			varEmailList.Add(varclsConfig.UserEmail);
		}
		if (varEmailList.Count <= 0)
		{
			MessageBox.Show("Nenhum e-mail foi identificado para envio das informações.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			varEmailIsValid = false;
		}
		if (!varEmailIsValid)
		{
			return varEmailIsValid;
		}
		if (varEmailList.Count == 1)
		{
			lbProgress.Text = "Verificando se email de destino é valido...";
		}
		else
		{
			lbProgress.Text = "Verificando se emails de destino são validos...";
		}
		plnMessage.Visible = true;
		Application.DoEvents();
		clsReturn varclsReturnFunc = new clsReturn();
		Task<clsReturn> varclsTaskCheckEmail = varclsEmailService.funcCheckEmailAsync(varEmailList);
		if (await varclsTaskCheckEmail.WaitAsync(TimeSpan.FromSeconds(5.0)))
		{
			varclsReturnFunc = varclsTaskCheckEmail.Result;
		}
		plnMessage.Visible = false;
		Application.DoEvents();
		if (varclsReturnFunc.HasError || varclsReturnFunc.HasWarning)
		{
			funcShowErrorMessage(varclsReturnFunc);
			varEmailIsValid = false;
		}
		if (!varEmailIsValid)
		{
			return varEmailIsValid;
		}
		varEmailList = varclsReturnFunc.GetObject<List<string>>("EmailList");
		if (varEmailList == null)
		{
			varEmailList = new List<string>();
		}
		if (varEmailList.Count > 0)
		{
			string varMessage = "Email inválido. Favor corrigir !!!" + Environment.NewLine + Environment.NewLine;
			varEmailIsValid = false;
			foreach (string varclsItem in varEmailList)
			{
				varMessage = varMessage + varclsItem + Environment.NewLine + Environment.NewLine;
			}
			MessageBox.Show(varMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		if (!varEmailIsValid)
		{
			return varEmailIsValid;
		}
		return true;
	}

	private clsEmailDocItem funcGetDocItem(Document pclsDocument, Event pclsEvent)
	{
		clsEmailDocItem varDocItem = new clsEmailDocItem();
		varDocItem.DocNum = pclsDocument.Num;
		varDocItem.Serie = pclsDocument.Serie;
		string varDocName = clsFunction.funcGetDocName(pclsDocument.Model);
		varDocItem.Tipo = varDocName;
		varDocItem.EmitID = pclsDocument.EmitID;
		varDocItem.EmitNome = pclsDocument.EmitNome;
		varDocItem.DtAut = pclsDocument.DtAut;
		varDocItem.HrAut = pclsDocument.HrAut;
		if (pclsEvent == null)
		{
			return varDocItem;
		}
		varDocItem.Tipo = pclsEvent.xEvento;
		varDocItem.tpEvento = pclsEvent.tpEvento;
		varDocItem.nSeqEvento = pclsEvent.nSeqEvento;
		varDocItem.DtAut = pclsEvent.DtAut;
		varDocItem.HrAut = pclsDocument.HrAut;
		return varDocItem;
	}

	public async Task<clsReturn> funcCreateZipAttachmentsAsync(List<List<string>> pFileBatches)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		List<string> varZippedBatches = new List<string>();
		int varZipCount = 1;
		foreach (List<string> varBatch in pFileBatches)
		{
			string varZipFile = await varclsZipService.funcZipContentAsync(varBatch, $"doc-fiscal-io-{varZipCount}.zip");
			if (string.IsNullOrEmpty(varZipFile))
			{
				MessageBox.Show("Erro ao compactar arquivos", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				varclsReturnFunc.AddMessage(new clsMessage("E", "9999", "Erro ao compactar arquivos"));
				return varclsReturnFunc;
			}
			varZipCount++;
			varZippedBatches.Add(varZipFile);
		}
		foreach (List<string> pFileBatch in pFileBatches)
		{
			pFileBatch.Clear();
		}
		pFileBatches.Clear();
		foreach (string varZippedBatch in varZippedBatches)
		{
			pFileBatches.Add(new List<string> { varZippedBatch });
		}
		varclsReturnFunc.AddValue("ObjBatchList", pFileBatches);
		varclsReturnFunc.AddValue("ObjTrashFileList", varZippedBatches);
		return varclsReturnFunc;
	}

	private async Task<clsReturn> funcSendMailAsync(List<List<string>> pFileBatches, List<string> pMailListTo, List<string> pMailListCc, string pSubject, string pEmailBody)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		if (pFileBatches.Count > 1)
		{
			int varCurrentBatch = 1;
			int varTotalBatchs = pFileBatches.Count;
			bool varIsFirstBatch = true;
			foreach (List<string> varBatch in pFileBatches)
			{
				varclsReturnFunc = await varclsEmailService.funcSendEmailAsync(pMailListTo, pMailListCc, $"[{varCurrentBatch}/{varTotalBatchs}]" + pSubject, varIsFirstBatch ? pEmailBody : "", varBatch);
				if (varclsReturnFunc.HasError)
				{
					varclsReturnFunc.AddMessage(new clsMessage("E", "9999", "Erro ao enviar o email"));
					return varclsReturnFunc;
				}
				varIsFirstBatch = false;
				varCurrentBatch++;
			}
		}
		else
		{
			varclsReturnFunc = await varclsEmailService.funcSendEmailAsync(pMailListTo, pMailListCc, pSubject, pEmailBody, pFileBatches[0]);
			if (varclsReturnFunc.HasError)
			{
				varclsReturnFunc.AddMessage(new clsMessage("E", "9999", "Erro ao enviar email"));
				return varclsReturnFunc;
			}
		}
		return varclsReturnFunc;
	}

	private async void funcSendDocsAsync(bool pXml, bool pPdf, bool pEdi, ToolStripButton pTsbButton = null, ToolStripMenuItem pTsmButton = null)
	{
		bool varHasNoXml = false;
		clsReturn varclsReturnFunc = new clsReturn();
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		string varButtonText = string.Empty;
		_ = string.Empty;
		if (pTsbButton != null)
		{
			varButtonText = pTsbButton.Text;
		}
		if (pTsmButton != null)
		{
			varButtonText = pTsmButton.Text;
		}
		string varUserMessage;
		if (clsFunction.IsEmpty(txtSubject.Text))
		{
			varUserMessage = "Informe o assunto do E-mail.";
			MessageBox.Show(varUserMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtSubject.Focus();
			return;
		}
		if (txtSubject.Text.Length > 250)
		{
			varUserMessage = " O campo Assunto deve ter no máximo 250 caracteres.";
			MessageBox.Show(varUserMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtSubject.Focus();
			return;
		}
		if (clsFunction.IsEmpty(txPartnerEmail.Text) && !ckbSendMySelf.Checked)
		{
			varUserMessage = "Nenhum destinatário foi informado para envio de e-mail.";
			MessageBox.Show(varUserMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txPartnerEmail.Focus();
			return;
		}
		if (txPartnerEmail.Text.Length > 500)
		{
			varUserMessage = "O campo E-mail deve ter no máximo 500 caracteres.";
			MessageBox.Show(varUserMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txPartnerEmail.Focus();
			return;
		}
		if (!(await funcValidateEmailListAsync()))
		{
			txPartnerEmail.Focus();
			return;
		}
		List<Document> varDocList = new List<Document>();
		foreach (ListViewItem varCheckedItem in lstDocs.CheckedItems)
		{
			bool varIsDocType = false;
			if (typeof(Document).Equals(varCheckedItem.Tag.GetType()))
			{
				varIsDocType = true;
			}
			if (varIsDocType)
			{
				Document varclsDoc = (Document)varCheckedItem.Tag;
				if (clsFunction.IsEmpty(varclsDoc.HasXml))
				{
					varHasNoXml = true;
					break;
				}
				varDocList.Add(varclsDoc);
			}
		}
		if (varHasNoXml)
		{
			varUserMessage = "Falta o arquivo digital(XML) para um dos itens selecionados.";
			MessageBox.Show(varUserMessage, "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		varUserMessage = string.Empty;
		if (pEdi && !varDocList.Any((Document r) => clsFunction.funcIsNFe(r.Model) || clsFunction.funcIsCTe(r.Model)))
		{
			varUserMessage = "Nenhum (NFe ou CTe) selecionado para geração do arquivo EDI CONEMB ou NOTFIS.";
		}
		if (!clsFunction.IsEmpty(varUserMessage))
		{
			MessageBox.Show(varUserMessage, "Operação cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		_ = string.Empty;
		List<string> varMailListTo = new List<string>();
		List<string> varMailListCc = new List<string>();
		if (!clsFunction.IsEmpty(txPartnerEmail.Text))
		{
			varMailListTo.AddRange(clsFunction.funcGetMailList(txPartnerEmail.Text));
		}
		if (ckbSendMySelf.Checked && varMailListTo.Count <= 0)
		{
			varMailListTo.Add(varclsConfig.UserEmail);
		}
		else if (ckbSendMySelf.Checked)
		{
			varMailListCc.Add(varclsConfig.UserEmail);
		}
		if (pTsbButton != null)
		{
			pTsbButton.Text = "Enviando ...";
			pTsbButton.Enabled = false;
		}
		if (pTsmButton != null)
		{
			pTsmButton.Text = "Enviando ...";
			pTsmButton.Enabled = false;
		}
		lbProgress.Text = "Preparando arquivo(s) a enviar ...";
		plnMessage.Visible = true;
		Application.DoEvents();
		List<string> varEmailFileList = new List<string>();
		List<clsEmailDocItem> varEmailDocList = new List<clsEmailDocItem>();
		try
		{
			foreach (ListViewItem varCheckedItem2 in lstDocs.CheckedItems)
			{
				bool varIsDocumentType = false;
				bool varIsEventType = false;
				if (typeof(Document).Equals(varCheckedItem2.Tag.GetType()))
				{
					varIsDocumentType = true;
				}
				if (typeof(Event).Equals(varCheckedItem2.Tag.GetType()))
				{
					varIsEventType = true;
				}
				string varXmlFileName = string.Empty;
				string varPdfFileName = string.Empty;
				string varEdiFileName = string.Empty;
				if (varIsDocumentType)
				{
					Document varclsDoc2 = (Document)varCheckedItem2.Tag;
					FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varclsDoc2.Filial);
					intXmlObject varXmlObject = new clsXmlFactory().funcGetDocClass(varclsConfig, varclsDoc2.Model, pReload: false);
					intPdfObject varPdfObject = new clsPdfFactory().funcGetDocClass(varXmlObject, varclsDoc2.Model);
					intEdiObject varEdiObject = new clsEdiFactory().funcGetDocClass(varclsConfig, varXmlObject, varclsDoc2.Model);
					string varObjectKey = varclsDoc2.Chave;
					varEmailDocList.Add(funcGetDocItem(varclsDoc2, null));
					try
					{
						if (pXml && varXmlObject != null)
						{
							varXmlFileName = await varXmlObject.funcGetBinaryFileAsync(varclsFilial, varObjectKey, varclsDoc2);
						}
						if (pPdf && varPdfObject != null)
						{
							await varPdfObject.funcGenerateAsync(varclsFilial, varObjectKey, varclsDoc2);
							varPdfFileName = varPdfObject.funcGetFilePath(varclsFilial, varObjectKey);
						}
						if (pEdi && varEdiObject != null)
						{
							varEdiFileName = (await varEdiObject.funcGenerateAsync(varclsFilial, null, varObjectKey, varclsDoc2)).GetValue("FILEPATH");
						}
					}
					catch (Exception)
					{
						if (clsFunction.IsAdmin)
						{
							throw;
						}
					}
				}
				else if (varIsEventType)
				{
					Event varclsEvent = (Event)varCheckedItem2.Tag;
					Document varclsDoc2 = await funcGetDocumentAsync(varclsEvent.Chave);
					FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varclsDoc2.Filial);
					intXmlObject varXmlObject2 = new clsXmlFactory().funcGetEventClass(varclsConfig, varclsDoc2.Model, pReload: false);
					intPdfObject varPdfObject = new clsPdfFactory().funcGetEventClass(varXmlObject2, varclsDoc2.Model);
					string varObjectKey = varclsEvent.Chave + "-" + varclsEvent.tpEvento + "-" + varclsEvent.nSeqEvento;
					varEmailDocList.Add(funcGetDocItem(varclsDoc2, varclsEvent));
					try
					{
						if (pXml)
						{
							varXmlFileName = await varXmlObject2.funcGetBinaryFileAsync(varclsFilial, varObjectKey, varclsDoc2);
						}
						if (pPdf)
						{
							await varPdfObject.funcGenerateAsync(varclsFilial, varObjectKey, varclsDoc2);
							varPdfFileName = varPdfObject.funcGetFilePath(varclsFilial, varObjectKey);
						}
					}
					catch (Exception)
					{
						if (clsFunction.IsAdmin)
						{
							throw;
						}
					}
				}
				if (!string.IsNullOrEmpty(varXmlFileName))
				{
					varEmailFileList.Add(varXmlFileName);
				}
				if (!string.IsNullOrEmpty(varPdfFileName))
				{
					varEmailFileList.Add(varPdfFileName);
				}
				if (!string.IsNullOrEmpty(varEdiFileName))
				{
					varEmailFileList.Add(varEdiFileName);
				}
			}
			List<string> varTrashFileList = new List<string>();
			varTrashFileList.AddRange(varEmailFileList);
			clsReturn varFileBatchesReturn = funcCreateFileBatches(varEmailFileList, 10485760);
			if (varFileBatchesReturn == null || varFileBatchesReturn.HasError)
			{
				MessageBox.Show("Erro ao criar lotes de email", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			List<List<string>> varFileBatches = (List<List<string>>)varFileBatchesReturn.GetObject("ObjBatchList");
			if (ckbSendZip.Checked)
			{
				clsReturn varClsReturn = await funcCreateZipAttachmentsAsync(varFileBatches);
				if (varFileBatchesReturn == null || varClsReturn.HasError)
				{
					MessageBox.Show("Erro ao criar lotes de email", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				varFileBatches = (List<List<string>>)varClsReturn.GetObject("ObjBatchList");
				List<string> varTrashZip = (List<string>)varClsReturn.GetObject("ObjTrashFileList");
				varTrashFileList.AddRange(varTrashZip);
			}
			clsEmailTemplate varclsEmailTemplate = new clsEmailTemplate(varclsConfig);
			if (varclsEmailTemplate == null)
			{
				return;
			}
			string varTemplate = await varclsEmailTemplate.funcGetTemplateAsync("GENERIC");
			if (string.IsNullOrEmpty(varTemplate))
			{
				return;
			}
			string varFileFormat = string.Empty;
			if (pXml)
			{
				varFileFormat += "XML, ";
			}
			if (pPdf)
			{
				varFileFormat += "PDF, ";
			}
			if (pEdi)
			{
				varFileFormat += "EDI, ";
			}
			varFileFormat = clsFunction.funcClearEnd(varFileFormat, ", ");
			string varEmailBody = varclsEmailTemplate.funcFillGenericData(varTemplate, varFileFormat, varEmailDocList);
			if (string.IsNullOrEmpty(varTemplate))
			{
				return;
			}
			if (pTsbButton != null)
			{
				pTsbButton.Text = "Enviado ...";
				pTsbButton.Enabled = false;
			}
			if (pTsmButton != null)
			{
				pTsmButton.Text = "Enviado ...";
				pTsmButton.Enabled = false;
			}
			lbProgress.Text = "Enviando documento(s) por e-mail ...";
			plnMessage.Visible = true;
			Application.DoEvents();
			string varSubject = txtSubject.Text;
			varclsReturnFunc = await funcSendMailAsync(varFileBatches, varMailListTo, varMailListCc, varSubject, varEmailBody);
			if (varclsReturnFunc.HasError)
			{
				varclsReturnFunc.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos."));
				funcShowErrorMessage(varclsReturnFunc);
				if (pTsbButton != null)
				{
					pTsbButton.Text = varButtonText;
					pTsbButton.Enabled = true;
				}
				if (pTsmButton != null)
				{
					pTsmButton.Text = varButtonText;
					pTsmButton.Enabled = true;
				}
				plnMessage.Visible = false;
				Application.DoEvents();
				return;
			}
			varclsConfig.EmailForDocs = (clsFunction.funcConvStrToInt(varclsConfig.EmailForDocs) + varFileBatches.Count()).ToString();
			await new clsDataConfig().funcUpdateAsync(varclsConfig);
			clsDataGeral.funcReleaseBinary(varclsConfig, varTrashFileList);
			if (pTsbButton != null)
			{
				pTsbButton.Text = varButtonText;
				pTsbButton.Enabled = true;
			}
			if (pTsmButton != null)
			{
				pTsmButton.Text = varButtonText;
				pTsmButton.Enabled = true;
			}
			plnMessage.Visible = false;
			Application.DoEvents();
			varUserMessage = "Documento(s) enviado(s) com sucesso !!!";
			MessageBox.Show(varUserMessage, "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Close();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos.", pException));
			funcShowErrorMessage(varclsReturnFunc);
			if (pTsbButton != null)
			{
				pTsbButton.Text = varButtonText;
				pTsbButton.Enabled = true;
			}
			if (pTsmButton != null)
			{
				pTsmButton.Text = varButtonText;
				pTsmButton.Enabled = true;
			}
			plnMessage.Visible = false;
			Application.DoEvents();
			return;
		}
		if (pTsbButton != null)
		{
			pTsbButton.Text = varButtonText;
			pTsbButton.Enabled = true;
		}
		if (pTsmButton != null)
		{
			pTsmButton.Text = varButtonText;
			pTsmButton.Enabled = true;
		}
		plnMessage.Visible = false;
		Application.DoEvents();
	}

	private clsReturn funcCreateFileBatches(List<string> pFiles, int pMaxSizeInBytes)
	{
		List<List<string>> varBatches = new List<List<string>>();
		List<string> varCurrentBatch = new List<string>();
		int varCurrentSize = 0;
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			foreach (string file in pFiles)
			{
				if (File.Exists(file))
				{
					long varFileSize = new FileInfo(file).Length;
					if (varCurrentSize + varFileSize > pMaxSizeInBytes)
					{
						varBatches.Add(new List<string>(varCurrentBatch));
						varCurrentBatch.Clear();
						varCurrentSize = 0;
					}
					varCurrentBatch.Add(file);
					varCurrentSize += (int)varFileSize;
				}
			}
			if (varCurrentBatch.Count > 0)
			{
				varBatches.Add(varCurrentBatch);
			}
		}
		catch (Exception ex)
		{
			varclsReturnFunc.AddMessage(new clsMessage("E", "9999", ex.Message, ex));
		}
		varclsReturnFunc.AddValue("ObjBatchList", varBatches);
		return varclsReturnFunc;
	}

	private void funcShowErrorMessage(clsReturn pclsReturn)
	{
		if (base.Visible && pclsReturn != null && (pclsReturn.HasError || pclsReturn.HasWarning))
		{
			clsScreenGeral.funcShowUserMessage(this, pclsReturn);
		}
	}

	private void tsbSendAll_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: true, pPdf: true, pEdi: false, tsbSendAll);
	}

	private void tsbSendXML_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: true, pPdf: false, pEdi: false, null, tsbSendXML);
	}

	private void tsbSendPDF_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: false, pPdf: true, pEdi: false, null, tsbSendPDF);
	}

	private void tsbSendEDI_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: false, pPdf: false, pEdi: true, null, tsbSendEDI);
	}

	private void tsbSendEDIAll_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: true, pPdf: true, pEdi: true, null, tsbSendEDIAll);
	}

	private void ckbSendMySelf_CheckedChanged(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private async void funcSaveUserEmailDataPreferencesAsync()
	{
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		varclsConfig.EmailMySelfSetted = (ckbSendMySelf.Checked ? "X" : "");
		varclsConfig.SendZipped = (ckbSendZip.Checked ? "X" : "");
		varclsConfig.EmailTitle = txtSubject.Text;
		varclsConfig.EmailAddress = txPartnerEmail.Text;
		await new clsDataConfig().funcUpdateAsync(varclsConfig);
	}

	private void funcSetFieldsEnabled()
	{
		lbMyEmail.Enabled = ckbSendMySelf.Checked;
		lkbChangeMyEmail.Enabled = ckbSendMySelf.Checked;
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

	private void txtSubject_Validated(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
	}

	private void txPartnerEmail_Validated(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
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

	private void tsbSendEDIXML_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: true, pPdf: false, pEdi: true, null, tsbSendEDIXML);
	}

	private void tsbSendEDIPDF_Click(object sender, EventArgs e)
	{
		funcSendDocsAsync(pXml: false, pPdf: true, pEdi: true, null, tsbSendEDIXML);
	}

	private void tsbBaixarEDIXML_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: true, pPdf: false, pEdi: true);
	}

	private void tsbBaixarEDIPDF_Click(object sender, EventArgs e)
	{
		funcBaixarDocs(pXml: false, pPdf: true, pEdi: true);
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void lbTitleBar_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			clsScreenGeral.funcFormMouseMove(base.Handle);
		}
	}

	private void frmDocSend_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		clsHelpService.funcCallEnviarHelp();
	}

	private void btHelp_Click(object sender, EventArgs e)
	{
		clsHelpService.funcCallEnviarHelp();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmDocSend));
		this.splitList = new System.Windows.Forms.SplitContainer();
		this.pnEmailData = new System.Windows.Forms.Panel();
		this.plnMessage = new System.Windows.Forms.Panel();
		this.lbProgress = new System.Windows.Forms.Label();
		this.lbProgress01 = new System.Windows.Forms.Label();
		this.picProgress = new System.Windows.Forms.PictureBox();
		this.ckbSendZip = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.lbSeparator02 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtSubject = new System.Windows.Forms.TextBox();
		this.lkbChangeMyEmail = new System.Windows.Forms.LinkLabel();
		this.lbMyEmail = new System.Windows.Forms.Label();
		this.lbWarning02 = new System.Windows.Forms.Label();
		this.lbOthersEmail = new System.Windows.Forms.Label();
		this.txPartnerEmail = new System.Windows.Forms.TextBox();
		this.ckbSendMySelf = new System.Windows.Forms.CheckBox();
		this.lbSeparator03 = new System.Windows.Forms.Label();
		this.lstDocs = new System.Windows.Forms.ListView();
		this.clHasXml = new System.Windows.Forms.ColumnHeader();
		this.clDoc = new System.Windows.Forms.ColumnHeader();
		this.clSerie = new System.Windows.Forms.ColumnHeader();
		this.clTipo = new System.Windows.Forms.ColumnHeader();
		this.clEvent = new System.Windows.Forms.ColumnHeader();
		this.clSeq = new System.Windows.Forms.ColumnHeader();
		this.clData = new System.Windows.Forms.ColumnHeader();
		this.clHora = new System.Windows.Forms.ColumnHeader();
		this.clProtoc = new System.Windows.Forms.ColumnHeader();
		this.clMotivo = new System.Windows.Forms.ColumnHeader();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.tsbSendAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tslXMLTitle = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbSendXML = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarXML = new System.Windows.Forms.ToolStripMenuItem();
		this.tslPDFTitle = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbSendPDF = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarPDF = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.tslEDITitle = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbSendEDI = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarEDI = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSendEDIAll = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarEDIAll = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSendEDIOthers = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbSendEDIXML = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbSendEDIPDF = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarEDIXML = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarEDIPDF = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
		this.pnForm = new System.Windows.Forms.Panel();
		this.pnContent = new System.Windows.Forms.Panel();
		this.pnTitleBar = new System.Windows.Forms.Panel();
		this.picTitleBar = new System.Windows.Forms.PictureBox();
		this.btHelp = new System.Windows.Forms.Button();
		this.btClose = new System.Windows.Forms.Button();
		this.lbTitleBar = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.splitList).BeginInit();
		this.splitList.Panel1.SuspendLayout();
		this.splitList.Panel2.SuspendLayout();
		this.splitList.SuspendLayout();
		this.pnEmailData.SuspendLayout();
		this.plnMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.pnForm.SuspendLayout();
		this.pnContent.SuspendLayout();
		this.pnTitleBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).BeginInit();
		base.SuspendLayout();
		this.splitList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitList.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitList.Location = new System.Drawing.Point(0, 0);
		this.splitList.Name = "splitList";
		this.splitList.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitList.Panel1.Controls.Add(this.pnEmailData);
		this.splitList.Panel1MinSize = 113;
		this.splitList.Panel2.Controls.Add(this.lstDocs);
		this.splitList.Panel2.Controls.Add(this.toolStrip1);
		this.splitList.Size = new System.Drawing.Size(1044, 478);
		this.splitList.SplitterDistance = 113;
		this.splitList.TabIndex = 2;
		this.pnEmailData.Controls.Add(this.plnMessage);
		this.pnEmailData.Controls.Add(this.ckbSendZip);
		this.pnEmailData.Controls.Add(this.label2);
		this.pnEmailData.Controls.Add(this.lbSeparator02);
		this.pnEmailData.Controls.Add(this.label1);
		this.pnEmailData.Controls.Add(this.txtSubject);
		this.pnEmailData.Controls.Add(this.lkbChangeMyEmail);
		this.pnEmailData.Controls.Add(this.lbMyEmail);
		this.pnEmailData.Controls.Add(this.lbWarning02);
		this.pnEmailData.Controls.Add(this.lbOthersEmail);
		this.pnEmailData.Controls.Add(this.txPartnerEmail);
		this.pnEmailData.Controls.Add(this.ckbSendMySelf);
		this.pnEmailData.Controls.Add(this.lbSeparator03);
		this.pnEmailData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnEmailData.Location = new System.Drawing.Point(0, 0);
		this.pnEmailData.Name = "pnEmailData";
		this.pnEmailData.Size = new System.Drawing.Size(1044, 113);
		this.pnEmailData.TabIndex = 101;
		this.plnMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.plnMessage.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.plnMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.plnMessage.Controls.Add(this.lbProgress);
		this.plnMessage.Controls.Add(this.lbProgress01);
		this.plnMessage.Controls.Add(this.picProgress);
		this.plnMessage.Location = new System.Drawing.Point(0, 75);
		this.plnMessage.Name = "plnMessage";
		this.plnMessage.Size = new System.Drawing.Size(1038, 38);
		this.plnMessage.TabIndex = 104;
		this.plnMessage.Visible = false;
		this.lbProgress.AutoSize = true;
		this.lbProgress.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbProgress.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress.Location = new System.Drawing.Point(39, 11);
		this.lbProgress.Name = "lbProgress";
		this.lbProgress.Size = new System.Drawing.Size(15, 13);
		this.lbProgress.TabIndex = 103;
		this.lbProgress.Text = "..";
		this.lbProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbProgress01.AutoSize = true;
		this.lbProgress01.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbProgress01.ForeColor = System.Drawing.Color.DodgerBlue;
		this.lbProgress01.Location = new System.Drawing.Point(39, 3);
		this.lbProgress01.Name = "lbProgress01";
		this.lbProgress01.Size = new System.Drawing.Size(15, 13);
		this.lbProgress01.TabIndex = 101;
		this.lbProgress01.Text = "..";
		this.lbProgress01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picProgress.Image = Monitor.Resources.gif_loading;
		this.picProgress.Location = new System.Drawing.Point(4, 3);
		this.picProgress.Name = "picProgress";
		this.picProgress.Size = new System.Drawing.Size(30, 30);
		this.picProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picProgress.TabIndex = 1;
		this.picProgress.TabStop = false;
		this.ckbSendZip.AutoSize = true;
		this.ckbSendZip.Checked = true;
		this.ckbSendZip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckbSendZip.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ckbSendZip.Location = new System.Drawing.Point(602, 84);
		this.ckbSendZip.Name = "ckbSendZip";
		this.ckbSendZip.Size = new System.Drawing.Size(321, 18);
		this.ckbSendZip.TabIndex = 4;
		this.ckbSendZip.Text = "Enviar arquivo(s) em formato compactado [zip]";
		this.ckbSendZip.UseVisualStyleBackColor = true;
		this.ckbSendZip.CheckedChanged += new System.EventHandler(ckbSendZip_CheckedChanged);
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(582, 76);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(456, 5);
		this.label2.TabIndex = 125;
		this.lbSeparator02.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbSeparator02.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator02.Location = new System.Drawing.Point(10, 106);
		this.lbSeparator02.Name = "lbSeparator02";
		this.lbSeparator02.Size = new System.Drawing.Size(1028, 5);
		this.lbSeparator02.TabIndex = 124;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(71, 14);
		this.label1.TabIndex = 123;
		this.label1.Text = "Assunto : ";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubject.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubject.Location = new System.Drawing.Point(85, 15);
		this.txtSubject.MaxLength = 250;
		this.txtSubject.Name = "txtSubject";
		this.txtSubject.Size = new System.Drawing.Size(492, 22);
		this.txtSubject.TabIndex = 0;
		this.txtSubject.Validated += new System.EventHandler(txtSubject_Validated);
		this.lkbChangeMyEmail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lkbChangeMyEmail.Location = new System.Drawing.Point(624, 54);
		this.lkbChangeMyEmail.Name = "lkbChangeMyEmail";
		this.lkbChangeMyEmail.Size = new System.Drawing.Size(377, 18);
		this.lkbChangeMyEmail.TabIndex = 3;
		this.lkbChangeMyEmail.TabStop = true;
		this.lkbChangeMyEmail.Text = "Corrigir meu e-mail";
		this.lkbChangeMyEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbChangeMyEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbChangeMyEmail_LinkClicked);
		this.lbMyEmail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbMyEmail.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
		this.lbMyEmail.Location = new System.Drawing.Point(624, 35);
		this.lbMyEmail.Name = "lbMyEmail";
		this.lbMyEmail.Size = new System.Drawing.Size(377, 16);
		this.lbMyEmail.TabIndex = 120;
		this.lbMyEmail.Text = "lucas@fiscal.io";
		this.lbMyEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lbWarning02.AutoSize = true;
		this.lbWarning02.Font = new System.Drawing.Font("Verdana", 8f);
		this.lbWarning02.Location = new System.Drawing.Point(83, 85);
		this.lbWarning02.Name = "lbWarning02";
		this.lbWarning02.Size = new System.Drawing.Size(474, 13);
		this.lbWarning02.TabIndex = 119;
		this.lbWarning02.Text = "Para indicar mais que um endereço de e-mail, separe-os com ponto e vírgula (;).";
		this.lbOthersEmail.AutoSize = true;
		this.lbOthersEmail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbOthersEmail.Location = new System.Drawing.Point(10, 44);
		this.lbOthersEmail.Name = "lbOthersEmail";
		this.lbOthersEmail.Size = new System.Drawing.Size(71, 14);
		this.lbOthersEmail.TabIndex = 118;
		this.lbOthersEmail.Text = "E-mail(s) :";
		this.lbOthersEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txPartnerEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txPartnerEmail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txPartnerEmail.Location = new System.Drawing.Point(85, 41);
		this.txPartnerEmail.MaxLength = 500;
		this.txPartnerEmail.Multiline = true;
		this.txPartnerEmail.Name = "txPartnerEmail";
		this.txPartnerEmail.Size = new System.Drawing.Size(492, 41);
		this.txPartnerEmail.TabIndex = 1;
		this.txPartnerEmail.Validated += new System.EventHandler(txPartnerEmail_Validated);
		this.ckbSendMySelf.AutoSize = true;
		this.ckbSendMySelf.Checked = true;
		this.ckbSendMySelf.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ckbSendMySelf.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ckbSendMySelf.Location = new System.Drawing.Point(602, 16);
		this.ckbSendMySelf.Name = "ckbSendMySelf";
		this.ckbSendMySelf.Size = new System.Drawing.Size(301, 18);
		this.ckbSendMySelf.TabIndex = 2;
		this.ckbSendMySelf.Text = "Também quero receber uma cópia do e-mail";
		this.ckbSendMySelf.UseVisualStyleBackColor = true;
		this.ckbSendMySelf.CheckedChanged += new System.EventHandler(ckbSendMySelf_CheckedChanged);
		this.lbSeparator03.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbSeparator03.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbSeparator03.Location = new System.Drawing.Point(10, 6);
		this.lbSeparator03.Name = "lbSeparator03";
		this.lbSeparator03.Size = new System.Drawing.Size(1028, 5);
		this.lbSeparator03.TabIndex = 109;
		this.lstDocs.CheckBoxes = true;
		this.lstDocs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[10] { this.clHasXml, this.clDoc, this.clSerie, this.clTipo, this.clEvent, this.clSeq, this.clData, this.clHora, this.clProtoc, this.clMotivo });
		this.lstDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lstDocs.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lstDocs.FullRowSelect = true;
		this.lstDocs.HideSelection = false;
		this.lstDocs.Location = new System.Drawing.Point(0, 25);
		this.lstDocs.MultiSelect = false;
		this.lstDocs.Name = "lstDocs";
		this.lstDocs.ShowItemToolTips = true;
		this.lstDocs.Size = new System.Drawing.Size(1044, 336);
		this.lstDocs.SmallImageList = this.ImageListDocs;
		this.lstDocs.TabIndex = 6;
		this.lstDocs.UseCompatibleStateImageBehavior = false;
		this.lstDocs.View = System.Windows.Forms.View.Details;
		this.lstDocs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lstDocs_ColumnClick);
		this.lstDocs.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lstDocs_ItemChecked);
		this.clHasXml.Text = "";
		this.clHasXml.Width = 43;
		this.clDoc.Text = "Documento";
		this.clDoc.Width = 80;
		this.clSerie.Text = "Serie";
		this.clSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clSerie.Width = 45;
		this.clTipo.Text = "Tipo";
		this.clTipo.Width = 200;
		this.clEvent.Text = "Evento";
		this.clEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clEvent.Width = 75;
		this.clSeq.Text = "Seq.";
		this.clSeq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clSeq.Width = 38;
		this.clData.Text = "Data";
		this.clData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clData.Width = 80;
		this.clHora.Text = "Hora";
		this.clHora.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clHora.Width = 65;
		this.clProtoc.Text = "Protocolo";
		this.clProtoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clProtoc.Width = 120;
		this.clMotivo.Text = "Status";
		this.clMotivo.Width = 280;
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.tsbSendAll, this.toolStripSeparator3, this.tslXMLTitle, this.tslPDFTitle, this.toolStripSeparator1, this.tsbBaixarAll, this.toolStripSeparator4, this.tslEDITitle, this.toolStripSeparator2 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1044, 25);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.tsbSendAll.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.tsbSendAll.Image = (System.Drawing.Image)resources.GetObject("tsbSendAll.Image");
		this.tsbSendAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSendAll.Name = "tsbSendAll";
		this.tsbSendAll.Size = new System.Drawing.Size(208, 22);
		this.tsbSendAll.Text = "Enviar agora [ XML + PDF ]";
		this.tsbSendAll.Click += new System.EventHandler(tsbSendAll_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.tslXMLTitle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsbSendXML, this.tsbBaixarXML });
		this.tslXMLTitle.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.tslXMLTitle.Image = Monitor.Resources.image_xml_blue;
		this.tslXMLTitle.Name = "tslXMLTitle";
		this.tslXMLTitle.Size = new System.Drawing.Size(60, 22);
		this.tslXMLTitle.Text = "XML";
		this.tsbSendXML.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbSendXML.Image = (System.Drawing.Image)resources.GetObject("tsbSendXML.Image");
		this.tsbSendXML.Name = "tsbSendXML";
		this.tsbSendXML.Size = new System.Drawing.Size(179, 22);
		this.tsbSendXML.Text = "Enviar por email";
		this.tsbSendXML.Click += new System.EventHandler(tsbSendXML_Click);
		this.tsbBaixarXML.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarXML.Image = Monitor.Resources.image_xml_blue;
		this.tsbBaixarXML.Name = "tsbBaixarXML";
		this.tsbBaixarXML.Size = new System.Drawing.Size(179, 22);
		this.tsbBaixarXML.Text = "Exportar arquivo";
		this.tsbBaixarXML.Click += new System.EventHandler(tsbBaixarXML_Click);
		this.tslPDFTitle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsbSendPDF, this.tsbBaixarPDF });
		this.tslPDFTitle.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.tslPDFTitle.Image = Monitor.Resources.image_pdf_file;
		this.tslPDFTitle.Name = "tslPDFTitle";
		this.tslPDFTitle.Size = new System.Drawing.Size(57, 22);
		this.tslPDFTitle.Text = "PDF";
		this.tsbSendPDF.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbSendPDF.Image = (System.Drawing.Image)resources.GetObject("tsbSendPDF.Image");
		this.tsbSendPDF.Name = "tsbSendPDF";
		this.tsbSendPDF.Size = new System.Drawing.Size(179, 22);
		this.tsbSendPDF.Text = "Enviar por email";
		this.tsbSendPDF.Click += new System.EventHandler(tsbSendPDF_Click);
		this.tsbBaixarPDF.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarPDF.Image = Monitor.Resources.image_pdf_file;
		this.tsbBaixarPDF.Name = "tsbBaixarPDF";
		this.tsbBaixarPDF.Size = new System.Drawing.Size(179, 22);
		this.tsbBaixarPDF.Text = "Exportar arquivo";
		this.tsbBaixarPDF.Click += new System.EventHandler(tsbBaixarPDF_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tsbBaixarAll.Image = Monitor.Resources.image_download;
		this.tsbBaixarAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbBaixarAll.Name = "tsbBaixarAll";
		this.tsbBaixarAll.Size = new System.Drawing.Size(151, 22);
		this.tsbBaixarAll.Text = "Exportar XML + PDF";
		this.tsbBaixarAll.Click += new System.EventHandler(tsbBaixarAll_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.tslEDITitle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.tsbSendEDI, this.tsbBaixarEDI, this.toolStripSeparator5, this.tsbSendEDIAll, this.tsbBaixarEDIAll, this.toolStripSeparator6, this.tsbSendEDIOthers });
		this.tslEDITitle.Image = Monitor.Resources.image_ediproceda;
		this.tslEDITitle.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tslEDITitle.Name = "tslEDITitle";
		this.tslEDITitle.Size = new System.Drawing.Size(58, 22);
		this.tslEDITitle.Tag = "#NOT-TABPARTNER";
		this.tslEDITitle.Text = "EDI";
		this.tsbSendEDI.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbSendEDI.Image = (System.Drawing.Image)resources.GetObject("tsbSendEDI.Image");
		this.tsbSendEDI.Name = "tsbSendEDI";
		this.tsbSendEDI.Size = new System.Drawing.Size(237, 22);
		this.tsbSendEDI.Text = "Enviar por email";
		this.tsbSendEDI.Click += new System.EventHandler(tsbSendEDI_Click);
		this.tsbBaixarEDI.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarEDI.Image = (System.Drawing.Image)resources.GetObject("tsbBaixarEDI.Image");
		this.tsbBaixarEDI.Name = "tsbBaixarEDI";
		this.tsbBaixarEDI.Size = new System.Drawing.Size(237, 22);
		this.tsbBaixarEDI.Text = "Exportar arquivo";
		this.tsbBaixarEDI.Click += new System.EventHandler(tsbBaixarEDI_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(234, 6);
		this.tsbSendEDIAll.Image = Monitor.Resources.image_email;
		this.tsbSendEDIAll.Name = "tsbSendEDIAll";
		this.tsbSendEDIAll.Size = new System.Drawing.Size(237, 22);
		this.tsbSendEDIAll.Text = "Enviar EDI + XML + PDF";
		this.tsbSendEDIAll.Click += new System.EventHandler(tsbSendEDIAll_Click);
		this.tsbBaixarEDIAll.Image = Monitor.Resources.image_download;
		this.tsbBaixarEDIAll.Name = "tsbBaixarEDIAll";
		this.tsbBaixarEDIAll.Size = new System.Drawing.Size(237, 22);
		this.tsbBaixarEDIAll.Text = "Exportar EDI + XML + PDF";
		this.tsbBaixarEDIAll.Click += new System.EventHandler(tsbBaixarEDIAll_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(234, 6);
		this.tsbSendEDIOthers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsbSendEDIXML, this.tsbSendEDIPDF, this.toolStripSeparator7, this.tsbBaixarEDIXML, this.tsbBaixarEDIPDF });
		this.tsbSendEDIOthers.Name = "tsbSendEDIOthers";
		this.tsbSendEDIOthers.Size = new System.Drawing.Size(237, 22);
		this.tsbSendEDIOthers.Text = "Outras opções ...";
		this.tsbSendEDIXML.Name = "tsbSendEDIXML";
		this.tsbSendEDIXML.Size = new System.Drawing.Size(196, 22);
		this.tsbSendEDIXML.Text = "Enviar EDI + XML";
		this.tsbSendEDIXML.Click += new System.EventHandler(tsbSendEDIXML_Click);
		this.tsbSendEDIPDF.Name = "tsbSendEDIPDF";
		this.tsbSendEDIPDF.Size = new System.Drawing.Size(196, 22);
		this.tsbSendEDIPDF.Text = "Enviar EDI + PDF";
		this.tsbSendEDIPDF.Click += new System.EventHandler(tsbSendEDIPDF_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(193, 6);
		this.tsbBaixarEDIXML.Name = "tsbBaixarEDIXML";
		this.tsbBaixarEDIXML.Size = new System.Drawing.Size(196, 22);
		this.tsbBaixarEDIXML.Text = "Exportar EDI + XML";
		this.tsbBaixarEDIXML.Click += new System.EventHandler(tsbBaixarEDIXML_Click);
		this.tsbBaixarEDIPDF.Name = "tsbBaixarEDIPDF";
		this.tsbBaixarEDIPDF.Size = new System.Drawing.Size(196, 22);
		this.tsbBaixarEDIPDF.Text = "Exportar EDI + PDF";
		this.tsbBaixarEDIPDF.Click += new System.EventHandler(tsbBaixarEDIPDF_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.folderDialog.RootFolder = System.Environment.SpecialFolder.DesktopDirectory;
		this.pnForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnForm.Controls.Add(this.pnContent);
		this.pnForm.Controls.Add(this.pnTitleBar);
		this.pnForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnForm.Location = new System.Drawing.Point(0, 0);
		this.pnForm.Name = "pnForm";
		this.pnForm.Size = new System.Drawing.Size(1058, 513);
		this.pnForm.TabIndex = 10;
		this.pnContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnContent.Controls.Add(this.splitList);
		this.pnContent.Location = new System.Drawing.Point(6, 30);
		this.pnContent.Name = "pnContent";
		this.pnContent.Size = new System.Drawing.Size(1044, 478);
		this.pnContent.TabIndex = 8;
		this.pnTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pnTitleBar.BackColor = System.Drawing.Color.White;
		this.pnTitleBar.Controls.Add(this.picTitleBar);
		this.pnTitleBar.Controls.Add(this.btHelp);
		this.pnTitleBar.Controls.Add(this.btClose);
		this.pnTitleBar.Controls.Add(this.lbTitleBar);
		this.pnTitleBar.Location = new System.Drawing.Point(0, 0);
		this.pnTitleBar.Name = "pnTitleBar";
		this.pnTitleBar.Size = new System.Drawing.Size(1057, 31);
		this.pnTitleBar.TabIndex = 2;
		this.picTitleBar.Image = Monitor.Resources.image_favicon;
		this.picTitleBar.Location = new System.Drawing.Point(5, 4);
		this.picTitleBar.Name = "picTitleBar";
		this.picTitleBar.Size = new System.Drawing.Size(23, 23);
		this.picTitleBar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picTitleBar.TabIndex = 3;
		this.picTitleBar.TabStop = false;
		this.btHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btHelp.BackColor = System.Drawing.Color.Transparent;
		this.btHelp.BackgroundImage = Monitor.Resources.image_form_help;
		this.btHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btHelp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.btHelp.FlatAppearance.BorderSize = 0;
		this.btHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btHelp.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btHelp.ForeColor = System.Drawing.Color.White;
		this.btHelp.Location = new System.Drawing.Point(943, 3);
		this.btHelp.Name = "btHelp";
		this.btHelp.Size = new System.Drawing.Size(75, 24);
		this.btHelp.TabIndex = 2;
		this.btHelp.UseVisualStyleBackColor = false;
		this.btHelp.Click += new System.EventHandler(btHelp_Click);
		this.btClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btClose.FlatAppearance.BorderSize = 0;
		this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btClose.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btClose.Image = Monitor.Resources.image_screen_close;
		this.btClose.Location = new System.Drawing.Point(1024, 3);
		this.btClose.Name = "btClose";
		this.btClose.Size = new System.Drawing.Size(26, 24);
		this.btClose.TabIndex = 1;
		this.btClose.UseVisualStyleBackColor = true;
		this.btClose.Click += new System.EventHandler(btClose_Click);
		this.lbTitleBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lbTitleBar.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbTitleBar.Location = new System.Drawing.Point(32, 5);
		this.lbTitleBar.Name = "lbTitleBar";
		this.lbTitleBar.Size = new System.Drawing.Size(905, 20);
		this.lbTitleBar.TabIndex = 0;
		this.lbTitleBar.Text = "Fiscal.io - Enviar Documentos";
		this.lbTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lbTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(lbTitleBar_MouseDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1058, 513);
		base.Controls.Add(this.pnForm);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmDocSend";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Enviar Documentos";
		base.Load += new System.EventHandler(frmDocSend_Load);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(frmDocSend_HelpRequested);
		this.splitList.Panel1.ResumeLayout(false);
		this.splitList.Panel2.ResumeLayout(false);
		this.splitList.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitList).EndInit();
		this.splitList.ResumeLayout(false);
		this.pnEmailData.ResumeLayout(false);
		this.pnEmailData.PerformLayout();
		this.plnMessage.ResumeLayout(false);
		this.plnMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picProgress).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.pnForm.ResumeLayout(false);
		this.pnContent.ResumeLayout(false);
		this.pnTitleBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).EndInit();
		base.ResumeLayout(false);
	}
}
