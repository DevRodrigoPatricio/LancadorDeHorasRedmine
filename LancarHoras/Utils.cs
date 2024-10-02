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

    }
}
