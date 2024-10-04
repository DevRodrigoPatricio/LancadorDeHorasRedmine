using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public static DialogResult MensagemAlerta(string mensagem, Enums.TituloAlerta tituloAlerta, Enums.BotaoAlerta botaoAlerta, Enums.IconeAlerta iconeAlerta) //#107504
        {
            string _titulo = Utils.getNameEnum<Enums.TituloAlerta>(tituloAlerta);

            return MessageBox.Show(mensagem, _titulo, (MessageBoxButtons)botaoAlerta, (MessageBoxIcon)iconeAlerta);
        }

        public static DialogResult MensagemAlerta(string mensagem, Enums.TituloAlerta tituloAlerta) //#107504
        {
            DialogResult dialogResult;
            string _titulo = Utils.getNameEnum<Enums.TituloAlerta>(tituloAlerta); //Retorna a descrição do enum (annotation "Display")

            switch (tituloAlerta)
            {
                case Enums.TituloAlerta.AVISO:
                    dialogResult = MessageBox.Show(mensagem, _titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case Enums.TituloAlerta.CONFIRM:
                    dialogResult = MessageBox.Show(mensagem, _titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    break;
                case Enums.TituloAlerta.INCONSIS:
                    dialogResult = MessageBox.Show(mensagem, _titulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                default:
                    dialogResult = MessageBox.Show(mensagem, _titulo, MessageBoxButtons.OK, MessageBoxIcon.None);
                    break;
            }

            return dialogResult;
        }

        public static string getNameEnum<T>(T value)
        {
            var type = value.GetType();
            var fieldName = Enum.GetName(type, value);
            var objs = type.GetField(fieldName).GetCustomAttributes(typeof(DisplayAttribute), false);

            return objs.Length > 0 ? ((DisplayAttribute)objs[0]).Name : value.ToString();
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
