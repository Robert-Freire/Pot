// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleAuthorizationServerProvider.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The simple authorization server provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;

    using Pot.Data;
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;

    /// <summary>
    /// The simple authorization server provider.
    /// </summary>
    public sealed class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider, IDisposable
    {
        private readonly IUserFactory userFactory;

       // private readonly IRepositoryAsync<User> repoClients;

        /// <summary>
        /// The repo of authorizations users.
        /// </summary>
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleAuthorizationServerProvider"/> class.
        /// </summary>
        /// <param name="authFactory">
        /// The authorization factory.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Authorization factory is null
        /// </exception>
        //public SimpleAuthorizationServerProvider(IUserFactory userFactory)
        //{
        //    this.userFactory = userFactory;
        //    if (userFactory == null)
        //    {
        //        throw new ArgumentNullException("userFactory");
        //    }

        //    this.repoUsers = userFactory.UsersRepository;
        //}

        public SimpleAuthorizationServerProvider()
        {

            //this.userFactory = new UserFactory(new PotDbContext());

            this.userManager = new UserManager<User>(new UserStore<User>(new PotDbContext()));

        }

        /// <summary>
        /// responsible for validating the “Client”
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clientId;
            //string clientSecret;

            //if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            //{
            //    context.TryGetFormCredentials(out clientId, out clientSecret);
            //}

            //if (context.ClientId == null)
            //{
            //    // Remove the comments from the below line context.SetError, and invalidate context 
            //    // if you want to force sending clientId/secrects once obtain access tokens. 
            //    //                context.Validated();
            //    context.SetError("invalid_clientId", "Application ClientId should be sent.");
            //    return;
            //}

            //var client = await this.repoClients.FindAsync(context.ClientId);

            //if (client == null)
            //{
            //    context.SetError("invalid_clientId", string.Format("The client application'{0}' is not registered in the system.", context.ClientId));
            //    return;
            //}

            //if (client.ApplicationType == ApplicationType.NativeConfidential)
            //{
            //    if (string.IsNullOrWhiteSpace(clientSecret))
            //    {
            //        context.SetError("invalid_clientId", "Client secret should be sent.");
            //        return;
            //    }

            //    if (client.Secret != clientSecret.GetHash())
            //    {
            //        context.SetError("invalid_clientId", "Client secret is invalid.");
            //        return;
            //    }
            //}

            //if (!client.Active)
            //{
            //    context.SetError("invalid_clientId", "Client is inactive.");
            //    return;
            //}

            //context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            //context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifetime.ToString(CultureInfo.InvariantCulture));

            context.Validated();
        }

        /// <summary>
        /// responsible to validate the username and password sent to the authorization server’s token endpoint
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user = await this.userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            var props =
                new AuthenticationProperties(
                    new Dictionary<string, string>
                    {
                        { "as:client_id", context.ClientId ?? string.Empty },
                        { "userName", context.UserName }
                    });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        /// <summary>
        /// The token endpoint.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// The grant refresh token.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            this.userFactory.Dispose();
        }
    }
}