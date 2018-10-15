using System.Collections.Generic;
using ForestSimulation;
using ForestSimulation.ForestObject;
using ForestSimulation.services;
using Moq;
using NUnit.Framework;

namespace ForestSimulationTests
{
    public static class BearTests
    {
        [TestFixture]
        public static class when_tick
        {
            [Test]
            public static void then_should_move_five_times_in_a_random_direction_if_all_spots_empty()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2, 7, 7, 7, 7 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(5);
                var bear = new Bear(0, 0, numberGeneratorService.Object);

                // Act
                bear.Tick(forest.Object);

                // Assert
                Assert.That(bear.Location.X, Is.EqualTo(5));
                Assert.That(bear.Location.Y, Is.EqualTo(5));
            }
        }

        [TestFixture]
        public static class when_bear_ticks_and_moves_into_a_space_with_a_lumberjack
        {
            [Test]
            public static void then_should_stop_moving()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] {2, 7, 7, 7, 7};
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var bear = new Bear(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(5);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1)))
                    .Returns(new List<IForestObject>() {new Mock<ILumberJack>().Object});

                // Act
                bear.Tick(forest.Object);

                // Assert
                Assert.That(bear.Location.X, Is.EqualTo(1));
                Assert.That(bear.Location.Y, Is.EqualTo(1));
            }

            [Test]
            public static void then_if_lumberjack_should_maul()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var bear = new Bear(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                var lumberJack = new LumberJack(0, 0);
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1)))
                    .Returns(new List<IForestObject>() {lumberJack});

                // Act
                bear.Tick(forest.Object);

                // Assert
                forest.Verify(forest1 => forest1.Remove(lumberJack));
            }
            [Test]
            public static void then_if_lumberjack_should_increment_maw_counter()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var bear = new Bear(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                var lumberJack = new LumberJack(0, 0);
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1)))
                    .Returns(new List<IForestObject>() {lumberJack});

                // Act
                bear.Tick(forest.Object);

                // Assert
                forest.VerifySet(forest1 => forest1.Maulings += 1);
            }

        }
    }
}