namespace PO
{
    partial class frmLogin
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
            BtOk = new Button();
            txtUsuario = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txtSenha = new TextBox();
            label3 = new Label();
            cbEmpresa = new ComboBox();
            BtCancelar = new Button();
            SuspendLayout();
            // 
            // BtOk
            // 
            BtOk.Location = new Point(16, 112);
            BtOk.Name = "BtOk";
            BtOk.Size = new Size(94, 29);
            BtOk.TabIndex = 0;
            BtOk.Text = "Login";
            BtOk.UseVisualStyleBackColor = true;
            BtOk.Click += BtOk_Click;
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(102, 9);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(294, 27);
            txtUsuario.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 16);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 2;
            label1.Text = "Usuário:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 46);
            label2.Name = "label2";
            label2.Size = new Size(52, 20);
            label2.TabIndex = 3;
            label2.Text = "Senha:";
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(102, 38);
            txtSenha.Name = "txtSenha";
            txtSenha.PasswordChar = '*';
            txtSenha.Size = new Size(294, 27);
            txtSenha.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 74);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 5;
            label3.Text = "Empresa:";
            // 
            // cbEmpresa
            // 
            cbEmpresa.FormattingEnabled = true;
            cbEmpresa.Location = new Point(102, 67);
            cbEmpresa.Name = "cbEmpresa";
            cbEmpresa.Size = new Size(294, 28);
            cbEmpresa.TabIndex = 6;
            // 
            // BtCancelar
            // 
            BtCancelar.Location = new Point(116, 112);
            BtCancelar.Name = "BtCancelar";
            BtCancelar.Size = new Size(94, 29);
            BtCancelar.TabIndex = 7;
            BtCancelar.Text = "Cancelar";
            BtCancelar.UseVisualStyleBackColor = true;
            BtCancelar.Click += BtCancelar_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(408, 165);
            Controls.Add(BtCancelar);
            Controls.Add(cbEmpresa);
            Controls.Add(label3);
            Controls.Add(txtSenha);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtUsuario);
            Controls.Add(BtOk);
            Name = "frmLogin";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtOk;
        private TextBox txtUsuario;
        private Label label1;
        private Label label2;
        private TextBox txtSenha;
        private Label label3;
        private ComboBox cbEmpresa;
        private Button BtCancelar;
    }
}