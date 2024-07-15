using sap.dev.core;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Zopone.AddOn.PO.UtilAddOn;
using static sap.dev.core.EnumList;

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

            Configuracoes.CarregarConfiguracaoes();
            UtilAddOn.UtilAddOn.CarregarMenus();

            Globals.Master.Connection.Interface.MenuEvent += MenuEventHandler.Interface_MenuEvent;
            Globals.Master.Connection.Interface.ItemEvent += ItemEventHandler.Interface_ItemEvent;
            Globals.Master.Connection.Interface.FormDataEvent += FormDataEventHandler.Interface_FormDataEvent;
            Globals.Master.Connection.Interface.RightClickEvent += RightClickEventHandler.Interface_RightClickEvent;
        }

        private static Int32 GetDLLVersion()
        {
            return Convert.ToInt32(Regex.Replace(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion, "[^0-9]", ""));
        }
    }
}
