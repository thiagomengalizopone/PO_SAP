using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;


namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmPreFaturamento : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }
        EditText EdPO { get; set; }
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

            MtPedidos = (Matrix)oForm.Items.Item("MtPed").Specific;

            DtPesquisa = oForm.DataSources.DataTables.Item("DtPO");

            BtPesquisar = (Button)oForm.Items.Item("BtPesq").Specific;
            BtPesquisar.PressedAfter += BtPesquisar_PressedAfter;


            BtEnviarFaturamento = (Button)oForm.Items.Item("BtEnv").Specific;
            BtEnviarFaturamento.PressedAfter += BtPreFaturamento_PressedAfter;

            MtPedidos.LostFocusAfter += MtPedidos_LostFocusAfter;
            MtPedidos.ValidateBefore += MtPedidos_ValidateBefore;

            MtPedidos.AutoResizeColumns();

            oForm.Visible = true;



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
                
                for (int iRow = 0; iRow < DtPesquisa.Rows.Count; iRow++)
                {
                    if (DtPesquisa.GetValue("Selecionar", iRow).ToString() == "Y")
                    {
                        MensagemErro += GerarDocumentoPreFaturamento(iRow);                     
                    }
                }

                if (!string.IsNullOrEmpty(MensagemErro))
                {
                    Util.ExibeMensagensDialogoStatusBar($"Há pedido(s) não faturado(s) \n: {MensagemErro}", BoMessageTime.bmt_Medium, true);
                }
                else
                {
                    Util.ExibirMensagemStatusBar("Pré Faturamento gerado com sucesso!");
                }
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

        private string GerarDocumentoPreFaturamento(int iRow)
        {
            try
            {

                Int32 DocEntry = Convert.ToInt32(DtPesquisa.GetValue("Pedido", iRow));
                Int32 LineNum = Convert.ToInt32(DtPesquisa.GetValue("Linha", iRow));
                string ItemCode = DtPesquisa.GetValue("ItemCode", iRow).ToString();
                double TotalLinha = Convert.ToDouble(DtPesquisa.GetValue("TotalFaturar", iRow));

                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                SAPbobsCOM.Documents oNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                if (oPedidoVenda.GetByKey(DocEntry))
                {
                    oPedidoVenda.Lines.SetCurrentLine(LineNum);

                    oNotaFiscalSaida.DocObjectCodeEx = "13";
                    oNotaFiscalSaida.CardCode = oPedidoVenda.CardCode;
                    oNotaFiscalSaida.NumAtCard = oPedidoVenda.NumAtCard;

                    oNotaFiscalSaida.BPL_IDAssignedToInvoice = oPedidoVenda.BPL_IDAssignedToInvoice;
                    oNotaFiscalSaida.UserFields.Fields.Item("U_IdPO").Value             = oPedidoVenda.UserFields.Fields.Item("U_IdPO").Value;
                    oNotaFiscalSaida.UserFields.Fields.Item("U_IdPCI").Value            = oPedidoVenda.UserFields.Fields.Item("U_IdPCI").Value;
                    oNotaFiscalSaida.UserFields.Fields.Item("U_ZPN_TipoDocto").Value    = oPedidoVenda.UserFields.Fields.Item("U_ZPN_TipoDocto").Value;
                    oNotaFiscalSaida.UserFields.Fields.Item("U_NroCont").Value          = oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value;

                    oPedidoVenda.Lines.SetCurrentLine(LineNum);

                    oNotaFiscalSaida.Lines.ItemCode = ItemCode;
                    oNotaFiscalSaida.Lines.Quantity = 1;
                    oNotaFiscalSaida.Lines.LineTotal = TotalLinha;
                    oNotaFiscalSaida.Lines.Usage = oPedidoVenda.Lines.Usage;
                    oNotaFiscalSaida.Lines.ProjectCode = oPedidoVenda.Lines.ProjectCode;
                    oNotaFiscalSaida.Lines.COGSCostingCode = oPedidoVenda.Lines.COGSCostingCode;
                    oNotaFiscalSaida.Lines.COGSCostingCode2 = oPedidoVenda.Lines.COGSCostingCode2;
                    oNotaFiscalSaida.Lines.COGSCostingCode3 = oPedidoVenda.Lines.COGSCostingCode3;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Candidato").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Item").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_ItemFat").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_ItemFat").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_DescItemFat").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_DescItemFat").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Parcela").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Parcela").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Tipo").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Tipo").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_itemDescription").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_itemDescription").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_manSiteInfo").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_manSiteInfo").Value;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_Atividade").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_Atividade").Value;

                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_BaseEntry").Value = oPedidoVenda.DocEntry;
                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_BaseLine").Value = oPedidoVenda.Lines.LineNum;

                    oNotaFiscalSaida.Lines.UserFields.Fields.Item("U_manSiteInfo").Value = oPedidoVenda.Lines.UserFields.Fields.Item("U_manSiteInfo").Value;


                    oNotaFiscalSaida.DocumentReferences.ReferencedDocEntry = oPedidoVenda.DocEntry;
                    oNotaFiscalSaida.DocumentReferences.ReferencedObjectType = ReferencedObjectTypeEnum.rot_SalesOrder;
                    oNotaFiscalSaida.DocumentReferences.ReferencedAmount = TotalLinha;
                    



                    if (oNotaFiscalSaida.Add() != 0)
                        return $"Erro ao gerar PO {oPedidoVenda.NumAtCard} Linha {LineNum} -  {Globals.Master.Connection.Database.GetLastErrorDescription()} \n";

                }

                Util.ExibirMensagemStatusBar($"Pré Faturamento gerado com sucesso - {oPedidoVenda.NumAtCard} Linha {LineNum}!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar pré faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            

            return string.Empty;
        }


        private void MtPedidos_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID == "Col_8")
                    SelecionaAtividadeServico(pVal.Row-1);
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
                DtPesquisa.SetValue("ItemCode", row, dtRegistrosItens.Rows[0]["ItemCode"].ToString()   );
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

                string SQL_Query = $@"ZPN_SP_ListaPedidosGerarPreFaturamento '{dataInicial}', '{dataFinal}', '{EdPO.Value}'";

                DtPesquisa.ExecuteQuery(SQL_Query);


                MtPedidos.Columns.Item("Col_9").DataBind.Bind("DtPO", "Selecionar");
                MtPedidos.Columns.Item("Col_0").DataBind.Bind("DtPO", "Pedido");
                MtPedidos.Columns.Item("Col_1").DataBind.Bind("DtPO", "PO");
                
                MtPedidos.Columns.Item("Col_3").DataBind.Bind("DtPO", "Linha");
                MtPedidos.Columns.Item("Col_2").DataBind.Bind("DtPO", "Item");
                MtPedidos.Columns.Item("Col_8").DataBind.Bind("DtPO", "Atividade");
                MtPedidos.Columns.Item("Col_4").DataBind.Bind("DtPO", "Descricao");
                MtPedidos.Columns.Item("Col_5").DataBind.Bind("DtPO", "Valor");
                MtPedidos.Columns.Item("Col_6").DataBind.Bind("DtPO", "Esboco");
                MtPedidos.Columns.Item("Col_7").DataBind.Bind("DtPO", "NF");
                MtPedidos.Columns.Item("Col_10").DataBind.Bind("DtPO", "ItemCode");
                MtPedidos.Columns.Item("Col_11").DataBind.Bind("DtPO", "Dscription");

                MtPedidos.Columns.Item("Col_12").DataBind.Bind("DtPO", "Status");


                MtPedidos.Columns.Item("Col_14").DataBind.Bind("DtPO", "SaldoFaturado");
                MtPedidos.Columns.Item("Col_13").DataBind.Bind("DtPO", "SaldoAberto");
                MtPedidos.Columns.Item("Col_15").DataBind.Bind("DtPO", "TotalFaturar");
                MtPedidos.Columns.Item("Col_16").DataBind.Bind("DtPO", "TotalDocumento");                
                MtPedidos.LoadFromDataSourceEx();
                MtPedidos.AutoResizeColumns();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
