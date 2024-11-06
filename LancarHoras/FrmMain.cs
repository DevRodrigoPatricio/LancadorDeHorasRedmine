using LancarHoras.Controller;
using LancarHoras.Domain;
using LancarHoras.Domain.Entities;
using LancarHoras.Repository;
using LancarHoras.Repository.EntityFrameworkConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
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
            txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

        private async void btnLancar_Click(object sender, EventArgs e)
        {
            try
            {
                List<HorasTrabalhadas> horasLancadas = lancamentoHorasController.gethorasPorData(Convert.ToDateTime(txtData.Text));

                foreach (HorasTrabalhadas horaLancada in horasLancadas)
                {
                    if (horaLancada.Situacao == "INCLUIDO" || horaLancada.Situacao == "ATUALIZADO")
                    {
                        await lancamentoHorasController.LancarHorasNaAPI(horaLancada);
                    }
                }

                MessageBox.Show("Horas lançadas com sucesso!");

                CarregarDadosDoBanco();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao lançar horas: {ex.Message}");
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvHoras.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()) ||
                        row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()) ||
                        row.Cells[2].Value == null || string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ||
                        row.Cells[3].Value == null || string.IsNullOrWhiteSpace(row.Cells[3].Value.ToString()) ||
                        row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString()) ||
                        row.Cells["Atividade"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Atividade"].Value.ToString()))
                    {
                        MessageBox.Show("Preencha todos os campos antes de salvar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    HorasTrabalhadas hora = new HorasTrabalhadas();

                    hora.Data = DateTime.Parse(row.Cells[0].Value.ToString());
                    hora.Tarefa = row.Cells[1].Value.ToString();
                    hora.HorarioInicial = TimeSpan.Parse(row.Cells[2].Value.ToString());
                    hora.HorarioFinal = TimeSpan.Parse(row.Cells[3].Value.ToString());
                    hora.Duracao = TimeSpan.Parse(row.Cells[4].Value.ToString());
                    hora.Comentario = row.Cells[5]?.Value?.ToString() ?? "";
                    hora.Atividade = row.Cells["Atividade"].Value.ToString();

                    string idValue = row.Cells["ID"].Value?.ToString();
                    int? id = !string.IsNullOrEmpty(idValue) ? (int?)Convert.ToInt32(idValue) : null;

                    hora.Situacao = "INCLUIDO";

                    if (id.HasValue)
                    {
                        if (lancamentoHorasController.getSituacaoById(id.Value) == "LANCADO")
                        {
                            continue;
                        }
                        else
                        {
                            hora.Id = (int)id;
                            hora.Situacao = "ATUALIZADO";
                            lancamentoHorasController.atualizaHoras(hora);
                        }
                    }
                    else
                    {
                        lancamentoHorasController.criarHoras(hora);
                    }
                }

                Config configuracoes = new Config
                {
                    chaveKey = txtChavekey.Text,
                    url = txtUrl.Text
                };
                lancamentoHorasController.saveConfig(configuracoes);

                MessageBox.Show("Dados salvos com sucesso!", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLancar.Enabled = true;

                CarregarDadosDoBanco();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarDadosDoBanco()
        {
            try
            {
                MontaGridHoras();
                List<HorasTrabalhadas> horasLancadas = lancamentoHorasController.gethorasPorData(Convert.ToDateTime(txtData.Text));
                foreach (HorasTrabalhadas horas in horasLancadas)
                {
                    int rowIndex = dgvHoras.Rows.Add(
                        horas.Data.ToString("dd/MM/yyyy"),
                        horas.Tarefa.ToString(),
                        horas.HorarioInicial.ToString(@"hh\:mm"),
                        horas.HorarioFinal.ToString(@"hh\:mm"),
                        horas.Duracao.ToString(@"hh\:mm"),
                        horas.Comentario.ToString(),
                        horas.Atividade.ToString(),
                        horas.Id.ToString()
                    );
                    if (horas.Situacao == "LANCADO")
                    {
                        dgvHoras.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                        dgvHoras.Rows[rowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgvHoras.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
                Config config = lancamentoHorasController.GetConfig();
                txtUrl.Text = config.url;
                txtChavekey.Text = config.chaveKey;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpVendasDe_ValueChanged(object sender, EventArgs e)
        {
            txtData.Text = dtpVendasDe.Value.ToString("dd/MM/yyyy");
            CarregarDadosDoBanco();


        }

        private void txtDtVendasDe_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Atenção: A importação de dados poderá resultar em duplicações, "+
                    "caso a planilha contenha registros que já foram cadastrados anteriormente no banco de dados.\n\n" +
                    "Deseja continuar com a importação?",
                    "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string caminhoArquivo = "";

                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = "c:\\";
                        openFileDialog.Filter = "Arquivos Excel (*.xlsx)|*.xlsx";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            caminhoArquivo = openFileDialog.FileName;
                        }

                    }
                    List<HorasTrabalhadas> horasTrabalhadas = LerExcelController.LerHorasTrabalhadasDoExcel(caminhoArquivo);

                    foreach (var horas in horasTrabalhadas)
                    {
                        if (horas.Tarefa != "" && horas.Tarefa != null)
                        {
                            lancamentoHorasController.criarHoras(horas);
                        }
                    }

                    btnSalvar_Click(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}

