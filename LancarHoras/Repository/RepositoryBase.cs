using LancarHoras.Domain.Interface;
using LancarHoras.Repository.EntityFrameworkConfig;
using LancarHoras.TransactionControl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected DbContext Context { get; private set; }

        public RepositoryBase()
        {
  
        }

        public void Add(TEntity obj)
        {
            emTransacao();
            Context.Set<TEntity>().Add(obj);
            Context.SaveChanges();
        }

        public void Delete(TEntity obj)
        {
            emTransacao();
            Context.Set<TEntity>().Remove(obj);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Get(id);
            Delete(obj);
        }

        public TEntity Get(int id)
        {
            emTransacao();
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            emTransacao();
            return Context.Set<TEntity>().ToList();
        }

        public void Update(TEntity obj)
        {
            emTransacao();
            Context.Entry(obj).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void NewContext()
        {
            Context = new BaseContext();
        }

        public bool emTransacao()
        {
            if (Transactions.emTransacao)
                Context = Transactions.baseContext;
            else
                Context = new BaseContext();

            return Transactions.emTransacao;
        }

        public int GetNextID(string nomeColuna, TEntity classe)
        {
            emTransacao();

            var nomeTab = classe.GetType().ToString().Split('.');
            var query = string.Format("SELECT MAX({0}) AS {0} FROM {1} ", nomeColuna, nomeTab[nomeTab.Length - 1]);
            var retorno = Context.Database.SqlQuery<int?>(query).ToList();

            if (retorno.Count == 0) return 1;

            return retorno[0] == null ? 1 : (int)retorno[0] + 1;
        }

        public long GetNextIDLong(string nomeColuna, TEntity classe)
        {
            emTransacao();

            var nomeTab = classe.GetType().ToString().Split('.');
            var query = string.Format("SELECT MAX({0}) AS {0} FROM {1} ", nomeColuna, nomeTab[nomeTab.Length - 1]);
            var retorno = Context.Database.SqlQuery<long?>(query).ToList();

            if (retorno.Count == 0) return 1;

            return retorno[0] == null ? 1 : (long)retorno[0] + 1;
        }

        public int UpdateField(string nomeTab, string campoTab, string campoCond, int id)
        {
            emTransacao();

            var query = string.Format("UPDATE {0} SET {1} WHERE {2} = {3}", nomeTab, campoTab, campoCond, id);
            int retorno = Context.Database.ExecuteSqlCommand(query);

            return retorno;
        }
    }
}
