using LancarHoras.Domain;
using LancarHoras.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Repository
{
    public class HorasTrabalhadasRepository : RepositoryBase<HorasTrabalhadas>, IHorasTrabalhadasRepository
    {

        public List<HorasTrabalhadas> getHorasPorData(DateTime data)
        {
            try
            {
                emTransacao();
                return Context.Set<HorasTrabalhadas>().Where(h => h.Data == data).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string getSituacaoById(int id)
        {
            try
            {
                emTransacao();
                var situacao = Context.Set<HorasTrabalhadas>()
                                      .Where(h => h.Id == id)
                                      .Select(h => h.Situacao)
                                      .FirstOrDefault();

                return situacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
