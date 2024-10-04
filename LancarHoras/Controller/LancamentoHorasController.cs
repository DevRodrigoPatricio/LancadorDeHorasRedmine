using LancarHoras.Domain;
using LancarHoras.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Controller
{
   public  class LancamentoHorasController
    {
        private IHorasTrabalhadasRepository horasTrabalhadasRepository;

        public LancamentoHorasController(IHorasTrabalhadasRepository horasTrabalhadasRepository)
        {
            this.horasTrabalhadasRepository = horasTrabalhadasRepository;
        }

        public List<HorasTrabalhadas> getHorasLancadas()
        {
            return horasTrabalhadasRepository.GetAll().ToList();
        }

    }
}
