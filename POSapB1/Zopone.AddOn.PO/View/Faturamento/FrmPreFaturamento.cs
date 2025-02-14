using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using Zopone.AddOn.PO.Helpers;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.UtilAddOn;
using static Zopone.AddOn.PO.Model.SqlProcedures;

namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmPreFaturamento : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }
        EditText EdDataIVencimento { get; set; }
        EditText EdDataFVencimento { get; set; }
        EditText EdPO { get; set; }
        EditText EdCliente { get; set; }
        DataTable DtPesquisa { get; set; }
        Matrix MtPedidos { get; set; }

        Button BtPesquisar { get; set; }

        Button BtEnviarFaturamento { get; set; }

        private Dictionary<string, (string, string, string, string)> colMapping = new Dictionary<string, (string, string, string, string)>
            {
                { "CodAlc1", ("AlocacaoFAT1", "DescAlocacaoFAT1", "PercFaturar1", "ValorFat1") },
                { "CodAlc2", ("AlocacaoFAT2", "DescAlocacaoFAT2", "PercFaturar2", "ValorFat2") },
                { "CodAlc3", ("AlocacaoFAT3", "DescAlocacaoFAT3", "PercFaturar3", "ValorFat3") },
                { "CodAlc4", ("AlocacaoFAT4", "DescAlocacaoFAT4", "PercFaturar4", "ValorFat4") }
            };



        public FrmPreFaturamento() : base()
        {
            if (oForm == null)
                return;

            EdDataI = (EditText)oForm.Items.Item("EdDataI").Specific;
            EdDataF = (EditText)oForm.Items.Item("EdDataF").Specific;
            EdPO = (EditText)oForm.Items.Item("EdPO").Specific;

            EdDataIVencimento = (EditText)oForm.Items.Item("EdDataIV").Specific;
            EdDataFVencimento = (EditText)oForm.Items.Item("EdDataFV").Specific;

            EdCliente = (EditText)oForm.Items.Item("EdCliente").Specific;

            MtPedidos = (Matrix)oForm.Items.Item("MtPed").Specific;

            DtPesquisa = oForm.DataSources.DataTables.Item("DtPO");

            BtPesquisar = (Button)oForm.Items.Item("BtPesq").Specific;
            BtPesquisar.PressedAfter += BtPesquisar_PressedAfter;

            BtEnviarFaturamento = (Button)oForm.Items.Item("BtEnv").Specific;
            BtEnviarFaturamento.PressedAfter += BtPreFaturamento_PressedAfter;

            MtPedidos.LostFocusAfter += MtPedidos_LostFocusAfter;
            MtPedidos.ValidateBefore += MtPedidos_ValidateBefore;

            MtPedidos.ChooseFromListBefore += MtPedidos_ChooseFromListBefore;
            MtPedidos.ChooseFromListAfter += MtPedidos_ChooseFromListAfter;

            MtPedidos.PressedAfter += MtPedidos_PressedAfter;
            MtPedidos.GotFocusAfter += MtPedidos_GotFocusAfter;

            MtPedidos.AutoResizeColumns();

            CarregarDadosFaturamentoFaturar();

            oForm.Visible = true;
        }

        private void MtPedidos_GotFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            SelectedRows(pVal.Row);
        }

        private void MtPedidos_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            SelectedRows(pVal.Row);
        }

        private void SelectedRows(int row)
        {
            try
            {
                if (row > 0)
                    MtPedidos.SelectRow(row, true, false);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Linha: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private Conditions CriaCondicoesCidade(string Estado)
        {
            if (string.IsNullOrEmpty(Estado))
                throw new Exception("Selecione o Estado!");

            var oConds = new SAPbouiCOM.Conditions();

            var oCond = oConds.Add();

            oCond.Alias = "State";

            oCond.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;

            oCond.CondVal = Estado;

            return oConds;
        }

        private void MtPedidos_ChooseFromListBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                if (pVal.ColUID == "CodAlc1" ||
                    pVal.ColUID == "CodAlc2" ||
                    pVal.ColUID == "CodAlc3" ||
                    pVal.ColUID == "CodAlc4")
                {
                    string cflName = string.Empty;

                    if (pVal.ColUID == "CodAlc1")
                        cflName = "CFL_ALOC1";
                    else if (pVal.ColUID == "CodAlc2")
                        cflName = "CFL_ALOC2";
                    else if (pVal.ColUID == "CodAlc3")
                        cflName = "CFL_ALOC3";
                    else if (pVal.ColUID == "CodAlc4")
                        cflName = "CFL_ALOC4";

                    var oConds = new SAPbouiCOM.Conditions();
                    var oCfLs = oForm.ChooseFromLists;

                    var cfl = oCfLs.Item(cflName);

                    if (cfl.GetConditions().Count > 0)
                    {
                        SAPbouiCOM.Conditions emptyCon = new SAPbouiCOM.Conditions();

                        cfl.SetConditions(emptyCon);
                    }

                    var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                    string sql_query = $"SP_ZPN_LISTAALOCACOESOBRAFAT '{DtPesquisa.GetValue("Obra", pVal.Row - 1)}'";
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
                }

                BubbleEvent = true;
            }
            catch (Exception Ex)
            {
                BubbleEvent = false;
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void MtPedidos_ValidateBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                BubbleEvent = true;

                if (pVal.ColUID != "ColPO5")
                    return;


                MtPedidos.FlushToDataSource();

                double dblSaldoAberto = Convert.ToDouble(DtPesquisa.GetValue("SaldoAberto", pVal.Row - 1));
                double dblValorFat = Convert.ToDouble(DtPesquisa.GetValue("TotalFaturar", pVal.Row - 1));

                if (dblValorFat > dblSaldoAberto)
                {
                    Util.ExibirMensagemStatusBar("Valor a faturar não pode ser maior que valor em aberto!", BoMessageTime.bmt_Medium, true);
                    BubbleEvent = false;
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar dados coluna: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                BubbleEvent = false;
            }
        }

        private void BtPreFaturamento_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo("Deseja gerar o pré faturamento dos documentos?"))
                    return;

                MtPedidos.FlushToDataSource();

                string MensagemErro = string.Empty;



                GerarDocumentoPreFaturamento();

                Util.ExibirMensagemStatusBar("Pré Faturamento gerado com sucesso!");
                if (Globals.Master.Connection.Database.InTransaction)
                    Globals.Master.Connection.Database.EndTransaction(BoWfTransOpt.wf_Commit);

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar pré faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);


            }
            finally
            {
                CarregarDadosFaturamentoFaturar();
            }
        }

        private void GerarDocumentoPreFaturamento()
        {
            try
            {
                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                SAPbobsCOM.Documents oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                string dataFaturamentoPO = DateTime.Now.ToString("yyyyMMdd HHmmss");

                string AlocacaoFAT1 = string.Empty;
                string AlocacaoFAT2 = string.Empty;
                string AlocacaoFAT3 = string.Empty;
                string AlocacaoFAT4 = string.Empty;

                string DescAlocacaoFAT1 = string.Empty;
                string DescAlocacaoFAT2 = string.Empty;
                string DescAlocacaoFAT3 = string.Empty;
                string DescAlocacaoFAT4 = string.Empty;

                double TotalFat1 = 0;
                double TotalFat2 = 0;
                double TotalFat3 = 0;
                double TotalFat4 = 0;

                double PercFat1 = 0;
                double PercFat2 = 0;
                double PercFat3 = 0;
                double PercFat4 = 0;

                string IdPCI = SqlUtils.GetValue("select newid()");

                oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                Int32 PedidoVendaDocEntry = 0;
                string Obra = string.Empty;
                string ItemCodeFaturamento = string.Empty;

                for (int iRow = 0; iRow < DtPesquisa.Rows.Count; iRow++)
                {
                    if (DtPesquisa.GetValue("Selecionar", iRow).ToString() == "Y")
                    {
                        if (string.IsNullOrEmpty(DtPesquisa.GetValue("Atividade", iRow).ToString()))
                        {
                            throw new Exception($"Não há atividade selecionada para a linha {iRow + 1}");
                        }
                        else
                        {
                            Int32 DocEntry = Convert.ToInt32(DtPesquisa.GetValue("Pedido", iRow));
                            Int32 LineNum = Convert.ToInt32(DtPesquisa.GetValue("Linha", iRow));
                            string ItemCode = DtPesquisa.GetValue("ItemCode", iRow).ToString();
                            string Atividade = DtPesquisa.GetValue("Atividade", iRow).ToString();
                            string IbgeCode = DtPesquisa.GetValue("IbgeCode", iRow).ToString();                           

                            DateTime dataFaturamento = Convert.ToDateTime(DtPesquisa.GetValue("PrevFat", iRow));

                            oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                            if (!oPedidoVenda.GetByKey(DocEntry))
                                throw new Exception($"Pedido de venda (PO) não encontrado: {DocEntry}");

                            if (string.IsNullOrEmpty(Obra))
                            {
                                Obra = oPedidoVenda.Lines.ProjectCode;
                                PedidoVendaDocEntry = oPedidoVenda.DocEntry;
                                ItemCodeFaturamento = ItemCode;
                            }

                            if (Obra != oPedidoVenda.Lines.ProjectCode || oPedidoVenda.DocEntry != PedidoVendaDocEntry || ItemCode != ItemCodeFaturamento)
                            {
                                if (oNotaFiscalSaida.Add() != 0)
                                    throw new Exception($"Erro ao faturar PO: {oPedidoVenda.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                                Int32 NewDocEntry = Convert.ToInt32(SqlUtils.GetValue("SELECT MAX(DocEntry) FROM ODRF WHERE ObjType = '13'"));

                                AddInstallments(
                                    AlocacaoFAT1, PercFat1, DescAlocacaoFAT1,
                                    AlocacaoFAT2, PercFat2, DescAlocacaoFAT2,
                                    AlocacaoFAT3, PercFat3, DescAlocacaoFAT3,
                                    AlocacaoFAT4, PercFat4, DescAlocacaoFAT4,
                                    DocEntry);


                                AtualizaDocumentoCidadeImposto(NewDocEntry);

                                dataFaturamentoPO = DateTime.Now.ToString("yyyyMMdd HHmmss");
                                oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                                oNotaFiscalSaida.CardCode = string.Empty;
                                oNotaFiscalSaida.Lines.LineTotal = 0;
                                oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value = string.Empty;
                                oNotaFiscalSaida.NumberOfInstallments = 0;
                                IdPCI = SqlUtils.GetValue("select newid()");
                            }

                            oNotaFiscalSaida.DiscountPercent = 0;

                            oNotaFiscalSaida.SequenceCode = 29;
                            oNotaFiscalSaida.SequenceModel = "46";

                            oNotaFiscalSaida.DocObjectCodeEx = "13";
                            oNotaFiscalSaida.DocDate = dataFaturamento;
                            oNotaFiscalSaida.CardCode = oPedidoVenda.CardCode;
                            oNotaFiscalSaida.NumAtCard = oPedidoVenda.NumAtCard;
                            oNotaFiscalSaida.BPL_IDAssignedToInvoice = oPedidoVenda.BPL_IDAssignedToInvoice;
                            oNotaFiscalSaida.UserFields.Fields.Item("U_IdPO").Value = oPedidoVenda.UserFields.Fields.Item("U_IdPO").Value;
                            oNotaFiscalSaida.UserFields.Fields.Item("U_TX_OrigemIbge").Value = IbgeCode;
                            oNotaFiscalSaida.Project = oPedidoVenda.Lines.ProjectCode;

                            AlocacaoFAT1 = DtPesquisa.GetValue("AlocacaoFAT1", iRow).ToString();
                            AlocacaoFAT2 = DtPesquisa.GetValue("AlocacaoFAT2", iRow).ToString();
                            AlocacaoFAT3 = DtPesquisa.GetValue("AlocacaoFAT3", iRow).ToString();
                            AlocacaoFAT4 = DtPesquisa.GetValue("AlocacaoFAT4", iRow).ToString();

                            DescAlocacaoFAT1 = DtPesquisa.GetValue("DescAlocacaoFAT1", iRow).ToString();
                            DescAlocacaoFAT2 = DtPesquisa.GetValue("DescAlocacaoFAT2", iRow).ToString();
                            DescAlocacaoFAT3 = DtPesquisa.GetValue("DescAlocacaoFAT3", iRow).ToString();
                            DescAlocacaoFAT4 = DtPesquisa.GetValue("DescAlocacaoFAT4", iRow).ToString();

                            TotalFat1 = Convert.ToDouble(DtPesquisa.GetValue("ValorFat1", iRow));
                            TotalFat2 = Convert.ToDouble(DtPesquisa.GetValue("ValorFat2", iRow));
                            TotalFat3 = Convert.ToDouble(DtPesquisa.GetValue("ValorFat3", iRow));
                            TotalFat4 = Convert.ToDouble(DtPesquisa.GetValue("ValorFat4", iRow));

                            PercFat1 = Convert.ToDouble(DtPesquisa.GetValue("PercFaturar1", iRow));
                            PercFat2 = Convert.ToDouble(DtPesquisa.GetValue("PercFaturar2", iRow));
                            PercFat3 = Convert.ToDouble(DtPesquisa.GetValue("PercFaturar3", iRow));
                            PercFat4 = Convert.ToDouble(DtPesquisa.GetValue("PercFaturar4", iRow));

                            if (PercFat1 + PercFat2 + PercFat3 + PercFat4 != 100)
                            {
                                throw new Exception($"Percentual de faturamento maior que 100 na linha {iRow + 1}!");
                            }



                            oNotaFiscalSaida.UserFields.Fields.Item("U_ZPN_TipoDocto").Value = oPedidoVenda.UserFields.Fields.Item("U_ZPN_TipoDocto").Value;
                            oNotaFiscalSaida.UserFields.Fields.Item("U_NroCont").Value = oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value;

                            for (int iRowPedido = 0; iRowPedido < oPedidoVenda.Lines.Count; iRowPedido++)
                            {
                                oPedidoVenda.Lines.SetCurrentLine(iRowPedido);

                                if (oPedidoVenda.Lines.LineNum == LineNum)
                                    break;
                            }

                            //oNotaFiscalSaida.NumberOfInstallments += 1;

                            oNotaFiscalSaida.UserFields.Fields.Item("U_IdPCI").Value = IdPCI;


                            Util.ExibirMensagemStatusBar($"Faturando PO {oPedidoVenda.NumAtCard} - Linha -  {oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString()}", BoMessageTime.bmt_Medium, false);

                            oNotaFiscalSaida.TaxExtension.MainUsage = Convert.ToInt32(ConfiguracoesImportacaoPO.Utilizacao);


                            oNotaFiscalSaida.Lines.ItemCode = ItemCode;
                            oNotaFiscalSaida.Lines.Quantity = 1;
                            oNotaFiscalSaida.Lines.LineTotal += (TotalFat1 + TotalFat2 + TotalFat3 + TotalFat4);

                            oNotaFiscalSaida.Lines.Usage = ConfiguracoesImportacaoPO.Utilizacao;
                            oNotaFiscalSaida.TaxExtension.MainUsage = Convert.ToInt32(oPedidoVenda.Lines.Usage);
                            oNotaFiscalSaida.Lines.ProjectCode = oPedidoVenda.Lines.ProjectCode;
                            oNotaFiscalSaida.Lines.COGSCostingCode = oPedidoVenda.Lines.COGSCostingCode;
                            oNotaFiscalSaida.Lines.COGSCostingCode2 = oPedidoVenda.Lines.COGSCostingCode2;
                            oNotaFiscalSaida.Lines.COGSCostingCode3 = oPedidoVenda.Lines.COGSCostingCode3;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Candidato").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value += ($" {oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString()}");

                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Project").Value = oPedidoVenda.Lines.ProjectCode;


                            //oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_ItemFat").Value = ItemFaturamento;
                            //oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_DescItemFat").Value = DescItemFaturamento;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Atividade").Value = Atividade;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_StatusFat").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_StatusFat").Value;

                            oNotaFiscalSaida.DocumentReferences.ReferencedDocEntry = oPedidoVenda.DocEntry;
                            oNotaFiscalSaida.DocumentReferences.ReferencedObjectType = ReferencedObjectTypeEnum.rot_SalesOrder;
                            oNotaFiscalSaida.DocumentReferences.Remark += ($" {oPedidoVenda.Lines.LineNum.ToString()} ");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(oNotaFiscalSaida.CardCode))
                {
                    if (oNotaFiscalSaida.Add() != 0)
                    {
                        SqlUtils.DoNonQuery("DELETE FROM ZPN_FATURADOCUMENTOPO WHERE DataHoraFaturamento = ''");
                        throw new Exception($"Erro ao faturar PO: {oPedidoVenda.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }
                    Int32 DocEntry = Convert.ToInt32(SqlUtils.GetValue("SELECT MAX(DocEntry) FROM ODRF WHERE ObjType = '13'"));

                    AddInstallments(
                            AlocacaoFAT1, PercFat1, DescAlocacaoFAT1,
                            AlocacaoFAT2, PercFat2, DescAlocacaoFAT2,
                            AlocacaoFAT3, PercFat3, DescAlocacaoFAT3,
                            AlocacaoFAT4, PercFat4, DescAlocacaoFAT4,
                            DocEntry);

                    AtualizaDocumentoCidadeImposto(DocEntry);

                }
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao gerar pré faturamento: {Ex.Message}");
            }
        }

        public void AddInstallments(
            string AlocacaoFAT1, double PercFat1, string DescAlocacaoFAT1,
            string AlocacaoFAT2, double PercFat2, string DescAlocacaoFAT2,
            string AlocacaoFAT3, double PercFat3, string DescAlocacaoFAT3,
            string AlocacaoFAT4, double PercFat4, string DescAlocacaoFAT4,
            Int32 DocEntry)
        {

            Documents oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            Int32 NumberOfInstallments = 0;

            if (oNotaFiscalSaida.GetByKey(DocEntry))
            {
                oNotaFiscalSaida.Installments.SetCurrentLine(0);

                if (!string.IsNullOrEmpty(AlocacaoFAT1) && PercFat1 > 0)
                {
                    NumberOfInstallments += 1;
                    oNotaFiscalSaida.Installments.Percentage = PercFat1;
                    oNotaFiscalSaida.Installments.DueDate = oNotaFiscalSaida.DocDueDate;

                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_ItemFat").Value = AlocacaoFAT1;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_DescItemFat").Value = DescAlocacaoFAT1;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_Project").Value = oNotaFiscalSaida.Project;
                    oNotaFiscalSaida.Installments.Add();
                }

                if (!string.IsNullOrEmpty(AlocacaoFAT2) && PercFat2 > 0)
                {
                    NumberOfInstallments += 1;
                    oNotaFiscalSaida.Installments.Percentage = PercFat2;
                    oNotaFiscalSaida.Installments.DueDate = oNotaFiscalSaida.DocDueDate;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_ItemFat").Value = AlocacaoFAT2;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_DescItemFat").Value = DescAlocacaoFAT2;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_Project").Value = oNotaFiscalSaida.Project;
                    oNotaFiscalSaida.Installments.Add();
                }

                if (!string.IsNullOrEmpty(AlocacaoFAT3) && PercFat3 > 0)
                {
                    NumberOfInstallments += 1;
                    oNotaFiscalSaida.Installments.Percentage = PercFat3;
                    oNotaFiscalSaida.Installments.DueDate = oNotaFiscalSaida.DocDueDate;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_ItemFat").Value = AlocacaoFAT3;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_DescItemFat").Value = DescAlocacaoFAT3;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_Project").Value = oNotaFiscalSaida.Project;
                    oNotaFiscalSaida.Installments.Add();
                }

                if (!string.IsNullOrEmpty(AlocacaoFAT4) && PercFat4 > 0)
                {
                    NumberOfInstallments += 1;
                    oNotaFiscalSaida.Installments.Percentage = PercFat4;
                    oNotaFiscalSaida.Installments.DueDate = oNotaFiscalSaida.DocDueDate;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_ItemFat").Value = AlocacaoFAT4;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_DescItemFat").Value = DescAlocacaoFAT4;
                    oNotaFiscalSaida.Installments.UserFields.Fields.Item("U_Project").Value = oNotaFiscalSaida.Project;
                    oNotaFiscalSaida.Installments.Add();
                }

                oNotaFiscalSaida.NumberOfInstallments = NumberOfInstallments;

                if (oNotaFiscalSaida.Update() != 0)
                    throw new Exception($"Erro ao atualizar alocações no esboço {DocEntry}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");

            }
        }


        private void GeraDocumentoAlocacao(int docEntry, string IdPCIDocumento, string alocacaoFAT1, string descAlocacaoFAT1, double totalFat1, double percFat1, string alocacaoFAT2, string descAlocacaoFAT2, double totalFat2, double percFat2, string alocacaoFAT3, string descAlocacaoFAT3, double totalFat3, double percFat3, string alocacaoFAT4, string descAlocacaoFAT4, double totalFat4, double percFat4)
        {
            try
            {
                // Criação de uma lista de alocações e descrições
                var alocacoes = new List<(double total, double perc, string alocacao, string descAlocacao)>
                        {
                            (totalFat1, percFat1, alocacaoFAT1, descAlocacaoFAT1),
                            (totalFat2, percFat2, alocacaoFAT2, descAlocacaoFAT2),
                            (totalFat3, percFat3, alocacaoFAT3, descAlocacaoFAT3),
                            (totalFat4, percFat4, alocacaoFAT4, descAlocacaoFAT4)
                        };

                // Itera sobre cada tupla e executa o comando apenas se total > 0
                foreach (var (total, perc, alocacao, descAlocacao) in alocacoes)
                {
                    if (total > 0)
                    {
                        string sqlInsereAtualizaDoc = $"SP_ZPN_InsereAtualizaDocumentoAlocacao null, 13, 'E', {docEntry},  {perc.ToString().Replace(".", "").Replace(",", ".")}, {total.ToString().Replace(".", "").Replace(",", ".")}, '{alocacao}', '{descAlocacao}', null, '{IdPCIDocumento}' ";

                        SqlUtils.DoNonQuery(sqlInsereAtualizaDoc);
                    }
                }

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar alocações de documento no esboço {docEntry} - {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void AtualizaDocumentoCidadeImposto(Int32 DocEntry)
        {
            try
            {

                Documents oNotaFiscalSaidaImposto = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                SqlUtils.DoNonQuery($"SP_ZPN_ATUALIZAPROJETONF {DocEntry}");

                if (oNotaFiscalSaidaImposto.GetByKey(DocEntry))
                {
                    string IssCode = SqlUtils.GetValue($"SELECT U_ISS FROM OCNT WHERE IbgeCode = '{oNotaFiscalSaidaImposto.UserFields.Fields.Item("U_TX_OrigemIbge").Value}'");

                    if (!string.IsNullOrEmpty(IssCode))
                    {
                        if (!string.IsNullOrEmpty(oNotaFiscalSaidaImposto.Lines.WithholdingTaxLines.WTCode))
                            oNotaFiscalSaidaImposto.Lines.WithholdingTaxLines.Add();

                        oNotaFiscalSaidaImposto.Lines.WithholdingTaxLines.WTCode = IssCode;

                        oNotaFiscalSaidaImposto.DiscountPercent = 0;

                        if (oNotaFiscalSaidaImposto.Update() != 0)
                            throw new Exception($"Erro ao Atualizar NF Faturamento: {oNotaFiscalSaidaImposto.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }

                    oNotaFiscalSaidaImposto.GetByKey(DocEntry);
                    oNotaFiscalSaidaImposto.DiscountPercent = 0;

                    if (oNotaFiscalSaidaImposto.Update() != 0)
                        throw new Exception($"Erro ao Atualizar NF Faturamento: {oNotaFiscalSaidaImposto.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                    SqlUtils.DoNonQuery($"exec SP_ZPN_CriaObservacoesFaturamentoEsboco {DocEntry}");


                }

                UtilPCI.EnviarDadosNFDigitacaoPCIAsync(DocEntry);

            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro atualizar documento na cidade da NF: {Ex.Message}");
            }
        }

        private void MtPedidos_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (colMapping.ContainsKey(pVal.ColUID))
                {
                    var (CampoAlocacaoFAT, DescAlocacaoFAT, PercFaturar, ValorFat) = colMapping[pVal.ColUID];

                    MtPedidos.FlushToDataSource();

                    Int32 row = pVal.Row - 1;

                    SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                    if (aEvent.SelectedObjects == null)
                        return;

                    string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("Code", 0));
                    string Descricao = Convert.ToString(aEvent.SelectedObjects.GetValue("U_Desc", 0));
                    double dblPercentualFaturamento = Convert.ToDouble(aEvent.SelectedObjects.GetValue("U_Perc", 0));

                    DtPesquisa.SetValue(CampoAlocacaoFAT, row, Code);
                    DtPesquisa.SetValue(DescAlocacaoFAT, row, Descricao);

                    if (dblPercentualFaturamento > 0)
                    {
                        DtPesquisa.SetValue(PercFaturar, row, dblPercentualFaturamento);
                    }

                    MtPedidos.LoadFromDataSourceEx();

                }
                else if (pVal.ColUID == "Col_25")
                {
                    MtPedidos.FlushToDataSource();

                    Int32 row = pVal.Row - 1;

                    SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                    if (aEvent.SelectedObjects == null)
                        return;

                    string IbgeCode = Convert.ToString(aEvent.SelectedObjects.GetValue("IbgeCode", 0));
                    string State = Convert.ToString(aEvent.SelectedObjects.GetValue("State", 0));
                    string Name = Convert.ToString(aEvent.SelectedObjects.GetValue("Name", 0));

                    DtPesquisa.SetValue("IbgeCode", row, IbgeCode);
                    DtPesquisa.SetValue("Estado", row, State);
                    DtPesquisa.SetValue("Cidade", row, Name);

                    MtPedidos.LoadFromDataSourceEx();
                }

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar alocação faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
        private void CalculaPorcentagemFaturamento(Int32 row, string ColUID)
        {
            try
            {
                oForm.Freeze(true);

                MtPedidos.FlushToDataSource();

                if (ColUID.StartsWith("PercAlc"))
                {
                    ColUID = ColUID.Replace("PercAlc", "CodAlc");
                }


                var colunaReferenciaMap = new Dictionary<string, string>
                    {
                        { "CodAlc1", "PercAlc1"  },
                        { "CodAlc2", "PercAlc2"  },
                        { "CodAlc3", "PercAlc3"  },
                        { "CodAlc4", "PercAlc4"  }
                    };

                if (!colunaReferenciaMap.ContainsKey(ColUID))
                {
                    return;
                }

                string colunaReferencia = colunaReferenciaMap[ColUID];

                if (!colMapping.ContainsKey(ColUID))
                {
                    return;
                }

                var (CampoAlocacaoFAT, DescAlocacaoFAT, PercFaturar, ValorFat) = colMapping[ColUID];

                if (!double.TryParse(Convert.ToString(DtPesquisa.GetValue("SaldoAberto", row)), out double dblSaldoAberto) ||
                    !double.TryParse(Convert.ToString(DtPesquisa.GetValue(PercFaturar, row)), out double dblPercentualFaturamento))
                {
                    return;
                }

                DtPesquisa.SetValue(ValorFat, row, (dblSaldoAberto * dblPercentualFaturamento / 100));

                MtPedidos.LoadFromDataSourceEx(true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao calcular valor da alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }

        }


        private void MtPedidos_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Col_8")
                    SelecionaAtividadeServico(pVal.Row - 1);
                else if (pVal.ColUID == "PercAlc1" ||
                         pVal.ColUID == "PercAlc2" ||
                         pVal.ColUID == "PercAlc3" ||
                         pVal.ColUID == "PercAlc4")
                    CalculaPorcentagemFaturamento(pVal.Row - 1, pVal.ColUID);
                else if (colMapping.ContainsKey(pVal.ColUID))
                    CalculaPorcentagemFaturamento(pVal.Row - 1, pVal.ColUID);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar atividade: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void SelecionaAtividadeServico(int row)
        {
            try
            {
                MtPedidos.FlushToDataSource();

                string Atividade = DtPesquisa.GetValue("Atividade", row).ToString();

                if (string.IsNullOrEmpty(Atividade))
                    return;

                string SQL = $"ZPN_SP_SelecionaItemAtividade '{Atividade}'";

                System.Data.DataTable dtRegistrosItens = SqlUtils.ExecuteCommand(SQL);

                if (dtRegistrosItens.Rows.Count == 0)
                {
                    DtPesquisa.SetValue("Selecionar", row, "N");
                    DtPesquisa.SetValue("ItemCode", row, string.Empty);
                    DtPesquisa.SetValue("Dscription", row, string.Empty);

                    Util.ExibeMensagensDialogoStatusBar($"Não há serviço cadastrado com o código {Atividade}");
                    return;
                }

                DtPesquisa.SetValue("Selecionar", row, "Y");
                DtPesquisa.SetValue("ItemCode", row, dtRegistrosItens.Rows[0]["ItemCode"].ToString());
                DtPesquisa.SetValue("Dscription", row, dtRegistrosItens.Rows[0]["ItemName"].ToString());

                MtPedidos.LoadFromDataSourceEx();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar tipo serviço: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void BtPesquisar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                CarregarDadosFaturamentoFaturar();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao Pesquisar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }


        private void CarregarDadosFaturamentoFaturar()
        {
            try
            {
                // Definindo as datas padrão
                string dataInicial = GetDataValor(EdDataI.Value, "20200101");
                string dataFinal = GetDataValor(EdDataF.Value, "20500101");
                string dataInicialVencimento = GetDataValor(EdDataIVencimento.Value, "20200101");
                string dataFinalVencimento = GetDataValor(EdDataFVencimento.Value, "20500101");

                // Construindo a consulta SQL
                string SQL_Query = $@"ZPN_SP_ListarPedidosGerarPreFaturamento '{dataInicial}', '{dataFinal}', '{dataInicialVencimento}', '{dataFinalVencimento}', '{EdPO.Value}', '{EdCliente.Value}'";

                // Executando a consulta
                DtPesquisa.ExecuteQuery(SQL_Query);

                // Mapeamento das colunas
                MapearColunas();

                MtPedidos.LoadFromDataSourceEx();
                MtPedidos.AutoResizeColumns();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        // Método para definir o valor de data com fallback
        private string GetDataValor(string data, string valorDefault)
        {
            return !string.IsNullOrEmpty(data) ? data : valorDefault;
        }

        private void MapearColunas()
        {
            var colunas = new (string ColunaMtPedidos, string ColunaDtPO, string ChooseFromListUID, string ChooseFromListAlias)[]
            {
                ("Selec", "Selecionar", null, null),
                ("ColPed", "Pedido", null, null),
                ("ColPO", "PO", null, null),
                ("Col_22", "PrevFat", null, null),
                ("Col_2", "Item", null, null),
                ("Col_8", "Atividade", null, null),
                ("Col_4", "Descricao", null, null),
                ("Col_5", "Valor", null, null),
                ("ItemCode", "ItemCode", null, null),
                ("Dscription", "Dscription", null, null),
                ("Col_11", "Cliente", null, null),
                ("Col_10", "CodCliente", null, null),
                ("Col_12", "Status", null, null),
                ("CodAlc1", "AlocacaoFAT1", "CFL_ALOC1", "Code"),
                ("Col_16", "DescAlocacaoFAT1", null, null),
                ("PercAlc1", "PercFaturar1", null, null),
                ("ValorFat1", "ValorFat1", null, null),
                ("CodAlc2", "AlocacaoFAT2", "CFL_ALOC2", "Code"),
                ("Col_30", "DescAlocacaoFAT2", null, null),
                ("PercAlc2", "PercFaturar2", null, null),
                ("ValorFat2", "ValorFat2", null, null),
                ("CodAlc3", "AlocacaoFAT3", "CFL_ALOC3", "Code"),
                ("Col_33", "DescAlocacaoFAT3", null, null),
                ("PercAlc3", "PercFaturar3", null, null),
                ("ValorFat3", "ValorFat3", null, null),
                ("CodAlc4", "AlocacaoFAT4", "CFL_ALOC4", "Code"),
                ("Col_37", "DescAlocacaoFAT4", null, null),
                ("PercAlc4", "PercFaturar4", null, null),
                ("ValorFat4", "ValorFat4", null, null),
                ("Col_14", "SaldoFaturado", null, null),
                ("Col_13", "SaldoAberto", null, null),
                ("Col_15", "TotalFaturar", null, null),
                ("Col_3", "Alocacao", null, null),
                ("Col_17", "Linha", null, null),
                ("Col_18", "Obra", null, null),
                ("Col_20", "Contrato", null, null),
                ("Col_23", "IbgeCode", null, null),
                ("Col_24", "Estado", null, null),
                ("Col_25", "Cidade", "CFL_265", "Name"),
                ("Col_26", "CidadeObra", null, null)
            };

            // Bindando as colunas
            foreach (var coluna in colunas)
            {
                try
                {
                    MtPedidos.Columns.Item(coluna.ColunaMtPedidos).DataBind.Bind("DtPO", coluna.ColunaDtPO);
                    if (!string.IsNullOrEmpty(coluna.ChooseFromListUID))
                    {

                        var oColuna = MtPedidos.Columns.Item(coluna.ColunaMtPedidos);
                        oColuna.ChooseFromListUID = coluna.ChooseFromListUID;
                        oColuna.ChooseFromListAlias = coluna.ChooseFromListAlias;

                    }
                }
                catch (Exception Ex)
                {

                }
            }
        }

    }
}



