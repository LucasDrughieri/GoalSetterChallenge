using Autofac;
using Core.Interfaces.Repository;

namespace Repository
{
    /// <summary>
    /// Configuration of dependency injection for repository project
    /// </summary>
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRepository>().As<IClientRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RentalRepository>().As<IRentalRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Repository>().As<IRepository>().InstancePerLifetimeScope();
        }
    }
}
