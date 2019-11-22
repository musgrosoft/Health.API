//using System.Collections.Generic;
//using GoogleSheets;
//using Moq;
//using Repositories.Health.Models;
//using Utils;
//using Xunit;
//
//namespace GoogleSheets.Tests.Unit
//{
//    public class RowMapperTests
//    {
//        [Fact]
//        public void ShouldMapRows()
//        {
//            //Given
//            var logger = new Mock<ILogger>();
//            var rowMapper = new RowMapper(logger.Object);
//
//            var rows = new List<IList<object>>
//            {
//                new List<object> {1D},
//                new List<object> {2D},
//                new List<object> {3D},
//            };
//
//            //When
//            var drinks = rowMapper.Get(rows,MapRowToDrink);
//
//            //Then
//            Assert.Equal(3, drinks.Count);
//            Assert.Contains(drinks, x => x.Units == 1D);
//            Assert.Contains(drinks, x => x.Units == 2D);
//            Assert.Contains(drinks, x => x.Units == 3D);
//
//        }
//
//        private Drink MapRowToDrink(IList<object> row)
//        {
//            return new Drink
//            {
//                //CreatedDate = DateTime.Parse((string)row[0]),
//                Units = (double)row[0]
//            };
//        }
//    }
//}