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

namespace Zopone.AddOn.PO.View.Obra
{
    public partial class FrmPO : Form
    {
        public static string  TipoPesquisa { get; set; }

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
                    txtObra.Text = retornoDados[0];
                    lblObra.Text = retornoDados[1];

                    txtCliente.Text = retornoDados[3];
                    lblCliente.Text = retornoDados[2];
                }
                else if (TipoPesquisa == "CANDIDATO")
                {
                    txtCandidato.Text = retornoDados[0];
                }
                else if (TipoPesquisa == "ITEMFAT")
                {
                    txtItemFaturamento.Text = retornoDados[0];
                    lblItemFat.Text = retornoDados[1];
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
    }
}
