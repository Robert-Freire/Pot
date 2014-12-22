// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationsErrorResult.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The validations error result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Newtonsoft.Json;

    /// <summary>
    /// The validations error result.
    /// </summary>
    public class ValidationsErrorResult : IHttpActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationsErrorResult"/> class.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public ValidationsErrorResult(HttpRequestMessage request, IList<ValidationResult> errors)
        {
            this.Request = request;
            this.Errors = errors;
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public HttpRequestMessage Request { get; private set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IList<ValidationResult> Errors { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return JsonConvert.SerializeObject(this.Errors.Select(error => error.ErrorMessage).ToList());
            }
        }

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.ExecuteResult());
        }

        /// <summary>
        /// The execute result.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        public HttpResponseMessage ExecuteResult()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(this.ErrorMessage),
                RequestMessage = this.Request
            };

            return response;
        }
    }
}