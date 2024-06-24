using sap.dev.core;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.Controller.Constantes;

namespace Zopone.AddOn.PO.Menus
{
    public class AddMenusGestaoUtilitarios
    {
        public void AddMenus()
        {
            try
            {
                MenusGestaoUtilitarios  mnuGC = new MenusGestaoUtilitarios();

                Util.RemoverMenus(MenuConstantes.MnuFaturamento);

                sap.dev.core.Util.AddMenuItens(mnuGC.GetMenus());
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao adicionar Menu {Ex.Message}", BoMessageTime.bmt_Long, true, Ex);
            }
        }
    }
}
