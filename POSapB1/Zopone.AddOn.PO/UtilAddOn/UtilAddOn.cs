using sap.dev.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.Menus;

namespace Zopone.AddOn.PO.UtilAddOn
{
    public class UtilAddOn
    {
        public static void CarregarMenus()
        {
            Globals.Master.Connection.Interface.Forms.GetFormByTypeAndCount(169, 1).Freeze(true);
            try
            {

                AddMenusGestaoUtilitarios mnuGC = new AddMenusGestaoUtilitarios();
                mnuGC.AddMenus();
            }
            finally
            {
                Globals.Master.Connection.Interface.Forms.GetFormByTypeAndCount(169, 1).Freeze(false);
                Globals.Master.Connection.Interface.Forms.GetFormByTypeAndCount(169, 1).Update();
            }
        }
    }
}
