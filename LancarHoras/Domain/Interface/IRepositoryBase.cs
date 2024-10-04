using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Domain.Interface
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void NewContext();
        void Add(TEntity obj);
        void Delete(TEntity obj);
        void Delete(int id);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        int GetNextID(string nomeColuna, TEntity classe);
        long GetNextIDLong(string nomeColuna, TEntity classe);
    }
}
