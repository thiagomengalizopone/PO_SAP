using SAPbouiCOM;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.Config;
using Zopone.AddOn.PO.View.Contrato;
using Zopone.AddOn.PO.View.Deposito;

namespace Zopone.AddOn.PO
{
    public class ItemEventHandler
    {
        public static void Interface_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;


            if (pVal.FormTypeEx == FormConstantes.FrmContratoGuardaChuvas)
            {
                BubbleEvent = Frm1250000100.Interface_FormItemEvent(ref pVal);

            }
            else if (pVal.FormTypeEx == FormConstantes.FrmProjetoObra)
            {
                BubbleEvent = Frm711.Interface_FormItemEvent(ref pVal);
            }
            else if (pVal.FormTypeEx == FormConstantes.FrmConfiguracoesPO)
            {
                BubbleEvent = FrmConfPO.Interface_FormItemEvent(ref pVal);
            }
            else if (pVal.FormTypeEx == FormConstantes.FrmDeposito)
            {
                BubbleEvent = Frm62.Interface_FormItemEvent(ref pVal);
            }
        }
    }
}
