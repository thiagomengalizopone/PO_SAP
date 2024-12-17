using Newtonsoft.Json;
using RestSharp;
using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Windows.Forms;
using Zopone.AddOn.PO.Model.Objects;
using Button = SAPbouiCOM.Button;
using CheckBox = SAPbouiCOM.CheckBox;
using ComboBox = SAPbouiCOM.ComboBox;

namespace Zopone.AddOn.PO.View.Manutencao
{
    public class FrmAtualizaDataProgramada : FormSDK
    {
        #region Variáveis

        EditText EdCliente { get; set; }


        EditText EdDtLancI { get; set; }
        EditText EdDtLancF { get; set; }


        EditText EdDtVencI { get; set; }
        EditText EdDtVencF { get; set; }


        EditText EdDtProgI { get; set; }
        EditText EdDtProgF { get; set; }


        ComboBox CbDocProg { get; set; }
        Button BtFiltrar { get; set; }
        Item iBtFiltrar { get; set; }


        SAPbouiCOM.DataTable DtDocs { get; set; }
        Matrix MtNotasP { get; set; }


        EditText EdDataProg { get; set; }
        Item iEdDataProg { get; set; }
        Button BtAtualizar { get; set; }

        Button BtHelp1 { get; set; }

        bool FlegarChecks = false;

        #endregion

        public FrmAtualizaDataProgramada() : base()
        {
            if (oForm == null)
                return;

            EdCliente = (EditText)oForm.Items.Item("EdCli").Specific;

            EdDtLancI = (EditText)oForm.Items.Item("EdDtLaI").Specific;
            EdDtLancF = (EditText)oForm.Items.Item("EdDtLaF").Specific;

            EdDtVencI = (EditText)oForm.Items.Item("EdDtVeI").Specific;
            EdDtVencF = (EditText)oForm.Items.Item("EdDtVeF").Specific;

            EdDtProgI = (EditText)oForm.Items.Item("EdDtPrI").Specific;
            EdDtProgF = (EditText)oForm.Items.Item("EdDtPrF").Specific;

            CbDocProg = (ComboBox)oForm.Items.Item("CbDocProg").Specific;
            CbDocProg.Select(0, BoSearchKey.psk_Index);

            BtFiltrar = (Button)oForm.Items.Item("BtFiltrar").Specific;
            iBtFiltrar = oForm.Items.Item("BtFiltrar");
            BtFiltrar.PressedAfter += BtFiltrar_PressedAfter;

            DtDocs = oForm.DataSources.DataTables.Item("DtDocs");
            MtNotasP = (Matrix)oForm.Items.Item("MtNotasP").Specific;
            MtNotasP.ClickAfter += CkBoxMatrixLotes_PressedAfter;
            MtNotasP.KeyDownAfter += CkBoxMatrixLotes_PressedAfter;
            MtNotasP.DoubleClickAfter += Double_ClickAter;

            EdDataProg = (EditText)oForm.Items.Item("EdDataProg").Specific;
            iEdDataProg = oForm.Items.Item("EdDataProg");

            BtAtualizar = (Button)oForm.Items.Item("BtAtt").Specific;
            BtAtualizar.PressedAfter += BtAtualizar_PressedAfter;

            BtHelp1 = (Button)oForm.Items.Item("BtHelp1").Specific;
            BtHelp1.PressedAfter += BtHelp1_PressedAfter;
            BtHelp1.Item.Visible = false;

            MtNotasP.AutoResizeColumns();

            oForm.Visible = true;
        }

        private void BtHelp1_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.Item oItem = oForm.Items.Add("browser", SAPbouiCOM.BoFormItemTypes.it_WEB_BROWSER);
                oItem.Top = 50;
                oItem.Left = 10;
                oItem.Width = 400;
                oItem.Height = 300;

                // Configurar o navegador
                SAPbouiCOM.WebBrowser oBrowser = (SAPbouiCOM.WebBrowser)oItem.Specific;
                oBrowser.Url = "\\\\srvdc\\Users\\thiago.martins\\help\\help1.html"; // URL que será aberta
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private int ConverteRGB(int R, int G, int B)
        {
            try
            {
                return (R * 65536) + (G * 256) + B;
            }
            catch { }
            return 0;
        }

