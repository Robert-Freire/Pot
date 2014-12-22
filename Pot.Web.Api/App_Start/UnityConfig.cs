// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityConfig.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The unity config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api
{
    using System.Web.Http;

    using Microsoft.Practices.Unity;

    using Pot.Data;
    using Pot.Data.SQLServer;

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
        public static void RegisterUnityComponents(this HttpConfiguration config)
        {
            if (config == null)
            {
                return;
            }

            var container = new UnityContainer();

            container.RegisterType<IUserFactory, UserFactory>();
             //        .RegisterType<IPotContext, CustomerContext>(new HierarchicalLifetimeManager());


            //container.RegisterType<IAuthFactory, AuthFactory>();

            config.DependencyResolver = new UnityResolver(container);
        }
    }
}