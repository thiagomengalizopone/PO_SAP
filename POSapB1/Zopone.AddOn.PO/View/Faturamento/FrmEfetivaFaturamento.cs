using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;


namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmEfetivaFaturamento : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }
        EditText EdPO { get; set; }
        DataTable DtPesquisa { get; set; }
        Matrix MtPedidos { get; set; }

        Button BtPesquisar { get; set; }

        Button BtEnviarFaturamento { get; set; }

        public FrmEfetivaFaturamento() : base()
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
                if (!Util.RetornarDialogo("Deseja efetivar o pré faturamento dos documentos?"))
                    return;

                MtPedidos.FlushToDataSource();

                string MensagemErro = string.Empty;
                
                for (int iRow = 0; iRow < DtPesquisa.Rows.Count; iRow++)
                {
                    if (DtPesquisa.GetValue("Selecionar", iRow).ToString() == "Y")
                    {
                        MensagemErro += EfetivarDocumentoFaturamento(iRow);                     
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

        private string EfetivarDocumentoFaturamento(int iRow)
        {
            try
            {

                Int32 DocEntry = Convert.ToInt32(DtPesquisa.GetValue("Esboco", iRow));

                SAPbobsCOM.Documents oEsbocoNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
               
                if (oEsbocoNotaFiscalSaida.GetByKey(DocEntry))
                {
                    if (oEsbocoNotaFiscalSaida.SaveDraftToDocument() != 0)
                        return $"Erro ao Faturar PO {oEsbocoNotaFiscalSaida.NumAtCard} Linha {iRow+1} -  {Globals.Master.Connection.Database.GetLastErrorDescription()} \n";


                    oEsbocoNotaFiscalSaida.GetByKey(DocEntry);

                    if (oEsbocoNotaFiscalSaida.DocumentStatus == BoStatus.bost_Open)
                    {
                        if (oEsbocoNotaFiscalSaida.Close() != 0)
                            return $"Erro ao Fechar Esboço pré faturamento: {oEsbocoNotaFiscalSaida.DocEntry} Linha {iRow + 1} -  {Globals.Master.Connection.Database.GetLastErrorDescription()} \n";
                    }
                }




                Util.ExibirMensagemStatusBar($"PO faturada com sucesso - {oEsbocoNotaFiscalSaida.NumAtCard}!");
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

                string SQL_Query = $@"ZPN_SP_EfetivaPedidosPreFaturamento '{dataInicial}', '{dataFinal}', '{EdPO.Value}'";

                DtPesquisa.ExecuteQuery(SQL_Query);


                MtPedidos.Columns.Item("Col_9").DataBind.Bind("DtPO", "Selecionar");
                MtPedidos.Columns.Item("Col_0").DataBind.Bind("DtPO", "Pedido");
                MtPedidos.Columns.Item("Col_1").DataBind.Bind("DtPO", "PO");
                
                MtPedidos.Columns.Item("Col_3").DataBind.Bind("DtPO", "Linha");
                MtPedidos.Columns.Item("Col_2").DataBind.Bind("DtPO", "Item");
                MtPedidos.Columns.Item("Col_5").DataBind.Bind("DtPO", "Valor");
                MtPedidos.Columns.Item("Col_6").DataBind.Bind("DtPO", "Esboco");
                MtPedidos.Columns.Item("Col_7").DataBind.Bind("DtPO", "NF");
                MtPedidos.Columns.Item("Col_10").DataBind.Bind("DtPO", "ItemCode");
                MtPedidos.Columns.Item("Col_11").DataBind.Bind("DtPO", "Dscription");

                MtPedidos.Columns.Item("Col_12").DataBind.Bind("DtPO", "Status");


                MtPedidos.Columns.Item("Col_14").DataBind.Bind("DtPO", "SaldoFaturado");
                MtPedidos.Columns.Item("Col_13").DataBind.Bind("DtPO", "SaldoAberto");
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
