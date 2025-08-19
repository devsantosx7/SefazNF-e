using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using data.fiscal.io;
using manager.fiscal.io;
using srv.fiscal.io;
using util.fiscal.io;

namespace Monitor;

public class clsStartupService
{
	private bool _ShowStatus;

	public event EventStartupSrvHandler EventStartupSrv;

	public clsStartupService(bool pShowStatus)
	{
		_ShowStatus = pShowStatus;
	}

	protected virtual void OnEventStartupSrv(string pRetMessage, clsReturn pReturnFunc)
	{
		if (_ShowStatus)
		{
			EventStartupSrvEventArgs varEvtArgs = new EventStartupSrvEventArgs
			{
				RetMessage = pRetMessage,
				ReturnFunc = pReturnFunc,
				ShowStatus = _ShowStatus
			};
			this.EventStartupSrv?.Invoke(this, varEvtArgs);
		}
	}

	private void funcEventDbaManager(object sender, EventDataBaseEventArgs e)
	{
		if (e != null && !clsFunction.IsEmpty(e.RetMessage))
		{
			double varPercent = Convert.ToDouble(e.TaskPoint) / Convert.ToDouble(e.TaskCount);
			string varMessage = $"{e.RetMessage} [ {varPercent:P} ]";
			OnEventStartupSrv(varMessage, null);
		}
	}

