//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="ProjectUsersTests.cs" company="Nova Language Services">
////   Copyright © Nova Language Services 2014
//// </copyright>
//// <summary>
////   The translatorMotherTongue.
//// </summary>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Pot.Web.Api.Integration.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Diagnostics;
//    using System.Linq;
//    using System.Threading.Tasks;

//    using Moq;

//    using NUnit.Framework;

//    using Pot.Data.Infraestructure;
//    using Pot.Data.Model;
//    using Pot.Web.Api.Controllers;

//    /// <summary>
//    /// The translatorMotherTongue.
//    /// </summary>
//    [TestFixture]
//    public class ProjectUserTests
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ProjectUserTests"/> class.
//        /// </summary>
//        public ProjectUserTests()
//        {
//            AutomapperConfig.Initialize();
//        }

//        /// <summary>
//        /// The get filtered projectUser when there are two projects then returns two projectUser.
//        /// </summary>
//        [Test]
//        public void GetFilteredProjectUsers_ThereAreTwoMotherTonguesForAProjectUser_ReturnsTwoMotherTongues()
//        {
//            // Arrange
//            var motherTonguesScenario = new DefaultSavedProjectUsers();
//            var projectUsersController = ProjectUsersControllerMockFactory(motherTonguesScenario.ProjectUsersTest);

//            // Action
//            var result = projectUsersController.GetFiltered(motherTonguesScenario.TranslatorTest.IdTranslator).Result;

//            // Assert
//            result.ToList().Count.Should().Be(motherTonguesScenario.TranslatorTest.ProjectUsers.Count);
//        }

//        /// <summary>
//        /// The get translatorMotherTongue when there are no projectUsers then returns not found.
//        /// </summary>
//        [Test]
//        public void GetProjectUser_ThereAreNoProjectUsers_ReturnsNotFound()
//        {
//            // Arrange
//            var motherTongues = new List<ProjectUser>().AsQueryable();
//            var projectUsersController = ProjectUsersControllerMockFactory(motherTongues);

//            // Action
//            var result = projectUsersController.Get(Guid.NewGuid(), Guid.NewGuid()).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.NotFoundResult));
//        }

//        /// <summary>
//        /// The get translatorMotherTongue when there are a translatorMotherTongue then returns translatorMotherTongue.
//        /// </summary>
//        [Test]
//        public void GetProjectUser_ThereAreProjectUser_ReturnProjectUser()
//        {
//            var motherTonguesScenario = new DefaultSavedProjectUsers();
//            var projectUsersController = ProjectUsersControllerMockFactory(motherTonguesScenario.ProjectUsersTest);

//            // Action
//            var result = projectUsersController.Get(motherTonguesScenario.TranslatorTest.IdTranslator, motherTonguesScenario.LanguageSpanish.IdLanguage).Result;

//            // Assert
//            result.ShouldBeEquivalent(motherTonguesScenario.ProjectUsersTest.FirstOrDefault(t => t.IdLanguage == motherTonguesScenario.LanguageSpanish.IdLanguage));
//        }

//        /// <summary>
//        /// The delete translatorMotherTongue if the translatorMotherTongue exists then returns ok.
//        /// </summary>
//        [Test]
//        public void DeleteProjectUser_ProjectUserExists_ReturnsOk()
//        {
//            // Arrange
//            var motherTonguesScenario = new DefaultSavedProjectUsers();
//            var translatorMotherTongueController = ProjectUsersControllerMockFactory(motherTonguesScenario.ProjectUsersTest, motherTonguesScenario.ProjectUsersTest.FirstOrDefault());
//            var motherTongueToDelete = motherTonguesScenario.ProjectUsersTest.FirstOrDefault();

//            // Action
//            Debug.Assert(motherTongueToDelete != null, "motherTongueToDelete != null");
//            var result = translatorMotherTongueController.Delete(motherTongueToDelete.IdTranslator, motherTongueToDelete.IdLanguage).Result;

//            // Assert
//            result.ShouldBeEquivalent(motherTongueToDelete);
//        }

//        /// <summary>
//        /// The delete translatorMotherTongue if the translatorMotherTongue not exists then returns error.
//        /// </summary>
//        [Test]
//        public void DeleteProjectUser_ProjectUserDoNotExists_ReturnsError()
//        {
//            // Arrange
//            var projectUsers = new List<ProjectUser>().AsQueryable();
//            var translatorMotherTongueController = ProjectUsersControllerMockFactory(projectUsers);

