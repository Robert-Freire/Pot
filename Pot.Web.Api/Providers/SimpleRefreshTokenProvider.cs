//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="SimpleRefreshTokenProvider.cs" company="Nova Language Services">
////   Copyright © Nova Language Services 2014
//// </copyright>
//// <summary>
////   The simple refresh token provider.
//// </summary>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Pot.Web.Api
//{
//    using System;
//    using System.Threading.Tasks;

//    using Microsoft.Owin.Security.Infrastructure;

//    using Pot.Data;

//    /// <summary>
//    /// The simple refresh token provider.
//    /// </summary>
//    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider, IDisposable
//    {
//        private readonly IUserFactory userFactory;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="SimpleRefreshTokenProvider"/> class.
//        /// </summary>
//        /// <param name="authFactory">
//        /// The authorization factory.
//        /// </param>
//        public SimpleRefreshTokenProvider(IUserFactory authFactory)
//        {
//            if (authFactory == null)
//            {
//                throw new ArgumentNullException("authFactory");
//            }

//            this.authFactory = authFactory;
//            this.repoRefreshTokens = authFactory.GetRepositoryAsync<RefreshToken>();
//            this.unitOfWorkAsync = authFactory.UnitOfWorkAsync;
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="SimpleRefreshTokenProvider"/> class.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
//            Justification = "The objects are dispossed through the factory")]
//        public SimpleRefreshTokenProvider()
//        {
//            this.authFactory = new AuthFactory(new AuthContext());

//            this.repoRefreshTokens = this.authFactory.GetRepositoryAsync<RefreshToken>();
//            this.unitOfWorkAsync = this.authFactory.UnitOfWorkAsync;
//        }

//        /// <summary>
//        /// The create.
//        /// </summary>
//        /// <param name="context">
//        /// The context.
//        /// </param>
//        /// <exception cref="NotImplementedException">
//        /// Not implemented
//        /// </exception>
//        public void Create(AuthenticationTokenCreateContext context)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// The create async.
//        /// </summary>
//        /// <param name="context">
//        /// The context.
//        /// </param>
//        /// <returns>
//        /// The <see cref="Task"/>.
//        /// </returns>
//        public async Task CreateAsync(AuthenticationTokenCreateContext context)
//        {
//            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

//            if (string.IsNullOrEmpty(clientid))
//            {
//                return;
//            }

//            var refreshTokenId = Guid.NewGuid().ToString("N");

//            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

//            var token = new RefreshToken
//            {
//                Id = refreshTokenId.GetHash(),
//                ClientId = clientid,
//                Subject = context.Ticket.Identity.Name,
//                IssuedUtc = DateTime.UtcNow,
//                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
//            };

//            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
//            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

//            token.ProtectedTicket = context.SerializeTicket();

//            var result = await this.RefreshToken(token);

//            if (result)
//            {
//                context.SetToken(refreshTokenId);
//            }
//        }

//        /// <summary>
//        /// The receive 
//        /// </summary>
//        /// <param name="context">
//        /// The context.
//        /// </param>
//        /// <exception cref="NotImplementedException">
//        /// Not implemented
//        /// </exception>
//        public void Receive(AuthenticationTokenReceiveContext context)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// The receive async.
//        /// </summary>
//        /// <param name="context">
//        /// The context.
//        /// </param>
//        /// <returns>
//        /// The <see cref="Task"/>.
//        /// </returns>
//        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
//        {
//            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
//            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

//            var hashedTokenId = context.Token.GetHash();

//            var refreshToken = await this.repoRefreshTokens.FindAsync(hashedTokenId).WithCurrentCulture();

//            if (refreshToken == null)
//            {
//                return;
//            }

//            context.DeserializeTicket(refreshToken.ProtectedTicket);
//            if (await this.repoRefreshTokens.DeleteAsync(hashedTokenId))
//            {
//                await this.unitOfWorkAsync.SaveChangesAsync();
//            }
//        }

//        /// <summary>
//        /// The dispose.
//        /// </summary>
//        public void Dispose()
//        {
//            this.Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        /// <summary>
//        /// The dispose.
//        /// </summary>
//        /// <param name="disposing">
//        /// The disposing.
//        /// </param>
//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                this.unitOfWorkAsync.Dispose();
//                this.authFactory.Dispose();
//            }
//        }

//        /// <summary>
//        /// The refresh token.
//        /// </summary>
//        /// <param name="token">
//        /// The token.
//        /// </param>
//        /// <returns>
//        /// The <see cref="Task"/>.
//        /// </returns>
//        private async Task<bool> RefreshToken(RefreshToken token)
//        {
//            var existingToken = this.repoRefreshTokens.Queryable().SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

//            if (existingToken != null)
//            {
//                var result = await this.repoRefreshTokens.DeleteAsync(existingToken.Id);
//                if (!result)
//                {
//                    return false;
//                }
//            }

//            token = this.repoRefreshTokens.Insert(token);
//            if (token == null)
//            {
//                return false;
//            }

//            var resultSave = await this.unitOfWorkAsync.SaveChangesAsync();
//            return resultSave.IsValid;
//        }
//    }
//}