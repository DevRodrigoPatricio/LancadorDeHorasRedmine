
namespace LancarHoras
{
    partial class FrmMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.dgvHoras = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChavekey = new System.Windows.Forms.TextBox();
            this.btnLancar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtData = new System.Windows.Forms.MaskedTextBox();
            this.dtpVendasDe = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoras)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHoras
            // 
            this.dgvHoras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoras.Location = new System.Drawing.Point(12, 76);
            this.dgvHoras.Name = "dgvHoras";
            this.dgvHoras.Size = new System.Drawing.Size(813, 442);
            this.dgvHoras.TabIndex = 0;
            this.dgvHoras.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoras_CellClick);
            this.dgvHoras.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoras_CellContentClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 5;
            // 
            // txtChavekey
            // 
            this.txtChavekey.Location = new System.Drawing.Point(117, 48);
            this.txtChavekey.Name = "txtChavekey";
            this.txtChavekey.Size = new System.Drawing.Size(415, 20);
            this.txtChavekey.TabIndex = 2;
            // 
            // btnLancar
            // 
            this.btnLancar.Enabled = false;
            this.btnLancar.Location = new System.Drawing.Point(739, 45);
            this.btnLancar.Name = "btnLancar";
            this.btnLancar.Size = new System.Drawing.Size(89, 25);
            this.btnLancar.TabIndex = 3;
            this.btnLancar.Text = "Lançar";
            this.btnLancar.UseVisualStyleBackColor = true;
            this.btnLancar.Click += new System.EventHandler(this.btnLancar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(644, 45);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(89, 25);
            this.btnSalvar.TabIndex = 4;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Chave de Acesso :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Url do Redmine:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(117, 20);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(415, 20);
            this.txtUrl.TabIndex = 8;
            // 
            // txtData
            // 
            this.txtData.Enabled = false;
            this.txtData.Location = new System.Drawing.Point(728, 20);
            this.txtData.Mask = "##/##/####";
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(77, 20);
            this.txtData.TabIndex = 27;
            this.txtData.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txtDtVendasDe_MaskInputRejected);
            // 
            // dtpVendasDe
            // 
            this.dtpVendasDe.Location = new System.Drawing.Point(807, 20);
            this.dtpVendasDe.Name = "dtpVendasDe";
            this.dtpVendasDe.Size = new System.Drawing.Size(18, 20);
            this.dtpVendasDe.TabIndex = 26;
            this.dtpVendasDe.ValueChanged += new System.EventHandler(this.dtpVendasDe_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(613, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Data de lançamento :";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(549, 45);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(89, 25);
            this.btnImportar.TabIndex = 29;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 522);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.dtpVendasDe);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnLancar);
            this.Controls.Add(this.txtChavekey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvHoras);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lançar Horas Redmine";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoras)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHoras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChavekey;
        private System.Windows.Forms.Button btnLancar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.MaskedTextBox txtData;
        private System.Windows.Forms.DateTimePicker dtpVendasDe;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnImportar;
    }
}

