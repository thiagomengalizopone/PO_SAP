using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.Financeiro
{
    public class FrmContasReceber : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }
        ComboBox CbTpDt { get; set; }
        EditText EdObra { get; set; }
        EditText EdPO { get; set; }
        EditText EdCardCode { get; set; }
        EditText EdCardName { get; set; }
        EditText EdCont { get; set; }
        EditText EdFat { get; set; }

        EditText EdTotPag { get; set; }
        EditText EdTotBrt { get; set; }
        EditText EdTotLiq { get; set; }
        EditText EdCofins { get; set; }
        EditText EdCSLL { get; set; }
        EditText EdIRRF { get; set; }
        EditText EdPis { get; set; }
        EditText EdINSS { get; set; }
        EditText EdISS { get; set; }

        EditText EdTotPagG { get; set; }
        EditText EdTotBrtG { get; set; }
        EditText EdTotLiqG { get; set; }
        EditText EdCofinsG { get; set; }
        EditText EdCSLLG { get; set; }
        EditText EdIRRFG { get; set; }
        EditText EdPisG { get; set; }
        EditText EdINSSG { get; set; }
        EditText EdISSG { get; set; }

        Button BtFiltrar { get ; set; }

        Matrix MtDadosCR { get; set; }

        DataTable DtDadosCR { get; set; }

        List<Int32> LinhasSelecionadas = new List<int>();

        Button BtEnviarFaturamento { get; set; }

        public FrmContasReceber() : base()
        {
            if (oForm == null)
                return;

            MtDadosCR = (Matrix)oForm.Items.Item("MtPed").Specific;

            DtDadosCR = oForm.DataSources.DataTables.Item("DtItm");

            EdTotPag = (EditText)oForm.Items.Item("EdTotPag").Specific;
            EdTotBrt = (EditText)oForm.Items.Item("EdTotBrt").Specific;
            EdTotLiq = (EditText)oForm.Items.Item("EdTotLiq").Specific;
            EdCofins = (EditText)oForm.Items.Item("EdCofins").Specific;
            EdCSLL = (EditText)oForm.Items.Item("EdCsll").Specific;
            EdIRRF = (EditText)oForm.Items.Item("EdIrrf").Specific;
            EdPis = (EditText)oForm.Items.Item("EdPis").Specific;
            EdINSS = (EditText)oForm.Items.Item("EdInss").Specific;
            EdISS = (EditText)oForm.Items.Item("EdIss").Specific;

            EdTotPagG = (EditText)oForm.Items.Item("EdTotPagG").Specific;
            EdTotBrtG = (EditText)oForm.Items.Item("EdTotBrtG").Specific;
            EdTotLiqG = (EditText)oForm.Items.Item("EdTotLiqG").Specific;
            EdCofinsG = (EditText)oForm.Items.Item("EdCofinsG").Specific;
            EdCSLLG = (EditText)oForm.Items.Item("EdCsllG").Specific;
            EdIRRFG = (EditText)oForm.Items.Item("EdIrrfG").Specific;
            EdPisG = (EditText)oForm.Items.Item("EdPisG").Specific;
            EdINSSG = (EditText)oForm.Items.Item("EdInssG").Specific;
            EdISSG = (EditText)oForm.Items.Item("EdIssG").Specific;

            EdDataI = (EditText)oForm.Items.Item("EdDataI").Specific;
            EdDataF = (EditText)oForm.Items.Item("EdDataF").Specific;
            CbTpDt = (ComboBox)oForm.Items.Item("CbTpDt").Specific;
            EdObra = (EditText)oForm.Items.Item("EdObra").Specific;
            EdPO = (EditText)oForm.Items.Item("EdPO").Specific;
            EdCardCode = (EditText)oForm.Items.Item("EdCardCode").Specific;
            EdCardName = (EditText)oForm.Items.Item("EdCardName").Specific;
            EdCont = (EditText)oForm.Items.Item("EdCont").Specific;
            EdFat = (EditText)oForm.Items.Item("EdFat").Specific;

            BtFiltrar = (Button)oForm.Items.Item("BtPesq").Specific;
            BtFiltrar.PressedAfter += BtFiltrar_PressedAfter;

            oForm.Visible = true;

            LinhasSelecionadas = new List<int>();

            PesquisarDadosPagamento();
        }

        private void BtFiltrar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            PesquisarDadosPagamento();
        }

        private void PesquisarDadosPagamento()
        {
            try
            {
                string dataInicial = !string.IsNullOrEmpty(EdDataI.Value) ? EdDataI.Value : "20200101";
                string dataFinal = !string.IsNullOrEmpty(EdDataF.Value) ? EdDataF.Value : "20500101";
                string tipoData = !string.IsNullOrEmpty(CbTpDt.Value) ? CbTpDt.Value : "V";

                string SQL_Query = $@"EXEC SP_ZPN_LISTACONTASRECEBER 
                                '{dataInicial}', 
                                '{dataFinal}',
                                '{tipoData}',
                                '{EdCardCode.Value}',
                                '{EdCardName.Value}',
                                '{EdCont.Value}',
                                '{EdObra.Value}',
                                '{EdPO.Value}',
                                '{EdFat.Value}'";

                DtDadosCR.ExecuteQuery(SQL_Query);

                //MtDadosCR.Columns.Item("#").DataBind.Bind("DtPO", "LineId");
                MtDadosCR.Columns.Item("Col_0").DataBind.Bind("DtItm", "Fatura");
                MtDadosCR.Columns.Item("Col_1").DataBind.Bind("DtItm", "Seleciona");
                MtDadosCR.Columns.Item("Col_2").DataBind.Bind("DtItm", "Obra");
                MtDadosCR.Columns.Item("Col_3").DataBind.Bind("DtItm", "Candidato");
                MtDadosCR.Columns.Item("Col_4").DataBind.Bind("DtItm", "Local");
                MtDadosCR.Columns.Item("Col_5").DataBind.Bind("DtItm", "Parcela");
                MtDadosCR.Columns.Item("Col_6").DataBind.Bind("DtItm", "Emissao");
                MtDadosCR.Columns.Item("Col_7").DataBind.Bind("DtItm", "Vencimento");
                MtDadosCR.Columns.Item("Col_8").DataBind.Bind("DtItm", "Programado");
                MtDadosCR.Columns.Item("Col_9").DataBind.Bind("DtItm", "Tipo");
                MtDadosCR.Columns.Item("Col_10").DataBind.Bind("DtItm", "Recebimento");
                MtDadosCR.Columns.Item("Col_11").DataBind.Bind("DtItm", "OF");
                MtDadosCR.Columns.Item("Col_12").DataBind.Bind("DtItm", "Situacao");
                MtDadosCR.Columns.Item("Col_13").DataBind.Bind("DtItm", "Cliente");
                MtDadosCR.Columns.Item("Col_14").DataBind.Bind("DtItm", "RazaoSocial");
                MtDadosCR.Columns.Item("Col_15").DataBind.Bind("DtItm", "Contrato");
                MtDadosCR.Columns.Item("Col_16").DataBind.Bind("DtItm", "ValorTitulo");
                MtDadosCR.Columns.Item("Col_17").DataBind.Bind("DtItm", "ValorRecebido");
                MtDadosCR.Columns.Item("Col_18").DataBind.Bind("DtItm", "Desconto");
                MtDadosCR.Columns.Item("Col_19").DataBind.Bind("DtItm", "Outros");
                MtDadosCR.Columns.Item("Col_20").DataBind.Bind("DtItm", "PIS");
                MtDadosCR.Columns.Item("Col_21").DataBind.Bind("DtItm", "COFINS");
                MtDadosCR.Columns.Item("Col_22").DataBind.Bind("DtItm", "CSLL");
                MtDadosCR.Columns.Item("Col_23").DataBind.Bind("DtItm", "INSS");
                MtDadosCR.Columns.Item("Col_24").DataBind.Bind("DtItm", "IRRF");
                MtDadosCR.Columns.Item("Col_25").DataBind.Bind("DtItm", "ISS");
                MtDadosCR.Columns.Item("Col_26").DataBind.Bind("DtItm", "ValorLiquido");
                MtDadosCR.Columns.Item("Col_27").DataBind.Bind("DtItm", "EtapaRecebimento");
                
                MtDadosCR.LoadFromDataSourceEx();
                MtDadosCR.AutoResizeColumns();

                LinhasSelecionadas = new List<int>();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao pesquisar dados de contas a receber: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

    }
}
