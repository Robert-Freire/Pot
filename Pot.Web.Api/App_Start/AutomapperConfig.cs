// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutomapperConfig.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the AutomapperConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api
{
    using System.Diagnostics.CodeAnalysis;

    using Pot.Web.Api.Model;

    /// <summary>
    /// The automapper config.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    internal static class AutomapperConfig
    {
        /// <summary>
        /// The initialize.
        /// </summary>
        internal static void Initialize()
        {
            UserResource.InitializeMappings();
            ProjectResource.InitializeMappings();
        }
    }
}