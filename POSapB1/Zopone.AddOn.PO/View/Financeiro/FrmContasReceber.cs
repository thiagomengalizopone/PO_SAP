using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using RestSharp;
using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Zopone.AddOn.PO.Model.Objects;
using static System.Windows.Forms.LinkLabel;
using Button = SAPbouiCOM.Button;
using CheckBox = SAPbouiCOM.CheckBox;
using ComboBox = SAPbouiCOM.ComboBox;
using DataTable = SAPbouiCOM.DataTable;

namespace Zopone.AddOn.PO.View.Financeiro
{
    public class FrmContasReceber : FormSDK
    {
        #region Variáveis
        bool semafaro { get; set; } = true;
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

        Button BtFiltrar { get; set; }

        Matrix MtDadosCR { get; set; }

        DataTable DtDadosCR { get; set; }

        List<Int32> LinhasSelecionadas = new List<int>();

        Button BtEfetivar { get; set; }
        Button BtExcel { get; set; }
        Item iBtExcel { get; set; }

        CheckBox CkbLinhas { get; set; }

        EditText DtPag { get; set; }
        Item iDtPag { get; set; }

        LinkedButton lbutton { get; set; }

        #endregion

        public FrmContasReceber() : base()
        {
            if (oForm == null)
                return;

            MtDadosCR = (Matrix)oForm.Items.Item("MtPed").Specific;
            MtDadosCR.PressedAfter += CkBoxMatrixCR_PressedAfter;
            MtDadosCR.KeyDownAfter += MatrixCR_KeyDownAfter;
            MtDadosCR.ClickAfter += MtDadosCR_ClickAfter;            

            Column oColumn = MtDadosCR.Columns.Item("Col_29");
            oColumn.Visible = false;
            oColumn = MtDadosCR.Columns.Item("Col_30");
            oColumn.Visible = false;
            oColumn = MtDadosCR.Columns.Item("Col_34");
            oColumn.Visible = false;

            MtDadosCR.LinkPressedBefore += LinkPressedBefore_ClickBefore;

            MtDadosCR.LostFocusAfter += MtDadosCR_LostFocusAfter;

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
            //EdDataI.Value = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + "01";

            EdDataF = (EditText)oForm.Items.Item("EdDataF").Specific;
            //EdDataF.Value = DateTime.Now.ToString("yyyyMMdd");

            CbTpDt = (ComboBox)oForm.Items.Item("CbTpDt").Specific;
            EdObra = (EditText)oForm.Items.Item("EdObra").Specific;
            EdObra.ChooseFromListAfter += EdObra_ChooseFromListAfter;

            EdPO = (EditText)oForm.Items.Item("EdPO").Specific;
            EdCardCode = (EditText)oForm.Items.Item("EdCardCode").Specific;
            EdCardName = (EditText)oForm.Items.Item("EdCardName").Specific;
            EdCont = (EditText)oForm.Items.Item("EdCont").Specific;
            EdFat = (EditText)oForm.Items.Item("EdFat").Specific;

            CkbLinhas = (CheckBox)oForm.Items.Item("ckbLinhas").Specific;
            CkbLinhas.Checked = false;
            CkbLinhas.ClickAfter += CkbLinhas_ClickAfter;

            BtFiltrar = (Button)oForm.Items.Item("BtPesq").Specific;
            BtFiltrar.PressedAfter += BtFiltrar_PressedAfter;

            BtEfetivar = (Button)oForm.Items.Item("BtEnv").Specific;           
            BtEfetivar.PressedAfter += BtEfetivar_PressedAfter;

            BtExcel = (Button)oForm.Items.Item("BtExcel").Specific;            
            BtExcel.PressedAfter += BtExcel_PressedAfter;
            iBtExcel = oForm.Items.Item("BtExcel");
            iBtExcel.Visible = false;

            CbTpDt.Select("V", BoSearchKey.psk_ByValue);

            DtPag = (EditText)oForm.Items.Item("EdDtPt").Specific;
            iDtPag = oForm.Items.Item("EdDtPt");

            oForm.Visible = true;

            LinhasSelecionadas = new List<int>();

            Util.ExibirMensagemStatusBar("Aguarde enquanto a tela é carregada!");

            //PesquisarDadosPagamento();
        }

        static string OpenExcelFile()
        {
            string filePath = "";
            Thread thread = new Thread(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Arquivos Excel|*.xls;*.xlsx",
                    Title = "Selecione um arquivo Excel"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    //MessageBox.Show("Arquivo selecionado: " + filePath);
                    Util.ExibirMensagemStatusBar("Arquivo selecionado: " + filePath, BoMessageTime.bmt_Short);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(); // Aguarda o thread terminar antes de prosseguir

            return filePath;

            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
            //    openFileDialog.Title = "Selecione um arquivo Excel";

            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        return openFileDialog.FileName;
            //    }
            //}
            //return null;
        }

        static void ReadExcel(string filePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Lê a primeira planilha
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 1; row <= rowCount; row++)
                {
                    Console.WriteLine(worksheet.Cells[row, 1].Text + " | " + worksheet.Cells[row, 2].Text + "\t");
                    //for (int col = 1; col <= colCount; col++)
                    //{
                    //    Console.Write(worksheet.Cells[row, col].Text + "\t");
                    //}
                    //Console.WriteLine();
                }
            }
        }

        private void BtExcel_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            string filePath = OpenExcelFile();

