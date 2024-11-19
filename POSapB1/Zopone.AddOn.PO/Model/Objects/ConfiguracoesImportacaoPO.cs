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
        public static string CardCodePOHawuey { get; set; }
        public static string CardNamePOHawuey { get; set; }
        public static string CardCodePOEricsson { get; set; }
        public static string CardNamePOEricsson { get; set; }
        public static string TipoDocumentoPO { get; set; }
        public static string Utilizacao { get; set; }
        public static string SenhaSenior { get; set; }
        public static string UsuarioSenior { get; set; }


        public static void CarregarConfiguracoesPO()
        {
            try
            {
                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);


                oRecordSet.DoQuery($@"
                                        SELECT 
                                            ""U_CardCodeH"", 
                                            ""U_CardNameH"", 
                                            ""U_CardCodeE"", 
                                            ""U_CardNameE"", 
                                            ""U_ItemCode"", 
                                            ""U_ItemName"", 
                                            ""U_TipoDoc"",
                                            ""U_Usage"",
                                            ""U_SeSenior"",
                                            ""U_UsSenior""
                                        FROM
                                            ""@ZPN_CONFPO""
                                        WHERE
                                            ""Code"" = '1'
                ");

                if (!oRecordSet.EoF)
                {
                    CardCodePOHawuey = oRecordSet.Fields.Item("U_CardCodeH").Value.ToString();
                    CardNamePOHawuey = oRecordSet.Fields.Item("U_CardNameH").Value.ToString();
                    CardCodePOEricsson = oRecordSet.Fields.Item("U_CardCodeE").Value.ToString();
                    CardNamePOEricsson = oRecordSet.Fields.Item("U_CardNameE").Value.ToString();
                    ItemCodePO = oRecordSet.Fields.Item("U_ItemCode").Value.ToString();
                    ItemNamePO = oRecordSet.Fields.Item("U_ItemName").Value.ToString();
                    TipoDocumentoPO = oRecordSet.Fields.Item("U_TipoDoc").Value.ToString();
                    Utilizacao = oRecordSet.Fields.Item("U_Usage").Value.ToString();
                    SenhaSenior = oRecordSet.Fields.Item("U_SeSenior").Value.ToString();
                    UsuarioSenior = oRecordSet.Fields.Item("U_UsSenior").Value.ToString();                    
                }              
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar configurações PO: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
