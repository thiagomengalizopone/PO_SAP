using sap.dev.core;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;


namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmListaFaturamento : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }
        EditText EdPO { get; set; }
        ComboBox CbStatus { get; set; }
        DataTable DtPesquisa { get; set; }
        Matrix MtPedidos { get; set; }

        Button BtPesquisar { get; set; }
        public FrmListaFaturamento() : base()
        {
            if (oForm == null)
                return;

            EdDataI = (EditText)oForm.Items.Item("EdDataI").Specific;
            EdDataF = (EditText)oForm.Items.Item("EdDataF").Specific;
            EdPO = (EditText)oForm.Items.Item("EdPO").Specific;
            CbStatus = (ComboBox)oForm.Items.Item("CbStatus").Specific;

            MtPedidos = (Matrix)oForm.Items.Item("MtPed").Specific;

            DtPesquisa = oForm.DataSources.DataTables.Item("DtPO");

            BtPesquisar = (Button)oForm.Items.Item("BtPesq").Specific;
            BtPesquisar.PressedAfter += BtPesquisar_PressedAfter;

            MtPedidos.AutoResizeColumns();

            oForm.Visible = true;



        }

        private void BtPesquisar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                CarregarDadosFaturamento();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao Pesquisar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void CarregarDadosFaturamento()
        {
            try
            {
                if (string.IsNullOrEmpty(CbStatus.Value))
                {
                    Util.ExibeMensagensDialogoStatusBar("Obrigatório selecionar o status!");

                    return;
                }

                string dataInicial = !string.IsNullOrEmpty(EdDataI.Value) ? EdDataI.Value : "20200101";
                string dataFinal = !string.IsNullOrEmpty(EdDataF.Value) ? EdDataF.Value : "20500101";

                string SQL_Query = $@"ZPN_SP_ListaPedidosFaturamento '{dataInicial}', '{dataFinal}', '{EdPO.Value}', '{CbStatus.Value}'";

                DtPesquisa.ExecuteQuery(SQL_Query);


                MtPedidos.Columns.Item("Col_9").DataBind.Bind("DtPO", "Selecionar");
                MtPedidos.Columns.Item("Col_0").DataBind.Bind("DtPO", "Pedido");
                MtPedidos.Columns.Item("Col_1").DataBind.Bind("DtPO", "PO");
                MtPedidos.Columns.Item("Col_2").DataBind.Bind("DtPO", "Linha");
                MtPedidos.Columns.Item("Col_3").DataBind.Bind("DtPO", "Item");
                MtPedidos.Columns.Item("Col_8").DataBind.Bind("DtPO", "Atividade");
                MtPedidos.Columns.Item("Col_4").DataBind.Bind("DtPO", "Descricao");
                MtPedidos.Columns.Item("Col_5").DataBind.Bind("DtPO", "Valor");
                MtPedidos.Columns.Item("Col_6").DataBind.Bind("DtPO", "Esboco");
                MtPedidos.Columns.Item("Col_7").DataBind.Bind("DtPO", "NF");



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
