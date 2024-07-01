using Zopone.AddOn.PO.View.Obra.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sap.dev.core;
using Zopone.AddOn.PO.View.PO;
using System.Threading;
using Zopone.AddOn.PO.Model.Objects;
using SAPbobsCOM;

namespace Zopone.AddOn.PO.View.Obra
{
    public partial class FrmPO : Form
    {
        public static string TipoPesquisa { get; set; }
        public List<LinePO> linesPO = new List<LinePO>();

        public FrmPO()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    textBox.LostFocus += UtilTextBox.ExtOnLostFocus;
                    textBox.GotFocus += UtilTextBox.ExtOnGotFocus;
                }
            }

            DgItensPO.AutoResizeColumns();

        }

        private void BtAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarItemGrid();
        }

        private void AdicionarItemGrid()
        {
            try
            {
                linesPO.Add(
                    new LinePO()
                    {
                        Project = txtObra.Text,
                        U_Candidato = txtCandidato.Text,
                        U_CardCode = txtCliente.Text,
                        U_Item = txtItem.Text,
                        U_ItemFat = txtItemFaturamento.Text,
                        U_DescItemFat = lblItemFat.Text,
                        ItemCode = lblItemCode.Text,
                        U_Parcela = txtParcela.Text,
                        LineTotal = Convert.ToDouble(txtValor.Text),
                        U_Tipo = CbTipo.Text,
                        U_DataLanc = Convert.ToDateTime(mskDataLancamento.Text),
                        U_DataFat = Convert.ToDateTime(mskDataFaturamento.Text),
                        U_NroNF = txtNroNF.Text,
                        U_DataSol = Convert.ToDateTime(maskedTextBox2.Text),
                        FreeTxt = txtObservacao.Text
                    }
                    );

                txtObra.Text = string.Empty;
                txtCandidato.Text = string.Empty;
                txtCliente.Text = string.Empty;
                txtItem.Text = string.Empty;
                txtItemFaturamento.Text = string.Empty;
                lblItemCode.Text = string.Empty;
                lblObra.Text = string.Empty;
                lblItemFat.Text = string.Empty;
                txtParcela.Text = string.Empty;
                txtValor.Text = string.Empty;
                CbTipo.Text = string.Empty;
                mskDataLancamento.Text = string.Empty;
                mskDataFaturamento.Text = string.Empty;
                txtNroNF.Text = string.Empty;
                maskedTextBox2.Text = string.Empty;
                txtObservacao.Text  = string.Empty;

                BindingSource dgItensPO = new BindingSource();
                dgItensPO.DataSource = linesPO;

                DgItensPO.DataSource = dgItensPO;

                DgItensPO.AutoResizeColumns();
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao adicionar dados de PO: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, comma, dot, backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one dot or comma
            if ((e.KeyChar == '.' || e.KeyChar == ',') && (sender as TextBox).Text.IndexOfAny(new char[] { '.', ',' }) > -1)
            {
                e.Handled = true;
            }
        }

        private void txtObra_DoubleClick(object sender, EventArgs e)
        {
            PesquisarDados("OBRA");
        }

        private void PesquisarDados(string tipoPesquisa)
        {
            try
            {
                TipoPesquisa = tipoPesquisa;
                List<string> parametro = new List<string>();

                if (TipoPesquisa == "CANDIDATO")
                {
                    parametro.Add(txtObra.Text);
                }

                FrmPesquisa frmPesq = new FrmPesquisa(TipoPesquisa, parametro);
                frmPesq.ShowDialog();

                List<string> retornoDados = frmPesq.retornoDados;

                if (TipoPesquisa == "OBRA")
                {
                    if (retornoDados.Count == 0)
                    {
                        txtObra.Text = string.Empty;
                        lblObra.Text = string.Empty;

                        txtCliente.Text = string.Empty;
                        lblCliente.Text = string.Empty;
                    }
                    else
                    {
                        txtObra.Text = retornoDados[0];
                        lblObra.Text = retornoDados[1];

                        txtCliente.Text = retornoDados[3];
                        lblCliente.Text = retornoDados[2];
                    }
                }
                else if (TipoPesquisa == "CANDIDATO")
                {
                    if (retornoDados.Count == 0)
                        txtCandidato.Text = string.Empty;
                    else
                        txtCandidato.Text = retornoDados[0];
                }
                else if (TipoPesquisa == "ITEMFAT")
                {
                    if (retornoDados.Count == 0)
                    {

                        txtItemFaturamento.Text = string.Empty;
                        lblItemFat.Text = string.Empty;
                        lblItemCode.Text = string.Empty;
                    }
                    else
                    {
                        txtItemFaturamento.Text = retornoDados[0];
                        lblItemFat.Text = retornoDados[1];
                        lblItemCode.Text = retornoDados[2];
                    }
                }
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao pesquisar dados de PO: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void txtCandidato_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCandidato_DoubleClick(object sender, EventArgs e)
        {
            PesquisarDados("CANDIDATO");
        }

        private void DgItensPO_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtItemFaturamento_DoubleClick(object sender, EventArgs e)
        {
            PesquisarDados("ITEMFAT");

        }

        private void txtObservacao_Validated(object sender, EventArgs e)
        {
            AdicionarItemGrid();
        }

        private void lblObra_Click(object sender, EventArgs e)
        {

        }
    }
}
