using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using data.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using srv.fiscal.io.email;
using util.fiscal.io;

namespace Monitor;

public class frmExcelErrorSend : Form
{
	private List<DocAuditGroup> varDocumentList = new List<DocAuditGroup>();

	private clsZipService varclsZipService = new clsZipService();

	private clsDataConfig varclsDataConfig = new clsDataConfig();

	private Configuration varclsConfig;

	private clsEmailService varclsEmailService = new clsEmailService();

	private const int MAX_EMAIL_ATTACHMENT_SIZE_IN_BYTES = 10485760;

	private IContainer components;

	private SplitContainer splitList;

	private FolderBrowserDialog folderDialog;

	private ToolStrip toolStrip1;

	private ToolStripButton tsbSendAll;

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

	private CheckBox ckbSendZip;

	private Label label2;

	private Panel plnMessage;

	private Label lbProgress;

	private Label lbProgress01;

	private PictureBox picProgress;

	private Panel pnForm;

	private Panel pnContent;

	private Panel pnTitleBar;

	private PictureBox picTitleBar;

	private Button btHelp;

	private Label lbTitleBar;

	private Button btClose;

	private ImageList ImageListDocs;

	private ObjectListView lstDocs;

	private OLVColumn olvSelectGroup;

	private OLVColumn olvAudTitle;

	private OLVColumn olvDocPart;

	private OLVColumn olvStTotal;

	public frmExcelErrorSend(List<DocAuditGroup> pDocumentList, List<Event> pEventList, bool pEventLoad = false)
	{
		InitializeComponent();
		if (clsFunction.IsAdmin)
		{
			lbTitleBar.Text = base.Name + " | " + lbTitleBar.Text;
		}
		varDocumentList = pDocumentList;
	}

	private DescribedTaskRenderer funcCreateDescribedTaskRenderer()
	{
		return new DescribedTaskRenderer
		{
			ImageList = ImageListDocs,
			DescriptionAspectName = "AudDesc",
			TitleFont = new Font("Tahoma", 9f, FontStyle.Bold),
			DescriptionFont = new Font("Tahoma", 9f),
			ImageTextSpace = 8,
			TitleDescriptionSpace = 1,
			UseGdiTextRendering = true
		};
	}

	private void funcDefineListViewLayout()
	{
		lstDocs.RowHeight = 40;
		olvAudTitle.Renderer = funcCreateDescribedTaskRenderer();
		olvAudTitle.CellPadding = new Rectangle(2, 4, 2, 2);
	}

	private void funcDefineListViewFeatures()
	{
		olvAudTitle.ImageGetter = delegate(object x)
		{
			DocAuditGroup docAuditGroup = (DocAuditGroup)x;
			if (docAuditGroup == null)
			{
				return (object)null;
			}
			if (clsFunction.IsEqual(docAuditGroup.StaType, "E"))
			{
				return 0;
			}
			if (clsFunction.IsEqual(docAuditGroup.StaType, "W"))
			{
				return 1;
			}
			if (clsFunction.IsEqual(docAuditGroup.StaType, "S"))
			{
				return 2;
			}
			return clsFunction.IsEqual(docAuditGroup.StaType, "A") ? ((object)3) : null;
		};
	}

	private async void frmDocSend_Load(object sender, EventArgs e)
	{
		funcDefineListViewLayout();
		funcDefineListViewFeatures();
		Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		lbProgress.Text = "Carregando documentos ...";
		plnMessage.Visible = true;
		Application.DoEvents();
		toolStrip1.Enabled = false;
		ckbSendMySelf.Checked = clsFunction.funcConvStrToBool(varclsConfig.EmailMySelfSetted);
		lbMyEmail.Text = clsFunction.funcFixEmail(varclsConfig.UserEmail);
		txtSubject.Text = varclsConfig.EmailTitle;
		txPartnerEmail.Text = varclsConfig.EmailAddress;
		lstDocs.ClearObjects();
		lstDocs.AddObjects(varDocumentList);
		lstDocs.CheckAll();
		plnMessage.Visible = false;
		Application.DoEvents();
		toolStrip1.Enabled = true;
		ckbSendZip.Checked = !string.IsNullOrEmpty(varclsConfig.SendZipped);
		funcSetFieldsEnabled();
	}

