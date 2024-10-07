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
            try
            {
                return horasTrabalhadasRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            
        }

        public void criarHoras(HorasTrabalhadas hora)
        {
            try
            {
                horasTrabalhadasRepository.Add(hora);
            }catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public void atualizaHoras(HorasTrabalhadas hora)
        {
            try
            {
                horasTrabalhadasRepository.Update(hora);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
