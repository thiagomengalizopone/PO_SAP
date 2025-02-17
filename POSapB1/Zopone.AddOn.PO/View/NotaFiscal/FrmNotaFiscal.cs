using sap.dev.core;
using sap.dev.core.ApiService_n8n;
using sap.dev.core.DTO;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zopone.AddOn.PO.Helpers;
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

        public EditText EdTotalAlocacapPercentual { get; set; }
        public EditText EdTotalAlocacaoLiquido { get; set; }
        public EditText EdTotalAlocacaoBruto { get; set; }

        public EditText EdNroDocumento { get; set; }
        public EditText EdNroNotaFiscal { get; set; }
        public EditText EdContrato { get; set; }

        DBDataSource dbOINV { get; set; }
        DBDataSource dbINV1 { get; set; }

        public FrmNotaFiscal() : base()
        {
            if (oForm == null)
                return;


            EdTotalAlocacapPercentual = (EditText)oForm.Items.Item("EdTotAlocP").Specific;
            EdTotalAlocacaoLiquido = (EditText)oForm.Items.Item("EdTotAlocL").Specific;
            EdTotalAlocacaoBruto = (EditText)oForm.Items.Item("EdTotAlocB").Specific;

            EdContrato = (EditText)oForm.Items.Item("EdCont").Specific;

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

            EdNroDocumento = (EditText)oForm.Items.Item("EdDocEntry").Specific;
            EdNroDocumento.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            EdNroDocumento.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

            EdNroNotaFiscal = (EditText)oForm.Items.Item("EdNroNF").Specific;
            EdNroNotaFiscal.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            EdNroNotaFiscal.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

            CarregaDadosAlocacao(-9999, oMtAlocacao, DtAlocacao, oForm.UniqueID);
        }

        private void OMtAlocacao_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Percent")
                    CalcularValorParcela(pVal.Row - 1);
                else if (pVal.ColUID == "ValParcB")
                    CalculaValorPercentual(pVal.Row - 1);


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar dados de Alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }

        }

        private void CalculaValorPercentual(int row)
        {
            try
            {
                oForm.Freeze(true);

                oMtAlocacao.FlushToDataSource();

                double dblValorLiquidoTotal = Convert.ToDouble(dbOINV.GetValue("DocTotal", 0).ToString().Replace(",", "").Replace(".", ","));
                double dblValorBrutoTotal = Convert.ToDouble(dbINV1.GetValue("LineTotal", 0).ToString().Replace(",", "").Replace(".", ","));
                double ValorBrutoLinha = Convert.ToDouble(DtAlocacao.GetValue("ValorParcelaBruto", row));

                double Percentual = ValorBrutoLinha / dblValorBrutoTotal;

                DtAlocacao.SetValue("Percentual", row, Percentual * 100);
                DtAlocacao.SetValue("ValorParcela", row, dblValorLiquidoTotal * Percentual);

                oMtAlocacao.LoadFromDataSourceEx(true);

                SomaValoresAlocacao(oForm.UniqueID);

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao calcular valor de faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private static void SomaValoresAlocacao(string formUID)
        {
            Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

            try
            {
                oForm.Freeze(true);

                DataTable DtAlocacao = oForm.DataSources.DataTables.Item("DtAloc");

                EditText EdTotalAlocacapPercentual = (EditText)oForm.Items.Item("EdTotAlocP").Specific;
                EditText EdTotalAlocacaoLiquido = (EditText)oForm.Items.Item("EdTotAlocL").Specific;
                EditText EdTotalAlocacaoBruto = (EditText)oForm.Items.Item("EdTotAlocB").Specific;

                double TotalAlocacaoPercentual = 0;
                double TotalAlocacaoLiquid = 0;
                double TotalAlocacaoBruto = 0;

                for (int iRow = 0; iRow < DtAlocacao.Rows.Count; iRow++)
                {
                    TotalAlocacaoPercentual += Convert.ToDouble(DtAlocacao.GetValue("Percentual", iRow));
                    TotalAlocacaoLiquid += Convert.ToDouble(DtAlocacao.GetValue("ValorParcela", iRow));
                    TotalAlocacaoBruto += Convert.ToDouble(DtAlocacao.GetValue("ValorParcelaBruto", iRow));
                }

                EdTotalAlocacapPercentual.Value = Math.Round(TotalAlocacaoPercentual, 2).ToString().Replace(".", "").Replace(",", ".");
                EdTotalAlocacaoLiquido.Value = Math.Round(TotalAlocacaoLiquid, 2).ToString().Replace(".", "").Replace(",", ".");
                EdTotalAlocacaoBruto.Value = Math.Round(TotalAlocacaoBruto, 2).ToString().Replace(".", "").Replace(",", ".");

                if (TotalAlocacaoPercentual != 100 && TotalAlocacaoLiquid > 0)
                    Util.ExibirMensagemStatusBar($"Atenção: Total Percentual diferente de 100%", BoMessageTime.bmt_Medium, true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao Somar Valores Alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
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

                    CalcularValorParcela(pVal.Row - 1);
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
                oForm.Freeze(true);

                oMtAlocacao.FlushToDataSource();

                double dblValorLiquido = Convert.ToDouble(dbOINV.GetValue("DocTotal", 0).ToString().Replace(",", "").Replace(".", ","));
                double dblValorBruto = Convert.ToDouble(dbINV1.GetValue("LineTotal", 0).ToString().Replace(",", "").Replace(".", ","));
                double PercentualFaturamento = Convert.ToDouble(DtAlocacao.GetValue("Percentual", row).ToString().Replace(".", "").Replace(",", "."));

                DtAlocacao.SetValue("ValorParcelaBruto", row, dblValorBruto * PercentualFaturamento / 100);
                DtAlocacao.SetValue("ValorParcela", row, dblValorLiquido * PercentualFaturamento / 100);

                oMtAlocacao.LoadFromDataSourceEx(true);

                SomaValoresAlocacao(oForm.UniqueID);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao calcular percentual de faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
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

        private static void CarregaDadosAlocacao(Int32 docEntry, Matrix oMtAlocacao, DataTable DtAlocacao, string formUID)
        {
            try
            {
                string SQL_Alocacao = $"SP_ZPN_ExibeAlocacaoDocumento '{docEntry}'";

                DtAlocacao.Rows.Clear();

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

                SomaValoresAlocacao(formUID);
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
                EditText oEdContrato = (EditText)oForm.Items.Item("EdCont").Specific;

                DBDataSource DbOINV = oForm.DataSources.DBDataSources.Item("OINV");

                if (DbOINV.GetValue("CANCELED", 0).ToString() == "N")
                    oForm.Title = "Editar Nota Fiscal de Saída";
                else if (DbOINV.GetValue("CANCELED", 0).ToString() == "Y")
                    oForm.Title = "Editar Nota Fiscal de Saída - CANCELADA";
                else if (DbOINV.GetValue("CANCELED", 0).ToString() == "C")
                    oForm.Title = "Editar Nota Fiscal de Saída - CANCELAMENTO";

                oEdContrato.Value = SqlUtils.GetValue($@"select U_DescContrato from ""@ZPN_OPRJ"" WHERE ""Code"" = '{DbOINV.GetValue("Project", 0).ToString()}'");


                DataTable DtAlocacao = oForm.DataSources.DataTables.Item("DtAloc");
                Matrix oMtAlocacao = (Matrix)oForm.Items.Item("MtAloca").Specific;

                VerificaAlocacaoParcela(Convert.ToInt32(DbOINV.GetValue("DocEntry", 0)));

                CarregaDadosAlocacao(Convert.ToInt32(DbOINV.GetValue("DocEntry", 0)), oMtAlocacao, DtAlocacao, oForm.UniqueID);


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados na tela de Edição de NF: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static void VerificaAlocacaoParcela(Int32 DocEntry)
        {
            try
            {
                bool verificaAloca = SqlUtils.ExistemRegistros($@"
                                                SELECT 
	                                                1
                                                FROM 
	                                                ZPN_ALOCACAOPARCELANF 
                                                WHERE 
	                                                ""DocEntry"" = {DocEntry}");

                if (!verificaAloca)
                {
                    SqlUtils.DoNonQuery($@"SP_ZPN_CRIATABELAALOCACAO {DocEntry}");
                }


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao verificar alocação de parcela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
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
                else if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE && businessObjectInfo.BeforeAction)
                {
                    if (!Util.RetornarDialogo("Deseja atualizar a Nota Fiscal?"))
                        return false;
                }
                else if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE && !businessObjectInfo.BeforeAction)
                {
                    AtualizaAlocacaoDocumento(businessObjectInfo.FormUID);
                    EnviarDadosPCI(businessObjectInfo.FormUID);
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

        private static void EnviarDadosPCI(string formUID)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);
                DBDataSource dbOINV = oForm.DataSources.DBDataSources.Item("OINV");
                Int32 DocEntry = Convert.ToInt32(dbOINV.GetValue("DocEntry", 0));

                UtilPCI.EnviarDadosNFTransmitida(DocEntry);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao enviar dados PCI!");
            }
        }

        private static void AtualizaAlocacaoDocumento(string formUID)
        {
            try
            {

                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

                DataTable DtAlocacao = oForm.DataSources.DataTables.Item("DtAloc");
                Matrix oMtAlocacao = (Matrix)oForm.Items.Item("MtAloca").Specific;
                DBDataSource dbOINV = oForm.DataSources.DBDataSources.Item("OINV");

                string IdPCIDocumento = dbOINV.GetValue("U_IdPCI", 0);
                string DocEntry = dbOINV.GetValue("DocEntry", 0);


                oMtAlocacao.FlushToDataSource();

                SqlUtils.DoNonQuery($"EXEC SP_ZPN_RemoveContasReceberPCI {DocEntry}");

                for (int iRow = 0; iRow < DtAlocacao.Rows.Count; iRow++)
                {
                    if (
                        !string.IsNullOrEmpty(DtAlocacao.GetValue("CodigoAlocacao", iRow).ToString()) &&

                        double.TryParse(Convert.ToString(DtAlocacao.GetValue("Percentual", iRow)), out double dblPercentual) &&
                        double.TryParse(Convert.ToString(DtAlocacao.GetValue("ValorParcela", iRow)), out double dblValorParcela) &&
                        double.TryParse(Convert.ToString(DtAlocacao.GetValue("ValorParcelaBruto", iRow)), out double dblValorParcelaBruto))
                    {
                        string CodAlocacao = DtAlocacao.GetValue("CodigoAlocacao", iRow).ToString();
                        string DescricaoAlocacao = DtAlocacao.GetValue("DescricaoAlocacao", iRow).ToString();
                        string IdPCI = DtAlocacao.GetValue("IdPCI", iRow).ToString();

                        string sqlInsereAtualizaDoc = $"SP_ZPN_InsereAtualizaDocumentoAlocacao {DocEntry}, 13, 'N', {DocEntry},  {dblPercentual.ToString().Replace(".", "").Replace(",", ".")}, {dblValorParcelaBruto.ToString().Replace(".", "").Replace(",", ".")}, '{CodAlocacao}', '{DescricaoAlocacao}', '{IdPCI}', '{IdPCIDocumento}' ";

                        SqlUtils.DoNonQuery(sqlInsereAtualizaDoc);
                    }
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao executar atualizar de dados no documento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }


}