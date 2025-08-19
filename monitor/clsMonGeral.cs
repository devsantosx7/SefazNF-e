using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsMonGeral
{
	public static async Task<bool> funcDocViewerAsync(Form pThis, List<Document> pDocList, bool pShowPDF = true, bool pShowXML = false, bool pShowEDI = false)
	{
		clsFeatureService varclsFeatureService = new clsFeatureService();
		string varFeatExtId = clsFeatureService.consFeatScanDocNFSeIn;
		if (clsFunction.Contains(await varclsFeatureService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
		{
			if (pDocList.FirstOrDefault((Document r) => clsFunction.funcIsNFSe(r.Model) && clsFunction.IsEqual(r.DFeSource, "SEFAZ", pIgnoreCase: true)) != null)
			{
				await new clsManGeral().funcGetSalesActionAsync(pThis, varFeatExtId);
				return false;
			}
			pDocList = pDocList.Where((Document r) => !clsFunction.funcIsNFSe(r.Model) || (clsFunction.funcIsNFSe(r.Model) && !clsFunction.IsEqual(r.DFeSource, "Sefaz", pIgnoreCase: true))).ToList();
			if (pDocList.Count == 0)
			{
				return true;
			}
		}
		varFeatExtId = clsFeatureService.consFeatScanDocCFeOut;
		if (clsFunction.Contains(await varclsFeatureService.funcGetFeatTypeAsync(varFeatExtId), "LOCK"))
		{
			if (pDocList.FirstOrDefault((Document r) => clsFunction.funcIsCFeSat(r.Model) && clsFunction.IsEqual(r.DFeSource, "SEFAZ", pIgnoreCase: true)) != null)
			{
				await new clsManGeral().funcGetSalesActionAsync(pThis, varFeatExtId);
				return false;
			}
			pDocList = pDocList.Where((Document r) => !clsFunction.funcIsCFeSat(r.Model) || (clsFunction.funcIsCFeSat(r.Model) && !clsFunction.IsEqual(r.DFeSource, "Sefaz", pIgnoreCase: true))).ToList();
			if (pDocList.Count == 0)
			{
				return true;
			}
		}
		frmDocViewer obj = new frmDocViewer(pDocList, pShowPDF, pShowXML, pShowEDI);
		obj.ShowDialog(pThis);
		obj.Dispose();
		return true;
	}

	public static async Task<bool> funcLoadTagsAsync(Hashtable pHasColors, Hashtable pTagList, ContextMenuStrip pclsMenu, EventHandler pActCopyKey, EventHandler pActTransfer, EventHandler pActTagCode, EventHandler pActDocNote, EventHandler pActFiscReset, EventHandler pActRemoveDocNote)
	{
		new clsDataTag();
		clsDataWorkFlow varclsDataWork = new clsDataWorkFlow();
		(await new clsDbaFactory().funcGetClassAsync()).funcGetDbaType();
		string varFeatAutoTagDet = clsFeatureService.consFeatAutTagDeter;
		string varFeatType01 = await new clsFeatureService().funcGetFeatTypeAsync(varFeatAutoTagDet);
		pTagList.Clear();
		pHasColors.Clear();
		pclsMenu.Items.Clear();
		foreach (Tag varTag in await new clsDataTag().funcGetListAsync())
		{
			pTagList.Add(varTag.Code, varTag.Nome);
			pHasColors.Add(varTag.Code, varTag.Color);
		}
		if (pActCopyKey != null)
		{
			ToolStripMenuItem varCtxMenuDocKey = new ToolStripMenuItem();
			varCtxMenuDocKey.Tag = "";
			varCtxMenuDocKey.Text = "CTRL+C : Copiar Chave de Acesso";
			varCtxMenuDocKey.BackColor = Color.White;
			varCtxMenuDocKey.Click += pActCopyKey.Invoke;
			pclsMenu.Items.Add(varCtxMenuDocKey);
		}
		if (pActTransfer != null)
		{
			ToolStripSeparator varSeparator01 = new ToolStripSeparator();
			varSeparator01.Tag = "TRANSFER";
			pclsMenu.Items.Add(varSeparator01);
			ToolStripMenuItem varCtxMenuTransfer = new ToolStripMenuItem();
			varCtxMenuTransfer.Tag = "TRANSFER";
			varCtxMenuTransfer.Text = "CTRL+T : Transferir de Empresa";
			varCtxMenuTransfer.BackColor = Color.White;
			varCtxMenuTransfer.Click += pActTransfer.Invoke;
			pclsMenu.Items.Add(varCtxMenuTransfer);
		}
		List<Tag> varTagListAccess = await new clsDataTag().funcGetListAsync(pJustWithAccess: true);
		if (pActTagCode != null)
		{
			ToolStripSeparator varSeparator2 = new ToolStripSeparator();
			varSeparator2.Text = string.Empty;
			pclsMenu.Items.Add(varSeparator2);
			foreach (Tag varTag2 in varTagListAccess)
			{
				ToolStripMenuItem varNewTagMenu = new ToolStripMenuItem();
				varNewTagMenu.Tag = varTag2.Code;
				varNewTagMenu.Text = varTag2.Nome;
				varNewTagMenu.BackColor = ColorTranslator.FromHtml(varTag2.Color);
				varNewTagMenu.Click += pActTagCode.Invoke;
				pclsMenu.Items.Add(varNewTagMenu);
			}
			List<WorkFlow> varWorkList = await varclsDataWork.funcGetListAsync("PROC-SETTING-TAG");
			if (varFeatType01.Contains("LOCK"))
			{
				varWorkList.Clear();
			}
			if (varWorkList.Count > 0)
			{
				pclsMenu.Items.Add(new ToolStripSeparator());
				ToolStripMenuItem varAutoTagMenu = new ToolStripMenuItem();
				varAutoTagMenu.Tag = "";
				varAutoTagMenu.Text = "Determinação automática";
				pclsMenu.Items.Add(varAutoTagMenu);
				foreach (WorkFlow varclsItem in varWorkList)
				{
					ToolStripMenuItem varAutoTagItem = new ToolStripMenuItem();
					varAutoTagItem.Tag = varclsItem.ID;
					varAutoTagItem.Text = varclsItem.Description;
					varAutoTagItem.Click += pActTagCode.Invoke;
					varAutoTagMenu.DropDownItems.Add(varAutoTagItem);
				}
			}
			pclsMenu.Items.Add(new ToolStripSeparator());
			ToolStripMenuItem varEmptyCtxMenu = new ToolStripMenuItem();
			varEmptyCtxMenu.Tag = "";
			varEmptyCtxMenu.Text = "Sem Etiqueta";
			varEmptyCtxMenu.BackColor = Color.White;
			varEmptyCtxMenu.Click += pActTagCode.Invoke;
			pclsMenu.Items.Add(varEmptyCtxMenu);
		}
		if (pActDocNote != null)
		{
			ToolStripSeparator varSeparator4 = new ToolStripSeparator();
			varSeparator4.Text = string.Empty;
			pclsMenu.Items.Add(varSeparator4);
			ToolStripMenuItem varNewDocNote = new ToolStripMenuItem();
			varNewDocNote.Image = Resources.image_notes;
			varNewDocNote.Text = "Atribuir comentário";
			varNewDocNote.Click += pActDocNote.Invoke;
			pclsMenu.Items.Add(varNewDocNote);
		}
		if (pActRemoveDocNote != null)
		{
			ToolStripSeparator varSeparator6 = new ToolStripSeparator();
			varSeparator6.Text = string.Empty;
			pclsMenu.Items.Add(varSeparator6);
			ToolStripMenuItem varRemoveDocNote = new ToolStripMenuItem();
			varRemoveDocNote.Image = Resources.image_delete_object;
			varRemoveDocNote.Text = "Remover comentário";
			varRemoveDocNote.Click += pActRemoveDocNote.Invoke;
			pclsMenu.Items.Add(varRemoveDocNote);
		}
		if (pActFiscReset != null)
		{
			ToolStripSeparator varSeparator7 = new ToolStripSeparator();
			varSeparator7.Text = string.Empty;
			pclsMenu.Items.Add(varSeparator7);
			ToolStripMenuItem varNewDocReset = new ToolStripMenuItem();
			varNewDocReset.Image = Resources.image_restart;
			varNewDocReset.Text = "Reiniciar Status Fiscal";
			varNewDocReset.Click += pActFiscReset.Invoke;
			pclsMenu.Items.Add(varNewDocReset);
		}
		if (!pHasColors.ContainsKey(""))
		{
			pHasColors.Add("", Color.White.Name);
		}
		return true;
	}

	public static async Task<clsComexProfile> funcGetCertComexAsync(Form pForm, clsComexProfile pclsProfile, FilialView pclsFilial)
	{
		if (pclsProfile == null)
		{
			pclsProfile = new clsComexProfile();
		}
		bool varMustGetCert = false;
		string varLastSerial = pclsProfile?.CrtSerial;
		if (clsFunction.IsEmpty(pclsProfile.CrtSerial))
		{
			varMustGetCert = true;
		}
		else if (!clsFunction.IsEmpty(pclsFilial?.ComexCertSerial) && !clsFunction.IsEqual(pclsFilial?.ComexCertSerial, varLastSerial))
		{
			varMustGetCert = true;
		}
		if (!varMustGetCert)
		{
			return pclsProfile;
		}
		clsSingleCertificates varclsCertificates = clsSingleCertificates.Instance;
		clsDataParameter varclsDataParam = new clsDataParameter();
		string varCertSerial = clsFunction.funcGetValue(pclsFilial?.ComexCertSerial);
		if (clsFunction.IsEmpty(varCertSerial))
		{
			varCertSerial = await varclsDataParam.funcGetAsync("Export-Digital-Certificate", pBuffer: true, pGlobal: true);
		}
		if (clsFunction.IsEqual(varCertSerial, pclsProfile.CrtSerial) && !clsFunction.IsEmpty(varCertSerial))
		{
			return pclsProfile;
		}
		clsComexProfile clsComexProfile = pclsProfile;
		clsComexProfile.CertFull = await varclsCertificates.funcGetCertDataAsync(varCertSerial);
		clsComexProfile = pclsProfile;
		clsComexProfile.Profile = await varclsDataParam.funcGetAsync("Export-Siscomex-Profile", pBuffer: true, pGlobal: true);
		if (pclsProfile.CertFull == null)
		{
			frmCertComex varfrmCertific = new frmCertComex();
			varfrmCertific.ShowDialog(pForm);
			varCertSerial = varfrmCertific.funcGetCertificate();
			varfrmCertific.Dispose();
			clsComexProfile = pclsProfile;
			clsComexProfile.CertFull = await varclsCertificates.funcGetCertDataAsync(varCertSerial);
			pclsProfile.Profile = varfrmCertific.funcGetUserProfile();
		}
		pclsProfile.CrtSerial = pclsProfile?.CertFull?.SerialNumber;
		return pclsProfile;
	}

	public static async Task<FilialView> funcSetCertificateAsync(Form pWindowsForm, FilialView pFilial, bool pInTask = false)
	{
		FilialView varclsClone = pFilial.GetClone();
		if (pFilial == null)
		{
			return null;
		}
		bool varShowForm = pWindowsForm.Visible && !pInTask;
		FilialView result;
		try
		{
			clsSingleCertificates varclsCertificates = clsSingleCertificates.Instance;
			X509Certificate2 varCertifInfo = await varclsCertificates.funcGetCertificateAsync(pFilial.Certificado, pFilial.CNPJ, pPrivateKey: true, pCheckPin: true);
			if (varCertifInfo == null)
			{
				pFilial.Certificado = string.Empty;
			}
			else if (!clsFunction.IsEmpty(pFilial.Certificado))
			{
				clsSrvCertFull varCertContent = await varclsCertificates.funcGetCertDataAsync(varCertifInfo.SerialNumber);
				if (varCertContent == null)
				{
					pFilial.Certificado = string.Empty;
				}
				else
				{
					string varErrorMessage = (await varCertContent.funcCheckSignAsync()).funcGetAllMessageResult();
					if (!varShowForm || !clsFunction.funcIsCertError(varErrorMessage))
					{
						if (clsFunction.funcIsCertCancel(varErrorMessage))
						{
							pFilial.Certificado = string.Empty;
							result = pFilial;
						}
						else
						{
							pFilial.Certificado = varCertifInfo.SerialNumber;
							result = pFilial;
						}
						goto IL_03e8;
					}
					frmCertAdvice obj = new frmCertAdvice();
					obj.ShowDialog(pWindowsForm);
					obj.Dispose();
					pFilial.Certificado = string.Empty;
				}
			}
			if (!varShowForm)
			{
				result = pFilial;
				goto IL_03e8;
			}
			frmCertFilial varfrmCertific = new frmCertFilial(pFilial);
			varfrmCertific.ShowDialog(pWindowsForm);
			pFilial.Certificado = varfrmCertific.funcGetCertificate();
			varfrmCertific.Dispose();
		}
		finally
		{
			if (!clsFunction.IsEmpty(pFilial.Certificado) && clsFunction.IsEmpty(varclsClone.Certificado))
			{
				await new clsDataFilial().funcUpdateAsync(pFilial, pLogUserFields: true);
			}
		}
		return pFilial;
		IL_03e8:
		return result;
	}
}
