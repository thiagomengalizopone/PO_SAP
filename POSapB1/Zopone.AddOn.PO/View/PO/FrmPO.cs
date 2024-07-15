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
using System.Data.Common;
using Zopone.AddOn.PO.Model.SAP;
using static System.Windows.Forms.LinkLabel;
using sap.dev.data;
using System.Collections;

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

        private void CarregarDadosPO(string docEntryPO)
        {
            try
            {
                PurchaseOrderSAP oPOSAP = new PurchaseOrderSAP();

                if (oPOSAP.GetByDocEntry(Convert.ToInt16(txtCodigo.Text)))
                {
                    txtNroPedido.Text = oPOSAP.U_NroPedido;
                    txtValorPO.Text = oPOSAP.U_Valor.ToString();
                    mskDATA.Text = oPOSAP.U_Data.ToString("dd/mm/yyyy");
                    txtNroContratoCliente.Text = oPOSAP.U_NroCont;
                    CbStatus.SelectedValue = oPOSAP.U_Status;
                    txtDescricao.Text = oPOSAP.U_Desc;
                    txtAnexo.Text = oPOSAP.U_Anexo;

                    linesPO.Clear();

                    foreach (var poLine in oPOSAP.Lines)
                    {
                        linesPO.Add(
                           new LinePO()
                           {
                               LineNum = -1,
                               U_PrjCode = poLine.U_PrjCode,
                               U_Candidato = poLine.U_Candidato,
                               U_CardCode = poLine.U_CardCode,
                               U_CardName = poLine.U_CardName,
                               U_Item = poLine.U_Item,
                               U_ItemFat = poLine.U_ItemFat,
                               U_DescItemFat = poLine.U_DescItemFat,
                               U_ItemCode = poLine.U_ItemCode,
                               U_Parcela = poLine.U_Parcela,
                               U_Valor = poLine.U_Valor,
                               U_Tipo = poLine.U_Tipo,
                               U_DataFat = poLine.U_DataFat,
                               U_NroNF = poLine.U_NroNF,
                               U_DataSol = poLine.U_DataSol,
                               U_Obs = poLine.U_Obs
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
                    U_Obs = txtObservacao.Text
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

                    CarregarDadosPO(txtCodigo.Text);
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

                PurchaseOrderSAP oPOSAP = new PurchaseOrderSAP();

                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    if (!oPOSAP.GetByDocEntry(Convert.ToInt32(txtCodigo.Text)))
                        throw new Exception($"Erro ao pesquisar PO: {txtCodigo.Text}");

                    bExistePedido = true;
                }

                if (!bExistePedido)
                {
                    oPOSAP.U_NroPedido = txtNroPedido.Text;
                    oPOSAP.U_Data = Convert.ToDateTime(mskDATA.Text);
                    oPOSAP.BplID = BPLId;
                }

                oPOSAP.U_NroCont = txtNroContratoCliente.Text;
                oPOSAP.U_Desc = txtObservacao.Text;
                oPOSAP.U_Anexo = txtAnexo.Text;
                oPOSAP.U_Status = CbStatus.SelectedValue.ToString();
                oPOSAP.U_Valor = Convert.ToDouble(txtValorPO.Text);

                oPOSAP.Lines.Clear();

                foreach (var linePO in linesPO)
                {
                    oPOSAP.Lines.Add(new PurchaseOrderSAP.PurchaseOrderLine()
                    {
                        U_Candidato = linePO.U_Candidato,
                        U_CardCode = linePO.U_CardCode,
                        U_CardName = linePO.U_CardName,
                        U_DataFat = linePO.U_DataFat,
                        U_DataLanc = DateTime.Now,
                        U_DataSol = linePO.U_DataSol,
                        U_DescItemFat = linePO.U_DescItemFat,
                        U_Item = linePO.U_Item,
                        U_ItemCode = linePO.U_ItemCode,
                        U_ItemFat = linePO.U_ItemFat,
                        U_NroNF = linePO.U_NroNF,
                        U_Obs = linePO.U_Obs,
                        U_Parcela = linePO.U_Parcela,
                        U_PrjCode = linePO.U_PrjCode,
                        U_Tipo = linePO.U_Tipo,
                        U_Valor = linePO.U_Valor
                    });
                }

                if (bExistePedido)
                    oPOSAP.Update();
                else
                {
                    oPOSAP.Add();
                    txtCodigo.Text = SqlUtils.GetValue(@"SELECT MAX(""DocEntry"") FROM ""@ZPN_ORDR"" ");
                }
                
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
