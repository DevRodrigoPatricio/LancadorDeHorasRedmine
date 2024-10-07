using LancarHoras.Repository.EntityFrameworkConfig;
using LancarHoras.Ui.Integrador.View;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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

            if (Globals.tipoConfigBD == Enums.TipoConfigBD.Definido)
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
                MessageBox.Show("Aconteceu um erro inesperado. \nInforme: " + ex.Message,
                    "Lançador de horas Redmine", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }



        private static void verificarTipoBD()
        {
            if (File.Exists(Path.Combine(Globals.pathLocal, Globals.NOME_ARQ_DEFINIDO)))
            {
                Globals.tipoConfigBD = Enums.TipoConfigBD.Definido;
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
            //ALTERAR O NOME
            string nomeAplicacao = "LancadorHoras.Redmine";
            var processos = Process.GetProcesses();
            foreach (var processo in processos)
            {
                if (processo.ProcessName.ToString().Equals(nomeAplicacao))
                {
                    quant++;
                    if (quant > 1)
                    {
                        MessageBox.Show("O programa está aberto! \n" +
                        "Aperte Alt + Tab ou verifique se está oculto próximo ao relógio.", "Lançador de horas Redmine",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
