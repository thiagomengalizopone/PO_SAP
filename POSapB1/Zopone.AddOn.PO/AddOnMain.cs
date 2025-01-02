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

            #region DEBUG
            //ImportaContratoHomologacao.ImportaContratoValidacao();
            //ImportaContratoHomologacao.ImportarObrasSAPB1();
            //ImportaContratoHomologacao.criacentrocusto();
            #endregion

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
