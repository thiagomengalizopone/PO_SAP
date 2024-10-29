using sap.dev.core;
using sap.dev.core.Controller;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.Helpers;

namespace Zopone.AddOn.PO.Importação
{
    public class ImportaContratoHomologacao
    {




        public static void criacentrocusto()
        {

            System.Data.DataTable DtContrato =
                       SqlUtils.ExecuteCommand(@"                                                
                           SELECT 
	                            ""Code""
                            FROM 
	                            ""@ZPN_OPRJ""
                            WHERE
	                            ""Code"" NOT IN 
	                            (
		                            select 
			                            U_Obra
		                            from	
			                            oprc
		                            where
			                            U_Obra = ""Code""
	                            )


                        ");


            Int32 Dimensao = Convert.ToInt32(SqlUtils.GetValue(@"SELECT Max(T0.""DimCode"") FROM ODIM T0 WHERE T0.""DimDesc"" = 'OBRA'"));
            string TipoCentroCusto = SqlUtils.GetValue(@"SELECT maX(CctCode) FROM OCCT WHERE CctName = 'Receitas'");
            int count = 0;
            foreach (DataRow row in DtContrato.Rows)
            {
                try
                {
                    Util.ExibirMensagemStatusBar($"Criando centro de custo {count} de {DtContrato.Rows.Count}");

                    count++;

                    string CentroCustoRetorno = CentroCusto.CriaCentroCusto(row["Code"].ToString(), Dimensao, TipoCentroCusto, "", "", row["Code"].ToString());

                    string SqL_UPDATE = $@"UPDATE ""@ZPN_OPRJ"" SET ""U_PCG"" = '{CentroCustoRetorno}' WHERE ""Code"" =  '{row["Code"].ToString()}'";

                }
                catch (Exception ex)
                {
                    // Trate exceções ou faça logging
                    Console.WriteLine($"Erro ao importar contrato: {ex.Message}");
                }
            }


            
        }
        public static void ImportarObrasSAPB1()
        {
            System.Data.DataTable DtContrato =
                       SqlUtils.ExecuteCommand(@"                                                
                                    SELECT 
                                        REFERENCIA, 
                                        ABSID,
                                        DESCRIPT
                                    FROM
                                        vw_tmp_obrasimportar
                                    where
	                                    REFERENCIA collate SQL_Latin1_General_CP1_CI_AS NOT IN 
	                                    (
		                                    SELECT PrjCode FROM OPRJ where PRjCode = referencia  collate SQL_Latin1_General_CP1_CI_AS
	                                    );

                        ");

            int count = 0;
            foreach (DataRow row in DtContrato.Rows)
            {
                try
                {
                    count++;

                    UtilProjetos.SalvarProjeto(row["REFERENCIA"].ToString(), row["REFERENCIA"].ToString());


                    Util.ExibirMensagemStatusBar($"Importando obra {count} de {DtContrato.Rows.Count}");


                    //SqlUtils.DoNonQuery($"SP_ZPN_INSEREIMPORTAOBRA2 '{row["REFERENCIA"].ToString()}'");


                    //SqlUtils.DoNonQuery($"ZPN_SP_PCI_ATUALIZAOBRA '{row["REFERENCIA"].ToString()}', '2024-01-01'");


                }
                catch (Exception ex)
                {
                    // Trate exceções ou faça logging
                    Console.WriteLine($"Erro ao importar contrato: {ex.Message}");
                }
            }
        }


        public static void ImportaContratoValidacao()
        {
             
            System.Data.DataTable DtContrato = 
                        SqlUtils.ExecuteCommand(@"
                                                SELECT * FROM vw_sp_temp_importarcontratos
                        ");

            Int32 rowID = 1;
            Int32 TotalContrato = DtContrato.Rows.Count;
            foreach (DataRow row in DtContrato.Rows)
            {
                try
                {
                    Util.ExibirMensagemStatusBar($"Importando contrato {rowID} de {TotalContrato}");


                    // Acessa os valores das colunas usando o nome das colunas
                    ImportaContrato(
                        row["CardCode"].ToString(),
                        Convert.ToDateTime("2024-01-01"),
                        row["contratoid"].ToString(),
                        row["referencia"].ToString(),
                        row["descricao"].ToString()
                    );

                    rowID++;
                }
                catch (Exception ex)
                {
                    // Trate exceções ou faça logging
                    Console.WriteLine($"Erro ao importar contrato: {ex.Message}");
                }
            }


        }




        private static void ImportaContrato(string CardCode, DateTime DataInicio, string ContratoId, string referencia, string descricao)
        {


            // Declare variables

            SAPbobsCOM.CompanyService oCompanyService;

            SAPbobsCOM.BlanketAgreementsService oBlanketAgreementsService;

            SAPbobsCOM.BlanketAgreement oBlanketAgreement;

            oCompanyService = Globals.Master.Connection.Database.GetCompanyService();


            oBlanketAgreementsService = oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.BlanketAgreementsService) as SAPbobsCOM.BlanketAgreementsService;

            oBlanketAgreement = oBlanketAgreementsService.GetDataInterface(SAPbobsCOM.BlanketAgreementsServiceDataInterfaces.basBlanketAgreement) as SAPbobsCOM.BlanketAgreement;



            oBlanketAgreement.BPCode = CardCode;

            oBlanketAgreement.Series = 47;

            oBlanketAgreement.StartDate = DataInicio;

            oBlanketAgreement.SigningDate = DateTime.Now;

            oBlanketAgreement.UserFields.Item("U_IdPCI").Value = ContratoId;

            oBlanketAgreement.Description = referencia;

            oBlanketAgreement.Remarks = descricao;


            DateTime dataDocDateF;



            oBlanketAgreement.EndDate = DataInicio.AddYears(10);



            oBlanketAgreement.AgreementMethod = SAPbobsCOM.BlanketAgreementMethodEnum.amMonetary;



            oBlanketAgreement.Status = SAPbobsCOM.BlanketAgreementStatusEnum.asApproved;

            SAPbobsCOM.BlanketAgreements_ItemsLine oBlanketAgreements_ItemsLines = oBlanketAgreement.BlanketAgreements_ItemsLines.Add();

            oBlanketAgreements_ItemsLines.PlannedAmountLC = 9999999;

            // Save changes

            oBlanketAgreementsService.AddBlanketAgreement(oBlanketAgreement);




        }


    }
}
