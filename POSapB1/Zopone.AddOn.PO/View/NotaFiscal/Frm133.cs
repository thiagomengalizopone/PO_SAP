using sap.dev.core;
using sap.dev.core.ApiService_n8n;
using sap.dev.core.DTO;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.FrmParceiroNegocio
{
    public class Frm133 : FormSDK
    {

        public class ItensUIDs
        {

        }

        public Frm133() : base()
        {
            if (oForm == null)
                return;
        }

        internal static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {

            
            return true;
        }

        internal static bool Interface_FormDataEvent(BusinessObjectInfo businessObjectInfo)
        {
            if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD && !businessObjectInfo.BeforeAction && businessObjectInfo.ActionSuccess)
            {

            }
            
            return true;
        
        }
    }

}