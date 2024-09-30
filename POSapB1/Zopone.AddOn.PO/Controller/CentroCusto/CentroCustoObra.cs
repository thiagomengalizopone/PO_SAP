﻿using sap.dev.core;
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
    public class CentroCustoObra
    {




        public static void AtualizaCentroCustoObra()
        {
            string SQL_Query = "select PrcCode from oprc where (validfrom > '2024-01-01'  or dimcode = 2) and dimcode <=3  ";

            DataTable DtResultados = SqlUtils.ExecuteCommand(SQL_Query);

            int cont = 0;


            foreach (DataRow dr in DtResultados.Rows)
            {
                cont++;
                Util.ExibirMensagemStatusBar($"Atualizando centro de custo {cont} de {DtResultados.Rows.Count}");
                CentroCusto.AtualizaCentroCusto(dr["PrcCode"].ToString());
            }

        }


        public static void CriarCentroCustoObra()
        {
            Int32 Dimensao = Convert.ToInt32(SqlUtils.GetValue(@"SELECT Max(T0.""DimCode"") FROM ODIM T0 WHERE T0.""DimDesc"" = 'OBRA'"));
            string TipoCentroCusto = SqlUtils.GetValue(@"SELECT maX(CctCode) FROM OCCT WHERE CctName = 'Receitas'");



            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string SQL_Query = ($@"
                                select 
	                                OBRA.""Code""
                                from 
	                                ""@ZPN_OPRJ"" OBRA
                                WHERE
	                                OBRA.""Code""  NOT in 
	                                (
		                                SELECT
			                                OPRC.""PrcName""
		                                FROM
			                                OPRC
		                                WHERE 
			                                OPRC.""PrcName"" = OBRA.""Code""
	                                )
                                order by 
	                                OBRA.""Code"" 
                                desc 
                                ");

            DataTable DtResultados =  SqlUtils.ExecuteCommand(SQL_Query);
            
            
            foreach (DataRow dr in DtResultados.Rows) 
            {
                CentroCusto.CriaCentroCusto(dr["Code"].ToString(), Dimensao, TipoCentroCusto, "", "", dr["Code"].ToString());
            }

        }
    }
}
