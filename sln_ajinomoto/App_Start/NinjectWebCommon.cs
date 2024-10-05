using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using RetoTecnicoAjinomoto.Service.Implementation;
using RetoTecnicoAjinomoto.Service.Interface;
using System;
using System.Web;

namespace RetoTecnicoAjinomoto.App_Start
{
   
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Inicia la aplicación
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Detiene la aplicación
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Crea el núcleo de Ninject que gestionará las dependencias
        /// </summary>
        /// <returns>El núcleo de Ninject.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Registra los servicios con el contenedor de inyección de dependencias
        /// </summary>
        /// <param name="kernel">El núcleo de Ninject.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Aquí registras tus dependencias
            kernel.Bind<ITareaService>().To<TareaService>().InRequestScope();
        }
    }
}