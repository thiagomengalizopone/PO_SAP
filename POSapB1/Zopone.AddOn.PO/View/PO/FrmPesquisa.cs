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
                {
                    if (Parametros.Count > 0 && string.IsNullOrEmpty(txtPesquisar.Text) && !string.IsNullOrEmpty(Parametros[0]))
                    {
                        txtPesquisar.Text = Parametros[0];
                        Parametros[0] = string.Empty;
                    }

                    commandSQL = $"SP_ZPN_PESQUISAOBRA '{txtPesquisar.Text}'";
                }
                else if (TipoPesquisa == "CANDIDATO")
                    commandSQL = $"SP_ZPN_PESQUISAOBRACANDIDATO '{txtPesquisar.Text}', '{Parametros[0]}'";
                else if (TipoPesquisa == "ITEMFAT")
                {
                    if (Parametros.Count > 0 && string.IsNullOrEmpty(txtPesquisar.Text) && !string.IsNullOrEmpty(Parametros[0]))
                    {
                        txtPesquisar.Text = Parametros[1];
                        Parametros[1] = string.Empty;
                    }

                    commandSQL = $"SP_ZPN_PESQUISAETAPA '{txtPesquisar.Text}', '{Parametros[0]}'";
                }
                else if (TipoPesquisa == "PO")
                    commandSQL = $"SP_ZPN_PESQUISAPO '{txtPesquisar.Text}'";
                else if (TipoPesquisa == "CLIENTE")
                    commandSQL = $"SP_ZPN_PESQUISACLIENTE '{txtPesquisar.Text}'";                

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
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());//obra
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[1].Value.ToString());//desc obra
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[5].Value.ToString());//código cliente
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[6].Value.ToString());//cliente
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[8].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[10].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[11].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[12].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[13].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[3].Value.ToString()); //id Contrato
                }
                else if (TipoPesquisa == "CANDIDATO")
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                }
                else if (TipoPesquisa == "ITEMFAT")
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[1].Value.ToString());
                }
                else if(TipoPesquisa == "PO") 
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[8].Value.ToString());
                }
                else if (TipoPesquisa == "CLIENTE")
                {
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[0].Value.ToString());
                    retornoDados.Add(dgResultado.Rows[selectedRowIndex].Cells[1].Value.ToString());

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

        private void FrmPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelecionarDados();
            }
        }
    }
}
