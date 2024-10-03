using LancarHoras.Controller;
using LancarHoras.Repository.EntityFrameworkConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            dgvHoras.CellValidating += dgvHoras_CellValidating;
        }

        private void MontaGridFilial()
        {
            dgvHoras.Rows.Clear();
            dgvHoras.ColumnCount = 6;
            dgvHoras.RowHeadersVisible = true;
            dgvHoras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoras.AllowUserToAddRows = true;

            dgvHoras.Columns[0].HeaderText = "Data";
            dgvHoras.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[0].Width = 80;

            dgvHoras.Columns[1].HeaderText = "Tarefa";
            dgvHoras.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[1].Width = 60;

            dgvHoras.Columns[2].HeaderText = "Horário Inicial";
            dgvHoras.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[2].Width = 55;

            dgvHoras.Columns[3].HeaderText = "Horário Final";
            dgvHoras.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[3].Width = 55;

            dgvHoras.Columns[4].HeaderText = "Duração";
            dgvHoras.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoras.Columns[4].Width = 55;

            dgvHoras.Columns[5].HeaderText = "Comentário";
            dgvHoras.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHoras.Columns[5].Width = 267;

            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
            statusColumn.HeaderText = "Atividade";
            statusColumn.Name = "Atividade";
            statusColumn.Items.AddRange("Análise", "Desenvolvimento", "Design", "Revisão por pares", "Reunião", "Teste", "Apoio", "Outros"); // Adicionar opções
            statusColumn.Width = 120;
            dgvHoras.Columns.Add(statusColumn);

            dgvHoras.Columns[0].ReadOnly = false;
            dgvHoras.Columns[1].ReadOnly = false;
            dgvHoras.Columns[2].ReadOnly = false;
            dgvHoras.Columns[3].ReadOnly = false;
            dgvHoras.Columns[4].ReadOnly = true;
            dgvHoras.Columns[5].ReadOnly = false;
            dgvHoras.Columns["Atividade"].ReadOnly = false;

            dgvHoras.CellValueChanged += dgvHoras_CellValueChanged;
            dgvHoras.CurrentCellDirtyStateChanged += dgvHoras_CurrentCellDirtyStateChanged;
        }

        // Evento que calcula a duração ao mudar o valor de uma célula
        private void dgvHoras_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a edição foi nas colunas de horário
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                CalcularDuracao(dgvHoras.Rows[e.RowIndex]);
            }
        }

        private void dgvHoras_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvHoras.IsCurrentCellDirty)
            {
                dgvHoras.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void CalcularDuracao(DataGridViewRow row)
        {
            if (DateTime.TryParse(row.Cells[2].Value?.ToString(), out DateTime horarioInicial) &&
                DateTime.TryParse(row.Cells[3].Value?.ToString(), out DateTime horarioFinal))
            {
                TimeSpan duracao = horarioFinal - horarioInicial;

                row.Cells[4].Value = duracao.ToString(@"hh\:mm");
            }
            else
            {
                row.Cells[4].Value = string.Empty;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (Globals.tipoConfigBD == Enums.TipoConfigBD.Definido)
                if (!validarStrConexao())
                {
                    this.Close();
                }
        }

        private bool validarStrConexao()
        {
            if (string.IsNullOrWhiteSpace(Globals.strConexaoEntityFramework))
            {
                MessageBox.Show("A string de conexão está vazia. Verifique as configurações.", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (var connection = new SqlConnection(Globals.strConexaoEntityFramework))
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar conectar ao banco de dados: " + ex.Message, "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // FAZER TRATAMENTO DE ERRO
        private void dgvHoras_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string inputValue = e.FormattedValue.ToString();

                if (inputValue.Length == 8 && int.TryParse(inputValue, out _))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(inputValue, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        dgvHoras.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = parsedDate;
                        dgvHoras.Rows[e.RowIndex].Cells[e.ColumnIndex].ValueType = typeof(DateTime);

                        dgvHoras.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Format = "dd/MM/yyyy";
                    }
                }
            }
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                string inputValue = e.FormattedValue.ToString();

                if (inputValue.Length == 4 && int.TryParse(inputValue, out _))
                {
                    string formattedTime = $"{inputValue.Substring(0, 2)}:{inputValue.Substring(2, 2)}";
                    TimeSpan parsedTime;
                    if (TimeSpan.TryParse(formattedTime, out parsedTime))
                    {
                        dgvHoras.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = formattedTime;
                    }
                }

            }
        }
    }
}
