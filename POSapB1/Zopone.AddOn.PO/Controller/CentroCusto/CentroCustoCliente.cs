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
    public class CentroCustoCliente
    {

        public static void CriarCentroCustoCliente()
        {
            Int32 Dimensao = Convert.ToInt32(SqlUtils.GetValue(@"SELECT Max(T0.""DimCode"") FROM ODIM T0 WHERE T0.""DimDesc"" = 'PCG'"));
            string TipoCentroCusto = SqlUtils.GetValue(@"SELECT maX(CctCode) FROM OCCT WHERE CctName = 'Receitas'");



            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string SQL_Query = ($@"
                             select 
	                                        OCRD.""CardCode"", OCRD.""CardName""
                                        from 
	                                        OCRD
                                        WHERE
	                                        OCRD.""CardCode""  NOT in 
	                                        (
		                                        SELECT
			                                        OPRC.U_CardCode
		                                        FROM
			                                        OPRC
		                                        WHERE 
			                                        OPRC.U_CardCode = OCRD.""CardCode""
	                                        ) 
                                                                        ");

            DataTable DtResultados =  SqlUtils.ExecuteCommand(SQL_Query);
            
            
            foreach (DataRow dr in DtResultados.Rows) 
            {
                CentroCusto.CriaCentroCusto(dr["CardName"].ToString(), Dimensao, TipoCentroCusto, dr["CardCode"].ToString(), dr["CardName"].ToString());
            }

        }
    }
}
