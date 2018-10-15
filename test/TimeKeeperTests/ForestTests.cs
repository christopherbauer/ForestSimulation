using System.Linq;
using System.Security.Cryptography;
using ForestSimulation;
using NUnit.Framework;

namespace ForestSimulationTests
{
    public static class ForestTests
    {
        [TestFixture]
        public static class when_creating_a_forest_with_initializer_ten
        {
            [Test]
            public static void then_should_create_a_forest_with_y_bound_of_ten()
            {
                // Arrange

                // Act
                var forest = new Forest(10);
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Bound, Is.EqualTo(10));
            }

            [Test]
            public static void then_should_create_a_forest_with_x_bound_of_ten()
            {
                // Arrange
                

                // Act
                var forest = new Forest(10);
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Bound, Is.EqualTo(10));
            }
        }

        [TestFixture]
        public static class when_initialize_forest
        {
            [Test]
            public static void then_should_generate_ten_lumberjacks_for_initializer_ten()
            {
                // Arrange
                var forest = new Forest(10);

                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.LumberJacks.Count, Is.EqualTo(10));
            }

            [Test]
            public static void then_should_generate_a_lumberjack_at_a_random_X_location()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());
                

                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.LumberJacks.First().Location.X, Is.Not.EqualTo(0));
            }
            [Test]
            public static void then_should_generate_a_lumberjack_at_a_random_Y_location()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());
                

                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.LumberJacks.First().Location.Y, Is.Not.EqualTo(0));
            }

            [Test]
            public static void then_should_generate_fifty_trees_for_initializer_ten()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());
                

                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Trees.Count, Is.EqualTo(50));
            }

            [Test]
            public static void then_should_generate_a_tree_at_a_random_X_location()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());


                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Trees.First().Location.X, Is.Not.EqualTo(0));
            }
            [Test]
            public static void then_should_generate_a_tree_at_a_random_Y_location()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());


                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Trees.First().Location.Y, Is.Not.EqualTo(0));
            }

            [Test]
            public static void then_should_generate_two_bears_for_initializer_ten()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());
                

                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Bears.Count, Is.EqualTo(2));
            }

            [Test]
            public static void then_should_generate_a_bear_at_a_random_X_location()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());


                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Bears.First().Location.X, Is.Not.EqualTo(0));
            }
            [Test]
            public static void then_should_generate_a_bear_at_a_random_Y_location()
            {
                // Arrange
                var forest = new Forest(10, new GreaterThanOneGeneratorService());


                // Act
                forest.InitializeForest();

                // Assert
                Assert.That(forest.Bears.First().Location.Y, Is.Not.EqualTo(0));
            }
        }
    }
}