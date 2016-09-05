using Autofac;
using FileManager.Core.Data.Repositories;
using FileManager.Data.Repositories;
using FileManager.Web.References;

namespace FileManager.Web.Capsule.Modules
{
    public class RepositoryCapsuleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ReferencedAssemblies.Repositories).
                Where(_ => _.Name.EndsWith("Repository")).
                AsImplementedInterfaces().
                InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
        }

    }
}