        private void CkBoxMatrixLotes_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if ((pVal.CharPressed == 0 || pVal.CharPressed == 32) && pVal.ColUID == "Col_0")
            {
                try
                {
                    oForm.Freeze(true);

                    if (((CheckBox)MtNotasP.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific).Checked == true)
                    {

                        MtNotasP.CommonSetting.SetRowBackColor(pVal.Row, 16753920);
                        MtNotasP.CommonSetting.SetRowFontColor(pVal.Row, ConverteRGB(255, 255, 255));

                        MtNotasP.CommonSetting.SetCellBackColor(pVal.Row, 1, ConverteRGB(255, 255, 255));
                        //MtNotasP.CommonSetting.SetCellFontColor(pVal.Row, 0, ConverteRGB(0, 0, 0));                          
                    }
                    else
                    {
                        MtNotasP.CommonSetting.SetRowBackColor(pVal.Row, -1);
                        MtNotasP.CommonSetting.SetRowFontColor(pVal.Row, ConverteRGB(0, 0, 0));
                    }
                }
                catch (Exception Ex)
                {
                    var tempErro = Ex.Message;
                }
                finally
                {
                    oForm.Freeze(false);
                }
            }
        }

        private void Double_ClickAter(object sboObject, SBOItemEventArg pVal)
        {
            if ((pVal.CharPressed is 0 || pVal.CharPressed is 32) && pVal.ColUID is "Col_0" && pVal.Row is 0)
            {
                try
                {
                    oForm.Freeze(true);

                    //for (int i = 0; i < DtDocs.Rows.Count; i++)                    
                    //    DtDocs.SetValue("Selecionar", i, FlegarChecks ? "Y" : "N");

                    //MtNotasP.LoadFromDataSource();

                    for (int iRow = 1; iRow <= DtDocs.Rows.Count; iRow++)
                        ((CheckBox)MtNotasP.Columns.Item("Col_0").Cells.Item(iRow).Specific).Checked = FlegarChecks;

                    FlegarChecks = !FlegarChecks;
                }
                catch (Exception Ex)
                {
                    var tempErro = Ex.Message;
                }
                finally
                {
                    oForm.Freeze(false);
                }
            }
        }

        private void BtAtualizar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (DtDocs.IsEmpty || DtDocs.Rows.Count == 0)
                {
                    Util.ExibeMensagensDialogoStatusBar("Necessário filtrar os documentos!");                    
                    return;
                }

                MtNotasP.FlushToDataSource();

                string MensagemErro = string.Empty;
                string batch = "";

                for (int iRow = 0; iRow < DtDocs.Rows.Count; iRow++)
                {
                    if (DtDocs.GetValue("Selecionar", iRow).ToString() == "Y")
                    {
                        batch += "--batch_boundary\n" +
                        "Content-Type: application/http\n" +
                        "Content-Transfer-Encoding: binary\n\n" +

                        $"PATCH /b1s/v1/Invoices({DtDocs.GetValue("Documento", iRow).ToString()})\n" +
                        "Content-Type: application/json\n\n" +
                        
                        "{\n" +
                        "\"DocumentInstallments\": [{\n" +
                        "\"InstallmentId\": " + DtDocs.GetValue("Parcela", iRow).ToString() +",\n" +
                        "\"U_InformacaoCobranca\": " + (string.IsNullOrEmpty(EdDataProg.Value) ? "null" : $"\"Programação:{EdDataProg.Value.Substring(6, 2) + "/" + EdDataProg.Value.Substring(4, 2) + "/" + EdDataProg.Value.Substring(0, 4)}\"") +",\n" +
                        "\"U_DataProgramacao\": " + (string.IsNullOrEmpty(EdDataProg.Value) ? "null" : $"\"{EdDataProg.Value.Substring(0, 4) + "-" + EdDataProg.Value.Substring(4, 2) + "-" + EdDataProg.Value.Substring(6, 2)}"+"\"") + "\n" +
                        "}]\n" +
                        "}\n\n";
                    }
                }

