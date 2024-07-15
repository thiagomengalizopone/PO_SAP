﻿using sap.dev.core;
using sap.dev.data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.Model.SAP;
using Zopone.AddOn.PO.View.Obra.Helpers;
using Zopone.AddOn.PO.View.PO;

namespace Zopone.AddOn.PO.View.Obra
{
    public partial class FrmPO : Form
    {
        public static string TipoPesquisa { get; set; }
        public List<LinePO> linesPO = new List<LinePO>();
        public Int32 BPLId { get; set; }
        public Int32 RowIndexEdit { get; set; }

        public static string DocEntryPO { get; set; }
        private static Thread formThread;

        private static DbConnection DbConnection;



        public static void MenuPO(string docEntryPO = "")
        {
            DocEntryPO = docEntryPO;

            formThread = new Thread(new ThreadStart(OpenFormPO));
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
        }

        private static void OpenFormPO()
        {
            System.Windows.Forms.Application.Run(new FrmPO());
        }

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

                if (control is MaskedTextBox)
                {
                    MaskedTextBox maskedEdit = (MaskedTextBox)control;
                    maskedEdit.LostFocus += UtilTextBox.MskOnLostFocus;
                    maskedEdit.GotFocus += UtilTextBox.MskOnGotFocus;
                }
            }

            foreach (Control control in this.gbItens.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    textBox.LostFocus += UtilTextBox.ExtOnLostFocus;
                    textBox.GotFocus += UtilTextBox.ExtOnGotFocus;
                }

