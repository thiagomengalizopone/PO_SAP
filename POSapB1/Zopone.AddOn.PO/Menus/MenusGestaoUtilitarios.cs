using sap.dev.core.Menus;
using SAPbouiCOM;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Zopone.AddOn.PO.Controller.Constantes;

namespace Zopone.AddOn.PO.Menus
{
    public class MenusGestaoUtilitarios : MenusAddons
    {
        public const string PrefixoMenu = "Gc";


        public override List<Menu> GetMenus()
        {
            string imagem = string.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\zopone.jpg");

            var listMenu = new List<Menu>();

            #region [ PO ]
            listMenu.Add(new Menu(
                    $"43520",
                    BoMenuType.mt_POPUP,
                    MenuConstantes.MnuFaturamentoPO,
                    "Faturamento",
                    16,
                    imagem));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuFaturamentoPO,
                 BoMenuType.mt_POPUP,
                 MenuConstantes.MnuConfiguracoes,
                 "Configurações",
                 17,
                 null));

            listMenu.Add(new Menu(
              MenuConstantes.MnuConfiguracoes,
              BoMenuType.mt_STRING,
              MenuConstantes.MnuConfAddOnPO,
              "Configuração AddOn PO",
              17,
              null));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuConfiguracoes,
                 BoMenuType.mt_STRING,
                 MenuConstantes.MnuConfImpFatPO,
                 "Configuração Importação Faturamento PO",
                 17,
                 null));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuConfiguracoes,
                 BoMenuType.mt_STRING,
                 MenuConstantes.MnuConfImportacaoPO,
                 "Configuração Importação PO",
                 17,
                 null));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuFaturamentoPO,
                 BoMenuType.mt_POPUP,
                 MenuConstantes.MnuCadastros,
                 "Cadastros",
                 17,
                 null));

            listMenu.Add(new Menu(
                     MenuConstantes.MnuCadastros,
                     BoMenuType.mt_STRING,
                     MenuConstantes.MnuClassificacaoObra,
                     "Classificação Obra",
                     18,
                     null));

            listMenu.Add(new Menu(
                   MenuConstantes.MnuCadastros,
                   BoMenuType.mt_STRING,
                   MenuConstantes.MnuAlocacao,
                   "Alocação",
                   19,
                   null));

            listMenu.Add(new Menu(
                   MenuConstantes.MnuCadastros,
                   BoMenuType.mt_STRING,
                   MenuConstantes.MnuContrato,
                   "Contrato",
                   20,
                   null));

            listMenu.Add(new Menu(
                   MenuConstantes.MnuCadastros,
                   BoMenuType.mt_STRING,
                   MenuConstantes.MnuObra,
                   "Obra",
                   21,
                   null));
            listMenu.Add(new Menu(
               MenuConstantes.MnuCadastros,
               BoMenuType.mt_STRING,
               MenuConstantes.MnuGerarObra,
               "Gerar Obra",
               21,
               null));

            listMenu.Add(new Menu(
                             MenuConstantes.MnuFaturamentoPO,
                             BoMenuType.mt_POPUP,
                             MenuConstantes.MnuPO,
                             "PO",
                             17,
                             null));

            listMenu.Add(new Menu(
                      MenuConstantes.MnuPO,
                      BoMenuType.mt_STRING,
                      MenuConstantes.MnuCadPO,
                      "Cadastro de PO",
                      22,
                      null));

            listMenu.Add(new Menu(
                      MenuConstantes.MnuPO,
                      BoMenuType.mt_STRING,
                      MenuConstantes.MnuImportacaoPO,
                      "Importação de PO",
                      22,
                      null));

            listMenu.Add(new Menu(
                      MenuConstantes.MnuPO,
                      BoMenuType.mt_STRING,
                      MenuConstantes.MnuVerificaImpPO,
                      "Verifica Importação de PO",
                      23,
                      null));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuFaturamentoPO,
                 BoMenuType.mt_POPUP,
                 MenuConstantes.MnuFaturamento,
                 "Faturamento",
                 17,
                 null));


            listMenu.Add(new Menu(
                 MenuConstantes.MnuFaturamento,
                 BoMenuType.mt_STRING,
                 MenuConstantes.MnuPreFaturaPO,
                 "Gerar Pré-Faturamento PO",
                 24,
                 null));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuFaturamento,
                 BoMenuType.mt_STRING,
                 MenuConstantes.MnuEfetFaturaPO,
                 "Efetivar Pré-Faturamento PO",
                 24,
                 null));

            listMenu.Add(new Menu(
                MenuConstantes.MnuFaturamentoPO,
                BoMenuType.mt_POPUP,
                MenuConstantes.MnuManutencao,
                "Manutenção",
                25,
                null));

            listMenu.Add(new Menu(
                 MenuConstantes.MnuManutencao,
                 BoMenuType.mt_STRING,
                 MenuConstantes.MnuManutencaoDtProgramanda,
                 "Data Programada",
                 26,
                 null));


            #endregion [ PO ]



            return listMenu;
        }
    }
}
