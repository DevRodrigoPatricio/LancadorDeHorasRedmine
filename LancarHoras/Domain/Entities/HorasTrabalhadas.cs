using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Domain
{
    [Table("HorasTrabalhadas")]
    public class HorasTrabalhadas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Tarefa { get; set; }
        public TimeSpan HorarioInicial { get; set; }
        public TimeSpan HorarioFinal { get; set; }
        public TimeSpan Duracao { get; set; }
        public string Comentario { get; set; }
        public string Atividade { get; set; }
    }
}
