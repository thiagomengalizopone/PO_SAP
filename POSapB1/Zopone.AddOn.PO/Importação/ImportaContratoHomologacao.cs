using sap.dev.core;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.Helpers;

namespace Zopone.AddOn.PO.Importação
{
    public class ImportaContratoHomologacao
    {

        public static void ImportarObrasSAPB1()
        {
            System.Data.DataTable DtContrato =
                       SqlUtils.ExecuteCommand(@"
                                                SELECT * FROM vw_sp_importaobrapcivalidacao
                        ");


            foreach (DataRow row in DtContrato.Rows)
            {
                try
                {
                    UtilProjetos.SalvarProjeto(row["referencia"].ToString(), row["referencia"].ToString());

                    //SqlUtils.DoNonQuery($"SP_ZPN_INSEREIMPORTAOBRA '{row["referencia"].ToString()}'");
                    
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


            foreach (DataRow row in DtContrato.Rows)
            {
                try
                {
                    // Acessa os valores das colunas usando o nome das colunas
                    ImportaContrato(
                        row["CardCode"].ToString(),
                        Convert.ToDateTime(row["iniciocontrato"]),
                        row["contratoid"].ToString(),
                        row["referencia"].ToString(),
                        row["descricao"].ToString()
                    );
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
