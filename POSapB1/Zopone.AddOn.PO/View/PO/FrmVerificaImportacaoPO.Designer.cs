namespace Zopone.AddOn.PO.View.PO
{
    partial class FrmVerificaImportacaoPO
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.mskDataI = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mskDataF = new System.Windows.Forms.MaskedTextBox();
            this.dgDadosPO = new System.Windows.Forms.DataGridView();
            this.po_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocEntryPO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.poNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOCTOOBRA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.po_lis_DataConfirmacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mensagem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnPesquisar = new System.Windows.Forms.Button();
            this.cbDataPesq = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgDadosPO)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Período";
            // 
            // mskDataI
            // 
            this.mskDataI.Location = new System.Drawing.Point(90, 12);
            this.mskDataI.Mask = "99/99/9999";
            this.mskDataI.Name = "mskDataI";
            this.mskDataI.Size = new System.Drawing.Size(100, 22);
            this.mskDataI.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "à";
            // 
            // mskDataF
            // 
            this.mskDataF.Location = new System.Drawing.Point(226, 12);
            this.mskDataF.Mask = "99/99/9999";
            this.mskDataF.Name = "mskDataF";
            this.mskDataF.Size = new System.Drawing.Size(100, 22);
            this.mskDataF.TabIndex = 4;
            // 
            // dgDadosPO
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDadosPO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgDadosPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDadosPO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.po_id,
            this.DocEntryPO,
            this.poNumber,
            this.DOCTOOBRA,
            this.po_lis_DataConfirmacao,
            this.Status,
            this.Mensagem});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDadosPO.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgDadosPO.Location = new System.Drawing.Point(12, 53);
            this.dgDadosPO.MultiSelect = false;
            this.dgDadosPO.Name = "dgDadosPO";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDadosPO.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgDadosPO.RowHeadersWidth = 51;
            this.dgDadosPO.RowTemplate.Height = 24;
            this.dgDadosPO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDadosPO.Size = new System.Drawing.Size(1269, 488);
            this.dgDadosPO.TabIndex = 8;
            this.dgDadosPO.DoubleClick += new System.EventHandler(this.dgDadosPO_DoubleClick);
            // 
            // po_id
            // 
            this.po_id.DataPropertyName = "po_id";
            this.po_id.HeaderText = "PO Id ";
            this.po_id.MinimumWidth = 6;
            this.po_id.Name = "po_id";
            this.po_id.ReadOnly = true;
            this.po_id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.po_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.po_id.Width = 125;
            // 
            // DocEntryPO
            // 
            this.DocEntryPO.DataPropertyName = "DocEntryPO";
            this.DocEntryPO.HeaderText = "PO SAP";
            this.DocEntryPO.MinimumWidth = 6;
            this.DocEntryPO.Name = "DocEntryPO";
            this.DocEntryPO.ReadOnly = true;
            this.DocEntryPO.Width = 125;
            // 
            // poNumber
            // 
            this.poNumber.DataPropertyName = "poNumber";
            this.poNumber.HeaderText = "PO Number";
            this.poNumber.MinimumWidth = 6;
            this.poNumber.Name = "poNumber";
            this.poNumber.ReadOnly = true;
            this.poNumber.Width = 125;
            // 
            // DOCTOOBRA
            // 
            this.DOCTOOBRA.DataPropertyName = "DOCTOOBRA";
            this.DOCTOOBRA.HeaderText = "PO Info";
            this.DOCTOOBRA.MinimumWidth = 6;
            this.DOCTOOBRA.Name = "DOCTOOBRA";
            this.DOCTOOBRA.ReadOnly = true;
            this.DOCTOOBRA.Width = 125;
            // 
            // po_lis_DataConfirmacao
            // 
            this.po_lis_DataConfirmacao.DataPropertyName = "DocDate";
            this.po_lis_DataConfirmacao.HeaderText = "Data Confirmação";
            this.po_lis_DataConfirmacao.MinimumWidth = 6;
            this.po_lis_DataConfirmacao.Name = "po_lis_DataConfirmacao";
            this.po_lis_DataConfirmacao.ReadOnly = true;
            this.po_lis_DataConfirmacao.Width = 125;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 125;
            // 
            // Mensagem
            // 
            this.Mensagem.DataPropertyName = "Mensagem";
            this.Mensagem.HeaderText = "Mensagem";
            this.Mensagem.MinimumWidth = 6;
            this.Mensagem.Name = "Mensagem";
            this.Mensagem.ReadOnly = true;
            this.Mensagem.Width = 125;
            // 
            // BtnPesquisar
            // 
            this.BtnPesquisar.Location = new System.Drawing.Point(581, 13);
            this.BtnPesquisar.Name = "BtnPesquisar";
            this.BtnPesquisar.Size = new System.Drawing.Size(94, 23);
            this.BtnPesquisar.TabIndex = 9;
            this.BtnPesquisar.Text = "Pesquisar";
            this.BtnPesquisar.UseVisualStyleBackColor = true;
            this.BtnPesquisar.Click += new System.EventHandler(this.BtnPesquisar_Click);
            // 
            // cbDataPesq
            // 
            this.cbDataPesq.FormattingEnabled = true;
            this.cbDataPesq.Items.AddRange(new object[] {
            "Data de Importação",
            "Data da PO"});
            this.cbDataPesq.Location = new System.Drawing.Point(332, 12);
            this.cbDataPesq.Name = "cbDataPesq";
            this.cbDataPesq.Size = new System.Drawing.Size(243, 24);
            this.cbDataPesq.TabIndex = 10;
            // 
            // FrmVerificaImportacaoPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 548);
            this.Controls.Add(this.cbDataPesq);
            this.Controls.Add(this.dgDadosPO);
            this.Controls.Add(this.BtnPesquisar);
            this.Controls.Add(this.mskDataF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mskDataI);
            this.Controls.Add(this.label1);
            this.Name = "FrmVerificaImportacaoPO";
            this.Text = "Verifica Importação de PO";
            this.Load += new System.EventHandler(this.FrmImportacaoPO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgDadosPO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mskDataI;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mskDataF;
        private System.Windows.Forms.DataGridView dgDadosPO;
        private System.Windows.Forms.Button BtnPesquisar;
        private System.Windows.Forms.DataGridViewTextBoxColumn po_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocEntryPO;
        private System.Windows.Forms.DataGridViewTextBoxColumn poNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOCTOOBRA;
        private System.Windows.Forms.DataGridViewTextBoxColumn po_lis_DataConfirmacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mensagem;
        private System.Windows.Forms.ComboBox cbDataPesq;
    }
}