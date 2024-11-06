using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using LancarHoras.Domain;

namespace LancarHoras.Controller
{
    public class LerExcelController
    {
        public static List<HorasTrabalhadas> LerHorasTrabalhadasDoExcel(string caminhoArquivo)
        {
            var listaHorasTrabalhadas = new List<HorasTrabalhadas>();

            // Configura o EPPlus para não lançar exceções em arquivos não licenciados.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo Excel não encontrado.", caminhoArquivo);

            using (var pacote = new ExcelPackage(new FileInfo(caminhoArquivo)))
            {
                var planilha = pacote.Workbook.Worksheets[0];

                int linhaInicial = 2;
                int linhaFinal = planilha.Dimension.End.Row;

                for (int linha = linhaInicial; linha <= linhaFinal; linha++)
                {
                    var horasTrabalhadas = new HorasTrabalhadas();

                    if (DateTime.TryParse(planilha.Cells[linha, 1].Text, out DateTime data))
                    {
                        horasTrabalhadas.Data = data;
                    }
                    else
                    {
                        horasTrabalhadas.Data = DateTime.MinValue; 
                    }

                    horasTrabalhadas.Tarefa = planilha.Cells[linha, 2].Text;

                    if (TimeSpan.TryParse(planilha.Cells[linha, 3].Text, out TimeSpan horarioInicial))
                    {
                        horasTrabalhadas.HorarioInicial = horarioInicial;
                    }
                    else
                    {
                        horasTrabalhadas.HorarioInicial = TimeSpan.Zero;
                    }

                    if (TimeSpan.TryParse(planilha.Cells[linha, 4].Text, out TimeSpan horarioFinal))
                    {
                        horasTrabalhadas.HorarioFinal = horarioFinal;
                    }
                    else
                    {
                        horasTrabalhadas.HorarioFinal = TimeSpan.Zero;
                    }

                    if (TimeSpan.TryParse(planilha.Cells[linha, 5].Text, out TimeSpan duracao))
                    {
                        horasTrabalhadas.Duracao = duracao;
                    }
                    else
                    {
                        horasTrabalhadas.Duracao = TimeSpan.Zero;
                    }

                    horasTrabalhadas.Atividade = planilha.Cells[linha, 6].Text;
                    horasTrabalhadas.Comentario = planilha.Cells[linha, 7].Text;

                    horasTrabalhadas.Situacao = "ATUALIZADO"; 

                    listaHorasTrabalhadas.Add(horasTrabalhadas);
                }
            }

            return listaHorasTrabalhadas;
        }
    }
}
