// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseServerTest.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   Defines the BaseServerTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Integration.Tests.Utils
{
    using System;
    using System.Data.Entity;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.Owin.Testing;

    using NUnit.Framework;

    using Owin;

    /// <summary>
    /// The base server test.
    /// </summary>
    [TestFixture]
    public abstract class BaseServerTest
    {
        /// <summary>
        /// The default server uri.
        /// </summary>
        protected const string DefaultServerUri = "http://localhost/";

        /// <summary>
        /// The customers test seeds initializer.
        /// </summary>
        protected static readonly TestsSeedsInitializer TestsDb = InitializeDatabase();

        /// <summary>
        /// Gets the server.
        /// </summary>
        private TestServer server;

        /// <summary>
        /// Gets the uri.
        /// </summary>
        protected abstract Uri Uri { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        protected TestServer Server
        {
            get
            {
                return this.server;
            }
        }

        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.server = TestServer.Create(app =>
            {
                var startup = new Startup();
                startup.ConfigureOAuth(app);

                var config = new HttpConfiguration();
                config.RegisterWebApi();
                config.RegisterUnityComponents();
                AutomapperConfig.Initialize();

                app.UseWebApi(config);
            });

            this.PostSetup(this.server);
        }

        /// <summary>
        /// The teardown.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            if (this.server != null)
            {
                this.server.Dispose();
            }
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual async Task<HttpResponseMessage> GetAsync()
        {
            return await this.server.CreateRequest(this.Uri.AbsolutePath).GetAsync();
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual async Task<HttpResponseMessage> GetAsync(string language)
        {
            return await this.Server.CreateRequest(this.Uri.AbsolutePath)
//                                    .AddHeader("Accept-Language", language ?? Language.English)
                                    .GetAsync();
        }

        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="TModel">
        /// Type of the resource
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual async Task<HttpResponseMessage> PostAsync<TModel>(TModel model)
        {
            return
                await
                    this.server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
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
        protected virtual async Task<HttpResponseMessage> PostAsync<TModel>(TModel model, string language)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
//                        .AddHeader("Accept-Language", language ?? Language.English)
                        .PostAsync();
        }

        /// <summary>
        /// The post setup.
        /// </summary>
        /// <param name="testServer">
        /// The server.
        /// </param>
        protected virtual void PostSetup(TestServer testServer)
        {
        }

        /// <summary>
        /// The put async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <typeparam name="TModel">
        /// The Model
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual async Task<HttpResponseMessage> PutAsync<TModel>(TModel model)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
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
        /// The Model
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual async Task<HttpResponseMessage> PutAsync<TModel>(TModel model, string language)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
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
        protected virtual async Task<HttpResponseMessage> DeleteAsync<TModel>(TModel model)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
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
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected virtual async Task<HttpResponseMessage> DeleteAsync<TModel>(TModel model, string language)
        {
            return
                await
                    this.Server.CreateRequest(this.Uri.AbsolutePath)
                        .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
//                        .AddHeader("Accept-Language", language ?? Language.English)
                        .SendAsync(HttpMethod.Delete.ToString());
        }

        /// <summary>
        /// The initialize database.
        /// </summary>
        /// <returns>
        /// The <see cref="TestsDb"/>.
        /// </returns>
        private static TestsSeedsInitializer InitializeDatabase()
        {
            try
            {
                var testsSeeds = new TestsSeedsInitializer();
                Database.SetInitializer(testsSeeds);
                //using (var administration = new AdministrationContext())
                //{
                //    administration.Database.Initialize(false);
                //}

                return testsSeeds;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex, "Initializing test database");
                throw;
            }
        }

    }
}
