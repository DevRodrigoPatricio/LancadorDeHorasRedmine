using LancarHoras.Domain.Interface;
using System;
using Unity;
using Unity.Lifetime;

namespace LancarHoras.Repository
{
    public class RepositoryInstaller
    {
        public static void Install(IUnityContainer container)
        {
            try
            {
                container.RegisterType(typeof(IRepositoryBase<>), typeof(RepositoryBase<>), new TransientLifetimeManager());
                container.RegisterType<IHorasTrabalhadasRepository, HorasTrabalhadasRepository>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\n" + ex.StackTrace);
            }
        }
    }
}
