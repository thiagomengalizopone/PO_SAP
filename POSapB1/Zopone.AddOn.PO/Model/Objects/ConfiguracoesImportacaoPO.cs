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

        public static Int32 NumeroPO { get; set; }
        public static Int32 NumeroLinha { get; set; }
        public static Int32 QuantidadeFaturada { get; set; }
        public static Int32 CodigoServico { get; set; }
        public static Int32 Item { get; set; }
        public static Int32 ValorUnitario { get; set; }
        public static Int32 ValorTotal { get; set; }


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

                oRecordSet.DoQuery($@"

                                        SELECT 
                                            ISNULL(T0.[U_NumeroPO],'') U_NumeroPO, 
                                            ISNULL(T0.[U_NumeroLinha],'') U_NumeroLinha, 
                                            ISNULL(T0.[U_QtdeFat],'') U_QtdeFat, 
                                            ISNULL(T0.[U_CodigoServ],'') U_CodigoServ, 
                                            ISNULL(T0.[U_Item],'') U_Item, 
                                            ISNULL(T0.[U_ValorUnit],'') U_ValorUnit, 
                                            ISNULL(T0.[U_ValorTot],'')  U_ValorTot
                                        FROM 
                                            [@ZPN_PARAMPO]  T0
                                        WHERE
                                            ""Code"" = '1'
                ");

                if (!oRecordSet.EoF)
                {
                    NumeroPO = ExcelColumnToNumber(oRecordSet.Fields.Item("U_NumeroPO").Value.ToString());
                    NumeroLinha = ExcelColumnToNumber(oRecordSet.Fields.Item("U_NumeroLinha").Value.ToString());
                    QuantidadeFaturada = ExcelColumnToNumber(oRecordSet.Fields.Item("U_QtdeFat").Value.ToString());
                    CodigoServico = ExcelColumnToNumber(oRecordSet.Fields.Item("U_CodigoServ").Value.ToString());
                    Item = ExcelColumnToNumber(oRecordSet.Fields.Item("U_Item").Value.ToString());
                    ValorUnitario = ExcelColumnToNumber(oRecordSet.Fields.Item("U_ValorUnit").Value.ToString());
                    ValorTotal = ExcelColumnToNumber(oRecordSet.Fields.Item("U_ValorTot").Value.ToString());
                }



            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar configurações PO: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        public static int ExcelColumnToNumber(string column)
        {

            if (string.IsNullOrEmpty(column))
                return -1;

            int result = 0;

            // Itera sobre cada caractere da string da esquerda para a direita
            for (int i = 0; i < column.Length; i++)
            {
                // Obtemos o valor da letra atual (A = 0, B = 1, ..., Z = 25)
                int currentValue = column[i] - 'A';

                // Atualiza o resultado considerando a posição
                result = result * 26 + (currentValue + 1);  // Ajusta para que A seja 1, B seja 2,... Z seja 26
            }

            return result - 1;  // Subtrai 1 para que A seja 0, Z seja 26 e AA seja 27
        }
    }
}
