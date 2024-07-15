using sap.dev.core;
using System;

namespace Zopone.AddOn.PO.UtilAddOn
{
    public class Configuracoes
    {
        public static SAPbobsCOM.Recordset oRecordsetConfig;
        public static bool GerarContasAutomaticamentePN => GetBoolConfig("U_GeraConPN");
        public static string ContaPaiCliente => GetStringConfig("U_ContaPaiC");
        public static Int32 DimensaoCentroCustoCliente => GetIntConfig("U_Dimensao");
        public static string TipoCentroCustoCliente => GetStringConfig("U_TipoCrC");
        public static string ContaPaiFornecedor => GetStringConfig("U_ContaPaiF");



        public static void CarregarConfiguracaoes()
        {
            oRecordsetConfig = (SAPbobsCOM.Recordset)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            oRecordsetConfig.DoQuery(@"
                                    SELECT 
	                                    ISNULL(U_GeraConPN,'') U_GeraConPN,
	                                    ISNULL(U_ContaPaiC,'')U_ContaPaiC,
	                                    ISNULL(U_Dimensao,'')U_Dimensao,
	                                    ISNULL(U_TipoCrC,'')U_TipoCrC,
	                                    ISNULL(U_ContaPaiF,'')U_ContaPaiF
                                    FROM
	                                     [@ZPN_UTILCONF]
                                    WHERE 
	                                    Code  = '000000001'
                                ");
        }

        private static string GetStringConfig(string config)
        {
            if (oRecordsetConfig.RecordCount == 0)
                CarregarConfiguracaoes();

            return oRecordsetConfig.Fields.Item(config).Value.ToString();
        }

        private static bool GetBoolConfig(string config)
        {
            if (oRecordsetConfig.RecordCount == 0)
                CarregarConfiguracaoes();

            switch (oRecordsetConfig.Fields.Item(config).Value.ToString())
            {
                case "Y": { return true; }
                case "N": { return false; }
                default: return false;
            }
        }


        private static Int32 GetIntConfig(string config)
        {
            if (oRecordsetConfig.RecordCount == 0)
                CarregarConfiguracaoes();

            return Convert.ToInt32(oRecordsetConfig.Fields.Item(config).Value);
        }
    }
}
