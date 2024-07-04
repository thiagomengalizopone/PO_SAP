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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtCancelar = new System.Windows.Forms.Button();
            this.BtSalvar = new System.Windows.Forms.Button();
            this.lblItemCode = new System.Windows.Forms.Label();
            this.lblItemFat = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblObra = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.mskDataFaturamento = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.BtAdicionar = new System.Windows.Forms.Button();
            this.DgItensPO = new System.Windows.Forms.DataGridView();
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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.CbStatus = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
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
            this.label3.Location = new System.Drawing.Point(118, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Nro Pedido";
            // 
            // txtNroPedido
            // 
            this.txtNroPedido.Location = new System.Drawing.Point(118, 40);
            this.txtNroPedido.Name = "txtNroPedido";
            this.txtNroPedido.Size = new System.Drawing.Size(329, 22);
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
            this.txtAnexo.Location = new System.Drawing.Point(12, 217);
            this.txtAnexo.Name = "txtAnexo";
            this.txtAnexo.Size = new System.Drawing.Size(906, 22);
            this.txtAnexo.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtCancelar);
            this.groupBox1.Controls.Add(this.BtSalvar);
            this.groupBox1.Controls.Add(this.lblItemCode);
            this.groupBox1.Controls.Add(this.lblItemFat);
            this.groupBox1.Controls.Add(this.lblCliente);
            this.groupBox1.Controls.Add(this.lblObra);
            this.groupBox1.Controls.Add(this.txtValor);
            this.groupBox1.Controls.Add(this.mskDataFaturamento);
            this.groupBox1.Controls.Add(this.maskedTextBox2);
            this.groupBox1.Controls.Add(this.BtAdicionar);
            this.groupBox1.Controls.Add(this.DgItensPO);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtObservacao);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtNroNF);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.CbTipo);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtParcela);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtItemFaturamento);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtItem);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtCliente);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtCandidato);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtObra);
            this.groupBox1.Location = new System.Drawing.Point(12, 249);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1105, 543);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Itens - PO";
            // 
            // BtCancelar
            // 
            this.BtCancelar.Location = new System.Drawing.Point(87, 492);
            this.BtCancelar.Name = "BtCancelar";
            this.BtCancelar.Size = new System.Drawing.Size(75, 23);
            this.BtCancelar.TabIndex = 53;
            this.BtCancelar.Text = "Cancelar";
            this.BtCancelar.UseVisualStyleBackColor = true;
            this.BtCancelar.Click += new System.EventHandler(this.BtCancelar_Click);
            // 
            // BtSalvar
            // 
            this.BtSalvar.Location = new System.Drawing.Point(6, 492);
            this.BtSalvar.Name = "BtSalvar";
            this.BtSalvar.Size = new System.Drawing.Size(75, 23);
            this.BtSalvar.TabIndex = 52;
            this.BtSalvar.Text = "Salvar";
            this.BtSalvar.UseVisualStyleBackColor = true;
            this.BtSalvar.Click += new System.EventHandler(this.BtSalvar_Click);
            // 
            // lblItemCode
            // 
            this.lblItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemCode.ForeColor = System.Drawing.Color.Red;
            this.lblItemCode.Location = new System.Drawing.Point(991, 89);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(108, 22);
            this.lblItemCode.TabIndex = 51;
            this.lblItemCode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblItemFat
            // 
            this.lblItemFat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemFat.ForeColor = System.Drawing.Color.Red;
            this.lblItemFat.Location = new System.Drawing.Point(683, 92);
            this.lblItemFat.Name = "lblItemFat";
            this.lblItemFat.Size = new System.Drawing.Size(282, 22);
            this.lblItemFat.TabIndex = 50;
            this.lblItemFat.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblCliente
            // 
            this.lblCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.ForeColor = System.Drawing.Color.Red;
            this.lblCliente.Location = new System.Drawing.Point(683, 43);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(411, 22);
            this.lblCliente.TabIndex = 49;
            this.lblCliente.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblObra
            // 
            this.lblObra.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObra.ForeColor = System.Drawing.Color.Red;
            this.lblObra.Location = new System.Drawing.Point(220, 45);
            this.lblObra.Name = "lblObra";
            this.lblObra.Size = new System.Drawing.Size(302, 22);
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
            // maskedTextBox2
            // 
            this.maskedTextBox2.Location = new System.Drawing.Point(6, 198);
            this.maskedTextBox2.Mask = "99/99/9999";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(100, 22);
            this.maskedTextBox2.TabIndex = 18;
            this.maskedTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BtAdicionar
            // 
            this.BtAdicionar.Location = new System.Drawing.Point(686, 197);
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
            this.Observacao});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgItensPO.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgItensPO.Location = new System.Drawing.Point(6, 232);
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
            this.DgItensPO.Size = new System.Drawing.Size(1093, 252);
            this.DgItensPO.TabIndex = 21;
            this.DgItensPO.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgItensPO_CellContentClick);
            // 
            // Obra
            // 
            this.Obra.DataPropertyName = "Project";
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
            this.ItemCode.DataPropertyName = "ItemCode";
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
            this.Valor.DataPropertyName = "LineTotal";
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
            this.Observacao.DataPropertyName = "FreeTxt";
            this.Observacao.HeaderText = "Observação";
            this.Observacao.MinimumWidth = 6;
            this.Observacao.Name = "Observacao";
            this.Observacao.ReadOnly = true;
            this.Observacao.Width = 125;
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
            this.txtItemFaturamento.Size = new System.Drawing.Size(245, 22);
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
            this.txtCliente.Size = new System.Drawing.Size(152, 22);
            this.txtCliente.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(112, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "Candidato";
            // 
            // txtCandidato
            // 
            this.txtCandidato.Location = new System.Drawing.Point(112, 45);
            this.txtCandidato.Name = "txtCandidato";
            this.txtCandidato.Size = new System.Drawing.Size(100, 22);
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
            this.txtObra.Size = new System.Drawing.Size(100, 22);
            this.txtObra.TabIndex = 7;
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
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(456, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(186, 22);
            this.textBox2.TabIndex = 2;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
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
            // FrmPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1118, 804);
            this.Controls.Add(this.CbStatus);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.mskDATA);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private GroupBox groupBox1;
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
        private MaskedTextBox maskedTextBox2;
        private MaskedTextBox mskDATA;
        private MaskedTextBox mskDataFaturamento;
        private TextBox txtValor;
        private TextBox textBox2;
        private Label lblObra;
        private Label lblCliente;
        private Label lblItemFat;
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
        private Label lblItemCode;
        private Button BtCancelar;
        private Button BtSalvar;
        private ComboBox CbStatus;
    }
}

