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

            CentroCustoLocalizacao.CriarCentroCustoLocalizacao();            

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

        private static void AtualizaPN()
        {

            //
            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            SAPbobsCOM.BusinessPartners oParceiroNegocio = (SAPbobsCOM.BusinessPartners)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            oRecordSet.DoQuery(@"select ""CardCode"", ""GroupNum"" from vw_atualizapn order by ""GroupNum""");

            while (!oRecordSet.EoF)
            {
                oParceiroNegocio = (SAPbobsCOM.BusinessPartners)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (oParceiroNegocio.GetByKey(oRecordSet.Fields.Item("CardCode").Value.ToString()))
                {
                    oParceiroNegocio.PayTermsGrpCode = Convert.ToInt32(oRecordSet.Fields.Item("GroupNum").Value);

                    oParceiroNegocio.Update();
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
