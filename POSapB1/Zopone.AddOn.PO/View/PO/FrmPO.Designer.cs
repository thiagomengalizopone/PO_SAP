using Zopone.AddOn.PO.View.Obra.Helpers;
using System.Drawing;
using System.Windows.Forms;

namespace Zopone.AddOn.PO.View.Obra
{
    partial class FrmPO
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        Color backgroundColor = Color.FromArgb(247, 247, 247);


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPO));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNroPedido = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNroContratoCliente = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAnexo = new System.Windows.Forms.TextBox();
            this.gbItens = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtDescItemPO = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtInfoSitePO = new System.Windows.Forms.TextBox();
            this.cbBloqueado = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.BtPesqObra = new System.Windows.Forms.Button();
            this.BtCancelar = new System.Windows.Forms.Button();
            this.BtSalvar = new System.Windows.Forms.Button();
            this.lblItemFat = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblObra = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.mskDataFaturamento = new System.Windows.Forms.MaskedTextBox();
            this.mskDataSol = new System.Windows.Forms.MaskedTextBox();
            this.BtAdicionar = new System.Windows.Forms.Button();
            this.DgItensPO = new System.Windows.Forms.DataGridView();
            this.label20 = new System.Windows.Forms.Label();
            this.txtObservacao = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtNroNF = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.CbTipo = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtParcela = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtItemFaturamento = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtItem = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCandidato = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtObra = new System.Windows.Forms.TextBox();
            this.mskDATA = new System.Windows.Forms.MaskedTextBox();
            this.txtValorPO = new System.Windows.Forms.TextBox();
            this.CbStatus = new System.Windows.Forms.ComboBox();
            this.BtPesqPO = new System.Windows.Forms.Button();
            this.BtAnexo = new System.Windows.Forms.Button();
            this.txtNroCont = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.Obra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Candidato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemFaturamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parcela = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataLancamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataFaturamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NroNF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSolicitacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Observacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.U_PrjName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.U_CardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.U_DescItemFat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.U_Bloqueado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Contrato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbItens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgItensPO)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCodigo
            // 
            this.txtCodigo.Enabled = false;
            this.txtCodigo.Location = new System.Drawing.Point(12, 40);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(100, 22);
            this.txtCodigo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Código";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(139, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Nro Pedido";
            // 
            // txtNroPedido
            // 
            this.txtNroPedido.Location = new System.Drawing.Point(142, 40);
            this.txtNroPedido.Name = "txtNroPedido";
            this.txtNroPedido.Size = new System.Drawing.Size(305, 22);
            this.txtNroPedido.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Valor";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(653, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Data";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(767, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Nro Contrato Cliente";
            // 
            // txtNroContratoCliente
            // 
            this.txtNroContratoCliente.Location = new System.Drawing.Point(767, 40);
            this.txtNroContratoCliente.Name = "txtNroContratoCliente";
            this.txtNroContratoCliente.Size = new System.Drawing.Size(151, 22);
            this.txtNroContratoCliente.TabIndex = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 16);
            this.label7.TabIndex = 16;
            this.label7.Text = "Descrição";
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(12, 94);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(906, 92);
            this.txtDescricao.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Anexo";
            // 
            // txtAnexo
            // 
            this.txtAnexo.Enabled = false;
            this.txtAnexo.Location = new System.Drawing.Point(12, 217);
            this.txtAnexo.Name = "txtAnexo";
            this.txtAnexo.Size = new System.Drawing.Size(906, 22);
            this.txtAnexo.TabIndex = 6;
            // 
            // gbItens
            // 
            this.gbItens.Controls.Add(this.label22);
            this.gbItens.Controls.Add(this.txtNroCont);
            this.gbItens.Controls.Add(this.label21);
            this.gbItens.Controls.Add(this.txtDescItemPO);
            this.gbItens.Controls.Add(this.label16);
            this.gbItens.Controls.Add(this.txtInfoSitePO);
            this.gbItens.Controls.Add(this.cbBloqueado);
            this.gbItens.Controls.Add(this.button5);
            this.gbItens.Controls.Add(this.button4);
            this.gbItens.Controls.Add(this.button3);
            this.gbItens.Controls.Add(this.BtPesqObra);
            this.gbItens.Controls.Add(this.BtCancelar);
            this.gbItens.Controls.Add(this.BtSalvar);
            this.gbItens.Controls.Add(this.lblItemFat);
            this.gbItens.Controls.Add(this.lblCliente);
            this.gbItens.Controls.Add(this.lblObra);
            this.gbItens.Controls.Add(this.txtValor);
            this.gbItens.Controls.Add(this.mskDataFaturamento);
            this.gbItens.Controls.Add(this.mskDataSol);
            this.gbItens.Controls.Add(this.BtAdicionar);
            this.gbItens.Controls.Add(this.DgItensPO);
            this.gbItens.Controls.Add(this.label20);
            this.gbItens.Controls.Add(this.txtObservacao);
            this.gbItens.Controls.Add(this.label19);
            this.gbItens.Controls.Add(this.label18);
            this.gbItens.Controls.Add(this.txtNroNF);
            this.gbItens.Controls.Add(this.label17);
            this.gbItens.Controls.Add(this.label15);
            this.gbItens.Controls.Add(this.CbTipo);
            this.gbItens.Controls.Add(this.label14);
            this.gbItens.Controls.Add(this.label13);
            this.gbItens.Controls.Add(this.txtParcela);
            this.gbItens.Controls.Add(this.label12);
            this.gbItens.Controls.Add(this.txtItemFaturamento);
            this.gbItens.Controls.Add(this.label11);
            this.gbItens.Controls.Add(this.txtItem);
            this.gbItens.Controls.Add(this.label10);
            this.gbItens.Controls.Add(this.txtCliente);
            this.gbItens.Controls.Add(this.label9);
            this.gbItens.Controls.Add(this.txtCandidato);
            this.gbItens.Controls.Add(this.label8);
            this.gbItens.Controls.Add(this.txtObra);
            this.gbItens.Location = new System.Drawing.Point(12, 249);
            this.gbItens.Name = "gbItens";
            this.gbItens.Size = new System.Drawing.Size(1105, 640);
            this.gbItens.TabIndex = 21;
            this.gbItens.TabStop = false;
            this.gbItens.Text = "Itens - PO";
            this.gbItens.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 229);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(138, 16);
            this.label21.TabIndex = 60;
            this.label21.Text = "Descrição do Item PO";
            // 
            // txtDescItemPO
            // 
            this.txtDescItemPO.Enabled = false;
            this.txtDescItemPO.Location = new System.Drawing.Point(9, 250);
            this.txtDescItemPO.Name = "txtDescItemPO";
            this.txtDescItemPO.Size = new System.Drawing.Size(1004, 22);
            this.txtDescItemPO.TabIndex = 59;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 285);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(148, 16);
            this.label16.TabIndex = 58;
            this.label16.Text = "Informações do Site PO";
            // 
            // txtInfoSitePO
            // 
            this.txtInfoSitePO.Enabled = false;
            this.txtInfoSitePO.Location = new System.Drawing.Point(9, 305);
            this.txtInfoSitePO.Name = "txtInfoSitePO";
            this.txtInfoSitePO.Size = new System.Drawing.Size(1004, 22);
            this.txtInfoSitePO.TabIndex = 57;
            // 
            // cbBloqueado
            // 
            this.cbBloqueado.AutoSize = true;
            this.cbBloqueado.Location = new System.Drawing.Point(686, 200);
            this.cbBloqueado.Name = "cbBloqueado";
            this.cbBloqueado.Size = new System.Drawing.Size(205, 20);
            this.cbBloqueado.TabIndex = 20;
            this.cbBloqueado.Text = "Bloqueado para Faturamento";
            this.cbBloqueado.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.Location = new System.Drawing.Point(652, 40);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(28, 28);
            this.button5.TabIndex = 56;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(652, 86);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(28, 28);
            this.button4.TabIndex = 55;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(203, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(28, 28);
            this.button3.TabIndex = 54;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // BtPesqObra
            // 
            this.BtPesqObra.Image = ((System.Drawing.Image)(resources.GetObject("BtPesqObra.Image")));
            this.BtPesqObra.Location = new System.Drawing.Point(88, 40);
            this.BtPesqObra.Name = "BtPesqObra";
            this.BtPesqObra.Size = new System.Drawing.Size(28, 28);
            this.BtPesqObra.TabIndex = 24;
            this.BtPesqObra.UseVisualStyleBackColor = true;
            this.BtPesqObra.Click += new System.EventHandler(this.BtPesqObra_Click);
            // 
            // BtCancelar
            // 
            this.BtCancelar.Location = new System.Drawing.Point(87, 579);
            this.BtCancelar.Name = "BtCancelar";
            this.BtCancelar.Size = new System.Drawing.Size(75, 23);
            this.BtCancelar.TabIndex = 53;
            this.BtCancelar.Text = "Cancelar";
            this.BtCancelar.UseVisualStyleBackColor = true;
            this.BtCancelar.Click += new System.EventHandler(this.BtCancelar_Click);
            // 
            // BtSalvar
            // 
            this.BtSalvar.Location = new System.Drawing.Point(6, 579);
            this.BtSalvar.Name = "BtSalvar";
            this.BtSalvar.Size = new System.Drawing.Size(75, 23);
            this.BtSalvar.TabIndex = 52;
            this.BtSalvar.Text = "Salvar";
            this.BtSalvar.UseVisualStyleBackColor = true;
            this.BtSalvar.Click += new System.EventHandler(this.BtSalvar_Click);
            // 
            // lblItemFat
            // 
            this.lblItemFat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemFat.ForeColor = System.Drawing.Color.Red;
            this.lblItemFat.Location = new System.Drawing.Point(705, 92);
            this.lblItemFat.Name = "lblItemFat";
            this.lblItemFat.Size = new System.Drawing.Size(260, 22);
            this.lblItemFat.TabIndex = 50;
            this.lblItemFat.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblCliente
            // 
            this.lblCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.ForeColor = System.Drawing.Color.Red;
            this.lblCliente.Location = new System.Drawing.Point(702, 43);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(392, 22);
            this.lblCliente.TabIndex = 49;
            this.lblCliente.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblObra
            // 
            this.lblObra.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObra.ForeColor = System.Drawing.Color.Red;
            this.lblObra.Location = new System.Drawing.Point(237, 45);
            this.lblObra.Name = "lblObra";
            this.lblObra.Size = new System.Drawing.Size(285, 22);
            this.lblObra.TabIndex = 48;
            this.lblObra.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblObra.Click += new System.EventHandler(this.lblObra_Click);
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(90, 140);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(130, 22);
            this.txtValor.TabIndex = 13;
            this.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // mskDataFaturamento
            // 
            this.mskDataFaturamento.Location = new System.Drawing.Point(467, 140);
            this.mskDataFaturamento.Mask = "99/99/9999";
            this.mskDataFaturamento.Name = "mskDataFaturamento";
            this.mskDataFaturamento.Size = new System.Drawing.Size(100, 22);
            this.mskDataFaturamento.TabIndex = 16;
            this.mskDataFaturamento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mskDataSol
            // 
            this.mskDataSol.Location = new System.Drawing.Point(6, 198);
            this.mskDataSol.Mask = "99/99/9999";
            this.mskDataSol.Name = "mskDataSol";
            this.mskDataSol.Size = new System.Drawing.Size(100, 22);
            this.mskDataSol.TabIndex = 18;
            this.mskDataSol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BtAdicionar
            // 
            this.BtAdicionar.Location = new System.Drawing.Point(1019, 304);
            this.BtAdicionar.Name = "BtAdicionar";
            this.BtAdicionar.Size = new System.Drawing.Size(75, 23);
            this.BtAdicionar.TabIndex = 20;
            this.BtAdicionar.Text = "Adicionar";
            this.BtAdicionar.UseVisualStyleBackColor = true;
            this.BtAdicionar.Click += new System.EventHandler(this.BtAdicionar_Click);
            // 
            // DgItensPO
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgItensPO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgItensPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgItensPO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Obra,
            this.Candidato,
            this.Cliente,
            this.Item,
            this.ItemFaturamento,
            this.ItemCode,
            this.Parcela,
            this.Valor,
            this.Tipo,
            this.DataLancamento,
            this.DataFaturamento,
            this.NroNF,
            this.DataSolicitacao,
            this.Observacao,
            this.U_PrjName,
            this.U_CardName,
            this.U_DescItemFat,
            this.U_Bloqueado,
            this.Contrato});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgItensPO.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgItensPO.Location = new System.Drawing.Point(6, 334);
            this.DgItensPO.Name = "DgItensPO";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgItensPO.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DgItensPO.RowHeadersWidth = 51;
            this.DgItensPO.RowTemplate.Height = 24;
            this.DgItensPO.Size = new System.Drawing.Size(1093, 236);
            this.DgItensPO.TabIndex = 21;
            this.DgItensPO.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgItensPO_CellContentClick);
            this.DgItensPO.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgItensPO_CellDoubleClick);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(112, 173);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(82, 16);
            this.label20.TabIndex = 47;
            this.label20.Text = "Observação";
            // 
            // txtObservacao
            // 
            this.txtObservacao.Location = new System.Drawing.Point(115, 198);
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(565, 22);
            this.txtObservacao.TabIndex = 19;
            this.txtObservacao.Validated += new System.EventHandler(this.txtObservacao_Validated);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 173);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(106, 16);
            this.label19.TabIndex = 45;
            this.label19.Text = "Data Solicitação";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(580, 119);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 16);
            this.label18.TabIndex = 43;
            this.label18.Text = "Nro NF";
            // 
            // txtNroNF
            // 
            this.txtNroNF.Location = new System.Drawing.Point(580, 140);
            this.txtNroNF.Name = "txtNroNF";
            this.txtNroNF.Size = new System.Drawing.Size(100, 22);
            this.txtNroNF.TabIndex = 17;
            this.txtNroNF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(464, 117);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(114, 16);
            this.label17.TabIndex = 41;
            this.label17.Text = "Data Faturamento";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(220, 116);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 16);
            this.label15.TabIndex = 37;
            this.label15.Text = "Tipo";
            // 
            // CbTipo
            // 
            this.CbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbTipo.FormattingEnabled = true;
            this.CbTipo.Location = new System.Drawing.Point(223, 138);
            this.CbTipo.Name = "CbTipo";
            this.CbTipo.Size = new System.Drawing.Size(121, 24);
            this.CbTipo.TabIndex = 14;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(90, 117);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(39, 16);
            this.label14.TabIndex = 35;
            this.label14.Text = "Valor";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 117);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 16);
            this.label13.TabIndex = 33;
            this.label13.Text = "Parcela";
            // 
            // txtParcela
            // 
            this.txtParcela.Location = new System.Drawing.Point(6, 140);
            this.txtParcela.Name = "txtParcela";
            this.txtParcela.Size = new System.Drawing.Size(81, 22);
            this.txtParcela.TabIndex = 12;
            this.txtParcela.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(435, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 16);
            this.label12.TabIndex = 31;
            this.label12.Text = "Item Faturamento";
            // 
            // txtItemFaturamento
            // 
            this.txtItemFaturamento.Location = new System.Drawing.Point(435, 92);
            this.txtItemFaturamento.Name = "txtItemFaturamento";
            this.txtItemFaturamento.Size = new System.Drawing.Size(211, 22);
            this.txtItemFaturamento.TabIndex = 11;
            this.txtItemFaturamento.DoubleClick += new System.EventHandler(this.txtItemFaturamento_DoubleClick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 29;
            this.label11.Text = "Item";
            // 
            // txtItem
            // 
            this.txtItem.Location = new System.Drawing.Point(6, 92);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(423, 22);
            this.txtItem.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(528, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 27;
            this.label10.Text = "Cliente";
            // 
            // txtCliente
            // 
            this.txtCliente.Location = new System.Drawing.Point(528, 45);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(118, 22);
            this.txtCliente.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(119, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "Candidato";
            // 
            // txtCandidato
            // 
            this.txtCandidato.Location = new System.Drawing.Point(122, 45);
            this.txtCandidato.Name = "txtCandidato";
            this.txtCandidato.Size = new System.Drawing.Size(82, 22);
            this.txtCandidato.TabIndex = 8;
            this.txtCandidato.TextChanged += new System.EventHandler(this.txtCandidato_TextChanged);
            this.txtCandidato.DoubleClick += new System.EventHandler(this.txtCandidato_DoubleClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "Obra";
            // 
            // txtObra
            // 
            this.txtObra.Location = new System.Drawing.Point(6, 45);
            this.txtObra.Name = "txtObra";
            this.txtObra.Size = new System.Drawing.Size(81, 22);
            this.txtObra.TabIndex = 7;
            this.txtObra.TextChanged += new System.EventHandler(this.txtObra_TextChanged);
            this.txtObra.DoubleClick += new System.EventHandler(this.txtObra_DoubleClick);
            // 
            // mskDATA
            // 
            this.mskDATA.Location = new System.Drawing.Point(656, 40);
            this.mskDATA.Mask = "99/99/9999";
            this.mskDATA.Name = "mskDATA";
            this.mskDATA.Size = new System.Drawing.Size(87, 22);
            this.mskDATA.TabIndex = 3;
            this.mskDATA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtValorPO
            // 
            this.txtValorPO.Location = new System.Drawing.Point(456, 40);
            this.txtValorPO.Name = "txtValorPO";
            this.txtValorPO.Size = new System.Drawing.Size(186, 22);
            this.txtValorPO.TabIndex = 2;
            this.txtValorPO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValorPO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            // 
            // CbStatus
            // 
            this.CbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbStatus.Enabled = false;
            this.CbStatus.FormattingEnabled = true;
            this.CbStatus.Location = new System.Drawing.Point(936, 38);
            this.CbStatus.Name = "CbStatus";
            this.CbStatus.Size = new System.Drawing.Size(175, 24);
            this.CbStatus.TabIndex = 22;
            // 
            // BtPesqPO
            // 
            this.BtPesqPO.Image = ((System.Drawing.Image)(resources.GetObject("BtPesqPO.Image")));
            this.BtPesqPO.Location = new System.Drawing.Point(113, 38);
            this.BtPesqPO.Name = "BtPesqPO";
            this.BtPesqPO.Size = new System.Drawing.Size(23, 25);
            this.BtPesqPO.TabIndex = 23;
            this.BtPesqPO.UseVisualStyleBackColor = true;
            this.BtPesqPO.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtAnexo
            // 
            this.BtAnexo.Image = ((System.Drawing.Image)(resources.GetObject("BtAnexo.Image")));
            this.BtAnexo.Location = new System.Drawing.Point(924, 215);
            this.BtAnexo.Name = "BtAnexo";
            this.BtAnexo.Size = new System.Drawing.Size(28, 28);
            this.BtAnexo.TabIndex = 57;
            this.BtAnexo.UseVisualStyleBackColor = true;
            this.BtAnexo.Click += new System.EventHandler(this.BtAnexo_Click);
            // 
            // txtNroCont
            // 
            this.txtNroCont.Enabled = false;
            this.txtNroCont.Location = new System.Drawing.Point(686, 140);
            this.txtNroCont.Name = "txtNroCont";
            this.txtNroCont.Size = new System.Drawing.Size(81, 22);
            this.txtNroCont.TabIndex = 61;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(683, 118);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(82, 16);
            this.label22.TabIndex = 62;
            this.label22.Text = "Nro Contrato";
            // 
            // Obra
            // 
            this.Obra.DataPropertyName = "U_PrjCode";
            this.Obra.HeaderText = "Obra";
            this.Obra.MinimumWidth = 6;
            this.Obra.Name = "Obra";
            this.Obra.ReadOnly = true;
            this.Obra.Width = 125;
            // 
            // Candidato
            // 
            this.Candidato.DataPropertyName = "U_Candidato";
            this.Candidato.HeaderText = "Candidato";
            this.Candidato.MinimumWidth = 6;
            this.Candidato.Name = "Candidato";
            this.Candidato.ReadOnly = true;
            this.Candidato.Width = 125;
            // 
            // Cliente
            // 
            this.Cliente.DataPropertyName = "U_CardCode";
            this.Cliente.HeaderText = "Cliente";
            this.Cliente.MinimumWidth = 6;
            this.Cliente.Name = "Cliente";
            this.Cliente.ReadOnly = true;
            this.Cliente.Width = 125;
            // 
            // Item
            // 
            this.Item.DataPropertyName = "U_Item";
            this.Item.HeaderText = "Item";
            this.Item.MinimumWidth = 6;
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Width = 125;
            // 
            // ItemFaturamento
            // 
            this.ItemFaturamento.DataPropertyName = "U_ItemFat";
            this.ItemFaturamento.HeaderText = "Item Faturamento";
            this.ItemFaturamento.MinimumWidth = 6;
            this.ItemFaturamento.Name = "ItemFaturamento";
            this.ItemFaturamento.ReadOnly = true;
            this.ItemFaturamento.Width = 125;
            // 
            // ItemCode
            // 
            this.ItemCode.DataPropertyName = "U_ItemCode";
            this.ItemCode.HeaderText = "Item SAP";
            this.ItemCode.MinimumWidth = 6;
            this.ItemCode.Name = "ItemCode";
            this.ItemCode.ReadOnly = true;
            this.ItemCode.Width = 125;
            // 
            // Parcela
            // 
            this.Parcela.DataPropertyName = "U_Parcela";
            this.Parcela.HeaderText = "Parcela";
            this.Parcela.MinimumWidth = 6;
            this.Parcela.Name = "Parcela";
            this.Parcela.ReadOnly = true;
            this.Parcela.Width = 125;
            // 
            // Valor
            // 
            this.Valor.DataPropertyName = "U_Valor";
            this.Valor.HeaderText = "Valor";
            this.Valor.MinimumWidth = 6;
            this.Valor.Name = "Valor";
            this.Valor.ReadOnly = true;
            this.Valor.Width = 125;
            // 
            // Tipo
            // 
            this.Tipo.DataPropertyName = "U_Tipo";
            this.Tipo.HeaderText = "Tipo";
            this.Tipo.MinimumWidth = 6;
            this.Tipo.Name = "Tipo";
            this.Tipo.ReadOnly = true;
            this.Tipo.Width = 125;
            // 
            // DataLancamento
            // 
            this.DataLancamento.DataPropertyName = "U_DataLanc";
            this.DataLancamento.HeaderText = "Data Lancamento";
            this.DataLancamento.MinimumWidth = 6;
            this.DataLancamento.Name = "DataLancamento";
            this.DataLancamento.ReadOnly = true;
            this.DataLancamento.Width = 125;
            // 
            // DataFaturamento
            // 
            this.DataFaturamento.DataPropertyName = "U_DataFat";
            this.DataFaturamento.HeaderText = "Data Faturamento";
            this.DataFaturamento.MinimumWidth = 6;
            this.DataFaturamento.Name = "DataFaturamento";
            this.DataFaturamento.ReadOnly = true;
            this.DataFaturamento.Width = 125;
            // 
            // NroNF
            // 
            this.NroNF.DataPropertyName = "U_NroNF";
            this.NroNF.HeaderText = "Nro NF";
            this.NroNF.MinimumWidth = 6;
            this.NroNF.Name = "NroNF";
            this.NroNF.ReadOnly = true;
            this.NroNF.Width = 125;
            // 
            // DataSolicitacao
            // 
            this.DataSolicitacao.DataPropertyName = "U_DataSol";
            this.DataSolicitacao.HeaderText = "Data Solicitação";
            this.DataSolicitacao.MinimumWidth = 6;
            this.DataSolicitacao.Name = "DataSolicitacao";
            this.DataSolicitacao.ReadOnly = true;
            this.DataSolicitacao.Width = 125;
            // 
            // Observacao
            // 
            this.Observacao.DataPropertyName = "U_Obs";
            this.Observacao.HeaderText = "Observação";
            this.Observacao.MinimumWidth = 6;
            this.Observacao.Name = "Observacao";
            this.Observacao.ReadOnly = true;
            this.Observacao.Width = 125;
            // 
            // U_PrjName
            // 
            this.U_PrjName.DataPropertyName = "U_PrjName";
            this.U_PrjName.HeaderText = "Nome Projeto";
            this.U_PrjName.MinimumWidth = 6;
            this.U_PrjName.Name = "U_PrjName";
            this.U_PrjName.ReadOnly = true;
            this.U_PrjName.Visible = false;
            this.U_PrjName.Width = 125;
            // 
            // U_CardName
            // 
            this.U_CardName.DataPropertyName = "U_CardName";
            this.U_CardName.HeaderText = "Nome cliente";
            this.U_CardName.MinimumWidth = 6;
            this.U_CardName.Name = "U_CardName";
            this.U_CardName.Visible = false;
            this.U_CardName.Width = 125;
            // 
            // U_DescItemFat
            // 
            this.U_DescItemFat.DataPropertyName = "U_DescItemFat";
            this.U_DescItemFat.HeaderText = "Descrição Item";
            this.U_DescItemFat.MinimumWidth = 6;
            this.U_DescItemFat.Name = "U_DescItemFat";
            this.U_DescItemFat.ReadOnly = true;
            this.U_DescItemFat.Width = 125;
            // 
            // U_Bloqueado
            // 
            this.U_Bloqueado.DataPropertyName = "U_Bloqueado";
            this.U_Bloqueado.HeaderText = "Bloqueado";
            this.U_Bloqueado.MinimumWidth = 6;
            this.U_Bloqueado.Name = "U_Bloqueado";
            this.U_Bloqueado.ReadOnly = true;
            this.U_Bloqueado.Width = 125;
            // 
            // Contrato
            // 
            this.Contrato.DataPropertyName = "AgrNo";
            this.Contrato.HeaderText = "Contrato";
            this.Contrato.MinimumWidth = 6;
            this.Contrato.Name = "Contrato";
            this.Contrato.ReadOnly = true;
            this.Contrato.Width = 125;
            // 
            // FrmPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1132, 874);
            this.Controls.Add(this.BtAnexo);
            this.Controls.Add(this.BtPesqPO);
            this.Controls.Add(this.CbStatus);
            this.Controls.Add(this.txtValorPO);
            this.Controls.Add(this.mskDATA);
            this.Controls.Add(this.gbItens);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAnexo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtNroContratoCliente);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNroPedido);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCodigo);
            this.Name = "FrmPO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalhe PO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPO_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbItens.ResumeLayout(false);
            this.gbItens.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgItensPO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCodigo;
        private Label label1;
        private Label label3;
        private TextBox txtNroPedido;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox txtNroContratoCliente;
        private ContextMenuStrip contextMenuStrip1;
        private Label label7;
        private TextBox txtDescricao;
        private Label label2;
        private TextBox txtAnexo;
        private GroupBox gbItens;
        private Label label8;
        private TextBox txtObra;
        private Label label9;
        private TextBox txtCandidato;
        private Label label10;
        private TextBox txtCliente;
        private Label label12;
        private TextBox txtItemFaturamento;
        private Label label11;
        private TextBox txtItem;
        private Label label13;
        private TextBox txtParcela;
        private Label label14;
        private Label label15;
        private ComboBox CbTipo;
        private Label label17;
        private Label label18;
        private TextBox txtNroNF;
        private Label label19;
        private Label label20;
        private TextBox txtObservacao;
        private DataGridView DgItensPO;
        private Button BtAdicionar;
        private MaskedTextBox mskDataSol;
        private MaskedTextBox mskDATA;
        private MaskedTextBox mskDataFaturamento;
        private TextBox txtValor;
        private TextBox txtValorPO;
        private Label lblObra;
        private Label lblCliente;
        private Label lblItemFat;
        private Button BtCancelar;
        private Button BtSalvar;
        private ComboBox CbStatus;
        private Button BtPesqPO;
        private Button BtPesqObra;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button BtAnexo;
        private CheckBox cbBloqueado;
        private Label label16;
        private TextBox txtInfoSitePO;
        private Label label21;
        private TextBox txtDescItemPO;
        private Label label22;
        private TextBox txtNroCont;
        private DataGridViewTextBoxColumn Obra;
        private DataGridViewTextBoxColumn Candidato;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Item;
        private DataGridViewTextBoxColumn ItemFaturamento;
        private DataGridViewTextBoxColumn ItemCode;
        private DataGridViewTextBoxColumn Parcela;
        private DataGridViewTextBoxColumn Valor;
        private DataGridViewTextBoxColumn Tipo;
        private DataGridViewTextBoxColumn DataLancamento;
        private DataGridViewTextBoxColumn DataFaturamento;
        private DataGridViewTextBoxColumn NroNF;
        private DataGridViewTextBoxColumn DataSolicitacao;
        private DataGridViewTextBoxColumn Observacao;
        private DataGridViewTextBoxColumn U_PrjName;
        private DataGridViewTextBoxColumn U_CardName;
        private DataGridViewTextBoxColumn U_DescItemFat;
        private DataGridViewCheckBoxColumn U_Bloqueado;
        private DataGridViewTextBoxColumn Contrato;
    }
}

