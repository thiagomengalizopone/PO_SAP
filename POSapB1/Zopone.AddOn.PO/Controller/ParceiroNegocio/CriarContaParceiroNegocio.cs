using sap.dev.core;
using sap.dev.core.Controller;
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
                        CentroCusto.CriaCentroCusto(oParceiroNegocio.CardName, Configuracoes.DimensaoCentroCustoCliente, Configuracoes.TipoCentroCustoCliente, oParceiroNegocio.CardCode, oParceiroNegocio.CardName);
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
