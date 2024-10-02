using IntegradorGShop.Ui.Integrador.Controller;
using IntegradorGShop.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegradorGShop.Ui.Integrador.View
{
    public partial class FrmConexao : Form
    {
        private bool conValidada = false;
        private const string STR_BASE_CONEXAO = "Provider=sqloledb;Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
        private const string END_STR_CONEXAO = ";MultipleActiveResultSets=True;App=EntityFramework";
        private ConexaoController _conexaoController;
        private string _nomeArqConexao = "GSsql.gs";
        private string _pathSaveArqGs = Globals.pathLocal;
        private string _tituloTela = "Conexão ao Banco de dados - {0}";
        private string _banco = "GShop";
        private string _login = "gshop_login";
        private string _senha = "gshop_senha";
        

        public FrmConexao()
        {
            InitializeComponent();
        }

        public void setDadosLogin(string banco, string login, string senha)
        {
            _banco = banco;
            _login = login;
            _senha = senha;
        }

        public FrmConexao(string nomeArqConexao, string path, string sistema = "Group Shopping")
            : this()
        {
            _nomeArqConexao = nomeArqConexao;
            _pathSaveArqGs = path;
            this.Text = string.Format(_tituloTela, sistema);
        }

        private void txtServidor_KeyPress(object sender, KeyPressEventArgs e)
        {
            conexaoValidada(false);
        }

        private void conexaoValidada(bool value)
        {
            cmdConfirmar.Enabled = value;
            conValidada = value;
        }

        private void txtBanco_KeyPress(object sender, KeyPressEventArgs e)
        {
            conexaoValidada(false);
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            conexaoValidada(false);
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            conexaoValidada(false);
        }

        private void FrmConexao_Load(object sender, EventArgs e)
        {
            conexaoValidada(false);
            _conexaoController = new ConexaoController();
            txtBanco.Text = _banco;
            txtLogin.Text = _login;
            txtSenha.Text = _senha;
        }

        public bool isConexaoValidada()
        {
            return conValidada;
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            conexaoValidada(false);
            this.Close();
        }

        private void cmdTestarConexao_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validarCampos()) return;
                
                cmdTestarConexao.Enabled = false;
                conexaoValidada(false);
                this.Refresh();
                var stringConexaoEntity = string.Format(STR_BASE_CONEXAO, txtServidor.Text, txtBanco.Text, 
                    txtLogin.Text, txtSenha.Text).Replace("Provider=sqloledb;","") + END_STR_CONEXAO;

                if (_conexaoController.testarConexao(stringConexaoEntity))
                {
                    MessageBox.Show("Conexão estabelecida com sucesso!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conexaoValidada(true);
                }
                else
                {
                    MessageBox.Show("Conexão com o banco de dados não efetuada.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception)
            {
                conexaoValidada(false);
                MessageBox.Show("Conexão com o banco de dados não efetuada.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            cmdTestarConexao.Enabled = true;
        }

        private bool validarCampos()
        {
            bool validado = true;
            if (txtBanco.Text.Equals("")) validado = false;
            if (txtLogin.Text.Equals("")) validado = false;
            if (txtSenha.Text.Equals("")) validado = false;
            if (txtServidor.Text.Equals("")) validado = false;

            if (!validado)
                MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return validado;
        }

        private void cmdConfirmar_Click(object sender, EventArgs e)
        {
            if (isConexaoValidada())
            {
                var stringConexaoEntity = string.Format(STR_BASE_CONEXAO, txtServidor.Text, txtBanco.Text,
                    txtLogin.Text, txtSenha.Text);
                stringConexaoEntity = Utils.CriptografarSimples(stringConexaoEntity, 1);

                if (File.Exists(Path.Combine(_pathSaveArqGs, _nomeArqConexao)))
                    File.Delete(Path.Combine(_pathSaveArqGs, _nomeArqConexao));

                Utils.salvarArquivoTexto(stringConexaoEntity, Path.Combine(_pathSaveArqGs, _nomeArqConexao), true);

                this.Close();
            }
        }
    }
}
