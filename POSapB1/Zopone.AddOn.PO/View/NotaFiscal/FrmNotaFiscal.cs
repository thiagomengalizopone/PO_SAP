using sap.dev.core;
using sap.dev.core.ApiService_n8n;
using sap.dev.core.DTO;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.NotaFiscal
{
    public class FrmNotaFiscal : FormSDK
    {
        public Matrix oMtItens { get; set; }
        public Matrix oMtAlocacao { get; set; }
        public DataTable DtAlocacao { get; set; }

        public FrmNotaFiscal() : base()
        {
            if (oForm == null)
                return;

            DtAlocacao = oForm.DataSources.DataTables.Item("DtAloc");

            oMtItens = (Matrix)oForm.Items.Item("MtItem").Specific;
            oMtAlocacao = (Matrix)oForm.Items.Item("MtAloca").Specific;

            oMtItens.AutoResizeColumns();
            oMtAlocacao.AutoResizeColumns();

            CarregaDadosAlocacao("-9999");
        }

        private void CarregaDadosAlocacao(string IdPciDocumento = "")
        {
            try
            {
                string SQL_Alocacao = $"SP_ZPN_ExibeAlocacaoDocumento '{IdPciDocumento}'";

                DtAlocacao.ExecuteQuery(SQL_Alocacao);

                oMtAlocacao.Columns.Item("Parcela").DataBind.Bind("DtAloc", "Parcela");
                oMtAlocacao.Columns.Item("Percent").DataBind.Bind("DtAloc", "Percentual");
                oMtAlocacao.Columns.Item("ValParc").DataBind.Bind("DtAloc", "ValorParcela");
                oMtAlocacao.Columns.Item("CodAloc").DataBind.Bind("DtAloc", "CodigoAlocacao");
                oMtAlocacao.Columns.Item("DescAloc").DataBind.Bind("DtAloc", "DescricaoAlocacao");
                oMtAlocacao.Columns.Item("IdPCI").DataBind.Bind("DtAloc", "IdPCI");

                oMtAlocacao.AutoResizeColumns();
            }
            catch (Exception ex) 
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados de alocação: {ex.Message}", BoMessageTime.bmt_Medium, true, ex);
            }
        }
        private static void CarregarDadosTela(string formUID)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

                DBDataSource DbOINV = oForm.DataSources.DBDataSources.Item("OINV");

                CarregaDadosAlocacao(DbOINV.GetValue("U_IdPCI", 0));

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados na tela de Edição de NF: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        internal static bool Interface_FormDataEvent(BusinessObjectInfo businessObjectInfo)
        {
            try
            {
                if (businessObjectInfo.EventType == BoEventTypes.et_DATASOURCE_LOAD && !businessObjectInfo.BeforeAction)
                {
                    CarregarDadosTela(businessObjectInfo.FormUID);
                }


                return true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao executar métodos de dados no documento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }
        }

        
    }

}