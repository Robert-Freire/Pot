﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslatorFactoryFake.cs" company="Nova Language Services">
//   Copyright © Nova Language Services 2014
// </copyright>
// <summary>
//   The customer factory mock.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Pot.Web.Api.Unit.Tests
{
    using System.Data.Entity;

    using Moq;

    using Pot.Data;
    using Pot.Data.Infraestructure;
    using Pot.Data.Model;
    using Pot.Data.SQLServer;

    /// <summary>
    /// The customer factory mock.
    /// </summary>
    internal class UserFactoryFake : UserFactory, IUserFactory
    {
        private readonly PotDbContext potDbContext;
        private readonly Mock<IRepositoryAsync<User>> userRepositoryMock;

        ///// <summary>
        ///// The customer repository mock.
        ///// </summary>
        //private readonly Mock<ITranslatorRepository> translatorRepositoryMock;

        ///// <summary>
        ///// The translator mother tongue repository.
        ///// </summary>
        //private readonly IRepositoryAsync<TranslatorMotherTongue> translatorMotherTongueRepository;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="TranslatorFactoryFake"/> class. 
        ///// </summary>
        ///// <param name="context">
        ///// The context.
        ///// </param>
        ///// <param name="authContext">
        ///// The authorization Context.
        ///// </param>
        ///// <param name="translatorRepositoryMock">
        ///// The customer repository mock.
        ///// </param>
        ///// <param name="translatorMotherTongueRepository">
        ///// The translator Mother Tongue Repository.
        ///// </param>
        //public TranslatorFactoryFake(
        //    TranslatorContext context,
        //    AuthContext authContext = null,
        //    Mock<ITranslatorRepository> translatorRepositoryMock = null,
        //    IRepositoryAsync<TranslatorMotherTongue> translatorMotherTongueRepository = null)
        //    : base(context, authContext)
        //{
        //    this.translatorRepositoryMock = translatorRepositoryMock;
        //    this.translatorMotherTongueRepository = translatorMotherTongueRepository;
        //}

        public override IRepositoryAsync<User> UsersRepository
        {
            get
            {
                return new UserRepositoryFake(this.potDbContext, this.userRepositoryMock);
            }
        }

        public UserFactoryFake(PotDbContext dbContext, Mock<IRepositoryAsync<User>> userRepositoryMock)
            : base(dbContext)
        {
            this.potDbContext = dbContext;
            this.userRepositoryMock = userRepositoryMock;
        }
    }
}
