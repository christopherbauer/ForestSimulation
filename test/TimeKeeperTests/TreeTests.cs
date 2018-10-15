using ForestSimulation;
using ForestSimulation.ForestObject;
using ForestSimulation.services;
using Moq;
using NUnit.Framework;

namespace ForestSimulationTests
{
    public static class TreeTests
    {
        [TestFixture]
        public static class when_creating_a_new_tree
        {
            [Test]
            public static void then_should_be_a_sapling()
            {
                // Arrange

                // Act
                var tree = new Tree(0, 0, null);

                // Assert
                Assert.That(tree.Age, Is.EqualTo(TreeAge.Sapling));
            }

            [Test]
            public static void then_should_grow_to_tree_in_thirteen_months()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                numberGeneratorService.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(10);
                var tree = new Tree(0, 0, new Mock<ISaplingGeneratorService>().Object, TreeAge.Sapling, numberGeneratorService.Object);

                // Act
                for (var i = 0; i < 13; i++)
                {
                    tree.Tick(new Mock<IForest>().Object);
                }

                // Assert
                Assert.That(tree.Age, Is.EqualTo(TreeAge.Tree));
            }

            [Test]
            public static void then_should_grow_to_elder_tree_in_one_hundred_twenty_one_months()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                numberGeneratorService.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(10);
                var tree = new Tree(0, 0, new Mock<ISaplingGeneratorService>().Object, TreeAge.Sapling, numberGeneratorService.Object);

                // Act
                for (var i = 0; i < 121; i++)
                {
                    tree.Tick(new Mock<IForest>().Object);
                }

                // Assert
                Assert.That(tree.Age, Is.EqualTo(TreeAge.ElderTree));
            }
        }

        [TestFixture]
        public static class when_creating_a_sapling
        {
            [Test]
            public static void then_should_create_a_sapling_if_spot_is_open_at_zero_zero()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(1);
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(0, 2)).Returns(0);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(0, 0)).Returns(true);
                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.Tree, specificNumberGenerator.Object);

                // Act
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, 0, 0),
                    Times.Once);
            }

            [Test]
            public static void then_should_loop_until_open_spot_is_found_if_zero_zero_is_not_open
                ()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(1);

                int[] specificSet = { 0, 0, 0, 1 };
                var setCalls = 0;
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(0, 2))
                    .Returns(() => specificSet[setCalls]) //first two calls to random in range return 0 and 0, then the next two return 0 and 1 (x-y points)
                    .Callback(() => setCalls++);

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(0, 0)).Returns(false);
                forest.Setup(forest1 => forest1.IsOpenSpot(0, 1)).Returns(true);
                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.Tree, specificNumberGenerator.Object);

                // Act
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, 0, 0),
                    Times.Never);
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, 0, 1),
                    Times.Once);
            }

            [Test]
            public static void then_should_abandon_trying_to_create_a_sapling_after_checking_all_spots()
            {
                // Arrange
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                numberGeneratorService.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(1);
                var tree = new Tree(1, 1, new Mock<ISaplingGeneratorService>().Object, TreeAge.Tree, numberGeneratorService.Object);

                // Act
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.IsOpenSpot(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                tree.Tick(forest.Object);

                // Assert
                forest.Verify(forest1 => forest1.IsOpenSpot(It.IsAny<int>(),It.IsAny<int>()), Times.Exactly(8));
            }
        }

        [TestFixture]
        public static class when_tree_tick_and_is_age_elder_tree
        {
            [Test]
            public static void then_should_create_a_sapling_if_chance_roll_is_one()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(1);
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(0, 2)).Returns(0);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.ElderTree, specificNumberGenerator.Object);

                // Act
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
            }      
            [Test]
            public static void then_should_create_a_sapling_if_chance_roll_is_two()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(1);
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(0, 2)).Returns(0);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.ElderTree, specificNumberGenerator.Object);

                // Act
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, 0, 0),
                    Times.Once);
            }
            [Test]
            public static void then_should_not_create_sapling_if_chance_roll_not_one_or_two()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(10);

                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.ElderTree, specificNumberGenerator.Object);

                // Act
                var forest = new Mock<IForest>();
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, It.IsAny<int>(), It.IsAny<int>()),
                    Times.Never);
            }
        }
        [TestFixture]
        public static class when_tree_tick_and_is_age_tree
        {
            [Test]
            public static void then_should_create_a_sapling_if_chance_roll_is_one()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(1);
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(0, 2)).Returns(0);
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.Bound).Returns(3);
                forest.Setup(forest1 => forest1.IsOpenSpot(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.Tree, specificNumberGenerator.Object);

                // Act
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, It.IsAny<int>(), It.IsAny<int>()),
                    Times.Once);
            }
            [Test]
            public static void then_should_not_create_sapling_if_chance_roll_not_one()
            {
                // Arrange
                var specificNumberGenerator = new Mock<INumberGeneratorService>();
                specificNumberGenerator.Setup(service => service.GetNextRandomInRange(1, 10)).Returns(2);

                var saplingGeneratorService = new Mock<ISaplingGeneratorService>();
                var tree = new Tree(1, 1, saplingGeneratorService.Object, TreeAge.Tree, specificNumberGenerator.Object);

                // Act
                var forest = new Mock<IForest>();
                tree.Tick(forest.Object);

                // Assert
                saplingGeneratorService.Verify(service => service.CreateSapling(forest.Object, It.IsAny<int>(), It.IsAny<int>()),
                    Times.Never);
            }
        }
    }
}