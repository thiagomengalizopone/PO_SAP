using sap.dev.core;
using sap.dev.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.PO
{
    public partial class FrmImportacaoPO : Form
    {
        private static Thread formThread;
        public DataTable dtRegistros { get; set; }

        public Int32 BPLId { get; set; }
        public FrmImportacaoPO()
        {
            InitializeComponent();
        }

        internal static void MenuImpPO()
        {
            formThread = new Thread(new ThreadStart(OpenFormImportacaoPO));
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
        }

        private static void OpenFormImportacaoPO() => System.Windows.Forms.Application.Run(new FrmImportacaoPO());

        private void BtImportar_Click(object sender, EventArgs e)
        {

            if (CbEmpresa.Text == "Huawei")
                ImportarPOHuawei();
        }

        private void ImportarPOHuawei()
        {
            try
            {
                if (dtRegistros.Rows.Count == 0)
                {
                    MessageBox.Show("Não há registros para importação!");
                    return;
                }

                if (MessageBox.Show($@"Deseja prosseguir com a importação? Há um total de {dtRegistros.Rows.Count} registros!",
                    "Atenção!",
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question ) == DialogResult.No)
                    return;


                BPLId = Convert.ToInt32(SqlUtils.GetValue("SELECT MIN(BPLId) FROM OBPL WHERE Disabled = 'N'"));

                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                if (ConfiguracoesImportacaoPO.TipoDocumentoPO == "E")
                {
                    oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                    oPedidoVenda.DocObjectCodeEx = "17";
                }

                pbProgresso.Value = 0;
                pbProgresso.Maximum = dtRegistros.Rows.Count;

                for (int iPedido = 0; iPedido < dtRegistros.Rows.Count; iPedido++)
                {
                    try
                    {
                        pbProgresso.Value += 1;


                        if (dgDadosPO.Rows[iPedido].Cells["Importar"].Value == null || dgDadosPO.Rows[iPedido].Cells["Importar"].Value.ToString() != "1")
                            continue;

                        oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                        if (ConfiguracoesImportacaoPO.TipoDocumentoPO == "E")
                        {
                            oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                            oPedidoVenda.DocObjectCodeEx = "17";
                        }

                        oPedidoVenda.CardCode = ConfiguracoesImportacaoPO.CardCodePO;
                        oPedidoVenda.DocDate = DateTime.Now;
                        oPedidoVenda.DocDueDate = DateTime.Now;
                        oPedidoVenda.NumAtCard = dtRegistros.Rows[iPedido]["poNumber"].ToString();
                        oPedidoVenda.UserFields.Fields.Item("U_IdPO").Value = dtRegistros.Rows[iPedido]["po_id"].ToString();

                        string SQL = $@"SP_ZPN_IMPORTARPOHuaweiItens '{dtRegistros.Rows[iPedido]["po_id"].ToString()}'";

                        DataTable dtRegistrosItens = SqlUtils.ExecuteCommand(SQL);

                        for (int iPedidoLinha = 0; iPedidoLinha < dtRegistrosItens.Rows.Count; iPedidoLinha++)
                        {
                            if (!string.IsNullOrEmpty(oPedidoVenda.Lines.ItemCode))
                                oPedidoVenda.Lines.Add();

                            if (!string.IsNullOrEmpty(dtRegistrosItens.Rows[iPedidoLinha]["Filial"].ToString()))
                            {
                                if (Convert.ToInt32(dtRegistrosItens.Rows[iPedidoLinha]["Filial"].ToString()) > 0)
                                    BPLId = Convert.ToInt32(dtRegistrosItens.Rows[iPedidoLinha]["Filial"].ToString());
                            }

                            oPedidoVenda.Lines.ItemCode = ConfiguracoesImportacaoPO.ItemCodePO;
                            oPedidoVenda.Lines.Quantity = Convert.ToDouble(dtRegistrosItens.Rows[iPedidoLinha]["quantity"]);
                            oPedidoVenda.Lines.Price = Convert.ToDouble(dtRegistrosItens.Rows[iPedidoLinha]["unitPrice"]);
                            
                            oPedidoVenda.Lines.UserFields.Fields.Item("U_itemDescription").Value = dtRegistrosItens.Rows[iPedidoLinha]["itemDescription"].ToString();
                            oPedidoVenda.Lines.UserFields.Fields.Item("U_manSiteInfo").Value = dtRegistrosItens.Rows[iPedidoLinha]["manufactureSiteInfo"].ToString();
                            
                            oPedidoVenda.BPL_IDAssignedToInvoice = BPLId;
                        }

                        if (oPedidoVenda.Add() != 0)
                            throw new Exception($"{Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }
                    catch (Exception Ex)
                    {
                        SqlUtils.DoNonQuery($@"ZPN_SP_LOGIMPORTACAOPO {dtRegistros.Rows[iPedido]["po_id"].ToString()}, '{Ex.Message}'");
                    }

                }

                MessageBox.Show("PO Importadas com sucesso! Verifique o log de importações!");
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao importar dados PO - {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void BtnPesquisar_Click(object sender, EventArgs e)
        {
            if (CbEmpresa.Text == "Huawei")
                PesquisarDadosHuawei();
            else if (CbEmpresa.Text == "Ericsson")
                PesquisarDadosEricssonAsync();
        }

        private async Task PesquisarDadosEricssonAsync()
        {
            try
            {
                string fileNameAnexo = await Util.OpenFileDialogAsync(EnumList.TipoArquivo.CSV);

                if (!string.IsNullOrEmpty(fileNameAnexo))
                {
                    using (var arquivoEricsson = new StreamReader(fileNameAnexo))
                    {
                        pbProgresso.Value = 0;
                        pbProgresso.Maximum = arquivoEricsson.cou

                        while (!arquivoEricsson.EndOfStream)
                        {
                            
                            var valores = arquivoEricsson.ReadLine().Split(';');

                            if (valores.Length == 13)
                            {
                                if (Int64.TryParse(valores[2].Trim(), out Int64 PO))
                                {
                                    string SQL = $@"ZPN_SP_POERICSSON 
                                                            '{fileNameAnexo}',
                                                            {PO}, 
                                                            '{valores[3].Trim()}', 
                                                            '{valores[4].Trim()}', 
                                                            '{valores[5].Trim()}', 
                                                            {valores[6].Trim().Replace(".", "").Replace(",", ".")}, 
                                                            '{valores[7].Trim()}', 
                                                            '{valores[8].Trim()}', 
                                                            {valores[9].Trim().Replace(".", "").Replace(",", ".")}, 
                                                            '{valores[10].Trim()}', 
                                                            'N' ";

                                    SqlUtils.DoNonQuery(SQL);

                                }
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("Não há arquivo selecionado!");

            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados importação PO - Ericsson - {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }

        }

        private void PesquisarDadosHuawei()
        {
            try
            {
                string dataInicial = (mskDataI.MaskFull ? Convert.ToDateTime(mskDataI.Text) : DateTime.MinValue).ToString("yyyyMMdd");
                string dataFinal = (mskDataF.MaskFull ? Convert.ToDateTime(mskDataF.Text) : DateTime.MaxValue).ToString("yyyyMMdd");


                string SQL = $@"SP_ZPN_IMPORTARPOHuawei '{dataInicial}', '{dataFinal}', 'N'";

                string pedidoVenda = string.Empty;

                dtRegistros = SqlUtils.ExecuteCommand(SQL);

                dgDadosPO.DataSource = dtRegistros;

                dgDadosPO.AutoResizeColumns();

                dgDadosPO.Columns[0].ReadOnly = false;
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados importação PO - Huawei - {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void FrmImportacaoPO_Load(object sender, EventArgs e)
        {

        }
    }
}
