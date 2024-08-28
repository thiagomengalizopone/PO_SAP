using sap.dev.core.Controller;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
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

            oRecordSet.DoQuery(UtilScriptsSQLAddOn.SQL_Localizacao(CodeLocalizacao));

            while (!oRecordSet.EoF)
            {
                CentroCusto.CriaCentroCusto(oRecordSet.Fields.Item("Location").Value.ToString(), Dimensao, TipoCentroCusto, "", "", "",  oRecordSet.Fields.Item("Code").Value.ToString());

                oRecordSet.MoveNext();
            }


        }
    }
}
