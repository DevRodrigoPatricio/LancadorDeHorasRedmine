using LancarHoras.Controller;
using LancarHoras.Repository.EntityFrameworkConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LancarHoras
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            MontaGridFilial();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void MontaGridFilial()
        {
            dgvHoras.Rows.Clear();
            dgvHoras.ColumnCount = 6; // Aumentar o número de colunas
            dgvHoras.RowHeadersVisible = true;
            dgvHoras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoras.AllowUserToAddRows = true; // Permitir adição de linhas

            // Configurar colunas
            dgvHoras.Columns[0].HeaderText = "Data";
            dgvHoras.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[0].Width = 100;

            dgvHoras.Columns[1].HeaderText = "Tarefa";
            dgvHoras.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[1].Width = 75;

            dgvHoras.Columns[2].HeaderText = "Horário Inicial";
            dgvHoras.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHoras.Columns[2].Width = 120;

            dgvHoras.Columns[3].HeaderText = "Horário Final";
            dgvHoras.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHoras.Columns[3].Width = 120;

            dgvHoras.Columns[4].HeaderText = "Duração";
            dgvHoras.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHoras.Columns[4].Width = 100;

            dgvHoras.Columns[5].HeaderText = "Comentário";
            dgvHoras.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHoras.Columns[5].Width = 250;

            // Adicionar a coluna ComboBox para Status
            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
            statusColumn.HeaderText = "Status";
            statusColumn.Name = "Status";
            statusColumn.Items.AddRange("Pendente", "Em Progresso", "Concluído"); // Adicionar opções
            statusColumn.Width = 120;
            dgvHoras.Columns.Add(statusColumn); // Adicionar a coluna ComboBox ao DataGridView

            // Definir colunas como somente leitura, exceto a coluna de Duração e Status
            dgvHoras.Columns[0].ReadOnly = false; // Permitir edição na coluna Data
            dgvHoras.Columns[1].ReadOnly = false; // Permitir edição na coluna Tarefa
            dgvHoras.Columns[2].ReadOnly = false; // Permitir edição na coluna Horário Inicial
            dgvHoras.Columns[3].ReadOnly = false; // Permitir edição na coluna Horário Final
            dgvHoras.Columns[4].ReadOnly = true; // Duração calculada deve ser somente leitura
            dgvHoras.Columns[5].ReadOnly = false; // Permitir edição na coluna Comentário
            dgvHoras.Columns["Status"].ReadOnly = false; // Permitir edição na coluna Status

            // Conectar os eventos
            dgvHoras.CellValueChanged += dgvHoras_CellValueChanged;
            dgvHoras.CurrentCellDirtyStateChanged += dgvHoras_CurrentCellDirtyStateChanged;
        }

        // Evento que calcula a duração ao mudar o valor de uma célula
        private void dgvHoras_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a edição foi nas colunas de horário
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                // Chama o método para calcular a duração na linha editada
                CalcularDuracao(dgvHoras.Rows[e.RowIndex]);
            }
        }

        // Necessário para garantir que o evento seja disparado
        private void dgvHoras_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvHoras.IsCurrentCellDirty)
            {
                dgvHoras.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void CalcularDuracao(DataGridViewRow row)
        {
            // Obter horários inicial e final das células
            if (DateTime.TryParse(row.Cells[2].Value?.ToString(), out DateTime horarioInicial) &&
                DateTime.TryParse(row.Cells[3].Value?.ToString(), out DateTime horarioFinal))
            {
                // Calcular a diferença entre os horários
                TimeSpan duracao = horarioFinal - horarioInicial;

                // Mostrar a duração no formato HH:mm
                row.Cells[4].Value = duracao.ToString(@"hh\:mm");
            }
            else
            {
                // Limpar a célula de duração se os valores não forem válidos
                row.Cells[4].Value = string.Empty;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {


            if (Globals.tipoConfigBD == Enums.TipoConfigBD.UnicoBanco)
                if (!validarStrConexao())
                {
        
                    this.Close();
                }
        }


       

    }
}
