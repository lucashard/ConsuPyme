using System.Web.Mvc;
using Ninject.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ConsuPyme_MVC.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(ConsuPyme_MVC.App_Start.NinjectWebCommon), "Stop")]

namespace ConsuPyme_MVC.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using ConsuPyme_MVC.Models;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAcarreos>().To<AcarreoServicio>();
            kernel.Bind<IDeposito>().To<DepositoServicio>();
            kernel.Bind<IPosicion_Arancelaria>().To<Posicion_ArancelariaServicio>();
            kernel.Bind<IProducto>().To<ProductoServicio>();
            kernel.Bind<IDespachantes>().To<DespachantesServicios>();
            kernel.Bind<IFacturas>().To<FacturaServicio>();
            kernel.Bind<IDespachos>().To<DespachosServicio>();
            kernel.Bind<Ireporte>().To<ReporteServicio>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }        
    }
}
