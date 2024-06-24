using sap.dev.core;
using System;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.ClassificacaoObra;
using Zopone.AddOn.PO.View.ConfiguracaoImportWMS;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO
{
    public class MenuEventHandler
    {
        public static void Interface_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            try
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.MenuUID)
                    {
                        case MenuConstantes.MnuClassificacaoObra:
                            {
                                new FrmClassObra();
                            }
                            break;
                    }
                }
            
                BubbleEvent = true;
            }
            catch (Exception ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao abrir menu {ex.Message}", SAPbouiCOM.BoMessageTime.bmt_Short, true, ex);
                BubbleEvent = false;
            }

        }
    }
}
