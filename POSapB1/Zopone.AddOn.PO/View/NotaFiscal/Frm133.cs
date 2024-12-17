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
using Zopone.AddOn.PO.Helpers;
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
            if (
                (
                businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD ||
                businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE 
                )
                && 
                !businessObjectInfo.BeforeAction && 
                businessObjectInfo.ActionSuccess)
            {
                Form oFormNF = Globals.Master.Connection.Interface.Forms.Item(businessObjectInfo.FormUID);

                Int32 DocEntry = 0;

                DBDataSource oDBOINV = oFormNF.DataSources.DBDataSources.Item(0);

                if (businessObjectInfo.Type == "112")
                {
                    if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                        DocEntry = Convert.ToInt32(oDBOINV.GetValue("DocEntry", 0));
                    else
                        DocEntry = Convert.ToInt32(SqlUtils.GetValue(@"SELECT MAX(""DocEntry"") FROM ODRF WHERE ObjType = '13' "));

                    SqlUtils.DoNonQuery($"SP_ZPN_VERIFICACADASTROPCI {DocEntry}, 112");

                    UtilPCI.EnviarDadosNFDigitacaoPCIAsync(DocEntry);
                }
                else
                {
                    if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                        DocEntry = Convert.ToInt32(oDBOINV.GetValue("DocEntry", 0));
                    else
                        DocEntry = Convert.ToInt32(SqlUtils.GetValue(@"SELECT MAX(""DocEntry"") FROM OINV WHERE ObjType = '13' "));

                    SqlUtils.DoNonQuery($"SP_ZPN_VERIFICACADASTROPCI {DocEntry}, 13");

                    UtilPCI.EnviarDadosNFLiberadaPCIAsync(DocEntry);

                }


            }
            
            return true;
        
        }
    }

}