	public async Task<clsReturn> funcOpenDbaAsync()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		clsDataWebService varclsDataWebService = new clsDataWebService();
		clsDataRegionDest varclsDataRegionDest = new clsDataRegionDest();
		clsTableMigration varclsTableMigration = new clsTableMigration();
		clsDbaFactory varclsDbaFactory = new clsDbaFactory();
		clsFileManager varclsFileManager = new clsFileManager();
		clsDataParameter varclsDataParam = new clsDataParameter();
		clsDataConfig varclsDataConfig = new clsDataConfig();
		bool varIsNewDatabase = false;
		Configuration varclsConfigModel = null;
		OnEventStartupSrv("Definindo gerenciador de banco de dados ...", null);
		varclsDbaFactory.funcGetDbaCStr();
		intDatabase varclsDataBase = null;
		try
		{
			varclsDataBase = await varclsDbaFactory.funcGetClassAsync();
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		varclsDataBase.EventDbaManager += funcEventDbaManager;
		bool varExitLoop = false;
		int varCounter = 0;
		string varDbaType = varclsDataBase.funcGetDbaType();
		string varDbaName = varclsDataBase.funcGetDbaName();
		varclsDbaFactory.funcGetDbaGuid();
		if (!varclsDataBase.funcIsLocalDba())
		{
			varExitLoop = true;
		}
		else if (await varclsDataBase.funcIsDbaExistAsync())
		{
			varExitLoop = true;
		}
		while (!varExitLoop)
		{
			varCounter++;
			if (varCounter > 2)
			{
				break;
			}
			OnEventStartupSrv("Criando banco de dados ...", null);
			if (varclsReturnFunc.HasError && clsFunction.IsEqual(varDbaType, "SQLSRVLOC"))
			{
				varclsDbaFactory.funcSetSqlLiteDbaType();
				varDbaType = "SQLLITE";
				_ = string.Empty;
				new clsDbaConStr().funcResetDbaConfigBuffer();
				varclsDbaFactory.funcClearBuffer();
			}
			varDbaName = varclsDataBase.funcGetDbaName();
			string varDbaGuid = varclsDbaFactory.funcGetDbaGuid();
			string varStrConnect = clsDataGeral.funcGetDbaStrCon(varDbaType, "");
			varclsDataBase = await varclsDbaFactory.funcGetClassAsync(varDbaType, varStrConnect, varDbaName, varDbaGuid);
			clsReturn varRetDbaCreate = await varclsDataBase.funcCreateDataBaseAsync();
			varIsNewDatabase = true;
			if (varRetDbaCreate.HasError)
			{
				varclsReturnFunc.AddRange(varRetDbaCreate);
				if (!clsFunction.IsEqual(varDbaType, "SQLSRVLOC"))
				{
					break;
				}
				continue;
			}
			varclsReturnFunc.Clear();
			break;
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			OnEventStartupSrv("Conectando no banco de dados ...", null);
			varclsDataBase = await varclsDbaFactory.funcGetClassAsync();
			varDbaType = varclsDataBase.funcGetDbaType();
			varDbaName = varclsDataBase.funcGetDbaName();
			string varDbaGuid = varclsDbaFactory.funcGetDbaGuid();
			string varFileName = string.Empty;
			if (clsFunction.IsEqual(varDbaType, "SQLLITE", pIgnoreCase: true))
			{
				varFileName = varclsFileManager.funcGetSqlLiteDbaName();
			}
			else if (clsFunction.IsEqual(varDbaType, "SQLSRVLOC", pIgnoreCase: true))
			{
				varFileName = varclsFileManager.funcGetSqlSrvLocDbaName();
			}
			bool varIsMonitorOpenedByOtherUser = false;
			varExitLoop = false;
			varCounter = 0;
			while (!clsFunction.IsEmpty(varFileName) && !varExitLoop)
			{
				varCounter++;
				varIsMonitorOpenedByOtherUser = clsFunction.funcIsMonitorOpenedByOtherUser();
				if (!varIsMonitorOpenedByOtherUser)
				{
					break;
				}
				if (varIsMonitorOpenedByOtherUser && Program.LaunchedViaStartup)
				{
					Application.Exit();
					return varclsReturnFunc;
				}
				if (varIsMonitorOpenedByOtherUser)
				{
					clsFunction.funcKillMonitorOpenedByOtherUser();
				}
				if (varCounter > 3)
				{
					break;
				}
			}
			if (varIsMonitorOpenedByOtherUser)
			{
				string varUserMessage = clsFunction.funcAlertFileInUseFull(varFileName);
				varclsReturnFunc.AddMessage(new clsMessage("E", "9999", varUserMessage, null, "DBA-ALREADY-IN-USE"));
			}
			if (varclsReturnFunc.HasError)
			{
				return varclsReturnFunc;
			}
			clsReturn varResultOpen = await varclsDataBase.funcOpenAsync();
			if (varResultOpen.HasError)
			{
				varclsReturnFunc.AddRange(varResultOpen);
			}
			bool varTryConnection = false;
			if (clsFunction.funcIsDbaLoginFailed(varResultOpen) && clsFunction.IsEqual(varDbaType, "SQLSRVLOC"))
			{
				string varStrConnect2 = clsDataGeral.funcGetDbaStrCon(varDbaType, "");
				varclsDataBase = await varclsDbaFactory.funcGetClassAsync(varDbaType, varStrConnect2, varDbaName, varDbaGuid);
				clsReturn varRetUserCreate = await varclsDataBase.funcCreateAdminUserAsync();
				varTryConnection = true;
				if (varRetUserCreate.HasError)
				{
					varclsReturnFunc.AddRange(varRetUserCreate);
				}
			}
			if (varTryConnection)
			{
				varclsDataBase = await varclsDbaFactory.funcGetClassAsync();
				varResultOpen = await varclsDataBase.funcOpenAsync();
				if (varResultOpen.HasError)
				{
					varclsReturnFunc.AddRange(varResultOpen);
				}
				else
				{
					varclsReturnFunc.Clear();
				}
			}
		}
		catch (Exception pException2)
		{
			varclsReturnFunc.AddException(pException2);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		if (!varIsNewDatabase && clsFunction.IsEmpty(await varclsDataParam.funcGetInitAsync("SQLSRVLOC-ADMIN-USER")) && clsFunction.IsEqual(varDbaType, "SQLSRVLOC"))
		{
			clsReturn varRetUserCreate2 = await varclsDataBase.funcCreateAdminUserAsync();
			if (!varRetUserCreate2.HasError)
			{
				await varclsDataParam.funcSetAsync("SQLSRVLOC-ADMIN-USER", "DONE");
			}
			else
			{
				varclsReturnFunc.AddRange(varRetUserCreate2);
			}
		}
		OnEventStartupSrv("Sincronizando conexão de banco de dados ...", null);
		if (varclsConfigModel != null)
		{
			varclsConfigModel.DbaType = varDbaType;
			varclsConfigModel.DbaName = varDbaName;
			await new clsDataConfig().funcSyncDbaDataAsync(varclsConfigModel, pSave: true);
		}
		OnEventStartupSrv("Configurando dicionário de dados ...", null);
		varclsDataBase.EventDbaManager += funcEventDbaManager;
		bool varHardInit = varIsNewDatabase;
		bool varSetFixTableConfiguratAsDone = false;
		bool varSetFixTableProductAsDone = false;
		bool varSetFixTableFeatureAsDone = false;
		bool varSetFixTableBatchItemAsDone = false;
		bool varSetFixTableInstallMeasureAsDone = false;
		bool varSetFixTableMeaDownItemAsDone = false;
		bool varSetFixTableMeasurePriceAsDone = false;
		bool varSetFixTableObjectMeasureAsDone = false;
		bool varSetFixTableAlert01AsDone = false;
		bool varSetFixTableAlert02AsDone = false;
		varclsDataConfig.funcResetBuffer();
		varclsTableMigration.MigrationStatusProcess += funcMigrationStatusProcess;
		List<Configuration> varConfigList = new List<Configuration>();
		try
		{
			string varParamTaskConfigurat = "DONE";
			if (!varIsNewDatabase)
			{
				varParamTaskConfigurat = await varclsDataParam.funcGetInitAsync("FixTable-Configuration", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParamTaskConfigurat))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<Configuration>())
				{
					try
					{
						varConfigList = await varclsDataConfig.funcGetListAsync();
						clsReturn varclsRetItem01 = clsSrvGeral.funcSaveTempConfigBuffer(varConfigList);
						if (varclsRetItem01.HasError)
						{
							varclsReturnFunc.AddRange(varclsRetItem01);
						}
					}
					catch (Exception pException3)
					{
						varclsReturnFunc.AddException(pException3);
					}
					clsReturn varclsRetItem2 = await varclsTableMigration.funcDropTableAsync<Configuration>();
					if (varclsRetItem2.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem2);
					}
					else
					{
						varHardInit = true;
						varSetFixTableConfiguratAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableConfiguratAsDone = true;
				}
			}
		}
		catch (Exception pException4)
		{
			varclsReturnFunc.AddException(pException4);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParamProduct = "DONE";
			if (!varIsNewDatabase)
			{
				varParamProduct = await varclsDataParam.funcGetInitAsync("FixTable-Product-01", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParamProduct))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<Product>())
				{
					clsReturn varclsRetItem3 = await varclsTableMigration.funcDropTableAsync<Product>();
					if (varclsRetItem3.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem3);
					}
					else
					{
						varHardInit = true;
						varSetFixTableProductAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableProductAsDone = true;
				}
			}
		}
		catch (Exception pException5)
		{
			varclsReturnFunc.AddException(pException5);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParamFeature = "DONE";
			if (!varIsNewDatabase)
			{
				varParamFeature = await varclsDataParam.funcGetInitAsync("FixTable-Feature-01", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParamFeature))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<Feature>())
				{
					clsReturn varclsRetItem4 = await varclsTableMigration.funcDropTableAsync<Feature>();
					if (varclsRetItem4.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem4);
					}
					else
					{
						varHardInit = true;
						varSetFixTableFeatureAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableFeatureAsDone = true;
				}
			}
		}
		catch (Exception pException6)
		{
			varclsReturnFunc.AddException(pException6);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParBatchItem = "DONE";
			if (!varIsNewDatabase)
			{
				varParBatchItem = await varclsDataParam.funcGetInitAsync("FixTable-BatchItem", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParBatchItem))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<BatchItem>())
				{
					clsReturn varclsRetItem5 = await varclsTableMigration.funcDropTableAsync<BatchItem>();
					if (varclsRetItem5.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem5);
					}
					else
					{
						varHardInit = true;
						varSetFixTableBatchItemAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableBatchItemAsDone = true;
				}
			}
		}
		catch (Exception pException7)
		{
			varclsReturnFunc.AddException(pException7);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParAlertTable01 = "DONE";
			if (!varIsNewDatabase)
			{
				varParAlertTable01 = await varclsDataParam.funcGetInitAsync("FixTable-Alert", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParAlertTable01))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<Alert>())
				{
					clsReturn varclsRetItem6 = await varclsTableMigration.funcDropTableAsync<Alert>();
					if (varclsRetItem6.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem6);
					}
					else
					{
						varHardInit = true;
						varSetFixTableAlert01AsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableAlert01AsDone = true;
				}
			}
		}
		catch (Exception pException8)
		{
			varclsReturnFunc.AddException(pException8);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParAlertTable2 = "DONE";
			if (!varIsNewDatabase)
			{
				varParAlertTable2 = await varclsDataParam.funcGetInitAsync("FixTable-Alert-02", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParAlertTable2))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<Alert>())
				{
					clsReturn varclsRetItem7 = await varclsTableMigration.funcDropTableAsync<Alert>();
					if (varclsRetItem7.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem7);
					}
					else
					{
						varHardInit = true;
						varSetFixTableAlert02AsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableAlert02AsDone = true;
				}
			}
		}
		catch (Exception pException9)
		{
			varclsReturnFunc.AddException(pException9);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParInstallMeasureTable = "DONE";
			if (!varIsNewDatabase)
			{
				varParInstallMeasureTable = await varclsDataParam.funcGetInitAsync("FixTable-InstallMeasure", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParInstallMeasureTable))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<InstallMeasure>())
				{
					clsReturn varclsRetItem8 = await varclsTableMigration.funcDropTableAsync<InstallMeasure>();
					if (varclsRetItem8.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem8);
					}
					else
					{
						varHardInit = true;
						varSetFixTableInstallMeasureAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableInstallMeasureAsDone = true;
				}
			}
		}
		catch (Exception pException10)
		{
			varclsReturnFunc.AddException(pException10);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParMeaDownItemTable = "DONE";
			if (!varIsNewDatabase)
			{
				varParMeaDownItemTable = await varclsDataParam.funcGetInitAsync("FixTable-MeaDownItem", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParMeaDownItemTable))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<MeaDownItem>())
				{
					clsReturn varclsRetItem9 = await varclsTableMigration.funcDropTableAsync<MeaDownItem>();
					if (varclsRetItem9.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem9);
					}
					else
					{
						varHardInit = true;
						varSetFixTableMeaDownItemAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableMeaDownItemAsDone = true;
				}
			}
		}
		catch (Exception pException11)
		{
			varclsReturnFunc.AddException(pException11);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParMeasurePriceTable = "DONE";
			if (!varIsNewDatabase)
			{
				varParMeasurePriceTable = await varclsDataParam.funcGetInitAsync("FixTable-MeasurePrice", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParMeasurePriceTable))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<MeasurePrice>())
				{
					clsReturn varclsRetItem10 = await varclsTableMigration.funcDropTableAsync<MeasurePrice>();
					if (varclsRetItem10.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem10);
					}
					else
					{
						varHardInit = true;
						varSetFixTableMeasurePriceAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableMeasurePriceAsDone = true;
				}
			}
		}
		catch (Exception pException12)
		{
			varclsReturnFunc.AddException(pException12);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			string varParObjectMeasureTable = "DONE";
			if (!varIsNewDatabase)
			{
				varParObjectMeasureTable = await varclsDataParam.funcGetInitAsync("FixTable-ObjectMeasure", pBuffer: true, pGlobal: true);
			}
			if (clsFunction.IsEmpty(varParObjectMeasureTable))
			{
				if (await varclsTableMigration.funcIsTableExistAsync<ObjectMeasure>())
				{
					clsReturn varclsRetItem11 = await varclsTableMigration.funcDropTableAsync<ObjectMeasure>();
					if (varclsRetItem11.HasError)
					{
						varclsReturnFunc.AddRange(varclsRetItem11);
					}
					else
					{
						varHardInit = true;
						varSetFixTableObjectMeasureAsDone = true;
					}
				}
				else
				{
					varHardInit = true;
					varSetFixTableObjectMeasureAsDone = true;
				}
			}
		}
		catch (Exception pException13)
		{
			varclsReturnFunc.AddException(pException13);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			varclsReturnFunc.AddRange((await varclsDataBase.funcInitializeAsync(varHardInit)).Messages);
		}
		catch (Exception pException14)
		{
			varclsReturnFunc.AddException(pException14);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		await varclsDataParam.funcBufferDataAsync();
		if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("SQL-TRACE-ACTIVE")))
		{
			clsFunction.funcDisableSqlTracer();
		}
		else
		{
			clsFunction.funcEnableSqlTracer();
		}
		if (Program.MustRunDbaOptimize)
		{
			return varclsReturnFunc;
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetInitAsync("FixTable-Parameter-Old01", pBuffer: true, pGlobal: true)) && await varclsTableMigration.funcIsTableExistAsync<Parameter_Old01>() && await varclsTableMigration.funcIsTableExistAsync<Parameter>())
			{
				OnEventStartupSrv("Migrando tabela Parameter - Parametros...", null);
				clsReturn varclsRetItem12 = await varclsTableMigration.funcCopyTableAsync<Parameter_Old01, Parameter>(null);
				if (!varclsRetItem12.HasError)
				{
					await varclsDataParam.funcSetAsync("FixTable-Parameter-Old01", "DONE", pGlobal: true);
				}
				else
				{
					varclsReturnFunc.AddRange(varclsRetItem12);
				}
			}
		}
		catch (Exception pException15)
		{
			varclsReturnFunc.AddException(pException15);
		}
		if (varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("SQLSRVLOC-ADMIN-USER", "DONE");
		}
		if (varSetFixTableConfiguratAsDone || varIsNewDatabase)
		{
			if (varConfigList.Count <= 0)
			{
				varConfigList = clsSrvGeral.funcLoadTempConfigBuffer();
			}
			foreach (Configuration varConfig in varConfigList)
			{
				await varclsDataConfig.funcInsertAsync(varConfig);
			}
			await varclsDataParam.funcSetAsync("FixTable-Configuration", "DONE", pGlobal: true);
		}
		if (varSetFixTableProductAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-Product-01", "DONE", pGlobal: true);
		}
		if (varSetFixTableFeatureAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-Feature-01", "DONE", pGlobal: true);
		}
		if (varSetFixTableBatchItemAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-BatchItem", "DONE", pGlobal: true);
		}
		if (varSetFixTableAlert01AsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-Alert", "DONE", pGlobal: true);
		}
		if (varSetFixTableAlert02AsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-Alert-02", "DONE", pGlobal: true);
		}
		if (varSetFixTableInstallMeasureAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-InstallMeasure", "DONE", pGlobal: true);
		}
		if (varSetFixTableMeaDownItemAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-MeaDownItem", "DONE", pGlobal: true);
		}
		if (varSetFixTableMeasurePriceAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-MeasurePrice", "DONE", pGlobal: true);
		}
		if (varSetFixTableObjectMeasureAsDone || varIsNewDatabase)
		{
			await varclsDataParam.funcSetAsync("FixTable-ObjectMeasure", "DONE", pGlobal: true);
		}
		if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FixConfig-SetConfigUseApi01", pBuffer: true, pGlobal: true)))
		{
			string varDFeScanChannel = "AUTO";
			await varclsDataParam.funcGetAsync("UseApiToScanDFeIn", pBuffer: true, pGlobal: true);
			if (!clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("NotUseApiToScanDFeIn", pBuffer: true, pGlobal: true)))
			{
				varDFeScanChannel = "LOCAL";
			}
			await varclsDataParam.funcSetAsync("DFeScanChannel", varDFeScanChannel, pGlobal: true);
			await varclsDataParam.funcSetAsync("FixConfig-SetConfigUseApi01", "X", pGlobal: true);
		}
		WebService varclsWebService01 = await varclsDataWebService.funcGetItemByKeyAsync("NfeConsultaProtocolo-NFe-4.00", "1", "MS");
		if (varclsWebService01 == null)
		{
			varclsWebService01 = new WebService();
		}
		if (clsFunction.IsEqual(varclsWebService01.WBSADR, "https://nfe.fazenda.ms.gov.br/ws/NFeConsultaProtocolo4"))
		{
			varclsWebService01.WBSADR = "https://nfe.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
			await varclsDataWebService.funcUpdateAsync(varclsWebService01);
		}
		WebService varclsWebService2 = await varclsDataWebService.funcGetItemByKeyAsync("NfeConsultaProtocolo-NFCe-4.00", "1", "MS");
		if (varclsWebService2 == null)
		{
			varclsWebService2 = new WebService();
		}
		if (clsFunction.IsEqual(varclsWebService2.WBSADR, "https://nfce.fazenda.ms.gov.br/ws/NFeConsultaProtocolo4"))
		{
			varclsWebService2.WBSADR = "https://nfce.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
			await varclsDataWebService.funcUpdateAsync(varclsWebService2);
		}
		WebService varclsWebService3 = await varclsDataWebService.funcGetItemByKeyAsync("NfeConsultaProtocolo-NFe-4.00", "1", "BA");
		if (varclsWebService3 == null)
		{
			varclsWebService3 = new WebService();
		}
		if (clsFunction.IsEqual(varclsWebService3.WBSADR, "https://nfe.sefaz.ba.gov.br/webservices/NFeStatusServico4/NFeStatusServico4.asmx"))
		{
			varclsWebService3.WBSADR = "https://nfe.sefaz.ba.gov.br/webservices/NFeConsultaProtocolo4/NFeConsultaProtocolo4.asmx";
			await varclsDataWebService.funcUpdateAsync(varclsWebService3);
		}
		WebService varclsWebService4 = await varclsDataWebService.funcGetItemByKeyAsync("NFeDistribuicaoDFe", "2", "AN");
		if (varclsWebService4 == null)
		{
			varclsWebService4 = new WebService();
		}
		if (clsFunction.IsEqual(varclsWebService4.WBSADR, "https://hom.nfe.fazenda.gov.br/NFeDistribuicaoDFe/NFeDistribuicaoDFe.asmx"))
		{
			varclsWebService4.WBSADR = "https://hom1.nfe.fazenda.gov.br/NFeDistribuicaoDFe/NFeDistribuicaoDFe.asmx";
			await varclsDataWebService.funcUpdateAsync(varclsWebService4);
		}
		WebService varclsWebService5 = await varclsDataWebService.funcGetItemByKeyAsync("NfeRecepcaoEvento-NFe-4.00", "2", "AN");
		if (varclsWebService5 == null)
		{
			varclsWebService5 = new WebService();
		}
		if (clsFunction.IsEqual(varclsWebService5.WBSADR, "https://hom.nfe.fazenda.gov.br/NFeRecepcaoEvento4/NFeRecepcaoEvento4.asmx"))
		{
			varclsWebService5.WBSADR = "https://hom1.nfe.fazenda.gov.br/NFeRecepcaoEvento4/NFeRecepcaoEvento4.asmx";
			await varclsDataWebService.funcUpdateAsync(varclsWebService5);
		}
		RegionDest varclsRegionDest = await varclsDataRegionDest.funcGetItemByKeyAsync("PA", "NfeConsultaProtocolo");
		if (varclsRegionDest == null)
		{
			varclsRegionDest = new RegionDest();
		}
		if (clsFunction.IsEqual(varclsRegionDest.WBSAUT, "SVAN"))
		{
			varclsRegionDest.WBSAUT = "SVRS";
			await varclsDataRegionDest.funcUpdateAsync(varclsRegionDest);
		}
		try
		{
			string varDFeScanChannel = await varclsDataParam.funcGetAsync("InitialSelectiveSearch");
			string varParSelectSearchByGlobal = await varclsDataParam.funcGetAsync("InitialSelectiveSearch", pBuffer: true, pGlobal: true);
			if (!clsFunction.IsEmpty(varDFeScanChannel) && clsFunction.IsEmpty(varParSelectSearchByGlobal))
			{
				await varclsDataParam.funcSetAsync("InitialSelectiveSearch", "DONE", pGlobal: true);
			}
			else if (clsFunction.IsEmpty(varParSelectSearchByGlobal))
			{
				foreach (FilialView varFilialItem in await new clsDataFilial().funcGetListAsync(pLoadDummy: true))
				{
					string text = (varFilialItem.GetNFSeAndEvent = "X");
					string getNFeAndEvent = (varFilialItem.GetCTeAndEvent = text);
					varFilialItem.GetNFeAndEvent = getNFeAndEvent;
					await new clsDataFilial().funcUpdateAsync(varFilialItem, pLogUserFields: false);
				}
				await varclsDataParam.funcSetAsync("InitialSelectiveSearch", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException16)
		{
			varclsReturnFunc.AddException(pException16);
		}
		if (varclsReturnFunc.HasError)
		{
			return varclsReturnFunc;
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("RefactoryChannelClientServer", pBuffer: true, pGlobal: true)))
			{
				bool varHasServer = await new clsWorkProcService().funcHasFiscalServerAsync();
				foreach (Channel varclsChannel in await new clsDataChannel().funcGetListAsync())
				{
					if (!varHasServer && clsFunction.IsEmpty(varclsChannel.Machine))
					{
						varclsChannel.Machine = Environment.MachineName;
						await new clsDataChannel().funcUpdateAsync(varclsChannel);
					}
				}
				await varclsDataParam.funcSetAsync("RefactoryChannelClientServer", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException17)
		{
			varclsReturnFunc.AddException(pException17);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("Fix-WorkProc-UserData-01")))
			{
				Configuration varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
				clsDataWorkProc varclsDataWorkProc = new clsDataWorkProc();
				foreach (WorkProc varclsWorkProc in await varclsDataWorkProc.funcGetListByServerAsync("Server", Environment.MachineName))
				{
					if (clsFunction.IsEmpty(varclsWorkProc.SrvUser))
					{
						varclsWorkProc.SrvUser = varclsConfig.ID;
					}
					varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync(varclsWorkProc.SrvUser);
					if (varclsConfig == null)
					{
						varclsConfig = await varclsDataConfig.funcGetItemByKeyAsync();
					}
					varclsWorkProc.SrvUser = varclsConfig.ID;
					await varclsDataWorkProc.funcUpdateAsync(varclsWorkProc);
				}
				await varclsDataParam.funcSetAsync("Fix-WorkProc-UserData-01", "DONE");
			}
		}
		catch (Exception pException18)
		{
			varclsReturnFunc.AddException(pException18);
		}
		varclsDataConfig.funcResetBuffer();
		try
		{
			string varDFeScanChannel = await varclsDataParam.funcGetAsync("Start-Backup-DigitalOcean");
			string varParBackupAws = await varclsDataParam.funcGetAsync("Start-Backup-AwsService");
			string varParBackupWas = await varclsDataParam.funcGetAsync("Start-Backup-WasabisysService");
			if (clsFunction.IsEmpty(varDFeScanChannel) || clsFunction.IsEmpty(varParBackupAws) || clsFunction.IsEmpty(varParBackupWas))
			{
				Configuration varclsConfig2 = await varclsDataConfig.funcGetItemByKeyAsync();
				varclsConfig2.BackupType = "FULL";
				await varclsDataConfig.funcUpdateAsync(varclsConfig2);
				await varclsDataParam.funcSetAsync("Start-Backup-DigitalOcean", "DONE");
				await varclsDataParam.funcSetAsync("Start-Backup-AwsService", "DONE");
				await varclsDataParam.funcSetAsync("Start-Backup-WasabisysService", "DONE");
			}
		}
		catch (Exception pException19)
		{
			varclsReturnFunc.AddException(pException19);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("Startup-NpsSurvey-Sync")))
			{
				clsDataNpsSurvey varclsDataNpsSurvey = new clsDataNpsSurvey();
				foreach (NpsSurvey varNpsItem in await varclsDataNpsSurvey.funcGetListAsync())
				{
					varNpsItem.nsSync = "";
					await varclsDataNpsSurvey.funcUpdateAsync(varNpsItem);
				}
				await varclsDataParam.funcSetAsync("Startup-NpsSurvey-Sync", "DONE");
			}
		}
		catch (Exception pException20)
		{
			varclsReturnFunc.AddException(pException20);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-01", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject01Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-01", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException21)
		{
			varclsReturnFunc.AddException(pException21);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-02", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject02Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-02", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException22)
		{
			varclsReturnFunc.AddException(pException22);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-03", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject03Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-03", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException23)
		{
			varclsReturnFunc.AddException(pException23);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-04", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject04Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-04", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException24)
		{
			varclsReturnFunc.AddException(pException24);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-05", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject05Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-05", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException25)
		{
			varclsReturnFunc.AddException(pException25);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-06", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject06Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-06", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException26)
		{
			varclsReturnFunc.AddException(pException26);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-07", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject07Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-07", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException27)
		{
			varclsReturnFunc.AddException(pException27);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-AUTHOBJECT-08", pBuffer: true, pGlobal: true)))
			{
				await new clsDataAuthObject().funcFixAuthObject08Async();
				await varclsDataParam.funcSetAsync("FIX-AUTHOBJECT-08", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException28)
		{
			varclsReturnFunc.AddException(pException28);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-FILIAL-01", pBuffer: true, pGlobal: true)))
			{
				await new clsDataFilial().funcFixFilial01Async();
				await varclsDataParam.funcSetAsync("FIX-FILIAL-01", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException29)
		{
			varclsReturnFunc.AddException(pException29);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-FILIAL-02", pBuffer: true, pGlobal: true)) && await new clsDataFilial().funcFixFilial02Async())
			{
				await varclsDataParam.funcSetAsync("FIX-FILIAL-02", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException30)
		{
			varclsReturnFunc.AddException(pException30);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("USERDATA-ON-WIN-REGISTRY")))
			{
				Configuration varclsConfig3 = await varclsDataConfig.funcGetItemByKeyAsync();
				new clsWinUserService().funcSet(varclsConfig3);
				await varclsDataParam.funcSetAsync("USERDATA-ON-WIN-REGISTRY", "DONE");
			}
		}
		catch (Exception pException31)
		{
			varclsReturnFunc.AddException(pException31);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("SYNC-CERT-CERTCON", pBuffer: true, pGlobal: true)))
			{
				await new clsDataCertificate().funcFixSyncCertifcateCertContentAsync();
				await varclsDataParam.funcSetAsync("SYNC-CERT-CERTCON", "DONE", pGlobal: true);
			}
		}
		catch (Exception pException32)
		{
			varclsReturnFunc.AddException(pException32);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-TASKACTION-STATUS-QUERY-01", pBuffer: false, pGlobal: true)))
			{
				new clsDataTaskAction().funcDeleteByActionAsync("CONSULTA-PROTOCOLO").ContinueWith((Func<Task<int>, Task>)async delegate
				{
					await varclsDataParam.funcSetAsync("FIX-TASKACTION-STATUS-QUERY-01", "DONE", pGlobal: true);
				});
			}
		}
		catch (Exception pException33)
		{
			varclsReturnFunc.AddException(pException33);
		}
		try
		{
			if (clsFunction.IsEmpty(await varclsDataParam.funcGetAsync("FIX-EVENT-STATUS-QUERY-01", pBuffer: false, pGlobal: true)))
			{
				new clsDataEvent().funcDeleteByTpEventoAsync("949494").ContinueWith((Func<Task<int>, Task>)async delegate
				{
					await varclsDataParam.funcSetAsync("FIX-EVENT-STATUS-QUERY-01", "DONE", pGlobal: true);
				});
			}
		}
		catch (Exception pException34)
		{
			varclsReturnFunc.AddException(pException34);
		}
		return varclsReturnFunc;
	}

	private void funcMigrationStatusProcess(object sender, MigrationStatusProcessEventArgs e)
	{
		try
		{
			OnEventStartupSrv(e.Message, null);
		}
		catch (Exception)
		{
		}
	}

	public async Task<clsReturn> funcLoadCertificatesAsync(bool pBuffer, bool pShowStatus)
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			if (Program.NotLoadDataInStart)
			{
				return varclsReturnFunc;
			}
			if (pShowStatus)
			{
				OnEventStartupSrv("Carregando certificados digitais ...", null);
			}
			clsSingleCertificates varclsCertificates = clsSingleCertificates.Instance;
			if (pBuffer)
			{
				Task<clsReturn> varTaskCert = varclsCertificates.funcInitializeAsync(pBuffer);
				if (!(await varTaskCert.WaitAsync(TimeSpan.FromSeconds(20.0))))
				{
					varclsReturnFunc.AddMessage(new clsMessage("W", "9999", "Tempo de procura dos certificados superior a 20 segundos."));
				}
				else
				{
					varclsReturnFunc = varTaskCert.Result;
				}
			}
			else
			{
				varclsReturnFunc = await varclsCertificates.funcInitializeAsync(pBuffer);
			}
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddMessage(new clsMessage("E", "9999", "Erro na leitura dos certificados digitais.", pException));
		}
		return varclsReturnFunc;
	}

	public async Task<clsReturn> funcExecuteBackgroundTasks()
	{
		clsReturn varclsReturnFunc = new clsReturn();
		try
		{
			OnEventStartupSrv("Definindo parâmetros de otimização ...", null);
			string varCommandTimeOut = await new clsDataParameter().funcGetAsync("DbaCommandTimeOut", pBuffer: true, pGlobal: true);
			if (clsFunction.IsEmpty(varCommandTimeOut))
			{
				varCommandTimeOut = "86400";
			}
			clsFunction.funcSetDbaCommandTimeout(clsFunction.funcConvStrToInt(varCommandTimeOut));
			await clsSrvGeral.funcTryToSetParallelProcessAsync();
			OnEventStartupSrv("Configurando inicialização automática ...", null);
			if (clsFunction.IsEqual((await clsManGeral.funcSetAutoStartupAsync()).GetValue("NotAutoStartup"), "S") && Program.LaunchedViaStartup)
			{
				Application.Exit();
				Environment.Exit(0);
				return varclsReturnFunc;
			}
			clsNgenService.funcExecuteNgenProcess();
			OnEventStartupSrv("Atualizando variáveis de registro", null);
			WebBrowserHelper.FixBrowserVersion(Assembly.GetExecutingAssembly().Location);
			OnEventStartupSrv("Carregando plugins instalados ...", null);
			varclsReturnFunc.AddRange(await new clsPluginService().funcSyncAsync());
			OnEventStartupSrv("Configurando tarefas de pocessamento automático ...", null);
			new clsChannelManager().funcSyncInboundAsync();
			new clsTaskManager(null).funcCreateMainTasksAsync(pIsService: false);
			funcLoadCertificatesAsync(pBuffer: false, pShowStatus: false);
		}
		catch (Exception pException)
		{
			varclsReturnFunc.AddException(pException);
		}
		if (varclsReturnFunc.HasError)
		{
			OnEventStartupSrv(null, varclsReturnFunc);
		}
		return varclsReturnFunc;
	}
}
