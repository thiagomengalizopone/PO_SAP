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
       
        public Button BtAdicionarLinha { get; set; }
        public Button BtRemoverLinha { get; set; }

        DBDataSource dbOINV { get; set; }
        DBDataSource dbINV1 { get; set; }

        public FrmNotaFiscal() : base()
        {
            if (oForm == null)
                return;

            DtAlocacao = oForm.DataSources.DataTables.Item("DtAloc");
            dbOINV = oForm.DataSources.DBDataSources.Item("OINV");
            dbINV1 = oForm.DataSources.DBDataSources.Item("INV1");

            oMtItens = (Matrix)oForm.Items.Item("MtItem").Specific;
            oMtAlocacao = (Matrix)oForm.Items.Item("MtAloca").Specific;

            BtAdicionarLinha = (Button)oForm.Items.Item("BtAdd").Specific;
            BtAdicionarLinha.PressedAfter += BtAdicionarLinha_PressedAfter;
            
            BtRemoverLinha = (Button)oForm.Items.Item("BtDel").Specific;
            BtRemoverLinha.PressedAfter += BtRemoverLinha_PressedAfter;

            oMtItens.AutoResizeColumns();
            oMtAlocacao.AutoResizeColumns();
            oMtAlocacao.ChooseFromListBefore += OMtAlocacao_ChooseFromListBefore;
            oMtAlocacao.ChooseFromListAfter += OMtAlocacao_ChooseFromListAfter;
            oMtAlocacao.LostFocusAfter += OMtAlocacao_LostFocusAfter;

            CarregaDadosAlocacao("-9999", oMtAlocacao, DtAlocacao);
        }

        private void OMtAlocacao_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Percent")
                    CalcularValorParcela(pVal.Row - 1);


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar dados de Alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }

        }

        private void OMtAlocacao_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                var oConds = new SAPbouiCOM.Conditions();
                var oCfLs = oForm.ChooseFromLists;

                var cfl = oCfLs.Item("CFL_Aloc");

                if (cfl.GetConditions().Count > 0)
                {
                    SAPbouiCOM.Conditions emptyCon = new SAPbouiCOM.Conditions();

                    cfl.SetConditions(emptyCon);
                }

                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                string sql_query = $"SP_ZPN_LISTAALOCACOESOBRAFAT '{dbOINV.GetValue("Project", 0).ToString()}'";
                oRecordSet.DoQuery(sql_query);

                int iRow = 1;

                while (!oRecordSet.EoF)
                {
                    var oCond = oConds.Add();

                    if (oConds.Count == 1)
                        oCond.BracketOpenNum = 1;


                    oCond.Alias = "Code";
                    oCond.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCond.CondVal = oRecordSet.Fields.Item("Codigo").Value.ToString();

                    if (oRecordSet.RecordCount > 1 && iRow != oRecordSet.RecordCount)
                        oCond.Relationship = BoConditionRelationship.cr_OR;

                    if (iRow == oRecordSet.RecordCount)
                        oCond.BracketCloseNum = 1;

                    oRecordSet.MoveNext();

                    iRow++;
                }

                cfl.SetConditions(oConds);

                BubbleEvent = true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao filtrar dados de alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                BubbleEvent = false;
            }
            

        }

        private void OMtAlocacao_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "CodAloc" && pVal.ActionSuccess)
                {

                    oMtAlocacao.FlushToDataSource();

                    Int32 row = pVal.Row - 1;

                    SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                    if (aEvent.SelectedObjects == null)
                        return;

                    string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("Code", 0));
                    string Descricao = Convert.ToString(aEvent.SelectedObjects.GetValue("U_Desc", 0));
                    double dblPercentualFaturamento = Convert.ToDouble(aEvent.SelectedObjects.GetValue("U_Perc", 0));

                    DtAlocacao.SetValue("CodigoAlocacao", row, Code);
                    DtAlocacao.SetValue("DescricaoAlocacao", row, Descricao);

                    if (dblPercentualFaturamento > 0)
                    {
                        DtAlocacao.SetValue("Percentual", row, dblPercentualFaturamento);
                    }

                    oMtAlocacao.LoadFromDataSourceEx(true);

                    CalcularValorParcela(pVal.Row-1);
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void CalcularValorParcela(int row)
        {
            try
            {
                oMtAlocacao.FlushToDataSource();

                double dblValorLiquido = Convert.ToDouble(dbOINV.GetValue("DocTotal", 0).ToString().Replace(",", "").Replace(".", ","));
                double dblValorBruto = Convert.ToDouble(dbINV1.GetValue("LineTotal", 0).ToString().Replace(",", "").Replace(".", ","));
                double PercentualFaturamento = Convert.ToDouble(DtAlocacao.GetValue("Percentual", row).ToString().Replace(".", "").Replace(",", "."));

                DtAlocacao.SetValue("ValorParcelaBruto", row, dblValorLiquido * PercentualFaturamento/100);
                DtAlocacao.SetValue("ValorParcela", row, dblValorBruto * PercentualFaturamento / 100);

                oMtAlocacao.LoadFromDataSourceEx(true);

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao calcular percentual de faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void BtRemoverLinha_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo("Tem certeza que deseja remover esta linha?"))
                    return;

                Util.MatrixRemoverLinha(oForm, oMtAlocacao, DtAlocacao, true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao remover linha: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void BtAdicionarLinha_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Util.MatrixInserirLinha(oForm, oMtAlocacao, DtAlocacao, true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao adicionar linha: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static void CarregaDadosAlocacao(string IdPciDocumento, Matrix oMtAlocacao, DataTable DtAlocacao)
        {
            try
            {
                string SQL_Alocacao = $"SP_ZPN_ExibeAlocacaoDocumento '{IdPciDocumento}'";

                DtAlocacao.ExecuteQuery(SQL_Alocacao);

                oMtAlocacao.Columns.Item("Parcela").DataBind.Bind("DtAloc", "Parcela");
                oMtAlocacao.Columns.Item("Percent").DataBind.Bind("DtAloc", "Percentual");
                oMtAlocacao.Columns.Item("ValParcB").DataBind.Bind("DtAloc", "ValorParcelaBruto");
                oMtAlocacao.Columns.Item("ValParc").DataBind.Bind("DtAloc", "ValorParcela");
                oMtAlocacao.Columns.Item("CodAloc").DataBind.Bind("DtAloc", "CodigoAlocacao");
                oMtAlocacao.Columns.Item("DescAloc").DataBind.Bind("DtAloc", "DescricaoAlocacao");
                oMtAlocacao.Columns.Item("IdPCI").DataBind.Bind("DtAloc", "IdPCI");

                oMtAlocacao.Columns.Item("CodAloc").ChooseFromListUID = "CFL_Aloc";
                oMtAlocacao.Columns.Item("CodAloc").ChooseFromListAlias = "Code";


                oMtAlocacao.LoadFromDataSourceEx(true);

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
                
                DataTable DtAlocacao = oForm.DataSources.DataTables.Item("DtAloc");                
                Matrix oMtAlocacao = (Matrix)oForm.Items.Item("MtAloca").Specific;

                CarregaDadosAlocacao(DbOINV.GetValue("U_IdPCI", 0), oMtAlocacao, DtAlocacao);

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
                if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD && !businessObjectInfo.BeforeAction)
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