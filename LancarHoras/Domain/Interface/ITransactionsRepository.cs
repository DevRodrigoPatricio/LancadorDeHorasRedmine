using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Domain.Interface
{
    public interface ITransactionsRepository
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
