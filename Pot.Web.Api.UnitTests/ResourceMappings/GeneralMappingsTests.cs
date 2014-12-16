

namespace Pot.Web.Api.Unit.Tests
{
    using AutoMapper;

    using NUnit.Framework;

    /// <summary>
    /// The general mappings tests.
    /// </summary>
    [TestFixture]
    public class GeneralMappingsTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralMappingsTests"/> class.
        /// </summary>
        public GeneralMappingsTests()
        {
   //         AutomapperConfig.Initialize();
        }

        /// <summary>
        /// When the map is configured then the configuration is correct.
        /// </summary>
        [Test]
        public void MapsConfigured_ConfigurationIsCorrect()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
