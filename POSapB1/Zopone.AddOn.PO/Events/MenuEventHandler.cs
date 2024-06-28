using sap.dev.core;
using System;
using System.Threading;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.Alocação;
using Zopone.AddOn.PO.View.ClassificacaoObra;
using Zopone.AddOn.PO.View.ConfiguracaoImportWMS;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO
{
    public class MenuEventHandler
    {
        private static Thread formThread;

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
                        case MenuConstantes.MnuAlocacao:
                            {
                                new FrmAloca();
                            }
                            break;
                        case MenuConstantes.MnuCadPO:
                            {
                                formThread = new Thread(new ThreadStart(OpenForm2));
                                formThread.SetApartmentState(ApartmentState.STA); // Define o estado do apartamento como STA (Single-Threaded Apartment)
                                formThread.Start();
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

        private static void OpenForm2()
        {
            System.Windows.Forms.Application.Run(new FrmPO()); 
        }
    }
}
