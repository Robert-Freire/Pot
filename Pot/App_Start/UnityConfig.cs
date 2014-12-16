// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityConfig.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The unity config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NWL.Web.Api
{
    using System.Web.Http;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// The unity config.
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// The register components.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public static void RegisterComponents(HttpConfiguration config)
        {
            if (config == null)
            {
                return;
            }

            var container = new UnityContainer();

            //container.RegisterType<ICustomerFactory, CustomerFactory>()
            //         .RegisterType<ICustomerContext, CustomerContext>(new HierarchicalLifetimeManager());


            //container.RegisterType<IAuthFactory, AuthFactory>();

            //config.DependencyResolver = new UnityResolver(container);
        }
    }
}