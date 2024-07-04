using sap.dev.core.Menus;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
                    MenuConstantes.MnuFaturamento,
                    "Faturamento",
                    16,
                    imagem));

            listMenu.Add(new Menu(
                     MenuConstantes.MnuFaturamento,
                     BoMenuType.mt_STRING,
                     MenuConstantes.MnuClassificacaoObra,
                     "Classificação Obra",
                     15,
                     null));

            listMenu.Add(new Menu(
                   MenuConstantes.MnuFaturamento,
                   BoMenuType.mt_STRING,
                   MenuConstantes.MnuAlocacao,
                   "Alocação",
                   16,
                   null));

            listMenu.Add(new Menu(
                   MenuConstantes.MnuFaturamento,
                   BoMenuType.mt_STRING,
                   MenuConstantes.MnuContrato,
                   "Contrato",
                   16,
                   null));

            listMenu.Add(new Menu(
                   MenuConstantes.MnuFaturamento,
                   BoMenuType.mt_STRING,
                   MenuConstantes.MnuObra,
                   "Obra",
                   16,
                   null));

            listMenu.Add(new Menu(
                      MenuConstantes.MnuFaturamento,
                      BoMenuType.mt_STRING,
                      MenuConstantes.MnuCadPO,
                      "Cadastro de PO",
                      17,
                      null));

           






            #endregion [ PO ]



            return listMenu;
        }
    }
}
