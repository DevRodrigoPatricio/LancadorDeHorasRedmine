using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace LancarHoras.Repository
{
    public class RepositoryInstaller
    {
        public static void Install(IUnityContainer container)
        {
            try
            {
//                container.RegisterType(typeof(IRepositoryBase<>), typeof(RepositoryBase<>), new TransientLifetimeManager());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\n" + ex.StackTrace);
            }
        }
    }
}
