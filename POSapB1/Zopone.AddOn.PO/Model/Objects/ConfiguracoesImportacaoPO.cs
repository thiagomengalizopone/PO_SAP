using sap.dev.core;
using sap.dev.data;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.Model.Objects
{
    public static class ConfiguracoesImportacaoPO
    {

        public static string ItemCodePO { get; set; }
        public static string ItemNamePO { get; set; }
        public static string CardCodePO { get; set; }
        public static string CardNamePO { get; set; }
        public static string TipoDocumentoPO { get; set; }



        public static void CarregarConfiguracoesPO()
        {
            try
            {
                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);


                oRecordSet.DoQuery($@"
                                        SELECT 
                                            ""U_CardCodeH"", 
                                            ""U_CardNameH"", 
                                            ""U_ItemCode"", 
                                            ""U_ItemName"", 
                                            ""U_TipoDoc""
                                        FROM
                                            ""@ZPN_CONFPO""
                                        WHERE
                                            ""Code"" = '1'
                ");

                if (!oRecordSet.EoF)
                {
                    CardCodePO = oRecordSet.Fields.Item("U_CardCodeH").Value.ToString();
                    CardNamePO = oRecordSet.Fields.Item("U_CardNameH").Value.ToString();
                    ItemCodePO = oRecordSet.Fields.Item("U_ItemCode").Value.ToString();
                    ItemNamePO = oRecordSet.Fields.Item("U_ItemName").Value.ToString();
                    TipoDocumentoPO = oRecordSet.Fields.Item("U_TipoDoc").Value.ToString();
                }              
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar configurações PO: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }


    }


    






}