	private void lkbChangeMyEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		frmLicense frmLicense = new frmLicense(frmLicense.enTabPage.LicenseData);
		frmLicense.ShowDialog(this);
		frmLicense.Dispose();
		lbMyEmail.Text = varclsConfig.UserEmail;
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

	private async Task<clsReturn> funcPrepareDocsAsync(Action<decimal> onProgessChange = null)
	{
		frmTabAuditor obj = Application.OpenForms["frmTabAuditor"] as frmTabAuditor;
		List<DocAuditGroup> varDocumentList = ((lstDocs.CheckedObjects != null) ? lstDocs.CheckedObjects.Cast<DocAuditGroup>().ToList() : new List<DocAuditGroup>());
		List<DocAuditItem> pListObjects = await obj.funcPrepareAuditGroupToExport(varDocumentList);
		string varTemporaryPath = new clsFileManager().funcGetTemporaryFolder();
		return await clsScreenGeral.funcExportDocToExcelAsync(pListObjects, null, varTemporaryPath, openFileAfterExport: false, showMessageBoxOnError: false);
	}

	private async Task<bool> funcPerformFieldValidationsAsync()
	{
		_ = string.Empty;
		_ = string.Empty;
		if (clsFunction.IsEmpty(txtSubject.Text))
		{
			MessageBox.Show("Informe o assunto do E-mail.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtSubject.Focus();
			return false;
		}
		if (txtSubject.Text.Length > 250)
		{
			MessageBox.Show(" O campo Assunto deve ter no máximo 250 caracteres.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtSubject.Focus();
			return false;
		}
		if (clsFunction.IsEmpty(txPartnerEmail.Text) && !ckbSendMySelf.Checked)
		{
			MessageBox.Show("Nenhum destinatário foi informado para envio de e-mail.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txPartnerEmail.Focus();
			return false;
		}
		if (txPartnerEmail.Text.Length > 500)
		{
			MessageBox.Show("O campo E-mail deve ter no máximo 500 caracteres.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txPartnerEmail.Focus();
			return false;
		}
		if (!(await funcValidateEmailListAsync()))
		{
			txPartnerEmail.Focus();
			return false;
		}
		return true;
	}

	private clsReturn funcPrepareMailParams()
	{
		clsReturn clsReturn = new clsReturn();
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
		string varSubject = txtSubject.Text;
		clsReturn.AddValue("varSubject", varSubject);
		clsReturn.AddValue("varMailListTo", varMailListTo);
		clsReturn.AddValue("varMailListCc", varMailListCc);
		return clsReturn;
	}

	private async Task<clsReturn> funcPrepareAttachmentsAsync(List<string> pFileLocationList)
	{
		lbProgress.Text = "Preparando arquivo(s) a enviar ...";
		plnMessage.Visible = true;
		Application.DoEvents();
		clsReturn clsReturnObj = new clsReturn();
		try
		{
			List<string> varEmailFileList = new List<string>();
			foreach (string varFile in pFileLocationList)
			{
				if (!string.IsNullOrEmpty(varFile))
				{
					varEmailFileList.Add(varFile);
				}
			}
			List<string> varTrashFileList = new List<string>();
			varTrashFileList.AddRange(varEmailFileList);
			clsReturn varFileBatchesReturn = funcCreateFileBatches(varEmailFileList);
			if (varFileBatchesReturn == null || varFileBatchesReturn.HasError)
			{
				MessageBox.Show("Erro ao criar lotes de email", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return clsReturnObj;
			}
			List<List<string>> varFileBatches = (List<List<string>>)varFileBatchesReturn.GetObject("ObjBatchList");
			if (ckbSendZip.Checked)
			{
				clsReturn varClsReturn = await funcCreateZipAttachmentsAsync(varFileBatches);
				if (varFileBatchesReturn == null || varClsReturn.HasError)
				{
					MessageBox.Show("Erro ao criar lotes de email", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return clsReturnObj;
				}
				varFileBatches = (List<List<string>>)varClsReturn.GetObject("ObjBatchList");
				List<string> varTrashZip = (List<string>)varClsReturn.GetObject("ObjTrashFileList");
				varTrashFileList.AddRange(varTrashZip);
			}
			clsReturnObj.AddValue("varTrashFileList", varTrashFileList);
			clsReturnObj.AddValue("ObjBatchList", varFileBatches);
		}
		catch (Exception ex)
		{
			clsReturnObj.AddMessage(new clsMessage("E", "9999", ex.Message, ex));
		}
		return clsReturnObj;
	}

	public async Task<clsReturn> funcPrepareBodyAsync(List<string> pFileList)
	{
		clsReturn varClsReturnFunc = new clsReturn();
		new List<clsEmailDocItem>();
		clsEmailTemplate varclsEmailTemplate = new clsEmailTemplate(varclsConfig);
		if (varclsEmailTemplate == null)
		{
			return varClsReturnFunc;
		}
		string varTemplate = await varclsEmailTemplate.funcGetTemplateAsync("XLSX");
		if (string.IsNullOrEmpty(varTemplate))
		{
			return varClsReturnFunc;
		}
		string varFileFormat = "XLSX";
		string varEmailBody = varclsEmailTemplate.funcFillXlsxData(varTemplate, varFileFormat, pFileList);
		if (string.IsNullOrEmpty(varTemplate))
		{
			return varClsReturnFunc;
		}
		varClsReturnFunc.AddValue("varEmailBody", varEmailBody);
		return varClsReturnFunc;
	}

	private async void funcSendDocsAsync(ToolStripButton pTsbButton = null, ToolStripMenuItem pTsmButton = null)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		List<string> varTrashFileList = null;
		try
		{
			tsbSendAll.Enabled = false;
			if (lstDocs.CheckedObjects.Count == 0)
			{
				MessageBox.Show("Selecione pelo menos um item a ser enviado.", "Operação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			if (!(await funcPerformFieldValidationsAsync()))
			{
				return;
			}
			clsReturn varEmailFields = funcPrepareMailParams();
			List<string> varMailListTo = varEmailFields.GetObject("varMailListTo") as List<string>;
			List<string> varMailListCc = varEmailFields.GetObject("varMailListCc") as List<string>;
			string varSubject = varEmailFields.GetValue("varSubject");
			clsReturn varPreparedDocument = await funcPrepareDocsAsync();
			if (varPreparedDocument.HasError)
			{
				varPreparedDocument.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos."));
				funcShowErrorMessage(varPreparedDocument);
				plnMessage.Visible = false;
				Application.DoEvents();
				return;
			}
			string varPreparedDocumentLocation = varPreparedDocument.GetValue("outputPath");
			List<string> varAttachmentsLocation = new List<string> { varPreparedDocumentLocation };
			clsReturn varPreparedAttachments = await funcPrepareAttachmentsAsync(varAttachmentsLocation);
			if (varPreparedAttachments.HasError)
			{
				varPreparedAttachments.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos."));
				funcShowErrorMessage(varPreparedAttachments);
				plnMessage.Visible = false;
				Application.DoEvents();
				return;
			}
			List<List<string>> varFileBatches = varPreparedAttachments.GetObject("ObjBatchList") as List<List<string>>;
			varTrashFileList = varPreparedAttachments.GetObject("varTrashFileList") as List<string>;
			clsReturn varEmailBodyData = await funcPrepareBodyAsync(varAttachmentsLocation);
			if (varEmailBodyData.HasError)
			{
				varEmailBodyData.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos."));
				funcShowErrorMessage(varEmailBodyData);
				plnMessage.Visible = false;
				Application.DoEvents();
				return;
			}
			string varEmailBody = varEmailBodyData.GetValue("varEmailBody");
			lbProgress.Text = "Enviando documento(s) por e-mail ...";
			plnMessage.Visible = true;
			Application.DoEvents();
			varclsReturnFunc = await funcSendMailAsync(varFileBatches, varMailListTo, varMailListCc, varSubject, varEmailBody);
			if (varclsReturnFunc.HasError)
			{
				varclsReturnFunc.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos."));
				funcShowErrorMessage(varclsReturnFunc);
				plnMessage.Visible = false;
				Application.DoEvents();
				return;
			}
			int varTotalEmails = clsFunction.funcConvStrToInt(varclsConfig.EmailForDocs) + varFileBatches.Count();
			varclsConfig.EmailForDocs = varTotalEmails.ToString();
			await new clsDataConfig().funcUpdateAsync(varclsConfig);
			plnMessage.Visible = false;
			Application.DoEvents();
			MessageBox.Show("Documento(s) enviado(s) com sucesso !!!", "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Close();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddMessage(new clsMessage("E", "999", "Não foi possivel enviar os arquivos.", pException));
			funcShowErrorMessage(varclsReturnFunc);
			plnMessage.Visible = false;
			Application.DoEvents();
			return;
		}
		finally
		{
			tsbSendAll.Enabled = true;
			if (varTrashFileList != null)
			{
				clsDataGeral.funcReleaseBinary(varclsConfig, varTrashFileList);
			}
		}
		plnMessage.Visible = false;
		Application.DoEvents();
	}

	private clsReturn funcCreateFileBatches(List<string> pFiles)
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
					if (varCurrentSize + varFileSize > 10485760)
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
		funcSendDocsAsync();
	}

	private void ckbSendMySelf_CheckedChanged(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
		funcSetFieldsEnabled();
	}

	private async void funcSaveUserEmailDataPreferencesAsync()
	{
		varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
		varclsConfig.EmailMySelfSetted = (ckbSendMySelf.Checked ? "X" : "");
		varclsConfig.SendZipped = (ckbSendZip.Checked ? "X" : "");
		varclsConfig.EmailTitle = txtSubject.Text;
		varclsConfig.EmailAddress = txPartnerEmail.Text;
		await varclsDataConfig.funcUpdateAsync(varclsConfig);
	}

	private void funcSetFieldsEnabled()
	{
		lbMyEmail.Enabled = ckbSendMySelf.Checked;
		lkbChangeMyEmail.Enabled = ckbSendMySelf.Checked;
	}

	private void txtSubject_Validated(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
	}

	private void txPartnerEmail_Validated(object sender, EventArgs e)
	{
		funcSaveUserEmailDataPreferencesAsync();
	}

	private void lstDocs_MouseClick(object sender, MouseEventArgs e)
	{
		ListViewHitTestInfo hit = lstDocs.HitTest(e.Location);
		if (hit.Item != null && hit.SubItem != null && hit.Item.SubItems.IndexOf(hit.SubItem) == 0)
		{
			e = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);
			hit.Item.Checked = !hit.Item.Checked;
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmExcelErrorSend));
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
		this.lstDocs = new BrightIdeasSoftware.ObjectListView();
		this.olvSelectGroup = new BrightIdeasSoftware.OLVColumn();
		this.olvAudTitle = new BrightIdeasSoftware.OLVColumn();
		this.olvDocPart = new BrightIdeasSoftware.OLVColumn();
		this.olvStTotal = new BrightIdeasSoftware.OLVColumn();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.tsbSendAll = new System.Windows.Forms.ToolStripButton();
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
		((System.ComponentModel.ISupportInitialize)this.lstDocs).BeginInit();
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
		this.lstDocs.AllColumns.Add(this.olvSelectGroup);
		this.lstDocs.AllColumns.Add(this.olvAudTitle);
		this.lstDocs.AllColumns.Add(this.olvDocPart);
		this.lstDocs.AllColumns.Add(this.olvStTotal);
		this.lstDocs.AllowColumnReorder = true;
		this.lstDocs.AllowDrop = true;
		this.lstDocs.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.lstDocs.CellEditUseWholeCell = false;
		this.lstDocs.CheckBoxes = true;
		this.lstDocs.CheckedAspectName = "";
		this.lstDocs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.olvSelectGroup, this.olvAudTitle, this.olvDocPart, this.olvStTotal });
		this.lstDocs.Cursor = System.Windows.Forms.Cursors.Default;
		this.lstDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lstDocs.EmptyListMsgFont = new System.Drawing.Font("Tahoma", 8.25f);
		this.lstDocs.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.lstDocs.HeaderWordWrap = true;
		this.lstDocs.HideSelection = false;
		this.lstDocs.IncludeColumnHeadersInCopy = true;
		this.lstDocs.Location = new System.Drawing.Point(0, 25);
		this.lstDocs.MenuLabelGroupBy = "Agrupar por '{0}'";
		this.lstDocs.MenuLabelLockGroupingOn = "Fixar  grupo em '{0}'";
		this.lstDocs.MenuLabelSelectColumns = "Selecionar colunas...";
		this.lstDocs.MenuLabelSortAscending = "Ordenar crescente por '{0}'";
		this.lstDocs.MenuLabelSortDescending = "Ordenar decrescente por '{0}'";
		this.lstDocs.MenuLabelTurnOffGroups = "Desativar agrupamento";
		this.lstDocs.MenuLabelUnlockGroupingOn = "Desafixar grupo em '{0}'";
		this.lstDocs.MenuLabelUnsort = "Remover ordenação";
		this.lstDocs.Name = "lstDocs";
		this.lstDocs.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
		this.lstDocs.ShowCommandMenuOnRightClick = true;
		this.lstDocs.ShowGroups = false;
		this.lstDocs.ShowHeaderInAllViews = false;
		this.lstDocs.ShowItemToolTips = true;
		this.lstDocs.Size = new System.Drawing.Size(1044, 336);
		this.lstDocs.SmallImageList = this.ImageListDocs;
		this.lstDocs.SortGroupItemsByPrimaryColumn = false;
		this.lstDocs.TabIndex = 39;
		this.lstDocs.UseAlternatingBackColors = true;
		this.lstDocs.UseCellFormatEvents = true;
		this.lstDocs.UseCompatibleStateImageBehavior = false;
		this.lstDocs.UseFilterIndicator = true;
		this.lstDocs.UseFiltering = true;
		this.lstDocs.UseHotItem = true;
		this.lstDocs.View = System.Windows.Forms.View.Details;
		this.lstDocs.MouseClick += new System.Windows.Forms.MouseEventHandler(lstDocs_MouseClick);
		this.olvSelectGroup.AspectName = "";
		this.olvSelectGroup.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
		this.olvSelectGroup.Groupable = false;
		this.olvSelectGroup.HeaderCheckBox = true;
		this.olvSelectGroup.HeaderCheckState = System.Windows.Forms.CheckState.Checked;
		this.olvSelectGroup.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelectGroup.Sortable = false;
		this.olvSelectGroup.Text = "";
		this.olvSelectGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvSelectGroup.Width = 33;
		this.olvAudTitle.AspectName = "AudTitle";
		this.olvAudTitle.Text = "Regra";
		this.olvAudTitle.Width = 803;
		this.olvDocPart.AspectName = "DocPart";
		this.olvDocPart.MinimumWidth = 60;
		this.olvDocPart.Text = "Local";
		this.olvDocPart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvDocPart.ToolTipText = "";
		this.olvDocPart.Width = 104;
		this.olvStTotal.AspectName = "StTotal";
		this.olvStTotal.MinimumWidth = 50;
		this.olvStTotal.Text = "Total";
		this.olvStTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.olvStTotal.Width = 104;
		this.ImageListDocs.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListDocs.ImageStream");
		this.ImageListDocs.TransparentColor = System.Drawing.Color.Transparent;
		this.ImageListDocs.Images.SetKeyName(0, "image_error.png");
		this.ImageListDocs.Images.SetKeyName(1, "image_warning.png");
		this.ImageListDocs.Images.SetKeyName(2, "dfe_confirm.png");
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.tsbSendAll });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1044, 25);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.tsbSendAll.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.tsbSendAll.Image = Monitor.Resources.image_email;
		this.tsbSendAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSendAll.Name = "tsbSendAll";
		this.tsbSendAll.Size = new System.Drawing.Size(111, 22);
		this.tsbSendAll.Text = "Enviar agora";
		this.tsbSendAll.Click += new System.EventHandler(tsbSendAll_Click);
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
		base.Name = "frmExcelErrorSend";
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
		((System.ComponentModel.ISupportInitialize)this.lstDocs).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.pnForm.ResumeLayout(false);
		this.pnContent.ResumeLayout(false);
		this.pnTitleBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picTitleBar).EndInit();
		base.ResumeLayout(false);
	}
}
