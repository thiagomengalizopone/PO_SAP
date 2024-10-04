using sap.dev.core;
using sap.dev.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.PO
{
    public partial class FrmVerificaImportacaoPO : Form
    {
        private static Thread formThread;
        public DataTable dtRegistros { get; set; }

        public Int32 BPLId { get; set; }
        public FrmVerificaImportacaoPO()
        {
            InitializeComponent();
        }

        internal static void MenuVerificaPO()
        {
            formThread = new Thread(new ThreadStart(OpenFormImportacaoPO));
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
        }

        private static void OpenFormImportacaoPO()
        {
            System.Windows.Forms.Application.Run(new FrmVerificaImportacaoPO());
        }

        private void BtnPesquisar_Click(object sender, EventArgs e)
        {
            PesquisarDados();
        }

        private void PesquisarDados()
        {
            try
            {

                string tipoDataPEsquisa = cbDataPesq.Text.Trim() == "Data de Importação" ? "I" : "P";

                string dataInicial = (mskDataI.MaskFull ? Convert.ToDateTime(mskDataI.Text) : DateTime.MinValue).ToString("yyyyMMdd");
                string dataFinal = (mskDataF.MaskFull ? Convert.ToDateTime(mskDataF.Text) : DateTime.MaxValue).ToString("yyyyMMdd");
                string IgnorarValidados = CkValidado.Checked ? "Y" : "N";
                string Empresa = !string.IsNullOrEmpty(CbEmpresa.Text) ? CbEmpresa.Text : "";

                string SQL = $@"SP_ZPN_VERIFICAIMPORTARPOHuawei '{dataInicial}', '{dataFinal}', '{tipoDataPEsquisa}', '{Empresa}', '{IgnorarValidados}'";

                string pedidoVenda = string.Empty;

                dtRegistros = SqlUtils.ExecuteCommand(SQL);

                dgDadosPO.DataSource = dtRegistros;

                dgDadosPO.AutoResizeColumns();

                for (int iRow = 0; iRow < dgDadosPO.Rows.Count; iRow++)
                {
                    if (
                        (dgDadosPO.Rows[iRow].Cells["po_id"].Value != null && !string.IsNullOrEmpty(dgDadosPO.Rows[iRow].Cells["po_id"].Value.ToString()))
                        &&
                        (dgDadosPO.Rows[iRow].Cells["DOCTOOBRA"].Value != null && !string.IsNullOrEmpty(dgDadosPO.Rows[iRow].Cells["DOCTOOBRA"].Value.ToString()))
                        )
                    {
                        dgDadosPO.Rows[iRow].DefaultCellStyle.BackColor = Color.OrangeRed;
                    }
                }

            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados importação PO - {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void FrmImportacaoPO_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.BringToFront();
            this.TopMost = false;

            cbDataPesq.SelectedIndex = 0;

            CarregarDadosLogImportacaoPO();
        }

        private void CarregarDadosLogImportacaoPO()
        {
            try
            {
                mskDataI.Text = DateTime.Now.ToString("dd/MM/yyyy");
                mskDataF.Text = DateTime.Now.ToString("dd/MM/yyyy");

                PesquisarDados();
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados log importação PO - {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }        
        }

        private void dgDadosPO_DoubleClick(object sender, EventArgs e)
        {
            int rowId = dgDadosPO.SelectedRows[0].Index ;

            bool IsDraft = dtRegistros.Rows[rowId]["Status"].ToString() != "Pedido";
            string DocEntry = dtRegistros.Rows[rowId]["DocEntryPo"].ToString();

            FrmPO.MenuPO(DocEntry, IsDraft);

        }

        private void CkValidado_CheckedChanged(object sender, EventArgs e)
        {
            PesquisarDados();
        }

        private void FrmVerificaImportacaoPO_Activated(object sender, EventArgs e)
        {
        }
    }
}
