using LancarHoras.Repository.EntityFrameworkConfig;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.TransactionControl
{
    public class Transactions
    {

        public static BaseContext baseContext = null;
        private static DbContextTransaction dbTrans;

        public static bool emTransacao = false;

        public static void BeginTransaction()
        {
            if (emTransacao) return;
            baseContext = new BaseContext();
            dbTrans = baseContext.Database.BeginTransaction();
            emTransacao = true;
        }

        public static void CommitTransaction()
        {
            if (emTransacao) dbTrans.Commit();
            emTransacao = false;
        }
        public static void RollbackTransaction()
        {
            if (emTransacao) dbTrans.Rollback();
            emTransacao = false;
        }

    }
}
