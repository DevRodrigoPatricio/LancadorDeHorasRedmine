using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Domain.Entities
{
    [Table("Config")]
    public class Config
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; } 
        public string url { get; set; }
        public string chaveKey { get; set; }
    }
}
