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
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmEfetivaFaturamento : FormSDK
    {
        EditText EdDataI { get; set; }
        EditText EdDataF { get; set; }

        EditText EdDataIncI { get; set; }
        EditText EdDataIncF { get; set; }
        EditText EdPO { get; set; }
        EditText EdItem { get; set; }
        EditText EdCliente { get; set; }
        DataTable DtPesquisa { get; set; }
        Matrix MtPedidos { get; set; }
        ComboBox cbUsuario { get; set; }

        EditText EdDataT { get; set; }
        Button BtAlterarDataT { get; set; }

        Button BtPesquisar { get; set; }
        Button BtImportarFaturamento { get; set; }
        Button BtCancelarPreFaturamento { get; set; }

        EditText EdObra { get; set; }

        EditText EDTotBru { get; set; }
        EditText EdCofins { get; set; }
        EditText EdCsll { get; set; }
        EditText EdIrrf { get; set; }
        EditText EdPis { get; set; }
        EditText EdInss { get; set; }
        EditText EdIss { get; set; }

        EditText EDTotBruG { get; set; }
        EditText EdCofinsG { get; set; }
        EditText EdCsllG { get; set; }
        EditText EdIrrfG { get; set; }
        EditText EdPisG { get; set; }
        EditText EdInssG { get; set; }
        EditText EdIssG { get; set; }

        List<Int32> LinhasSelecionadas = new List<int>();


        Button BtEnviarFaturamento { get; set; }

        public FrmEfetivaFaturamento() : base()
        {
            if (oForm == null)
                return;

            cbUsuario = (ComboBox)oForm.Items.Item("cbUsu").Specific;

            EdDataI = (EditText)oForm.Items.Item("EdDataI").Specific;
            EdDataF = (EditText)oForm.Items.Item("EdDataF").Specific;

            EdDataIncI = (EditText)oForm.Items.Item("EdDtIncI").Specific;
            EdDataIncF = (EditText)oForm.Items.Item("EdDtIncF").Specific;

            EDTotBru = (EditText)oForm.Items.Item("EDTotBru").Specific;
            EdCofins = (EditText)oForm.Items.Item("EdCofins").Specific;
            EdCsll = (EditText)oForm.Items.Item("EdCsll").Specific;
            EdIrrf = (EditText)oForm.Items.Item("EdIrrf").Specific;
            EdPis = (EditText)oForm.Items.Item("EdPis").Specific;
            EdInss = (EditText)oForm.Items.Item("EdInss").Specific;
            EdIss = (EditText)oForm.Items.Item("EdIss").Specific;
            
            EDTotBruG = (EditText)oForm.Items.Item("EDTotBruG").Specific;
            EdCofinsG = (EditText)oForm.Items.Item("EdCofinsG").Specific;
            EdCsllG = (EditText)oForm.Items.Item("EdCsllG").Specific;
            EdIrrfG = (EditText)oForm.Items.Item("EdIrrfG").Specific;
            EdPisG = (EditText)oForm.Items.Item("EdPisG").Specific;
            EdInssG = (EditText)oForm.Items.Item("EdInssG").Specific;
            EdIssG = (EditText)oForm.Items.Item("EdIssG").Specific;

            EdItem = (EditText)oForm.Items.Item("EdItem").Specific;

            EdItem = (EditText)oForm.Items.Item("EdItem").Specific;

            EdObra = (EditText)oForm.Items.Item("EdObra").Specific;

            EdPO = (EditText)oForm.Items.Item("EdPO").Specific;

            EdCliente = (EditText)oForm.Items.Item("EdCliente").Specific;

            MtPedidos = (Matrix)oForm.Items.Item("MtPed").Specific;

            DtPesquisa = oForm.DataSources.DataTables.Item("DtPO");

            EdDataT = (EditText)oForm.Items.Item("EdDataT").Specific;
            BtAlterarDataT = (Button)oForm.Items.Item("BtDataT").Specific;
            BtAlterarDataT.PressedAfter += BtAlterarDataT_PressedAfter;

            BtPesquisar = (Button)oForm.Items.Item("BtPesq").Specific;
            BtPesquisar.PressedAfter += BtPesquisar_PressedAfter;

            BtEnviarFaturamento = (Button)oForm.Items.Item("BtEnv").Specific;
            BtEnviarFaturamento.PressedAfter += BtPreFaturamento_PressedAfter;

            BtImportarFaturamento = (Button)oForm.Items.Item("BtImpFat").Specific;
            BtImportarFaturamento.PressedAfter += BtImportarFaturamento_PressedAfter;

            BtCancelarPreFaturamento = (Button)oForm.Items.Item("BtCanc").Specific;
            BtCancelarPreFaturamento.PressedAfter += BtCancelarPreFaturamento_PressedAfter;

            BtImportarFaturamento.Item.Visible = false;

            MtPedidos.LostFocusAfter += MtPedidos_LostFocusAfter;
            MtPedidos.ValidateBefore += MtPedidos_ValidateBefore;

            MtPedidos.ClickAfter += MtPedidos_ClickAfter;

            MtPedidos.LinkPressedBefore += MtPedidos_LinkPressedBefore;

            MtPedidos.AutoResizeColumns();

            CarregarDadosTela();

            CarregarDadosFaturamentoFaturar();

            oForm.Visible = true;

            LinhasSelecionadas = new List<int>();
        }             

        private void MtPedidos_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.Row > 0 && pVal.ColUID == "Col_9")
                    SomarRegistrosTelaSelecionado(pVal.Row-1);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar registro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);

            }
        }
        
        private void SomarRegistrosTelaSelecionado(int row)
        {
            try
            {

                oForm.Freeze(true);

                MtPedidos.FlushToDataSource();

                if (DtPesquisa.GetValue("Selecionar", row).ToString() == "Y")
                {
                    if (!LinhasSelecionadas.Contains(row))
                        LinhasSelecionadas.Add(row);
                }
                else
                {
                    if (LinhasSelecionadas.Contains(row))
                        LinhasSelecionadas.Remove(row);
                }

                double Valor = 0;
                double COFINS = 0;
                double CSLL = 0;
                double IRRF = 0;
                double PIS = 0;
                double INSS = 0;
                double ISS = 0;

                for (int iRow = 0; iRow  < LinhasSelecionadas.Count; iRow ++)
                {
                    Valor += Convert.ToDouble(DtPesquisa.GetValue("Valor", LinhasSelecionadas[iRow] ));
                    COFINS += Convert.ToDouble(DtPesquisa.GetValue("COFINS", LinhasSelecionadas[iRow]));
                    CSLL += Convert.ToDouble(DtPesquisa.GetValue("CSLL", LinhasSelecionadas[iRow]));
                    IRRF += Convert.ToDouble(DtPesquisa.GetValue("IRRF", LinhasSelecionadas[iRow]));
                    PIS += Convert.ToDouble(DtPesquisa.GetValue("PIS", LinhasSelecionadas[iRow]));
                    INSS += Convert.ToDouble(DtPesquisa.GetValue("INSS", LinhasSelecionadas[iRow]));
                    ISS += Convert.ToDouble(DtPesquisa.GetValue("ISS", LinhasSelecionadas[iRow]));
                }

                EDTotBru.Value = Valor.ToString().Replace(".", "").Replace(",", ".");
                EdCofins.Value = COFINS.ToString().Replace(".", "").Replace(",", ".");
                EdCsll.Value = CSLL.ToString().Replace(".", "").Replace(",", ".");
                EdIrrf.Value = IRRF.ToString().Replace(".", "").Replace(",", ".");
                EdPis.Value = PIS.ToString().Replace(".", "").Replace(",", ".");
                EdInss.Value = INSS.ToString().Replace(".", "").Replace(",", ".");
                EdIss.Value = ISS.ToString().Replace(".", "").Replace(",", ".");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao somar registros selecionados em tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }

        }

        private void SomarRegistrosTelaTotal()
        {
            try
            {
                oForm.Freeze(true);

                MtPedidos.FlushToDataSource();

                double Valor = 0;
                double COFINS = 0;
                double CSLL = 0;
                double IRRF = 0;
                double PIS = 0;
                double INSS = 0;
                double ISS = 0;

                for (int iRow = 0; iRow < DtPesquisa.Rows.Count; iRow++)
                {
                    Valor += Convert.ToDouble(DtPesquisa.GetValue("Valor", iRow));
                    COFINS += Convert.ToDouble(DtPesquisa.GetValue("COFINS", iRow));
                    CSLL += Convert.ToDouble(DtPesquisa.GetValue("CSLL", iRow));
                    IRRF += Convert.ToDouble(DtPesquisa.GetValue("IRRF", iRow));
                    PIS += Convert.ToDouble(DtPesquisa.GetValue("PIS", iRow));
                    INSS += Convert.ToDouble(DtPesquisa.GetValue("INSS", iRow));
                    ISS += Convert.ToDouble(DtPesquisa.GetValue("ISS", iRow));
                }

                EDTotBruG.Value = Valor.ToString().Replace(".", "").Replace(",", ".");
                EdCofinsG.Value = COFINS.ToString().Replace(".", "").Replace(",", ".");
                EdCsllG.Value = CSLL.ToString().Replace(".", "").Replace(",", ".");
                EdIrrfG.Value = IRRF.ToString().Replace(".", "").Replace(",", ".");
                EdPisG.Value = PIS.ToString().Replace(".", "").Replace(",", ".");
                EdInssG.Value = INSS.ToString().Replace(".", "").Replace(",", ".");
                EdIssG.Value = ISS.ToString().Replace(".", "").Replace(",", ".");

                EDTotBru.Value = "0";
                EdCofins.Value = "0";
                EdCsll.Value = "0";
                EdIrrf.Value = "0";
                EdPis.Value = "0";
                EdInss.Value = "0";
                EdIss.Value = "0";

                LinhasSelecionadas = new List<int>();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao somar registros em tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void CarregarDadosTela()
        {
            try
            {
                Util.ComboBoxSetValoresValidosPorSQL(cbUsuario, UtilScriptsSQL.SQL_Usuarios, null, true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados em tela: {Ex.Message}");
            }

        }

        private void MtPedidos_LinkPressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                Int32 DocEntry = Convert.ToInt32(DtPesquisa.GetValue("Esboco", pVal.Row - 1));

                SqlUtils.ExecuteCommand($"SP_ZPN_ATUALIZAPROJETOESBOCO {DocEntry.ToString()}");

                SAPbobsCOM.Documents oEsbocoNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                if (oEsbocoNotaFiscalSaida.GetByKey(DocEntry))
                {
                    if (oEsbocoNotaFiscalSaida.Cancelled == BoYesNoEnum.tYES || oEsbocoNotaFiscalSaida.DocumentStatus != BoStatus.bost_Open)
                    {
                        Util.ExibeMensagensDialogoStatusBar($"Documento já faturado!");
                        CarregarDadosFaturamentoFaturar();

                    }
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar Pré Faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }


            BubbleEvent = true;
        }

        private void BtAlterarDataT_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo("Deseja alterar a Previsão de Transmissão para as notas selecionadas?"))
                    return;

                AlterarDataPrevisaoFaturamento();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao importar faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void AlterarDataPrevisaoFaturamento()
        {
            try
            {
                string MensagemErro = string.Empty;

                MtPedidos.FlushToDataSource();

                DateTime dtDataTransmissao = DateTime.ParseExact(EdDataT.Value, "yyyyMMdd", CultureInfo.InvariantCulture);

                for (int iRow = 0; iRow < DtPesquisa.Rows.Count; iRow++)
                {
                    if (DtPesquisa.GetValue("Selecionar", iRow).ToString() == "Y")
                    {
                        Int32 DocEntry = Convert.ToInt32(DtPesquisa.GetValue("Esboco", iRow));

                        SAPbobsCOM.Documents oEsbocoNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                        if (oEsbocoNotaFiscalSaida.GetByKey(DocEntry))
                        {
                            oEsbocoNotaFiscalSaida.DocDate = dtDataTransmissao;
                            if (oEsbocoNotaFiscalSaida.Update() != 0)
                                MensagemErro += $" {Globals.Master.Connection.Database.GetLastErrorDescription()} ";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(MensagemErro))
                    Util.ExibeMensagensDialogoStatusBar($"Erro ao alterar data de transmissão: {MensagemErro}", BoMessageTime.bmt_Medium, true);

                CarregarDadosFaturamentoFaturar();

                Util.ExibirMensagemStatusBar("Dados alterados com sucesso!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao alterar data de previsão de transmissão: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void BtImportarFaturamento_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                ImportarArquivoFaturamento();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao importar faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void ImportarArquivoFaturamento()
        {
            string fileNameAnexo = string.Empty;

            // Cria uma nova thread STA
            Thread thread = new Thread(() =>
            {
                using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
                {
                    openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    openFileDialog.Title = "Selecione um arquivo CSV";

                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        fileNameAnexo = openFileDialog.FileName;
                    }
                }
            });

            // Define a thread para ser do tipo STA
            thread.SetApartmentState(ApartmentState.STA);

            // Inicia a thread
            thread.Start();

            // Aguarda a thread terminar
            thread.Join();

            if (!string.IsNullOrEmpty(fileNameAnexo))
            {
                using (var arquivoImportacaoFaturamentoHuawei = new StreamReader(fileNameAnexo))
                {
                    var linesArquivoFaturamentoHuawei = arquivoImportacaoFaturamentoHuawei.ReadToEnd().Split(new char[] { '\n' });
                    var count = linesArquivoFaturamentoHuawei.Length;

                    string SQL = string.Empty;

                    SqlUtils.DoNonQuery("TRUNCATE TABLE ZPN_IMPFATURAMENTOHUAWEI");


                    for (int iPos = 0; iPos < linesArquivoFaturamentoHuawei.Length; iPos++)
                    {
                        try
                        {
                            Util.ExibirMensagemStatusBar($"Leitura de arquivo, linha {iPos}");

                            var valores = linesArquivoFaturamentoHuawei[iPos].Split(';');

                            if (string.IsNullOrEmpty(valores[0]) || valores[1] == "poLineLocationId")
                                continue;


                            SQL = $@"ZPN_SP_IMPFATURAMENTOHUAWEI 
                                            '{fileNameAnexo}',
                                            '{valores[ConfiguracoesImportacaoPO.NumeroPO].Trim()}', 
                                            '{valores[ConfiguracoesImportacaoPO.NumeroLinha].Trim()}', 
                                            {valores[ConfiguracoesImportacaoPO.QuantidadeFaturada].Trim()},
                                            '{valores[ConfiguracoesImportacaoPO.CodigoServico].Trim()}', 
                                            '{valores[ConfiguracoesImportacaoPO.Item].Trim()}', 
                                            {valores[ConfiguracoesImportacaoPO.ValorUnitario].Trim()}, 
                                            {valores[ConfiguracoesImportacaoPO.ValorUnitario].Trim()}";


                            SqlUtils.DoNonQuery(SQL);
                        }
                        catch (Exception Ex)
                        {
                            string erro = $"Erro ao importar linha {iPos + 1} do arquivo: {Ex.Message}!";
                            throw new Exception(erro);
                        }
                    }

                    Util.ExibirMensagemStatusBar($"Fim da Leitura de arquivo, gerando faturamento.");


                }
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
        private void BtCancelarPreFaturamento_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo("Deseja CANCELAR o pré faturamento dos documentos selecionados?"))
                    return;

                MtPedidos.FlushToDataSource();

                for (int iRow = 0; iRow < DtPesquisa.Rows.Count; iRow++)
                {
                    if (DtPesquisa.GetValue("Selecionar", iRow).ToString() == "Y")
                    {
                        CancelarDocumentoPreFaturamento(iRow);

                        //voltaraqui

                    }
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro cancelar pré faturamento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                CarregarDadosFaturamentoFaturar();
            }
        }



        private void BtPreFaturamento_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo("Deseja efetivar o faturamento dos documentos?"))
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
        private void CancelarDocumentoPreFaturamento(int iRow)
        {
            Int32 DocEntry = Convert.ToInt32(DtPesquisa.GetValue("Esboco", iRow));

            SAPbobsCOM.Documents oEsbocoNotaFiscalSaida = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            if (oEsbocoNotaFiscalSaida.GetByKey(DocEntry))
            {
                if (oEsbocoNotaFiscalSaida.Cancel() != 0)
                    throw new Exception($"Erro ao cancelar documentos de pré faturamento: {oEsbocoNotaFiscalSaida.NumAtCard} - {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                SqlUtils.DoNonQuery($"EXEC SP_ZPN_REMOVENFSERVICO {DocEntry}");
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
                    if (oEsbocoNotaFiscalSaida.DocDate != DateTime.Now.Date)
                    {
                        return $"Erro ao Faturar PO {oEsbocoNotaFiscalSaida.NumAtCard} - Data de transmissão diferente da data de hoje!";
                    }
                    
                    oEsbocoNotaFiscalSaida.DiscountPercent = 0;

                    if (oEsbocoNotaFiscalSaida.SaveDraftToDocument() != 0)
                        return $"Erro ao Faturar PO {oEsbocoNotaFiscalSaida.NumAtCard} Linha {iRow + 1} -  {Globals.Master.Connection.Database.GetLastErrorDescription()} \n";


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
                    SelecionaAtividadeServico(pVal.Row - 1);
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

                string dataInicialInclusao = !string.IsNullOrEmpty(EdDataIncI.Value) ? EdDataIncI.Value : "20200101";
                string dataFinalInclusao = !string.IsNullOrEmpty(EdDataIncF.Value) ? EdDataIncF.Value : "20500101";

                string Usuario = !string.IsNullOrEmpty(cbUsuario.Value) ? cbUsuario.Value : "-1";


                string SQL_Query = $@"ZPN_SP_EfetivaPedidosPreFaturamento '{dataInicial}', '{dataFinal}', '{dataInicialInclusao}', '{dataFinalInclusao}','{EdPO.Value}','{EdCliente.Value}', {Usuario}, '{EdItem.Value}', '{EdObra.Value}'";

                DtPesquisa.ExecuteQuery(SQL_Query);



                MtPedidos.Columns.Item("#").DataBind.Bind("DtPO", "LineId");
                MtPedidos.Columns.Item("Col_9").DataBind.Bind("DtPO", "Selecionar");
                MtPedidos.Columns.Item("Col_0").DataBind.Bind("DtPO", "Pedido");
                MtPedidos.Columns.Item("Col_1").DataBind.Bind("DtPO", "PO");

                MtPedidos.Columns.Item("Col_4").DataBind.Bind("DtPO", "DataT");

                MtPedidos.Columns.Item("Col_20").DataBind.Bind("DtPO", "DataI");

                MtPedidos.Columns.Item("CardCode").DataBind.Bind("DtPO", "CardCode");
                MtPedidos.Columns.Item("CardName").DataBind.Bind("DtPO", "CardName");

                MtPedidos.Columns.Item("Col_3").DataBind.Bind("DtPO", "Linha");
                MtPedidos.Columns.Item("Col_2").DataBind.Bind("DtPO", "Item");
                MtPedidos.Columns.Item("Col_5").DataBind.Bind("DtPO", "Valor");
                MtPedidos.Columns.Item("Col_6").DataBind.Bind("DtPO", "Esboco");

                MtPedidos.Columns.Item("Col_10").DataBind.Bind("DtPO", "ItemCode");
                MtPedidos.Columns.Item("Col_11").DataBind.Bind("DtPO", "Dscription");
                MtPedidos.Columns.Item("Col_12").DataBind.Bind("DtPO", "Status");
                MtPedidos.Columns.Item("Col_14").DataBind.Bind("DtPO", "SaldoFaturado");

                MtPedidos.Columns.Item("Col_8").DataBind.Bind("DtPO", "Obra");
                MtPedidos.Columns.Item("Col_7").DataBind.Bind("DtPO", "Contrato");

                MtPedidos.Columns.Item("Col_13").DataBind.Bind("DtPO", "COFINS");
                MtPedidos.Columns.Item("Col_15").DataBind.Bind("DtPO", "CSLL");
                MtPedidos.Columns.Item("Col_16").DataBind.Bind("DtPO", "IRRF");
                MtPedidos.Columns.Item("Col_17").DataBind.Bind("DtPO", "PIS");
                MtPedidos.Columns.Item("Col_18").DataBind.Bind("DtPO", "INSS");
                MtPedidos.Columns.Item("Col_19").DataBind.Bind("DtPO", "ISS");

                MtPedidos.Columns.Item("Col_21").DataBind.Bind("DtPO", "USER_CODE");

                MtPedidos.LoadFromDataSourceEx();
                MtPedidos.AutoResizeColumns();

                SomarRegistrosTelaTotal();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
