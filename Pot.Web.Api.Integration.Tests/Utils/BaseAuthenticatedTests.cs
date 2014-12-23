// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseAuthenticatedTests.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the BaseAuthenticatedTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Integration.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;

    using Microsoft.Owin.Testing;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The base authenticated tests.
    /// </summary>
    public abstract class BaseAuthenticatedTests : BaseServerTest
    {
        /// <summary>
        /// Gets the client id.
        /// </summary>
   //     private const string ClientId = AuthConfiguration.TestWebApiClientId;

        /// <summary>
        /// The token.
        /// </summary>
        private string token;

        /// <summary>
        /// Gets the username.
        /// </summary>
        //protected virtual string UserName
        //{
        //    get
        //    {
        //        return AuthTestConfiguration.DefaultUser;
        //    }
        //}

        /// <summary>
        /// Gets the password.
        /// </summary>
        //protected virtual string Password
        //{
        //    get
        //    {
        //        return AuthTestConfiguration.DefaultPassword;
        //    }
        //}

        /// <summary>
        /// The post setup.
        /// </summary>
        /// <param name="testServer">
        /// The server.
        /// </param>
        //protected override void PostSetup(TestServer testServer)
        //{
        //    if (testServer == null)
        //    {
        //        throw new ArgumentNullException("testServer");
        //    }

      //      Database.SetInitializer(new AuthTestConfiguration());

            //var tokenDetails = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("grant_type", "password"),
            //    new KeyValuePair<string, string>("username", this.UserName),
            //    new KeyValuePair<string, string>("password", this.Password),
            //    new KeyValuePair<string, string>("client_id", ClientId)
            //};

            //var tokenPostData = new FormUrlEncodedContent(tokenDetails);
            //var tokenUri = new Uri("/Token", UriKind.RelativeOrAbsolute);
            //var tokenResult = testServer.HttpClient.PostAsync(tokenUri, tokenPostData).Result;
            //tokenResult.StatusCode.Should().Be(HttpStatusCode.OK, "the user %1 is authenticated", this.UserName);

            //var body = JObject.Parse(tokenResult.Content.ReadAsStringAsync().Result);

            //this.token = (string)body["access_token"];
       // }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> GetAsync(string language)
        {
            return await this.Server.CreateRequest(this.Uri.AbsolutePath)
                .AddHeader("Authorization", "Bearer " + this.token)
//                .AddHeader("Accept-Language", language ?? Language.English)
                .GetAsync();
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> GetAsync()
        {
            return await this.Server.CreateRequest(this.Uri.AbsolutePath)
  //              .AddHeader("Authorization", "Bearer " + this.token)
                .GetAsync();
        }

        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="TModel">
        /// Type of Resource 
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> PostAsync<TModel>(TModel model)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                        .AddHeader("Authorization", "Bearer " + this.token)
                        .PostAsync();
        }

        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <typeparam name="TModel">
        /// The model
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> PostAsync<TModel>(TModel model, string language)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
//                        .AddHeader("Accept-Language", language ?? Language.English)
                        .AddHeader("Authorization", "Bearer " + this.token)
                        .PostAsync();
        }

        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="TModel">
        /// Type of Resource 
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> PutAsync<TModel>(TModel model)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                        .AddHeader("Authorization", "Bearer " + this.token)
                        .SendAsync(HttpMethod.Put.ToString());
        }

        /// <summary>
        /// The put async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <typeparam name="TModel">
        /// The model
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> PutAsync<TModel>(TModel model, string language)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                        .AddHeader("Authorization", "Bearer " + this.token)
//                        .AddHeader("Accept-Language", language ?? Language.English)
                        .SendAsync(HttpMethod.Put.ToString());
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="TModel">
        /// The model
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> DeleteAsync<TModel>(TModel model)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                        .AddHeader("Authorization", "Bearer " + this.token)
                        .SendAsync(HttpMethod.Delete.ToString());
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <typeparam name="TModel">
        /// The model
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task<HttpResponseMessage> DeleteAsync<TModel>(TModel model, string language)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                        .AddHeader("Authorization", "Bearer " + this.token)
//                        .AddHeader("Accept-Language", language ?? Language.English)
                        .SendAsync(HttpMethod.Delete.ToString());
        }
    }
}
