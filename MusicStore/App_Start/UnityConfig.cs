using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using MusicStore.Repository;
using MusicStore.Service;
using MusicStore.Controllers;
namespace MusicStore
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType( typeof(IGenericRepository<>), typeof(GenericRepository<>) );
            container.RegisterType<IValidationDictionary, ModelStateWrapper>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType <IAlbumRepository, AlbumRepository>();
          //  container.RegisterType<IGenreRepository, GenreRepository>();
            container.RegisterType<IAlbumService, AlbumService>();
          //  container.RegisterType<IGenreService, GenreService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}