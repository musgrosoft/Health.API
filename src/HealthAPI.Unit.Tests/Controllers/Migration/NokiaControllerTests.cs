using System.Threading.Tasks;
using HealthAPI.Controllers.Migration;
using Migrators;
using Moq;
using Utils;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Migration
{
    public class NokiaControllerTests
    {
        //[Fact]
        //public async Task ShouldMigrateNokiaData()
        //{
        //    var _logger = new Mock<ILogger>();
        //    var _nokiaMigrator = new Mock<INokiaMigrator>();

        //    var _fitbitController = new NokiaController(_logger.Object, _nokiaMigrator.Object,null,null);

        //    //When
        //    await _fitbitController.Migrate();

        //    //Then
        //    _nokiaMigrator.Verify(x => x.MigrateWeights(), Times.Once);
        //    _nokiaMigrator.Verify(x => x.MigrateBloodPressures(), Times.Once);
        //}
    }
}