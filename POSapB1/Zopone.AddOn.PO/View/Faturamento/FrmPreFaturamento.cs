using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Drawing;
using Zopone.AddOn.PO.Helpers;


namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmPreFaturamento : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }
        EditText EdPO { get; set; }
        EditText EdCliente { get; set; }
        DataTable DtPesquisa { get; set; }
        Matrix MtPedidos { get; set; }

        Button BtPesquisar { get; set; }

        Button BtEnviarFaturamento { get; set; }

        public FrmPreFaturamento() : base()
        {
            if (oForm == null)
                return;

            EdDataI = (EditText)oForm.Items.Item("EdDataI").Specific;
            EdDataF = (EditText)oForm.Items.Item("EdDataF").Specific;
            EdPO = (EditText)oForm.Items.Item("EdPO").Specific;

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

            MtPedidos.AutoResizeColumns();

            CarregarDadosFaturamentoFaturar();

            oForm.Visible = true;



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
                if (pVal.ColUID == "Col_7")
                {

                    var oConds = new SAPbouiCOM.Conditions();
                    var oCfLs = oForm.ChooseFromLists;

                    var cfl = oCfLs.Item("CFL_ALOC");

                    if (cfl.GetConditions().Count > 0)
                    {
                        SAPbouiCOM.Conditions emptyCon = new SAPbouiCOM.Conditions();

                        cfl.SetConditions(emptyCon);
                    }

                    var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                    string sql_query = $"SP_ZPN_LISTAALOCACOESOBRA '{DtPesquisa.GetValue("Obra", pVal.Row - 1)}'";
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

                if (pVal.ColUID != "Col_15")
                    return;


                MtPedidos.FlushToDataSource();

                double dblSaldoAberto = Convert.ToDouble(DtPesquisa.GetValue("SaldoAberto", pVal.Row - 1));
                double dblValorFaturar = Convert.ToDouble(DtPesquisa.GetValue("TotalFaturar", pVal.Row - 1));

                if (dblValorFaturar > dblSaldoAberto)
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


                Globals.Master.Connection.Database.StartTransaction();

                GerarDocumentoPreFaturamento();

                Util.ExibirMensagemStatusBar("Pré Faturamento gerado com sucesso!");
                if (Globals.Master.Connection.Database.InTransaction)
                    Globals.Master.Connection.Database.EndTransaction(BoWfTransOpt.wf_Commit);

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar pré faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);

                if (Globals.Master.Connection.Database.InTransaction)
                    Globals.Master.Connection.Database.EndTransaction(BoWfTransOpt.wf_RollBack);
            }
            finally
            {
                if (Globals.Master.Connection.Database.InTransaction)
                    Globals.Master.Connection.Database.EndTransaction(BoWfTransOpt.wf_Commit);

                CarregarDadosFaturamentoFaturar();
            }
        }

        private void GerarDocumentoPreFaturamento()
        {
            try
            {
                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                SAPbobsCOM.Documents oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                
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
                            double TotalLinha = Convert.ToDouble(DtPesquisa.GetValue("TotalFaturar", iRow));
                            DateTime dataFaturamento = Convert.ToDateTime(DtPesquisa.GetValue("PrevFat", iRow));


                            string ItemFaturamento = DtPesquisa.GetValue("AlocacaoFAT", iRow).ToString();
                            string DescItemFaturamento = DtPesquisa.GetValue("DescAlocacaoFAT", iRow).ToString();

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

                                oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                                oNotaFiscalSaida.CardCode = string.Empty;
                                oNotaFiscalSaida.Lines.LineTotal = 0;
                                oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value = string.Empty;
                            }


                            oNotaFiscalSaida.DocObjectCodeEx = "13";
                            oNotaFiscalSaida.DocDate = dataFaturamento;                            
                            oNotaFiscalSaida.CardCode = oPedidoVenda.CardCode;
                            oNotaFiscalSaida.NumAtCard = oPedidoVenda.NumAtCard;
                            oNotaFiscalSaida.BPL_IDAssignedToInvoice = oPedidoVenda.BPL_IDAssignedToInvoice;
                            oNotaFiscalSaida.UserFields.Fields.Item("U_IdPO").Value = oPedidoVenda.UserFields.Fields.Item("U_IdPO").Value;

                            oNotaFiscalSaida.UserFields.Fields.Item("U_ZPN_TipoDocto").Value = oPedidoVenda.UserFields.Fields.Item("U_ZPN_TipoDocto").Value;
                            oNotaFiscalSaida.UserFields.Fields.Item("U_NroCont").Value = oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value;

                            oPedidoVenda.Lines.SetCurrentLine(LineNum);


                            Util.ExibirMensagemStatusBar($"Faturando PO {oPedidoVenda.NumAtCard} - Linha -  {oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString()}", BoMessageTime.bmt_Medium, false);

                            oNotaFiscalSaida.Lines.ItemCode = ItemCode;
                            oNotaFiscalSaida.Lines.Quantity = 1;
                            oNotaFiscalSaida.Lines.LineTotal += TotalLinha;
                            oNotaFiscalSaida.Lines.Usage = oPedidoVenda.Lines.Usage;
                            oNotaFiscalSaida.TaxExtension.MainUsage = Convert.ToInt32(oPedidoVenda.Lines.Usage);
                            oNotaFiscalSaida.Lines.TaxCode = "1556-001"; // Temporário até definição do imposto
                            oNotaFiscalSaida.Lines.ProjectCode = oPedidoVenda.Lines.ProjectCode;
                            oNotaFiscalSaida.Lines.COGSCostingCode = oPedidoVenda.Lines.COGSCostingCode;
                            oNotaFiscalSaida.Lines.COGSCostingCode2 = oPedidoVenda.Lines.COGSCostingCode2;
                            oNotaFiscalSaida.Lines.COGSCostingCode3 = oPedidoVenda.Lines.COGSCostingCode3;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Candidato").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value += ($" {oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString()}");


                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_ItemFat").Value =  ItemFaturamento;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_DescItemFat").Value = DescItemFaturamento;
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
                        throw new Exception($"Erro ao faturar PO: {oPedidoVenda.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao gerar pré faturamento: {Ex.Message}");
            }
        }


        private void MtPedidos_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Col_7")
                {
                    MtPedidos.FlushToDataSource();

                    Int32 row = pVal.Row - 1;

                    SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                    if (aEvent.SelectedObjects == null)
                        return;

                    string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("Code", 0));
                    string Descricao = Convert.ToString(aEvent.SelectedObjects.GetValue("U_Desc", 0));


                    DtPesquisa.SetValue("AlocacaoFAT", row, Code);
                    DtPesquisa.SetValue("DescAlocacaoFAT", row, Descricao);

                    string SQL_PESQUISA = string.Empty;

                    SQL_PESQUISA = $"SELECT isnull(max(T0.[U_Perc]),0) FROM [dbo].[@ZPN_ALOCA]  T0 WHERE T0.[Code] = '{Code}'";

                    double dblPercentualFaturamento = Convert.ToDouble(SqlUtils.GetValue(SQL_PESQUISA));

                    if (dblPercentualFaturamento > 0)
                        DtPesquisa.SetValue("PercFaturar", row, dblPercentualFaturamento);

                    MtPedidos.LoadFromDataSourceEx();

                    CalculaPorcentagemFaturamento(row);
                }
                else if (pVal.ColUID == "Col_23")
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
        private void CalculaPorcentagemFaturamento(Int32 row)
        {
            MtPedidos.FlushToDataSource();

            double dblSaldoAberto = Convert.ToDouble(DtPesquisa.GetValue("SaldoAberto", row));
            double dblPercentualFaturamento = Convert.ToDouble(DtPesquisa.GetValue("PercFaturar", row));

            DtPesquisa.SetValue("TotalFaturar", row, (dblSaldoAberto * dblPercentualFaturamento / 100));

            MtPedidos.LoadFromDataSourceEx(true);
        }


        private void MtPedidos_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Col_8")
                    SelecionaAtividadeServico(pVal.Row - 1);
                else if (pVal.ColUID == "Col_21")
                    CalculaPorcentagemFaturamento(pVal.Row - 1);
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
                string dataInicial = !string.IsNullOrEmpty(EdDataI.Value) ? EdDataI.Value : "20200101";
                string dataFinal = !string.IsNullOrEmpty(EdDataF.Value) ? EdDataF.Value : "20500101";

                string SQL_Query = $@"ZPN_SP_ListaPedidosGerarPreFaturamento '{dataInicial}', '{dataFinal}', '{EdPO.Value}', '{EdCliente.Value}'";

                DtPesquisa.ExecuteQuery(SQL_Query);


                MtPedidos.Columns.Item("Col_9").DataBind.Bind("DtPO", "Selecionar");
                MtPedidos.Columns.Item("Col_0").DataBind.Bind("DtPO", "Pedido");
                MtPedidos.Columns.Item("Col_1").DataBind.Bind("DtPO", "PO");
                
                MtPedidos.Columns.Item("Col_22").DataBind.Bind("DtPO", "PrevFat");

                MtPedidos.Columns.Item("Col_2").DataBind.Bind("DtPO", "Item");
                MtPedidos.Columns.Item("Col_8").DataBind.Bind("DtPO", "Atividade");
                MtPedidos.Columns.Item("Col_4").DataBind.Bind("DtPO", "Descricao");
                MtPedidos.Columns.Item("Col_5").DataBind.Bind("DtPO", "Valor");

                MtPedidos.Columns.Item("ItemCode").DataBind.Bind("DtPO", "ItemCode");
                MtPedidos.Columns.Item("Dscription").DataBind.Bind("DtPO", "Dscription");

                MtPedidos.Columns.Item("Col_10").DataBind.Bind("DtPO", "Cliente");
                MtPedidos.Columns.Item("Col_11").DataBind.Bind("DtPO", "CodCliente");
                MtPedidos.Columns.Item("Col_20").DataBind.Bind("DtPO", "CodCliente");

                MtPedidos.Columns.Item("Col_12").DataBind.Bind("DtPO", "Status");

                MtPedidos.Columns.Item("Col_7").DataBind.Bind("DtPO", "AlocacaoFAT");
                MtPedidos.Columns.Item("Col_16").DataBind.Bind("DtPO", "DescAlocacaoFAT");
                MtPedidos.Columns.Item("Col_21").DataBind.Bind("DtPO", "PercFaturar");

                MtPedidos.Columns.Item("Col_14").DataBind.Bind("DtPO", "SaldoFaturado");
                MtPedidos.Columns.Item("Col_13").DataBind.Bind("DtPO", "SaldoAberto");
                MtPedidos.Columns.Item("Col_15").DataBind.Bind("DtPO", "TotalFaturar");

                MtPedidos.Columns.Item("Col_3").DataBind.Bind("DtPO", "Alocacao");
                MtPedidos.Columns.Item("Col_7").DataBind.Bind("DtPO", "AlocacaoFAT");
                MtPedidos.Columns.Item("Col_16").DataBind.Bind("DtPO", "DescAlocacaoFAT");

                MtPedidos.Columns.Item("Col_17").DataBind.Bind("DtPO", "Linha");
                MtPedidos.Columns.Item("Col_18").DataBind.Bind("DtPO", "Obra");

                MtPedidos.Columns.Item("Col_17").DataBind.Bind("DtPO", "Linha");
                MtPedidos.Columns.Item("Col_18").DataBind.Bind("DtPO", "Obra");

                MtPedidos.Columns.Item("Col_23").DataBind.Bind("DtPO", "IbgeCode");
                MtPedidos.Columns.Item("Col_24").DataBind.Bind("DtPO", "Estado");
                MtPedidos.Columns.Item("Col_25").DataBind.Bind("DtPO", "Cidade");

                MtPedidos.LoadFromDataSourceEx();
                MtPedidos.AutoResizeColumns();

                Column oColuna = MtPedidos.Columns.Item("Col_7");
                oColuna.ChooseFromListUID = "CFL_ALOC";
                oColuna.ChooseFromListAlias = "Code";

                 oColuna = MtPedidos.Columns.Item("Col_23");
                oColuna.ChooseFromListUID = "CFL_265";
                oColuna.ChooseFromListAlias = "IbgeCode";


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}



