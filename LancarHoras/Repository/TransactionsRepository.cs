using LancarHoras.Domain.Interface;
using LancarHoras.TransactionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Repository
{
    public class TransactionsRepository : ITransactionsRepository
    {
        public void BeginTransaction()
        {
            Transactions.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Transactions.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            Transactions.RollbackTransaction();
        }
    }
}
