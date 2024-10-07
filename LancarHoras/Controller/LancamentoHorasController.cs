using LancarHoras.Domain;
using LancarHoras.Domain.Entities;
using LancarHoras.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Controller
{
    public class LancamentoHorasController
    {
        private IHorasTrabalhadasRepository horasTrabalhadasRepository;
        private IConfigRepository configRepository;
        private ITransactionsRepository transactionsRepository;

        public LancamentoHorasController(IHorasTrabalhadasRepository horasTrabalhadasRepository,
            IConfigRepository configRepository, ITransactionsRepository transactionsRepository)
        {
            this.horasTrabalhadasRepository = horasTrabalhadasRepository;
            this.configRepository = configRepository;
            this.transactionsRepository = transactionsRepository;
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
                hora.Id = horasTrabalhadasRepository.GetNextID("ID", hora);
                horasTrabalhadasRepository.Add(hora);
            }
            catch (Exception ex)
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

        public void saveConfig(Config config)
        {

            try
            {
                transactionsRepository.BeginTransaction();
                var configAux = configRepository.GetAll().ToList();
                if (configAux.Count == 0)
                {
                    config.id = configRepository.GetNextID("Id", config);
                    configRepository.Add(config);
                }
                else
                {
                    configAux[0].url = config.url;
                    configAux[0].chaveKey = config.chaveKey;
                    configRepository.Update(configAux[0]);

                }
                transactionsRepository.CommitTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Config GetConfig()
        {
            try
            {
                var config = configRepository.GetAll().ToList();
                if (config.Count == 0)
                {
                    return new Config()
                    {
                        url = "",
                        chaveKey = ""
                    };
                }
                return config[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<HorasTrabalhadas> gethorasPorData(DateTime data)
        {
            try
            {
                return horasTrabalhadasRepository.getHorasPorData(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