                if (batch is "")
                {
                    Util.ExibeMensagensDialogoStatusBar("Necessário selecionar os documentos!");
                    MtNotasP.Columns.Item(1).Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular);                    
                    return;
                }
                batch += "--batch_boundary--";

                if (string.IsNullOrEmpty(EdDataProg.Value))
                {
                    if (!Util.RetornarDialogo("A data programada será excluída dos documentos selecionados, continuar?"))
                    {
                        iEdDataProg.Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                        return;
                    }     
                }
                else if (!Util.RetornarDialogo("Deseja atualizar a data programada dos documentos selecionados?"))
                        return;

                Util.ExibirMensagemStatusBar("Aguarde, processando...");

                #region N8N Batch

                var options = new RestClientOptions("http://srvetl:5678")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/webhook/batch-sap", Method.Post);
                request.AddHeader("Content-Type", "application/json");

                request.AddStringBody(JsonConvert.SerializeObject(new Cbatch
                {
                    CompanyDB = Globals.Master.Connection.Database.CompanyDB,
                    JsonBatch = batch

                }), DataFormat.Json);

                RestResponse response = client.Execute(request);
                //tratar possíveis erros de retorno da API local!
                //Exemplo, webhook offline.
                #endregion
                //return;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CarregarDadosNotas(true);
                    Util.ExibirMensagemStatusBar("Datas atualizadas!");
                }
                else
                    Util.ExibeMensagensDialogoStatusBar("Data não atualizada, entre em contato com o setor de TI.");

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private class Cbatch
        {
            public string CompanyDB { get; set; }
            public string JsonBatch { get; set; }
        }

        private void BtFiltrar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                CarregarDadosNotas(true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private bool CorPadraoTable()
        {
            try
            {
                for (int iRow = 0; iRow < DtDocs.Rows.Count; iRow++)
                {
                    MtNotasP.CommonSetting.SetRowBackColor(iRow + 1, -1);
                    MtNotasP.CommonSetting.SetRowFontColor(iRow + 1, ConverteRGB(0, 0, 0));
                }
            }
            catch { }
            return true;
        }

        private void CarregarDadosNotas(bool VoltarPadraBackGround = false)
        {
            try
            {
                string dataLancInicial = !string.IsNullOrEmpty(EdDtLancI.Value) ? EdDtLancI.Value : "20200101";
                string dataLancFinal = !string.IsNullOrEmpty(EdDtLancF.Value) ? EdDtLancF.Value : "20900101";

                string dataVencInicial = !string.IsNullOrEmpty(EdDtVencI.Value) ? EdDtVencI.Value : "20200101";
                string dataVencFinal = !string.IsNullOrEmpty(EdDtVencF.Value) ? EdDtVencF.Value : "20900101";

                string dataProgInicial = !string.IsNullOrEmpty(EdDtProgI.Value) ? EdDtProgI.Value : "20200101";
                string dataProgFinal = !string.IsNullOrEmpty(EdDtProgF.Value) ? EdDtProgF.Value : "20900101";


                if (CbDocProg.Selected.Value != "1")
                {
                    dataLancInicial = "20200101";
                    dataLancFinal   = "20900101";

                    dataVencInicial = "20200101";
                    dataVencFinal   = "20900101";

                    dataProgInicial = "20200101";
                    dataProgFinal   = "20900101";
                }
                                
                string SQL_Query = $@"SELECT
                                    Sequencia,
                                    Selecionar,
                                    Cliente,
                                    CNPJ,
                                    RazaoSocial,
                                    Documento,
                                    Fatura,
                                    Obra,
                                    Local,
                                    PO,
                                    Parcela,
                                    Emissao,
                                    Vencimento,
                                    Programacao,
                                    InfoPrg,
                                    PIS,
                                    COFINS,
                                    CSLL,
                                    INSS,
                                    IRRF,
                                    ISS,
                                    ValorTitulo,
                                    ValorRecebido,
                                    Desconto,
                                    Outros,
                                    ValorLiquido
                                    FROM ZPN_VW_DOCUMENTOS_REPROGRAMACAO ";// '{dataLancInicial}', '{dataLancFinal}', '{EdCliente.Value}'";

                SQL_Query += $" WHERE Emissao between '{dataLancInicial}' AND '{dataLancFinal}' ";
                
                SQL_Query += $" AND Vencimento between '{dataVencInicial}' AND '{dataVencFinal}' ";

                if (!string.IsNullOrEmpty(EdDtProgI.Value) && !string.IsNullOrEmpty(EdDtProgF.Value))                
                    SQL_Query += $" AND Programacao between '{dataProgInicial}' AND '{dataProgFinal}' ";  

                if(CbDocProg.Selected.Value != "1")                
                    SQL_Query += $" AND Programacao is " + (CbDocProg.Selected.Value == "2" ? "not" : "") + " null ";    
                
                if(EdCliente.Value != "")
                    SQL_Query += $" AND Cliente = '" + EdCliente.Value + "' ";

                DtDocs.ExecuteQuery(SQL_Query);

                //MtNotasP.Columns.Item("#").DataBind.Bind("DtDocs", "Sequencia");
                MtNotasP.Columns.Item("Col_0").DataBind.Bind("DtDocs", "Selecionar");
                MtNotasP.Columns.Item("Col_1").DataBind.Bind("DtDocs", "Cliente");
                MtNotasP.Columns.Item("Col_2").DataBind.Bind("DtDocs", "RazaoSocial");
                MtNotasP.Columns.Item("Col_3").DataBind.Bind("DtDocs", "Documento");
                MtNotasP.Columns.Item("Col_4").DataBind.Bind("DtDocs", "Fatura");
                MtNotasP.Columns.Item("Col_5").DataBind.Bind("DtDocs", "Obra");
                MtNotasP.Columns.Item("Col_6").DataBind.Bind("DtDocs", "Local");
                MtNotasP.Columns.Item("Col_7").DataBind.Bind("DtDocs", "PO");
                MtNotasP.Columns.Item("Col_8").DataBind.Bind("DtDocs", "Parcela");
                MtNotasP.Columns.Item("Col_9").DataBind.Bind("DtDocs", "Emissao");
                MtNotasP.Columns.Item("Col_10").DataBind.Bind("DtDocs", "Vencimento");
                MtNotasP.Columns.Item("Col_11").DataBind.Bind("DtDocs", "Programacao");
                MtNotasP.Columns.Item("Col_12").DataBind.Bind("DtDocs", "InfoPrg");
                MtNotasP.Columns.Item("Col_13").DataBind.Bind("DtDocs", "PIS");
                MtNotasP.Columns.Item("Col_14").DataBind.Bind("DtDocs", "COFINS");
                MtNotasP.Columns.Item("Col_15").DataBind.Bind("DtDocs", "CSLL");
                MtNotasP.Columns.Item("Col_16").DataBind.Bind("DtDocs", "INSS");
                MtNotasP.Columns.Item("Col_17").DataBind.Bind("DtDocs", "IRRF");
                MtNotasP.Columns.Item("Col_18").DataBind.Bind("DtDocs", "ISS");
                MtNotasP.Columns.Item("Col_19").DataBind.Bind("DtDocs", "ValorTitulo");
                MtNotasP.Columns.Item("Col_20").DataBind.Bind("DtDocs", "ValorRecebido");
                MtNotasP.Columns.Item("Col_21").DataBind.Bind("DtDocs", "Desconto");
                MtNotasP.Columns.Item("Col_22").DataBind.Bind("DtDocs", "Outros");
                MtNotasP.Columns.Item("Col_23").DataBind.Bind("DtDocs", "ValorLiquido");

                MtNotasP.LoadFromDataSourceEx();

                if (VoltarPadraBackGround)
                    CorPadraoTable();

                MtNotasP.AutoResizeColumns();


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
