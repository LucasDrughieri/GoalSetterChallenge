using Autofac;
using Core.Interfaces.Repository;

namespace Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRepository>().As<IClientRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RentalRepository>().As<IRentalRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
