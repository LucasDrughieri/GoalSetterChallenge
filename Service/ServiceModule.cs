using Autofac;
using Core.Interfaces.Services;
using Repository;

namespace Service
{
    /// <summary>
    /// Configuration of dependency injection for service project
    /// </summary>
    public class ServiceModule : Module
    {
        public ServiceModule(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new RepositoryModule());
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientService>().As<IClientService>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleService>().As<IVehicleService>().InstancePerLifetimeScope();
            builder.RegisterType<RentalService>().As<IRentalService>().InstancePerLifetimeScope();
        }
    }
}
