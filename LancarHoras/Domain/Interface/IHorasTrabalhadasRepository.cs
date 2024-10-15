using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Domain.Interface
{
    public interface IHorasTrabalhadasRepository :IRepositoryBase<HorasTrabalhadas>
    {
        List<HorasTrabalhadas> getHorasPorData(DateTime data);
        string getSituacaoById(int id);
    }
}
