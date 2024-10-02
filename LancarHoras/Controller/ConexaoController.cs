using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Controller
{
    public class ConexaoController
    {
        public bool testarConexao(string strConexao)
        {
            try
            {
                var connection = new SqlConnection(strConexao);
                SqlConnectionStringBuilder cnn = new SqlConnectionStringBuilder(strConexao);
                connection.Open();
                connection.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
