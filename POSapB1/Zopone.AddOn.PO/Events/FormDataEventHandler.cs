using sap.dev.core.Controller;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.Alocacao;
using Zopone.AddOn.PO.View.ClassificacaoObra;
using Zopone.AddOn.PO.View.Contrato;
using Zopone.AddOn.PO.View.Obra;
using Zopone.AddOn.PO.View.FrmParceiroNegocio;
using Zopone.AddOn.PO.View.Deposito;

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
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmAloca)
            {
                BubbleEvent = FrmAloca.Interface_FormDataEvent(ref BusinessObjectInfo);
            }
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmClassificacaoObra)
            {
                BubbleEvent = FrmClassObra.Interface_FormDataEvent(ref BusinessObjectInfo);
            }
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmParceiroNegocio)
            {
                BubbleEvent = Frm134.Interface_FormDataEvent(ref BusinessObjectInfo);
            }
            

        }
    }
}