//            // Action
//            var result = translatorMotherTongueController.Delete(Guid.NewGuid(), Guid.NewGuid()).Result;

//            // Assert
//            result.Should().BeOfType(typeof(System.Web.Http.Results.NotFoundResult));
//        }

//        /// <summary>
//        /// The translator mother tongues controller mock factory.
//        /// </summary>
//        /// <param name="projectUsers">
//        /// The translator mother tongues.
//        /// </param>
//        /// <param name="translatorMotherTongueFound">
//        /// The translator mother tongue found.
//        /// </param>
//        /// <returns>
//        /// The <see cref="ProjectUsersController"/>.
//        /// </returns>
//        private static ProjectUserController ProjectUsersControllerMockFactory(
//            IEnumerable<ProjectUser> projectUsers,
//            ProjectUser translatorMotherTongueFound = null)
//        {
//            var translatorContextMock = new TranslatorContextFake();
//            translatorContextMock.SetupSets(motherTongues: projectUsers.AsQueryable());
//            var translatorRepositoryMock = new Mock<IRepositoryAsync<ProjectUser>>();
//            translatorRepositoryMock.Setup(cr => cr.FindAsync(It.IsAny<object>())).Returns(Task.FromResult(translatorMotherTongueFound));
//            translatorRepositoryMock.Setup(cr => cr.Queryable()).Returns(translatorContextMock.Object.Set<ProjectUser>());

//            var translatorMotherTongueRepositoryMock = new ProjectUsersRepositoryFake(
//                translatorContextMock.Object,
//                translatorRepositoryMock);

//            return new ProjectUserController( new TranslatorFactoryFake(translatorContextMock.Object, translatorMotherTongueRepository: translatorMotherTongueRepositoryMock));
//        }

//        /// <summary>
//        /// The default saved mother tongues.
//        /// </summary>
//        private class DefaultSavedProjectUsers
//        {
//            /// <summary>
//            /// The translator test.
//            /// </summary>
//            private Project projectTest;

//            /// <summary>
//            /// One user of the project
//            /// </summary>
//            private User userTestA;

//            /// <summary>
//            /// Another user of the project
//            /// </summary>
//            private User userTestB;

//            /// <summary>
//            /// The translator mother tongues.
//            /// </summary>
//            private List<ProjectUser> projectUsers;

//            /// <summary>
//            /// Gets the first user for the tests
//            /// </summary>
//            internal User UserTestA
//            {
//                get
//                {
//                    return this.userTestA
//                           ?? (this.userTestA = new User { UserId = Guid.NewGuid(), UserName = "First user" });
//                }
//            }

//            /// <summary>
//            /// Gets the language catalan.
//            /// </summary>
//            internal User UserTestB
//            {
//                get
//                {
//                    return this.userTestB
//                           ?? (this.userTestB = new User { UserId = Guid.NewGuid(), UserName = "Second user" });
//                }
//            }

//            /// <summary>
//            /// Gets the translator test.
//            /// </summary>
//            internal Project ProjectTest
//            {
//                get
//                {
//                    if (this.projectTest == null)
//                    {
//                        this.projectTest = new Project { ProjectId = Guid.NewGuid(), Name = "Project"};

//                        this.projectTest.ProjectUsers.Add(
//                            new ProjectUser
//                            {
//                                ProjectId = this.ProjectTest.ProjectId,
//                                UserId = this.UserTestA.UserId
//                            });
//                        this.projectTest.ProjectUsers.Add(
//                            new ProjectUser
//                            {
//                                ProjectId = this.ProjectTest.ProjectId,
//                                UserId = this.UserTestB.UserId
//                            });
//                    }

//                    return this.projectTest;
//                }
//            }

//            /// <summary>
//            /// Gets the translator mother tongues test.
//            /// </summary>
//            internal IEnumerable<ProjectUser> ProjectUsersTest
//            {
//                get
//                {
//                    if (this.projectUsers == null)
//                    {
//                        //this.projectUsers = new List<ProjectUser>
//                        //                    {
//                        //                        new ProjectUser
//                        //                        {
//                        //                            ProjectId = this.ProjectTest.ProjectId,
//                        //                            UserId = this.UserTestA.UserId
//                        //                        }
//                        //                    };
//                        this.projectUsers = new List<ProjectUser>();
//                        this.projectUsers.AddRange(this.ProjectTest.ProjectUsers);
//                    }

//                    return this.projectUsers;
//                }
//            }
//        }
//    }
//}
