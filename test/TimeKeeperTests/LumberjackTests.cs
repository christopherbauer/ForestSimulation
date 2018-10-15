using System.Collections.Generic;
using ForestSimulation;
using ForestSimulation.ForestObject;
using ForestSimulation.services;
using Moq;
using NUnit.Framework;

namespace ForestSimulationTests
{
    public static class LumberjackTests
    {
        [TestFixture]
        public static class when_lumberjack_tick
        {
            [Test]
            public static void then_should_move_three_times_in_a_random_direction_if_all_spots_empty()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] {2, 7, 7};
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                var lumberjack = new LumberJack(0,0, numberGeneratorService.Object);

                // Act
                lumberjack.Tick(forest.Object);

                // Assert
                Assert.That(lumberjack.Location.X, Is.EqualTo(3));
                Assert.That(lumberjack.Location.Y, Is.EqualTo(3));
            }
        }

        [TestFixture]
        public static class when_lumberjack_ticks_and_moves_into_a_space_with_a_tree
        {
            [Test]
            public static void then_should_stop_moving()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2, 7, 7 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var lumberjack = new LumberJack(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1))).Returns(
                    new List<IForestObject>()
                    {
                        new Tree(1, 1, new Mock<ISaplingGeneratorService>().Object) {Age = TreeAge.Tree}
                    });

                // Act
                lumberjack.Tick(forest.Object);
                    
                // Assert
                Assert.That(lumberjack.Location.X, Is.EqualTo(1));
                Assert.That(lumberjack.Location.Y, Is.EqualTo(1));

            }
            [Test]
            public static void then_if_tree_should_cut_down_the_tree()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var lumberjack = new LumberJack(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                var tree = new Tree(1, 1, new Mock<ISaplingGeneratorService>().Object) { Age = TreeAge.Tree };
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1))).Returns(new List<IForestObject>() {tree});

                // Act
                lumberjack.Tick(forest.Object);

                // Assert
                forest.Verify(forest1 => forest1.Remove(tree));
            }
            [Test]
            public static void then_if_sapling_should_not_cut_down_the_tree()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var lumberjack = new LumberJack(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                var tree = new Tree(1, 1, new Mock<ISaplingGeneratorService>().Object) { Age = TreeAge.Sapling };
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1))).Returns(new List<IForestObject>() {tree});

                // Act
                lumberjack.Tick(forest.Object);

                // Assert
                forest.Verify(forest1 => forest1.Remove(tree), Times.Never);
            }
            [Test]
            public static void then_if_tree_should_add_one_lumber_to_forest_lumber()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var lumberjack = new LumberJack(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                var tree = new Tree(1, 1, new Mock<ISaplingGeneratorService>().Object) { Age = TreeAge.Tree };
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1))).Returns(new List<IForestObject> {tree});

                // Act
                lumberjack.Tick(forest.Object);

                // Assert
                forest.VerifySet(forest1 => forest1.Lumber += 1);
            }
            [Test]
            public static void then_if_elder_tree_should_add_two_lumber_to_forest_lumber()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                var specificSet = new[] { 2 };
                var calls = 0;
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(() => specificSet[calls]).Callback(() => calls++);
                var lumberjack = new LumberJack(0, 0, numberGeneratorService.Object);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(1, 1)).Returns(false);
                forest.Setup(forest1 => forest1.GridContents(new Point(0, 0))).Returns((List<IForestObject>) null);
                var tree = new Tree(1, 1, new Mock<ISaplingGeneratorService>().Object) { Age = TreeAge.ElderTree };
                forest.Setup(forest1 => forest1.GridContents(new Point(1, 1))).Returns(new List<IForestObject>() {tree});

                // Act
                lumberjack.Tick(forest.Object);

                // Assert
                forest.VerifySet(forest1 => forest1.Lumber += 2);
            }
        }
    }
}