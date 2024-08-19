using sap.dev.core;
using System;
using System.Diagnostics;
using System.Threading;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.Alocação;
using Zopone.AddOn.PO.View.ClassificacaoObra;
using Zopone.AddOn.PO.View.Config;
using Zopone.AddOn.PO.View.Faturamento;
using Zopone.AddOn.PO.View.Obra;
using Zopone.AddOn.PO.View.PO;


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
                        case MenuConstantes.MnuConfImportacaoPO:
                            {
                                new FrmConfPO();
                            }
                            break;
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
                        case MenuConstantes.MnuContrato:
                            {
                                Globals.Master.Connection.Interface.ActivateMenuItem("2705");
                            }
                            break;
                        case MenuConstantes.MnuObra:
                            {
                                new FrmObra();
                            }
                            break;
                        case MenuConstantes.MnuCadPO:
                            {
                                FrmPO.MenuPO();
                            }
                            break;
                        case MenuConstantes.MnuImportacaoPO:
                            {
                                FrmImportacaoPO.MenuImpPO();
                            }
                            break;
                        case MenuConstantes.MnuVerificaImpPO:
                            {
                                FrmVerificaImportacaoPO.MenuVerificaPO();
                            }
                            break;
                        case MenuConstantes.MnuFaturarPO:
                            {
                                new FrmListaFaturamento();
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
