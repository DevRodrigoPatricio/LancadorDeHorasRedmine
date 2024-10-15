using LancarHoras.Domain;
using LancarHoras.Domain.Entities;
using LancarHoras.Domain.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static LancarHoras.Utils;

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

        public string getSituacaoById(int id)
        {
            try
            {
                return horasTrabalhadasRepository.getSituacaoById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task LancarHorasNaAPI(HorasTrabalhadas horasTrabalhadas)
        {
            var config = GetConfig();
            var url = $"{config.url}/time_entries.json";
            var id_atividade = ActivityMapper.GetActivityId(horasTrabalhadas.Atividade);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", config.chaveKey);

                var requestData = new
                {
                    time_entry = new
                    {
                        issue_id = horasTrabalhadas.Tarefa,
                        spent_on = horasTrabalhadas.Data.ToString("yyyy-MM-dd"),
                        hours = horasTrabalhadas.Duracao.ToString(@"hh\:mm"),
                        activity_id = id_atividade,
                        comments = horasTrabalhadas.Comentario
                    }
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro ao lançar horas: {responseContent}");
                }

                horasTrabalhadas.Situacao = "LANCADO";
                atualizaHoras(horasTrabalhadas);
            }
        }

    }
}
