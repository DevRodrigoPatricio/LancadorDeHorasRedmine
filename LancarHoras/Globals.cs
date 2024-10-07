using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Repository.EntityFrameworkConfig
{
    public class Globals
    {
        private Globals()
        { }


        public static string strConexaoEntityFramework = @"Data Source=GROUP-NOTE02311\SQLEXPRESS2022;Initial Catalog=HorasRedmine;User Id=gshop_login;Password=gshop_senha;MultipleActiveResultSets=True;App=EntityFramework";
        public static string strConexaoEntityFramework2 { get; set; }
        public static string pathLocal { get; set; }
        public static Enums.TipoIntegracao tipoIntegracao { get; set; }
        public static string versao { get; set; }
        public const string NOME_ARQ_DEFINIDO = "GSsql.gs";
        public static Enums.TipoConfigBD tipoConfigBD { get; set; }
    }
}
