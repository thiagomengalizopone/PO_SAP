using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
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
                { "CodAlc1", ("AlocacaoFAT1", "DescAlocacaoFAT1", "PercFaturar1", "ValorFaturar1") },
                { "CodAlc2", ("AlocacaoFAT2", "DescAlocacaoFAT2", "PercFaturar2", "ValorFaturar2") },
                { "CodAlc3", ("AlocacaoFAT3", "DescAlocacaoFAT3", "PercFaturar3", "ValorFaturar3") },
                { "CodAlc4", ("AlocacaoFAT4", "DescAlocacaoFAT4", "PercFaturar4", "ValorFaturar4") }
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
                if (pVal.ColUID == "CodAlc1" ||
                    pVal.ColUID == "CodAlc2" ||
                    pVal.ColUID == "CodAlc2" ||
                    pVal.ColUID == "CodAlc4")
                {
                    string cflName = string.Empty;

                    if (pVal.ColUID == "CodAlc1")
                        cflName = "CFL_ALOC1";
                    else if (pVal.ColUID == "CodAlc2")
                        cflName = "CFL_ALOC2";
                    else if (pVal.ColUID == "CodAlc2")
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

                                AtualizaDocumentoCidadeImposto();

                                dataFaturamentoPO = DateTime.Now.ToString("yyyyMMdd HHmmss");
                                oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                                oNotaFiscalSaida.CardCode = string.Empty;
                                oNotaFiscalSaida.Lines.LineTotal = 0;
                                oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value = string.Empty;
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

                            oNotaFiscalSaida.UserFields.Fields.Item("U_ZPN_TipoDocto").Value = oPedidoVenda.UserFields.Fields.Item("U_ZPN_TipoDocto").Value;
                            oNotaFiscalSaida.UserFields.Fields.Item("U_NroCont").Value = oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value;

                            for (int iRowPedido = 0; iRowPedido < oPedidoVenda.Lines.Count; iRowPedido++)
                            {
                                oPedidoVenda.Lines.SetCurrentLine(iRowPedido);

                                if (oPedidoVenda.Lines.LineNum == LineNum)
                                    break;
                            }



                            Util.ExibirMensagemStatusBar($"Faturando PO {oPedidoVenda.NumAtCard} - Linha -  {oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString()}", BoMessageTime.bmt_Medium, false);

                            oNotaFiscalSaida.TaxExtension.MainUsage = Convert.ToInt32(ConfiguracoesImportacaoPO.Utilizacao);

                            oNotaFiscalSaida.Project = oPedidoVenda.Lines.ProjectCode;

                            oNotaFiscalSaida.Lines.ItemCode = ItemCode;
                            oNotaFiscalSaida.Lines.Quantity = 1;
                            oNotaFiscalSaida.Lines.LineTotal += TotalLinha;

                            oNotaFiscalSaida.Lines.Usage = ConfiguracoesImportacaoPO.Utilizacao;
                            oNotaFiscalSaida.TaxExtension.MainUsage = Convert.ToInt32(oPedidoVenda.Lines.Usage);
                            oNotaFiscalSaida.Lines.ProjectCode = oPedidoVenda.Lines.ProjectCode;
                            oNotaFiscalSaida.Lines.COGSCostingCode = oPedidoVenda.Lines.COGSCostingCode;
                            oNotaFiscalSaida.Lines.COGSCostingCode2 = oPedidoVenda.Lines.COGSCostingCode2;
                            oNotaFiscalSaida.Lines.COGSCostingCode3 = oPedidoVenda.Lines.COGSCostingCode3;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Candidato").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value;
                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value += ($" {oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString()}");

                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Project").Value = oPedidoVenda.Lines.ProjectCode;


                            oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_ItemFat").Value = ItemFaturamento;
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
                    {
                        SqlUtils.DoNonQuery("DELETE FROM ZPN_FATURADOCUMENTOPO WHERE DataHoraFaturamento = ''");
                        throw new Exception($"Erro ao faturar PO: {oPedidoVenda.NumAtCard}: {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }

                    AtualizaDocumentoCidadeImposto();

                }
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao gerar pré faturamento: {Ex.Message}");
            }
        }

        private void AtualizaDocumentoCidadeImposto()
        {
            try
            {
                Int32 DocEntry = Convert.ToInt32(SqlUtils.GetValue("SELECT MAX(DocEntry) FROM ODRF WHERE ObjType = '13'"));

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
                    var (CampoAlocacaoFAT, DescAlocacaoFAT, PercFaturar, ValorFaturar) = colMapping[pVal.ColUID];

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

                    CalculaPorcentagemFaturamento(row, pVal.ColUID);
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
            MtPedidos.FlushToDataSource();

            var colunaReferenciaMap = new Dictionary<string, string>
                    {
                        { "PercAlc1", "CodAlc1" },
                        { "PercAlc2", "CodAlc2" },
                        { "PercAlc3", "CodAlc3" }, 
                        { "PercAlc4", "CodAlc4" }
                    };

            if (!colunaReferenciaMap.ContainsKey(ColUID))
            {
                return;
            }

            string colunaReferencia = colunaReferenciaMap[ColUID];

            if (!colMapping.ContainsKey(colunaReferencia))
            {
                return;
            }

            var (CampoAlocacaoFAT, DescAlocacaoFAT, PercFaturar, ValorFaturar) = colMapping[colunaReferencia];

            if (!double.TryParse(Convert.ToString(DtPesquisa.GetValue("SaldoAberto", row)), out double dblSaldoAberto) ||
                !double.TryParse(Convert.ToString(DtPesquisa.GetValue(PercFaturar, row)), out double dblPercentualFaturamento))
            {
                return;
            }

            DtPesquisa.SetValue(ValorFaturar, row, (dblSaldoAberto * dblPercentualFaturamento / 100));

            MtPedidos.LoadFromDataSourceEx(true);
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
                string SQL_Query = $@"ZPN_SP_ListaPedidosGerarPreFaturamento '{dataInicial}', '{dataFinal}', '{dataInicialVencimento}', '{dataFinalVencimento}', '{EdPO.Value}', '{EdCliente.Value}'";

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
                ("Col_27", "ValorFaturar1", null, null),
                ("CodAlc2", "AlocacaoFAT2", "CFL_ALOC2", "Code"),
                ("Col_30", "DescAlocacaoFAT2", null, null),
                ("PercAlc2", "PercFaturar2", null, null),
                ("Col_31", "ValorFaturar2", null, null),
                ("CodAlc3", "AlocacaoFAT3", "CFL_ALOC3", "Code"),
                ("Col_33", "DescAlocacaoFAT3", null, null),
                ("PercAlc3", "PercFaturar3", null, null),
                ("Col_35", "ValorFaturar3", null, null),
                ("CodAlc4", "AlocacaoFAT4", "CFL_ALOC4", "Code"),
                ("Col_37", "DescAlocacaoFAT4", null, null),
                ("PercAlc4", "PercFaturar4", null, null),
                ("Col_39", "ValorFaturar4", null, null),
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



