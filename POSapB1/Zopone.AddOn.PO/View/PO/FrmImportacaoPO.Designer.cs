namespace Zopone.AddOn.PO.View.PO
{
    partial class FrmImportacaoPO
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
            this.BtImportar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mskDataI = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mskDataF = new System.Windows.Forms.MaskedTextBox();
            this.CbEmpresa = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pbProgresso = new System.Windows.Forms.ProgressBar();
            this.dgDadosPO = new System.Windows.Forms.DataGridView();
            this.Importar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.po_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.poNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.po_lis_DataConfirmacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mensagem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnPesquisar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgDadosPO)).BeginInit();
            this.SuspendLayout();
            // 
            // BtImportar
            // 
            this.BtImportar.Location = new System.Drawing.Point(1189, 45);
            this.BtImportar.Name = "BtImportar";
            this.BtImportar.Size = new System.Drawing.Size(75, 23);
            this.BtImportar.TabIndex = 0;
            this.BtImportar.Text = "Importar";
            this.BtImportar.UseVisualStyleBackColor = true;
            this.BtImportar.Click += new System.EventHandler(this.BtImportar_Click);
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
            // CbEmpresa
            // 
            this.CbEmpresa.FormattingEnabled = true;
            this.CbEmpresa.Items.AddRange(new object[] {
            "Huawei",
            "Ericsson"});
            this.CbEmpresa.Location = new System.Drawing.Point(90, 37);
            this.CbEmpresa.Name = "CbEmpresa";
            this.CbEmpresa.Size = new System.Drawing.Size(236, 24);
            this.CbEmpresa.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Empresa";
            // 
            // pbProgresso
            // 
            this.pbProgresso.Location = new System.Drawing.Point(2, 722);
            this.pbProgresso.Name = "pbProgresso";
            this.pbProgresso.Size = new System.Drawing.Size(1271, 23);
            this.pbProgresso.TabIndex = 7;
            // 
            // dgDadosPO
            // 
            this.dgDadosPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDadosPO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Importar,
            this.po_id,
            this.poNumber,
            this.po_lis_DataConfirmacao,
            this.Status,
            this.Mensagem});
            this.dgDadosPO.Location = new System.Drawing.Point(12, 79);
            this.dgDadosPO.Name = "dgDadosPO";
            this.dgDadosPO.RowHeadersWidth = 51;
            this.dgDadosPO.RowTemplate.Height = 24;
            this.dgDadosPO.Size = new System.Drawing.Size(1252, 637);
            this.dgDadosPO.TabIndex = 8;
            // 
            // Importar
            // 
            this.Importar.FalseValue = "0";
            this.Importar.HeaderText = "Importar";
            this.Importar.IndeterminateValue = "0";
            this.Importar.MinimumWidth = 6;
            this.Importar.Name = "Importar";
            this.Importar.TrueValue = "1";
            this.Importar.Width = 125;
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
            // poNumber
            // 
            this.poNumber.DataPropertyName = "poNumber";
            this.poNumber.HeaderText = "PO Number";
            this.poNumber.MinimumWidth = 6;
            this.poNumber.Name = "poNumber";
            this.poNumber.ReadOnly = true;
            this.poNumber.Width = 125;
            // 
            // po_lis_DataConfirmacao
            // 
            this.po_lis_DataConfirmacao.DataPropertyName = "po_lis_DataConfirmacao";
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
            this.BtnPesquisar.Location = new System.Drawing.Point(332, 42);
            this.BtnPesquisar.Name = "BtnPesquisar";
            this.BtnPesquisar.Size = new System.Drawing.Size(94, 23);
            this.BtnPesquisar.TabIndex = 9;
            this.BtnPesquisar.Text = "Pesquisar";
            this.BtnPesquisar.UseVisualStyleBackColor = true;
            this.BtnPesquisar.Click += new System.EventHandler(this.BtnPesquisar_Click);
            // 
            // FrmImportacaoPO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 750);
            this.Controls.Add(this.BtnPesquisar);
            this.Controls.Add(this.dgDadosPO);
            this.Controls.Add(this.pbProgresso);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CbEmpresa);
            this.Controls.Add(this.mskDataF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mskDataI);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtImportar);
            this.Name = "FrmImportacaoPO";
            this.Text = "Importação de PO";
            this.Load += new System.EventHandler(this.FrmImportacaoPO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgDadosPO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtImportar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mskDataI;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mskDataF;
        private System.Windows.Forms.ComboBox CbEmpresa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pbProgresso;
        private System.Windows.Forms.DataGridView dgDadosPO;
        private System.Windows.Forms.Button BtnPesquisar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Importar;
        private System.Windows.Forms.DataGridViewTextBoxColumn po_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn poNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn po_lis_DataConfirmacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mensagem;
    }
}