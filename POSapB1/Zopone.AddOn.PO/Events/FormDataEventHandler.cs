﻿using sap.dev.core.Controller;
using Zopone.AddOn.PO.Controller.Constantes;
using Zopone.AddOn.PO.View.Alocacao;
using Zopone.AddOn.PO.View.ClassificacaoObra;
using Zopone.AddOn.PO.View.Contrato;
using Zopone.AddOn.PO.View.Obra;
using Zopone.AddOn.PO.View.FrmParceiroNegocio;
using Zopone.AddOn.PO.View.Deposito;
using Zopone.AddOn.PO.View.ParametrosPO;
using Zopone.AddOn.PO.View.NotaFiscal;
//using Zopone.AddOn.PO.View.ParametrosPO;

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
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmParametrosImportacao)
            {
                BubbleEvent = FrmParamPO.Interface_FormDataEvent(BusinessObjectInfo);
            }
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmNotaFiscalSaida) 
            {
                BubbleEvent = Frm133.Interface_FormDataEvent(BusinessObjectInfo);
            }
            else if (BusinessObjectInfo.FormTypeEx == FormConstantes.FrmEditarNotaFiscalSaida)
            {
                BubbleEvent = FrmNotaFiscal.Interface_FormDataEvent(BusinessObjectInfo);
            }
            

        }
    }
}
