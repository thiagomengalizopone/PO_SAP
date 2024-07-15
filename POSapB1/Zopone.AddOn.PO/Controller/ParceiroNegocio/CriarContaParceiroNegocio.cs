using sap.dev.core;
using sap.dev.data;
using SAPbobsCOM;
using System;
using Zopone.AddOn.PO.UtilAddOn;

namespace Zopone.AddOn.PO.Controller.ParceiroNegocio
{
    public class CriarContaParceiroNegocio
    {
        public static bool GerarContaContabilCentroCustoPN(string CardCode, out string CodigoConta)
        {
            try
            {
                string AcctCode = string.Empty;

                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                SAPbobsCOM.BusinessPartners oParceiroNegocio = (SAPbobsCOM.BusinessPartners)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                SAPbobsCOM.ChartOfAccounts oContaContabil = (SAPbobsCOM.ChartOfAccounts)Globals.Master.Connection.Database.GetBusinessObject(BoObjectTypes.oChartOfAccounts);



                if (oParceiroNegocio.GetByKey(CardCode))
                {
                    string ContaPai = oParceiroNegocio.CardType == BoCardTypes.cCustomer ? Configuracoes.ContaPaiCliente : Configuracoes.ContaPaiFornecedor;

                    oRecordSet.DoQuery($@"
                            SELECT 
                                AcctCode 
                            FROM 
                                OACT 
                            WHERE 
                                FatherNum = '{ContaPai}' AND
                                AcctName  = '{oParceiroNegocio.CardName}'");

                    if (!oRecordSet.EoF)
                        AcctCode = oRecordSet.Fields.Item("AcctCode").Value.ToString();

                    if (string.IsNullOrEmpty(AcctCode))
                    {
                        AcctCode = SqlUtils.GetValue($"SP_ZPN_CriaCodigoConta '{ContaPai}'");

                        oContaContabil.Code = AcctCode;
                        oContaContabil.Name = oParceiroNegocio.CardName;
                        oContaContabil.FatherAccountKey = ContaPai;
                        oContaContabil.LockManualTransaction = BoYesNoEnum.tYES;

                        if (oContaContabil.Add() != 0)
                            throw new Exception($"Erro ao criar conta contábil {Globals.Master.Connection.Database.GetLastErrorCode()}- {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                    }

                    oParceiroNegocio.DebitorAccount = AcctCode;

                    if (oParceiroNegocio.Update() != 0)
                        throw new Exception($"Erro ao atualizar conta contábil do PN - {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                    oRecordSet.DoQuery($"SELECT 1 FROM OPRC WHERE U_CardCode  = '{oParceiroNegocio.CardCode}' ");

                    if (oRecordSet.EoF)
                    {

                        oRecordSet.DoQuery("SELECT max(cast(PrcCode as int))+1  FROM OPRC where isnumeric(PrcCode) = 1");

                        string CenterCode = oRecordSet.Fields.Item(0).Value.ToString();

                        SAPbobsCOM.CompanyService oCompanyService = Globals.Master.Connection.Database.GetCompanyService();
                        SAPbobsCOM.ProfitCentersService oPCService;
                        SAPbobsCOM.ProfitCenter oPC;
                        SAPbobsCOM.ProfitCenterParams oPCParams;

                        oPCService = (SAPbobsCOM.ProfitCentersService)oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ProfitCentersService);
                        oPCParams = (SAPbobsCOM.ProfitCenterParams)oPCService.GetDataInterface(SAPbobsCOM.ProfitCentersServiceDataInterfaces.pcsProfitCenterParams);
                        oPC = (SAPbobsCOM.ProfitCenter)oPCService.GetDataInterface(SAPbobsCOM.ProfitCentersServiceDataInterfaces.pcsProfitCenter);
                        oPC.CenterCode = CenterCode;
                        oPC.CenterName = oParceiroNegocio.CardName.Length > 30 ? oParceiroNegocio.CardName.Substring(0, 30) : oParceiroNegocio.CardName;
                        oPC.InWhichDimension = Configuracoes.DimensaoCentroCustoCliente;
                        oPC.CostCenterType = Configuracoes.TipoCentroCustoCliente;
                        oPC.Effectivefrom = DateTime.Today;
                        oPC.UserFields.Item("U_CardCode").Value = oParceiroNegocio.CardCode;
                        oPC.UserFields.Item("U_Descricao").Value = oParceiroNegocio.CardName;
                        oPC.UserFields.Item("U_MM_Item").Value = ".";
                        oPC.UserFields.Item("U_MM_DRZ").Value = ".";
                        oPCParams = oPCService.AddProfitCenter(oPC);
                    }
                }
                CodigoConta = AcctCode;

                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao gerar Conta Contábil do PN - {Ex.Message}");
            }
        }
    }
}
