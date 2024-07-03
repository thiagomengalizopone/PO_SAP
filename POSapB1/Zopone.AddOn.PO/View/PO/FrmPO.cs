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
        public Int32 BPLId { get; set; }

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

            SelecionaValoresTela();

        }

        private void SelecionaValoresTela()
        {
            try
            {
                var valoresStatus = new List<Tuple<string, string>>()
                {
                    Tuple.Create("F", "Faturado"),
                    Tuple.Create("N", "Não Faturado"),
                    Tuple.Create("P", "Parcialmente Faturado")
                };

                CbStatus.DataSource = valoresStatus;
                CbStatus.ValueMember = "Item1";
                CbStatus.DisplayMember = "Item2";


                CbStatus.SelectedValue = "N";

                var valoresTipoItem = new List<Tuple<string, string>>()
                {
                    Tuple.Create("I", "Item"),
                    Tuple.Create("S", "Serviço")
                };

                CbTipo.DataSource = valoresTipoItem;
                CbTipo.ValueMember = "Item1";
                CbTipo.DisplayMember = "Item2";

            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados tela PO: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
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
                        LineNum = -1,
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
                        BPLId = -1;
                    }
                    else
                    {
                        txtObra.Text = retornoDados[0];
                        lblObra.Text = retornoDados[1];

                        txtCliente.Text = retornoDados[3];
                        lblCliente.Text = retornoDados[2];

                        BPLId = Convert.ToInt32(retornoDados[4]);
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

        private void BtCancelar_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        private bool ValidarFecharForm()
        {
            return (MessageBox.Show("Deseja sair da edição de PO?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void FrmPO_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ValidarFecharForm();
        }

        private void BtSalvar_Click(object sender, EventArgs e)
        {
            SalvarPO();
        }

        private void SalvarPO()
        {
            try
            {
                if (MessageBox.Show("Deseja salvar a PO?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                bool bExistePedido = false;

                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                if (!string.IsNullOrEmpty(txtNroNF.Text))
                {
                    if (!oPedidoVenda.GetByKey(Convert.ToInt32(txtNroNF.Text)))
                        throw new Exception($"Erro ao pesquisar Pedido: {txtNroNF.Text}");
                    
                    bExistePedido = true;
                }

                if (!bExistePedido) 
                {
                    oPedidoVenda.NumAtCard = txtNroPedido.Text;
                    oPedidoVenda.DocDate = Convert.ToDateTime(mskDATA.Text);
                    oPedidoVenda.DocDueDate = Convert.ToDateTime(mskDATA.Text);
                    oPedidoVenda.CardCode = linesPO[0].U_CardCode;
                    oPedidoVenda.BPL_IDAssignedToInvoice = BPLId;
                }

                oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value = txtNroContratoCliente.Text;
                oPedidoVenda.Comments = txtObservacao.Text;

                foreach (var linePO in linesPO)
                {
                    if (linePO.LineNum == -1 && !string.IsNullOrEmpty(oPedidoVenda.Lines.ItemCode))
                        oPedidoVenda.Lines.Add();
                    else if (linePO.LineNum >= 0)
                        oPedidoVenda.Lines.SetCurrentLine(linePO.LineNum);

                    oPedidoVenda.Lines.ItemCode = linePO.ItemCode;
                    oPedidoVenda.Lines.Quantity = 1;
                    oPedidoVenda.Lines.Price = Convert.ToDouble(linePO.LineTotal);
                    oPedidoVenda.Lines.ProjectCode = linePO.Project;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value = linePO.U_Candidato;
                    oPedidoVenda.Lines.FreeText = linePO.FreeTxt;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DataFat").Value = linePO.U_DataFat;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DataLanc").Value = linePO.U_DataLanc;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DataSol").Value = linePO.U_DataSol;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value = linePO.U_Item;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_ItemFat").Value = linePO.U_ItemFat;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_NroNF").Value = linePO.U_NroNF;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Tipo").Value = linePO.U_Tipo;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Parcela").Value = linePO.U_Parcela;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DescItemFat").Value = linePO.U_DescItemFat;
                }

                if (bExistePedido)
                {
                    if (oPedidoVenda.Update() !=0)
                        throw new Exception($"Erro ao adicionar PO - {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                }
                else
                {
                    if (oPedidoVenda.Add() != 0)
                        throw new Exception($"Erro ao adicionar PO - {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                }

                txtCodigo.Text = Globals.Master.Connection.Database.GetNewObjectKey();

                MessageBox.Show("PO salva com sucesso!");


            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao salvar PO: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
            
        }
    }
}