                if (control is MaskedTextBox)
                {
                    MaskedTextBox maskedEdit = (MaskedTextBox)control;
                    maskedEdit.LostFocus += UtilTextBox.MskOnLostFocus;
                    maskedEdit.GotFocus += UtilTextBox.MskOnGotFocus;
                }
            }

            DgItensPO.AutoResizeColumns();

            SelecionaValoresTela();

            mskDATA.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (!string.IsNullOrEmpty(DocEntryPO))
            {
                CarregarDadosPO(DocEntryPO);
                DocEntryPO = string.Empty;
            }

            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void CarregarDadosPO(string docEntryPO, bool isDraft = false)
        {
            try
            {
                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                if (isDraft)
                    oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                if (oPedidoVenda.GetByKey(Convert.ToInt16(txtCodigo.Text)))
                {
                    txtNroPedido.Text = oPedidoVenda.NumAtCard;
                    txtValorPO.Text = oPedidoVenda.DocTotal.ToString();
                    mskDATA.Text = oPedidoVenda.DocDate.ToString("dd/MM/yyyy");
                    txtNroContratoCliente.Text = oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value.ToString();
                    //CbStatus.SelectedValue = oPOSAP.U_Status;
                    txtDescricao.Text = oPedidoVenda.Comments;
                    //txtAnexo.Text = oPOSAP.U_Anexo;

                    linesPO.Clear();

                    for (int iRow = 0; iRow < oPedidoVenda.Lines.Count; iRow++) 
                    {

                        linesPO.Add(
                           new LinePO()
                           {
                               LineNum = oPedidoVenda.Lines.LineNum,
                               U_PrjCode = oPedidoVenda.Lines.ProjectCode,
                               U_Candidato = oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value.ToString(),
                               U_CardCode = oPedidoVenda.CardCode,
                               U_CardName = oPedidoVenda.CardName,
                               U_Item = oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value.ToString(),
                               U_ItemFat = oPedidoVenda.Lines.UserFields.Fields.Item("U_ItemFat").Value.ToString(),
                               U_DescItemFat = oPedidoVenda.Lines.ItemDescription,
                               U_ItemCode = oPedidoVenda.Lines.ItemCode,
                               U_Parcela = oPedidoVenda.Lines.UserFields.Fields.Item("Parcela").Value.ToString(),
                               U_Valor = oPedidoVenda.Lines.LineTotal,
                               U_Tipo = oPedidoVenda.Lines.UserFields.Fields.Item("").Value.ToString(),
                               U_DataFat = Convert.ToDateTime(oPedidoVenda.Lines.UserFields.Fields.Item("DataFat").Value),
                               U_NroNF = oPedidoVenda.Lines.UserFields.Fields.Item("U_NroNF").Value.ToString(),
                               U_DataSol = Convert.ToDateTime(oPedidoVenda.Lines.UserFields.Fields.Item("DataSol").Value),
                               U_Obs = oPedidoVenda.Lines.FreeText,
                               U_Bloqueado = oPedidoVenda.Lines.UserFields.Fields.Item("U_Bloqueado").Value.ToString() == "Y"
                           }
                           );
                    }

                    CarregarMatrixPO();

                    LimparLinhaPO();
                }
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados  PO - {docEntryPO}: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }


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
                if (string.IsNullOrEmpty(txtObra.Text))
                    return;

                LinePO oLinePO = new LinePO()
                {
                    LineNum = -1,
                    U_PrjCode = txtObra.Text,
                    U_Candidato = txtCandidato.Text,
                    U_CardCode = txtCliente.Text,
                    U_CardName = lblCliente.Text,
                    U_Item = txtItem.Text,
                    U_ItemFat = txtItemFaturamento.Text,
                    U_DescItemFat = lblItemFat.Text,
                    U_ItemCode = lblItemCode.Text,
                    U_Parcela = txtParcela.Text,
                    U_Valor = Convert.ToDouble(txtValor.Text),
                    U_Tipo = CbTipo.Text,
                    U_DataFat = Convert.ToDateTime(mskDataFaturamento.Text),
                    U_NroNF = txtNroNF.Text,
                    U_DataSol = Convert.ToDateTime(mskDataSol.Text),
                    U_Obs = txtObservacao.Text,
                    U_Bloqueado = cbBloqueado.Checked 
                };

                if (RowIndexEdit < 0)
                    linesPO.Add(oLinePO);
                else
                    linesPO[RowIndexEdit] = oLinePO;

                CarregarMatrixPO();

                LimparLinhaPO();

                RowIndexEdit = -1;
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao adicionar dados de PO: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        public void CarregarMatrixPO()
        {
            BindingSource dgItensPO = new BindingSource();
            dgItensPO.DataSource = linesPO;

            DgItensPO.DataSource = dgItensPO;

            DgItensPO.AutoResizeColumns();
        }

        private void LimparLinhaPO()
        {
            txtObra.Text = string.Empty;
            txtCandidato.Text = string.Empty;
            txtCliente.Text = string.Empty;
            txtItem.Text = string.Empty;
            txtItemFaturamento.Text = string.Empty;
            lblItemCode.Text = string.Empty;
            lblCliente.Text = string.Empty;
            lblObra.Text = string.Empty;
            lblItemFat.Text = string.Empty;
            txtParcela.Text = string.Empty;
            txtValor.Text = string.Empty;
            CbTipo.Text = string.Empty;
            mskDataFaturamento.Text = string.Empty;
            txtNroNF.Text = string.Empty;
            mskDataSol.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            lblItemFat.Text = string.Empty;
            cbBloqueado.Checked = false;
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
                else if (TipoPesquisa == "PO")
                {
                    if (retornoDados.Count == 0)
                        txtCodigo.Text = string.Empty;
                    else
                        txtCodigo.Text = retornoDados[0];

                    bool isDraft = retornoDados[1] == "D";

                    CarregarDadosPO(txtCodigo.Text, isDraft);
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

        private void CarregarDadosLinhaPO(int rowIndex)
        {
            try
            {
                RowIndexEdit = rowIndex;
                txtObra.Text = linesPO[rowIndex].U_PrjCode;
                lblObra.Text = linesPO[rowIndex].U_PrjName;
                txtCandidato.Text = linesPO[rowIndex].U_Candidato;
                txtCliente.Text = linesPO[rowIndex].U_CardCode;
                lblCliente.Text = linesPO[rowIndex].U_CardName;
                txtItem.Text = linesPO[rowIndex].U_Item;
                txtItemFaturamento.Text = linesPO[rowIndex].U_ItemFat;
                lblItemCode.Text = linesPO[rowIndex].U_ItemCode;
                lblItemFat.Text = linesPO[rowIndex].U_DescItemFat;
                txtParcela.Text = linesPO[rowIndex].U_Parcela;
                txtValor.Text = linesPO[rowIndex].U_Valor.ToString();
                CbTipo.SelectedValue = linesPO[rowIndex].U_Tipo;
                mskDataFaturamento.Text = linesPO[rowIndex].U_DataFat.ToString("dd/MM/yyyy");
                txtNroNF.Text = linesPO[rowIndex].U_NroNF;
                mskDataSol.Text = linesPO[rowIndex].U_DataSol.ToString("dd/MM/yyyy");
                txtObservacao.Text = linesPO[rowIndex].U_Obs;
                cbBloqueado.Checked = linesPO[rowIndex].U_Bloqueado;
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao carregar dados de linha PO: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private void SalvarPO()
        {
            try
            {
                if (MessageBox.Show("Deseja salvar a PO?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                double dblTotalPO = Convert.ToDouble(txtValorPO.Text);
                double dblTotalLinhasPO = linesPO.Sum(item => item.U_Valor);

                if (dblTotalPO != dblTotalLinhasPO)
                {
                    MessageBox.Show("Total das linhas diferente do total da PO", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool bExistePedido = false;


                if (!string.IsNullOrEmpty(txtItem.Text))
                    AdicionarItemGrid();



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

                    oPedidoVenda.Lines.ItemCode = linePO.U_ItemCode;
                    oPedidoVenda.Lines.Quantity = 1;
                    oPedidoVenda.Lines.Price = Convert.ToDouble(linePO.U_Valor);
                    oPedidoVenda.Lines.ProjectCode = linePO.U_PrjCode;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value = linePO.U_Candidato;
                    oPedidoVenda.Lines.FreeText = linePO.U_Obs;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DataFat").Value = linePO.U_DataFat;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DataLanc").Value = linePO.U_DataLanc;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DataSol").Value = linePO.U_DataSol;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value = linePO.U_Item;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_ItemFat").Value = linePO.U_ItemFat;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_NroNF").Value = linePO.U_NroNF;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Tipo").Value = linePO.U_Tipo;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Parcela").Value = linePO.U_Parcela;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_DescItemFat").Value = linePO.U_DescItemFat;
                    oPedidoVenda.Lines.UserFields.Fields.Item("U_Bloqueado").Value = linePO.U_DescItemFat;

                }


                if (bExistePedido)
                {
                    if (oPedidoVenda.Update() != 0)
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

        private void txtObra_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtPesqObra_Click(object sender, EventArgs e)
        {
            PesquisarDados("OBRA");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PesquisarDados("CANDIDATO");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PesquisarDados("CLIENTE");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PesquisarDados("ITEMFAT");
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PesquisarDados("PO");
        }

        private void DgItensPO_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CarregarDadosLinhaPO(e.RowIndex);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private async void BtAnexo_Click(object sender, EventArgs e)
        {
            string fileNameAnexo = await Util.OpenFileDialogAsync(EnumList.TipoArquivo.Todos);

            if (!string.IsNullOrEmpty(fileNameAnexo))
                txtAnexo.Text = fileNameAnexo;
        }

    }
}
