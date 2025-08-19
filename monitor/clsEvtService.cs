using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using screen.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsEvtService
{
	private clsDFeCodes varclsDFeCodes = new clsDFeCodes();

	private clsDataConfig varclsDataConfig = new clsDataConfig();

	private clsDataEvent varclsDataEvent = new clsDataEvent();

	public async Task<clsReturn> funcCheckAuthorizationAsync(clsEventData pclsEvtData)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		if (pclsEvtData.TypeList.ContainsValue("DOWNLOAD"))
		{
			varclsReturnFunc.ActionDone = true;
			return varclsReturnFunc;
		}
		foreach (KeyValuePair<string, string> type in pclsEvtData.TypeList)
		{
			string varEvtType = type.Value;
			if (clsFunction.IsEqual(varEvtType, "DOWN-MANFST"))
			{
				varEvtType = varclsDFeCodes.GetEvtDownloadXmlWebs();
			}
			if (!(await clsScreenGeral.funcHasAccessAsync("MANIFEST-MANAGER", "EXECUTE", varEvtType)))
			{
				return varclsReturnFunc;
			}
		}
		varclsReturnFunc.ActionDone = true;
		return varclsReturnFunc;
	}

	public async Task<clsReturn> funcExecuteAsync(Form pForm, clsEventData pclsEvtData)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		pclsEvtData = funcGetFilialForEvent(pclsEvtData);
		if (pclsEvtData.Cancel)
		{
			return varclsReturnFunc;
		}
		if (pclsEvtData.IsEvtDownload)
		{
			pclsEvtData = funcGetDownEvtType(pForm, pclsEvtData, "NFe");
			pclsEvtData = funcGetDownEvtType(pForm, pclsEvtData, "NFCe");
			pclsEvtData = funcGetDownEvtType(pForm, pclsEvtData, "CTe");
			pclsEvtData = funcGetDownEvtType(pForm, pclsEvtData, "CTeOs");
			pclsEvtData = funcGetDownEvtType(pForm, pclsEvtData, "NFSe");
		}
		if (pclsEvtData.Cancel)
		{
			return varclsReturnFunc;
		}
		varclsReturnFunc = await funcCheckAuthorizationAsync(pclsEvtData);
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		if (!varclsReturnFunc.ActionDone)
		{
			return varclsReturnFunc;
		}
		List<string> varCNPJList = new List<string>();
		foreach (Document varclsDoc in pclsEvtData.DocList)
		{
			FilialView varclsFilial = await clsSrvGeral.funcGetFilialAsync(varclsDoc.Filial);
			if (!pclsEvtData.FilialList.Any((FilialView r) => r.CNPJ.Equals(varclsFilial.CNPJ)))
			{
				pclsEvtData.FilialList.Add(varclsFilial);
			}
			string varDocType = clsFunction.funcGetDocType(varclsDoc.Model, pCTeOs: false);
			pclsEvtData.TypeList.TryGetValue(varDocType, out var varEventType);
			if (!clsFunction.IsEmpty(varEventType))
			{
				List<Event> varEvtList = await funcNewEventAsync(varEventType, varclsFilial, varclsDoc, pclsEvtData.Agent);
				pclsEvtData.EvtList.AddRange(varEvtList);
				varCNPJList.Add(varclsDoc.EmitID);
			}
		}
		if (pclsEvtData.CheckPartner && !(await funcCheckPartnerListAsync(varCNPJList)))
		{
			pclsEvtData.Cancel = true;
			return varclsReturnFunc;
		}
		frmEventConfirm obj = new frmEventConfirm(pclsEvtData);
		obj.ShowDialog(pForm);
		obj.Dispose();
		return varclsReturnFunc;
	}

	public bool IsToDown(Document pclsDoc)
	{
		if (clsFunction.IsEmpty(pclsDoc.HasXml))
		{
			return true;
		}
		if (!clsFunction.IsEmpty(pclsDoc.XmlError) && clsFunction.IsEmpty(pclsDoc.FisIoApi))
		{
			return true;
		}
		if (clsFunction.IsAdmin)
		{
			return true;
		}
		return false;
	}

	public string funcGetDownOption(Form pForm, string pDocType, bool pHasDocSelected)
	{
		string varclsReturnFunc = string.Empty;
		if (pDocType.Equals("NFe"))
		{
			frmDownOptionNFe obj = new frmDownOptionNFe(pHasDocSelected);
			obj.ShowDialog(pForm);
			varclsReturnFunc = obj.funcGetDownOption();
			obj.Dispose();
		}
		else if (pDocType.Equals("CTe"))
		{
			frmDownOptionCTe obj2 = new frmDownOptionCTe(pHasDocSelected);
			obj2.ShowDialog(pForm);
			varclsReturnFunc = obj2.funcGetDownOption();
			obj2.Dispose();
		}
		return varclsReturnFunc;
	}

	private async Task<Event> funcGetEventAsync(FilialView pclsFilial, Document pclsDoc, string pTpEvento, string pAgent)
	{
		clsEventTemplate varclsTemplate = new clsEventTemplate();
		Event varclsEvent = null;
		if (clsFunction.IsEqual(pTpEvento, "949494", "848484", "747474", "757575"))
		{
			return await varclsTemplate.GetAsync(pclsFilial, pclsDoc.Chave, pTpEvento, pAgent);
		}
		if (varclsEvent != null)
		{
			return varclsEvent;
		}
		await varclsDataConfig.funcGetItemByKeyAsync();
		varclsEvent = await varclsDataEvent.funcGetItemByChaveTpEventoAsync(pclsDoc.Chave, pTpEvento);
		if (varclsEvent == null || !clsFunction.IsEmpty(varclsEvent.Protc))
		{
			varclsEvent = await varclsTemplate.GetAsync(pclsFilial, pclsDoc.Chave, pTpEvento, pAgent);
		}
		string varEvtMessage = string.Empty;
		if (clsFunction.IsEqual(pclsDoc.Canceled, "X", "C"))
		{
			varEvtMessage = "Documento já se encontra cancelado na SEFAZ";
		}
		else if (clsFunction.IsEqual(pclsDoc.Canceled, "D"))
		{
			varEvtMessage = "Documento já se encontra denegado na SEFAZ";
		}
		else if (clsFunction.IsEqual(pclsDoc.Canceled, "I"))
		{
			varEvtMessage = "Documento já se encontra inutilizado na SEFAZ";
		}
		else if (clsFunction.IsEqual(pclsDoc.Canceled, "R"))
		{
			varEvtMessage = "Documento já se encontra rejeitado na SEFAZ";
		}
		if (!clsFunction.IsEmpty(varEvtMessage))
		{
			varclsEvent.xMotivo = varEvtMessage;
		}
		return varclsEvent;
	}

	public static async Task<bool> funcCheckPartnerListAsync(List<string> pPartnerList)
	{
		pPartnerList = pPartnerList.Distinct().ToList();
		if (pPartnerList.Count <= 1)
		{
			return true;
		}
		clsFeatureService clsFeatureService = new clsFeatureService();
		string varFeatExtId = clsFeatureService.consEventInBatch;
		clsFeatStatus varFeatStatus = await clsFeatureService.funcGetFeatStatusAsync(varFeatExtId);
		await new clsDataParameter().funcAddCounterAsync(varFeatExtId + "-CLICKS");
		frmHelpEventInBatch varfrmHelpEventInBatch = new frmHelpEventInBatch();
		if (varFeatStatus == null)
		{
			varfrmHelpEventInBatch.ShowDialog();
			varfrmHelpEventInBatch.Dispose();
			return false;
		}
		if (varFeatStatus.ftFeatType.Contains("LOCK"))
		{
			varfrmHelpEventInBatch.ShowDialog();
			varfrmHelpEventInBatch.Dispose();
			return false;
		}
		return true;
	}

	public async Task<List<Event>> funcNewEventAsync(string pEvtType, FilialView pclsFilial, Document pclsDoc, string pAgent)
	{
		List<Event> varEvtList = new List<Event>();
		clsFunction.funcGetDocType(pclsDoc.Model, pCTeOs: false);
		if (clsFunction.IsEqual(pEvtType, "DOWN-MANFST"))
		{
			string varTpEvento01 = varclsDFeCodes.GetNFeAcknow();
			varEvtList.Add(await funcGetEventAsync(pclsFilial, pclsDoc, varTpEvento01, pAgent));
			string varTpEvento2 = varclsDFeCodes.GetEvtDownloadXmlWebs();
			varEvtList.Add(await funcGetEventAsync(pclsFilial, pclsDoc, varTpEvento2, pAgent));
		}
		else
		{
			varEvtList.Add(await funcGetEventAsync(pclsFilial, pclsDoc, pEvtType, pAgent));
		}
		return varEvtList;
	}

	public clsEventData funcGetDownEvtType(Form pForm, clsEventData pclsEvtData, string pDocType)
	{
		if (!pclsEvtData.IsEvtDownload)
		{
			return pclsEvtData;
		}
		string varDownloadXmlFull = varclsDFeCodes.GetEvtDownloadXmlFull();
		string varDownloadXmlWebs = varclsDFeCodes.GetEvtDownloadXmlWebs();
		pclsEvtData.TypeList.TryGetValue(pDocType, out var varEvtType);
		varEvtType = clsFunction.funcGetValue(varEvtType);
		if (clsFunction.IsEmpty(varEvtType))
		{
			varEvtType = "DOWNLOAD";
		}
		if (!clsFunction.IsEqual(varEvtType, "DOWNLOAD"))
		{
			return pclsEvtData;
		}
		if (clsFunction.IsEqual(pDocType, "CTe", "CTeOs", "NFCe"))
		{
			pclsEvtData.TypeList.Remove(pDocType);
			pclsEvtData.TypeList.Add(pDocType, varDownloadXmlFull);
			return pclsEvtData;
		}
		if (clsFunction.IsEqual(pDocType, "NFSe", "CFeSat"))
		{
			pclsEvtData.TypeList.Remove(pDocType);
			pclsEvtData.TypeList.Add(pDocType, varDownloadXmlWebs);
			return pclsEvtData;
		}
		if (!pclsEvtData.DocList.Any((Document r) => clsFunction.funcIsNFe(r.Model)) && !pclsEvtData.ForceScreen)
		{
			return pclsEvtData;
		}
		string varDownOption = funcGetDownOption(pForm, pDocType, pclsEvtData.HasDocSelect);
		if (clsFunction.IsEmpty(varDownOption))
		{
			pclsEvtData.Cancel = true;
			return pclsEvtData;
		}
		pclsEvtData.TypeList.Remove(pDocType);
		pclsEvtData.TypeList.Add(pDocType, varDownOption);
		pclsEvtData.EvtUserType = varDownOption;
		return pclsEvtData;
	}

	public clsEventData funcGetFilialForEvent(clsEventData pclsEvtData, List<clsObjecSearch> pDocList = null)
	{
		if (pDocList == null)
		{
			pDocList = new List<clsObjecSearch>();
		}
		if (pclsEvtData.DocList.Count > 0)
		{
			pclsEvtData.FilialList.Clear();
			return pclsEvtData;
		}
		if (pclsEvtData.FilialList.Count == 1 && pDocList.Count <= 0)
		{
			return pclsEvtData;
		}
		frmFilialSingleSelect varFrmSelect = new frmFilialSingleSelect(pclsEvtData.FilialList);
		if (varFrmSelect.ShowDialog().Equals(DialogResult.Cancel))
		{
			pclsEvtData.Cancel = true;
			return pclsEvtData;
		}
		pclsEvtData.FilialList.Clear();
		pclsEvtData.FilialList.Add(varFrmSelect.funcGetFilial());
		foreach (clsObjecSearch item in pDocList.Where((clsObjecSearch r) => clsFunction.IsEmpty(r.Filial)))
		{
			item.Filial = varFrmSelect.funcGetFilial().CNPJ;
		}
		return pclsEvtData;
	}
}
