using sap.dev.core;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.Model.SAP;
using Zopone.AddOn.PO.View.Obra.Helpers;
using Zopone.AddOn.PO.View.PO;
using static System.Windows.Forms.LinkLabel;

namespace Zopone.AddOn.PO.View.Obra
{
    public partial class FrmPO : Form
    {

        public static string TipoPesquisa { get; set; }
        public List<LinePO> linesPO = new List<LinePO>();
        public List<Int32?> linesPODeleted = new List<Int32?>();
        public Int32 BPLId { get; set; }
        public Int32 RowIndexEdit { get; set; }
        public Int32 LineNumEdit { get; set; }
        public Int32? VisOrderEdit { get; set; }

        public string PCG { get; set; }
        public string OBRA { get; set; }
        public string REGIONAL { get; set; }

        public static Boolean IsDraft { get; set; }

        public static string DocEntryPO { get; set; }
        private static Thread formThread;

        private static DbConnection DbConnection;



        public static void MenuPO(string docEntryPO = "", bool isDraft = false)
        {
            DocEntryPO = docEntryPO;
            IsDraft = isDraft;

            FrmPO form = new FrmPO();

            form.ShowDialog();
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

                txtNroPedido.Focus();
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
                txtCodigo.Text = DocEntryPO;
                CarregarDadosPO(DocEntryPO, IsDraft);
                DocEntryPO = string.Empty;
            }

            RowIndexEdit = -1;

            this.TopMost = true;
            this.BringToFront();
            this.TopMost = false;

            txtNroPedido.Focus();

        }

