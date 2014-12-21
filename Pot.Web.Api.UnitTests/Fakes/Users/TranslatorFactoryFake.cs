// --------------------------------------------------------------------------------------------------------------------
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

    using Pot.Data;
    using Pot.Data.SQLServer;

    /// <summary>
    /// The customer factory mock.
    /// </summary>
    internal class UserFactoryFake : UserFactory, IUserFactory
    {
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

        ///// <summary>
        ///// Gets the get base repository.
        ///// </summary>
        //public override IRepositoryAsync<Translator> GetBaseRepository
        //{
        //    get
        //    {
        //        return new TranslatorRepositoryFake(this.TranslatorContext, new Mock<IAuthRepository>().Object, this.translatorRepositoryMock);
        //    }
        //}

        ///// <summary>
        ///// The get repository async.
        ///// </summary>
        ///// <typeparam name="T">
        ///// The entity
        ///// </typeparam>
        ///// <returns>
        ///// The <see cref="IRepositoryAsync"/>.
        ///// </returns>
        //public override IRepositoryAsync<T> GetRepositoryAsync<T>()
        //{
        //    if (typeof(T) == typeof(TranslatorMotherTongue))
        //    {
        //        if (this.translatorMotherTongueRepository != null)
        //        {
        //            return (IRepositoryAsync<T>)this.translatorMotherTongueRepository;
        //        }
        //    }

        //    return base.GetRepositoryAsync<T>();
        //}



        //GetRepositoryAsync<TranslatorMotherTongue>(),
        public UserFactoryFake(PotDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
