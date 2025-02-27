using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using sap.dev.core;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Zopone.AddOn.PO.Controller.Localizacao;
using Zopone.AddOn.PO.Importação;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.UtilAddOn;
using static sap.dev.core.EnumList;
using System.Text;
using System.Net.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using sap.dev.data;
using SAPbobsCOM;

namespace Zopone.AddOn.PO
{
    public class AddOnMain
    {

        public static void Inicializar(string[] aArguments)
        {
            Util.LimparArquivosTEMP();

            InstanceStartup ISup = new InstanceStartup(aArguments);

            SAPConnection lConexao = ISup.lConexao;

            Globals.Master = new Master(lConexao, EnumAddOn.CadastroPO, GetDLLVersion());

            Util.CriarPastaLog();

            Install.VerificaInstalacaoAddOn();

            Instalar.ExecutaScriptsAtualizacaoCampos();

            Configuracoes.CarregarConfiguracaoes();
            UtilAddOn.UtilAddOn.CarregarMenus();

            ConfiguracoesImportacaoPO.CarregarConfiguracoesPO();

            Globals.Master.Connection.Interface.MenuEvent += MenuEventHandler.Interface_MenuEvent;
            Globals.Master.Connection.Interface.ItemEvent += ItemEventHandler.Interface_ItemEvent;
            Globals.Master.Connection.Interface.FormDataEvent += FormDataEventHandler.Interface_FormDataEvent;
            Globals.Master.Connection.Interface.RightClickEvent += RightClickEventHandler.Interface_RightClickEvent;

            UtilWarehouses.CriaDepositosRAAsync();

            Util.ExibirMensagemStatusBar("AddOn Faturamento iniciado com sucesso!", SAPbouiCOM.BoMessageTime.bmt_Long);
        }

        private static void AtualizaItensSAP()
        {
            try
            {
                SAPbobsCOM.Items oItem = (SAPbobsCOM.Items)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                string itemCode = string.Empty;

                oRecordSet.DoQuery(@"
	                                SELECT distinct
		                                ItemCode
	                                FROM 
		                                OITM 
	                                WHERE 
		                                SellItem <> 'Y' AND InvntItem = 'Y'
                                ");

                int itemCount = 1;
                while (!oRecordSet.EoF)
                {
                    itemCode = oRecordSet.Fields.Item("ItemCode").Value.ToString();

                    if (oItem.GetByKey(itemCode))
                    {
                        oItem.SalesItem = BoYesNoEnum.tYES;
                        
                        if (oItem.Update() != 0)
                            throw new Exception($"Erro ao Atualizar Item: {Globals.Master.Connection.Database.GetLastErrorCode()} {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }

                    itemCount++;

                    oRecordSet.MoveNext();
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao atualizar itens SAP: {Ex.Message}");
            }
        }

        private static void AtualizaCorrigeVencimentos()
        {
            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordSet.DoQuery(@"
                                SELECT DISTINCT 
                                    DocEntry
                                FROM 
	                                INV6 
                                WHERE 
	                                DUEDATE < '2025-01-01'
                                ");

            Documents oNotaFiscalSaidaImposto = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

            while (!oRecordSet.EoF)
            {
                try
                {
                    Int32 DocEntry = Convert.ToInt32(oRecordSet.Fields.Item("DocEntry").Value);

                    if (oNotaFiscalSaidaImposto.GetByKey(DocEntry))
                    {
                        for (int iRow = 0; iRow < oNotaFiscalSaidaImposto.Installments.Count; iRow++)
                        {
                            oNotaFiscalSaidaImposto.Installments.SetCurrentLine(iRow);
                            oNotaFiscalSaidaImposto.Installments.DueDate = Convert.ToDateTime("2025-01-30");
                        }

                        if (oNotaFiscalSaidaImposto.Update() != 0)
                            throw new Exception($"Erro ao Atualizar NF Faturamento: {oNotaFiscalSaidaImposto.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorCode()} {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                    }
                }
                catch (Exception Ex)
                {
                    string erro = Ex.ToString();
                }
                oRecordSet.MoveNext();
            }
        }

        private static void AtualizaEsbocoCidadeISS()
        {

            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordSet.DoQuery(@"
                                SELECT 
	                                DocEntry,
	                                U_ISS
                                FROM 
	                                VW_IMPOSTODOCUMENTOISSCIDADE
                                order by 
	                                DocEntry
                                ");

            Documents oNotaFiscalSaidaImposto = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            while (!oRecordSet.EoF)
            {
                try
                {
                    Int32 DocEntry = Convert.ToInt32(oRecordSet.Fields.Item("DocEntry").Value);
                    string IssCode = oRecordSet.Fields.Item("U_ISS").Value.ToString();

                    if (oNotaFiscalSaidaImposto.GetByKey(DocEntry))
                    {
                        string cardcode = oNotaFiscalSaidaImposto.CardCode;

                        if (!string.IsNullOrEmpty(IssCode))
                        {
                            oNotaFiscalSaidaImposto.Lines.SetCurrentLine(0);

                            if (!string.IsNullOrEmpty(oNotaFiscalSaidaImposto.Lines.WithholdingTaxLines.WTCode))
                                oNotaFiscalSaidaImposto.Lines.WithholdingTaxLines.Add();

                            oNotaFiscalSaidaImposto.SequenceCode = 29;
                            oNotaFiscalSaidaImposto.SequenceModel = "46";

                            oNotaFiscalSaidaImposto.DiscountPercent = 0;

                            oNotaFiscalSaidaImposto.Lines.WithholdingTaxLines.WTCode = IssCode;

                            if (oNotaFiscalSaidaImposto.Update() != 0)
                                throw new Exception($"Erro ao Atualizar NF Faturamento: {oNotaFiscalSaidaImposto.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorCode()} {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                        }
                    }
                }
                catch (Exception Ex)
                {
                    string erro = Ex.ToString();
                }
                oRecordSet.MoveNext();
            }
        }

        private static Int32 GetDLLVersion()
        {
            return Convert.ToInt32(Regex.Replace(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion, "[^0-9]", ""));
        }
    }
}
