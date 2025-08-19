using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using FastReport;
using FastReport.Preview;
using FastReport.Utils;
using manager.fiscal.io;
using PdfiumViewer;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class frmDocViewer : Form
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

	private List<clsChave> varChaveList = new List<clsChave>();

	private clsDFeCodes varclsDFeCodes = new clsDFeCodes();

	private Configuration varclsConfig = new Configuration();

	private clsDataDoc varclsDataDoc = new clsDataDoc();

	private clsDataEvent varclsDataEvent = new clsDataEvent();

	private clsDataDoc _clsDataDoc = new clsDataDoc();

	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private string varLastDocViewed = string.Empty;

	private string varLastEvtViewed = string.Empty;

	private bool _HasNFSeFeatEnabled;

	private bool _HasCFeSatFeatEnabled;

	private const int _MaxTreeLevel = 3;

	private bool _ColumnsLoaded;

	private bool _ShowPDF = true;

	private bool _ShowXML = true;

	private bool _ShowEDI = true;

	private IContainer components;

	private ListView lstDocs;

	private ColumnHeader clTipo;

	private ColumnHeader clDoc;

	private ColumnHeader clSeq;

	private ColumnHeader clData;

	private ColumnHeader clHora;

	private ColumnHeader clProtoc;

	private SplitContainer splitList;

	private ColumnHeader clSerie;

	private ColumnHeader clEvent;

	private ColumnHeader clMotivo;

	private ToolStrip toolStrip1;

	private ToolStripButton tsbSendAll;

	private ToolStripSeparator toolStripSeparator2;

	private ColumnHeader clHasXml;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton tsbBaixarPDF;

	private ToolStripButton tsbBaixarXML;

	private ToolStripSeparator toolStripSeparator4;

	private ContextMenuStrip contextMenuDocs;

	private ToolStripMenuItem tsmCopyDocKey;

	private TabControl tabDocs;

	private TabPage tabDocXml;

	private WebBrowser wbsXMLViewer;

	private TabPage tabDocFast;

	private PreviewControl pvcPdfViewer;

	private TabPage tabDocPdf;

	private TabPage tabDocTree;

	private TreeView trvDocs;

	private ColumnHeader clDFeSource;

	private ColumnHeader clDFeDtLoad;

	private ColumnHeader clXmlSource;

	private ColumnHeader clXmlDtLoad;

	private ColumnHeader clDFeDtStat;

	private TabPage tabDocEdi;

	private TextBox txtEdiViewer;

	private Panel panel7;

	private LinkLabel lkbMessageEdi;

	private Label lbMessageEdi;

	private PictureBox picMessageEdi;

	private ToolStripButton tsbBaixarAll;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripDropDownButton tsmBaixarEDI;

	private ToolStripMenuItem tsbBaixarEDI;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripMenuItem tsbBaixarEDIAll;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem tsbSendEDIOthers;

	private ToolStripMenuItem tsbBaixarEDIPDF;

	private ToolStripMenuItem tsbBaixarEDIXML;

	private ColumnHeader clDFeHrLoad;

	private ColumnHeader clXmlHrLoad;

	private ColumnHeader clDFeHrStat;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton tsbExportar;

	private TabPage tabDocNote;

	private TextBox txDocNote;

	private ToolStripSeparator toolStripSeparator8;

	private ToolStripButton tsbChanges;

	private ImageList ImageListDocs;

	private PdfViewer cntPdfViewer;

	private TabPage tabDocWbs;

	private WebBrowser wbsPdfViewer;

	private ColumnHeader clEmail;

	private ColumnHeader clMachine;

	private ColumnHeader clAgent;

	private TabPage tabDocInfo;

	private Label lbDFeSource;

	private Label label11;

	private Label lbDFeSourceTitle;

	private Label txDFeSource;

	private Label txMachine;

	private Label lbMachine;

	private Label txXmlDtLoad;

	private Label lbXmlDtLoad;

	private Label txDFeDtLoad;

	private Label lbDFeDtLoad;

	private Label txEmail;

	private Label lbManifLine;

	private Label lbManifSource;

	private Label lbEmail;

	private Label txXmlSource;

	private Label label2;

	private Label lbXmlSourceTitle;

	private Label lbXmlSource;

	private Label txAgent;

	private Label lbAgent;

	private Label txTagUser;

	private Label lbTagLine;

	private Label lbTagTitle;

	private Label lbTagUser;

	private Label txTagDtHr;

	private Label lbTagDtHr;

	private Label txDocNoteDtHr;

	private Label lbDocNoteDtHr;

	private Label txDocNoteUser;

	private Label lbDocNoteLine;

	private Label lbDocNoteTitle;

	private Label lbDocNoteUser;

	public frmDocViewer(List<Document> pDocumentList, bool pShowPDF = true, bool pShowXML = false, bool pShowEDI = false)
	{
		InitializeComponent();
		varDocumentList = pDocumentList;
		base.HandleDestroyed += TxDocNote_HandleDestroyed;
		_ShowPDF = pShowPDF;
		_ShowXML = pShowXML;
		_ShowEDI = pShowEDI;
	}

	private async void TxDocNote_HandleDestroyed(object sender, EventArgs e)
	{
		await funcUpdateNote();
	}

	private async void frmDocViewer_Load(object sender, EventArgs e)
	{
		varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeSatFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		_ColumnsLoaded = false;
		clsFunction.funcSetColumnsConfiguration(this, lstDocs);
		_ColumnsLoaded = true;
		tabDocs.TabPages.Clear();
		TabPage varTabPdfViewer = (clsFunction.IsEqual(varclsConfig.ActPDFViewer, "W") ? tabDocWbs : ((!clsFunction.IsEqual(varclsConfig.ActPDFViewer, "X")) ? tabDocPdf : tabDocFast));
		tabDocs.TabPages.Add(varTabPdfViewer);
		tabDocs.TabPages.Add(tabDocXml);
		tabDocs.TabPages.Add(tabDocEdi);
		if (_ShowPDF)
		{
			tabDocs.SelectedTab = varTabPdfViewer;
		}
		else if (_ShowXML)
		{
			tabDocs.SelectedTab = tabDocXml;
		}
		else if (_ShowEDI)
		{
			tabDocs.SelectedTab = tabDocEdi;
		}
		tabDocs.TabPages.Add(tabDocTree);
		tabDocs.TabPages.Add(tabDocNote);
		_HasNFSeFeatEnabled = await clsScreenGeral.funcHasNFSeNacionalFeatureAsync();
		_HasCFeSatFeatEnabled = await clsScreenGeral.funcHasCFeNacionalFeatureAsync();
		List<Document> varDocList = varDocumentList.ToList();
		varDocumentList.Clear();
		foreach (Document varclsItem in varDocList)
		{
			Document varclsDoc = await _clsDataDoc.funcGetItemByKeyAsync(varclsItem.Filial, varclsItem.Chave);
			if (varclsDoc != null)
			{
				varDocumentList.Add(varclsDoc);
			}
			else
			{
				varDocumentList.Add(varclsItem);
			}
		}
		funcLoadDocsAsync(varDocumentList);
	}

	private void funcRemoveTabPage(TabPage pclsTabPage)
	{
		while (tabDocs.TabPages.Contains(pclsTabPage))
		{
			tabDocs.TabPages.Remove(pclsTabPage);
		}
	}

	private async Task<bool> funcShowDocTreeAsync(Document pDocument)
	{
		int varNodeLevel = 0;
		funcRemoveTabPage(tabDocTree);
		trvDocs.BeginUpdate();
		trvDocs.SuspendLayout();
		trvDocs.Nodes.Clear();
		TreeNode varNodeItem = await funcCreateDocTreeAsync(pDocument, varNodeLevel);
		if (varNodeItem != null)
		{
			trvDocs.Nodes.Add(varNodeItem);
		}
		trvDocs.EndUpdate();
		trvDocs.ResumeLayout();
		tabDocs.TabPages.Add(tabDocTree);
		return true;
	}

	private TreeNode funcAddDocInfo(TreeNode pTreeNode, Document pDocument)
	{
		pTreeNode.Nodes.Add(new TreeNode("Emissor : " + pDocument.EmitNome + " [ " + clsFunction.funcFormatDoc(pDocument.EmitID) + " ]"));
		pTreeNode.Nodes.Add(new TreeNode("Tomador : " + pDocument.TomaNome + " [ " + clsFunction.funcFormatDoc(pDocument.TomaID) + " ]"));
		pTreeNode.Nodes.Add(new TreeNode("Data : " + pDocument.DtAut));
		if (!clsFunction.IsEmpty(pDocument.tpServ))
		{
			string varTipoServ = varclsDFeCodes.funcGetTpServ(pDocument.Model, pDocument.tpServ);
			pTreeNode.Nodes.Add(new TreeNode("Tipo Serviço : " + varTipoServ));
		}
		if (!clsFunction.IsEmpty(pDocument.FinNFe))
		{
			string varFinalidade = varclsDFeCodes.funcGetFinalidade(pDocument.Model, pDocument.FinNFe);
			pTreeNode.Nodes.Add(new TreeNode("Finalidade : " + varFinalidade));
		}
		if (!clsFunction.IsEmpty(pDocument.ModFrete))
		{
			string varModalidade = varclsDFeCodes.funcGetNFeModFrete(pDocument.Model, pDocument.ModFrete);
			pTreeNode.Nodes.Add(new TreeNode("Modalidade de Frete : " + varModalidade));
		}
		return pTreeNode;
	}

	private async Task<TreeNode> funcCreateDocTreeAsync(Document pDocument, int pNodeLevel)
	{
		pNodeLevel++;
		if (pNodeLevel > 3)
		{
			return null;
		}
		if (pDocument == null)
		{
			return null;
		}
		string varCancMessage = string.Empty;
		pDocument.Canceled = clsFunction.funcGetValue(pDocument.Canceled);
		if (pDocument.Canceled.Equals("X") || !clsFunction.IsEmpty(pDocument.HasCancelEvent))
		{
			varCancMessage = " *** Cancelado ***";
		}
		else if (pDocument.Canceled.Equals("I"))
		{
			varCancMessage = " *** Inutilizado ***";
		}
		else if (pDocument.Canceled.Equals("D"))
		{
			varCancMessage = " *** Denegado ***";
		}
		else if (pDocument.Canceled.Equals("R"))
		{
			varCancMessage = " *** Rejeitado ***";
		}
		string varNodeText = clsFunction.funcGetDocType(pDocument.Model) + " " + pDocument.Num + "-" + pDocument.Serie;
		if (!clsFunction.IsEmpty(pDocument.TpDoc))
		{
			string varTipoDoc = varclsDFeCodes.funcGetTipoDoc(pDocument.Model, pDocument.TpDoc);
			varNodeText = varNodeText + " [ " + varTipoDoc + " ] ";
		}
		if (!clsFunction.IsEmpty(pDocument.Valor))
		{
			varNodeText = varNodeText + " ( Valor : " + pDocument.Valor + " ) ";
		}
		if (!clsFunction.IsEmpty(pDocument.xMunIni))
		{
			varNodeText = varNodeText + " -> De " + pDocument.UFIni + "/" + pDocument.xMunIni;
		}
		if (!clsFunction.IsEmpty(pDocument.xMunFim))
		{
			varNodeText = varNodeText + " -> Para " + pDocument.UFFim + "/" + pDocument.xMunFim;
		}
		if (!clsFunction.IsEmpty(varCancMessage))
		{
			varNodeText += varCancMessage;
		}
		TreeNode varNodeTree = new TreeNode(varNodeText)
		{
			Tag = pDocument,
			ForeColor = Color.Blue
		};
		if (!clsFunction.IsEmpty(varCancMessage))
		{
			varNodeTree.ForeColor = Color.Gray;
		}
		varNodeTree = funcAddDocInfo(varNodeTree, pDocument);
		foreach (DocLink varDocItem in await new clsDataDocLink().funcGetListByDocKeyAsync(pDocument.Chave))
		{
			Document varDocument = await varclsDataDoc.funcGetItemByKeyAsync(pDocument.Filial, varDocItem.RefKey);
			if (varDocument == null)
			{
				varDocument = await varclsDataDoc.funcGetItemByChaveAsync(varDocItem.RefKey);
			}
			if (varDocument == null)
			{
				varDocument = varclsDataDoc.funcGetDocFromKey(pDocument.Filial, varDocItem.RefKey);
			}
			TreeNode varNodeItem = await funcCreateDocTreeAsync(varDocument, pNodeLevel);
			if (varNodeItem != null)
			{
				varNodeTree.Nodes.Add(varNodeItem);
			}
		}
		if (pNodeLevel < 3)
		{
			varNodeTree.Expand();
		}
		return varNodeTree;
	}

	private async Task<bool> funcShowDocAsync(Document pDocument)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			tsbChanges.Visible = true;
			if (clsFunction.IsEqual(pDocument.Chave, varLastDocViewed))
			{
				return false;
			}
			FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(pDocument.Filial);
			clsXmlFactory varXmlFactory = new clsXmlFactory();
			intXmlObject varXmlObject = varXmlFactory.funcGetDocClass(varclsConfig, pDocument.Model, pReload: false);
			string varXmlFilePath = clsFunction.funcGetValue(await varXmlObject.funcGetBinaryFileAsync(varclsFilial, pDocument.Chave, pDocument));
			if (varXmlFilePath.Contains("_DFe"))
			{
				varXmlFilePath = string.Empty;
			}
			if (!File.Exists(varXmlFilePath))
			{
				pDocument = await funcDownloadXmlAsync(varclsFilial, pDocument);
			}
			else if (clsFunction.IsEmpty(pDocument.HasXml))
			{
				pDocument.HasXml = "X";
				await varclsDataDoc.funcUpdateAsync(pDocument);
			}
			varLastDocViewed = pDocument.Chave;
			varLastEvtViewed = string.Empty;
			await funcShowXmlAsync(pDocument, varXmlFilePath);
			await funcShowPdfAsync(varclsFilial, pDocument.Model, pDocument.Chave, varXmlObject, varXmlFilePath, pEvent: false);
			await funcShowEdiAsync(varclsFilial, pDocument.Model, pDocument.Chave, varXmlObject, varXmlFilePath);
			await funcShowDocTreeAsync(pDocument);
			funcRemoveTabPage(tabDocNote);
			tabDocs.TabPages.Add(tabDocNote);
			txDocNote.Text = pDocument.DocNote;
			funcShowTraceability(pDocument);
			if (tabDocs.TabPages.Contains(tabDocXml) && (tabDocs.TabPages.Contains(tabDocFast) || tabDocs.TabPages.Contains(tabDocPdf)))
			{
				tsbBaixarAll.Visible = true;
			}
			else
			{
				tsbBaixarAll.Visible = false;
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
			funcShowErrorMessage(varclsReturnFunc);
		}
		return true;
	}

	private async Task<bool> funcShowEvtAsync(Event pEvent)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			tsbChanges.Visible = false;
			string varChave = pEvent.Chave + "-" + pEvent.tpEvento + "-" + pEvent.nSeqEvento;
			if (varChave.Equals(varLastEvtViewed))
			{
				return false;
			}
			Document varDocument = await new clsDataDoc().funcGetItemByChaveAsync(pEvent.Chave);
			if (varDocument == null)
			{
				varDocument = _clsDataDoc.funcGetDocFromKey(string.Empty, pEvent.Chave);
			}
			FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varDocument.Filial);
			if (varclsFilial == null)
			{
				varclsFilial = new FilialView();
			}
			string varDFeModel = clsFunction.funcGetDFeModel(pEvent.Chave);
			Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
			clsXmlFactory varXmlFactory = new clsXmlFactory();
			intXmlObject varXmlObject = varXmlFactory.funcGetEventClass(varclsConfig, varDFeModel, pReload: false);
			string varXmlFilePath = await varXmlObject.funcGetBinaryFileAsync(varclsFilial, varChave, varDocument);
			if (File.Exists(varXmlFilePath) && clsFunction.IsEmpty(pEvent.XmlDtLoad))
			{
				pEvent.XmlSource = pEvent.DFeSource;
				pEvent.XmlDtLoad = pEvent.DFeDtLoad;
				pEvent.XmlHrLoad = pEvent.DFeHrLoad;
				await varclsDataEvent.funcUpdateAsync(pEvent);
			}
			varLastEvtViewed = varChave;
			varLastDocViewed = string.Empty;
			await funcShowXmlAsync(pEvent, varXmlFilePath);
			await funcShowPdfAsync(varclsFilial, varDocument.Model, varChave, varXmlObject, varXmlFilePath, pEvent: true);
			await funcShowEdiAsync(varclsFilial, varDocument.Model, varChave, varXmlObject, varXmlFilePath);
			await funcShowDocTreeAsync(varDocument);
			TabPage varTabDocPdf = (clsFunction.IsEqual(varclsConfig.ActPDFViewer, "W") ? tabDocWbs : ((!clsFunction.IsEqual(varclsConfig.ActPDFViewer, "X")) ? tabDocFast : tabDocPdf));
			funcRemoveTabPage(tabDocNote);
			txDocNote.Text = string.Empty;
			funcShowTraceability(pEvent);
			if (tabDocs.TabPages.Contains(tabDocXml) && tabDocs.TabPages.Contains(varTabDocPdf))
			{
				tsbBaixarAll.Visible = true;
			}
			else
			{
				tsbBaixarAll.Visible = false;
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
			funcShowErrorMessage(varclsReturnFunc);
		}
		return true;
	}

	private async Task<bool> funcShowPdfAsync(FilialView pclsFilial, string pDocModel, string pDocKey, intXmlObject pclsXmlObject, string pXmlSource, bool pEvent)
	{
		if (tabDocs.TabPages.Contains(tabDocFast))
		{
			pvcPdfViewer.Clear();
		}
		else if (tabDocs.TabPages.Contains(tabDocPdf))
		{
			cntPdfViewer.Document = null;
			cntPdfViewer.Tag = null;
		}
		else if (tabDocs.TabPages.Contains(tabDocWbs))
		{
			wbsPdfViewer.Navigate("about:blank");
		}
		clsReturn varclsReturn = await funcGeneratePdfAsync(pclsFilial, pDocModel, pDocKey, pclsXmlObject, pEvent);
		if (!varclsReturn.ActionDone || varclsReturn.HasError)
		{
			funcRemoveTabPage(tabDocFast);
			funcRemoveTabPage(tabDocPdf);
			funcRemoveTabPage(tabDocWbs);
			tsbBaixarPDF.Visible = false;
		}
		if (varclsReturn.HasError && clsFunction.IsAdmin)
		{
			throw varclsReturn.funcGetExceptionMessage();
		}
		if (varclsReturn.HasError)
		{
			return false;
		}
		if (!varclsReturn.ActionDone)
		{
			return false;
		}
		TabPage varTabPdfViewer = varclsReturn.GetObject<TabPage>("TabPageViewer");
		if (varTabPdfViewer == null)
		{
			return false;
		}
		tsbBaixarPDF.Visible = true;
		if (!tabDocs.TabPages.Contains(varTabPdfViewer))
		{
			funcRemoveTabPage(varTabPdfViewer);
			tabDocs.TabPages.Insert(0, varTabPdfViewer);
			tabDocs.SelectedTab = varTabPdfViewer;
		}
		if (tabDocFast != varTabPdfViewer && tabDocs.TabPages.Contains(tabDocFast))
		{
			funcRemoveTabPage(tabDocFast);
		}
		if (tabDocPdf != varTabPdfViewer && tabDocs.TabPages.Contains(tabDocPdf))
		{
			funcRemoveTabPage(tabDocPdf);
		}
		if (tabDocWbs != varTabPdfViewer && tabDocs.TabPages.Contains(tabDocWbs))
		{
			funcRemoveTabPage(tabDocWbs);
		}
		return true;
	}

	private async Task<clsReturn> funcGeneratePdfAsync(FilialView pclsFilial, string pDocModel, string pDocKey, intXmlObject pclsXmlObject, bool pEvent)
	{
		clsReturn varclsReturn = new clsReturn();
		clsPdfFactory varPdfFactory = new clsPdfFactory();
		try
		{
			intPdfObject varPdfObject = (pEvent ? varPdfFactory.funcGetEventClass(pclsXmlObject, pDocModel) : varPdfFactory.funcGetDocClass(pclsXmlObject, pDocModel));
			if (clsFunction.IsEqual(varclsConfig.ActPDFViewer, "W"))
			{
				varclsReturn = await varPdfObject.funcGenerateAsync(pclsFilial, pDocKey, null);
				string varPdfFilePath = varPdfObject.funcGetFilePath(pclsFilial, pDocKey);
				varPdfFilePath += "#view=fitH,100&navpanes=0&toolbar=0&statusbar=0";
				wbsPdfViewer.Navigate(varPdfFilePath);
				varclsReturn.AddValue("TabPageViewer", tabDocWbs);
				varclsReturn.ActionDone = true;
			}
			else if (clsFunction.IsEqual(varclsConfig.ActPDFViewer, "X") || clsFunction.funcIsNFSe(pDocModel))
			{
				varclsReturn = await varPdfObject.funcGenerateAsync(pclsFilial, pDocKey, null);
				MemoryStream varMemoryStream = new MemoryStream(File.ReadAllBytes(varPdfObject.funcGetFilePath(pclsFilial, pDocKey)));
				cntPdfViewer.Document = PdfDocument.Load(varMemoryStream);
				varclsReturn.AddValue("TabPageViewer", tabDocPdf);
				varclsReturn.ActionDone = true;
			}
			else
			{
				varclsReturn = await varPdfObject.funcGetReportAsync(pclsFilial, pDocKey, null);
				Report varObjReport = (Report)varclsReturn.GetObject("Object");
				if (varObjReport == null)
				{
					return varclsReturn;
				}
				varObjReport.Preview = pvcPdfViewer;
				varObjReport.Prepare();
				varObjReport.ShowPrepared();
				varclsReturn.AddValue("TabPageViewer", tabDocFast);
				varclsReturn.ActionDone = true;
			}
		}
		catch (Exception pException)
		{
			varclsReturn.AddException(pException);
		}
		return varclsReturn;
	}

	private async Task<bool> funcShowXmlAsync(object pDFeObject, string pXmlSource)
	{
		wbsXMLViewer.Navigate("");
		wbsXMLViewer.Tag = null;
		if (!File.Exists(pXmlSource))
		{
			funcRemoveTabPage(tabDocXml);
			tsbBaixarXML.Visible = false;
			return false;
		}
		if (!tabDocs.TabPages.Contains(tabDocXml))
		{
			tabDocs.TabPages.Add(tabDocXml);
		}
		wbsXMLViewer.Navigate(pXmlSource);
		wbsXMLViewer.Tag = pDFeObject;
		tsbBaixarXML.Visible = true;
		return true;
	}

	private async Task<bool> funcShowEdiAsync(FilialView pclsFilial, string pDocModel, string pDocKey, intXmlObject pclsXmlObject, string pXmlSource)
	{
		txtEdiViewer.Text = string.Empty;
		txtEdiViewer.Tag = null;
		if (!File.Exists(pXmlSource) || pDocKey.Length > 44)
		{
			funcRemoveTabPage(tabDocEdi);
			tsmBaixarEDI.Visible = false;
			return false;
		}
		intEdiObject varEdiHandler = new clsEdiFactory().funcGetDocClass(varclsConfig, pclsXmlObject, pDocModel);
		if (varEdiHandler == null)
		{
			funcRemoveTabPage(tabDocEdi);
			tabDocEdi.Visible = false;
			return false;
		}
		clsReturn varclsReturn = await varEdiHandler.funcGenerateAsync(pclsFilial, null, pDocKey, null, pSimulate: true);
		txtEdiViewer.Text = varclsReturn.GetValue("CONTENT");
		if (!clsFunction.IsEmpty(txtEdiViewer.Text))
		{
			if (!tabDocs.TabPages.Contains(tabDocEdi))
			{
				tabDocs.TabPages.Add(tabDocEdi);
			}
			tsmBaixarEDI.Visible = true;
		}
		else
		{
			funcRemoveTabPage(tabDocEdi);
			tsmBaixarEDI.Visible = false;
		}
		return true;
	}

	private bool funcShowTraceability(Document pDocument)
	{
		if (pDocument == null)
		{
			return false;
		}
		funcRemoveTabPage(tabDocInfo);
		tabDocs.TabPages.Add(tabDocInfo);
		funcInvisibleTraceInfo();
		bool varShowTagFields = false;
		bool varShowDocNoteFields = false;
		if (!clsFunction.IsEmpty(pDocument.DFeSource))
		{
			txDFeSource.Text = pDocument.DFeSource;
		}
		else
		{
			txDFeSource.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.DFeDtLoad))
		{
			txDFeDtLoad.Text = pDocument.DFeDtLoad + " " + pDocument.DFeHrLoad;
		}
		else
		{
			txDFeDtLoad.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.XmlSource))
		{
			txXmlSource.Text = pDocument.XmlSource;
		}
		else
		{
			txXmlSource.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.XmlDtLoad))
		{
			txXmlDtLoad.Text = pDocument.XmlDtLoad + " " + pDocument.XmlHrLoad;
		}
		else
		{
			txXmlDtLoad.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.TagUser))
		{
			txTagUser.Text = pDocument.TagUser;
			varShowTagFields = true;
		}
		else
		{
			txTagUser.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.TagDtHr))
		{
			txTagDtHr.Text = pDocument.TagDtHr;
			varShowTagFields = true;
		}
		else
		{
			txTagDtHr.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.DocNoteUser))
		{
			txDocNoteUser.Text = pDocument.DocNoteUser;
			varShowDocNoteFields = true;
		}
		else
		{
			txDocNoteUser.Visible = false;
		}
		if (!clsFunction.IsEmpty(pDocument.DocNoteDtHr))
		{
			txDocNoteDtHr.Text = pDocument.DocNoteDtHr;
			varShowDocNoteFields = true;
		}
		else
		{
			txDocNoteDtHr.Visible = false;
		}
		Label label = lbTagTitle;
		bool visible = (lbTagLine.Visible = varShowTagFields);
		label.Visible = visible;
		Label label2 = lbTagUser;
		visible = (txTagUser.Visible = varShowTagFields);
		label2.Visible = visible;
		Label label3 = lbTagDtHr;
		visible = (txTagDtHr.Visible = varShowTagFields);
		label3.Visible = visible;
		Label label4 = lbDocNoteTitle;
		visible = (lbDocNoteLine.Visible = varShowDocNoteFields);
		label4.Visible = visible;
		Label label5 = lbDocNoteUser;
		visible = (txDocNoteUser.Visible = varShowDocNoteFields);
		label5.Visible = visible;
		Label label6 = lbDocNoteDtHr;
		visible = (txDocNoteDtHr.Visible = varShowDocNoteFields);
		label6.Visible = visible;
		return true;
	}

	private bool funcShowTraceability(Event pEvent)
	{
		if (pEvent == null)
		{
			return false;
		}
		funcRemoveTabPage(tabDocInfo);
		tabDocs.TabPages.Add(tabDocInfo);
		funcInvisibleTraceInfo();
		bool varShowManFields = false;
		if (!clsFunction.IsEmpty(pEvent.DFeSource))
		{
			txDFeSource.Text = pEvent.DFeSource;
		}
		else
		{
			txDFeSource.Visible = false;
		}
		if (!clsFunction.IsEmpty(pEvent.DFeDtLoad))
		{
			txDFeDtLoad.Text = pEvent.DFeDtLoad + " " + pEvent.DFeHrLoad;
		}
		else
		{
			txDFeDtLoad.Visible = false;
		}
		if (!clsFunction.IsEmpty(pEvent.XmlSource))
		{
			txXmlSource.Text = pEvent.XmlSource;
		}
		else
		{
			txXmlSource.Visible = false;
		}
		if (!clsFunction.IsEmpty(pEvent.XmlDtLoad))
		{
			txXmlDtLoad.Text = pEvent.XmlDtLoad + " " + pEvent.XmlHrLoad;
		}
		else
		{
			txXmlDtLoad.Visible = false;
		}
		if (!clsFunction.IsEmpty(pEvent.Email))
		{
			txEmail.Text = pEvent.Email;
			varShowManFields = true;
		}
		else
		{
			txEmail.Visible = false;
		}
		if (!clsFunction.IsEmpty(pEvent.Machine))
		{
			txMachine.Text = pEvent.Machine;
			varShowManFields = true;
		}
		else
		{
			txMachine.Visible = false;
		}
		if (!clsFunction.IsEmpty(pEvent.Agent))
		{
			txAgent.Text = pEvent.Agent;
			varShowManFields = true;
		}
		else
		{
			txAgent.Visible = false;
		}
		Label label = lbManifSource;
		bool visible = (lbManifLine.Visible = varShowManFields);
		label.Visible = visible;
		Label label2 = lbEmail;
		Label label3 = lbMachine;
		bool flag2 = (lbAgent.Visible = varShowManFields);
		visible = (label3.Visible = flag2);
		label2.Visible = visible;
		Label label4 = txEmail;
		Label label5 = txMachine;
		flag2 = (txAgent.Visible = varShowManFields);
		visible = (label5.Visible = flag2);
		label4.Visible = visible;
		return true;
	}

	private void funcInvisibleTraceInfo()
	{
		Label label = lbTagTitle;
		bool visible = (lbTagLine.Visible = false);
		label.Visible = visible;
		Label label2 = lbTagUser;
		visible = (txTagUser.Visible = false);
		label2.Visible = visible;
		Label label3 = lbTagDtHr;
		visible = (txTagDtHr.Visible = false);
		label3.Visible = visible;
		Label label4 = lbDocNoteTitle;
		visible = (lbDocNoteLine.Visible = false);
		label4.Visible = visible;
		Label label5 = lbDocNoteUser;
		visible = (txDocNoteUser.Visible = false);
		label5.Visible = visible;
		Label label6 = lbDocNoteDtHr;
		visible = (txDocNoteDtHr.Visible = false);
		label6.Visible = visible;
		Label label7 = lbManifSource;
		visible = (lbManifLine.Visible = false);
		label7.Visible = visible;
		Label label8 = lbEmail;
		Label label9 = lbMachine;
		bool flag8 = (lbAgent.Visible = false);
		visible = (label9.Visible = flag8);
		label8.Visible = visible;
		Label label10 = txEmail;
		Label label11 = txMachine;
		flag8 = (txAgent.Visible = false);
		visible = (label11.Visible = flag8);
		label10.Visible = visible;
	}

	private void funcShowErrorMessage(clsReturn pclsReturn)
	{
		if (base.Visible && pclsReturn != null && (pclsReturn.HasError || pclsReturn.HasWarning))
		{
			clsScreenGeral.funcShowUserMessage(this, pclsReturn);
		}
	}

	public async Task<Document> funcDownloadXmlAsync(FilialView pclsFilial, Document pclsDoc)
	{
		clsEvtService varclsEvtService = new clsEvtService();
		if (varChaveList.Find((clsChave r) => r.value == pclsDoc.Chave) != null)
		{
			return pclsDoc;
		}
		varChaveList.Add(new clsChave(pclsDoc.Chave));
		clsEventData varclsEvtData = new clsEventData();
		varclsEvtData.EvtUserType = "DOWNLOAD";
		varclsEvtData.HasDocSelect = true;
		varclsEvtData.Agent = "Manual";
		varclsEvtData.DocList = new List<Document> { pclsDoc };
		await varclsEvtService.funcExecuteAsync(this, varclsEvtData);
		Document varDocument = await varclsDataDoc.funcGetItemByKeyAsync(pclsDoc.Filial, pclsDoc.Chave);
		if (varDocument == null)
		{
			varDocument = pclsDoc;
		}
		return varDocument;
	}

	public async void funcLoadDocsAsync(List<Document> pDocList)
	{
		clsValidatorService varclsValidator = new clsValidatorService();
		lstDocs.Items.Clear();
		new ListViewItem();
		int varItemCount = 0;
		foreach (Document varclsDoc in pDocList)
		{
			varItemCount++;
			ListViewItem varItem = lstDocs.Items.Add("");
			varItem.ImageIndex = clsScreenGeral.funcGetDocXmlIcon(varclsDoc, _HasNFSeFeatEnabled, _HasCFeSatFeatEnabled);
			varItem.Tag = varclsDoc;
			varItem.ToolTipText = varclsValidator.funcGetDesc(varclsDoc);
			varItem.SubItems.Add(varclsDoc.Num.PadLeft(9, '0'));
			varItem.SubItems.Add(varclsDoc.Serie);
			string varDocName = clsFunction.funcGetDocName(varclsDoc.Model);
			varItem.SubItems.Add(varDocName);
			varItem.ForeColor = Color.Blue;
			varItem.SubItems.Add("");
			varItem.SubItems.Add("");
			varItem.SubItems.Add(varclsDoc.DtAut);
			varItem.SubItems.Add(varclsDoc.HrAut);
			varItem.SubItems.Add(varclsDoc.Protc);
			varItem.SubItems.Add(varclsDoc.xMotivo);
			varItem.SubItems.Add(varclsDoc.DFeSource);
			varItem.SubItems.Add(varclsDoc.DFeDtLoad);
			varItem.SubItems.Add(varclsDoc.DFeHrLoad);
			varItem.SubItems.Add(varclsDoc.XmlSource);
			varItem.SubItems.Add(varclsDoc.XmlDtLoad);
			varItem.SubItems.Add(varclsDoc.XmlHrLoad);
			varItem.SubItems.Add(varclsDoc.DFeDtStat);
			varItem.SubItems.Add(varclsDoc.DFeHrStat);
			if (varItemCount == 1)
			{
				try
				{
					varItem.Selected = true;
				}
				catch
				{
				}
			}
			foreach (Event varclsEvent in await new clsDataEvent().funcGetListByDocAsync(varclsDoc))
			{
				if (!varclsDFeCodes.IsDummyEvent(varclsEvent.tpEvento))
				{
					varItem = lstDocs.Items.Add("");
					bool varNotHasXmlEvent = clsFunction.IsEmpty(varclsEvent.XmlHrLoad) && clsFunction.IsEmpty(varclsEvent.XmlDtLoad);
					varItem.ImageIndex = (varNotHasXmlEvent ? (-1) : 0);
					varItem.Tag = varclsEvent;
					varItem.SubItems.Add(varclsDoc.Num.PadLeft(9, '0'));
					varItem.SubItems.Add(varclsDoc.Serie);
					varItem.SubItems.Add(varclsEvent.xEvento);
					varItem.SubItems.Add(varclsEvent.tpEvento);
					varItem.SubItems.Add(varclsEvent.nSeqEvento);
					varItem.SubItems.Add(varclsEvent.DtAut);
					varItem.SubItems.Add(varclsEvent.HrAut);
					varItem.SubItems.Add(varclsEvent.Protc);
					varItem.SubItems.Add(varclsEvent.xMotivo);
					varItem.SubItems.Add(varclsEvent.DFeSource);
					varItem.SubItems.Add(varclsEvent.DFeDtLoad);
					varItem.SubItems.Add(varclsEvent.DFeHrLoad);
					varItem.SubItems.Add(varclsEvent.XmlSource);
					varItem.SubItems.Add(varclsEvent.XmlDtLoad);
					varItem.SubItems.Add(varclsEvent.XmlHrLoad);
					varItem.SubItems.Add(varclsEvent.DFeDtStat);
					varItem.SubItems.Add(varclsEvent.DFeHrStat);
					varItem.SubItems.Add(varclsEvent.Email);
					varItem.SubItems.Add(varclsEvent.Machine);
					varItem.SubItems.Add(varclsEvent.Agent);
				}
			}
		}
	}

	private async Task<bool> funcShowSelectedItemAsync()
	{
		foreach (ListViewItem varItem in lstDocs.Items)
		{
			if (varItem.Selected)
			{
				varItem.BackColor = Color.LightBlue;
				if (varItem.Tag.GetType() == typeof(Document))
				{
					await funcShowDocAsync((Document)varItem.Tag);
				}
				else
				{
					await funcShowEvtAsync((Event)varItem.Tag);
				}
			}
			else
			{
				varItem.BackColor = Color.White;
			}
		}
		return true;
	}

	private async void lstDocs_SelectedIndexChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			await _semaphore.WaitAsync();
			await funcShowSelectedItemAsync();
		}
		finally
		{
			_semaphore.Release();
		}
	}

	private async void funcBaixarDocsAsync(bool pXml, bool pPdf, bool pEdi)
	{
		if (lstDocs.SelectedItems.Count == 0 || lstDocs.SelectedItems[0].Tag == null)
		{
			MessageBox.Show("Selecione um documento da lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		FolderBrowserDialog varFolderDialog = new FolderBrowserDialog();
		Configuration varclsConfig = await new clsDataConfig().funcGetItemByKeyAsync();
		varclsConfig.FolderPath = clsFileManager.funcGetDefaultFolder(varclsConfig.FolderPath);
		if (!clsFunction.IsEmpty(varclsConfig.FolderPath))
		{
			varFolderDialog.SelectedPath = varclsConfig.FolderPath;
		}
		if (!varFolderDialog.ShowDialog(this).Equals(DialogResult.OK))
		{
			return;
		}
		varclsConfig.FolderPath = varFolderDialog.SelectedPath;
		varFolderDialog.Dispose();
		if (clsFunction.IsEmpty(varclsConfig.FolderPath))
		{
			return;
		}
		if (!Directory.Exists(varclsConfig.FolderPath))
		{
			MessageBox.Show("O diretório [" + varclsConfig.FolderPath + "] selecionado não existe !!!", "Operação cancelada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		await new clsDataConfig().funcUpdateAsync(varclsConfig);
		clsXmlFactory varclsXmlFactory = new clsXmlFactory();
		clsPdfFactory varclsPdfFactory = new clsPdfFactory();
		clsEdiFactory varclsEdiFactory = new clsEdiFactory();
		object varObject = lstDocs.SelectedItems[0].Tag;
		if (varObject.GetType() == typeof(Document))
		{
			Document varclsDoc = (Document)varObject;
			FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varclsDoc.Filial);
			intXmlObject varXmlObject = varclsXmlFactory.funcGetDocClass(varclsConfig, varclsDoc.Model, pReload: false);
			intPdfObject varPdfObject = varclsPdfFactory.funcGetDocClass(varXmlObject, varclsDoc.Model);
			intEdiObject varEdiObject = varclsEdiFactory.funcGetDocClass(varclsConfig, varXmlObject, varclsDoc.Model);
			if (pXml)
			{
				await varXmlObject.funcDownloadAsync(varclsFilial, varclsDoc.Chave, varclsConfig.FolderPath, varclsDoc);
			}
			if (pPdf)
			{
				await varPdfObject.funcDownloadAsync(varclsFilial, varclsDoc.Chave, varclsConfig.FolderPath, varclsDoc);
			}
			if (pEdi)
			{
				await varEdiObject.funcDownloadAsync(varclsFilial, varclsDoc.Chave, varclsConfig.FolderPath, varclsDoc);
			}
		}
		else if (varObject.GetType() == typeof(Event))
		{
			Event varclsEvent = (Event)varObject;
			Document varclsDoc = await new clsDataDoc().funcGetItemByChaveAsync(varclsEvent.Chave);
			FilialView varclsFilial = await new clsDataFilial().funcGetItemByKeyAsync(varclsDoc.Filial);
			string varChave = varclsEvent.Chave + "-" + varclsEvent.tpEvento + "-" + varclsEvent.nSeqEvento;
			intXmlObject varXmlObject2 = varclsXmlFactory.funcGetEventClass(varclsConfig, clsFunction.funcGetDFeModel(varclsEvent.Chave), pReload: false);
			intPdfObject varPdfObject = varclsPdfFactory.funcGetEventClass(varXmlObject2, clsFunction.funcGetDFeModel(varclsEvent.Chave));
			if (pXml)
			{
				await varXmlObject2.funcDownloadAsync(varclsFilial, varChave, varclsConfig.FolderPath, varclsDoc);
			}
			if (pPdf)
			{
				await varPdfObject.funcDownloadAsync(varclsFilial, varChave, varclsConfig.FolderPath, varclsDoc);
			}
		}
		MessageBox.Show("Arquivo baixado com sucesso !!!", "Operação realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	private void tsbBaixarAll_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: true, pPdf: true, pEdi: false);
	}

	private async void tsbSendAll_Click(object sender, EventArgs e)
	{
		if (await clsScreenGeral.funcHasAccessAsync("EMAIL-EXECUTE", "EXECUTE"))
		{
			frmDocSend obj = new frmDocSend(varDocumentList, null, pEventLoad: true);
			obj.ShowDialog(this);
			obj.Dispose();
		}
	}

	private void tsbBaixarPDF_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: false, pPdf: true, pEdi: false);
	}

	private void tsbBaixarXML_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: true, pPdf: false, pEdi: false);
	}

	private void tsbBaixarEDI_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: false, pPdf: false, pEdi: true);
	}

	private void tsbBaixarEDIAll_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: true, pPdf: true, pEdi: true);
	}

	private void tsbBaixarEDIXML_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: true, pPdf: false, pEdi: true);
	}

	private void tsbBaixarEDIPDF_Click(object sender, EventArgs e)
	{
		funcBaixarDocsAsync(pXml: false, pPdf: true, pEdi: true);
	}

	private void trvDocs_DoubleClick(object sender, EventArgs e)
	{
		if (trvDocs == null)
		{
			return;
		}
		TreeNode varNodeTree = trvDocs.SelectedNode;
		if (varNodeTree == null || varNodeTree.Tag == null)
		{
			return;
		}
		try
		{
			varDocumentList.Clear();
			varDocumentList.Add((Document)varNodeTree.Tag);
			funcLoadDocsAsync(varDocumentList);
		}
		catch
		{
			if (clsFunction.IsAdmin)
			{
				throw;
			}
		}
	}

	private async void tsmCopyDocKey_Click(object sender, EventArgs e)
	{
		if (lstDocs.Focused)
		{
			if (lstDocs.FocusedItem == null)
			{
				return;
			}
			object varObject = lstDocs.FocusedItem.Tag;
			if (varObject != null)
			{
				if (varObject.GetType().Equals(typeof(Document)))
				{
					await funcGetCopyKeyAsync(((Document)varObject).Chave);
				}
				else
				{
					await funcGetCopyKeyAsync(((Event)varObject).Chave);
				}
			}
		}
		else if (trvDocs.Focused && trvDocs.SelectedNode != null)
		{
			Document varObjDoc = (Document)trvDocs.SelectedNode.Tag;
			if (varObjDoc != null)
			{
				await funcGetCopyKeyAsync(varObjDoc.Chave);
			}
		}
	}

	private async Task<bool> funcGetCopyKeyAsync(string pDocKey)
	{
		return await clsScreenGeral.funcCopyDocKeyAsync(pDocKey);
	}

	private void lkbMessageEdi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		clsHelpService.funcCallEdiProcedaHelp();
	}

	private void lstView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		if (_ColumnsLoaded)
		{
			ListView varListView = (ListView)sender;
			if (varListView != null)
			{
				int varColumnIndex = e.ColumnIndex;
				clsFunction.funcSetRegisterValue($"{base.Name}-{varListView.Name}-Index{varColumnIndex}", varListView.Columns[varColumnIndex].Width.ToString(), pGlobal: false);
			}
		}
	}

	private void lstView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
	{
		if (_ColumnsLoaded)
		{
			ListView varListView = (ListView)sender;
			if (varListView != null)
			{
				int varColumnIndex = e.Header.Index;
				clsFunction.funcSetRegisterValue($"{base.Name}-{varListView.Name}-Order{varColumnIndex}", e.NewDisplayIndex.ToString(), pGlobal: false);
			}
		}
	}

	private async void tsbExportar_Click(object sender, EventArgs e)
	{
		await clsScreenGeral.funcExportToCsvAsync(lstDocs);
	}

	private async void txDocNote_Validated(object sender, EventArgs e)
	{
		await funcUpdateNote();
	}

	private async Task funcUpdateNote()
	{
		ListViewItem varListItem = null;
		foreach (ListViewItem varclsItem in funcGetSelectedItens())
		{
			if (varclsItem.Tag.GetType() == typeof(Document))
			{
				varListItem = varclsItem;
				break;
			}
		}
		if (varListItem != null)
		{
			Document varclsDoc = (Document)varListItem.Tag;
			if (varclsDoc != null && !clsFunction.IsEqual(varclsDoc.DocNote, txDocNote.Text))
			{
				await varclsDataDoc.funcSetDocNoteAsync(varclsDoc, txDocNote.Text);
			}
		}
	}

	private List<ListViewItem> funcGetSelectedItens()
	{
		List<ListViewItem> varListItens = new List<ListViewItem>();
		foreach (ListViewItem varclsItem in lstDocs.SelectedItems)
		{
			varListItens.Add(varclsItem);
		}
		if (varListItens.Count <= 0 && lstDocs.FocusedItem != null)
		{
			varListItens.Add(lstDocs.FocusedItem);
		}
		return varListItens;
	}

	private async void tsbChanges_Click(object sender, EventArgs e)
	{
		string varFeatureIdCode = clsFeatureService.consChangeManager;
		await new clsDataParameter().funcAddCounterAsync(varFeatureIdCode + "-CLICKS");
		if (clsFunction.Contains(await new clsFeatureService().funcGetFeatTypeAsync(varFeatureIdCode), "LOCK"))
		{
			await new clsManGeral().funcGetSalesActionAsync(this, varFeatureIdCode);
			return;
		}
		Document varclsObject = null;
		foreach (ListViewItem varclsItem in funcGetSelectedItens())
		{
			if (!(varclsItem.Tag.GetType() != typeof(Document)))
			{
				varclsObject = (Document)varclsItem.Tag;
			}
		}
		if (varclsObject == null)
		{
			return;
		}
		string varTypeName = varclsObject.GetType().Name;
		if (!(await clsScreenGeral.funcHasAccessAsync("CHANGE-MANAGER", "MANAGER", varTypeName)))
		{
			return;
		}
		using frmChangeManager varfrmChangeManager = new frmChangeManager(varclsObject);
		varfrmChangeManager.TopMost = false;
		varfrmChangeManager.ShowDialog(this);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor.frmDocViewer));
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
		this.clDFeSource = new System.Windows.Forms.ColumnHeader();
		this.clDFeDtLoad = new System.Windows.Forms.ColumnHeader();
		this.clDFeHrLoad = new System.Windows.Forms.ColumnHeader();
		this.clXmlSource = new System.Windows.Forms.ColumnHeader();
		this.clXmlDtLoad = new System.Windows.Forms.ColumnHeader();
		this.clXmlHrLoad = new System.Windows.Forms.ColumnHeader();
		this.clDFeDtStat = new System.Windows.Forms.ColumnHeader();
		this.clDFeHrStat = new System.Windows.Forms.ColumnHeader();
		this.clEmail = new System.Windows.Forms.ColumnHeader();
		this.clMachine = new System.Windows.Forms.ColumnHeader();
		this.clAgent = new System.Windows.Forms.ColumnHeader();
		this.contextMenuDocs = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.tsmCopyDocKey = new System.Windows.Forms.ToolStripMenuItem();
		this.ImageListDocs = new System.Windows.Forms.ImageList(this.components);
		this.splitList = new System.Windows.Forms.SplitContainer();
		this.tabDocs = new System.Windows.Forms.TabControl();
		this.tabDocXml = new System.Windows.Forms.TabPage();
		this.wbsXMLViewer = new System.Windows.Forms.WebBrowser();
		this.tabDocFast = new System.Windows.Forms.TabPage();
		this.pvcPdfViewer = new FastReport.Preview.PreviewControl();
		this.tabDocPdf = new System.Windows.Forms.TabPage();
		this.cntPdfViewer = new PdfiumViewer.PdfViewer();
		this.tabDocWbs = new System.Windows.Forms.TabPage();
		this.wbsPdfViewer = new System.Windows.Forms.WebBrowser();
		this.tabDocEdi = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.lkbMessageEdi = new System.Windows.Forms.LinkLabel();
		this.lbMessageEdi = new System.Windows.Forms.Label();
		this.picMessageEdi = new System.Windows.Forms.PictureBox();
		this.txtEdiViewer = new System.Windows.Forms.TextBox();
		this.tabDocTree = new System.Windows.Forms.TabPage();
		this.trvDocs = new System.Windows.Forms.TreeView();
		this.tabDocNote = new System.Windows.Forms.TabPage();
		this.txDocNote = new System.Windows.Forms.TextBox();
		this.tabDocInfo = new System.Windows.Forms.TabPage();
		this.txAgent = new System.Windows.Forms.Label();
		this.lbAgent = new System.Windows.Forms.Label();
		this.txMachine = new System.Windows.Forms.Label();
		this.lbMachine = new System.Windows.Forms.Label();
		this.txXmlDtLoad = new System.Windows.Forms.Label();
		this.lbXmlDtLoad = new System.Windows.Forms.Label();
		this.txDFeDtLoad = new System.Windows.Forms.Label();
		this.lbDFeDtLoad = new System.Windows.Forms.Label();
		this.txEmail = new System.Windows.Forms.Label();
		this.lbManifLine = new System.Windows.Forms.Label();
		this.lbManifSource = new System.Windows.Forms.Label();
		this.lbEmail = new System.Windows.Forms.Label();
		this.txXmlSource = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lbXmlSourceTitle = new System.Windows.Forms.Label();
		this.lbXmlSource = new System.Windows.Forms.Label();
		this.txDFeSource = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lbDFeSourceTitle = new System.Windows.Forms.Label();
		this.lbDFeSource = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.tsbSendAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarPDF = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarXML = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.tsmBaixarEDI = new System.Windows.Forms.ToolStripDropDownButton();
		this.tsbBaixarEDI = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbBaixarEDIAll = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbSendEDIOthers = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarEDIXML = new System.Windows.Forms.ToolStripMenuItem();
		this.tsbBaixarEDIPDF = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbExportar = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.tsbChanges = new System.Windows.Forms.ToolStripButton();
		this.txTagUser = new System.Windows.Forms.Label();
		this.lbTagLine = new System.Windows.Forms.Label();
		this.lbTagTitle = new System.Windows.Forms.Label();
		this.lbTagUser = new System.Windows.Forms.Label();
		this.txTagDtHr = new System.Windows.Forms.Label();
		this.lbTagDtHr = new System.Windows.Forms.Label();
		this.txDocNoteDtHr = new System.Windows.Forms.Label();
		this.lbDocNoteDtHr = new System.Windows.Forms.Label();
		this.txDocNoteUser = new System.Windows.Forms.Label();
		this.lbDocNoteLine = new System.Windows.Forms.Label();
		this.lbDocNoteTitle = new System.Windows.Forms.Label();
		this.lbDocNoteUser = new System.Windows.Forms.Label();
		this.contextMenuDocs.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitList).BeginInit();
		this.splitList.Panel1.SuspendLayout();
		this.splitList.Panel2.SuspendLayout();
		this.splitList.SuspendLayout();
		this.tabDocs.SuspendLayout();
		this.tabDocXml.SuspendLayout();
		this.tabDocFast.SuspendLayout();
		this.tabDocPdf.SuspendLayout();
		this.tabDocWbs.SuspendLayout();
		this.tabDocEdi.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picMessageEdi).BeginInit();
		this.tabDocTree.SuspendLayout();
		this.tabDocNote.SuspendLayout();
		this.tabDocInfo.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.lstDocs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[21]
		{
			this.clHasXml, this.clDoc, this.clSerie, this.clTipo, this.clEvent, this.clSeq, this.clData, this.clHora, this.clProtoc, this.clMotivo,
			this.clDFeSource, this.clDFeDtLoad, this.clDFeHrLoad, this.clXmlSource, this.clXmlDtLoad, this.clXmlHrLoad, this.clDFeDtStat, this.clDFeHrStat, this.clEmail, this.clMachine,
			this.clAgent
		});
		this.lstDocs.ContextMenuStrip = this.contextMenuDocs;
		this.lstDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lstDocs.FullRowSelect = true;
		this.lstDocs.HideSelection = false;
		this.lstDocs.Location = new System.Drawing.Point(0, 0);
		this.lstDocs.MultiSelect = false;
		this.lstDocs.Name = "lstDocs";
		this.lstDocs.ShowItemToolTips = true;
		this.lstDocs.Size = new System.Drawing.Size(1042, 140);
		this.lstDocs.SmallImageList = this.ImageListDocs;
		this.lstDocs.TabIndex = 0;
		this.lstDocs.UseCompatibleStateImageBehavior = false;
		this.lstDocs.View = System.Windows.Forms.View.Details;
		this.lstDocs.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(lstView_ColumnReordered);
		this.lstDocs.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lstView_ColumnWidthChanged);
		this.lstDocs.SelectedIndexChanged += new System.EventHandler(lstDocs_SelectedIndexChanged);
		this.clHasXml.Text = "";
		this.clHasXml.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clHasXml.Width = 29;
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
		this.clProtoc.Width = 80;
		this.clMotivo.Text = "Status";
		this.clMotivo.Width = 212;
		this.clDFeSource.Text = "DocFonte";
		this.clDFeSource.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDFeSource.Width = 10;
		this.clDFeDtLoad.Text = "DocData";
		this.clDFeDtLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDFeDtLoad.Width = 10;
		this.clDFeHrLoad.Text = "DocHora";
		this.clDFeHrLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDFeHrLoad.Width = 10;
		this.clXmlSource.Text = "XmlFonte";
		this.clXmlSource.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clXmlSource.Width = 10;
		this.clXmlDtLoad.Text = "XmlData";
		this.clXmlDtLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clXmlDtLoad.Width = 10;
		this.clXmlHrLoad.Text = "XmlHora";
		this.clXmlHrLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clXmlHrLoad.Width = 10;
		this.clDFeDtStat.Text = "DtStatus";
		this.clDFeDtStat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDFeDtStat.Width = 10;
		this.clDFeHrStat.Text = "HrStatus";
		this.clDFeHrStat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.clDFeHrStat.Width = 10;
		this.clEmail.Text = "Email";
		this.clEmail.Width = 10;
		this.clMachine.Text = "Máquina";
		this.clMachine.Width = 10;
		this.clAgent.Text = "Agent";
		this.clAgent.Width = 10;
		this.contextMenuDocs.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.tsmCopyDocKey });
		this.contextMenuDocs.Name = "contextMenuDocs";
		this.contextMenuDocs.Size = new System.Drawing.Size(269, 26);
		this.tsmCopyDocKey.Name = "tsmCopyDocKey";
		this.tsmCopyDocKey.Size = new System.Drawing.Size(268, 22);
		this.tsmCopyDocKey.Text = "Copiar Chave de Acesso [ CTRL + C ]";
		this.tsmCopyDocKey.Click += new System.EventHandler(tsmCopyDocKey_Click);
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
		this.ImageListDocs.Images.SetKeyName(17, "image_pdf_file.png");
		this.ImageListDocs.Images.SetKeyName(18, "image_ediproceda.png");
		this.ImageListDocs.Images.SetKeyName(19, "image_treview.png");
		this.ImageListDocs.Images.SetKeyName(20, "image_userdata.png");
		this.splitList.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitList.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitList.Location = new System.Drawing.Point(0, 0);
		this.splitList.Name = "splitList";
		this.splitList.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitList.Panel1.Controls.Add(this.lstDocs);
		this.splitList.Panel1MinSize = 140;
		this.splitList.Panel2.Controls.Add(this.tabDocs);
		this.splitList.Panel2.Controls.Add(this.toolStrip1);
		this.splitList.Size = new System.Drawing.Size(1042, 468);
		this.splitList.SplitterDistance = 140;
		this.splitList.TabIndex = 2;
		this.tabDocs.Controls.Add(this.tabDocXml);
		this.tabDocs.Controls.Add(this.tabDocFast);
		this.tabDocs.Controls.Add(this.tabDocPdf);
		this.tabDocs.Controls.Add(this.tabDocWbs);
		this.tabDocs.Controls.Add(this.tabDocEdi);
		this.tabDocs.Controls.Add(this.tabDocTree);
		this.tabDocs.Controls.Add(this.tabDocNote);
		this.tabDocs.Controls.Add(this.tabDocInfo);
		this.tabDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabDocs.ImageList = this.ImageListDocs;
		this.tabDocs.ItemSize = new System.Drawing.Size(56, 19);
		this.tabDocs.Location = new System.Drawing.Point(0, 25);
		this.tabDocs.Name = "tabDocs";
		this.tabDocs.SelectedIndex = 0;
		this.tabDocs.Size = new System.Drawing.Size(1042, 299);
		this.tabDocs.TabIndex = 6;
		this.tabDocXml.Controls.Add(this.wbsXMLViewer);
		this.tabDocXml.ImageIndex = 0;
		this.tabDocXml.Location = new System.Drawing.Point(4, 23);
		this.tabDocXml.Name = "tabDocXml";
		this.tabDocXml.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocXml.Size = new System.Drawing.Size(1034, 272);
		this.tabDocXml.TabIndex = 1;
		this.tabDocXml.Text = "XML";
		this.tabDocXml.UseVisualStyleBackColor = true;
		this.wbsXMLViewer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.wbsXMLViewer.Location = new System.Drawing.Point(3, 3);
		this.wbsXMLViewer.MinimumSize = new System.Drawing.Size(23, 20);
		this.wbsXMLViewer.Name = "wbsXMLViewer";
		this.wbsXMLViewer.ScriptErrorsSuppressed = true;
		this.wbsXMLViewer.Size = new System.Drawing.Size(1028, 266);
		this.wbsXMLViewer.TabIndex = 2;
		this.tabDocFast.Controls.Add(this.pvcPdfViewer);
		this.tabDocFast.ImageIndex = 17;
		this.tabDocFast.Location = new System.Drawing.Point(4, 23);
		this.tabDocFast.Name = "tabDocFast";
		this.tabDocFast.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocFast.Size = new System.Drawing.Size(1034, 272);
		this.tabDocFast.TabIndex = 0;
		this.tabDocFast.Text = "PDF";
		this.tabDocFast.UseVisualStyleBackColor = true;
		this.pvcPdfViewer.BackColor = System.Drawing.Color.Transparent;
		this.pvcPdfViewer.Buttons = FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save | FastReport.PreviewButtons.Find | FastReport.PreviewButtons.Zoom | FastReport.PreviewButtons.Navigator;
		this.pvcPdfViewer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pvcPdfViewer.Font = new System.Drawing.Font("Tahoma", 8f);
		this.pvcPdfViewer.Location = new System.Drawing.Point(3, 3);
		this.pvcPdfViewer.Name = "pvcPdfViewer";
		this.pvcPdfViewer.PageOffset = new System.Drawing.Point(10, 10);
		this.pvcPdfViewer.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.pvcPdfViewer.SaveInitialDirectory = null;
		this.pvcPdfViewer.Size = new System.Drawing.Size(1028, 266);
		this.pvcPdfViewer.TabIndex = 0;
		this.pvcPdfViewer.UIStyle = FastReport.Utils.UIStyle.VistaGlass;
		this.tabDocPdf.Controls.Add(this.cntPdfViewer);
		this.tabDocPdf.ImageIndex = 17;
		this.tabDocPdf.Location = new System.Drawing.Point(4, 23);
		this.tabDocPdf.Name = "tabDocPdf";
		this.tabDocPdf.Size = new System.Drawing.Size(1034, 272);
		this.tabDocPdf.TabIndex = 3;
		this.tabDocPdf.Text = "PDF";
		this.tabDocPdf.UseVisualStyleBackColor = true;
		this.cntPdfViewer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cntPdfViewer.Location = new System.Drawing.Point(0, 0);
		this.cntPdfViewer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.cntPdfViewer.Name = "cntPdfViewer";
		this.cntPdfViewer.ShowToolbar = false;
		this.cntPdfViewer.Size = new System.Drawing.Size(1034, 272);
		this.cntPdfViewer.TabIndex = 0;
		this.cntPdfViewer.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitWidth;
		this.tabDocWbs.Controls.Add(this.wbsPdfViewer);
		this.tabDocWbs.ImageIndex = 17;
		this.tabDocWbs.Location = new System.Drawing.Point(4, 23);
		this.tabDocWbs.Name = "tabDocWbs";
		this.tabDocWbs.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocWbs.Size = new System.Drawing.Size(1034, 272);
		this.tabDocWbs.TabIndex = 6;
		this.tabDocWbs.Text = "PDF";
		this.tabDocWbs.UseVisualStyleBackColor = true;
		this.wbsPdfViewer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.wbsPdfViewer.IsWebBrowserContextMenuEnabled = false;
		this.wbsPdfViewer.Location = new System.Drawing.Point(3, 3);
		this.wbsPdfViewer.MinimumSize = new System.Drawing.Size(20, 20);
		this.wbsPdfViewer.Name = "wbsPdfViewer";
		this.wbsPdfViewer.Size = new System.Drawing.Size(1028, 266);
		this.wbsPdfViewer.TabIndex = 0;
		this.tabDocEdi.Controls.Add(this.panel7);
		this.tabDocEdi.Controls.Add(this.txtEdiViewer);
		this.tabDocEdi.ImageIndex = 18;
		this.tabDocEdi.Location = new System.Drawing.Point(4, 23);
		this.tabDocEdi.Name = "tabDocEdi";
		this.tabDocEdi.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocEdi.Size = new System.Drawing.Size(1034, 272);
		this.tabDocEdi.TabIndex = 4;
		this.tabDocEdi.Text = "EDI ";
		this.tabDocEdi.UseVisualStyleBackColor = true;
		this.panel7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel7.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
		this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel7.Controls.Add(this.lkbMessageEdi);
		this.panel7.Controls.Add(this.lbMessageEdi);
		this.panel7.Controls.Add(this.picMessageEdi);
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1034, 28);
		this.panel7.TabIndex = 6;
		this.lkbMessageEdi.AutoSize = true;
		this.lkbMessageEdi.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lkbMessageEdi.Location = new System.Drawing.Point(314, 6);
		this.lkbMessageEdi.Name = "lkbMessageEdi";
		this.lkbMessageEdi.Size = new System.Drawing.Size(87, 13);
		this.lkbMessageEdi.TabIndex = 105;
		this.lkbMessageEdi.TabStop = true;
		this.lkbMessageEdi.Text = "Clique aqui !!!";
		this.lkbMessageEdi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lkbMessageEdi.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lkbMessageEdi_LinkClicked);
		this.lbMessageEdi.AutoSize = true;
		this.lbMessageEdi.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lbMessageEdi.ForeColor = System.Drawing.Color.Blue;
		this.lbMessageEdi.Location = new System.Drawing.Point(30, 6);
		this.lbMessageEdi.Name = "lbMessageEdi";
		this.lbMessageEdi.Size = new System.Drawing.Size(272, 13);
		this.lbMessageEdi.TabIndex = 103;
		this.lbMessageEdi.Text = "Deseja obter mais informações sobre o EDI?  ";
		this.lbMessageEdi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.picMessageEdi.Image = Monitor.Resources.image_help;
		this.picMessageEdi.Location = new System.Drawing.Point(3, 2);
		this.picMessageEdi.Name = "picMessageEdi";
		this.picMessageEdi.Size = new System.Drawing.Size(20, 20);
		this.picMessageEdi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picMessageEdi.TabIndex = 102;
		this.picMessageEdi.TabStop = false;
		this.txtEdiViewer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtEdiViewer.BackColor = System.Drawing.SystemColors.HotTrack;
		this.txtEdiViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEdiViewer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEdiViewer.ForeColor = System.Drawing.SystemColors.Window;
		this.txtEdiViewer.Location = new System.Drawing.Point(0, 30);
		this.txtEdiViewer.Multiline = true;
		this.txtEdiViewer.Name = "txtEdiViewer";
		this.txtEdiViewer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtEdiViewer.Size = new System.Drawing.Size(1038, 240);
		this.txtEdiViewer.TabIndex = 0;
		this.txtEdiViewer.WordWrap = false;
		this.tabDocTree.Controls.Add(this.trvDocs);
		this.tabDocTree.ImageIndex = 19;
		this.tabDocTree.Location = new System.Drawing.Point(4, 23);
		this.tabDocTree.Name = "tabDocTree";
		this.tabDocTree.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocTree.Size = new System.Drawing.Size(1034, 272);
		this.tabDocTree.TabIndex = 2;
		this.tabDocTree.Text = "Documentos";
		this.tabDocTree.UseVisualStyleBackColor = true;
		this.trvDocs.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.trvDocs.ContextMenuStrip = this.contextMenuDocs;
		this.trvDocs.Dock = System.Windows.Forms.DockStyle.Fill;
		this.trvDocs.HotTracking = true;
		this.trvDocs.Location = new System.Drawing.Point(3, 3);
		this.trvDocs.Name = "trvDocs";
		this.trvDocs.Size = new System.Drawing.Size(1028, 266);
		this.trvDocs.TabIndex = 0;
		this.trvDocs.DoubleClick += new System.EventHandler(trvDocs_DoubleClick);
		this.tabDocNote.Controls.Add(this.txDocNote);
		this.tabDocNote.ImageIndex = 14;
		this.tabDocNote.Location = new System.Drawing.Point(4, 23);
		this.tabDocNote.Name = "tabDocNote";
		this.tabDocNote.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocNote.Size = new System.Drawing.Size(1034, 272);
		this.tabDocNote.TabIndex = 5;
		this.tabDocNote.Text = "Comentários";
		this.tabDocNote.UseVisualStyleBackColor = true;
		this.txDocNote.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txDocNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txDocNote.Location = new System.Drawing.Point(2, 3);
		this.txDocNote.MaxLength = 1000;
		this.txDocNote.Multiline = true;
		this.txDocNote.Name = "txDocNote";
		this.txDocNote.Size = new System.Drawing.Size(1030, 266);
		this.txDocNote.TabIndex = 0;
		this.txDocNote.Validated += new System.EventHandler(txDocNote_Validated);
		this.tabDocInfo.Controls.Add(this.txDocNoteDtHr);
		this.tabDocInfo.Controls.Add(this.lbDocNoteDtHr);
		this.tabDocInfo.Controls.Add(this.txDocNoteUser);
		this.tabDocInfo.Controls.Add(this.lbDocNoteLine);
		this.tabDocInfo.Controls.Add(this.lbDocNoteTitle);
		this.tabDocInfo.Controls.Add(this.lbDocNoteUser);
		this.tabDocInfo.Controls.Add(this.txTagDtHr);
		this.tabDocInfo.Controls.Add(this.lbTagDtHr);
		this.tabDocInfo.Controls.Add(this.txTagUser);
		this.tabDocInfo.Controls.Add(this.lbTagLine);
		this.tabDocInfo.Controls.Add(this.lbTagTitle);
		this.tabDocInfo.Controls.Add(this.lbTagUser);
		this.tabDocInfo.Controls.Add(this.txAgent);
		this.tabDocInfo.Controls.Add(this.lbAgent);
		this.tabDocInfo.Controls.Add(this.txMachine);
		this.tabDocInfo.Controls.Add(this.lbMachine);
		this.tabDocInfo.Controls.Add(this.txXmlDtLoad);
		this.tabDocInfo.Controls.Add(this.lbXmlDtLoad);
		this.tabDocInfo.Controls.Add(this.txDFeDtLoad);
		this.tabDocInfo.Controls.Add(this.lbDFeDtLoad);
		this.tabDocInfo.Controls.Add(this.txEmail);
		this.tabDocInfo.Controls.Add(this.lbManifLine);
		this.tabDocInfo.Controls.Add(this.lbManifSource);
		this.tabDocInfo.Controls.Add(this.lbEmail);
		this.tabDocInfo.Controls.Add(this.txXmlSource);
		this.tabDocInfo.Controls.Add(this.label2);
		this.tabDocInfo.Controls.Add(this.lbXmlSourceTitle);
		this.tabDocInfo.Controls.Add(this.lbXmlSource);
		this.tabDocInfo.Controls.Add(this.txDFeSource);
		this.tabDocInfo.Controls.Add(this.label11);
		this.tabDocInfo.Controls.Add(this.lbDFeSourceTitle);
		this.tabDocInfo.Controls.Add(this.lbDFeSource);
		this.tabDocInfo.ImageIndex = 20;
		this.tabDocInfo.Location = new System.Drawing.Point(4, 23);
		this.tabDocInfo.Name = "tabDocInfo";
		this.tabDocInfo.Padding = new System.Windows.Forms.Padding(3);
		this.tabDocInfo.Size = new System.Drawing.Size(1034, 272);
		this.tabDocInfo.TabIndex = 7;
		this.tabDocInfo.Text = "Rastreabilidade";
		this.tabDocInfo.UseVisualStyleBackColor = true;
		this.txAgent.AutoSize = true;
		this.txAgent.Location = new System.Drawing.Point(453, 84);
		this.txAgent.Name = "txAgent";
		this.txAgent.Size = new System.Drawing.Size(19, 13);
		this.txAgent.TabIndex = 245;
		this.txAgent.Text = "...";
		this.lbAgent.Location = new System.Drawing.Point(384, 84);
		this.lbAgent.Name = "lbAgent";
		this.lbAgent.Size = new System.Drawing.Size(63, 13);
		this.lbAgent.TabIndex = 244;
		this.lbAgent.Text = "Agente :";
		this.lbAgent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txMachine.AutoSize = true;
		this.txMachine.Location = new System.Drawing.Point(453, 59);
		this.txMachine.Name = "txMachine";
		this.txMachine.Size = new System.Drawing.Size(19, 13);
		this.txMachine.TabIndex = 243;
		this.txMachine.Text = "...";
		this.lbMachine.Location = new System.Drawing.Point(384, 59);
		this.lbMachine.Name = "lbMachine";
		this.lbMachine.Size = new System.Drawing.Size(63, 13);
		this.lbMachine.TabIndex = 242;
		this.lbMachine.Text = "Máquina :";
		this.lbMachine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txXmlDtLoad.AutoSize = true;
		this.txXmlDtLoad.Location = new System.Drawing.Point(82, 161);
		this.txXmlDtLoad.Name = "txXmlDtLoad";
		this.txXmlDtLoad.Size = new System.Drawing.Size(19, 13);
		this.txXmlDtLoad.TabIndex = 241;
		this.txXmlDtLoad.Text = "...";
		this.lbXmlDtLoad.Location = new System.Drawing.Point(13, 161);
		this.lbXmlDtLoad.Name = "lbXmlDtLoad";
		this.lbXmlDtLoad.Size = new System.Drawing.Size(63, 13);
		this.lbXmlDtLoad.TabIndex = 240;
		this.lbXmlDtLoad.Text = "Data :";
		this.lbXmlDtLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txDFeDtLoad.AutoSize = true;
		this.txDFeDtLoad.Location = new System.Drawing.Point(82, 59);
		this.txDFeDtLoad.Name = "txDFeDtLoad";
		this.txDFeDtLoad.Size = new System.Drawing.Size(19, 13);
		this.txDFeDtLoad.TabIndex = 239;
		this.txDFeDtLoad.Text = "...";
		this.lbDFeDtLoad.Location = new System.Drawing.Point(13, 59);
		this.lbDFeDtLoad.Name = "lbDFeDtLoad";
		this.lbDFeDtLoad.Size = new System.Drawing.Size(63, 13);
		this.lbDFeDtLoad.TabIndex = 238;
		this.lbDFeDtLoad.Text = "Data :";
		this.lbDFeDtLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txEmail.AutoSize = true;
		this.txEmail.Location = new System.Drawing.Point(453, 37);
		this.txEmail.Name = "txEmail";
		this.txEmail.Size = new System.Drawing.Size(19, 13);
		this.txEmail.TabIndex = 237;
		this.txEmail.Text = "...";
		this.lbManifLine.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbManifLine.Location = new System.Drawing.Point(384, 30);
		this.lbManifLine.Name = "lbManifLine";
		this.lbManifLine.Size = new System.Drawing.Size(300, 1);
		this.lbManifLine.TabIndex = 236;
		this.lbManifSource.AutoSize = true;
		this.lbManifSource.Location = new System.Drawing.Point(384, 13);
		this.lbManifSource.Name = "lbManifSource";
		this.lbManifSource.Size = new System.Drawing.Size(81, 13);
		this.lbManifSource.TabIndex = 235;
		this.lbManifSource.Text = "Manifestação";
		this.lbEmail.Location = new System.Drawing.Point(384, 37);
		this.lbEmail.Name = "lbEmail";
		this.lbEmail.Size = new System.Drawing.Size(63, 13);
		this.lbEmail.TabIndex = 234;
		this.lbEmail.Text = "Email :";
		this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txXmlSource.AutoSize = true;
		this.txXmlSource.Location = new System.Drawing.Point(82, 137);
		this.txXmlSource.Name = "txXmlSource";
		this.txXmlSource.Size = new System.Drawing.Size(19, 13);
		this.txXmlSource.TabIndex = 233;
		this.txXmlSource.Text = "...";
		this.label2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(11, 130);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(300, 1);
		this.label2.TabIndex = 232;
		this.lbXmlSourceTitle.AutoSize = true;
		this.lbXmlSourceTitle.Location = new System.Drawing.Point(13, 113);
		this.lbXmlSourceTitle.Name = "lbXmlSourceTitle";
		this.lbXmlSourceTitle.Size = new System.Drawing.Size(94, 13);
		this.lbXmlSourceTitle.TabIndex = 231;
		this.lbXmlSourceTitle.Text = "Origem do XML";
		this.lbXmlSource.Location = new System.Drawing.Point(13, 137);
		this.lbXmlSource.Name = "lbXmlSource";
		this.lbXmlSource.Size = new System.Drawing.Size(63, 13);
		this.lbXmlSource.TabIndex = 230;
		this.lbXmlSource.Text = "Fonte :";
		this.lbXmlSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txDFeSource.AutoSize = true;
		this.txDFeSource.Location = new System.Drawing.Point(82, 37);
		this.txDFeSource.Name = "txDFeSource";
		this.txDFeSource.Size = new System.Drawing.Size(19, 13);
		this.txDFeSource.TabIndex = 229;
		this.txDFeSource.Text = "...";
		this.label11.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label11.Location = new System.Drawing.Point(11, 30);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(300, 1);
		this.label11.TabIndex = 228;
		this.lbDFeSourceTitle.AutoSize = true;
		this.lbDFeSourceTitle.Location = new System.Drawing.Point(13, 13);
		this.lbDFeSourceTitle.Name = "lbDFeSourceTitle";
		this.lbDFeSourceTitle.Size = new System.Drawing.Size(115, 13);
		this.lbDFeSourceTitle.TabIndex = 227;
		this.lbDFeSourceTitle.Text = "Origem do registro";
		this.lbDFeSource.Location = new System.Drawing.Point(13, 37);
		this.lbDFeSource.Name = "lbDFeSource";
		this.lbDFeSource.Size = new System.Drawing.Size(63, 13);
		this.lbDFeSource.TabIndex = 0;
		this.lbDFeSource.Text = "Fonte :";
		this.lbDFeSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.tsbSendAll, this.toolStripSeparator3, this.tsbBaixarAll, this.toolStripSeparator1, this.tsbBaixarPDF, this.toolStripSeparator2, this.tsbBaixarXML, this.toolStripSeparator4, this.tsmBaixarEDI, this.toolStripSeparator7,
			this.tsbExportar, this.toolStripSeparator8, this.tsbChanges
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1042, 25);
		this.toolStrip1.TabIndex = 5;
		this.toolStrip1.Text = "toolStrip1";
		this.tsbSendAll.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbSendAll.Image = (System.Drawing.Image)resources.GetObject("tsbSendAll.Image");
		this.tsbSendAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbSendAll.Name = "tsbSendAll";
		this.tsbSendAll.Size = new System.Drawing.Size(128, 22);
		this.tsbSendAll.Text = "Enviar por email";
		this.tsbSendAll.Click += new System.EventHandler(tsbSendAll_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.tsbBaixarAll.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarAll.Image = Monitor.Resources.image_download;
		this.tsbBaixarAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbBaixarAll.Name = "tsbBaixarAll";
		this.tsbBaixarAll.Size = new System.Drawing.Size(151, 22);
		this.tsbBaixarAll.Text = "Exportar XML + PDF";
		this.tsbBaixarAll.Click += new System.EventHandler(tsbBaixarAll_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tsbBaixarPDF.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarPDF.Image = Monitor.Resources.image_pdf_file;
		this.tsbBaixarPDF.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbBaixarPDF.Name = "tsbBaixarPDF";
		this.tsbBaixarPDF.Size = new System.Drawing.Size(109, 22);
		this.tsbBaixarPDF.Text = "Exportar PDF";
		this.tsbBaixarPDF.Click += new System.EventHandler(tsbBaixarPDF_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.tsbBaixarXML.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarXML.Image = Monitor.Resources.image_xml_blue;
		this.tsbBaixarXML.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbBaixarXML.Name = "tsbBaixarXML";
		this.tsbBaixarXML.Size = new System.Drawing.Size(110, 22);
		this.tsbBaixarXML.Text = "Exportar XML";
		this.tsbBaixarXML.Click += new System.EventHandler(tsbBaixarXML_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.tsmBaixarEDI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.tsbBaixarEDI, this.toolStripSeparator5, this.tsbBaixarEDIAll, this.toolStripSeparator6, this.tsbSendEDIOthers });
		this.tsmBaixarEDI.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsmBaixarEDI.Image = Monitor.Resources.image_ediproceda;
		this.tsmBaixarEDI.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsmBaixarEDI.Name = "tsmBaixarEDI";
		this.tsmBaixarEDI.Size = new System.Drawing.Size(58, 22);
		this.tsmBaixarEDI.Tag = "#NOT-TABPARTNER";
		this.tsmBaixarEDI.Text = "EDI";
		this.tsbBaixarEDI.Font = new System.Drawing.Font("Verdana", 9f);
		this.tsbBaixarEDI.Name = "tsbBaixarEDI";
		this.tsbBaixarEDI.Size = new System.Drawing.Size(237, 22);
		this.tsbBaixarEDI.Text = "Exportar EDI";
		this.tsbBaixarEDI.Click += new System.EventHandler(tsbBaixarEDI_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(234, 6);
		this.tsbBaixarEDIAll.Image = Monitor.Resources.image_download;
		this.tsbBaixarEDIAll.Name = "tsbBaixarEDIAll";
		this.tsbBaixarEDIAll.Size = new System.Drawing.Size(237, 22);
		this.tsbBaixarEDIAll.Text = "Exportar EDI + XML + PDF";
		this.tsbBaixarEDIAll.Click += new System.EventHandler(tsbBaixarEDIAll_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(234, 6);
		this.tsbSendEDIOthers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.tsbBaixarEDIXML, this.tsbBaixarEDIPDF });
		this.tsbSendEDIOthers.Name = "tsbSendEDIOthers";
		this.tsbSendEDIOthers.Size = new System.Drawing.Size(237, 22);
		this.tsbSendEDIOthers.Text = "Outras opções ...";
		this.tsbBaixarEDIXML.Name = "tsbBaixarEDIXML";
		this.tsbBaixarEDIXML.Size = new System.Drawing.Size(196, 22);
		this.tsbBaixarEDIXML.Text = "Exportar EDI + XML";
		this.tsbBaixarEDIXML.Click += new System.EventHandler(tsbBaixarEDIXML_Click);
		this.tsbBaixarEDIPDF.Name = "tsbBaixarEDIPDF";
		this.tsbBaixarEDIPDF.Size = new System.Drawing.Size(196, 22);
		this.tsbBaixarEDIPDF.Text = "Exportar EDI + PDF";
		this.tsbBaixarEDIPDF.Click += new System.EventHandler(tsbBaixarEDIPDF_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
		this.tsbExportar.Image = (System.Drawing.Image)resources.GetObject("tsbExportar.Image");
		this.tsbExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbExportar.Name = "tsbExportar";
		this.tsbExportar.Size = new System.Drawing.Size(23, 22);
		this.tsbExportar.ToolTipText = "Exportar lista de documentos para o Excel";
		this.tsbExportar.Click += new System.EventHandler(tsbExportar_Click);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
		this.tsbChanges.Image = Monitor.Resources.image_changes;
		this.tsbChanges.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsbChanges.Name = "tsbChanges";
		this.tsbChanges.Size = new System.Drawing.Size(47, 22);
		this.tsbChanges.Text = "Log";
		this.tsbChanges.Click += new System.EventHandler(tsbChanges_Click);
		this.txTagUser.AutoSize = true;
		this.txTagUser.Location = new System.Drawing.Point(453, 137);
		this.txTagUser.Name = "txTagUser";
		this.txTagUser.Size = new System.Drawing.Size(19, 13);
		this.txTagUser.TabIndex = 249;
		this.txTagUser.Text = "...";
		this.lbTagLine.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbTagLine.Location = new System.Drawing.Point(384, 130);
		this.lbTagLine.Name = "lbTagLine";
		this.lbTagLine.Size = new System.Drawing.Size(300, 1);
		this.lbTagLine.TabIndex = 248;
		this.lbTagTitle.AutoSize = true;
		this.lbTagTitle.Location = new System.Drawing.Point(384, 113);
		this.lbTagTitle.Name = "lbTagTitle";
		this.lbTagTitle.Size = new System.Drawing.Size(53, 13);
		this.lbTagTitle.TabIndex = 247;
		this.lbTagTitle.Text = "Etiqueta";
		this.lbTagUser.Location = new System.Drawing.Point(384, 137);
		this.lbTagUser.Name = "lbTagUser";
		this.lbTagUser.Size = new System.Drawing.Size(63, 13);
		this.lbTagUser.TabIndex = 246;
		this.lbTagUser.Text = "Email :";
		this.lbTagUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txTagDtHr.AutoSize = true;
		this.txTagDtHr.Location = new System.Drawing.Point(453, 161);
		this.txTagDtHr.Name = "txTagDtHr";
		this.txTagDtHr.Size = new System.Drawing.Size(19, 13);
		this.txTagDtHr.TabIndex = 251;
		this.txTagDtHr.Text = "...";
		this.lbTagDtHr.Location = new System.Drawing.Point(384, 161);
		this.lbTagDtHr.Name = "lbTagDtHr";
		this.lbTagDtHr.Size = new System.Drawing.Size(63, 13);
		this.lbTagDtHr.TabIndex = 250;
		this.lbTagDtHr.Text = "Data :";
		this.lbTagDtHr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txDocNoteDtHr.AutoSize = true;
		this.txDocNoteDtHr.Location = new System.Drawing.Point(453, 239);
		this.txDocNoteDtHr.Name = "txDocNoteDtHr";
		this.txDocNoteDtHr.Size = new System.Drawing.Size(19, 13);
		this.txDocNoteDtHr.TabIndex = 257;
		this.txDocNoteDtHr.Text = "...";
		this.lbDocNoteDtHr.Location = new System.Drawing.Point(384, 239);
		this.lbDocNoteDtHr.Name = "lbDocNoteDtHr";
		this.lbDocNoteDtHr.Size = new System.Drawing.Size(63, 13);
		this.lbDocNoteDtHr.TabIndex = 256;
		this.lbDocNoteDtHr.Text = "Data :";
		this.lbDocNoteDtHr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txDocNoteUser.AutoSize = true;
		this.txDocNoteUser.Location = new System.Drawing.Point(453, 211);
		this.txDocNoteUser.Name = "txDocNoteUser";
		this.txDocNoteUser.Size = new System.Drawing.Size(19, 13);
		this.txDocNoteUser.TabIndex = 255;
		this.txDocNoteUser.Text = "...";
		this.lbDocNoteLine.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.lbDocNoteLine.Location = new System.Drawing.Point(384, 204);
		this.lbDocNoteLine.Name = "lbDocNoteLine";
		this.lbDocNoteLine.Size = new System.Drawing.Size(300, 1);
		this.lbDocNoteLine.TabIndex = 254;
		this.lbDocNoteTitle.AutoSize = true;
		this.lbDocNoteTitle.Location = new System.Drawing.Point(384, 187);
		this.lbDocNoteTitle.Name = "lbDocNoteTitle";
		this.lbDocNoteTitle.Size = new System.Drawing.Size(80, 13);
		this.lbDocNoteTitle.TabIndex = 253;
		this.lbDocNoteTitle.Text = "Comentários";
		this.lbDocNoteUser.Location = new System.Drawing.Point(384, 211);
		this.lbDocNoteUser.Name = "lbDocNoteUser";
		this.lbDocNoteUser.Size = new System.Drawing.Size(63, 13);
		this.lbDocNoteUser.TabIndex = 252;
		this.lbDocNoteUser.Text = "Email :";
		this.lbDocNoteUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1042, 468);
		base.Controls.Add(this.splitList);
		this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmDocViewer";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Fiscal.io - Visualizador de Documentos";
		base.Load += new System.EventHandler(frmDocViewer_Load);
		this.contextMenuDocs.ResumeLayout(false);
		this.splitList.Panel1.ResumeLayout(false);
		this.splitList.Panel2.ResumeLayout(false);
		this.splitList.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitList).EndInit();
		this.splitList.ResumeLayout(false);
		this.tabDocs.ResumeLayout(false);
		this.tabDocXml.ResumeLayout(false);
		this.tabDocFast.ResumeLayout(false);
		this.tabDocPdf.ResumeLayout(false);
		this.tabDocWbs.ResumeLayout(false);
		this.tabDocEdi.ResumeLayout(false);
		this.tabDocEdi.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picMessageEdi).EndInit();
		this.tabDocTree.ResumeLayout(false);
		this.tabDocNote.ResumeLayout(false);
		this.tabDocNote.PerformLayout();
		this.tabDocInfo.ResumeLayout(false);
		this.tabDocInfo.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
