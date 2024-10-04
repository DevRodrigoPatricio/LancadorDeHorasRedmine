using LancarHoras.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras.Repository.EntityFrameworkConfig
{
  public partial class BaseContext : DbContext
    {
        public BaseContext() : base(Globals.strConexaoEntityFramework)
        {
            base.Database.CommandTimeout = 20000;
        }

        public virtual DbSet<HorasTrabalhadas> HorasTrabalhadas { get; set; }
    }
}
