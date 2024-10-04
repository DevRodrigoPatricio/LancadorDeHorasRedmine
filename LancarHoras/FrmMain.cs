using LancarHoras.Controller;
using LancarHoras.Domain;
using LancarHoras.Domain.Interface;
using LancarHoras.Repository;
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
using Unity;

namespace LancarHoras
{
    public partial class FrmMain : Form
    {
        private UnityContainer _container;
        private LancamentoHorasController lancamentoHorasController;

        public FrmMain()
        {
            InitializeComponent(); ;
            dgvHoras.CellValidating += dgvHoras_CellValidating;
        }

        private void MontaGridHoras()
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
            dgvHoras.Columns[5].Width = 297;

            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
            statusColumn.HeaderText = "Atividade";
            statusColumn.Name = "Atividade";
            statusColumn.Items.AddRange("Análise", "Desenvolvimento", "Design", "Revisão por pares", "Reunião", "Teste", "Apoio", "Outros"); // Adicionar opções
            statusColumn.Width = 120;
            dgvHoras.Columns.Add(statusColumn);

            dgvHoras.Columns.Add("ID", "ID");
            dgvHoras.Columns["ID"].Visible = false;

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

        private void dgvHoras_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
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
            this.Text += " - Versão: " + Globals.versao;
            inicializaDependencias();
            MontaGridHoras();
            CarregarDadosDoBanco();
            if (Globals.tipoConfigBD == Enums.TipoConfigBD.Definido)
                if (!validarStrConexao())
                {
                    Utils.MensagemAlerta("Não foi possível conectar com o banco de dados. \n" +
                        "Verifique sua conexão e teste novamente.", Enums.TituloAlerta.INCONSIS);
                    this.Close();
                }
        }

        private void inicializaDependencias()
        {
            _container = new UnityContainer();
            RepositoryInstaller.Install(_container);
            lancamentoHorasController = _container.Resolve<LancamentoHorasController>();
        }


        private bool validarStrConexao()
        {
            try
            {
                ConexaoController conexaoController;
                conexaoController = new ConexaoController();
                return conexaoController.testarConexao(Globals.strConexaoEntityFramework);
            }
            catch
            {
                return false;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void salvar_Click(object sender, EventArgs e)
        {
            string connectionString = Globals.strConexaoEntityFramework;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (DataGridViewRow row in dgvHoras.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()) ||
                            row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ||
                            row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ||
                            row.Cells[3].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()) ||
                            row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()) ||
                            row.Cells[5].Value == null || string.IsNullOrWhiteSpace(row.Cells[5].Value.ToString()) ||
                            row.Cells["Atividade"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Atividade"].Value.ToString()))
                        {
                            MessageBox.Show("Preencha todos os campos antes de salvar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }



                        DateTime data = DateTime.Parse(row.Cells[0].Value.ToString());
                        string tarefa = row.Cells[1].Value.ToString();
                        TimeSpan horarioInicial = TimeSpan.Parse(row.Cells[2].Value.ToString());
                        TimeSpan horarioFinal = TimeSpan.Parse(row.Cells[3].Value.ToString());
                        TimeSpan duracao = TimeSpan.Parse(row.Cells[4].Value.ToString());
                        string comentario = row.Cells[5].Value.ToString();
                        string atividade = row.Cells["Atividade"].Value.ToString();

                        string idValue = row.Cells["ID"].Value?.ToString();
                        int? id = !string.IsNullOrEmpty(idValue) ? (int?)Convert.ToInt32(idValue) : null;

                        string query;
                        if (id.HasValue)
                        {
                            query = "UPDATE HorasTrabalhadas SET Data = @Data, Tarefa = @Tarefa, HorarioInicial = @HorarioInicial, " +
                                    "HorarioFinal = @HorarioFinal, Duracao = @Duracao, Comentario = @Comentario, Atividade = @Atividade " +
                                    "WHERE ID = @ID";
                        }
                        else
                        {
                            query = "INSERT INTO HorasTrabalhadas (Data, Tarefa, HorarioInicial, HorarioFinal, Duracao, Comentario, Atividade) " +
                                    "VALUES (@Data, @Tarefa, @HorarioInicial, @HorarioFinal, @Duracao, @Comentario, @Atividade)";
                        }

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Data", data);
                            command.Parameters.AddWithValue("@Tarefa", tarefa);
                            command.Parameters.AddWithValue("@HorarioInicial", horarioInicial);
                            command.Parameters.AddWithValue("@HorarioFinal", horarioFinal);
                            command.Parameters.AddWithValue("@Duracao", duracao);
                            command.Parameters.AddWithValue("@Comentario", comentario);
                            command.Parameters.AddWithValue("@Atividade", atividade);

                            if (id.HasValue)
                            {
                                command.Parameters.AddWithValue("@ID", id.Value);
                            }

                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Dados salvos com sucesso!", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarDadosDoBanco()
        {
            try
            {
                List<HorasTrabalhadas> horasLancadas = lancamentoHorasController.getHorasLancadas();
                foreach (HorasTrabalhadas horas in horasLancadas)
                {
                    dgvHoras.Rows.Add(
                       horas.Data.ToString("dd/MM/yyyy"),
                        horas.Tarefa.ToString(),
                        horas.HorarioInicial.ToString(@"hh\:mm"),
                        horas.HorarioFinal.ToString(@"hh\:mm"),
                        horas.Duracao.ToString(@"hh\:mm"),
                        horas.Comentario.ToString(),
                        horas.Atividade.ToString(),
                       horas.Id.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}