            if (!string.IsNullOrEmpty(filePath))
            {
                ReadExcel(filePath);
            }
            else
            {
                Console.WriteLine("Nenhum arquivo selecionado.");
            }
        }

        private void EdObra_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                oForm.Freeze(true);

                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                EdObra.Value = Convert.ToString(aEvent.SelectedObjects.GetValue("PrjCode", 0));
                //EdDescCliente.Value = Convert.ToString(aEvent.SelectedObjects.GetValue("CardName", 0));
                //EdPamCardDig.Value = Convert.ToString(aEvent.SelectedObjects.GetValue("U_PamCard", 0));
            }
            catch (Exception Ex)
            {
                if (!Ex.Message.Contains("Data Table"))
                    Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar PN: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private int RGB(int r, int g, int b)
        {
            return (b << 16) | (g << 8) | r;
        }

        private void MtDadosCR_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.Row > 0)
                MtDadosCR.SelectRow(pVal.Row, true, false);
        }

        private void MatrixCR_KeyDownAfter(object sboObject, SBOItemEventArg pVal)
        {

        }

        private void LinkPressedBefore_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            if ((pVal.CharPressed == 0 || pVal.CharPressed == 32) && pVal.ColUID == "Col_11")
            {
                try
                {
                    lbutton = (LinkedButton)MtDadosCR.Columns.Item("Col_11").ExtendedObject;
                    lbutton.LinkedObjectType = ((EditText)MtDadosCR.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific).Value == "NFE" ? "13" : "30";
                }
                catch (Exception ex)
                {
                    var exTemp = ex.Message;
                }
            }
            BubbleEvent = true;
        }

        private void MtDadosCR_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            if ((pVal.CharPressed == 0 || pVal.CharPressed == 32) && (pVal.ColUID == "Col_18" || pVal.ColUID == "Col_19" || pVal.ColUID == "Col_28" || pVal.ColUID == "Col_31" || pVal.ColUID == "Col_32"))
            {
                try
                {
                    oForm.Freeze(true);

                    double DescOri   = Math.Round(double.Parse(DtDadosCR.GetValue("Desconto",      pVal.Row - 1).ToString()), 2);
                    double OutrosOri = Math.Round(double.Parse(DtDadosCR.GetValue("Outros",        pVal.Row - 1).ToString()), 2);
                    double ValAReOri = Math.Round(double.Parse(DtDadosCR.GetValue("ValorAReceber", pVal.Row - 1).ToString()), 2);
                    double DescPOri  = Math.Round(double.Parse(DtDadosCR.GetValue("DescP",         pVal.Row - 1).ToString()), 2);

                    DtDadosCR.SetValue("Desconto", pVal.Row - 1, ((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value);
                    DtDadosCR.SetValue("Outros", pVal.Row - 1, ((EditText)MtDadosCR.Columns.Item("Col_19").Cells.Item(pVal.Row).Specific).Value);
                    DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, ((EditText)MtDadosCR.Columns.Item("Col_28").Cells.Item(pVal.Row).Specific).Value);
                    DtDadosCR.SetValue("OBS", pVal.Row - 1, ((EditText)MtDadosCR.Columns.Item("Col_31").Cells.Item(pVal.Row).Specific).Value);
                    DtDadosCR.SetValue("DescP", pVal.Row - 1, ((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value);

                    if (pVal.ColUID == "Col_28" &&  (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_28").Cells.Item(pVal.Row).Specific).Value)
                        >
                        double.Parse(((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value)))
                    {
                        Util.ExibeMensagensDialogoStatusBar("O valor a receber não pode ser maior que o líquido do documento, para esses casos, faça a baixa manualmente por meio da tela padrão do SAP!");
                        ((EditText)MtDadosCR.Columns.Item("Col_28").Cells.Item(pVal.Row).Specific).Value = ((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value;
                        ((EditText)MtDadosCR.Columns.Item("Col_19").Cells.Item(pVal.Row).Specific).Value = "0";
                        ((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value = "0";
                        ((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value = "0";                        
                    }
                    else
                    {
                        MtDadosCR.FlushToDataSource();
                        //-------------------------------------------------------------------------------------------------------------------------------------- Valor a pagar
                        if (pVal.ColUID == "Col_28" && (ValAReOri != Math.Round(double.Parse(DtDadosCR.GetValue("ValorAReceber", pVal.Row - 1).ToString()), 2)))
                        {
                            ((EditText)MtDadosCR.Columns.Item("Col_19").Cells.Item(pVal.Row).Specific).Value = "0";
                            ((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value = "0";
                            ((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value = "0";
                            ((EditText)MtDadosCR.Columns.Item("Col_28").Cells.Item(pVal.Row).Specific).Value = DtDadosCR.GetValue("ValorAReceber", pVal.Row - 1).ToString();
                        }

                        //-------------------------------------------------------------------------------------------------------------------------------------- Percentual de desconto!
                        if (pVal.ColUID == "Col_32" && (DescPOri != Math.Round(double.Parse(DtDadosCR.GetValue("DescP", pVal.Row - 1).ToString()), 2)))
                        {
                            if (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) > 100
                               ||
                               double.Parse(((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) < 0)
                            {
                                Util.ExibeMensagensDialogoStatusBar("Percentual de desconto inválido!");
                                return;
                            }

                            DtDadosCR.SetValue("Desconto", pVal.Row - 1, (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) / 100)
                                *
                                double.Parse(((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")));

                            DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) - (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_32").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) / 100)
                                *
                                double.Parse(((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value.Replace(".", ","))));

                            DtDadosCR.SetValue("Outros", pVal.Row - 1, double.Parse("0"));
                            MtDadosCR.CommonSetting.SetCellFontColor(pVal.Row, 31, RGB(0, 0, 0));

                        }
                        //-------------------------------------------------------------------------------------------------------------------------------------- Valor de desconto!                        
                        else if (pVal.ColUID == "Col_18" && (DescOri != Math.Round(double.Parse(DtDadosCR.GetValue("Desconto", pVal.Row - 1).ToString()), 2)))
                        {
                            if (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) > double.Parse(((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value.Replace(".", ","))
                               ||
                               double.Parse(((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) < 0)
                            {
                                Util.ExibeMensagensDialogoStatusBar("Valor de desconto inválido!");
                                return;
                            }

                            DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_33").Cells.Item(pVal.Row).Specific).Value.Replace(".", ","))
                                -
                                (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")))));

                            DtDadosCR.SetValue("DescP", pVal.Row - 1, (double.Parse(((EditText)MtDadosCR.Columns.Item("Col_18").Cells.Item(pVal.Row).Specific).Value.Replace(".", ",")) / double.Parse(((EditText)MtDadosCR.Columns.Item("Col_28").Cells.Item(pVal.Row).Specific).Value.Replace(".", ","))
                                *
                                100));

                            DtDadosCR.SetValue("Outros", pVal.Row - 1, double.Parse("0"));
                            MtDadosCR.CommonSetting.SetCellFontColor(pVal.Row, 31, RGB(0, 0, 0));

                        }
                        //-------------------------------------------------------------------------------------------------------------------------------------- Valor total pagamento
                        else if (pVal.ColUID == "Col_33" && (DescOri != Math.Round(double.Parse(DtDadosCR.GetValue("ValorAtual", pVal.Row - 1).ToString()), 2)))
                        {
                            DtDadosCR.SetValue("DescP", pVal.Row - 1, double.Parse("0"));
                            DtDadosCR.SetValue("Desconto", pVal.Row - 1, double.Parse("0"));
                            DtDadosCR.SetValue("Outros", pVal.Row - 1, double.Parse("0"));
                            MtDadosCR.CommonSetting.SetCellFontColor(pVal.Row, 31, RGB(0, 0, 0));

                            DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, Math.Round(double.Parse(DtDadosCR.GetValue("ValorAtual", pVal.Row - 1).ToString()), 2));
                        }
                        //-------------------------------------------------------------------------------------------------------------------------------------- Valor Outros
                        else if (pVal.ColUID == "Col_19" && (OutrosOri != Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2)))
                        {
                            DtDadosCR.SetValue("DescP", pVal.Row - 1, double.Parse("0"));
                            DtDadosCR.SetValue("Desconto", pVal.Row - 1, double.Parse("0"));

                            if (Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2) < 0 && pVal.Row > 0)
                            {
                                if (Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2) < (Math.Round(double.Parse(DtDadosCR.GetValue("ValorAtual", pVal.Row - 1).ToString()), 2) * -1))
                                {
                                    Util.ExibeMensagensDialogoStatusBar("Valor Outros inválido!", BoMessageTime.bmt_Short, false);
                                    DtDadosCR.SetValue("Outros", pVal.Row - 1, OutrosOri);
                                    return;
                                }
                                else
                                {
                                    DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, Math.Round(double.Parse(DtDadosCR.GetValue("ValorAtual", pVal.Row - 1).ToString()), 2) + Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2));
                                    MtDadosCR.CommonSetting.SetCellFontColor(pVal.Row, 31, RGB(255, 0, 0));
                                }
                            }
                            else if (Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2) > 0 && pVal.Row > 0)
                            {
                                DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, Math.Round(double.Parse(DtDadosCR.GetValue("ValorAtual", pVal.Row - 1).ToString()), 2) + Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2));
                                MtDadosCR.CommonSetting.SetCellFontColor(pVal.Row, 31, RGB(0, 128, 0));
                            }
                            else if (Math.Round(double.Parse(DtDadosCR.GetValue("Outros", pVal.Row - 1).ToString()), 2) == 0 && pVal.Row > 0)
                            {
                                DtDadosCR.SetValue("ValorAReceber", pVal.Row - 1, Math.Round(double.Parse(DtDadosCR.GetValue("ValorAtual", pVal.Row - 1).ToString()), 2));
                                MtDadosCR.CommonSetting.SetCellFontColor(pVal.Row, 31, RGB(0, 0, 0));
                            }
                        }

                        //MtDadosCR.FlushToDataSource();
                        MtDadosCR.LoadFromDataSourceEx();
                        //MtDadosCR.AutoResizeColumns();

                        Column oColumn = MtDadosCR.Columns.Item("Col_31");

                        if (oColumn.Width <= 150)
                            oColumn.Width = 150;
                        //new Task(() => { SomarValoresDocumentosSelecionados(); }).Start();
                        //SomarValoresDocumentosSelecionados();
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

        private void BtEfetivar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                iDtPag.Click();
                if (!Util.RetornarDialogo("Efetivar as parcelas selecionadas?"))
                    return;
                else
                {
                    int doc = 0;
                    SAPbobsCOM.Payments vPayGet = (SAPbobsCOM.Payments)SAPDbConnection.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                    if (vPayGet.GetByKey(doc))
                    {
                        vPayGet.SaveXML(@"c:\temp\cReceber\doc.xml");
                    }
                }

                if (string.IsNullOrEmpty(DtPag.Value))
                {
                    Util.ExibeMensagensDialogoStatusBar("Necessário informar a data do pagamento!", BoMessageTime.bmt_Short);
                    return;
                }

                //------------------------------------------------------------------------------------------------------------------------------- Baixando NOTAS no contas a receber!

                MtDadosCR.FlushToDataSource();

                var filteredRowsNf = from row in Enumerable.Range(0, DtDadosCR.Rows.Count)
                                     where DtDadosCR.GetValue("Seleciona", row).ToString() == "Y" && DtDadosCR.GetValue("TIPO", row).ToString() == "NFE"
                                     select new
                                     {
                                         OF = DtDadosCR.GetValue("OF", row).ToString(),
                                         ValorAReceber = DtDadosCR.GetValue("ValorAReceber", row).ToString(),
                                         DataPagto = DtPag.Value,
                                         TipoDoc = DtDadosCR.GetValue("TIPO", row).ToString(),
                                         CodLinDoc = DtDadosCR.GetValue("LINUN", row).ToString(),
                                         ValorLiq = DtDadosCR.GetValue("ValorLiquido", row).ToString(),
                                         Cliente = DtDadosCR.GetValue("Cliente", row).ToString(),
                                         CodDocR = DtDadosCR.GetValue("CodDocR", row).ToString(),
                                         ValorDoc = DtDadosCR.GetValue("ValorTitulo", row),
                                         Fatura = DtDadosCR.GetValue("Fatura", row).ToString(),
                                         Obra = DtDadosCR.GetValue("Obra", row).ToString(),
                                         Site = DtDadosCR.GetValue("Local", row).ToString(),
                                         Outros = DtDadosCR.GetValue("Outros", row).ToString(),
                                         Desc = DtDadosCR.GetValue("Desconto", row).ToString(),
                                         DescP = DtDadosCR.GetValue("DescP", row).ToString(),
                                         ValoAtual = DtDadosCR.GetValue("ValorAtual", row).ToString(),
                                         Parcela = DtDadosCR.GetValue("Parcela", row).ToString(),
                                         Obs = DtDadosCR.GetValue("OBS", row).ToString(),
                                         Filial = DtDadosCR.GetValue("Filial", row).ToString()
                                     };

                bool ControlLoopNF = true;
                bool erro = false;

                Util.ExibirMensagemStatusBar("Processo iniciado, aguarde!", BoMessageTime.bmt_Short, false);                

                List<string> OFUnicosNf = filteredRowsNf.Select(Item => Item.OF).Distinct().ToList();

                Util.ExibirMensagemStatusBar("Verificando notas selecionadas!", BoMessageTime.bmt_Short, false);

                foreach (var fatura in OFUnicosNf)
                {
                    #region DIAPI
                    var grupoMesmaFaturaNf = filteredRowsNf.Where(Item => Item.OF == fatura).ToList();

                    ControlLoopNF = false;
                    try
                    {
                        Util.ExibirMensagemStatusBar("Gerando documento: " + grupoMesmaFaturaNf[0].Fatura, BoMessageTime.bmt_Short);

                        SAPbobsCOM.Payments vPay = (SAPbobsCOM.Payments)SAPDbConnection.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                        vPay.CardCode = grupoMesmaFaturaNf[0].Cliente.ToString();// "103000";

                        vPay.BPLID = int.Parse(grupoMesmaFaturaNf[0].Filial.ToString());

                        vPay.DocType = BoRcptTypes.rCustomer;

                        vPay.TransferAccount = "11001004000002";

                        //vPay.TransferRealAmount = 0;                       

                        vPay.JournalRemarks = "FATURA " + grupoMesmaFaturaNf[0].Fatura.ToString() + " - " + grupoMesmaFaturaNf[0].Obra.ToString() + " - " + grupoMesmaFaturaNf[0].Site.ToString();

                        vPay.Remarks = grupoMesmaFaturaNf[0].Fatura.ToString() + (string.IsNullOrEmpty(grupoMesmaFaturaNf[0].Obs) ? "" : " - " + grupoMesmaFaturaNf[0].Obs);

                        vPay.ProjectCode = grupoMesmaFaturaNf[0].Obra.ToString();

                        DateTime DtPagto = DateTime.Parse(grupoMesmaFaturaNf[0].DataPagto.Substring(6, 2) + "/" + grupoMesmaFaturaNf[0].DataPagto.Substring(4, 2) + "/" + grupoMesmaFaturaNf[0].DataPagto.Substring(0, 4));

                        vPay.TaxDate = DtPagto;
                        vPay.DueDate = DtPagto;
                        vPay.DocDate = DtPagto;
                        vPay.TaxDate = DtPagto;

                        double TransFerSum = 0d;
                        double BankChargeA = 0d;

                        foreach (var row in grupoMesmaFaturaNf)
                        {
                            if (row != grupoMesmaFaturaNf[0])
                                vPay.Invoices.Add();

                            vPay.Invoices.DocLine = int.Parse(row.CodLinDoc.ToString());

                            vPay.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;

                            vPay.Invoices.DocEntry = int.Parse(row.CodDocR.ToString());

                            vPay.Invoices.TotalDiscount = double.Parse(row.Desc.ToString());

                            vPay.Invoices.InstallmentId = int.Parse(row.Parcela.ToString());                                                       

                            if (double.Parse(row.Outros) > 0)
                            {
                                vPay.Invoices.SumApplied = double.Parse(row.ValoAtual.ToString());
                                TransFerSum += double.Parse(row.ValorAReceber.ToString());
                            }
                            else
                            {
                                if(double.Parse(row.Desc.ToString()) == 0)
                                    vPay.Invoices.SumApplied = double.Parse(row.ValorAReceber.ToString());

                                if (double.Parse(row.Outros) < 0)
                                {
                                    TransFerSum += double.Parse(row.ValorAReceber.ToString());
                                    vPay.Invoices.SumApplied = double.Parse(row.ValorLiq.ToString());
                                }
                                else
                                {
                                    if (double.Parse(row.ValorAReceber.ToString()) < double.Parse(row.ValoAtual.ToString()))
                                        TransFerSum += double.Parse(row.ValorAReceber.ToString());
                                    else
                                        TransFerSum += double.Parse(row.ValorLiq.ToString());
                                }
                            }                            

                            if (double.Parse(row.Outros) < 0 || double.Parse(row.Desc) > 0)
                            {
                                if (double.Parse(row.Outros) > 0)
                                    BankChargeA += double.Parse(row.Outros);
                                else if (double.Parse(row.Outros) < 0)
                                    BankChargeA += double.Parse(row.Outros) * -1;
                            }                            

                        }

                        //vPay.TransferSum = double.Parse(grupoMesmaFaturaNf[0].ValorAReceber.ToString());
                        vPay.TransferSum = TransFerSum;
                        vPay.BankChargeAmount = BankChargeA;

                        int Resultado = vPay.Add();

                        if (Resultado != 0)
                        {
                            var temp = SAPDbConnection.oCompany.GetLastErrorDescription();
                            MessageBox.Show(temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    //Payments oPayment = (Payments)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.oPaymentsDrafts);

                    #endregion

                    #region N8N API

                    //ControlLoopNF = false;

                    //var options = new RestClientOptions("http://srvetl:5678")
                    //{
                    //    MaxTimeout = -1,
                    //};
                    //var client = new RestClient(options);
                    //var request = new RestRequest("/webhook/contasrecebernf", Method.Post);
                    ////var request = new RestRequest("/webhook-test/contasrecebernf", Method.Post);
                    //request.AddHeader("Content-Type", "application/json");

                    //request.AddStringBody(JsonConvert.SerializeObject(new BodyCRN8NNF
                    //{
                    //    CompanyDB = Globals.Master.Connection.Database.CompanyDB,
                    //    TipoDoc = row.TipoDoc.ToString(), //NFE ou LCM
                    //    CodDoc = row.OF.ToString(),
                    //    DataPagto = row.DataPagto.ToString(),
                    //    ValorPagto = row.ValorAReceber.ToString(),
                    //    CodLinDoc = row.CodLinDoc.ToString(),
                    //    ValorLiq = row.ValorLiq.ToString(),
                    //    Cliente = row.Cliente.ToString(),
                    //    CodDocR = row.CodDocR.ToString(),
                    //    ValorDoc = row.ValorDoc.ToString(),
                    //    Fatura = row.Fatura.ToString(),
                    //    Obra = row.Obra.ToString(),
                    //    Site = row.Site.ToString(),
                    //    Outros = row.Outros.ToString(),
                    //    Desc = row.Desc.ToString(),
                    //    DescP = row.DescP.ToString(),
                    //    ValoAtual = row.ValoAtual.ToString(),
                    //    Parcela = row.Parcela.ToString()

                    //}), DataFormat.Json);

                    //RestResponse response = client.Execute(request);

                    //if (response.StatusCode.ToString().Contains("NotFound"))
                    //{
                    //    Util.ExibirMensagemStatusBar("Falha ao acessar a API, comunique o departamento de TI!", BoMessageTime.bmt_Short, true);
                    //    return;
                    //}
                    //else if (response.StatusCode.ToString().Contains("OK"))
                    //{
                    //    RetResponse ret = JsonConvert.DeserializeObject<RetResponse>(response.Content);
                    //    Util.ExibirMensagemStatusBar(ret.MSG, BoMessageTime.bmt_Short, !ret.CODE.Equals("1"));
                    //    //erro = !ret.CODE.Equals("1");
                    //}
                    #endregion
                }                              

                //------------------------------------------------------------------------------------------------------------------------------- Baixando LCM no contas a receber!

                var filteredRowsLc = from row in Enumerable.Range(0, DtDadosCR.Rows.Count)
                                     where DtDadosCR.GetValue("Seleciona", row).ToString() == "Y" && DtDadosCR.GetValue("TIPO", row).ToString() == "LCM"
                                     select new
                                     {
                                         CompanyDB = Globals.Master.Connection.Database.CompanyDB,
                                         CodDoc = DtDadosCR.GetValue("OF", row).ToString(),
                                         ValorAReceber = DtDadosCR.GetValue("ValorAReceber", row).ToString(),
                                         DataPagto = DtPag.Value,
                                         TipoDoc = DtDadosCR.GetValue("TIPO", row).ToString(),
                                         CodLinDoc = DtDadosCR.GetValue("LINUN", row).ToString(),
                                         ValorLiq = DtDadosCR.GetValue("ValorLiquido", row).ToString(),
                                         Cliente = DtDadosCR.GetValue("Cliente", row).ToString(),
                                         CodDocR = DtDadosCR.GetValue("CodDocR", row).ToString(),
                                         ValorDoc = DtDadosCR.GetValue("ValorTitulo", row).ToString(),
                                         Fatura = DtDadosCR.GetValue("Fatura", row).ToString(),
                                         Obra = DtDadosCR.GetValue("Obra", row).ToString(),
                                         Site = DtDadosCR.GetValue("Local", row).ToString(),
                                         Outros = DtDadosCR.GetValue("Outros", row).ToString(),
                                         Desc = DtDadosCR.GetValue("Desconto", row).ToString(),
                                         DescP = DtDadosCR.GetValue("DescP", row).ToString(),
                                         ValoAtual = DtDadosCR.GetValue("ValorAtual", row).ToString(),
                                         ValorReal = double.Parse(DtDadosCR.GetValue("ValorTitulo", row).ToString()),
                                         Filial = DtDadosCR.GetValue("Filial", row).ToString()
                                     };

                bool ControlLoopLC = true;
                erro = false;

                List<string> valoresUnicosLc = filteredRowsLc.Select(Item => Item.Fatura).Distinct().ToList();

                Util.ExibirMensagemStatusBar("Verificando lançamentos selecionados!", BoMessageTime.bmt_Short, false);

                double valorSomado = 0;

                foreach (string valu in valoresUnicosLc)
                {
                    var grupoMesmaFatura = filteredRowsLc.Where(Item => Item.Fatura == valu).ToList();
                    ControlLoopLC = false;
                    #region DIAPI
                    try
                    {
                        Util.ExibirMensagemStatusBar("Gerando documento: " + grupoMesmaFatura[0].Fatura, BoMessageTime.bmt_Short);

                        SAPbobsCOM.Payments vPay = (SAPbobsCOM.Payments)SAPDbConnection.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                        vPay.CardCode = grupoMesmaFatura[0].Cliente.ToString();// "103000";

                        vPay.BPLID = int.Parse(grupoMesmaFatura[0].Filial.ToString());

                        vPay.DocType = BoRcptTypes.rCustomer;

                        vPay.TransferAccount = "11001004000002";                                              

                        vPay.TransferRealAmount = 0;

                        vPay.Remarks = grupoMesmaFatura[0].Fatura.ToString();

                        vPay.ProjectCode = grupoMesmaFatura[0].Obra.ToString();

                        vPay.JournalRemarks = "FATURA " + grupoMesmaFatura[0].Fatura.ToString() + " - " + grupoMesmaFatura[0].Obra.ToString() + " - " + grupoMesmaFatura[0].Site.ToString();

                        DateTime DtPagto = DateTime.Parse(grupoMesmaFatura[0].DataPagto.Substring(6, 2) + "/" + grupoMesmaFatura[0].DataPagto.Substring(4, 2) + "/" + grupoMesmaFatura[0].DataPagto.Substring(0, 4));

                        vPay.TaxDate = DtPagto;
                        vPay.DueDate = DtPagto;
                        vPay.DocDate = DtPagto;
                        vPay.TaxDate = DtPagto;

                        double TransFerSum = 0d;
                        double BankChargeA = 0d;
                        double TotDesconto = 0d;

                        foreach (var row in grupoMesmaFatura)
                        {
                            if (row != grupoMesmaFatura[0])
                                vPay.Invoices.Add();

                            vPay.Invoices.DocLine = int.Parse(row.CodLinDoc.ToString());

                            vPay.Invoices.InvoiceType = BoRcptInvTypes.it_JournalEntry;

                            vPay.Invoices.DocEntry = int.Parse(row.CodDocR.ToString());

                            TotDesconto += double.Parse(row.Desc.ToString());     

                            if (double.Parse(row.Outros) > 0)
                            {
                                vPay.Invoices.SumApplied = double.Parse(row.ValoAtual.ToString());
                                TransFerSum += double.Parse(row.ValorAReceber.ToString());
                            }
                            else
                            {
                                if (double.Parse(row.Desc.ToString()) == 0)
                                    vPay.Invoices.SumApplied = double.Parse(row.ValorAReceber.ToString());

                                if (double.Parse(row.Outros) < 0)
                                {
                                    TransFerSum += double.Parse(row.ValorAReceber.ToString());
                                    vPay.Invoices.SumApplied = double.Parse(row.ValorLiq.ToString());
                                }
                                else
                                {
                                    if (double.Parse(row.ValorAReceber.ToString()) < double.Parse(row.ValoAtual.ToString()))
                                        TransFerSum += double.Parse(row.ValorAReceber.ToString());
                                    else
                                        TransFerSum += double.Parse(row.ValorLiq.ToString());
                                }
                            }
                           
                            /*
                             * Para que a baixa tenha ganho eventuais, não pode passar o valor do campo outros para o BankCharge.
                             */
                            if (double.Parse(row.Outros.ToString()) < 0 || double.Parse(row.Desc.ToString()) > 0)
                            {
                                if (double.Parse(row.Outros) > 0)
                                    BankChargeA += double.Parse(row.Outros);
                                else
                                    BankChargeA += double.Parse(row.Outros) * -1;
                            }
                        }

                        //vPay.TransferSum = double.Parse(grupoMesmaFaturaNf[0].ValorAReceber.ToString());

                        //if (TotDesconto > 0)
                        //    TransFerSum = TransFerSum - TotDesconto;

                        vPay.TransferSum = TransFerSum;
                        vPay.BankChargeAmount = BankChargeA;

                        int Resultado = vPay.Add();

                        if (Resultado != 0)
                        {
                            var temp = SAPDbConnection.oCompany.GetLastErrorDescription();
                            MessageBox.Show(temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    #endregion

                    #region N8N
                    //ControlLoopLC = false;

                    //var options = new RestClientOptions("http://srvetl:5678")
                    //{
                    //    MaxTimeout = -1,
                    //};
                    //var client = new RestClient(options);
                    //var request = new RestRequest("/webhook/contasreceberlc", Method.Post);
                    ////var request = new RestRequest("/webhook-test/contasreceberlc", Method.Post);
                    ////var request = new RestRequest("/webhook-test/testelc", Method.Post);
                    //request.AddHeader("Content-Type", "application/json");
                    //request.AddStringBody(JsonConvert.SerializeObject(grupoMesmaFatura), DataFormat.Json);     

                    //RestResponse response = client.Execute(request);

                    //if (response.StatusCode.ToString().Contains("NotFound"))
                    //{
                    //    Util.ExibirMensagemStatusBar("Falha ao acessar a API, comunique o departamento de TI!", BoMessageTime.bmt_Short, true);
                    //    return;
                    //}
                    //else if (response.StatusCode.ToString().Contains("OK"))
                    //{
                    //    RetResponse ret = JsonConvert.DeserializeObject<RetResponse>(response.Content);
                    //    Util.ExibirMensagemStatusBar(ret.MSG, BoMessageTime.bmt_Short, !ret.CODE.Equals("1"));
                    //    //erro = !ret.CODE.Equals("1");
                    //}
                    #endregion
                    //}
                }

                if (ControlLoopNF && ControlLoopLC)
                    Util.ExibeMensagensDialogoStatusBar("Selecione os documentos a serem efetivados!");
                else
                {
                    PesquisarDadosPagamento();
                    Util.ExibirMensagemStatusBar("Processo concluido!", BoMessageTime.bmt_Short, false);
                }
            }
            catch (Exception ex)
            {
                var tempRet = ex.Message;
            }
            //finally
            //{
            //    PesquisarDadosPagamento();
            //}
        }

        private void CkbLinhas_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                oForm.Freeze(true);

                for (int iRow = 0; iRow < DtDadosCR.Rows.Count; iRow++)
                    DtDadosCR.SetValue("Seleciona", iRow, CkbLinhas.Checked ? "N" : "Y");

                //MtDadosCR.FlushToDataSource();
                MtDadosCR.LoadFromDataSourceEx();
                MtDadosCR.AutoResizeColumns();

                Column oColumn = MtDadosCR.Columns.Item("Col_31");

                if (oColumn.Width <= 150)
                    oColumn.Width = 150;

                SomarValoresDocumentosSelecionados();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void CkBoxMatrixCR_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if ((pVal.CharPressed == 0 || pVal.CharPressed == 32) && pVal.ColUID == "Col_1")
            {
                try
                {
                    oForm.Freeze(true);
                    //DtDadosCR.SetValue("Seleciona", pVal.Row - 1, ((CheckBox)MtDadosCR.Columns.Item("Col_1").Cells.Item(pVal.Row).Specific).Checked ? "Y" : "N");

                    MtDadosCR.FlushToDataSource();                    
                    //MtDadosCR.LoadFromDataSourceEx();
                    //MtDadosCR.AutoResizeColumns();

                    Column oColumn = MtDadosCR.Columns.Item("Col_31");

                    if (oColumn.Width <= 150)
                        oColumn.Width = 150;

                    //new Task(() => { SomarValoresDocumentosSelecionados(); }).Start();
                    SomarValoresDocumentosSelecionados();
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

        private void BtFiltrar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            PesquisarDadosPagamento();
        }

        private void PesquisarDadosPagamento()
        {
            try
            {
                oForm.Freeze(true);

                CkbLinhas.Checked = false;

                string dataInicial = !string.IsNullOrEmpty(EdDataI.Value) ? EdDataI.Value : "20200101";
                string dataFinal = !string.IsNullOrEmpty(EdDataF.Value) ? EdDataF.Value : "20500101";
                string tipoData = !string.IsNullOrEmpty(CbTpDt.Value) ? CbTpDt.Value : "V";


                //Caso nada tenha sido informado com filtro
                if (string.IsNullOrEmpty(EdDataI.Value) && string.IsNullOrEmpty(EdDataF.Value)
                    && string.IsNullOrEmpty(EdFat.Value) && string.IsNullOrEmpty(EdObra.Value)
                    && string.IsNullOrEmpty(EdPO.Value) && string.IsNullOrEmpty(EdCardCode.Value)
                    && string.IsNullOrEmpty(EdCardName.Value) && string.IsNullOrEmpty(EdCont.Value))
                {
                    dataInicial = DateTime.Now.ToString("yyyyMM01");
                    dataFinal   = DateTime.Now.ToString("yyyyMMdd");
                }

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
                MtDadosCR.Columns.Item("Col_28").DataBind.Bind("DtItm", "ValorAReceber");
                MtDadosCR.Columns.Item("Col_29").DataBind.Bind("DtItm", "LINUN");
                MtDadosCR.Columns.Item("Col_30").DataBind.Bind("DtItm", "CodDocR");
                MtDadosCR.Columns.Item("Col_31").DataBind.Bind("DtItm", "OBS");
                MtDadosCR.Columns.Item("Col_32").DataBind.Bind("DtItm", "DescP");
                MtDadosCR.Columns.Item("Col_33").DataBind.Bind("DtItm", "ValorAtual");
                MtDadosCR.Columns.Item("Col_34").DataBind.Bind("DtItm", "Filial");

                MtDadosCR.LoadFromDataSourceEx();
                MtDadosCR.AutoResizeColumns();

                Column oColumn = MtDadosCR.Columns.Item("Col_31");

                if (oColumn.Width <= 150)
                    oColumn.Width = 150;
                //LinhasSelecionadas = new List<int>();
                oForm.Freeze(false);
                SomarValoresDocumentosGeral();
                SomarValoresDocumentosSelecionados();                

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao pesquisar dados de contas a receber: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void SomarValoresDocumentosGeral()
        {
            oForm.Freeze(true);

            try
            {
                double totalPagto = 0;
                double totalBruto = 0;
                double totalLiquido = 0;
                double totalConfins = 0;
                double totalCSLL = 0;
                double totalIRRF = 0;
                double totalPIS = 0;
                double totalINSS = 0;
                double totalISS = 0;

                for (int iRow = 0; iRow < DtDadosCR.Rows.Count; iRow++)
                {
                    totalPagto += Convert.ToDouble(DtDadosCR.GetValue("ValorTitulo", iRow));
                    totalBruto += Convert.ToDouble(DtDadosCR.GetValue("ValorTitulo", iRow));
                    totalLiquido += Convert.ToDouble(DtDadosCR.GetValue("ValorLiquido", iRow));
                    totalConfins += Convert.ToDouble(DtDadosCR.GetValue("COFINS", iRow));
                    totalCSLL += Convert.ToDouble(DtDadosCR.GetValue("CSLL", iRow));
                    totalIRRF += Convert.ToDouble(DtDadosCR.GetValue("IRRF", iRow));
                    totalPIS += Convert.ToDouble(DtDadosCR.GetValue("PIS", iRow));
                    totalINSS += Convert.ToDouble(DtDadosCR.GetValue("INSS", iRow));
                    totalISS += Convert.ToDouble(DtDadosCR.GetValue("ISS", iRow));
                }

                oForm.DataSources.UserDataSources.Item("TotalPagG").ValueEx = totalPagto.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("TotalBrtG").ValueEx = totalBruto.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("TotalLiqG").ValueEx = totalLiquido.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("CofinsG").ValueEx = totalConfins.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("CsllG").ValueEx = totalCSLL.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("IrrfG").ValueEx = totalIRRF.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("PisG").ValueEx = totalPIS.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("InssG").ValueEx = totalINSS.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("IssG").ValueEx = totalISS.ToString().Replace(".", "").Replace(",", ".");

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao somar Parcelas {Ex.Message}", BoMessageTime.bmt_Medium, true,
                    Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void SomarValoresDocumentosSelecionados()
        {
            //oForm.Freeze(true);

            try
            {
                double totalPagto = 0;
                double totalBruto = 0;
                double totalLiquido = 0;
                double totalConfins = 0;
                double totalCSLL = 0;
                double totalIRRF = 0;
                double totalPIS = 0;
                double totalINSS = 0;
                double totalISS = 0;

                var filteredRows = from row in Enumerable.Range(0, DtDadosCR.Rows.Count)
                                   where DtDadosCR.GetValue("Seleciona", row).ToString() == "Y"
                                   select new
                                   {
                                       ValorTitulo = DtDadosCR.GetValue("ValorTitulo", row),
                                       ValorLiquido = DtDadosCR.GetValue("ValorLiquido", row),
                                       COFINS = DtDadosCR.GetValue("COFINS", row),
                                       CSLL = DtDadosCR.GetValue("CSLL", row),
                                       IRRF = DtDadosCR.GetValue("IRRF", row),
                                       PIS = DtDadosCR.GetValue("PIS", row),
                                       INSS = DtDadosCR.GetValue("INSS", row),
                                       ISS = DtDadosCR.GetValue("ISS", row)
                                   };

                foreach (var row in filteredRows)
                {
                    totalPagto += Convert.ToDouble(row.ValorTitulo);
                    totalBruto += Convert.ToDouble(row.ValorTitulo);
                    totalLiquido += Convert.ToDouble(row.ValorLiquido);
                    totalConfins += Convert.ToDouble(row.COFINS);
                    totalCSLL += Convert.ToDouble(row.CSLL);
                    totalIRRF += Convert.ToDouble(row.IRRF);
                    totalPIS += Convert.ToDouble(row.PIS);
                    totalINSS += Convert.ToDouble(row.INSS);
                    totalISS += Convert.ToDouble(row.ISS);
                }

                oForm.DataSources.UserDataSources.Item("TotalPag").ValueEx = totalPagto.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("TotalBruto").ValueEx = totalBruto.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("TotalLiq").ValueEx = totalLiquido.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("Cofins").ValueEx = totalConfins.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("Csll").ValueEx = totalCSLL.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("Irrf").ValueEx = totalIRRF.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("Pis").ValueEx = totalPIS.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("Inss").ValueEx = totalINSS.ToString().Replace(".", "").Replace(",", ".");
                oForm.DataSources.UserDataSources.Item("Iss").ValueEx = totalISS.ToString().Replace(".", "").Replace(",", ".");

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao somar Parcelas {Ex.Message}", BoMessageTime.bmt_Medium, true,
                    Ex);
            }
            finally
            {
                //semafaro = true;
                //oForm.Freeze(false);
            }
        }

        public class BodyCRN8NNF
        {
            public string CompanyDB { get; set; }
            public string TipoDoc { get; set; }
            public string CodDoc { get; set; }
            public string DataPagto { get; set; }
            public string ValorPagto { get; set; }
            public string CodLinDoc { get; set; }
            public string ValorLiq { get; set; }
            public string Cliente { get; set; }
            public string CodDocR { get; set; }
            public string ValorDoc { get; set; }
            public string Fatura { get; set; }
            public string Obra { get; set; }
            public string Site { get; set; }
            public string Outros { get; set; }
            public string Desc { get; set; }
            public string DescP { get; set; }
            public string ValoAtual { get; set; }
            public double ValorReal { get; set; }
            public string Parcela { get; set; }
        }

        public class BodyCRN8NLCCAB
        {
            public string CompanyDB { get; set; }
            public string CodDoc { get; set; }
            public string DataPagto { get; set; }
            public string Cliente { get; set; }
            public string Obra { get; set; }
            public string Site { get; set; }
            public List<BodyCRN8NLCLIN> PaymentInvoices { get; set; }
        }

        public class BodyCRN8NLCLIN
        {
            public string TipoDoc { get; set; }
            public string CodLinDoc { get; set; }
            public string CodDocR { get; set; }
            public string Fatura { get; set; }
            public string Outros { get; set; }
            public string Desc { get; set; }
            public string DescP { get; set; }
            public string ValoAtual { get; set; }
            public string ValorDoc { get; set; }
            public string ValorLiq { get; set; }
            public string ValorPagto { get; set; }
        }

        public class RetResponse
        {
            public string CODE { get; set; }
            public string MSG { get; set; }
        }
    }
}
