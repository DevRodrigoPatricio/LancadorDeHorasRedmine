using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras
{
    public class Utils
    {
        private Utils()
        {

        }

        public static bool verificarSeExisteGSsql()
        {
            string PathDLL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathConfig = Path.Combine(PathDLL, "GSsql.gs");
            return File.Exists(pathConfig);
        }

        public static string RetornaArquivoGS(bool RemoveProvider = true, string caminho = "", string nomeArqGssql = "")
        {
            string PathDLL;
            if (caminho.Equals(""))
                PathDLL = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            else
                PathDLL = caminho;

            string pathConfig = Path.Combine(PathDLL, (nomeArqGssql.Equals("") ? "GSsql.gs" : nomeArqGssql));
            string linha = "";
            List<string> mensagemLinha = new List<string>();

            if (File.Exists(pathConfig))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(pathConfig, Encoding.Default, false))
                    {
                        while ((linha = sr.ReadLine()) != null)
                        {
                            mensagemLinha.Add(DescriptografarSimples(linha, 1));

                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

            if (mensagemLinha.Count > 0)
            {
                return RemoveProvider ? mensagemLinha[0].ToString().Replace("Provider=sqloledb;", "") : mensagemLinha[0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static string DescriptografarSimples(string texto, int valoraux)
        {
            try
            {
                string resultFinal = "";
                string chave;
                chave = "SEGURANCA".ToLower();
                int chaveNum = 0;
                char result;

                for (int i = 0; i <= texto.Length - 1; i++)
                {
                    string s = texto.Substring(i, 1);
                    byte[] caracter = new byte[System.Text.Encoding.Default.GetByteCount(s)];
                    caracter = System.Text.Encoding.Default.GetBytes(s);

                    result = Convert.ToChar(Convert.ToChar(caracter[0]) - Convert.ToChar(chave.Substring(chaveNum, 1)) - valoraux);

                    resultFinal = resultFinal + Encoding.Default.GetChars(new byte[] { Convert.ToByte(result) })[0];
                    chaveNum = chaveNum + 1;

                    if (chaveNum >= chave.Length)
                    {
                        chaveNum = 0;
                    }
                }
                return resultFinal;
            }
            catch
            {
                throw;
            }
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

        public static void salvarArquivoTexto(string texto, string caminhoNomeExtensao, bool encodingDefault = false)
        {
            List<string> textoAux = new List<string>();
            textoAux.Add(texto);
            salvarArquivoTexto(textoAux, caminhoNomeExtensao, encodingDefault);
        }


        public static void salvarArquivoTexto(List<string> texto, string caminhoNomeExtensao, bool encodingDefault = false)
        {
            string arqAux = caminhoNomeExtensao;
            int numArqRepetido = 0;
            while (true)
            {
                if (File.Exists(arqAux))
                {
                    numArqRepetido++;
                    string[] arqQuebrado = caminhoNomeExtensao.Split('.');
                    arqAux = arqQuebrado[0] + "_" + numArqRepetido + "." + arqQuebrado[1];
                }
                else
                {
                    break;
                }
            }

            FileStream fs = new FileStream(arqAux, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, (encodingDefault ? Encoding.Default : Encoding.UTF8));

            texto.ForEach(sw.WriteLine);
            sw.Flush();
            sw.Close();
            fs.Close();
        }



        public static string CriptografarSimples(string texto, int valoraux)
        {
            try
            {
                string resultFinal = "";
                string chave = "SEGURANCA".ToLower();
                int chaveNum = 0;
                char result;

                for (int i = 0; i <= texto.Length - 1; i++)
                {
                    result = Convert.ToChar(Convert.ToChar(texto.Substring(i, 1)) + Convert.ToChar(chave.Substring(chaveNum, 1)) + valoraux);

                    resultFinal = resultFinal + Encoding.Default.GetChars(new byte[] { Convert.ToByte(result) })[0];
                    chaveNum = chaveNum + 1;

                    if (chaveNum >= chave.Length)
                    {
                        chaveNum = 0;
                    }
                }
                return resultFinal;
            }
            catch
            {
                throw;
            }
        }


    }
}
