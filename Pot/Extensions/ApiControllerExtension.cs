// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiControllerExtension.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the ApiControllerExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// The API controller extension.
    /// </summary>
    public static class ApiControllerExtension
    {
        /// <summary>
        /// The validations error result.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        /// <returns>
        /// The <see cref="Pot.Web.Api.ValidationsErrorResult"/>.
        /// </returns>
        public static ValidationsErrorResult ValidationsErrorResult(ApiController controller, IEnumerable<ValidationResult> errors)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            return new ValidationsErrorResult(controller.Request, errors.ToList());
        }
    }
}