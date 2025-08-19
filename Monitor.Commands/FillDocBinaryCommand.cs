using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using data.fiscal.io;
using screen.fiscal.io;
using util.fiscal.io;

namespace Monitor.Commands;

[CommandAttributes(CommandName = "filldocbinary")]
public class FillDocBinaryCommand : Command
{
	private clsTraceService Tracer;

	private clsDataFilial DataFilial = new clsDataFilial();

	private clsDataDocBinary DataDocBinary = new clsDataDocBinary();

	private clsDataEvent DataEvent = new clsDataEvent();

	private string Source = "filldocbinaryComand";

	public FillDocBinaryCommand(bool ableLog)
	{
		if (ableLog)
		{
			Tracer = new clsTraceService();
		}
	}

	public override async Task RunAsync()
	{
		clsReturn varclsReturn = new clsReturn();
		try
		{
			funcSetTaskStatus(new clsTaskStatus("Populando documentos selecionados"));
			if (base.varclsTabManager == null)
			{
				Tracer?.funcTrace(Source, "varclsTabManager nulo");
				return;
			}
			intDocTabData varTabData = base.varclsTabManager.Selected();
			if (varTabData == null)
			{
				Tracer?.funcTrace(Source, "varTabData nulo");
				return;
			}
			List<Document> varDocList = await funcGetDocListAsync(varTabData, pFocused: false, pChecked: true, pSyncFromDbaFirst: true);
			Tracer?.funcTrace(Source, $"Total de documentos selecionados:  {varDocList.Count}");
			Configuration config = (await new clsDataConfig().funcGetItemByKeyAsync()).GetClone();
			config.DbaFileStore = null;
			foreach (Document item in varDocList)
			{
				try
				{
					FilialView filial = await DataFilial.funcGetItemByKeyAsync(item.Filial);
					if (filial == null)
					{
						Tracer?.funcTrace(Source, "Filial nula");
						continue;
					}
					Tracer?.funcTrace(Source, "Filial retornada: " + filial.CNPJ);
					string file = await clsDataGeral.funcGetBinaryFileAsync(config, filial, item.Chave, item);
					Tracer?.funcTrace(Source, "Arquivo retornado " + file);
					foreach (Event docEvent in await DataEvent.funcGetListByChaveAsync(item.Chave))
					{
						string varEvtDocKey = item.Chave + "-" + docEvent.tpEvento + "-" + docEvent.nSeqEvento;
						string evtFile = await clsDataGeral.funcGetBinaryFileAsync(config, filial, varEvtDocKey, item);
						if (evtFile != null && !File.Exists(evtFile))
						{
							Tracer?.funcTrace(Source, "Arquivo de evento não existe: " + evtFile);
							continue;
						}
						clsReturn fileEvtSaveResult = await funcSaveDocOnDbAsync(evtFile);
						if (fileEvtSaveResult.HasError)
						{
							varclsReturn.AddRange(fileEvtSaveResult);
						}
					}
					if (file != null && !File.Exists(file))
					{
						Tracer?.funcTrace(Source, "Arquivo não existe: " + file);
						continue;
					}
					clsReturn fileSaveResult = await funcSaveDocOnDbAsync(file);
					if (fileSaveResult.HasError)
					{
						varclsReturn.AddRange(fileSaveResult);
					}
				}
				catch (Exception ex)
				{
					Exception ex2 = new Exception("Erro ao tentar inserir documento " + item.Chave, ex);
					Tracer?.funcTrace(Source, "Erro ao tentar inserir documento " + item.Chave + ": " + Environment.NewLine + ex.Message + ex.StackTrace);
					varclsReturn.AddException(ex2);
				}
			}
		}
		catch (Exception pException)
		{
			varclsReturn.AddException(pException);
		}
		funcSetTaskStatus(new clsTaskStatus(pShow: false));
		if (varclsReturn.HasError)
		{
			clsScreenGeral.funcShowUserMessage(null, varclsReturn);
		}
		else
		{
			MessageBox.Show(base.WindowOwner, "Documentos populados na DocBinary", "Operação Realizada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private async Task<clsReturn> funcSaveDocOnDbAsync(string file)
	{
		clsReturn varclsReturn = new clsReturn();
		try
		{
			XmlDocument xml = new XmlDocument();
			xml.Load(file);
			Tracer?.funcTrace(Source, "Arquivo carregado para xml");
			int result = await DataDocBinary.funcPutAsync(file, xml);
			Tracer?.funcTrace(Source, $"resultado da query na tabela docbinary: {result}");
		}
		catch (Exception pException)
		{
			varclsReturn.AddException(pException);
		}
		return varclsReturn;
	}

	private async Task<List<Document>> funcGetDocListAsync(intDocTabData pTabData, bool pFocused, bool pChecked, bool pSyncFromDbaFirst)
	{
		List<Document> varDocList = await pTabData.funcGetDocListAsync(pFocused: false, pChecked: true, pSyncFromDbaFirst);
		if (varDocList.Count == 0)
		{
			varDocList = await pTabData.funcGetDocListAsync(pFocused: false, pChecked: false, pSyncFromDbaFirst);
		}
		return varDocList;
	}
}
