using sap.dev.core.Controller;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.UtilAddOn;

namespace Zopone.AddOn.PO.Controller.Localizacao
{
    public class CentroCustoLocalizacao
    {

        public static void CriarCentroCustoLocalizacao(string CodeLocalizacao = "")
        {
            Int32 Dimensao = Convert.ToInt32(SqlUtils.GetValue(@"SELECT Max(T0.""DimCode"") FROM ODIM T0 WHERE T0.""DimDesc"" = 'REGIONAL'"));
            string TipoCentroCusto = SqlUtils.GetValue(@"SELECT maX(CctCode) FROM OCCT WHERE CctName = 'Receitas'");



            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string SQL_Query = UtilScriptsSQLAddOn.SQL_Localizacao(CodeLocalizacao);

            DataTable DtResultados =  SqlUtils.ExecuteCommand(SQL_Query);
            
            
            foreach (DataRow dr in DtResultados.Rows) 
            {
                CentroCusto.CriaCentroCusto(dr["Location"].ToString(), Dimensao, TipoCentroCusto, "", "", "",dr["Code"].ToString());
            }
        }
    }
}
