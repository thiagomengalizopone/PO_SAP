using sap.dev.core;
using sap.dev.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Zopone.AddOn.PO.View.PO
{
    public partial class FrmPesquisa : Form
    {
        public string TipoPesquisa { get; set; }
        public List<string> retornoDados = new List<string>();
        public List<string> Parametros = new List<string>();
        public FrmPesquisa(string tipoPesquisa, List<string> parametros = null)
        {
            TipoPesquisa = tipoPesquisa;
            Parametros = parametros;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPesquisa_Load(object sender, EventArgs e)
        {            
            CarregarDadosPesquisa();
            this.TopMost = true;
            this.BringToFront();
            this.TopMost = false;
        }

        private void CarregarDadosPesquisa()
        {
            try
            {
                string commandSQL = string.Empty;


                if (TipoPesquisa == "OBRA")
                    commandSQL = $"SP_ZPN_PESQUISAOBRA '{txtPesquisar.Text}'";
                else if (TipoPesquisa == "CANDIDATO")
                    commandSQL = $"SP_ZPN_PESQUISAOBRACANDIDATO '{txtPesquisar.Text}', '{Parametros[0]}'";
                else if (TipoPesquisa == "ITEMFAT")
                    commandSQL = $"SP_ZPN_PESQUISAETAPA '{txtPesquisar.Text}'";
                else if (TipoPesquisa == "PO")
                    commandSQL = $"ZPN_SP_LISTAPO '{txtPesquisar.Text}'";
                DataTable result = SqlUtils.ExecuteCommand(commandSQL);

                dgResultado.DataSource = result;

                for (int iCol = 0; iCol < dgResultado.Columns.Count; iCol++)
                {
                    dgResultado.Columns[iCol].SortMode = DataGridViewColumnSortMode.Automatic;
                    dgResultado.Columns[iCol].ReadOnly = true;
                }

                dgResultado.AutoResizeColumns();
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao pesquisar dados: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            CarregarDadosPesquisa();
        }

        private void BtPesq_Click(object sender, EventArgs e)
        {
            SelecionarDados();
        }

        private void SelecionarDados()
        {
            try
            {
                if (dgResultado.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Não há linha selecionada!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int selectedRowIndex = dgResultado.SelectedRows[0].Index;

                retornoDados = new List<string>();

                if (TipoPesquisa == "OBRA")
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[1].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[4].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[5].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[8].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[10].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[11].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[12].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[13].Value.ToString());
                }
                else if (TipoPesquisa == "CANDIDATO")
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                }
                else if (TipoPesquisa == "ITEMFAT")
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[1].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[2].Value.ToString());
                }
                else if(TipoPesquisa == "PO") 
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[7].Value.ToString());

                }

                this.Close();
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao selecionar dados: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void dgResultado_DoubleClick(object sender, EventArgs e)
        {
            SelecionarDados();
        }
    }
}
