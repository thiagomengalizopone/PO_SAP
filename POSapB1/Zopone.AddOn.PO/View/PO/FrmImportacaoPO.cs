using sap.dev.core;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.PO
{
    public partial class FrmImportacaoPO : Form
    {
        private static Thread formThread;
        public DataTable DtRegistros { get; set; }

        public int BPLId { get; set; }

        public FrmImportacaoPO()
        {
            InitializeComponent();
        }

        internal static void MenuImpPO()
        {
            formThread = new Thread(OpenFormImportacaoPO);
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
        }

        private static void OpenFormImportacaoPO() => Application.Run(new FrmImportacaoPO());

        private void BtImportar_Click(object sender, EventArgs e)
        {
            IImportacaoService importacaoService = ImportacaoServiceFactory.CreateImportacaoService(CbEmpresa.Text);
            importacaoService.Importar(DtRegistros, BPLId, pbProgresso, dgDadosPO, CbEmpresa.Text);
        }

        private void BtnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtRegistros = new DataTable();
                IPesquisaService pesquisaService = PesquisaServiceFactory.CreatePesquisaService(CbEmpresa.Text);
                pesquisaService.Pesquisar(mskDataI, mskDataF, dgDadosPO, pbProgresso,  out dtRegistros);

                DtRegistros = dtRegistros;
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao Pesquisar dados -  {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void FrmImportacaoPO_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.BringToFront();
            this.TopMost = false;
        }
    }

    public interface IImportacaoService
    {
        void Importar(DataTable dtRegistros, int bplId, ProgressBar pbProgresso, DataGridView dgDadosPO, string Empresa);
    }

    public class ImportacaoService : IImportacaoService
    {
        public void Importar(DataTable dtRegistros, int bplId, ProgressBar pbProgresso, DataGridView dgDadosPO, string Empresa)
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
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;

                bplId = Convert.ToInt32(SqlUtils.GetValue("SELECT MIN(BPLId) FROM OBPL WHERE Disabled = 'N'"));

                SAPbobsCOM.Documents oPedidoVenda = CreatePedidoVenda();

                pbProgresso.Value = 0;
                pbProgresso.Maximum = dtRegistros.Rows.Count;

                for (int iPedido = 0; iPedido < dtRegistros.Rows.Count; iPedido++)
                {
                    try
                    {
                        pbProgresso.Value += 1;

                        if (dgDadosPO.Rows[iPedido].Cells["Importar"].Value == null || dgDadosPO.Rows[iPedido].Cells["Importar"].Value.ToString() != "true")
                            continue;

                        oPedidoVenda = CreatePedidoVenda();

                        PopulatePedidoVenda(dtRegistros, iPedido, oPedidoVenda, bplId, Empresa);

                        if (oPedidoVenda.Add() != 0)
                            throw new Exception($"PN: {oPedidoVenda.CardCode} - {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }
                    catch (Exception ex)
                    {
                        LogImportacaoErro(dtRegistros, iPedido, ex);
                    }
                }

                FrmVerificaImportacaoPO.MenuVerificaPO();
            }
            catch (Exception ex)
            {
                HandleImportacaoException("Huawei", ex);
            }
        }

        private static SAPbobsCOM.Documents CreatePedidoVenda()
        {
            var oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            if (ConfiguracoesImportacaoPO.TipoDocumentoPO == "E")
            {
                oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                oPedidoVenda.DocObjectCodeEx = "17";
            }

            return oPedidoVenda;
        }

        private static void PopulatePedidoVenda(DataTable dtRegistros, int iPedido, SAPbobsCOM.Documents oPedidoVenda, int bplId, string Empresa)
        {
            oPedidoVenda.CardCode = ConfiguracoesImportacaoPO.CardCodePOHawuey;
            oPedidoVenda.DocDate = DateTime.Now;
            oPedidoVenda.DocDueDate = DateTime.Now;
            oPedidoVenda.NumAtCard = dtRegistros.Rows[iPedido]["poNumber"].ToString();
            oPedidoVenda.UserFields.Fields.Item("U_IdPO").Value = Convert.ToDouble(dtRegistros.Rows[iPedido]["po_id"]);

            string SQL = string.Empty;

            if (Empresa == "Huawei")
            {
                oPedidoVenda.CardCode = ConfiguracoesImportacaoPO.CardCodePOHawuey;

                SQL = $@"SP_ZPN_IMPORTARPOHuaweiItens '{dtRegistros.Rows[iPedido]["po_id"].ToString()}'";
            }
            else if (Empresa == "Ericsson")
            {
                oPedidoVenda.CardCode = ConfiguracoesImportacaoPO.CardCodePOEricsson;

                SQL = $@"SP_ZPN_IMPORTARPOERICSSONItens '{dtRegistros.Rows[iPedido]["po_id"].ToString()}'";
            }

            DataTable dtRegistrosItens = SqlUtils.ExecuteCommand(SQL);

            for (int iPedidoLinha = 0; iPedidoLinha < dtRegistrosItens.Rows.Count; iPedidoLinha++)
            {
                if (!string.IsNullOrEmpty(oPedidoVenda.Lines.ItemCode))
                    oPedidoVenda.Lines.Add();

                if (!string.IsNullOrEmpty(dtRegistrosItens.Rows[iPedidoLinha]["Filial"].ToString()))
                {
                    if (Convert.ToInt32(dtRegistrosItens.Rows[iPedidoLinha]["Filial"].ToString()) > 0)
                        bplId = Convert.ToInt32(dtRegistrosItens.Rows[iPedidoLinha]["Filial"].ToString());
                }

                oPedidoVenda.Lines.ItemCode = ConfiguracoesImportacaoPO.ItemCodePO;
                oPedidoVenda.Lines.Quantity = Convert.ToDouble(dtRegistrosItens.Rows[iPedidoLinha]["quantity"]);
                oPedidoVenda.Lines.Price = Convert.ToDouble(dtRegistrosItens.Rows[iPedidoLinha]["unitPrice"]);

                if (!string.IsNullOrEmpty(dtRegistrosItens.Rows[iPedidoLinha]["IdObra"].ToString()))
                    oPedidoVenda.Lines.ProjectCode = dtRegistrosItens.Rows[iPedidoLinha]["IdObra"].ToString();

                if (Convert.ToInt32(dtRegistrosItens.Rows[iPedidoLinha]["U_CodContrato"]) > 0)
                    oPedidoVenda.Lines.AgreementNo = Convert.ToInt32(dtRegistrosItens.Rows[iPedidoLinha]["U_CodContrato"]);

                oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value = dtRegistrosItens.Rows[iPedidoLinha]["ITEM"].ToString();

                if (Empresa == "Ericsson")
                {
                    oPedidoVenda.Lines.FreeText = dtRegistrosItens.Rows[iPedidoLinha]["SITE"].ToString();
                }

                
                oPedidoVenda.Lines.UserFields.Fields.Item("U_itemDescription").Value = dtRegistrosItens.Rows[iPedidoLinha]["itemDescription"].ToString();
                oPedidoVenda.Lines.UserFields.Fields.Item("U_manSiteInfo").Value = dtRegistrosItens.Rows[iPedidoLinha]["manufactureSiteInfo"].ToString();


                oPedidoVenda.BPL_IDAssignedToInvoice = bplId;
            }
        }

        private static void LogImportacaoErro(DataTable dtRegistros, int iPedido, Exception ex)
        {
            try
            {
                string SQL_LOG = $@"ZPN_SP_LOGIMPORTACAOPO {dtRegistros.Rows[iPedido]["po_id"].ToString()}, '{ex.Message.Replace("'", "")}'";
                SqlUtils.DoNonQuery(SQL_LOG);
            }
            catch (Exception Ex)
            {
                HandleImportacaoException("", Ex);
            }
        }

        private static void HandleImportacaoException(string empresa, Exception ex)
        {
            string mensagemErro = $"Erro ao importar dados PO {empresa} - {ex.Message}".Replace("'", "") ;
            MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, ex);
        }
    }


    public static class ImportacaoServiceFactory
    {
        public static IImportacaoService CreateImportacaoService(string empresa)
        {
            if (empresa == "Huawei")
                return new ImportacaoService();
            else if (empresa == "Ericsson")
                return new ImportacaoService();
            else
                throw new NotSupportedException($"Empresa {empresa} não suportada");
        }
    }

    public interface IPesquisaService
    {
        void Pesquisar(MaskedTextBox mskDataI, MaskedTextBox mskDataF, DataGridView dgDadosPO, ProgressBar pbProgresso, out DataTable dtRegistros);
    }

    public class PesquisaHuaweiService : IPesquisaService
    {
        public void Pesquisar(MaskedTextBox mskDataI, MaskedTextBox mskDataF, DataGridView dgDadosPO, ProgressBar pbProgresso, out DataTable dtRegistros)
        {
            dtRegistros = new DataTable();
            try
            {
                string dataInicial = (mskDataI.MaskFull ? Convert.ToDateTime(mskDataI.Text) : DateTime.MinValue).ToString("yyyyMMdd");
                string dataFinal = (mskDataF.MaskFull ? Convert.ToDateTime(mskDataF.Text) : DateTime.MaxValue).ToString("yyyyMMdd");

                string SQL = $@"SP_ZPN_IMPORTARPOHuawei '{dataInicial}', '{dataFinal}', 'N'";

                dtRegistros = SqlUtils.ExecuteCommand(SQL);

                dgDadosPO.DataSource = dtRegistros;

                dgDadosPO.AutoResizeColumns();

                foreach (DataGridViewRow row in dgDadosPO.Rows)
                {
                    if (row.Cells["Status"].Value != null && row.Cells["Status"].Value.ToString() != "Importado")
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Importar"];
                        chk.Value = chk.TrueValue; // Sets the checkbox to true
                    }
                }



                dgDadosPO.Columns[0].ReadOnly = false;
            }
            catch (Exception ex)
            {
                HandlePesquisaException("Huawei", ex);
            }
        }

        private static void HandlePesquisaException(string empresa, Exception ex)
        {
            string mensagemErro = $"Erro ao carregar dados importação PO - {empresa} - {ex.Message}";
            MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, ex);
        }
    }

    public class PesquisaEricssonService : IPesquisaService
    {
        public void Pesquisar(MaskedTextBox mskDataI, MaskedTextBox mskDataF, DataGridView dgDadosPO, ProgressBar pbProgresso, out DataTable dtRegistros)
        {
            dtRegistros = new DataTable();
            try
            {
                string fileNameAnexo = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    openFileDialog.Title = "Selecione um arquivo CSV";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        fileNameAnexo = openFileDialog.FileName;
                    }
                }
                if (!string.IsNullOrEmpty(fileNameAnexo))
                {
                    using (var arquivoEricsson = new StreamReader(fileNameAnexo))
                    {
                        var linesArquivoEricsson = arquivoEricsson.ReadToEnd().Split(new char[] { '\n' });
                        var count = linesArquivoEricsson.Length;

                        pbProgresso.Value = 0;
                        pbProgresso.Maximum = count;
                        string SQL = string.Empty;

                        SqlUtils.DoNonQuery("TRUNCATE TABLE ZPN_POERICSSON");

                        for (int iPos = 0; iPos < linesArquivoEricsson.Length; iPos++)
                        {
                            pbProgresso.Value += 1;

                            var valores = linesArquivoEricsson[iPos].Split(';');

                            if (valores.Length == 13)
                            {
                                if (Int64.TryParse(valores[2].Trim(), out Int64 PO))
                                {
                                    SQL = $@"ZPN_SP_POERICSSON 
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

                        SQL = $@"SP_ZPN_IMPORTARPOERICSSON";

                        dtRegistros = SqlUtils.ExecuteCommand(SQL);



                        dgDadosPO.DataSource = dtRegistros;
                        dgDadosPO.AutoResizeColumns();
                        dgDadosPO.Columns[0].ReadOnly = false;


                        foreach (DataGridViewRow row in dgDadosPO.Rows)
                        {
                            if (row.Cells["Status"].Value != null && row.Cells["Status"].Value.ToString() != "Importado")
                            {
                                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Importar"];
                                chk.Value = chk.TrueValue; // Sets the checkbox to true
                            }
                        }


                        MessageBox.Show("Fim da leitura do arquivo!");
                    }
                }
                else
                {
                    MessageBox.Show("Não há arquivo selecionado!");
                }
            }
            catch (Exception ex)
            {
                HandlePesquisaException("Ericsson", ex);
            }
        }

        private static void HandlePesquisaException(string empresa, Exception ex)
        {
            string mensagemErro = $"Erro ao carregar dados importação PO - {empresa} - {ex.Message}";
            MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, ex);
        }
    }


    public static class PesquisaServiceFactory
    {
        public static IPesquisaService CreatePesquisaService(string empresa)
        {
            if (empresa == "Huawei")
                return new PesquisaHuaweiService();
            else if (empresa == "Ericsson")
                return new PesquisaEricssonService();
            else
                throw new NotSupportedException($"Empresa {empresa} não suportada");
        }
    }
}
