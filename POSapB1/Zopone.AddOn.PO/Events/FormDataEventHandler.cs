using sap.dev.core.Controller;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.Contrato;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO
{
    public class FormDataEventHandler
    {
        public static void Interface_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (BusinessObjectInfo.FormTypeEx == FormConstants.FrmObra)
            {
                BubbleEvent = FrmObra.Interface_FormDataEvent(ref BusinessObjectInfo);
            }
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmContratoGuardaChuvas)
            {
                BubbleEvent = Frm1250000100.Interface_FormDataEvent(ref BusinessObjectInfo);
            }

        }
    }
}
