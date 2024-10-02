using IntegradorGShop.Ui.Integrador.View;
using LancarHoras.Repository.EntityFrameworkConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LancarHoras
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            iniciarVariaveisGlobais();
            if (verificarSeAppEstaExec()) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            verificarTipoBD();

            if (Globals.tipoConfigBD == Enums.TipoConfigBD.NaoDefinido)
            {
                FrmConexao frmConexao = new FrmConexao();
                frmConexao.ShowDialog();
                //if (frmConexao.isSair()) return;
            }

            if (Globals.tipoConfigBD == Enums.TipoConfigBD.UnicoBanco)
            {

                if (!Utils.verificarSeExisteGSsql())
                {
                    FrmConexao frmConexao = new FrmConexao();
                    frmConexao.ShowDialog();
                    if (!frmConexao.isConexaoValidada()) return;
                }

                Globals.strConexaoEntityFramework = "";
                const string endStringConexao = ";MultipleActiveResultSets=True;App=EntityFramework";
                var strConexaoAux = Utils.RetornaArquivoGS(true);
                if (strConexaoAux != null && !strConexaoAux.Equals(""))
                    Globals.strConexaoEntityFramework = Utils.RetornaArquivoGS(true) + endStringConexao;

            }

            try
            {
                Application.Run(new FrmMain());
            }
            catch (Exception ex)
            {
                var caminho = Utils.criarEstruturaDePastas(Globals.pathLocal, "Fails");
                Utils.salvarLogException(caminho,
                    string.Format("{0}_{1}.{2}", "LogFail", DateTime.Now.ToString("yyyyMMddHHmmss"), "log"),
                    ex, "Program", "");
                MessageBox.Show("Aconteceu um erro inesperado. \nInforme: " + ex.Message,
                    "Integrador Group Shopping", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void verificarTipoBD()
        {

            if (File.Exists(Path.Combine(Globals.pathLocal, Globals.NOME_ARQ_MULTIBD)))
            {
                Globals.tipoConfigBD = Enums.TipoConfigBD.MultiBanco;
                return;
            }
            //senão verificar se existe um Gssql.gs
            if (File.Exists(Path.Combine(Globals.pathLocal, Globals.NOME_ARQ_UNICOBD)))
            {
                Globals.tipoConfigBD = Enums.TipoConfigBD.UnicoBanco;
                return;
            }
            Globals.tipoConfigBD = Enums.TipoConfigBD.NaoDefinido;
        }

        private static string getVersaoAssembly()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        private static void iniciarVariaveisGlobais()
        {
            Globals.tipoIntegracao = Enums.TipoIntegracao.Manual;
            Globals.strConexaoEntityFramework = "";
            Globals.versao = getVersaoAssembly();
            Globals.pathLocal = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Globals.tipoConfigBD = Enums.TipoConfigBD.NaoDefinido;
        }

        private static bool verificarSeAppEstaExec()
        {
            int quant = 0;
            string nomeAplicacao = "IntegradorGShop.Ui.Integrador";
            var processos = Process.GetProcesses();
            foreach (var processo in processos)
            {
                if (processo.ProcessName.ToString().Equals(nomeAplicacao))
                {
                    quant++;
                    if (quant > 1)
                    {
                        MessageBox.Show("O Integrador Group Shopping está aberto! \n" +
                        "Aperte Alt + Tab ou verifique se está oculto próximo ao relógio.", "Integrador Group Shopping",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            return false;
        }

        public static string criarEstruturaDePastas(string pathBase, params string[] pastas)
        {
            var pathAux = pathBase;
            var pastasAux = pastas.ToList().Where(p => p != null).ToList();
            foreach (var pasta in pastasAux)
            {
                pathAux = Path.Combine(pathAux, pasta);
                if (!Directory.Exists(pathAux))
                    Directory.CreateDirectory(pathAux);
            }
            return pathAux;
        }
    }
}