        private void CarregarDadosPO(string docEntryPO, bool isDraft = false)
        {
            try
            {
                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                if (isDraft)
                    oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                if (oPedidoVenda.GetByKey(Convert.ToInt32(txtCodigo.Text)))
                {
                    txtNroPedido.Text = oPedidoVenda.NumAtCard;
                    txtValorPO.Text = oPedidoVenda.DocTotal.ToString();
                    mskDATA.Text = oPedidoVenda.DocDate.ToString("dd/MM/yyyy");
                    txtNroContratoCliente.Text = oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value.ToString();
                    //CbStatus.SelectedValue = oPOSAP.U_Status;
                    txtDescricao.Text = oPedidoVenda.Comments;
                    //txtAnexo.Text = oPOSAP.U_Anexo;

                    linesPO.Clear();

                    DataTable dtRegistros = SqlUtils.ExecuteCommand($"SP_ZPN_RetornaDadosLinhaPO {txtCodigo.Text.Trim()}");

                    foreach (DataRow row in dtRegistros.Rows)
                    {
                        linesPO.Add(new LinePO()
                        {
                            LineNum = row.Field<int>("LineNum"),
                            VisOrder = row.Field<int>("VisOrder"),
                            U_PrjCode = row.Field<string>("Project"),
                            U_Candidato = row.Field<string>("U_Candidato"),
                            U_CardCode = row.Field<string>("CardCode"),
                            U_CardName = row.IsNull("CardName") ? null : row.Field<string>("CardName"),
                            U_Item = row.Field<string>("U_Item"),
                            U_ItemFat = row.Field<string>("U_ItemFat"),
                            U_DescItemFat = row.Field<string>("U_DescItemFat"),
                            U_Parcela = row.Field<string>("U_Parcela"),
                            U_Valor = Convert.ToDouble(row.Field<decimal>("U_Valor")),
                            U_Tipo = row.Field<string>("U_Tipo"),
                            U_DataFat = row.IsNull("U_DataFat") ? (DateTime?)null : row.Field<DateTime>("U_DataFat"),
                            U_NroNF = row.Field<string>("U_NroNF"),
                            U_DataSol = row.IsNull("U_DataSol") ? (DateTime?)null : row.Field<DateTime>("U_DataSol"),
                            U_Obs = row.Field<string>("U_Obs"),
                            U_Bloqueado = row.Field<string>("U_Bloqueado") == "Y",
                            U_itemDescription = row.Field<string>("U_itemDescription"),
                            U_manSiteInfo = row.Field<string>("U_manSiteInfo"),
                            AgrNo = row.Field<Int32?>("AgrNo"),
                            DescContrato = row.Field<string>("U_DescCont"),
                            PCG = row.Field<string>("PCG"),
                            Obra = row.Field<string>("Obra"),
                            Regional = row.Field<string>("Regional"),
                            Edited = false
                        });
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
            AdicionarRemoverItemGrid();
        }

        private void DeletaLinhaPO()
        {
            try
            {
                if (DgItensPO.SelectedRows.Count == 0)
                    return;

                if (MessageBox.Show("Deseja remover a linha selecionada da PO?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                RowIndexEdit = DgItensPO.SelectedRows[0].Index;
                LineNumEdit = linesPO[RowIndexEdit].LineNum;
                VisOrderEdit = linesPO[RowIndexEdit].VisOrder;

                string mensagem = $"Usuário {Globals.Master.Connection.Database.UserName} removeu a linha {VisOrderEdit} da PO {txtNroPedido.Text}";

                AdicionarRemoverItemGrid(true);

                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Alerta, mensagem);

            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao excluir linha da PO: {Ex.Message}";
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
                MessageBox.Show(mensagemErro);
            }
        }

        private void AdicionarRemoverItemGrid(bool remover = false, bool salvar = false)
        {
            try
            {
                double dblTotalPO = 0;
                double dblTotalLinhasPO = 0;

                if (!remover)
                {

                    dblTotalPO = Math.Round(Convert.ToDouble(txtValorPO.Text), 2);
                    dblTotalLinhasPO = Math.Round(linesPO.Sum(item => item.U_Valor), 2);

                    if (RowIndexEdit >= 0)
                        dblTotalLinhasPO -= linesPO[RowIndexEdit].U_Valor;

                    if (dblTotalLinhasPO > dblTotalPO || Math.Round((dblTotalLinhasPO + Convert.ToDouble(txtValor.Text)), 2) > dblTotalPO)
                    {
                        MessageBox.Show("Não é possível adicionar novas linhas. Total das linhas não pode ser maior que total da PO!");
                        return;
                    }
                }

                if (!remover)
                {
                    LinePO oLinePO = new LinePO()
                    {
                        Agrupar = "N",
                        LineNum = LineNumEdit,
                        U_PrjCode = txtObra.Text,
                        VisOrder = string.IsNullOrEmpty(txtLinhaSAP.Text) ? -1 : Convert.ToInt32(txtLinhaSAP.Text),
                        U_Candidato = txtCandidato.Text,
                        U_CardCode = txtCliente.Text,
                        U_CardName = txtNomeCliente.Text,
                        U_Item = txtItem.Text,
                        U_ItemFat = txtItemFaturamento.Text,
                        U_DescItemFat = lblItemFat.Text,
                        U_Parcela = txtParcela.Text,
                        U_Valor = Convert.ToDouble(txtValor.Text),
                        U_Tipo = CbTipo.Text,
                        U_DataFat = mskDataFaturamento.MaskFull ? Convert.ToDateTime(mskDataFaturamento.Text) : (DateTime?)null,
                        U_NroNF = txtNroNF.Text,
                        U_DataSol = mskDataSol.MaskFull ? Convert.ToDateTime(mskDataSol.Text) : (DateTime?)null,
                        U_Obs = txtObservacao.Text,
                        U_Bloqueado = cbBloqueado.Checked,
                        U_itemDescription = txtDescItemPO.Text,
                        U_manSiteInfo = txtInfoSitePO.Text,
                        AgrNo = !string.IsNullOrEmpty(txtNroCont.Text) ? Convert.ToInt32(txtNroCont.Text) : 0,
                        DescContrato = txtDescContrato.Text,
                        CostingCode = PCG,
                        CostingCode2 = OBRA,
                        CostingCode3 = REGIONAL,
                        Edited = true

                    };


                    if (RowIndexEdit < 0)
                        linesPO.Add(oLinePO);
                    else
                        linesPO[RowIndexEdit] = oLinePO;


                }
                else
                {
                    linesPO.Remove(linesPO[RowIndexEdit]);
                    linesPODeleted.Add(VisOrderEdit);
                }


                CarregarMatrixPO();

                LimparLinhaPO();

                RowIndexEdit = -1;
                LineNumEdit = -1;

                dblTotalPO = Math.Round(Convert.ToDouble(txtValorPO.Text), 2);
                dblTotalLinhasPO = Math.Round(linesPO.Sum(item => item.U_Valor), 2);

                if (dblTotalPO == dblTotalLinhasPO && !salvar)
                {
                    SalvarPO();
                }
                else
                {
                    txtObra.Focus();
                }

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

            for (int iRow = 0; iRow < DgItensPO.Rows.Count; iRow++)
            {
                if (
                    (DgItensPO.Rows[iRow].Cells["Cliente"].Value != null && !string.IsNullOrEmpty(DgItensPO.Rows[iRow].Cells["Cliente"].Value.ToString()))
                    &&
                    (DgItensPO.Rows[iRow].Cells["Obra"].Value == null || string.IsNullOrEmpty(DgItensPO.Rows[iRow].Cells["Obra"].Value.ToString()))
                    )
                {
                    DgItensPO.Rows[iRow].DefaultCellStyle.BackColor = Color.OrangeRed;
                }
            }

            txtTotalPO.Text = Math.Round(linesPO.Sum(item => item.U_Valor), 2).ToString();
        }


        private void LimparLinhaPO()
        {
            txtObra.Text = string.Empty;
            txtLinhaSAP.Text = string.Empty;
            txtCandidato.Text = string.Empty;
            txtCliente.Text = string.Empty;
            txtItem.Text = string.Empty;
            txtItemFaturamento.Text = string.Empty;
            txtNomeCliente.Text = string.Empty;
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
            txtInfoSitePO.Text = string.Empty;
            txtDescItemPO.Text = string.Empty;
            txtNroCont.Text = string.Empty;
            txtDescContrato.Text = string.Empty;

            PCG = string.Empty;
            OBRA = string.Empty;
            REGIONAL = string.Empty;
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
                else if (tipoPesquisa == "ITEMFAT")
                {
                    parametro.Add(txtObra.Text);
                    parametro.Add(txtItemFaturamento.Text);

                }
                else if (tipoPesquisa == "OBRA")
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
                        txtLinhaSAP.Text = string.Empty;

                        txtCliente.Text = string.Empty;
                        txtNomeCliente.Text = string.Empty;
                        txtNroCont.Text = string.Empty;
                        txtDescContrato.Text = string.Empty;
                        lblObra.Text = string.Empty;
                        BPLId = -1;

                        PCG = string.Empty;
                        OBRA = string.Empty;
                        REGIONAL = string.Empty;
                        txtCandidato.Focus();
                    }
                    else
                    {
                        txtObra.Text = retornoDados[0];
                        lblObra.Text = retornoDados[1];

                        txtCliente.Text = retornoDados[2];
                        txtNomeCliente.Text = retornoDados[3];

                        BPLId = Convert.ToInt32(retornoDados[4]);

                        txtNroCont.Text = retornoDados[5];
                        txtDescContrato.Text = retornoDados[9];
                        PCG = retornoDados[6];
                        OBRA = retornoDados[7];
                        REGIONAL = retornoDados[8];

                        txtCandidato.Focus();
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
                        txtParcela.Focus();
                    }
                    else
                    {
                        txtItemFaturamento.Text = retornoDados[0];
                        lblItemFat.Text = retornoDados[1];
                        txtParcela.Focus();
                    }
                }
                else if (TipoPesquisa == "PO")
                {
                    if (retornoDados.Count == 0)
                        txtCodigo.Text = string.Empty;
                    else
                    {
                        txtCodigo.Text = retornoDados[0];
                        CarregarDadosPO(txtCodigo.Text, retornoDados[1] == "D");
                    }
                }
                else if (TipoPesquisa == "CLIENTE")
                {
                    if (retornoDados.Count != 0)
                    {
                        txtCliente.Text = retornoDados[0];
                        txtNomeCliente.Text = retornoDados[1];
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
            AdicionarRemoverItemGrid();
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

        private void LimparTelaPO()
        {

            LimparLinhaPO();
            linesPO = new List<LinePO>();
            linesPODeleted = new List<Int32?>();

            foreach (Control controle in this.Controls)
            {
                if (controle is TextBox)
                {
                    (controle as TextBox).Clear();
                }
            }

            CarregarMatrixPO();

            txtNroPedido.Focus();
        }

        private void CarregarDadosLinhaPO(int rowIndex)
        {
            try
            {

                RowIndexEdit = rowIndex;
                LineNumEdit = linesPO[rowIndex].LineNum;
                txtLinhaSAP.Text = linesPO[rowIndex].VisOrder.ToString();
                txtObra.Text = linesPO[rowIndex].U_PrjCode;
                txtLinhaSAP.Text = linesPO[rowIndex].VisOrder >= 0 ? linesPO[rowIndex].VisOrder.ToString() : string.Empty;
                txtCandidato.Text = linesPO[rowIndex].U_Candidato;
                txtCliente.Text = linesPO[rowIndex].U_CardCode;
                txtNomeCliente.Text = linesPO[rowIndex].U_CardName;
                txtItem.Text = linesPO[rowIndex].U_Item;
                txtItemFaturamento.Text = linesPO[rowIndex].U_ItemFat;
                lblItemFat.Text = linesPO[rowIndex].U_DescItemFat;
                txtParcela.Text = linesPO[rowIndex].U_Parcela;
                txtValor.Text = linesPO[rowIndex].U_Valor.ToString();
                CbTipo.SelectedValue = linesPO[rowIndex].U_Tipo;
                mskDataFaturamento.Text = linesPO[rowIndex].U_DataFat?.ToString("dd/MM/yyyy");
                txtNroNF.Text = linesPO[rowIndex].U_NroNF;
                mskDataSol.Text = linesPO[rowIndex].U_DataSol?.ToString("dd/MM/yyyy");
                txtObservacao.Text = linesPO[rowIndex].U_Obs;
                cbBloqueado.Checked = linesPO[rowIndex].U_Bloqueado;
                txtDescItemPO.Text = linesPO[rowIndex].U_itemDescription;
                txtInfoSitePO.Text = linesPO[rowIndex].U_manSiteInfo;
                txtNroCont.Text = linesPO[rowIndex].AgrNo.ToString();
                txtDescContrato.Text = linesPO[rowIndex].DescContrato;
                PCG = linesPO[rowIndex].CostingCode;
                OBRA = linesPO[rowIndex].CostingCode2;
                REGIONAL = linesPO[rowIndex].CostingCode3;

                lblObra.Text = SqlUtils.GetValue($@"SELECT Name FROM ""@ZPN_OPRJ"" WHERE ""Code"" = '{linesPO[rowIndex].U_PrjCode}'");

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
                BtSalvar.Enabled = false;

                if (string.IsNullOrEmpty(txtNroPedido.Text))
                {
                    MessageBox.Show("Não há número de pedido digitado!");
                    txtNroPedido.Focus();
                    return;
                }

                double dblTotalPO = Math.Round(Convert.ToDouble(txtValorPO.Text), 2);
                double dblTotalLinhasPO = Math.Round(linesPO.Sum(item => item.U_Valor), 2);

                if (dblTotalPO != dblTotalLinhasPO)
                {
                    MessageBox.Show("Total das linhas diferente do total da PO", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool bExistePedido = false;
                string CodigoPO = string.Empty;
                string CodigoPedidoCancelado = string.Empty;

                if (!string.IsNullOrEmpty(txtItem.Text))
                    AdicionarRemoverItemGrid(false, true);


                SAPbobsCOM.Documents oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                try
                {

                    if (!string.IsNullOrEmpty(txtCodigo.Text))
                    {
                        oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                        if (!oPedidoVenda.GetByKey(Convert.ToInt32(txtCodigo.Text)))
                            throw new Exception($"Erro ao pesquisar Pedido: {txtNroNF.Text}");

                        if (oPedidoVenda.CardCode != linesPO[0].U_CardCode)
                        {
                            CodigoPedidoCancelado = oPedidoVenda.DocEntry.ToString();
                            CancelarPedidoVenda(oPedidoVenda);
                            oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                            bExistePedido = false;
                        }
                        else
                            bExistePedido = true;
                    }
                    else
                    {

                        if (ConfiguracoesImportacaoPO.TipoDocumentoPO == "E")
                        {
                            oPedidoVenda = (SAPbobsCOM.Documents)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                            oPedidoVenda.DocObjectCodeEx = "17";
                        }
                        oPedidoVenda.TaxExtension.MainUsage = Convert.ToInt32(ConfiguracoesImportacaoPO.Utilizacao);
                    }

                    if (!bExistePedido)
                    {
                        oPedidoVenda.DocDate = Convert.ToDateTime(mskDATA.Text);
                        oPedidoVenda.DocDueDate = Convert.ToDateTime(mskDATA.Text);
                        oPedidoVenda.CardCode = linesPO[0].U_CardCode;
                        oPedidoVenda.BPL_IDAssignedToInvoice = 1;
                    }

                    oPedidoVenda.NumAtCard = txtNroPedido.Text;

                    oPedidoVenda.UserFields.Fields.Item("U_NroCont").Value = txtNroContratoCliente.Text;
                    oPedidoVenda.Comments = txtObservacao.Text;

                    foreach (var linePO in linesPO.Where(linePO => linePO.Edited == true).ToList())
                    {
                        if (!string.IsNullOrEmpty(oPedidoVenda.Lines.ItemCode))
                        {
                            if (linePO.VisOrder >= 0)
                                oPedidoVenda.Lines.SetCurrentLine((int)linePO.VisOrder);
                            else
                                oPedidoVenda.Lines.Add();
                        }

                        oPedidoVenda.Lines.Usage = ConfiguracoesImportacaoPO.Utilizacao;
                        oPedidoVenda.Lines.ItemCode = ConfiguracoesImportacaoPO.ItemCodePO;
                        oPedidoVenda.Lines.Quantity = 1;
                        oPedidoVenda.Lines.UnitPrice = Convert.ToDouble(linePO.U_Valor);
                        oPedidoVenda.Lines.Price = Convert.ToDouble(linePO.U_Valor);
                        oPedidoVenda.Lines.LineTotal = Convert.ToDouble(linePO.U_Valor);
                        oPedidoVenda.Lines.ProjectCode = linePO.U_PrjCode;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_Candidato").Value = linePO.U_Candidato;
                        oPedidoVenda.Lines.FreeText = linePO.U_Obs;

                        if (linePO.U_DataFat != null)
                            oPedidoVenda.Lines.UserFields.Fields.Item("U_DataFat").Value = linePO.U_DataFat;

                        if (linePO.U_DataLanc != null)
                            oPedidoVenda.Lines.UserFields.Fields.Item("U_DataLanc").Value = linePO.U_DataLanc;

                        if (linePO.U_DataSol != null)
                            oPedidoVenda.Lines.UserFields.Fields.Item("U_DataSol").Value = linePO.U_DataSol;

                        oPedidoVenda.Lines.UserFields.Fields.Item("U_Item").Value = linePO.U_Item;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_ItemFat").Value = linePO.U_ItemFat;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_DescItemFat").Value = linePO.U_DescItemFat;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_NroNF").Value = linePO.U_NroNF;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_Tipo").Value = linePO.U_Tipo;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_Parcela").Value = linePO.U_Parcela;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_Bloqueado").Value = linePO.U_Bloqueado ? "Y" : "N";
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_itemDescription").Value = linePO.U_itemDescription;
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_manSiteInfo").Value = linePO.U_manSiteInfo;

                        oPedidoVenda.Lines.CostingCode = linePO.CostingCode;
                        oPedidoVenda.Lines.CostingCode2 = linePO.CostingCode2;
                        oPedidoVenda.Lines.CostingCode3 = linePO.CostingCode3;

                        oPedidoVenda.Lines.UserFields.Fields.Item("U_StatusImp").Value = "Y";
                        oPedidoVenda.Lines.UserFields.Fields.Item("U_DescCont").Value = linePO.DescContrato;

                        if (linePO.AgrNo > 0 && ValidaClienteContrato(linePO.AgrNo, oPedidoVenda.CardCode))
                            oPedidoVenda.Lines.AgreementNo = Convert.ToInt32(linePO.AgrNo);
                    }

                    foreach (var deletedLine in linesPODeleted.OrderByDescending(line => line))
                    {
                        oPedidoVenda.Lines.SetCurrentLine(Convert.ToInt32(deletedLine));
                        oPedidoVenda.Lines.Delete();

                    }

                    if (bExistePedido)
                    {
                        if (oPedidoVenda.Update() != 0)
                            throw new Exception($"Erro ao atualizar PO - {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                    }
                    else
                    {
                        if (oPedidoVenda.Add() != 0)
                            throw new Exception($"Erro ao adicionar PO - {Globals.Master.Connection.Database.GetLastErrorDescription()}");

                        txtCodigo.Text = Globals.Master.Connection.Database.GetNewObjectKey();
                    }

                    CodigoPO = txtCodigo.Text;

                    if (!string.IsNullOrEmpty(CodigoPedidoCancelado))
                        RemoverDadosPCIAsync(CodigoPedidoCancelado);

                    new Task(() => { EnviarDadosPCIAsync(CodigoPO); }).Start();

                    LimparTelaPO();

                    linesPODeleted = new List<Int32?>();

                    lblMensagemTela.Text = "PO Salva com sucesso!";
                    lblMensagemTela.Font = new Font(lblMensagemTela.Font, FontStyle.Bold);
                    lblMensagemTela.ForeColor = Color.Black;

                }
                catch (Exception Ex)
                {
                    throw;
                }
                finally
                {

                }

            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao salvar PO: {Ex.Message}";

                lblMensagemTela.Text = mensagemErro;
                lblMensagemTela.Font = new Font(lblMensagemTela.Font, FontStyle.Bold);
                lblMensagemTela.ForeColor = Color.Red;

                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);

            }
            finally
            {
                BtSalvar.Enabled = true;
            }

        }

        private bool ValidaClienteContrato(Int32? agrNo, string cardCode)
        {
            return SqlUtils.ExistemRegistros($"SELECT 1 FROM OOAT WHERE AbsId = {agrNo} and BpCode = '{cardCode}'");
        }

        private void CancelarPedidoVenda(Documents oPedidoVenda)
        {
            try
            {
                oPedidoVenda.NumAtCard = string.Empty;

                if (oPedidoVenda.Update() != 0)
                    throw new Exception(Globals.Master.Connection.Database.GetLastErrorDescription());

                if (oPedidoVenda.Cancel() != 0)
                    throw new Exception(Globals.Master.Connection.Database.GetLastErrorDescription());
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao salvar pedido de Vendas - PO Cliente - {Ex.Message}");
            }
        }

        private static async Task RemoverDadosPCIAsync(string Docentry)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                string SQL_Query = $"ZPN_SP_PCI_REMOVEPO '{Docentry}'";

                SqlUtils.DoNonQueryAsync(SQL_Query);
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao enviar dados PCI: {Ex.Message}";
                MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, mensagemErro, Ex);
            }
        }

        private static async Task EnviarDadosPCIAsync(string Docentry)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                //SqlUtils.DoNonQuery($"SP_ZPN_VERIFICACADASTROPCI {Docentry}, 17");

                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                oRecordSet.DoQuery($"SP_ZPN_VERIFICACADASTROPCI {Docentry}, 17");


                string SQL_Query = $"ZPN_SP_PCI_INSEREATUALIZAPO '{Docentry}'";

                //SqlUtils.DoNonQueryAsync(SQL_Query);

                oRecordSet.DoQuery(SQL_Query);

                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");
            }
            catch (Exception Ex)
            {
                string mensagemErro = $"Erro ao enviar dados PCI: {Ex.Message}";
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
            linesPODeleted = new List<Int32?>();
            lblMensagemTela.Text = string.Empty;
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

        private void txtObra_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                bool pesquisaObra = true;
                if (!string.IsNullOrEmpty(txtObra.Text))
                {
                    string SQL_CONSULTA = $"SP_ZPN_PESQUISAOBRA '{txtObra.Text}'";

                    DataTable dgResultado = SqlUtils.ExecuteCommand(SQL_CONSULTA);

                    if (dgResultado.Rows.Count == 1)
                    {
                        txtObra.Text = dgResultado.Rows[0][0].ToString();//obra
                        lblObra.Text = dgResultado.Rows[0][1].ToString();//desc obra
                        txtCliente.Text = dgResultado.Rows[0][5].ToString();//código cliente
                        txtNomeCliente.Text = dgResultado.Rows[0][6].ToString();//cliente
                        BPLId = Convert.ToInt32(dgResultado.Rows[0][8].ToString());
                        txtNroCont.Text = dgResultado.Rows[0][10].ToString();
                        txtDescContrato.Text = dgResultado.Rows[0][9].ToString();
                        PCG = dgResultado.Rows[0][11].ToString();
                        OBRA = dgResultado.Rows[0][12].ToString();
                        REGIONAL = dgResultado.Rows[0][13].ToString();

                        pesquisaObra = false;

                        txtCandidato.Focus();
                    }
                }

                if (pesquisaObra)
                {
                    PesquisarDados("OBRA");
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show($"Erro ao pesquisar obra: {Ex.Message}", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void BtCancelar_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void FrmPO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DeletaLinhaPO();
        }

        private void txtNroPedido_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNroPedido.Text))
                    e.Cancel = ValidaNumeroPOExistente();

                lblMensagemTela.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                string Mensagem = $"Erro ao validar Número de PO: {Ex.Message}";

                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, Mensagem, Ex);
                MessageBox.Show(Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidaNumeroPOExistente()
        {
            bool bExistente = false;
            if (!string.IsNullOrEmpty(txtNroPedido.Text))
            {
                string sql = $"SELECT 1 FROM ORDR WHERE Canceled <> 'Y' and NumAtCard = '{txtNroPedido.Text.Trim()}'";

                if (!string.IsNullOrEmpty(txtCodigo.Text))
                    sql += $" AND DocNum <> {txtCodigo.Text}";


                bExistente = SqlUtils.ExistemRegistros(sql);

                if (bExistente)
                    MessageBox.Show($"PO {txtNroPedido.Text} já existe no sistema!");
            }

            return bExistente;
        }

        private void txtItemFaturamento_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool pesquisaEtapa = true;


            if (!string.IsNullOrEmpty(txtObra.Text))
            {
                string SQL_CONSULTA = $"SP_ZPN_PESQUISAETAPA '{txtItemFaturamento.Text}', '{txtObra.Text}'";

                DataTable dgResultado = SqlUtils.ExecuteCommand(SQL_CONSULTA);

                if (dgResultado.Rows.Count == 1)
                {
                    txtItemFaturamento.Text = dgResultado.Rows[0][0].ToString();
                    lblItemFat.Text = dgResultado.Rows[0][1].ToString();

                    pesquisaEtapa = false;

                    txtParcela.Focus();
                }
            }

            if (pesquisaEtapa)
            {
                PesquisarDados("ITEMFAT");
            }
        }

        private void BtMesclar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Deseja mesclas as linhas da PO?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                int iRow = 0;


                LinePO linePOAgrupamento = new LinePO();
                linePOAgrupamento.U_Valor = 0;

                List<LinePO> itemsToRemove = new List<LinePO>();

                foreach (var linePO in linesPO)
                {
                    var mesclarCellValue = DgItensPO.Rows[iRow].Cells["Mesclar"].Value;

                    if (mesclarCellValue != DBNull.Value && mesclarCellValue?.ToString() == "Y")
                    {
                        linesPODeleted.Add(iRow);

                        linePOAgrupamento.Obra = linePO.Obra;
                        linePOAgrupamento.U_PrjCode = linePO.U_PrjCode;
                        linePOAgrupamento.U_PrjName = linePO.U_PrjName;
                        linePOAgrupamento.U_Candidato = linePO.U_Candidato;
                        linePOAgrupamento.U_CardCode = linePO.U_CardCode;
                        linePOAgrupamento.U_CardName = linePO.U_CardName;
                        linePOAgrupamento.U_Item = string.IsNullOrEmpty(linePOAgrupamento.U_Item) ? linePO.U_Item : linePOAgrupamento.U_Item + " " + linePO.U_Item;
                        linePOAgrupamento.U_ItemFat = linePO.U_ItemFat;
                        linePOAgrupamento.PCG = linePO.PCG;
                        linePOAgrupamento.Regional = linePO.Regional;
                        linePOAgrupamento.U_DescItemFat = linePO.U_DescItemFat;
                        linePOAgrupamento.U_ItemCode = linePO.U_ItemCode;
                        linePOAgrupamento.U_Parcela = linePO.U_Parcela;
                        linePOAgrupamento.U_Valor += linePO.U_Valor;
                        linePOAgrupamento.U_Tipo = linePO.U_Tipo;
                        linePOAgrupamento.AgrNo = linePO.AgrNo;
                        linePOAgrupamento.DescContrato = linePO.DescContrato;
                        linePOAgrupamento.U_DataLanc = linePO.U_DataLanc;
                        linePOAgrupamento.U_DataFat = linePO.U_DataFat;
                        linePOAgrupamento.U_NroNF = linePO.U_NroNF;
                        linePOAgrupamento.U_DataSol = linePO.U_DataSol;
                        linePOAgrupamento.U_Obs = linePO.U_Obs;
                        linePOAgrupamento.U_Bloqueado = linePO.U_Bloqueado;
                        linePOAgrupamento.U_Validado = linePO.U_Validado;
                        linePOAgrupamento.U_itemDescription = linePO.U_itemDescription;
                        linePOAgrupamento.U_manSiteInfo = linePO.U_manSiteInfo;
                        linePOAgrupamento.CostingCode = linePO.CostingCode;
                        linePOAgrupamento.CostingCode2 = linePO.CostingCode2;
                        linePOAgrupamento.CostingCode3 = linePO.CostingCode3;

                        itemsToRemove.Add(linePO);
                    }
                    iRow++;
                }

                // Agora remove todos os itens de uma vez após o loop
                foreach (var item in itemsToRemove)
                {
                    linesPO.Remove(item);
                }
                linesPO.Add(linePOAgrupamento);

                CarregarMatrixPO();

                LimparLinhaPO();

            }
            catch (Exception Ex)
            {
                string MensagemErro = $"Erro ao mesclar linhas: {Ex.Message}";
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, MensagemErro, Ex);
                MessageBox.Show(MensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtValor_Validated(object sender, EventArgs e)
        {
            AdicionarRemoverItemGrid();
        }
    }
}
