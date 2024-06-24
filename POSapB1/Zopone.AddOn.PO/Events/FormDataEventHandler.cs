using sap.dev.core.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.View.Contrato;

namespace Zopone.AddOn.PO
{
    public class FormDataEventHandler
    {
        public static void Interface_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (BusinessObjectInfo.FormTypeEx == FormConstants.FrmParceiroNegocio)
            {
            }
        }
    }
}
